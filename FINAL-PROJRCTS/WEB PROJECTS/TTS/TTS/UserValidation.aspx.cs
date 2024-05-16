using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Web.Security;
using System.Text;

namespace TTS
{
    public partial class UserValidation : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                if (Request["cname"] != null || Request["cname"].ToString() != "")
                {
                    if (Request["type"].ToString() == "chkcust")
                        Response.Write(Verify_CustomerName(Request["cname"].ToString(), Request["custtype"].ToString()));
                    else if (Request["type"].ToString() == "getcust")
                        Response.Write(Get_CustomerName_List(Request["cname"].ToString()));
                    else if (Request["type"].ToString() == "custdetails")
                        Response.Write(GetCustomer_Details(Request["cname"].ToString()));
                    else if (Request["type"].ToString() == "getCompareCust")
                        Response.Write(GetCompareCustomer_Details(Request["cname"].ToString(), Request["curr"].ToString(), Request["likeCust"].ToString()));
                }
            }
        }
        private string Verify_CustomerName(string strName, string strCustType)
        {
            string msg = "success";
            try
            {
                SqlParameter[] sparam = new SqlParameter[1];
                sparam[0] = new SqlParameter("@Name", strName);
                DataTable dt = new DataTable();

                if (strCustType == "Exist")
                    dt = (DataTable)daTTS.ExecuteReader_SP("Sp_chk_CustomerName", sparam, DataAccess.Return_Type.DataTable);
                else if (strCustType == "Prospect")
                    dt = (DataTable)daPORT.ExecuteReader_SP("Sp_chk_CustomerName_Prospect", sparam, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                    msg = "exists";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return msg;
        }

        private string Get_CustomerName_List(string strLikeName)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custname", strLikeName);

                DataTable dt = new DataTable();

                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Like_CustList", sp1, DataAccess.Return_Type.DataTable);

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
                {
                    strApp.Append("");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }

        private string GetCustomer_Details(string strCustName)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custname", strCustName);

                DataTable dt = new DataTable();

                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Particular_CustDetails", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        strApp.Append(row["Add1"].ToString());
                        strApp.Append("~" + row["Add2"].ToString());
                        strApp.Append("~" + row["Add3"].ToString());
                        strApp.Append("~" + row["Country"].ToString());
                        strApp.Append("~" + row["City"].ToString());
                        strApp.Append("~" + row["Zipcode"].ToString());
                        strApp.Append("~" + row["Email"].ToString());
                        strApp.Append("~" + row["Custtype"].ToString());
                        strApp.Append("~" + row["Custcode"].ToString());
                        strApp.Append("~" + row["Contactname"].ToString());
                        strApp.Append("~" + row["Phoneno"].ToString());
                        strApp.Append("~" + row["Mobile"].ToString());
                        strApp.Append("~" + row["Channel"].ToString());
                        strApp.Append("~" + row["Pricebasis"].ToString());
                        strApp.Append("~" + row["PriceUnit"].ToString());
                        strApp.Append("~" + row["specialinstruction"].ToString());
                    }
                }
                else
                {
                    strApp.Append("");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

            return strApp.ToString();
        }

        private string GetCompareCustomer_Details(string strCustName, string strCurr, string strLikeCust)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custname", strCustName);
                sp1[1] = new SqlParameter("@PriceUnit", strCurr);
                sp1[2] = new SqlParameter("@likeCust", strLikeCust);

                DataTable dt = new DataTable();

                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Like_CompareCustList", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().ToLower() != strLikeCust.ToLower())
                    {
                        strApp.Append("<ul class='popupUL'>");
                        foreach (DataRow row in dt.Rows)
                        {
                            strApp.Append("<li>" + row["custname"].ToString() + "</li>");
                        }
                        strApp.Append("</ul>");
                    }
                }
                else
                {
                    strApp.Append("");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }
    }
}