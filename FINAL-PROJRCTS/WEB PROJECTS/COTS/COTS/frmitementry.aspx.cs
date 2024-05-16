using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Reflection;

namespace COTS
{
    public partial class frmitementry : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        if (Request["qno"] != null && Utilities.Decrypt(Request["qno"].ToString()) != "")
                        {
                            hdnVirtualStr.Value = ConfigurationManager.AppSettings["virtualstr"];
                            lblOrderRefNo.Text = Utilities.Decrypt(Request["qno"].ToString());
                            lblCurType.Text = Session["cotscur"].ToString().ToUpper();
                            hdnCustCode.Value = Session["cotscode"].ToString();
                            Bind_GV_All_Grade("Solid", gvSolid);
                            Bind_GV_All_Grade("Pob", gvPob);
                        }
                        else
                            Response.Redirect("frmmsgdisplay.aspx?msgtype=sheetmsg", false);
                    }
                }
                else
                    Response.Redirect("SessionExp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        ConvertToJSON();
                        if (Request["qtype"] != null && Utilities.Decrypt(Request["qtype"].ToString()) != "")
                            hdnQType.Value = Utilities.Decrypt(Request["qtype"].ToString());
                    }
                }
                else
                    Response.Redirect("SessionExp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private DataTable Get_PriceSheet(string category)
        {
            DataTable dtSheet = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@category", category);
                sp1[2] = new SqlParameter("@custstdcode", Session["cotsstdcode"].ToString());
                dtSheet = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_PublishedPriceSheet", sp1, DataAccess.Return_Type.DataTable);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dtSheet;
        }
        private void Bind_GV_All_Grade(string _category, GridView ctrl_GV)
        {
            try
            {
                DataTable dtSheet = Get_PriceSheet(_category);
                if (dtSheet.Rows.Count > 0)
                {
                    SqlParameter[] sp3 = new SqlParameter[] { new SqlParameter("@CustCode", Session["cotscode"].ToString()) };
                    DataTable dtSelDis = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_TypeWiseDiscount", sp3, DataAccess.Return_Type.DataTable);

                    DataTable dtMain = new DataTable();
                    dtMain.Columns.Add("TotFinishedWT", typeof(System.String));
                    dtMain.Columns.Add("TotPcs", typeof(System.String));
                    dtMain.Columns.Add("TyreSize", typeof(System.String));
                    dtMain.Columns.Add("RimSize", typeof(System.String));

                    DataView dtTypeView = new DataView(dtSheet);
                    dtTypeView.Sort = "TypePosition ASC,TyreType ASC,Brand ASC,Sidewall ASC,Config ASC";
                    DataTable distinctType = dtTypeView.ToTable(true, "TypePosition", "TyreType", "Brand", "Sidewall", "Config");
                    foreach (DataRow rowType in distinctType.Rows)
                    {
                        DataColumn dmaincol = new DataColumn(rowType["Config"].ToString().Replace(" ", "~") + "_" + rowType["Brand"].ToString().Replace(" ", "~") +
                            "_" + rowType["Sidewall"].ToString().Replace(" ", "~") + "_" + rowType["TyreType"].ToString().Replace(" ", "~"), typeof(System.String));
                        dtMain.Columns.Add(dmaincol);
                    }
                    dtMain.Rows.Add(dtMain.NewRow());

                    DataView dtSizeView = new DataView(dtSheet);
                    dtSizeView.Sort = "SizePosition ASC";
                    DataTable distinctSize = dtSizeView.ToTable(true, "SizePosition", "TyreSize", "RimSize");
                    foreach (DataRow row in distinctSize.Rows)
                    {
                        DataRow mainRow = dtMain.NewRow();
                        foreach (DataColumn priceCol in dtMain.Columns)
                        {
                            string strHead = priceCol.ColumnName;
                            if (strHead == "TotFinishedWT")
                                mainRow["TotFinishedWT"] = "";
                            else if (strHead == "TotPcs")
                                mainRow["TotPcs"] = "";
                            else if (strHead == "TyreSize")
                                mainRow["TyreSize"] = row["TyreSize"].ToString();
                            else if (strHead == "RimSize")
                                mainRow["RimSize"] = row["RimSize"].ToString();
                            else
                            {
                                string[] splitHead = strHead.Replace('~', ' ').Split('_');
                                string unitprice = "0"; decimal disVal = 0;
                                foreach (DataRow disRow in dtSelDis.Select("Config='" + splitHead[0].ToString() + "' and Brand='" + splitHead[1].ToString() + "' and TyreType='" + splitHead[3].ToString() + "' and Sidewall='" + splitHead[2].ToString() + "'"))
                                {
                                    disVal = Convert.ToDecimal(disRow["Discount"].ToString());
                                }
                                foreach (DataRow priceRow in dtSheet.Select("Config='" + splitHead[0].ToString() + "' and TyreSize='" + row["TyreSize"].ToString() + "' and RimSize='" + row["RimSize"].ToString() + "' and TyreType='" + splitHead[3].ToString() + "' and Brand='" + splitHead[1].ToString() + "' and Sidewall='" + splitHead[2].ToString() + "'"))
                                {
                                    if (disVal != 0)
                                        unitprice = ((Convert.ToDecimal(priceRow["UnitPrice"].ToString()) - (Convert.ToDecimal(priceRow["UnitPrice"].ToString()) * (disVal / 100)))).ToString();
                                    else
                                        unitprice = (Convert.ToDecimal(priceRow["UnitPrice"])).ToString();
                                }
                                mainRow["" + priceCol.ColumnName + ""] = Session["cotscur"].ToString().ToLower() == "inr" ? Convert.ToDecimal(unitprice).ToString("0") : Convert.ToDecimal(unitprice).ToString();
                            }
                        }
                        dtMain.Rows.Add(mainRow);
                    }

                    DataTable dtTypeDesc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Cots_typedescription", DataAccess.Return_Type.DataTable);
                    foreach (DataColumn col in dtMain.Columns)
                    {
                        string strHead = col.ColumnName; string strDesc = string.Empty;
                        if (strHead != "TotFinishedWT" && strHead != "TotPcs" && strHead != "TyreSize" && strHead != "RimSize")
                        {
                            string[] splitHead = strHead.Replace('~', ' ').Split('_');
                            foreach (DataRow row in dtTypeDesc.Select("TyreType='" + splitHead[3].ToString() + "'"))
                            {
                                strDesc = row["TypeDesc"].ToString();
                            }
                        }
                        TemplateField bfield = new TemplateField();
                        bfield.HeaderTemplate = new GridViewAllTemplate(ListItemType.Header, col.ColumnName, strDesc);
                        bfield.ItemTemplate = new GridViewAllTemplate(ListItemType.Item, col.ColumnName, strDesc);
                        ctrl_GV.Columns.Add(bfield);
                    }

                    ctrl_GV.DataSource = dtMain;
                    ctrl_GV.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "display_divs('block','" + _category.ToLower() + "');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript2", "display_divs('none','" + _category.ToLower() + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvSolid_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                    create_Each_Header("Solid", gvSolid);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvPob_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                    create_Each_Header("Pob", gvPob);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void create_Each_Header(string strCategory, GridView ctrlGV)
        {
            try
            {
                DataTable dtSheet = Get_PriceSheet(strCategory);

                DataView dtTypeView = new DataView(dtSheet);
                dtTypeView.Sort = "TypePosition ASC,TyreType ASC,Brand ASC,Sidewall ASC,Config ASC";
                DataTable distinctType = dtTypeView.ToTable(true, "TypePosition", "TyreType", "Brand", "Sidewall", "Config");

                GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                ctrlGV.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);
                GridViewRow gvHeaderRowCopy1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                ctrlGV.Controls[0].Controls.AddAt(1, gvHeaderRowCopy1);
                GridViewRow gvHeaderRowCopy2 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                ctrlGV.Controls[0].Controls.AddAt(2, gvHeaderRowCopy2);

                TableCell tcMergeProduct = new TableCell();
                tcMergeProduct.ColumnSpan = 4;
                tcMergeProduct.Attributes.Add("class", "mergecss configCss");
                gvHeaderRowCopy.Cells.AddAt(0, tcMergeProduct);

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = strCategory.ToUpper();
                tcMergeProduct.ColumnSpan = 4;
                tcMergeProduct.Attributes.Add("class", "mergecss configCss");
                gvHeaderRowCopy1.Cells.AddAt(0, tcMergeProduct);

                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@category", strCategory);
                string strLastOrderDate = (string)daCOTS.ExecuteScalar_SP("sp_sel_LastOrderDate", sp1);

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "Last ordered on : " + (strLastOrderDate != null ? strLastOrderDate : "NA");
                tcMergeProduct.ColumnSpan = 4;
                tcMergeProduct.Attributes.Add("class", "mergecss configCss");
                gvHeaderRowCopy2.Cells.AddAt(0, tcMergeProduct);

                int j = 1;
                foreach (DataRow row in distinctType.Rows)
                {
                    TableCell tcConfigProduct = new TableCell();
                    tcConfigProduct.Text = row["Config"].ToString();
                    tcConfigProduct.Attributes.Add("class", "mergecss configCss");
                    gvHeaderRowCopy.Cells.AddAt(j, tcConfigProduct);

                    TableCell tcBrandProduct = new TableCell();
                    tcBrandProduct.Text = row["Brand"].ToString();
                    tcBrandProduct.Attributes.Add("class", "mergecss configCss");
                    gvHeaderRowCopy1.Cells.AddAt(j, tcBrandProduct);

                    TableCell tcSidewallProduct = new TableCell();
                    tcSidewallProduct.Text = row["Sidewall"].ToString();
                    tcSidewallProduct.Attributes.Add("class", "mergecss configCss");
                    gvHeaderRowCopy2.Cells.AddAt(j, tcSidewallProduct);
                    j++;
                }
                gvHeaderRowCopy.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmproforma.aspx?qcomplete=" + Utilities.Encrypt(lblOrderRefNo.Text), false);
        }

        #region[make price json]
        private void ConvertToJSON()
        {
            JavaScriptSerializer jss1 = new JavaScriptSerializer();
            string _myJSONSolidstring = jss1.Serialize(CreatePriceJson_WebMethod("Solid", Session["cotscode"].ToString()));
            string player = "var priceJsonSolid=" + _myJSONSolidstring + ";";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "player123", player, true);

            string _myJSONPobstring = jss1.Serialize(CreatePriceJson_WebMethod("Pob", Session["cotscode"].ToString()));
            string player1 = "var priceJsonPob=" + _myJSONPobstring + ";";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "player456", player1, true);
        }
        private static Dictionary<string, object> CreatePriceJson_WebMethod(string strCategory, string strCustID)
        {
            Dictionary<string, object> objMainList = new Dictionary<string, object>();
            DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
            SqlParameter[] sp1 = new SqlParameter[3];
            sp1[0] = new SqlParameter("@custcode", HttpContext.Current.Session["cotscode"].ToString());
            sp1[1] = new SqlParameter("@category", strCategory);
            sp1[2] = new SqlParameter("@custstdcode", HttpContext.Current.Session["cotsstdcode"].ToString());
            DataTable dtSheet = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_PublishedPriceSheet", sp1, DataAccess.Return_Type.DataTable);

            if (dtSheet.Rows.Count > 0)
            {
                SqlParameter[] sp3 = new SqlParameter[] { new SqlParameter("@CustCode", strCustID) };
                DataTable dtSelDis = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_TypeWiseDiscount", sp3, DataAccess.Return_Type.DataTable);

                Dictionary<string, object> objBrand = new Dictionary<string, object>();
                Dictionary<string, object> objSidewall = new Dictionary<string, object>();
                Dictionary<string, object> objType = new Dictionary<string, object>();
                Dictionary<string, object> objTyreSize = new Dictionary<string, object>();

                DataView dtConfigView = new DataView(dtSheet);
                dtConfigView.Sort = "Config ASC";
                DataTable distinctConfig = dtConfigView.ToTable(true, "Config");

                DataView dtBrandView = new DataView(dtSheet);
                dtBrandView.Sort = "Config ASC,Brand ASC";
                DataTable distinctBrand = dtBrandView.ToTable(true, "Config", "Brand");

                DataView dtSidewallView = new DataView(dtSheet);
                dtSidewallView.Sort = "Config ASC,Brand ASC,Sidewall ASC";
                DataTable distinctSidewall = dtSidewallView.ToTable(true, "Config", "Brand", "Sidewall");

                DataView dtTypeView = new DataView(dtSheet);
                dtTypeView.Sort = "Config ASC,Brand ASC,Sidewall ASC,TyreType ASC";
                DataTable distinctType = dtTypeView.ToTable(true, "Config", "Brand", "Sidewall", "TyreType");

                string _Config = string.Empty; string _Brand = string.Empty; string _Sidewall = string.Empty; string _TyreType = string.Empty;
                List<priceJson> priceList = new List<priceJson>();
                priceJson pj1 = new priceJson();
                foreach (DataRow drConfig in distinctConfig.Rows)
                {
                    _Config = drConfig["Config"].ToString();
                    objBrand = new Dictionary<string, object>();
                    foreach (DataRow drBrand in distinctBrand.Select("Config='" + _Config + "'"))
                    {
                        _Brand = drBrand["Brand"].ToString();
                        objSidewall = new Dictionary<string, object>();
                        foreach (DataRow drSidewall in distinctSidewall.Select("Config='" + _Config + "' AND Brand='" + _Brand + "'"))
                        {
                            _Sidewall = drSidewall["Sidewall"].ToString();
                            objType = new Dictionary<string, object>();
                            foreach (DataRow drType in distinctType.Select("Config='" + _Config + "' AND Brand='" + _Brand + "' AND Sidewall='" + _Sidewall + "'"))
                            {
                                _TyreType = drType["TyreType"].ToString();
                                decimal disVal = 0;
                                foreach (DataRow row in dtSelDis.Select("Config='" + _Config + "' and Brand='" + _Brand + "' and TyreType='" + _TyreType + "' and Sidewall='" + _Sidewall + "'"))
                                {
                                    disVal = Convert.ToDecimal(row["Discount"].ToString());
                                }
                                priceList = new List<priceJson>();
                                foreach (DataRow drAll in dtSheet.Select("Config='" + _Config + "' AND Brand='" + _Brand + "' AND Sidewall='" + _Sidewall + "' AND TyreType='" + _TyreType + "'"))
                                {
                                    decimal unitprice = 0;
                                    pj1 = new priceJson();
                                    pj1.size = drAll["TyreSize"].ToString().Replace(" ", "~");
                                    pj1.rim = drAll["RimSize"].ToString().Replace(" ", "~");
                                    if (disVal != 0)
                                        unitprice = (Convert.ToDecimal(drAll["UnitPrice"].ToString()) - (Convert.ToDecimal(drAll["UnitPrice"].ToString()) * (disVal / 100)));
                                    else
                                        unitprice = (Convert.ToDecimal(drAll["UnitPrice"].ToString()));

                                    pj1.price = Convert.ToDecimal(unitprice.ToString("0"));
                                    pj1.finishedwt = Convert.ToDecimal(drAll["FinishedWt"].ToString());
                                    priceList.Add(pj1);
                                }
                                objType.Add(_TyreType.Replace(" ", "~"), priceList);
                            }
                            objSidewall.Add(_Sidewall.Replace(" ", "~"), objType);
                        }
                        objBrand.Add(_Brand.Replace(" ", "~"), objSidewall);
                    }
                    objMainList.Add(_Config.Replace(" ", "~"), objBrand);
                }
            }
            return objMainList;
        }
        private class priceJson
        {
            public string size { get; set; }
            public string rim { get; set; }
            public decimal price { get; set; }
            public decimal finishedwt { get; set; }
        }
        #endregion
        #region[get Null Data From DB WebMethod]
        [WebMethod]
        public static string getNullDataFromDB_WebMethod(string strCustCode, string strcategory, string strStdCustCode)
        {
            return getNullvalue_FromDB(strCustCode.ToString(), strcategory.ToString(), strStdCustCode.ToString()).GetXml();
        }
        private static DataSet getNullvalue_FromDB(string strCustCode, string strcategory, string strStdCustCode)
        {
            DataAccess daCOTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@category", strcategory);
                sp1[1] = new SqlParameter("@CustCode", strCustCode);
                sp1[2] = new SqlParameter("@custStdCode", strStdCustCode);
                ds = (DataSet)daCOTSWebMethod.ExecuteReader_SP("sp_sel_nullvalue_publishedsheet", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "frmitementry.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion
        #region[get Incomplete Order  WebMethod]
        [WebMethod]
        public static string getIncompleteItems_WebMethod(string strcsutcode, string strcategory, string orderno)
        {
            return getIncompleteOrderItems_FromDB(strcsutcode.ToString(), strcategory.ToString(), orderno.ToString()).GetXml();
        }
        private static DataSet getIncompleteOrderItems_FromDB(string strcsutcode, string strcategory, string strorderno)
        {
            DataAccess daCOTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custocde", strcsutcode);
                sp1[1] = new SqlParameter("@category", strcategory);
                sp1[2] = new SqlParameter("@orderrefno", strorderno);
                ds = (DataSet)daCOTSWebMethod.ExecuteReader_SP("sp_sel_incomplete_orderitems", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "frmitementry.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion
    }
}