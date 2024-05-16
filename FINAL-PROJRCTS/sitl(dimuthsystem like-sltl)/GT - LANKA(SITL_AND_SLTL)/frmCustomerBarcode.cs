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
using System.Runtime.InteropServices;

namespace GT
{
    public partial class frmCustomerBarcode : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int conn, int val);

        SqlConnection oleCon = new SqlConnection();
        public frmCustomerBarcode()
        {
            InitializeComponent();
        }
        private void frmCustomerBarcode_Load(object sender, EventArgs e)
        {
            try
            {
                int Out;
                if (InternetGetConnectedState(out Out, 0) == false)
                    MessageBox.Show("Internet Not Connected !");
                else
                {
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
                        oleCon.Open();
                    }
                    catch (SqlException)
                    {
                        try
                        {
                            oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
                            oleCon.Open();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (oleCon.State == ConnectionState.Open)
                    {
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@PdiPlant", Program.strPlantName) };
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_sel_CustomerList_For_BarcodeRequired";
                        cmd.CommandTimeout = 1200;
                        cmd.Connection = oleCon;
                        foreach (SqlParameter Sp in sp)
                        {
                            cmd.Parameters.Add(Sp);
                        }
                        DataSet ds = new DataSet();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(ds);

                        DataTable dtCust = ds.Tables[0];
                        if (dtCust.Rows.Count > 0)
                        {
                            DataRow dr = dtCust.NewRow();
                            dr.ItemArray = new object[] { "CHOOSE" };
                            dtCust.Rows.InsertAt(dr, 0);
                            cboCustomer.DataSource = dtCust;
                            cboCustomer.DisplayMember = "customername";
                            cboCustomer.ValueMember = "customername";
                            if (dtCust.Rows.Count == 2)
                                cboCustomer.SelectedIndex = 1;
                        }
                        cmd.Dispose();
                        oleCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int Out;
                if (InternetGetConnectedState(out Out, 0) == false)
                    MessageBox.Show("Internet Not Connected !");
                else
                {
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
                        oleCon.Open();
                    }
                    catch (SqlException)
                    {
                        try
                        {
                            oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
                            oleCon.Open();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (oleCon.State == ConnectionState.Open)
                    {
                        cboWorkorder.DataSource = null;
                        dg_OrderItem.DataSource = null;
                        if (cboCustomer.SelectedIndex > 0)
                        {
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@custname", cboCustomer.Text), new SqlParameter("@PdiPlant", Program.strPlantName) };
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "sp_sel_woList_barcodeRequired";
                            cmd.CommandTimeout = 1200;
                            cmd.Connection = oleCon;
                            foreach (SqlParameter Sp in sp)
                            {
                                cmd.Parameters.Add(Sp);
                            }
                            DataSet ds = new DataSet();
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            adp.Fill(ds);

                            DataTable dtWo = ds.Tables[0];
                            if (dtWo.Rows.Count > 0)
                            {
                                DataRow dr = dtWo.NewRow();
                                dr.ItemArray = new object[] { "CHOOSE" };
                                dtWo.Rows.InsertAt(dr, 0);
                                cboWorkorder.DataSource = dtWo;
                                cboWorkorder.DisplayMember = "workorderno";
                                cboWorkorder.ValueMember = "workorderno";
                                if (dtWo.Rows.Count == 2)
                                    cboWorkorder.SelectedIndex = 1;
                            }
                            cmd.Dispose();
                            oleCon.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cboWorkorder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int Out;
                if (InternetGetConnectedState(out Out, 0) == false)
                    MessageBox.Show("Internet Not Connected !");
                else
                {
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
                        oleCon.Open();
                    }
                    catch (SqlException)
                    {
                        try
                        {
                            oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
                            oleCon.Open();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (oleCon.State == ConnectionState.Open)
                    {
                        dg_OrderItem.DataSource = null;
                        if (cboWorkorder.SelectedIndex > 0)
                        {
                            SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@custname", cboCustomer.Text), 
                                new SqlParameter("@workorderno", cboWorkorder.Text), 
                                new SqlParameter("@plant", Program.strPlantName) 
                            };
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "sp_sel_itemlist_for_BarcodeRequired";
                            cmd.CommandTimeout = 1200;
                            cmd.Connection = oleCon;
                            foreach (SqlParameter Sp in sp)
                            {
                                cmd.Parameters.Add(Sp);
                            }
                            DataSet ds = new DataSet();
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            adp.Fill(ds);

                            DataTable dtItem = ds.Tables[0];
                            if (dtItem.Rows.Count > 0)
                            {
                                dg_OrderItem.DataSource = dtItem;
                                dg_OrderItem.ColumnHeadersDefaultCellStyle.BackColor = Color.Cyan;
                                dg_OrderItem.EnableHeadersVisualStyles = false;
                                for (int i = 0; i <= dg_OrderItem.Columns.Count - 1; i++)
                                {
                                    dg_OrderItem.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                }
                            }
                            cmd.Dispose();
                            oleCon.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dg_OrderItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    string strBarcode = (dg_OrderItem.Rows[e.RowIndex].Cells[7].Value).ToString();
                    if (Convert.ToInt32((dg_OrderItem.Rows[e.RowIndex].Cells[6].Value).ToString()) > 0)
                    {
                        if (strBarcode != "")
                        {
                            if ((dg_OrderItem.Rows[e.RowIndex].Cells[8].Value).ToString() == "WAITING")
                            {
                                bool boolStatus = false;
                                for (int k = 0; k < Convert.ToInt32((dg_OrderItem.Rows[e.RowIndex].Cells[6].Value).ToString()); k++)
                                {
                                    string strLogFile = @"C:\\Barcode Data\\";
                                    if (!Directory.Exists(strLogFile))
                                        Directory.CreateDirectory(strLogFile);
                                    strLogFile = strLogFile + (DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second) + k + ".txt";
                                    StreamWriter SWrtiter = System.IO.File.AppendText(strLogFile);
                                    SWrtiter.WriteLine(strBarcode);
                                    SWrtiter.Close();
                                    boolStatus = true;
                                }
                                if (boolStatus)
                                {
                                    MessageBox.Show("LABLE PRINTED SUCCESSFULLY FOR ROW ITEM " + (e.RowIndex + 1));
                                    dg_OrderItem.Rows[e.RowIndex].Cells[8].Value = "PRINTED";
                                    dg_OrderItem.Rows[e.RowIndex].Cells[8].Style.BackColor = Color.Green;
                                }
                            }
                            else if ((dg_OrderItem.Rows[e.RowIndex].Cells[8].Value).ToString() == "PRINTED")
                                MessageBox.Show("ALREADY PRINTED");
                        }
                        else
                            MessageBox.Show("BARCODE TEXT NOT AVAILABLE");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
