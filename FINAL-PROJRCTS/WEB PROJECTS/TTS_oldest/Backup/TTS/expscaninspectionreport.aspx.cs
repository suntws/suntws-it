using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.IO;

namespace TTS
{
    public partial class expscaninspectionreport : System.Web.UI.Page
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
                            (dtUser.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True" ||
                             dtUser.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True"))
                        {
                            if (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageTitle.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ") + " PDI REPORT PREPARE";
                                SqlParameter[] sp = new SqlParameter[] { 
                                    new SqlParameter("@qstring", Utilities.Decrypt(Request["fid"].ToString())), 
                                    new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())) 
                                };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdiscanmaster_InspectionLevel2", sp, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gv_InspectionOrders.DataSource = dt;
                                    gv_InspectionOrders.DataBind();
                                }
                                else
                                    lblErrMsgcontent.Text = "No Records";
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL is wrong.";
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
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnk_ViewJathagam_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lbl_CustomerName.Text = clickedRow.Cells[0].Text;
                hdn_orderrefno.Value = clickedRow.Cells[1].Text;
                lbl_WorkOrderNo.Text = clickedRow.Cells[2].Text;
                hdn_CustCode.Value = ((HiddenField)clickedRow.FindControl("hdn_CustCode")).Value;
                hdn_PID.Value = ((HiddenField)clickedRow.FindControl("hdn_PID")).Value;

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@PID", hdn_PID.Value) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ProdWareHouse_InspectionLevel2", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gv_Jathagam.DataSource = dt;
                    gv_Jathagam.DataBind();

                    string strOrderNo = Utilities.Decrypt(Request["fid"].ToString()).ToLower() == "d" ? hdn_orderrefno.Value : lbl_WorkOrderNo.Text;
                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    string path = serverURL + "invoicefiles\\" + hdn_CustCode.Value + "\\FinalInspection_" + strOrderNo + "_" +
                        Utilities.Decrypt(Request["pid"].ToString()).ToLower() + ".pdf";
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                        lnkFinlaInpsect.Text = "FinalInspection_" + strOrderNo + "_" + Utilities.Decrypt(Request["pid"].ToString()).ToLower() + ".pdf";
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showjathagam", "gotoPreviewDiv('div_Jathagam');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnk_ViewBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lbl_sel_Platform.Text = clickedRow.Cells[0].Text;
                lbl_sel_Brand.Text = clickedRow.Cells[1].Text;
                lbl_sel_SideWall.Text = clickedRow.Cells[2].Text;
                lbl_sel_Type.Text = clickedRow.Cells[3].Text;
                lbl_sel_Size.Text = clickedRow.Cells[4].Text;
                lbl_sel_Rim.Text = clickedRow.Cells[5].Text;
                lbl_sel_ProcessID.Text = ((Label)clickedRow.FindControl("lbl_ProcessID")).Text;
                hdn_OrderQty.Value = clickedRow.Cells[7].Text;

