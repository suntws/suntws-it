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
using System.Runtime.InteropServices;

namespace GT
{
    public partial class frmOrderPressAssign : Form
    {
        DBAccess dba = new DBAccess();
        private static int Prod_ItemID = 0;
        public frmOrderPressAssign()
        {
            InitializeComponent();
        }
        private void frmOrderPressAssign_Load(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                btnComplete.Visible = false;
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Plant", Program.strPlantName), new SqlParameter("@NpStatus", "1") };
                DataTable dtCust = (DataTable)dba.ExecuteReader_SP("sp_sel_Prod_Customer", sp, DBAccess.Return_Type.DataTable);
                if (dtCust.Rows.Count > 0)
                {
                    DataRow dr = dtCust.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtCust.Rows.InsertAt(dr, 0);
                    cmbCustomerName.DataSource = dtCust;
                    cmbCustomerName.DisplayMember = "CustomerName";
                    cmbCustomerName.ValueMember = "CustomerCode";
                    if (cmbCustomerName.Items.Count == 2)
                        cmbCustomerName.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderPressAssign", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbWorkOrderNo.DataSource = null;
                txtOrderQuantity.Text = "";
                txtStockQuantity.Text = "";
                txtRequiredQuantity.Text = "";

                txtPlatform.Text = "";
                txtTyresize.Text = "";
                txtRimsize.Text = "";
                txtTyretype.Text = "";
                txtBrand.Text = "";
                txtsidewall.Text = "";
                txtItemqty.Text = "";
                lblProcessID.Text = "";
                chkPress.Items.Clear();

                dgvSavedRecords.DataSource = null;
                dgvSavedRecords.Columns.Clear();
                dgvSavedRecords.Rows.Clear();
                if (cmbCustomerName.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Plant", Program.strPlantName), 
                        new SqlParameter("@customercode", cmbCustomerName.SelectedValue.ToString()), 
                        new SqlParameter("@NpStatus", "1")
                    };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_Prod_WorkOrderNo", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dt.Rows.InsertAt(dr, 0);
                        cmbWorkOrderNo.DataSource = dt;
                        cmbWorkOrderNo.DisplayMember = "workorderno";
                        cmbWorkOrderNo.ValueMember = "NPID";
                    }
                    if (cmbWorkOrderNo.Items.Count == 2)
                        cmbWorkOrderNo.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderPressAssign", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbWorkOrderNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtOrderQuantity.Text = "";
                txtStockQuantity.Text = "";
                txtRequiredQuantity.Text = "";
                chkPress.Items.Clear();

                txtPlatform.Text = "";
                txtTyresize.Text = "";
                txtRimsize.Text = "";
                txtTyretype.Text = "";
                txtBrand.Text = "";
                txtsidewall.Text = "";
                txtItemqty.Text = "";
                lblProcessID.Text = "";

                dgvSavedRecords.DataSource = null;
                dgvSavedRecords.Columns.Clear();
                dgvSavedRecords.Rows.Clear();
                btnComplete.Visible = false;

                if (cmbWorkOrderNo.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@NpID", Convert.ToInt32(cmbWorkOrderNo.SelectedValue.ToString())) };
                    DataSet ds = (DataSet)dba.ExecuteReader_SP("sp_sel_PreparationplaningData", sp, DBAccess.Return_Type.DataSet);
                    DataTable dt = ds.Tables[0];
                    DataTable dtitem = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        txtStockQuantity.Text = dt.Rows[0]["StockQuantity"].ToString();
                        txtOrderQuantity.Text = dt.Rows[0]["OrderQuantity"].ToString();
                        txtRequiredQuantity.Text = dt.Rows[0]["RequiredQuantity"].ToString();
                    }
                    if (dtitem.Rows.Count > 0)
                    {
                        dgvSavedRecords.DataSource = dtitem;
                        gridProperty();

                        if (dgvSavedRecords.Rows.Count == Convert.ToInt32(ds.Tables[2].Rows[0]["RecCount"].ToString()))
                            btnComplete.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderPressAssign", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void gridProperty()
        {
            try
            {
                dgvSavedRecords.Columns[0].Visible = false;
                dgvSavedRecords.Columns[1].Visible = false;
                dgvSavedRecords.Columns[2].Width = 80; //Platform
                dgvSavedRecords.Columns[3].Width = 150; //tyre size
                dgvSavedRecords.Columns[4].Width = 50; //rim
                dgvSavedRecords.Columns[5].Width = 70; //type
                dgvSavedRecords.Columns[6].Width = 80; //brand
                dgvSavedRecords.Columns[7].Width = 80; //sidewall
                dgvSavedRecords.Columns[8].Width = 80; //process-id
                dgvSavedRecords.Columns[8].HeaderText = "PROCESS-ID";
                dgvSavedRecords.Columns[9].Width = 60; //order qty
                dgvSavedRecords.Columns[9].HeaderText = "ORDER";
                dgvSavedRecords.Columns[10].Width = 60; //sotck qty
                dgvSavedRecords.Columns[10].HeaderText = "STOCK";
                dgvSavedRecords.Columns[11].Width = 100; //req qty
                dgvSavedRecords.Columns[11].HeaderText = "PRODUCTION";
                dgvSavedRecords.Columns[12].Width = 150;
                dgvSavedRecords.Columns[12].HeaderText = "PRESS";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderPressAssign", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnComplete.Visible = false;
                DataTable dtPress = new DataTable();
                dtPress.Columns.Add("Press", typeof(string));
                foreach (int checkedPress in chkPress.CheckedIndices)
                    dtPress.Rows.Add(chkPress.Items[checkedPress].ToString());

                if (cmbCustomerName.SelectedIndex <= 0 || cmbWorkOrderNo.SelectedIndex <= 0 || txtStockQuantity.Text == "" ||
                    txtOrderQuantity.Text == "" || txtRequiredQuantity.Text == "" || txtPlatform.Text == "")
                    MessageBox.Show("SELECT JATHAGAM FROM BELOW ORDER LIST");
                else if (dtPress.Rows.Count == 0)
                    MessageBox.Show("SELECT PRESS");
                else
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@NPID", Convert.ToInt32(cmbWorkOrderNo.SelectedValue.ToString())), 
                        new SqlParameter("@Prod_ItemID", Prod_ItemID), 
                        new SqlParameter("@OrderItem_Press_Dt", dtPress) 
                    };
                    Int32 dtRecCount = (Int32)dba.ExecuteScalar_SP("sp_Ins_Press_OrderItem", sp);
                    if (dgvSavedRecords.Rows.Count == dtRecCount)
                        btnComplete.Visible = true;

                    chkPress.Items.Clear();
                    lblErrMsg.Text = "";
                    txtBrand.Text = "";
                    txtPlatform.Text = "";
                    txtBrand.Text = "";
                    txtsidewall.Text = "";
                    txtTyretype.Text = "";
                    txtTyresize.Text = "";
                    txtRimsize.Text = "";
                    lblProcessID.Text = "";
                    txtItemqty.Text = "";

                    cmbWorkOrderNo_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderPressAssign", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                dgvSavedRecords.ClearSelection();
                txtBrand.Text = "";
                Prod_ItemID = 0;
                txtPlatform.Text = "";
                txtBrand.Text = "";
                txtsidewall.Text = "";
                txtTyretype.Text = "";
                txtTyresize.Text = "";
                txtRimsize.Text = "";
                lblProcessID.Text = "";
                txtItemqty.Text = "";
                cmbCustomerName.DataSource = null;
                cmbWorkOrderNo.DataSource = null;
                txtOrderQuantity.Text = "";
                txtRequiredQuantity.Text = "";
                txtStockQuantity.Text = "";
                chkPress.Items.Clear();
                frmOrderPressAssign_Load(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderPressAssign", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@NPID", Convert.ToInt32(cmbWorkOrderNo.SelectedValue.ToString())), new SqlParameter("@NpStatus", "3") };
                int resp = dba.ExecuteNonQuery_SP("sp_upd_status_NewProductionMaster", sp);
                if (resp > 0)
                {
                    btnClear_Click(sender, e);
                    MessageBox.Show("Record Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnComplete.Hide();
                }
                else
                    MessageBox.Show("Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderPressAssign", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void dgvSavedRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Prod_ItemID = 0;
                lblProcessID.Text = "";
                txtPlatform.Text = "";
                txtTyresize.Text = "";
                txtRimsize.Text = "";
                txtTyretype.Text = "";
                txtBrand.Text = "";
                txtsidewall.Text = "";
                txtItemqty.Text = "";
                lblProcessID.Text = "";
                chkPress.Items.Clear();
                if (e.RowIndex >= 0)
                {
                    Prod_ItemID = Convert.ToInt32(dgvSavedRecords.Rows[e.RowIndex].Cells["Prod_ItemID"].Value.ToString());
                    txtPlatform.Text = dgvSavedRecords.Rows[e.RowIndex].Cells["PLATFORM"].Value.ToString();
                    txtBrand.Text = dgvSavedRecords.Rows[e.RowIndex].Cells["BRAND"].Value.ToString();
                    txtsidewall.Text = dgvSavedRecords.Rows[e.RowIndex].Cells["SIDEWALL"].Value.ToString();
                    txtTyretype.Text = dgvSavedRecords.Rows[e.RowIndex].Cells["TYPE"].Value.ToString();
                    txtTyresize.Text = dgvSavedRecords.Rows[e.RowIndex].Cells["SIZE"].Value.ToString();
                    txtRimsize.Text = dgvSavedRecords.Rows[e.RowIndex].Cells["RIM"].Value.ToString();
                    lblProcessID.Text = dgvSavedRecords.Rows[e.RowIndex].Cells["PROCESSID"].Value.ToString();
                    txtItemqty.Text = dgvSavedRecords.Rows[e.RowIndex].Cells["REQQTY"].Value.ToString();

                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TyreSize", txtTyresize.Text) };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringPressList_SizeWise", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                            chkPress.Items.Add(dr["CuringUnit"].ToString());
                    }

                    SqlParameter[] spPress = new SqlParameter[] { new SqlParameter("@Prod_ItemID", Prod_ItemID) };
                    DataTable dtPress = (DataTable)dba.ExecuteReader_SP("sp_sel_Assign_ItemProductionPress", spPress, DBAccess.Return_Type.DataTable);
                    if (dtPress.Rows.Count > 0)
                    {
                        for (int i = 0; i < chkPress.Items.Count; i++)
                        {
                            foreach (DataRow row in dtPress.Select("Press='" + chkPress.Items[i].ToString() + "'"))
                            {
                                chkPress.SetItemChecked(i, true);
                            }
                        }
                    }
                    if (chkPress.Items.Count == 1)
                        chkPress.SetItemChecked(0, true);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderPressAssign", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}