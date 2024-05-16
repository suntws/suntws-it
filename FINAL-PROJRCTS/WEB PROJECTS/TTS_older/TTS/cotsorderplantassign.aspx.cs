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
using System.IO;
using System.Text;

namespace TTS
{
    public partial class cotsorderplantassign : System.Web.UI.Page
    {
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_plantassign"].ToString() == "True")
                        {
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_plant_unassigned_list", DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvorderlist.DataSource = dtorderlist;
                                gvorderlist.DataBind();
                            }

                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PlantList", DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                ddlPlantList.DataSource = dt;
                                ddlPlantList.DataTextField = "Plant";
                                ddlPlantList.DataValueField = "Plant";
                                ddlPlantList.DataBind();
                            }
                            ddlPlantList.Items.Insert(0, "CHOOSE");
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
        protected void lnkPlantAssign_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblStausOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                lblCustName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                hdnOrderID.Value = ((HiddenField)clickedRow.FindControl("hdnOID")).Value;

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@OID", hdnOrderID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Stock_OrderItemList_PlantAssign_v1", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvPlantAssignList.DataSource = dtItemList;
                    gvPlantAssignList.DataBind();

                    gvPlantAssignList.Columns[9].Visible = false;
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvPlantAssignList.Columns[9].Visible = true;
                        break;
                    }
                    gvPlantAssignList.Columns[12].Visible = false;
                    if (ddlPlantList.Items.Count == 4)
                        gvPlantAssignList.Columns[12].Visible = true;
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnOrderPlantAssign_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvPlantAssignList.HeaderRow != null)
                {
                    int resp = 0;
                    CheckBox checkAllChk = (CheckBox)gvPlantAssignList.HeaderRow.FindControl("checkAllChk");
                    if (checkAllChk.Checked)
                    {
                        SqlParameter[] sp1 = new SqlParameter[3];
                        sp1[0] = new SqlParameter("@Plant", ddlPlantList.SelectedItem.Text);
                        sp1[1] = new SqlParameter("@AssignedUser", Request.Cookies["TTSUser"].Value);
                        sp1[2] = new SqlParameter("@OID", hdnOrderID.Value);

                        resp = daCOTS.ExecuteNonQuery_SP("sp_update_plant_ordermaster_All", sp1);
                    }
                    else if (!checkAllChk.Checked)
                    {
                        DataTable dtProcessID = new DataTable();
                        DataColumn col = new DataColumn("processid", typeof(System.String));
                        dtProcessID.Columns.Add(col);
                        foreach (GridViewRow row in gvPlantAssignList.Rows)
                        {
                            Label lblProcessid = row.FindControl("lblProcessid") as Label;
                            CheckBox chkPlantAssign = row.FindControl("chkPlantAssign") as CheckBox;
                            if (chkPlantAssign.Checked)
                                dtProcessID.Rows.Add(lblProcessid.Text);
                        }

                        if (dtProcessID.Rows.Count > 0)
                        {
                            SqlParameter[] sp2 = new SqlParameter[5];
                            sp2[0] = new SqlParameter("@OrderItemList_ProcessID_dt", dtProcessID);
                            sp2[1] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                            sp2[2] = new SqlParameter("@OID", hdnOrderID.Value);
                            sp2[3] = new SqlParameter("@Plant", ddlPlantList.SelectedItem.Text);
                            sp2[4] = new SqlParameter("@UpdateOrderRefNo", lblStausOrderRefNo.Text + "(" + ddlPlantList.SelectedItem.Text.Substring(0, 1) + ")");

                            resp = daCOTS.ExecuteNonQuery_SP("sp_update_plant_ordermaster_Particular", sp2);
                        }
                    }
                    if (resp > 0)
                        Response.Redirect("cotsorderplantassign.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}