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
    public partial class frmGtNextSequence : Form
    {
        DBAccess dba = new DBAccess();
        public frmGtNextSequence()
        {
            InitializeComponent();
        }
        private void frmGtNextSequence_Load(object sender, EventArgs e)
        {
            try
            {
                pnlSequence.Location = new Point(0, 20);
                cboSeqUnit.DataSource = null;
                dgvNextSequence.DataSource = null;
                DataTable dtPress = (DataTable)dba.ExecuteReader_SP("sp_sel_newProductionSequence_Press", DBAccess.Return_Type.DataTable);
                if (dtPress.Rows.Count > 0)
                {
                    DataRow dr = dtPress.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtPress.Rows.InsertAt(dr, 0);

                    cboSeqUnit.DataSource = dtPress;
                    cboSeqUnit.DisplayMember = "Prod_Press";
                    cboSeqUnit.ValueMember = "Prod_Press";

                    if (cboSeqUnit.Items.Count == 2)
                        cboSeqUnit.SelectedIndex = 1;
                    else if (frmWeighPlan.Prod_Press != "")
                        cboSeqUnit.SelectedIndex = cboSeqUnit.FindStringExact(frmWeighPlan.Prod_Press);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtNextSequence", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dgvNextSequence_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvNextSequence.Columns[e.ColumnIndex].Name == "btn_NextPlan")
                {
                    SqlParameter[] spIns = new SqlParameter[] { 
                        new SqlParameter("@Prod_ItemID", Convert.ToInt32(dgvNextSequence.Rows[e.RowIndex].Cells["Prod_ItemID"].Value.ToString())), 
                        new SqlParameter("@Prod_ReqQty", Convert.ToInt32(dgvNextSequence.Rows[e.RowIndex].Cells["Prod_ReqQty"].Value.ToString())), 
                        new SqlParameter("@Prod_Press", dgvNextSequence.Rows[e.RowIndex].Cells["Prod_Press"].Value.ToString()), 
                        new SqlParameter("@Prod_MouldID", dgvNextSequence.Rows[e.RowIndex].Cells["Prod_MouldID"].Value.ToString()), 
                        new SqlParameter("@PlannedBy", Program.strUserName) 
                    };
                    int resp = (int)dba.ExecuteScalar_SP("sp_ins_newProductionPlanList_FromSeq", spIns);
                    if (resp > 0)
                    {
                        SqlParameter[] spUpt = new SqlParameter[] { 
                            new SqlParameter("@SequenceID", Convert.ToInt32(dgvNextSequence.Rows[e.RowIndex].Cells["SequenceID"].Value.ToString())),
                            new SqlParameter("@PlanID", resp)
                        };
                        int upResp = dba.ExecuteNonQuery_SP("sp_upd_newProductionSequence_status", spUpt);
                        if (upResp > 0)
                        {
                            MessageBox.Show("SEQUENCE MOULD MOVED TO PLAN SUCCESSFULLY");
                            frmGtNextSequence_Load(sender, e); ;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtNextSequence", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboSeqUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgvNextSequence.DataSource = null;
                dgvNextSequence.Columns.Clear();
                dgvNextSequence.Rows.Clear();
                if (cboSeqUnit.SelectedIndex > 0)
                {
                    SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@Prod_Press", cboSeqUnit.SelectedValue.ToString()) };
                    DataTable dtSeq = (DataTable)dba.ExecuteReader_SP("sp_sel_mould_prod_sequence_v1", spSel, DBAccess.Return_Type.DataTable);
                    if (dtSeq.Rows.Count > 0)
                    {
                        dgvNextSequence.DataSource = dtSeq;
                        dgvNextSequence.Columns[0].Width = 100; //Config
                        dgvNextSequence.Columns[0].HeaderText = "PLATFORM";
                        dgvNextSequence.Columns[0].ReadOnly = true;
                        dgvNextSequence.Columns[1].Width = 180; //tyresize
                        dgvNextSequence.Columns[1].HeaderText = "TYRE SIZE";
                        dgvNextSequence.Columns[1].ReadOnly = true;
                        dgvNextSequence.Columns[2].Width = 40; //rimsize
                        dgvNextSequence.Columns[2].HeaderText = "RIM";
                        dgvNextSequence.Columns[2].ReadOnly = true;
                        dgvNextSequence.Columns[3].Width = 60; //tyretype
                        dgvNextSequence.Columns[3].HeaderText = "TYPE";
                        dgvNextSequence.Columns[3].ReadOnly = true;
                        dgvNextSequence.Columns[4].Width = 100; //brand
                        dgvNextSequence.Columns[4].HeaderText = "BRAND";
                        dgvNextSequence.Columns[4].ReadOnly = true;
                        dgvNextSequence.Columns[5].Width = 100; //sidewall
                        dgvNextSequence.Columns[5].HeaderText = "SIDEWALL";
                        dgvNextSequence.Columns[5].ReadOnly = true;
                        dgvNextSequence.Columns[6].Width = 40; //Prod_ReqQty
                        dgvNextSequence.Columns[6].HeaderText = "QTY";
                        dgvNextSequence.Columns[6].ReadOnly = true;
                        dgvNextSequence.Columns[7].Width = 80; //Prod_Press
                        dgvNextSequence.Columns[7].HeaderText = "CURING UNIT";
                        dgvNextSequence.Columns[7].ReadOnly = true;
                        dgvNextSequence.Columns[8].Width = 80; //Prod_MouldID
                        dgvNextSequence.Columns[8].HeaderText = "MOULD";
                        dgvNextSequence.Columns[8].ReadOnly = true;
                        dgvNextSequence.Columns[9].Width = 60; //seq_priority
                        dgvNextSequence.Columns[9].HeaderText = "S.NO";
                        dgvNextSequence.Columns[9].ReadOnly = true;
                        dgvNextSequence.Columns[10].Visible = false;//Prod_ItemID
                        dgvNextSequence.Columns[11].Visible = false;//SequenceID

                        if (frmGtWeigh.intRemainQty <= 0)
                        {
                            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                            dgvNextSequence.Columns.Add(btn);
                            btn.HeaderText = "NEXT PLAN";
                            btn.Text = "PRODUCE";
                            btn.Name = "btn_NextPlan";
                            btn.UseColumnTextForButtonValue = true;
                        }
                        else
                            MessageBox.Show("NEXT PRODUCTION SEQUENCE LISTED.\n ALERT THE TEAM FOR CHANGES.");
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmGtNextSequence", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSequenceClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
