using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Web.UI;
using System.IO;

namespace TTS
{
    public class expscanpdiinspectionreport
    {
        DataAccess daCOTS = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        public string str_CustCode = "";
        public string str_WorkOrderNo = "";
        public string str_CustName = "";
        public string str_OrderNo = "";
        public string str_OrderDate = "";
        public string str_InvoiceNo = "";
        public string str_InvoiceDate = "";
        public string str_PdiPlant = "";
        public string str_PID = "";
        public string str_fID = "";
        Document document = null;
        PdfWriter writer = null;
        private List<string> lst_StencilNo = new List<string>();
        private string[] arrDescriptn = null;
        private string[] arrActColumnName = null;
        private string[] arrSpecVal = null;
        private string[] arrJudgeVal = null;
        public void PdiInspectionRptCreation()
        {
            try
            {
                //Allocate path to save file
                string strFileNo = str_fID.ToLower() == "d" ? str_OrderNo : str_WorkOrderNo;
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/invoicefiles/" + str_CustCode + "/"))
                    Directory.CreateDirectory(serverURL + "/invoicefiles/" + str_CustCode + "/");
                string path = serverURL + "/invoicefiles/" + str_CustCode + "/FinalInspection_" + strFileNo + "_" + str_PdiPlant.ToLower() + ".pdf";

                //get inspect details
                SqlParameter[] spOrder = new SqlParameter[] { new SqlParameter("@PID", str_PID) };
                DataTable dtOrder = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ProdWareHouse_InspectionLevel2", spOrder, DataAccess.Return_Type.DataTable);

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@Custcode", str_CustCode), 
                    new SqlParameter("@WorkOrderNo", str_WorkOrderNo), 
                    new SqlParameter("@Plant", str_PdiPlant) 
                };
                DataSet dt_InspectionDetails = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Rpt_InspectionLevel2", sp, DataAccess.Return_Type.DataSet);
                if (dt_InspectionDetails.Tables[0].Rows.Count > 0)
                {
                    //Create PDF Document
                    document = new Document(PageSize.A4.Rotate(), 20f, 20f, 20f, 10f);
                    writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
                    document.Open();

                    arrDescriptn = new string[] { "Rim Width (mm)", "Outer Diameter (mm)", "Tyre Width (mm)", "Inner Diameter (mm)", 
                            "Hardness (sh-A)", "Apperance", "Tyre Grade" };
                    arrActColumnName = new string[] { "RimWidth_Act", "OuterDiameter_Act", "TyreWidth_Act", "InnerDiameter_Act", 
                            "Hardness_Act", "Apperance_Act", "TyreGrade_Act" };
                    arrSpecVal = new string[arrActColumnName.Length];
                    arrJudgeVal = new string[arrActColumnName.Length];

                    DataView dtDescView = new DataView(dt_InspectionDetails.Tables[0]);
                    dtDescView.Sort = "ProcessID ASC";
                    DataTable distinctProcess = dtDescView.ToTable(true, "ProcessID");
                    foreach (DataRow pRow in distinctProcess.Rows)
                    {
                        lst_StencilNo = new List<string>();
                        string str_SampleQty = "";
                        foreach (DataRow sRow in dt_InspectionDetails.Tables[1].Select("ProcessID='" + pRow["ProcessID"].ToString() + "'"))
                            str_SampleQty = sRow["sampleqty"].ToString();

                        foreach (DataRow dr in dt_InspectionDetails.Tables[0].Select("ProcessID='" + pRow["ProcessID"].ToString() + "'"))
                            lst_StencilNo.Add(dr["StencilNo"].ToString());
                        for (int i = 0; i < arrActColumnName.Length; i++)
                        {
                            arrSpecVal[i] = dt_InspectionDetails.Tables[0].Rows[0][arrActColumnName[i].Replace("Act", "Spec")].ToString();
                            arrJudgeVal[i] = dt_InspectionDetails.Tables[0].Rows[0][arrActColumnName[i].Replace("Act", "Judge")].ToString();
                        }

                        string str_RptDate = "";
                        foreach (DataRow dr in dt_InspectionDetails.Tables[0].Select("ProcessID='" + pRow["ProcessID"].ToString() + "'"))
                        {
                            str_RptDate = dr["Created_Date"].ToString();
                            break;
                        }
                        foreach (DataRow oRow in dtOrder.Select("ProcessID='" + pRow["ProcessID"].ToString() + "'"))
                        {
                            document.Add(Draw_Heading());
                            document.Add(Draw_Customer());
                            document.Add(Draw_Category(str_RptDate));
                            document.Add(Draw_Defaults("ORDER REF NO", str_OrderNo, "ORDER DATE", str_OrderDate));
                            document.Add(Draw_Defaults("TYRE SIZE", oRow["tyresize"].ToString(), "QTY", oRow["Qty"].ToString()));
                            document.Add(Draw_Defaults("PLATFORM", oRow["config"].ToString(), "SAMPLE QTY", str_SampleQty));
                            document.Add(Draw_Defaults("TYPE", oRow["tyretype"].ToString(), "INVOICE NO", str_InvoiceNo));
                            document.Add(Draw_Defaults("BRAND", oRow["brand"].ToString(), "INVOICE DATE", str_InvoiceDate));
                            // to Add S.No,Description and observation
                            document.Add(Draw_Description(str_SampleQty));
                            // to Add Spec and Act values to StencilNo
                            for (int i = 0; i < arrDescriptn.Length; i++)
                                document.Add(Draw_SpecActValues(i + 1, arrDescriptn[i], arrActColumnName[i], arrSpecVal[i], arrJudgeVal[i],
                                    dt_InspectionDetails.Tables[0], str_SampleQty));
                            // to Add Report Status
                            document.Add(Draw_ReportStatus());
                            // to Add Remarks
                            document.Add(Draw_Remarks());
                            // to Add Approval
                            document.Add(Draw_Approval());
                        }
                        document.NewPage();
                    }
                    // save and close document
                    document.Close();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "expscanpdiinspectionreport.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private PdfPTable Draw_Heading()
        {
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 4f, 20f };
            table.SetWidths(widths);

            //First Cell
            string imgPath = HttpContext.Current.Server.MapPath("~/images/tvs_suntws.jpg");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imgPath);
            img.ScaleToFit(100f, 30f);
            PdfPCell cell1_imgLogo = new PdfPCell(img);
            cell1_imgLogo.MinimumHeight = 30f;
            cell1_imgLogo.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1_imgLogo.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 1st cell
            table.AddCell(cell1_imgLogo);

            Chunk chk_Title = new Chunk("PDI INSPECTION REPORT", Pdf_Fonts(1));
            PdfPCell cell2_Title = new PdfPCell(new Phrase(chk_Title));
            cell2_Title.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2_Title.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 2st cell
            table.AddCell(cell2_Title);

            return table;
        }
        private PdfPTable Draw_Customer()
        {
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 4f, 20f };
            table.SetWidths(widths);

