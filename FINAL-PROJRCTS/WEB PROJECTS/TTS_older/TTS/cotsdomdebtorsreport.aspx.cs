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
    public partial class cotsdomdebtorsreport : System.Web.UI.Page
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
                lblErrMsgcontent.Text = "";
                gvDebtReceipts.DataSource = "";
                gvDebtReceipts.DataBind();
                btnDownload.Style.Add("display", "block");

                if (ddlPlant.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_domDebtorsReport", sp1, DataAccess.Return_Type.DataTable);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        gvDebtReceipts.DataSource = dt;
                        gvDebtReceipts.DataBind();

                    }
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
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "Domestic Debtors Report-" +
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