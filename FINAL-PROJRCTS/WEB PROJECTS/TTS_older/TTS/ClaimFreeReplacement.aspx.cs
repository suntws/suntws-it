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
using System.Data.OleDb;
using System.Globalization;
namespace TTS
{
    public partial class ClaimFreeReplacement : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["claim_dispatch_mmn"].ToString() == "True" || dtUser.Rows[0]["claim_dispatch_pdk"].ToString() == "True"))
                        {
                            DataTable dt = new DataTable();
                            gvClaimTrackList.DataSource = null;
                            gvClaimTrackList.DataBind();
                            dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimFreeReplacement", DataAccess.Return_Type.DataTable);
                            gvClaimTrackList.DataSource = dt;
                            gvClaimTrackList.DataBind();
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
            try
            {
                btnSave.Style.Add("display", "none");
                gvReplaceStencil.DataSource = null;
                gvReplaceStencil.DataBind();
                gvClaimApproveItems.DataSource = null;
                gvClaimApproveItems.DataBind();

                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                hdnplant.Value = clickedRow.Cells[3].Text.Replace("&amp;", "&");
                hdnCreditNote.Value = (clickedRow.FindControl("hdncreditnote") as HiddenField).Value;
                hdnCustCode.Value = (clickedRow.FindControl("hdnClaimCustCode") as HiddenField).Value;

                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp1[2] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCrmOpinionItems_FreeSettlement", sp1, DataAccess.Return_Type.DataTable);
                if (dtItems.Rows.Count > 0)
                {
                    gvClaimApproveItems.DataSource = dtItems;
                    gvClaimApproveItems.DataBind();
                    SqlParameter[] sp2 = new SqlParameter[] { 
                        new SqlParameter("@plant", hdnplant.Value), 
                        new SqlParameter("@custcode", hdnCustCode.Value), 
                        new SqlParameter("@orderrefno", lblClaimNo.Text) 
                    };
                    DataTable dtStockData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_domesticdispatchedbarcode", sp2, DataAccess.Return_Type.DataTable);
                    if (dtStockData != null && dtStockData.Rows.Count > 0)
                    {
                        gvReplaceStencil.DataSource = dtStockData;
                        gvReplaceStencil.DataBind();
                        btnSave.Style.Add("display", "block");
                    }
                    else
                        lblErrMsg.Text = "PDI INSPECTION NOT COMPLETED<br/>PLEASE INFORM TO QC TEAM";

                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "displayblock('divcancel');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[11];
                sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp2[2] = new SqlParameter("@plant", hdnplant.Value);
                sp2[3] = new SqlParameter("@DCNO", txtDcNo.Text);
                sp2[4] = new SqlParameter("@JJNO", txtJJNo.Text);
                sp2[5] = new SqlParameter("@Qty", txtQty.Text);
                sp2[6] = new SqlParameter("@TranspoterName", txttransport.Text);
                sp2[7] = new SqlParameter("@LRNO", txtLrno.Text);
                sp2[8] = new SqlParameter("@LRDATE", DateTime.ParseExact(txtLrDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                sp2[9] = new SqlParameter("@CreatedBy", Request.Cookies["TTSUser"].Value);
                sp2[10] = new SqlParameter("@CreditNoteNo", hdnCreditNote.Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_ClaimReplacementDetails", sp2);
                if (resp > 0)
                {
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}