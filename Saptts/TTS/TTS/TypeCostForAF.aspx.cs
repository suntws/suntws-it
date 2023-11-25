using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace TTS
{
    public partial class TypeCostForAF : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (Request.Cookies["TTSUser"].Value.ToLower() == "admin" ||
                            Request.Cookies["TTSUser"].Value.ToLower() != "anand" || Request.Cookies["TTSUser"].Value.ToLower() == "bhargav_std" ||
                            Request.Cookies["TTSUser"].Value.ToLower() == "arun"))
                        {
                            //SqlParameter[] sp = new SqlParameter[] { new SqlParameter("", Request.Cookies["TTSUser"].Value) };
                            //string strPlant = Request.Cookies["TTSUser"].Value.ToLower() == "balamurugan" ? "MMN" : "PDK";
                            string strPlant = Request.Cookies["TTSUser"].Value.ToLower() == "bhargav_std" ? "MMN" : "PDK";
                            ddlFinancePlant.Items.Insert(0, new System.Web.UI.WebControls.ListItem("CHOOSE", "CHOOSE"));
                            ddlFinancePlant.Items.Insert(1, new System.Web.UI.WebControls.ListItem(strPlant, strPlant == "SLTL" ? "LANKA" : strPlant));

                            if (ddlFinancePlant.Items.Count == 2)
                            {
                                ddlFinancePlant.SelectedIndex = 1;
                                ddlFinancePlant_IndexChange(null, null);
                            }
                        }
                        else
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                    }
                }
                else
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlFinancePlant_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@FinancePlant", ddlFinancePlant.SelectedItem.Value), 
                    new SqlParameter("@CreatedBy", Request.Cookies["TTSUser"].Value) 
                };
                DataTable dtVal = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FinanceTypeCost", sp, DataAccess.Return_Type.DataTable);
                if (dtVal.Rows.Count > 0)
                {
                    gv_TypeCose.DataSource = dtVal;
                    gv_TypeCose.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveTypeCost_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtData = new DataTable();
                dtData.Columns.Add(new DataColumn("TyreType", typeof(System.String)));
                dtData.Columns.Add(new DataColumn("TypeCost", typeof(System.Decimal)));
                foreach (GridViewRow row in gv_TypeCose.Rows)
                {
                    dtData.Rows.Add(((Label)row.FindControl("lblCostTyreType")).Text, Convert.ToDecimal(((TextBox)row.FindControl("txtTypeCostValue")).Text).ToString("0.00"));
                }

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@FinanceTypeCostVal_Dt", dtData),
                    new SqlParameter("@FinancePlant", ddlFinancePlant.SelectedItem.Value), 
                    new SqlParameter("@CostMethod", "INR"),
                    new SqlParameter("@CreatedBy", Request.Cookies["TTSUser"].Value) 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_upd_FinanceTypeCost", sp);
                if (resp > 0)
                {
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl, false);
        }
    }
}