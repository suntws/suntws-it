using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml;

namespace TTS
{
    public partial class userprivilege : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        DataAccess daChat = new DataAccess(ConfigurationManager.ConnectionStrings["CHATDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["userprivilege"].ToString() == "True")
                        {
                            Get_NewUser_Frame();
                            if (Request["lname"] != null && Request["lname"].ToString() != "")
                            {
                                txtUserName.Text = Request["lname"].ToString();
                                txtUserName.Enabled = false;
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

        private void Get_NewUser_Frame()
        {
            tdNewUser.Style.Add("display", "block");
            tdExistUser.Style.Add("display", "none");
            btnSuspendUser.Style.Add("display", "none");

            rdbExistUserList.DataSource = "";
            rdbExistUserList.DataBind();

            Fill_UserLevel_From_XML();
        }

        protected void rdbUserType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlUserLoginType.SelectedIndex = -1;
                ddlUserChannel.SelectedIndex = -1;
                txtEmail.Text = "";
                txtUserName.Text = "";
                btnSuspendUser.Style.Add("display", "none");
                lblErrMsg.Text = "";
                if (rdbUserType.SelectedItem.Value == "New")
                    Get_NewUser_Frame();
                else if (rdbUserType.SelectedItem.Value == "Exist")
                {
                    tdNewUser.Style.Add("display", "none");
                    tdExistUser.Style.Add("display", "block");

                    DataTable dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_UserList", DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        rdbExistUserList.DataSource = dt;
                        rdbExistUserList.DataTextField = "PUserNameWithDept";
                        rdbExistUserList.DataValueField = "PUserName";
                        rdbExistUserList.DataBind();
                    }

                    Fill_UserLevel_From_XML();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Fill_UserLevel_From_XML()
        {
            try
            {
                string strXmlUserLevelList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"UserLevelMenu.xml");
                XmlDocument xmlLevelList = new XmlDocument();

                xmlLevelList.Load(strXmlUserLevelList);
                if (xmlLevelList != null)
                {
                    DataTable dtList = new DataTable();
                    dtList.Columns.Add("text", typeof(string));
                    dtList.Columns.Add("key", typeof(string));

                    Bind_CheckboxList(chkMasterUserLevel, dtList, xmlLevelList, "MASTER", lblMasterMenu, "masterlevel");
                    Bind_CheckboxList(chkDashboardUserLevel, dtList, xmlLevelList, "DASHBOARD", lblDashboardMenu, "dashboardlevel");
                    Bind_CheckboxList(chkProspectUserLevel, dtList, xmlLevelList, "PROSPECT", lblProspectMenu, "prospectlevel");
                    Bind_CheckboxList(chkScotsDomesticUserLevel, dtList, xmlLevelList, "S-COTS DOMESTIC", lblScotsDomesticMenu, "scotsdomesticlevel");
                    Bind_CheckboxList(chkScotsExportUserLevel, dtList, xmlLevelList, "S-COTS INTERNATIONAL", lblScotsExportMenu, "scotsexportlevel");
                    Bind_CheckboxList(chkClaimUserLevel, dtList, xmlLevelList, "CLAIM", lblClaimMenu, "claimlevel");
                    Bind_CheckboxList(chkOrderTrackingUserLevel, dtList, xmlLevelList, "ORDER TRACKING", lblOrderTrackingMenu, "ordertrackinglevel");
                    Bind_CheckboxList(chkEBidUserLevel, dtList, xmlLevelList, "E-BID", lblEBidMenu, "ebidlevel");
                }

                DataTable dtDept = (DataTable)daTTS.ExecuteReader_SP("sp_sel_UserDepartment", DataAccess.Return_Type.DataTable);
                if (dtDept.Rows.Count > 0)
                {
                    ddlUserDepartment.DataSource = dtDept;
                    ddlUserDepartment.DataTextField = "UserDepartment";
                    ddlUserDepartment.DataValueField = "UserDepartment";
                    ddlUserDepartment.DataBind();
                    ddlUserDepartment.Items.Insert(0, "CHOOSE");
                    ddlUserDepartment.Items.Insert(dtDept.Rows.Count + 1, "ADD NEW DEPT");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_CheckboxList(CheckBoxList chklist, DataTable dt, XmlDocument xmllist, string strHead, Label lbl, string xmlTitle)
        {
            try
            {
                dt.Clear();
                foreach (XmlNode xNode in xmllist.SelectNodes("/userlevel/level[@menuvisible='" + xmlTitle + "']"))
                {
                    dt.Rows.Add(xNode.Attributes["text"].Value, xNode.Attributes["key"].Value);
                }
                if (dt.Rows.Count > 0)
                {
                    lbl.Text = strHead + " MENU LIST";
                    chklist.DataSource = dt;
                    chklist.DataTextField = "text";
                    chklist.DataValueField = "key";
                    chklist.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void rdbExistUserList_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (rdbExistUserList.SelectedItem.Value != "")
                {
                    SqlParameter[] sp2 = new SqlParameter[1];
                    sp2[0] = new SqlParameter("@UserName", rdbExistUserList.SelectedItem.Value);
                    DataTable dt1 = (DataTable)daTTS.ExecuteReader_SP("sp_sel_UserMaster_Type", sp2, DataAccess.Return_Type.DataTable);
                    txtEmail.Text = dt1.Rows[0]["PEmailID"].ToString();
                    ddlUserLoginType.SelectedIndex = ddlUserLoginType.Items.IndexOf(ddlUserLoginType.Items.FindByValue(dt1.Rows[0]["UserType"].ToString()));
                    ddlUserChannel.SelectedIndex = ddlUserChannel.Items.IndexOf(ddlUserChannel.Items.FindByValue(dt1.Rows[0]["UserChannel"].ToString()));
                    ddlUserDepartment.SelectedIndex = ddlUserDepartment.Items.IndexOf(ddlUserDepartment.Items.FindByValue(dt1.Rows[0]["UserDepartment"].ToString()));

                    lblErrMsg.Text = "";
                    btnSuspendUser.Text = "Delete username: " + rdbExistUserList.SelectedItem.Value;
                    btnSuspendUser.Style.Add("display", "block");
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@UserName", rdbExistUserList.SelectedItem.Value);

                    DataTable dtLevel = new DataTable();
                    dtLevel = (DataTable)daTTS.ExecuteReader_SP("sp_sel_userprivilege", sp1, DataAccess.Return_Type.DataTable);
                    if (dtLevel.Rows.Count > 0)
                    {
                        Enable_ExistingMenu_Checkbox(chkMasterUserLevel, dtLevel.Rows[0]);
                        Enable_ExistingMenu_Checkbox(chkDashboardUserLevel, dtLevel.Rows[0]);
                        Enable_ExistingMenu_Checkbox(chkProspectUserLevel, dtLevel.Rows[0]);
                        Enable_ExistingMenu_Checkbox(chkScotsDomesticUserLevel, dtLevel.Rows[0]);
                        Enable_ExistingMenu_Checkbox(chkScotsExportUserLevel, dtLevel.Rows[0]);
                        Enable_ExistingMenu_Checkbox(chkClaimUserLevel, dtLevel.Rows[0]);
                        Enable_ExistingMenu_Checkbox(chkOrderTrackingUserLevel, dtLevel.Rows[0]);
                        Enable_ExistingMenu_Checkbox(chkEBidUserLevel, dtLevel.Rows[0]);
                    }
                    else
                    {
                        Disable_All_CheckBox(chkMasterUserLevel);
                        Disable_All_CheckBox(chkDashboardUserLevel);
                        Disable_All_CheckBox(chkProspectUserLevel);
                        Disable_All_CheckBox(chkScotsDomesticUserLevel);
                        Disable_All_CheckBox(chkScotsExportUserLevel);
                        Disable_All_CheckBox(chkClaimUserLevel);
                        Disable_All_CheckBox(chkOrderTrackingUserLevel);
                        Disable_All_CheckBox(chkEBidUserLevel);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Enable_ExistingMenu_Checkbox(CheckBoxList chklist, DataRow dtRow)
        {
            try
            {
                foreach (ListItem item in chklist.Items)
                {
                    bool boolLevel = false;
                    item.Selected = boolLevel;
                    if (dtRow["" + item.Value + ""] != null && dtRow["" + item.Value + ""].ToString() != "")
                    {
                        boolLevel = Convert.ToBoolean(dtRow["" + item.Value + ""].ToString());
                    }
                    item.Selected = boolLevel;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Disable_All_CheckBox(CheckBoxList chklist)
        {
            foreach (ListItem item in chklist.Items)
            {
                item.Selected = false;
            }
        }

        protected void lnkCreate_click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                if (rdbUserType.SelectedItem.Value == "New")
                {
                    SqlParameter[] spChk = new SqlParameter[1];
                    spChk[0] = new SqlParameter("@PUserName", txtUserName.Text);
                    DataTable dt = new DataTable();
                    dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Chk_PriceUserMaster", spChk, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count == 0)
                    {
                        SqlParameter[] sparam = new SqlParameter[] { 
                            new SqlParameter("@PUserName", txtUserName.Text), 
                            new SqlParameter("@PPassword", txtPassword.Text), 
                            new SqlParameter("@PEmailID", txtEmail.Text), 
                            new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value), 
                            new SqlParameter("@UserType", ddlUserLoginType.SelectedItem.Text), 
                            new SqlParameter("@UserChannel", ddlUserChannel.SelectedItem.Text), 
                            new SqlParameter("@UserDepartment", ddlUserDepartment.SelectedItem.Text != "ADD NEW DEPT" ? ddlUserDepartment.SelectedItem.Text : txtUserDepartment.Text) 
                        };
                        daTTS.ExecuteNonQuery_SP("Sp_Ins_PriceUserMaster", sparam);

                        daTTS.ExecuteNonQuery_SP("sp_update_userlevel", Build_UserLevel_Parameter(txtUserName.Text));

                        SqlParameter[] spChatUser = new SqlParameter[1];
                        spChatUser[0] = new SqlParameter("@username", txtUserName.Text);
                        daChat.ExecuteNonQuery_SP("sp_ins_ttsuserlist", spChatUser);

                        rdbUserType_IndexChange(sender, e);
                        lblErrMsg.Text = "Username created successfully";
                        lblErrMsg.Style.Add("color", "green");
                    }
                    else
                    {
                        lblErrMsg.Text = "Username already exists";
                    }
                }
                else if (rdbUserType.SelectedItem.Value == "Exist")
                {
                    daTTS.ExecuteNonQuery_SP("sp_update_userlevel", Build_UserLevel_Parameter(rdbExistUserList.SelectedItem.Value));

                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@PUserName", rdbExistUserList.SelectedItem.Value), 
                        new SqlParameter("@PEmailID", txtEmail.Text), 
                        new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value), 
                        new SqlParameter("@UserType", ddlUserLoginType.SelectedItem.Text), 
                        new SqlParameter("@UserChannel", ddlUserChannel.SelectedItem.Text), 
                        new SqlParameter("@UserDepartment", ddlUserDepartment.SelectedItem.Text != "ADD NEW DEPT" ? ddlUserDepartment.SelectedItem.Text : txtUserDepartment.Text) 
                    };
                    daTTS.ExecuteNonQuery_SP("sp_update_UserMaster_Type", sp1);

                    rdbUserType_IndexChange(sender, e);
                    lblErrMsg.Text = "Username updated successfully";
                    lblErrMsg.Style.Add("color", "green");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private SqlParameter[] Build_UserLevel_Parameter(string strUserName)
        {
            string strXmlUserLevelList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"UserLevelMenu.xml");
            XmlDocument xmlLevelList = new XmlDocument();
            xmlLevelList.Load(strXmlUserLevelList);
            int colCount = xmlLevelList.SelectNodes("/userlevel/level").Count;

            string strXmlNotUserLevelList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"notuserlevel.xml");
            XmlDocument xmlNotLevelList = new XmlDocument();
            xmlNotLevelList.Load(strXmlNotUserLevelList);
            int colNotCount = xmlNotLevelList.SelectNodes("/notuserlevel/level").Count;
            SqlParameter[] param = new SqlParameter[colCount + colNotCount + 2];
            try
            {
                param[0] = new SqlParameter("@username", strUserName);
                param[1] = new SqlParameter("@modifieduser", Request.Cookies["TTSUser"].Value);
                int j = 2;
                foreach (ListItem item in chkMasterUserLevel.Items)
                {
                    param[j] = new SqlParameter("" + item.Value + "", item.Selected);
                    j++;
                }
                foreach (ListItem item in chkDashboardUserLevel.Items)
                {
                    param[j] = new SqlParameter("" + item.Value + "", item.Selected);
                    j++;
                }
                foreach (ListItem item in chkProspectUserLevel.Items)
                {
                    param[j] = new SqlParameter("" + item.Value + "", item.Selected);
                    j++;
                }
                foreach (ListItem item in chkScotsDomesticUserLevel.Items)
                {
                    param[j] = new SqlParameter("" + item.Value + "", item.Selected);
                    j++;
                }
                foreach (ListItem item in chkScotsExportUserLevel.Items)
                {
                    param[j] = new SqlParameter("" + item.Value + "", item.Selected);
                    j++;
                }
                foreach (ListItem item in chkClaimUserLevel.Items)
                {
                    param[j] = new SqlParameter("" + item.Value + "", item.Selected);
                    j++;
                }
                foreach (ListItem item in chkOrderTrackingUserLevel.Items)
                {
                    param[j] = new SqlParameter("" + item.Value + "", item.Selected);
                    j++;
                }
                foreach (ListItem item in chkEBidUserLevel.Items)
                {
                    param[j] = new SqlParameter("" + item.Value + "", item.Selected);
                    j++;
                }
                foreach (XmlNode xNode in xmlNotLevelList.SelectNodes("/notuserlevel/level"))
                {
                    param[j] = new SqlParameter("" + xNode.Attributes["key"].Value + "", false);
                    j++;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return param;
        }

        protected void btnSuspendUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdbUserType.SelectedItem.Value == "Exist")
                {
                    string strUseName = rdbExistUserList.SelectedItem.Value;
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@UserName", rdbExistUserList.SelectedItem.Value);
                    daTTS.ExecuteNonQuery_SP("sp_suspend_TTS_User", param);

                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@UserName", rdbExistUserList.SelectedItem.Value);
                    daPORT.ExecuteNonQuery_SP("sp_del_LeadNames", sp1);

                    rdbUserType_IndexChange(sender, e);
                    lblErrMsg.Text = "Username " + strUseName + " deleted successfully";
                    lblErrMsg.Style.Add("color", "green");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnPageClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("userprivilege.aspx", false);
        }

        protected void chkScotsDomesticUserLevel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}