using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace GT
{
    public partial class frmUpdateStencilNo : Form
    {
        DBAccess dba = new DBAccess();
        public frmUpdateStencilNo()
        {
            InitializeComponent();
        }

        private void frmUpdateStencilNo_Load(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
            btnUpdate.Enabled = false;
            txtNewStencil.Enabled = false;
        }

        private void btnFindStencil_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPrevStencil.Text.Length == 10)
                {
                    SqlParameter[] spG = new SqlParameter[] { new SqlParameter("@StencilNo", txtPrevStencil.Text.Trim()) };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_stencil_wise_data", spG, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        SqlParameter[] spC = new SqlParameter[] { new SqlParameter("@StencilNo", txtPrevStencil.Text.Trim()) };
                        DataTable dt1 = (DataTable)dba.ExecuteReader_SP("sp_chk_stencil_barcoded", spC, DBAccess.Return_Type.DataTable);
                        if (dt1.Rows.Count == 0)
                        {
                            dataGridView1.DataSource = dt;
                            txtPrevStencil.Enabled = false;
                            btnFindStencil.Enabled = false;
                            txtNewStencil.Enabled = true;
                            btnUpdate.Enabled = true;
                        }
                        else
                            lblErrorMsg.Text = "Stencil no Already barcoded on " + dt1.Rows[0]["CreateDate"].ToString() + ". You Cannot change it.";
                    }
                    else
                        lblErrorMsg.Text = "Stencil No not found in production details";
                }
                else
                    lblErrorMsg.Text = "Invalid Previous Stencil No.";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUpdateStencilNo", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewStencil.Text.Length == 10)
                {
                    SqlParameter[] spC = new SqlParameter[] { new SqlParameter("@StencilNo", txtNewStencil.Text.Trim()) };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_chk_stencil_produce", spC, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                        lblErrorMsg.Text = "This StencilNo already Existing";
                    else
                    {
                        lblErrorMsg.Text = "";
                        SqlParameter[] spU = new SqlParameter[] { 
                            new SqlParameter("@PrevStencilNo", txtPrevStencil.Text.Trim()), 
                            new SqlParameter("@NewStencilNo", txtNewStencil.Text.Trim()) 
                        };
                        dba.ExecuteNonQuery_SP("sp_upd_stencilno", spU);
                        MessageBox.Show("Stencil No updated Successfully");
                        btnClear_Click(null, null);
                    }
                }
                else
                    lblErrorMsg.Text = "Invalid New Stencil No.";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUpdateStencilNo", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtPrevStencil.Text = "";
                txtPrevStencil.Enabled = true;
                btnFindStencil.Enabled = true;
                dataGridView1.DataSource = null;
                txtNewStencil.Text = "";
                txtNewStencil.Enabled = false;
                btnUpdate.Enabled = false;
                lblErrorMsg.Text = "";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmUpdateStencilNo", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
