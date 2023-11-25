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
using System.IO;

namespace TTS
{
    public partial class cotstrackstatus : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_ordertrack"].ToString() == "True")
                            Bind_TrackOrder_list("ALL");
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
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_TrackOrder_list(ddlplant.SelectedValue);
        }
        private void Bind_TrackOrder_list(string plant)
        {
            try
            {
                lblErrMsg.Text = "";
                lblOrderCount.Text = "";
                DataTable dtlist = new DataTable();
                string strUserName = (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support") ?
                    Request.Cookies["TTSUser"].Value : "";

                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@Username", strUserName), new SqlParameter("@Plant", plant) };
                dtlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dom_track_orders_status", sp2, DataAccess.Return_Type.DataTable);

                if (dtlist.Rows.Count > 0)
                {
                    lblOrderTotWt.Text = (dtlist.Compute("Sum(FWT)", "")).ToString();
                    lblOrderCount.Text = dtlist.Rows.Count.ToString();
                    gvTrackOrderList.DataSource = dtlist;
                    gvTrackOrderList.DataBind();
                }
                else
                    lblErrMsg.Text = "No records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvTrackOrderList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Bind_TrackOrder_list(ddlplant.SelectedValue);
                gvTrackOrderList.PageIndex = e.NewPageIndex;
                gvTrackOrderList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkTrackBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectOrderDetails(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectOrderDetails(GridViewRow row)
        {
            try
            {
                lblStausOrderRefNo.Text = ((Label)row.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)row.FindControl("hdnStatusCustCode")).Value;
                lblCurrStatus.Text = ((Label)row.FindControl("lblStatusText")).Text;
                lblCustName.Text = ((Label)row.FindControl("lblStatusCustName")).Text;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                Bind_OrderMasterDetails();
                Bind_OrderItem();
                Bind_OrderInstruction();
                Bind_ReviseDetails();
                Bind_StatusChangeDetails();

                gv_DownloadFiles.DataSource = "";
                gv_DownloadFiles.DataBind();
                if (((HiddenField)row.FindControl("hdnStatusID")).Value != "1")
                    Bind_PDF_Files();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderMasterDetails()
        {
            DataTable dtMasterList = DomesticScots.Bind_OrderMasterDetails(Convert.ToInt32(hdnOID.Value));
            if (dtMasterList.Rows.Count > 0)
            {
                dlOrderMaster.DataSource = dtMasterList;
                dlOrderMaster.DataBind();
            }
        }
        public string Bind_BillingAddress(string BillID)
        {
            string strAddress = string.Empty;
            DataTable dtAddressList = DomesticScots.Bind_BillingAddress(BillID);

            if (dtAddressList.Rows.Count > 0)
            {
                DataRow row = dtAddressList.Rows[0];
                strAddress += "<div style='font-weight:bold;'>" + row["contact_name"].ToString() + "</div>";
                strAddress += "<div>" + row["shipaddress"].ToString().Replace("~", "<br/>") + "</div>";
                strAddress += "<div>" + row["city"].ToString() + ", " + row["statename"].ToString() + "</div>";
                strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
                strAddress += "<div>GST: " + row["GST_No"].ToString() + "</div>";
            }
            return strAddress;
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

                    gvOrderItemList.FooterRow.Cells[6].Text = "TOTAL";
                    gvOrderItemList.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    object sumQty = dtItemList.Compute("Sum(itemqty)", "");
                    gvOrderItemList.FooterRow.Cells[8].Text = sumQty.ToString();

                    gvOrderItemList.Columns[10].Visible = false; //RIM QTY
                    gvOrderItemList.Columns[11].Visible = false; //RIM BASIC PRICE
                    gvOrderItemList.Columns[12].Visible = false; //RIM FWT
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[10].Visible = true;
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;
                        break;
                    }
                    DataTable dtUser = (DataTable)Session["dtuserlevel"];
                    if (dtUser != null && (dtUser.Rows[0]["dom_proforma"].ToString() != "True" && dtUser.Rows[0]["dom_paymentconfirm"].ToString() != "True" &&
                        dtUser.Rows[0]["dom_invoice"].ToString() != "True"))
                    {
                        gvOrderItemList.Columns[7].Visible = false;
                        gvOrderItemList.Columns[11].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderInstruction()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtIns = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Instruction_Request", sp1, DataAccess.Return_Type.DataTable);
                if (dtIns.Rows.Count > 0)
                {
                    txtOrderSplIns.Text = dtIns.Rows[0]["SplIns"].ToString().Replace("~", "\r\n");
                    txtOrdersplReq.Text = dtIns.Rows[0]["SpecialRequset"].ToString().Replace("~", "\r\n");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ReviseDetails()
        {
            try
            {
                lblReviseHistory.Text = "";
                gvRevisedHistory.DataSource = null;
                gvRevisedHistory.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtRevise = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_revisedlist", sp1, DataAccess.Return_Type.DataTable);
                if (dtRevise != null && dtRevise.Rows.Count > 0)
                {
                    gvRevisedHistory.DataSource = dtRevise;
                    gvRevisedHistory.DataBind();
                    lblReviseHistory.Text = "ORDER REVISED HISTORY";
                }
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
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
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
        private void Bind_PDF_Files()
        {
            try
            {
                string UserType = "";
                DataTable dtUser = (DataTable)Session["dtuserlevel"];
                if (dtUser != null && (dtUser.Rows[0]["dom_proforma"].ToString() != "True" && dtUser.Rows[0]["dom_paymentconfirm"].ToString() != "True" &&
                        dtUser.Rows[0]["dom_invoice"].ToString() != "True"))
                    UserType = "PDI";
                else
                    UserType = "";
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value), new SqlParameter("@UserType", UserType) };
                DataTable dtPdfFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdf_PendingStatus_AttachInvoiceFiles", sp1, DataAccess.Return_Type.DataTable);
                if (dtPdfFile.Rows.Count > 0)
                {
                    gv_DownloadFiles.DataSource = dtPdfFile;
                    gv_DownloadFiles.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "document.getElementById('div_DownloadFiles').style.display='block';", true);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('div_DownloadFiles').style.display='none';", true);
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
                    case "PROFORMA FILE":
                        Url = Server.MapPath("~/proformafiles/" + hdnCustCode.Value + "/") + lnkpdfFileName.Text;
                        break;
                    case "WORKORDER FILE":
                        Url = Server.MapPath("~/workorderfiles/" + hdnCustCode.Value + "/") + lnkpdfFileName.Text;
                        break;
                    case "UPLOAD LR COPY":
                        Url = Server.MapPath("~/lrcopyfiles/" + hdnCustCode.Value + "/") + lnkpdfFileName.Text;
                        break;
                    case "DUPLICATE FOR TRANSPORTER":
                    case "ORIGINAL FOR RECEPIENT":
                    case "TRIPLICATE FOR SUPPLIER":
                    case "PDI LIST":
                        Url = Server.MapPath("~/invoicefiles/" + hdnCustCode.Value + "/") + lnkpdfFileName.Text;
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

       
    }
}