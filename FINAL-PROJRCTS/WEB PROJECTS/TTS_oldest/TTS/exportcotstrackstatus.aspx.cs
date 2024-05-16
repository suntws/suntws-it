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

namespace TTS
{
    public partial class exportcotstrackstatus : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_ordertrack"].ToString() == "True")
                        {
                            string strUserName = "";
                            if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                                strUserName = Request.Cookies["TTSUser"].Value;
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Username", strUserName) };
                            DataTable dtlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_Track_orders_List", sp, DataAccess.Return_Type.DataTable);
                            ViewState["dtExpTracklist"] = dtlist;

                            DataView dtDescView = new DataView(dtlist);
                            dtDescView.Sort = "Plant ASC";
                            DataTable distinctPlant = dtDescView.ToTable(true, "Plant");
                            ddl_Plant.DataSource = distinctPlant;
                            ddl_Plant.DataTextField = "Plant";
                            ddl_Plant.DataValueField = "Plant";
                            ddl_Plant.DataBind();
                            ddl_Plant.Items.Insert(0, "ALL");

                            dtDescView.Sort = "custfullname ASC";
                            DataTable distinctCustomer = dtDescView.ToTable(true, "custfullname");
                            ddl_Customer.DataSource = distinctCustomer;
                            ddl_Customer.DataTextField = "custfullname";
                            ddl_Customer.DataValueField = "custfullname";
                            ddl_Customer.DataBind();
                            ddl_Customer.Items.Insert(0, "ALL");

                            dtDescView.Sort = "workorderno ASC";
                            DataTable distinctWO = dtDescView.ToTable(true, "workorderno");
                            ddl_WorkOrder.DataSource = distinctWO;
                            ddl_WorkOrder.DataTextField = "workorderno";
                            ddl_WorkOrder.DataValueField = "workorderno";
                            ddl_WorkOrder.DataBind();
                            ddl_WorkOrder.Items.Insert(0, "ALL");

