using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Text;
using System.Xml;
using System.Web.Services;

namespace TTS
{
    public partial class CotsManualOrderIncomplete : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["dom_orderentry"].ToString() == "True" || dtUser.Rows[0]["exp_orderentry"].ToString() == "True"))
                        {
                            lblPageHead.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "e" ? "EXPORT" : "DOMESTIC") + " INCOMPLETE ORDER";
                            string strLeadName = "";
                            if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                                strLeadName = Request.Cookies["TTSUser"].Value;

                            SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@LeadWise", strLeadName), 
                                new SqlParameter("@QString", Utilities.Decrypt(Request["fid"].ToString())) 
                            };
                            DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsDomesticCustomer_incomplete", sp, DataAccess.Return_Type.DataTable);
                            if (dtList.Rows.Count > 0)
                            {
                                gv_Addeditems.DataSource = dtList;
                                gv_Addeditems.DataBind();
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
        protected void btnReviseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((Button)sender).NamingContainer as GridViewRow;
                Response.Redirect("cots_manualorderprepare_01.aspx?oid=" + Utilities.Encrypt(((HiddenField)row.FindControl("hdnOrderID")).Value) + "&fid=" +
                    Request["fid"].ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}