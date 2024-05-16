﻿using System;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services;
using System.Web.Script.Serialization;
using Mail_Sender;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TTS
{
    public partial class cotsorderprocess2 : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dom_proforma"].ToString() == "True" ||
                            dtUser.Rows[0]["dom_invoice"].ToString() == "True" || dtUser.Rows[0]["dom_paymentconfirm"].ToString() == "True"))
                        {
                            hdnQstring.Value = Utilities.Decrypt(Request["qstring"].ToString());
                            if (hdnQstring.Value == "proforma" || hdnQstring.Value == "workorder" || hdnQstring.Value == "invoice" || hdnQstring.Value == "payment"
                                || hdnQstring.Value == "tcs")
                            {
                                if (hdnQstring.Value == "proforma")
                                {
                                    DataTable dtgv1 = new DataTable();
                                    dtgv1.Columns.Add("slno", typeof(String));
                                    for (int i = 1; i <= 3; i++)
                                        dtgv1.Rows.Add(i);
                                    gvAmountSub.DataSource = dtgv1;
                                    gvAmountSub.DataBind();
                                    //txtClaimAdjustment.Text = "";
                                    //txtLESSAMT.Text = "";
                                    //txtotherdiscount.Text = "";
                                    //txtOtherDisAmt.Text = "";
                                }
                                else if (hdnQstring.Value == "tcs")
                                {
                                    rdbTcsAmt.SelectedIndex = 0;
                                    rdbTcsPan.SelectedIndex = 0;
                                    SqlParameter[] spT = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) };
                                    DataTable dtTcs = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_TcsTax_Status", spT, DataAccess.Return_Type.DataTable);
                                    if (dtTcs.Rows.Count > 0)
                                    {
                                        if (Convert.ToBoolean(dtTcs.Rows[0]["tcsApplicable"].ToString()))
                                        {
                                            rdbTcsAmt.SelectedIndex = 1;
                                            if (Convert.ToBoolean(dtTcs.Rows[0]["validPan"].ToString()))
                                                rdbTcsPan.SelectedIndex = 1;
                                        }
                                    }
                                }
                                rdbModeOfTransport.SelectedIndex = -1;

                                BuildOrderMasterDetails();
                                BuildOrderItemDetails();
                                Bind_OrderOtherDetails();

                                LabelCheck();

                                Bind_Attach_PdfFiles();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL IS WRONG !!!";
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "USER PRIVILEGE DISABLED. PLEASE CONTACT ADMINISTRATOR !!!";
                        }
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoBottomDiv('MainContent_btnClearOrder');", true);
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
        private void BuildOrderMasterDetails()
        {
            try
            {
                DataTable dtMasterList = DomesticScots.Bind_OrderMasterDetails(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())));
                if (dtMasterList.Rows.Count > 0)
                {
                    hdnCustCode.Value = dtMasterList.Rows[0]["custcode"].ToString();
                    hdnOrderRefNo.Value = dtMasterList.Rows[0]["orderrefno"].ToString();

                    DataTable dtHsn = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_HsnCodeMaster", DataAccess.Return_Type.DataTable);
                    ViewState["dtHsnVal"] = dtHsn;

                    hdnPartNo.Value = dtMasterList.Rows[0]["specialdesc"].ToString();
                    hdnorderPlant.Value = dtMasterList.Rows[0]["Plant"].ToString();
                    hdnCustCategory.Value = dtMasterList.Rows[0]["CustCategory"].ToString();
                    if (dtMasterList.Rows[0]["ModeOfTransport"].ToString() != "")
                    {
                        System.Web.UI.WebControls.ListItem itemTransport = rdbModeOfTransport.Items.FindByText(dtMasterList.Rows[0]["ModeOfTransport"].ToString().ToUpper());
                        if (itemTransport != null) itemTransport.Selected = true;
                    }
                    txtCGST.Text = (dtMasterList.Rows[0]["CGST"].ToString() != "" && Convert.ToDecimal(dtMasterList.Rows[0]["CGST"].ToString()) > 0) ?
                        Convert.ToDecimal(dtMasterList.Rows[0]["CGST"].ToString()).ToString("0.00") : "";
                    txtSGST.Text = (dtMasterList.Rows[0]["SGST"].ToString() != "" && Convert.ToDecimal(dtMasterList.Rows[0]["SGST"].ToString()) > 0) ?
                        Convert.ToDecimal(dtMasterList.Rows[0]["SGST"].ToString()).ToString("0.00") : "";
                    txtIGST.Text = (dtMasterList.Rows[0]["IGST"].ToString() != "" && Convert.ToDecimal(dtMasterList.Rows[0]["IGST"].ToString()) > 0) ?
                        Convert.ToDecimal(dtMasterList.Rows[0]["IGST"].ToString()).ToString("0.00") : "";
                    if (txtCGST.Text != "") chkCGST.Checked = true;
                    if (txtSGST.Text != "") chkSGST.Checked = true;
                    if (txtIGST.Text != "") chkIGST.Checked = true;
                    lblModeOfTransport.Text = dtMasterList.Rows[0]["ModeOfTransport"].ToString();
                    hdnStatusID.Value = dtMasterList.Rows[0]["OrderStatus"].ToString();
                    hdnFreightChanges.Value = dtMasterList.Rows[0]["FreightCharges"].ToString();
                    txtPayterms.Text = Utilities.Decrypt(Request["qstring"].ToString()) == "proforma" ?
                        Server.HtmlDecode(dtMasterList.Rows[0]["PaymentTerms"].ToString().Replace("~", "\r\n")) : "";
                    frmOrderMasterDetails.DataSource = dtMasterList;
                    frmOrderMasterDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void LabelCheck()
        {
            try
            {
                switch (Utilities.Decrypt(Request["qstring"].ToString()))
                {
                    case "proforma":
                        {
                            lblpageHeading.Text = "Order Process - Proforma Preparation";
                            SqlParameter[] spDummy = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                            DataTable dummyProformaRefno = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_Dummy_ProformaRefNo", spDummy, DataAccess.Return_Type.DataTable);
                            if (dummyProformaRefno != null && dummyProformaRefno.Rows.Count > 0)
                            {
                                txtProformaRefNo.Text = Convert.ToDecimal(dummyProformaRefno.Rows[0]["proformarefno"].ToString()).ToString("00000");
                                txtProformaReviseNo.Text = Convert.ToDecimal(Convert.ToDecimal(dummyProformaRefno.Rows[0]["revise_no"].ToString())).ToString("00");
                            }
                            break;
                        }
                    case "workorder":
                        {
                            lblpageHeading.Text = "Order Process - WorkOrder Preparation";
                            SqlParameter[] spDummy = new SqlParameter[] { 
                                new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                                new SqlParameter("@Plant", hdnorderPlant.Value) 
                            };
                            DataTable dummyProformaRefno = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_Dummy_WorkOrderNo", spDummy, DataAccess.Return_Type.DataTable);
                            if (dummyProformaRefno != null && dummyProformaRefno.Rows.Count > 0)
                            {
                                txtWorkOrderNo.Text = Convert.ToDecimal(dummyProformaRefno.Rows[0]["workorderno"].ToString()).ToString("00000");
                                txtWorkorderReviseNo.Text = Convert.ToDecimal(Convert.ToDecimal(dummyProformaRefno.Rows[0]["revise_no"].ToString())).ToString("00");
                            }
                            break;
                        }
                    case "invoice":
                        {
                            lblpageHeading.Text = "Order Process - Invoice Preparation";
                            SqlParameter[] spAdd = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                            DataSet dsAdd = (DataSet)daCOTS.ExecuteReader_SP("SP_SEL_cotsInvoicePrepare_TaxAndDiscount_v1", spAdd, DataAccess.Return_Type.DataSet);
                            string strRndrEle = "";
                            if (dsAdd.Tables[0].Rows.Count > 0)
                            {
                                if (dsAdd.Tables[0].Rows[0]["CGST"].ToString() != "" && Convert.ToDecimal(dsAdd.Tables[0].Rows[0]["CGST"].ToString()) > 0)
                                    strRndrEle += "CGST % : " + Convert.ToDecimal(dsAdd.Tables[0].Rows[0]["CGST"].ToString()).ToString("00.00");
                                if (dsAdd.Tables[0].Rows[0]["SGST"].ToString() != "" && Convert.ToDecimal(dsAdd.Tables[0].Rows[0]["SGST"].ToString()) > 0)
                                    strRndrEle += (strRndrEle == "" ? "" : "<br/>") + "SGST % : " + Convert.ToDecimal(dsAdd.Tables[0].Rows[0]["SGST"].ToString()).ToString("00.00");
                                if (dsAdd.Tables[0].Rows[0]["IGST"].ToString() != "" && Convert.ToDecimal(dsAdd.Tables[0].Rows[0]["IGST"].ToString()) > 0)
                                    strRndrEle += (strRndrEle == "" ? "" : "<br/>") + "IGST % : " + Convert.ToDecimal(dsAdd.Tables[0].Rows[0]["IGST"].ToString()).ToString("00.00");
                                if (Convert.ToBoolean(dsAdd.Tables[0].Rows[0]["tcsApplicable"].ToString()))
                                {
                                    if (Convert.ToBoolean(dsAdd.Tables[0].Rows[0]["validPan"].ToString()))
                                        strRndrEle += (strRndrEle == "" ? "" : "<br/>") + "TCS % : 0.075";
                                    else
                                        strRndrEle += (strRndrEle == "" ? "" : "<br/>") + "TCS % : 0.75";
                                }
                                if ((Convert.ToDecimal(dsAdd.Tables[0].Rows[0]["CGST"].ToString()) == 0 && Convert.ToDecimal(dsAdd.Tables[0].Rows[0]["SGST"].ToString()) == 0 &&
                                    Convert.ToDecimal(dsAdd.Tables[0].Rows[0]["IGST"].ToString()) == 0) || hdnCustCode.Value == "2652" ||
                                    dsAdd.Tables[2].Rows[0]["GST_No"].ToString() == "NA")
                                {
                                    lblIrn.Text = "NA";
                                    if (hdnShipID.Value == "960" || hdnShipID.Value == "4594" || hdnShipID.Value == "8016" )
                                        lblIrn.Text = "";
                                }
                            }
                            if (dsAdd.Tables[1].Rows.Count > 0)
                            {
                                strRndrEle += (strRndrEle == "" ? "" : "<br/>");
                                for (int i = 0; i < dsAdd.Tables[1].Rows.Count; i++)
                                {
                                    if (dsAdd.Tables[1].Rows[i]["DescriptionDiscount"].ToString() != "")
                                        strRndrEle += "<br/><span>" + dsAdd.Tables[1].Rows[i]["Category"].ToString() + "  " +
                                            dsAdd.Tables[1].Rows[i]["DescriptionDiscount"].ToString() +
                                            "  :  " + Convert.ToDecimal(dsAdd.Tables[1].Rows[i]["Amount"].ToString()).ToString("00000.00") + "</span>";
                                }
                            }
                            div_addtionalCharge.InnerHtml = strRndrEle;

                            SqlParameter[] spDummy = new SqlParameter[] { new SqlParameter("@CustCode", hdnCustCode.Value), new SqlParameter("@Plant", hdnorderPlant.Value) };
                            string dummyInvoice = (string)daCOTS.ExecuteScalar_SP("Sp_Sel_Dummy_InvoiceNo_v1", spDummy);//Sp_Sel_Dummy_InvoiceNo
                            if (dummyInvoice != "")
                                lblInvoiceNo.Text = dummyInvoice;

                            //Assign Values to Default fields                                
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_invoicedetails", sp, DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                lblInvoiceNo.Text = dt.Rows[0]["invoiceno"].ToString();
                                lblInvoiceDate.Text = dt.Rows[0]["createdate"].ToString();
                                lblTransportName.Text = dt.Rows[0]["despatch"].ToString();
                                lblContactPerson.Text = dt.Rows[0]["ContactName"].ToString();
                                lblContactNo.Text = dt.Rows[0]["ContactNo"].ToString();
                                txtLRno.Text = dt.Rows[0]["llrno"].ToString();
                                txtvehicleNo.Text = dt.Rows[0]["VehicleNo"].ToString();
                                lblIrn.Text = dt.Rows[0]["Irn"].ToString();
                                hdnIrnQrCode.Value = dt.Rows[0]["SignedQRCode"].ToString();
                                lblAckNo.Text = dt.Rows[0]["AckNo"].ToString();
                                lblAckDate.Text = dt.Rows[0]["AckDt"].ToString();
                            }

                            break;
                        }
                    case "payment":
                        {
                            lblpageHeading.Text = "Order Process - Payment Confirmation";
                            btnMoveOrder.Text = "Move To PDI";
                            break;
                        }
                    case "tcs":
                        {
                            lblpageHeading.Text = "Order Process - Confirmation Of Tcs";
                            btnMoveOrder.Text = "Save & Move To Proforma Prepare";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public string Bind_BillingAddress(string BillID, bool isShipAddress)
        {
            try
            {
                string strAddress = string.Empty;
                DataTable dtAddressList = DomesticScots.Bind_BillingAddress(BillID);
                if (dtAddressList.Rows.Count > 0)
                {
                    DataRow row = dtAddressList.Rows[0];
                    strAddress += "<div style='font-weight:bold;'>M/S. " + row["CompanyName"].ToString() + "</div>";
                    strAddress += "<div>" + row["shipaddress"].ToString().Replace("~", "<br/>") + "</div>";
                    strAddress += "<div>" + row["city"].ToString() + ", " + row["statename"].ToString() + "</div>";
                    strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
                    strAddress += "<div>" + row["contact_name"].ToString() + "</div>";
                    strAddress += "<div>" + row["EmailID"].ToString() + " / " + row["mobile"].ToString() + "</div>";
                    strAddress += "<div>GST: " + row["GST_No"].ToString() + "</div>";
                    lblContactPerson.Text = row["contact_name"].ToString();
                    lblContactNo.Text = row["mobile"].ToString();
                    if (isShipAddress == true)
                    {
                        txtCGST.Text = (txtCGST.Text != "" && Convert.ToDecimal(txtCGST.Text) <= 0) ? row["CGST"].ToString().Trim() == "" ? "0.00" : row["CGST"].ToString().Trim() : txtCGST.Text;
                        txtSGST.Text = (txtSGST.Text != "" && Convert.ToDecimal(txtSGST.Text) <= 0) ? row["SGST"].ToString().Trim() == "" ? "0.00" : row["SGST"].ToString().Trim() : txtSGST.Text;
                        txtIGST.Text = (txtIGST.Text != "" && Convert.ToDecimal(txtIGST.Text) <= 0) ? row["IGST"].ToString().Trim() == "" ? "0.00" : row["IGST"].ToString().Trim() : txtIGST.Text;
                        hdnShipID.Value = BillID;
                    }
                }
                return strAddress;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        private void BuildOrderItemDetails()
        {
            try
            {
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())));
                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    ViewState["dtItemList"] = dtItemList;

                    gvOrderItemList.FooterRow.Cells[6].Text = "TOTAL";
                    gvOrderItemList.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    object sumCost = dtItemList.Compute("Sum(unitprice)", "");
                    gvOrderItemList.FooterRow.Cells[7].Text = Convert.ToDecimal(sumCost).ToString();

                    object sumQty = dtItemList.Compute("Sum(itemqty)", "");
                    gvOrderItemList.FooterRow.Cells[8].Text = sumQty.ToString();

                    object sumFwt = dtItemList.Compute("Sum(finishedwt)", "");
                    gvOrderItemList.FooterRow.Cells[9].Text = Convert.ToDecimal(sumFwt).ToString();

                    gvOrderItemList.Columns[10].Visible = false; //RIM QTY
                    gvOrderItemList.Columns[11].Visible = false; //RIM BASIC PRICE
                    gvOrderItemList.Columns[12].Visible = false; //RIM FWT
                    gvOrderItemList.Columns[13].Visible = false; //DWG NO.
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[10].Visible = true;
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;
                        gvOrderItemList.Columns[13].Visible = true;
                        break;
                    }
                    gvOrderItemList.Columns[14].Visible = false; //PART NO

                    chkSpecialOffer.Visible = false; chkSpecialOffer.Enabled = true; chkSpecialOffer.Checked = false;
                    if (Utilities.Decrypt(Request["qstring"].ToString()) == "proforma")
                    {
                        if (hdnPartNo.Value == "PartNo")
                            gvOrderItemList.Columns[14].Visible = true;

                        DataTable dtSplOffer = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_DomesticOfferDetails", DataAccess.Return_Type.DataTable);
                        if (dtSplOffer.Rows.Count > 0)
                        {
                            string strSplBrand = dtSplOffer.Rows[0]["Brand"].ToString();
                            string strSplType = dtSplOffer.Rows[0]["Tyretype"].ToString();
                            var splQty = dtItemList.AsEnumerable().Where(b => b.Field<string>("Brand").Equals(strSplBrand) &&
                                                b.Field<string>("Tyretype").StartsWith(strSplType)).Sum(A => A.Field<int>("itemqty"));

                            if (splQty >= Convert.ToInt32(dtSplOffer.Rows[0]["MinQty"].ToString()))
                            {
                                chkSpecialOffer.Visible = true;
                                chkSpecialOffer.Text = "SPECIAL PRICE AVAILABLE FOR " + strSplBrand + " " + strSplType;
                            }

                            SqlParameter[] spSpl = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                            DataTable dtsplStatus = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_SplOfferStatus", spSpl, DataAccess.Return_Type.DataTable);
                            if (dtsplStatus.Rows.Count > 0 && dtsplStatus.Rows[0]["SplOffer"].ToString() == "1")
                            {
                                chkSpecialOffer.Checked = true;
                                chkSpecialOffer.Enabled = false;
                            }
                        }
                    }
                    else if (Utilities.Decrypt(Request["qstring"].ToString()) == "workorder")
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
        private void Bind_OrderOtherDetails()
        {
            try
            {
                if (Utilities.Decrypt(Request["qstring"].ToString()) == "proforma")
                {
                    SqlParameter[] spdiscount = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                    DataTable dtdiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_Discount", spdiscount, DataAccess.Return_Type.DataTable);
                    if (dtdiscount.Rows.Count > 0)
                    {
                        int gvRow = 0;
                        foreach (DataRow disRow in dtdiscount.Select("Discounttype='OTHER CHARGES'"))
                        {
                            TextBox txtAddDesc = gvAmountSub.Rows[gvRow].FindControl("txtAddDesc") as TextBox;
                            TextBox txtCAddAmt = gvAmountSub.Rows[gvRow].FindControl("txtCAddAmt") as TextBox;
                            txtAddDesc.Text = disRow["DescriptionDiscount"].ToString();
                            txtCAddAmt.Text = disRow["Amount"].ToString();
                            gvRow++;
                        }
                        //foreach (DataRow disRow in dtdiscount.Select("Discounttype='CLAIM ADJUSTMENT'"))
                        //{
                        //    txtClaimAdjustment.Text = disRow["DescriptionDiscount"].ToString();
                        //    txtLESSAMT.Text = disRow["Amount"].ToString();
                        //}
                        //foreach (DataRow disRow in dtdiscount.Select("Discounttype='OTHER DISCOUNT'"))
                        //{
                        //    txtotherdiscount.Text = disRow["DescriptionDiscount"].ToString();
                        //    txtOtherDisAmt.Text = disRow["Amount"].ToString();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Attach_PdfFiles()
        {
            try
            {
                bool boolPreparestatus = false;
                int lrfilestatus = 0;
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
                    new SqlParameter("@Qstring", Utilities.Decrypt(Request["qstring"].ToString())) 
                };
                DataTable dtPdfFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdf_AttachInvoiceFiles", sp1, DataAccess.Return_Type.DataTable);
                if (dtPdfFile != null && dtPdfFile.Rows.Count > 0)
                {
                    boolPreparestatus = true;
                    gv_DownloadFiles.DataSource = dtPdfFile;
                    gv_DownloadFiles.DataBind();
                    lbl_downloadMessage.Text = "Download PDF Files";
                    switch (Utilities.Decrypt(Request["qstring"].ToString()))
                    {
                        case "proforma":
                            if (hdnStatusID.Value == "1")
                                lblEmailsendMessage.Text = "Mail to Customer";
                            else if (hdnStatusID.Value == "2")
                                btnMoveOrder.Text = "Move to workorder prepare";
                            break;
                        case "workorder":
                            {
                                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_vs2_WorkOrderDetails", sp, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    txtExpectedShippingDate.Text = ((Label)frmOrderMasterDetails.FindControl("lblExpectedDate")).Text.Replace("-", "/");
                                    txtWorkOrderNo.Text = dt.Rows[0]["workorderno"].ToString();
                                    if (dt.Rows[0]["deliverytype"].ToString() != "Immediate" && dt.Rows[0]["deliverytype"].ToString() != "Normal")
                                        ddl_deliverypriority.Items.FindByValue(dt.Rows[0]["deliverytype"].ToString()).Selected = true;
                                    DataView view = new DataView(dt);
                                    DataTable dtfiltered = view.ToTable(false, "1", "2", "3", "4", "5", "6");
                                    for (int i = 0; i < 6; i++)
                                    {
                                        if (Convert.ToBoolean(dtfiltered.Rows[0][i].ToString()))
                                            chk_preDispatch.Items[i].Selected = true;
                                    }
                                }
                                if (dtPdfFile.Rows.Count == 1)
                                    boolPreparestatus = false;
                                else
                                    btnMoveOrder.Text = "Move to Assign Stencil";
                                break;
                            }
                        case "invoice":
                            {
                                foreach (DataRow aRow in dtPdfFile.Select("FileType='UPLOAD LR COPY'"))
                                {
                                    lrfilestatus = 1;
                                }
                                if (dtPdfFile.Rows.Count > 3)
                                {
                                    if (hdnStatusID.Value == "21")
                                        btnMoveOrder.Text = "Move to Dispatched";
                                    else if (hdnStatusID.Value == "7")
                                        btnMoveOrder.Text = "Move to Vehicle Load";
                                }
                                else if (dtPdfFile.Rows.Count <= 3)
                                    boolPreparestatus = false;

                                if (!boolPreparestatus)
                                {
                                    if (lblIrn.Text == "" && lblIrn.Text.Length == 0 && lblIrn.Text != "NA")
                                        lblPrepareMessage.Text = "Generate IRN";
                                    else
                                        lblPrepareMessage.Text = "Prepare invoice";
                                }
                                else if (boolPreparestatus)
                                {
                                    lblPrepareMessage.Text = "Invoice prepared";
                                    lblPrepareMessage.Style.Add("color", "#c3c305");
                                }
                                break;
                            }
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jtostatuschk2", "CtrlEnable('" +
                    Utilities.Decrypt(Request["qstring"].ToString()) + "','" + hdnStatusID.Value + "','" + boolPreparestatus.ToString().ToLower() + "','" +
                    lrfilestatus + "','" + (((HiddenField)frmOrderMasterDetails.FindControl("hdnCustHoldStatus")).Value).ToLower() + "');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void imgPrepare_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                switch (Utilities.Decrypt(Request["qstring"].ToString()))
                {
                    case "proforma":
                        ProformaDocPreparation();
                        break;
                    case "workorder":
                        WorkOrderDocPreparation();
                        break;
                    case "invoice":
                        {
                            //Create a Orginal Invoice no
                            SqlParameter[] spIns = new SqlParameter[] { 
                                new SqlParameter("@CustCode", hdnCustCode.Value), 
                                new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
                                new SqlParameter("@Plant", hdnorderPlant.Value), 
                                new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                                new SqlParameter("@Irn", lblIrn.Text)
                            };
                            int resp = daCOTS.ExecuteNonQuery_SP("Sp_create_InvoiceNo_v2", spIns);//Sp_create_InvoiceNo_v1
                            if (resp > 0)
                            {
                                if (lblPrepareMessage.Text == "Generate IRN")
                                    IrnNoPreparation();
                                else if (lblPrepareMessage.Text == "Prepare invoice" || lblPrepareMessage.Text == "Invoice prepared")
                                    InvoiceDocPreparation();
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void ProformaDocPreparation()
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[5];
                sp2[0] = new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString()));
                sp2[1] = new SqlParameter("@proformarefno", txtProformaRefNo.Text);
                sp2[2] = new SqlParameter("@payterms", txtPayterms.Text.Replace("\r\n", "~"));
                sp2[3] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                sp2[4] = new SqlParameter("@revise_no", txtProformaReviseNo.Text);
                daCOTS.ExecuteNonQuery_SP("sp_ins_ProformaCreateDetails_domestic", sp2);

                AddtionalCharge();

                if (hdnPartNo.Value == "PartNo")
                    PartNo_Insert();

                string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/proformafiles/" + hdnCustCode.Value + "/"))
                    Directory.CreateDirectory(serverURL + "/proformafiles/" + hdnCustCode.Value + "/");
                string path = serverURL + "/proformafiles/" + hdnCustCode.Value + "/";
                string sPathToWritePdfTo = path + hdnOrderRefNo.Value + ".pdf";

                Document document = new Document(PageSize.A4, 18f, 2f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sPathToWritePdfTo, FileMode.Create));
                writer.PageEvent = new PDFWriterEvents("PROFORMA INVOICE");
                document.Open();
                InvoiceDataModel idm = Build_ProformaDataModel();
                document.Add(PrepareInvoice.Prepare(document, idm));
                document.Close();

                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString()));
                sp1[1] = new SqlParameter("@AttachFileName", hdnOrderRefNo.Value + ".pdf");
                sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp1[3] = new SqlParameter("@FileType", "PROFORMA FILE");
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);
                if (resp > 0)
                    Response.Redirect(Request.RawUrl.ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private InvoiceDataModel Build_ProformaDataModel()
        {
            InvoiceDataModel iDM = new InvoiceDataModel();
            iDM.InvoiceHead = "PROFORMA INVOICE";
            iDM.Plant = hdnorderPlant.Value;
            iDM.Category = hdnCustCategory.Value;
            iDM.LogoPath = Server.MapPath("~/images/tvs_suntws.jpg");
            iDM.CustCode = hdnCustCode.Value;
            iDM.IsProfoma = true;
            iDM.RefNo = txtProformaRefNo.Text;
            iDM.OrderRefNo = hdnOrderRefNo.Value;
            iDM.ModeOfTransport = rdbModeOfTransport.SelectedItem.Text;
            iDM.ReviseCount = Convert.ToInt32(txtProformaReviseNo.Text);
            iDM.OrderDate = ((Label)frmOrderMasterDetails.FindControl("lblOrderDate")).Text;
            iDM.TransporterName = ((Label)frmOrderMasterDetails.FindControl("lblTransportDetails")).Text;
            iDM.FreightCharges = ((Label)frmOrderMasterDetails.FindControl("lblFreightCharges")).Text;
            iDM.CcAttached = (((Label)frmOrderMasterDetails.FindControl("lblDeliveryMethod")).Text) +
                (((Label)frmOrderMasterDetails.FindControl("lblGodownName")).Text != "" ? "\n( " + ((Label)frmOrderMasterDetails.FindControl("lblGodownName")).Text + " )" : "");
            iDM.CreditNote = ((HiddenField)frmOrderMasterDetails.FindControl("hdnCreditNote")).Value;
            iDM.Paymentdays = ((HiddenField)frmOrderMasterDetails.FindControl("hdnPaymentdays")).Value != "" ? ((HiddenField)frmOrderMasterDetails.FindControl("hdnPaymentdays")).Value : "0";
            iDM.dtHsnCode = ViewState["dtHsnVal"] as DataTable;
            iDM.PartNo = hdnPartNo.Value == "PartNo" ? true : false;
            iDM.OID = Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString()));
            return iDM;
        }
        private void AddtionalCharge()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                daCOTS.ExecuteNonQuery_SP("sp_del_Export_Discount", sp);

                //insert other charges
                for (int i = 0; i < gvAmountSub.Rows.Count; i++)
                {
                    string DESC = ((TextBox)gvAmountSub.Rows[i].FindControl("txtAddDesc")).Text.Trim();
                    string AMT = ((TextBox)gvAmountSub.Rows[i].FindControl("txtCAddAmt")).Text.Trim();
                    string category = ((Label)gvAmountSub.Rows[i].FindControl("lblplus")).ToolTip.Trim();
                    if (DESC != "" && AMT != "")
                    {
                        SqlParameter[] sp2 = new SqlParameter[] { 
                            new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                            new SqlParameter("@Category", category), 
                            new SqlParameter("@DescriptionDiscount", DESC), 
                            new SqlParameter("@Amount", AMT), 
                            new SqlParameter("@username", Request.Cookies["TTSUser"].Value), 
                            new SqlParameter("@discountType", "OTHER CHARGES") 
                        };
                        daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp2);
                    }
                }
                //insert Claim Adjusement
                //if (txtClaimAdjustment.Text != "" && txtLESSAMT.Text != "")
                //{
                //    SqlParameter[] sp3 = new SqlParameter[] { 
                //        new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                //        new SqlParameter("@Category", lblLESSclaimAdjus.ToolTip), 
                //        new SqlParameter("@DescriptionDiscount", txtClaimAdjustment.Text), 
                //        new SqlParameter("@Amount", txtLESSAMT.Text), 
                //        new SqlParameter("@username", Request.Cookies["TTSUser"].Value), 
                //        new SqlParameter("@discountType", "CLAIM ADJUSTMENT") 
                //    };
                //    daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp3);
                //}
                //insert Discount Details
                //if (txtotherdiscount.Text != "" && txtOtherDisAmt.Text != "")
                //{
                //    SqlParameter[] sp4 = new SqlParameter[] { 
                //        new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                //        new SqlParameter("@Category", lblLessdiscount.Text), 
                //        new SqlParameter("@DescriptionDiscount", txtotherdiscount.Text), 
                //        new SqlParameter("@Amount", txtOtherDisAmt.Text), 
                //        new SqlParameter("@username", Request.Cookies["TTSUser"].Value), 
                //        new SqlParameter("@discountType", "OTHER DISCOUNT") 
                //    };
                //    daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp4);
                //}
                //insert GST Details
                if (chkCGST.Checked || chkSGST.Checked || chkIGST.Checked)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                        new SqlParameter("@CGST", chkCGST.Checked == true ? Convert.ToDecimal(txtCGST.Text) : Convert.ToDecimal("0.00")), 
                        new SqlParameter("@SGST", chkSGST.Checked == true ? Convert.ToDecimal(txtSGST.Text) : Convert.ToDecimal("0.00")), 
                        new SqlParameter("@IGST", chkIGST.Checked == true ? Convert.ToDecimal(txtIGST.Text) : Convert.ToDecimal("0.00")), 
                        new SqlParameter("@ExporterAddressID", Convert.ToInt32("0")), 
                        new SqlParameter("@ModeOfTransport", rdbModeOfTransport.SelectedItem.Text) 
                    };
                    daCOTS.ExecuteNonQuery_SP("SP_UPD_CotsProfomasent_GST", sp1);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void PartNo_Insert()
        {
            try
            {
                DataTable dtItemList = (DataTable)ViewState["dtItemList"];
                for (int i = 0; i < gvOrderItemList.Rows.Count; i++)
                {
                    string ItemType = dtItemList.Rows[i]["AssyRimstatus"].ToString() == "True" ? "ASSY" : dtItemList.Rows[i]["category"].ToString() == "SPLIT RIMS" || dtItemList.Rows[i]["category"].ToString() == "POB WHEEL" ? "RIM" : "TYRE";
                    string TyreSize = dtItemList.Rows[i]["tyresize"].ToString();
                    string Rimsize = dtItemList.Rows[i]["rimsize"].ToString();
                    string tyretype = dtItemList.Rows[i]["category"].ToString() == "SPLIT RIMS" || dtItemList.Rows[i]["category"].ToString() == "POB WHEEL" ? dtItemList.Rows[i]["category"].ToString() : dtItemList.Rows[i]["tyretype"].ToString();
                    string ItemCode = ((TextBox)gvOrderItemList.Rows[i].FindControl("txtPartNo")).Text.Trim();
                    if (ItemType != "" && TyreSize != "" && Rimsize != "" && tyretype != "" && ItemCode != "")
                    {
                        SqlParameter[] sp2 = new SqlParameter[7];
                        sp2[0] = new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString()));
                        sp2[1] = new SqlParameter("@TyreType", tyretype);
                        sp2[2] = new SqlParameter("@TyreSize", TyreSize);
                        sp2[3] = new SqlParameter("@Rimsize", Rimsize);
                        sp2[4] = new SqlParameter("@ItemCode", ItemCode);
                        sp2[5] = new SqlParameter("@ItemType", ItemType);
                        sp2[6] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                        daCOTS.ExecuteNonQuery_SP("sp_ins_Customer_PartNo", sp2);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void chkSpecialOffer_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value) 
                };
                daCOTS.ExecuteNonQuery_SP("sp_update_DomesticOfferPrice", sp1);
                BuildOrderItemDetails();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void WorkOrderDocPreparation()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[3];
                sp[0] = new SqlParameter("@ExpectedShipDate", DateTime.ParseExact(txtExpectedShippingDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                sp[1] = new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString()));
                sp[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_ExpectedShipDate", sp);

                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
                    new SqlParameter("@workorderno", txtWorkOrderNo.Text), 
                    new SqlParameter("@deliverytype", ddl_deliverypriority.SelectedItem.Text), 
                    new SqlParameter("@category1", chk_preDispatch.Items[0].Selected), 
                    new SqlParameter("@category2", chk_preDispatch.Items[1].Selected), 
                    new SqlParameter("@category3", chk_preDispatch.Items[2].Selected), 
                    new SqlParameter("@category4", chk_preDispatch.Items[3].Selected), 
                    new SqlParameter("@category5", chk_preDispatch.Items[4].Selected), 
                    new SqlParameter("@category6", chk_preDispatch.Items[5].Selected), 
                    new SqlParameter("@splRemarks", txtcomments.Text), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value), 
                    new SqlParameter("@revise_no", txtWorkorderReviseNo.Text)
                };
                int resp2 = daCOTS.ExecuteNonQuery_SP("sp_ins_workorderdetails_domestic", sp1);

                if (resp > 0 && resp2 > 0)
                {
                    Pdf_WorkOrderDetails.CustCode = hdnCustCode.Value;
                    Pdf_WorkOrderDetails.OrderRefNo = hdnOrderRefNo.Value;
                    Pdf_WorkOrderDetails.dtItemList = (DataTable)ViewState["dtItemList"];
                    Pdf_WorkOrderDetails.CustomerName = ((Label)frmOrderMasterDetails.FindControl("lblCustomerName")).Text;
                    Pdf_WorkOrderDetails.DeliveryDate = ((Label)frmOrderMasterDetails.FindControl("lblExpectedDate")).Text;
                    Pdf_WorkOrderDetails.WorkOrderNo = txtWorkOrderNo.Text;
                    Pdf_WorkOrderDetails.WorkOrderReviseNo = txtWorkorderReviseNo.Text;
                    Pdf_WorkOrderDetails.Priority = ddl_deliverypriority.SelectedItem.Text;
                    Pdf_WorkOrderDetails.Remarks = txtcomments.Text;
                    Pdf_WorkOrderDetails.Category1 = Convert.ToBoolean(chk_preDispatch.Items[0].Selected);
                    Pdf_WorkOrderDetails.Category2 = Convert.ToBoolean(chk_preDispatch.Items[1].Selected);
                    Pdf_WorkOrderDetails.Category3 = Convert.ToBoolean(chk_preDispatch.Items[2].Selected);
                    Pdf_WorkOrderDetails.Category4 = Convert.ToBoolean(chk_preDispatch.Items[3].Selected);
                    Pdf_WorkOrderDetails.Category5 = Convert.ToBoolean(chk_preDispatch.Items[4].Selected);
                    Pdf_WorkOrderDetails.Category6 = Convert.ToBoolean(chk_preDispatch.Items[5].Selected);
                    Pdf_WorkOrderDetails.OID = Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString()));

                    string OutputString = Pdf_WorkOrderDetails.WorkOrderCreation();
                    if (OutputString == "Success")
                    {
                        Response.Redirect(Request.RawUrl.ToString(), false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void IrnNoPreparation()
        {
            try
            {
                dom_eInvoice.eInvoiceRoot respJsonClass = dom_eInvoice.Dom_eInvoice_Irn(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())), "GEN");
                if (respJsonClass != null)
                {
                    SqlParameter[] spIns = new SqlParameter[] { 
                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
                        new SqlParameter("@invoiceno", lblInvoiceNo.Text), 
                        new SqlParameter("@respstatus", respJsonClass.status), 
                        new SqlParameter("@respmessage", respJsonClass.message != null ? respJsonClass.message : "Error"), 
                        new SqlParameter("@uuid", respJsonClass.uuid != null ? respJsonClass.uuid : ""), 
                        new SqlParameter("@SignedQrCodeImgUrl", respJsonClass.SignedQrCodeImgUrl != null ? respJsonClass.SignedQrCodeImgUrl : ""), 
                        new SqlParameter("@Irn", respJsonClass.Irn != null ? respJsonClass.Irn : ""), 
                        new SqlParameter("@AckDt", respJsonClass.AckDt != null ? respJsonClass.AckDt : ""), 
                        new SqlParameter("@AckNo", respJsonClass.status == 1 ? respJsonClass.AckNo : 0), 
                        new SqlParameter("@invStatus", respJsonClass.Status != null ? respJsonClass.Status : ""), 
                        new SqlParameter("@SignedQRCode", respJsonClass.SignedQRCode != null ? respJsonClass.SignedQRCode : ""), 
                        new SqlParameter("@SignedInvoice", respJsonClass.SignedInvoice != null ? respJsonClass.SignedInvoice : ""), 
                        new SqlParameter("@IrnStatus", respJsonClass.IrnStatus != null ? respJsonClass.IrnStatus : ""), 
                        new SqlParameter("@EwbStatus", respJsonClass.EwbStatus != null ? respJsonClass.EwbStatus : ""), 
                        new SqlParameter("@EwbNo", respJsonClass.EwbNo != null ? respJsonClass.EwbNo : ""), 
                        new SqlParameter("@EwbDt", respJsonClass.EwbDt != null ? respJsonClass.EwbDt : ""), 
                        new SqlParameter("@EwbValidTill", respJsonClass.EwbValidTill != null ? respJsonClass.EwbValidTill : ""), 
                        new SqlParameter("@Remarks", respJsonClass.Remarks != null ? respJsonClass.Remarks : ""  ), 
                        new SqlParameter("@ErrorCode", respJsonClass.ErrorCode != null ? respJsonClass.ErrorCode : ""), 
                        new SqlParameter("@ErrorDetails", respJsonClass.ErrorDetails != null ? respJsonClass.ErrorDetails : ""), 
                        new SqlParameter("@InfoDtls", respJsonClass.InfoDtls != null ? respJsonClass.InfoDtls : ""), 
                        new SqlParameter("@Irp", respJsonClass.Irp != null ? respJsonClass.Irp : ""), 
                        new SqlParameter("@createdby", Request.Cookies["TTSUser"].Value) 
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_eInvoiceResponse", spIns);
                    if (resp > 0)
                    {
                        if (respJsonClass.message.ToLower().Contains("irn generated"))
                        {
                            SqlParameter[] spUpd = new SqlParameter[] { 
                                new SqlParameter("@llrno", txtLRno.Text),
                                new SqlParameter("@VehicleNo", txtvehicleNo.Text),
                                new SqlParameter("@invoiceno", lblInvoiceNo.Text), 
                                new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) 
                            };
                            int respUpd = daCOTS.ExecuteNonQuery_SP("sp_upd_Irn_invoicedetails", spUpd);
                            if (respUpd > 0)
                                Response.Redirect(Request.RawUrl.ToString(), false);
                        }
                    }
                    lblIrnErrMsg.Text = respJsonClass.message;
                    lblIrnErrMsg.Text += (lblIrnErrMsg.Text != "" ? "</br>" : "") + respJsonClass.ErrorDetails;
                    lblIrnErrMsg.Text += (lblIrnErrMsg.Text != "" ? "</br>" : "") + respJsonClass.InfoDtls;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JWrite", "writeErrMsg('" + Utilities.RemoveSpecialCharacters(lblIrnErrMsg.Text) + "');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JWriteErr", "writeErrMsg('" + Utilities.RemoveSpecialCharacters(ex.Message) + "');", true);
            }
        }

        private void InvoiceDocPreparation()
        {
            try
            {
                string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/invoicefiles/" + hdnCustCode.Value + "/"))
                    Directory.CreateDirectory(serverURL + "/invoicefiles/" + hdnCustCode.Value + "/");
                string path = serverURL + "/invoicefiles/" + hdnCustCode.Value + "/";
                string strWaterMark = string.Empty; string strfileAdditional = string.Empty; string sPathToWritePdfTo = string.Empty;

                InvoiceDataModel idm;
                strfileAdditional = "Trasport_invoice_";
                strWaterMark = "DUPLICATE FOR TRANSPORTER";
                sPathToWritePdfTo = path + strfileAdditional + hdnOrderRefNo.Value + ".pdf";
                idm = Build_InvoiceDataModel(strWaterMark);
                Build_AllInvoices(sPathToWritePdfTo, strWaterMark, strfileAdditional, idm);

                SqlParameter[] sp2 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())),
                        new SqlParameter("@invoiceno",  lblInvoiceNo.Text), 
                        new SqlParameter("@despatch", lblTransportName.Text), 
                        new SqlParameter("@llrno", txtLRno.Text), 
                        new SqlParameter("@ContactName", lblContactPerson.Text), 
                        new SqlParameter("@ContactNo", lblContactNo.Text), 
                        new SqlParameter("@CGSTPer", idm.CGSTPer), 
                        new SqlParameter("@SGSTPer", idm.SGSTPer), 
                        new SqlParameter("@IGSTPer", idm.IGSTPer), 
                        new SqlParameter("@CGSTVal", idm.CGSTVal), 
                        new SqlParameter("@SGSTVal", idm.SGSTVal), 
                        new SqlParameter("@IGSTVal", idm.IGSTVal), 
                        new SqlParameter("@VehicleNo", txtvehicleNo.Text), 
                        new SqlParameter("@TransportMethod", lblModeOfTransport.Text)
                    };
                int resp3 = daCOTS.ExecuteNonQuery_SP("sp_update_invoicedetails", sp2);
                if (resp3 > 0)
                {
                    strfileAdditional = "Recepient_invoice_";
                    strWaterMark = "ORIGINAL FOR RECEPIENT";
                    sPathToWritePdfTo = path + strfileAdditional + hdnOrderRefNo.Value + ".pdf";
                    idm = Build_InvoiceDataModel(strWaterMark);
                    Build_AllInvoices(sPathToWritePdfTo, strWaterMark, strfileAdditional, idm);

                    strfileAdditional = "Supplier_invoice_";
                    strWaterMark = "TRIPLICATE FOR SUPPLIER";
                    sPathToWritePdfTo = path + strfileAdditional + hdnOrderRefNo.Value + ".pdf";
                    idm = Build_InvoiceDataModel(strWaterMark);
                    Build_AllInvoices(sPathToWritePdfTo, strWaterMark, strfileAdditional, idm);

                    Build_PDILIST();

                    Response.Redirect(Request.RawUrl.ToString(), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private InvoiceDataModel Build_InvoiceDataModel(string strInvoiceHead)
        {
            InvoiceDataModel iDM = new InvoiceDataModel();
            iDM.InvoiceHead = strInvoiceHead;
            iDM.Plant = hdnorderPlant.Value;
            iDM.Category = hdnCustCategory.Value;
            iDM.LogoPath = Server.MapPath("~/images/tvs_suntws.jpg");
            iDM.CustCode = hdnCustCode.Value;
            iDM.IsProfoma = false;
            iDM.InvoiceNo = lblInvoiceNo.Text;
            iDM.InvoiceDate = lblInvoiceDate.Text;
            iDM.OrderRefNo = hdnOrderRefNo.Value;
            iDM.OrderDate = ((Label)frmOrderMasterDetails.FindControl("lblOrderDate")).Text;
            iDM.TransporterName = ((Label)frmOrderMasterDetails.FindControl("lblTransportDetails")).Text;
            iDM.ModeOfTransport = rdbModeOfTransport.SelectedItem.Text;
            iDM.VehicleNo = txtvehicleNo.Text;
            iDM.LRNo = txtLRno.Text;
            iDM.FreightCharges = ((Label)frmOrderMasterDetails.FindControl("lblFreightCharges")).Text;
            iDM.CcAttached = (((Label)frmOrderMasterDetails.FindControl("lblDeliveryMethod")).Text) +
                (((Label)frmOrderMasterDetails.FindControl("lblGodownName")).Text != "" ? "\n( " + ((Label)frmOrderMasterDetails.FindControl("lblGodownName")).Text + " )" : "");
            iDM.CreditNote = ((HiddenField)frmOrderMasterDetails.FindControl("hdnCreditNote")).Value;
            iDM.Paymentdays = ((HiddenField)frmOrderMasterDetails.FindControl("hdnPaymentdays")).Value != "" ? ((HiddenField)frmOrderMasterDetails.FindControl("hdnPaymentdays")).Value : "0";
            iDM.dtHsnCode = ViewState["dtHsnVal"] as DataTable;
            iDM.PartNo = hdnPartNo.Value == "PartNo" ? true : false;
            iDM.OID = Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString()));
            iDM.IRN = lblIrn.Text;
            iDM.IrnQrCode = hdnIrnQrCode.Value;
            iDM.AckNo = Convert.ToInt64(lblAckNo.Text == "" ? "0" : lblAckNo.Text);
            iDM.AckDt = lblAckDate.Text;
            return iDM;
        }
        private void Build_AllInvoices(string sPathTo, string WaterMark, string fileAdditional, InvoiceDataModel idm)
        {
            try
            {
                Document document = new Document(PageSize.A4, 18f, 2f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sPathTo, FileMode.Create));
                writer.PageEvent = new PDFWriterEvents(WaterMark, 50f);
                document.Open();
                document.Add(PrepareInvoice.Prepare(document, idm));

                document.NewPage();
                PdfPTable mailTable = new PdfPTable(1);
                mailTable.TotalWidth = 520f;
                mailTable.LockedWidth = true;
                PdfPCell cellEmpty = new PdfPCell(new Phrase("\n"));
                cellEmpty.Border = Rectangle.NO_BORDER;
                mailTable.AddCell(cellEmpty);
                mailTable.AddCell(cellEmpty);

                var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                var headFont = FontFactory.GetFont("Arial", 9, Font.UNDERLINE);

                Chunk p1 = new Chunk("\nRefer Item 4 of Terms:\n\n", headFont);
                string strOverDue = "SUN-TWS shall have the right to levy ‘Liquidated Damages’ for any delay in receipt of monies under this " +
                    "invoice at the rate of 2% of gross invoice for delays up to and including 30 days from due date, and at " +
                    "the rate of 5% of gross invoice, for delays in receipt beyond 30 days and within 60 days from due date " +
                    "for payment. All payments received beyond 60 days from the due date shall attract a levy of 10% of the " +
                    "gross invoice value.\n\n";
                Chunk p2 = new Chunk(strOverDue, titleFont);

                Chunk p3 = new Chunk("Additional conditions:\n\n", headFont);
                Chunk p4 = new Chunk("This liquidated damages shall be settled immediately as and when it is levied.\n\n", titleFont);
                Paragraph para = new Paragraph();
                para.Add(p1);
                para.Add(p2);
                para.Add(p3);
                para.Add(p4);
                PdfPCell cell = new PdfPCell(para);
                cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                mailTable.AddCell(cell);

                document.Add(mailTable);
                document.Close();

                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString()));
                sp1[1] = new SqlParameter("@AttachFileName", fileAdditional + hdnOrderRefNo.Value + ".pdf");
                sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp1[3] = new SqlParameter("@FileType", idm.InvoiceHead);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_PDILIST()
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[] { 
                    new SqlParameter("@plant", hdnorderPlant.Value), 
                    new SqlParameter("@custcode", hdnCustCode.Value), 
                    new SqlParameter("@orderrefno", hdnOrderRefNo.Value) 
                };
                DataTable dtStockData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_domesticdispatchedbarcode", sp2, DataAccess.Return_Type.DataTable);
                if (dtStockData != null && dtStockData.Rows.Count > 0)
                {
                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    if (!Directory.Exists(serverURL + "/invoicefiles/" + hdnCustCode.Value + "/"))
                        Directory.CreateDirectory(serverURL + "/invoicefiles/" + hdnCustCode.Value + "/");
                    string path = serverURL + "/invoicefiles/" + hdnCustCode.Value + "/";
                    string strfileAdditional = "PDIList_";
                    string strWaterMark = "PDI REPORT";
                    string sPathToWritePdfTo = path + strfileAdditional + hdnOrderRefNo.Value + ".pdf";

                    Document document = new Document(PageSize.A4, 18f, 2f, 10f, 10f);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sPathToWritePdfTo, FileMode.Create));
                    writer.PageEvent = new PDFWriterEvents(strWaterMark, 80f);
                    document.Open();
                    document.Add(Build_AnnexureTable(document, strWaterMark, dtStockData));
                    document.Close();

                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString()));
                    sp1[1] = new SqlParameter("@AttachFileName", strfileAdditional + hdnOrderRefNo.Value + ".pdf");
                    sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                    sp1[3] = new SqlParameter("@FileType", "PDI LIST");
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", HttpContext.Current.Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private PdfPTable Build_AnnexureTable(Document doc, string strFileType, DataTable dtPdiList)
        {
            PdfPTable table = new PdfPTable(9);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            //                             No   Con Size  Rim  ty  br  si  ste  barco
            float[] widths = new float[] { 1.2f, 3f, 5f, 1.3f, 2f, 3f, 3f, 3f, 5f };
            table.SetWidths(widths);
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                PdfPCell cellTitle = new PdfPCell(new Phrase(strFileType.ToUpper(), titleFont));
                cellTitle.HorizontalAlignment = 1;
                cellTitle.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellTitle.Border = Rectangle.BOTTOM_BORDER;
                cellTitle.Colspan = 9;
                table.AddCell(cellTitle);

                PdfPTable nestedMain = new PdfPTable(2);
                nestedMain.TotalWidth = 560f;
                nestedMain.LockedWidth = true;
                float[] widthsOrderInfo = new float[] { 20f, 10f };
                nestedMain.SetWidths(widthsOrderInfo);

                PdfPTable nestedOrder = new PdfPTable(3);
                nestedOrder.TotalWidth = 280f;
                float[] widthsOrder = new float[] { 3f, 0.2f, 15f };
                nestedOrder.SetWidths(widthsOrder);

                nestedOrder.AddCell(Build_OrderMainDetailsStyle("CUSTOMER"));
                nestedOrder.AddCell(Build_OrderMainDetailsStyle(":"));
                nestedOrder.AddCell(Build_OrderMainDetailsStyle(((Label)frmOrderMasterDetails.FindControl("lblCustomerName")).Text));

                nestedOrder.AddCell(Build_OrderMainDetailsStyle("ORDER NO"));
                nestedOrder.AddCell(Build_OrderMainDetailsStyle(":"));
                nestedOrder.AddCell(Build_OrderMainDetailsStyle(hdnOrderRefNo.Value));

                nestedMain.AddCell(nestedOrder);

                PdfPTable nestedInvoice = new PdfPTable(3);
                nestedInvoice.TotalWidth = 280f;
                float[] widthsInvoice = new float[] { 3f, 0.2f, 5f };
                nestedInvoice.SetWidths(widthsInvoice);

                nestedInvoice.AddCell(Build_OrderMainDetailsStyle("INVOICE NO"));
                nestedInvoice.AddCell(Build_OrderMainDetailsStyle(":"));
                nestedInvoice.AddCell(Build_OrderMainDetailsStyle(lblInvoiceNo.Text));

                nestedInvoice.AddCell(Build_OrderMainDetailsStyle("DATE"));
                nestedInvoice.AddCell(Build_OrderMainDetailsStyle(":"));
                nestedInvoice.AddCell(Build_OrderMainDetailsStyle(lblInvoiceDate.Text));

                nestedMain.AddCell(nestedInvoice);
                PdfPCell cell1 = new PdfPCell(nestedMain);
                cell1.HorizontalAlignment = 0;
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.Border = Rectangle.NO_BORDER;
                cell1.Colspan = 9;
                table.AddCell(cell1);

                table.AddCell(Build_ItemTableBottomStyle("SNo.", 1));
                table.AddCell(Build_ItemTableBottomStyle("PLATFORM", 1));
                table.AddCell(Build_ItemTableBottomStyle("TYRE SIZE", 1));
                table.AddCell(Build_ItemTableBottomStyle("RIM", 1));
                table.AddCell(Build_ItemTableBottomStyle("TYPE", 1));
                table.AddCell(Build_ItemTableBottomStyle("BRAND", 1));
                table.AddCell(Build_ItemTableBottomStyle("SIDEWALL", 1));
                table.AddCell(Build_ItemTableBottomStyle("STENCIL NO", 1));
                table.AddCell(Build_ItemTableBottomStyle("BARCODE NO", 1));

                int j = 1;
                foreach (DataRow row in dtPdiList.Rows)
                {
                    table.AddCell(Build_ItemTableBottomStyle(j.ToString(), 2));
                    table.AddCell(Build_ItemTableBottomStyle(row["config"].ToString(), 0));
                    table.AddCell(Build_ItemTableBottomStyle(row["tyresize"].ToString(), 0));
                    table.AddCell(Build_ItemTableBottomStyle(row["rimsize"].ToString(), 0));
                    table.AddCell(Build_ItemTableBottomStyle(row["tyretype"].ToString(), 0));
                    table.AddCell(Build_ItemTableBottomStyle(row["brand"].ToString(), 0));
                    table.AddCell(Build_ItemTableBottomStyle(row["sidewall"].ToString(), 0));
                    table.AddCell(Build_ItemTableBottomStyle(row["stencilno"].ToString(), 2));
                    table.AddCell(Build_ItemTableBottomStyle(row["barcode"].ToString(), 2));
                    j++;
                }
                var valFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                valFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                //PdfPCell cell = new PdfPCell(new Phrase("\nFor SUN TYRE & WHEEL SYSTEMS\n(A DIVISION OF T.S.RAJAM TYRES PRIVATE LIMITED) \n\n\n\n\n Authorised Signatory", valFont));
                PdfPCell cell = new PdfPCell(new Phrase("\nFor SUN TYRE & WHEEL SYSTEMS\n(A DIVISION OF SUNDARAM INDUSTRIES PRIVATE LIMITED) \n\n\n\n\n Authorised Signatory", valFont));//*************To Be inserted  On 14-02-2022***********************
                cell.Padding = 0f;
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                cell.Colspan = 9;
                cell.PaddingBottom = 1f;
                table.AddCell(cell);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", HttpContext.Current.Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return table;
        }
        private PdfPCell Build_ItemTableBottomStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            return cell;
        }
        private PdfPCell Build_OrderMainDetailsStyle(string strText)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = 0;
            return cell;
        }

        protected void btnMoveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                Label CustomerName = (Label)frmOrderMasterDetails.FindControl("lblCustomerName");
                switch (Utilities.Decrypt(Request["qstring"].ToString()))
                {
                    case "proforma":
                        {
                            DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())), "3", txtcomments.Text,
                                Request.Cookies["TTSUser"].Value);
                            break;
                        }
                    case "workorder":
                        {
                            DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())), "34", txtcomments.Text,
                                Request.Cookies["TTSUser"].Value);
                            break;
                        }
                    case "invoice":
                        {
                            int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())),
                                (btnMoveOrder.Text == "Move to Vehicle Load" ? "20" : (btnMoveOrder.Text == "Move to Dispatched" ? "5" : "0")), txtcomments.Text,
                                Request.Cookies["TTSUser"].Value);

                            break;
                        }
                    case "payment":
                        {
                            DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())), "19", txtcomments.Text,
                                Request.Cookies["TTSUser"].Value);
                            break;
                        }
                    case "tcs":
                        {
                            SqlParameter[] spTcs = new SqlParameter[] { 
                                new SqlParameter("@tcsApplicable",rdbTcsAmt.SelectedItem.Value), 
                                new SqlParameter("@validPan",rdbTcsPan.SelectedItem.Value),
                                new SqlParameter("@O_ID", Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString()))) 
                            };
                            int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_OrderMasterTcs", spTcs);
                            if (resp > 0)
                            {
                                DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())), "1", txtcomments.Text,
                                    Request.Cookies["TTSUser"].Value);
                            }
                            break;
                        }
                }
                Response.Redirect("cotsorderprocess.aspx?qstring=" + Request["qstring"].ToString() + "", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JtoAlert", "alert('Order not Moved'..Try Again!!..');", true);
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
        protected void imgEmail_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                switch (Utilities.Decrypt(Request["qstring"].ToString()))
                {
                    case "proforma":
                        {
                            int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())), "2", txtcomments.Text,
                                Request.Cookies["TTSUser"].Value);
                            if (resp > 0)
                            {
                                string mailConcat = "ATTACHED PROFORMA INVOICE FOR ORDER NO.: " + hdnOrderRefNo.Value + "<br/><br/>";
                                SqlParameter[] param = new SqlParameter[] { new SqlParameter("@custCode", hdnCustCode.Value) };
                                string strTo = daCOTS.ExecuteScalar_SP("sp_sel_useremail_master", param).ToString();
                                string strCc = DomesticScots.Build_ScotsDomestic_CRM_MailList(hdnCustCode.Value);
                                string sPathToWritePdfTo = Server.MapPath("~/").Replace("TTS", "pdfs") + ("/proformafiles/" + hdnCustCode.Value + "/" + hdnOrderRefNo.Value + ".pdf");
                                DomesticScots.scots_domestic_mail_notification_WithAttach(mailConcat.ToString(), "PROFORMA GENERATED", strTo, strCc, sPathToWritePdfTo);
                            }
                            break;
                        }
                }
                Response.Redirect(Request.RawUrl.ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUploadControl.HasFile)
                {
                    //string filename = Path.GetFileName(FileUploadControl.FileName);

                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    if (!Directory.Exists(serverURL + "/lrcopyfiles/"))
                        Directory.CreateDirectory(serverURL + "/lrcopyfiles/");
                    if (!Directory.Exists(serverURL + "/lrcopyfiles/" + hdnCustCode.Value + "/"))
                        Directory.CreateDirectory(serverURL + "/lrcopyfiles/" + hdnCustCode.Value + "/");
                    string path = serverURL + "/lrcopyfiles/" + hdnCustCode.Value + "/";

                    string pathToSave = path + FileUploadControl.FileName;
                    FileUploadControl.SaveAs(pathToSave);

                    if (Directory.Exists(path))
                    {
                        string[] str1 = FileUploadControl.FileName.Split('.');
                        string strExtension = str1[str1.Length - 1].ToString();
                        Directory.Move(pathToSave, serverURL + "/lrcopyfiles/" + hdnCustCode.Value + "/LR_" + hdnOrderRefNo.Value + "." + strExtension);

                        SqlParameter[] sp1 = new SqlParameter[4];
                        sp1[0] = new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString()));
                        sp1[1] = new SqlParameter("@AttachFileName", "LR_" + hdnOrderRefNo.Value + "." + strExtension);
                        sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                        sp1[3] = new SqlParameter("@FileType", "UPLOAD LR COPY");
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);
                        if (resp > 0)
                            Response.Redirect(Request.RawUrl.ToString(), false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnBacktoHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx", false);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("cotsorderprocess.aspx?qstring=" + Request["qstring"].ToString() + "", false);
        }
        protected void btnClearOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl.ToString(), false);
        }
    }
}