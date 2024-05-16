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
    public partial class CustApprovedList : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        if (Utilities.Decrypt(Request["ccode"]) != null && Utilities.Decrypt(Request["ccode"].ToString()) != "")
                        {
                            hdnCustCode.Value = Utilities.Decrypt(Request["ccode"].ToString());
                            lblCustName.Text = (string)daTTS.ExecuteScalar("select Custname from CustMaster where Custcode='" + hdnCustCode.Value + "'");
                            rbPlatformType_IndexChanged(sender, e);
                        }
                    }
                    if (chkTypeList.Items.Count == 0)
                        lnkAddType.Style.Add("display", "none");
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

        protected void rbPlatformType_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPlatform.DataSource = "";
                ddlPlatform.DataBind();
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();
                chkTypeList.DataSource = "";
                chkTypeList.DataBind();

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@sizecategory", rbPlatformType.SelectedItem.Value) };
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_Config_List", sp, DataAccess.Return_Type.DataTable);
                ddlPlatform.DataSource = dt;
                ddlPlatform.DataValueField = "Config";
                ddlPlatform.DataTextField = "Config";
                ddlPlatform.DataBind();

                if (dt.Rows.Count == 1)
                    ddlPlatform_IndexChanged(sender, e);
                else
                {
                    ddlPlatform.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                    ddlPlatform.SelectedIndex = 0;
                    Bind_Gridview();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlPlatform_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPlatform.SelectedItem.Text != "CHOOSE")
                {
                    ddlBrand.DataSource = "";
                    ddlBrand.DataBind();
                    ddlSidewall.DataSource = "";
                    ddlSidewall.DataBind();
                    chkTypeList.DataSource = "";
                    chkTypeList.DataBind();

                    DataTable dt = new DataTable();
                    SqlParameter[] sp1 = new SqlParameter[2];
                    sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                    sp1[1] = new SqlParameter("@SizeCategory", rbPlatformType.SelectedItem.Value);
                    dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_Brand", sp1, DataAccess.Return_Type.DataTable);

                    ddlBrand.DataSource = dt;
                    ddlBrand.DataTextField = "Brand";
                    ddlBrand.DataValueField = "Brand";
                    ddlBrand.DataBind();
                    if (dt.Rows.Count == 1)
                        ddlBrand_IndexChanged(sender, e);
                    else
                    {
                        ddlBrand.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                        ddlBrand.SelectedIndex = 0;
                        Bind_Gridview();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlBrand_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPlatform.SelectedItem.Text != "CHOOSE" && ddlBrand.SelectedItem.Text != "CHOOSE")
                {
                    ddlSidewall.DataSource = "";
                    ddlSidewall.DataBind();
                    chkTypeList.DataSource = "";
                    chkTypeList.DataBind();

                    DataTable dt = new DataTable();
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                    sp1[1] = new SqlParameter("@Brand", ddlBrand.SelectedItem.Text);
                    sp1[2] = new SqlParameter("@SizeCategory", rbPlatformType.SelectedItem.Value);
                    dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_Sidewall", sp1, DataAccess.Return_Type.DataTable);

                    ddlSidewall.DataSource = dt;
                    ddlSidewall.DataTextField = "Sidewall";
                    ddlSidewall.DataValueField = "Sidewall";
                    ddlSidewall.DataBind();

                    if (dt.Rows.Count == 1)
                        ddlSidewall_IndexChanged(sender, e);
                    else
                    {
                        ddlSidewall.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                        ddlSidewall.SelectedIndex = 0;
                        Bind_Gridview();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlSidewall_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPlatform.SelectedItem.Text != "CHOOSE" && ddlBrand.SelectedItem.Text != "CHOOSE" && ddlSidewall.SelectedItem.Text != "CHOOSE")
                {
                    chkTypeList.DataSource = "";
                    chkTypeList.DataBind();

                    DataTable dt = new DataTable();
                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                    sp1[1] = new SqlParameter("@Brand", ddlBrand.SelectedItem.Text);
                    sp1[2] = new SqlParameter("@Sidewall", ddlSidewall.SelectedItem.Text);
                    sp1[3] = new SqlParameter("@SizeCategory", rbPlatformType.SelectedItem.Value);

                    dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_Type_ApprovedList", sp1, DataAccess.Return_Type.DataTable);

                    if (dt.Rows.Count > 0)
                    {
                        Bind_Gridview();

                        chkTypeList.DataSource = dt;
                        chkTypeList.DataTextField = "TyreType";
                        chkTypeList.DataValueField = "TyreType";
                        chkTypeList.DataBind();
                        lblMsg.Text = "";
                        lnkAddType.Style.Add("display", "block");
                    }
                    else
                    {
                        lblMsg.Text = "No records";
                        lnkAddType.Style.Add("display", "none");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void chkTypeList_DataBind(object sender, EventArgs e)
        {
            try
            {
                if (chkTypeList.Items.Count > 0 && gv_ApprovedList.Rows.Count > 0)
                {
                    DataTable dtAppList = (DataTable)gv_ApprovedList.DataSource;
                    foreach (ListItem item in chkTypeList.Items)
                    {
                        item.Selected = false;
                        item.Enabled = true;
                        foreach (DataRow row in dtAppList.Select("Tyretype='" + item.Value + "'"))
                        {
                            item.Selected = true;
                            item.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkAddType_click(object sender, EventArgs e)
        {
            try
            {
                if (lblMsg.Text == "" && chkTypeList.Items.Count > 0)
                {
                    foreach (ListItem item in chkTypeList.Items)
                    {
                        if (item.Selected && item.Enabled)
                        {
                            SqlParameter[] sp1 = new SqlParameter[7];
                            sp1[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                            sp1[1] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                            sp1[2] = new SqlParameter("@Tyretype", item.Value);
                            sp1[3] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                            sp1[4] = new SqlParameter("@Sidewall", ddlSidewall.SelectedItem.Text);
                            sp1[5] = new SqlParameter("@SizeCategory", rbPlatformType.SelectedItem.Text);
                            sp1[6] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                            daTTS.ExecuteNonQuery_SP("Sp_Ins_ApprovedTyreList", sp1);
                        }
                    }
                    Bind_Gridview();
                    chkTypeList_DataBind(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Gridview()
        {
            try
            {
                gv_ApprovedList.DataSource = null;
                gv_ApprovedList.DataBind();

                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@Config", ddlPlatform.Items.Count > 0 && ddlPlatform.SelectedItem.Text != "CHOOSE" && ddlPlatform.SelectedItem.Text != "" ? ddlPlatform.SelectedItem.Text : "");
                sp1[2] = new SqlParameter("@brand", ddlBrand.Items.Count > 0 && ddlBrand.SelectedItem.Text != "CHOOSE" && ddlBrand.SelectedItem.Text != "" ? ddlBrand.SelectedItem.Text : "");
                sp1[3] = new SqlParameter("@Sidewall", ddlSidewall.Items.Count > 0 && ddlSidewall.SelectedItem.Text != "CHOOSE" && ddlSidewall.SelectedItem.Text != "" ? ddlSidewall.SelectedItem.Text : "");
                sp1[4] = new SqlParameter("@SizeCategory", rbPlatformType.SelectedItem.Text);

                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_ApprovedTyreList", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    gv_ApprovedList.DataSource = dt;
                    gv_ApprovedList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Bind_Gridview();
                gv_ApprovedList.PageIndex = e.NewPageIndex;
                gv_ApprovedList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gv_ApprovedList.Rows[e.RowIndex];
                Label lblConfig = row.FindControl("lblConfig") as Label;
                Label lblType = row.FindControl("lblType") as Label;
                Label lblBrand = row.FindControl("lblBrand") as Label;
                Label lblSidewall = row.FindControl("lblSidewall") as Label;
                Label lblCategory = row.FindControl("lblCategory") as Label;

                if (lblConfig.Text != "" && lblType.Text != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[7];
                    sp1[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    sp1[1] = new SqlParameter("@Config", lblConfig.Text);
                    sp1[2] = new SqlParameter("@Tyretype", lblType.Text);
                    sp1[3] = new SqlParameter("@brand", lblBrand.Text);
                    sp1[4] = new SqlParameter("@Sidewall", lblSidewall.Text);
                    sp1[5] = new SqlParameter("@SizeCategory", lblCategory.Text);
                    sp1[6] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                    int resp = daTTS.ExecuteNonQuery_SP("Sp_Ins_Edit_Temp_ApprovedTyreList", sp1);
                }
                gv_ApprovedList.EditIndex = -1;
                Bind_Gridview();
                chkTypeList_DataBind(sender, e);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            if (Utilities.Decrypt(Request["gpage"].ToString()) == "orderentry")
                Response.Redirect("cotsmanualorderincomplete.aspx?fid=" + Utilities.Encrypt("e"), false);
            else if (Utilities.Decrypt(Request["gpage"].ToString()) == "masterentry")
                Response.Redirect("editCustDetails.aspx?ccode=" + Request["ccode"].ToString(), false);
        }
    }
}