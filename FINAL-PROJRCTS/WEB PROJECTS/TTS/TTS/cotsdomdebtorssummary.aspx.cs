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
    public partial class cotsdomdebtorssummary : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        int col1;
        int col2;
        int col3;
        int col4;
        int col5;
        int col6;
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
                        DataTable dtSum = new DataTable();
                        dtSum.Columns.Add(new DataColumn("NAME OF PARTY", typeof(System.String)));
                        dtSum.Columns.Add(new DataColumn("ABOVE 360", typeof(System.Decimal)));
                        dtSum.Columns.Add(new DataColumn("181-360", typeof(System.Decimal)));
                        dtSum.Columns.Add(new DataColumn("91-180", typeof(System.Decimal)));
                        dtSum.Columns.Add(new DataColumn("61-90", typeof(System.Decimal)));
                        dtSum.Columns.Add(new DataColumn("31-60", typeof(System.Decimal)));
                        dtSum.Columns.Add(new DataColumn("0-30", typeof(System.Decimal)));
                        dtSum.Columns.Add(new DataColumn("TOTAL", typeof(System.Double)));
                        DataTable crmNames = dt.DefaultView.ToTable(true, "CRM Name");
                        foreach (DataRow crm in crmNames.Rows)
                        {
                            dtSum.Rows.Add(crm["CRM Name"].ToString());
                        }
                        float colt1=0;
                        float colt2=0;
                        float colt3=0;
                        float colt4=0;
                        float colt5=0;
                        float colt6=0;
                        decimal colt7 = 0;
                        foreach (DataRow f in dtSum.Rows)
                        {
                            col1 = 0; col2 = 0; col3 = 0; col4 = 0; col5 = 0; col6 = 0;
                            foreach (DataRow s in dt.Select("[CRM Name]='" + f["NAME OF PARTY"].ToString() + "'"))
                            {
                                if ((int)s["Due Days"] >= 360)
                                {
                                    col1 += (int)s["Pending"];
                                }
                                if ((int)s["Due Days"] >= 181 && (int)s["Due Days"] <= 360)
                                {
                                    col2 += (int)s["Pending"];
                                }
                                if ((int)s["Due Days"] >= 91 && (int)s["Due Days"] <= 180)
                                {
                                    col3 += (int)s["Pending"];
                                }
                                if ((int)s["Due Days"] >= 61 && (int)s["Due Days"] <= 90)
                                {
                                    col4 += (int)s["Pending"];
                                }
                                if ((int)s["Due Days"] >= 31 && (int)s["Due Days"] <= 60)
                                {
                                    col5 += (int)s["Pending"];
                                }
                                if ((int)s["Due Days"] >= 0 && (int)s["Due Days"] <= 30)
                                {
                                    col6 += (int)s["Pending"];
                                }
                            }
                            
                            f["ABOVE 360"] = (float)col1 / 1000;
                            f["181-360"] = (float)col2 / 1000;
                            f["91-180"] = (float)col3 / 1000;
                            f["61-90"] = (float)col4 / 1000;
                            f["31-60"] = (float)col5 / 1000;
                            f["0-30"] = (float)col6 / 1000;
                            decimal ttl = Convert.ToDecimal((col1 / 1000) + (col2 / 1000) + (col3 / 1000) + (col4 / 1000) + (col5 / 1000) + (col6 / 1000));
                            f["TOTAL"] = ttl;

                            colt1 += (float)col1 / 1000;
                            colt2 += (float)col2 / 1000;
                            colt3 += (float)col3 / 1000;
                            colt4 += (float)col4 / 1000;
                            colt5 += (float)col5 / 1000;
                            colt6 += (float)col6 / 1000;
                            colt7 += ttl;
                        }
                        dtSum.Rows.Add("GRAND TOTAL", colt1, colt2, colt3, colt4, colt5, colt6, colt7);
                        gvDebtReceipts.DataSource = dtSum;
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
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "Domestic Debtors Summary-" +
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