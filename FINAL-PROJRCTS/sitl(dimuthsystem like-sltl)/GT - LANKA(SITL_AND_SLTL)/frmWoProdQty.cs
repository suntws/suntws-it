using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace GT
{
    public partial class frmWoProdQty : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dtExcel;
        public frmWoProdQty()
        {
            InitializeComponent();
        }
        private void frmWoProdQty_Load(object sender, EventArgs e)
        {
            try
            {
                this.Location = new Point(0, 0);
                DataTable dtYear = (DataTable)dba.ExecuteReader_SP("sp_sel_WO_Year", DBAccess.Return_Type.DataTable);
                if (dtYear.Rows.Count > 0)
                {
                    DataRow dr = dtYear.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtYear.Rows.InsertAt(dr, 0);
                    cboYear.DataSource = dtYear;
                    cboYear.DisplayMember = "OrderYear";
                    cboYear.ValueMember = "OrderYear";
                    if (cboYear.Items.Count == 2)
                        cboYear.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWoProdQty", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboCustomer.DataSource = null;
                cboWorkorder.DataSource = null;
                dgv_Wo_Prod_Qty.DataSource = null;
                dgv_Wo_Prod_Qty.Columns.Clear();
                dgv_Wo_Prod_Qty.Rows.Clear();

                if (cboYear.SelectedIndex > 0)
                {
                    SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Year", Convert.ToInt32(cboYear.SelectedValue.ToString())) };
                    DataTable dtCust = (DataTable)dba.ExecuteReader_SP("sp_sel_WO_Year_Customer", spSel, DBAccess.Return_Type.DataTable);
                    if (dtCust.Rows.Count > 0)
                    {
                        DataRow dr = dtCust.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtCust.Rows.InsertAt(dr, 0);
                        cboCustomer.DataSource = dtCust;
                        cboCustomer.DisplayMember = "CustomerName";
                        cboCustomer.ValueMember = "CustomerName";
                        if (cboCustomer.Items.Count == 2)
                            cboCustomer.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWoProdQty", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboWorkorder.DataSource = null;
                dgv_Wo_Prod_Qty.DataSource = null;
                dgv_Wo_Prod_Qty.Columns.Clear();
                dgv_Wo_Prod_Qty.Rows.Clear();

                if (cboCustomer.SelectedIndex > 0)
                {
                    SqlParameter[] spSel = new SqlParameter[] { 
                        new SqlParameter("@Year", Convert.ToInt32(cboYear.SelectedValue.ToString())), 
                        new SqlParameter("@CustomerName", cboCustomer.SelectedValue.ToString()) 
                    };
                    DataTable dtWo = (DataTable)dba.ExecuteReader_SP("sp_sel_WO_Year_CustomerWise_WO", spSel, DBAccess.Return_Type.DataTable);
                    if (dtWo.Rows.Count > 0)
                    {
                        DataRow dr = dtWo.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtWo.Rows.InsertAt(dr, 0);
                        cboWorkorder.DataSource = dtWo;
                        cboWorkorder.DisplayMember = "WorkOrderNo";
                        cboWorkorder.ValueMember = "NPID";
                        if (cboWorkorder.Items.Count == 2)
                            cboWorkorder.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWoProdQty", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboWorkorder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgv_Wo_Prod_Qty.DataSource = null;
                dgv_Wo_Prod_Qty.Columns.Clear();
                dgv_Wo_Prod_Qty.Rows.Clear();

                if (cboWorkorder.SelectedIndex > 0)
                {
                    SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@NPID", cboWorkorder.SelectedValue.ToString()) };
                    DataTable dtData = (DataTable)dba.ExecuteReader_SP("sp_sel_DateWiseProdQty_to_Wo_v1", spSel, DBAccess.Return_Type.DataTable);
                    if (dtData.Rows.Count > 0)
                    {
                        dgv_Wo_Prod_Qty.DataSource = dtData;
                        dtExcel = dtData;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWoProdQty", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtExcel != null && dtExcel.Rows.Count > 0 && dgv_Wo_Prod_Qty.Rows.Count > 0)
                {
                    // Bind table data to Stream Writer to export data to respective folder
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = @"C:\";
                    saveFileDialog1.Title = "Save xls Files";
                    saveFileDialog1.DefaultExt = "xls";
                    saveFileDialog1.Filter = "Xls files (*.xls)|*.xls|All files (*.*)|*.*";
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string location = saveFileDialog1.FileName;
                        StreamWriter wr = new StreamWriter(location);
                        wr.Write("CUSTOMER:\t");
                        wr.Write(cboCustomer.SelectedValue.ToString() + "\t");
                        wr.Write("WO:\t");
                        wr.Write(cboWorkorder.Text + "\t");
                        wr.WriteLine();
                        // Write Columns to excel file
                        for (int i = 0; i < dtExcel.Columns.Count; i++)
                        {
                            wr.Write(dtExcel.Columns[i].ToString().ToUpper() + "\t");
                        }
                        wr.WriteLine();
                        //write rows to excel file
                        for (int i = 0; i < (dtExcel.Rows.Count); i++)
                        {
                            for (int j = 0; j < dtExcel.Columns.Count; j++)
                            {
                                if (dtExcel.Rows[i][j] != null)
                                    wr.Write(Convert.ToString(dtExcel.Rows[i][j]) + "\t");
                                else
                                    wr.Write("\t");
                            }
                            wr.WriteLine();
                        }
                        wr.Close();
                        MessageBox.Show("Data Exported Successfully");
                    }
                }
                else
                    MessageBox.Show("NO RECORDS");
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWoProdQty", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
