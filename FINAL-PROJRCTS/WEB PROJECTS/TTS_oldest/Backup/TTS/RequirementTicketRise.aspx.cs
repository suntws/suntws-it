using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace TTS
{
    public partial class RequirementTicketRise : System.Web.UI.Page
    {
        XmlDocument xmlLevelList = new XmlDocument();
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtList = new DataTable();
                        dtList.Columns.Add("text", typeof(string));
                        dtList.Columns.Add("value", typeof(string));

                        dtList.Rows.Add("Master", "masterlevel");
                        dtList.Rows.Add("Dashboard", "dashboardlevel");
                        dtList.Rows.Add("Prospect", "prospectlevel");
                        dtList.Rows.Add("S-COTS Domestic", "scotsdomesticlevel");
                        dtList.Rows.Add("S-COTS International", "scotsexportlevel");
                        dtList.Rows.Add("Claim Module", "claimlevel");
                        dtList.Rows.Add("Order Tracking", "ordertrackinglevel");
                        dtList.Rows.Add("E-BID", "ebidlevel");
                        if (dtList.Rows.Count > 0)
                        {
                            ddl_module_exis.DataSource = dtList;
                            ddl_module_exis.DataTextField = "text";
                            ddl_module_exis.DataValueField = "value";
                            ddl_module_exis.DataBind();
                        }
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

        protected void ddl_module_exis_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblmoduleselect.Text = "Please Select Your Module";
                Bind_RadioList(ddl_module_exis.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }

        protected void Bind_RadioList(string ddlvalue)
        {
            try
            {
                string strXmlUserLevelList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"UserLevelMenu.xml");
                xmlLevelList.Load(strXmlUserLevelList);
                DataTable dtradio = new DataTable();
                dtradio.Columns.Add("text", typeof(string));
                dtradio.Columns.Add("key", typeof(string));
                foreach (XmlNode xNode in xmlLevelList.SelectNodes("/userlevel/level[@menuvisible='" + ddlvalue + "']"))
                {
                    dtradio.Rows.Add(xNode.Attributes["text"].Value, xNode.Attributes["key"].Value);
                }
                if (dtradio.Rows.Count > 0)
                {
                    rdosubmodules.DataSource = dtradio;
                    rdosubmodules.DataTextField = "text";
                    rdosubmodules.DataValueField = "key";
                    rdosubmodules.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jcurrentpage4", "requestview();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnticketrise_Click(object sender, EventArgs e)
        {
            try
            {
                int getresp = Save_TicketDetails();
                if (getresp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Jcurrentpage2", "alert('Request Send Successfully.');", true);
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private int Save_TicketDetails()
        {
            int resp = 0;
            try
            {
                string ticket_no = Ticke_No_Generation();
                if (rdbrequestType.SelectedValue.ToString() == "NEW" || rdbrequestType.SelectedValue.ToString() == "FILE")
                {
                    SqlParameter[] param1 = new SqlParameter[8];
                    param1[0] = new SqlParameter("@Updation_Type", rdbrequestType.SelectedValue.ToString());
                    param1[1] = new SqlParameter("@Priority", ddl_priority.Text);
                    param1[2] = new SqlParameter("@Subject", txtticketsubject.Text);
                    param1[3] = new SqlParameter("@Expected_Date", DateTime.ParseExact(txtExpectedDate.Text, "dd/MM/YYYY", CultureInfo.InvariantCulture));
                    param1[4] = new SqlParameter("@Requested_Comments", txtnewmodulecommands.Text);
                    param1[5] = new SqlParameter("@Ticket_Raised_User", Request.Cookies["TTSUser"].Value);
                    param1[6] = new SqlParameter("@Ticket_Status", "SENT");
                    param1[7] = new SqlParameter("@stdcode", ticket_no);
                    resp = daTTS.ExecuteNonQuery_SP("sp_ins__ScotsNewRequestAnalysis", param1);
                }
                else if (rdbrequestType.SelectedValue.ToString() == "MODIFY")
                {
                    SqlParameter[] param1 = new SqlParameter[10];
                    param1[0] = new SqlParameter("@Updation_Type", rdbrequestType.SelectedValue.ToString());
                    param1[1] = new SqlParameter("@Priority", ddl_priority.Text);
                    param1[2] = new SqlParameter("@Subject", txtticketsubject.Text);
                    param1[3] = new SqlParameter("@Module_Name", ddl_module_exis.Text);
                    param1[4] = new SqlParameter("@Function_Name", rdosubmodules.SelectedValue);
                    param1[5] = new SqlParameter("@Expected_Date", DateTime.ParseExact(txtExpectedDate.Text, "dd/MM/YYYY", CultureInfo.InvariantCulture));
                    param1[6] = new SqlParameter("@Requested_Comments", txtnewmodulecommands.Text);
                    param1[7] = new SqlParameter("@Ticket_Raised_User", Request.Cookies["TTSUser"].Value);
                    param1[8] = new SqlParameter("@Ticket_Status", "SENT");
                    param1[9] = new SqlParameter("@stdcode", ticket_no);
                    resp = daTTS.ExecuteNonQuery_SP("sp_ins_ScotsExistRequestAnalysis", param1);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return resp;
        }
        protected void rdbrequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jcurrentpage3", "requestview();", true);
                gv_bind_status();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void gv_bind_status()
        {
            try
            {
                lblErrorRequest.Text = "";
                DataTable dt = new DataTable();
                SqlParameter[] param1 = new SqlParameter[1];
                param1[0] = new SqlParameter("@Updation_Type", rdbrequestType.SelectedValue);
                switch (rdbrequestType.SelectedValue)
                {
                    case ("NEW"):
                        {
                            dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_updType_req_ScotsRequestAnalysis", param1, DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                gv_TicketDetails.DataSource = dt;
                                gv_TicketDetails.DataBind();
                                gv_TicketDetails.Columns[5].Visible = false;
                                gv_TicketDetails.Columns[6].Visible = false;
                                gv_TicketDetails.Columns[9].Visible = false;
                            }
                            break;
                        }
                    case ("MODIFY"):
                        {

                            param1[0] = new SqlParameter("@Updation_Type", rdbrequestType.SelectedValue);
                            dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_updType_req_ScotsRequestAnalysis", param1, DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                gv_TicketDetails.DataSource = dt;
                                gv_TicketDetails.DataBind();
                                gv_TicketDetails.Columns[5].Visible = true;
                                gv_TicketDetails.Columns[6].Visible = true;
                                gv_TicketDetails.Columns[9].Visible = false;
                            }

                            break;
                        }
                    case ("FILE"):
                        {
                            param1[0] = new SqlParameter("@Updation_Type", rdbrequestType.SelectedValue);
                            dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_updType_req_ScotsRequestAnalysis", param1, DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                gv_TicketDetails.DataSource = dt;
                                gv_TicketDetails.DataBind();
                                gv_TicketDetails.Columns[5].Visible = false;
                                gv_TicketDetails.Columns[6].Visible = false;
                                gv_TicketDetails.Columns[9].Visible = true;
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkrequestedData_file_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton ClickedLink = sender as LinkButton;
                string Filename = ClickedLink.Text;
                string Filepath = Server.MapPath("~/User Needs/" + Filename);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", "Download; Filename=" + Filename);
                Response.Write(Filepath);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jsingleview1", "opensingleview();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        private string Ticke_No_Generation()
        {
            string ticketno = null;
            try
            {
                switch (rdbrequestType.SelectedValue)
                {
                    case ("NEW"):
                        {
                            ticketno = "R-N";
                            break;
                        }
                    case ("MODIFY"):
                        {
                            ticketno = "R-E";
                            break;
                        }
                    case ("FILE"):
                        {
                            ticketno = "R-F";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ticketno;
        }
    }
}