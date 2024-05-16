using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class typeupgrade : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["type_masterdata"].ToString() == "True")
                        {
                            btnPosition.CssClass = "Clicked";
                            btnSubsti.CssClass = "Initial";
                            MultiView1.ActiveViewIndex = 0;
                            Tab_Click(null, null);
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
        protected void Tab_Click(object sender, EventArgs e)
        {
            try
            {
                btnPosition.CssClass = "Initial";
                btnSubsti.CssClass = "Initial";
                Button lnkTxt = sender as Button;
                if (sender == null || lnkTxt.Text == "TYPE POSITION")
                {
                    rdbPositionCategory.SelectedIndex = -1;

                    lstType.Items.Clear();
                    lstType.DataSource = null;
                    lstType.DataBind();

                    lstTypePosition.Items.Clear();
                    lstTypePosition.DataSource = null;
                    lstTypePosition.DataBind();

                    btnPosition.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 0;
                }
                else if (lnkTxt.Text == "TYPE SUBSTITUTION")
                {
                    rdbSubstiCategory.SelectedIndex = -1;

                    ddl_MasterType.Items.Clear();
                    ddl_MasterType.DataSource = null;
                    ddl_MasterType.DataBind();

                    list1.Items.Clear();
                    list1.DataSource = null;
                    list1.DataBind();

                    list2.Items.Clear();
                    list2.DataSource = null;
                    list2.DataBind();

                    btnSubsti.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdbPositionCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@SizeCategory", rdbPositionCategory.SelectedItem.Value) };
                DataSet ds = (DataSet)daTTS.ExecuteReader_SP("sp_sel_ProcessID_Type", sp, DataAccess.Return_Type.DataSet);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lstTypePosition.DataSource = ds.Tables[0];
                    lstTypePosition.DataTextField = "TyreType";
                    lstTypePosition.DataValueField = "TypePosition";
                    lstTypePosition.DataBind();
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    lstType.DataSource = ds.Tables[1];
                    lstType.DataTextField = "TyreType";
                    lstType.DataValueField = "TypePosition";
                    lstType.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPositionSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnTypePosition.Value != "")
                {
                    DataTable dtType = new DataTable();
                    dtType.Columns.Add(new DataColumn("RowNo", typeof(System.Int16)));
                    dtType.Columns.Add(new DataColumn("TyreType", typeof(System.String)));

                    string[] strSplit = hdnTypePosition.Value.Split(',');
                    int intRowNo = 1;
                    for (int itemCount = 0; itemCount < strSplit.Length; itemCount++)
                    {
                        dtType.Rows.Add(intRowNo++, strSplit[itemCount].ToString());
                    }

                    SqlParameter[] spIns = new SqlParameter[] { new SqlParameter("@ProcessID_Type_DT", dtType), new SqlParameter("@SizeCategory", rdbPositionCategory.SelectedValue) };
                    daTTS.ExecuteNonQuery_SP("sp_Ins_ProcessID_TypePosition", spIns);
                }
                rdbPositionCategory_IndexChange(null, null);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdbSubstiCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                list1.Items.Clear();
                list1.DataSource = null;
                list1.DataBind();

                list2.Items.Clear();
                list2.DataSource = null;
                list2.DataBind();

                rdb_StockMethod.SelectedIndex = -1;

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@SizeCategory", rdbSubstiCategory.SelectedItem.Value) };
                DataSet ds = (DataSet)daTTS.ExecuteReader_SP("sp_sel_ProcessID_Type", sp, DataAccess.Return_Type.DataSet);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    ddl_MasterType.DataSource = ds.Tables[0];
                    ddl_MasterType.DataTextField = "TyreType";
                    ddl_MasterType.DataValueField = "TypePosition";
                    ddl_MasterType.DataBind();
                    ddl_MasterType.Items.Insert(0, "CHOOSE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdb_StockMethod_IndexChange(object sender, EventArgs e)
        {
            try
            {
                list1.Items.Clear();
                list1.DataSource = null;
                list1.DataBind();

                list2.Items.Clear();
                list2.DataSource = null;
                list2.DataBind();

                if (ddl_MasterType.Items.Count > 0)
                    ddl_MasterType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_MasterType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                list1.Items.Clear();
                list1.DataSource = null;
                list1.DataBind();

                list2.Items.Clear();
                list2.DataSource = null;
                list2.DataBind();

                DataTable dtType = new DataTable();
                dtType.Columns.Add(new DataColumn("TypePosition", typeof(System.Int16)));
                dtType.Columns.Add(new DataColumn("UpgradeType", typeof(System.String)));

                SqlParameter[] spSel = new SqlParameter[] { 
                    new SqlParameter("@MasterType", ddl_MasterType.SelectedItem.Text), 
                    new SqlParameter("@Category", rdbSubstiCategory.SelectedItem.Value),
                    new SqlParameter("@StockMethod", rdb_StockMethod.SelectedValue)
                };
                DataTable dtExists = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_TypeWise_UpgradeList", spSel, DataAccess.Return_Type.DataTable);

                int itemCount = 1;
                for (int ddlIndx = ddl_MasterType.SelectedIndex + 1; ddlIndx < ddl_MasterType.Items.Count; ddlIndx++)
                {
                    bool boolUpgradeList = false;
                    if (dtExists.Rows.Count > 0)
                    {
                        foreach (DataRow dRow in dtExists.Select("UpgradeType='" + ddl_MasterType.Items[ddlIndx].Text + "'"))
                        {
                            boolUpgradeList = true;
                        }
                    }
                    if (!boolUpgradeList)
                        dtType.Rows.Add(itemCount, ddl_MasterType.Items[ddlIndx].Text);
                }

                if (dtType.Rows.Count > 0)
                {
                    list1.DataSource = dtType;
                    list1.DataTextField = "UpgradeType";
                    list1.DataValueField = "TypePosition";
                    list1.DataBind();
                }

                if (dtExists.Rows.Count > 0)
                {
                    list2.DataSource = dtExists;
                    list2.DataTextField = "UpgradeType";
                    list2.DataValueField = "TypePosition";
                    list2.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnUpgradeSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnTypePosition.Value != "")
                {
                    DataTable dtType = new DataTable();
                    dtType.Columns.Add(new DataColumn("TypePosition", typeof(System.Int16)));
                    dtType.Columns.Add(new DataColumn("UpgradeType", typeof(System.String)));

                    string[] strSplit = hdnTypePosition.Value.Split(',');
                    int intRowNo = 1;
                    for (int itemCount = 0; itemCount < strSplit.Length; itemCount++)
                    {
                        dtType.Rows.Add(intRowNo++, strSplit[itemCount].ToString());
                    }

                    SqlParameter[] spIns = new SqlParameter[] { 
                        new SqlParameter("@Type_Upgrade_DT", dtType), 
                        new SqlParameter("@MasterType", ddl_MasterType.SelectedItem.Text), 
                        new SqlParameter("@Category", rdbSubstiCategory.SelectedValue), 
                        new SqlParameter("@StockMethod", rdb_StockMethod.SelectedValue),
                        new SqlParameter("@Username", Request.Cookies["TTSUser"].Value) 
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_Ins_TypeWise_UpgradeList", spIns);

                    list1.Items.Clear();
                    list1.DataSource = null;
                    list1.DataBind();

                    list2.Items.Clear();
                    list2.DataSource = null;
                    list2.DataBind();

                    if (ddl_MasterType.SelectedIndex == ddl_MasterType.Items.Count - 1)
                        ddl_MasterType.SelectedIndex = 1;
                    else
                        ddl_MasterType.SelectedIndex = ddl_MasterType.SelectedIndex + 1;

                    ddl_MasterType_IndexChange(null, null);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}