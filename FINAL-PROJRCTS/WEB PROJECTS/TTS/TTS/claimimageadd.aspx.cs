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

namespace TTS
{
    public partial class claimimageadd : System.Web.UI.Page
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
                                Bind_ClaimCategoryWise();
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
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_ClaimCategoryWise()
        {
            try
            {
                if (Request["aid"].ToString() == "0")
                {
                    hdnStencilPlant.Value = "SITL";
                    lblPageHead.Text = "CLAIM SITL STENCIL IMAGES ADD";
                }
                else if (Request["aid"].ToString() == "1")
                {
                    hdnStencilPlant.Value = "MMN";
                    lblPageHead.Text = "CLAIM MMN STENCIL IMAGES ADD";
                }
                else if (Request["aid"].ToString() == "2")
                {
                    hdnStencilPlant.Value = "SLTL";
                    lblPageHead.Text = "CLAIM SLTL STENCIL IMAGES ADD";
                }
                else if (Request["aid"].ToString() == "3")
                {
                    hdnStencilPlant.Value = "PDK";
                    lblPageHead.Text = "CLAIM PDK STENCIL IMAGES ADD";
                }
                if (hdnStencilPlant.Value != "")
                {
                    DataTable dt = new DataTable();
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@assigntoqc", hdnStencilPlant.Value);
                    if (Request["pid"].ToString().ToUpper() == "ME")
                        sp1[1] = new SqlParameter("@ClaimType", "E");
                    else
                        sp1[1] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                    if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                        sp1[2] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    else
                        sp1[2] = new SqlParameter("@username", "");
                    //if (Request["pid"].ToString().ToUpper() == "ME")
                    //    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmFeedBackList_MerchantExporter", sp1, DataAccess.Return_Type.DataTable);
                    //else
                    //    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmFeedBackList", sp1, DataAccess.Return_Type.DataTable);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    gvClaimImageAddList.DataSource = dt;
                    //    gvClaimImageAddList.DataBind();
                    //}
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}