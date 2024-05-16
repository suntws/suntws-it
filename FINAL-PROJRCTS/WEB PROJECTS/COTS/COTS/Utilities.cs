using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace COTS
{
    public class Utilities
    {
        private static string ErrDB = ConfigurationManager.ConnectionStrings["ErrDB"].ConnectionString;
        private static string ORDERDB = ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString;
        private static string TTSDB = ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString;

        public static void WriteToErrorLog(string strAppName, string strModuleName, string strFunctionName, int intErrorid, string strErrorMsg)
        {
            try
            {
                string userName = "";
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session["cotsUser"] != null && HttpContext.Current.Session["cotsUser"].ToString() != "")
                        userName = HttpContext.Current.Session["cotsUser"].ToString();
                }
                SqlParameter[] param = new SqlParameter[6];
                COTS.DataAccess da = new COTS.DataAccess(ErrDB);

                param[0] = new SqlParameter("@ProjectName", strAppName);
                param[1] = new SqlParameter("@ModuleName", strModuleName);
                param[2] = new SqlParameter("@MethodName", strFunctionName);
                param[3] = new SqlParameter("@EID", intErrorid);
                param[4] = new SqlParameter("@ErrorMsg", strErrorMsg);
                param[5] = new SqlParameter("@UserName", userName);

                int status = da.ExecuteNonQuery_SP("sp_ErrorDetails", param);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public static void CotsOrderMailSent(string strBody, string strSubject, string strTo, string strCC)
        {
            try
            {
                if (!string.IsNullOrEmpty(strTo))
                {
                    YmailSender es = new YmailSender();
                    es.From = HttpContext.Current.Session["cotsstdcode"].ToString() == "DE0048" ? "s-cots_domestic@sun-tws.com" : "scots_international@sun-tws.com";
                    es.To = strTo;
                    es.CC = strCC;
                    es.Password = HttpContext.Current.Session["cotsstdcode"].ToString() == "DE0048" ? "Y4K/HsD1" : "W^5/:]r1";
                    es.Subject = strSubject;
                    es.Body = strBody + "<br/><br/><br/><span style='color:#d34639;'>This is a system generated mail. Please do not reply to this email ID.</span>";
                    es.IsHtmlBody = true;
                    es.EmailProvider = YmailSender.EmailProviderType.Gmail;
                    es.Send();
                }
            }
            catch (Exception ex)
            {
                WriteToErrorLog("COTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, strSubject + " - " + ex.Message);
            }
        }

        public static void CotsClaimMailSent(string strBody, string strSubject, string strTo, string strCC)
        {
            try
            {
                YmailSender es = new YmailSender();
                es.From = "claims_alert@sun-tws.com";
                es.To = strTo;
                es.CC = strCC;
                es.Password = "2tCCK%n1";
                es.Subject = strSubject;
                es.Body = strBody + "<br/><br/><br/><span style='color:#d34639;'>This is a system generated mail. Please do not reply to this email ID.</span>";
                es.IsHtmlBody = true;
                es.EmailProvider = YmailSender.EmailProviderType.Gmail;
                es.Send();
            }
            catch (Exception ex)
            {
                WriteToErrorLog("COTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, strSubject + " - " + ex.Message);
            }
        }

        public static string Build_CC_MailList(string strCustCode, string strPrevCC)
        {
            string returnValue = strPrevCC;
            try
            {
                COTS.DataAccess daCots = new COTS.DataAccess(ORDERDB);
                COTS.DataAccess daTts = new COTS.DataAccess(TTSDB);
                SqlParameter[] spLeadName = new SqlParameter[1];
                spLeadName[0] = new SqlParameter("@custcode", strCustCode);
                DataTable dtLead = (DataTable)daCots.ExecuteReader_SP("sp_sel_AssociateLeadName", spLeadName, DataAccess.Return_Type.DataTable);
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
                    DataTable dtMail = (DataTable)daTts.ExecuteReader_SP("sp_sel_AssociateLeadEmail", spLeadMail, DataAccess.Return_Type.DataTable);
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
                WriteToErrorLog("COTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, "CC-Mail Prepare: " + " - " + ex.Message);
            }
            return returnValue;
        }

        public static string Build_Cliam_ToList(string strMailID, string strField)
        {
            string returnValue = strMailID;
            try
            {
                COTS.DataAccess daTTS = new COTS.DataAccess(TTSDB);
                SqlParameter[] spLeadName = new SqlParameter[1];
                spLeadName[0] = new SqlParameter("@ClaimFieldName", strField);
                DataTable dtMail = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ClaimModule_UserEmailId", spLeadName, DataAccess.Return_Type.DataTable);
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
            catch (Exception ex)
            {
                WriteToErrorLog("COTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, "CC-Mail Prepare: " + " - " + ex.Message);
            }
            return returnValue;
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}