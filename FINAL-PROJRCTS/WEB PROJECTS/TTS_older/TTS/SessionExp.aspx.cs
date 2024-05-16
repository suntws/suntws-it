using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace TTS
{
    public partial class SessionExp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkLoginAgain_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx?ReturnUrl=default.aspx", false);
        }
    }
}