            //1st cell
            Chunk chk_Category = new Chunk("CUSTOMER", Pdf_Fonts(2));
            PdfPCell cell_Category = new PdfPCell(new Phrase(chk_Category));
            cell_Category.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Category.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_Category.MinimumHeight = 20;
            table.AddCell(cell_Category);

            PdfPCell cell_value1 = new PdfPCell(new Phrase(str_CustName, Pdf_Fonts(3)));
            cell_value1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_value1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_value1.MinimumHeight = 20;
            table.AddCell(cell_value1);
            return table;
        }
        private PdfPTable Draw_Category(string strRptDate)
        {
            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 4f, 14f, 3f, 3f };
            table.SetWidths(widths);

            //1st cell
            Chunk chk_Category = new Chunk("CATEGORY", Pdf_Fonts(2));
            PdfPCell cell_Category = new PdfPCell(new Phrase(chk_Category));
            cell_Category.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Category.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_Category.MinimumHeight = 20;

            //Adding 1st cell
            table.AddCell(cell_Category);

            //2nd cell
            PdfTemplate template_black = writer.DirectContent.CreateTemplate(10, 10);
            template_black.SetColorFill(BaseColor.BLACK);
            template_black.Rectangle(0, 0, 10, 10);
            template_black.Fill();
            writer.ReleaseTemplate(template_black);

