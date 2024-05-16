using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Reflection;
using System.Web.Services;
using System.IO;

namespace TTS
{
    public partial class prospectstatus : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["prospect_leadfeedback"].ToString() == "True")
                        {
                            Bind_DropDown();
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
                else Response.Redirect("sessionexp.aspx", false);
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
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                dtList = (DataTable)daPORT.ExecuteReader_SP("Sp_sel_LeadStatus_ddl", sp1, DataAccess.Return_Type.DataTable);
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
                    foreach (DataRow dtrow in distinctCity.Rows) details.Append("<option value='" + dtrow["City"].ToString() + "'>" + dtrow["City"].ToString() + "</option>");
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

        private void Get_PreviousLeadHistory()
        {
            //Previous Lead History
            SqlParameter[] sp1 = new SqlParameter[1];
            sp1[0] = new SqlParameter("@assignto", Request.Cookies["TTSUser"].Value);
            DataTable dtLeadHis = (DataTable)daPORT.ExecuteReader_SP("sp_sel_LeadDetails_toprecords", sp1, DataAccess.Return_Type.DataTable);
            if (dtLeadHis.Rows.Count > 0)
            {
                ViewState["dtLeadHis"] = dtLeadHis;
            }
        }

        protected void btnLeadShowList_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtAssigned = new DataTable();
                dtAssigned = (DataTable)daPORT.ExecuteReader("select a.custcode,b.Custname,b.City,b.Country,b.focus,b.Mobile,b.Phoneno,b.flag,b.port,b.Email,b.webaddress,b.Contactname,b.specialinstruction,b.ID from AssignDetails a,ProspectCustomer b where  a.custcode=b.Custcode and b.CustStatus=1 and a.assignstatus=1 and a.assignto='" + Request.Cookies["TTSUser"].Value + "'", DataAccess.Return_Type.DataTable);
                if (dtAssigned.Rows.Count > 0)
                {
                    if (dtAssigned.Rows.Count > 1)
                        ViewState["dtAssigned"] = dtAssigned;
                    else if (dtAssigned.Rows.Count == 1)
                    {
                        hdnProsCustCode.Value = dtAssigned.Rows[0]["custcode"].ToString();
                        //gv_SingleList.DataSource = dtAssigned;
                        //gv_SingleList.DataBind();
                        //txtContactPerson.Text = dtAssigned.Rows[0]["Contactname"].ToString();
                        //txtContact1.Text = dtAssigned.Rows[0]["Mobile"].ToString(); ;
                        //txtContact2.Text = dtAssigned.Rows[0]["Phoneno"].ToString(); ;
                        //txtEmail.Text = dtAssigned.Rows[0]["Email"].ToString(); ;
                        //txtWebaddress.Text = dtAssigned.Rows[0]["webaddress"].ToString();

                        ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptSingleList1", "FlagBgChange(\"" + dtAssigned.Rows[0]["flag"].ToString() + "\");", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptSingleList2", "getCustWiseLeadDetails('1', '6', '1', 'divPrevHistory');", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptSingleList3", "getCustAllAttachment();", true);
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptVisible", "gvRecordsWiseVisible();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen", "BindFlagBgColor();", true);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public string Bind_SupplierDetails(string custid)
        {
            StringBuilder strReturn = new StringBuilder();
            string strConcat = string.Empty;
            try
            {
                DataTable dtSupply = ViewState["dtSupply"] as DataTable;
                if (dtSupply != null && dtSupply.Rows.Count > 0)
                {
                    foreach (DataRow row in dtSupply.Select("p_custcode='" + custid + "'"))
                    {
                        strConcat += "<tr><td style='width:140px;'>" + row["sup_name"].ToString() + "</td><td style='width:30px;'>" + row["sup_from"].ToString() + "</td><td style='width:30px;'>" + row["sup_to"].ToString() + "</td></tr>";
                    }
                }
                if (strConcat.Length > 0)
                    strReturn.Append("<table cellspacing='0' rules='all' border='1' style='border-collapse: collapse;width:150px;background-color:#E1EE9E;color: #000;'>" + strConcat + "</table>");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strReturn.ToString();
        }

        public string Bind_TargetDetails(string custid)
        {
            StringBuilder strReturn = new StringBuilder();
            string strConcat = string.Empty;
            try
            {
                DataTable dtTarget = ViewState["dtTarget"] as DataTable;
                if (dtTarget != null && dtTarget.Rows.Count > 0)
                {
                    foreach (DataRow row in dtTarget.Select("custcode='" + custid + "'"))
                    {
                        strConcat += "<tr><td style='width:80px;'>" + row["TargetYear"].ToString() + "</td><td>" + row["TargetQty"].ToString() + "~" + row["TargetTon"].ToString() + "</td></tr>";
                    }
                }
                if (strConcat.Length > 0)
                    strReturn.Append("<table cellspacing='0' rules='all' border='1' style='border-collapse: collapse;width:100px;background-color:#AEEFE7;color: #000;'>" + strConcat + "</table>");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strReturn.ToString();
        }

        public string Bind_LeadHistoryDetails(string custcode)
        {
            StringBuilder strReturn = new StringBuilder();
            try
            {
                DataTable dtLeadHis = ViewState["dtLeadHis"] as DataTable;
                if (dtLeadHis != null && dtLeadHis.Rows.Count > 0)
                {
                    foreach (DataRow row in dtLeadHis.Select("custcode='" + custcode + "'"))
                    {
                        if (row["leadsfeedback"].ToString().Length > 100)
                            strReturn.Append("<div>" + row["leadsfeedback"].ToString().Substring(0, 100) + "..<span class='leadhistorymore' onclick=\"goingtosingleLeadList(\'" + custcode + "\');\">more</span></div>");
                        else
                            strReturn.Append("<div>" + row["leadsfeedback"].ToString() + "</div>");
                        strReturn.Append("<div><span>Led by: <b>" + row["username"].ToString() + "</b></span><span style='padding-left:10px;'>" + row["createddate"].ToString() + "</span></div>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strReturn.ToString();
        }




        #region[get_LeadCustDetails_WebMethod]
        [WebMethod]
        public static string get_LeadCustDetails(string strCustCode)
        {
            return GetLeadName_Details(strCustCode.ToString()).GetXml();
        }

        private static DataSet GetLeadName_Details(string strCustCode)
        {
            DataAccess daPORTWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                ds = (DataSet)daPORTWebMethod.ExecuteReader("select contactperson,mobileno,phoneno,webaddress,email,leadsfeedback,CONVERT(VARCHAR(19),createddate) as createddate,username from LeadDetails where custcode='" + strCustCode + "' order by ID desc", DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "prospectstatus.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[Get Attachment File WebMethod]
        [WebMethod]
        public static string Get_AllAttchment_CustWise(string strCustCode)
        {
            return GetFileList_FromCustFolder(strCustCode);
        }

        private static string GetFileList_FromCustFolder(string CustCode)
        {
            string strFileList = string.Empty;
            try
            {
                if (Directory.Exists(HttpContext.Current.Server.MapPath("~/leadfeedback/" + CustCode + "/")))
                {
                    string strFileName = string.Empty;
                    foreach (string d in Directory.GetFiles(HttpContext.Current.Server.MapPath("~/leadfeedback/" + CustCode + "/")))
                    {
                        string strName = d.Replace(HttpContext.Current.Server.MapPath("~/leadfeedback/" + CustCode + "/"), "");
                        string strURL = "/leadfeedback/" + CustCode + "/" + strName;
                        strFileName += "<li><span onclick='attachDownload(\"" + strURL + "\");' class='attachSpan'>" + strName + "</span></li>";
                    }
                    if (strFileName.Length > 0)
                        strFileList = "<ul style='list-style-position: inside;padding: 0;margin: 0;'>" + strFileName + "</ul>";
                }
            }
            catch (System.Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "prospectstatus.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strFileList;
        }
        #endregion
    }
}