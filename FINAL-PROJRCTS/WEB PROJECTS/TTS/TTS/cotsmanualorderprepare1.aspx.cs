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
    public partial class cotsmanualorderprepare1 : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_orderentry"].ToString() == "True")
                        {
                            if ((string)daCOTS.ExecuteScalar_SP("sp_chk_pricesheet_expireddate") == "")
                            {
                                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                                DataTable dtMasterList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_vs2_OrderMasterdetails", sp, DataAccess.Return_Type.DataTable);
                                if (dtMasterList.Rows.Count > 0)
                                {
                                    hdnCustCode.Value = dtMasterList.Rows[0]["custcode"].ToString();
                                    hdnOrderRefNo.Value = dtMasterList.Rows[0]["orderrefno"].ToString();

                                    frmOrderMasterDetails.DataSource = dtMasterList;
                                    frmOrderMasterDetails.DataBind();

                                    Bind_OrdeItemGrid(hdnCustCode.Value, hdnOrderRefNo.Value);
                                    rdo_GradeSelection.SelectedIndex = rdo_GradeSelection.Items.IndexOf(rdo_GradeSelection.Items.FindByValue(dtMasterList.Rows[0]["grade"].ToString()));
                                }

                                SqlParameter[] spData = new SqlParameter[] { new SqlParameter("@CustCode", "DE0048") };
                                DataTable dtJathagam = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ApprovedList_For_ManualTyreType", spData, DataAccess.Return_Type.DataTable);
                                ViewState["dtJathagam"] = dtJathagam;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "PRICE SHEET ALREADY EXPIRED. PLEASE CONTACT YOUR ADMINISTRATOR";
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
        public string Bind_BillingAddress(string BillID)
        {
            try
            {
                string strAddress = string.Empty;
                DataTable dtAddressList = DomesticScots.Bind_BillingAddress(BillID);
                if (dtAddressList.Rows.Count > 0)
                {
                    DataRow row = dtAddressList.Rows[0];
                    strAddress += "<div style='font-weight:bold;'>M/S. " + row["CompanyName"].ToString() + "</div>";
                    strAddress += "<div>" + row["shipaddress"].ToString().Replace("~", "<br/>") + "</div>";
                    strAddress += "<div>" + row["city"].ToString() + ", " + row["statename"].ToString() + "</div>";
                    strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
                    strAddress += "<div>" + row["contact_name"].ToString() + "</div>";
                    strAddress += "<div>" + row["EmailID"].ToString() + " / " + row["mobile"].ToString() + "</div>";
                    strAddress += "<div>GST: " + row["GST_No"].ToString() + "</div>";
                }
                return strAddress;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
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
                txt_SheetPrice.Text = "";
                txt_BasicPrice.Text = "";
                txt_Discount.Text = "";
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
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('tr_rimAssembly').style.display='none';", true);
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
                txt_SheetPrice.Text = "";
                txt_BasicPrice.Text = "";
                txt_Discount.Text = "";
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
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview1", "DisableJathagam_Category();", true);
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
                txt_SheetPrice.Text = "";
                txt_BasicPrice.Text = "";
                txt_Discount.Text = "";
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
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview2", "DisableJathagam_Category();", true);
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
                txt_SheetPrice.Text = "";
                txt_BasicPrice.Text = "";
                txt_Discount.Text = "";
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
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview3", "DisableJathagam_Category();", true);
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
                txt_SheetPrice.Text = "";
                txt_BasicPrice.Text = "";
                txt_Discount.Text = "";
                lbl_ProcessID.Text = "";
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_DrawingNo.Text = "";
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                if (ddl_Type.SelectedItem.Value != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                        new SqlParameter("@brand", ddl_Brand.SelectedItem.Text), 
                        new SqlParameter("@sidewall", ddl_Sidwall.SelectedItem.Text), 
                        new SqlParameter("@Tyretype", ddl_Type.SelectedItem.Text), 
                        new SqlParameter("@custId", hdnCustCode.Value), 
                        new SqlParameter("@grade", rdo_GradeSelection.SelectedItem.Value) 
                    };
                    DataTable dt_Discount = (DataTable)daCOTS.ExecuteReader_SP("SP_GET_TypeWiseDiscount_Discount", sp, DataAccess.Return_Type.DataTable);
                    txt_Discount.Text = dt_Discount.Rows[0]["Discount"].ToString();
                    hdnDiscount.Value = dt_Discount.Rows[0]["Discount"].ToString();

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
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview4", "DisableJathagam_Category();", true);
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
                txt_SheetPrice.Text = "";
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
                txt_SheetPrice.Text = "";
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
                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_applicable_unitprice", sp, DataAccess.Return_Type.DataTable);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow priceRow = dt.Rows[0];
                            lbl_ProcessID.Text = priceRow["ProcessID"].ToString() != "" ? priceRow["ProcessID"].ToString() : "";
                            txt_Weight.Text = priceRow["FinishedWt"].ToString();
                            txt_SheetPrice.Text = priceRow["UnitPrice"].ToString() == "" ? "0" : priceRow["UnitPrice"].ToString();
                            txt_BasicPrice.Text = priceRow["BasicPrice"].ToString() == "" ? "0" : priceRow["BasicPrice"].ToString();
                            txt_Discount.Text = priceRow["Discount"].ToString() == "" ? "0" : priceRow["Discount"].ToString();
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
                                lblErrMsg1.Text = "unit price not available in published price-sheet list<br/> you can enter manual price sheet value";
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
                    if (ddl_Category.SelectedItem.Value == "4" || ddl_Category.SelectedItem.Value == "5")
                        ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview6", "DisableJathagam_Category();", true);
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
                //To add items to gridview
                lblErrMsg2.Text = "";
                bool rimAssmly = chk_AddRimAssembly.Checked ? true : false;
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
                SqlParameter[] spExists = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
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
                int itemcount = (int)daCOTS.ExecuteScalar_SP("sp_chk_ItemExists", spExists);
                if (itemcount > 0)
                    lblErrMsg2.Text = "Item Already Added. You can change the qty or delete this item";
                else
                {
                    SqlParameter[] sp = new SqlParameter[23];
                    sp[0] = new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString()));
                    sp[1] = new SqlParameter("@Category", ddl_Category.SelectedItem.Text);
                    sp[2] = new SqlParameter("@Config", strPlatform);
                    sp[3] = new SqlParameter("@Brand", strBrand);
                    sp[4] = new SqlParameter("@Sidewall", strSidewall);
                    sp[5] = new SqlParameter("@TyreType", strType);
                    sp[6] = new SqlParameter("@TyreSize", strSize);
                    sp[7] = new SqlParameter("@RimSize", strRim);
                    sp[8] = new SqlParameter("@ItemQty", txt_Quantity.Text);
                    sp[9] = new SqlParameter("@UnitPrice", txt_BasicPrice.Text != "" ? Convert.ToDecimal(txt_BasicPrice.Text) : Convert.ToDecimal("0.00"));
                    sp[10] = new SqlParameter("@FinishedWt", txt_Weight.Text != "" ? Convert.ToDecimal(txt_Weight.Text) : Convert.ToDecimal("0.00"));
                    sp[11] = new SqlParameter("@Discount", txt_Discount.Text != "" ? Convert.ToDecimal(txt_Discount.Text) : Convert.ToDecimal("0.00"));
                    sp[12] = new SqlParameter("@SheetPrice", txt_SheetPrice.Text != "" ? Convert.ToDecimal(txt_SheetPrice.Text) : Convert.ToDecimal("0.00"));
                    sp[13] = new SqlParameter("@RimUnitPrce", (rimAssmly || strEDC != "") && txt_RimPrice.Text != "" ? Convert.ToDecimal(txt_RimPrice.Text) : Convert.ToDecimal("0.00"));
                    sp[14] = new SqlParameter("@RimItemQty", (rimAssmly || strEDC != "") ? txt_Quantity.Text : "0");
                    sp[15] = new SqlParameter("@RimFinishedWt", (rimAssmly || strEDC != "") && txt_RimWeight.Text != "" ? Convert.ToDecimal(txt_RimWeight.Text) : Convert.ToDecimal("0.00"));
                    sp[16] = new SqlParameter("@AssyRimStatus", rimAssmly);
                    sp[17] = new SqlParameter("@RimDwg", (rimAssmly || strEDC != "") && txt_DrawingNo.Text != "" ? txt_DrawingNo.Text : "");
                    sp[18] = new SqlParameter("@ProcessId", lbl_ProcessID.Text != "" ? lbl_ProcessID.Text : "M" + hdnCustCode.Value + "-" + gv_Addeditems.Rows.Count.ToString());
                    sp[19] = new SqlParameter("@Grade", rdo_GradeSelection.SelectedValue.ToString());
                    sp[20] = new SqlParameter("@ItemCode", "");
                    sp[21] = new SqlParameter("@AdditionalInfo", "");
                    sp[22] = new SqlParameter("@EdcNo", strEDC);

                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_vs2_OrderItemList", sp);
                    if (resp > 0)
                        Bind_OrdeItemGrid(hdnCustCode.Value, hdnOrderRefNo.Value);
                }
                txt_Quantity.Text = "";
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_DrawingNo.Text = "";
                txt_BasicPrice.Text = "";
                txt_SheetPrice.Text = "";
                lbl_ProcessID.Text = "";
                txt_Weight.Enabled = true;
                txt_Weight.Text = "";
                ddl_Size.SelectedIndex = 0;
                ddl_RimWidth.SelectedIndex = 0;
                ddl_EdcNo.DataSource = "";
                ddl_EdcNo.DataBind();
                frmRimProcessID_Details.DataSource = "";
                frmRimProcessID_Details.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrdeItemGrid(string CustCode, string OrderRefNo)
        {
            try
            {
                chk_AddRimAssembly.Checked = false;
                chk_AddRimAssembly.Enabled = false;
                lbl_ProcessID.Text = "";
                gv_Addeditems.DataSource = "";
                gv_Addeditems.DataBind();
                lblErrMsg3.Text = "";
                DataTable dt = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())));
                if (dt.Rows.Count > 0)
                {
                    gv_Addeditems.DataSource = dt;
                    gv_Addeditems.DataBind();

                    gv_Addeditems.FooterRow.Cells[9].Text = "Total";
                    gv_Addeditems.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                    gv_Addeditems.FooterRow.Cells[10].Text = dt.Compute("Sum(itemqty)", "").ToString();
                    gv_Addeditems.FooterRow.Cells[11].Text = Convert.ToDecimal(dt.Compute("Sum(unitpricepdf)", "")).ToString();
                    gv_Addeditems.FooterRow.Cells[12].Text = Convert.ToDecimal(dt.Compute("Sum(totalfwt)", "")).ToString();

                    gv_Addeditems.Columns[13].Visible = false;
                    gv_Addeditems.Columns[14].Visible = false;
                    gv_Addeditems.Columns[15].Visible = false;
                    gv_Addeditems.Columns[16].Visible = false;
                    gv_Addeditems.Columns[17].Visible = false;
                    foreach (DataRow row in dt.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gv_Addeditems.Columns[13].Visible = true;
                        gv_Addeditems.Columns[14].Visible = true;
                        gv_Addeditems.Columns[15].Visible = true;
                        gv_Addeditems.Columns[16].Visible = true;
                        gv_Addeditems.Columns[17].Visible = true;

                        gv_Addeditems.FooterRow.Cells[14].Text = dt.Compute("Sum(Rimitemqty)", "").ToString();
                        gv_Addeditems.FooterRow.Cells[15].Text = Convert.ToDecimal(dt.Compute("Sum(Rimpricepdf)", "")).ToString();
                        gv_Addeditems.FooterRow.Cells[16].Text = Convert.ToDecimal(dt.Compute("Sum(totalRimWt)", "")).ToString();
                        break;
                    }
                }
                else
                    lblErrMsg3.Text = "No Items added Yet!!!";
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
                HiddenField hdnProcessID = (HiddenField)row.FindControl("hdn_ProcessID");
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                    new SqlParameter("@ProcessId", hdnProcessID.Value),
                    new SqlParameter("@AssyRimStatus", ((HiddenField)row.FindControl("hdnAssyStatus")).Value ),
                    new SqlParameter("@EdcNo", ((Label)row.FindControl("lblEdcNo")).Text)
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_del_vs2_Orderitemlist", sp);
                if (resp > 0)
                    Bind_OrdeItemGrid(hdnCustCode.Value, hdnOrderRefNo.Value);
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
                Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnMoveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_OrderEntryComplete", sp);
                if (resp > 0)
                    Response.Redirect("cotsmanualorderprepare.aspx", false);
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