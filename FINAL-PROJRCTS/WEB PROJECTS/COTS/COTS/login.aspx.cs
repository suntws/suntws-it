using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace COTS
{
    public partial class login : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request["qno"] != null && Request["uid"] != null && Request["qyr"] != null)
                    {
                        SqlParameter[] sqcook = new SqlParameter[1];
                        sqcook[0] = new SqlParameter("@userId", Request["uid"].ToString());
                        DataTable dtcook = new DataTable();
                        dtcook = (DataTable)daCOTS.ExecuteReader_SP("sp_loginCOTs_TTS", sqcook, DataAccess.Return_Type.DataTable);
                        if (dtcook.Rows.Count == 1)
                        {
                            txtUserName.Text = dtcook.Rows[0]["username"].ToString();
                            txtPassword.Text = dtcook.Rows[0]["userpassword"].ToString();
                            lnkLogin_click(sender, e);
                            Response.Redirect("frmshipaddress.aspx?qno=" + Request["qno"].ToString() + "&uid=" + Request["uid"].ToString() + "&qyr=" + Request["qyr"].ToString(), false);
                        }
                    }
                    else
                    {
                        if (Request["ReturnUrl"] != null && Request["ReturnUrl"].Contains(".aspx") == true)
                            hdnReturnUrl.Value = Request["ReturnUrl"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        protected void lnkLogin_click(object sender, EventArgs e)
        {
            try
            {
                Session["cotscode"] = "";
                Session["cotsstdcode"] = "";
                Session["cotsuser"] = "";
                Session["cotscur"] = "";
                Session["cotsuserfullname"] = "";
                Session["cotscategory"] = "";
                Session["trackorder"] = "";
                Session["hdnOID"] = "";
                Session["passwordchnaged"] = "";

                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@username", txtUserName.Text);
                sp1[1] = new SqlParameter("@userpassword", txtPassword.Text);

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_usermaster", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count == 1)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@UserName", txtUserName.Text);
                    daCOTS.ExecuteNonQuery_SP("sp_update_lastlogin", param);

                    Session["cotsuser"] = dt.Rows[0]["username"].ToString();
                    Session["cotscode"] = dt.Rows[0]["custcode"].ToString();
                    Session["cotscur"] = dt.Rows[0]["usercurrency"].ToString();
                    Session["cotsstdcode"] = dt.Rows[0]["stdCode"].ToString();
                    Session["cotsuserfullname"] = dt.Rows[0]["custfullname"].ToString();
                    Session["cotsmail"] = dt.Rows[0]["usermail"].ToString();
                    Session["cotscategory"] = dt.Rows[0]["CustCategory"].ToString();
                    Session["passwordchnaged"] = dt.Rows[0]["passwordchnaged"].ToString();
                    FormsAuthentication.SetAuthCookie(HttpUtility.HtmlEncode(txtUserName.Text), false);

                    if (Convert.ToInt32(Session["passwordchnaged"].ToString()) > 90)
                        Response.RedirectPermanent("changePassword.aspx", false);
                    else
                        Response.RedirectPermanent("default.aspx", false);
                }
                else
                {
                    errmsg.InnerHtml = "";
                    errmsg.InnerHtml = "<span style='color:#ff0000'>Username/Password incorrect</span>";
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
    }
}