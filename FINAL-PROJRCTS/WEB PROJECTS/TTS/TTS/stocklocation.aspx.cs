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
using System.Data.OleDb;

namespace TTS
{
    public partial class stocklocation : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        private static DataTable dtStockDetails = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["stock_report"].ToString() == "True")
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "")
                            {
                                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()) };
                                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Jathagam_Earmark", sp1, DataAccess.Return_Type.DataSet);
                                if (ds.Tables.Count > 0)
                                {
                                    Utilities.ddl_Binding(ddl_Config, ds.Tables[0], "config", "config", "Choose");
                                    Utilities.ddl_Binding(ddl_TyreSize, ds.Tables[1], "tyresize", "tyresize", "Choose");
                                    Utilities.ddl_Binding(ddl_RimSize, ds.Tables[2], "rimsize", "rimsize", "Choose");
                                    Utilities.ddl_Binding(ddl_TyreType, ds.Tables[3], "tyretype", "tyretype", "Choose");
                                    Utilities.ddl_Binding(ddl_Brand, ds.Tables[4], "brand", "brand", "Choose");
                                    Utilities.ddl_Binding(ddl_Sidewall, ds.Tables[5], "sidewall", "sidewall", "Choose");
                                    dtStockDetails = ds.Tables[6].Copy();
                                }
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_GV()
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "HideLocationCntrls", "document.getElementById('div_Warehouse_Location').style.display = 'none';", true);
                lblTotalRecords.Text = "";
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                DataTable dt = new DataTable();
                string strConfig = ddl_Config.SelectedItem.Text != "Choose" ? ddl_Config.SelectedItem.Text : "";
                string strTyreSize = ddl_TyreSize.SelectedItem.Text != "Choose" ? ddl_TyreSize.SelectedItem.Text : "";
                string strRimSize = ddl_RimSize.SelectedItem.Text != "Choose" ? ddl_RimSize.SelectedItem.Text : "";
                string strTyreType = ddl_TyreType.SelectedItem.Text != "Choose" ? ddl_TyreType.SelectedItem.Text : "";
                string strBrand = ddl_Brand.SelectedItem.Text != "Choose" ? ddl_Brand.SelectedItem.Text : "";
                string strSidewall = ddl_Sidewall.SelectedItem.Text != "Choose" ? ddl_Sidewall.SelectedItem.Text : "";
                string query = "";
                if (strConfig != "")
                    query += "config='" + strConfig + "' and ";
                if (strTyreSize != "")
                    query += "tyresize='" + strTyreSize + "' and ";
                if (strRimSize != "")
                    query += "rimsize='" + strRimSize + "' and ";
                if (strTyreType != "")
                    query += "tyretype='" + strTyreType + "' and ";
                if (strBrand != "")
                    query += "brand='" + strBrand + "' and ";
                if (strSidewall != "")
                    query += "sidewall='" + strSidewall + "' and ";
                if (query != "")
                    query = query.Remove(query.Length - 4, 4);
                if (dtStockDetails.Select(query).Length > 0)
                    dt = dtStockDetails.Select(query).CopyToDataTable();
                if (dt.Rows.Count > 0)
                {
                    gvStockDetails.DataSource = dt;
                    gvStockDetails.DataBind();
                    lblTotalRecords.Text = "AS PER FILTER BASED STOCK COUNT : " + dt.Rows.Count;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowLocationCntrls", "document.getElementById('div_Warehouse_Location').style.display = 'block';", true);
                }
                else
                {
                    gvStockDetails.DataSource = null;
                    gvStockDetails.DataBind();
                    lblTotalRecords.Text = "No Records";
                }
            }
            catch (Exception ex)
            {
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                lblTotalRecords.Text = "No Records";
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddl_Config_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GV();
        }

        protected void ddl_TyreSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GV();
        }

        protected void ddl_RimSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GV();
        }

        protected void ddl_TyreType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GV();
        }

        protected void ddl_Brand_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GV();
        }

        protected void ddl_Sidewall_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_GV();
        }

        protected void btnSaveRecords_Click(object sender, EventArgs e)
        {
            try
            {
                string strBarcode = hdnSelectedBarcode.Value;
                strBarcode = strBarcode.Remove(strBarcode.Length - 1, 1);
               // string strQry = "update Production_warehouse_Data set warehouse_location='" + txtWarehouse_Location.Text + "',LocatedBy='" +//**************************07-09-2022
                string strQry = "update Production_warehouse_Data set scntyreinwh='" + txtWarehouse_Location.Text + "',LocatedBy='" +
                           Request.Cookies["TTSUser"].Value + "',LocatedOn=getdate() where barcode in(" + strBarcode + ") and Plant='" +
                           Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + "'";
                if (strQry != "")
                {
                    int resp = daCOTS.ExecuteNonQuery(strQry);
                    if (resp > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Record Saved Successfully');", true);
                        Response.Redirect(Request.RawUrl.ToString(), false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnScanLocation_Click(object sender, EventArgs e)
        {
            try
            {
                gvLocationUpdate.DataSource = null;
                gvLocationUpdate.DataBind();
                DataTable dt = new DataTable();
                hdnSelectedBarcode.Value = "";
                if (fupScanLocation.HasFile)
                {
                    string FileName = Path.GetFileName(fupScanLocation.PostedFile.FileName);
                    string Extension = Path.GetExtension(fupScanLocation.PostedFile.FileName);
                    string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                    string path = serverURL + "/locationfiles/" + Request.Cookies["TTSUser"].Value + "/";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string FilePath = path + FileName;
                    FileInfo file1 = new FileInfo(FilePath);
                    if (file1.Exists)
                        file1.Delete();
                    fupScanLocation.SaveAs(FilePath);

                    if (Extension.ToLower().CompareTo(".xls") == 0 || Extension.ToLower().CompareTo(".xlsx") == 0)
                    {
                        string conStr = "";
                        switch (Extension.ToLower())
                        {
                            case ".xls": //Excel 97-03
                                conStr = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
                                break;
                            case ".xlsx": //Excel 07
                                conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
                                break;
                        }

                        conStr = String.Format(conStr, FilePath, 0);
                        OleDbConnection connExcel = new OleDbConnection(conStr);
                        OleDbCommand cmdExcel = new OleDbCommand();
                        OleDbDataAdapter oda = new OleDbDataAdapter();
                        cmdExcel.Connection = connExcel;

                        //Get the name of First Sheet
                        connExcel.Open();
                        DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                        oda.SelectCommand = cmdExcel;
                        oda.Fill(dt);
                        connExcel.Close();
                        if (Extension.ToLower() == ".xls")
                        {
                            string strDtHead = dt.Columns[0].ColumnName;
                            dt.Rows.Add(strDtHead);
                        }
                        dt.Columns[0].ColumnName = "scanbarcode";
                        foreach (DataRow delRow in dt.Select("LEN(scanbarcode)<18"))
                        {
                            delRow.Delete();
                        }
                        dt.AcceptChanges();
                    }
                    else if (Extension.ToLower().CompareTo(".csv") == 0 || Extension.ToLower().CompareTo(".txt") == 0)
                    {
                        DataColumn col = new DataColumn("scanbarcode", typeof(System.String));
                        dt.Columns.Add(col);

                        string[] arrBarcodes = new string[] { };
                        string text = File.ReadAllText(FilePath);
                        string[] stringSeparators = new string[] { "\r\n" };
                        arrBarcodes = text.Split(stringSeparators, StringSplitOptions.None);
                        foreach (string line in arrBarcodes)
                        {
                            if (line.Length >= 18)
                                dt.Rows.Add(line);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dt = dt.DefaultView.ToTable(true, "scanbarcode");
                        foreach (DataRow dRow in dt.Rows)
                        {
                            hdnSelectedBarcode.Value += "'" + dRow["scanbarcode"].ToString() + "',";
                        }
                        string strBarcode = hdnSelectedBarcode.Value;
                        strBarcode = strBarcode.Remove(strBarcode.Length - 1, 1);
                        string strQry = "select config as [PLATFOMR],tyresize as [TYRE SIZE],rimsize as [RIM],tyretype as [TYPE],brand as [BRAND],sidewall as [SIDEWALL]," +
                                        "barcode as [BARCODE],REPLACE(CONVERT(varchar,dateofmanufacture,106),' ','-') as [DOM],warehouse_location as [LOCATION]," +
                                        "case scandispatch when 0 then 'STOCK' else 'DISPATCHED' end as [STATUS] from Production_warehouse_Data where barcode in (" + strBarcode + ")";
                        DataTable dtLoca = (DataTable)daCOTS.ExecuteReader(strQry, DataAccess.Return_Type.DataTable);
                        if (dtLoca.Rows.Count == 0)
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Alert1", "alert('No records');", true);
                        else if (dt.Rows.Count != dtLoca.Rows.Count)
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Alert2", "alert('NOT MATCHED: SCAN QTY " + dt.Rows.Count + " / STOCK QTY " + dtLoca.Rows.Count + "');", true);
                        else if (dtLoca.Select("STATUS='DISPATCHED'").Length > 0)
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Alert3", "alert('Some qty already dispatched');", true);

                        gvLocationUpdate.DataSource = dtLoca;
                        gvLocationUpdate.DataBind();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Alert", "alert('Upload file is not correct');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}