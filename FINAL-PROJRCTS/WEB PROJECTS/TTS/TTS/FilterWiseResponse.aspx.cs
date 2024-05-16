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
using System.Threading;


namespace TTS
{
    public partial class FilterWiseResponse : System.Web.UI.Page
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
                        lblPageTitle.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "e" ? "Export " : "Domestic ") + "dispatched " +
                            (Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "Stencil " : "Item ") + "Wise Report";

                        if (Utilities.Decrypt(Request["qplant"].ToString()) != "" && Utilities.Decrypt(Request["qplant"]) != null
                            && Utilities.Decrypt(Request["qfyear"].ToString()) != "" && Utilities.Decrypt(Request["qfyear"]) != null
                            && Utilities.Decrypt(Request["qfmonth"].ToString()) != "" && Utilities.Decrypt(Request["qfmonth"]) != null
                            && Utilities.Decrypt(Request["qtyear"].ToString()) != "" && Utilities.Decrypt(Request["qtyear"]) != null
                            && Utilities.Decrypt(Request["qtmonth"].ToString()) != "" && Utilities.Decrypt(Request["qtmonth"]) != null
                            && Utilities.Decrypt(Request["qcustcode"].ToString()) != "" && Utilities.Decrypt(Request["qcustcode"]) != null
                            && Utilities.Decrypt(Request["fid"].ToString()) != "" && Utilities.Decrypt(Request["fid"]) != null
                            && Utilities.Decrypt(Request["fileid"].ToString()) != "" && Utilities.Decrypt(Request["fileid"]) != null)
                        {
                            lblPlant.Text = Utilities.Decrypt(Request["qplant"].ToString());
                            lblFromYear.Text = Utilities.Decrypt(Request["qfyear"].ToString());
                            lblFromMonth.Text = Utilities.Decrypt(Request["qfmonth"].ToString());
                            lblToYear.Text = Utilities.Decrypt(Request["qtyear"].ToString());
                            lblToMonth.Text = Utilities.Decrypt(Request["qtmonth"].ToString());
                            lblCustomer.Text = Utilities.Decrypt(Request["qcustname"].ToString());
                            hdncustcode.Value = Utilities.Decrypt(Request["qcustcode"].ToString());
                            hdncusttype.Value = Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "DOMESTIC" : "EXPORT";
                            hdnfiletype.Value = Utilities.Decrypt(Request["fileid"].ToString());

                            bindChkboxlists();
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
        private void bindChkboxlists()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@plant", lblPlant.Text),
                                new SqlParameter("@Fyear", lblFromYear.Text),
                                new SqlParameter("@Tyear", lblToYear.Text),
                                new SqlParameter("@Fmonth", lblFromMonth.Text),
                                new SqlParameter("@Tmonth",lblToMonth.Text),
                                new SqlParameter("@custcode",hdncustcode.Value),
                                new SqlParameter("@custType",hdncusttype.Value)
                            };

                DataTable dtFilterReport = (DataTable)daCOTS.ExecuteReader_SP(Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "SP_disFilteredRep_Categ" : "SP_sel_invoiceFilters", sp, DataAccess.Return_Type.DataTable);

                List<string> lstConfig = dtFilterReport.AsEnumerable().Where(n => n.Field<string>("config") != "" && n.Field<string>("config") != null).OrderBy(n => n.Field<string>("config")).Select(n => n.Field<string>("config")).Distinct().ToList();
                List<string> rimsize = dtFilterReport.AsEnumerable().Where(n => n.Field<string>("rimsize") != "" && n.Field<string>("rimsize") != null).OrderBy(n => n.Field<string>("rimsize")).Select(n => n.Field<string>("rimsize")).Distinct().ToList();
                List<string> tyretype = dtFilterReport.AsEnumerable().Where(n => n.Field<string>("tyretype") != "" && n.Field<string>("tyretype") != null).OrderBy(n => n.Field<string>("tyretype")).Select(n => n.Field<string>("tyretype")).Distinct().ToList();
                List<string> brand = dtFilterReport.AsEnumerable().Where(n => n.Field<string>("brand") != "" && n.Field<string>("brand") != null).OrderBy(n => n.Field<string>("brand")).Select(n => n.Field<string>("brand")).Distinct().ToList();
                List<string> sidewall = dtFilterReport.AsEnumerable().Where(n => n.Field<string>("sidewall") != "" && n.Field<string>("sidewall") != null).OrderBy(n => n.Field<string>("sidewall")).Select(n => n.Field<string>("sidewall")).Distinct().ToList();
                List<string> tyresize = dtFilterReport.AsEnumerable().Where(n => n.Field<string>("tyresize") != "" && n.Field<string>("tyresize") != null).OrderBy(n => n.Field<string>("tyresize")).Select(n => n.Field<string>("tyresize")).Distinct().ToList();

                if (lstConfig.Count > 0)
                {
                    chkPlatform.DataSource = lstConfig;
                    chkPlatform.DataBind();
                }
                if (brand.Count > 0)
                {
                    chkBrand.DataSource = brand;
                    chkBrand.DataBind();
                }
                if (sidewall.Count > 0)
                {
                    chkSidewall.DataSource = sidewall;
                    chkSidewall.DataBind();
                }
                if (tyretype.Count > 0)
                {
                    chkTyretype.DataSource = tyretype;
                    chkTyretype.DataBind();
                }
                if (tyresize.Count > 0)
                {
                    chkTyresize.DataSource = tyresize;
                    chkTyresize.DataBind();
                }
                if (rimsize.Count > 0)
                {
                    chkRimsize.DataSource = rimsize;
                    chkRimsize.DataBind();
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnApplyFilters_Click(object sender, EventArgs e)
        {
            bindGridView();
        }
        private void bindGridView()
        {
            try
            {
                lblNoOfRecords.Text = "";
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                string message = "";
                hideAndView();

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@QString", Utilities.Decrypt(Request["fid"].ToString())), 
                    new SqlParameter("@config", hdnPlatform.Value.Length > 0 ? hdnPlatform.Value.Remove(hdnPlatform.Value.Length - 1, 1) : hdnPlatform.Value), 
                    new SqlParameter("@tyresize", hdnTyresize.Value.Length > 0 ? hdnTyresize.Value.Remove(hdnTyresize.Value.Length - 1, 1) : hdnTyresize.Value), 
                    new SqlParameter("@rimsize", hdnRimsize.Value.Length > 0 ? hdnRimsize.Value.Remove(hdnRimsize.Value.Length - 1, 1) : hdnRimsize.Value), 
                    new SqlParameter("@tyretype", hdnTyretype.Value.Length > 0 ? hdnTyretype.Value.Remove(hdnTyretype.Value.Length - 1, 1) : hdnTyretype.Value), 
                    new SqlParameter("@brand", hdnBrand.Value.Length > 0 ? hdnBrand.Value.Remove(hdnBrand.Value.Length - 1, 1) : hdnBrand.Value), 
                    new SqlParameter("@sidewall", hdnSidewall.Value.Length > 0 ? hdnSidewall.Value.Remove(hdnSidewall.Value.Length - 1, 1) : hdnSidewall.Value), 
                    new SqlParameter("@plant", lblPlant.Text), 
                    new SqlParameter("@fyear", lblFromYear.Text), 
                    new SqlParameter("@tyear", lblToYear.Text), 
                    new SqlParameter("@fmonth", lblFromMonth.Text), 
                    new SqlParameter("@tmonth", lblToMonth.Text), 
                    new SqlParameter("@custcode", hdncustcode.Value), 
                    new SqlParameter("@custType", hdncusttype.Value), 
                    new SqlParameter("@ResultQuery", SqlDbType.VarChar, 8000, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Default, message) 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP(hdnfiletype.Value == "dis" ? "SP_LST_StockFilteredReport_DisDetails" : "SP_LST_InvoiceFilterWiseRep",
                    sp, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    gvStockDetails.DataSource = dt;
                    gvStockDetails.DataBind();
                    ViewState["dt"] = dt;
                    lblNoOfRecords.Text = "FILTER WISE AVAILABLE " + (hdnfiletype.Value == "dis" ? "DISPATCHED" : "INVOICE") + " LIST: " + dt.Rows.Count.ToString();
                }
                else
                {

                    lblNoOfRecords.Text = "NO RECORDS";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "btndownloadHide", "document.getElementById('lnkbtnDownload').style.display='none';", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void hideAndView()
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "PlatformShow", "bindFilterCateg('#chkPlatform');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "BrandShow", "bindFilterCateg('#chkBrand');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "SidewallShow", "bindFilterCateg('#chkSidewall');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "TypeShow", "bindFilterCateg('#chkTyretype');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "SizeShow", "bindFilterCateg('#chkTyresize');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "RimShow", "bindFilterCateg('#chkRimsize');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "btnShow&Hide", "btnHide();", true);
        }
        protected void lnkbtnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Request.RawUrl.Replace("filterwiseresponse.aspx", "filterwiserequest.aspx"), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
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
                hideAndView();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDownload_Click(object sedner, EventArgs e)
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
                string strFileName = DateTime.Now.Year + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + DateTime.Now.Hour.ToString("00") +
                    DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls",
                    Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "Dispatched-" : "Invoice-" + strFileName + ""));
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvStockDetails.AllowPaging = false;
                    this.bindGridView();
                    gvStockDetails.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.Clear();
                    Response.End();
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "bindCat", "bindCateg();", true);
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                chkPlatform.SelectedIndex = -1;
                chkBrand.SelectedIndex = -1;
                chkTyretype.SelectedIndex = -1;
                chkTyresize.SelectedIndex = -1;
                chkRimsize.SelectedIndex = -1;
                lblNoOfRecords.Text = "";
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();

                Page_Load(sender, e);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //
        }
    }
}