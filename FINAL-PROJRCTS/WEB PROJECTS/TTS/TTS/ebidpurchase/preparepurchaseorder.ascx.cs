using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;

namespace TTS.ebidpurchase
{
    public partial class preparepurchaseorder : System.Web.UI.UserControl
    {
        DataAccess daEBID = new DataAccess(ConfigurationManager.ConnectionStrings["eBidDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            bindEnquiryList();
            if (Request["enqid"] != null)
            {
                getProductDetails();
            }
        }

        private void bindEnquiryList()
        {
            try
            {
                SqlParameter[] SpParams = new SqlParameter[] { };
                DataTable dtPOEnquiriesList = (DataTable)daEBID.ExecuteReader_SP("eBitData_SP_LST_EnquiryPO_Supplier", SpParams, DataAccess.Return_Type.DataTable);

                SqlParameter[] sp_param_OtherCharge = new SqlParameter[] { };
                DataTable dt_OtherCharges = (DataTable)daEBID.ExecuteReader_SP("eBitData_SP_LST_EnquiryPO_OtherCharges", sp_param_OtherCharge, DataAccess.Return_Type.DataTable);

                DataColumn dc_Desc = new DataColumn("ChargesDescription", typeof(string));
                DataColumn dc_Percent = new DataColumn("ChargesPercent", typeof(string));
                DataColumn dc_Rate = new DataColumn("ChargesRate", typeof(string));
                dtPOEnquiriesList.Columns.Add(dc_Desc);
                dtPOEnquiriesList.Columns.Add(dc_Percent);
                dtPOEnquiriesList.Columns.Add(dc_Rate);

                for (int i = 0; i < dtPOEnquiriesList.Rows.Count; i++)
                {
                    string strDescLiteral = "", strPercentLiteral = "", strRateLiteral = "";
                    for (int j = 0; j < dt_OtherCharges.Rows.Count; j++)
                    {
                        if (dtPOEnquiriesList.Rows[i]["SupplierId"].ToString() == dt_OtherCharges.Rows[j]["SuppID"].ToString() && dtPOEnquiriesList.Rows[i]["ID"].ToString() == dt_OtherCharges.Rows[j]["ID"].ToString())
                        {
                            strDescLiteral += " <div class='comments' style='max-width: 200px; height: 20px; border:1px solid black; background-color:#f5f5dc;clear:both;margin-top:1px; margin-bottom:1px;'> <div style='max-width: 180px; height: 20px; float: left; overflow: hidden;line-height: 18px;'  class='comment'>"
                                              + dt_OtherCharges.Rows[j]["ChargesDescription"] + "</div> <literal style='float: left; margin-top: -5px; margin-left: 5px;line-height: 18px;'>... </literal></div>";
                            strPercentLiteral += "<div style='width:60px; height:20px; border:1px solid black; background-color:#f5f5dc;clear:both;margin-top:1px; margin-bottom:1px;'>" + dt_OtherCharges.Rows[j]["ChargesPercent"] + "</div>";
                            strRateLiteral += "<div style='width:60px; height:20px; border:1px solid black; background-color:#f5f5dc;clear:both;margin-top:1px; margin-bottom:1px;'>" + dt_OtherCharges.Rows[j]["ChargesRate"] + "</div>";
                        }
                    }
                    dtPOEnquiriesList.Rows[i]["ChargesDescription"] = strDescLiteral;
                    dtPOEnquiriesList.Rows[i]["ChargesPercent"] = strPercentLiteral;
                    dtPOEnquiriesList.Rows[i]["ChargesRate"] = strRateLiteral;
                }


                if (dtPOEnquiriesList.Rows.Count > 0)
                {
                    gvPOEnqList.DataSource = dtPOEnquiriesList;
                    gvPOEnqList.DataBind();
                }
                else
                {
                    gvPOEnqList.DataSource = null;
                    gvPOEnqList.DataBind();
                }


            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void getProductDetails()
        {
            try
            {
                int enqId = Convert.ToInt32(Request["enqid"]);
                SqlParameter[] SpParams1 = new SqlParameter[] { new SqlParameter("@EnquiredId", enqId) };
                DataTable dtPOEnquiriesList_Product = (DataTable)daEBID.ExecuteReader_SP("eBitData_SP_LST_EnquiryPO_Product", SpParams1, DataAccess.Return_Type.DataTable);
                if (dtPOEnquiriesList_Product.Rows.Count > 0)
                {
                    gvPOEnqListProd.DataSource = dtPOEnquiriesList_Product;
                    gvPOEnqListProd.DataBind();
                    //lblTaxPercent.Text = dtPOEnquiriesList_Product.Rows[0]["TaxPercent"].ToString();
                    //lblTotalAmount.Text = dtPOEnquiriesList_Product.Rows[0]["TotAmount"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "SshowDetailsdiv", "showDetailsDiv(1);", true);

                    lblPONumber.Text = Request.Form["lblPONumber"];
                    bindDDL();
                    getPODetails();
                }
                else
                {
                    gvPOEnqListProd.DataSource = null;
                    gvPOEnqListProd.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "SshowDetailsdiv", "showDetailsDiv(0);", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void getPODetails()
        {
            try
            {
                int enqId = Convert.ToInt32(Request["enqid"]);
                SqlParameter[] SpParams2 = new SqlParameter[] { new SqlParameter("@EnqId", enqId) };
                DataTable dtPODetails = (DataTable)daEBID.ExecuteReader_SP("eBitData_SP_GET_PurchaseOrderDetail", SpParams2, DataAccess.Return_Type.DataTable);
                if (dtPODetails.Rows.Count > 0)
                {
                    if (dtPODetails.Rows[0]["ExpArrivalDate"].ToString() != "") txtPOExpiredDate.Text = Convert.ToDateTime(dtPODetails.Rows[0]["ExpArrivalDate"]).ToString("dd/MM/yyyy");
                    ddlBillingAddress.ClearSelection();
                    ddlShippingAddress.ClearSelection();
                    if (dtPODetails.Rows[0]["BillID"].ToString() != "") ddlBillingAddress.Items.FindByValue(dtPODetails.Rows[0]["BillID"].ToString()).Selected = true;
                    if (dtPODetails.Rows[0]["ShipID"].ToString() != "") ddlShippingAddress.Items.FindByValue(dtPODetails.Rows[0]["ShipID"].ToString()).Selected = true;
                    hdnAreaComments.Value = dtPODetails.Rows[0]["PoComments"].ToString();
                    txtPreferedTransport.Text = dtPODetails.Rows[0]["PreferedTransport"].ToString();

                    if (txtPOExpiredDate.Text != "" || dtPODetails.Rows[0]["BillID"].ToString() != "" || dtPODetails.Rows[0]["ShipID"].ToString() != "" || hdnAreaComments.Value != "")
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "EditMode", "toggleEditMode(0);", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "EditMode", "toggleEditMode(1);", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindDDL()
        {
            try
            {
                DataTable dtAddress = (DataTable)daEBID.ExecuteReader_SP("[eBitData_SP_LST_ContactDetails]", DataAccess.Return_Type.DataTable);
                if (dtAddress.Rows.Count > 1)
                {
                    Dictionary<int, string> address = new Dictionary<int, string>();
                    for (int i = 0; i < dtAddress.Rows.Count; i++)
                    {
                        int id = Convert.ToInt32(dtAddress.Rows[i]["Id"]);
                        string Ref_name = dtAddress.Rows[i]["Reference_Name"].ToString();
                        string addr = dtAddress.Rows[i]["Address"].ToString();
                        string city = dtAddress.Rows[i]["City"].ToString();
                        string country = dtAddress.Rows[i]["Country"].ToString();
                        string pincode = dtAddress.Rows[i]["Pincode"].ToString();

                        address.Add(id, Ref_name + " , " + addr + " , " + city + " , " + country + " , " + pincode);
                    }

                    ddlBillingAddress.DataSource = null;
                    ddlBillingAddress.DataSource = address;
                    ddlBillingAddress.DataTextField = "Value";
                    ddlBillingAddress.DataValueField = "Key";
                    ddlBillingAddress.DataBind();

                    ddlShippingAddress.DataSource = null;
                    ddlShippingAddress.DataSource = address;
                    ddlShippingAddress.DataTextField = "Value";
                    ddlShippingAddress.DataValueField = "Key";
                    ddlShippingAddress.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSendToSupplier_Click(object sender, EventArgs eve)
        {
            submit(6);
            Response.Redirect("ebid_purchase.aspx?vid=10", false);
            Response.End();
        }

        protected void btnSavePO_Click(object sender, EventArgs eve)
        {
            submit(5);
            Response.Redirect("ebid_purchase.aspx?vid=8", false);
            Response.End();
        }

        private void submit(int enqStatus)
        {
            try
            {
                HttpCookie userEmail = new HttpCookie("TTSUserEmail");
                userEmail = Request.Cookies["TTSUserEmail"];

                int enqid = Convert.ToInt32(Request["enqid"]);
                string poNumber = hdnPONumber.Value.ToString();
                string poUsername = Request.Cookies["TTSUser"].Value.ToString();
                int supid = Convert.ToInt32(Request["supid"]);
                int billid = Convert.ToInt32(ddlBillingAddress.SelectedValue);
                int shipid = Convert.ToInt32(ddlShippingAddress.SelectedValue);
                string preferedTransport = txtPreferedTransport.Text.ToString();

                string expArrivalDate = "";
                if (txtPOExpiredDate.Text.ToString() != "") expArrivalDate = Convert.ToDateTime(txtPOExpiredDate.Text).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss");


                string poComments = hdnAreaComments.Value.ToString();

                SqlParameter[] sp_param_PO = new SqlParameter[]{
                new SqlParameter("@EnqId",enqid),
                new SqlParameter("@PoNo",poNumber),
                new SqlParameter("@PoUsername",poUsername),
                new SqlParameter("@SupId", supid),
                new SqlParameter("@BillId", billid),
                new SqlParameter("@ShipId", shipid),
                new SqlParameter("@ExpArrivalDate",SqlDbType.DateTime),
                new SqlParameter("@PoComments",SqlDbType.NVarChar,1000),
                new SqlParameter("@EnqStatus",enqStatus),
                new SqlParameter("@preferedTransport",preferedTransport)
                };

                if (expArrivalDate == "") sp_param_PO[6].Value = DBNull.Value;
                else sp_param_PO[6].Value = expArrivalDate;

                if (poComments == "") sp_param_PO[7].Value = DBNull.Value;
                else sp_param_PO[7].Value = poComments;
                daEBID.ExecuteNonQuery_SP("eBitData_SP_SAV_PurchaseOrderPrepare", sp_param_PO);


            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}