                lbl_ErrMsg.Text = "";
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@PID", Convert.ToInt32(hdn_PID.Value)), 
                    new SqlParameter("@ProcessID", lbl_sel_ProcessID.Text), 
                    new SqlParameter("@Custcode", hdn_CustCode.Value), 
                    new SqlParameter("@workorderno", lbl_WorkOrderNo.Text), 
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()) 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdiscanlist_InspectionLevel2", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    chk_BarcodeSel.DataSource = dt;
                    chk_BarcodeSel.DataTextField = "StencilNo";
                    chk_BarcodeSel.DataValueField = "StencilNo";
                    chk_BarcodeSel.DataBind();

                    foreach (ListItem item in chk_BarcodeSel.Items)
                    {
                        foreach (DataRow row in dt.Select("StencilNo='" + item.Text + "'"))
                        {
                            item.Selected = Convert.ToBoolean(row["inspectstatus"].ToString());
                            item.Enabled = Convert.ToBoolean(row["inspectstatus"].ToString()) ? false : true;
                        }
                    }

                    ScriptManager.RegisterStartupScript(Page, GetType(), "showjathagam1", "gotoPreviewDiv('div_Jathagam');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showBarcode", "gotoPreviewDiv('div_Barcode');", true);
                }
                else
                    lbl_ErrMsg.Text = "No Records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_InspectBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> lst_InpsectionBarcode = new List<string>();
                lst_InpsectionBarcode = InspectedBarcodeList();
                int colmnCount = 4 + lst_InpsectionBarcode.Count;
                int colspan = colmnCount - 2;
                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='tbl_Inspection' cellpadding='5' border='1px' style='width: 100%;'>");
                //1st row .s no ,description,observation
                sb.AppendLine("<tr><th rowspan='3'>S.NO</th><th rowspan='3'>DESCRIPTION</th><th colspan='" + colspan + "'>SUN-TWS OBSERVATION</th></tr>");

                //2nd row
                sb.AppendLine("<tr><th colspan='" + (colspan - 1).ToString() + "'>SAMPLE SIZE</th><th rowspan='2'>JUDGEMENT</th></tr>");

                //3rd row
                sb.AppendLine("<th>SPEC</th>");
                for (int i = 0; i < lst_InpsectionBarcode.Count; i++)
                    sb.AppendLine("<th>" + lst_InpsectionBarcode[i].ToString() + "</th>");

                string[] arrDescriptn = { "Rim Width(mm)", "Outer Diameter(mm)", "Tyre Width(mm)", "Inner Diameter(mm)", "Hardness(sh-A)", "Apperance", "Tyre Grade" };
                string[] arrtxtName = { "txt_RimWidth_", "txt_OuterDiameter_", "txt_TyreWidth_", "txt_InnerDiameter_", "txt_Hardness_", "txt_APPERANCE_", "txt_TyreGrade_" };
                // 4 to 11 rows
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@Custcode", hdn_CustCode.Value), 
                    new SqlParameter("@WorkOrderNo", lbl_WorkOrderNo.Text), 
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()),
                    new SqlParameter("@ProcessID", lbl_sel_ProcessID.Text) 
                };
                DataTable dt_InspectionDetails = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Rpt_InspectionLevel2_ProcessID", sp, DataAccess.Return_Type.DataTable);
                string str_Spec = ""; string str_Judge = ""; string str_Act = "";
                bool boolUpdateStencil = false;
                for (int j = 0; j < 7; j++)
                {
                    str_Spec = ""; str_Judge = ""; bool boolSepcCtrl = true;
                    sb.AppendLine("<tr><td>" + (j + 1) + "</td><td class='spanCss'>" + arrDescriptn[j].ToString() + "</td>");

                    bool boolSpecVal = true;
                    for (int i = 0; i < lst_InpsectionBarcode.Count; i++)
                    {
                        bool boolStencil = true; str_Act = "";
                        if (dt_InspectionDetails.Rows.Count > 0)
                        {
                            str_Spec = dt_InspectionDetails.Rows[0]["" + arrtxtName[j].Replace("txt_", "") + "Spec"].ToString();
                            str_Judge = dt_InspectionDetails.Rows[0]["" + arrtxtName[j].Replace("txt_", "") + "Judge"].ToString();
                            boolSepcCtrl = false;
                            foreach (DataRow row in dt_InspectionDetails.Select("StencilNo='" + lst_InpsectionBarcode[i].ToString() + "'"))
                            {
                                str_Act = row["" + arrtxtName[j].Replace("txt_", "") + "Act"].ToString();
                                boolStencil = false;
                                break;
                            }
                            if (boolStencil && i == 0)
                                boolUpdateStencil = true;
                        }
                        if (boolSpecVal)
                        {
                            sb.AppendLine("<td><input type='text' id='" + arrtxtName[j] + "Spec' name='" + arrtxtName[j] + "Spec' " + (!boolSepcCtrl ? " " +
                                "readonly=readonly " : "") + " class='form-control' style='width: 100%; height:20px;' value='" + str_Spec + "' /></td>");
                            boolSpecVal = false;
                        }
                        sb.AppendLine("<td><input type='text' id='" + arrtxtName[j] + "Act_" + lst_InpsectionBarcode[i].ToString() + "' name ='" + arrtxtName[j] + "Act_" +
                            lst_InpsectionBarcode[i].ToString() + "' value='" + str_Act + "' " + (!boolStencil ? " disabled=disabled " : "") + " class='form-control' " +
                            "style='width: " + "100%; height:20px;'/></td>");
                    }
                    sb.AppendLine("<td><input type='text' id='" + arrtxtName[j] + "Judge' name='" + arrtxtName[j] + "Judge'  value='" + str_Judge + "' class='form-control' " +
                        "style='width: 100%; height:20px;'/></td></tr>");
                }
                sb.AppendLine("</table>");
                div_Dynamictbl.InnerHtml = sb.ToString();

                ScriptManager.RegisterStartupScript(Page, GetType(), "showjathagam2", "gotoPreviewDiv('div_Jathagam');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showInspection", "gotoPreviewDiv('div_BarcodeInspection');", true);
                if (!boolUpdateStencil)
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowRptGeneratn", "gotoPreviewDiv('div_GenerateRpt');", true);
                if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                    Bind_RptDates();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_SaveRecords_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_InspectionDetails = new DataTable();
                string[] arrColmns = { "CustCode", "WorkOrderNo", "Plant", "ProcessID", "StencilNo", "RimWidth_Spec", "RimWidth_Act", "RimWidth_Judge", "OuterDiameter_Spec", 
                                         "OuterDiameter_Act", "OuterDiameter_Judge", "TyreWidth_Spec", "TyreWidth_Act", "TyreWidth_Judge", "InnerDiameter_Spec", 
                                         "InnerDiameter_Act", "InnerDiameter_Judge", "Hardness_Spec", "Hardness_Act", "Hardness_Judge", "Apperance_Spec", "Apperance_Act", 
                                         "Apperance_Judge", "TyreGrade_Spec", "TyreGrade_Act", "TyreGrade_Judge", "Created_Date", "UserName" };
                foreach (var col in arrColmns)
                {
                    if (col.ToString().Contains("Apperance") || col.ToString().Contains("TyreGrade") || col.ToString().Contains("_Judge") || col.ToString().Contains("ProcessID")
                        || col.ToString().Contains("UserName") || col.ToString().Contains("CustCode") || col.ToString().Contains("WorkOrderNo")
                        || col.ToString().Contains("StencilNo")) dt_InspectionDetails.Columns.Add(col, typeof(string));
                    else if (col.ToString().Contains("Created_Date"))
                        dt_InspectionDetails.Columns.Add(col, typeof(DateTime));
                    else
                        dt_InspectionDetails.Columns.Add(col, typeof(string));
                }
                string str_RimWidth_Spec = Request.Form["txt_RimWidth_Spec"].ToString();
                string str_RimWidth_Judge = Request.Form["txt_RimWidth_Judge"].ToString();
                string str_OuterDiameter_Spec = Request.Form["txt_OuterDiameter_Spec"].ToString();
                string str_OuterDiameter_Judge = Request.Form["txt_OuterDiameter_Judge"].ToString();
                string str_TyreWidth_Spec = Request.Form["txt_TyreWidth_Spec"].ToString();
                string str_TyreWidth_Judge = Request.Form["txt_TyreWidth_Judge"].ToString();
                string str_InnerDiameter_Spec = Request.Form["txt_InnerDiameter_Spec"].ToString();
                string str_InnerDiameter_Judge = Request.Form["txt_InnerDiameter_Judge"].ToString();
                string str_Hardness_Spec = Request.Form["txt_Hardness_Spec"].ToString();
                string str_Hardness_Judge = Request.Form["txt_Hardness_Judge"].ToString();
                string str_APPERANCE_Spec = Request.Form["txt_APPERANCE_Spec"].ToString();
                string str_APPERANCE_Judge = Request.Form["txt_APPERANCE_Judge"].ToString();
                string str_TyreGrade_Spec = Request.Form["txt_TyreGrade_Spec"].ToString();
                string str_TyreGrade_Judge = Request.Form["txt_TyreGrade_Judge"].ToString();
                foreach (var item in InspectedBarcodeList())
                {
                    if (Request.Form["txt_RimWidth_Act_" + item + ""] != null)
                    {
                        string str_RimWidth_Act = Request.Form["txt_RimWidth_Act_" + item + ""].ToString();
                        string str_OuterDiameter_Act = Request.Form["txt_OuterDiameter_Act_" + item + ""].ToString();
                        string str_TyreWidth_Act = Request.Form["txt_TyreWidth_Act_" + item + ""].ToString();
                        string str_InnerDiameter_Act = Request.Form["txt_InnerDiameter_Act_" + item + ""].ToString();
                        string str_Hardness_Act = Request.Form["txt_Hardness_Act_" + item + ""].ToString();
                        string str_APPERANCE_Act = Request.Form["txt_APPERANCE_Act_" + item + ""].ToString();
                        string str_TyreGrade_Act = Request.Form["txt_TyreGrade_Act_" + item + ""].ToString();

                        //bind rows to dt_InspectionDetails datatable
                        dt_InspectionDetails.Rows.Add(hdn_CustCode.Value, lbl_WorkOrderNo.Text, Utilities.Decrypt(Request["pid"].ToString()).ToUpper(), lbl_sel_ProcessID.Text,
                            item.ToString(), str_RimWidth_Spec, str_RimWidth_Act, str_RimWidth_Judge, str_OuterDiameter_Spec, str_OuterDiameter_Act, str_OuterDiameter_Judge,
                            str_TyreWidth_Spec, str_TyreWidth_Act, str_TyreWidth_Judge, str_InnerDiameter_Spec, str_InnerDiameter_Act, str_InnerDiameter_Judge, str_Hardness_Spec,
                            str_Hardness_Act, str_Hardness_Judge, str_APPERANCE_Spec, str_APPERANCE_Act, str_APPERANCE_Judge, str_TyreGrade_Spec, str_TyreGrade_Act,
                            str_TyreGrade_Judge, DateTime.Now, Request.Cookies["TTSUser"].Value);
                    }
                }
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Pdi_InspectionLevel2_datatable", dt_InspectionDetails) };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_PdiInspectionLevel2", sp);
                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showjathagam3", "gotoPreviewDiv('div_Jathagam');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showInspection1", "gotoPreviewDiv('div_BarcodeInspection');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowRptGeneratn", "gotoPreviewDiv('div_GenerateRpt');", true);
                    if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                        Bind_RptDates();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_RptDates()
        {
            try
            {
                SqlParameter[] spDate = new SqlParameter[] { 
                            new SqlParameter("@custcode", hdn_CustCode.Value), 
                            new SqlParameter("@orderrefno", hdn_orderrefno.Value), 
                            new SqlParameter("@pdiplant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                            new SqlParameter("@workorderno", lbl_WorkOrderNo.Text) 
                        };
                DataTable dtDate = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdi_finalreport", spDate, DataAccess.Return_Type.DataTable);
                if (dtDate.Rows.Count > 0)
                {
                    txt_OrderDate.Text = dtDate.Rows[0]["OrderDate"].ToString();
                    txt_InvoiceNo.Text = dtDate.Rows[0]["invoiceno"].ToString();
                    txt_InvoiceDate.Text = dtDate.Rows[0]["InvoiceDate"].ToString();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public List<string> InspectedBarcodeList()
        {
            try
            {
                List<string> lst = new List<string>();
                foreach (ListItem row in chk_BarcodeSel.Items)
                {
                    if (row.Selected)
                        lst.Add(row.Text);
                }
                return lst;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return null;
            }
        }
        protected void btnGenerateRpt_Click(object sender, EventArgs e)
        {
            try
            {
                expscanpdiinspectionreport pdi = new expscanpdiinspectionreport();
                pdi.str_CustCode = hdn_CustCode.Value;
                pdi.str_OrderNo = hdn_orderrefno.Value;
                pdi.str_WorkOrderNo = lbl_WorkOrderNo.Text;
                pdi.str_CustName = lbl_CustomerName.Text;
                pdi.str_OrderDate = Convert.ToDateTime(txt_OrderDate.Text).ToString("dd-MM-yyyy");
                pdi.str_InvoiceDate = Convert.ToDateTime(txt_InvoiceDate.Text).ToString("dd-MM-yyyy");
                pdi.str_InvoiceNo = txt_InvoiceNo.Text;
                pdi.str_PdiPlant = Utilities.Decrypt(Request["pid"].ToString()).ToUpper();
                pdi.str_fID = Utilities.Decrypt(Request["fid"].ToString());
                pdi.str_PID = hdn_PID.Value;
                pdi.PdiInspectionRptCreation();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkPDIFileDownload_Click(object sender, EventArgs e)
        {
            LinkButton lnkTxt = sender as LinkButton;
            string path = (Server.MapPath("~/invoicefiles/" + hdn_CustCode.Value + "/").Replace("TTS", "pdfs")) + "/" + lnkTxt.Text;
            Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
            Response.WriteFile(path);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}