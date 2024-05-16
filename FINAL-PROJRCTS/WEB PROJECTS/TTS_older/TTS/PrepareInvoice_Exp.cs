
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
        static string returnResult;
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

                //Build_TransportTable(table);
                Build_OrderItems(table);//*******************************
                if(returnResult == "mini")
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
                //Chunk postHeadCell = new Chunk("INVOICE\n", titleFont);changed by kannan
                Chunk postHeadCell = new Chunk("INVOICE\n", FontFactory.GetFont("Arial", 9, Font.NORMAL));
                PdfPCell nestHeadhousing = new PdfPCell(new Phrase(postHeadCell));
                nestHeadhousing.Colspan = 2;
                nestHeadhousing.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(nestHeadhousing);

               PdfPTable nested = new PdfPTable(1);
               float[] widths = new float[] { 45.3f };
                nested.SetWidths(widths);   
                SqlParameter[] spAdd = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                DataTable dtAdd = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ExportAddress_invoice", spAdd, DataAccess.Return_Type.DataTable);
                if (dtAdd.Rows.Count == 1)
                {
                    var headFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    Chunk postCell1 = new Chunk("Exporter:\n", headFont);
                    Chunk postCell2 = new Chunk(dtAdd.Rows[0]["AuthorizedAddress"].ToString() + "\n" +
                        dtAdd.Rows[0]["ExpoterAddress"].ToString().Replace("~", "\n"), titleFont);
                    Chunk postCell3 = new Chunk( "IEC CODE : " + dtAdd.Rows[0]["Iec_Code"].ToString() + "\n" +
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
                nested.TotalWidth = 140f;
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
                //PdfPTable nestedSub = new PdfPTable(1);

                PdfPTable nested = new PdfPTable(2);
                float[] widths = new float[] { 8.3f,13.3f };
                nested.SetWidths(widths);

                PdfPCell cell;
                var titleNormalFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var valBoldFont = FontFactory.GetFont("Arial", 12);
               
                cell = new PdfPCell(new Phrase("INVOICENO:", titleNormalFont));
                cell.Border = 0;
                nested.AddCell(cell);

                //cell = new PdfPCell(new Phrase(":", titleNormalFont));
                //cell.Border = 0;
                //nested.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.InvoiceNo, valBoldFont));
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase("Date :" , titleNormalFont));
                cell.Border = 0;
                nested.AddCell(cell);

                //cell = new PdfPCell(new Phrase(":", titleNormalFont));
                //cell.Border = 0;
                //nested.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.InvoiceDate, valBoldFont));
                cell.Border = 0;
                nested.AddCell(cell);

               // nestedSub.AddCell(nested);

                cell = new PdfPCell(new Phrase("Buyers Order No Ref :", titleNormalFont));
               // cell.Colspan = 3;
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase(Dm.OrderRefNo, valBoldFont));
                cell.Border = 0;
                nested.AddCell(cell);

               // nestedSub.AddCell(nested);

                cell = new PdfPCell(new Phrase("Other Reference(s) ", titleNormalFont));
                //cell.Colspan = 3;
                cell.Border = 0;
                nested.AddCell(cell);

                cell = new PdfPCell(new Phrase("\n", valBoldFont));
                cell.Border = 0;
                nested.AddCell(cell);

                            

                table.AddCell(nested);
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
                    cell.Colspan = 2;
                    cell.Border = 0;
                    tblNestedLeft.AddCell(cell);
                    cell = new PdfPCell(new Phrase(row["custfullname"].ToString() + "", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell.Border = 0;

                    tblNestedLeft.AddCell(cell);
                    cell = new PdfPCell(new Phrase("\n", boldFont));
                    cell.Border=0;
                    tblNestedLeft.AddCell(cell);

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
                    cell.Border = 0;
                    cell.Colspan = 2;
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
                DataTable dtTrans = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_invoice_details", sp1, DataAccess.Return_Type.DataTable);
                if (dtTrans.Rows.Count == 1)
                {
                    cell = new PdfPCell(new Phrase("BILL OF LADING NO:", portHeadFont));
                    //cell.Colspan = 2;
                    cell.Border = 0;
                    tblNestedLeft.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["Billoflading"].ToString(), portHeadFont));
                   cell.Colspan = 1;
                    
                    tblNestedLeft.AddCell(cell);
                    string carria=("Pre-Carriage By\n");
                    cell = new PdfPCell(new Phrase(carria, portHeadFont));
                    tblNestedLeft.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["PreCarriageBy"].ToString(), portValFont));
                    cell.Rowspan = 1;
                    tblNestedLeft.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["PreCarriageBy"].ToString(), portValFont));
                    //tblNestedLeft.AddCell(cell);
                    string recepit="Place of Receipt by Pre-Carrier";
                    
                    cell = new PdfPCell(new Phrase(recepit, portHeadFont));
                    cell.Rowspan = 1;
                    tblNestedLeft.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["Placeofrecepit"].ToString(), portValFont));
                    tblNestedLeft.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["PlaceOfReceiptByPreCarrier"].ToString(), portValFont));
                    //tblNestedLeft.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Vessel / Flight No:", portHeadFont));
                    //cell.Rowspan=2; 
                    tblNestedLeft.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["vessel/flightno"].ToString(), portValFont));
                    tblNestedLeft.AddCell(cell);
                    //string port = ("Port of Loading ");
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["Portofloading"].ToString(), portHeadFont));
                    tblNestedLeft.AddCell(cell);
                    //cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["PortOfLoading"].ToString(), portValFont));
                    //tblNestedLeft.AddCell(cell);
                    //string dishcharge = ($"Port of Discharge\n");
                    cell = new PdfPCell(new Phrase("Port of Discharge", portHeadFont));
                    tblNestedLeft.AddCell(cell);
                    //cell = new PdfPCell(new Phrase("\n", portValFont));
                    //tblNestedLeft.AddCell(cell);
                    string final = ("Final Destination\n");
                    cell = new PdfPCell(new Phrase(final, portHeadFont));
                    tblNestedLeft.AddCell(cell);
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

                    cell = new PdfPCell(new Phrase("Consignee\n ", boldFont));
                   
                    cell.Colspan = 2;
                    cell.Border = 0;
                    tblNestedRight.AddCell(cell);
                    cell = new PdfPCell(new Phrase(row["custfullname"].ToString() + "", FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell.Colspan = 2;
                    cell.Border = 0;
                    tblNestedRight.AddCell(cell);
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
                cell.Border = 0;
                tblNestedRight.AddCell(cell);

                if (dtTrans.Rows.Count == 1)
                {
                    cell = new PdfPCell(new Phrase("Country of Origin of Goods:", portHeadFont));

                    tblNestedRight.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["CountryOfOrigin"].ToString(), portValFont));
                    tblNestedRight.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Country of Final Destination\n:"));
                    tblNestedRight.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["Finaldestination"].ToString(), portValFont));
                    tblNestedRight.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Terms of Delivery and Payment", portHeadFont));
                    tblNestedRight.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtTrans.Rows[0]["payterms"].ToString(), portValFont));
                    
                    tblNestedRight.AddCell(cell);
                    cell.Colspan = 3;
                }
                table.AddCell(tblNestedRight);
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
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Dm.OID);
                 int balanceItem = dtItemList.Rows.Count;
                 
                 if (balanceItem <= 4)
                 {
                                         
                     var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                     PdfPTable dt = new PdfPTable(12);

                     
                     float[] widths = new float[] { 4.5f, 4f, 0.5f, 3f, 2f, 2f, 2f, 2f, 3f, 2, 2f, 5f };
                     
                     dt.TotalWidth = 560f;
                     dt.LockedWidth = true;
                     dt.SetWidths(widths);


                     //Order Items Heading
                     dt.AddCell(Build_ItemTableHeadStyle("NO&CONTAINER"));        //1
                     dt.AddCell(Build_ItemTableHeadStyle("NO& KIND OF PACKING"));  //2
                     PdfPCell cell = new PdfPCell(new Phrase("Description of Goods"));//9
                     cell.HorizontalAlignment = 0;
                     //cell.Border = 0;
                     cell.Colspan = 7;
                     cell.VerticalAlignment = Element.ALIGN_CENTER;

                     dt.AddCell(cell);     //4
                     // if (Dm.PartNo)

                     dt.AddCell(Build_ItemTableHeadStyle("QUANTITY IN NOS"));     //10
                     dt.AddCell(Build_ItemTableHeadStyle("UNIT PRICE in " + Dm.custcurrency + "\n"));    //11
                     dt.AddCell(Build_ItemTableHeadStyle("TOTAL VALUE IN " + Dm.custcurrency));//12
                    
                     SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                     DataTable dtcont = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cont_invoice_details", sp1, DataAccess.Return_Type.DataTable);
                                       
                         string c = "FCL\n" + dtcont.Rows[0]["Containertype"].ToString();

                         dt.AddCell(Build_ItemTableListStyle(c, 0));  //1
                         dt.AddCell(Build_ItemTableListStyle("I.T.C(H.S)CODE \n 40129020", 0));    //2
                         PdfPCell cellS = new PdfPCell(new Phrase("SOLID INDUSTRIAL TYTES"));  //9
                         cellS.HorizontalAlignment = 0;
                     
                         cellS.Colspan = 7;
                         cellS.VerticalAlignment = Element.ALIGN_CENTER;

                         dt.AddCell(cellS);

                         dt.AddCell(Build_ItemTableListStyle(".", 0));        //10
                         dt.AddCell(Build_ItemTableListStyle(".", 0));        //11
                         dt.AddCell(Build_ItemTableListStyle(".", 0));        //12


                         String ContainerNo = "Container No:\n" + dtcont.Rows[0]["Containerno"].ToString();
                         dt.AddCell(Build_ItemTableListStyle(ContainerNo, 0));  //1
                         int nocount = dtItemList.Rows.Count;
                         String Loose = dtItemList.Compute("Sum(itemqty)", "").ToString() + "LOOSE \n TYRES";
                         dt.AddCell(Build_ItemTableListStyle(Loose, 0));    //2
                         dt.AddCell(Build_ItemTableHeadStyle("SR.NO"));  //3
                         dt.AddCell(Build_ItemTableHeadStyle("SIZE"));//4
                         dt.AddCell(Build_ItemTableHeadStyle("RIM"));//5
                         dt.AddCell(Build_ItemTableHeadStyle("TREAD"));//6
                         dt.AddCell(Build_ItemTableHeadStyle("TYPE"));//7
                         dt.AddCell(Build_ItemTableHeadStyle("BRAND"));//8
                         dt.AddCell(Build_ItemTableHeadStyle("SIDEWALL"));//9
                         dt.AddCell(Build_ItemTableListStyle("quantity", 0));//10
                         dt.AddCell(Build_ItemTableListStyle("total", 0));//11
                         dt.AddCell(Build_ItemTableListStyle("     ", 0));//12
                         String SEALNO = "SEAL No:\n" + dtcont.Rows[0]["Containerseal"].ToString();

                         SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                         DataTable dtItemsList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_orderitemlist_for_proforma", sp2, DataAccess.Return_Type.DataTable);

                         int j = 1;
                         DataView dtDescView = new DataView(dtItemList);
                         dtDescView.Sort = "brand,AssyRimstatus ASC";
                         DataTable distinctDesc = dtDescView.ToTable(true, "brand", "tyretype", "TypeDesc", "category", "AssyRimstatus", "RimDwg", "EdcNo");
                         foreach (DataRow dRow in distinctDesc.Rows)
                         {
                             if (j == (nocount / 2))
                                 dt.AddCell(Build_ItemTableListStyle(SEALNO, 0));


                             dt.AddCell(Build_ItemTableListStyle(" ", 0));//1
                             dt.AddCell(Build_ItemTableListStyle(" ", 0));//2

                             string strItemDesc = dRow["brand"].ToString() != "" ? dRow["brand"].ToString() + ", " : "";
                             strItemDesc += dRow["TypeDesc"].ToString() != "" ? dRow["TypeDesc"].ToString() + ", " : "";
                             strItemDesc += dRow["tyretype"].ToString() != "" ? dRow["tyretype"].ToString() : "";
                             string strLimpet = dRow["tyretype"].ToString() != "" ? ((dRow["tyretype"].ToString()).Substring(dRow["tyretype"].ToString().Length - 1, 1).ToUpper()) : "";
                             strItemDesc += strLimpet == "X" ? " WITHOUT CLIP / LIMPET" : strLimpet == "4" ? " WITH CLIP / LIMPET" : "";
                             if (Convert.ToBoolean(dRow["AssyRimstatus"].ToString()) == true)
                                 strItemDesc += " WITH ASSY (" + dRow["EdcNo"].ToString().ToUpper() + " / " + dRow["RimDwg"].ToString().ToUpper() + ")";

                             strItemDesc = strItemDesc != "" ? strItemDesc : dRow["category"].ToString() + (dRow["EdcNo"].ToString() != "" ? " (" + dRow["EdcNo"].ToString() + " / " +
                                 dRow["RimDwg"].ToString().ToUpper() + ")" : "");
                             dt.AddCell(Build_ItemTableBottomStyle(strItemDesc.ToUpper(), 0, 10));//12
                             foreach (DataRow row in dtItemList.Select("category='" + dRow["category"].ToString() + "' and brand='" + dRow["brand"].ToString()
                                 + "' and tyretype='" + dRow["tyretype"].ToString() + "' and TypeDesc='" + dRow["TypeDesc"].ToString() + "' and AssyRimstatus='" +
                                 dRow["AssyRimstatus"].ToString() + "' and EdcNo='" + dRow["EdcNo"].ToString() + "'"))
                             {
                                 if (Convert.ToBoolean(row["AssyRimstatus"]) == false)
                                 {
                                    // dt.AddCell(Build_ItemTableListStyle(" ", 0));//1
                                    // dt.AddCell(Build_ItemTableListStyle(" ", 0));//2
                                     dt.AddCell(Build_ItemTableListStyle(j.ToString(), 0));//3
                                     dt.AddCell(Build_ItemTableListStyle(row["Tyresize"].ToString(), 0));//4
                                     dt.AddCell(Build_ItemTableListStyle(row["RimSize"].ToString(), 0));//5
                                     dt.AddCell(Build_ItemTableListStyle(row["Config"].ToString(), 0));//6
                                     dt.AddCell(Build_ItemTableListStyle(row["TyreType"].ToString(), 0));//7
                                     dt.AddCell(Build_ItemTableListStyle(row["Brand"].ToString(), 0));//8
                                     dt.AddCell(Build_ItemTableListStyle(row["sidewall"].ToString(), 0));//9
                                     dt.AddCell(Build_ItemTableListStyle(row["ItemQty"].ToString(), 2));//10
                                     dt.AddCell(Build_ItemTableListStyle(row["listprice"].ToString(), 2));//11
                                     dt.AddCell(Build_ItemTableListStyle(row["unitpricepdf"].ToString(), 2));//12
                                   
                                 }
                                 j++;

                             }

                         }

                         dt.AddCell(Build_ItemTableBottomStyle("SUB TOTAL", 2, 9));
                         dt.AddCell(Build_ItemTableBottomStyle(dtItemList.Compute("Sum(itemqty)", "").ToString(), 2, 1));
                         dt.AddCell(Build_ItemTableBottomStyle(dtItemList.Compute("Sum(unitprice)", "").ToString(), 2, 2));
                         
                         SqlParameter[] spdiscount = new SqlParameter[] { new SqlParameter("@OID", Dm.OID) };
                         DataTable dtdiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_Discount", spdiscount, DataAccess.Return_Type.DataTable);
                         for (int i = 0; i < dtdiscount.Rows.Count; i++)
                         {
                             dt.AddCell(Build_ItemTableBottomStyle(dtdiscount.Rows[i]["Category"].ToString() + ": " + dtdiscount.Rows[i]["DescriptionDiscount"].ToString(), 2, 9));
                             dt.AddCell(Build_ItemTableBottomStyle("", 2, 1));
                             dt.AddCell(Build_ItemTableBottomStyle(dtdiscount.Rows[i]["Amount"].ToString(), 2, 2));
                             dt.AddCell(Build_ItemTableBottomStyle("\n", 2, 2));
                         }
                         dt.AddCell(Build_ItemTableBottomStyle("TOTAL " + Dm.custcurrency, 2, 7));
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
                         var titleFont1 = FontFactory.GetFont("Arial", 8, Font.BOLD);

                         string[] strSplit = txtTOTALCOST1.ToString().Split('.');
                         int intPart = Convert.ToInt32(strSplit[0].ToString());
                         string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
                         string text = Dm.custcurrency + " " + Utilities.NumberToText(intPart, true, false).ToString().ToUpper();
                         if (Convert.ToDecimal(decimalPart) > 0)
                             text += " AND CENTS" + Utilities.DecimalToText(decimalPart).ToUpper() + " ONLY";
                         else
                             text += " ONLY";

                         Chunk p2 = new Chunk(text + "\n", titleFont);
                         Paragraph para = new Paragraph();
                         
                         para.Add(p2);
                         PdfPCell cell1 = new PdfPCell(new Phrase(para));
                         cell1.HorizontalAlignment = 0;
                         cell1.VerticalAlignment = PdfPCell.ALIGN_TOP;
                         cell1.FixedHeight = 30f;
                         cell1.Colspan = 12;
                         dt.AddCell(cell1);
                         PdfPCell nesthousing1 = new PdfPCell(dt);
                         nesthousing1.Padding = 0f;
                         nesthousing1.Colspan = 6;
                         table.AddCell(nesthousing1);
                         returnResult = "mini";
                     }
                 
                 else
                 {
                     returnResult = "multi";
                                        
                     var titleFont = FontFactory.GetFont("Arial", 7, Font.NORMAL);
                     PdfPTable dt = new PdfPTable(5);// TABLE COLUMN
                     float[] widths = new float[] { 3f, 3f,11f, 3f,5f };
                     dt.TotalWidth = 560f;
                     dt.LockedWidth = true;
                     dt.SetWidths(widths);
                     dt.AddCell(Build_ItemTableHeadStyle("Marks & Nos/ContainerNo"));        //1
                     dt.AddCell(Build_ItemTableHeadStyle("No & Kind Of Packing"));  //2

                     PdfPCell cell = new PdfPCell(new Phrase("Description of Goods"));//3
                     cell.HorizontalAlignment = 0;
                     cell.VerticalAlignment = Element.ALIGN_CENTER;
                     dt.AddCell(cell);     //4

                     dt.AddCell(Build_ItemTableHeadStyle("Quantity In Nos"));     //4
                     dt.AddCell(Build_ItemTableHeadStyle("Total Value In " + Dm.custcurrency));//5
                     SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                     DataTable dtcont = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cont_invoice_details", sp1, DataAccess.Return_Type.DataTable);
                     //DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Dm.OID);

                     string c = "FCL\n" + dtcont.Rows[0]["Containertype"].ToString();

                     dt.AddCell(Build_ItemTableListStyle(c, 0));  //1
                     dt.AddCell(Build_ItemTableListStyle("I.T.C(H.S)CODE \n 40129020", 0));    //2
                     PdfPCell cellS = new PdfPCell(new Phrase("SOLID INDUSTRIAL TYRES"));  
                     cellS.HorizontalAlignment = 0;
                     cellS.VerticalAlignment = Element.ALIGN_CENTER;

                     dt.AddCell(cellS);//3
                     
                     dt.AddCell("..");//4
                     dt.AddCell("..");//5
                     dt.AddCell("..");//1
                     dt.AddCell("..");//2
                     PdfPCell midcellS = new PdfPCell(new Phrase("As Per The Annexure Enclosed"));
                     midcellS.HorizontalAlignment = 0;
                     midcellS.VerticalAlignment = Element.ALIGN_CENTER;
                     dt.AddCell(midcellS);//3
                     dt.AddCell("..");//4
                    
                     Decimal txtTOTALCOST1 = 0;
                     txtTOTALCOST1 = txtTOTALCOST1 + (Convert.ToDecimal(dtItemList.Compute("Sum(unitprice)", "").ToString()));
                     dt.AddCell(Build_ItemTableBottomStyle(txtTOTALCOST1.ToString(), 2, 1));
                     dt.AddCell("..");//1
                     dt.AddCell("..");//2
                                         
                     dt.AddCell(Build_ItemTableBottomStyle("TOTAL " + Dm.custcurrency, 2, 1));//4
                     dt.AddCell(Build_ItemTableBottomStyle("", 2, 1));//5
                      txtTOTALCOST1 = 0;
                     txtTOTALCOST1 = txtTOTALCOST1 + (Convert.ToDecimal(dtItemList.Compute("Sum(unitprice)", "").ToString()));
           
                     dt.AddCell(Build_ItemTableBottomStyle(txtTOTALCOST1.ToString(), 2, 1));
                     dt.AddCell(Build_ItemTableBottomStyle("\n", 2, 2));
                     var titleFont1 = FontFactory.GetFont("Arial", 8, Font.BOLD);

                     string[] strSplit = txtTOTALCOST1.ToString().Split('.');
                     int intPart = Convert.ToInt32(strSplit[0].ToString());
                     string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
                     string text = Dm.custcurrency + " " + Utilities.NumberToText(intPart, true, false).ToString().ToUpper();
                     if (Convert.ToDecimal(decimalPart) > 0)
                         text += " AND CENTS" + Utilities.DecimalToText(decimalPart).ToUpper() + " ONLY";
                     else
                         text +=  " ONLY";

                    

                     Chunk p2 = new Chunk(text + "\n", titleFont);
                     
                     Paragraph para = new Paragraph();                     
                     para.Add(p2);
                     PdfPCell cell1 = new PdfPCell(new Phrase(para));
                     cell1.HorizontalAlignment = 0;;
                     cell1.VerticalAlignment = PdfPCell.ALIGN_TOP;
                     cell1.FixedHeight = 30f;
                     cell1.Colspan = 12;

                     dt.AddCell(cell1);
                     PdfPCell nesthousing1 = new PdfPCell(dt);
                     nesthousing1.Padding = 0f;
                     nesthousing1.Colspan = 6;
                    
                     table.AddCell(nesthousing1);
                     //************************************************************************************************
                      Build_BottomDetails(table);
                     //********************************Annexure start below********************************************************
                     // var titleFont2 = FontFactory.GetFont("Arial", 9, Font.NORMAL);

                     PdfPTable dt_01 = new PdfPTable(10);                     
                     float[] widths_01 = new float[] { 1.5f, 4f, 2.5f, 3f, 2f, 3.5f, 3.5f, 2f, 3f, 5f };                    
                     dt_01.TotalWidth = 560f;
                     dt_01.LockedWidth = true;
                     dt_01.SetWidths(widths_01);

                         dt_01.AddCell(Build_ItemTableHeadStyle("SR.NO"));  //1
                         dt_01.AddCell(Build_ItemTableHeadStyle("SIZE"));//2
                         dt_01.AddCell(Build_ItemTableHeadStyle("RIM"));//3
                         dt_01.AddCell(Build_ItemTableHeadStyle("PLATFORM"));//4
                         dt_01.AddCell(Build_ItemTableHeadStyle("TYPE"));//5
                         dt_01.AddCell(Build_ItemTableHeadStyle("BRAND"));//6
                         dt_01.AddCell(Build_ItemTableHeadStyle("SIDEWALL"));//7
                         dt_01.AddCell(Build_ItemTableHeadStyle("QTY IN NOS"));//8
                         dt_01.AddCell(Build_ItemTableHeadStyle("UNIT PRICE"));//9
                         dt_01.AddCell(Build_ItemTableHeadStyle("TOTAL VALUE"));//10 

                         SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@O_ID", Dm.OID) };
                         DataTable dtItemsList_01 = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_orderitemlist_for_proforma", sp2, DataAccess.Return_Type.DataTable);

                         int j = 1;
                         DataView dtDescView_01 = new DataView(dtItemsList_01);
                         dtDescView_01.Sort = "brand,AssyRimstatus ASC";
                         DataTable distinctDesc_01 = dtDescView_01.ToTable(true, "brand", "tyretype", "TypeDesc", "category", "AssyRimstatus", "RimDwg", "EdcNo");
                         foreach (DataRow dRow in distinctDesc_01.Rows)
                         {
                             

                             string strItemDesc_01 = dRow["brand"].ToString() != "" ? dRow["brand"].ToString() + ", " : "";
                             strItemDesc_01 += dRow["TypeDesc"].ToString() != "" ? dRow["TypeDesc"].ToString() + ", " : "";
                             strItemDesc_01 += dRow["tyretype"].ToString() != "" ? dRow["tyretype"].ToString() : "";
                             string strLimpet_01 = dRow["tyretype"].ToString() != "" ? ((dRow["tyretype"].ToString()).Substring(dRow["tyretype"].ToString().Length - 1, 1).ToUpper()) : "";
                             strItemDesc_01 += strLimpet_01 == "X" ? " WITHOUT CLIP / LIMPET" : strLimpet_01 == "4" ? " WITH CLIP / LIMPET" : "";
                             if (Convert.ToBoolean(dRow["AssyRimstatus"].ToString()) == true)
                                 strItemDesc_01 += " WITH ASSY (" + dRow["EdcNo"].ToString().ToUpper() + " / " + dRow["RimDwg"].ToString().ToUpper() + ")";

                             strItemDesc_01 = strItemDesc_01 != "" ? strItemDesc_01 : dRow["category"].ToString() + (dRow["EdcNo"].ToString() != "" ? " (" + dRow["EdcNo"].ToString() + " / " +
                                 dRow["RimDwg"].ToString().ToUpper() + ")" : "");
                             dt_01.AddCell(Build_ItemTableBottomStyle(strItemDesc_01.ToUpper(), 0, 10));//12
                             foreach (DataRow row in dtItemsList_01.Select("category='" + dRow["category"].ToString() + "' and brand='" + dRow["brand"].ToString()
                                 + "' and tyretype='" + dRow["tyretype"].ToString() + "' and TypeDesc='" + dRow["TypeDesc"].ToString() + "' and AssyRimstatus='" +
                                 dRow["AssyRimstatus"].ToString() + "' and EdcNo='" + dRow["EdcNo"].ToString() + "'"))
                             {
                                 if (Convert.ToBoolean(row["AssyRimstatus"]) == false)
                                 {
                                   
                                     dt_01.AddCell(Build_ItemTableListStyle(j.ToString(), 0));//3
                                     dt_01.AddCell(Build_ItemTableListStyle(row["Tyresize"].ToString(), 0));//4
                                     dt_01.AddCell(Build_ItemTableListStyle(row["RimSize"].ToString(), 0));//5
                                     dt_01.AddCell(Build_ItemTableListStyle(row["Config"].ToString(), 0));//6
                                     dt_01.AddCell(Build_ItemTableListStyle(row["TyreType"].ToString(), 0));//7
                                     dt_01.AddCell(Build_ItemTableListStyle(row["Brand"].ToString(), 0));//8
                                     dt_01.AddCell(Build_ItemTableListStyle(row["sidewall"].ToString(), 0));//9
                                     dt_01.AddCell(Build_ItemTableListStyle(row["ItemQty"].ToString(), 2));//10
                                     dt_01.AddCell(Build_ItemTableListStyle(row["listprice"].ToString(), 2));//11
                                     dt_01.AddCell(Build_ItemTableListStyle(row["unitpricepdf"].ToString(), 2));//12
                                   
                                 }
                                 j++;

                             }

                         }
                         //*******************************************************************
                        // dt_01.AddCell(Build_ItemTableBottomStyle("SUB TOTAL", 2, 9));
                       //dt_01.AddCell(Build_ItemTableBottomStyle(dtItemList.Compute("Sum(itemqty)", "").ToString(), 2, 1));
                      //  dt_01.AddCell(Build_ItemTableBottomStyle(dtItemList.Compute("Sum(unitprice)", "").ToString(), 2, 2));
                     //*******************************************************************    

                         SqlParameter[] spdiscount = new SqlParameter[] { new SqlParameter("@OID", Dm.OID) };
                         DataTable dtdiscount_01 = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_Discount", spdiscount, DataAccess.Return_Type.DataTable);
                         for (int i = 0; i < dtdiscount_01.Rows.Count; i++)
                         {
                             dt_01.AddCell(Build_ItemTableBottomStyle(dtdiscount_01.Rows[i]["Category"].ToString() + ": " + dtdiscount_01.Rows[i]["DescriptionDiscount"].ToString(), 2, 9));
                             dt_01.AddCell(Build_ItemTableBottomStyle("", 2, 1));
                             dt_01.AddCell(Build_ItemTableBottomStyle(dtdiscount_01.Rows[i]["Amount"].ToString(), 2, 2));
                             dt_01.AddCell(Build_ItemTableBottomStyle("\n", 2, 2));
                         }
                         dt_01.AddCell(Build_ItemTableBottomStyle("TOTAL " + Dm.custcurrency, 2, 9));
                        
                         
                         Decimal txtTOTALCOST_01 = 0;
                         txtTOTALCOST_01 = txtTOTALCOST_01 + (Convert.ToDecimal(dtItemList.Compute("Sum(unitprice)", "").ToString()));
                         for (int i = 0; i < dtdiscount_01.Rows.Count; i++)
                         {
                             if (dtdiscount_01.Rows[i]["Category"].ToString() == "ADD")
                             {
                                 txtTOTALCOST_01 += (Convert.ToDecimal(dtdiscount_01.Rows[i]["Amount"]));
                             }
                             else if (dtdiscount_01.Rows[i]["Category"].ToString() == "LESS")
                             {
                                 txtTOTALCOST_01 -= (Convert.ToDecimal(dtdiscount_01.Rows[i]["Amount"]));
                             }
                         }
                         dt_01.AddCell(Build_ItemTableBottomStyle(txtTOTALCOST_01.ToString(), 2, 0));
                        
                       
                         var titleFont_01 = FontFactory.GetFont("Arial", 6, Font.BOLD);

                         string[] strSplit_01= txtTOTALCOST_01.ToString().Split('.');//****************************************
                         int intPart_01 = Convert.ToInt32(strSplit[0].ToString());//****************************************
                         string decimalPart_01 = strSplit.Length > 1 ? strSplit[1].ToString() : "00";//****************************************
                         string text_01 = Dm.custcurrency + " " + Utilities.NumberToText(intPart, true, false).ToString().ToUpper();
                         if (Convert.ToDecimal(decimalPart) > 0)
                             text_01 += " AND CENTS" + Utilities.DecimalToText(decimalPart).ToUpper() + " ONLY";
                         else
                             text_01 += " ONLY";
                        
                         Chunk p3 = new Chunk(text_01 + "\n", titleFont);
                         Paragraph para_01 = new Paragraph();
                         //para.Add(p1);
                         para_01.Add(p2);
                         PdfPCell cell_01 = new PdfPCell(new Phrase(para_01));
                         cell_01.HorizontalAlignment = 0;
                         cell_01.VerticalAlignment = PdfPCell.ALIGN_TOP;
                         cell_01.FixedHeight = 30f;
                         cell_01.Colspan = 12;
                         dt_01.AddCell(cell_01);
                         PdfPCell nesthousing_01 = new PdfPCell(dt_01);
                         nesthousing_01.Padding = 0f;
                         nesthousing_01.Colspan = 6;
                         table.AddCell(nesthousing_01);
                        //************************************************************************

                         var titleFont_02 = FontFactory.GetFont("Arial", 4, Font.NORMAL);
                         PdfPTable dt_02 = new PdfPTable(10);// WHICH CONTAINS 5TH TABLE COLUMN                    
                         dt_02.TotalWidth = 560f;
                         dt_02.LockedWidth = true;
                         //dt_02.DefaultCellBorder = Rectangle.NO_BORDER;//it doesnot work
                         //dt_02.Border = Rectangle.NO_BORDER;
                         //dt_02.SetWidths(widths);
                         dt_02.AddCell(Build_ItemTableBottomStylesp("FOR SUN TYRE & WHEEL SYSTEMS",0,10));        //1
                         dt_02.AddCell(Build_ItemTableBottomStylesp(" ", 0, 10));        //2
                         dt_02.AddCell(Build_ItemTableBottomStylesp("(A DIVISION OF SUNDARAM INDUSTRIES PRIVATE LIMITED",0,10));  //3
                         dt_02.AddCell(Build_ItemTableBottomStylesp(" ", 0, 10));   //4
                         dt_02.AddCell(Build_ItemTableBottomStylesp(" ", 0, 10));   //5
                         dt_02.AddCell(Build_ItemTableBottomStylesp(" ", 0, 10));   //6
                         dt_02.AddCell(Build_ItemTableBottomStylesp(" ", 0, 10));   //7
                         dt_02.AddCell(Build_ItemTableBottomStylesp("AUTORIZED SIGNATORY",0,10));  //8

                         PdfPCell nesthousing_02 = new PdfPCell(dt_02);
                         nesthousing_02.Padding = 0f;
                         nesthousing_02.Colspan = 10;
                         nesthousing_02.Border = 0;
                         //nesthousing_02.BorderWidthRight= 0;
                         //nesthousing_02.BorderWidthBottom  = 0;
                        // nesthousing_02.BorderWidthTop = 0;
                        // nesthousing_02.BorderWidth  = 0;
                          
                         
                         table.AddCell(nesthousing_02);
                        
                        

//******************************************************************************************************************************************
                      
  
                 }//else
                 return;

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
        static PdfPCell Build_ItemTableBottomStylesp(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            cell.Border = 0;
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