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
    public partial class frmProdAllocateRpt : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dtExcel;
        public frmProdAllocateRpt()
        {
            InitializeComponent();
        }

        private void frmProdAllocateRpt_Load(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Plant", Program.strPlantName) };
                DataTable dtDate = (DataTable)dba.ExecuteReader_SP("sp_sel_MinMax_ProductionDate_v1", spSel, DBAccess.Return_Type.DataTable);
                if (dtDate.Rows.Count == 1)
                {
                    this.dtp_ProdAllocateDate.MinDate = Convert.ToDateTime(dtDate.Rows[0]["MinDate"].ToString());
                    this.dtp_ProdAllocateDate.MaxDate = Convert.ToDateTime(dtDate.Rows[0]["MaxDate"].ToString());
                    if (dtp_ProdAllocateDate.MaxDate > DateTime.Now)
                        this.dtp_ProdAllocateDate.Value = DateTime.Now;
                    else
                        this.dtp_ProdAllocateDate.Value = dtp_ProdAllocateDate.MaxDate;
                }
                else
                    MessageBox.Show("NO RECORDS");
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmProdAllocateRpt", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Plant", Program.strPlantName), new SqlParameter("@ProductionDate", dtp_ProdAllocateDate.Value) };
                DataTable dtList = (DataTable)dba.ExecuteReader_SP("sp_sel_ProdAllocation_List_v1", spSel, DBAccess.Return_Type.DataTable);
                if (dtList.Rows.Count > 0)
                {
                    dgv_ProdAllocated.DataSource = dtList;
                    dgv_ProdAllocated.Columns[0].Width = 100; //Workorder
                    dgv_ProdAllocated.Columns[1].Width = 100; //Platform
                    dgv_ProdAllocated.Columns[2].Width = 150; //tyre size
                    dgv_ProdAllocated.Columns[3].Width = 40; //rim
                    dgv_ProdAllocated.Columns[4].Width = 60; //type
                    dgv_ProdAllocated.Columns[5].Width = 100; //brand
                    dgv_ProdAllocated.Columns[6].Width = 100; //sidewall
                    dgv_ProdAllocated.Columns[7].Width = 50; //process id
                    dgv_ProdAllocated.Columns[8].Width = 40; //order qty
                    dgv_ProdAllocated.Columns[9].Width = 40; //req qty
                    dgv_ProdAllocated.Columns[10].Width = 50; //weigh QTY
                    dgv_ProdAllocated.Columns[11].Width = 50; //QC Reject
                    dgv_ProdAllocated.Columns[12].Width = 70; //produce

                    dtExcel = dtList;
                }
                else
                {
                    MessageBox.Show("NO RECORDS");
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmProdAllocateRpt", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtExcel != null && dtExcel.Rows.Count > 0 && dgv_ProdAllocated.Rows.Count > 0)
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
                        wr.Write("DATE ON:\t");
                        wr.Write(dtp_ProdAllocateDate.Value.ToShortDateString() + "\t");
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
