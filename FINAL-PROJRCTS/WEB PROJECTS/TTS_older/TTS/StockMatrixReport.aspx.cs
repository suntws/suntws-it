using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;

namespace TTS
{
    public partial class StockMatrixReport : System.Web.UI.Page
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
                            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
                            {
                                DataTable dtCategories = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_StockMatrixReport_Categories", DataAccess.Return_Type.DataTable);
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "bindDDL(" + serializeDt(dtCategories) + ");", true);
                            }
                            else
                            {
                                Response.Write(bindDdl_WebMethod(Request["stocktype"].ToString(), Request["config"].ToString(), Request["grade"].ToString(), Request["brand"].ToString()));
                                Response.End();
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
            catch (System.Threading.ThreadAbortException)
            {
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
                    List<string> lstConfig = dtCategories.AsEnumerable().Where(n => n.Field<string>("config") != "" && n.Field<string>("config") != null).OrderBy(n => n.Field<string>("config")).Select(n => n.Field<string>("config")).Distinct().ToList();
                    List<string> brand = dtCategories.AsEnumerable().Where(n => n.Field<string>("brand") != "" && n.Field<string>("brand") != null).OrderBy(n => n.Field<string>("brand")).Select(n => n.Field<string>("brand")).Distinct().ToList();
                    List<string> sidewall = dtCategories.AsEnumerable().Where(n => n.Field<string>("sidewall") != "" && n.Field<string>("sidewall") != null).OrderBy(n => n.Field<string>("sidewall")).Select(n => n.Field<string>("sidewall")).Distinct().ToList();
                    List<string> stocktype = dtCategories.AsEnumerable().Where(n => n.Field<string>("stocktype") != "" && n.Field<string>("stocktype") != null).OrderBy(n => n.Field<string>("stocktype")).Select(n => n.Field<string>("stocktype")).Distinct().ToList();
                    List<string> grade = dtCategories.AsEnumerable().Where(n => n.Field<string>("grade") != "" && n.Field<string>("grade") != null).OrderBy(n => n.Field<string>("grade")).Select(n => n.Field<string>("grade")).Distinct().ToList();

                    var jsonSerializer = new JavaScriptSerializer();
                    string jsonString = "{ \"config\" :" + jsonSerializer.Serialize(lstConfig);
                    jsonString += ", \"brand\" :" + jsonSerializer.Serialize(brand);
                    jsonString += ", \"sidewall\" :" + jsonSerializer.Serialize(sidewall);
                    jsonString += ", \"stocktype\" :" + jsonSerializer.Serialize(stocktype);
                    jsonString += ", \"grade\" :" + jsonSerializer.Serialize(grade) + " }";
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

        private string bindDdl_WebMethod(string plant, string config, string grade, string brand)
        {
            try
            {
                string message = "";
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@stocktype",plant),
                    new SqlParameter("@platform",config.Length > 0 ?  config : string.Empty),
                    new SqlParameter("@grade",grade.Length > 0 ? grade : string.Empty),
                    new SqlParameter("@brand",brand.Length > 0 ? brand : string.Empty),
                    new SqlParameter("@ResultQuery",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message)
                    };
                DataTable dtCategories = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_StockMatrix_SpecificCategories", sp, DataAccess.Return_Type.DataTable);
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