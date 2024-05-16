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
    public partial class ExpPPC_Revision : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sitl"].ToString() == "True"
                            || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True" || dtUser.Rows[0]["ot_qcmmn"].ToString() == "True"
                            || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True" || dtUser.Rows[0]["exp_pricechange"].ToString() == "True" || dtUser.Rows[0]["exp_ordersplit"].ToString() == "True"
                            || dtUser.Rows[0]["exp_revise"].ToString() == "True" || dtUser.Rows[0]["dom_pricechange"].ToString() == "True" || dtUser.Rows[0]["dom_ordersplit"].ToString() == "True"
                            || dtUser.Rows[0]["dom_revise"].ToString() == "True" || dtUser.Rows[0]["dom_orderentry"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "" && Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                if (Utilities.Decrypt(Request["pid"].ToString()) == "1")
                                {
                                    lblPageHead.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "e" ? "EXPORT " : "DOMESTIC ") + "PPC REVISE RESPONSE";
                                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())) };
                                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ppc_revise_response_CRM_orders", sp1, DataAccess.Return_Type.DataTable);
                                    if (dt.Rows.Count > 0)
                                    {
                                        gv_PPCOrders.DataSource = dt;
                                        gv_PPCOrders.DataBind();
                                    }
                                }
                                else
                                {
                                    lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                        " - EXPORT " : " - DOMESTIC ") + "REVISE REQUEST";

                                    SqlParameter[] sp1 = new SqlParameter[] { 
                                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                                        new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())) 
                                    };
                                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ppc_revise_request_orders", sp1, DataAccess.Return_Type.DataTable);
                                    if (dt.Rows.Count > 0)
                                    {
                                        gv_PPCOrders.DataSource = dt;
                                        gv_PPCOrders.DataBind();
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL IS WRONG";
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
        protected void lnkReviseOrder_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblSelectedCustomerName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblSelectedOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
                lblCurrStatus.Text = ((Label)clickedRow.FindControl("lblStatusText")).Text;
                hdnCurrStatusID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderStatus")).Value;
                hdnPlant.Value = ((Label)clickedRow.FindControl("LblPlant")).Text;
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;

                BuildOrderItemDetails();

                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Sub_OrderItems');", true);
                if (Utilities.Decrypt(Request["pid"].ToString()) == "1")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV1", "gotoPreviewDiv('div_response');", true);
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV2", "gotoPreviewDiv('div_Request');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void BuildOrderItemDetails()
        {
            try
            {
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(hdnOID.Value));
                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    gvOrderItemList.FooterRow.Cells[6].Text = "TOTAL";
                    gvOrderItemList.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    object sumCost = dtItemList.Compute("Sum(unitprice)", "");
                    gvOrderItemList.FooterRow.Cells[7].Text = Convert.ToDecimal(sumCost).ToString();

                    object sumQty = dtItemList.Compute("Sum(itemqty)", "");
                    gvOrderItemList.FooterRow.Cells[8].Text = sumQty.ToString();

                    object sumFwt = dtItemList.Compute("Sum(finishedwt)", "");
                    gvOrderItemList.FooterRow.Cells[9].Text = Convert.ToDecimal(sumFwt).ToString();

                    gvOrderItemList.Columns[10].Visible = false; //RIM QTY
                    gvOrderItemList.Columns[11].Visible = false; //RIM BASIC PRICE
                    gvOrderItemList.Columns[12].Visible = false; //RIM FWT
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[10].Visible = true;
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnppcrequestsend_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@ReviseRequestTo", Convert.ToInt32(hdnCurrStatusID.Value)),
                        new SqlParameter("@RequestStatus", Convert.ToInt32("30"))
                    };
                daCOTS.ExecuteNonQuery_SP("sp_update_reviserequest", sp2);

                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), "30", txtppcrequestcommands.Text.Replace("\r\n", "~"),
                    Request.Cookies["TTSUser"].Value);

                if (resp > 0)
                {
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPpcApprove_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), "32", txtcrmresponsecomments.Text.Replace("\r\n", "~"),
                    Request.Cookies["TTSUser"].Value);

                if (resp > 0)
                {
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}