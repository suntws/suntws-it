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
using System.Text;

namespace TTS
{
    public partial class CotsReviseResponse : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["dom_proforma"].ToString() == "True" || dtUser.Rows[0]["dom_invoice"].ToString() == "True"
                            || dtUser.Rows[0]["dom_paymentconfirm"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["ot_qcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True"))
                        {
                            if (dtUser.Rows[0]["dom_proforma"].ToString() == "True")
                                hdnApproveBy.Value = "1";
                            if (dtUser.Rows[0]["dom_invoice"].ToString() == "True")
                                hdnApproveBy.Value = (hdnApproveBy.Value != "" ? hdnApproveBy.Value + "," : "") + "3";
                            if (dtUser.Rows[0]["dom_paymentconfirm"].ToString() == "True")
                                hdnApproveBy.Value = (hdnApproveBy.Value != "" ? hdnApproveBy.Value + "," : "") + "4";
                            if (dtUser.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk"].ToString() == "True")
                                hdnApproveBy.Value = (hdnApproveBy.Value != "" ? hdnApproveBy.Value + "," : "") + "5";
                            if (dtUser.Rows[0]["ot_qcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True")
                                hdnApproveBy.Value = (hdnApproveBy.Value != "" ? hdnApproveBy.Value + "," : "") + "6";
                            ddlplant_SelectedIndexChanged(sender, e);
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
                Utilities.WriteToErrorLog("DOMESTIC REVISE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvRequestedOrderDetails.DataSource = null;
                gvRequestedOrderDetails.DataBind();
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@Plant", ddlplant.SelectedValue.ToString()), 
                    new SqlParameter("@ReviseRequestTo", hdnApproveBy.Value) 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Request_orders_List", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvRequestedOrderDetails.DataSource = dt;
                    gvRequestedOrderDetails.DataBind();
                }
                else
                    lblErrMsgcontent.Text = "No Records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("DOMESTIC REVISE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkApproveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Bind_gvSingleView_OrderItemList(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_gvSingleView_OrderItemList(GridViewRow Row)
        {
            try
            {
                lblStausOrderRefNo.Text = ((Label)Row.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)Row.FindControl("hdnStatusCustCode")).Value;
                lblCustName.Text = ((Label)Row.FindControl("lblStatusCustName")).Text;
                hdnResponseID.Value = ((HiddenField)Row.FindControl("hdnReviseRequestTo")).Value;
                hdnOrderStatusID.Value = ((HiddenField)Row.FindControl("hdnOrderStatus")).Value;
                hdnOID.Value = ((HiddenField)Row.FindControl("hdnOrderID")).Value;

                hdnPlant.Value = Row.Cells[5].Text;
                Bind_OrderItem();
                Bind_RequestedComments();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('div_SingleView');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderItem()
        {
            try
            {
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(hdnOID.Value));
                if (dtItemList.Rows.Count > 0)
                {
                    gvSingleView_OrderItemList.DataSource = dtItemList;
                    gvSingleView_OrderItemList.DataBind();

                    gvSingleView_OrderItemList.FooterRow.Cells[7].Text = "TOTAL";
                    gvSingleView_OrderItemList.FooterRow.Cells[7].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gvSingleView_OrderItemList.FooterRow.Cells[13].Text = dtItemList.Compute("Sum(finishedwt)", "").ToString();
                    gvSingleView_OrderItemList.FooterRow.Cells[14].Text = Math.Round(Convert.ToDecimal(dtItemList.Compute("Sum(unitprice)", ""))).ToString("0.00");

                    gvSingleView_OrderItemList.Columns[10].Visible = false;
                    gvSingleView_OrderItemList.Columns[11].Visible = false;
                    gvSingleView_OrderItemList.Columns[12].Visible = false;
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvSingleView_OrderItemList.Columns[10].Visible = true;
                        gvSingleView_OrderItemList.Columns[11].Visible = true;
                        gvSingleView_OrderItemList.Columns[12].Visible = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_RequestedComments()
        {
            SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
            DataTable dtStatus = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_statuschange_history", sp1, DataAccess.Return_Type.DataTable);
            if (dtStatus.Rows.Count > 0)
            {
                txtRequestedCommands.Text = dtStatus.Rows[0]["feedback"].ToString().Replace("~", "\r\n");
            }
        }
        protected void btnResponsesend_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), "17", txtResponsedCommands.Text.Replace("\r\n", "~"),
                    Request.Cookies["TTSUser"].Value);
                string strTbField = string.Empty;
                if (hdnResponseID.Value == "1")
                    strTbField = "dom_proforma";
                else if (hdnResponseID.Value == "3")
                    strTbField = "dom_invoice";
                else if (hdnResponseID.Value == "4")
                    strTbField = "dom_paymentconfirm";
                else if (hdnResponseID.Value == "5")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@custcode", hdnCustCode.Value), 
                        new SqlParameter("@orderrefno", lblStausOrderRefNo.Text), 
                        new SqlParameter("@PdiPlant", hdnPlant.Value),  
                        new SqlParameter("@orderstatusid", hdnOrderStatusID.Value) 
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_upd_pdi_scanmasterdata_holdstatus", sp);
                    strTbField = "dom_pdi_" + hdnPlant.Value.ToLower();
                }
                else if (hdnResponseID.Value == "6")
                    strTbField = "ot_qc" + hdnPlant.Value;

                Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}