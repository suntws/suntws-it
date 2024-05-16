using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.Web.Services;
using System.IO;
using System.Globalization;

namespace TTS
{
    public partial class StockOrderPrepare : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dom_orderentry"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_orderentry"].ToString() == "True" || Request.Cookies["TTSUserDepartment"].Value == "EDC" ||
                            Request.Cookies["TTSUserDepartment"].Value == "QC" || Request.Cookies["TTSUserDepartment"].Value == "PPC"))
                        {
                            lblPageHead.Text = Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "DOMESTIC " : "EXPORT ";
                            DataTable dtCustList = new DataTable();
                            if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                            {
                                ddl_StockCustomerName.Items.Add(new ListItem("--SELECT--", "--SELECT--"));
                                ddl_StockCustomerName.Items.Add(new ListItem("DOMESTIC STOCK ORDER", "BACKLOG"));
                            }
                            else if (Utilities.Decrypt(Request["fid"].ToString()) == "t")
                            {
                                ddl_StockCustomerName.Items.Add(new ListItem("--SELECT--", "--SELECT--"));
                                ddl_StockCustomerName.Items.Add(new ListItem("TRIAL ORDER", "TRIAL"));
                            }
                            else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                            {
                                DataTable dtStockCustName = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_export_fullname", DataAccess.Return_Type.DataTable);
                                Utilities.ddl_Binding(ddl_StockCustomerName, dtStockCustName, "custfullname", "custcode", "--SELECT--");
                            }
                            if (Utilities.Decrypt(Request["sid"].ToString()) != "")
                            {
                                hdnStockorderid.Value = (Utilities.Decrypt(Request.QueryString["sid"].ToString()));
                                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StockOrderId", Convert.ToInt32(hdnStockorderid.Value)) };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stockordercustdetails", sp, DataAccess.Return_Type.DataTable);
                                ViewState["dtMaster"] = dt;
                                bind_Stockcustdetails();
                                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
                            }
                        }
                        else
                        {
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
        protected void ddl_StockCustomerName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (ddl_StockCustomerName.SelectedItem.Value != "TRIAL")
                {
                    string strCode = ddl_StockCustomerName.SelectedItem.Value == "BACKLOG" ? "DE0048" : ddl_StockCustomerName.SelectedItem.Value;
                    SqlParameter[] spData = new SqlParameter[] { new SqlParameter("@CustCode", strCode) };
                    DataTable dtJathagam = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ApprovedList_For_ManualTyreType", spData, DataAccess.Return_Type.DataTable);
                    ViewState["dtJathagam"] = dtJathagam;
                }
                else if (ddl_StockCustomerName.SelectedItem.Value == "TRIAL")
                {
                    DataTable dtData = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Data_For_TrialOrder", DataAccess.Return_Type.DataTable);
                    ViewState["dtJathagam"] = dtData;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                ddlPlatform.DataSource = "";
                ddlPlatform.DataBind();
                DataTable dtJathagam = (DataTable)ViewState["dtJathagam"];
                if (dtJathagam.Rows.Count > 0)
                {
                    DataView dtTypeView = new DataView(dtJathagam);
                    dtTypeView.RowFilter = "SizeCategory = '" + ddl_Category.SelectedItem.Text + "'";
                    dtTypeView.Sort = "Config ASC";
                    DataTable disConfig = dtTypeView.ToTable(true, "Config");
                    if (disConfig.Rows.Count > 0)
                    {
                        ddlPlatform.DataSource = disConfig;
                        ddlPlatform.DataTextField = "Config";
                        ddlPlatform.DataValueField = "Config";
                        ddlPlatform.DataBind();
                        if (disConfig.Rows.Count == 1)
                            ddlPlatform_IndexChange(null, null);
                        else
                            ddlPlatform.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
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
                lblErrMsg.Text = "";
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                DataTable dtJathagam = ViewState["dtJathagam"] as DataTable;
                if (dtJathagam.Rows.Count > 0)
                {
                    DataView dtTypeView = new DataView(dtJathagam);
                    dtTypeView.RowFilter = "SizeCategory = '" + ddl_Category.SelectedItem.Text + "' and Config = '" + ddlPlatform.SelectedItem.Text + "'";
                    dtTypeView.Sort = "Brand ASC";
                    DataTable disBrand = dtTypeView.ToTable(true, "Brand");
                    if (disBrand.Rows.Count > 0)
                    {
                        ddlBrand.DataSource = disBrand;
                        ddlBrand.DataTextField = "Brand";
                        ddlBrand.DataValueField = "Brand";
                        ddlBrand.DataBind();
                        if (disBrand.Rows.Count == 1)
                            ddlBrand_IndexChange(sender, e);
                        else
                            ddlBrand.Items.Insert(0, new System.Web.UI.WebControls.ListItem("CHOOSE", "CHOOSE"));
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
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
                lblErrMsg.Text = "";
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();

                DataTable dtJathagam = ViewState["dtJathagam"] as DataTable;
                if (dtJathagam.Rows.Count > 0)
                {
                    DataView dtTypeView = new DataView(dtJathagam);
                    dtTypeView.RowFilter = "SizeCategory = '" + ddl_Category.SelectedItem.Text + "' and Config = '" + ddlPlatform.SelectedItem.Text + "' and Brand='" +
                        ddlBrand.SelectedItem.Text + "'";
                    dtTypeView.Sort = "Sidewall ASC";
                    DataTable disSidewall = dtTypeView.ToTable(true, "Sidewall");
                    if (disSidewall.Rows.Count > 0)
                    {
                        ddlSidewall.DataSource = disSidewall;
                        ddlSidewall.DataTextField = "Sidewall";
                        ddlSidewall.DataValueField = "Sidewall";
                        ddlSidewall.DataBind();
                        if (disSidewall.Rows.Count == 1)
                            ddlSidewall_IndexChange(sender, e);
                        else
                            ddlSidewall.Items.Insert(0, new System.Web.UI.WebControls.ListItem("CHOOSE", "CHOOSE"));
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlSidewall_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                ddlType.DataSource = "";
                ddlType.DataBind();

                DataTable dtJathagam = ViewState["dtJathagam"] as DataTable;
                if (dtJathagam.Rows.Count > 0)
                {
                    DataView dtTypeView = new DataView(dtJathagam);
                    dtTypeView.RowFilter = "SizeCategory = '" + ddl_Category.SelectedItem.Text + "' and Config = '" + ddlPlatform.SelectedItem.Text + "' and Brand='" +
                        ddlBrand.SelectedItem.Text + "' and Sidewall='" + ddlSidewall.SelectedItem.Text + "'";
                    dtTypeView.Sort = "TyreType ASC";
                    DataTable disTyreType = dtTypeView.ToTable(true, "TyreType");
                    if (disTyreType.Rows.Count > 0)
                    {
                        ddlType.DataSource = disTyreType;
                        ddlType.DataTextField = "TyreType";
                        ddlType.DataValueField = "TyreType";
                        ddlType.DataBind();
                        if (disTyreType.Rows.Count == 1)
                            ddlType_IndexChange(sender, e);
                        else
                            ddlType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("CHOOSE", "CHOOSE"));
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Size_For_ManualOrder", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlSize.DataSource = dt;
                    ddlSize.DataTextField = "TyreSize";
                    ddlSize.DataValueField = "TyreSize";
                    ddlSize.DataBind();
                    if (dt.Rows.Count == 1)
                        ddlSize_IndexChange(sender, e);
                    else
                        ddlSize.Items.Insert(0, new System.Web.UI.WebControls.ListItem("CHOOSE", "CHOOSE"));
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                sp1[4] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Rim_ForManualOrder", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlRim.DataSource = dt;
                    ddlRim.DataTextField = "RimSize";
                    ddlRim.DataValueField = "RimSize";
                    ddlRim.DataBind();
                    if (dt.Rows.Count == 1)
                        ddlRim_IndexChange(sender, e);
                    else
                        ddlRim.Items.Insert(0, new System.Web.UI.WebControls.ListItem("CHOOSE", "CHOOSE"));
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlRim_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                txt_Itemqty.Text = "";
                txt_processid.Text = "";
                txt_finishWght.Text = "0.00";

                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                sp1[4] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);
                sp1[5] = new SqlParameter("@rimsize", ddlRim.SelectedItem.Text);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("SP_GET_QuotationPrepare_FinishedWt", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    txt_processid.Text = dt.Rows[0]["ProcessID"].ToString();
                    txt_finishWght.Text = dt.Rows[0]["FinishedWt"].ToString();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                SqlParameter[] sp = new SqlParameter[12];
                sp[0] = new SqlParameter("@StockOrderId", Convert.ToInt32(hdnStockorderid.Value));
                sp[1] = new SqlParameter("@Category", ddl_Category.SelectedItem.Text);
                sp[2] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                sp[3] = new SqlParameter("@Brand", ddlBrand.SelectedItem.Text);
                sp[4] = new SqlParameter("@Sidewall", ddlSidewall.SelectedItem.Text);
                sp[5] = new SqlParameter("@TyreType", ddlType.SelectedItem.Text);
                sp[6] = new SqlParameter("@TyreSize", ddlSize.SelectedItem.Text);
                sp[7] = new SqlParameter("@RimSize", ddlRim.SelectedItem.Text);
                sp[8] = new SqlParameter("@ProcessId", txt_processid.Text);
                sp[9] = new SqlParameter("@ItemQty", Convert.ToInt32(txt_Itemqty.Text));
                sp[10] = new SqlParameter("@FinishedWt", txt_finishWght.Text != "" ? Convert.ToDecimal(txt_finishWght.Text) : Convert.ToDecimal("0.00"));
                sp[11] = new SqlParameter("@OrderId", Convert.ToInt32(hdnStockorderid.Value));
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_StockOrderItemlist", sp);
                if (resp > 0)
                    Bind_OrdeItemGrid();
                else
                    lblErrMsg.Text = "Item already exists";

                ddlSize.SelectedIndex = 0;
                ddlRim.SelectedIndex = 0;
                txt_finishWght.Text = "0.00";
                txt_Itemqty.Text = "";
                txt_processid.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveCustDetails_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";

                SqlParameter[] spchk = new SqlParameter[] { 
                    new SqlParameter("@CustStdcode", ddl_StockCustomerName.SelectedItem.Value), 
                    new SqlParameter("@RefNo", txt_StockOrderRefNo.Text) 
                };
                DataTable dtChk = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_Stockorderrefno", spchk, DataAccess.Return_Type.DataTable);
                if (dtChk.Rows.Count > 0)
                    lblErrMsg.Text = "Ref no already created to " + dtChk.Rows[0]["Plant"].ToString() + " on " + dtChk.Rows[0]["CreatedDate"].ToString() +
                        " by " + dtChk.Rows[0]["CreatedBy"].ToString();
                else
                {
                    SqlParameter[] sp = new SqlParameter[7];
                    sp[0] = new SqlParameter("@CustStdCode", ddl_StockCustomerName.SelectedItem.Value);
                    sp[1] = new SqlParameter("@StockRefNo", txt_StockOrderRefNo.Text);
                    sp[2] = new SqlParameter("@Plant", ddl_plant.SelectedItem.Text);
                    sp[3] = new SqlParameter("@StartDate", DateTime.ParseExact(txt_StockStartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    sp[4] = new SqlParameter("@EndDate", DateTime.ParseExact(txt_StockEndDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    sp[5] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    sp[6] = new SqlParameter("@Ordertype", Utilities.Decrypt(Request["fid"].ToString()));
                    DataTable dtStockOrderId = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_StockOrderMasterDetails", sp, DataAccess.Return_Type.DataTable);
                    if (dtStockOrderId != null && dtStockOrderId.Rows.Count > 0)
                    {
                        hdnStockorderid.Value = dtStockOrderId.Rows[0]["StockOrderid"].ToString();
                        sp = new SqlParameter[] { new SqlParameter("@StockOrderId", Convert.ToInt32(hdnStockorderid.Value)) };
                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stockordercustdetails", sp, DataAccess.Return_Type.DataTable);
                        ViewState["dtMaster"] = dt;
                        bind_Stockcustdetails();
                        btnSaveOrder.Text = "COMPLETE THE ORDER ENTRY AND MOVE TO NEXT PROCESS";
                        btnSaveOrder.Visible = true;
                        btnDraft.Visible = true;
                        ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
                    }
                    else
                        lblErrMsg.Text = "Stock Order Ref No Already Exists";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_Stockcustdetails()
        {
            try
            {
                txt_workorderNo.Enabled = true;
                DataTable dt = (DataTable)ViewState["dtMaster"];
                if (dt.Rows.Count > 0)
                {
                    ddl_StockCustomerName.SelectedIndex = ddl_StockCustomerName.Items.IndexOf(ddl_StockCustomerName.Items.FindByValue(dt.Rows[0]["CustStdCode"].ToString()));
                    ddl_StockCustomerName_IndexChange(null, null);
                    ddl_plant.SelectedIndex = ddl_plant.Items.IndexOf(ddl_plant.Items.FindByText(dt.Rows[0]["Plant"].ToString()));
                    hdnPlant.Value = dt.Rows[0]["Plant"].ToString();
                    txt_StockOrderRefNo.Text = dt.Rows[0]["RefNo"].ToString();
                    txt_StockStartDate.Text = dt.Rows[0]["StartDate"].ToString();
                    txt_StockEndDate.Text = dt.Rows[0]["EndDate"].ToString();

                    if (dt.Rows[0]["Workorderno"].ToString() != "0")
                    {
                        txt_stockreviseno.Text = dt.Rows[0]["revise_no"].ToString() == "" ? "00" : dt.Rows[0]["revise_no"].ToString();
                        txt_workorderNo.Text = dt.Rows[0]["Workorderno"].ToString();
                        txt_workorderNo.Enabled = false;
                    }
                    else if (txt_workorderNo.Text == "")
                        WorkOrderPrepare();

                    Bind_OrdeItemGrid();
                    if (dt.Rows[0]["StockStatus"].ToString() == "4" || dt.Rows[0]["StockStatus"].ToString() == "3" || dt.Rows[0]["StockStatus"].ToString() == "2")
                    {
                        btnSaveOrder.Visible = true;
                        btnDraft.Visible = true;
                        lblreviseno.Visible = true;
                        lblworkorderno.Visible = true;
                        txt_workorderNo.Visible = true;
                        txt_stockreviseno.Visible = true;
                        if (dt.Rows[0]["StockStatus"].ToString() == "2")
                            btnSaveOrder.Text = "GENERATE PDF";
                        if (dt.Rows[0]["StockStatus"].ToString() == "3")
                            btnSaveOrder.Text = "MOVE TO PRODUCTION";
                        if (dt.Rows[0]["StockStatus"].ToString() == "4")
                            btnSaveOrder.Text = "COMPLETE THE ORDER ENTRY AND MOVE TO NEXT PROCESS";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "disOrder", "div_custorder();", true);
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrdeItemGrid()
        {
            try
            {
                lblErrMsg.Text = "";
                lnkStockWorkOrder.Text = "";
                gv_Addeditems.DataSource = "";
                gv_Addeditems.DataBind();

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StockOrderId", Convert.ToInt32(hdnStockorderid.Value)) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complete_Stockorderitem_list", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ViewState["StockItemlist"] = dt;
                    gv_Addeditems.DataSource = dt;
                    gv_Addeditems.DataBind();

                    gv_Addeditems.FooterRow.Cells[8].Text = "Total";
                    gv_Addeditems.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                    gv_Addeditems.FooterRow.Cells[9].Text = dt.Compute("Sum(itemqty)", "").ToString();
                    gv_Addeditems.FooterRow.Cells[0].Visible = false;
                    btnSaveOrder.Text = "COMPLETE THE ORDER ENTRY AND MOVE TO NEXT PROCESS";
                    btnSaveOrder.Visible = true;
                    btnDraft.Visible = true;

                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    string path2 = serverURL + "Stockorderfiles\\" + ddl_StockCustomerName.SelectedItem.Value + "\\" + hdnStockorderid.Value + ".pdf";
                    FileInfo file2 = new FileInfo(path2);
                    if (file2.Exists)
                        lnkStockWorkOrder.Text = hdnStockorderid.Value + ".pdf";

                    ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
                }
                else
                    lblErrMsg.Text = "No Items added Yet!!!";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_Addeditems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gv_Addeditems.Rows[e.RowIndex];
                Label lblProcessid = row.FindControl("lblSid") as Label;
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@ItemID", Convert.ToInt32(lblProcessid.Text)), 
                    new SqlParameter("@OrderId", Convert.ToInt32(hdnStockorderid.Value)) 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_del_stockOrderitemlist_additem", sp);
                if (resp > 0)
                    Bind_OrdeItemGrid();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_Addeditems_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gv_Addeditems.EditIndex = e.NewEditIndex;
                Bind_OrdeItemGrid();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_Addeditems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gv_Addeditems.EditIndex = -1;
                Bind_OrdeItemGrid();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_Addeditems_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gv_Addeditems.Rows[e.RowIndex];
                Label lblProcessid = row.FindControl("lblSid") as Label;
                TextBox txtChangeQty = row.FindControl("txtChangeQty") as TextBox;
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@ItemID", Convert.ToInt32(lblProcessid.Text)), 
                    new SqlParameter("@itemqty", Convert.ToInt32(txtChangeQty.Text)), 
                    new SqlParameter("@OrderID", Convert.ToInt32(hdnStockorderid.Value)) 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_stockOrderitemlist_additem", sp);
                if (resp > 0)
                {
                    gv_Addeditems.EditIndex = -1;
                    Bind_OrdeItemGrid();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                if (gv_Addeditems.Rows.Count > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StockOrderId", Convert.ToInt32(hdnStockorderid.Value)) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stockordercustdetails", sp, DataAccess.Return_Type.DataTable);
                    ViewState["dtMaster"] = dt;
                    bind_Stockcustdetails();
                    if (btnSaveOrder.Text == "COMPLETE THE ORDER ENTRY AND MOVE TO NEXT PROCESS")
                    {
                        if (Update_Status(2) > 0)
                        {
                            lblworkorderno.Visible = true;
                            txt_workorderNo.Visible = true;
                            Bind_OrdeItemGrid();
                            WorkOrderPrepare();
                            btnSaveOrder.Text = "GENERATE PDF";
                        }
                    }
                    else if (btnSaveOrder.Text == "GENERATE PDF")
                    {
                        SqlParameter[] spChk = new SqlParameter[] { 
                            new SqlParameter("@workorderno", txt_workorderNo.Text), 
                            new SqlParameter("@Plant", hdnPlant.Value), 
                            new SqlParameter("@revision_no", txt_stockreviseno.Text) 
                        };
                        DataTable dtChk = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_backlog_workorderno", spChk, DataAccess.Return_Type.DataTable);
                        if (dtChk.Rows.Count > 0)
                            lblErrMsg.Text = "Work order no already created on " + dtChk.Rows[0]["CreatedDate"].ToString() + " by " + dtChk.Rows[0]["username"].ToString();
                        else
                        {
                            SqlParameter[] sp1 = new SqlParameter[] { 
                                new SqlParameter("@StockOrderId", Convert.ToInt32(hdnStockorderid.Value)), 
                                new SqlParameter("@WorkOrderNo", txt_workorderNo.Text), 
                                new SqlParameter("@RevisionNo", txt_stockreviseno.Text),
                                new SqlParameter("@userBy", Request.Cookies["TTSUser"].Value)
                            };
                            int resp1 = daCOTS.ExecuteNonQuery_SP("sp_WorkOrderDetail_stockOrdermaster", sp1);
                            if (resp1 > 0)
                            {
                                if (Update_Status(3) > 0)
                                {
                                    btnSaveOrder.Text = "MOVE TO PRODUCTION";
                                    Bind_OrdeItemGrid();
                                    WorkOrderDocPreparation();
                                }
                            }
                        }
                    }
                    else if (btnSaveOrder.Text == "MOVE TO PRODUCTION")
                    {
                        if (Update_Status(4) > 0)
                            Response.Redirect("stockorderprepare.aspx?fid=" + Request["fid"].ToString() + "&sid=" + Utilities.Encrypt(""), false);
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "disOrder", "div_custorder();", true);
                }
                else
                {
                    lblErrMsg.Text = "ADD ATLEAST ONE ITEM..!!!..";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMaster", "div_master();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private int Update_Status(int StatusID)
        {
            int resp = 0;
            SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@StockOrderId", Convert.ToInt32(hdnStockorderid.Value)), 
                new SqlParameter("@StockOrderStatus", StatusID) 
            };
            return resp = daCOTS.ExecuteNonQuery_SP("sp_update_stockOrdermaster_Status", sp);
        }
        private void WorkOrderPrepare()
        {
            try
            {
                SqlParameter[] spDummy = new SqlParameter[] { new SqlParameter("@SID", Convert.ToInt32(hdnStockorderid.Value)), new SqlParameter("@Plant", hdnPlant.Value) };
                DataTable dummyProformaRefno = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_Dummy_WorkOrderNo_StockOrder", spDummy, DataAccess.Return_Type.DataTable);
                if (dummyProformaRefno != null && dummyProformaRefno.Rows.Count > 0)
                {
                    txt_workorderNo.Text = Convert.ToDecimal(dummyProformaRefno.Rows[0]["workorderno"].ToString()).ToString("00000");
                    txt_stockreviseno.Text = Convert.ToDecimal(Convert.ToDecimal(dummyProformaRefno.Rows[0]["revise_no"].ToString())).ToString("00");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void WorkOrderDocPreparation()
        {
            try
            {
                StockOrderPdf.OrderRefNo = txt_StockOrderRefNo.Text;
                StockOrderPdf.StockItemlist = (DataTable)ViewState["StockItemlist"];
                StockOrderPdf.CustomerName = ddl_StockCustomerName.SelectedItem.Text;
                StockOrderPdf.CustStdCode = ddl_StockCustomerName.SelectedItem.Value;
                StockOrderPdf.WorkOrderNo = txt_workorderNo.Text;
                StockOrderPdf.WorkOrderReviseNo = txt_stockreviseno.Text;
                StockOrderPdf.Startdate = txt_StockStartDate.Text;
                StockOrderPdf.Enddate = txt_StockEndDate.Text;
                StockOrderPdf.plant = ddl_plant.SelectedItem.Text;
                StockOrderPdf.SID = Convert.ToInt32(hdnStockorderid.Value);
                string OutputString = StockOrderPdf.WorkOrderCreation();
                if (OutputString == "Success")
                    Response.Redirect(Request.RawUrl.ToString(), false);
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
                string serverUrl = Server.MapPath("~/Stockorderfiles/" + ddl_StockCustomerName.SelectedItem.Value + "/").Replace("TTS", "pdfs");
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
        protected void btnDraft_Click(object sender, EventArgs e)
        {
            Response.Redirect("stockorderprepare.aspx?fid=" + Request["fid"].ToString() + "&sid=" + Utilities.Encrypt(""), false);
        }
    }
}
