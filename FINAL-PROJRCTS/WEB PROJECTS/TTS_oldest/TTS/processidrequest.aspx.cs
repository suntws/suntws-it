using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Services;

namespace TTS
{
    public partial class processidrequest : System.Web.UI.Page
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
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["processid_request"].ToString() == "True")
                            bindDDl();
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
                Utilities.WriteToErrorLog("TTS", "processidrequest.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindDDl()
        {
            try
            {
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("SP_LST_ProcessIdCreate_Category", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    List<string> lstPlatform = new List<string>();
                    List<string> lstTyreSize = new List<string>();
                    List<string> lstTyreRim = new List<string>();
                    List<string> lstTyreType = new List<string>();
                    List<string> lstBrand = new List<string>();
                    List<string> lstSidewall = new List<string>();
                    lstPlatform = dt.AsEnumerable().Where(n => n.Field<string>("config").ToString() != "").Select(n => n.Field<string>("Config")).ToList<string>();
                    lstPlatform.Insert(0, "--SELECT--");

                    lstTyreSize = dt.AsEnumerable().Where(n => n.Field<string>("TyreSize").ToString() != "").Select(n => n.Field<string>("TyreSize")).ToList<string>();
                    lstTyreSize.Insert(0, "--SELECT--");

                    lstTyreRim = dt.AsEnumerable().Where(n => n.Field<string>("TyreRim").ToString() != "").Select(n => n.Field<string>("TyreRim")).ToList<string>();
                    lstTyreRim.Insert(0, "--SELECT--");

                    lstTyreType = dt.AsEnumerable().Where(n => n.Field<string>("TyreType").ToString() != "").Select(n => n.Field<string>("TyreType")).ToList<string>();
                    lstTyreType.Insert(0, "--SELECT--");

                    lstBrand = dt.AsEnumerable().Where(n => n.Field<string>("Brand").ToString() != "").Select(n => n.Field<string>("Brand")).ToList<string>();
                    lstBrand.Insert(0, "--SELECT--");

                    lstSidewall = dt.AsEnumerable().Where(n => n.Field<string>("Sidewall").ToString() != "").Select(n => n.Field<string>("Sidewall")).ToList<string>();
                    lstSidewall.Insert(0, "--SELECT--");

                    if (Request.Cookies["TTSUserDepartment"].Value == "CRM" || Request.Cookies["TTSUserDepartment"].Value == "MARKETING" ||
                        Request.Cookies["TTSUserDepartment"].Value == "PPC" || Request.Cookies["TTSUserDepartment"].Value == "EDC" ||
                        Request.Cookies["TTSUserDepartment"].Value == "QC")
                    // if (Request.Cookies["TTSUserDepartment"].Value == "EDC" || Request.Cookies["TTSUserDepartment"].Value == "QC")
                    
                    {
                        lstPlatform.Insert(lstPlatform.Count, "ADD NEW PLATFORM");
                        lstBrand.Insert(lstBrand.Count, "ADD NEW BRAND");
                        lstSidewall.Insert(lstSidewall.Count, "ADD NEW SIDEWALL");
                    }
                    if (Request.Cookies["TTSUserDepartment"].Value == "EDC" || Request.Cookies["TTSUserDepartment"].Value == "QC")
                    {
                        lstTyreSize.Insert(lstTyreSize.Count, "ADD NEW TYRE SIZE");
                        lstTyreRim.Insert(lstTyreRim.Count, "ADD NEW RIM");
                        lstTyreType.Insert(lstTyreType.Count, "ADD NEW TYPE");
                    }

                    dt = null;
                    ddlConfig.DataSource = lstPlatform;
                    ddlConfig.DataBind();
                    ddlTyreSize.DataSource = lstTyreSize;
                    ddlTyreSize.DataBind();
                    ddlRim.DataSource = lstTyreRim;
                    ddlRim.DataBind();
                    ddlTyreType.DataSource = lstTyreType;
                    ddlTyreType.DataBind();
                    ddlBrand.DataSource = lstBrand;
                    ddlBrand.DataBind();
                    ddlSidewall.DataSource = lstSidewall;
                    ddlSidewall.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnTriggerGrid_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                ViewState["dtProcessID"] = null;
                lblNoOfRecords.Text = "";
                gvProcessIDDetails.DataSource = null;
                gvProcessIDDetails.DataBind();
                string message = "";
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@config", ddlConfig.SelectedItem.Text != "--SELECT--" && ddlConfig.SelectedItem.Text != "ADD NEW PLATFORM" ? ddlConfig.SelectedItem.Text : ""), 
                    new SqlParameter("@tyresize", ddlTyreSize.SelectedItem.Text != "--SELECT--" && ddlTyreSize.SelectedItem.Text != "ADD NEW TYRE SIZE" ? ddlTyreSize.SelectedItem.Text : ""), 
                    new SqlParameter("@rimsize", ddlRim.SelectedItem.Text != "--SELECT--" && ddlRim.SelectedItem.Text != "ADD NEW RIM" ? ddlRim.SelectedItem.Text : ""), 
                    new SqlParameter("@tyretype", ddlTyreType.SelectedItem.Text != "--SELECT--" && ddlTyreType.SelectedItem.Text != "ADD NEW TYPE" ? ddlTyreType.SelectedItem.Text : ""), 
                    new SqlParameter("@brand", ddlBrand.SelectedItem.Text != "--SELECT--" && ddlBrand.SelectedItem.Text != "ADD NEW BRAND" ? ddlBrand.SelectedItem.Text : ""), 
                    new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text != "--SELECT--" && ddlSidewall.SelectedItem.Text != "ADD NEW SIDEWALL" ? ddlSidewall.SelectedItem.Text : ""), 
                    new SqlParameter("@category", ddlSizeCategory.SelectedItem.Text != "--SELECT--" ? ddlSizeCategory.SelectedItem.Value : ""), 
                    new SqlParameter("@ResultQuery", SqlDbType.VarChar, 8000, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Default, message) 
                };
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("SP_LST_Filtered_ProcessID", sp, DataAccess.Return_Type.DataTable);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JEnable1", "document.getElementById('divEnableProcessID').style.display='none';", true);
                if (dt.Rows.Count > 0)
                {
                    gvProcessIDDetails.DataSource = dt;
                    gvProcessIDDetails.DataBind();
                    ViewState["dtProcessID"] = dt;
                    lblNoOfRecords.Text = "AS PER FILTER BASED PROCESS-ID COUNT : " + dt.Rows.Count;

                    if (!ddlConfig.SelectedItem.Text.Contains("ADD NEW ") && !ddlBrand.SelectedItem.Text.Contains("ADD NEW ") &&
                        !ddlSidewall.SelectedItem.Text.Contains("ADD NEW ") && !ddlTyreType.SelectedItem.Text.Contains("ADD NEW ") &&
                        !ddlTyreSize.SelectedItem.Text.Contains("ADD NEW ") && !ddlRim.SelectedItem.Text.Contains("ADD NEW "))
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JCreate1", "document.getElementById('divNewProcessID').style.display='none';", true);
                        if (dt.Rows.Count == 1 && ((dt.Rows[0]["MMN"].ToString() == "N" || dt.Rows[0]["SLTL"].ToString() == "N" || dt.Rows[0]["SITL"].ToString() == "N"
                            || dt.Rows[0]["PDK"].ToString() == "N") && dt.Rows[0]["USAGE"].ToString() == "ON"))
                        {
                            lblEnableID.Text = dt.Rows[0]["PROCESS-ID"].ToString();
                            DataTable dtPlant = new DataTable();
                            DataColumn dCol = new DataColumn("ENABLE_PLANT", typeof(System.String));
                            dtPlant.Columns.Add(dCol);
                            if (dt.Rows[0]["MMN"].ToString() == "N")
                                dtPlant.Rows.Add("MMN");
                            if (dt.Rows[0]["SLTL"].ToString() == "N")
                                dtPlant.Rows.Add("SLTL");
                            if (dt.Rows[0]["SITL"].ToString() == "N")
                                dtPlant.Rows.Add("SITL");
                            if (dt.Rows[0]["PDK"].ToString() == "N")
                                dtPlant.Rows.Add("PDK");
                            if (dt.Rows.Count > 0)
                            {
                                rdoEnablePlant.DataSource = dtPlant;
                                rdoEnablePlant.DataTextField = "ENABLE_PLANT";
                                rdoEnablePlant.DataValueField = "ENABLE_PLANT";
                                rdoEnablePlant.DataBind();
                            }
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JEnable2", "document.getElementById('divEnableProcessID').style.display='block';", true);
                        }
                    }
                    else
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JCreate2", "document.getElementById('divNewProcessID').style.display='block';", true);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JCreate3", "document.getElementById('divNewProcessID').style.display='block';", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnEnablePlant_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@Plant", rdoEnablePlant.SelectedItem.Value), 
                    new SqlParameter("@processID", lblEnableID.Text), 
                    new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value) 
                };
                int resp = daTTS.ExecuteNonQuery_SP("sp_upt_plant_ProcessIDDetails", sp);
                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JAlert", "alert('Process-ID Enabled Successfully');", true);
                    Response.Redirect("processidrequest.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                ViewState["dtProcessID"] = null;
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@ProcessID", txtFindProcessID.Text), new SqlParameter("@Plant", "") };
                DataTable dtFind = (DataTable)daTTS.ExecuteReader_SP("SP_Sel_Find_ProcessID_Data", sp, DataAccess.Return_Type.DataTable);
                if (dtFind.Rows.Count > 0)
                {
                    gvProcessIDDetails.DataSource = dtFind;
                    gvProcessIDDetails.DataBind();
                    ViewState["dtProcessID"] = dtFind;
                }
                else
                    lblErrMsg.Text = "NO RECORDS FOR THIS PROCESS-ID";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlProcessIDPlant_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                ViewState["dtProcessID"] = null;
                if (ddlProcessIDPlant.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@ProcessID", ""), new SqlParameter("@Plant", ddlProcessIDPlant.SelectedItem.Value) };
                    DataTable dtFind = (DataTable)daTTS.ExecuteReader_SP("SP_Sel_Find_ProcessID_Data", sp, DataAccess.Return_Type.DataTable);
                    if (dtFind.Rows.Count > 0)
                    {
                        gvProcessIDDetails.DataSource = dtFind;
                        gvProcessIDDetails.DataBind();
                        ViewState["dtProcessID"] = dtFind;
                        lblNoOfRecords.Text = "AS PER FILTER BASED PROCESS-ID COUNT : " + dtFind.Rows.Count;
                    }
                    else
                        lblErrMsg.Text = "NO RECORDS FOR THIS PLANT";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnStockXls_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                if (ViewState["dtProcessID"] != null)
                {
                    string s = "";
                    string r = s.Reverse().ToString();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "ProcessID-" + DateTime.Now.ToShortDateString() + ""));
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/x-msexcel";
                    using (System.IO.StringWriter sw = new System.IO.StringWriter())
                    {
                        HtmlTextWriter hw = new HtmlTextWriter(sw);
                        gvProcessIDDetails.AllowPaging = false;
                        DataTable dtList = ViewState["dtProcessID"] as DataTable;
                        gvProcessIDDetails.DataSource = dtList;
                        gvProcessIDDetails.DataBind();
                        gvProcessIDDetails.RenderControl(hw);
                        Response.Write(sw.ToString());
                        Response.Flush();
                        Response.Clear();
                        Response.End();
                    }
                }
                else
                    lblErrMsg.Text = "NO RECORDS";
            }
            catch (System.Threading.ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        [WebMethod]
        public static string GetProcessId(string sizeCategory, string config, string brand, string sidewall, string type, string size, string rim,
            string finishedWt, string userName, string plant)
        {
            try
            {
                using (DataAccess daTTS = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString))
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@SizeCategory", sizeCategory.Trim()), 
                        new SqlParameter("@Config", config.Trim()), 
                        new SqlParameter("@TyreSize", size.Trim()), 
                        new SqlParameter("@TyreRim", rim.Trim()), 
                        new SqlParameter("@TyreType", type.Trim()), 
                        new SqlParameter("@Brand", brand.Trim()), 
                        new SqlParameter("@Sidewall", sidewall.Trim()), 
                        new SqlParameter("@FinishedWt", Convert.ToDecimal(finishedWt)), 
                        new SqlParameter("@CreatedBy", userName) 
                    };
                    DataTable dt = (DataTable)daTTS.ExecuteReader_SP("SP_GET_ProcessIdCreate_ProcessID_Details", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        string jsonObj;
                        if (dt.Columns[0].ToString() == "ProcessID")
                        {
                            sp = new SqlParameter[] { 
                            new SqlParameter("@Plant", plant), 
                            new SqlParameter("@processID", dt.Rows[0]["ProcessId"].ToString().Replace("REQUEST SAVED ","")), 
                            new SqlParameter("@UserName", userName)                         };
                            daTTS.ExecuteNonQuery_SP("sp_upt_plant_ProcessIDDetails", sp);

                            jsonObj = "{\"processid\":\"" + dt.Rows[0]["ProcessId"].ToString() + "\"}";
                        }
                        else
                            jsonObj = "{\"newprocessid\":\"" + dt.Rows[0]["NewProcessId"].ToString() + "\"}";
                        return jsonObj;
                    }
                    else
                        return "";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "processidrequest", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
    }
}