using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TTS
{
    public partial class roomreservemaster : System.Web.UI.Page
    {
        DataAccess daTTSDB = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daErrDB = new DataAccess(ConfigurationManager.ConnectionStrings["ErrDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        if (Request.Cookies["TTSUser"].Value.ToLower() == "admin" || Request.Cookies["TTSUser"].Value.ToLower() == "somu" ||
                            Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                        {
                            DataTable dtUser = (DataTable)daTTSDB.ExecuteReader("select upper(username) as username from userlevel where roomreserve=1", DataAccess.Return_Type.DataTable);
                            rdbPrivilegeUserList.DataSource = dtUser;
                            rdbPrivilegeUserList.DataTextField = "UserName";
                            rdbPrivilegeUserList.DataValueField = "UserName";
                            rdbPrivilegeUserList.DataBind();

                            DataTable dtRoom = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_RoomNameList", DataAccess.Return_Type.DataTable);
                            chkRoomList.DataSource = dtRoom;
                            chkRoomList.DataTextField = "RoomList";
                            chkRoomList.DataValueField = "RoomID";
                            chkRoomList.DataBind();
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
                Utilities.WriteToErrorLog("ROOM_RESERVE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void rdbPrivilegeUserList_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (rdbPrivilegeUserList.SelectedItem.Text != "")
                {
                    foreach (ListItem item in chkRoomList.Items)
                    {
                        item.Selected = false;
                    }
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@BookUser", rdbPrivilegeUserList.SelectedItem.Text);
                    DataTable dt = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_RoomPrivilege", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        string str1 = dt.Rows[0]["RoomIDList"].ToString();
                        string[] strIdList = str1.Split(',');
                        foreach (ListItem item in chkRoomList.Items)
                        {
                            item.Selected = false;
                            foreach (string str in strIdList)
                            {
                                if (item.Value == str)
                                    item.Selected = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ROOM_RESERVE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnRoomSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strConcat = string.Empty;
                foreach (ListItem item in chkRoomList.Items)
                {
                    if (item.Selected)
                    {
                        if (strConcat.Length > 0)
                            strConcat += "," + item.Value;
                        else
                            strConcat = item.Value;
                    }
                }
                if (strConcat.Length > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@BookUser", rdbPrivilegeUserList.SelectedItem.Text);
                    sp1[1] = new SqlParameter("@RoomIDList", strConcat);
                    sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                    int resp = daErrDB.ExecuteNonQuery_SP("sp_ins_RoomPrivilege", sp1);
                    if (resp > 0)
                        Response.Redirect("roomreservemaster.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ROOM_RESERVE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}