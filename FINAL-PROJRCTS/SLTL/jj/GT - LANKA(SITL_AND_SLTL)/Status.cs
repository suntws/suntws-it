using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace GT
{
    public partial class Status : Form
    {
        DBAccess dba = new DBAccess();
        public Status()
        {
            InitializeComponent();
        }

        private void dateTimePicker2_CloseUp(object sender, EventArgs e)
        {
            DateTime Fromdatee = Convert.ToDateTime(dateTimePicker1.Text);
            DateTime Todatee = Convert.ToDateTime(dateTimePicker2.Text);
            TimeSpan ts = Todatee.Subtract(Fromdatee);
            int days = Convert.ToInt16(ts.Days);
            if (Fromdatee<=Todatee)
            {
                
                label3.Text = "the data below  provided for" + days +"days";
                label3.Font = new Font("arial", 10);
                label3.ForeColor = Color.Green;
                Bind_Gridview();


            }
            else
            {
                label3.Text = "kindly check the from and to date:" + days;
                label3.Font = new Font("arial", 10);
                label3.ForeColor = Color.Red;
            }
        }
        private void Bind_Gridview()
        {
            if(comboBox1.SelectedIndex==0)
            {
                SqlParameter[] spDate = new SqlParameter[] { new SqlParameter("@from", dateTimePicker1.Text), new SqlParameter("@todate", dateTimePicker2.Text) };
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_status_v1", spDate, DBAccess.Return_Type.DataTable);

                dataGridView1.DataSource = dt;
            }
            else
            {
                SqlParameter[] spDate = new SqlParameter[] { new SqlParameter("@from", dateTimePicker1.Text), new SqlParameter("@todate", dateTimePicker2.Text), new SqlParameter("@shift", comboBox1.Text) };
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_status_shift_v1", spDate, DBAccess.Return_Type.DataTable);

                dataGridView1.DataSource = dt;

            }


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.SelectAll();
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            DataObject copydata = dataGridView1.GetClipboardContent() ;
            
            if (copydata != null) Clipboard.SetDataObject(copydata);
            Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            xlapp.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook xlWbook;
            Microsoft.Office.Interop.Excel.Worksheet xlsheet;
            object miseddata = System.Reflection.Missing.Value;
            xlWbook = xlapp.Workbooks.Add(miseddata);

            xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range xlr = (Microsoft.Office.Interop.Excel.Range)xlsheet.Cells[1, 1];
            xlr.Select();

            xlsheet.PasteSpecial(xlr, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        private void Status_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {


            foreach (DataGridViewRow row in dataGridView1.Rows)
                if (row.Cells[11].Value != null)
                {
                    if (row.Cells[11].Value.ToString() == "shift1")
                    {
                        row.DefaultCellStyle.BackColor = Color.Red;
                        row.DefaultCellStyle.ForeColor = Color.Green;
                    }
                }


        }

        private void dataGridView1_RowsDefaultCellStyleChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }
    }
}
