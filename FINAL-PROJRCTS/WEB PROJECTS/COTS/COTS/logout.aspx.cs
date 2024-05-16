using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace COTS
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                FormsAuthentication.SignOut();
                Session["cotscode"] = "";
                Session["cotsstdcode"] = "";
                Session["cotsuser"] = "";
                Session["cotscur"] = "";
                Session["cotsuserfullname"] = "";
                Session["cotscategory"] = "";
                Session["trackorder"] = "";
                Session["hdnOID"] = "";
                Session["passwordchnaged"] = "";
                Response.Redirect("default.aspx", false);
            }
            catch (System.Threading.ThreadAbortException) { }
        }
    }
}