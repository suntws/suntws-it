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
using System.Text;
using System.IO;

namespace COTS
{
    public partial class bindvalues : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["orderjathagam"] != null && Request["orderjathagam"].ToString() != "")
                    Response.Write(insert_temp_order_list(Request["orderjathagam"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "ChkOrderNo")
                    Response.Write(chk_orderrefno(Request["chkrefno"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "getorderrefno_custwise")
                    Response.Write(Get_CurrentYear_OrderRefNo(Request["orderkno_key"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "getLikeAddress")
                    Response.Write(Get_Like_Order_Address(Request["likeCity"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "getFullAddress")
                    Response.Write(Get_All_Order_Address());
                else if (Request["type"] != null && Request["type"].ToString() == "getAddress")
                    Response.Write(Get_Address_CustChoice(Request["addid"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "getClaimSize")
                    Response.Write(Get_ClaimCust_TyreSize(Request["Brand"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "getClaimType")
                    Response.Write(Get_ClaimCust_TyreType(Request["Brand"].ToString(), Request["Size"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "delClaimImg")
                    Response.Write(Del_ClaimImages(Request["claimimgname"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "chkClaimstencil")
                    Response.Write(Chk_ClaimStencilno(Request["claimstencilno"].ToString()));
                else if (Request["type"] != null && Request["type"].ToString() == "delstencilFailure")
                    Response.Write(RemoveStencilFailureImages());
            }
        }

        private string Get_Like_Order_Address(string strLikeCity)
        {
            string returnValue = string.Empty;
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@city", strLikeCity);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_Like_ShipBillAddress", sp1, DataAccess.Return_Type.DataTable);

                StringBuilder strApp = new StringBuilder();
                if (dt.Rows.Count > 0 && dt.Rows[0]["city"].ToString() != "")
                {
                    strApp.Append("<ul class='popupUL'>");
                    foreach (DataRow row in dt.Rows)
                    {
                        strApp.Append("<li style='word-break: break-word;width: 273px;float: left;'>" + row["city"].ToString() + " - " + row["statename"].ToString() + row["contact_name"].ToString() + row["shipaddress"].ToString().Replace("~", "") + row["zipcode"].ToString() + "</li>");
                    }
                    strApp.Append("</ul>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnValue;
        }

        private string Get_All_Order_Address()
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_Cust_ShipBillAddress", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0 && dt.Rows[0]["city"].ToString() != "")
                {
                    strApp.Append("<ul class='popupUL'>");
                    foreach (DataRow row in dt.Rows)
                    {
                        strApp.Append("<li style='word-break: break-word;width: 273px;float: left;'>" + row["city"].ToString() + " - " + row["statename"].ToString() + row["contact_name"].ToString() + row["shipaddress"].ToString().Replace("~", "") + row["zipcode"].ToString() + "</li>");
                    }
                    strApp.Append("</ul>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }

        private string chk_orderrefno(string refno)
        {
            string returnValue = "";
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@orderrefno", refno);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_orderrefno", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0 && dt.Rows[0]["OrderRefNo"].ToString() != "")
                    returnValue = "ORDER NO ALREADY EXISTING";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return returnValue.ToString();
        }

        private string insert_temp_order_list(string _orderjathagam)
        {
            string strMsg = "fail";
            try
            {
                string[] strItemList = _orderjathagam.Split('_');
                SqlParameter[] sp1 = new SqlParameter[11];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@category", strItemList[7].ToString());
                sp1[2] = new SqlParameter("@config", strItemList[0].ToString().Replace("~", " "));
                sp1[3] = new SqlParameter("@tyresize", strItemList[4].ToString().Replace("~", " "));
                sp1[4] = new SqlParameter("@rimsize", strItemList[5].ToString().Replace("~", " "));
                sp1[5] = new SqlParameter("@tyretype", strItemList[3].ToString().Replace("~", " "));
                sp1[6] = new SqlParameter("@brand", strItemList[1].ToString().Replace("~", " "));
                sp1[7] = new SqlParameter("@sidewall", strItemList[2].ToString().Replace("~", " "));
                sp1[8] = new SqlParameter("@itemqty", Convert.ToInt32(strItemList[6].ToString()));
                sp1[9] = new SqlParameter("@stdCustCode", Session["cotsstdcode"].ToString());
                sp1[10] = new SqlParameter("@OrderRefNo", strItemList[8].ToString());

                int resp = 0;
                if (Session["cotsstdcode"].ToString() == "DE0048")
                    resp = daCOTS.ExecuteNonQuery_SP("sp_ins_OrderItemList", sp1);
                else
                    resp = daCOTS.ExecuteNonQuery_SP("sp_ins_OrderItemList_Export", sp1);
                if (resp == 1)
                    strMsg = "success";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strMsg.ToString();
        }

        private string Get_CurrentYear_OrderRefNo(string strorderno)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@orderno", strorderno);

                DataTable dtOrderno = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_like_orderrefno", sp1, DataAccess.Return_Type.DataTable);
                if (dtOrderno.Rows.Count > 0)
                {
                    if (dtOrderno.Rows[0][0].ToString().ToLower() != strorderno.ToLower())
                    {
                        strApp.Append("<ul class='popupUL'>");
                        foreach (DataRow row in dtOrderno.Rows)
                        {
                            strApp.Append("<li>" + row["OrderRefNo"].ToString() + "</li>");
                        }
                        strApp.Append("</ul>");
                    }
                }
                else
                    strApp.Append("");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }

        private string Get_Address_CustChoice(string strID)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@shipid", Convert.ToInt32(strID));
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_address_IDwise", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    strApp.Append("<table style='border-collapse: collapse; border-color: #000; width: 502px; line-height: 16px;'>");
                    DataRow row = dt.Rows[0];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string tr = "<tr style='vertical-align: top;'><th style='width:85px; background-color: #861616; text-align: left; color: #fff;'>" +
                            dt.Columns[i].ToString().ToUpper() + "</th><td style='font-weight:bold;'>:</td><td style='font-weight:bold;'>" +
                            row[i].ToString().Replace("~", "<br/>") + "</td></tr>";
                        strApp.Append(tr);
                    }
                    strApp.Append("</table>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
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

        private string Del_ClaimImages(string strImg)
        {
            string strApp = string.Empty;
            try
            {
                string serverURL = Server.MapPath("~/").Replace("SCOTS", "TTS").Replace("COTS", "TTS");
                string path = serverURL + "/claimimages/" + Session["cotscode"].ToString() + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/dummy/" + strImg;
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
                Utilities.WriteToErrorLog("SCOTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }

        private string RemoveStencilFailureImages()
        {
            try
            {
                string fileFolder = Request.QueryString["path"].ToString();
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("SCOTS", "TTS").Replace("COTS", "TTS");
                string path = serverURL + fileFolder;
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return "";
        }
    }
}