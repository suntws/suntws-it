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
using System.Globalization;

namespace TTS
{
    public partial class Exp_Documents : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_documents"].ToString() == "True" && Request["mid"] != null)
                        {
                            lblPageHead.Text = Utilities.Decrypt(Request["mid"].ToString()) == "documents" ? "SHIPPING DOCUMENT PROCESS" : "CONTAINER DELIVERED AT DESTINATION";

                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Qstring", Utilities.Decrypt(Request["mid"].ToString())) };
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_OrderDocuments", sp, DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvDocumentVerifyList.DataSource = dtorderlist;
                                gvDocumentVerifyList.DataBind();
                            }
                            else
                                lblErrMsg.Text = "No records";
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
        protected void btnShowDetails_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;
            hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
            Build_SelectOrder(clickedRow);
        }
        private void Build_SelectOrder(GridViewRow rowClick)
        {
            try
            {
                lblSelectedOrderRefNo.Text = ((Label)rowClick.FindControl("lblOrderRefNo")).Text;
                lblSelectedCustomerName.Text = ((Label)rowClick.FindControl("lblCustomerName")).Text;
                hdnCustCode.Value = ((HiddenField)rowClick.FindControl("hdnStatusCustCode")).Value;
                hdnPlant.Value = ((Label)rowClick.FindControl("lblPlant")).Text;
                hdnOID.Value = ((HiddenField)rowClick.FindControl("hdnOrderID")).Value;

                if (((HiddenField)rowClick.FindControl("hdnStatusID")).Value != "39")
                    Bind_OrderSumValue();

                if (((HiddenField)rowClick.FindControl("hdnStatusID")).Value == "9")
                {
                    txt_InvoiceNo.Text = "";
                    txt_Date.Text = "";
                    txt_Currency.Text = "";
                    txt_ExchangeRate.Text = "";
                    txt_Blno.Text = "";
                    txt_BLDate.Text = "";
                    txt_ContainerNo.Text = "";
                    txt_SBillingNO.Text = "";
                    txt_SBillDate.Text = "";
                    txt_Port.Text = "";
                    btn_Save.Text = "SAVE";
                    btn_Clear.Style.Add("display", "block");
                    btnMoveStatus.Style.Add("display", "none");

                    lnkIN.Text = "";
                    div_IN_Clear.Style.Add("display", "none");
                    div_IN_upload.Style.Add("display", "block");
                    lnkPL.Text = "";
                    div_PL_Clear.Style.Add("display", "none");
                    div_PL_upload.Style.Add("display", "block");
                    lnkBL.Text = "";
                    div_BL_Clear.Style.Add("display", "none");
                    div_BL_upload.Style.Add("display", "block");
                    lnkOrigin.Text = "";
                    div_Origin_Clear.Style.Add("display", "none");
                    div_Origin_upload.Style.Add("display", "block");
                    lnkIns.Text = "";
                    div_Ins_Clear.Style.Add("display", "none");
                    div_Insur_upload.Style.Add("display", "block");
                    lnkOth.Text = "";
                    div_Oth_Clear.Style.Add("display", "none");
                    div_Oth_upload.Style.Add("display", "block");

                    txt_Currency.Text = ((HiddenField)rowClick.FindControl("hdnUserCurrency")).Value;
                    txt_ContainerNo.Text = ((HiddenField)rowClick.FindControl("hdContainerNo")).Value;

                    SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)) };
                    DataTable dtDetails = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_Post_ShipmentDetails", sp2, DataAccess.Return_Type.DataTable);
                    foreach (DataRow drd in dtDetails.Rows)
                    {
                        txt_InvoiceNo.Text = drd["InvoiceNo"].ToString();
                        txt_Date.Text = drd["InvoiceDate"].ToString();
                        txt_Currency.Text = drd["Currency"].ToString();
                        txt_ExchangeRate.Text = drd["ExchangeRate"].ToString();
                        txt_Blno.Text = drd["BLNo"].ToString();
                        txt_BLDate.Text = drd["BLDate"].ToString();
                        txt_ContainerNo.Text = drd["ContainerNo"].ToString();
                        txt_SBillingNO.Text = drd["ShippingBillNo"].ToString();
                        txt_SBillDate.Text = drd["ShippingBillDate"].ToString();
                        txt_Port.Text = drd["Port"].ToString();
                        btn_Save.Text = "UPDATE";
                        btn_Clear.Style.Add("display", "none");
                        btnMoveStatus.Style.Add("display", "block");
                    }

                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                    DataTable dtPdfFile = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_Post_ShipmentDetails_Document", sp1, DataAccess.Return_Type.DataTable);
                    foreach (DataRow dr in dtPdfFile.Rows)
                    {
                        string[] strSplit = dr["AttachFileName"].ToString().Split('_');
                        if (strSplit[0].ToString() == "IN")
                        {
                            lnkIN.Text = dr["AttachFileName"].ToString();
                            div_IN_Clear.Style.Add("display", "block");
                            div_IN_upload.Style.Add("display", "none");
                        }
                        else if (strSplit[0].ToString() == "PL")
                        {
                            lnkPL.Text = dr["AttachFileName"].ToString();
                            div_PL_Clear.Style.Add("display", "block");
                            div_PL_upload.Style.Add("display", "none");
                        }
                        else if (strSplit[0].ToString() == "BL")
                        {
                            lnkBL.Text = dr["AttachFileName"].ToString();
                            div_BL_Clear.Style.Add("display", "block");
                            div_BL_upload.Style.Add("display", "none");
                        }
                        else if (strSplit[0].ToString() == "ORGIN")
                        {
                            lnkOrigin.Text = dr["AttachFileName"].ToString();
                            div_Origin_Clear.Style.Add("display", "block");
                            div_Origin_upload.Style.Add("display", "none");
                        }
                        else if (strSplit[0].ToString() == "INSUR")
                        {
                            lnkIns.Text = dr["AttachFileName"].ToString();
                            div_Ins_Clear.Style.Add("display", "block");
                            div_Insur_upload.Style.Add("display", "none");
                        }
                        else if (strSplit[0].ToString() == "Others")
                        {
                            lnkOth.Text = dr["AttachFileName"].ToString();
                            div_Oth_Clear.Style.Add("display", "block");
                            div_Oth_upload.Style.Add("display", "none");
                        }
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showUpload", "gotoPreviewDiv('div_Doc_Upload');", true);
                }
                else if (((HiddenField)rowClick.FindControl("hdnStatusID")).Value == "10")
                {
                    txt_TarckNo.Text = "";
                    txt_CourierDate.Text = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                    txt_CourierThrough.Text = "";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showCouier", "gotoPreviewDiv('div_doc_courier');", true);
                }
                else if (((HiddenField)rowClick.FindControl("hdnStatusID")).Value == "11")
                {
                    txt_PaymentReceive.Text = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showPayment", "gotoPreviewDiv('div_payment_received');", true);
                }
                else if (((HiddenField)rowClick.FindControl("hdnStatusID")).Value == "39")
                {
                    txt_ArrivedOn.Text = DateTime.Now.ToString("dd/MM/yyyy").Replace("-", "/");
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showPayment", "gotoPreviewDiv('div_arrived');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderSumValue()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)) };
                DataTable dtSumList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_Order_SumValue_Logistics", sp1, DataAccess.Return_Type.DataTable);
                if (dtSumList.Rows.Count > 0)
                {
                    gv_OrderSumValue.DataSource = dtSumList;
                    gv_OrderSumValue.DataBind();

                    gv_OrderSumValue.FooterRow.Cells[6].Text = "TOTAL";
                    gv_OrderSumValue.FooterRow.Cells[7].Text = dtSumList.Compute("Sum([TOT QTY])", "").ToString();
                    gv_OrderSumValue.FooterRow.Cells[8].Text = dtSumList.Compute("Sum([TOT PRICE])", "").ToString();
                    gv_OrderSumValue.FooterRow.Cells[9].Text = dtSumList.Compute("Sum([TOT WT])", "").ToString();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_Clear_Click(object sender, EventArgs e)
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
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[]{ 
                    new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)),
                    new SqlParameter("@InvoiceNO", Convert.ToString(txt_InvoiceNo.Text)),
                    new SqlParameter("@InvoiceDate", DateTime.ParseExact(txt_Date.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    new SqlParameter("@Currency", Convert.ToString(txt_Currency.Text)),
                    new SqlParameter("@ExchangeRate", Convert.ToDecimal(txt_ExchangeRate.Text)),
                    new SqlParameter("@BLNo", Convert.ToString(txt_Blno.Text)),
                    new SqlParameter("@BLDate", DateTime.ParseExact((txt_BLDate.Text != "" ? txt_BLDate.Text : txt_Date.Text), "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    new SqlParameter("@ContainerNo", Convert.ToString(txt_ContainerNo.Text)),
                    new SqlParameter("@SBillingNo", Convert.ToString(txt_SBillingNO.Text)),
                    new SqlParameter("@SBillDate", DateTime.ParseExact((txt_SBillDate.Text != "" ? txt_SBillDate.Text : txt_Date.Text), "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    new SqlParameter("@Port", Convert.ToString(txt_Port.Text)),
                    new SqlParameter("@UserName", Convert.ToString(Request.Cookies["TTSUser"].Value))
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_Post_ShipmentDetails", sp1);
                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "SaveSuccess", "alert('Record Saved Succesffully');", true);
                    gvDocumentVerifyList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                    Build_SelectOrder(gvDocumentVerifyList.SelectedRow);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            FileUpload fileupload = (FileUpload)btn.FindControl(hdnFileType.Value);
            if (fileupload.HasFile)
            {
                string strInitFile = "";
                if (hdnFileType.Value == "FileUploadControl_IN")
                    strInitFile = "IN_";
                else if (hdnFileType.Value == "FileUploadControl_PL")
                    strInitFile = "PL_";
                else if (hdnFileType.Value == "FileUploadControl_BL")
                    strInitFile = "BL_";
                else if (hdnFileType.Value == "FileUploadControl_ORIGIN")
                    strInitFile = "ORGIN_";
                else if (hdnFileType.Value == "FileUploadControl_Insur")
                    strInitFile = "INSUR_";
                else if (hdnFileType.Value == "FileUploadControl_Oth")
                    strInitFile = "Others_";
                Ins_UploadDocumentDetails(strInitFile, fileupload);
            }
        }
        private void Ins_UploadDocumentDetails(string strFileStart, FileUpload flpCtrl)
        {
            try
            {
                lblErrMsg.Text = "";
                string[] str1 = flpCtrl.FileName.Split('.');
                string strExtension = str1[str1.Length - 1].ToString();
                if (strExtension.ToLower() == "pdf")
                {
                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    if (!Directory.Exists(serverURL + "/PostShipmentDetails/" + hdnCustCode.Value + "/"))
                        Directory.CreateDirectory(serverURL + "/PostShipmentDetails/" + hdnCustCode.Value + "/");
                    string pathToSave = serverURL + "/PostShipmentDetails/" + hdnCustCode.Value + "/" + flpCtrl.FileName;
                    flpCtrl.SaveAs(pathToSave);

                    Directory.Move(pathToSave, serverURL + "/PostShipmentDetails/" + hdnCustCode.Value + "/" + strFileStart +
                        lblSelectedOrderRefNo.Text + "_" + hdnPlant.Value + ".pdf");

                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@OID", hdnOID.Value);
                    sp1[1] = new SqlParameter("@AttachFileName", strFileStart + lblSelectedOrderRefNo.Text + "_" + hdnPlant.Value + ".pdf");
                    sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                    sp1[3] = new SqlParameter("@FileType", "POST SHIPMENT DOCUMENTS");
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);
                    if (resp > 0)
                    {
                        gvDocumentVerifyList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                        Build_SelectOrder(gvDocumentVerifyList.SelectedRow);
                    }
                }
                else
                    lblErrMsg.Text = "Kindly upload pdf format file";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                LinkButton lnkCtrl = (LinkButton)btn.FindControl("lnk" + btn.ID.Replace("btnDelete", ""));

                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/PostShipmentDetails/" + hdnCustCode.Value + "/"))
                    Directory.CreateDirectory(serverURL + "/PostShipmentDetails/" + hdnCustCode.Value + "/");
                string pathToSave = serverURL + "/PostShipmentDetails/" + hdnCustCode.Value + "/" + lnkCtrl.Text;
                FileInfo file1 = new FileInfo(pathToSave);
                if (file1.Exists)
                {
                    File.Delete(pathToSave);
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)), new SqlParameter("@AttachFileName", lnkCtrl.Text) };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_Del_PostShipmentFiles", sp1);

                    if (resp > 0)
                    {
                        gvDocumentVerifyList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                        Build_SelectOrder(gvDocumentVerifyList.SelectedRow);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnlPdfDownload_Click(object sender, EventArgs e)
        {
            LinkButton lnkTxt = sender as LinkButton;
            string path = Server.MapPath("~/PostShipmentDetails/" + hdnCustCode.Value + "/").Replace("TTS", "pdfs") + lnkTxt.Text;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
            Response.WriteFile(path);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        protected void btnMoveStatus_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Convert.ToInt32( hdnOID.Value)), 
                    new SqlParameter("@statusid", Convert.ToInt32(10)), 
                    new SqlParameter("@feedback", "MOVE TO COURIER CONFIRMATION"), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                };
                int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                if (resp1 > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCourier_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spcourier = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)),
                    new SqlParameter("@CourierTrackNo", txt_TarckNo.Text), 
                    new SqlParameter("@CourieredDate", DateTime.ParseExact(txt_CourierDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                    new SqlParameter("@CourierThrough", txt_CourierThrough.Text)
                };
                daCOTS.ExecuteNonQuery_SP("sp_upd_ShipmentDetails_CourierDetails", spcourier);

                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)), 
                    new SqlParameter("@statusid", Convert.ToInt32(11)), 
                    new SqlParameter("@feedback", "MOVE TO PAYMENT RECEIVED CONFIRMATION"), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                };
                int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                if (resp1 > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPaymentSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spcourier = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value),
                    new SqlParameter("@PaymentReceivedOn", DateTime.ParseExact(txt_PaymentReceive.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                };
                daCOTS.ExecuteNonQuery_SP("sp_upd_ShipmentDetails_paymentreceive", spcourier);

                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)), 
                    new SqlParameter("@statusid", Convert.ToInt32(39)), 
                    new SqlParameter("@feedback", "PAYMENT RECEIVED. MOVE TO CONTAINER DELIVERED CONFIRMATION"), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                };
                int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                if (resp1 > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDelivered_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)), 
                    new SqlParameter("@statusid", Convert.ToInt32(5)), 
                    new SqlParameter("@feedback", "DELIVERED AT DESTINATION"), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                };
                int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                if (resp1 > 0)
                {
                    SqlParameter[] sparrive = new SqlParameter[] { 
                        new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)), 
                        new SqlParameter("@dispatchedon", DateTime.ParseExact(txt_ArrivedOn.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)) 
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_upd_order_Dispatched", sparrive);
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}