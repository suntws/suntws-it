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
using System.Text;

namespace TTS
{
    public partial class claimadditionaldetails : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["claim_crm_exp"].ToString() == "True" || dtUser.Rows[0]["claim_crm_dom"].ToString() == "True"))
                        {
                            string strLeadname = "";
                            if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                                strLeadname = Request.Cookies["TTSUser"].Value;
                            SqlParameter[] sp2 = new SqlParameter[2];
                            sp2[0] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                            sp2[1] = new SqlParameter("@username", strLeadname);
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimAddititonalDetails", sp2, DataAccess.Return_Type.DataTable);
                            if (Request["pid"].ToString().ToUpper() == "D")
                            {
                                SqlParameter[] spME = new SqlParameter[2];
                                spME[0] = new SqlParameter("@ClaimType", "E");
                                spME[1] = new SqlParameter("@username", strLeadname);
                                DataTable dtME = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimAdditionalDetails_ME", spME, DataAccess.Return_Type.DataTable);
                                if (dtME.Rows.Count > 0)
                                    dt.Merge(dtME);
                            }
                            if (dt.Rows.Count > 0)
                            {
                                gvClaimAdditionalDetails.DataSource = dt;
                                gvClaimAdditionalDetails.DataBind();
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
        protected void lnkClaimNo_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectClaimDetails(clickedRow);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoClaimDiv('divclaimadddetails');", true);
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
                HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;
                HiddenField hdnstatusid = clickedRow.FindControl("hdnstatusid") as HiddenField;
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                hdnStencilPlant.Value = clickedRow.Cells[3].Text;
                hdnCustCode.Value = hdnClaimCustCode.Value;
                hdnClaimStatus.Value = clickedRow.Cells[5].Text;

                if (lblClaimCustName.Text != "" && lblClaimNo.Text != "")
                    lblClaim.Text = "-";
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[2] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                sp1[3] = new SqlParameter("@StautsID", Convert.ToInt32(hdnstatusid.Value));
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Claim_AdditionalDetailsReqList", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvClaimItems.DataSource = dt;
                    gvClaimItems.DataBind();

                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                    DataTable dtComm = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complaintcommets", sp2, DataAccess.Return_Type.DataTable);

                    if (dtComm.Rows[0]["Commets"].ToString() != "")
                        lblComplaintComments.Text = "<span class='headCss'>CUSTOMER COMMENTS</span><br/>" + dtComm.Rows[0]["Commets"].ToString().Replace("~", "<br/>");
                    else
                        lblComplaintComments.Text = "";

                    bool assystatus = false;
                    foreach (DataRow dr in dt.Select("AdditionalStatus='QC REQUEST' or AdditionalStatus='EDC REQUEST'"))
                    {
                        assystatus = true;
                        break;
                    }
                    if (!assystatus)
                        ScriptManager.RegisterStartupScript(Page, GetType(), "status3", "gotoClaimDiv('divMovedFromAddInfo');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkClaimStencil_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnselectedrow.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectStencilNoDetails(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectStencilNoDetails(GridViewRow clickedRow)
        {
            try
            {
                HiddenField hdnTyreType = clickedRow.FindControl("hdnTyreType") as HiddenField;
                HiddenField hdnComplaint = clickedRow.FindControl("hdnComplaint") as HiddenField;
                HiddenField hdnOperatingCondition = clickedRow.FindControl("hdnOperatingCondition") as HiddenField;
                HiddenField hdnAdditionalUpdateComments = clickedRow.FindControl("hdnAdditionalUpdateComments") as HiddenField;
                lblStencilNo.Text = clickedRow.Cells[2].Text;
                lblBrand.Text = clickedRow.Cells[0].Text;
                lblTyreSize.Text = clickedRow.Cells[1].Text;
                lblTyreType.Text = hdnTyreType.Value;
                txtComplaint.Text = hdnComplaint.Value.Replace("~", "\r\n");
                txtOperatingCondition.Text = hdnOperatingCondition.Value.Replace("~", "\r\n");
                txtComments.Text = hdnAdditionalUpdateComments.Value.Replace("~", "\r\n");
                Build_ClaimImages(lblStencilNo.Text);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoClaimDiv('divclaimadddetails');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "gotoClaimDiv('divAddInfo');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_ClaimImages(string strStencil)
        {
            try
            {
                gvClaimImages.DataSource = null;
                gvClaimImages.DataBind();
                DataTable dtImage = new DataTable();
                DataColumn col = new DataColumn("ClaimImage", typeof(System.String));
                dtImage.Columns.Add(col);

                if (Directory.Exists(Server.MapPath("~/claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/")))
                {
                    string strFileName = string.Empty;
                    foreach (string d in Directory.GetFiles(Server.MapPath("~/claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/")))
                    {
                        string strImgName = d.Replace(Server.MapPath("~/claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/"), "");
                        string[] strSplit = strImgName.Split('.');
                        string strExtension = "." + strSplit[(strSplit.Length) - 1].ToString().ToLower();
                        if (strExtension == ".jpeg" || strExtension == ".bmp" || strExtension == ".png" || strExtension == ".tif" || strExtension == ".jpg")
                        {
                            string strURL = ConfigurationManager.AppSettings["virdir"] + "claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/" + strImgName;
                            dtImage.Rows.Add(ResolveUrl(strURL));
                        }
                    }
                    gvClaimImages.DataSource = dtImage;
                    gvClaimImages.DataBind();
                    ViewState["dtImage"] = gvClaimImages.DataSource;
                }
                Session["ClaimCustCode"] = hdnCustCode.Value;
                Session["ClaimNo"] = lblClaimNo.Text;
                Session["ClaimStencil"] = strStencil;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_StencilFailureImages(string strStencil)
        {
            try
            {
                gv_StencilFailure.DataSource = null;
                gv_StencilFailure.DataBind();
                DataTable dtImage = new DataTable();
                DataColumn col = new DataColumn("ClaimImage", typeof(System.String));
                dtImage.Columns.Add(col);
                col = new DataColumn("ClaimImageName", typeof(System.String));
                dtImage.Columns.Add(col);

                if (Directory.Exists(Server.MapPath("~/claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/")))
                {
                    string strFileName = string.Empty;
                    foreach (string d in Directory.GetFiles(Server.MapPath("~/claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/")))
                    {
                        string strImgName = d.Replace(Server.MapPath("~/claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/"), "");
                        string strURL = ConfigurationManager.AppSettings["virdir"] + "claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/" + strImgName;
                        dtImage.Rows.Add(ResolveUrl(strURL), strImgName);
                    }
                    gv_StencilFailure.DataSource = dtImage;
                    gv_StencilFailure.DataBind();
                    ViewState["dtImage"] = gvClaimImages.DataSource;
                }
                Session["ClaimCustCode"] = hdnCustCode.Value;
                Session["ClaimNo"] = lblClaimNo.Text;
                Session["ClaimStencil"] = strStencil;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvClaimImages_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtImage = ViewState["dtImage"] as DataTable;
                gvClaimImages.DataSource = dtImage;
                gvClaimImages.DataBind();
                gvClaimImages.PageIndex = e.NewPageIndex;
                gvClaimImages.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveAdditionalDetails_Click(object sender, EventArgs e)
        {
            try
            {

                SqlParameter[] sp1 = new SqlParameter[8];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[2] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                sp1[3] = new SqlParameter("@stencilno", lblStencilNo.Text);
                sp1[4] = new SqlParameter("@appstyle", txtComplaint.Text.Replace("\r\n", "~"));
                sp1[5] = new SqlParameter("@runninghours", txtOperatingCondition.Text.Replace("\r\n", "~"));
                sp1[6] = new SqlParameter("@AdditionalUpdateComments", txtComments.Text.Replace("\r\n", "~"));
                sp1[7] = new SqlParameter("@UpdateType", hdnClaimStatus.Value == "QC WAITING FOR ADDITIONAL DETAILS FROM CRM" ? "QC" : "EDC");
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_AdditionalDetails_FromCrm", sp1);
                if (resp > 0)
                {
                    gvClaimAdditionalDetails.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                    Build_SelectClaimDetails(gvClaimAdditionalDetails.SelectedRow);
                    gvClaimItems.SelectedIndex = Convert.ToInt32(hdnselectedrow.Value);
                    Build_SelectStencilNoDetails(gvClaimItems.SelectedRow);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnMovedFromAdditionalDetails_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spQc = new SqlParameter[3];
                spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                spQc[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                int resp = 0;
                if (hdnClaimStatus.Value == "QC WAITING FOR ADDITIONAL DETAILS FROM CRM")
                {
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_QcAdditionalDetailsAdded", spQc);
                    Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnStencilPlant.Value, "5", txtAdditionalComments.Text, Request.Cookies["TTSUser"].Value);
                }
                else if (hdnClaimStatus.Value == "EDC WAITING FOR ADDITIONAL DETAILS FROM CRM")
                {
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_EdcAdditionalDetailsAdded", spQc);
                    Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnStencilPlant.Value, "12", txtAdditionalComments.Text, Request.Cookies["TTSUser"].Value);
                }
                if (resp > 0)
                    Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnTriggerGv_Click(object sender, EventArgs e)
        {
            Build_StencilFailureImages(lblStencilNo.Text);
        }
    }
}