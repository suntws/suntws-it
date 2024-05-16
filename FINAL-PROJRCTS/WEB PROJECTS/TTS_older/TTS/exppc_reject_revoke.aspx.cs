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

namespace TTS
{
    public partial class exppc_reject_revoke : System.Web.UI.Page
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
                                    " - EXPORT " : " - DOMESTIC ") + "STENCIL REVOKING LIST";
                                SqlParameter[] sp1 = new SqlParameter[] { 
                                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                                    new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())) 
                                };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stencil_revoke_ppc_rejected", sp1, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gv_RevokeOrders.DataSource = dt;
                                    gv_RevokeOrders.DataBind();
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
        protected void lnkRevokeOrderSelection_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblSelectedCustomerName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblSelectedOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
                hdnStatus.Value = ((HiddenField)clickedRow.FindControl("hdnOrderStatus")).Value;
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;
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
                gv_RevokeItems.DataSource = null;
                gv_RevokeItems.DataBind();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stencil_earmark_verify", sp, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gv_RevokeItems.DataSource = dtItemList;
                    gv_RevokeItems.DataBind();

                    gv_RevokeItems.FooterRow.Cells[6].Text = "Total";
                    gv_RevokeItems.FooterRow.Cells[7].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();

                    gv_RevokeItems.Columns[8].Visible = false;
                    foreach (DataRow row1 in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gv_RevokeItems.Columns[8].Visible = true;
                        gv_RevokeItems.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                    }
                    gv_RevokeItems.FooterRow.Cells[9].Text = dtItemList.Compute("Sum(totEarmarkqty)", "").ToString();
                    gv_RevokeItems.FooterRow.Cells[10].Text = dtItemList.Compute("Sum(rejectqty)", "").ToString();
                }
                btnRevokeUpdate.Style.Add("display", "none");
                btnMoveStatus.Style.Add("display", "none");
                lblRevokeMsg.Text = "";
                gv_RevokingList.DataSource = null;
                gv_RevokingList.DataBind();

                if (hdnStatus.Value == "37")
                {
                    lblRevokeMsg.Text = "Kindly click the <b>SAVE</b> button for revoke";
                    btnRevokeUpdate.Style.Add("display", "block");
                    SqlParameter[] spSel = new SqlParameter[]{
                        new SqlParameter("@O_ID", hdnOID.Value),
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()))
                    };
                    DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Earmark_PdiRejectRevokeList", spSel, DataAccess.Return_Type.DataTable);
                    if (dtList.Rows.Count > 0)
                    {
                        gv_RevokingList.DataSource = dtList;
                        gv_RevokingList.DataBind();
                        lblRevokeMsg.Text = "Kindly click the <b>SAVE</b> button for revoke";
                        btnRevokeUpdate.Style.Add("display", "block");
                    }
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_SubOrderItems');", true);
                if (gv_RevokeItems.FooterRow.Cells[10].Text == "0")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showCompleteDiv", "gotoPreviewDiv('btnMoveStatus');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkQcRevokeItem_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;

                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@O_ID", hdnOID.Value),
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())),
                    new SqlParameter("@processid", ((HiddenField)clickedRow.FindControl("hdnProcessID")).Value),
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                };
                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Earmark_RejectRevokeList", sp, DataAccess.Return_Type.DataTable);

                lblRevokeMsg.Text = "Kindly inform to stencil assigned user for revoke.";
                btnRevokeUpdate.Style.Add("display", "none");
                btnMoveStatus.Style.Add("display", "none");
                if (dtList.Rows.Count > 0)
                {
                    gv_RevokingList.DataSource = dtList;
                    gv_RevokingList.DataBind();
                    foreach (DataRow row in dtList.Select("ChkEnable='true'"))
                    {
                        lblRevokeMsg.Text = "Kindly click the <b>SAVE</b> button for revoke";
                        btnRevokeUpdate.Style.Add("display", "block");
                        break;
                    }
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_SubOrderItems');", true);
                if (gv_RevokeItems.FooterRow.Cells[10].Text == "0")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showCompleteDiv", "gotoPreviewDiv('btnMoveStatus');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnRevokeUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_StencilNo = new DataTable();
                dt_StencilNo.Columns.Add("stencilno", typeof(string));
                dt_StencilNo.Columns.Add("revokecomments", typeof(string));
                foreach (GridViewRow Row in gv_RevokingList.Rows)
                {
                    CheckBox chk = Row.FindControl("chk_select") as CheckBox;
                    if (chk.Checked && !chk.Enabled)
                        dt_StencilNo.Rows.Add(Row.Cells[6].Text, Row.Cells[10].Text);
                }
                if (dt_StencilNo.Rows.Count > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Data_Earmark_Revoke", dt_StencilNo),
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())),
                        new SqlParameter("@RevokeBy", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@O_ID", hdnOID.Value),
                        new SqlParameter("@O_ItemID", ((HiddenField)gv_RevokingList.Rows[0].FindControl("hdnO_ItemID")).Value)
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_Earmark_Revoke_Data", sp);
                    if (resp > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Rejected stencil revoked successfully');", true);
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
                string strstatusid = hdnStatus.Value == "36" ? "24" : "19";
                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), strstatusid, (hdnStatus.Value == "36" ? "QC" : "PDI") +
                    " REJECTED STECNIL REVOKED BY ASSIGNED USERS", Request.Cookies["TTSUser"].Value);
                if (resp > 0)
                {
                    if (strstatusid == "24")
                    {
                        SqlParameter[] sp = new SqlParameter[] { 
                            new SqlParameter("@O_ID", hdnOID.Value), 
                            new SqlParameter("@requestmail", Request.Cookies["TTSUserEmail"].Value), 
                            new SqlParameter("@requestby", Request.Cookies["TTSUser"].Value), 
                            new SqlParameter("@processtype", "ASSIGN STENCIL") 
                        };
                        daCOTS.ExecuteNonQuery_SP("sp_ins_excel_prepare_history", sp);
                    }
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}