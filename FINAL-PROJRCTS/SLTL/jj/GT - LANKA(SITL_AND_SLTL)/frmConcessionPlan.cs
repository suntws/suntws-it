using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace GT
{
    public partial class frmConcessionPlan : Form
    {
        DBAccess dba = new DBAccess();
        decimal decConcUsage = 0, decConcLimit = 0;
        public frmConcessionPlan()
        {
            InitializeComponent();
        }

        private void frmConcessionPlan_Load(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";
                cmbCateg.DataSource = null;
                cmbCateg.Items.Clear();
                cmbCompCode.DataSource = null;
                cmbCompCode.Items.Clear();
                cmbConcCode.DataSource = null;
                cmbConcCode.Items.Clear();
                grpPanel.Controls.Clear();
                txtCompLimit.Text = "";
                grdConDetails.DataSource = null;
                pnlGrid.Visible = false;
                btnDelete.Visible = false;
                btnSave.Text = "SAVE";
                DataTable dtCategory = (DataTable)dba.ExecuteReader_SP("sp_CompoundCode_Category", DBAccess.Return_Type.DataTable);
                if (dtCategory != null && dtCategory.Rows.Count > 0)
                {
                    DataRow dr = dtCategory.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtCategory.Rows.InsertAt(dr, 0);

                    cmbCateg.DataSource = dtCategory;
                    cmbCateg.DisplayMember = "Category";
                    cmbCateg.ValueMember = "Category";

                    if (cmbCateg.Items.Count == 2)
                        cmbCateg.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cmbCateg_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";
                cmbCompCode.DataSource = null;
                cmbCompCode.Items.Clear();
                cmbConcCode.DataSource = null;
                cmbConcCode.Items.Clear();
                grpPanel.Controls.Clear();
                txtCompLimit.Text = "";
                grdConDetails.DataSource = null;
                pnlGrid.Visible = false;
                btnDelete.Visible = false;
                btnSave.Text = "SAVE";

                if (cmbCateg.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@category", cmbCateg.SelectedValue.ToString()) };
                    DataTable dtMaster = (DataTable)dba.ExecuteReader_SP("sp_CompoundCode_Master", sp, DBAccess.Return_Type.DataTable);
                    if (dtMaster != null && dtMaster.Rows.Count > 0)
                    {
                        DataRow dr = dtMaster.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtMaster.Rows.InsertAt(dr, 0);

                        cmbCompCode.DataSource = dtMaster;
                        cmbCompCode.DisplayMember = "MasterCode";
                        cmbCompCode.ValueMember = "MasterCode";

                        if (cmbCompCode.Items.Count == 2)
                            cmbCompCode.SelectedIndex = 1;
                        else
                            Bind_DataGridView(cmbCateg.SelectedValue.ToString(), "", "");
                    }
                    else
                        lblErr.Text = "There is No Compound Code for " + cmbCateg.SelectedValue.ToString();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cmbCompCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";
                cmbConcCode.DataSource = null;
                cmbConcCode.Items.Clear();
                grpPanel.Controls.Clear();
                txtCompLimit.Text = "";
                pnlGrid.Visible = false;
                btnDelete.Visible = false;
                btnSave.Text = "SAVE";

                if (cmbCompCode.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] {
                        new SqlParameter("@category", cmbCateg.SelectedValue.ToString()),
                        new SqlParameter("@MasterCode", cmbCompCode.SelectedValue.ToString())
                    };
                    DataSet ds = (DataSet)dba.ExecuteReader_SP("sp_sel_concCode_positionWise", sp, DBAccess.Return_Type.DataSet);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        ds.Tables[0].Rows.InsertAt(dr, 0);

                        cmbConcCode.DataSource = ds.Tables[0];
                        cmbConcCode.DisplayMember = "CompCode";
                        cmbConcCode.ValueMember = "CompCode";
                    }
                    else
                    {
                        lblErr.Text = "There is No Concession Code for " + cmbCompCode.SelectedValue.ToString();
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        int maxHeight = grpPanel.Height - 20;
                        int maxWidth = grpPanel.Width - 120;
                        int curHeight = 0;
                        int curWidth = 5;

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            CheckBox chk = new CheckBox();
                            chk.Text = ds.Tables[1].Rows[i]["TyreType"].ToString();
                            if (i > 0) curWidth += 120;
                            if (curWidth >= maxWidth)
                            {
                                curHeight += 30;
                                curWidth = 5;
                            }
                            chk.Location = new Point(curWidth, curHeight);
                            chk.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
                            grpPanel.Controls.Add(chk);
                        }
                        pnlGrid.Visible = true;
                    }

                    if (cmbConcCode.Items.Count == 2)
                        cmbConcCode.SelectedIndex = 1;
                    else
                        Bind_DataGridView(cmbCateg.SelectedValue.ToString(), cmbCompCode.SelectedValue.ToString(), "");
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cmbConcCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";
                txtCompLimit.Text = "";
                btnDelete.Visible = false;
                btnSave.Text = "SAVE";

                foreach (CheckBox chk in grpPanel.Controls)
                {
                    chk.Checked = false;
                    chk.Enabled = true;
                }

                if (cmbConcCode.SelectedIndex > 0)
                {
                    Bind_DataGridView(cmbCateg.SelectedValue.ToString(), cmbCompCode.SelectedValue.ToString(), cmbConcCode.SelectedValue.ToString());

                    SqlParameter[] sp = new SqlParameter[] {
                        new SqlParameter("@category", cmbCateg.SelectedValue.ToString()),
                        new SqlParameter("@MasterCode", cmbCompCode.SelectedValue.ToString()),
                        new SqlParameter("@concCode", cmbConcCode.SelectedValue.ToString())
                    };

                    DataSet dsMain = (DataSet)dba.ExecuteReader_SP("sp_sel_concLimitandUsage_details", sp, DBAccess.Return_Type.DataSet);
                    if (dsMain.Tables[0].Rows.Count == 1)
                    {
                        if (dsMain.Tables[1].Rows.Count > 0)
                        {
                            foreach (CheckBox chk in grpPanel.Controls)
                            {
                                if (dsMain.Tables[1].AsEnumerable().Where(n => n.Field<string>("tyreType").Equals(chk.Text)).Select(n =>
                                n.Field<string>("tyreType").ToString()).Count() > 0)
                                    chk.Checked = true;
                            }
                        }

                        decConcUsage = Convert.ToDecimal(dsMain.Tables[0].Rows[0]["concUsage"].ToString());
                        decConcLimit = Convert.ToDecimal(dsMain.Tables[0].Rows[0]["concLimit"].ToString());
                        txtCompLimit.Text = decConcLimit.ToString();
                        btnSave.Text = "UPDATE";
                        btnDelete.Visible = true;
                    }
                    if (dsMain.Tables[2].Rows.Count > 0)
                    {
                        foreach (CheckBox chk in grpPanel.Controls)
                        {
                            if (dsMain.Tables[2].AsEnumerable().Where(n => n.Field<string>("tyreType").Equals(chk.Text)).Select(n =>
                            n.Field<string>("tyreType").ToString()).Count() > 0)
                                chk.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_DataGridView(string strCategory, string strMaster, string strConcession)
        {
            try
            {
                grdConDetails.DataSource = null;

                SqlParameter[] spS = new SqlParameter[] {
                    new SqlParameter("@Category", strCategory),
                    new SqlParameter("@MasterCode", strMaster),
                    new SqlParameter("@CompCode", strConcession)
                };
                DataTable dtPlantList = (DataTable)dba.ExecuteReader_SP("sp_sel_CompConcession_Plan_details", spS, DBAccess.Return_Type.DataTable);
                if (dtPlantList != null && dtPlantList.Rows.Count > 0)
                {
                    grdConDetails.DataSource = dtPlantList;
                    for (int i = 0; i <= grdConDetails.Columns.Count - 1; i++)
                    {
                        grdConDetails.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void grdConDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblErr.Text = "";
                txtCompLimit.Text = "";
                btnDelete.Visible = false;
                btnSave.Text = "SAVE";

                if (e.RowIndex >= 0)
                {
                    string strMasterCode = grdConDetails.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string strConcCode = grdConDetails.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtCompLimit.Text = grdConDetails.Rows[e.RowIndex].Cells[4].Value.ToString();

                    cmbCompCode.SelectedIndex = cmbCompCode.FindStringExact(strMasterCode);
                    cmbConcCode.SelectedIndex = cmbConcCode.FindStringExact(strConcCode);

                    btnSave.Text = "UPDATE";
                    btnDelete.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txtCompLimit_Leave(object sender, EventArgs e)
        {
            try
            {
                lblErr.Text = "";
                if (txtCompLimit.Text != "")
                {
                    if (Convert.ToDecimal(txtCompLimit.Text) < decConcUsage)
                    {
                        if (decConcUsage > 0)
                            MessageBox.Show("CONCESSION ALREADY USED " + decConcUsage + "kg. ENTER GREATER THAN TO " + decConcUsage.ToString());
                        txtCompLimit.Text = decConcLimit.ToString();
                        txtCompLimit.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtSave = new DataTable();
                dtSave.Columns.Add(new DataColumn("TyreType", typeof(String)));
                foreach (CheckBox chk in grpPanel.Controls)
                {
                    if (chk.Checked == true)
                        dtSave.Rows.Add(chk.Text);
                }

                if (cmbCateg.SelectedIndex <= 0)
                    lblErr.Text = "CHOOSE CATEGORY";
                else if (cmbCompCode.SelectedIndex <= 0)
                    lblErr.Text = "CHOOSE MASRER CODE";
                else if (cmbConcCode.SelectedIndex <= 0)
                    lblErr.Text = "CHOOSE CONCESSION CODE";
                else if (txtCompLimit.Text == "")
                    lblErr.Text = "ENTER CONCESSION LIMIT";
                else if (dtSave.Rows.Count == 0)
                    lblErr.Text = "CHOOSE ANY ONE TYPE";
                else
                {
                    bool boolSave = btnSave.Text == "UPDATE" ? false : true;
                    if (btnSave.Text == "UPDATE" && Convert.ToDecimal(txtCompLimit.Text) != decConcLimit)
                    {
                        if (MessageBox.Show("DO YOU WANT TO CHANGE THE CONCESSION LIMIT FROM " + decConcLimit + "kg TO " + Convert.ToDecimal(txtCompLimit.Text) + "kg?",
                            btnSave.Text + " CONCESSION LIMIT", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            boolSave = true;
                    }
                    else if (btnSave.Text == "UPDATE" && Convert.ToDecimal(txtCompLimit.Text) == decConcLimit)
                        lblErr.Text = "NO CHANGES IN THIS CONCESSION LIMIT";

                    if (boolSave)
                    {
                        SqlParameter[] sp = new SqlParameter[] {
                            new SqlParameter("@Category", cmbCateg.SelectedValue.ToString()),
                            new SqlParameter("@MasterCode", cmbCompCode.SelectedValue.ToString()),
                            new SqlParameter("@ConsCode", cmbConcCode.SelectedValue.ToString()),
                            new SqlParameter("@Limit", Convert.ToDecimal(txtCompLimit.Text)),
                            new SqlParameter("@CreatedBy", Program.strUserName),
                            new SqlParameter("@limitchartTbl", dtSave)
                        };
                        int rest = dba.ExecuteNonQuery_SP("sp_ins_upd_concessionLimit", sp);
                        if (rest > 0)
                        {
                            MessageBox.Show("CONCESSION LIMIT HAS BEEN " + btnSave.Text + "D SUCCESSFULLY!");
                            cmbCateg_SelectedIndexChanged(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                cmbCateg_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("DO YOU WANT TO DELETE THE CONCESSION?", "DELETE CONCESSION", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SqlParameter[] sp = new SqlParameter[]{
                        new SqlParameter("@category",cmbCateg.SelectedValue.ToString()),
                        new SqlParameter("@MasterCode",cmbCompCode.SelectedValue.ToString()),
                        new SqlParameter("@concCode",cmbConcCode.SelectedValue.ToString()),
                        new SqlParameter("@user",Program.strUserName)
                    };
                    int result = dba.ExecuteNonQuery_SP("sp_del_ConcesCodeLimit_details", sp);
                    if (result > 0)
                    {
                        MessageBox.Show("CONCESSION LIMIT DELETED!");
                        cmbCateg_SelectedIndexChanged(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void chkSelAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkSelAll.Checked == true)
                {
                    foreach (CheckBox chk in grpPanel.Controls)
                    {
                        chk.Checked = true;
                    }
                }
                else
                {
                    foreach (CheckBox chk in grpPanel.Controls)
                    {
                        chk.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txtCompLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }
        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkSelAll.Checked = true;
                foreach (CheckBox chk in grpPanel.Controls)
                {
                    if (chk.Checked == false)
                    {
                        chkSelAll.Checked = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmConcessionPlan", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
