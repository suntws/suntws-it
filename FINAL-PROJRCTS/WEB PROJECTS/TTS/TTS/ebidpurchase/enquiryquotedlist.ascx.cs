using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using System.IO;
using System.Reflection;

namespace TTS.ebidpurchase
{
    public partial class enquiryquotedlist : System.Web.UI.UserControl
    {
        DataAccess da = new DataAccess(ConfigurationManager.ConnectionStrings["eBidDB"].ConnectionString);
        private int enquiryId;
        protected void Page_Load(object sender, EventArgs e)
        {
            bindMainGrid();
            if (Request["enqid"] != null)
            {
                enquiryId = Convert.ToInt32(Request["enqid"]);
                ShowDetails(enquiryId);
            }
        }

        private void bindMainGrid()
        {
            try
            {
                DataTable dt = (DataTable)da.ExecuteReader_SP("eBitData_SP_LST_EnquiryQuoted", DataAccess.Return_Type.DataTable);
                gvPurEnqList.DataSource = dt;
                gvPurEnqList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ShowDetails(int enqId)
        {
            try
            {
                gvEnqDetails.DataSource = null;
                gvEnqDetails.DataBind();
                gvEnquirySuppliersProduct.DataSource = null;
                gvEnquirySuppliersProduct.DataBind();

                SqlParameter[] sp_param = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId) };
                DataTable dt = (DataTable)da.ExecuteReader_SP("eBitData_SP_GET_EnquirysSuppliers", sp_param, DataAccess.Return_Type.DataTable);

                SqlParameter[] sp_param_OtherCharge = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId) };
                DataTable dt_OtherCharges = (DataTable)da.ExecuteReader_SP("eBitData_SP_GET_OtherCharges", sp_param_OtherCharge, DataAccess.Return_Type.DataTable);

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
                        if (dt.Rows[i]["SupplierId"].ToString() == dt_OtherCharges.Rows[j]["SuppID"].ToString() && dt.Rows[i]["EnquiredId"].ToString() == dt_OtherCharges.Rows[j]["EnqId"].ToString())
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

                if (dt.Rows.Count > 0)
                {
                    gvEnqDetails.DataSource = dt;
                    gvEnqDetails.DataBind();

                    lblEnquiryNo.Text = dt.Rows[0]["EnquiryNo"].ToString();
                }
                else
                {
                    gvEnqDetails.DataSource = null;
                    gvEnqDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkSupplierQuote_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                getEnquirySuppliersProduct(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void getEnquirySuppliersProduct(GridViewRow row)
        {
            try
            {
                int enqId = Convert.ToInt32(Request["enqid"]);
                HiddenField hdnSupplierId = (HiddenField)row.FindControl("hdnSupplierId");
                int supId = Convert.ToInt32(hdnSupplierId.Value);
                hdnsuppID.Value = hdnSupplierId.Value;

                LinkButton lnk1 = (LinkButton)row.FindControl("lnkSupplierQuote");
                spnName.InnerHtml = lnk1.Text;
                HiddenField hdnSuppEmailID = (HiddenField)row.FindControl("hdnSuppEmailID");
                spnEmail.InnerHtml = hdnSuppEmailID.Value;
                HiddenField hdnSuppCountry = (HiddenField)row.FindControl("hdnSuppCountry");
                spnCountry.InnerHtml = hdnSuppCountry.Value;
                HiddenField hdnSuppCity = (HiddenField)row.FindControl("hdnSuppCity");
                spnCity.InnerHtml = hdnSuppCity.Value;
                HiddenField hdnSupplierComment = (HiddenField)row.FindControl("hdnSupplierComment");
                divComments.InnerHtml = hdnSupplierComment.Value;

                SqlParameter[] sp_param = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId), new SqlParameter("@SuppId", supId) };
                DataTable dt = (DataTable)da.ExecuteReader_SP("eBitData_SP_GET_EnquirysSuppliersProduct", sp_param, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvEnquirySuppliersProduct.DataSource = dt;
                    gvEnquirySuppliersProduct.DataBind();

                    lblSupplierName.Text = dt.Rows[0]["SupplierName"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showDetailsDiv", "showDetailsDiv(1);", true);
                }
                else
                {
                    gvEnquirySuppliersProduct.DataSource = null;
                    gvEnquirySuppliersProduct.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int enqId = Convert.ToInt32(Request["enqid"]);
                int supId = Convert.ToInt32(hdnsuppID.Value);

                SqlParameter[] sp_param = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId), new SqlParameter("@SuppId", supId) };
                da.ExecuteNonQuery_SP("eBitData_SP_UPD_EnquiryConfirm", sp_param);
                Response.Redirect("ebid_purchase.aspx?vid=8", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}