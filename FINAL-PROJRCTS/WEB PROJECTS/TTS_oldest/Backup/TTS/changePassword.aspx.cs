using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TTS
{
    public partial class changePassword : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] == null || Request.Cookies["TTSUser"].Value == "")
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@oldpass", txtOldPassword.Text);
                sp1[1] = new SqlParameter("@confirmpass", txtConfirmPassword.Text);
                sp1[2] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                int resp = daTTS.ExecuteNonQuery_SP("sp_ChangePassword", sp1);

                if (resp > 0)
                    Response.RedirectPermanent("logout.aspx", false);
                else
                    lblErrMsg.Text = "Please enter correct details";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}