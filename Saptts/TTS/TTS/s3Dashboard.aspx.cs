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
    public partial class s3Dashboard : System.Web.UI.Page
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
                            Bind_AllDropDownList();
                            btn3sListShow_Click(sender, e);
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

        private void Bind_AllDropDownList()
        {
            try
            {
                DataTable dtZone = (DataTable)daPORT.ExecuteReader_SP("sp_sel_Zone_s3CustomerList", DataAccess.Return_Type.DataTable);
                ddl3sZone.DataSource = dtZone;
                ddl3sZone.DataTextField = "Zone";
                ddl3sZone.DataValueField = "Zone";
                ddl3sZone.DataBind();

                ddl3sZone.Items.Add("ALL");
                ddl3sZone.Text = "ALL";

                DataTable dtState = (DataTable)daPORT.ExecuteReader_SP("sp_sel_state_s3CustomerList", DataAccess.Return_Type.DataTable);
                ddl3sState.DataSource = dtState;
                ddl3sState.DataTextField = "StateName";
                ddl3sState.DataValueField = "StateName";
                ddl3sState.DataBind();

                ddl3sState.Items.Add("ALL");
                ddl3sState.Text = "ALL";

                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@Method", "Category");
                DataTable dtCategory = (DataTable)daPORT.ExecuteReader_SP("sp_sel_s3CategoryMaster", sp1, DataAccess.Return_Type.DataTable);
                ddl3sCategory.DataSource = dtCategory;
                ddl3sCategory.DataValueField = "ID";
                ddl3sCategory.DataTextField = "CategoryName";
                ddl3sCategory.DataBind();

                ddl3sCategory.Items.Add("ALL");
                ddl3sCategory.Text = "ALL";

                DataTable dt3sChangeList = (DataTable)daPORT.ExecuteReader_SP("sp_sel_All_s3ActivateChangedList_toprecords", DataAccess.Return_Type.DataTable);
                ViewState["dt3sChangeList"] = dt3sChangeList;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btn3sListShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddl3sZone.Items.Count > 0 && ddl3sState.Items.Count > 0 && ddl3sCategory.Items.Count > 0)
                {
                    string strZoneName = ddl3sZone.SelectedItem.Text == "ALL" ? "" : " and a.Zone ='" + ddl3sZone.SelectedItem.Text + "'";

                    DataTable dt3sList = new DataTable();
                    if (ddl3sState.Text == "ALL" && ddl3sCategory.Text == "ALL")
                        dt3sList = (DataTable)daPORT.ExecuteReader("select a.ID,a.StateName,a.City,a.Zone,a.CustName,a.Comments,a.UserName from s3CustomerList a where a.CustStatus=1 " + strZoneName + " order by Zone,StateName,City,CustName", DataAccess.Return_Type.DataTable);
                    else if (ddl3sState.Text == "ALL" && ddl3sCategory.Text != "ALL")
                        dt3sList = (DataTable)daPORT.ExecuteReader("select a.ID,a.StateName,a.City,a.Zone,a.CustName,a.Comments,a.UserName from s3CustomerList a where a.CustStatus=1 " + strZoneName + " and a.CategoryID='" + ddl3sCategory.SelectedItem.Value + "' order by Zone,StateName,City,CustName", DataAccess.Return_Type.DataTable);
                    else if (ddl3sState.Text != "ALL" && ddl3sCategory.Text == "ALL")
                        dt3sList = (DataTable)daPORT.ExecuteReader("select a.ID,a.StateName,a.City,a.Zone,a.CustName,a.Comments,a.UserName from s3CustomerList a where a.CustStatus=1 " + strZoneName + " and a.StateName='" + ddl3sState.SelectedItem.Text + "' order by Zone,StateName,City,CustName", DataAccess.Return_Type.DataTable);
                    else if (ddl3sState.Text != "ALL" && ddl3sCategory.Text != "ALL")
                        dt3sList = (DataTable)daPORT.ExecuteReader("select a.ID,a.StateName,a.City,a.Zone,a.CustName,a.Comments,a.UserName from s3CustomerList a where a.CustStatus=1 " + strZoneName + " and a.StateName='" + ddl3sState.SelectedItem.Text + "' and a.CategoryID='" + ddl3sCategory.SelectedItem.Value + "' order by Zone,StateName,City,CustName", DataAccess.Return_Type.DataTable);

                    if (dt3sList.Rows.Count > 0)
                    {
                        ViewState["dt3sList"] = dt3sList;

                        DataView view = new DataView(dt3sList);
                        view.Sort = "Zone ASC";
                        ViewState["sortorder"] = "DESC";
                        gv_3sDashboard.DataSource = view;
                        gv_3sDashboard.DataBind();
                        lblErrMsg.Text = "";
                        gv_3sDashboard.HeaderRow.Cells[0].CssClass = "sortasc";
                    }
                    else
                    {
                        gv_3sDashboard.DataSource = null;
                        gv_3sDashboard.DataBind();
                        lblErrMsg.Text = "No Records";
                    }

                    //DataTable dtCategory = (DataTable)daPORT.ExecuteReader("select ID,CategoryName,Method from s3CategoryMaster", DataAccess.Return_Type.DataTable);
                    //ViewState["dtCategory"] = dtCategory;
                }
                else
                    lblErrMsg.Text = "No records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvReviewList_sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dtReview = ViewState["dt3sList"] as DataTable;
                if (dtReview.Rows.Count > 0)
                {
                    DataView view = new DataView(dtReview);
                    if (e.SortDirection == SortDirection.Ascending && ViewState["sortorder"].ToString() == "")
                        view.Sort = e.SortExpression + " ASC";
                    else
                        view.Sort = e.SortExpression + " DESC";

                    gv_3sDashboard.DataSource = view;
                    gv_3sDashboard.DataBind();

                    if (ViewState["sortorder"].ToString() == "DESC")
                    {
                        gv_3sDashboard.HeaderRow.Cells[GetIndex(e.SortExpression)].CssClass = "sortdesc";
                        ViewState["sortorder"] = "";
                    }
                    else
                    {
                        gv_3sDashboard.HeaderRow.Cells[GetIndex(e.SortExpression)].CssClass = "sortasc";
                        ViewState["sortorder"] = "DESC";
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen", "BindFlagBgColor();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private int GetIndex(string SortExp)
        {
            int i = 0;
            foreach (DataControlField c in gv_3sDashboard.Columns)
            {
                if (c.SortExpression == SortExp)
                    return i;
                else
                    i++;
            }
            return i;
        }

        public string Bind_CustName(string str3sCustName, string str3sID)
        {
            string returnValue = string.Empty;
            try
            {
                returnValue += "<div onclick='goto3sChangePage(\"" + str3sID + "\");' class='s3custlink'>" + str3sCustName + "</div>";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnValue;
        }

        protected void gv_3sDashboard_OnDataBound(object sender, EventArgs e)
        {
            for (int i = gv_3sDashboard.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gv_3sDashboard.Rows[i];
                GridViewRow previousRow = gv_3sDashboard.Rows[i - 1];
                for (int j = 0; j < 2; j++)
                {
                    if (row.Cells[j].Text == previousRow.Cells[j].Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }

        public string Bind_3sMonthYears(string custcode, string strCategoryID)
        {
            StringBuilder strReturn = new StringBuilder();
            try
            {
                DataTable dt3sChangeList = ViewState["dt3sChangeList"] as DataTable;
                if (dt3sChangeList != null && dt3sChangeList.Rows.Count > 0)
                {
                    foreach (DataRow row in dt3sChangeList.Select("CustID='" + custcode + "' and ToCategoryID='" + strCategoryID + "'"))
                    {
                        strReturn.Append("<div>" + row["createddate"].ToString() + "</div>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strReturn.ToString();
        }

        public string Bind_3sTier(string custcode)
        {
            StringBuilder strReturn = new StringBuilder();
            try
            {
                DataTable dt3sChangeList = ViewState["dt3sChangeList"] as DataTable;
                if (dt3sChangeList != null && dt3sChangeList.Rows.Count > 0)
                {
                    foreach (DataRow row in dt3sChangeList.Select("CustID='" + custcode + "'"))
                    {
                        if (row["StatusID"].ToString() != "0")
                            strReturn.Append("<div>Tier " + row["StatusID"].ToString() + "</div>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strReturn.ToString();
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