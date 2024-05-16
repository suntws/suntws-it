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

namespace COTS
{
    public partial class changePassword : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["cotsuser"] == null || Session["cotsuser"].ToString() == "")
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
                else
                {
                    lblPassChangedOn.Text = "PASSWORD CHANGED " + Session["passwordchnaged"].ToString() + " DAYS BEFORE";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@useroldpassword", txtOldPassword.Text);
                sp1[1] = new SqlParameter("@usernewpassword", txtConfirmPassword.Text);
                sp1[2] = new SqlParameter("@username", Session["cotsuser"].ToString());
                int resp = daCOTS.ExecuteNonQuery_SP("sp_edit_password", sp1);

                if (resp > 0)
                    Response.RedirectPermanent("logout.aspx", false);
                else
                    lblErrMsg.Text = "Please enter correct details";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}