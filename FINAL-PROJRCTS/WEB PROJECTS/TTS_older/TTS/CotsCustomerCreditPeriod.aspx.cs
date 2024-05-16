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
    public partial class CotsCustomerCreditPeriod : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_paymentcontrol"].ToString() == "True")
                            BindCustomerPeriodDetails();
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
        private void BindCustomerPeriodDetails()
        {
            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_sel_CreditPeriod_Usermaster", DataAccess.Return_Type.DataTable);
            if (dt.Rows.Count > 0)
            {
                gvCustPeriodDetails.DataSource = dt;
                gvCustPeriodDetails.DataBind();
            }
        }

        protected void gvCustPeriodDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCustPeriodDetails.EditIndex = e.NewEditIndex;
            BindCustomerPeriodDetails();
            RadioButtonList rdoPaymentMode = (RadioButtonList)gvCustPeriodDetails.Rows[e.NewEditIndex].FindControl("rdoPaymentMode");
            HiddenField hdnCrediteNote = (HiddenField)gvCustPeriodDetails.Rows[e.NewEditIndex].FindControl("hdnCrediteNote");
            if (!Convert.ToBoolean(hdnCrediteNote.Value))
                rdoPaymentMode.SelectedIndex = 0;
            else
                rdoPaymentMode.SelectedIndex = 1;
            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow11", "ShowPayDaysCtrl();", true);
        }

        protected void gvCustPeriodDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow Row = gvCustPeriodDetails.Rows[e.RowIndex];
                HiddenField hdnCustCode = (HiddenField)Row.FindControl("hdnCustCode");
                TextBox txtPaymentDays = (TextBox)Row.FindControl("txtpaymentDays");
                RadioButtonList rdoPaymentMode = (RadioButtonList)Row.FindControl("rdoPaymentMode");
                TextBox txtSalesLimit = (TextBox)Row.FindControl("txtSalesLimit");
                if (rdoPaymentMode.SelectedValue.ToString() == "Probation")
                    txtPaymentDays.Text = "0";
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@CustCode",hdnCustCode.Value),
                    new SqlParameter("@PaymentDays",txtPaymentDays.Text),
                    new SqlParameter("@appusername",Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@CreditNote", rdoPaymentMode.SelectedItem.Text == "Establish" ? false : true),
                    new SqlParameter("@SalesLimit", Convert.ToDecimal(txtSalesLimit.Text))
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_upt_CreditPeriod_UserMaster", sp);
                if (resp > 0)
                {
                    gvCustPeriodDetails.EditIndex = -1;
                    BindCustomerPeriodDetails();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }

        protected void gvCustPeriodDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            e.Cancel = true;
            gvCustPeriodDetails.EditIndex = -1;
            BindCustomerPeriodDetails();
        }
    }
}