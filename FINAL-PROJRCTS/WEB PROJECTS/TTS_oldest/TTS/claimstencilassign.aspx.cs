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
using System.Text;

namespace TTS
{
    public partial class claimstencilassign : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    btnAssignto.Style.Add("display", "none");
                    btnMoveToQc.Style.Add("display", "none");
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["claim_stencilassign"].ToString() == "True")
                        {
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReceivedList", DataAccess.Return_Type.DataTable);
                            gvClaimNoList.DataSource = dt;
                            gvClaimNoList.DataBind();
                            gvClaimItems.Columns[5].Visible = true;
                            gvClaimItems.Columns[6].Visible = false;
                            gvClaimItems.Columns[7].Visible = false;
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

        protected void lnkClaimNo_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            ClaimNoClick(clickedRow);
            hdnClaimNoClick.Value = Convert.ToString(clickedRow.RowIndex);
            ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoClaimDiv('divclaim');", true);
        }

        private void ClaimNoClick(GridViewRow clickedRow)
        {
            try
            {
                btnMoveToQc.Style.Add("display", "none");
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;

                if (lblClaimCustName.Text != "" && lblClaimNo.Text != "")
                    lblClaim.Text = "-";
                hdnCustCode.Value = hdnClaimCustCode.Value;
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FullStencil_ClaimNoWise", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvClaimItems.DataSource = dt;
                    gvClaimItems.DataBind();
                    bool assy = false;
                    foreach (DataRow row in dt.Select("assigntoqc is null"))
                    {
                        assy = true;
                        break;
                    }
                    if (!assy)
                    {
                        gvClaimItems.Columns[5].Visible = false;
                        gvClaimItems.Columns[6].Visible = true;
                        gvClaimItems.Columns[7].Visible = true;
                        btnAssignto.Style.Add("display", "none");
                        btnMoveToQc.Style.Add("display", "block");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "Ctrlassigplant();", true);
                        btnAssignto.Style.Add("display", "block");
                        btnMoveToQc.Style.Add("display", "none");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvClaimItems.EditIndex = e.NewEditIndex;
                gvClaimNoList.SelectedIndex = Convert.ToInt32(hdnClaimNoClick.Value);
                ClaimNoClick(gvClaimNoList.SelectedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvClaimItems.Rows[e.RowIndex];
                Label lblAssignStencil = row.FindControl("lblAssignStencil") as Label;
                DropDownList ddlAssignToQc = row.FindControl("ddlAssignToQc") as DropDownList;

                if (lblAssignStencil.Text != "")
                {
                    SqlParameter[] spQc = new SqlParameter[5];
                    spQc[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    spQc[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    spQc[2] = new SqlParameter("@stencilno", lblAssignStencil.Text);
                    spQc[3] = new SqlParameter("@assigntoqc", ddlAssignToQc.SelectedItem.Text);
                    spQc[4] = new SqlParameter("@assignUser", Request.Cookies["TTSUser"].Value);

                    int resp = daCOTS.ExecuteNonQuery_SP("sp_assignQC_StencilNo", spQc);
                }
                gvClaimItems.EditIndex = -1;
                gvClaimNoList.SelectedIndex = Convert.ToInt32(hdnClaimNoClick.Value);
                ClaimNoClick(gvClaimNoList.SelectedRow);
                btnAssignto.Style.Add("display", "none");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gvClaimItems.EditIndex = -1;
                gvClaimNoList.SelectedIndex = Convert.ToInt32(hdnClaimNoClick.Value);
                ClaimNoClick(gvClaimNoList.SelectedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnMoveToQc_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                DataTable dtdistrict = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_plant_claimregister", sp1, DataAccess.Return_Type.DataTable);
                if (dtdistrict.Rows.Count > 0)
                {
                    int i = 0;
                    foreach (DataRow dr in dtdistrict.Rows)
                    {
                        i++;
                        SqlParameter[] sp = new SqlParameter[4];
                        sp[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                        sp[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                        sp[2] = new SqlParameter("@plant", dr["assigntoqc"].ToString());
                        sp[3] = new SqlParameter("@plantno", i);
                        resp = daCOTS.ExecuteNonQuery_SP("sp_assignQC_claim_master_details", sp);
                        if (resp > 0)
                        {
                            Utilities.ClaimMoveToAnotherTeam_StatusInsert(hdnCustCode.Value, lblClaimNo.Text, dr["assigntoqc"].ToString(), "5", "", Request.Cookies["TTSUser"].Value);
                        }
                    }
                    SqlParameter[] sp2 = new SqlParameter[2];
                    sp2[0] = new SqlParameter("@complaintno", lblClaimNo.Text);
                    sp2[1] = new SqlParameter("@custcode", hdnCustCode.Value);
                    daCOTS.ExecuteNonQuery_SP("sp_AssignClaim_moveToQc", sp2);
                }
                Response.Redirect("claimstencilassign.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnAssignto_Click(object sender, EventArgs e)
        {
            DataTable dtstencilno = new DataTable();
            DataColumn col = new DataColumn("assigntoqc", typeof(System.String));
            dtstencilno.Columns.Add(col);
            col = new DataColumn("stencilno", typeof(System.String));
            dtstencilno.Columns.Add(col);
            col = new DataColumn("complaintno", typeof(System.String));
            dtstencilno.Columns.Add(col);
            foreach (GridViewRow row in gvClaimItems.Rows)
            {
                Label lblstencilno = row.FindControl("lblstencilno") as Label;
                DropDownList ddlCombiPlantList = row.FindControl("ddlAssignQc") as DropDownList;
                dtstencilno.Rows.Add(ddlCombiPlantList.SelectedItem.Text, lblstencilno.Text, lblClaimNo.Text);
            }
            if (dtstencilno.Rows.Count > 0)
            {
                SqlParameter[] spQc = new SqlParameter[3];
                spQc[0] = new SqlParameter("@Claim_assignQC_StencilNo_dt", dtstencilno);
                spQc[1] = new SqlParameter("@assignUser", Request.Cookies["TTSUser"].Value);
                spQc[2] = new SqlParameter("@custcode", hdnCustCode.Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_assignQC_StencilNo_bulk", spQc);
                {
                    gvClaimItems.EditIndex = -1;
                    gvClaimNoList.SelectedIndex = Convert.ToInt32(hdnClaimNoClick.Value);
                    ClaimNoClick(gvClaimNoList.SelectedRow);
                }
            }
        }
    }
}