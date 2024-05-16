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
using System.Web.Services;
using System.Globalization;

namespace TTS
{
    public partial class exportcotsproduction : System.Web.UI.Page
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
                            gvProductionOrderList.DataSource = null;
                            gvProductionOrderList.DataBind();
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_prodcution_orders", DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvProductionOrderList.DataSource = dtorderlist;
                                gvProductionOrderList.DataBind();
                            }
                            else
                                lblErrMsg.Text = "No records";
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
        protected void lnkProductionBtn_Click(object sender, EventArgs e)
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
                chk1.Checked = false;
                chk2.Checked = false;
                chk3.Checked = false;
                chk5.Checked = false;
                chk7.Checked = false;
                chk8.Checked = false;
                txtWorkOrderNo.Text = "";
                txtDeliveryDate.Text = "";
                ddlPriority.SelectedIndex = -1;
                txtSplRemarks.Text = "";
                lblWoDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                lblReviseCount.Text = "00";

                lblStausOrderRefNo.Text = ((Label)row.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)row.FindControl("hdnStatusCustCode")).Value;
                lblCurrStatus.Text = ((Label)row.FindControl("lblStatusText")).Text;
                lblCustName.Text = ((Label)row.FindControl("lblStatusCustName")).Text;
                hdnOrderDate.Value = ((Label)row.FindControl("lblOrderDate")).Text;
                hdnShipType.Value = ((Label)row.FindControl("lblShipmenType")).Text;
                hdnRequestStatusTo.Value = ((HiddenField)row.FindControl("hdnRequestStatus")).Value;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                Bind_OrderMasterDetails();
                Bind_OrderItem();

                gv_OrderSumValue.DataSource = null;
                gv_OrderSumValue.DataBind();

                if (rdb_Plant.Items.Count > 1)
                    Bind_OrderSumValue();
                Bind_Attach_PdfFiles();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
                if (rdb_Plant.Items.Count == 1)
                {
                    rdb_Plant.SelectedIndex = 0;
                    rdb_Plant_IndexChanged(null, null);
                }
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
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
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
                    ViewState["dtItemList"] = dtItemList;

                    gvOrderItemList.FooterRow.Cells[7].Text = "TOTAL";
                    gvOrderItemList.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gvOrderItemList.FooterRow.Cells[9].Text = Convert.ToDecimal(dtItemList.Compute("Sum(unitpricepdf)", "")).ToString();
                    gvOrderItemList.FooterRow.Cells[10].Text = Convert.ToDecimal(dtItemList.Compute("Sum(totalfwt)", "")).ToString();

                    gvOrderItemList.Columns[11].Visible = false;
                    gvOrderItemList.Columns[12].Visible = false;
                    gvOrderItemList.Columns[13].Visible = false;
                    gvOrderItemList.Columns[14].Visible = false;
                    chkAssyWt.Visible = false;
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;
                        gvOrderItemList.Columns[13].Visible = true;
                        gvOrderItemList.Columns[14].Visible = true;
                        chkAssyWt.Visible = true;

                        gvOrderItemList.FooterRow.Cells[12].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                        gvOrderItemList.FooterRow.Cells[13].Text = Convert.ToDecimal(dtItemList.Compute("Sum(Rimpricepdf)", "")).ToString();
                        gvOrderItemList.FooterRow.Cells[14].Text = Convert.ToDecimal(dtItemList.Compute("Sum(totalRimWt)", "")).ToString();
                        break;
                    }

                    DataTable dtPlant = dtItemList.DefaultView.ToTable(true, "ItemPlant", "O_ID");
                    rdb_Plant.DataSource = dtPlant;
                    rdb_Plant.DataTextField = "ItemPlant";
                    rdb_Plant.DataValueField = "O_ID";
                    rdb_Plant.DataBind();
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

                    gv_OrderSumValue.FooterRow.Cells[6].Text = "TOTAL";
                    gv_OrderSumValue.FooterRow.Cells[7].Text = dtSumList.Compute("Sum([TOT QTY])", "").ToString();
                    gv_OrderSumValue.FooterRow.Cells[8].Text = dtSumList.Compute("Sum([TOT PRICE])", "").ToString();
                    gv_OrderSumValue.FooterRow.Cells[9].Text = dtSumList.Compute("Sum([TOT WT])", "").ToString();
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
                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtPro = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ProformaRefNo", sp2, DataAccess.Return_Type.DataTable);
                if (dtPro.Rows.Count == 1)
                {
                    lblProformaRefNo.Text = dtPro.Rows[0]["proformarefno"].ToString();
                    if (dtPro.Rows[0]["ContainerType"].ToString() == "20'")
                        chk6.Text = "20-FOOT CONTAINER";
                    else if (dtPro.Rows[0]["ContainerType"].ToString() == "40'")
                        chk6.Text = "40-FOOT CONTAINER";
                    else if (dtPro.Rows[0]["ContainerType"].ToString() == "LCL")
                        chk6.Text = "LCL";
                    chk6.Checked = true;

                    chk4.Checked = false;
                    if (dtPro.Rows[0]["PackingMethod"].ToString().ToLower() == "palletization")
                        chk4.Checked = true;
                }

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value), new SqlParameter("@Qstring", "workorder") };
                DataTable dtPdfFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdf_AttachInvoiceFiles", sp1, DataAccess.Return_Type.DataTable);

                if (dtPdfFile != null && dtPdfFile.Rows.Count > 0)
                {
                    gv_DownloadFiles.DataSource = dtPdfFile;
                    gv_DownloadFiles.DataBind();

                    if (rdb_Plant.Items.Count == dtPdfFile.Rows.Count - 1)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow3", "showStatusChangeBtn();", true);
                    }
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
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Label FileType = (Label)clickedRow.FindControl("lblFileType");
                LinkButton lnkpdfFileName = (LinkButton)clickedRow.FindControl("lnkPdfFileName");
                string Url = "";
                switch (FileType.Text)
                {
                    case "PROFORMA FILE":
                        Url = Server.MapPath("~/proformafiles/" + hdnCustCode.Value + "/") + lnkpdfFileName.Text;
                        break;
                    case "WORKORDER FILE":
                        Url = Server.MapPath("~/workorderfiles/" + hdnCustCode.Value + "/") + lnkpdfFileName.Text;
                        break;
                }
                Url = Url.Replace("TTS", "pdfs");
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkpdfFileName.Text);
                Response.WriteFile(Url);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdb_Plant_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                chk1.Checked = false;
                chk2.Checked = false;
                chk3.Checked = false;
                chk5.Checked = false;
                chk7.Checked = false;
                chk8.Checked = false;
                txtWorkOrderNo.Text = "";
                txtDeliveryDate.Text = "";
                ddlPriority.SelectedIndex = -1;
                txtSplRemarks.Text = "";
                lblWoDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                lblReviseCount.Text = "00";

                chk5.Text = "MADE IN INDIA MARKING";
                if (rdb_Plant.SelectedItem.Text == "SLTL" || rdb_Plant.SelectedItem.Text == "SITL")
                    chk5.Text = "MADE IN SRILANKA MARKING";

                txtWorkOrderNo.Enabled = true;
                SqlParameter[] sprev = new SqlParameter[] { new SqlParameter("@O_ID", rdb_Plant.SelectedItem.Value) };
                DataTable dtrev = (DataTable)daCOTS.ExecuteReader_SP("Sp_Chk_Exp_WorkOrderNo", sprev, DataAccess.Return_Type.DataTable);
                if (dtrev != null && dtrev.Rows.Count > 0)
                {
                    txtWorkOrderNo.Text = dtrev.Rows[0]["workorderno"].ToString();
                    txtWorkOrderNo.Enabled = false;
                    lblReviseCount.Text = dtrev.Rows[0]["revise_no"].ToString();
                    lblWoDate.Text = dtrev.Rows[0]["WoDate"].ToString();
                    ddlPriority.SelectedIndex = ddlPriority.Items.IndexOf(ddlPriority.Items.FindByValue(dtrev.Rows[0]["deliverytype"].ToString()));
                    txtSplRemarks.Text = dtrev.Rows[0]["splRemarks"].ToString();
                    txtDeliveryDate.Text = dtrev.Rows[0]["ExpectedShipDate"].ToString();
                    chk1.Checked = Convert.ToBoolean(dtrev.Rows[0]["category1"].ToString());
                    chk2.Checked = Convert.ToBoolean(dtrev.Rows[0]["category2"].ToString());
                    chk3.Checked = Convert.ToBoolean(dtrev.Rows[0]["category3"].ToString());
                    chk4.Checked = Convert.ToBoolean(dtrev.Rows[0]["category4"].ToString());
                    chk5.Checked = Convert.ToBoolean(dtrev.Rows[0]["category5"].ToString());
                    chk6.Checked = Convert.ToBoolean(dtrev.Rows[0]["category6"].ToString());
                    chk7.Checked = Convert.ToBoolean(dtrev.Rows[0]["category7"].ToString());
                    chk8.Checked = Convert.ToBoolean(dtrev.Rows[0]["category8"].ToString());
                    chkAssyWt.Checked = Convert.ToBoolean(dtrev.Rows[0]["AssyWtSplit"].ToString());
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow4", "gotoPreviewDiv('divStatusChange');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "gotoPreviewDiv('btnPrepareWorkOrder');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPrepareWorkOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (Insert_WorkOrderDetails() > 0)
                {
                    string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                    if (!Directory.Exists(serverURL + "/workorderfiles/" + hdnCustCode.Value + "/"))
                        Directory.CreateDirectory(serverURL + "/workorderfiles/" + hdnCustCode.Value + "/");
                    string path = serverURL + "/workorderfiles/" + hdnCustCode.Value + "/";

                    string sPathToWritePdfTo = path + lblStausOrderRefNo.Text + "_" + rdb_Plant.SelectedItem.Text + ".pdf";
                    string filename = lblStausOrderRefNo.Text + "_" + rdb_Plant.SelectedItem.Text + ".pdf";
                    Document doc = new Document(PageSize.A4, 25f, 25f, 50f, 25f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(sPathToWritePdfTo, FileMode.Create));
                    writer.PageEvent = new PDFWriterEvents("WORK ORDER - " + rdb_Plant.SelectedItem.Text, 20, 300, 50, 0);
                    doc.Open();
                    doc.Add(build_WorkOrder(doc, writer));
                    doc.Close();

                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@OID", rdb_Plant.SelectedItem.Value);
                    sp1[1] = new SqlParameter("@AttachFileName", filename);
                    sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                    sp1[3] = new SqlParameter("@FileType", "WORKORDER FILE");
                    daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);

                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value),
                        new SqlParameter("@requestmail", Request.Cookies["TTSUserEmail"].Value), 
                        new SqlParameter("@requestby", Request.Cookies["TTSUser"].Value), 
                        new SqlParameter("@processtype", "EXP WORKORDER")
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_ins_excel_prepare_history", sp);

                    gvProductionOrderList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                    Build_SelectOrderDetails(gvProductionOrderList.SelectedRow);

                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow3", "gotoPreviewDiv('btnPrepareWorkOrder');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private int Insert_WorkOrderDetails()
        {
            int intResp = 0;
            try
            {
                SqlParameter[] spdate = new SqlParameter[3];
                spdate[0] = new SqlParameter("@ExpectedShipDate", DateTime.ParseExact(txtDeliveryDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                spdate[1] = new SqlParameter("@O_ID", hdnOID.Value);
                spdate[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                daCOTS.ExecuteNonQuery_SP("sp_update_ExpectedShipDate", spdate);

                SqlParameter[] sp1 = new SqlParameter[]{
                    new SqlParameter("@O_ID", rdb_Plant.SelectedItem.Value),
                    new SqlParameter("@workorderno", txtWorkOrderNo.Text),
                    new SqlParameter("@deliverytype", ddlPriority.SelectedItem.Text),
                    new SqlParameter("@category1", chk1.Checked),
                    new SqlParameter("@category2", chk2.Checked),
                    new SqlParameter("@category3", chk3.Checked),
                    new SqlParameter("@category4", chk4.Checked),
                    new SqlParameter("@category5", chk5.Checked),
                    new SqlParameter("@category6", chk6.Checked),
                    new SqlParameter("@category7", chk7.Checked),
                    new SqlParameter("@category8", chk8.Checked),
                    new SqlParameter("@splRemarks", txtSplRemarks.Text),
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@revise_no", lblReviseCount.Text),
                    new SqlParameter("@AssyWtSplit", chkAssyWt.Checked)
                };
                intResp = daCOTS.ExecuteNonQuery_SP("sp_ins_workorderdetails_export", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return intResp;
        }
        private PdfPTable build_WorkOrder(Document doc, PdfWriter wr)
        {
            PdfPTable table = new PdfPTable(10);
            try
            {
                table.TotalWidth = 525f;
                table.LockedWidth = true;
                //                              No   Size  Rim   Con  ty   br  si  qt   wt   tot wt
                float[] widths = new float[] { 0.7f, 4.5f, 1.1f, 3f, 1.5f, 3f, 3f, 1.1f, 1.4f, 1.7f };
                table.SetWidths(widths);
                table.SpacingBefore = 25f;
                table.SpacingAfter = 15f;

                addWorkOrderHead(table, "WORK ORDER", 1, 10);
                addWorkMainDetails(table, "WORK ORDER NO: " + txtWorkOrderNo.Text, 0, 6);
                addWorkMainDetails(table, "DATE", 0, 1);
                if (lblWoDate.Text == "")
                    addWorkMainDetails(table, DateTime.Now.ToString("dd-MMM-yyyy"), 0, 3);
                else
                    addWorkMainDetails(table, lblWoDate.Text, 0, 3);

                Build_ChildTable(table);

                addWorkDescDetails(table, "Please arrange to process / despatch Solid Cushion Tyres as per details given below:\n \n", 0, 10);

                addWorkMainDetails(table, "Order Reference : " + lblStausOrderRefNo.Text + "  /  " + lblCustName.Text + "\nDate:" + hdnOrderDate.Value + " \n \n", 0, 10);

                addWorkOrderHeadCell(table, "No");
                addWorkOrderHeadCell(table, "SIZE");
                addWorkOrderHeadCell(table, "RIM");
                addWorkOrderHeadCell(table, "PLATFORM");
                addWorkOrderHeadCell(table, "TYPE");
                addWorkOrderHeadCell(table, "BRAND");
                addWorkOrderHeadCell(table, "SIDEWALL");
                addWorkOrderHeadCell(table, "QTY");
                addWorkOrderHeadCell(table, "WEI\nGHT");
                addWorkOrderHeadCell(table, "TOTAL WT");

                Build_WorkOrderItems(table);
                Build_DeliveryDetails(table, wr);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return table;
        }
        private void addWorkOrderHead(PdfPTable table, string text, int alignCode, int cols)
        {
            try
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
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void addWorkMainDetails(PdfPTable table, string text, int alignCode, int cols)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                PdfPCell cellTot = new PdfPCell();
                cellTot = new PdfPCell(new Phrase(text, titleFont));
                //cellTot.FixedHeight = 18f;
                cellTot.Colspan = cols;
                cellTot.HorizontalAlignment = alignCode;
                cellTot.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                table.AddCell(cellTot);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void addWorkDescDetails(PdfPTable table, string text, int alignCode, int cols)
        {
            try
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
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_ChildTable(PdfPTable table)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                PdfPCell reviseTable = new PdfPCell(new Phrase("TO: Production Executive / Despatch Section", titleFont));
                reviseTable.Colspan = 6;
                reviseTable.FixedHeight = 40f;
                table.AddCell(reviseTable);

                PdfPTable nested = new PdfPTable(2);
                float[] widths = new float[] { 3.2f, 4.5f };
                nested.SetWidths(widths);
                reviseTable = new PdfPCell(new Phrase("REV NO", titleFont));
                nested.AddCell(reviseTable);
                nested.AddCell(new Phrase(Convert.ToInt32(lblReviseCount.Text).ToString("00"), titleFont));

                reviseTable = new PdfPCell(new Phrase("REV DATE", titleFont));
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                nested.AddCell(reviseTable);
                if (lblWoDate.Text == "")
                    nested.AddCell(new Phrase("\n", titleFont));
                else
                    nested.AddCell(new Phrase(lblWoDate.Text, titleFont));

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Padding = 0f;
                nesthousing.Colspan = 4;
                table.AddCell(nesthousing);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void addWorkOrderHeadCell(PdfPTable table, string text)
        {
            try
            {
                var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
                cell.FixedHeight = 24f;
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                table.AddCell(cell);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_WorkOrderItems(PdfPTable table)
        {
            try
            {
                string strQty = "0";
                string strWt = "0";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", rdb_Plant.SelectedItem.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_orderitem_list_PPC", sp, DataAccess.Return_Type.DataTable);

                DataView dtDescView = new DataView(dtItemList);
                dtDescView.Sort = "brand,AssyRimstatus ASC";
                DataTable distinctDesc = dtDescView.ToTable(true, "brand", "tyretype", "TypeDesc", "category", "AssyRimstatus", "RimDwg", "EdcNo");

                int j = 1;
                int rowcount = 0;
                foreach (DataRow fRow in distinctDesc.Rows)
                {
                    string strItemDesc = fRow["brand"].ToString() != "" ? fRow["brand"].ToString() + ", " : "";
                    strItemDesc += fRow["TypeDesc"].ToString() != "" ? fRow["TypeDesc"].ToString() + ", " : "";
                    strItemDesc += fRow["tyretype"].ToString() != "" ? fRow["tyretype"].ToString() : "";
                    string strLimpet = fRow["tyretype"].ToString() != "" ? ((fRow["tyretype"].ToString()).Substring(fRow["tyretype"].ToString().Length - 1, 1).ToUpper()) : "";
                    strItemDesc += strLimpet == "X" ? " WITHOUT CLIP / LIMPET" : strLimpet == "4" ? " WITH CLIP / LIMPET" : "";
                    if (Convert.ToBoolean(fRow["AssyRimstatus"].ToString()) == true)
                        strItemDesc += " WITH ASSY (" + fRow["EdcNo"].ToString().ToUpper() + " / " + fRow["RimDwg"].ToString().ToUpper() + ")";

                    strItemDesc = strItemDesc != "" ? strItemDesc : fRow["category"].ToString() + (fRow["RimDwg"].ToString() != "" ? " (" + fRow["EdcNo"].ToString().ToUpper() +
                        " / " + fRow["RimDwg"].ToString() + ")" : "");
                    addWorkOrderTotalCell1(table, strItemDesc.ToUpper(), 0, 10);
                    foreach (DataRow dRow in dtItemList.Select("category='" + fRow["category"].ToString() + "' and brand='" + fRow["brand"].ToString()
                        + "' and tyretype='" + fRow["tyretype"].ToString() + "' and TypeDesc='" + fRow["TypeDesc"].ToString() + "' and AssyRimstatus='" +
                        fRow["AssyRimstatus"].ToString() + "' and EdcNo='" + fRow["EdcNo"].ToString() + "' and RimDwg='" + fRow["RimDwg"].ToString() +
                        "' and ItemPlant='" + rdb_Plant.SelectedItem.Text + "'"))
                    {
                        if (Convert.ToBoolean(dRow["AssyRimstatus"]) == false)
                        {
                            addWorkOrderItemCell(table, Convert.ToString(j++), 0);
                            addWorkOrderItemCell(table, ((dRow["category"].ToString() == "SPLIT RIMS" || dRow["category"].ToString() == "POB WHEEL") ?
                                dRow["EdcRimSize"].ToString() : dRow["Tyresize"].ToString()), 0);
                            addWorkOrderItemCell(table, dRow["RimSize"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["Config"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["tyretype"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["brand"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["sidewall"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["itemqty"].ToString(), 2);
                            addWorkOrderItemCell(table, dRow["tyrewt"].ToString(), 2);
                            addWorkOrderItemCell(table, dRow["totalfwt"].ToString(), 2);
                            rowcount++;
                        }
                        else if (Convert.ToBoolean(dRow["AssyRimstatus"]) == true && chkAssyWt.Checked)
                        {
                            table.AddCell(Build_ItemTableListStyleSameQty(Convert.ToString(j++), 2));
                            table.AddCell(Build_ItemTableListStyleSameQty(((dRow["category"].ToString() == "SPLIT RIMS" || dRow["category"].ToString() == "POB WHEEL") ?
                                dRow["EdcRimSize"].ToString() : dRow["Tyresize"].ToString()), 2));
                            table.AddCell(Build_ItemTableListStyleSameQty(dRow["RimSize"].ToString(), 2));
                            table.AddCell(Build_ItemTableListStyleSameQty(dRow["Config"].ToString(), 2));
                            table.AddCell(Build_ItemTableListStyleSameQty(dRow["tyretype"].ToString(), 2));
                            table.AddCell(Build_ItemTableListStyleSameQty(dRow["brand"].ToString(), 2));
                            table.AddCell(Build_ItemTableListStyleSameQty(dRow["sidewall"].ToString(), 2));
                            table.AddCell(Build_ItemTableListStyleSameQty(dRow["itemqty"].ToString(), 2));
                            addWorkOrderItemCell(table, dRow["tyrewt"].ToString(), 2);
                            addWorkOrderItemCell(table, (Convert.ToDecimal(dRow["itemqty"].ToString()) * Convert.ToDecimal(dRow["tyrewt"].ToString())).ToString(), 2);
                            addWorkOrderItemCell(table, dRow["Rimfinishedwt"].ToString(), 2);
                            addWorkOrderItemCell(table, (Convert.ToDecimal(dRow["Rimitemqty"].ToString()) * Convert.ToDecimal(dRow["Rimfinishedwt"].ToString())).ToString(), 2);
                            rowcount++;
                        }
                        else if (Convert.ToBoolean(dRow["AssyRimstatus"]) == true && !chkAssyWt.Checked)
                        {
                            addWorkOrderItemCell(table, Convert.ToString(j++), 0);
                            addWorkOrderItemCell(table, ((dRow["category"].ToString() == "SPLIT RIMS" || dRow["category"].ToString() == "POB WHEEL") ?
                                dRow["EdcRimSize"].ToString() : dRow["Tyresize"].ToString()), 0);
                            addWorkOrderItemCell(table, dRow["RimSize"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["Config"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["tyretype"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["brand"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["sidewall"].ToString(), 0);
                            addWorkOrderItemCell(table, dRow["itemqty"].ToString(), 2);
                            addWorkOrderItemCell(table, dRow["bothfinWt"].ToString(), 2);
                            addWorkOrderItemCell(table, (Convert.ToDecimal(dRow["ItemQty"].ToString()) * Convert.ToDecimal(dRow["bothfinWt"].ToString())).ToString(), 2);
                            rowcount++;
                        }
                        strQty = (Convert.ToInt32(strQty) + Convert.ToInt32(dRow["itemqty"].ToString())).ToString();
                        strWt = (Convert.ToDecimal(strWt) + Convert.ToDecimal(dRow["finishedwt"].ToString())).ToString();
                    }
                }
                for (int k = rowcount; k < 13; k++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase("\n"));
                    cell.FixedHeight = 20f;
                    table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell);
                    table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell); table.AddCell(cell);
                }
                addWorkOrderTotalCell(table, "TOTAL", 2, 7);
                addWorkOrderTotalCell(table, strQty, 1, 1);
                addWorkOrderTotalCell(table, strWt, 2, 2);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private PdfPCell Build_ItemTableListStyleSameQty(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.Border = Rectangle.BOTTOM_BORDER;
            cell.Rowspan = 2;
            return cell;
        }
        private void addWorkOrderItemCell(PdfPTable table, string text, int alignCode)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
            cell.HorizontalAlignment = alignCode;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.FixedHeight = 20f;
            table.AddCell(cell);
        }
        private void addWorkOrderTotalCell(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
            cell.Colspan = cols;
            cell.HorizontalAlignment = alignCode;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.FixedHeight = 22f;
            table.AddCell(cell);
        }
        private void addWorkOrderTotalCell1(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
            cell.Colspan = cols;
            cell.HorizontalAlignment = alignCode;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.FixedHeight = 20f;
            table.AddCell(cell);
        }
        private void Build_DeliveryDetails(PdfPTable table, PdfWriter wr)
        {
            try
            {
                addWorkOrderTotalCell(table, "DELIVERY: " + ddlPriority.SelectedItem.Text.ToUpper(), 0, 3);
                addWorkOrderTotalCell(table, "THIS IS " + hdnShipType.Value + " SHIPMENT", 0, 3);
                addWorkOrderTotalCell(table, "Expected Shipping Date: " + txtDeliveryDate.Text, 2, 4);
                PdfTemplate template = wr.DirectContent.CreateTemplate(70, 20);
                template.SetColorFill(BaseColor.BLACK);
                template.Rectangle(0, 0, 70, 20);
                template.Fill();
                wr.ReleaseTemplate(template);
                PdfTemplate template1 = wr.DirectContent.CreateTemplate(70, 20);
                template.SetColorFill(BaseColor.WHITE);
                template1.Rectangle(0, 0, 70, 20);
                template1.Stroke();
                wr.ReleaseTemplate(template1);
                var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                PdfPTable nested = new PdfPTable(4);
                float[] widths = new float[] { 9f, 4f, 9.8f, 4f };
                nested.SetWidths(widths);

                PdfPCell reviseTable = new PdfPCell(new Phrase("1) " + chk1.Text, titleFont));
                reviseTable.FixedHeight = 30f;
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                if (chk1.Checked == true)
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                else
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template1));

                reviseTable.Border = Rectangle.NO_BORDER;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nested.AddCell(reviseTable);

                reviseTable = new PdfPCell(new Phrase("5) " + chk4.Text, titleFont));
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                if (chk4.Checked == true)
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                else
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template1));
                reviseTable.Border = Rectangle.NO_BORDER;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nested.AddCell(reviseTable);

                reviseTable = new PdfPCell(new Phrase("2) " + chk2.Text, titleFont));
                reviseTable.FixedHeight = 30f;
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);
                if (chk2.Checked == true)
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                else
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template1));
                reviseTable.Border = Rectangle.NO_BORDER;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nested.AddCell(reviseTable);
                reviseTable = new PdfPCell(new Phrase("6) " + chk5.Text, titleFont));
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                if (chk5.Checked == true)
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                else
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template1));
                reviseTable.Border = Rectangle.NO_BORDER;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nested.AddCell(reviseTable);

                reviseTable = new PdfPCell(new Phrase("3) " + chk3.Text, titleFont));
                reviseTable.FixedHeight = 30f;
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                if (chk3.Checked == true)
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                else
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template1));
                reviseTable.Border = Rectangle.NO_BORDER;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nested.AddCell(reviseTable);

                reviseTable = new PdfPCell(new Phrase("7) " + chk6.Text, titleFont));
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                if (chk6.Checked == true)
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                else
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template1));
                reviseTable.Border = Rectangle.NO_BORDER;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nested.AddCell(reviseTable);

                reviseTable = new PdfPCell(new Phrase("4) " + chk7.Text, titleFont));
                reviseTable.FixedHeight = 30f;
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                if (chk7.Checked == true)
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                else
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template1));
                reviseTable.Border = Rectangle.NO_BORDER;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nested.AddCell(reviseTable);

                reviseTable = new PdfPCell(new Phrase("8) " + chk8.Text, titleFont));
                reviseTable.HorizontalAlignment = Element.ALIGN_LEFT;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.Border = Rectangle.NO_BORDER;
                nested.AddCell(reviseTable);

                if (chk8.Checked == true)
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template));
                else
                    reviseTable = new PdfPCell(iTextSharp.text.Image.GetInstance(template1));
                reviseTable.Border = Rectangle.NO_BORDER;
                reviseTable.VerticalAlignment = Element.ALIGN_MIDDLE;
                reviseTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nested.AddCell(reviseTable);

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Padding = 0f;
                nesthousing.Colspan = 10;
                table.AddCell(nesthousing);

                addWorkOrderBottomCell(table, "CONFIRM PRODUCTION FEASIBILITY AND LOADABILITY WITHIN 3 DAYS THE DATE OF WORK ORDER RECEIVED\n", 0, 10);

                addWorkOrderBottomCell(table, "SPECIAL REMARKS: \n" + txtSplRemarks.Text, 0, 10);

                addWorkOrderBottomCell(table, "Prepared by \n \n \n ", 0, 3);
                addWorkOrderBottomCell(table, "Checked by \n \n \n ", 1, 3);
                addWorkOrderBottomCell(table, "Approved by \n \n \n ", 2, 4);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void addWorkOrderBottomCell(PdfPTable table, string text, int alignCode, int cols)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(text, titleFont));
            cell.Colspan = cols;
            cell.HorizontalAlignment = alignCode;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            //cell.FixedHeight = 22f;
            table.AddCell(cell);
        }
        protected void btnSaveChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                if (hdnRequestStatusTo.Value == "23")
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value), new SqlParameter("@RequestStatus", hdnRequestStatusTo.Value) };
                    resp = daCOTS.ExecuteNonQuery_SP("sp_upd_exp_proforma_completed", sp);
                }
                else
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", Convert.ToInt32(hdnOID.Value)), 
                        new SqlParameter("@statusid", hdnCustCode.Value == "1608" ? Convert.ToInt32(24) : Convert.ToInt32(34)), 
                        new SqlParameter("@feedback", txtOrderChangeComments.Text.Replace("\r\n", "~")), 
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                    };
                    resp = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);

                    //SLTL&SITL
                    //____________________________________________________________________________
                    //for (int k = 0; k < rdb_Plant.Items.Count; k++)
                    //{
                    //    if (rdb_Plant.Items[k].Text == "SLTL" || rdb_Plant.Items[k].Text == "SITL")
                    //    {
                    //        SqlParameter[] spL = new SqlParameter[] { 
                    //            new SqlParameter("@O_ID", Convert.ToInt32(rdb_Plant.Items[k].Value)), 
                    //            new SqlParameter("@statusid", Convert.ToInt32(24)), 
                    //            new SqlParameter("@feedback", txtOrderChangeComments.Text.Replace("\r\n", "~")), 
                    //            new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                    //        };
                    //        daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", spL);
                    //    }
                    //}
                    //____________________________________________________________________________
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