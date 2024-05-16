using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TTS
{
    public partial class CustomerWiseSalesAnalysis : System.Web.UI.Page
    {
        DataAccess daCots = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
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
        protected void ddlCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlYear.Items.Clear();
                ddlType.Items.Clear();
                if (ddlCategory.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@TypeOfSales", Utilities.Decrypt(Request["sale"].ToString())), 
                        new SqlParameter("@Category", ddlCategory.Text) 
                    };
                    DataTable dtCategories = (DataTable)daCots.ExecuteReader_SP("SP_LST_ExportSalesData", sp, DataAccess.Return_Type.DataTable);
                    Session["dtTest"] = dtCategories;
                    DataView dtView = new DataView(dtCategories);
                    dtView.Sort = "DispatchedYear";
                    DataTable distinctYear = dtView.ToTable(true, "DispatchedYear");
                    Utilities.ddl_Binding(ddlYear, distinctYear, "DispatchedYear", "DispatchedYear", "CHOOSE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlYear_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlType.DataSource = null;
                chkCustomerList.DataSource = null;
                chkCustomerList.DataBind();
                DataTable dt = (DataTable)Session["dtTest"];
                DataView dtView1 = new DataView(dt, "category='" + ddlCategory.SelectedItem.Text + "' and DispatchedYear='" + ddlYear.SelectedItem.Text + "'",
                    "DispatchedYear DESC", DataViewRowState.CurrentRows);
                dtView1.Sort = "Grade";
                DataTable distinctGrade = dtView1.ToTable(true, "Grade");
                Utilities.ddl_Binding(ddlType, distinctGrade, "Grade", "Grade", "CHOOSE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                chkCustomerList.DataSource = null;
                chkCustomerList.DataBind();
                DataTable dt = (DataTable)Session["dtTest"];
                DataView dtView1 = new DataView(dt, "category='" + ddlCategory.SelectedItem.Text + "' and DispatchedYear='" + ddlYear.SelectedItem.Text +
                    "'and Grade='" + ddlType.SelectedItem.Text + "'", "DispatchedYear DESC", DataViewRowState.CurrentRows);
                DataTable tb1 = dtView1.ToTable();
                List<string> lstCustomerName = new List<string>();
                lstCustomerName = tb1.AsEnumerable().Where(n => n.Field<string>("CustomerName").ToString() != "").Select(n => n.Field<string>("CustomerName")).ToList<string>();
                chkCustomerList.DataSource = lstCustomerName;
                chkCustomerList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}