            Paragraph p = new Paragraph();
            Phrase ph1 = new Phrase("\t APPEREANCE \t\t", Pdf_Fonts(3));
            Phrase ph2 = new Phrase("\t DIM'S", Pdf_Fonts(2));
            p.Add("\t \t \t");
            p.Add(new Chunk(iTextSharp.text.Image.GetInstance(template_black), 0, 0));
            p.Add(ph1);
            p.Add("\t \t \t");
            p.Add(new Chunk(iTextSharp.text.Image.GetInstance(template_black), 0, 0));
            p.Add(ph2);
            PdfPCell cell_appereanceDims = new PdfPCell(p);
            cell_appereanceDims.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_appereanceDims.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_appereanceDims.MinimumHeight = 20;

            //Adding 2nd cell
            table.AddCell(cell_appereanceDims);

            //3nd cell
            PdfPCell cell_RptDate = new PdfPCell(new Phrase("REPORT DATE", Pdf_Fonts(2)));
            cell_RptDate.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_RptDate.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_RptDate.MinimumHeight = 20;

            //Adding 3nd cell
            table.AddCell(cell_RptDate);

            //4rd cell
            PdfPCell cell_RptDateVal = new PdfPCell(new Phrase(strRptDate, Pdf_Fonts(3)));
            cell_RptDateVal.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_RptDateVal.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_RptDateVal.MinimumHeight = 20;

            //Adding 4rd cell
            table.AddCell(cell_RptDateVal);

