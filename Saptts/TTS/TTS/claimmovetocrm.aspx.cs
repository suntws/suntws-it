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
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
namespace TTS
{
    public partial class claimmovetocrm : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        bool pendingReopinion = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["claim_edc_mmn"].ToString() == "True" ||
                            dtUser.Rows[0]["claim_edc_sltl"].ToString() == "True" || dtUser.Rows[0]["claim_edc_sitl"].ToString() == "True" ||
                            dtUser.Rows[0]["claim_edc_pdk"].ToString() == "True"))
                        {
                            ddlEdcPlant.Items.Insert(0, "CHOOSE");
                            if (dtUser.Rows[0]["claim_edc_mmn"].ToString() == "True")
                                ddlEdcPlant.Items.Insert(ddlEdcPlant.Items.Count, "MMN");
                            if (dtUser.Rows[0]["claim_edc_sltl"].ToString() == "True")
                                ddlEdcPlant.Items.Insert(ddlEdcPlant.Items.Count, "SLTL");
                            if (dtUser.Rows[0]["claim_edc_sitl"].ToString() == "True")
                                ddlEdcPlant.Items.Insert(ddlEdcPlant.Items.Count, "SITL");
                            if (dtUser.Rows[0]["claim_edc_pdk"].ToString() == "True")
                                ddlEdcPlant.Items.Insert(ddlEdcPlant.Items.Count, "PDK");

                            if (ddlEdcPlant.Items.Count == 2)
                            {
                                ddlEdcPlant.SelectedIndex = 1;
                                ddlEdcPlant_IndexChanged(sender, e);
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
        protected void ddlEdcPlant_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "NO RECORDS";
                gvClaimNoList.DataSource = null;
                gvClaimNoList.DataBind();
                hdnStencilPlant.Value = ddlEdcPlant.SelectedItem.Text;
                if (hdnStencilPlant.Value != "" && hdnStencilPlant.Value != "CHOOSE")
                {
                    lblPageHead.Text = "CLAIM " + hdnStencilPlant.Value + " EDC OPINION";
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimNoList_ForEDC", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        gvClaimNoList.DataSource = dt;
                        gvClaimNoList.DataBind();
                        lblErr.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkClaimNo_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            ClaimNoClick(clickedRow);
            hdnClaimNoClick.Value = Convert.ToString(clickedRow.RowIndex);
            ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoClaimDiv('divclaimitems');", true);
        }
        private void Complaint_Description()
        {
            DataTable dtComplaintype = (DataTable)daCOTS.ExecuteReader_SP("sp_ddl_Complainttype_ClaimLibrary", DataAccess.Return_Type.DataTable);
            ddlComplaintDesc.DataSource = dtComplaintype;
            ddlComplaintDesc.DataTextField = "Complaintype";
            ddlComplaintDesc.DataValueField = "Complaintype";
            ddlComplaintDesc.DataBind();
            ddlComplaintDesc.Items.Insert(0, "CHOOSE");
            ddlComplaintDesc.Items.Insert(1, "ADD NEW ENTRY");
        }
        private void ClaimNoClick(GridViewRow clickedRow)
        {
            try
            {
                lblMsgForEdcAddtionalDetails.Text = "";
                lblRechkMsg.Text = "";
                hdnselectedrow.Value = "";
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                hdnStatus.Value = clickedRow.Cells[4].Text.Trim();
                HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;
                if (lblClaimCustName.Text != "" && lblClaimNo.Text != "")
                    lblClaim.Text = "-";
                hdnCustCode.Value = hdnClaimCustCode.Value;
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[2] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FullList_ClaimNoWise", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvClaimItems.DataSource = dt;
                    gvClaimItems.DataBind();

                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                    DataTable dtComm = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complaintcommets", sp2, DataAccess.Return_Type.DataTable);
                    lblComplaintComments.Text = ""; lblQcMovedUser.Text = ""; lblQcComments.Text = "";
                    if (dtComm.Rows.Count > 0)
                    {
                        if (dtComm.Rows[0]["Commets"].ToString() != "")
                            lblComplaintComments.Text = "<span class='headCss'>CUSTOMER COMMENTS:</span><br/>" + dtComm.Rows[0]["Commets"].ToString().Replace("~", "<br/>");
                        if (dtComm.Rows[0]["QcAnalysiscomments"].ToString() != "")
                        {
                            lblQcComments.Text = "<br/><span class='headCss'>QC COMMENTS:</span><br/>" + dtComm.Rows[0]["QcAnalysiscomments"].ToString().Replace("~", "<br/>");
                            lblQcMovedUser.Text = "<br/><span class='headCss'>BY: </span>" + dtComm.Rows[0]["QcAnalysisuser"].ToString();
                        }
                    }

                    if (hdnStatus.Value != "WAITING FOR EDC RE-OPINION")
                    {
                        bool assystatus = false;
                        foreach (DataRow dr in dt.Select("AnalysisStatus='Recheck' or EdcAdditionalStatus='EDC REQUEST'"))
                        {
                            assystatus = true;
                            break;
                        }
                        if (!assystatus)
                        {
                            int k = dt.AsEnumerable().Where(p => p["ConclusionStatus"].ToString() == "").Count();
                            if (k == 0)
                                ScriptManager.RegisterStartupScript(Page, GetType(), "status1", "showAnalysisData('tdAnalysisData');", true);
                        }
                        else
                        {
                            int j = 0;
                            j = dt.AsEnumerable().Where(p => p.Field<string>("AnalysisStatus") == "Recheck").Count();
                            if (j > 0)
                            {
                                lblRechkMsg.Text = "You have chosen " + j + " stencil no. for reanalysis";
                                ScriptManager.RegisterStartupScript(Page, GetType(), "status1", "showAnalysisData('tdQcRecheck');", true);
                            }
                            j = dt.AsEnumerable().Where(p => p.Field<string>("EdcAdditionalStatus") == "EDC REQUEST").Count();
                            if (j > 0)
                            {
                                lblMsgForEdcAddtionalDetails.Text = "You have chosen " + j + " stencil no. for additional details asked from CRM";
                                ScriptManager.RegisterStartupScript(Page, GetType(), "status1", "showAnalysisData('tdEdcAdditional');", true);
                            }
                        }
                    }
                    else
                    {
                        bool assystatus = false;
                        foreach (DataRow dr in dt.Select("Crm_EdcReOpnionStatus='Reopinion'"))
                        {
                            assystatus = true;
                            break;
                        }
                        if (assystatus)
                            ScriptManager.RegisterStartupScript(Page, GetType(), "showReopinion", "showDivEdcReopinion(1);", true);
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "status1", "showAnalysisData('tdAnalysisData');", true);
                            ScriptManager.RegisterStartupScript(Page, GetType(), "showReopinion", "showDivEdcReopinion(0);", true);
                            pendingReopinion = false;
                        }
                    }

                    // hide additional details if empty and reanalysis if empty. if not empty then disable. show crm_edc reopinion .

                    sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp1[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                    DataTable dtRootCause = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_EdcRootCause", sp1, DataAccess.Return_Type.DataTable);
                    ViewState["dtRootCause"] = dtRootCause;
                    ClaimAnalysisData();
                    if (hdnselectedrow.Value != "")
                    {
                        var previousRowIndex = Convert.ToInt32(hdnselectedrow.Value);
                        GridViewRow PreviousRow = gvClaimItems.Rows[previousRowIndex];
                        PreviousRow.BackColor = System.Drawing.Color.Yellow;
                        if (hdnselectedrow.Value != "0")
                        {
                            GridViewRow PreviousRow1 = gvClaimItems.Rows[0];
                            PreviousRow1.BackColor = System.Drawing.Color.White;
                        }
                    }
                    Conditional();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Conditional()
        {
            if (hdnStatus.Value == "WAITING FOR EDC OPINION" || hdnStatus.Value == "WAITING FOR EDC RE-OPINION")
            {
                if (hdnStatus.Value == "WITING FOR EDC RE-OPINION" && pendingReopinion == true) return;
                btnComplaintMoved.Text = "MOVE TO CRM FOR SETTLEMENT OPINION";
                rdbEdcConclusion.Enabled = true;
                divreanalysis.Style.Add("display", "block");
            }
            else
            {
                btnComplaintMoved.Text = "MOVE TO CRM FOR SETTLEMENT OPINION THROUGH CONDITIONALLY APPROVED";
                rdbEdcConclusion.Items.FindByText("Others").Selected = true; rdbEdcConclusion.Enabled = false;
                divreanalysis.Style.Add("display", "none");
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "ShowConclusionWise();", true);
            }
        }
        private void ClaimAnalysisData()
        {
            try
            {
                dgList.DataSource = null;
                dgList.DataBind();
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AnalysisData_EachStencil", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    dgList.DataSource = dt;
                    dgList.DataBind();
                    ViewState["dtData"] = dt;
                    gvClaimItems.Rows[0].BackColor = System.Drawing.Color.Yellow;
                    if (hdnselectedrow.Value == "")
                        hdnselectedrow.Value = "0";
                    Bind_EdcFinalStatement();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_EdcFinalStatement()
        {
            try
            {
                txtOtherEdcConclusion.Text = "";
                rdbEdcConclusion.SelectedIndex = -1;
                rdbClaimNoStatus.SelectedIndex = -1;
                Label lblAnalysedSetncil = dgList.Rows[0].FindControl("lblAnalysedSetncil") as Label;
                HiddenField hdnEdctype = dgList.Rows[0].FindControl("hdnEdctype") as HiddenField;
                HiddenField hdnCustType = dgList.Rows[0].FindControl("hdnCustType") as HiddenField;
                HiddenField hdnimageUrl = dgList.Rows[0].FindControl("hdnimageUrl") as HiddenField;
                Label lbltyretype = dgList.Rows[0].FindControl("lbltyretype") as Label;
                Label lblbrand = dgList.Rows[0].FindControl("lblbrand") as Label;
                Label lbltyresize = dgList.Rows[0].FindControl("lbltyresize") as Label;
                Label lblComplaintDesc = dgList.Rows[0].FindControl("lblComplaintDesc") as Label;
                lblEdcSetncil.Text = lblAnalysedSetncil.Text;
                lblCustType.Text = hdnCustType.Value == "" ? "" : "<span class='headCss'>CUSTOMER: </span>" + hdnCustType.Value;
                lblQcType.Text = lbltyretype.Text == "" ? "" : "<span class='headCss'>QC: </span>" + lbltyretype.Text;
                Label lblAnalyserRootCause = dgList.Rows[0].FindControl("lblAnalyserRootCause") as Label;
                txtEdcRootCause.Text = lblAnalyserRootCause.Text.Replace("<br/>", "\r\n");
                HiddenField hdnEdcReopinion = dgList.Rows[0].FindControl("hdnEdcReopinionComment") as HiddenField;
                HiddenField hdnEdcReopinionUpdate = dgList.Rows[0].FindControl("hdnEdcReopinionUpdateComments") as HiddenField;
                txtEdc_crmReopinion.Text = hdnEdcReopinionUpdate.Value;
                lblCrm_EdcReopinion.Text = hdnEdcReopinion.Value;
                hdnstencilurl.Value = hdnimageUrl.Value;
                Label lblEdcAdditionalReq = dgList.Rows[0].FindControl("lblEdcAdditionalReq") as Label;
                Label lblEdcAdditionalUpdates = dgList.Rows[0].FindControl("lblEdcAdditionalUpdates") as Label;
                lnkCancelRequest.Text = "";
                txtEdcAdditional.Text = lblEdcAdditionalReq.Text;
                txtEdcAdditional.Enabled = true;
                if (lblEdcAdditionalUpdates.Text != "")
                {
                    txtEdcAdditional.Text += "\r\nCRM:" + lblEdcAdditionalUpdates.Text;
                    txtEdcAdditional.Enabled = false;
                }

                HiddenField hdnReanalysisComment = dgList.Rows[0].FindControl("hdnReanalysisComment") as HiddenField;
                HiddenField hdnAnalysisStatus = dgList.Rows[0].FindControl("hdnAnalysisStatus") as HiddenField;
                txtReanalysis.Text = hdnReanalysisComment.Value;
                txtReanalysis.Enabled = true;
                if (hdnAnalysisStatus.Value == "Analysed" && txtReanalysis.Text != "") txtReanalysis.Enabled = false;

                if (txtEdcAdditional.Text != "" && txtEdcAdditional.Enabled == true) lnkCancelRequest.Text = "CANCEL ADDTIONAL DETAILS REQUEST";
                if (txtReanalysis.Text != "" && txtReanalysis.Enabled == true) lnkCancelRequest.Text = "CANCEL QC REANALYSIS";
                SqlParameter[] sp = new SqlParameter[2];
                sp[0] = new SqlParameter("@brand", lblbrand.Text);
                sp[1] = new SqlParameter("@TyreSize", lbltyresize.Text);
                DataTable dt1 = (DataTable)daTTS.ExecuteReader_SP("sp_sel_tyreType_Brand_Size_Wise", sp, DataAccess.Return_Type.DataTable);
                if (dt1.Rows.Count > 0)
                {
                    ddltype.DataSource = dt1;
                    ddltype.DataTextField = "TyreType";
                    ddltype.DataValueField = "TyreType";
                    ddltype.DataBind();
                    ddltype.Items.Insert(0, new ListItem("Choose", "Choose"));
                }
                ListItem selectedListItem = ddltype.Items.FindByText("" + hdnEdctype.Value == "" ? (lbltyretype.Text == "" ? "Choose" : lbltyretype.Text) : hdnEdctype.Value + "");

                selectedListItem = selectedListItem == null ? ddltype.Items.FindByText("Choose") : selectedListItem;
                if (selectedListItem != null)
                {
                    ddltype.Items.FindByText(ddltype.SelectedItem.Text).Selected = false;
                    selectedListItem.Selected = true;
                }
                Complaint_Description();
                ListItem selectedList = ddlComplaintDesc.Items.FindByText("" + lblComplaintDesc.Text + "");
                selectedList = selectedList == null ? ddlComplaintDesc.Items.FindByText("CHOOSE") : selectedList;
                if (selectedList != null)
                {
                    ddlComplaintDesc.Items.FindByText(ddlComplaintDesc.SelectedItem.Text).Selected = false;
                    selectedList.Selected = true;
                }
                Build_ClaimImages(lblEdcSetncil.Text, hdnCustCode.Value, lblClaimNo.Text);
                DataTable dtRootCause = ViewState["dtRootCause"] as DataTable;
                if (dtRootCause.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRootCause.Select("stencilno='" + lblEdcSetncil.Text + "'"))
                    {
                        txtEdcRootCause.Text = row["rootcause"].ToString().Replace("~", "\r\n");
                        txtOtherEdcConclusion.Text = row["conclusion"].ToString();
                        rdbEdcConclusion.Items.FindByText("" + row["QcConclusion"].ToString() + "").Selected = true;
                        if (row["StencilClaimStatus"].ToString() == "True")
                            rdbClaimNoStatus.Items.FindByValue("1").Selected = true;
                        else
                            rdbClaimNoStatus.Items.FindByValue("0").Selected = true;
                        txtEdcAdditional.Text = row["EdcAdditionalReq"].ToString().Replace("~", "\r\n");
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen4", "ShowConclusionWise();", true);
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen3", "showAnalysisData('divEDCFinalUpdate');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void dgList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                if (hdnselectedrow.Value != "")
                {
                    var previousRowIndex = Convert.ToInt32(hdnselectedrow.Value);
                    GridViewRow PreviousRow = gvClaimItems.Rows[previousRowIndex];
                    PreviousRow.BackColor = System.Drawing.Color.White;
                }
                var rowClick = e.NewPageIndex;
                gvClaimItems.Rows[rowClick].BackColor = System.Drawing.Color.Yellow;
                hdnselectedrow.Value = rowClick.ToString();
                DataTable dtData = ViewState["dtData"] as DataTable;
                dgList.DataSource = dtData;
                dgList.DataBind();
                dgList.PageIndex = e.NewPageIndex;
                dgList.DataBind();

                if (hdnStatus.Value == "WAITING FOR EDC RE-OPINION")
                {
                    bool assystatus = false;
                    foreach (DataRow dr in dtData.Select("Crm_EdcReOpnionStatus='Reopinion'"))
                    {
                        assystatus = true;
                        break;
                    }
                    if (!assystatus)
                    {
                        //int k = dtData.AsEnumerable().Where(p => p["QcConclusion"].ToString() == "").Count();
                        //if (k == 0)
                        if (dtData.Rows[e.NewPageIndex]["Crm_EdcReOpnionStatus"].ToString() != "")
                            ScriptManager.RegisterStartupScript(Page, GetType(), "showReopinion", "showDivEdcReopinion(1);", true);
                        else
                            ScriptManager.RegisterStartupScript(Page, GetType(), "showReopinion", "showDivEdcReopinion(0);", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "status1", "showAnalysisData('tdAnalysisData');", true);
                        //ScriptManager.RegisterStartupScript(Page, GetType(), "showReopinion", "showDivEdcReopinion(0);", true);
                    }
                    else
                    {
                        if (dtData.Rows[e.NewPageIndex]["Crm_EdcReOpnionStatus"].ToString() != "")
                            ScriptManager.RegisterStartupScript(Page, GetType(), "showReopinion", "showDivEdcReopinion(1);", true);
                        else
                            ScriptManager.RegisterStartupScript(Page, GetType(), "showReopinion", "showDivEdcReopinion(0);", true);
                    }
                }
                Bind_EdcFinalStatement();
                Conditional();

                if (lblRechkMsg.Text != "")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "status1", "showAnalysisData('tdQcRecheck');", true);
                if (lblMsgForEdcAddtionalDetails.Text != "")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "status1", "showAnalysisData('tdEdcAdditional');", true);


            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnmovetoqc_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spQc = new SqlParameter[3];
                spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                spQc[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_claim_movetoQcfromEdc", spQc);
                if (resp > 0)
                {
                    Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnStencilPlant.Value, "7", "", Request.Cookies["TTSUser"].Value);
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnEdcFinalStatement_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblEdcSetncil.Text != "")
                {
                    int resp = 0;

                    if (hdnSaveType.Value == "SAVE REANALYSIS COMMENTS / INSTRUCTION")
                    {
                        SqlParameter[] spQc = new SqlParameter[4];
                        spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                        spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                        spQc[2] = new SqlParameter("@stencilno", lblEdcSetncil.Text);
                        spQc[3] = new SqlParameter("@ReanalysisComment", txtReanalysis.Text.Replace("\n", "~"));
                        resp = daCOTS.ExecuteNonQuery_SP("sp_update_claim_recheck", spQc);
                        //lnkCancelRequest.Text = "QC REQUEST CANCEL";
                    }
                    else if (hdnSaveType.Value == "SAVE REOPINION COMMENTS")
                    {
                        SqlParameter[] spQc = new SqlParameter[5];
                        spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                        spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                        spQc[2] = new SqlParameter("@stencilno", lblEdcSetncil.Text);
                        spQc[3] = new SqlParameter("@plant", hdnStencilPlant.Value);
                        spQc[4] = new SqlParameter("@edcUpdateComments", txtEdc_crmReopinion.Text.Replace("\n", "~"));
                        resp = daCOTS.ExecuteNonQuery_SP("sp_upd_edcReopinionStatus", spQc);
                    }
                    else if (hdnSaveType.Value == "SAVE ADDTIONAL DETAILS REQUEST")
                    {
                        SqlParameter[] spQc = new SqlParameter[6];
                        spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                        spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                        spQc[2] = new SqlParameter("@stencilno", lblEdcSetncil.Text);
                        spQc[3] = new SqlParameter("@EdcAdditionalReq", txtEdcAdditional.Text.Replace("\n", "~"));
                        spQc[4] = new SqlParameter("@EdcReqUser", Request.Cookies["TTSUser"].Value);
                        spQc[5] = new SqlParameter("@Plant", hdnStencilPlant.Value);
                        resp = daCOTS.ExecuteNonQuery_SP("sp_update_Edc_AdditionalDetailsStencilNo", spQc);
                        //lnkCancelRequest.Text = "ADDTIONAL DETAILS REQUEST CANCEL";
                    }
                    else if (hdnSaveType.Value == "SAVE ROOT CAUSE / CONCLUSION")
                    {

                        int count = 0; string strsave = string.Empty;
                        string ComplaintType = ddlComplaintDesc.SelectedValue.Trim() == "ADD NEW ENTRY" ? txtComplaintDesc.Text.Trim().ToUpper() : ddlComplaintDesc.SelectedValue.Trim().ToUpper();
                        string ComplaintPath = Utilities.RemoveSpecialCharacters(ComplaintType);
                        SqlParameter[] sp1 = new SqlParameter[1];
                        sp1[0] = new SqlParameter("@Complaintype", ComplaintType);
                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimLibrary_param", sp1, DataAccess.Return_Type.DataTable);
                        if (dt.Rows.Count > 0)
                        {
                            hdnCount.Value = dt.Rows[0]["imgcount"].ToString();
                            hdnID.Value = dt.Rows[0]["ID"].ToString();
                        }
                        if (hdnID.Value != "")
                            count = Convert.ToInt32(hdnCount.Value);

                        if (!Directory.Exists(Server.MapPath("~/ClaimLibrary/" + ComplaintPath + "/")))
                            Directory.CreateDirectory(Server.MapPath("~/ClaimLibrary/" + ComplaintPath + "/"));
                        foreach (DataListItem dr in gvClaimImages.Items)
                        {
                            CheckBox chk1 = dr.FindControl("chk1") as CheckBox;
                            HiddenField hdnClaimImage = dr.FindControl("hdnClaimImage") as HiddenField;
                            if (chk1.Checked == true && chk1.Enabled == true)
                            {
                                count++;
                                string sourceFile = Server.MapPath("~" + hdnClaimImage.Value);
                                string filename = count.ToString();
                                strsave = Server.MapPath("~/ClaimLibrary/" + ComplaintPath + "/") + filename + ".jpeg";
                                System.IO.File.Copy(sourceFile, strsave, true);
                            }
                        }
                        SqlParameter[] sp = new SqlParameter[8];
                        if (hdnID.Value != "")
                            sp = new SqlParameter[9];
                        sp[0] = new SqlParameter("@Complaintype", ComplaintType);
                        sp[1] = new SqlParameter("@Apperance", "");
                        sp[2] = new SqlParameter("@ManufacturingEnd", "");
                        sp[3] = new SqlParameter("@CustomerEnd", "");
                        sp[4] = new SqlParameter("@actioncomments", "");
                        sp[5] = new SqlParameter("@warranty", "");
                        sp[6] = new SqlParameter("@imgcount", count);
                        sp[7] = new SqlParameter("@CreatedUser", Request.Cookies["TTSUser"].Value);
                        if (hdnID.Value != "")
                        {
                            sp[8] = new SqlParameter("@ID", hdnID.Value);
                            daCOTS.ExecuteNonQuery_SP("sp_update_ClaimLibrary", sp);
                        }
                        else
                            daCOTS.ExecuteNonQuery_SP("sp_ins_ClaimLibrary", sp);

                        SqlParameter[] spQc = new SqlParameter[11];
                        spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                        spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                        spQc[2] = new SqlParameter("@stencilno", lblEdcSetncil.Text);
                        spQc[3] = new SqlParameter("@rootcause", txtEdcRootCause.Text.Replace("\r\n", "~"));
                        spQc[4] = new SqlParameter("@conclusion", txtOtherEdcConclusion.Text);
                        spQc[5] = new SqlParameter("@QcConclusion", rdbEdcConclusion.SelectedItem.Text);
                        if (rdbClaimNoStatus.SelectedIndex == -1)
                            spQc[6] = new SqlParameter("@StencilClaimStatus", false);
                        else
                            spQc[6] = new SqlParameter("@StencilClaimStatus", rdbClaimNoStatus.SelectedItem.Value);
                        spQc[7] = new SqlParameter("@updateuser", Request.Cookies["TTSUser"].Value);
                        spQc[8] = new SqlParameter("@EdcType", ddltype.SelectedValue == "Choose" ? "" : ddltype.SelectedValue);
                        spQc[9] = new SqlParameter("@ClaimDescription", ComplaintType);
                        spQc[10] = new SqlParameter("@complaintmoveimg", hdnstencilurl.Value.Replace("/tts/", "/"));
                        resp = daCOTS.ExecuteNonQuery_SP("sp_update_EdcRootCause", spQc);
                    }
                    if (resp > 0)
                    {
                        gvClaimNoList.SelectedIndex = Convert.ToInt32(hdnClaimNoClick.Value);
                        ClaimNoClick(gvClaimNoList.SelectedRow);
                    }
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnComplaintMoved_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@EdcMovedcomments", txtComments.Text.Replace("\r\n", "~"));
                sp1[3] = new SqlParameter("@EdcUser", Request.Cookies["TTSUser"].Value);
                sp1[4] = new SqlParameter("@plant", hdnStencilPlant.Value);
                if (hdnStatus.Value == "WAITING FOR EDC OPINION" || hdnStatus.Value == "WAITING FOR EDC RE-OPINION")
                {
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_MoveToCrm_FromEdc", sp1);
                    if (resp > 0)
                    {
                        Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnStencilPlant.Value, "15", txtComments.Text, Request.Cookies["TTSUser"].Value);
                        Response.Redirect("default.aspx", false);
                    }
                }
                else
                {
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_MoveToCrm_FromEdc_Conditionally", sp1);
                    if (resp > 0)
                        Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_ClaimImages(string strStencil, string strCustCode, string strClaimNo)
        {
            try
            {
                gvClaimImages.DataSource = null;
                gvClaimImages.DataBind();
                lblclaimimages.Text = "";
                DataTable dtImage = new DataTable();
                DataColumn col = new DataColumn("ClaimImage", typeof(System.String));
                dtImage.Columns.Add(col);
                if (Directory.Exists(Server.MapPath("~/claimimages" + "/" + strCustCode + "/" + strClaimNo + "/" + strStencil + "/")))
                {
                    string strFileName = string.Empty;
                    foreach (string d in Directory.GetFiles(Server.MapPath("~/claimimages" + "/" + strCustCode + "/" + strClaimNo + "/" + strStencil + "/")))
                    {
                        string strImgName = d.Replace(Server.MapPath("~/claimimages" + "/" + strCustCode + "/" + strClaimNo + "/" + strStencil + "/"), "");
                        string[] strSplit = strImgName.Split('.');
                        string strExtension = "." + strSplit[(strSplit.Length) - 1].ToString().ToLower();
                        if (strExtension == ".jpeg" || strExtension == ".bmp" || strExtension == ".png" || strExtension == ".tif" || strExtension == ".jpg")
                        {
                            string strURL = ConfigurationManager.AppSettings["virdir"] + "claimimages" + "/" + strCustCode + "/" + strClaimNo + "/" + strStencil + "/" + strImgName;
                            dtImage.Rows.Add(ResolveUrl(strURL));
                        }
                    }
                }
                lblStencilShowImages.Text = "";
                if (dtImage.Rows.Count > 0)
                {
                    lblclaimimages.Text = "If you want move any images to claim library. Please select the checkbox.";
                    gvClaimImages.DataSource = dtImage;
                    gvClaimImages.DataBind();
                    if (hdnstencilurl.Value != "")
                    {
                        string[] str1 = hdnstencilurl.Value.Split('~');
                        foreach (string strImgVal in str1)
                        {
                            foreach (DataListItem item in gvClaimImages.Items)
                            {
                                CheckBox chk1 = item.FindControl("chk1") as CheckBox;
                                HiddenField hdnClaimImage = item.FindControl("hdnClaimImage") as HiddenField;
                                if (hdnClaimImage.Value == strImgVal)
                                {
                                    chk1.Checked = true;
                                    chk1.Enabled = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                    lblStencilShowImages.Text = "IMAGES NOT AVAILABLE";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnMoveToCrmRequest_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spQc = new SqlParameter[3];
                spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                spQc[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_EdcAdditionalDetailsFromCrm", spQc);
                if (resp > 0)
                {
                    Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnStencilPlant.Value, "11", "", Request.Cookies["TTSUser"].Value);
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkCancelRequest_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEdcAdditional.Text != "" && txtEdcAdditional.Enabled == true)
                {
                    SqlParameter[] sp1 = new SqlParameter[5];
                    sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp1[2] = new SqlParameter("@stencilno", lblEdcSetncil.Text);
                    sp1[3] = new SqlParameter("@Plant", hdnStencilPlant.Value);
                    sp1[4] = new SqlParameter("@RequestType", "Additional Details");
                    daCOTS.ExecuteNonQuery_SP("sp_cancel_Edc_Request", sp1);
                }
                else if (txtReanalysis.Text != "" && txtReanalysis.Enabled == true)
                {
                    SqlParameter[] sp2 = new SqlParameter[5];
                    sp2[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[2] = new SqlParameter("@stencilno", lblEdcSetncil.Text);
                    sp2[3] = new SqlParameter("@Plant", hdnStencilPlant.Value);
                    sp2[4] = new SqlParameter("@RequestType", "QC Reanalysis");
                    daCOTS.ExecuteNonQuery_SP("sp_cancel_Edc_Request", sp2);
                }
                lnkCancelRequest.Text = "";

                GridViewRow clkdRow = null;
                foreach (GridViewRow gvr in gvClaimNoList.Rows)
                {
                    if (gvr.Cells[1].Text == lblClaimNo.Text)
                    {
                        clkdRow = gvr;
                        break;
                    }
                }
                if (clkdRow != null)
                {
                    //hdnClaimNoClick.Value = clkdRow.RowIndex.ToString();
                    ClaimNoClick(clkdRow);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoClaimDiv('divclaimitems');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}