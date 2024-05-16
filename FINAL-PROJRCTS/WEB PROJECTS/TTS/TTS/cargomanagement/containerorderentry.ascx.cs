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

namespace TTS.cargomanagement
{
    public partial class containerorderentry : System.Web.UI.UserControl
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
                        SqlParameter[] sp1 = new SqlParameter[1];
                        sp1[0] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                        DataTable dtUser = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_UserLevel", sp1, DataAccess.Return_Type.DataTable);
                        if (dtUser.Rows.Count > 0)
                        {
                            if (dtUser.Rows[0]["cotsexpcargomanagement"].ToString() == "True")
                            {
                                if (Request["vid"].ToString() == "0")
                                {
                                    DataTable dtCustList = new DataTable();
                                    dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsCargoCustomer", DataAccess.Return_Type.DataTable);
                                    if (dtCustList.Rows.Count > 0)
                                    {
                                        ddlCargoCustomer.DataSource = dtCustList;
                                        ddlCargoCustomer.DataTextField = "custfullname";
                                        ddlCargoCustomer.DataValueField = "custcode";
                                        ddlCargoCustomer.DataBind();
                                        ddlCargoCustomer.Items.Insert(0, "CHOOSE");
                                    }
                                    create_Datatable_ForManual();
                                }
                                else if (Request["vid"].ToString() == "0e")
                                {

                                }
                            }
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
        private void create_Datatable_ForManual()
        {
            try
            {
                DataTable dtManual = new DataTable();
                DataColumn col = new DataColumn("category", typeof(System.String));
                dtManual.Columns.Add(col);
                col = new DataColumn("processid", typeof(System.String));
                dtManual.Columns.Add(col);
                col = new DataColumn("itemqty", typeof(System.Int32));
                dtManual.Columns.Add(col);
                col = new DataColumn("finishedwt", typeof(System.Decimal));
                dtManual.Columns.Add(col);
                col = new DataColumn("loadingwt", typeof(System.Decimal));
                dtManual.Columns.Add(col);
                col = new DataColumn("sizeposition", typeof(System.Int32));
                dtManual.Columns.Add(col);
                col = new DataColumn("typeposition", typeof(System.Int32));
                dtManual.Columns.Add(col);
                col = new DataColumn("Config", typeof(System.String));
                dtManual.Columns.Add(col);
                col = new DataColumn("tyresize", typeof(System.String));
                dtManual.Columns.Add(col);
                col = new DataColumn("rimsize", typeof(System.String));
                dtManual.Columns.Add(col);
                col = new DataColumn("tyretype", typeof(System.String));
                dtManual.Columns.Add(col);
                col = new DataColumn("brand", typeof(System.String));
                dtManual.Columns.Add(col);
                col = new DataColumn("sidewall", typeof(System.String));
                dtManual.Columns.Add(col);
                col = new DataColumn("Rimitemqty", typeof(System.Int32));
                dtManual.Columns.Add(col);
                col = new DataColumn("Rimfinishedwt", typeof(System.Decimal));
                dtManual.Columns.Add(col);
                col = new DataColumn("RIMSTATUS", typeof(System.Boolean));
                dtManual.Columns.Add(col);
                ViewState["dtManual"] = dtManual;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCargoCustomer_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custname", ddlCargoCustomer.SelectedItem.Text);
                DataTable dtClaimUserID = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claimCustomer_userID", sp1, DataAccess.Return_Type.DataTable);
                if (dtClaimUserID.Rows.Count > 0)
                {
                    ddlCargoUserID.DataSource = dtClaimUserID;
                    ddlCargoUserID.DataTextField = "username";
                    ddlCargoUserID.DataValueField = "ID";
                    ddlCargoUserID.DataBind();
                    if (dtClaimUserID.Rows.Count > 1)
                        ddlCargoUserID.Items.Insert(0, "CHOOSE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        protected void ddlCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlPlatform.DataSource = "";
                ddlPlatform.DataBind();
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@category", ddlCategory.SelectedItem.Value);
                DataTable dtAppType = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Jathagam_For_ContainerPlan", sp1, DataAccess.Return_Type.DataTable);

                if (dtAppType.Rows.Count > 0)
                {
                    ViewState["dtAppType"] = dtAppType;
                    DataView dtTypeView = new DataView(dtAppType);
                    dtTypeView.Sort = "Config ASC";
                    DataTable disConfig = dtTypeView.ToTable(true, "Config");
                    if (disConfig.Rows.Count > 0)
                    {
                        ddlPlatform.DataSource = disConfig;
                        ddlPlatform.DataTextField = "Config";
                        ddlPlatform.DataValueField = "Config";
                        ddlPlatform.DataBind();
                        if (disConfig.Rows.Count == 1)
                            ddlPlatform_IndexChange(sender, e);
                        else
                            ddlPlatform.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
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
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                DataTable dtAppType = ViewState["dtAppType"] as DataTable;
                if (dtAppType.Rows.Count > 0)
                {
                    //DataTable dt = new DataTable();
                    //DataColumn dmaincol = new DataColumn("Brand", typeof(System.String));
                    //dt.Columns.Add(dmaincol);

                    //foreach (DataRow row in dtAppType.Select("Config='" + ddlPlatform.SelectedItem.Text + "'"))
                    //{
                    //    dt.Rows.Add(row["Brand"].ToString());
                    //}

                    DataView dtTypeView = new DataView(dtAppType);
                    dtTypeView.RowFilter = "Config = '" + ddlPlatform.SelectedItem.Text + "'";
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
                            ddlBrand.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                    }
                }
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
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();

                DataTable dtAppType = ViewState["dtAppType"] as DataTable;
                if (dtAppType.Rows.Count > 0)
                {
                    //DataTable dt = new DataTable();
                    //DataColumn dmaincol = new DataColumn("Sidewall", typeof(System.String));
                    //dt.Columns.Add(dmaincol);

                    //foreach (DataRow row in dtAppType.Select("Config='" + ddlPlatform.SelectedItem.Text + "' and Brand='" + ddlBrand.SelectedItem.Text + "'"))
                    //{
                    //    dt.Rows.Add(row["Sidewall"].ToString());
                    //}

                    DataView dtTypeView = new DataView(dtAppType);
                    dtTypeView.RowFilter = "Config = '" + ddlPlatform.SelectedItem.Text + "' and Brand='" + ddlBrand.SelectedItem.Text + "'";
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
                            ddlSidewall.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                    }
                }
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
                ddlType.DataSource = "";
                ddlType.DataBind();

                DataTable dtAppType = ViewState["dtAppType"] as DataTable;
                if (dtAppType.Rows.Count > 0)
                {
                    //DataTable dt = new DataTable();
                    //DataColumn dmaincol = new DataColumn("TyreType", typeof(System.String));
                    //dt.Columns.Add(dmaincol);

                    //foreach (DataRow row in dtAppType.Select("Config='" + ddlPlatform.SelectedItem.Text + "' and Brand='" + ddlBrand.SelectedItem.Text + "' and Sidewall='" + ddlSidewall.SelectedItem.Text + "'"))
                    //{
                    //    dt.Rows.Add(row["TyreType"].ToString());
                    //}

                    DataView dtTypeView = new DataView(dtAppType);
                    dtTypeView.RowFilter = "Config = '" + ddlPlatform.SelectedItem.Text + "' and Brand='" + ddlBrand.SelectedItem.Text + "' and Sidewall='" + ddlSidewall.SelectedItem.Text + "'";
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
                            ddlType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                    }
                }
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
                        ddlSize.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                }
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
                        ddlRim.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                }
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
                txtQty.Text = "";
                txtFinishedWt.Text = "";
                lblProcessID.Text = "";
                hdnLoadingQty.Value = "";
                hdnSizePosition.Value = "";
                hdnTypePosition.Value = "";

                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                sp1[4] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);
                sp1[5] = new SqlParameter("@rimsize", ddlRim.SelectedItem.Text);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Cargo_ProcessIDMaster", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    DataRow priceRow = dt.Rows[0];
                    txtFinishedWt.Text = priceRow["FinishedWt"].ToString();
                    lblProcessID.Text = priceRow["ProcessID"].ToString();

                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                    sp2[1] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);
                    sp2[2] = new SqlParameter("@rimsize", ddlRim.SelectedItem.Text);
                    dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_position", sp2, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow positionRow = dt.Rows[0];
                        hdnLoadingQty.Value = positionRow["LoadingQty"].ToString();
                        hdnSizePosition.Value = positionRow["SizePosition"].ToString();
                        hdnTypePosition.Value = positionRow["TypePosition"].ToString();
                    }
                }
                if (dt.Rows.Count == 0 || lblProcessID.Text == "")
                {
                    lblProcessID.Text = "M-" + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second;
                }
                chkrimassmbly.Checked = false;
                txtRimWt.Text = "";
                txtRimQty.Text = "";
                txtQty.Text = "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnAddNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool chkRimAssy = chkrimassmbly.Checked == true ? true : false;

                bool rowExist = false;
                DataTable dtManual = ViewState["dtManual"] as DataTable;
                foreach (DataRow row in dtManual.Select("category='" + ddlCategory.SelectedItem.Text + "' and Config='" + ddlPlatform.SelectedItem.Text + "' and tyresize='" + ddlSize.SelectedItem.Text + "' and rimsize='" + ddlRim.SelectedItem.Text + "' and tyretype='" + ddlType.SelectedItem.Text + "' and brand='" + ddlBrand.SelectedItem.Text + "' and sidewall='" + ddlSidewall.SelectedItem.Text + "' and RIMSTATUS='" + chkRimAssy + "'"))
                {
                    rowExist = true;
                }
                if (rowExist)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JEMsg2", "bind_errmsg('Item already added. You can change the qty or delete this item');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "show_ManualOrderCtrl('divManualItemList');", true);
                }
                else if (!rowExist)
                {
                    DataRow nRow = dtManual.NewRow();
                    nRow["category"] = ddlCategory.SelectedItem.Text;
                    nRow["processid"] = lblProcessID.Text;
                    nRow["itemqty"] = txtQty.Text;
                    nRow["finishedwt"] = txtFinishedWt.Text;
                    nRow["loadingwt"] = hdnLoadingQty.Value != "" ? hdnLoadingQty.Value : "0.1";
                    nRow["sizeposition"] = hdnSizePosition.Value != "" ? hdnSizePosition.Value : "0";
                    nRow["typeposition"] = hdnTypePosition.Value != "" ? hdnTypePosition.Value : "0";
                    nRow["Config"] = ddlPlatform.SelectedItem.Text;
                    nRow["tyresize"] = ddlSize.SelectedItem.Text;
                    nRow["rimsize"] = ddlRim.SelectedItem.Text;
                    nRow["tyretype"] = ddlType.SelectedItem.Text;
                    nRow["brand"] = ddlBrand.SelectedItem.Text;
                    nRow["sidewall"] = ddlSidewall.SelectedItem.Text;
                    nRow["Rimfinishedwt"] = txtRimWt.Text != "" ? txtRimWt.Text : "0.00";
                    nRow["Rimitemqty"] = txtRimQty.Text != "" ? txtRimQty.Text : "0";
                    nRow["RIMSTATUS"] = chkrimassmbly.Checked == true ? true : false;
                    dtManual.Rows.Add(nRow);
                    hdnSizePosition.Value = "";
                    hdnTypePosition.Value = "";
                    Build_ManualOrderItem(dtManual);
                }
                txtQty.Text = "";
                txtRimQty.Text = "";
                txtRimWt.Text = "";
                txtFinishedWt.Text = "";
                hdnLoadingQty.Value = "";
                hdnSizePosition.Value = "";
                hdnTypePosition.Value = "";
                ddlSize.SelectedIndex = 0;
                ddlRim.SelectedIndex = 0;
                lblProcessID.Text = "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_ManualOrderItem(DataTable dtManual)
        {
            gv_ManualOrderList.DataSource = null;
            gv_ManualOrderList.DataBind();
            lblTotQTy.Text = "";
            lblTotWt.Text = "";
            if (dtManual.Rows.Count > 0)
            {
                gv_ManualOrderList.DataSource = dtManual;
                gv_ManualOrderList.DataBind();
                ViewState["dtManual"] = dtManual;
                object sumQty;
                sumQty = dtManual.Compute("Sum(itemqty)", "");
                lblTotQTy.Text = sumQty.ToString();
                decimal decTotWt = 0;
                foreach (DataRow row in dtManual.Rows)
                {
                    decTotWt += (Convert.ToDecimal(row["itemqty"].ToString()) * Convert.ToDecimal(row["finishedwt"].ToString())) + (Convert.ToDecimal(row["Rimitemqty"].ToString()) * Convert.ToDecimal(row["Rimfinishedwt"].ToString()));
                }
                lblTotWt.Text = decTotWt.ToString();
                chkrimassmbly.Checked = false;
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "show_ManualOrderCtrl('divManualMasterData');", true);

                bool AssyRim = false;
                foreach (DataRow row in dtManual.Select("RIMSTATUS='True'"))
                {
                    AssyRim = true;
                    break;
                }
                if (!AssyRim)
                {
                    gv_ManualOrderList.Columns[9].Visible = false;
                    gv_ManualOrderList.Columns[10].Visible = false;
                }
                else
                {
                    gv_ManualOrderList.Columns[9].Visible = true;
                    gv_ManualOrderList.Columns[10].Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "show_ManualOrderCtrl('divManualItemList');", true);
        }
        protected void gv_ManualOrderList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtManual"] as DataTable;
                gv_ManualOrderList.EditIndex = e.NewEditIndex;
                Build_ManualOrderItem(dtQuoteItem);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_ManualOrderList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtManual"] as DataTable;
                GridViewRow row = gv_ManualOrderList.Rows[e.RowIndex];
                HiddenField hdnprocessid = row.FindControl("hdnprocessid") as HiddenField;
                TextBox txtGItemQty = row.FindControl("txtGItemQty") as TextBox;

                TextBox txtGRimQty = row.FindControl("txtGRimQty") as TextBox;
                TextBox txtGRimFwt = row.FindControl("txtGRimFwt") as TextBox;
                if (hdnprocessid.Value != "" && txtGItemQty.Text != "")
                {
                    if (dtQuoteItem.Rows.Count > 0)
                    {
                        foreach (DataRow iRow in dtQuoteItem.Select("ProcessID='" + hdnprocessid.Value + "'"))
                        {
                            iRow["itemqty"] = Convert.ToInt32(txtGItemQty.Text);
                            if (txtGRimQty.Text != "" && txtGRimFwt.Text != "")
                            {
                                if (Convert.ToInt32(txtGItemQty.Text) >= Convert.ToInt32(txtGRimQty.Text))
                                    iRow["Rimitemqty"] = Convert.ToInt32(txtGRimQty.Text);
                                iRow["Rimfinishedwt"] = Convert.ToDecimal(txtGRimFwt.Text);
                            }
                        }
                    }
                }
                gv_ManualOrderList.EditIndex = -1;
                Build_ManualOrderItem(dtQuoteItem);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_ManualOrderList_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtManual"] as DataTable;
                e.Cancel = true;
                gv_ManualOrderList.EditIndex = -1;
                Build_ManualOrderItem(dtQuoteItem);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_ManualOrderList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtManual"] as DataTable;
                GridViewRow row = gv_ManualOrderList.Rows[e.RowIndex];
                HiddenField hdnprocessid = row.FindControl("hdnprocessid") as HiddenField;
                if (hdnprocessid.Value != "")
                {
                    if (dtQuoteItem.Rows.Count > 0)
                    {
                        foreach (DataRow iRow in dtQuoteItem.Select("ProcessID='" + hdnprocessid.Value + "'"))
                        {
                            iRow.Delete();
                        }
                        dtQuoteItem.AcceptChanges();
                    }
                    Build_ManualOrderItem(dtQuoteItem);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCargoOrderSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@custcode", ddlCargoUserID.SelectedItem.Value);
                sp2[1] = new SqlParameter("@orderrefno", txtCargoOrderNo.Text);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_cargo_orderrefno", sp2, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0 && dt.Rows[0]["OrderRefNo"].ToString() == txtCargoOrderNo.Text && dt.Rows[0]["OrderRefNo"].ToString() != "")
                    lblErrMsg.Text = "ORDER NO ALREADY EXISTING";
                else
                {
                    DataTable dtManual = ViewState["dtManual"] as DataTable;
                    if (dtManual.Rows.Count > 0)
                    {
                        SqlParameter[] sp1 = new SqlParameter[5];
                        sp1[0] = new SqlParameter("@CustCode", ddlCargoUserID.SelectedItem.Value);
                        sp1[1] = new SqlParameter("@OrderRefNo", txtCargoOrderNo.Text.Trim());
                        sp1[2] = new SqlParameter("@SplIns", txtCargoSplIns.Text.Replace("\r\n", "~"));
                        sp1[3] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                        sp1[4] = new SqlParameter("@Plant", ddlCargoPlant.SelectedItem.Text);
                        daCOTS.ExecuteNonQuery_SP("sp_ins_cargo_ordermasterdetails", sp1);

                        SqlParameter[] sp3 = new SqlParameter[4];
                        sp3[0] = new SqlParameter("@orderid", txtCargoOrderNo.Text.Trim());
                        sp3[1] = new SqlParameter("@custcode", ddlCargoUserID.SelectedItem.Value);
                        sp3[2] = new SqlParameter("@cargo_orderitem_dt", dtManual);
                        sp3[3] = new SqlParameter("@ItemPlant", ddlCargoPlant.SelectedItem.Text);
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_cargo_orderitems", sp3);
                        if (resp > 0)
                            Response.Redirect("cargo_management.aspx?vid=1", false);
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