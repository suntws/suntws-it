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
    public partial class cotsdispatchedlist : System.Web.UI.Page
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
                            (dtUser.Rows[0]["dom_ordertrack"].ToString() == "True" || dtUser.Rows[0]["exp_ordertrack"].ToString() == "True"))
                        {
                            if (Request["disid"] != null)
                            {
                                if (Request["disid"].ToString() == "dom")
                                    lblPageHead.Text = "DOMESTIC ";
                                else if (Request["disid"].ToString() == "exp")
                                    lblPageHead.Text = "INTERNATIONAL ";
                                DataTable dt = new DataTable();
                                SqlParameter[] sp = new SqlParameter[1];
                                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                                    sp = new SqlParameter[2];
                                sp[0] = new SqlParameter("@OrderType", Request["disid"].ToString().ToUpper());
                                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                                {
                                    sp[1] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedlist_plant_userwise", sp, DataAccess.Return_Type.DataTable);
                                }
                                else
                                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedlist_plant", sp, DataAccess.Return_Type.DataTable);
                                Utilities.ddl_Binding(ddlplant, dt, "plant", "plant", "ALL");
                                if (dt.Rows.Count > 0)
                                {
                                    ddl_bindyear();
                                    ddl_bindMonth();
                                    Bind_Dispatchedgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
                                }
                                else
                                    lblErrMsg.Text = "No Records";
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
        private void ddl_bindyear()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                    sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@ClaimType", Request["disid"].ToString().ToUpper());
                sp1[1] = new SqlParameter("@Plant", ddlplant.SelectedItem.Text);
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                {
                    sp1[2] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedlist_year_userwise", sp1, DataAccess.Return_Type.DataTable);
                }
                else
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedlist_year", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlYear, dt, "Dispatchedyear", "Dispatchedyear", "");
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
                DataTable dt = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[3];
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                    sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@ClaimType", Request["disid"].ToString().ToUpper());
                sp1[1] = new SqlParameter("@Plant", ddlplant.SelectedItem.Text);
                sp1[2] = new SqlParameter("@year", Convert.ToInt32(ddlYear.SelectedItem.Text));
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                {
                    sp1[3] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedlist_Month_userwise", sp1, DataAccess.Return_Type.DataTable);
                }
                else
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatchedlist_Month", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlMonth, dt, "Dispatchedmonth", "Dispatchedid", "");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindMonth();
            Bind_Dispatchedgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_Dispatchedgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindyear();
            ddl_bindMonth();
            Bind_Dispatchedgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        private void Bind_Dispatchedgvlist(string plant, string year, string month)
        {
            try
            {
                gvDispatchedorderlist.DataSource = null;
                gvDispatchedorderlist.DataBind();
                DataTable dtorderlist = new DataTable();
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                {
                    SqlParameter[] sp1 = new SqlParameter[5];
                    sp1[0] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                    sp1[1] = new SqlParameter("@Plant", plant);
                    sp1[2] = new SqlParameter("@year", Convert.ToInt32(year));
                    sp1[3] = new SqlParameter("@month", Convert.ToInt32(month));
                    sp1[4] = new SqlParameter("@OrderType", Request["disid"].ToString().ToUpper());
                    dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatched_orders_Userwise_PlantWise", sp1, DataAccess.Return_Type.DataTable);
                }
                else
                {
                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@Plant", plant);
                    sp1[1] = new SqlParameter("@year", Convert.ToInt32(year));
                    sp1[2] = new SqlParameter("@month", Convert.ToInt32(month));
                    sp1[3] = new SqlParameter("@OrderType", Request["disid"].ToString().ToUpper());
                    dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatched_orders_PlantWise", sp1, DataAccess.Return_Type.DataTable);
                }
                Bind_GvList(dtorderlist);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_GvList(DataTable dtList)
        {
            try
            {
                lblErrMsg.Text = "";
                lblOrderCount.Text = "";
                if (dtList.Rows.Count > 0)
                {
                    lblOrderCount.Text = dtList.Rows.Count.ToString();
                    gvDispatchedorderlist.DataSource = dtList;
                    gvDispatchedorderlist.DataBind();

                    object sumTotDispWt;
                    sumTotDispWt = dtList.Compute("Sum(TotWt)", "");
                    sumTotDispWt = Math.Round((Convert.ToDecimal(sumTotDispWt.ToString()) / 1000), 2);
                    lblTotDispWt.Text = sumTotDispWt.ToString();

                    ViewState["dtorderlist"] = dtList;
                    if (Request["disid"].ToString() == "exp")
                        gvDispatchedorderlist.Columns[8].Visible = false;
                    else
                        gvDispatchedorderlist.Columns[8].Visible = true;
                }
                else
                {
                    lblErrMsg.Text = "No records";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkDispatchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectDispatchedOrder(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvDispatchedorderlist_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtorderlist = ViewState["dtorderlist"] as DataTable;
                Bind_GvList(dtorderlist);
                gvDispatchedorderlist.PageIndex = e.NewPageIndex;
                gvDispatchedorderlist.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectDispatchedOrder(GridViewRow row)
        {
            try
            {
                lblStausOrderRefNo.Text = ((Label)row.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)row.FindControl("hdnStatusCustCode")).Value;
                lblCustName.Text = ((Label)row.FindControl("lblStatusCustName")).Text;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                Bind_OrderMasterDetails();
                Bind_OrderItem();
                Bind_OrderInstruction();
                Bind_ReviseDetails();
                Bind_StatusChangeDetails();
                Bind_LrCopyDetails();
                Bind_PDF_Files();
                Session["lrcopydetails"] = hdnCustCode.Value + "~" + lblStausOrderRefNo.Text;
                Session["pocopydetails"] = hdnCustCode.Value + "~" + lblStausOrderRefNo.Text;
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
                strAddress += "<div>" + row["EmailID"].ToString() + " / " + row["mobile"].ToString() + "</div>";
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
                        dtUser.Rows[0]["dom_invoice"].ToString() != "True" && dtUser.Rows[0]["exp_proforma"].ToString() != "True"))
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
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)) };
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
        private void Bind_LrCopyDetails()
        {
            try
            {
                Session["lrcopydetails"] = ""; Session["pocopydetails"] = ""; div_LR_upload.Style.Add("display", "none"); div_PO_upload.Style.Add("display", "none");
                if (Request["disid"].ToString() == "dom")
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)), new SqlParameter("@FileType", "UPLOAD LR COPY") };
                    DataTable dtLRFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AttachPdfFiles", sp, DataAccess.Return_Type.DataTable);
                    if (dtLRFile.Rows.Count == 0)
                    {
                        Session["lrcopydetails"] = hdnCustCode.Value + "~" + lblStausOrderRefNo.Text;
                        div_LR_upload.Style.Add("display", "block");
                    }
                    else
                        div_LR_upload.Style.Add("display", "none");

                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)), new SqlParameter("@FileType", "PO FILE") };
                    DataTable dtPOFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AttachPdfFiles", sp1, DataAccess.Return_Type.DataTable);
                    if (dtPOFile.Rows.Count == 0)
                    {
                        Session["pocopydetails"] = hdnCustCode.Value + "~" + lblStausOrderRefNo.Text;
                        div_PO_upload.Style.Add("display", "block");
                    }
                    else
                        div_PO_upload.Style.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnUploadPO_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUploadControl_PO.HasFile)
                {
                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    if (!Directory.Exists(serverURL + "/pocopyfiles/"))
                        Directory.CreateDirectory(serverURL + "/pocopyfiles/");
                    if (!Directory.Exists(serverURL + "/pocopyfiles/" + hdnCustCode.Value + "/"))
                        Directory.CreateDirectory(serverURL + "/pocopyfiles/" + hdnCustCode.Value + "/");
                    string path = serverURL + "/pocopyfiles/" + hdnCustCode.Value + "/";

                    string pathToSave = path + FileUploadControl_PO.FileName;
                    FileUploadControl_PO.SaveAs(pathToSave);

                    if (Directory.Exists(path))
                    {
                        string[] str1 = FileUploadControl_PO.FileName.Split('.');
                        string strExtension = str1[str1.Length - 1].ToString();
                        Directory.Move(pathToSave, serverURL + "/pocopyfiles/" + hdnCustCode.Value + "/PO_" + lblStausOrderRefNo.Text + "." + strExtension);

                        SqlParameter[] sp1 = new SqlParameter[4];
                        sp1[0] = new SqlParameter("@OID", Convert.ToInt32(hdnOID.Value));
                        sp1[1] = new SqlParameter("@AttachFileName", "PO_" + lblStausOrderRefNo.Text + "." + strExtension);
                        sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                        sp1[3] = new SqlParameter("@FileType", "PO FILE");
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);
                        if (resp > 0)
                        {
                            gvDispatchedorderlist.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                            Build_SelectDispatchedOrder(gvDispatchedorderlist.SelectedRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnUploadLR_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUploadControl_LR.HasFile)
                {
                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    if (!Directory.Exists(serverURL + "/lrcopyfiles/"))
                        Directory.CreateDirectory(serverURL + "/lrcopyfiles/");
                    if (!Directory.Exists(serverURL + "/lrcopyfiles/" + hdnCustCode.Value + "/"))
                        Directory.CreateDirectory(serverURL + "/lrcopyfiles/" + hdnCustCode.Value + "/");
                    string path = serverURL + "/lrcopyfiles/" + hdnCustCode.Value + "/";

                    string pathToSave = path + FileUploadControl_LR.FileName;
                    FileUploadControl_LR.SaveAs(pathToSave);

                    if (Directory.Exists(path))
                    {
                        string[] str1 = FileUploadControl_LR.FileName.Split('.');
                        string strExtension = str1[str1.Length - 1].ToString();
                        Directory.Move(pathToSave, serverURL + "/lrcopyfiles/" + hdnCustCode.Value + "/LR_" + lblStausOrderRefNo.Text + "." + strExtension);

                        SqlParameter[] sp1 = new SqlParameter[4];
                        sp1[0] = new SqlParameter("@OID", Convert.ToInt32(hdnOID.Value));
                        sp1[1] = new SqlParameter("@AttachFileName", "LR_" + lblStausOrderRefNo.Text + "." + strExtension);
                        sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                        sp1[3] = new SqlParameter("@FileType", "UPLOAD LR COPY");
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);
                        if (resp > 0)
                        {
                            gvDispatchedorderlist.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                            Build_SelectDispatchedOrder(gvDispatchedorderlist.SelectedRow);
                        }
                    }
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
                        dtUser.Rows[0]["dom_invoice"].ToString() != "True" && dtUser.Rows[0]["exp_proforma"].ToString() != "True"))
                    UserType = "PDI";
                else
                    UserType = "";
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@UserType", UserType) 
                };
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
                    case "POST SHIPMENT DOCUMENTS":
                        Url = Server.MapPath("~/PostShipmentDetails/" + hdnCustCode.Value + "/") + lnkpdfFileName.Text;
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