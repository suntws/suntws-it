using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections;
using System.Globalization;

namespace TTS
{
    public partial class CustPriceMaster : System.Web.UI.Page
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
                        fill_TypeGrid();
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

        private void fill_TypeGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TyreType");
            dt.Columns.Add("PREMIUM");
            dt.Rows.Add();
            gv_CustType.DataSource = dt;
            gv_CustType.DataBind();
        }

        protected void btnCustPriceCal_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscriptopen", "showProgress();", true);
                errMsg.InnerHtml = "";
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@RatesID", txtRatesID.Text);
                sp1[1] = new SqlParameter("@Cur", txtCurType.Text);

                DataTable dtCurVal = new DataTable();
                dtCurVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CurVal_RatesIDWise", sp1, DataAccess.Return_Type.DataTable);

                if (dtCurVal.Rows.Count == 0)
                    errMsg.InnerHtml = "Currency Value is empty - Please check rates master <br />";
                else
                {
                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    sp2[1] = new SqlParameter("@Config", hdnPlatform.Value);
                    sp2[2] = new SqlParameter("@RatesID", txtRatesID.Text);

                    DataTable dtTypeVal = new DataTable();
                    dtTypeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_TypeDetails_CustTypeWise", sp2, DataAccess.Return_Type.DataTable);

                    if (dtTypeVal.Rows.Count == 0)
                        errMsg.InnerHtml += "Type cost is empty -Please check rates master <br />";
                    else
                    {
                        SqlParameter[] sp3 = new SqlParameter[1];
                        sp3[0] = new SqlParameter("@RatesID", txtRatesID.Text);

                        DataTable dtSizeVal = new DataTable();
                        dtSizeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_SizeVal_RatesIDWise", sp3, DataAccess.Return_Type.DataTable);

                        if (dtSizeVal.Rows.Count == 0)
                            errMsg.InnerHtml += "Size value is empty - Please check rates master <br />";
                        else
                        {
                            SqlParameter[] sp4 = new SqlParameter[1];
                            sp4[0] = new SqlParameter("@Config", hdnPlatform.Value);

                            DataTable dtBeadsVal = new DataTable();
                            dtBeadsVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_BeadQty_ConfigWise", sp4, DataAccess.Return_Type.DataTable);

                            if (dtBeadsVal.Rows.Count == 0)
                                errMsg.InnerHtml += "Beadsqty value is empty - Please check beadband master <br />";
                            else
                            {
                                SqlParameter[] sp6 = new SqlParameter[3];
                                sp6[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                                sp6[1] = new SqlParameter("@Config", hdnPlatform.Value);
                                sp6[2] = new SqlParameter("@SizeCategory", hdnCategory.Value);

                                DataTable dtCustSizes = new DataTable();
                                dtCustSizes = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_SizeRimTypeBrandSidewall_CustWise", sp6, DataAccess.Return_Type.DataTable);

                                DataTable dtMap = new DataTable("dtMap");
                                dtMap.Columns.Add("Size");
                                dtMap.Columns.Add("Rim");
                                dtMap.Columns.Add("Type");
                                dtMap.Columns.Add("Brand");
                                dtMap.Columns.Add("Sidewall");
                                dtMap.Columns.Add("UnitPrice");

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
                                try
                                {
                                    foreach (DataRow dtcustRow in dtCustSizes.Select("TyreType='" + hdnType.Value + "'", "Brand ASC, Sidewall ASC,TypePosition ASC, TyreType ASC, position ASC"))
                                    {
                                        strSize = ""; strRim = ""; strType = ""; strBrand = ""; strSidewall = ""; boolBeadBand = false;
                                        decTypeCost = 0; decSizeCost = 0; decBeadsQty = 0; decFinishedWt = 0; decUnitPrice = 0;
                                        strSize = dtcustRow["TyreSize"].ToString();
                                        strRim = dtcustRow["RimSize"].ToString();
                                        strType = dtcustRow["TyreType"].ToString();
                                        strBrand = dtcustRow["Brand"].ToString();
                                        strSidewall = dtcustRow["Sidewall"].ToString();

                                        foreach (DataRow dtTypeRow in dtTypeVal.Select("Type='" + strType + "'"))
                                        {
                                            boolBeadBand = dtTypeRow["beadband"].ToString() == "Yes" ? true : false;
                                            decTypeCost = Convert.ToDecimal(dtTypeRow["Typecost"].ToString());
                                        }

                                        foreach (DataRow dtSizeRow in dtSizeVal.Select("TyreSize='" + strSize + "'")) { decSizeCost = Convert.ToDecimal(dtSizeRow["SizeVal"].ToString()); }

                                        foreach (DataRow dtBeadsRow in dtBeadsVal.Select("TyreSize='" + strSize + "' and RimSize='" + strRim + "'"))
                                        {
                                            decBeadsQty = Convert.ToDecimal(dtBeadsRow["Beads"].ToString());
                                            decFinishedWt = Convert.ToDecimal(dtBeadsRow["Finished"].ToString());
                                        }

                                        if (decTypeCost > 0 && decSizeCost > 0 && decBeadsQty > 0 && decFinishedWt > 0)
                                        {
                                            if (boolBeadBand)
                                            {
                                                decUnitPrice = ((((decTypeCost + Convert.ToDecimal(txtBaseRmcb.Text) + ((decSizeCost * decBeadsQty) / decFinishedWt)) * decFinishedWt) / Convert.ToDecimal(dtCurVal.Rows[0][0].ToString())) * 1 + (Convert.ToDecimal(txtNegotiate.Text) / 100));
                                            }
                                            else
                                            {
                                                decUnitPrice = ((decTypeCost + Convert.ToDecimal(txtBaseRmcb.Text)) * decFinishedWt) / Convert.ToDecimal(dtCurVal.Rows[0][0].ToString());
                                            }
                                        }

                                        dtMap.Rows.Add(strSize, strRim, strType, strBrand, strSidewall, txtCurType.Text.ToLower() == "inr" ? Math.Round(Convert.ToDecimal(decUnitPrice), 0).ToString("0.00") : decUnitPrice.ToString("0.00"));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Base records mapping: " + ex.Message);
                                }

                                try
                                {
                                    if (hdnPremiumValue.Value != "")
                                    {
                                        string[] strPremiumVal = hdnPremiumValue.Value.Split('~');
                                        for (int j = 0; j < strPremiumVal.Length; j++)
                                        {
                                            string[] strSplit = strPremiumVal[j].Split(',');
                                            string lblPreType = strSplit[0].ToString();
                                            string txtPreValue = strSplit[1].ToString();

                                            if (txtPreValue != "")
                                            {
                                                foreach (DataRow dtcustRow in dtCustSizes.Select("TyreType='" + lblPreType + "'", "Brand ASC, Sidewall ASC,TypePosition ASC, TyreType ASC, position ASC"))
                                                {
                                                    strSize = ""; strRim = ""; strType = ""; strBrand = ""; strSidewall = ""; decUnitPrice = 0;
                                                    strSize = dtcustRow["TyreSize"].ToString();
                                                    strRim = dtcustRow["RimSize"].ToString();
                                                    strType = dtcustRow["TyreType"].ToString();
                                                    strBrand = dtcustRow["Brand"].ToString();
                                                    strSidewall = dtcustRow["Sidewall"].ToString();

                                                    foreach (DataRow mapRow in dtMap.Select("Size='" + strSize + "' and Rim='" + strRim + "' and Type='" + hdnType.Value + "'"))
                                                    {
                                                        decUnitPrice = Convert.ToDecimal(mapRow["UnitPrice"].ToString()) * (1 + (Convert.ToDecimal(txtPreValue) / 100));
                                                    }

                                                    dtMap.Rows.Add(strSize, strRim, strType, strBrand, strSidewall, txtCurType.Text.ToLower() == "inr" ? Math.Round(Convert.ToDecimal(decUnitPrice), 0).ToString("0.00") : decUnitPrice.ToString("0.00"));
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Premium records mapping: " + ex.Message);
                                }

                                if (dtMap.Rows.Count > 0)
                                {
                                    ViewState["dtMap"] = dtMap;
                                    gv_CustMasterPrice.DataSource = dtMap;
                                    gv_CustMasterPrice.DataBind();
                                }
                                else
                                {
                                    ViewState["dtMap"] = null;
                                    gv_CustMasterPrice.DataSource = null;
                                    gv_CustMasterPrice.DataBind();
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

        protected void btnCustPriceSave_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscriptopen", "showProgress();", true);
                if (gv_CustMasterPrice.Rows.Count == 0)
                {
                    errMsg.InnerText = "No Records in Price List";
                }
                else
                {
                    DataTable dtChk = new DataTable();
                    SqlParameter[] spChk = new SqlParameter[4];
                    spChk[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    spChk[1] = new SqlParameter("@Config", hdnPlatform.Value);
                    spChk[2] = new SqlParameter("@PriceSheetRefNo", txtPriceRefNo.Text);
                    spChk[3] = new SqlParameter("@SizeCategory", hdnCategory.Value);

                    dtChk = (DataTable)daTTS.ExecuteReader_SP("Sp_Chk_CustomerPriceMaster", spChk, DataAccess.Return_Type.DataTable);

                    if (dtChk.Rows.Count > 0)
                    {
                        errMsg.InnerText = "Price Ref No. Already Exists: Enter new Price Ref No. / Click edit button";
                    }
                    else
                    {
                        DataTable dtPriceMap = dtPriceTable_Mapping();
                        if (dtPriceMap.Rows.Count > 0)
                        {
                            SqlParameter[] sp1 = new SqlParameter[5];
                            sp1[0] = new SqlParameter("@Ins_custprice_datatable", dtPriceMap);
                            sp1[1] = new SqlParameter("@CustCode", hdnCustCode.Value);
                            sp1[2] = new SqlParameter("@PriceSheetRefNo", txtPriceRefNo.Text);
                            sp1[3] = new SqlParameter("@Category", hdnCategory.Value);
                            sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                            daTTS.ExecuteNonQuery_SP("Sp_Ins_CustomerPriceMaster_TableWise", sp1);

                            Response.Redirect("default.aspx", false);
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

        protected void btnCustPriceEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscriptopen", "showProgress();", true);
                if (gv_CustMasterPrice.Rows.Count == 0)
                {
                    errMsg.InnerText = "No Records in UnitPrice List";
                }
                else
                {
                    DataTable dtChk = new DataTable();
                    SqlParameter[] spChk = new SqlParameter[4];
                    spChk[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    spChk[1] = new SqlParameter("@Config", hdnPlatform.Value);
                    spChk[2] = new SqlParameter("@PriceSheetRefNo", txtPriceRefNo.Text);
                    spChk[3] = new SqlParameter("@SizeCategory", hdnCategory.Value);

                    dtChk = (DataTable)daTTS.ExecuteReader_SP("Sp_Chk_CustomerPriceMaster", spChk, DataAccess.Return_Type.DataTable);

                    if (dtChk.Rows.Count == 0)
                    {
                        errMsg.InnerText = "Price Ref No. not exists: Do u want save this details plz Click add button";
                    }
                    else
                    {
                        DataTable dtPriceMap = dtPriceTable_Mapping();
                        if (dtPriceMap.Rows.Count > 0)
                        {
                            SqlParameter[] sp1 = new SqlParameter[5];
                            sp1[0] = new SqlParameter("@Ins_custprice_datatable", dtPriceMap);
                            sp1[1] = new SqlParameter("@CustCode", hdnCustCode.Value);
                            sp1[2] = new SqlParameter("@PriceSheetRefNo", txtPriceRefNo.Text);
                            sp1[3] = new SqlParameter("@Category", hdnCategory.Value);
                            sp1[4] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                            daTTS.ExecuteNonQuery_SP("Sp_Edit_CustomerPriceMaster_TableWise", sp1);

                            Response.Redirect("default.aspx", false);
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

        private DataTable dtPriceTable_Mapping()
        {
            DataTable dtPriceMap = new DataTable();
            try
            {
                DataTable dtProcessID = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@Config", hdnPlatform.Value);
                dtProcessID = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_ProcessIDList", sp1, DataAccess.Return_Type.DataTable);

                string[] strPremiumVal = hdnPremiumValue.Value.Split('~');
                Hashtable htPremium = new Hashtable();

                htPremium.Add(hdnType.Value, txtBaseRmcb.Text);
                for (int j = 0; j < strPremiumVal.Length; j++)
                {
                    string[] strSplit = strPremiumVal[j].Split(',');
                    if (strSplit[1].ToString() != "")
                        htPremium.Add(strSplit[0].ToString(), strSplit[1].ToString());
                }

                dtPriceMap = ViewState["dtMap"] as DataTable;
                DataColumn dColumn = new DataColumn("Config", typeof(System.String));
                dColumn.DefaultValue = hdnPlatform.Value;
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("StartDate", typeof(System.DateTime));
                try
                {
                    dColumn.DefaultValue = DateTime.Parse(txtCustPriceAppFrom.Text);
                }
                catch (Exception)
                {
                    dColumn.DefaultValue = DateTime.ParseExact(txtCustPriceAppFrom.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("EndDate", typeof(System.DateTime));
                try
                {
                    dColumn.DefaultValue = DateTime.Parse(txtCustPriceAppTill.Text);
                }
                catch (Exception)
                {
                    dColumn.DefaultValue = DateTime.ParseExact(txtCustPriceAppTill.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("RatesID", typeof(System.String));
                dColumn.DefaultValue = txtRatesID.Text;
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("BaseType", typeof(System.String));
                dColumn.DefaultValue = hdnType.Value;
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("BaseRmcb", typeof(System.Decimal));
                dColumn.DefaultValue = Convert.ToDecimal(txtBaseRmcb.Text).ToString("0.00");
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("PremiumType", typeof(System.String));
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("PremiumValue", typeof(System.Decimal));
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("ProcessID", typeof(System.String));
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("Arv", typeof(System.Decimal));
                dtPriceMap.Columns.Add(dColumn);
                dColumn = new DataColumn("Rmc", typeof(System.Decimal));
                dtPriceMap.Columns.Add(dColumn);

                foreach (DataRow row in dtPriceMap.Rows)
                {
                    row["PremiumType"] = row["Type"].ToString();
                    if (row["Type"].ToString() == hdnType.Value)
                        row["PremiumValue"] = "0.00";
                    else
                        row["PremiumValue"] = Convert.ToDecimal(htPremium[row["PremiumType"].ToString()].ToString()).ToString("0.00");

                    foreach (DataRow processidRow in dtProcessID.Select("Config='" + hdnPlatform.Value + "' AND TyreSize='" + row["Size"].ToString() + "' AND TyreRim='" + row["Rim"].ToString() + "' AND TyreType='" + row["Type"].ToString() + "' AND Brand='" + row["Brand"].ToString() + "' AND Sidewall='" + row["Sidewall"].ToString() + "'"))
                    {
                        row["ProcessID"] = processidRow["ProcessID"].ToString() != "" ? processidRow["ProcessID"].ToString() : null;
                    }
                    row["Arv"] = row["UnitPrice"].ToString();
                    row["Rmc"] = row["UnitPrice"].ToString();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dtPriceMap;
        }

        protected void btnCheckNullValues_Click(object sender, EventArgs e)
        {
            try
            {
                litNullValueCustPrice.Text = "";
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@RatesID", txtRatesID.Text);
                sp1[1] = new SqlParameter("@Cur", txtCurType.Text);

                DataTable dtCurVal = new DataTable();
                dtCurVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CurVal_RatesIDWise", sp1, DataAccess.Return_Type.DataTable);

                if (dtCurVal.Rows.Count == 0)
                    errMsg.InnerHtml = "Currency Value is empty - Please check rates master <br />";
                else
                {
                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    sp2[1] = new SqlParameter("@Config", hdnPlatform.Value);
                    sp2[2] = new SqlParameter("@RatesID", txtRatesID.Text);

                    DataTable dtTypeVal = new DataTable();
                    dtTypeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_TypeDetails_CustTypeWise", sp2, DataAccess.Return_Type.DataTable);

                    if (dtTypeVal.Rows.Count == 0)
                        errMsg.InnerHtml += "Type cost is empty -Please check rates master <br />";
                    else
                    {
                        SqlParameter[] sp3 = new SqlParameter[1];
                        sp3[0] = new SqlParameter("@RatesID", txtRatesID.Text);

                        DataTable dtSizeVal = new DataTable();
                        dtSizeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_SizeVal_RatesIDWise", sp3, DataAccess.Return_Type.DataTable);

                        if (dtSizeVal.Rows.Count == 0)
                            errMsg.InnerHtml += "Size value is empty - Please check rates master <br />";
                        else
                        {
                            SqlParameter[] sp4 = new SqlParameter[1];
                            sp4[0] = new SqlParameter("@Config", hdnPlatform.Value);

                            DataTable dtBeadsVal = new DataTable();
                            dtBeadsVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_BeadQty_ConfigWise", sp4, DataAccess.Return_Type.DataTable);

                            if (dtBeadsVal.Rows.Count == 0)
                                errMsg.InnerHtml += "Beadsqty value is empty - Please check beadband master <br />";
                            else
                            {
                                SqlParameter[] sp6 = new SqlParameter[3];
                                sp6[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                                sp6[1] = new SqlParameter("@Config", hdnPlatform.Value);
                                sp6[2] = new SqlParameter("@SizeCategory", hdnCategory.Value);

                                DataTable dtCustSizes = new DataTable();
                                dtCustSizes = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_SizeRimTypeBrandSidewall_CustWise", sp6, DataAccess.Return_Type.DataTable);

                                DataView dtView = new DataView(dtCustSizes);
                                DataTable distinctSizes = dtView.ToTable(true, "TyreSize", "RimSize");

                                DataView dtView1 = new DataView(dtCustSizes);
                                DataTable distinctSizes1 = dtView1.ToTable(true, "TyreSize", "RimSize", "Brand", "Sidewall", "TyreType");

                                string strNullType = string.Empty;
                                //check type cost
                                foreach (DataRow typeCHK in dtTypeVal.Select("Typecost='0' or Typecost='0.00'"))
                                {
                                    strNullType += typeCHK["type"].ToString() + "<br />";
                                }
                                if (strNullType != "")
                                {
                                    litNullValueCustPrice.Text += "<div style='width:80px; float:left;'><span style='color:#CF2112;font-weight: bold;font-size: 15px;'>Type Cost</span><br />" + strNullType + "</div>";
                                }
                                //check size cost
                                string strNullSize = string.Empty; string strNullBeads = string.Empty; string strNullFinished = string.Empty;
                                foreach (DataRow disRow in distinctSizes.Rows)
                                {
                                    foreach (DataRow sizeCHK in dtSizeVal.Select("SizeVal='0' and TyreSize='" + disRow["TyreSize"].ToString() + "'"))
                                    {
                                        strNullSize += sizeCHK["TyreSize"].ToString() + "<br />";
                                    }
                                    //check beads weight
                                    foreach (DataRow beadsCHK in dtBeadsVal.Select("Beads='0.000' and TyreSize='" + disRow["TyreSize"].ToString() + "' and RimSize='" + disRow["RimSize"].ToString() + "'"))
                                    {
                                        strNullBeads += beadsCHK["TyreSize"].ToString() + "-" + beadsCHK["RimSize"].ToString() + "<br />";
                                    }
                                    //check finished weight
                                    foreach (DataRow finishedCHK in dtBeadsVal.Select("Finished='0.000' and TyreSize='" + disRow["TyreSize"].ToString() + "' and RimSize='" + disRow["RimSize"].ToString() + "'"))
                                    {
                                        strNullFinished += finishedCHK["TyreSize"].ToString() + "-" + finishedCHK["RimSize"].ToString() + "<br />";
                                    }
                                }

                                if (strNullSize != "")
                                    litNullValueCustPrice.Text += "<div style='width:120px; float:left;'><span style='color:#CF2112;font-weight: bold;font-size: 15px;'>TyreSize Cost</span><br />" + strNullSize + "</div>";
                                if (strNullBeads != "")
                                    litNullValueCustPrice.Text += "<div style='width:180px; float:left;'><span style='color:#CF2112;font-weight: bold;font-size: 15px;'>Beadband Wt</span><br />" + strNullBeads + "</div>";
                                if (strNullFinished != "")
                                    litNullValueCustPrice.Text += "<div style='width:320px; float:left;'><span style='color:#CF2112;font-weight: bold;font-size: 15px;'>Finished Wt / Process-ID Missing</span><br />" + strNullFinished + "</div>";

                                if (litNullValueCustPrice.Text == "")
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptNull", "nullValueDivShow('none');", true);
                                else
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptNull", "nullValueDivShow('block');", true);

                                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                            }
                        }
                    }
                }
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
                Utilities.WriteToErrorLog("TTS", "CustPriceMaster.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[Get Price Details WebMethod]
        [WebMethod]
        public static string get_PriceDetails_WebMethod(string strCustCode, string strPriceNo, string strCategory)
        {
            return GetPrice_Details(strCustCode.ToString(), strPriceNo.ToString(), strCategory.ToString()).GetXml();
        }

        private static DataSet GetPrice_Details(string strCustCode, string strPriceNo, string strCategory)
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
                Utilities.WriteToErrorLog("TTS", "CustPriceMaster.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion
    }
}