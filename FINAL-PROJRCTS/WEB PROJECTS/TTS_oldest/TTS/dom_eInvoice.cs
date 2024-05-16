using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TTS
{
    class dom_eInvoice
    {
        private static DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        public static eInvoiceRoot Dom_eInvoice_Irn(int int_OID, string strAPI)
        {
            eInvoiceRoot eReturnIRN = null;
            try
            {
                SqlParameter[] param = new SqlParameter[] { new SqlParameter("@O_ID", int_OID) };
                DataSet dsInv = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_data_domestic_e_invoice", param, DataAccess.Return_Type.DataSet);
                if (dsInv.Tables[0].Rows.Count > 0 && dsInv.Tables[1].Rows.Count > 0 && dsInv.Tables[2].Rows.Count > 0 && dsInv.Tables[3].Rows.Count > 0 &&
                    dsInv.Tables[4].Rows.Count > 0 && dsInv.Tables[5].Rows.Count > 0)
                {
                    Dictionary<string, object> objMainList = new Dictionary<string, object>();
                    objMainList.Add("Version", "1.1");
                    objMainList.Add("DocDtls", DocDtls(dsInv.Tables[0]));
                    objMainList.Add("ItemList", ItemList(dsInv.Tables[1]));
                    objMainList.Add("ValDtls", ValDtls(dsInv.Tables[2]));

                    string strIgstOnTntra = "N", strSubType = "B2B";
                    if ((dsInv.Tables[3].Rows[0]["Stcd"].ToString() == dsInv.Tables[5].Rows[0]["StateCode"].ToString()) &&
                        Convert.ToDouble(dsInv.Tables[2].Rows[0]["IgstVal"].ToString()) > 0)
                        strIgstOnTntra = "Y";
                    if (dsInv.Tables[4].Rows[0]["shipID"].ToString() == "960" || dsInv.Tables[4].Rows[0]["shipID"].ToString() == "4594" ||
                        dsInv.Tables[4].Rows[0]["shipID"].ToString() == "8016")
                        strSubType = "SEZWOP";

                    objMainList.Add("TranDtls", TranDtls(strIgstOnTntra, strSubType));
                    objMainList.Add("BuyerDtls", BuyerDtls(dsInv));
                    objMainList.Add("SellerDtls", SellerDtls(dsInv.Tables[5]));

                    string JSONresult = JsonConvert.SerializeObject(objMainList);

                    string strResult = CallApiInvoiceGen(JSONresult, strAPI, dsInv.Tables[5].Rows[0]["ZenToken"].ToString());
                    if (strResult != "")
                        eReturnIRN = JsonConvert.DeserializeObject<eInvoiceRoot>(strResult);
                    else
                    {
                        eReturnIRN.status = 0;
                        eReturnIRN.message = "RESPONSE FAILED";
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "dom_eInvoice.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return eReturnIRN;
        }
        private static Dictionary<string, object> DocDtls(DataTable dtDocs)
        {
            Dictionary<string, object> objMainList = new Dictionary<string, object>();
            try
            {
                objMainList.Add("Dt", dtDocs.Rows[0]["invoicedate"].ToString());
                objMainList.Add("No", dtDocs.Rows[0]["invoiceno"].ToString());
                objMainList.Add("Typ", "INV");
            }
            catch (Exception e)
            {
                Utilities.WriteToErrorLog("TTS", "dom_eInvoice.cs", MethodBase.GetCurrentMethod().Name, 1, e.Message);
            }
            return objMainList;
        }
        private static List<object> ItemList(DataTable dtItem)
        {
            List<object> strItems = new List<object>();
            ItemListJson iList = new ItemListJson();
            int i = 1;
            foreach (DataRow iROW in dtItem.Rows)
            {
                iList = new ItemListJson();
                iList.SlNo = i.ToString();
                iList.Qty = Convert.ToDouble(iROW["itemqty"].ToString());
                iList.Unit = "NOS";
                iList.GstRt = Convert.ToDouble(iROW["GstRt"].ToString());
                iList.HsnCd = iROW["HsnCode"].ToString();
                iList.UnitPrice = Convert.ToDouble(iROW["unitprice"].ToString());
                iList.CesAmt = 0.00;
                iList.TotAmt = Convert.ToDouble(iROW["TotAssAmt"].ToString());
                iList.Discount = 0.00;
                iList.AssAmt = Convert.ToDouble(iROW["TotAssAmt"].ToString());
                iList.CgstAmt = Convert.ToDouble(iROW["CgstAmt"].ToString());
                iList.IgstAmt = Convert.ToDouble(iROW["IgstAmt"].ToString());
                iList.SgstAmt = Convert.ToDouble(iROW["SgstAmt"].ToString());
                iList.TotItemVal = Convert.ToDouble(iROW["TotItemVal"].ToString());
                iList.IsServc = "N";
                iList.PrdDesc = "INDUSTRIAL SOLID TYRES";
                strItems.Add(iList);
                i++;
            }
            return strItems;
        }
        private static Dictionary<string, object> ValDtls(DataTable dtInvVal)
        {
            Dictionary<string, object> objMainList = new Dictionary<string, object>();
            objMainList.Add("AssVal", Convert.ToDouble(dtInvVal.Rows[0]["AssVal"].ToString()));
            objMainList.Add("CesVal", 0.00);
            objMainList.Add("StCesVal", 0.00);
            objMainList.Add("Discount", 0.00);
            objMainList.Add("CgstVal", Convert.ToDouble(dtInvVal.Rows[0]["CgstVal"].ToString()));
            objMainList.Add("IgstVal", Convert.ToDouble(dtInvVal.Rows[0]["IgstVal"].ToString()));
            objMainList.Add("SgstVal", Convert.ToDouble(dtInvVal.Rows[0]["SgstVal"].ToString()));
            double totalInvoice = (Convert.ToDouble(dtInvVal.Rows[0]["AssVal"].ToString()) + Convert.ToDouble(dtInvVal.Rows[0]["CgstVal"].ToString()) +
                Convert.ToDouble(dtInvVal.Rows[0]["IgstVal"].ToString()) + Convert.ToDouble(dtInvVal.Rows[0]["SgstVal"].ToString()));
            double tcsCharges = 0.00;
            if (Convert.ToBoolean(dtInvVal.Rows[0]["tcsApplicable"].ToString()))
            {
                //double decTcsPercentage = Convert.ToDouble(Convert.ToBoolean(dtInvVal.Rows[0]["validPan"].ToString()) ? 0.075 : 0.75);
                double decTcsPercentage = Convert.ToDouble(Convert.ToBoolean(dtInvVal.Rows[0]["validPan"].ToString()) ? 0.1 : 0.00);

                tcsCharges = Math.Round((totalInvoice * (decTcsPercentage / 100)), 2);
            }
            objMainList.Add("OthChrg", tcsCharges);
            double totInvVal = (totalInvoice + tcsCharges);

            double decRoundOff = Math.Round((Math.Round(totInvVal, 0) - totInvVal), 2);
            objMainList.Add("RndOffAmt", decRoundOff);
            objMainList.Add("TotInvVal", Math.Round((totInvVal + decRoundOff), 0));
            objMainList.Add("TotItemValSum", Convert.ToDouble(dtInvVal.Rows[0]["AssVal"].ToString()));
            return objMainList;
        }
        private static Dictionary<string, object> TranDtls(string strOnIntra, string strSupType)
        {
            Dictionary<string, object> objMainList = new Dictionary<string, object>();
            objMainList.Add("RegRev", "N");
            objMainList.Add("SupTyp", strSupType);
            objMainList.Add("TaxSch", "GST");
            //objMainList.Add("EcmGstin", "null");
            objMainList.Add("IgstOnIntra", strOnIntra);
            return objMainList;
        }
        private static Dictionary<string, object> BuyerDtls(DataSet dsData)
        {
            Dictionary<string, object> objMainList = new Dictionary<string, object>();
            //objMainList.Add("Em", "null");
            //objMainList.Add("Ph", "null");
            objMainList.Add("Loc", dsData.Tables[3].Rows[0]["city"].ToString());
            objMainList.Add("Pin", Convert.ToInt32(dsData.Tables[3].Rows[0]["zipcode"].ToString()));
            objMainList.Add("Pos", dsData.Tables[3].Rows[0]["Stcd"].ToString());
            objMainList.Add("Stcd", dsData.Tables[3].Rows[0]["Stcd"].ToString());
            string[] split = dsData.Tables[4].Rows[0]["Addr1"].ToString().Split('~');
            objMainList.Add("Addr1", split[0].ToString());
            objMainList.Add("Addr2", split.Length > 1 ? split[1].ToString() : "");
            objMainList.Add("Gstin", dsData.Tables[3].Rows[0]["GST_No"].ToString());
            objMainList.Add("LglNm", dsData.Tables[4].Rows[0]["LglNm"].ToString());
            return objMainList;
        }
        private static Dictionary<string, object> SellerDtls(DataTable dtSeller)
        {
            Dictionary<string, object> objMainList = new Dictionary<string, object>();
            objMainList.Add("Version", "1.1");
            objMainList.Add("Em", "crm@sun-tws.in");
            objMainList.Add("Ph", dtSeller.Rows[0]["Phone"].ToString());
            objMainList.Add("Loc", dtSeller.Rows[0]["Location"].ToString());
            objMainList.Add("Pin", Convert.ToInt32(dtSeller.Rows[0]["PinCode"].ToString()));
            objMainList.Add("Stcd", dtSeller.Rows[0]["StateCode"].ToString());
            objMainList.Add("Addr1", dtSeller.Rows[0]["Addr1"].ToString());
            objMainList.Add("Addr2", dtSeller.Rows[0]["Addr2"].ToString());
            objMainList.Add("Gstin", dtSeller.Rows[0]["GstNo"].ToString());
            //objMainList.Add("LglNm", "T.S.RAJAM TYRES PRIVATE LIMITED");
            objMainList.Add("LglNm", "SUNDARAM INDUSTRIES PRIVATE LIMITED");//*************To Be inserted  On 14-02-2022***********************
            return objMainList;
        }
        private static string CallApiInvoiceGen(string strJson, string strAPIMethod, string strZenToken)
        {
            string postResult = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

                string postUrl = "";
                if (strAPIMethod == "GEN")
                    postUrl = "https://my.gstzen.in/~gstzen/a/post-einvoice-data/einvoice-json/";
                else if (strAPIMethod == "CAN")
                    postUrl = "https://my.gstzen.in/~gstzen/a/post-einvoice-data/einvoice-json/cancel/";

                WebRequest reqPostObject = WebRequest.Create(postUrl);
                reqPostObject.Method = "POST";
                reqPostObject.Headers.Add("Token", strZenToken);
                reqPostObject.ContentType = "application/json";

                using (var streamPostWriter = new StreamWriter(reqPostObject.GetRequestStream()))
                {
                    streamPostWriter.Write(strJson);
                    streamPostWriter.Flush();
                    streamPostWriter.Close();

                    var httpResponse = (HttpWebResponse)reqPostObject.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        return postResult = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("e_INVOICE", "dom_eInvoice.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        #region ItemListJsonsk
        private class ItemListJson
        {
            public double Qty { get; set; }
            public string SlNo { get; set; }
            public string Unit { get; set; }
            public double GstRt { get; set; }
            public string HsnCd { get; set; }
            public double AssAmt { get; set; }
            public double CesAmt { get; set; }
            public double TotAmt { get; set; }
            public double CgstAmt { get; set; }
            public double IgstAmt { get; set; }
            public string IsServc { get; set; }
            public string PrdDesc { get; set; }
            public double SgstAmt { get; set; }
            public double Discount { get; set; }
            public double UnitPrice { get; set; }
            public double TotItemVal { get; set; }
        }
        #endregion
        #region eInvoiceRoot
        public class eInvoiceRoot
        {
            public int status { get; set; }
            public string message { get; set; }
            public string uuid { get; set; }
            public string SignedQrCodeImgUrl { get; set; }
            public string Irn { get; set; }
            public string AckDt { get; set; }
            public long AckNo { get; set; }
            public string Status { get; set; }
            public string SignedQRCode { get; set; }
            public string SignedInvoice { get; set; }
            public string IrnStatus { get; set; }
            public string EwbStatus { get; set; }
            public string EwbNo { get; set; }
            public string EwbDt { get; set; }
            public string EwbValidTill { get; set; }
            public string Remarks { get; set; }
            public string ErrorCode { get; set; }
            public string ErrorDetails { get; set; }
            public string InfoDtls { get; set; }
            public string Irp { get; set; }
        }
        #endregion
    }
}
