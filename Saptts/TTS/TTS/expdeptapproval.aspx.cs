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
    public partial class expdeptapproval : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["exp_orderentry"].ToString() == "True" || dtUser.Rows[0]["exp_plantassign"].ToString() == "True"
                    || dtUser.Rows[0]["exp_proforma"].ToString() == "True" || dtUser.Rows[0]["exp_substitution"].ToString() == "True" || dtUser.Rows[0]["exp_documents"].ToString() == "True"
                    || dtUser.Rows[0]["exp_ordertrack"].ToString() == "True" || dtUser.Rows[0]["exp_pricechange"].ToString() == "True" || dtUser.Rows[0]["exp_ordersplit"].ToString() == "True"
                    || dtUser.Rows[0]["exp_revise"].ToString() == "True" || dtUser.Rows[0]["exp_usermaster"].ToString() == "True" || dtUser.Rows[0]["exp_paymentcontrol"].ToString() == "True"
                    || dtUser.Rows[0]["exp_leadassign"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True"
                    || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True" || dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sitl"].ToString() == "True"
                    || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True"
                    || dtUser.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True" || dtUser.Rows[0]["exp_earmark_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_earmark_sltl"].ToString() == "True" || dtUser.Rows[0]["exp_earmark_sitl"].ToString() == "True"
                    || dtUser.Rows[0]["exp_earmark_pdk"].ToString() == "True"))
                        {
                            if ((Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "") &&
                                (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != ""))
                            {
                                tbApproval.Visible = false;
                                tbSelection.Visible = false;
                                Bind_ApprovalWaitingOrder();
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
        protected void lnkOrderSelection_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblSelectedCustomerName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblSelectedOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;

                Bind_OrderItemList();
                ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv", "gotoPreviewDiv('div_OrderItems');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderItemList()
        {
            try
            {
                gv_OrderItems.DataSource = null;
                gv_OrderItems.DataBind();
                tbApproval.Visible = false;
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@custcode", hdnCustCode.Value), 
                    new SqlParameter("@orderrefno", lblSelectedOrderRefNo.Text), 
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) 
                };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_DepartmentApproval", sp, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gv_OrderItems.DataSource = dtItemList;
                    gv_OrderItems.DataBind();

                    gv_OrderItems.FooterRow.Cells[6].Text = "Total";
                    gv_OrderItems.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    gv_OrderItems.FooterRow.Cells[7].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[9].Text = dtItemList.Compute("Sum(TotalfinishedWT)", "").ToString();
                    foreach (DataRow row1 in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gv_OrderItems.Columns[10].Visible = true;
                        gv_OrderItems.FooterRow.Cells[10].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                    }

                    for (int i = 0; i < dtItemList.Rows.Count; i++)
                    {
                        string Department = (dtItemList.Rows[i]["Department"].ToString());
                        if (Department == "")
                        {
                            Department = Request.Cookies["TTSUserDepartment"].Value;
                        }
                        {
                            switch (Department)
                            {
                                case "CRM":
                                    if (dtItemList.Rows[i]["Comments"].ToString() == "")
                                    {
                                        tbSelection.Visible = true;
                                        lblDepartment.Text = Department;
                                        rdbSelection.Focus();
                                        lblCrmApprovalBy.Visible = false;
                                        lblCrmApprovalOn.Visible = false;
                                        //txtCrmApprovalBy.Visible = false;
                                        //txtCrmApprovalOn.Visible = false;

                                    }
                                    else
                                    {
                                        tbApproval.Visible = true;
                                        tbSelection.Visible = false;
                                        lblCrmApprovalBy.Visible = true;
                                        lblCrmApprovalOn.Visible = true;
                                        rdbCrm.SelectedIndex = Convert.ToInt32(dtItemList.Rows[i]["Approval"].ToString());
                                        txtCrmComments.Text = dtItemList.Rows[i]["Comments"].ToString();
                                        lblCrmApprovalBy.Text = dtItemList.Rows[i]["UserName"].ToString();
                                        lblCrmApprovalOn.Text = dtItemList.Rows[i]["ApprovalOn"].ToString();

                                    }
                                    break;
                                case "PPL":
                                    if (dtItemList.Rows[i]["Comments"].ToString() == "")
                                    {
                                        tbSelection.Visible = true;
                                        lblDepartment.Text = Department;
                                        rdbSelection.Focus();
                                        lblPplApprovalOn.Visible = false;
                                        lblPplApprovalBy.Visible = false;
                                    }
                                    else
                                    {
                                        if ("1" == dtItemList.Rows[i]["Department"].ToString())
                                            rdbPpl.SelectedIndex = 0;
                                        tbApproval.Visible = true;
                                        tbSelection.Visible = false;
                                        txtPplComments.Text = dtItemList.Rows[i]["Comments"].ToString();
                                        lblPplApprovalBy.Text = dtItemList.Rows[i]["Approval"].ToString();
                                        lblPplApprovalOn.Text = dtItemList.Rows[i]["ApprovalOn"].ToString();
                                    }
                                    break;
                                case "PDI":
                                    if (dtItemList.Rows[i]["Comments"].ToString() == "")
                                    {
                                        tbSelection.Visible = true;

                                        lblDepartment.Text = Department;
                                        rdbSelection.Focus();
                                        lblPdiApprovalOn.Visible = false;
                                        lblPdiApprovalBy.Visible = false;
                                    }
                                    else
                                    {
                                        tbApproval.Visible = true;
                                        tbSelection.Visible = false;
                                        rdbPdi.SelectedIndex = Convert.ToInt32(dtItemList.Rows[i]["Approval"].ToString());
                                        txtPdiComments.Text = dtItemList.Rows[i]["Comments"].ToString();
                                        lblPdiApprovalBy.Text = dtItemList.Rows[i]["Approval"].ToString();
                                        lblPdiApprovalOn.Text = dtItemList.Rows[i]["ApprovalOn"].ToString();
                                    }
                                    break;
                                case "LOG":
                                    if (dtItemList.Rows[i]["Comments"].ToString() == "")
                                    {
                                        tbSelection.Visible = true;
                                        lblDepartment.Text = Department;
                                        rdbSelection.Focus();
                                        lblLogApprovalBy.Visible = false;
                                        lblLogApprovalOn.Visible = false;
                                    }
                                    else
                                    {
                                        tbApproval.Visible = true;
                                        tbSelection.Visible = false;
                                        rdbLog.SelectedIndex = Convert.ToInt32(dtItemList.Rows[i]["Approval"].ToString());
                                        txtLogComments.Text = dtItemList.Rows[i]["Comments"].ToString();
                                        lblLogApprovalBy.Text = dtItemList.Rows[i]["Approval"].ToString();
                                        lblLogApprovalOn.Text = dtItemList.Rows[i]["ApprovalOn"].ToString();
                                    }
                                    break;
                                case "AF":
                                    if (dtItemList.Rows[i]["Comments"].ToString() == "")
                                    {
                                        tbSelection.Visible = true;
                                        lblDepartment.Text = Department;
                                        rdbSelection.Focus();
                                        lblAFApprovalBy.Visible = false;
                                        lblAFApprovalOn.Visible = false;
                                    }
                                    else
                                    {
                                        tbApproval.Visible = true;
                                        tbSelection.Visible = false;
                                        rdbAF.SelectedIndex = Convert.ToInt32(dtItemList.Rows[i]["Approval"].ToString());
                                        txtAFComments.Text = dtItemList.Rows[i]["Comments"].ToString();
                                        lblAFApprovalBy.Text = dtItemList.Rows[i]["Approval"].ToString();
                                        lblAFApprovalOn.Text = dtItemList.Rows[i]["ApprovalOn"].ToString();

                                    }
                                    break;
                                case "PURCHASE":
                                    if (dtItemList.Rows[i]["Comments"].ToString() == "")
                                    {

                                        tbSelection.Visible = true;

                                        lblDepartment.Text = Department;
                                        rdbSelection.Focus();
                                        lblPurchaseApprovalBy.Visible = false;
                                        lblPurchaseApprovalOn.Visible = false;
                                    }
                                    else
                                    {

                                        rdbPurchase.SelectedIndex = Convert.ToInt32(dtItemList.Rows[i]["Approval"].ToString());
                                        tbApproval.Visible = true;
                                        tbSelection.Visible = false;
                                        txtPurchaseComments.Text = dtItemList.Rows[i]["Comments"].ToString();
                                        lblPurchaseApprovalBy.Text = dtItemList.Rows[i]["Approval"].ToString();
                                        lblPurchaseApprovalOn.Text = dtItemList.Rows[i]["ApprovalOn"].ToString();
                                    }
                                    break;
                                default:
                                    Response.Redirect("sessionexp.aspx", false);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ApprovalWaitingOrder()
        {
            try
            {
                gv_WaitingOrders.DataSource = null;
                gv_WaitingOrders.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@Plant", ddlplant.SelectedValue.ToString()) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Request_orders_List", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gv_WaitingOrders.DataSource = dt;
                    gv_WaitingOrders.DataBind();
                }
                else
                    lblErrMsg.Text = "No Records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("DOMESTIC REVISE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_ApprovalWaitingOrder();
        }
        protected void btnSaveDeptApproval_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[7];
                sp1[0] = new SqlParameter("@Department", Convert.ToString(lblDepartment.Text));
                sp1[1] = new SqlParameter("@Selection", Convert.ToInt32(rdbSelection.SelectedValue));
                sp1[2] = new SqlParameter("@Comments", Convert.ToString(txtComments.Text));
                sp1[3] = new SqlParameter("@custcode", Convert.ToString(hdnCustCode.Value));
                sp1[4] = new SqlParameter("@orderrefno", Convert.ToString(lblSelectedOrderRefNo.Text));
                sp1[5] = new SqlParameter("@Plant", Convert.ToString(ddlplant.Text));
                sp1[6] = new SqlParameter("@user", Convert.ToString(Request.Cookies["TTSUser"].Value));

                int result = daCOTS.ExecuteNonQuery_SP("sp_ins_orderapproval_Department", sp1);
                if (result > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "SaveSuccess", "alert('Record Saved Succesffully');", true);
                    tbApproval.Visible = true;
                    tbSelection.Visible = false;
                    Bind_OrderItemList();
                    tbSelection.Visible = false;
                }
                txtComments.Text = "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("DOMESTIC REVISE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}