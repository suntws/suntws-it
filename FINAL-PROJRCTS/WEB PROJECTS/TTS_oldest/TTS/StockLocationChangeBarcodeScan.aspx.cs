using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace TTS
{
    public partial class StockLocationChangeBarcodeScan : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        List<string> lstLocations = new List<string> { "CHOOSE", "FACTORY", "GODOWN", "LATHE" };
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        lblPageTitle.Text = "STOCK LOCATION CHANGING " + (Utilities.Decrypt(Request["action"].ToString()) == "dc" ? "DC PREPARE (" : (Utilities.Decrypt(Request["action"].ToString()) != "prepare" ? ((Utilities.Decrypt(Request["action"].ToString()).ToUpper()) + " BARCODE (") : "ORDER PREPARE (")) + Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + ")";
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["stock_report"].ToString() == "True")
                        {
                            if (Utilities.Decrypt(Request["action"].ToString()) == "prepare")
                            {
                                ddlLocatFrom.DataSource = lstLocations;
                                ddlLocatFrom.DataBind();
                                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
                            }
                            else
                            {
                                bindGv();
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }


        protected void ddlLocatFrom_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlLocatTo.DataSource = "";
                ddlLocatTo.DataBind();
                ddlPLatform.DataSource = "";
                ddlPLatform.DataBind();
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();
                ddlType.DataSource = "";
                ddlType.DataBind();
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtQty.Text = "";
                lblErrMsgcontent.Text = "";
                btnComplete.Style.Add("display", "none");
                grdStockChangeDetails.DataSource = null;
                grdStockChangeDetails.DataBind();

                if (ddlLocatFrom.SelectedIndex > 0)
                {
                    ddlLocatTo.DataSource = lstLocations.Where(a => a != ddlLocatFrom.SelectedItem.Text).ToList();
                    ddlLocatTo.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlLocatTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPLatform.DataSource = "";
                ddlPLatform.DataBind();
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();
                ddlType.DataSource = "";
                ddlType.DataBind();
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtQty.Text = "";
                lblErrMsgcontent.Text = "";
                btnClear.Visible = false;
                btnSave.Text = "SAVE";

                if (ddlLocatTo.SelectedIndex > 0)
                {
                    DataSet ds = bindDdl("", "", "", "", "", "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlPLatform.DataSource = ds.Tables[0];
                        ddlPLatform.DataTextField = "Config";
                        ddlPLatform.DataValueField = "Config";
                        ddlPLatform.DataBind();
                        ddlPLatform.Items.Insert(0, "CHOOSE");
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ddlPLatform.SelectedIndex = 1;
                            ddlPlatform_indexchanged(null, null);
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        hdnMasterId.Value = ds.Tables[1].Rows[0][0].ToString();
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltShow", "document.getElementById('divFilters').style.display='block';", true);
                    bindGridview();
                    ddlLocatFrom.Attributes.Add("disabled", "disabled");
                    ddlLocatTo.Attributes.Add("disabled", "disabled");
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlPlatform_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlPLatform.Attributes.Remove("disabled");
                ddlBrand.Attributes.Remove("disabled");
                ddlSidewall.Attributes.Remove("disabled");
                ddlType.Attributes.Remove("disabled");
                ddlSize.Attributes.Remove("disabled");
                ddlRim.Attributes.Remove("disabled");
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();
                ddlType.DataSource = "";
                ddlType.DataBind();
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtQty.Text = "";
                lblErrMsgcontent.Text = "";
                btnClear.Visible = false;
                btnSave.Text = "SAVE";


                if (ddlPLatform.SelectedIndex > 0)
                {
                    btnClear.Visible = true;
                    DataSet ds = bindDdl(ddlPLatform.SelectedItem.Text, "", "", "", "", "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlBrand.DataSource = ds.Tables[0];
                        ddlBrand.DataTextField = "Brand";
                        ddlBrand.DataValueField = "Brand";
                        ddlBrand.DataBind();
                        ddlBrand.Items.Insert(0, "CHOOSE");
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ddlBrand.SelectedIndex = 1;
                            ddlBrand_indexchanged(null, null);
                        }
                    }
                    else
                    {
                        lblErrMsgcontent.Text = "NO BRAND AVAILABLE FOR THE SELECTED PLATFORM";
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltShow1", "document.getElementById('divFilters').style.display='block';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlBrand_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();
                ddlType.DataSource = "";
                ddlType.DataBind();
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtQty.Text = "";
                lblErrMsgcontent.Text = "";

                if (ddlBrand.SelectedIndex > 0)
                {

                    DataSet ds = bindDdl(ddlPLatform.SelectedItem.Text, ddlBrand.SelectedItem.Text, "", "", "", "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlSidewall.DataSource = ds.Tables[0];
                        ddlSidewall.DataTextField = "Sidewall";
                        ddlSidewall.DataValueField = "Sidewall";
                        ddlSidewall.DataBind();
                        ddlSidewall.Items.Insert(0, "CHOOSE");
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ddlSidewall.SelectedIndex = 1;
                            ddlSidewall_indexchanged(null, null);
                        }
                    }
                    else
                    {
                        lblErrMsgcontent.Text = "NO SIDEWALL AVAILABLE FOR THE SELECTED CATEGORIES";
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltShow2", "document.getElementById('divFilters').style.display='block';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlSidewall_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlType.DataSource = "";
                ddlType.DataBind();
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtQty.Text = "";
                lblErrMsgcontent.Text = "";

                if (ddlSidewall.SelectedIndex > 0)
                {
                    DataSet ds = bindDdl(ddlPLatform.SelectedItem.Text, ddlBrand.SelectedItem.Text, ddlSidewall.SelectedItem.Text, "", "", "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlType.DataSource = ds.Tables[0];
                        ddlType.DataTextField = "TyreType";
                        ddlType.DataValueField = "TyreType";
                        ddlType.DataBind();
                        ddlType.Items.Insert(0, "CHOOSE");
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ddlType.SelectedIndex = 1;
                            ddlType_indexchanged(null, null);
                        }
                    }
                    else
                    {
                        lblErrMsgcontent.Text = "NO TYRE TYPE AVAILABLE FOR THE SELECTED CATEGORIES";
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltShow3", "document.getElementById('divFilters').style.display='block';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlType_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtQty.Text = "";
                lblErrMsgcontent.Text = "";

                if (ddlType.SelectedIndex > 0)
                {
                    DataSet ds = bindDdl(ddlPLatform.SelectedItem.Text, ddlBrand.SelectedItem.Text, ddlSidewall.SelectedItem.Text, ddlType.SelectedItem.Text, "", "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlSize.DataSource = ds.Tables[0];
                        ddlSize.DataTextField = "TyreSize";
                        ddlSize.DataValueField = "TyreSize";
                        ddlSize.DataBind();
                        ddlSize.Items.Insert(0, "CHOOSE");
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ddlSize.SelectedIndex = 1;
                            ddlSize_indexchanged(null, null);
                        }
                    }
                    else
                    {
                        lblErrMsgcontent.Text = "NO TYRE SIZE AVAILABLE FOR THE SELECTED CATEGORIES";
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltShow4", "document.getElementById('divFilters').style.display='block';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlSize_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtQty.Text = "";
                lblErrMsgcontent.Text = "";

                if (ddlSize.SelectedIndex > 0)
                {
                    DataSet ds = bindDdl(ddlPLatform.SelectedItem.Text, ddlBrand.SelectedItem.Text, ddlSidewall.SelectedItem.Text, ddlType.SelectedItem.Text, ddlSize.SelectedItem.Text, "");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlRim.DataSource = ds.Tables[0];
                        ddlRim.DataTextField = "TyreRim";
                        ddlRim.DataValueField = "TyreRim";
                        ddlRim.DataBind();
                        ddlRim.Items.Insert(0, "CHOOSE");
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            ddlRim.SelectedIndex = 1;
                            ddlRim_indexchanged(null, null);
                        }
                    }
                    else
                    {
                        lblErrMsgcontent.Text = "NO RIM SIZE AVAILABLE FOR THE SELECTED CATEGORIES";
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltShow5", "document.getElementById('divFilters').style.display='block';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlRim_indexchanged(object sender, EventArgs e)
        {
            try
            {
                txtQty.Text = "";
                lblErrMsgcontent.Text = "";
                if (ddlRim.SelectedIndex > 0)
                {
                    DataSet ds = bindDdl(ddlPLatform.SelectedItem.Text, ddlBrand.SelectedItem.Text, ddlSidewall.SelectedItem.Text, ddlType.SelectedItem.Text, ddlSize.SelectedItem.Text, ddlRim.SelectedItem.Text);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtQty.Text = ds.Tables[0].Rows[0]["qty"].ToString();
                            hdnItemId.Value = ds.Tables[0].Rows[0]["item_ID"].ToString();

                            ddlPLatform.Attributes.Add("disabled", "disabled");
                            ddlBrand.Attributes.Add("disabled", "disabled");
                            ddlSidewall.Attributes.Add("disabled", "disabled");
                            ddlType.Attributes.Add("disabled", "disabled");
                            ddlSize.Attributes.Add("disabled", "disabled");
                            ddlRim.Attributes.Add("disabled", "disabled");

                            btnSave.Text = "UPDATE";
                        }
                    }
                    txtQty.Focus();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltShow6", "document.getElementById('divFilters').style.display='block';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsgcontent.Text = "";
                int itemId = (hdnItemId.Value == "" ? 0 : Convert.ToInt32(hdnItemId.Value));

                SqlParameter[] spSave = new SqlParameter[] {
                    new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()),
                    new SqlParameter("@fromPlace",ddlLocatFrom.SelectedItem.Text),
                    new SqlParameter("@toPlace",ddlLocatTo.SelectedItem.Text),
                    new SqlParameter("@platform",ddlPLatform.SelectedItem.Text),
                    new SqlParameter("@brand",ddlBrand.SelectedItem.Text),
                    new SqlParameter("@sidewall",ddlSidewall.SelectedItem.Text),
                    new SqlParameter("@tyreType",ddlType.SelectedItem.Text),
                    new SqlParameter("@tyreSize",ddlSize.SelectedItem.Text),
                    new SqlParameter("@rimSize",ddlRim.SelectedItem.Text),
                    new SqlParameter("@itemQty",txtQty.Text),
                    new SqlParameter("@user",Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@masterId",Convert.ToInt32(hdnMasterId.Value)),
                    new SqlParameter("@ItemId",itemId),
                    new SqlParameter("@action",btnSave.Text)
                };

                int res = daCOTS.ExecuteNonQuery_SP("sp_ins_stockLocationChange_items", spSave);

                if (res > 0)
                {
                    ddlPLatform.SelectedIndex = 0;
                    ddlPlatform_indexchanged(null, null);
                    bindGridview();
                }

                btnSave.Text = "SAVE";
                hdnItemId.Value = "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
                SqlParameter[] spComplete = new SqlParameter[]{
                    new SqlParameter("@plant",Utilities.Decrypt(Request["pid"].ToString()).ToUpper()),
                    new SqlParameter("@fromPlace",ddlLocatFrom.SelectedItem.Text),
                    new SqlParameter("@toPlace",ddlLocatTo.SelectedItem.Text),
                    new SqlParameter("@user",Request.Cookies["TTSUser"].Value)
                };

                int res = daCOTS.ExecuteNonQuery_SP("sp_upd_stocklocateChange_Order_Complete", spComplete);
                if (res > 0)
                {
                    Response.Write("<script>alert('ORDER COMPLETED AND SAVED SUCCESSFULLY!');</script>");
                    ddlLocatFrom.Attributes.Remove("disabled");
                    ddlLocatTo.Attributes.Remove("disabled");
                    ddlLocatFrom.SelectedIndex = 0;
                    ddlLocatFrom_indexchanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltHide", "document.getElementById('divFilters').style.display='none';", true);
                ddlPLatform.SelectedIndex = 0;
                ddlPlatform_indexchanged(null, null);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindGridview()
        {
            try
            {
                grdStockChangeDetails.DataSource = "";
                grdStockChangeDetails.DataBind();

                SqlParameter[] spGrid = new SqlParameter[]{
                    new SqlParameter("@plant",Utilities.Decrypt(Request["pid"].ToString()).ToUpper()),
                    new SqlParameter("@fromPlace",ddlLocatFrom.SelectedItem.Text),
                    new SqlParameter("@toPlace",ddlLocatTo.SelectedItem.Text),
                    new SqlParameter("@user",Request.Cookies["TTSUser"].Value)
                };

                DataTable dtGrid = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stocklocateChange_Items_details", spGrid, DataAccess.Return_Type.DataTable);
                if (dtGrid != null && dtGrid.Rows.Count > 0)
                {
                    grdStockChangeDetails.DataSource = dtGrid;
                    grdStockChangeDetails.DataBind();

                    grdStockChangeDetails.FooterRow.Cells[6].Text = "TOTAL";
                    grdStockChangeDetails.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                    grdStockChangeDetails.FooterRow.Cells[7].Text = dtGrid.Compute("Sum(qty)", "").ToString();

                    btnComplete.Style.Add("display", "inline-block");
                }
                else
                {
                    btnComplete.Style.Add("display", "none");
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private DataSet bindDdl(string platform, string brand, string sidewall, string type, string size, string rim)
        {
            try
            {

                SqlParameter[] sp1 = new SqlParameter[] {
                    new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()),
                    new SqlParameter("@fromPlace",ddlLocatFrom.SelectedItem.Text),
                    new SqlParameter("@toPlace",ddlLocatTo.SelectedItem.Text),
                    new SqlParameter("@platform",platform),
                    new SqlParameter("@brand",brand),
                    new SqlParameter("@sidewall",sidewall),
                    new SqlParameter("@tyretype",type),
                    new SqlParameter("@tyresize",size),
                    new SqlParameter("@rimsize",rim),
                    new SqlParameter("@createdBy",Request.Cookies["TTSUser"].Value)
                };
                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_StockLocateChange_Categories", sp1, DataAccess.Return_Type.DataSet);

                return ds;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                DataSet ds1 = null;
                return ds1;
            }
        }
        protected void grdStockChangeDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
                GridViewRow clickRow = grdStockChangeDetails.Rows[e.RowIndex];
                int ItemId = Convert.ToInt32(((HiddenField)clickRow.FindControl("hdn_ItemID")).Value);
                SqlParameter[] spDel = new SqlParameter[]{
                    new SqlParameter("@itemId",ItemId)
                };
                int res = daCOTS.ExecuteNonQuery_SP("sp_del_StockLocateChange_items", spDel);
                if (res > 0)
                {
                    bindGridview();
                    ddlPLatform.SelectedIndex = 0;
                    ddlPlatform_indexchanged(null, null);
                }
                else
                {
                    Response.Write("<script>alert('Sorry! There is a problem with server. Kindly try again later.');</script>");
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltShow7", "document.getElementById('divFilters').style.display='block';", true);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void grdStockChangeDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hdnItemId.Value = (grdStockChangeDetails.SelectedRow.Cells[8].FindControl("hdn_ItemID") as HiddenField).Value;

                ddlPLatform.SelectedIndex = ddlPLatform.Items.IndexOf(ddlPLatform.Items.FindByText(grdStockChangeDetails.SelectedRow.Cells[1].Text.Trim()));
                ddlPlatform_indexchanged(null, null);
                if (ddlBrand.Items.Count > 2)
                {
                    ddlBrand.SelectedIndex = ddlBrand.Items.IndexOf(ddlBrand.Items.FindByText(grdStockChangeDetails.SelectedRow.Cells[2].Text.Trim()));
                    ddlBrand_indexchanged(null, null);
                }
                if (ddlSidewall.Items.Count > 2)
                {
                    ddlSidewall.SelectedIndex = ddlSidewall.Items.IndexOf(ddlSidewall.Items.FindByText(grdStockChangeDetails.SelectedRow.Cells[3].Text.Trim()));
                    ddlSidewall_indexchanged(null, null);
                }
                if (ddlType.Items.Count > 2)
                {
                    ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByText(grdStockChangeDetails.SelectedRow.Cells[4].Text.Trim()));
                    ddlType_indexchanged(null, null);
                }
                if (ddlSize.Items.Count > 2)
                {
                    ddlSize.SelectedIndex = ddlSize.Items.IndexOf(ddlSize.Items.FindByText(grdStockChangeDetails.SelectedRow.Cells[5].Text.Trim()));
                    ddlSize_indexchanged(null, null);
                }
                if (ddlRim.Items.Count > 2)
                    ddlRim.SelectedItem.Text = grdStockChangeDetails.SelectedRow.Cells[6].Text.Trim();

                txtQty.Text = grdStockChangeDetails.SelectedRow.Cells[7].Text.Trim();

                ddlPLatform.Attributes.Add("disabled", "disabled");
                ddlBrand.Attributes.Add("disabled", "disabled");
                ddlSidewall.Attributes.Add("disabled", "disabled");
                ddlType.Attributes.Add("disabled", "disabled");
                ddlSize.Attributes.Add("disabled", "disabled");
                ddlRim.Attributes.Add("disabled", "disabled");

                btnSave.Text = "UPDATE";

                ScriptManager.RegisterStartupScript(Page, GetType(), "divFiltShow8", "document.getElementById('divFilters').style.display='block';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }


        private void bindGv()
        {
            try
            {
                txtDcno.Text = "";
                txtVehicleNo.Text = "";
                txtRemarks.Text = "";
                lblScannedQty.Text = "0";
                lblTotalQty.Text = "0";
                txtBarcode.Text = "";
                lblBarcode.Text = "";
                lblErrMsgcontent.Text = "";
                gvLocatChangeOrders.DataSource = "";
                gvLocatChangeOrders.DataBind();
                gvOrderItems.DataSource = "";
                gvOrderItems.DataBind();

                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@plant",Utilities.Decrypt(Request["pid"].ToString()).ToUpper()),
                    new SqlParameter("@qaction",Utilities.Decrypt(Request["action"].ToString()))
                };

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stockLocationChange_orderDetails", sp, DataAccess.Return_Type.DataTable);

                if (dt != null && dt.Rows.Count > 0)
                {
                    gvLocatChangeOrders.DataSource = dt;
                    gvLocatChangeOrders.DataBind();
                }
                else
                {
                    lblErrMsgcontent.Text = "NO RECORDS";
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkOrderView_Click(object sender, EventArgs e)
        {
            try
            {
                gvOrderItems.DataSource = "";
                gvOrderItems.DataBind();
                lblErrMsgcontent.Text = "";

                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnMasterId.Value = ((HiddenField)clickedRow.FindControl("hdnMaster_Id")).Value;

                SqlParameter[] spItems = new SqlParameter[]{
                    new SqlParameter("@masterId",Convert.ToInt32(hdnMasterId.Value)),
                    new SqlParameter("@qaction",Utilities.Decrypt(Request["action"].ToString()))
                };

                DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stockLocationChange_Items_Details", spItems, DataAccess.Return_Type.DataTable);

                if (dtItems != null && dtItems.Rows.Count > 0)
                {
                    lblTotalQty.Text = dtItems.AsEnumerable().Sum(x => x.Field<int>("qty")).ToString();
                    gvOrderItems.DataSource = dtItems;
                    gvOrderItems.DataBind();
                    gvOrderItems.Columns[gvOrderItems.Columns.Count - 1].Visible = false;
                    gvOrderItems.Columns[8].Visible = false;

                    if (Utilities.Decrypt(Request["action"].ToString()) == "dc")
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "btnorderCompshow", "document.getElementById('tblComplete').style.display='block';", true);
                    }
                    else if (Utilities.Decrypt(Request["action"].ToString()) == "scan")
                    {
                        btnBarcodeCheck_Click(sender, e);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JMainDiv1", "gotoPreviewDiv('divItemDetails');", true);
                    }
                    else if (Utilities.Decrypt(Request["action"].ToString()) == "delete")
                    {
                        gvOrderItems.Columns[gvOrderItems.Columns.Count - 1].Visible = true;
                        gvOrderItems.Columns[8].Visible = true;
                    }
                }
                else
                {
                    lblErrMsgcontent.Text = "NO RECORDS";
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkDelOrder_Click(object sender, EventArgs e)
        {
            try
            {
                gv_ScanedList.DataSource = "";
                gv_ScanedList.DataBind();
                lblErrMsgcontent.Text = "";

                GridViewRow clickRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnprocessId.Value = ((HiddenField)clickRow.FindControl("hdnProcess_Id")).Value;

                SqlParameter[] spDel = new SqlParameter[]{
                    new SqlParameter("@masterId",Convert.ToInt32(hdnMasterId.Value)),
                    new SqlParameter("@processId",hdnprocessId.Value)
                };

                DataTable dtBarList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stocklocateChange_itemBarcodeList", spDel, DataAccess.Return_Type.DataTable);

                if (dtBarList != null && dtBarList.Rows.Count > 0)
                {
                    hdnItemId.Value = dtBarList.Rows[0][3].ToString();
                    gv_ScanedList.DataSource = dtBarList;
                    gv_ScanedList.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "divDelShow", "gotoPreviewDiv('divDelBarcode');", true);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDelBarcode_click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_Barcode = new DataTable();
                dt_Barcode.Columns.Add("barcode", typeof(string));
                foreach (GridViewRow Row in gv_ScanedList.Rows)
                {
                    CheckBox chk = Row.FindControl("chk_selectQty") as CheckBox;
                    if (chk.Checked && chk.Enabled)
                        dt_Barcode.Rows.Add(Row.Cells[0].Text);
                }
                if (dt_Barcode.Rows.Count > 0)
                {
                    SqlParameter[] spDelItems = new SqlParameter[]{
                        new SqlParameter("@barcodeDt",dt_Barcode),
                        new SqlParameter("@itemId",Convert.ToInt32(hdnItemId.Value))
                    };

                    int res = daCOTS.ExecuteNonQuery_SP("sp_del_stocklocatechange_barcode", spDelItems);

                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "delShow", "alert('SELECTED BARCODES ARE DELETED SUCCESSFULLY!');", true);
                        bindGv();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnBarcodeCheck_Click(object sender, EventArgs e)
        {
            try
            {
                txtLoadScanStatus.Text = "";
                lblBarcode.Text = "";
                lblScannedQty.Text = "0";

                SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@masterID", Convert.ToInt32(hdnMasterId.Value)),
                        new SqlParameter("@barcode",txtBarcode.Text)
                    };

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_locateChange_BarcodeCheck", sp, DataAccess.Return_Type.DataTable);

                if (dt != null && dt.Rows.Count > 0)
                {
                    lblScannedQty.Text = dt.Rows[0]["qty"].ToString();
                }

                if (txtBarcode.Text != "" && txtBarcode.Text.Length >= 19)
                {
                    if (dt.Rows[0]["result"].ToString() == "SCAN OK")
                    {
                        txtLoadScanStatus.Text = "SCAN OK";
                        txtLoadScanStatus.Style.Add("color", "#11c728");
                    }
                    else
                    {
                        txtLoadScanStatus.Text = dt.Rows[0]["result"].ToString();
                        txtLoadScanStatus.Style.Add("color", "#c7112a");
                    }
                    lblBarcode.Text = txtBarcode.Text;
                }
                if (lblScannedQty.Text != "" && lblTotalQty.Text == lblScannedQty.Text)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "isCompleted();", true);
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JMainDiv2", "gotoPreviewDiv('divItemDetails');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv1", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveDc_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spDc = new SqlParameter[]{
                    new SqlParameter("@masterID", Convert.ToInt32(hdnMasterId.Value)),
                    new SqlParameter("@dcNo",txtDcno.Text),
                    new SqlParameter("@complUser",Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@vehicleNo",txtVehicleNo.Text.ToUpper()),
                    new SqlParameter("@remarks",txtRemarks.Text),
                    new SqlParameter("@qaction",Utilities.Decrypt(Request["action"].ToString()))
                };

                int resp = (int)daCOTS.ExecuteNonQuery_SP("sp_upd_stockLocateChange_DCcreate", spDc);

                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "disMainDiv", "disDivShowHide('" + Utilities.Decrypt(Request["action"].ToString()) + "');", true);
                    if (Utilities.Decrypt(Request["action"].ToString()) == "dc")
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DCShow", "alert('DC SUCCESSFULLY GENERATED!');", true);
                    else if (Utilities.Decrypt(Request["action"].ToString()) == "scan")
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DCShow", "alert('ORDER MOVED FOR DC PREPARATION!');", true);
                    bindGv();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}