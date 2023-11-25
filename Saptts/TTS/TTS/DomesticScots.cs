using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
namespace TTS
{
    public class DomesticScots
    {
        private static DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        private static DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        public static DataTable Bind_BillingAddress(string BillID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@addressid", BillID) };
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderAddressDetails", sp1, DataAccess.Return_Type.DataTable);
                return dt;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "DomesticScots.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }
        public static DataTable Bind_OrderMasterDetails(int OID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@OID", OID) };
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderAllDetails", sp1, DataAccess.Return_Type.DataTable);
                return dt;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "DomesticScots.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }
        public static DataTable complete_orderitem_list_V1(int OID)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@OID", OID) };
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complete_orderitem_list_V1", sp1, DataAccess.Return_Type.DataTable);
                return dt;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "DomesticScots.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }
        public static int ins_dom_StatusChangedDetails(int OID, string statusid, string feedback, string username)
        {
            int resp = 0;
            try
            {
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@O_ID", OID);
                sp1[1] = new SqlParameter("@statusid", Convert.ToInt32(statusid));
                sp1[2] = new SqlParameter("@feedback", feedback.Replace("\r\n", "~"));
                sp1[3] = new SqlParameter("@username", username);
                resp = daCOTS.ExecuteNonQuery_SP("sp_ins_dom_StatusChangedDetails", sp1);
                return resp;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "DomesticScots.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return resp;
        }
        public static void scots_domestic_mail_notification_WithAttach(string strBody, string strSubject, string strTo, string strCC, string strAttach)
        {
            try
            {
                if (strTo != "")
                {
                    YmailSender es = new YmailSender();
                    es.From = "s-cots_domestic@sun-tws.com";
                    es.To = strTo;
                    es.CC = strCC;
                    es.Password = "Y4K/HsD1";
                    es.Subject = strSubject;
                    es.AttachFile = strAttach;
                    es.Body = strBody + "<br/><br/><br/><span style='color:#d34639;'>This is a system generated mail. Please do not reply to this email ID.</span>";
                    es.IsHtmlBody = true;
                    es.EmailProvider = YmailSender.EmailProviderType.Gmail;
                    es.Send();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "DomesticScots.cs~" + strSubject + "~" + strTo + "~" + strCC, MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public static string Build_ScotsDomestic_CRM_MailList(string strCustCode)
        {
            string returnValue = string.Empty;
            try
            {
                SqlParameter[] spLeadName = new SqlParameter[1];
                spLeadName[0] = new SqlParameter("@custcode", strCustCode);
                DataTable dtLead = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AssociateLeadName", spLeadName, DataAccess.Return_Type.DataTable);
                if (dtLead.Rows.Count > 0)
                {
                    DataRow row = dtLead.Rows[0];
                    string strLead = string.Empty;
                    string strSupervisor = string.Empty;
                    string strManager = string.Empty;
                    if (row["Lead"].ToString() != "" & row["Lead"].ToString() != "Choose")
                        strLead += row["Lead"].ToString();
                    if (row["Supervisor"].ToString() != "" & row["Supervisor"].ToString() != "Choose")
                        strSupervisor += row["Supervisor"].ToString();
                    if (row["Manager"].ToString() != "" & row["Manager"].ToString() != "Choose")
                        strManager += row["Manager"].ToString();

                    SqlParameter[] spLeadMail = new SqlParameter[3];
                    spLeadMail[0] = new SqlParameter("@Lead", strLead);
                    spLeadMail[1] = new SqlParameter("@Supervisor", strSupervisor);
                    spLeadMail[2] = new SqlParameter("@Manager", strManager);
                    DataTable dtMail = (DataTable)daTTS.ExecuteReader_SP("sp_sel_AssociateLeadEmail", spLeadMail, DataAccess.Return_Type.DataTable);
                    if (dtMail.Rows.Count > 0)
                    {
                        foreach (DataRow mRow in dtMail.Rows)
                        {
                            if (returnValue.Length > 0)
                                returnValue += "," + mRow["PEmailID"].ToString();
                            else
                                returnValue += mRow["PEmailID"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "DomesticScots.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnValue;
        }
    }
}