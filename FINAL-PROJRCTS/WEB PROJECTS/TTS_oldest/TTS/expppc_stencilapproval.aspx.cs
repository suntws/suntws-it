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
using System.IO;

namespace TTS
{
    public partial class expppc_stencilapproval : System.Web.UI.Page
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
                            (dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True"
                            || dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk"].ToString() == "True"))
                        {
                            if ((Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "") &&
                                (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "") &&
                                (Request["oid"] != null && Utilities.Decrypt(Request["oid"].ToString()) != ""))
                            {
                                lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ") + "ASSINGED STENCIL APPROVAL FROM QC/PDI";

                                SqlParameter[] spID = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) };
                                DataTable dtID = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_customer_ID_Name", spID, DataAccess.Return_Type.DataTable);
                                if (dtID.Rows.Count > 0)
                                {
                                    hdnCustCode.Value = dtID.Rows[0]["ID"].ToString();
                                    lblSelectedOrderRefNo.Text = dtID.Rows[0]["OrderRefNo"].ToString();
                                    lblSelectedCustomerName.Text = dtID.Rows[0]["custfullname"].ToString();
                                    bind_SelectOrder();
                                }
                                lnkEarmarkExcel.Text = ""; lblText.Text = "";
                                string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                                if (Directory.Exists(serverURL + "/stockearmark/" + hdnCustCode.Value + "/"))
                                {
                                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) };
                                    string strFileName = (string)daCOTS.ExecuteScalar_SP("sp_sel_assign_stencil_filename", sp);
                                    if (strFileName != "")
                                    {
                                        lnkEarmarkExcel.Text = strFileName;
                                        lblText.Text = "EXCEL FILE FOR ASSIGNED STENCIL LIST :  ";
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
        private void bind_SelectOrder()
        {
            try
            {
                gv_EarmarkedItems.DataSource = null;
                gv_EarmarkedItems.DataBind();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stencil_earmark_verify", sp, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gv_EarmarkedItems.DataSource = dtItemList;
                    gv_EarmarkedItems.DataBind();

                    gv_EarmarkedItems.FooterRow.Cells[6].Text = "Total";
                    gv_EarmarkedItems.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    gv_EarmarkedItems.FooterRow.Cells[7].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();

                    gv_EarmarkedItems.Columns[8].Visible = false;
                    foreach (DataRow row1 in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gv_EarmarkedItems.Columns[8].Visible = true;
                        gv_EarmarkedItems.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                    }

                    gv_EarmarkedItems.FooterRow.Cells[9].Text = dtItemList.Compute("Sum(totEarmarkqty)", "").ToString();
                    gv_EarmarkedItems.FooterRow.Cells[10].Text = dtItemList.Compute("Sum(rejectqty)", "").ToString();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkEarmarkSencil_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            hdnSelectProcessID.Value = ((HiddenField)clickedRow.FindControl("hdnProcessID")).Value;
            hdnO_ItemID.Value = ((HiddenField)clickedRow.FindControl("hdnOrder_ItemID")).Value;
            Bind_EarmarkedList();
        }
        private void Bind_EarmarkedList()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())),
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                    new SqlParameter("@processid", hdnSelectProcessID.Value),
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@O_ItemID", hdnO_ItemID.Value)
                };
                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_EarmarkedList", sp, DataAccess.Return_Type.DataTable);
                if (dtList.Rows.Count > 0)
                {
                    gv_EarmarkedStock.DataSource = dtList;
                    gv_EarmarkedStock.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_EarmarkedStock_OnDataBound(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gv_EarmarkedStock.Rows)
                {
                    if (((TextBox)row.FindControl("txtCmt")).Text != "")
                        ((RadioButtonList)row.FindControl("rdoSelect")).SelectedIndex = 1;
                    else
                        ((RadioButtonList)row.FindControl("rdoSelect")).SelectedIndex = 0;
                }
                if (gv_EarmarkedItems.FooterRow.Cells[10].Text == "0")
                {
                    lblStatusMsg.Text = "If accept all the assigned stencil, Kindly move to PDI.";
                    hdnOrderStatus.Value = "19";
                }
                else
                {
                    lblStatusMsg.Text = "If complete the verification for assigned stencil, Kindly move to CRM/PPC for revoking the stencil as per your rejection.";
                    hdnOrderStatus.Value = "36";
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "bindval", "bind_existStatusVal();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showStockGV", "gotoPreviewDiv('div_StockCtrls');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_StencilNo = new DataTable();
                dt_StencilNo.Columns.Add("StencilNo", typeof(string));
                dt_StencilNo.Columns.Add("PPC_Status", typeof(string));
                foreach (GridViewRow row in gv_EarmarkedStock.Rows)
                {
                    dt_StencilNo.Rows.Add(row.Cells[8].Text, ((TextBox)row.FindControl("txtCmt")).Text);
                }
                if (dt_StencilNo.Rows.Count > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Data_PPCApproval", dt_StencilNo),
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())),
                        new SqlParameter("@RejectedBY", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@O_Item_ID", hdnO_ItemID.Value)
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_PPC_Approval_Status", sp);
                    if (resp > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Assinged stencil verification updated successfully');", true);
                        bind_SelectOrder();
                        Bind_EarmarkedList();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Bind_EarmarkedList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnMoveToRfdStatus_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())), hdnOrderStatus.Value,
                    txtEarmarkVerifyCommetns.Text, Request.Cookies["TTSUser"].Value);
                if (resp > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
                        new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value) 
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_upd_earmark_qc_verify", sp);
                    if (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() != "SLTL" && Utilities.Decrypt(Request["pid"].ToString()).ToUpper() != "SITL")
                        Response.Redirect("expppc_verification1.aspx?pid=" + Request["pid"].ToString() + "&fid=" + Request["fid"].ToString(), false);
                    else
                        Response.Redirect("expppc_verification.aspx?pid=" + Request["pid"].ToString() + "&fid=" + Request["fid"].ToString(), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkEarmarkExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                Response.WriteFile((Server.MapPath("~/").Replace("TTS", "pdfs")) + ("/stockearmark/" + hdnCustCode.Value + "/" + lnkTxt.Text));
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}