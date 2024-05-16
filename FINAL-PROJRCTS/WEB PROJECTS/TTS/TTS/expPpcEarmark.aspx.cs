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
    public partial class expPpcEarmark : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sltl"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_ppc_sitl"].ToString() == "True" || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True" || dtUser.Rows[0]["ot_qcmmn"].ToString() == "True"
                            || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "" && Request["processid"] != null &&
                                Utilities.Decrypt(Request["processid"].ToString()) != "")
                            {
                                SqlParameter[] spSel = new SqlParameter[] { 
                                    new SqlParameter("@ProcessID", Utilities.Decrypt(Request["processid"].ToString())), 
                                    new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
                                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) 
                                };
                                DataTable dtRelItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Related_OrderItems", spSel, DataAccess.Return_Type.DataTable);
                                if (dtRelItems.Rows.Count > 0)
                                {
                                    gv_ItemRelatedOrders.DataSource = dtRelItems;
                                    gv_ItemRelatedOrders.DataBind();

                                    foreach (GridViewRow gRow in gv_ItemRelatedOrders.Rows)
                                    {
                                        if (((HiddenField)gRow.FindControl("hdnAssignOrderOID")).Value == Utilities.Decrypt(Request["oid"].ToString()) &&
                                            ((HiddenField)gRow.FindControl("hdnO_ItemID")).Value == Utilities.Decrypt(Request["itemid"].ToString()))
                                        {
                                            hdnMasterOID.Value = ((HiddenField)gRow.FindControl("hdnO_ItemID")).Value;
                                            hdnSelectIndex.Value = gRow.RowIndex.ToString();
                                            gv_ItemRelatedOrders.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                                            Bind_SelectAssignMaster(gv_ItemRelatedOrders.SelectedRow);
                                            break;
                                        }
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
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkAssignOrder_click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
            Bind_SelectAssignMaster(clickedRow);
        }
        private void Bind_SelectAssignMaster(GridViewRow clickedRow)
        {
            try
            {
                hdnAssingOID.Value = ((HiddenField)clickedRow.FindControl("hdnAssignOrderOID")).Value;
                hdnItemID.Value = ((HiddenField)clickedRow.FindControl("hdnO_ItemID")).Value;

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnAssingOID.Value) };
                DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ppc_assign_ordermaster", sp, DataAccess.Return_Type.DataTable);
                if (dtData.Rows.Count > 0)
                {
                    lblSelectedCustomerName.Text = dtData.Rows[0]["custfullname"].ToString();
                    lblSelectedOrderRefNo.Text = dtData.Rows[0]["OrderRefNo"].ToString();
                    lblSelectedWorkOrderNo.Text = dtData.Rows[0]["workorderno"].ToString();

                    Bind_AvailableStock();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "BindBackColor", "ctrlBackColor('" + hdnSelectIndex.Value + "');", true);
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

                SqlParameter[] spItem = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnAssingOID.Value), 
                    new SqlParameter("@O_ItemID", hdnItemID.Value) 
                };
                DataTable dtItem = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Item_Deatils_ForEarmark_v1", spItem, DataAccess.Return_Type.DataTable);
                if (dtItem.Rows.Count > 0)
                {
                    lblPlatform.Text = dtItem.Rows[0]["Config"].ToString();
                    lblTyresize.Text = dtItem.Rows[0]["tyresize"].ToString();
                    lblRim.Text = dtItem.Rows[0]["rimsize"].ToString();
                    lblType.Text = dtItem.Rows[0]["tyretype"].ToString();
                    lblBrand.Text = dtItem.Rows[0]["brand"].ToString();
                    lblSidewall.Text = dtItem.Rows[0]["sidewall"].ToString();
                    lblProcessID.Text = dtItem.Rows[0]["processid"].ToString();

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

                    SqlParameter[] spAvl = new SqlParameter[] { 
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                        new SqlParameter("@O_ID", hdnAssingOID.Value), 
                        new SqlParameter("@processid", lblProcessID.Text) 
                    };
                    DataSet dsAvl = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_available_stock_part_wise_To_PPC", spAvl, DataAccess.Return_Type.DataSet);
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
                        if ((lnkPartA.Text == "" && lnkPartB.Text == "" && lnkPartC.Text == "" && lnkPartD.Text == "" && lnkPartE.Text == "" && lnkPartF.Text == "")
                           || (lnkPartA.Text == "" && lnkPartD.Text == "" && Utilities.Decrypt(Request["oid"].ToString()) == hdnAssingOID.Value) || lblRequiredQty.Text == "0")
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "alertmsg", "alert('STOCK NOT AVAILABLE');", true);
                            Response.Redirect("expppc_verification1.aspx?pid=" + Request["pid"].ToString() + "&fid=" + Request["fid"].ToString(), false);
                        }
                    }
                }
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

                ScriptManager.RegisterStartupScript(Page, GetType(), "BindBackColor", "ctrlBackColor('" + hdnSelectIndex.Value + "');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_manufactureyear');", true);
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
                        new SqlParameter("@O_ID", hdnAssingOID.Value), 
                        new SqlParameter("@processid", lblProcessID.Text), 
                        new SqlParameter("@PartType", hdnPartType.Value) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stencil_mfy_processid_wise_To_PPC", sp, DataAccess.Return_Type.DataTable);
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

                            string strDom = DateTime.Now.AddMonths(-48).ToString("MMM") + "'" + DateTime.Now.AddMonths(-48).ToString("yy");

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

                string strQry = "";
                if (hdnPartType.Value == "PART-A" || hdnPartType.Value == "PART-D")
                {
                    strQry = "select config,tyresize,rimsize,tyretype,brand,sidewall,right(barcode,1) as grade,Plant,substring(barcode,9,10) as stencilno," +
                        "warehouse_location as location,(Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)) as yearofmanufacture from Production_warehouse_Data " +
                        "where scandispatch=0 and (EarmarkStatus=0 or EarmarkStatus is null) and Plant='" + Utilities.Decrypt(Request["pid"].ToString()) + "' and " +
                        "substring(barcode,10,2) in (" + strMfy.Remove(strMfy.Length - 1) + ") and Right(barcode,1) in (" + strGrade.Remove(strGrade.Length - 1) + ") and " +
                        "config='" + lblPlatform.Text + "' and tyresize='" + lblTyresize.Text + "' and rimsize='" + lblRim.Text + "' and tyretype='" + lblType.Text + "' and " +
                        "brand='" + lblBrand.Text + "' and sidewall='" + lblSidewall.Text + "' " + strYearMonth +
                        " order by (Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)),substring(barcode,9,10)";
                }
                else if (hdnPartType.Value == "PART-B" || hdnPartType.Value == "PART-E")
                {
                    strQry = "select config,tyresize,rimsize,tyretype,brand,sidewall,right(barcode,1) as grade,Plant,substring(barcode,9,10) as stencilno," +
                        "warehouse_location as location,(Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)) as yearofmanufacture from Production_warehouse_Data " +
                        "where scandispatch=0 and (EarmarkStatus=0 or EarmarkStatus is null) and Plant='" + Utilities.Decrypt(Request["pid"].ToString()) + "' and " +
                        "substring(barcode,10,2) in (" + strMfy.Remove(strMfy.Length - 1) + ") and Right(barcode,1) in (" + strGrade.Remove(strGrade.Length - 1) + ") and " +
                        "config='" + lblPlatform.Text + "' and tyresize='" + lblTyresize.Text + "' and rimsize='" + lblRim.Text + "' and tyretype='" + lblType.Text +
                        "' and (brand<>'" + lblBrand.Text + "' or sidewall<>'" + lblSidewall.Text + "') " + strYearMonth +
                        " order by (Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)),substring(barcode,9,10)";
                }
                else if (hdnPartType.Value == "PART-C" || hdnPartType.Value == "PART-F")
                {
                    string strStockMode = hdnPartType.Value == "PART-C" ? "GSA" : "CURRENT";

                    strQry = "select config,tyresize,rimsize,tyretype,brand,sidewall,right(barcode,1) as grade,Plant,substring(barcode,9,10) as stencilno," +
                        "warehouse_location as location,(Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)) as yearofmanufacture from Production_warehouse_Data " +
                        "where scandispatch=0 and (EarmarkStatus=0 or EarmarkStatus is null) and Plant='" + Utilities.Decrypt(Request["pid"].ToString()) + "' and " +
                        "substring(barcode,10,2) in (" + strMfy.Remove(strMfy.Length - 1) + ") and Right(barcode,1) in (" + strGrade.Remove(strGrade.Length - 1) + ") and " +
                        "config='" + lblPlatform.Text + "' and tyresize='" + lblTyresize.Text + "' and rimsize='" + lblRim.Text + "' and tyretype in (select UpgradeType from " +
                        "TypeWise_UpgradeList where MasterType='" + lblType.Text + "' and StockMethod='" + strStockMode + "' and UpgradeStatus=1) " + strYearMonth +
                        "order by (Left(YEAR(dateofmanufacture),2)+SUBSTRING(barcode,10,2)),substring(barcode,9,10)";
                }

                DataTable dtStockList = (DataTable)daCOTS.ExecuteReader(strQry, DataAccess.Return_Type.DataTable);
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
                    ScriptManager.RegisterStartupScript(Page, GetType(), "BindBackColor", "ctrlBackColor('" + hdnSelectIndex.Value + "');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_manufactureyear');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showStockDiv", "gotoPreviewDiv('div_availablestencil');", true);
                }
                else
                {
                    lbl_StockErrMsg.Text = "AS PER FILTER STOCK NOT AVAILABLE, KINDLY CHANGE YOUR FILTER.";
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
                        new SqlParameter("@O_ItemID", hdnItemID.Value), 
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
                        new SqlParameter("@O_ID", hdnAssingOID.Value),
                        new SqlParameter("@O_ItemID", hdnItemID.Value)
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_GSAEarmark_Production_WareHouse_Data", sp);
                    if (resp > 0)
                    {
                        Response.Redirect(Request.RawUrl, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnReqSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spIns = new SqlParameter[] { 
                    new SqlParameter("@ItemID", hdnMasterOID.Value), 
                    new SqlParameter("@ReqComment", txtReqRemarks.Text), 
                    new SqlParameter("@ReqBy", Request.Cookies["TTSUser"].Value) 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_ItemNewProductionRequest", spIns);
                if (resp > 0)
                    Response.Redirect("expppc_verification1.aspx?pid=" + Request["pid"].ToString() + "&fid=" + Request["fid"].ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}