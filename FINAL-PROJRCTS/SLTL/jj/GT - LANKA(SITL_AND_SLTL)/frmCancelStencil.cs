using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Configuration;

namespace GT
{
    public partial class frmCancelStencil : Form
    {
        DBAccess dba = new DBAccess();
        public frmCancelStencil()
        {
            InitializeComponent();
        }
        private void frmCancelStencil_Load(object sender, EventArgs e)
        {
            try
            {
                Ctrl_Clear();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCancelStencil", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Stencil_bind()
        {
            try
            {
                string strStage = "";
                listBox1.DataSource = null;
                if (rdoStage1.Checked == true)
                    strStage = rdoStage1.Text;
                else if (rdoStage2.Checked == true)
                    strStage = rdoStage2.Text;
                else if (rdoStage3.Checked == true)
                    strStage = rdoStage3.Text;
                else if (rdoStage4.Checked == true)
                    strStage = rdoStage4.Text;

                if (strStage != "")
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@satge", strStage) };
                    DataTable dtWT = (DataTable)dba.ExecuteReader_SP("sp_sel_stencil_for_cancel", sp, DBAccess.Return_Type.DataTable);
                    if (dtWT.Rows.Count > 0)
                    {
                        listBox1.DataSource = dtWT;
                        listBox1.ValueMember = "StencilNo";
                        listBox1.DisplayMember = "StencilNo";
                        listBox1.ClearSelected();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCancelStencil", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void txtStencilno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoStage1.Checked == true)
                    Bind_Stage1_Details();
                else if (rdoStage2.Checked == true)
                {
                    Bind_Stage1_Details();
                    Bind_Stage2_Details();
                }
                else if (rdoStage3.Checked == true || rdoStage4.Checked == true)
                {
                    Bind_Stage1_Details();
                    Bind_Stage2_Details();
                    Bind_Stage3_Details();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCancelStencil", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void listBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.Items.Count > 0)
                {
                    txtStencilno.Text = listBox1.SelectedValue.ToString();
                    listBox1.SetSelected(listBox1.SelectedIndex, true);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCancelStencil", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);

            }
        }
        private void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrorMessage.Text = "";
                if (txtStencilno.Text == "") lblErrorMessage.Text = "Choose Stencil NO";
                else if (txtRemarks.Text.Trim() == "") lblErrorMessage.Text = "Enter Remarks";
                else
                {
                    string strStage = "";
                    if (rdoStage1.Checked == true)
                        strStage = rdoStage1.Text;
                    else if (rdoStage2.Checked == true)
                        strStage = rdoStage2.Text;
                    else if (rdoStage3.Checked == true)
                        strStage = rdoStage3.Text;
                    else if (rdoStage4.Checked == true)
                        strStage = rdoStage4.Text;

                    if (strStage != "")
                    {
                        SqlParameter[] sp = new SqlParameter[] { 
                            new SqlParameter("@StencilNo", txtStencilno.Text), 
                            new SqlParameter("@RejectRemarks", txtRemarks.Text), 
                            new SqlParameter("@RejectedBy", Program.strUserName), 
                            new SqlParameter("@Stage", strStage) 
                        };
                        int resp = dba.ExecuteNonQuery_SP("sp_upd_stencil_cancel_v1", sp);
                        if (resp > 0)
                        {
                            MessageBox.Show("Stencil Cancelled Successfully");
                            Ctrl_Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCancelStencil", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Ctrl_Clear()
        {
            Stencil_bind();
            listBox1.Enabled = true;
            lblhbwspec.Text = "";
            lblbasespec.Text = "";
            lblinterspec.Text = "";
            lblcenterspec.Text = "";
            lbltreadspec.Text = "";
            lblhbwweighed.Text = "";
            lblbaseweighed.Text = "";
            lblinterfaceweighed.Text = "";
            lblcenterweighed.Text = "";
            lbltreadweighed.Text = "";
            lblstencilno.Text = "";
            lbltype.Text = "";
            lbltyresize.Text = "";
            lblrimWidth.Text = "";
            lblDate.Text = "";
            lblShift.Text = "";
            lbloperatorname.Text = "";
            lbllipcode.Text = "";
            lblbasecode.Text = "";
            lblInterfaceCode.Text = "";
            lblCenterCode.Text = "";
            lblTreadCode.Text = "";
            txtStencilno.Text = "";
            lblSpecTotal.Text = "";
            lbllipswspec.Text = "";
            lblbaseswspec.Text = "";
            lblinterfaceswspec.Text = "";
            lblCenterswspec.Text = "";
            lbltreadswspec.Text = "";
            lblActualTotal.Text = "";
            lblErrorMessage.Text = "";
            txtRemarks.Text = "";

            lblDateShift.Text = "";
            lblOperator.Text = "";
            lblMouldCode.Text = "";
            lblFormerCode.Text = "";
            lblBwSize.Text = "";
            lblNoofBw.Text = "";
            lblGtTemp.Text = "";
            lblbuildPressurefrom.Text = "";
            lblbuildPressureTo.Text = "";
            lblGtOD.Text = "";
            lblGtWidth.Text = "";
            lblStage2ActWt.Text = "";
            label10.Text = "";
            lblBwSize.Text = "";

            lblPlatform.Text = "";
            lblSize.Text = "";
            lblRim.Text = "";
            lblTyreType.Text = "";
            lblBrand.Text = "";
            lblSidewall.Text = "";

            lblProcessID.Text = "";
            lblPress.Text = "";
            lblMouldTemp.Text = "";
            lblPumbMachine.Text = "";
            lblLoadMachine.Text = "";
            lblUnloadMachine.Text = "";

            lblLoad.Text = "";
            lblUnload.Text = "";
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Ctrl_Clear();
        }
        private void rdoStage2_CheckedChanged(object sender, EventArgs e)
        {
            Ctrl_Clear();
        }
        private void Bind_Stage1_Details()
        {
            try
            {
                SqlParameter[] spPro = new SqlParameter[] { new SqlParameter("@StencilNo", txtStencilno.Text) };
                DataTable dtdetails = (DataTable)dba.ExecuteReader_SP("sp_sel_productiondetails_stencilwise", spPro, DBAccess.Return_Type.DataTable);
                if (dtdetails.Rows.Count > 0)
                {
                    lblstencilno.Text = txtStencilno.Text;
                    lbltype.Text = dtdetails.Rows[0]["TyreType"].ToString();
                    lbltyresize.Text = dtdetails.Rows[0]["TyreSize"].ToString();
                    lblrimWidth.Text = dtdetails.Rows[0]["RimSize"].ToString();
                    lblDate.Text = dtdetails.Rows[0]["ProductionDate"].ToString();
                    lblShift.Text = dtdetails.Rows[0]["Shift"].ToString();
                    lbloperatorname.Text = dtdetails.Rows[0]["Operator"].ToString();

                    lbllipcode.Text = dtdetails.Rows[0]["LipCode"].ToString();
                    lblbasecode.Text = dtdetails.Rows[0]["BaseCode"].ToString();
                    lblInterfaceCode.Text = dtdetails.Rows[0]["InterfaceCode"].ToString();
                    lblCenterCode.Text = dtdetails.Rows[0]["CenterCode"].ToString();
                    lblTreadCode.Text = dtdetails.Rows[0]["TreadCode"].ToString();
                    lblhbwspec.Text = dtdetails.Rows[0]["LipRqWt"].ToString();
                    lblbasespec.Text = dtdetails.Rows[0]["BaseRqWt"].ToString();
                    lblinterspec.Text = dtdetails.Rows[0]["InterfaceRqWt"].ToString();
                    lblcenterspec.Text = dtdetails.Rows[0]["CenterRqWt"].ToString();
                    lbltreadspec.Text = dtdetails.Rows[0]["TreadRqWt"].ToString();
                    lblhbwweighed.Text = dtdetails.Rows[0]["LipScale"].ToString();
                    lblbaseweighed.Text = dtdetails.Rows[0]["BaseScale"].ToString();
                    lblinterfaceweighed.Text = dtdetails.Rows[0]["InterfaceScale"].ToString();
                    lblcenterweighed.Text = dtdetails.Rows[0]["CenterScale"].ToString();
                    lbltreadweighed.Text = dtdetails.Rows[0]["TreadScale"].ToString();

                    lblActualTotal.Text = Convert.ToDecimal(Convert.ToDecimal(lblhbwweighed.Text) + Convert.ToDecimal(lblbaseweighed.Text) + Convert.ToDecimal(lblinterfaceweighed.Text) + Convert.ToDecimal(lblcenterweighed.Text) + Convert.ToDecimal(lbltreadweighed.Text)).ToString();
                    lblSpecTotal.Text = Convert.ToDecimal(Convert.ToDecimal(lblhbwspec.Text) + Convert.ToDecimal(lblbasespec.Text) + Convert.ToDecimal(lblinterspec.Text) + Convert.ToDecimal(lblcenterspec.Text) + Convert.ToDecimal(lbltreadspec.Text)).ToString();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCancelStencil", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Stage2_Details()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StencilNo", txtStencilno.Text) };
                DataTable dtdetails2 = (DataTable)dba.ExecuteReader_SP("sp_sel_ProductionSWspecDetails_stencilWise", sp, DBAccess.Return_Type.DataTable);
                if (dtdetails2.Rows.Count > 0)
                {
                    lblDateShift.Text = dtdetails2.Rows[0]["ProductionDate"].ToString() + "&&" + dtdetails2.Rows[0]["Shift"].ToString();
                    lblOperator.Text = dtdetails2.Rows[0]["UserName"].ToString();
                    lblMouldCode.Text = dtdetails2.Rows[0]["MouldCode"].ToString();
                    lblFormerCode.Text = dtdetails2.Rows[0]["FormerCode"].ToString();
                    lblBwSize.Text = dtdetails2.Rows[0]["BWSize"].ToString();
                    lblNoofBw.Text = dtdetails2.Rows[0]["NoofBw"].ToString();
                    lblGtTemp.Text = dtdetails2.Rows[0]["GdTemp"].ToString();
                    lblbuildPressurefrom.Text = dtdetails2.Rows[0]["BulidPressureFrom"].ToString();
                    lblbuildPressureTo.Text = dtdetails2.Rows[0]["BulidPressureTo"].ToString();
                    lblGtOD.Text = dtdetails2.Rows[0]["GtOdOld"].ToString() + " / " + dtdetails2.Rows[0]["GtOdNew"].ToString();
                    lblGtWidth.Text = dtdetails2.Rows[0]["GtWidthOld"].ToString() + " / " + dtdetails2.Rows[0]["GtWitdthNew"].ToString();
                    lblStage2ActWt.Text = "BUILD WT: " + dtdetails2.Rows[0]["ActualGtWeight"].ToString();

                    if (Convert.ToDecimal(lblrimWidth.Text) == 0)
                    {
                        label10.Text = "BAND + CG";
                        lbllipcode.Text = dtdetails2.Rows[0]["BandWt"].ToString() + "+" + dtdetails2.Rows[0]["CushionGumWt"].ToString();
                        lblhbwspec.Text = (Convert.ToDecimal(dtdetails2.Rows[0]["BandWt"].ToString()) + Convert.ToDecimal(dtdetails2.Rows[0]["CushionGumWt"].ToString())).ToString();
                        lblhbwweighed.Text = dtdetails2.Rows[0]["ActBandCushionWt"].ToString();
                        lblBwSize.Text = dtdetails2.Rows[0]["BandSize"].ToString();
                    }

                    lbllipswspec.Text = dtdetails2.Rows[0]["LipSwSpec"].ToString();
                    lblbaseswspec.Text = dtdetails2.Rows[0]["BaseSwSpec"].ToString();
                    lblinterfaceswspec.Text = dtdetails2.Rows[0]["InterfaceSwSpec"].ToString();
                    lblCenterswspec.Text = dtdetails2.Rows[0]["CenterSwSpec"].ToString();
                    lbltreadswspec.Text = dtdetails2.Rows[0]["TreadSwSpec"].ToString();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCancelStencil", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Stage3_Details()
        {
            try
            {
                SqlParameter[] sp3 = new SqlParameter[] { new SqlParameter("@StencilNo", txtStencilno.Text) };
                DataTable dtdetails3 = (DataTable)dba.ExecuteReader_SP("sp_sel_productiondetails_stage3_stencilwise", sp3, DBAccess.Return_Type.DataTable);
                if (dtdetails3.Rows.Count > 0)
                {
                    lblPlatform.Text = dtdetails3.Rows[0]["Config"].ToString();
                    lblSize.Text = dtdetails3.Rows[0]["TyreSize"].ToString();
                    lblRim.Text = dtdetails3.Rows[0]["RimSize"].ToString();
                    lblTyreType.Text = dtdetails3.Rows[0]["TyreType"].ToString();
                    lblBrand.Text = dtdetails3.Rows[0]["Brand"].ToString();
                    lblSidewall.Text = dtdetails3.Rows[0]["Sidewall"].ToString();

                    lblProcessID.Text = dtdetails3.Rows[0]["Process_Id"].ToString();
                    lblPress.Text = dtdetails3.Rows[0]["Press"].ToString();
                    lblMouldTemp.Text = dtdetails3.Rows[0]["MouldTemp"].ToString() + " / " + dtdetails3.Rows[0]["MouldTempBottom"].ToString();
                    lblPumbMachine.Text = dtdetails3.Rows[0]["Pumping_Machine"].ToString();
                    lblLoadMachine.Text = dtdetails3.Rows[0]["Loading_Machine"].ToString();
                    lblLoad.Text = "Loaded On : " + dtdetails3.Rows[0]["Loaded_Date"].ToString() + " By " +
                        dtdetails3.Rows[0]["Loaded_By"].ToString() + "\n" + dtdetails3.Rows[0]["Loaded_Remarks"].ToString();

                    if (dtdetails3.Rows[0]["Unloading_Machine"].ToString() != "")
                    {
                        lblUnloadMachine.Text = dtdetails3.Rows[0]["Unloading_Machine"].ToString();
                        lblUnload.Text = "Unloaded On : " + dtdetails3.Rows[0]["Unloaded_Date"].ToString() + " By " +
                            dtdetails3.Rows[0]["Unloaded_By"].ToString() + "\n" + dtdetails3.Rows[0]["Unloaded_Remarks"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmCancelStencil", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
