using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using System.IO;
namespace TTS
{
    public partial class stockorder_ppc : System.Web.UI.Page
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
                            (dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_ppc_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sitl"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_qcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "" && Request["fid"] != null &&
                                Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ");
                                SqlParameter[] sp1 = new SqlParameter[] { 
                                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                                    new SqlParameter("@Qtype", Utilities.Decrypt(Request["fid"].ToString())) 
                                };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stockorder_ppc", sp1, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gv_Orders.DataSource = dt;
                                    gv_Orders.DataBind();
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
        protected void lnkOrderSelection_click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
            Bind_SelectOrderDetails(clickedRow);
        }
        private void Bind_SelectOrderDetails(GridViewRow clickedRow)
        {
            try
            {
                lblSelectedCustomerName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblSelectedOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
                hdnSID.Value = ((HiddenField)clickedRow.FindControl("hdnStockOrderID")).Value;

                Bind_OrderItemList();

                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                string path2 = serverURL + "Stockorderfiles\\" + hdnCustCode.Value + "\\" + hdnSID.Value + ".pdf";
                FileInfo file2 = new FileInfo(path2);
                if (file2.Exists)
                    lnkStockWorkOrder.Text = hdnSID.Value + ".pdf";

                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Sub_OrderItems');", true);
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
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StockOrderId", Convert.ToInt32(hdnSID.Value)) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complete_Stockorderitem_list", sp, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gv_OrderItems.DataSource = dtItemList;
                    gv_OrderItems.DataBind();

                    gv_OrderItems.FooterRow.Cells[7].Text = "Total";
                    gv_OrderItems.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                    gv_OrderItems.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[10].Text = dtItemList.Compute("Sum(O_Newproduction)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[11].Text = Convert.ToDecimal(dtItemList.Compute("Sum(finishedwt)", "")).ToString();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_SaveRecords_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtPPC = new DataTable();
                dtPPC.Columns.Add(new DataColumn("processid", typeof(System.String)));
                dtPPC.Columns.Add(new DataColumn("AvlQty", typeof(System.Int32)));
                dtPPC.Columns.Add(new DataColumn("Newproduction", typeof(System.Int32)));
                dtPPC.Columns.Add(new DataColumn("ItemID", typeof(System.Int32)));
                foreach (GridViewRow row in gv_OrderItems.Rows)
                {
                    TextBox txt_AvalQty = (TextBox)row.FindControl("txt_AvalQty");
                    HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
                    Label lblQty = row.FindControl("lblQty") as Label;
                    int s1 = Convert.ToInt32(Convert.ToInt32(lblQty.Text) - Convert.ToInt32(txt_AvalQty.Text));
                    dtPPC.Rows.Add(row.Cells[7].Text, txt_AvalQty.Text, s1, hdnItemID.Value);
                }
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@SID", hdnSID.Value), 
                    new SqlParameter("@stockItemList_AvalQty_dt", dtPPC)
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_Upd_stockorder_PPC_AvlQty", sp);
                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "SaveSuccess", "alert('Record Saved Succesffully');", true);
                    gv_Orders.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                    Bind_SelectOrderDetails(gv_Orders.SelectedRow);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showFinal", "gotoPreviewDiv('div_StatusChange');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_StatusChange_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@StockOrderId", Convert.ToInt32(hdnSID.Value)), 
                    new SqlParameter("@StockOrderStatus", "5") 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_stockOrdermaster_Status", sp);
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkStockWorkOrder_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string serverUrl = Server.MapPath("~/Stockorderfiles/" + hdnCustCode.Value + "/").Replace("TTS", "pdfs");
                string path = serverUrl + "/" + lnkTxt.Text;

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        
    }
}