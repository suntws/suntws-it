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
using System.Text;

namespace TTS
{
    public partial class claimcreditapprove : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["claim_crm_exp"].ToString() == "True" || dtUser.Rows[0]["claim_crm_dom"].ToString() == "True"))
                        {
                            SqlParameter[] sp1 = new SqlParameter[1];
                            sp1[0] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CreditNoteApproval_List", sp1, DataAccess.Return_Type.DataTable);
                            if (Request["pid"].ToString().ToUpper() == "D")
                            {
                                SqlParameter[] spME = new SqlParameter[1];
                                spME[0] = new SqlParameter("@ClaimType", "E");
                                DataTable dtME = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CreditNoteApproval_List_MerchantExport", spME, DataAccess.Return_Type.DataTable);
                                if (dtME.Rows.Count > 0)
                                    dt.Merge(dtME);
                            }
                            gvComplaintList.DataSource = dt;
                            gvComplaintList.DataBind();
                            lblPageHead.Text = "CLAIM CRM APPROVAL CREDIT NOTE LIST";
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
        protected void lnkClaimNo_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Bind_ApprovalClaimClick(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ApprovalClaimClick(GridViewRow clickedRow)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen1", "hideClaimApprove();", true);
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                lblCreditNoteNo.Text = clickedRow.Cells[4].Text;
                hdnConditionalStatus.Value = clickedRow.Cells[5].Text;
                HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;
                HiddenField hdnCreditNoteNo = clickedRow.FindControl("hdnCreditNoteNo") as HiddenField;
                hdnCreditNote.Value = hdnCreditNoteNo.Value;
                HiddenField hdnCreditFileName = clickedRow.FindControl("hdnCreditFileName") as HiddenField;
                lnkCrditNoteDownload.Text = hdnCreditFileName.Value;

                if (lblClaimCustName.Text != "" && lblClaimNo.Text != "")
                    lblClaim.Text = "-";
                hdnCustCode.Value = hdnClaimCustCode.Value;
                if (hdnConditionalStatus.Value == "CONDITIONALLY APPROVED") btnApproveCreditNote.Text = "CREDIT NOTE CONDITIONALLY APPROVE";
                else btnApproveCreditNote.Text = "SAVE CREDIT NOTE APPROVAL";
                gvClaimApproveItems.DataSource = null;
                gvClaimApproveItems.DataBind();

                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmOpinionItems_settlement", sp1, DataAccess.Return_Type.DataTable);
                if (dtItems.Rows.Count > 0) { gvClaimApproveItems.DataSource = dtItems; gvClaimApproveItems.DataBind(); }
                sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                DataTable dtComments = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_CreditApproval_MoveComments", sp1, DataAccess.Return_Type.DataTable);
                if (dtComments.Rows.Count > 0)
                {
                    if (hdnConditionalStatus.Value == "CONDITIONALLY APPROVED")
                    {
                        lblCreditMovedComments.Text = "<span class='headCss'>COMMENTS: </span>" + dtComments.Rows[0]["CondAccComments"].ToString().Replace("\r\n", "~");
                        lblCreditMovedUser.Text = "<span class='headCss'>BY: </span>" + dtComments.Rows[0]["CondAccBy"].ToString() + "  " + dtComments.Rows[0]["CondAccDate"].ToString();
                    }
                    else
                    {
                        lblCreditMovedComments.Text = "<span class='headCss'>COMMENTS: </span>" + dtComments.Rows[0]["CreditNoteComments"].ToString().Replace("\r\n", "~");
                        lblCreditMovedUser.Text = "<span class='headCss'>BY: </span>" + dtComments.Rows[0]["CreditMoveUser"].ToString() + "  " + dtComments.Rows[0]["CreditMoveDate"].ToString();
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen2", "showClaimApprove();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen21", "showPriceOption();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkCrditNoteDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/CreditNote/" + hdnCustCode.Value + "/" + lnkTxt.Text;

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnApproveCreditNote_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@CreditApproveComments", txtCreditApproveComments.Text.Replace("\r\n", "~"));
                sp1[3] = new SqlParameter("@CreditApprovedUser", Request.Cookies["TTSUser"].Value);
                sp1[4] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                if (hdnConditionalStatus.Value != "CONDITIONALLY APPROVED")
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_CreditNoteApproved", sp1);
                else
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_CreditNoteApproved_conditional", sp1);
                if (resp > 0)
                {
                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                    DataTable dtClaim = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_DistinctPlant_CreditNoteWise", sp2, DataAccess.Return_Type.DataTable);
                    if (dtClaim.Rows.Count > 0)
                    {
                        string strCCMail = string.Empty;
                        foreach (DataRow row in dtClaim.Rows)
                        {
                            if (strCCMail.Length > 0)
                                strCCMail += "," + Build_NotificationMailID(row["plant"].ToString());
                            else
                                strCCMail = Build_NotificationMailID(row["plant"].ToString());
                            Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, row["plant"].ToString(), "28", txtCreditApproveComments.Text, Request.Cookies["TTSUser"].Value);
                        }
                    }
                    Response.Redirect("default.aspx", false);
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSettlePrice_Click(object sender, EventArgs e)
        {
            try
            {
                int resp1 = 0;
                DataTable dt = new DataTable();
                DataColumn c1 = new DataColumn("CurrentPrice", typeof(System.Decimal));
                dt.Columns.Add(c1);
                c1 = new DataColumn("PreviewPrice", typeof(System.Decimal));
                dt.Columns.Add(c1);
                c1 = new DataColumn("stencilno", typeof(System.String));
                dt.Columns.Add(c1);
                c1 = new DataColumn("TyrePrice", typeof(System.Decimal));
                dt.Columns.Add(c1);
                foreach (GridViewRow row in gvClaimApproveItems.Rows)
                {
                    string strStencil = row.Cells[4].Text;
                    string strStatus = row.Cells[6].Text;
                    HiddenField hdnClaimPrice = row.FindControl("hdnclaimprice") as HiddenField;
                    TextBox txtClaimPrice = row.FindControl("txtClaimPrice") as TextBox;
                    TextBox txtTyrePrice = row.FindControl("txtTyrePrice") as TextBox;
                    if (strStencil != "" && strStatus == "ACCEPT" && txtClaimPrice.Text != "" && txtTyrePrice.Text != "")
                        dt.Rows.Add(Convert.ToDecimal(txtClaimPrice.Text), Convert.ToDecimal(hdnClaimPrice.Value), strStencil, Convert.ToDecimal(txtTyrePrice.Text));
                }

                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@ClaimUnitPrice", dt);
                sp1[3] = new SqlParameter("@AcUser", Request.Cookies["TTSUser"].Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_accounts_update_claimprice", sp1);
                if (resp > 0)
                {
                    SqlParameter[] sp2 = new SqlParameter[4];
                    sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[2] = new SqlParameter("@ClaimUnitPrice", dt);
                    sp2[3] = new SqlParameter("@User", Request.Cookies["TTSUser"].Value);
                    daCOTS.ExecuteNonQuery_SP("Sp_ins_ClaimPriceHistory", sp2);
                }
                SqlParameter[] sp = new SqlParameter[4];
                sp[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                sp[3] = new SqlParameter("@crmReprepareComments", txtCreditApproveComments.Text.Replace("\r\n", "~"));
                if (hdnConditionalStatus.Value == "CONDITIONALLY APPROVED")
                    resp1 = daCOTS.ExecuteNonQuery_SP("sp_update_creditNoteReperparation_conditional", sp);
                else
                    resp1 = daCOTS.ExecuteNonQuery_SP("sp_update_creditNoteReperparation", sp);
                if (resp1 > 0)
                {
                    Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnCreditNote.Value, "27", txtCreditApproveComments.Text, Request.Cookies["TTSUser"].Value);
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPriceChangeDisable_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen2", "showClaimApprove();", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen21", "hideClaimAccounts();", true);
        }
        private string Build_NotificationMailID(string strPlant)
        {
            string returnValue = string.Empty;

            if (strPlant == "MMN")
            {
                if (returnValue.Length > 0)
                    returnValue += "," + Utilities.Build_Cliam_ToList("claim_edc_mmn");
                else
                    returnValue = Utilities.Build_Cliam_ToList("claim_edc_mmn");
                if (returnValue.Length > 0)
                    returnValue += "," + Utilities.Build_Cliam_ToList("claim_qc_mmn");
                else
                    returnValue = Utilities.Build_Cliam_ToList("claim_qc_mmn");
            }
            else if (strPlant == "SLTL")
            {
                if (returnValue.Length > 0)
                    returnValue += "," + Utilities.Build_Cliam_ToList("claim_edc_sltl");
                else
                    returnValue = Utilities.Build_Cliam_ToList("claim_edc_sltl");
                if (returnValue.Length > 0)
                    returnValue += "," + Utilities.Build_Cliam_ToList("claim_qc_sltl");
                else
                    returnValue = Utilities.Build_Cliam_ToList("claim_qc_sltl");
            }
            else if (strPlant == "SITL")
            {
                if (returnValue.Length > 0)
                    returnValue += "," + Utilities.Build_Cliam_ToList("claim_edc_sitl");
                else
                    returnValue = Utilities.Build_Cliam_ToList("claim_edc_sitl");
                if (returnValue.Length > 0)
                    returnValue += "," + Utilities.Build_Cliam_ToList("claim_qc_sitl");
                else
                    returnValue = Utilities.Build_Cliam_ToList("claim_qc_sitl");
            }
            if (strPlant == "PDK")
            {
                if (returnValue.Length > 0)
                    returnValue += "," + Utilities.Build_Cliam_ToList("claim_edc_pdk");
                else
                    returnValue = Utilities.Build_Cliam_ToList("claim_edc_pdk");
                if (returnValue.Length > 0)
                    returnValue += "," + Utilities.Build_Cliam_ToList("claim_qc_pdk");
                else
                    returnValue = Utilities.Build_Cliam_ToList("claim_qc_pdk");
            }
            return returnValue;
        }

        protected void gvComplaintList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}