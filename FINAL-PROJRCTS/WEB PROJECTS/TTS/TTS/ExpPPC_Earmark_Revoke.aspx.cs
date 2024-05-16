using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Diagnostics;

namespace TTS
{
    public partial class ExpPPC_Earmark_Revoke : System.Web.UI.Page
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
                            (dtUser.Rows[0]["exp_earmark_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_earmark_sltl"].ToString() == "True" || dtUser.Rows[0]["exp_earmark_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_earmark_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["dom_earmark_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_earmark_pdk"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "" && Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ") + "STENCIL ASSIGNED LIST";
                                SqlParameter[] sp1 = new SqlParameter[] { 
                                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                                    new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())),
                                    new SqlParameter("@userdepartment", Request.Cookies["TTSUserDepartment"].Value)
                                };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stencil_earmark_assigned_orders", sp1, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gv_EarmarkedOrders.DataSource = dt;
                                    gv_EarmarkedOrders.DataBind();
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
        protected void lnkEarmarkedOrderSelection_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblSelectedCustomerName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblSelectedOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;
                hdnStatusID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderStatus")).Value;
                bind_SelectOrder();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_SelectOrder()
        {
            try
            {
                gv_EarmarkRevokeItems.DataSource = null;
                gv_EarmarkRevokeItems.DataBind();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_orderitem_for_stencil_earmark_revoke", sp, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gv_EarmarkRevokeItems.DataSource = dtItemList;
                    gv_EarmarkRevokeItems.DataBind();

                    gv_EarmarkRevokeItems.FooterRow.Cells[6].Text = "Total";
                    gv_EarmarkRevokeItems.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    gv_EarmarkRevokeItems.FooterRow.Cells[7].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gv_EarmarkRevokeItems.FooterRow.Cells[9].Text = dtItemList.Compute("Sum(PartA)", "").ToString();
                    gv_EarmarkRevokeItems.FooterRow.Cells[10].Text = dtItemList.Compute("Sum(PartB)", "").ToString();
                    gv_EarmarkRevokeItems.FooterRow.Cells[11].Text = dtItemList.Compute("Sum(PartC)", "").ToString();
                    gv_EarmarkRevokeItems.FooterRow.Cells[12].Text = dtItemList.Compute("Sum(PartD)", "").ToString();
                    gv_EarmarkRevokeItems.FooterRow.Cells[13].Text = dtItemList.Compute("Sum(PartE)", "").ToString();
                    gv_EarmarkRevokeItems.FooterRow.Cells[14].Text = dtItemList.Compute("Sum(PartF)", "").ToString();

                    gv_EarmarkRevokeItems.Columns[8].Visible = false;
                    foreach (DataRow row1 in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gv_EarmarkRevokeItems.Columns[8].Visible = true;
                        gv_EarmarkRevokeItems.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Sub_OrderItems');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showCompleteDiv", "gotoPreviewDiv('div_earmarkcompleted');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkEarmarkRevokeItem_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@O_ID", hdnOID.Value),
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())),
                    new SqlParameter("@processid", ((HiddenField)clickedRow.FindControl("hdnProcessID")).Value),
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@O_ItemID", ((HiddenField)clickedRow.FindControl("hdnOrder_ItemID")).Value)
                };
                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_EarmarkedList", sp, DataAccess.Return_Type.DataTable);

                if (dtList.Rows.Count > 0)
                {
                    gv_EarmarkedStock.DataSource = dtList;
                    gv_EarmarkedStock.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showStockGV", "gotoPreviewDiv('div_StockCtrls');", true);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Sub_OrderItems');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showCompleteDiv", "gotoPreviewDiv('div_earmarkcompleted');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnEarmarkRevoke_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_StencilNo = new DataTable();
                dt_StencilNo.Columns.Add("stencilno", typeof(string));
                dt_StencilNo.Columns.Add("revokecomments", typeof(string));
                foreach (GridViewRow Row in gv_EarmarkedStock.Rows)
                {
                    CheckBox chk = Row.FindControl("chk_selectQty") as CheckBox;
                    if (chk.Checked && chk.Enabled)
                        dt_StencilNo.Rows.Add(Row.Cells[8].Text, txtRevokeCommets.Text);
                }
                if (dt_StencilNo.Rows.Count > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Data_Earmark_Revoke", dt_StencilNo),
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())),
                        new SqlParameter("@RevokeBy", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@O_ID", hdnOID.Value),
                        new SqlParameter("@O_ItemID", ((HiddenField)gv_EarmarkedStock.Rows[0].FindControl("hdnO_ItemID")).Value)
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_Earmark_Revoke_Data", sp);
                    if (resp > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Assigned stencil revoke successfully');", true);
                        bind_SelectOrder();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnMoveStatus_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), hdnStatusID.Value == "34" ? "41" : hdnStatusID.Value == "41" ? "24" :
                    hdnStatusID.Value, txtEarmarkCommetns.Text, Request.Cookies["TTSUser"].Value);
                if (resp > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@requestmail", Request.Cookies["TTSUserEmail"].Value), 
                        new SqlParameter("@requestby", Request.Cookies["TTSUser"].Value), 
                        new SqlParameter("@processtype", "ASSIGN STENCIL") 
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_ins_excel_prepare_history", sp);
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