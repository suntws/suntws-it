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
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Net;

namespace GT
{
    public partial class frmOrderImport : Form
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int conn, int val);

        SqlConnection oleCon = new SqlConnection();
        DataTable dtitem = new DataTable();
        string strO_Method = "", str_CustType = "";

        DBAccess dba = new DBAccess();
        public frmOrderImport()
        {
            InitializeComponent();
        }
        private DataTable GetData_FromOrderDB_SP(string strSP, SqlParameter[] spParam, string execute)
        {
            DataTable dtData = new DataTable();
            try
            {
                int Out;
                if (InternetGetConnectedState(out Out, 0) == false)
                    MessageBox.Show("Internet Not Connected !");
                else
                {
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
                        oleCon.Open();
                    }
                    catch (SqlException)
                    {
                        try
                        {
                            oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=ORDERDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
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
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = strSP;
                        if (spParam != null)
                        {
                            foreach (SqlParameter Sp in spParam)
                            {
                                cmd.Parameters.Add(Sp);
                            }
                        }
                        cmd.CommandTimeout = 1200;
                        cmd.Connection = oleCon;
                        if (execute == "reader")
                        {
                            DataSet ds = new DataSet();
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            adp.Fill(ds);
                            if (ds.Tables[0].Rows.Count > 0)
                                dtData = ds.Tables[0];
                        }
                        else
                            cmd.ExecuteNonQuery();

                        cmd.Dispose();
                        oleCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderImport", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dtData;
        }
        private void frmOrderImport_Load(object sender, EventArgs e)
        {
            try
            {
                strO_Method = ""; str_CustType = "";
                cmbCustomerName.DataSource = null;
                cmbWorkOrderNo.DataSource = null;
                dgv_NonDataMaster.DataSource = null;
                txtOrderQuantity.Text = "";
                txtStockQuantity.Text = "";
                txtRequiredQuantity.Text = "";

                dgvprodorder.DataSource = null;
                dgvprodorder.Columns.Clear();
                dgvprodorder.Rows.Clear();

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Plant", Program.strPlantName) };
                DataTable dtCust = GetData_FromOrderDB_SP("SP_SEL_NewOrder_export_v1", sp, "reader");
                if (dtCust.Rows.Count > 0)
                {
                    DataRow dr = dtCust.NewRow();
                    dr.ItemArray = new object[] { "--CHOOSE--" };
                    dtCust.Rows.InsertAt(dr, 0);
                    cmbCustomerName.DataSource = dtCust;
                    cmbCustomerName.DisplayMember = "custfullname";
                    cmbCustomerName.ValueMember = "custcode";
                    if (cmbCustomerName.Items.Count == 2)
                        cmbCustomerName.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderImport", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbWorkOrderNo.DataSource = null;
                dgv_NonDataMaster.DataSource = null;
                txtOrderQuantity.Text = "";
                txtStockQuantity.Text = "";
                txtRequiredQuantity.Text = "";

                dgvprodorder.DataSource = null;
                dgvprodorder.Columns.Clear();
                dgvprodorder.Rows.Clear();

                if (cmbCustomerName.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Plant", Program.strPlantName), new SqlParameter("@custstdcode", cmbCustomerName.SelectedValue.ToString()) };
                    DataTable dt = GetData_FromOrderDB_SP("sp_sel_newProd_WorkOrderNo", sp, "reader");
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr.ItemArray = new object[] { "--CHOOSE--" };
                        dt.Rows.InsertAt(dr, 0);
                        cmbWorkOrderNo.DataSource = dt;
                        cmbWorkOrderNo.DisplayMember = "WorkOrderNo";
                        cmbWorkOrderNo.ValueMember = "O_ID";
                        if (cmbWorkOrderNo.Items.Count == 2)
                            cmbWorkOrderNo.SelectedIndex = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderImport", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void cmbWorkOrderNo_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                dgv_NonDataMaster.DataSource = null;
                txtOrderQuantity.Text = "";
                txtStockQuantity.Text = "";
                txtRequiredQuantity.Text = "";
                dgvprodorder.DataSource = null;
                dgvprodorder.Columns.Clear();
                if (cmbWorkOrderNo.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@id", Convert.ToInt32(cmbWorkOrderNo.SelectedValue.ToString())), 
                        new SqlParameter("@WorkOrderNO", Convert.ToString(cmbWorkOrderNo.Text)) 
                    };
                    DataTable dt = GetData_FromOrderDB_SP("sp_sel_PreparationplaningMaster_v2", sp, "reader");
                    if (dt.Rows.Count > 0)
                    {
                        txtStockQuantity.Text = dt.Rows[0]["StockQuantity"].ToString();
                        txtOrderQuantity.Text = dt.Rows[0]["OrderQuantity"].ToString();
                        txtRequiredQuantity.Text = dt.Rows[0]["RequiredQuantity"].ToString();
                        lblRfd.Text = dt.Rows[0]["rfd"].ToString();
                        strO_Method = dt.Rows[0]["O_Method"].ToString();
                        str_CustType = dt.Rows[0]["CustType"].ToString();

                        SqlParameter[] spSel = new SqlParameter[] { 
                            new SqlParameter("@O_ID", Convert.ToInt32(cmbWorkOrderNo.SelectedValue.ToString())), 
                            new SqlParameter("@WorkOrderNO", Convert.ToString(cmbWorkOrderNo.Text)) 
                        };
                        dtitem = GetData_FromOrderDB_SP("sp_sel_orderitemlist_for_split1", spSel, "reader");

                        if (dtitem.Rows.Count > 0)
                        {
                            dgvprodorder.DataSource = dtitem;
                            dgvprodorder.Columns[0].Visible = false;
                            dgvprodorder.Columns[1].Visible = false;
                            dgvprodorder.Columns[2].Width = 80; //Platform
                            dgvprodorder.Columns[3].Width = 150; //tyre size
                            dgvprodorder.Columns[4].Width = 50; //rim
                            dgvprodorder.Columns[5].Width = 70; //type
                            dgvprodorder.Columns[6].Width = 80; //brand
                            dgvprodorder.Columns[7].Width = 80; //sidewall
                            dgvprodorder.Columns[8].Width = 80; //process-id
                            dgvprodorder.Columns[8].HeaderText = "PROCESS-ID";
                            dgvprodorder.Columns[9].Width = 60; //order qty
                            dgvprodorder.Columns[9].HeaderText = "ORDER";
                            dgvprodorder.Columns[10].Width = 60; //sotck qty
                            dgvprodorder.Columns[10].HeaderText = "STOCK";
                            dgvprodorder.Columns[11].Width = 100; //req qty
                            dgvprodorder.Columns[11].HeaderText = "PRODUCTION";

                            Build_UnAvailableMasters(dtitem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderImport", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCustomerName.Text == "--CHOOSE--")
                {
                    lbl_errmsg.Text = "Choose Customer Name...";
                    cmbCustomerName.Focus();
                }
                else if (cmbWorkOrderNo.Text == "--CHOOSE--")
                {
                    lbl_errmsg.Text = "Choose WorkOrder No...";
                    cmbWorkOrderNo.Focus();
                }
                else
                {
                    SqlParameter[] spM = new SqlParameter[] { 
                        new SqlParameter("@New_OrderItem_DataTable1", dtitem), 
                        new SqlParameter("@O_ID", cmbWorkOrderNo.SelectedValue.ToString()), 
                        new SqlParameter("@CustomerName", cmbCustomerName.Text), 
                        new SqlParameter("@CustomerCode", cmbCustomerName.SelectedValue.ToString()),
                        new SqlParameter("@WorkOrderNo", cmbWorkOrderNo.Text), 
                        new SqlParameter("@OrderQty", txtOrderQuantity.Text), 
                        new SqlParameter("@StockQty", txtStockQuantity.Text), 
                        new SqlParameter("@RequiredQty", txtRequiredQuantity.Text), 
                        new SqlParameter("@UserName", Program.strUserName), 
                        new SqlParameter("@Plant", Program.strPlantName),
                        new SqlParameter("@O_Method", strO_Method),
                        new SqlParameter("@CustType", str_CustType)
                    };
                    int resp = dba.ExecuteNonQuery_SP("sp_upd_NewOrderItem_table_v3", spM);
                    if (resp < 0)
                        MessageBox.Show("Record Already Moved!..");
                    else
                    {
                        SqlParameter[] sppdi = new SqlParameter[] { 
                            new SqlParameter("@O_ID", cmbWorkOrderNo.SelectedValue.ToString()), 
                            new SqlParameter("@CustStdCode", cmbCustomerName.SelectedValue.ToString()),
                            new SqlParameter("@workorderno", cmbWorkOrderNo.Text)
                        };
                        DataTable dt1 = GetData_FromOrderDB_SP("sp_upd_Ordermaster_TOMovePDi", sppdi, "writer");

                        SqlParameter[] spchk = new SqlParameter[] { 
                            new SqlParameter("@O_ID", cmbWorkOrderNo.SelectedValue.ToString()), 
                            new SqlParameter("@CustStdCode", cmbCustomerName.SelectedValue.ToString()),
                            new SqlParameter("@workorderno", cmbWorkOrderNo.Text)
                        };
                        DataTable dtChk = GetData_FromOrderDB_SP("sp_sel_orderitem_for_GtProduction_chk", spchk, "reader");
                        if (dtChk.Rows.Count > 0)
                        {
                            SqlParameter[] spdel = new SqlParameter[] { 
                                new SqlParameter("@Order_ItemID_Dt", dtChk), 
                                new SqlParameter("@O_ID", cmbWorkOrderNo.SelectedValue.ToString()), 
                                new SqlParameter("@CustomerName", cmbCustomerName.Text), 
                                new SqlParameter("@CustomerCode", cmbCustomerName.SelectedValue.ToString()),
                                new SqlParameter("@WorkOrderNo", cmbWorkOrderNo.Text)
                            };
                            dba.ExecuteNonQuery_SP("sp_disable_TTS_GT_Items_v1", spdel);
                        }
                        Merge_ProcessID();
                        MessageBox.Show("Record  Moved Successfully!..");
                    }
                    frmOrderImport_Load(null, null);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderImport", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                dgv_NonDataMaster.DataSource = null;
                txtOrderQuantity.Text = "";
                txtStockQuantity.Text = "";
                txtRequiredQuantity.Text = "";

                dgvprodorder.DataSource = null;
                dgvprodorder.Columns.Clear();
                dgvprodorder.Rows.Clear();
                cmbCustomerName.SelectedIndex = 0;
                cmbWorkOrderNo.DataSource = null;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderImport", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Merge_ProcessID()
        {
            try
            {
                try
                {
                    oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath1 + ";Database=TTSDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
                    oleCon.Open();
                }
                catch (SqlException)
                {
                    try
                    {
                        oleCon = new SqlConnection("Data Source=" + Program.strServerDbPath2 + ";Database=TTSDB;User ID=sa;Password=" + Program.strServerDbPass + ";Trusted_Connection=False;");
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
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = oleCon;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update ProcessID_Details set " + Program.strPlantName + "_status=2 where " + Program.strPlantName + "_status=1";
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand();
                    cmd.Connection = oleCon;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select Config,TyreSize,TyreRim,TyreType,Brand,Sidewall,ProcessID,CreateDate,FinishedWt from ProcessID_Details where " + Program.strPlantName + "_status=2";
                    DataTable dt_TTSProcessID = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt_TTSProcessID);

                    if (dt_TTSProcessID.Rows.Count > 0)
                    {
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
                            }
                        }

                        if (resp > 0)
                        {
                            cmd = new SqlCommand();
                            cmd.Connection = oleCon;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "update ProcessID_Details set " + Program.strPlantName + "_status=3 where " + Program.strPlantName + "_status=2";
                            cmd.ExecuteNonQuery();

                            if (dtMail.Rows.Count > 0)
                            {
                                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@ProcessID_Merged_DataTable", dtMail), new SqlParameter("@Plant", Program.strPlantName) };
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

                                string strTTSQuery = "select COUNT(*) as TTSCOUNT,MailReceipent from ProcessID_Details a,EmailAddressList b where a." + Program.strPlantName +
                                    "_status in (3,4) and b.MailType='PROCESS-ID MERGE " + Program.strPlantName + "' group by b.MailReceipent";
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
                                        html += "<br/><br/><br/><span style='color:#d34639;'>This is a system generated mail. Please do not reply to this email ID.</span>";
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderImport", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_UnAvailableMasters(DataTable dtItems)
        {
            try
            {
                dgv_NonDataMaster.DataSource = null;
                btnSave.Visible = true;

                SqlParameter[] sppdi = new SqlParameter[] { 
                    new SqlParameter("@O_ID", cmbWorkOrderNo.SelectedValue.ToString()), 
                    new SqlParameter("@CustStdCode", cmbCustomerName.SelectedValue.ToString()),
                    new SqlParameter("@workorderno", cmbWorkOrderNo.Text)
                };
                DataTable dt1 = GetData_FromOrderDB_SP("sp_upd_ProcessID_EnableToPlant", sppdi, "writer");

                DataTable dtData = dtItems.DefaultView.ToTable(true, "SIZE", "RIM", "TYPE");
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Order_Items_MasterCheck_Dt", dtData) };
                DataTable dtNonDataMaster = (DataTable)dba.ExecuteReader_SP("sp_chk_MasterDataAvailable", sp, DBAccess.Return_Type.DataTable);
                if (dtNonDataMaster.Rows.Count > 0)
                {
                    dgv_NonDataMaster.DataSource = dtNonDataMaster;
                    dgv_NonDataMaster.Columns[0].HeaderText = "ITEM";
                    dgv_NonDataMaster.Columns[0].Width = 150;
                    dgv_NonDataMaster.Columns[1].HeaderText = "REMARKS";
                    dgv_NonDataMaster.Columns[1].Width = 200;
                    btnSave.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmOrderImport", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

