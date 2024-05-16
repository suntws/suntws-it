using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.DataVisualization.Charting;

namespace TTS
{
    public partial class claimreview : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["claim_track"].ToString() == "True")
                        {
                            if (Request["crid"] != null && Request["crid"].ToString() == "c")
                            {
                                lblReviewCustomer.Text = "CUSTOMER";
                                ddlClaimCustomer.Visible = true;
                                ddlClaimCustUser.Visible = true;
                                ddlClaimPlant.Visible = false;
                                DataTable dtClaimCust = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claimCustomer", DataAccess.Return_Type.DataTable);
                                if (dtClaimCust.Rows.Count > 0)
                                {
                                    ddlClaimCustomer.DataSource = dtClaimCust;
                                    ddlClaimCustomer.DataTextField = "custname";
                                    ddlClaimCustomer.DataValueField = "custname";
                                    ddlClaimCustomer.DataBind();
                                    ddlClaimCustomer.Items.Insert(0, "CHOOSE");
                                }
                            }
                            else if (Request["crid"] != null && Request["crid"].ToString() == "g")
                            {
                                lblReviewCustomer.Text = "PLANT";
                                ddlClaimCustomer.Visible = false;
                                ddlClaimCustUser.Visible = false;
                                ddlClaimPlant.Visible = true;
                            }
                            Array ChartTypes = Enum.GetValues(typeof(SeriesChartType));
                            foreach (var item in ChartTypes)
                            {
                                if (item.ToString().ToLower() != "renko" && item.ToString().ToLower() != "threelinebreak" && item.ToString().ToLower() != "kagi" && item.ToString().ToLower() != "pointandfigure" && item.ToString().ToLower() != "errorbar")
                                    ddlChartType.Items.Add(item.ToString());
                            }
                            ddlChartType.Text = "Column";
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
        protected void rdbReviewType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                hdnClaimReviewType.Value = rdbReviewType.SelectedItem.Text;
                if (Request["crid"].ToString() == "c")
                {
                    ddlClaimCustomer.SelectedIndex = -1;
                    ddlClaimCustUser.Items.Clear();
                }
                else
                {
                    ddlClaimPlant.SelectedIndex = -1;
                }
                ddlClaimYear.Items.Clear();
                ddlReviewPeriod.Items.Clear();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlClaimCustomer_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custname", ddlClaimCustomer.SelectedItem.Text);
                DataTable dtClaimUserID = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claimCustomer_userID", sp1, DataAccess.Return_Type.DataTable);
                if (dtClaimUserID.Rows.Count > 0)
                {
                    ddlClaimCustUser.DataSource = dtClaimUserID;
                    ddlClaimCustUser.DataTextField = "username";
                    ddlClaimCustUser.DataValueField = "ID";
                    ddlClaimCustUser.DataBind();
                    if (dtClaimUserID.Rows.Count == 1)
                        ddlClaimCustUser_IndexChange(sender, e);
                    else
                        ddlClaimCustUser.Items.Insert(0, "CHOOSE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlClaimCustUser_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", ddlClaimCustUser.SelectedItem.Value);
                sp1[1] = new SqlParameter("@Plant", "");
                DataTable dtClaimYear = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claimCustomer_Year", sp1, DataAccess.Return_Type.DataTable);
                if (dtClaimYear.Rows.Count > 0)
                {
                    ddlClaimYear.DataSource = dtClaimYear;
                    ddlClaimYear.DataTextField = "ClaimYear";
                    ddlClaimYear.DataValueField = "ClaimYear";
                    ddlClaimYear.DataBind();
                    ddlClaimYear.Items.Insert(0, "CHOOSE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlClaimPlant_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spYear = new SqlParameter[2];
                spYear[0] = new SqlParameter("@custcode", "");
                spYear[1] = new SqlParameter("@Plant", ddlClaimPlant.SelectedItem.Value);
                DataTable dtClaimYear = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claimCustomer_Year", spYear, DataAccess.Return_Type.DataTable);
                if (dtClaimYear.Rows.Count > 0)
                {
                    ddlClaimYear.DataSource = dtClaimYear;
                    ddlClaimYear.DataTextField = "ClaimYear";
                    ddlClaimYear.DataValueField = "ClaimYear";
                    ddlClaimYear.DataBind();
                    ddlClaimYear.Items.Insert(0, "CHOOSE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlClaimYear_IndexChange(object sender, EventArgs e)
        {
            try
            {
                DataTable dtClaimPeriod = new DataTable();
                if (hdnClaimReviewType.Value == "MONTHLY")
                {
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@custcode", Request["crid"].ToString() == "c" ? ddlClaimCustUser.SelectedItem.Value : "");
                    sp1[1] = new SqlParameter("@Year", ddlClaimYear.SelectedItem.Text);
                    sp1[2] = new SqlParameter("@Plant", ddlClaimPlant.SelectedItem.Text);
                    dtClaimPeriod = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReview_Period", sp1, DataAccess.Return_Type.DataTable);
                }
                else if (hdnClaimReviewType.Value == "QUARTERLY")
                {
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@custcode", Request["crid"].ToString() == "c" ? ddlClaimCustUser.SelectedItem.Value : "");
                    sp1[1] = new SqlParameter("@Year", ddlClaimYear.SelectedItem.Text);
                    sp1[2] = new SqlParameter("@Plant", ddlClaimPlant.SelectedItem.Text);
                    DataTable dtClaimQuater = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReview_Period", sp1, DataAccess.Return_Type.DataTable);

                    DataColumn col = new DataColumn("MonthNo", typeof(System.String));
                    dtClaimPeriod.Columns.Add(col);
                    col = new DataColumn("ClaimMonth", typeof(System.String));
                    dtClaimPeriod.Columns.Add(col);
                    if (dtClaimQuater.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtClaimQuater.Select("MonthNo in (1,2,3)"))
                        {
                            dtClaimPeriod.Rows.Add("1,2,3", "JAN-MAR");
                            break;
                        }
                        foreach (DataRow row in dtClaimQuater.Select("MonthNo in (4,5,6)"))
                        {
                            dtClaimPeriod.Rows.Add("4,5,6", "APR-JUN");
                            break;
                        }
                        foreach (DataRow row in dtClaimQuater.Select("MonthNo in (7,8,9)"))
                        {
                            dtClaimPeriod.Rows.Add("7,8,9", "JUL-SEP");
                            break;
                        }
                        foreach (DataRow row in dtClaimQuater.Select("MonthNo in (10,11,12)"))
                        {
                            dtClaimPeriod.Rows.Add("10,11,12", "OCT-DEC");
                            break;
                        }
                    }
                }
                else if (hdnClaimReviewType.Value == "YEARLY")
                {
                    DataColumn col = new DataColumn("MonthNo", typeof(System.String));
                    dtClaimPeriod.Columns.Add(col);
                    col = new DataColumn("ClaimMonth", typeof(System.String));
                    dtClaimPeriod.Columns.Add(col);
                    dtClaimPeriod.Rows.Add("1,2,3,4,5,6,7,8,9,10,11,12", "JAN-DEC");
                }
                if (dtClaimPeriod.Rows.Count > 0)
                {
                    ddlReviewPeriod.DataSource = dtClaimPeriod;
                    ddlReviewPeriod.DataTextField = "ClaimMonth";
                    ddlReviewPeriod.DataValueField = "MonthNo";
                    ddlReviewPeriod.DataBind();
                    ddlReviewPeriod.Items.Insert(0, "CHOOSE");
                }
                if (hdnClaimReviewType.Value == "YEARLY")
                {
                    ddlReviewPeriod.SelectedIndex = 1;
                    ddlReviewPeriod_IndexChange(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlReviewPeriod_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Int32 monthNo = 0; Int32 qMonthNo = 0;
                if (hdnClaimReviewType.Value == "MONTHLY")
                {
                    monthNo = Convert.ToInt32(ddlReviewPeriod.SelectedItem.Value);
                }
                else if (hdnClaimReviewType.Value == "QUARTERLY")
                {
                    string[] strPeriod = (ddlReviewPeriod.SelectedItem.Value).Split(',');
                    monthNo = Convert.ToInt32(strPeriod[0].ToString());
                    qMonthNo = Convert.ToInt32(strPeriod[2].ToString());
                }
                else if (hdnClaimReviewType.Value == "YEARLY")
                {
                    monthNo = 0;
                    qMonthNo = 0;
                }
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@custcode", Request["crid"].ToString() == "c" ? ddlClaimCustUser.SelectedItem.Value : "");
                sp1[1] = new SqlParameter("@Year", ddlClaimYear.SelectedItem.Text);
                sp1[2] = new SqlParameter("@Month", monthNo);
                sp1[3] = new SqlParameter("@PeriodType", hdnClaimReviewType.Value);
                sp1[4] = new SqlParameter("@QMonth", qMonthNo);
                sp1[5] = new SqlParameter("@Plant", Request["crid"].ToString() == "g" ? ddlClaimPlant.SelectedItem.Value : "");
                DataTable dtChartTyreSize = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReview_ChartSizeWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtChartTyreSize.Rows.Count > 0)
                {
                    ViewState["dtChartTyreSize"] = dtChartTyreSize;
                    Bind_ClaimReviewChart(dtChartTyreSize, "SIZEWISE");
                }

                sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@custcode", Request["crid"].ToString() == "c" ? ddlClaimCustUser.SelectedItem.Value : "");
                sp1[1] = new SqlParameter("@Year", ddlClaimYear.SelectedItem.Text);
                sp1[2] = new SqlParameter("@Month", monthNo);
                sp1[3] = new SqlParameter("@PeriodType", hdnClaimReviewType.Value);
                sp1[4] = new SqlParameter("@QMonth", qMonthNo);
                sp1[5] = new SqlParameter("@Plant", Request["crid"].ToString() == "g" ? ddlClaimPlant.SelectedItem.Value : "");
                DataTable dtChartType = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReview_ChartTypeWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtChartType.Rows.Count > 0)
                {
                    ViewState["dtChartType"] = dtChartType;
                    Bind_ClaimReviewChart(dtChartType, "TYPEWISE");
                }

                sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@custcode", Request["crid"].ToString() == "c" ? ddlClaimCustUser.SelectedItem.Value : "");
                sp1[1] = new SqlParameter("@Year", ddlClaimYear.SelectedItem.Text);
                sp1[2] = new SqlParameter("@Month", monthNo);
                sp1[3] = new SqlParameter("@PeriodType", hdnClaimReviewType.Value);
                sp1[4] = new SqlParameter("@QMonth", qMonthNo);
                sp1[5] = new SqlParameter("@Plant", Request["crid"].ToString() == "g" ? ddlClaimPlant.SelectedItem.Value : "");
                DataTable dtChartDefect = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReview_ChartDefectsWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtChartDefect.Rows.Count > 0)
                {
                    ViewState["dtChartDefect"] = dtChartDefect;
                    Bind_ClaimReviewChart(dtChartDefect, "DEFECTWISE");
                }

                sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@custcode", Request["crid"].ToString() == "c" ? ddlClaimCustUser.SelectedItem.Value : "");
                sp1[1] = new SqlParameter("@Year", ddlClaimYear.SelectedItem.Text);
                sp1[2] = new SqlParameter("@Month", monthNo);
                sp1[3] = new SqlParameter("@PeriodType", hdnClaimReviewType.Value);
                sp1[4] = new SqlParameter("@QMonth", qMonthNo);
                sp1[5] = new SqlParameter("@Plant", Request["crid"].ToString() == "g" ? ddlClaimPlant.SelectedItem.Value : "");
                DataTable dtChartMonth = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReview_ChartMonthWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtChartMonth.Rows.Count > 0)
                {
                    ViewState["dtChartMonth"] = dtChartMonth;
                    Bind_ClaimReviewChart(dtChartMonth, "MONTHWISE");
                }

                sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@custcode", Request["crid"].ToString() == "c" ? ddlClaimCustUser.SelectedItem.Value : "");
                sp1[1] = new SqlParameter("@Year", ddlClaimYear.SelectedItem.Text);
                sp1[2] = new SqlParameter("@Month", monthNo);
                sp1[3] = new SqlParameter("@PeriodType", hdnClaimReviewType.Value);
                sp1[4] = new SqlParameter("@QMonth", qMonthNo);
                sp1[5] = new SqlParameter("@Plant", Request["crid"].ToString() == "g" ? ddlClaimPlant.SelectedItem.Value : "");
                DataTable dtChartClassification = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReview_ChartClassificationWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtChartClassification.Rows.Count > 0)
                {
                    ViewState["dtChartClassification"] = dtChartClassification;
                    Bind_ClaimReviewChart(dtChartClassification, "CLASSIFICATIONWISE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ClaimReviewChart(DataTable dtChart, string strChartWise)
        {
            Dictionary<string, double> chartData1 = new Dictionary<string, double>();
            if (strChartWise == "SIZEWISE")
            {
                foreach (DataRow r in dtChart.Rows)
                {
                    chartData1.Add(r["tyresize"].ToString(), Convert.ToDouble(r["qty"].ToString()));
                }
                chartTyreSize.Series["tyresize"].Points.DataBind(chartData1, "Key", "Value", string.Empty);
            }
            else if (strChartWise == "TYPEWISE")
            {
                foreach (DataRow r in dtChart.Rows)
                {
                    chartData1.Add(r["tyretype"].ToString(), Convert.ToDouble(r["qty"].ToString()));
                }
                chartTyreType.Series["tyretype"].Points.DataBind(chartData1, "Key", "Value", string.Empty);
            }
            else if (strChartWise == "DEFECTWISE")
            {
                foreach (DataRow r in dtChart.Rows)
                {
                    chartData1.Add(r["ClaimDescription"].ToString(), Convert.ToDouble(r["qty"].ToString()));
                }
                chartDefect.Series["ClaimDescription"].Points.DataBind(chartData1, "Key", "Value", string.Empty);
            }
            else if (strChartWise == "MONTHWISE")
            {
                foreach (DataRow r in dtChart.Rows)
                {
                    chartData1.Add(r["ClaimMonth"].ToString(), Convert.ToDouble(r["qty"].ToString()));
                }
                chartMonthQty.Series["ClaimMonth"].Points.DataBind(chartData1, "Key", "Value", string.Empty);
            }
            else if (strChartWise == "CLASSIFICATIONWISE")
            {
                foreach (DataRow r in dtChart.Rows)
                {
                    chartData1.Add(r["Classification"].ToString(), Convert.ToDouble(r["qty"].ToString()));
                }
                chartClassification.Series["Classification"].Points.DataBind(chartData1, "Key", "Value", string.Empty);
            }
        }
        protected void ddlChartType_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                chartTyreSize.Series["tyresize"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartType.SelectedItem.Text);
                Bind_ClaimReviewChart(ViewState["dtChartTyreSize"] as DataTable, "SIZEWISE");
                chartTyreType.Series["tyretype"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartType.SelectedItem.Text);
                Bind_ClaimReviewChart(ViewState["dtChartType"] as DataTable, "TYPEWISE");
                chartDefect.Series["ClaimDescription"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartType.SelectedItem.Text);
                Bind_ClaimReviewChart(ViewState["dtChartDefect"] as DataTable, "DEFECTWISE");
                chartMonthQty.Series["ClaimMonth"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartType.SelectedItem.Text);
                Bind_ClaimReviewChart(ViewState["dtChartMonth"] as DataTable, "MONTHWISE");
                chartClassification.Series["Classification"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), "Pie");
                Bind_ClaimReviewChart(ViewState["dtChartClassification"] as DataTable, "CLASSIFICATIONWISE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}