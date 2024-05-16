using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
namespace TTS
{
    public partial class StockReturnReview : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_documents"].ToString() == "True" || dtUser.Rows[0]["ot_qcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True" ||
                            dtUser.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True" ||
                            dtUser.Rows[0]["dom_paymentconfirm"].ToString() == "True" || dtUser.Rows[0]["dom_invoice"].ToString() == "True"))
                        {
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@QString", Utilities.Decrypt(Request["fid"].ToString()).ToUpper()) };
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedStockitemlist_plant", sp, DataAccess.Return_Type.DataTable);
                            Utilities.ddl_Binding(ddlplant, dt, "plant", "plant", "ALL");
                            if (dt.Rows.Count > 0)
                            {
                                ddl_bindyear();
                                ddl_bindMonth();
                                Bind_DispatchStockgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
                            }
                            else
                            {
                                lblErrMsgcontent.Text = "No Records";
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('displaycontent').style.display='none';", true);
                            }
                        }
                        else
                        {
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
        private void ddl_bindyear()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@Plant", ddlplant.SelectedItem.Text), 
                    new SqlParameter("@QString", Utilities.Decrypt(Request["fid"].ToString()).ToUpper()) 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedStocklist_year", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlYear, dt, "CreatedOn", "CreatedOn", "");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ddl_bindMonth()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@Plant", ddlplant.SelectedItem.Text), 
                    new SqlParameter("@year", Convert.ToInt32(ddlYear.SelectedItem.Text)),
                    new SqlParameter("@QString", Utilities.Decrypt(Request["fid"].ToString()).ToUpper())
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedstocklist_Month", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlMonth, dt, "CreatedOn", "Createdid", "");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindMonth();
            Bind_DispatchStockgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_DispatchStockgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindyear();
            ddl_bindMonth();
            Bind_DispatchStockgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        private void Bind_DispatchStockgvlist(string plant, string year, string month)
        {
            try
            {
                gv_Returnitemslist.DataSource = null;
                gv_Returnitemslist.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@Plant", plant), 
                    new SqlParameter("@year", Convert.ToInt32(year)), 
                    new SqlParameter("@month", Convert.ToInt32(month)),
                    new SqlParameter("@QString", Utilities.Decrypt(Request["fid"].ToString()).ToUpper())
                };
                DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedStockItemlistorders", sp1, DataAccess.Return_Type.DataTable);
                if (dtorderlist.Rows.Count > 0)
                {
                    gv_Returnitemslist.DataSource = dtorderlist;
                    gv_Returnitemslist.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((Button)sender).NamingContainer as GridViewRow;

                lblCustName.Text = row.Cells[0].Text;
                lblStausOrderRefNo.Text = row.Cells[1].Text;

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@ReturnID", ((HiddenField)row.FindControl("hdnReturnID")).Value) };
                DataSet dsList = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_StockReturn_forQc_ItemList", sp, DataAccess.Return_Type.DataSet);
                if (dsList.Tables[0].Rows.Count > 0)
                {
                    GV_ReturnItem.DataSource = dsList.Tables[0];
                    GV_ReturnItem.DataBind();

                    lblRtnRemarks.Text = dsList.Tables[1].Rows[0]["Remarks"].ToString();
                    lblRtnReason.Text = dsList.Tables[1].Rows[0]["ReturnForReason"].ToString();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkPdf_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((LinkButton)sender).NamingContainer as GridViewRow;
                HiddenField hdnID = (HiddenField)row.FindControl("hdnReturnID");
                HiddenField hdnCode = (HiddenField)row.FindControl("hdnCustCode");

                string serverUrl = Server.MapPath("~/DispatchReturn/" + hdnCode.Value).Replace("TTS", "pdfs");
                string path = serverUrl + "/" + hdnID.Value + ".pdf";
                string lnkTxt = hdnID.Value + ".pdf";

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}