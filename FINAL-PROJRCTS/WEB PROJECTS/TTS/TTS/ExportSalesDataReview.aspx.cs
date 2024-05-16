using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TTS
{
    public partial class ExportSalesDataReview : System.Web.UI.Page
    {
        DataAccess daCots = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
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
                            if (Request["duration"] != null && Request["duration"].ToString() != "" && Request["config"] != null && Request["config"].ToString() != "")
                            {
                                lblErr.Text = "";
                                hdnconfig.Value = Request["config"].ToString();
                                hdnduration.Value = Request["duration"].ToString();
                                hdngrade.Value = Request["grade"] != null ? (Request["grade"].ToString() != "" ? Request["grade"].ToString() : "") : "";
                                hdncustname.Value = Request["custname"] != null ? (Request["custname"].ToString() != "" ? Request["custname"].ToString() : "") : "";
                                bindGridView();
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

        private void bindGridView()
        {
            try
            {
                lnkExport.Text = "";
                string message = "";
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@Duration",hdnduration.Value),
                    new SqlParameter("@CustomerName",hdncustname.Value.Length > 0 ? hdncustname.Value : string.Empty),
                    new SqlParameter("@platform",hdnconfig.Value.Length > 0 ?  hdnconfig.Value : string.Empty),
                    new SqlParameter("@grade",hdngrade.Value.Length > 0 ? hdngrade.Value : string.Empty),
                    new SqlParameter("@ResultQuery",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message)
                };
                DataTable dtSalesData = (DataTable)daCots.ExecuteReader_SP("SP_LST_ExportSalesData_SalesData", sp, DataAccess.Return_Type.DataTable);
                if (dtSalesData.Rows.Count > 0)
                {
                    SqlParameter[] spPBG = new SqlParameter[]{
                        new SqlParameter("@Duration",Request["Duration"].ToString()),
                        new SqlParameter("@CustomerName",hdncustname.Value.Length > 0 ? hdncustname.Value : string.Empty),
                        new SqlParameter("@platform",hdnconfig.Value.Length > 0 ?  hdnconfig.Value : string.Empty),
                        new SqlParameter("@grade",hdngrade.Value.Length > 0 ? hdngrade.Value : string.Empty)
                    };
                    DataTable dtPBG = (DataTable)daCots.ExecuteReader_SP("SP_GET_ExportSalesData_PBG", spPBG, DataAccess.Return_Type.DataTable);

                    SqlParameter[] spSR = new SqlParameter[]{
                        new SqlParameter("@Duration",Request["Duration"].ToString()),
                        new SqlParameter("@CustomerName",hdncustname.Value.Length > 0 ? hdncustname.Value : string.Empty),
                        new SqlParameter("@platform",hdnconfig.Value.Length > 0 ?  hdnconfig.Value : string.Empty),
                        new SqlParameter("@grade",hdngrade.Value.Length > 0 ? hdngrade.Value : string.Empty)
                    };
                    DataTable dtSR = (DataTable)daCots.ExecuteReader_SP("SP_GET_ExportSalesData_SR", spSR, DataAccess.Return_Type.DataTable);

                    if (dtPBG.Rows.Count <= 0 || dtSR.Rows.Count <= 0) return;

                    var arrPBG = dtPBG.AsEnumerable().ToArray();
                    var arrSR = dtSR.AsEnumerable().ToArray();
                    DataTable dtMain = new DataTable();

                    if (dtPBG.Rows.Count > 0)
                    {
                        int noOfSR_Col = ((DataRow)arrSR[0]).Table.Columns.Count;
                        int noOfPBG_Col = ((DataRow)arrPBG[0]).Table.Columns.Count;

                        for (int noOfCols = 0; noOfCols < arrPBG.Count() + noOfSR_Col + 1; noOfCols++)
                        {
                            DataColumn dc = new DataColumn();
                            dtMain.Columns.Add(dc);
                        }
                        for (int noOfRows = 0; noOfRows < noOfPBG_Col; noOfRows++)
                        {
                            DataRow dr = dtMain.NewRow();
                            dtMain.Rows.Add(dr);
                        }

                        for (int col = 0; col <= arrPBG.Count(); col++)
                        {
                            if (col == 0)
                            {
                                DataRow dr = ((DataRow)arrPBG[col]);
                                dtMain.Rows[0][col + 1] = dr.Table.Columns[0].ToString();
                                dtMain.Rows[1][col + 1] = dr.Table.Columns[1].ToString();
                                dtMain.Rows[2][col + 1] = dr.Table.Columns[2].ToString();
                            }
                            if (col == arrPBG.Count())
                            {
                                dtMain.Rows[0][col + 2] = "TOTAL";
                            }
                            else
                            {
                                DataRow dr = ((DataRow)arrPBG[col]);
                                dtMain.Rows[0][col + 2] = dr.ItemArray[0].ToString();
                                dtMain.Rows[1][col + 2] = dr.ItemArray[1].ToString();
                                dtMain.Rows[2][col + 2] = dr.ItemArray[2].ToString();
                            }
                        }

                        for (int row = noOfPBG_Col; row < dtMain.Rows.Count + 1; row++)
                        {
                            if (row < dtMain.Rows.Count + 1 && row < arrSR.Count() + noOfPBG_Col + 1)
                            {
                                DataRow dr = dtMain.NewRow();
                                dtMain.Rows.Add(dr);
                                if (row == noOfPBG_Col)
                                {
                                    DataRow dr1 = dtMain.NewRow();
                                    dtMain.Rows.Add(dr1);
                                    dtMain.Rows[row][0] = ((DataRow)arrSR[row - noOfPBG_Col]).Table.Columns[0].ToString();
                                    dtMain.Rows[row][1] = ((DataRow)arrSR[row - noOfPBG_Col]).Table.Columns[1].ToString();
                                }
                                if (row > noOfPBG_Col && row < arrSR.Count() + noOfPBG_Col + 1)
                                {
                                    dtMain.Rows[row][0] = ((DataRow)arrSR[row - noOfPBG_Col - 1]).ItemArray[0].ToString();
                                    dtMain.Rows[row][1] = ((DataRow)arrSR[row - noOfPBG_Col - 1]).ItemArray[1].ToString();
                                    for (int col = noOfSR_Col; col < dtMain.Columns.Count; col++)
                                    {
                                        if (col < dtMain.Columns.Count - 1)
                                        {
                                            string _customer = dtMain.Rows[0][col].ToString();
                                            string brand = dtMain.Rows[1][col].ToString();
                                            string _grade = dtMain.Rows[2][col].ToString();
                                            string tyresize = ((DataRow)arrSR[row - noOfPBG_Col - 1]).ItemArray[0].ToString();
                                            string rim = ((DataRow)arrSR[row - noOfPBG_Col - 1]).ItemArray[1].ToString();
                                            DataRow[] arrdr = dtSalesData.Select("PLATFORM='" + hdnconfig.Value + "' and BRAND='" + brand + "' and GRADE='" + _grade + "' and TYRESIZE='" + tyresize + "' and RIM='" + rim + "' and CUSTOMER='" + _customer + "'");
                                            if (arrdr.Length > 0)
                                            {
                                                string val = arrdr[0].ItemArray[6].ToString();
                                                dtMain.Rows[row][col] = val.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (row > noOfPBG_Col)
                                            {
                                                int count = 0;
                                                for (int _col = noOfSR_Col; _col < dtMain.Columns.Count; _col++)
                                                {
                                                    if (dtMain.Rows[row][_col].ToString() != "")
                                                        count += Convert.ToInt32(dtMain.Rows[row][_col]);
                                                }
                                                if (row < dtMain.Rows.Count) dtMain.Rows[row][dtMain.Columns.Count - 1] = count.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (row == dtMain.Rows.Count - 1)
                                {
                                    dtMain.Rows[row][0] = "TOTAL";
                                    dtMain.Rows[row][1] = "";
                                    // -1 : last column is total
                                    for (int col = noOfSR_Col; col < dtMain.Columns.Count; col++)
                                    {
                                        int count = 0;
                                        for (int _row = noOfPBG_Col + 1; _row < dtMain.Rows.Count; _row++)
                                        {
                                            if (dtMain.Rows[_row][col].ToString() != "")
                                            {
                                                count += Convert.ToInt32(dtMain.Rows[_row][col]);
                                            }
                                        }
                                        if (col == dtMain.Columns.Count - 1)
                                        {
                                            dtMain.Rows[2][0] = "TOTAL : " + count.ToString();
                                        }
                                        else
                                            dtMain.Rows[row][col] = count.ToString();
                                    }
                                }
                            }
                        }
                    }
                    gvExportData.DataSource = dtMain;
                    gvExportData.DataBind();
                    lnkExport.Text = "Export";
                }
                else
                {
                    lblErr.Text = "NO RECORDS FOUND!!!";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvExportData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header) e.Row.Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Width = 140;
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
                        e.Row.Cells[1].CssClass += " align-right";
                        if (e.Row.RowIndex > 3)
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

        protected void gvExportData_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "GVModel", "gvModel();", true);
        }

        protected void btnStockXls_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "Sales-" + hdnconfig.Value + (hdnduration.Value == "1" ? "Apr'15 - Mar'16'" : "Apr'16 - Mar'17")));
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvExportData.AllowPaging = false;
                    this.bindGridView();
                    gvExportData.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                gvExportData.AllowPaging = true;
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
    }
}