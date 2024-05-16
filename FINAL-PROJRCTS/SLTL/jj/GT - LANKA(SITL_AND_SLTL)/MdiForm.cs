using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GT
{
    public partial class MdiForm : Form
    {
        DBAccess dba = new DBAccess();
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int conn, int val);
        public MdiForm()
        {
            InitializeComponent();
        }
        private void MdiForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = Program.strPlantName + " GT-SOFT {V" + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion + "} (USER: " + Program.strUserName + ")";

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@username", Program.strUserName) };
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_UserDetails_menu_privilege", sp, DBAccess.Return_Type.DataTable);
                if (dt.Rows.Count == 1)
                {
                    //MASTER
                    if (Program.strUserName.ToUpper() == "ADMIN" || dt.Rows[0]["mnuWtVariantMaster"].ToString() == "True" || dt.Rows[0]["mnuFrictionMaster"].ToString() == "True"
                        || dt.Rows[0]["mnuTypeMaster"].ToString() == "True" || dt.Rows[0]["mnuMouldMaster"].ToString() == "True" || dt.Rows[0]["mnuBlendMaster"].ToString() == "True"
                        || dt.Rows[0]["mnuWtTolerance"].ToString() == "True" || dt.Rows[0]["mnuD4Defect"].ToString() == "True" || dt.Rows[0]["mnuWeightStandard"].ToString() == "True"
                        || dt.Rows[0]["mnuSizePressStandard"].ToString() == "True" || dt.Rows[0]["mnuConcessionChart"].ToString() == "True" || dt.Rows[0]["mnuConcessionPlan"].ToString() == "True")
                    {
                        mASTERToolStripMenuItem.Visible = true;
                        uSERACCOUNTToolStripMenuItem.Visible = Program.strUserName.ToUpper() == "ADMIN" ? true : false;
                        wEIGHTVARIANTToolStripMenuItem.Visible = dt.Rows[0]["mnuWtVariantMaster"].ToString() == "False" ? false : true;
                        fRICTIONSOURCEToolStripMenuItem.Visible = dt.Rows[0]["mnuFrictionMaster"].ToString() == "False" ? false : true;
                        tYPEToolStripMenuItem.Visible = dt.Rows[0]["mnuTypeMaster"].ToString() == "False" ? false : true;
                        mOULDToolStripMenuItem.Visible = dt.Rows[0]["mnuMouldMaster"].ToString() == "False" ? false : true;
                        bLENDToolStripMenuItem.Available = dt.Rows[0]["mnuBlendMaster"].ToString() == "False" ? false : true;
                        wEIGHTTOLERANCEToolStripMenuItem.Visible = dt.Rows[0]["mnuWtTolerance"].ToString() == "False" ? false : true;
                        dEFECTD4RANGEToolStripMenuItem.Visible = dt.Rows[0]["mnuD4Defect"].ToString() == "False" ? false : true;
                        sTANDARDWEIGHTSToolStripMenuItem.Visible = dt.Rows[0]["mnuWeightStandard"].ToString() == "False" ? false : true;
                        tYRESIZEPRESSSTANDARDToolStripMenuItem.Visible = dt.Rows[0]["mnuSizePressStandard"].ToString() == "False" ? false : true;
                        sUSPENDACTIVATEToolStripMenuItem.Visible = Program.strUserName.ToUpper() == "ADMIN" ? true : false;
                        pRESSMOULDSLOTMASTERToolStripMenuItem.Visible = Program.strUserName.ToUpper() == "ADMIN" ? true : false;
                        cOMPCONCESSIONCHARTToolStripMenuItem.Visible = dt.Rows[0]["mnuConcessionChart"].ToString() == "False" ? false : true;
                        cOMPCONCESSIONPLANToolStripMenuItem.Visible = dt.Rows[0]["mnuConcessionPlan"].ToString() == "False" ? false : true;
                    }
                    else
                        mASTERToolStripMenuItem.Visible = false;

                    //PROD ALLOCATE
                    pRODALLOCATEToolStripMenuItem.Visible = dt.Rows[0]["mnuProdAllocate"].ToString() == "False" ? false : true;
                    rUNNINGaLLOCATEToolStripMenuItem.Visible = dt.Rows[0]["mnuRunningAllocate"].ToString() == "False" ? false : true;
                    rUNNINGrEVOKEToolStripMenuItem.Visible = dt.Rows[0]["mnuProdRevoke"].ToString() == "False" ? false : true;
                    //WEIGH
                    wEIGHINGToolStripMenuItem.Visible = dt.Rows[0]["mnuWeightTrack"].ToString() == "False" && dt.Rows[0]["mnuManualWeighEntry"].ToString() == "False" ? false : true;
                    //BUILD
                    bUILDToolStripMenuItem.Visible = dt.Rows[0]["mnuGtBuild"].ToString() == "False" && dt.Rows[0]["mnuManualWeighEntry"].ToString() == "False" ? false : true;
                    //CURE
                    cURINGToolStripMenuItem.Visible = dt.Rows[0]["mnuTyreCure"].ToString() == "False" && dt.Rows[0]["mnuDelayApproval"].ToString() == "False" ? false : true;

                    Program.boolLoadUnloadDelayApproval = Convert.ToBoolean(dt.Rows[0]["mnuDelayApproval"].ToString());
                    Program.boolManualWeighing = Convert.ToBoolean(dt.Rows[0]["mnuManualWeighEntry"].ToString());

                    //INSPECT
                    if (dt.Rows[0]["mnuInspection"].ToString() == "True" || dt.Rows[0]["mnuInspectionReview"].ToString() == "True")
                    {
                        iNSPECTToolStripMenuItem.Visible = true;
                        iNSPECTIONToolStripMenuItem.Visible = dt.Rows[0]["mnuInspection"].ToString() == "False" ? false : true;
                        iNSPECTIONREVIEWToolStripMenuItem.Visible = dt.Rows[0]["mnuInspectionReview"].ToString() == "False" ? false : true;
                        dATAMODIFYToolStripMenuItem.Visible = dt.Rows[0]["mnuInspectionReview"].ToString() == "False" ? false : true;
                        pRODMODIFYToolStripMenuItem.Visible = dt.Rows[0]["mnuInspectionReview"].ToString() == "False" ? false : true;
                        nONDATASTENCILBARCODEToolStripMenuItem.Visible = dt.Rows[0]["mnuInspection"].ToString() == "False" ? false : true;
                        tTSINTEGRATIONToolStripMenuItem.Visible = true;
                        Program.boolInspectDelayApproval = Convert.ToBoolean(dt.Rows[0]["mnuInspectionReview"].ToString());
                    }
                    else
                        iNSPECTToolStripMenuItem.Visible = false;

                    if (dt.Rows[0]["mnuStencilUpdate"].ToString() == "True" || dt.Rows[0]["mnuStencilCancel"].ToString() == "True")
                    {
                        sTENCILNOToolStripMenuItem.Visible = true;
                        uPDATEToolStripMenuItem.Visible = dt.Rows[0]["mnuStencilUpdate"].ToString() == "False" ? false : true;
                        cANCELToolStripMenuItem.Visible = dt.Rows[0]["mnuStencilCancel"].ToString() == "False" ? false : true;
                    }
                    else
                        sTENCILNOToolStripMenuItem.Visible = false;

                    if (dt.Rows[0]["mnuGeneralRpt"].ToString() == "True" || dt.Rows[0]["mnuCompoundConsumrpt"].ToString() == "True" ||
                        dt.Rows[0]["mnuStencilRpt"].ToString() == "True" || dt.Rows[0]["mnuGtBuildRpt"].ToString() == "True" ||
                        dt.Rows[0]["mnuStencilCancelRpt"].ToString() == "True" || dt.Rows[0]["mnuBarcodedRpt"].ToString() == "True" ||
                        dt.Rows[0]["mnuDefectedRpt"].ToString() == "True" || dt.Rows[0]["mnuLoadUnloadRpt"].ToString() == "True")
                    {
                        rEPORTToolStripMenuItem.Visible = true;
                    }
                    else
                        rEPORTToolStripMenuItem.Visible = false;

                    //dashboard
                    dASHBOARDToolStripMenuItem.Visible = dt.Rows[0]["mnudashboard"].ToString() == "True" ? true : false;

                    rEPRINTLABLEToolStripMenuItem.Visible = dt.Rows[0]["mnuInspectionReview"].ToString() == "True" ? true : false;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "MDIParent", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void DisposeAllButThis(Form form)
        {
            try
            {
                foreach (Form frm in this.MdiChildren)
                {
                    if (frm != form)
                        frm.Close();
                }
                form.Location = new Point(0, 0);
                form.MdiParent = this;
                form.Visible = true;
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmMdiParent", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cLOSEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void uSERACCOUNTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmUserCreate();
            DisposeAllButThis(childForm);
        }
        private void wEIGHTCLASSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmWeightVariant();
            DisposeAllButThis(childForm);
        }
        private void fRICTIONSOURCEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmFrictionMaster();
            DisposeAllButThis(childForm);
        }
        private void tYPEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmTypeMaster();
            DisposeAllButThis(childForm);
        }
        private void mOULDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmMouldMaster();
            DisposeAllButThis(childForm);
        }
        private void bLENDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmBlendMaster();
            DisposeAllButThis(childForm);
        }
        private void wEIGHTVARIANTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmWeightTolerance();
            DisposeAllButThis(childForm);
        }
        private void dEFECTD4RANGEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmDefectD4Master();
            DisposeAllButThis(childForm);
        }
        private void uPDATEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmUpdateStencilNo();
            DisposeAllButThis(childForm);
        }
        private void cANCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmCancelStencil();
            DisposeAllButThis(childForm);
        }
        private void sTANDARDWEIGHTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmWeightMaster();
            DisposeAllButThis(childForm);
        }
        private void tYRESIZEWISEPRESSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmPressMaster();
            DisposeAllButThis(childForm);
        }
        private void oRDERIMPORTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Out;
            if (InternetGetConnectedState(out Out, 0) == false)
                MessageBox.Show("Internet Not Connected !");
            else
            {
                Form childForm = new frmOrderImport();
                DisposeAllButThis(childForm);
            }
        }
        private void pRESSASSIGNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmOrderPressAssign();
            DisposeAllButThis(childForm);
        }
        private void bUILDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmGtBuild();
            DisposeAllButThis(childForm);
        }
        private void iNSPECTIONToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new frmInspect();
            DisposeAllButThis(childForm);
        }
        private void iNSPECTIONREVIEWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmInspectReview();
            DisposeAllButThis(childForm);
        }
        private void dATAMODIFYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Out;
            if (InternetGetConnectedState(out Out, 0) == false)
                MessageBox.Show("Internet Not Connected !");
            else
            {
                Form childForm = new frmTtsIntegration();
                childForm.Text = "MODIFY TYRE DATA AND RE-BARCODE IT";
                DisposeAllButThis(childForm);

                childForm = new frmStencilDataModify();
                DisposeAllButThis(childForm);
            }
        }
        private void lOADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmLoad();
            DisposeAllButThis(childForm);
        }
        private void uNLOADToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmUnload();
            DisposeAllButThis(childForm);
        }
        private void dASHBOARDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmDashboard();
            DisposeAllButThis(childForm);
        }
        private void sUSPENDACTIVATEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmSuspendActive();
            DisposeAllButThis(childForm);
        }
        private void tTSINTEGRATIONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Out;
            if (InternetGetConnectedState(out Out, 0) == false)
                MessageBox.Show("Internet Not Connected !");
            else
            {
                Form childForm = new frmTtsIntegration();
                DisposeAllButThis(childForm);
            }
        }
        private void cHANGEPASSWORDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmChangePass();
            DisposeAllButThis(childForm);
        }
        private void cMSADDRESSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmCureMasterAddress();
            DisposeAllButThis(childForm);
        }
        private void cUSTOMERLABLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Out;
            if (InternetGetConnectedState(out Out, 0) == false)
                MessageBox.Show("Internet Not Connected !");
            else
            {
                Form childForm = new frmCustomerBarcode();
                DisposeAllButThis(childForm);
            }
        }
        private void rEPRINTLABLEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmRePrint();
            DisposeAllButThis(childForm);
        }
        private void rEPORTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string oldExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Green_Tyre_WT.exe");
            //if (File.Exists(oldExePath))
            //    File.Delete(oldExePath);

            IntPtr admin_token = new IntPtr(0);
            //WindowsIdentity wid_admin = null;
           // WindowsImpersonationContext wic = null;

            SqlConnection connection = new SqlConnection("Data Source=" + Program.strLocalDbPath + ";Database=GTDB" + Program.strPlantName +
                ";User ID=sa;Password=" + Program.strLocalDbPass + ";Trusted_Connection=False;");
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select serversysname,serversyspass from serversys";
            cmd.CommandTimeout = 300;
            connection.Open();
            da.Fill(ds, "T1");

            if (ds.Tables[0].Rows.Count == 1)
            {
                //bool loginBool = LogonUser(ds.Tables[0].Rows[0]["serversysname"].ToString(), "192.168.5.253", ds.Tables[0].Rows[0]["serversyspass"].ToString(), 9, 0, ref admin_token);
                //if (loginBool)
                //{
                //    //wid_admin = new WindowsIdentity(admin_token);
                //    //wic = wid_admin.Impersonate();

                //    //string rptExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Green_Tyre_WT.exe");
                //    //if (!File.Exists(rptExePath))
                //    //    File.Copy(@"\\192.168.5.253\GtApp\Green_Tyre_WT.exe", AppDomain.CurrentDomain.BaseDirectory + "Green_Tyre_WT.exe", true);
                //    //else if (File.Exists(rptExePath))
                //    //{
                //    //    if (FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + "Green_Tyre_WT.exe").FileVersion !=
                //    //        FileVersionInfo.GetVersionInfo(@"\\192.168.5.253\GtApp\Green_Tyre_WT.exe").FileVersion)
                //    //    {
                //    //        if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "OLDVERSION"))
                //    //            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "OLDVERSION");

                //    //        string archivePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "OLDVERSION", "Green_Tyre_WT_" +
                //    //            FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.BaseDirectory + "Green_Tyre_WT.exe").FileVersion + ".exe");
                //    //        if (File.Exists(archivePath))
                //    //            File.Delete(archivePath);
                //    //        File.Move(AppDomain.CurrentDomain.BaseDirectory + "Green_Tyre_WT.exe", archivePath);

                //    //        File.Copy(@"\\192.168.5.253\GtApp\Green_Tyre_WT.exe", AppDomain.CurrentDomain.BaseDirectory + "Green_Tyre_WT.exe", true);
                //    //    }
                //    //}
                //}
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "Green_Tyre_WT.exe";
                startInfo.Arguments = Program.strUserName;
                Process.Start(startInfo);
            }
        }
        private void MdiForm_Leave(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void pRODMODIFYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Out;
            if (InternetGetConnectedState(out Out, 0) == false)
                MessageBox.Show("Internet Not Connected !");
            else
            {
                Form childForm = new frmTtsIntegration();
                childForm.Text = "BARCODED RE-PRINT";
                DisposeAllButThis(childForm);

                childForm = new frmStencilProdModify();
                DisposeAllButThis(childForm);
            }
        }
        private void aLLOCATIONREPORTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmProdAllocateRpt();
            DisposeAllButThis(childForm);
        }
        private void nONDATASTENCILBARCODEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmNonDataStencilBarcode();
            DisposeAllButThis(childForm);
        }
        private void wOWISEPRODUCEDQTYREPORTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmWoProdQty();
            DisposeAllButThis(childForm);
        }
        private void allocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmProdPlan();
            DisposeAllButThis(childForm);
        }
        private void pRESSMOULDSLOTMASTERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmPressSlotMaster();
            DisposeAllButThis(childForm);
        }
        private void allocaionRevokeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmPlanRevoke();
            DisposeAllButThis(childForm);
        }
        private void sEQUENCEaLLOCATEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmSequencePlan();
            DisposeAllButThis(childForm);
        }
        private void sEQUENCEpRIORITYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmSequencePriority();
            DisposeAllButThis(childForm);
        }
        private void rUNNINGrEVOKEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmPlanRevoke();
            DisposeAllButThis(childForm);
        }
        private void rUNNINGaLLOCATEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmProdPlan();
            DisposeAllButThis(childForm);
        }
        private void wEIGHTSTANDARDVIEWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmWeightMaster();
            childForm.Name = "WEIGHT STANDARAD VIEW";
            DisposeAllButThis(childForm);
        }
        private void tYRECOMPOUNDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmWeighPlan();
            DisposeAllButThis(childForm);
        }
        private void fRICTIONBLENDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmFrictionBlendWeigh();
            childForm.Name = "FRICTION BLEND WEIGHING";
            DisposeAllButThis(childForm);
        }
        private void fRICTIONPLANToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmFrictionBlendWeigh();
            childForm.Name = "FRICTION SOURCE PLAN";
            DisposeAllButThis(childForm);
        }
        private void cOMPCONCESSIONCHARTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmConcessionChart();
            childForm.Name = "COMP CONCESSION CHART";
            DisposeAllButThis(childForm);
        }
        private void cOMPCONCESSIONPLANToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new frmConcessionPlan();
            childForm.Name = "COMP CONCESSION PLAN";
            DisposeAllButThis(childForm);
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new frmWeightMaster();
            childForm.Name = "WEIGHT STANDARAD VIEW";
            DisposeAllButThis(childForm);
        }

        private void sTENCILNOToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void wEIGHINGToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pRODALLOCATEToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sTATUSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new Status();
            DisposeAllButThis(childForm);
        }
    }
}
