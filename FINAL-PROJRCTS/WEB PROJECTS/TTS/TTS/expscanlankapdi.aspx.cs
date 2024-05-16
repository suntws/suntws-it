using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Web.Services;
using System.Data.Common;
using System.Data.OleDb;

namespace TTS
{
    public partial class expscanlankapdi : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True"))
                        {
                            btnPdiUploadFileSave.Style.Add("display", "none");
                            tb_pdi_uploadMsg.Style.Add("display", "none");
                            tb_LoadDetails.Style.Add("display", "none");
                            tb_pdiuploadCtrls.Style.Add("display", "none");
                            tb_Shipmethod.Style.Add("display", "none");
                            lblError.Text = "";

                            lblPageTitle.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + " - EXPORT ";
                            hdnPlant.Value = Utilities.Decrypt(Request["pid"].ToString());
                            hdnCustCode.Value = Utilities.Decrypt(Request["ccode"].ToString());
                            lbl_OrderNo.Text = Utilities.Decrypt(Request["oid"].ToString());

                            SqlParameter[] sp1 = new SqlParameter[] { 
                                new SqlParameter("@custcode", hdnCustCode.Value),
                                new SqlParameter("@orderrefno", lbl_OrderNo.Text),
                                new SqlParameter("@O_ID", Utilities.Decrypt(Request["id"].ToString())), 
                                new SqlParameter("@pdiPlant", hdnPlant.Value) 
                            };
                            DataSet dtDomItemList = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_orderitem_list_for_pdi_inspect", sp1, DataAccess.Return_Type.DataSet);
                            if (dtDomItemList.Tables.Count == 2)
                            {
                                if (dtDomItemList.Tables[0].Rows.Count > 0)
                                {
                                    ViewState["dtDomItemList"] = dtDomItemList.Tables[0];
                                    hdnPdiFor.Value = dtDomItemList.Tables[0].Rows[0]["ProcessType"].ToString();
                                }
                                if (dtDomItemList.Tables[1].Rows.Count > 0)
                                {
                                    hdnPID.Value = dtDomItemList.Tables[1].Rows[0]["PID"].ToString();
                                    txt_OrderRefNo.Text = dtDomItemList.Tables[1].Rows[0]["workorderno"].ToString();
                                    txt_OrderRefNo.Enabled = dtDomItemList.Tables[1].Rows[0]["workorderno"].ToString() == "" ? true : false;
                                    lblCustomer.Text = dtDomItemList.Tables[1].Rows[0]["custfullname"].ToString();
                                    txt_OrderQty.Text = dtDomItemList.Tables[1].Rows[0]["itemqty"].ToString();
                                }

                                if (Utilities.Decrypt(Request["fid"].ToString()) == "e" && hdnPdiFor.Value == "ORDER")
                                    lnkWorkOrder.Text = (Utilities.Decrypt(Request["oid"].ToString()) + "_" + Utilities.Decrypt(Request["pid"].ToString()) + ".pdf");

                                if (txt_OrderRefNo.Text == "" && hdnPdiFor.Value == "ORDER")
                                {
                                    SqlParameter[] spWo = new SqlParameter[] { 
                                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["id"].ToString())), 
                                        new SqlParameter("@Plant", hdnPlant.Value) 
                                    };
                                    txt_OrderRefNo.Text = (string)daCOTS.ExecuteScalar_SP("sp_sel_workorderno", spWo);
                                }

