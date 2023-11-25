using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace TTS
{
    public partial class cotsInvoicePriceReport : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    gvItemPriceList.DataSource = null;
                    gvItemPriceList.DataBind();
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dom_orderentry"].ToString() == "True" || dtUser.Rows[0]["dom_proforma"].ToString() == "True"
                            || dtUser.Rows[0]["dom_invoice"].ToString() == "True" || dtUser.Rows[0]["dom_paymentconfirm"].ToString() == "True" || dtUser.Rows[0]["dom_pricechange"].ToString() == "True"))
                        {
                            DataTable dtCustList = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_InvoiceItemPriceReport_Custname", DataAccess.Return_Type.DataTable);
                            if (dtCustList.Rows.Count > 0)
                            {
                                ddlCotsCustName.DataSource = dtCustList;
                                ddlCotsCustName.DataTextField = "custfullname";
                                ddlCotsCustName.DataValueField = "custfullname";
                                ddlCotsCustName.DataBind();
                                ddlCotsCustName.Items.Insert(0, "Choose");
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

        protected void ddlCotsCustName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlLoginUserName.DataSource = "";
                ddlLoginUserName.DataBind();
                //hdnFullName.Value = ddlCotsCustName.SelectedItem.Text;
                ListItem selectedListItem = ddlCotsCustName.Items.FindByText("" + ddlCotsCustName.SelectedItem.Value + "");
                if (selectedListItem != null)
                {
                    ddlCotsCustName.Items.FindByText(ddlCotsCustName.SelectedItem.Text).Selected = false;
                    selectedListItem.Selected = true;
                }
                if (ddlCotsCustName.SelectedItem.Text != "Choose")
                {
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@custfullname", ddlCotsCustName.SelectedItem.Text);
                    DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_InvoiceItemPriceReport_CustUserId", sp1, DataAccess.Return_Type.DataTable);
                    if (dtUserList.Rows.Count > 0)
                    {
                        ddlLoginUserName.DataSource = dtUserList;
                        ddlLoginUserName.DataTextField = "username";
                        ddlLoginUserName.DataValueField = "CustCode";
                        ddlLoginUserName.DataBind();
                        if (dtUserList.Rows.Count == 1)
                            ddlLoginUserName_IndexChange(sender, e);
                        else
                            ddlLoginUserName.Items.Insert(0, "Choose");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlLoginUserName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custcode", ddlLoginUserName.SelectedItem.Value);
                DataTable dtCategories = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_InvoiceItemPriceReport_Categories", sp1, DataAccess.Return_Type.DataTable);
                ViewState["dtCategories"] = dtCategories;
                if (dtCategories.Rows.Count > 0)
                {
                    List<string> lstCategory = dtCategories.AsEnumerable().Select(n => n.Field<string>("sizecategory")).Distinct().ToList<string>();
                    ddlCategory.DataSource = lstCategory;
                    //ddlCategory.DataTextField = "sizecategory";
                    //ddlCategory.DataValueField = "sizecategory";
                    ddlCategory.DataBind();
                    if (lstCategory.Count == 1)
                        ddlCategory_IndexChange(sender, e);
                    else
                        ddlCategory.Items.Insert(0, "Choose");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlPlatform.DataSource = "";
                ddlPlatform.DataBind();
                if (ddlCategory.SelectedItem.Text != "Choose")
                {
                    DataView dtTypeView = new DataView(ViewState["dtCategories"] as DataTable);
                    dtTypeView.RowFilter = "sizecategory = '" + ddlCategory.SelectedItem.Text + "'";
                    dtTypeView.Sort = "config ASC";
                    DataTable disConfig = dtTypeView.ToTable(true, "config");
                    if (disConfig.Rows.Count > 0)
                    {
                        ddlPlatform.DataSource = disConfig;
                        ddlPlatform.DataTextField = "config";
                        ddlPlatform.DataValueField = "config";
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlPlatform_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                if (ddlPlatform.SelectedItem.Text != "Choose")
                {
                    DataView dtTypeView = new DataView(ViewState["dtCategories"] as DataTable);
                    dtTypeView.RowFilter = "sizecategory = '" + ddlCategory.SelectedItem.Text + "' and config = '" + ddlPlatform.SelectedItem.Text + "'";
                    dtTypeView.Sort = "brand ASC";
                    DataTable disBrand = dtTypeView.ToTable(true, "brand");
                    if (disBrand.Rows.Count > 0)
                    {
                        ddlBrand.DataSource = disBrand;
                        ddlBrand.DataTextField = "brand";
                        ddlBrand.DataValueField = "brand";
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlBrand_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();
                if (ddlBrand.SelectedItem.Text != "Choose")
                {
                    DataView dtTypeView = new DataView(ViewState["dtCategories"] as DataTable);
                    dtTypeView.RowFilter = "sizecategory = '" + ddlCategory.SelectedItem.Text + "' and config = '" + ddlPlatform.SelectedItem.Text + "' and brand='" + ddlBrand.SelectedItem.Text + "'";
                    dtTypeView.Sort = "sidewall ASC";
                    DataTable disSidewall = dtTypeView.ToTable(true, "sidewall");
                    if (disSidewall.Rows.Count > 0)
                    {
                        ddlSidewall.DataSource = disSidewall;
                        ddlSidewall.DataTextField = "sidewall";
                        ddlSidewall.DataValueField = "sidewall";
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlSidewall_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlType.DataSource = "";
                ddlType.DataBind();
                if (ddlSidewall.SelectedItem.Text != "Choose")
                {
                    DataView dtTypeView = new DataView(ViewState["dtCategories"] as DataTable);
                    dtTypeView.RowFilter = "sizecategory = '" + ddlCategory.SelectedItem.Text + "' and config = '" + ddlPlatform.SelectedItem.Text + "' and brand='" + ddlBrand.SelectedItem.Text + "' and sidewall='" + ddlSidewall.SelectedItem.Text + "'";
                    dtTypeView.Sort = "tyretype ASC";
                    DataTable disTyreType = dtTypeView.ToTable(true, "tyretype");
                    if (disTyreType.Rows.Count > 0)
                    {
                        ddlType.DataSource = disTyreType;
                        ddlType.DataTextField = "tyretype";
                        ddlType.DataValueField = "tyretype";
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                if (ddlType.SelectedItem.Text != "Choose")
                {
                    DataView dtTypeView = new DataView(ViewState["dtCategories"] as DataTable);
                    dtTypeView.RowFilter = "sizecategory = '" + ddlCategory.SelectedItem.Text + "' and config = '" + ddlPlatform.SelectedItem.Text + "' and brand='"
                                         + ddlBrand.SelectedItem.Text + "' and sidewall='" + ddlSidewall.SelectedItem.Text + "' and tyretype='" + ddlType.SelectedItem.Text + "'";
                    dtTypeView.Sort = "tyresize ASC";
                    DataTable dtSize = dtTypeView.ToTable(true, "tyresize");
                    ddlSize.DataSource = dtSize;
                    ddlSize.DataTextField = "tyresize";
                    ddlSize.DataValueField = "tyresize";
                    ddlSize.DataBind();
                    if (dtSize.Rows.Count == 1)
                        ddlSize_IndexChange(sender, e);
                    else
                        ddlSize.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                if (ddlSize.SelectedItem.Text != "Choose")
                {
                    DataView dtTypeView = new DataView(ViewState["dtCategories"] as DataTable);
                    dtTypeView.RowFilter = "sizecategory = '" + ddlCategory.SelectedItem.Text + "' and config = '" + ddlPlatform.SelectedItem.Text + "' and brand='"
                                         + ddlBrand.SelectedItem.Text + "' and sidewall='" + ddlSidewall.SelectedItem.Text + "' and tyretype='" + ddlType.SelectedItem.Text + "' and tyresize='" + ddlSize.SelectedItem.Text + "'";
                    dtTypeView.Sort = "rimsize ASC";
                    DataTable dtRim = dtTypeView.ToTable(true, "rimsize");
                    ddlRim.DataSource = dtRim;
                    ddlRim.DataTextField = "rimsize";
                    ddlRim.DataValueField = "rimsize";
                    ddlRim.DataBind();
                    if (dtRim.Rows.Count == 1)
                        ddlRim_IndexChange(sender, e);
                    else
                        ddlRim.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlRim_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (ddlRim.SelectedItem.Text != "Choose")
                {
                    btnGetReport.Visible = true;
                    DataView dtTypeView = new DataView(ViewState["dtCategories"] as DataTable);
                    dtTypeView.RowFilter = "sizecategory = '" + ddlCategory.SelectedItem.Text + "' and config = '" + ddlPlatform.SelectedItem.Text + "' and brand='"
                                         + ddlBrand.SelectedItem.Text + "' and sidewall='" + ddlSidewall.SelectedItem.Text + "' and tyretype='" + ddlType.SelectedItem.Text
                                         + "' and tyresize='" + ddlSize.SelectedItem.Text + "' and rimsize='" + ddlRim.SelectedItem.Text + "'";
                    hdnProcessId.Value = dtTypeView.ToTable().Rows[0]["processid"].ToString();
                }
                else
                {
                    btnGetReport.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[8];
                sp1[0] = new SqlParameter("@custcode", ddlLoginUserName.SelectedItem.Value);
                sp1[1] = new SqlParameter("@sizecategory", ddlCategory.SelectedItem.Text);
                sp1[2] = new SqlParameter("@config", ddlPlatform.SelectedItem.Text);
                sp1[3] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                sp1[4] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                sp1[5] = new SqlParameter("@tyretype", ddlType.SelectedItem.Text);
                sp1[6] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);
                sp1[7] = new SqlParameter("@rimsize", ddlRim.SelectedItem.Text);
                DataTable dtPriceList = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_InvoiceItemPriceReport_PriceList", sp1, DataAccess.Return_Type.DataTable);
                if (dtPriceList.Rows.Count > 0)
                {
                    gvItemPriceList.DataSource = dtPriceList;
                    gvItemPriceList.DataBind();
                }
                else
                {
                    gvItemPriceList.DataSource = null;
                    gvItemPriceList.DataBind();
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}