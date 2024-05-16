using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;

namespace TTS
{
    public partial class StockReturnQc : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True" ||
                            dtUser.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True"))
                        {
                            lblPageHead.Text = "INSPECTION FOR DISPATCH RETURN STOCK - " + Utilities.Decrypt(Request["pid"].ToString()).ToUpper();
                            SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                                new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString()).ToUpper()) 
                            };
                            DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_StockReturn_ForQC", sp, DataAccess.Return_Type.DataTable);
                            if (dtList.Rows.Count > 0)
                            {
                                gv_Returnitems.DataSource = dtList;
                                gv_Returnitems.DataBind();
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnQCReport_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow ClickedRow = ((Button)sender).NamingContainer as GridViewRow;
                lblCustName.Text = ClickedRow.Cells[0].Text;
                lblStausOrderRefNo.Text = ClickedRow.Cells[1].Text;
                hdnReturnID.Value = ((HiddenField)ClickedRow.FindControl("hdnReturnID")).Value;
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@ReturnID", hdnReturnID.Value) };
                DataSet dsList = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_StockReturn_forQc_ItemList", sp, DataAccess.Return_Type.DataSet);
                if (dsList.Tables[0].Rows.Count > 0)
                {
                    GV_QC.DataSource = dsList.Tables[0];
                    GV_QC.DataBind();

                    foreach (GridViewRow gv in GV_QC.Rows)
                    {
                        DropDownList ddl = (DropDownList)(gv.FindControl("ddl_Grade"));
                        ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByText(((HiddenField)(gv.FindControl("hdnGrade"))).Value));
                    }

                    lblRtnRemarks.Text = dsList.Tables[1].Rows[0]["Remarks"].ToString();
                    lblRtnReason.Text = dsList.Tables[1].Rows[0]["ReturnForReason"].ToString();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow", "showReturnQc();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnQCSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow gv in GV_QC.Rows)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@ReturnID", hdnReturnID.Value), 
                        new SqlParameter("@Stencilno", Convert.ToString(gv.Cells[1].Text)), 
                        new SqlParameter("@QCGrade", Convert.ToString((gv.FindControl("ddl_Grade") as DropDownList).Text)), 
                        new SqlParameter("@QCReason", Convert.ToString((gv.FindControl("txtReason") as TextBox).Text)), 
                        new SqlParameter("@QCCondition", Convert.ToString((gv.FindControl("txtConditionofthetyre") as TextBox).Text)), 
                        new SqlParameter("@QCBy", Request.Cookies["TTSUser"].Value) 
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_ins_RetrunItem_Inspection", sp);
                }
                SqlParameter[] spU = new SqlParameter[] { new SqlParameter("@ReturnID", hdnReturnID.Value) };
                int result = daCOTS.ExecuteNonQuery_SP("sp_upd_DispatchReturn_Inpsection", spU);
                if (result > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnQCCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}



