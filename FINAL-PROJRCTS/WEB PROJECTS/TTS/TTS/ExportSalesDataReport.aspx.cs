using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace TTS
{
    public partial class ExportSalesDataReport : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCots = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
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
                            if (Request["duration"] != null)
                            {
                                Response.Write(bindDdl_WebMethod(Request["duration"].ToString(), Request["config"].ToString(), Request["grade"].ToString()));
                                Response.End();
                            }
                            else
                                bindCategories();
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
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindCategories()
        {
            try
            {
                DataTable dtCategories = (DataTable)daCots.ExecuteReader_SP("SP_LST_EXPORTSALESDATA_Categories", DataAccess.Return_Type.DataTable);
                ScriptManager.RegisterStartupScript(Page, GetType(), "bindCategories", "bindDDL(" + serializeDt(dtCategories) + ");", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private string serializeDt(DataTable dtCategories)
        {
            try
            {
                if (dtCategories.Rows.Count > 0)
                {
                    List<string> Platform = dtCategories.AsEnumerable().Where(n => n.Field<string>("Platform") != "" && n.Field<string>("Platform") != null).OrderBy(n => n.Field<string>("Platform")).Select(n => n.Field<string>("Platform")).Distinct().ToList();
                    List<string> Grade = dtCategories.AsEnumerable().Where(n => n.Field<string>("Grade") != "" && n.Field<string>("Grade") != null).OrderBy(n => n.Field<string>("Grade")).Select(n => n.Field<string>("Grade")).Distinct().ToList();
                    List<string> CustomerName = dtCategories.AsEnumerable().Where(n => n.Field<string>("CustomerName") != "" && n.Field<string>("CustomerName") != null).OrderBy(n => n.Field<string>("CustomerName")).Select(n => n.Field<string>("CustomerName")).Distinct().ToList();

                    var jsonSerializer = new JavaScriptSerializer();
                    string jsonString = "{ \"Platform\" :" + jsonSerializer.Serialize(Platform);
                    jsonString += ", \"Grade\" :" + jsonSerializer.Serialize(Grade);
                    jsonString += ", \"CustomerName\" :" + jsonSerializer.Serialize(CustomerName) + " }";
                    return jsonString;
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }

        }

        private string bindDdl_WebMethod(string duration, string config, string grade)
        {
            try
            {
                string message = "";
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@duration", duration == "" ? 0 : Convert.ToInt32(duration)),
                    new SqlParameter("@platform",config.Length > 0 ?  config : string.Empty),
                    new SqlParameter("@grade",grade.Length > 0 ? grade : string.Empty),
                    new SqlParameter("@ResultQuery",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message)
                    };
                DataTable dtCategories = (DataTable)daCots.ExecuteReader_SP("SP_SEL_ExportSalesData_SpecificCategories", sp, DataAccess.Return_Type.DataTable);
                if (dtCategories.Rows.Count > 0)
                    return serializeDt(dtCategories);
                else
                    return "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
    }
}