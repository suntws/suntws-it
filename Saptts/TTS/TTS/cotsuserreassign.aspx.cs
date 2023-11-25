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
    public partial class cotsuserreassign : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_leadassign"].ToString() == "True")
                        {
                            if (Request["cid"] != null && Request["cid"].ToString() != "")
                            {
                                if (Request["cid"].ToString() == "lead")
                                    Bind_CustAssociateLead();
                                Bind_gvCotsUser();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL is wrong.";
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
                ddlCustLead.DataSource = dtLead;
                ddlCustLead.DataTextField = "PUserName";
                ddlCustLead.DataValueField = "PUserName";
                ddlCustLead.DataBind();

                ddlCustLead.Items.Insert(0, "Choose");
                ddlCustLead.Text = "Choose";

                sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@UserType", "Supervisor");
                DataTable dtSupervisor = (DataTable)daTTS.ExecuteReader_SP("sp_sel_UserName_UsertypeWise", sp1, DataAccess.Return_Type.DataTable);
                ddlCustSupervisor.DataSource = dtSupervisor;
                ddlCustSupervisor.DataTextField = "PUserName";
                ddlCustSupervisor.DataValueField = "PUserName";
                ddlCustSupervisor.DataBind();

                ddlCustSupervisor.Items.Insert(0, "Choose");
                ddlCustSupervisor.Text = "Choose";

                sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@UserType", "Manager");
                DataTable dtManager = (DataTable)daTTS.ExecuteReader_SP("sp_sel_UserName_UsertypeWise", sp1, DataAccess.Return_Type.DataTable);
                ddlCustManger.DataSource = dtManager;
                ddlCustManger.DataTextField = "PUserName";
                ddlCustManger.DataValueField = "PUserName";
                ddlCustManger.DataBind();

                ddlCustManger.Items.Insert(0, "Choose");
                ddlCustManger.Text = "Choose";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_gvCotsUser()
        {
            try
            {
                hdnQType.Value = Request["cid"].ToString();
                if (Request["cid"].ToString() == "lead")
                {
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cots_assignuser", DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        gvCotsUserAssign.DataSource = dt;
                        gvCotsUserAssign.DataBind();
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "showDiv('divLeadValue');", true);
                }
                else if (Request["cid"].ToString() == "type")
                {
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_List_For_AssignCustType", DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        gvCustTypeAssign.DataSource = dt;
                        gvCustTypeAssign.DataBind();
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "showDiv('divCustType');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvCotsUserAssign_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Bind_gvCotsUser();
                gvCotsUserAssign.PageIndex = e.NewPageIndex;
                gvCotsUserAssign.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvCustTypeAssign_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Bind_gvCotsUser();
                gvCustTypeAssign.PageIndex = e.NewPageIndex;
                gvCustTypeAssign.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private string Build_Select_CustId(GridView gv)
        {
            string custID = string.Empty;
            try
            {
                foreach (GridViewRow gvRow in gv.Rows)
                {
                    CheckBox chkCode = gvRow.FindControl("ChkCotsCustList") as CheckBox;
                    HiddenField hdnCotsCustID = gvRow.FindControl("hdnCotsCustID") as HiddenField;
                    if (chkCode.Checked)
                    {
                        if (custID != "")
                            custID += "','" + hdnCotsCustID.Value;
                        else
                            custID = hdnCotsCustID.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return custID;
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                string concatCustID = "";
                if (Request["cid"].ToString() == "lead")
                {
                    concatCustID = Build_Select_CustId(gvCotsUserAssign);
                    if (concatCustID != "")
                    {
                        string strQuery = string.Empty;
                        if (ddlCustLead.SelectedItem.Text != "Choose")
                            strQuery += (strQuery != "" ? "," : "") + "Lead='" + ddlCustLead.SelectedItem.Text + "'";
                        if (ddlCustSupervisor.SelectedItem.Text != "Choose")
                            strQuery += (strQuery != "" ? "," : "") + "Supervisor='" + ddlCustSupervisor.SelectedItem.Text + "'";
                        if (ddlCustManger.SelectedItem.Text != "Choose")
                            strQuery += (strQuery != "" ? "," : "") + "Manager='" + ddlCustManger.SelectedItem.Text + "'";

                        if (strQuery.Length > 0)
                            resp = daCOTS.ExecuteNonQuery("update usermaster set " + strQuery + " where ID in ('" + concatCustID + "')");
                        ddlCustLead.SelectedIndex = -1;
                        ddlCustSupervisor.SelectedIndex = -1;
                        ddlCustManger.SelectedIndex = -1;
                    }
                }
                else if (Request["cid"].ToString() == "type")
                {
                    concatCustID = Build_Select_CustId(gvCustTypeAssign);
                    resp = daCOTS.ExecuteNonQuery("update usermaster set custtype='" + rdbCustType.SelectedItem.Text + "' where ID in ('" + concatCustID + "')");
                    rdbCustType.SelectedIndex = -1;
                }
                if (resp > 0)
                    Bind_gvCotsUser();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}