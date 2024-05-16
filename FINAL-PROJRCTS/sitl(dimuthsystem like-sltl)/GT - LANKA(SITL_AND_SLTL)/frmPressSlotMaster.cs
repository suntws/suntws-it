using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

namespace GT
{
    public partial class frmPressSlotMaster : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dtPressList;
        public frmPressSlotMaster()
        {
            InitializeComponent();
        }
        private void frmPressSlotMaster_Load(object sender, EventArgs e)
        {
            try
            {
                txtUnit.Visible = false;
                txtStation.Visible = false;
                btnSuspend.Visible = false;
                dtPressList = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringPressList", DBAccess.Return_Type.DataTable);
                if (dtPressList.Rows.Count > 0)
                {
                    DataTable dtList = new DataTable();
                    dtList = dtPressList;
                    bind_GridView(dtList);
                    DataView view = new DataView(dtList);
                    view.Sort = "CuringUnit ASC";
                    DataTable distinctUnit = view.ToTable(true, "CuringUnit");

                    DataRow toInsert = distinctUnit.NewRow();
                    toInsert.ItemArray = new object[] { "CHOOSE" };
                    distinctUnit.Rows.InsertAt(toInsert, 0);

                    toInsert = distinctUnit.NewRow();
                    toInsert.ItemArray = new object[] { "ADD NEW UNIT" };
                    distinctUnit.Rows.InsertAt(toInsert, distinctUnit.Rows.Count);

                    cboUnit.DataSource = distinctUnit;
                    cboUnit.DisplayMember = "CuringUnit";
                    cboUnit.ValueMember = "CuringUnit";

                    if (cboUnit.Items.Count == 2)
                        cboUnit.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressSlotMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_GridView(DataTable dtItems)
        {
            try
            {
                dgvPressMaster.DataSource = null;
                dgvPressMaster.Rows.Clear();

                if (dtItems.Rows.Count > 0)
                {
                    dgvPressMaster.DataSource = dtItems;

                    dgvPressMaster.Columns[0].HeaderText = "CURING UNIT";
                    dgvPressMaster.Columns[1].HeaderText = "STATION";
                    dgvPressMaster.Columns[2].HeaderText = "AVAL SLOT";
                    dgvPressMaster.Columns[3].HeaderText = "CREATED BY";
                    dgvPressMaster.Columns[4].HeaderText = "CREATED ON";
                    dgvPressMaster.Columns[5].HeaderText = "MODIFIED ON";

                    dgvPressMaster.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressSlotMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtUnit.Visible = false;
                txtStation.Visible = false;
                btnSuspend.Visible = false;
                cboStation.Enabled = true;
                cboStation.DataSource = null;
                dgvPressMaster.DataSource = null;
                txtUnit.Text = "";
                txtStation.Text = "";
                txtAvalSLot.Text = "";
                if (cboUnit.SelectedIndex > 0)
                {
                    if (cboUnit.SelectedIndex < cboUnit.Items.Count - 1)
                    {
                        DataView dtView = new DataView(dtPressList);
                        dtView.RowFilter = "CuringUnit='" + cboUnit.SelectedValue.ToString() + "'";
                        dtView.Sort = "CuringStation ASC";
                        DataTable disList = dtView.ToTable(true);
                        if (disList.Rows.Count > 0)
                        {
                            bind_GridView(disList);

                            DataView view = new DataView(disList);
                            view.Sort = "CuringStation ASC";
                            DataTable distinctStation = view.ToTable(true, "CuringStation");

                            DataRow toInsert = distinctStation.NewRow();
                            toInsert.ItemArray = new object[] { "CHOOSE" };
                            distinctStation.Rows.InsertAt(toInsert, 0);

                            toInsert = distinctStation.NewRow();
                            toInsert.ItemArray = new object[] { "ADD NEW STATION" };
                            distinctStation.Rows.InsertAt(toInsert, distinctStation.Rows.Count);

                            cboStation.DataSource = distinctStation;
                            cboStation.DisplayMember = "CuringStation";
                            cboStation.ValueMember = "CuringStation";

                            if (cboStation.Items.Count == 2)
                                cboStation.SelectedIndex = 1;
                        }
                    }
                    else if (cboUnit.Items.Count == cboUnit.SelectedIndex + 1)
                    {
                        txtUnit.Visible = true;
                        txtStation.Visible = true;
                        cboStation.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressSlotMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtStation.Visible = false;
                btnSuspend.Visible = false;
                dgvPressMaster.DataSource = null;
                txtStation.Text = "";
                txtAvalSLot.Text = "";
                if (cboStation.SelectedIndex > 0)
                {
                    if (cboStation.SelectedIndex < cboStation.Items.Count - 1)
                    {
                        DataView dtView = new DataView(dtPressList);
                        dtView.RowFilter = "CuringUnit='" + cboUnit.SelectedValue.ToString() + "' AND CuringStation='" + cboStation.SelectedValue.ToString() + "'";
                        dtView.Sort = "AvalSlot ASC";
                        DataTable disList = dtView.ToTable(true);
                        if (disList.Rows.Count == 1)
                        {
                            bind_GridView(disList);
                            txtAvalSLot.Text = disList.Rows[0]["AvalSlot"].ToString();
                            btnSuspend.Visible = true;
                        }
                    }
                    else if (cboStation.Items.Count == cboStation.SelectedIndex + 1)
                    {
                        txtStation.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressSlotMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtAvalSLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            frmPressSlotMaster_Load(sender, e);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboUnit.SelectedIndex == 0)
                    MessageBox.Show("Choose unit");
                else if (cboUnit.SelectedIndex + 1 == cboUnit.Items.Count && txtUnit.Text == "" && txtStation.Text == "")
                    MessageBox.Show("Enter new unit/station");
                else if (cboStation.SelectedIndex == 0)
                    MessageBox.Show("Choose station");
                else if (cboStation.SelectedIndex + 1 == cboStation.Items.Count && txtStation.Text == "")
                    MessageBox.Show("Enter new station");
                else if (txtAvalSLot.Text == "" || txtAvalSLot.Text == "0")
                    MessageBox.Show("Enter available mould slot");
                else
                {
                    SqlParameter[] spIns = new SqlParameter[] { 
                        new SqlParameter("@CuringUnit", cboUnit.SelectedIndex + 1 == cboUnit.Items.Count ? txtUnit.Text : cboUnit.SelectedValue.ToString()), 
                        new SqlParameter("@CuringStation", cboStation.SelectedIndex + 1 == cboStation.Items.Count ? txtStation.Text : cboStation.SelectedValue.ToString()), 
                        new SqlParameter("@AvalSlot", txtAvalSLot.Text), 
                        new SqlParameter("@CreatedBy", Program.strUserName) 
                    };
                    int resp = dba.ExecuteNonQuery_SP("sp_ins_CuringPressList", spIns);
                    if (resp > 0)
                    {
                        MessageBox.Show("SAVED SUCCESSFULLY");
                        frmPressSlotMaster_Load(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressSlotMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSuspend_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spUpd = new SqlParameter[] { 
                    new SqlParameter("@CuringUnit", cboUnit.SelectedValue.ToString()), 
                    new SqlParameter("@CuringStation", cboStation.SelectedValue.ToString()), 
                    new SqlParameter("@CreatedBy", Program.strUserName) 
                };
                int resp = dba.ExecuteNonQuery_SP("sp_sel_upd_CuringPressList", spUpd);
                if (resp > 0)
                {
                    MessageBox.Show("DISABLED SUCCESSFULLY");
                    frmPressSlotMaster_Load(sender, e);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPressSlotMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
