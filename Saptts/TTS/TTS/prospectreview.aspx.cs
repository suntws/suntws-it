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
    public partial class prospectreview : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["prospect_reviewexp"].ToString() == "True")
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
                                else lblRecordCount.Text = "No Records - " + Request["proscode"].ToString();
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
                DataTable dtList = new DataTable();
                dtList = (DataTable)daPORT.ExecuteReader_SP("sp_sel_ReviseAll_ddl", DataAccess.Return_Type.DataTable);

                //Bind CustName
                DataView dtView = new DataView(dtList);
                dtView.Sort = "Custname";
                DataTable distinctName = dtView.ToTable(true, "custcode", "Custname");
                Utilities.ddl_Binding(ddlCustList, distinctName, "Custname", "custcode", "ALL");
                //Bind Country
                dtView = new DataView(dtList);
                dtView.Sort = "Country";
                DataTable distinctCountry = dtView.ToTable(true, "Country");
                Utilities.ddl_Binding(ddlCountryList, distinctCountry, "Country", "Country", "ALL");
                //Bind Focus
                dtView = new DataView(dtList);
                dtView.Sort = "focus";
                DataTable distinctFocus = dtView.ToTable(true, "focus");
                Utilities.ddl_Binding(ddlFocusList, distinctFocus, "focus", "focus", "ALL");
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
                //Bind Lead Name List
                //SqlParameter[] sp1 = new SqlParameter[1];
                //sp1[0] = new SqlParameter("@UserType", "Lead");
                //DataTable dtLead = (DataTable)daTTS.ExecuteReader_SP("sp_sel_UserName_UsertypeWise", sp1, DataAccess.Return_Type.DataTable);
                //Utilities.ddl_Binding(ddlLeadList, dtLead, "PUserName", "PUserName", "ALL");
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

                DataTable dt = (DataTable)daPORT.ExecuteReader_SP("sp_sel_supplierHistory_Reassign", DataAccess.Return_Type.DataTable);
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

        #region[Get_Supplier_CustWise_WebMethod]
        [WebMethod]
        public static string Get_Supplier_CustWise(string strCustCode)
        {
            return Get_SupplierDetails(strCustCode.ToString()).ToString();
        }

        private static string Get_SupplierDetails(string strCustCode)
        {
            DataAccess daPORTWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp = new SqlParameter[1];
                sp[0] = new SqlParameter("@custcode", strCustCode);
                DataTable dtCust = (DataTable)daPORTWebMethod.ExecuteReader_SP("Sp_sel_ProspectCustomer_Review", sp, DataAccess.Return_Type.DataTable);
                if (dtCust.Rows.Count > 0)
                {
                    strApp.Append("<div class='revPrevCon'><div class='headname'>" + dtCust.Rows[0]["CustName"].ToString().ToUpper() + "</div><div>" + dtCust.Rows[0]["City"].ToString() + "</div><div>" + dtCust.Rows[0]["Country"].ToString() + "</div>");
                    strApp.Append("<div>" + dtCust.Rows[0]["Contactname"].ToString() + "</div><div>" + dtCust.Rows[0]["Phoneno"].ToString() + "</div>");
                    strApp.Append("<div>" + dtCust.Rows[0]["Email"].ToString() + "</div><div>" + dtCust.Rows[0]["Mobile"].ToString() + "</div>");
                    strApp.Append("<div><a onclick='goingWebUrl(this);' class='webSingleurlCss'>" + dtCust.Rows[0]["webaddress"].ToString() + "</a></div></div>");
                }
                sp = new SqlParameter[1];
                sp[0] = new SqlParameter("@custcode", strCustCode);
                DataTable dtSupply = (DataTable)daPORTWebMethod.ExecuteReader_SP("Sp_sel_Prospect_Supplier_Review", sp, DataAccess.Return_Type.DataTable);
                string strConcat = string.Empty;
                if (dtSupply != null && dtSupply.Rows.Count > 0)
                {
                    foreach (DataRow row in dtSupply.Rows)
                        strConcat += "<tr><td style='width:240px;'>" + row["sup_name"].ToString() + "</td><td style='width:30px;'>" + row["sup_from"].ToString() + "</td><td style='width:30px;'>" + row["sup_to"].ToString() + "</td></tr>";
                }
                if (strConcat.Length > 0)
                    strApp.Append("<div style='float:left;width:300px;'><table border='1px' style='width:300px;background-color:#E1EE9E;color: #000;'>" + strConcat + "</table></div>");

                if (dtCust.Rows[0]["specialinstruction"].ToString() != "" && dtCust.Rows[0]["specialinstruction"].ToString() != "Enter Special Instructions")
                    strApp.Append("<div style='float: left; width: 550px;background-color: #B5F0F8;margin-top: 5px;'>" + dtCust.Rows[0]["specialinstruction"].ToString() + "</div>");
                sp = new SqlParameter[1];
                sp[0] = new SqlParameter("@custcode", strCustCode);
                DataTable dtTarget = (DataTable)daPORTWebMethod.ExecuteReader_SP("Sp_sel_Prospect_Target_Review", sp, DataAccess.Return_Type.DataTable);
                string strConcat1 = string.Empty;
                if (dtTarget != null && dtTarget.Rows.Count > 0)
                {
                    foreach (DataRow row in dtTarget.Rows)
                        strConcat1 += "<tr><td style='width:80px;'>" + row["TargetYear"].ToString() + "</td><td>" + row["TargetQty"].ToString() + "~" + row["TargetTon"].ToString() + "</td></tr>";
                }
                if (strConcat1.Length > 0)
                    strApp.Append("<table cellspacing='0' rules='all' border='1' style='border-collapse: collapse;width:100px;background-color:#AEEFE7;color: #000;'>" + strConcat1 + "</table>");

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "prospectReview.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }
        #endregion

        #region[Get_Focus_Details_WebMethod]
        [WebMethod]
        public static string Get_Focus_Details(string strCustCode, string strFocus)
        {
            return Get_FocusDetails(strCustCode.ToString(), strFocus.ToString()).ToString();
        }

        private static string Get_FocusDetails(string strCustCode, string strFocus)
        {
            DataAccess daPORTWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
            StringBuilder strApp = new StringBuilder();
            try
            {
                DataTable dtFocus = (DataTable)daPORTWebMethod.ExecuteReader("select distinct focus from ProspectCustomer", DataAccess.Return_Type.DataTable);
                if (dtFocus.Rows.Count > 0)
                {
                    strApp.Append("<option value='Choose'>Choose</option>");
                    foreach (DataRow dtrow in dtFocus.Select("focus<>'" + strFocus + "'"))
                    {
                        strApp.Append("<option value='" + dtrow["focus"].ToString() + "'>" + dtrow["focus"].ToString() + "</option>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "prospectReview.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }
        #endregion
    }
}