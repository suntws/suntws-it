using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT
{
    static class ttsintegration
    {
        static void Main()
        {
        }
        private static void ProcessId_Merge()
        {
            try
            {
                SqlConnection oleCon = new SqlConnection();
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
                    int i = 0;
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
                                        lRow["Brand"].ToString() != row["Brand"].ToString() || lRow["Sidewall"].ToString() != row["Sidewall"].ToString())
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
                                        resp = dba.ExecuteNonQuery_SP("", spUpd);
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
                            MessageBox.Show("PROCESS-ID MERGED SUCCESSFULLY", "BARCODE_APP", MessageBoxButtons.OK);
                        }
                        else
                            MessageBox.Show("PROCESS-ID NOT MERGED. PLEASE TRY AGAIN LATER", "FAILURE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("NO RECORDS AVAILABLE FOR MERGE", "BARCODE_APP", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("TTS NOT CONNECTED. PLEASE CONTACT ADMINISTRATOR", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
