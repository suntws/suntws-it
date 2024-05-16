using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace COTS
{
    public partial class frmmsgdisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                    {
                        if (Request["msgtype"] != null && Request["msgtype"].ToString() != "")
                        {
                            if (Request["msgtype"].ToString() == "sheetmsg")
                                divpricesheetmsg.Style.Add("display", "block");
                            else if (Request["msgtype"].ToString() == "ordercomplete")
                            {
                                lblOrderNo.Text = Request["oid"].ToString();
                                divordercompletemsg.Style.Add("display", "block");
                            }
                            else if (Request["msgtype"].ToString() == "confirmedmsg")
                                divConfirmedMsg.Style.Add("display", "block");
                            else if (Request["msgtype"].ToString() == "claimReg")
                            {
                                lblComplaintNo.Text = "Your Complaint No. - " + Request["claimid"].ToString();
                                divClaimRegister.Style.Add("display", "block");
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("SessionExp.aspx", false);
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                }
            }
        }
    }
}