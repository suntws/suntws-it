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
    public partial class stockrim : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["stock_report"].ToString() == "True")
                        {
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@RimPlant", Utilities.Decrypt(Request["spid"].ToString())) };
                            DataTable dtMaster = (DataTable)daCOTS.ExecuteReader_SP("sel_Rimprocessid_ddlRimstock", sp, DataAccess.Return_Type.DataTable);
                            if (dtMaster.Rows.Count > 0)
                            {
                                ViewState["dtMaster"] = dtMaster;
                                DataView viewMM = new DataView(dtMaster);
                                DataTable distinctRimSize = viewMM.ToTable(true, "RimSize");
                                List<string> lstRimSize = new List<string>();
                                lstRimSize = distinctRimSize.AsEnumerable().Where(n => n.Field<string>("RimSize").ToString() != "").Select(n => n.Field<string>("RimSize")).ToList<string>();

                                if (lstRimSize.Count > 0)
                                {
                                    lstRimSize.Insert(0, "ALL");
                                    ddl_Rimsize.DataSource = lstRimSize;
                                    ddl_Rimsize.DataBind();

                                    DataTable distinctEDCNO = viewMM.ToTable(true, "EDCNO");
                                    List<string> lstEDCNO = new List<string>();
                                    lstEDCNO = dtMaster.AsEnumerable().Where(n => n.Field<string>("EDCNO").ToString() != "").Select(n => n.Field<string>("EDCNO")).ToList<string>();
                                    lstEDCNO.Insert(0, "ALL");
                                    ddl_EdcNo.DataSource = lstEDCNO;
                                    ddl_EdcNo.DataBind();

                                    Bind_GridView();
                                }
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
                    Response.Redirect("SessionExp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "stockrim.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvRimStock_DatBound(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvRimStock.Rows)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@EDCNO", ((Label)row.FindControl("lblEdcNo")).Text) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_sel_Rim_ProcessID_Details_ForEdcNo", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        FormView frmRimProcessID = (FormView)row.FindControl("frmRimProcessID_Details");
                        frmRimProcessID.DataSource = dt;
                        frmRimProcessID.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "stockrim.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Rimsize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddl_EdcNo.DataSource = null;
                ddl_EdcNo.DataBind();
                DataTable dtMaster = (DataTable)ViewState["dtMaster"];
                if (dtMaster.Rows.Count > 0)
                {
                    List<string> lstEDCNO = new List<string>();
                    if (ddl_Rimsize.SelectedIndex > 0)
                        lstEDCNO = dtMaster.AsEnumerable().Where(n => n.Field<string>("RimSize").ToString() == ddl_Rimsize.SelectedItem.Text).Select(n =>
                            n.Field<string>("EDCNO")).ToList<string>();
                    else if (ddl_Rimsize.SelectedIndex == 0)
                        lstEDCNO = dtMaster.AsEnumerable().Where(n => n.Field<string>("EDCNO").ToString() != "").Select(n => n.Field<string>("EDCNO")).ToList<string>();
                    if (lstEDCNO.Count > 0)
                    {
                        lstEDCNO.Insert(0, "ALL");
                        ddl_EdcNo.DataSource = lstEDCNO;
                        ddl_EdcNo.DataBind();

                        Bind_GridView();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "stockrim.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_EdcNo_IndexChange(object sender, EventArgs e)
        {
            try
            {
                DataTable dtMaster = (DataTable)ViewState["dtMaster"];
                if (dtMaster.Rows.Count > 0)
                {
                    List<string> lstRimSize = new List<string>();
                    if (ddl_EdcNo.SelectedIndex > 0)
                    {
                        lstRimSize = dtMaster.AsEnumerable().Where(n => n.Field<string>("EDCNO").ToString() == ddl_EdcNo.SelectedItem.Text).Select(n =>
                            n.Field<string>("RimSize")).ToList<string>();
                        if (lstRimSize.Count == 1)
                            ddl_Rimsize.SelectedIndex = ddl_Rimsize.Items.IndexOf(ddl_Rimsize.Items.FindByValue(lstRimSize[0].ToString()));
                    }

                    Bind_GridView();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "stockrim.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_GridView()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@Edcno", ddl_EdcNo.SelectedItem.Text), 
                    new SqlParameter("@RimSize", ddl_Rimsize.SelectedItem.Text),
                    new SqlParameter("@RimPlant", Utilities.Decrypt(Request["spid"].ToString())) 
                };
                DataTable dtEdc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_edcno_rimsize_stock", sp1, DataAccess.Return_Type.DataTable);
                if (dtEdc.Rows.Count > 0)
                {
                    gvRimStock.DataSource = dtEdc;
                    gvRimStock.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "stockrim.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}