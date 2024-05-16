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

namespace TTS.ebidpurchase
{
    public partial class enquirycancel : System.Web.UI.UserControl
    {
        DataAccess da = new DataAccess(ConfigurationManager.ConnectionStrings["eBidDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            bindMainGrid();

            if (Request["enqid"] != null && Request["enqid"] != "")
            {
                getEnqDetails();
            }
        }

        private void bindMainGrid()
        {
            try
            {
                DataTable dt = (DataTable)da.ExecuteReader_SP("eBitData_SP_LST_Enquiry", DataAccess.Return_Type.DataTable);
                gvEnquiryList.DataSource = dt;
                gvEnquiryList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ebid_enquirycancel", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void getEnqDetails()
        {
            try
            {
                int enqId = Convert.ToInt32(Request["enqid"]);
                SqlParameter[] sp_param = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId) };
                DataTable dt = (DataTable)da.ExecuteReader_SP("eBitData_SP_GET_EnquiryCancel_Details", sp_param, DataAccess.Return_Type.DataTable);

                SqlParameter[] sp_param_OtherCharge = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId) };
                DataTable dt_OtherCharges = (DataTable)da.ExecuteReader_SP("[eBitData_SP_GET_EnquiryCancel_OtherCharges]", sp_param_OtherCharge, DataAccess.Return_Type.DataTable);

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
                lblEnquiryNo.Text = dt.Rows[0]["EnqNo"].ToString();
                ScriptManager.RegisterStartupScript(Page, GetType(), "showDetailsDiv", "showDetailsDiv(1);", true);

                if (Convert.ToInt32(dt.Rows[0]["StatusID"]) < 4) ScriptManager.RegisterStartupScript(Page, GetType(), "showPriceColumn", "showPriceColumns(1);", true);
                else ScriptManager.RegisterStartupScript(Page, GetType(), "showPriceColumn", "showPriceColumns(0);", true);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ebid_enquirycancel", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnConfirmCancel_Click(object sender, EventArgs e)
        {
            try
            {
                int enqid = Convert.ToInt32(Request["enqid"]);
                string cancelComment = hdnCancelComment.Value;
                SqlParameter[] sp_param2 = new SqlParameter[] { 
                    new SqlParameter("@EnquiryId", enqid),
                    new SqlParameter("@EnqStatus", 10), 
                    new SqlParameter("@CancelComments", cancelComment ) ,
                    new SqlParameter("@User", 1) 

                };
                da.ExecuteNonQuery_SP("eBitData_SP_UPD_CancelStatus", sp_param2);
                Response.Redirect("ebid_purchase.aspx?vid=10", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ebid_enquirycancel", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}