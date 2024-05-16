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
using System.Text;

namespace TTS
{
    public partial class prospectdatabuild : System.Web.UI.Page
    {
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                try
                {
                    if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                    {
                        if (!IsPostBack)
                        {
                            if (Request["type"].ToString() == "LeadFetchRec")
                                Response.Write(get_Lead_Details(Request["custcode"].ToString(), Convert.ToInt16(Request["fromval"].ToString()), Convert.ToInt16(Request["toval"].ToString()), Convert.ToInt16(Request["fetchcount"].ToString())));
                            else if (Request["type"].ToString() == "FeedbackFetchRec")
                                Response.Write(get_Admin_Feedback_Details(Request["custcode"].ToString(), Convert.ToInt16(Request["fromval"].ToString()), Convert.ToInt16(Request["toval"].ToString()), Convert.ToInt16(Request["fetchcount"].ToString())));
                            else if (Request["type"].ToString() == "LastFeedback")
                                Response.Write(get_Last_Feedback_Only(Request["custcode"].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteToErrorLog("TTS-PORTDB", "buildleadhistory.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                }
            }
        }

        private string get_Lead_Details(string strCode, int fromVal, int toVal, int fetchCount)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", strCode);
                sp1[1] = new SqlParameter("@From", fromVal);
                sp1[2] = new SqlParameter("@To", toVal);

                DataTable dtLeadList = (DataTable)daPORT.ExecuteReader_SP("Sp_Sel_Between_LeadDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dtLeadList.Rows.Count > 0)
                {
                    int j = 0; string concatStr = string.Empty;
                    int idCount = fetchCount; string strMoreLnk = string.Empty;
                    foreach (DataRow dRow in dtLeadList.Rows)
                    {
                        j++;
                        if (j < 6)
                        {
                            string strURL = "/leadfeedback/" + strCode + "/" +  dRow["AttachFileName"].ToString();
                            string[] strfilename= dRow["AttachFileName"].ToString().Split('.');
                            concatStr += "<tr><td class='histhCss'>" + fromVal++ + "</td>";
                            concatStr += "<td style='vertical-align: top;'><div style='width: 250px;float: left;'>" + dRow["contactperson"].ToString() + "</div><div style='width:250px;float:left;'>" + dRow["email"].ToString() + "</div>";
                            concatStr += "<div style='width:250px;float:left;'>" + dRow["mobileno"].ToString() + "</div><div style='width:250px;float:left;'>" + dRow["phoneno"].ToString() + "</div><div style='width:250px;float:left;'>" + dRow["webaddress"].ToString() + "</div>";
                            concatStr += "<div style='width:250px;float:left;'>Led by : <span style='font-weight:bold;'>" + dRow["username"].ToString() + "</span><div style='float:left;padding-left:45px;width:185px;'>" + dRow["createddate"].ToString() + "</div></td>";
                            if (dRow["AttachFileName"].ToString() != "")
                                concatStr += "<td style='vertical-align: top;'><div style='word-break: break-word;width:610px;'>" + dRow["leadsfeedback"].ToString() + "</div></br><span onclick='attachDownload(\"" + strURL + "\",\"" + strfilename[0] + "\");' class='attachSpan'>" + dRow["AttachFileName"].ToString() + "</span></td></tr>";
                            else
                                concatStr += "<td style='vertical-align: top;'><div style='word-break: break-word;width:610px;'>" + dRow["leadsfeedback"].ToString() + "</div></td></tr>";
                        }
                        else if (j == 6)
                        {
                            idCount++;
                            strMoreLnk = "<span id=\"spanLead" + fetchCount + "\" class='morelinkcss'><span onclick='getCustWiseLeadDetails(\"" + toVal + "\",\"" + (toVal + 5) + "\",\"" + idCount + "\",\"spanLead" + fetchCount + "\")' class='morelnkfeedbackCss'>more...</span></span>";
                        }
                    }
                    if (concatStr.Length > 0)
                    {
                        string strHead = string.Empty;
                        if (fetchCount == 1)
                        {
                            strHead += "<tr><th class='hislisrhead' style='width:30px;'>S.No.</th><th class='hislisrhead' style='width:250px;'>Contacted Details</th><th class='hislisrhead' style='width:610px;'>Feedback</th></tr>";
                            strApp.Append("<span class='headCss' style='text-decoration: underline;float:left;width:890px;'>Previous Lead History :</span>");
                            strApp.Append("<table cellspacing='0' rules='all' border='1' style='float:left;width:902px;border-collapse:collapse;'>" + strHead + concatStr + "</table>" + strMoreLnk);
                        }
                        else strApp.Append("<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>" + concatStr + "</table>" + strMoreLnk);
                    }
                }
                else
                    strApp.Append("<span class='norecords'>No Records.</span>");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", "buildleadhistory.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }

        private string get_Admin_Feedback_Details(string strCode, int fromVal, int toVal, int fetchCount)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", strCode);
                sp1[1] = new SqlParameter("@From", fromVal);
                sp1[2] = new SqlParameter("@To", toVal);

                DataTable dtfeedbackList = (DataTable)daPORT.ExecuteReader_SP("Sp_Sel_Between_UpdateMsgToLead", sp1, DataAccess.Return_Type.DataTable);
                if (dtfeedbackList.Rows.Count > 0)
                {
                    int j = 0; string concatStr = string.Empty;
                    int idCount = fetchCount; string strMoreLnk = string.Empty;
                    foreach (DataRow dRow in dtfeedbackList.Rows)
                    {
                        j++;
                        if (j < 6)
                        {
                            concatStr += "<tr><td class='histhCss' style='width:30px;'>" + fromVal++ + "</td>";
                            concatStr += "<td style='vertical-align: top;'><div>Updated by : <span style='font-weight:bold;'>" + dRow["username"].ToString() + "</span>";
                            concatStr += "<div style='float:left;padding-left:65px;width:185px;'>" + dRow["createddate"].ToString() + "</div></td>";
                            concatStr += "<td style='vertical-align: top;'><div style='word-break: break-word;width:610px;'>" + dRow["UpdateMsg"].ToString() + "</div></td></tr>";
                        }
                        else if (j == 6)
                        {
                            idCount++;
                            strMoreLnk = "<span id=\"spanFeed" + fetchCount + "\" class='morelinkcss'><span onclick='getpreviousfeedback(\"" + toVal + "\",\"" + (toVal + 5) + "\",\"" + idCount + "\",\"spanFeed" + fetchCount + "\")' class='morelnkfeedbackCss'>more...</span></span>";
                        }
                    }
                    if (concatStr.Length > 0)
                    {
                        string strHead = string.Empty;
                        if (fetchCount == 1)
                        {
                            strHead += "<tr><th class='feedbcakhead' style='width:30px;'>S.No.</th><th class='feedbcakhead' style='width: 170px'>Admin Details</th><th class='feedbcakhead' style='width:450px;'>Admin Feedback</th></tr>";
                            strApp.Append("<span class='headCss' style='text-decoration: underline;float:left;width:690px;'>Admin Feedback History :</span>");
                            strApp.Append("<table cellspacing='0' rules='all' border='1' style='float:left;width:902px;border-collapse:collapse;'>" + strHead + concatStr + "</table>" + strMoreLnk);
                        }
                        else strApp.Append("<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>" + concatStr + "</table>" + strMoreLnk);
                    }
                }
                else strApp.Append("<span class='norecords'>No Records.</span>");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", "buildleadhistory.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }

        private string get_Last_Feedback_Only(string strcustcode)
        {
            StringBuilder appendVal = new StringBuilder();
            try
            {
                DataTable dtLastFeedback = (DataTable)daPORT.ExecuteReader("select top 1 UpdateMsg,CONVERT(VARCHAR(19),createddate) as createddate,Username from UpdateMsgToLead where custcode='" + strcustcode + "' order by ID desc", DataAccess.Return_Type.DataTable);
                if (dtLastFeedback.Rows.Count > 0)
                {
                    appendVal.Append("<div class='headCss'>Last feedback from admin:</div>");
                    appendVal.Append("<div id='divUpdateSingleList' style='line-height:20px;'><div>" + dtLastFeedback.Rows[0]["UpdateMsg"].ToString() + "</div>");
                    appendVal.Append("<div>Updated by : " + dtLastFeedback.Rows[0]["Username"].ToString() + "</div>");
                    appendVal.Append("<div>Date       :" + dtLastFeedback.Rows[0]["createddate"].ToString() + "</div></div>");
                    appendVal.Append("<span style='float: left; width: 120px; margin-left: 0px; text-decoration: underline;'");
                    string makefun = "getpreviousfeedback('1', '6', '1', 'popup_feedback_box');";
                    appendVal.Append(" onclick=\"" + makefun + "\" class='morefeedbackCss'>Previous Feedback</span>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", "buildleadhistory.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return appendVal.ToString();
        }
    }
}