using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Data.SqlClient;

namespace GT
{
    public partial class frmConcessionChart : Form
    {
        DBAccess dba = new DBAccess();
        List<string> withoutPosition = new List<string>();
        List<string> withPosition = new List<string>();
        string sitem = "";
        string rdbText = "";
        public frmConcessionChart()
        {
            InitializeComponent();
        }

        private void frmConsMaster_Load(object sender, EventArgs e)
        {
            lblErr.Text = "";
        }
        private void rdbBase_CheckedChanged(object sender, EventArgs e)
        {
            rdbText = rdbBase.Text;
            Bind_comBox();
        }

        private void rdbCenter_CheckedChanged(object sender, EventArgs e)
        {
            rdbText = rdbCenter.Text;
            Bind_comBox();
        }

        private void rdbTread_CheckedChanged(object sender, EventArgs e)
        {
            rdbText = rdbTread.Text;
            Bind_comBox();
        }
        private void Bind_comBox()
        {
            try
            {
                lblErr.Text = "";
                lbxExist.DataSource = null;
                lbxExist.Items.Clear();
                lbxUpdate.DataSource = null;
                lbxUpdate.Items.Clear();
                cmbMastCode.DataSource = null;
                cmbMastCode.Items.Clear();

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@category", rdbText) };
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_masterCompCode", sp, DBAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dt.Rows.InsertAt(dr, 0);

                    cmbMastCode.DataSource = dt;
                    cmbMastCode.DisplayMember = "MasterCode";
                    cmbMastCode.ValueMember = "MasterCode";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConsMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";
                int i = lbxExist.SelectedIndex;
                if (i >= 0 && i < lbxExist.Items.Count)
                {
                    sitem = lbxExist.SelectedItem.ToString();
                    lbxExist.DataSource = "".ToList();
                    lbxUpdate.DataSource = "".ToList();

                    withPosition.Add(sitem);
                    withoutPosition.Remove(sitem);

                    lbxExist.DataSource = withoutPosition;
                    lbxUpdate.DataSource = withPosition;

                    lbxUpdate.SelectedItem = sitem;
                }
                else
                {
                    lblErr.Text = "Code not selected in compound list";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConsMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";
                int i = lbxUpdate.SelectedIndex;
                if (i >= 0 && i < lbxUpdate.Items.Count)
                {
                    sitem = lbxUpdate.SelectedItem.ToString();
                    lbxExist.DataSource = "".ToList();
                    lbxUpdate.DataSource = "".ToList();

                    withoutPosition.Add(sitem);
                    withPosition.Remove(sitem);

                    lbxExist.DataSource = withoutPosition;
                    lbxUpdate.DataSource = withPosition;

                    lbxExist.SelectedItem = sitem;
                }
                else
                {
                    lblErr.Text = "Code not selected iin higher grade list";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConsMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";

                int i = lbxUpdate.SelectedIndex;
                if (i > 0 && i < lbxUpdate.Items.Count)
                {
                    sitem = lbxUpdate.SelectedItem.ToString();
                    lbxUpdate.DataSource = "".ToList();

                    withPosition.Remove(sitem);
                    withPosition.Insert(i - 1, sitem);

                    lbxUpdate.DataSource = withPosition;
                    lbxUpdate.SetSelected(i - 1, true);
                }
                else
                {
                    lblErr.Text = "Code Reached First Position";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConsMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";
                int i = lbxUpdate.SelectedIndex;
                if (i >= 0 && i < lbxUpdate.Items.Count - 1)
                {
                    sitem = lbxUpdate.SelectedItem.ToString();
                    lbxUpdate.DataSource = "".ToList();

                    withPosition.Remove(sitem);
                    withPosition.Insert(i + 1, sitem);

                    lbxUpdate.DataSource = withPosition;
                    lbxUpdate.SetSelected(i + 1, true);
                }
                else
                {
                    lblErr.Text = "Code Reached End Position";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConsMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCompChart = new DataTable();
                dtCompChart.Columns.Add(new DataColumn("CompCode", typeof(String)));
                dtCompChart.Columns.Add(new DataColumn("Position", typeof(int)));
                for (int i = 1; i <= withPosition.Count; i++)
                {
                    dtCompChart.Rows.Add(withPosition.ElementAt(i - 1).ToString(), i);
                }
                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@category", rdbText),
                    new SqlParameter("@masterCode", cmbMastCode.SelectedValue.ToString()),
                    new SqlParameter("@priorityTbl", dtCompChart),
                    new SqlParameter("@CreatedBy", Program.strUserName)
                };
                int res = dba.ExecuteNonQuery_SP("sp_ins_upd_compondPosition", sp);
                if (res > 0)
                {
                    MessageBox.Show("Compound High Grade Position Updated!");
                    Bind_comBox();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConsMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Bind_comBox();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConsMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cmbMastCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";
                lbxExist.DataSource = null;
                lbxExist.Items.Clear();
                lbxUpdate.DataSource = null;
                lbxUpdate.Items.Clear();

                if (cmbMastCode.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] {
                        new SqlParameter("@category", rdbText),
                        new SqlParameter("@masterCode", cmbMastCode.SelectedValue.ToString())
                    };

                    DataSet ds = (DataSet)dba.ExecuteReader_SP("sp_sel_compCode", sp1, DBAccess.Return_Type.DataSet);

                    withoutPosition = (from row in ds.Tables[0].AsEnumerable() select row["CompoundCode"].ToString()).ToList();
                    withPosition = (from row in ds.Tables[1].AsEnumerable() select row["CompCode"].ToString()).ToList();

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                            lbxExist.DataSource = withoutPosition;
                        if (ds.Tables[1].Rows.Count > 0)
                            lbxUpdate.DataSource = withPosition;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConsMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
