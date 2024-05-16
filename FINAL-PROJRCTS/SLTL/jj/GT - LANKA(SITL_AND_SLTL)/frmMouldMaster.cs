using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using cusRegex = System.Text.RegularExpressions;
using System.Configuration;
using System.Reflection;

namespace GT
{
    public partial class frmMouldMaster : Form
    {
        DBAccess dbCon = new DBAccess();
        public frmMouldMaster()
        {
            InitializeComponent();
        }
        private void frmMouldMaster_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.Location = new Point(0, 0);
                DataGridViewCellStyle style = this.gvMould.ColumnHeadersDefaultCellStyle;
                style.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                this.KeyPreview = true;
                this.txtMouldCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Enter_Keypress);
                this.txtFormerCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtNoofBw.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtBuildFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtbuildPressureTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtLipswspec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtBaseswspec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtInterfaceswspec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtCenterswspec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtTreadSwSpec.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtgtod.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtgtwidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtNoOfBumping.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtBumpingTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumericPercentage_KeyPress);
                this.txtBandWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCount_KeyPress);
                this.txtCushionWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCount_KeyPress);

                gvMould.CellClick += new DataGridViewCellEventHandler(GridViewRow_SelectionChanged);

                DataTable dtSize = (DataTable)dbCon.ExecuteReader_SP("SP_GET_MouldMaster_TyreSize_List", DBAccess.Return_Type.DataTable);
                if (dtSize.Rows.Count > 0)
                {
                    DataRow dr = dtSize.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtSize.Rows.InsertAt(dr, 0);
                    cmbTyreSize.DataSource = dtSize;
                    cmbTyreSize.DisplayMember = "TyreSize";
                    cmbTyreSize.ValueMember = "TyreSize";
                    if (cmbTyreSize.Items.Count == 2)
                        cmbTyreSize.SelectedIndex = 1;
                    else
                        cmbTyreSize.SelectedIndex = 0;
                }

                lblErrMsg.Text = "";
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmMouldMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Gridview()
        {
            try
            {
                if (cmbTyreSize.SelectedIndex > 0 && cmbRim.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@Tyresize", cmbTyreSize.SelectedValue.ToString()), 
                        new SqlParameter("@Rimsize", cmbRim.SelectedValue.ToString())
                    };
                    DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_GET_MouldMaster_Detail", sp1, DBAccess.Return_Type.DataTable);
                    DataTable dt1 = new DataTable();
                    foreach (DataColumn column in dt.Columns)
                    {
                        DataColumn col = new DataColumn(column.ColumnName, typeof(System.String));
                        dt1.Columns.Add(col);
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt1.NewRow();
                        dt1.Rows.Add(dr);

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j == 18 || j == 19 || j == 20)
                            {
                                if (dt.Rows[i][18].ToString() != "") dt1.Rows[i][18] = Get24Hrs(Convert.ToDateTime(dt.Rows[i][18].ToString()));
                                if (dt.Rows[i][19].ToString() != "") dt1.Rows[i][19] = Get24Hrs(Convert.ToDateTime(dt.Rows[i][19].ToString()));
                                if (dt.Rows[i][20].ToString() != "") dt1.Rows[i][20] = Get24Hrs(Convert.ToDateTime(dt.Rows[i][20].ToString()));
                            }
                            else
                            {
                                dt1.Rows[i][j] = dt.Rows[i][j].ToString();
                            }
                        }

                    }
                    dt = null;
                    gvMould.DataSource = dt1;

                    gvMould.Columns[0].HeaderText = "TYRE SIZE";
                    gvMould.Columns[1].Width = 150;
                    gvMould.Columns[1].HeaderText = "RIM";
                    gvMould.Columns[1].Width = 40;
                    gvMould.Columns[2].HeaderText = "MOULD UNIQUE CODE";
                    gvMould.Columns[2].Width = 40;
                    gvMould.Columns[3].HeaderText = "FORMER CODE";
                    gvMould.Columns[3].Width = 45;
                    gvMould.Columns[4].HeaderText = "BW SIZE";
                    gvMould.Columns[4].Width = 40;
                    gvMould.Columns[5].HeaderText = "NO OF BW";
                    gvMould.Columns[5].Width = 30;
                    gvMould.Columns[6].HeaderText = "BUILDING PRESSURE FROM";
                    gvMould.Columns[6].Width = 55;
                    gvMould.Columns[7].HeaderText = "BUILDING PRESSURE TO";
                    gvMould.Columns[7].Width = 55;
                    gvMould.Columns[8].HeaderText = "LIP SHEET WIDTH";
                    gvMould.Columns[8].Width = 40;
                    gvMould.Columns[9].HeaderText = "BASE SHEET WIDTH";
                    gvMould.Columns[9].Width = 40;
                    gvMould.Columns[10].HeaderText = "INTERFACE SHEET WIDTH";
                    gvMould.Columns[10].Width = 50;
                    gvMould.Columns[11].HeaderText = "CENTER SHEET WIDTH";
                    gvMould.Columns[11].Width = 50;
                    gvMould.Columns[12].HeaderText = "TREAD SHEET WIDTH";
                    gvMould.Columns[12].Width = 50;
                    gvMould.Columns[13].HeaderText = "GT OD";
                    gvMould.Columns[13].Width = 30;
                    gvMould.Columns[14].HeaderText = "GT WIDTH";
                    gvMould.Columns[14].Width = 40;
                    gvMould.Columns[15].HeaderText = "NO OF BUMPING";
                    gvMould.Columns[15].Width = 50;
                    gvMould.Columns[16].HeaderText = "BUMPING TON";
                    gvMould.Columns[16].Width = 50;
                    gvMould.Columns[17].HeaderText = "PLATFORM";
                    gvMould.Columns[17].Width = 60;
                    gvMould.Columns[18].HeaderText = "CURE SET 1";
                    gvMould.Columns[18].Width = 40;
                    gvMould.Columns[19].HeaderText = "CURE SET 2";
                    gvMould.Columns[19].Width = 40;
                    gvMould.Columns[20].HeaderText = "CURE SET 3";
                    gvMould.Columns[20].Width = 40;
                    gvMould.Columns[21].HeaderText = "BAND WT";
                    gvMould.Columns[21].Width = 40;
                    gvMould.Columns[22].HeaderText = "CUSHION GUM WT";
                    gvMould.Columns[22].Width = 60;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmMouldMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtNumericPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }
        private void txt_Enter_Keypress(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                SendKeys.Send("{TAB}");
        }
        private void txtCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            Program.digitonly(e);
        }
        private void GridViewRow_SelectionChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtMouldCode.Text = "";
                txtMouldCode.Enabled = true;
                lblErrMsg.Text = "";
                if (e.RowIndex == 0 || e.RowIndex > 0)
                {
                    cmbTyreSize.SelectedIndex = cmbTyreSize.FindStringExact((gvMould.Rows[e.RowIndex].Cells[0].Value).ToString());
                    cmbRim.SelectedIndex = cmbRim.FindStringExact((gvMould.Rows[e.RowIndex].Cells[1].Value).ToString());
                    txtMouldCode.Text = (gvMould.Rows[e.RowIndex].Cells[2].Value).ToString();

                    txtFormerCode.Text = (gvMould.Rows[e.RowIndex].Cells[3].Value).ToString();
                    txtBWsize.Text = (gvMould.Rows[e.RowIndex].Cells[4].Value).ToString();
                    txtNoofBw.Text = (gvMould.Rows[e.RowIndex].Cells[5].Value).ToString();
                    txtBuildFrom.Text = (gvMould.Rows[e.RowIndex].Cells[6].Value).ToString();
                    txtbuildPressureTo.Text = (gvMould.Rows[e.RowIndex].Cells[7].Value).ToString();
                    txtLipswspec.Text = (gvMould.Rows[e.RowIndex].Cells[8].Value).ToString();
                    txtBaseswspec.Text = (gvMould.Rows[e.RowIndex].Cells[9].Value).ToString();
                    txtInterfaceswspec.Text = (gvMould.Rows[e.RowIndex].Cells[10].Value).ToString();
                    txtCenterswspec.Text = (gvMould.Rows[e.RowIndex].Cells[11].Value).ToString();
                    txtTreadSwSpec.Text = (gvMould.Rows[e.RowIndex].Cells[12].Value).ToString();
                    txtgtod.Text = (gvMould.Rows[e.RowIndex].Cells[13].Value).ToString();
                    txtgtwidth.Text = (gvMould.Rows[e.RowIndex].Cells[14].Value).ToString();
                    txtNoOfBumping.Text = (gvMould.Rows[e.RowIndex].Cells[15].Value).ToString();
                    txtBumpingTon.Text = (gvMould.Rows[e.RowIndex].Cells[16].Value).ToString();
                    if (gvMould.Rows[e.RowIndex].Cells[17].Value.ToString() != "")
                        cboPlatform.Text = (gvMould.Rows[e.RowIndex].Cells[17].Value).ToString();
                    else
                        cboPlatform.SelectedIndex = 0;
                    txtCure1.Text = ""; txtCure2.Text = ""; txtCure3.Text = "";
                    if (gvMould.Rows[e.RowIndex].Cells[18].Value.ToString() != "") { txtCure1.Text = Get24Hrs(Convert.ToDateTime(gvMould.Rows[e.RowIndex].Cells[18].Value)); }
                    if (gvMould.Rows[e.RowIndex].Cells[19].Value.ToString() != "") { txtCure2.Text = Get24Hrs(Convert.ToDateTime(gvMould.Rows[e.RowIndex].Cells[19].Value)); }
                    if (gvMould.Rows[e.RowIndex].Cells[20].Value.ToString() != "") { txtCure3.Text = Get24Hrs(Convert.ToDateTime(gvMould.Rows[e.RowIndex].Cells[20].Value)); }
                    txtBandWt.Text = (gvMould.Rows[e.RowIndex].Cells[21].Value).ToString();
                    txtCushionWt.Text = (gvMould.Rows[e.RowIndex].Cells[22].Value).ToString();

                    btnSave.Text = "UPDATE";
                    btnSuspend.Visible = true;
                    txtMouldCode.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmMouldMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private string Get24Hrs(DateTime curingTime)
        {
            //CuringTime = Convert.ToDateTime();
            string temp;
            temp = curingTime.ToString("HH:mm");
            return curingTime.ToString("HH:mm");
        }
        public bool validCureTime(string cureTime)
        {
            cusRegex.Regex regex = new cusRegex.Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
            cusRegex.Match match = regex.Match(cureTime);
            if (!match.Success) { return false; } else return true;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtMouldCode.Text = "";
                lblErrMsg.Text = "";
                progressBar1.Value = 0;
                btnSave.Text = "SAVE";
                btnSuspend.Visible = false;
                txtFormerCode.Text = "0";
                txtBWsize.Text = "0";
                txtNoofBw.Text = "0";
                txtBuildFrom.Text = "0";
                txtbuildPressureTo.Text = "0";
                txtMouldCode.Enabled = true;
                txtLipswspec.Text = "0";
                txtBaseswspec.Text = "0";
                txtInterfaceswspec.Text = "0";
                txtCenterswspec.Text = "0";
                txtTreadSwSpec.Text = "0";
                txtgtod.Text = "0";
                txtgtwidth.Text = "0";
                txtBumpingTon.Text = "0";
                txtNoOfBumping.Text = "0";
                txtCure1.Text = "";
                txtCure2.Text = "";
                txtCure3.Text = "";
                txtBandWt.Text = "";
                txtCushionWt.Text = "";
                txtBandWt.Enabled = false;
                txtCushionWt.Enabled = false;
                if (cboPlatform.Items.Count == 2)
                    cboPlatform.SelectedIndex = 1;
                else
                    cboPlatform.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmMouldMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                string strQuery = string.Empty;
                if (cmbTyreSize.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Choose tyre size";
                    cmbTyreSize.Focus();
                }
                else if (cmbRim.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Choose rim width";
                    cmbRim.Focus();
                }
                else if (txtMouldCode.Text == "")
                {
                    lblErrMsg.Text = "Enter Mould Unique Code";
                    txtMouldCode.Focus();
                }
                else if (txtFormerCode.Text == "")
                {
                    lblErrMsg.Text = "Enter Former Code";
                    txtFormerCode.Focus();
                }
                else if (txtBWsize.Text == "")
                {
                    lblErrMsg.Text = "Enter BW Size";
                    txtBWsize.Focus();
                }
                else if (txtNoofBw.Text == "")
                {
                    lblErrMsg.Text = "Enter No of BW";
                    txtNoofBw.Focus();
                }
                else if (txtBuildFrom.Text == "")
                {
                    lblErrMsg.Text = "Enter Buliding Pressure From";
                    txtBuildFrom.Focus();
                }
                else if (txtbuildPressureTo.Text == "")
                {
                    lblErrMsg.Text = "Enter Buliding Pressure To";
                    txtbuildPressureTo.Focus();
                }
                else if (txtLipswspec.Text == "")
                {
                    lblErrMsg.Text = "Enter Lip SW Spec";
                    txtLipswspec.Focus();
                }
                else if (txtBaseswspec.Text == "")
                {
                    lblErrMsg.Text = "Enter Base SW Spec";
                    txtBaseswspec.Focus();
                }
                else if (txtInterfaceswspec.Text == "")
                {
                    lblErrMsg.Text = "Enter Interface SW Spec";
                    txtInterfaceswspec.Focus();
                }
                else if (txtCenterswspec.Text == "")
                {
                    lblErrMsg.Text = "Enter Center SW Spec";
                    txtCenterswspec.Focus();
                }
                else if (txtTreadSwSpec.Text == "")
                {
                    lblErrMsg.Text = "Enter Tread SW Spec";
                    txtTreadSwSpec.Focus();
                }
                else if (txtgtod.Text == "")
                {
                    lblErrMsg.Text = "Enter GT OD";
                    txtgtod.Focus();
                }
                else if (txtgtwidth.Text == "")
                {
                    lblErrMsg.Text = "Enter GT Width";
                    txtgtwidth.Focus();
                }
                else if (txtNoOfBumping.Text == "")
                {
                    lblErrMsg.Text = "Enter No of Bumping";
                    txtNoOfBumping.Focus();
                }
                else if (txtBumpingTon.Text == "")
                {
                    lblErrMsg.Text = "Enter Bumping ton";
                    txtBumpingTon.Focus();
                }
                else if (cboPlatform.SelectedIndex == 0)
                {
                    lblErrMsg.Text = "Select platform";
                    cboPlatform.Focus();
                }
                else if (txtCure1.Text.Trim().Length == 1 && txtCure2.Text.Trim().Length == 1 && txtCure3.Text.Trim().Length == 1)
                {
                    lblErrMsg.Text = "Please enter cure set value";
                    txtCure1.Focus();
                }
                else
                {
                    bool isError = false;
                    int mode = 0;
                    string cureset1 = DBNull.Value.ToString(), cureset2 = DBNull.Value.ToString(), cureset3 = DBNull.Value.ToString();

                    if (txtCure1.Text.Trim().Length > 1 || txtCure2.Text.Trim().Length > 1 || txtCure3.Text.Trim().Length > 1)
                    {
                        if (txtCure1.Text.Trim().Length > 1)
                        {
                            if (validCureTime(txtCure1.Text) == false)
                            {
                                lblErrMsg.Text = "Enter valid Cure Set 1";
                                txtCure1.Focus();
                                isError = true;
                            }
                        }
                        if (txtCure2.Text.Trim().Length > 1)
                        {
                            if (validCureTime(txtCure2.Text) == false)
                            {
                                lblErrMsg.Text = "Enter valid Cure Set 2";
                                txtCure2.Focus();
                                isError = true;
                            }
                        }
                        if (txtCure3.Text.Trim().Length > 1)
                        {
                            if (validCureTime(txtCure3.Text) == false)
                            {
                                lblErrMsg.Text = "Enter valid Cure Set 3";
                                txtCure3.Focus();
                                isError = true;
                            }
                        }
                    }

                    if (isError == false)
                    {
                        if (txtCure1.Text.Trim().Length == 1) cureset1 = DBNull.Value.ToString();
                        else cureset1 = Convert.ToDateTime(txtCure1.Text).ToString("HH:mm");
                        if (txtCure2.Text.Trim().Length == 1) cureset2 = DBNull.Value.ToString();
                        else cureset2 = Convert.ToDateTime(txtCure2.Text).ToString("HH:mm");
                        if (txtCure3.Text.Trim().Length == 1) cureset3 = DBNull.Value.ToString();
                        else cureset3 = Convert.ToDateTime(txtCure3.Text).ToString("HH:mm");
                    }
                    if (btnSave.Text == "SAVE" && isError == false)
                    {
                        if (lblErrMsg.Text.Length == 0)
                        {
                            DataTable dtChk = new DataTable();
                            if (Program.strPlantName == "PDK")
                            {
                                SqlParameter[] sp3 = new SqlParameter[] { new SqlParameter("@MouldCode", txtMouldCode.Text) };
                                dtChk = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_MouldMaster_SizeOfMould", sp3, DBAccess.Return_Type.DataTable);
                            }
                            else
                            {
                                SqlParameter[] sp = new SqlParameter[] { 
                                    new SqlParameter("@TyreSize", cmbTyreSize.SelectedValue.ToString()), 
                                    new SqlParameter("@RimSize", cmbRim.SelectedValue.ToString()),
                                    new SqlParameter("@MouldCode", txtMouldCode.Text.Trim())
                                };
                                dtChk = (DataTable)dbCon.ExecuteReader_SP("SP_GET_MouldMaster_DetailOfSize", sp, DBAccess.Return_Type.DataTable);
                            }

                            if (dtChk.Rows.Count > 0)
                                lblErrMsg.Text = "Mould Code Already Existing for " + dtChk.Rows[0]["TyreSize"].ToString() + " ~ " + dtChk.Rows[0]["RimSize"].ToString();
                            else
                                mode = 1;
                        }
                    }
                    else if (btnSave.Text == "UPDATE" && isError == false)
                        mode = 2;

                    if (mode > 0)
                    {
                        SqlParameter[] sp4 = new SqlParameter[] { 
                            new SqlParameter("@TyreSize",cmbTyreSize.SelectedValue.ToString()),
                            new SqlParameter("@RimSize",cmbRim.SelectedValue.ToString()),
                            new SqlParameter("@MouldCode",txtMouldCode.Text),
                            new SqlParameter("@SizeStatus","1"),
                            new SqlParameter("@UserName",Program.strUserName ),
                            new SqlParameter("@FormerCode",txtFormerCode.Text),
                            new SqlParameter("@BWSize",Convert.ToDecimal(cmbRim.SelectedValue.ToString())==0 || 
                                (cmbTyreSize.SelectedValue.ToString() == "100/80-8MOULDON" && cmbRim.SelectedValue.ToString() == "2.50")? "0":txtBWsize.Text),
                            new SqlParameter("@NoofBw",txtNoofBw.Text),
                            new SqlParameter("@BuildPressureFrom",txtBuildFrom.Text),
                            new SqlParameter("@BuildPressureTo",txtbuildPressureTo.Text),
                            new SqlParameter("@LipSwSpec",txtLipswspec.Text),
                            new SqlParameter("@BaseSwSpec",txtBaseswspec.Text ),
                            new SqlParameter("@InterfaceSwSpec",txtInterfaceswspec.Text),
                            new SqlParameter("@CenterSwSpec",txtCenterswspec.Text),
                            new SqlParameter("@TreadSwSpec",txtTreadSwSpec.Text),
                            new SqlParameter("@GTOD",txtgtod.Text),
                            new SqlParameter("@GTWidth",txtgtwidth.Text),
                            new SqlParameter("@NoofBumping",txtNoOfBumping.Text),
                            new SqlParameter("@BumpingTon",txtBumpingTon.Text),
                            new SqlParameter("@Platform",cboPlatform.SelectedValue.ToString()),
                            new SqlParameter("@CureSet1",cureset1),
                            new SqlParameter("@CureSet2",cureset2), 
                            new SqlParameter("@CureSet3",cureset3),
                            new SqlParameter("@Mode",mode),
                            new SqlParameter("@BandSize",Convert.ToDecimal(cmbRim.SelectedValue.ToString())==0 || 
                                (cmbTyreSize.SelectedValue.ToString() == "100/80-8MOULDON" && cmbRim.SelectedValue.ToString() == "2.50")?txtBWsize.Text :""),
                            new SqlParameter("@BandWt",txtBandWt.Text),
                            new SqlParameter("@CushionGumWt",txtCushionWt.Text)
                        };

                        int resp = dbCon.ExecuteNonQuery_SP("SP_SAV_MouldMaster_MouldDetails", sp4);
                        if (resp > 0)
                        {
                            MessageBox.Show("Details successfully " + btnSave.Text.ToLower() + "d");
                            btnClear_Click(sender, e);
                            cmbTyreSize_SelectedIndexChanged(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmMouldMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbTyreSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbRim.DataSource = null;
                gvMould.DataSource = null;
                cboPlatform.DataSource = null;
                txtMouldCode.Text = "";
                txtMouldCode.Enabled = true;
                if (cmbTyreSize.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TyreSize", cmbTyreSize.SelectedValue.ToString()) };
                    DataTable dtRim = (DataTable)dbCon.ExecuteReader_SP("SP_GET_MouldMaster_Rim_List", sp, DBAccess.Return_Type.DataTable);
                    if (dtRim.Rows.Count > 0)
                    {
                        DataRow dr = dtRim.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtRim.Rows.InsertAt(dr, 0);
                        cmbRim.DataSource = dtRim;
                        cmbRim.DisplayMember = "TyreRim";
                        cmbRim.ValueMember = "TyreRim";
                        if (cmbRim.Items.Count == 2)
                            cmbRim.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmMouldMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbRim_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvMould.DataSource = null;
                txtMouldCode.Text = "";
                txtMouldCode.Enabled = true;

                txtBandWt.Enabled = false;
                txtCushionWt.Enabled = false;
                if (cmbRim.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@TyreSize", cmbTyreSize.SelectedValue.ToString()), 
                        new SqlParameter("@Rimsize", cmbRim.SelectedValue.ToString()) 
                    };
                    DataTable dtConfig = (DataTable)dbCon.ExecuteReader_SP("SP_LST_MouldMaster_Platform", sp, DBAccess.Return_Type.DataTable);
                    if (dtConfig.Rows.Count > 0)
                    {
                        DataRow dr = dtConfig.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtConfig.Rows.InsertAt(dr, 0);
                        cboPlatform.DataSource = dtConfig;
                        cboPlatform.DisplayMember = "Config";
                        cboPlatform.ValueMember = "Config";
                        if (cboPlatform.Items.Count == 2)
                            cboPlatform.SelectedIndex = 1;
                    }
                    if (Convert.ToDecimal(cmbRim.SelectedValue.ToString()) == 0)
                    {
                        txtBandWt.Enabled = true;
                        txtCushionWt.Enabled = true;
                    }
                    Bind_Gridview();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmMouldMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSuspend_Click(object sender, EventArgs e)
        {
            try
            {
                string strRemarks = Microsoft.VisualBasic.Interaction.InputBox("ENTER REASON FOR SUSPEND", "Title", "");
                if (strRemarks != "" && MessageBox.Show(strRemarks, "MOULD SUSPEND", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    SqlParameter[] spDel = new SqlParameter[] { 
                        new SqlParameter("@Val1", txtMouldCode.Text), 
                        new SqlParameter("@Val2", cmbTyreSize.SelectedValue.ToString()), 
                        new SqlParameter("@Val3", cmbRim.SelectedValue.ToString()), 
                        new SqlParameter("@UserName", Program.strUserName),
                        new SqlParameter("@Category", "MOULD SIZE"),
                        new SqlParameter("@Remarks", ("SUSPEND: " + strRemarks))
                    };
                    int resp = dbCon.ExecuteNonQuery_SP("sp_upd_masterdata_suspend", spDel);
                    if (resp > 0)
                    {
                        MessageBox.Show("Mould successfully " + btnSuspend.Text.ToLower() + "ed");
                        btnClear_Click(sender, e);
                        frmMouldMaster_Load(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmMouldMaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
