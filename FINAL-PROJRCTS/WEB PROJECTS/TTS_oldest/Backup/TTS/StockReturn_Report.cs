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
    public class StockReturn_Report
    {
        // Static Variables
        public static string CustomerName { get; set; }
        public static string PONO { get; set; }
        public static string Preparedby { get; set; }
        public static string OrderQuantity { get; set; }
        public static string InspectedBy { get; set; }
        public static string CustomerReferenceNo { get; set; }
        public static string InspectedOn { get; set; }
        public static string InvoiceNo { get; set; }
        public static string PreparedOn { get; set; }
        public static string Createdon { get; set; }
        public static string DateOfReceipt { get; set; }
        public static DataTable dtItem { get; set; }
        public static string RefPo { get; set; }
        public static void PDI_Generation(string FileSavePath)
        {
            try
            {
                Document document = new Document(PageSize.A4.Rotate(), 5f, 5f, 20f, 15f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(FileSavePath, FileMode.Create));
                writer.PageEvent = new PDFWriterEvents("GOODS INSPECTION", 50f, 450f, 240f, 45f);
                document.Open();
                document.Add(Build_Table1_MainHeader("GOODS INSPECTION"));
                document.Add(Build_Table2_SubHeader());
                document.Add(Build_Table4_SubHeader());
                int i = 1;
                foreach (DataRow dr in dtItem.Rows)
                {
                    if (i <= dtItem.Rows.Count)
                    {
                        document.Add(Build_Table5_Content(i, dr));
                        i++;
                    }
                }
                for (int increment = dtItem.Rows.Count + 1; increment <= 20; increment++)
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
            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 3.7f, 25.4f, 5.9f };
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
            Chunk nestedtable1_chk1 = new Chunk("InvoiceNO", Pdf_Fonts(3));
            PdfPCell nestedtable1_cell1 = new PdfPCell(new Phrase(nestedtable1_chk1));
            nestedtable1_cell1.MinimumHeight = 10;
            nestedtable1_cell1.Padding = 5;
            nestedtable1_cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell1);

            Chunk chk3 = new Chunk(InvoiceNo, Pdf_Fonts(4));
            PdfPCell nestedtable1_cell2 = new PdfPCell(new Phrase(chk3));
            nestedtable1_cell2.MinimumHeight = 30;
            nestedtable1_cell2.Padding = 5;
            nestedtable1_cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell2);
            PdfPCell cell3 = new PdfPCell(nestedtable1);
            table.AddCell(cell3);
            return table;

        }
        //Build Customer Details
        static PdfPTable Build_Table2_SubHeader()
        {
            PdfPTable table = new PdfPTable(6);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 3.7f, 14.5f, 2f, 6f, 2.4f, 5.9F };
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk("Customer ", Pdf_Fonts(4));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.FixedHeight = 20f;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk(CustomerName, Pdf_Fonts(3));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.FixedHeight = 20f;
            table.AddCell(cell2);

            //3rd Column
            Chunk chk5 = new Chunk("Ref PO No.", Pdf_Fonts(4));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.FixedHeight = 20f;
            table.AddCell(cell5);

            //4th Column
            Chunk chk6 = new Chunk(PONO, Pdf_Fonts(3));
            PdfPCell cell6 = new PdfPCell(new Phrase(chk6));
            cell6.FixedHeight = 20f;
            table.AddCell(cell6);

            //5th Column
            Chunk chk7 = new Chunk("Date of rec", Pdf_Fonts(4));
            PdfPCell cell7 = new PdfPCell(new Phrase(chk7));
            cell7.FixedHeight = 20f;
            table.AddCell(cell7);

            //6th Column      
            Chunk chk8 = new Chunk(DateOfReceipt, Pdf_Fonts(3));
            PdfPCell cell8 = new PdfPCell(new Phrase(chk8));
            cell8.FixedHeight = 20f;
            table.AddCell(cell8);

            return table;
        }
        //Build Header for Jathagam
        static PdfPTable Build_Table4_SubHeader()
        {
            PdfPTable table = new PdfPTable(8);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            //                             SNO   DATE  STEN  TYPE  BRAN  QUAN  REA INS 
            float[] widths = new float[] { 1.1f, 2.6f, 2.6f, 2.0f, 2.6f, 2.3f, 5.5f, 16.3f };
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk("S.No", Pdf_Fonts(4));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.FixedHeight = 40f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk("Tyre size", Pdf_Fonts(4));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.FixedHeight = 40f;
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell2);

            //3rd Column
            Chunk chk3 = new Chunk("EDC No.", Pdf_Fonts(4));
            PdfPCell cell3 = new PdfPCell(new Phrase(chk3));
            cell3.FixedHeight = 40f;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell3);

            //4th Column
            Chunk chk4 = new Chunk("Type", Pdf_Fonts(4));
            PdfPCell cell4 = new PdfPCell(new Phrase(chk4));
            cell4.FixedHeight = 40f;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell4);

            //5th Column
            Chunk chk5 = new Chunk("Brand", Pdf_Fonts(4));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.FixedHeight = 40f;
            cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell5);

            //6th Column
            Chunk chk6 = new Chunk("Quantity", Pdf_Fonts(4));
            PdfPCell cell6 = new PdfPCell(new Phrase(chk6));
            cell6.FixedHeight = 40f;
            cell6.HorizontalAlignment = Element.ALIGN_CENTER;
            cell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell6);

            //7th Column
            Chunk chk7 = new Chunk("Remarks", Pdf_Fonts(4));
            PdfPCell cell7 = new PdfPCell(new Phrase(chk7));
            cell7.FixedHeight = 40f;
            cell7.HorizontalAlignment = Element.ALIGN_CENTER;
            cell7.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell7);

            //8th Column
            PdfPTable nestedtable1 = new PdfPTable(3);
            float[] width = new float[] { 8f, 2.4f, 5.9f };
            nestedtable1.SetWidths(width);
            Chunk nestedtable1_chk1 = new Chunk("Quality Inspection", Pdf_Fonts(3));
            PdfPCell nestedtable1_cell1 = new PdfPCell(new Phrase(nestedtable1_chk1));
            nestedtable1_cell1.Colspan = 3;
            nestedtable1_cell1.MinimumHeight = 10f;
            nestedtable1_cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell1);

            Chunk nestedtable1_chk2 = new Chunk("Condition of the tyre", Pdf_Fonts(4));
            PdfPCell nestedtable1_cell2 = new PdfPCell(new Phrase(nestedtable1_chk2));
            nestedtable1_cell2.MinimumHeight = 30f;
            nestedtable1_cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell2);

            Chunk nestedtable1_chk3 = new Chunk("Suggested Grade", Pdf_Fonts(4));
            PdfPCell nestedtable1_cell3 = new PdfPCell(new Phrase(nestedtable1_chk3));
            nestedtable1_cell3.MinimumHeight = 30f;
            nestedtable1_cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell3);


            Chunk nestedtable1_chk4 = new Chunk("Reason for Return", Pdf_Fonts(4));
            PdfPCell nestedtable1_cell4 = new PdfPCell(new Phrase(nestedtable1_chk4));
            nestedtable1_cell4.MinimumHeight = 30f;
            nestedtable1_cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            nestedtable1_cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            nestedtable1.AddCell(nestedtable1_cell4);

            PdfPCell cell8 = new PdfPCell(nestedtable1);
            table.AddCell(cell8);
            return table;
        }
        //Get Values  Jathagam
        static PdfPTable Build_Table5_Content(int No, DataRow dr)
        {
            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1.1f, 2.6f, 2.6f, 2.0f, 2.6f, 2.3f, 5.5f, 8f, 2.4f, 5.9f };
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk(No.ToString(), Pdf_Fonts(4));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.MinimumHeight = 20f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk(dr["Tyresize"].ToString(), Pdf_Fonts(3));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.MinimumHeight = 20f;
            table.AddCell(cell2);

            //3rd Column
            Chunk chk3 = new Chunk(dr["StencilNo"].ToString(), Pdf_Fonts(3));
            PdfPCell cell3 = new PdfPCell(new Phrase(chk3));
            cell3.MinimumHeight = 20f;
            table.AddCell(cell3);

            //4th Column
            Chunk chk4 = new Chunk(dr["tyreType"].ToString(), Pdf_Fonts(3));
            PdfPCell cell4 = new PdfPCell(new Phrase(chk4));
            cell4.MinimumHeight = 20f;
            table.AddCell(cell4);

            //5th Column
            Chunk chk5 = new Chunk(dr["Brand"].ToString(), Pdf_Fonts(3));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.MinimumHeight = 20f;
            table.AddCell(cell5);

            //6th Column
            Chunk chk6 = new Chunk(dr["ReturnQty"].ToString(), Pdf_Fonts(3));
            PdfPCell cell6 = new PdfPCell(new Phrase(chk6));
            cell6.MinimumHeight = 20f;
            table.AddCell(cell6);

            //7th Column       
            Chunk chk7 = new Chunk(dr["Remarks"].ToString(), Pdf_Fonts(3));
            PdfPCell cell7 = new PdfPCell(new Phrase(chk7));
            cell7.MinimumHeight = 20f;
            table.AddCell(cell7);

            //8th Column
            Chunk chk8 = new Chunk(dr["QCConditionOfthetyre"].ToString(), Pdf_Fonts(3));
            PdfPCell cell8 = new PdfPCell(new Phrase(chk8));
            cell8.MinimumHeight = 20f;
            table.AddCell(cell8);

            //9th Column
            Chunk chk9 = new Chunk(dr["QCGrade"].ToString(), Pdf_Fonts(3));
            PdfPCell cell9 = new PdfPCell(new Phrase(chk9));
            cell9.MinimumHeight = 20f;
            table.AddCell(cell9);

            //10th Column
            Chunk chk10 = new Chunk(dr["QCReason"].ToString(), Pdf_Fonts(3));
            PdfPCell cell10 = new PdfPCell(new Phrase(chk10));
            cell10.MinimumHeight = 20f;
            table.AddCell(cell10);
            return table;
        }
        //Build Empty Rows upto 20
        static PdfPTable Build_Table5_Content(int No)
        {
            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1.1f, 2.6f, 2.6f, 2.0f, 2.6f, 2.3f, 5.5f, 8f, 2.4f, 5.9f };
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk(No.ToString(), Pdf_Fonts(4));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.MinimumHeight = 20f;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk("", Pdf_Fonts(3));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.MinimumHeight = 20f;
            table.AddCell(cell2);

            //3rd Column
            Chunk chk3 = new Chunk("", Pdf_Fonts(3));
            PdfPCell cell3 = new PdfPCell(new Phrase(chk3));
            cell3.MinimumHeight = 20f;
            table.AddCell(cell3);

            //4th Column
            Chunk chk4 = new Chunk("", Pdf_Fonts(3));
            PdfPCell cell4 = new PdfPCell(new Phrase(chk4));
            cell4.MinimumHeight = 20f;
            table.AddCell(cell4);

            //5th Column
            string value = "";
            Chunk chk5 = new Chunk(value, Pdf_Fonts(3));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.MinimumHeight = 20f;
            table.AddCell(cell5);

            //6th Column
            Chunk chk6 = new Chunk("", Pdf_Fonts(3));
            PdfPCell cell6 = new PdfPCell(new Phrase(chk6));
            cell6.MinimumHeight = 20f;
            table.AddCell(cell6);

            //7th Column
            Chunk chk7 = new Chunk("", Pdf_Fonts(3));
            PdfPCell cell7 = new PdfPCell(new Phrase(chk7));
            cell7.MinimumHeight = 20f;
            table.AddCell(cell7);

            //8th Column
            Chunk chk8 = new Chunk("", Pdf_Fonts(3));
            PdfPCell cell8 = new PdfPCell(new Phrase(chk8));
            cell8.MinimumHeight = 20f;
            table.AddCell(cell8);

            //9th Column
            Chunk chk9 = new Chunk("", Pdf_Fonts(3));
            PdfPCell cell9 = new PdfPCell(new Phrase(chk9));
            cell9.MinimumHeight = 20f;
            table.AddCell(cell9);

            //10th Column
            Chunk chk10 = new Chunk("", Pdf_Fonts(3));
            PdfPCell cell10 = new PdfPCell(new Phrase(chk10));
            cell10.MinimumHeight = 20f;
            table.AddCell(cell10);

            return table;
        }
        //Build Footer Text
        static PdfPTable Build_Table6_Footer()
        {
            PdfPTable table = new PdfPTable(10);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 3f, 2.6f, 2.6f, 2.0f, 2.8f, 2.5f, 3f, 3f, 3f, 3f };
            table.SetWidths(widths);

            //1st Column
            Chunk chk1 = new Chunk("Add to FG stock", FontFactory.GetFont("Arial", 9, Font.NORMAL));
            PdfPCell cell1 = new PdfPCell(new Phrase(chk1));
            cell1.MinimumHeight = 20f;
            cell1.Border = 0;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell1);

            //2nd Column
            Chunk chk2 = new Chunk("Prepared by:", FontFactory.GetFont("Arial", 9, Font.NORMAL));
            PdfPCell cell2 = new PdfPCell(new Phrase(chk2));
            cell2.MinimumHeight = 20f;
            cell2.Border = 0;
            //cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell2);

            //3rd Column
            Chunk chk3 = new Chunk(Preparedby, FontFactory.GetFont("Arial", 10, Font.BOLD));
            PdfPCell cell3 = new PdfPCell(new Phrase(chk3));
            cell3.MinimumHeight = 20f;
            cell3.Border = 0;
            cell3.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell3);

            //4th Column
            Chunk chk4 = new Chunk("Date:", Pdf_Fonts(3));
            PdfPCell cell4 = new PdfPCell(new Phrase(chk4));
            cell4.MinimumHeight = 20f;
            cell4.Border = 0;
            cell4.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell4);

            //5th Column
            Chunk chk5 = new Chunk(PreparedOn, Pdf_Fonts(3));
            PdfPCell cell5 = new PdfPCell(new Phrase(chk5));
            cell5.MinimumHeight = 20f;
            cell5.Border = 0;
            //cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell5);

            //6th Column
            Chunk chk6 = new Chunk("InspectedBy", Pdf_Fonts(4));
            PdfPCell cell6 = new PdfPCell(new Phrase(chk6));
            cell6.MinimumHeight = 20f;
            cell6.Border = 0;
            //cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell6.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell6);

            //7th Column
            Chunk chk7 = new Chunk(InspectedBy, Pdf_Fonts(3));
            PdfPCell cell7 = new PdfPCell(new Phrase(chk7));
            cell7.MinimumHeight = 20f;
            cell7.Border = 0;
            //cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell7.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell7);


            //8th Column
            Chunk chk8 = new Chunk("InspectedOn", Pdf_Fonts(4));
            PdfPCell cell8 = new PdfPCell(new Phrase(chk8));
            cell8.MinimumHeight = 20f;
            cell8.Border = 0;
            //cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell8.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell8);

            //9th Column
            Chunk chk9 = new Chunk(InspectedOn, Pdf_Fonts(3));
            PdfPCell cell9 = new PdfPCell(new Phrase(chk9));
            cell9.MinimumHeight = 20f;
            cell9.Border = 0;
            //cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell8.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell9);

            //10th Column
            Chunk chk10 = new Chunk("Signature", Pdf_Fonts(3));
            PdfPCell cell10 = new PdfPCell(new Phrase(chk10));
            cell10.MinimumHeight = 20f;
            cell10.Border = 0;
            //cell5.HorizontalAlignment = Element.ALIGN_CENTER;
            cell10.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(cell10);
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