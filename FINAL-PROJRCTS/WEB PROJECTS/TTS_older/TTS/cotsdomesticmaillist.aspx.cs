using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class cotsdomesticmaillist : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (Request.Cookies["TTSUser"].Value.ToLower() == "admin" || Request.Cookies["TTSUser"].Value.ToLower() == "somu" || Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                    {
                        if (!IsPostBack)
                        {
                            if (Request["mid"] != null)
                            {
                                bind_gvMailList();
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                        lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
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

        private void bind_gvMailList()
        {
            DataTable dt = new DataTable();
            if (Request["mid"].ToString() == "dom")
            {
                lblPageHead.Text = "DOMESTIC ORDER MAIL ALERT";
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_DomesticEmailAlert", DataAccess.Return_Type.DataTable);
            }
            else if (Request["mid"].ToString() == "exp")
            {
                lblPageHead.Text = "EXPORT ORDER MAIL ALERT";
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_DomesticEmailAlert", DataAccess.Return_Type.DataTable);
            }
            else if (Request["mid"].ToString() == "cla")
            {
                lblPageHead.Text = "CLAIM MAIL ALERT";
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimEmailAlert", DataAccess.Return_Type.DataTable);
            }
            else if (Request["mid"].ToString() == "otppc")
            {
                lblPageHead.Text = "ORDER TRACKING MAIL ALERT";
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PendingTrackingEmailAlert", DataAccess.Return_Type.DataTable);
            }
            if (dt.Rows.Count > 0)
            {
                gvCotsDomesticMailAlert.DataSource = dt;
                gvCotsDomesticMailAlert.DataBind();
            }
        }

        protected void gvCotsDomesticMailAlert_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtQuoteItem"] as DataTable;
                gvCotsDomesticMailAlert.EditIndex = e.NewEditIndex;
                bind_gvMailList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvCotsDomesticMailAlert_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvCotsDomesticMailAlert.Rows[e.RowIndex];
                Label lblSlNo = row.FindControl("lblSlNo") as Label;
                Label lblMailType = row.FindControl("lblMailType") as Label;
                TextBox txtReceipent = row.FindControl("txtReceipent") as TextBox;
                TextBox txtMailCC = row.FindControl("txtMailCC") as TextBox;

                if (lblSlNo.Text != "" && lblMailType.Text != "" && (txtReceipent.Text != "" || txtMailCC.Text != ""))
                {
                    SqlParameter[] sp1 = new SqlParameter[5];
                    sp1[0] = new SqlParameter("@ID", Convert.ToInt32(lblSlNo.Text));
                    sp1[1] = new SqlParameter("@MailType", lblMailType.Text);
                    sp1[2] = new SqlParameter("@MailReceipent", txtReceipent.Text);
                    sp1[3] = new SqlParameter("@MailCC", txtMailCC.Text);
                    sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                    if (Request["mid"].ToString() == "dom")
                        daCOTS.ExecuteNonQuery_SP("sp_edit_DomesticEmailAlert", sp1);
                    else if (Request["mid"].ToString() == "exp")
                        daCOTS.ExecuteNonQuery_SP("sp_edit_DomesticEmailAlert", sp1);
                    else if (Request["mid"].ToString() == "cla")
                        daCOTS.ExecuteNonQuery_SP("sp_edit_ClaimEmailAlert", sp1);
                    else if (Request["mid"].ToString() == "otppc")
                        daCOTS.ExecuteNonQuery_SP("sp_edit_PendingTrackingEmailAlert", sp1);
                }
                gvCotsDomesticMailAlert.EditIndex = -1;
                bind_gvMailList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvCotsDomesticMailAlert_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gvCotsDomesticMailAlert.EditIndex = -1;
                bind_gvMailList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}