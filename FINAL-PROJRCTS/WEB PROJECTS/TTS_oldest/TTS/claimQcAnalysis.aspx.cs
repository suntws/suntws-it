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
using System.Globalization;

namespace TTS
{
    public partial class claimQcAnalysis : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["claim_qc_mmn"].ToString() == "True" ||
                            dtUser.Rows[0]["claim_qc_sltl"].ToString() == "True" || dtUser.Rows[0]["claim_qc_sitl"].ToString() == "True" ||
                            dtUser.Rows[0]["claim_qc_pdk"].ToString() == "True"))
                        {
                            Complaint_Description();

                            ddlQcPlant.Items.Insert(0, "CHOOSE");
                            if (dtUser.Rows[0]["claim_qc_mmn"].ToString() == "True")
                                ddlQcPlant.Items.Insert(ddlQcPlant.Items.Count, "MMN");
                            if (dtUser.Rows[0]["claim_qc_sltl"].ToString() == "True")
                                ddlQcPlant.Items.Insert(ddlQcPlant.Items.Count, "SLTL");
                            if (dtUser.Rows[0]["claim_qc_sitl"].ToString() == "True")
                                ddlQcPlant.Items.Insert(ddlQcPlant.Items.Count, "SITL");
                            if (dtUser.Rows[0]["claim_qc_pdk"].ToString() == "True")
                                ddlQcPlant.Items.Insert(ddlQcPlant.Items.Count, "PDK");

                            if (ddlQcPlant.Items.Count == 2)
                            {
                                ddlQcPlant.SelectedIndex = 1;
                                ddlQcPlant_IndexChanged(sender, e);
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
        private void Complaint_Description()
        {
            try
            {
                DataTable dtTread = (DataTable)daTTS.ExecuteReader_SP("Sp_sel_Tread_From_TypeMaster", DataAccess.Return_Type.DataTable);
                ddlTread.DataSource = dtTread;
                ddlTread.DataTextField = "tread";
                ddlTread.DataValueField = "tread";
                ddlTread.DataBind();
                ddlTread.Items.Add("OTHERS");
                ddlTread.Items.Insert(0, "CHOOSE");

                DataTable dtCenter = (DataTable)daTTS.ExecuteReader_SP("Sp_sel_Center_From_TypeMaster", DataAccess.Return_Type.DataTable);
                ddlCenter.DataSource = dtCenter;
                ddlCenter.DataTextField = "center";
                ddlCenter.DataValueField = "center";
                ddlCenter.DataBind();
                ddlCenter.Items.Add("OTHERS");
                ddlCenter.Items.Insert(0, "CHOOSE");

                DataTable dtBase = (DataTable)daTTS.ExecuteReader_SP("Sp_sel_Base_From_TypeMaster", DataAccess.Return_Type.DataTable);
                ddlBase.DataSource = dtBase;
                ddlBase.DataTextField = "base";
                ddlBase.DataValueField = "base";
                ddlBase.DataBind();
                ddlBase.Items.Add("OTHERS");
                ddlBase.Items.Insert(0, "CHOOSE");

                DataTable dtComplaintype = (DataTable)daCOTS.ExecuteReader_SP("sp_ddl_Complainttype_ClaimLibrary", DataAccess.Return_Type.DataTable);
                ddlComplaintDesc.DataSource = dtComplaintype;
                ddlComplaintDesc.DataTextField = "Complaintype";
                ddlComplaintDesc.DataValueField = "Complaintype";
                ddlComplaintDesc.DataBind();
                ddlComplaintDesc.Items.Insert(0, "CHOOSE");
                ddlComplaintDesc.Items.Insert(1, "ADD NEW ENTRY");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlQcPlant_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "NO RECORDS";
                gvClaimItems.DataSource = null;
                gvClaimItems.DataBind();
                lblCustName.Text = "";
                lblComplaintNo.Text = "";
                gvClaimNoList.DataSource = null;
                gvClaimNoList.DataBind();
                hdnStencilPlant.Value = ddlQcPlant.SelectedItem.Text;
                if (hdnStencilPlant.Value != "" && hdnStencilPlant.Value != "CHOOSE")
                {
                    lblPageHead.Text = "CLAIM " + hdnStencilPlant.Value + " QC ANALYSIS";
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimNoList_AssignWise", sp1, DataAccess.Return_Type.DataTable);
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
            ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoClaimDiv('divgvclaim');", true);
        }
        private void ClaimNoClick(GridViewRow clickedRow)
        {
            try
            {
                lblMsgForQcAddtionalDetails.Text = "";
                hdnselectedrow.Value = "";
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                lblComplaintDate.Text = clickedRow.Cells[2].Text;
                lblCustName.Text = clickedRow.Cells[0].Text;
                lblComplaintNo.Text = clickedRow.Cells[1].Text;
                HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;

                if (lblClaimCustName.Text != "" && lblClaimNo.Text != "")
                    lblClaim.Text = "-";
                hdnCustCode.Value = hdnClaimCustCode.Value;
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[2] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                // get information about stencil no like config, type, qty,running hours, comments, analysis status etc....
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimStencilList_AssignWise", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvClaimItems.DataSource = dt;
                    gvClaimItems.DataBind();
                    bool assystatus = false;
                    foreach (DataRow dr in dt.Select("AnalysisStatus='Pending' or AnalysisStatus='Recheck' or QcAdditionalStatus='QC REQUEST'"))
                    {
                        assystatus = true;
                        break;
                    }
                    // if already analyzed then show the analysis statements else proceed further
                    if (!assystatus)
                        ScriptManager.RegisterStartupScript(Page, GetType(), "status1", "showAnalysisData('tdAnalysisDataqc');", true);
                    else
                    {
                        int j = 0, k = 0;
                        j = dt.AsEnumerable().Where(p => p.Field<string>("QcAdditionalStatus") == "QC REQUEST").Count();
                        k = dt.AsEnumerable().Where(p => p.Field<string>("AnalysisStatus") != "Pending" && ("AnalysisStatus") != "Recheck").Count();
                        if (j > 0)
                            lblMsgForQcAddtionalDetails.Text = "You have chosen " + j + " stencil no. for additional details asked from CRM";
                        if (lblMsgForQcAddtionalDetails.Text != "" && j > 0 && dt.Rows.Count == (j + k))
                            ScriptManager.RegisterStartupScript(Page, GetType(), "status3", "showAnalysisData('tdQcAskAdditionalDetails');", true);
                    }

                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp2[2] = new SqlParameter("@plant", hdnStencilPlant.Value);
                    DataTable dtComm = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complaintcommets", sp2, DataAccess.Return_Type.DataTable);

                    if (dtComm.Rows[0]["Commets"].ToString() != "")
                        lblComplaintComments.Text = "<span class='headCss'>CUSTOMER COMMENTS</span><br/>" + dtComm.Rows[0]["Commets"].ToString().Replace("~", "<br/>");
                    else
                        lblComplaintComments.Text = "";
                }
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
                }

                if (dtImage.Rows.Count > 0)
                {
                    gvClaimImages.DataSource = dtImage;
                    gvClaimImages.DataBind();

                    dlImageList.DataSource = dtImage;
                    dlImageList.DataBind();
                    foreach (DataListItem item in dlImageList.Items)
                    {
                        CheckBox chk1 = item.FindControl("chk1") as CheckBox;
                        HiddenField hdn1 = item.FindControl("hdn1") as HiddenField;
                        hdnImagesUrl.Value = hdn1.Value;
                        chk1.Checked = true;
                        break;
                    }
                    ViewState["dtImage"] = dtImage;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        // if prepare/edit-analysis link is clicked 
        protected void lnkClaimItem_Click(object sender, EventArgs e)
        {
            GridViewRow rowClick = ((LinkButton)sender).NamingContainer as GridViewRow;
            LinkButton lnk = rowClick.FindControl("lnkClaimItem") as LinkButton;
            hdnClaimStencilClick.Value = Convert.ToString(rowClick.RowIndex);
            if (lnk.Text == "Prepare/Edit Analysis")
                ClaimStencilClick(rowClick);
            else
            {
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[2] = new SqlParameter("@stencilno", rowClick.Cells[2].Text);
                sp1[3] = new SqlParameter("@Plant", hdnStencilPlant.Value);
                daCOTS.ExecuteNonQuery_SP("sp_cancel_Qc_AdditionalDetailsStencilNo", sp1);
                gvClaimNoList.SelectedIndex = Convert.ToInt32(hdnClaimNoClick.Value);
                ClaimNoClick(gvClaimNoList.SelectedRow);
            }

        }
        private void ClaimStencilClick(GridViewRow rowClick)
        {
            try
            {
                if (hdnselectedrow.Value != "")
                {
                    // maintain the previous row identity to toggle the highlight and de-highlight
                    var previousRowIndex = Convert.ToInt32(hdnselectedrow.Value);
                    GridViewRow PreviousRow = gvClaimItems.Rows[previousRowIndex];
                    PreviousRow.BackColor = System.Drawing.Color.White;
                }
                // highlight the current row with yellow color;
                rowClick.BackColor = System.Drawing.Color.Yellow;
                hdnselectedrow.Value = rowClick.RowIndex.ToString();
                lblClaimBrand.Text = rowClick.Cells[0].Text;
                lblClaimSize.Text = rowClick.Cells[1].Text;
                lblClaimStencils.Text = rowClick.Cells[2].Text;
                hdnType.Value = ((HiddenField)rowClick.FindControl("hdnTyreType")).Value;
                HiddenField hdnCustgvnType = rowClick.FindControl("hdnCustgvnType") as HiddenField;
                HiddenField hdnReanalysisComment = rowClick.FindControl("hdnReanalysisComment") as HiddenField;
                hdnPlatform.Value = ((HiddenField)rowClick.FindControl("hdnConfig")).Value;
                lblReanalysisComment.Text = hdnReanalysisComment.Value == "" ? "" : "<span class='headCss'>Reanalysis Comments: </span>" + hdnReanalysisComment.Value.Replace("~", "\n");
                lblCustgvnType.Text = hdnCustgvnType.Value == "" ? "" : "<span class='headCss'>Customer Mentioned: </span>" + hdnCustgvnType.Value;

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Tyresize", lblClaimSize.Text) };
                DataTable dtConfig = (DataTable)daTTS.ExecuteReader_SP("Sp_sel_Config_From_SizeWise", sp, DataAccess.Return_Type.DataTable);
                ddlConfig.DataSource = dtConfig;
                ddlConfig.DataTextField = "config";
                ddlConfig.DataValueField = "config";
                ddlConfig.DataBind();
                ddlConfig.Items.Insert(0, "CHOOSE");

                ddlComplaintDesc.SelectedIndex = ddlComplaintDesc.Items.IndexOf(ddlComplaintDesc.Items.FindByText("" +
                    ((HiddenField)rowClick.FindControl("hdnComplaintDesc")).Value + ""));

                ddlConfig.SelectedIndex = ddlConfig.Items.IndexOf(ddlConfig.Items.FindByText("" + hdnPlatform.Value + ""));
                Bind_Type_ConfigWise(hdnPlatform.Value);

                ddlTyretype.SelectedIndex = ddlTyretype.Items.IndexOf(ddlTyretype.Items.FindByText("" + hdnType.Value + ""));

                Build_ClaimImages(lblClaimStencils.Text);
                Clear_All_TextBoxes();
                Bind_AlreadyUpdatedDetails_txt();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen1", "showParticularItem('divParticularItem','block');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jdiv14", "showParticularItem('divSaveItem','block');", true);
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
                ddlTyretype.DataSource = dtType;
                ddlTyretype.DataTextField = "TyreType";
                ddlTyretype.DataValueField = "TyreType";
                ddlTyretype.DataBind();
            }
            ListItem selectedListItem = ddlTyretype.Items.FindByText("CHOOSE");
            if (selectedListItem == null)
            {
                ddlTyretype.Items.Insert(0, "CHOOSE");
            }
        }
        // where is the gvClaimImages is used when executing in browser ?
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
        private void Clear_All_TextBoxes()
        {
            ddlTread.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;
            ddlBase.SelectedIndex = -1;
            ddlShift.SelectedIndex = -1;
            txtProDate.Text = "";
            txtPressDetails.Text = "";
            txtMouldDetails.Text = "";
            txtMouldHistory.Text = "";
            txtTreadComp.Text = "";
            txtCenterComp.Text = "";
            txtBaseComp.Text = "";
            txtWtReq.Text = "";
            txtWtAct.Text = "";
            txtSteamReq.Text = "";
            txtSteamAct.Text = "";
            txtHyReq.Text = "";
            txtHyAct.Text = "";
            txtTempReq.Text = "";
            txtTempAct.Text = "";
            txtChk1Req.Text = "";
            txtChk1Act.Text = "";
            txtChk2Req.Text = "";
            txtChk2Act.Text = "";
            txtBuildStart.Text = "";
            txtBuildEnd.Text = "";
            txtUlStart.Text = "";
            txtUlEnd.Text = "";
            txtCureStart.Text = "";
            txtCureEnd.Text = "";
            txtQcComments.Text = "";
            txtQcAdditional.Text = "";
        }
        private void Bind_AlreadyUpdatedDetails_txt()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblComplaintNo.Text);
                sp1[2] = new SqlParameter("@stencilno", lblClaimStencils.Text);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimAnalysis_EachStencil", sp1, DataAccess.Return_Type.DataTable);

                // clarify about these prodate,basecomp, centercomp, treadcomp 
                if (dt.Rows.Count > 0)
                {
                    txtQcAdditional.Enabled = true;
                    DataRow row = dt.Rows[0];
                    txtQcAdditional.Text = row["QcAdditionalReq"].ToString() != "" ? "QC:" + row["QcAdditionalReq"].ToString().Replace("~", "\r\n") : "";
                    if (row["QcAdditionalStatus"].ToString() == "UPDATED")
                    {
                        txtQcAdditional.Text += "\r\nCRM:" + row["QcAdditionalUpdateComments"].ToString().Replace("~", "\r\n");
                        txtQcAdditional.Enabled = false;
                    }
                    if (txtQcAdditional.Text != "" && row["QcAdditionalStatus"].ToString() == "QC REQUEST")
                    {
                        btnSaveClaimAnalysis.Text = "SAVE ADDTIONAL DETAILS REQUEST";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Jdiv11", "showParticularItem('divParticularItem','none');", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Jdiv12", "showParticularItem('divSaveItem','block');", true);
                    }
                    else
                    {
                        btnSaveClaimAnalysis.Text = "SAVE ANALYSIS DETAILS";
                        ListItem selectedConfig = ddlConfig.Items.FindByText("" + row["config"].ToString() + "");
                        if (selectedConfig != null)
                        {
                            ddlConfig.Items.FindByText(ddlConfig.SelectedItem.Text).Selected = false;
                            selectedConfig.Selected = true;
                        }
                        ListItem selectedType = ddlTyretype.Items.FindByText("" + row["tyretype"].ToString() + "");
                        if (selectedType != null)
                        {
                            ddlTyretype.Items.FindByText(ddlTyretype.SelectedItem.Text).Selected = false;
                            selectedType.Selected = true;
                        }
                        if (row["prodate"].ToString() == "15/08/1947")
                        {
                            checkdate.Checked = true;
                            txtProDate.Text = "NA";
                        }
                        else
                            txtProDate.Text = row["prodate"].ToString();
                        if (row["shift"].ToString() != "")
                            ddlShift.SelectedIndex = Convert.ToInt32(row["shift"].ToString());
                        txtPressDetails.Text = row["pressdetails"].ToString();
                        txtMouldDetails.Text = row["moulddetails"].ToString();
                        txtMouldHistory.Text = row["mouldhistory"].ToString();
                        if (row["treadcomp"].ToString() != "")
                        {
                            txtTreadComp.Text = row["treadcomp"].ToString();
                            ListItem selectedTread = ddlTread.Items.FindByText("" + txtTreadComp.Text + "");
                            selectedTread = selectedTread == null ? ddlTread.Items.FindByText("OTHERS") : selectedTread;
                            if (selectedTread != null)
                            {
                                ddlTread.Items.FindByText(ddlTread.SelectedItem.Text).Selected = false;
                                selectedTread.Selected = true;
                            }
                            if (ddlTread.SelectedItem.Text == "OTHERS")
                                ScriptManager.RegisterStartupScript(Page, GetType(), "Jdiv1", "showParticularItem('divTread','block');", true);
                        }
                        if (row["centercomp"].ToString() != "")
                        {
                            txtCenterComp.Text = row["centercomp"].ToString();
                            ListItem selectedCenter = ddlCenter.Items.FindByText("" + txtCenterComp.Text + "");
                            selectedCenter = selectedCenter == null ? ddlCenter.Items.FindByText("OTHERS") : selectedCenter;
                            if (selectedCenter != null)
                            {
                                ddlCenter.Items.FindByText(ddlCenter.SelectedItem.Text).Selected = false;
                                selectedCenter.Selected = true;
                            }
                            if (ddlCenter.SelectedItem.Text == "OTHERS")
                                ScriptManager.RegisterStartupScript(Page, GetType(), "Jdiv2", "showParticularItem('divCenter','block');", true);
                        }
                        if (row["basecomp"].ToString() != "")
                        {
                            txtBaseComp.Text = row["basecomp"].ToString();
                            ListItem selectedBase = ddlBase.Items.FindByText("" + txtBaseComp.Text + "");
                            selectedBase = selectedBase == null ? ddlBase.Items.FindByText("OTHERS") : selectedBase;
                            if (selectedBase != null)
                            {
                                ddlBase.Items.FindByText(ddlBase.SelectedItem.Text).Selected = false;
                                selectedBase.Selected = true;
                            }
                            if (ddlBase.SelectedItem.Text == "OTHERS")
                                ScriptManager.RegisterStartupScript(Page, GetType(), "Jdiv3", "showParticularItem('divBase','block');", true);
                        }
                        txtWtReq.Text = row["wtReq"].ToString();
                        txtWtAct.Text = row["wtAct"].ToString();
                        txtSteamReq.Text = row["steamReq"].ToString();
                        txtSteamAct.Text = row["steamAct"].ToString();
                        txtHyReq.Text = row["hydReq"].ToString();
                        txtHyAct.Text = row["hydAct"].ToString();
                        txtTempReq.Text = row["tempReq"].ToString();
                        txtTempAct.Text = row["tempAct"].ToString();
                        txtChk1Req.Text = row["chk1Req"].ToString();
                        txtChk1Act.Text = row["chk1Act"].ToString();
                        txtChk2Req.Text = row["chk2Req"].ToString();
                        txtChk2Act.Text = row["chk2Act"].ToString();
                        txtBuildStart.Text = row["buildStart"].ToString();
                        txtBuildEnd.Text = row["buildEnd"].ToString();
                        txtUlStart.Text = row["ulStart"].ToString();
                        txtUlEnd.Text = row["ulEnd"].ToString();
                        txtCureStart.Text = row["cureStart"].ToString();
                        txtCureEnd.Text = row["cureEnd"].ToString();
                        txtQcComments.Text = row["QcComments"].ToString().Replace("~", "\r\n");
                        txtHardReq.Text = row["hardReq"].ToString();
                        txtHardAct.Text = row["hardAct"].ToString();
                        txtCuringReq.Text = row["cureReq"].ToString();
                        txtCuringAct.Text = row["cureAct"].ToString();

                        if (row["ImgUrls"].ToString() != "")
                        {
                            string[] str1 = row["ImgUrls"].ToString().Split('~');
                            foreach (string strImgVal in str1)
                            {
                                foreach (DataListItem item in dlImageList.Items)
                                {
                                    CheckBox chk1 = item.FindControl("chk1") as CheckBox;
                                    HiddenField hdn1 = item.FindControl("hdn1") as HiddenField;
                                    if (hdn1.Value == strImgVal)
                                    {
                                        chk1.Checked = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveClaimAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                btnSaveClaimAnalysis.Text = hdnSaveType.Value;
                if (btnSaveClaimAnalysis.Text == "SAVE ADDTIONAL DETAILS REQUEST")
                {
                    SqlParameter[] spQc = new SqlParameter[6];
                    spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    spQc[2] = new SqlParameter("@stencilno", lblClaimStencils.Text);
                    spQc[3] = new SqlParameter("@QcAdditionalReq", txtQcAdditional.Text.Replace("\n", "~"));
                    spQc[4] = new SqlParameter("@QcReqUser", Request.Cookies["TTSUser"].Value);
                    spQc[5] = new SqlParameter("@Plant", hdnStencilPlant.Value);
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_Qc_AdditionalDetailsStencilNo", spQc);

                }
                else if (btnSaveClaimAnalysis.Text == "SAVE ANALYSIS DETAILS")
                {
                    string strComplaintDesc = ddlComplaintDesc.SelectedValue == "ADD NEW ENTRY" ? txtComplaintDesc.Text.Trim().ToUpper() : ddlComplaintDesc.SelectedValue;
                    if (ddlComplaintDesc.SelectedValue == "ADD NEW ENTRY")
                    {
                        SqlParameter[] sp = new SqlParameter[8];
                        sp[0] = new SqlParameter("@Complaintype", txtComplaintDesc.Text.Trim().ToUpper());
                        sp[1] = new SqlParameter("@Apperance", "");
                        sp[2] = new SqlParameter("@ManufacturingEnd", "");
                        sp[3] = new SqlParameter("@CustomerEnd", "");
                        sp[4] = new SqlParameter("@actioncomments", "");
                        sp[5] = new SqlParameter("@warranty", "");
                        sp[6] = new SqlParameter("@imgcount", Convert.ToInt32("0"));
                        sp[7] = new SqlParameter("@CreatedUser", Request.Cookies["TTSUser"].Value);
                        daCOTS.ExecuteNonQuery_SP("sp_ins_ClaimLibrary", sp);
                        Complaint_Description();
                    }
                    SqlParameter[] sp1 = new SqlParameter[40];
                    sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp1[2] = new SqlParameter("@stencilno", lblClaimStencils.Text);
                    sp1[3] = new SqlParameter("@prodate", DateTime.ParseExact(hdnProDate.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    sp1[4] = new SqlParameter("@shift", Convert.ToInt32(ddlShift.SelectedIndex));
                    sp1[5] = new SqlParameter("@pressdetails", txtPressDetails.Text);
                    sp1[6] = new SqlParameter("@moulddetails", txtMouldDetails.Text);
                    sp1[7] = new SqlParameter("@mouldhistory", txtMouldHistory.Text);
                    sp1[8] = new SqlParameter("@treadcomp", txtTreadComp.Text);
                    sp1[9] = new SqlParameter("@centercomp", txtCenterComp.Text);
                    sp1[10] = new SqlParameter("@basecomp", txtBaseComp.Text);
                    sp1[11] = new SqlParameter("@wtReq", txtWtReq.Text);
                    sp1[12] = new SqlParameter("@wtAct", txtWtAct.Text);
                    sp1[13] = new SqlParameter("@steamReq", txtSteamReq.Text);
                    sp1[14] = new SqlParameter("@steamAct", txtSteamAct.Text);
                    sp1[15] = new SqlParameter("@hydReq", txtHyReq.Text);
                    sp1[16] = new SqlParameter("@hydAct", txtHyAct.Text);
                    sp1[17] = new SqlParameter("@tempReq", txtTempReq.Text);
                    sp1[18] = new SqlParameter("@tempAct", txtTempAct.Text);
                    sp1[19] = new SqlParameter("@chk1Req", txtChk1Req.Text);
                    sp1[20] = new SqlParameter("@chk1Act", txtChk1Act.Text);
                    sp1[21] = new SqlParameter("@chk2Req", txtChk2Req.Text);
                    sp1[22] = new SqlParameter("@chk2Act", txtChk2Act.Text);
                    sp1[23] = new SqlParameter("@buildStart", txtBuildStart.Text);
                    sp1[24] = new SqlParameter("@buildEnd", txtBuildEnd.Text);
                    sp1[25] = new SqlParameter("@ulStart", txtUlStart.Text);
                    sp1[26] = new SqlParameter("@ulEnd", txtUlEnd.Text);
                    sp1[27] = new SqlParameter("@cureStart", txtCureStart.Text);
                    sp1[28] = new SqlParameter("@cureEnd", txtCureEnd.Text);
                    sp1[29] = new SqlParameter("@QcComments", txtQcComments.Text.Replace("\r\n", "~"));
                    sp1[30] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    sp1[31] = new SqlParameter("@ImgUrls", hdnImagesUrl.Value.Replace("/tts/", "/"));
                    sp1[32] = new SqlParameter("@hardReq", txtHardReq.Text);
                    sp1[33] = new SqlParameter("@hardAct", txtHardAct.Text);
                    sp1[34] = new SqlParameter("@cureReq", txtCuringReq.Text);
                    sp1[35] = new SqlParameter("@cureAct", txtCuringAct.Text);
                    sp1[36] = new SqlParameter("@config", hdnPlatform.Value);
                    sp1[37] = new SqlParameter("@tyretype", hdnType.Value);
                    sp1[38] = new SqlParameter("@plant", hdnStencilPlant.Value);
                    sp1[39] = new SqlParameter("@ClaimDescription", strComplaintDesc);
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_ClaimAnalysis_EachStencil", sp1);
                    if (resp > 0)
                        lblSaveMsg.Text = "Record Saved successfully";
                }
                if (resp > 0)
                {
                    gvClaimNoList.SelectedIndex = Convert.ToInt32(hdnClaimNoClick.Value);
                    ClaimNoClick(gvClaimNoList.SelectedRow);
                    gvClaimItems.SelectedIndex = Convert.ToInt32(hdnClaimStencilClick.Value);
                    ClaimStencilClick(gvClaimItems.SelectedRow);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen1", "showParticularItem('divParticularItem','block');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Jdiv13", "showParticularItem('divSaveItem','block');", true);
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
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@QcAnalysiscomments", txtComments.Text.Replace("\r\n", "~"));
                sp1[3] = new SqlParameter("@QcAnalysisuser", Request.Cookies["TTSUser"].Value);
                sp1[4] = new SqlParameter("@plant", hdnStencilPlant.Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_MoveToEDC_FromQc", sp1);
                if (resp > 0)
                {
                    Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnStencilPlant.Value, "12", txtComments.Text, Request.Cookies["TTSUser"].Value);
                    Response.Redirect("default.aspx", false);
                }
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
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_QcAdditionalDetailsFromCrm", spQc);
                if (resp > 0)
                {
                    Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, hdnStencilPlant.Value, "4", txtComments.Text.Replace("\r\n", "~"), Request.Cookies["TTSUser"].Value);
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}