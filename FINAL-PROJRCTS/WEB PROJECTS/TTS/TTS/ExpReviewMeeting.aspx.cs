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
    public partial class ExpReviewMeeting : System.Web.UI.Page
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
                                SqlParameter[] spExp = new SqlParameter[] { new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()) };
                                DataTable dtSumExp = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_DispatchReview_Summary", spExp, DataAccess.Return_Type.DataTable);

                                SqlParameter[] spDom = new SqlParameter[] { new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()) };
                                DataTable dtSumDom = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dom_DispatchReview_Summary", spDom, DataAccess.Return_Type.DataTable);

                                if (dtSumExp.Rows.Count > 0 || dtSumDom.Rows.Count > 0)
                                {
                                    Bind_TrackOrder_list();

                                    lblReviewPlant.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper();
                                    //SHIPPABLE BACKLOG
                                    DataRow[] foundRows = dtSumExp.Select("Backlog in ('CURRENT MONTH','NEXT MONTH')");
                                    double decFwt = 0;
                                    foreach (DataRow dr in foundRows) { decFwt += Convert.ToDouble(dr["fwt"]); }
                                    lblShipableBacklog.Text = decFwt.ToString("0");

                                    foundRows = dtSumExp.Select("Backlog in ('CARRY FORWARD')");
                                    decFwt = 0;
                                    foreach (DataRow dr in foundRows) { decFwt += Convert.ToDouble(dr["fwt"]); }
                                    lblCarryForward.Text = decFwt.ToString("0");

                                    foundRows = dtSumExp.Select("Backlog in ('CURRENT MONTH')");
                                    decFwt = 0;
                                    foreach (DataRow dr in foundRows) { decFwt += Convert.ToDouble(dr["fwt"]); }
                                    lblCurrMonthShip.Text = decFwt.ToString("0");

                                    lblExpPlanTot.Text = (Convert.ToDouble(lblCarryForward.Text) + Convert.ToDouble(lblCurrMonthShip.Text)).ToString("0");

                                    lblExpPlan.Text = (Convert.ToDouble(lblCarryForward.Text) + Convert.ToDouble(lblCurrMonthShip.Text)).ToString("0");

                                    foundRows = dtSumExp.Select("Backlog in ('DISPATCHED')");
                                    decFwt = 0;
                                    foreach (DataRow dr in foundRows) { decFwt += Convert.ToDouble(dr["fwt"]); }
                                    lblExpDisp.Text = decFwt.ToString("0");

                                    lblExpBal.Text = (Convert.ToDouble(lblExpPlan.Text) - Convert.ToDouble(lblExpDisp.Text)).ToString("0");

                                    //TOT BACKLOG
                                    foundRows = dtSumExp.Select("Backlog in ('FUTURE SHIPMENTS')");
                                    decFwt = 0;
                                    foreach (DataRow dr in foundRows) { decFwt += Convert.ToDouble(dr["fwt"]); }
                                    lblTotBacklog.Text = (Convert.ToDouble(lblShipableBacklog.Text) + Convert.ToDouble(lblCarryForward.Text) + decFwt).ToString("0");

                                    foundRows = dtSumDom.Select("Backlog in ('CARRY FORWARD','CURRENT MONTH')");
                                    decFwt = 0;
                                    foreach (DataRow dr in foundRows) { decFwt += Convert.ToDouble(dr["fwt"]); }
                                    lblDomPlan.Text = decFwt.ToString("0");

                                    foundRows = dtSumDom.Select("Backlog in ('DISPATCHED')");
                                    decFwt = 0;
                                    foreach (DataRow dr in foundRows) { decFwt += Convert.ToDouble(dr["fwt"]); }
                                    lblDomDisp.Text = decFwt.ToString("0");

                                    lblDomBalance.Text = ((Convert.ToDouble(lblDomPlan.Text) - Convert.ToDouble(lblDomDisp.Text))).ToString("0");
                                }
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
                //rdbstatus.SelectedIndex = -1;
                //txtCommt.Text = "";
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

                    if (e.Row.RowIndex == 0)
                    {
                        GridViewRow row = new GridViewRow(e.Row.RowIndex - 1, e.Row.RowIndex - 1, DataControlRowType.DataRow, DataControlRowState.Normal);
                        TableCell cell1 = new TableCell();
                        cell1.ColumnSpan = 12;
                        cell1.Text = "CARRY FORWARD";
                        row.Cells.Add(cell1);
                        gvTrackOrderList.Controls[0].Controls.Add(row);
                    }
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