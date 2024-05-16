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
    public class Pdf_WorkOrderDetails
    {
        static DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        //Static Variables
        public static string CustCode { get; set; }
        public static string OrderRefNo { get; set; }
        public static DataTable dtItemList { get; set; }
        public static string WorkOrderNo { get; set; }
        public static string WorkOrderReviseNo { get; set; }
        public static string CustomerName { get; set; }
        public static string Priority { get; set; }
        public static string DeliveryDate { get; set; }
        public static string Remarks { get; set; }
        public static bool Category1 { get; set; }
        public static bool Category2 { get; set; }
        public static bool Category3 { get; set; }
        public static bool Category4 { get; set; }
        public static bool Category5 { get; set; }
        public static bool Category6 { get; set; }
        public static int OID { get; set; }

        public static string WorkOrderCreation()
        {
            try
            {
                //Allocate path to save file
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/workorderfiles/" + CustCode + "/"))
                    Directory.CreateDirectory(serverURL + "/workorderfiles/" + CustCode + "/");
                string path = serverURL + "/workorderfiles/" + CustCode + "/" + OrderRefNo + ".pdf";

                //Create PDF Document
                Document document = new Document(PageSize.A4, 25f, 25f, 50f, 25f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
                writer.PageEvent = new PDFWriterEvents("WORK ORDER");
                document.Open();
                document.Add(PreparePdf());
                build_Category_Boxes(writer);
                document.Close();
                string Output = SaveWorkOrderDetails();
                if (Output.Length > 0)
                    return "Success";
                else
                    return "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Pdf_WorkOrderDetails.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        public static PdfPTable PreparePdf()
        {
            PdfPTable table = new PdfPTable(12);
            try
            {
                //Allocate Table Widths
                table.TotalWidth = 550f;
                table.LockedWidth = true;
                //                              No   Cate  Size  Rim   Con    ty   br     si    qt   sto    wt   tot wt
                float[] widths = new float[] { 0.7f, 1.4f, 3.8f, 0.8f, 2.2f, 1.6f, 2.5f, 2.2f, 0.9f, 0.9f, 1.2f, 1.5f };
                table.SetWidths(widths);
                table.SpacingBefore = 25f;
                table.SpacingAfter = 15f;

                //Fill WorkOrder Heading Table
                addWorkOrderHead(table, "WORK ORDER / DESPATCH ADVICE", 1, 12);
                //Fill WorkOrderNo and Date Details
                addWorkMainDetails(table, "WORK ORDER NO: " + WorkOrderNo, 0, 8);
                addWorkMainDetails(table, "DATE", 0, 2);
                addWorkMainDetails(table, DateTime.Now.ToString("dd/MM/yyyy"), 0, 2);
                //Fill OrderRefNo and Customer Details
                Build_ChildTable(table);
                //Fill Common Messages
                addWorkDescDetails(table, "Please arrange to process / despatch Solid Cushion Tyres as per details given below:\n \n", 0, 12);
                //Fill OrderRefNo and Customer Details
                addWorkMainDetails(table, "Order Reference : " + OrderRefNo + "  /  " + CustomerName + "\n \nDate:\n \n", 0, 12);
                //Fill WorkOrder table Heading Cell
                addWorkOrderHeadCell(table, "No");
                addWorkOrderHeadCell(table, "CATEGORY");
                addWorkOrderHeadCell(table, "SIZE");
                addWorkOrderHeadCell(table, "RIM");
                addWorkOrderHeadCell(table, "PLATFORM");
                addWorkOrderHeadCell(table, "TYPE");
                addWorkOrderHeadCell(table, "BRAND");
                addWorkOrderHeadCell(table, "SIDEWALL");
                addWorkOrderHeadCell(table, "QTY");
                addWorkOrderHeadCell(table, "STOCK");
                addWorkOrderHeadCell(table, "WEIGHT");
                addWorkOrderHeadCell(table, "TOTAL WT");

                //Fill WorkOrder table items Cell
                Build_WorkOrderItems(table);
                //Fill Delivery Details
                Build_DeliveryDetails(table);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Pdf_WorkOrderDetails.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return table;
        }
        public static void addWorkOrderHead(PdfPTable table, string text, int alignCode, int cols)
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
        public static void addWorkMainDetails(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            PdfPCell cellTot = new PdfPCell();
            cellTot = new PdfPCell(new Phrase(text, titleFont));
            //cellTot.FixedHeight = 18f;
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
                PdfPCell reviseTable = new PdfPCell(new Phrase("TO: Production Executive / Despatch Section", titleFont));
                reviseTable.Colspan = 8;
                reviseTable.FixedHeight = 40f;
                table.AddCell(reviseTable);
                PdfPTable nested = new PdfPTable(2);
                reviseTable = new PdfPCell(new Phrase("Rev No:", titleFont));
                nested.AddCell(reviseTable);
                nested.AddCell(new Phrase(WorkOrderReviseNo, titleFont));

                reviseTable = new PdfPCell(new Phrase("Rev Date:", titleFont));
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                nested.AddCell(reviseTable);
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
        public static void addWorkDescDetails(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cellTot = new PdfPCell();
            cellTot = new PdfPCell(new Phrase(text, titleFont));
            //cellTot.FixedHeight = 18f;
            cellTot.Colspan = cols;
            cellTot.HorizontalAlignment = alignCode;
            cellTot.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cellTot);
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
        public static void Build_WorkOrderItems(PdfPTable table)
        {
            try
            {
                int j = 1;
                foreach (DataRow dRow in dtItemList.Rows)
                {
                    addWorkOrderItemCell(table, Convert.ToString(j++), 0);
                    addWorkOrderItemCell(table, dRow["AssyRimstatus"].ToString() == "True" ? "ASSY" : dRow["category"].ToString() == "SPLIT RIMS" ? "RIMS" : dRow["category"].ToString(), 0);
                    string strassy = dRow["AssyRimstatus"].ToString() == "True" ? "ASSY" : dRow["category"].ToString() == "SPLIT RIMS" || dRow["category"].ToString() == "POB WHEEL" ? "RIMS" : "";
                    string strDwg = strassy != "" ? ("\n" + dRow["EdcNO"].ToString() + " (" + dRow["EdcRimSize"].ToString() + ")\nREF: " + dRow["RimDwg"].ToString()) : "";
                    addWorkOrderItemCell(table, dRow["Tyresize"].ToString() + strDwg, 0);
                    addWorkOrderItemCell(table, dRow["RimSize"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["Config"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["tyretype"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["brand"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["sidewall"].ToString(), 0);
                    addWorkOrderItemCell(table, dRow["itemqty"].ToString(), 2);
                    addWorkOrderItemCell(table, dRow["itemqty"].ToString(), 2);
                    addWorkOrderItemCell(table, (Convert.ToDecimal(dRow["finishedwt"].ToString()) / Convert.ToDecimal(dRow["itemqty"].ToString())).ToString(), 2);
                    addWorkOrderItemCell(table, dRow["finishedwt"].ToString(), 2);
                }
                for (int k = dtItemList.Rows.Count; k < 13; k++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase("\n"));
                    cell.FixedHeight = 20f;
                    table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell);
                    table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell);
                    table.AddCell(cell); table.AddCell(cell);
                }
                addWorkOrderTotalCell(table, "TOTAL", 2, 8);
                addWorkOrderTotalCell(table, dtItemList.Compute("Sum(itemqty)", "").ToString(), 1, 1);
                addWorkOrderTotalCell(table, "\n", 2, 1);
                addWorkOrderTotalCell(table, dtItemList.Compute("Sum(finishedwt)", "").ToString(), 2, 2);
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
            cell.FixedHeight = 22f;
            table.AddCell(cell);
        }
        public static void Build_DeliveryDetails(PdfPTable table)
        {
            try
            {
                addWorkOrderTotalCell(table, "DELIVERY: ", 0, 3);
                addWorkOrderTotalCell(table, Priority.ToUpper(), 0, 4);
                addWorkOrderTotalCell(table, "Expected Shipping Date: " + DeliveryDate, 2, 5);

                var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                PdfPTable nested = new PdfPTable(2);
                float[] widths = new float[] { 9f, 9.8f };
                nested.SetWidths(widths);

                PdfPCell reviseTable = new PdfPCell(new Phrase("1) FUMIGATION   ", titleFont));
                reviseTable.FixedHeight = 30f;
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);
                reviseTable = new PdfPCell(new Phrase("4) PALLETISATION / CRATE / BOX  ", titleFont));
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                reviseTable = new PdfPCell(new Phrase("2) SPIRAL WRAP   ", titleFont));
                reviseTable.FixedHeight = 30f;
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);
                reviseTable = new PdfPCell(new Phrase("5) MADE IN INDIA MARKING  ", titleFont));
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                reviseTable = new PdfPCell(new Phrase("3) SHRINK WRAP   ", titleFont));
                reviseTable.FixedHeight = 30f;
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);
                reviseTable = new PdfPCell(new Phrase("6) 20-FOOT CONTAINER  ", titleFont));
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Padding = 0f;
                nesthousing.Colspan = 12;
                table.AddCell(nesthousing);

                addWorkOrderBottomCell(table, "SPECIAL REMARKS: \n" + Remarks, 0, 12);

                addWorkOrderBottomCell(table, "Prepared by \n \n \n \n \n \n", 0, 4);
                addWorkOrderBottomCell(table, "Checked by \n \n \n \n \n \n", 1, 3);
                addWorkOrderBottomCell(table, "Approved by \n \n \n \n \n \n", 2, 5);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Pdf_WorkOrderDetails.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public static void addWorkOrderBottomCell(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
            cell.Colspan = cols;
            cell.HorizontalAlignment = alignCode;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            //cell.FixedHeight = 22f;
            table.AddCell(cell);
        }
        public static void build_Category_Boxes(PdfWriter wr)
        {
            PdfContentByte cb = wr.DirectContent;
            cb.MoveTo(250, 315);
            cb.LineTo(200, 315);
            cb.LineTo(200, 300);
            cb.LineTo(250, 300);
            if (Category1 == true)
                cb.Fill();
            else
                cb.ClosePathStroke();

            cb.MoveTo(250, 285);
            cb.LineTo(200, 285);
            cb.LineTo(200, 270);
            cb.LineTo(250, 270);
            if (Category2 == true)
                cb.Fill();
            else
                cb.ClosePathStroke();

            cb.MoveTo(250, 255);
            cb.LineTo(200, 255);
            cb.LineTo(200, 240);
            cb.LineTo(250, 240);
            if (Category3 == true)
                cb.Fill();
            else
                cb.ClosePathStroke();

            cb.MoveTo(550, 315);
            cb.LineTo(500, 315);
            cb.LineTo(500, 300);
            cb.LineTo(550, 300);
            if (Category4 == true)
                cb.Fill();
            else
                cb.ClosePathStroke();

            cb.MoveTo(550, 285);
            cb.LineTo(500, 285);
            cb.LineTo(500, 270);
            cb.LineTo(550, 270);
            if (Category5 == true)
                cb.Fill();
            else
                cb.ClosePathStroke();

            cb.MoveTo(550, 255);
            cb.LineTo(500, 255);
            cb.LineTo(500, 240);
            cb.LineTo(550, 240);
            if (Category6 == true)
                cb.Fill();
            else
                cb.ClosePathStroke();
        }
        public static string SaveWorkOrderDetails()
        {
            SqlParameter[] sp1 = new SqlParameter[4];
            sp1[0] = new SqlParameter("@OID", OID);
            sp1[1] = new SqlParameter("@AttachFileName", OrderRefNo + ".pdf");
            sp1[2] = new SqlParameter("@UserName", HttpContext.Current.Request.Cookies["TTSUser"].Value);
            sp1[3] = new SqlParameter("@FileType", "WORKORDER FILE");
            int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);
            if (resp > 0)
                return "Success";
            else
                return "";
        }
    }
}