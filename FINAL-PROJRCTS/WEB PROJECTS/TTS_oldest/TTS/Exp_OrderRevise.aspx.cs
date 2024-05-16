using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;

namespace TTS
{
    public partial class Exp_OrderRevise : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        private static string strEditProcessID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_revise"].ToString() == "True")
                        {
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_item_change_orders", DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gv_ReviseOrders.DataSource = dtorderlist;
                                gv_ReviseOrders.DataBind();
                            }
                            else
                                lblErrMsgcontent.Text = "No Records";
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
        protected void lnkItemRevise_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow Row = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnCustCode.Value = ((HiddenField)Row.FindControl("hdnCustCode")).Value;
                lblSelectedOrderRefNo.Text = ((Label)Row.FindControl("lblOrderRefNo")).Text;
                lblSelectedCustomerName.Text = ((Label)Row.FindControl("lblStatusCustName")).Text;
                hdnPlant.Value = Row.Cells[5].Text;
                hdnOID.Value = ((HiddenField)Row.FindControl("hdnOrderID")).Value;

                hdnItemChangeStatus.Value = "0";
                Bind_OrdeItemGrid();

                SqlParameter[] spData = new SqlParameter[] { new SqlParameter("@CustCode", ((HiddenField)Row.FindControl("hdnCustCodeStd")).Value) };
                DataTable dtJathagam = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ApprovedList_For_ManualTyreType", spData, DataAccess.Return_Type.DataTable);
                ViewState["dtJathagam"] = dtJathagam;
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Item');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        private void Bind_OrdeItemGrid()
        {
            try
            {
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                lbl_ProcessID.Text = "";
                gv_AddedItems.DataSource = "";
                gv_AddedItems.DataBind();
                lblErrMsg3.Text = "";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_orderitem_list_PPC", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gv_AddedItems.DataSource = dt;
                    gv_AddedItems.DataBind();

                    gv_AddedItems.FooterRow.Cells[7].Text = "Total";
                    gv_AddedItems.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                    gv_AddedItems.FooterRow.Cells[8].Text = dt.Compute("Sum(itemqty)", "").ToString();

                    gv_AddedItems.Columns[10].Visible = false;
                    gv_AddedItems.Columns[11].Visible = false;
                    gv_AddedItems.Columns[12].Visible = false;
                    gv_AddedItems.Columns[13].Visible = false;
                    foreach (DataRow row in dt.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gv_AddedItems.Columns[10].Visible = true;
                        gv_AddedItems.Columns[11].Visible = true;
                        gv_AddedItems.Columns[12].Visible = true;
                        gv_AddedItems.Columns[13].Visible = true;

                        gv_AddedItems.FooterRow.Cells[11].Text = dt.Compute("Sum(Rimitemqty)", "").ToString();
                        break;
                    }
                }
                else
                    lblErrMsg3.Text = "No Items added Yet!!!";
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Item');", true);
                if (hdnItemChangeStatus.Value == "1")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EnableComplete2", "gotoPreviewDiv('divItemChange');", true);
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
                ddl_Platform.DataSource = "";
                ddl_Platform.DataBind();
                ddl_Brand.DataSource = "";
                ddl_Brand.DataBind();
                ddl_Sidwall.DataSource = "";
                ddl_Sidwall.DataBind();
                ddl_Type.DataSource = "";
                ddl_Type.DataBind();
                ddl_Size.DataSource = "";
                ddl_Size.DataBind();
                ddl_RimWidth.DataSource = "";
                ddl_RimWidth.DataBind();
                txt_Weight.Text = "";
                txt_BasicPrice.Text = "";
                lbl_ProcessID.Text = "";
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_DrawingNo.Text = "";
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                if (ddl_Category.SelectedItem.Value != "CHOOSE")
                {
                    string strCategoryValue = ddl_Category.SelectedItem.Value;
                    if (strCategoryValue == "1" || strCategoryValue == "2" || strCategoryValue == "3")
                    {
                        DataTable dt = (DataTable)ViewState["dtJathagam"];
                        DataView dv_Platform = new DataView(dt);
                        dv_Platform.RowFilter = "SizeCategory = '" + ddl_Category.SelectedItem.Text + "'";
                        dv_Platform.Sort = "Config ASC";
                        DataTable dt_platform = dv_Platform.ToTable(true, "Config");
                        if (dt_platform.Rows.Count > 0)
                        {
                            ddl_Platform.DataSource = dt_platform;
                            ddl_Platform.DataTextField = "Config";
                            ddl_Platform.DataValueField = "Config";
                            ddl_Platform.DataBind();
                            if (ddl_Platform.Items.Count == 1)
                                ddl_Platform_SelectedIndexChanged(sender, e);
                            else
                                ddl_Platform.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                        }
                        chk_AddRimAssembly.Enabled = true;
                    }
                    else if (strCategoryValue == "4" || strCategoryValue == "5")
                    {
                        ddl_EdcNo.DataSource = "";
                        ddl_EdcNo.DataBind();
                        frmRimProcessID_Details.DataSource = "";
                        frmRimProcessID_Details.DataBind();

                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_EDCNO_Rim_ProcessID", DataAccess.Return_Type.DataTable);
                        if (dt.Rows.Count > 0)
                        {
                            ddl_EdcNo.DataSource = dt;
                            ddl_EdcNo.DataTextField = "EDCNO";
                            ddl_EdcNo.DataValueField = "EDCNO";
                            ddl_EdcNo.DataBind();
                        }
                        ddl_EdcNo.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                        chk_AddRimAssembly.Enabled = false;
                        ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview", "DisableJathagam_Category();", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "document.getElementById('tr_rimAssembly').style.display='block';", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Platform_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_Brand.DataSource = "";
                ddl_Brand.DataBind();
                ddl_Sidwall.DataSource = "";
                ddl_Sidwall.DataBind();
                ddl_Type.DataSource = "";
                ddl_Type.DataBind();
                ddl_Size.DataSource = "";
                ddl_Size.DataBind();
                ddl_RimWidth.DataSource = "";
                ddl_RimWidth.DataBind();
                txt_Weight.Text = "";
                txt_BasicPrice.Text = "";
                lbl_ProcessID.Text = "";
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_DrawingNo.Text = "";
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                if (ddl_Platform.SelectedItem.Value != "CHOOSE")
                {
                    DataTable dt = (DataTable)ViewState["dtJathagam"];
                    DataView dv_Brand = new DataView(dt);
                    dv_Brand.RowFilter = "SizeCategory = '" + ddl_Category.SelectedItem.Text + "' and Config = '" + ddl_Platform.SelectedItem.Text + "'";
                    dv_Brand.Sort = "Brand ASC";
                    DataTable dt_Brand = dv_Brand.ToTable(true, "Brand");
                    if (dt_Brand.Rows.Count > 0)
                    {
                        ddl_Brand.DataSource = dt_Brand;
                        ddl_Brand.DataTextField = "Brand";
                        ddl_Brand.DataValueField = "Brand";
                        ddl_Brand.DataBind();
                        if (ddl_Brand.Items.Count == 1)
                            ddl_Brand_SelectedIndexChanged(sender, e);
                        else
                            ddl_Brand.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                    }
                }
                if (ddl_Category.SelectedItem.Value == "4" || ddl_Category.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview", "DisableJathagam_Category();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_Sidwall.DataSource = "";
                ddl_Sidwall.DataBind();
                ddl_Type.DataSource = "";
                ddl_Type.DataBind();
                ddl_Size.DataSource = "";
                ddl_Size.DataBind();
                ddl_RimWidth.DataSource = "";
                ddl_RimWidth.DataBind();
                txt_Weight.Text = "";
                txt_BasicPrice.Text = "";
                lbl_ProcessID.Text = "";
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_DrawingNo.Text = "";
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                if (ddl_Brand.SelectedItem.Value != "CHOOSE")
                {
                    DataTable dt = (DataTable)ViewState["dtJathagam"];
                    DataView dv_Sidewall = new DataView(dt);
                    dv_Sidewall.RowFilter = "SizeCategory = '" + ddl_Category.SelectedItem.Text + "' and Config = '" + ddl_Platform.SelectedItem.Text
                                             + "' and Brand = '" + ddl_Brand.SelectedItem.Text + "'";
                    dv_Sidewall.Sort = "Sidewall ASC";
                    DataTable dt_Sidewall = dv_Sidewall.ToTable(true, "Sidewall");
                    if (dt_Sidewall.Rows.Count > 0)
                    {
                        ddl_Sidwall.DataSource = dt_Sidewall;
                        ddl_Sidwall.DataTextField = "Sidewall";
                        ddl_Sidwall.DataValueField = "Sidewall";
                        ddl_Sidwall.DataBind();
                        if (ddl_Sidwall.Items.Count == 1)
                            ddl_Sidwall_SelectedIndexChanged(sender, e);
                        else
                            ddl_Sidwall.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                    }
                }
                if (ddl_Category.SelectedItem.Value == "4" || ddl_Category.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview", "DisableJathagam_Category();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Sidwall_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_Type.DataSource = "";
                ddl_Type.DataBind();
                ddl_Size.DataSource = "";
                ddl_Size.DataBind();
                ddl_RimWidth.DataSource = "";
                ddl_RimWidth.DataBind();
                txt_Weight.Text = "";
                txt_BasicPrice.Text = "";
                lbl_ProcessID.Text = "";
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_DrawingNo.Text = "";
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                if (ddl_Sidwall.SelectedItem.Value != "CHOOSE")
                {
                    DataTable dt = (DataTable)ViewState["dtJathagam"];
                    DataView dv_TyreType = new DataView(dt);
                    dv_TyreType.RowFilter = "SizeCategory = '" + ddl_Category.SelectedItem.Text + "' and Config = '" + ddl_Platform.SelectedItem.Text + "' and Brand = '"
                                             + ddl_Brand.SelectedItem.Text + "' and Sidewall = '" + ddl_Sidwall.SelectedItem.Text + "'";
                    dv_TyreType.Sort = "TyreType ASC";
                    DataTable dt_TyreType = dv_TyreType.ToTable(true, "TyreType");
                    if (dt_TyreType.Rows.Count > 0)
                    {
                        ddl_Type.DataSource = dt_TyreType;
                        ddl_Type.DataTextField = "TyreType";
                        ddl_Type.DataValueField = "TyreType";
                        ddl_Type.DataBind();
                        if (ddl_Type.Items.Count == 1)
                            ddl_Type_SelectedIndexChanged(sender, e);
                        else
                            ddl_Type.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                    }
                }
                if (ddl_Category.SelectedItem.Value == "4" || ddl_Category.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview", "DisableJathagam_Category();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_Size.DataSource = "";
                ddl_Size.DataBind();
                ddl_RimWidth.DataSource = "";
                ddl_RimWidth.DataBind();
                txt_Weight.Text = "";
                txt_BasicPrice.Text = "";
                lbl_ProcessID.Text = "";
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_DrawingNo.Text = "";
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                if (ddl_Type.SelectedItem.Value != "CHOOSE")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                        new SqlParameter("@brand", ddl_Brand.SelectedItem.Text), 
                        new SqlParameter("@sidewall", ddl_Sidwall.SelectedItem.Text), 
                        new SqlParameter("@Tyretype", ddl_Type.SelectedItem.Text) 
                    };
                    DataTable dt_Size = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Size_For_ManualOrder", sp1, DataAccess.Return_Type.DataTable);
                    if (dt_Size.Rows.Count > 0)
                    {
                        DataView dtTypeView = new DataView(dt_Size);
                        DataTable dtDisSize = dtTypeView.ToTable(true, "TyreSize");
                        ddl_Size.DataSource = dtDisSize;
                        ddl_Size.DataTextField = "TyreSize";
                        ddl_Size.DataValueField = "TyreSize";
                        ddl_Size.DataBind();
                        if (ddl_Size.Items.Count == 1)
                            ddl_Size_SelectedIndexChanged(sender, e);
                        else
                            ddl_Size.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                    }
                }
                if (ddl_Category.SelectedItem.Value == "4" || ddl_Category.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview", "DisableJathagam_Category();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Size_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_RimWidth.DataSource = "";
                ddl_RimWidth.DataBind();
                txt_Weight.Text = "";
                txt_BasicPrice.Text = "";
                lbl_ProcessID.Text = "";
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_DrawingNo.Text = "";
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                if (ddl_Size.SelectedItem.Value != "CHOOSE")
                {
                    string strCategoryValue = ddl_Category.SelectedItem.Value;
                    if (strCategoryValue == "1" || strCategoryValue == "2" || strCategoryValue == "3")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                            new SqlParameter("@brand", ddl_Brand.SelectedItem.Text), 
                            new SqlParameter("@sidewall", ddl_Sidwall.SelectedItem.Text), 
                            new SqlParameter("@Tyretype", ddl_Type.SelectedItem.Text), 
                            new SqlParameter("@tyresize", ddl_Size.SelectedItem.Value) 
                        };
                        DataTable dt_Rim = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Rim_ForManualOrder", sp1, DataAccess.Return_Type.DataTable);
                        if (dt_Rim.Rows.Count > 0)
                        {
                            ddl_RimWidth.DataSource = dt_Rim;
                            ddl_RimWidth.DataTextField = "RimSize";
                            ddl_RimWidth.DataValueField = "RimSize";
                            ddl_RimWidth.DataBind();
                            if (ddl_RimWidth.Items.Count == 1)
                                ddl_RimWidth_SelectedIndexChanged(sender, e);
                            else
                                ddl_RimWidth.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                        }
                    }
                    if (ddl_Category.SelectedItem.Value == "4" || ddl_Category.SelectedItem.Value == "5")
                        ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview5", "DisableJathagam_Category();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_RimWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txt_Weight.Enabled = true;
                txt_Weight.Text = "";
                txt_BasicPrice.Text = "";
                lbl_ProcessID.Text = "";
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_DrawingNo.Text = "";
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                if (ddl_RimWidth.SelectedItem.Value != "CHOOSE")
                {
                    string strCategoryValue = ddl_Category.SelectedItem.Value;
                    if (strCategoryValue == "1" || strCategoryValue == "2" || strCategoryValue == "3")
                    {
                        SqlParameter[] sp = new SqlParameter[] { 
                            new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                            new SqlParameter("@brand", ddl_Brand.SelectedItem.Text), 
                            new SqlParameter("@sidewall", ddl_Sidwall.SelectedItem.Text), 
                            new SqlParameter("@Tyretype", ddl_Type.SelectedItem.Text), 
                            new SqlParameter("@tyresize", ddl_Size.SelectedItem.Value), 
                            new SqlParameter("@rimsize", ddl_RimWidth.SelectedItem.Text), 
                            new SqlParameter("@custcode", hdnCustCode.Value)
                        };
                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_unitprice", sp, DataAccess.Return_Type.DataTable);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow priceRow = dt.Rows[0];
                            lbl_ProcessID.Text = priceRow["ProcessID"].ToString() != "" ? priceRow["ProcessID"].ToString() : "";
                            txt_Weight.Text = priceRow["FinishedWt"].ToString();
                            txt_BasicPrice.Text = priceRow["BasicPrice"].ToString() == "" ? "0" : priceRow["BasicPrice"].ToString();
                        }
                        else
                        {
                            SqlParameter[] sp1 = new SqlParameter[] { 
                                new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                                new SqlParameter("@brand", ddl_Brand.SelectedItem.Text), 
                                new SqlParameter("@sidewall", ddl_Sidwall.SelectedItem.Text), 
                                new SqlParameter("@Tyretype", ddl_Type.SelectedItem.Text), 
                                new SqlParameter("@tyresize", ddl_Size.SelectedItem.Value), 
                                new SqlParameter("@TyreRim", ddl_RimWidth.SelectedItem.Text) 
                            };
                            DataTable dt1 = (DataTable)daTTS.ExecuteReader_SP("sp_sel_OtherDetails_ForManualOrder", sp1, DataAccess.Return_Type.DataTable);
                            if (dt1.Rows.Count > 0)
                            {
                                DataRow oRow = dt1.Rows[0];
                                lbl_ProcessID.Text = oRow["ProcessID"].ToString() != "" ? oRow["ProcessID"].ToString() : "";
                                txt_Weight.Text = oRow["Finished"].ToString();
                                lblErrMsg3.Text = "unit price not available in published price-sheet list<br/> you can enter manual price sheet value";
                            }
                        }

                        ddl_EdcNo.DataSource = "";
                        ddl_EdcNo.DataBind();
                        frmRimProcessID_Details.DataSource = "";
                        frmRimProcessID_Details.DataBind();

                        SqlParameter[] spEdc = new SqlParameter[] { 
                            new SqlParameter("@TyreSize", ddl_Size.SelectedItem.Text), 
                            new SqlParameter("@Rimsize", ddl_RimWidth.SelectedItem.Text) 
                        };
                        DataTable dtEDC = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_EDCNO_Order_TyresizeWise", spEdc, DataAccess.Return_Type.DataTable);
                        if (dtEDC.Rows.Count > 0)
                        {
                            ddl_EdcNo.DataSource = dtEDC;
                            ddl_EdcNo.DataTextField = "EDCNO";
                            ddl_EdcNo.DataValueField = "EDCNO";
                            ddl_EdcNo.DataBind();
                            ddl_EdcNo.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));

                            chk_AddRimAssembly.Enabled = true;
                            chk_AddRimAssembly.Checked = false;
                        }
                    }
                    else if (strCategoryValue == "4" || strCategoryValue == "5")
                        ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview", "DisableJathagam_Category();", true);
                    if (txt_Weight.Text != "")
                        txt_Weight.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void chk_AddRimAssembly_Changed(object sender, EventArgs e)
        {
            if (chk_AddRimAssembly.Checked)
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "document.getElementById('tr_rimAssembly').style.display='block';", true);
            else if (!chk_AddRimAssembly.Checked)
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('tr_rimAssembly').style.display='none';", true);
        }
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                string strProcess = lbl_ProcessID.Text != "" ? lbl_ProcessID.Text : "M" + hdnCustCode.Value + "-" + gv_AddedItems.Rows.Count.ToString();
                int itemcount = 0;
                bool rimAssmly = false;
                if (btnAddItem.Text == "UPDATE QTY")
                    rimAssmly = Convert.ToBoolean(hdnAssyStatus.Value);
                else
                    rimAssmly = chk_AddRimAssembly.Checked ? true : false;
                string strPlatform = string.Empty, strBrand = string.Empty, strSidewall = string.Empty, strType = string.Empty, strSize = string.Empty, strRim = string.Empty;
                if (ddl_Category.SelectedValue.ToString() == "1" || ddl_Category.SelectedValue.ToString() == "2" || ddl_Category.SelectedValue.ToString() == "3")
                {
                    strPlatform = ddl_Platform.SelectedItem.Text;
                    strBrand = ddl_Brand.SelectedItem.Text;
                    strSidewall = ddl_Sidwall.SelectedItem.Text;
                    strType = ddl_Type.SelectedItem.Text;
                    strSize = ddl_Size.SelectedItem.Text;
                    strRim = ddl_RimWidth.SelectedItem.Text;
                }
                string strEDC = (ddl_EdcNo.SelectedItem != null && ddl_EdcNo.SelectedItem.Text != "CHOOSE") ? ddl_EdcNo.SelectedItem.Text : "";

                if (strEditProcessID == "")
                {
                    SqlParameter[] spExists = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@category", ddl_Category.SelectedItem.Text),
                        new SqlParameter("@Config", strPlatform),
                        new SqlParameter("@brand", strBrand),
                        new SqlParameter("@sidewall", strSidewall),
                        new SqlParameter("@tyretype", strType),
                        new SqlParameter("@tyresize", strSize),
                        new SqlParameter("@rimsize", strRim),
                        new SqlParameter("@AssyRimstatus", rimAssmly),
                        new SqlParameter("@EdcNo", strEDC)
                    };
                    itemcount = (int)daCOTS.ExecuteScalar_SP("sp_chk_ItemExists", spExists);
                }
                if (itemcount == 0)
                {
                    SqlParameter[] sp = new SqlParameter[22];
                    sp[0] = new SqlParameter("@O_ID", hdnOID.Value);
                    sp[1] = new SqlParameter("@Category", ddl_Category.SelectedItem.Text);
                    sp[2] = new SqlParameter("@Config", strPlatform);
                    sp[3] = new SqlParameter("@Brand", strBrand);
                    sp[4] = new SqlParameter("@Sidewall", strSidewall);
                    sp[5] = new SqlParameter("@TyreType", strType);
                    sp[6] = new SqlParameter("@TyreSize", strSize);
                    sp[7] = new SqlParameter("@RimSize", strRim);
                    sp[8] = new SqlParameter("@ItemQtry", txt_Quantity.Text);
                    sp[9] = new SqlParameter("@UnitPrice", txt_BasicPrice.Text != "" ? Convert.ToDecimal(txt_BasicPrice.Text) : Convert.ToDecimal("0.00"));
                    sp[10] = new SqlParameter("@FinishedWt", txt_Weight.Text != "" ? Convert.ToDecimal(txt_Weight.Text) : Convert.ToDecimal("0.00"));
                    sp[11] = new SqlParameter("@Discount", Convert.ToDecimal("0.00"));
                    sp[12] = new SqlParameter("@SheetPrice", Convert.ToDecimal("0.00"));
                    sp[13] = new SqlParameter("@RimUnitPrce", (rimAssmly || strEDC != "") && txt_RimPrice.Text != "" ? Convert.ToDecimal(txt_RimPrice.Text) : Convert.ToDecimal("0.00"));
                    sp[14] = new SqlParameter("@RimItemQty", (rimAssmly || strEDC != "") ? txt_Quantity.Text : "0");
                    sp[15] = new SqlParameter("@RimFinishedWt", (rimAssmly || strEDC != "") && txt_RimWeight.Text != "" ? Convert.ToDecimal(txt_RimWeight.Text) : Convert.ToDecimal("0.00"));
                    sp[16] = new SqlParameter("@AssyRimStatus", rimAssmly);
                    sp[17] = new SqlParameter("@RimDwg", (rimAssmly || strEDC != "") && txt_DrawingNo.Text != "" ? txt_DrawingNo.Text : "");
                    sp[18] = new SqlParameter("@ProcessId", strProcess);
                    sp[19] = new SqlParameter("@PrevProcessID", strEditProcessID);
                    sp[20] = new SqlParameter("@AdditionalInfo", txtAddInfo.Text);
                    sp[21] = new SqlParameter("@EdcNo", strEDC);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_UPD_Exp_OrderItemList_Revise", sp);

                    if (resp > 0)
                    {
                        SqlParameter[] sp2 = new SqlParameter[]{
                            new SqlParameter("@O_ID", hdnOID.Value),
                            new SqlParameter("@Preview", hdn_Quantity.Value),
                            new SqlParameter("@Revise", txt_Quantity.Text),
                            new SqlParameter("@ReviseType", strEditProcessID != "" ? "Qty revised" : "Item Added"),
                            new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                            new SqlParameter("@ProcessID", strEditProcessID != "" ? strEditProcessID :
                            (lbl_ProcessID.Text != "" ? lbl_ProcessID.Text : "M" + hdnCustCode.Value + "-" + gv_AddedItems.Rows.Count.ToString()))
                        };
                        daCOTS.ExecuteNonQuery_SP("sp_ins_OrderRevisedHistory", sp2);

                        hdnItemChangeStatus.Value = "1";
                    }
                    Bind_OrdeItemGrid();

                    txt_Quantity.Text = "";
                    hdn_Quantity.Value = "";
                    txt_RimPrice.Text = "";
                    txt_RimWeight.Text = "";
                    txt_DrawingNo.Text = "";
                    txt_BasicPrice.Text = "";
                    lbl_ProcessID.Text = "";
                    txt_Weight.Enabled = true;
                    txt_Weight.Text = "";
                    txtAddInfo.Text = "";
                    ddl_Size.SelectedIndex = 0;
                    ddl_RimWidth.SelectedIndex = 0;
                    ddl_EdcNo.DataSource = "";
                    ddl_EdcNo.DataBind();
                    frmRimProcessID_Details.DataSource = "";
                    frmRimProcessID_Details.DataBind();
                    strEditProcessID = "";
                    btnAddItem.Text = "ADD ITEM";
                }
                else
                    lblErrMsg3.Text = "Already item added, you should delete the previous item and add new";
                ScriptManager.RegisterStartupScript(Page, GetType(), "CtrlDisable", "CrtlEnableDispable(0);", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Item');", true);
                chk_AddRimAssembly.Enabled = false;
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
                GridViewRow row = gv_AddedItems.Rows[e.RowIndex];
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@ProcessId", ((HiddenField)row.FindControl("hdn_ProcessID")).Value),
                    new SqlParameter("@AssyRimStatus", ((HiddenField)row.FindControl("hdnAssyStatus")).Value ),
                    new SqlParameter("@EdcNo", ((Label)row.FindControl("lblEdcNo")).Text)
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_del_vs2_Orderitemlist_exp_revise", sp);

                SqlParameter[] sp2 = new SqlParameter[]{
                    new SqlParameter("@O_ID", hdnCustCode.Value),
                    new SqlParameter("@Preview", ""),
                    new SqlParameter("@Revise", hdnPlant.Value),
                    new SqlParameter("@ReviseType", "Item Deleted"),
                    new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@ProcessID", ((HiddenField)row.FindControl("hdn_ProcessID")).Value)
                };
                daCOTS.ExecuteNonQuery_SP("sp_ins_OrderRevisedHistory", sp2);

                if (resp > 0)
                {
                    hdnItemChangeStatus.Value = "1";
                    Bind_OrdeItemGrid();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnEditItem_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((Button)sender).NamingContainer as GridViewRow;
                strEditProcessID = ((HiddenField)row.FindControl("hdn_ProcessID")).Value;
                ddl_Category.SelectedIndex = ddl_Category.Items.IndexOf(ddl_Category.Items.FindByText(((Label)row.FindControl("lblCategory")).Text));
                ddl_Category_SelectedIndexChanged(sender, e);
                if (((Label)row.FindControl("lblCategory")).Text != "SPLIT RIMS" && ((Label)row.FindControl("lblCategory")).Text != "POB WHEEL")
                {
                    ddl_Platform.SelectedIndex = ddl_Platform.Items.IndexOf(ddl_Platform.Items.FindByValue(row.Cells[1].Text));
                    ddl_Platform_SelectedIndexChanged(sender, e);
                    ddl_Brand.SelectedIndex = ddl_Brand.Items.IndexOf(ddl_Brand.Items.FindByValue(row.Cells[2].Text));
                    ddl_Brand_SelectedIndexChanged(sender, e);
                    ddl_Sidwall.SelectedIndex = ddl_Sidwall.Items.IndexOf(ddl_Sidwall.Items.FindByValue(row.Cells[3].Text));
                    ddl_Sidwall_SelectedIndexChanged(sender, e);
                    ddl_Type.SelectedIndex = ddl_Type.Items.IndexOf(ddl_Type.Items.FindByValue(row.Cells[4].Text));
                    ddl_Type_SelectedIndexChanged(sender, e);
                    ddl_Size.SelectedIndex = ddl_Size.Items.IndexOf(ddl_Size.Items.FindByValue(row.Cells[5].Text));
                    ddl_Size_SelectedIndexChanged(sender, e);
                    ddl_RimWidth.SelectedIndex = ddl_RimWidth.Items.IndexOf(ddl_RimWidth.Items.FindByValue(row.Cells[6].Text));
                    ddl_RimWidth_SelectedIndexChanged(sender, e);
                }
                txt_BasicPrice.Text = row.Cells[7].Text;
                txt_Quantity.Text = row.Cells[8].Text;
                hdn_Quantity.Value = row.Cells[8].Text;
                txt_Weight.Text = row.Cells[9].Text;
                txtAddInfo.Text = ((Label)row.FindControl("lblAdditionalInfo")).Text;

                if (Convert.ToBoolean(((HiddenField)row.FindControl("hdnAssyStatus")).Value) == true || ((Label)row.FindControl("lblCategory")).Text == "SPLIT RIMS" ||
                    ((Label)row.FindControl("lblCategory")).Text == "POB WHEEL")
                {
                    chk_AddRimAssembly.Checked = Convert.ToBoolean(((HiddenField)row.FindControl("hdnAssyStatus")).Value);
                    btnAddItem.Text = "UPDATE QTY";
                    hdnAssyStatus.Value = Convert.ToBoolean(((HiddenField)row.FindControl("hdnAssyStatus")).Value).ToString();
                    txt_DrawingNo.Text = ((HiddenField)row.FindControl("hdn_RimDwg")).Value;
                    ddl_EdcNo.SelectedIndex = ddl_EdcNo.Items.IndexOf(ddl_EdcNo.Items.FindByValue(((Label)row.FindControl("lblEdcNo")).Text));

                    txt_RimPrice.Text = ((Label)row.FindControl("lblRimUnitPrice")).Text;
                    txt_RimWeight.Text = ((Label)row.FindControl("lbltotalRimWt")).Text;

                    ddl_EdcNo_SelectedIndexChanged(null, null);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "document.getElementById('tr_rimAssembly').style.display='block';", true);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Item');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "CtrlDisable1", "CrtlEnableDispable(1);", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnItemChangeCompleted_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_exp_itemchange_completed", sp);
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_EdcNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                frmRimProcessID_Details.DataSource = "";
                frmRimProcessID_Details.DataBind();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@EDCNO", ddl_EdcNo.SelectedValue) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_sel_Rim_ProcessID_Details_ForEdcNo", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    frmRimProcessID_Details.DataSource = dt;
                    frmRimProcessID_Details.DataBind();

                    txt_RimWeight.Text = dt.Rows[0]["RimWt"].ToString();
                }
                if (ddl_Category.SelectedItem.Value == "4" || ddl_Category.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview7", "DisableJathagam_Category();", true);
                else
                    chk_AddRimAssembly_Changed(null, null);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}