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
    public partial class expscanincomplete : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "")
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True"|| dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk"].ToString() == "True"))
                        {
                            if (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageTitle.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ") + "INSPECTION INCOMPLETE";
                                SqlParameter[] sp = new SqlParameter[] { 
                                    new SqlParameter("@qstring", Utilities.Decrypt(Request["fid"].ToString())), 
                                    new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())) 
                                };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdi_incomplete_order", sp, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gv_PdiIncomplete.DataSource = dt;
                                    gv_PdiIncomplete.DataBind();
                                }
                                else
                                    lblErrMsg.Text = "NO RECORDS";
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL is wrong";
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
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnViewOrderForInspect_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow ClickedRow = ((Button)sender).NamingContainer as GridViewRow;
                HiddenField hdnCustCode = (HiddenField)ClickedRow.FindControl("hdnCustCode");
                HiddenField hdnPID = (HiddenField)ClickedRow.FindControl("hdnPID");
                HiddenField hdnOrderRefno = (HiddenField)ClickedRow.FindControl("hdnOrderRefno");
                Label lblWorkOrderNo = (Label)ClickedRow.FindControl("lblWorkOrderNo");
                HiddenField hdnOrderID = (HiddenField)ClickedRow.FindControl("hdnOrderID");

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@custcode", hdnCustCode.Value), 
                    new SqlParameter("@orderid", hdnOrderRefno.Value), 
                    new SqlParameter("@pdiPlant", Utilities.Decrypt(Request["pid"].ToString())) 
                };
                DataTable dtCHK = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_orderitemlist_entry", sp, DataAccess.Return_Type.DataTable);

                if (dtCHK != null && dtCHK.Rows.Count > 0)
                {
                    Response.Redirect("expscanpdi_1.aspx?pid=" + Request["pid"].ToString() + "&fid=" + Request["fid"].ToString() + "&ccode=" +
                        Utilities.Encrypt(hdnCustCode.Value) + "&oid=" + Utilities.Encrypt(hdnOrderRefno.Value) + "&mtype=" + Utilities.Encrypt("exists") +
                        "&id=" + Utilities.Encrypt(hdnOrderID.Value), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}