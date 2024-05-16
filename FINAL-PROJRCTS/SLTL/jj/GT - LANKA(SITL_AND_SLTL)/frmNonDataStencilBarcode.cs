using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Data.OleDb;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace GT
{
    public partial class frmNonDataStencilBarcode : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int conn, int val);
        DBAccess dba = new DBAccess();
        DataTable dtProcessIdDetails = new DataTable();
        public frmNonDataStencilBarcode()
        {
            try
            {
                InitializeComponent();

                RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
                rkey.SetValue("sShortDate", "dd-MM-yyyy");
                rkey.SetValue("sLongDate", "dd-MM-yyyy");

                dtpProductionDate.MaxDate = DateTime.Now;
                dtpProductionDate.Value = DateTime.Now;
                dtpProductionDate.MinDate = DateTime.Now.AddDays(-365);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmDailyProduction_Load(object sender, EventArgs e)
        {
            try
            {

                txtStencil.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1)) +
                    dtpProductionDate.Value.Year.ToString().Substring(2, 2) + dtpProductionDate.Value.Month.ToString("00");

                cboPlatform.DataSource = null;
                cboBrand.DataSource = null;
                cboSidewall.DataSource = null;
                cboType.DataSource = null;
                cboSize.DataSource = null;
                cboRim.DataSource = null;
                txtProcessID.Text = "";
                txtLocation.Text = "";
                txtBarcode.Text = "";
                txtRemarks.Text = "";

                cmbGradeSelection.DataSource = null;
                DataTable dtgrade = new DataTable();
                dtgrade.Columns.Add("GRADE", typeof(string));
                //for (int i = 65; i <= 90; i++)
                //{
                //    dtgrade.Rows.Add(Convert.ToChar(i));
                //}

                dtgrade.Rows.Add("A");
                dtgrade.Rows.Add("B");
                dtgrade.Rows.Add("C");
                dtgrade.Rows.Add("D");
                dtgrade.Rows.Add("E");
                dtgrade.Rows.Add("R");
                dtgrade.Rows.Add("S");
                dtgrade.Rows.Add("Z");

                cmbGradeSelection.DataSource = dtgrade;
                cmbGradeSelection.DisplayMember = "GRADE";
                cmbGradeSelection.ValueMember = "GRADE";
                cmbGradeSelection.SelectedIndex = 0;

                dtProcessIdDetails = (DataTable)dba.ExecuteReader_SP("SP_SEL_EditBarcode_ProcessIdDetails", DBAccess.Return_Type.DataTable);

                Bind_ProcessID_data("Config");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Bind_ProcessID_data(string strField)
        {
            try
            {
                switch (strField)
                {
                    case "Config":
                        List<string> lstConfig = dtProcessIdDetails.AsEnumerable().OrderBy(n => n.Field<string>("Config")).Select(A => A.Field<string>("Config")).Distinct().ToList();
                        lstConfig.Insert(lstConfig.Count, "CHOOSE");
                        cboPlatform.DataSource = lstConfig;
                        if (lstConfig.Count == 2)
                        {
                            cboBrand.Focus();
                            cboPlatform.SelectedIndex = 0;
                            cboPlatform_Click(null, null);
                        }
                        else
                        {
                            cboPlatform.SelectedIndex = cboPlatform.Items.Count - 1;
                            cboPlatform.Focus();
                        }
                        break;
                    case "Brand":
                        List<string> lstBrand = dtProcessIdDetails.AsEnumerable().Where(
                           b => b.Field<string>("Config").Equals(cboPlatform.Text)
                            ).OrderBy(n => n.Field<string>("Brand")).Select(A => A.Field<string>("Brand")).Distinct().ToList();
                        lstBrand.Insert(lstBrand.Count, "CHOOSE");
                        cboBrand.DataSource = lstBrand;
                        if (lstBrand.Count == 2)
                        {
                            cboSidewall.Focus();
                            cboBrand.SelectedIndex = 0;
                            cboBrand_Click(null, null);
                        }
                        else
                        {
                            cboBrand.SelectedIndex = cboBrand.Items.Count - 1;
                            cboBrand.Focus();
                        }
                        break;
                    case "Sidewall":
                        List<string> lstSidewall = dtProcessIdDetails.AsEnumerable().Where(
                        b => b.Field<string>("Config").Equals(cboPlatform.Text) &&
                        b.Field<string>("Brand").Equals(cboBrand.SelectedItem.ToString())
                        ).OrderBy(n => n.Field<string>("Sidewall")).Select(A => A.Field<string>("Sidewall")).Distinct().ToList();
                        lstSidewall.Insert(lstSidewall.Count, "CHOOSE");
                        cboSidewall.DataSource = lstSidewall;
                        if (lstSidewall.Count == 2)
                        {
                            cboType.Focus();
                            cboSidewall.SelectedIndex = 0;
                            cboSidewall_Click(null, null);
                        }
                        else
                        {
                            cboSidewall.SelectedIndex = cboSidewall.Items.Count - 1;
                            cboSidewall.Focus();
                        }
                        break;
                    case "TyreType":
                        List<string> lstType = dtProcessIdDetails.AsEnumerable().Where(
                         b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                             b.Field<string>("Brand").Equals(cboBrand.SelectedItem.ToString()) &&
                             b.Field<string>("Sidewall").Equals(cboSidewall.SelectedItem.ToString())
                         ).OrderBy(n => n.Field<string>("TyreType")).Select(A => A.Field<string>("TyreType")).Distinct().ToList();
                        lstType.Insert(lstType.Count, "CHOOSE");
                        cboType.DataSource = lstType;
                        if (lstType.Count == 2)
                        {
                            cboSize.Focus();
                            cboType.SelectedIndex = 0;
                            cboType_Click(null, null);
                        }
                        else
                        {
                            cboType.SelectedIndex = cboType.Items.Count - 1;
                            cboType.Focus();
                        }
                        break;
                    case "TyreSize":
                        List<string> lstSize = dtProcessIdDetails.AsEnumerable().Where(
                         b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                         b.Field<string>("Brand").Equals(cboBrand.SelectedItem.ToString()) &&
                         b.Field<string>("Sidewall").Equals(cboSidewall.SelectedItem.ToString()) &&
                         b.Field<string>("TyreType").Equals(cboType.SelectedItem.ToString())
                         ).OrderBy(n => n.Field<string>("TyreSize")).Select(A => A.Field<string>("TyreSize")).Distinct().ToList();
                        lstSize.Insert(lstSize.Count, "CHOOSE");
                        cboSize.DataSource = lstSize;
                        if (lstSize.Count == 2)
                        {
                            cboRim.Focus();
                            cboSize.SelectedIndex = 0;
                            cboSize_Click(null, null);
                        }
                        else
                        {
                            cboSize.SelectedIndex = cboSize.Items.Count - 1;
                            cboSize.Focus();
                        }
                        break;
                    case "TyreRim":
                        List<string> lstRim = dtProcessIdDetails.AsEnumerable().Where(
                            b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                            b.Field<string>("Brand").Equals(cboBrand.SelectedItem.ToString()) &&
                            b.Field<string>("Sidewall").Equals(cboSidewall.SelectedItem.ToString()) &&
                            b.Field<string>("TyreType").Equals(cboType.SelectedItem.ToString()) &&
                            b.Field<string>("TyreSize").Equals(cboSize.SelectedItem.ToString())
                            ).OrderBy(n => n.Field<string>("TyreRim")).Select(A => A.Field<string>("TyreRim")).Distinct().ToList();
                        lstRim.Insert(lstRim.Count, "CHOOSE");
                        cboRim.DataSource = lstRim;
                        if (lstRim.Count == 2)
                        {
                            cmbGradeSelection.SelectedIndex = 0;
                            cmbGradeSelection.Focus();
                            cboRim.SelectedIndex = 0;
                            cboRim_Click(null, null);
                        }
                        else
                        {
                            cboRim.SelectedIndex = cboRim.Items.Count - 1;
                            cboRim.Focus();
                        }
                        break;
                    case "ProcessID":
                        txtProcessID.Text = "";
                        List<string> lst = dtProcessIdDetails.AsEnumerable().Where(
                                b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                                b.Field<string>("Brand").Equals(cboBrand.SelectedItem.ToString()) &&
                                b.Field<string>("Sidewall").Equals(cboSidewall.SelectedItem.ToString()) &&
                                b.Field<string>("TyreType").Equals(cboType.SelectedItem.ToString()) &&
                                b.Field<string>("TyreSize").Equals(cboSize.SelectedItem.ToString()) &&
                                b.Field<string>("TyreRim").Equals(cboRim.SelectedItem.ToString())
                                ).Select(A => A.Field<string>("ProcessID")).Distinct().ToList();
                        txtProcessID.Text = lst[0].ToString();
                        cmbGradeSelection.SelectedIndex = 0;
                        cmbGradeSelection.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ErrMsg = "";
                if (cboPlatform.Text == "CHOOSE")
                    ErrMsg += "Select Platform\n";
                if (cboBrand.Text == "CHOOSE")
                    ErrMsg += "Select Brand\n";
                if (cboSidewall.Text == "CHOOSE")
                    ErrMsg += "Select Sidewall\n";
                if (cboType.Text == "CHOOSE")
                    ErrMsg += "Select Type\n";
                if (cboSize.Text == "CHOOSE")
                    ErrMsg += "Select Tyre size\n";
                if (cboRim.Text == "CHOOSE")
                    ErrMsg += "Select Rim\n";
                if (txtProcessID.Text == "")
                    ErrMsg += "Process-ID not created\n";
                if (cmbGradeSelection.SelectedIndex == -1)
                    ErrMsg += "Seelct Grade\n";
                if (txtStencil.Text == "" || txtStencil.Text.Length != 10)
                    ErrMsg += "Enter proper stencil no";
                if (txtBarcode.Text == "" || txtBarcode.Text.Length != 19)
                    ErrMsg += "Enter proper barcode no";

                if (ErrMsg.Length > 0)
                    MessageBox.Show(ErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    int Out;
                    if (InternetGetConnectedState(out Out, 0) == false)
                    {
                        MessageBox.Show("Internet Not Connected !");
                        this.Hide();
                    }
                    else
                        Save_Data();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Save_Data()
        {
            try
            {
                int Out;
                if (InternetGetConnectedState(out Out, 0) == true)
                {
                    SqlConnection oleCon = new SqlConnection();
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass +
                            ";Trusted_Connection=False;");
                        oleCon.Open();
                    }
                    catch (SqlException)
                    {
                        try
                        {
                            oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass +
                                ";Trusted_Connection=False;");
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
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@Stencil", txtStencil.Text), 
                            new SqlParameter("@Plant", Program.strPlantName) 
                        };
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = oleCon;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_chk_CrossPlant_NonDataBarcode";
                        cmd.CommandTimeout = 1200;
                        foreach (SqlParameter Sp in sp1)
                        {
                            cmd.Parameters.Add(Sp);
                        }
                        DataTable dt_CrossPlant = new DataTable();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt_CrossPlant);

                        if (dt_CrossPlant.Rows.Count > 0)
                        {
                            MessageBox.Show("STOCK AVAILABLE AT " + dt_CrossPlant.Rows[0]["Plant"].ToString() + " FROM " +
                                dt_CrossPlant.Rows[0]["BarcodeOn"].ToString());
                        }
                        else
                        {
                            string strChar = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1));
                            SqlParameter[] spE = new SqlParameter[] { 
                                new SqlParameter("@stencilno", txtStencil.Text), 
                                new SqlParameter("@ChkCondition", txtStencil.Text.Contains(strChar) ? 1 : 0) 
                            };
                            DataTable dtExist = (DataTable)dba.ExecuteReader_SP("sp_sel_Stencil_ExistsMsg", spE, DBAccess.Return_Type.DataTable);
                            if (dtExist == null || dtExist.Rows.Count == 0)
                            {
                                SqlParameter[] spIns = new SqlParameter[] { 
                                    new SqlParameter("@config", cboPlatform.Text), 
                                    new SqlParameter("@tyresize", cboSize.Text), 
                                    new SqlParameter("@tyrerim", cboRim.Text), 
                                    new SqlParameter("@tyretype", cboType.Text), 
                                    new SqlParameter("@brand", cboBrand.Text), 
                                    new SqlParameter("@sidewall", cboSidewall.Text), 
                                    new SqlParameter("@barcode", txtBarcode.Text.ToUpper()), 
                                    new SqlParameter("@remarks", txtRemarks.Text), 
                                    new SqlParameter("@dateofmanufacture", dtpProductionDate.Text), 
                                    new SqlParameter("@Location", txtLocation.Text),
                                    new SqlParameter("@username", Program.strUserName)
                                };
                                int resp1 = dba.ExecuteNonQuery_SP("sp_ins_nondata_stencil", spIns);
                                if (resp1 > 0)
                                {
                                    Program.PreparePrintLable(txtBarcode.Text);
                                    txtBarcode.Text = "";
                                    txtRemarks.Text = "";
                                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StencilNo", txtStencil.Text) };
                                    dba.ExecuteNonQuery_SP("sp_upd_load_and_inspection", sp);
                                    txtStencil.Text = txtStencil.Text.Substring(0, 6);
                                    txtStencil.Focus();
                                    txtStencil.SelectionStart = txtStencil.Text.Length;
                                }
                                else
                                    MessageBox.Show("Stencilno not saved", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (dtExist.Rows.Count > 0)
                                MessageBox.Show(dtExist.Rows[0]["createdate"].ToString(), "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            frmDailyProduction_Load(sender, e);
        }
        private void cboPlatform_Click(object sender, EventArgs e)
        {
            if (cboPlatform.SelectedIndex != cboPlatform.Items.Count - 1)
                Bind_ProcessID_data("Brand");
        }
        private void cboPlatform_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboPlatform.FindStringExact(cboPlatform.Text) != -1 && cboPlatform.Text != "CHOOSE")
                {
                    cboPlatform.SelectedIndex = cboPlatform.FindStringExact(cboPlatform.Text);
                    Bind_ProcessID_data("Brand");
                    SendKeys.Send("{F4}");
                }
                else
                {
                    cboPlatform.Focus();
                    SendKeys.Send("{F4}");
                }
            }
        }
        private void cboBrand_Click(object sender, EventArgs e)
        {
            if (cboPlatform.Text == "" || cboPlatform.Text == "CHOOSE" || cboPlatform.FindStringExact(cboPlatform.Text) == -1)
            {
                MessageBox.Show("select platform");
                cboPlatform.SelectedIndex = cboPlatform.Items.Count - 1;
                cboPlatform.Focus();
            }
            else
                Bind_ProcessID_data("Sidewall");
        }
        private void cboBrand_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboBrand.FindStringExact(cboBrand.Text) != -1 && cboBrand.Text != "CHOOSE")
                {
                    cboBrand.SelectedIndex = cboBrand.FindStringExact(cboBrand.Text);
                    Bind_ProcessID_data("Sidewall");
                    SendKeys.Send("{F4}");
                }
                else
                {
                    cboBrand.Focus();
                    SendKeys.Send("{F4}");
                }
            }
        }
        private void cboSidewall_Click(object sender, EventArgs e)
        {
            if (cboBrand.Text == "" || cboBrand.Text == "CHOOSE" || cboBrand.FindStringExact(cboBrand.Text) == -1)
            {
                MessageBox.Show("select brand");
                cboBrand.SelectedIndex = cboBrand.Items.Count - 1;
                cboBrand.Focus();
            }
            else
                Bind_ProcessID_data("TyreType");
        }
        private void cboSidewall_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboSidewall.FindStringExact(cboSidewall.Text) != -1 && cboSidewall.Text != "CHOOSE")
                {
                    cboSidewall.SelectedIndex = cboSidewall.FindStringExact(cboSidewall.Text);
                    Bind_ProcessID_data("TyreType");
                    SendKeys.Send("{F4}");
                }
                else
                {
                    cboSidewall.Focus();
                    SendKeys.Send("{F4}");
                }
            }
        }
        private void cboType_Click(object sender, EventArgs e)
        {
            if (cboSidewall.Text == "" || cboSidewall.Text == "CHOOSE" || cboSidewall.FindStringExact(cboSidewall.Text) == -1)
            {
                MessageBox.Show("select sidewall");
                cboSidewall.SelectedIndex = cboSidewall.Items.Count - 1;
                cboSidewall.Focus();
            }
            else
                Bind_ProcessID_data("TyreSize");
        }
        private void cboType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboType.FindStringExact(cboType.Text) != -1 && cboType.Text != "CHOOSE")
                {
                    cboType.SelectedIndex = cboType.FindStringExact(cboType.Text);
                    Bind_ProcessID_data("TyreSize");
                    SendKeys.Send("{F4}");
                }
                else
                {
                    cboType.Focus();
                    SendKeys.Send("{F4}");
                }
            }
        }
        private void cboSize_Click(object sender, EventArgs e)
        {
            if (cboType.Text == "" || cboType.Text == "CHOOSE" || cboType.FindStringExact(cboType.Text) == -1)
            {
                MessageBox.Show("select type");
                cboType.SelectedIndex = cboType.Items.Count - 1;
                cboType.Focus();
            }
            else
                Bind_ProcessID_data("TyreRim");
        }
        private void cboSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboSize.FindStringExact(cboSize.Text) != -1 && cboSize.Text != "CHOOSE")
                {
                    cboSize.SelectedIndex = cboSize.FindStringExact(cboSize.Text);
                    Bind_ProcessID_data("TyreRim");
                    SendKeys.Send("{F4}");
                }
                else
                {
                    cboSize.Focus();
                    SendKeys.Send("{F4}");
                }
            }
        }
        private void cboRim_Click(object sender, EventArgs e)
        {
            if (cboSize.Text == "" || cboSize.Text == "CHOOSE" || cboSize.FindStringExact(cboSize.Text) == -1)
            {
                MessageBox.Show("select tyre size");
                cboSize.SelectedIndex = cboSize.Items.Count - 1;
                cboSize.Focus();
            }
            else
                Bind_ProcessID_data("ProcessID");
        }
        private void cboRim_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboRim.FindStringExact(cboRim.Text) != -1 && cboRim.Text != "CHOOSE")
                {
                    cboRim.SelectedIndex = cboRim.FindStringExact(cboRim.Text);
                    Bind_ProcessID_data("ProcessID");
                }
                else
                {
                    cboRim.Focus();
                    SendKeys.Send("{F4}");
                }
            }
        }
        private void dtpProductionDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                cboPlatform.SelectedIndex = 1;
                cboPlatform.Focus();
                SendKeys.Send("{F4}");
            }
        }
        private void cmbGradeSelection_Click(object sender, EventArgs e)
        {
            if (cboRim.Text == "" || cboRim.Text == "CHOOSE" || cboRim.FindStringExact(cboSize.Text) == -1)
            {
                txtStencil.Focus();
                txtStencil.SelectionStart = txtStencil.Text.Length;
            }
            else
                txtBarcode.Text = "";
        }
        private void cmbGradeSelection_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cmbGradeSelection.FindStringExact(cmbGradeSelection.Text) != -1 && cmbGradeSelection.Text != "CHOOSE")
                {
                    cmbGradeSelection.SelectedIndex = cmbGradeSelection.FindStringExact(cmbGradeSelection.Text);
                    txtStencil.Focus();
                    txtStencil.SelectionStart = txtStencil.Text.Length;
                }
                else
                {
                    cmbGradeSelection.Focus();
                    SendKeys.Send("{F4}");
                }
            }
        }
        private void txtLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnSave.Focus();
        }
        private void txtStencil_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && (txtStencil.Text.Length > 9 && txtStencil.Text.Length < 13) && cmbGradeSelection.SelectedIndex > -1 && txtProcessID.Text != "" && txtStencil.Text != "")
            {
                txtBarcode.Text = (txtProcessID.Text + txtStencil.Text + cmbGradeSelection.Text).ToUpper();
                txtLocation.Focus();
            }
        }
        private void dtpProductionDate_ValueChanged(object sender, EventArgs e)
        {
            txtStencil.Text = "C" + dtpProductionDate.Value.Year.ToString().Substring(2, 2) + dtpProductionDate.Value.Month.ToString("00");
        }
        private void txtStencil_Leave(object sender, EventArgs e)
        {
            if (cmbGradeSelection.SelectedIndex > -1 && txtProcessID.Text != "" && txtStencil.Text != "" && (txtStencil.Text.Length > 9 && txtStencil.Text.Length < 13))
            {
                txtBarcode.Text = (txtProcessID.Text + txtStencil.Text + cmbGradeSelection.Text).ToUpper();
                txtLocation.Focus();
            }
            else
                txtBarcode.Text = "";
        }
        private void cmbGradeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
