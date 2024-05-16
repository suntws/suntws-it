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

namespace TTS
{
    public partial class expprodapprove : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["prod_approval_mmn"].ToString() == "True" || dtUser.Rows[0]["prod_approval_sltl"].ToString() == "True"
                            || dtUser.Rows[0]["prod_approval_sitl"].ToString() == "True" || dtUser.Rows[0]["prod_approval_pdk"].ToString() == "True" || dtUser.Rows[0]["prod_approval_sltl"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "")
                            {
                                lblPageHead.Text += Utilities.Decrypt(Request["pid"].ToString());

                                SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) };
                                DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ProductionRequest_Approve_PendingList", spSel, DataAccess.Return_Type.DataTable);
                                if (dtData.Rows.Count > 0)
                                {
                                    gv_ApprovePendingOrders.DataSource = dtData;
                                    gv_ApprovePendingOrders.DataBind();
                                }
                                else
                                    lblErrMsgcontent.Text = "NO RECORDS";
                            }
                        }
                    }
                }
                else
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkApproveOrder_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnOrderID.Value = ((HiddenField)clickedRow.FindControl("hdnOID")).Value;

                lblSelectedCustomerName.Text = clickedRow.Cells[0].Text;
                lblSelectedOrderRefNo.Text = clickedRow.Cells[1].Text;
                lblSelectedWorkOrderNo.Text = clickedRow.Cells[2].Text;

                SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@O_ID", hdnOrderID.Value), new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) };
                DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ProductionRequest_Approve_Pending_Items", spSel, DataAccess.Return_Type.DataTable);
                if (dtItems.Rows.Count > 0)
                {
                    gv_ReqOrderItems.DataSource = dtItems;
                    gv_ReqOrderItems.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Sub_OrderItems');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnApproveSave_click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                foreach (GridViewRow gRow in gv_ReqOrderItems.Rows)
                {
                    SqlParameter[] spUpd = new SqlParameter[] { 
                        new SqlParameter("@ApproveComment", ((TextBox)gRow.FindControl("txtApproveComments")).Text), 
                        new SqlParameter("@ReqID", ((HiddenField)gRow.FindControl("hdnReqID")).Value), 
                        new SqlParameter("@ApproveBy", Request.Cookies["TTSUser"].Value) 
                    };
                    resp += daCOTS.ExecuteNonQuery_SP("sp_upd_ItemNewProductionRequest_ApproveStatus", spUpd);
                }

                if (resp == (gv_ReqOrderItems.Rows.Count * 2))
                {
                    resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOrderID.Value), "4", txt_Comments.Text, Request.Cookies["TTSUser"].Value);
                    if (resp > 0)
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