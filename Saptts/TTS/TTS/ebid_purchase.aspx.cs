using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class ebid_purchase : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (Request["vid"] != null && Request["vid"].ToString() != "")
                    {
                        if (Request["vid"].ToString() == "1" || Request["vid"].ToString() == "2")
                            plhEbidPurchase.Controls.Add(LoadControl("ebidpurchase/masterentry.ascx"));
                        else if (Request["vid"].ToString() == "3" || Request["vid"].ToString() == "4")
                            plhEbidPurchase.Controls.Add(LoadControl("ebidpurchase/assignentry.ascx")); 
                        else if (Request["vid"].ToString() == "5")
                            plhEbidPurchase.Controls.Add(LoadControl("ebidpurchase/enquiryprepare.ascx"));
                        else if (Request["vid"].ToString() == "6")
                            plhEbidPurchase.Controls.Add(LoadControl("ebidpurchase/enquirypreparedlist.ascx"));
                        else if (Request["vid"].ToString() == "7")
                            plhEbidPurchase.Controls.Add(LoadControl("ebidpurchase/enquiryquotedlist.ascx"));
                        else if (Request["vid"].ToString() == "8")
                            plhEbidPurchase.Controls.Add(LoadControl("ebidpurchase/preparepurchaseorder.ascx"));
                        else if (Request["vid"].ToString() == "9")
                            plhEbidPurchase.Controls.Add(LoadControl("ebidpurchase/enquirycancel.ascx"));
                        else if (Request["vid"].ToString() == "10")
                            plhEbidPurchase.Controls.Add(LoadControl("ebidpurchase/ebidordertracking.ascx"));
                        else if (Request["vid"].ToString() == "11")
                            plhEbidPurchase.Controls.Add(LoadControl("ebidpurchase/ebiddispatchedorder.ascx"));
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                        lblErrMsgcontent.Text = "URL IS WRONG.";
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
    }
}