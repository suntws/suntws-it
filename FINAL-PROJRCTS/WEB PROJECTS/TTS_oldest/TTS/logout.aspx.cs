using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TTS
{
    public partial class logout : System.Web.UI.Page
    {
        DataAccess daUser = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value) };
                daUser.ExecuteNonQuery_SP("sp_update_loginhistory", sp1);

                FormsAuthentication.SignOut();
                Response.Cookies["TTSUser"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["TTSUserEmail"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["TTSUserType"].Expires = DateTime.Now.AddDays(-1);
                Response.Redirect("default.aspx", false);
            }
            catch (System.Threading.ThreadAbortException) { }
        }
    }
}