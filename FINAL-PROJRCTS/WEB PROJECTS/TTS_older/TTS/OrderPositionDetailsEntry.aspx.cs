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
using System.Globalization;

namespace TTS
{
    public partial class OrderPositionDetailsEntry : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["orderposition_entry"].ToString() == "True")
                        {
                            lblcurrentDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                            DataTable dtMonth = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPositionEntry_Month", DataAccess.Return_Type.DataTable);
                            if (dtMonth.Rows.Count > 0 && dtMonth.Rows[0]["AsMonth"].ToString() != "")
                            {
                                rptMonthRecord.DataSource = dtMonth;
                                rptMonthRecord.DataBind();
                                Bind_Backlog(dtMonth);
                            }

                            lblPreviousEntry.Text = "";
                            lblBacklogEntry.Text = "";
                            btnSavepreviosMonth.Text = "";
                            btnSaveBacklog.Text = "";
                            btnSavepreviosMonth.Style.Add("display", "none");
                            btnSaveBacklog.Style.Add("display", "none");
                            string strMonthName = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
                            if (dtMonth.Rows.Count == 0 || dtMonth.Rows[0]["AsMonth"].ToString() == "" || dtMonth.Rows[0]["AsMonth"].ToString().ToLower() != strMonthName.ToLower())
                            {
                                lblPreviousEntry.Text = "Do you want add any previous months records " +
                                    "<span style='color: #2E2BFB;font-size: 14px;text-decoration: underline;cursor: pointer;' onclick='openpreviousmonth()'>Yes</span>";
                                lblBacklogEntry.Text = "Do you want save the open backlog records " +
                                    "<span style='color: #2E2BFB;font-size: 14px;font-weight: bold;text-decoration: underline;cursor: pointer;' onclick='openbacklog()'>Yes</span>";

                                btnSavepreviosMonth.Text = "SAVE DATA TO " + new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1).ToShortDateString();
                                btnSave.Text = "SAVE DATA TO " + DateTime.Now.ToString("MMMM").ToUpper();
                                btnSaveBacklog.Text = "SAVE BACKLOG DATA TO " + DateTime.Now.ToString("MMMM").ToUpper();
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
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Backlog(DataTable dtBacklog)
        {
            try
            {
                if (dtBacklog.Rows.Count > 0)
                {
                    txtLankaExp_TyreBacklog.Text = dtBacklog.Rows[0]["LankaExp_TyreBacklog"].ToString();
                    txtLankaExp_RimBacklog.Text = dtBacklog.Rows[0]["LankaExp_RimBacklog"].ToString();
                    txtLankaExp_PneumaticsBacklog.Text = dtBacklog.Rows[0]["LankaExp_PneumaticsBacklog"].ToString();

                    txtStarcoExp_TyreBacklog.Text = dtBacklog.Rows[0]["StarcoExp_TyreBacklog"].ToString();
                    txtStarcoExp_RimBacklog.Text = dtBacklog.Rows[0]["StarcoExp_RimBacklog"].ToString();
                    txtStarcoExp_PneumaticsBacklog.Text = dtBacklog.Rows[0]["StarcoExp_PneumaticsBacklog"].ToString();

                    txtmmnExp_TyreBacklog.Text = dtBacklog.Rows[0]["mmnExp_TyreBacklog"].ToString();
                    txtmmnExp_RimBacklog.Text = dtBacklog.Rows[0]["mmnExp_RimBacklog"].ToString();
                    txtmmnExp_PneumaticsBacklog.Text = dtBacklog.Rows[0]["mmnExp_PneumaticsBacklog"].ToString();
                    txtpdkExp_TyreBacklog.Text = dtBacklog.Rows[0]["pdkExp_TyreBacklog"].ToString();
                    txtpdkExp_RimBacklog.Text = dtBacklog.Rows[0]["pdkExp_RimBacklog"].ToString();
                    txtpdkExp_PneumaticsBacklog.Text = dtBacklog.Rows[0]["pdkExp_PneumaticsBacklog"].ToString();
                    txtmmnDom_TyreBacklog.Text = dtBacklog.Rows[0]["mmnDom_TyreBacklog"].ToString();
                    txtmmnDom_RimBacklog.Text = dtBacklog.Rows[0]["mmnDom_RimBacklog"].ToString();
                    txtmmnDom_PneumaticsBacklog.Text = dtBacklog.Rows[0]["mmnDom_PneumaticsBacklog"].ToString();
                    txtpdkDom_TyreBacklog.Text = dtBacklog.Rows[0]["pdkDom_TyreBacklog"].ToString();
                    txtpdkDom_RimBacklog.Text = dtBacklog.Rows[0]["pdkDom_RimBacklog"].ToString();
                    txtpdkDom_PneumaticsBacklog.Text = dtBacklog.Rows[0]["pdkDom_PneumaticsBacklog"].ToString();
                    txtmmnCF_TyreBacklog.Text = dtBacklog.Rows[0]["mmnCF_TyreBacklog"].ToString();
                    txtmmnCF_RimBacklog.Text = dtBacklog.Rows[0]["mmnCF_RimBacklog"].ToString();
                    txtmmnCF_PneumaticsBacklog.Text = dtBacklog.Rows[0]["mmnCF_PneumaticsBacklog"].ToString();
                    txtpdkCF_TyreBacklog.Text = dtBacklog.Rows[0]["pdkCF_TyreBacklog"].ToString();
                    txtpdkCF_RimBacklog.Text = dtBacklog.Rows[0]["pdkCF_RimBacklog"].ToString();
                    txtpdkCF_PneumaticsBacklog.Text = dtBacklog.Rows[0]["pdkCF_PneumaticsBacklog"].ToString();

                    hdnLankaExpTyre.Value = dtBacklog.Rows[0]["LankaExp_TyreBacklog"].ToString();
                    hdnLankaExpRim.Value = dtBacklog.Rows[0]["LankaExp_RimBacklog"].ToString();
                    hdnLankaExpPneu.Value = dtBacklog.Rows[0]["LankaExp_PneumaticsBacklog"].ToString();

                    hdnStarcoExpTyre.Value = dtBacklog.Rows[0]["StarcoExp_TyreBacklog"].ToString();
                    hdnStarcoExpRim.Value = dtBacklog.Rows[0]["StarcoExp_RimBacklog"].ToString();
                    hdnStarcoExpPneu.Value = dtBacklog.Rows[0]["StarcoExp_PneumaticsBacklog"].ToString();

                    hdnmmnExpTyre.Value = dtBacklog.Rows[0]["mmnExp_TyreBacklog"].ToString();
                    hdnmmnExpRim.Value = dtBacklog.Rows[0]["mmnExp_RimBacklog"].ToString();
                    hdnmmnExpPneu.Value = dtBacklog.Rows[0]["mmnExp_PneumaticsBacklog"].ToString();
                    hdnpdkExpTyre.Value = dtBacklog.Rows[0]["pdkExp_TyreBacklog"].ToString();
                    hdnpdkExpRim.Value = dtBacklog.Rows[0]["pdkExp_RimBacklog"].ToString();
                    hdnpdkExpPneu.Value = dtBacklog.Rows[0]["pdkExp_PneumaticsBacklog"].ToString();
                    hdnmmnDomTyre.Value = dtBacklog.Rows[0]["mmnDom_TyreBacklog"].ToString();
                    hdnmmnDomRim.Value = dtBacklog.Rows[0]["mmnDom_RimBacklog"].ToString();
                    hdnmmnDomPneu.Value = dtBacklog.Rows[0]["mmnDom_PneumaticsBacklog"].ToString();
                    hdnpdkDomTyre.Value = dtBacklog.Rows[0]["pdkDom_TyreBacklog"].ToString();
                    hdnpdkDomRim.Value = dtBacklog.Rows[0]["pdkDom_RimBacklog"].ToString();
                    hdnpdkDomPneu.Value = dtBacklog.Rows[0]["pdkDom_PneumaticsBacklog"].ToString();
                    hdnmmnCFTyre.Value = dtBacklog.Rows[0]["mmnCF_TyreBacklog"].ToString();
                    hdnmmnCFRim.Value = dtBacklog.Rows[0]["mmnCF_RimBacklog"].ToString();
                    hdnmmnCFPneu.Value = dtBacklog.Rows[0]["mmnCF_PneumaticsBacklog"].ToString();
                    hdnpdkCFTyre.Value = dtBacklog.Rows[0]["pdkCF_TyreBacklog"].ToString();
                    hdnpdkCFRim.Value = dtBacklog.Rows[0]["pdkCF_RimBacklog"].ToString();
                    hdnpdkCFPneu.Value = dtBacklog.Rows[0]["pdkCF_PneumaticsBacklog"].ToString();

                    ScriptManager.RegisterStartupScript(Page, GetType(), "J1", "SubTotal('tbBacklog');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "J2", "GrandTotal('tbBacklog', 'txtGrandTotal_TotalBacklog');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            Response.Redirect("orderPositionDetailsEntry.aspx", false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Save_PositionEntryData(DateTime.ParseExact(System.DateTime.Now.ToLongDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSavepreviosMonth_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime AsonDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                Save_PositionEntryData(DateTime.ParseExact(AsonDate.ToLongDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSaveBacklog_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spBacklog = new SqlParameter[]{
                    new SqlParameter("@UserName",Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@AsonDate",DateTime.ParseExact(System.DateTime.Now.ToLongDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    new SqlParameter("@BacklogStatus",1),
                    new SqlParameter("@LankaExp_TyreBacklog",txtLankaExp_TyreBacklog.Text.Length > 0 ? Convert.ToDecimal(txtLankaExp_TyreBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@LankaExp_RimBacklog",txtLankaExp_RimBacklog.Text.Length > 0 ? Convert.ToDecimal(txtLankaExp_RimBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@LankaExp_PneumaticsBacklog",txtLankaExp_PneumaticsBacklog.Text.Length > 0 ? Convert.ToDecimal(txtLankaExp_PneumaticsBacklog.Text).ToString("0.00") : "0.00"),
                    
                    new SqlParameter("@StarcoExp_TyreBacklog",txtStarcoExp_TyreBacklog.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExp_TyreBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@StarcoExp_RimBacklog",txtStarcoExp_RimBacklog.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExp_RimBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@StarcoExp_PneumaticsBacklog",txtStarcoExp_PneumaticsBacklog.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExp_PneumaticsBacklog.Text).ToString("0.00") : "0.00"),
                    
                    new SqlParameter("@mmnExp_TyreBacklog",txtmmnExp_TyreBacklog.Text.Length > 0 ? Convert.ToDecimal(txtmmnExp_TyreBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@mmnExp_RimBacklog",txtmmnExp_RimBacklog.Text.Length > 0 ? Convert.ToDecimal(txtmmnExp_RimBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@mmnExp_PneumaticsBacklog",txtmmnExp_PneumaticsBacklog.Text.Length > 0 ? Convert.ToDecimal(txtmmnExp_PneumaticsBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@pdkExp_TyreBacklog",txtpdkExp_TyreBacklog.Text.Length > 0 ? Convert.ToDecimal(txtpdkExp_TyreBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@pdkExp_RimBacklog",txtpdkExp_RimBacklog.Text.Length > 0 ? Convert.ToDecimal(txtpdkExp_RimBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@pdkExp_PneumaticsBacklog",txtpdkExp_PneumaticsBacklog.Text.Length > 0 ? Convert.ToDecimal(txtpdkExp_PneumaticsBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@mmnDom_TyreBacklog",txtmmnDom_TyreBacklog.Text.Length > 0 ? Convert.ToDecimal(txtmmnDom_TyreBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@mmnDom_RimBacklog",txtmmnDom_RimBacklog.Text.Length > 0 ? Convert.ToDecimal(txtmmnDom_RimBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@mmnDom_PneumaticsBacklog",txtmmnDom_PneumaticsBacklog.Text.Length > 0 ? Convert.ToDecimal(txtmmnDom_PneumaticsBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@pdkDom_TyreBacklog",txtpdkDom_TyreBacklog.Text.Length > 0 ? Convert.ToDecimal(txtpdkDom_TyreBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@pdkDom_RimBacklog",txtpdkDom_RimBacklog.Text.Length > 0 ? Convert.ToDecimal(txtpdkDom_RimBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@pdkDom_PneumaticsBacklog",txtpdkDom_PneumaticsBacklog.Text.Length > 0 ? Convert.ToDecimal(txtpdkDom_PneumaticsBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@mmnCF_TyreBacklog",txtmmnCF_TyreBacklog.Text.Length > 0 ? Convert.ToDecimal(txtmmnCF_TyreBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@mmnCF_RimBacklog",txtmmnCF_RimBacklog.Text.Length > 0 ? Convert.ToDecimal(txtmmnCF_RimBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@mmnCF_PneumaticsBacklog",txtmmnCF_PneumaticsBacklog.Text.Length > 0 ? Convert.ToDecimal(txtmmnCF_PneumaticsBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@pdkCF_TyreBacklog",txtpdkCF_TyreBacklog.Text.Length > 0 ? Convert.ToDecimal(txtpdkCF_TyreBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@pdkCF_RimBacklog",txtpdkCF_RimBacklog.Text.Length > 0 ? Convert.ToDecimal(txtpdkCF_RimBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@pdkCF_PneumaticsBacklog",txtpdkCF_PneumaticsBacklog.Text.Length > 0 ? Convert.ToDecimal(txtpdkCF_PneumaticsBacklog.Text).ToString("0.00") : "0.00")
                };
                int resp = daErrDB.ExecuteNonQuery_SP("sp_ins_OrderPositionBacklog", spBacklog);
                if (resp > 0)
                    Response.Redirect("orderPositionDetailsEntry.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Save_PositionEntryData(DateTime AsonDate)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]{
                new SqlParameter("@UserName",Request.Cookies["TTSUser"].Value),
                new SqlParameter("@AsonDate",AsonDate),
                new SqlParameter("@LankaExp_TyreInflow",txtLankaExp_TyreInflow.Text.Length > 0 ? Convert.ToDecimal(txtLankaExp_TyreInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@LankaExp_TyreDespatch",txtLankaExp_TyreDespatch.Text.Length > 0 ? Convert.ToDecimal(txtLankaExp_TyreDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@LankaExp_RimInflow",txtLankaExp_RimInflow.Text.Length > 0 ? Convert.ToDecimal(txtLankaExp_RimInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@LankaExp_RimDespatch",txtLankaExp_RimDespatch.Text.Length > 0 ? Convert.ToDecimal(txtLankaExp_RimDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@LankaExp_PneumaticsInflow",txtLankaExp_PneumaticsInflow.Text.Length > 0 ? Convert.ToDecimal(txtLankaExp_PneumaticsInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@LankaExp_PneumaticsDespatch",txtLankaExp_PneumaticsDespatch.Text.Length > 0 ? Convert.ToDecimal(txtLankaExp_PneumaticsDespatch.Text).ToString("0.00") : "0.00"),

                new SqlParameter("@StarcoExp_TyreInflow",txtStarcoExp_TyreInflow.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExp_TyreInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@StarcoExp_TyreDespatch",txtStarcoExp_TyreDespatch.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExp_TyreDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@StarcoExp_RimInflow",txtStarcoExp_RimInflow.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExp_RimInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@StarcoExp_RimDespatch",txtStarcoExp_RimDespatch.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExp_RimDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@StarcoExp_PneumaticsInflow",txtStarcoExp_PneumaticsInflow.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExp_PneumaticsInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@StarcoExp_PneumaticsDespatch",txtStarcoExp_PneumaticsDespatch.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExp_PneumaticsDespatch.Text).ToString("0.00") : "0.00"),

                new SqlParameter("@mmnExp_TyreInflow",txtmmnExp_TyreInflow.Text.Length > 0 ? Convert.ToDecimal(txtmmnExp_TyreInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnExp_TyreDespatch",txtmmnExp_TyreDespatch.Text.Length > 0 ? Convert.ToDecimal(txtmmnExp_TyreDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnExp_RimInflow",txtmmnExp_RimInflow.Text.Length > 0 ? Convert.ToDecimal(txtmmnExp_RimInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnExp_RimDespatch",txtmmnExp_RimDespatch.Text.Length > 0 ? Convert.ToDecimal(txtmmnExp_RimDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnExp_PneumaticsInflow",txtmmnExp_PneumaticsInflow.Text.Length > 0 ? Convert.ToDecimal(txtmmnExp_PneumaticsInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnExp_PneumaticsDespatch",txtmmnExp_PneumaticsDespatch.Text.Length > 0 ? Convert.ToDecimal(txtmmnExp_PneumaticsDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkExp_TyreInflow",txtpdkExp_TyreInflow.Text.Length > 0 ? Convert.ToDecimal(txtpdkExp_TyreInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkExp_TyreDespatch",txtpdkExp_TyreDespatch.Text.Length > 0 ? Convert.ToDecimal(txtpdkExp_TyreDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkExp_RimInflow",txtpdkExp_RimInflow.Text.Length > 0 ? Convert.ToDecimal(txtpdkExp_RimInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkExp_RimDespatch",txtpdkExp_RimDespatch.Text.Length > 0 ? Convert.ToDecimal(txtpdkExp_RimDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkExp_PneumaticsInflow",txtpdkExp_PneumaticsInflow.Text.Length > 0 ? Convert.ToDecimal(txtpdkExp_PneumaticsInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkExp_PneumaticsDespatch",txtpdkExp_PneumaticsDespatch.Text.Length > 0 ? Convert.ToDecimal(txtpdkExp_PneumaticsDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnDom_TyreInflow",txtmmnDom_TyreInflow.Text.Length > 0 ? Convert.ToDecimal(txtmmnDom_TyreInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnDom_TyreDespatch",txtmmnDom_TyreDespatch.Text.Length > 0 ? Convert.ToDecimal(txtmmnDom_TyreDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnDom_RimInflow",txtmmnDom_RimInflow.Text.Length > 0 ? Convert.ToDecimal(txtmmnDom_RimInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnDom_RimDespatch",txtmmnDom_RimDespatch.Text.Length > 0 ? Convert.ToDecimal(txtmmnDom_RimDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnDom_PneumaticsInflow",txtmmnDom_PneumaticsInflow.Text.Length > 0 ? Convert.ToDecimal(txtmmnDom_PneumaticsInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnDom_PneumaticsDespatch",txtmmnDom_PneumaticsDespatch.Text.Length > 0 ? Convert.ToDecimal(txtmmnDom_PneumaticsDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkDom_TyreInflow",txtpdkDom_TyreInflow.Text.Length > 0 ? Convert.ToDecimal(txtpdkDom_TyreInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkDom_TyreDespatch",txtpdkDom_TyreDespatch.Text.Length > 0 ? Convert.ToDecimal(txtpdkDom_TyreDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkDom_RimInflow",txtpdkDom_RimInflow.Text.Length > 0 ? Convert.ToDecimal(txtpdkDom_RimInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkDom_RimDespatch",txtpdkDom_RimDespatch.Text.Length > 0 ? Convert.ToDecimal(txtpdkDom_RimDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkDom_PneumaticsInflow",txtpdkDom_PneumaticsInflow.Text.Length > 0 ? Convert.ToDecimal(txtpdkDom_PneumaticsInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkDom_PneumaticsDespatch",txtpdkDom_PneumaticsDespatch.Text.Length > 0 ? Convert.ToDecimal(txtpdkDom_PneumaticsDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnCF_TyreInflow",txtmmnCF_TyreInflow.Text.Length > 0 ? Convert.ToDecimal(txtmmnCF_TyreInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnCF_TyreDespatch",txtmmnCF_TyreDespatch.Text.Length > 0 ? Convert.ToDecimal(txtmmnCF_TyreDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnCF_RimInflow",txtmmnCF_RimInflow.Text.Length > 0 ? Convert.ToDecimal(txtmmnCF_RimInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnCF_RimDespatch",txtmmnCF_RimDespatch.Text.Length > 0 ? Convert.ToDecimal(txtmmnCF_RimDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnCF_PneumaticsInflow",txtmmnCF_PneumaticsInflow.Text.Length > 0 ? Convert.ToDecimal(txtmmnCF_PneumaticsInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@mmnCF_PneumaticsDespatch",txtmmnCF_PneumaticsDespatch.Text.Length > 0 ? Convert.ToDecimal(txtmmnCF_PneumaticsDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkCF_TyreInflow",txtpdkCF_TyreInflow.Text.Length > 0 ? Convert.ToDecimal(txtpdkCF_TyreInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkCF_TyreDespatch",txtpdkCF_TyreDespatch.Text.Length > 0 ? Convert.ToDecimal(txtpdkCF_TyreDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkCF_RimInflow",txtpdkCF_RimInflow.Text.Length > 0 ? Convert.ToDecimal(txtpdkCF_RimInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkCF_RimDespatch",txtpdkCF_RimDespatch.Text.Length > 0 ? Convert.ToDecimal(txtpdkCF_RimDespatch.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkCF_PneumaticsInflow",txtpdkCF_PneumaticsInflow.Text.Length > 0 ? Convert.ToDecimal(txtpdkCF_PneumaticsInflow.Text).ToString("0.00") : "0.00"),
                new SqlParameter("@pdkCF_PneumaticsDespatch",txtpdkCF_PneumaticsDespatch.Text.Length > 0 ? Convert.ToDecimal(txtpdkCF_PneumaticsDespatch.Text).ToString("0.00") : "0.00")
                };
                int resp = daErrDB.ExecuteNonQuery_SP("SP_ins_OrderPositionEntry", sp);
                if (resp > 0)
                    Response.Redirect("orderPositionDetailsEntry.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}