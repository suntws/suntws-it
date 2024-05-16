using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace GT
{
    public partial class frmStencilProdModify : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dtProcessIdDetails;
        public frmStencilProdModify()
        {
            InitializeComponent();
        }
        private void frmStencilProdModify_Load(object sender, EventArgs e)
        {
            try
            {
                txtSearchStencil_Auto();
                lblErrMsg.Text = "";
                dtProcessIdDetails = (DataTable)dba.ExecuteReader_SP("SP_SEL_EditBarcode_ProcessIdDetails", DBAccess.Return_Type.DataTable);
                List<string> lstConfig = dtProcessIdDetails.AsEnumerable().Select(A => A.Field<string>("Config")).Distinct().ToList();
                lstConfig.Insert(0, "CHOOSE");
                cboPlatform.DataSource = lstConfig;

                DataTable dtRe = (DataTable)dba.ExecuteReader_SP("sp_sel_tyre_remarks", DBAccess.Return_Type.DataTable);
                if (dtRe.Rows.Count > 0)
                {
                    DataRow dr = dtRe.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtRe.Rows.InsertAt(dr, 0);

                    cboTyreRemarks.DataSource = dtRe;
                    cboTyreRemarks.DisplayMember = "remarks";
                    cboTyreRemarks.ValueMember = "remarks";

                    if (cboTyreRemarks.Items.Count == 2)
                        cboTyreRemarks.SelectedIndex = 1;
                }

                DataTable dtgrade = new DataTable();
                dtgrade.Columns.Add("GRADE", typeof(string));
                dtgrade.Rows.Add("CHOOSE");
                dtgrade.Rows.Add("A");
                dtgrade.Rows.Add("B");
                dtgrade.Rows.Add("C");
                dtgrade.Rows.Add("D");
                dtgrade.Rows.Add("E");
                dtgrade.Rows.Add("R");
                dtgrade.Rows.Add("S");
                dtgrade.Rows.Add("Z");

                cboGrade.DataSource = dtgrade;
                cboGrade.DisplayMember = "GRADE";
                cboGrade.ValueMember = "GRADE";
                cboGrade.SelectedIndex = 0;

                this.KeyPreview = true;
                this.cboPlatform.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
                this.cbobrand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
                this.cbosidewall.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
                this.cbotype.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
                this.cbotyresize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
                this.cborim.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
                this.cboGrade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtSearchStencil_Auto()
        {
            try
            {
                var stencilCollection = new AutoCompleteStringCollection();
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_Dispatch_non_list", DBAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    string[] postSource = dt.AsEnumerable().Select<System.Data.DataRow, String>(x => x.Field<String>("stencilno")).ToArray();
                    stencilCollection.AddRange(postSource);
                }
                txtStencil.AutoCompleteCustomSource = stencilCollection;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cbo_Enter_Keypress(object sender, KeyEventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;
            if (e.KeyValue == 13 && cbo.SelectedValue.ToString() != "CHOOSE")
            {
                CursorChangedEvents();
                SendKeys.Send("{TAB}");
            }
        }
        private void CursorChangedEvents()
        {
            txtCurrProcessID.Text = ""; txtBarcode.Text = ""; btnSAVE.Visible = false;
            if (cboPlatform.SelectedItem.ToString() != "CHOOSE" && cbobrand.SelectedItem.ToString() != "CHOOSE" &&
                cbosidewall.SelectedItem.ToString() != "CHOOSE" && cbotype.SelectedItem.ToString() != "CHOOSE" &&
                cbotyresize.SelectedItem.ToString() != "CHOOSE" && cborim.SelectedItem.ToString() != "CHOOSE")
            {
                List<string> lst = dtProcessIdDetails.AsEnumerable().Where(
                                b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                                b.Field<string>("Brand").Equals(cbobrand.SelectedItem.ToString()) &&
                                b.Field<string>("Sidewall").Equals(cbosidewall.SelectedItem.ToString()) &&
                                b.Field<string>("TyreType").Equals(cbotype.SelectedItem.ToString()) &&
                                b.Field<string>("TyreSize").Equals(cbotyresize.SelectedItem.ToString()) &&
                                b.Field<string>("TyreRim").Equals(cborim.SelectedItem.ToString())
                                ).Select(A => A.Field<string>("ProcessID")).Distinct().ToList();
                txtCurrProcessID.Text = lst[0].ToString();

                if ((txtStencil.Text.Length > 9 && txtStencil.Text.Length < 13) && txtCurrProcessID.Text != "")
                    txtBarcode.Text = (txtCurrProcessID.Text + txtStencil.Text + cboGrade.Text).ToUpper();
                if (txtBarcode.Text != "")
                    btnSAVE.Visible = true;
            }
        }
        private void cboPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboPlatform.SelectedItem.ToString() != "CHOOSE")
                {
                    List<string> lst = dtProcessIdDetails.AsEnumerable().Where(
                            b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString())
                            ).Select(A => A.Field<string>("Brand")).Distinct().ToList();
                    lst.Insert(0, "CHOOSE");
                    cbobrand.DataSource = lst;
                    txtBarcode.Text = "";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cbobrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbobrand.SelectedItem.ToString() != "CHOOSE")
                {
                    List<string> lst = dtProcessIdDetails.AsEnumerable().Where(
                        b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                        b.Field<string>("Brand").Equals(cbobrand.SelectedItem.ToString())
                        ).Select(A => A.Field<string>("Sidewall")).Distinct().ToList();
                    lst.Insert(0, "CHOOSE");
                    cbosidewall.DataSource = lst;
                    txtBarcode.Text = "";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cbosidewall_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbosidewall.SelectedItem.ToString() != "CHOOSE")
                {
                    List<string> lst = dtProcessIdDetails.AsEnumerable().Where(
                         b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                             b.Field<string>("Brand").Equals(cbobrand.SelectedItem.ToString()) &&
                             b.Field<string>("Sidewall").Equals(cbosidewall.SelectedItem.ToString())
                         ).Select(A => A.Field<string>("TyreType")).Distinct().ToList();
                    lst.Insert(0, "CHOOSE");
                    cbotype.DataSource = lst;
                    txtBarcode.Text = "";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cbotype_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbotype.SelectedItem.ToString() != "CHOOSE")
                {
                    List<string> lst = dtProcessIdDetails.AsEnumerable().Where(
                         b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                         b.Field<string>("Brand").Equals(cbobrand.SelectedItem.ToString()) &&
                         b.Field<string>("Sidewall").Equals(cbosidewall.SelectedItem.ToString()) &&
                         b.Field<string>("TyreType").Equals(cbotype.SelectedItem.ToString())
                         ).Select(A => A.Field<string>("TyreSize")).Distinct().ToList();
                    lst.Insert(0, "CHOOSE");
                    cbotyresize.DataSource = lst;
                    txtBarcode.Text = "";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cbotyresize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbotyresize.SelectedItem.ToString() != "CHOOSE")
                {
                    List<string> lst = dtProcessIdDetails.AsEnumerable().Where(
                            b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                            b.Field<string>("Brand").Equals(cbobrand.SelectedItem.ToString()) &&
                            b.Field<string>("Sidewall").Equals(cbosidewall.SelectedItem.ToString()) &&
                            b.Field<string>("TyreType").Equals(cbotype.SelectedItem.ToString()) &&
                            b.Field<string>("TyreSize").Equals(cbotyresize.SelectedItem.ToString())
                            ).Select(A => A.Field<string>("TyreRim")).Distinct().ToList();
                    lst.Insert(0, "CHOOSE");
                    cborim.DataSource = lst;
                    txtBarcode.Text = "";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cborim_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtCurrProcessID.Text = "";
                if (cborim.SelectedItem.ToString() != "CHOOSE")
                {
                    List<string> lst = dtProcessIdDetails.AsEnumerable().Where(
                            b => b.Field<string>("Config").Equals(cboPlatform.SelectedItem.ToString()) &&
                            b.Field<string>("Brand").Equals(cbobrand.SelectedItem.ToString()) &&
                            b.Field<string>("Sidewall").Equals(cbosidewall.SelectedItem.ToString()) &&
                            b.Field<string>("TyreType").Equals(cbotype.SelectedItem.ToString()) &&
                            b.Field<string>("TyreSize").Equals(cbotyresize.SelectedItem.ToString()) &&
                            b.Field<string>("TyreRim").Equals(cborim.SelectedItem.ToString())
                            ).Select(A => A.Field<string>("ProcessID")).Distinct().ToList();
                    txtCurrProcessID.Text = lst[0].ToString();
                    txtBarcode.Text = "";
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSAVE_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                if (txtRemarks.Text == "")
                    lblErrMsg.Text = "Enter remarks";
                else if (txtBarcode.Text == "")
                    lblErrMsg.Text = "Choose proper values";
                else if (cboTyreRemarks.SelectedIndex <= 0)
                {
                    MessageBox.Show("Choose Tyre Modify Remarks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboTyreRemarks.Focus();
                }
                else
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@StencilNo", txtStencil.Text), 
                        new SqlParameter("@config", cboPlatform.Text), 
                        new SqlParameter("@tyresize", cbotyresize.Text), 
                        new SqlParameter("@tyrerim", cborim.Text), 
                        new SqlParameter("@tyretype", cbotype.Text), 
                        new SqlParameter("@brand", cbobrand.Text), 
                        new SqlParameter("@sidewall", cbosidewall.Text), 
                        new SqlParameter("@grade", cboGrade.Text), 
                        new SqlParameter("@barcode", txtBarcode.Text), 
                        new SqlParameter("@remarks", txtRemarks.Text), 
                        new SqlParameter("@Processid", txtCurrProcessID.Text), 
                        new SqlParameter("@prevProcessid", txtPrevProcessId.Text), 
                        new SqlParameter("@dateofmanufacture", dateTimePicker1.Text),
                        new SqlParameter("@tyre_modify_remarks", cboTyreRemarks.SelectedValue.ToString()),
                        new SqlParameter("@username", Program.strUserName)
                    };
                    int resp = dba.ExecuteNonQuery_SP("SP_UPD_EditBarcode_qualityControlDetails_v1", sp);
                    if (resp > 0)
                    {
                        Program.PreparePrintLable(txtBarcode.Text);
                        MessageBox.Show("RECORD UPDATED SUCCESSFULLY");
                        groupBox1.Visible = false;
                        txtStencil.Text = "";
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Visible = false;
                if (txtStencil.Text.Length >= 10 && txtStencil.Text.Length <= 12)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@stencilno", txtStencil.Text) };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("SP_SEL_Popup_qualityControlDetails", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["dispatchStatus"].ToString().ToUpper() == "YES")
                            MessageBox.Show("Stencil already dispatched to " + dt.Rows[0]["tyrestatus"].ToString() + " on " + Convert.ToDateTime(dt.Rows[0]["Dispatchdate"].ToString()).ToShortDateString());
                        else
                        {
                            groupBox1.Visible = true;
                            dateTimePicker1.MaxDate = DateTime.Now;
                            dateTimePicker1.Text = Convert.ToDateTime(dt.Rows[0].Field<DateTime>("dateofmanufacture").ToString()).ToString("dd-MMM-yyyy");
                            cboPlatform.SelectedIndex = cboPlatform.FindStringExact(dt.Rows[0].Field<string>("Config").ToString());
                            cbobrand.SelectedIndex = cbobrand.FindStringExact(dt.Rows[0].Field<string>("Brand").ToString());
                            cbosidewall.SelectedIndex = cbosidewall.FindStringExact(dt.Rows[0].Field<string>("Sidewall").ToString());
                            cbotype.SelectedIndex = cbotype.FindStringExact(dt.Rows[0].Field<string>("TyreType").ToString());
                            cbotyresize.SelectedIndex = cbotyresize.FindStringExact(dt.Rows[0].Field<string>("TyreSize").ToString());
                            cborim.SelectedIndex = cborim.FindStringExact(dt.Rows[0].Field<string>("TyreRim").ToString());
                            cboGrade.SelectedIndex = cboGrade.FindStringExact(dt.Rows[0].Field<string>("qualitygrade").ToString());
                            txtStencil.Text = dt.Rows[0].Field<string>("stencilno").ToString();
                            txtPrevProcessId.Text = dt.Rows[0].Field<string>("Processid").ToString();
                            txtRemarks.Text = dt.Rows[0].Field<string>("remarks") == null ? "" : dt.Rows[0].Field<string>("remarks").ToString();
                            btnSAVE.Visible = false;
                            cboTyreRemarks.SelectedIndex = cboTyreRemarks.FindStringExact(dt.Rows[0].Field<string>("tyre_modify_remarks").ToString());
                            txtBarcode.Text = dt.Rows[0].Field<string>("barcode") == null ? "" : dt.Rows[0].Field<string>("barcode").ToString().ToUpper();
                            if (txtBarcode.Text != "")
                                btnSAVE.Visible = true;
                            txtRemarks.SelectionStart = txtRemarks.Text.Length;
                        }
                    }
                    else
                        lblErrMsg.Text = "STENCIL NO DOES NOT EXIST";
                }
                else
                    lblErrMsg.Text = "INVALID STENCIL NO";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilProdModify", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtStencil_KeyDown(object sender, KeyEventArgs e)
        {
            lblErrMsg.Text = "";
            if (e.KeyValue == 13)
            {
                if (txtStencil.Text.Length > 9 && txtStencil.Text.Length < 13)
                    btnFind_Click(sender, e);
                else
                {
                    groupBox1.Visible = false;
                    lblErrMsg.Text = "INVALID STENCIL NO";
                }
            }
        }
    }
}
