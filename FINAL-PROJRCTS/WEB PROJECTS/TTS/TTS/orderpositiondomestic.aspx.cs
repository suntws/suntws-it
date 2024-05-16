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

namespace TTS
{
    public partial class orderpositiondomestic : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["orderposition_domestic"].ToString() == "True")
                        {
                            Bind_TodayValues();
                            Bind_MonthValues();
                            Bind_12MonthValues();
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
                DataTable dt = new DataTable();
                dt = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_Domestic_OrderPosition_Today", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    rptDomesticToday.DataSource = dt;
                    rptDomesticToday.DataBind();
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
                DataTable dt = new DataTable();
                dt = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_Domestic_OrderPosition_Month", DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    rptDomesticMonth.DataSource = dt;
                    rptDomesticMonth.DataBind();
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
                dtCur = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_Domestic_OrderPosition_CurYear", DataAccess.Return_Type.DataTable);

                if (dtCur.Rows.Count > 0)
                {
                    DataTable dtPrev = new DataTable();
                    dtPrev = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_Domestic_OrderPosition_PrevYear", DataAccess.Return_Type.DataTable);

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
                        gv_DomesticLast12Months.DataSource = dtMerge;
                        gv_DomesticLast12Months.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}