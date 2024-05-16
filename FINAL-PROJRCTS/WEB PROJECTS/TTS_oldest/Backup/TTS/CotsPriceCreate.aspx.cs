using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace TTS
{
    public partial class CotsPriceCreate : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_usermaster"].ToString() == "True")
                        {
                            DataTable dt = (DataTable)daTTS.ExecuteReader("select distinct Sizecategory from ApprovedTyreList where CustCode='DE0048' order by SizeCategory",
                                DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                ddl_Category.DataSource = dt;
                                ddl_Category.DataTextField = "Sizecategory";
                                ddl_Category.DataValueField = "Sizecategory";
                                ddl_Category.DataBind();
                                ddl_Category.Items.Insert(0, "Choose");
                                if (ddl_Category.Items.Count == 2)
                                {
                                    ddl_Category.SelectedIndex = 1;
                                    ddl_Category_SelectedIndexChanged(sender, e);
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                }
                else
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddl_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                gvPriceDetails.DataSource = "";
                gvPriceDetails.DataBind();
                chk_TypeSelection.DataSource = "";
                chk_TypeSelection.DataBind();
                ddl_Sidewall.DataSource = "";
                ddl_Sidewall.DataBind();
                ddl_Brand.DataSource = "";
                ddl_Brand.DataBind();
                ddl_Platform.DataSource = "";
                ddl_Platform.DataBind();
                ddlPriceSheetSelection.DataSource = "";
                ddlPriceSheetSelection.DataBind();

                if (ddl_Category.SelectedIndex > 0)
                {
                    SqlParameter[] spI = new SqlParameter[] { new SqlParameter("@category", ddl_Category.SelectedItem.Text) };
                    DataTable dtPrice = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pricesheetrefno", spI, DataAccess.Return_Type.DataTable);
                    if (dtPrice.Rows.Count > 0)
                    {
                        ddlPriceSheetSelection.DataSource = dtPrice;
                        ddlPriceSheetSelection.DataTextField = "PriceSheetRefNo";
                        ddlPriceSheetSelection.DataValueField = "PriceSheetRefNo";
                        ddlPriceSheetSelection.DataBind();
                        ddlPriceSheetSelection.Items.Insert(dtPrice.Rows.Count, "ADD NEW SHEET");
                        ddlPriceSheetSelection.Items.Insert(0, "Choose");
                    }
                    else
                    {
                        ddlPriceSheetSelection.Items.Insert(0, "Choose");
                        ddlPriceSheetSelection.Items.Insert(1, "ADD NEW SHEET");
                    }
                    DataTable dt = (DataTable)daTTS.ExecuteReader("select distinct Config from ApprovedTyreList where CustCode='DE0048' and " +
                        "SizeCategory='" + ddl_Category.SelectedItem.Text + "' order by Config", DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddl_Platform.DataSource = dt;
                        ddl_Platform.DataTextField = "Config";
                        ddl_Platform.DataValueField = "Config";
                        ddl_Platform.DataBind();
                        ddl_Platform.Items.Insert(0, "Choose");
                        if (ddl_Platform.Items.Count == 2)
                        {
                            ddl_Platform.SelectedIndex = 1;
                            ddl_Platform_SelectedIndexChanged(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddl_Platform_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                gvPriceDetails.DataSource = "";
                gvPriceDetails.DataBind();
                chk_TypeSelection.DataSource = "";
                chk_TypeSelection.DataBind();
                ddl_Sidewall.DataSource = "";
                ddl_Sidewall.DataBind();
                ddl_Brand.DataSource = "";
                ddl_Brand.DataBind();
                if (ddl_Platform.SelectedIndex > 0)
                {
                    DataTable dt = (DataTable)daTTS.ExecuteReader("select distinct brand from ApprovedTyreList where CustCode='DE0048' and SizeCategory='" +
                        ddl_Category.SelectedItem.Text + "' and Config='" + ddl_Platform.SelectedItem.Text + "' order by brand", DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddl_Brand.DataSource = dt;
                        ddl_Brand.DataTextField = "brand";
                        ddl_Brand.DataValueField = "brand";
                        ddl_Brand.DataBind();
                        if (dt.Rows.Count == 1)
                            ddl_Brand_SelectedIndexChanged(sender, e);
                        else
                            ddl_Brand.Items.Insert(0, "Choose");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddl_Brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                gvPriceDetails.DataSource = "";
                gvPriceDetails.DataBind();
                chk_TypeSelection.DataSource = "";
                chk_TypeSelection.DataBind();
                ddl_Sidewall.DataSource = "";
                ddl_Sidewall.DataBind();
                if (ddl_Brand.SelectedIndex > 0)
                {
                    DataTable dt = (DataTable)daTTS.ExecuteReader("select distinct Sidewall from ApprovedTyreList where CustCode='DE0048' and " +
                        " SizeCategory='" + ddl_Category.SelectedItem.Text + "' and Config='" + ddl_Platform.SelectedItem.Text + "' and brand='" +
                        ddl_Brand.SelectedItem.Text + "' order by Sidewall", DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddl_Sidewall.DataSource = dt;
                        ddl_Sidewall.DataTextField = "Sidewall";
                        ddl_Sidewall.DataValueField = "Sidewall";
                        ddl_Sidewall.DataBind();
                        ddl_Sidewall.Items.Insert(0, "Choose");
                        if (ddl_Sidewall.Items.Count == 2)
                        {
                            ddl_Sidewall.SelectedIndex = 1;
                            ddl_Sidewall_SelectedIndexChanged(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddl_Sidewall_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                gvPriceDetails.DataSource = "";
                gvPriceDetails.DataBind();
                chk_TypeSelection.DataSource = "";
                chk_TypeSelection.DataBind();
                if (ddl_Sidewall.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                        new SqlParameter("@Brand", ddl_Brand.SelectedItem.Text), 
                        new SqlParameter("@SideWall", ddl_Sidewall.SelectedItem.Text), 
                        new SqlParameter("@Category", ddl_Category.SelectedItem.Text), 
                    };
                    DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Type_ProcessIdDetails", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        chk_TypeSelection.DataSource = dt;
                        chk_TypeSelection.DataTextField = "TyreType";
                        chk_TypeSelection.DataValueField = "TyreType";
                        chk_TypeSelection.DataBind();

                        if (dt.Rows.Count == 1)
                        {
                            chk_TypeSelection.Items[0].Selected = true;
                            btnView_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                gvPriceDetails.DataSource = "";
                gvPriceDetails.DataBind();
                string strTyreType = null;
                //Create table with default columns
                DataTable dtMain = new DataTable();
                DataColumn dcMain = new DataColumn("TyreSize", typeof(System.String));
                dtMain.Columns.Add(dcMain);
                dcMain = new DataColumn("RimSize", typeof(System.String));
                dtMain.Columns.Add(dcMain);
                //Add columns based on tyretype
                strTyreType = "";
                foreach (ListItem item in chk_TypeSelection.Items)
                {
                    if (item.Selected)
                    {
                        strTyreType += "'" + item.ToString() + "',";
                        dcMain = new DataColumn(item.ToString(), typeof(System.String));
                        dtMain.Columns.Add(dcMain);
                    }
                }
                strTyreType = strTyreType.Remove(strTyreType.Length - 1);

                //Get Filtered records for null check
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@SizeCategory", ddl_Category.SelectedItem.Text), 
                    new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                    new SqlParameter("@Brand", ddl_Brand.SelectedItem.Text), 
                    new SqlParameter("@Sidewall", ddl_Sidewall.SelectedItem.Text), 
                    new SqlParameter("@TyreType", strTyreType) 
                };
                DataTable dtnullRecords = (DataTable)daTTS.ExecuteReader_SP("sp_sel_null_ProcessID_Details", sp1, DataAccess.Return_Type.DataTable);

                //Get Filtered records for null check
                SqlParameter[] sp2 = new SqlParameter[] { 
                    new SqlParameter("@SizeCategory", ddl_Category.SelectedItem.Text), 
                    new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                    new SqlParameter("@Brand", ddl_Brand.SelectedItem.Text), 
                    new SqlParameter("@Sidewall", ddl_Sidewall.SelectedItem.Text), 
                    new SqlParameter("@TyreType", strTyreType),
                    new SqlParameter("@PriceSheetRefNo", ddlPriceSheetSelection.SelectedItem.Text == "ADD NEW SHEET" ? txtPriceSheet.Text : ddlPriceSheetSelection.SelectedItem.Text )
                };
                DataTable dtPricesheet = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_price_PriceSheetDetails", sp2, DataAccess.Return_Type.DataTable);

                //Get Filtered Records
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@SizeCategory", ddl_Category.SelectedItem.Text), 
                    new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                    new SqlParameter("@Brand", ddl_Brand.SelectedItem.Text), 
                    new SqlParameter("@Sidewall", ddl_Sidewall.SelectedItem.Text), 
                    new SqlParameter("@TyreType", strTyreType) 
                };
                DataTable dtRecords = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Size_ProcessID_Details", sp, DataAccess.Return_Type.DataTable);

                //Add size and rim to Maintable
                foreach (DataRow row in dtRecords.Rows)
                {
                    DataRow drMain = dtMain.NewRow();
                    foreach (DataColumn column in dtMain.Columns)
                    {
                        string Name = column.ColumnName;
                        if (Name == "TyreSize")
                            drMain["TyreSize"] = row["TyreSize"].ToString();
                        if (Name == "RimSize")
                            drMain["RimSize"] = row["TyreRim"].ToString();
                        if (Name != "TyreSize" && Name != "RimSize")
                        {
                            bool boolProcessID = false, boolFwt = false;
                            foreach (DataRow dr_nullRecords in dtnullRecords.Select("TyreType='" + Name + "'and TyreSize='" + row["TyreSize"].ToString() +
                                "'and TyreRim='" + row["TyreRim"].ToString() + "'"))
                            {
                                boolProcessID = true; boolFwt = true;
                                if (dr_nullRecords["FinishedWt"] == null || dr_nullRecords["FinishedWt"].ToString() == "")
                                    boolFwt = false;
                                break;
                            }
                            if (!boolProcessID && !boolFwt)
                                drMain["" + Name + ""] = "P-NA";
                            else if (boolProcessID && !boolFwt)
                                drMain["" + Name + ""] = "F-NA";

                            if (boolProcessID && boolFwt)
                            {
                                foreach (DataRow dr_pricesheet in dtPricesheet.Select("TyreType='" + Name + "'and TyreSize='" + row["TyreSize"].ToString() +
                                    "'and RimSize='" + row["TyreRim"].ToString() + "'"))
                                {
                                    drMain["" + Name + ""] = dr_pricesheet["UnitPrice"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                    dtMain.Rows.Add(drMain);
                }
                foreach (DataColumn col in dtMain.Columns)
                {
                    TemplateField bfield = new TemplateField();
                    bfield.HeaderTemplate = new GridViewAllTemplate(ListItemType.Header, col.ColumnName);
                    bfield.ItemTemplate = new GridViewAllTemplate(ListItemType.Item, col.ColumnName);
                    gvPriceDetails.Columns.Add(bfield);
                }
                if (dtMain.Rows.Count > 0)
                {
                    gvPriceDetails.DataSource = dtMain;
                    gvPriceDetails.DataBind();
                    if (ddlPriceSheetSelection.Items.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JGet1", "GetRatesID_FromPriceSheet();", true);
                        SqlParameter[] spExistsPrice = new SqlParameter[] { 
                            new SqlParameter("@PriceSheet_RefNo", ddlPriceSheetSelection.SelectedItem.Text == "ADD NEW SHEET" ? txtPriceSheet.Text : ddlPriceSheetSelection.SelectedItem.Text), 
                            new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                            new SqlParameter("@Brand", ddl_Brand.SelectedItem.Text), 
                            new SqlParameter("@Sidewall", ddl_Sidewall.SelectedItem.Text), 
                            new SqlParameter("@TyreType", strTyreType), 
                            new SqlParameter("@Custcode", "DE0048")
                        };
                        DataTable dtExistsPrice = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Manual_Entered_ListPrice", spExistsPrice, DataAccess.Return_Type.DataTable);
                        if (dtExistsPrice != null && dtExistsPrice.Rows.Count > 0)
                        {
                            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                            Dictionary<string, object> row;
                            foreach (DataRow dr in dtExistsPrice.Rows)
                            {
                                row = new Dictionary<string, object>();
                                foreach (DataColumn col in dtExistsPrice.Columns)
                                {
                                    row.Add(col.ColumnName, dr[col]);
                                }
                                rows.Add(row);
                            }
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JBind2", "Bind_Exists_EnteredPrice('" + serializer.Serialize(rows) + "');", true);
                        }
                    }
                }
                else
                    lblErrMsg.Text = "No Records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        [WebMethod]
        public static string GetRatesId(string PriceSheetRefNo)
        {
            try
            {
                DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
                {
                    string Json = "";
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@PriceSheetRefNo", PriceSheetRefNo) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_RatedID_Enddate", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        Json = dt.Rows[0]["RatesID"].ToString() + "||" + dt.Rows[0]["EndDate"].ToString();
                    }
                    return Json;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", HttpContext.Current.Request.UrlReferrer.PathAndQuery.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return "";
        }
        [WebMethod]
        public static string SaveMasterData(string PriceSheetRefNo, string RatesID, string EndDate, string Custcode, string Category)
        {
            try
            {
                DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
                {
                    SqlParameter[] sp = new SqlParameter[6];
                    sp[0] = new SqlParameter("@PriceSheet_RefNo", PriceSheetRefNo);
                    sp[1] = new SqlParameter("@Rates_ID", RatesID);
                    sp[2] = new SqlParameter("@End_Date", EndDate);
                    sp[3] = new SqlParameter("@UserName", HttpContext.Current.Request.Cookies["TTSUser"].Value);
                    sp[4] = new SqlParameter("@Custcode", Custcode);
                    sp[5] = new SqlParameter("@Category", Category);

                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_PriceSheet_Master", sp);
                    if (resp > 0)
                        return "Success";
                }
                Utilities.WriteToErrorLog("TTS-CLOSE", HttpContext.Current.Request.UrlReferrer.PathAndQuery.ToString(), MethodBase.GetCurrentMethod().Name, 1, "");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", HttpContext.Current.Request.UrlReferrer.PathAndQuery.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return "";
        }
        [WebMethod]
        public static string SaveItemDetails(string PriceSheetRefNo, string Jathagam)
        {
            try
            {
                string[] Items = Jathagam.Split('_');
                if (Items.Length > 0)
                {
                    DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
                    {
                        SqlParameter[] sp1 = new SqlParameter[]{
                                new SqlParameter("@PriceSheet_RefNo",PriceSheetRefNo),
                                new SqlParameter("@Category",Items[0]),
                                new SqlParameter("@Config",Items[1]),
                                new SqlParameter("@Brand",Items[2]),
                                new SqlParameter("@Sidewall",Items[3]),
                                new SqlParameter("@TyreType",Items[4]),
                                new SqlParameter("@TyreSize",Items[5]),
                                new SqlParameter("@RimSize",Items[6]),
                                new SqlParameter("@Unit_Price",Convert.ToDecimal(Items[7])),
                                new SqlParameter("@Custcode",Items[8])
                            };
                        int resp2 = daCOTS.ExecuteNonQuery_SP("sp_ins_PriceSheet_Details", sp1);
                        if (resp2 > 0)
                            return "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", HttpContext.Current.Request.UrlReferrer.PathAndQuery.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return "";
        }

        protected void btnSavePriceSheet_Click(object sender, EventArgs e)
        {

        }

        protected void ddlPriceSheetSelection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}