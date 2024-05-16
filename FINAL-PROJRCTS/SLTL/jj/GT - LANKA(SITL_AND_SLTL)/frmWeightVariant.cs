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
using System.Reflection;

namespace GT
{
    public partial class frmWeightVariant : Form
    {
        DBAccess dbCon = new DBAccess();
        public frmWeightVariant()
        {
            InitializeComponent();
        }

        private void frmWeightVariant_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.Location = new Point(20, 20);
                DataGridViewCellStyle style1 = this.gvWtClassMaster.ColumnHeadersDefaultCellStyle;
                style1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Bind_GridView();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightVariant", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_GridView()
        {
            try
            {
                DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_WeightClassMaster_WtClass", DBAccess.Return_Type.DataTable);
                gvWtClassMaster.DataSource = dt;
                gvWtClassMaster.Columns[0].HeaderText = "WEIGHT CLASS";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightVariant", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            lblErrMsg.Text = "";
            progressBar1.Value = 0;
            btnSave.Text = "SAVE";
            Bind_GridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "")
                {
                    lblErrMsg.Text = "Enter Weight Class";
                    textBox1.Focus();
                }
                else
                {
                    if (btnSave.Text == "SAVE")
                    {
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@WtClass", textBox1.Text) };
                        DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_WeightClassMaster_MasterDetails", sp, DBAccess.Return_Type.DataTable);
                        if (dt.Rows.Count > 0)
                            lblErrMsg.Text = "Weight Class Already Existing";
                        else
                        {
                            SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@WtClass", textBox1.Text), new SqlParameter("@UserName", Program.strUserName) };
                            int resp = dbCon.ExecuteNonQuery_SP("SP_INS_WeightClassMaster_MasterDetails", sp1);
                            if (resp > 0)
                            {
                                MessageBox.Show("Record successfully saved");
                                btnClear_Click(sender, e);
                            }
                        }
                    }
                    else if (btnSave.Text == "SUSPEND")
                    {
                        string strRemarks = Microsoft.VisualBasic.Interaction.InputBox("ENTER REASON FOR SUSPEND", "Title", "");
                        if (strRemarks != "" && MessageBox.Show(strRemarks, "WEIGHT VARIANT SUSPEND", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            SqlParameter[] spDel = new SqlParameter[] { 
                                new SqlParameter("@Val1", textBox1.Text), 
                                new SqlParameter("@Val2", ""), 
                                new SqlParameter("@Val3", ""),   
                                new SqlParameter("@UserName", Program.strUserName),
                                new SqlParameter("@Category", "WEIGHT VARIANT"),
                                new SqlParameter("@Remarks", ("SUSPEND: " + strRemarks))  
                            };
                            int resp = dbCon.ExecuteNonQuery_SP("sp_upd_masterdata_suspend", spDel);
                            if (resp > 0)
                            {
                                MessageBox.Show("Weight variant successfully " + btnSave.Text.ToLower() + "ed");
                                btnClear_Click(null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightVariant", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void gvWtClassMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox1.Text = gvWtClassMaster.Rows[e.RowIndex].Cells["WtClass"].Value.ToString();
                btnSave.Text = "SUSPEND";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightVariant", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
