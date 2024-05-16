using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace GT
{
    public partial class frmWeightTolerance : Form
    {
        DBAccess dbCon = new DBAccess();
        public frmWeightTolerance()
        {
            InitializeComponent();
        }

        private void frmWeightTolerance_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.Location = new Point(20, 20);
                DataGridViewCellStyle style = this.gvWtVariant.ColumnHeadersDefaultCellStyle;
                style.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                this.KeyPreview = true;
                this.gvWtVariant.CellClick += new DataGridViewCellEventHandler(GridViewRow_SelectionChanged);

                Bind_WeightVariant();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightTolerance", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_WeightVariant()
        {
            try
            {
                DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_LST_WeightVariant_WeightDetails", DBAccess.Return_Type.DataTable);

                gvWtVariant.DataSource = dt;
                gvWtVariant.Columns[0].HeaderText = "S.No.";
                gvWtVariant.Columns[1].HeaderText = "CATEGORY";
                gvWtVariant.Columns[2].HeaderText = "LAYER";
                gvWtVariant.Columns[3].HeaderText = "SEPC";
                gvWtVariant.Columns[4].HeaderText = "-WT";
                gvWtVariant.Columns[5].HeaderText = "+WT";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightTolerance", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void GridViewRow_SelectionChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == 0 || e.RowIndex > 0)
                {
                    lblRowID.Text = (gvWtVariant.Rows[e.RowIndex].Cells[0].Value).ToString();
                    lblWeightVariant.Text = (gvWtVariant.Rows[e.RowIndex].Cells[3].Value).ToString();
                    txtWtMinus.Text = (gvWtVariant.Rows[e.RowIndex].Cells[4].Value).ToString();
                    txtWtPlus.Text = (gvWtVariant.Rows[e.RowIndex].Cells[5].Value).ToString();
                    label3.Text = (gvWtVariant.Rows[e.RowIndex].Cells[1].Value).ToString();
                    label5.Text = (gvWtVariant.Rows[e.RowIndex].Cells[2].Value).ToString();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightTolerance", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblWeightVariant.Text = "";
            txtWtMinus.Text = "";
            txtWtPlus.Text = "";
            lblErrMsg.Text = "";
            label3.Text = "";
            label5.Text = "";
            Bind_WeightVariant();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtWtMinus.Text == "")
                    lblErrMsg.Text = "Enter weight minus";
                else if (txtWtPlus.Text == "")
                    lblErrMsg.Text = "Enter weight plus";
                else if (label3.Text == "" && label5.Text == "" && lblWeightVariant.Text == "")
                    lblErrMsg.Text = "Choose anyone weight variant list";
                else
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@ModifiedName",Program.strUserName),
                        new SqlParameter("@HBWTYPE",label3.Text ),
                        new SqlParameter("@Layer",label5.Text),
                        new SqlParameter("@SpecType",lblWeightVariant.Text)
                    };

                    int resp = dbCon.ExecuteNonQuery_SP("SP_UPD_WeightVariant_WeightDetails", sp);
                    if (resp > 0)
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@RowID",lblRowID.Text),
                        new SqlParameter("@HBWTYPE",label3.Text),
                        new SqlParameter("@Layer",label5.Text),
                        new SqlParameter("@SpecType",lblWeightVariant.Text),
                        new SqlParameter("@MinusWt",txtWtMinus.Text),
                        new SqlParameter("@PlusWt",txtWtPlus.Text),
                        new SqlParameter("@WtStatus",1),
                        new SqlParameter("@CreatedName", Program.strUserName ),
                        };
                        resp = dbCon.ExecuteNonQuery_SP("SP_INS_WeightVariant_WeightDetails", sp1);
                        if (resp > 0)
                        {
                            MessageBox.Show("Weight variant details saved");
                            btnClear_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightVariant", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
