using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Threading;

namespace TTS
{
    public partial class stockdump : System.Web.UI.Page
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
                            DataTable dtDate = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stockupdate_date", DataAccess.Return_Type.DataTable);

                            DataView view = new DataView(dtDate);
                            view.Sort = "Plant ASC";
                            DataTable dtPlant = view.ToTable(true, "Plant");

                            if (dtPlant.Rows.Count > 0)
                            {
                                ddlStockPlant.DataSource = dtPlant;
                                ddlStockPlant.DataValueField = "Plant";
                                ddlStockPlant.DataTextField = "Plant";
                                ddlStockPlant.DataBind();
                                ddlStockPlant.Items.Insert(0, "ALL");
                            }

                            gvStockUpdateDate.DataSource = dtDate;
                            gvStockUpdateDate.DataBind();
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
        protected void ddlStockPlant_IndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDump = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_StockDump_v1", DataAccess.Return_Type.DataTable);
                if (ddlStockPlant.SelectedIndex > 0)
                {
                    DataView dv_Brand = new DataView(dtDump);
                    dv_Brand.RowFilter = "Plant='" + ddlStockPlant.SelectedItem.Text + "'";
                    dtDump = new DataTable();
                    dtDump = dv_Brand.ToTable(true);
                }

                string attachment = "attachment; filename=GSD" + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "_" +
                    DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dtDump.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dtDump.Rows)
                {
                    tab = "";
                    for (i = 0; i < dtDump.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.Flush();
                Response.Clear();
                Response.End();
                Response.Redirect(Request.RawUrl, false);
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnGsdFileMail_Click(object sender, EventArgs e)
        {
            try
                
            {
               // double o_id1;
               // o_id1 = Convert.ToInt64(DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + DateTime.Now.Millisecond);
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Convert.ToInt64(DateTime.Now.Day.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Year + DateTime.Now.Millisecond)), 
                    new SqlParameter("@requestmail", Request.Cookies["TTSUserEmail"].Value), 
                    new SqlParameter("@requestby", ddlStockPlant.SelectedItem.Text), 
                    new SqlParameter("@processtype", "GSD FILE") 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_excel_prepare_history", sp);
                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Within 30 minutes GSD file will be prepared and sent to your email (" +
                        Request.Cookies["TTSUserEmail"].Value + ").')", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}