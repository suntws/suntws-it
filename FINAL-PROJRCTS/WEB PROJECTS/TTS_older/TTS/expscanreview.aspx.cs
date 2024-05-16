using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;

namespace TTS
{
    public partial class expscanreview : System.Web.UI.Page
    {
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["dom_ordertrack"].ToString() == "True" || dtUser.Rows[0]["exp_ordertrack"].ToString() == "True"))
                        {
                            lblPageHead.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "DOMESTIC" : "EXPORT") + " PDI REPORT";

                            string strUserName = "";
                            if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                                strUserName = Request.Cookies["TTSUser"].Value;
                            SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@PdiType", Utilities.Decrypt(Request["fid"].ToString())), 
                                new SqlParameter("@Username", strUserName) 
                            };
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdi_scanmasterdata_plant", sp, DataAccess.Return_Type.DataTable);
                            Utilities.ddl_Binding(ddlplant, dt, "pdiplant", "pdiplant", "ALL");
                            if (dt.Rows.Count > 0)
                            {
                                ddl_bindyear();
                                ddl_bindMonth();
                                Bind_Pdiscantrack(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
                            }
                            else
                                lblErrMsg.Text = "No Records";
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
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ddl_bindyear()
        {
            try
            {
                string strUserName = "";
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                    strUserName = Request.Cookies["TTSUser"].Value;

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@PdiType", Utilities.Decrypt(Request["fid"].ToString())), 
                    new SqlParameter("@Username", strUserName), 
                    new SqlParameter("@pdiPlant", ddlplant.SelectedItem.Text) 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdi_scanmasterdata_year", sp, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlYear, dt, "PdiYear", "PdiYear", "");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ddl_bindMonth()
        {
            try
            {
                string strUserName = "";
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                    strUserName = Request.Cookies["TTSUser"].Value;

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@PdiType", Utilities.Decrypt(Request["fid"].ToString())), 
                    new SqlParameter("@Username", strUserName), 
                    new SqlParameter("@pdiPlant", ddlplant.SelectedItem.Text),
                    new SqlParameter("@PdiYear", ddlYear.SelectedItem.Text)
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdi_scanmasterdata_month", sp, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlMonth, dt, "PdiMonthName", "PdiMonth", "");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindMonth();
            Bind_Pdiscantrack(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_Pdiscantrack(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindyear();
            ddl_bindMonth();
            Bind_Pdiscantrack(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        private void Bind_Pdiscantrack(string plant, string year, string month)
        {
            try
            {
                string strUserName = "";
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                    strUserName = Request.Cookies["TTSUser"].Value;

                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@Username", strUserName);
                sp1[1] = new SqlParameter("@pdiPlant", plant);
                sp1[2] = new SqlParameter("@PdiYear", Convert.ToInt32(year));
                sp1[3] = new SqlParameter("@PdiMonth", Convert.ToInt32(month));
                sp1[4] = new SqlParameter("@PdiType", Utilities.Decrypt(Request["fid"].ToString()));
                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdi_DispatchedList", sp1, DataAccess.Return_Type.DataTable);

                Bind_GvList(dtList);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkShowDetails_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Build_SelectDispatchedOrder(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_GvList(DataTable dtList)
        {
            try
            {
                lblErrMsg.Text = "";
                lblOrderCount.Text = "";
                if (dtList.Rows.Count > 0)
                {
                    lblOrderCount.Text = dtList.Rows.Count.ToString();
                    gvDispatchedPdiList.DataSource = dtList;
                    gvDispatchedPdiList.DataBind();
                    ViewState["dtorderlist"] = dtList;
                }
                else
                {
                    lblErrMsg.Text = "No records";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectDispatchedOrder(GridViewRow row)
        {
            try
            {
                hdnID.Value = ((HiddenField)row.FindControl("hdnSNo")).Value;
                hdnCustCode.Value = ((HiddenField)row.FindControl("hdnStatusCustCode")).Value;
                lblCustName.Text = ((Label)row.FindControl("lblStatusCustName")).Text;
                lblWorkorderno.Text = ((Label)row.FindControl("lblWorkorderno")).Text;
                lblStausOrderRefNo.Text = ((Label)row.FindControl("lblOrderRefNo")).Text;

                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@ID", hdnID.Value) };
                DataTable dtMasterData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanMasterData", sp2, DataAccess.Return_Type.DataTable);
                if (dtMasterData.Rows.Count > 0)
                {
                    dlOrderMaster.DataSource = dtMasterData;
                    dlOrderMaster.DataBind();
                    Bind_Scanned_ItemQty();
                    Bind_Gridview();

                    string strOrderNo = Utilities.Decrypt(Request["fid"].ToString()).ToLower() == "d" ? lblStausOrderRefNo.Text : lblWorkorderno.Text;
                    //OLD FORMAT
                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    string path2 = serverURL + "invoicefiles\\" + hdnCustCode.Value + "\\PDIList_" + strOrderNo + ".pdf";
                    FileInfo file2 = new FileInfo(path2);
                    if (file2.Exists)
                        lnkPDIFileDownload.Text = "PDIList_" + strOrderNo + ".pdf";

                    //NEW FORMAT
                    string path1 = serverURL + "invoicefiles\\" + hdnCustCode.Value + "\\PDIList_" + strOrderNo + "_" +
                        ddlplant.SelectedItem.Text.ToLower() + ".pdf";
                    FileInfo file1 = new FileInfo(path1);
                    if (file1.Exists)
                        lnkPDIFile.Text = "PDIList_" + strOrderNo + "_" + ddlplant.SelectedItem.Text.ToLower() + ".pdf";

                    //Final Inspection FORMAT
                    string path = serverURL + "invoicefiles\\" + hdnCustCode.Value + "\\FinalInspection_" + strOrderNo + "_" +
                        ddlplant.SelectedItem.Text.ToLower() + ".pdf";
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                        lnkFinlaInpsect.Text = "FinalInspection_" + strOrderNo + "_" + ddlplant.SelectedItem.Text.ToLower() + ".pdf";

                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Gridview()
        {
            SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@ID", hdnID.Value) };
            DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanList", sp1, DataAccess.Return_Type.DataTable);
            if (dtData.Rows.Count > 0)
            {
                gvScanPdiLsit.DataSource = dtData;
                gvScanPdiLsit.DataBind();

                ViewState["dtData"] = dtData;
                Bind_DDL_GV(dtData, "");
            }
        }
        protected void btnPdiPdfGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/invoicefiles/" + hdnCustCode.Value + "/"))
                    Directory.CreateDirectory(serverURL + "/invoicefiles/" + hdnCustCode.Value + "/");
                string path = serverURL + "/invoicefiles/" + hdnCustCode.Value + "/";
                string strfileAdditional = "PDIList_";
                string strOrderNo = Utilities.Decrypt(Request["fid"].ToString()).ToLower() == "d" ? lblStausOrderRefNo.Text : lblWorkorderno.Text;
                string sPathToWritePdfTo = path + strfileAdditional + strOrderNo + "_" + ddlplant.SelectedItem.Text.ToLower() + ".pdf";

                Exp_PDI_Report.CustomerName = lblCustName.Text;
                Exp_PDI_Report.OrderNumber = lblWorkorderno.Text;
                Exp_PDI_Report.OrderQuantity = ((Label)dlOrderMaster.Items[0].FindControl("lblOrderQty")).Text;
                Exp_PDI_Report.InspectedBy = ((Label)dlOrderMaster.Items[0].FindControl("lblInspectedBy")).Text;
                Exp_PDI_Report.VerifiedBy = ((Label)dlOrderMaster.Items[0].FindControl("lblApprovedBy")).Text;
                Exp_PDI_Report.ApprovedBy = ((Label)dlOrderMaster.Items[0].FindControl("lblApprovedBy")).Text;
                Exp_PDI_Report.dtScanList = (DataTable)ViewState["dtData"];
                Exp_PDI_Report.PDI_Generation(sPathToWritePdfTo);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void Tab_Click(object sender, EventArgs e)
        {
            try
            {
                Button1.CssClass = "Initial";
                Button2.CssClass = "Initial";
                Button lnkTxt = sender as Button;
                if (lnkTxt.Text == "ITEM QTY WISE")
                {
                    Button1.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 0;
                }
                else if (lnkTxt.Text == "BARCODE WISE")
                {
                    Button2.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 1;
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Scanned_ItemQty()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@PID", hdnID.Value) };
                DataTable dtQty = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanItemQty", sp1, DataAccess.Return_Type.DataTable);
                if (dtQty.Rows.Count > 0)
                {
                    gvScannedItemWise.DataSource = dtQty;
                    gvScannedItemWise.DataBind();

                    Button1.CssClass = "Clicked";
                    Button2.CssClass = "Initial";
                    MultiView1.ActiveViewIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnExportXls_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", lblWorkorderno.Text));
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    GridView gv = new GridView();
                    gv.DataSource = ViewState["dtData"] as DataTable;
                    gv.DataBind();
                    gv.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                    gv = null;
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        protected void lnkPDIFileDownload_Click(object sender, EventArgs e)
        {
            LinkButton lnkTxt = sender as LinkButton;
            string serverUrl = Server.MapPath("~/invoicefiles/" + hdnCustCode.Value + "/").Replace("TTS", "pdfs");
            string path = serverUrl + "/" + lnkTxt.Text;

            Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
            Response.WriteFile(path);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        protected void Pdiscantracklist_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtorderlist = ViewState["dtorderlist"] as DataTable;
                Bind_GvList(dtorderlist);
                gvDispatchedPdiList.PageIndex = e.NewPageIndex;
                gvDispatchedPdiList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlPlatform_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("PLATFORM");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlTyreSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("TYRESIZE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlRimSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("RIMSIZE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlTyretype_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("TYRETYPE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlBrand_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("BRAND");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Sidewall_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("SIDEWALL");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Make_Where_Condition(string strField)
        {
            try
            {
                DataTable dt = ViewState["dtData"] as DataTable;
                string strCond = "";
                if (ddlPlatform.SelectedItem.Text != "ALL")

                    if (strCond.Length == 0)
                        strCond += "PLATFORM='" + ddlPlatform.SelectedItem.Text + "'";
                    else
                        strCond += " and PLATFORM='" + ddlPlatform.SelectedItem.Text + "'";
                if (ddlTyreSize.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "[TYRE SIZE]='" + ddlTyreSize.SelectedItem.Text + "'";
                    else
                        strCond += " and [TYRE SIZE]='" + ddlTyreSize.SelectedItem.Text + "'";
                if (ddlRimSize.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "RIM='" + ddlRimSize.SelectedItem.Text + "'";
                    else
                        strCond += " and RIM='" + ddlRimSize.SelectedItem.Text + "'";
                if (ddlTyretype.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "TYPE='" + ddlTyretype.SelectedItem.Text + "'";
                    else
                        strCond += " and TYPE='" + ddlTyretype.SelectedItem.Text + "'";
                if (ddlBrand.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "BRAND='" + ddlBrand.SelectedItem.Text + "'";
                    else
                        strCond += " and BRAND='" + ddlBrand.SelectedItem.Text + "'";
                if (ddl_Sidewall.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "SIDEWALL='" + ddl_Sidewall.SelectedItem.Text + "'";
                    else
                        strCond += " and SIDEWALL='" + ddl_Sidewall.SelectedItem.Text + "'";
                DataView dtView = new DataView(dt, strCond, "PROCESSID", DataViewRowState.CurrentRows);
                if (dtView.Count > 0)
                {
                    Bind_DDL_GV(dtView.ToTable(), strField);
                }
                else
                {
                    gvScanPdiLsit.DataSource = dtView;
                    gvScanPdiLsit.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('divStatusChange');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_DDL_GV(DataTable dtGv, string strCont)
        {
            try
            {
                DataView dtView = new DataView(dtGv);
                switch (strCont)
                {
                    case "PLATFORM":
                        dtView.Sort = "TYRE SIZE";
                        DataTable distinctTyreSize = dtView.ToTable(true, "TYRE SIZE");
                        Utilities.ddl_Binding(ddlTyreSize, distinctTyreSize, "TYRE SIZE", "TYRE SIZE", "");
                        ddlTyreSize.Items.Insert(0, "ALL");
                        dtView.Sort = "RIM";
                        DataTable distinctRimSize = dtView.ToTable(true, "RIM");
                        Utilities.ddl_Binding(ddlRimSize, distinctRimSize, "RIM", "RIM", "");
                        ddlRimSize.Items.Insert(0, "ALL");
                        dtView.Sort = "TYPE";
                        DataTable distinctTyretype = dtView.ToTable(true, "TYPE");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "TYPE", "TYPE", "");
                        ddlTyretype.Items.Insert(0, "ALL");
                        dtView.Sort = "BRAND";
                        DataTable distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");
                        dtView.Sort = "SIDEWALL";
                        DataTable distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "SIDEWALL", "SIDEWALL", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                    case "TYRESIZE":
                        dtView.Sort = "RIM";
                        distinctRimSize = dtView.ToTable(true, "RIM");
                        Utilities.ddl_Binding(ddlRimSize, distinctRimSize, "RIM", "RIM", "");
                        ddlRimSize.Items.Insert(0, "ALL");
                        dtView.Sort = "TYPE";
                        distinctTyretype = dtView.ToTable(true, "TYPE");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "TYPE", "TYPE", "");
                        ddlTyretype.Items.Insert(0, "ALL");
                        dtView.Sort = "BRAND";
                        distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");
                        dtView.Sort = "SIDEWALL";
                        distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "SIDEWALL", "SIDEWALL", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                    case "RIMSIZE":
                        distinctTyretype = dtView.ToTable(true, "TYPE");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "TYPE", "TYPE", "");
                        ddlTyretype.Items.Insert(0, "ALL");


                        dtView.Sort = "BRAND";
                        distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");

                        dtView.Sort = "SIDEWALL";
                        distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "SIDEWALL", "SIDEWALL", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;

                    case "TYRETYPE":
                        dtView.Sort = "BRAND";
                        distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");

                        dtView.Sort = "SIDEWALL";
                        distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;

                    case "BRAND":
                        dtView.Sort = "sidewall";
                        distinctSidewall = dtView.ToTable(true, "sidewall");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                    case "SIDEWALL":
                        break;
                    default:
                        dtView.Sort = "PLATFORM";
                        DataTable distinctType = dtView.ToTable(true, "PLATFORM");
                        Utilities.ddl_Binding(ddlPlatform, distinctType, "PLATFORM", "PLATFORM", "");
                        ddlPlatform.Items.Insert(0, "ALL");

                        dtView.Sort = "TYRE SIZE";
                        distinctTyreSize = dtView.ToTable(true, "TYRE SIZE");
                        Utilities.ddl_Binding(ddlTyreSize, distinctTyreSize, "TYRE SIZE", "TYRE SIZE", "");
                        ddlTyreSize.Items.Insert(0, "ALL");

                        dtView.Sort = "RIM";
                        distinctRimSize = dtView.ToTable(true, "RIM");
                        Utilities.ddl_Binding(ddlRimSize, distinctRimSize, "RIM", "RIM", "");
                        ddlRimSize.Items.Insert(0, "ALL");

                        dtView.Sort = "TYPE";
                        distinctTyretype = dtView.ToTable(true, "TYPE");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "TYPE", "TYPE", "");
                        ddlTyretype.Items.Insert(0, "ALL");


                        dtView.Sort = "BRAND";
                        distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");

                        dtView.Sort = "SIDEWALL";
                        distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "SIDEWALL", "SIDEWALL", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                }
                dtView = new DataView(dtGv, "", "PROCESSID DESC", DataViewRowState.CurrentRows);
                if (dtView.Count > 0)
                {
                    gvScanPdiLsit.DataSource = dtView;
                    gvScanPdiLsit.DataBind();
                }
                else
                {
                    gvScanPdiLsit.DataSource = null;
                    gvScanPdiLsit.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('divStatusChange');", true);
            }

            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}