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
    public partial class claimsettle : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnCreditNote);
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["claim_creditnote_settle_exp"].ToString() == "True" || dtUser.Rows[0]["claim_creditnote_settle_dom"].ToString() == "True"))
                        {
                            trDdlPlant.Style.Add("display", "none");
                            if (Request["cid"].ToString() == "25")
                            {
                                trDdlPlant.Style.Add("display", "block");
                                trDdlPlant.Style.Add("text-align", "center");
                                ddlAccPlant.Items.Insert(0, "CHOOSE");
                                ddlAccPlant.Items.Insert(ddlAccPlant.Items.Count, "MMN");
                                ddlAccPlant.Items.Insert(ddlAccPlant.Items.Count, "SLTL");
                                ddlAccPlant.Items.Insert(ddlAccPlant.Items.Count, "SITL");
                                ddlAccPlant.Items.Insert(ddlAccPlant.Items.Count, "PDK");
                            }
                            else if (Request["cid"].ToString() == "28")
                            {
                                lblPageHead.Text = "CLAIM - SETTLEMENT LIST";
                                SqlParameter[] sp1 = new SqlParameter[1];
                                sp1[0] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claimSettlement_Accounts", sp1, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gvAccountSettlement.DataSource = dt;
                                    gvAccountSettlement.DataBind();
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
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlAccPlant_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "NO RECORDS";
                gvAccountSettlement.DataSource = null;
                gvAccountSettlement.DataBind();
                hdnStencilPlant.Value = ddlAccPlant.SelectedItem.Text;
                lblPageHead.Text = "CLAIM " + hdnStencilPlant.Value + " CREDIT NOTE PREPARATION";
                if (hdnStencilPlant.Value != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[2];
                    sp1[0] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                    sp1[1] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                    DataTable dt = dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCreditNote_Accounts", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        gvAccountSettlement.DataSource = dt;
                        gvAccountSettlement.DataBind();
                        gvAccountSettlement.Columns[4].Visible = false;
                        gvAccountSettlement.Columns[6].Visible = false;
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
            hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
            hdnotherplant.Value = "";
            gvOTHERPLANT.DataSource = null;
            gvOTHERPLANT.DataBind();
            Build_SelectClaimNoDetails(clickedRow);
        }
        private void Build_SelectClaimNoDetails(GridViewRow clickedRow)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen", "hideClaimAccounts();", true);
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                hdnCondinalStatus.Value = clickedRow.Cells[5].Text;
                HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;
                if (lblClaimCustName.Text != "" && lblClaimNo.Text != "")
                    lblClaim.Text = "-";
                hdnCustCode.Value = hdnClaimCustCode.Value;
                HiddenField hdnCreditNoteNo = clickedRow.FindControl("hdnCreditNoteNo") as HiddenField;
                HiddenField hdnstatus = clickedRow.FindControl("hdnstatus") as HiddenField;
                hdnStatusid.Value = hdnstatus.Value;
                hdnCreditNote.Value = hdnCreditNoteNo.Value;
                HiddenField hdnCreditFileName = clickedRow.FindControl("hdnCreditFileName") as HiddenField;
                lnkCrditNoteDownload.Text = hdnCreditFileName.Value;
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/ExcelCreditNote/" + hdnCustCode.Value + "/" + hdnCreditFileName.Value.Replace(".pdf", ".xls");
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    lblexcel.Text = "EXCEL FILE :";
                    lnkCrditNoteexcel.Text = hdnCreditFileName.Value.Replace(".pdf", ".xls");
                    lnkCrditNoteexcel.Visible = true;
                }
                else
                {
                    lblexcel.Text = "";
                    lnkCrditNoteexcel.Visible = false;
                }
                hdnCreditFile.Value = hdnCreditFileName.Value;
                HiddenField hdnCondReprestatus = clickedRow.FindControl("hdnCondReprestatus") as HiddenField;
                hdncondreparestatus.Value = hdnCondReprestatus.Value;
                SqlParameter[] sp3 = new SqlParameter[3];
                sp3[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp3[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp3[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                DataTable dtComm = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complaintcommets", sp3, DataAccess.Return_Type.DataTable);

                if (dtComm.Rows.Count > 0)
                {
                    lblSettleOpinion.Text = dtComm.Rows[0]["CrmSettleType"].ToString();
                    if (hdnCondinalStatus.Value != "CONDITIONALLY APPROVED")
                        lblcrmmovedprepare.Text = dtComm.Rows[0]["crmcomments"].ToString() != "" ? "<span class='headCss'>CRM COMMENTS</span><br/>" + dtComm.Rows[0]["crmcomments"].ToString() : "";
                    else
                        lblcrmmovedprepare.Text = dtComm.Rows[0]["CondCrmComments"].ToString() != "" ? "<span class='headCss'>CRM COMMENTS</span><br/>" + dtComm.Rows[0]["CondCrmComments"].ToString() : "";
                }

                if (Request["cid"].ToString() == "25")
                {
                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                    DataTable dtNote = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CreditNoteCrmApproval", sp2, DataAccess.Return_Type.DataTable);
                    if (hdnStatusid.Value == "25" || (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED" && hdncondreparestatus.Value == "False"))
                    {
                        if (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED") btnCreditNoteApproval.Text = "MOVE TO CRM FOR CREDIT NOTE APPROVAL THROUGH CONDITIONALLY APPROVED";
                        else btnCreditNoteApproval.Text = "MOVE TO CRM FOR CREDIT NOTE APPROVAL";
                        if (dtNote.Rows.Count == 0) credit_first(); else if (dtNote.Rows.Count > 0) Status_Moved(dtNote);
                    }
                    else if (hdnStatusid.Value == "27" || (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED" && hdncondreparestatus.Value == "True"))
                    {
                        if (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED") btnCreditNoteApproval.Text = "MOVE TO CRM FOR CREDIT NOTE REAPPROVAL THROUGH CONDITIONALLY APPROVED";
                        else btnCreditNoteApproval.Text = "MOVE TO CRM FOR CREDIT NOTE REAPPROVAL";
                        if (dtNote.Rows[0]["CreditNoteUser"].ToString() == "")
                        {
                            credit_first();
                            SqlParameter[] sp1 = new SqlParameter[3];
                            sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                            sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                            sp1[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                            DataTable dtComments = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_CRMRepreparation_MoveComments", sp1, DataAccess.Return_Type.DataTable);
                            if (dtComments.Rows.Count > 0)
                                lblcrmreprepare.Text = "<span class='headCss'>COMMENTS: </span>" + dtComments.Rows[0]["crmReprepareComments"].ToString().Replace("\r\n", "~");
                            SqlParameter[] spsel = new SqlParameter[3];
                            spsel[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                            spsel[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                            spsel[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                            DataTable dtdis = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claim_Discount", spsel, DataAccess.Return_Type.DataTable);
                            if (dtdis.Rows.Count > 0)
                            {
                                int i = 0;
                                foreach (DataRow dr in dtdis.Rows)
                                {
                                    TextBox txtClaimAddDesc = gvClaimOtherCharges.Rows[i].FindControl("txtClaimAddDesc") as TextBox;
                                    TextBox txtClaimAddAmt = gvClaimOtherCharges.Rows[i].FindControl("txtClaimAddAmt") as TextBox;
                                    DropDownList ddlClaimCalcType = gvClaimOtherCharges.Rows[i].FindControl("ddlClaimCalcType") as DropDownList;
                                    txtClaimAddDesc.Text = dr["DescrDiscount"].ToString();
                                    txtClaimAddAmt.Text = dr["Amount"].ToString();
                                    ddlClaimCalcType.SelectedValue = dr["Category"].ToString();
                                    i++;
                                }
                            }
                        }
                        else { Status_Moved(dtNote); }

                    }
                }
                else if (Request["cid"].ToString() == "28")
                {
                    lblmsg1.Text = "";
                    HiddenField hdnCrmSettleType = clickedRow.FindControl("hdnCrmSettleType") as HiddenField;
                    if (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED") { btnAccountsStatusChange.Visible = false; txtSettleInvoiceNo.Enabled = false; txtAccountsComments.Enabled = false; }
                    else { btnAccountsStatusChange.Visible = true; txtSettleInvoiceNo.Enabled = true; txtAccountsComments.Enabled = true; }
                    lblCreditNoteNo.Text = clickedRow.Cells[4].Text;
                    gv_SettleItems.DataSource = null;
                    gv_SettleItems.DataBind();
                    SqlParameter[] sp11 = new SqlParameter[3];
                    sp11[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp11[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp11[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                    DataTable dtother = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmOpinionItems_settlement_plant", sp11, DataAccess.Return_Type.DataTable);
                    if (dtother.Rows.Count > 0)
                    {
                        gvothersettle.DataSource = dtother; gvothersettle.DataBind();
                        btnAccountsStatusChange.Visible = false; txtSettleInvoiceNo.Enabled = false; txtAccountsComments.Enabled = false;
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen11", "showOtherPlant('divsettleplant');", true);
                    }
                    lblTotalprice.Text = "";
                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                    DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmOpinionItems_settlement", sp2, DataAccess.Return_Type.DataTable);
                    if (dtItems.Rows.Count > 0)
                    {
                        gv_SettleItems.DataSource = dtItems;
                        gv_SettleItems.DataBind();
                        object sumtotal;
                        sumtotal = dtItems.Compute("Sum(unitprice)", "");
                        lblTotalprice.Text = "<span class='headCss' style='width:120px;float:left'>TOTAL AMOUNT: </span>" + sumtotal.ToString();
                    }
                    if (hdnCrmSettleType.Value == "Free replacement in next shipment" && Request["pid"].ToString().ToUpper() == "D")
                    {
                        sp2 = new SqlParameter[3];
                        sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                        sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                        sp2[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                        DataTable dtItem1 = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReplacementDetails", sp2, DataAccess.Return_Type.DataTable);
                        if (dtItem1.Rows.Count > 0)
                        {
                            txtSettleInvoiceNo.Enabled = true;
                            txtAccountsComments.Enabled = true;
                            btnAccountsStatusChange.Visible = true;
                            lblDCno.Text = "<span class='headCss' style='width:80px;float:left'>DCNO: </span>" + dtItem1.Rows[0]["DCNO"].ToString();
                            lbljjno.Text = "<span class='headCss' style='width:80px;float:left'>FORM JJ NO.</span>" + dtItem1.Rows[0]["JJNO"].ToString();
                            lblqty.Text = "<span class='headCss' style='width:80px;float:left'>QTY: </span>" + dtItem1.Rows[0]["Qty"].ToString();
                            lbltransport.Text = "<span class='headCss' style='width:150px;float:left'>NAME OF TRANSPORT: </span>" + dtItem1.Rows[0]["TranspoterName"].ToString();
                            lbllrno.Text = "<span class='headCss' style='width:150px;float:left'>LR NO.: </span>" + dtItem1.Rows[0]["LRNO"].ToString();
                            lbllrdate.Text = "<span class='headCss' style='width:150px;float:left'>LR DATE: </span>" + dtItem1.Rows[0]["LRDATE"].ToString();
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen112", "showOtherPlant('divdispatch');", true);
                        }
                        else
                        {
                            txtSettleInvoiceNo.Enabled = false;
                            txtAccountsComments.Enabled = false;
                            btnAccountsStatusChange.Visible = false;
                            lblmsg1.Text = "FREE REPLACEMENT DISPATCH DETAILS NOT ENTERED<br/>PLEASE INFORM TO DISPATCH TEAM";
                            lblDCno.Text = "";
                            lbljjno.Text = "";
                            lblqty.Text = "";
                            lbltransport.Text = "";
                            lbllrno.Text = "";
                            lbllrdate.Text = "";
                        }
                    }
                    else
                    {
                    }
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp1[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                    DataTable dtComments = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_CreditApproval_MoveComments", sp1, DataAccess.Return_Type.DataTable);
                    if (dtComments.Rows.Count > 0)
                    {
                        lblCreditApprovedComments.Text = "<span class='headCss' style='width:235px;float:left'>CREDIT NOTE APPROVED COMMENTS: </span>" + dtComments.Rows[0]["CreditApproveComments"].ToString().Replace("\r\n", "~");
                        lblCreditApprovedUser.Text = "<span class='headCss' style='width:100px;float:left'>APPROVED BY: </span>" + dtComments.Rows[0]["CreditApprovedUser"].ToString() + "  " + dtComments.Rows[0]["CreditApprovedDate"].ToString();
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen2", "showInvoiceEntry();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void credit_first()
        {
            DataTable dtgv1 = new DataTable();
            dtgv1.Columns.Add("slno", typeof(String));
            for (int i = 1; i <= 3; i++)
                dtgv1.Rows.Add(i);
            if (hdnStatusid.Value != "27" && (hdnCondinalStatus.Value != "CONDITIONALLY APPROVED" && hdncondreparestatus.Value != "True") || (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED" && hdncondreparestatus.Value != "True"))
            {
                crm_comments(hdnStencilPlant.Value);
                SqlParameter[] spplant = new SqlParameter[3];
                spplant[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                spplant[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                spplant[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                DataTable dtplant = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_plant_status", spplant, DataAccess.Return_Type.DataTable);
                if (dtplant.Rows.Count > 0)
                {
                    gvother.DataSource = dtplant; gvother.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Jother2", "chkgvotherplant();", true);
                }
            }
            gvClaimOtherCharges.DataSource = dtgv1;
            gvClaimOtherCharges.DataBind();
            DataTable dtItems = new DataTable();

            SqlParameter[] spClass = new SqlParameter[2];
            spClass[0] = new SqlParameter("@custcode", hdnCustCode.Value);
            spClass[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
            DataTable dtClass = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimClassification_complaintwise", spClass, DataAccess.Return_Type.DataTable);
            ViewState["dtClass"] = dtClass;
            gvClaimApproveItems.DataSource = null;
            gvClaimApproveItems.DataBind();
            if (hdnStatusid.Value != "27" && (hdnCondinalStatus.Value != "CONDITIONALLY APPROVED" && hdncondreparestatus.Value != "True") || (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED" && hdncondreparestatus.Value != "True")) dtItems = bind_CRM_Opinion(hdnStencilPlant.Value);
            else
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmOpinionItems_settlement", sp1, DataAccess.Return_Type.DataTable);
            }
            if (dtItems.Rows.Count > 0)
            {
                gvClaimApproveItems.DataSource = dtItems;
                gvClaimApproveItems.DataBind();

                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                DataTable dtNoteAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CreditNote_Address", sp1, DataAccess.Return_Type.DataTable);
                if (dtNoteAddress.Rows.Count > 0)
                {
                    gvClaimApproveItems.HeaderRow.Cells[8].Text = "ACTUAL TYRE PRICE<br/>(" + dtNoteAddress.Rows[0]["usercurrency"].ToString() + ")";
                    gvClaimApproveItems.HeaderRow.Cells[9].Text = "PREVIOUS CLAIM PRICE<br/>(" + dtNoteAddress.Rows[0]["usercurrency"].ToString() + ")";
                    gvClaimApproveItems.HeaderRow.Cells[10].Text = "CURRENT CLAIM PRICE<br/>(" + dtNoteAddress.Rows[0]["usercurrency"].ToString() + ")";
                    hdnCreditCurrency.Value = dtNoteAddress.Rows[0]["usercurrency"].ToString();
                }
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "Jother311", "pricechangehide();", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen1", "showCreditEntry();", true);
            //if (lblSettleOpinion.Text != "Free replacement in next shipment")
            ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen21", "showPriceOption();", true);
        }
        private void Status_Moved(DataTable dtNote)
        {
            hdnCreditNote.Value = dtNote.Rows[0]["CreditNoteNo"].ToString();
            string strCreditNo = string.Empty;
            if (Request["pid"].ToString() == "e")
                strCreditNo = "EXP/CN-" + hdnCreditNote.Value + "/" + dtNote.Rows[0]["CreditNoteYear"].ToString();
            else if (Request["pid"].ToString() == "d")
                strCreditNo = "DOM/CN-" + hdnCreditNote.Value + "/" + dtNote.Rows[0]["CreditNoteYear"].ToString();
            lblCreditNoteNo.Text = strCreditNo;
            lnkCrditNoteDownload.Text = dtNote.Rows[0]["CreditNoteFile"].ToString();
            gv_SettleItems.DataSource = null;
            gv_SettleItems.DataBind();
            SqlParameter[] sp1 = new SqlParameter[3];
            sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
            sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
            sp1[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
            DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmOpinionItems_settlement", sp1, DataAccess.Return_Type.DataTable);
            if (dtItems.Rows.Count > 0)
            {
                gv_SettleItems.DataSource = dtItems;
                gv_SettleItems.DataBind();
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "JOpenApp1", "showCreditApproval();", true);
        }
        private DataTable bind_CRM_Opinion(string plant)
        {
            SqlParameter[] sp2 = new SqlParameter[3];
            sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
            sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
            sp2[2] = new SqlParameter("@plant", plant);
            DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmOpinionItems_CRM_Acc", sp2, DataAccess.Return_Type.DataTable);
            return dtItems;
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
        private void crm_comments(string strplant)
        {
            string[] plant = strplant.Split('~');
            SqlParameter[] spcom = new SqlParameter[2];
            spcom[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
            spcom[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
            DataTable dtcom = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CRM_COMMENTS", spcom, DataAccess.Return_Type.DataTable);
            DataTable dt = new DataTable();
            dt.Columns.Add("plant", typeof(string));
            dt.Columns.Add("crmcomments", typeof(string));
            for (int i = 0; i < plant.Length; i++)
            {
                foreach (DataRow dr in dtcom.Select("plant='" + plant[i] + "'"))
                    dt.Rows.Add(dr["plant"].ToString(), dr["crmcomments"].ToString());
            }
            if (dt.Rows.Count > 0)
            {
                gvCrmComments.DataSource = dt;
                gvCrmComments.DataBind();
            }
        }

        private void check_otherplant()
        {
            gvAccountSettlement.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
            Build_SelectClaimNoDetails(gvAccountSettlement.SelectedRow);
            if (gvOTHERPLANT.Rows.Count > 0)
            {
                string strplant = string.Empty;
                string[] splitplant = { };
                if (hdnStencilPlant.Value != "" && hdnotherplant.Value == "")
                {
                    strplant = hdnStencilPlant.Value;
                    splitplant = strplant.Split('~');
                }
                else if (hdnStencilPlant.Value != "" && hdnotherplant.Value != "")
                {
                    strplant = hdnStencilPlant.Value + "~" + hdnotherplant.Value;
                    splitplant = strplant.Split('~');
                }
                foreach (GridViewRow row in gvother.Rows)
                {
                    CheckBox chkclaimassign = row.FindControl("chkclaimassign") as CheckBox;
                    chkclaimassign.Checked = false;
                    string strPlant = row.Cells[1].Text;
                    for (int i = 0; i < splitplant.Length; i++)
                    {
                        if (splitplant[i].ToString() == strPlant)
                            chkclaimassign.Checked = true;
                    }
                }
            }
        }
        protected void btnSettlePrice_Click(object sender, EventArgs e)
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
                foreach (GridViewRow row in gvOTHERPLANT.Rows)
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
                check_otherplant();
                btnMergePlant_Click(sender, e);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPriceChangeDisable_Click(object sender, EventArgs e)
        {
            check_otherplant();
        }
        protected void btnCreditNote_Click(object sender, EventArgs e)
        {
            try
            {
                string strplant = string.Empty;
                string[] splitplant = { }; string strCreditYear = Build_CreditNoteYear();
                if (hdnStatusid.Value == "27" || (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED" && hdncondreparestatus.Value == "True"))
                {
                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp1[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                    sp1[3] = new SqlParameter("@CreditNoteUser", Request.Cookies["TTSUser"].Value);
                    daCOTS.ExecuteNonQuery_SP("sp_update_ClaimCreditNote_Reperparation", sp1);
                }
                else
                {
                    if (hdnStencilPlant.Value != "" && hdnotherplant.Value == "")
                    {
                        strplant = hdnStencilPlant.Value; splitplant = strplant.Split('~');
                    }
                    else if (hdnStencilPlant.Value != "" && hdnotherplant.Value != "")
                    {
                        strplant = hdnStencilPlant.Value + "~" + hdnotherplant.Value; splitplant = strplant.Split('~');
                    }
                    string strCreditNote = string.Empty; strCreditNote = hdnCreditNote.Value;
                    for (int i = 0; i < splitplant.Length; i++)
                    {
                        SqlParameter[] sp1 = new SqlParameter[8];
                        sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                        sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                        sp1[2] = new SqlParameter("@CreditNoteUser", Request.Cookies["TTSUser"].Value);
                        sp1[3] = new SqlParameter("@CreditNoteNo", strCreditNote);
                        sp1[4] = new SqlParameter("@plant", splitplant[i].ToString());
                        sp1[5] = new SqlParameter("@CreditNoteFile", lblClaimNo.Text + "_" + strplant.Replace("~", "_").Replace(" & ", "-") + ".pdf");
                        sp1[6] = new SqlParameter("@CreditNoteYear", strCreditYear);
                        sp1[7] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                        if (strCreditNote != "") daCOTS.ExecuteNonQuery_SP("sp_update_ClaimCreditNote", sp1);
                        else if (strCreditNote == "")
                        {
                            strCreditNote = (string)daCOTS.ExecuteScalar_SP("sp_insert_and_get_creditnoteno", sp1);
                            hdnCreditNote.Value = strCreditNote;
                        }
                    }
                }
                SqlParameter[] spdel = new SqlParameter[3];
                spdel[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                spdel[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                spdel[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                daCOTS.ExecuteNonQuery_SP("sp_del_claim_Discount", spdel);
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
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_claim_Discount", sp2);
                    }
                }
                DataTable dt = new DataTable();
                SqlParameter[] spval = new SqlParameter[3];
                spval[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                spval[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                spval[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claim_creditnote_pdf", spval, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    object sumQty;
                    sumQty = dt.Compute("Sum(qty)", "");
                    hdntotalqty.Value = sumQty.ToString();
                    object sumunit;
                    sumunit = dt.Compute("Sum(TotalPrice)", "");
                    hdntotalprice.Value = sumunit.ToString();
                    ViewState["CREDITNOTE_ANN"] = dt;

                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    if (!Directory.Exists(serverURL + "/CreditNote/" + hdnCustCode.Value + "/"))
                        Directory.CreateDirectory(serverURL + "/CreditNote/" + hdnCustCode.Value + "/");
                    string path = serverURL + "/CreditNote/" + hdnCustCode.Value + "/";

                    string sPathToWritePdfTo = string.Empty;
                    if (hdnStatusid.Value == "27" || (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED" && hdncondreparestatus.Value == "True")) sPathToWritePdfTo = path + hdnCreditFile.Value;
                    else sPathToWritePdfTo = path + lblClaimNo.Text + "_" + strplant.Replace("~", "_").Replace(" & ", "-") + ".pdf";
                    hdnaddress.Value = strplant;
                    Document document = new Document(PageSize.A4, 22f, 14f, 18f, 18f);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sPathToWritePdfTo, FileMode.Create));
                    document.Open();
                    document.Add(Build_CreditNoteDetails(document, "CREDIT NOTE"));
                    if (dt.Rows.Count > 5)
                    {
                        document.NewPage();
                        document.Add(Build_CreditNote_AnnexureTable(document, "CREDIT NOTE"));
                    }
                    document.Close();
                    //  try{ExportExcel("filename");}catch{}
                }

                gvAccountSettlement.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                Build_SelectClaimNoDetails(gvAccountSettlement.SelectedRow);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ExportExcel(string filename)
        {
            try
            {
                StringBuilder strReturn = new StringBuilder();
                string strConcat = string.Empty;

                DataTable dt1 = ViewState["CREDITNOTE_ANN"] as DataTable;
                string strCreditNo = string.Empty;
                if (Request["pid"].ToString() == "e")
                    strCreditNo = "EXP/CN-" + hdnCreditNote.Value + "/" + dt1.Rows[0]["CreditNoteYear"].ToString();
                else if (Request["pid"].ToString() == "d")
                    strCreditNo = "DOM/CN-" + hdnCreditNote.Value + "/" + dt1.Rows[0]["CreditNoteYear"].ToString();

                DataView dtDescView = new DataView(dt1);
                dtDescView.Sort = "assigntoqc ASC";
                DataTable distinctPlant = dtDescView.ToTable(true, "assigntoqc");
                SqlParameter[] sp2 = new SqlParameter[3];
                sp2[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                sp2[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                sp2[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                DataTable dtClaimDiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claim_Discount", sp2, DataAccess.Return_Type.DataTable);
                decimal itemtotal = 0;
                string address = hdncustaddress.Value.Replace("/n", "~");
                string[] address1 = address.Split('~');
                strConcat += "<table cellspacing='0' rules='all' border='1' style='border-collapse: collapse;border-color: #000;'>";
                strConcat += "<tr><td  style='text-align:center;font-weight: bold;font-size: 18px;'>TVS</td><td colspan='3'><table><tr><td colspan='3'>180, Anna Salai, Chennai - 600 006, India.</td></tr><tr><td colspan='3'>Tel. Off  : 91-44-2852 5289 / 2852 0781</td></tr><tr><td colspan='3'>Fax        : 91-20-26068388</td></tr><tr><td colspan='3'>www.sun-tws.com</td></tr></table></td></tr>";
                strConcat += "<tr><td colspan='4' style='text-align:center;font-weight: bold;font-size: 15px;'>CREDIT NOTE </td></tr>";
                strConcat += "<tr><td><table>";
                for (int i = 0; i < address1.Length; i++)
                {
                    strConcat += "<tr><td>" + address1[i].ToString() + "</td></tr>";
                }
                strConcat += " </table></td><td colspan='3'><table><tr><td>CREDIT NOTE NO.</td><td colspan='2'>: " + strCreditNo + "</td></tr><tr><td>DATE</td><td colspan='2'>: " + DateTime.Now.ToString("dd/MM/yyyy") + "</td></tr><tr><td>CLAIM REF.</td><td colspan='2'>: " + lblClaimNo.Text + "</td></tr>";
                if (dt1.Rows[0]["CustomerRefNo"].ToString().Trim() != "")
                    strConcat = "<tr><td >CUSTOMER REF.</td><td colspan='2'>: " + dt1.Rows[0]["CustomerRefNo"].ToString() + "</td></tr>";
                strConcat += "</table></td></tr><tr  style='text-align:center;font-weight: bold;font-size: 13px;'><td >PARTICULARS</td><td colspan=\"" + "\">QTY</td><td>UNIT PRICE\n(" + hdnCreditCurrency.Value + ")</td><td>TOTAL UNITPRICE\n(" + hdnCreditCurrency.Value + ")</td></tr><br />";
                strConcat += "<tr><td >TO AGREED AMOUNT PAYABLE TO YOUR COMPANY : </td><td></td><td></td><td></td></tr><br />";
                foreach (DataRow dPlant in distinctPlant.Rows)
                {
                    strConcat += "<tr><td>" + dPlant["assigntoqc"].ToString().ToUpper() + " TYRES</td><td></td><td></td><td></td></tr><br />";
                    foreach (DataRow dr in dt1.Select("assigntoqc='" + dPlant["assigntoqc"].ToString() + "'"))
                    {
                        strConcat += "<tr><td>" + dr["tyresize"].ToString() + " / " + dr["brand"].ToString() + " / " + dr["CrmType"].ToString() + "</td><td>" + dr["qty"].ToString() + "</td><td>" + dr["unitprice"].ToString() + "</td><td>" + (Convert.ToDecimal(dr["qty"].ToString()) * Convert.ToDecimal(dr["unitprice"].ToString())).ToString() + "</td></tr><br />";
                        itemtotal += (Convert.ToDecimal(dr["qty"].ToString()) * Convert.ToDecimal(dr["unitprice"].ToString()));
                    }
                }
                decimal discount = 0;
                if (dtClaimDiscount.Rows.Count > 0)
                {
                    for (int i = 0; i < dtClaimDiscount.Rows.Count; i++)
                    {
                        strConcat += "<tr><td >" + dtClaimDiscount.Rows[i]["Category"].ToString() + ": " + dtClaimDiscount.Rows[i]["DescrDiscount"].ToString() + "</td><td></td><td></td><td>" + dtClaimDiscount.Rows[i]["Amount"].ToString() + "</td></tr><br />";
                        discount += Convert.ToDecimal(dtClaimDiscount.Rows[i]["Amount"].ToString());
                    }
                } decimal total = discount + itemtotal;
                strConcat += "<tr><td>TOTAL</td><td>" + hdntotalqty.Value + "</td><td></td><td>" + total.ToString() + "</td></tr><br />";
                string[] strSplit = total.ToString().Split('.');
                int intPart = Convert.ToInt32(strSplit[0].ToString());
                string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
                string text = Utilities.NumberToText(intPart, true, false).ToString();
                if (Convert.ToDecimal(decimalPart) > 0)
                    text += " Point " + Utilities.DecimalToText(decimalPart) + " Only";
                else
                    text += " Only";
                //strConcat += "<tr><td>" + "(" + hdnCreditCurrency.Value.ToUpper() + ") " + text.ToUpper() + "</td><td colspan='3'><table><tr><td colspan='3'>For T.S.RAJAM TYRES PRIVATE LIMITED</td></tr><tr><td colspan='3'></td></tr><tr><td colspan='3'> Authorised Signatory</td></tr></table></td></tr><br />";
                strConcat += "<tr><td>" + "(" + hdnCreditCurrency.Value.ToUpper() + ") " + text.ToUpper() + "</td><td colspan='3'><table><tr><td colspan='3'>For SUNDARAM INDUSTRIES PRIVATE LIMITED</td></tr><tr><td colspan='3'></td></tr><tr><td colspan='3'> Authorised Signatory</td></tr></table></td></tr><br />";
                //*************To Be inserted  On 14-02-2022***********************

                string strSettle = dt1.Rows[0]["CrmSettleType"].ToString();

                if (strSettle == "Adjustable in the subsequent invoice")
                {
                    strConcat += "<tr><td colspan='4'>The above credit value of " + total.ToString() + " (" + hdnCreditCurrency.Value + ") will be adjusted in the subsequent invoice.</td></tr>";
                }
                else if (strSettle == "Claim amount to be refunded")
                {
                    strConcat += "<tr><td colspan='4'The above credit value of " + total.ToString() + " (" + hdnCreditCurrency.Value + ") will be credited to your account.</td></tr>";
                }
                else if (strSettle == "Free replacement in next shipment")
                {
                    strConcat += "<tr><td colspan='4'The free replacement tyres will be sent in the next shipment.</td></tr>";
                }
                strConcat += "</table>";
                gv_Excel.DataSource = null;
                gv_Excel.DataBind();
                DataTable dt = new DataTable();
                DataColumn col = new DataColumn("Excel", typeof(System.String));
                dt.Columns.Add(col);
                dt.Rows.Add(strConcat.ToString());
                if (dt.Rows.Count > 0)
                {
                    gv_Excel.DataSource = dt;
                    gv_Excel.DataBind();
                }
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/ExcelCreditNote/" + hdnCustCode.Value + "/"))
                    Directory.CreateDirectory(serverURL + "/ExcelCreditNote/" + hdnCustCode.Value + "/");
                string path = serverURL + "ExcelCreditNote/" + hdnCustCode.Value + "/";

                string sPathToWritePdfTo = string.Empty;
                if (hdnStatusid.Value == "27" || (hdnCondinalStatus.Value == "CONDITIONALLY APPROVED" && hdncondreparestatus.Value == "True")) sPathToWritePdfTo = path + hdnCreditFile.Value.Replace(".pdf", ".xls");
                else sPathToWritePdfTo = path + lblClaimNo.Text + "_" + hdnaddress.Value.Replace("~", "_").Replace(" & ", "-") + ".xls";
                FileInfo file = new FileInfo(sPathToWritePdfTo);
                if (file.Exists)
                {
                    file.Delete();
                }
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StreamWriter writer = File.AppendText(sPathToWritePdfTo);
                        gv_Excel.RenderControl(hw);
                        writer.WriteLine(sw.ToString());
                        writer.Close();
                    }
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
            /* Verifies that the control is rendered */
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
        protected void lnkCrditNoteexcel_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/ExcelCreditNote/" + hdnCustCode.Value + "/" + lnkTxt.Text;
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    try
                    {
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                        Response.AddHeader("Content-Type", "application/Excel");
                        Response.ContentType = "application/vnd.xls";
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.WriteFile(file.FullName);
                        Response.End();
                    }
                    catch { }
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCreditNoteApproval_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@CreditNoteComments", txtCreditNoComments.Text.Replace("\r\n", "~"));
                sp1[3] = new SqlParameter("@CreditMoveUser", Request.Cookies["TTSUser"].Value);
                sp1[4] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                if (hdnCondinalStatus.Value != "CONDITIONALLY APPROVED")
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_CreditApproval_MoveToCrm", sp1);
                else
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_CreditApproval_MoveToCrm_conditional", sp1);
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
                            if (strCCMail.Length > 0) strCCMail += "," + Build_NotificationMailID(row["plant"].ToString());
                            else strCCMail = Build_NotificationMailID(row["plant"].ToString());
                            Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, row["plant"].ToString(), "26", txtCreditNoComments.Text, Request.Cookies["TTSUser"].Value);
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
        protected void btnAccountsStatusChange_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@closedcomments", txtAccountsComments.Text.Replace("\r\n", "~"));
                sp1[3] = new SqlParameter("@closeduser", Request.Cookies["TTSUser"].Value);
                sp1[4] = new SqlParameter("@InvoiceNo", txtSettleInvoiceNo.Text);
                sp1[5] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_ClaimAccounts_Closed", sp1);
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
                            Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, row["plant"].ToString(), "30", txtAccountsComments.Text, Request.Cookies["TTSUser"].Value);
                        }
                    }
                    Response.Redirect("claimsettle.aspx?cid=28&pid=" + Request["pid"].ToString(), false);
                }
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
                Chunk postCell1 = new Chunk("SUNDARAM INDUSTRIES PRIVATE LIMITED\n", headFont);
                //*************To Be inserted  On 14-02-2022***********************
                var subHead = FontFactory.GetFont("Arial", 6, Font.BOLD);
                //Chunk postCell2 = new Chunk("(Formerly Known as T.S.Rajam Tyres Pvt. Ltd.)\n", subHead);
                Chunk postCell2 = new Chunk("(Formerly Known as T.S.RAJAM TYRES PRIVATE LIMITED)\n", subHead);
                para.Add(postCell1);
                para.Add(postCell2);
               /* AddressTxt += "180, Anna Salai, Chennai - 600 006, India.\n";*/
                AddressTxt += "31/B, Krishna Towers 3rd Floor,\n";
                AddressTxt += "Ekkatuthangal , Chennai -600032, India.\n";
                AddressTxt += "Tel. Off  : 91-44-2852 5289 / 2852 0781\n";
               /* AddressTxt += "Fax       : 91-20-26068388\n";*/
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
                }
                hdncustaddress.Value = strCustAddress;
                Cell1 = new PdfPCell(new Phrase(strCustAddress, titleFont1));
                Cell1.ExtraParagraphSpace = 3f;
                table.AddCell(Cell1);

                PdfPTable nestedtable = new PdfPTable(3);
                float[] width = new float[] { 10f, 1.5f, 18f };
                nestedtable.SetWidths(width);
                PdfPCell Cell = new PdfPCell(new Phrase("CREDIT NOTE NO.", titleFont1));
                Cell.Border = Rectangle.NO_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(":", titleFont1));
                Cell.Border = Rectangle.NO_BORDER;
                nestedtable.AddCell(Cell);

                DataTable dt1 = ViewState["CREDITNOTE_ANN"] as DataTable;
                string strCreditNo = string.Empty;
                if (Request["pid"].ToString() == "e")
                    strCreditNo = "EXP/CN-" + hdnCreditNote.Value + "/" + dt1.Rows[0]["CreditNoteYear"].ToString();
                else if (Request["pid"].ToString() == "d")
                    strCreditNo = "DOM/CN-" + hdnCreditNote.Value + "/" + dt1.Rows[0]["CreditNoteYear"].ToString();

                Cell = new PdfPCell(new Phrase(strCreditNo, titleFont1));
                Cell.Border = Rectangle.RIGHT_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("DATE", titleFont1));
                Cell.Border = Rectangle.NO_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(":", titleFont1));
                Cell.Border = Rectangle.NO_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy"), titleFont1));
                Cell.Border = Rectangle.RIGHT_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase("CLAIM REF.", titleFont1));
                Cell.Border = Rectangle.NO_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(":", titleFont1));
                Cell.Border = Rectangle.NO_BORDER;
                nestedtable.AddCell(Cell);
                Cell = new PdfPCell(new Phrase(lblClaimNo.Text, titleFont1));
                Cell.Border = Rectangle.RIGHT_BORDER;
                nestedtable.AddCell(Cell);
                if (dt1.Rows[0]["CustomerRefNo"].ToString().Trim() != "")
                {
                    Cell = new PdfPCell(new Phrase("CUSTOMER REF.", titleFont1));
                    Cell.Border = Rectangle.NO_BORDER;
                    nestedtable.AddCell(Cell);
                    Cell = new PdfPCell(new Phrase(":", titleFont1));
                    Cell.Border = Rectangle.NO_BORDER;
                    nestedtable.AddCell(Cell);
                    Cell = new PdfPCell(new Phrase(dt1.Rows[0]["CustomerRefNo"].ToString(), titleFont1));
                    Cell.Border = Rectangle.RIGHT_BORDER;
                    nestedtable.AddCell(Cell);
                }
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
                PdfPTable dt = new PdfPTable(4);
                dt.TotalWidth = 520f;
                dt.LockedWidth = true;
                float[] widths = new float[] { 15f, 5f, 5f, 5f };
                dt.SetWidths(widths);
                dt.AddCell(Build_ItemTableHeadStyle("PARTICULARS"));
                dt.AddCell(Build_ItemTableHeadStyle("QTY"));
                dt.AddCell(Build_ItemTableHeadStyle("UNIT PRICE\n(" + hdnCreditCurrency.Value + ")"));
                dt.AddCell(Build_ItemTableHeadStyle("TOTAL UNITPRICE\n(" + hdnCreditCurrency.Value + ")"));
                dt.AddCell(Build_ItemTableListStyle("TO AGREED AMOUNT PAYABLE TO YOUR COMPANY :", 0, 1));
                Bind_itemsTableEmptyRow(dt, 3);

                DataTable dt1 = ViewState["CREDITNOTE_ANN"] as DataTable;
                DataView dtDescView = new DataView(dt1);
                dtDescView.Sort = "assigntoqc ASC";
                DataTable distinctPlant = dtDescView.ToTable(true, "assigntoqc");
                SqlParameter[] sp2 = new SqlParameter[3];
                sp2[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                sp2[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                sp2[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                DataTable dtClaimDiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claim_Discount", sp2, DataAccess.Return_Type.DataTable);
                decimal itemtotal = 0;
                if (dt1.Rows.Count <= 5)
                {
                    foreach (DataRow dPlant in distinctPlant.Rows)
                    {
                        Bind_itemsTableEmptyRow(dt, 4);
                        dt.AddCell(Build_ItemTablePlantHeadStyle(dPlant["assigntoqc"].ToString().ToUpper() + " TYRES", 0));
                        Bind_itemsTableEmptyRow(dt, 3);
                        foreach (DataRow dr in dt1.Select("assigntoqc='" + dPlant["assigntoqc"].ToString() + "'"))
                        {
                            dt.AddCell(Build_ItemTableListStyle(dr["tyresize"].ToString() + " / " + dr["brand"].ToString() + " / " + dr["CrmType"].ToString(), 0, 1));// + (dr["CrmStatus"].ToString() == "REJECT" ? "  (" + dr["CrmStatus"].ToString() + ")" : "")
                            dt.AddCell(Build_ItemTableListStyle(dr["qty"].ToString(), 2, 1));
                            dt.AddCell(Build_ItemTableListStyle(dr["unitprice"].ToString(), 2, 1));
                            dt.AddCell(Build_ItemTableListStyle((Convert.ToDecimal(dr["qty"].ToString()) * Convert.ToDecimal(dr["unitprice"].ToString())).ToString(), 2, 1));
                            itemtotal += (Convert.ToDecimal(dr["qty"].ToString()) * Convert.ToDecimal(dr["unitprice"].ToString()));
                        }
                    }
                }
                else
                {
                    string strtxt = "BEING CREDIT PASSED TOWARDS TYRES SENT\nAS PER ANNEXURE ENCLOSED";
                    dt.AddCell(Build_ItemTableListStyle(strtxt, 0, 1));
                    Bind_itemsTableEmptyRow(dt, 3);
                    Bind_itemsTableEmptyRow(dt, 4);
                    foreach (DataRow dPlant in distinctPlant.Rows)
                    {
                        int plantQty = 0; decimal plantAmt = 0;
                        dt.AddCell(Build_ItemTablePlantHeadStyle(dPlant["assigntoqc"].ToString().ToUpper() + " TYRES", 1));
                        foreach (DataRow dr in dt1.Select("assigntoqc='" + dPlant["assigntoqc"].ToString() + "'"))
                        {
                            plantQty += Convert.ToInt32(dr["qty"].ToString());
                            plantAmt += Convert.ToDecimal(dr["qty"].ToString()) * Convert.ToDecimal(dr["unitprice"].ToString());
                        }
                        dt.AddCell(Build_ItemTableListStyle(plantQty.ToString(), 2, 1));
                        dt.AddCell(Build_ItemTableListStyle("", 2, 1));
                        dt.AddCell(Build_ItemTableListStyle(plantAmt.ToString(), 2, 1));
                        itemtotal += plantAmt;
                    }
                }
                decimal discount = 0;
                if (dtClaimDiscount.Rows.Count > 0)
                {
                    for (int i = 0; i < dtClaimDiscount.Rows.Count; i++)
                    {
                        dt.AddCell(Build_ItemTableListStyle(dtClaimDiscount.Rows[i]["Category"].ToString() + ": " + dtClaimDiscount.Rows[i]["DescrDiscount"].ToString(), 0, 1));
                        dt.AddCell(Build_ItemTableListStyle("", 2, 1));
                        dt.AddCell(Build_ItemTableListStyle("", 2, 1));
                        dt.AddCell(Build_ItemTableListStyle(dtClaimDiscount.Rows[i]["Amount"].ToString(), 2, 1));
                        discount += Convert.ToDecimal(dtClaimDiscount.Rows[i]["Amount"].ToString());
                    }
                }
                for (int j = 0; j < 3; j++)
                {
                    Bind_itemsTableEmptyRow(dt, 4);
                }
                decimal total = discount + itemtotal;
                dt.AddCell(Build_ItemTableBottomStyle("TOTAL", 2, 0));
                dt.AddCell(Build_ItemTableBottomStyle(hdntotalqty.Value, 2, 0));
                dt.AddCell(Build_ItemTableBottomStyle("", 2, 0));
                dt.AddCell(Build_ItemTableBottomStyle(total.ToString(), 2, 0));

                string[] strSplit = total.ToString().Split('.');
                int intPart = Convert.ToInt32(strSplit[0].ToString());
                string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
                string text = Utilities.NumberToText(intPart, true, false).ToString();
                if (Convert.ToDecimal(decimalPart) > 0)
                    text += " Point " + Utilities.DecimalToText(decimalPart) + " Only";
                else
                    text += " Only";

                PdfPCell Cell1 = new PdfPCell(new Phrase("(" + hdnCreditCurrency.Value.ToUpper() + ") " + text.ToUpper(), titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.Colspan = 1;
                dt.AddCell(Cell1);

                //Cell1 = new PdfPCell(new Phrase("For T.S.RAJAM TYRES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory", titleFont));
                Cell1 = new PdfPCell(new Phrase("For SUNDARAM INDUSTRIES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory", titleFont));//*************To Be inserted  On 14-02-2022***********************
                Cell1.Colspan = 3;
                Cell1.HorizontalAlignment = 1;
                dt.AddCell(Cell1);

                PdfPCell nesthousing1 = new PdfPCell(dt);
                nesthousing1.Padding = 0f;
                nesthousing1.Colspan = 2;
                table.AddCell(nesthousing1);

                var valFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                string strSettle = dt1.Rows[0]["CrmSettleType"].ToString();
                string strCont = "\n\n";
                if (strSettle == "Adjustable in the subsequent invoice")
                {
                    strCont += "The above credit value of " + total.ToString() + " (" + hdnCreditCurrency.Value + ") will be adjusted in the subsequent invoice.";
                }
                else if (strSettle == "Claim amount to be refunded")
                {
                    strCont += "The above credit value of " + total.ToString() + " (" + hdnCreditCurrency.Value + ") will be credited to your account.";
                }
                else if (strSettle == "Free replacement in next shipment")
                {
                    strCont += "The free replacement tyres will be sent in the next shipment.";
                }
                PdfPCell CellSettle = new PdfPCell(new Phrase(strCont.ToUpper(), valFont));
                CellSettle.Border = Rectangle.NO_BORDER;
                CellSettle.Colspan = 2;
                table.AddCell(CellSettle);
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
        private PdfPTable Build_CreditNote_AnnexureTable(Document doc, string strFileType)
        {
            PdfPTable mailTable = new PdfPTable(1);
            mailTable.TotalWidth = 520f;
            mailTable.LockedWidth = true;
            try
            {
                PdfPCell emptyCell = new PdfPCell(new Phrase("\n\n\n"));
                emptyCell.HorizontalAlignment = 1;
                emptyCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                emptyCell.Border = Rectangle.NO_BORDER;
                mailTable.AddCell(emptyCell);
                PdfPTable table = new PdfPTable(5);
                var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                table.TotalWidth = 520f;
                table.LockedWidth = true;
                //                              No   Size   qt   up   tot up
                float[] widths = new float[] { 0.7f, 3.8f, 1f, 1.5f, 2.3f };
                table.SetWidths(widths);

                table.AddCell(Build_HeadTableColsPan("ANNEXURE TO CREDIT NOTE NO.: " + lblCreditNoteNo.Text, 0, 3));
                table.AddCell(Build_HeadTableColsPan("DATE : " + DateTime.Now.ToString("dd/MM/yyyy"), 2, 2));

                table.AddCell(Build_ItemTableHeadStyle("SR\nNo."));
                table.AddCell(Build_ItemTableHeadStyle("PARTICULARS"));
                table.AddCell(Build_ItemTableHeadStyle("QTY"));
                table.AddCell(Build_ItemTableHeadStyle("UNIT PRICE\n(" + hdnCreditCurrency.Value + ")"));
                table.AddCell(Build_ItemTableHeadStyle("TOTAL UNIT PRICE\n(" + hdnCreditCurrency.Value + ")"));
                DataTable dt1 = new DataTable();
                dt1 = ViewState["CREDITNOTE_ANN"] as DataTable;
                decimal total = 0;
                int i = 0;
                foreach (DataRow dr in dt1.Rows)
                {
                    i++;
                    table.AddCell(Build_ItemTableListStyle(i.ToString(), 2, 1));
                    table.AddCell(Build_ItemTableListStyle(dr["tyresize"].ToString() + " / " + dr["brand"].ToString() + " / " + dr["CrmType"].ToString() + (dr["CrmStatus"].ToString() == "REJECT" ? "  (" + dr["CrmStatus"].ToString() + ")" : ""), 0, 1));
                    table.AddCell(Build_ItemTableListStyle(dr["qty"].ToString(), 2, 1));
                    table.AddCell(Build_ItemTableListStyle(dr["unitprice"].ToString(), 2, 1));
                    total = (Convert.ToDecimal(dr["qty"].ToString()) * Convert.ToDecimal(dr["unitprice"].ToString()));
                    table.AddCell(Build_ItemTableListStyle(total.ToString(), 2, 1));
                }
                table.AddCell(Build_ItemTableBottomStyle("TOTAL", 2, 2));
                table.AddCell(Build_ItemTableBottomStyle(hdntotalqty.Value, 2, 0));
                table.AddCell(Build_ItemTableBottomStyle("", 2, 0));
                table.AddCell(Build_ItemTableBottomStyle(hdntotalprice.Value, 2, 0));
                if (hdntotalprice.Value != "")
                {
                    var valFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                    string[] strSplit = hdntotalprice.Value.ToString().Split('.');
                    int intPart = Convert.ToInt32(strSplit[0].ToString());
                    string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
                    string text = Utilities.NumberToText(intPart, true, false).ToString();
                    if (Convert.ToDecimal(decimalPart) > 0)
                        text += " Point " + Utilities.DecimalToText(decimalPart) + " Only";
                    else
                        text += " Only";

                    PdfPCell Cell1 = new PdfPCell(new Phrase("(" + hdnCreditCurrency.Value.ToUpper() + ") " + text.ToUpper(), titleFont));
                    Cell1.HorizontalAlignment = 0;
                    Cell1.Colspan = 2;
                    table.AddCell(Cell1);
                    //Cell1 = new PdfPCell(new Phrase("For T.S.RAJAM TYRES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory", titleFont));
                    Cell1 = new PdfPCell(new Phrase("For SUNDARAM INDUSTRIES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory", titleFont));//*************To Be inserted  On 14-02-2022***********************
                    Cell1.Colspan = 3;
                    Cell1.HorizontalAlignment = 1;
                    table.AddCell(Cell1);
                }
                PdfPCell nesthousing1 = new PdfPCell(table);
                nesthousing1.Padding = 0f;
                mailTable.AddCell(nesthousing1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return mailTable;
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
        protected void btnMergePlant_Click(object sender, EventArgs e)
        {
            gvOTHERPLANT.DataSource = null;
            gvOTHERPLANT.DataBind();
            string[] splitplant = hdnotherplant.Value.Split('~');
            string strplant = hdnotherplant.Value + '~' + hdnStencilPlant.Value;
            crm_comments(strplant);
            DataTable dtall = new DataTable();
            for (int i = 0; i < splitplant.Length; i++)
            {
                DataTable dtItems = bind_CRM_Opinion(splitplant[i]);
                if (i == 0)
                    dtall = dtItems.Copy();
                else
                    dtall.Merge(dtItems);
                lbl1.Text = "OTHER PLANT ITEMS";
            }
            if (dtall.Rows.Count > 0)
            {
                gvOTHERPLANT.DataSource = dtall;
                gvOTHERPLANT.DataBind();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jother31", "pricechangehide();", true);
            }
            foreach (GridViewRow row in gvother.Rows)
            {
                CheckBox chkclaimassign = row.FindControl("chkclaimassign") as CheckBox;
                chkclaimassign.Checked = false;
                string strPlant = row.Cells[1].Text;
                for (int i = 0; i < splitplant.Length; i++)
                { if (splitplant[i].ToString() == strPlant) chkclaimassign.Checked = true; }
            } if (dtall.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jother121", "showOtherPlant('divClaimforotherplant');", true);
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "Jother211", "showCreditEntry();", true);
            //if (lblSettleOpinion.Text != "Free replacement in next shipment")
            ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen21", "showPriceOption();", true);
        }
        private string Build_NotificationMailID(string strPlant)
        {
            string returnValue = string.Empty;
            if (Request["pid"].ToString() == "e")
                returnValue += Utilities.Build_Cliam_ToList("claim_creditnote_settle_exp");
            else if (Request["pid"].ToString() == "d")
                returnValue += Utilities.Build_Cliam_ToList("claim_creditnote_settle_dom");
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
            else if (strPlant == "PDK")
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
    }
}