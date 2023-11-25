using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web.Services;
namespace TTS
{
    public partial class prospectReassign : System.Web.UI.Page
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
                            lblErrMsg.Text = "User privilege disabled. Please contact administrator.";
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
                dtList = (DataTable)daPORT.ExecuteReader_SP("sp_sel_ddl_assigned_list", DataAccess.Return_Type.DataTable);

                DataView dtView = new DataView(dtList);
                dtView.Sort = "Custname";
                DataTable distinctName = dtView.ToTable(true, "Custname");
                Utilities.ddl_Binding(ddlCustList, distinctName, "Custname", "Custname", "ALL");

                //Bind Country
                dtView = new DataView(dtList);
                dtView.Sort = "Country";
                DataTable distinctCountry = dtView.ToTable(true, "Country");
                Utilities.ddl_Binding(ddlCountryList, distinctCountry, "Country", "Country", "ALL");
                //Bind City
                dtView = new DataView(dtList);
                dtView.Sort = "focus";
                DataTable distinctFocus = dtView.ToTable(true, "focus");
                Utilities.ddl_Binding(ddlFocusList, distinctFocus, "focus", "focus", "ALL");

                //Bind Focus
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

                //Assinged To
                DataTable dtAssignTo = new DataTable();
                DataColumn dCol = new DataColumn("AssignedTo", typeof(System.String));
                dtAssignTo.Columns.Add(dCol);
                DataView dvAssign = new DataView(dtList);
                dvAssign.Sort = "lead";
                DataTable distinctAssign = dvAssign.ToTable(true, "lead");
                foreach (DataRow dRow in distinctAssign.Rows)
                {
                    if (dRow["lead"].ToString() != "")
                        dtAssignTo.Rows.Add(dRow["lead"].ToString());
                }
                dvAssign = new DataView(dtList);
                dvAssign.Sort = "Supervisor";
                DataTable distinctAssign1 = dvAssign.ToTable(true, "Supervisor");
                foreach (DataRow dRow in distinctAssign1.Rows)
                {
                    DataRow[] row1 = dtAssignTo.Select("AssignedTo='" + dRow["Supervisor"].ToString() + "'");
                    if (dRow["Supervisor"].ToString() != "" && row1.Length == 0)
                        dtAssignTo.Rows.Add(dRow["Supervisor"].ToString());
                }

                dvAssign = new DataView(dtList);
                dvAssign.Sort = "Manager";
                DataTable distinctAssign2 = dvAssign.ToTable(true, "Manager");
                foreach (DataRow dRow in distinctAssign2.Rows)
                {
                    DataRow[] row1 = dtAssignTo.Select("AssignedTo='" + dRow["Manager"].ToString() + "'");
                    if (dRow["Manager"].ToString() != "" && row1.Length == 0)
                        dtAssignTo.Rows.Add(dRow["Manager"].ToString());
                }
                dtAssignTo.DefaultView.Sort = "AssignedTo ASC";
                Utilities.ddl_Binding(ddlLeadList, dtAssignTo, "AssignedTo", "AssignedTo", "ALL");

                DataTable dt = (DataTable)daPORT.ExecuteReader_SP("sp_sel_supplierHistory_Reassign", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    dtView = new DataView(dt);
                    dtView.Sort = "sup_name";
                    DataTable distinctSupplier = dtView.ToTable(true, "sup_name");
                    Utilities.ddl_Binding(ddlSupplier, distinctSupplier, "sup_name", "sup_name", "ALL");

                    hdnSupplier.Value = Utilities.Serialization(dt);
                }

                DataTable dtLeadStatus = (DataTable)daPORT.ExecuteReader("select custcode,username as leadName,CONVERT(VARCHAR(11),createddate,106) as leadtime from LeadDetails", DataAccess.Return_Type.DataTable);
                if (dtLeadStatus.Rows.Count > 0)
                    hdnLeadStatus.Value = Utilities.Serialization(dtLeadStatus);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnReAssign_Click(object sender, EventArgs e)
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
                    daPORT.ExecuteNonQuery_SP("Sp_Ins_ReAssign_TableWise", sp1);

                    Response.Redirect("prospectreassign.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

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
                ds = (DataSet)daPORTWebMethod.ExecuteReader_SP("sp_sel_ddl_assigned_City", sp, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "prospectReassign.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion
    }
}