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
    public partial class quotediscount : System.Web.UI.Page
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
                        if (Request.Cookies["TTSUser"].Value.ToLower() == "admin" || Request.Cookies["TTSUser"].Value.ToLower() == "somu" ||
                            Request.Cookies["TTSUser"].Value.ToLower() != "anand")
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void rdbDiscGrade_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (rdbDiscGrade.SelectedItem.Text != "")
                {
                    Bind_GvList();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_GvList()
        {
            SqlParameter[] sp1 = new SqlParameter[1];
            sp1[0] = new SqlParameter("@Grade", rdbDiscGrade.SelectedItem.Text);

            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_QuoteDiscount", sp1, DataAccess.Return_Type.DataTable);

            gvCustTypeDisc.DataSource = dt;
            gvCustTypeDisc.DataBind();
        }

        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvCustTypeDisc.EditIndex = e.NewEditIndex;
                Bind_GvList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }

        protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvCustTypeDisc.Rows[e.RowIndex];
                TextBox txtQLeadPer = row.FindControl("txtQLeadPer") as TextBox;
                TextBox txtQDSupervisorPer = row.FindControl("txtQDSupervisorPer") as TextBox;
                TextBox txtQDManagerPer = row.FindControl("txtQDManagerPer") as TextBox;
                Label lblQDCustType = row.FindControl("lblQDCustType") as Label;

                if (txtQLeadPer.Text != "" || txtQDSupervisorPer.Text != "" || txtQDManagerPer.Text != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[6];

                    sp1[0] = new SqlParameter("@Grade", rdbDiscGrade.SelectedItem.Text);
                    sp1[1] = new SqlParameter("@CustType", lblQDCustType.Text);
                    sp1[2] = new SqlParameter("@LeadPer", txtQLeadPer.Text != "" ? Convert.ToDecimal(txtQLeadPer.Text) : 0);
                    sp1[3] = new SqlParameter("@SupervisorPer", txtQDSupervisorPer.Text != "" ? Convert.ToDecimal(txtQDSupervisorPer.Text) : 0);
                    sp1[4] = new SqlParameter("@ManagerPer", txtQDManagerPer.Text != "" ? Convert.ToDecimal(txtQDManagerPer.Text) : 0);
                    sp1[5] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);

                    int resp = daCOTS.ExecuteNonQuery_SP("sp_edit_QuoteDiscount", sp1);
                }
                gvCustTypeDisc.EditIndex = -1;
                Bind_GvList();
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
                gvCustTypeDisc.EditIndex = -1;
                Bind_GvList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

    }
}