using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace GT
{
    public partial class frmWeightMaster : Form
    {
        DBAccess dbCon = new DBAccess();
        public frmWeightMaster()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.cboWtClass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
            this.cboType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
            this.cboTyreSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
            this.txtHbw.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
            this.txtBase.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
            this.txtInterface.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
            this.txtCenter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);
            this.txtTread.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Enter_Keypress);

            this.txtHbw.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
            this.txtBase.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
            this.txtInterface.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
            this.txtCenter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
            this.txtTread.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
            this.cboWtClass.SelectedIndexChanged += new EventHandler(cboWtClass_IndexChange);
            this.cboType.SelectedIndexChanged += new EventHandler(cboType_IndexChange);
            this.cboTyreSize.SelectedIndexChanged += new EventHandler(cboTyreSize_IndexChange);
            this.txtHbw.KeyUp += new KeyEventHandler(this.txtTotal_Calc);
            this.txtBase.KeyUp += new KeyEventHandler(this.txtTotal_Calc);
            this.txtInterface.KeyUp += new KeyEventHandler(this.txtTotal_Calc);
            this.txtCenter.KeyUp += new KeyEventHandler(this.txtTotal_Calc);
            this.txtTread.KeyUp += new KeyEventHandler(this.txtTotal_Calc);
            this.txtHbw.LostFocus += new EventHandler(this.txt_LostFocus);
            this.txtBase.LostFocus += new EventHandler(this.txt_LostFocus);
            this.txtInterface.LostFocus += new EventHandler(this.txt_LostFocus);
            this.txtCenter.LostFocus += new EventHandler(this.txt_LostFocus);
            this.txtTread.LostFocus += new EventHandler(this.txt_LostFocus);
            panel1.Location = new Point(0, 0);
            DataGridViewCellStyle style = this.gvWeightMaster.ColumnHeadersDefaultCellStyle;
            style.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void frmWeightMaster_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Name == "WEIGHT STANDARAD VIEW")
                    panel3.Visible = false;
                else
                    this.gvWeightMaster.CellClick += new DataGridViewCellEventHandler(GridViewRow_SelectionChanged);

                DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_WeightMaster_WtClass", DBAccess.Return_Type.DataTable);

                DataRow dr = dt.NewRow();
                dr.ItemArray = new object[] { "CHOOSE" };
                dt.Rows.InsertAt(dr, 0);

                cboWtClass.DataSource = dt;
                cboWtClass.DisplayMember = "WtClass";
                cboWtClass.ValueMember = "WtClass";

                if (cboWtClass.Items.Count == 2)
                    cboWtClass.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txtNumericPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }

        private void txt_LostFocus(object sender, EventArgs e)
        {
            TextBox txtFoucs = sender as TextBox;
            if (txtFoucs.Text.Length == 0)
                txtFoucs.Text = "0";
        }

        private void cbo_Enter_Keypress(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                SendKeys.Send("{TAB}");
        }

        private void cboWtClass_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@WtClass", cboWtClass.Text) };
                DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_WeightMaster_TyreType", sp, DBAccess.Return_Type.DataTable);
                DataRow dr = dt.NewRow();
                dr.ItemArray = new object[] { "CHOOSE" };
                dt.Rows.InsertAt(dr, 0);

                cboType.DataSource = dt;
                cboType.DisplayMember = "TyreType";
                cboType.ValueMember = "TyreType";

                if (cboType.Items.Count == 2)
                    cboType.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (cboType.SelectedIndex > 0)
                {
                    txtHbw.Enabled = true;
                    txtBase.Enabled = true;
                    txtInterface.Enabled = true;
                    txtCenter.Enabled = true;
                    txtTread.Enabled = true;

                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@TyreType", cboType.Text) };
                    DataTable dt1 = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_WeightMaster_OtherInfoOfType", sp1, DBAccess.Return_Type.DataTable);
                    if (dt1.Rows.Count > 0)
                    {
                        DataRow row = dt1.Rows[0];
                        txtHbw.Enabled = row["LipPer"].ToString() != "" ? true : false;
                        txtBase.Enabled = row["BasePer"].ToString() != "" ? true : false;
                        txtInterface.Enabled = row["InterfacePer"].ToString() != "" ? true : false;
                        txtCenter.Enabled = row["CenterPer"].ToString() != "" ? true : false;
                        txtTread.Enabled = row["TreadPer"].ToString() != "" ? true : false;
                    }
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@WtClass", cboWtClass.Text), new SqlParameter("@TyreType", cboType.Text) };
                    DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_WeightMaster_TyreSize", sp, DBAccess.Return_Type.DataTable);

                    DataRow dr = dt.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dt.Rows.InsertAt(dr, 0);

                    cboTyreSize.DataSource = dt;
                    cboTyreSize.DisplayMember = "TyreSize";
                    cboTyreSize.ValueMember = "TyreSize";

                    if (cboTyreSize.Items.Count == 2)
                        cboTyreSize.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cboTyreSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@WtClass",cboWtClass.Text),
                    new SqlParameter("@TyreType",cboType.Text),
                    new SqlParameter("@TyreSize",cboTyreSize.Text),
                };
                DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_WeightMaster_WeightDetails", sp, DBAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                    Bind_Gridview(dt);
                else
                    gvWeightMaster.DataSource = null;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Gridview(DataTable dtList)
        {
            try
            {
                gvWeightMaster.DataSource = dtList;

                gvWeightMaster.Columns[0].HeaderText = "TYRE SIZE";
                gvWeightMaster.Columns[1].HeaderText = "RIM";
                gvWeightMaster.Columns[2].HeaderText = "MOULD UNIQUE CODE";
                gvWeightMaster.Columns[3].HeaderText = "H+BW";
                gvWeightMaster.Columns[4].HeaderText = "BASE";
                gvWeightMaster.Columns[5].HeaderText = "INTERFACE";
                gvWeightMaster.Columns[6].HeaderText = "CENTER";
                gvWeightMaster.Columns[7].HeaderText = "TREAD";
                gvWeightMaster.Columns[8].HeaderText = "TOTAL";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txtTotal_Calc(object sender, KeyEventArgs e)
        {
            try
            {
                decimal decHbw = txtHbw.Text.Length > 0 && Convert.ToDecimal(txtHbw.Text) > 0 ? Convert.ToDecimal(txtHbw.Text) : 0;
                decimal decBase = txtBase.Text.Length > 0 && Convert.ToDecimal(txtBase.Text) > 0 ? Convert.ToDecimal(txtBase.Text) : 0;
                decimal decInterface = txtInterface.Text.Length > 0 && Convert.ToDecimal(txtInterface.Text) > 0 ? Convert.ToDecimal(txtInterface.Text) : 0;
                decimal decCenter = txtCenter.Text.Length > 0 && Convert.ToDecimal(txtCenter.Text) > 0 ? Convert.ToDecimal(txtCenter.Text) : 0;
                decimal decTread = txtTread.Text.Length > 0 && Convert.ToDecimal(txtTread.Text) > 0 ? Convert.ToDecimal(txtTread.Text) : 0;

                txtTotalWt.Text = (decHbw + decBase + decInterface + decCenter + decTread).ToString();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void GridViewRow_SelectionChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == 0 || e.RowIndex > 0)
                {
                    txtSize.Text = (gvWeightMaster.Rows[e.RowIndex].Cells[0].Value).ToString();
                    txtRim.Text = (gvWeightMaster.Rows[e.RowIndex].Cells[1].Value).ToString();
                    txtMould.Text = (gvWeightMaster.Rows[e.RowIndex].Cells[2].Value).ToString();
                    txtHbw.Text = (gvWeightMaster.Rows[e.RowIndex].Cells[3].Value).ToString();
                    txtBase.Text = (gvWeightMaster.Rows[e.RowIndex].Cells[4].Value).ToString();
                    txtInterface.Text = (gvWeightMaster.Rows[e.RowIndex].Cells[5].Value).ToString();
                    txtCenter.Text = (gvWeightMaster.Rows[e.RowIndex].Cells[6].Value).ToString();
                    txtTread.Text = (gvWeightMaster.Rows[e.RowIndex].Cells[7].Value).ToString();
                    txtTotalWt.Text = (gvWeightMaster.Rows[e.RowIndex].Cells[8].Value).ToString();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cboWtClass.DataSource = null;
            cboType.DataSource = null;
            cboTyreSize.DataSource = null;
            Ctrl_Clear();
            frmWeightMaster_Load(sender, e);
        }

        private void Ctrl_Clear()
        {
            txtSize.Text = "";
            txtRim.Text = "";
            txtMould.Text = "";
            txtHbw.Text = "0";
            txtBase.Text = "0";
            txtInterface.Text = "0";
            txtCenter.Text = "0";
            txtTread.Text = "0";
            txtTotalWt.Text = "0";
            lblErrmsg.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboWtClass.Text == "CHOOSE")
                    lblErrmsg.Text = "Select weight class";
                else if (cboType.Text == "CHOOSE")
                    lblErrmsg.Text = "Select tyre type";
                else if (cboTyreSize.Text == "CHOOSE")
                    lblErrmsg.Text = "Select tyre size";
                else if (txtSize.Text == "" || txtRim.Text == "" || txtMould.Text == "")
                    lblErrmsg.Text = "Choose any one item in below list";
                else if (txtTotalWt.Text == "0")
                    lblErrmsg.Text = "Enter any one weight";
                else
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@LipWt", txtHbw.Text), 
                        new SqlParameter("@BaseWt", txtBase.Text), 
                        new SqlParameter("@InterfaceWt", txtInterface.Text), 
                        new SqlParameter("@CenterWt", txtCenter.Text), 
                        new SqlParameter("@TreadWt", txtTread.Text), 
                        new SqlParameter("@TotWt", txtTotalWt.Text), 
                        new SqlParameter("@UserName", Program.strUserName), 
                        new SqlParameter("@WtClass", cboWtClass.Text), 
                        new SqlParameter("@TyreType", cboType.Text), 
                        new SqlParameter("@TyreSize", txtSize.Text), 
                        new SqlParameter("@RimSize", txtRim.Text), 
                        new SqlParameter("@MouldCode", txtMould.Text) 
                    };
                    int resp = dbCon.ExecuteNonQuery_SP("SP_UPD_WeightMaster_WeightDetails", sp);
                    if (resp > 0)
                    {
                        MessageBox.Show("Weight details saved");
                        Ctrl_Clear();
                        cboTyreSize_IndexChange(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmWeightMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
