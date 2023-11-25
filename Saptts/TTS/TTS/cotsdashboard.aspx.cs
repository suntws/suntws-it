using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class cotsdashboard : System.Web.UI.Page
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
                        BuildMainTable("sp_sel_Rpt_SegmentWise", "SEGMENT");
                        BuildMainTable("sp_sel_Rpt_RegionWise", "REGION");
                        BuildMainTable("sp_sel_Rpt_LeadWise", "LEAD");
                        BuildMainTable("sp_sel_Rpt_MonthWise", "MONTH");
                        BuildMainTable("sp_sel_Rpt_StockTransfer", "STOCK");
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
        private void BuildMainTable(string spName, string CategoryName)
        {
            try
            {
                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP(spName, DataAccess.Return_Type.DataSet);

                DataTable dt_Main = new DataTable();
                dt_Main.Columns.Add("WISE");
                //dt_Main.Columns.Add("ItemQty_ft14To15");
                //dt_Main.Columns.Add("Tonnage_ft14To15");
                dt_Main.Columns.Add("ItemQty_ft15To16");
                dt_Main.Columns.Add("Tonnage_ft15To16");
                dt_Main.Columns.Add("ItemQty_ft16To17");
                dt_Main.Columns.Add("Tonnage_ft16To17");
                dt_Main.Columns.Add("ItemQty_ft17To18");
                dt_Main.Columns.Add("Tonnage_ft17To18");
                dt_Main.Columns.Add("ItemQty_ft18To19");
                dt_Main.Columns.Add("Tonnage_ft18To19");

                if (CategoryName != "MONTH")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@FieldName", CategoryName) };
                    DataTable dtArrList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_SalesDashBoard", sp1, DataAccess.Return_Type.DataTable);
                    DataView dView = new DataView(dtArrList);
                    dView.Sort = "field ASC";
                    DataTable disVal = dView.ToTable(true, "field");
                    foreach (DataRow row in disVal.Rows)
                    {
                        dt_Main.Rows.Add(row[0].ToString());
                    }
                }
                else if (CategoryName == "MONTH")
                {
                    string[] arr = { "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER", "JANUARY", "FEBRUARY", "MARCH" };
                    for (int z = 0; z < arr.Length; z++)
                    {
                        dt_Main.Rows.Add(arr[z].ToString());
                    }
                }
                dt_Main.Rows.Add("TOTAL");

                //Build_dt_Main(ds.Tables[0], "ft14To15", dt_Main);
                Build_dt_Main(ds.Tables[1], "ft15To16", dt_Main);
                Build_dt_Main(ds.Tables[2], "ft16To17", dt_Main);
                Build_dt_Main(ds.Tables[3], "ft17To18", dt_Main);
                Build_dt_Main(ds.Tables[4], "ft18To19", dt_Main);

                dt_Main.Columns[0].ColumnName = CategoryName;

                if (CategoryName == "SEGMENT")
                {
                    gvSEGMENT.DataSource = dt_Main;
                    gvSEGMENT.DataBind();
                }
                else if (CategoryName == "REGION")
                {
                    gvREGION.DataSource = dt_Main;
                    gvREGION.DataBind();
                }
                else if (CategoryName == "LEAD")
                {
                    gvLEAD.DataSource = dt_Main;
                    gvLEAD.DataBind();
                }
                else if (CategoryName == "MONTH")
                {
                    gvMONTH.DataSource = dt_Main;
                    gvMONTH.DataBind();
                }
                else if (CategoryName == "STOCK")
                {
                    gvStock.DataSource = dt_Main;
                    gvStock.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_dt_Main(DataTable dt, string Year, DataTable dtBind)
        {
            try
            {
                string ItemQtyColumnName = "ItemQty_" + Year;
                string TonnageColumnName = "Tonnage_" + Year;
                decimal totQty = 0, totTon = 0;
                foreach (DataRow drMain1 in dtBind.Rows)
                {
                    DataRow[] row = dt.Select("wise='" + drMain1[0].ToString() + "'");
                    if (row.Length > 0)
                    {
                        totQty += Convert.ToDecimal(row[0][ItemQtyColumnName].ToString());
                        totTon += Convert.ToDecimal(row[0][TonnageColumnName].ToString()) / 1000;
                        drMain1[ItemQtyColumnName] = Convert.ToInt32(row[0][ItemQtyColumnName].ToString()).ToString("000");
                        drMain1[TonnageColumnName] = (Convert.ToDecimal(row[0][TonnageColumnName].ToString()) / 1000).ToString("00.00");
                    }
                    else
                    {
                        drMain1[ItemQtyColumnName] = "000";
                        drMain1[TonnageColumnName] = "00.00";
                    }
                }
                dtBind.Rows[dtBind.Rows.Count - 1][ItemQtyColumnName] = Convert.ToInt32(totQty).ToString("000");
                dtBind.Rows[dtBind.Rows.Count - 1][TonnageColumnName] = Convert.ToDecimal(totTon).ToString("00.00");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvtemp_DataBound(object sender, EventArgs e)
        {
            try
            {
                DateTime ss = DateTime.Now;
                int year = (ss.Year) % 100;

                GridView gvName = sender as GridView;
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                row.Style.Add("background-color", "#adccf3");
                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "FY-" + (year - 3) + "-" + (year - 2);
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "FY-" + (year - 2) + "-" + (year - 1);
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "FY-" + (year - 1) + "-" + (year);
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "FY-" + (year) + "-" + (year + 1);
                row.Controls.Add(cell);

                gvName.HeaderRow.Parent.Controls.AddAt(0, row);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}