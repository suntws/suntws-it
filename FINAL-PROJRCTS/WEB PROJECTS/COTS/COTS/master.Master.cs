using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Reflection;

namespace COTS
{
    public partial class master : System.Web.UI.MasterPage
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                    {
                        StringBuilder strApp = new StringBuilder();
                        strApp.Append("<div id=\"myjquerymenu\" class=\"jquerycssmenu\">");
                        //Start Parent
                        strApp.Append("<ul>");
                        strApp.Append("<li><a href=\"#\">Order</a><ul>");
                        SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@custcode", Session["cotsstdcode"].ToString()) };
                        DataTable dtPrice = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_pricesheet_custwise", sp2, DataAccess.Return_Type.DataTable);
                        if (dtPrice.Rows.Count > 0 && Session["cotsstdcode"].ToString() == "DE0048")
                        {
                            if (Session["cotsstdcode"].ToString() != "DE0048")
                                strApp.Append("<li><a href=\"frmexporderprepare.aspx\">New</a></li>");
                            else
                                strApp.Append("<li><a href=\"frmshipaddress.aspx\">New</a></li>");
                            strApp.Append("<li><a href=\"incompleteorder.aspx\">Incomplete</a></li>");
                        }
                        strApp.Append("<li><a href=\"frmtrackorder.aspx\">Track</a></li>");
                        strApp.Append("</ul></li>");

                        if (Session["cotsstdcode"].ToString() != "DE0048")
                        {
                            if (dtPrice.Rows.Count > 0)
                            {
                                strApp.Append("<li><a href=\"#\">Stock</a><ul>");
                                strApp.Append("<li><a href=\"frmstockdata.aspx\">Tyre</a></li>");

                                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@custcode", Session["cotsstdcode"].ToString()) };
                                DataTable dtRimPrice = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_rimpricesheet_custwise", sp1, DataAccess.Return_Type.DataTable);
                                if (dtRimPrice.Rows.Count > 0)
                                    strApp.Append("<li><a href=\"frmstockrim.aspx\">Rim</a></li>");
                                strApp.Append("</ul></li>");
                            }
                        }

                        strApp.Append("<li><a href=\"#\">Claim</a><ul>");
                        strApp.Append("<li><a href=\"claimregister1.aspx\">Register</a></li>");
                        strApp.Append("<li><a href=\"claimtrack.aspx\">Track</a></li>");
                        strApp.Append("</ul></li>");
                        strApp.Append("<li><a href=\"changepassword.aspx\">Change Password</a></li>");

                        strApp.Append("<li><a href=\"logout.aspx\">Logout</a></li>");
                        strApp.Append("</ul><br style=\"clear: left\" />");
                        //End Parent
                        strApp.Append("</div>");

                        litMenu.Text = strApp.ToString();
                        lblWel.Text = "Welcome " + (Session["cotscode"].ToString() == "2642" ? "SAGE PARTS PLUS, INC" : Session["cotsuserfullname"].ToString());
                    }
                    else
                        Response.Redirect("login.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}