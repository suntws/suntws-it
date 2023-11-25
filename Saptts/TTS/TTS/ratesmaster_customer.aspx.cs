using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;

namespace TTS
{
    public partial class ratesmaster_customer : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["pricesheet_build"].ToString() == "True")
                        {
                            lblPageHead.Text = Utilities.Decrypt(Request["shid"].ToString()) + " " + lblPageHead.Text;

                            btnCalculate.Style.Add("display", "none");
                            btnSave_PriceMaster.Style.Add("display", "none");
                            btnCalcSizeWiseEdit.Style.Add("display", "none");
                            SqlParameter[] spS = new SqlParameter[] { new SqlParameter("@SheetType", Utilities.Decrypt(Request["shid"].ToString())) };
                            DataTable dtCust = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PriceSheet_Customer", spS, DataAccess.Return_Type.DataTable);
                            if (dtCust.Rows.Count > 0)
                            {
                                ddlCustomer.DataSource = dtCust;
                                ddlCustomer.DataTextField = "Custname";
                                ddlCustomer.DataValueField = "Custcode";
                                ddlCustomer.DataBind();
                                ddlCustomer.Items.Insert(0, "CHOOSE");
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }

                    trBufferCtrl.Style.Add("dispaly", "none");
                    if (Utilities.Decrypt(Request["shid"].ToString()) == "buffer" && ddlPriceSheetNo.SelectedIndex > 0)
                        trBufferCtrl.Style.Add("display", "block");
                }
                else
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCustomer_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnCalculate.Style.Add("display", "none");
                btnSave_PriceMaster.Style.Add("display", "none");
                btnCalcSizeWiseEdit.Style.Add("display", "none");
                ddlCategory.DataSource = "";
                ddlCategory.DataBind();
                ddlPlatform.DataSource = "";
                ddlPlatform.DataBind();
                ddlBaseType.DataSource = "";
                ddlBaseType.DataBind();
                txtBaseRmcb.Text = "";
                ddlRatesID.DataSource = "";
                ddlRatesID.DataBind();
                ddlPriceSheetNo.DataSource = "";
                ddlPriceSheetNo.DataBind();
                txtNewPriceSheetNo.Text = "";
                lblBaseCost.Text = "TYPE COST";
                lblBaseCostVal.Text = "";
                lblCurrency.Text = "";
                txtConvCur.Text = "";
                lblRatesID_Cur.Text = "";
                lblIncoterm.Text = "";
                txtDestinationPort.Text = "";
                ddlFreightType.SelectedIndex = 0;
                txtFreightCost.Text = "";
                txtClearanceCost.Text = "";
                txtFreightWt.Text = "";
                lblFreightInrKg.Text = "";
                lblClearanceInrKg.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                dlPremiumTypeList.DataSource = null;
                dlPremiumTypeList.DataBind();
                gvTypeBrand.DataSource = null;
                gvTypeBrand.DataBind();
                rdbPriceType.DataSource = "";
                rdbPriceType.DataBind();
                gvPriceMatrix.DataSource = null;
                gvPriceMatrix.DataBind();
                txtIncrementVal.Text = "";
                rdbEditSizeWise.SelectedIndex = -1;

                if (ddlCustomer.SelectedIndex > 0)
                {
                    SqlParameter[] spS = new SqlParameter[] { 
                        new SqlParameter("@SheetType", Utilities.Decrypt(Request["shid"].ToString())), 
                        new SqlParameter("@CustCode", ddlCustomer.SelectedItem.Value) 
                    };
                    DataTable dtCategory = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Cust_Approved_Category", spS, DataAccess.Return_Type.DataTable);
                    if (dtCategory.Rows.Count > 0)
                    {
                        ddlCategory.DataSource = dtCategory;
                        ddlCategory.DataTextField = "Category";
                        ddlCategory.DataValueField = "Category";
                        ddlCategory.DataBind();
                        ddlCategory.Items.Insert(0, "CHOOSE");
                        if (ddlCategory.Items.Count == 2)
                        {
                            ddlCategory.SelectedIndex = 1;
                            ddlCategory_IndexChanged(null, null);
                        }
                        else
                            ddlCategory.SelectedIndex = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCategory_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnCalculate.Style.Add("display", "none");
                btnSave_PriceMaster.Style.Add("display", "none");
                btnCalcSizeWiseEdit.Style.Add("display", "none");
                ddlPlatform.DataSource = "";
                ddlPlatform.DataBind();
                ddlBaseType.DataSource = "";
                ddlBaseType.DataBind();
                txtBaseRmcb.Text = "";
                ddlRatesID.DataSource = "";
                ddlRatesID.DataBind();
                ddlPriceSheetNo.DataSource = "";
                ddlPriceSheetNo.DataBind();
                txtNewPriceSheetNo.Text = "";
                lblBaseCost.Text = "TYPE COST";
                lblBaseCostVal.Text = "";
                lblCurrency.Text = "";
                txtConvCur.Text = "";
                lblRatesID_Cur.Text = "";
                lblIncoterm.Text = "";
                txtDestinationPort.Text = "";
                ddlFreightType.SelectedIndex = 0;
                txtFreightCost.Text = "";
                txtClearanceCost.Text = "";
                txtFreightWt.Text = "";
                lblFreightInrKg.Text = "";
                lblClearanceInrKg.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                dlPremiumTypeList.DataSource = null;
                dlPremiumTypeList.DataBind();
                gvTypeBrand.DataSource = null;
                gvTypeBrand.DataBind();
                rdbPriceType.DataSource = "";
                rdbPriceType.DataBind();
                gvPriceMatrix.DataSource = null;
                gvPriceMatrix.DataBind();
                txtIncrementVal.Text = "";
                rdbEditSizeWise.SelectedIndex = -1;

                if (ddlCategory.SelectedIndex > 0)
                {
                    SqlParameter[] spS = new SqlParameter[] { 
                        new SqlParameter("@SheetType", Utilities.Decrypt(Request["shid"].ToString())), 
                        new SqlParameter("@CustCode", ddlCustomer.SelectedItem.Value),
                        new SqlParameter("@Category", ddlCategory.SelectedItem.Value)
                    };
                    DataTable dtConfig = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Cust_Approved_Platform", spS, DataAccess.Return_Type.DataTable);
                    if (dtConfig.Rows.Count > 0)
                    {
                        ddlPlatform.DataSource = dtConfig;
                        ddlPlatform.DataTextField = "Config";
                        ddlPlatform.DataValueField = "Config";
                        ddlPlatform.DataBind();
                        ddlPlatform.Items.Insert(0, "CHOOSE");
                        if (ddlPlatform.Items.Count == 2)
                        {
                            ddlPlatform.SelectedIndex = 1;
                            ddlPlatform_IndexChanged(null, null);
                        }
                        else
                            ddlPlatform.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlPlatform_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnCalculate.Style.Add("display", "none");
                btnSave_PriceMaster.Style.Add("display", "none");
                btnCalcSizeWiseEdit.Style.Add("display", "none");
                ddlBaseType.DataSource = "";
                ddlBaseType.DataBind();
                txtBaseRmcb.Text = "";
                ddlRatesID.DataSource = "";
                ddlRatesID.DataBind();
                ddlPriceSheetNo.DataSource = "";
                ddlPriceSheetNo.DataBind();
                txtNewPriceSheetNo.Text = "";
                lblBaseCost.Text = "TYPE COST";
                lblBaseCostVal.Text = "";
                lblCurrency.Text = "";
                txtConvCur.Text = "";
                lblRatesID_Cur.Text = "";
                lblIncoterm.Text = "";
                txtDestinationPort.Text = "";
                ddlFreightType.SelectedIndex = 0;
                txtFreightCost.Text = "";
                txtClearanceCost.Text = "";
                txtFreightWt.Text = "";
                lblFreightInrKg.Text = "";
                lblClearanceInrKg.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                dlPremiumTypeList.DataSource = null;
                dlPremiumTypeList.DataBind();
                gvTypeBrand.DataSource = null;
                gvTypeBrand.DataBind();
                rdbPriceType.DataSource = "";
                rdbPriceType.DataBind();
                gvPriceMatrix.DataSource = null;
                gvPriceMatrix.DataBind();
                txtIncrementVal.Text = "";
                rdbEditSizeWise.SelectedIndex = -1;

                if (ddlPlatform.SelectedIndex > 0)
                {
                    SqlParameter[] spS = new SqlParameter[] { 
                        new SqlParameter("@SheetType", Utilities.Decrypt(Request["shid"].ToString())), 
                        new SqlParameter("@CustCode", ddlCustomer.SelectedItem.Value),
                        new SqlParameter("@Category", ddlCategory.SelectedItem.Value),
                        new SqlParameter("@Config", ddlPlatform.SelectedItem.Value)
                    };
                    DataTable dtBase = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Cust_Approved_BaseType", spS, DataAccess.Return_Type.DataTable);
                    if (dtBase.Rows.Count > 0)
                    {
                        ddlBaseType.DataSource = dtBase;
                        ddlBaseType.DataTextField = "BaseType";
                        ddlBaseType.DataValueField = "BaseType";
                        ddlBaseType.DataBind();
                        ddlBaseType.Items.Insert(0, "CHOOSE");
                        if (ddlBaseType.Items.Count == 2)
                        {
                            ddlBaseType.SelectedIndex = 1;
                            ddlBaseType_IndexChanged(null, null);
                        }
                        else
                            ddlBaseType.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlBaseType_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnCalculate.Style.Add("display", "none");
                btnSave_PriceMaster.Style.Add("display", "none");
                btnCalcSizeWiseEdit.Style.Add("display", "none");
                txtBaseRmcb.Text = "";
                ddlRatesID.DataSource = "";
                ddlRatesID.DataBind();
                ddlPriceSheetNo.DataSource = "";
                ddlPriceSheetNo.DataBind();
                txtNewPriceSheetNo.Text = "";
                lblBaseCost.Text = "TYPE COST";
                lblBaseCostVal.Text = "";
                lblCurrency.Text = "";
                txtConvCur.Text = "";
                lblRatesID_Cur.Text = "";
                lblIncoterm.Text = "";
                txtDestinationPort.Text = "";
                ddlFreightType.SelectedIndex = 0;
                txtFreightCost.Text = "";
                txtClearanceCost.Text = "";
                txtFreightWt.Text = "";
                lblFreightInrKg.Text = "";
                lblClearanceInrKg.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                dlPremiumTypeList.DataSource = null;
                dlPremiumTypeList.DataBind();
                gvTypeBrand.DataSource = null;
                gvTypeBrand.DataBind();
                rdbPriceType.DataSource = "";
                rdbPriceType.DataBind();
                gvPriceMatrix.DataSource = null;
                gvPriceMatrix.DataBind();
                txtIncrementVal.Text = "";
                rdbEditSizeWise.SelectedIndex = -1;

                if (ddlBaseType.SelectedIndex > 0)
                {
                    SqlParameter[] spS = new SqlParameter[] { 
                        new SqlParameter("@SheetType", Utilities.Decrypt(Request["shid"].ToString())), 
                        new SqlParameter("@CustCode", ddlCustomer.SelectedItem.Value),
                        new SqlParameter("@Category", ddlCategory.SelectedItem.Value),
                        new SqlParameter("@Config", ddlPlatform.SelectedItem.Value),
                        new SqlParameter("@BaseType", ddlBaseType.SelectedItem.Value)
                    };
                    DataSet dsRates = (DataSet)daCOTS.ExecuteReader_SP("Sp_sel_CustCurrency_RatesID", spS, DataAccess.Return_Type.DataSet);
                    if (dsRates.Tables.Count > 0)
                    {
                        lblCurrency.Text = dsRates.Tables[0].Rows[0]["Currency"].ToString();
                        lblIncoterm.Text = dsRates.Tables[0].Rows[0]["Pricebasis"].ToString();

                        ddlRatesID.DataSource = dsRates.Tables[1];
                        ddlRatesID.DataTextField = "RatesRefID";
                        ddlRatesID.DataValueField = "RatesID";
                        ddlRatesID.DataBind();
                        ddlRatesID.Items.Insert(0, "CHOOSE");
                        if (ddlRatesID.Items.Count == 2)
                        {
                            ddlRatesID.SelectedIndex = 1;
                            ddlRatesID_IndexChanged(null, null);
                        }
                        else
                            ddlRatesID.SelectedIndex = 0;
                    }

                    SqlParameter[] spP = new SqlParameter[] { 
                        new SqlParameter("@SheetType", Utilities.Decrypt(Request["shid"].ToString())), 
                        new SqlParameter("@CustCode", ddlCustomer.SelectedItem.Value),
                        new SqlParameter("@Category", ddlCategory.SelectedItem.Value),
                        new SqlParameter("@Config", ddlPlatform.SelectedItem.Value),
                        new SqlParameter("@BaseType", ddlBaseType.SelectedItem.Value)
                    };
                    DataTable dtPremium = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Cust_Approved_PremiumType", spP, DataAccess.Return_Type.DataTable);
                    if (dtPremium.Rows.Count > 0)
                    {
                        dlPremiumTypeList.DataSource = dtPremium;
                        dlPremiumTypeList.DataBind();
                        ViewState["dtPremium"] = dtPremium;
                    }

                    SqlParameter[] spD = new SqlParameter[] { 
                        new SqlParameter("@SheetType", Utilities.Decrypt(Request["shid"].ToString())), 
                        new SqlParameter("@CustCode", ddlCustomer.SelectedItem.Value),
                        new SqlParameter("@Category", ddlCategory.SelectedItem.Value),
                        new SqlParameter("@Config", ddlPlatform.SelectedItem.Value),
                        new SqlParameter("@BaseType", ddlBaseType.SelectedItem.Value)
                    };
                    DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Cust_Approved_BarndSidewall", spD, DataAccess.Return_Type.DataTable);
                    if (dtData.Rows.Count > 0)
                    {
                        gvTypeBrand.DataSource = dtData;
                        gvTypeBrand.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlRatesID_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnCalculate.Style.Add("display", "none");
                btnSave_PriceMaster.Style.Add("display", "none");
                btnCalcSizeWiseEdit.Style.Add("display", "none");
                ddlPriceSheetNo.DataSource = "";
                ddlPriceSheetNo.DataBind();
                txtNewPriceSheetNo.Text = "";
                lblBaseCost.Text = "TYPE COST";
                lblBaseCostVal.Text = "";
                txtConvCur.Text = "";
                lblRatesID_Cur.Text = "";
                txtDestinationPort.Text = "";
                ddlFreightType.SelectedIndex = 0;
                txtFreightCost.Text = "";
                txtClearanceCost.Text = "";
                txtFreightWt.Text = "";
                lblFreightInrKg.Text = "";
                lblClearanceInrKg.Text = "";
                rdbPriceType.DataSource = "";
                rdbPriceType.DataBind();
                gvPriceMatrix.DataSource = null;
                gvPriceMatrix.DataBind();
                txtIncrementVal.Text = "";
                rdbEditSizeWise.SelectedIndex = -1;

                if (ddlRatesID.SelectedIndex > 0)
                {
                    SqlParameter[] spD = new SqlParameter[] { 
                        new SqlParameter("@SheetType", Utilities.Decrypt(Request["shid"].ToString())), 
                        new SqlParameter("@CustCode", ddlCustomer.SelectedItem.Value),
                        new SqlParameter("@Category", ddlCategory.SelectedItem.Value),
                        new SqlParameter("@Config", ddlPlatform.SelectedItem.Value),
                        new SqlParameter("@BaseType", ddlBaseType.SelectedItem.Value),
                        new SqlParameter("@RatesID", ddlRatesID.SelectedItem.Value)
                    };
                    DataSet dsData = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Cust_PriceSheetRefno", spD, DataAccess.Return_Type.DataSet);
                    if (dsData.Tables.Count > 0)
                    {
                        foreach (DataRow cRow in dsData.Tables[1].Select("TyreType='" + ddlBaseType.SelectedItem.Text + "'"))
                        {
                            lblBaseCostVal.Text = cRow["TypeValue"].ToString();
                        }
                        lblBaseCost.Text = ddlBaseType.SelectedItem.Text + " TYPE COST";
                        lblRatesID_Cur.Text = dsData.Tables[2].Rows[0]["Cur"].ToString();

                        if (dsData.Tables[0].Rows.Count > 0)
                        {
                            ddlPriceSheetNo.DataSource = dsData.Tables[0];
                            ddlPriceSheetNo.DataTextField = "PriceSheetRefNo";
                            ddlPriceSheetNo.DataValueField = "PriceID";
                            ddlPriceSheetNo.DataBind();
                            ddlPriceSheetNo.Items.Insert(0, "CHOOSE");
                            ddlPriceSheetNo.SelectedIndex = 0;
                        }
                    }
                }
                if (Utilities.Decrypt(Request["shid"].ToString()) == "master")
                    btnCalculate.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlPriceSheetNo_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnCalculate.Style.Add("display", "block");
                btnSave_PriceMaster.Style.Add("display", "none");
                btnCalcSizeWiseEdit.Style.Add("display", "none");
                txtNewPriceSheetNo.Text = "";
                lblBaseCost.Text = "TYPE COST";
                lblBaseCostVal.Text = "";
                lblCurrency.Text = "";
                txtConvCur.Text = "";
                lblRatesID_Cur.Text = "";
                lblIncoterm.Text = "";
                txtDestinationPort.Text = "";
                ddlFreightType.SelectedIndex = 0;
                txtFreightCost.Text = "";
                txtClearanceCost.Text = "";
                txtFreightWt.Text = "";
                lblFreightInrKg.Text = "";
                lblClearanceInrKg.Text = "";
                dlPremiumTypeList.DataSource = null;
                dlPremiumTypeList.DataBind();
                gvTypeBrand.DataSource = null;
                gvTypeBrand.DataBind();
                rdbPriceType.DataSource = "";
                rdbPriceType.DataBind();
                gvPriceMatrix.DataSource = null;
                gvPriceMatrix.DataBind();
                txtIncrementVal.Text = "";
                rdbEditSizeWise.SelectedIndex = -1;

                if (ddlPriceSheetNo.SelectedIndex > 0)
                {
                    SqlParameter[] spS = new SqlParameter[] { new SqlParameter("@PriceID", ddlPriceSheetNo.SelectedItem.Value) };
                    DataSet dsPrice = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Cust_PriceList_Exists", spS, DataAccess.Return_Type.DataSet);
                    if (dsPrice.Tables.Count > 0)
                    {
                        SqlParameter[] spC = new SqlParameter[] { 
                            new SqlParameter("@RatesID", ddlRatesID.SelectedItem.Value), 
                            new SqlParameter("@BaseType", ddlBaseType.SelectedItem.Text) 
                        };
                        DataTable dtCost = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_TypeCost_BaseType", spC, DataAccess.Return_Type.DataTable);
                        if (dtCost != null && dtCost.Rows[0][0].ToString() != "")
                            lblBaseCostVal.Text = dtCost.Rows[0][0].ToString();

                        txtBaseRmcb.Text = dsPrice.Tables[1].Rows[0]["BaseRmcb"].ToString();
                        txtFromDate.Text = dsPrice.Tables[1].Rows[0]["StartDate"].ToString();
                        txtToDate.Text = dsPrice.Tables[1].Rows[0]["EndDate"].ToString();
                        lblCurrency.Text = dsPrice.Tables[1].Rows[0]["CustCur"].ToString();
                        txtConvCur.Text = dsPrice.Tables[1].Rows[0]["CurRate"].ToString();
                        lblRatesID_Cur.Text = dsPrice.Tables[1].Rows[0]["RatesID_Cur"].ToString();
                        lblIncoterm.Text = dsPrice.Tables[1].Rows[0]["Incoterm"].ToString();
                        txtDestinationPort.Text = dsPrice.Tables[1].Rows[0]["DestinationPort"].ToString();
                        ddlFreightType.SelectedIndex = ddlFreightType.Items.IndexOf(ddlFreightType.Items.FindByValue(dsPrice.Tables[1].Rows[0]["FreightType"].ToString()));
                        txtFreightWt.Text = dsPrice.Tables[1].Rows[0]["FreightWt"].ToString();
                        txtFreightCost.Text = dsPrice.Tables[1].Rows[0]["FreightCost"].ToString();
                        txtClearanceCost.Text = dsPrice.Tables[1].Rows[0]["ClearanceCost"].ToString();
                        ddlSubCategory.SelectedIndex = ddlSubCategory.Items.IndexOf(ddlSubCategory.Items.FindByValue(dsPrice.Tables[1].Rows[0]["SubCategory"].ToString()));

                        lblClearanceInrKg.Text = Math.Round(((Convert.ToDecimal(txtClearanceCost.Text) * Convert.ToDecimal(txtConvCur.Text)) /
                                Convert.ToDecimal(txtFreightWt.Text)), 2).ToString();
                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                        {
                            lblFreightInrKg.Text = Math.Round(((Convert.ToDecimal(txtFreightCost.Text) * Convert.ToDecimal(txtConvCur.Text)) /
                                Convert.ToDecimal(txtFreightWt.Text)), 2).ToString();
                        }

                        if (dsPrice.Tables[2].Rows.Count > 0)
                        {
                            dlPremiumTypeList.DataSource = dsPrice.Tables[2];
                            dlPremiumTypeList.DataBind();
                        }

                        if (dsPrice.Tables[0].Rows.Count > 0)
                        {
                            DataView dView = new DataView(dsPrice.Tables[0]);
                            dView.Sort = "Config,TyreType,brand,Sidewall";
                            DataTable dtApprove = dView.ToTable(true, "Config", "TyreType", "brand", "Sidewall");

                            gvTypeBrand.DataSource = dtApprove;
                            gvTypeBrand.DataBind();

                            foreach (GridViewRow gRow in gvTypeBrand.Rows)
                            {
                                ((CheckBox)gRow.FindControl("chk_SelectRows")).Checked = true;
                            }

                            build_Master_PriceMatrix_Exists(dsPrice.Tables[0], dtApprove, dsPrice.Tables[2]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                rdbPriceType.DataSource = "";
                rdbPriceType.DataBind();
                gvPriceMatrix.DataSource = null;
                gvPriceMatrix.DataBind();

                DataTable dtPremium = ViewState["dtPremium"] as DataTable;
                if (dtPremium.Rows.Count > 0)
                {
                    foreach (DataListItem dlItem in dlPremiumTypeList.Items)
                    {
                        if (((TextBox)dlItem.FindControl("txtPremiumVal")).Text != "")
                        {
                            foreach (DataRow pRow in dtPremium.Select("PremiumType='" + ((Label)dlItem.FindControl("lblPremiumType")).Text + "'"))
                            {
                                pRow["PremiumValue"] = ((TextBox)dlItem.FindControl("txtPremiumVal")).Text;
                            }
                        }
                    }
                }

                DataTable dtApprove = new DataTable();
                dtApprove.Columns.Add("Config", typeof(string));
                dtApprove.Columns.Add("Tyretype", typeof(string));
                dtApprove.Columns.Add("brand", typeof(string));
                dtApprove.Columns.Add("Sidewall", typeof(string));

                foreach (GridViewRow gRow in gvTypeBrand.Rows)
                {
                    CheckBox chk_SelectRows = gRow.FindControl("chk_SelectRows") as CheckBox;
                    if (chk_SelectRows.Checked)
                        dtApprove.Rows.Add(gRow.Cells[0].Text, gRow.Cells[1].Text, gRow.Cells[2].Text, gRow.Cells[3].Text);
                }

                if (dtApprove.Rows.Count > 0)
                {
                    SqlParameter[] spS = new SqlParameter[] { 
                        new SqlParameter("@SizeRimTypeBrandSidewall_DT", dtApprove), 
                        new SqlParameter("@Category", ddlCategory.SelectedItem.Text) 
                    };
                    DataTable dtApprovePID = (DataTable)daTTS.ExecuteReader_SP("sp_sel_cust_SizeRimTypeBrandSidewall", spS, DataAccess.Return_Type.DataTable);
                    if (dtApprovePID.Rows.Count > 0)
                    {
                        if (Utilities.Decrypt(Request["shid"].ToString()) == "master")
                            build_Master_PriceMatrix(dtApprovePID, dtApprove, dtPremium);
                        else if (Utilities.Decrypt(Request["shid"].ToString()) == "buffer")
                            build_Master_PriceMatrix_Buffer(dtApprovePID, dtApprove, dtPremium);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void build_Master_PriceMatrix(DataTable dtPID, DataTable dtAppCategory, DataTable dtPre)
        {
            try
            {
                DataView dtView = new DataView(dtPID);
                dtView.RowFilter = "TyreType='" + ddlBaseType.SelectedItem.Text + "'";
                DataTable distinctList = dtView.ToTable(true, "TyreSize", "RimSize", "FinishedWt", "SizePosition");
                distinctList.Columns.Remove("SizePosition");

                lblClearanceInrKg.Text = Math.Round(((Convert.ToDecimal(txtClearanceCost.Text) * Convert.ToDecimal(txtConvCur.Text)) /
                        Convert.ToDecimal(txtFreightWt.Text)), 2).ToString();
                if (!lblIncoterm.Text.ToLower().Contains("fob"))
                {
                    lblFreightInrKg.Text = Math.Round(((Convert.ToDecimal(txtFreightCost.Text) * Convert.ToDecimal(txtConvCur.Text)) /
                        Convert.ToDecimal(txtFreightWt.Text)), 2).ToString();
                }

                foreach (DataRow aRow in dtAppCategory.Select("Tyretype='" + ddlBaseType.SelectedItem.Text + "'"))
                {
                    distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- RMCB", typeof(decimal));
                    distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- FOB", typeof(decimal));
                    if (!lblIncoterm.Text.ToLower().Contains("fob"))
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- " + lblIncoterm.Text, typeof(decimal));
                    foreach (DataRow mRow in distinctList.Rows)
                    {
                        decimal decFOB = Math.Round((((Convert.ToDecimal(lblBaseCostVal.Text) + Convert.ToDecimal(txtBaseRmcb.Text) +
                            Convert.ToDecimal(lblClearanceInrKg.Text)) * Convert.ToDecimal(mRow[2].ToString())) / Convert.ToDecimal(txtConvCur.Text)), 2);
                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                        {
                            mRow[distinctList.Columns.Count - 1] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                Convert.ToDecimal(mRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                            mRow[distinctList.Columns.Count - 2] = decFOB.ToString("0.00");
                            mRow[distinctList.Columns.Count - 3] = Convert.ToDecimal(txtBaseRmcb.Text).ToString("0.00");
                        }
                        else
                        {
                            mRow[distinctList.Columns.Count - 1] = decFOB.ToString("0.00");
                            mRow[distinctList.Columns.Count - 2] = Convert.ToDecimal(txtBaseRmcb.Text).ToString("0.00");
                        }
                    }

                    if (Utilities.Decrypt(Request["shid"].ToString()) == "buffer")
                    {
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW RMCB", typeof(decimal));
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW FOB", typeof(decimal));
                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW " + lblIncoterm.Text, typeof(decimal));

                        foreach (DataRow mRow in distinctList.Rows)
                        {
                            if (!lblIncoterm.Text.ToLower().Contains("fob"))
                            {
                                mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 4];
                                mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 5];
                                mRow[distinctList.Columns.Count - 3] = mRow[distinctList.Columns.Count - 6];
                            }
                            else
                            {
                                mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 3];
                                mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 4];
                            }
                        }
                    }
                }
                hdnGvColCount.Value = distinctList.Columns.Count.ToString();

                if (dtPre != null && dtPre.Rows.Count > 0)
                {
                    foreach (DataRow aRow in dtAppCategory.Select("Tyretype<>'" + ddlBaseType.SelectedItem.Text + "'"))
                    {
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- RMCB", typeof(decimal));
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- FOB", typeof(decimal));
                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- " + lblIncoterm.Text, typeof(decimal));

                        decimal decPremiumVal = 0;
                        foreach (DataRow pRow in dtPre.Select("PremiumType='" + aRow["Tyretype"].ToString() + "'"))
                        {
                            decPremiumVal = Convert.ToDecimal(pRow["PremiumValue"].ToString());
                        }

                        foreach (DataRow mRow in distinctList.Rows)
                        {
                            //foreach (DataRow cRow in dtPID.Select("Tyretype='" + aRow["Tyretype"].ToString() + "' and brand='" + aRow["brand"].ToString() + "' and Sidewall='"
                            //    + aRow["Sidewall"].ToString() + "' and TyreSize='" + mRow["TyreSize"].ToString() + "' and RimSize='" + mRow["RimSize"].ToString() + "'"))
                            //{
                            decimal decFOB = Math.Round(Convert.ToDecimal(mRow[4].ToString()) * (1 + (decPremiumVal / 100)), 2);
                            decimal decRMCB = Math.Round((((decFOB * Convert.ToDecimal(txtConvCur.Text)) / Convert.ToDecimal(mRow[2].ToString())) -
                                Convert.ToDecimal(lblBaseCostVal.Text) - Convert.ToDecimal(lblClearanceInrKg.Text)), 2);
                            if (!lblIncoterm.Text.ToLower().Contains("fob"))
                            {
                                mRow[distinctList.Columns.Count - 1] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                Convert.ToDecimal(mRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                                mRow[distinctList.Columns.Count - 2] = decFOB.ToString("0.00");
                                mRow[distinctList.Columns.Count - 3] = decRMCB.ToString("0.00");
                            }
                            else
                            {
                                mRow[distinctList.Columns.Count - 1] = decFOB.ToString("0.00");
                                mRow[distinctList.Columns.Count - 2] = decRMCB.ToString("0.00");
                            }
                            //}
                        }

                        if (Utilities.Decrypt(Request["shid"].ToString()) == "buffer")
                        {
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW RMCB", typeof(decimal));
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW FOB", typeof(decimal));
                            if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW " + lblIncoterm.Text, typeof(decimal));

                            foreach (DataRow mRow in distinctList.Rows)
                            {
                                if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                {
                                    mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 4];
                                    mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 5];
                                    mRow[distinctList.Columns.Count - 3] = mRow[distinctList.Columns.Count - 6];
                                }
                                else
                                {
                                    mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 3];
                                    mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 4];
                                }
                            }
                        }
                    }
                }

                DataView dvRpt = new DataView(dtAppCategory);
                DataTable dtRdbPrice = dvRpt.ToTable(true, "Tyretype");
                foreach (DataRow rRow in dtRdbPrice.Select("Tyretype='" + ddlBaseType.SelectedItem.Text + "'"))
                {
                    rRow.Delete();
                }
                rdbPriceType.DataSource = dtRdbPrice;
                rdbPriceType.DataTextField = "Tyretype";
                rdbPriceType.DataValueField = "Tyretype";
                rdbPriceType.DataBind();

                gvPriceMatrix.DataSource = distinctList;
                gvPriceMatrix.DataBind();
                ViewState["dtPriceMatrix"] = distinctList;
                ScriptManager.RegisterStartupScript(Page, GetType(), "JGvHide", "BuildGridViewHeader();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JPreview", "gotoPreviewDiv('MainContent_gvPriceMatrix');", true);
                btnSave_PriceMaster.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void build_Master_PriceMatrix_Exists(DataTable dtPID, DataTable dtAppCategory, DataTable dtPre)
        {
            try
            {
                DataView dtView = new DataView(dtPID);
                dtView.RowFilter = "TyreType='" + ddlBaseType.SelectedItem.Text + "'";
                DataTable distinctList = dtView.ToTable(true, "TyreSize", "RimSize", "FinishedWt", "SizePosition");
                distinctList.Columns.Remove("SizePosition");

                foreach (DataRow aRow in dtAppCategory.Select("Tyretype='" + ddlBaseType.SelectedItem.Text + "'"))
                {
                    distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- RMCB", typeof(decimal));
                    distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- FOB", typeof(decimal));
                    if (!lblIncoterm.Text.ToLower().Contains("fob"))
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- " + lblIncoterm.Text, typeof(decimal));
                    foreach (DataRow mRow in distinctList.Rows)
                    {
                        decimal decFOB = 0;
                        foreach (DataRow eRow in dtPID.Select("TyreSize='" + mRow[0].ToString() + "' and RimSize='" + mRow[1].ToString() + "' and TyreType='" +
                            aRow["Tyretype"].ToString() + "' and Brand='" + aRow["brand"].ToString() + "' and Sidewall='" + aRow["Sidewall"].ToString() + "'"))
                        {
                            decFOB = Convert.ToDecimal(eRow["UnitPrice"].ToString());
                        }
                        decimal decRMCB = Math.Round((((decFOB * Convert.ToDecimal(txtConvCur.Text)) / Convert.ToDecimal(mRow[2].ToString())) -
                            Convert.ToDecimal(lblBaseCostVal.Text) - Convert.ToDecimal(lblClearanceInrKg.Text)), 2);
                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                        {
                            mRow[distinctList.Columns.Count - 1] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                Convert.ToDecimal(mRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                            mRow[distinctList.Columns.Count - 2] = decFOB.ToString("0.00");
                            mRow[distinctList.Columns.Count - 3] = decRMCB.ToString("0.00");
                        }
                        else
                        {
                            mRow[distinctList.Columns.Count - 1] = decFOB.ToString("0.00");
                            mRow[distinctList.Columns.Count - 2] = decRMCB.ToString("0.00");
                        }
                    }

                    if (Utilities.Decrypt(Request["shid"].ToString()) == "buffer")
                    {
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW RMCB", typeof(decimal));
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW FOB", typeof(decimal));
                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW " + lblIncoterm.Text, typeof(decimal));

                        foreach (DataRow mRow in distinctList.Rows)
                        {
                            if (!lblIncoterm.Text.ToLower().Contains("fob"))
                            {
                                mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 4];
                                mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 5];
                                mRow[distinctList.Columns.Count - 3] = mRow[distinctList.Columns.Count - 6];
                            }
                            else
                            {
                                mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 3];
                                mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 4];
                            }
                        }
                    }
                }
                hdnGvColCount.Value = distinctList.Columns.Count.ToString();

                if (dtPre != null && dtPre.Rows.Count > 0)
                {
                    foreach (DataRow aRow in dtAppCategory.Select("Tyretype<>'" + ddlBaseType.SelectedItem.Text + "'"))
                    {
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- RMCB", typeof(decimal));
                        distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- FOB", typeof(decimal));
                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- " + lblIncoterm.Text, typeof(decimal));

                        foreach (DataRow mRow in distinctList.Rows)
                        {
                            foreach (DataRow cRow in dtPID.Select("Tyretype='" + aRow["Tyretype"].ToString() + "' and brand='" + aRow["brand"].ToString() + "' and Sidewall='"
                                + aRow["Sidewall"].ToString() + "' and TyreSize='" + mRow["TyreSize"].ToString() + "' and RimSize='" + mRow["RimSize"].ToString() + "'"))
                            {
                                decimal decFOB = 0;
                                foreach (DataRow eRow in dtPID.Select("TyreSize='" + mRow[0].ToString() + "' and RimSize='" + mRow[1].ToString() + "' and TyreType='" +
                                    aRow["Tyretype"].ToString() + "' and Brand='" + aRow["brand"].ToString() + "' and Sidewall='" + aRow["Sidewall"].ToString() + "'"))
                                {
                                    decFOB = Convert.ToDecimal(eRow["UnitPrice"].ToString());
                                }
                                decimal decRMCB = Math.Round((((decFOB * Convert.ToDecimal(txtConvCur.Text)) / Convert.ToDecimal(mRow[2].ToString())) -
                                    Convert.ToDecimal(lblBaseCostVal.Text) - Convert.ToDecimal(lblClearanceInrKg.Text)), 2);
                                if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                {
                                    mRow[distinctList.Columns.Count - 1] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                        Convert.ToDecimal(mRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                                    mRow[distinctList.Columns.Count - 2] = decFOB.ToString("0.00");
                                    mRow[distinctList.Columns.Count - 3] = decRMCB.ToString("0.00");
                                }
                                else
                                {
                                    mRow[distinctList.Columns.Count - 1] = decFOB.ToString("0.00");
                                    mRow[distinctList.Columns.Count - 2] = decRMCB.ToString("0.00");
                                }
                            }
                        }

                        if (Utilities.Decrypt(Request["shid"].ToString()) == "buffer")
                        {
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW RMCB", typeof(decimal));
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW FOB", typeof(decimal));
                            if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW " + lblIncoterm.Text, typeof(decimal));

                            foreach (DataRow mRow in distinctList.Rows)
                            {
                                if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                {
                                    mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 4];
                                    mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 5];
                                    mRow[distinctList.Columns.Count - 3] = mRow[distinctList.Columns.Count - 6];
                                }
                                else
                                {
                                    mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 3];
                                    mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 4];
                                }
                            }
                        }
                    }
                }

                DataView dvRpt = new DataView(dtAppCategory);
                DataTable dtRdbPrice = dvRpt.ToTable(true, "Tyretype");
                foreach (DataRow rRow in dtRdbPrice.Select("Tyretype='" + ddlBaseType.SelectedItem.Text + "'"))
                {
                    rRow.Delete();
                }
                rdbPriceType.DataSource = dtRdbPrice;
                rdbPriceType.DataTextField = "Tyretype";
                rdbPriceType.DataValueField = "Tyretype";
                rdbPriceType.DataBind();

                gvPriceMatrix.DataSource = distinctList;
                gvPriceMatrix.DataBind();
                ViewState["dtPriceMatrix"] = distinctList;
                ScriptManager.RegisterStartupScript(Page, GetType(), "JGvHide", "BuildGridViewHeader();", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JPreview", "gotoPreviewDiv('MainContent_gvPriceMatrix');", true);
                btnSave_PriceMaster.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void build_Master_PriceMatrix_Buffer(DataTable dtPID, DataTable dtAppCategory, DataTable dtPre)
        {
            try
            {
                DataTable distinctList = ViewState["dtPriceMatrix"] as DataTable;
                if (distinctList.Rows.Count > 0)
                {
                    for (int dCol = distinctList.Columns.Count - 1; dCol > 2; dCol--)
                    {
                        bool dtColStatus = false;
                        string[] strSplit = distinctList.Columns[dCol].ColumnName.Split('|');
                        foreach (DataRow aRow in dtAppCategory.Select("TyreType='" + strSplit[0] + "'"))
                        {
                            dtColStatus = true;
                        }
                        if (!dtColStatus)
                            distinctList.Columns.RemoveAt(dCol);
                    }

                    foreach (DataRow aRow in dtAppCategory.Rows)
                    {
                        bool boolColExists = false;
                        string strColName = aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- FOB";
                        foreach (DataColumn dCol in distinctList.Columns)
                        {
                            if (dCol.ColumnName == strColName)
                            {
                                boolColExists = true;
                                break;
                            }
                        }
                        if (!boolColExists)
                        {
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- RMCB", typeof(decimal));
                            distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- FOB", typeof(decimal));
                            if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- " + lblIncoterm.Text, typeof(decimal));

                            decimal decPremiumVal = 0;
                            foreach (DataRow pRow in dtPre.Select("PremiumType='" + aRow["Tyretype"].ToString() + "'"))
                            {
                                decPremiumVal = Convert.ToDecimal(pRow["PremiumValue"].ToString());
                            }

                            foreach (DataRow mRow in distinctList.Rows)
                            {
                                //foreach (DataRow cRow in dtPID.Select("Tyretype='" + aRow["Tyretype"].ToString() + "' and brand='" + aRow["brand"].ToString() + "' and Sidewall='"
                                //    + aRow["Sidewall"].ToString() + "' and TyreSize='" + mRow["TyreSize"].ToString() + "' and RimSize='" + mRow["RimSize"].ToString() + "'"))
                                //{
                                decimal decFOB = Math.Round(Convert.ToDecimal(mRow[4].ToString()) * (1 + (decPremiumVal / 100)), 2);
                                decimal decRMCB = Math.Round((((decFOB * Convert.ToDecimal(txtConvCur.Text)) / Convert.ToDecimal(mRow[2].ToString())) -
                                    Convert.ToDecimal(lblBaseCostVal.Text) - Convert.ToDecimal(lblClearanceInrKg.Text)), 2);
                                if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                {
                                    mRow[distinctList.Columns.Count - 1] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                        Convert.ToDecimal(mRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                                    mRow[distinctList.Columns.Count - 2] = decFOB.ToString("0.00");
                                    mRow[distinctList.Columns.Count - 3] = decRMCB.ToString("0.00");
                                }
                                else
                                {
                                    mRow[distinctList.Columns.Count - 1] = decFOB.ToString("0.00");
                                    mRow[distinctList.Columns.Count - 2] = decRMCB.ToString("0.00");
                                }
                                //}
                            }

                            if (Utilities.Decrypt(Request["shid"].ToString()) == "buffer")
                            {
                                distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW RMCB", typeof(decimal));
                                distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW FOB", typeof(decimal));
                                if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                    distinctList.Columns.Add(aRow["Tyretype"].ToString() + "|" + aRow["brand"].ToString() + "|" + aRow["Sidewall"].ToString() + "|- NEW " + lblIncoterm.Text, typeof(decimal));

                                foreach (DataRow mRow in distinctList.Rows)
                                {
                                    if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                    {
                                        mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 4];
                                        mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 5];
                                        mRow[distinctList.Columns.Count - 3] = mRow[distinctList.Columns.Count - 6];
                                    }
                                    else
                                    {
                                        mRow[distinctList.Columns.Count - 1] = mRow[distinctList.Columns.Count - 3];
                                        mRow[distinctList.Columns.Count - 2] = mRow[distinctList.Columns.Count - 4];
                                    }
                                }
                            }
                        }
                    }

                    lblClearanceInrKg.Text = Math.Round(((Convert.ToDecimal(txtClearanceCost.Text) * Convert.ToDecimal(txtConvCur.Text)) /
                           Convert.ToDecimal(txtFreightWt.Text)), 2).ToString();
                    if (!lblIncoterm.Text.ToLower().Contains("fob"))
                    {
                        lblFreightInrKg.Text = Math.Round(((Convert.ToDecimal(txtFreightCost.Text) * Convert.ToDecimal(txtConvCur.Text)) /
                            Convert.ToDecimal(txtFreightWt.Text)), 2).ToString();
                    }

                    int iCol = 0;
                    foreach (DataColumn dCol in distinctList.Columns)
                    {
                        if (dCol.ColumnName.Contains("- NEW FOB"))
                        {
                            string[] spHead = dCol.ColumnName.Split('|');
                            if (spHead[0].ToString() == ddlBaseType.SelectedItem.Text)
                            {
                                foreach (DataRow mRow in distinctList.Rows)
                                {
                                    decimal decFOB = Math.Round((((Convert.ToDecimal(lblBaseCostVal.Text) + Convert.ToDecimal(txtBaseRmcb.Text) +
                                        Convert.ToDecimal(lblClearanceInrKg.Text)) * Convert.ToDecimal(mRow[2].ToString())) / Convert.ToDecimal(txtConvCur.Text)), 2);
                                    if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                    {
                                        mRow[iCol + 1] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                            Convert.ToDecimal(mRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                                        mRow[iCol] = decFOB.ToString("0.00");
                                        mRow[iCol - 1] = Math.Round(Convert.ToDecimal(txtBaseRmcb.Text), 2).ToString("0.00");
                                    }
                                    else
                                    {
                                        mRow[iCol] = decFOB.ToString("0.00");
                                        mRow[iCol - 1] = Math.Round(Convert.ToDecimal(txtBaseRmcb.Text), 2).ToString("0.00");
                                    }
                                }
                            }
                            else if (spHead[0].ToString() != ddlBaseType.SelectedItem.Text)
                            {
                                if (dtPre != null && dtPre.Rows.Count > 0)
                                {
                                    decimal decPremiumVal = 0;
                                    foreach (DataRow pRow in dtPre.Select("PremiumType='" + spHead[0].ToString() + "'"))
                                    {
                                        decPremiumVal = Convert.ToDecimal(pRow["PremiumValue"].ToString());
                                    }
                                    foreach (DataRow mRow in distinctList.Rows)
                                    {
                                        //foreach (DataRow cRow in dtPID.Select("Tyretype='" + spHead[0].ToString() + "' and brand='" + spHead[1].ToString() + "' and Sidewall='"
                                        //    + spHead[2].ToString() + "' and TyreSize='" + mRow["TyreSize"].ToString() + "' and RimSize='" + mRow["RimSize"].ToString() + "'"))
                                        //{
                                        decimal decFOB = Math.Round(Convert.ToDecimal(mRow[(!lblIncoterm.Text.ToLower().Contains("fob") ? 7 : 6)].ToString()) *
                                            (1 + (decPremiumVal / 100)), 2);
                                        decimal decRMCB = Math.Round((((decFOB * Convert.ToDecimal(txtConvCur.Text)) / Convert.ToDecimal(mRow[2].ToString())) -
                                            Convert.ToDecimal(lblBaseCostVal.Text) - Convert.ToDecimal(lblClearanceInrKg.Text)), 2);
                                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                        {
                                            mRow[iCol + 1] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                                Convert.ToDecimal(mRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                                            mRow[iCol] = decFOB.ToString("0.00");
                                            mRow[iCol - 1] = decRMCB.ToString("0.00");
                                        }
                                        else
                                        {
                                            mRow[iCol] = decFOB.ToString("0.00");
                                            mRow[iCol - 1] = decRMCB.ToString("0.00");
                                        }
                                        //}
                                    }
                                }
                            }
                        }
                        iCol++;
                    }

                    DataView dtView = new DataView(dtAppCategory);
                    DataTable dtRdbPrice = dtView.ToTable(true, "Tyretype");
                    foreach (DataRow rRow in dtRdbPrice.Select("Tyretype='" + ddlBaseType.SelectedItem.Text + "'"))
                    {
                        rRow.Delete();
                    }
                    rdbPriceType.DataSource = dtRdbPrice;
                    rdbPriceType.DataTextField = "Tyretype";
                    rdbPriceType.DataValueField = "Tyretype";
                    rdbPriceType.DataBind();

                    gvPriceMatrix.DataSource = distinctList;
                    gvPriceMatrix.DataBind();
                    ViewState["dtPriceMatrix"] = distinctList;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JGvHide", "BuildGridViewHeader();", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JPreview", "gotoPreviewDiv('MainContent_gvPriceMatrix');", true);
                    btnSave_PriceMaster.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnIncreasePrice_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave_PriceMaster.Style.Add("display", "none");
                btnCalcSizeWiseEdit.Style.Add("display", "none");
                rdbEditSizeWise.SelectedIndex = -1;

                DataTable dtPriceMatrix = ViewState["dtPriceMatrix"] as DataTable;
                if (dtPriceMatrix.Rows.Count > 0)
                {
                    int colIndex = 0;
                    Button btn = (Button)sender;
                    if (btn.Text == "STD PRICE")
                    {
                        foreach (DataColumn dCol in dtPriceMatrix.Columns)
                        {
                            if (dCol.ColumnName != "TyreSize" && dCol.ColumnName != "RimSize" && dCol.ColumnName != "FinishedWt" && dCol.ColumnName.Contains("- FOB"))
                            {
                                foreach (DataRow dRow in dtPriceMatrix.Rows)
                                {
                                    if (dRow[dCol].ToString() != "")
                                    {
                                        decimal decFOB = Math.Round(Convert.ToDecimal(dRow[dCol].ToString()) + (Convert.ToDecimal(dRow[dCol].ToString()) *
                                            (Convert.ToDecimal(txtIncrementVal.Text) / 100)), 2);
                                        decimal decRMCB = Math.Round((((decFOB * Convert.ToDecimal(txtConvCur.Text)) / Convert.ToDecimal(dRow[2].ToString())) -
                                            Convert.ToDecimal(lblBaseCostVal.Text) - Convert.ToDecimal(lblClearanceInrKg.Text)), 2);
                                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                        {
                                            dRow[colIndex + 4] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                                Convert.ToDecimal(dRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                                            dRow[colIndex + 3] = decFOB.ToString("0.00");
                                            dRow[colIndex + 2] = decRMCB.ToString("0.00");

                                        }
                                        else
                                        {
                                            dRow[colIndex + 2] = decFOB.ToString("0.00");
                                            dRow[colIndex + 1] = decRMCB.ToString("0.00");
                                        }
                                    }
                                }
                            }
                            colIndex++;
                        }
                    }
                    else if (btn.Text == "NEW PRICE")
                    {
                        foreach (DataColumn dCol in dtPriceMatrix.Columns)
                        {
                            if (dCol.ColumnName != "TyreSize" && dCol.ColumnName != "RimSize" && dCol.ColumnName != "FinishedWt" && dCol.ColumnName.Contains("- NEW FOB"))
                            {
                                foreach (DataRow dRow in dtPriceMatrix.Rows)
                                {
                                    if (dRow[dCol].ToString() != "")
                                    {
                                        decimal decFOB = Math.Round(Convert.ToDecimal(dRow[dCol].ToString()) + (Convert.ToDecimal(dRow[dCol].ToString()) *
                                            (Convert.ToDecimal(txtIncrementVal.Text) / 100)), 2);
                                        decimal decRMCB = Math.Round((((decFOB * Convert.ToDecimal(txtConvCur.Text)) / Convert.ToDecimal(dRow[2].ToString())) -
                                            Convert.ToDecimal(lblBaseCostVal.Text) - Convert.ToDecimal(lblClearanceInrKg.Text)), 2);
                                        dRow[dCol] = decFOB.ToString("0.00");
                                        dRow[colIndex - 1] = decRMCB.ToString("0.00");
                                        if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                        {
                                            dRow[colIndex + 1] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                                Convert.ToDecimal(dRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                                        }
                                    }
                                }
                            }
                            colIndex++;
                        }
                    }

                    DataTable dtApprove = new DataTable();
                    dtApprove.Columns.Add("Config", typeof(string));
                    dtApprove.Columns.Add("Tyretype", typeof(string));
                    dtApprove.Columns.Add("brand", typeof(string));
                    dtApprove.Columns.Add("Sidewall", typeof(string));

                    foreach (GridViewRow gRow in gvTypeBrand.Rows)
                    {
                        CheckBox chk_SelectRows = gRow.FindControl("chk_SelectRows") as CheckBox;
                        if (chk_SelectRows.Checked)
                            dtApprove.Rows.Add(gRow.Cells[0].Text, gRow.Cells[1].Text, gRow.Cells[2].Text, gRow.Cells[3].Text);
                    }

                    DataView dtView = new DataView(dtApprove);
                    DataTable dtRdbPrice = dtView.ToTable(true, "Tyretype");
                    foreach (DataRow rRow in dtRdbPrice.Select("Tyretype='" + ddlBaseType.SelectedItem.Text + "'"))
                    {
                        rRow.Delete();
                    }
                    rdbPriceType.DataSource = dtRdbPrice;
                    rdbPriceType.DataTextField = "Tyretype";
                    rdbPriceType.DataValueField = "Tyretype";
                    rdbPriceType.DataBind();

                    gvPriceMatrix.DataSource = dtPriceMatrix;
                    gvPriceMatrix.DataBind();
                    ViewState["dtPriceMatrix"] = dtPriceMatrix;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JGvHide", "BuildGridViewHeader();", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JPreview", "gotoPreviewDiv('MainContent_gvPriceMatrix');", true);
                    btnSave_PriceMaster.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdbEditSizeWise_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnSave_PriceMaster.Style.Add("display", "none");
                btnCalcSizeWiseEdit.Style.Add("display", "none");
                rdbPriceType.DataSource = "";
                rdbPriceType.DataBind();
                gvPriceMatrix.DataSource = null;
                gvPriceMatrix.DataBind();

                DataView dtView = new DataView(ViewState["dtPriceMatrix"] as DataTable);
                DataTable dtEditMatrix = dtView.ToTable(true);
                if (dtEditMatrix.Rows.Count > 0)
                {
                    for (int dCol = dtEditMatrix.Columns.Count - 1; dCol > (!lblIncoterm.Text.ToLower().Contains("fob") ? 5 : 4); dCol--)
                    {
                        string[] strSplit = dtEditMatrix.Columns[dCol].ColumnName.Split('|');
                        if (ddlBaseType.SelectedItem.Text != strSplit[0].ToString() || strSplit[3].ToString().Contains("- NEW"))
                            dtEditMatrix.Columns.RemoveAt(dCol);
                    }

                    dtEditMatrix.Columns.Add("EDIT " + rdbEditSizeWise.SelectedItem.Text, typeof(string));
                    dtEditMatrix.Columns.Add("NEW " + (rdbEditSizeWise.SelectedItem.Text == "RMCB" ? "FOB" : "RMCB"), typeof(decimal));
                    dtEditMatrix.Columns.Add("NEW ARV", typeof(decimal));
                    if (!lblIncoterm.Text.ToLower().Contains("fob"))
                        dtEditMatrix.Columns.Add("NEW " + lblIncoterm.Text, typeof(decimal));

                    gvPriceMatrix.DataSource = dtEditMatrix;
                    gvPriceMatrix.DataBind();
                    hdnGvColCount.Value = dtEditMatrix.Columns.Count.ToString();

                    foreach (GridViewRow tRow in gvPriceMatrix.Rows)
                    {
                        TextBox txt = new TextBox();
                        txt.Width = 70;
                        txt.ID = "txtEdit";
                        txt.MaxLength = 10;
                        txt.CssClass = "form-control";
                        txt.Attributes.Add("onkeypress", "return isNumberKey(event)");
                        txt.Attributes.Add("onkeyup", "calcEditSizePrice(this)");
                        txt.Attributes.Add("onblur", "calcEditSizePrice(this)");
                        txt.Text = rdbEditSizeWise.SelectedItem.Text == "UNIT PRICE" ? dtEditMatrix.Rows[tRow.RowIndex][4].ToString() : dtEditMatrix.Rows[tRow.RowIndex][3].ToString();
                        tRow.Cells[dtEditMatrix.Columns.Count - (!lblIncoterm.Text.ToLower().Contains("fob") ? 4 : 3)].Controls.Add(txt);
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JGvHide", "BuildGridViewHeader();", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JPreview", "gotoPreviewDiv('MainContent_gvPriceMatrix');", true);
                    btnCalcSizeWiseEdit.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCalcSizeWiseEdit_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave_PriceMaster.Style.Add("display", "none");
                btnCalcSizeWiseEdit.Style.Add("display", "none");
                rdbPriceType.DataSource = "";
                rdbPriceType.DataBind();

                DataTable dtPriceMatrix = ViewState["dtPriceMatrix"] as DataTable;
                if (dtPriceMatrix.Rows.Count > 0)
                {
                    var myDetails = JsonConvert.DeserializeObject<List<EditSizeWise>>(hdnEditVal.Value);
                    foreach (EditSizeWise gRow in myDetails)
                    {
                        decimal _ctrlIncoterm = 0, _ctrlFWT = Convert.ToDecimal(gRow.Fwt);
                        string _ctrlRim = gRow.RimSize, _ctrlSize = gRow.TyreSize;
                        if (rdbEditSizeWise.SelectedItem.Text == "UNIT PRICE")
                        {
                            decimal _ctrlRMCB = Math.Round(((((Convert.ToDecimal(gRow.PriceVal) * Convert.ToDecimal(txtConvCur.Text)) / _ctrlFWT)) -
                                 Convert.ToDecimal(lblClearanceInrKg.Text) - Convert.ToDecimal(lblBaseCostVal.Text)), 2);
                            _ctrlIncoterm = Math.Round((gRow.PriceVal + (Convert.ToDecimal(lblFreightInrKg.Text) * _ctrlFWT / Convert.ToDecimal(txtConvCur.Text))), 2);

                            foreach (DataRow pRow in dtPriceMatrix.Select("TyreSize='" + _ctrlSize + "' and RimSize='" + _ctrlRim + "'"))
                            {
                                if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                {
                                    pRow[6] = _ctrlRMCB.ToString("0.00");
                                    pRow[7] = Convert.ToDecimal(gRow.PriceVal).ToString("0.00");
                                    pRow[8] = _ctrlIncoterm.ToString("0.00");
                                }
                                else
                                {
                                    pRow[5] = _ctrlRMCB.ToString("0.00");
                                    pRow[6] = Convert.ToDecimal(gRow.PriceVal).ToString("0.00");
                                }
                            }
                        }
                        else if (rdbEditSizeWise.SelectedItem.Text == "RMCB")
                        {
                            decimal _ctrlPRICE = Math.Round((((Convert.ToDecimal(gRow.PriceVal) + Convert.ToDecimal(lblBaseCostVal.Text) +
                                Convert.ToDecimal(lblClearanceInrKg.Text)) * _ctrlFWT) / Convert.ToDecimal(txtConvCur.Text)), 2);
                            _ctrlIncoterm = Math.Round((_ctrlPRICE + (Convert.ToDecimal(lblFreightInrKg.Text) * _ctrlFWT / Convert.ToDecimal(txtConvCur.Text))), 2);

                            foreach (DataRow pRow in dtPriceMatrix.Select("TyreSize='" + _ctrlSize + "' and RimSize='" + _ctrlRim + "'"))
                            {
                                if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                {
                                    pRow[6] = Convert.ToDecimal(gRow.PriceVal).ToString("0.00");
                                    pRow[7] = _ctrlPRICE.ToString("0.00");
                                    pRow[8] = _ctrlIncoterm.ToString("0.00");
                                }
                                else
                                {
                                    pRow[5] = Convert.ToDecimal(gRow.PriceVal).ToString("0.00");
                                    pRow[6] = _ctrlPRICE.ToString("0.00");
                                }
                            }
                        }
                    }
                    hdnGvColCount.Value = (Convert.ToInt32(hdnGvColCount.Value) - 1).ToString();

                    DataTable dtPremium = ViewState["dtPremium"] as DataTable;
                    if (dtPremium.Rows.Count > 0)
                    {
                        foreach (DataListItem dlItem in dlPremiumTypeList.Items)
                        {
                            if (((TextBox)dlItem.FindControl("txtPremiumVal")).Text != "")
                            {
                                foreach (DataRow pRow in dtPremium.Select("PremiumType='" + ((Label)dlItem.FindControl("lblPremiumType")).Text + "'"))
                                {
                                    pRow["PremiumValue"] = ((TextBox)dlItem.FindControl("txtPremiumVal")).Text;
                                }
                            }
                        }
                    }

                    for (int eCol = ((!lblIncoterm.Text.ToLower().Contains("fob")) ? 10 : 8); eCol < dtPriceMatrix.Columns.Count - 1; eCol++)
                    {
                        string[] strEditSplit = dtPriceMatrix.Columns[eCol].ColumnName.Split('|');
                        if (strEditSplit[strEditSplit.Length - 1].ToString() == "- NEW FOB" && strEditSplit[0].ToString() == ddlBaseType.SelectedItem.Text)
                        {
                            foreach (DataRow eRow in dtPriceMatrix.Rows)
                            {
                                if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                {
                                    eRow[eCol - 1] = eRow[6].ToString();
                                    eRow[eCol] = eRow[7].ToString();
                                    eRow[eCol + 1] = eRow[8].ToString();
                                }
                                else
                                {
                                    eRow[eCol - 1] = eRow[5].ToString();
                                    eRow[eCol] = eRow[6].ToString();
                                }
                            }
                        }
                        else if (strEditSplit[strEditSplit.Length - 1].ToString() == "- NEW FOB" && strEditSplit[0].ToString() != ddlBaseType.SelectedItem.Text)
                        {
                            decimal decPremiumVal = 0;
                            foreach (DataRow pRow in dtPremium.Select("PremiumType='" + strEditSplit[0].ToString() + "'"))
                            {
                                decPremiumVal = Convert.ToDecimal(pRow["PremiumValue"].ToString());
                            }
                            foreach (DataRow eRow in dtPriceMatrix.Rows)
                            {
                                if (eRow[eCol].ToString() != "")
                                {
                                    decimal decFOB = Math.Round(Convert.ToDecimal(eRow[((!lblIncoterm.Text.ToLower().Contains("fob")) ? 7 : 6)].ToString()) *
                                        (1 + (decPremiumVal / 100)), 2);
                                    decimal decRMCB = Math.Round((((decFOB * Convert.ToDecimal(txtConvCur.Text)) / Convert.ToDecimal(eRow[2].ToString())) -
                                        Convert.ToDecimal(lblClearanceInrKg.Text) - Convert.ToDecimal(lblBaseCostVal.Text)), 2);
                                    if (!lblIncoterm.Text.ToLower().Contains("fob"))
                                    {
                                        eRow[eCol + 1] = Math.Round((decFOB + (Convert.ToDecimal(lblFreightInrKg.Text) *
                                            Convert.ToDecimal(eRow[2].ToString()) / Convert.ToDecimal(txtConvCur.Text))), 2).ToString("0.00");
                                        eRow[eCol] = decFOB.ToString("0.00");
                                        eRow[eCol - 1] = decRMCB.ToString("0.00");
                                    }
                                    else
                                    {
                                        eRow[eCol] = decFOB.ToString("0.00");
                                        eRow[eCol - 1] = decRMCB.ToString("0.00");
                                    }
                                }
                            }
                        }
                    }

                    DataTable dtApprove = new DataTable();
                    dtApprove.Columns.Add("Config", typeof(string));
                    dtApprove.Columns.Add("Tyretype", typeof(string));
                    dtApprove.Columns.Add("brand", typeof(string));
                    dtApprove.Columns.Add("Sidewall", typeof(string));

                    foreach (GridViewRow gRow in gvTypeBrand.Rows)
                    {
                        CheckBox chk_SelectRows = gRow.FindControl("chk_SelectRows") as CheckBox;
                        if (chk_SelectRows.Checked)
                            dtApprove.Rows.Add(gRow.Cells[0].Text, gRow.Cells[1].Text, gRow.Cells[2].Text, gRow.Cells[3].Text);
                    }

                    DataView dtView = new DataView(dtApprove);
                    DataTable dtRdbPrice = dtView.ToTable(true, "Tyretype");
                    foreach (DataRow rRow in dtRdbPrice.Select("Tyretype='" + ddlBaseType.SelectedItem.Text + "'"))
                    {
                        rRow.Delete();
                    }
                    rdbPriceType.DataSource = dtRdbPrice;
                    rdbPriceType.DataTextField = "Tyretype";
                    rdbPriceType.DataValueField = "Tyretype";
                    rdbPriceType.DataBind();

                    gvPriceMatrix.DataSource = dtPriceMatrix;
                    gvPriceMatrix.DataBind();
                    ViewState["dtPriceMatrix"] = dtPriceMatrix;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JGvHide", "BuildGridViewHeader();", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JPreview", "gotoPreviewDiv('MainContent_gvPriceMatrix');", true);
                    btnSave_PriceMaster.Style.Add("display", "block");
                    rdbEditSizeWise.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnXlsDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtPriceMatrix = ViewState["dtPriceMatrix"] as DataTable;
                if (dtPriceMatrix.Rows.Count > 0)
                {
                    for (int delCol = dtPriceMatrix.Columns.Count - 1; delCol > 1; delCol--)
                    {
                        if (Utilities.Decrypt(Request["shid"].ToString()) == "master" && !dtPriceMatrix.Columns[delCol].ColumnName.Contains("- " + lblIncoterm.Text))
                            dtPriceMatrix.Columns.RemoveAt(delCol);
                        else if (Utilities.Decrypt(Request["shid"].ToString()) == "buffer" && !dtPriceMatrix.Columns[delCol].ColumnName.Contains("- NEW " + lblIncoterm.Text))
                            dtPriceMatrix.Columns.RemoveAt(delCol);
                        else
                            dtPriceMatrix.Columns[delCol].ColumnName = dtPriceMatrix.Columns[delCol].ColumnName.Replace("|", " ");
                    }

                    if (dtPriceMatrix.Columns.Count >= 2)
                    {
                        string strFileName = (ddlPriceSheetNo.Items.Count > 0 && ddlPriceSheetNo.SelectedItem.Text != "CHOOSE" &&
                                        ddlPriceSheetNo.SelectedItem.Text != txtNewPriceSheetNo.Text ? ddlPriceSheetNo.SelectedItem.Text : txtNewPriceSheetNo.Text);
                        string strDateTime = DateTime.Now.Year + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") +
                            DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
                        strFileName = System.Text.RegularExpressions.Regex.Replace(strFileName, @"(\s+|@|&|'|\(|\)|<|>|#)", "");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", strFileName + "-" + strDateTime));
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/x-msexcel";
                        using (System.IO.StringWriter sw = new System.IO.StringWriter())
                        {
                            HtmlTextWriter hw = new HtmlTextWriter(sw);
                            GridView gv = new GridView();
                            gv.DataSource = dtPriceMatrix;
                            gv.DataBind();
                            gv.RenderControl(hw);
                            Response.Write(sw.ToString());
                            Response.Flush();
                            Response.End();
                            gv = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //
        }
        protected void btnSave_PriceMaster_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtPriceMatrix = ViewState["dtPriceMatrix"] as DataTable;
                if (dtPriceMatrix.Rows.Count > 0)
                {
                    DataTable dtPremium = new DataTable();
                    dtPremium.Columns.Add("PremiumType", typeof(string));
                    dtPremium.Columns.Add("PremiumValue", typeof(decimal));
                    foreach (DataListItem dlItem in dlPremiumTypeList.Items)
                    {
                        if (((TextBox)dlItem.FindControl("txtPremiumVal")).Text != "")
                        {
                            dtPremium.Rows.Add(((Label)dlItem.FindControl("lblPremiumType")).Text, ((TextBox)dlItem.FindControl("txtPremiumVal")).Text);
                        }
                    }

                    DataTable dtApprove = new DataTable();
                    dtApprove.Columns.Add("Config", typeof(string));
                    dtApprove.Columns.Add("Tyretype", typeof(string));
                    dtApprove.Columns.Add("brand", typeof(string));
                    dtApprove.Columns.Add("Sidewall", typeof(string));

                    foreach (GridViewRow gRow in gvTypeBrand.Rows)
                    {
                        CheckBox chk_SelectRows = gRow.FindControl("chk_SelectRows") as CheckBox;
                        if (chk_SelectRows.Checked)
                            dtApprove.Rows.Add(gRow.Cells[0].Text, gRow.Cells[1].Text, gRow.Cells[2].Text, gRow.Cells[3].Text);
                    }

                    if (dtApprove.Rows.Count > 0)
                    {
                        SqlParameter[] spS = new SqlParameter[] { 
                        new SqlParameter("@SizeRimTypeBrandSidewall_DT", dtApprove), 
                        new SqlParameter("@Category", ddlCategory.SelectedItem.Text) 
                    };
                        DataTable dtApprovePID = (DataTable)daTTS.ExecuteReader_SP("sp_sel_cust_SizeRimTypeBrandSidewall", spS, DataAccess.Return_Type.DataTable);
                        if (dtApprovePID.Rows.Count > 0)
                        {
                            DataTable dtPrice = new DataTable();
                            dtPrice.Columns.Add("ProcessID", typeof(string));
                            dtPrice.Columns.Add("UnitPrice", typeof(decimal));
                            dtPrice.Columns.Add("Arv", typeof(decimal));
                            dtPrice.Columns.Add("Rmc", typeof(decimal));

                            int iCol = 0;
                            foreach (DataColumn dCol in dtPriceMatrix.Columns)
                            {
                                if (dCol.ColumnName != "TyreSize" && dCol.ColumnName != "RimSize" && dCol.ColumnName != "FinishedWt")
                                {
                                    string[] spHead = dCol.ColumnName.Split('|');
                                    if ((Utilities.Decrypt(Request["shid"].ToString()) == "master" && dCol.ColumnName.Contains("- FOB")) ||
                                        (Utilities.Decrypt(Request["shid"].ToString()) == "buffer" && dCol.ColumnName.Contains("- NEW FOB")))
                                    {
                                        foreach (DataRow gMRow in dtPriceMatrix.Rows)
                                        {
                                            foreach (DataRow mRow in dtApprovePID.Select("TyreSize='" + gMRow[0].ToString() + "' and RimSize='" +
                                                gMRow[1].ToString() + "' and TyreType='" + spHead[0].ToString() + "' and Brand='" + spHead[1].ToString() +
                                                "' and Sidewall='" + spHead[2].ToString() + "'"))
                                            {
                                                dtPrice.Rows.Add(mRow["ProcessID"].ToString(), gMRow[iCol].ToString(), "0.00", gMRow[iCol - 1].ToString());
                                            }
                                        }
                                    }
                                }
                                iCol++;
                            }

                            SqlParameter[] spI = new SqlParameter[] { 
                                new SqlParameter("@CustCode", ddlCustomer.SelectedItem.Value), 
                                new SqlParameter("@PriceSheetRefNo", txtNewPriceSheetNo.Text), 
                                new SqlParameter("@RatesID", ddlRatesID.SelectedItem.Value), 
                                new SqlParameter("@Category", ddlCategory.SelectedItem.Text), 
                                new SqlParameter("@SubCategory", ddlSubCategory.SelectedItem.Text),
                                new SqlParameter("@Config", ddlPlatform.SelectedItem.Text), 
                                new SqlParameter("@BaseType", ddlBaseType.SelectedItem.Text), 
                                new SqlParameter("@BaseRmcb", txtBaseRmcb.Text), 
                                new SqlParameter("@StartDate", DateTime.ParseExact(txtFromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                                new SqlParameter("@EndDate", DateTime.ParseExact(txtToDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                                new SqlParameter("@Username", Request.Cookies["TTSUser"].Value), 
                                new SqlParameter("@Price_Data_DT", dtPrice), 
                                new SqlParameter("@SheetType", Utilities.Decrypt(Request["shid"].ToString())), 
                                new SqlParameter("@CustCur", lblCurrency.Text), 
                                new SqlParameter("@CurRate", txtConvCur.Text), 
                                new SqlParameter("@RatesID_Cur", lblRatesID_Cur.Text), 
                                new SqlParameter("@Incoterm", lblIncoterm.Text), 
                                new SqlParameter("@DestinationPort", txtDestinationPort.Text), 
                                new SqlParameter("@FreightType", ddlFreightType.SelectedItem.Text), 
                                new SqlParameter("@FreightCost", txtFreightCost.Text),
                                new SqlParameter("@ClearanceCost", txtClearanceCost.Text),
                                new SqlParameter("@FreightWt", txtFreightWt.Text), 
                                new SqlParameter("@PricePremium_Data_DT", dtPremium), 
                                new SqlParameter("@ParentPriceID", ddlPriceSheetNo.Items.Count>0 && ddlPriceSheetNo.SelectedItem.Text != "CHOOSE" && 
                                    ddlPriceSheetNo.SelectedItem.Text != txtNewPriceSheetNo.Text ? ddlPriceSheetNo.SelectedItem.Value : "0" ) 
                            };
                            int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_PriceList_Master", spI);
                            if (resp > 0)
                            {
                                Response.Redirect(Request.RawUrl, false);
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
        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl, false);
        }
        public class EditSizeWise
        {
            public string TyreSize { get; set; }
            public string RimSize { get; set; }
            public decimal Fwt { get; set; }
            public decimal PriceVal { get; set; }
        }
    }
}