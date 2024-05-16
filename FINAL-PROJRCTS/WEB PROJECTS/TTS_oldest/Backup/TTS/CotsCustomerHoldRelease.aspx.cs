using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
namespace TTS
{
    public partial class CotsCustomerHoldRelease : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    lblErrMsgcontent.Text = "";
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_paymentcontrol"].ToString() == "True")
                        {
                            if (Request["qid"] != null && (Request["qid"].ToString() == "0" || Request["qid"].ToString() == "1"))
                            {
                                gv_customerdetails.DataSource = null;
                                gv_customerdetails.DataBind();
                                SqlParameter[] sp = new SqlParameter[1];
                                sp[0] = new SqlParameter("@CustHoldStatus", Convert.ToInt32(Request["qid"].ToString()));
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_sel_HoldStatus_usermaster", sp, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gv_customerdetails.DataSource = dt;
                                    gv_customerdetails.DataBind();
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow11", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "Wrong URL. Please reload this page.";
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
        protected void lnkaction_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow ClickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblCustomerName.Text = ClickedRow.Cells[0].Text;
                HiddenField hdnStatusCustCode = (HiddenField)ClickedRow.FindControl("hdnStatusCustCode");
                hdncustcode.Value = hdnStatusCustCode.Value;
                if (Request["qid"].ToString() == "0")
                {
                    lblpurposemessage.Text = "Purpose of Hold";
                    btnStatusChange.Text = "HOLD";
                }
                else if (Request["qid"].ToString() == "1")
                {
                    lblpurposemessage.Text = "Purpose of Revoke";
                    btnStatusChange.Text = "REVOKE";
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JToopensubmenu", "openSubMenu();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('td_SubMenu');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnStatusChange_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean HoldStatus = Request["qid"].ToString() == "0" ? true : false;
                SqlParameter[] sp = new SqlParameter[4];
                sp[0] = new SqlParameter("@CustCode", hdncustcode.Value);
                sp[1] = new SqlParameter("@PurposeMessage", txtpurposemessage.Text);
                sp[2] = new SqlParameter("@CustHoldStatus", HoldStatus);
                sp[3] = new SqlParameter("@ModifiedUserName", Request.Cookies["TTSUser"].Value);
                daCOTS.ExecuteNonQuery_SP("Sp_ins_ScotsCustHoldHistory", sp);

                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@CustCode", hdncustcode.Value);
                sp1[1] = new SqlParameter("@CustHoldStatus", HoldStatus);
                daCOTS.ExecuteNonQuery_SP("SP_upt_Holdstatus_usermaster", sp1);

                Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}