using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace TTS
{
    public partial class invoiceFilterwise : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblPageTitle.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "Domestic " : "Export ") + "Invoice Filter Wise Report";

                    if (Request["fid"] != null && Request["fid"].ToString() != "")
                    {
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())) };

                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_InvoiceRep_Plant", sp, DataAccess.Return_Type.DataTable);
                        ddlPlant.DataSource = dt;
                        ddlPlant.DataTextField = "plant";
                        ddlPlant.DataValueField = "plant";
                        ddlPlant.DataBind();
                        ddlPlant.Items.Insert(0, "--SELECT--");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlPlant_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlFromYear.DataSource = "";
                ddlFromYear.DataBind();
                ddlFromMonth.DataSource = "";
                ddlFromMonth.DataBind();
                ddlToYear.DataSource = "";
                ddlToYear.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();
                hdnstocktype.Value = ddlPlant.SelectedItem.Text;

                if (ddlPlant.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_InvoiceRep_Fyear", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlFromYear.DataSource = dt;
                        ddlFromYear.DataTextField = "InvoiceRepFYear";
                        ddlFromYear.DataValueField = "InvoiceRepFYear";
                        ddlFromYear.DataBind();
                        ddlFromYear.Items.Insert(0, "--SELECT--");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlFromYear_indexchanged(object sender, EventArgs e)
        {
            try
            {

                ddlFromMonth.DataSource = "";
                ddlFromMonth.DataBind();
                ddlToYear.DataSource = "";
                ddlToYear.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();

                if (ddlFromYear.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@year", Convert.ToInt32(ddlFromYear.SelectedItem.Text)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_InvoiceRep_Fmonth", sp1, DataAccess.Return_Type.DataTable);
                    ddlFromMonth.DataSource = dt;
                    ddlFromMonth.DataTextField = "InvoiceRepFmonth";
                    ddlFromMonth.DataValueField = "InvoiceRepFmonthId";
                    ddlFromMonth.DataBind();
                    ddlFromMonth.Items.Insert(0, "--SELECT--");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlFromMonth_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlToYear.DataSource = "";
                ddlToYear.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();


                if (ddlFromMonth.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@year", Convert.ToInt32(ddlFromYear.SelectedItem.Text)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_InvoiceFilterReport_Tyear", sp1, DataAccess.Return_Type.DataTable);
                    ddlToYear.DataSource = dt;
                    ddlToYear.DataTextField = "InvoiceFilTYear";
                    ddlToYear.DataValueField = "InvoiceFilTYear";
                    ddlToYear.DataBind();
                    ddlToYear.Items.Insert(0, "--SELECT--");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlToYear_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();

                if (ddlToYear.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@fyear", Convert.ToInt32(ddlFromYear.SelectedItem.Text)), 
                        new SqlParameter("@tyear", Convert.ToInt32(ddlToYear.SelectedItem.Text)), 
                        new SqlParameter("@fmonth", Convert.ToInt32(ddlFromMonth.SelectedItem.Value)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_InvoiceFilter_Tmonth", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlToMonth.DataSource = dt;
                        ddlToMonth.DataTextField = "InvoiceFilTmonth";
                        ddlToMonth.DataValueField = "InvoiceFilTmonthId";
                        ddlToMonth.DataBind();
                        ddlToMonth.Items.Insert(0, "--SELECT--");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlToMonth_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();
                if (ddlToMonth.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@fyear", Convert.ToInt32(ddlFromYear.SelectedItem.Text)), 
                        new SqlParameter("@tyear", Convert.ToInt32(ddlToYear.SelectedItem.Text)), 
                        new SqlParameter("@fmonth", Convert.ToInt32(ddlFromMonth.SelectedItem.Value)),
                        new SqlParameter("@tmonth", Convert.ToInt32(ddlToMonth.SelectedItem.Value))
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_InvoiceFilter_CustName", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlCustName.DataSource = dt;
                        ddlCustName.DataTextField = "CustName";
                        ddlCustName.DataValueField = "CustCode";
                        ddlCustName.DataBind();
                        ddlCustName.Items.Insert(0, "--SELECT--");
                        ddlCustName.Items.Insert(1, "ALL");
                    }
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCustName_indexchanged(object sender, EventArgs e)
        {
            try
            {
                lblNoOfRecords.Text = "";
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                bindCategories();
                ScriptManager.RegisterStartupScript(Page, GetType(), "showfiltercont", "showFilterContent();", true);
                

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnTriggerGv_Click(object sender, EventArgs e)
        {
            bindCategories();
            ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptStock", "hideProgress();", true);
        }
        private void bindCategories()
        {
            try
            {
                div_platform.InnerHtml = Bind_selectedCategory(hdnconfig.Value, "badge-success", "tr_Select_Platform");
                div_brand.InnerHtml = Bind_selectedCategory(hdnbrand.Value, "badge-secondary", "tr_Select_Brand");
                div_sidewall.InnerHtml = Bind_selectedCategory(hdnsidewall.Value, "badge-primary", "tr_Select_Sidewall");
                div_type.InnerHtml = Bind_selectedCategory(hdntyretype.Value, "badge-warning", "tr_Select_Type");
                div_size.InnerHtml = Bind_selectedCategory(hdntyresize.Value, "badge-info", "tr_Select_Size");
                div_rim.InnerHtml = Bind_selectedCategory(hdnrimsize.Value, "badge-light", "tr_Select_Rim");

                ScriptManager.RegisterStartupScript(Page, GetType(), "remove", "removeDoubleQuotes();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                lblNoOfRecords.Text = "";
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                string message = "";
                SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@config",hdnconfig.Value.Length > 0 ? hdnconfig.Value.Remove(hdnconfig.Value.Length-1,1) : hdnconfig.Value),
                new SqlParameter("@tyresize",hdntyresize.Value.Length > 0 ? hdntyresize.Value.Remove(hdntyresize.Value.Length-1,1) : hdntyresize.Value),
                new SqlParameter("@rimsize",hdnrimsize.Value.Length > 0 ? hdnrimsize.Value.Remove(hdnrimsize.Value.Length-1,1) : hdnrimsize.Value),
                new SqlParameter("@tyretype",hdntyretype.Value.Length > 0 ? hdntyretype.Value.Remove(hdntyretype.Value.Length-1,1) : hdntyretype.Value),
                new SqlParameter("@brand",hdnbrand.Value.Length > 0 ? hdnbrand.Value.Remove(hdnbrand.Value.Length-1,1) : hdnbrand.Value),
                new SqlParameter("@sidewall",hdnsidewall.Value.Length > 0 ? hdnsidewall.Value.Remove(hdnsidewall.Value.Length-1,1) : hdnsidewall.Value),
                new SqlParameter("@plant", hdnstocktype.Value),
                new SqlParameter("@fyear",ddlFromYear.SelectedItem.Text),
                new SqlParameter("@tyear",ddlToYear.SelectedItem.Text),
                new SqlParameter("@fmonth",ddlFromMonth.SelectedItem.Text),
                new SqlParameter("@tmonth",ddlToMonth.SelectedItem.Text), 
                new SqlParameter("@custcode",ddlCustName.SelectedItem.Value),
                new SqlParameter("@custType",Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "DOMESTIC" : "EXPORT"),
                new SqlParameter("@ResultQuery",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message)
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_InvoiceFilterWiseRep", sp, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    gvStockDetails.DataSource = dt;
                    gvStockDetails.DataBind();
                    ViewState["dt"] = dt;
                    lblNoOfRecords.Text = "FILTER WISE AVAILABLE INVOICE LIST: " + dt.Rows.Count.ToString();
                    lblNoOfRecords.Visible = true;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showgrid", "btnApplyClick();", true);
                }
                else
                {
                    lblNoOfRecords.Text = "NO RECORDS";
                    lblNoOfRecords.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Request.RawUrl.ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private string Bind_selectedCategory(string arrValue, string cssClass, string row)
        {
            try
            {
                string str = "";
                if (arrValue.Length > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Show" + row + "", "document.getElementById('" + row + "').style.display = 'block';", true);
                    string[] arr = arrValue.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].Length > 0)
                            str += "<span class='badge " + cssClass + "' style='border-radius: 20px;'>" + arr[i] + " </span> &nbsp;";
                    }
                    return str.Replace("'", "\"");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Hide" + row + "", "document.getElementById('" + row + "').style.display = 'none';", true);
                    return "";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        protected void gvStockList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dt = ViewState["dt"] as DataTable;
                gvStockDetails.DataSource = dt;
                gvStockDetails.DataBind();
                gvStockDetails.PageIndex = e.NewPageIndex;
                gvStockDetails.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkStockXls_Click(object sedner, EventArgs e)
        {
            DownloadXlsFile();
        }
        protected void btnStockXls_Click(object sender, ImageClickEventArgs e)
        {
            DownloadXlsFile();
        }
        private void DownloadXlsFile()
        {
            try
            {
                string s = "";
                string r = s.Reverse().ToString();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "Invoice-" + DateTime.Now.ToShortDateString() + ""));//"attachment;filename=" + txtStockXlsName.Text + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvStockDetails.AllowPaging = false;
                    this.bindCategories();
                    gvStockDetails.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.Clear();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}