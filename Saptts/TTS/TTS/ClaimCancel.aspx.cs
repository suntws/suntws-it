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
    public partial class ClaimCancel : System.Web.UI.Page
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
                        if (Request["pid"] != null && Request["pid"].ToString() != "")
                        {
                            DataTable dtUser = Session["dtuserlevel"] as DataTable;
                            if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["claim_crm_exp"].ToString() == "True" || dtUser.Rows[0]["claim_crm_dom"].ToString() == "True"))
                            {
                                if (Request["aid"].ToString() == "1")
                                    lblTrackHead.Text = Request["pid"].ToString() == "e" ? "EXPORT" + " CANCEL CLAIM" : "DOMESTIC" + " CANCEL CLAIM";
                                else if (Request["aid"].ToString() == "2")
                                    lblTrackHead.Text = "CANCELLED CLAIM LIST - " + Request["pid"].ToString() == "e" ? "EXPORT" : "DOMESTIC";
                                gv_bind();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "URL IS WRONG";
                        }
                    }
                }
                else
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        private void gv_bind()
        {
            try
            {
                DataTable dt = new DataTable();
                gvClaimTrackList.DataSource = null;
                gvClaimTrackList.DataBind();
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                if (Request["aid"].ToString() == "1")
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claim_cancel_list", sp1, DataAccess.Return_Type.DataTable);
                else if (Request["aid"].ToString() == "2")
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claim_canceled", sp1, DataAccess.Return_Type.DataTable);
                ViewState["dt"] = dt;
                gvClaimTrackList.DataSource = dt;
                gvClaimTrackList.DataBind();
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
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;
                if (lblClaimCustName.Text != "" && lblClaimNo.Text != "")
                    lblClaim.Text = "-";
                hdnCustCode.Value = hdnClaimCustCode.Value;
                gvClaimApproveItems.DataSource = null;
                gvClaimApproveItems.DataBind();
                string strPlant = clickedRow.Cells[3].Text.Replace("&amp;", "&");
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimTrack_ItemList_cancel", sp2, DataAccess.Return_Type.DataTable);
                if (dtItems.Rows.Count > 0)
                {
                    gvClaimApproveItems.DataSource = dtItems;
                    gvClaimApproveItems.DataBind();
                    if (Request["aid"].ToString() == "1")
                    {
                        bool status = false;
                        foreach (DataRow dr in dtItems.Rows)
                        {
                            if (Convert.ToInt32(dr["statusid"].ToString()) > 20 || dr["CondCrmDate"].ToString() != "")
                                status = true;
                        }
                        if (status == false)
                        {
                            lblMsg.Text = "";
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "displayblock('divcancel');", true);
                        }
                        else
                            lblMsg.Text = "FOR THIS COMPLAINT NO MOVED FOR CREDIT NOTE PREPARTATION.SO YOU CAN'T CANCLE THE COMPLAINT.PLEASE CONTACT ADMIN";
                    }
                    if (Request["aid"].ToString() == "2")
                    {
                        SqlParameter[] sp = new SqlParameter[2];
                        sp[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                        sp[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                        DataTable dtComm = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Claim_cancel_comments", sp, DataAccess.Return_Type.DataTable);
                        if (dtComm.Rows.Count > 0)
                        {
                            lblcancelComments.Text = "<span class='headCss'>CANCELLED COMMENTS</span><br/>" + dtComm.Rows[0]["CancelComment"].ToString().Replace("~", "<br/>");
                            lblcanceluser.Text = "<span class='headCss'>CANCELLED USER & DATE</span><br/>" + dtComm.Rows[0]["CancelUser"].ToString() + " & " + dtComm.Rows[0]["Canceldate"].ToString();
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "displayblock('divcancelcomment');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[4];
                sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp2[2] = new SqlParameter("@CancelComment", txtCancelComments.Text.Replace("\r\n", "~"));
                sp2[3] = new SqlParameter("@CancelUser", Request.Cookies["TTSUser"].Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_Claim_cancel", sp2);
                if (resp > 0)
                {
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private string Build_NotificationMailID()
        {
            DataTable dt = ViewState["dt"] as DataTable;
            string returnValue = string.Empty;
            foreach (DataRow dr in dt.Select("custcode='" + hdnCustCode.Value + "' and complaintno='" + lblClaimNo.Text + "'"))
            {
                if (dr["statusid"].ToString() != "1")
                {
                    if (Convert.ToInt32(dr["statusid"].ToString()) > 1)
                    {
                        if (dr["plant"].ToString() == "MMN")
                        {
                            if (returnValue.Length > 0)
                                returnValue += "," + Utilities.Build_Cliam_ToList("claim_qc_mmn");
                            else
                                returnValue = Utilities.Build_Cliam_ToList("claim_qc_mmn");
                        }
                        if (dr["plant"].ToString() == "SLTL")
                        {
                            if (returnValue.Length > 0)
                                returnValue += "," + Utilities.Build_Cliam_ToList("claim_qc_sltl");
                            else
                                returnValue = Utilities.Build_Cliam_ToList("claim_qc_sltl");
                        }
                        if (dr["plant"].ToString() == "SITL")
                        {
                            if (returnValue.Length > 0)
                                returnValue += "," + Utilities.Build_Cliam_ToList("claim_qc_sitl");
                            else
                                returnValue = Utilities.Build_Cliam_ToList("claim_qc_sitl");
                        }
                        if (dr["plant"].ToString() == "PDK")
                        {
                            if (returnValue.Length > 0)
                                returnValue += "," + Utilities.Build_Cliam_ToList("claim_qc_pdk");
                            else
                                returnValue = Utilities.Build_Cliam_ToList("claim_qc_pdk");
                        }
                    }
                    if (Convert.ToInt32(dr["statusid"].ToString()) < 20 && Convert.ToInt32(dr["statusid"].ToString()) >= 10)
                    {
                        if (dr["plant"].ToString() == "MMN")
                        {
                            if (returnValue.Length > 0)
                                returnValue += "," + Utilities.Build_Cliam_ToList("claim_edc_mmn");
                            else
                                returnValue = Utilities.Build_Cliam_ToList("claim_edc_mmn");
                        }
                        if (dr["plant"].ToString() == "SLTL")
                        {
                            if (returnValue.Length > 0)
                                returnValue += "," + Utilities.Build_Cliam_ToList("claim_edc_sltl");
                            else
                                returnValue = Utilities.Build_Cliam_ToList("claim_edc_sltl");
                        }
                        if (dr["plant"].ToString() == "SITL")
                        {
                            if (returnValue.Length > 0)
                                returnValue += "," + Utilities.Build_Cliam_ToList("claim_edc_sitl");
                            else
                                returnValue = Utilities.Build_Cliam_ToList("claim_edc_sitl");
                        }
                        if (dr["plant"].ToString() == "PDK")
                        {
                            if (returnValue.Length > 0)
                                returnValue += "," + Utilities.Build_Cliam_ToList("claim_edc_pdk");
                            else
                                returnValue = Utilities.Build_Cliam_ToList("claim_edc_pdk");
                        }
                    }
                }
                else
                {
                    if (returnValue.Length > 0)
                        returnValue += "," + Utilities.Build_Cliam_ToList("claim_stencilassign");
                    else
                        returnValue = Utilities.Build_Cliam_ToList("claim_stencilassign");
                }
            }
            return returnValue;
        }
        protected void gvClaimTrackList_OnDataBound(object sender, EventArgs e)
        {
            for (int i = gvClaimTrackList.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gvClaimTrackList.Rows[i];
                GridViewRow previousRow = gvClaimTrackList.Rows[i - 1];
                for (int j = 1; j < 2; j++)
                {
                    if (row.Cells[j].Text == previousRow.Cells[j].Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                                previousRow.Cells[6].RowSpan += 2;
                                previousRow.Cells[0].RowSpan += 2;
                                previousRow.Cells[2].RowSpan += 2;
                                previousRow.Cells[4].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                previousRow.Cells[6].RowSpan = row.Cells[6].RowSpan + 1;
                                previousRow.Cells[0].RowSpan = row.Cells[0].RowSpan + 1;
                                previousRow.Cells[2].RowSpan = row.Cells[2].RowSpan + 1;
                                previousRow.Cells[4].RowSpan = row.Cells[4].RowSpan + 1;
                            }
                            row.Cells[6].Visible = false;
                            row.Cells[j].Visible = false;
                            row.Cells[0].Visible = false;
                            row.Cells[2].Visible = false;
                            row.Cells[4].Visible = false;
                        }
                    }
                }
            }
        }
    }
}