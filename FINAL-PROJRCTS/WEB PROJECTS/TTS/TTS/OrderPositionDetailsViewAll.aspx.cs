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
    public partial class OrderPositionDetailsViewAll : System.Web.UI.Page
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
                            BindMonthRecords();
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
        private void BindMonthRecords()
        {
            try
            {
                //for Month Record
                DataTable dtMonth = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPositionEntry_Month", DataAccess.Return_Type.DataTable);
                if (dtMonth.Rows.Count > 0 && dtMonth.Rows[0]["AsMonth"].ToString() != "")
                {
                    rptMonthRecord.DataSource = dtMonth;
                    rptMonthRecord.DataBind();
                }

                //for Last Updated Record
                DataTable dtLastUpdated = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_OrderPositionEntry_LastUpdated", DataAccess.Return_Type.DataTable);
                if (dtLastUpdated.Rows.Count > 0 && dtLastUpdated.Rows[0]["AsonDate"].ToString() != "")
                {
                    rptLastUpdatedRecord.DataSource = dtLastUpdated;
                    rptLastUpdatedRecord.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ORDER_POSITION", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}