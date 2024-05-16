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
    public partial class earmark : System.Web.UI.Page
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
                            (dtUser.Rows[0]["exp_earmark_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_earmark_sltl"].ToString() == "True"
                            || dtUser.Rows[0]["exp_earmark_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_earmark_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["dom_earmark_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_earmark_pdk"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "" && Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ") + "ORDER STENCIL EARMARK";
                                SqlParameter[] sp1 = new SqlParameter[] { 
                                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                                    new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())),
                                    new SqlParameter("@userdepartment", Request.Cookies["TTSUserDepartment"].Value)
                                };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_stencil_earmark_orders", sp1, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gv_EarmarkOrders.DataSource = dt;
                                    gv_EarmarkOrders.DataBind();

                                    ScriptManager.RegisterStartupScript(Page, GetType(), "showOrderGV", "gotoPreviewDiv('div_EarmarkOrders');", true);
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
                    ScriptManager.RegisterStartupScript(Page, GetType(), "hidechkbox", "ChkBox_hide();", true);
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
        protected void lnkEarmarkOrderSelection_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblSelectedCustomerName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblSelectedOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;
                hdnStatusID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderStatus")).Value;
                hdnSelectItem.Value = "";

                Bind_OrderItemList();
                ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv", "gotoPreviewDiv('div_earkmark_OrderItems');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkSkipEarmark_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnSelectItem.Value != "")
                    gv_OrderItems.Rows[Convert.ToInt32(hdnSelectItem.Value)].BackColor = System.Drawing.Color.White;
                lblAvlStock.Text = "";
                lblSkipStencil.Text = "";
                gv_SkipStencil.DataSource = null;
                gv_SkipStencil.DataBind();

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), new SqlParameter("@O_ID", hdnOID.Value) };
                DataSet dtStockChk = (DataSet)daCOTS.ExecuteReader_SP("sp_chk_available_stock_for_assign_v1", sp, DataAccess.Return_Type.DataSet);
                if (dtStockChk != null && dtStockChk.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dtStockChk.Tables[0].Rows)
                    {
                        if (row["PartCount"].ToString() != "0")
                        {
                            lblAvlStock.Text += lblAvlStock.Text == "" ? "<u><i>ORDER ITEM BASED AVAILABLE A-GRADE QTY</i></u><br/>" : "";
                            lblAvlStock.Text += row["PartType"].ToString() + " : " + row["PartCount"].ToString() + "<br/>";
                        }
                    }
                    if (dtStockChk.Tables[0].Compute("Sum(PartCount)", "").ToString() != "0")
                    {
                        lblAvlStock.Text += "TOTAL  : " + dtStockChk.Tables[0].Compute("Sum(PartCount)", "").ToString() + "<br/>";
                        lblSkipStencil.Text = "SEE THE BELOW LIST FOR AVAILABLE STENCIL DATA";
                        gv_SkipStencil.DataSource = dtStockChk.Tables[1];
                        gv_SkipStencil.DataBind();
                    }
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv", "gotoPreviewDiv('div_earkmark_OrderItems');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSkipDiv", "gotoPreviewDiv('div_skipearmark');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_SkipEarmark_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), hdnStatusID.Value == "34" ? "41" : hdnStatusID.Value == "41" ? "24" :
                    hdnStatusID.Value, txtSkipEarmarkComments.Text, Request.Cookies["TTSUser"].Value);
                if (resp > 0)
                    Response.Redirect(Request.RawUrl.ToString(), false);
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
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value), new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_orderitem_for_stencil_earmark_v1", sp, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gv_OrderItems.DataSource = dtItemList;
                    gv_OrderItems.DataBind();

                    gv_OrderItems.FooterRow.Cells[6].Text = "TOTAL";

                    gv_OrderItems.FooterRow.Cells[7].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[7].Font.Size = 14;

                    gv_OrderItems.Columns[8].Visible = false;
                    foreach (DataRow row1 in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gv_OrderItems.Columns[8].Visible = true;
                        gv_OrderItems.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                    }

                    gv_OrderItems.FooterRow.Cells[9].Text = dtItemList.Compute("Sum(PartA)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[10].Text = dtItemList.Compute("Sum(PartB)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[11].Text = dtItemList.Compute("Sum(PartC)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[12].Text = dtItemList.Compute("Sum(PartD)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[13].Text = dtItemList.Compute("Sum(PartE)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[14].Text = dtItemList.Compute("Sum(PartF)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[15].Text = dtItemList.Compute("Sum(PartG)", "").ToString();

                    lnkSkipEarmark.Text = "";
                    if (gv_OrderItems.FooterRow.Cells[9].Text == "0" && gv_OrderItems.FooterRow.Cells[10].Text == "0" && gv_OrderItems.FooterRow.Cells[11].Text == "0" &&
                        gv_OrderItems.FooterRow.Cells[12].Text == "0" && gv_OrderItems.FooterRow.Cells[13].Text == "0" && gv_OrderItems.FooterRow.Cells[14].Text == "0")
                        lnkSkipEarmark.Text = "STENCIL ASSIGN PROCESS NOT REQUIRE, MOVE FOR FRESH PRODUCTION";
                    else
                    {
                        gv_OrderItems.FooterRow.Cells[16].Text = (Convert.ToInt32(gv_OrderItems.FooterRow.Cells[9].Text) + Convert.ToInt32(gv_OrderItems.FooterRow.Cells[10].Text) +
                            Convert.ToInt32(gv_OrderItems.FooterRow.Cells[11].Text) + Convert.ToInt32(gv_OrderItems.FooterRow.Cells[12].Text) +
                            Convert.ToInt32(gv_OrderItems.FooterRow.Cells[13].Text) + Convert.ToInt32(gv_OrderItems.FooterRow.Cells[14].Text)).ToString();
                        gv_OrderItems.FooterRow.Cells[16].Font.Size = 14;
                    }

                    if (hdnSelectItem.Value != "")
                        gv_OrderItems.Rows[Convert.ToInt32(hdnSelectItem.Value)].BackColor = System.Drawing.Color.Yellow;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_AvailableStock()
        {
            try
            {
                lblManuYearErrMsg.Text = "";
                lblYearMsg.Text = "";
                lblNextMsg.Text = "";
                lbl_MANUFACTUREYEAR.Text = "";
                lbl_QUALITYGRADE.Text = "";

                btnFindStencil.Text = "";
                btnFindStencil.Style.Add("display", "none");

                chkManufactureYear.DataSource = "";
                chkManufactureYear.DataBind();

                chkManufactureGrade.DataSource = "";
                chkManufactureGrade.DataBind();

                lblPlatform.Text = "";
                lblTyresize.Text = "";
                lblRim.Text = "";
                lblType.Text = "";
                lblBrand.Text = "";
                lblSidewall.Text = "";

                lblOrderQty.Text = "";
                lblEarmarkQty.Text = "";
                lblRequiredQty.Text = "";

                lblPartA.Text = "";
                lblPartB.Text = "";
                lblPartC.Text = "";
                lblPartD.Text = "";
                lblPartE.Text = "";
                lblPartF.Text = "";

                lnkPartA.Text = "";
                lnkPartB.Text = "";
                lnkPartC.Text = "";
                lnkPartD.Text = "";
                lnkPartE.Text = "";
                lnkPartF.Text = "";
                lblStockMsg.Text = "";

                SqlParameter[] spItem = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@processid", lblProcessID.Text), 
                    new SqlParameter("@assy", hdnAssyStatus.Value) 
                };
                DataTable dtItem = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Item_Deatils_ForEarmark", spItem, DataAccess.Return_Type.DataTable);
                if (dtItem.Rows.Count > 0)
                {
                    lblPlatform.Text = dtItem.Rows[0]["Config"].ToString();
                    lblTyresize.Text = dtItem.Rows[0]["tyresize"].ToString();
                    lblRim.Text = dtItem.Rows[0]["rimsize"].ToString();
                    lblType.Text = dtItem.Rows[0]["tyretype"].ToString();
                    lblBrand.Text = dtItem.Rows[0]["brand"].ToString();
                    lblSidewall.Text = dtItem.Rows[0]["sidewall"].ToString();

                    lblOrderQty.Text = dtItem.Rows[0]["itemqty"].ToString();
                    lblEarmarkQty.Text = dtItem.Rows[0]["totearmarkqty"].ToString();
                    lblRequiredQty.Text = dtItem.Rows[0]["requiredqty"].ToString();
                    lblProducedQty.Text = dtItem.Rows[0]["PartG"].ToString() != "0" ? "WORK ORDER BASED PRODUCED QTY: " + dtItem.Rows[0]["PartG"].ToString() : "";

                    lblPartA.Text = dtItem.Rows[0]["PartA"].ToString();
                    lblPartB.Text = dtItem.Rows[0]["PartB"].ToString();
                    lblPartC.Text = dtItem.Rows[0]["PartC"].ToString();
                    lblPartD.Text = dtItem.Rows[0]["PartD"].ToString();
                    lblPartE.Text = dtItem.Rows[0]["PartE"].ToString();
                    lblPartF.Text = dtItem.Rows[0]["PartF"].ToString();
                }

                SqlParameter[] spAvl = new SqlParameter[] { 
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@processid", lblProcessID.Text) 
                };
                DataSet dsAvl = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_available_stock_part_wise_v1", spAvl, DataAccess.Return_Type.DataSet);
                if (dsAvl != null && dsAvl.Tables.Count > 0)
                {
                    foreach (DataTable table in dsAvl.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            if (dr["PartType"].ToString() == "PART-A")
                                lnkPartA.Text = dr["PartCount"].ToString() != "0" ? dr["PartCount"].ToString() + " QTY AVAILABLE" : "";
                            else if (dr["PartType"].ToString() == "PART-B")
                                lnkPartB.Text = dr["PartCount"].ToString() != "0" ? dr["PartCount"].ToString() + " QTY AVAILABLE" : "";
                            else if (dr["PartType"].ToString() == "PART-C")
                                lnkPartC.Text = dr["PartCount"].ToString() != "0" ? dr["PartCount"].ToString() + " QTY AVAILABLE" : "";
                            else if (dr["PartType"].ToString() == "PART-D")
                                lnkPartD.Text = dr["PartCount"].ToString() != "0" ? dr["PartCount"].ToString() + " QTY AVAILABLE" : "";
                            else if (dr["PartType"].ToString() == "PART-E")
                                lnkPartE.Text = dr["PartCount"].ToString() != "0" ? dr["PartCount"].ToString() + " QTY AVAILABLE" : "";
                            else if (dr["PartType"].ToString() == "PART-F")
                                lnkPartF.Text = dr["PartCount"].ToString() != "0" ? dr["PartCount"].ToString() + " QTY AVAILABLE" : "";
                        }
                    }
                    if (lblStockMsg.Text != "")
                        lblStockMsg.Text += " STOCK NOT AVAILABLE";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_SelectOrder(string strPartType)
        {
            try
            {
                lblManuYearErrMsg.Text = "";
                lblYearMsg.Text = "";
                lblNextMsg.Text = "";
                lbl_MANUFACTUREYEAR.Text = "";
                lbl_QUALITYGRADE.Text = "";
                btnFindStencil.Text = "";
                btnFindStencil.Style.Add("display", "none");

                chkManufactureYear.DataSource = "";
                chkManufactureYear.DataBind();

                chkManufactureGrade.DataSource = "";
                chkManufactureGrade.DataBind();

                hdnPartType.Value = strPartType;
                if (hdnPartType.Value == "PART-A")
                    hdnPartMethod.Value = "GSA EXACT MATCH";
                else if (hdnPartType.Value == "PART-B")
                    hdnPartMethod.Value = "GSA EXACT WITH REBRAND";
                else if (hdnPartType.Value == "PART-C")
                    hdnPartMethod.Value = "GSA UPGRADE WITH REBRAND";
                else if (hdnPartType.Value == "PART-D")
                    hdnPartMethod.Value = "CURRENT STOCK EXACT MATCH";
                else if (hdnPartType.Value == "PART-E")
                    hdnPartMethod.Value = "CURRENT STOCK MATCH WITH REBRAND";
                else if (hdnPartType.Value == "PART-F")
                    hdnPartMethod.Value = "CURRENT STOCK UPGRADE WITH REBRAND";


                if ((Convert.ToInt32(lblOrderQty.Text) == Convert.ToInt32(lblEarmarkQty.Text)) && (Convert.ToInt32(lblRequiredQty.Text) == 0))
                    lblYearMsg.Text = "ASSIGNING COMPLETED FOR ABOVE SELECTED ORDER ITEM. SELECT NEXT ITEM.";
                else if ((Convert.ToInt32(lblOrderQty.Text) > Convert.ToInt32(lblEarmarkQty.Text)) && (Convert.ToInt32(lblRequiredQty.Text) > 0))
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@processid", lblProcessID.Text), 
                        new SqlParameter("@PartType", hdnPartType.Value) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stencil_mfy_processid_wise_v1", sp, DataAccess.Return_Type.DataTable);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataView dtDescView = new DataView(dt);
                        dtDescView.Sort = "MFY ASC";
                        DataTable distinctMFY = dtDescView.ToTable(true, "MFY", "MFY_Val");

                        DataTable dtMfy = new DataTable();
                        DataColumn col = new DataColumn("MFY", typeof(System.String));
                        dtMfy.Columns.Add(col);
                        col = new DataColumn("MFY_Val", typeof(System.String));
                        dtMfy.Columns.Add(col);

                        foreach (DataRow mRow in distinctMFY.Rows)
                        {
                            try
                            {
                                if (Convert.ToInt32(mRow["MFY"].ToString()) > 0 && Convert.ToInt32(mRow["MFY"].ToString()) <= DateTime.Now.Year)
                                    dtMfy.Rows.Add(mRow["MFY"].ToString(), mRow["MFY_Val"].ToString());
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (dtMfy.Rows.Count > 0)
                        {
                            chkManufactureYear.DataSource = dtMfy;
                            chkManufactureYear.DataTextField = "MFY";
                            chkManufactureYear.DataValueField = "MFY_Val";
                            chkManufactureYear.DataBind();

                            dtDescView.Sort = "Grade ASC";
                            DataTable distinctGrade = dtDescView.ToTable(true, "Grade");
                            chkManufactureGrade.DataSource = distinctGrade;
                            chkManufactureGrade.DataTextField = "Grade";
                            chkManufactureGrade.DataValueField = "Grade";
                            chkManufactureGrade.DataBind();

                            lbl_MANUFACTUREYEAR.Text = "MANUFACTURE YEAR";
                            lbl_QUALITYGRADE.Text = "QUALITY GRADE";
                            btnFindStencil.Text = "FIND STENCIL LIST";
                            btnFindStencil.Style.Add("display", "block");

                            string strDom = DateTime.Now.AddMonths(-48).ToString("MMM") + "-" + DateTime.Now.AddMonths(-48).ToString("yyyy");

                            if (hdnPartType.Value == "PART-D" || hdnPartType.Value == "PART-E" || hdnPartType.Value == "PART-F")
                                lblYearMsg.Text = "STOCK AVAILABLE FROM " + strDom.ToUpper() + " TO TILL DATE FOR " + hdnPartType.Value + " (" + hdnPartMethod.Value + ")";
                            else
                                lblYearMsg.Text = "STOCK AVAILABLE UPTO " + strDom.ToUpper() + " FOR " + hdnPartType.Value + " (" + hdnPartMethod.Value + ")";

                            foreach (ListItem item in chkManufactureYear.Items)
                            {
                                item.Selected = true;
                            }
                            if (chkManufactureGrade.Items[0].Text == "A" || chkManufactureGrade.Items[0].Text == "E")
                                chkManufactureGrade.Items[0].Selected = true;
                            if (chkManufactureGrade.Items[0].Selected == true && chkManufactureYear.Items[0].Selected == true)
                                btnFindStencil_Click(null, null);
                        }
                    }

                    if (hdnPartType.Value == "PART-F" && lblYearMsg.Text != "")
                        lblNextMsg.Text = "OR " + lblRequiredQty.Text + " QTY TO BE PRODUCED. SELECT NEXT ITEM FOR STENCIL ASSIGN";
                    else if (hdnPartType.Value == "PART-F" && lblYearMsg.Text == "")
                        lblManuYearErrMsg.Text = "STOCK NOT AVAILABLE FOR " + hdnPartType.Value + ". " + lblRequiredQty.Text + " QTY TO BE PRODUCED. SELECT NEXT ITEM FOR STENCIL ASSIGN";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkEarmarkItem_Click(object sender, EventArgs e)
        {
            try
            {
                hdnPartType.Value = "";
                if (hdnSelectItem.Value != "")
                    gv_OrderItems.Rows[Convert.ToInt32(hdnSelectItem.Value)].BackColor = System.Drawing.Color.White;
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectItem.Value = clickedRow.RowIndex.ToString();
                if (hdnSelectItem.Value != "")
                    gv_OrderItems.Rows[Convert.ToInt32(hdnSelectItem.Value)].BackColor = System.Drawing.Color.Yellow;

                hdnAssyStatus.Value = ((HiddenField)clickedRow.FindControl("hdnAssyRimstatus")).Value;
                lblProcessID.Text = ((HiddenField)clickedRow.FindControl("hdnProcessID")).Value;
                hdnO_ItemID.Value = ((HiddenField)clickedRow.FindControl("hdnOrder_ItemID")).Value;
                Bind_AvailableStock();

                chkUpgradeList.DataSource = "";
                chkUpgradeList.DataBind();
                lblUpgradePopMsg.Text = "";
                spanTag.InnerText = "";
                lblPartFLevel.Text = "";
                chkUpgrade_PartF.DataSource = "";
                chkUpgrade_PartF.DataBind();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@tyretype", lblType.Text) };
                DataTable dtType = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_type_upgarde_list_v1", sp, DataAccess.Return_Type.DataTable);
                if (dtType != null && dtType.Rows.Count > 0)
                {
                    chkUpgradeList.DataSource = dtType;
                    chkUpgradeList.DataTextField = "UpgradeType";
                    chkUpgradeList.DataValueField = "UpgradeType";
                    chkUpgradeList.DataBind();

                    lblUpgradePopMsg.Text = "PART-C (GSA UPGRADE LIST FOR " + lblType.Text + ")";
                    spanTag.InnerText = "SHOW UPGRADE CHART FOR " + lblType.Text;

                    SqlParameter[] spUp = new SqlParameter[] { new SqlParameter("@tyretype", lblType.Text) };
                    DataTable dtTypeLevel = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_type_substitution_equivalents_v1", spUp, DataAccess.Return_Type.DataTable);
                    if (dtTypeLevel.Rows.Count > 0)
                    {
                        lblPartFLevel.Text = "PART-F (CURRENT STOCK UPGRADE LIST FOR " + lblType.Text + ")";
                        chkUpgrade_PartF.DataSource = dtTypeLevel;
                        chkUpgrade_PartF.DataTextField = "UpgradeType";
                        chkUpgrade_PartF.DataValueField = "UpgradeType";
                        chkUpgrade_PartF.DataBind();
                    }
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv", "gotoPreviewDiv('div_earkmark_OrderItems');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showdivitem", "gotoPreviewDiv('div_itemwisedesc');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_manufactureyear');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkAvailablePart_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkAvailableTxt = sender as LinkButton;
                bind_SelectOrder(lnkAvailableTxt.ID.Replace("lnkPart", "PART-"));

                ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv", "gotoPreviewDiv('div_earkmark_OrderItems');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showdivitem", "gotoPreviewDiv('div_itemwisedesc');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_manufactureyear');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnFindStencil_Click(object sender, EventArgs e)
        {
            try
            {
                lbl_StockQty.Text = "";
                lbl_StockErrMsg.Text = "";
                btnGSA_Assign.Text = "";

                gvStockSelection.DataSource = "";
                gvStockSelection.DataBind();

                string strMfy = "";
                foreach (ListItem chkMfy in chkManufactureYear.Items) { if (chkMfy.Selected) { strMfy += "'" + chkMfy.Value + "',"; } }

                string strGrade = "";
                foreach (ListItem chkGrade in chkManufactureGrade.Items) { if (chkGrade.Selected) { strGrade += "'" + chkGrade.Value + "',"; } }

                string strYearMonth = "";
                if (hdnPartType.Value == "PART-A" || hdnPartType.Value == "PART-B" || hdnPartType.Value == "PART-C")
                    strYearMonth = " and substring(barcode,10,4)<(RIGHT(YEAR(DATEADD(YEAR,-4,GETDATE())),2)+FORMAT(DATEADD(YEAR,-4,GETDATE()),'MM'))";
                else if (hdnPartType.Value == "PART-D" || hdnPartType.Value == "PART-E" || hdnPartType.Value == "PART-F")
                    strYearMonth = " and substring(barcode,10,4)>=(RIGHT(YEAR(DATEADD(YEAR,-4,GETDATE())),2)+FORMAT(DATEADD(YEAR,-4,GETDATE()),'MM'))";

                //string strQry = "";
                //if (hdnPartType.Value == "PART-A" || hdnPartType.Value == "PART-D")
                //{
                //    strQry = "select config,tyresize,rimsize,tyretype,brand,sidewall,right(barcode,1) as grade,Plant,substring(barcode,9,10) as stencilno," +
                //        "warehouse_location as location,(Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)) as yearofmanufacture from Production_warehouse_Data " +
                //        "where scandispatch=0 and (EarmarkStatus=0 or EarmarkStatus is null) and Plant='" + Utilities.Decrypt(Request["pid"].ToString()) + "' and " +
                //        "substring(barcode,10,2) in (" + strMfy.Remove(strMfy.Length - 1) + ") and Right(barcode,1) in (" + strGrade.Remove(strGrade.Length - 1) + ") and " +
                //        "config='" + lblPlatform.Text + "' and tyresize='" + lblTyresize.Text + "' and rimsize='" + lblRim.Text + "' and tyretype='" + lblType.Text + "' and " +
                //        "brand='" + lblBrand.Text + "' and sidewall='" + lblSidewall.Text + "' " + strYearMonth +
                //        " order by (Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)),substring(barcode,9,10)";
                //}
                //else if (hdnPartType.Value == "PART-B" || hdnPartType.Value == "PART-E")
                //{
                //    strQry = "select config,tyresize,rimsize,tyretype,brand,sidewall,right(barcode,1) as grade,Plant,substring(barcode,9,10) as stencilno," +
                //        "warehouse_location as location,(Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)) as yearofmanufacture from Production_warehouse_Data " +
                //        "where scandispatch=0 and (EarmarkStatus=0 or EarmarkStatus is null) and Plant='" + Utilities.Decrypt(Request["pid"].ToString()) + "' and " +
                //        "substring(barcode,10,2) in (" + strMfy.Remove(strMfy.Length - 1) + ") and Right(barcode,1) in (" + strGrade.Remove(strGrade.Length - 1) + ") and " +
                //        "config='" + lblPlatform.Text + "' and tyresize='" + lblTyresize.Text + "' and rimsize='" + lblRim.Text + "' and tyretype='" + lblType.Text +
                //        "' and (brand<>'" + lblBrand.Text + "' or sidewall<>'" + lblSidewall.Text + "') " + strYearMonth +
                //        " order by (Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)),substring(barcode,9,10)";
                //}
                //else if (hdnPartType.Value == "PART-C" || hdnPartType.Value == "PART-F")
                //{
                //    string strStockMode = hdnPartType.Value == "PART-C" ? "GSA" : "CURRENT";

                //    strQry = "select config,tyresize,rimsize,tyretype,brand,sidewall,right(barcode,1) as grade,Plant,substring(barcode,9,10) as stencilno," +
                //        "warehouse_location as location,(Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)) as yearofmanufacture from Production_warehouse_Data " +
                //        "where scandispatch=0 and (EarmarkStatus=0 or EarmarkStatus is null) and Plant='" + Utilities.Decrypt(Request["pid"].ToString()) + "' and " +
                //        "substring(barcode,10,2) in (" + strMfy.Remove(strMfy.Length - 1) + ") and Right(barcode,1) in (" + strGrade.Remove(strGrade.Length - 1) + ") and " +
                //        "config='" + lblPlatform.Text + "' and tyresize='" + lblTyresize.Text + "' and rimsize='" + lblRim.Text + "' and tyretype in (select UpgradeType from " +
                //        "TypeWise_UpgradeList where MasterType='" + lblType.Text + "' and StockMethod='" + strStockMode + "' and UpgradeStatus=1) " + strYearMonth +
                //        "order by (Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)),substring(barcode,9,10)";
                //}
                //DataTable dtStockList = (DataTable)daCOTS.ExecuteReader(strQry, DataAccess.Return_Type.DataTable);

                SqlParameter[] spS = new SqlParameter[] { 
                        new SqlParameter("@config", lblPlatform.Text), 
                        new SqlParameter("@tyresize", lblTyresize.Text), 
                        new SqlParameter("@rimsize", lblRim.Text), 
                        new SqlParameter("@tyretype", lblType.Text), 
                        new SqlParameter("@brand", lblBrand.Text), 
                        new SqlParameter("@sidewall", lblSidewall.Text), 
                        new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString())), 
                        new SqlParameter("@strMfy", strMfy.Remove(strMfy.Length - 1)), 
                        new SqlParameter("@strGrade", strGrade.Remove(strGrade.Length - 1)), 
                        new SqlParameter("@strYearMonth", strYearMonth), 
                        new SqlParameter("@PartType", hdnPartType.Value) 
                    };
                DataTable dtStockList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stencil_For_Assign", spS, DataAccess.Return_Type.DataTable);

                if (dtStockList.Rows.Count > 0)
                {
                    gvStockSelection.DataSource = dtStockList;
                    gvStockSelection.DataBind();

                    btnFindStencil.Style.Add("display", "none");
                    lbl_StockQty.Text = "AS PER FILTER AVAILABLE STOCK QTY : " + dtStockList.Rows.Count;
                    if (Convert.ToInt32(lblRequiredQty.Text) < dtStockList.Rows.Count)
                        gvStockSelection.HeaderRow.FindControl("checkAllChk").Visible = false;

                    for (int ch = 1; ch <= gvStockSelection.Rows.Count && ch <= Convert.ToInt32(lblRequiredQty.Text); ch++)
                    {
                        ((CheckBox)gvStockSelection.Rows[ch - 1].FindControl("chk_selectQty")).Checked = true;
                    }
                    if (gvStockSelection.Rows.Count <= Convert.ToInt32(lblRequiredQty.Text))
                        ((CheckBox)gvStockSelection.HeaderRow.FindControl("checkAllChk")).Checked = true;
                    else if (gvStockSelection.Rows.Count > Convert.ToInt32(lblRequiredQty.Text))
                    {
                        for (int ch = Convert.ToInt32(lblRequiredQty.Text); ch <= gvStockSelection.Rows.Count; ch++)
                        {
                            if (!((CheckBox)gvStockSelection.Rows[ch - 1].FindControl("chk_selectQty")).Checked)
                                ((CheckBox)gvStockSelection.Rows[ch - 1].FindControl("chk_selectQty")).Enabled = false;
                        }
                    }

                    btnGSA_Assign.Text = "ASSIGN STENCIL TO " + hdnPartType.Value;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv", "gotoPreviewDiv('div_earkmark_OrderItems');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showdivitem", "gotoPreviewDiv('div_itemwisedesc');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_manufactureyear');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showStockDiv", "gotoPreviewDiv('div_availablestencil');", true);
                }
                else
                {
                    lbl_StockErrMsg.Text = "AS PER FILTER STOCK NOT AVAILABLE, KINDLY CHANGE YOUR FILTER.";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv", "gotoPreviewDiv('div_earkmark_OrderItems');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showdivitem", "gotoPreviewDiv('div_itemwisedesc');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_manufactureyear');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnGSA_Assign_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_AssignStencilNo = new DataTable();
                dt_AssignStencilNo.Columns.Add("stencilno", typeof(string));

                DataTable dt_UnSignStencilNo = new DataTable();
                dt_UnSignStencilNo.Columns.Add("stencilno", typeof(string));

                for (int gv = gvStockSelection.Rows.Count - 1; gv >= 0; gv--)
                {
                    CheckBox chk = gvStockSelection.Rows[gv].FindControl("chk_selectQty") as CheckBox;
                    if (chk.Checked)
                        dt_AssignStencilNo.Rows.Add(gvStockSelection.Rows[gv].Cells[8].Text);
                    else if (!chk.Checked && dt_AssignStencilNo.Rows.Count > 0)
                        dt_UnSignStencilNo.Rows.Add(gvStockSelection.Rows[gv].Cells[8].Text);
                }

                if (dt_UnSignStencilNo.Rows.Count > 0)
                {
                    SqlParameter[] spU = new SqlParameter[] { 
                        new SqlParameter("@Production_Warehouse_Data_Earmark", dt_UnSignStencilNo), 
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                        new SqlParameter("@O_ItemID", hdnO_ItemID.Value), 
                        new SqlParameter("@AvoidkBy", Request.Cookies["TTSUser"].Value), 
                        new SqlParameter("@AvoidPartType", hdnPartType.Value) 
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_ins_Stencil_UnAssign_History", spU);
                }

                if (dt_AssignStencilNo.Rows.Count > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Production_Warehouse_Data_Earmark", dt_AssignStencilNo), 
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                        new SqlParameter("@EarmarkBy", Request.Cookies["TTSUser"].Value), 
                        new SqlParameter("@EarmarkPart", hdnPartType.Value), 
                        new SqlParameter("@O_ID", hdnOID.Value),
                        new SqlParameter("@O_ItemID", hdnO_ItemID.Value)
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_GSAEarmark_Production_WareHouse_Data", sp);
                    if (resp > 0)
                    {
                        lnkSkipEarmark.Text = "";
                        Bind_OrderItemList();
                        Bind_AvailableStock();
                        if (dt_UnSignStencilNo.Rows.Count == 0 && Convert.ToInt32(lblRequiredQty.Text) > 0)
                            bind_SelectOrder(hdnPartType.Value);
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv", "gotoPreviewDiv('div_earkmark_OrderItems');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showdivitem", "gotoPreviewDiv('div_itemwisedesc');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_manufactureyear');", true);
                if (dt_UnSignStencilNo.Rows.Count > 0)
                {
                    SqlParameter[] spF = new SqlParameter[] { new SqlParameter("@O_ItemID", hdnO_ItemID.Value), new SqlParameter("@AvoidPartType", hdnPartType.Value) };
                    DataSet dsF = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_StencilNotFifo", spF, DataAccess.Return_Type.DataSet);
                    if (dsF.Tables.Count == 2)
                    {
                        dlFifoItemMaster.DataSource = dsF.Tables[1];
                        dlFifoItemMaster.DataBind();

                        gvFifoUnassign.DataSource = dsF.Tables[0];
                        gvFifoUnassign.DataBind();
                        ScriptManager.RegisterStartupScript(Page, GetType(), "showFifoGV", "ShowUpgardeLevel('div_FifoUnAssignList');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}