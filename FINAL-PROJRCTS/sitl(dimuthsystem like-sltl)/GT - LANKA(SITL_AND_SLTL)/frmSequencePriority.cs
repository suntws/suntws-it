using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GT
{
    public partial class frmSequencePriority : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dtPriority = new DataTable();
        public frmSequencePriority()
        {
            InitializeComponent();
        }
        private void frmSequencePriority_Load(object sender, EventArgs e)
        {
            try
            {
                cboSize.DataSource = null;
                cboRim.DataSource = null;
                dgv_PriorityItems.DataSource = null;
                dtPriority = (DataTable)dba.ExecuteReader_SP("sp_sel_sequence_priority", DBAccess.Return_Type.DataTable);
                if (dtPriority.Rows.Count > 0)
                {
                    DataView view = new DataView(dtPriority);
                    view.Sort = "tyresize ASC";
                    DataTable dtSize = view.ToTable(true, "tyresize");

                    DataRow dr = dtSize.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtSize.Rows.InsertAt(dr, 0);
                    cboSize.DataSource = dtSize;
                    cboSize.DisplayMember = "tyresize";
                    cboSize.ValueMember = "tyresize";
                    if (cboSize.Items.Count == 2)
                        cboSize.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePriority", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboRim.DataSource = null;
                dgv_PriorityItems.DataSource = null;
                if (cboSize.SelectedIndex > 0)
                {
                    DataView view = new DataView(dtPriority);
                    view.RowFilter = "tyresize='" + cboSize.SelectedValue.ToString() + "'";
                    view.Sort = "rimsize ASC";
                    DataTable dtRim = view.ToTable(true, "rimsize");

                    DataRow dr = dtRim.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtRim.Rows.InsertAt(dr, 0);
                    cboRim.DataSource = dtRim;
                    cboRim.DisplayMember = "rimsize";
                    cboRim.ValueMember = "rimsize";
                    if (cboRim.Items.Count == 2)
                        cboRim.SelectedIndex = 1;
                    else
                        bind_PriorityGridView();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePriority", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboRim_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgv_PriorityItems.DataSource = null;
                if (cboRim.SelectedIndex > 0)
                    bind_PriorityGridView();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePriority", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_PriorityGridView()
        {
            try
            {
                dgv_PriorityItems.DataSource = null;
                dgv_PriorityItems.Columns.Clear();

                string strFilter = cboSize.SelectedIndex > 0 ? "tyresize='" + cboSize.SelectedValue.ToString() + "'" : "";
                strFilter += (strFilter != "" && cboRim.SelectedIndex > 0 ? " AND " : "") + (cboRim.SelectedIndex > 0 ? "rimsize='" + cboRim.SelectedValue.ToString() + "'" : "");
                DataView dv_Items = new DataView(dtPriority);
                dv_Items.RowFilter = strFilter;
                dv_Items.Sort = "seq_priority ASC";
                DataTable dtFilterItems = dv_Items.ToTable(true);

                if (dtFilterItems.Rows.Count > 0)
                {
                    dgv_PriorityItems.DataSource = dtFilterItems;

                    dgv_PriorityItems.Columns[0].ReadOnly = true;
                    dgv_PriorityItems.Columns[0].Visible = false;
                    dgv_PriorityItems.Columns[1].ReadOnly = true;
                    dgv_PriorityItems.Columns[1].HeaderText = "CUSTOMER";
                    dgv_PriorityItems.Columns[1].Width = 150;
                    dgv_PriorityItems.Columns[2].ReadOnly = true;
                    dgv_PriorityItems.Columns[2].HeaderText = "WORK ORDER";
                    dgv_PriorityItems.Columns[2].Width = 130;
                    dgv_PriorityItems.Columns[3].ReadOnly = true;
                    dgv_PriorityItems.Columns[3].HeaderText = "PLATFORM";
                    dgv_PriorityItems.Columns[3].Width = 130;
                    dgv_PriorityItems.Columns[4].ReadOnly = true;
                    dgv_PriorityItems.Columns[4].HeaderText = "TYRE SIZE";
                    dgv_PriorityItems.Columns[4].Width = 130;
                    dgv_PriorityItems.Columns[5].ReadOnly = true;
                    dgv_PriorityItems.Columns[5].HeaderText = "RIM";
                    dgv_PriorityItems.Columns[5].Width = 60;
                    dgv_PriorityItems.Columns[6].ReadOnly = true;
                    dgv_PriorityItems.Columns[6].HeaderText = "TYPE";
                    dgv_PriorityItems.Columns[6].Width = 110;
                    dgv_PriorityItems.Columns[7].ReadOnly = true;
                    dgv_PriorityItems.Columns[7].HeaderText = "BRAND";
                    dgv_PriorityItems.Columns[7].Width = 130;
                    dgv_PriorityItems.Columns[8].ReadOnly = true;
                    dgv_PriorityItems.Columns[8].HeaderText = "SIDEWALL";
                    dgv_PriorityItems.Columns[8].Width = 130;
                    dgv_PriorityItems.Columns[9].ReadOnly = true;
                    dgv_PriorityItems.Columns[9].HeaderText = "REQ QTY";
                    dgv_PriorityItems.Columns[9].Width = 40;
                    dgv_PriorityItems.Columns[10].ReadOnly = true;
                    dgv_PriorityItems.Columns[10].HeaderText = "PRIORITY";
                    dgv_PriorityItems.Columns[10].Width = 80;

                    for (int z = 1; z <= dgv_PriorityItems.Rows.Count; z++)
                    {
                        dgv_PriorityItems.Rows[z - 1].Cells[dgv_PriorityItems.Columns.Count - 1].Value = z;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePriority", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            frmSequencePriority_Load(sender, e);
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_PriorityItems.Rows.Count > 0)
                {
                    var row = dgv_PriorityItems.SelectedRows[0];
                    if (row != null && row.Index > 0)
                    {
                        var swapRow = dgv_PriorityItems.Rows[row.Index - 1];
                        object[] values = new object[swapRow.Cells.Count];

                        foreach (DataGridViewCell cell in swapRow.Cells)
                        {
                            values[cell.ColumnIndex] = cell.Value;
                            cell.Value = row.Cells[cell.ColumnIndex].Value;
                        }

                        foreach (DataGridViewCell cell in row.Cells)
                            cell.Value = values[cell.ColumnIndex];

                        dgv_PriorityItems.Rows[row.Index - 1].Selected = true;
                    }
                    for (int z = 1; z <= dgv_PriorityItems.Rows.Count; z++)
                    {
                        dgv_PriorityItems.Rows[z - 1].Cells[dgv_PriorityItems.Columns.Count - 1].Value = z;
                    }
                }
                else
                    MessageBox.Show("NO RECORDS");
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePriority", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_PriorityItems.Rows.Count > 0)
                {
                    var row = dgv_PriorityItems.SelectedRows[0];
                    if (row != null && row.Index >= 0 && dgv_PriorityItems.Rows.Count - 1 > row.Index)
                    {
                        var swapRow = dgv_PriorityItems.Rows[row.Index + 1];
                        object[] values = new object[swapRow.Cells.Count];

                        foreach (DataGridViewCell cell in swapRow.Cells)
                        {
                            values[cell.ColumnIndex] = cell.Value;
                            cell.Value = row.Cells[cell.ColumnIndex].Value;
                        }

                        foreach (DataGridViewCell cell in row.Cells)
                            cell.Value = values[cell.ColumnIndex];

                        dgv_PriorityItems.Rows[row.Index + 1].Selected = true;
                    }
                    for (int z = 1; z <= dgv_PriorityItems.Rows.Count; z++)
                    {
                        dgv_PriorityItems.Rows[z - 1].Cells[dgv_PriorityItems.Columns.Count - 1].Value = z;
                    }
                }
                else
                    MessageBox.Show("NO RECORDS");
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePriority", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_PriorityItems.Rows.Count > 0)
                {
                    int resp = 0;
                    foreach (DataGridViewRow uRow in dgv_PriorityItems.Rows)
                    {
                        SqlParameter[] spU = new SqlParameter[] { 
                            new SqlParameter("@seq_priority", uRow.Cells[dgv_PriorityItems.Columns.Count - 1].Value), 
                            new SqlParameter("@Prod_ItemID", uRow.Cells[0].Value), 
                            new SqlParameter("@seqUser", Program.strUserName) 
                        };
                        resp += dba.ExecuteNonQuery_SP("sp_upd_newProduction_SequenceChange", spU);
                    }
                    if (resp > 0)
                    {
                        MessageBox.Show("PRODUCTION PRIORITY UPDATED SUCCESSFULLY");
                        frmSequencePriority_Load(sender, e);
                    }
                }
                else
                    MessageBox.Show("NO RECORDS");
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePriority", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
