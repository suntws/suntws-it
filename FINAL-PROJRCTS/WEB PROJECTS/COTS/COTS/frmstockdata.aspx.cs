using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace COTS
{
    public partial class frmstockdata : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        Bind_GV_All_Grade("Solid", gvSolidStock);
                        Bind_GV_All_Grade("Pob", gvPobStock);

                        if (gvSolidStock.Rows.Count == 0 && gvPobStock.Rows.Count == 0)
                            lblErrMsg.Text = "STOCK DATA NOT AVAILABLE";
                    }
                }
                else
                    Response.Redirect("SessionExp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_GV_All_Grade(string _category, GridView ctrl_GV)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@custstdcode", Session["cotsstdcode"].ToString()), 
                    new SqlParameter("@category", _category) 
                };
                DataTable dtSheet = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_StockData_Export", sp1, DataAccess.Return_Type.DataTable);
                if (dtSheet.Rows.Count > 0)
                {
                    if (_category == "Solid")
                    {
                        lblSolid.Text = "SOLID LIST";
                        gvSolidList.DataSource = dtSheet;
                        gvSolidList.DataBind();
                    }
                    else if (_category == "Pob")
                    {
                        lblPobList.Text = "POB LIST";
                        gvPobList.DataSource = dtSheet;
                        gvPobList.DataBind();
                    }

                    DataTable dtMain = new DataTable();
                    dtMain.Columns.Add("TyreSize", typeof(System.String));
                    dtMain.Columns.Add("RimSize", typeof(System.String));

                    DataView dtTypeView = new DataView(dtSheet);
                    dtTypeView.Sort = "TypePosition ASC,TyreType ASC,Brand ASC,Sidewall ASC,Config ASC";
                    DataTable distinctType = dtTypeView.ToTable(true, "TypePosition", "TyreType", "Brand", "Sidewall", "Config");
                    foreach (DataRow rowType in distinctType.Rows)
                    {
                        DataColumn dmaincol = new DataColumn(rowType["Config"].ToString().Replace(" ", "~") + "_" + rowType["Brand"].ToString().Replace(" ", "~") +
                            "_" + rowType["Sidewall"].ToString().Replace(" ", "~") + "_" + rowType["TyreType"].ToString().Replace(" ", "~"), typeof(System.String));
                        dtMain.Columns.Add(dmaincol);
                    }

                    DataView dtSizeView = new DataView(dtSheet);
                    dtSizeView.Sort = "SizePosition ASC";
                    DataTable distinctSize = dtSizeView.ToTable(true, "SizePosition", "TyreSize", "RimSize");
                    foreach (DataRow row in distinctSize.Rows)
                    {
                        DataRow mainRow = dtMain.NewRow();
                        foreach (DataColumn priceCol in dtMain.Columns)
                        {
                            string strHead = priceCol.ColumnName;
                            if (strHead == "TyreSize")
                                mainRow["TyreSize"] = row["TyreSize"].ToString();
                            else if (strHead == "RimSize")
                                mainRow["RimSize"] = row["RimSize"].ToString();
                            else
                            {
                                string[] splitHead = strHead.Replace('~', ' ').Split('_');
                                string unitprice = "0";
                                foreach (DataRow priceRow in dtSheet.Select("Config='" + splitHead[0].ToString() + "' and TyreSize='" + row["TyreSize"].ToString() +
                                    "' and RimSize='" + row["RimSize"].ToString() + "' and TyreType='" + splitHead[3].ToString() + "' and Brand='" + splitHead[1].ToString() +
                                    "' and Sidewall='" + splitHead[2].ToString() + "'"))
                                {
                                    unitprice = (Convert.ToDecimal(priceRow["StockCount"])).ToString();
                                }
                                mainRow["" + priceCol.ColumnName + ""] = Convert.ToDecimal(unitprice).ToString();
                            }
                        }
                        dtMain.Rows.Add(mainRow);
                    }

                    foreach (DataColumn col in dtMain.Columns)
                    {
                        TemplateField bfield = new TemplateField();
                        bfield.HeaderTemplate = new GridViewAllTemplate(ListItemType.Header, col.ColumnName, "STOCK");
                        bfield.ItemTemplate = new GridViewAllTemplate(ListItemType.Item, col.ColumnName, "STOCK");
                        ctrl_GV.Columns.Add(bfield);
                    }
                    ctrl_GV.DataSource = dtMain;
                    ctrl_GV.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvSolidStock_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                    create_Each_Header("Solid", gvSolidStock);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvPobStock_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                    create_Each_Header("Pob", gvPobStock);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void create_Each_Header(string strCategory, GridView ctrlGV)
        {
            try
            {
                SqlParameter[] spStock = new SqlParameter[] { 
                    new SqlParameter("@custstdcode", Session["cotsstdcode"].ToString()), 
                    new SqlParameter("@category", strCategory) 
                };
                DataTable dtSheet = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_StockData_Export", spStock, DataAccess.Return_Type.DataTable);

                DataView dtTypeView = new DataView(dtSheet);
                dtTypeView.Sort = "TypePosition ASC,TyreType ASC,Brand ASC,Sidewall ASC,Config ASC";
                DataTable distinctType = dtTypeView.ToTable(true, "TypePosition", "TyreType", "Brand", "Sidewall", "Config");

                GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                ctrlGV.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);
                GridViewRow gvHeaderRowCopy1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                ctrlGV.Controls[0].Controls.AddAt(1, gvHeaderRowCopy1);
                GridViewRow gvHeaderRowCopy2 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                ctrlGV.Controls[0].Controls.AddAt(2, gvHeaderRowCopy2);

                TableCell tcMergeProduct = new TableCell();
                tcMergeProduct.ColumnSpan = 2;
                tcMergeProduct.Attributes.Add("class", "mergecss configCss");
                gvHeaderRowCopy.Cells.AddAt(0, tcMergeProduct);

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = strCategory.ToUpper();
                tcMergeProduct.ColumnSpan = 2;
                tcMergeProduct.Attributes.Add("class", "mergecss configCss");
                gvHeaderRowCopy1.Cells.AddAt(0, tcMergeProduct);

                tcMergeProduct = new TableCell();
                tcMergeProduct.Text = "STOCK";
                tcMergeProduct.ColumnSpan = 2;
                tcMergeProduct.Attributes.Add("class", "mergecss configCss");
                gvHeaderRowCopy2.Cells.AddAt(0, tcMergeProduct);

                int j = 1;
                foreach (DataRow row in distinctType.Rows)
                {
                    TableCell tcConfigProduct = new TableCell();
                    tcConfigProduct.Text = row["Config"].ToString();
                    tcConfigProduct.Attributes.Add("class", "mergecss configCss");
                    gvHeaderRowCopy.Cells.AddAt(j, tcConfigProduct);

                    TableCell tcBrandProduct = new TableCell();
                    tcBrandProduct.Text = row["Brand"].ToString();
                    tcBrandProduct.Attributes.Add("class", "mergecss configCss");
                    gvHeaderRowCopy1.Cells.AddAt(j, tcBrandProduct);

                    TableCell tcSidewallProduct = new TableCell();
                    tcSidewallProduct.Text = row["Sidewall"].ToString();
                    tcSidewallProduct.Attributes.Add("class", "mergecss configCss");
                    gvHeaderRowCopy2.Cells.AddAt(j, tcSidewallProduct);
                    j++;
                }
                gvHeaderRowCopy.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}