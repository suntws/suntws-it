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
    public partial class StockReturnFg : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["ot_qcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True"))
                        {
                            lblPageHead.Text = "DISPATCH RETURN STOCK MOVE TO GODOWN - " + Utilities.Decrypt(Request["pid"].ToString()).ToUpper();
                            SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                                new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString()).ToUpper()) 
                            };
                            DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_StockReturn_ForFG", sp, DataAccess.Return_Type.DataTable);
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
        protected void btnFGReport_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedrow = ((Button)sender).NamingContainer as GridViewRow;
                hdnReturnID.Value = ((HiddenField)clickedrow.FindControl("hdnReturnID")).Value;
                lblCustName.Text = clickedrow.Cells[0].Text;
                lblStausOrderRefNo.Text = clickedrow.Cells[1].Text;

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@ReturnID", hdnReturnID.Value) };
                DataSet dslist = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_StockReturn_forQc_ItemList", sp, DataAccess.Return_Type.DataSet);
                if (dslist.Tables[0].Rows.Count > 0)
                {
                    GV_QC.DataSource = dslist.Tables[0];
                    GV_QC.DataBind();

                    lblRtnRemarks.Text = dslist.Tables[1].Rows[0]["Remarks"].ToString();
                    lblRtnReason.Text = dslist.Tables[1].Rows[0]["ReturnForReason"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow", "showdiv('gvhide');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnFGSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@ReturnBy", Request.Cookies["TTSUser"].Value), 
                    new SqlParameter("@ReturnRemarks", txt_Remark.Text.Replace("\r\n","~")), 
                    new SqlParameter("@ReturnID", hdnReturnID.Value),
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper())
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_upt_WareHouseData_StockReturn", sp1);
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnFGCancel_Click(object sender, EventArgs e)
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