            return table;
        }
        private PdfPTable Draw_Defaults(string str_name1, string str_value1, string str_name2, string str_value2)
        {
            PdfPTable table = new PdfPTable(4);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 4f, 14f, 3f, 3f };
            table.SetWidths(widths);

            //1st cell
            PdfPCell cell_name1 = new PdfPCell(new Phrase(str_name1, Pdf_Fonts(2)));
            cell_name1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_name1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_name1.MinimumHeight = 20;

            //Adding 1st cell
            table.AddCell(cell_name1);

            //2nd cell
            PdfPCell cell_value1 = new PdfPCell(new Phrase(str_value1, Pdf_Fonts(3)));
            cell_value1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_value1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_value1.MinimumHeight = 20;

            //Adding 2nd cell
            table.AddCell(cell_value1);

            //3rd cell
            PdfPCell cell_name2 = new PdfPCell(new Phrase(str_name2, Pdf_Fonts(2)));
            cell_name2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_name2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_name2.MinimumHeight = 20;

            //Adding 3rd cell
            table.AddCell(cell_name2);

            //4th cell
            PdfPCell cell_value2 = new PdfPCell(new Phrase(str_value2, Pdf_Fonts(3)));
            cell_value2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_value2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_value2.MinimumHeight = 20;

            //Adding 4th cell
            table.AddCell(cell_value2);

            return table;
        }
        private PdfPTable Draw_Description(string strSampleQty)
        {
            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 1f, 5f, 3f, 10f, 3f };
            table.SetWidths(widths);

            try
            {
                //1st cell
                PdfPCell cell_sno = new PdfPCell(new Phrase("S NO", Pdf_Fonts(4)));
                cell_sno.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_sno.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_sno.MinimumHeight = 40;
                //adding 1st cell
                table.AddCell(cell_sno);

                //2nd cell
                PdfPCell cell_Description = new PdfPCell(new Phrase("DESCRIPTION", Pdf_Fonts(4)));
                cell_Description.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_Description.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_Description.MinimumHeight = 40;
                //addding 2nd cell
                table.AddCell(cell_Description);

                //3rd cell
                PdfPCell cell_Spec = new PdfPCell(new Phrase("Spec", Pdf_Fonts(4)));
                cell_Spec.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_Spec.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_Spec.MinimumHeight = 10;
                //adding 4th nested cell
                table.AddCell(cell_Spec);

                PdfPTable tbl_cell3 = new PdfPTable(Convert.ToInt32(strSampleQty));
                table.LockedWidth = true;
                decimal item_commonWidth = 10 / (Convert.ToDecimal(strSampleQty));
                float[] width_tbl_cell3 = new float[Convert.ToInt32(strSampleQty)];
                for (int i = 0; i < Convert.ToInt32(strSampleQty); i++)
                    width_tbl_cell3[i] = (float)item_commonWidth;
                //width_tbl_cell3[width_tbl_cell3.Length - 1] = 6;
                tbl_cell3.SetWidths(width_tbl_cell3);

                //1st nested cell
                PdfPCell cell_Observation = new PdfPCell(new Phrase("SUN-TWS OBESERVATION", Pdf_Fonts(4)));
                cell_Observation.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_Observation.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_Observation.Colspan = Convert.ToInt32(strSampleQty);
                cell_Observation.MinimumHeight = 20;
                //adding 1st nested cell
                tbl_cell3.AddCell(cell_Observation);

                for (int i = 0; i < Convert.ToInt32(strSampleQty); i++)
                {
                    PdfPCell cell_Act = new PdfPCell(new Phrase(lst_StencilNo[i].ToString(), Pdf_Fonts(4)));
                    cell_Act.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_Act.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell_Act.MinimumHeight = 10;
                    //adding nth nested cell
                    tbl_cell3.AddCell(cell_Act);
                }

                PdfPCell cell3 = new PdfPCell(tbl_cell3);
                //adding 3th cell
                table.AddCell(cell3);

                //3rd nested cell
                PdfPCell cell_Judgement = new PdfPCell(new Phrase("JUDGEMENT", Pdf_Fonts(4)));
                cell_Judgement.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_Judgement.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_Judgement.MinimumHeight = 40;
                //adding 3rd nested cell
                table.AddCell(cell_Judgement);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "expscanpdiinspectionreport.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return table;
        }
        private PdfPTable Draw_SpecActValues(int Sno, string Description, string ActColumnName, string SpecVal, string JudgeVal, DataTable dt, string strSampleQty)
        {
            PdfPTable table = new PdfPTable(4 + Convert.ToInt32(strSampleQty));
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            decimal item_commonWidth = 10 / (Convert.ToDecimal(strSampleQty));
            float[] widths = new float[4 + Convert.ToInt32(strSampleQty)];
            widths[0] = 1f;
            widths[1] = 5f;
            widths[2] = 3f;
            for (int i = 0; i < Convert.ToInt32(strSampleQty); i++)
                widths[i + 3] = (float)item_commonWidth;
            widths[widths.Length - 1] = 3f;
            table.SetWidths(widths);

            //1st cell
            PdfPCell cell_sno = new PdfPCell(new Phrase(Sno.ToString(), Pdf_Fonts(4)));
            cell_sno.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_sno.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_sno.MinimumHeight = 20;
            //adding 1st cell
            table.AddCell(cell_sno);

            //2nd cell
            PdfPCell cell_Description = new PdfPCell(new Phrase(Description, Pdf_Fonts(4)));
            cell_Description.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_Description.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_Description.MinimumHeight = 20;
            //addding 2nd cell
            table.AddCell(cell_Description);

            //3rd cell
            PdfPCell cell_Spec = new PdfPCell(new Phrase(SpecVal, Pdf_Fonts(5)));
            cell_Spec.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_Spec.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_Spec.MinimumHeight = 20;
            //addding 3rd cell
            table.AddCell(cell_Spec);

            //nth cell
            foreach (var stencilno in lst_StencilNo)
            {
                DataRow[] dr = dt.Select("StencilNo='" + stencilno.ToString() + "'");
                DataRow dr_filter = dr[0];

                //3rd cell
                PdfPCell cell_Act = new PdfPCell(new Phrase(dr_filter[ActColumnName].ToString(), Pdf_Fonts(5)));
                cell_Act.HorizontalAlignment = Element.ALIGN_CENTER;
                cell_Act.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell_Act.MinimumHeight = 20;

                //addding nth cell
                table.AddCell(cell_Act);
            }

            //last cell
            PdfPCell cell_Judge = new PdfPCell(new Phrase(JudgeVal, Pdf_Fonts(5)));
            cell_Judge.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_Judge.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_Judge.MinimumHeight = 20;

            //addding 3rd cell
            table.AddCell(cell_Judge);

            return table;
        }
        private PdfPTable Draw_ReportStatus()
        {
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 7f, 16f };
            table.SetWidths(widths);

            //1st cell
            PdfPCell cell_ReportStatus = new PdfPCell(new Phrase("REPORT STATUS", Pdf_Fonts(2)));
            cell_ReportStatus.MinimumHeight = 20;
            cell_ReportStatus.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_ReportStatus.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 1st cell
            table.AddCell(cell_ReportStatus);

            //2nd cell
            PdfTemplate template_black = writer.DirectContent.CreateTemplate(10, 10);
            template_black.SetColorFill(BaseColor.BLACK);
            template_black.Rectangle(0, 0, 10, 10);
            template_black.Fill();
            writer.ReleaseTemplate(template_black);

            PdfTemplate template_White = writer.DirectContent.CreateTemplate(10, 10);
            template_White.SetColorFill(BaseColor.BLACK);
            template_White.Rectangle(0, 0, 10, 10);
            template_White.Stroke();
            writer.ReleaseTemplate(template_White);


            Paragraph p = new Paragraph();
            p.Add("\t \t \t");
            p.Add(new Chunk(iTextSharp.text.Image.GetInstance(template_black), 0, 0));
            p.Add(new Phrase("\t APPROVED \t\t", Pdf_Fonts(2)));
            p.Add("\t \t \t \t \t \t");
            p.Add(new Chunk(iTextSharp.text.Image.GetInstance(template_White), 0, 0));
            p.Add(new Phrase("\t REJECTED", Pdf_Fonts(2)));
            p.Add("\t \t \t \t \t \t");
            p.Add(new Chunk(iTextSharp.text.Image.GetInstance(template_White), 0, 0));
            p.Add(new Phrase("\t OTHERS ( )", Pdf_Fonts(2)));

            PdfPCell cell_ReportStatusVal = new PdfPCell(p);
            cell_ReportStatusVal.HorizontalAlignment = Element.ALIGN_CENTER;
            cell_ReportStatusVal.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell_ReportStatusVal.MinimumHeight = 30;

            //Adding 2nd cell
            table.AddCell(cell_ReportStatusVal);

            return table;
        }
        private PdfPTable Draw_Remarks()
        {
            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = 800f;
            table.LockedWidth = true;

            //1st cell
            PdfPCell cell_ReportStatus = new PdfPCell(new Phrase("REMARKS:", Pdf_Fonts(2)));
            cell_ReportStatus.MinimumHeight = 40;
            cell_ReportStatus.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_ReportStatus.VerticalAlignment = Element.ALIGN_TOP;

            //Adding 1st cell
            table.AddCell(cell_ReportStatus);

            return table;
        }
        private PdfPTable Draw_Approval()
        {
            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = 800f;
            table.LockedWidth = true;
            float[] widths = new float[] { 23f };
            table.SetWidths(widths);

            //1st cell
            PdfPTable tbl = new PdfPTable(3);
            float[] widths_tbl = new float[] { 2f, 2f, 2f };
            tbl.SetWidths(widths_tbl);

            //0th nested cell
            PdfPCell cell0_tbl = new PdfPCell(new Phrase("Approval", Pdf_Fonts(4)));
            cell0_tbl.MinimumHeight = 10;
            cell0_tbl.Colspan = 3;
            cell0_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell0_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 0th nested cell
            tbl.AddCell(cell0_tbl);

            //lst nested cell
            PdfPCell cell1_tbl = new PdfPCell(new Phrase("Prepared", Pdf_Fonts(4)));
            cell1_tbl.MinimumHeight = 10;
            cell1_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 1st nested cell
            tbl.AddCell(cell1_tbl);

            //2nd nested cell
            PdfPCell cell2_tbl = new PdfPCell(new Phrase("Verified", Pdf_Fonts(4)));
            cell2_tbl.MinimumHeight = 10;
            cell2_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 2nd nested cell
            tbl.AddCell(cell2_tbl);

            //3rd nested cell
            PdfPCell cell3_tbl = new PdfPCell(new Phrase("Approved", Pdf_Fonts(4)));
            cell3_tbl.MinimumHeight = 10;
            cell3_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell3_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 3rd nested cell
            tbl.AddCell(cell3_tbl);

            //4th nested cell
            PdfPCell cell4_tbl = new PdfPCell(new Phrase("", Pdf_Fonts(4)));
            cell4_tbl.MinimumHeight = 20;
            cell4_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell4_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 4th nested cell
            tbl.AddCell(cell4_tbl);

            //5th nested cell
            PdfPCell cell5_tbl = new PdfPCell(new Phrase("", Pdf_Fonts(4)));
            cell5_tbl.MinimumHeight = 20;
            cell5_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell5_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 5th nested cell
            tbl.AddCell(cell5_tbl);

            //6th nested cell
            PdfPCell cell6_tbl = new PdfPCell(new Phrase("", Pdf_Fonts(4)));
            cell6_tbl.MinimumHeight = 20;
            cell6_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell6_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 6th nested cell
            tbl.AddCell(cell6_tbl);

            //7th nested cell
            PdfPCell cell7_tbl = new PdfPCell(new Phrase("", Pdf_Fonts(4)));
            cell7_tbl.MinimumHeight = 20;
            cell7_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell7_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 7th nested cell
            tbl.AddCell(cell7_tbl);

            //8th nested cell
            PdfPCell cell8_tbl = new PdfPCell(new Phrase("", Pdf_Fonts(4)));
            cell8_tbl.MinimumHeight = 20;
            cell8_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell8_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 8th nested cell
            tbl.AddCell(cell8_tbl);

            //9th nested cell
            PdfPCell cell9_tbl = new PdfPCell(new Phrase("", Pdf_Fonts(4)));
            cell9_tbl.MinimumHeight = 20;
            cell9_tbl.HorizontalAlignment = Element.ALIGN_CENTER;
            cell9_tbl.VerticalAlignment = Element.ALIGN_MIDDLE;

            //Adding 9th nested cell
            tbl.AddCell(cell9_tbl);

            PdfPCell cell2 = new PdfPCell(tbl);
            cell2.MinimumHeight = 20;
            cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell2.PaddingRight = 20;
            cell2.PaddingLeft = 350;

            //Adding 1st cell
            table.AddCell(cell2);

            return table;
        }
        private Font Pdf_Fonts(int font_id)
        {
            Font SelectedFont = null; ;
            switch (font_id)
            {
                case (1)://Title font
                    SelectedFont = FontFactory.GetFont("Times New Roman", 20, Font.BOLD);
                    break;
                case (2)://Heading font
                    SelectedFont = FontFactory.GetFont("Arial", 11, Font.NORMAL);
                    break;
                case (3)://Heading font for answer
                    SelectedFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    break;
                case (4):// Content font
                    SelectedFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
                    break;
                case (5):// Content font
                    SelectedFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
                    break;
            }
            return SelectedFont;
        }
    }
}