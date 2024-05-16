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
using System.Data.OleDb;
using System.IO;
using System.Threading;
using System.Web.UI.DataVisualization.Charting;

namespace TTS
{
    public partial class liquidationreport : System.Web.UI.Page
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
                            lblPageHead.Text = "STOCK LIQUIDATION REPORT";
                            DataTable dtPlant = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Liq_Plant", DataAccess.Return_Type.DataTable);
                            if (dtPlant.Rows.Count > 0)
                            {
                                ddlLiqPlant.DataSource = dtPlant;
                                ddlLiqPlant.DataTextField = "Plant";
                                ddlLiqPlant.DataValueField = "Plant";
                                ddlLiqPlant.DataBind();
                                ddlLiqPlant.Items.Insert(0, "CHOOSE");
                                //ddlLiqPlant.Items.Insert(1, "ALL");
                            }
                            spnLink.Style.Add("display", "none");
                            btnLiqDownload.Style.Add("display", "none");
                            lblLiqDescription.Text = "";
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlLiqPlant_IndexChange(object sender, EventArgs e)
        {
            try
            {
                spnLink.Style.Add("display", "none");
                btnLiqDownload.Style.Add("display", "none");
                lblLiqDescription.Text = "";
                ddlLiqYear.DataSource = null;
                ddlLiqYear.DataBind();
                ddlLiqFromMonth.DataSource = null;
                ddlLiqFromMonth.DataBind();
                ddlLiqToMonth.DataSource = null;
                ddlLiqToMonth.DataBind();
                gvLiqTotalReport.DataSource = null;
                gvLiqTotalReport.DataBind();
                gvLiqCustReport.DataSource = null;
                gvLiqCustReport.DataBind();
                if (ddlLiqPlant.SelectedIndex > 0)
                {
                    SqlParameter[] spY = new SqlParameter[] { new SqlParameter("@Plant", ddlLiqPlant.SelectedItem.Text) };
                    DataTable dtYear = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Liq_Year", spY, DataAccess.Return_Type.DataTable);
                    if (dtYear.Rows.Count > 0)
                    {
                        ddlLiqYear.DataSource = dtYear;
                        ddlLiqYear.DataTextField = "LiqYear";
                        ddlLiqYear.DataValueField = "LiqYear";
                        ddlLiqYear.DataBind();
                        ddlLiqYear.Items.Insert(0, "CHOOSE");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlLiqYear_IndexChange(object sender, EventArgs e)
        {
            try
            {
                spnLink.Style.Add("display", "none");
                btnLiqDownload.Style.Add("display", "none");
                lblLiqDescription.Text = "";
                ddlLiqFromMonth.DataSource = null;
                ddlLiqFromMonth.DataBind();
                ddlLiqToMonth.DataSource = null;
                ddlLiqToMonth.DataBind();
                gvLiqTotalReport.DataSource = null;
                gvLiqTotalReport.DataBind();
                gvLiqCustReport.DataSource = null;
                gvLiqCustReport.DataBind();
                if (ddlLiqYear.SelectedIndex > 0)
                {
                    SqlParameter[] spY = new SqlParameter[] { 
                        new SqlParameter("@Plant", ddlLiqPlant.SelectedItem.Text), 
                        new SqlParameter("@LiqYear", ddlLiqYear.SelectedItem.Text) 
                    };
                    DataTable dtFMonth = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Liq_FromMonth", spY, DataAccess.Return_Type.DataTable);
                    if (dtFMonth.Rows.Count > 0)
                    {
                        ddlLiqFromMonth.DataSource = dtFMonth;
                        ddlLiqFromMonth.DataTextField = "Liqmonth";
                        ddlLiqFromMonth.DataValueField = "LiqMonthID";
                        ddlLiqFromMonth.DataBind();
                        ddlLiqFromMonth.Items.Insert(0, "CHOOSE");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlLiqFromMonth_IndexChange(object sender, EventArgs e)
        {
            try
            {
                spnLink.Style.Add("display", "none");
                btnLiqDownload.Style.Add("display", "none");
                lblLiqDescription.Text = "";
                ddlLiqToMonth.DataSource = null;
                ddlLiqToMonth.DataBind();
                gvLiqTotalReport.DataSource = null;
                gvLiqTotalReport.DataBind();
                gvLiqCustReport.DataSource = null;
                gvLiqCustReport.DataBind();
                if (ddlLiqFromMonth.SelectedIndex > 0)
                {
                    SqlParameter[] spY = new SqlParameter[] { 
                        new SqlParameter("@Plant", ddlLiqPlant.SelectedItem.Text), 
                        new SqlParameter("@LiqYear", ddlLiqYear.SelectedItem.Text), 
                        new SqlParameter("@LiqFromMonth", ddlLiqFromMonth.SelectedItem.Value) 
                    };
                    DataTable dtTMonth = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Liq_ToMonth", spY, DataAccess.Return_Type.DataTable);
                    if (dtTMonth.Rows.Count > 0)
                    {
                        ddlLiqToMonth.DataSource = dtTMonth;
                        ddlLiqToMonth.DataTextField = "Liqmonth";
                        ddlLiqToMonth.DataValueField = "LiqMonthID";
                        ddlLiqToMonth.DataBind();
                        ddlLiqToMonth.Items.Insert(0, "CHOOSE");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlLiqToMonth_IndexChange(object sender, EventArgs e)
        {
            try
            {
                spnLink.Style.Add("display", "none");
                btnLiqDownload.Style.Add("display", "none");
                lblLiqDescription.Text = "";
                gvLiqTotalReport.DataSource = null;
                gvLiqTotalReport.DataBind();
                gvLiqCustReport.DataSource = null;
                gvLiqCustReport.DataBind();
                if (ddlLiqToMonth.SelectedIndex > 0)
                {
                    SqlParameter[] spY = new SqlParameter[] { 
                        new SqlParameter("@Plant", ddlLiqPlant.SelectedItem.Text), 
                        new SqlParameter("@LiqYear", ddlLiqYear.SelectedItem.Text), 
                        new SqlParameter("@LiqFromMonth", ddlLiqFromMonth.SelectedItem.Value), 
                        new SqlParameter("@LiqToMonth", ddlLiqToMonth.SelectedItem.Value)
                    };
                    DataSet dtRpt = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Liq_Report_Details", spY, DataAccess.Return_Type.DataSet);
                    if (dtRpt.Tables[0].Rows.Count > 0)
                    {
                        gvLiqCustReport.DataSource = dtRpt.Tables[0];
                        gvLiqCustReport.DataBind();

                        ViewState["gvLiqCustReport"] = dtRpt.Tables[0];
                    }
                    if (dtRpt.Tables[1].Rows.Count > 0)
                    {
                        gvLiqTotalReport.DataSource = dtRpt.Tables[1];
                        gvLiqTotalReport.DataBind();
                        ViewState["gvLiqTotalReport"] = dtRpt.Tables[1];

                        gvLiqTotalReport.FooterRow.Cells[1].Text = "TOTAL";
                        gvLiqTotalReport.FooterRow.Cells[2].Text = Convert.ToDecimal(dtRpt.Tables[1].Compute("Sum(A_FWT)", "")).ToString();
                        gvLiqTotalReport.FooterRow.Cells[3].Text = Convert.ToDecimal(dtRpt.Tables[1].Compute("Sum(B_FWT)", "")).ToString();
                        gvLiqTotalReport.FooterRow.Cells[4].Text = Convert.ToDecimal(dtRpt.Tables[1].Compute("Sum(C_FWT)", "")).ToString();
                        gvLiqTotalReport.FooterRow.Cells[5].Text = Convert.ToDecimal(dtRpt.Tables[1].Compute("Sum(D_FWT)", "")).ToString();
                        gvLiqTotalReport.FooterRow.Cells[6].Text = Convert.ToDecimal(dtRpt.Tables[1].Compute("Sum(E_FWT)", "")).ToString();
                        gvLiqTotalReport.FooterRow.Cells[7].Text = Convert.ToDecimal(dtRpt.Tables[1].Compute("Sum(F_FWT)", "")).ToString();
                        gvLiqTotalReport.FooterRow.Cells[8].Text = Convert.ToDecimal(dtRpt.Tables[1].Compute("Sum(G_FWT)", "")).ToString();
                        gvLiqTotalReport.FooterRow.Cells[9].Text = Convert.ToDecimal(dtRpt.Tables[1].Compute("Sum(TOT_FWT)", "")).ToString();
                        Bind_Liq_Chart();

                        string strMsg = "<div style='font-weight: bold;'> 1. All the values are in tons.<br /> " +
                            "2. The data shown are based on stencil nos(YY&MM for part definition) and the system date on which the despatches happened.<br /> " +
                            "3. Part-D/E/F data sometimes includes current production(Part-G) due to order backlogs, stock WO or WO no revision/changes.<br /></div>";

                        lblLiqDescription.Text = strMsg;

                        spnLink.Style.Add("display", "block");
                        btnLiqDownload.Style.Add("display", "block");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnLiqDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTot = ViewState["gvLiqTotalReport"] as DataTable;
                string attachment = "attachment; filename=GSD" + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "_" +
                    DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".xlsx";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dtTot.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dtTot.Rows)
                {
                    tab = "";
                    for (i = 0; i < dtTot.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.Write("\n");
                Response.Write("\n");

                tab = "";
                DataTable dtDetails = ViewState["gvLiqCustReport"] as DataTable;
                foreach (DataColumn dc in dtDetails.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                foreach (DataRow dr in dtDetails.Rows)
                {
                    tab = "";
                    for (i = 0; i < dtDetails.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }

                Response.Flush();
                Response.Clear();
                Response.End();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Liq_Chart()
        {
            try
            {
                DataTable dt = ViewState["gvLiqTotalReport"] as DataTable;
                Dictionary<string, double> a1 = new Dictionary<string, double>();
                Dictionary<string, double> b1 = new Dictionary<string, double>();
                Dictionary<string, double> c1 = new Dictionary<string, double>();
                Dictionary<string, double> d1 = new Dictionary<string, double>();
                Dictionary<string, double> e1 = new Dictionary<string, double>();
                Dictionary<string, double> f1 = new Dictionary<string, double>();
                foreach (DataRow r in dt.Rows)
                {
                    string key = r["YEAR"].ToString().Substring(r["YEAR"].ToString().Length - 2, 2);
                    a1.Add(r["MONTH"].ToString() + " '" + key, Convert.ToDouble(r["A_FWT"].ToString()));
                    b1.Add(r["MONTH"].ToString() + " '" + key, Convert.ToDouble(r["B_FWT"].ToString()));
                    c1.Add(r["MONTH"].ToString() + " '" + key, Convert.ToDouble(r["C_FWT"].ToString()));
                    d1.Add(r["MONTH"].ToString() + " '" + key, Convert.ToDouble(r["D_FWT"].ToString()));
                    e1.Add(r["MONTH"].ToString() + " '" + key, Convert.ToDouble(r["E_FWT"].ToString()));
                    f1.Add(r["MONTH"].ToString() + " '" + key, Convert.ToDouble(r["F_FWT"].ToString()));
                }

                chartLiqColumn.Series["A"].Points.DataBind(a1, "Key", "Value", string.Empty);
                chartLiqColumn.Series["B"].Points.DataBind(b1, "Key", "Value", string.Empty);
                chartLiqColumn.Series["C"].Points.DataBind(c1, "Key", "Value", string.Empty);
                chartLiqColumn.Series["D"].Points.DataBind(d1, "Key", "Value", string.Empty);
                chartLiqColumn.Series["E"].Points.DataBind(e1, "Key", "Value", string.Empty);
                chartLiqColumn.Series["F"].Points.DataBind(f1, "Key", "Value", string.Empty);

                DataTable dtChartPie = new DataTable();
                dtChartPie.Columns.Add("PART", typeof(String));
                dtChartPie.Columns.Add("TOTAL", typeof(Decimal));
                gvLiqTotalReport.FooterRow.Cells[1].Text = "TOTAL";
                dtChartPie.Rows.Add("A", Convert.ToDecimal(dt.Compute("Sum(A_FWT)", "")).ToString());
                dtChartPie.Rows.Add("B", Convert.ToDecimal(dt.Compute("Sum(B_FWT)", "")).ToString());
                dtChartPie.Rows.Add("C", Convert.ToDecimal(dt.Compute("Sum(C_FWT)", "")).ToString());
                dtChartPie.Rows.Add("D", Convert.ToDecimal(dt.Compute("Sum(D_FWT)", "")).ToString());
                dtChartPie.Rows.Add("E", Convert.ToDecimal(dt.Compute("Sum(E_FWT)", "")).ToString());
                dtChartPie.Rows.Add("F", Convert.ToDecimal(dt.Compute("Sum(F_FWT)", "")).ToString());
                dtChartPie.Rows.Add("G", Convert.ToDecimal(dt.Compute("Sum(G_FWT)", "")).ToString());

                Dictionary<string, double> chartData1 = new Dictionary<string, double>();
                foreach (DataRow r in dtChartPie.Rows)
                {
                    chartData1.Add(r["PART"].ToString(), Convert.ToDouble(r["TOTAL"].ToString()));
                }
                chartLiqPie.Series["PART"].Points.DataBind(chartData1, "Key", "Value", string.Empty);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}