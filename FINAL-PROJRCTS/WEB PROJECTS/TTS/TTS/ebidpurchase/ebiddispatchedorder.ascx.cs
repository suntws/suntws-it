using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;


namespace TTS.ebidpurchase
{
    public partial class ebiddispatchedorder : System.Web.UI.UserControl
    {
        DataAccess da = new DataAccess(ConfigurationManager.ConnectionStrings["eBidDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindMainGrid();

                if (Request["enqid"] != null && Request["enqid"] != "")
                {
                    getEnqDetails();
                }
            }
        }

        private void bindMainGrid()
        {
            try
            {
                DataTable dt = (DataTable)da.ExecuteReader_SP("eBitData_SP_LST_DispatchedOrder", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count <= 0) return;
                gvEnquiryList.DataSource = dt;
                gvEnquiryList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ebid_dispatchedorder", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void getEnqDetails()
        {
            try
            {
                int enqId = Convert.ToInt32(Request["enqid"]);
                SqlParameter[] sp_param = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId) };
                DataTable dt = (DataTable)da.ExecuteReader_SP("eBitData_SP_GET_EnquiryDispatched_Details", sp_param, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count <= 0) return;

                SqlParameter[] sp_param_OtherCharge = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId) };
                DataTable dt_OtherCharges = (DataTable)da.ExecuteReader_SP("[eBitData_SP_GET_EnquiryDispatched_OtherCharges]", sp_param_OtherCharge, DataAccess.Return_Type.DataTable);

                DataColumn dc_Desc = new DataColumn("ChargesDescription", typeof(string));
                DataColumn dc_Percent = new DataColumn("ChargesPercent", typeof(string));
                DataColumn dc_Rate = new DataColumn("ChargesRate", typeof(string));
                dt.Columns.Add(dc_Desc);
                dt.Columns.Add(dc_Percent);
                dt.Columns.Add(dc_Rate);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strDescLiteral = "", strPercentLiteral = "", strRateLiteral = "";
                    for (int j = 0; j < dt_OtherCharges.Rows.Count; j++)
                    {
                        if (dt.Rows[i]["SuppID"].ToString() == dt_OtherCharges.Rows[j]["SuppID"].ToString())
                        {
                            strDescLiteral += " <div class='comments' style='max-width: 200px; height: 20px; border:1px solid black; background-color:#f5f5dc;clear:both;margin-top:1px; margin-bottom:1px;'> <div style='max-width: 180px; height: 20px; float: left; overflow: hidden'  class='comment'>"
                                              + dt_OtherCharges.Rows[j]["ChargesDescription"] + "</div> <literal style='float: left; margin-top: -5px; margin-left: 5px;'>... </literal></div>";
                            strPercentLiteral += "<div style='width:60px; height:20px; border:1px solid black; background-color:#f5f5dc;clear:both;margin-top:1px; margin-bottom:1px;'>" + dt_OtherCharges.Rows[j]["ChargesPercent"] + "</div>";
                            strRateLiteral += "<div style='width:60px; height:20px; border:1px solid black; background-color:#f5f5dc;clear:both;margin-top:1px; margin-bottom:1px;'>" + dt_OtherCharges.Rows[j]["ChargesRate"] + "</div>";
                        }
                    }
                    dt.Rows[i]["ChargesDescription"] = strDescLiteral;
                    dt.Rows[i]["ChargesPercent"] = strPercentLiteral;
                    dt.Rows[i]["ChargesRate"] = strRateLiteral;
                }

                gvEnquiryDetails.DataSource = dt;
                gvEnquiryDetails.DataBind();

                hdnShipAddr.Value = dt.Rows[0]["ShipAddr"].ToString();
                hdnBillAddr.Value = dt.Rows[0]["BillAddr"].ToString();
                hdnPoComments.Value = dt.Rows[0]["PoComments"].ToString();
                hdnPoUsername.Value = dt.Rows[0]["PoUsername"].ToString();
                hdnExpArrivalDate.Value = dt.Rows[0]["ExpArrivalDate"].ToString();
                hdnPoDate.Value = dt.Rows[0]["PoDate"].ToString();
                hdnPoNo.Value = dt.Rows[0]["PoNo"].ToString();
                hdnFilename.Value = dt.Rows[0]["Filename"].ToString();
                hdnLRNumber.Value = dt.Rows[0]["LRNumber"].ToString();
                hdnInvoiceDate.Value = dt.Rows[0]["InvoiceDate"].ToString();
                hdnInvoiceNo.Value = dt.Rows[0]["InvoiceNo"].ToString();
                lblEnquiryNo.Text = dt.Rows[0]["EnqNo"].ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ebid_dispatchedorder", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }

        protected void btnSubmit_Click(Object sender, EventArgs e)
        {
            try
            {
                int enqId = Convert.ToInt32(Request["enqid"]);
                string comments = hdnReceiverComments.Value;
                SqlParameter[] sp_param3 = new SqlParameter[] {
                    new SqlParameter("@EnquiryId", enqId),
                    new SqlParameter("@ReceiverComments", comments)
                };
                da.ExecuteNonQuery_SP("eBitData_SP_UPD_ReceivedStatus", sp_param3);
                Response.Redirect("ebid_purchase.aspx?vid=10", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ebid_dispatchedorder", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                string serverURL = Server.MapPath("~/invoicefiles/" + hdnSupplierID.Value + "").Replace("TTS\\TTS", "pdfs\\pdfs\\ebid");
                string spdfPathTo = serverURL + "\\" + hdnFilename.Value;
                Response.AddHeader("content-disposition", "attachment; filename=\"" + hdnFilename.Value + "\"");
                Response.WriteFile(spdfPathTo);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ebid_ordertracking", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}