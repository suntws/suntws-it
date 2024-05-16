using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace TTS
{
    public partial class cotsDomDebtorsReceipts : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request["qstring"] != null && Request["qstring"].ToString() != "")
                    {
                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_seldebtplant", DataAccess.Return_Type.DataTable);
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
                ddlYear.DataSource = "";
                ddlYear.DataBind();
                ddlMonth.DataSource = "";
                ddlMonth.DataBind();
                ddlDay.DataSource = "";
                ddlDay.DataBind();
                lblErrMsg.Text = "";
                gvDebtReceipts.DataSource = "";
                gvDebtReceipts.DataBind();

                if (ddlPlant.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_debtyear", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlYear.DataSource = dt;
                        ddlYear.DataTextField = "debtyear";
                        ddlYear.DataValueField = "debtyear";
                        ddlYear.DataBind();
                        ddlYear.Items.Insert(0, "--SELECT--");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        protected void ddlYear_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlMonth.DataSource = "";
                ddlMonth.DataBind();
                ddlDay.DataSource = "";
                ddlDay.DataBind();
                lblErrMsg.Text = "";
                gvDebtReceipts.DataSource = "";
                gvDebtReceipts.DataBind();

                if (ddlYear.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] {  
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@year", Convert.ToInt32(ddlYear.SelectedItem.Text)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_debtmonth", sp1, DataAccess.Return_Type.DataTable);
                    ddlMonth.DataSource = dt;
                    ddlMonth.DataTextField = "debtmonth";
                    ddlMonth.DataValueField = "debtmonthid";
                    ddlMonth.DataBind();
                    ddlMonth.Items.Insert(0, "--SELECT--");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlMonth_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlDay.DataSource = "";
                ddlDay.DataBind();
                lblErrMsg.Text = "";
                gvDebtReceipts.DataSource = "";
                gvDebtReceipts.DataBind();

                if (ddlMonth.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@year", Convert.ToInt32(ddlYear.SelectedItem.Text)),
                        new SqlParameter("@month", Convert.ToInt32(ddlMonth.SelectedItem.Value)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_debtday", sp1, DataAccess.Return_Type.DataTable);
                    ddlDay.DataSource = dt;
                    ddlDay.DataTextField = "debtdays";
                    ddlDay.DataValueField = "debtdays";
                    ddlDay.DataBind();
                    ddlDay.Items.Insert(0, "--SELECT--");
                    ddlDay.Items.Insert(1, "All");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        } 
        protected void ddlDay_indexchanged(object sender, EventArgs e)
        {
            try
            {
                gvDebtReceipts.DataSource = "";
                gvDebtReceipts.DataBind();
                btnDownload.Style.Add("display", "block");
                if (ddlDay.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                         
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@year", Convert.ToInt32(ddlYear.SelectedItem.Text)), 
                        new SqlParameter("@month", Convert.ToInt32(ddlMonth.SelectedItem.Value)), 
                        new SqlParameter("@day", ddlDay.SelectedItem.Text)
                    };
                    //DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("", sp1, DataAccess.Return_Type.DataTable);
                    //if (dt != null && dt.Rows.Count > 0)
                    //{
                    //    gvDebtReceipts.DataSource = dt;
                    //    gvDebtReceipts.DataBind();
                        
                    //}
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string s = "";
                string r = s.Reverse().ToString();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "Domestic Debtors Receipts-" +
                    DateTime.Now.ToShortDateString() + ""));
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvDebtReceipts.AllowPaging = false;
                    gvDebtReceipts.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.Clear();
                    Response.End();
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
                Response.Redirect(Request.RawUrl.ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}