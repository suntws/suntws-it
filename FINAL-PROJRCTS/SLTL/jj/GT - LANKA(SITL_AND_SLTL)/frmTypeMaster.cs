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
    public partial class frmTypeMaster : Form
    {
        DBAccess dba = new DBAccess();
        public frmTypeMaster()
        {
            InitializeComponent();
            if (Program.strPlantName == "SITL" || Program.strPlantName == "SLTL")
            {
                pnlCategory.Visible = false;
                rdoBoth.Checked = true;
            }
        }

        private void frmTypeMaster_Load(object sender, EventArgs e)
        {
            try
            {
                panel2.Location = new Point(0, 0);
                DataGridViewCellStyle style = this.gvTypeMaster.ColumnHeadersDefaultCellStyle;
                style.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                this.KeyPreview = true;
                this.txtType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtHbw.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtHbwPer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtBase.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtBasePer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtInterface.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtInterfacePer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtCenter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtCenterPer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtTread.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtTreadPer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);

                this.txtHbwPer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtBasePer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtInterfacePer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtCenterPer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtTreadPer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.gvTypeMaster.CellClick += new DataGridViewCellEventHandler(GridViewRow_SelectionChanged);
                Bind_Gridview();
                bindOriginFriction();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Gridview()
        {
            try
            {
                DataTable dt = (DataTable)dba.ExecuteReader_SP("SP_LST_TypeMaster_Compounds", DBAccess.Return_Type.DataTable);
                gvTypeMaster.DataSource = dt;
                gvTypeMaster.Columns[0].HeaderText = "TYPE";
                gvTypeMaster.Columns[0].Width = 90;
                gvTypeMaster.Columns[1].HeaderText = "H+BW";
                gvTypeMaster.Columns[1].Width = 90;
                gvTypeMaster.Columns[2].HeaderText = "%";
                gvTypeMaster.Columns[2].Width = 30;
                gvTypeMaster.Columns[3].HeaderText = "BASE";
                gvTypeMaster.Columns[3].Width = 90;
                gvTypeMaster.Columns[4].HeaderText = "%";
                gvTypeMaster.Columns[4].Width = 30;
                gvTypeMaster.Columns[5].HeaderText = "INTERFACE";
                gvTypeMaster.Columns[5].Width = 150;
                gvTypeMaster.Columns[6].HeaderText = "%";
                gvTypeMaster.Columns[6].Width = 30;
                gvTypeMaster.Columns[7].HeaderText = "CENTER";
                gvTypeMaster.Columns[7].Width = 90;
                gvTypeMaster.Columns[8].HeaderText = "%";
                gvTypeMaster.Columns[8].Width = 30;
                gvTypeMaster.Columns[9].HeaderText = "TREAD";
                gvTypeMaster.Columns[9].Width = 90;
                gvTypeMaster.Columns[10].HeaderText = "%";
                gvTypeMaster.Columns[10].Width = 30;
                gvTypeMaster.Columns[11].HeaderText = "CURE 1";
                gvTypeMaster.Columns[11].Width = 50;
                gvTypeMaster.Columns[12].HeaderText = "CURE 2";
                gvTypeMaster.Columns[12].Width = 50;
                gvTypeMaster.Columns[13].HeaderText = "CURE 3";
                gvTypeMaster.Columns[13].Width = 50;
                gvTypeMaster.Columns[14].HeaderText = "BW STATUS";
                gvTypeMaster.Columns[14].Width = 50;
                gvTypeMaster.Columns[15].HeaderText = "CATEGORY";
                gvTypeMaster.Columns[15].Width = 80;
                gvTypeMaster.Columns[16].Visible = false;
                gvTypeMaster.Columns[17].Visible = false;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindOriginFriction()
        {
            try
            {
                DataTable dtOrigin = (DataTable)dba.ExecuteReader_SP("SP_LST_TypeMaster_Friction", DBAccess.Return_Type.DataTable);
                if (dtOrigin.Rows.Count > 0)
                {
                    int maxY = pnlOriginFriction.Height - 20;
                    int maxWidth = pnlOriginFriction.Width - 10;
                    int curHeight = 0;
                    int curWidth = 5;
                    for (int i = 0; i < dtOrigin.Rows.Count; i++)
                    {
                        CheckBox chk = new CheckBox();
                        chk.Text = dtOrigin.Rows[i]["FrictionOrigin"].ToString();
                        if (i > 0) curHeight += 30;
                        chk.Location = new Point(curWidth, curHeight);
                        pnlOriginFriction.Controls.Add(chk);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtType.Text = "";
                txtHbw.Text = "";
                txtHbwPer.Text = "";
                txtBase.Text = "";
                txtBasePer.Text = "";
                txtInterface.Text = "";
                txtInterfacePer.Text = "";
                txtCenter.Text = "";
                txtCenterPer.Text = "";
                txtTread.Text = "";
                txtTreadPer.Text = "";
                btnSave.Text = "SAVE";
                btnSuspend.Visible = false;
                txtType.Enabled = true;
                progressBar1.Value = 0;
                rdbbwstatus.Checked = false;
                rdbbwstatusno.Checked = false;
                rdoCureFamily1.Checked = false;
                rdoCureFamily2.Checked = false;
                rdoCureFamily3.Checked = false;
                rdoBoth.Checked = false;
                rdoDomestic.Checked = false;
                rdoDomestic.Checked = false;
                pnlCategory.Enabled = true;
                cboType.DataSource = null;
                Bind_Gridview();
                deSelCheck();
                if (Program.strPlantName == "SITL" || Program.strPlantName == "SLTL")
                {
                    pnlCategory.Visible = false;
                    rdoBoth.Checked = true;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txt_Enter_Keypress(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                SendKeys.Send("{TAB}");
        }

        private void txtNumericPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }

        private void GridViewRow_SelectionChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == 0 || e.RowIndex > 0)
                {
                    deSelCheck();
                    txtType.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[0].Value).ToString();
                    txtType_Leave(null, null);
                    txtHbw.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[1].Value).ToString();
                    txtHbwPer.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[2].Value).ToString();
                    txtBase.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[3].Value).ToString();
                    txtBasePer.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[4].Value).ToString();
                    txtInterface.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[5].Value).ToString();
                    txtInterfacePer.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[6].Value).ToString();
                    txtCenter.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[7].Value).ToString();
                    txtCenterPer.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[8].Value).ToString();
                    txtTread.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[9].Value).ToString();
                    txtTreadPer.Text = (gvTypeMaster.Rows[e.RowIndex].Cells[10].Value).ToString();
                    if ((gvTypeMaster.Rows[e.RowIndex].Cells[11].Value).ToString() == "YES")
                        rdoCureFamily1.Checked = true;
                    else if ((gvTypeMaster.Rows[e.RowIndex].Cells[12].Value).ToString() == "YES")
                        rdoCureFamily2.Checked = true;
                    else if ((gvTypeMaster.Rows[e.RowIndex].Cells[13].Value).ToString() == "YES")
                        rdoCureFamily3.Checked = true;
                    else
                    {
                        rdoCureFamily1.Checked = false;
                        rdoCureFamily2.Checked = false;
                        rdoCureFamily3.Checked = false;
                    }

                    if ((gvTypeMaster.Rows[e.RowIndex].Cells[14].Value).ToString() == "YES")
                        rdbbwstatus.Checked = true;
                    else if ((gvTypeMaster.Rows[e.RowIndex].Cells[14].Value).ToString() == "NO")
                        rdbbwstatusno.Checked = true;

                    if ((gvTypeMaster.Rows[e.RowIndex].Cells[15].Value).ToString() == "BOTH")
                        rdoBoth.Checked = true;
                    else if ((gvTypeMaster.Rows[e.RowIndex].Cells[15].Value).ToString() == "DOMESTIC")
                        rdoDomestic.Checked = true;
                    else if ((gvTypeMaster.Rows[e.RowIndex].Cells[15].Value).ToString() == "EXPORT")
                        rdoExport.Checked = true;

                    if ((gvTypeMaster.Rows[e.RowIndex].Cells[15].Value).ToString() != "")
                        pnlCategory.Enabled = false;

                    if ((gvTypeMaster.Rows[e.RowIndex].Cells[17].Value).ToString() != "")
                        cboType.SelectedIndex = cboType.FindStringExact((gvTypeMaster.Rows[e.RowIndex].Cells[17].Value).ToString());

                    selectOriginFriction(txtType.Text);

                    btnSave.Text = "UPDATE";
                    btnSuspend.Visible = true;
                    txtType.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void selectOriginFriction(string typeid)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TyreType", typeid) };
                DataTable dtOringin = (DataTable)dba.ExecuteReader_SP("SP_LST_TypeMaster_FrictionOfType", sp, DBAccess.Return_Type.DataTable);
                if (dtOringin.Rows.Count > 0)
                {
                    foreach (CheckBox chk in pnlOriginFriction.Controls)
                    {
                        List<string> lstType = dtOringin.AsEnumerable().Where(n => n.Field<string>("FrictionOrigin").Equals(chk.Text)).Select(n => n.Field<string>("FrictionOrigin").ToString()).ToList();
                        if (lstType.Count > 0) { chk.Checked = true; }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void deSelCheck()
        {
            foreach (CheckBox chk in pnlOriginFriction.Controls)
            {
                chk.Checked = false;
                chk.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int noOfChecked = 0;
                foreach (CheckBox chk in pnlOriginFriction.Controls)
                {
                    if (chk.Checked == true)
                    {
                        noOfChecked += 1;
                        break;
                    }
                }

                if (txtType.Text == "")
                {
                    MessageBox.Show("Enter type");
                    txtType.Focus();
                }
                else if (txtHbw.Text != "" && txtHbwPer.Text == "")
                {
                    MessageBox.Show("Enter H+BW percentage");
                    txtHbwPer.Focus();
                }
                else if (txtBase.Text != "" && txtBasePer.Text == "")
                {
                    MessageBox.Show("Enter Base percentage");
                    txtBasePer.Focus();
                }
                else if (txtInterface.Text != "" && txtInterfacePer.Text == "")
                {
                    MessageBox.Show("Enter Interface percentage");
                    txtInterfacePer.Focus();
                }
                else if (txtCenter.Text != "" && txtCenterPer.Text == "")
                {
                    MessageBox.Show("Enter Center percentage");
                    txtCenterPer.Focus();
                }
                else if (txtTread.Text != "" && txtTreadPer.Text == "")
                {
                    MessageBox.Show("Enter Tread percentage");
                    txtTreadPer.Focus();
                }
                else if (rdoCureFamily1.Checked == false && rdoCureFamily2.Checked == false && rdoCureFamily3.Checked == false)
                    MessageBox.Show("Choose Cure Family");
                else if (rdbbwstatus.Checked == false && rdbbwstatusno.Checked == false)
                    MessageBox.Show("Choose BW status");
                else if (rdoBoth.Checked == false && rdoDomestic.Checked == false && rdoExport.Checked == false)
                    MessageBox.Show("Choose type category");
                else if (cboType.SelectedIndex <= 0)
                    MessageBox.Show("Choose Equal Type");
                else if (noOfChecked <= 0)
                    MessageBox.Show("Choose Source Friction for this TyreType");
                else
                {
                    bool boolBlendMasterExists = true;
                    DataTable dtBlendCategory = (DataTable)dba.ExecuteReader_SP("sp_sel_BlendMaster_CategoryList", DBAccess.Return_Type.DataTable);
                    if (txtHbw.Text != "")
                    {
                        boolBlendMasterExists = false;
                        foreach (DataRow lRow in dtBlendCategory.Select("Category='" + txtHbw.Text + "'"))
                        {
                            boolBlendMasterExists = true;
                        }
                        if (!boolBlendMasterExists)
                            MessageBox.Show("H+BW Code is not available in blend Master, Please inform Admin.");
                    }
                    if (txtBase.Text != "")
                    {
                        boolBlendMasterExists = false;
                        foreach (DataRow bRow in dtBlendCategory.Select("Category='" + txtBase.Text + "'"))
                        {
                            boolBlendMasterExists = true;
                        }
                        if (!boolBlendMasterExists)
                            MessageBox.Show("Base Code is not available in blend Master, Please inform Admin.");
                    }
                    if (txtInterface.Text != "")
                    {
                        boolBlendMasterExists = false;
                        foreach (DataRow iRow in dtBlendCategory.Select("Category='" + txtInterface.Text + "'"))
                        {
                            boolBlendMasterExists = true;
                        }
                        if (!boolBlendMasterExists)
                            MessageBox.Show("Interface Code is not available in blend Master, Please inform Admin.");
                    }

                    decimal decHbw = txtHbwPer.Text.Length > 0 ? Convert.ToDecimal(txtHbwPer.Text) : 0;
                    decimal decBase = txtBasePer.Text.Length > 0 ? Convert.ToDecimal(txtBasePer.Text) : 0;
                    decimal decInterface = txtInterfacePer.Text.Length > 0 ? Convert.ToDecimal(txtInterfacePer.Text) : 0;
                    decimal decCenter = txtCenterPer.Text.Length > 0 ? Convert.ToDecimal(txtCenterPer.Text) : 0;
                    decimal decTread = txtTreadPer.Text.Length > 0 ? Convert.ToDecimal(txtTreadPer.Text) : 0;

                    decimal totPer = decHbw + decBase + decInterface + decCenter + decTread;
                    if (totPer < 100 || totPer > 100)
                        MessageBox.Show("Total percentage value should be 100");
                    else if (totPer == 100)
                    {
                        bool strbwstatus = false;
                        if (rdbbwstatus.Checked == true)
                            strbwstatus = true;
                        else if (rdbbwstatusno.Checked == true)
                            strbwstatus = false;

                        string strTypeCategory = "";
                        if (rdoBoth.Checked == true)
                            strTypeCategory = rdoBoth.Text;
                        else if (rdoDomestic.Checked == true)
                            strTypeCategory = rdoDomestic.Text;
                        else if (rdoExport.Checked == true)
                            strTypeCategory = rdoExport.Text;

                        string strQuery = string.Empty;
                        int mode = 0;

                        if (btnSave.Text == "SAVE")
                        {
                            SqlParameter[] spChk = new SqlParameter[] { new SqlParameter("@TyreType", txtType.Text), new SqlParameter("@TypeCategory", strTypeCategory) };
                            DataTable dtChk = (DataTable)dba.ExecuteReader_SP("SP_chk_TypeMaster", spChk, DBAccess.Return_Type.DataTable);
                            if (dtChk.Rows.Count > 0)
                            {
                                MessageBox.Show("Type with category already existing");
                                return;
                            }
                            else
                                mode = 1;
                        }
                        else if (btnSave.Text == "UPDATE") mode = 2;

                        if (mode > 0)
                        {
                            SqlParameter[] sp1 = new SqlParameter[] {
                                new SqlParameter("@TyreType", txtType.Text == "" ? string.Empty : txtType.Text),
                                new SqlParameter("@Lip", txtHbw.Text == "" ? string.Empty : txtHbw.Text),
                                new SqlParameter("@LipPer", txtHbwPer.Text == "" ? string.Empty : txtHbwPer.Text),
                                new SqlParameter("@Base", txtBase.Text == "" ? string.Empty : txtBase.Text),
                                new SqlParameter("@BasePer", txtBasePer.Text == "" ? string.Empty : txtBasePer.Text),
                                new SqlParameter("@Interface", txtInterface.Text == "" ? string.Empty : txtInterface.Text),
                                new SqlParameter("@InterfacePer", txtInterfacePer.Text == "" ? string.Empty : txtInterfacePer.Text),
                                new SqlParameter("@Center", txtCenter.Text == "" ? string.Empty : txtCenter.Text),
                                new SqlParameter("@CenterPer", txtCenterPer.Text == "" ? string.Empty : txtCenterPer.Text),
                                new SqlParameter("@Tread", txtTread.Text == "" ? string.Empty : txtTread.Text),
                                new SqlParameter("@TreadPer", txtTreadPer.Text == "" ? string.Empty : txtTreadPer.Text),
                                new SqlParameter("@TypePosition", "0"),
                                new SqlParameter("@TypeStatus", "1"),
                                new SqlParameter("@UserName", Program.strUserName),
                                new SqlParameter("@BwStatus", strbwstatus),
                                new SqlParameter("@cureFamily1", rdoCureFamily1.Checked),
                                new SqlParameter("@cureFamily2", rdoCureFamily2.Checked),
                                new SqlParameter("@cureFamily3", rdoCureFamily3.Checked),
                                new SqlParameter("@mode", mode),
                                new SqlParameter("@TypeCategory", strTypeCategory),
                                new SqlParameter("@EqualType", cboType.SelectedValue.ToString())
                            };
                            int resp = dba.ExecuteNonQuery_SP("SP_SAV_TypeMaster_TypeDetails", sp1);
                            if (resp > 0)
                                MessageBox.Show("TYPE MASTER " + btnSave.Text + "ED");
                        }

                        List<string> checkedOrigins = new List<string>();
                        foreach (CheckBox chk in pnlOriginFriction.Controls)
                        {
                            if (chk.Checked == true)
                                checkedOrigins.Add(chk.Text);
                        }
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TyreType", txtType.Text) };
                        DataTable dtFrictionOrigin = (DataTable)dba.ExecuteReader_SP("SP_LST_TypeMaster_FrictionOfType", sp, DBAccess.Return_Type.DataTable);
                        List<string> lstFrictionOrigin = dtFrictionOrigin.AsEnumerable().Select(n => n.Field<string>("FrictionOrigin")).ToList();
                        if (dtFrictionOrigin.Rows.Count > 0)
                        {
                            List<string> lstToUncheck = dtFrictionOrigin.AsEnumerable().Where(n => !checkedOrigins.Contains(n.Field<string>("FrictionOrigin"))).Select(n => n.Field<string>("FrictionOrigin")).ToList();
                            List<string> lstToCheck = checkedOrigins.AsEnumerable().Where(n => !lstFrictionOrigin.Contains(n.ToString())).Select(n => n.ToString()).ToList();
                            foreach (string str in lstToUncheck)
                            {
                                SqlParameter[] sp1 = new SqlParameter[]{
                                    new SqlParameter("@FrictionOrigin",str),
                                    new SqlParameter("@TyreType",txtType.Text)
                                };
                                int resp = dba.ExecuteNonQuery_SP("SP_UPD_TypeMaster_OriginWiseType", sp1);
                            }

                            foreach (string str in lstToCheck)
                            {
                                SqlParameter[] sp1 = new SqlParameter[]{
                                    new SqlParameter("@FrictionOrigin",str),
                                    new SqlParameter("@TyreType",txtType.Text),
                                    new SqlParameter("@Username",Program.strUserName)
                                };
                                int resp = dba.ExecuteNonQuery_SP("SP_INS_TypeMaster_OriginWiseType", sp1);
                            }
                        }
                        else
                        {
                            foreach (string str in checkedOrigins)
                            {
                                SqlParameter[] sp1 = new SqlParameter[]{
                                    new SqlParameter("@FrictionOrigin",str),
                                    new SqlParameter("@TyreType",txtType.Text),
                                    new SqlParameter("@Username",Program.strUserName)
                                };
                                int resp = dba.ExecuteNonQuery_SP("SP_INS_TypeMaster_OriginWiseType", sp1);
                            }
                        }
                        MessageBox.Show("Records successfully " + btnSave.Text.ToLower() + "d");
                        btnClear_Click(sender, e);
                        frmTypeMaster_Load(sender, e);
                        if (Program.strPlantName == "SITL" || Program.strPlantName == "SLTL")
                        {
                            pnlCategory.Visible = false;
                            rdoBoth.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txtType_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtType.Text != "" && txtType.Text.Length >= 2)
                {
                    SqlParameter[] sp = new SqlParameter[] {
                        new SqlParameter("@likeType", txtType.Text.Substring(0, 2)),
                        new SqlParameter("@TyreType", txtType.Text)
                    };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_EqualType", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dt.Rows.InsertAt(dr, 0);
                        cboType.DataSource = dt;
                        cboType.DisplayMember = "TyreType";
                        cboType.ValueMember = "TyreType";
                        if (cboType.Items.Count == 2)
                            cboType.SelectedIndex = 1;
                        else
                            cboType.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnSuspend_Click(object sender, EventArgs e)
        {
            try
            {
                string strRemarks = Microsoft.VisualBasic.Interaction.InputBox("ENTER REASON FOR SUSPEND", "Title", "");
                if (strRemarks != "" && MessageBox.Show(strRemarks, "TYPE SUSPEND", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    SqlParameter[] spDel = new SqlParameter[] {
                        new SqlParameter("@Val1", txtType.Text),
                        new SqlParameter("@Val2", ""),
                        new SqlParameter("@Val3", ""),
                        new SqlParameter("@UserName", Program.strUserName),
                        new SqlParameter("@Category", "TYPE"),
                        new SqlParameter("@Remarks", ("SUSPEND: " + strRemarks))
                    };
                    int resp = dba.ExecuteNonQuery_SP("sp_upd_masterdata_suspend", spDel);
                    if (resp > 0)
                    {
                        MessageBox.Show("Type successfully " + btnSuspend.Text.ToLower() + "ed");
                        btnClear_Click(sender, e);
                        frmTypeMaster_Load(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txt_ConcateInterface_Leave(object sender, EventArgs e)
        {
            try
            {
                txtInterface.Text = "";
                if (txtBase.Text != "" && (txtCenter.Text != "" || txtTread.Text != ""))
                {
                    if (txtBase.Text != "" && txtCenter.Text != "")
                        txtInterface.Text = txtBase.Text + "+" + txtCenter.Text;
                    else if (txtBase.Text != "" && txtCenter.Text == "" && txtTread.Text != "")
                        txtInterface.Text = txtBase.Text + "+" + txtTread.Text;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmTypeMaster", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
