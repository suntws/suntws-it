using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Threading;

namespace TTS
{
    public partial class StockFilteredReport_V1 : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["stock_report"].ToString() == "True")
                        {
                            hdnstocktype.Value = Utilities.Decrypt(Request["spid"].ToString());
                            lblPageTitle.Text = "STOCK AVAILABILITY REPORT(" + Utilities.Decrypt(Request["spid"].ToString()) + ")";
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@plant", hdnstocktype.Value) };
                            DataSet dsFiltCateg = (DataSet)daCOTS.ExecuteReader_SP("SP_LST_StockFilteredReport_Categories_v1", sp, DataAccess.Return_Type.DataSet);
                            if (dsFiltCateg.Tables[0].Rows.Count > 0)
                            {
                                chkPlatform.DataSource = dsFiltCateg.Tables[0];
                                chkPlatform.DataTextField = "config";
                                chkPlatform.DataValueField = "config";
                                chkPlatform.DataBind();
                            }
                            if (dsFiltCateg.Tables[1].Rows.Count > 0)
                            {
                                chkBrand.DataSource = dsFiltCateg.Tables[1];
                                chkBrand.DataTextField = "brand";
                                chkBrand.DataValueField = "brand";
                                chkBrand.DataBind();
                            }
                            if (dsFiltCateg.Tables[2].Rows.Count > 0)
                            {
                                chkSidewall.DataSource = dsFiltCateg.Tables[2];
                                chkSidewall.DataTextField = "sidewall";
                                chkSidewall.DataValueField = "sidewall";
                                chkSidewall.DataBind();
                            }
                            if (dsFiltCateg.Tables[3].Rows.Count > 0)
                            {
                                chkTyretype.DataSource = dsFiltCateg.Tables[3];
                                chkTyretype.DataTextField = "tyretype";
                                chkTyretype.DataValueField = "tyretype";
                                chkTyretype.DataBind();
                            }
                            if (dsFiltCateg.Tables[4].Rows.Count > 0)
                            {
                                chkRimsize.DataSource = dsFiltCateg.Tables[4];
                                chkRimsize.DataTextField = "rimsize";
                                chkRimsize.DataValueField = "rimsize";
                                chkRimsize.DataBind();
                            }
                            if (dsFiltCateg.Tables[5].Rows.Count > 0)
                            {
                                chkGrade.DataSource = dsFiltCateg.Tables[5];
                                chkGrade.DataTextField = "grade";
                                chkGrade.DataValueField = "grade";
                                chkGrade.DataBind();
                            }
                            if (dsFiltCateg.Tables[6].Rows.Count > 0)
                            {
                                chkYear.DataSource = dsFiltCateg.Tables[6];
                                chkYear.DataTextField = "yom";
                                chkYear.DataValueField = "yom";
                                chkYear.DataBind();
                            }
                            if (dsFiltCateg.Tables[8].Rows.Count > 0)
                            {
                                chkTyresize.DataSource = dsFiltCateg.Tables[8];
                                chkTyresize.DataTextField = "tyresize";
                                chkTyresize.DataValueField = "tyresize";
                                chkTyresize.DataBind();
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

                HideAndView();
                string message = "";
                SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@config",hdnPlatform.Value.Length > 0 ? hdnPlatform.Value.Remove(hdnPlatform.Value.Length-1,1) : hdnPlatform.Value),
                new SqlParameter("@tyresize",hdnTyresize.Value.Length > 0 ? hdnTyresize.Value.Remove(hdnTyresize.Value.Length-1,1) : hdnTyresize.Value),
                new SqlParameter("@rimsize",hdnRimsize.Value.Length > 0 ? hdnRimsize.Value.Remove(hdnRimsize.Value.Length-1,1) : hdnRimsize.Value),
                new SqlParameter("@tyretype",hdnTyretype.Value.Length > 0 ? hdnTyretype.Value.Remove(hdnTyretype.Value.Length-1,1) : hdnTyretype.Value),
                new SqlParameter("@brand",hdnBrand.Value.Length > 0 ? hdnBrand.Value.Remove(hdnBrand.Value.Length-1,1) : hdnBrand.Value),
                new SqlParameter("@sidewall",hdnSidewall.Value.Length > 0 ? hdnSidewall.Value.Remove(hdnSidewall.Value.Length-1,1) : hdnSidewall.Value),
                new SqlParameter("@grade",hdnGrade.Value.Length > 0 ? hdnGrade.Value.Remove(hdnGrade.Value.Length-1,1) : hdnGrade.Value),
                new SqlParameter("@year",hdnYear.Value.Length > 0 ? hdnYear.Value.Remove(hdnYear.Value.Length-1,1) : hdnYear.Value),
                new SqlParameter("@withStencil",rdblStencil.SelectedValue),new SqlParameter("@StockType", hdnstocktype.Value),
                new SqlParameter("@ResultQuery",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message)
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_StockFilteredReport_StockDetails_V1", sp, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    gvStockDetails.DataSource = dt;
                    gvStockDetails.DataBind();
                    ViewState["dt"] = dt;
                    lblNoOfRecords.Text = "FILTER WISE AVAILABLE STOCK: " + (rdblStencil.SelectedValue == "1" ? dt.Rows.Count.ToString() : dt.Compute("Sum(QTY)", "").ToString());
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
        private void HideAndView()
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "PlatformShow", "bindFilterCateg('#chkPlatform');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "BrandShow", "bindFilterCateg('#chkBrand');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "SidewallShow", "bindFilterCateg('#chkSidewall');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "TypeShow", "bindFilterCateg('#chkTyretype');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "SizeShow", "bindFilterCateg('#chkTyresize');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "RimShow", "bindFilterCateg('#chkRimsize');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "GradeShow", "bindFilterCateg('#chkGrade');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "YearShow", "bindFilterCateg('#chkYear');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "btnShow&Hide", "btnHide();", true);
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
                HideAndView();
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
                chkGrade.SelectedIndex = -1;
                chkYear.SelectedIndex = -1;
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
        protected void btnDownload_Click(object sender, EventArgs e)
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
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "Stock-" + strFileName));
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
                HideAndView();
            }
            catch (ThreadAbortException)
            {

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