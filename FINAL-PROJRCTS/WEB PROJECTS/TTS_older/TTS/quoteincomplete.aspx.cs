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
    public partial class quoteincomplete : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_quote"].ToString() == "True")
                        {
                            if (Request["qid"] != null && Request["qid"].ToString() != "")
                            {
                                DataTable dt = new DataTable();
                                if (Request["qid"].ToString() == "1")
                                {
                                    lblQuoteHead.Text = "INCOMPLETE QUOTATION";
                                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Incomplete_Quote", DataAccess.Return_Type.DataTable);
                                    gvIncompleteQuoteItem.Columns[10].Visible = false;
                                }
                                else if (Request["qid"].ToString() == "2")
                                {
                                    lblQuoteHead.Text = "QUOTATION: WAITING FOR CUSTOMER CONFIRMATION";
                                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_SendQuote_Revise", DataAccess.Return_Type.DataTable);
                                    gvIncompleteQuoteItem.Columns[10].Visible = true;
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    ViewState["dtGv"] = dt;
                                    Bind_DDL_GV(dt, "ALL");
                                }
                                else
                                    lblErrMsg.Text = "No records";
                            }
                            else
                                lblErrMsg.Text = "URL is wrong. Please try again";
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
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

        private void Bind_DDL_GV(DataTable dtGv, string strCont)
        {
            try
            {
                lblErrMsg.Text = "";
                DataView dtView = new DataView(dtGv);
                switch (strCont)
                {
                    case "Type":
                        dtView.Sort = "QCustomer";
                        DataTable distinctName = dtView.ToTable(true, "QCustomer");
                        Utilities.ddl_Binding(ddlQCustName, distinctName, "QCustomer", "QCustomer", "");
                        ddlQCustName.Items.Insert(0, "ALL");

                        dtView.Sort = "UserName";
                        DataTable distinctUser = dtView.ToTable(true, "UserName");
                        Utilities.ddl_Binding(ddlQUser, distinctUser, "UserName", "UserName", "");
                        ddlQUser.Items.Insert(0, "ALL");

                        dtView.Sort = "QYear DESC";
                        DataTable distinctYear = dtView.ToTable(true, "QYear");
                        Utilities.ddl_Binding(ddlQYear, distinctYear, "QYear", "QYear", "");

                        dtView = new DataView(dtGv, "QYear='" + ddlQYear.SelectedItem.Text + "'", "QMonthID", DataViewRowState.CurrentRows);
                        dtView.Sort = "QMonthID DESC";
                        DataTable distinctMonth = dtView.ToTable(true, "QMonth", "QMonthID");
                        Utilities.ddl_Binding(ddlQMonth, distinctMonth, "QMonth", "QMonthID", "");
                        break;

                    case "Customer":
                        dtView.Sort = "UserName";
                        distinctUser = dtView.ToTable(true, "UserName");
                        Utilities.ddl_Binding(ddlQUser, distinctUser, "UserName", "UserName", "");
                        ddlQUser.Items.Insert(0, "ALL");

                        dtView.Sort = "QYear DESC";
                        distinctYear = dtView.ToTable(true, "QYear");
                        Utilities.ddl_Binding(ddlQYear, distinctYear, "QYear", "QYear", "");

                        dtView = new DataView(dtGv, "QYear='" + ddlQYear.SelectedItem.Text + "'", "QMonthID", DataViewRowState.CurrentRows);
                        dtView.Sort = "QMonthID DESC";
                        distinctMonth = dtView.ToTable(true, "QMonth", "QMonthID");
                        Utilities.ddl_Binding(ddlQMonth, distinctMonth, "QMonth", "QMonthID", "");
                        break;

                    case "User":
                        dtView.Sort = "QYear DESC";
                        distinctYear = dtView.ToTable(true, "QYear");
                        Utilities.ddl_Binding(ddlQYear, distinctYear, "QYear", "QYear", "");

                        dtView = new DataView(dtGv, "QYear='" + ddlQYear.SelectedItem.Text + "'", "QMonthID", DataViewRowState.CurrentRows);
                        dtView.Sort = "QMonthID DESC";
                        distinctMonth = dtView.ToTable(true, "QMonth", "QMonthID");
                        Utilities.ddl_Binding(ddlQMonth, distinctMonth, "QMonth", "QMonthID", "");
                        break;

                    case "Year":
                        dtView = new DataView(dtGv, "QYear='" + ddlQYear.SelectedItem.Text + "'", "QMonthID", DataViewRowState.CurrentRows);
                        dtView.Sort = "QMonthID DESC";
                        distinctMonth = dtView.ToTable(true, "QMonth", "QMonthID");
                        Utilities.ddl_Binding(ddlQMonth, distinctMonth, "QMonth", "QMonthID", "");
                        break;

                    case "Month":
                        break;
                    default:
                        dtView.Sort = "QCustType";
                        DataTable distinctType = dtView.ToTable(true, "QCustType");
                        Utilities.ddl_Binding(ddlQCustType, distinctType, "QCustType", "QCustType", "");
                        ddlQCustType.Items.Insert(0, "ALL");

                        dtView.Sort = "QCustomer";
                        distinctName = dtView.ToTable(true, "QCustomer");
                        Utilities.ddl_Binding(ddlQCustName, distinctName, "QCustomer", "QCustomer", "");
                        ddlQCustName.Items.Insert(0, "ALL");

                        dtView.Sort = "UserName";
                        distinctUser = dtView.ToTable(true, "UserName");
                        Utilities.ddl_Binding(ddlQUser, distinctUser, "UserName", "UserName", "");
                        ddlQUser.Items.Insert(0, "ALL");

                        dtView.Sort = "QYear DESC";
                        distinctYear = dtView.ToTable(true, "QYear");
                        Utilities.ddl_Binding(ddlQYear, distinctYear, "QYear", "QYear", "");

                        dtView = new DataView(dtGv, "QYear='" + ddlQYear.SelectedItem.Text + "'", "QMonthID", DataViewRowState.CurrentRows);
                        dtView.Sort = "QMonthID DESC";
                        distinctMonth = dtView.ToTable(true, "QMonth", "QMonthID");
                        Utilities.ddl_Binding(ddlQMonth, distinctMonth, "QMonth", "QMonthID", "");
                        break;
                }
                dtView = new DataView(dtGv, "QYear='" + ddlQYear.SelectedItem.Text + "' and QMonth='" + ddlQMonth.SelectedItem.Text + "'", "QRefNo DESC", DataViewRowState.CurrentRows);
                if (dtView.Count > 0)
                {
                    gvIncompleteQuoteItem.DataSource = dtView;
                    gvIncompleteQuoteItem.DataBind();
                }
                else
                {
                    gvIncompleteQuoteItem.DataSource = null;
                    gvIncompleteQuoteItem.DataBind();
                    lblErrMsg.Text = "NO RECORDS";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkPrepareQuotePage_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((LinkButton)sender).NamingContainer as GridViewRow;
                Label lblQAcYear = (Label)row.FindControl("lblQAcYear");
                Label lblQRefNo = (Label)row.FindControl("lblQRefNo");

                Response.Redirect("quotegradewiseprepare.aspx?qac=" + lblQAcYear.Text + "&qref=" + lblQRefNo.Text, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkQuotePdf_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((LinkButton)sender).NamingContainer as GridViewRow;
                Label lblQAcYear = (Label)row.FindControl("lblQAcYear");
                Label lblQRefNo = (Label)row.FindControl("lblQRefNo");
                Label lblRevisedCount = (Label)row.FindControl("lblRevisedCount");

                string serverUrl = Server.MapPath("~/quote/").Replace("TTS", "pdfs");
                string path = serverUrl + "/" + lblQAcYear.Text + "_" + lblQRefNo.Text + "_" + lblRevisedCount.Text + ".pdf";
                string lnkTxt = lblQAcYear.Text + "_" + lblQRefNo.Text + "_" + lblRevisedCount.Text + ".pdf";

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlQCustType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("Type");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlQCustName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("Customer");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlQUser_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("User");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlQYear_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("Year");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlQMonth_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("Month");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Make_Where_Condition(string strField)
        {
            try
            {
                DataTable dt = ViewState["dtGv"] as DataTable;
                string strCond = " QYear='" + ddlQYear.SelectedItem.Text + "' ";//+ "' and QMonth='" + ddlQMonth.SelectedItem.Text 
                if (ddlQCustType.SelectedItem.Text != "ALL")
                    strCond += " and QCustType='" + ddlQCustType.SelectedItem.Text + "'";
                if (ddlQCustName.SelectedItem.Text != "ALL")
                    strCond += " and QCustomer='" + ddlQCustName.SelectedItem.Text + "'";
                if (ddlQUser.SelectedItem.Text != "ALL")
                    strCond += " and UserName='" + ddlQUser.SelectedItem.Text + "'";
                DataView dtView = new DataView(dt, strCond, "QRefNo DESC", DataViewRowState.CurrentRows);
                Bind_DDL_GV(dtView.ToTable(), strField);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}