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
    public partial class frmSequencePlan : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dtQty = new DataTable();
        DataTable dtItems = new DataTable();
        string strPress = "";
        public frmSequencePlan()
        {
            InitializeComponent();
            dtQty.Columns.Add("PRESS", typeof(string));
            dtQty.Columns.Add("MOULD", typeof(string));
            dtQty.Columns.Add("QTY", typeof(int));
        }
        private void frmSequencePlan_Load(object sender, EventArgs e)
        {
            try
            {
                lblMouldMsg.Text = "";
                cboPress.DataSource = null;
                cboSize.DataSource = null;
                cboRim.DataSource = null;
                cboType.DataSource = null;
                dgv_OrderItems.DataSource = null;
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                lblSlotCount.Text = "";
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dgv_AllocateList.DataSource = null;
                dtQty.Rows.Clear();

                DataTable dtPressList = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringUnit", DBAccess.Return_Type.DataTable);
                if (dtPressList.Rows.Count > 0)
                {
                    DataRow dr = dtPressList.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtPressList.Rows.InsertAt(dr, 0);
                    cboPress.DataSource = dtPressList;
                    cboPress.DisplayMember = "CuringUnit";
                    cboPress.ValueMember = "CuringUnit";
                    if (cboPress.Items.Count == 2)
                        cboPress.SelectedIndex = 1;
                    else if (strPress != "")
                        cboPress.SelectedIndex = cboPress.FindStringExact(strPress);

                    if (cboPress.SelectedIndex == -1)
                        cboPress.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboPress_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMouldMsg.Text = "";
                cboSize.DataSource = null;
                cboRim.DataSource = null;
                cboType.DataSource = null;
                dgv_OrderItems.DataSource = null;
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                lblSlotCount.Text = "";
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dgv_AllocateList.DataSource = null;
                dtQty.Rows.Clear();

                if (cboPress.SelectedIndex > 0)
                {
                    SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Press", cboPress.SelectedValue.ToString()) };
                    dtItems = (DataTable)dba.ExecuteReader_SP("sp_sel_Items_For_ProdSequence", spSel, DBAccess.Return_Type.DataTable);
                    if (dtItems.Rows.Count > 0)
                    {
                        DataView view = new DataView(dtItems);
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
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMouldMsg.Text = "";
                cboRim.DataSource = null;
                cboType.DataSource = null;
                dgv_OrderItems.DataSource = null;
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                lblSlotCount.Text = "";
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dgv_AllocateList.DataSource = null;
                dtQty.Rows.Clear();

                if (cboSize.SelectedIndex > 0)
                {
                    DataView view = new DataView(dtItems);
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
                        bind_ItemGridView();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboRim_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboType.DataSource = null;
                dgv_OrderItems.DataSource = null;
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                lblSlotCount.Text = "";
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dtQty.Rows.Clear();

                if (cboRim.SelectedIndex > 0)
                {
                    bind_SequencePlan_SizeWise(cboSize.SelectedValue.ToString(), cboRim.SelectedValue.ToString());
                    SqlParameter[] spCount = new SqlParameter[] {
                        new SqlParameter("@tyresize", cboSize.SelectedValue.ToString()),
                        new SqlParameter("@rimsize", cboRim.SelectedValue.ToString()) 
                    };
                    int intMouldCount = (int)dba.ExecuteScalar_SP("sp_sel_MouldCount_sizeWise", spCount);
                    lblMouldMsg.Text = "TOTAL MOULD'S " + intMouldCount.ToString();

                    DataView view = new DataView(dtItems);
                    view.RowFilter = "tyresize='" + cboSize.SelectedValue.ToString() + "' and rimsize='" + cboRim.SelectedValue.ToString() + "'";
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
                        bind_ItemGridView();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_SequencePlan_SizeWise(string strSize, string strRim)
        {
            try
            {
                SqlParameter[] spList = new SqlParameter[] { new SqlParameter("@tyresize", strSize), new SqlParameter("@rimsize", strRim) };
                DataTable dtPlan = (DataTable)dba.ExecuteReader_SP("sp_sel_SequencePlan_SizeWise", spList, DBAccess.Return_Type.DataTable);
                if (dtPlan.Rows.Count > 0)
                {
                    dgv_AllocateList.DataSource = dtPlan;
                    dgv_AllocateList.Columns[0].ReadOnly = true;
                    dgv_AllocateList.Columns[0].Visible = false;
                    dgv_AllocateList.Columns[1].ReadOnly = true;
                    dgv_AllocateList.Columns[1].HeaderText = "WORK ORDER";
                    dgv_AllocateList.Columns[1].Width = 100;
                    dgv_AllocateList.Columns[2].ReadOnly = true;
                    dgv_AllocateList.Columns[2].HeaderText = "TYPE";
                    dgv_AllocateList.Columns[2].Width = 80;
                    dgv_AllocateList.Columns[3].ReadOnly = true;
                    dgv_AllocateList.Columns[3].HeaderText = "PRESS";
                    dgv_AllocateList.Columns[3].Width = 80;
                    dgv_AllocateList.Columns[4].ReadOnly = true;
                    dgv_AllocateList.Columns[4].HeaderText = "MOULD ID";
                    dgv_AllocateList.Columns[4].Width = 80;
                    dgv_AllocateList.Columns[5].ReadOnly = true;
                    dgv_AllocateList.Columns[5].HeaderText = "PLAN QTY";
                    dgv_AllocateList.Columns[5].Width = 60;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgv_OrderItems.DataSource = null;
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_AvalPress.DataSource = null;
                dgv_AvalPress.Columns.Clear();
                lblSlotCount.Text = "";
                grpPnlMouldCode.Controls.Clear();
                dgvMouldAssign.DataSource = null;
                dtQty.Rows.Clear();

                bind_ItemGridView();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_ItemGridView()
        {
            try
            {
                dgv_OrderItems.DataSource = null;
                dgv_OrderItems.Columns.Clear();

                string strFilter = cboSize.SelectedIndex > 0 ? "tyresize='" + cboSize.SelectedValue.ToString() + "'" : "";
                strFilter += (strFilter != "" && cboRim.SelectedIndex > 0 ? " AND " : "") + (cboRim.SelectedIndex > 0 ? "rimsize='" + cboRim.SelectedValue.ToString() + "'" : "");
                strFilter += (strFilter != "" && cboType.SelectedIndex > 0 ? " AND " : "") + (cboType.SelectedIndex > 0 ? "tyretype='" + cboType.SelectedValue.ToString() + "'" : "");
                DataView dv_Items = new DataView(dtItems);
                dv_Items.RowFilter = strFilter;
                DataTable dtFilterItems = dv_Items.ToTable(true);

                if (dtFilterItems.Rows.Count > 0)
                {
                    dgv_OrderItems.DataSource = dtFilterItems;

                    DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                    checkBoxColumn.Width = 30;
                    checkBoxColumn.HeaderText = "";
                    checkBoxColumn.Name = "checkBoxColumn";
                    checkBoxColumn.Selected = false;
                    dgv_OrderItems.Columns.Insert(0, checkBoxColumn);
                    dgv_OrderItems.Columns[0].Width = 30;
                    dgv_OrderItems.Columns[0].ReadOnly = false;

                    dgv_OrderItems.Columns[1].ReadOnly = true;
                    dgv_OrderItems.Columns[1].Visible = false;
                    dgv_OrderItems.Columns[2].ReadOnly = true;
                    dgv_OrderItems.Columns[2].HeaderText = "CUSTOMER";
                    dgv_OrderItems.Columns[2].Width = 150;
                    dgv_OrderItems.Columns[3].ReadOnly = true;
                    dgv_OrderItems.Columns[3].HeaderText = "WORK ORDER";
                    dgv_OrderItems.Columns[3].Width = 130;
                    dgv_OrderItems.Columns[4].ReadOnly = true;
                    dgv_OrderItems.Columns[4].HeaderText = "PLATFORM";
                    dgv_OrderItems.Columns[4].Width = 130;
                    dgv_OrderItems.Columns[5].ReadOnly = true;
                    dgv_OrderItems.Columns[5].HeaderText = "TYRE SIZE";
                    dgv_OrderItems.Columns[5].Width = 130;
                    dgv_OrderItems.Columns[6].ReadOnly = true;
                    dgv_OrderItems.Columns[6].HeaderText = "RIM";
                    dgv_OrderItems.Columns[6].Width = 60;
                    dgv_OrderItems.Columns[7].ReadOnly = true;
                    dgv_OrderItems.Columns[7].HeaderText = "TYPE";
                    dgv_OrderItems.Columns[7].Width = 110;
                    dgv_OrderItems.Columns[8].ReadOnly = true;
                    dgv_OrderItems.Columns[8].HeaderText = "BRAND";
                    dgv_OrderItems.Columns[8].Width = 130;
                    dgv_OrderItems.Columns[9].ReadOnly = true;
                    dgv_OrderItems.Columns[9].HeaderText = "SIDEWALL";
                    dgv_OrderItems.Columns[9].Width = 130;
                    dgv_OrderItems.Columns[10].ReadOnly = true;
                    dgv_OrderItems.Columns[10].HeaderText = "REQ QTY";
                    dgv_OrderItems.Columns[10].Width = 40;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dgv_OrderItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                    foreach (DataGridViewRow gRow in dgv_OrderItems.Rows)
                    {
                        for (int colIndex = 1; colIndex < dgv_OrderItems.Columns.Count; colIndex++)
                        {
                            gRow.Cells[colIndex].Style.BackColor = System.Drawing.Color.White;
                        }
                        if (gRow.Index == e.RowIndex)
                        {
                            gRow.Cells["checkBoxColumn"].Value = !Convert.ToBoolean(gRow.Cells["checkBoxColumn"].EditedFormattedValue);
                            if (gRow.Cells["checkBoxColumn"].Value != null && !Convert.ToBoolean(gRow.Cells["checkBoxColumn"].Value))
                            {
                                for (int colIndex = 1; colIndex < dgv_OrderItems.Columns.Count; colIndex++)
                                {
                                    gRow.Cells[colIndex].Style.BackColor = System.Drawing.Color.LightGreen;
                                }
                            }
                        }
                        else
                            gRow.Cells["checkBoxColumn"].Value = false;
                    }

                    DataGridViewRow row = dgv_OrderItems.Rows[e.RowIndex];
                    row.Cells["checkBoxColumn"].Value = Convert.ToBoolean(row.Cells["checkBoxColumn"].EditedFormattedValue);
                    if (row.Cells["checkBoxColumn"].Value != null && Convert.ToBoolean(row.Cells["checkBoxColumn"].Value))
                    {
                        lblProdItem.Text = row.Cells[1].Value.ToString();
                        lblWo.Text = row.Cells[3].Value.ToString();
                        lblReqQty.Text = row.Cells[10].Value.ToString();

                        if (dgv_AllocateList.Rows.Count <= 0)
                            bind_SequencePlan_SizeWise(row.Cells[5].Value.ToString(), row.Cells[6].Value.ToString());

                        Bind_MouldAssignDetails();

                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TyreSize", cboSize.SelectedValue.ToString()) };
                        DataTable dtPress = (DataTable)dba.ExecuteReader_SP("sp_sel_PressList_SizeWise_sequence", sp, DBAccess.Return_Type.DataTable);
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

                            foreach (DataGridViewRow gRow in dgv_AvalPress.Rows)
                            {
                                if (gRow.Cells[0].Value.ToString() == cboPress.SelectedValue.ToString())
                                {
                                    gRow.Cells["checkBoxColumn"].Value = true;

                                    lblPress.Text = gRow.Cells[0].Value.ToString();

                                    SqlParameter[] sp0 = new SqlParameter[] { 
                                        new SqlParameter("@Prod_ItemID", Convert.ToInt32(lblProdItem.Text)),
                                        new SqlParameter("@Prod_Press", lblPress.Text)
                                    };
                                    DataTable dtMould = (DataTable)dba.ExecuteReader_SP("sp_sel_mouldcode_for_sequence", sp0, DBAccess.Return_Type.DataTable);
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
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_MouldAssignDetails()
        {
            try
            {
                dgvMouldAssign.DataSource = null;
                SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Prod_ItemID", lblProdItem.Text) };
                dtQty = (DataTable)dba.ExecuteReader_SP("sp_sel_newProductionSequence", spSel, DBAccess.Return_Type.DataTable);
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

        }
        private void checkedMouldEntry(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                if (chk.Checked)
                {
                    foreach (DataRow qRow in dtQty.Select("MOULD='" + chk.Text + "'"))
                    {
                        qRow.Delete();
                    }
                    dtQty.Rows.Add(lblPress.Text, chk.Text, 0);
                }
                else if (!chk.Checked)
                {
                    foreach (DataRow qRow in dtQty.Select("MOULD='" + chk.Text + "'"))
                    {
                        qRow.Delete();
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
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
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
                            new SqlParameter("@SequencedBy", Program.strUserName) 
                        };
                        resp += dba.ExecuteNonQuery_SP("sp_ins_newProductionSequence", spIns);
                    }
                    if (resp > 0)
                    {
                        strPress = cboPress.SelectedValue.ToString();
                        frmSequencePlan_Load(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmSequencePlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
