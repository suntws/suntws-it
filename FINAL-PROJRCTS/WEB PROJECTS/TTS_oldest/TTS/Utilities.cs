using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace TTS
{
    public class Utilities
    {
        private static string ErrDB = ConfigurationManager.ConnectionStrings["ErrDB"].ConnectionString;
        private static string TTSDB = ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString;
        private static string COTSDB = ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString;

        public static void WriteToErrorLog(string strAppName, string strModuleName, string strFunctionName, int intErrorid, string strErrorMsg)
        {
            try
            {
                string userName = "";
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Request.Cookies["TTSUser"] != null && HttpContext.Current.Request.Cookies["TTSUser"].Value != "")
                        userName = HttpContext.Current.Request.Cookies["TTSUser"].Value;
                }
                SqlParameter[] param = new SqlParameter[6];
                TTS.DataAccess da = new TTS.DataAccess(ErrDB);

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
                Utilities.WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public static string ProperCase(string s)
        {
            if (s != null)
            {
                if (s.Trim() == "")
                    return "";
                char[] letters = s.Trim().ToCharArray();
                letters[0] = Char.ToUpper(letters[0]);
                for (int i = 0; i < letters.Length; i++)
                    if (letters[i] == '-')
                    {
                        letters[i] = ' ';
                        if (i != letters.Length - 1)
                            letters[i + 1] = Char.ToUpper(letters[i + 1]);
                    }
                    else if (letters[i] == ' ')
                    {
                        if (i != letters.Length - 1)
                            letters[i + 1] = Char.ToUpper(letters[i + 1]);
                    }
                return new string(letters);
            }
            else
                return "";
        }

        public static string getuserid(HttpContext httpcontext)
        {
            string userid = string.Empty;
            try
            {
                var httpCookie = httpcontext.Request.Cookies["UserId"];
                if (httpCookie != null)
                    userid = HttpUtility.UrlDecode(httpCookie.Value);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return userid;
        }

        public static DataTable GetCountryList(HttpContext httpcontext)
        {
            DataTable dt = new DataTable();
            try
            {
                if (HttpContext.Current != null)
                {
                    TTS.DataAccess da = new DataAccess(TTSDB);
                    SqlParameter[] sparam = new SqlParameter[1];

                    sparam[0] = new SqlParameter("@countrystatus", "1");
                    dt = (DataTable)da.ExecuteReader_SP("Sp_Sel_CountryName", sparam, DataAccess.Return_Type.DataTable);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }

        public static DataTable GetCityList(HttpContext httpcontext, string strCountry)
        {
            DataTable dt = new DataTable();
            try
            {
                if (HttpContext.Current != null)
                {
                    TTS.DataAccess da = new DataAccess(TTSDB);
                    SqlParameter[] sparam = new SqlParameter[2];

                    sparam[0] = new SqlParameter("@countryname", strCountry);
                    sparam[1] = new SqlParameter("@CountryStatus", "1");
                    dt = (DataTable)da.ExecuteReader_SP("Sp_Sel_City", sparam, DataAccess.Return_Type.DataTable);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }

        public static DataTable GetCurrecnyList(HttpContext httpcontext)
        {
            DataTable dt = new DataTable();
            try
            {
                if (HttpContext.Current != null)
                {
                    TTS.DataAccess da = new DataAccess(TTSDB);
                    SqlParameter[] sparam = new SqlParameter[1];

                    sparam[0] = new SqlParameter("@CurrencyStatus", "1");

                    dt = (DataTable)da.ExecuteReader_SP("Sp_Sel_Currency", sparam, DataAccess.Return_Type.DataTable);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }

        public static DataTable GetRatesIDList(HttpContext httpcontext, string strRatesID)
        {
            DataTable dt = new DataTable();
            try
            {
                if (HttpContext.Current != null)
                {
                    TTS.DataAccess da = new DataAccess(TTSDB);
                    SqlParameter[] sparam = new SqlParameter[1];

                    sparam[0] = new SqlParameter("@RatesID", strRatesID);
                    dt = (DataTable)da.ExecuteReader_SP("Sp_Sel_Like_RatesID", sparam, DataAccess.Return_Type.DataTable);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }

        public static decimal getArvValue(decimal unitPrice, decimal curValue, decimal finishedWt)
        {
            decimal arvVal = 0;
            try
            {
                arvVal = (unitPrice * curValue) / finishedWt;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return arvVal;
        }

        public static decimal getRmcValue(decimal typeCost, decimal BeadsQty, decimal sizeCost, decimal finishedWt, bool beadband)
        {
            decimal rmcVal = 0;
            try
            {
                if (beadband)
                    rmcVal = typeCost + (BeadsQty * sizeCost) / finishedWt;
                else
                    rmcVal = typeCost;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return rmcVal;
        }

        public static string Build_CC_MailList(string strCustCode, string strPrevCC)
        {
            string returnValue = strPrevCC;
            try
            {
                TTS.DataAccess daCots = new TTS.DataAccess(COTSDB);
                TTS.DataAccess daTts = new TTS.DataAccess(TTSDB);
                SqlParameter[] spLeadName = new SqlParameter[1];
                spLeadName[0] = new SqlParameter("@custcode", strCustCode);
                DataTable dtLead = (DataTable)daCots.ExecuteReader_SP("sp_sel_AssociateLeadName", spLeadName, DataAccess.Return_Type.DataTable);
                if (dtLead.Rows.Count > 0)
                {
                    DataRow row = dtLead.Rows[0];
                    string strLead = string.Empty;
                    string strSupervisor = string.Empty;
                    string strManager = string.Empty;
                    if (row["Lead"].ToString() != "" & row["Lead"].ToString().ToLower() != "choose")
                        strLead += row["Lead"].ToString();
                    if (row["Supervisor"].ToString() != "" & row["Supervisor"].ToString().ToLower() != "choose")
                        strSupervisor += row["Supervisor"].ToString();
                    if (row["Manager"].ToString() != "" & row["Manager"].ToString().ToLower() != "choose")
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
                WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, "CC-Mail Prepare: " + " - " + ex.Message);
            }
            return returnValue;
        }

        public static string DecimalToText(string decimalPart)
        {
            string[] digits = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
            string result = "";
            foreach (char c in decimalPart)
            {
                int i = (int)c - 48;
                if (i < 0 || i > 9) return ""; // invalid number, don't return anything
                result += " " + digits[i];
            }
            return result;
        }

        public static string NumberToText(int number, bool useAnd, bool useArab)
        {
            if (number == 0) return "Zero";

            string and = useAnd ? "and " : ""; // deals with using 'and' separator

            if (number == -2147483648) return "Minus Two Hundred " + and + "Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred " + and + "Forty Eight";

            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            num[0] = number % 1000; // units
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands
            num[3] = number / 10000000; // crores
            num[2] = num[2] - 100 * num[3]; // lakhs
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }

            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;

                u = num[i] % 10; // ones 
                t = num[i] / 10;
                h = num[i] / 100; // hundreds
                t = t - 10 * h; // tens

                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i < first) sb.Append(and);

                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0)
                {
                    if (u > 1 && i == 3)
                        sb.Append("Crores ");
                    else if (u > 1 && i == 2)
                        sb.Append("Lakhs ");
                    else
                        sb.Append(words3[i - 1]);
                }
            }

            string temp = sb.ToString().TrimEnd();

            if (useArab && Math.Abs(number) >= 1000000000)
            {
                int index = temp.IndexOf("Hundred Crore");
                if (index > -1) return temp.Substring(0, index) + "Arab" + temp.Substring(index + 13);
                index = temp.IndexOf("Hundred");
                return temp.Substring(0, index) + "Arab" + temp.Substring(index + 7);
            }
            return temp;
        }

        public static DataRow Get_PlantAddress(string StrPlant)
        {
            DataTable dt = new DataTable();
            DataRow dtRow = dt.NewRow();
            try
            {
                TTS.DataAccess daCots = new TTS.DataAccess(COTSDB);
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@Plant", StrPlant);
                dt = (DataTable)daCots.ExecuteReader_SP("sp_sel_PlantWise_List", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    dtRow = dt.Rows[0];
                }
            }
            catch (Exception ex)
            {
                WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, "CC-Mail Prepare: " + " - " + ex.Message);
            }
            return dtRow;
        }

        public static string Replace_OrderRefNo(string StrRefNo)
        {
            string strOrderNo = StrRefNo;
            try
            {
                strOrderNo = StrRefNo.Replace("_PART-A", "").Replace("_PART-B", "").Replace("(M)", "").Replace("(P)", "").Replace("(L)", "");
            }
            catch (Exception ex)
            {
                WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, "CC-Mail Prepare: " + " - " + ex.Message);
            }
            return strOrderNo;
        }

        public static string Build_Cliam_ToList(string strField)
        {
            string returnValue = string.Empty;
            try
            {
                TTS.DataAccess daTTS = new TTS.DataAccess(TTSDB);
                SqlParameter[] spLeadName = new SqlParameter[1];
                spLeadName[0] = new SqlParameter("@ClaimFieldName", strField);
                DataTable dtMail = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ClaimModule_UserEmailId", spLeadName, DataAccess.Return_Type.DataTable);
                if (dtMail.Rows.Count > 0)
                {
                    foreach (DataRow mRow in dtMail.Rows)
                    {
                        if (mRow["PEmailID"].ToString().ToLower() != "edc_software@sun-tws.com")
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
                WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, "Mail Prepare: " + " - " + ex.Message);
            }
            return returnValue;
        }

        public static void ClaimMoveToAnotherTeam_StatusInsert(string strcustcode, string strcomplaintno, string strplant, string strstatusid, string strcomments, string strusername)
        {
            try
            {
                TTS.DataAccess daCots = new TTS.DataAccess(COTSDB);
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@custcode", strcustcode);
                sp1[1] = new SqlParameter("@complaintno", strcomplaintno);
                sp1[2] = new SqlParameter("@plant", strplant);
                sp1[3] = new SqlParameter("@statusid", strstatusid);
                sp1[4] = new SqlParameter("@comments", strcomments);
                sp1[5] = new SqlParameter("@username", strusername);
                daCots.ExecuteNonQuery_SP("sp_ins_ClaimStatusChangedHistory", sp1);
            }
            catch (Exception ex)
            {
                WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);
        }

        public static void selectedListItem_Find(DropDownList ddlselected, string selectedtext, string type)
        {
            try
            {
                if (type.ToUpper() == "TEXT")
                {
                    ListItem selecteditem = ddlselected.Items.FindByText(selectedtext);
                    if (selecteditem != null)
                    {
                        ddlselected.Items.FindByText(ddlselected.SelectedItem.Text).Selected = false;
                        selecteditem.Selected = true;
                    }
                }
                else if (type.ToUpper() == "VALUE")
                {
                    ListItem selecteditem = ddlselected.Items.FindByValue(selectedtext);
                    if (selecteditem != null)
                    {
                        ddlselected.Items.FindByValue(ddlselected.SelectedItem.Value).Selected = false;
                        selecteditem.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public static void ddl_Binding(DropDownList ddlselected, DataTable dt, string TextField, string ValueField, string Index)
        {
            try
            {
                ddlselected.DataSource = null;
                ddlselected.DataBind();
                if (dt.Rows.Count > 0)
                {
                    ddlselected.DataSource = dt;
                    ddlselected.DataTextField = TextField;
                    ddlselected.DataValueField = ValueField;
                    ddlselected.DataBind();
                }
                if (Index != "")
                {
                    ddlselected.Items.Insert(0, Index);
                    ddlselected.Text = Index;
                }
            }
            catch (Exception ex)
            {
                WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public static void rdb_Binding(RadioButtonList rdbselected, DataTable dt, string TextField, string ValueField)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    rdbselected.DataSource = dt;
                    rdbselected.DataTextField = TextField;
                    rdbselected.DataValueField = ValueField;
                    rdbselected.DataBind();
                }


            }
            catch (Exception ex)
            {
                WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public static string Serialization(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        public static void PreparePrintLable(string strlblVal)
        {
            string strLogFile = @"C:\Barcode Data\";
            try
            {
                if (!Directory.Exists(strLogFile))
                    Directory.CreateDirectory(strLogFile);
                strLogFile = strLogFile + strlblVal + ".txt";
                if (File.Exists(strLogFile)) File.Delete(strLogFile);
                using (var tw = new StreamWriter(strLogFile.Replace('*', '0'), true))
                {
                    tw.WriteLine(strlblVal);
                    tw.Close();
                }
            }
            catch (Exception ex)
            {
                WriteToErrorLog("TTS", "Utilities.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
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