using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace TTS
{
    public partial class currentdebosters : System.Web.UI.Page
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
                            DataTable dtDate = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_debosters", DataAccess.Return_Type.DataTable);


                            gvdebosters.DataSource = dtDate;
                            gvdebosters.DataBind();
                            ViewState["dtscanneslist"] = dtDate;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            //lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
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
        protected void btnDownload_Click(object sedner, EventArgs e)
        {
            DownloadXlsFile();
        }
        private void DownloadXlsFile()
        {
            try
            {
                //lblErrMsg.Text = "";
                if (ViewState["dtscanneslist"] != null)
                {
                    string s = "debosters"+ DateTime.Now.ToShortDateString();
                   // string r = s.Reverse().ToString();
                    Response.ClearContent();
                    //Response.Buffer = true;
                    //Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "scanned" + DateTime.Now.ToShortDateString() + ""));
                    Response.AppendHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "DEBTORS" + DateTime.Now.ToShortDateString() + ""));
                    //Response.Charset = "";
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/excel";
                    StringWriter sw = new System.IO.StringWriter();


                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvdebosters.AllowPaging = false;
                    DataTable dtList = ViewState["dtscanneslist"] as DataTable;
                    gvdebosters.DataSource = dtList;
                    gvdebosters.DataBind();
                    gvdebosters.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.Clear();
                    Response.End();
                }

                else
                {

                }
                    //lblErrMsg.Text = "NO RECORDS";
            }
            catch (System.Threading.ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}