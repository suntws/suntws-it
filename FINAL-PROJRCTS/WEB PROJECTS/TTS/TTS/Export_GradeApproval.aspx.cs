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

namespace TTS
{
    public partial class Export_GradeApproval : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_usermaster"].ToString() == "True")
                        {
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsExportCustomer", DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                ddlCotsCustName.DataSource = dt;
                                ddlCotsCustName.DataTextField = "custfullname";
                                ddlCotsCustName.DataValueField = "custcode";
                                ddlCotsCustName.DataBind();
                            }
                            ddlCotsCustName.Items.Insert(0, "Choose");
                            ddlCotsCustName.Text = "Choose";
                            btnApprovedUserTypes.Style.Add("display", "none");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                    if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCotsCustName")
                        ddlCotsCustName_IndexChange(sender, e);
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
        protected void ddlCotsCustName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCotsCustName")
                {
                    hdnGrade.Value = "";
                    hdnLoginName.Value = "";
                    hdnCotsCustID.Value = "";
                    ddlLoginUserName.DataSource = "";
                    ddlLoginUserName.DataBind();
                    ddlCotsGrade.DataSource = "";
                    ddlCotsGrade.DataBind();
                    gv_TTSApprovedList.DataSource = null;
                    gv_TTSApprovedList.DataBind();
                    gv_TypeWiseDiscount.DataSource = null;
                    gv_TypeWiseDiscount.DataBind();
                    ddlCotsCustName.SelectedIndex = ddlCotsCustName.Items.IndexOf(ddlCotsCustName.Items.FindByText(hdnFullName.Value));

                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddlCotsCustName.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddlCotsCustName.SelectedItem.Value) 
                    };
                    DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                    if (dtUserList.Rows.Count > 0)
                    {
                        ddlLoginUserName.DataSource = dtUserList;
                        ddlLoginUserName.DataTextField = "username";
                        ddlLoginUserName.DataValueField = "ID";
                        ddlLoginUserName.DataBind();
                        if (dtUserList.Rows.Count == 1)
                            ddlLoginUserName_IndexChange(sender, e);
                        else
                            ddlLoginUserName.Items.Insert(0, "Choose");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlLoginUserName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                btnApprovedUserTypes.Style.Add("display", "none");
                hdnGrade.Value = "";
                ddlCotsGrade.DataSource = "";
                ddlCotsGrade.DataBind();
                gv_TTSApprovedList.DataSource = null;
                gv_TTSApprovedList.DataBind();
                gv_TypeWiseDiscount.DataSource = null;
                gv_TypeWiseDiscount.DataBind();
                Select_DropDownValue();
                if (ddlLoginUserName.SelectedItem.Text != "Choose")
                {
                    hdnCotsCustID.Value = ddlLoginUserName.SelectedItem.Value;
                    SqlParameter[] sp3 = new SqlParameter[1];
                    sp3[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                    DataTable dtSelDis = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_TypeWiseDiscount", sp3, DataAccess.Return_Type.DataTable);

                    if (dtSelDis.Rows.Count > 0)
                    {
                        gv_TypeWiseDiscount.DataSource = dtSelDis;
                        gv_TypeWiseDiscount.DataBind();
                    }

                    SqlParameter[] sp2 = new SqlParameter[1];
                    sp2[0] = new SqlParameter("@CustCode", ddlCotsCustName.SelectedValue);
                    DataTable dtGrade = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ApprovedGrade_Domestic", sp2, DataAccess.Return_Type.DataTable);
                    if (dtGrade.Rows.Count > 0)
                    {
                        ddlCotsGrade.DataSource = dtGrade;
                        ddlCotsGrade.DataTextField = "tyretype";
                        ddlCotsGrade.DataValueField = "tyretype";
                        ddlCotsGrade.DataBind();

                        if (ddlCotsGrade.Items.Count == 1)
                            ddlCotsGrade_IndexChange(sender, e);
                        else
                            ddlCotsGrade.Items.Insert(0, "Choose");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCotsGrade_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gv_TTSApprovedList.DataSource = null;
                gv_TTSApprovedList.DataBind();
                Select_DropDownValue();
                if (ddlCotsGrade.SelectedItem.Text != "Choose")
                {
                    SqlParameter[] sp2 = new SqlParameter[2];
                    sp2[0] = new SqlParameter("@CustCode", ddlCotsCustName.SelectedItem.Value);
                    sp2[1] = new SqlParameter("@tyretype", ddlCotsGrade.SelectedItem.Text);
                    DataTable dtTypes = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ApprovedBrandList_ForDiscount", sp2, DataAccess.Return_Type.DataTable);
                    foreach (GridViewRow row in gv_TTSApprovedList.Rows)
                    {
                        CheckBox chkRow = (row.Cells[4].FindControl("listCheckBox") as CheckBox);
                        chkRow.Checked = false;
                        chkRow.Enabled = true;
                    }
                    if (dtTypes.Rows.Count > 0)
                    {
                        gv_TTSApprovedList.DataSource = dtTypes;
                        gv_TTSApprovedList.DataBind();
                        btnApprovedUserTypes.Style.Add("display", "block");

                        foreach (GridViewRow row in gv_TTSApprovedList.Rows)
                        {
                            string strConfig = row.Cells[0].Text;
                            string strType = row.Cells[1].Text;
                            string strBrand = row.Cells[2].Text;
                            string strSidewall = row.Cells[3].Text;
                            CheckBox chkRow = (row.Cells[4].FindControl("listCheckBox") as CheckBox);
                            chkRow.Checked = false;
                            chkRow.Enabled = true;
                            foreach (GridViewRow rowIns in gv_TypeWiseDiscount.Rows)
                            {
                                Label lblTypeDiscount = rowIns.FindControl("lblTypeDiscount") as Label;
                                Label lblBrandDiscount = rowIns.FindControl("lblBrandDiscount") as Label;
                                Label lblConfigDiscount = rowIns.FindControl("lblConfigDiscount") as Label;
                                Label lblSidewallDiscount = rowIns.FindControl("lblSidewallDiscount") as Label;
                                if (lblConfigDiscount.Text == strConfig && lblTypeDiscount.Text == strType && lblBrandDiscount.Text == strBrand && lblSidewallDiscount.Text == strSidewall)
                                {
                                    chkRow.Checked = true;
                                    chkRow.Enabled = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_TypeWiseDiscount_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gv_TypeWiseDiscount.EditIndex = e.NewEditIndex;
                ddlLoginUserName_IndexChange(sender, e);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_TypeWiseDiscount_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gv_TypeWiseDiscount.Rows[e.RowIndex];
                Label lblTypeDiscount = row.FindControl("lblTypeDiscount") as Label;
                Label lblBrandDiscount = row.FindControl("lblBrandDiscount") as Label;
                Label lblConfigDiscount = row.FindControl("lblConfigDiscount") as Label;
                Label lblSidewallDiscount = row.FindControl("lblSidewallDiscount") as Label;
                CheckBox chkGradeView = row.FindControl("chkGradeView") as CheckBox;
                int strView = chkGradeView.Checked == true ? 1 : 0;
                if (lblTypeDiscount.Text != "" && lblBrandDiscount.Text != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[8];
                    sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                    sp1[1] = new SqlParameter("@TyreType", lblTypeDiscount.Text);
                    sp1[2] = new SqlParameter("@Brand", lblBrandDiscount.Text);
                    sp1[3] = new SqlParameter("@Discount", Convert.ToDecimal("0.00"));
                    sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                    sp1[5] = new SqlParameter("@Config", lblConfigDiscount.Text);
                    sp1[6] = new SqlParameter("@Sidewall", lblSidewallDiscount.Text);
                    sp1[7] = new SqlParameter("@GradeView", Convert.ToInt32(strView));

                    daCOTS.ExecuteNonQuery_SP("sp_ins_TypeWiseDiscount", sp1);
                }
                gv_TypeWiseDiscount.EditIndex = -1;
                ddlLoginUserName_IndexChange(sender, e);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_TypeWiseDiscount_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gv_TypeWiseDiscount.EditIndex = -1;
                ddlLoginUserName_IndexChange(sender, e);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_TypeWiseDiscount_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gv_TypeWiseDiscount.Rows[e.RowIndex];
                Label lblTypeDiscount = row.FindControl("lblTypeDiscount") as Label;
                Label lblBrandDiscount = row.FindControl("lblBrandDiscount") as Label;
                Label lblConfigDiscount = row.FindControl("lblConfigDiscount") as Label;
                Label lblSidewallDiscount = row.FindControl("lblSidewallDiscount") as Label;
                if (lblTypeDiscount.Text != "" && lblBrandDiscount.Text != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[7];
                    sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                    sp1[1] = new SqlParameter("@TyreType", lblTypeDiscount.Text);
                    sp1[2] = new SqlParameter("@Brand", lblBrandDiscount.Text);
                    sp1[3] = new SqlParameter("@Discount", Convert.ToDecimal("0.00"));
                    sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                    sp1[5] = new SqlParameter("@Config", lblConfigDiscount.Text);
                    sp1[6] = new SqlParameter("@Sidewall", lblSidewallDiscount.Text);

                    int resp = daCOTS.ExecuteNonQuery_SP("Sp_Del_TypeWiseDiscount", sp1);
                }
                gv_TypeWiseDiscount.EditIndex = -1;
                ddlLoginUserName_IndexChange(sender, e);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnApprovedUserTypes_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gv_TTSApprovedList.Rows)
                {
                    string strConfig = row.Cells[0].Text;
                    string strType = row.Cells[1].Text;
                    string strBrand = row.Cells[2].Text;
                    string strSidewall = row.Cells[3].Text;
                    CheckBox chkRow = (row.Cells[4].FindControl("listCheckBox") as CheckBox);
                    if (strConfig != "" && strType != "" && strBrand != "" && strSidewall != "" && chkRow.Checked && chkRow.Enabled)
                    {
                        SqlParameter[] sp1 = new SqlParameter[8];
                        sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                        sp1[1] = new SqlParameter("@TyreType", strType);
                        sp1[2] = new SqlParameter("@Brand", strBrand);
                        sp1[3] = new SqlParameter("@Discount", Convert.ToDecimal("0.00"));
                        sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                        sp1[5] = new SqlParameter("@Config", strConfig);
                        sp1[6] = new SqlParameter("@Sidewall", strSidewall);
                        sp1[7] = new SqlParameter("@GradeView", Convert.ToInt32("1"));
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_TypeWiseDiscount", sp1);
                        if (resp > 0)
                            ddlLoginUserName_IndexChange(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Select_DropDownValue()
        {
            try
            {
                ddlCotsCustName.SelectedIndex = ddlCotsCustName.Items.IndexOf(ddlCotsCustName.Items.FindByText(hdnFullName.Value));
                ddlLoginUserName.SelectedIndex = ddlLoginUserName.Items.IndexOf(ddlLoginUserName.Items.FindByText(hdnLoginName.Value));
                ddlCotsGrade.SelectedIndex = ddlCotsGrade.Items.IndexOf(ddlCotsGrade.Items.FindByText(hdnGrade.Value));
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}