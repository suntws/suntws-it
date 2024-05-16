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
using System.Web.Services;
using System.Collections;
using System.Globalization;

namespace TTS
{
    public partial class PreparePriceSheet : System.Web.UI.Page
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

                        fill_PremiumValue();
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

        private void fill_PremiumValue()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Config");
            dt.Columns.Add("PremiumType");
            dt.Columns.Add("PremiumValue");
            dt.Columns.Add("BaseType");
            dt.Columns.Add("BaseRmcb");
            dt.Rows.Add();
            gv_PremiumValue.DataSource = dt;
            gv_PremiumValue.DataBind();
        }

        protected void btnPriceShow_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscriptopen", "showProgress();", true);
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@PriceRefNo", txtPriceRefNo.Text);
                sp1[2] = new SqlParameter("@Category", hdnCategory.Value);
                DataTable dtDate = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_PriceDateDetails_CustWise", sp1, DataAccess.Return_Type.DataTable);

                if (dtDate != null && dtDate.Rows.Count > 0)
                {
                    txtCustPriceAppFrom.Text = dtDate.Rows[0]["StartDate"].ToString();
                    txtCustPriceAppTill.Text = dtDate.Rows[0]["EndDate"].ToString();
                }

                DataTable dtPreVal = new DataTable();
                sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                sp1[1] = new SqlParameter("@SizeCategory", hdnCategory.Value);
                sp1[2] = new SqlParameter("@PRefNo", txtPriceRefNo.Text);

                dtPreVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_PremiumVal_CustWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtPreVal != null && dtPreVal.Rows.Count > 0)
                {
                    gv_PremiumValue.DataSource = dtPreVal;
                    gv_PremiumValue.DataBind();

                    foreach (GridViewRow row in gv_PremiumValue.Rows)
                    {
                        Label lblBase = row.FindControl("lblBase") as Label;
                        Label lblPremium = row.FindControl("lblPremium") as Label;
                        TextBox txtPreValue = row.FindControl("txtPreValue") as TextBox;
                        if (lblBase.Text == lblPremium.Text)
                        {
                            txtPreValue.Text = "0.00";
                            txtPreValue.Enabled = false;
                            break;
                        }
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptForApprovedDate", "getPreparePriceDateDetails();", true);
                }
                else
                {
                    gv_PremiumValue.DataSource = null;
                    gv_PremiumValue.DataBind();
                }

                DataTable dtCustPrice = new DataTable();
                SqlParameter[] sp2 = new SqlParameter[3];
                sp2[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                sp2[1] = new SqlParameter("@SizeCategory", hdnCategory.Value);
                sp2[2] = new SqlParameter("@PRefNo", txtPriceRefNo.Text);

                dtCustPrice = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustomerPriceMaster", sp2, DataAccess.Return_Type.DataTable);
                if (dtCustPrice != null && dtCustPrice.Rows.Count > 0)
                {
                    MappingPriceDetails(dtCustPrice, dtPreVal);
                }
                else
                {
                    gv_PriceGrid.DataSource = null;
                    gv_PriceGrid.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private DataTable Sel_CurValue()
        {
            DataTable dtCurVal = new DataTable();
            SqlParameter[] sp1 = new SqlParameter[2];
            sp1[0] = new SqlParameter("@RatesID", txtRatesID.Text);
            sp1[1] = new SqlParameter("@Cur", txtCurType.Text);

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
            sp4[0] = new SqlParameter("@CustCode", hdnCustCode.Value);

            DataTable dtBeadsVal = new DataTable();
            return dtBeadsVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_BeadQty_CustConfigWise", sp4, DataAccess.Return_Type.DataTable);
        }

        private void MappingPriceDetails(DataTable dtCustPrice, DataTable dtPreVal)
        {
            try
            {
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
                                SqlParameter[] sp6 = new SqlParameter[3];
                                sp6[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                                sp6[1] = new SqlParameter("@SizeCategory", hdnCategory.Value);
                                sp6[2] = new SqlParameter("@PriceSheetRefNo", txtPriceRefNo.Text);

                                DataTable dtCustSizes = new DataTable();
                                dtCustSizes = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_SizeRimTypeBrandSidewall_CustAllConfig", sp6, DataAccess.Return_Type.DataTable);

                                DataTable dtPriceMap = new DataTable("dtPriceMap");
                                dtPriceMap.Columns.Add("Config");
                                dtPriceMap.Columns.Add("Size");
                                dtPriceMap.Columns.Add("Rim");
                                dtPriceMap.Columns.Add("Type");
                                dtPriceMap.Columns.Add("Brand");
                                dtPriceMap.Columns.Add("Sidewall");
                                dtPriceMap.Columns.Add("Qty");
                                dtPriceMap.Columns.Add("Price1");
                                dtPriceMap.Columns.Add("Arv1");
                                dtPriceMap.Columns.Add("Rmc1");
                                dtPriceMap.Columns.Add("Rmcb1");
                                dtPriceMap.Columns.Add("Price2");
                                dtPriceMap.Columns.Add("Arv2");
                                dtPriceMap.Columns.Add("Rmc2");
                                dtPriceMap.Columns.Add("Rmcb2");
                                dtPriceMap.Columns.Add("CalcValue");

                                string strCalcValue = string.Empty;
                                string strConfig = string.Empty;
                                string strSize = string.Empty;
                                string strRim = string.Empty;
                                string strType = string.Empty;
                                string strBrand = string.Empty;
                                string strSidewall = string.Empty;
                                bool boolBeadBand = false;
                                decimal decTypeCost = 0;
                                decimal decSizeCost = 0;
                                decimal decBeadsQty = 0;
                                decimal decFinishedWt = 0;
                                decimal decUnitPrice = 0;
                                decimal decArvVal = 0;
                                decimal decRmcVal = 0;

                                DataView dtView = new DataView(dtPreVal);
                                DataTable distinctVal = dtView.ToTable(true, "Config", "BaseType", "BaseRmcb");

                                foreach (DataRow dtBase in distinctVal.Rows)
                                {
                                    string strBaseType = string.Empty;
                                    decimal decBaseRmcb = 0;
                                    strConfig = dtBase["Config"].ToString();
                                    strBaseType = dtBase["BaseType"].ToString();
                                    decBaseRmcb = Convert.ToDecimal(dtBase["BaseRmcb"].ToString());

                                    try
                                    {
                                        foreach (DataRow custRow in dtCustSizes.Select("TyreType='" + strBaseType + "' and Config='" + strConfig + "'", "Brand ASC,Sidewall ASC,TypePosition ASC,TyreType ASC,position ASC"))
                                        {
                                            strSize = ""; strRim = ""; strType = ""; strBrand = ""; strSidewall = ""; boolBeadBand = false;
                                            decTypeCost = 0; decSizeCost = 0; decBeadsQty = 0; decFinishedWt = 0; decUnitPrice = 0; decArvVal = 0; decRmcVal = 0;

                                            strSize = custRow["TyreSize"].ToString();
                                            strRim = custRow["RimSize"].ToString();
                                            strType = custRow["TyreType"].ToString();
                                            strBrand = custRow["Brand"].ToString();
                                            strSidewall = custRow["Sidewall"].ToString();

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

                                            foreach (DataRow priceRow in dtCustPrice.Select("Config='" + strConfig + "' and TyreSize='" + strSize + "' and RimSize='" + strRim + "' and TyreType='" + strType + "'"))// and Brand='" + strBrand + "' and Sidewall='" + strSidewall + "'
                                            {
                                                decUnitPrice = Convert.ToDecimal(priceRow["UnitPrice"].ToString());
                                            }

                                            if (decUnitPrice == 0)
                                            {
                                                if (decTypeCost > 0 && decSizeCost > 0 && decBeadsQty > 0 && decFinishedWt > 0)
                                                {
                                                    if (boolBeadBand)
                                                    {
                                                        decUnitPrice = ((((decTypeCost + decBaseRmcb + ((decSizeCost * decBeadsQty) / decFinishedWt)) * decFinishedWt) / Convert.ToDecimal(dtCurVal.Rows[0][0].ToString())) * 1);
                                                    }
                                                    else
                                                    {
                                                        decUnitPrice = ((decTypeCost + decBaseRmcb) * decFinishedWt) / Convert.ToDecimal(dtCurVal.Rows[0][0].ToString());
                                                    }
                                                }
                                            }

                                            if (decUnitPrice > 0)
                                            {
                                                if (decUnitPrice > 0 && Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()) > 0 && decFinishedWt > 0)
                                                    decArvVal = Utilities.getArvValue(decUnitPrice, Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()), decFinishedWt);
                                                if (decTypeCost > 0 && decBeadsQty > 0 && decSizeCost > 0 && decFinishedWt > 0)
                                                    decRmcVal = Utilities.getRmcValue(decTypeCost, decBeadsQty, decSizeCost, decFinishedWt, boolBeadBand);

                                                strCalcValue = boolBeadBand + "~" + decTypeCost.ToString("0.00") + "~" + decSizeCost.ToString("0.00") + "~" + decBeadsQty + "~" + decFinishedWt + "~" + Convert.ToDecimal(dtCurVal.Rows[0][0].ToString());

                                                decUnitPrice = txtCurType.Text.ToLower() == "inr" ? Math.Round(Convert.ToDecimal(decUnitPrice), 0) : decUnitPrice;

                                                dtPriceMap.Rows.Add(strConfig, strSize, strRim, strType, strBrand, strSidewall, "0", decUnitPrice.ToString("0.00"), decArvVal.ToString("0.00"), decRmcVal.ToString("0.00"), (decArvVal - decRmcVal).ToString("0.00"), decUnitPrice.ToString("0.00"), decArvVal.ToString("0.00"), decRmcVal.ToString("0.00"), (decArvVal - decRmcVal).ToString("0.00"), strCalcValue);
                                            }
                                            else
                                            {
                                                dtPriceMap.Rows.Add(strConfig, strSize, strRim, strType, strBrand, strSidewall, "0", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "");
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Base records mapping-PricePrepare: " + ex.Message);
                                    }

                                    try
                                    {
                                        foreach (DataRow custRow in dtCustSizes.Select("TyreType<>'" + strBaseType + "' and Config='" + strConfig + "'", "Brand ASC,Sidewall ASC,TypePosition ASC,TyreType ASC,position ASC"))
                                        {
                                            strSize = ""; strRim = ""; strType = ""; strBrand = ""; strSidewall = ""; boolBeadBand = false;
                                            decTypeCost = 0; decSizeCost = 0; decBeadsQty = 0; decFinishedWt = 0; decUnitPrice = 0; decArvVal = 0; decRmcVal = 0;

                                            strSize = custRow["TyreSize"].ToString();
                                            strRim = custRow["RimSize"].ToString();
                                            strType = custRow["TyreType"].ToString();
                                            strBrand = custRow["Brand"].ToString();
                                            strSidewall = custRow["Sidewall"].ToString();

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

                                            foreach (DataRow priceRow in dtCustPrice.Select("Config='" + strConfig + "' and TyreSize='" + strSize + "' and RimSize='" + strRim + "' and TyreType='" + strType + "' and Brand='" + strBrand + "' and Sidewall='" + strSidewall + "'"))
                                            {
                                                decUnitPrice = Convert.ToDecimal(priceRow["UnitPrice"].ToString());
                                            }

                                            if (decUnitPrice == 0)
                                            {
                                                if (decTypeCost > 0 && decSizeCost > 0 && decBeadsQty > 0 && decFinishedWt > 0)
                                                {
                                                    if (boolBeadBand)
                                                    {
                                                        decUnitPrice = ((((decTypeCost + decBaseRmcb + ((decSizeCost * decBeadsQty) / decFinishedWt)) * decFinishedWt) / Convert.ToDecimal(dtCurVal.Rows[0][0].ToString())) * 1);
                                                    }
                                                    else
                                                    {
                                                        decUnitPrice = ((decTypeCost + decBaseRmcb) * decFinishedWt) / Convert.ToDecimal(dtCurVal.Rows[0][0].ToString());
                                                    }
                                                }
                                            }

                                            if (decUnitPrice > 0)
                                            {
                                                if (decUnitPrice > 0 && Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()) > 0 && decFinishedWt > 0)
                                                    decArvVal = Utilities.getArvValue(decUnitPrice, Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()), decFinishedWt);
                                                if (decTypeCost > 0 && decBeadsQty > 0 && decSizeCost > 0 && decFinishedWt > 0)
                                                    decRmcVal = Utilities.getRmcValue(decTypeCost, decBeadsQty, decSizeCost, decFinishedWt, boolBeadBand);

                                                strCalcValue = boolBeadBand + "~" + decTypeCost.ToString("0.00") + "~" + decSizeCost.ToString("0.00") + "~" + decBeadsQty + "~" + decFinishedWt + "~" + Convert.ToDecimal(dtCurVal.Rows[0][0].ToString());

                                                decUnitPrice = txtCurType.Text.ToLower() == "inr" ? Math.Round(Convert.ToDecimal(decUnitPrice), 0) : decUnitPrice;

                                                dtPriceMap.Rows.Add(strConfig, strSize, strRim, strType, strBrand, strSidewall, "0", decUnitPrice.ToString("0.00"), decArvVal.ToString("0.00"), decRmcVal.ToString("0.00"), (decArvVal - decRmcVal).ToString("0.00"), decUnitPrice.ToString("0.00"), decArvVal.ToString("0.00"), decRmcVal.ToString("0.00"), (decArvVal - decRmcVal).ToString("0.00"), strCalcValue);
                                            }
                                            else
                                            {
                                                dtPriceMap.Rows.Add(strConfig, strSize, strRim, strType, strBrand, strSidewall, "0", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "0.00", "");
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Premium records mapping-PricePrepare: " + ex.Message);
                                    }
                                }

                                if (dtPriceMap.Rows.Count > 0)
                                {
                                    ViewState["dtPriceMap"] = dtPriceMap;
                                    gv_PriceGrid.DataSource = dtPriceMap;
                                    gv_PriceGrid.DataBind();
                                    //hidePremiumnValue_ZeroType();
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptColumnHide", "priceGvEditColumnHide();", true);
                                }
                                else
                                {
                                    gv_PriceGrid.DataSource = null;
                                    gv_PriceGrid.DataBind();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void rdbUnitPrice_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptOpen", "showProgress();", true);
                divErrmsg.InnerHtml = "";
                if (txtGenIncr.Text == "" || Convert.ToDecimal(txtGenIncr.Text) == 0)
                {
                    divErrmsg.InnerHtml = "Enter proper increment value";
                }
                else
                {
                    DataTable dtCurVal = new DataTable();
                    dtCurVal = Sel_CurValue();

                    DataTable dtTypeVal = new DataTable();
                    dtTypeVal = Sel_TypeValue();

                    DataTable dtSizeVal = new DataTable();
                    dtSizeVal = Sel_SizeValue();

                    DataTable dtBeadsVal = new DataTable();
                    dtBeadsVal = Sel_BeadsValue();

                    string strConfig = string.Empty;
                    string strSize = string.Empty;
                    string strRim = string.Empty;
                    string strType = string.Empty;
                    string strBrand = string.Empty;
                    string strSidewall = string.Empty;
                    bool boolBeadBand = false;
                    decimal decTypeCost = 0;
                    decimal decSizeCost = 0;
                    decimal decBeadsQty = 0;
                    decimal decFinishedWt = 0;
                    decimal decOldUnitPrice = 0;
                    decimal decNewArvVal = 0;
                    decimal decNewRmcVal = 0;
                    decimal decNewUnitPrice = 0;

                    foreach (GridViewRow row in gv_PriceGrid.Rows)
                    {
                        strConfig = row.Cells[0].Text;
                        strSize = row.Cells[1].Text;
                        strRim = row.Cells[2].Text;
                        strType = row.Cells[3].Text;
                        strBrand = row.Cells[4].Text;
                        strSidewall = row.Cells[5].Text;
                        decOldUnitPrice = Convert.ToDecimal(row.Cells[7].Text);
                        decNewUnitPrice = Convert.ToDecimal(row.Cells[11].Text);
                        decNewArvVal = Convert.ToDecimal(row.Cells[12].Text);
                        decNewRmcVal = Convert.ToDecimal(row.Cells[13].Text);

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

                        if (hdnIncrType.Value == "CHANGE STD PRICE")
                        {
                            if (decOldUnitPrice > 0)
                                decNewUnitPrice = Convert.ToDecimal((decOldUnitPrice + (decOldUnitPrice * Convert.ToDecimal(txtGenIncr.Text)) / 100).ToString("0.00"));
                        }
                        else if (hdnIncrType.Value == "CHANGE NEW PRICE")
                        {
                            if (decNewUnitPrice > 0)
                                decNewUnitPrice = Convert.ToDecimal((decNewUnitPrice + (decNewUnitPrice * Convert.ToDecimal(txtGenIncr.Text)) / 100).ToString("0.00"));
                        }
                        if (decNewUnitPrice > 0)
                        {
                            if (decNewUnitPrice > 0 && Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()) > 0 && decFinishedWt > 0)
                                decNewArvVal = Utilities.getArvValue(decNewUnitPrice, Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()), decFinishedWt);
                            if (decTypeCost > 0 && decBeadsQty > 0 && decSizeCost > 0 && decFinishedWt > 0)
                                decNewRmcVal = Utilities.getRmcValue(decTypeCost, decBeadsQty, decSizeCost, decFinishedWt, boolBeadBand);

                            decNewUnitPrice = txtCurType.Text.ToLower() == "inr" ? Math.Round(Convert.ToDecimal(decNewUnitPrice), 0) : decNewUnitPrice;

                            row.Cells[11].Text = decNewUnitPrice.ToString("0.00");
                            row.Cells[12].Text = decNewArvVal.ToString("0.00");
                            row.Cells[13].Text = decNewRmcVal.ToString("0.00");
                            row.Cells[14].Text = (decNewArvVal - decNewRmcVal).ToString("0.00");
                            row.Cells[15].Text = boolBeadBand + "~" + decTypeCost.ToString("0.00") + "~" + decSizeCost.ToString("0.00") + "~" + decBeadsQty + "~" + decFinishedWt + "~" + Convert.ToDecimal(dtCurVal.Rows[0][0].ToString());
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptColumnHide", "priceGvEditColumnHide();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptOpen", "showProgress();", true);
                //hidePremiumnValue_ZeroType();
                try
                {
                    string _strBaseBrand = gv_PriceGrid.Rows[0].Cells[4].Text;
                    string _strBaseSidewall = gv_PriceGrid.Rows[0].Cells[5].Text;
                    foreach (GridViewRow PriceRowI in gv_PriceGrid.Rows)
                    {
                        foreach (GridViewRow PreRowJ in gv_PremiumValue.Rows)
                        {
                            Label lblPlatform = PreRowJ.FindControl("lblPlatform") as Label;
                            Label lblBase = PreRowJ.FindControl("lblBase") as Label;
                            Label lblPremium = PreRowJ.FindControl("lblPremium") as Label;
                            TextBox txtPreValue = PreRowJ.FindControl("txtPreValue") as TextBox;

                            if (PriceRowI.Cells[0].Text == lblPlatform.Text)
                            {
                                if (PriceRowI.Cells[3].Text != lblBase.Text)
                                {
                                    if (PriceRowI.Cells[3].Text == lblPremium.Text)
                                    {
                                        foreach (GridViewRow gvRowK in gv_PriceGrid.Rows)
                                        {
                                            if (gvRowK.Cells[0].Text == PriceRowI.Cells[0].Text && gvRowK.Cells[1].Text == PriceRowI.Cells[1].Text && gvRowK.Cells[2].Text == PriceRowI.Cells[2].Text && gvRowK.Cells[3].Text == lblBase.Text)
                                            {
                                                PriceRowI.Cells[11].Text = (Convert.ToDecimal(gvRowK.Cells[11].Text) * (1 + Convert.ToDecimal(txtPreValue.Text) / 100)).ToString("0.00");
                                            }
                                        }
                                    }
                                }
                                else if (PriceRowI.Cells[3].Text == lblBase.Text)
                                {
                                    foreach (GridViewRow gvRowK in gv_PriceGrid.Rows)
                                    {
                                        if (gvRowK.Cells[0].Text == PriceRowI.Cells[0].Text && gvRowK.Cells[1].Text == PriceRowI.Cells[1].Text && gvRowK.Cells[2].Text == PriceRowI.Cells[2].Text && gvRowK.Cells[3].Text == lblBase.Text && _strBaseBrand != gvRowK.Cells[4].Text && _strBaseSidewall != gvRowK.Cells[5].Text)
                                        {
                                            gvRowK.Cells[11].Text = (Convert.ToDecimal(PriceRowI.Cells[11].Text)).ToString("0.00");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), "Calc premium value wise unitprice", 1, ex.Message);
                }
                try
                {
                    DataTable dtCurVal = new DataTable();
                    dtCurVal = Sel_CurValue();

                    DataTable dtTypeVal = new DataTable();
                    dtTypeVal = Sel_TypeValue();

                    DataTable dtSizeVal = new DataTable();
                    dtSizeVal = Sel_SizeValue();

                    DataTable dtBeadsVal = new DataTable();
                    dtBeadsVal = Sel_BeadsValue();

                    string strConfig = string.Empty;
                    string strSize = string.Empty;
                    string strRim = string.Empty;
                    string strType = string.Empty;
                    string strBrand = string.Empty;
                    string strSidewall = string.Empty;
                    bool boolBeadBand = false;
                    decimal decTypeCost = 0;
                    decimal decSizeCost = 0;
                    decimal decBeadsQty = 0;
                    decimal decFinishedWt = 0;
                    decimal decNewArvVal = 0;
                    decimal decNewRmcVal = 0;
                    decimal decNewUnitPrice = 0;

                    foreach (GridViewRow PriceRowI in gv_PriceGrid.Rows)
                    {
                        strConfig = PriceRowI.Cells[0].Text;
                        strSize = PriceRowI.Cells[1].Text;
                        strRim = PriceRowI.Cells[2].Text;
                        strType = PriceRowI.Cells[3].Text;
                        strBrand = PriceRowI.Cells[4].Text;
                        strSidewall = PriceRowI.Cells[5].Text;
                        decNewUnitPrice = Convert.ToDecimal(PriceRowI.Cells[11].Text);
                        decNewArvVal = Convert.ToDecimal(PriceRowI.Cells[12].Text);
                        decNewRmcVal = Convert.ToDecimal(PriceRowI.Cells[13].Text);

                        if (decNewUnitPrice > 0)
                        {
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

                            if (decNewUnitPrice > 0)
                            {
                                if (decNewUnitPrice > 0 && Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()) > 0 && decFinishedWt > 0)
                                    decNewArvVal = Utilities.getArvValue(decNewUnitPrice, Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()), decFinishedWt);
                                if (decTypeCost > 0 && decBeadsQty > 0 && decSizeCost > 0 && decFinishedWt > 0)
                                    decNewRmcVal = Utilities.getRmcValue(decTypeCost, decBeadsQty, decSizeCost, decFinishedWt, boolBeadBand);

                                PriceRowI.Cells[12].Text = decNewArvVal.ToString("0.00");
                                PriceRowI.Cells[13].Text = decNewRmcVal.ToString("0.00");
                                PriceRowI.Cells[14].Text = (decNewArvVal - decNewRmcVal).ToString("0.00");
                                PriceRowI.Cells[15].Text = boolBeadBand + "~" + decTypeCost.ToString("0.00") + "~" + decSizeCost.ToString("0.00") + "~" + decBeadsQty + "~" + decFinishedWt + "~" + Convert.ToDecimal(dtCurVal.Rows[0][0].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), "Calc premium value wise rmcb", 1, ex.Message);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptColumnHide", "priceGvEditColumnHide();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnEditCalc_Click(object sender, EventArgs e)
        {
            try
            {
                string strCalcVal = string.Empty;
                bool boolBeadBand = false;
                decimal decTypeCost = 0;
                decimal decSizeCost = 0;
                decimal decBeadsQty = 0;
                decimal decFinishedWt = 0;
                decimal decCurVal = 0;
                decimal decNewArvVal = 0;
                decimal decNewRmcVal = 0;
                decimal decNewUnitPrice = 0;
                decimal decNewRMCB = 0;
                foreach (GridViewRow PriceRow in gv_PriceGrid.Rows)
                {
                    strCalcVal = PriceRow.Cells[15].Text;
                    if (strCalcVal != "" && strCalcVal != "&nbsp;")
                    {
                        string[] strCalcSplit = strCalcVal.Split('~');
                        boolBeadBand = Convert.ToBoolean(strCalcSplit[0].ToString());
                        decTypeCost = Convert.ToDecimal(strCalcSplit[1].ToString());
                        decSizeCost = Convert.ToDecimal(strCalcSplit[2].ToString());
                        decBeadsQty = Convert.ToDecimal(strCalcSplit[3].ToString());
                        decFinishedWt = Convert.ToDecimal(strCalcSplit[4].ToString());
                        decCurVal = Convert.ToDecimal(strCalcSplit[5].ToString());
                        if (hdnPriceEditType.Value == "UNITPRICE" || rdbEditPrice.SelectedItem.Text == "UNITPRICE")
                        {
                            TextBox txtEditUnitPrice = PriceRow.FindControl("txtEditUnitPrice") as TextBox;
                            if (txtEditUnitPrice.Text != "" && txtEditUnitPrice.Text != PriceRow.Cells[11].Text)
                            {
                                if (boolBeadBand)
                                {
                                    decNewRMCB = (Convert.ToDecimal(txtEditUnitPrice.Text) * decCurVal) / decFinishedWt - (decTypeCost + (decBeadsQty * decSizeCost) / decFinishedWt);
                                    PriceRow.Cells[14].Text = decNewRMCB.ToString("0.00");
                                    decNewArvVal = (Convert.ToDecimal(txtEditUnitPrice.Text) * decCurVal) / decFinishedWt;
                                    PriceRow.Cells[12].Text = decNewArvVal.ToString("0.00");
                                    decNewRmcVal = decTypeCost + (decBeadsQty * decSizeCost) / decFinishedWt;
                                    PriceRow.Cells[13].Text = decNewRmcVal.ToString("0.00");
                                }
                                else
                                {
                                    decNewRMCB = (Convert.ToDecimal(txtEditUnitPrice.Text) * decCurVal) / decFinishedWt - (decTypeCost + (0 * decSizeCost) / decFinishedWt);
                                    PriceRow.Cells[14].Text = decNewRMCB.ToString("0.00");
                                    decNewArvVal = (Convert.ToDecimal(txtEditUnitPrice.Text) * decCurVal) / decFinishedWt;
                                    PriceRow.Cells[12].Text = decNewArvVal.ToString("0.00");
                                    decNewRmcVal = decTypeCost;
                                    PriceRow.Cells[13].Text = decNewRmcVal.ToString("0.00");
                                }
                                PriceRow.Cells[11].Text = Convert.ToDecimal(txtEditUnitPrice.Text).ToString("0.00");
                                txtEditUnitPrice.Visible = false;
                            }
                        }
                        else if (hdnPriceEditType.Value == "RMCB" || rdbEditPrice.SelectedItem.Text == "RMCB")
                        {
                            TextBox txtEditRMCB = PriceRow.FindControl("txtEditRMCB") as TextBox;
                            if (txtEditRMCB.Text != "" && txtEditRMCB.Text != PriceRow.Cells[14].Text)
                            {
                                if (boolBeadBand)
                                {
                                    decNewUnitPrice = ((Convert.ToDecimal(txtEditRMCB.Text) + (decTypeCost + (decBeadsQty * decSizeCost) / decFinishedWt)) * decFinishedWt) / decCurVal;
                                    PriceRow.Cells[11].Text = decNewUnitPrice.ToString("0.00");
                                    decNewArvVal = (decNewUnitPrice * decCurVal) / decFinishedWt;
                                    PriceRow.Cells[12].Text = decNewArvVal.ToString("0.00");
                                    decNewRmcVal = decTypeCost + (decBeadsQty * decSizeCost) / decFinishedWt;
                                    PriceRow.Cells[13].Text = decNewRmcVal.ToString("0.00");
                                }
                                else
                                {
                                    decNewUnitPrice = ((Convert.ToDecimal(txtEditRMCB.Text) + (decTypeCost + (0 * decSizeCost) / decFinishedWt)) * decFinishedWt) / decCurVal;
                                    PriceRow.Cells[11].Text = decNewUnitPrice.ToString("0.00");
                                    decNewArvVal = (decNewUnitPrice * decCurVal) / decFinishedWt;
                                    PriceRow.Cells[12].Text = decNewArvVal.ToString("0.00");
                                    decNewRmcVal = decTypeCost;
                                    PriceRow.Cells[13].Text = decNewRmcVal.ToString("0.00");
                                }
                                PriceRow.Cells[14].Text = Convert.ToDecimal(txtEditRMCB.Text).ToString("0.00");
                                txtEditRMCB.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnCalcRmcb_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscriptopen", "showProgress();", true);

                btnEditCalc_Click(sender, e);

                btnCalc_Click(sender, e);

                //DataTable dtCurVal = new DataTable();
                //dtCurVal = Sel_CurValue();

                //DataTable dtTypeVal = new DataTable();
                //dtTypeVal = Sel_TypeValue();

                //DataTable dtSizeVal = new DataTable();
                //dtSizeVal = Sel_SizeValue();

                //DataTable dtBeadsVal = new DataTable();
                //dtBeadsVal = Sel_BeadsValue();

                //string strConfig = string.Empty;
                //string strSize = string.Empty;
                //string strRim = string.Empty;
                //string strType = string.Empty;
                //string strBrand = string.Empty;
                //string strSidewall = string.Empty;
                //bool boolBeadBand = false;
                //decimal decTypeCost = 0;
                //decimal decSizeCost = 0;
                //decimal decBeadsQty = 0;
                //decimal decFinishedWt = 0;
                //decimal decNewArvVal = 0;
                //decimal decNewRmcVal = 0;
                //decimal decNewUnitPrice = 0;
                //decimal dec_ModPrice = 0;

                //string ty_Size = string.Empty;
                //string ty_Rim = string.Empty;
                //foreach (GridViewRow preRowI in gv_PremiumValue.Rows)
                //{
                //    Label lblPlatform = preRowI.FindControl("lblPlatform") as Label;
                //    Label lblBase = preRowI.FindControl("lblBase") as Label;

                //    foreach (GridViewRow priceRow_J in gv_PriceGrid.Rows)
                //    {
                //        ty_Size = priceRow_J.Cells[1].Text;
                //        ty_Rim = priceRow_J.Cells[2].Text;
                //        dec_ModPrice = Convert.ToDecimal(priceRow_J.Cells[11].Text);

                //        if (lblPlatform.Text == priceRow_J.Cells[0].Text && lblBase.Text == priceRow_J.Cells[3].Text)
                //        {
                //            if (priceRow_J.Cells[7].Text == priceRow_J.Cells[11].Text)
                //            {
                //                foreach (GridViewRow priceRow_M in gv_PriceGrid.Rows)
                //                {
                //                    strConfig = priceRow_M.Cells[0].Text;
                //                    strSize = priceRow_M.Cells[1].Text;
                //                    strRim = priceRow_M.Cells[2].Text;
                //                    strType = priceRow_M.Cells[3].Text;
                //                    strBrand = priceRow_M.Cells[4].Text;
                //                    strSidewall = priceRow_M.Cells[5].Text;

                //                    if (lblPlatform.Text == priceRow_M.Cells[0].Text && ty_Size == priceRow_M.Cells[1].Text && ty_Rim == priceRow_M.Cells[2].Text)
                //                    {
                //                        if (priceRow_M.Cells[11].Text != dec_ModPrice.ToString())
                //                        {
                //                            priceRow_M.Cells[11].Text = dec_ModPrice.ToString("0.00");
                //                            decNewUnitPrice = Convert.ToDecimal(priceRow_M.Cells[11].Text);

                //                            if (decNewUnitPrice > 0)
                //                            {
                //                                foreach (DataRow dtTypeRow in dtTypeVal.Select("Type='" + strType + "'"))
                //                                {
                //                                    boolBeadBand = dtTypeRow["beadband"].ToString() == "Yes" ? true : false;
                //                                    decTypeCost = Convert.ToDecimal(dtTypeRow["Typecost"].ToString());
                //                                }
                //                                foreach (DataRow dtSizeRow in dtSizeVal.Select("TyreSize='" + strSize + "'")) { decSizeCost = Convert.ToDecimal(dtSizeRow["SizeVal"].ToString()); }

                //                                foreach (DataRow dtBeadsRow in dtBeadsVal.Select("Config='" + strConfig + "' and TyreSize='" + strSize + "' and RimSize='" + strRim + "'"))
                //                                {
                //                                    decBeadsQty = Convert.ToDecimal(dtBeadsRow["Beads"].ToString());
                //                                    decFinishedWt = Convert.ToDecimal(dtBeadsRow["Finished"].ToString());
                //                                }

                //                                if (decNewUnitPrice > 0)
                //                                {
                //                                    if (decNewUnitPrice > 0 && Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()) > 0 && decFinishedWt > 0)
                //                                        decNewArvVal = Utilities.getArvValue(decNewUnitPrice, Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()), decFinishedWt);
                //                                    if (decTypeCost > 0 && decBeadsQty > 0 && decSizeCost > 0 && decFinishedWt > 0)
                //                                        decNewRmcVal = Utilities.getRmcValue(decTypeCost, decBeadsQty, decSizeCost, decFinishedWt, boolBeadBand);

                //                                    priceRow_M.Cells[12].Text = decNewArvVal.ToString("0.00");
                //                                    priceRow_M.Cells[13].Text = decNewRmcVal.ToString("0.00");
                //                                    priceRow_M.Cells[14].Text = (decNewArvVal - decNewRmcVal).ToString("0.00");
                //                                    priceRow_M.Cells[15].Text = boolBeadBand + "~" + decTypeCost.ToString("0.00") + "~" + decSizeCost.ToString("0.00") + "~" + decBeadsQty + "~" + decFinishedWt + "~" + Convert.ToDecimal(dtCurVal.Rows[0][0].ToString());
                //                                }
                //                            }
                //                        }
                //                    }
                //                } //M
                //            }
                //            else
                //            {
                //                foreach (GridViewRow priceRow_K in gv_PriceGrid.Rows)
                //                {
                //                    strConfig = priceRow_K.Cells[0].Text;
                //                    strSize = priceRow_K.Cells[1].Text;
                //                    strRim = priceRow_K.Cells[2].Text;
                //                    strType = priceRow_K.Cells[3].Text;
                //                    strBrand = priceRow_K.Cells[4].Text;
                //                    strSidewall = priceRow_K.Cells[5].Text;

                //                    if (lblPlatform.Text == priceRow_K.Cells[0].Text && ty_Size == priceRow_K.Cells[1].Text && ty_Rim == priceRow_K.Cells[2].Text)
                //                    {
                //                        priceRow_K.Cells[11].Text = dec_ModPrice.ToString("0.00");
                //                        decNewUnitPrice = Convert.ToDecimal(priceRow_K.Cells[11].Text);

                //                        if (decNewUnitPrice > 0)
                //                        {
                //                            foreach (DataRow dtTypeRow in dtTypeVal.Select("Type='" + strType + "'"))
                //                            {
                //                                boolBeadBand = dtTypeRow["beadband"].ToString() == "Yes" ? true : false;
                //                                decTypeCost = Convert.ToDecimal(dtTypeRow["Typecost"].ToString());
                //                            }
                //                            foreach (DataRow dtSizeRow in dtSizeVal.Select("TyreSize='" + strSize + "'")) { decSizeCost = Convert.ToDecimal(dtSizeRow["SizeVal"].ToString()); }

                //                            foreach (DataRow dtBeadsRow in dtBeadsVal.Select("Config='" + strConfig + "' and TyreSize='" + strSize + "' and RimSize='" + strRim + "'"))
                //                            {
                //                                decBeadsQty = Convert.ToDecimal(dtBeadsRow["Beads"].ToString());
                //                                decFinishedWt = Convert.ToDecimal(dtBeadsRow["Finished"].ToString());
                //                            }

                //                            if (decNewUnitPrice > 0)
                //                            {
                //                                if (decNewUnitPrice > 0 && Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()) > 0 && decFinishedWt > 0)
                //                                    decNewArvVal = Utilities.getArvValue(decNewUnitPrice, Convert.ToDecimal(dtCurVal.Rows[0][0].ToString()), decFinishedWt);
                //                                if (decTypeCost > 0 && decBeadsQty > 0 && decSizeCost > 0 && decFinishedWt > 0)
                //                                    decNewRmcVal = Utilities.getRmcValue(decTypeCost, decBeadsQty, decSizeCost, decFinishedWt, boolBeadBand);

                //                                priceRow_K.Cells[12].Text = decNewArvVal.ToString("0.00");
                //                                priceRow_K.Cells[13].Text = decNewRmcVal.ToString("0.00");
                //                                priceRow_K.Cells[14].Text = (decNewArvVal - decNewRmcVal).ToString("0.00");
                //                                priceRow_K.Cells[15].Text = boolBeadBand + "~" + decTypeCost.ToString("0.00") + "~" + decSizeCost.ToString("0.00") + "~" + decBeadsQty + "~" + decFinishedWt + "~" + Convert.ToDecimal(dtCurVal.Rows[0][0].ToString());
                //                            }
                //                        }
                //                    }
                //                } //K
                //            }
                //        }
                //    } //J
                //}//I
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptColumnHide", "priceGvEditColumnHide();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }

        protected void rdbEditPrice_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscriptopen", "showProgress();", true);
                string strBaseType = string.Empty;
                foreach (GridViewRow preRow in gv_PremiumValue.Rows)
                {
                    Label lblBase = preRow.FindControl("lblBase") as Label;
                    strBaseType = lblBase.Text;
                    break;
                }
                hdnPriceEditType.Value = rdbEditPrice.SelectedItem.Text;
                string _strBrand = gv_PriceGrid.Rows[0].Cells[4].Text;
                string _strSidewall = gv_PriceGrid.Rows[0].Cells[5].Text;
                foreach (GridViewRow priceRow in gv_PriceGrid.Rows)
                {
                    TextBox txtEditUnitPrice = priceRow.FindControl("txtEditUnitPrice") as TextBox;
                    TextBox txtEditRMCB = priceRow.FindControl("txtEditRMCB") as TextBox;
                    txtEditUnitPrice.Visible = false;
                    txtEditRMCB.Visible = false;
                    if (priceRow.Cells[3].Text == strBaseType && priceRow.Cells[4].Text == _strBrand && priceRow.Cells[5].Text == _strSidewall)
                    {
                        if (rdbEditPrice.SelectedItem.Text == "UNITPRICE")
                        {
                            txtEditUnitPrice.Visible = true;
                            txtEditUnitPrice.Text = priceRow.Cells[11].Text;
                        }
                        else if (rdbEditPrice.SelectedItem.Text == "RMCB")
                        {
                            txtEditRMCB.Visible = true;
                            txtEditRMCB.Text = priceRow.Cells[14].Text;
                        }
                    }
                    else
                        break;
                }
                if (rdbEditPrice.SelectedItem.Text == "UNITPRICE")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptColumnShow", "priceGvEditColumnUnitPrice();", true);
                else if (rdbEditPrice.SelectedItem.Text == "RMCB")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptColumnShow", "priceGvEditColumnRmcb();", true);

                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideDivOptEdit();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptColumnHide", "priceGvEditColumnHide();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideDivOptEdit();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnRecordsSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtPreparePriceMap = dtPreparePriceTable_Mapping();
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@Ins_Edit_prepareprice_datatable", dtPreparePriceMap);
                sp1[1] = new SqlParameter("@CustCode", hdnCustCode.Value);
                sp1[2] = new SqlParameter("@PriceSheetRefNo", txtSaveRefNo.Text);
                sp1[3] = new SqlParameter("@Category", hdnCategory.Value);
                sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                if (hdnRecordSaveType.Value == "SAVE")
                {
                    daTTS.ExecuteNonQuery_SP("Sp_Ins_Edit_CustPrice_TableWise", sp1);
                }
                else if (hdnRecordSaveType.Value == "AUTHORIZE")
                {
                    daTTS.ExecuteNonQuery_SP("Sp_Ins_Edit_PriceBuffer_TableWise", sp1);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptColumnHide", "priceGvEditColumnHide();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "hideProgress();", true);

                if (txtGenIncr.Text != "")
                {
                    SqlParameter[] sp2 = new SqlParameter[6];
                    sp2[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    sp2[1] = new SqlParameter("@PriceSheet", txtPriceRefNo.Text);
                    sp2[2] = new SqlParameter("@RatesID", txtRatesID.Text);
                    sp2[3] = new SqlParameter("@Discount", Convert.ToDecimal(txtGenIncr.Text));
                    sp2[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                    sp2[5] = new SqlParameter("@SavePriceSheet", txtSaveRefNo.Text);

                    daTTS.ExecuteNonQuery_SP("sp_ins_PriceGeneralDiscount", sp2);
                }
                Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptColumnHide", "priceGvEditColumnHide();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private DataTable dtPreparePriceTable_Mapping()
        {
            DataTable dtPreparePriceMap = new DataTable();
            try
            {

                string[] strPremiumVal = hdnPremiumValue.Value.Split('~');
                string[] strBaseVal = hdnBaseValue.Value.Split(',');
                Hashtable htPremium = new Hashtable();

                for (int j = 0; j < strPremiumVal.Length; j++)
                {
                    string[] strSplit = strPremiumVal[j].Split(',');
                    htPremium.Add(strSplit[0].ToString(), strSplit[1].ToString());
                }

                //dtPreparePriceMap = ViewState["dtPriceMap"] as DataTable;

                //dtPreparePriceMap.Columns.Remove("Qty");
                //dtPreparePriceMap.Columns.Remove("Price1");
                //dtPreparePriceMap.Columns.Remove("Arv1");
                //dtPreparePriceMap.Columns.Remove("Rmc1");
                //dtPreparePriceMap.Columns.Remove("Rmcb1");
                //dtPreparePriceMap.Columns.Remove("Rmcb2");
                //dtPreparePriceMap.Columns.Remove("CalcValue");
                DataColumn dCol = new DataColumn("Config", typeof(System.String));
                dtPreparePriceMap.Columns.Add(dCol);
                dCol = new DataColumn("Size", typeof(System.String));
                dtPreparePriceMap.Columns.Add(dCol);
                dCol = new DataColumn("Rim", typeof(System.String));
                dtPreparePriceMap.Columns.Add(dCol);
                dCol = new DataColumn("Type", typeof(System.String));
                dtPreparePriceMap.Columns.Add(dCol);
                dCol = new DataColumn("Brand", typeof(System.String));
                dtPreparePriceMap.Columns.Add(dCol);
                dCol = new DataColumn("Sidewall", typeof(System.String));
                dtPreparePriceMap.Columns.Add(dCol);
                dCol = new DataColumn("Price2", typeof(System.Decimal));
                dtPreparePriceMap.Columns.Add(dCol);
                dCol = new DataColumn("Arv2", typeof(System.Decimal));
                dtPreparePriceMap.Columns.Add(dCol);
                dCol = new DataColumn("Rmc2", typeof(System.Decimal));
                dtPreparePriceMap.Columns.Add(dCol);

                foreach (GridViewRow gvRow in gv_PriceGrid.Rows)
                {
                    dtPreparePriceMap.Rows.Add(gvRow.Cells[0].Text, gvRow.Cells[1].Text, gvRow.Cells[2].Text, gvRow.Cells[3].Text, gvRow.Cells[4].Text, gvRow.Cells[5].Text, gvRow.Cells[11].Text, gvRow.Cells[12].Text, gvRow.Cells[13].Text);
                }

                DataColumn dColumn = new DataColumn("StartDate", typeof(System.DateTime));
                try
                {
                    dColumn.DefaultValue = DateTime.Parse(txtCustPriceAppFrom.Text);
                }
                catch (Exception)
                {
                    dColumn.DefaultValue = DateTime.ParseExact(txtCustPriceAppFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                dtPreparePriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("EndDate", typeof(System.DateTime));
                try
                {
                    dColumn.DefaultValue = DateTime.Parse(txtCustPriceAppTill.Text);
                }
                catch (Exception)
                {
                    dColumn.DefaultValue = DateTime.ParseExact(txtCustPriceAppTill.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                dtPreparePriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("RatesID", typeof(System.String));
                dColumn.DefaultValue = txtRatesID.Text;
                dtPreparePriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("BaseType", typeof(System.String));
                dColumn.DefaultValue = strBaseVal[0].ToString();
                dtPreparePriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("BaseRmcb", typeof(System.Decimal));
                dColumn.DefaultValue = Convert.ToDecimal(strBaseVal[1].ToString()).ToString("0.00");
                dtPreparePriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("PremiumType", typeof(System.String));
                dtPreparePriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("PremiumValue", typeof(System.Decimal));
                dtPreparePriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("ProcessID", typeof(System.String));
                dtPreparePriceMap.Columns.Add(dColumn);

                DataTable dtProcessID = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@Config", dtPreparePriceMap.Rows[0]["Config"].ToString());
                dtProcessID = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_ProcessIDList", sp1, DataAccess.Return_Type.DataTable);

                foreach (DataRow row in dtPreparePriceMap.Rows)
                {
                    row["PremiumType"] = row["Type"].ToString();
                    if (row["Type"].ToString() == strBaseVal[0].ToString())
                        row["PremiumValue"] = "0.00";
                    else
                        row["PremiumValue"] = Convert.ToDecimal(htPremium[row["PremiumType"].ToString()].ToString()).ToString("0.00");

                    foreach (DataRow processidRow in dtProcessID.Select("Config='" + dtPreparePriceMap.Rows[0]["Config"].ToString() + "' AND TyreSize='" + row["Size"].ToString() + "' AND TyreRim='" + row["Rim"].ToString() + "' AND TyreType='" + row["Type"].ToString() + "' AND Brand='" + row["Brand"].ToString() + "' AND Sidewall='" + row["Sidewall"].ToString() + "'"))
                    {
                        row["ProcessID"] = processidRow["ProcessID"].ToString() != "" ? processidRow["ProcessID"].ToString() : null;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dtPreparePriceMap;
        }

        private void hidePremiumnValue_ZeroType()
        {
            try
            {
                Hashtable htPremium = new Hashtable();
                string strBaseType = string.Empty;

                foreach (GridViewRow PreRow in gv_PremiumValue.Rows)
                {
                    Label lblBase = PreRow.FindControl("lblBase") as Label;
                    Label lblPremium = PreRow.FindControl("lblPremium") as Label;
                    TextBox txtPreValue = PreRow.FindControl("txtPreValue") as TextBox;
                    if (lblBase.Text != lblPremium.Text)
                    {
                        if (Convert.ToDecimal(txtPreValue.Text) > 0)
                            htPremium.Add(lblPremium.Text, "true");
                        else
                            htPremium.Add(lblPremium.Text, "false");
                    }
                    strBaseType = lblBase.Text;
                }

                foreach (GridViewRow gvRow in gv_PriceGrid.Rows)
                {
                    if (gvRow.Cells[3].Text != strBaseType)
                        gvRow.Visible = Convert.ToBoolean(htPremium[gvRow.Cells[3].Text].ToString());
                }
            }
            catch (Exception ex)
            {
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

        #region[Get Price Details WebMethod]
        [WebMethod]
        public static string get_PriceDateDetails_WebMethod(string strCustCode, string strPriceNo, string strCategory)
        {
            return GetPriceDate_Details(strCustCode.ToString(), strPriceNo.ToString(), strCategory.ToString()).GetXml();
        }

        private static DataSet GetPriceDate_Details(string strCustCode, string strPriceNo, string strCategory)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", strCustCode);
                sp1[1] = new SqlParameter("@PriceRefNo", strPriceNo);
                sp1[2] = new SqlParameter("@Category", strCategory);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Get_PreparePriceDetails_CustWise", sp1, DataAccess.Return_Type.DataSet);
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