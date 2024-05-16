using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Web.Script.Serialization;
using System.IO;
using System.Globalization;

namespace TTS
{
    public partial class BindRecords : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                if (Request["type"].ToString() == "getRatesID")
                    Response.Write(Get_RatesID_List(Request["rid"].ToString()));
                else if (Request["type"].ToString() == "getPriceRef")
                    Response.Write(Get_PriceSheet_List(Request["cCode"].ToString(), Request["priceref"].ToString()));
                else if (Request["type"].ToString() == "getPriceRefAutorize")
                    Response.Write(Get_PriceSheet_AuthorizeList(Request["cCode"].ToString(), Request["priceref"].ToString()));
                else if (Request["type"].ToString() == "getPriceRefCustMaster")
                    Response.Write(Get_CustMaster_PriceSheet_List(Request["cCode"].ToString(), Request["priceref"].ToString()));
                else if (Request["type"].ToString() == "getApprovedTypeConfigWise")
                    Response.Write(Get_ApprovedType_List(Request["cCode"].ToString(), Request["config"].ToString(), Request["category"].ToString()));
                else if (Request["type"].ToString() == "getApprovedExceptTypeWise")
                    Response.Write(Get_ApprovedExcept_TypeList(Request["cCode"].ToString(), Request["config"].ToString(), Request["tyretype"].ToString(), Request["category"].ToString()));
                else if (Request["type"].ToString() == "getApprovedConfigCategoryWise")
                    Response.Write(Get_ApprovedConfig_CategoryWise(Request["cCode"].ToString(), Request["category"].ToString()));
                else if (Request["type"].ToString() == "getPremiumTypeVal")
                    Response.Write(Get_PremiumTypeVal_PriceSheetWise(Request["cCode"].ToString(), Request["config"].ToString(), Request["tyretype"].ToString(), Request["category"].ToString(), Request["PRefNo"].ToString(), Request["ratesID"].ToString()));
                else if (Request["type"].ToString() == "getBaseTypeVal")
                    Response.Write(Get_BaseTypeVal(Request["config"].ToString(), Request["cCode"].ToString(), Request["basetype"].ToString(), Request["PRefNo"].ToString()));
                else if (Request["type"].ToString() == "removeRptPriceData")
                    Response.Write(Remove_RptCustPriceList_TypeWise(Request["cCode"].ToString(), Request["priceref"].ToString(), Request["config"].ToString(), Request["tyretype"].ToString(), Request["disStatus"].ToString()));
                else if (Request["type"].ToString() == "getPriceRefCustWise")
                    Response.Write(Get_PriceSheet_List_CustNameWise(Request["cCname"].ToString(), Request["priceref"].ToString(), Request["category"].ToString()));
                else if (Request["type"].ToString() == "Insert_update_to_Lead")
                    Response.Write(Insert_Update_Feedback_to_Lead(Request["updatemsg"].ToString(), Request["custcode"].ToString()));
                else if (Request["type"].ToString() == "update_to_ProspectFocus")
                    Response.Write(Update_Focus_to_ProspectMaster(Request["changeFocus"].ToString(), Request["custcode"].ToString()));
                else if (Request["type"].ToString() == "update_to_ProspectFlag")
                    Response.Write(Update_Flag_to_ProspectMaster(Request["changeFlag"].ToString(), Request["custcode"].ToString()));
                else if (Request["type"].ToString() == "ChkWorkOrderNo")
                    Response.Write(Check_WorkOrderNo(Request["wid"].ToString()));
                else if (Request["type"].ToString() == "ChkProformaNo")
                    Response.Write(Check_ProformaRefNo(Request["ProfromaId"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "delLrCopy")
                    Response.Write(Del_LrCopy(Request["LrCopyName"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "ChkSubProcessID")
                    Response.Write(Check_Substitution_ProcessID());
                else if (Request["type"] != null && Request["type"].ToString() == "ChkCommercialProcessID")
                    Response.Write(Check_ChkCommercialProcessID_ProcessID());
                else if (Request["type"].ToString() == "getTypeConfigWise")
                    Response.Write(Get_TypeList_ConfigWise(Request["config"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "ChkOrderNo")
                    Response.Write(chk_orderrefno(Request["chkrefno"].ToString(), Request["cotscode"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "Cancel_RoomReserve")
                    Response.Write(Cancel_ReservedMeetingRoom(Request["rDate"].ToString(), Request["rName"].ToString(), Request["rRoom"].ToString(), Request["rSno"].ToString(), Request["rTno"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "delExpDoc")
                    Response.Write(Del_ExportDocumentFile(Request["expfilename"].ToString(), Request["docname"].ToString()));
                else if (Request["type"].ToString() == "ChkCotsUserName")
                    Response.Write(Check_CotsUsername(Request["userId"].ToString()));
                else if (Request["type"].ToString() == "ChkppcOrderEntry")
                    Response.Write(Check_PPCORDEREnter(Request["orderno"].ToString(), Request["Plant"].ToString(), Request["custcode"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "getAddress")
                    Response.Write(Get_Address_CustChoice(Request["custcode"].ToString(), Request["addid"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "getFilterCategories")
                    Response.Write(getFilterCategory(Request["stocktype"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "delstencilFailure")
                    Response.Write(RemoveStencilFailureImages());
                else if (Request["type"] != null && Request["type"].ToString() == "getClaimSize")
                    Response.Write(Get_ClaimCust_TyreSize(Request["Brand"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "getClaimType")
                    Response.Write(Get_ClaimCust_TyreType(Request["Brand"].ToString(), Request["Size"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "chkClaimstencil")
                    Response.Write(Chk_ClaimStencilno(Request["claimstencilno"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "chkexpwo")
                    Response.Write(chk_exp_wo(Request["plant"].ToString(), Request["workorderno"].ToString()));
            }
            if (Request["type"].ToString() == "GetCustomerspage")
                Response.Write(GetCustomers(Request["CustName"].ToString(), Request["country"].ToString(), Request["City"].ToString(), Request["flag"].ToString(), Request["port"].ToString(), Request["focus"].ToString(), Request["Supplier"].ToString(), Request["LeadSource"].ToString()));
            else if (Request["type"].ToString() == "GetAssignList")
                Response.Write(GetCustomersAssignList(Request["CustName"].ToString(), Request["country"].ToString(), Request["City"].ToString(), Request["flag"].ToString(), Request["port"].ToString(), Request["focus"].ToString(), Request["Lead"].ToString(), Request["Supplier"].ToString(), Request["LeadSource"].ToString()));
            else if (Request["type"].ToString() == "GetReviewAll")
                Response.Write(GetCustomersReviewAll(Request["CustName"].ToString(), Request["country"].ToString(), Request["City"].ToString(), Request["flag"].ToString(), Request["port"].ToString(), Request["focus"].ToString(), Request["Lead"].ToString(), Request["Supplier"].ToString(), Request["LeadSource"].ToString()));
            else if (Request["type"].ToString() == "Getreviewdom")
            {
                string supervisor = "ALL", manager = "ALL";
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                {
                    if (Request.Cookies["TTSUserType"].Value.ToLower() == "supervisor")
                        supervisor = Request.Cookies["TTSUser"].Value;
                    else if (Request.Cookies["TTSUserType"].Value.ToLower() == "manager")
                        manager = Request.Cookies["TTSUser"].Value;
                }
                Response.Write(GetCustomersReviewdom(Request["CustName"].ToString(), Request["City"].ToString(), Request["flag"].ToString(), Request["port"].ToString(), Request["focus"].ToString(), Request["Lead"].ToString(), supervisor, manager, Request["Supplier"].ToString(), Request["LeadSource"].ToString()));
            }
            else if (Request["type"].ToString() == "GetSatusLead")
                Response.Write(GetCustomersStausLead(Request["CustName"].ToString(), Request["country"].ToString(), Request["City"].ToString(), Request["flag"].ToString(), Request["port"].ToString(), Request["focus"].ToString(), Request.Cookies["TTSUser"].Value, Request["Supplier"].ToString(), Request["LeadSource"].ToString()));
            else if (Request["type"] != null && Request["type"].ToString() == "delClaimImg")
                Response.Write(Del_ClaimImages(Request["claimimgname"].ToString()));
        }
        public static string GetCustomersStausLead(string CustName, string country, string City, string flag, string port, string focus, string Lead, string Supplier, string LeadSource)
        {
            DataAccess daPORTWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
            string str = string.Empty;
            SqlParameter[] sp1 = new SqlParameter[9];
            sp1[0] = new SqlParameter("@custcode", CustName);
            sp1[1] = new SqlParameter("@country", country);
            sp1[2] = new SqlParameter("@City", City);
            sp1[3] = new SqlParameter("@flag", flag);
            sp1[4] = new SqlParameter("@port", port);
            sp1[5] = new SqlParameter("@focus", focus);
            sp1[6] = new SqlParameter("@assignto", Lead);
            sp1[7] = new SqlParameter("@Supplier_Name", Supplier);
            sp1[8] = new SqlParameter("@LeadSource", LeadSource);
            DataTable dtCustList = (DataTable)daPORTWebMethod.ExecuteReader_SP("sp_sel_LeadStatus", sp1, DataAccess.Return_Type.DataTable);
            if (dtCustList.Rows.Count > 0)
                str = Utilities.Serialization(dtCustList);
            else
                str = "No Record";
            return str;

        }
        public static string GetCustomersReviewAll(string CustName, string country, string City, string flag, string port, string focus, string Lead, string Supplier, string LeadSource)
        {
            DataAccess daPORTWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
            string str = string.Empty;
            SqlParameter[] sp1 = new SqlParameter[9];
            sp1[0] = new SqlParameter("@custcode", CustName);
            sp1[1] = new SqlParameter("@country", country);
            sp1[2] = new SqlParameter("@City", City);
            sp1[3] = new SqlParameter("@flag", flag);
            sp1[4] = new SqlParameter("@port", port);
            sp1[5] = new SqlParameter("@focus", focus);
            sp1[6] = new SqlParameter("@assignto", Lead);
            sp1[7] = new SqlParameter("@Supplier_Name", Supplier);
            sp1[8] = new SqlParameter("@LeadSource", LeadSource);
            DataTable dtCustList = (DataTable)daPORTWebMethod.ExecuteReader_SP("sp_sel_ReviseAll", sp1, DataAccess.Return_Type.DataTable);
            if (dtCustList.Rows.Count > 0)
                str = Utilities.Serialization(dtCustList);
            else
                str = "No Record";
            return str;
        }

        public static string GetCustomersReviewdom(string CustName, string City, string flag, string port, string focus, string Lead, string supervisor, string manager, string Supplier, string LeadSource)
        {
            DataAccess daPORTWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
            string str = string.Empty;
            SqlParameter[] sp1 = new SqlParameter[10];
            sp1[0] = new SqlParameter("@custcode", CustName);
            sp1[1] = new SqlParameter("@City", City);
            sp1[2] = new SqlParameter("@flag", flag);
            sp1[3] = new SqlParameter("@port", port);
            sp1[4] = new SqlParameter("@focus", focus);
            sp1[5] = new SqlParameter("@assignto", Lead);
            sp1[6] = new SqlParameter("@supervisor", supervisor);
            sp1[7] = new SqlParameter("@manager", manager);
            sp1[8] = new SqlParameter("@Supplier_Name", Supplier);
            sp1[9] = new SqlParameter("@LeadSource", LeadSource);
            DataTable dtCustList = (DataTable)daPORTWebMethod.ExecuteReader_SP("Sp_sel_ReviewDom", sp1, DataAccess.Return_Type.DataTable);
            if (dtCustList.Rows.Count > 0)
                str = Utilities.Serialization(dtCustList);
            else
                str = "No Record";
            return str;

        }
        public static string GetCustomersAssignList(string CustName, string country, string City, string flag, string port, string focus, string Lead, string Supplier, string LeadSource)
        {
            DataAccess daPORTWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
            string str = string.Empty;
            SqlParameter[] sp1 = new SqlParameter[9];
            sp1[0] = new SqlParameter("@custcode", CustName);
            sp1[1] = new SqlParameter("@country", country);
            sp1[2] = new SqlParameter("@City", City);
            sp1[3] = new SqlParameter("@flag", flag);
            sp1[4] = new SqlParameter("@port", port);
            sp1[5] = new SqlParameter("@focus", focus);
            sp1[6] = new SqlParameter("@assignto", Lead);
            sp1[7] = new SqlParameter("@Supplier_Name", Supplier);
            sp1[8] = new SqlParameter("@LeadSource", LeadSource);
            DataTable dtCustList = (DataTable)daPORTWebMethod.ExecuteReader_SP("Sp_sel_AssignCrmList", sp1, DataAccess.Return_Type.DataTable);
            if (dtCustList.Rows.Count > 0)
                str = Utilities.Serialization(dtCustList);
            else
                str = "No Record";
            return str;
        }

        private string Get_Address_CustChoice(string custcode, string strID)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", custcode);
                sp1[1] = new SqlParameter("@shipid", Convert.ToInt32(strID));

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ExportAddress_IDwise", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    strApp.Append("<span style='line-height: 13px;float: left;width: 500px;'>");
                    strApp.Append("<span style='float: left;width: 500px;'><span style='font-weight:bold;'>M/S. " + row["CompanyName"].ToString().ToUpper() + "</span></span>");
                    strApp.Append("<span style='float: left;width: 500px;word-break: break-word;'>" + row["shipaddress"].ToString().Replace("~", "&nbsp;&nbsp;") + "</span>");
                    strApp.Append("<span style='float: left;width: 500px;'>" + row["city"].ToString() + "    " + row["statename"].ToString() + "</span>");
                    strApp.Append("<span>" + row["country"].ToString() + "    " + row["zipcode"].ToString() + "</span>");
                    strApp.Append("</span>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }

        public static string GetCustomers(string CustName, string country, string City, string flag, string port, string focus, string Supplier, string LeadSource)
        {
            DataAccess daPORTWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
            string str = string.Empty;
            SqlParameter[] sp1 = new SqlParameter[8];
            sp1[0] = new SqlParameter("@custcode", CustName);
            sp1[1] = new SqlParameter("@country", country);
            sp1[2] = new SqlParameter("@City", City);
            sp1[3] = new SqlParameter("@flag", flag);
            sp1[4] = new SqlParameter("@port", port);
            sp1[5] = new SqlParameter("@focus", focus);
            sp1[6] = new SqlParameter("@Supplier_Name", Supplier);
            sp1[7] = new SqlParameter("@LeadSource", LeadSource);
            DataTable dtCustList = (DataTable)daPORTWebMethod.ExecuteReader_SP("Sp_sel_UnAssignCrmList", sp1, DataAccess.Return_Type.DataTable);
            if (dtCustList.Rows.Count > 0)
                str = Utilities.Serialization(dtCustList);
            else
                str = "No Record";
            return str;
        }

        private string Get_RatesID_List(string ratesID)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtRates = new DataTable();

                dtRates = Utilities.GetRatesIDList(HttpContext.Current, ratesID);
                if (dtRates.Rows.Count > 0)
                {
                    if (dtRates.Rows[0][0].ToString().ToLower() != ratesID.ToLower())
                    {
                        details.Append("<ul class='popupUL'>");
                        foreach (DataRow dtrow in dtRates.Rows)
                        {
                            details.Append("<li>" + dtrow["RatesID"].ToString() + "</li>");
                        }
                        details.Append("</ul>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_PriceSheet_List(string strCode, string strpriceref)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtPrice = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@SheetRefNo", strpriceref);
                sp1[1] = new SqlParameter("@CustCode", strCode);

                dtPrice = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Like_PriceSheetRefNo", sp1, DataAccess.Return_Type.DataTable);
                if (dtPrice.Rows.Count > 0)
                {
                    if (dtPrice.Rows[0][0].ToString().ToLower() != strpriceref.ToLower())
                    {
                        details.Append("<ul class='popupUL'>");
                        foreach (DataRow dtrow in dtPrice.Rows)
                        {
                            details.Append("<li>" + dtrow["PriceSheetRefNo"].ToString() + "</li>");
                        }
                        details.Append("</ul>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_PriceSheet_AuthorizeList(string strCode, string strpriceref)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtPrice = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@SheetRefNo", strpriceref);
                sp1[1] = new SqlParameter("@CustCode", strCode);

                dtPrice = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Like_PriceSheetRefNo_AuthorizeOnly", sp1, DataAccess.Return_Type.DataTable);
                if (dtPrice.Rows.Count > 0)
                {
                    if (dtPrice.Rows[0][0].ToString().ToLower() != strpriceref.ToLower())
                    {
                        details.Append("<ul class='popupUL'>");
                        foreach (DataRow dtrow in dtPrice.Rows)
                        {
                            details.Append("<li>" + dtrow["PriceSheetRefNo"].ToString() + "</li>");
                        }
                        details.Append("</ul>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_CustMaster_PriceSheet_List(string strCode, string strpriceref)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtPrice = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@SheetRefNo", strpriceref);
                sp1[1] = new SqlParameter("@CustCode", strCode);

                dtPrice = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Like_PriceSheetRefNo_Custmaster", sp1, DataAccess.Return_Type.DataTable);
                if (dtPrice.Rows.Count > 0)
                {
                    if (dtPrice.Rows[0][0].ToString().ToLower() != strpriceref.ToLower())
                    {
                        details.Append("<ul class='popupUL'>");
                        foreach (DataRow dtrow in dtPrice.Rows)
                        {
                            details.Append("<li>" + dtrow["PriceSheetRefNo"].ToString() + "</li>");
                        }
                        details.Append("</ul>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_ApprovedType_List(string strCode, string strPlatform, string strCategory)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtType = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@Config", strPlatform);
                sp1[1] = new SqlParameter("@CustCode", strCode);
                sp1[2] = new SqlParameter("@SizeCategory", strCategory);

                dtType = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_ApprovedType_CustWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtType.Rows.Count > 0)
                {
                    details.Append("<option value='CHOOSE'>CHOOSE</option>");
                    foreach (DataRow dtrow in dtType.Rows)
                    {
                        details.Append("<option value='" + dtrow["Tyretype"].ToString() + "'>" + dtrow["Tyretype"].ToString() + "</option>");
                    }

                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_ApprovedExcept_TypeList(string strCode, string strPlatform, string strType, string strCategory)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtType = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@Config", strPlatform);
                sp1[1] = new SqlParameter("@CustCode", strCode);
                sp1[2] = new SqlParameter("@TyreType", strType);
                sp1[3] = new SqlParameter("@SizeCategory", strCategory);

                dtType = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_ApprovedExceptType_CustWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtType.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dtType.Rows)
                    {
                        details.Append(dtrow["Tyretype"].ToString() + "~");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_ApprovedConfig_CategoryWise(string strCode, string strCategory)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtConfig = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@SizeCategory", strCategory);
                sp1[1] = new SqlParameter("@CustCode", strCode);

                dtConfig = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_ApprovedConfig_CustWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtConfig.Rows.Count > 0)
                {
                    details.Append("<option value='CHOOSE'>CHOOSE</option>");
                    foreach (DataRow dtrow in dtConfig.Rows)
                    {
                        details.Append("<option value='" + dtrow["Config"].ToString() + "'>" + dtrow["Config"].ToString() + "</option>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_PremiumTypeVal_PriceSheetWise(string strCode, string strPlatform, string strType, string strCategory, string strPRefNo, string strRatesID)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtPreVal = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@Config", strPlatform);
                sp1[1] = new SqlParameter("@CustCode", strCode);
                sp1[2] = new SqlParameter("@TyreType", strType);
                sp1[3] = new SqlParameter("@SizeCategory", strCategory);
                sp1[4] = new SqlParameter("@PRefNo", strPRefNo);
                sp1[5] = new SqlParameter("@RatesID", strRatesID);

                dtPreVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_PremiumTypeVal_PRefNoWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtPreVal.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dtPreVal.Select("PremiumType='" + strType + "'"))
                    {
                        details.Append(dtrow["PremiumType"].ToString() + ":" + dtrow["PremiumValue"].ToString() + "~");
                    }

                    foreach (DataRow dtrow in dtPreVal.Select("PremiumType<>'" + strType + "'", "PremiumType ASC"))
                    {
                        details.Append(dtrow["PremiumType"].ToString() + ":" + dtrow["PremiumValue"].ToString() + "~");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Remove_RptCustPriceList_TypeWise(string strCode, string strPriceRef, string strConfig, string strtyreType, string disStatus)
        {
            string returnMsg = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@CustCode", strCode);
                sp1[1] = new SqlParameter("@PriceSheetRefNo", strPriceRef);
                sp1[2] = new SqlParameter("@Config", strConfig);
                sp1[3] = new SqlParameter("@TyreType", strtyreType);
                sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp1[5] = new SqlParameter("@DisplayStatus", Convert.ToBoolean(disStatus));

                returnMsg = daTTS.ExecuteNonQuery_SP("Sp_Del_RptPriceMaster_TypeWise", sp1).ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnMsg;
        }

        private string Get_PriceSheet_List_CustNameWise(string strCustName, string strpriceref, string strCategory)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtPrice = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@SheetRefNo", strpriceref);
                sp1[1] = new SqlParameter("@CustName", strCustName);
                sp1[2] = new SqlParameter("@SizeCategory", strCategory);

                dtPrice = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Like_PriceSheetRefNo_CustNameWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtPrice.Rows.Count > 0)
                {
                    if (dtPrice.Rows[0][0].ToString().ToLower() != strpriceref.ToLower())
                    {
                        details.Append("<ul class='popupUL'>");
                        foreach (DataRow dtrow in dtPrice.Rows)
                        {
                            details.Append("<li>" + dtrow["PriceSheetRefNo"].ToString() + "</li>");
                        }
                        details.Append("</ul>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string Get_BaseTypeVal(string strConfig, string strCode, string strBaseType, string strPriceRef)
        {
            string returnMsg = "";
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@Config", strConfig);
                sp1[1] = new SqlParameter("@CustCode", strCode);
                sp1[2] = new SqlParameter("@BaseType", strBaseType);
                sp1[3] = new SqlParameter("@PRefNo", strPriceRef);

                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_BaseValue_CustPriceSheet", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count == 1)
                    returnMsg = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnMsg;
        }

        private string Insert_Update_Feedback_to_Lead(string strUpdateMsg, string strCustCode)
        {
            string reMsg = "fail";
            try
            {
                int resp = 0;
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", strCustCode);
                sp1[1] = new SqlParameter("@UpdateMsg", strUpdateMsg);
                sp1[2] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);

                resp = daPORT.ExecuteNonQuery_SP("Sp_Ins_UpdateMsgToLead", sp1);
                if (resp == 1)
                    reMsg = "Feedback saved successfully";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reMsg;
        }

        private string Update_Focus_to_ProspectMaster(string strFocus, string strCustCode)
        {
            string reMsg = "fail";
            try
            {
                int resp = 0;
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", strCustCode);
                sp1[1] = new SqlParameter("@Focus", strFocus.Replace(" ", "+"));
                sp1[2] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);

                resp = daPORT.ExecuteNonQuery_SP("Sp_Edit_ProspectFocus", sp1);
                if (resp > 0)
                    reMsg = "Focus changed successfully";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reMsg;
        }

        private string Update_Flag_to_ProspectMaster(string strFlag, string strCustCode)
        {
            string reMsg = "fail";
            try
            {
                int resp = 0;
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", strCustCode);
                sp1[1] = new SqlParameter("@Flag", strFlag);
                sp1[2] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);

                resp = daPORT.ExecuteNonQuery_SP("Sp_Edit_ProspectFlag", sp1);
                if (resp > 0)
                    reMsg = "Flag changed successfully";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PROTDB", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reMsg;
        }

        private string Check_ProformaRefNo(string strProformano)
        {
            string reMsg = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@proformarefno", strProformano);

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_Chk_ProformaRefNo", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0 && dt.Rows[0]["proformarefno"].ToString() == strProformano)
                    reMsg = "Already existing this proforma ref no.";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-ORDERDB", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reMsg;
        }

        private string Check_WorkOrderNo(string strworkno)
        {
            string reMsg = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@workorderno", strworkno);

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_Chk_WorkOrderNO", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0 && dt.Rows[0]["workorderno"].ToString() == strworkno)
                    reMsg = "Already existing this work order no.";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-ORDERDB", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reMsg;
        }

        private string Del_LrCopy(string strImg)
        {
            string strApp = string.Empty;
            try
            {
                string strLrDetails = Session["lrcopydetails"].ToString();
                string[] strSplit = strLrDetails.Split('~');
                string[] str1 = strImg.Split('.');
                string strExtension = str1[str1.Length - 1].ToString();
                string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/lrcopyfiles/" + strSplit[0].ToString() + "/LR_" + strSplit[1].ToString() + "." + strExtension;
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Delete Lr File: " + ex.Message);
                strApp = ex.Message;
            }
            return strApp;
        }

        private string Check_Substitution_ProcessID()
        {
            string reMsg = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[7];
                sp1[0] = new SqlParameter("@config", Request["config"].ToString());
                sp1[1] = new SqlParameter("@tyresize", Request["size"].ToString());
                sp1[2] = new SqlParameter("@rimsize", Request["rim"].ToString());
                sp1[3] = new SqlParameter("@tyretype", Request["tyretype"].ToString());
                sp1[4] = new SqlParameter("@brand", Request["brand"].ToString());
                sp1[5] = new SqlParameter("@sidewall", Request["sidewall"].ToString());
                sp1[6] = new SqlParameter("@CustCode", Request["code"].ToString());

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_chkProcessID", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count == 0)
                    reMsg = "Process-ID not available";
                else
                    reMsg = dt.Rows[0]["ProcessID"].ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-ORDERDB", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reMsg;
        }
        private string Check_ChkCommercialProcessID_ProcessID()
        {
            string reMsg = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@config", Request["config"].ToString());
                sp1[1] = new SqlParameter("@tyresize", Request["size"].ToString());
                sp1[2] = new SqlParameter("@rimsize", Request["rim"].ToString());
                sp1[3] = new SqlParameter("@tyretype", Request["tyretype"].ToString());
                sp1[4] = new SqlParameter("@brand", Request["brand"].ToString());
                sp1[5] = new SqlParameter("@sidewall", Request["sidewall"].ToString());

                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_chk_CommercialProcessID", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count == 0)
                    reMsg = "Process-ID not available";
                else
                    reMsg = dt.Rows[0]["ProcessID"].ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-TTSDB", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reMsg;
        }

        private string Get_TypeList_ConfigWise(string strPlatform)
        {
            StringBuilder details = new StringBuilder();
            try
            {
                DataTable dtType = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@config", strPlatform);

                dtType = (DataTable)daTTS.ExecuteReader_SP("Sp_sel_Type_From_ProcessIDMaster", sp1, DataAccess.Return_Type.DataTable);
                if (dtType.Rows.Count > 0)
                {
                    details.Append("<option value='CHOOSE'>CHOOSE</option>");
                    foreach (DataRow dtrow in dtType.Rows)
                    {
                        details.Append("<option value='" + dtrow["TyreType"].ToString() + "'>" + dtrow["TyreType"].ToString() + "</option>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return details.ToString();
        }

        private string chk_orderrefno(string refno, string strcotscode)
        {
            string returnValue = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", strcotscode);
                sp1[1] = new SqlParameter("@orderrefno", refno);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_orderrefno", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0 && dt.Rows[0]["OrderRefNo"].ToString() != "")
                    returnValue = "ORDER NO ALREADY ENTERED ON " + dt.Rows[0]["CompletedDate"].ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnValue.ToString();
        }

        private string Cancel_ReservedMeetingRoom(string strRDate, string strRName, string strRoomID, string strFSno, string strTSno)
        {
            DataAccess daErrDB = new DataAccess(ConfigurationManager.ConnectionStrings["ErrDB"].ConnectionString);
            string returnMsg = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@ClosedBy", Request.Cookies["TTSUser"].Value);
                sp1[1] = new SqlParameter("@ReservedDate", DateTime.ParseExact(strRDate.Replace("-", "/"), "dd/MM/yyyy", CultureInfo.InvariantCulture));
                sp1[2] = new SqlParameter("@ReservedBy", strRName);
                sp1[3] = new SqlParameter("@RoomID", Convert.ToInt32(strRoomID));
                sp1[4] = new SqlParameter("@FSno", Convert.ToInt32(strFSno));
                sp1[5] = new SqlParameter("@TSno", Convert.ToInt32(strTSno));

                int resp = daErrDB.ExecuteNonQuery_SP("sp_cancel_RoomReservations", sp1);
                if (resp > 0)
                {
                    try
                    {
                        string mailConcat = string.Empty;
                        mailConcat += "Dear Sir/Madam,<br/><br/>";
                        mailConcat += "Your Room Cancelled Details:<br/>";
                        mailConcat += "Date: " + strRDate + "<br/>";
                        mailConcat += "Cancelled By: " + Request.Cookies["TTSUser"].Value + "<br/>";

                        string strMailID = Request.Cookies["TTSUserEmail"].Value;
                        if (!string.IsNullOrEmpty(strMailID))
                        {
                            YmailSender es = new YmailSender();
                            es.From = "cityoffice_reservations@sun-tws.com";
                            es.To = strMailID;
                            es.CC = "operations@sun-tws.com";
                            es.Password = "$GCm#8g1";
                            es.Subject = "CITY OFFICE MEETING ROOM REGISTRATION CANCELLED: " + strRDate;
                            es.Body = mailConcat + "<br/><br/><br/><span style='color:#d34639;'>This is a system generated mail. Please do not reply to this email ID.</span>";
                            es.IsHtmlBody = true;
                            es.EmailProvider = YmailSender.EmailProviderType.Gmail;
                            es.Send();
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Room Cancel Mail Error: " + ex.Message);
                    }
                    returnMsg = "SUCCESS";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnMsg;
        }

        private string Del_ExportDocumentFile(string strFile, string strDocName)
        {
            string strApp = string.Empty;
            try
            {
                string strExpDetails = Session["expdocdetails"].ToString();
                string[] strSplit = strExpDetails.Split('~');
                string[] str1 = strFile.Split('.');
                string strExtension = str1[str1.Length - 1].ToString();
                string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/exportdocuments/" + strSplit[0].ToString() + "/" + strSplit[1].ToString() + "/" + strDocName + "." + strExtension;
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Delete Image File: " + ex.Message);
                strApp = ex.Message;
            }
            return strApp;
        }

        private string Check_CotsUsername(string strProformano)
        {
            string reMsg = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@username", strProformano);

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_Chk_CotsUserName", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0 && dt.Rows[0]["username"].ToString() == strProformano)
                    reMsg = "Already existing this username to " + dt.Rows[0]["custfullname"].ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-ORDERDB", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reMsg;
        }
        private string Check_PPCORDEREnter(string orderno, string plant, string custcode)
        {
            string reMsg = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", custcode);
                sp1[1] = new SqlParameter("@Plant", plant);
                sp1[2] = new SqlParameter("@OrderNo", orderno);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_Chk_ppcorderentry", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                    reMsg = "Already existing this username and plant and orderno";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-ORDERDB", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return reMsg;
        }
        private string Del_ClaimImages(string strImg)
        {
            string strApp = string.Empty;
            try
            {
                string strClaimCode = Session["ClaimCustCode"].ToString();
                string strClaimNo = Session["ClaimNo"].ToString();
                string strClaimStencil = Session["ClaimStencil"].ToString();
                string serverURL = Server.MapPath("~/").Replace("SCOTS", "TTS").Replace("COTS", "TTS");
                string path = serverURL + "/claimimages/" + strClaimCode + "/" + strClaimNo + "/" + strClaimStencil + "/" + strImg;
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("SCOTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Delete Image File: " + ex.Message);
                strApp = ex.Message;
            }
            return strApp;
        }
        private string getFilterCategory(string strPlant)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@plant", strPlant) };
                DataTable dtFilterCategory = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_StockFilteredReport_Categories", sp, DataAccess.Return_Type.DataTable);
                if (dtFilterCategory.Rows.Count > 0)
                {
                    List<string> lstConfig = dtFilterCategory.AsEnumerable().Where(n => n.Field<string>("config") != "" && n.Field<string>("config") != null).OrderBy(n => n.Field<string>("config")).Select(n => n.Field<string>("config")).Distinct().ToList();
                    List<string> rimsize = dtFilterCategory.AsEnumerable().Where(n => n.Field<string>("rimsize") != "" && n.Field<string>("rimsize") != null).OrderBy(n => n.Field<string>("rimsize")).Select(n => n.Field<string>("rimsize")).Distinct().ToList();
                    List<string> tyretype = dtFilterCategory.AsEnumerable().Where(n => n.Field<string>("tyretype") != "" && n.Field<string>("tyretype") != null).OrderBy(n => n.Field<string>("tyretype")).Select(n => n.Field<string>("tyretype")).Distinct().ToList();
                    List<string> brand = dtFilterCategory.AsEnumerable().Where(n => n.Field<string>("brand") != "" && n.Field<string>("brand") != null).OrderBy(n => n.Field<string>("brand")).Select(n => n.Field<string>("brand")).Distinct().ToList();
                    List<string> sidewall = dtFilterCategory.AsEnumerable().Where(n => n.Field<string>("sidewall") != "" && n.Field<string>("sidewall") != null).OrderBy(n => n.Field<string>("sidewall")).Select(n => n.Field<string>("sidewall")).Distinct().ToList();
                    List<string> grade = dtFilterCategory.AsEnumerable().Where(n => n.Field<string>("grade") != "" && n.Field<string>("grade") != null).OrderBy(n => n.Field<string>("grade")).Select(n => n.Field<string>("grade")).Distinct().ToList();
                    List<string> year = dtFilterCategory.AsEnumerable().Where(n => n.Field<string>("yom") != "" && n.Field<string>("yom") != null).OrderBy(n => n.Field<string>("yom")).Select(n => n.Field<string>("yom")).Distinct().ToList();
                    List<string> stocktype = dtFilterCategory.AsEnumerable().Where(n => n.Field<string>("stocktype") != "" && n.Field<string>("stocktype") != null).OrderBy(n => n.Field<string>("stocktype")).Select(n => n.Field<string>("stocktype")).Distinct().ToList();
                    List<string> tyresize = dtFilterCategory.AsEnumerable().Where(n => n.Field<string>("tyresize") != "" && n.Field<string>("tyresize") != null).OrderBy(n => n.Field<int>("position")).Select(n => n.Field<string>("tyresize")).Distinct().ToList();

                    var jsonSerializer = new JavaScriptSerializer();
                    string jsonString = "{ \"config\" :" + jsonSerializer.Serialize(lstConfig);
                    jsonString += ", \"rimsize\" :" + jsonSerializer.Serialize(rimsize);
                    jsonString += ", \"tyretype\" :" + jsonSerializer.Serialize(tyretype);
                    jsonString += ", \"brand\" :" + jsonSerializer.Serialize(brand);
                    jsonString += ", \"sidewall\" :" + jsonSerializer.Serialize(sidewall);
                    jsonString += ", \"grade\" :" + jsonSerializer.Serialize(grade);
                    jsonString += ", \"year\" :" + jsonSerializer.Serialize(year);
                    jsonString += ", \"stocktype\" :" + jsonSerializer.Serialize(stocktype);
                    jsonString += ", \"tyresize\" :" + jsonSerializer.Serialize(tyresize) + " }";

                    return jsonString;
                }
                return "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        private string RemoveStencilFailureImages()
        {
            try
            {
                string path = Request.QueryString["path"].ToString();
                path = Server.MapPath("~/") + path;
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);

            }
            return "";
        }
        private string Get_ClaimCust_TyreSize(string strBrand)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@brand", strBrand);

                //DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_publishedsheet_tyresize", sp1, DataAccess.Return_Type.DataTable);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_tyresize_BrandWise", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    strApp.Append("<option value='Choose'>Choose</option>");
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        strApp.Append("<option value='" + dtrow["TyreSize"].ToString() + "'>" + dtrow["TyreSize"].ToString() + "</option>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }
        private string Get_ClaimCust_TyreType(string strBrand, string strSize)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@brand", strBrand);
                sp1[1] = new SqlParameter("@TyreSize", strSize);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_tyreType_Brand_Size_Wise", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    strApp.Append("<option value='Choose'>Choose</option>");
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        strApp.Append("<option value='" + dtrow["TyreType"].ToString() + "'>" + dtrow["TyreType"].ToString() + "</option>");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }
        private string Chk_ClaimStencilno(string stencilno)
        {
            string strApp = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@stencilno", stencilno);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_stencilno", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                    strApp = "STENCIL NO. ALREADY EXISTING";
                else
                {
                    sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@stencilno", stencilno);
                    DataTable dt1 = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_stencilno_olddata", sp1, DataAccess.Return_Type.DataTable);
                    if (dt1.Rows.Count > 0)
                        strApp = "STENCIL NO. ALREADY EXISTING";
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }
        private string chk_exp_wo(string Plant, string Workorderno)
        {
            try
            {
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Plant", Plant), new SqlParameter("@workorderno", Workorderno) };
                    string strMsg = (string)daCOTS.ExecuteScalar_SP("sp_chk_exp_exists_workorderno", sp);
                    return strMsg;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return "Error";
        }
    }
}