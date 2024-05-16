using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TTS
{
    public partial class typeHardnessRange : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["type_masterdata"].ToString() == "True")
                        {
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_processid_TyreType", DataAccess.Return_Type.DataTable);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                ddlType.DataSource = dt;
                                ddlType.DataTextField = "Tyre_Type";
                                ddlType.DataValueField = "Tyre_Type";
                                ddlType.DataBind();
                                ddlType.Items.Insert(0, "CHOOSE");
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
        protected void ddlType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblExists.Text = "";
                lblHistory.Text = "";
                grdExist.DataSource = null;
                grdExist.DataBind();
                grdHistory.DataSource = null;
                grdHistory.DataBind();
                txtFbase.Text = "0";
                txtTbase.Text = "0";
                txtFtread.Text = "0";
                txtTtread.Text = "0";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Tyretype", ddlType.SelectedItem.ToString()) };
                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_TyreType_ExistRange", sp, DataAccess.Return_Type.DataSet);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblExists.Text = "CURRENT HARDNESS RANGE";
                        txtFbase.Text = ds.Tables[0].Rows[0]["BaseFromRange"].ToString();
                        txtTbase.Text = ds.Tables[0].Rows[0]["BaseToRange"].ToString();
                        txtFtread.Text = ds.Tables[0].Rows[0]["TreadFromRange"].ToString();
                        txtTtread.Text = ds.Tables[0].Rows[0]["TreadToRange"].ToString();
                        grdExist.DataSource = ds.Tables[0];
                        grdExist.DataBind();
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        lblHistory.Text = "PREVIOUS HISTORY OF HARDNESS RANGE";
                        grdHistory.DataSource = ds.Tables[1];
                        grdHistory.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkSave_click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@Tyretype", ddlType.SelectedItem.Text), 
                    new SqlParameter("@Fbase", txtFbase.Text), 
                    new SqlParameter("@Tbase", txtTbase.Text), 
                    new SqlParameter("@Ftread", txtFtread.Text), 
                    new SqlParameter("@Ttread", txtTtread.Text), 
                    new SqlParameter("@User", Request.Cookies["TTSUser"].Value) 
                };
                int res = (int)daCOTS.ExecuteNonQuery_SP("sp_Ins_TyreType_NewRange", sp);
                if (res > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "SaveSuccess", "alert('Range limit successfully saved');", true);
                    ddlType_IndexChange(null, null);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}