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
using System.IO;
using System.Globalization;

namespace TTS
{
    public partial class RequirementTicketResponse : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0)
                        {
                            Bind_gvTicketDetails();
                            ScriptManager.RegisterStartupScript(Page, GetType(), "jopenMaintable", "openMainTable();", true);
                            //if (dtUser.Rows[0][""].ToString() == "True")
                            //{

                            //}
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            //    lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                            //}
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

        protected void lnkTciketresponse_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow ClickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Bind_gv_SingleView(ClickedRow);
                if (rdbResponseType.SelectedValue == "STATUS")
                {
                    if (ClickedRow.Cells[3].Text.ToString() == "FILE")
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JopenFileupload1", "document.getElementById('div_lblfileupload').style.display='block';", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JopenFileupload2", "document.getElementById('div_upfileupload').style.display='block';", true);
                    }
                    btn_ResonseSend.Text = "STATUS CHANGE";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JopenFileupload3", "openSubTableForStatusChange();", true);
                }
                else
                {
                    btn_ResonseSend.Text = "RESPONSE SENT";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JopenSubTable", "openSubTable();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        private void Bind_gv_SingleView(GridViewRow Row)
        {
            try
            {
                DataTable dt = new DataTable();
                txtrequestedcommands.Text = Row.Cells[7].Text;
                lblTicketNumber.Text = Row.Cells[0].Text;
                lblPriority.Text = Row.Cells[4].Text;
                txtpossibledays.Text = Row.Cells[10].Text.Length != 0 && Row.Cells[10].Text != "&nbsp;" ? Row.Cells[10].Text.ToString() : "0";
                txtstartsfrom.Text = Row.Cells[11].Text.Length != 0 && Row.Cells[11].Text != "&nbsp;" ? Row.Cells[11].Text.ToString() : DateTime.Today.Date.ToShortDateString();
                txtendto.Text = Row.Cells[12].Text.Length != 0 && Row.Cells[12].Text != "&nbsp;" ? Row.Cells[12].Text.ToString() : DateTime.Today.Date.ToShortDateString();
                SqlParameter[] param1 = new SqlParameter[1];
                param1[0] = new SqlParameter("@TicketNo", lblTicketNumber.Text);
                dt = (DataTable)daTTS.ExecuteReader_SP("sp_Sel_TicketDetails_ScotsNewRequestAnalysis", param1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    txtresponsedcommands.Text = dt.Rows[0]["Responsed_Comments"].ToString();
                    gv_SingleView.DataSource = dt;
                    gv_SingleView.DataBind();

                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btn_ResonseSend_Click(object sender, EventArgs e)
        {
            try
            {

                int getresp = Save_ResponsedDetails();
                if (getresp > 0)
                {
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        private int Save_ResponsedDetails()
        {
            int resp = 0;
            try
            {
                if (rdbResponseType.SelectedValue.ToString() == "STATUS")
                {
                    if (uprequestedfile.FileName.Length > 0)
                    {
                        string serverpath = Server.MapPath("~");
                        if (!Directory.Exists(serverpath + "/User Needs/"))
                            Directory.CreateDirectory(serverpath + "/User Needs/");
                        uprequestedfile.SaveAs(serverpath + "/User Needs/" + uprequestedfile.FileName);
                    }
                    SqlParameter[] param1 = new SqlParameter[3];
                    param1[0] = new SqlParameter("@Ticket_No", lblTicketNumber.Text);
                    param1[1] = new SqlParameter("@Ticket_Status", "UPDATED");
                    param1[2] = new SqlParameter("@Requested_Data", uprequestedfile.FileName.Length > 0 ? uprequestedfile.FileName : string.Empty);
                    resp = daTTS.ExecuteNonQuery_SP("sp_upt_FinalUpt_ScotsRequestAnalysis", param1);
                }
                else
                {
                    SqlParameter[] param1 = new SqlParameter[6];
                    param1[0] = new SqlParameter("@Ticket_No", lblTicketNumber.Text);
                    param1[1] = new SqlParameter("@Responsed_Comments", txtresponsedcommands.Text);
                    param1[2] = new SqlParameter("@Possible_Days", txtpossibledays.Text);
                    param1[3] = new SqlParameter("@Starts_From", DateTime.ParseExact(txtstartsfrom.Text, "dd/MM/YYYY", CultureInfo.InvariantCulture));
                    param1[4] = new SqlParameter("@End_To", DateTime.ParseExact(txtendto.Text, "dd/MM/YYYY", CultureInfo.InvariantCulture));
                    param1[5] = new SqlParameter("@Ticket_Status", "QUEUED");
                    resp = daTTS.ExecuteNonQuery_SP("sp_upt_FinalQue_ScotsRequestAnalysis", param1);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return resp;
        }
        protected void lnkrequestedData_ViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton SelectedLink = sender as LinkButton;
                string Filename = SelectedLink.Text;
                string Filepath = Server.MapPath("~/User Needs/" + Filename);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("content-disposition", "Download; Filename=" + Filename);
                Response.Write(Filepath);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdbResponseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Bind_gvTicketDetails();
                ScriptManager.RegisterStartupScript(Page, GetType(), "jopenMaintable", "openMainTable();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_gvTicketDetails()
        {
            try
            {
                lblErrMsgcontent.Text = "";
                DataTable dt = new DataTable();
                SqlParameter[] param1 = new SqlParameter[1];
                param1[0] = new SqlParameter("@Updation_Type", rdbResponseType.SelectedValue);
                dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ScotsRequestAnalysis", param1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    if (rdbResponseType.SelectedValue == "VIEW")
                    {
                        gvTicketDetails.DataSource = dt;
                        gvTicketDetails.DataBind();
                        gvTicketDetails.Columns[9].Visible = true;
                        gvTicketDetails.Columns[14].Visible = false;
                    }
                    else
                    {
                        gvTicketDetails.DataSource = dt;
                        gvTicketDetails.DataBind();
                        gvTicketDetails.Columns[9].Visible = false;
                        gvTicketDetails.Columns[14].Visible = true;
                    }
                }
                else
                {
                    lblErrMsgcontent.Text = "No Records to Show";
                    gvTicketDetails.DataSource = null;
                    gvTicketDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

    }
}