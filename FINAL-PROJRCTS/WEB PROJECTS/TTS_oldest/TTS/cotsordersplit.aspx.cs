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
using System.IO;

namespace TTS
{
    public partial class cotsordersplit : System.Web.UI.Page
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
                            (dtUser.Rows[0]["dom_ordersplit"].ToString() == "True" || dtUser.Rows[0]["exp_ordersplit"].ToString() == "True"))
                        {
                            lblTitlepage.Text = "ORDER SPLIT " + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ? " - EXPORT " : " - DOMESTIC ");

                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@QString", Utilities.Decrypt(Request["fid"].ToString())) };
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_processing_orders", sp, DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvorderlist.DataSource = dtorderlist;
                                gvorderlist.DataBind();
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
        protected void lnkSplitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblStausOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
                hdnStatus.Value = ((Label)clickedRow.FindControl("lblStatusText")).Text;
                lblCustName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                hdnCurrency.Value = ((HiddenField)clickedRow.FindControl("hdnUserCurrency")).Value;
                hdnStatusOrderDate.Value = ((Label)clickedRow.FindControl("lblOrderDate")).Text;
                hdnOrderPlant.Value = ((Label)clickedRow.FindControl("lblPlant")).Text;
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;

                Bind_OrderItem();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
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
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_orderitemlist_for_split", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvSplitOrderQty.DataSource = dtItemList;
                    gvSplitOrderQty.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow", "showTotalValue();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSplitOrder_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp3 = new SqlParameter[6];
                sp3[0] = new SqlParameter("@mainorderid", lblStausOrderRefNo.Text);
                sp3[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp3[2] = new SqlParameter("@orderid1", lblStausOrderRefNo.Text + "_PART-A");
                sp3[3] = new SqlParameter("@orderid2", lblStausOrderRefNo.Text + "_PART-B");
                sp3[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp3[5] = new SqlParameter("@Plant", hdnOrderPlant.Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_edit_masterorder_and_ins_Temp_OrderDetails", sp3);

                foreach (GridViewRow row in gvSplitOrderQty.Rows)
                {
                    HiddenField hdnItemID = row.FindControl("hdnItemID") as HiddenField;
                    Label lblQty = row.FindControl("lblQty") as Label;
                    TextBox txtPart1Qty = row.FindControl("txtPart1Qty") as TextBox;
                    Label lblPart2Qty = row.FindControl("lblPart2Qty") as Label;

                    SqlParameter[] sp1 = new SqlParameter[8];
                    sp1[0] = new SqlParameter("@mainorderid", lblStausOrderRefNo.Text);
                    sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    sp1[2] = new SqlParameter("@orderid1", lblStausOrderRefNo.Text + "_PART-A");
                    sp1[3] = new SqlParameter("@orderid2", lblStausOrderRefNo.Text + "_PART-B");
                    sp1[4] = new SqlParameter("@order1Qty", Convert.ToInt32(txtPart1Qty.Text));
                    sp1[5] = new SqlParameter("@order2Qty", Convert.ToInt32(Convert.ToInt32(lblQty.Text) - Convert.ToInt32(txtPart1Qty.Text)));
                    sp1[6] = new SqlParameter("@O_ItemID", hdnItemID.Value);
                    sp1[7] = new SqlParameter("@Plant", hdnOrderPlant.Value);
                    daCOTS.ExecuteNonQuery_SP("sp_split_ins_edit_OrderItemList", sp1);
                }

                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                daCOTS.ExecuteNonQuery_SP("sp_ins_Temp_OrderItemList", sp2);

                if (resp > 0)
                    Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}