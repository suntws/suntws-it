using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Data;

namespace GT
{
    static class Program
    {
        public static string strUserName;
        public static string strCurrPass;
        public static string strPlantName;
        public static string strLocalDbPath;
        public static string strLocalDbPass;
        public static string strServerDbPath1;
        public static string strServerDbPath2;
        public static string strServerDbPass;
        public static bool boolLoadUnloadDelayApproval;
        public static bool boolInspectDelayApproval;
        public static bool boolManualWeighing;
        public static DataTable dt_Wt_Com;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            Mutex m = new Mutex(true, "myApp", out createdNew);
            if (!createdNew)
                MessageBox.Show("GT APPLICATION IS ALREADY RUNNING!", "Multiple Instances");
            else
            {
                strPlantName = GetPlantName();
                strLocalDbPath = GetConVal(1);
                strLocalDbPass = GetConVal(2);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmLogin());
            }
        }
        public static void WriteToGtErrorLog(string strAppName, string strModuleName, string strFunctionName, int intErrorid, string strErrorMsg)
        {
            try
            {
                DBAccess dba = new DBAccess();
                SqlParameter[] param = new SqlParameter[] { 
                    new SqlParameter("@ProjectName", strAppName), 
                    new SqlParameter("@ModuleName", strModuleName), 
                    new SqlParameter("@MethodName", strFunctionName), 
                    new SqlParameter("@EID", intErrorid), 
                    new SqlParameter("@ErrorMsg", strErrorMsg), 
                    new SqlParameter("@UserName", strUserName) 
                };
                int status = dba.ExecuteNonQuery_SP("sp_GtErrorDetails", param);
            }
            catch (Exception ex)
            {
                WriteToGtErrorLog("GT", "Progrma.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public static void digitonly(KeyPressEventArgs e)
        {
            try
            {
                if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
                {
                    e.Handled = true;
                    MessageBox.Show("Enter only digit and decimal point.", "Alert!");
                }
            }
            catch { }
        }
        public static void PreparePrintLable(string strlblVal)
        {
            string strLogFile = @"C:\\Barcode Data\\";
            try
            {
                //                ^XA
                //^FX Third section with barcode.
                //^BY5,2,200
                //^FO50,250
                //^BC
                //^FDP-192168S200100062^FS
                //^XZ

                //if (Environment.Is64BitOperatingSystem || Environment.Is64BitProcess)
                //{
                strLogFile = @"C:\\Barcode Data\\";
                if (!Directory.Exists(strLogFile))
                    Directory.CreateDirectory(strLogFile);
                strLogFile = strLogFile + strlblVal + ".txt";
                StreamWriter SWrtiter = System.IO.File.AppendText(strLogFile);
                SWrtiter.WriteLine(strlblVal);
                SWrtiter.Close();
                //}
                //else
                //{
                //    strLogFile = @"C:\\Barone\\jobs\\";
                //    if (!Directory.Exists(strLogFile))
                //        Directory.CreateDirectory(strLogFile);
                //    strLogFile = strLogFile + strlblVal + ".pj";
                //    StreamWriter SWrtiter = System.IO.File.AppendText(strLogFile);
                //    SWrtiter.WriteLine(@"C:\Barone\formats\tyres.lbl" + System.Environment.NewLine + "1" + System.Environment.NewLine + strlblVal.Trim());
                //    SWrtiter.Close();
                //}
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private static string GetPlantName()
        {
            string strPlant = "";
            try
            {
                var fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\maplog.dll", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
                {
                    strPlant = streamReader.ReadToEnd();
                    //string[] strSplit = strPlant.Split('\r\n');
                    strPlant = EncryptDecrypt.Decrypt(strPlant, "STOCKMERGE");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return strPlant;
        }
        private static string GetConVal(int intPosition)
        {
            string strReturn = "";
            try
            {
                int counter = 0;
                string line;
                var fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "\\conlog.dll", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8))
                {
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        strReturn = EncryptDecrypt.Decrypt(line, "STOCKMERGE");
                        counter++;
                        if (counter == intPosition)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return strReturn;
        }
    }
}
