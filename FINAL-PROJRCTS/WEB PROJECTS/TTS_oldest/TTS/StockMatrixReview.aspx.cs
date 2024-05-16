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
    public partial class StockMatrixReview : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["stock_report"].ToString() == "True")
                        {
                            if (Request["config"] != null && Request["config"].ToString() != "" && Request["plant"] != null && Request["plant"].ToString() != "")
                            {
                                hdnConfig.Value = Request["config"].ToString();
                                hdnPlant.Value = Request["plant"].ToString();
                                hdnBrand.Value = Request["brand"] != null ? (Request["brand"].ToString() != "" ? Request["brand"].ToString() : "") : "";
                                hdnSidewall.Value = Request["sdw"] != null ? (Request["sdw"].ToString() != "" ? Request["sdw"].ToString() : "") : "";
                                hdnGrade.Value = Request["grade"] != null ? (Request["grade"].ToString() != "" ? Request["grade"].ToString() : "") : "";

                                string message = "";
                                SqlParameter[] sp = new SqlParameter[] { 
                                    new SqlParameter("@config", hdnConfig.Value.Trim().Length > 0 ? hdnConfig.Value : string.Empty), 
                                    new SqlParameter("@brand", hdnBrand.Value.Trim().Length > 0 ? hdnBrand.Value : string.Empty), 
                                    new SqlParameter("@sidewall", hdnSidewall.Value.Trim().Length > 0 ? hdnSidewall.Value : string.Empty), 
                                    new SqlParameter("@StockType", hdnPlant.Value.Trim().Length > 0 ? hdnPlant.Value : string.Empty), 
                                    new SqlParameter("@grade", hdnGrade.Value.Trim().Length > 0 ? hdnGrade.Value : string.Empty), 
                                    new SqlParameter("@ResultQuery", SqlDbType.VarChar, 8000, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Default, message) 
                                };
                                DataTable dtStockDetails = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_StockMatrix_StockDetails", sp, DataAccess.Return_Type.DataTable);
                                gvStockDetails.DataSource = null;
                                gvStockDetails.DataBind();

                                if (dtStockDetails.Rows.Count > 0)
                                {
                                    lblErr.Text = "";
                                    ViewState["StockDetails"] = dtStockDetails;
                                    bindGridView();
                                }
                                else
                                    lblErr.Text = "NO RECORDS FOUND!!!";
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
                string message = "";
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@config",hdnConfig.Value.Trim().Length > 0 ?  hdnConfig.Value : string.Empty),
                    new SqlParameter("@brand",hdnBrand.Value.Trim().Length > 0 ?  hdnBrand.Value : string.Empty),
                    new SqlParameter("@sidewall",hdnSidewall.Value.Trim().Length > 0 ? hdnSidewall.Value : string.Empty),
                    new SqlParameter("@StockType",hdnPlant.Value.Trim().Length > 0 ?  hdnPlant.Value: string.Empty),
                    new SqlParameter("@grade",hdnGrade.Value.Trim().Length > 0 ?  hdnGrade.Value: string.Empty),
                    new SqlParameter("@ResultQuery",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message)
                };
                DataTable dtCBST = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_StockMatrix_CBST", sp, DataAccess.Return_Type.DataTable);

                string message1 = "";
                SqlParameter[] sp1 = new SqlParameter[]{
                    new SqlParameter("@config",hdnConfig.Value.Trim().Length > 0 ?  hdnConfig.Value : string.Empty),
                    new SqlParameter("@brand",hdnBrand.Value.Trim().Length > 0 ?  hdnBrand.Value : string.Empty),
                    new SqlParameter("@sidewall",hdnSidewall.Value.Trim().Length > 0 ? hdnSidewall.Value : string.Empty),
                    new SqlParameter("@StockType",hdnPlant.Value.Trim().Length > 0 ? hdnPlant.Value : string.Empty),
                    new SqlParameter("@grade",hdnGrade.Value.Trim().Length > 0 ?  hdnGrade.Value: string.Empty),
                    new SqlParameter("@ResultQuery",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message1)
                };
                DataTable dtSR = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_StockMatrix_SR", sp1, DataAccess.Return_Type.DataTable);
                DataTable dtStockDetails = (DataTable)ViewState["StockDetails"];

                if (dtCBST.Rows.Count <= 0 || dtSR.Rows.Count <= 0) return;

                var arrCBST = dtCBST.AsEnumerable().ToArray();
                var arrSR = dtSR.AsEnumerable().ToArray();
                DataTable dtMain = new DataTable();

                if (dtCBST.Rows.Count > 0)
                {
                    int noOfSR_Col = ((DataRow)arrSR[0]).Table.Columns.Count;
                    int noOfCBST_Col = ((DataRow)arrCBST[0]).Table.Columns.Count;
                    // +1 column for total Value
                    for (int noOfCols = 0; noOfCols < arrCBST.Count() + noOfSR_Col + 1; noOfCols++)
                    {
                        DataColumn dc = new DataColumn();
                        dtMain.Columns.Add(dc);
                    }
                    for (int noOfRows = 0; noOfRows < noOfCBST_Col; noOfRows++)
                    {
                        DataRow dr = dtMain.NewRow();
                        dtMain.Rows.Add(dr);
                    }

                    for (int col = 0; col <= arrCBST.Count(); col++)
                    {
                        if (col == 0)
                        {
                            dtMain.Rows[0][col] = hdnPlant.Value;
                            dtMain.Rows[0][col + 1] = ((DataRow)arrCBST[col]).Table.Columns[0].ToString();
                            dtMain.Rows[1][col] = hdnConfig.Value;
                            dtMain.Rows[1][col + 1] = ((DataRow)arrCBST[col]).Table.Columns[1].ToString();
                            dtMain.Rows[2][col + 1] = ((DataRow)arrCBST[col]).Table.Columns[2].ToString();
                            dtMain.Rows[3][col + 1] = ((DataRow)arrCBST[col]).Table.Columns[3].ToString();
                        }
                        if (col == arrCBST.Count())
                        {
                            dtMain.Rows[0][col + 2] = "TOTAL";
                        }
                        else
                        {
                            dtMain.Rows[0][col + 2] = ((DataRow)arrCBST[col]).Table.Rows[col][0].ToString();
                            dtMain.Rows[1][col + 2] = ((DataRow)arrCBST[col]).Table.Rows[col][1].ToString();
                            dtMain.Rows[2][col + 2] = ((DataRow)arrCBST[col]).Table.Rows[col][2].ToString();
                            dtMain.Rows[3][col + 2] = ((DataRow)arrCBST[col]).Table.Rows[col][3].ToString();
                        }
                    }

                    // + 1 is for horizontal Total value 
                    for (int row = noOfCBST_Col; row < dtMain.Rows.Count + 1; row++)
                    {
                        if (row < dtMain.Rows.Count + 1 && row < arrSR.Count() + noOfCBST_Col + 1)
                        {
                            DataRow dr = dtMain.NewRow();
                            dtMain.Rows.Add(dr);
                            if (row == noOfCBST_Col)
                            {
                                DataRow dr1 = dtMain.NewRow();
                                dtMain.Rows.Add(dr1);
                                dtMain.Rows[row][0] = ((DataRow)arrSR[row - noOfCBST_Col]).Table.Columns[0].ToString();
                                dtMain.Rows[row][1] = ((DataRow)arrSR[row - noOfCBST_Col]).Table.Columns[1].ToString();
                            }
                            if (row > noOfCBST_Col && row < arrSR.Count() + noOfCBST_Col + 1)
                            {
                                dtMain.Rows[row][0] = ((DataRow)arrSR[row - noOfCBST_Col - 1]).Table.Rows[row - noOfCBST_Col - 1][0].ToString();
                                dtMain.Rows[row][1] = ((DataRow)arrSR[row - noOfCBST_Col - 1]).Table.Rows[row - noOfCBST_Col - 1][1].ToString();
                                for (int col = noOfSR_Col; col < dtMain.Columns.Count; col++)
                                {
                                    if (col < dtMain.Columns.Count - 1)
                                    {
                                        string grade = dtMain.Rows[0][col].ToString();
                                        string brand = dtMain.Rows[1][col].ToString();
                                        string sidewall = dtMain.Rows[2][col].ToString();
                                        string type = dtMain.Rows[3][col].ToString();
                                        string tyresize = ((DataRow)arrSR[row - noOfCBST_Col - 1]).ItemArray[0].ToString();
                                        string rim = ((DataRow)arrSR[row - noOfCBST_Col - 1]).ItemArray[1].ToString();
                                        DataRow[] arrdr = dtStockDetails.Select("PLATFORM='" + hdnConfig.Value + "' and BRAND='" + brand + "' and TYPE='" + type + "' and GRADE='" + grade + "' and TYRESIZE='" + tyresize + "' and RIM='" + rim + "' and SIDEWALL='" + sidewall + "'");

                                        if (arrdr.Length > 0)
                                        {
                                            int i = 0;
                                            int val = 0;
                                            do
                                            {
                                                val += Convert.ToInt32(arrdr[i].ItemArray[7]);
                                                i += 1;
                                            } while (i < arrdr.Length);
                                            dtMain.Rows[row][col] = val.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (row > noOfCBST_Col)
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
                                    for (int _row = noOfCBST_Col + 1; _row < dtMain.Rows.Count; _row++)
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
                gvStockDetails.DataSource = dtMain;
                gvStockDetails.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvStockDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header) e.Row.Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Width = 140;
                    e.Row.Cells[1].Width = 80;
                    if (e.Row.RowIndex < 4)
                    {
                        e.Row.ControlStyle.Font.Bold = true;
                        e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        if (e.Row.RowIndex < 2) e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        for (int i = 2; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        }
                    }
                    if (e.Row.RowIndex >= 4)
                    {
                        e.Row.Cells[1].CssClass += " align-right";
                        if (e.Row.RowIndex > 4)
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

        protected void gvStockDetails_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "GVModel", "gvModel();", true);
        }

        protected void btnStockXls_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "Stock-" + DateTime.Now.ToShortDateString() + ""));//"attachment;filename=" + txtStockXlsName.Text + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvStockDetails.AllowPaging = false;
                    this.bindGridView();
                    gvStockDetails.RenderControl(hw);
                    Response.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                gvStockDetails.AllowPaging = true;

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