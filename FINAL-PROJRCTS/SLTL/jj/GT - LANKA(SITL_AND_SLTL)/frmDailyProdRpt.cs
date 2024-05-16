using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace GT
{
    public partial class frmDailyProdRpt : Form
    {
        DBAccess dba = new DBAccess();
        string duration = ""; DataTable dtExcel;
        public frmDailyProdRpt()
        {
            InitializeComponent();
        }
        private void frmDailyProdRpt_Load(object sender, EventArgs e)
        {
            try
            {
                this.rdoDay.CheckedChanged += new System.EventHandler(this.rdoDuration_CheckedChanged);
                this.rdoMonth.CheckedChanged += new System.EventHandler(this.rdoDuration_CheckedChanged);
                this.rdoYear.CheckedChanged += new System.EventHandler(this.rdoDuration_CheckedChanged);
                this.rdoFromTo.CheckedChanged += new System.EventHandler(this.rdoDuration_CheckedChanged);

                dtpFromDate.Value = System.DateTime.Now;
                dtpToDate.Value = System.DateTime.Now;
                dtpFromDate.MaxDate = DateTime.Now;
                dtpToDate.MaxDate = DateTime.Now;
                label2.Visible = false;
                rdbProdcution.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void rdoDuration_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rdo = (RadioButton)sender;
                duration = rdo.Text;
                dtpFromDate.ShowUpDown = false;
                dtpToDate.Visible = false;
                label2.Visible = false;
                dtpFromDate.Value = DateTime.Now;
                dtpToDate.Value = DateTime.Now;
                switch (rdo.Text)
                {
                    case "Day":
                        dtpFromDate.CustomFormat = "dd/MMM/yyyy";
                        dtpToDate.Visible = false;
                        dtpToDate.Visible = false;
                        break;
                    case "Month":
                        dtpFromDate.CustomFormat = "MMM/yyyy";
                        dtpToDate.Visible = false;
                        dtpToDate.Visible = false;
                        break;
                    case "Year":
                        dtpFromDate.CustomFormat = "yyyy";
                        dtpFromDate.ShowUpDown = true;
                        dtpToDate.Visible = false;
                        dtpToDate.Visible = false;
                        break;
                    case "From-To":
                        dtpFromDate.CustomFormat = "dd/MMM/yyyy";
                        dtpFromDate.ShowUpDown = true;
                        dtpToDate.CustomFormat = "dd/MMM/yyyy";
                        dtpToDate.ShowUpDown = true;
                        dtpToDate.Visible = true;
                        dtpToDate.Visible = true;
                        label2.Visible = true;
                        break;
                }
                if (duration != "")
                    dtpFromDate.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string strCondition = string.Empty;
                string fromDate = "", toDate = "";
                if (duration == "Day")
                {
                    fromDate = dtpFromDate.Text;
                    toDate = dtpFromDate.Text;
                    strCondition += " convert(varchar,dateofmanufacture,105)='" + Convert.ToDateTime(dtpFromDate.Value).ToString("dd-MM-yyyy") + "'";
                }
                else if (duration == "Month")
                {
                    fromDate = "1/" + dtpFromDate.Text;
                    if (dtpFromDate.Text == DateTime.Now.ToString("MMM/yyyy"))
                        toDate = DateTime.Now.ToString("dd/MMM/yyyy");
                    else
                        toDate = Convert.ToDateTime("1/" + dtpFromDate.Text).AddMonths(1).AddDays(-1).ToString("dd/MMM/yyyy");
                    strCondition += " MONTH(dateofmanufacture)=" + Convert.ToDateTime(fromDate).Month + " and YEAR(dateofmanufacture)=" + Convert.ToDateTime(fromDate).Year + "";
                }
                else if (duration == "Year")
                {
                    fromDate = "1/Jan/" + dtpFromDate.Text;
                    if (Convert.ToDateTime("1/" + dtpFromDate.Text).Year == DateTime.Now.Year)
                        toDate = DateTime.Now.ToString("dd/MMM/yyyy");
                    else
                        toDate = Convert.ToDateTime("1/" + dtpFromDate.Text).AddYears(1).AddDays(-1).ToString("dd/MMM/yyyy");
                    strCondition += " YEAR(dateofmanufacture)=" + Convert.ToDateTime(fromDate).Year + "";
                }
                else if (duration == "From-To")
                {
                    fromDate = dtpFromDate.Text;
                    toDate = dtpToDate.Text;
                    strCondition += "(convert(varchar,dateofmanufacture,112)>=" + Convert.ToDateTime(dtpFromDate.Value).ToString("yyyyMMdd") + " and CONVERT(varchar,dateofmanufacture,112)<=" + Convert.ToDateTime(dtpToDate.Value).ToString("yyyyMMdd") + ")";
                }
                dtpFromDate.Value = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToLongDateString());
                dtpToDate.Value = Convert.ToDateTime(Convert.ToDateTime(toDate).ToLongDateString());
                string strQry = "";
                if (rdbDispatch.Checked)
                {
                    strCondition = strCondition.Replace("dateofmanufacture", "Dispatchdate");
                    strQry = "select Config as [PLATFORM],tyresize as [TYRE SIZE],convert(float,tyrerim) as [RIM],tyretype as [TYPE],Brand as [BRAND]," +
                        "Sidewall as [SIDEWALL],qualitygrade as [GRADE],convert(varchar,dateofmanufacture,105) as [DOM],Barcode as [BARCODE],stencilno as [STENCIL NO]," +
                        "Location as [LOCATION],Dispatchdate as [DISPATCHED ON] from tbqualitycontrol where " + strCondition + "  order by CreateDate desc";
                }
                else
                    strQry = "select Config as [PLATFORM],tyresize as [TYRE SIZE],convert(float,tyrerim) as [RIM],tyretype as [TYPE],Brand as [BRAND]," +
                        "Sidewall as [SIDEWALL],qualitygrade as [GRADE],convert(varchar,dateofmanufacture,105) as [DOM],Barcode as [BARCODE],stencilno as [STENCIL NO]," +
                        "Location as [LOCATION] from tbqualitycontrol where " + strCondition + "  order by CreateDate desc";

                DataTable dtData = (DataTable)dba.ExecuteReader_SP(strQry, DBAccess.Return_Type.DataTable);
                dataGridView1.DataSource = null;
                if (dtData.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dtData;
                    dtExcel = dtData;
                    lblCountMsg.Text = "COUNT: " + dtData.Rows.Count;
                }
                else
                    MessageBox.Show("NO RECORDS");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtExcel != null && dtExcel.Rows.Count > 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = @"C:\";
                    saveFileDialog1.Title = "Save txt Files";
                    saveFileDialog1.DefaultExt = "txt";
                    saveFileDialog1.Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string strLogFile = saveFileDialog1.FileName;
                        if (!Directory.Exists(strLogFile))
                            Directory.CreateDirectory(strLogFile);
                        TextWriter writer = File.AppendText(strLogFile + ".txt");
                        for (int i = 0; i < dtExcel.Columns.Count; i++)
                        {
                            writer.Write(dtExcel.Columns[i].ToString().ToUpper() + "|");
                        }
                        writer.Write(System.Environment.NewLine);
                        for (int i = 0; i < dtExcel.Rows.Count; i++)
                        {
                            for (int j = 0; j < dtExcel.Columns.Count; j++)
                            {
                                if (j != 7)
                                    writer.Write(dtExcel.Rows[i][j].ToString() + "|");
                                else if (j == 7)
                                    writer.Write(Convert.ToDateTime(dtExcel.Rows[i][j].ToString()).ToShortDateString() + "|");
                            }
                            writer.Write(System.Environment.NewLine);
                        }
                        writer.Close();
                        MessageBox.Show("Data Exported");
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
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtExcel != null && dtExcel.Rows.Count > 0)
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
                        StreamWriter wr = new StreamWriter(location + ".xls");
                        wr.Write("FROM\t");
                        wr.Write(dtpFromDate.Value.ToShortDateString() + "\t");
                        wr.WriteLine();
                        wr.Write("TO\t");
                        wr.Write(dtpToDate.Value.ToShortDateString() + "\t");
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
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string strQry = "select Config as [PLATFORM],TyreSize as [TYRE SIZE],TyreRim as [RIM],TyreType as [TYPE],Brand as [BRAND],Sidewall as [SIDEWALL]," +
                    "ProcessID as [PROCESS-ID],finishedWt as [FWT] from ProcessID_Details";
                DataTable dtProcess = (DataTable)dba.ExecuteReader_SP(strQry, DBAccess.Return_Type.DataTable);
                if (dtProcess != null && dtProcess.Rows.Count > 0)
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
                        StreamWriter wr = new StreamWriter(location + ".xls");
                        // Write Columns to excel file
                        for (int i = 0; i < dtProcess.Columns.Count; i++)
                        {
                            wr.Write(dtProcess.Columns[i].ToString().ToUpper() + "\t");
                        }
                        wr.WriteLine();
                        //write rows to excel file
                        for (int i = 0; i < (dtProcess.Rows.Count); i++)
                        {
                            for (int j = 0; j < dtProcess.Columns.Count; j++)
                            {
                                if (dtProcess.Rows[i][j] != null)
                                    wr.Write(Convert.ToString(dtProcess.Rows[i][j]) + "\t");
                                else
                                    wr.Write("\t");
                            }
                            wr.WriteLine();
                        }
                        wr.Close();
                        MessageBox.Show("Process-ID Exported Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
