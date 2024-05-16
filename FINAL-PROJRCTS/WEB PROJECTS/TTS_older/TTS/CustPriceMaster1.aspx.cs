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
using System.Text;
using System.Globalization;

namespace TTS
{
    public partial class CustPriceMaster1 : System.Web.UI.Page
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
                        bindCustName();
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
                ViewState["dtMap"] = null;
                gv_CustMasterPrice.DataSource = null;
                gv_CustMasterPrice.DataBind();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscriptopen", "showProgress();", true);
                errMsg.InnerHtml = "";
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@RatesID", ddlRatesId.Text);
                sp1[1] = new SqlParameter("@Cur", txtCurType.Text);

                DataTable dtCurVal = new DataTable();
                dtCurVal = (DataTable)daTTS.ExecuteReader_SP("SP_SEL_CustPriceMaster_CurVal", sp1, DataAccess.Return_Type.DataTable);

                if (dtCurVal.Rows.Count == 0)
                    errMsg.InnerHtml = "Currency Value is empty - Please check rates master <br />";
                else
                {
                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    sp2[1] = new SqlParameter("@Config", hdnPlatform.Value);
                    sp2[2] = new SqlParameter("@RatesID", ddlRatesId.Text);

                    DataTable dtTypeVal = new DataTable();
                    dtTypeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_TypeDetails", sp2, DataAccess.Return_Type.DataTable);

                    if (dtTypeVal.Rows.Count == 0)
                        errMsg.InnerHtml += "Type cost is empty -Please check rates master <br />";
                    else
                    {
                        SqlParameter[] sp3 = new SqlParameter[1];
                        sp3[0] = new SqlParameter("@RatesID", ddlRatesId.Text);

                        DataTable dtSizeVal = new DataTable();
                        dtSizeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_SizeVal", sp3, DataAccess.Return_Type.DataTable);

                        if (dtSizeVal.Rows.Count == 0)
                            errMsg.InnerHtml += "Size value is empty - Please check rates master <br />";
                        else
                        {
                            SqlParameter[] sp4 = new SqlParameter[1];
                            sp4[0] = new SqlParameter("@Config", hdnPlatform.Value);

                            DataTable dtBeadsVal = new DataTable();
                            dtBeadsVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_BeadQty", sp4, DataAccess.Return_Type.DataTable);

                            if (dtBeadsVal.Rows.Count == 0)
                                errMsg.InnerHtml += "Beadsqty value is empty - Please check beadband master <br />";
                            else
                            {
                                SqlParameter[] sp6 = new SqlParameter[3];
                                sp6[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                                sp6[1] = new SqlParameter("@Config", hdnPlatform.Value);
                                sp6[2] = new SqlParameter("@SizeCategory", hdnCategory.Value);

                                DataTable dtCustSizes = new DataTable();
                                dtCustSizes = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_SizeRimTypeBrandSidewall", sp6, DataAccess.Return_Type.DataTable);

                                if (dtCustSizes.Rows.Count > 0)
                                {
                                    bool boolBeadBand = false;
                                    decimal decTypeCost = 0;
                                    decimal decSizeCost = 0;
                                    decimal decBeadsQty = 0;
                                    decimal decFinishedWt = 0;
                                    decimal decUnitPrice = 0;
                                    string[] arrPremiumVal = new string[] { "" };
                                    string[] arrType = dtCustSizes.AsEnumerable().Where(n => n.Field<string>("TyreType").Equals(hdnType.Value)).Select(n => n.Field<string>("TyreType").ToString()).Distinct().ToArray();
                                    var arrTR = dtCustSizes.AsEnumerable().Where(n => n.Field<string>("TyreType").Equals(hdnType.Value)).Select(n => new { TyreSize = n.Field<string>("TyreSize").ToString(), rimSize = n.Field<string>("RimSize").ToString() }).Distinct().ToArray();

                                    int cntType = arrType.Count();

                                    DataTable dtMap = new DataTable("dtMap");
                                    DataRow dr = dtMap.NewRow();
                                    dtMap.Rows.Add(dr);
                                    // +2 is for heading
                                    for (int i = 0; i < cntType + 2; i++)
                                    {
                                        DataColumn dc = new DataColumn();
                                        dtMap.Columns.Add(dc);
                                        if (i == 1)
                                            dtMap.Rows[0][i] = "TYPE";
                                        if (i > 1)
                                            dtMap.Rows[0][i] = arrType[i - 2].ToString(); // if i = 2 then 2 -2 = 0; array position starts from 0
                                    }

                                    if (hdnPremiumValue.Value != "")
                                    {
                                        arrPremiumVal = hdnPremiumValue.Value.Split('~');
                                        if (arrPremiumVal.Count() > 0)
                                        {
                                            int existColCount = dtMap.Columns.Count;
                                            for (int i = 0; i < arrPremiumVal.Count(); i++)
                                            {
                                                string[] strSplit = arrPremiumVal[i].Split(',');
                                                DataColumn dc = new DataColumn();
                                                dtMap.Columns.Add(dc);
                                                dtMap.Rows[0][i + existColCount] = strSplit[0].ToString();
                                            }
                                        }
                                    }

                                    for (int i = 0; i <= dtMap.Rows.Count; i++)
                                    {
                                        if (i < arrTR.Count() + 2)
                                        {
                                            dr = dtMap.NewRow();
                                            if (i == 1)
                                            {
                                                dtMap.Rows.Add(dr);
                                                dr[0] = "TYRE SIZE";
                                                dr[1] = "RIM SIZE";
                                            }
                                            else if (i > 1)
                                            {
                                                dtMap.Rows.Add(dr);
                                                dr[0] = arrTR[i - 2].TyreSize;
                                                dr[1] = arrTR[i - 2].rimSize;
                                                for (int j = 2; j < dtMap.Columns.Count; j++)
                                                {
                                                    decTypeCost = 0;
                                                    decBeadsQty = 0;
                                                    boolBeadBand = false;
                                                    decFinishedWt = 0;
                                                    decSizeCost = 0;
                                                    decUnitPrice = 0;
                                                    string type = dtMap.Rows[0][j].ToString();
                                                    foreach (DataRow dtTypeRow in dtTypeVal.Select("Type='" + type + "'"))
                                                    {
                                                        boolBeadBand = dtTypeRow["beadband"].ToString() == "Yes" ? true : false;
                                                        decTypeCost = Convert.ToDecimal(dtTypeRow["Typecost"].ToString());
                                                    }

                                                    foreach (DataRow dtSizeRow in dtSizeVal.Select("TyreSize='" + dr[0] + "'")) { decSizeCost = Convert.ToDecimal(dtSizeRow["SizeVal"].ToString()); }

                                                    //decTypeCost = Convert.ToDecimal(((DataRow)dtTypeVal.Select("Type='" + type + "'")[0]).ItemArray[2]);
                                                    //boolBeadBand = ((DataRow)dtTypeVal.Select("Type='" + type + "'")[0]).ItemArray[1].ToString() == "Yes" ? true : false;
                                                    //decSizeCost = Convert.ToDecimal(((DataRow)dtSizeVal.Select("TyreSize='" + dr[0] + "'")[0]).ItemArray[1]);
                                                    foreach (DataRow dtBeadsRow in dtBeadsVal.Select("TyreSize='" + dr[0] + "' and RimSize='" + dr[1] + "'"))
                                                    {
                                                        decBeadsQty = dtBeadsRow["Beads"].ToString() != "" ? Convert.ToDecimal(dtBeadsRow["Beads"]) : 0;
                                                        decFinishedWt = dtBeadsRow["Finished"].ToString() != "" ? Convert.ToDecimal(dtBeadsRow["Finished"].ToString()) : 0;
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
                                                    if (hdnPremiumValue.Value.IndexOf(type) >= 0)
                                                    {
                                                        var arr = arrPremiumVal.AsEnumerable().Where(n => n.ToString().Contains(type)).ToArray();
                                                        for (int index = 0; index < arr.Count(); index++)
                                                        {
                                                            string[] strSplit = arr[index].Split(',');
                                                            string txtPreValue = "";
                                                            if (strSplit[0].ToString() == type) txtPreValue = strSplit[1].ToString();
                                                            if (txtPreValue.ToString() != "")
                                                                decUnitPrice = decUnitPrice * (1 + (Convert.ToDecimal(txtPreValue) / 100));
                                                        }
                                                    }
                                                    dr[j] = (decUnitPrice > 0 ? (txtCurType.Text == "INR" ? Math.Round(decUnitPrice).ToString() : decUnitPrice.ToString("0.00")) : "");
                                                    //dr[j] = (decUnitPrice > 0 ? decUnitPrice.ToString("0.00") : "");
                                                }
                                            }
                                        }
                                    }
                                    if (dtMap.Rows.Count > 0)
                                    {
                                        ViewState["dtMap"] = dtMap;
                                        gv_CustMasterPrice.DataSource = dtMap;
                                        gv_CustMasterPrice.DataBind();
                                    }
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

                    dtChk = (DataTable)daTTS.ExecuteReader_SP("Sp_Chk_CustPriceMaster_CustomerPriceMaster", spChk, DataAccess.Return_Type.DataTable);

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

                            daTTS.ExecuteNonQuery_SP("Sp_Ins_CustPriceMaster_CustomerPriceMaster", sp1);

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

                    dtChk = (DataTable)daTTS.ExecuteReader_SP("Sp_Chk_CustPriceMaster_CustomerPriceMaster", spChk, DataAccess.Return_Type.DataTable);

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

                            daTTS.ExecuteNonQuery_SP("Sp_Edit_CustPriceMaster_CustomerPriceMaster", sp1);

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
                dtProcessID = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_ProcessIDList", sp1, DataAccess.Return_Type.DataTable);

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
                dColumn.DefaultValue = ddlRatesId.Text;
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
                sp1[0] = new SqlParameter("@RatesID", ddlRatesId.Text);
                sp1[1] = new SqlParameter("@Cur", txtCurType.Text);

                DataTable dtCurVal = new DataTable();
                dtCurVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_CurVal_RatesIDWise", sp1, DataAccess.Return_Type.DataTable);

                if (dtCurVal.Rows.Count == 0)
                    errMsg.InnerHtml = "Currency Value is empty - Please check rates master <br />";
                else
                {
                    SqlParameter[] sp2 = new SqlParameter[3];
                    sp2[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                    sp2[1] = new SqlParameter("@Config", hdnPlatform.Value);
                    sp2[2] = new SqlParameter("@RatesID", ddlRatesId.Text);

                    DataTable dtTypeVal = new DataTable();
                    dtTypeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_TypeDetails", sp2, DataAccess.Return_Type.DataTable);

                    if (dtTypeVal.Rows.Count == 0)
                        errMsg.InnerHtml += "Type cost is empty -Please check rates master <br />";
                    else
                    {
                        SqlParameter[] sp3 = new SqlParameter[1];
                        sp3[0] = new SqlParameter("@RatesID", ddlRatesId.Text);

                        DataTable dtSizeVal = new DataTable();
                        dtSizeVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_SizeVal", sp3, DataAccess.Return_Type.DataTable);

                        if (dtSizeVal.Rows.Count == 0)
                            errMsg.InnerHtml += "Size value is empty - Please check rates master <br />";
                        else
                        {
                            SqlParameter[] sp4 = new SqlParameter[1];
                            sp4[0] = new SqlParameter("@Config", hdnPlatform.Value);

                            DataTable dtBeadsVal = new DataTable();
                            dtBeadsVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_BeadQty", sp4, DataAccess.Return_Type.DataTable);

                            if (dtBeadsVal.Rows.Count == 0)
                                errMsg.InnerHtml += "Beadsqty value is empty - Please check beadband master <br />";
                            else
                            {
                                SqlParameter[] sp6 = new SqlParameter[3];
                                sp6[0] = new SqlParameter("@CustCode", hdnCustCode.Value);
                                sp6[1] = new SqlParameter("@Config", hdnPlatform.Value);
                                sp6[2] = new SqlParameter("@SizeCategory", hdnCategory.Value);

                                DataTable dtCustSizes = new DataTable();
                                dtCustSizes = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_SizeRimTypeBrandSidewall", sp6, DataAccess.Return_Type.DataTable);

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

        public void get_CustDetails(string strCustName)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@CustName", strCustName);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("SP_GET_CustPriceMaster_CustDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    hdnCustCode.Value = dt.Rows[0]["CustCode"].ToString();
                    txtCurType.Text = dt.Rows[0]["PriceUnit"].ToString().Substring(0, 3);
                    txtCustPriceSpl.Text = dt.Rows[0]["specialinstruction"].ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["SizeCategory"].ToString().ToLower() == "solid") rdoSolid.Visible = true;
                        else if (dr["SizeCategory"].ToString().ToLower() == "pob") rdoPob.Visible = true;
                        else if (dr["SizeCategory"].ToString().ToLower() == "pneumatic") rdoPneumatic.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            //ScriptManager.RegisterStartupScript(Page, GetType(), "getCustDetails", "getCustDetails('" + data + "');", true);
        }

        public void getPriceDetails(string strCustCode, string strPriceNo, string strCategory)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@CustCode", strCustCode);
                sp1[1] = new SqlParameter("@PriceRefNo", strPriceNo);
                sp1[2] = new SqlParameter("@Category", strCategory);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("SP_GET_CustPriceMaster_PriceDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    txtCustPriceAppFrom.Text = dt.Rows[0]["StartDate"].ToString();
                    txtCustPriceAppTill.Text = dt.Rows[0]["EndDate"].ToString();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindCustName()
        {
            try
            {
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_CustList", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr.ItemArray = new object[] { "--SELECT--" };
                    dt.Rows.InsertAt(dr, 0);
                    ddlCustName.DataSource = dt;
                    ddlCustName.DataTextField = "Custname";
                    ddlCustName.DataValueField = "Custcode";
                    ddlCustName.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindPriceRefNo()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[]{
                    new SqlParameter("@CustCode", ddlCustName.Text)
                };

                DataTable dtPrice = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_CustPriceMaster_PriceSheetRefNo", sp1, DataAccess.Return_Type.DataTable);
                if (dtPrice.Rows.Count > 0)
                {
                    DataRow dr = dtPrice.NewRow();
                    dr.ItemArray = new object[] { "--SELECT--" };
                    dtPrice.Rows.InsertAt(dr, 0);
                    ddlPriceRef.DataSource = dtPrice;
                    ddlPriceRef.DataTextField = "PriceSheetRefNo";
                    ddlPriceRef.DataValueField = "PriceSheetRefNo";
                    ddlPriceRef.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlCustName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DropDownList)sender).SelectedIndex > 0)
            {
                rdoExistingPrice.Enabled = true;
                rdoNewPrice.Enabled = true;
                if (rdoExistingPrice.Checked == true)
                    bindPriceRefNo();
                get_CustDetails(ddlCustName.SelectedItem.Text);
            }
            else
            {
                rdoExistingPrice.Checked = false;
                rdoNewPrice.Checked = false;
                rdoExistingPrice.Enabled = false;
                rdoNewPrice.Enabled = false;
            }
        }

        protected void rdoCategory_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ddlPriceRef.Items.Clear();
                txtPriceRefNo.Text = "";
                ddlRatesId.Items.Clear();
                rdoPob.Checked = false;
                rdoSolid.Checked = false;
                ddlPlatform.Items.Clear();
                ddlType.Items.Clear();
                txtBaseRmcb.Text = "";
                txtNegotiate.Text = "";
                if (((RadioButton)sender).Text == "New Price Ref")
                {
                    txtPriceRefNo.Visible = true;
                    ddlPriceRef.Visible = false;
                    DataTable dtRatesId = (DataTable)daTTS.ExecuteReader_SP("SP_LST_CustPriceMaster_RatesID", DataAccess.Return_Type.DataTable);
                    if (dtRatesId.Rows.Count > 0)
                    {

                        ddlRatesId.DataSource = dtRatesId;
                        ddlRatesId.DataTextField = "RatesID";
                        ddlRatesId.DataValueField = "RatesID";
                        if (dtRatesId.Rows.Count > 1)
                        {
                            DataRow dr = dtRatesId.NewRow();
                            dr.ItemArray = new object[] { "--SELECT--" };
                            dtRatesId.Rows.InsertAt(dr, 0);
                        }
                        ddlRatesId.DataBind();
                    }
                }
                else if (((RadioButton)sender).Text == "Existing Price Ref")
                {
                    ddlPriceRef.Visible = true;
                    txtPriceRefNo.Visible = false;
                    bindPriceRefNo();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlPriceRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlRatesId.Items.Clear();
                rdoPob.Checked = false;
                rdoSolid.Checked = false;
                ddlPlatform.Items.Clear();
                ddlType.Items.Clear();
                ddlRatesId.Items.Clear();
                txtBaseRmcb.Text = "";
                txtNegotiate.Text = "";
                txtPriceRefNo.Text = "";
                //txtCustPriceAppFrom.Text = ""'
                //txtCustPriceAppTill.Text = "";
                if (((DropDownList)sender).SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[]{
                        new SqlParameter("@custcode",ddlCustName.Text),
                        new SqlParameter("@PriceSheetRefNo",ddlPriceRef.Text)
                    };
                    DataTable dtRatesId = (DataTable)daTTS.ExecuteReader_SP("SP_SEL_CustPriceMaster_RatesId", sp, DataAccess.Return_Type.DataTable);
                    if (dtRatesId.Rows.Count > 0)
                    {

                        ddlRatesId.DataSource = dtRatesId;
                        ddlRatesId.DataTextField = "RatesID";
                        ddlRatesId.DataValueField = "RatesID";
                        if (dtRatesId.Rows.Count > 1)
                        {
                            DataRow dr = dtRatesId.NewRow();
                            dr.ItemArray = new object[] { "--SELECT--" };
                            dtRatesId.Rows.InsertAt(dr, 0);
                        }
                        ddlRatesId.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void SizeCategory_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                DataTable dtConfig = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@SizeCategory", ((RadioButton)sender).Text);
                sp1[1] = new SqlParameter("@CustCode", ddlCustName.Text);

                dtConfig = (DataTable)daTTS.ExecuteReader_SP("SP_GET_CustPriceMaster_ApprovedConfig", sp1, DataAccess.Return_Type.DataTable);
                if (dtConfig.Rows.Count > 0)
                {
                    ddlPlatform.DataSource = dtConfig;
                    ddlPlatform.DataTextField = "Config";
                    ddlPlatform.DataValueField = "Config";
                    if (dtConfig.Rows.Count > 1)
                    {
                        DataRow dr = dtConfig.NewRow();
                        dr.ItemArray = new object[] { "--SELECT--" };
                        dtConfig.Rows.InsertAt(dr, 0);
                    }
                    ddlPlatform.DataBind();
                }
                getPriceDetails(ddlCustName.Text, ddlPriceRef.Text, ((RadioButton)sender).Text);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlPlatform_SelectedIndexChange(object sender, EventArgs e)
        {
            try
            {
                hdnPlatform.Value = "";
                ddlType.Items.Clear();
                if (((DropDownList)sender).SelectedIndex > 0)
                {
                    hdnPlatform.Value = ((DropDownList)sender).Text;
                    DataTable dtType = new DataTable();
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@Config", ddlPlatform.Text);
                    sp1[1] = new SqlParameter("@CustCode", ddlCustName.Text);
                    sp1[2] = new SqlParameter("@SizeCategory", rdoSolid.Checked == true ? "solid" : (rdoPob.Checked == true ? "pob" : (rdoPneumatic.Checked == true ? "pneumatic" : "")));

                    dtType = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_CustPriceMaster_ApprovedType", sp1, DataAccess.Return_Type.DataTable);
                    if (dtType.Rows.Count > 0)
                    {
                        ddlType.DataSource = dtType;
                        ddlType.DataTextField = "Tyretype";
                        ddlType.DataValueField = "Tyretype";
                        if (dtType.Rows.Count > 1)
                        {
                            DataRow dr = dtType.NewRow();
                            dr.ItemArray = new object[] { "--SELECT--" };
                            dtType.Rows.InsertAt(dr, 0);
                        }
                        ddlType.DataBind();

                        for (int i = 0; i < gv_CustType.Rows.Count; i++)
                        {

                        }
                    }
                }
                else
                {
                    txtNegotiate.Text = "";
                    txtBaseRmcb.Text = "";
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlRatesId_SelectedIndexChanged(object sender, EventArgs e)
        {
            rdoPob.Checked = false;
            rdoSolid.Checked = false;
            ddlPlatform.Items.Clear();
            ddlType.Items.Clear();
            txtBaseRmcb.Text = "";
            txtNegotiate.Text = "";
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gv_CustMasterPrice.DataSource = null;
                gv_CustMasterPrice.DataBind();
                txtBaseRmcb.Text = "";
                txtNegotiate.Text = "0";
                if (((DropDownList)sender).SelectedIndex > 0)
                {
                    hdnType.Value = ((DropDownList)sender).Text;
                    SqlParameter[] sp1 = new SqlParameter[]{
                        new SqlParameter("@Config", ddlPlatform.Text),
                        new SqlParameter("@CustCode", ddlCustName.Text),
                        new SqlParameter("@TyreType", ((DropDownList)sender).Text),
                        new SqlParameter("@SizeCategory", rdoSolid.Checked == true ? "solid" : (rdoPob.Checked == true ? "pob" : (rdoPneumatic.Checked == true ? "pneumatic" : "")))
                    };

                    DataTable dtType = (DataTable)daTTS.ExecuteReader_SP("SP_GET_CustPriceMaster_ApprovedExceptType", sp1, DataAccess.Return_Type.DataTable);
                    if (dtType.Rows.Count > 0)
                    {
                        string types = "";
                        foreach (DataRow dr in dtType.Rows)
                        {
                            if (types == "") types += dr["TyreType"].ToString();
                            else types += "," + dr["TyreType"].ToString();
                        }
                        ScriptManager.RegisterStartupScript(Page, GetType(), "bindGvCustType", "bindGvCustType('" + types + "');", true);
                    }


                    SqlParameter[] sp2 = new SqlParameter[]{
                        new SqlParameter("@Config", ddlPlatform.Text),
                        new SqlParameter("@CustCode", ddlCustName.Text),
                        new SqlParameter("@BaseType", ((DropDownList)sender).Text),
                        new SqlParameter("@PRefNo", ddlPriceRef.Text)
                    };

                    DataTable dt = (DataTable)daTTS.ExecuteReader_SP("SP_SEL_CustPriceMaster_BaseValue", sp2, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count == 1)
                        txtBaseRmcb.Text = dt.Rows[0]["BaseRmcb"].ToString();


                    SqlParameter[] sp3 = new SqlParameter[]{
                        new SqlParameter("@Config", ddlPlatform.Text),
                        new SqlParameter("@CustCode",  ddlCustName.Text),
                        new SqlParameter("@TyreType", ((DropDownList)sender).Text),
                        new SqlParameter("@SizeCategory", rdoSolid.Checked == true ? "solid" : (rdoPob.Checked == true ? "pob" : (rdoPneumatic.Checked == true ? "pneumatic" : ""))),
                        new SqlParameter("@PRefNo", ddlPriceRef.Text),
                        new SqlParameter("@RatesID", ddlRatesId.Text)
                    };

                    DataTable dtPreVal = (DataTable)daTTS.ExecuteReader_SP("Sp_Get_CustPriceMaster_PremiumTypeVal", sp3, DataAccess.Return_Type.DataTable);
                    string preDetails = "";
                    if (dtPreVal.Rows.Count > 0)
                    {
                        foreach (DataRow dtrow in dtPreVal.Select("PremiumType='" + ((DropDownList)sender).Text + "'"))
                        {
                            preDetails += dtrow["PremiumType"].ToString() + ":" + dtrow["PremiumValue"].ToString() + "~";
                        }

                        foreach (DataRow dtrow in dtPreVal.Select("PremiumType<>'" + ((DropDownList)sender).Text + "'", "PremiumType ASC"))
                        {
                            preDetails += dtrow["PremiumType"].ToString() + ":" + dtrow["PremiumValue"].ToString() + "~";
                        }
                    }
                    if (preDetails.Length > 0)
                        ScriptManager.RegisterStartupScript(Page, GetType(), "bindPremium", "bindPremium('" + preDetails + "');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_CustMasterPrice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header) e.Row.Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].Width = 140;
                    e.Row.Cells[1].Width = 80;
                    if (e.Row.RowIndex < 1)
                    {
                        e.Row.ControlStyle.Font.Bold = true;
                        e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        e.Row.Cells[1].BackColor = System.Drawing.Color.FromArgb(1, 175, 174, 177);
                        e.Row.Cells[0].CssClass += " colheading ";
                        e.Row.Cells[1].CssClass += " colheading ";
                        for (int i = 2; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].CssClass += " colheading ";
                            e.Row.Cells[i].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                        }
                    }
                    if (e.Row.RowIndex >= 1)
                    {
                        e.Row.Cells[0].CssClass += " rowheading ";
                        e.Row.Cells[1].CssClass += " rowheading ";
                        e.Row.Cells[1].CssClass += " align-right";
                        if (e.Row.RowIndex == 1)
                        {
                            e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(1, 175, 174, 177);
                            e.Row.Cells[1].BackColor = System.Drawing.Color.FromArgb(1, 175, 174, 177);
                        }
                        if (e.Row.RowIndex > 1)
                        {
                            e.Row.Cells[0].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                            e.Row.Cells[1].BackColor = System.Drawing.Color.FromArgb(1, 239, 239, 239);
                            for (int i = 2; i < e.Row.Cells.Count; i++)
                            {
                                e.Row.Cells[i].CssClass = "values";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_CustMasterPrice_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "GVModel", "gvModel();", true);
        }
    }

}