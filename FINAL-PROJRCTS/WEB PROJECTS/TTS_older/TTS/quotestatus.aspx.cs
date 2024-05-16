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
    public partial class quotestatus : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_quote"].ToString() == "True")
                        {
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Quote_ConfirmationList", DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                gvConfirmedQuote.DataSource = dt;
                                gvConfirmedQuote.DataBind();
                            }
                            else
                                lblErrMsg.Text = "No records";
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

        protected void lnkMoveToOriginal_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(row.RowIndex);
                Label lblCustomerType = (Label)row.FindControl("lblCustomerType");
                Label lblQRefNo = (Label)row.FindControl("lblQRefNo");
                Label lblQCustomer = (Label)row.FindControl("lblQCustomer");
                Label lblQAcYear = (Label)row.FindControl("lblQAcYear");
                HiddenField hdnUSERID = (HiddenField)row.FindControl("hdnUSERID");
                if (lblCustomerType.Text == "NEW CUSTOMER")
                {
                    Build_ConfirmedQuote_CustDeatils(row);
                }
                else if (lblCustomerType.Text == "EXSITING CUSTOMER")
                {
                    if (lblQRefNo.Text.Trim() != "" && hdnUSERID.Value.Trim() != "" && lblQAcYear.Text.Trim() != "")
                    {
                        if (Request.Url.Host.ToLower() == "www.suntws.com")
                            Response.Redirect("http://www.suntws.com/scots/login.aspx?qno=" + lblQRefNo.Text.Trim() + "&uid=" + hdnUSERID.Value.Trim() + "&qyr=" + lblQAcYear.Text.Trim(), false);
                        else
                            Response.Redirect("http://localhost:50227/scots/login.aspx?qno=" + lblQRefNo.Text.Trim() + "&uid=" + hdnUSERID.Value.Trim() + "&qyr=" + lblQAcYear.Text.Trim(), false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Build_ConfirmedQuote_CustDeatils(GridViewRow row)
        {
            try
            {
                Label lblQAcYear = (Label)row.FindControl("lblQAcYear");
                Label lblQRefNo = (Label)row.FindControl("lblQRefNo");
                Label lblQCustomer = (Label)row.FindControl("lblQCustomer");

                lblQuoteCustName.Text = lblQCustomer.Text;
                lblQuoteAcYear.Text = lblQAcYear.Text;
                lblQuoteRefNo.Text = lblQRefNo.Text;

                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@AcYear", lblQuoteAcYear.Text);
                sp1[1] = new SqlParameter("@RefNo", lblQRefNo.Text);
                DataTable dtDetails = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_QuoteCustomerSharedDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dtDetails.Rows.Count > 0)
                {
                    dlQuoteCustDetails.DataSource = dtDetails;
                    dlQuoteCustDetails.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JmoveShow", "gotoConfirmQuoteDiv('divQuoteConfirm');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}