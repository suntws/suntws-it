using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.Data.SqlClient;

namespace TTS
{
    public static class PrepareInvoice
    {
        static InvoiceDataModel Dm;
        static DataAccess daCOTS = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        static string _strGstNo, _strAddressTxt;
        public static PdfPTable Prepare(Document doc, InvoiceDataModel dm)
        {
            PdfPTable table = new PdfPTable(3);
            try
            {
                Dm = dm;
                var contentFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);

                table.TotalWidth = 560f;
                table.LockedWidth = true;
                //                             
                float[] widths = new float[] { 9f, 8f, 9f };
                table.SetWidths(widths);

                DataRow dtRow = Utilities.Get_PlantAddress(Dm.Plant);
                _strGstNo = dtRow["GstNo"].ToString();
                _strAddressTxt = dtRow["PlantAddress"].ToString().Replace("~", "\n");

                if (dm.IsProfoma == true)
                {
                    Build_AddressChildTable(table);
                    Build_ProfomaPdfHeadingTable(table);
                    Build_ProfomaRecordTable(table);
                }
                else
                {
                    Build_AddressChildTable(table);
                    Build_InvoicePdfHeadingTable(table);
                    Build_InvoiceRecordTable(table);
                }

                Build_CustAddressDetailsTable(table);
                Build_TransportTable(table);
                Build_OrderItems(table);
                Build_BottomDetails(table);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return table;
        }
        static void Build_AddressChildTable(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                PdfPTable nested = new PdfPTable(1);

                var headFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                //Chunk postCell1 = new Chunk("T.S.RAJAM TYRES PRIVATE LIMITED\n", headFont);
                Chunk postCell1 = new Chunk("SUNDARAM INDUSTRIES PRIVATE LIMITED\n", headFont);//*************To Be inserted  On 14-02-2022***********************
                var subHead = FontFactory.GetFont("Arial",6, Font.BOLD);
               // Chunk postCell2 = new Chunk("(Formerly Known as T.S.Rajam Tyres Pvt. Ltd.)\n", subHead);
                Chunk postCell2 = new Chunk("(Formerly Known as T.S.RAJAM TYRES PRIVATE LIMITED)\n", subHead);
                //Railway Category
                if (Dm.Category == "RAILWAY")
                {
                    Chunk titleCell1 = new Chunk("Office address:\n", titleFont);
                    string strRailway = "Usilampatti road, Kochadai,\n";
                    strRailway += "Madurai - 625015  Phone: 0452 - 4348888\n";
                    Chunk chRailway = new Chunk(strRailway, titleFont);
                    Phrase paraRailway = new Phrase();
                    paraRailway.Add(titleCell1);
                    paraRailway.Add(postCell1);
                    paraRailway.Add(postCell2);
                    paraRailway.Add(chRailway);
                    PdfPCell pCell = new PdfPCell(new Phrase(paraRailway));
                    nested.AddCell(pCell);
                }

                Chunk postCell3 = new Chunk(_strAddressTxt, titleFont);
                Phrase para = new Phrase();
                if (Dm.Category == "RAILWAY")
                {
                    Chunk titleCell2 = new Chunk("Factory address:\n", titleFont);
                    para.Add(titleCell2);
                }
                para.Add(postCell1);
                para.Add(postCell2);
                para.Add(postCell3);
                PdfPCell cell = new PdfPCell(new Phrase(para));
                if (Dm.Category != "RAILWAY")
                    cell.ExtraParagraphSpace = 2f;
                nested.AddCell(cell);
                nested.TotalWidth = 180f;
                PdfPCell nesthousing = new PdfPCell(nested);

                table.AddCell(nesthousing);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static void Build_InvoicePdfHeadingTable(PdfPTable table)
        {
            try
            {
                PdfPTable nested = new PdfPTable(1);
                PdfPCell cell;
                var titleBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                cell = new PdfPCell(new Phrase(Dm.CustCode != "2652" ? "TAX INVOICE" : "DELIVERY CHALLAN", titleBoldFont));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                nested.AddCell(cell);
                var font1 = FontFactory.GetFont("Arial", 6, Font.ITALIC);
                Chunk chnk = new Chunk("[See Rule 1 under Tax Invoice, Credit and Debit Note Rules]", font1);
                Phrase para = new Phrase();
                para.Add(chnk);
                nested.AddCell(para);

                cell = new PdfPCell(new Phrase(Dm.InvoiceHead, titleBoldFont));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase("GSTIN No   : " + _strGstNo, titleBoldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase("PAN No   : " + _strGstNo.Substring(2, 10), titleBoldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                nested.AddCell(cell);

                PdfPCell nesthousing1 = new PdfPCell(nested);
                nesthousing1.Padding = 0f;
                table.AddCell(nesthousing1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static void Build_InvoiceRecordTable(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                var valueFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                //Heading
                PdfPTable nested1 = new PdfPTable(3);
                nested1.TotalWidth = 188f;
                nested1.LockedWidth = true;
                //                             
                float[] widths = new float[] { 2.4f, 0.2f, 4.4f };
                nested1.SetWidths(widths);

                string imageFilePath = Dm.LogoPath;
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageFilePath);
                img.ScaleToFit(165f, 39f);
                PdfPCell cell1 = new PdfPCell(img);
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.VerticalAlignment = Element.ALIGN_TOP;
                cell1.Colspan = 3;
                cell1.PaddingTop = 1f;
                cell1.PaddingBottom = 1f;
                cell1.Border = 0;
                nested1.AddCell(cell1);

                var invoiceFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                PdfPCell cell = new PdfPCell(new Phrase("Invoice No.", invoiceFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", invoiceFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                invoiceFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                cell = new PdfPCell(new Phrase(Dm.InvoiceNo, invoiceFont));//invoiceNo
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                cell = new PdfPCell(new Phrase("Invoice Date", titleFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", invoiceFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                string str = Dm.InvoiceDate == "" ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : Dm.InvoiceDate;
                cell = new PdfPCell(new Phrase(str, valueFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                if (Dm.IRN != null && Dm.IRN != "" && Dm.IRN != "NA")
                {
                    cell = new PdfPCell(new Phrase("Ack No.", titleFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);

                    cell = new PdfPCell(new Phrase(":", invoiceFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Dm.AckNo.ToString(), valueFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Ack Date", titleFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);

                    cell = new PdfPCell(new Phrase(":", invoiceFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Dm.AckDt, valueFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);
                }

                PdfPCell nesthousing = new PdfPCell(nested1);
                nesthousing.HorizontalAlignment = Element.ALIGN_CENTER;
                nesthousing.VerticalAlignment = Element.ALIGN_TOP;
                nesthousing.Padding = 0f;
                table.AddCell(nesthousing);

                if (Dm.IRN != null && Dm.IRN != "" && Dm.IRN != "NA")
                {
                    PdfPCell irnCell = new PdfPCell(new Phrase("IRN : " + Dm.IRN, valueFont));
                    irnCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    irnCell.VerticalAlignment = Element.ALIGN_TOP;
                    irnCell.PaddingTop = 1f;
                    irnCell.PaddingBottom = 2f;
                    irnCell.Colspan = 3;
                    table.AddCell(irnCell);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static void Build_ProfomaPdfHeadingTable(PdfPTable table)
        {
            try
            {
                PdfPTable nested = new PdfPTable(1);
                nested.TotalWidth = 120f;
                PdfPCell cell;

                var titleBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Chunk chnk = new Chunk(Dm.InvoiceHead + " \n\n", titleBoldFont);
                Phrase para = new Phrase();
                para.Add(chnk);
                cell = new PdfPCell(para);
                cell.HorizontalAlignment = 1;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.WaterMarkHead, titleBoldFont));
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase("GSTIN No   : " + _strGstNo, titleBoldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                nested.AddCell(cell);

                PdfPTable nested1 = new PdfPTable(1);
                nested1.TotalWidth = 120f;

                PdfPCell nesthousing1 = new PdfPCell(nested);
                nesthousing1.Padding = 0f;
                table.AddCell(nesthousing1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static void Build_ProfomaRecordTable(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                var valueFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                //Heading
                PdfPTable nested = new PdfPTable(2);
                PdfPTable nested1 = new PdfPTable(2);
                nested1.TotalWidth = 188f;
                nested1.LockedWidth = true;
                //                             
                float[] widths = new float[] { 2f, 5f };
                nested1.SetWidths(widths);

                string imageFilePath = Dm.LogoPath;
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

                var invoiceFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                PdfPCell cell = new PdfPCell(new Phrase("Ref No.  ", invoiceFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                invoiceFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                cell = new PdfPCell(new Phrase(": " + Dm.RefNo, invoiceFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                cell = new PdfPCell(new Phrase("Date ", titleFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                string str = DateTime.Now.ToString("dd/MM/yyyy");
                cell = new PdfPCell(new Phrase(": " + str, valueFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nested1.AddCell(cell);

                if (Dm.ReviseCount != 0)
                {
                    cell = new PdfPCell(new Phrase("Rev No.", titleFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);

                    cell = new PdfPCell(new Phrase(": " + Dm.ReviseCount, invoiceFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Rev Date", titleFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);

                    str = DateTime.Now.ToString("dd/MM/yyyy");
                    cell = new PdfPCell(new Phrase(": " + str, valueFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nested1.AddCell(cell);
                }

                PdfPCell nesthousing = new PdfPCell(nested1);
                nesthousing.HorizontalAlignment = Element.ALIGN_CENTER;
                nesthousing.VerticalAlignment = Element.ALIGN_TOP;
                nesthousing.Padding = 0f;
                table.AddCell(nesthousing);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static void Build_CustAddressDetailsTable(PdfPTable table)
        {
            try
            {
                var normalFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                var boldFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                PdfPTable tblAddress = new PdfPTable(3);
                tblAddress.TotalWidth = 560f;
                float[] widthsAddress = new float[] { 12f, 12f, 6f };
                tblAddress.SetWidths(widthsAddress);

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderBillingAddress", sp1, DataAccess.Return_Type.DataTable);
                if (dtBillAddress.Rows.Count > 0)
                {
                    DataRow row = dtBillAddress.Rows[0];

                    PdfPTable tblBilledTo = new PdfPTable(1);
                    tblBilledTo.TotalWidth = 230f;
                    float[] widthsOrder = new float[] { 12f };
                    tblBilledTo.SetWidths(widthsOrder);

                    PdfPCell cell = new PdfPCell(new Phrase("BILLED TO", boldFont));
                    cell.HorizontalAlignment = 0;
                    tblBilledTo.AddCell(cell);

                    cell = new PdfPCell(new Phrase("M/S. " + row["custfullname"].ToString() + "", boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblBilledTo.AddCell(cell);

                    string AddressTxt = string.Empty;
                    AddressTxt += row["shipaddress"].ToString().Replace("~", "\n") + "\n";
                    AddressTxt += row["city"].ToString() + " - " + row["zipcode"].ToString() + "\n";
                    cell = new PdfPCell(new Phrase(AddressTxt, boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblBilledTo.AddCell(cell);

                    cell = new PdfPCell(new Phrase(row["statename"].ToString() + " / " + row["stateCode"].ToString(), boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblBilledTo.AddCell(cell);

                    cell = new PdfPCell(new Phrase("GSTIN : " + row["GST_No"].ToString(), boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblBilledTo.AddCell(cell);

                    cell = new PdfPCell(new Phrase("PAN : " + row["GST_No"].ToString().Substring(2, 10), boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblBilledTo.AddCell(cell);

                    cell = new PdfPCell(new Phrase("CONTACT : " + row["contact_name"].ToString() + " / " + row["mobile"].ToString(), boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblBilledTo.AddCell(cell);

                    PdfPCell nesthousing = new PdfPCell(tblBilledTo);
                    nesthousing.Padding = 0f;
                    tblAddress.AddCell(nesthousing);
                }
                else
                {
                    PdfPCell nesthousing = new PdfPCell(new Phrase("\n"));
                    nesthousing.Padding = 0f;
                    tblAddress.AddCell(nesthousing);
                }

                sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                DataTable dtShipAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderShippingAddress", sp1, DataAccess.Return_Type.DataTable);
                if (dtShipAddress.Rows.Count > 0)
                {
                    DataRow row = dtShipAddress.Rows[0];

                    PdfPTable tblConsignee = new PdfPTable(1);
                    tblConsignee.TotalWidth = 230f;
                    float[] widthsOrder = new float[] { 12f };
                    tblConsignee.SetWidths(widthsOrder);

                    PdfPCell cell = new PdfPCell(new Phrase("CONSIGNEE", boldFont));
                    cell.HorizontalAlignment = 0;
                    tblConsignee.AddCell(cell);

                    cell = new PdfPCell(new Phrase("M/S. " + row["custfullname"].ToString() + "", boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblConsignee.AddCell(cell);

                    string AddressTxt = string.Empty;
                    AddressTxt += row["shipaddress"].ToString().Replace("~", "\n") + "\n";
                    AddressTxt += row["city"].ToString() + " - " + row["zipcode"].ToString() + "\n";
                    cell = new PdfPCell(new Phrase(AddressTxt, boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblConsignee.AddCell(cell);

                    cell = new PdfPCell(new Phrase(row["statename"].ToString() + " / " + row["stateCode"].ToString(), boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblConsignee.AddCell(cell);

                    cell = new PdfPCell(new Phrase("GSTIN : " + row["GST_No"].ToString(), boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblConsignee.AddCell(cell);

                    cell = new PdfPCell(new Phrase("PAN : " + row["GST_No"].ToString().Substring(2, 10), boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblConsignee.AddCell(cell);

                    cell = new PdfPCell(new Phrase("CONTACT : " + row["contact_name"].ToString() + " / " + row["mobile"].ToString(), boldFont));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = Element.ALIGN_TOP;
                    cell.Border = Rectangle.NO_BORDER;
                    tblConsignee.AddCell(cell);

                    PdfPCell nesthousing = new PdfPCell(tblConsignee);
                    nesthousing.Padding = 0f;
                    tblAddress.AddCell(nesthousing);
                }
                else
                {
                    PdfPCell nesthousing = new PdfPCell(new Phrase("\n"));
                    nesthousing.Padding = 0f;
                    tblAddress.AddCell(nesthousing);
                }

                if (Dm.IRN != null && Dm.IRN != "" && Dm.IRN != "NA")
                {
                    iTextSharp.text.pdf.BarcodeQRCode qrcode = new BarcodeQRCode(Dm.IrnQrCode, 50, 50, null);
                    iTextSharp.text.Image img1 = qrcode.GetImage();
                    PdfPCell cell = new PdfPCell(img1);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    cell.PaddingTop = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 1f;
                    tblAddress.AddCell(cell);
                }
                else
                {
                    PdfPCell nesthousing = new PdfPCell(new Phrase("\n"));
                    nesthousing.Padding = 0f;
                    tblAddress.AddCell(nesthousing);
                }

                PdfPCell nesthousingMain = new PdfPCell(tblAddress);
                nesthousingMain.Padding = 0f;
                nesthousingMain.Colspan = 3;
                table.AddCell(nesthousingMain);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static void Build_TransportTable(PdfPTable table)
        {
            try
            {
                var fntHeading = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                var fntValue = FontFactory.GetFont("Arial", 9, Font.BOLD);

                PdfPTable nestedOrderInfo = new PdfPTable(2);
                nestedOrderInfo.TotalWidth = 560f;
                nestedOrderInfo.LockedWidth = true;
                float[] widthsOrderInfo = new float[] { 15f, 15f };
                nestedOrderInfo.SetWidths(widthsOrderInfo);

                //Order Details
                PdfPCell cell;
                PdfPTable nestedDomdata = new PdfPTable(3);
                nestedDomdata.TotalWidth = 280f;
                float[] widthsDomdata = new float[] { 3f, 0.2f, 6f };
                nestedDomdata.SetWidths(widthsDomdata);

                cell = new PdfPCell(new Phrase("ORDER NO.", fntHeading));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.OrderRefNo, fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                if (Dm.CustCode != "82" && Dm.CustCode != "508")
                {
                    cell = new PdfPCell(new Phrase("ORDER DATE", fntHeading));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedDomdata.AddCell(cell);

                    cell = new PdfPCell(new Phrase(":", fntValue));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedDomdata.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Dm.OrderDate, fntValue));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedDomdata.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("FREIGHT", fntHeading));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.FreightCharges.ToUpper(), fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                cell = new PdfPCell(new Phrase("C/C ATTACHED", fntHeading));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.CcAttached.ToUpper(), fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                cell = new PdfPCell(new Phrase("PAYMENT TERMS", fntHeading));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                string strPayTerms = Dm.CreditNote == "True" ? "IMMEDIATE" : (Dm.Paymentdays + " days");
                if (Dm.IsProfoma != true && Dm.InvoiceDate != "")
                    strPayTerms += ";  DUE DATE : " + Convert.ToDateTime(Dm.InvoiceDate).AddDays(Convert.ToInt32(Dm.Paymentdays)).ToString("yyyy-MM-dd");

                cell = new PdfPCell(new Phrase(strPayTerms.ToUpper(), fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedDomdata.AddCell(cell);

                nestedOrderInfo.AddCell(nestedDomdata);
                //Transport
                PdfPTable nestedTrans = new PdfPTable(3);
                nestedTrans.TotalWidth = 280f;
                float[] widthsTrans = new float[] { 3.2f, 0.2f, 5f };
                nestedTrans.SetWidths(widthsTrans);

                cell = new PdfPCell(new Phrase("MODE OF TRANSPORT", fntHeading));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedTrans.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedTrans.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.ModeOfTransport, fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedTrans.AddCell(cell);

                cell = new PdfPCell(new Phrase("TRANSPORTER NAME", fntHeading));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedTrans.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedTrans.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.TransporterName, fntValue));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                nestedTrans.AddCell(cell);

                if (!Dm.IsProfoma)
                {
                    cell = new PdfPCell(new Phrase("VEHICLE NO", fntHeading));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedTrans.AddCell(cell);

                    cell = new PdfPCell(new Phrase(":", fntValue));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedTrans.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Dm.VehicleNo, fntValue));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedTrans.AddCell(cell);

                    cell = new PdfPCell(new Phrase("LR.NO & DATE", fntHeading));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedTrans.AddCell(cell);

                    cell = new PdfPCell(new Phrase(":", fntValue));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedTrans.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Dm.LRNo, fntValue));
                    cell.HorizontalAlignment = 0;
                    cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell.Border = 0;
                    nestedTrans.AddCell(cell);
                }

                nestedOrderInfo.AddCell(nestedTrans);
                PdfPCell nesthousingMain = new PdfPCell(nestedOrderInfo);
                nesthousingMain.Padding = 0f;
                nesthousingMain.Colspan = 3;
                table.AddCell(nesthousingMain);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static void Build_OrderItems(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                PdfPTable dt;

                float[] widths = new float[] { 1f, 8f, 3f, 3f, 2.3f, 2.6f, 3f, 3.1f };
                if (Dm.PartNo)
                {
                    dt = new PdfPTable(11);
                    //                     NO   Item code   tar  unit  qty  u.wt rat  dis totWt totVal
                    widths = new float[] { 0.9f, 8f, 3f, 2.4f, 1.2f, 1.2f, 2f, 2.2f, 1.3f, 2.2f, 3f };
                }
                else
                {
                    dt = new PdfPTable(10);
                    //                     NO  Item  tar   unit   qty  u.wt rat  dis totWt totVal
                    widths = new float[] { 0.9f, 8f, 2.4f, 1.2f, 1.5f, 2f, 2.5f, 1.3f, 2.8f, 3f };
                }
                dt.TotalWidth = 560f;
                dt.LockedWidth = true;
                dt.SetWidths(widths);

                //Order Items Heading
                dt.AddCell(Build_ItemTableHeadStyle("Sl. No."));
                dt.AddCell(Build_ItemTableHeadStyle("Description of Goods"));
                if (Dm.PartNo)
                    dt.AddCell(Build_ItemTableHeadStyle("Part No."));
                dt.AddCell(Build_ItemTableHeadStyle("HSN \n Code/SAC"));
                dt.AddCell(Build_ItemTableHeadStyle("Uom"));
                dt.AddCell(Build_ItemTableHeadStyle("Qty"));
                dt.AddCell(Build_ItemTableHeadStyle("Weight\n(per unit)"));
                dt.AddCell(Build_ItemTableHeadStyle("Rate\n(per item)"));
                dt.AddCell(Build_ItemTableHeadStyle("Dis\ncount"));
                dt.AddCell(Build_ItemTableHeadStyle("Total\n Weight"));
                dt.AddCell(Build_ItemTableHeadStyle("Taxable Value"));

                //Items Add
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Dm.OID);

                DataTable dtPartNo = new DataTable();
                if (Dm.PartNo)
                {
                    SqlParameter[] spPart = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                    dtPartNo = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Customer_PartNo", spPart, DataAccess.Return_Type.DataTable);
                }

                for (int k = 1; k < 3; k++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase("\n"));
                    cell.Border = Rectangle.RIGHT_BORDER;
                    dt.AddCell(cell);
                    if ((dtItemList.Rows[0]["grade"].ToString() == "A" || dtItemList.Rows[0]["grade"].ToString() == "B") && k == 1)
                    {
                        var headFont = FontFactory.GetFont("Arial", 9, Font.UNDERLINE);
                        PdfPCell cellHead = new PdfPCell(new Phrase("INDUSTRIAL SOLID TYRES", headFont));
                        cellHead.Border = Rectangle.RIGHT_BORDER;
                        dt.AddCell(cellHead);
                    }
                    else if ((dtItemList.Rows[0]["grade"].ToString() == "C" || dtItemList.Rows[0]["grade"].ToString() == "D") && k == 1)
                    {
                        var headFont = FontFactory.GetFont("Arial", 9, Font.UNDERLINE);
                        PdfPCell cellHead = new PdfPCell(new Phrase("INDUSTRIAL SOLID TYRES (" + dtItemList.Rows[0]["grade"].ToString() + "-GRADE)", headFont));
                        cellHead.Border = Rectangle.RIGHT_BORDER;
                        dt.AddCell(cellHead);
                    }
                    else
                        dt.AddCell(cell);
                    if (Dm.PartNo)
                        dt.AddCell(cell);
                    dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
                }

                int j = 1;
                int balanceRow = 6 - dtItemList.Rows.Count;// - dtdiscount.Rows.Count;
                foreach (DataRow row in dtItemList.Rows)
                {
                    dt.AddCell(Build_ItemTableListStyle(j.ToString(), 2));
                    string strItemType = row["TyreType"].ToString();
                    string strType = strItemType.Length > 2 ? (strItemType.Substring(0, 2)) + (strItemType[2].ToString() == "N" ? "N" : "") : strItemType;
                    string strRim = row["category"].ToString() == "SPLIT RIMS" ? " - SPLIT RIMS " : "" + row["category"].ToString() == "POB WHEEL" ? " - POB WHEEL " : "";
                    string strassy = row["AssyRimstatus"].ToString() == "True" ? " - ASSY " : strRim;
                    string strDwg = strassy != "" ? (row["EdcNO"].ToString() + " (" + row["EdcRimSize"].ToString() + ")") : "";
                    string strRef = strDwg != "" && row["RimDwg"].ToString().Length > 3 ? "\nREF: " + row["RimDwg"].ToString() : "";
                    if (Dm.CustCode == "442" && row["Tyresize"].ToString() == "8.25-20LUG" && row["RimSize"].ToString() == "7.50")
                        dt.AddCell(Build_ItemTableListStyle("SIPL/ACCL/EDC-1" + " - " + row["Brand"].ToString() + " " + strType + strassy + strDwg + strRef, 0));
                    else
                        dt.AddCell(Build_ItemTableListStyle(((row["category"].ToString() == "SPLIT RIMS" || row["category"].ToString() == "POB WHEEL") ? row["EdcRimSize"].ToString() :
                            row["Tyresize"].ToString()) + " /" + row["RimSize"].ToString() + " " + row["Brand"].ToString() + " " + strType + strassy + strDwg + strRef, 0));
                    if (Dm.PartNo)
                    {
                        string partnotype = row["category"].ToString() == "SPLIT RIMS" || row["category"].ToString() == "POB WHEEL" ? row["category"].ToString() : row["tyretype"].ToString();
                        string strAssyStatus = row["AssyRimstatus"].ToString() == "True" ? "ASSY" : row["category"].ToString() == "SPLIT RIMS" || row["category"].ToString() == "POB WHEEL" ? "RIM" : "TYRE";
                        string strPartNo = "";
                        foreach (DataRow rowPart in dtPartNo.Select("TyreType='" + partnotype + "' and TyreSize='" + row["Tyresize"].ToString() + "' and Rimsize='" + row["RimSize"].ToString() + "' and ItemType='" + strAssyStatus + "'"))
                        {
                            strPartNo = rowPart["ItemCode"].ToString();
                        }
                        dt.AddCell(Build_ItemTableListStyle(strPartNo, 0));
                    }
                    string strHsnCode = "-";
                    foreach (DataRow hRow in Dm.dtHsnCode.Select("GoodsCategory='" + row["category"].ToString().ToLower() + "'"))
                    {
                        strHsnCode = hRow["HsnCode"].ToString();
                    }
                    dt.AddCell(Build_ItemTableListStyle(strHsnCode, 0));
                    dt.AddCell(Build_ItemTableListStyle("Nos", 2));
                    dt.AddCell(Build_ItemTableListStyle(row["itemqty"].ToString(), 2));
                    dt.AddCell(Build_ItemTableListStyle(row["bothfinWt"].ToString(), 2));
                    dt.AddCell(Build_ItemTableListStyle(row["bothunitprice"].ToString(), 2));
                    dt.AddCell(Build_ItemTableListStyle("0.0", 2));
                    dt.AddCell(Build_ItemTableListStyle(row["finishedwt"].ToString(), 2));
                    dt.AddCell(Build_ItemTableListStyle(row["unitprice"].ToString(), 2));
                    j++;
                    if (strDwg.Length > 0)
                        balanceRow--;
                    if (strRef.Length > 0)
                        balanceRow--;
                }

                if (Dm.CustCode == "3781")
                {
                    PdfPCell cell = new PdfPCell(new Phrase("\n"));
                    cell.Border = Rectangle.RIGHT_BORDER;
                    dt.AddCell(cell); dt.AddCell(Build_ItemTableListStyle("SUPPLY TO SEZ UNIT WITHOUT \nPAYMENT OF IGST UNDER LUT", 0));
                    dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
                    if (Dm.PartNo)
                        dt.AddCell(cell);
                    dt.AddCell(cell); dt.AddCell(Build_ItemTableListStyle("ARN NO : AD330320008881L", 0));
                    dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
                    if (Dm.PartNo)
                        dt.AddCell(cell);
                    dt.AddCell(cell); dt.AddCell(Build_ItemTableListStyle("Date : 17/03/2020", 0));
                    dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
                    if (Dm.PartNo)
                        dt.AddCell(cell);
                    balanceRow = balanceRow - 3;
                }

                for (int k = 0; k < balanceRow; k++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase("\n"));
                    cell.Border = Rectangle.RIGHT_BORDER;
                    dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
                    dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
                    if (Dm.PartNo)
                        dt.AddCell(cell);
                }

                // total value row
                dt.AddCell(Build_ItemTableBottomStyle("Total", 2, Dm.PartNo == true ? 4 : 3));
                dt.AddCell(Build_ItemTableBottomStyle("", 2, 1)); // units - empty
                object sumQty = dtItemList.Compute("Sum(itemqty)", "");
                dt.AddCell(Build_ItemTableBottomStyle(sumQty.ToString(), 2, 1)); // sum of qty
                dt.AddCell(Build_ItemTableBottomStyle("", 2, 1)); // weights - empty
                dt.AddCell(Build_ItemTableBottomStyle(" ", 2, 1)); // rate  - empty
                dt.AddCell(Build_ItemTableBottomStyle(" ", 2, 1)); // discount - empty
                object sumWt = dtItemList.Compute("Sum(finishedwt)", "");
                dt.AddCell(Build_ItemTableBottomStyle(sumWt.ToString(), 2, 1)); // sum of total weight
                object sumCost = dtItemList.Compute("Sum(unitprice)", "");
                dt.AddCell(Build_ItemTableBottomStyle(sumCost.ToString(), 2, 1)); // sum of taxable value 

                dt.AddCell(Build_Duties_Taxes(Convert.ToDecimal(sumCost.ToString())));

                PdfPCell nesthousing1 = new PdfPCell(dt);
                nesthousing1.Padding = 0f;
                nesthousing1.Colspan = 3;
                table.AddCell(nesthousing1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static PdfPCell Build_Duties_Taxes(decimal totTaxValue)
        {
            PdfPCell cell = new PdfPCell();
            try
            {
                PdfPTable tab2 = new PdfPTable(3);
                float[] widths2 = new float[] { 23.2f, 3.2f, 3.4f };
                tab2.TotalWidth = 560f;
                tab2.SetWidths(widths2);

                SqlParameter[] spdiscount = new SqlParameter[] { new SqlParameter("@OID", Dm.OID) };
                DataTable dtdiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_Discount", spdiscount, DataAccess.Return_Type.DataTable);

                for (int i = 0; i < dtdiscount.Rows.Count; i++)
                {
                    tab2.AddCell(Build_TaxesTableListStyle(dtdiscount.Rows[i]["Category"].ToString() + ": " + dtdiscount.Rows[i]["DescriptionDiscount"].ToString(), 2));
                    tab2.AddCell(Build_TaxesTableListStyle("", 2));
                    tab2.AddCell(Build_TaxesTableListStyle(dtdiscount.Rows[i]["Amount"].ToString(), 2));
                }

                decimal txtTOTALCOST1 = totTaxValue;
                for (int i = 0; i < dtdiscount.Rows.Count; i++)
                {
                    if (dtdiscount.Rows[i]["Category"].ToString() == "ADD")
                    {
                        txtTOTALCOST1 += (Convert.ToDecimal(dtdiscount.Rows[i]["Amount"]));
                    }
                    //else if (dtdiscount.Rows[i]["Category"].ToString() == "LESS")
                    //{
                    //    txtTOTALCOST1 -= (Convert.ToDecimal(dtdiscount.Rows[i]["Amount"]));
                    //}
                }

                tab2.AddCell(Build_TaxesTableBottomListStyle("Sub Total", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                tab2.AddCell(Build_TaxesTableBottomListStyle(txtTOTALCOST1.ToString("0.00"), 2));

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@OID", Dm.OID) };
                DataTable dtTax = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_PrepareInvoice_TaxDuties", sp1, DataAccess.Return_Type.DataTable);
                DataRow row = dtTax.Rows[0];
                decimal totCGST = 0, totSGST = 0, totIGST = 0;
                //CGST
                decimal cgst = Convert.ToDecimal(row["CGST"].ToString() == "" ? "0" : row["CGST"].ToString());
                tab2.AddCell(Build_TaxesTableListStyle("CGST", 2));
                tab2.AddCell(Build_TaxesTableListStyle(cgst.ToString() + "%", 2));
                totCGST = Math.Round(txtTOTALCOST1 * (cgst / 100), 2);
                tab2.AddCell(Build_TaxesTableListStyle(totCGST.ToString("0.00"), 2));
                Dm.CGSTPer = cgst;
                Dm.CGSTVal = totCGST;
                //SGST
                decimal sgst = Convert.ToDecimal(row["SGST"].ToString() == "" ? "0" : row["SGST"].ToString());
                tab2.AddCell(Build_TaxesTableListStyle("SGST", 2));
                tab2.AddCell(Build_TaxesTableListStyle(sgst.ToString() + "%", 2));
                totSGST = Math.Round(txtTOTALCOST1 * (sgst / 100), 2);
                tab2.AddCell(Build_TaxesTableListStyle(totSGST.ToString("0.00"), 2));
                Dm.SGSTPer = sgst;
                Dm.SGSTVal = totSGST;
                //IGST
                decimal igst = Convert.ToDecimal(row["IGST"].ToString() == "" ? "0" : row["IGST"].ToString());
                tab2.AddCell(Build_TaxesTableListStyle("IGST", 2));
                tab2.AddCell(Build_TaxesTableListStyle(igst.ToString() + "%", 2));
                totIGST = Math.Round(txtTOTALCOST1 * (igst / 100), 2);
                tab2.AddCell(Build_TaxesTableListStyle(totIGST.ToString("0.00"), 2));
                Dm.IGSTPer = igst;
                Dm.IGSTVal = totIGST;

                tab2.AddCell(Build_TaxesTableBottomListStyle("Total Tax", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                decimal totalTax = totCGST + totSGST + totIGST;
                tab2.AddCell(Build_TaxesTableBottomListStyle(totalTax.ToString("0.00"), 2));

                tab2.AddCell(Build_TaxesTableBottomListStyle("Total Value With GST", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                decimal totalAssesVal = Math.Round((totalTax + txtTOTALCOST1), 2);
                tab2.AddCell(Build_TaxesTableBottomListStyle(totalAssesVal.ToString("0.00"), 2));

                decimal decTcsValue = 0;
                SqlParameter[] spT = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                DataTable dtTcs = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_TcsTax_Status", spT, DataAccess.Return_Type.DataTable);
                if (dtTcs.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtTcs.Rows[0]["tcsApplicable"].ToString()))
                    {
                        tab2.AddCell(Build_TaxesTableBottomListStyle("TCS", 2));
                        decimal decTcsPercentage = Convert.ToDecimal(Convert.ToBoolean(dtTcs.Rows[0]["validPan"].ToString()) ? 0.1 : 1);
                        tab2.AddCell(Build_TaxesTableListStyle(decTcsPercentage.ToString() + "%", 2));
                        decTcsValue = Math.Round((totalAssesVal * (decTcsPercentage / 100)), 2);
                        tab2.AddCell(Build_TaxesTableBottomListStyle(decTcsValue.ToString("0.00"), 2));
                    }
                }

                decimal totInvVal = totalAssesVal + decTcsValue;
                decimal decRoundOff = Math.Round((Math.Round(totInvVal, 0) - totInvVal), 2);

                tab2.AddCell(Build_TaxesTableBottomListStyle("Round Off Value", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                tab2.AddCell(Build_TaxesTableBottomListStyle((decRoundOff).ToString("0.00"), 2));

                decimal decTotInvFinalVal = (Math.Round((totInvVal + decRoundOff), 0));
                tab2.AddCell(Build_TaxesTableBottomListStyle("Total Invoice Value (in figure)", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                tab2.AddCell(Build_TaxesTableBottomListStyle(decTotInvFinalVal.ToString("0.00"), 2));

                tab2.AddCell(Build_TaxesTableBottomListStyle("Amount of TaxSubject to Reverse charge", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                tab2.AddCell(Build_TaxesTableBottomListStyle("0.00", 2));

                PdfPTable tab3 = new PdfPTable(2);
                tab3.TotalWidth = 560f;
                tab3.LockedWidth = true;
                float[] widths3 = new float[] { 5f, 25f };
                tab3.SetWidths(widths3);

                var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                Chunk p1 = new Chunk("Total Invoice Value (in words): ", titleFont);
                titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);

                string[] strSplit = decTotInvFinalVal.ToString().Split('.');
                int intPart = Convert.ToInt32(strSplit[0].ToString());
                string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
                string text = Utilities.NumberToText(intPart, true, false).ToString();
                if (Convert.ToDecimal(decimalPart) > 0)
                    text += " Point " + Utilities.DecimalToText(decimalPart) + " Only";
                else
                    text += " Only";

                Chunk p2 = new Chunk(text, titleFont);
                Paragraph para = new Paragraph();
                para.Add(p1);
                para.Add(p2);
                PdfPCell cell1 = new PdfPCell(new Phrase(para));
                cell1.HorizontalAlignment = 0;
                cell1.Colspan = 2;
                cell1.VerticalAlignment = PdfPCell.ALIGN_TOP;
                tab3.AddCell(cell1);

                titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                p1 = new Chunk("Total GST Value (in words): ", titleFont);
                titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);

                string[] strSplit1 = totalTax.ToString().Split('.');
                int intPart1 = Convert.ToInt32(strSplit1[0].ToString());
                string decimalPart1 = strSplit1.Length > 1 ? strSplit1[1].ToString() : "00";
                string text1 = Utilities.NumberToText(intPart1, true, false).ToString();
                if (Convert.ToDecimal(decimalPart1) > 0)
                    text1 += " Point " + Utilities.DecimalToText(decimalPart1) + " Only";
                else
                    text1 += " Only";

                p2 = new Chunk(text1, titleFont);
                para = new Paragraph();
                para.Add(p1);
                para.Add(p2);
                cell1 = new PdfPCell(new Phrase(para));
                cell1.HorizontalAlignment = 0;
                cell1.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell1.Colspan = 2;
                tab3.AddCell(cell1);

                if (decTcsValue > 0)
                {
                    titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                    p1 = new Chunk("Total Tcs Value (in words): ", titleFont);
                    titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);

                    string[] strSplit2 = decTcsValue.ToString().Split('.');
                    int intPart2 = Convert.ToInt32(strSplit2[0].ToString());
                    string decimalPart2 = strSplit2.Length > 1 ? strSplit2[1].ToString() : "00";
                    string text2 = Utilities.NumberToText(intPart2, true, false).ToString();
                    if (Convert.ToDecimal(decimalPart2) > 0)
                        text2 += " Point " + Utilities.DecimalToText(decimalPart2) + " Only";
                    else
                        text2 += " Only";

                    p2 = new Chunk(text2, titleFont);
                    para = new Paragraph();
                    para.Add(p1);
                    para.Add(p2);
                    cell1 = new PdfPCell(new Phrase(para));
                    cell1.HorizontalAlignment = 0;
                    cell1.VerticalAlignment = PdfPCell.ALIGN_TOP;
                    cell1.Colspan = 2;
                    tab3.AddCell(cell1);
                }

                PdfPTable nested = new PdfPTable(1);
                PdfPCell nesthousing = new PdfPCell(tab2);
                nesthousing.Padding = 0f;
                nesthousing.Colspan = 3;
                nested.AddCell(nesthousing);

                PdfPCell nesthousing1 = new PdfPCell(tab3);
                nesthousing1.Padding = 0f;
                nesthousing1.Colspan = 3;
                nested.AddCell(nesthousing1);

                cell = new PdfPCell(nested);
                cell.Padding = 0f;
                cell.Colspan = Dm.PartNo == true ? 11 : 10;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return cell;
        }
        static void Build_BottomDetails(PdfPTable table)
        {
            try
            {
                PdfPTable tab3 = new PdfPTable(2);
                tab3.TotalWidth = 560f;
                tab3.LockedWidth = true;
                float[] widths3 = new float[] { 15f, 15f };
                tab3.SetWidths(widths3);

                var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                PdfPTable termsTd = new PdfPTable(2);
                termsTd.TotalWidth = 560f;
                termsTd.LockedWidth = true;
                float[] widths = new float[] { 17f, 13f };
                termsTd.SetWidths(widths);
                titleFont = FontFactory.GetFont("Arial", 9, Font.UNDERLINE);
                Chunk p1 = new Chunk("TERMS OF SALE\n\n", titleFont);
                titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                string strSaleRules = string.Empty;
                strSaleRules += "1. Goods once sold can not be taken back/exchanged.\n";
                strSaleRules += "2. We are not responsible for any loss/damage to the  goods during transit.\n";
                strSaleRules += "3. Disputes, if any, will be subject to sellers court jurisdiction.\n";
                //strSaleRules += "4. Interest @18% will be charged on overdue.\n";
                strSaleRules += "4. Additional conditions for supply attached overleaf.\n";
                //strSaleRules += "5. In respect of tyre size 22X12X16ML, as one time special concession the rate is fixed @ Rs.6696 per tyre.\n";

                Chunk p2 = new Chunk(strSaleRules, titleFont);
                Paragraph para = new Paragraph();
                para.Add(p1);
                para.Add(p2);
                PdfPCell cell = new PdfPCell(para);
                termsTd.AddCell(cell);

                titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);               
                //cell = new PdfPCell(new Phrase("\nFor T.S.RAJAM TYRES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory\n", titleFont));
                cell = new PdfPCell(new Phrase("\nFor SUNDARAM INDUSTRIES PRIVATE LIMITED \n\n\n\n\n Authorised Signatory\n", titleFont));//*************To Be inserted  On 14-02-2022***********************
                cell.Padding = 0f;
                cell.HorizontalAlignment = 1;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Rowspan = 2;
                termsTd.AddCell(cell);

                titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                string strCertified = string.Empty;
                strCertified += "Certified that the particulars given above are true and correct and the\n";
                strCertified += "amount indicated represents the price actually charged and there is no\n";
                strCertified += "flow of additional consideration directly or indirectly from the buyer";
                p2 = new Chunk(strCertified, titleFont);
                para = new Paragraph();
                para.Add(p2);
                cell = new PdfPCell(para);
                termsTd.AddCell(cell);

                var titleFont1 = FontFactory.GetFont("Arial", 9, Font.BOLD);
                string strBankDet = string.Empty;
                strBankDet += "   BANK NAME : STATE BANK OF INDIA                ACCOUNT TYPE : CASH CREDIT                 ACCOUNT NO   : 40776755597\n\n";
                strBankDet += "   IFSC CODE   : SBIN0011933                               BRANCH NAME  : SME MARAIMALAINAGAR\n";
                p1 = new Chunk(strBankDet, titleFont1);
                para = new Paragraph();
                para.Add(p1);
                para.Alignment = Element.ALIGN_LEFT;
                PdfPCell cell1 = new PdfPCell(para);
                cell1.Padding = 0f;
                cell1.Colspan = 3;
                cell1.FixedHeight = 30f;
                termsTd.AddCell(cell1);

                //p1 = new Chunk("T.S.Rajam Tyres Private Limited\n", titleFont);
                p1 = new Chunk("Sundaram Industries Private Limited (Formerly Known as T.S.Rajam Tyres Private Limited) \n", titleFont);//*************To Be inserted  On 14-02-2022***********************
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
                termsTd.AddCell(cell);

                PdfPCell nesthousingTerms = new PdfPCell(termsTd);
                nesthousingTerms.Padding = 0f;
                nesthousingTerms.Colspan = 2;
                tab3.AddCell(nesthousingTerms);

                PdfPCell nesthousing1 = new PdfPCell(tab3);
                nesthousing1.Padding = 0f;
                nesthousing1.Colspan = 3;
                table.AddCell(nesthousing1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static PdfPCell Build_ItemTableHeadStyle(string strText)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }
        static PdfPCell Build_ItemTableBottomStyle(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            return cell;
        }
        static PdfPCell Build_ItemTableListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = Rectangle.RIGHT_BORDER;
            return cell;
        }
        static PdfPCell Build_TaxesTableListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            //cell.Border = Rectangle.RIGHT_BORDER;
            return cell;
        }
        static PdfPCell Build_TaxesTableBottomListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            //cell.Border = Rectangle.RIGHT_BORDER;
            return cell;
        }
    }
}