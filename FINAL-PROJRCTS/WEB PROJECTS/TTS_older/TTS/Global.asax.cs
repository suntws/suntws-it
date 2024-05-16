using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TTS
{
    public class Global : System.Web.HttpApplication
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        void Application_Start(object sender, EventArgs e)
        {

            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            
            //if (Request.Cookies["TTSUser"] != null)
            //{
            //    Session.Add("UserName", Request.Cookies["TTSUser"].Value);
            //    if (Session["UserName"].ToString() != "")
            //    {
            //        SqlParameter[] param = new SqlParameter[1];
            //        param[0] = new SqlParameter("@UserName", Session["UserName"].ToString());
            //        daTTS.ExecuteNonQuery_SP("sp_update_login_SessionStart", param);
            //    }
            //}
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@UserName", Session["UserName"].ToString());
                daTTS.ExecuteNonQuery_SP("sp_update_login_SessionClose", param);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS_Global.asax", Request.Url.ToString(), "Session_End", 1, ex.Message);
            }
        }
    }
}
