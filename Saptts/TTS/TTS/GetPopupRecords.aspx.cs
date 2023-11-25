using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace TTS
{
    public partial class GetPopupRecords : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daPortDB = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        DataAccess daCOTSDB = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                if (Request["type"].ToString() == "getAllCustList")
                    Response.Write(Get_AllCustList_FromDB());
                else if (Request["type"].ToString() == "getAllRatesID")
                    Response.Write(Get_AllRatesID_FromDB());
                else if (Request["type"].ToString() == "getAllPriceSheetCustWise")
                    Response.Write(Get_AllPriceSheetList_CustWise_FromDB(Request["cCode"].ToString()));
                else if (Request["type"].ToString() == "getAllCompareCust")
                    Response.Write(Get_AllCompareCustList(Request["cname"].ToString(), Request["curr"].ToString()));
                else if (Request["type"].ToString() == "getPriceSheetNameWise")
                    Response.Write(Get_PriceSheetList_CustNameWise(Request["cname"].ToString(), Request["category"].ToString()));
                else if (Request["type"].ToString() == "getAllProspectCust")
                    Response.Write(Get_AllProspectCustList());
                else if (Request["type"].ToString() == "getprospectlikecust")
                    Response.Write(Get_ProspectCustomerName_List(Request["cname"].ToString()));
                else if (Request["type"].ToString() == "getprospectUserWiselikecust")
                    Response.Write(Get_UserWise_ProspectCustomerName_List(Request["cname"].ToString()));
                else if (Request["type"].ToString() == "chkprospectcust")
                    Response.Write(Verify_ProspectCustomerName(Request["cname"].ToString()));
                else if (Request["type"].ToString() == "getUserWiseProspectCust")
                    Response.Write(Get_UserWiseProspectCustList());
            }
        }

        private string Get_AllCustList_FromDB()
        {
            StringBuilder strAppend = new StringBuilder();
            try
            {
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_All_CustList", DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    strAppend.Append("<ul class='popupUL'>");
                    foreach (DataRow row in dt.Rows)
                    {
                        strAppend.Append("<li>" + row["custname"].ToString() + "</li>");
                    }
                    strAppend.Append("</ul>");
                }
                else
                {
                    strAppend.Append("");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAppend.ToString();
        }

        private string Get_AllPriceSheetList_CustWise_FromDB(string strCode)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtPrice = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@CustCode", strCode);

                dtPrice = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_All_PriceSheetRefNo_CustWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtPrice.Rows.Count > 0)
                {
                    details.Append("<ul class='popupUL'>");
                    foreach (DataRow dtrow in dtPrice.Rows)
                    {
                        details.Append("<li>" + dtrow["PriceSheetRefNo"].ToString() + "</li>");
                    }
                    details.Append("</ul>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_AllRatesID_FromDB()
        {
            StringBuilder strAppend = new StringBuilder();
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_All_RatesID", DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    strAppend.Append("<ul class='popupUL'>");
                    foreach (DataRow row in dt.Rows)
                    {
                        strAppend.Append("<li>" + row["RatesID"].ToString() + "</li>");
                    }
                    strAppend.Append("</ul>");
                }
                else
                {
                    strAppend.Append("");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAppend.ToString();
        }

        private string Get_AllCompareCustList(string strCustName, string strCurr)
        {
            StringBuilder strAppend = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custname", strCustName);
                sp1[1] = new SqlParameter("@PriceUnit", strCurr);

                DataTable dt = new DataTable();

                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CompareCustList", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    strAppend.Append("<ul class='popupUL'>");
                    foreach (DataRow row in dt.Rows)
                    {
                        strAppend.Append("<li>" + row["custname"].ToString() + "</li>");
                    }
                    strAppend.Append("</ul>");
                }
                else
                {
                    strAppend.Append("");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAppend.ToString();
        }

        private string Get_PriceSheetList_CustNameWise(string strCustName, string strCategory)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtPrice = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@CustName", strCustName);
                sp1[1] = new SqlParameter("@SizeCategory", strCategory);

                dtPrice = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_PriceSheetList_CustWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtPrice.Rows.Count > 0)
                {
                    details.Append("<ul class='popupUL'>");
                    foreach (DataRow dtrow in dtPrice.Rows)
                    {
                        details.Append("<li>" + dtrow["PriceSheetRefNo"].ToString() + "</li>");
                    }
                    details.Append("</ul>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_AllProspectCustList()
        {
            StringBuilder strAppend = new StringBuilder();
            try
            {
                DataTable dt = (DataTable)daPortDB.ExecuteReader_SP("Sp_Sel_All_ProspectCustList", DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    strAppend.Append("<ul class='popupUL'>");
                    foreach (DataRow row in dt.Rows)
                    {
                        strAppend.Append("<li>" + row["custname"].ToString() + "</li>");
                    }
                    strAppend.Append("</ul>");
                }
                else
                {
                    strAppend.Append("");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAppend.ToString();
        }

        private string Get_UserWiseProspectCustList()
        {
            StringBuilder strAppend = new StringBuilder();
            try
            {
                DataTable dt = new DataTable();

                if (Request.Cookies["TTSUserType"].Value.ToLower() == "admin" || Request.Cookies["TTSUserType"].Value.ToLower() == "support")
                    dt = (DataTable)daPortDB.ExecuteReader_SP("Sp_Sel_All_ProspectCustList", DataAccess.Return_Type.DataTable);
                else
                {
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@assignname", Request.Cookies["TTSUser"].Value);
                    dt = (DataTable)daPortDB.ExecuteReader_SP("Sp_Sel_UserWise_ProspectCustList", sp1, DataAccess.Return_Type.DataTable);
                }

                if (dt.Rows.Count > 0)
                {
                    strAppend.Append("<ul class='popupUL'>");
                    foreach (DataRow row in dt.Rows)
                    {
                        strAppend.Append("<li>" + row["custname"].ToString() + "</li>");
                    }
                    strAppend.Append("</ul>");
                }
                else
                {
                    strAppend.Append("");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAppend.ToString();
        }

        private string Get_ProspectCustomerName_List(string strLikeName)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custname", strLikeName);
                DataTable dt = new DataTable();
                dt = (DataTable)daPortDB.ExecuteReader_SP("Sp_Sel_Like_ProspectCustList", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().ToLower() != strLikeName.ToLower())
                    {
                        strApp.Append("<ul class='popupUL'>");
                        foreach (DataRow row in dt.Rows)
                        {
                            strApp.Append("<li>" + row["custname"].ToString() + "</li>");// onclick=\"javascript: Click_popupcustName('" + row["custname"].ToString() + "');\"
                        }
                        strApp.Append("</ul>");
                    }
                }
                else
                    strApp.Append("");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }

        private string Get_UserWise_ProspectCustomerName_List(string strLikeName)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                DataTable dt = new DataTable();
                if (Request.Cookies["TTSUserType"].Value.ToLower() == "admin" || Request.Cookies["TTSUserType"].Value.ToLower() == "support")
                {
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@custname", strLikeName);

                    dt = (DataTable)daPortDB.ExecuteReader_SP("Sp_Sel_Like_ProspectCustList", sp1, DataAccess.Return_Type.DataTable);
                }
                else
                {
                    SqlParameter[] sp2 = new SqlParameter[2];
                    sp2[0] = new SqlParameter("@custname", strLikeName);
                    sp2[1] = new SqlParameter("@assignname", Request.Cookies["TTSUser"].Value.ToLower());

                    dt = (DataTable)daPortDB.ExecuteReader_SP("Sp_Sel_Like_UserWise_ProspectCustList", sp2, DataAccess.Return_Type.DataTable);
                }
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().ToLower() != strLikeName.ToLower())
                    {
                        strApp.Append("<ul class='popupUL'>");
                        foreach (DataRow row in dt.Rows)
                        {
                            strApp.Append("<li>" + row["custname"].ToString() + "</li>");// onclick=\"javascript: Click_popupcustName('" + row["custname"].ToString() + "');\"
                        }
                        strApp.Append("</ul>");
                    }
                }
                else
                    strApp.Append("");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }

        private string Verify_ProspectCustomerName(string strName)
        {
            string msg = "success";
            try
            {
                SqlParameter[] sparam = new SqlParameter[1];

                sparam[0] = new SqlParameter("@CustName", strName);

                DataTable dt = new DataTable();
                dt = (DataTable)daPortDB.ExecuteReader_SP("Sp_chk_ProspectCustomerName", sparam, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                    msg = "exists";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return msg;
        }

    }
}