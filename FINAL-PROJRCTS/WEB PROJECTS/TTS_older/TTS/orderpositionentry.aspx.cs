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
    public partial class orderpositionentry : System.Web.UI.Page
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
                            Bind_txtValues();
                            Bind_MonthValues();
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

        private void Bind_txtValues()
        {
            try
            {
                lblCurDate.Text = System.DateTime.Now.ToShortDateString();
                DataTable dt = new DataTable();
                dt = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPosition", DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    rptOrderPosition.DataSource = dt;
                    rptOrderPosition.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnPositionSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[]{
                    new SqlParameter("@AsOnDate", System.DateTime.Now),
                    new SqlParameter("@LankaExpInflow", txtLankaExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtLankaExpInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@LankaExpBacklog", txtLankaExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtLankaExpBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@LankaExpDispatch", txtLankaExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtLankaExpDispatch.Text).ToString("0.00") : "0.00"),

                    new SqlParameter("@StarcoExpInflow", txtStarcoExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExpInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@StarcoExpBacklog", txtStarcoExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExpBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@StarcoExpDispatch", txtStarcoExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExpDispatch.Text).ToString("0.00") : "0.00"),

                    new SqlParameter("@StarcoJobWorkInflow", txtStarcoJobWorkInflow.Text.Length > 0 ? Convert.ToDecimal(txtStarcoJobWorkInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@StarcoJobWorkBacklog", txtStarcoJobWorkBacklog.Text.Length > 0 ? Convert.ToDecimal(txtStarcoJobWorkBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@StarcoJobWorkDispatch", txtStarcoJobWorkDispatch.Text.Length > 0 ? Convert.ToDecimal(txtStarcoJobWorkDispatch.Text).ToString("0.00") : "0.00"),

                    new SqlParameter("@MmnExpInflow", txtMMNExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtMMNExpInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnExpBacklog", txtMMNExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtMMNExpBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnExpDispatch", txtMMNExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtMMNExpDispatch.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnDomInflow", txtMMNDomInflow.Text.Length > 0 ? Convert.ToDecimal(txtMMNDomInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnDomBacklog", txtMMNDomBacklog.Text.Length > 0 ? Convert.ToDecimal(txtMMNDomBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnDomDispatch", txtMMNDomDispatch.Text.Length > 0 ? Convert.ToDecimal(txtMMNDomDispatch.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@InflowTot", hdnInflowTot.Value.Length > 0 ? Convert.ToDecimal(hdnInflowTot.Value).ToString("0.00") : "0.00"),
                    new SqlParameter("@BacklogTot", hdnBacklogTot.Value.Length > 0 ? Convert.ToDecimal(hdnBacklogTot.Value).ToString("0.00") : "0.00"),
                    new SqlParameter("@DispatchTot", hdnDispatchTot.Value.Length > 0 ? Convert.ToDecimal(hdnDispatchTot.Value).ToString("0.00") : "0.00"),
                    new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@PdkDomInflow", txtPDKDomInflow.Text.Length > 0 ? Convert.ToDecimal(txtPDKDomInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkDomBacklog", txtPDKDomBacklog.Text.Length > 0 ? Convert.ToDecimal(txtPDKDomBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkDomDispatch", txtPDKDomDispatch.Text.Length > 0 ? Convert.ToDecimal(txtPDKDomDispatch.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkExpInflow", txtPdkExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtPdkExpInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkExpBacklog", txtPDKExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtPDKExpBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkExpDispatch", txtPDKExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtPDKExpDispatch.Text).ToString("0.00") : "0.00"),

                    new SqlParameter("@PneuExpInflow", txtPneuExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtPneuExpInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PneuExpBacklog", txtPneuExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtPneuExpBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PneuExpDispatch", txtPneuExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtPneuExpDispatch.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@RimsExpInflow", txtRimsExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtRimsExpInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@RimsExpBacklog", txtRimsExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtRimsExpBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@RimsExpDispatch", txtRimsExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtRimsExpDispatch.Text).ToString("0.00") : "0.00"),

                    new SqlParameter("@CfDomInflow", txtCFDomInflow.Text.Length > 0 ? Convert.ToDecimal(txtCFDomInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@CfDomBacklog", txtCFDomBacklog.Text.Length > 0 ? Convert.ToDecimal(txtCFDomBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@CfDomDispatch", txtCFDomDispatch.Text.Length > 0 ? Convert.ToDecimal(txtCFDomDispatch.Text).ToString("0.00") : "0.00"),

                    new SqlParameter("@MmnMeInflow", txtMmnMeInflow.Text.Length > 0 ? Convert.ToDecimal(txtMmnMeInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnMeBacklog", txtMmnMeBacklog.Text.Length > 0 ? Convert.ToDecimal(txtMmnMeBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnMeDispatch", txtMmnMeDispatch.Text.Length > 0 ? Convert.ToDecimal(txtMmnMeDispatch.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkMeInflow", txtPdkMeInflow.Text.Length > 0 ? Convert.ToDecimal(txtPdkMeInflow.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkMeBacklog", txtPdkMeBacklog.Text.Length > 0 ? Convert.ToDecimal(txtPdkMeBacklog.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkMeDispatch", txtPdkMeDispatch.Text.Length > 0 ? Convert.ToDecimal(txtPdkMeDispatch.Text).ToString("0.00") : "0.00")
                };
                daErrDB.ExecuteNonQuery_SP("sp_ins_OrderPosition", sp1);

                Response.Redirect("orderpositionentry.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnPreviousMonthSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnPreviousMonthSave.Text != "")
                {
                    DateTime now = DateTime.Now;
                    DateTime lastDayLastMonth = new DateTime(now.Year, now.Month, 1);
                    lastDayLastMonth = lastDayLastMonth.AddDays(-1);

                    SqlParameter[] sp1 = new SqlParameter[]{
                        new SqlParameter("@AsOnDate", lastDayLastMonth),
                        new SqlParameter("@LankaExpInflow", txtLankaExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtLankaExpInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@LankaExpBacklog", txtLankaExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtLankaExpBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@LankaExpDispatch", txtLankaExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtLankaExpDispatch.Text).ToString("0.00") : "0.00"),

                        new SqlParameter("@StarcoExpInflow", txtStarcoExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExpInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@StarcoExpBacklog", txtStarcoExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExpBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@StarcoExpDispatch", txtStarcoExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtStarcoExpDispatch.Text).ToString("0.00") : "0.00"),

                        new SqlParameter("@StarcoJobWorkInflow", txtStarcoJobWorkInflow.Text.Length > 0 ? Convert.ToDecimal(txtStarcoJobWorkInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@StarcoJobWorkBacklog", txtStarcoJobWorkBacklog.Text.Length > 0 ? Convert.ToDecimal(txtStarcoJobWorkBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@StarcoJobWorkDispatch", txtStarcoJobWorkDispatch.Text.Length > 0 ? Convert.ToDecimal(txtStarcoJobWorkDispatch.Text).ToString("0.00") : "0.00"),

                        new SqlParameter("@MmnExpInflow", txtMMNExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtMMNExpInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@MmnExpBacklog", txtMMNExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtMMNExpBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@MmnExpDispatch", txtMMNExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtMMNExpDispatch.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@MmnDomInflow", txtMMNDomInflow.Text.Length > 0 ? Convert.ToDecimal(txtMMNDomInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@MmnDomBacklog", txtMMNDomBacklog.Text.Length > 0 ? Convert.ToDecimal(txtMMNDomBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@MmnDomDispatch", txtMMNDomDispatch.Text.Length > 0 ? Convert.ToDecimal(txtMMNDomDispatch.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@InflowTot", hdnInflowTot.Value.Length > 0 ? Convert.ToDecimal(hdnInflowTot.Value).ToString("0.00") : "0.00"),
                        new SqlParameter("@BacklogTot", hdnBacklogTot.Value.Length > 0 ? Convert.ToDecimal(hdnBacklogTot.Value).ToString("0.00") : "0.00"),
                        new SqlParameter("@DispatchTot", hdnDispatchTot.Value.Length > 0 ? Convert.ToDecimal(hdnDispatchTot.Value).ToString("0.00") : "0.00"),
                        new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@PdkDomInflow", txtPDKDomInflow.Text.Length > 0 ? Convert.ToDecimal(txtPDKDomInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PdkDomBacklog", txtPDKDomBacklog.Text.Length > 0 ? Convert.ToDecimal(txtPDKDomBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PdkDomDispatch", txtPDKDomDispatch.Text.Length > 0 ? Convert.ToDecimal(txtPDKDomDispatch.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PdkExpInflow", txtPdkExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtPdkExpInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PdkExpBacklog", txtPDKExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtPDKExpBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PdkExpDispatch", txtPDKExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtPDKExpDispatch.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PneuExpInflow", txtPneuExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtPneuExpInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PneuExpBacklog", txtPneuExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtPneuExpBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PneuExpDispatch", txtPneuExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtPneuExpDispatch.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@RimsExpInflow", txtRimsExpInflow.Text.Length > 0 ? Convert.ToDecimal(txtRimsExpInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@RimsExpBacklog", txtRimsExpBacklog.Text.Length > 0 ? Convert.ToDecimal(txtRimsExpBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@RimsExpDispatch", txtRimsExpDispatch.Text.Length > 0 ? Convert.ToDecimal(txtRimsExpDispatch.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@CfDomInflow", txtCFDomInflow.Text.Length > 0 ? Convert.ToDecimal(txtCFDomInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@CfDomBacklog", txtCFDomBacklog.Text.Length > 0 ? Convert.ToDecimal(txtCFDomBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@CfDomDispatch", txtCFDomDispatch.Text.Length > 0 ? Convert.ToDecimal(txtCFDomDispatch.Text).ToString("0.00") : "0.00"),

                        new SqlParameter("@MmnMeInflow", txtMmnMeInflow.Text.Length > 0 ? Convert.ToDecimal(txtMmnMeInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@MmnMeBacklog", txtMmnMeBacklog.Text.Length > 0 ? Convert.ToDecimal(txtMmnMeBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@MmnMeDispatch", txtMmnMeDispatch.Text.Length > 0 ? Convert.ToDecimal(txtMmnMeDispatch.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PdkMeInflow", txtPdkMeInflow.Text.Length > 0 ? Convert.ToDecimal(txtPdkMeInflow.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PdkMeBacklog", txtPdkMeBacklog.Text.Length > 0 ? Convert.ToDecimal(txtPdkMeBacklog.Text).ToString("0.00") : "0.00"),
                        new SqlParameter("@PdkMeDispatch", txtPdkMeDispatch.Text.Length > 0 ? Convert.ToDecimal(txtPdkMeDispatch.Text).ToString("0.00") : "0.00")
                    };
                    daErrDB.ExecuteNonQuery_SP("sp_ins_OrderPosition_PreviousMonth", sp1);

                    SqlParameter[] sp2 = new SqlParameter[5];
                    sp2[0] = new SqlParameter("@InflowTotal", hdnInflowTot.Value.Length > 0 ? Convert.ToDecimal(hdnInflowTot.Value).ToString("0.00") : "0.00");
                    sp2[1] = new SqlParameter("@BacklogTotal", hdnBacklogTot.Value.Length > 0 ? Convert.ToDecimal(hdnBacklogTot.Value).ToString("0.00") : "0.00");
                    sp2[2] = new SqlParameter("@DispatchTotal", hdnDispatchTot.Value.Length > 0 ? Convert.ToDecimal(hdnDispatchTot.Value).ToString("0.00") : "0.00");
                    sp2[3] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                    sp2[4] = new SqlParameter("@AsOnDate", lastDayLastMonth.ToString("yyyy-MM-dd"));

                    daErrDB.ExecuteNonQuery_SP("sp_edit_OrderPositionTotal_Previous", sp2);

                    Response.Redirect("orderpositionentry.aspx", false);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JVerify3", "alert('RECORD NOT SAVED');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSaveOpenStock_Click(object sender, EventArgs e)
        {
            try
            {
                decimal decLanka = txtOpenStockLanka.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockLanka.Text) : 0;
                decimal decStarco = txtOpenStockStarco.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockStarco.Text) : 0;
                decimal decStarcoJobWork = txtOpenStockStarcoJobWork.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockStarcoJobWork.Text) : 0;
                decimal decMMN = txtOpenStockMMN.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockMMN.Text) : 0;
                decimal decDomestic = txtOpenStockMMNDomestic.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockMMNDomestic.Text) : 0;
                decimal decPDKDom = txtOpenStockPDKDomestic.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockPDKDomestic.Text) : 0;
                decimal decPDKExp = txtOpenStockPDKExport.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockPDKExport.Text) : 0;
                decimal decPneuExp = txtOpenStockPneuExport.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockPneuExport.Text) : 0;
                decimal decRimsExp = txtOpenStockRimsExport.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockRimsExport.Text) : 0;
                decimal decCFDom = txtOpenStockCFDom.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockCFDom.Text) : 0;
                decimal decMmnMe = txtOpenStockMmnMe.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockMmnMe.Text) : 0;
                decimal decPdkMe = txtOpenStockPdkMe.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockPdkMe.Text) : 0;

                SqlParameter[] sp1 = new SqlParameter[]{
                    new SqlParameter("@AsOnDate", System.DateTime.Now),
                    new SqlParameter("@LankaExpInflow", "0.00"),
                    new SqlParameter("@LankaExpBacklog", txtOpenStockLanka.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockLanka.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@LankaExpDispatch", "0.00"),
                    new SqlParameter("@StarcoExpInflow", "0.00"),
                    new SqlParameter("@StarcoExpBacklog", txtOpenStockStarco.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockStarco.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@StarcoExpDispatch", "0.00"),
                    new SqlParameter("@StarcoJobWorkInflow", "0.00"),
                    new SqlParameter("@StarcoJobWorkBacklog", txtOpenStockStarcoJobWork.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockStarcoJobWork.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@StarcoJobWorkDispatch", "0.00"),
                    new SqlParameter("@MmnExpInflow", "0.00"),
                    new SqlParameter("@MmnExpBacklog", txtOpenStockMMN.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockMMN.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnExpDispatch", "0.00"),
                    new SqlParameter("@MmnDomInflow", "0.00"),
                    new SqlParameter("@MmnDomBacklog", txtOpenStockMMNDomestic.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockMMNDomestic.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnDomDispatch", "0.00"),
                    new SqlParameter("@InflowTot", "0.00"),
                    new SqlParameter("@BacklogTot", (decLanka + decStarco + decStarcoJobWork + decMMN + decDomestic + decPDKDom + decPDKExp + decPneuExp + decRimsExp + decCFDom + decMmnMe + decPdkMe).ToString("0.00")),
                    new SqlParameter("@DispatchTot", "0.00"),
                    new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@PdkDomInflow", "0.00"),
                    new SqlParameter("@PdkDomBacklog", txtOpenStockPDKDomestic.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockPDKDomestic.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkDomDispatch", "0.00"),
                    new SqlParameter("@PdkExpInflow", "0.00"),
                    new SqlParameter("@PdkExpBacklog", txtOpenStockPDKExport.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockPDKExport.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkExpDispatch", "0.00"),
                    new SqlParameter("@PneuExpInflow", "0.00"),
                    new SqlParameter("@PneuExpBacklog", txtOpenStockPneuExport.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockPneuExport.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PneuExpDispatch", "0.00"),
                    new SqlParameter("@RimsExpInflow", "0.00"),
                    new SqlParameter("@RimsExpBacklog", txtOpenStockRimsExport.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockRimsExport.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@RimsExpDispatch", "0.00"),
                    new SqlParameter("@CfDomInflow", "0.00"),
                    new SqlParameter("@CfDomBacklog", txtOpenStockCFDom.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockCFDom.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@CfDomDispatch", "0.00"),

                    new SqlParameter("@MmnMeInflow", "0.00"),
                    new SqlParameter("@MmnMeBacklog", txtOpenStockMmnMe.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockMmnMe.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@MmnMeDispatch", "0.00"),
                    new SqlParameter("@PdkMeInflow", "0.00"),
                    new SqlParameter("@PdkMeBacklog", txtOpenStockPdkMe.Text.Length > 0 ? Convert.ToDecimal(txtOpenStockPdkMe.Text).ToString("0.00") : "0.00"),
                    new SqlParameter("@PdkMeDispatch", "0.00")
                };
                daErrDB.ExecuteNonQuery_SP("sp_ins_OrderPosition", sp1);

                Response.Redirect("orderpositionentry.aspx", false);
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

                if (dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "")
                {
                    rptOrderPositionMonth.DataSource = dt;
                    rptOrderPositionMonth.DataBind();

                    hdnLankaExpBacklog.Value = dt.Rows[0]["LankaExpBacklog"].ToString();
                    hdnStarcoExpBacklog.Value = dt.Rows[0]["StarcoExpBacklog"].ToString();
                    hdnStarcoJobWorkBacklog.Value = dt.Rows[0]["StarcoJobWorkBacklog"].ToString();
                    hdnMMNExpBacklog.Value = dt.Rows[0]["MmnExpBacklog"].ToString();
                    hdnMMNDomBacklog.Value = dt.Rows[0]["MmnDomBacklog"].ToString();
                    hdnPDKDomBacklog.Value = dt.Rows[0]["PdkDomBacklog"].ToString();
                    hdnPDKExpBacklog.Value = dt.Rows[0]["PdkExpBacklog"].ToString();
                    hdnPneuExpBacklog.Value = dt.Rows[0]["PneuExpBacklog"].ToString();
                    hdnRimsExpBacklog.Value = dt.Rows[0]["RimsExpBacklog"].ToString();
                    hdnCFDomBacklog.Value = dt.Rows[0]["CfDomBacklog"].ToString();
                    hdnMmnMeBacklog.Value = dt.Rows[0]["MmnMeBacklog"].ToString();
                    hdnPdkMeBacklog.Value = dt.Rows[0]["PdkMeBacklog"].ToString();

                    txtLankaExpBacklog.Text = dt.Rows[0]["LankaExpBacklog"].ToString();
                    txtStarcoExpBacklog.Text = dt.Rows[0]["StarcoExpBacklog"].ToString();
                    txtStarcoJobWorkBacklog.Text = dt.Rows[0]["StarcoJobWorkBacklog"].ToString();
                    txtMMNExpBacklog.Text = dt.Rows[0]["MmnExpBacklog"].ToString();
                    txtMMNDomBacklog.Text = dt.Rows[0]["MmnDomBacklog"].ToString();
                    txtPDKDomBacklog.Text = dt.Rows[0]["PdkDomBacklog"].ToString();
                    txtPDKExpBacklog.Text = dt.Rows[0]["PdkExpBacklog"].ToString();
                    txtPneuExpBacklog.Text = dt.Rows[0]["PneuExpBacklog"].ToString();
                    txtRimsExpBacklog.Text = dt.Rows[0]["RimsExpBacklog"].ToString();
                    txtCFDomBacklog.Text = dt.Rows[0]["CfDomBacklog"].ToString();

                    txtMmnMeBacklog.Text = dt.Rows[0]["MmnMeBacklog"].ToString();
                    txtPdkMeBacklog.Text = dt.Rows[0]["PdkMeBacklog"].ToString();

                    string strMonthName = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
                    if (dt.Rows[0]["AsMonth"].ToString().ToLower() != strMonthName.ToLower())
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Jscriptopen", "ctrlBacklogShow();", true);
                        DateTime now = DateTime.Now;
                        DateTime lastDayLastMonth = new DateTime(now.Year, now.Month, 1);
                        lastDayLastMonth = lastDayLastMonth.AddDays(-1);
                        btnPreviousMonthSave.Text = "SAVE RECORD TO " + lastDayLastMonth.ToShortDateString();
                        lblOR.Text = "  OR  ";
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("orderpositionentry.aspx", false);
        }
    }
}