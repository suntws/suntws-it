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
    public partial class CotsReviseRequest : System.Web.UI.Page
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
                            (dtUser.Rows[0]["dom_orderentry"].ToString() == "True" || dtUser.Rows[0]["dom_pricechange"].ToString() == "True"
                            || dtUser.Rows[0]["dom_ordersplit"].ToString() == "True" || dtUser.Rows[0]["dom_revise"].ToString() == "True"))
                        {
                            DataTable dt = (DataTable)daCOTS.ExecuteReader("sp_sel_orderStatusMaster_request", DataAccess.Return_Type.DataTable);
                            rdbrequestcrm.DataSource = dt;
                            rdbrequestcrm.DataTextField = "StatusText";
                            rdbrequestcrm.DataValueField = "id";
                            rdbrequestcrm.DataBind();

                            Bind_gvOrderDetails();
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
        private void Bind_gvOrderDetails()
        {
            try
            {
                string strUserName = "";
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                    strUserName = Request.Cookies["TTSUser"].Value;
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@Username", strUserName);
                sp2[1] = new SqlParameter("@Plant", ddlplant.SelectedValue.ToString());
                DataTable dtlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Cots_ChangeRequest_orders_List", sp2, DataAccess.Return_Type.DataTable);
                if (dtlist.Rows.Count > 0)
                {
                    gvOrderDetails.DataSource = dtlist;
                    gvOrderDetails.DataBind();
                }
                else
                    lblErrMsgcontent.Text = "No Records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("DOMESTIC REVISE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_gvOrderDetails();
        }
        protected void lnkRequest_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
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
                lblCurrStatus.Text = ((Label)Row.FindControl("lblStatusText")).Text;
                lblCustName.Text = ((Label)Row.FindControl("lblStatusCustName")).Text;
                hdnOID.Value = ((HiddenField)Row.FindControl("hdnOrderID")).Value;

                hdnPlant.Value = Row.Cells[5].Text;
                Bind_OrderItem();
                Build_RequestTo(((HiddenField)Row.FindControl("hdnStatusID")).Value);
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

                    gvSingleView_OrderItemList.FooterRow.Cells[6].Text = "TOTAL";
                    gvSingleView_OrderItemList.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                    gvSingleView_OrderItemList.FooterRow.Cells[7].Text = (dtItemList.Compute("Sum(itemqty)", "")).ToString();
                    gvSingleView_OrderItemList.FooterRow.Cells[13].Text = Convert.ToDecimal(dtItemList.Compute("Sum(unitprice)", "")).ToString();
                    gvSingleView_OrderItemList.FooterRow.Cells[14].Text = Convert.ToDecimal(dtItemList.Compute("Sum(finishedwt)", "")).ToString();

                    gvSingleView_OrderItemList.Columns[10].Visible = false;
                    gvSingleView_OrderItemList.Columns[11].Visible = false;
                    gvSingleView_OrderItemList.Columns[12].Visible = false;
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvSingleView_OrderItemList.FooterRow.Cells[10].Text = (dtItemList.Compute("Sum(Rimitemqty)", "")).ToString();
                        gvSingleView_OrderItemList.Columns[10].Visible = true;
                        gvSingleView_OrderItemList.Columns[11].Visible = true;
                        gvSingleView_OrderItemList.Columns[12].Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_RequestTo(string statusid)
        {
            try
            {
                if (statusid == "2" || statusid == "3" || statusid == "34")
                {
                    lblApproveTeam.Text = "CRM";
                    hdnApprovalID.Value = "1";
                    hdnApprovalTeam.Value = "dom_proforma";
                }
                else if (statusid == "7")
                {
                    lblApproveTeam.Text = "DISPATCH";
                    hdnApprovalID.Value = "3";
                    hdnApprovalTeam.Value = "dom_invoice";
                }
                else if (statusid == "8")
                {
                    lblApproveTeam.Text = "ACCOUNTS";
                    hdnApprovalID.Value = "4";
                    hdnApprovalTeam.Value = "dom_paymentconfirm";
                }
                else if (statusid == "19")
                {
                    lblApproveTeam.Text = "INSPECTION";
                    hdnApprovalID.Value = "5";
                    hdnApprovalTeam.Value = "dom_pdi_" + hdnPlant.Value.ToLower();
                }
                else if (statusid == "4" || statusid == "24" || statusid == "41")
                {
                    lblApproveTeam.Text = "PPC";
                    hdnApprovalID.Value = "6";
                    hdnApprovalTeam.Value = "ot_qc" + hdnPlant.Value.ToLower();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnrequestsend_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@ReviseRequestTo", Convert.ToInt32(hdnApprovalID.Value)),
                        new SqlParameter("@RequestStatus", Convert.ToInt32(rdbrequestcrm.SelectedItem.Value))
                    };
                daCOTS.ExecuteNonQuery_SP("sp_update_reviserequest", sp2);

                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), rdbrequestcrm.SelectedItem.Value,
                    txtrequestcommands.Text.Replace("\r\n", "~"), Request.Cookies["TTSUser"].Value);

                if (resp > 0)
                {
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}