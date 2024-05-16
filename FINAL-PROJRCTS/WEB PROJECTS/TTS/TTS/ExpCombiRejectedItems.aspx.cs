using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class ExpCombiRejectedItems : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk"].ToString() == "True"))
                        {
                            if (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageTitle.Text = "COMBI REJECTED ITEMS(" + Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + ")";
                                bindGrid();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL IS WRONG";
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
        private void bindGrid()
        {
            try
            {
                lblCustName.Text = "";
                lblWorkorderNo.Text = "";
                gvRejectedDet.DataSource = null;
                gvRejectedDet.DataBind();
                gvRejectedlist.DataSource = null;
                gvRejectedlist.DataBind();
                gvRejOpinionDet.DataSource = null;
                gvRejOpinionDet.DataBind();
                rdblRejOpinion.SelectedIndex = -1;
                txtRejRemark.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "tblOpinHide", "document.getElementById('tblOpin').style.display='none';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "btnHide", "document.getElementById('btnSave').style.display='none';", true);


                SqlParameter[] sp = new SqlParameter[] {  
                                    new SqlParameter("@rejectedPlant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper())
                                };
                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_rejectedCombi_details", sp, DataAccess.Return_Type.DataTable);
                if (dtList.Rows.Count > 0)
                {
                    gvRejectedDet.DataSource = dtList;
                    gvRejectedDet.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkDcno_Click(object sender, EventArgs e)
        {
            try
            {
                txtRejRemark.Text = "";

                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnPID.Value = ((HiddenField)clickedRow.FindControl("hdnDPID")).Value;
                hdnDCID.Value = ((HiddenField)clickedRow.FindControl("hdnDID")).Value;
                hdnDcStatusId.Value = ((HiddenField)clickedRow.FindControl("hdnRejVal")).Value;
                hdnRejPlant.Value = ((HiddenField)clickedRow.FindControl("hdnRPlant")).Value;
                lblCustName.Text = ((HiddenField)clickedRow.FindControl("hdnCustname")).Value;
                lblWorkorderNo.Text = ((HiddenField)clickedRow.FindControl("hdnWo")).Value;

                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@rejPid",hdnPID.Value),
                    new SqlParameter("@RejDcid",hdnDCID.Value)
                };

                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_rejectedList_details", sp, DataAccess.Return_Type.DataSet);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvRejectedlist.DataSource = ds.Tables[0];
                    gvRejectedlist.DataBind();
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    gvRejOpinionDet.DataSource = ds.Tables[1];
                    gvRejOpinionDet.DataBind();
                }
                switch (Convert.ToInt32(hdnDcStatusId.Value))
                {
                    case 3:
                        if (hdnRejPlant.Value == Utilities.Decrypt(Request["pid"].ToString()).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "hideActions", "hideActivities();", true);
                        }
                        else
                        {
                            lblVariOpinions.Text = "SENDER'S OPINION";
                            ScriptManager.RegisterStartupScript(Page, GetType(), "tblRejOpinShow", "document.getElementById('tblOpin').style.display='block';", true);
                            btnSave.Text = "SAVE OPINION";
                        }
                        break;
                    case 4:
                        if (hdnRejPlant.Value == Utilities.Decrypt(Request["pid"].ToString()).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "tblRejOpinHide", "document.getElementById('tblOpin').style.display='none ';", true);
                            if (ds.Tables[1].Rows[0]["ReceiverOpinion"].ToString().Contains("INWARD"))
                                btnSave.Text = ds.Tables[1].Rows[0]["SenderOpinion"].ToString() == "OK" ? ("INWARD TO " + hdnRejPlant.Value) : ("RETURN TO " + (hdnRejPlant.Value == "MMN" ? "PDK" : "MMN"));
                            else
                                btnSave.Text = ds.Tables[1].Rows[0]["SenderOpinion"].ToString() == "OK" ? ("RETURN TO " + (hdnRejPlant.Value == "MMN" ? "PDK" : "MMN")) : ("INWARD TO " + hdnRejPlant.Value);
                            ScriptManager.RegisterStartupScript(Page, GetType(), "btnSaveShow", "btnSaveshow();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "hideActions", "hideActivities();", true);
                        }
                        break;
                    case 5:
                        if (hdnRejPlant.Value == Utilities.Decrypt(Request["pid"].ToString()).ToUpper())
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "hideActions", "hideActivities();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "tblRejOpinHide", "document.getElementById('tblOpin').style.display='none ';", true);
                            btnSave.Text = "RECEIVED";
                            ScriptManager.RegisterStartupScript(Page, GetType(), "btnSaveShow", "btnSaveshow();", true);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string sOpin = "";
                string rReOpin = "";
                string sReOpin = "";
                if (btnSave.Text == "SAVE OPINION")
                {
                    sOpin = rdblRejOpinion.SelectedValue == "1" ? "OK" : "NOT OK, " + txtRejRemark.Text.ToUpper();
                }
                else if (btnSave.Text == "RECEIVED")
                {
                    sReOpin = btnSave.Text + ", " + txtRejRemark.Text.ToUpper();
                }
                else
                {
                    rReOpin = (btnSave.Text.Contains("INWARD TO ") ? btnSave.Text.Replace("INWARD", "INWARDED") : btnSave.Text.Replace("RETURN", "RETURNED")) + ", " + txtRejRemark.Text.ToUpper();
                }
                SqlParameter[] spSave = new SqlParameter[]{
                    new SqlParameter("@dcId",hdnDCID.Value),
                    new SqlParameter("@sendOpin",sOpin),
                    new SqlParameter("@recReOpin",rReOpin),
                    new SqlParameter("@sendReOpin",sReOpin),
                    new SqlParameter("@user",Request.Cookies["TTSUser"].Value)
                };

                int res = daCOTS.ExecuteNonQuery_SP("sp_upd_DCRejectedOpinions_Details", spSave);
                if (res > 0)
                {

                    ScriptManager.RegisterStartupScript(Page, GetType(), "btnSaveAlert", "alert('OPINION SAVED SUCCESSFULLY!');", true);
                    bindGrid();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}