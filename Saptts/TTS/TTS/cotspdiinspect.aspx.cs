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

namespace TTS
{
    public partial class cotspdiinspect : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True"
                            || dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "")
                            {
                                lblPageTitle.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ") + "PDI INSPECTION";
                                SqlParameter[] sp1 = new SqlParameter[] { 
                                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                                    new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())) 
                                };
                                DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dom_pdi_inspection_orders", sp1, DataAccess.Return_Type.DataTable);
                                if (dtorderlist.Rows.Count > 0)
                                {
                                    gvDomPdiList.DataSource = dtorderlist;
                                    gvDomPdiList.DataBind();
                                }

                                SqlParameter[] spRim = new SqlParameter[] { 
                                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                                    new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())) 
                                };
                                DataTable dtRimOrder = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_SPlitRim_Pdi_Orders", spRim, DataAccess.Return_Type.DataTable);
                                if (dtRimOrder.Rows.Count > 0)
                                {
                                    lblSplitRims.Text = "SPLIT RIM ORDER'S";
                                    gvRimsPDI.DataSource = dtRimOrder;
                                    gvRimsPDI.DataBind();
                                }
                                btnRefresh.Style.Add("display", "none");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL IS WRONG";
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
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkPdiInspect_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                HiddenField hdnCustCode = (HiddenField)clickedRow.FindControl("hdnCustCode");
                Label lblOrderRefNo = (Label)clickedRow.FindControl("lblOrderRefNo");
                HiddenField hdnOrderID = (HiddenField)clickedRow.FindControl("hdnOrderID");
                HiddenField hdnOrderStatus = (HiddenField)clickedRow.FindControl("hdnOrderStatus");

                if (hdnOrderStatus.Value == "35")
                    Response.Redirect("expppc_stencilapproval.aspx?pid=" + Request["pid"].ToString() + "&fid=" + Request["fid"].ToString() + "&oid=" +
                    Utilities.Encrypt(hdnOrderID.Value), false);
                else
                {
                    if (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() != "SLTL" && Utilities.Decrypt(Request["pid"].ToString()).ToUpper() != "SITL")
                        Response.Redirect("expscanpdi_1.aspx?pid=" + Request["pid"].ToString() + "&fid=" + Request["fid"].ToString() + "&ccode=" +
                            Utilities.Encrypt(hdnCustCode.Value) + "&oid=" + Utilities.Encrypt(lblOrderRefNo.Text) + "&mtype=" + Utilities.Encrypt("new") +
                            "&id=" + Utilities.Encrypt(hdnOrderID.Value), false);
                    else
                        Response.Redirect("expscanlankapdi.aspx?pid=" + Request["pid"].ToString() + "&fid=" + Request["fid"].ToString() + "&ccode=" +
                            Utilities.Encrypt(hdnCustCode.Value) + "&oid=" + Utilities.Encrypt(lblOrderRefNo.Text) + "&mtype=" + Utilities.Encrypt("new") +
                            "&id=" + Utilities.Encrypt(hdnOrderID.Value), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkRimPdiInspect_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                HiddenField hdnCustCode = (HiddenField)clickedRow.FindControl("hdnCustCode");
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@custcode", hdnCustCode.Value), 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@workorderno", clickedRow.Cells[1].Text),
                    new SqlParameter("@RimPlant", Utilities.Decrypt(Request["pid"].ToString())),
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                };
                string strMsg = (string)daCOTS.ExecuteScalar_SP("sp_chk_upd_splitrim_order", sp);
                lblStatusMsg.Text = strMsg.ToString();
                lblStatusMsg.Style.Add("color", strMsg.Contains("INSPECTION COMPLETED.") ? "#1aad10" : "#ff0000");
                btnRefresh.Text = strMsg.Contains("INSPECTION COMPLETED.") ? "MOVE TO NEXT PROCESS" : "MOVE TO RIM INSPECTION";
                btnRefresh.CssClass = "btnactive";
                btnRefresh.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (btnRefresh.Text == "MOVE TO NEXT PROCESS")
            {
                if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                    DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), "7", "RIM INSPECTION COMPLETED. ORDER MOVED TO INVOICE PREPARE",
                        Request.Cookies["TTSUser"].Value);
                else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", Convert.ToInt32( hdnOID.Value)), 
                        new SqlParameter("@statusid", Convert.ToInt32(38)), 
                        new SqlParameter("@feedback", "RIM INSPECTION COMPLETED. ORDER MOVED TO NEXT PROCESS"), 
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                }
            }
            Response.Redirect(Request.RawUrl, false);
        }
    }
}