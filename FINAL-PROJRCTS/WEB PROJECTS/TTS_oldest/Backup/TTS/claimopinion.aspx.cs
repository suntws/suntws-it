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
using System.Xml;
using System.Text;
using System.IO;
namespace TTS
{
    public partial class claimopinion : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["claim_crm_exp"].ToString() == "True" || dtUser.Rows[0]["claim_crm_dom"].ToString() == "True"))
                        {
                            Fill_Classification_List_XML();
                            DataTable dtConfig = (DataTable)daTTS.ExecuteReader_SP("Sp_sel_Config_From_ProcessIDMaster", DataAccess.Return_Type.DataTable);
                            ddlplatform.DataSource = dtConfig;
                            ddlplatform.DataTextField = "config";
                            ddlplatform.DataValueField = "config";
                            ddlplatform.DataBind();
                            ddlplatform.Items.Insert(0, "CHOOSE");
                            ddlplatform.Text = "CHOOSE";

                            ddlCrmPlant.Items.Insert(0, "CHOOSE");
                            ddlCrmPlant.Items.Insert(ddlCrmPlant.Items.Count, "MMN");
                            ddlCrmPlant.Items.Insert(ddlCrmPlant.Items.Count, "SLTL");
                            ddlCrmPlant.Items.Insert(ddlCrmPlant.Items.Count, "SITL");
                            ddlCrmPlant.Items.Insert(ddlCrmPlant.Items.Count, "PDK");
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
        protected void ddlCrmPlant_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "NO RECORDS";
                gvClaimCrmList.DataSource = null;
                gvClaimCrmList.DataBind();
                hdnStencilPlant.Value = ddlCrmPlant.SelectedItem.Text;
                if (hdnStencilPlant.Value != "" && hdnStencilPlant.Value != "CHOOSE")
                {
                    lblPageHead.Text = "CLAIM " + hdnStencilPlant.Value + " CRM SETTLEMENT OPINION";
                    string strLeadname = "";
                    if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                        strLeadname = Request.Cookies["TTSUser"].Value;
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                    sp1[1] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                    sp1[2] = new SqlParameter("@username", strLeadname);
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmFeedBackList", sp1, DataAccess.Return_Type.DataTable);
                    if (Request["pid"].ToString().ToUpper() == "D")
                    {
                        SqlParameter[] spME = new SqlParameter[3];
                        spME[0] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                        spME[1] = new SqlParameter("@ClaimType", "E");
                        spME[2] = new SqlParameter("@username", strLeadname);
                        DataTable dtME = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmFeedBackList_MerchantExporter", spME, DataAccess.Return_Type.DataTable);
                        if (dtME.Rows.Count > 0)
                            dt.Merge(dtME);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        gvClaimCrmList.DataSource = dt;
                        gvClaimCrmList.DataBind();
                        lblErr.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Fill_Classification_List_XML()
        {
            try
            {
                string strXmlUserLevelList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"ClaimObservationList.xml");
                XmlDocument xmlLevelList = new XmlDocument();

                xmlLevelList.Load(strXmlUserLevelList);

                if (xmlLevelList != null)
                {
                    var dict = new Dictionary<string, string>();

                    DataTable dtList = new DataTable();
                    dtList.Columns.Add("text", typeof(string));
                    dtList.Columns.Add("key", typeof(string));

                    //Bind Process Conditions & Parameters
                    foreach (XmlNode xNode in xmlLevelList.SelectNodes("/ClaimAnalysis/Classification/item"))
                    {
                        dtList.Rows.Add(xNode.Attributes["text"].Value, xNode.Attributes["text"].Value);
                    }
                    if (dtList.Rows.Count > 0)
                    {
                        gvClassification.DataSource = dtList;
                        gvClassification.DataBind();
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
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectClaimDetails(clickedRow);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoClaimDiv('divclaimitems');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectClaimDetails(GridViewRow clickedRow)
        {
            try
            {
                hdnselectedrow.Value = "";
                lblRechkMsg.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen1", "hideCrmOpinion();", true);
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                hdncondinalstatus.Value = clickedRow.Cells[4].Text;
                HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;

                if (lblClaimCustName.Text != "" && lblClaimNo.Text != "")
                    lblClaim.Text = "-";
                hdnCustCode.Value = hdnClaimCustCode.Value;
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ConclusionList_ClaimNoWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtList.Rows.Count > 0)
                {
                    gvClaimItems.DataSource = dtList;
                    gvClaimItems.DataBind();
                    if (hdncondinalstatus.Value != "CONDITIONALLY APPROVED")
                    {
                        bool assystatus = false;
                        int j = 0;
                        foreach (DataRow dr in dtList.Select("Crm_EdcReOpinion<>''"))
                        {
                            assystatus = true;
                            j++;
                        }
                        if (assystatus)
                        {
                            int k = dtList.AsEnumerable().Where(p => p["CrmEdcReOpinionUpdateComments"].ToString() != "").Count();
                            if (k <= 0)
                            {
                                if (j > 0) lblRechkMsg.Text = "You have chosen " + j + " stencil no. for EDC Re-opinion";
                                ScriptManager.RegisterStartupScript(Page, GetType(), "status5", "showCrmOpinion('tdEdcReOpinion');", true);
                            }
                            else if (dtList.AsEnumerable().Where(p => p["CrmStatus"].ToString() == "").Count() <= 0)
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "status1", "showCrmOpinion('divSettleOpinion');", true);
                            }

                        }

                        //else
                        //{
                        //    //lblRechkMsg.Text = "You have chosen " + j + " stencil no. for EDC Re-opinion";
                        //    ScriptManager.RegisterStartupScript(Page, GetType(), "status5", "showCrmOpinion('tdEdcReOpinion');", true);
                        //}
                    }
                    if (hdncondinalstatus.Value == "CONDITIONALLY APPROVED")
                        btnMoveToAccount.Text = "SAVE AND MOVE TO ACCOUNTS FOR CREDIT NOTE PREPARATION THROUGH CONDITIONALLY APPROVED";
                    else
                    {
                        SqlParameter[] spsel = new SqlParameter[3];
                        spsel[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                        spsel[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                        spsel[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                        DataTable dtsel = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_btnMovename", spsel, DataAccess.Return_Type.DataTable);
                        if (dtsel.Rows.Count > 0)
                        {
                            if (dtsel.Rows[0]["statusid"].ToString() == "25") btnMoveToAccount.Text = "SAVE AND MOVE TO ACCOUNTS FOR CREDIT NOTE PREPARATION";
                            else if (dtsel.Rows[0]["statusid"].ToString() == "26") btnMoveToAccount.Text = "SAVE AND MOVE TO CREDIT NOTE APPORVAL";
                            else if (dtsel.Rows[0]["statusid"].ToString() == "28") btnMoveToAccount.Text = "SAVE AND MOVE TO CREDIT NOTE SETTLEMENT";
                            else if (dtsel.Rows[0]["statusid"].ToString() == "27") btnMoveToAccount.Text = "SAVE AND MOVE TO ACCOUNTS FOR CREDIT NOTE REPREPARATION";
                        }
                    }

                    sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp1[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                    DataTable dtCrmOpinion = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CrmOpinion_PlantWise", sp1, DataAccess.Return_Type.DataTable);
                    ViewState["dtCrmOpinion"] = dtCrmOpinion;

                    SqlParameter[] spClass = new SqlParameter[2];
                    spClass[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    spClass[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    DataTable dtClass = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimClassification_complaintwise", spClass, DataAccess.Return_Type.DataTable);
                    ViewState["dtClass"] = dtClass;

                    ClaimAnalysisData();
                    if (hdnselectedrow.Value != "")
                    {
                        var previousRowIndex = Convert.ToInt32(hdnselectedrow.Value);
                        GridViewRow PreviousRow = gvClaimItems.Rows[previousRowIndex];
                        PreviousRow.BackColor = System.Drawing.Color.Yellow;
                        if (hdnselectedrow.Value != "0") { GridViewRow PreviousRow1 = gvClaimItems.Rows[0]; PreviousRow1.BackColor = System.Drawing.Color.White; }
                    }
                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                    DataTable dtComm = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complaintcommets", sp2, DataAccess.Return_Type.DataTable);
                    if (dtComm.Rows.Count > 0)
                    {
                        lblComplaintComments.Text = ""; lblEdcMovedComments.Text = "";
                        if (dtComm.Rows[0]["Commets"].ToString() != "")
                            lblComplaintComments.Text = "<span class='headCss'>CUSTOMER COMMENTS</span><br/>" + dtComm.Rows[0]["Commets"].ToString().Replace("~", "<br/>");
                        if (hdncondinalstatus.Value != "CONDITIONALLY APPROVED")
                        {
                            if (dtComm.Rows[0]["Commets"].ToString() != "")
                                lblComplaintComments.Text = "<span class='headCss'>CUSTOMER COMMENTS</span><br/>" + dtComm.Rows[0]["Commets"].ToString().Replace("~", "<br/>");
                            if (dtComm.Rows[0]["EdcMovedcomments"].ToString() != "")
                                lblEdcMovedComments.Text = "<span class='headCss'>APPROVER COMMENTS</span><br/>" + dtComm.Rows[0]["EdcMovedcomments"].ToString().Replace("~", "<br/>");
                            lblEdcMovedUser.Text = "<span class='headCss'>CHECKED & APPROVED BY(EDC): </span>" + dtComm.Rows[0]["EdcUser"].ToString().Replace("~", "<br/>");
                        }
                        else
                        {
                            if (dtComm.Rows[0]["CondEdcComments"].ToString() != "")
                                lblEdcMovedComments.Text = "<span class='headCss'>APPROVER COMMENTS</span><br/>" + dtComm.Rows[0]["CondEdcComments"].ToString().Replace("~", "<br/>");
                            lblEdcMovedUser.Text = "<span class='headCss'>CHECKED & APPROVED BY(EDC): </span>" + dtComm.Rows[0]["CondEdcBy"].ToString().Replace("~", "<br/>");
                        }
                        if (dtComm.Rows[0]["CrmSettleType"].ToString() != "")
                        {
                            hdnSettletype.Value = dtComm.Rows[0]["CrmSettleType"].ToString();
                            rdbSettlementOpinion.Items.FindByText(dtComm.Rows[0]["CrmSettleType"].ToString()).Selected = true;
                            rdbSettlementOpinion.Enabled = false;
                            foreach (GridViewRow row in gvOpinionItems.Rows)
                            {
                                TextBox txtClaimPrice = row.FindControl("txtClaimPrice") as TextBox;
                                txtClaimPrice.Enabled = false;
                            }
                        }
                    }

                    //SqlParameter[] sp3 = new SqlParameter[4];
                    //sp3[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    //sp3[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    //sp3[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                    //sp3[3] = new SqlParameter("@StencilNo", lblCRMSetncil.Text);
                    //DataTable dtReanalysis = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Crm_EdcReopinion", sp3, DataAccess.Return_Type.DataTable);
                    //lblEDCReanalysis.Visible = true;
                    //txtEDCReanalysis.Visible = true;
                    //btnEdcReanalysis.Visible = true;
                    //if (dtReanalysis.Rows.Count > 0)
                    //{

                    //     txtEDCReanalysis if (dtReanalysis.Rows[0]["Crm_EdcReOpinion"].ToString() != "") text =  dtReanalysis.Rows[0]["Crm_EdcReOpinion"].ToString().Replace("~","/n") ;
                    //    //if (dtReanalysis.Rows[0]["CrmEdcReOpinionUpdateComments"].ToString() != "") text += "<b>EDC : </b>" + dtReanalysis.Rows[0]["CrmEdcReOpinionUpdateComments"].ToString() + "<br/>";

                    //}
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ClaimAnalysisData()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[2] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AnalysedData_ForCRM", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    dgAnalysisList.DataSource = dt;
                    dgAnalysisList.DataBind();
                    ViewState["dtAnalysedData"] = dt;
                    gvClaimItems.Rows[0].BackColor = System.Drawing.Color.Yellow;
                    if (hdnselectedrow.Value == "")
                        hdnselectedrow.Value = "0";

                    Bind_EachStencilCrmOpinion();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen2", "showMoveToAccountBtn();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_EachStencilCrmOpinion()
        {
            try
            {
                foreach (GridViewRow row in gvClassification.Rows)
                {
                    CheckBox chkClassification = row.FindControl("chkClassification") as CheckBox;
                    TextBox txtClassification = row.FindControl("txtClassification") as TextBox;
                    chkClassification.Checked = false;
                    txtClassification.Text = "";
                }
                rdbClaimNoStatus.SelectedIndex = -1;
                txtCommercialDisc.Text = "";
                Label lblCRMOpinionSetncil = dgAnalysisList.Rows[0].FindControl("lblCRMOpinionSetncil") as Label;
                Label lblconfig = dgAnalysisList.Rows[0].FindControl("lblconfig") as Label;
                Label lblType = dgAnalysisList.Rows[0].FindControl("lblType") as Label;
                HiddenField hdnEDCType = dgAnalysisList.Rows[0].FindControl("hdnEDCType") as HiddenField;
                HiddenField hdnCustType = dgAnalysisList.Rows[0].FindControl("hdnCustType") as HiddenField;
                HiddenField hdnCrmType = dgAnalysisList.Rows[0].FindControl("hdnCrmType") as HiddenField;
                HiddenField hdnEdcReOpinion = dgAnalysisList.Rows[0].FindControl("hdnEdcReOpinion") as HiddenField;
                HiddenField hdnEdcReOpinionUpdate = dgAnalysisList.Rows[0].FindControl("hdnEdcReopinionUpdate") as HiddenField;
                lblCRMSetncil.Text = lblCRMOpinionSetncil.Text;
                hdnPlatform.Value = lblconfig.Text;
                hdnType.Value = hdnEDCType.Value;
                lblEdcType.Text = hdnEDCType.Value == "" ? "" : "<span class='headCss'>EDC: </span>" + hdnEDCType.Value;
                lblQcType.Text = lblType.Text == "" ? "" : "<br /><span class='headCss'>QC: </span>" + lblType.Text;
                lblCustType.Text = hdnCustType.Value == "" ? "" : "<span class='headCss'>CUSTOMER: </span>" + hdnCustType.Value;
                txtEdcReOpinion.Text = hdnEdcReOpinion.Value;
                if (hdnEdcReOpinionUpdate.Value != "")
                {
                    txtEdcReOpinion.Text = "\bCRM : \b" + hdnEdcReOpinion.Value.Replace("~", "\n") + "\n\bEDC : \b" + hdnEdcReOpinionUpdate.Value.Replace("~", "\n");
                    txtEdcReOpinion.Enabled = false;
                }
                if (hdncondinalstatus.Value == "CONDITIONALLY APPROVED") txtEdcReOpinion.Enabled = false;
                //if (txtEdcReOpinion.Text != "" && txtEdcReOpinion.Enabled == true) btnMoveToAccount.Text = "SAVE AND MOVE TO EDC FOR REOPINION";
                ListItem selectedListItem = ddlplatform.Items.FindByText("" + hdnPlatform.Value + "");
                selectedListItem = selectedListItem == null ? ddlplatform.Items.FindByText("CHOOSE") : selectedListItem;
                if (selectedListItem != null)
                {
                    Bind_Type_ConfigWise(hdnPlatform.Value);
                    ddlplatform.Items.FindByText(ddlplatform.SelectedItem.Text).Selected = false;
                    selectedListItem.Selected = true;
                }
                ListItem selectedItem = new ListItem();
                if (hdnCrmType.Value == "")
                    selectedItem = ddlType.Items.FindByText("" + hdnType.Value + "");
                else
                    selectedItem = ddlType.Items.FindByText("" + hdnCrmType.Value + "");
                selectedItem = selectedItem == null ? ddlType.Items.FindByText("CHOOSE") : selectedItem;
                if (selectedItem != null)
                {
                    ddlType.Items.FindByText(ddlType.SelectedItem.Text).Selected = false;
                    selectedItem.Selected = true;
                }
                Label lblStencilConclusion = dgAnalysisList.Rows[0].FindControl("lblStencilConclusion") as Label;
                if (lblStencilConclusion.Text == "Manufacturing Defect")
                    rdbClaimNoStatus.Items.FindByValue("1").Selected = true;

                Build_ClaimImages(lblCRMSetncil.Text, hdnCustCode.Value, lblClaimNo.Text);
                DataTable dtCrmOpinion = ViewState["dtCrmOpinion"] as DataTable;
                DataTable dtClass = ViewState["dtClass"] as DataTable;
                if (dtCrmOpinion.Rows.Count > 0 || dtClass.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCrmOpinion.Select("stencilno='" + lblCRMSetncil.Text + "'"))
                    {
                        if (row["StencilCrmStatus"].ToString() == "ACCEPT")
                            rdbClaimNoStatus.Items.FindByText("ACCEPT").Selected = true;
                        else
                            rdbClaimNoStatus.Items.FindByText("REJECT").Selected = true;
                        txtCommercialDisc.Text = row["CommercialDesc"].ToString().Replace("~", "\r\n");
                    }

                    foreach (GridViewRow row in gvClassification.Rows)
                    {
                        CheckBox chkClassification = row.FindControl("chkClassification") as CheckBox;
                        TextBox txtClassification = row.FindControl("txtClassification") as TextBox;
                        foreach (DataRow cRow in dtClass.Select("stencilno='" + lblCRMSetncil.Text + "' and Classification='" + chkClassification.Text + "'"))
                        {
                            if (chkClassification.Text == cRow["Classification"].ToString())
                            {
                                chkClassification.Checked = true;
                                txtClassification.Text = cRow["Person"].ToString();
                                break;
                            }
                        }
                    }
                    if (dtCrmOpinion.Rows.Count == gvClaimItems.Rows.Count)
                        Bind_OpinionItems();
                    else
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen5", "showMoveToAccountBtn();", true);
                }
                if (lblRechkMsg.Text != "")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "status6", "showCrmOpinion('tdEdcReOpinion');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen3", "showCrmOpinion('divCrmdecision');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Type_ConfigWise(string strConfig)
        {
            DataTable dtType = new DataTable();
            SqlParameter[] sp1 = new SqlParameter[1];
            sp1[0] = new SqlParameter("@config", strConfig);

            dtType = (DataTable)daTTS.ExecuteReader_SP("Sp_sel_Type_From_ProcessIDMaster", sp1, DataAccess.Return_Type.DataTable);
            if (dtType.Rows.Count > 0)
            {
                ddlType.DataSource = dtType;
                ddlType.DataTextField = "TyreType";
                ddlType.DataValueField = "TyreType";
                ddlType.DataBind();
            }
            ListItem selectedListItem = ddlType.Items.FindByText("CHOOSE");
            if (selectedListItem == null)
            {
                ddlType.Items.Insert(0, "CHOOSE");
            }
        }
        private void Bind_OpinionItems()
        {
            try
            {
                SqlParameter[] sp3 = new SqlParameter[3];
                sp3[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp3[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp3[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                DataTable dtOpinion = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmOpinionItems_CRM", sp3, DataAccess.Return_Type.DataTable);
                if (dtOpinion.Rows.Count > 0)
                {
                    gvOpinionItems.DataSource = dtOpinion;
                    gvOpinionItems.DataBind();

                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    DataTable dtNoteAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CreditNote_Address", sp1, DataAccess.Return_Type.DataTable);
                    if (dtNoteAddress.Rows.Count > 0)
                    {
                        gvOpinionItems.HeaderRow.Cells[7].Text = "ACTUAL PRICE<br/>(" + dtNoteAddress.Rows[0]["usercurrency"].ToString() + ")";
                        gvOpinionItems.HeaderRow.Cells[8].Text = "CLAIM PRICE<br/>(" + dtNoteAddress.Rows[0]["usercurrency"].ToString() + ")";
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen8", "showMoveToAccountBtn();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void dgAnalysisData_PageIndex(object sender, GridViewPageEventArgs e)
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
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen1", "hideCrmOpinion();", true);
                DataTable dtAnalysedData = ViewState["dtAnalysedData"] as DataTable;
                dgAnalysisList.DataSource = dtAnalysedData;
                dgAnalysisList.DataBind();
                dgAnalysisList.PageIndex = e.NewPageIndex;
                dgAnalysisList.DataBind();

                Bind_EachStencilCrmOpinion();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCrmStautsChange_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[8];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@stencilno", lblCRMSetncil.Text);
                sp1[3] = new SqlParameter("@CommercialDesc", txtCommercialDisc.Text.Replace("\r\n", "~"));
                sp1[4] = new SqlParameter("@StencilCrmStatus", rdbClaimNoStatus.SelectedItem.Text);
                sp1[5] = new SqlParameter("@CrmUser", Request.Cookies["TTSUser"].Value);
                sp1[6] = new SqlParameter("@config", hdnPlatform.Value);
                sp1[7] = new SqlParameter("@CrmType", ddlType.SelectedValue);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_ClaimCrmOpinionItems", sp1);

                if (resp > 0)
                {
                    SqlParameter[] sp3 = new SqlParameter[3];
                    sp3[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp3[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp3[2] = new SqlParameter("@stencilno", lblCRMSetncil.Text);
                    daCOTS.ExecuteNonQuery_SP("sp_del_ClaimClassification", sp3);
                    foreach (GridViewRow row in gvClassification.Rows)
                    {
                        CheckBox chkClassification = row.FindControl("chkClassification") as CheckBox;
                        TextBox txtClassification = row.FindControl("txtClassification") as TextBox;
                        if (chkClassification.Checked)
                        {
                            SqlParameter[] sp2 = new SqlParameter[6];
                            sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                            sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                            sp2[2] = new SqlParameter("@stencilno", lblCRMSetncil.Text);
                            sp2[3] = new SqlParameter("@Classification", chkClassification.Text);
                            sp2[4] = new SqlParameter("@Person", txtClassification.Text);
                            sp2[5] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                            daCOTS.ExecuteNonQuery_SP("sp_ins_ClaimClassification", sp2);
                        }
                    }
                }
                gvClaimCrmList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                Build_SelectClaimDetails(gvClaimCrmList.SelectedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public string Bind_ClaimClassification(string stencilno)
        {
            string reValue = "";
            try
            {
                DataTable dt = ViewState["dtClass"] as DataTable;
                foreach (DataRow row in dt.Select("stencilno='" + stencilno + "'"))
                {
                    reValue += "<b>" + row["Classification"].ToString() + "</b> - " + row["Person"].ToString() + "<br/>";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reValue;
        }
        private void Update_UnitPrice()
        {
            try
            {
                DataTable dt = new DataTable();
                DataColumn c1 = new DataColumn("CurrentPrice", typeof(System.Decimal));
                dt.Columns.Add(c1);
                c1 = new DataColumn("PreviewPrice", typeof(System.Decimal));
                dt.Columns.Add(c1);
                c1 = new DataColumn("stencilno", typeof(System.String));
                dt.Columns.Add(c1);
                c1 = new DataColumn("TyrePrice", typeof(System.Decimal));
                dt.Columns.Add(c1);

                foreach (GridViewRow row in gvOpinionItems.Rows)
                {
                    string strStencil = row.Cells[4].Text;
                    TextBox txtClaimPrice = row.FindControl("txtClaimPrice") as TextBox;
                    TextBox txtTyrePrice = row.FindControl("txtTyrePrice") as TextBox;
                    if (strStencil != "" && txtClaimPrice.Text != "" && txtTyrePrice.Text != "")
                    {
                        dt.Rows.Add(Convert.ToDecimal(txtClaimPrice.Text), 0, strStencil, Convert.ToDecimal(txtTyrePrice.Text));
                    }
                }
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@ClaimUnitPrice", dt);
                sp1[3] = new SqlParameter("@CrmUser", Request.Cookies["TTSUser"].Value);

                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_claimprice", sp1);
                if (resp > 0)
                {
                    SqlParameter[] sp2 = new SqlParameter[4];
                    sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[2] = new SqlParameter("@ClaimUnitPrice", dt);
                    sp2[3] = new SqlParameter("@User", Request.Cookies["TTSUser"].Value);
                    daCOTS.ExecuteNonQuery_SP("Sp_ins_ClaimPriceHistory", sp2);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnMoveToAccount_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                Update_UnitPrice();
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@crmcomments", txtMoveToAccount.Text.Replace("\r\n", "~"));
                sp1[3] = new SqlParameter("@crmuser", Request.Cookies["TTSUser"].Value);
                sp1[4] = new SqlParameter("@CrmSettleType", rdbSettlementOpinion.SelectedItem.Text);
                sp1[5] = new SqlParameter("@plant", hdnStencilPlant.Value);
                if (hdncondinalstatus.Value == "CONDITIONALLY APPROVED")
                { resp = daCOTS.ExecuteNonQuery_SP("sp_update_CrmClaimToAccount_Conditional", sp1); if (resp > 0)Response.Redirect("default.aspx", false); }
                else
                {
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_CrmClaimToAccount", sp1);
                    if (resp > 0)
                    {
                        Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnStencilPlant.Value, "25", txtMoveToAccount.Text, Request.Cookies["TTSUser"].Value);
                        Response.Redirect("default.aspx", false);
                    }
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
                    gvClaimImages.DataSource = dtImage;
                    gvClaimImages.DataBind();
                    ViewState["dtImage"] = dtImage;
                }
                else
                {
                    lblStencilShowImages.Text = "IMAGES NOT AVAILABLE";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnReOpinion_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[2] = new SqlParameter("@stencilno", lblCRMSetncil.Text);
                sp1[3] = new SqlParameter("@Crm_EdcReOpinion", txtEdcReOpinion.Text.Replace("\n", "~"));
                sp1[4] = new SqlParameter("@CrmUser", Request.Cookies["TTSUser"].Value);
                sp1[5] = new SqlParameter("@Plant", hdnStencilPlant.Value);
                daCOTS.ExecuteNonQuery_SP("sp_update_Edc_ReOpinionStencilNo", sp1);
                gvClaimCrmList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                Build_SelectClaimDetails(gvClaimCrmList.SelectedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnmovetoEdcReopinion_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spQc = new SqlParameter[3];
                spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                spQc[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_EdcReopinoinStatus", spQc);
                if (resp > 0)
                {
                    Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnStencilPlant.Value, "13", "", Request.Cookies["TTSUser"].Value);
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        //protected void btnEdcReanalysis_Click(object sender, EventArgs e) 
        //{
        //    try
        //    {
        //        SqlParameter[] spQc = new SqlParameter[6];
        //        spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
        //        spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
        //        spQc[2] = new SqlParameter("@stencilno", lblCRMSetncil.Text);
        //        spQc[3] = new SqlParameter("@Crm_EdcReanalysis", txtEDCReanalysis.Text.Replace("\n", "~"));
        //        spQc[4] = new SqlParameter("@CrmUser", Request.Cookies["TTSUser"].Value);
        //        spQc[5] = new SqlParameter("@Plant", hdnStencilPlant.Value);
        //        daCOTS.ExecuteNonQuery_SP("sp_upd_Crm_EdcReOpinion", spQc);

        //         GridViewRow clickedRow = null;
        //         foreach (GridViewRow gvr in gvClaimCrmList.Rows)
        //        {
        //            if (gvr.Cells[1].Text == lblClaimNo.Text)
        //            {
        //                clickedRow = gvr;
        //                break;
        //            }
        //        }
        //        if (clickedRow != null)
        //        {
        //            Build_SelectClaimDetails(clickedRow);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
        //    }
        //}
    }
}