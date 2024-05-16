using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;

namespace TTS
{
    public partial class login : System.Web.UI.Page
    {
        DataAccess daUser = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = ConfigurationManager.AppSettings["pagetitle"];
            try
            {
                if (!IsPostBack)
                {
                    if (Request["ReturnUrl"] != null && Request["ReturnUrl"].Contains(".aspx") == true)
                        hdnReturnUrl.Value = Request["ReturnUrl"].ToString();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkLogin_click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sparam = new SqlParameter[2];
                sparam[0] = new SqlParameter("@PUserName", txtUserName.Text);
                sparam[1] = new SqlParameter("@PPassword", txtPassword.Text);
                DataTable dt = (DataTable)daUser.ExecuteReader_SP("Sp_Chk_Login", sparam, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count == 1)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@UserName", txtUserName.Text);
                    daUser.ExecuteNonQuery_SP("sp_update_lastlogin", param);

                    HttpCookie aCookie = new HttpCookie("TTSUser");
                    aCookie.Value = dt.Rows[0]["PUserName"].ToString();
                    aCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(aCookie);

                    aCookie = new HttpCookie("TTSUserDepartment");
                    aCookie.Value = dt.Rows[0]["UserDepartment"].ToString();
                    aCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(aCookie);

                    aCookie = new HttpCookie("TTSUserType");
                    aCookie.Value = dt.Rows[0]["UserType"].ToString();
                    aCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(aCookie);

                    aCookie = new HttpCookie("TTSUserEmail");
                    aCookie.Value = dt.Rows[0]["PEmailID"].ToString();
                    aCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(aCookie);

                    aCookie = new HttpCookie("TTSLastLogin");
                    aCookie.Value = dt.Rows[0]["LastLogin"].ToString();
                    aCookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(aCookie);

                    FormsAuthentication.SetAuthCookie(HttpUtility.HtmlEncode(txtUserName.Text), false);
                    Session.Add("UserName", Request.Cookies["TTSUser"].Value);
                    if (Session["UserName"].ToString() != "")
                        Ins_LoginHistory();

                    SqlParameter[] spUserLevel = new SqlParameter[] { new SqlParameter("@username", Request.Cookies["TTSUser"].Value) };
                    DataTable dtuserlevel = (DataTable)daUser.ExecuteReader_SP("sp_sel_userprivilege", spUserLevel, DataAccess.Return_Type.DataTable);
                    Session["dtuserlevel"] = dtuserlevel;
                    if (hdnReturnUrl.Value != "")
                        Response.RedirectPermanent(hdnReturnUrl.Value, false);
                    else
                        Response.RedirectPermanent("default.aspx", false);
                }
                else
                {
                    errmsg.InnerText = "Invalid username and password";
                    errmsg.Style.Add("color", "red");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Ins_LoginHistory()
        {
            try
            {
                SqlParameter[] param = new SqlParameter[] { new SqlParameter("@UserName", Session["UserName"].ToString()) };
                daUser.ExecuteNonQuery_SP("sp_update_login_SessionStart", param);

                string ipaddress = "", macAddress = "", deviceName = "", netType = "", usermachine = "";
                try
                {
                    ipaddress = Request.ServerVariables["REMOTE_ADDR"];
                    foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (nic.OperationalStatus == OperationalStatus.Up && nic.GetPhysicalAddress().ToString() != "")
                        {
                            byte[] bytes = nic.GetPhysicalAddress().GetAddressBytes();
                            for (int i = 0; i < bytes.Length; i++)
                            {
                                macAddress = macAddress + bytes[i].ToString("X2");
                                if (i != bytes.Length - 1)
                                    macAddress = macAddress + ("-");
                            }
                            deviceName = nic.Description;
                            netType = nic.Name;
                        }
                    }
                    usermachine = Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName;
                }
                catch (Exception)
                {
                }

                SqlParameter[] spHis = new SqlParameter[] { 
                    new SqlParameter("@UserName", Session["UserName"].ToString()), 
                    new SqlParameter("@MachineName", Request.UserHostName),
                    new SqlParameter("@Browser", Request.Browser.Browser), 
                    new SqlParameter("@BrowserVersion", Request.Browser.Version),
                    new SqlParameter("@CurrentUserMachine", usermachine),
                    new SqlParameter("@ipAddress", ipaddress),
                    new SqlParameter("@macAddress", macAddress),
                    new SqlParameter("@deviceName", deviceName),
                    new SqlParameter("@netType", netType)
                };
                daUser.ExecuteNonQuery_SP("sp_ins_UserLoginHistory", spHis);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), "sp_ins_UserLoginHistory", 1, ex.Message);
            }
        }
    }
}