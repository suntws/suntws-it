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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using System.Text;

namespace TTS
{
    public partial class exportcotsproforma : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_proforma"].ToString() == "True")
                        {
                            gvReceivedOrderList.DataSource = null;
                            gvReceivedOrderList.DataBind();
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_received_orders", DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvReceivedOrderList.DataSource = dtorderlist;
                                gvReceivedOrderList.DataBind();
                            }
                            else
                            {
                                lblErrMsg.Text = "No records";
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
        protected void lnkProformaBtn_Click(object sender, EventArgs e)
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
                lblCustCurrency.Text = ((HiddenField)row.FindControl("hdnUserCurrency")).Value;

                DataTable dtgv1 = new DataTable();
                dtgv1.Columns.Add("slno", typeof(String));
                for (int i = 1; i <= 3; i++)
                    dtgv1.Rows.Add(i);
                gvAmountSub.DataSource = dtgv1;
                gvAmountSub.DataBind();
                gvAmountSub.HeaderRow.Cells[3].Text = lblCustCurrency.Text;

                txtProformaRefNo.Text = "";
                lblProformaDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtPayterms.Text = "";
                txtPriceBasisContent.Text = "";
                txtClaimAdjustment.Text = "";
                txtLESSAMT.Text = "";
                txtotherdiscount.Text = "";
                txtOtherDisAmt.Text = "";

                hdnStatusOrderDate.Value = ((Label)row.FindControl("lblOrderDate")).Text;
                hdnCustCode.Value = ((HiddenField)row.FindControl("hdnStatusCustCode")).Value;
                lblCurrStatus.Text = ((Label)row.FindControl("lblStatusText")).Text;
                lblCustName.Text = ((Label)row.FindControl("lblStatusCustName")).Text;
                lblStausOrderRefNo.Text = ((Label)row.FindControl("lblOrderRefNo")).Text;
                hdnRequestStatusID.Value = ((HiddenField)row.FindControl("hdnRequestStatus")).Value;
                hdnShipmentType.Value = row.Cells[6].Text;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                if (lblCurrStatus.Text == "ORDER RECEIVED")
                    btnSaveChangeStatus.Text = "CHANGE STATUS TO PROFORMA GENERATED";
                else if (lblCurrStatus.Text == "PROFORMA GENERATED")
                    btnSaveChangeStatus.Text = "CHANGE STATUS TO WORK ORDER PREPARE";
                else
                {
                    if (hdnRequestStatusID.Value == "14" || hdnRequestStatusID.Value == "16" || hdnRequestStatusID.Value == "28")
                        btnSaveChangeStatus.Text = "CHANGE STATUS TO ORDER REVISE PROCESS DONE";
                    else
                        btnSaveChangeStatus.Text = "CHANGE STATUS TO WORK ORDER PREPARE";
                }

                Bind_OrderMasterDetails();
                Bind_OrderItem();
                if (hdnShipmentType.Value == "COMBI")
                    Bind_OrderSumValue();
                Bind_ReviseDetails();
                Bind_Attach_PdfFiles();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
                if (lnkPdfLink.Text != "")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "showStatusChangeBtn();", true);
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
                DataTable dtMasterList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_OrderAllDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dtMasterList.Rows.Count > 0)
                {
                    dlOrderMaster.DataSource = dtMasterList;
                    dlOrderMaster.DataBind();
                    ddlPreCarriageBy.SelectedIndex = ddlPreCarriageBy.Items.IndexOf(ddlPreCarriageBy.Items.FindByText(dtMasterList.Rows[0]["DeliveryMethod"].ToString()));
                    ddlPriceBasis.SelectedIndex = ddlPriceBasis.Items.IndexOf(ddlPriceBasis.Items.FindByText(dtMasterList.Rows[0]["GodownName"].ToString()));
                    hdnPartNo.Value = dtMasterList.Rows[0]["specialdesc"].ToString();
                }

                DataTable dtExpAuthorizedAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CustWise_AuthorizedAddress", DataAccess.Return_Type.DataTable);
                if (dtExpAuthorizedAddress.Rows.Count > 0)
                {
                    ddlExpAuthorizedAddress.DataSource = dtExpAuthorizedAddress;
                    ddlExpAuthorizedAddress.DataTextField = "ExpoterAddress";
                    ddlExpAuthorizedAddress.DataValueField = "ID";
                    ddlExpAuthorizedAddress.DataBind();
                }
                ddlExpAuthorizedAddress.Items.Insert(0, "CHOOSE");
                ddlExpAuthorizedAddress.SelectedIndex = ddlExpAuthorizedAddress.Items.IndexOf(ddlExpAuthorizedAddress.Items.FindByValue(dtMasterList.Rows[0]["ExporterAddressID"].ToString()));

                txtProformaRefNo.Enabled = true;
                SqlParameter[] sprev = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtrev = (DataTable)daCOTS.ExecuteReader_SP("Sp_Chk_Exp_ProformaRefNo", sprev, DataAccess.Return_Type.DataTable);
                if (dtrev != null && dtrev.Rows.Count > 0)
                {
                    txtProformaRefNo.Text = dtrev.Rows[0]["proformarefno"].ToString();
                    lblProformaDate.Text = dtrev.Rows[0]["PIDATE"].ToString();
                    lblReviseCount.Text = dtrev.Rows[0]["revise_no"].ToString();
                    txtProformaRefNo.Enabled = false;
                }
                else
                    lblReviseCount.Text = "00";

                SqlParameter[] spRef = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtProformaRef = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_ProformaRefNo", spRef, DataAccess.Return_Type.DataTable);
                if (dtProformaRef != null && dtProformaRef.Rows.Count > 0)
                {
                    txtPayterms.Text = dtProformaRef.Rows[0]["payterms"].ToString().Replace("~", "\r\n");
                    txtPriceBasisContent.Text = dtProformaRef.Rows[0]["PriceBasisContent"].ToString();
                    ddlPriceBasis.SelectedIndex = ddlPriceBasis.Items.IndexOf(ddlPriceBasis.Items.FindByText(dtProformaRef.Rows[0]["PriceMethod"].ToString()));
                    chkAssyPrice.Checked = Convert.ToBoolean(dtProformaRef.Rows[0]["AssyPriceSplit"].ToString());
                    ddlContainerType.SelectedIndex = ddlContainerType.Items.IndexOf(ddlContainerType.Items.FindByText(dtProformaRef.Rows[0]["ContainerType"].ToString()));
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
        private void Bind_OrderItem()
        {
            try
            {
                gvOrderItemList.DataSource = null;
                gvOrderItemList.DataBind();

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
                    chkAssyPrice.Visible = false;
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;
                        gvOrderItemList.Columns[13].Visible = true;
                        gvOrderItemList.Columns[14].Visible = true;
                        chkAssyPrice.Visible = true;

                        gvOrderItemList.FooterRow.Cells[12].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                        gvOrderItemList.FooterRow.Cells[13].Text = Convert.ToDecimal(dtItemList.Compute("Sum(Rimpricepdf)", "")).ToString();
                        gvOrderItemList.FooterRow.Cells[14].Text = Convert.ToDecimal(dtItemList.Compute("Sum(totalRimWt)", "")).ToString();
                        break;
                    }
                    gvOrderItemList.Columns[17].Visible = false;
                    if (hdnPartNo.Value == "BarcodeLable")
                        gvOrderItemList.Columns[17].Visible = true;
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
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtSumList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_Order_SumValue_Logistics", sp1, DataAccess.Return_Type.DataTable);
                if (dtSumList.Rows.Count > 0)
                {
                    gv_OrderSumValue.DataSource = dtSumList;
                    gv_OrderSumValue.DataBind();

                    gv_OrderSumValue.FooterRow.Cells[6].Text = "Total";
                    gv_OrderSumValue.FooterRow.Cells[7].Text = dtSumList.Compute("Sum([TOT QTY])", "").ToString();
                    gv_OrderSumValue.FooterRow.Cells[8].Text = dtSumList.Compute("Sum([TOT PRICE])", "").ToString();
                    gv_OrderSumValue.FooterRow.Cells[9].Text = dtSumList.Compute("Sum([TOT WT])", "").ToString();

                    if ((Convert.ToDecimal(gv_OrderSumValue.FooterRow.Cells[9].Text) / 1000) < 14)
                        ddlContainerType.SelectedIndex = 2;
                    else if ((Convert.ToDecimal(gv_OrderSumValue.FooterRow.Cells[9].Text) / 1000) > 20)
                        ddlContainerType.SelectedIndex = 1;
                    else
                        ddlContainerType.SelectedIndex = 0;
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
                SqlParameter[] spdiscount = new SqlParameter[] { new SqlParameter("@OID", hdnOID.Value) };
                DataTable dtdiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_Discount", spdiscount, DataAccess.Return_Type.DataTable);
                if (dtdiscount != null && dtdiscount.Rows.Count > 0)
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
                    foreach (DataRow disRow in dtdiscount.Select("Discounttype='CLAIM ADJUSTMENT'"))
                    {
                        txtClaimAdjustment.Text = disRow["DescriptionDiscount"].ToString();
                        txtLESSAMT.Text = disRow["Amount"].ToString();
                    }
                    foreach (DataRow disRow in dtdiscount.Select("Discounttype='OTHER DISCOUNT'"))
                    {
                        txtotherdiscount.Text = disRow["DescriptionDiscount"].ToString();
                        txtOtherDisAmt.Text = disRow["Amount"].ToString();
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
                lnkPdfLink.Text = "";
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value), new SqlParameter("@FileType", "PROFORMA FILE") };
                DataTable dtPdfFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AttachPdfFiles", sp1, DataAccess.Return_Type.DataTable);

                if (dtPdfFile != null && dtPdfFile.Rows.Count > 0)
                    lnkPdfLink.Text = dtPdfFile.Rows[0]["AttachFileName"].ToString();
                else
                {
                    string strTerms = (string)daCOTS.ExecuteScalar("select PaymentTerms from usermaster where ID='" + hdnCustCode.Value + "'");
                    txtPayterms.Text = Server.HtmlDecode(strTerms.Replace("~", "\r\n"));
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkPdfLink_click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string path = Server.MapPath("~/proformafiles/" + hdnCustCode.Value + "/").Replace("TTS", "pdfs") + lnkTxt.Text;

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPrepareProforma_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[14];
                sp2[0] = new SqlParameter("@O_ID", hdnOID.Value);
                sp2[1] = new SqlParameter("@proformarefno", txtProformaRefNo.Text);
                sp2[2] = new SqlParameter("@payterms", txtPayterms.Text.Replace("\r\n", "~"));
                sp2[3] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                sp2[4] = new SqlParameter("@PriceMethod", ddlPriceBasis.SelectedItem.Text);
                sp2[5] = new SqlParameter("@PriceBasisContent", txtPriceBasisContent.Text);
                sp2[6] = new SqlParameter("@CountryOfOrigin", ddlCountryOrigin.SelectedItem.Text);
                sp2[7] = new SqlParameter("@PreCarriageBy", ddlPreCarriageBy.SelectedItem.Text);
                sp2[8] = new SqlParameter("@PlaceOfReceiptByPreCarrier", ddlPlaceofReceipt.SelectedItem.Text);
                sp2[9] = new SqlParameter("@PortOfLoading", ddlPortLoading.SelectedItem.Text.ToUpper());
                sp2[10] = new SqlParameter("@revise_no", lblReviseCount.Text);
                sp2[11] = new SqlParameter("@ExporterAddressID", ddlExpAuthorizedAddress.SelectedItem.Value);
                sp2[12] = new SqlParameter("@AssyPriceSplit", chkAssyPrice.Checked);
                sp2[13] = new SqlParameter("@ContainerType", ddlContainerType.SelectedItem.Text);
                daCOTS.ExecuteNonQuery_SP("sp_ins_ProformaCreateDetails_export", sp2);

                AddtionalCharge();

                if (hdnPartNo.Value == "BarcodeLable")
                    BarcodeLable_TextUpdate();

                string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/proformafiles/" + hdnCustCode.Value + "/"))
                    Directory.CreateDirectory(serverURL + "/proformafiles/" + hdnCustCode.Value + "/");
                string path = serverURL + "/proformafiles/" + hdnCustCode.Value + "/";
                string sPathToWritePdfTo = path + lblStausOrderRefNo.Text + ".pdf";

                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sPathToWritePdfTo, FileMode.Create));
                writer.PageEvent = new PDFWriterEvents("PROFORMA INVOICE", 20, 300, 50, 0);
                document.Open();
                document.Add(Build_ProformaDetails(document, "PROFORMA INVOICE"));
                document.Close();

                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@OID", hdnOID.Value);
                sp1[1] = new SqlParameter("@AttachFileName", lblStausOrderRefNo.Text + ".pdf");
                sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp1[3] = new SqlParameter("@FileType", "PROFORMA FILE");
                daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value),
                    new SqlParameter("@requestmail", Request.Cookies["TTSUserEmail"].Value),
                    new SqlParameter("@requestby", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@processtype", "EXP PROFORMA")
                };
                daCOTS.ExecuteNonQuery_SP("sp_ins_excel_prepare_history", sp);

                gvReceivedOrderList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                Build_SelectOrderDetails(gvReceivedOrderList.SelectedRow);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "showStatusChangeBtn();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void AddtionalCharge()
        {
            try
            {
                SqlParameter[] spdel = new SqlParameter[] { new SqlParameter("@OID", hdnOID.Value) };
                daCOTS.ExecuteNonQuery_SP("sp_del_Export_Discount", spdel);

                for (int i = 0; i < gvAmountSub.Rows.Count; i++)
                {
                    string DESC = ((TextBox)gvAmountSub.Rows[i].FindControl("txtAddDesc")).Text.Trim();
                    string AMT = ((TextBox)gvAmountSub.Rows[i].FindControl("txtCAddAmt")).Text.Trim();
                    if (DESC != "" && AMT != "")
                    {
                        SqlParameter[] sp2 = new SqlParameter[] {
                            new SqlParameter("@OID", hdnOID.Value),
                            new SqlParameter("@Category", "ADD"),
                            new SqlParameter("@DescriptionDiscount", DESC),
                            new SqlParameter("@Amount", AMT),
                            new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                            new SqlParameter("@discountType", "OTHER CHARGES")
                        };
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp2);
                    }
                }
                if (txtClaimAdjustment.Text != "" && txtLESSAMT.Text != "")
                {
                    SqlParameter[] sp12 = new SqlParameter[]{
                        new SqlParameter("@OID", hdnOID.Value),
                        new SqlParameter("@Category", lblLESSclaimAdjus.ToolTip),
                        new SqlParameter("@DescriptionDiscount", txtClaimAdjustment.Text),
                        new SqlParameter("@Amount", txtLESSAMT.Text),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@discountType", "CLAIM ADJUSTMENT")
                    };
                    int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp12);
                }
                if (txtotherdiscount.Text != "" && txtOtherDisAmt.Text != "")
                {
                    SqlParameter[] sp112 = new SqlParameter[]{
                        new SqlParameter("@OID", hdnOID.Value),
                        new SqlParameter("@Category", lblLessdiscount.ToolTip),
                        new SqlParameter("@DescriptionDiscount", txtotherdiscount.Text),
                        new SqlParameter("@Amount", txtOtherDisAmt.Text),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@discountType", "OTHER DISCOUNT")
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp112);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void BarcodeLable_TextUpdate()
        {
            try
            {
                for (int i = 0; i < gvOrderItemList.Rows.Count; i++)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value),
                        new SqlParameter("@ItemCode", ((TextBox)gvOrderItemList.Rows[i].FindControl("txtCode")).Text.Trim()), 
                        new SqlParameter("@ProcessID", ((HiddenField) gvOrderItemList.Rows[i].FindControl("hdnProcessID")).Value) 
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_upd_Customer_BarcodeLable", sp);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private PdfPTable Build_ProformaDetails(Document doc, string strFileHead)
        {
            PdfPTable table = new PdfPTable(2);
            try
            {
                table.TotalWidth = 560f;
                table.LockedWidth = true;
                //                             
                float[] widths = new float[] { 20.7f, 12f };
                table.SetWidths(widths);

                Build_PdfHeadingTable(table, strFileHead);
                Build_AddressChildTable(table);
                Build_OrderRevision(table);

                Build_CustAddressDetailsTable(table);
                Build_OrderMainTable(table);

                Build_ProformaOrderItems(table);
                Build_ProformaBottomDetails(table);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return table;
        }
        private void Build_PdfHeadingTable(PdfPTable table, string strInvoiceFileHead)
        {
            try
            {
                PdfPTable nested = new PdfPTable(2);
                string imageFilePath = Server.MapPath("~/images/tvs_suntws.jpg");
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageFilePath);
                img.ScaleToFit(199f, 39f);
                PdfPCell cell1 = new PdfPCell(img);
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1.VerticalAlignment = Element.ALIGN_TOP;
                cell1.Padding = 0f;
                cell1.Border = Rectangle.NO_BORDER;
                nested.AddCell(cell1);

                var headFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
                Chunk cellTot = new Chunk("PROFORMA INVOICE", headFont);
                PdfPCell cell = new PdfPCell(new Phrase(cellTot));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                cell.FixedHeight = 30f;
                cell.Border = Rectangle.NO_BORDER;
                nested.AddCell(cell);

                PdfPCell nesthousing1 = new PdfPCell(nested);
                nesthousing1.Padding = 0f;
                nesthousing1.Border = Rectangle.NO_BORDER;
                table.AddCell(nesthousing1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_AddressChildTable(PdfPTable table)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtAdd = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ExportAddress", sp, DataAccess.Return_Type.DataTable);
                PdfPTable nested = new PdfPTable(1);
                var headFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                Chunk postCell1 = new Chunk(dtAdd.Rows[0]["AuthorizedAddress"].ToString() + "\n", headFont);
                string AddressTxt = dtAdd.Rows[0]["ExpoterAddress"].ToString().Replace("~", "\n");
                var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                Chunk postCell2 = new Chunk(AddressTxt + "\n\n", titleFont);
                Phrase para = new Phrase();
                para.Add(postCell1);
                para.Add(postCell2);
                PdfPCell cell = new PdfPCell(new Phrase(para));
                cell.ExtraParagraphSpace = 2f;
                cell.Border = Rectangle.NO_BORDER;
                nested.AddCell(cell);
                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Padding = 0f;
                nesthousing.Border = Rectangle.NO_BORDER;
                table.AddCell(nesthousing);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_OrderRevision(PdfPTable table)
        {
            try
            {
                string strProformaDetails = string.Empty;
                strProformaDetails += "\nORDER REF NO.: " + txtProformaRefNo.Text + "\n";

                var titleBoldFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                PdfPCell cellRef = new PdfPCell(new Phrase(strProformaDetails, titleBoldFont));
                cellRef.HorizontalAlignment = Element.ALIGN_LEFT;
                cellRef.VerticalAlignment = Element.ALIGN_TOP;
                cellRef.Padding = 0f;
                cellRef.Border = Rectangle.NO_BORDER;
                table.AddCell(cellRef);

                PdfPTable nested = new PdfPTable(2);
                PdfPCell reviseTable = new PdfPCell(new Phrase("REV NO", titleBoldFont));
                nested.AddCell(reviseTable);
                nested.AddCell(new Phrase(lblReviseCount.Text != "" ? Convert.ToInt32(lblReviseCount.Text).ToString("00") : "00", titleBoldFont));
                reviseTable = new PdfPCell(new Phrase("REV DATE", titleBoldFont));
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                nested.AddCell(reviseTable);
                nested.AddCell(new Phrase(DateTime.Now.ToString("dd-MMM-yyyy") + "\n", titleBoldFont));

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Padding = 0f;
                //nesthousing.Colspan = 2;
                nesthousing.HorizontalAlignment = Element.ALIGN_RIGHT;
                nesthousing.VerticalAlignment = Element.ALIGN_TOP;
                nesthousing.Padding = 0f;
                table.AddCell(nesthousing);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_CustAddressDetailsTable(PdfPTable table)
        {
            try
            {
                PdfPTable nested1;
                float[] widths;
                if (hdnCustCode.Value == "2642")
                {
                    nested1 = new PdfPTable(2);
                    widths = new float[] { 15f, 15f };
                }
                else
                {
                    nested1 = new PdfPTable(1);
                    widths = new float[] { 30f };
                }
                nested1.SetWidths(widths);
                nested1.TotalWidth = 560f;
                nested1.LockedWidth = true;

                if (hdnCustCode.Value == "2642")
                {
                    PdfPCell dumCell = new PdfPCell(new Phrase("\n"));
                    dumCell.Border = Rectangle.NO_BORDER;
                    nested1.AddCell(dumCell);
                    dumCell = new PdfPCell(new Phrase("\n"));
                    dumCell.Border = Rectangle.NO_BORDER;
                    nested1.AddCell(dumCell);
                }

                //Customer Address
                PdfPTable nested2 = new PdfPTable(1);
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderBillingAddress", sp1, DataAccess.Return_Type.DataTable);
                if (dtBillAddress.Rows.Count > 0)
                {
                    DataRow row = dtBillAddress.Rows[0];
                    var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    PdfPCell Cell1 = new PdfPCell(new Phrase(row["custfullname"].ToString() + "", titleFont));//"M/S." +
                    Cell1.HorizontalAlignment = 0;
                    Cell1.VerticalAlignment = Element.ALIGN_TOP;
                    Cell1.Border = Rectangle.NO_BORDER;
                    nested2.AddCell(Cell1);

                    titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                    string AddressTxt = string.Empty;
                    AddressTxt += row["shipaddress"].ToString().Trim() != "" ? row["shipaddress"].ToString().Replace("~", "\n") + "\n" : "\n";
                    AddressTxt += row["city"].ToString().Trim() != "" ? row["city"].ToString() + "\n" : "\n";
                    AddressTxt += row["statename"].ToString().Trim() != "" ? row["statename"].ToString() + " - " : "";
                    AddressTxt += row["country"].ToString().Trim() != "" ? row["country"].ToString() + " - " : "";
                    AddressTxt += row["zipcode"].ToString().Trim() != "" ? row["zipcode"].ToString() + "\n" : "\n";
                    AddressTxt += row["mobile"].ToString().Trim() != "" ? "TEL NO: " + row["mobile"].ToString() + "\n" : "\n";
                    AddressTxt += row["EmailID"].ToString().Trim() != "" ? "EMAIL: " + row["EmailID"].ToString() + "\n\n" : "\n\n";
                    AddressTxt += row["contact_name"].ToString().Trim() != "" ? "KIND ATTN: " + row["contact_name"].ToString() + "\n" : "\n";
                    Cell1 = new PdfPCell(new Phrase(AddressTxt, titleFont));
                    Cell1.HorizontalAlignment = 0;
                    Cell1.VerticalAlignment = Element.ALIGN_TOP;
                    Cell1.Border = Rectangle.NO_BORDER;
                    nested2.AddCell(Cell1);

                    PdfPCell nesthousing = new PdfPCell(nested2);
                    nesthousing.Padding = 0f;
                    nesthousing.Border = Rectangle.NO_BORDER;
                    nested1.AddCell(nesthousing);
                }

                if (hdnCustCode.Value == "2642")
                {
                    nested2 = new PdfPTable(1);
                    sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                    DataTable dtShipAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderShippingAddress", sp1, DataAccess.Return_Type.DataTable);
                    if (dtShipAddress.Rows.Count > 0)
                    {
                        DataRow row = dtShipAddress.Rows[0];
                        var headFont = FontFactory.GetFont("Arial", 10, Font.UNDERLINE);
                        PdfPCell Cell1 = new PdfPCell(new Phrase("SHIP TO", headFont));
                        Cell1.HorizontalAlignment = 0;
                        Cell1.VerticalAlignment = Element.ALIGN_TOP;
                        Cell1.Border = Rectangle.NO_BORDER;
                        nested2.AddCell(Cell1);

                        hdnAuthorized.Value = row["custfullname"].ToString();
                        var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                        Cell1 = new PdfPCell(new Phrase(row["custfullname"].ToString() + "", titleFont));
                        Cell1.HorizontalAlignment = 0;
                        Cell1.VerticalAlignment = Element.ALIGN_TOP;
                        Cell1.Border = Rectangle.NO_BORDER;
                        nested2.AddCell(Cell1);

                        titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                        string AddressTxt = string.Empty;
                        AddressTxt += row["shipaddress"].ToString().Trim() != "" ? row["shipaddress"].ToString().Replace("~", "\n") + "\n" : "\n";
                        AddressTxt += row["city"].ToString().Trim() != "" ? row["city"].ToString() + "\n" : "\n";
                        AddressTxt += row["statename"].ToString().Trim() != "" ? row["statename"].ToString() + " - " : "";
                        AddressTxt += row["country"].ToString().Trim() != "" ? row["country"].ToString() + " - " : "";
                        AddressTxt += row["zipcode"].ToString().Trim() != "" ? row["zipcode"].ToString() + "\n" : "\n";
                        AddressTxt += row["mobile"].ToString().Trim() != "" ? "TEL NO: " + row["mobile"].ToString() + "\n" : "\n";
                        AddressTxt += row["EmailID"].ToString().Trim() != "" ? "EMAIL: " + row["EmailID"].ToString() + "\n\n" : "\n\n";
                        AddressTxt += row["contact_name"].ToString().Trim() != "" ? "KIND ATTN: " + row["contact_name"].ToString() + "\n" : "\n";
                        Cell1 = new PdfPCell(new Phrase(AddressTxt, titleFont));
                        Cell1.HorizontalAlignment = 0;
                        Cell1.VerticalAlignment = Element.ALIGN_TOP;
                        Cell1.Border = Rectangle.NO_BORDER;
                        nested2.AddCell(Cell1);

                        PdfPTable nestSub = new PdfPTable(1);
                        nestSub.AddCell(nested2);

                        PdfPCell nesthousing = new PdfPCell(nestSub);
                        nesthousing.Padding = 0f;
                        nesthousing.Border = Rectangle.NO_BORDER;
                        nested1.AddCell(nesthousing);
                    }
                }

                PdfPCell nesthousingMain = new PdfPCell(nested1);
                nesthousingMain.Padding = 0f;
                nesthousingMain.Colspan = 2;
                nesthousingMain.Border = Rectangle.NO_BORDER;
                table.AddCell(nesthousingMain);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_OrderMainTable(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                PdfPTable nested1 = new PdfPTable(1);
                nested1.TotalWidth = 560f;
                nested1.LockedWidth = true;
                //                             
                float[] widths = new float[] { 30f };
                nested1.SetWidths(widths);

                //PdfPCell cell = new PdfPCell(new Phrase("\nDEAR CUSTOMER, \n", titleFont));
                //cell.HorizontalAlignment = 0;
                //cell.Border = 0;
                //nested1.AddCell(cell);

                PdfPCell cell = new PdfPCell(new Phrase("CUSTOMER PO NO: " + Utilities.Replace_OrderRefNo(lblStausOrderRefNo.Text) + "\n", titleFont));
                cell.HorizontalAlignment = 0;
                cell.Border = 0;
                nested1.AddCell(cell);

                PdfPCell nesthousingMain = new PdfPCell(nested1);
                nesthousingMain.Padding = 0f;
                nesthousingMain.Colspan = 2;
                nesthousingMain.Border = Rectangle.NO_BORDER;
                table.AddCell(nesthousingMain);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_ProformaOrderItems(PdfPTable table)
        {
            try
            {
                PdfPTable dt = new PdfPTable(12);
                dt.TotalWidth = 560f;
                dt.LockedWidth = true;
                //                             NO   size  rim  pl  br  ty   sw   qty   pri  totpri  wt  totwt  Pla
                float[] widths = new float[] { 0.9f, 4.5f, 1f, 3f, 3f, 1.5f, 3f, 1.1f, 1.6f, 1.9f, 1.4f, 1.9f };//, 1.5f 
                dt.SetWidths(widths);

                //Order Items Heading
                dt.AddCell(Build_ItemTableHeadStyle("SR NO"));
                dt.AddCell(Build_ItemTableHeadStyle("TYRE SIZE"));
                dt.AddCell(Build_ItemTableHeadStyle("RIM"));
                dt.AddCell(Build_ItemTableHeadStyle("PLATFORM"));
                dt.AddCell(Build_ItemTableHeadStyle("BRAND"));
                dt.AddCell(Build_ItemTableHeadStyle("TYPE"));
                dt.AddCell(Build_ItemTableHeadStyle("SIDEWALL"));
                dt.AddCell(Build_ItemTableHeadStyle("QTY"));
                dt.AddCell(Build_ItemTableHeadStyle("UNIT PRICE\n" + lblCustCurrency.Text + "\n" + ddlPriceBasis.SelectedItem.Text.ToUpper() + " " + txtPriceBasisContent.Text));
                dt.AddCell(Build_ItemTableHeadStyle("TOTAL VALUE\n" + lblCustCurrency.Text + "\n" + ddlPriceBasis.SelectedItem.Text.ToUpper() + " " + txtPriceBasisContent.Text));
                dt.AddCell(Build_ItemTableHeadStyle("WT\nKGS"));
                dt.AddCell(Build_ItemTableHeadStyle("TOT WT\nKGS"));
                //dt.AddCell(Build_ItemTableHeadStyle("PLANT"));

                //Items Add
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_orderitemlist_for_proforma", sp1, DataAccess.Return_Type.DataTable);

                int j = 1;
                DataView dtDescView = new DataView(dtItemList);
                dtDescView.Sort = "brand,AssyRimstatus ASC";
                DataTable distinctDesc = dtDescView.ToTable(true, "brand", "tyretype", "TypeDesc", "category", "AssyRimstatus", "RimDwg", "EdcNo");
                foreach (DataRow dRow in distinctDesc.Rows)
                {

                    string strItemDesc = dRow["brand"].ToString() != "" ? dRow["brand"].ToString() + ", " : "";
                    strItemDesc += dRow["TypeDesc"].ToString() != "" ? dRow["TypeDesc"].ToString() + ", " : "";
                    strItemDesc += dRow["tyretype"].ToString() != "" ? dRow["tyretype"].ToString() : "";
                    string strLimpet = dRow["tyretype"].ToString() != "" ? ((dRow["tyretype"].ToString()).Substring(dRow["tyretype"].ToString().Length - 1, 1).ToUpper()) : "";
                    strItemDesc += strLimpet == "X" ? " WITHOUT CLIP / LIMPET" : strLimpet == "4" ? " WITH CLIP / LIMPET" : "";
                    if (Convert.ToBoolean(dRow["AssyRimstatus"].ToString()) == true)
                        strItemDesc += " WITH ASSY (" + dRow["EdcNo"].ToString().ToUpper() + " / " + dRow["RimDwg"].ToString().ToUpper() + ")";

                    strItemDesc = strItemDesc != "" ? strItemDesc : dRow["category"].ToString() + (dRow["EdcNo"].ToString() != "" ? " (" + dRow["EdcNo"].ToString() + " / " +
                        dRow["RimDwg"].ToString().ToUpper() + ")" : "");
                    dt.AddCell(Build_ItemTableBottomStyle(strItemDesc.ToUpper(), 0, 13));
                    foreach (DataRow row in dtItemList.Select("category='" + dRow["category"].ToString() + "' and brand='" + dRow["brand"].ToString()
                        + "' and tyretype='" + dRow["tyretype"].ToString() + "' and TypeDesc='" + dRow["TypeDesc"].ToString() + "' and AssyRimstatus='" +
                        dRow["AssyRimstatus"].ToString() + "' and EdcNo='" + dRow["EdcNo"].ToString() + "'"))
                    {
                        if (Convert.ToBoolean(row["AssyRimstatus"]) == false)
                        {
                            dt.AddCell(Build_ItemTableListStyle(j.ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(((row["category"].ToString() == "SPLIT RIMS" || row["category"].ToString() == "POB WHEEL") ?
                                row["EdcRimSize"].ToString() : row["Tyresize"].ToString()), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["RimSize"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["Config"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["Brand"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["TyreType"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["sidewall"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["ItemQty"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(row["listprice"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(row["unitpricepdf"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(row["tyrewt"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(row["totalfwt"].ToString(), 2));
                        }
                        else if (Convert.ToBoolean(row["AssyRimstatus"]) == true && chkAssyPrice.Checked)
                        {
                            dt.AddCell(Build_ItemTableListStyleSameQty(j.ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyleSameQty(((row["category"].ToString() == "SPLIT RIMS" || row["category"].ToString() == "POB WHEEL") ?
                                row["EdcRimSize"].ToString() : row["Tyresize"].ToString()), 0));
                            dt.AddCell(Build_ItemTableListStyleSameQty(row["RimSize"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyleSameQty(row["Config"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyleSameQty(row["Brand"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyleSameQty(row["TyreType"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyleSameQty(row["sidewall"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyleSameQty(row["ItemQty"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(row["listprice"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle((Convert.ToDecimal(row["ItemQty"].ToString()) * Convert.ToDecimal(row["listprice"].ToString())).ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(Convert.ToDecimal(row["tyrewt"].ToString()).ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle((Convert.ToDecimal(row["ItemQty"].ToString()) * Convert.ToDecimal(row["tyrewt"].ToString())).ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(row["Rimunitprice"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle((Convert.ToDecimal(row["Rimitemqty"].ToString()) * Convert.ToDecimal(row["Rimunitprice"].ToString())).ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(Convert.ToDecimal(row["Rimfinishedwt"].ToString()).ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle((Convert.ToDecimal(row["Rimitemqty"].ToString()) * Convert.ToDecimal(row["Rimfinishedwt"].ToString())).ToString(), 2));
                        }
                        else if (Convert.ToBoolean(row["AssyRimstatus"]) == true && !chkAssyPrice.Checked)
                        {
                            dt.AddCell(Build_ItemTableListStyle(j.ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(((row["category"].ToString() == "SPLIT RIMS" || row["category"].ToString() == "POB WHEEL") ?
                                row["EdcRimSize"].ToString() : row["Tyresize"].ToString()), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["RimSize"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["Config"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["Brand"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["TyreType"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["sidewall"].ToString(), 0));
                            dt.AddCell(Build_ItemTableListStyle(row["ItemQty"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(row["bothunitprice"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle((Convert.ToDecimal(row["ItemQty"].ToString()) * Convert.ToDecimal(row["bothunitprice"].ToString())).ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle(row["bothfinWt"].ToString(), 2));
                            dt.AddCell(Build_ItemTableListStyle((Convert.ToDecimal(row["ItemQty"].ToString()) * Convert.ToDecimal(row["bothfinWt"].ToString())).ToString(), 2));
                        }
                        j++;
                    }
                }
                dt.AddCell(Build_ItemTableBottomStyle("SUB TOTAL", 2, 7));
                dt.AddCell(Build_ItemTableBottomStyle(dtItemList.Compute("Sum(itemqty)", "").ToString(), 2, 1));
                dt.AddCell(Build_ItemTableBottomStyle(dtItemList.Compute("Sum(unitprice)", "").ToString(), 2, 2));
                dt.AddCell(Build_ItemTableBottomStyle(dtItemList.Compute("Sum(finishedwt)", "").ToString(), 2, 2));
                SqlParameter[] spdiscount = new SqlParameter[] { new SqlParameter("@OID", hdnOID.Value) };
                DataTable dtdiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_Discount", spdiscount, DataAccess.Return_Type.DataTable);
                for (int i = 0; i < dtdiscount.Rows.Count; i++)
                {
                    dt.AddCell(Build_ItemTableBottomStyle(dtdiscount.Rows[i]["Category"].ToString() + ": " + dtdiscount.Rows[i]["DescriptionDiscount"].ToString(), 2, 7));
                    dt.AddCell(Build_ItemTableBottomStyle("", 2, 1));
                    dt.AddCell(Build_ItemTableBottomStyle(dtdiscount.Rows[i]["Amount"].ToString(), 2, 2));
                    dt.AddCell(Build_ItemTableBottomStyle("\n", 2, 2));
                }
                dt.AddCell(Build_ItemTableBottomStyle("TOTAL " + lblCustCurrency.Text, 2, 7));
                dt.AddCell(Build_ItemTableBottomStyle("", 2, 1));
                Decimal txtTOTALCOST1 = 0;
                txtTOTALCOST1 = txtTOTALCOST1 + (Convert.ToDecimal(dtItemList.Compute("Sum(unitprice)", "").ToString()));
                for (int i = 0; i < dtdiscount.Rows.Count; i++)
                {
                    if (dtdiscount.Rows[i]["Category"].ToString() == "ADD")
                    {
                        txtTOTALCOST1 += (Convert.ToDecimal(dtdiscount.Rows[i]["Amount"]));
                    }
                    else if (dtdiscount.Rows[i]["Category"].ToString() == "LESS")
                    {
                        txtTOTALCOST1 -= (Convert.ToDecimal(dtdiscount.Rows[i]["Amount"]));
                    }
                }
                dt.AddCell(Build_ItemTableBottomStyle(txtTOTALCOST1.ToString(), 2, 2));
                dt.AddCell(Build_ItemTableBottomStyle("\n", 2, 2));
                var titleFont = FontFactory.GetFont("Arial", 8, Font.BOLD);

                string[] strSplit = txtTOTALCOST1.ToString().Split('.');
                int intPart = Convert.ToInt32(strSplit[0].ToString());
                string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
                string text = lblCustCurrency.Text + " " + Utilities.NumberToText(intPart, true, false).ToString().ToUpper();
                if (Convert.ToDecimal(decimalPart) > 0)
                    text += " AND CENTS" + Utilities.DecimalToText(decimalPart).ToUpper() + " ONLY";
                else
                    text += " ONLY";

                Chunk p2 = new Chunk(text + "\n", titleFont);
                Paragraph para = new Paragraph();
                //para.Add(p1);
                para.Add(p2);
                PdfPCell cell = new PdfPCell(new Phrase(para));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.FixedHeight = 30f;
                cell.Colspan = 12;
                dt.AddCell(cell);

                //Proforma subject
                string strCategory = string.Empty;
                string strBrandSubj = string.Empty;
                string strSubj = string.Empty;
                dtDescView = new DataView(dtItemList);
                dtDescView.Sort = "category ASC";
                DataTable distinctCategory = dtDescView.ToTable(true, "category");

                dtDescView = new DataView(dtItemList);
                dtDescView.Sort = "brand ASC";
                DataTable distinctBrand = dtDescView.ToTable(true, "brand", "category");
                foreach (DataRow rowCategory in distinctCategory.Rows)
                {
                    foreach (DataRow row in distinctBrand.Select("category='" + rowCategory["category"].ToString() + "'"))
                    {
                        if (strBrandSubj.Length > 0)
                            strBrandSubj += ",'" + row["brand"].ToString() + "'";
                        else
                            strBrandSubj += "'" + row["brand"].ToString() + "'";
                    }
                    if (strBrandSubj.Length > 0)
                    {
                        if (rowCategory["category"].ToString() == "SOLID" || rowCategory["category"].ToString() == "PNEUMATIC")
                            strCategory = strSubj == "" ? "INDUSTRIAL " + rowCategory["category"].ToString() + " TYRES OF " + strBrandSubj + " BRAND(S)" : " AND INDUSTRIAL " + rowCategory["category"].ToString() + " TYRES OF " + strBrandSubj + " BRAND(S)";
                        else if (rowCategory["category"].ToString() == "POB")
                            strCategory = strSubj == "" ? "PRESS ON WHEELS OF " + strBrandSubj + " BRAND(S)" : " AND PRESS ON WHEELS OF " + strBrandSubj + " BRAND(S)";
                        else if (rowCategory["category"].ToString() == "SPLIT RIMS")
                            strCategory = strSubj == "" ? "SPLIT RIMS" : " AND SPLIT RIMS";
                        else
                            strCategory = strSubj == "" ? "INDUSTRIAL GOODS " : " AND INDUSTRIAL GOODS ";
                    }
                    if (strCategory.Length > 0)
                        strSubj += strCategory;
                    strBrandSubj = "";
                }
                if (strSubj.Length > 0)
                {
                    var subFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                    string strCell = "\nWE HAVE PLEASURE IN RELEASING OUR PROFORMA ON YOUR COMPANY FOR " + strSubj + "\n\n";
                    PdfPCell nest1 = new PdfPCell(new Phrase(strCell, subFont));
                    nest1.Padding = 0f;
                    nest1.Colspan = 2;
                    nest1.Border = Rectangle.NO_BORDER;
                    table.AddCell(nest1);
                }

                PdfPCell nesthousing1 = new PdfPCell(dt);
                nesthousing1.Padding = 0f;
                nesthousing1.Colspan = 2;
                table.AddCell(nesthousing1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_ProformaBottomDetails(PdfPTable table)
        {
            try
            {
                PdfPTable termsTd = new PdfPTable(2);
                termsTd.TotalWidth = 560f;
                termsTd.LockedWidth = true;
                float[] widths = new float[] { 15f, 15f };
                termsTd.SetWidths(widths);
                var titleFont = FontFactory.GetFont("Arial", 8, Font.UNDERLINE);
                Chunk p1 = new Chunk("TERMS & CONDITION\n", titleFont);

                string strTerms = string.Empty;
                if (txtPayterms.Text != "")
                {
                    strTerms = "\n" + Server.HtmlDecode(txtPayterms.Text.Replace("\r\n", "\n"));
                }
                titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                Chunk p2 = new Chunk(strTerms, titleFont);
                Paragraph para = new Paragraph();
                para.Add(p1);
                para.Add(p2);
                PdfPCell cell = new PdfPCell(para);
                cell.Colspan = 2;
                termsTd.AddCell(cell);

                titleFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                //cell = new PdfPCell(new Phrase("For T.S.RAJAM TYRES PRIVATE LIMITED\n \n \n \n \nAuthorised Signatory", titleFont));
                cell = new PdfPCell(new Phrase("For SUNDARAM INDUSTRIES PRIVATE LIMITED (Formerly Known as T.S.RAJAM TYRES PRIVATE LIMITED) \n \n \n \n \nAuthorised Signatory", titleFont));//********************To be inserted on 14-02-2022*******************
                cell.Padding = 0f;
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = 1.5f;
                termsTd.AddCell(cell);

                cell = new PdfPCell(new Phrase("    ACCEPTED    \n For " + (hdnCustCode.Value == "2642" ? hdnAuthorized.Value : lblCustName.Text) + "\n \n \n \n Authorised Signatory", titleFont));
                cell.Padding = 0f;
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.PaddingBottom = 1.5f;
                termsTd.AddCell(cell);

                PdfPCell nesthousingTerms = new PdfPCell(termsTd);
                nesthousingTerms.Padding = 0f;
                nesthousingTerms.Colspan = 2;
                nesthousingTerms.Border = Rectangle.NO_BORDER;
                table.AddCell(nesthousingTerms);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private PdfPCell Build_ItemTableHeadStyle(string strText)
        {
            var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }
        private PdfPCell Build_ItemTableListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.Border = Rectangle.RIGHT_BORDER;
            return cell;
        }
        private PdfPCell Build_ItemTableListStyleSameQty(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.Border = Rectangle.RIGHT_BORDER;
            cell.Rowspan = 2;
            return cell;
        }
        private PdfPCell Build_ItemTableBottomStyle(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            return cell;
        }
        protected void btnSaveChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                if (hdnRequestStatusID.Value == "14" || hdnRequestStatusID.Value == "16" || hdnRequestStatusID.Value == "28")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@RequestStatus", hdnRequestStatusID.Value) 
                    };
                    resp = daCOTS.ExecuteNonQuery_SP("sp_upd_exp_proforma_completed", sp);
                }
                else
                {
                    SqlParameter[] sp1 = new SqlParameter[]{
                        new SqlParameter("@O_ID", hdnOID.Value),
                        new SqlParameter("@statusid", lblCurrStatus.Text == "ORDER RECEIVED" ? Convert.ToInt32(2) : Convert.ToInt32(23)),
                        new SqlParameter("@feedback", txtOrderChangeComments.Text.Replace("\r\n", "~")),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                    };
                    resp = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                }
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}