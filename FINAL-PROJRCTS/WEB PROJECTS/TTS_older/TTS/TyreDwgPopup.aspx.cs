using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace TTS
{
    public partial class TyreDwgPopup : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        if (Request["dwgid"] != null && Request["dwgid"].ToString() != "")
                        {
                            if (Request["aid"] != null && Request["aid"].ToString() != "")
                            {
                                lblApproveHeading.Text = "HISTORY";
                                string id = string.Empty;
                                if (Request["dwgid"] != null && Request["Pid"].ToString() != "")
                                {
                                    id = Request["Pid"].ToString();

                                    SqlParameter[] sp = new SqlParameter[1];
                                    sp[0] = new SqlParameter("@PID", Convert.ToInt32(id));
                                    DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Allocation_Pid", sp, DataAccess.Return_Type.DataTable);
                                    if (dt.Rows.Count > 0)
                                    {
                                        lblALLOCATION.Text = "Below Tyresize is Allocation to this drawing";
                                        gvTyreSizeList.DataSource = dt;
                                        gvTyreSizeList.DataBind();
                                    }
                                }
                                else
                                {
                                    lblALLOCATION.Text = "";
                                    id = Request["dwgid"].ToString();
                                }
                                SqlParameter[] sp1 = new SqlParameter[1];
                                sp1[0] = new SqlParameter("@PID", Convert.ToInt32(id));
                                DataTable dtHist = (DataTable)daTTS.ExecuteReader_SP("sp_sel_DwgHistory_ID_Wise", sp1, DataAccess.Return_Type.DataTable);
                                if (dtHist.Rows.Count > 0)
                                {
                                    gvDwgApprovedHistory.DataSource = dtHist;
                                    gvDwgApprovedHistory.DataBind();
                                }
                            }
                            else
                            {
                                SqlParameter[] sp1 = new SqlParameter[1];
                                sp1[0] = new SqlParameter("@PID", Convert.ToInt32(Request["dwgid"].ToString()));
                                hdnStatus.Value = Request["Status"].ToString().ToLower();
                                hdnStatusID.Value = Request["pid"].ToString();
                                string type = string.Empty;
                                if (Request["pid"].ToString() == "cus")
                                    type = "CUSTOMER";
                                else if (Request["pid"].ToString() == "sup")
                                    type = "SUPPLIER";
                                DataTable dtHist = (DataTable)daTTS.ExecuteReader_SP("sp_sel_DwgHistory_ID_Wise", sp1, DataAccess.Return_Type.DataTable);
                                if (dtHist.Rows.Count > 0)
                                {
                                    gvDwgApprovedHistory.DataSource = dtHist;
                                    gvDwgApprovedHistory.DataBind();
                                    if (hdnStatus.Value != "crm")
                                        lblremarks.Text = dtHist.Rows[0]["EdcComments"].ToString();
                                    string strUrl = ConfigurationManager.AppSettings["virdir"] + "TYREDRAWINGCATALOG" + "/" + type + "/" + dtHist.Rows[0]["Category"].ToString() + "/" + dtHist.Rows[0]["DrawingName"].ToString() + "-" + dtHist.Rows[0]["PdfNo"].ToString() + ".pdf";
                                    if (Request.Url.Host.ToLower() == "www.suntws.com")
                                        strUrl = "http://" + Request.Url.Host.ToLower() + "" + strUrl;
                                    else
                                        strUrl = "http://" + Request.Url.Host + ":" + Request.Url.Port + strUrl;
                                    divPdfDwg.InnerHtml = "<iframe style='width:985px; height:500px;' frameborder='0' src='" + strUrl + "'></iframe>";
                                }
                                lblCustomer.Text = "";
                                ddlCustomer.Visible = false;
                                if (hdnStatus.Value == "edc1" || hdnStatus.Value == "edc2" || hdnStatus.Value == "edc3")
                                {
                                    if (Request["dwgstatus"].ToString().ToUpper() == "APPROVE")
                                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow12", "divblock('divApporove');", true);
                                    else if (Request["dwgstatus"].ToString().ToUpper() == "VIEW" && Request["viewtype"].ToString() == "none")
                                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow12", "divnone('divtext');", true);
                                    else if (Request["dwgstatus"].ToString().ToUpper() == "VIEW" && Request["viewtype"].ToString() != "none")
                                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow12", "divblock('divRemarks');", true);
                                    btnApprove.Text = "SAVE EDC STATUS";
                                    lblApproveHeading.Text = "EDC APPROVE";
                                }
                                else if (hdnStatus.Value == "crm")
                                {
                                    ddlCustomer.Visible = true;
                                    lblCustomer.Text = "CUSTOMER";
                                    DataTable dtCust = (DataTable)daCOTS.ExecuteReader("select distinct custfullname,ID from usermaster where userstatus=1 order by custfullname", DataAccess.Return_Type.DataTable);
                                    if (dtCust.Rows.Count > 0)
                                    {
                                        ddlCustomer.DataSource = dtCust;
                                        ddlCustomer.DataTextField = "custfullname";
                                        ddlCustomer.DataValueField = "custfullname";
                                        ddlCustomer.DataBind();
                                    }
                                    ddlCustomer.Items.Insert(0, "Choose");
                                    btnApprove.Text = "SAVE CRM STATUS";
                                    lblApproveHeading.Text = "CRM APPROVE";
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow12", "divblock('divApporove');", true);
                                }
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "divblock('divbutton');", true);
                            }
                        }
                        else if (Request["Reqid"] != null && Request["Reqid"].ToString() != "")
                        {
                            string strUrl = ConfigurationManager.AppSettings["virdir"] + Request["Reqid"].ToString();
                            if (Request.Url.Host.ToLower() == "www.suntws.com")
                                strUrl = "http://" + Request.Url.Host.ToLower() + "" + strUrl;
                            else
                                strUrl = "http://" + Request.Url.Host + ":" + Request.Url.Port + strUrl;

                            divPdfDwg.InnerHtml = "<iframe style='width:985px; height:695px;' frameborder='0' src='" + strUrl + "'></iframe>";
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "divnone('divbutton');", true);
                        }
                        else if (Request["notapproveid"] != null && Request["notapproveid"].ToString() != "")
                        {
                            SqlParameter[] sp1 = new SqlParameter[1];
                            sp1[0] = new SqlParameter("@PID", Convert.ToInt32(Request["notapproveid"].ToString()));
                            DataTable dtHist = (DataTable)daTTS.ExecuteReader_SP("sp_sel_DwgHistory_ID_Wise", sp1, DataAccess.Return_Type.DataTable);
                            if (dtHist.Rows.Count > 0)
                            {
                                string type = dtHist.Rows[0]["DwgCategory"].ToString();
                                gvDwgApprovedHistory.DataSource = dtHist;
                                gvDwgApprovedHistory.DataBind();
                                string strUrl = ConfigurationManager.AppSettings["virdir"] + "TYREDRAWINGCATALOG" + "/" + type + "/" + dtHist.Rows[0]["Category"].ToString() + "/" + dtHist.Rows[0]["DrawingName"].ToString() + "-" + dtHist.Rows[0]["PdfNo"].ToString() + ".pdf";
                                if (Request.Url.Host.ToLower() == "www.suntws.com")
                                    strUrl = "http://" + Request.Url.Host.ToLower() + "" + strUrl;
                                else
                                    strUrl = "http://" + Request.Url.Host + ":" + Request.Url.Port + strUrl;
                                divPdfDwg.InnerHtml = "<iframe style='width:985px; height:500px;' frameborder='0' src='" + strUrl + "'></iframe>";
                            }
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "divnone('divbutton');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                string strCondition = string.Empty;
                if (hdnStatus.Value == "edc1") strCondition = "EDC1";
                else if (hdnStatus.Value == "edc2") strCondition = "EDC2";
                else if (hdnStatus.Value == "edc3") strCondition = "EDC3";
                else if (hdnStatus.Value == "crm") strCondition = "CRM";
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@PID", Convert.ToInt32(Request["dwgid"].ToString()));
                sp1[1] = new SqlParameter("@Remarks", txtApproveRemarks.Text);
                sp1[2] = new SqlParameter("@ApprovedBy", Request.Cookies["TTSUser"].Value);
                sp1[3] = new SqlParameter("@ApprovedStatus", rdbDwgApprovedStatus.SelectedItem.Value);
                sp1[4] = new SqlParameter("@ApproveCondition", strCondition);
                int resp = daTTS.ExecuteNonQuery_SP("sp_update_DwgApproveHistory_ID_Wise", sp1);

                if (rdbDwgApprovedStatus.SelectedItem.Text == "OK")
                {
                    if (hdnStatus.Value == "crm" && ddlCustomer.SelectedItem.Text != "Choose")
                    {
                        SqlParameter[] spCust = new SqlParameter[2];
                        spCust[0] = new SqlParameter("@PID", Convert.ToInt32(Request["dwgid"].ToString()));
                        spCust[0] = new SqlParameter("@CUSTOMERSPECIFIC", ddlCustomer.SelectedItem.Text);
                        daTTS.ExecuteNonQuery_SP("sp_update_DwgCustomer", spCust);
                    }
                }
                else if (rdbDwgApprovedStatus.SelectedItem.Text == "NOT OK")
                {
                    SqlParameter[] sp2 = new SqlParameter[2];
                    sp2[0] = new SqlParameter("@PID", Convert.ToInt32(Request["dwgid"].ToString()));
                    sp2[1] = new SqlParameter("@ApprovedBy", Request.Cookies["TTSUser"].Value);
                    resp = daTTS.ExecuteNonQuery_SP("sp_Del_DwgApproveHistory_ID_Wise", sp2);
                }
                if (resp > 0) ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "closePopup();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnremarks_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@PID", Convert.ToInt32(Request["dwgid"].ToString()));
                sp2[1] = new SqlParameter("@EdcComments", txtApproveRemarks.Text);
                int resp = daTTS.ExecuteNonQuery_SP("sp_Update_EdcTopLevel_DwgApproveHistory_Comments", sp2);
                if (resp > 0)
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "closePopup();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}