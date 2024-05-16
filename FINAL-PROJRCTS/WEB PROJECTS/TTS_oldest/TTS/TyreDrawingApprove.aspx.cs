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
    public partial class TyreDrawingApprove : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        lblApproveHead.Text = "";
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dwg_edc_level1"].ToString() == "True" || dtUser.Rows[0]["dwg_edc_level2"].ToString() == "True"
                            || dtUser.Rows[0]["dwg_edc_level3"].ToString() == "True" || dtUser.Rows[0]["dwg_crm_approve"].ToString() == "True"))
                        {
                            DataTable dtDwgList = new DataTable();
                            if ((dtUser.Rows[0]["dwg_edc_level1"].ToString() == "True" || dtUser.Rows[0]["dwg_edc_level2"].ToString() == "True"
                            || dtUser.Rows[0]["dwg_edc_level3"].ToString() == "True") && Request["qid"].ToString() != "crm" && Request["qid"] != "")
                            {
                                if (Request["pid"].ToString() == "cus")
                                {
                                    if (Request["qid"].ToString() == "edc1")
                                        dtDwgList = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ECDApprovedPending", DataAccess.Return_Type.DataTable);
                                    else if (Request["qid"].ToString() == "edc2")
                                        dtDwgList = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ECDApprovedPending1", DataAccess.Return_Type.DataTable);
                                    else if (Request["qid"].ToString() == "edc3")
                                        dtDwgList = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ECDApprovedPending2", DataAccess.Return_Type.DataTable);
                                    lblApproveHead.Text = "DRAWING EDC APPROVE FOR CUSTOMER";
                                }
                                else if (Request["pid"].ToString() == "sup")
                                {
                                    lblApproveHead.Text = "DRAWING EDC APPROVE FOR SUPPLIER";
                                    if (Request["qid"].ToString() == "edc1")
                                        dtDwgList = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ECDApprovedPending_Supplier", DataAccess.Return_Type.DataTable);
                                    else if (Request["qid"].ToString() == "edc2")
                                        dtDwgList = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ECDApprovedPending1_Supplier", DataAccess.Return_Type.DataTable);
                                    else if (Request["qid"].ToString() == "edc3")
                                        dtDwgList = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ECDApprovedPending2_Supplier", DataAccess.Return_Type.DataTable);
                                }
                                if (dtDwgList.Rows.Count > 0)
                                {
                                    ViewState["dtDwgList"] = dtDwgList;
                                    bind_gvDwgList();
                                }
                            }
                            else if (dtUser.Rows[0]["dwg_crm_approve"].ToString() == "True" && Request["qid"].ToString() == "crm" && Request["qid"] != "")
                            {
                                if (Request["pid"].ToString() == "cus")
                                {
                                    lblApproveHead.Text = "DRAWING CRM APPROVE FOR CUSTOMER";
                                    dtDwgList = (DataTable)daTTS.ExecuteReader_SP("sp_sel_CRMApprovedPending", DataAccess.Return_Type.DataTable);
                                }
                                else if (Request["pid"].ToString() == "sup")
                                {
                                    lblApproveHead.Text = "DRAWING CRM APPROVE FOR SUPPLIER";
                                    dtDwgList = (DataTable)daTTS.ExecuteReader_SP("sp_sel_CRMApprovedPendingSupplier", DataAccess.Return_Type.DataTable);
                                }
                                if (dtDwgList.Rows.Count > 0)
                                {
                                    ViewState["dtDwgList"] = dtDwgList;
                                    bind_gvDwgList();
                                }
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

        protected void gvDwgApprovePendingList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                bind_gvDwgList();
                gvDwgApprovePendingList.PageIndex = e.NewPageIndex;
                gvDwgApprovePendingList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bind_gvDwgList()
        {
            try
            {
                gvDwgApprovePendingList.DataSource = null;
                gvDwgApprovePendingList.DataBind();
                DataTable dtDwgList = ViewState["dtDwgList"] as DataTable;
                if (dtDwgList.Rows.Count > 0)
                {
                    gvDwgApprovePendingList.DataSource = dtDwgList;
                    gvDwgApprovePendingList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}