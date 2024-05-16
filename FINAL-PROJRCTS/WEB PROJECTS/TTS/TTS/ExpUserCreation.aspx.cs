using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
namespace TTS
{
    public partial class ExpUserCreation : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    lblErrMsgcontent.Text = "";
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_usermaster"].ToString() == "True")
                        {
                            hdnType.Value = Request.QueryString["type"].ToString();
                            Bind_DefaultValues();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                }
                else
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_DefaultValues()
        {
            try
            {
                ddl_CustomerSelection.DataSource = null;
                ddl_CustomerSelection.DataBind();
                ddl_CountrySelection.DataSource = null;
                ddl_CountrySelection.DataBind();
                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_CustomerDetails_usercreate", DataAccess.Return_Type.DataSet);
                if (ds.Tables.Count > 0)
                {
                    Utilities.ddl_Binding(ddl_CountrySelection, ds.Tables[1], "country", "country", "CHOOSE");
                    Utilities.ddl_Binding(ddl_LeadSelection, ds.Tables[2], "PUserName", "PUserName", "CHOOSE");
                    Utilities.ddl_Binding(ddl_SupervisorSelection, ds.Tables[3], "PUserName", "PUserName", "CHOOSE");
                    Utilities.ddl_Binding(ddl_ManagerSelection, ds.Tables[4], "PUserName", "PUserName", "CHOOSE");
                    ddl_CountrySelection.Items.Insert(1, "ADD NEW");
                }
                ddl_CategorySelection.Items.Insert(0, "CHOOSE");
                txt_UserID.Style.Add("display", "none");
                ddl_UserIDSelection.Style.Add("display", "none");
                if (hdnType.Value == "new")
                {
                    txt_UserID.Style.Add("display", "block");
                    lblPageHeading.Text = "User Creation";
                    btn_SaveRecord.Text = "Save Record";
                    Utilities.ddl_Binding(ddl_CustomerSelection, ds.Tables[0], "Custname", "CustCode", "CHOOSE");
                }
                else if (hdnType.Value == "modify")
                {
                    ddl_UserIDSelection.Style.Add("display", "block");
                    lblPageHeading.Text = "User Modification";
                    btn_SaveRecord.Text = "Update Record";
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_export_fullname", DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                        Utilities.ddl_Binding(ddl_CustomerSelection, dt, "custfullname", "custcode", "CHOOSE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_CountrySelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_CitySelection.DataSource = null;
                ddl_CitySelection.DataBind();
                ScriptManager.RegisterStartupScript(Page, GetType(), "DisableNewCountryCity", "NewCountryCity('none','country');", true);
                if (ddl_CountrySelection.SelectedValue != "ADD NEW" && ddl_CountrySelection.SelectedValue != "CHOOSE")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@country", ddl_CountrySelection.SelectedItem.Text) };
                    DataTable dtCity = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cots_city_countrywise", sp1, DataAccess.Return_Type.DataTable);
                    if (dtCity.Rows.Count > 0)
                        Utilities.ddl_Binding(ddl_CitySelection, dtCity, "city", "city", "CHOOSE");
                    else
                        ddl_CitySelection.Items.Insert(0, "CHOOSE");
                    ddl_CitySelection.Items.Insert(1, "ADD NEW");
                }
                else if (ddl_CountrySelection.SelectedValue == "ADD NEW")
                {
                    ddl_CitySelection.Items.Insert(0, "ADD NEW");
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EnableNewCountryCity", "NewCountryCity('block','country');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_CitySelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "DisableNewCountryCity", "NewCountryCity('none','city');", true);
                if (ddl_CitySelection.SelectedValue == "ADD NEW")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EnableNewCountryCity", "NewCountryCity('block','city');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdo_PaymentSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                rdo_PayTimeSelection.DataSource = "";
                rdo_PayTimeSelection.DataBind();
                if (rdo_PaymentSelection.SelectedItem.Value != "payagainst")
                {
                    lblNoofDays.Text = "";
                    string strXmlPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"exportcontent.xml");
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(strXmlPath);
                    if (xmlDoc != null)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("text", typeof(string));
                        dt.Columns.Add("val", typeof(string));
                        if (rdo_PaymentSelection.SelectedItem.Value == "advance")
                            lblNoofDays.Text = "Percentage";
                        else if (rdo_PaymentSelection.SelectedItem.Value != "advance")
                            lblNoofDays.Text = "Days";
                        foreach (XmlNode xnode in xmlDoc.SelectNodes("/export/" + rdo_PaymentSelection.SelectedItem.Value))
                            dt.Rows.Add(xnode.Attributes["item"].Value, xnode.Attributes["id"].Value);
                        if (dt.Rows.Count > 0)
                            Utilities.rdb_Binding(rdo_PayTimeSelection, dt, "text", "val");
                    }
                }
                if (ddl_CountrySelection.SelectedValue == "ADD NEW")
                {
                    ddl_CitySelection.Items.Insert(0, "ADD NEW");
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EnableNewCountryCity", "NewCountryCity('block','country');", true);
                }
                if (ddl_CitySelection.SelectedValue == "ADD NEW")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EnableNewCountryCity", "NewCountryCity('block','city');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdo_PayTimeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "DisablePayTimeDays", "document.getElementById('div_PaymentSelection').style.display = 'none';", true);
                txt_NoofDays.Text = rdo_PayTimeSelection.SelectedItem.Value;
                if (txt_NoofDays.Text == "")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EnablePayTimeDays", "document.getElementById('div_PaymentSelection').style.display = 'block';", true);
                if (ddl_CountrySelection.SelectedValue == "ADD NEW")
                {
                    ddl_CitySelection.Items.Insert(0, "ADD NEW");
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EnableNewCountryCity", "NewCountryCity('block','country');", true);
                }
                if (ddl_CitySelection.SelectedValue == "ADD NEW")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EnableNewCountryCity", "NewCountryCity('block','city');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_CustomerSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hdnType.Value == "modify" && ddl_CustomerSelection.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddl_CustomerSelection.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddl_CustomerSelection.SelectedItem.Value) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddl_UserIDSelection.DataSource = dt;
                        ddl_UserIDSelection.DataTextField = "username";
                        ddl_UserIDSelection.DataValueField = "ID";
                        ddl_UserIDSelection.DataBind();
                        if (ddl_UserIDSelection.Items.Count == 1)
                            ddl_UserIDSelection_SelectedIndexChanged(sender, e);
                        else
                            ddl_UserIDSelection.Items.Insert(0, "CHOOSE");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_UserIDSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddl_UserIDSelection.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Custcode", ddl_UserIDSelection.SelectedItem.Value) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_userDetails", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddl_CountrySelection.SelectedIndex = ddl_CountrySelection.Items.IndexOf(ddl_CountrySelection.Items.FindByValue(dt.Rows[0]["country"].ToString()));
                        ddl_CountrySelection_SelectedIndexChanged(sender, e);
                        ddl_CitySelection.SelectedIndex = ddl_CitySelection.Items.IndexOf(ddl_CitySelection.Items.FindByValue(dt.Rows[0]["city"].ToString()));
                        txt_Password.Text = dt.Rows[0]["userpassword"].ToString();
                        txt_EmailID.Text = dt.Rows[0]["usermail"].ToString();
                        txt_Currency.Text = dt.Rows[0]["usercurrency"].ToString();
                        ddl_CategorySelection.SelectedIndex = ddl_CategorySelection.Items.IndexOf(ddl_CategorySelection.Items.FindByValue(dt.Rows[0]["CustCategory"].ToString()));
                        ddl_LeadSelection.SelectedIndex = ddl_LeadSelection.Items.IndexOf(ddl_LeadSelection.Items.FindByValue(dt.Rows[0]["Lead"].ToString()));
                        ddl_ManagerSelection.SelectedIndex = ddl_ManagerSelection.Items.IndexOf(ddl_ManagerSelection.Items.FindByValue(dt.Rows[0]["Manager"].ToString()));
                        ddl_SupervisorSelection.SelectedIndex = ddl_SupervisorSelection.Items.IndexOf(ddl_SupervisorSelection.Items.FindByValue(dt.Rows[0]["Supervisor"].ToString()));
                        rdo_PaymentSelection.SelectedIndex = rdo_PaymentSelection.Items.IndexOf(rdo_PaymentSelection.Items.FindByText(dt.Rows[0]["ExpPayMode"].ToString()));
                        rdo_PaymentSelection_SelectedIndexChanged(sender, e);
                        if (rdo_PaymentSelection.SelectedItem.Value != "payagainst")
                        {
                            rdo_PayTimeSelection.SelectedIndex = rdo_PayTimeSelection.Items.IndexOf(rdo_PayTimeSelection.Items.FindByText(dt.Rows[0]["ExpPayTime"].ToString()));
                            rdo_PayTimeSelection_SelectedIndexChanged(sender, e);
                            txt_NoofDays.Text = dt.Rows[0]["Paymentdays"].ToString();
                        }
                        txt_PaymentTerms.Text = Server.HtmlDecode(dt.Rows[0]["PaymentTerms"].ToString().Replace("~", "\r\n"));
                        txt_Instructions.Text = Server.HtmlDecode(dt.Rows[0]["CustInstruction"].ToString().Replace("~", "\r\n"));
                        txt_Password.Enabled = false;
                        txt_Currency.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_SaveRecord_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@username", hdnType.Value == "modify" ? ddl_UserIDSelection.SelectedItem.Text : txt_UserID.Text), 
                    new SqlParameter("@userpassword", txt_Password.Text), 
                    new SqlParameter("@usermail", txt_EmailID.Text), 
                    new SqlParameter("@usercurrency", txt_Currency.Text.ToUpper()), 
                    new SqlParameter("@appusername", Request.Cookies["TTSUser"].Value), 
                    new SqlParameter("@cotscountry", ddl_CountrySelection.SelectedItem.Text == "ADD NEW" ? txt_NewCountry.Text : ddl_CountrySelection.SelectedItem.Text), 
                    new SqlParameter("@cotscity", ddl_CitySelection.SelectedItem.Text == "ADD NEW" ? txt_NewCity.Text : ddl_CitySelection.SelectedItem.Text), 
                    new SqlParameter("@custcode", ddl_CustomerSelection.SelectedItem.Value), 
                    new SqlParameter("@custfullname", ddl_CustomerSelection.SelectedItem.Text.ToUpper()), 
                    new SqlParameter("@PaymentTerms", Server.HtmlEncode(txt_PaymentTerms.Text.Replace("\r\n", "~"))), 
                    new SqlParameter("@CustInstruction", Server.HtmlEncode(txt_Instructions.Text.Replace("\r\n", "~"))), 
                    new SqlParameter("@CreditNote", false), 
                    new SqlParameter("@ExpPayMode", rdo_PaymentSelection.SelectedItem.Text), 
                    new SqlParameter("@ExpPayTime", rdo_PayTimeSelection.Items.Count > 0 && rdo_PayTimeSelection.SelectedIndex != -1 ? rdo_PayTimeSelection.SelectedItem.Text : ""), 
                    new SqlParameter("@Paymentdays", Convert.ToInt32(txt_NoofDays.Text == "" ? "0" : txt_NoofDays.Text)), 
                    new SqlParameter("@region", "CHOOSE"), 
                    new SqlParameter("@CustCategory", ddl_CategorySelection.SelectedItem.Text), 
                    new SqlParameter("@Lead", ddl_LeadSelection.SelectedItem.Text), 
                    new SqlParameter("@Supervisor", ddl_SupervisorSelection.SelectedItem.Text), 
                    new SqlParameter("@Manager", ddl_ManagerSelection.SelectedItem.Text), 
                    new SqlParameter("@CustType", ""),
                    new SqlParameter("@SalesLimit", Convert.ToDecimal("0.00"))
                };
                int resp = daCOTS.ExecuteNonQuery_SP(hdnType.Value == "new" ? "sp_ins_cots_usermaster" : "sp_edit_cots_usermaster", sp);
                if (resp > 0)
                    Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_ClearRecord_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl.ToString(), false);
        }
    }
}