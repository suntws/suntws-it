using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class emailalert : System.Web.UI.Page
    {
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        Bind_MailAlertGV();
                    }
                }
                else
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_MailAlertGV()
        {
            try
            {
                DataTable dtDayCount = (DataTable)daPORT.ExecuteReader_SP("sp_sel_maildayscount", DataAccess.Return_Type.DataTable);
                ViewState["dtDayCount"] = dtDayCount;

                DataTable dt = (DataTable)daPORT.ExecuteReader_SP("sp_sel_mailalertdays", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gv_mailalert.DataSource = dt;
                    gv_mailalert.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public string Bind_DayCount(string strFocus)
        {
            string returnVal = string.Empty;
            try
            {
                DataTable dtDayCount = ViewState["dtDayCount"] as DataTable;
                if (dtDayCount != null && dtDayCount.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDayCount.Select("focus='" + strFocus + "'"))
                    {
                        returnVal = row["dayCount"].ToString();
                    }
                    if (returnVal == "")
                        returnVal = "0";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnVal;
        }

        public string Bind_MailType(string strFocus)
        {
            string returnVal = string.Empty;
            try
            {
                DataTable dtDayCount = ViewState["dtDayCount"] as DataTable;
                if (dtDayCount != null && dtDayCount.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDayCount.Select("focus='" + strFocus + "'"))
                    {
                        returnVal = row["mailtype"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnVal;
        }

        public string Bind_DayNames(string strDays, string strFocus)
        {
            string returnVal = string.Empty;
            try
            {
                string strMailType = string.Empty;
                DataTable dtDayCount = ViewState["dtDayCount"] as DataTable;
                if (dtDayCount != null && dtDayCount.Rows.Count > 0)
                {
                    foreach (DataRow row in dtDayCount.Select("focus='" + strFocus + "'"))
                    {
                        strMailType = row["mailtype"].ToString();
                    }
                }

                if (strMailType == "Day Wise")
                {
                    string[] objSplit = strDays.Split(',');
                    if (objSplit != null && objSplit.Length > 1)
                    {
                        var dict = new Dictionary<int, string>();
                        foreach (string row in objSplit)
                        {
                            switch (row as string)
                            {
                                case "Monday":
                                    dict.Add(1, row);
                                    break;
                                case "Tuesday":
                                    dict.Add(2, row);
                                    break;
                                case "Wednesday":
                                    dict.Add(3, row);
                                    break;
                                case "Thursday":
                                    dict.Add(4, row);
                                    break;
                                case "Friday":
                                    dict.Add(5, row);
                                    break;
                                case "Saturday":
                                    dict.Add(6, row);
                                    break;
                            }
                        }

                        if (dict.Count > 0)
                        {
                            foreach (var item in dict.OrderBy(c => c.Key))
                            {
                                if (returnVal == "")
                                    returnVal = item.Value;
                                else
                                    returnVal += "," + item.Value;
                            }
                        }
                    }
                    else if (objSplit != null)
                        returnVal = strDays;
                }
                else if (strMailType == "Month Wise")
                    returnVal = strDays;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnVal;
        }

        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gv_mailalert.EditIndex = e.NewEditIndex;
                Bind_MailAlertGV();
                ScriptManager.RegisterStartupScript(Page, GetType(), "EditScript", "editEmailAlertsAfter();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }

        protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gv_mailalert.Rows[e.RowIndex];
                Label lblFocus = row.FindControl("lblFocus") as Label;
                HiddenField hdnMailType = row.FindControl("hdnMailType") as HiddenField;
                HiddenField hdnAddDayList = new HiddenField();
                if (hdnMailType.Value == "Day Wise")
                    hdnAddDayList = row.FindControl("hdnAddDayList") as HiddenField;
                else if (hdnMailType.Value == "Month Wise")
                    hdnAddDayList = row.FindControl("hdnAddMonthList") as HiddenField;

                if (hdnAddDayList.Value != "")
                {
                    SqlParameter[] spDaysEdit = new SqlParameter[4];

                    spDaysEdit[0] = new SqlParameter("@focus", lblFocus.Text);
                    spDaysEdit[1] = new SqlParameter("@maildays", hdnAddDayList.Value);
                    spDaysEdit[2] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    spDaysEdit[3] = new SqlParameter("@mailtype", hdnMailType.Value);

                    int resp = daPORT.ExecuteNonQuery_SP("sp_ins_focusmailalert", spDaysEdit);
                }
                gv_mailalert.EditIndex = -1;
                Bind_MailAlertGV();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gv_mailalert.EditIndex = -1;
                Bind_MailAlertGV();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

    }
}