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
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace TTS
{
    public partial class cotsftinvoice : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_invoice"].ToString() == "True")
                        {
                            lblHeadPlant.Text = Request["pid"].ToString().ToUpper();
                            gvFtList.DataSource = null;
                            gvFtList.DataBind();
                            SqlParameter[] sp = new SqlParameter[1];
                            sp[0] = new SqlParameter("@Plant", lblHeadPlant.Text);
                            DataTable dtFtlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FtMasterDetailsForInvoice", sp, DataAccess.Return_Type.DataTable);
                            if (dtFtlist.Rows.Count > 0)
                            {
                                gvFtList.DataSource = dtFtlist;
                                gvFtList.DataBind();
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
        protected void lnkInvoiceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                clearFields();
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectInvoiceFt(clickedRow);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        void clearFields()
        {
            rdbDeliveryMethod.ClearSelection();
            rdbLocation.ClearSelection();
            rdbDeliveryTo.ClearSelection();
            txtContactPerson.Text = "";
            txtContactNo.Text = "";
            txtComments.Text = "";
            chkCGST.Checked = false;
            txtCGST.Text = "0.00";
            lblCGST.Text = "";
            chkSGST.Checked = false;
            txtSGST.Text = "0.00";
            lblSGST.Text = "";
            chkIGST.Checked = false;
            txtIGST.Text = "0.00";
            lblIGST.Text = "";
            lblTotTaxValue.Text = "";
            lblGrandTotal.Text = "";
            txtTrasporterName.Text = "";
            txtLrDate.Text = "";
            txtLrNo.Text = "";

        }
        protected void btnpricelist_Click(object sender, EventArgs e)
        {
            try
            {
                Bind_RateDetails();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow", "gotoPreviewDiv('divStatusChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectInvoiceFt(GridViewRow row)
        {
            try
            {
                Label lblFtNo = (Label)row.FindControl("lblFtNo");
                HiddenField hdnCustCode = (HiddenField)row.FindControl("hdnCustCode");
                HiddenField hdnBillId = (HiddenField)row.FindControl("hdnBillId");
                Label lblStatusCustName = (Label)row.FindControl("lblStatusCustName");
                HiddenField hdnTaxNo = (HiddenField)row.FindControl("hdnTaxNo");
                HiddenField hdnPanno = (HiddenField)row.FindControl("hdnPanno");
                hdnCotsCustID.Value = hdnCustCode.Value;
                lblCustName.Text = lblStatusCustName.Text;
                lblStausOrderRefNo.Text = lblFtNo.Text;
                lbltaxno.Text = hdnTaxNo.Value == "" ? "" : "<span class='headCss'>CUSTOMER SERVICE TAX NO: </span>" + hdnTaxNo.Value;
                lblPanno.Text = hdnPanno.Value == "" ? "" : "<span class='headCss'>CUSTOMER PAN NO: </span>" + hdnPanno.Value;
                hdnBillID.Value = hdnBillId.Value;

                SqlParameter[] spBillAddress = new SqlParameter[2];
                spBillAddress[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                spBillAddress[1] = new SqlParameter("@shipid", hdnBillID.Value);
                DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_CotsFtInvoice_Address", spBillAddress, DataAccess.Return_Type.DataTable);
                ViewState["BillAddress"] = dtBillAddress;
                if (dtBillAddress.Rows.Count > 0)
                {
                    lblbillTo.Text = Get_Address_CustChoice(dtBillAddress);
                    ViewState["BillAddress"] = dtBillAddress;

                    DataRow dr = dtBillAddress.Rows[0];
                    if (dr["CGST"].ToString() != "") txtCGST.Text = Convert.ToDecimal(dr["CGST"].ToString()) > 0 ? Convert.ToDecimal(dr["CGST"].ToString()).ToString("0.00") : "0.00";
                    if (dr["SGST"].ToString() != "") txtSGST.Text = Convert.ToDecimal(dr["SGST"].ToString()) > 0 ? Convert.ToDecimal(dr["SGST"].ToString()).ToString("0.00") : "0.00";
                    if (dr["IGST"].ToString() != "") txtIGST.Text = Convert.ToDecimal(dr["IGST"].ToString()) > 0 ? Convert.ToDecimal(dr["IGST"].ToString()).ToString("0.00") : "0.00";
                    hdnCGSTPer.Value = txtCGST.Text;
                    hdnSGSTPer.Value = txtSGST.Text;
                    hdnIGSTPer.Value = txtIGST.Text;
                }

                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@BillId", Convert.ToInt32(hdnBillId.Value));
                sp1[2] = new SqlParameter("@Plant", lblHeadPlant.Text);
                DataTable dtotherFtNo = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FtMasterDetailsForInvoice_customer", sp1, DataAccess.Return_Type.DataTable);
                if (dtotherFtNo.Rows.Count > 0)
                {
                    if (dtotherFtNo.Rows[0]["GST_No"].ToString() != "") hdnGSTINNo.Value = dtotherFtNo.Rows[0]["GST_No"].ToString();
                    gvOtherFtlist.DataSource = dtotherFtNo;
                    gvOtherFtlist.DataBind();

                    foreach (GridViewRow gr in gvOtherFtlist.Rows)
                    {
                        CheckBox chkftno = gr.FindControl("chkftno") as CheckBox;
                        Label lblFtNog = gr.FindControl("lblFtNo") as Label;
                        if (lblFtNog.Text == lblStausOrderRefNo.Text)
                        {
                            chkftno.Checked = true;
                            chkftno.Enabled = false;
                            if (dtotherFtNo.Rows.Count == 1)
                            {
                                Bind_RateDetails();
                                btnpricelist.Visible = false;
                            }
                            else
                                btnpricelist.Visible = true;
                            break;
                        }
                    }
                }

                gvdispatchitem.DataSource = null;
                gvdispatchitem.DataBind();
                sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@FtNo", lblFtNo.Text);
                sp1[2] = new SqlParameter("@Plant", lblHeadPlant.Text);
                DataTable dtFtNoDisList = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_DispatchedItemList_FtNoWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtFtNoDisList.Rows.Count > 0)
                {
                    gvdispatchitem.DataSource = dtFtNoDisList;
                    gvdispatchitem.DataBind();

                    DataView dtView = new DataView(dtFtNoDisList);
                    dtView.Sort = "Invoiceno";
                    DataTable distinct = dtView.ToTable(true, "Invoiceno", "InvoiceYear", "Plant", "ContactPerson", "Contactno", "DeliveryTo", "DispatchMethod", "GoDown", "Location", "LrDate", "LrNo", "Transpoter", "VehicleNo", "Comments", "GrandTotal", "servicetax", "Total", "TotalQty", "ServicetaxPercent", "CGST", "SGST", "IGST", "CGSTVal", "SGSTVal", "IGSTVal");
                    if (distinct.Rows.Count > 0)
                    {
                        gvFtInvoiceDownload.DataSource = distinct;
                        gvFtInvoiceDownload.DataBind();
                    }
                    lblAlreadyPreapre.Text = "ALREADY DISPATCHED INVOICE DETAILS FOR FITMENT ORDER NO: " + lblFtNo.Text; ;
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkInvoiceFile_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblDownloadInvoiceNo = clickedRow.FindControl("lblDownloadInvoiceNo") as Label;
            HiddenField hdnDownloadUInvoiceYear = clickedRow.FindControl("hdnDownloadUInvoiceYear") as HiddenField;

            LinkButton lnkTxt = sender as LinkButton;
            string serverURL = Server.MapPath("~/ftinvoicefiles/" + hdnCotsCustID.Value + "/" + lblHeadPlant.Text + "/").Replace("TTS", "pdfs");
            string spdfPathTo = serverURL + lnkTxt.ID + hdnDownloadUInvoiceYear.Value + "_" + lblDownloadInvoiceNo.Text + ".pdf";

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.ID + hdnDownloadUInvoiceYear.Value + "_" + lblDownloadInvoiceNo.Text + ".pdf");
            Response.WriteFile(spdfPathTo);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        private void Bind_RateDetails()
        {
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn("Ftno", typeof(System.String));
            dt.Columns.Add(col);
            col = new DataColumn("tyresize", typeof(System.String));
            dt.Columns.Add(col);
            col = new DataColumn("ReceivedQty", typeof(System.Int32));
            dt.Columns.Add(col);
            col = new DataColumn("Rate", typeof(System.Decimal));
            dt.Columns.Add(col);
            col = new DataColumn("AvaQty", typeof(System.Int32));
            dt.Columns.Add(col);
            col = new DataColumn("DispachQty", typeof(System.Int32));
            dt.Columns.Add(col);
            col = new DataColumn("BalanceQty", typeof(System.Int32));
            dt.Columns.Add(col);
            ViewState["ItemList"] = dt;
            SqlParameter[] sp1 = new SqlParameter[2];
            sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
            sp1[1] = new SqlParameter("@Plant", lblHeadPlant.Text);
            DataTable dtFtList = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_ftitemlist", sp1, DataAccess.Return_Type.DataTable);
            if (dtFtList.Rows.Count > 0)
            {
                foreach (GridViewRow gr in gvOtherFtlist.Rows)
                {
                    CheckBox chkftno = gr.FindControl("chkftno") as CheckBox;
                    Label lblFtNo = gr.FindControl("lblFtNo") as Label;
                    if (chkftno.Checked == true)
                    {
                        foreach (DataRow dr in dtFtList.Select("FtNo='" + lblFtNo.Text + "'"))
                        {
                            dt.Rows.Add(dr["FtNo"], dr["TyreSize"], dr["ReceivedQty"], dr["Rate"], dr["AvaQty"], 0, 0);
                        }
                    }
                } if (dt.Rows.Count > 0)
                {
                    gvItemlist.DataSource = dt;
                    gvItemlist.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "gotoPreviewDiv('divInvoiceDetails');", true);
                }
            }
        }
        private string Get_Address_CustChoice(DataTable dt)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                DataRow row = dt.Rows[0];
                strApp.Append("<span style='line-height: 13px;float: left;width: 500px;'>");
                strApp.Append("<span style='float: left;width: 500px;'><span style='font-weight:bold;'>M/S. " + row["CompanyName"].ToString().ToUpper() + "</span></span>");
                strApp.Append("<span style='float: left;width: 500px;word-break: break-word;'>" + row["shipaddress"].ToString().Replace("~", "&nbsp;&nbsp;") + "</span>");
                strApp.Append("<span style='float: left;width: 500px;'>" + row["city"].ToString() + "    " + row["statename"].ToString() + "</span>");
                strApp.Append("<span>" + row["country"].ToString() + "    " + row["zipcode"].ToString() + "</span>");
                strApp.Append("</span>");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }
        protected void btnInvoiceprepare_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ViewState["ItemList"] as DataTable;
                dt.Clear();
                foreach (GridViewRow gr in gvItemlist.Rows)
                {
                    TextBox txtRate = gr.FindControl("txtRate") as TextBox;
                    TextBox txtcurrentDispatch = gr.FindControl("txtcurrentDispatch") as TextBox;
                    Label lblFtno = gr.FindControl("lblFtno") as Label;
                    Label lblRecQty = gr.FindControl("lblRecQty") as Label;
                    Label lblAvaQty = gr.FindControl("lblAvaQty") as Label;
                    Label lbltyresize = gr.FindControl("lbltyresize") as Label;
                    HiddenField hdnbalQty = gr.FindControl("hdnbalQty") as HiddenField;
                    dt.Rows.Add(lblFtno.Text, lbltyresize.Text, lblRecQty.Text, txtRate.Text, lblAvaQty.Text, txtcurrentDispatch.Text, hdnbalQty.Value);

                }
                if (dt.Rows.Count > 0)
                {
                    DataTable dt1 = new DataTable();
                    DataColumn col = new DataColumn("Ftno", typeof(System.String));
                    dt1.Columns.Add(col);
                    col = new DataColumn("BalanceQty", typeof(System.Int32));
                    dt1.Columns.Add(col);
                    DataView dtView = new DataView(dt);
                    dtView.Sort = "Ftno";
                    DataTable distinct = dtView.ToTable(true, "Ftno");
                    string strInvoiceFtNo = string.Empty;
                    foreach (DataRow dr in distinct.Rows)
                    {
                        int total = 0;
                        foreach (DataRow row in dt.Select("Ftno='" + dr["Ftno"].ToString() + "'"))
                        {
                            total += Convert.ToInt32(row["BalanceQty"].ToString());
                        }
                        dt1.Rows.Add(dr["Ftno"].ToString(), total);

                        if (strInvoiceFtNo.Length == 0)
                            strInvoiceFtNo = dr["Ftno"].ToString();
                        else
                            strInvoiceFtNo += "~" + dr["Ftno"].ToString();
                    }

                    hdnyear.Value = Build_InvoiceYear();
                    SqlParameter[] sp1 = new SqlParameter[18];
                    sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                    sp1[1] = new SqlParameter("@DispatchMethod", rdbDeliveryMethod.SelectedValue);
                    sp1[2] = new SqlParameter("@Location", rdbDeliveryMethod.SelectedValue == "SENT TO" ? rdbLocation.SelectedValue : "");
                    sp1[3] = new SqlParameter("@DeliveryTo", rdbDeliveryMethod.SelectedValue == "SENT TO" ? rdbDeliveryTo.SelectedValue : "");
                    sp1[4] = new SqlParameter("@GoDown", txtGodownname.Text.Trim());
                    sp1[5] = new SqlParameter("@Transpoter", txtTrasporterName.Text.Trim());
                    sp1[6] = new SqlParameter("@VehicleNo", txtVechicleNo.Text.Trim());
                    sp1[7] = new SqlParameter("@ContactPerson", txtContactPerson.Text.Trim());
                    sp1[8] = new SqlParameter("@Contactno", txtContactNo.Text.Trim());
                    sp1[9] = new SqlParameter("@LrNo", txtLrNo.Text);
                    sp1[10] = new SqlParameter("@LrDate", txtLrDate.Text);
                    sp1[11] = new SqlParameter("@CreatedBy", Request.Cookies["TTSUser"].Value);
                    sp1[12] = new SqlParameter("@FtDispatchedItem_datatable", dt);
                    sp1[13] = new SqlParameter("@FtBalance_datatable", dt1);
                    sp1[14] = new SqlParameter("@Plant", lblHeadPlant.Text);
                    sp1[15] = new SqlParameter("@InvoiceFtNo", strInvoiceFtNo);
                    sp1[16] = new SqlParameter("@Comments", txtComments.Text.Replace("\r\n", "~"));
                    sp1[17] = new SqlParameter("@InvoiceYear", hdnyear.Value);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_FtInvoiceDetails", sp1);
                    if (resp > 0)
                    {
                        invoice_prepare();
                        SqlParameter[] sp = new SqlParameter[14];
                        sp[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                        sp[1] = new SqlParameter("@Invoiceno", hdnInvoice.Value);
                        sp[2] = new SqlParameter("@Plant", lblHeadPlant.Text);
                        sp[3] = new SqlParameter("@GrandTotal", Convert.ToDecimal(hdnGrandTotal.Value != "" ? hdnGrandTotal.Value : "0"));
                        sp[4] = new SqlParameter("@Total", Convert.ToDecimal(hdntotal.Value != "" ? hdntotal.Value : "0"));
                        sp[5] = new SqlParameter("@TotalQty", Convert.ToInt32(hdntotalqty.Value != "" ? hdntotalqty.Value : "0"));
                        sp[6] = new SqlParameter("@ServicetaxPercent", Convert.ToDecimal("15.00"));
                        sp[7] = new SqlParameter("@InvoiceYear", hdnyear.Value);
                        sp[8] = new SqlParameter("@CGST", hdnCGSTPer.Value);
                        sp[9] = new SqlParameter("@SGST", hdnSGSTPer.Value);
                        sp[10] = new SqlParameter("@IGST", hdnIGSTPer.Value);
                        sp[11] = new SqlParameter("@CGSTVal", hdnCGSTVal.Value);
                        sp[12] = new SqlParameter("@SGSTVal", hdnSGSTVal.Value);
                        sp[13] = new SqlParameter("@IGSTVal", hdnIGSTVal.Value);
                        daCOTS.ExecuteNonQuery_SP("sp_update_FtInvoiceDetails", sp);

                        Response.Redirect("cotsfttrack.aspx?aid=inv&pid=" + lblHeadPlant.Text.ToLower() + "&cid=" + hdnInvoice.Value, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private string Build_InvoiceYear()
        {
            string acYearFormat = string.Empty;
            if (DateTime.Today.Month < 4)
            {
                string nextyear = DateTime.Today.Year.ToString();
                acYearFormat = (DateTime.Today.Year - 1).ToString() + '-' + nextyear.Substring(2, 2);
            }
            else if (DateTime.Today.Month > 3)
            {
                string nextyear = (DateTime.Today.Year + 1).ToString();
                acYearFormat = DateTime.Today.Year.ToString() + '-' + nextyear.Substring(2, 2);
            }
            return acYearFormat;
        }
        private void invoice_prepare()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@Plant", lblHeadPlant.Text);
                sp1[2] = new SqlParameter("@InvoiceYear", hdnyear.Value);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_IvoiceDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    hdnInvoice.Value = dt.Rows[0]["Invoiceno"].ToString();
                    ViewState["Invoicedetails"] = dt;
                }
                string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/ftinvoicefiles/" + hdnCotsCustID.Value + "/" + lblHeadPlant.Text + "/"))
                    Directory.CreateDirectory(serverURL + "/ftinvoicefiles/" + hdnCotsCustID.Value + "/" + lblHeadPlant.Text + "/");
                string path = serverURL + "/ftinvoicefiles/" + hdnCotsCustID.Value + "/" + lblHeadPlant.Text + "/";
                string strWaterMark = string.Empty; string strfileAdditional = string.Empty; string sPathToWritePdfTo = string.Empty;
                //strfileAdditional = "Original_invoice_";
                strfileAdditional = "Original_Recepient_Invoice";
                strWaterMark = "ORIGINAL FOR RECEPIENT";
                sPathToWritePdfTo = path + strfileAdditional + hdnyear.Value + "_" + hdnInvoice.Value + ".pdf";
                Build_AllInvoices(sPathToWritePdfTo, strWaterMark, strfileAdditional);
                //strfileAdditional = "Extra_copy_invoice_";
                strfileAdditional = "Duplicate_Transporter_Invoice";
                strWaterMark = "DUPLICATE FOR TRANSPORTER";
                sPathToWritePdfTo = path + strfileAdditional + hdnyear.Value + "_" + hdnInvoice.Value + ".pdf";
                Build_AllInvoices(sPathToWritePdfTo, strWaterMark, strfileAdditional);
                //strfileAdditional = "Assessee_invoice_";
                strfileAdditional = "Triplicate_Supplier_Invoice";
                strWaterMark = "TRIPLICATE FOR SUPPLIER";
                sPathToWritePdfTo = path + strfileAdditional + hdnyear.Value + "_" + hdnInvoice.Value + ".pdf";
                Build_AllInvoices(sPathToWritePdfTo, strWaterMark, strfileAdditional);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_AllInvoices(string sPathTo, string WaterMark, string fileAdditional)
        {
            try
            {
                Document document = new Document(PageSize.A4, 18f, 2f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sPathTo, FileMode.Create));
                writer.PageEvent = new PDFWriterEvents(WaterMark, 50f);
                document.Open();
                document.Add(Build_InvoiceDetails(document, WaterMark));
                document.Close();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private PdfPTable Build_InvoiceDetails(Document doc, string strFileHead)
        {
            PdfPTable table = new PdfPTable(3);
            var contentFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            float[] widths = new float[] { 11.5f, 8f, 10f };
            table.SetWidths(widths);
            table.AddCell(Build_HeadTableColsPan("\n\n", 0, 3));
            Build_AddressChildTable(table);
            Build_PdfHeadingTable(table, strFileHead);
            Build_InvoiceRecordTable(table);
            Build_CustAddressDetailsTable(table);
            Build_InvoiceOrderItems(table);
            Build_InvoiceBottomDetails(table);
            return table;
        }
        private void Build_AddressChildTable(PdfPTable table)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPTable nested = new PdfPTable(1);
            string AddressTxt = string.Empty;
            //Registration Address
            var headFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            //Chunk postCell1 = new Chunk("T.S.RAJAM TYRES PRIVATE LIMITED\n", headFont);
             Chunk postCell1 = new Chunk("SUNDARAM INDUSTRIES PRIVATE LIMITED\n", headFont);//*************To Be inserted  On 14-02-2022***********************
             var subHead = FontFactory.GetFont("Arial",6, Font.BOLD);             
             Chunk postCell2 = new Chunk("(Formerly known as T.S.RAJAM TYRES PRIVATE LIMITED.)\n", subHead);
            DataRow dtRow = Utilities.Get_PlantAddress(lblHeadPlant.Text);
            AddressTxt = dtRow["PlantAddress"].ToString().Replace("~", "\n");
            Chunk postCell3 = new Chunk(AddressTxt, titleFont);
            Phrase para = new Phrase();
            para.Add(postCell1);
            para.Add(postCell2);
            para.Add(postCell3);
            PdfPCell cell = new PdfPCell(new Phrase(para));
            cell.ExtraParagraphSpace = 2f;
            nested.AddCell(cell);
            PdfPCell nesthousing = new PdfPCell(nested);
            table.AddCell(nesthousing);
        }
        private void Build_PdfHeadingTable(PdfPTable table, string strInvoiceFileHead)
        {
            PdfPTable nested = new PdfPTable(1);
            //Heading
            var headFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Chunk cellTot;
            cellTot = new Chunk("FITMENT-INVOICE", headFont);
            Phrase para = new Phrase();
            para.Add(cellTot);
            PdfPCell cell = new PdfPCell(new Phrase(para));
            cell.HorizontalAlignment = 
                
                
                1;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            nested.AddCell(cell);

            var titleBoldFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            cell = new PdfPCell(new Phrase(strInvoiceFileHead, titleBoldFont));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            nested.AddCell(cell);

            cell = new PdfPCell(new Phrase("PAN       : AAGCT6465R", titleBoldFont));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT;
            cell.Border = Rectangle.NO_BORDER;
            nested.AddCell(cell);

            cell = new PdfPCell(new Phrase("GSTIN NO : 33AAGCT6465R2ZJ", titleBoldFont));// 33AABCS5320H3ZP
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
            nested.AddCell(cell);

            PdfPCell nesthousing1 = new PdfPCell(nested);
            nesthousing1.Padding = 0f;
            table.AddCell(nesthousing1);
        }
        private void Build_InvoiceRecordTable(PdfPTable table)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            var valueFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            //Heading
            PdfPTable nested1 = new PdfPTable(2);
            nested1.TotalWidth = 188f;
            nested1.LockedWidth = true;
            //                             
            float[] widths = new float[] { 2.8f, 4.2f };
            nested1.SetWidths(widths);

            string imageFilePath = Server.MapPath("~/images/tvs_suntws.jpg");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageFilePath);
            img.ScaleToFit(165f, 39f);
            PdfPCell cell1 = new PdfPCell(img);
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_TOP;
            cell1.Colspan = 2;
            cell1.Padding = 0f;
            cell1.PaddingTop = 1f;
            cell1.PaddingBottom = 1f;
            nested1.AddCell(cell1);

            var invoiceFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase("\nInvoice No. ", invoiceFont));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.Border = 0;
            nested1.AddCell(cell);
            invoiceFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            cell = new PdfPCell(new Phrase("\n: " + hdnInvoice.Value, invoiceFont));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.Border = 0;
            nested1.AddCell(cell);

            cell = new PdfPCell(new Phrase("Invoice Date\n ", titleFont));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.Border = 0;
            nested1.AddCell(cell);

            cell = new PdfPCell(new Phrase(": " + DateTime.Now.ToString("dd/MM/yyyy"), valueFont));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.Border = 0;
            nested1.AddCell(cell);

            PdfPCell nesthousing = new PdfPCell(nested1);
            nesthousing.HorizontalAlignment = Element.ALIGN_CENTER;
            nesthousing.VerticalAlignment = Element.ALIGN_TOP;
            nesthousing.Padding = 0f;
            table.AddCell(nesthousing);
        }
        private void Build_CustAddressDetailsTable(PdfPTable table)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPTable nested1 = new PdfPTable(2);
            nested1.TotalWidth = 560f;
            nested1.LockedWidth = true;
            //                             
            float[] widths = new float[] { 16.7f, 12f };
            nested1.SetWidths(widths);

            //Customer Address
            PdfPTable nested2 = new PdfPTable(3);
            nested2.SetWidths(new float[] { 4f, 0.5f, 12f });
            DataTable dtBillAddress = (DataTable)ViewState["BillAddress"];
            if (dtBillAddress.Rows.Count > 0)
            {
                var normalFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                DataRow row = dtBillAddress.Rows[0];

                // Colon Cell " colon between Heading and value" eg NAME : Value  -  declare as common and use at required place.. 
                PdfPCell CellColon = new PdfPCell(new Phrase(":", titleFont));
                CellColon.HorizontalAlignment = 0;
                CellColon.VerticalAlignment = Element.ALIGN_TOP;
                CellColon.Border = Rectangle.NO_BORDER;

                PdfPCell Cell1 = new PdfPCell(new Phrase("FITMENT SERVICE PROVIDED TO:", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                Cell1.Colspan = 3;
                nested2.AddCell(Cell1);

                Cell1 = new PdfPCell(new Phrase("NAME", normalFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                // cellcolon is declared common and used in required place.
                nested2.AddCell(new PdfPCell(CellColon));

                Cell1 = new PdfPCell(new Phrase("M/S." + row["CompanyName"].ToString() + "", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                Cell1 = new PdfPCell(new Phrase("ADDRESS", normalFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                nested2.AddCell(new PdfPCell(CellColon));

                Cell1 = new PdfPCell(new Phrase(row["shipaddress"].ToString().Replace("~", "\n"), titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                Cell1 = new PdfPCell(new Phrase("CITY", normalFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                nested2.AddCell(new PdfPCell(CellColon));

                Cell1 = new PdfPCell(new Phrase(row["city"].ToString(), titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                Cell1 = new PdfPCell(new Phrase("STATE / CODE", normalFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                nested2.AddCell(new PdfPCell(CellColon));

                Cell1 = new PdfPCell(new Phrase(row["statename"].ToString() + " / " + row["stateCode"].ToString(), titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);


                Cell1 = new PdfPCell(new Phrase("PINCODE", normalFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                nested2.AddCell(new PdfPCell(CellColon));

                Cell1 = new PdfPCell(new Phrase(row["zipcode"].ToString(), titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                Cell1 = new PdfPCell(new Phrase("GSTIN NO", normalFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = PdfPCell.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                nested2.AddCell(new PdfPCell(CellColon));

                Cell1 = new PdfPCell(new Phrase(hdnGSTINNo.Value, titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested2.AddCell(Cell1);

                PdfPCell nesthousing = new PdfPCell(nested2);
                nesthousing.Padding = 0f;
                nested1.AddCell(nesthousing);
            }

            PdfPTable nested3 = new PdfPTable(3);

            float[] width = new float[] { 12f, 0.5f, 12f };
            nested3.SetWidths(width);

            DataTable dt = ViewState["Invoicedetails"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                var titleFont2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
                PdfPCell Cell1 = new PdfPCell(new Phrase(row["DispatchMethod"].ToString().ToUpper() + " CUSTOMER", titleFont2));
                Cell1.HorizontalAlignment = 0;
                Cell1.Colspan = 3;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["DispatchMethod"].ToString() == "COLLECTED BY" ? "NAME OF PERSON" : "CONTACT PERSON", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(":", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["ContactPerson"].ToString(), titleFont2));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase("MOBILE NO.", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(":", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["Contactno"].ToString(), titleFont2));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);

                if (row["DispatchMethod"].ToString().ToUpper() == "SENT TO")
                {
                    Cell1 = new PdfPCell(new Phrase("TRANSPORTER NAME", titleFont));
                    Cell1.HorizontalAlignment = 0;
                    Cell1.VerticalAlignment = Element.ALIGN_TOP;
                    Cell1.Border = Rectangle.NO_BORDER;
                    nested3.AddCell(Cell1);
                    Cell1 = new PdfPCell(new Phrase(":", titleFont));
                    Cell1.HorizontalAlignment = 0;
                    Cell1.VerticalAlignment = Element.ALIGN_TOP;
                    Cell1.Border = Rectangle.NO_BORDER;
                    nested3.AddCell(Cell1);
                    Cell1 = new PdfPCell(new Phrase(row["Transpoter"].ToString(), titleFont2));
                    Cell1.HorizontalAlignment = 0;
                    Cell1.VerticalAlignment = Element.ALIGN_TOP;
                    Cell1.Border = Rectangle.NO_BORDER;
                    nested3.AddCell(Cell1);
                    titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                    string DispatchMethod = string.Empty;
                    Cell1 = new PdfPCell(new Phrase("DELIVERY TO", titleFont));
                    Cell1.HorizontalAlignment = 0;
                    Cell1.VerticalAlignment = Element.ALIGN_TOP;
                    Cell1.Border = Rectangle.NO_BORDER;
                    nested3.AddCell(Cell1);
                    Cell1 = new PdfPCell(new Phrase(":", titleFont));
                    Cell1.HorizontalAlignment = 0;
                    Cell1.VerticalAlignment = Element.ALIGN_TOP;
                    Cell1.Border = Rectangle.NO_BORDER;
                    nested3.AddCell(Cell1);
                    DispatchMethod += row["DeliveryTo"].ToString();
                    if (row["DeliveryTo"].ToString() == "GODOWN") DispatchMethod += "-" + row["GoDown"].ToString() + "\n";
                    Cell1 = new PdfPCell(new Phrase(DispatchMethod, titleFont2));
                    Cell1.HorizontalAlignment = 0;
                    Cell1.VerticalAlignment = Element.ALIGN_TOP;
                    Cell1.Border = Rectangle.NO_BORDER;
                    nested3.AddCell(Cell1);
                }
                Cell1 = new PdfPCell(new Phrase(row["VehicleNo"].ToString() == "" ? "" : "VEHICLE REG NO.", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["VehicleNo"].ToString() == "" ? "" : ": ", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["VehicleNo"].ToString() == "" ? "" : row["VehicleNo"].ToString(), titleFont2));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["LrNo"].ToString() == "" ? "" : "LR NO.", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["LrNo"].ToString() == "" ? "" : ": ", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["LrNo"].ToString() == "" ? "" : row["LrNo"].ToString(), titleFont2));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["LrDate"].ToString() == "" ? "" : "LR DATE", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["LrDate"].ToString() == "" ? "" : ": ", titleFont));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                Cell1 = new PdfPCell(new Phrase(row["LrDate"].ToString() == "" ? "" : row["LrDate"].ToString(), titleFont2));
                Cell1.HorizontalAlignment = 0;
                Cell1.VerticalAlignment = Element.ALIGN_TOP;
                Cell1.Border = Rectangle.NO_BORDER;
                nested3.AddCell(Cell1);
                PdfPCell nesthousing = new PdfPCell(nested3);
                nesthousing.Padding = 0f;
                nesthousing.Rowspan = 2;
                nesthousing.Border = Rectangle.NO_BORDER;
                nested1.AddCell(nesthousing);
            }

            SqlParameter[] sp2 = new SqlParameter[4];
            sp2[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
            sp2[1] = new SqlParameter("@Invoiceno", hdnInvoice.Value);
            sp2[2] = new SqlParameter("@Plant", lblHeadPlant.Text);
            sp2[3] = new SqlParameter("@InvoiceYear", hdnyear.Value);
            DataTable dt2 = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_FtMaster_Details", sp2, DataAccess.Return_Type.DataTable);
            PdfPCell cell;
            if (dt2.Rows.Count > 0)
            {
                var titleFont2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
                string FTNO = string.Empty, NoOfRecRim = string.Empty, Challan = string.Empty, date = string.Empty, CreateDate = string.Empty;
                string PanNo = string.Empty;
                foreach (DataRow dr in dt2.Rows)
                {
                    FTNO += FTNO == "" ? dr["FtNo"].ToString() : " / " + dr["FtNo"].ToString();
                    NoOfRecRim += NoOfRecRim == "" ? dr["ReceivedQty"].ToString() : " / " + dr["ReceivedQty"].ToString();
                    Challan += Challan == "" ? dr["ChallanRefno"].ToString() : " / " + dr["ChallanRefno"].ToString();
                    date += date == "" ? dr["ChallanDate"].ToString() : " / " + dr["ChallanDate"].ToString();
                    CreateDate += CreateDate == "" ? dr["CreateDate"].ToString() : " / " + dr["CreateDate"].ToString();
                    PanNo += PanNo == "" ? dr["PanNo"].ToString() : " / " + dr["PanNo"].ToString();
                }

                // Colon Cell " colon between Heading and value" eg NAME : Value  -  declare as common and use at required place.. 
                PdfPCell CellColon = new PdfPCell(new Phrase(":", titleFont));
                CellColon.HorizontalAlignment = 0;
                CellColon.VerticalAlignment = Element.ALIGN_TOP;
                CellColon.Border = Rectangle.NO_BORDER;

                PdfPTable nestedOrder = new PdfPTable(3);
                float[] width1 = new float[] { 8.5f, 0.5f, 12.5f };
                nestedOrder.SetWidths(width1);
                cell = new PdfPCell(new Phrase("FITMENT ORDER NO.", titleFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);

                nestedOrder.AddCell(new PdfPCell(CellColon));

                cell = new PdfPCell(new Phrase(FTNO, titleFont2));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);
                cell = new PdfPCell(new Phrase("ORDER RECEIVED DATE", titleFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);

                nestedOrder.AddCell(new PdfPCell(CellColon));

                cell = new PdfPCell(new Phrase(CreateDate, titleFont2));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);

                cell = new PdfPCell(new Phrase("CHALLAN / GATE PASS NO", titleFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);

                nestedOrder.AddCell(new PdfPCell(CellColon));

                cell = new PdfPCell(new Phrase(Challan, titleFont2));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);
                if (PanNo != null && PanNo != "")
                {
                    cell = new PdfPCell(new Phrase("PAN NO", titleFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedOrder.AddCell(cell);

                    nestedOrder.AddCell(new PdfPCell(CellColon));

                    cell = new PdfPCell(new Phrase(PanNo, titleFont2));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedOrder.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("DATE", titleFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);

                nestedOrder.AddCell(new PdfPCell(CellColon));

                cell = new PdfPCell(new Phrase(date, titleFont2));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);
                cell = new PdfPCell(new Phrase("NO OF RIMS RECEIVED", titleFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);

                nestedOrder.AddCell(new PdfPCell(CellColon));

                cell = new PdfPCell(new Phrase(NoOfRecRim, titleFont2));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedOrder.AddCell(cell);

                PdfPCell nesthousingOrder = new PdfPCell(nestedOrder);
                nesthousingOrder.Padding = 0f;
                nested1.AddCell(nesthousingOrder);
            } PdfPCell nesthousing1 = new PdfPCell(new Phrase("", titleFont));
            nesthousing1.Padding = 0f;
            nested1.AddCell(nesthousing1);

            PdfPCell nesthousingMain = new PdfPCell(nested1);
            nesthousingMain.Padding = 0f;
            nesthousingMain.Colspan = 3;
            table.AddCell(nesthousingMain);

        }
        private void Build_InvoiceOrderItems(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                PdfPTable dt = new PdfPTable(7);
                dt.TotalWidth = 560f;
                dt.LockedWidth = true;
                float[] widths = new float[] { 1f, 7.6f,2.2f, 2.2f, 2.6f, 2.6f, 3f };
                dt.SetWidths(widths);

                // Items Heading
                dt.AddCell(Build_ItemTableHeadStyle("Sl No."));
                dt.AddCell(Build_ItemTableHeadStyle("TYRE SIZE"));
                dt.AddCell(Build_ItemTableHeadStyle("HSN/SAC CODE"));
                dt.AddCell(Build_ItemTableHeadStyle("QTY IN NOS."));
                dt.AddCell(Build_ItemTableHeadStyle("FITMENT CHARGES PER TYRE"));
                dt.AddCell(Build_ItemTableHeadStyle("DISCOUNT"));
                dt.AddCell(Build_ItemTableHeadStyle("TOTAL"));

                //Items Add
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@invoiceno", hdnInvoice.Value);
                sp1[2] = new SqlParameter("@Plant", lblHeadPlant.Text);
                sp1[3] = new SqlParameter("@InvoiceYear", hdnyear.Value);
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_ItemListInvoice", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    int sumqty = 0; decimal sumTotal = 0, total = 0;
                    int j = 1;
                    foreach (DataRow row in dtItemList.Rows)
                    {
                        dt.AddCell(Build_ItemTableListStyle(j.ToString(), 2));
                        dt.AddCell(Build_ItemTableListStyle(row["Tyresize"].ToString(), 0));
                        dt.AddCell(Build_ItemTableListStyle("998714", 2));
                        dt.AddCell(Build_ItemTableListStyle(row["DispatchedQty"].ToString(), 2));
                        dt.AddCell(Build_ItemTableListStyle(Convert.ToDecimal(row["Rate"].ToString()).ToString("0.00"), 2));
                        dt.AddCell(Build_ItemTableListStyle("0.00", 2));
                        total = Convert.ToInt32(row["DispatchedQty"].ToString()) * Convert.ToDecimal(row["Rate"].ToString());
                        dt.AddCell(Build_ItemTableListStyle(Convert.ToDecimal(total).ToString("0.00"), 2));
                        j++;
                        sumqty += Convert.ToInt32(row["DispatchedQty"].ToString());
                        sumTotal += total;
                    }
                    for (int i = dtItemList.Rows.Count; i <= 13; i++)
                    {
                        dt.AddCell(Build_ItemTableListStyle("\n", 2));
                        dt.AddCell(Build_ItemTableListStyle("\n", 0));
                        dt.AddCell(Build_ItemTableListStyle("\n", 2));
                        dt.AddCell(Build_ItemTableListStyle("\n", 2));
                        dt.AddCell(Build_ItemTableListStyle("\n", 2));
                        dt.AddCell(Build_ItemTableListStyle("\n", 2));
                        dt.AddCell(Build_ItemTableListStyle("\n", 2));
                    }
                    dt.AddCell(Build_ItemTableBottomStyle("TOTAL", 2, 2));
                    dt.AddCell(Build_ItemTableBottomStyle("", 2, 0));
                    dt.AddCell(Build_ItemTableBottomStyle(sumqty.ToString(), 2, 0));
                    hdntotalqty.Value = sumqty.ToString();
                    dt.AddCell(Build_ItemTableBottomStyle("", 2, 0));
                    dt.AddCell(Build_ItemTableBottomStyle("0", 2, 0));
                    dt.AddCell(Build_ItemTableBottomStyle(Convert.ToDecimal(sumTotal).ToString("0.00"), 2, 0));

                    //GST tax
                    hdntotal.Value = Math.Round(sumTotal).ToString();
                    dt.AddCell(Build_ItemTableBottomStyle("CGST @ " + Convert.ToDecimal(hdnCGSTPer.Value).ToString("0.00") + "%", 2, 6));
                    decimal cgstVal = hdnCGSTVal.Value != "" ? Convert.ToDecimal(hdnCGSTVal.Value) : 0;
                    dt.AddCell(Build_ItemTableBottomStyle(cgstVal.ToString("0.00"), 2, 0));
                    dt.AddCell(Build_ItemTableBottomStyle("SGST @ " + Convert.ToDecimal(hdnSGSTPer.Value).ToString("0.00") + "%", 2, 6));
                    decimal sgstVal = hdnSGSTVal.Value != "" ? Convert.ToDecimal(hdnSGSTVal.Value) : 0;
                    sgstVal = sumTotal * Convert.ToDecimal(hdnSGSTPer.Value) / 100;
                    dt.AddCell(Build_ItemTableBottomStyle(sgstVal.ToString("0.00"), 2, 0));
                    dt.AddCell(Build_ItemTableBottomStyle("IGST @ " + Convert.ToDecimal(hdnIGSTPer.Value).ToString("0.00") + "%", 2, 6));
                    decimal igstVal = hdnIGSTVal.Value != "" ? Convert.ToDecimal(hdnIGSTVal.Value) : 0;
                    dt.AddCell(Build_ItemTableBottomStyle(Convert.ToDecimal(igstVal).ToString("0.00"), 2, 0));

                    decimal totalGST = 0;
                    totalGST = cgstVal + sgstVal + igstVal;
                    hdnTotalDuty.Value = Math.Round(totalGST).ToString();
                    decimal Grandtotal = 0;
                    Grandtotal = sumTotal + totalGST;
                    hdnGrandTotal.Value = Math.Round(Grandtotal).ToString();
                    dt.AddCell(Build_ItemTableBottomStyle("GRAND TOTAL", 2, 6));
                    dt.AddCell(Build_ItemTableBottomStyle(Convert.ToDecimal(hdnGrandTotal.Value).ToString("0.00"), 2, 0));
                    dt.AddCell(Build_ItemTableBottomStyle("Amount of Tax Subject to Reverse charge", 2, 6));
                    dt.AddCell(Build_ItemTableBottomStyle("0", 2, 0));
                }
                PdfPCell nesthousing1 = new PdfPCell(dt);
                nesthousing1.Padding = 0f;
                nesthousing1.Colspan = 3;
                table.AddCell(nesthousing1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_InvoiceBottomDetails(PdfPTable table)
        {
            PdfPTable tab3 = new PdfPTable(2);
            tab3.TotalWidth = 560f;
            tab3.LockedWidth = true;
            float[] widths3 = new float[] { 4f, 26f };
            tab3.SetWidths(widths3);

            var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            Chunk p1 = new Chunk("Total Invoice Value: ", titleFont);
            titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);

            string[] strSplit = hdnGrandTotal.Value.Split('.');
            int intPart = Convert.ToInt32(strSplit[0].ToString());
            string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
            string text = Utilities.NumberToText(intPart, true, false).ToString();
            if (Convert.ToDecimal(decimalPart) > 0)
                text += " Point " + Utilities.DecimalToText(decimalPart) + " Only";
            else
                text += " Only";

            Chunk p2 = new Chunk(text + "\n", titleFont);
            Paragraph para = new Paragraph();
            para.Add(p1);
            para.Add(p2);
            PdfPCell cell = new PdfPCell(new Phrase(para));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.Colspan = 2;
            cell.FixedHeight = 24f;
            tab3.AddCell(cell);

            titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            p1 = new Chunk("Total Tax Value: ", titleFont);
            titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);

            string[] strSplit1 = hdnTotalDuty.Value.Split('.');
            int intPart1 = Convert.ToInt32(strSplit1[0].ToString());
            string decimalPart1 = strSplit1.Length > 1 ? strSplit1[1].ToString() : "00";
            string text1 = Utilities.NumberToText(intPart1, true, false).ToString();
            if (Convert.ToDecimal(decimalPart1) > 0)
                text1 += " Point " + Utilities.DecimalToText(decimalPart1) + " Only";
            else
                text1 += " Only";

            p2 = new Chunk(text1 + "\n", titleFont);
            para = new Paragraph();
            para.Add(p1);
            para.Add(p2);
            cell = new PdfPCell(new Phrase(para));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.Colspan = 2;
            cell.FixedHeight = 24f;
            tab3.AddCell(cell);

            PdfPTable termsTd = new PdfPTable(2);
            termsTd.TotalWidth = 560f;
            termsTd.LockedWidth = true;
            float[] widths = new float[] { 19.6f, 12f };
            termsTd.SetWidths(widths);
            titleFont = FontFactory.GetFont("Arial", 9, Font.UNDERLINE);
            p1 = new Chunk("CONDITIONS OF FITMENT:\n\n", titleFont);
            string strSaleRules = string.Empty;
            titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            strSaleRules += "1. Rims should reach our plant by 14:00 hrs.\n";
            strSaleRules += "2. If rims are booked for fitment. \n    ( I) Book to our " + lblHeadPlant.Text + " plant on freight paid door delivery basis. \n    (II) Rims fitted with tyres shall dispatched as per the instructions given by you.\n";
            strSaleRules += "3. Adequate precaution shall be taken during fitment. If however due to \n";
            strSaleRules += "    unavoidable circumstances the rim gets damaged the onus for either\n";
            strSaleRules += "    replacement or the cost of the rim shall solely to your account.\n";
            strSaleRules += "4. We are not responsible for any loss/damage to the  goods during transit.\n";
            strSaleRules += "5. Disputes, if any, will be subject to sellers court jurisdiction.\n";

            p2 = new Chunk(strSaleRules, titleFont);
            para = new Paragraph();
            para.Add(p1);
            para.Add(p2);
            cell = new PdfPCell(para);
            termsTd.AddCell(cell);

            titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            //cell = new PdfPCell(new Phrase("\n\n\nFor T.S.RAJAM TYRES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory", titleFont));
            cell = new PdfPCell(new Phrase("\n\n\nFor SUNDARAM INDUSTRIES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory", titleFont));//*************To Be inserted  On 14-02-2022***********************
            cell.Padding = 0f;
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            termsTd.AddCell(cell);

            PdfPCell nesthousingTerms = new PdfPCell(termsTd);
            nesthousingTerms.Padding = 0f;
            nesthousingTerms.Colspan = 2;
            tab3.AddCell(nesthousingTerms);

            PdfPCell nesthousing1 = new PdfPCell(tab3);
            nesthousing1.Padding = 0f;
            nesthousing1.Colspan = 3;
            table.AddCell(nesthousing1);

            //p1 = new Chunk("T.S.Rajam Tyres Private Limited\n", titleFont);
            p1 = new Chunk("Sundaram Industries Private Limited (Formerly Known as T.S.Rajam Tyres Private Limited)\n", titleFont); //*************To Be inserted  On 14-02-2022***********************
            titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            string strRegAdd = string.Empty;
            //strRegAdd += "Regd Office: ‘TVS Building’, 7-B, West veli street, Madurai – 625001.\n";
            strRegAdd += "Regd Office: No.10, Jawahar Road, Chokki Kulam, Madurai, Tamil Nadu, India - 625002.\n";
            //strRegAdd += "\n";
            strRegAdd += "CIN: U51901TN2018PTC121156         Email: crm@sun-tws.com         Website: www.sun-tws.com         Tel: 044-45504157";
            p2 = new Chunk(strRegAdd, titleFont);
            para = new Paragraph();
            para.Add(p1);
            para.Add(p2);
            cell = new PdfPCell(new Phrase(para));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.Padding = 0f;
            cell.Colspan = 3;
            cell.FixedHeight = 30f;
            table.AddCell(cell);
        }
        private PdfPCell Build_ItemTableHeadStyle(string strText)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }
        private PdfPCell Build_HeadTableColsPan(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = Rectangle.BOTTOM_BORDER;
            cell.Colspan = colspan;
            return cell;
        }
        private PdfPCell Build_GoodsHeadCols(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 10, Font.UNDERLINE);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.Border = Rectangle.RIGHT_BORDER;
            cell.Colspan = colspan;
            return cell;
        }
        private PdfPCell Build_ItemTableListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = Rectangle.RIGHT_BORDER;
            return cell;
        }
        private PdfPCell Build_TaxesTableListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            //cell.Border = Rectangle.RIGHT_BORDER;
            return cell;
        }
        private PdfPCell Build_TaxesTableBottomListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            //cell.Border = Rectangle.RIGHT_BORDER;
            return cell;
        }
        private PdfPCell Build_ItemTableBottomStyle(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            return cell;
        }
    }
}