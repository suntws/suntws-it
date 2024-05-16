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
using System.Data.OleDb;
using System.IO;

namespace TTS
{
    public partial class stockinward : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["ot_ppc_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sitl"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "")
                            {
                                lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + " STOCK INWARD";
                                hdnPlant.Value = Utilities.Decrypt(Request["pid"].ToString()).ToUpper();
                                btnInwardFileSave.Style.Add("display", "none");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL IS WRONG";
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
                    btnInwardFileSave.Style.Add("display", "none");
                    tb_stock_inward.Style.Add("display", "none");

                    string FileName = Path.GetFileName(fupStockInward.PostedFile.FileName);
                    string Extension = Path.GetExtension(fupStockInward.PostedFile.FileName);

                    if (Extension == ".xls" || Extension == ".xlsx" || Extension == ".csv" || Extension == ".txt")
                    {
                        string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                        if (!Directory.Exists(serverURL + "/inwardfiles/" + hdnPlant.Value + "/"))
                            Directory.CreateDirectory(serverURL + "/inwardfiles/" + hdnPlant.Value + "/");
                        string path = serverURL + "/inwardfiles/" + hdnPlant.Value + "/";

                        string FilePath = path + FileName;
                        fupStockInward.SaveAs(FilePath);

                        string strRenameFile = DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") +
                            DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + Extension;
                        FileInfo file1 = new FileInfo(serverURL + "/inwardfiles/" + hdnPlant.Value + "/" + strRenameFile);
                        if (file1.Exists)
                            file1.Delete();
                        File.Copy(FilePath, serverURL + "/inwardfiles/" + hdnPlant.Value + "/" + strRenameFile);
                        File.Delete(FilePath);

                        GetDataFromScanFile(path + strRenameFile, Extension);
                    }
                    else
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Upload file is not correct. Kindly upload only [.xls,.xlsx,.csv,.txt]');", true);
                }
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
                    Check_ScanBased_StockData(dt);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Check_ScanBased_StockData(DataTable dtScanData)
        {
            try
            {
                btnInwardFileSave.Style.Add("display", "none");
                tb_stock_inward.Style.Add("display", "none");

                gvItemCount.DataSource = null;
                gvItemCount.DataBind();
                gvItemList.DataSource = null;
                gvItemList.DataBind();
                gvNotMatchedList.DataSource = null;
                gvNotMatchedList.DataBind();

                SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@ProcessID_DT", dtScanData) };
                DataSet dsData = (DataSet)daTTS.ExecuteReader_SP("sp_sel_ProcessID_Data_StockInward", spSel, DataAccess.Return_Type.DataSet);

                if (dsData.Tables.Count > 0)
                {
                    if (dsData.Tables[2].Rows.Count > 0)
                    {
                        gvNotMatchedList.DataSource = dsData.Tables[2];
                        gvNotMatchedList.DataBind();
                        Button3.Style.Add("display", "block");
                        Button3.Text = "INVALID BARCODE (" + dsData.Tables[2].Rows.Count + ")";
                    }
                    else
                        Button3.Style.Add("display", "none");

                    if (dsData.Tables[1].Rows.Count > 0)
                    {
                        gvItemList.DataSource = dsData.Tables[1];
                        gvItemList.DataBind();
                        Button2.Style.Add("display", "block");
                        Button2.Text = "VALID BARCODE (" + dsData.Tables[1].Rows.Count + ")";
                        ViewState["ValidBarcode"] = dsData.Tables[1];
                    }
                    else
                        Button2.Style.Add("display", "none");

                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        gvItemCount.DataSource = dsData.Tables[0];
                        gvItemCount.DataBind();
                        Button1.Style.Add("display", "block");
                    }
                    else
                        Button1.Style.Add("display", "none");

                    Button1.CssClass = "Initial";
                    Button2.CssClass = "Initial";
                    Button3.CssClass = "Initial";
                    if (dsData.Tables[2].Rows.Count > 0)
                    {
                        Button3.CssClass = "Clicked";
                        MultiView1.ActiveViewIndex = 2;
                    }
                    else if (dsData.Tables[1].Rows.Count > 0)
                    {
                        Button2.CssClass = "Clicked";
                        MultiView1.ActiveViewIndex = 1;
                    }
                    else if (dsData.Tables[0].Rows.Count > 0)
                    {
                        Button3.CssClass = "Clicked";
                        MultiView1.ActiveViewIndex = 0;
                    }

                    if (dsData.Tables[1].Rows.Count > 0)
                        btnInwardFileSave.Style.Add("display", "block");
                    tb_stock_inward.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void Tab_Click(object sender, EventArgs e)
        {
            try
            {
                Button1.CssClass = "Initial";
                Button2.CssClass = "Initial";
                Button3.CssClass = "Initial";
                Button lnkTxt = sender as Button;
                if (lnkTxt.ID == "Button1")
                {
                    Button1.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 0;
                }
                else if (lnkTxt.ID == "Button2")
                {
                    Button2.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 1;
                }
                else if (lnkTxt.ID == "Button3")
                {
                    Button3.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 2;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnInwardFileSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtValidBarcode = ViewState["ValidBarcode"] as DataTable;
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@Lanka_Production_DT", dtValidBarcode), 
                    new SqlParameter("@plant", hdnPlant.Value), 
                    new SqlParameter("@Username", Request.Cookies["TTSUser"].Value) 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_stockdata_Inward_Lanka", sp1);
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnInwardUploadCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}