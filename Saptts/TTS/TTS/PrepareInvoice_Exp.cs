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
    public static class PrepareInvoice_Exp
    {
        static InvoiceDataModel Dm;
        static DataAccess daCOTS = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        public static PdfPTable Prepare(Document doc, InvoiceDataModel dm)
        {
            PdfPTable table = new PdfPTable(2);
            try
            {
                Dm = dm;
                var contentFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);

                table.TotalWidth = 560f;
                table.LockedWidth = true;
                float[] widths = new float[] { 15f, 15f };
                table.SetWidths(widths);

                Build_PlantAddress_Table(table);
                Build_Invoice_QR_Table(table);
                Build_CustAddress_Table(table);

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
        static void Build_PlantAddress_Table(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                Chunk postHeadCell = new Chunk("INVOICE CUM PACKING LIST\n", titleFont);
                PdfPCell nestHeadhousing = new PdfPCell(new Phrase(postHeadCell));
                nestHeadhousing.Colspan = 2;
                table.AddCell(nestHeadhousing);

                PdfPTable nested = new PdfPTable(1);
                SqlParameter[] spAdd = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                DataTable dtAdd = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ExportAddress_invoice", spAdd, DataAccess.Return_Type.DataTable);
                if (dtAdd.Rows.Count == 1)
                {
                    var headFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                    Chunk postCell1 = new Chunk("Exporter", headFont);
                    Chunk postCell2 = new Chunk(dtAdd.Rows[0]["AuthorizedAddress"].ToString() + "\n" +
                        dtAdd.Rows[0]["ExpoterAddress"].ToString().Replace("~", "\n"), titleFont);
                    Chunk postCell3 = new Chunk("IEC CODE : " + dtAdd.Rows[0]["Iec_Code"].ToString() + "\n" +
                        "GSTIN NO : " + dtAdd.Rows[0]["Gst_No"].ToString() + "\n" +
                        "PAN NO : " + dtAdd.Rows[0]["Pan_No"].ToString(), titleFont);
                    Phrase para = new Phrase();
                    para.Add(postCell1);
                    para.Add(postCell2);
                    para.Add(postCell3);
                    PdfPCell cell = new PdfPCell(new Phrase(para));
                    nested.AddCell(cell);
                }
                else
                {
                    nested.AddCell("\n");
                }
                nested.TotalWidth = 280f;
                table.AddCell(nested);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static void Build_Invoice_QR_Table(PdfPTable table)
        {
            try
            {
                PdfPTable nestedSub = new PdfPTable(1);

                PdfPTable nested = new PdfPTable(3);
                float[] widths = new float[] { 4.2f, 0.2f, 9.6f };
                nested.SetWidths(widths);

                PdfPCell cell;
                var titleNormalFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                var valBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                cell = new PdfPCell(new Phrase("Invoice No", titleNormalFont));
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", titleNormalFont));
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.InvoiceNo, valBoldFont));
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase("Date", titleNormalFont));
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", titleNormalFont));
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.InvoiceDate, valBoldFont));
                cell.Border = 0;
                nested.AddCell(cell);

                nestedSub.AddCell(nested);

                cell = new PdfPCell(new Phrase("Buyers Order No Ref", titleNormalFont));
                cell.Colspan = 3;
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.OrderRefNo, valBoldFont));
                cell.Border = 0;
                nested.AddCell(cell);

                nestedSub.AddCell(nested);

                cell = new PdfPCell(new Phrase("Other Reference(s)", titleNormalFont));
                cell.Colspan = 3;
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", valBoldFont));
                cell.Border = 0;
                nested.AddCell(cell);

                nestedSub.AddCell(nested);

                PdfPTable nestedTwo = new PdfPTable(2);
                nestedTwo.AddCell(nestedSub);
                if (Dm.IRN != null && Dm.IRN != "" && Dm.IRN != "NA")
                {
                    iTextSharp.text.pdf.BarcodeQRCode qrcode = new BarcodeQRCode(Dm.IrnQrCode, 50, 50, null);
                    iTextSharp.text.Image img1 = qrcode.GetImage();
                    cell = new PdfPCell(img1);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    cell.PaddingTop = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 1f;
                    nestedTwo.AddCell(cell);
                }
                else
                {
                    PdfPCell nesthousing = new PdfPCell(new Phrase("\n"));
                    nesthousing.Padding = 0f;
                    nestedTwo.AddCell(nesthousing);
                }

                table.AddCell(nestedTwo);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        static void Build_CustAddress_Table(PdfPTable table)
        {
            try
            {
                var normalFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                var boldFont = FontFactory.GetFont("Arial", 9, Font.BOLD);

                PdfPTable tblNestedLeft = new PdfPTable(2);
                PdfPCell cell;
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderBillingAddress", sp1, DataAccess.Return_Type.DataTable);
                if (dtBillAddress.Rows.Count > 0)
                {
                    DataRow row = dtBillAddress.Rows[0];

                    cell = new PdfPCell(new Phrase("Consignee\n", boldFont));
                    cell = new PdfPCell(new Phrase(row["custfullname"].ToString() + "", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    string AddressTxt = string.Empty;
                    AddressTxt += row["shipaddress"].ToString().Trim() != "" ? row["shipaddress"].ToString().Replace("~", "\n") + "\n" : "\n";
                    AddressTxt += row["city"].ToString().Trim() != "" ? row["city"].ToString() + "\n" : "\n";
                    AddressTxt += row["statename"].ToString().Trim() != "" ? row["statename"].ToString() + " - " : "";
                    AddressTxt += row["country"].ToString().Trim() != "" ? row["country"].ToString() + " - " : "";
                    AddressTxt += row["zipcode"].ToString().Trim() != "" ? row["zipcode"].ToString() + "\n" : "\n";
                    //AddressTxt += row["mobile"].ToString().Trim() != "" ? "TEL NO: " + row["mobile"].ToString() + "\n" : "\n";
                    //AddressTxt += row["EmailID"].ToString().Trim() != "" ? "EMAIL: " + row["EmailID"].ToString() + "\n\n" : "\n\n";
                    //AddressTxt += row["contact_name"].ToString().Trim() != "" ? "KIND ATTN: " + row["contact_name"].ToString() + "\n" : "\n";
                    AddressTxt += "\n\n\n\n\n";
                    cell = new PdfPCell(new Phrase(AddressTxt, boldFont));
                }
                else
                {
                    cell = new PdfPCell(new Phrase("\n", boldFont));
                }
                cell.Colspan = 2;
                tblNestedLeft.AddCell(cell);

                var portHeadFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var portValFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                DataTable dtTrans = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_Transport_details", sp1, DataAccess.Return_Type.DataTable);
                if (dtTrans.Rows.Count == 1)
                {
                    cell = new PdfPCell(new Phrase("Pre-Carriage By\n", portHeadFont));
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["PreCarriageBy"].ToString(), portValFont));
                    tblNestedLeft.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Place of Receipt by Pre-Carrier\n", portHeadFont));
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["PlaceOfReceiptByPreCarrier"].ToString(), portValFont));
                    tblNestedLeft.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Vessel / Flight No.\n", portHeadFont));
                    cell = new PdfPCell(new Phrase("\n", portValFont));
                    tblNestedLeft.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Port of Loading\n", portHeadFont));
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["PortOfLoading"].ToString(), portValFont));
                    tblNestedLeft.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Port of Discharge\n", portHeadFont));
                    cell = new PdfPCell(new Phrase("\n", portValFont));
                    tblNestedLeft.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Final Destination\n", portHeadFont));
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["FinalDestination"].ToString(), portValFont));
                    tblNestedLeft.AddCell(cell);
                }
                table.AddCell(tblNestedLeft);

                PdfPTable tblNestedRight = new PdfPTable(2);
                sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                DataTable dtShipAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderShippingAddress", sp1, DataAccess.Return_Type.DataTable);
                if (dtShipAddress.Rows.Count > 0)
                {
                    DataRow row = dtBillAddress.Rows[0];

                    cell = new PdfPCell(new Phrase("Consignee\n", boldFont));
                    cell = new PdfPCell(new Phrase(row["custfullname"].ToString() + "", FontFactory.GetFont("Arial", 10, Font.BOLD)));

                    string AddressTxt = string.Empty;
                    AddressTxt += row["shipaddress"].ToString().Trim() != "" ? row["shipaddress"].ToString().Replace("~", "\n") + "\n" : "\n";
                    AddressTxt += row["city"].ToString().Trim() != "" ? row["city"].ToString() + "\n" : "\n";
                    AddressTxt += row["statename"].ToString().Trim() != "" ? row["statename"].ToString() + " - " : "";
                    AddressTxt += row["country"].ToString().Trim() != "" ? row["country"].ToString() + " - " : "";
                    AddressTxt += row["zipcode"].ToString().Trim() != "" ? row["zipcode"].ToString() + "\n" : "\n";
                    //AddressTxt += row["mobile"].ToString().Trim() != "" ? "TEL NO: " + row["mobile"].ToString() + "\n" : "\n";
                    //AddressTxt += row["EmailID"].ToString().Trim() != "" ? "EMAIL: " + row["EmailID"].ToString() + "\n\n" : "\n\n";
                    //AddressTxt += row["contact_name"].ToString().Trim() != "" ? "KIND ATTN: " + row["contact_name"].ToString() + "\n" : "\n";
                    cell = new PdfPCell(new Phrase(AddressTxt, boldFont));
                }
                else
                {
                    cell = new PdfPCell(new Phrase("\n\n\n\n\n", boldFont));
                }
                cell.Colspan = 2;
                tblNestedRight.AddCell(cell);

                if (dtTrans.Rows.Count == 1)
                {
                    cell = new PdfPCell(new Phrase("Country of Origin of Goods\n", portHeadFont));
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["CountryOfOrigin"].ToString(), portValFont));
                    tblNestedRight.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Country of Final Destination\n", portHeadFont));
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["CountryOfDestination"].ToString(), portValFont));
                    tblNestedRight.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Terms of Delivery and Payment\n", portHeadFont));
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["payterms"].ToString(), portValFont));
                    cell.Colspan = 2;
                    tblNestedRight.AddCell(cell);
                }
                table.AddCell(tblNestedRight);
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

                cell = new PdfPCell(new Phrase(Dm.CreditNote == "True" ? "IMMEDIATE" : (Dm.Paymentdays + " days").ToUpper(), fntValue));
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
                    //                     NO  Item code  tar   qty   unit u.wt  rat   dis  totWt  totVal
                    widths = new float[] { 0.9f, 8f, 2f, 2.5f, 1.2f, 1.2f, 2f, 2.2f, 2f, 2f, 3f };
                }
                else
                {
                    dt = new PdfPTable(10);
                    //                     NO Item  tar   qty   unit   u.wt  rat   dis  totWt  totVal
                    widths = new float[] { 0.9f, 8f, 2.5f, 1.2f, 1.5f, 2f, 2.5f, 2f, 2.8f, 3f };
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
                dt.AddCell(Build_ItemTableHeadStyle("Discount"));
                dt.AddCell(Build_ItemTableHeadStyle("Total\n Weight"));
                dt.AddCell(Build_ItemTableHeadStyle("Taxable Value"));

                //Items Add
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Dm.OID);

                int balanceItem = 12 - dtItemList.Rows.Count;// - dtdiscount.Rows.Count;
                int balanceMod = balanceItem / 2;

                DataTable dtPartNo = new DataTable();
                if (Dm.PartNo)
                {
                    SqlParameter[] spPart = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                    dtPartNo = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Customer_PartNo", spPart, DataAccess.Return_Type.DataTable);
                }

                for (int k = 1; k < balanceMod; k++)
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
                foreach (DataRow row in dtItemList.Rows)
                {
                    dt.AddCell(Build_ItemTableListStyle(j.ToString(), 2));
                    string strItemType = row["TyreType"].ToString();
                    string strType = strItemType.Length > 2 ? (strItemType.Substring(0, 2)) + (strItemType[2].ToString() == "N" ? "N" : "") : strItemType;
                    string strRim = row["category"].ToString() == "SPLIT RIMS" ? " - SPLIT RIMS " : "" + row["category"].ToString() == "POB WHEEL" ? " - POB WHEEL " : "";
                    string strassy = row["AssyRimstatus"].ToString() == "True" ? " - ASSY " : strRim;
                    string strDwg = strassy != "" ? ("\n" + row["EdcNO"].ToString() + " (" + row["EdcRimSize"].ToString() + ")\nREF: " + row["RimDwg"].ToString()) : "";
                    if (Dm.CustCode == "442" && row["Tyresize"].ToString() == "8.25-20LUG" && row["RimSize"].ToString() == "7.50")
                        dt.AddCell(Build_ItemTableListStyle("SIPL/ACCL/EDC-1" + " - " + row["Brand"].ToString() + " " + strType + strassy + strDwg, 0));
                    else
                        dt.AddCell(Build_ItemTableListStyle(((row["category"].ToString() == "SPLIT RIMS" || row["category"].ToString() == "POB WHEEL") ? row["EdcRimSize"].ToString() :
                            row["Tyresize"].ToString()) + " /" + row["RimSize"].ToString() + " " + row["Brand"].ToString() + " " + strType + strassy + strDwg, 0));
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
                }

                for (int k = dtItemList.Rows.Count + balanceMod; k < (Dm.CustCode == "3781" ? 6 : 10); k++)
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
                    else if (dtdiscount.Rows[i]["Category"].ToString() == "LESS")
                    {
                        txtTOTALCOST1 -= (Convert.ToDecimal(dtdiscount.Rows[i]["Amount"]));
                    }
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
                totCGST = Math.Round(txtTOTALCOST1 * (cgst / 100), 0);
                tab2.AddCell(Build_TaxesTableListStyle(totCGST.ToString("0.00"), 2));
                Dm.CGSTPer = cgst;
                Dm.CGSTVal = totCGST;
                //SGST
                decimal sgst = Convert.ToDecimal(row["SGST"].ToString() == "" ? "0" : row["SGST"].ToString());
                tab2.AddCell(Build_TaxesTableListStyle("SGST", 2));
                tab2.AddCell(Build_TaxesTableListStyle(sgst.ToString() + "%", 2));
                totSGST = Math.Round(txtTOTALCOST1 * (sgst / 100), 0);
                tab2.AddCell(Build_TaxesTableListStyle(totSGST.ToString("0.00"), 2));
                Dm.SGSTPer = sgst;
                Dm.SGSTVal = totSGST;
                //IGST
                decimal igst = Convert.ToDecimal(row["IGST"].ToString() == "" ? "0" : row["IGST"].ToString());
                tab2.AddCell(Build_TaxesTableListStyle("IGST", 2));
                tab2.AddCell(Build_TaxesTableListStyle(igst.ToString() + "%", 2));
                totIGST = Math.Round(txtTOTALCOST1 * (igst / 100), 0);
                tab2.AddCell(Build_TaxesTableListStyle(totIGST.ToString("0.00"), 2));
                Dm.IGSTPer = igst;
                Dm.IGSTVal = totIGST;

                tab2.AddCell(Build_TaxesTableBottomListStyle("Total Tax", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                decimal totalTax = totCGST + totSGST + totIGST;
                tab2.AddCell(Build_TaxesTableBottomListStyle(totalTax.ToString("0.00"), 2));

                tab2.AddCell(Build_TaxesTableBottomListStyle("Total Value With GST", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                decimal totalInvoice = totalTax + txtTOTALCOST1;
                tab2.AddCell(Build_TaxesTableBottomListStyle(totalInvoice.ToString("0.00"), 2));

                decimal decTcsValue = 0;
                SqlParameter[] spT = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                DataTable dtTcs = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_TcsTax_Status", spT, DataAccess.Return_Type.DataTable);
                if (dtTcs.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtTcs.Rows[0]["tcsApplicable"].ToString()))
                    {
                        tab2.AddCell(Build_TaxesTableBottomListStyle("TCS", 2));
                        decimal decTcsPercentage = Convert.ToDecimal(Convert.ToBoolean(dtTcs.Rows[0]["validPan"].ToString()) ? 0.075 : 0.75);
                        tab2.AddCell(Build_TaxesTableListStyle(decTcsPercentage.ToString() + "%", 2));
                        decTcsValue = Math.Round((totalInvoice * (decTcsPercentage / 100)), 0);
                        tab2.AddCell(Build_TaxesTableBottomListStyle(decTcsValue.ToString("0.00"), 2));
                    }
                }

                tab2.AddCell(Build_TaxesTableBottomListStyle("Total Invoice Value (in figure)", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                tab2.AddCell(Build_TaxesTableBottomListStyle((totalInvoice + decTcsValue).ToString("0.00"), 2));

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

                string[] strSplit = (totalInvoice + decTcsValue).ToString().Split('.');
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
                strSaleRules += "4. Interest @18% will be charged on overdue.\n";

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

                PdfPCell nesthousingTerms = new PdfPCell(termsTd);
                nesthousingTerms.Padding = 0f;
                nesthousingTerms.Colspan = 2;
                tab3.AddCell(nesthousingTerms);

                PdfPCell nesthousing1 = new PdfPCell(tab3);
                nesthousing1.Padding = 0f;
                nesthousing1.Colspan = 3;
                table.AddCell(nesthousing1);

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
                table.AddCell(cell);
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