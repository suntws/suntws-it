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
    public partial class cotspricechange : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_pricechange"].ToString() == "True")
                        {
                            if (Request.Cookies["TTSUser"].Value.ToLower() == "sreekanth" || Request.Cookies["TTSUser"].Value.ToLower() == "thamarai selvi")
                                hdnUserName.Value = "1";
                            else
                                hdnUserName.Value = "0";
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AllQuoteDiscount", DataAccess.Return_Type.DataTable);
                            ViewState["DiscStru"] = dt;

                            DataTable dtCust = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pricechangeorders_customer", DataAccess.Return_Type.DataTable);
                            if (dtCust.Rows.Count > 0)
                            {
                                ddlCotsCustName.DataSource = dtCust;
                                ddlCotsCustName.DataTextField = "custfullname";
                                ddlCotsCustName.DataValueField = "ID";
                                ddlCotsCustName.DataBind();
                                ddlCotsCustName.Items.Insert(0, "CHOOSE");

                                DataTable dtRates = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_DistinctRatesID", DataAccess.Return_Type.DataTable);
                                if (dtRates.Rows.Count > 0)
                                {
                                    ddlPriceChangeRatesID.DataSource = dtRates;
                                    ddlPriceChangeRatesID.DataTextField = "RatesID";
                                    ddlPriceChangeRatesID.DataValueField = "RatesID";
                                    ddlPriceChangeRatesID.DataBind();
                                    if (dtRates.Rows.Count > 1)
                                        ddlPriceChangeRatesID.Items.Insert(0, "CHOOSE");
                                }
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
        protected void ddlCotsCustName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@ID", ddlCotsCustName.SelectedItem.Value) };
                DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pricechangeorders_CustWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtorderlist.Rows.Count > 0)
                {
                    gvPriceChangeOrderList.DataSource = dtorderlist;
                    gvPriceChangeOrderList.DataBind();
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
                hdnPlant.Value = row.Cells[6].Text;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                if (lblStausOrderRefNo.Text != "" && hdnCustCode.Value != "")
                {
                    Bind_PriceChangeOrderItem();
                    DataTable dt = ViewState["DiscStru"] as DataTable;
                    foreach (DataRow dRow in dt.Select("Grade='" + hdnOrderGrade.Value + "' and CustType='" + ((HiddenField)row.FindControl("hdnCustCategory")).Value + "'"))
                    {
                        if (Request.Cookies["TTSUserType"].Value.ToLower() == "admin" || Request.Cookies["TTSUserType"].Value.ToLower() == "support" || Request.Cookies["TTSUserType"].Value.ToLower() == "")
                            hdnMaxDiscount.Value = dRow["ManagerPer"].ToString() == "" ? "0" : dRow["ManagerPer"].ToString();
                        else
                            hdnMaxDiscount.Value = dRow[Request.Cookies["TTSUserType"].Value + "Per"].ToString() == "" ? "0" : dRow[Request.Cookies["TTSUserType"].Value + "Per"].ToString();
                    }
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
                    hdnOrderGrade.Value = dtIns.Rows[0]["grade"].ToString();
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
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@custcode", hdnCustCode.Value), new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pricechange_orderitemlist", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvRevisePriceList.DataSource = dtItemList;
                    gvRevisePriceList.DataBind();

                    Bind_OrderInstruction();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptShow", "divTotPriceShow();", true);

                    gvRevisePriceList.Columns[12].Visible = false;
                    gvRevisePriceList.Columns[13].Visible = false;
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvRevisePriceList.Columns[12].Visible = true;
                        gvRevisePriceList.Columns[13].Visible = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Sel_CurValue()
        {
            SqlParameter[] sp1 = new SqlParameter[2];
            sp1[0] = new SqlParameter("@RatesID", ddlPriceChangeRatesID.SelectedItem.Text);
            sp1[1] = new SqlParameter("@Cur", "INR");
            hdnCurVal.Value = daTTS.ExecuteScalar_SP("Sp_Sel_CurVal_RatesIDWise", sp1).ToString();
        }
        protected void gvRevisePriceList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                if (ddlPriceChangeRatesID.SelectedItem.Text != "" && ddlPriceChangeRatesID.SelectedItem.Text != "CHOOSE")
                {
                    gvRevisePriceList.EditIndex = e.NewEditIndex;
                    GridViewRow row = gvRevisePriceList.Rows[e.NewEditIndex];
                    Label lblProcessid = row.FindControl("lblProcessid") as Label;

                    if (lblProcessid.Text != "" && ddlPriceChangeRatesID.SelectedItem.Text != "")
                    {
                        SqlParameter[] sp1 = new SqlParameter[2];
                        sp1[0] = new SqlParameter("@ProcessID", lblProcessid.Text);
                        sp1[1] = new SqlParameter("@RatesID", ddlPriceChangeRatesID.SelectedItem.Text);

                        DataTable dtCostVal = (DataTable)daTTS.ExecuteReader_SP("sp_sel_processid_CostValues", sp1, DataAccess.Return_Type.DataTable);
                        if (dtCostVal.Rows.Count == 1)
                        {
                            string strVal = string.Empty;
                            Sel_CurValue();
                            bool boolBeadBand = dtCostVal.Rows[0]["beadband"].ToString() == "Yes" ? true : false;
                            decimal decTypeCost = Convert.ToDecimal(dtCostVal.Rows[0]["Typecost"].ToString());
                            decimal decSizeCost = Convert.ToDecimal(dtCostVal.Rows[0]["SizeVal"].ToString());
                            decimal decBeadsQty = Convert.ToDecimal(dtCostVal.Rows[0]["Beads"].ToString());
                            decimal decFinishedWt = Convert.ToDecimal(dtCostVal.Rows[0]["Finished"].ToString());

                            hdnRMCBCostValue.Value = boolBeadBand + "~" + decTypeCost.ToString("0.00") + "~" + decSizeCost.ToString("0.00") + "~" + decBeadsQty + "~" + decFinishedWt + "~" + Convert.ToDecimal(hdnCurVal.Value);
                        }
                        Bind_PriceChangeOrderItem();
                    }
                }
                else
                {
                    e.Cancel = true;
                    gvRevisePriceList.EditIndex = -1;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptAlert", "alert('Please Choose Rates-ID');", true);
                    Bind_PriceChangeOrderItem();
                }
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

                TextBox txtDiscPrice = row.FindControl("txtDiscPer") as TextBox;
                HiddenField hdnCurrentDisc = row.FindControl("hdnCurrentDisc") as HiddenField;

                TextBox txtRimChangePrice = row.FindControl("txtRimChangePrice") as TextBox;
                HiddenField hdnRimCurrentPrice = row.FindControl("hdnRimCurrentPrice") as HiddenField;

                HiddenField hdnItemID = row.FindControl("hdnItemID") as HiddenField;

                if (lblProcessid.Text != "" && txtChangePrice.Text != "" && Convert.ToDecimal(txtChangePrice.Text) > 0)
                {
                    if (txtChangePrice.Text != hdnCurrentPrice.Value || txtDiscPrice.Text != hdnCurrentDisc.Value)
                    {
                        SqlParameter[] sp1 = new SqlParameter[]{
                            new SqlParameter("@O_ID", hdnOID.Value), 
                            new SqlParameter("@processid", lblProcessid.Text),
                            new SqlParameter("@unitprice", Convert.ToDecimal(txtChangePrice.Text)),
                            new SqlParameter("@discprice", Convert.ToDecimal(txtDiscPrice.Text)),
                            new SqlParameter("OItemID", hdnItemID.Value)
                        };
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_Change_OrderItemList_unitprice", sp1);
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

                            SqlParameter[] sp3 = new SqlParameter[7];
                            sp3[0] = new SqlParameter("@O_ID", hdnOID.Value);
                            sp3[1] = new SqlParameter("@RatesID", ddlPriceChangeRatesID.SelectedItem.Text);
                            sp3[2] = new SqlParameter("@Arv", hdnArv.Value != "" ? Convert.ToDecimal(hdnArv.Value) : 0);
                            sp3[3] = new SqlParameter("@Rmc", hdnRmc.Value != "" ? Convert.ToDecimal(hdnRmc.Value) : 0);
                            sp3[4] = new SqlParameter("@Rmcb", hdnRmcb.Value != "" ? Convert.ToDecimal(hdnRmcb.Value) : 0);
                            sp3[5] = new SqlParameter("@ProcessID", lblProcessid.Text);
                            sp3[6] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);

                            daCOTS.ExecuteNonQuery_SP("sp_ins_OrderPriceChangeArvRmc", sp3);
                        }
                    }
                    if (lblProcessid.Text != "" && txtRimChangePrice.Text != "" && Convert.ToDecimal(txtRimChangePrice.Text) > 0 && txtRimChangePrice.Text != hdnRimCurrentPrice.Value)
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@O_ID", hdnOID.Value), 
                            new SqlParameter("@processid", lblProcessid.Text), 
                            new SqlParameter("@Rimunitprice", Convert.ToDecimal(txtRimChangePrice.Text)),
                            new SqlParameter("OItemID", hdnItemID.Value)
                        };
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_Change_OrderItemList_AssyRimPrice", sp1);
                        if (resp > 0)
                        {
                            SqlParameter[] sp2 = new SqlParameter[]{
                                new SqlParameter("@O_ID", hdnOID.Value),
                                new SqlParameter("@Preview", hdnRimCurrentPrice.Value),
                                new SqlParameter("@Revise", txtRimChangePrice.Text),
                                new SqlParameter("@ReviseType", "Price Revised"),
                                new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                                new SqlParameter("@ProcessID", lblProcessid.Text)
                            };
                            daCOTS.ExecuteNonQuery_SP("sp_ins_OrderRevisedHistory", sp2);
                        }
                    }
                }
                gvRevisePriceList.EditIndex = -1;
                Bind_PriceChangeOrderItem();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('divPriceChange').style.display='block';", true);
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
                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), "1", txtPriceChangecomment.Text.Replace("\r\n", "~"),
                                    Request.Cookies["TTSUser"].Value);
                if (resp > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                    daCOTS.ExecuteNonQuery_SP("sp_moved_initial", sp);
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