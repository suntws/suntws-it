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

namespace TTS
{
    public partial class processidrimsizeassign : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["processid_create"].ToString() == "True")
                        {
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_EDCNO_Rim_ProcessID", DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                ddlEDCNO.DataSource = dt;
                                ddlEDCNO.DataTextField = "EDCNO";
                                ddlEDCNO.DataValueField = "EDCNO";
                                ddlEDCNO.DataBind();
                                ddlEDCNO.Items.Insert(0, "--SELECT--");
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlEDCNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnSave.Visible = false;
                chkTyreSize.DataSource = "";
                chkTyreSize.DataBind();
                frmRimProcessID_Details.DataSource = "";
                frmRimProcessID_Details.DataBind();
                gvAssignEdcTyreSize.DataSource = "";
                gvAssignEdcTyreSize.DataBind();
                if (ddlEDCNO.SelectedValue != "--SELECT--")
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@EDCNO", ddlEDCNO.SelectedValue) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_sel_Rim_ProcessID_Details_ForEdcNo", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        frmRimProcessID_Details.DataSource = dt;
                        frmRimProcessID_Details.DataBind();

                        string _Category = ((Label)frmRimProcessID_Details.FindControl("lblTyreCategory")).Text;
                        SqlParameter[] sps = new SqlParameter[] { 
                            new SqlParameter("@SizeCategory", _Category == "SOLID" ? 1 : _Category == "PNEUMATIC" ? 3 : _Category == "POB" ? 2 : 0), 
                            new SqlParameter("@rimsize", (dt.Rows[0]["Rimsize"].ToString()).Substring(0, 3)) 
                        };
                        DataTable dts = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_TyreSize_For_RimProcessID_Assign", sps, DataAccess.Return_Type.DataTable);
                        if (dts.Rows.Count > 0)
                        {
                            chkTyreSize.DataSource = dts;
                            chkTyreSize.DataTextField = "AssignSizeList";
                            chkTyreSize.DataValueField = "AssignSizeList";
                            chkTyreSize.DataBind();
                        }

                        SqlParameter[] param = new SqlParameter[] { new SqlParameter("@EDCNO", Convert.ToString(ddlEDCNO.SelectedValue)) };
                        DataTable dtExists = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_RimProcessID_Tyresize_List", param, DataAccess.Return_Type.DataTable);
                        foreach (ListItem item in chkTyreSize.Items)
                        {
                            item.Enabled = true;
                            foreach (DataRow row in dtExists.Select("AssignSize='" + item.Text + "'"))
                            {
                                item.Selected = true;
                                item.Enabled = false;
                            }
                        }

                        SqlParameter[] spSize = new SqlParameter[] { new SqlParameter("@EDCNO", Convert.ToString(ddlEDCNO.SelectedValue)) };
                        DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Edc_AssignTyreSize", spSize, DataAccess.Return_Type.DataTable);
                        if (dtList.Rows.Count > 0)
                        {
                            gvAssignEdcTyreSize.DataSource = dtList;
                            gvAssignEdcTyreSize.DataBind();
                        }
                        btnSave.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "processidrequest", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@EDCNO", ddlEDCNO.SelectedValue);
                param[1] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                foreach (ListItem item in chkTyreSize.Items)
                {
                    if (item.Selected && item.Enabled)
                    {
                        string[] strSplit = item.Value.Split('~');
                        param[2] = new SqlParameter("@TyreSize", strSplit[0].ToString());
                        param[3] = new SqlParameter("@RimSize", strSplit[1].ToString());
                        daCOTS.ExecuteNonQuery_SP("Sp_Ins_RimProcessID_Assign_Tyresize", param);
                    }
                }
                ddlEDCNO_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@EDCNO", ddlEDCNO.SelectedValue),
                    new SqlParameter("@TyreSize", clickedRow.Cells[0].Text),
                    new SqlParameter("@RimSize", clickedRow.Cells[1].Text),
                    new SqlParameter("@ModifyUser", Request.Cookies["TTSUser"].Value)
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_Rim_ProcessID_TyreSize_Assign", sp);
                if (resp > 0)
                    ddlEDCNO_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}