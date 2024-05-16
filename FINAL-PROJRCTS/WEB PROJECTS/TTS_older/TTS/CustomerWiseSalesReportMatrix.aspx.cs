using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace TTS
{
    public partial class CustomerWiseSalesReportMatrix : System.Web.UI.Page
    {
        DataAccess daCots = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
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
                            if (Request["Year"] != null && Request["grade"].ToString() != "" && Request["custname"] != null)
                            {
                                lblErr.Text = "";
                                hdnCategory.Value = Request["Category"].ToString();
                                hdnYear.Value = Request["Year"].ToString();
                                hdngrade.Value = Request["grade"] != null ? (Request["grade"].ToString() != "" ? Request["grade"].ToString() : "") : "";
                                hdncustname.Value = Request["custname"] != null ? (Request["custname"].ToString() != "" ? Request["custname"].ToString() : "") : "";
                                bindSalesGridView();
                                bindStockGridView();
                            }
                            else
                                lblErr.Text = "You have entered wrong URL";
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
        private void bindSalesGridView()
        {
            try
            {
                DataTable dtMain = new DataTable();
                DataTable dtCust = new DataTable();
                lnkSalesExport.Text = "";
                string CustomerName = hdncustname.Value;
                string res = CustomerName.Replace(",", ";");
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@Duration", hdnYear.Value), 
                    new SqlParameter("@CustomerName", ";" + res + ";"), 
                    new SqlParameter("@grade", hdngrade.Value.Length > 0 ? hdngrade.Value : string.Empty) 
                };
                DataTable dtSalesData = (DataTable)daCots.ExecuteReader_SP("SP_LST_CustomerWiseSalesData", sp, DataAccess.Return_Type.DataTable);
                if (dtSalesData.Rows.Count > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@category", hdnCategory.Value) };
                    DataTable dtTyreSize = (DataTable)daTTS.ExecuteReader_SP("SP_SEL_Tyresize_Matrix", sp1, DataAccess.Return_Type.DataTable);

                    DataView view = new DataView(dtSalesData);
                    dtCust = view.ToTable(true, "CustomerName");
                    for (int i = 0; i < (dtCust.Rows.Count * 12) + 3; i++)
                    {
                        DataColumn dc = new DataColumn();
                        dtMain.Columns.Add(dc);
                    }
                    for (int k = 0; k < dtTyreSize.Rows.Count + 3; k++)
                    {
                        DataRow dr = dtMain.NewRow();
                        dtMain.Rows.Add(dr);
                    }
                    for (int i = 0, m = 0, j = 2; i < dtCust.Rows.Count; i++)
                    {
                        if (i >= 2)
                            m = m - 1;
                        dtMain.Rows[0][1] = "CUSTOMER";
                        dtMain.Rows[1][1] = "GRADE";
                        dtMain.Rows[2][1] = "RIM SIZE";
                        dtMain.Rows[1][2] = hdngrade.Value;
                        dtMain.Rows[0][i + j + m] = dtCust.Rows[i][0].ToString();
                        j += 12;
                    }
                    for (int i = 0; i < dtTyreSize.Rows.Count; i++)
                    {
                        dtMain.Rows[2][0] = "TYRE SIZE";
                        dtMain.Rows[i + 3][1] = dtTyreSize.Rows[i][1].ToString();
                        dtMain.Rows[i + 3][0] = dtTyreSize.Rows[i][0].ToString();
                    }
                    for (int j = 0, k = 0; j < dtCust.Rows.Count; j++)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            dtMain.Rows[2][2] = "MONTH";
                            dtMain.Rows[2][i + 2 + k] = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(i).ToString();
                        }
                        k += 12;
                    }
                    for (int m = 0, MonthName = 0, CompName = 0; m < dtCust.Rows.Count; m++)
                    {
                        for (int i = 0; i < dtTyreSize.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtSalesData.Rows.Count; j++)
                            {
                                if (dtMain.Rows[i + 3][0].ToString() == dtSalesData.Rows[j][1].ToString() && dtMain.Rows[0][2 + CompName].ToString() == dtSalesData.Rows[j][3].ToString() && dtMain.Rows[3 + i][1].ToString() == dtSalesData.Rows[j][2].ToString())
                                {
                                    int month = Convert.ToInt32(dtSalesData.Rows[j][6].ToString());
                                    dtMain.Rows[i + 3][month + 2 + MonthName] = dtSalesData.Rows[j][4];
                                }
                            }
                        }
                        MonthName += 12;
                        CompName += 13;
                        if (m >= 1)
                        {
                            MonthName -= 1;
                            CompName -= 1;
                        }
                    }
                    ViewState["dtCust"] = dtCust;
                    ViewState["dtMain"] = dtMain;
                    gvSalesExportData.DataSource = dtMain;
                    gvSalesExportData.DataBind();
                    lnkSalesExport.Text = "Sales Data Export To Excel";
                }
                else
                    lblErr.Text = "NO RECORDS FOUND!!!";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvSalesExportData_DataBound(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCust = (DataTable)ViewState["dtCust"];
                int RowCount = dtCust.Rows.Count;

                DataTable dtMain = (DataTable)ViewState["dtMain"];
                int ColCount = dtMain.Columns.Count;
                GridViewRow gvRow = gvSalesExportData.Rows[0];
                for (int j = 0, r = 13; j + 2 < ColCount; j++)
                {
                    if (j != 0)
                    {
                        r = 12;
                    }
                    if (j >= 26)
                        j -= 1;

                    gvRow.Cells[2 + j].ColumnSpan = r;
                    j = j + 12;
                }
                for (int j = 0, k = 0, m = 14; j < RowCount; j++)
                {
                    if (j >= 2)
                    {
                        k -= 1;
                    }
                    for (int i = k + 3; i <= m; i++)
                    {

                        gvRow.Cells[i].Visible = false;
                    }
                    k = k + 13;
                    m = m + 12;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvSalesExportData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header) e.Row.Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Width = 200;
                    e.Row.Cells[1].Width = 80;
                    if (e.Row.RowIndex < 3)
                    {
                        e.Row.ControlStyle.Font.Bold = true;
                        e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        if (e.Row.RowIndex < 2) e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        for (int i = 2; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        }
                    }
                    if (e.Row.RowIndex >= 3)
                    {
                        e.Row.Cells[1].CssClass += "align-right";
                        if (e.Row.RowIndex > 2)
                        {
                            e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                            e.Row.Cells[1].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvsalesExportData_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "GVModel", "gvSalesModel();", true);
        }
        protected void lnkSalesExport_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "SALES-REPORT-" + System.DateTime.Today.ToString("yyyyMMMdd") + "-" +
                    System.DateTime.Now.Hour.ToString("00") + System.DateTime.Now.Minute.ToString("00") + System.DateTime.Now.Second.ToString("00")));
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvSalesExportData.AllowPaging = false;
                    this.bindSalesGridView();
                    gvSalesExportData.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                gvSalesExportData.AllowPaging = true;
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        private void bindStockGridView()
        {
            try
            {
                DataTable dtMainList = new DataTable();
                SqlParameter[] spQty = new SqlParameter[] { new SqlParameter("@tyretype", hdngrade.Value) };
                DataTable dtStockQty = (DataTable)daCots.ExecuteReader_SP("sp_sel_stockQty", spQty, DataAccess.Return_Type.DataTable);
                if (dtStockQty.Rows.Count > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@category", hdnCategory.Value) };
                    DataTable dtSize = (DataTable)daTTS.ExecuteReader_SP("SP_SEL_Tyresize_Matrix", sp1, DataAccess.Return_Type.DataTable);

                    for (int i = 0; i < 2; i++)
                    {
                        DataColumn dc = new DataColumn();
                        dtMainList.Columns.Add(dc);
                    }
                    for (int k = 0; k < dtSize.Rows.Count + 3; k++)
                    {
                        DataRow dr = dtMainList.NewRow();
                        dtMainList.Rows.Add(dr);
                    }
                    dtMainList.Rows[0][0] = hdngrade.Value;
                    dtMainList.Rows[0][1] = "PLANT";
                    dtMainList.Rows[1][1] = "PLATFORM";
                    dtMainList.Rows[2][1] = "BRAND";
                    dtMainList.Rows[2][0] = "TYRE SIZE";

                    for (int i = 0; i < dtSize.Rows.Count; i++)
                    {
                        dtMainList.Rows[i + 3][0] = dtSize.Rows[i][0].ToString();
                        dtMainList.Rows[i + 3][1] = dtSize.Rows[i][1].ToString();
                    }

                    DataView view = new DataView(dtStockQty);
                    DataTable dtPlant = view.ToTable(true, "Plant");

                    DataTable dtPlatform = new DataTable();
                    DataTable dtBrand = new DataTable();

                    int z = 2;
                    foreach (DataRow rowPlant in dtPlant.Rows)
                    {
                        dtPlatform = new DataTable();
                        DataColumn dP = new DataColumn("config", typeof(System.String));
                        dtPlatform.Columns.Add(dP);

                        DataTable dtDistinctP = view.ToTable(true, "config", "Plant");
                        foreach (DataRow rowP in dtDistinctP.Select("Plant='" + rowPlant["Plant"].ToString() + "'"))
                        {
                            dtPlatform.Rows.Add(rowP["config"].ToString());
                        }
                        foreach (DataRow rowPlatform in dtPlatform.Rows)
                        {
                            dtBrand = new DataTable();
                            DataColumn dB = new DataColumn("brand", typeof(System.String));
                            dtBrand.Columns.Add(dB);

                            DataTable dtDistinctB = view.ToTable(true, "Brand", "config", "Plant");
                            foreach (DataRow rowP in dtDistinctB.Select("Plant='" + rowPlant["Plant"].ToString() + "' and config='" + rowPlatform["config"].ToString() + "'"))
                            {
                                dtBrand.Rows.Add(rowP["brand"].ToString());
                            }
                            foreach (DataRow rowBrand in dtBrand.Rows)
                            {
                                DataColumn dc = new DataColumn();
                                dtMainList.Columns.Add(dc);

                                dtMainList.Rows[0][z] = rowPlant["Plant"].ToString();
                                dtMainList.Rows[1][z] = rowPlatform["Config"].ToString();
                                dtMainList.Rows[2][z] = rowBrand["Brand"].ToString();
                                for (int i = 0; i < dtSize.Rows.Count; i++)
                                {
                                    foreach (DataRow rowQty in dtStockQty.Select("Plant='" + rowPlant["Plant"].ToString() + "' and config='" + rowPlatform["config"].ToString()
                                        + "' and Brand='" + rowBrand["brand"].ToString() + "' and tyresize='" + dtMainList.Rows[i + 3][0].ToString() + "' and rimsize='" +
                                        dtMainList.Rows[i + 3][1].ToString() + "'"))
                                    {
                                        dtMainList.Rows[i + 3][z] = rowQty["qty"].ToString();
                                    }
                                }
                                z++;
                            }
                        }
                    }
                    if (dtMainList.Rows.Count > 0)
                    {
                        gvStockExportData.DataSource = dtMainList;
                        gvStockExportData.DataBind();
                        lnkStockExport.Text = "Stock Data Export To Excel";
                    }
                }
                else
                    lblErr.Text += "NO RECORDS FOUND IN STOCK!!!";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvStockExportData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header) e.Row.Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Width = 200;
                    e.Row.Cells[1].Width = 80;
                    if (e.Row.RowIndex < 3)
                    {
                        e.Row.ControlStyle.Font.Bold = true;
                        e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        if (e.Row.RowIndex < 2) e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        for (int i = 2; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        }
                    }
                    if (e.Row.RowIndex >= 3)
                    {
                        e.Row.Cells[1].CssClass += "align-right";
                        if (e.Row.RowIndex > 2)
                        {
                            e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                            e.Row.Cells[1].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        }
                    }
                    if (e.Row.RowIndex == 0 || e.Row.RowIndex == 1)
                    {
                        int colsSpan = 2;
                        for (int i = e.Row.Cells.Count; i >= 2; i--)
                        {
                            if (e.Row.Cells[i - 1].Text == e.Row.Cells[i - 2].Text)
                            {
                                e.Row.Cells[i - 2].ColumnSpan = colsSpan;
                                e.Row.Cells[i - 1].Visible = false;
                                colsSpan += 1;
                            }
                            else
                                colsSpan = 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvStockExportData_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "GVStockModel", "gvStockModel();", true);
        }
        protected void lnkStockExport_Click(object sedner, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "STOCK-REPORT-" + System.DateTime.Today.ToString("yyyyMMMdd") + "-" +
                    System.DateTime.Now.Hour.ToString("00") + System.DateTime.Now.Minute.ToString("00") + System.DateTime.Now.Second.ToString("00")));
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvStockExportData.AllowPaging = false;
                    this.bindStockGridView();
                    gvStockExportData.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                gvStockExportData.AllowPaging = true;
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}

