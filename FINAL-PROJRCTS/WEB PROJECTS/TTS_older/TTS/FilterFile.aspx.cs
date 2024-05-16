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

namespace TTS
{
    public partial class FilterFile : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        lblPageTitle.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "e" ? "Export " : "Domestic ") +
                            (Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "Dispatched " : "Invoice ") + "Filter Wise Report";

                        if (Utilities.Decrypt(Request["qplant"].ToString()) != "" && Utilities.Decrypt(Request["qplant"]) != null
                            && Utilities.Decrypt(Request["qfyear"].ToString()) != "" && Utilities.Decrypt(Request["qfyear"]) != null
                            && Utilities.Decrypt(Request["qfmonth"].ToString()) != "" && Utilities.Decrypt(Request["qfmonth"]) != null
                            && Utilities.Decrypt(Request["qtyear"].ToString()) != "" && Utilities.Decrypt(Request["qtyear"]) != null
                            && Utilities.Decrypt(Request["qtmonth"].ToString()) != "" && Utilities.Decrypt(Request["qtmonth"]) != null
                            && Utilities.Decrypt(Request["qcustcode"].ToString()) != "" && Utilities.Decrypt(Request["qcustcode"]) != null
                            && Utilities.Decrypt(Request["fid"].ToString()) != "" && Utilities.Decrypt(Request["fid"]) != null
                            && Utilities.Decrypt(Request["fileid"].ToString()) != "" && Utilities.Decrypt(Request["fileid"]) != null)
                        {

                            hdnstocktype.Value = Utilities.Decrypt(Request["qplant"].ToString());
                            hdnfyear.Value = Utilities.Decrypt(Request["qfyear"].ToString());
                            hdnfmonth.Value = Utilities.Decrypt(Request["qfmonth"].ToString());
                            hdntyear.Value = Utilities.Decrypt(Request["qtyear"].ToString());
                            hdntmonth.Value = Utilities.Decrypt(Request["qtmonth"].ToString());
                            hdncustcode.Value = Utilities.Decrypt(Request["qcustcode"].ToString());
                            hdncusttype.Value = Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "DOMESTIC" : "EXPORT";
                            hdnfiletype.Value = Utilities.Decrypt(Request["fileid"].ToString());
                            ScriptManager.RegisterStartupScript(Page, GetType(), "showfilter", "filterRecords();", true);
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
                new SqlParameter("@plant", hdnstocktype.Value),new SqlParameter("@fyear",hdnfyear.Value),
                new SqlParameter("@tyear",hdntyear.Value),new SqlParameter("@fmonth",hdnfmonth.Value),
                new SqlParameter("@tmonth",hdntmonth.Value), new SqlParameter("@custcode",hdncustcode.Value),
                new SqlParameter("@custType",hdncusttype.Value ),
                new SqlParameter("@ResultQuery",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message)
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP(hdnfiletype.Value == "dis" ? "SP_LST_StockFilteredReport_DisDetails" : "SP_LST_InvoiceFilterWiseRep",
                    sp, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    gvStockDetails.DataSource = dt;
                    gvStockDetails.DataBind();
                    ViewState["dt"] = dt;
                    lblNoOfRecords.Text = "FILTER WISE AVAILABLE " + (hdnfiletype.Value == "dis" ? "DISPATCHED" : "INVOICE") + " LIST: " + dt.Rows.Count.ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowGrid", "onclick_btnApply();", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "bindCate", "bindCateg();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Request.RawUrl.Replace("FilterFile.aspx", "filterwiserequest.aspx"), false);
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
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "Dispatched-" : "Invoice-" +
                    DateTime.Now.ToShortDateString() + ""));
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvStockDetails.AllowPaging = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "bindCat", "bindCateg();", true);
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
        public override void VerifyRenderingInServerForm(Control control)
        {
            //
        }
    }
}