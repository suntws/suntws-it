using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;

namespace TTS
{
    public partial class orderpositionshow : System.Web.UI.Page
    {
        DataAccess daErrDB = new DataAccess(ConfigurationManager.ConnectionStrings["ErrDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["orderposition_all"].ToString() == "True")
                        {
                            Bind_TodayValues();
                            Bind_MonthValues();
                            Bind_12MonthValues();

                            Bind_12MonthChartValues();
                            bind_Chart();
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
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_TodayValues()
        {
            try
            {
                DataTable dt = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPosition_Today", DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    rptOrderPositionToday.DataSource = dt;
                    rptOrderPositionToday.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_MonthValues()
        {
            try
            {
                DataTable dt = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPosition_Month", DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    rptOrderPositionMonth.DataSource = dt;
                    rptOrderPositionMonth.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_12MonthValues()
        {
            try
            {
                DataTable dtCur = new DataTable();
                dtCur = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPosition_CurYear", DataAccess.Return_Type.DataTable);

                if (dtCur.Rows.Count > 0)
                {
                    DataTable dtPrev = new DataTable();
                    dtPrev = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPosition_PrevYear", DataAccess.Return_Type.DataTable);

                    DataTable dtMerge = new DataTable();
                    dtMerge.Merge(dtCur);
                    dtMerge.Merge(dtPrev);

                    dtMerge.Columns.Add("MonthField", typeof(string));

                    foreach (DataRow row in dtMerge.Rows)
                    {
                        DateTime date = new DateTime(1, Convert.ToInt32(row["AsMonth"].ToString()), 1);
                        row["MonthField"] = date.ToString("MMM").ToUpper();
                    }

                    if (dtMerge.Rows.Count > 0)
                    {
                        gv_Last12Months.DataSource = dtMerge;
                        gv_Last12Months.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bind_Chart()
        {
            try
            {
                DataTable dt = ViewState["12MonthsChart"] as DataTable;
                Dictionary<string, double> chartData1 = new Dictionary<string, double>();
                Dictionary<string, double> chartData2 = new Dictionary<string, double>();
                Dictionary<string, double> chartData3 = new Dictionary<string, double>();
                foreach (DataRow r in dt.Rows)
                {
                    string key = r["AsYear"].ToString().Substring(r["AsYear"].ToString().Length - 2, 2);
                    chartData1.Add(r["MonthField"].ToString() + " '" + key, Convert.ToDouble(r["MonthInflow"].ToString()));
                    chartData2.Add(r["MonthField"].ToString() + " '" + key, Convert.ToDouble(r["MonthBacklog"].ToString()));
                    chartData3.Add(r["MonthField"].ToString() + " '" + key, Convert.ToDouble(r["MonthDispatch"].ToString()));
                }

                Chart1.Series["Inflow"].Points.DataBind(chartData1, "Key", "Value", string.Empty);
                Chart1.Series["Closing"].Points.DataBind(chartData2, "Key", "Value", string.Empty);
                Chart1.Series["Dispatch"].Points.DataBind(chartData3, "Key", "Value", string.Empty);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_12MonthChartValues()
        {
            try
            {
                DataTable dtMerge = new DataTable();
                DataTable dtCur = new DataTable();
                dtCur = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPosition_CurYear", DataAccess.Return_Type.DataTable);

                DataView viewCur = new DataView(dtCur);
                viewCur.Sort = "AsMonth ASC";
                if (dtCur.Rows.Count > 0)
                {
                    DataTable dtPrev = new DataTable();
                    dtPrev = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPosition_PrevYear", DataAccess.Return_Type.DataTable);

                    DataView viewPrev = new DataView(dtPrev);
                    viewPrev.Sort = "AsMonth ASC";

                    if (dtPrev.Rows.Count > 0)
                        dtMerge.Merge(viewPrev.ToTable());

                    dtMerge.Merge(viewCur.ToTable());

                    dtMerge.Columns.Add("MonthField", typeof(string));

                    foreach (DataRow row in dtMerge.Rows)
                    {
                        DateTime date = new DateTime(1, Convert.ToInt32(row["AsMonth"].ToString()), 1);
                        row["MonthField"] = date.ToString("MMM").ToUpper();
                    }
                    ViewState["12MonthsChart"] = dtMerge;

                    Array ChartTypes = Enum.GetValues(typeof(SeriesChartType));

                    foreach (var item in ChartTypes)
                    {
                        if (item.ToString().ToLower() != "renko" && item.ToString().ToLower() != "threelinebreak" && item.ToString().ToLower() != "kagi" && item.ToString().ToLower() != "pointandfigure" && item.ToString().ToLower() != "errorbar")
                            ddlChartType.Items.Add(item.ToString());
                    }
                    ddlChartType.Text = "Column";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlChartType_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                Chart1.Series["Inflow"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartType.SelectedItem.Text);
                Chart1.Series["Closing"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartType.SelectedItem.Text);
                Chart1.Series["Dispatch"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartType.SelectedItem.Text);
                bind_Chart();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}