using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace GT
{
    public partial class frmTtsIntegration : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int conn, int val);
        DBAccess dba = new DBAccess();
        public frmTtsIntegration()
        {
            InitializeComponent();
        }
        private void frmAutoUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                label1.Text = "";
                int Out;
                if (InternetGetConnectedState(out Out, 0) == false)
                {
                    MessageBox.Show("Internet Not Connected !");
                    this.Hide();
                }
                else if (this.Text == "MODIFY TYRE DATA AND RE-BARCODE IT" || this.Text == "BARCODED RE-PRINT")
                {
                    btnDispatchMerge_Click();
                    frmAutoUpdate_Leave(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnProcessIDMerge_Click(object sender, EventArgs e)
        {
            try
            {
                int Out;
                if (InternetGetConnectedState(out Out, 0) == true)
                {
                    SqlConnection oleCon = new SqlConnection();
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=TTSDB;User ID=sa;Password=" + Program.strServerDbPass +
                            ";Trusted_Connection=False;");
                        oleCon.Open();
                    }
                    catch (SqlException)
                    {
                        try
                        {
                            oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=TTSDB;User ID=sa;Password=" + Program.strServerDbPass +
                                ";Trusted_Connection=False;");
                            oleCon.Open();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (oleCon.State == ConnectionState.Open)
                    {
                        progressBar1.Visible = true;
                        int i = 0;
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = oleCon;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update ProcessID_Details set " + Program.strPlantName + "_status=2 where " + Program.strPlantName + "_status=1";
                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand();
                        cmd.Connection = oleCon;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select Config,TyreSize,TyreRim,TyreType,Brand,Sidewall,ProcessID,CreateDate,FinishedWt from ProcessID_Details where " +
                            Program.strPlantName + "_status=2 and ID_Status=1";
                        DataTable dt_TTSProcessID = new DataTable();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt_TTSProcessID);

                        progressBar1.Maximum = dt_TTSProcessID.Rows.Count;
                        progressBar1.Value = i++;

                        if (dt_TTSProcessID.Rows.Count > 0)
                        {
                            label1.Text = "Please wait . . .";
                            DataTable dt_ProcessID = (DataTable)dba.ExecuteReader_SP("SP_SEL_EditBarcode_ProcessIdDetails", DBAccess.Return_Type.DataTable);

                            DataTable dtMail = new DataTable();
                            DataColumn col = new DataColumn("PROCESS-ID", System.Type.GetType("System.String"));
                            dtMail.Columns.Add(col);
                            col = new DataColumn("ACTION", System.Type.GetType("System.String"));
                            dtMail.Columns.Add(col);

                            int resp = 0;
                            foreach (DataRow row in dt_TTSProcessID.Rows)
                            {
                                bool boolProcessID = true;
                                if (dt_ProcessID != null && dt_ProcessID.Rows.Count > 0)
                                {
                                    foreach (DataRow lRow in dt_ProcessID.Select("ProcessID='" + row["ProcessID"].ToString() + "'"))
                                    {
                                        boolProcessID = false;
                                        if (lRow["Config"].ToString() != row["Config"].ToString() || lRow["TyreSize"].ToString() != row["TyreSize"].ToString() ||
                                            lRow["TyreRim"].ToString() != row["TyreRim"].ToString() || lRow["TyreType"].ToString() != row["TyreType"].ToString() ||
                                            lRow["Brand"].ToString() != row["Brand"].ToString() || lRow["Sidewall"].ToString() != row["Sidewall"].ToString() ||
                                            lRow["finishedWt"].ToString() != row["finishedWt"].ToString())
                                        {
                                            SqlParameter[] spUpd = new SqlParameter[] { 
                                                new SqlParameter("@Config", row["Config"].ToString()), 
                                                new SqlParameter("@TyreSize", row["TyreSize"].ToString()), 
                                                new SqlParameter("@TyreRim", row["TyreRim"].ToString()), 
                                                new SqlParameter("@TyreType", row["TyreType"].ToString()), 
                                                new SqlParameter("@Brand", row["Brand"].ToString()), 
                                                new SqlParameter("@Sidewall", row["Sidewall"].ToString()), 
                                                new SqlParameter("@finishedWt", row["finishedWt"].ToString()), 
                                                new SqlParameter("@ProcessID", row["ProcessID"].ToString()) 
                                            };
                                            resp = dba.ExecuteNonQuery_SP("sp_upd_ProcessID_Details", spUpd);
                                            dtMail.Rows.Add(row["ProcessID"].ToString(), "UPDATED");
                                        }
                                    }
                                }

                                if (boolProcessID)
                                {
                                    SqlParameter[] spIns = new SqlParameter[] { 
                                        new SqlParameter("@Config", row["Config"].ToString()), 
                                        new SqlParameter("@TyreSize", row["TyreSize"].ToString()), 
                                        new SqlParameter("@TyreRim", row["TyreRim"].ToString()), 
                                        new SqlParameter("@TyreType", row["TyreType"].ToString()), 
                                        new SqlParameter("@Brand", row["Brand"].ToString()), 
                                        new SqlParameter("@Sidewall", row["Sidewall"].ToString()), 
                                        new SqlParameter("@finishedWt", row["finishedWt"].ToString()), 
                                        new SqlParameter("@ProcessID", row["ProcessID"].ToString()) 
                                    };
                                    resp = dba.ExecuteNonQuery_SP("sp_ins_ProcessID_Details", spIns);
                                    dtMail.Rows.Add(row["ProcessID"].ToString(), "ADDED");
                                    progressBar1.Value = i++;
                                }
                            }

                            if (resp > 0)
                            {
                                label1.Text = "";
                                cmd = new SqlCommand();
                                cmd.Connection = oleCon;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "update ProcessID_Details set " + Program.strPlantName + "_status=3 where " + Program.strPlantName + "_status=2";
                                cmd.ExecuteNonQuery();

                                if (dtMail.Rows.Count > 0)
                                {
                                    SqlParameter[] sp1 = new SqlParameter[] { 
                                        new SqlParameter("@ProcessID_Merged_DataTable", dtMail), 
                                        new SqlParameter("@Plant", Program.strPlantName) 
                                    };
                                    SqlCommand command = new SqlCommand();
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.CommandText = "sp_ins_ProcessID_Merged_Report";
                                    command.CommandTimeout = 1200;
                                    command.Connection = oleCon;
                                    foreach (SqlParameter Sp in sp1)
                                    {
                                        command.Parameters.Add(Sp);
                                    }
                                    command.ExecuteNonQuery();

                                    string strTTSQuery = "select COUNT(*) as TTSCOUNT,MailReceipent from ProcessID_Details a,EmailAddressList b where a." +
                                        Program.strPlantName +
                                        "_status in (3,4) and b.MailType='PROCESS-ID MERGE " + Program.strPlantName + "' and ID_Status=1 group by b.MailReceipent";
                                    cmd = new SqlCommand();
                                    cmd.Connection = oleCon;
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = strTTSQuery;
                                    DataSet ds = new DataSet();
                                    adp = new SqlDataAdapter(cmd);
                                    adp.Fill(ds);

                                    DataTable dtCount = (DataTable)dba.ExecuteReader_SP("sp_get_ProcessID_count", DBAccess.Return_Type.DataTable);
                                    try
                                    {
                                        string html = "";
                                        if (ds.Tables[0].Rows[0]["TTSCOUNT"].ToString() != dtCount.Rows[0]["PIDCOUNT"].ToString())
                                        {
                                            html = "Dear Team,<br/>  You have merged the below process code to your barcode database <br/>";
                                            html += "<br/> PROCESS-ID COUNT NOT EQUAL: <br/> TTS - " + ds.Tables[0].Rows[0]["TTSCOUNT"].ToString() + "<br/> " +
                                                "BARCODE - " + dtCount.Rows[0]["PIDCOUNT"].ToString();
                                            html += "<br/><span style='color:#d34639;'>This is a system generated mail. Please do not reply to this email ID.</span>";
                                        }

                                        if (html != "")
                                        {
                                            MailMessage msg = new MailMessage();
                                            msg.From = new MailAddress("cityoffice_reservations@sun-tws.com");
                                            msg.To.Add(ds.Tables[0].Rows[0]["MailReceipent"].ToString());
                                            msg.CC.Add("edc_software@sun-tws.com");
                                            msg.Subject = "REPORT FOR PROCESS-ID MERGED DETAILS";
                                            msg.Body = html;
                                            msg.IsBodyHtml = true;

                                            SmtpClient smt = new SmtpClient();
                                            smt.Host = "smtp.gmail.com";
                                            System.Net.NetworkCredential ntcd = new NetworkCredential();
                                            ntcd.UserName = "cityoffice_reservations@sun-tws.com";
                                            ntcd.Password = "$GCm#8g1";
                                            smt.Credentials = ntcd;
                                            smt.EnableSsl = true;
                                            smt.Port = 587;
                                            smt.Send(msg);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string strErrQuery = "insert into ProcessID_Merged_Report (Plant,ProcessID,Merge_Action,MergeOn) " +
                                        "values('" + Program.strPlantName + "','ERROR','" + ex.Message + "',GETDATE())";
                                        cmd = new SqlCommand();
                                        cmd.Connection = oleCon;
                                        cmd.CommandType = CommandType.Text;
                                        cmd.CommandText = strErrQuery;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                MessageBox.Show("PROCESS-ID MERGED SUCCESSFULLY", "BARCODE_APP", MessageBoxButtons.OK);
                            }
                            else
                                MessageBox.Show("PROCESS-ID NOT MERGED. PLEASE TRY AGAIN LATER", "FAILURE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                            MessageBox.Show("NO RECORDS AVAILABLE FOR PROCESS-ID MERGE", "PROCESS-ID", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressBar1.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("TTS NOT CONNECTED. PLEASE CONTACT ADMINISTRATOR", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        label1.Text = "";
                        progressBar1.Visible = false;
                    }
                }
                else
                {
                    label1.Text = "";
                    progressBar1.Visible = false;
                    MessageBox.Show("Internet Not Connected !");
                }
            }
            catch (Exception ex)
            {
                label1.Text = "";
                progressBar1.Visible = false;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnStockMerge_Click(object sender, EventArgs e)
        {
            try
            {
                int Out;
                label1.Text = "Please Wait . . .";
                if (InternetGetConnectedState(out Out, 0) == true)
                {
                    SqlConnection oleCon = new SqlConnection();
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass +
                            ";Trusted_Connection=False;");
                        oleCon.Open();
                    }
                    catch (SqlException)
                    {
                        try
                        {
                            oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass +
                                ";Trusted_Connection=False;");
                            oleCon.Open();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (oleCon.State == ConnectionState.Open)
                    {
                        label1.Text = "Please wait . . .";
                        progressBar1.Visible = true;
                        int i = 0;
                        progressBar1.Maximum = 10;
                        progressBar1.Value = i++;
                        SqlParameter[] spUpd = new SqlParameter[] { new SqlParameter("@MergeStatusTo", 2), new SqlParameter("@MergeStatusFrom", 1) };
                        dba.ExecuteNonQuery_SP("sp_upd_tbqualitycontrol_MergeStatus", spUpd);
                        progressBar1.Value = i++;
                        progressBar1.Value = i++;
                        DataTable dt_BARCODESTOCK = (DataTable)dba.ExecuteReader_SP("sp_sel_stockmerge_GT_list_v2", DBAccess.Return_Type.DataTable);
                        if (dt_BARCODESTOCK.Rows.Count > 0)
                        {
                            //insert Records To Orderdb
                            SqlParameter[] sp1 = new SqlParameter[]{
                                new SqlParameter("@Production_warehouse_DataTable", dt_BARCODESTOCK),
                                new SqlParameter("@plant", Program.strPlantName)
                            };
                            SqlCommand command = new SqlCommand();
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "sp_ins_stockdata_V3";
                            command.CommandTimeout = 1200;
                            command.Connection = oleCon;
                            foreach (SqlParameter Sp in sp1)
                            {
                                command.Parameters.Add(Sp);
                            }
                            int resp = command.ExecuteNonQuery();

                            progressBar1.Value = i++;
                            if (resp > 0)
                            {
                                spUpd = new SqlParameter[] { new SqlParameter("@MergeStatusTo", 3), new SqlParameter("@MergeStatusFrom", 2) };
                                dba.ExecuteNonQuery_SP("sp_upd_tbqualitycontrol_MergeStatus", spUpd);
                                MessageBox.Show("STOCK MERGED SUCCESSFULLY", "GT", MessageBoxButtons.OK);
                            }
                            progressBar1.Value = i++;
                        }
                        else
                            MessageBox.Show("NO RECORDS");

                        label1.Text = "";
                        progressBar1.Value = i++;
                        progressBar1.Value = progressBar1.Maximum;
                        progressBar1.Value = 0;
                        progressBar1.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("TTS NOT CONNECTED. PLEASE CONTACT ADMINISTRATOR", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        label1.Text = "";
                        progressBar1.Visible = false;
                    }
                }
                else
                {
                    label1.Text = "";
                    progressBar1.Visible = false;
                    MessageBox.Show("Internet Not Connected !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label1.Text = "";
                progressBar1.Visible = false;
            }
        }
        private void btnDispatchMerge_Click()
        {
            try
            {
                int Out;
                if (InternetGetConnectedState(out Out, 0) == true)
                {
                    SqlConnection oleCon = new SqlConnection();
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass +
                            ";Trusted_Connection=False;");
                        oleCon.Open();
                    }
                    catch (SqlException)
                    {
                        try
                        {
                            oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass +
                                ";Trusted_Connection=False;");
                            oleCon.Open();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (oleCon.State == ConnectionState.Open)
                    {
                        //Get Records From Orderdb
                        SqlCommand command = new SqlCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "sp_sel_PDI_TO_GT";
                        command.CommandTimeout = 1200;
                        command.Connection = oleCon;
                        SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@plant", Program.strPlantName) };
                        foreach (SqlParameter Sp in sp1)
                        {
                            command.Parameters.Add(Sp);
                        }
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
                        {
                            SqlParameter[] spM = new SqlParameter[]{
                                new SqlParameter("@PDI_Dispatch_DataTable1", ds.Tables[0]),
                                new SqlParameter("@PDI_Dispatch_DataTable2", ds.Tables[1])
                            };
                            int resp = dba.ExecuteNonQuery_SP("sp_upd_PDI_Dispatch", spM);

                            if (resp > 0)
                            {
                                command = new SqlCommand();
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandText = "sp_upd_Gt_TO_Pdi";
                                command.CommandTimeout = 1200;
                                command.Connection = oleCon;
                                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@plant", Program.strPlantName) };
                                foreach (SqlParameter Sp in sp2)
                                {
                                    command.Parameters.Add(Sp);
                                }
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("TTS NOT CONNECTED. PLEASE CONTACT ADMINISTRATOR", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Internet Not Connected !");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisposeAllButThis(Form form)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm != form)
                    frm.Close();
            }
            form.Location = new Point(0, 0);
            form.MdiParent = this.MdiParent;
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmAutoUpdate_Leave(object sender, EventArgs e)
        {
            try
            {
                int Out;
                if (InternetGetConnectedState(out Out, 0) == true)
                {
                    SqlConnection oleCon = new SqlConnection();
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass +
                            ";Trusted_Connection=False;");
                        oleCon.Open();
                    }
                    catch (SqlException)
                    {
                        try
                        {
                            oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass +
                                ";Trusted_Connection=False;");
                            oleCon.Open();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (oleCon.State == ConnectionState.Open)
                    {
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Plant", Program.strPlantName) };
                        DataTable dtVerify = (DataTable)dba.ExecuteReader_SP("sp_sel_RunningOrder_for_verify", sp, DBAccess.Return_Type.DataTable);

                        if (dtVerify.Rows.Count > 0)
                        {
                            SqlParameter[] sp1 = new SqlParameter[] { 
                                new SqlParameter("@RunningWorkorder", dtVerify), 
                                new SqlParameter("@PdiPlant", Program.strPlantName) 
                            };
                            SqlCommand command = new SqlCommand();
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "sp_sel_completed_order_for_gt";
                            command.CommandTimeout = 1200;
                            command.Connection = oleCon;
                            foreach (SqlParameter Sp in sp1)
                            {
                                command.Parameters.Add(Sp);
                            }
                            DataSet ds = new DataSet();
                            SqlDataAdapter adp = new SqlDataAdapter(command);
                            adp.Fill(ds);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                SqlParameter[] spUpd = new SqlParameter[] { 
                                    new SqlParameter("@RunningWorkorder", ds.Tables[0]), 
                                    new SqlParameter("@Plant", Program.strPlantName) 
                                };
                                dba.ExecuteNonQuery_SP("sp_upd_NewProductionMasterDetails_Status", spUpd);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("TTS NOT CONNECTED. PLEASE CONTACT ADMINISTRATOR", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        label1.Text = "";
                        progressBar1.Visible = false;
                    }
                }
                else
                {
                    label1.Text = "";
                    progressBar1.Visible = false;
                    MessageBox.Show("Internet Not Connected !");
                }
            }
            catch (Exception ex)
            {
                label1.Text = "";
                progressBar1.Visible = false;
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
