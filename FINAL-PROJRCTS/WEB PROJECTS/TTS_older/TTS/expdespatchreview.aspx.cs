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
    public partial class expdespatchreview : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["exp_ordertrack"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "")
                            {
                                lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + " - ORDER FOR REVIEW";
                                Bind_TrackOrder_list();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
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
        private void Bind_TrackOrder_list()
        {
            try
            {
                string strUserName = (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support") ?
                    Request.Cookies["TTSUser"].Value : "";
                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) };
                DataTable dtlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ReviewMeeting_orders_Track_List", sp2, DataAccess.Return_Type.DataTable);
                if (dtlist.Rows.Count > 0)
                {
                    gvTrackOrderList.DataSource = dtlist;
                    gvTrackOrderList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkTrackBtn_click(object sender, EventArgs e)
        {
            try
            {
                rdbstatus.SelectedIndex = -1;
                txtCommt.Text = "";
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblCustName.Text = clickedRow.Cells[0].Text;
                lblWorkorder.Text = clickedRow.Cells[1].Text;
                hdnO_ID.Value = ((HiddenField)clickedRow.FindControl("hdnOID")).Value;

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnO_ID.Value) };
                DataTable dtSumList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Order_SumValue_ForReview", sp1, DataAccess.Return_Type.DataTable);
                if (dtSumList.Rows.Count > 0)
                {
                    gv_OrderSumValue.DataSource = dtSumList;
                    gv_OrderSumValue.DataBind();

                    gv_OrderSumValue.FooterRow.Cells[4].Text = "TOTAL";
                    gv_OrderSumValue.FooterRow.Cells[5].Text = dtSumList.Compute("Sum([TOT QTY])", "").ToString();
                    gv_OrderSumValue.FooterRow.Cells[6].Text = dtSumList.Compute("Sum([TOT WT])", "").ToString();
                }

                Bind_StatusChangeDetails();
                //Bind_ReviewStatusChangeDetails();
                Bind_PDF_Files();

                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_SingleView');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_StatusChangeDetails()
        {
            try
            {
                lblStatusHistory.Text = "";
                gvStatusHistory.DataSource = null;
                gvStatusHistory.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnO_ID.Value) };
                DataTable dtStatus = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_statuschange_history", sp1, DataAccess.Return_Type.DataTable);
                if (dtStatus != null && dtStatus.Rows.Count > 0)
                {
                    gvStatusHistory.DataSource = dtStatus;
                    gvStatusHistory.DataBind();
                    lblStatusHistory.Text = "STATUS CHANGED HISTORY";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ReviewStatusChangeDetails()
        {
            try
            {
                lblReviewStatus.Text = "";
                gvReviewHistory.DataSource = null;
                gvReviewHistory.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnO_ID.Value) };
                DataTable dtStatus = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderReviewstatuschange_history", sp1, DataAccess.Return_Type.DataTable);
                if (dtStatus != null && dtStatus.Rows.Count > 0)
                {
                    gvReviewHistory.DataSource = dtStatus;
                    gvReviewHistory.DataBind();
                    lblReviewStatus.Text = "REVIEW MEETING HISTORY";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_PDF_Files()
        {
            try
            {
                string UserType = "";
                DataTable dtUser = (DataTable)Session["dtuserlevel"];
                if (dtUser != null && dtUser.Rows[0]["exp_proforma"].ToString() != "True")
                    UserType = "PDI";
                else
                    UserType = "";
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnO_ID.Value), new SqlParameter("@UserType", UserType) };
                DataTable dtPdfFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdf_PendingStatus_AttachInvoiceFiles", sp1, DataAccess.Return_Type.DataTable);
                if (dtPdfFile.Rows.Count > 0)
                {
                    gv_DownloadFiles.DataSource = dtPdfFile;
                    gv_DownloadFiles.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_DownloadFiles_ItemCommand(object source, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)source).NamingContainer as GridViewRow;
                Label FileType = (Label)clickedRow.FindControl("lblFileType");
                LinkButton lnkpdfFileName = (LinkButton)clickedRow.FindControl("lnkPdfFileName");
                string Url = "";
                switch (FileType.Text)
                {
                    case "WORKORDER FILE":
                        Url = Server.MapPath("~/workorderfiles/" + hdn_Custcode.Value + "/") + lnkpdfFileName.Text;
                        break;
                    case "PDI LIST":
                        Url = Server.MapPath("~/invoicefiles/" + hdn_Custcode.Value + "/") + lnkpdfFileName.Text;
                        break;
                }
                Url = Url.Replace("TTS", "pdfs");
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkpdfFileName.Text);
                Response.WriteFile(Url);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvTrackOrderList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DataRowView dr1 = (DataRowView)e.Row.DataItem;
                    if (dr1["ShipmentType"].ToString() == "COMBI")
                        e.Row.Style.Add("background-color", "#effdc3");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_Save_click(object sender, EventArgs e)
        {
            try
            {
                //SqlParameter[] sp1 = new SqlParameter[6];
                //sp1[0] = new SqlParameter("@custcode", hdn_Custcode.Value);
                //sp1[1] = new SqlParameter("@orderrefno", lblSelectedOrderRefNo.Text);
                //sp1[2] = new SqlParameter("@Comments", txtCommt.Text);
                //sp1[3] = new SqlParameter("@ProcessType", rdbstatus.SelectedItem.Text);
                //sp1[4] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                //sp1[5] = new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()));
                //daCOTS.ExecuteNonQuery_SP("sp_Ins_ExpReviewMeeting", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}