                                if (hdnPID.Value != "")
                                    Bind_ContainerLoad_Div();
                                else
                                    tb_pdiuploadCtrls.Style.Add("display", "block");
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
        protected void btnBarcodeVerify_Click(object sender, EventArgs e)
        {
            try
            {
                if (fupStockInward.HasFile)
                {
                    btnPdiUploadFileSave.Style.Add("display", "none");
                    tb_pdi_uploadMsg.Style.Add("display", "none");
                    tb_LoadDetails.Style.Add("display", "none");
                    tb_Shipmethod.Style.Add("display", "none");
                    lblError.Text = "";

                    string FileName = Path.GetFileName(fupStockInward.PostedFile.FileName);
                    string Extension = Path.GetExtension(fupStockInward.PostedFile.FileName);

                    if (Extension == ".xls" || Extension == ".xlsx" || Extension == ".csv" || Extension == ".txt")
                    {
                        string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                        if (!Directory.Exists(serverURL + "/pdifiles/" + hdnPlant.Value + "/"))
                            Directory.CreateDirectory(serverURL + "/pdifiles/" + hdnPlant.Value + "/");
                        string path = serverURL + "/pdifiles/" + hdnPlant.Value + "/";

                        string FilePath = path + FileName;
                        fupStockInward.SaveAs(FilePath);

                        string strRenameFile = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") +
                            DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + Extension;
                        FileInfo file1 = new FileInfo(serverURL + "/pdifiles/" + hdnPlant.Value + "/" + strRenameFile);
                        if (file1.Exists)
                            file1.Delete();
                        File.Copy(FilePath, serverURL + "/pdifiles/" + hdnPlant.Value + "/" + strRenameFile);
                        File.Delete(FilePath);

                        GetDataFromScanFile(path + strRenameFile, Extension);
                    }
                    else
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Upload file is not correct. Kindly upload only [.xls,.xlsx,.csv,.txt]');", true);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Alert999", "alert(Choose the pdi upload file');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void GetDataFromScanFile(string strPath, string strExtension)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dtInvalid = new DataTable();
                dtInvalid.Columns.Add(new DataColumn("invalidbarcode", typeof(System.String)));
                if (strExtension.ToLower().CompareTo(".xls") == 0 || strExtension.ToLower().CompareTo(".xlsx") == 0)
                {
                    string conStr = "";
                    switch (strExtension.ToLower())
                    {
                        case ".xls": //Excel 97-03
                            conStr = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                            break;
                        case ".xlsx": //Excel 07
                            conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
                            break;
                    }

                    conStr = String.Format(conStr, strPath, 0);
                    OleDbConnection connExcel = new OleDbConnection(conStr);
                    OleDbCommand cmdExcel = new OleDbCommand();
                    OleDbDataAdapter oda = new OleDbDataAdapter();
                    cmdExcel.Connection = connExcel;

                    //Get the name of First Sheet
                    connExcel.Open();
                    DataTable dtExcelSchema;
                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    connExcel.Close();

                    //Read Data from First Sheet
                    connExcel.Open();
                    cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                    oda.SelectCommand = cmdExcel;
                    oda.Fill(dt);
                    connExcel.Close();
                    if (strExtension.ToLower() == ".xls")
                    {
                        string strDtHead = dt.Columns[0].ColumnName;
                        dt.Rows.Add(strDtHead);
                    }
                    dt.Columns[0].ColumnName = "scanbarcode";
                    for (int col = dt.Columns.Count - 1; col > 0; col--)
                    {
                        dt.Columns.RemoveAt(col);
                    }
                    foreach (DataRow delRow in dt.Select("LEN(scanbarcode)<>19"))
                    {
                        dtInvalid.Rows.Add(delRow["scanbarcode"].ToString());
                        delRow.Delete();
                    }
                    dt.AcceptChanges();
                }
                else if (strExtension.ToLower().CompareTo(".csv") == 0 || strExtension.ToLower().CompareTo(".txt") == 0)
                {
                    DataColumn col = new DataColumn("scanbarcode", typeof(System.String));
                    dt.Columns.Add(col);

                    string[] arrBarcodes = new string[] { };
                    string text = File.ReadAllText(strPath);
                    string[] stringSeparators = new string[] { "\r\n" };
                    arrBarcodes = text.Split(stringSeparators, StringSplitOptions.None);
                    foreach (string line in arrBarcodes)
                    {
                        if (line.Length == 19)
                            dt.Rows.Add(line);
                        else
                            dtInvalid.Rows.Add(line);
                    }
                }

                if (dt.Rows.Count > 0 || dtInvalid.Rows.Count > 0)
                {
                    lblMsg.Text = "TOTAL LINES IN THE FILE " + (dt.Rows.Count + dtInvalid.Rows.Count);
                    lblMsg.Text += "\r\nINVALID LINES " + dtInvalid.Rows.Count;
                }
                if (dt.Rows.Count > 0)
                {
                    dt = dt.DefaultView.ToTable(true, "scanbarcode");
                    Check_ScanBased_PdiData(dt);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Check_ScanBased_PdiData(DataTable dtScanData)
        {
            try
            {
                btnPdiUploadFileSave.Style.Add("display", "none");
                tb_pdi_uploadMsg.Style.Add("display", "none");
                tb_LoadDetails.Style.Add("display", "none");
                tb_Shipmethod.Style.Add("display", "none");
                lblError.Text = "";
                gvLankaPdiList.DataSource = null;
                gvLankaPdiList.DataBind();
                bool boolScanQtyMatch = true;

                if (txt_OrderQty.Text == dtScanData.Rows.Count.ToString())
                {
                    SqlParameter[] spSel = new SqlParameter[] { 
                        new SqlParameter("@PdiScanBarcode_DT", dtScanData), 
                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["id"].ToString())),
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()))
                    };
                    DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_sel_ScanPdi_to_Orderitem", spSel, DataAccess.Return_Type.DataTable);

                    if (dtData.Rows.Count > 0)
                    {
                        if (dtData.Columns.Count <= 3)
                        {
                            gvLankaPdiList.DataSource = dtData;
                            gvLankaPdiList.DataBind();
                            lblError.Text = "CORRECT THE BELOW " + dtData.Rows.Count + " BARCODE ERRORS IN THE UPLOAD PDI FILE.";
                        }
                        else if (dtData.Columns.Count > 3)
                        {
                            DataTable dtItem = ViewState["dtDomItemList"] as DataTable;
                            foreach (DataRow iRow in dtItem.Rows)
                            {
                                Int32 itemQty = Convert.ToInt32(iRow["itemqty"].ToString());
                                Int32 matchqty = 0;
                                for (int k = 0; k < itemQty; k++)
                                {
                                    foreach (DataRow sRow in dtData.Select("TYRESIZE='" + iRow["tyresize"].ToString() + "' and RIMWIDTH='" +
                                        iRow["rimsize"].ToString() + "' and TYRETYPE='" + iRow["tyretype"].ToString() + "' and BRAND='" + iRow["brand"].ToString() +
                                        "' and PLATFORM='" + iRow["Config"].ToString() + "' and SIDEWALL='" + iRow["sidewall"].ToString() + "' and MATCHED_STAUS='0'"))
                                    {
                                        sRow["MATCHED_STAUS"] = "1";
                                        matchqty += 1;
                                        break;
                                    }
                                }
                                if ((itemQty - matchqty) > 0)
                                {
                                    for (int k = 0; k < (itemQty - matchqty); k++)
                                    {
                                        foreach (DataRow sRow in dtData.Select("TYRESIZE='" + iRow["tyresize"].ToString() + "' and RIMWIDTH='" +
                                            iRow["rimsize"].ToString() + "' and TYRETYPE='" + iRow["tyretype"].ToString() + "' and MATCHED_STAUS='0'"))
                                        {
                                            sRow["MATCHED_STAUS"] = "1";
                                            matchqty += 1;
                                            break;
                                        }
                                    }
                                }
                                if ((itemQty - matchqty) > 0)
                                {
                                    DataTable dtUpgrade = (DataTable)daCOTS.ExecuteReader("select distinct UpgradeType from TypeWise_UpgradeList where MasterType='" +
                                        iRow["tyretype"].ToString() + "'", DataAccess.Return_Type.DataTable);
                                    for (int k = 0; k < (itemQty - matchqty); k++)
                                    {
                                        bool boolUpgrade = false;
                                        foreach (DataRow uRow in dtUpgrade.Rows)
                                        {
                                            foreach (DataRow sRow in dtData.Select("TYRESIZE='" + iRow["tyresize"].ToString() + "' and RIMWIDTH='" +
                                                iRow["rimsize"].ToString() + "' and TYRETYPE='" + uRow["UpgradeType"].ToString() + "' and MATCHED_STAUS='0'"))
                                            {
                                                sRow["MATCHED_STAUS"] = "1";
                                                boolUpgrade = true;
                                                break;
                                            }
                                            if (boolUpgrade)
                                                break;
                                        }
                                    }
                                }
                            }
                            Int32 unmatchQty = dtData.Select("MATCHED_STAUS='0'").Count();
                            if (unmatchQty > 0)
                            {
                                DataView dv_Scan = new DataView(dtData);
                                dv_Scan.RowFilter = "MATCHED_STAUS='0'";
                                DataTable dtUnMatch = new DataTable();
                                dtUnMatch = dv_Scan.ToTable(true);
                                gvLankaPdiList.DataSource = dtUnMatch;
                                gvLankaPdiList.DataBind();

                                if (Request.Cookies["TTSUser"].Value.ToLower() != "somu" || Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                                {
                                    boolScanQtyMatch = false;
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JVerify3", "alert('TOTAL UNMATCHED QTY FOR THIS ORDER: " + unmatchQty.ToString() + "');", true);
                                }
                            }
                            else
                            {
                                ViewState["dtData"] = dtData;
                                gvLankaPdiList.DataSource = dtData;
                                gvLankaPdiList.DataBind();
                            }
                            if (boolScanQtyMatch)
                                btnPdiUploadFileSave.Style.Add("display", "block");
                        }
                        tb_pdi_uploadMsg.Style.Add("display", "block");
                    }
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Alert1000", "alert('Qty not equal: Order " + txt_OrderQty.Text +
                        " - PDI " + dtScanData.Rows.Count.ToString() + ".');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPdiUploadFileSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtScanData = ViewState["dtData"] as DataTable;
                DataView dv_Scan = new DataView(dtScanData);
                DataTable dtBarcode = dv_Scan.ToTable(true, "BARCODE");
                dtBarcode.Columns[0].ColumnName = "scanbarcode";

                int returnPID = 0;
                SqlParameter[] spIns = new SqlParameter[] { 
                    new SqlParameter("@PdiScanBarcode_DT", dtBarcode), 
                    new SqlParameter("@O_ID", Utilities.Decrypt(Request["id"].ToString())), 
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                    new SqlParameter("@custcode", hdnCustCode.Value), 
                    new SqlParameter("@workorderno", txt_OrderRefNo.Text), 
                    new SqlParameter("@orderqty", Convert.ToInt32(txt_OrderQty.Text)), 
                    new SqlParameter("@orderrefno", lbl_OrderNo.Text), 
                    new SqlParameter("@Username", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@PID", SqlDbType.Int, 4, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Default, returnPID) 
                };
                returnPID = (int)daCOTS.ExecuteScalar_SP("sp_ins_pdi_data_lanka", spIns);
                if (returnPID > 0)
                {
                    hdnPID.Value = returnPID.ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Alert990", "alert('PDI SUCCESSFULLY SAVED');", true);
                    btnPdiUploadFileSave.Style.Add("display", "none");
                    tb_pdi_uploadMsg.Style.Add("display", "none");
                    tb_pdiuploadCtrls.Style.Add("display", "none");
                    Bind_ContainerLoad_Div();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ContainerLoad_Div()
        {
            try
            {
                lblErr.Text = "";
                lblShipType.Text = "";
                lblFinalLoading.Text = "";
                btnSavePdiLoadStatus.Text = "DISPATCH TO CUSTOMER";
                if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                {
                    SqlParameter[] spType = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["id"].ToString())) };
                    DataSet dsType = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_order_shiptype_v1", spType, DataAccess.Return_Type.DataSet);
                    if (dsType.Tables[0].Rows.Count == 1)
                    {
                        lblShipType.Text = dsType.Tables[0].Rows[0]["ShipmentType"].ToString();
                        lblFinalLoading.Text = dsType.Tables[0].Rows[0]["ContainerLoadFrom"].ToString();
                        if (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() != lblFinalLoading.Text)
                        {
                            btnSavePdiLoadStatus.Text = "SENT TO " + lblFinalLoading.Text;
                            hdnTotOrderQty.Value = txt_OrderQty.Text;
                        }
                        else
                        {
                            if (lblShipType.Text == "COMBI")
                            {
                                SqlParameter[] spA = new SqlParameter[] { 
                                    new SqlParameter("@ID", hdnPID.Value), 
                                    new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())), 
                                    new SqlParameter("@Loadedby", Request.Cookies["TTSUser"].Value) 
                                };
                                daCOTS.ExecuteNonQuery_SP("sp_upd_lanka_combibarcode_status", spA);
                            }

                            string strQtyTable = "<table cellspacing='0' rules='all' border='0'>";
                            foreach (DataRow rowQ in dsType.Tables[1].Rows)
                            {
                                strQtyTable += "<tr><th style='background-color: #f1e8aa;text-align: left;'>" + rowQ["ItemPlant"].ToString() +
                                    " QTY</th><td style='text-align: right;'>" + rowQ["orderqty"].ToString() + "</td></tr>";
                            }
                            strQtyTable += "<tr><th style='background-color: #f1e8aa;text-align: left;'>TOT QTY</th><td style='text-align: right;'>" +
                                dsType.Tables[1].Compute("Sum(orderqty)", "").ToString() + "</td></tr></table>";
                            lblQtyDetails.Text = strQtyTable;
                            hdnTotOrderQty.Value = dsType.Tables[1].Compute("Sum(orderqty)", "").ToString();
                        }
                        tb_Shipmethod.Style.Add("display", "block");
                    }
                }

                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@ID", hdnPID.Value) };
                DataTable dtLoadqty = new DataTable();
                if (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() == lblFinalLoading.Text && Utilities.Decrypt(Request["fid"].ToString()).ToLower() == "e" &&
                        lblShipType.Text == "COMBI")
                    dtLoadqty = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanMasterData_Loading_combi", sp2, DataAccess.Return_Type.DataTable);
                else
                    dtLoadqty = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanMasterData_Loading", sp2, DataAccess.Return_Type.DataTable);

                if (dtLoadqty.Rows[0]["LoadQty"].ToString() == hdnTotOrderQty.Value)
                    tb_LoadDetails.Style.Add("display", "block");
                else
                    lblErr.Text = "ANOTHER PLANT PDI NOT COMPLETED ";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSavePdiLoadStatus_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@containerno", txtContainerNo.Text) ,
                    new SqlParameter("@vehicleno", txtVehicleNo.Text) ,
                    new SqlParameter("@loadedremarks", txtRemarks.Text.Replace("\r\n","~")) ,
                    new SqlParameter("@loadedby", Request.Cookies["TTSUser"].Value) ,
                    new SqlParameter("@ID", hdnPID.Value) 
                };
                if (btnSavePdiLoadStatus.Text == "DISPATCH TO CUSTOMER")
                    resp = daCOTS.ExecuteNonQuery_SP("sp_upd_pdiscan_loaded", sp);
                else
                    resp = daCOTS.ExecuteNonQuery_SP("sp_upd_pdiscan_loaded_TransferToPlant", sp);

                if (resp > 0)
                {
                    Build_PDIReport();
                    resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(Utilities.Decrypt(Request["id"].ToString())),
                        btnSavePdiLoadStatus.Text == "DISPATCH TO CUSTOMER" ? "38" : "40", txtContainerNo.Text + "~" + txtVehicleNo.Text + "~" +
                        txtRemarks.Text, Request.Cookies["TTSUser"].Value);
                    if (resp > 0)
                        Response.Redirect("cotspdiinspect.aspx?pid=" + Utilities.Encrypt("sltl") + "&fid=" + Utilities.Encrypt("e"), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_PDIReport()
        {
            try
            {
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/invoicefiles/" + hdnCustCode.Value + "/"))
                    Directory.CreateDirectory(serverURL + "/invoicefiles/" + hdnCustCode.Value + "/");
                string sPathToWritePdfTo = serverURL + "/invoicefiles/" + hdnCustCode.Value + "/PDIList_" + hdnPID.Value + "_" +
                    Utilities.Decrypt(Request["pid"].ToString()) + ".pdf";

                Exp_PDI_Report.CustomerName = lblCustomer.Text;
                Exp_PDI_Report.OrderNumber = txt_OrderRefNo.Text;
                Exp_PDI_Report.OrderQuantity = txt_OrderQty.Text;
                Exp_PDI_Report.InspectedBy = Request.Cookies["TTSUser"].Value;
                Exp_PDI_Report.VerifiedBy = Request.Cookies["TTSUser"].Value;
                Exp_PDI_Report.ApprovedBy = Request.Cookies["TTSUser"].Value;

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@ID", hdnPID.Value) };
                DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanList", sp1, DataAccess.Return_Type.DataTable);
                Exp_PDI_Report.dtScanList = dtData;

                Exp_PDI_Report.PDI_Generation(sPathToWritePdfTo);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPdiUploadCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("cotspdiinspect.aspx?pid=" + Utilities.Encrypt("sltl") + "&fid=" + Utilities.Encrypt("e"), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkWorkOrder_CLick(object sender, EventArgs e)
        {
            try
            {
                string linkUrl = (Server.MapPath("~/workorderfiles/" + hdnCustCode.Value + "/") + lnkWorkOrder.Text).Replace("TTS", "pdfs");
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkWorkOrder.Text);
                Response.WriteFile(linkUrl);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}