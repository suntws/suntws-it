using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;

namespace TTS
{
    public partial class exppricechange : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_pricechange"].ToString() == "True")
                        {
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_pricechange_orders", DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvPriceChangeOrderList.DataSource = dtorderlist;
                                gvPriceChangeOrderList.DataBind();
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
        protected void gvPriceChangeOrderList_RowChoose(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gvPriceChangeOrderList.Rows[e.RowIndex];
                hdnCustCode.Value = ((HiddenField)row.FindControl("hdnCotsCustCode")).Value;
                lblCustName.Text = ((Label)row.FindControl("lblStatusCustName")).Text;
                lblStausOrderRefNo.Text = ((Label)row.FindControl("lblOrderRefNo")).Text;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                if (lblStausOrderRefNo.Text != "" && hdnCustCode.Value != "")
                {
                    Bind_OrderInstruction();
                    Bind_PriceChangeOrderItem();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderInstruction()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtIns = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Instruction_Request", sp1, DataAccess.Return_Type.DataTable);
                if (dtIns.Rows.Count > 0)
                {
                    txtOrderSplIns.Text = dtIns.Rows[0]["SplIns"].ToString().Replace("~", "\r\n");
                    txtOrdersplReq.Text = dtIns.Rows[0]["SpecialRequset"].ToString().Replace("~", "\r\n");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_PriceChangeOrderItem()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pricechange_orderitemlist_exp", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvRevisePriceList.DataSource = dtItemList;
                    gvRevisePriceList.DataBind();

                    gvRevisePriceList.Columns[10].Visible = false;
                    gvRevisePriceList.Columns[11].Visible = false;
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvRevisePriceList.Columns[10].Visible = true;
                        gvRevisePriceList.Columns[11].Visible = true;
                        break;
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
                    if (hdnPriceChangeStatus.Value == "1")
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('divPriceChange').style.display='block';", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvRevisePriceList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvRevisePriceList.EditIndex = e.NewEditIndex;
                GridViewRow row = gvRevisePriceList.Rows[e.NewEditIndex];
                Bind_PriceChangeOrderItem();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvRevisePriceList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvRevisePriceList.Rows[e.RowIndex];
                Label lblProcessid = row.FindControl("lblProcessid") as Label;
                TextBox txtChangePrice = row.FindControl("txtChangePrice") as TextBox;
                HiddenField hdnCurrentPrice = row.FindControl("hdnCurrentPrice") as HiddenField;
                TextBox txtRimChangePrice = row.FindControl("txtRimChangePrice") as TextBox;
                HiddenField hdnRimCurrentPrice = row.FindControl("hdnRimCurrentPrice") as HiddenField;
                Label lblEdcNo = row.FindControl("lblEdcNo") as Label;

                if (lblProcessid.Text != "" && txtChangePrice.Text != "" && Convert.ToDecimal(txtChangePrice.Text) > 0)
                {
                    if (txtChangePrice.Text != hdnCurrentPrice.Value)
                    {
                        SqlParameter[] sp1 = new SqlParameter[]{
                            new SqlParameter("@O_ID", hdnOID.Value),
                            new SqlParameter("@processid", lblProcessid.Text),
                            new SqlParameter("@unitprice", Convert.ToDecimal(txtChangePrice.Text)),
                            new SqlParameter("@discprice", Convert.ToDecimal("0.00")),
                            new SqlParameter("@EdcNo", lblEdcNo.Text)
                        };
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_Change_OrderItemList_unitprice_exp", sp1);
                        if (resp > 0)
                        {
                            SqlParameter[] sp2 = new SqlParameter[]{
                                new SqlParameter("@O_ID", hdnOID.Value),
                                new SqlParameter("@Preview", hdnCurrentPrice.Value),
                                new SqlParameter("@Revise", txtChangePrice.Text),
                                new SqlParameter("@ReviseType", "Price Revised"),
                                new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                                new SqlParameter("@ProcessID", lblProcessid.Text)
                            };
                            daCOTS.ExecuteNonQuery_SP("sp_ins_OrderRevisedHistory", sp2);
                        }
                    }
                    if (lblProcessid.Text != "" && txtRimChangePrice.Text != "" && Convert.ToDecimal(txtRimChangePrice.Text) > 0 && txtRimChangePrice.Text != hdnRimCurrentPrice.Value)
                    {
                        SqlParameter[] sp1 = new SqlParameter[]{
                            new SqlParameter("@O_ID", hdnOID.Value),
                            new SqlParameter("@processid", lblProcessid.Text),
                            new SqlParameter("@Rimunitprice", Convert.ToDecimal(txtRimChangePrice.Text)),
                            new SqlParameter("@EdcNo", lblEdcNo.Text)
                        };
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_Change_OrderItemList_AssyRimPrice_exp", sp1);
                        if (resp > 0)
                        {
                            SqlParameter[] sp2 = new SqlParameter[]{
                                new SqlParameter("@O_ID", hdnOID.Value),
                                new SqlParameter("@Preview", hdnRimCurrentPrice.Value),
                                new SqlParameter("@Revise", txtRimChangePrice.Text),
                                new SqlParameter("@ReviseType", "Rim Price Revised"),
                                new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                                new SqlParameter("@ProcessID", lblProcessid.Text)
                            };
                            daCOTS.ExecuteNonQuery_SP("sp_ins_OrderRevisedHistory", sp2);
                        }
                    }
                    hdnPriceChangeStatus.Value = "1";
                }
                gvRevisePriceList.EditIndex = -1;
                Bind_PriceChangeOrderItem();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvRevisePriceList_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gvRevisePriceList.EditIndex = -1;
                Bind_PriceChangeOrderItem();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPriceChangeCompleted_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_exp_pricechange_completed", sp);
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}