using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Configuration;
using System.Web.Services;
using System.IO;

namespace TTS
{
    public partial class prospectreviewdomestic : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["prospect_reviewdom"].ToString() == "True")
                        {
                            Bind_ReviewDropDown();
                            if (Request["proscode"] != null && Request["proscode"].ToString() != "null" && Request["proscode"].ToString() != "")
                            {
                                ListItem selectedListItem = ddlCustList.Items.FindByValue(Request["proscode"].ToString());
                                if (selectedListItem != null)
                                {
                                    hdncust.Value = Request["proscode"].ToString();
                                    ddlCustList.Items.FindByText("ALL").Selected = false;
                                    selectedListItem.Selected = true;
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow", "gridbind();", true);
                                }
                                else
                                {
                                    lblRecordCount.Text = "No Records - " + Request["proscode"].ToString();
                                }
                            }
                            else
                            {
                                hdncust.Value = "1";
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gridbind();", true);
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
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_ReviewDropDown()
        {
            try
            {
                string supervisor = "ALL", manager = "ALL";
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                {
                    if (Request.Cookies["TTSUserType"].Value.ToLower() == "supervisor")
                        supervisor = Request.Cookies["TTSUser"].Value;
                    else if (Request.Cookies["TTSUserType"].Value.ToLower() == "manager")
                        manager = Request.Cookies["TTSUser"].Value;
                }
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@supervisor", supervisor);
                sp1[1] = new SqlParameter("@manager", manager);
                DataTable dtList = (DataTable)daPORT.ExecuteReader_SP("sp_sel_Reviewdomestic_ddl", sp1, DataAccess.Return_Type.DataTable);
                //Bind CustName
                DataView dtView = new DataView(dtList);
                dtView.Sort = "Custname";
                DataTable distinctName = dtView.ToTable(true, "custcode", "Custname");
                Utilities.ddl_Binding(ddlCustList, distinctName, "Custname", "custcode", "ALL");

                //Bind Country
                dtView = new DataView(dtList);
                dtView.Sort = "City";
                DataTable distinctCountry = dtView.ToTable(true, "City");
                Utilities.ddl_Binding(ddlCity, distinctCountry, "City", "City", "ALL");

                //Bind Focus
                dtView = new DataView(dtList);
                dtView.Sort = "focus";
                DataTable distinctFocus = dtView.ToTable(true, "focus");
                Utilities.ddl_Binding(ddlFocusList, distinctFocus, "focus", "focus", "ALL");

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

                //Bind Lead Name List
                //DataTable dtLeadList = new DataTable();
                //SqlParameter[] sp = new SqlParameter[2];
                //sp[0] = new SqlParameter("@supervisor", supervisor);
                //sp[1] = new SqlParameter("@manager", manager);
                //dtLeadList = (DataTable)daPORT.ExecuteReader_SP("sp_sel_Reviewdomestic_Lead", sp, DataAccess.Return_Type.DataTable);
                //Utilities.ddl_Binding(ddlLeadList, dtLeadList, "LeadersName", "LeadersName", "ALL");
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

                DataTable dt = (DataTable)daPORT.ExecuteReader("Sp_sel_Supplier_Dom", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    dtView = new DataView(dt);
                    dtView.Sort = "sup_name";
                    DataTable distinctSupplier = dtView.ToTable(true, "sup_name");
                    Utilities.ddl_Binding(ddlSupplier, distinctSupplier, "sup_name", "sup_name", "ALL");

                    hdnSupplier.Value = Utilities.Serialization(dt);
                }

                DataTable dtLeadHis = (DataTable)daPORT.ExecuteReader_SP("sp_sel_All_LeadDetails_toprecords", DataAccess.Return_Type.DataTable);
                if (dtLeadHis.Rows.Count > 0)
                    hdnLeadfeedback.Value = Utilities.Serialization(dtLeadHis);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}