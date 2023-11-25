using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.IO;
using System.Globalization;

namespace TTS
{
    public partial class StockReturnAF : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dom_paymentconfirm"].ToString() == "True" ||
                            dtUser.Rows[0]["dom_invoice"].ToString() == "True" || dtUser.Rows[0]["exp_documents"].ToString() == "True"))
                        {
                            lblPageHead.Text = "CREDIT NOTE FOR DISPATCH RETURN STOCK - " + Utilities.Decrypt(Request["pid"].ToString()).ToUpper();
                            SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                                new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString()).ToUpper()) 
                            };
                            DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_StockReturn_ForAF", sp, DataAccess.Return_Type.DataTable);
                            if (dtList.Rows.Count > 0)
                            {
                                gv_Returnitems.DataSource = dtList;
                                gv_Returnitems.DataBind();
                            }
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
        protected void btnCredit_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow ClickedRow = ((Button)sender).NamingContainer as GridViewRow;
                lblCustName.Text = ClickedRow.Cells[0].Text;
                lblStausOrderRefNo.Text = ClickedRow.Cells[1].Text;

                lblCustName1.Text = ClickedRow.Cells[0].Text;
                lblStausOrderRefNo1.Text = ClickedRow.Cells[1].Text;
                txtCreditnote.Text = ((Label)ClickedRow.FindControl("lblCreditNote")).Text;
                hdn_custcode.Value = ((HiddenField)ClickedRow.FindControl("hdncustcode")).Value;
                hdnReturnID.Value = ((HiddenField)ClickedRow.FindControl("hdnReturnID")).Value;

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@ReturnID", hdnReturnID.Value) };
                DataSet dslist = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_StockReturn_forQc_ItemList", sp, DataAccess.Return_Type.DataSet);
                if (dslist.Tables[0].Rows.Count > 0)
                {
                    GV_QC.DataSource = dslist.Tables[0];
                    GV_QC.DataBind();

                    lblRtnRemarks.Text = dslist.Tables[1].Rows[0]["Remarks"].ToString();
                    lblRtnReason.Text = dslist.Tables[1].Rows[0]["ReturnForReason"].ToString();
                    if (txtCreditnote.Text != "")
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow", "showdiv('displayLabel');", true);
                    else
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow", "showdiv('displaytable');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@ReturnID", hdnReturnID.Value), 
                    new SqlParameter("@Comments", Convert.ToString(txtcomments.Text)), 
                    new SqlParameter("@AFBy", Request.Cookies["TTSUser"].Value) 
                };
                int result = daCOTS.ExecuteNonQuery_SP("sp_Close_StockReturn_ItemList_AF", sp);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "alert('Successfully Closed');", true);
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnAFSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@Creditdate", DateTime.ParseExact(txtCreditdate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    new SqlParameter("@ReturnID", hdnReturnID.Value), 
                    new SqlParameter("@Creditnote", Convert.ToString(txtCreditnote.Text)), 
                    new SqlParameter("@Amount", Convert.ToDecimal(txtAmount.Text)), 
                    new SqlParameter("@AFBy", Request.Cookies["TTSUser"].Value) 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_StockReturn_ItemList_AF", sp);
                if (resp > 0)
                {
                    GeneratePDF();
                    lnkPdfLink.Text = hdnReturnID.Value + ".pdf";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow", "showdiv('displayLabel');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "alert('Successfully saved');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkPdfLink_click(object sender, EventArgs e)
        {
            try
            {
                string serverUrl = Server.MapPath("~/DispatchReturn/" + hdn_custcode.Value).Replace("TTS", "pdfs");
                string path = serverUrl + "/" + hdnReturnID.Value + ".pdf";
                string lnkTxt = hdnReturnID.Value + ".pdf";

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow", "showdiv('displayLabel');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "alert('Successfully saved');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void GeneratePDF()
        {
            try
            {
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/DispatchReturn/" + hdn_custcode.Value))
                    Directory.CreateDirectory(serverURL + "/DispatchReturn/" + hdn_custcode.Value);
                string path = serverURL + "/DispatchReturn/" + hdn_custcode.Value;
                string strOrderNo = Utilities.Decrypt(Request["fid"].ToString()).ToLower() == "d" ? lblStausOrderRefNo.Text : "f";
                string sPathToWritePdfTo = path + "/" + hdnReturnID.Value + ".pdf";

                SqlParameter[] spD = new SqlParameter[] { new SqlParameter("@ReturnID", hdnReturnID.Value) };
                DataSet dsData = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_dispatchreturn_details_forpdf", spD, DataAccess.Return_Type.DataSet);
                DataTable dtMaster = dsData.Tables[1];
                StockReturn_Report.CustomerName = lblCustName.Text;
                StockReturn_Report.PONO = lblStausOrderRefNo.Text;
                StockReturn_Report.Preparedby = dtMaster.Rows[0]["CreatedBy"].ToString();
                StockReturn_Report.OrderQuantity = dtMaster.Rows[0]["ReturnQty"].ToString();
                StockReturn_Report.InspectedBy = dtMaster.Rows[0]["QCBy"].ToString();
                StockReturn_Report.InvoiceNo = dtMaster.Rows[0]["InvoiceNo"].ToString();
                StockReturn_Report.InspectedOn = Convert.ToString(Convert.ToDateTime(dtMaster.Rows[0]["QCOn"].ToString()).ToShortDateString());
                StockReturn_Report.PreparedOn = Convert.ToString(Convert.ToDateTime(dtMaster.Rows[0]["Createdon"].ToString()).ToShortDateString()); ;
                StockReturn_Report.DateOfReceipt = Convert.ToString(Convert.ToDateTime(dtMaster.Rows[0]["DateOfReceipt"].ToString()).ToShortDateString());
                StockReturn_Report.CustomerReferenceNo = dtMaster.Rows[0]["Custcode"].ToString();
                StockReturn_Report.dtItem = dsData.Tables[0];
                StockReturn_Report.PDI_Generation(sPathToWritePdfTo);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnAFCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
