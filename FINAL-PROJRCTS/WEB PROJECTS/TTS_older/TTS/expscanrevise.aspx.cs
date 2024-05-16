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
using System.Threading;

namespace TTS
{
    public partial class expscanrevise : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "")
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True" ||
                            dtUser.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True"))
                        {
                            if (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageTitle.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                       " - EXPORT " : " - DOMESTIC ");
                                Bind_GVpdiDetails();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL is wrong";
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
        private void Bind_GVpdiDetails()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@qstring", Utilities.Decrypt(Request["fid"].ToString())), 
                    new SqlParameter("@rtype", Utilities.Decrypt(Request["rtype"].ToString())), 
                    new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())) 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIDetails", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gv_PdiRevision.DataSource = dt;
                    gv_PdiRevision.DataBind();

                    switch (Utilities.Decrypt(Request["rtype"].ToString()))
                    {
                        case "delete":
                            {
                                lblPageHead.Text = "PDI BARCODE DELETE";
                                gv_PdiRevision.Columns[5].Visible = false;
                                gv_PdiRevision.Columns[6].Visible = false;
                                break;
                            }
                        case "approval":
                            {
                                lblPageHead.Text = "PDI APPROVAL FOR LOADING";
                                gv_PdiRevision.Columns[4].Visible = false;
                                break;
                            }
                    }
                }
                else
                {
                    lblErrMsg.Text = "No Records";
                    gv_PdiRevision.DataSource = null;
                    gv_PdiRevision.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkPdiReport_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                HiddenField hdnPID = (HiddenField)clickedRow.FindControl("hdnPID");
                Label lblOrderQty = (Label)clickedRow.FindControl("lblOrderQty");
                Label lblWorkOrderNo = (Label)clickedRow.FindControl("lblWorkOrderNo");
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@PID", hdnPID.Value), new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) };
                DataTable dtXls = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDI_Inspected_Report", sp, DataAccess.Return_Type.DataTable);
                if (dtXls.Rows.Count == Convert.ToInt32(lblOrderQty.Text))
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "PDI-WO-" + lblWorkOrderNo.Text + " QTY-" + lblOrderQty.Text));//"attachment;filename=" + txtStockXlsName.Text + ".xls");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/x-msexcel";
                    using (System.IO.StringWriter sw = new System.IO.StringWriter())
                    {
                        HtmlTextWriter hw = new HtmlTextWriter(sw);
                        GridView gv = new GridView();
                        gv.DataSource = dtXls;
                        gv.DataBind();
                        gv.RenderControl(hw);
                        Response.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                        gv = null;
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //
        }
        protected void btnViewOrderForDelete_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow ClickedRow = ((Button)sender).NamingContainer as GridViewRow;
                hdnSelectedRow.Value = Convert.ToString(ClickedRow.RowIndex);
                Bind_GVscannedList(ClickedRow);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('div_DeleteRecord');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_GVscannedList(GridViewRow Row)
        {
            try
            {
                lblSelectedCustomerName.Text = ((Label)Row.FindControl("lblCustomerName")).Text;
                lblSelectedOrderQty.Text = ((Label)Row.FindControl("lblOrderQty")).Text;
                lblSelectedWorkOrderNo.Text = ((Label)Row.FindControl("lblWorkOrderNo")).Text;
                lblSelectedScannedQty.Text = ((Label)Row.FindControl("lblScanedQty")).Text;
                hdnSelectedCustcode.Value = ((HiddenField)Row.FindControl("hdnCustCode")).Value;
                hdnCurrentPID.Value = ((HiddenField)Row.FindControl("hdnPID")).Value;

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@PID", hdnCurrentPID.Value), 
                    new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())) 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PdiScannedList", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gv_ScanedList.DataSource = dt;
                    gv_ScanedList.DataBind();

                    ViewState["dtGv"] = dt;
                    Bind_DDL_GV(dt, "");
                }
                Button1.CssClass = "Clicked";
                Button2.CssClass = "Initial";
                MultiView1.ActiveViewIndex = 0;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDeletePdi_click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_StencilNo = new DataTable();
                dt_StencilNo.Columns.Add("stencilno", typeof(string));
                foreach (GridViewRow Row in gv_ScanedList.Rows)
                {
                    CheckBox chk = Row.FindControl("chk_selectQty") as CheckBox;
                    if (chk.Checked && chk.Enabled)
                        dt_StencilNo.Rows.Add(((Label)Row.FindControl("lblbarcode")).Text.Substring(8, 10));
                }
                if (dt_StencilNo.Rows.Count > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Production_Warehouse_Data_Earmark", dt_StencilNo), 
                        new SqlParameter("@PID", hdnCurrentPID.Value), 
                        new SqlParameter("@RevisedBy", Request.Cookies["TTSUser"].Value), 
                        new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())) 
                    };
                    int resp2 = daCOTS.ExecuteNonQuery_SP("sp_del_scanned_pdi_barcode", sp);
                    if (resp2 > 0)
                    {
                        Bind_GVpdiDetails();
                        hdnSelectedRow.Value = "";
                        gv_ScanedList.DataSource = null;
                        gv_ScanedList.DataBind();
                        ddlPlatform.DataSource = "";
                        ddlPlatform.DataBind();
                        ddlBrand.DataSource = "";
                        ddlBrand.DataBind();
                        ddl_Sidewall.DataSource = "";
                        ddl_Sidewall.DataBind();
                        ddlTyretype.DataSource = "";
                        ddlTyretype.DataBind();
                        ddlTyreSize.DataSource = "";
                        ddlTyreSize.DataBind();
                        ddlRimSize.DataSource = "";
                        ddlRimSize.DataBind();

                        foreach (GridViewRow gRow in gv_PdiRevision.Rows)
                        {
                            if (hdnCurrentPID.Value == ((HiddenField)gRow.FindControl("hdnPID")).Value)
                            {
                                hdnSelectedRow.Value = gRow.RowIndex.ToString();
                                break;
                            }
                        }
                        if (hdnSelectedRow.Value != "")
                        {
                            GridViewRow Mainrow = gv_PdiRevision.Rows[Convert.ToInt32(hdnSelectedRow.Value)];
                            Bind_GVscannedList(Mainrow);
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('div_DeleteRecord');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_ScanedList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridViewRow Mainrow = gv_PdiRevision.Rows[Convert.ToInt32(hdnSelectedRow.Value)];
                Bind_GVscannedList(Mainrow);
                gv_ScanedList.PageIndex = e.NewPageIndex;
                gv_ScanedList.DataBind();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('div_DeleteRecord');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void Tab_Click(object sender, EventArgs e)
        {
            try
            {
                Button1.CssClass = "Initial";
                Button2.CssClass = "Initial";
                Button lnkTxt = sender as Button;
                if (lnkTxt.Text == "BARCODE WISE")
                {
                    Button1.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 0;
                }
                else if (lnkTxt.Text == "ITEM QTY WISE")
                {
                    if (hdnCurrentPID.Value != "")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@PID", hdnCurrentPID.Value) };
                        DataTable dtQty = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanItemQty", sp1, DataAccess.Return_Type.DataTable);
                        if (dtQty.Rows.Count > 0)
                        {
                            gvScannedItemWise.DataSource = dtQty;
                            gvScannedItemWise.DataBind();
                        }
                    }
                    Button2.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 1;
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('div_DeleteRecord');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnViewOrderForApproval_click(object sender, EventArgs e)
        {
            try
            {
                gvUnmatchBarcode.DataSource = null;
                gvUnmatchBarcode.DataBind();
                GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;
                bool boolScanQtyMatch = true;
                SqlParameter[] spRejectReason = new SqlParameter[] { 
                    new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString())), 
                    new SqlParameter("@PID", ((HiddenField)clickedRow.FindControl("hdnPID")).Value)
                };
                DataSet dsNotScanned = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Not_Scanned_Barcode", spRejectReason, DataAccess.Return_Type.DataSet);
                if (dsNotScanned.Tables[0].Rows.Count > 0)
                {
                    GV_Condition.DataSource = dsNotScanned.Tables[0];
                    GV_Condition.DataBind();

                    lblCustomer.Text = dsNotScanned.Tables[1].Rows[0]["customer"].ToString();
                    lblOrderNo.Text = dsNotScanned.Tables[1].Rows[0]["OrderNo"].ToString();
                    lblWorkOrder.Text = dsNotScanned.Tables[1].Rows[0]["workorderno"].ToString();
                    lblTotQty.Text = dsNotScanned.Tables[1].Rows[0]["totqty"].ToString();
                    lblEarmarkQty.Text = dsNotScanned.Tables[1].Rows[0]["earmarkqty"].ToString();
                    lblNotPdiQty.Text = dsNotScanned.Tables[1].Rows[0]["notpdiqty"].ToString();
                    hdnSelectedCustcode.Value = ((HiddenField)clickedRow.FindControl("hdnCustCode")).Value;

                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShowReject", "gotoPreviewDiv('div_PdiReject');", true);
                }
                else
                {
                    SqlParameter[] spDomChk = new SqlParameter[] { 
                        new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString())), 
                        new SqlParameter("@PID", ((HiddenField)clickedRow.FindControl("hdnPID")).Value)
                    };
                    DataSet dsScanData = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Matching_PDI_Inspection", spDomChk, DataAccess.Return_Type.DataSet);
                    if (dsScanData.Tables[0].Rows.Count > 0 && dsScanData.Tables[1].Rows.Count > 0 &&
                        dsScanData.Tables[0].Rows.Count == Convert.ToInt32(dsScanData.Tables[1].Compute("Sum(itemqty)", "").ToString()))
                    {
                        SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString())), 
                                new SqlParameter("@O_ID", ((HiddenField)clickedRow.FindControl("hdn_OrderID")).Value)
                            };
                        DataTable dtEarmark = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmark_barcodelist_ForApproval_Chk", sp, DataAccess.Return_Type.DataTable);
                        if (dtEarmark.Rows.Count > 0)
                        {
                            foreach (DataRow eRow in dtEarmark.Rows)
                            {
                                foreach (DataRow pRow in dsScanData.Tables[0].Select("stencilno='" + eRow["stencilno"].ToString() + "' and MatchedQty='0'"))
                                {
                                    pRow["MatchedQty"] = "1";
                                    break;
                                }
                            }
                        }
                        //********************************************************************
                        foreach (DataRow iRow in dsScanData.Tables[1].Rows)
                        {
                            Int32 itemQty = Convert.ToInt32(iRow["itemqty"].ToString());
                            foreach (DataRow eRow in dsScanData.Tables[0].Select("O_I_ID='" + iRow["O_ItemID"].ToString() + "' and MatchedQty=1"))
                            {
                                itemQty--;
                            }

                            if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                            {
                                for (int k = 0; k < itemQty; k++)
                                {
                                    foreach (DataRow sRow in dsScanData.Tables[0].Select("tyresize='" + iRow["tyresize"].ToString() + "' and rimsize='" +
                                        iRow["rimsize"].ToString() + "' and (tyretype='" + iRow["tyretype"].ToString() + "' or tyretype='" + iRow["sType"].ToString() +
                                        "' or tyretype='" + iRow["sType1"].ToString() + "') and (brand='" + iRow["brand"].ToString() + "' or brand='" +
                                        iRow["sBrand"].ToString() + "' or brand='" + iRow["sBrand1"].ToString() + "') and MatchedQty='0'"))
                                    {
                                        sRow["MatchedQty"] = "1";
                                        break;
                                    }
                                }
                            }
                            else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                            {
                                for (int k = 0; k < itemQty; k++)
                                {
                                    foreach (DataRow sRow in dsScanData.Tables[0].Select("tyresize='" + iRow["tyresize"].ToString() + "' and rimsize='" +
                                        iRow["rimsize"].ToString() + "' and (tyretype='" + iRow["tyretype"].ToString() + "' or tyretype='" + iRow["sType"].ToString() +
                                        "' or tyretype='" + iRow["sType1"].ToString() + "') and (brand='" + iRow["brand"].ToString() + "' or brand='" +
                                        iRow["sBrand"].ToString() + "' or brand='" + iRow["sBrand1"].ToString() + "') and (Config='" + iRow["Config"].ToString() +
                                        "' or Config='" + iRow["sPlatform"].ToString() + "' or Config='" + iRow["sPlatform1"].ToString() + "') and (sidewall='" +
                                        iRow["sidewall"].ToString() + "' or sidewall='" + iRow["sSidewall"].ToString() + "' or sidewall='" + iRow["sSidewall1"].ToString() +
                                        "') and MatchedQty='0'"))
                                    {
                                        sRow["MatchedQty"] = "1";
                                        break;
                                    }
                                }
                            }
                        }

                        Int32 unmatchQty = dsScanData.Tables[0].Select("MatchedQty='0'").Count();
                        if (unmatchQty > 0)
                        {
                            DataView dv_Scan = new DataView(dsScanData.Tables[0]);
                            dv_Scan.RowFilter = "MatchedQty='0'";
                            DataTable dtUnMatch = new DataTable();
                            dtUnMatch = dv_Scan.ToTable(true);
                            gvUnmatchBarcode.DataSource = dtUnMatch;
                            gvUnmatchBarcode.DataBind();


                            if (Request.Cookies["TTSUser"].Value.ToLower() == "somu" || Request.Cookies["TTSUser"].Value.ToLower() == "anand" || Request.Cookies["TTSUser"].Value.ToLower() == "admin")
                            {
                                                            }
                            else
                            {
                                string nams = Request.Cookies["TTSUser"].Value.ToLower();
                                boolScanQtyMatch = false;
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JVerify3", "alert('TOTAL UNMATCHED QTY FOR THIS ORDER: " + unmatchQty.ToString() + "');", true);


                            }
                        }
                    }
                    else if (dsScanData.Tables[0].Rows.Count > 0 && Convert.ToInt32(dsScanData.Tables[2].Rows[0]["claimqty"].ToString()) > 0)
                    {
                        if (dsScanData.Tables[0].Rows.Count == Convert.ToInt32(dsScanData.Tables[2].Rows[0]["claimqty"].ToString()))
                            boolScanQtyMatch = true;
                        else if (dsScanData.Tables[0].Rows.Count != Convert.ToInt32(dsScanData.Tables[2].Rows[0]["claimqty"].ToString()))
                        {
                            boolScanQtyMatch = false;
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JVerify99", "alert('CLIAM AND INSPECTION QTY NOT EQUAL');", true);
                        }
                    }
                    else
                    {
                        boolScanQtyMatch = false;
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JVerify99", "alert('ORDER AND INSPECTION QTY NOT EQUAL');", true);
                    }

                    if (boolScanQtyMatch)
                    {
                        bool boolRimInspect = true;
                        if (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() != "SLTL" && Utilities.Decrypt(Request["pid"].ToString()).ToUpper() != "SITL")
                        {
                            foreach (DataRow rRow in dsScanData.Tables[1].Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                            {
                                SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@custcode", ((HiddenField)clickedRow.FindControl("hdnCustCode")).Value), 
                                new SqlParameter("@O_ID", ((HiddenField)clickedRow.FindControl("hdn_OrderID")).Value), 
                                new SqlParameter("@workorderno", ((Label)clickedRow.FindControl("lblWorkOrderNo")).Text),
                                new SqlParameter("@RimPlant", Utilities.Decrypt(Request["pid"].ToString())),
                                new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                            };
                                string strMsg = (string)daCOTS.ExecuteScalar_SP("sp_chk_upd_splitrim_order", sp);
                                if (strMsg.Contains("INSPECTION COMPLETED."))
                                    boolRimInspect = true;
                                else
                                {
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JVerify100", "alert('" + strMsg + "');", true);
                                    boolRimInspect = false;
                                }
                                break;
                            }
                        }

                        if (boolRimInspect)
                        {
                            SqlParameter[] sp1 = new SqlParameter[] { 
                                new SqlParameter("@PID", ((HiddenField)clickedRow.FindControl("hdnPID")).Value),
                                new SqlParameter("@completedby", Request.Cookies["TTSUser"].Value)
                            };
                            int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_PDI_ScanComplete", sp1);
                            if (resp > 0)
                            {
                                SqlParameter[] sp = new SqlParameter[] { 
                                    new SqlParameter("@Custcode", ((HiddenField)clickedRow.FindControl("hdnCustCode")).Value), 
                                    new SqlParameter("@Orderrefno", ((HiddenField)clickedRow.FindControl("hdnOrderRefno")).Value), 
                                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) 
                                };
                                int int_OID = (int)daCOTS.ExecuteScalar_SP("sp_sel_O_ID", sp);
                                if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                                    DomesticScots.ins_dom_StatusChangedDetails(int_OID, "7", "MOVE TO INVOICE/ DC PREPARE", Request.Cookies["TTSUser"].Value);
                                else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                                    DomesticScots.ins_dom_StatusChangedDetails(int_OID, "20", "MOVE LOADING APPROVAL", Request.Cookies["TTSUser"].Value);
                                gv_PdiRevision.EditIndex = -1;
                                Bind_GVpdiDetails();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlPlatform_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("PLATFORM");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlTyreSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("TYRESIZE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlRimSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("RIMSIZE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlTyretype_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("TYRETYPE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlBrand_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("BRAND");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Sidewall_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("SIDEWALL");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Make_Where_Condition(string strField)
        {
            try
            {
                DataTable dt = ViewState["dtGv"] as DataTable;
                string strCond = "";
                if (ddlPlatform.SelectedItem.Text != "ALL")

                    if (strCond.Length == 0)
                        strCond += "Config='" + ddlPlatform.SelectedItem.Text + "'";
                    else
                        strCond += " and Config='" + ddlPlatform.SelectedItem.Text + "'";
                if (ddlTyreSize.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "tyresize='" + ddlTyreSize.SelectedItem.Text + "'";
                    else
                        strCond += " and tyresize='" + ddlTyreSize.SelectedItem.Text + "'";
                if (ddlRimSize.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "rimsize='" + ddlRimSize.SelectedItem.Text + "'";
                    else
                        strCond += " and rimsize='" + ddlRimSize.SelectedItem.Text + "'";
                if (ddlTyretype.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "tyretype='" + ddlTyretype.SelectedItem.Text + "'";
                    else
                        strCond += " and tyretype='" + ddlTyretype.SelectedItem.Text + "'";
                if (ddlBrand.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "brand='" + ddlBrand.SelectedItem.Text + "'";
                    else
                        strCond += " and brand='" + ddlBrand.SelectedItem.Text + "'";
                if (ddl_Sidewall.SelectedItem.Text != "ALL")
                    if (strCond.Length == 0)
                        strCond += "sidewall='" + ddl_Sidewall.SelectedItem.Text + "'";
                    else
                        strCond += " and sidewall='" + ddl_Sidewall.SelectedItem.Text + "'";
                DataView dtView = new DataView(dt, strCond, "barcode", DataViewRowState.CurrentRows);
                if (dtView.Count > 0)
                {
                    Bind_DDL_GV(dtView.ToTable(), strField);
                }
                else
                {
                    lblErrMsg.Text = "NO RECORDS";
                    gv_ScanedList.DataSource = dtView;
                    gv_ScanedList.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('div_DeleteRecord');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_DDL_GV(DataTable dtGv, string strCont)
        {
            try
            {
                DataView dtView = new DataView(dtGv);
                switch (strCont)
                {
                    case "PLATFORM":
                        dtView.Sort = "tyresize";
                        DataTable distinctTyreSize = dtView.ToTable(true, "tyresize");
                        Utilities.ddl_Binding(ddlTyreSize, distinctTyreSize, "tyresize", "tyresize", "");
                        ddlTyreSize.Items.Insert(0, "ALL");
                        dtView.Sort = "rimsize";
                        DataTable distinctRimSize = dtView.ToTable(true, "rimsize");
                        Utilities.ddl_Binding(ddlRimSize, distinctRimSize, "rimsize", "rimsize", "");
                        ddlRimSize.Items.Insert(0, "ALL");
                        dtView.Sort = "tyretype";
                        DataTable distinctTyretype = dtView.ToTable(true, "tyretype");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "tyretype", "tyretype", "");
                        ddlTyretype.Items.Insert(0, "ALL");
                        dtView.Sort = "brand";
                        DataTable distinctBrand = dtView.ToTable(true, "brand");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "brand", "brand", "");
                        ddlBrand.Items.Insert(0, "ALL");
                        dtView.Sort = "sidewall";
                        DataTable distinctSidewall = dtView.ToTable(true, "sidewall");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                    case "TYRESIZE":
                        dtView.Sort = "rimsize";
                        distinctRimSize = dtView.ToTable(true, "rimsize");
                        Utilities.ddl_Binding(ddlRimSize, distinctRimSize, "rimsize", "rimsize", "");
                        ddlRimSize.Items.Insert(0, "ALL");
                        dtView.Sort = "tyretype";
                        distinctTyretype = dtView.ToTable(true, "tyretype");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "tyretype", "tyretype", "");
                        ddlTyretype.Items.Insert(0, "ALL");
                        dtView.Sort = "brand";
                        distinctBrand = dtView.ToTable(true, "brand");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "brand", "brand", "");
                        ddlBrand.Items.Insert(0, "ALL");
                        dtView.Sort = "sidewall";
                        distinctSidewall = dtView.ToTable(true, "sidewall");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                    case "RIMSIZE":
                        distinctTyretype = dtView.ToTable(true, "tyretype");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "tyretype", "tyretype", "");
                        ddlTyretype.Items.Insert(0, "ALL");


                        dtView.Sort = "brand";
                        distinctBrand = dtView.ToTable(true, "brand");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "brand", "brand", "");
                        ddlBrand.Items.Insert(0, "ALL");

                        dtView.Sort = "sidewall";
                        distinctSidewall = dtView.ToTable(true, "sidewall");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;

                    case "TYRETYPE":
                        dtView.Sort = "brand";
                        distinctBrand = dtView.ToTable(true, "brand");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "brand", "brand", "");
                        ddlBrand.Items.Insert(0, "ALL");

                        dtView.Sort = "sidewall";
                        distinctSidewall = dtView.ToTable(true, "sidewall");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;

                    case "BRAND":
                        dtView.Sort = "sidewall";
                        distinctSidewall = dtView.ToTable(true, "sidewall");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                    case "SIDEWALL":
                        break;
                    default:
                        dtView.Sort = "Config";
                        DataTable distinctType = dtView.ToTable(true, "Config");
                        Utilities.ddl_Binding(ddlPlatform, distinctType, "Config", "Config", "");
                        ddlPlatform.Items.Insert(0, "ALL");

                        dtView.Sort = "tyresize";
                        distinctTyreSize = dtView.ToTable(true, "tyresize");
                        Utilities.ddl_Binding(ddlTyreSize, distinctTyreSize, "tyresize", "tyresize", "");
                        ddlTyreSize.Items.Insert(0, "ALL");

                        dtView.Sort = "rimsize";
                        distinctRimSize = dtView.ToTable(true, "rimsize");
                        Utilities.ddl_Binding(ddlRimSize, distinctRimSize, "rimsize", "rimsize", "");
                        ddlRimSize.Items.Insert(0, "ALL");

                        dtView.Sort = "tyretype";
                        distinctTyretype = dtView.ToTable(true, "tyretype");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "tyretype", "tyretype", "");
                        ddlTyretype.Items.Insert(0, "ALL");


                        dtView.Sort = "brand";
                        distinctBrand = dtView.ToTable(true, "brand");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "brand", "brand", "");
                        ddlBrand.Items.Insert(0, "ALL");

                        dtView.Sort = "sidewall";
                        distinctSidewall = dtView.ToTable(true, "sidewall");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                }
                dtView = new DataView(dtGv, "", "barcode DESC", DataViewRowState.CurrentRows);
                if (dtView.Count > 0)
                {
                    gv_ScanedList.DataSource = dtView;
                    gv_ScanedList.DataBind();
                }
                else
                {
                    gv_ScanedList.DataSource = null;
                    gv_ScanedList.DataBind();
                    lblErrMsg.Text = "NO RECORDS";
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('div_DeleteRecord');", true);
            }

            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPdiRejectSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_StencilNo = new DataTable();
                dt_StencilNo.Columns.Add("StencilNo", typeof(string));
                dt_StencilNo.Columns.Add("PPC_Status", typeof(string));
                foreach (GridViewRow row in GV_Condition.Rows)
                {
                    dt_StencilNo.Rows.Add(row.Cells[6].Text, ((TextBox)row.FindControl("txtReason")).Text);
                }
                if (dt_StencilNo.Rows.Count > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Data_PPCApproval", dt_StencilNo),
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())),
                        new SqlParameter("@RejectedBY", Request.Cookies["TTSUser"].Value)
                    };
                    int result = daCOTS.ExecuteNonQuery_SP("sp_ins_PDI_RejectReason", sp);
                    if (result > 0)
                    {
                        SqlParameter[] spID = new SqlParameter[] { 
                            new SqlParameter("@Custcode", hdnSelectedCustcode.Value), 
                            new SqlParameter("@Orderrefno", lblOrderNo.Text), 
                            new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) 
                        };
                        int int_OID = (int)daCOTS.ExecuteScalar_SP("sp_sel_O_ID", spID);
                        int resp = DomesticScots.ins_dom_StatusChangedDetails(int_OID, "37", "WAITING FOR PDI REJECTED STENCIL REVOKE", Request.Cookies["TTSUser"].Value);
                        if (resp > 0)
                        {
                            string strMsg = "Successfully saved. Kindly inform to CRM for revoking the rejected stencil";
                            ScriptManager.RegisterStartupScript(Page, GetType(), "SaveSuccess", "alert('" + strMsg + "');", true);
                            Bind_GVpdiDetails();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_PdiRevision_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}