using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using Modbus.Device;
using Modbus.Utility;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace GT
{
    public partial class frmFrictionBlendWeigh : Form
    {
        DBAccess dbCon = new DBAccess();
        DataTable dtSource = new DataTable();
        private string strCommunication = "";
        public frmFrictionBlendWeigh()
        {
            InitializeComponent();
            this.dtpBlendDate.MinDate = DateTime.Now.AddDays(-2);
            this.dtpBlendDate.MaxDate = DateTime.Now.AddDays(0);
            dtSource.Columns.Add("SOURCE", typeof(string));

            cboCommunication.DataSource = Program.dt_Wt_Com;
            cboCommunication.ValueMember = "Com_Method";
            cboCommunication.DisplayMember = "Com_Method";
            cboCommunication.SelectedIndex = 0;

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\comm.dll";
            FileInfo file1 = new FileInfo(filePath);
            if (file1.Exists)
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    strCommunication = EncryptDecrypt.Decrypt(streamReader.ReadToEnd(), "COMMUNICATION");
                }
                cboCommunication.SelectedIndex = cboCommunication.FindStringExact(strCommunication);
            }
        }
        private void frmFrictionBlendWeigh_Load(object sender, EventArgs e)
        {
            try
            {
                cboBlendStandard.DataSource = null;
                cboBlendGrade.DataSource = null;
                grpPnlSource.Controls.Clear();
                lblPlanDate.Text = "";
                txtBlendBatch.Text = "";

                if (this.Name == "FRICTION SOURCE PLAN")
                {
                    btnSourcePlan.Visible = true;
                    pnlBlendWeigh.Visible = false;
                    lblBlendPageTitle.Text = "FRICTION BLEND SOURCE PLAN";
                    grpPnlSource.Width = 900;
                    pnlSourcePlan.Width = 1000;

                    DataTable dtStd = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_WeightClassMaster_WtClass", DBAccess.Return_Type.DataTable);
                    if (dtStd.Rows.Count > 0)
                    {
                        DataRow dr = dtStd.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtStd.Rows.InsertAt(dr, 0);

                        cboBlendStandard.DataSource = dtStd;
                        cboBlendStandard.DisplayMember = "WtClass";
                        cboBlendStandard.ValueMember = "WtClass";

                        if (cboBlendStandard.Items.Count == 2)
                            cboBlendStandard.SelectedIndex = 1;
                    }

                    DataTable dtGrade = new DataTable();
                    dtGrade.Columns.Add(new DataColumn("GRADE", typeof(System.String)));
                    dtGrade.Rows.Add("CHOOSE");
                    dtGrade.Rows.Add("BFA10");
                    dtGrade.Rows.Add("BFA16");

                    cboBlendGrade.DataSource = dtGrade;
                    cboBlendGrade.DisplayMember = "GRADE";
                    cboBlendGrade.ValueMember = "GRADE";

                    DataTable dtFri = (DataTable)dbCon.ExecuteReader_SP("sp_sel_FrictionBlendSource", DBAccess.Return_Type.DataTable);
                    if (dtFri.Rows.Count > 0)
                    {
                        int maxHeight = grpPnlSource.Height - 20;
                        int maxWidth = grpPnlSource.Width - 20;
                        int curHeight = 0;
                        int curWidth = 5;

                        for (int i = 0; i < dtFri.Rows.Count; i++)
                        {
                            CheckBox chk = new CheckBox();
                            chk.Text = dtFri.Rows[i]["FrictionOrigin"].ToString();
                            if (i > 0) curHeight += 30;
                            if (curHeight >= maxHeight)
                            {
                                curWidth += 120;
                                curHeight = 0;
                            }
                            chk.Location = new Point(curWidth, curHeight);
                            chk.CheckedChanged += new EventHandler(checkedFrictionSourceEntry);
                            grpPnlSource.Controls.Add(chk);
                        }
                    }
                }
                else if (this.Name == "FRICTION BLEND WEIGHING")
                {
                    lblOperator.Text = Program.strUserName;
                    SqlParameter[] spS = new SqlParameter[] { new SqlParameter("@PlannedBy", lblOperator.Text) };
                    DataTable dtM = (DataTable)dbCon.ExecuteReader_SP("sp_sel_FrictionBlend_SourcePlan_Class", spS, DBAccess.Return_Type.DataTable);

                    if (dtM.Rows.Count == 0)
                    {
                        MessageBox.Show("FRICTION SOURCE PLAN NOT AVAILABLE FOR THIS OPERATOR");
                        btnStart.Visible = false;
                    }
                    else
                    {
                        DataRow dr = dtM.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtM.Rows.InsertAt(dr, 0);

                        cboBlendStandard.DataSource = dtM;
                        cboBlendStandard.DisplayMember = "BlendWtClass";
                        cboBlendStandard.ValueMember = "BlendWtClass";

                        if (cboBlendStandard.Items.Count == 2)
                            cboBlendStandard.SelectedIndex = 1;

                        pnlBlendWeigh.Visible = true;
                        lblBlendPageTitle.Text = "FRICTION BLEND WEIGHING";
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void make_DTpDate()
        {
            try
            {
                DateTime endTime = Convert.ToDateTime("06:06:00");
                if (System.DateTime.Now.TimeOfDay < endTime.TimeOfDay)
                {
                    DateTime result = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                    dtpBlendDate.Value = result;
                }

                lblShift.Text = "";
                DateTime shift1Time = Convert.ToDateTime("14:06:00");
                DateTime shift2Time = Convert.ToDateTime("22:06:00");
                DateTime shift3Time = Convert.ToDateTime("06:06:00");
                if ((System.DateTime.Now.TimeOfDay > shift3Time.TimeOfDay) && (System.DateTime.Now.TimeOfDay <= shift1Time.TimeOfDay))
                    lblShift.Text = "A";
                else if ((System.DateTime.Now.TimeOfDay > shift1Time.TimeOfDay) && (System.DateTime.Now.TimeOfDay <= shift2Time.TimeOfDay))
                    lblShift.Text = "B";
                else if (System.DateTime.Now.TimeOfDay > shift2Time.TimeOfDay || System.DateTime.Now.TimeOfDay < shift3Time.TimeOfDay)
                    lblShift.Text = "C";

                if (System.DateTime.Now.TimeOfDay < endTime.TimeOfDay)
                    txtBlendBatch.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1)) +
                        DateTime.Now.Year.ToString().Substring(2, 2) + (Convert.ToChar(64 + DateTime.Now.AddDays(-1).Month)) +
                        DateTime.Now.AddDays(-1).Day.ToString("00") + lblShift.Text;
                else
                    txtBlendBatch.Text = (Program.strPlantName == "SLTL" ? "L" : Program.strPlantName == "MMN" ? "C" : Program.strPlantName.Substring(0, 1)) +
                        DateTime.Now.Year.ToString().Substring(2, 2) + Convert.ToChar(64 + DateTime.Now.Month) + DateTime.Now.Day.ToString("00") +
                        lblShift.Text;

                SqlParameter[] spS = new SqlParameter[] { new SqlParameter("@BlendBatch", txtBlendBatch.Text.Substring(0, 7)) };
                string strBatchNo = (string)dbCon.ExecuteScalar_SP("sp_sel_Dummy_Fs_BatchNo", spS);

                txtBlendBatch.Text = txtBlendBatch.Text.Substring(0, 7) + strBatchNo;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void BlendCtrl_Clear()
        {
            try
            {
                lblWt1.Text = "0.00";
                lblWt2.Text = "0.00";
                lblFsSpec.Text = "0.00";
                lblFsAct.Text = "0.00";
                lblFrictionTot.Text = "0.00";
                lblTotWt.Text = "0.00";
                lblAddWt.Text = "0";

                txtRemarks.Text = "";
                txtBlendMachineWt.Text = "0.00";

                btnWt1.Visible = false;
                btnWt2.Visible = false;
                btnFs.Visible = false;
                btnSave.Visible = false;

                tmrWt1.Stop();
                tmrWt2.Stop();
                tmrFs.Stop();

                txtBlendManualValue.Text = "0";
                txtBlendManualValue.Visible = lblBlendPageTitle.Text == "FRICTION BLEND WEIGHING" ? false : true;

                dtSource.Rows.Clear();
                lblSource1.Text = "";
                lblSource2.Text = "";
                lblPlanDate.Text = "";
                btnStart.Visible = true;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void checkedFrictionSourceEntry(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                if (chk.Checked)
                {
                    if (dtSource == null || dtSource.Rows.Count == 0)
                        dtSource.Rows.Add(chk.Text);
                    else if (dtSource.Rows.Count == 1)
                        dtSource.Rows.Add(chk.Text);
                    else
                    {
                        MessageBox.Show("ALREADY SELECTED TWO SOURCE");
                        chk.Checked = false;
                    }
                }
                else if (!chk.Checked)
                {
                    foreach (DataRow qRow in dtSource.Select("SOURCE='" + chk.Text + "'"))
                    {
                        qRow.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void frmFrictionBlendWeigh_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F8)
                {
                    lblBlendPageTitle.Text = lblBlendPageTitle.Text == "FRICTION BLEND WEIGHING" ? "FRICTION BLEND WEIGHING - MANUAL" : "FRICTION BLEND WEIGHING";
                    txtBlendManualValue.Visible = lblBlendPageTitle.Text == "FRICTION BLEND WEIGHING" ? false : true;
                    txtBlendManualValue.Text = "0";
                    txtBlendManualValue.Focus();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                lblWt1.Text = "0.00";
                lblWt2.Text = "0.00";
                lblFsSpec.Text = "0.00";
                lblFsAct.Text = "0.00";
                lblFrictionTot.Text = "0.00";
                lblTotWt.Text = "0.00";

                btnWt2.Visible = false;
                btnFs.Visible = false;

                txtRemarks.Text = "";
                txtBlendMachineWt.Text = "0.00";
                txtBlendManualValue.Text = "0";
                tmrWt1.Start();
                if (lblBlendPageTitle.Text == "FRICTION BLEND WEIGHING - MANUAL")
                {
                    txtBlendManualValue.Visible = true;
                    txtBlendManualValue.Focus();
                }

                btnStart.Visible = false;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_MachineWt(Label lblM)
        {
            lblErrMsg.Text = "";
            decimal strVal = 0;
            if (lblBlendPageTitle.Text == "FRICTION BLEND WEIGHING - MANUAL")
                strVal = txtBlendManualValue.Text != "" ? Convert.ToDecimal(txtBlendManualValue.Text) : 0;
            else
            {
                if (strCommunication != "COM4")
                {
                    TcpClient tcpClient = new TcpClient();
                    try
                    {
                        IPAddress iPAddress = System.Net.IPAddress.Parse("192.168.5." + Convert.ToInt32(strCommunication));
                        int tcpPort = 502;
                        tcpClient = new TcpClient(iPAddress.ToString(), tcpPort);
                        tcpClient.ReceiveTimeout = 1000;

                        if (!tcpClient.Connected)
                            lblErrMsg.Text = "ETHERNET NOT CONNECTED";
                        else
                        {
                            try
                            {
                                IModbusSerialMaster master1 = ModbusSerialMaster.CreateRtu(tcpClient);

                                byte slaveID = 1;
                                ushort startAddress = 1000;
                                ushort numRegisters = 2;
                                ushort[] registers1 = master1.ReadHoldingRegisters(slaveID, startAddress, numRegisters);
                                uint value1 = (ModbusUtility.GetUInt32(registers1[0], registers1[1]));
                                int readPow = unchecked((Int32)value1);

                                int intDivide = 10;
                                if (readPow > 0)
                                    intDivide = Convert.ToInt32(Math.Pow(10, readPow));
                                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(tcpClient);

                                startAddress = 1002;
                                ushort[] registers = master.ReadHoldingRegisters(slaveID, startAddress, numRegisters);
                                uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
                                int readVal = unchecked((Int32)value);

                                strVal = Convert.ToDecimal(readVal) / Convert.ToInt32(intDivide);
                                master1.Dispose();
                                master.Dispose();
                            }
                            catch (TimeoutException exp)
                            {
                                lblErrMsg.Text = exp.Message.ToUpper();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrMsg.Text = ex.Message.ToUpper();
                    }
                    tcpClient.Close();
                }
                else
                {
                    SerialPort port = new SerialPort("COM4");
                    try
                    {
                        port.BaudRate = 9600;
                        port.DataBits = 8;
                        port.Parity = Parity.None;
                        port.StopBits = StopBits.One;
                        port.ReadTimeout = 300;
                        port.WriteTimeout = 300;
                        port.Open();

                        if (!port.IsOpen)
                            lblErrMsg.Text = "PORT NOT OPENED";
                        else
                        {
                            try
                            {
                                if (Program.strPlantName != "SITL")
                                {
                                    IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                                    byte slaveId = 1;
                                    ushort startAddress = 1002;
                                    ushort numRegisters = 2;
                                    ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
                                    uint value = (ModbusUtility.GetUInt32(registers[0], registers[1]));
                                    int readVal = unchecked((Int32)value);

                                    strVal = Convert.ToDecimal(value) / Convert.ToInt32(1000);
                                    master.Dispose();
                                }
                                else
                                {
                                    string strRtxt = port.ReadLine();
                                    if (strRtxt.Length > 0)
                                    {
                                        string numberOnly = Regex.Replace(strRtxt, "[^0-9.]", "");
                                        if (numberOnly.ToString() != "")
                                        {
                                            strVal = Convert.ToDecimal(numberOnly);
                                        }
                                    }
                                }
                            }
                            catch (TimeoutException exp)
                            {
                                lblErrMsg.Text = exp.Message.ToUpper();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrMsg.Text = ex.Message.ToUpper();
                    }
                    port.Close();
                }
            }
            if (strVal.ToString() != "")
            {
                if (lblAddWt.Text == "0")
                {
                    lblM.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                    txtBlendMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                }
                else
                {
                    lblM.Text = (Convert.ToDecimal(lblAddWt.Text) + (Convert.ToDecimal(strVal))).ToString("0.00");
                    txtBlendMachineWt.Text = (Convert.ToDecimal(strVal).ToString("0.00"));
                }
            }
            else
                txtBlendMachineWt.Text = "0.00";
        }
        private void tmrWt1_Tick(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(lblWt1.Text) > 0 &&
                ((!lblSource2.Visible && Convert.ToDecimal(lblWt1.Text) <= 60) || (lblSource2.Visible && Convert.ToDecimal(lblWt1.Text) <= 30)))
            {
                btnWt1.Visible = true;
                if (lblBlendPageTitle.Text == "FRICTION BLEND WEIGHING")
                    btnWt1.Focus();
            }
            else
                btnWt1.Visible = false;

            Bind_MachineWt(lblWt1);
        }
        private void btnWt1_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(lblWt1.Text) > 0)
            {
                tmrWt1.Stop();
                lblWt2.Text = "0.00";
                lblFsSpec.Text = "0.00";
                lblFsAct.Text = "0.00";
                lblTotWt.Text = "0.00";
                lblAddWt.Text = "0";
                lblAddWt.Visible = false;
                lnkAdd.Visible = false;

                txtRemarks.Text = "";
                txtBlendMachineWt.Text = "0.00";
                txtBlendManualValue.Text = "0";

                lblFrictionTot.Text = (Convert.ToDecimal(lblWt1.Text)).ToString("0.00");
                if (lblSource2.Visible)
                    tmrWt2.Start();
                else
                {
                    lblFrictionTot.Text = (Convert.ToDecimal(lblWt1.Text)).ToString("0.00");
                    lblFsSpec.Text = (Convert.ToDecimal(lblFrictionTot.Text) * Convert.ToDecimal(0.0694)).ToString("0.00");
                    tmrFs.Start();
                }

                btnWt1.Visible = false;

                if (lblBlendPageTitle.Text != "FRICTION BLEND WEIGHING")
                    txtBlendManualValue.Focus();
            }
        }
        private void tmrWt2_Tick(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(lblWt2.Text) > 0 && Convert.ToDecimal(lblWt2.Text) <= 30)
            {
                btnWt2.Visible = true;
                if (lblBlendPageTitle.Text == "FRICTION BLEND WEIGHING")
                    btnWt2.Focus();
            }
            else
                btnWt2.Visible = false;

            Bind_MachineWt(lblWt2);
        }
        private void btnWt2_Click(object sender, EventArgs e)
        {
            lblFsAct.Text = "0.00";
            lblTotWt.Text = "0.00";

            txtRemarks.Text = "";
            txtBlendMachineWt.Text = "0.00";
            txtBlendManualValue.Text = "0";

            tmrWt2.Stop();
            lblFrictionTot.Text = (Convert.ToDecimal(lblWt1.Text) + Convert.ToDecimal(lblWt2.Text)).ToString("0.00");
            lblFsSpec.Text = (Convert.ToDecimal(lblFrictionTot.Text) * Convert.ToDecimal(0.0694)).ToString("0.00");
            tmrFs.Start();
            btnWt2.Visible = false;

            if (lblBlendPageTitle.Text != "FRICTION BLEND WEIGHING")
                txtBlendManualValue.Focus();
        }
        private void tmrFs_Tick(object sender, EventArgs e)
        {
            if ((Convert.ToDecimal(lblFsSpec.Text) - (Convert.ToDecimal(lblFsSpec.Text) * Convert.ToDecimal(2) / 100) < Convert.ToDecimal(lblFsAct.Text)) &&
                (Convert.ToDecimal(lblFsSpec.Text) + (Convert.ToDecimal(lblFsSpec.Text) * Convert.ToDecimal(2) / 100) > Convert.ToDecimal(lblFsAct.Text)) ||
                (Convert.ToDecimal(lblFsAct.Text) == Convert.ToDecimal(lblFsSpec.Text)))
            {
                btnFs.Visible = true;
                if (lblBlendPageTitle.Text == "FRICTION BLEND WEIGHING")
                    btnFs.Focus();
            }
            else
                btnFs.Visible = false;

            Bind_MachineWt(lblFsAct);
        }
        private void btnFs_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(lblFsAct.Text) > 0)
            {
                txtRemarks.Text = "";
                txtBlendMachineWt.Text = "0.00";
                txtBlendManualValue.Text = "0";

                tmrFs.Stop();
                lblTotWt.Text = (Convert.ToDecimal(lblFrictionTot.Text) + Convert.ToDecimal(lblFsAct.Text)).ToString();
                btnFs.Visible = false;
                btnSave.Visible = true;

                make_DTpDate();

                if (lblBlendPageTitle.Text != "FRICTION BLEND WEIGHING")
                    txtRemarks.Focus();
                else
                    btnSave.Focus();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                if (cboBlendStandard.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Choose Standard";
                    cboBlendStandard.Focus();
                }
                else if (cboBlendGrade.SelectedIndex <= 0)
                {
                    lblErrMsg.Text = "Choose Grade";
                    cboBlendGrade.Focus();
                }
                else if (dtSource == null || dtSource.Rows.Count == 0)
                    lblErrMsg.Text = "Choose maximum 2 or 1 source";
                else if (Convert.ToDecimal(lblTotWt.Text) <= 0)
                    lblErrMsg.Text = "Total weight is less than or equal to zero. Kindly weighing properly";
                else
                {
                    make_DTpDate();
                    SqlParameter spBlendDate;
                    SqlParameter spBlendTime;
                    try
                    {
                        spBlendDate = new SqlParameter("@BlendDate", DateTime.Parse(dtpBlendDate.Value.ToString()));
                        spBlendTime = new SqlParameter("@BlendTime", DateTime.Parse(dtpBlendDate.Value.ToString("HH:mm")));
                    }
                    catch (Exception ex)
                    {
                        spBlendDate = new SqlParameter("@BlendDate", DateTime.ParseExact(dtpBlendDate.Value.ToString(), "dd/MM/yyyy", null));
                        spBlendTime = new SqlParameter("@BlendTime", DateTime.ParseExact(dtpBlendDate.Value.ToString(), "HH:mm", null));
                        Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                    }

                    SqlParameter[] spIns = new SqlParameter[] { 
                        spBlendDate, spBlendTime, 
                        new SqlParameter("@BlendShift", Convert.ToChar(lblShift.Text) - 64), new SqlParameter("@BlendOperator", lblOperator.Text), 
                        new SqlParameter("@BlendStandard", cboBlendStandard.SelectedValue.ToString()), 
                        new SqlParameter("@BlendGrade", cboBlendGrade.SelectedValue.ToString()), 
                        new SqlParameter("@BlendSource1", dtSource.Rows[0]["SOURCE"].ToString()),
                        new SqlParameter("@BlendSource2", dtSource.Rows.Count == 2 ? dtSource.Rows[1]["SOURCE"].ToString() : ""),
                        new SqlParameter("@BlendWt1", lblWt1.Text), new SqlParameter("@BlendWt2", lblWt2.Text), 
                        new SqlParameter("@BlendFsSpec", lblFsSpec.Text), new SqlParameter("@BlendFsAct", lblFsAct.Text), 
                        new SqlParameter("@BlendBatch", txtBlendBatch.Text), new SqlParameter("@BlendRemarks", txtRemarks.Text), 
                        new SqlParameter("@BlendUser", Program.strUserName) 
                    };
                    int resp = dbCon.ExecuteNonQuery_SP("sp_ins_FrictionBlendData", spIns);
                    if (resp > 0)
                    {
                        MessageBox.Show("Record saved successfully");
                        frmFrictionBlendWeigh_Load(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtBlendManualValue_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyValue == 13 && txtBlendManualValue.Text.Length > 0)
                {
                    if (btnWt1.Visible)
                        btnWt1.Focus();
                    else if (btnWt2.Visible)
                        btnWt2.Focus();
                    else if (btnFs.Visible)
                        btnFs.Focus();
                }
                else
                {
                    if (txtBlendManualValue.Text == "")
                        txtBlendManualValue.Text = "0";
                    txtBlendManualValue.Focus();
                    txtBlendManualValue.SelectionStart = txtBlendManualValue.Text.Length;
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToUpper();
            }
        }
        private void cboCommunication_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboCommunication.SelectedIndex > 0 && cboCommunication.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    strCommunication = cboCommunication.SelectedValue.ToString();
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\comm.dll";
                    FileInfo file1 = new FileInfo(filePath);
                    if (file1.Exists)
                        file1.Delete();

                    if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\"))
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\");

                    StreamWriter SWrtiter = System.IO.File.AppendText(filePath);
                    SWrtiter.WriteLine(EncryptDecrypt.Encrypt(strCommunication, "COMMUNICATION"));
                    SWrtiter.Close();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSourcePlan_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                if (cboBlendStandard.SelectedIndex <= 0)
                {
                    MessageBox.Show("Choose Standard");
                    cboBlendStandard.Focus();
                }
                else if (cboBlendGrade.SelectedIndex <= 0)
                {
                    MessageBox.Show("Choose Grade");
                    cboBlendGrade.Focus();
                }
                else if (dtSource == null || dtSource.Rows.Count == 0)
                    MessageBox.Show("Choose 1 or 2 source");
                else
                {
                    SqlParameter[] spI = new SqlParameter[] { 
                        new SqlParameter("@BlendWtClass", cboBlendStandard.SelectedValue.ToString()), 
                        new SqlParameter("@BlendBaseGarde", cboBlendGrade.SelectedValue.ToString()), 
                        new SqlParameter("@Source1", dtSource.Rows[0]["SOURCE"].ToString()), 
                        new SqlParameter("@Source2", dtSource.Rows.Count == 2 ? dtSource.Rows[1]["SOURCE"].ToString() : ""), 
                        new SqlParameter("@PlannedBy", Program.strUserName) 
                    };
                    int resp = dbCon.ExecuteNonQuery_SP("sp_ins_FrictionBlend_SourcePlan", spI);
                    if (resp > 0)
                    {
                        MessageBox.Show("Record saved successfully");
                        frmFrictionBlendWeigh_Load(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboBlendGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Name != "FRICTION SOURCE PLAN")
                {
                    BlendCtrl_Clear();
                    if (cboBlendGrade.SelectedIndex > 0)
                    {
                        SqlParameter[] spS = new SqlParameter[] { 
                            new SqlParameter("@PlannedBy", Program.strUserName), 
                            new SqlParameter("@BlendWtClass", cboBlendStandard.SelectedValue.ToString()), 
                            new SqlParameter("@BlendBaseGarde", cboBlendGrade.SelectedValue.ToString()) 
                        };
                        DataTable dtData = (DataTable)dbCon.ExecuteReader_SP("sp_sel_FrictionBlend_SourcePlan_Data", spS, DBAccess.Return_Type.DataTable);

                        if (dtData.Rows.Count > 0)
                        {
                            lblSource1.Text = dtData.Rows[0]["Source1"].ToString();
                            dtSource.Rows.Add(lblSource1.Text);

                            if (dtData.Rows[0]["Source2"].ToString() != "")
                            {
                                lblSource2.Text = dtData.Rows[0]["Source2"].ToString();
                                dtSource.Rows.Add(lblSource2.Text);

                                lblSource2.Visible = true;
                                lblWt2.Visible = true;
                            }
                            else
                            {
                                lblAddWt.Visible = true;
                                lnkAdd.Visible = true;

                                lblSource2.Visible = false;
                                lblWt2.Visible = false;
                            }
                            lblPlanDate.Text = "PLANNED ON : " + dtData.Rows[0]["PlannedOn"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cboBlendStandard_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Name != "FRICTION SOURCE PLAN")
                {
                    cboBlendGrade.DataSource = null;
                    BlendCtrl_Clear();

                    if (cboBlendStandard.SelectedIndex > 0)
                    {
                        dtSource.Rows.Clear();
                        lblSource1.Text = "";
                        lblSource2.Text = "";
                        lblPlanDate.Text = "";

                        SqlParameter[] spS = new SqlParameter[] { 
                            new SqlParameter("@PlannedBy", Program.strUserName), 
                            new SqlParameter("@BlendWtClass", cboBlendStandard.SelectedValue.ToString())
                        };
                        DataTable dtData = (DataTable)dbCon.ExecuteReader_SP("sp_sel_FrictionBlend_SourcePlan_Grade", spS, DBAccess.Return_Type.DataTable);

                        if (dtData.Rows.Count > 0)
                        {
                            DataRow dr = dtData.NewRow();
                            dr.ItemArray = new object[] { "CHOOSE" };
                            dtData.Rows.InsertAt(dr, 0);

                            cboBlendGrade.DataSource = dtData;
                            cboBlendGrade.DisplayMember = "BlendBaseGarde";
                            cboBlendGrade.ValueMember = "BlendBaseGarde";

                            if (cboBlendGrade.Items.Count == 2)
                                cboBlendGrade.SelectedIndex = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void lnkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (Convert.ToDecimal(lblWt1.Text) > 0 && Convert.ToDecimal(lblWt1.Text) <= 60)
                {
                    lblErrMsg.Text = "";
                    lblAddWt.Text = lblWt1.Text;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmFrictionBlendWeigh", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
