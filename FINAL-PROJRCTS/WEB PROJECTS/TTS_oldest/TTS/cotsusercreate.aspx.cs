using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml;
using System.Text.RegularExpressions;

namespace TTS
{
    public partial class cotsusercreate : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_usermaster"].ToString() == "True")
                        {
                            if (Request["uid"] != null && Request["uid"].ToString() != "")
                            {
                                Bind_CustAssociateLead();
                                Bind_CotsUserType();
                                hdnMethod.Value = Request["uid"].ToString();
                                txtDomesticCustomerName.Style.Add("display", "none");
                                ddlCustomerName.Style.Add("display", "none");
                                ddlUserID.Style.Add("display", "none");
                                txtUserName.Style.Add("display", "none");
                                if (Request["uid"].ToString() == "new")
                                {
                                    lblPageHeading.Text = "User Creation";
                                    txtDomesticCustomerName.Style.Add("display", "block");
                                    txtUserName.Style.Add("display", "block");
                                }
                                else if (Request["uid"].ToString() == "modify")
                                {
                                    lblPageHeading.Text = "User Modification";
                                    ddlCustomerName.Style.Add("display", "block");
                                    ddlUserID.Style.Add("display", "block");
                                    DataTable dtName = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_domestic_fullname", DataAccess.Return_Type.DataTable);
                                    if (dtName.Rows.Count > 0)
                                    {
                                        ddlCustomerName.DataSource = dtName;
                                        ddlCustomerName.DataTextField = "custfullname";
                                        ddlCustomerName.DataValueField = "custcode";
                                        ddlCustomerName.DataBind();
                                        ddlCustomerName.Items.Insert(0, "CHOOSE");
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow11", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "Wrong URL. Please reload this page.";
                            }
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
        private void Bind_CustAssociateLead()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@UserType", "Lead");
                DataTable dtLead = (DataTable)daTTS.ExecuteReader_SP("sp_sel_UserName_UsertypeWise", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlCustLead, dtLead, "PUserName", "PUserName", "CHOOSE");

                sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@UserType", "Manager");
                DataTable dtManager = (DataTable)daTTS.ExecuteReader_SP("sp_sel_UserName_UsertypeWise", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlCustManger, dtManager, "PUserName", "PUserName", "CHOOSE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_CotsUserType()
        {
            try
            {
                bindErrmsg.InnerHtml = "";
                txtCotsUserNewCountry.Text = "India";
                txtCotsCurrency.Text = "INR";
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@country", txtCotsUserNewCountry.Text);
                DataTable dtCity = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cots_city_countrywise", sp1, DataAccess.Return_Type.DataTable);
                if (dtCity.Rows.Count > 0)
                    Utilities.ddl_Binding(ddlCotsUserCity, dtCity, "city", "city", "CHOOSE");
                else
                {
                    ddlCotsUserCity.DataSource = "";
                    ddlCotsUserCity.DataBind();
                    ddlCotsUserCity.Items.Insert(0, "CHOOSE");
                    ddlCotsUserCity.Text = "CHOOSE";
                }
                ddlCotsUserCity.Items.Insert(1, "ADD NEW CITY");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCustomerName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCustomerName")
                {
                    ddlUserID.DataSource = "";
                    ddlUserID.DataBind();
                    ddlCustomerName.SelectedIndex = ddlCustomerName.Items.IndexOf(ddlCustomerName.Items.FindByText(hdnFullName.Value));
                    if (ddlCustomerName.SelectedItem.Text != "CHOOSE")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddlCustomerName.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddlCustomerName.SelectedItem.Value) 
                    };
                        DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                        if (dtUserList.Rows.Count > 0)
                        {
                            ddlUserID.DataSource = dtUserList;
                            ddlUserID.DataTextField = "username";
                            ddlUserID.DataValueField = "ID";
                            ddlUserID.DataBind();
                            if (dtUserList.Rows.Count == 1)
                            {
                                hdnUserId.Value = dtUserList.Rows[0]["username"].ToString();
                                ddlUserID_IndexChange(sender, e);
                            }
                            else
                                ddlUserID.Items.Insert(0, "CHOOSE");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlUserID_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlCustomerName.SelectedIndex = ddlCustomerName.Items.IndexOf(ddlCustomerName.Items.FindByText(hdnFullName.Value));
                ddlUserID.SelectedIndex = ddlUserID.Items.IndexOf(ddlUserID.Items.FindByText(hdnUserId.Value));
                if (ddlUserID.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Custcode", ddlUserID.SelectedItem.Value) };
                    DataTable dtUserData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_userDetails", sp, DataAccess.Return_Type.DataTable);
                    if (dtUserData.Rows.Count > 0)
                    {
                        txtCotsUserNewCountry.Text = dtUserData.Rows[0]["country"].ToString();
                        ddlCotsUserCity.SelectedIndex = ddlCotsUserCity.Items.IndexOf(ddlCotsUserCity.Items.FindByValue(dtUserData.Rows[0]["city"].ToString()));
                        txtPassword.Text = dtUserData.Rows[0]["userpassword"].ToString();
                        txtEmail.Text = dtUserData.Rows[0]["usermail"].ToString();
                        txtCotsCurrency.Text = dtUserData.Rows[0]["usercurrency"].ToString();
                        ddlCustCategory.SelectedIndex = ddlCustCategory.Items.IndexOf(ddlCustCategory.Items.FindByValue(dtUserData.Rows[0]["CustCategory"].ToString()));
                        ddlCustLead.SelectedIndex = ddlCustLead.Items.IndexOf(ddlCustLead.Items.FindByValue(dtUserData.Rows[0]["Lead"].ToString()));
                        ddlCustManger.SelectedIndex = ddlCustManger.Items.IndexOf(ddlCustManger.Items.FindByValue(dtUserData.Rows[0]["Manager"].ToString()));
                        rdbCreditNote.SelectedIndex = rdbCreditNote.Items.IndexOf(rdbCreditNote.Items.FindByValue(dtUserData.Rows[0]["CreditNote"].ToString() == "False" ? "0" : "1"));

                        ScriptManager.RegisterStartupScript(Page, GetType(), "PayMode", "PayMode();", true);

                        txtdaysdom.Text = dtUserData.Rows[0]["Paymentdays"].ToString();
                        txtSalesLimit.Text = dtUserData.Rows[0]["SalesLimit"].ToString();
                        txtPayTerms.Text = Server.HtmlDecode(dtUserData.Rows[0]["PaymentTerms"].ToString().Replace("~", "\r\n"));
                        txtCustInstructions.Text = Server.HtmlDecode(dtUserData.Rows[0]["CustInstruction"].ToString().Replace("~", "\r\n"));
                        rdbCustType.SelectedIndex = rdbCustType.Items.IndexOf(rdbCustType.Items.FindByText(dtUserData.Rows[0]["CustType"].ToString()));
                        ddlRegion.SelectedIndex = ddlRegion.Items.IndexOf(ddlRegion.Items.FindByText(dtUserData.Rows[0]["region"].ToString()));
                        txtPassword.Enabled = false;

                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCotsSave_click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@username", hdnMethod.Value == "new" ? txtUserName.Text : ddlUserID.SelectedItem.Text), 
                    new SqlParameter("@userpassword", txtPassword.Text), 
                    new SqlParameter("@usermail", txtEmail.Text), 
                    new SqlParameter("@usercurrency", txtCotsCurrency.Text.ToUpper()), 
                    new SqlParameter("@appusername", Request.Cookies["TTSUser"].Value), 
                    new SqlParameter("@cotscountry", txtCotsUserNewCountry.Text), 
                    new SqlParameter("@cotscity", ddlCotsUserCity.SelectedItem.Text == "ADD NEW CITY" ? txtCotsUserNewCity.Text : ddlCotsUserCity.SelectedItem.Text), 
                    new SqlParameter("@custcode", "DE0048"), 
                    new SqlParameter("@custfullname", hdnMethod.Value == "new" ? txtDomesticCustomerName.Text.ToUpper() : hdnFullName.Value), 
                    new SqlParameter("@PaymentTerms", Server.HtmlEncode(txtPayTerms.Text.Replace("\r\n", "~"))), 
                    new SqlParameter("@CustInstruction", Server.HtmlEncode(txtCustInstructions.Text.Replace("\r\n", "~"))), 
                    new SqlParameter("@CreditNote", rdbCreditNote.SelectedItem.Text == "Establish" ? false : true), 
                    new SqlParameter("@ExpPayMode", ""), 
                    new SqlParameter("@ExpPayTime", ""), 
                    new SqlParameter("@Paymentdays", Convert.ToInt32(txtdaysdom.Text == "" ? "0" : txtdaysdom.Text)), 
                    new SqlParameter("@region", ddlRegion.SelectedItem.Text), 
                    new SqlParameter("@CustCategory", ddlCustCategory.SelectedItem.Text), 
                    new SqlParameter("@Lead", ddlCustLead.SelectedItem.Text), 
                    new SqlParameter("@Supervisor", "NULL"), 
                    new SqlParameter("@Manager", ddlCustManger.SelectedItem.Text), 
                    new SqlParameter("@CustType", rdbCustType.SelectedItem.Text),
                    new SqlParameter("@SalesLimit", Convert.ToDecimal(txtSalesLimit.Text))
                };

                int resp = daCOTS.ExecuteNonQuery_SP(Request["uid"].ToString() == "new" ? "sp_ins_cots_usermaster" : "sp_edit_cots_usermaster", sp1);
                if (resp > 0)
                {
                    if (Request["uid"].ToString() == "new")
                    {
                        SqlParameter[] spid = new SqlParameter[1];
                        spid[0] = new SqlParameter("@username", txtUserName.Text);
                        string strCustID = (string)daCOTS.ExecuteScalar_SP("sp_sel_usermaster_id", spid);
                        Response.Redirect("cotsdomesticaddress.aspx?qid=2&cid=" + strCustID, false);
                    }
                    else
                        Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}