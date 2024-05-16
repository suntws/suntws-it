using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace GT
{
    public partial class frmCureMasterAddress : Form
    {
        DBAccess dba = new DBAccess();
        public frmCureMasterAddress()
        {
            InitializeComponent();
        }
        private void frmCureMasterAddress_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.Location = new Point(0, 0);
                DataTable dtUnit = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringUnit", DBAccess.Return_Type.DataTable);
                if (dtUnit.Rows.Count > 0)
                {
                    DataRow toInsert = dtUnit.NewRow();
                    toInsert.ItemArray = new object[] { "CHOOSE" };
                    dtUnit.Rows.InsertAt(toInsert, 0);

                    cmbUnit.DataSource = dtUnit;
                    cmbUnit.DisplayMember = "CuringUnit";
                    cmbUnit.ValueMember = "CuringUnit";

                    if (cmbUnit.Items.Count == 2)
                        cmbUnit.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCureMasterAddress", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void CtrlClears()
        {
            txtIpAddress.Text = "";
            txtIpPort.Text = "";
            txtCureStartHours.Text = "0";
            txtCureStartMinutes.Text = "0";
            txtCureStartSeconds.Text = "0";
            txtCureExpHours.Text = "0";
            txtCureExpMiuntes.Text = "0";
            txtCureExpSeconds.Text = "0";
            txtCureCompHours.Text = "0";
            txtCureCompMinutes.Text = "0";
            txtCureCompSeconds.Text = "0";
            //CureExtHoures
            txtCureExtMinutes.Text = "0";
            txtCureExtSeconds.Text = "0";
            txtCureSetHours.Text = "0";
            txtCureSetMinutes.Text = "0";
            txtTempTop.Text = "0";
            txtTempBot.Text = "0";

            txtHydPressure.Text = "0";
            txtBumpingCount.Text = "0";
            txtTempTopMsg.Text = "0";
            txtTempBottomMsg.Text = "0";
            txtPressureMsg.Text = "0";
            txtSteamMsg.Text = "0";

            txtCureNotStarted.Text = "0";
            txtCureEnd.Text = "0";
            txtCureOk.Text = "0";
            txtUnloadDelay.Text = "0";
        }
        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CtrlClears();
                cmbStation.DataSource = null;
                cmbSlot.DataSource = null;
                if (cmbUnit.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@CuringUnit", cmbUnit.SelectedValue.ToString()) };
                    DataTable dtStation = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringStation", sp, DBAccess.Return_Type.DataTable);
                    if (dtStation.Rows.Count > 0)
                    {
                        DataRow toInsert = dtStation.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dtStation.Rows.InsertAt(toInsert, 0);

                        cmbStation.DataSource = dtStation;
                        cmbStation.DisplayMember = "CuringStation";
                        cmbStation.ValueMember = "CuringStation";

                        if (cmbStation.Items.Count == 2)
                            cmbStation.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCureMasterAddress", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CtrlClears();
                cmbSlot.DataSource = null;
                if (cmbStation.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@CuringUnit", cmbUnit.SelectedValue.ToString()), 
                        new SqlParameter("@CuringStation", cmbStation.SelectedValue.ToString()) 
                    };
                    DataTable dtSlot = (DataTable)dba.ExecuteReader_SP("sp_sel_CuringAvalSlot", sp, DBAccess.Return_Type.DataTable);
                    if (dtSlot.Rows.Count > 0)
                    {
                        DataRow toInsert = dtSlot.NewRow();
                        toInsert.ItemArray = new object[] { "CHOOSE" };
                        dtSlot.Rows.InsertAt(toInsert, 0);

                        int slotCount = Convert.ToInt32(dtSlot.Rows[1]["AvalSlot"].ToString());
                        for (int k = 1; k < slotCount; k++)
                        {
                            toInsert = dtSlot.NewRow();
                            toInsert.ItemArray = new object[] { k };
                            dtSlot.Rows.InsertAt(toInsert, k);
                        }

                        cmbSlot.DataSource = dtSlot;
                        cmbSlot.DisplayMember = "AvalSlot";
                        cmbSlot.ValueMember = "AvalSlot";

                        if (cmbSlot.Items.Count == 2)
                            cmbSlot.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCureMasterAddress", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CtrlClears();
                if (cmbSlot.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@LoadingUnit", cmbUnit.SelectedValue.ToString()), 
                        new SqlParameter("@StationNo", cmbStation.SelectedValue.ToString()), 
                        new SqlParameter("@SlotNo", cmbSlot.SelectedValue.ToString()) 
                    };
                    DataTable dtSlotAddress = (DataTable)dba.ExecuteReader_SP("sp_sel_PressMaster_Slot_Address", sp, DBAccess.Return_Type.DataTable);
                    if (dtSlotAddress.Rows.Count == 1)
                    {
                        txtIpAddress.Text = dtSlotAddress.Rows[0]["IpAddress"].ToString();
                        txtIpPort.Text = dtSlotAddress.Rows[0]["PortNo"].ToString();

                        txtCureStartHours.Text = dtSlotAddress.Rows[0]["CureStartHours"].ToString();
                        txtCureStartMinutes.Text = dtSlotAddress.Rows[0]["CureStartMinutes"].ToString();
                        txtCureStartSeconds.Text = dtSlotAddress.Rows[0]["CureStartSeconds"].ToString();
                        txtCureExpHours.Text = dtSlotAddress.Rows[0]["CureExpHours"].ToString();
                        txtCureExpMiuntes.Text = dtSlotAddress.Rows[0]["CureExpMiuntes"].ToString();
                        txtCureExpSeconds.Text = dtSlotAddress.Rows[0]["CureExpSeconds"].ToString();
                        txtCureCompHours.Text = dtSlotAddress.Rows[0]["CureCompHours"].ToString();
                        txtCureCompMinutes.Text = dtSlotAddress.Rows[0]["CureCompMinutes"].ToString();
                        txtCureCompSeconds.Text = dtSlotAddress.Rows[0]["CureCompSeconds"].ToString();
                        //CureExtHoures
                        txtCureExtMinutes.Text = dtSlotAddress.Rows[0]["CureExtMinutes"].ToString();
                        txtCureExtSeconds.Text = dtSlotAddress.Rows[0]["CureExtSeconds"].ToString();
                        txtCureSetHours.Text = dtSlotAddress.Rows[0]["CureSetHours"].ToString();
                        txtCureSetMinutes.Text = dtSlotAddress.Rows[0]["CureSetMinutes"].ToString();
                        txtTempTop.Text = dtSlotAddress.Rows[0]["TempTop"].ToString();
                        txtTempBot.Text = dtSlotAddress.Rows[0]["TempBot"].ToString();

                        txtHydPressure.Text = dtSlotAddress.Rows[0]["HydPressure"].ToString();
                        txtBumpingCount.Text = dtSlotAddress.Rows[0]["BumpingCount"].ToString();
                        txtTempTopMsg.Text = dtSlotAddress.Rows[0]["TempTopMsg"].ToString();
                        txtTempBottomMsg.Text = dtSlotAddress.Rows[0]["TempBotMsg"].ToString();
                        txtPressureMsg.Text = dtSlotAddress.Rows[0]["PressureMsg"].ToString();
                        txtSteamMsg.Text = dtSlotAddress.Rows[0]["SteamMsg"].ToString();

                        txtCureNotStarted.Text = dtSlotAddress.Rows[0]["CureNotStart"].ToString();
                        txtCureEnd.Text = dtSlotAddress.Rows[0]["CureEnd"].ToString();
                        txtCureOk.Text = dtSlotAddress.Rows[0]["CureOk"].ToString();
                        txtUnloadDelay.Text = dtSlotAddress.Rows[0]["UnloadDelay"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCureMasterAddress", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbUnit.SelectedIndex <= 0)
                {
                    MessageBox.Show("Choose Loading Unit");
                    cmbUnit.Focus();
                }
                else if (cmbStation.SelectedIndex <= 0)
                {
                    MessageBox.Show("Choose station");
                    cmbStation.Focus();
                }
                else if (cmbSlot.SelectedIndex <= 0)
                {
                    MessageBox.Show("Choose slot no");
                    cmbSlot.Focus();
                }
                else if (txtIpAddress.Text == "")
                {
                    MessageBox.Show("Enter Ip Address");
                    txtIpAddress.Focus();
                }
                else if (!ValidateIPv4(txtIpAddress.Text))
                {
                    MessageBox.Show("This is not a valide ip address");
                    txtIpAddress.Focus();
                }
                else if (txtIpPort.Text == "")
                {
                    MessageBox.Show("Enter Ip Port");
                    txtIpPort.Focus();
                }
                else if (Convert.ToInt32(txtIpPort.Text) < 1 && Convert.ToInt32(txtIpPort.Text) > 65535)
                {
                    MessageBox.Show("This is not a valide port");
                    txtIpPort.Focus();
                }
                else
                {
                    SqlParameter[] spIns = new SqlParameter[] { 
                        new SqlParameter("@LoadingUnit", cmbUnit.SelectedValue.ToString()), 
                        new SqlParameter("@StationNo", cmbStation.SelectedValue.ToString()), 
                        new SqlParameter("@SlotNo", cmbSlot.SelectedValue.ToString()), 
                        new SqlParameter("@IpAddress", txtIpAddress.Text), 
                        new SqlParameter("@PortNo", txtIpPort.Text), 
                        new SqlParameter("@StartAddress", "0"), 
                        new SqlParameter("@CureStartHours", txtCureStartHours.Text != "" ? txtCureStartHours.Text : "0"), 
                        new SqlParameter("@CureStartMinutes", txtCureStartMinutes.Text != "" ? txtCureStartMinutes.Text : "0"), 
                        new SqlParameter("@CureStartSeconds", txtCureStartSeconds.Text != "" ? txtCureStartSeconds.Text : "0"), 
                        new SqlParameter("@CureExpHours", txtCureExpHours.Text != "" ? txtCureExpHours.Text : "0"), 
                        new SqlParameter("@CureExpMiuntes", txtCureExpMiuntes.Text != "" ? txtCureExpMiuntes.Text : "0"), 
                        new SqlParameter("@CureExpSeconds", txtCureExpSeconds.Text != "" ? txtCureExpSeconds.Text : "0"), 
                        new SqlParameter("@CureCompHours", txtCureCompHours.Text != "" ? txtCureCompHours.Text : "0"), 
                        new SqlParameter("@CureCompMinutes", txtCureCompMinutes.Text != "" ? txtCureCompMinutes.Text : "0"), 
                        new SqlParameter("@CureCompSeconds", txtCureCompSeconds.Text != "" ? txtCureCompSeconds.Text : "0"), 
                        new SqlParameter("@CureExtHoures", "0"), 
                        new SqlParameter("@CureExtMinutes", txtCureExtMinutes.Text != "" ? txtCureExtMinutes.Text : "0"), 
                        new SqlParameter("@CureExtSeconds", txtCureExtSeconds.Text != "" ? txtCureExtSeconds.Text : "0"), 
                        new SqlParameter("@TempTop", txtTempTop.Text != "" ? txtTempTop.Text : "0"), 
                        new SqlParameter("@TempBot", txtTempBot.Text != "" ? txtTempBot.Text : "0"), 
                        new SqlParameter("@HydPressure", txtHydPressure.Text != "" ? txtHydPressure.Text : "0"), 
                        new SqlParameter("@BumpingCount", txtBumpingCount.Text != "" ? txtBumpingCount.Text : "0"), 
                        new SqlParameter("@TempRangeMin", "0"), 
                        new SqlParameter("@TempRangeMax", "0"), 
                        new SqlParameter("@CureSetHours", txtCureSetHours.Text != "" ? txtCureSetHours.Text : "0"), 
                        new SqlParameter("@CureSetMinutes", txtCureSetMinutes.Text != "" ? txtCureSetMinutes.Text : "0"), 
                        new SqlParameter("@Username", Program.strUserName), 
                        new SqlParameter("@TempTopMsg", txtTempTopMsg.Text != "" ? txtTempTopMsg.Text : "0"), 
                        new SqlParameter("@TempBotMsg", txtTempBottomMsg.Text != "" ? txtTempBottomMsg.Text : "0"), 
                        new SqlParameter("@PressureMsg", txtPressureMsg.Text != "" ? txtPressureMsg.Text : "0"), 
                        new SqlParameter("@SteamMsg", txtSteamMsg.Text != "" ? txtSteamMsg.Text : "0"), 
                        new SqlParameter("@CureNotStart", txtCureNotStarted.Text != "" ? txtCureNotStarted.Text : "0"), 
                        new SqlParameter("@CureEnd", txtCureEnd.Text != "" ? txtCureEnd.Text : "0"), 
                        new SqlParameter("@CureOk", txtCureOk.Text != "" ? txtCureOk.Text : "0"), 
                        new SqlParameter("@UnloadDelay", txtUnloadDelay.Text != "" ? txtUnloadDelay.Text : "0") 
                    };
                    int resp = dba.ExecuteNonQuery_SP("sp_ins_PressMaster_Address", spIns);
                    if (resp > 0)
                    {
                        MessageBox.Show("Address list saved successfully");
                        CtrlClears();
                        frmCureMasterAddress_Load(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCureMasterAddress", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private bool ValidateIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
        private void KeyPress_Event(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                SendKeys.Send("{TAB}");
        }
    }
}
