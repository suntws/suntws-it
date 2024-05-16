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
    public partial class cotscommon : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_usermaster"].ToString() == "True")
                        {
                            if (Request["gid"] != null && Request["gid"].ToString() != "")
                            {
                                if (Request["gid"].ToString() == "dom")
                                    lblScotsType.Text = "DOMESTIC";
                                else if (Request["gid"].ToString() == "exp")
                                    lblScotsType.Text = "EXPORT";
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                    else
                    {
                        Response.Redirect("sessionexp.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlCustCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = ""; lblSuccessMsg.Text = "";
                if (ddlCustCategory.SelectedItem.Text != "Choose")
                {
                    SqlParameter[] sp1 = new SqlParameter[2];
                    sp1[0] = new SqlParameter("@CustCategory", ddlCustCategory.SelectedItem.Text);
                    sp1[1] = new SqlParameter("@ScotsType", lblScotsType.Text);
                    DataTable dtTxt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsCommon", sp1, DataAccess.Return_Type.DataTable);
                    if (dtTxt.Rows.Count > 0)
                        txtCommonText.Text = dtTxt.Rows[0]["txtCommon"].ToString().Replace("~", "\r\n");
                    else
                        txtCommonText.Text = "";
                }
                else
                {
                    txtCommonText.Text = "";
                    lblErrMsg.Text = "Choose customer category";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnCommonSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@txtCommon", txtCommonText.Text.Replace("\r\n", "~"));
                sp1[1] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp1[2] = new SqlParameter("@CustCategory", ddlCustCategory.SelectedItem.Text);
                sp1[3] = new SqlParameter("@ScotsType", lblScotsType.Text);

                daCOTS.ExecuteNonQuery_SP("sp_update_CotsCommon", sp1);
                lblSuccessMsg.Text = "Successfully saved";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}