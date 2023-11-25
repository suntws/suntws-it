using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Configuration;
using System.Web.Services;
using System.IO;

namespace TTS
{
    public partial class _s3Entry : System.Web.UI.Page
    {
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["s3_network"].ToString() == "True")
                        {
                            if (Request["qKey"] != null)
                            {
                                Bind_DropDownList();
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptAddState", "makedivdisplay('" + Request["qKey"].ToString() + "');", true);
                                if (Request["qKey"].ToString() != "0")
                                    Bind_ExcistingCustList(Request["qKey"].ToString());
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
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_ExcistingCustList(string custID)
        {
            try
            {
                hdnCustId.Value = custID;
                hdnCustType.Value = Request.Cookies["TTSUser"].Value.ToLower();

                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@ID", custID);
                DataTable dt = (DataTable)daPORT.ExecuteReader_SP("sp_sel_s3CustomerList", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    rpt3sCustDetails.DataSource = dt;
                    rpt3sCustDetails.DataBind();

                    txt3sPrevCategory.Text = dt.Rows[0]["CategoryName"].ToString() == "" ? "Not yet identified" : dt.Rows[0]["CategoryName"].ToString();
                    hdnCategoryID.Value = dt.Rows[0]["CategoryID"].ToString() == "" ? "11" : dt.Rows[0]["CategoryID"].ToString();
                    hdnCustName.Value = dt.Rows[0]["CustName"].ToString();

                    SqlParameter[] spEnt = new SqlParameter[1];
                    spEnt[0] = new SqlParameter("@Method", "Entry");
                    DataTable dtEnt = (DataTable)daPORT.ExecuteReader_SP("sp_sel_s3CategoryMaster", spEnt, DataAccess.Return_Type.DataTable);

                    ddl3sChangedCategory.DataSource = dtEnt;
                    ddl3sChangedCategory.DataValueField = "ID";
                    ddl3sChangedCategory.DataTextField = "CategoryName";
                    ddl3sChangedCategory.DataBind();

                    ddl3sChangedCategory.Items.Add("Choose");
                    ddl3sChangedCategory.Text = "Choose";

                    ListItem selectedCategory = ddl3sChangedCategory.Items.FindByValue(dt.Rows[0]["CategoryID"].ToString());
                    if (selectedCategory != null)
                    {
                        ddl3sChangedCategory.Items.FindByText("Choose").Selected = false;
                        selectedCategory.Selected = true;
                    }

                    SqlParameter[] spStatus = new SqlParameter[1];
                    spStatus[0] = new SqlParameter("@Method", "Category");
                    DataTable dtStatus = (DataTable)daPORT.ExecuteReader_SP("sp_sel_s3CategoryMaster", spStatus, DataAccess.Return_Type.DataTable);

                    ddl3sStatus.DataSource = dtStatus;
                    ddl3sStatus.DataValueField = "ID";
                    ddl3sStatus.DataTextField = "CategoryName";
                    ddl3sStatus.DataBind();

                    ddl3sStatus.Items.Add("Choose");
                    ddl3sStatus.Text = "Choose";

                    ListItem selectedListItem = ddl3sStatus.Items.FindByValue(dt.Rows[0]["TierID"].ToString());
                    if (selectedListItem != null)
                    {
                        ddl3sStatus.Items.FindByText("Choose").Selected = false;
                        selectedListItem.Selected = true;
                    }

                    SqlParameter[] sp2 = new SqlParameter[1];
                    sp2[0] = new SqlParameter("@ID", custID);
                    DataTable dtHistory = (DataTable)daPORT.ExecuteReader_SP("sp_sel_s3ActivateChangedList_CustWise", sp2, DataAccess.Return_Type.DataTable);

                    gv_3sChangeHistory.DataSource = dtHistory;
                    gv_3sChangeHistory.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_DropDownList()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@Method", "Entry");
                DataTable dtCategory = (DataTable)daPORT.ExecuteReader_SP("sp_sel_s3CategoryMaster", sp1, DataAccess.Return_Type.DataTable);
                ddl3sCategory.DataSource = dtCategory;
                ddl3sCategory.DataValueField = "ID";
                ddl3sCategory.DataTextField = "CategoryName";
                ddl3sCategory.DataBind();

                ddl3sCategory.Items.Add("Choose");
                ddl3sCategory.Text = "Choose";

                DataTable dtState = (DataTable)daPORT.ExecuteReader_SP("sp_sel_state_s3CustomerList", DataAccess.Return_Type.DataTable);
                ddl3sState.DataSource = dtState;
                ddl3sState.DataValueField = "StateName";
                ddl3sState.DataTextField = "StateName";
                ddl3sState.DataBind();

                ddl3sState.Items.Add("Add New State");
                ddl3sState.Items.Add("Choose");
                ddl3sState.Text = "Choose";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddl3sState_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                hdnStateName.Value = ddl3sState.SelectedItem.Text;
                if (ddl3sState.SelectedItem.Text != "Add New State")
                {
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@StateName", ddl3sState.SelectedItem.Text);
                    DataTable dtCity = (DataTable)daPORT.ExecuteReader_SP("sp_sel_city_s3CustomerList", sp1, DataAccess.Return_Type.DataTable);

                    ddl3sCity.DataSource = dtCity;
                    ddl3sCity.DataValueField = "City";
                    ddl3sCity.DataTextField = "City";
                    ddl3sCity.DataBind();

                    ddl3sCity.Items.Add("Add New City");
                    ddl3sCity.Items.Add("Choose");
                    ddl3sCity.Text = "Choose";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptAddState", "AddNew3sState();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddl3sCity_IndexChange(object sender, EventArgs e)
        {
            hdnCityName.Value = ddl3sCity.SelectedItem.Text;
            ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptAddCity", "AddNew3sCity();", true);
        }

        protected void btn3sSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[7];

                sp1[0] = new SqlParameter("@CustName", txt3sName.Text);
                sp1[1] = new SqlParameter("@StateName", hdnStateName.Value == "Add New State" ? txt3sState.Text : ddl3sState.SelectedItem.Text);
                sp1[2] = new SqlParameter("@City", hdnCityName.Value == "Add New City" ? txt3sCity.Text : ddl3sCity.SelectedItem.Text);
                sp1[3] = new SqlParameter("@Zone", txt3sZone.Text);
                sp1[4] = new SqlParameter("@CategoryID", ddl3sCategory.SelectedItem.Value);
                sp1[5] = new SqlParameter("@Comments", txt3sComments.Text.Replace("\r\n", "~"));
                sp1[6] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                int resp = daPORT.ExecuteNonQuery_SP("sp_ins_s3CustomerList", sp1);

                if (resp > 0)
                    Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btn3sChange_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@CustID", hdnCustId.Value);
                sp1[1] = new SqlParameter("@FromCategoryID", hdnCategoryID.Value);
                sp1[2] = new SqlParameter("@ToCategoryID", ddl3sChangedCategory.SelectedItem.Value);
                sp1[3] = new SqlParameter("@Comments", txt3sChangedComments.Text.Replace("\r\n", "~"));
                sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp1[5] = new SqlParameter("@StatusID", ddl3sStatus.SelectedItem.Text != "Choose" ? ddl3sStatus.SelectedItem.Value : "0");

                daPORT.ExecuteNonQuery_SP("sp_ins_s3ActivateChangedList", sp1);
                Response.Redirect("s3dashboard.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public string Bind_Feedback(string strComments, string strUsername)
        {
            string returnVal = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(strComments))
                {
                    returnVal += "<div style='font-weight: normal; line-height: 20px;'>" + strComments + "<br /><i>Updated by : </i><b>" + strUsername + "</b></div>";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnVal;
        }
    }
}