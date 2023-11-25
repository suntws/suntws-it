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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

namespace TTS
{
    public partial class ClaimServiceComplaint : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["claim_crm_exp"].ToString() == "True" || dtUser.Rows[0]["claim_crm_dom"].ToString() == "True"
                            || dtUser.Rows[0]["claim_creditnote_settle_exp"].ToString() == "True" || dtUser.Rows[0]["claim_creditnote_settle_dom"].ToString() == "True"))
                        {
                            gv_bind();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                    if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCustName")
                        ddlCustName_SelectedIndexChanged(sender, e);
                }
                else
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCustName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlUserId.DataSource = "";
                ddlUserId.DataBind();
                System.Web.UI.WebControls.ListItem selectedListItem = ddlCustName.Items.FindByText("" + hdnFullName.Value + "");
                if (selectedListItem != null)
                {
                    ddlCustName.Items.FindByText(ddlCustName.SelectedItem.Text).Selected = false;
                    selectedListItem.Selected = true;
                }
                if (ddlCustName.SelectedItem.Text != "Choose")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddlCustName.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddlCustName.SelectedItem.Value) 
                    };
                    DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                    if (dtUserList.Rows.Count > 0)
                    {
                        ddlUserId.DataSource = dtUserList;
                        ddlUserId.DataTextField = "username";
                        ddlUserId.DataValueField = "ID";
                        ddlUserId.DataBind();
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "gotoPreviewDiv('divbtn');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoPreviewDiv('divregister');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void gv_bind()
        {
            gvClaimServiceList.DataSource = null;
            gvClaimServiceList.DataBind();
            txtCrmComments.Text = "";
            hdnstatus.Value = Request["pid"].ToString();

            string strCategory = Request["pcid"].ToString() == "D" ? "S-D" : "S-E";
            string strType = Request["pcid"].ToString() == "D" ? "DOMESTIC" : "EXPORT";
            if (Request["pid"].ToString() == "1")
            {
                lblheader.Text = "SERVICE CLAIM REGISTER - " + strType;
                btnSAVE.Text = "SAVE";
                DataTable dtName = new DataTable();
                if (Request["pcid"].ToString() == "D")
                    dtName = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_domestic_fullname", DataAccess.Return_Type.DataTable);
                else if (Request["pcid"].ToString() == "E")
                    dtName = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_export_fullname", DataAccess.Return_Type.DataTable);
                if (dtName.Rows.Count > 0)
                {
                    ddlCustName.DataSource = dtName;
                    ddlCustName.DataTextField = "custfullname";
                    ddlCustName.DataValueField = "custcode";
                    ddlCustName.DataBind();
                }
                ddlCustName.Items.Insert(0, "Choose");
                ddlCustName.Text = "Choose";
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoPreviewDiv('divregister');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "gotoPreviewDiv('divbtn');", true);
            }
            else
            {
                DataTable dt = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@Stdcode", strCategory);
                if (Request["pid"].ToString() == "3")
                {
                    lblheader.Text = "SERVICE CLAIM CREDIT NOTE PREPARATION - " + strType;
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Service_ClaimCreditNoteList", sp1, DataAccess.Return_Type.DataTable);
                }
                else if (Request["pid"].ToString() == "0")
                {
                    lblheader.Text = "SERVICE CLAIM TRACK STATUS - " + strType;
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Service_Claim_track_status", sp1, DataAccess.Return_Type.DataTable);
                }
                else if (Request["pid"].ToString() == "6")
                {
                    lblheader.Text = "SERVICE CLAIM CLOSED - " + strType;
                    DataTable dt1 = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Service_Claim_CLOSED", sp1, DataAccess.Return_Type.DataTable);
                    if (dt1.Rows.Count > 0)
                    {
                        gvclosed.DataSource = dt1;
                        gvclosed.DataBind();
                    }
                }
                else
                {
                    SqlParameter[] sp = new SqlParameter[2];
                    sp[0] = new SqlParameter("@Stdcode", strCategory);
                    if (Request["pid"].ToString() == "2")
                    {
                        lblheader.Text = "SERVICE CLAIM CRM OPINION - " + strType;
                        sp[1] = new SqlParameter("@statusid", "1");
                    }
                    else if (Request["pid"].ToString() == "4")
                    {
                        lblheader.Text = "SERVICE CLAIM CRM APPROVAL - " + strType;
                        sp[1] = new SqlParameter("@statusid", "4");
                    }
                    else if (Request["pid"].ToString() == "5")
                    {
                        lblheader.Text = "SERVICE CLAIM SETTLEMENT LIST - " + strType;
                        sp[1] = new SqlParameter("@statusid", "5");
                    }
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Service_ClaimList", sp, DataAccess.Return_Type.DataTable);
                }
                if (dt.Rows.Count > 0)
                {
                    gvClaimServiceList.DataSource = dt;
                    gvClaimServiceList.DataBind();
                }
            }
        }
        protected void btnSAVE_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request["pid"].ToString() == "1")
                {
                    System.Web.UI.WebControls.ListItem selectedListItem = ddlUserId.Items.FindByText("" + hdnLoginName.Value + "");
                    if (selectedListItem != null)
                    {
                        ddlUserId.Items.FindByText(ddlUserId.SelectedItem.Text).Selected = false;
                        selectedListItem.Selected = true;
                    }
                    string stdcode = Request["pcid"].ToString() == "D" ? "S-D" : "S-E";
                    SqlParameter[] sp = new SqlParameter[5];
                    sp[0] = new SqlParameter("@CustCode", ddlUserId.SelectedValue);
                    sp[1] = new SqlParameter("@RefNo", txtRefNo.Text.Trim());
                    sp[2] = new SqlParameter("@ClaimCommets", txtClaimComments.Text.Replace("\r\n", "~"));
                    sp[3] = new SqlParameter("@CreatedBy", Request.Cookies["TTSUser"].Value);
                    sp[4] = new SqlParameter("@stdcode", stdcode);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_claim_service_register", sp);
                    if (resp > 0)
                    {
                        Response.Redirect("default.aspx", false);
                    }
                }
                else if (Request["pid"].ToString() == "2")
                {
                    SqlParameter[] sp = new SqlParameter[4];
                    sp[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    sp[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                    sp[2] = new SqlParameter("@CRMCommets", txtCrmComments.Text.Replace("\r\n", "~"));
                    sp[3] = new SqlParameter("@CrmCreateBy", Request.Cookies["TTSUser"].Value);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_update_Service_claim_crm_opnion", sp);
                    if (resp > 0)
                    {
                        ins_history("CRM OPINION");
                    }
                    gv_bind();
                }
                else if (Request["pid"].ToString() == "3")
                {
                    string srtchar = Request["pcid"].ToString() == "D" ? "S-D" : "S-E";
                    hdnstrcredityear.Value = Build_CreditNoteYear();
                    if (hdnstrStatus.Value != "WAITING FOR CREDIT NOTE MOVE TO CRM")
                    {
                        SqlParameter[] sp = new SqlParameter[8];
                        sp[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                        sp[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                        sp[2] = new SqlParameter("@AccComments", txtCrmComments.Text.Replace("\r\n", "~"));
                        sp[3] = new SqlParameter("@AccCreatedBy", Request.Cookies["TTSUser"].Value);
                        sp[4] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                        sp[5] = new SqlParameter("@CreditNoteFile", lblClaimNo.Text + ".pdf");
                        sp[6] = new SqlParameter("@CreditNoteYear", hdnstrcredityear.Value);
                        sp[7] = new SqlParameter("@ClaimType", srtchar);
                        hdnCreditNote.Value = (string)daCOTS.ExecuteScalar_SP("sp_update_Service_claim_Credit_perpared", sp);

                        if (btnSAVE.Text == "CERDIT NOTE REPREPARATION")
                        {
                            SqlParameter[] spdel = new SqlParameter[2];
                            spdel[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                            spdel[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                            daCOTS.ExecuteNonQuery_SP("sp_Del_Service_Claim_Discount", spdel);
                        }
                        for (int i = 0; i < gvClaimOtherCharges.Rows.Count; i++)
                        {
                            string DESC = ((TextBox)gvClaimOtherCharges.Rows[i].FindControl("txtClaimAddDesc")).Text.Trim();
                            string AMT = ((TextBox)gvClaimOtherCharges.Rows[i].FindControl("txtClaimAddAmt")).Text.Trim();
                            string ddlCalcType = ((DropDownList)gvClaimOtherCharges.Rows[i].FindControl("ddlClaimCalcType")).SelectedItem.Value.Trim();
                            if (DESC != "" && AMT != "")
                            {
                                SqlParameter[] sp2 = new SqlParameter[7];
                                sp2[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                                sp2[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                                sp2[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                                sp2[3] = new SqlParameter("@DescrDiscount", DESC);
                                sp2[4] = new SqlParameter("@Category", ddlCalcType);
                                sp2[5] = new SqlParameter("@Amount", AMT);
                                sp2[6] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_Service_Claim_Discount", sp2);
                            }
                        }
                        string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                        if (!Directory.Exists(serverURL + "/ServiceCreditNote/" + hdnCustCode.Value + "/"))
                            Directory.CreateDirectory(serverURL + "/ServiceCreditNote/" + hdnCustCode.Value + "/");
                        string path = serverURL + "/ServiceCreditNote/" + hdnCustCode.Value + "/";
                        string sPathToWritePdfTo = string.Empty;
                        sPathToWritePdfTo = path + lblClaimNo.Text + ".pdf";
                        Document document = new Document(PageSize.A4, 22f, 14f, 18f, 18f);
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sPathToWritePdfTo, FileMode.Create));
                        document.Open();
                        document.Add(Build_CreditNoteDetails(document, "CREDIT NOTE"));
                        document.Close();
                        if (btnSAVE.Text != "CERDIT NOTE REPREPARATION")
                            ins_history("CREDIT NOTE PREPARATION");
                        else
                            ins_history("CREDIT NOTE REPREPARATION");
                        gv_bind();
                        gvClaimServiceList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                        Build_SelectClaimDetails(gvClaimServiceList.SelectedRow);
                    }
                    else if (hdnstrStatus.Value == "WAITING FOR CREDIT NOTE MOVE TO CRM")
                    {
                        SqlParameter[] sp = new SqlParameter[4];
                        sp[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                        sp[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                        sp[2] = new SqlParameter("@CreditMoveCommets", txtCrmComments.Text.Replace("\r\n", "~"));
                        sp[3] = new SqlParameter("@CreditMovedBy ", Request.Cookies["TTSUser"].Value);
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_update_Service_claim_Acc_Move_crm", sp);
                        if (resp > 0)
                        {
                            ins_history("MOVE TO CRM APPROVAL");
                        }
                        gv_bind();
                    }
                }
                else if (Request["pid"].ToString() == "4")
                {
                    SqlParameter[] sp = new SqlParameter[4];
                    sp[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    sp[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                    sp[2] = new SqlParameter("@CrmAppComments", txtCrmComments.Text.Replace("\r\n", "~"));
                    sp[3] = new SqlParameter("@CrmApprovedBy", Request.Cookies["TTSUser"].Value);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_update_Service_claim_move_Settlement", sp);
                    if (resp > 0)
                    {
                        ins_history("CRM APPROVAL");
                    }
                    gv_bind();
                }
                else if (Request["pid"].ToString() == "5")
                {
                    SqlParameter[] sp = new SqlParameter[5];
                    sp[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    sp[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                    sp[2] = new SqlParameter("@InvoiceNo", txtSettleInvoiceNo.Text.Trim());
                    sp[3] = new SqlParameter("@SettlementComments ", txtCrmComments.Text.Replace("\r\n", "~"));
                    sp[4] = new SqlParameter("@SettlementBy ", Request.Cookies["TTSUser"].Value);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_update_Service_claim_Settled", sp);
                    if (resp > 0)
                    {
                        ins_history("SETTLEMENT");
                    }
                    gv_bind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ins_history(string Revisetype)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@Comments", txtCrmComments.Text.Replace("\r\n", "~"));
                sp1[3] = new SqlParameter("@ReviseType", Revisetype);
                sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                daCOTS.ExecuteNonQuery_SP("sp_ins_ClaimServiceRevisedHistory", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkshow_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
            Build_SelectClaimDetails(clickedRow);
            ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoPreviewDiv('divMaindisplay');", true);
        }
        private void Build_SelectClaimDetails(GridViewRow clickedRow)
        {
            txtCrmComments.Text = "";
            lblHistoryHead.Text = "";
            gvhistory.DataSource = null;
            gvhistory.DataBind();

            lblClaimCustName.Text = clickedRow.Cells[0].Text.Trim();
            lblClaimNo.Text = clickedRow.Cells[1].Text.Trim();
            HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;
            hdnCustCode.Value = hdnClaimCustCode.Value;
            HiddenField hdnCreditNoteYear = clickedRow.FindControl("hdnCreditNoteYear") as HiddenField;
            HiddenField hdnCreditNoteNo = clickedRow.FindControl("hdnCreditNoteNo") as HiddenField;
            HiddenField hdnCreditNoteFile = clickedRow.FindControl("hdnCreditNoteFile") as HiddenField;
            hdnCustCode.Value = hdnClaimCustCode.Value;
            hdnCreditNote.Value = hdnCreditNoteNo.Value;
            string strCreditNo = string.Empty;
            if (Request["pcid"].ToString() == "E")
                strCreditNo = "EXP/SCN-" + hdnCreditNoteNo.Value + "/" + hdnCreditNoteYear.Value;
            else if (Request["pcid"].ToString() == "D")
                strCreditNo = "DOM/SCN-" + hdnCreditNoteNo.Value + "/" + hdnCreditNoteYear.Value;
            lnkCrditNoteDownload.Text = hdnCreditNoteFile.Value;
            lblcreditNO.Text = hdnCreditNoteNo.Value == "" ? "" : "<br/><span class='headCss'>CREDIT NOTE NO & FILE: </span><br/>" + strCreditNo.Replace("~", "<br/>");
            if (Request["pid"].ToString() != "0")
            {
                if (Request["pid"].ToString() == "2")
                {
                    lblCommentsHead.Text = "ANY COMMETNS TO CREDIT NOTE PREPARATION";
                    btnSAVE.Text = "MOVE TO CERDIT NOTE PREPARATION";
                }
                else if (Request["pid"].ToString() == "3")
                {
                    hdnstrStatus.Value = clickedRow.Cells[5].Text.Trim();
                    if (hdnstrStatus.Value != "WAITING FOR CREDIT NOTE MOVE TO CRM")
                    {
                        btnSAVE.Text = "CERDIT NOTE PREPARATION";
                        DataTable dtgv1 = new DataTable();
                        dtgv1.Columns.Add("slno", typeof(String));
                        dtgv1.Columns.Add("DescrDiscount", typeof(String));
                        dtgv1.Columns.Add("Amount", typeof(String));
                        for (int i = 1; i <= 3; i++)
                            dtgv1.Rows.Add(i);
                        gvClaimOtherCharges.DataSource = dtgv1;
                        gvClaimOtherCharges.DataBind();
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "gotoPreviewDiv('divsubcrpre');", true);
                        if (hdnstrStatus.Value == "WAITING FOR CREDIT NOTE RECHECK")
                        {
                            SqlParameter[] sp2 = new SqlParameter[2];
                            sp2[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                            sp2[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                            DataTable dtClaimDiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Service_Claim_Discount", sp2, DataAccess.Return_Type.DataTable);
                            if (dtClaimDiscount.Rows.Count < 3)
                            {
                                for (int i = dtClaimDiscount.Rows.Count; i < 3; i++)
                                    dtClaimDiscount.Rows.Add("", "", 0.00);
                            }
                            gvClaimOtherCharges.DataSource = dtClaimDiscount;
                            gvClaimOtherCharges.DataBind();
                            btnSAVE.Text = "CERDIT NOTE REPREPARATION";
                        }
                        lblCommentsHead.Text = "CREDIT NOTE PREPARATION COMMENTS";
                    }
                    else if (hdnstrStatus.Value == "WAITING FOR CREDIT NOTE MOVE TO CRM")
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "gotoPreviewDiv('divbtn');", true);
                        btnSAVE.Text = "MOVE TO CRM FOR CREDIT NOTE APPROVAL";
                        lblCommentsHead.Text = "ANY COMMETNS TO CRM";
                    }
                }
                else if (Request["pid"].ToString() == "4")
                {
                    lblCommentsHead.Text = "ANY COMMETNS TO A&F";
                    btnSAVE.Text = "MOVE TO SETTLEMENT";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "gotoPreviewDiv('divcrmappsub');", true);
                }
                else if (Request["pid"].ToString() == "5")
                {
                    lblCommentsHead.Text = "ANY COMMETNS";
                    btnSAVE.Text = "SAVE SETTLEMENT DETAILS & CLOSE CLAIM";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "gotoPreviewDiv('divBtnSettlement');", true);
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow6", "gotoPreviewDiv('subcrmopi');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "gotoPreviewDiv('divbtn');", true);
            }
            SqlParameter[] sphis = new SqlParameter[2];
            sphis[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
            sphis[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
            DataTable dtsphis = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimServiceRevisedHistory", sphis, DataAccess.Return_Type.DataTable);
            if (dtsphis.Rows.Count > 0)
            {
                gvhistory.DataSource = dtsphis;
                gvhistory.DataBind();
                lblHistoryHead.Text = "SERVICE COMPLAINT HISTORY";
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoPreviewDiv('divMaindisplay');", true);

        }
        private string Build_CreditNoteYear()
        {
            string acYearFormat = string.Empty;
            if (DateTime.Today.Month < 4)
            {
                string nextyear = DateTime.Today.Year.ToString();
                acYearFormat = (DateTime.Today.Year - 1).ToString() + '-' + nextyear.Substring(2, 2);
            }
            else if (DateTime.Today.Month > 3)
            {
                string nextyear = (DateTime.Today.Year + 1).ToString();
                acYearFormat = DateTime.Today.Year.ToString() + '-' + nextyear.Substring(2, 2);
            }
            return acYearFormat;
        }
        protected void btnrecheck_Click(object sender, EventArgs e)
        {
            SqlParameter[] sp = new SqlParameter[4];
            sp[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
            sp[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
            sp[2] = new SqlParameter("@RecheckCommets", txtCrmComments.Text.Replace("\r\n", "~"));
            sp[3] = new SqlParameter("@RecheckBy ", Request.Cookies["TTSUser"].Value);
            int resp = daCOTS.ExecuteNonQuery_SP("sp_update_Service_claim_Recheck", sp);
            if (resp > 0)
            {
                ins_history("CREDIT NOTE RECHECK");
                gv_bind();
            }
        }
        protected void lnkCrditNoteDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/ServiceCreditNote/" + hdnCustCode.Value + "/" + lnkTxt.Text;
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private PdfPTable Build_CreditNoteDetails(Document doc, string strFileHead)
        {
            var contentFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 520f;
            table.LockedWidth = true;
            float[] widths = new float[] { 15f, 15f };
            table.SetWidths(widths);
            try
            {
                Build_ExporterAddress(table, strFileHead);
                Build_CreditNoteItems(table);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return table;
        }
        private void Build_ExporterAddress(PdfPTable table, string strFileType)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 15, Font.BOLD);
                var titleFont1 = FontFactory.GetFont("Arial", 9, Font.BOLD);
                var valFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                table.AddCell(Build_HeadTableColsPan("\n\n\n", 1, 2));

                string imageFilePath = Server.MapPath("~/images/tvs_suntws.jpg");
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageFilePath);
                img.ScaleToFit(165f, 39f);
                PdfPCell cell1 = new PdfPCell(img);
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.Padding = 0f;
                cell1.PaddingTop = 5f;
                cell1.PaddingBottom = 5f;
                table.AddCell(cell1);

                Phrase para = new Phrase();
                string AddressTxt = string.Empty;
                var headFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                //Chunk postCell1 = new Chunk("T.S.RAJAM TYRES PRIVATE LIMITED\n", headFont);
                Chunk postCell1 = new Chunk("SUNDARAM INDUSTRIES PRIVATE LIMITED\n", headFont);//*************To Be inserted  On 14-02-2022***********************
                var subHead = FontFactory.GetFont("Arial", 6, Font.BOLD);
                Chunk postCell2 = new Chunk("(Formerly known as T.S.RAJAM TYRES PRIVATE LIMITED)\n", subHead);//*********************
                para.Add(postCell1);
                para.Add(postCell2);
                AddressTxt += "180, Anna Salai, Chennai - 600 006, India.\n";
                AddressTxt += "Tel. Off  : 91-44-2852 5289 / 2852 0781\n";
                AddressTxt += "Fax        : 91-20-26068388\n";
                AddressTxt += "www.sun-tws.com";
                Chunk postCell3 = new Chunk(AddressTxt, valFont);
                para.Add(postCell3);
                PdfPCell Cell1 = new PdfPCell(new Phrase(para));
                Cell1.ExtraParagraphSpace = 3f;
                table.AddCell(Cell1);

                Cell1 = new PdfPCell(new Phrase(strFileType.ToUpper(), titleFont));
                Cell1.Padding = 5f;
                Cell1.Colspan = 2;
                Cell1.HorizontalAlignment = 1;
                table.AddCell(Cell1);

                string strCustAddress = string.Empty;
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                DataTable dtNoteAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CreditNote_Address", sp1, DataAccess.Return_Type.DataTable);
                if (dtNoteAddress.Rows.Count > 0)
                {
                    strCustAddress += "M/S." + dtNoteAddress.Rows[0]["CompanyName"].ToString() + "\n";
                    strCustAddress += dtNoteAddress.Rows[0]["shipaddress"].ToString().Replace("~", "\n") + "\n";
                    strCustAddress += dtNoteAddress.Rows[0]["city"].ToString() + ", " + dtNoteAddress.Rows[0]["statename"].ToString() + "\n";
                    strCustAddress += dtNoteAddress.Rows[0]["country"].ToString() + " - " + dtNoteAddress.Rows[0]["zipcode"].ToString() + "\n";
                    hdnCreditCurrency.Value = dtNoteAddress.Rows[0]["usercurrency"].ToString();
                }
                Cell1 = new PdfPCell(new Phrase(strCustAddress, titleFont1));
                Cell1.ExtraParagraphSpace = 3f;
                table.AddCell(Cell1);

                PdfPTable nestedtable = new PdfPTable(2);
                float[] width = new float[] { 5f, 25f };
                nestedtable.SetWidths(width);
                PdfPCell Cell = new PdfPCell(new Phrase("NO.      :", titleFont1));
                Cell.Border = Rectangle.NO_BORDER;
                nestedtable.AddCell(Cell);

                string strCreditNo = string.Empty;
                if (Request["pcid"].ToString() == "E")
                    strCreditNo = "EXP/SCN-" + hdnCreditNote.Value + "/" + hdnstrcredityear.Value;
                else if (Request["pcid"].ToString() == "D")
                    strCreditNo = "DOM/SCN-" + hdnCreditNote.Value + "/" + hdnstrcredityear.Value;
                Cell = new PdfPCell(new Phrase(strCreditNo, titleFont1));
                Cell.Border = Rectangle.RIGHT_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("DATE  :", titleFont1));
                Cell.Border = Rectangle.NO_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy"), titleFont1));
                Cell.Border = Rectangle.RIGHT_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("REF.    :", titleFont1));
                Cell.Border = Rectangle.NO_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(lblClaimNo.Text, titleFont1));
                Cell.Border = Rectangle.RIGHT_BORDER;
                nestedtable.AddCell(Cell);
                PdfPCell nesthousing = new PdfPCell(nestedtable);
                nesthousing.Padding = 0f;
                table.AddCell(nesthousing);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_CreditNoteItems(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                PdfPTable dt = new PdfPTable(2);
                dt.TotalWidth = 520f;
                dt.LockedWidth = true;
                float[] widths = new float[] { 15f, 15f };
                dt.SetWidths(widths);
                dt.AddCell(Build_ItemTableHeadStyle("PARTICULARS"));
                dt.AddCell(Build_ItemTableHeadStyle("TOTAL UNITPRICE\n(" + hdnCreditCurrency.Value + ")"));
                dt.AddCell(Build_ItemTableListStyle("TO AGREED AMOUNT PAYABLE TO YOUR COMPANY :", 0, 1));
                Bind_itemsTableEmptyRow(dt, 1);
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                sp2[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                DataTable dtClaimDiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Service_Claim_Discount", sp2, DataAccess.Return_Type.DataTable);
                decimal discount = 0;
                if (dtClaimDiscount.Rows.Count > 0)
                {
                    for (int i = 0; i < dtClaimDiscount.Rows.Count; i++)
                    {
                        dt.AddCell(Build_ItemTableListStyle(dtClaimDiscount.Rows[i]["Category"].ToString() + ": " + dtClaimDiscount.Rows[i]["DescrDiscount"].ToString(), 0, 1));
                        dt.AddCell(Build_ItemTableListStyle(dtClaimDiscount.Rows[i]["Amount"].ToString(), 2, 1));
                        discount += Convert.ToDecimal(dtClaimDiscount.Rows[i]["Amount"].ToString());
                    }
                }
                for (int j = 0; j < 3; j++)
                {
                    Bind_itemsTableEmptyRow(dt, 2);
                }
                dt.AddCell(Build_ItemTableBottomStyle("TOTAL", 2, 0));
                dt.AddCell(Build_ItemTableBottomStyle(discount.ToString(), 2, 0));

                PdfPCell nesthousing1 = new PdfPCell(dt);
                nesthousing1.Padding = 0f;
                nesthousing1.Colspan = 2;
                table.AddCell(nesthousing1);

                var valFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                string[] strSplit = discount.ToString().Split('.');
                int intPart = Convert.ToInt32(strSplit[0].ToString());
                string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
                string text = Utilities.NumberToText(intPart, true, false).ToString();
                if (Convert.ToDecimal(decimalPart) > 0)
                    text += " Point " + Utilities.DecimalToText(decimalPart) + " Only";
                else
                    text += " Only";

                PdfPCell Cell1 = new PdfPCell(new Phrase("(" + hdnCreditCurrency.Value.ToUpper() + ") " + text.ToUpper(), titleFont));
                Cell1.HorizontalAlignment = 0;
                table.AddCell(Cell1);

                //Cell1 = new PdfPCell(new Phrase("For T.S.RAJAM TYRES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory", titleFont));
                Cell1 = new PdfPCell(new Phrase("For SUNDARAM INDUSTRIES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory", titleFont));//*************To Be inserted  On 14-02-2022***********************
                Cell1.HorizontalAlignment = 1;
                table.AddCell(Cell1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private static void Bind_itemsTableEmptyRow(PdfPTable dt, int maxVal)
        {
            for (int j = 0; j < maxVal; j++)
            {
                PdfPCell cell = new PdfPCell(new Phrase("\n"));
                cell.Border = Rectangle.RIGHT_BORDER;
                dt.AddCell(cell);
            }
        }
        private PdfPCell Build_ItemTableListStyle(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = Rectangle.RIGHT_BORDER;
            //cell.FixedHeight = 18f;
            return cell;
        }
        private PdfPCell Build_ItemTableBottomStyle(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            return cell;
        }
        private static PdfPCell Build_ItemTableHeadStyle(string strText)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }
        private static PdfPCell Build_ItemTablePlantHeadStyle(string strText, int itemCont)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.UNDERLINE);
            if (itemCont == 1)
                titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = Rectangle.RIGHT_BORDER;
            return cell;
        }
        private static PdfPCell Build_HeadTableColsPan(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = Rectangle.NO_BORDER;
            cell.Colspan = colspan;
            return cell;
        }
    }
}