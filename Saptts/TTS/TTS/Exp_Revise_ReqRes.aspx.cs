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
using System.Text;

namespace TTS
{
    public partial class Exp_Revise_ReqRes : System.Web.UI.Page
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
                        hdnType.Value = Utilities.Decrypt(Request.QueryString["Type"].ToString());
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["exp_orderentry"].ToString() == "True" || dtUser.Rows[0]["exp_pricechange"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_ordersplit"].ToString() == "True" || dtUser.Rows[0]["exp_revise"].ToString() == "True") && hdnType.Value == "request")
                        {
                            lblPageHeading.Text = "Export Orders -> Revise Request";
                            DataTable dt = (DataTable)daCOTS.ExecuteReader("sp_sel_orderStatusMaster_request", DataAccess.Return_Type.DataTable);
                            rdo_RequstCrm.DataSource = dt;
                            rdo_RequstCrm.DataTextField = "StatusText";
                            rdo_RequstCrm.DataValueField = "id";
                            rdo_RequstCrm.DataBind();

                            ddl_PlantSelection_SelectedIndexChanged(null, null);
                        }
                        else if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["exp_proforma"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True" || dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_ppc_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sitl"].ToString() == "True" || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True" || dtUser.Rows[0]["ot_logisticsmmn"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_logistics_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_logistics_sitl"].ToString() == "True" || dtUser.Rows[0]["ot_logisticspdk"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True") && hdnType.Value == "response")
                        {
                            lblPageHeading.Text = "Export Orders -> Revise Response";
                            if (dtUser.Rows[0]["exp_proforma"].ToString() == "True")
                                hdnApproveBy_Res.Value = "1";
                            if (dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sitl"].ToString() == "True" || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True")
                                hdnApproveBy_Res.Value = (hdnApproveBy_Res.Value != "" ? hdnApproveBy_Res.Value + "," : "") + "2";
                            if (dtUser.Rows[0]["ot_logisticsmmn"].ToString() == "True" || dtUser.Rows[0]["ot_logistics_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_logistics_sitl"].ToString() == "True" || dtUser.Rows[0]["ot_logisticspdk"].ToString() == "True")
                                hdnApproveBy_Res.Value = (hdnApproveBy_Res.Value != "" ? hdnApproveBy_Res.Value + "," : "") + "3";
                            if (dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True" ||
                                dtUser.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True")
                                hdnApproveBy_Res.Value = (hdnApproveBy_Res.Value != "" ? hdnApproveBy_Res.Value + "," : "") + "4";

                            ddl_PlantSelection_SelectedIndexChanged(null, null);
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
                Utilities.WriteToErrorLog("EXPORT REVISE " + hdnType.Value.ToUpper() + "", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_PlantSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strUserName = (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support") ? Request.Cookies["TTSUser"].Value : "";
                SqlParameter[] sp = null;
                string spQuery = "";
                lblErrMsgcontent.Text = "";
                gvOrderDetails.DataSource = null;
                gvOrderDetails.DataBind();

                if (hdnType.Value == "request")
                {
                    sp = new SqlParameter[] { new SqlParameter("@Username", strUserName), new SqlParameter("@Plant", ddl_PlantSelection.SelectedItem.Value) };
                    spQuery = "sp_sel_Exp_ReqOrders";
                }
                else if (hdnType.Value == "response")
                {
                    sp = new SqlParameter[] { new SqlParameter("@Plant", ddl_PlantSelection.SelectedItem.Value), new SqlParameter("@ReviseRequestTo", hdnApproveBy_Res.Value) };
                    spQuery = "sp_sel_Exp_ReqResorders";
                }

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP(spQuery, sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvOrderDetails.DataSource = dt;
                    gvOrderDetails.DataBind();
                }
                else
                    lblErrMsgcontent.Text = "No Records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("EXPORT REVISE " + hdnType.Value.ToUpper() + "", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkReqResOrder_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow Row = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnCustCode.Value = ((HiddenField)Row.FindControl("hdnStatusCustCode")).Value;
                lblSelectedOrderRefNo.Text = ((Label)Row.FindControl("lblOrderRefNo")).Text;
                lblSelectedCustomerName.Text = ((Label)Row.FindControl("lblStatusCustName")).Text;
                hdnResponseID.Value = ((HiddenField)Row.FindControl("hdnReviseRequestTo")).Value;
                hdnOrderStatusID.Value = ((HiddenField)Row.FindControl("hdnOrderStatus")).Value;
                hdnPlant.Value = ((Label)Row.FindControl("lblPlant")).Text;
                hdnOID.Value = ((HiddenField)Row.FindControl("hdnOrderID")).Value;

                Bind_OrderItem();
                if (hdnType.Value == "response")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                    DataTable dtStatus = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_statuschange_history", sp1, DataAccess.Return_Type.DataTable);
                    if (dtStatus.Rows.Count > 0)
                        txt_ReqComments_Exist.Text = dtStatus.Rows[0]["feedback"].ToString().Replace("~", "\r\n");
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv", "ShowDiv('" + hdnType.Value + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("EXPORT REVISE " + hdnType.Value.ToUpper() + "", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderItem()
        {
            try
            {
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(hdnOID.Value));
                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    gvOrderItemList.FooterRow.Cells[7].Text = "Total";
                    gvOrderItemList.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                    gvOrderItemList.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gvOrderItemList.FooterRow.Cells[9].Text = Convert.ToDecimal(dtItemList.Compute("Sum(unitpricepdf)", "")).ToString();
                    gvOrderItemList.FooterRow.Cells[10].Text = Convert.ToDecimal(dtItemList.Compute("Sum(totalfwt)", "")).ToString();

                    gvOrderItemList.Columns[11].Visible = false;
                    gvOrderItemList.Columns[12].Visible = false;
                    gvOrderItemList.Columns[13].Visible = false;
                    gvOrderItemList.Columns[14].Visible = false;
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;
                        gvOrderItemList.Columns[13].Visible = true;
                        gvOrderItemList.Columns[14].Visible = true;

                        gvOrderItemList.FooterRow.Cells[12].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                        gvOrderItemList.FooterRow.Cells[13].Text = Convert.ToDecimal(dtItemList.Compute("Sum(Rimpricepdf)", "")).ToString();
                        gvOrderItemList.FooterRow.Cells[14].Text = Convert.ToDecimal(dtItemList.Compute("Sum(totalRimWt)", "")).ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("EXPORT REVISE " + hdnType.Value.ToUpper() + "", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdo_RequstCrm_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (rdo_RequstCrm.SelectedItem.Value == "15" || rdo_RequstCrm.SelectedItem.Value == "18" || rdo_RequstCrm.SelectedItem.Value == "27")
                {
                    if (hdnOrderStatusID.Value == "2" || hdnOrderStatusID.Value == "3" || hdnOrderStatusID.Value == "23" || hdnOrderStatusID.Value == "34" ||
                        hdnOrderStatusID.Value == "35")
                    {
                        lblApproveTeam.Text = "YOU SHOULD GET APPROVAL FROM CRM";
                        hdnApprovalID.Value = "1";
                        hdnApprovalTeam.Value = "exp_proforma";
                    }
                    else if (hdnOrderStatusID.Value == "4" || hdnOrderStatusID.Value == "24" || hdnOrderStatusID.Value == "40" || hdnOrderStatusID.Value == "41")
                    {
                        lblApproveTeam.Text = "YOU SHOULD GET APPROVAL FROM PPC";
                        hdnApprovalID.Value = "2";
                        hdnApprovalTeam.Value = "ot_ppc" + hdnPlant.Value;
                    }
                    else if (hdnOrderStatusID.Value == "25")
                    {
                        lblApproveTeam.Text = "YOU SHOULD GET APPROVAL FROM LOGISTICS";
                        hdnApprovalID.Value = "3";
                        hdnApprovalTeam.Value = "ot_logistics" + hdnPlant.Value;
                    }
                    else if (hdnOrderStatusID.Value == "19" || hdnOrderStatusID.Value == "20" || hdnOrderStatusID.Value == "21")
                    {
                        lblApproveTeam.Text = "YOU SHOULD GET APPROVAL FROM INSPECTION";
                        hdnApprovalID.Value = "4";
                        hdnApprovalTeam.Value = "exp_pdi_" + hdnPlant.Value;
                    }
                }
                else
                {
                    lblApproveTeam.Text = "NO NEED TO APPROVAL FOR CHANGES";
                    hdnApprovalID.Value = "0";
                    hdnApprovalTeam.Value = "exp_proforma";
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showdiv1", "ShowDiv('" + hdnType.Value + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("EXPORT REVISE " + hdnType.Value.ToUpper() + "", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_ReqRes_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                int StatusID = 0; string strComments = "";
                if (hdnType.Value == "request")
                {
                    SqlParameter[] sp2 = new SqlParameter[] { 
                        new SqlParameter("@ReviseRequestTo", Convert.ToInt32(hdnApprovalID.Value)), 
                        new SqlParameter("@RequestStatus", Convert.ToInt32(rdo_RequstCrm.SelectedItem.Value)), 
                        new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)) 
                    };
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_exp_reviserequest", sp2);
                    StatusID = Convert.ToInt32(rdo_RequstCrm.SelectedItem.Value);
                    strComments = txt_ReqComments_New.Text;
                }
                else if (hdnType.Value == "response")
                {
                    StatusID = 17;
                    strComments = txt_ResComments.Text;
                    string strTbField = string.Empty;
                    if (hdnResponseID.Value == "1")
                        strTbField = "exp_proforma";
                    else if (hdnResponseID.Value == "2")
                        strTbField = "ot_ppc" + hdnPlant.Value;
                    else if (hdnResponseID.Value == "3")
                        strTbField = "ot_logistics" + hdnPlant.Value;
                    else if (hdnResponseID.Value == "4")
                        strTbField = "exp_pdi_" + hdnPlant.Value;

                    if (hdnResponseID.Value == "2" || hdnResponseID.Value == "3" || hdnResponseID.Value == "4")
                    {
                        SqlParameter[] sp = new SqlParameter[] { 
                            new SqlParameter("@custcode", hdnCustCode.Value), 
                            new SqlParameter("@orderrefno", lblSelectedOrderRefNo.Text), 
                            new SqlParameter("@PdiPlant", hdnPlant.Value), 
                            new SqlParameter("@orderstatusid", hdnOrderStatusID.Value) 
                        };
                        daCOTS.ExecuteNonQuery_SP("sp_upd_pdi_scanmasterdata_holdstatus", sp);
                    }
                }

                if (hdnApprovalID.Value != "0" || hdnType.Value == "response")
                {
                    SqlParameter[] spIns = new SqlParameter[] { 
                        new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)), 
                        new SqlParameter("@statusid", StatusID), 
                        new SqlParameter("@feedback", strComments.Replace("\r\n", "~")), 
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                    };
                    resp = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", spIns);
                }
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("EXPORT REVISE " + hdnType.Value.ToUpper() + "", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvOrderDetails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}