                            Bind_TrackOrder_list(dtlist);
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
        private void Bind_TrackOrder_list(DataTable dtData)
        {
            try
            {
                if (dtData.Rows.Count > 0)
                {
                    gvTrackOrderList.DataSource = dtData;
                    gvTrackOrderList.DataBind();
                }
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
                hdnCurrency.Value = ((HiddenField)row.FindControl("hdnUserCurrency")).Value;
                hdnCustCode.Value = ((HiddenField)row.FindControl("hdnStatusCustCode")).Value;
                lblCurrStatus.Text = ((Label)row.FindControl("lblStatusText")).Text;
                lblCustName.Text = ((Label)row.FindControl("lblStatusCustName")).Text;
                lblStausOrderRefNo.Text = ((Label)row.FindControl("lblOrderRefNo")).Text;
                hdnPlant.Value = ((Label)row.FindControl("lblPlant")).Text;
                hdnStatusid.Value = ((HiddenField)row.FindControl("hdnStatusID")).Value;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                Bind_OrderMasterDetails();
                Bind_OrderItemList();
                Bind_ReviseDetails();
                Bind_StatusChangeDetails();
                if (((HiddenField)row.FindControl("hdnStatusID")).Value != "1")
                {
                    Bind_PDF_Files();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "document.getElementById('div_DownloadFiles').style.display='block';", true);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('div_DownloadFiles').style.display='none';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderMasterDetails()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtMasterList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_OrderAllDetails_Track", sp1, DataAccess.Return_Type.DataTable);
                if (dtMasterList.Rows.Count > 0)
                {
                    dlOrderMaster.DataSource = dtMasterList;
                    dlOrderMaster.DataBind();
                    txtOrderSplIns.Text = dtMasterList.Rows[0]["SplIns"].ToString().Replace("~", "\r\n");
                    txtOrdersplReq.Text = dtMasterList.Rows[0]["SpecialRequset"].ToString().Replace("~", "\r\n");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public string Bind_BillingAddress(string BillID)
        {
            string strAddress = string.Empty;
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@addressid", BillID) };
                DataTable dtAddressList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderAddressDetails", sp1, DataAccess.Return_Type.DataTable);

                if (dtAddressList.Rows.Count > 0)
                {
                    DataRow row = dtAddressList.Rows[0];
                    strAddress += "<div style='font-weight:bold;'>" + row["CompanyName"].ToString() + "</div>";
                    strAddress += "<div>" + row["shipaddress"].ToString().Replace("~", "<br/>") + "</div>";
                    strAddress += "<div>" + row["city"].ToString() + "</div>";
                    strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
                    strAddress += "<div>" + row["contact_name"].ToString() + "</div>";
                    strAddress += "<div>" + row["EmailID"].ToString() + " / " + row["mobile"].ToString() + "</div>";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAddress;
        }
        private void Bind_OrderItemList()
        {
            try
            {
                gvOrderItemList.DataSource = null;
                gvOrderItemList.DataBind();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_orderitem_list_PPC", sp, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    gvOrderItemList.FooterRow.Cells[6].Text = "Total";
                    gvOrderItemList.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    gvOrderItemList.FooterRow.Cells[7].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gvOrderItemList.FooterRow.Cells[14].Text = Convert.ToDecimal(dtItemList.Compute("Sum(finishedwt)", "")).ToString();

                    gvOrderItemList.Columns[10].Visible = false;
                    gvOrderItemList.Columns[11].Visible = false;
                    gvOrderItemList.Columns[12].Visible = false;
                    foreach (DataRow row1 in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[10].Visible = true;
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;

                        gvOrderItemList.FooterRow.Cells[10].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                    }
                    DataTable dtUser = (DataTable)Session["dtuserlevel"];
                    if (dtUser != null && dtUser.Rows[0]["exp_proforma"].ToString() != "True")
                    {
                        gvOrderItemList.Columns[8].Visible = false;
                        gvOrderItemList.Columns[13].Visible = false;
                    }
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
                DataTable dtStatus = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_statuschange_history", sp1, DataAccess.Return_Type.DataTable);
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
                string UserType = ""; lnkExpExcelFiles.Text = "";
                DataTable dtUser = (DataTable)Session["dtuserlevel"];
                if (dtUser != null && dtUser.Rows[0]["exp_proforma"].ToString() != "True")
                    UserType = "PDI";
                else
                {
                    UserType = "";
                    lnkExpExcelFiles.Text = "DONWLOAD";
                }
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
        protected void lnkExpExcelFiles_click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value),
                    new SqlParameter("@requestmail", Request.Cookies["TTSUserEmail"].Value),
                    new SqlParameter("@requestby", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@processtype", "ASSIGN STENCIL"),
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_excel_prepare_history", sp);

                sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value),
                    new SqlParameter("@requestmail", Request.Cookies["TTSUserEmail"].Value),
                    new SqlParameter("@requestby", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@processtype", "EXP PROFORMA")
                };
                resp += daCOTS.ExecuteNonQuery_SP("sp_ins_excel_prepare_history", sp);

                sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@requestmail", Request.Cookies["TTSUserEmail"].Value), 
                    new SqlParameter("@requestby", Request.Cookies["TTSUser"].Value), 
                    new SqlParameter("@processtype", "EXP WORKORDER") 
                };
                resp += daCOTS.ExecuteNonQuery_SP("sp_ins_excel_prepare_history", sp);

                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showmailmsg", "alert('EXCEL FILE WILL BE SENT TO YOUR E-MAIL SHORTLY');", true);
                    gvTrackOrderList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                    Build_SelectOrderDetails(gvTrackOrderList.SelectedRow);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Plant_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddl_Customer.SelectedIndex = 0;
                ddl_WorkOrder.SelectedIndex = 0;
                DataTable dtList = (DataTable)ViewState["dtExpTracklist"];
                DataView dv_Plant = new DataView(dtList);
                if (ddl_Plant.SelectedItem.Text != "ALL")
                    dv_Plant.RowFilter = "Plant = '" + ddl_Plant.SelectedItem.Text + "'";
                //dv_Brand.Sort = "Plant ASC";
                DataTable dt_Plant = dv_Plant.ToTable(true);
                Bind_TrackOrder_list(dt_Plant);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Customer_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddl_Plant.SelectedIndex = 0;
                ddl_WorkOrder.SelectedIndex = 0;
                DataTable dtList = (DataTable)ViewState["dtExpTracklist"];
                DataView dv_Customer = new DataView(dtList);
                if (ddl_Customer.SelectedItem.Text != "ALL")
                    dv_Customer.RowFilter = "custfullname = '" + ddl_Customer.SelectedItem.Text + "'";
                //dv_Brand.Sort = "Plant ASC";
                DataTable dt_Customer = dv_Customer.ToTable(true);
                Bind_TrackOrder_list(dt_Customer);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_WorkOrder_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddl_Plant.SelectedIndex = 0;
                ddl_Customer.SelectedIndex = 0;
                DataTable dtList = (DataTable)ViewState["dtExpTracklist"];
                DataView dv_Wo = new DataView(dtList);
                if (ddl_WorkOrder.SelectedItem.Text != "ALL")
                    dv_Wo.RowFilter = "workorderno = '" + ddl_WorkOrder.SelectedItem.Text + "'";
                //dv_Brand.Sort = "Plant ASC";
                DataTable dt_Wo = dv_Wo.ToTable(true);
                Bind_TrackOrder_list(dt_Wo);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvTrackOrderList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}