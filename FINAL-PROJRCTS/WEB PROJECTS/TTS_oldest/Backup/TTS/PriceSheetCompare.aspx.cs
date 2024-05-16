using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Reflection;

namespace TTS
{
    public partial class PriceSheetCompare : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
            {
                if (!IsPostBack)
                {
                    DataTable dtUser = Session["dtuserlevel"] as DataTable;
                    if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["pricesheet_build"].ToString() == "True")
                    {
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

        private DataTable Sel_CurValue()
        {
            DataTable dtCurVal = new DataTable();
            SqlParameter[] sp1 = new SqlParameter[2];
            sp1[0] = new SqlParameter("@RatesID", txtRatesID.Text);
            sp1[1] = new SqlParameter("@Cur", txtCurrency.Text);

            return dtCurVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CurVal_RatesIDWise", sp1, DataAccess.Return_Type.DataTable);
        }

        private DataTable Sel_TypeValue()
        {
            SqlParameter[] sp2 = new SqlParameter[1];
            sp2[0] = new SqlParameter("@RatesID", txtRatesID.Text);
            DataTable dtTypeVal = new DataTable();

            return dtTypeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_TypeDetails_All", sp2, DataAccess.Return_Type.DataTable);
        }

        private DataTable Sel_SizeValue()
        {
            SqlParameter[] sp3 = new SqlParameter[1];
            sp3[0] = new SqlParameter("@RatesID", txtRatesID.Text);

            DataTable dtSizeVal = new DataTable();
            return dtSizeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_SizeVal_RatesIDWise", sp3, DataAccess.Return_Type.DataTable);
        }

        private DataTable Sel_BeadsValue()
        {
            SqlParameter[] sp4 = new SqlParameter[1];
            sp4[0] = new SqlParameter("@CustCode", hdnCustCode1.Value);

            DataTable dtBeadsVal = new DataTable();
            return dtBeadsVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_BeadQty_CustConfigWise", sp4, DataAccess.Return_Type.DataTable);
        }

        private DataTable Sel_FinishedWt()
        {
            SqlParameter[] sp5 = new SqlParameter[2];
            sp5[0] = new SqlParameter("@CustCode", hdnCustCode1.Value);
            sp5[1] = new SqlParameter("@SizeCategory", hdnCategory.Value);

            DataTable dtFinishedVal = new DataTable();
            return dtFinishedVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_FinishedWt_CustAllConfig", sp5, DataAccess.Return_Type.DataTable);
        }

        private DataTable Sel_CustUnitPrice(string strCustCode, string strPRefNo)
        {
            DataTable dtCustPrice = new DataTable();
            SqlParameter[] sp2 = new SqlParameter[3];
            sp2[0] = new SqlParameter("@CustCode", strCustCode);
            sp2[1] = new SqlParameter("@SizeCategory", hdnCategory.Value);
            sp2[2] = new SqlParameter("@PRefNo", strPRefNo);

            return dtCustPrice = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustomerPriceMaster", sp2, DataAccess.Return_Type.DataTable);
        }

        protected void btnSheetCompare_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscriptopen", "showProgress();", true);

                DataTable dtCurVal = new DataTable();
                dtCurVal = Sel_CurValue();
                if (dtCurVal.Rows.Count == 0)
                    divErrmsg.InnerHtml = "Currency Value is empty - Please check rates master <br />";
                else
                {
                    DataTable dtTypeVal = new DataTable();
                    dtTypeVal = Sel_TypeValue();
                    if (dtTypeVal.Rows.Count == 0)
                        divErrmsg.InnerHtml += "Type cost is empty -Please check rates master <br />";
                    else
                    {
                        DataTable dtSizeVal = new DataTable();
                        dtSizeVal = Sel_SizeValue();
                        if (dtSizeVal.Rows.Count == 0)
                            divErrmsg.InnerHtml += "Size value is empty - Please check rates master <br />";
                        else
                        {
                            DataTable dtBeadsVal = new DataTable();
                            dtBeadsVal = Sel_BeadsValue();
                            if (dtBeadsVal.Rows.Count == 0)
                                divErrmsg.InnerHtml += "Beadsqty value is empty - Please check beadband master <br />";
                            else
                            {
                                //DataTable dtFinishedVal = new DataTable();
                                //dtFinishedVal = Sel_FinishedWt();

                                SqlParameter[] sp6 = new SqlParameter[2];
                                sp6[0] = new SqlParameter("@CustCode", hdnCustCode1.Value);
                                sp6[1] = new SqlParameter("@SizeCategory", hdnCategory.Value);

                                DataTable dtCustSizes = new DataTable();
                                dtCustSizes = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_SizeRimType_Compare_Cust", sp6, DataAccess.Return_Type.DataTable);

                                DataTable dtCustPrice1 = new DataTable();
                                dtCustPrice1 = Sel_CustUnitPrice(hdnCustCode1.Value, txtPriceRefNo1.Text);

                                DataTable dtCustPrice2 = new DataTable();
                                dtCustPrice2 = Sel_CustUnitPrice(hdnCustCode2.Value, txtPriceRefNo2.Text);

                                DataTable dtPriceMap = new DataTable("dtPriceMap");
                                dtPriceMap.Columns.Add("Config");
                                dtPriceMap.Columns.Add("Size");
                                dtPriceMap.Columns.Add("Rim");
                                dtPriceMap.Columns.Add("Type");
                                dtPriceMap.Columns.Add("Price1");
                                dtPriceMap.Columns.Add("Price2");
                                dtPriceMap.Columns.Add("Diff1");
                                dtPriceMap.Columns.Add("RMCB1");
                                dtPriceMap.Columns.Add("RMCB2");
                                dtPriceMap.Columns.Add("Diff2");

                                string strConfig = string.Empty;
                                string strSize = string.Empty;
                                string strRim = string.Empty;
                                string strType = string.Empty;
                                //string strBrand = string.Empty;
                                //string strSidewall = string.Empty;
                                bool boolBeadBand = false;
                                decimal decTypeCost = 0;
                                decimal decSizeCost = 0;
                                decimal decBeadsQty = 0;
                                decimal decFinishedWt = 0;
                                decimal decUnitPrice1 = 0;
                                decimal decUnitPrice2 = 0;
                                decimal decRmcb1 = 0;
                                decimal decRmcb2 = 0;

                                decimal decArvVal = 0;
                                decimal decRmcVal = 0;

                                try
                                {
                                    DataView dtView = new DataView(dtCustSizes);
                                    DataTable distinctVal = dtView.ToTable(true, "Config", "TyreType");

                                    foreach (DataRow ROW in distinctVal.Rows)
                                    {
                                        strConfig = ROW["Config"].ToString();
                                        strType = ROW["TyreType"].ToString();
                                        foreach (DataRow custRow in dtCustSizes.Select("TyreType='" + strType + "' and Config='" + strConfig + "'", "TypePosition ASC,TyreType ASC,position ASC"))
                                        {
                                            strSize = ""; strRim = ""; boolBeadBand = false;//strBrand = ""; strSidewall = "";
                                            decTypeCost = 0; decSizeCost = 0; decBeadsQty = 0; decFinishedWt = 0; decUnitPrice1 = 0; decRmcb1 = 0; decRmcb2 = 0;

                                            strSize = custRow["TyreSize"].ToString();
                                            strRim = custRow["RimSize"].ToString();
                                            strType = custRow["TyreType"].ToString();
                                            //strBrand = custRow["Brand"].ToString();
                                            //strSidewall = custRow["Sidewall"].ToString();

                                            foreach (DataRow dtTypeRow in dtTypeVal.Select("Type='" + strType + "'"))
                                            {
                                                boolBeadBand = dtTypeRow["beadband"].ToString() == "Yes" ? true : false;
                                                decTypeCost = Convert.ToDecimal(dtTypeRow["Typecost"].ToString());
                                            }
                                            foreach (DataRow dtSizeRow in dtSizeVal.Select("TyreSize='" + strSize + "'")) { decSizeCost = Convert.ToDecimal(dtSizeRow["SizeVal"].ToString()); }

                                            foreach (DataRow dtBeadsRow in dtBeadsVal.Select("Config='" + strConfig + "' and TyreSize='" + strSize + "' and RimSize='" + strRim + "'"))
                                            {
                                                decBeadsQty = Convert.ToDecimal(dtBeadsRow["Beads"].ToString());
                                                decFinishedWt = Convert.ToDecimal(dtBeadsRow["Finished"].ToString());
                                            }

                                            //foreach (DataRow dtFinishRow in dtFinishedVal.Select("Config='" + strConfig + "' and TyreSize='" + strSize + "' and TyreRim='" + strRim + "' and TyreType='" + strType + "'"))
                                            //{// and Brand='" + strBrand + "' and Sidewall='" + strSidewall + "'
                                            //    decFinishedWt = Convert.ToDecimal(dtFinishRow["FinishedWt"].ToString());
                                            //}

                                            //customer 1
                                            foreach (DataRow priceRow in dtCustPrice1.Select("Config='" + strConfig + "' and TyreSize='" + strSize + "' and RimSize='" + strRim + "' and TyreType='" + strType + "'"))
                                            {
                                                decUnitPrice1 = Convert.ToDecimal(priceRow["UnitPrice"].ToString());
                                            }

                                            if (decUnitPrice1 > 0)
                                            {
                                                if (decUnitPrice1 > 0 && Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()) > 0 && decFinishedWt > 0)
                                                    decArvVal = Utilities.getArvValue(decUnitPrice1, Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()), decFinishedWt);
                                                if (decTypeCost > 0 && decBeadsQty > 0 && decSizeCost > 0 && decFinishedWt > 0)
                                                    decRmcVal = Utilities.getRmcValue(decTypeCost, decBeadsQty, decSizeCost, decFinishedWt, boolBeadBand);

                                                decRmcb1 = (decArvVal - decRmcVal);
                                            }

                                            //customer 2
                                            foreach (DataRow priceRow in dtCustPrice2.Select("Config='" + strConfig + "' and TyreSize='" + strSize + "' and RimSize='" + strRim + "' and TyreType='" + strType + "'"))
                                            {
                                                decUnitPrice2 = Convert.ToDecimal(priceRow["UnitPrice"].ToString());
                                            }

                                            if (decUnitPrice2 > 0)
                                            {
                                                if (decUnitPrice2 > 0 && Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()) > 0 && decFinishedWt > 0)
                                                    decArvVal = Utilities.getArvValue(decUnitPrice2, Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()), decFinishedWt);
                                                if (decTypeCost > 0 && decBeadsQty > 0 && decSizeCost > 0 && decFinishedWt > 0)
                                                    decRmcVal = Utilities.getRmcValue(decTypeCost, decBeadsQty, decSizeCost, decFinishedWt, boolBeadBand);

                                                decRmcb2 = (decArvVal - decRmcVal);
                                            }
                                            decUnitPrice1 = txtCurrency.Text.ToLower() == "inr" ? Math.Round(decUnitPrice1, 0) : decUnitPrice1;
                                            decUnitPrice2 = txtCurrency.Text.ToLower() == "inr" ? Math.Round(decUnitPrice2, 0) : decUnitPrice2;
                                            dtPriceMap.Rows.Add(strConfig, strSize, strRim, strType, decUnitPrice1.ToString("0.00"), decUnitPrice2.ToString("0.00"), (decUnitPrice1 - decUnitPrice2).ToString("0.00"), decRmcb1.ToString("0.00"), decRmcb2.ToString("0.00"), (decRmcb1 - decRmcb2).ToString("0.00"));
                                        }
                                    }

                                    if (dtPriceMap.Rows.Count > 0)
                                    {
                                        gv_PriceCompareGrid.DataSource = dtPriceMap;
                                        gv_PriceCompareGrid.DataBind();
                                    }
                                    else
                                    {
                                        gv_PriceCompareGrid.DataSource = null;
                                        gv_PriceCompareGrid.DataBind();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Compare records mapping: " + ex.Message);
                                }

                            }
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        #region[Get Cust Details WebMethod]
        [WebMethod]
        public static string get_CustDetails_WebMethod(string strCustName)
        {
            return GetCustomer_Details(strCustName.ToString()).GetXml();
        }

        private static DataSet GetCustomer_Details(string strCustName)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@CustName", strCustName);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Get_Details_CustWise", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PreparePriceSheet.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion
    }
}