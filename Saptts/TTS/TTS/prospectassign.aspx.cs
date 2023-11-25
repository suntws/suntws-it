using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Web.Services;

namespace TTS
{
    public partial class prospectassign : System.Web.UI.Page
    {
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["prospect_assign"].ToString() == "True")
                        {
                            Bind_DropDown();
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gridbind();", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_DropDown()
        {
            try
            {
                DataTable dtList = new DataTable();
                dtList = (DataTable)daPORT.ExecuteReader_SP("Sp_Sel_ProspectCustomer", DataAccess.Return_Type.DataTable);

                DataView dtView = new DataView(dtList);
                dtView.Sort = "Custname";
                DataTable distinctName = dtView.ToTable(true, "Custname");
                Utilities.ddl_Binding(ddlCustName, distinctName, "Custname", "Custname", "ALL");

                //Bind Country
                dtView = new DataView(dtList);
                dtView.Sort = "Country";
                DataTable distinctCountry = dtView.ToTable(true, "country");
                Utilities.ddl_Binding(ddlCountry, distinctCountry, "country", "country", "ALL");
                //Bind Focus 
                dtView = new DataView(dtList);
                dtView.Sort = "focus";
                DataTable distinctFocus = dtView.ToTable(true, "focus");
                Utilities.ddl_Binding(ddlFocus, distinctFocus, "focus", "focus", "ALL");

                //Bind City 
                StringBuilder details = new StringBuilder();
                dtView = new DataView(dtList);
                dtView.Sort = "City";
                DataTable distinctCity = dtView.ToTable(true, "City");
                Utilities.ddl_Binding(ddlCity, distinctCity, "City", "City", "ALL");
                if (distinctCity.Rows.Count > 0)
                {
                    details.Append("<option value='ALL'>ALL</option>");
                    foreach (DataRow dtrow in distinctCity.Rows)
                        details.Append("<option value='" + dtrow["City"].ToString() + "'>" + dtrow["City"].ToString() + "</option>");
                    hdnCity.Value = details.ToString();
                }
                //Bind Port
                dtView = new DataView(dtList);
                dtView.Sort = "port";
                DataTable distinctport = dtView.ToTable(true, "port");
                Utilities.ddl_Binding(ddlPort, distinctport, "port", "port", "ALL");

                //Bind LeadSource
                dtView = new DataView(dtList);
                dtView.Sort = "LeadSource";
                DataTable distinctLeadSource = dtView.ToTable(true, "LeadSource");
                Utilities.ddl_Binding(ddlLeadSource, distinctLeadSource, "LeadSource", "LeadSource", "ALL");

                DataTable dt = (DataTable)daPORT.ExecuteReader_SP("sp_sel_supplierHistory", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    dtView = new DataView(dt);
                    dtView.Sort = "sup_name";
                    DataTable distinctSupplier = dtView.ToTable(true, "sup_name");
                    Utilities.ddl_Binding(ddlSupplier, distinctSupplier, "sup_name", "sup_name", "ALL");

                    hdnSupplier.Value = Utilities.Serialization(dt);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            try
            {
                string[] strCustcode = hdncustcode.Value.Split(',');
                DataTable dtAssignList = new DataTable();
                DataColumn dCol = new DataColumn("CustCode", typeof(System.String));
                dtAssignList.Columns.Add(dCol);
                dCol = new DataColumn("focus", typeof(System.String));
                dtAssignList.Columns.Add(dCol);
                dCol = new DataColumn("channel", typeof(System.String));
                dtAssignList.Columns.Add(dCol);
                dCol = new DataColumn("assignto", typeof(System.String));
                dtAssignList.Columns.Add(dCol);
                string strLead = hdnLeadName.Value;
                for (int i = 0; i < strCustcode.Length; i++)
                {
                    string[] strdata = strCustcode[i].Split('~');
                    dtAssignList.Rows.Add(strdata[0], strdata[1], ddlChannel.SelectedItem.Text, strLead);
                }

                if (dtAssignList.Rows.Count > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@AssignList_dt", dtAssignList);
                    sp1[1] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    sp1[2] = new SqlParameter("@Supervisor", hdnSupevisorName.Value != "CHOOSE" || hdnSupevisorName.Value != "" ? hdnSupevisorName.Value : "");
                    sp1[3] = new SqlParameter("@Manager", hdnManagerName.Value != "CHOOSE" || hdnManagerName.Value != "" ? hdnManagerName.Value : "");
                    daPORT.ExecuteNonQuery_SP("Sp_Ins_AssignDetails_TableWise", sp1);

                    //SqlParameter[] sp2 = new SqlParameter[1];
                    //sp2[0] = new SqlParameter("@UserName", strLead);

                    //DataTable dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Chk_UserCreate", sp2, DataAccess.Return_Type.DataTable);
                    //if (dt.Rows.Count == 1)
                    Response.Redirect("prospectreassign.aspx", false);
                    //else
                    //    Response.Redirect("userprivilege1.aspx?lname=" + strLead, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        #region[get_LeadName_WebMethod]
        [WebMethod]
        public static string get_LeadName(string strChannel)
        {
            return GetLeadName_Details(strChannel.ToString(), "", "").GetXml();
        }
        [WebMethod]
        public static string get_SupervisorName(string strChannel, string strLead1)
        {
            return GetLeadName_Details(strChannel.ToString(), strLead1.ToString(), "").GetXml();
        }
        [WebMethod]
        public static string get_ManagerName(string strChannel, string strLead1, string strLead2)
        {
            return GetLeadName_Details(strChannel.ToString(), strLead1.ToString(), strLead2.ToString()).GetXml();
        }
        private static DataSet GetLeadName_Details(string strChannel, string Lead1, string Lead2)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp = new SqlParameter[3];
                sp[0] = new SqlParameter("@UserChannel", strChannel.ToUpper());
                sp[1] = new SqlParameter("@Lead1", Lead1);
                sp[2] = new SqlParameter("@Lead2", Lead2);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("sp_sel_UserName_ForProspect", sp, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "prospectassign.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[get_City_WebMethod]
        [WebMethod]
        public static string get_City(string Country)
        {
            return get_City_Details(Country.ToString()).GetXml();
        }

        private static DataSet get_City_Details(string Country)
        {
            DataAccess daPORTWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp = new SqlParameter[1];
                sp[0] = new SqlParameter("@Country", Country);
                ds = (DataSet)daPORTWebMethod.ExecuteReader_SP("Sp_Sel_ProspectCustomer_City", sp, DataAccess.Return_Type.DataSet);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "prospectassign.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion
    }
}