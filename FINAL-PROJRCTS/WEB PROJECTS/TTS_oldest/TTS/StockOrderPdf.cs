using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Text;
using System.Reflection;

namespace TTS
{
    public class StockOrderPdf
    {
        static DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        //Static Variables
        //public static string CustCode { get; set; }
        public static string OrderRefNo { get; set; }
        public static DataTable StockItemlist { get; set; }
        public static string WorkOrderNo { get; set; }
        public static string WorkOrderReviseNo { get; set; }
        public static string CustomerName { get; set; }
        public static string CustStdCode { get; set; }
        public static string Startdate { get; set; }
        public static string Enddate { get; set; }
        public static string plant { get; set; }
        public static int SID { get; set; }
        public static string WorkOrderCreation()
        {
            try
            {
                //Allocate path to save file
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/Stockorderfiles/" + CustStdCode + "/"))
                    Directory.CreateDirectory(serverURL + "/Stockorderfiles/" + CustStdCode + "/");
                string path = serverURL + "/Stockorderfiles/" + CustStdCode + "/" + SID + ".pdf";
                //Create PDF Document
                Document document = new Document(PageSize.A4, 25f, 25f, 50f, 25f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
                writer.PageEvent = new PDFWriterEvents("STOCK ORDER - " + plant, 40f);
                document.Open();
                document.Add(PreparePdf());
                document.Close();
                return "Success";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Pdf_WorkOrderDetails.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        public static PdfPTable PreparePdf()
        {
            PdfPTable table = new PdfPTable(10);
            try
            {
                //Allocate Table Widths
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                //                              No   pla Si  Rim   ty    br  si  qt    wt    tot wt
                float[] widths = new float[] { 0.7f, 2f, 4f, 1.2f, 1.8f, 2f, 2f, 1.2f, 1.4f, 1.6f };
                table.SetWidths(widths);
                table.SpacingBefore = 25f;
                table.SpacingAfter = 15f;
                StockOrderHead(table, "STOCK ORDER", 1, 10);
                //Preparenested(table);

                Build_ChildTable(table);
                addWorkDescDetails(table, "Stock Order For The Period: " + Startdate + " to " + Enddate, 0, 10);
                addWorkDescDetails(table, "To: \n Production Executive: " + plant + "\n", 0, 10);
                //Build_Ch(table);
                addWorkDescDetails(table, "Please arrange to process / despatch Solid Cushion Tyres as per details given below:\n \n", 0, 10);
                //Fill WorkOrder table Heading Cell
                addWorkOrderHeadCell(table, "NO");
                addWorkOrderHeadCell(table, "PLATFORM");
                addWorkOrderHeadCell(table, "TYRE SIZE");
                addWorkOrderHeadCell(table, "RIM");
                addWorkOrderHeadCell(table, "TYPE");
                addWorkOrderHeadCell(table, "BRAND");
                addWorkOrderHeadCell(table, "SIDEWALL");
                addWorkOrderHeadCell(table, "QTY");
                addWorkOrderHeadCell(table, "WT");
                addWorkOrderHeadCell(table, "TOT WT");

                Build_WorkOrderItems(table);
                Build_DeliveryDetails(table);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Pdf_WorkOrderDetails.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return table;
        }
        public static void Build_DeliveryDetails(PdfPTable table)
        {
            try
            {
                addWorkOrderBottomCell(table, "NOTE: \n 1.THIS WORK ORDER SUPERCEDES ALL PREVIOUS PENDING WORK ORDERS.PRODUCTION AGAINST PREVIOUS PENDING WORK ORDERS SHOULD BE SUSPENDED.", 0, 12);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Pdf_WorkOrderDetails.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public static PdfPTable title()
        {
            PdfPTable table = new PdfPTable(3);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.TotalWidth = 550f;
            table.LockedWidth = true;
            float[] widths = new float[] { 11f, 6f, 8f };
            table.SetWidths(widths);
            table.SpacingBefore = 25f;
            table.SpacingAfter = 15f;
            var titleFont = FontFactory.GetFont("Arial", 15, Font.BOLD);

            PdfPCell cell1 = new PdfPCell(new Phrase(""));
            cell1.Border = 0;
            table.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("STOCK ORDER", titleFont));
            table.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase(""));
            cell3.Border = 0;
            table.AddCell(cell3);
            return table;
        }
        public static void StockOrderHead(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            PdfPCell cellTot = new PdfPCell();
            cellTot = new PdfPCell(new Phrase(text, titleFont));
            cellTot.FixedHeight = 22f;
            cellTot.Colspan = cols;
            cellTot.HorizontalAlignment = alignCode;
            cellTot.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cellTot);
        }
        public static void AddStockMainDetails(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 10);
            PdfPCell cellTot = new PdfPCell();
            cellTot = new PdfPCell(new Phrase(text, titleFont));
            cellTot.Colspan = cols;
            cellTot.HorizontalAlignment = alignCode;
            cellTot.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cellTot);
        }
        public static void Build_ChildTable(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                PdfPCell reviseTable = new PdfPCell(new Phrase("Work Order No: " + WorkOrderNo + "\n \nWork Order Date: " + DateTime.Now.ToString("dd/MM/yyyy") + "\n \n", titleFont));
                reviseTable.Colspan = 7;
                table.AddCell(reviseTable);

                PdfPTable nested = new PdfPTable(2);
                nested.AddCell(new Phrase("Rev No:", titleFont));
                nested.AddCell(new Phrase(WorkOrderReviseNo, titleFont));

                nested.AddCell(new Phrase("Rev Date:", titleFont));
                nested.AddCell(new Phrase(DateTime.Now.ToShortDateString(), titleFont));

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Padding = 0f;
                nesthousing.Colspan = 4;
                table.AddCell(nesthousing);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Pdf_WorkOrderDetails.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public static void addWorkOrderHeadCell(PdfPTable table, string text)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
            cell.FixedHeight = 24f;
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cell);
        }
        public static void addWorkOrderBottomCell(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
            cell.Colspan = cols;
            cell.HorizontalAlignment = alignCode;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

            table.AddCell(cell);
        }
        public static void addWorkDescDetails(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cellTot = new PdfPCell();
            cellTot = new PdfPCell(new Phrase(text, titleFont));
            cellTot.Colspan = cols;
            cellTot.HorizontalAlignment = alignCode;
            cellTot.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cellTot);
        }
        public static void Build_WorkOrderItems(PdfPTable table)
        {
            try
            {
                int j = 1;
                foreach (DataRow dRow in StockItemlist.Rows)
                {
                    addWorkOrderItemCell(table, Convert.ToString(j++), 2);
                    addWorkOrderItemCell(table, dRow["Config"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["Tyresize"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["RimSize"].ToString(), 0);

                    addWorkOrderItemCell(table, dRow["tyretype"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["brand"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["sidewall"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["itemqty"].ToString(), 2);
                    addWorkOrderItemCell(table, dRow["finishedwt"].ToString(), 2);
                    addWorkOrderItemCell(table, (Convert.ToDecimal(dRow["finishedwt"].ToString()) * Convert.ToDecimal(dRow["itemqty"].ToString())).ToString(), 2);
                }
                for (int k = StockItemlist.Rows.Count; k < 11; k++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase("\n"));
                    cell.FixedHeight = 20f;
                    table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell);
                    table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell);

                }
                addWorkOrderTotalCell(table, "TOTAL", 2, 7);
                addWorkOrderTotalCell(table, StockItemlist.Compute("Sum(itemqty)", "").ToString(), 2, 1);
                addWorkOrderTotalCell(table, StockItemlist.Compute("Sum(totfwt)", "").ToString(), 2, 2);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Pdf_WorkOrderDetails.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public static void addWorkOrderItemCell(PdfPTable table, string text, int alignCode)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
            cell.HorizontalAlignment = alignCode;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            if (!text.Contains("REF:"))
                cell.FixedHeight = 20f;
            table.AddCell(cell);
        }
        public static void addWorkOrderTotalCell(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
            cell.Colspan = cols;
            cell.HorizontalAlignment = alignCode;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cell);
        }
    }
}