using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Reflection;
using System.IO;
using System.Collections;

namespace TTS
{
    public partial class ReportsPriceSheet : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.btnExportExcel);
            if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
            {
                hdnUserName.Value = Request.Cookies["TTSUser"].Value;
                if (!IsPostBack)
                {
                    DataTable dtUser = Session["dtuserlevel"] as DataTable;
                    if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["pricesheet_build"].ToString() == "True")
                    {
                        Bind_gv_CustRptPriceList();
                        Bind_PremiumValue();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                        lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                    }
                }
            }
            else
            {
                Response.Redirect("sessionexp.aspx", false);
            }
        }

        private void Bind_gv_CustRptPriceList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Config");
            dt.Columns.Add("Tyretype");
            //dt.Columns.Add("brand");
            //dt.Columns.Add("Sidewall");
            dt.Rows.Add();
            gv_CustRptPriceList.DataSource = dt;
            gv_CustRptPriceList.DataBind();
        }

        private void Bind_PremiumValue()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Config");
            dt.Columns.Add("PremiumType");
            dt.Columns.Add("PremiumValue");
            dt.Columns.Add("BaseType");
            dt.Columns.Add("BaseRmcb");
            dt.Rows.Add();
            gv_Rpt_PremiumValue.DataSource = dt;
            gv_Rpt_PremiumValue.DataBind();
        }

        protected void btnDisplayRecords_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptOpen", "showProgress();", true);
                lblMsg.Text = "";
                DataTable dtPriceList = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@PriceSheetRefNo", txtRptPriceSheet.Text);
                sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                dtPriceList = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_RptPriceDetails", sp1, DataAccess.Return_Type.DataTable);

                DataTable dtTypeDesc = (DataTable)daTTS.ExecuteReader_SP("sp_sel_typedescription", DataAccess.Return_Type.DataTable);

                if (dtPriceList.Rows.Count > 0)
                {
                    //gv_RptPriceDetails.DataSource = dtPriceList;
                    //gv_RptPriceDetails.DataBind();

                    DataTable dtRptMap = new DataTable("dtRptMap");
                    dtRptMap.Columns.Add("Size");
                    dtRptMap.Columns.Add("Rim");

                    DataView view = new DataView(dtPriceList);
                    view.Sort = "position ASC";
                    DataTable distinctSizes = view.ToTable(true, "TyreSize", "RimSize", "position");

                    dtRptMap.Rows.Add("", "");
                    dtRptMap.Rows.Add("TyreSize", "Rim");
                    foreach (DataRow sizeRow in distinctSizes.Rows)
                    {
                        dtRptMap.Rows.Add(sizeRow["TyreSize"].ToString(), sizeRow["RimSize"].ToString());
                    }

                    DataView DTview = new DataView(dtPriceList);
                    DataTable distinctTypes = DTview.ToTable(true, "TyreType");// "Brand", "Sidewall",

                    foreach (DataRow typeRow in distinctTypes.Rows)
                    {
                        string typeVal = typeRow["TyreType"].ToString();
                        string headConcat = typeVal;
                        dtRptMap.Columns.Add(headConcat);
                        string strDesc = string.Empty;
                        foreach (DataRow descRow in dtTypeDesc.Select("type='" + typeVal + "'"))
                        {
                            strDesc = descRow["TypeDesc"].ToString();
                        }
                        dtRptMap.Rows[0][headConcat] = strDesc != "" ? strDesc : "-";
                    }

                    foreach (DataRow typeRow in distinctTypes.Rows)
                    {
                        string typeVal = typeRow["TyreType"].ToString();
                        //string brandval = typeRow["Brand"].ToString();
                        //string sidewallVal = typeRow["Sidewall"].ToString();
                        string headConcat = typeVal;// +"-" + brandval + "-" + sidewallVal;
                        //dtRptMap.Columns.Add(headConcat);

                        dtRptMap.Rows[1][headConcat] = "Unit Price";
                        foreach (DataRow drow in dtRptMap.Rows)
                        {
                            foreach (DataRow rptlistRow in dtPriceList.Select("TyreSize='" + drow["Size"].ToString() + "' and RimSize='" + drow["Rim"].ToString() + "' and TyreType='" + typeVal + "'"))// and Brand='" + brandval + "' and Sidewall='" + sidewallVal + "'
                            {
                                drow[headConcat] = rptlistRow["UnitPrice"].ToString();
                            }
                        }
                    }

                    if (dtRptMap.Rows.Count > 0)
                    {
                        gv_RptFinalPriceList.DataSource = dtRptMap;
                        gv_RptFinalPriceList.DataBind();
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptBindList", "bindReportSheet();", true);
                    }

                    Bind_AlreadyPublishedSheet();
                }
                else
                {
                    lblMsg.Text = "No records - Please choose the price sheet again";
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string strConcat = string.Empty;

                Int32 gvColumnCount = 6;

                strConcat += "<table>";
                strConcat += "<tr><td colspan=\"" + gvColumnCount + "\">SUN TYRE & WHEEL SYSTEMS</td></tr><br />";
                strConcat = "<tr><td colspan='2'>Customer Name: </td><td colspan=\"" + gvColumnCount + "\">" + txtRptCustName.Text + "</td></tr><br />";
                strConcat += "<tr><td colspan='2'>PriceSheetRefNo: </td><td colspan=\"" + gvColumnCount + "\">" + txtRptPriceSheet.Text + "</td></tr><br />";
                strConcat += "<tr><td colspan='2'>Rates-ID: </td><td colspan=\"" + gvColumnCount + "\">" + txtRptRatesID.Text + "</td></tr><br />";
                strConcat += "<tr><td colspan='2'>Start Date: </td><td colspan=\"" + gvColumnCount + "\">" + txtPriceSheetValidFrom.Text + "</td></tr><br />";
                strConcat += "<tr><td colspan='2'>End Date: </td><td colspan=\"" + gvColumnCount + "\">" + txtPriceSheetValidTill.Text + "</td></tr><br />";
                strConcat += "</table>";
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + txtXlsFileName.Text + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                //gv_RptPriceDetails.RenderControl(htmlWrite);
                gv_RptFinalPriceList.RenderControl(htmlWrite);
                Response.Write(strConcat + stringWrite.ToString());
                Response.End();
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@PriceSheetRefNo", txtRptPriceSheet.Text);
                sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                DataTable dt1 = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_PriceSheet_AuthorizeList", sp1, DataAccess.Return_Type.DataTable);

                if (dt1.Rows.Count > 0)
                {
                    SqlParameter[] sp2 = new SqlParameter[1];
                    sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    DataTable dt2 = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AvailableList", sp2, DataAccess.Return_Type.DataTable);
                    if (dt2.Rows.Count > 0)
                    {
                        SqlParameter[] sp3 = new SqlParameter[7];
                        foreach (DataRow row1 in dt1.Rows)
                        {
                            foreach (DataRow row2 in dt2.Select("Config='" + row1["Config"].ToString() + "' AND TyreType='" + row1["TyreType"].ToString() + "' AND Brand='" + row1["Brand"].ToString() + "' AND Sidewall='" + row1["Sidewall"].ToString() + "'"))
                            {
                                sp3 = new SqlParameter[7];
                                sp3[0] = new SqlParameter("@Config", row2["Config"].ToString());
                                sp3[1] = new SqlParameter("@TyreType", row2["TyreType"].ToString());
                                sp3[2] = new SqlParameter("@Brand", row2["Brand"].ToString());
                                sp3[3] = new SqlParameter("@Sidewall", row2["Sidewall"].ToString());
                                sp3[4] = new SqlParameter("@PriceSheetRefNo", row2["PriceSheetRefNo"].ToString());
                                sp3[5] = new SqlParameter("@custcode", hdnCustCode.Value);
                                sp3[6] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                                daCOTS.ExecuteNonQuery_SP("sp_sel_DeleteCurrentList", sp3);
                            }
                        }
                    }
                }
                SqlParameter[] spRpt = new SqlParameter[3];
                spRpt[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                spRpt[1] = new SqlParameter("@PriceSheetRefNo", txtRptPriceSheet.Text);
                spRpt[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                DataTable dtRpt = new DataTable();
                dtRpt = (DataTable)daTTS.ExecuteReader_SP("sel_ins_for_anotherdb_RptPriceMaster", spRpt, DataAccess.Return_Type.DataTable);

                if (dtRpt.Rows.Count > 0)
                {
                    insert_to_COTS_db(dtRpt);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void insert_to_COTS_db(DataTable dtRpt)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@publish_datatable", dtRpt);
                sp1[1] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp1[2] = new SqlParameter("@CustCode", hdnCustCode.Value);
                sp1[3] = new SqlParameter("@CustCountry", ddlCountry.Text);
                sp1[4] = new SqlParameter("@PriceSheetRefNo", txtRptPriceSheet.Text);

                DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
                daCOTS.ExecuteNonQuery_SP("ins_datatable_via_pricesheet", sp1);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void PrepareControlForExport(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void gv_AlreadyPublishedList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int resp = 0;
                GridViewRow row = gv_AlreadyPublishedList.Rows[e.RowIndex];
                string strPriceSheet = row.Cells[0].Text;
                string strRatesID = row.Cells[1].Text;
                if (strPriceSheet != "" && strRatesID != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp1[1] = new SqlParameter("@sheetrefno", strPriceSheet);
                    sp1[2] = new SqlParameter("@ratesid", strRatesID);
                    resp = daCOTS.ExecuteNonQuery_SP("sp_edit_sheetstatus", sp1);
                }
                if (resp > 0)
                    Bind_AlreadyPublishedSheet();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-ORDERDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_AlreadyPublishedSheet()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);

                DataTable dtSheet = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_alreadypublishedsheet", sp1, DataAccess.Return_Type.DataTable);
                if (dtSheet.Rows.Count > 0)
                {
                    gv_AlreadyPublishedList.DataSource = dtSheet;
                    gv_AlreadyPublishedList.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "alredyPublishedList('block');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "alredyPublishedList('none');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-ORDERDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        #region[Get Cust Currency WebMethod]
        [WebMethod]
        public static string get_CustCurrency_WebMethod(string strCustName)
        {
            return GetCustomer_Currency(strCustName.ToString()).GetXml();
        }

        private static DataSet GetCustomer_Currency(string strCustName)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@CustName", strCustName);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Get_Details_CustWise", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "ReportsPriceSheet.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[get_PriceSheetValues_WebMethod]
        [WebMethod]
        public static string get_PriceSheetValues_WebMethod(string strCustCode, string strPriceSheet, string strUserName)
        {
            return GetPriceSheet_Values(strCustCode.ToString(), strPriceSheet.ToString(), strUserName.ToString()).GetXml();
        }

        private static DataSet GetPriceSheet_Values(string strCustCode, string strPriceSheet, string strUserName)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", strCustCode);
                sp1[1] = new SqlParameter("@PriceSheetRefNo", strPriceSheet);
                sp1[2] = new SqlParameter("@UserName", strUserName);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Sel_PriceSheetValues", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "ReportsPriceSheet.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[get_PriceSheetTypeList_WebMethod]
        [WebMethod]
        public static string get_PriceSheetTypeList_WebMethod(string strCustCode, string strPriceSheet, string strUserName)
        {
            return GetPriceSheet_TypeList(strCustCode.ToString(), strPriceSheet.ToString(), strUserName.ToString()).GetXml();
        }

        private static DataSet GetPriceSheet_TypeList(string strCustCode, string strPriceSheet, string strUserName)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", strCustCode);
                sp1[1] = new SqlParameter("@PriceSheetRefNo", strPriceSheet);
                sp1[2] = new SqlParameter("@UserName", strUserName);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Sel_PriceSheet_TypeList", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "ReportsPriceSheet.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[get_PriceSheetDate_WebMethod]

        [WebMethod]
        public static string get_PriceSheetDate_WebMethod(string strCustCode, string strPriceSheet, string strUserName)
        {
            return GetPriceSheet_ApplicableDate(strCustCode.ToString(), strPriceSheet.ToString(), strUserName.ToString()).GetXml();
        }

        private static DataSet GetPriceSheet_ApplicableDate(string strCustCode, string strPriceSheet, string strUserName)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", strCustCode);
                sp1[1] = new SqlParameter("@PriceSheetRefNo", strPriceSheet);
                sp1[2] = new SqlParameter("@UserName", strUserName);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Sel_RptPriceSheetApplicableDate", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "ReportsPriceSheet.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion
    }
}