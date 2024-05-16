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
    public partial class expordercancel : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_revise"].ToString() == "True")
                        {
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_order_for_cancel", DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvCancelOrderList.DataSource = dtorderlist;
                                gvCancelOrderList.DataBind();
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                    divCancelBtn.Visible = false;
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
        protected void lnkOrderCancel_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblCustName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblStausOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnOrderCustCode")).Value;
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;
                divCancelBtn.Visible = true;
                btnSaveOrderCancel.Focus();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveOrderCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[]{
                    new SqlParameter("@O_ID", hdnOID.Value),
                    new SqlParameter("@statusid", "6"),
                    new SqlParameter("@feedback", txtCotsOrderCancelFeedBack.Text.Replace("\r\n", "~")),
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (System.Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}