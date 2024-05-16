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
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TTS
{
    public class Exp_PDI_Report
    {
        // Static Variables
        public static string CustomerName { get; set; }
        public static string OrderNumber { get; set; }
        public static string OrderQuantity { get; set; }
        public static string InspectedBy { get; set; }
        public static string VerifiedBy { get; set; }
        public static string ApprovedBy { get; set; }
        public static DataTable dtScanList { get; set; }

        //Document Creation
        public static void PDI_Generation(string FileSavePath)
        {
            try
            {
                Document document = new Document(PageSize.A4.Rotate(), 5f, 5f, 20f, 15f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(FileSavePath, FileMode.Create));
                writer.PageEvent = new PDFWriterEvents("PDI REPORT", 50f, 450f, 240f, 45f);
                document.Open();
                document.Add(Build_Table1_MainHeader("PDI REPORT"));
                document.Add(Build_Table2_SubHeader());
                document.Add(Build_Table4_SubHeader());
                int i = 1;
                foreach (DataRow dr in dtScanList.Rows)
                {
                    if (i <= dtScanList.Rows.Count)
                    {
                        document.Add(Build_Table5_Content(i, dr));
                        i++;
                    }
                }
                for (int increment = dtScanList.Rows.Count + 1; increment <= 20; increment++)
                    document.Add(Build_Table5_Content(increment));
                document.Add(Build_Table6_Footer());
                document.Close();

                byte[] bytes = File.ReadAllBytes(FileSavePath);
                Font blackFont = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK);
                using (MemoryStream stream = new MemoryStream())
                {
                    PdfReader reader = new PdfReader(bytes);
                    using (PdfStamper stamper = new PdfStamper(reader, stream))
                    {
                        int pages = reader.NumberOfPages;
                        for (int z = 1; z <= pages; z++)
                        {
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(z), Element.ALIGN_RIGHT, new Phrase("Page " + z.ToString() + " of " + pages, blackFont), 698f, 3f, 0);
                        }
                    }
                    bytes = stream.ToArray();
                }
                File.WriteAllBytes(FileSavePath, bytes);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("PDI Creation", HttpContext.Current.Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        //Build Heading
        static PdfPTable Build_Table1_MainHeader(string Heading)
        {
            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 3f, 17f, 3.5f, 3.5f, 3.5f };
            table.SetWidths(widths);

            //First Cell
            PdfPTable nestedtable1_img = new PdfPTable(1);
            string imageFilePath = HttpContext.Current.Server.MapPath("~/images/tvs_suntws.jpg");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageFilePath);
            img.ScaleToFit(75f, 20f);
            PdfPCell nestedtable1_img_cell1 = new PdfPCell(img);
            nestedtable1_img_cell1.MinimumHeight = 30f;
            nestedtable1_img_cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_img_cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1_img.AddCell(nestedtable1_img_cell1);

            Chunk chk1 = new Chunk("SUN-TWS", Pdf_Fonts(2));
            PdfPCell nestedtable1_img_cell2 = new PdfPCell(new Phrase(chk1));
            nestedtable1_img_cell2.MinimumHeight = 10f;
            nestedtable1_img_cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_img_cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1_img.AddCell(nestedtable1_img_cell2);
            PdfPCell cell1 = new PdfPCell(nestedtable1_img);
            table.AddCell(cell1);

            //Second Cell
            Chunk chk2 = new Chunk(Heading, Pdf_Fonts(1));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell2);

            //Third Cell
            PdfPTable nestedtable1 = new PdfPTable(1);
            float[] width1 = new float[] { 20f };
            nestedtable1.SetWidths(width1);
            Chunk nestedtable1_chk1 = new Chunk("Inspected", Pdf_Fonts(3));
            PdfPCell nestedtable1_cell1 = new PdfPCell(new Phrase(nestedtable1_chk1));
            nestedtable1_cell1.MinimumHeight = 10;
            nestedtable1_cell1.Padding = 5;
            nestedtable1_cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell1);

            Chunk chk3 = new Chunk(InspectedBy, Pdf_Fonts(4));
            PdfPCell nestedtable1_cell2 = new PdfPCell(new Phrase(chk3));
            nestedtable1_cell2.MinimumHeight = 30;
            nestedtable1_cell2.Padding = 5;
            nestedtable1_cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell2);
            PdfPCell cell3 = new PdfPCell(nestedtable1);
            table.AddCell(cell3);

            //Fourth Cell
            PdfPTable nestedtable2 = new PdfPTable(1);
            float[] width2 = new float[] { 20f };
            nestedtable2.SetWidths(width2);
            Chunk nestedtable2_chk1 = new Chunk("Verified", Pdf_Fonts(3));
            PdfPCell nestedtable2_cell1 = new PdfPCell(new Phrase(nestedtable2_chk1));
            nestedtable2_cell1.MinimumHeight = 10;
            nestedtable2_cell1.Padding = 5;
            nestedtable2_cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable2_cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable2.AddCell(nestedtable2_cell1);

            Chunk chk4 = new Chunk(VerifiedBy.ToUpper(), Pdf_Fonts(4));
            PdfPCell nestedtable2_cell2 = new PdfPCell(new Phrase(chk4));
            nestedtable2_cell2.MinimumHeight = 30;
            nestedtable2_cell2.Padding = 5;
            nestedtable2_cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable2_cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable2.AddCell(nestedtable2_cell2);
            PdfPCell cell4 = new PdfPCell(nestedtable2);
            table.AddCell(cell4);

            //Fifth Cell
            PdfPTable nestedtable3 = new PdfPTable(1);
            float[] width3 = new float[] { 20f };
            nestedtable1.SetWidths(width3);
            Chunk nestedtable3_chk1 = new Chunk("Approved", Pdf_Fonts(3));
            PdfPCell nestedtable3_cell1 = new PdfPCell(new Phrase(nestedtable3_chk1));
            nestedtable3_cell1.MinimumHeight = 10;
            nestedtable3_cell1.Padding = 5;
            nestedtable3_cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable3_cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable3.AddCell(nestedtable3_cell1);

            Chunk chk5 = new Chunk(ApprovedBy, Pdf_Fonts(4));
            PdfPCell nestedtable3_cell2 = new PdfPCell(new Phrase(chk5));
            nestedtable3_cell2.MinimumHeight = 30;
            nestedtable3_cell2.Padding = 5;
            nestedtable3_cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable3_cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable3.AddCell(nestedtable3_cell2);
            PdfPCell cell5 = new PdfPCell(nestedtable3);
            table.AddCell(cell5);

            return table;
        }
        //Build Customer Details
        static PdfPTable Build_Table2_SubHeader()
        {
            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 3f, 12.5f, 3f, 8f, 2f, 1.5f };
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk("Customer ", Pdf_Fonts(3));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.FixedHeight = 20f;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk(CustomerName, Pdf_Fonts(4));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.FixedHeight = 20f;
            table.AddCell(cell2);

            //3st Column
            Chunk chk3 = new Chunk("Order Number ", Pdf_Fonts(3));
            PdfPCell cell3 = new PdfPCell(new Phrase(chk3));
            cell3.FixedHeight = 20f;
            table.AddCell(cell3);

            //4nd Column
            Chunk chk4 = new Chunk(OrderNumber, Pdf_Fonts(4));
            PdfPCell cell4 = new PdfPCell(new Phrase(chk4));
            cell4.FixedHeight = 20f;
            table.AddCell(cell4);

            //5rd Column
            Chunk chk5 = new Chunk("Order Qty ", Pdf_Fonts(3));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.FixedHeight = 20f;
            table.AddCell(cell5);

            //6th Column
            Chunk chk6 = new Chunk(OrderQuantity, Pdf_Fonts(4));
            PdfPCell cell6 = new PdfPCell(new Phrase(chk6));
            cell6.FixedHeight = 20f;
            table.AddCell(cell6);

            return table;
        }
        //Build Header for Jathagam
        static PdfPTable Build_Table4_SubHeader()
        {
            PdfPTable table = new PdfPTable(12);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            //                             SNO   DATE  STEN  SIZE  RIM   TYP   APP   GUA   HARD  GRAD  QUA  REMA
            float[] widths = new float[] { 1.1f, 2.2f, 2.6f, 3.8f, 1.3f, 2.3f, 8.5f, 1.7f, 2.6f, 1.4f, 3.5f, 4f };
            
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk("S.No", Pdf_Fonts(3));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.FixedHeight = 40f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk("Inspection Date", Pdf_Fonts(3));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.FixedHeight = 40f;
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell2);

            //3rd Column
            Chunk chk3 = new Chunk("Tyre Number", Pdf_Fonts(3));
            PdfPCell cell3 = new PdfPCell(new Phrase(chk3));
            cell3.FixedHeight = 40f;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell3);

            //4th Column
            Chunk chk4 = new Chunk("Tyre Size", Pdf_Fonts(3));
            PdfPCell cell4 = new PdfPCell(new Phrase(chk4));
            cell4.FixedHeight = 40f;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell4);

            //5th Column
            Chunk chk5 = new Chunk("Rim \nWidth", Pdf_Fonts(3));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.FixedHeight = 40f;
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell5);

            //6th Column
            Chunk chk6 = new Chunk("Compound / Type", Pdf_Fonts(3));
            PdfPCell cell6 = new PdfPCell(new Phrase(chk6));
            cell6.FixedHeight = 40f;
            cell6.HorizontalAlignment = Element.ALIGN_CENTER;
            cell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell6);

            //7th Column
            PdfPTable nestedtable1 = new PdfPTable(2);
            float[] width = new float[] { 6.5f, 1.5f };
            nestedtable1.SetWidths(width);
            Chunk nestedtable1_chk1 = new Chunk("Appearance", Pdf_Fonts(3));
            PdfPCell nestedtable1_cell1 = new PdfPCell(new Phrase(nestedtable1_chk1));
            nestedtable1_cell1.Colspan = 2;
            nestedtable1_cell1.MinimumHeight = 10f;
            nestedtable1_cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell1);

            Chunk nestedtable1_chk2 = new Chunk("Visual \n(Brand / Message / Platform)", Pdf_Fonts(3));
            PdfPCell nestedtable1_cell2 = new PdfPCell(new Phrase(nestedtable1_chk2));
            nestedtable1_cell2.MinimumHeight = 30f;
            nestedtable1_cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell2);

            Chunk nestedtable1_chk3 = new Chunk("Bead Type", Pdf_Fonts(3));
            PdfPCell nestedtable1_cell3 = new PdfPCell(new Phrase(nestedtable1_chk3));
            nestedtable1_cell3.MinimumHeight = 30f;
            nestedtable1_cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell3);

            PdfPCell cell7 = new PdfPCell(nestedtable1);
            table.AddCell(cell7);

            //8th Column
            Chunk chk8 = new Chunk("ID\n(Gauge)", Pdf_Fonts(3));
            PdfPCell cell8 = new PdfPCell(new Phrase(chk8));
            cell8.FixedHeight = 40f;
            cell8.HorizontalAlignment = Element.ALIGN_CENTER;
            cell8.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell8);

            //9th Column
            PdfPTable nestedtable2 = new PdfPTable(2);
            float[] width2 = new float[] { 1.3f, 1.3f };
            nestedtable2.SetWidths(width2);
            Chunk nestedtable2_chk1 = new Chunk("Hardness", Pdf_Fonts(3));
            PdfPCell nestedtable2_cell1 = new PdfPCell(new Phrase(nestedtable2_chk1));
            nestedtable2_cell1.Colspan = 2;
            nestedtable2_cell1.MinimumHeight = 10f;
            nestedtable2_cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable2_cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable2.AddCell(nestedtable2_cell1);

            Chunk nestedtable2_chk2 = new Chunk("Tread", Pdf_Fonts(3));
            PdfPCell nestedtable2_cell2 = new PdfPCell(new Phrase(nestedtable2_chk2));
            nestedtable2_cell2.MinimumHeight = 30f;
            nestedtable2_cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable2_cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable2.AddCell(nestedtable2_cell2);

            Chunk nestedtable2_chk3 = new Chunk("Base", Pdf_Fonts(3));
            PdfPCell nestedtable2_cell3 = new PdfPCell(new Phrase(nestedtable2_chk3));
            nestedtable2_cell3.MinimumHeight = 30f;
            nestedtable2_cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable2_cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable2.AddCell(nestedtable2_cell3);

            PdfPCell cell9 = new PdfPCell(nestedtable2);
            table.AddCell(cell9);

            //10th Column
            Chunk chk10 = new Chunk("Grade", Pdf_Fonts(3));
            PdfPCell cell10 = new PdfPCell(new Phrase(chk10));
            cell10.FixedHeight = 40f;
            cell10.HorizontalAlignment = Element.ALIGN_CENTER;
            cell10.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell10);

            //11th Column
            Chunk chk11 = new Chunk("Best Quality test\n(only for POB)", Pdf_Fonts(3));
            PdfPCell cell11 = new PdfPCell(new Phrase(chk11));
            cell11.FixedHeight = 40f;
            cell11.HorizontalAlignment = Element.ALIGN_CENTER;
            cell11.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell11);

            //12th Column
            Chunk chk12 = new Chunk("Remarks", Pdf_Fonts(3));
            PdfPCell cell12 = new PdfPCell(new Phrase(chk12));
            cell12.FixedHeight = 40f;
            cell12.HorizontalAlignment = Element.ALIGN_CENTER;
            cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell12);

            return table;
        }
        //Get Values  Jathagam
        static PdfPTable Build_Table5_Content(int No, DataRow dr)
        {
            PdfPTable table = new PdfPTable(14);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1.1f, 2.2f, 2.6f, 3.8f, 1.3f, 2.3f, 6.9f, 1.6f, 1.7f, 1.3f, 1.3f, 1.4f, 3.5f, 4f };
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk(No.ToString(), Pdf_Fonts(3));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.MinimumHeight = 20f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk(dr["INSPECTED DATE"].ToString(), Pdf_Fonts(5));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.MinimumHeight = 20f;
            table.AddCell(cell2);

            //3rd Column
            Chunk chk3 = new Chunk(dr["STENCIL NO"].ToString(), Pdf_Fonts(4));
            PdfPCell cell3 = new PdfPCell(new Phrase(chk3));
            cell3.MinimumHeight = 20f;
            table.AddCell(cell3);

            //4th Column
            Chunk chk4 = new Chunk(dr["TYRE SIZE"].ToString(), Pdf_Fonts(4));
            PdfPCell cell4 = new PdfPCell(new Phrase(chk4));
            cell4.MinimumHeight = 20f;
            table.AddCell(cell4);

            //5th Column
            Chunk chk5 = new Chunk(dr["RIM"].ToString(), Pdf_Fonts(4));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.MinimumHeight = 20f;
            table.AddCell(cell5);

            //6th Column
            Chunk chk6 = new Chunk(dr["TYPE"].ToString(), Pdf_Fonts(4));
            PdfPCell cell6 = new PdfPCell(new Phrase(chk6));
            cell6.MinimumHeight = 20f;
            table.AddCell(cell6);

            //7th Column
            string value = dr["BRAND"].ToString() + " / " + dr["SIDEWALL"].ToString() + " / " + dr["PLATFORM"].ToString();
            Chunk chk7 = new Chunk(value, Pdf_Fonts(5));
            PdfPCell cell7 = new PdfPCell(new Phrase(chk7));
            cell7.MinimumHeight = 20f;
            table.AddCell(cell7);

            //8th Column
            Chunk chk8 = new Chunk(dr["BEAD TYPE"].ToString(), Pdf_Fonts(4));
            PdfPCell cell8 = new PdfPCell(new Phrase(chk8));
            cell8.MinimumHeight = 20f;
            table.AddCell(cell8);

            //9th Column
            Chunk chk9 = new Chunk(dr["ID GAUGE"].ToString(), Pdf_Fonts(4));
            PdfPCell cell9 = new PdfPCell(new Phrase(chk9));
            cell9.MinimumHeight = 20f;
            table.AddCell(cell9);

            //10th Column
            Chunk chk10 = new Chunk(dr["TREAD HARDNESS"].ToString(), Pdf_Fonts(4));
            PdfPCell cell10 = new PdfPCell(new Phrase(chk10));
            cell10.MinimumHeight = 20f;
            table.AddCell(cell10);

            //11th Column
            Chunk chk11 = new Chunk(dr["BASE HARDNESS"].ToString(), Pdf_Fonts(4));
            PdfPCell cell11 = new PdfPCell(new Phrase(chk11));
            cell11.MinimumHeight = 20f;
            table.AddCell(cell11);

            //12th Column
            Chunk chk12 = new Chunk(dr["GRADE"].ToString(), Pdf_Fonts(4));
            PdfPCell cell12 = new PdfPCell(new Phrase(chk12));
            cell12.MinimumHeight = 20f;
            table.AddCell(cell12);

            //13th Column
            Chunk chk13 = new Chunk(dr["QUALITY TEST"].ToString(), Pdf_Fonts(4));
            PdfPCell cell13 = new PdfPCell(new Phrase(chk13));
            cell13.MinimumHeight = 20f;
            table.AddCell(cell13);

            //14th Column
            Chunk chk14 = new Chunk(dr["REMARKS"].ToString(), Pdf_Fonts(4));
            PdfPCell cell14 = new PdfPCell(new Phrase(chk14));
            cell14.MinimumHeight = 20f;
            table.AddCell(cell14);

            return table;
        }
        //Build Empty Rows upto 20
        static PdfPTable Build_Table5_Content(int No)
        {
            PdfPTable table = new PdfPTable(14);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1.1f, 2.2f, 2.6f, 3.8f, 1.3f, 2.3f, 6.9f, 1.6f, 1.7f, 1.3f, 1.3f, 1.4f, 3.5f, 4f };
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk(No.ToString(), Pdf_Fonts(3));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.MinimumHeight = 20f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk("", Pdf_Fonts(4));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.MinimumHeight = 20f;
            table.AddCell(cell2);

            //3rd Column
            Chunk chk3 = new Chunk("", Pdf_Fonts(4));
            PdfPCell cell3 = new PdfPCell(new Phrase(chk3));
            cell3.MinimumHeight = 20f;
            table.AddCell(cell3);

            //4th Column
            Chunk chk4 = new Chunk("", Pdf_Fonts(4));
            PdfPCell cell4 = new PdfPCell(new Phrase(chk4));
            cell4.MinimumHeight = 20f;
            table.AddCell(cell4);

            //5th Column
            string value = "";
            Chunk chk5 = new Chunk(value, Pdf_Fonts(4));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.MinimumHeight = 20f;
            table.AddCell(cell5);

            //6th Column
            Chunk chk6 = new Chunk("", Pdf_Fonts(4));
            PdfPCell cell6 = new PdfPCell(new Phrase(chk6));
            cell6.MinimumHeight = 20f;
            table.AddCell(cell6);

            //7th Column
            Chunk chk7 = new Chunk("", Pdf_Fonts(4));
            PdfPCell cell7 = new PdfPCell(new Phrase(chk7));
            cell7.MinimumHeight = 20f;
            table.AddCell(cell7);

            //8th Column
            Chunk chk8 = new Chunk("", Pdf_Fonts(4));
            PdfPCell cell8 = new PdfPCell(new Phrase(chk8));
            cell8.MinimumHeight = 20f;
            table.AddCell(cell8);

            //9th Column
            Chunk chk9 = new Chunk("", Pdf_Fonts(4));
            PdfPCell cell9 = new PdfPCell(new Phrase(chk9));
            cell9.MinimumHeight = 20f;
            table.AddCell(cell9);

            //10th Column
            Chunk chk10 = new Chunk("", Pdf_Fonts(4));
            PdfPCell cell10 = new PdfPCell(new Phrase(chk10));
            cell10.MinimumHeight = 20f;
            table.AddCell(cell10);

            //11th Column
            Chunk chk11 = new Chunk("", Pdf_Fonts(4));
            PdfPCell cell11 = new PdfPCell(new Phrase(chk11));
            cell11.MinimumHeight = 20f;
            table.AddCell(cell11);

            //12th Column
            PdfPCell cell12 = new PdfPCell();
            cell12.MinimumHeight = 20f;
            table.AddCell(cell12);

            //13th Column
            PdfPCell cell13 = new PdfPCell();
            cell13.MinimumHeight = 20f;
            table.AddCell(cell13);

            //14th Column
            PdfPCell cell14 = new PdfPCell();
            cell14.MinimumHeight = 20f;
            table.AddCell(cell14);

            return table;
        }
        //Build Footer Text
        static PdfPTable Build_Table6_Footer()
        {
            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 3f, 4f, 18f, 3f, 4f };
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk("DOC No:", Pdf_Fonts(3));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.MinimumHeight = 20f;
            cell1.Border = 0;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk("QA / R / 8.2.4/01", Pdf_Fonts(3));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.MinimumHeight = 20f;
            cell2.Border = 0;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell2);

            //3rd Column
            PdfPCell cell3 = new PdfPCell();
            cell3.MinimumHeight = 20f;
            cell3.Border = 0;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell3);

            //4th Column
            Chunk chk4 = new Chunk("Rev No./Date:", Pdf_Fonts(3));
            PdfPCell cell4 = new PdfPCell(new Phrase(chk4));
            cell4.MinimumHeight = 20f;
            cell4.Border = 0;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell4);

            //5th Column
            Chunk chk5 = new Chunk("01/11.02.2010", Pdf_Fonts(3));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.MinimumHeight = 20f;
            cell5.Border = 0;
            //cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell5);

            return table;
        }
        //Fonts to Use
        static Font Pdf_Fonts(int font_id)
        {
            Font SelectedFont = null; ;
            switch (font_id)
            {
                case (1):
                    SelectedFont = FontFactory.GetFont("Arial", 20, Font.BOLD); //Main_TitleFont
                    break;
                case (2):
                    SelectedFont = FontFactory.GetFont("Arial", 10, Font.BOLD); //Sub_TitleFont
                    break;
                case (3):
                    SelectedFont = FontFactory.GetFont("Arial", 9, Font.BOLD); //Main_HeadFont
                    break;
                case (4):
                    SelectedFont = FontFactory.GetFont("Arial", 9, Font.NORMAL); //Main_HeadFont_value
                    break;
                case (5):
                    SelectedFont = FontFactory.GetFont("Arial", 8, Font.NORMAL); //ContentFont
                    break;
                case (6):
                    SelectedFont = FontFactory.GetFont("Arial", 7, Font.BOLD); //ContentFont
                    break;
            }
            return SelectedFont;
        }
    }
}