using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.IO;
using System.Data.OleDb;
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Globalization;

namespace TTS
{
    public partial class PriceSheetCreation : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DataTable dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_export_fullname", DataAccess.Return_Type.DataTable);
                if (dtCustList.Rows.Count > 0)
                {
                    ddlCustomer.DataSource = dtCustList;
                    ddlCustomer.DataTextField = "custfullname";
                    ddlCustomer.DataValueField = "custcode";
                    ddlCustomer.DataBind();
                    ddlCustomer.Items.Insert(0, "CHOOSE");
                }
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_Category.DataSource = "";
            ddl_Category.DataBind();

            DataTable dt = (DataTable)daTTS.ExecuteReader("select distinct Sizecategory from ApprovedTyreList where custcode='" + ddlCustomer.SelectedItem.Value
                    + "' order by SizeCategory", DataAccess.Return_Type.DataTable);
            if (dt.Rows.Count > 0)
            {
                ddl_Category.DataSource = dt;
                ddl_Category.DataTextField = "Sizecategory";
                ddl_Category.DataValueField = "Sizecategory";
                ddl_Category.DataBind();
                if (dt.Rows.Count == 1)
                    ddl_Category_SelectedIndexChanged(sender, e);
                else
                    ddl_Category.Items.Insert(0, "CHOOSE");
            }
        }



        

        protected void ddl_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_Platform.DataSource = "";
            ddl_Platform.DataBind();

            DataTable dt = (DataTable)daTTS.ExecuteReader("select distinct Config from ApprovedTyreList where CustCode='" + ddlCustomer.SelectedItem.Value + "' and " +
                    "SizeCategory='" + ddl_Category.SelectedItem.Text + "' order by Config", DataAccess.Return_Type.DataTable);

            if (dt.Rows.Count > 0)
            {
                ddl_Platform.DataSource = dt;
                ddl_Platform.DataTextField = "Config";
                ddl_Platform.DataValueField = "Config";
                ddl_Platform.DataBind();
                if (dt.Rows.Count == 1)
                    ddl_Platform_SelectedIndexChanged(sender, e);
                else
                    ddl_Platform.Items.Insert(0, "CHOOSE");
            }
        }

        protected void ddl_Platform_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




        

        protected void btnXLUPLD_Click(object sender, EventArgs e)
        {

       
            DataTable dt = new DataTable();//We have to read from excel and store in datatable. 
            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(FileUpload1.PostedFile.InputStream))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);
                //string readRange = "1:1"; 

                //Create a new DataTable.
                

                //Loop through the Worksheet rows.
                bool firstRow = true;
                string readRange = "1:1";
                int count = (workSheet.RowsUsed().Count())-2;

                //int count;
                //count = ;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        if (count > (dt.Rows.Count - 1) )//Add rows to DataTable.
                        {
                            dt.Rows.Add();
                            int i = 0;
                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                i++;
                            }
                        }
                    }


                }




            }



            //SqlParameter[] sp1 = new SqlParameter[]
            //{
            //        new SqlParameter("@datatable", dt)
            //        //new SqlParameter("@UT_PRICESHEET_DATATABLE_V1",dt)
                   
            //};

            //int resp = daCOTS.ExecuteNonQuery_SP("sp_insert_Testing_ExportPrice", sp1);
            //if (resp > 0)
            {

                SqlParameter[] sp2 = new SqlParameter[] { 
                      new SqlParameter("@PriceSheet_RefNo", txtPriceSheetRefNo.Text), 
                      //new SqlParameter("@Config", ddl_Platform.SelectedItem.Text), 
                      new SqlParameter("@Category", ddl_Category.SelectedItem.Text), 
                      new SqlParameter("@Custcode", ddlCustomer.SelectedItem.Value), 
                      new SqlParameter("@UserName", HttpContext.Current.Request.Cookies["TTSUser"].Value),
                      new SqlParameter("@Rates_ID", txtRatesId.Text),
                      new SqlParameter("@End_Date", txtEndDate.Text) 
                };

                int respp = daCOTS.ExecuteNonQuery_SP("sp_ins_PriceSheet_Master", sp2);
                    if (respp > 0)
                    {

                        //********************************************************************************
                        
                               
                         for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            SqlParameter[] sp3 = new SqlParameter[]
                             {
                                new SqlParameter("@Custcode", ddlCustomer.SelectedItem.Value),                                
                                new SqlParameter("@Tyresize", dt.Rows[i][0].ToString()),                                                            
                                new SqlParameter("@Tyrerim",   String.Format("{0:#,###,###.##}", dt.Rows[i][1].ToString())),
                                new SqlParameter("@Tyretype", dt.Rows[i][2].ToString().Trim()),
                                new SqlParameter("@Unitprice", dt.Rows[i][3].ToString()),                               
                                new SqlParameter("@Config", ddl_Platform.SelectedItem.Text.Trim()), 
                                new SqlParameter("@Category", ddl_Category.SelectedItem.Text.Trim()),
                                new SqlParameter("@PriceSheet_RefNo", txtPriceSheetRefNo.Text),
                                new SqlParameter("@Rates_ID", txtRatesId.Text),
                                new SqlParameter("@End_Date", txtEndDate.Text)
                             };
                             daCOTS.ExecuteNonQuery_SP("sp_ins_PriceSheet_Details_V1", sp3);
                        }

                        //********************************************************************************
                                                      

                    
            }
            }
                
            
            
            
            }

        }


       




    }


   







   
   

        


    
    
