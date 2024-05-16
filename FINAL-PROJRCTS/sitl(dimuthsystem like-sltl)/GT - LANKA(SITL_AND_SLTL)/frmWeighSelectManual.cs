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
    public partial class frmWeighSelectManual : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dtUnAssign = new DataTable();
        DataTable dtQty = new DataTable();
        public frmWeighSelectManual()
        {
            InitializeComponent();
        }
        private void frmWeighSelectManual_Load(object sender, EventArgs e)
        {
            try
            {
                pnlMain.Location = new Point(0, 20);
                SqlParameter[] spList = new SqlParameter[] { new SqlParameter("@NPStatus", 3), new SqlParameter("@Plant", Program.strPlantName) };
                dtUnAssign = (DataTable)dba.ExecuteReader_SP("sp_sel_production_unassign_list_v2", spList, DBAccess.Return_Type.DataTable);
                if (dtUnAssign.Rows.Count > 0)
                {
                    dtUnAssign.DefaultView.Sort = "tyresize ASC";
                    DataTable dtSize = dtUnAssign.DefaultView.ToTable(true, "tyresize");
                    if (dtSize.Rows.Count > 0)
                    {
                        DataRow toInsert = dtSize.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dtSize.Rows.InsertAt(toInsert, 0);

                        cboSize.DataSource = dtSize;
                        cboSize.DisplayMember = "tyresize";
                        cboSize.ValueMember = "tyresize";

                        if (cboSize.Items.Count == 2)
                            cboSize.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighSelectManual", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboRim.DataSource = null;
                cboType.DataSource = null;
                cboBrand.DataSource = null;
                cboSidewall.DataSource = null;
                cboPlatform.DataSource = null;
                dgv_Prod_Weigh_Manual.DataSource = null;
                dgv_Prod_Weigh_Manual.Columns.Clear();
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dgvMouldAssign.Columns.Clear();
                dtQty.Rows.Clear();
                if (cboSize.SelectedIndex > 0)
                {
                    if (dtUnAssign.Rows.Count > 0)
                    {
                        dtUnAssign.DefaultView.RowFilter = "tyresize='" + cboSize.SelectedValue.ToString() + "'";
                        dtUnAssign.DefaultView.Sort = "rimsize ASC";
                        DataTable dtRim = dtUnAssign.DefaultView.ToTable(true, "rimsize");
                        if (dtRim.Rows.Count > 0)
                        {
                            DataRow toInsert = dtRim.NewRow();
                            toInsert.ItemArray = new object[] { "CHOOSE" };
                            dtRim.Rows.InsertAt(toInsert, 0);

                            cboRim.DataSource = dtRim;
                            cboRim.DisplayMember = "rimsize";
                            cboRim.ValueMember = "rimsize";

                            if (cboRim.Items.Count == 2)
                                cboRim.SelectedIndex = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighSelectManual", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboRim_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboType.DataSource = null;
                cboBrand.DataSource = null;
                cboSidewall.DataSource = null;
                cboPlatform.DataSource = null;
                dgv_Prod_Weigh_Manual.DataSource = null;
                dgv_Prod_Weigh_Manual.Columns.Clear();
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dgvMouldAssign.Columns.Clear();
                dtQty.Rows.Clear();
                if (cboRim.SelectedIndex > 0)
                {
                    if (dtUnAssign.Rows.Count > 0)
                    {
                        dtUnAssign.DefaultView.RowFilter = "tyresize='" + cboSize.SelectedValue.ToString() + "' and rimsize='" + cboRim.SelectedValue.ToString() + "'";
                        dtUnAssign.DefaultView.Sort = "tyretype ASC";
                        DataTable dtType = dtUnAssign.DefaultView.ToTable(true, "tyretype");
                        if (dtType.Rows.Count > 0)
                        {
                            DataRow toInsert = dtType.NewRow();
                            toInsert.ItemArray = new object[] { "CHOOSE" };
                            dtType.Rows.InsertAt(toInsert, 0);

                            cboType.DataSource = dtType;
                            cboType.DisplayMember = "tyretype";
                            cboType.ValueMember = "tyretype";

                            if (cboType.Items.Count == 2)
                                cboType.SelectedIndex = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighSelectManual", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboBrand.DataSource = null;
                cboSidewall.DataSource = null;
                cboPlatform.DataSource = null;
                dgv_Prod_Weigh_Manual.DataSource = null;
                dgv_Prod_Weigh_Manual.Columns.Clear();
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dgvMouldAssign.Columns.Clear();
                dtQty.Rows.Clear();
                if (cboType.SelectedIndex > 0)
                {
                    if (dtUnAssign.Rows.Count > 0)
                    {
                        dtUnAssign.DefaultView.RowFilter = "tyresize='" + cboSize.SelectedValue.ToString() + "' and rimsize='" + cboRim.SelectedValue.ToString() +
                            "' and tyretype='" + cboType.SelectedValue.ToString() + "'";
                        dtUnAssign.DefaultView.Sort = "brand ASC";
                        DataTable dtBrand = dtUnAssign.DefaultView.ToTable(true, "brand");
                        if (dtBrand.Rows.Count > 0)
                        {
                            DataRow toInsert = dtBrand.NewRow();
                            toInsert.ItemArray = new object[] { "CHOOSE" };
                            dtBrand.Rows.InsertAt(toInsert, 0);

                            cboBrand.DataSource = dtBrand;
                            cboBrand.DisplayMember = "brand";
                            cboBrand.ValueMember = "brand";

                            if (cboBrand.Items.Count == 2)
                                cboBrand.SelectedIndex = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighSelectManual", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboSidewall.DataSource = null;
                cboPlatform.DataSource = null;
                dgv_Prod_Weigh_Manual.DataSource = null;
                dgv_Prod_Weigh_Manual.Columns.Clear();
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dgvMouldAssign.Columns.Clear();
                dtQty.Rows.Clear();
                if (cboBrand.SelectedIndex > 0)
                {
                    if (dtUnAssign.Rows.Count > 0)
                    {
                        dtUnAssign.DefaultView.RowFilter = "tyresize='" + cboSize.SelectedValue.ToString() + "' and rimsize='" + cboRim.SelectedValue.ToString() +
                            "' and tyretype='" + cboType.SelectedValue.ToString() + "' and brand='" + cboBrand.SelectedValue.ToString() + "'";
                        dtUnAssign.DefaultView.Sort = "sidewall ASC";
                        DataTable dtSidewall = dtUnAssign.DefaultView.ToTable(true, "sidewall");
                        if (dtSidewall.Rows.Count > 0)
                        {
                            DataRow toInsert = dtSidewall.NewRow();
                            toInsert.ItemArray = new object[] { "CHOOSE" };
                            dtSidewall.Rows.InsertAt(toInsert, 0);

                            cboSidewall.DataSource = dtSidewall;
                            cboSidewall.DisplayMember = "sidewall";
                            cboSidewall.ValueMember = "sidewall";

                            if (cboSidewall.Items.Count == 2)
                                cboSidewall.SelectedIndex = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighSelectManual", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboSidewall_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboPlatform.DataSource = null;
                dgv_Prod_Weigh_Manual.DataSource = null;
                dgv_Prod_Weigh_Manual.Columns.Clear();
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dgvMouldAssign.Columns.Clear();
                dtQty.Rows.Clear();
                if (cboSidewall.SelectedIndex > 0)
                {
                    if (dtUnAssign.Rows.Count > 0)
                    {
                        dtUnAssign.DefaultView.RowFilter = "tyresize='" + cboSize.SelectedValue.ToString() + "' and rimsize='" + cboRim.SelectedValue.ToString() +
                            "' and tyretype='" + cboType.SelectedValue.ToString() + "' and brand='" + cboBrand.SelectedValue.ToString() + "' and sidewall='" +
                            cboSidewall.SelectedValue.ToString() + "'";
                        dtUnAssign.DefaultView.Sort = "Config ASC";
                        DataTable dtConfig = dtUnAssign.DefaultView.ToTable(true, "Config");
                        if (dtConfig.Rows.Count > 0)
                        {
                            DataRow toInsert = dtConfig.NewRow();
                            toInsert.ItemArray = new object[] { "CHOOSE" };
                            dtConfig.Rows.InsertAt(toInsert, 0);

                            cboPlatform.DataSource = dtConfig;
                            cboPlatform.DisplayMember = "Config";
                            cboPlatform.ValueMember = "Config";

                            if (cboPlatform.Items.Count == 2)
                                cboPlatform.SelectedIndex = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighSelectManual", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgv_Prod_Weigh_Manual.DataSource = null;
                dgv_Prod_Weigh_Manual.Columns.Clear();
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dgvMouldAssign.Columns.Clear();
                dtQty.Rows.Clear();
                if (cboPlatform.SelectedIndex > 0)
                {
                    if (dtUnAssign.Rows.Count > 0)
                    {
                        dtUnAssign.DefaultView.RowFilter = "tyresize='" + cboSize.SelectedValue.ToString() + "' and rimsize='" + cboRim.SelectedValue.ToString() +
                            "' and tyretype='" + cboType.SelectedValue.ToString() + "' and brand='" + cboBrand.SelectedValue.ToString() + "' and sidewall='" +
                            cboSidewall.SelectedValue.ToString() + "' and Config='" + cboPlatform.SelectedValue.ToString() + "'";
                        dtUnAssign.DefaultView.Sort = "Prod_ItemID ASC";
                        DataTable dtData = dtUnAssign.DefaultView.ToTable(true, "Prod_ItemID", "WorkOrderNo", "RequiredQuantity", "RemainingQuantity");
                        if (dtData.Rows.Count > 0)
                        {
                            dgv_Prod_Weigh_Manual.DataSource = dtData;
                            
                            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                            checkBoxColumn.Width = 30;
                            checkBoxColumn.HeaderText = "";
                            checkBoxColumn.Name = "checkBoxColumn";
                            checkBoxColumn.Selected = false;
                            dgv_Prod_Weigh_Manual.Columns.Insert(0, checkBoxColumn);
                            dgv_Prod_Weigh_Manual.Columns[0].Width = 30;
                            dgv_Prod_Weigh_Manual.Columns[0].ReadOnly = false;

                            dgv_Prod_Weigh_Manual.Columns[1].ReadOnly = true;
                            dgv_Prod_Weigh_Manual.Columns[1].Visible = false;
                            dgv_Prod_Weigh_Manual.Columns[2].ReadOnly = true;
                            dgv_Prod_Weigh_Manual.Columns[2].HeaderText = "WORK ORDER";
                            dgv_Prod_Weigh_Manual.Columns[3].ReadOnly = true;
                            dgv_Prod_Weigh_Manual.Columns[3].HeaderText = "REQ QTY";
                            dgv_Prod_Weigh_Manual.Columns[4].ReadOnly = true;
                            dgv_Prod_Weigh_Manual.Columns[4].HeaderText = "TO BE PRODUCE";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighSelectManual", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dgv_Prod_Weigh_Manual_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                lblSlotCount.Text = "";
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dtQty.Rows.Clear();

                if (e.RowIndex >= 0 && e.ColumnIndex == 0)
                {
                    foreach (DataGridViewRow gRow in dgv_Prod_Weigh_Manual.Rows)
                    {
                        for (int colIndex = 1; colIndex < dgv_Prod_Weigh_Manual.Columns.Count; colIndex++)
                        {
                            gRow.Cells[colIndex].Style.BackColor = System.Drawing.Color.White;
                        }
                        if (gRow.Index == e.RowIndex)
                        {
                            gRow.Cells["checkBoxColumn"].Value = !Convert.ToBoolean(gRow.Cells["checkBoxColumn"].EditedFormattedValue);
                            if (gRow.Cells["checkBoxColumn"].Value != null && !Convert.ToBoolean(gRow.Cells["checkBoxColumn"].Value))
                            {
                                for (int colIndex = 1; colIndex < dgv_Prod_Weigh_Manual.Columns.Count; colIndex++)
                                {
                                    gRow.Cells[colIndex].Style.BackColor = System.Drawing.Color.LightGreen;
                                }
                            }
                        }
                        else
                            gRow.Cells["checkBoxColumn"].Value = false;
                    }

                    DataGridViewRow row = dgv_Prod_Weigh_Manual.Rows[e.RowIndex];
                    row.Cells["checkBoxColumn"].Value = Convert.ToBoolean(row.Cells["checkBoxColumn"].EditedFormattedValue);
                    if (row.Cells["checkBoxColumn"].Value != null && Convert.ToBoolean(row.Cells["checkBoxColumn"].Value))
                    {
                        lblProdItem.Text = row.Cells[1].Value.ToString();
                        lblWo.Text = row.Cells[2].Value.ToString();
                        lblReqQty.Text = row.Cells[4].Value.ToString();

                        Bind_MouldAssignDetails();

                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TyreSize", cboSize.SelectedValue.ToString()) };
                        DataTable dtPress = (DataTable)dba.ExecuteReader_SP("sp_sel_PressList_SizeWise", sp, DBAccess.Return_Type.DataTable);
                        if (dtPress.Rows.Count > 0)
                        {
                            dgv_AvalPress.DataSource = dtPress;
                            dgv_AvalPress.Columns[0].ReadOnly = true;
                            dgv_AvalPress.Columns[0].HeaderText = "CURING UNIT";
                            dgv_AvalPress.Columns[0].Width = 120;
                            dgv_AvalPress.Columns[1].ReadOnly = true;
                            dgv_AvalPress.Columns[1].HeaderText = "MAX SLOT";
                            dgv_AvalPress.Columns[1].Width = 30;
                            dgv_AvalPress.Columns[2].ReadOnly = true;
                            dgv_AvalPress.Columns[2].HeaderText = "ASSIGNED SLOT";
                            dgv_AvalPress.Columns[2].Width = 90;

                            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                            checkBoxColumn.Width = 30;
                            checkBoxColumn.HeaderText = "";
                            checkBoxColumn.Name = "checkBoxColumn";
                            checkBoxColumn.Selected = false;
                            dgv_AvalPress.Columns.Insert(dgv_AvalPress.Columns.Count, checkBoxColumn);
                            dgv_AvalPress.Columns[dgv_AvalPress.Columns.Count - 1].Width = 30;
                            dgv_AvalPress.Columns[dgv_AvalPress.Columns.Count - 1].ReadOnly = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmProdPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_MouldAssignDetails()
        {
            try
            {
                SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Prod_ItemID", lblProdItem.Text) };
                dtQty = (DataTable)dba.ExecuteReader_SP("sp_sel_newProductionPlanList_itemwise_v1", spSel, DBAccess.Return_Type.DataTable);
                if (dtQty.Rows.Count > 0)
                {
                    dgvMouldAssign.DataSource = dtQty;
                    dgvMouldAssign.Columns[0].ReadOnly = true;
                    dgvMouldAssign.Columns[0].Visible = false;
                    dgvMouldAssign.Columns[1].ReadOnly = true;
                    dgvMouldAssign.Columns[1].HeaderText = "MOULD";
                    dgvMouldAssign.Columns[1].Width = 60;
                    dgvMouldAssign.Columns[2].ReadOnly = true;
                    dgvMouldAssign.Columns[2].HeaderText = "QTY";
                    dgvMouldAssign.Columns[2].Width = 40;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmProdPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dgv_AvalPress_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblSlotCount.Text = "";
                grpPnlMouldCode.Controls.Clear();

                if (e.RowIndex >= 0 && e.ColumnIndex == 3)
                {
                    foreach (DataGridViewRow gRow in dgv_AvalPress.Rows)
                    {
                        if (gRow.Index == e.RowIndex)
                            gRow.Cells["checkBoxColumn"].Value = !Convert.ToBoolean(gRow.Cells["checkBoxColumn"].EditedFormattedValue);
                        else
                            gRow.Cells["checkBoxColumn"].Value = false;
                    }

                    Bind_MouldAssignDetails();

                    DataGridViewRow row = dgv_AvalPress.Rows[e.RowIndex];
                    row.Cells["checkBoxColumn"].Value = Convert.ToBoolean(row.Cells["checkBoxColumn"].EditedFormattedValue);
                    if (row.Cells["checkBoxColumn"].Value != null && Convert.ToBoolean(row.Cells["checkBoxColumn"].Value))
                    {
                        lblPress.Text = row.Cells[0].Value.ToString();
                        lblRemainSlot.Text = (Convert.ToInt32(row.Cells[1].Value) - Convert.ToInt32(row.Cells[2].Value)).ToString();

                        SqlParameter[] sp = new SqlParameter[] { 
                            new SqlParameter("@Prod_ItemID", Convert.ToInt32(lblProdItem.Text)),
                            new SqlParameter("@Prod_Press", lblPress.Text)
                        };
                        DataTable dtMould = (DataTable)dba.ExecuteReader_SP("sp_sel_mouldcode_for_allocation_v2", sp, DBAccess.Return_Type.DataTable);
                        if (dtMould.Rows.Count > 0)
                        {
                            int maxHeight = grpPnlMouldCode.Height - 20;
                            int maxWidth = grpPnlMouldCode.Width - 20;
                            int curHeight = 0;
                            int curWidth = 5;

                            for (int i = 0; i < dtMould.Rows.Count; i++)
                            {
                                CheckBox chk = new CheckBox();
                                chk.Text = dtMould.Rows[i]["MouldCode"].ToString();
                                if (i > 0) curHeight += 30;
                                if (curHeight >= maxHeight)
                                {
                                    curWidth += 120;
                                    curHeight = 0;
                                }
                                chk.Location = new Point(curWidth, curHeight);
                                chk.Enabled = Convert.ToBoolean(dtMould.Rows[i]["masterstatus"].ToString());
                                chk.CheckedChanged += new EventHandler(checkedMouldEntry);
                                grpPnlMouldCode.Controls.Add(chk);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmProdPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void checkedMouldEntry(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                if (chk.Checked)
                {
                    if (Convert.ToInt32(lblRemainSlot.Text) > 0)
                    {
                        dtQty.Rows.Add(lblPress.Text, chk.Text, 0);
                        lblRemainSlot.Text = (Convert.ToInt32(lblRemainSlot.Text) - 1).ToString();
                    }
                    else
                    {
                        MessageBox.Show(lblPress.Text + " MOULD SLOT ALREADY FULL");
                        chk.Checked = false;
                    }
                }
                else if (!chk.Checked)
                {
                    foreach (DataRow qRow in dtQty.Select("MOULD='" + chk.Text + "'"))
                    {
                        qRow.Delete();
                        lblRemainSlot.Text = (Convert.ToInt32(lblRemainSlot.Text) + 1).ToString();
                    }
                }
                if (dtQty.Rows.Count > 0)
                {
                    int modVal = (Convert.ToInt32(lblReqQty.Text) % dtQty.Rows.Count);
                    int intQty = (Convert.ToInt32(lblReqQty.Text) / dtQty.Rows.Count);

                    for (int z = 0; z < dtQty.Rows.Count; z++)
                        dtQty.Rows[z]["QTY"] = intQty;
                    for (int y = 0; y < modVal; y++)
                        dtQty.Rows[y]["QTY"] = (Convert.ToInt32(dtQty.Rows[y]["QTY"].ToString()) + 1);

                    foreach (DataRow dRow in dtQty.Select("QTY=0"))
                    {
                        MessageBox.Show("CHOOSE THE MOULD LESS THAN OR EQUAL TO REQ QTY \nMOULD CODE " + dRow["MOULD"].ToString() + " REMOVED");
                        dRow.Delete();
                        chk.Checked = false;
                    }
                    lblSlotCount.Text = "YOU ARE ASSIGNED " + dtQty.Rows.Count + " MOULD";

                    dgvMouldAssign.DataSource = dtQty;
                    dgvMouldAssign.Columns[0].ReadOnly = true;
                    dgvMouldAssign.Columns[0].Visible = false;
                    dgvMouldAssign.Columns[1].ReadOnly = true;
                    dgvMouldAssign.Columns[1].HeaderText = "MOULD";
                    dgvMouldAssign.Columns[1].Width = 60;
                    dgvMouldAssign.Columns[2].ReadOnly = true;
                    dgvMouldAssign.Columns[2].HeaderText = "QTY";
                    dgvMouldAssign.Columns[2].Width = 40;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmProdPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtQty == null || dtQty.Rows.Count == 0)
                    MessageBox.Show("NO RECORDS");
                else
                {
                    int resp = 0;
                    foreach (DataRow dRow in dtQty.Rows)
                    {
                        SqlParameter[] spIns = new SqlParameter[] { 
                            new SqlParameter("@Prod_ItemID", lblProdItem.Text), 
                            new SqlParameter("@Prod_ReqQty", dRow["QTY"].ToString()), 
                            new SqlParameter("@Prod_Press", dRow["PRESS"].ToString()), 
                            new SqlParameter("@Prod_MouldID", dRow["MOULD"].ToString()), 
                            new SqlParameter("@PlannedBy", Program.strUserName) 
                        };
                        resp += dba.ExecuteNonQuery_SP("sp_ins_newProductionPlanList", spIns);
                    }
                    if (resp > 0)
                    {
                        this.Hide();

                        Form frm = new frmWeighPlan();
                        frm.MdiParent = this.MdiParent;
                        frm.Location = new Point(0, 0);
                        frm.WindowState = FormWindowState.Maximized;
                        frm.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeighSelectManual", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
