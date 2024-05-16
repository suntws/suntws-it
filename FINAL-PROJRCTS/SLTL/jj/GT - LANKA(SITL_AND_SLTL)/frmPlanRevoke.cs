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
    public partial class frmPlanRevoke : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dtPlanRowID = new DataTable();
        DataTable dtItems = new DataTable();
        public frmPlanRevoke()
        {
            InitializeComponent();
        }
        private void frmPlanRevoke_Load(object sender, EventArgs e)
        {
            try
            {
                cboSize.DataSource = null;
                cboRim.DataSource = null;
                cboType.DataSource = null;
                dgv_ProdPlanItems.DataSource = null;
                dgv_PlannedList.DataSource = null;
                dgv_PlannedList.Columns.Clear();
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_UpdPlan.DataSource = null;
                dgv_PlannedList.DataSource = null;
                dtPlanRowID = new DataTable();
                dtPlanRowID.Columns.Add("rowID", typeof(Int32));
                dtPlanRowID.Columns.Add("tbName", typeof(String));

                dtItems = (DataTable)dba.ExecuteReader_SP("sp_sel_Items_For_AllocationRevoke_v1", DBAccess.Return_Type.DataTable);
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
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboRim.DataSource = null;
                cboType.DataSource = null;
                dgv_ProdPlanItems.DataSource = null;
                dgv_PlannedList.DataSource = null;
                dgv_PlannedList.Columns.Clear();
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_UpdPlan.DataSource = null;
                dgv_PlannedList.DataSource = null;
                dtPlanRowID.Rows.Clear();

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
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboRim_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboType.DataSource = null;
                dgv_ProdPlanItems.DataSource = null;
                dgv_PlannedList.DataSource = null;
                dgv_PlannedList.Columns.Clear();
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_UpdPlan.DataSource = null;
                dtPlanRowID.Rows.Clear();

                if (cboRim.SelectedIndex > 0)
                {
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
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgv_ProdPlanItems.DataSource = null;
                dgv_PlannedList.DataSource = null;
                dgv_PlannedList.Columns.Clear();
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_UpdPlan.DataSource = null;
                dtPlanRowID.Rows.Clear();

                bind_ItemGridView();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_ItemGridView()
        {
            try
            {
                dgv_ProdPlanItems.DataSource = null;
                dgv_ProdPlanItems.Columns.Clear();

                string strFilter = cboSize.SelectedIndex > 0 ? "tyresize='" + cboSize.SelectedValue.ToString() + "'" : "";
                strFilter += (strFilter != "" && cboRim.SelectedIndex > 0 ? " AND " : "") + (cboRim.SelectedIndex > 0 ? "rimsize='" + cboRim.SelectedValue.ToString() + "'" : "");
                strFilter += (strFilter != "" && cboType.SelectedIndex > 0 ? " AND " : "") + (cboType.SelectedIndex > 0 ? "tyretype='" + cboType.SelectedValue.ToString() + "'" : "");
                DataView dv_Items = new DataView(dtItems);
                dv_Items.RowFilter = strFilter;
                DataTable dtFilterItems = dv_Items.ToTable(true);

                if (dtFilterItems.Rows.Count > 0)
                {
                    dgv_ProdPlanItems.DataSource = dtFilterItems;

                    DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                    checkBoxColumn.Width = 30;
                    checkBoxColumn.HeaderText = "";
                    checkBoxColumn.Name = "checkBoxColumn";
                    checkBoxColumn.Selected = false;
                    dgv_ProdPlanItems.Columns.Insert(0, checkBoxColumn);
                    dgv_ProdPlanItems.Columns[0].Width = 30;
                    dgv_ProdPlanItems.Columns[0].ReadOnly = false;

                    dgv_ProdPlanItems.Columns[1].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[1].Visible = false;
                    dgv_ProdPlanItems.Columns[2].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[2].HeaderText = "CUSTOMER";
                    dgv_ProdPlanItems.Columns[2].Width = 150;
                    dgv_ProdPlanItems.Columns[3].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[3].HeaderText = "WORK ORDER";
                    dgv_ProdPlanItems.Columns[3].Width = 130;
                    dgv_ProdPlanItems.Columns[4].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[4].HeaderText = "PLATFORM";
                    dgv_ProdPlanItems.Columns[4].Width = 130;
                    dgv_ProdPlanItems.Columns[5].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[5].HeaderText = "TYRE SIZE";
                    dgv_ProdPlanItems.Columns[5].Width = 130;
                    dgv_ProdPlanItems.Columns[6].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[6].HeaderText = "RIM";
                    dgv_ProdPlanItems.Columns[6].Width = 60;
                    dgv_ProdPlanItems.Columns[7].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[7].HeaderText = "TYPE";
                    dgv_ProdPlanItems.Columns[7].Width = 110;
                    dgv_ProdPlanItems.Columns[8].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[8].HeaderText = "BRAND";
                    dgv_ProdPlanItems.Columns[8].Width = 130;
                    dgv_ProdPlanItems.Columns[9].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[9].HeaderText = "SIDEWALL";
                    dgv_ProdPlanItems.Columns[9].Width = 130;
                    dgv_ProdPlanItems.Columns[10].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[10].HeaderText = "ORDER QTY";
                    dgv_ProdPlanItems.Columns[10].Width = 50;
                    dgv_ProdPlanItems.Columns[11].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[11].HeaderText = "REQ QTY";
                    dgv_ProdPlanItems.Columns[11].Width = 40;
                    dgv_ProdPlanItems.Columns[12].ReadOnly = true;
                    dgv_ProdPlanItems.Columns[12].HeaderText = "BALANCE QTY";
                    dgv_ProdPlanItems.Columns[12].Width = 75;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dgv_ProdPlanItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblWo.Text = "";
                lblReqQty.Text = "";
                dgv_PlannedList.DataSource = null;
                dgv_PlannedList.Columns.Clear();
                dgv_UpdPlan.DataSource = null;
                dtPlanRowID.Rows.Clear();

                if (e.RowIndex >= 0 && e.ColumnIndex == 0)
                {
                    foreach (DataGridViewRow gRow in dgv_ProdPlanItems.Rows)
                    {
                        for (int colIndex = 1; colIndex < dgv_ProdPlanItems.Columns.Count; colIndex++)
                        {
                            gRow.Cells[colIndex].Style.BackColor = System.Drawing.Color.White;
                        }
                        if (gRow.Index == e.RowIndex)
                        {
                            gRow.Cells["checkBoxColumn"].Value = !Convert.ToBoolean(gRow.Cells["checkBoxColumn"].EditedFormattedValue);
                            if (gRow.Cells["checkBoxColumn"].Value != null && !Convert.ToBoolean(gRow.Cells["checkBoxColumn"].Value))
                            {
                                for (int colIndex = 1; colIndex < dgv_ProdPlanItems.Columns.Count; colIndex++)
                                {
                                    gRow.Cells[colIndex].Style.BackColor = System.Drawing.Color.LightGreen;
                                }
                            }
                        }
                        else
                            gRow.Cells["checkBoxColumn"].Value = false;
                    }

                    DataGridViewRow row = dgv_ProdPlanItems.Rows[e.RowIndex];
                    row.Cells["checkBoxColumn"].Value = Convert.ToBoolean(row.Cells["checkBoxColumn"].EditedFormattedValue);
                    if (row.Cells["checkBoxColumn"].Value != null && Convert.ToBoolean(row.Cells["checkBoxColumn"].Value))
                    {
                        lblProdItem.Text = row.Cells[1].Value.ToString();
                        lblWo.Text = row.Cells[3].Value.ToString();
                        lblReqQty.Text = row.Cells[12].Value.ToString();

                        SqlParameter[] spList = new SqlParameter[] { new SqlParameter("@Prod_ItemID", lblProdItem.Text) };
                        DataTable dtPlan = (DataTable)dba.ExecuteReader_SP("sp_sel_ProductionPlan_ProdItemIDWise_v1", spList, DBAccess.Return_Type.DataTable);
                        if (dtPlan.Rows.Count > 0)
                        {
                            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
                            checkBoxColumn.Width = 30;
                            checkBoxColumn.HeaderText = "";
                            checkBoxColumn.Name = "checkBoxColumn";
                            checkBoxColumn.Selected = false;
                            dgv_PlannedList.Columns.Insert(0, checkBoxColumn);
                            dgv_PlannedList.Columns[0].Width = 30;
                            dgv_PlannedList.Columns[0].ReadOnly = false;

                            dgv_PlannedList.DataSource = dtPlan;
                            dgv_PlannedList.Columns[1].ReadOnly = true;
                            dgv_PlannedList.Columns[1].Visible = false;
                            dgv_PlannedList.Columns[2].ReadOnly = true;
                            dgv_PlannedList.Columns[2].HeaderText = "WORK ORDER";
                            dgv_PlannedList.Columns[2].Width = 180;
                            dgv_PlannedList.Columns[3].ReadOnly = true;
                            dgv_PlannedList.Columns[3].HeaderText = "TYPE";
                            dgv_PlannedList.Columns[3].Width = 120;
                            dgv_PlannedList.Columns[4].ReadOnly = true;
                            dgv_PlannedList.Columns[4].HeaderText = "PRESS";
                            dgv_PlannedList.Columns[4].Width = 120;
                            dgv_PlannedList.Columns[5].ReadOnly = true;
                            dgv_PlannedList.Columns[5].HeaderText = "MOULD ID";
                            dgv_PlannedList.Columns[5].Width = 100;
                            dgv_PlannedList.Columns[6].ReadOnly = true;
                            dgv_PlannedList.Columns[6].HeaderText = "PLAN QTY";
                            dgv_PlannedList.Columns[6].Width = 60;
                            dgv_PlannedList.Columns[7].ReadOnly = true;
                            dgv_PlannedList.Columns[7].HeaderText = "REMAIN QTY";
                            dgv_PlannedList.Columns[7].Width = 80;
                            dgv_PlannedList.Columns[8].ReadOnly = true;
                            dgv_PlannedList.Columns[8].Visible = false;

                            DataGridViewTextBoxColumn textBoxColumn = new DataGridViewTextBoxColumn();
                            textBoxColumn.Width = 50;
                            textBoxColumn.HeaderText = "MOULD DROP QTY";
                            textBoxColumn.Name = "textBoxColumn";
                            textBoxColumn.MaxInputLength = 3;
                            textBoxColumn.Visible = true;
                            dgv_PlannedList.Columns.Insert(dgv_PlannedList.Columns.Count, textBoxColumn);
                            dgv_PlannedList.Columns[dgv_PlannedList.Columns.Count - 1].Width = 80;
                            dgv_PlannedList.Columns[dgv_PlannedList.Columns.Count - 1].ReadOnly = false;

                            foreach (DataGridViewRow gRow in dgv_PlannedList.Rows)
                            {
                                if (gRow.Cells[7].Value.ToString() == "0")
                                {
                                    gRow.Cells["checkBoxColumn"].ReadOnly = true;
                                    dtPlanRowID.Rows.Add(Convert.ToInt32(gRow.Cells[1].Value.ToString()), gRow.Cells[8].Value.ToString());
                                    for (int colIndex = 0; colIndex < dgv_PlannedList.Columns.Count; colIndex++)
                                    {
                                        gRow.Cells[colIndex].Style.BackColor = System.Drawing.Color.Yellow;
                                    }
                                }
                            }
                            if (dtPlanRowID.Rows.Count > 0)
                            {
                                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@PlanRowID_dt", dtPlanRowID) };
                                DataTable dtPlanList = (DataTable)dba.ExecuteReader_SP("sp_sel_AssignedPressList_RevokeQty_v1", sp, DBAccess.Return_Type.DataTable);

                                Bind_Upd_MouldQty(dtPlanList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dgv_PlannedList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dgv_UpdPlan.DataSource = null;
                if (e.RowIndex >= 0 && e.ColumnIndex == 0)
                {
                    DataGridViewRow row = dgv_PlannedList.Rows[e.RowIndex];
                    row.Cells["checkBoxColumn"].Value = Convert.ToBoolean(row.Cells["checkBoxColumn"].EditedFormattedValue);
                    if (row.Cells["checkBoxColumn"].Value != null && Convert.ToBoolean(row.Cells["checkBoxColumn"].Value))
                    {
                        dtPlanRowID.Rows.Add(Convert.ToInt32(row.Cells[1].Value.ToString()), row.Cells[8].Value.ToString());
                    }
                    else
                    {
                        foreach (DataRow qRow in dtPlanRowID.Select("rowID='" + row.Cells[1].Value.ToString() + "' AND tbName='" + row.Cells[8].Value.ToString() + "'"))
                        {
                            qRow.Delete();
                        }
                    }
                }
                if (dtPlanRowID.Rows.Count > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@PlanRowID_dt", dtPlanRowID) };
                    DataTable dtPlanList = (DataTable)dba.ExecuteReader_SP("sp_sel_AssignedPressList_RevokeQty_v1", sp, DBAccess.Return_Type.DataTable);

                    Bind_Upd_MouldQty(dtPlanList);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dgv_PlannedList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv_PlannedList.Columns[e.ColumnIndex].Name == "textBoxColumn" && dgv_PlannedList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                {
                    int dropCount;
                    if (!int.TryParse(this.dgv_PlannedList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out dropCount) &&
                        this.dgv_PlannedList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                    {
                        MessageBox.Show("Numeric only");
                        this.dgv_PlannedList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    }
                    else if (this.dgv_PlannedList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                    {
                        bool isSelected = Convert.ToBoolean(dgv_PlannedList.Rows[e.RowIndex].Cells["checkBoxColumn"].Value);
                        if (isSelected)
                        {
                            dropCount = Convert.ToInt32(dgv_PlannedList.Rows[e.RowIndex].Cells["textBoxColumn"].Value);
                            int RemainingCount = Convert.ToInt32(dgv_PlannedList.Rows[e.RowIndex].Cells[7].Value);
                            if (dropCount > RemainingCount || dropCount < 0)
                            {
                                MessageBox.Show("Enter the drop qty should be less than the balance quantity");
                                this.dgv_PlannedList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
                            }
                            else
                            {
                                if (dtPlanRowID.Rows.Count > 0)
                                {
                                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@PlanRowID_dt", dtPlanRowID) };
                                    DataTable dtPlanList = (DataTable)dba.ExecuteReader_SP("sp_sel_AssignedPressList_RevokeQty_v1", sp, DBAccess.Return_Type.DataTable);

                                    DataTable dtTempTable = dtPlanList.Clone();

                                    int dropQty = 0;
                                    foreach (DataGridViewRow dRow in dgv_PlannedList.Rows)
                                    {
                                        if (Convert.ToBoolean(dRow.Cells["checkBoxColumn"].Value) && dRow.Cells["textBoxColumn"].Value.ToString() != "")
                                        {
                                            if (Convert.ToInt32(dRow.Cells["textBoxColumn"].Value.ToString()) > 0)
                                            {
                                                dropQty += Convert.ToInt32(dgv_PlannedList.Rows[e.RowIndex].Cells["textBoxColumn"].Value);
                                                dtTempTable.Rows.Add(dRow.Cells[1].Value, dRow.Cells[4].Value, dRow.Cells[5].Value,
                                                    (Convert.ToInt32(dRow.Cells[6].Value) - (Convert.ToInt32(dRow.Cells[7].Value) - Convert.ToInt32(dRow.Cells[9].Value))),
                                                    (Convert.ToInt32(dRow.Cells[6].Value) - Convert.ToInt32(dRow.Cells[7].Value)), Convert.ToInt32(dRow.Cells[9].Value),
                                                    dRow.Cells[8].Value.ToString());

                                                foreach (DataRow qRow in dtPlanRowID.Select("rowID='" + dRow.Cells[1].Value.ToString() + "' AND tbName='" + dRow.Cells[8].Value.ToString() + "'"))
                                                {
                                                    qRow.Delete();
                                                }
                                            }
                                        }
                                    }

                                    if (dtPlanList.Rows.Count > 0)
                                    {
                                        int modVal = dropQty % (Convert.ToInt32(dtPlanList.Rows.Count));
                                        int subQty = Convert.ToInt32(dropQty / (Convert.ToInt32(dtPlanList.Rows.Count)));

                                        if (subQty > 0)
                                        {
                                            foreach (DataRow sRow in dtPlanList.Rows)
                                            {
                                                sRow[3] = (Convert.ToInt32(sRow[3].ToString()) - subQty);
                                                sRow[4] = (Convert.ToInt32(sRow[4].ToString()) - subQty);
                                                sRow[5] = (Convert.ToInt32(sRow[5].ToString()) - subQty);
                                            }
                                        }

                                        if (modVal > 0)
                                        {
                                            for (int k = 0; k < modVal; k++)
                                            {
                                                dtPlanList.Rows[k][3] = (Convert.ToInt32(dtPlanList.Rows[k][3].ToString()) - 1);
                                                dtPlanList.Rows[k][4] = (Convert.ToInt32(dtPlanList.Rows[k][4].ToString()) - 1);
                                                dtPlanList.Rows[k][5] = (Convert.ToInt32(dtPlanList.Rows[k][5].ToString()) - 1);
                                            }
                                        }
                                    }
                                    dtPlanList.Merge(dtTempTable);

                                    Bind_Upd_MouldQty(dtPlanList);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Enter the value in mould drop selected row");
                            this.dgv_PlannedList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Upd_MouldQty(DataTable dtPlanList)
        {
            try
            {
                if (dtPlanList.Rows.Count > 0)
                {
                    dgv_UpdPlan.DataSource = dtPlanList;
                    dgv_UpdPlan.Columns[0].ReadOnly = true;
                    dgv_UpdPlan.Columns[0].Visible = false;
                    dgv_UpdPlan.Columns[1].ReadOnly = true;
                    dgv_UpdPlan.Columns[1].HeaderText = "PRESS";
                    dgv_UpdPlan.Columns[1].Width = 100;
                    dgv_UpdPlan.Columns[2].ReadOnly = true;
                    dgv_UpdPlan.Columns[2].HeaderText = "MOULD";
                    dgv_UpdPlan.Columns[2].Width = 100;
                    dgv_UpdPlan.Columns[3].ReadOnly = true;
                    dgv_UpdPlan.Columns[3].HeaderText = "PLAN QTY";
                    dgv_UpdPlan.Columns[3].Width = 40;
                    dgv_UpdPlan.Columns[4].ReadOnly = true;
                    dgv_UpdPlan.Columns[4].HeaderText = "PRODUCED QTY";
                    dgv_UpdPlan.Columns[4].Width = 80;
                    dgv_UpdPlan.Columns[5].ReadOnly = true;
                    dgv_UpdPlan.Columns[5].HeaderText = "BALANCE QTY";
                    dgv_UpdPlan.Columns[5].Width = 80;
                    dgv_UpdPlan.Columns[6].ReadOnly = true;
                    dgv_UpdPlan.Columns[6].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if ((dgv_UpdPlan != null && dgv_UpdPlan.Rows.Count > 0) || dtPlanRowID.Rows.Count > 0)
                {
                    int resp = 0;
                    foreach (DataGridViewRow gRow in dgv_UpdPlan.Rows)
                    {
                        SqlParameter[] spUpd = new SqlParameter[] { 
                            new SqlParameter("@PlanID", Convert.ToInt32(gRow.Cells[0].Value.ToString())), 
                            new SqlParameter("@Prod_ReqQty", Convert.ToInt32(gRow.Cells[3].Value.ToString())),
                            new SqlParameter("@RevisedBy", Program.strUserName),
                            new SqlParameter("@tbName", gRow.Cells[6].Value.ToString())
                        };
                        resp += dba.ExecuteNonQuery_SP("sp_upd_newProductionPlanList_forRevoke_v1", spUpd);
                    }
                    if (dtPlanRowID.Rows.Count > 0)
                    {
                        SqlParameter[] spStatus = new SqlParameter[] { new SqlParameter("@PlanRowID_dt", dtPlanRowID), new SqlParameter("@RevisedBy", Program.strUserName) };
                        resp += dba.ExecuteNonQuery_SP("sp_upd_newProductionPlanList_HoldStatus_v1", spStatus);
                    }
                    if (resp > 0)
                        frmPlanRevoke_Load(sender, e);
                }
                else
                    MessageBox.Show("NO RECORDS");
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmPlanRevoke", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
