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
using System.Reflection;

namespace GT
{
    public partial class frmWeighPlan : Form
    {
        public static int Prod_ItemID = 0, ProductionID = 0;
        public static string Prod_Press = "";
        public static bool Weigh_Manual = false;
        DataTable dtPlantItem;
        DBAccess dba = new DBAccess();
        public frmWeighPlan()
        {
            InitializeComponent();
        }
        private void frmWeighPlan_Load(object sender, EventArgs e)
        {
            try
            {
                this.Location = new Point(0, 0);
                cboPress.DataSource = null;
                cboTyreSize.DataSource = null;
                cboType.DataSource = null;
                cboBrand.DataSource = null;
                dgv_Prod_PlanItems.DataSource = null;
                dgv_Prod_PlanItems.Columns.Clear();

                SqlParameter[] spDate = new SqlParameter[] { new SqlParameter("@Plant", Program.strPlantName) };
                dtPlantItem = (DataTable)dba.ExecuteReader_SP("sp_sel_Prod_Weighing_PlanList_v2", spDate, DBAccess.Return_Type.DataTable);
                if (dtPlantItem.Rows.Count > 0)
                {
                    DataView view = new DataView(dtPlantItem);
                    view.Sort = "Prod_Press ASC";
                    DataTable dtPress = view.ToTable(true, "Prod_Press");

                    DataRow dr = dtPress.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtPress.Rows.InsertAt(dr, 0);
                    cboPress.DataSource = dtPress;
                    cboPress.DisplayMember = "Prod_Press";
                    cboPress.ValueMember = "Prod_Press";
                    if (cboPress.Items.Count == 2)
                        cboPress.SelectedIndex = 1;
                    else
                        cbo_SelectIndex();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Gv(DataTable dtList)
        {
            try
            {
                dgv_Prod_PlanItems.DataSource = null;
                dgv_Prod_PlanItems.Columns.Clear();
                dgv_Prod_PlanItems.Rows.Clear();

                dgv_Prod_PlanItems.DataSource = dtList;
                dgv_Prod_PlanItems.Columns[0].Visible = false; //Prod_ItemID
                dgv_Prod_PlanItems.Columns[1].Visible = false; //Prod_PlanID
                dgv_Prod_PlanItems.Columns[2].Width = 100; //Prod_Press
                dgv_Prod_PlanItems.Columns[2].HeaderText = "CURING UNIT";
                dgv_Prod_PlanItems.Columns[2].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[3].Width = 80; //Prod_MouldID
                dgv_Prod_PlanItems.Columns[3].HeaderText = "MOULD";
                dgv_Prod_PlanItems.Columns[3].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[4].Width = 100; //Config
                dgv_Prod_PlanItems.Columns[4].HeaderText = "PLATFORM";
                dgv_Prod_PlanItems.Columns[4].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[5].Width = 180; //tyresize
                dgv_Prod_PlanItems.Columns[5].HeaderText = "TYRE SIZE";
                dgv_Prod_PlanItems.Columns[5].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[6].Width = 40; //rim
                dgv_Prod_PlanItems.Columns[6].HeaderText = "RIM";
                dgv_Prod_PlanItems.Columns[6].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[7].Width = 60; //type
                dgv_Prod_PlanItems.Columns[7].HeaderText = "TYPE";
                dgv_Prod_PlanItems.Columns[7].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[8].Width = 100; //brand
                dgv_Prod_PlanItems.Columns[8].HeaderText = "BRAND";
                dgv_Prod_PlanItems.Columns[8].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[9].Width = 100; //sidewall
                dgv_Prod_PlanItems.Columns[9].HeaderText = "SIDEWALL";
                dgv_Prod_PlanItems.Columns[9].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[10].Width = 65; //processid
                dgv_Prod_PlanItems.Columns[10].HeaderText = "PROCESS-ID";
                dgv_Prod_PlanItems.Columns[10].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[11].Width = 40; //Prod_ReqQty
                dgv_Prod_PlanItems.Columns[11].HeaderText = "REQ QTY";
                dgv_Prod_PlanItems.Columns[11].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[12].Width = 50; //WeighingQty
                dgv_Prod_PlanItems.Columns[12].HeaderText = "WEIGH QTY";
                dgv_Prod_PlanItems.Columns[12].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[13].Width = 45; //RejectQty
                dgv_Prod_PlanItems.Columns[13].HeaderText = "REJECT QTY";
                dgv_Prod_PlanItems.Columns[13].ReadOnly = true;
                dgv_Prod_PlanItems.Columns[14].Width = 65; //Prod_RemainQty
                dgv_Prod_PlanItems.Columns[14].HeaderText = "BALANCE QTY";
                dgv_Prod_PlanItems.Columns[14].ReadOnly = true;

                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                dgv_Prod_PlanItems.Columns.Add(btn);
                btn.HeaderText = "SYSTEM";
                btn.Text = "SCALE";
                btn.Name = "btn_weightMaster";
                btn.UseColumnTextForButtonValue = true;

                foreach (DataGridViewRow rGvc in dgv_Prod_PlanItems.Rows)
                {
                    if ((rGvc.Cells[0].Value).ToString() == "0" && (rGvc.Cells[1].Value).ToString() == "0")
                    {
                        DataGridViewTextBoxCell txtcell = new DataGridViewTextBoxCell();
                        rGvc.Cells[15] = txtcell;
                    }
                }

                foreach (DataGridViewColumn dgvc in dgv_Prod_PlanItems.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgv_Prod_PlanItems.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dgv_Prod_Weighing_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (dgv_Prod_PlanItems.Columns[e.ColumnIndex].Name == "btn_weightMaster")
                    {
                        Prod_ItemID = Convert.ToInt32(dgv_Prod_PlanItems.Rows[e.RowIndex].Cells["Prod_ItemID"].Value.ToString());
                        ProductionID = Convert.ToInt32(dgv_Prod_PlanItems.Rows[e.RowIndex].Cells["Prod_PlanID"].Value.ToString());
                        Prod_Press = dgv_Prod_PlanItems.Rows[e.RowIndex].Cells["Prod_Press"].Value.ToString();
                        Weigh_Manual = false;
                        this.Hide();

                        Form frm = new frmGtWeigh();
                        frm.Location = new Point(0, 0);
                        frm.MdiParent = this.MdiParent;
                        frm.WindowState = FormWindowState.Maximized;
                        frm.Show();
                    }
                    else if (dgv_Prod_PlanItems.Columns[e.ColumnIndex].Name == "btn_weightTrackManual")
                    {
                        Prod_ItemID = Convert.ToInt32(dgv_Prod_PlanItems.Rows[e.RowIndex].Cells["Prod_ItemID"].Value.ToString());
                        ProductionID = Convert.ToInt32(dgv_Prod_PlanItems.Rows[e.RowIndex].Cells["Prod_PlanID"].Value.ToString());
                        Prod_Press = dgv_Prod_PlanItems.Rows[e.RowIndex].Cells["Prod_Press"].Value.ToString();
                        Weigh_Manual = true;
                        this.Hide();

                        Form frm = new frmGtWeigh();
                        frm.Location = new Point(0, 0);
                        frm.MdiParent = this.MdiParent;
                        frm.WindowState = FormWindowState.Maximized;
                        frm.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void frmWeighPlan_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F8 && Program.boolManualWeighing)
                {
                    if (dgv_Prod_PlanItems.Columns.Count == 16)
                    {
                        DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
                        dgv_Prod_PlanItems.Columns.Add(btn1);
                        btn1.HeaderText = "MANUAL";
                        btn1.Text = "MANUAL";
                        btn1.Name = "btn_weightTrackManual";
                        btn1.UseColumnTextForButtonValue = true;

                        foreach (DataGridViewRow rGvc in dgv_Prod_PlanItems.Rows)
                        {
                            if ((rGvc.Cells[0].Value).ToString() == "0" && (rGvc.Cells[1].Value).ToString() == "0")
                            {
                                DataGridViewTextBoxCell txtcell = new DataGridViewTextBoxCell();
                                rGvc.Cells[16] = txtcell;
                            }
                        }
                    }
                }
                else if (e.KeyCode == Keys.F8)
                    MessageBox.Show("MANUAL ENTRY PRIVILEGE DISABLED. CONTACT YOUR SUPERVISOR.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (e.KeyCode == Keys.F9)
                {
                    btnTrack.Visible = true;
                    btnFromSeq.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboPress_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboTyreSize.DataSource = null;
                cboType.DataSource = null;
                cboBrand.DataSource = null;
                dgv_Prod_PlanItems.DataSource = null;
                dgv_Prod_PlanItems.Columns.Clear();

                if (cboPress.SelectedIndex > 0)
                {
                    DataView view = new DataView(dtPlantItem);
                    view.RowFilter = "Prod_Press='" + cboPress.SelectedValue.ToString() + "'";
                    view.Sort = "tyresize ASC";
                    DataTable dtSize = view.ToTable(true, "tyresize");

                    DataRow dr = dtSize.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtSize.Rows.InsertAt(dr, 0);
                    cboTyreSize.DataSource = dtSize;
                    cboTyreSize.DisplayMember = "tyresize";
                    cboTyreSize.ValueMember = "tyresize";
                    if (cboTyreSize.Items.Count == 2)
                        cboTyreSize.SelectedIndex = 1;
                    else
                        cbo_SelectIndex();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboTyreSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboType.DataSource = null;
                cboBrand.DataSource = null;
                dgv_Prod_PlanItems.DataSource = null;
                dgv_Prod_PlanItems.Columns.Clear();

                if (cboTyreSize.SelectedIndex > 0)
                {
                    DataView view = new DataView(dtPlantItem);
                    view.RowFilter = "Prod_Press='" + cboPress.SelectedValue.ToString() + "' AND tyresize='" + cboTyreSize.SelectedValue.ToString() + "'";
                    view.Sort = "tyretype ASC";
                    DataTable dtType = view.ToTable(true, "tyretype");

                    DataRow dr = dtType.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtType.Rows.InsertAt(dr, 0);
                    cboType.DataSource = dtType;
                    cboType.DisplayMember = "tyretype";
                    cboType.ValueMember = "tyretype";
                    if (cboType.Items.Count == 2)
                        cboType.SelectedIndex = 1;
                    else
                        cbo_SelectIndex();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboBrand.DataSource = null;
                dgv_Prod_PlanItems.DataSource = null;
                dgv_Prod_PlanItems.Columns.Clear();

                if (cboType.SelectedIndex > 0)
                {
                    DataView view = new DataView(dtPlantItem);
                    view.RowFilter = "Prod_Press='" + cboPress.SelectedValue.ToString() + "' AND tyresize='" + cboTyreSize.SelectedValue.ToString() + "' AND tyretype='" +
                        cboType.SelectedValue.ToString() + "'";
                    view.Sort = "tyretype ASC";
                    DataTable dtBrand = view.ToTable(true, "brand");

                    DataRow dr = dtBrand.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtBrand.Rows.InsertAt(dr, 0);
                    cboBrand.DataSource = dtBrand;
                    cboBrand.DisplayMember = "brand";
                    cboBrand.ValueMember = "brand";
                    if (cboBrand.Items.Count == 2)
                        cboBrand.SelectedIndex = 1;
                    else
                        cbo_SelectIndex();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgv_Prod_PlanItems.DataSource = null;
                dgv_Prod_PlanItems.Columns.Clear();

                cbo_SelectIndex();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cbo_SelectIndex()
        {
            try
            {
                if (cboBrand.SelectedIndex > 0 || cboType.SelectedIndex > 0 || cboTyreSize.SelectedIndex > 0 || cboPress.SelectedIndex > 0)
                {
                    string strFilter = cboPress.SelectedIndex > 0 ? "Prod_Press='" + cboPress.SelectedValue.ToString() + "'" : "";
                    strFilter += (strFilter != "" && cboTyreSize.SelectedIndex > 0 ? " AND " : "") + (cboTyreSize.SelectedIndex > 0 ? "tyresize='" + cboTyreSize.SelectedValue.ToString() + "'" : "");
                    strFilter += (strFilter != "" && cboType.SelectedIndex > 0 ? " AND " : "") + (cboType.SelectedIndex > 0 ? "tyretype='" + cboType.SelectedValue.ToString() + "'" : "");
                    strFilter += (strFilter != "" && cboBrand.SelectedIndex > 0 ? " AND " : "") + (cboBrand.SelectedIndex > 0 ? "brand='" + cboBrand.SelectedValue.ToString() + "'" : "");
                    DataView dv_Brand = new DataView(dtPlantItem);
                    dv_Brand.RowFilter = strFilter;
                    dv_Brand.Sort = Program.strPlantName == "PDK" ? "Prod_MouldID ASC" : "processid ASC";
                    DataTable dt_Brand = dv_Brand.ToTable(true);
                    Bind_Gv(dt_Brand);
                }
                else if (cboBrand.SelectedIndex == 0 && cboType.SelectedIndex == 0 && cboTyreSize.SelectedIndex == 0 && cboPress.SelectedIndex == 0)
                    Bind_Gv(dtPlantItem);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnTrack_Click(object sender, EventArgs e)
        {
            Form frm = new frmWeighSelectManual();
            frm.Location = new Point(0, 0);
            frm.MdiParent = this.MdiParent;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }
        private void btnFromSeq_Click(object sender, EventArgs e)
        {
            Form frm = new frmGtNextSequence();
            frm.Location = new Point(0, 0);
            frm.MdiParent = this.MdiParent;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }
    }
}
