using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;

namespace TTS
{
    public partial class RatesMaster : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
            {
                if (!IsPostBack)
                {
                    DataTable dtUser = Session["dtuserlevel"] as DataTable;
                    if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["ratesid_build"].ToString() == "True")
                    {
                        try
                        {
                            DataTable dt = new DataTable();
                            dt = Utilities.GetCurrecnyList(HttpContext.Current);

                            ddlCurrency.DataSource = dt;
                            ddlCurrency.DataValueField = "CurrencyName";
                            ddlCurrency.DataTextField = "CurrencyName";
                            ddlCurrency.DataBind();

                            ddlCurrency.Text = "INR India, Rupees";
                            txtCurRate.Text = "INR";

                            Bind_Lip_Details();
                            Bind_Base_Details();
                            Bind_Center_Details();
                            Bind_Tread_Details();

                            Bind_Solid_Size_Details();
                            Bind_Pob_Size_Details();
                            Bind_Pneumatic_Size_Details();

                            Bind_Type_Details();
                        }
                        catch (Exception ex)
                        {
                            Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                        }
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

        #region [Bind Details Loading Time]
        private void Bind_Lip_Details()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Get_ID_Details("lipgum");
                gv_Lip.DataSource = dt;
                gv_Lip.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Base_Details()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Get_ID_Details("base");
                gv_Base.DataSource = dt;
                gv_Base.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Center_Details()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Get_ID_Details("center");
                gv_Center.DataSource = dt;
                gv_Center.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Tread_Details()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Get_ID_Details("tread");
                gv_Tread.DataSource = dt;
                gv_Tread.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private DataTable Get_ID_Details(string strColumn)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@StrColumn", strColumn);
                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_IDList", sp1, DataAccess.Return_Type.DataTable);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }

        private void Bind_Solid_Size_Details()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Get_Size_Details("1");
                gv_SolidSize.DataSource = dt;
                gv_SolidSize.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Pob_Size_Details()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Get_Size_Details("2");
                gv_PobSize.DataSource = dt;
                gv_PobSize.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Pneumatic_Size_Details()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Get_Size_Details("3");
                gv_PneumaticSize.DataSource = dt;
                gv_PneumaticSize.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private DataTable Get_Size_Details(string strCategory)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@SizeCategory", strCategory);
                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_SolidSize", sp1, DataAccess.Return_Type.DataTable);

                if (strCategory == "Solid")
                {
                    DataView dtView = new DataView(dt);
                    dt = new DataTable();
                    dt = dtView.ToTable(true, "TyreSize");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }

        private void Bind_Type_Details()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_TypeOnly", DataAccess.Return_Type.DataTable);
                gv_TypeRates.DataSource = dt;
                gv_TypeRates.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        #endregion

        protected void btnCal_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptOpen", "showProgress();", true);
                DataTable dt = new DataTable();
                dt = (DataTable)daTTS.ExecuteReader_SP("sp_Sel_TypeMaster", DataAccess.Return_Type.DataTable);

                foreach (GridViewRow row in gv_TypeRates.Rows)
                {
                    decimal gvlipval = 0; decimal gvbaseval = 0; decimal gvcenterval = 0; decimal gvtreadval = 0;
                    decimal lipval = 0; decimal baseval = 0; decimal centerval = 0; decimal treadval = 0;
                    string strLip = ""; string strBase = ""; string strCenter = ""; string strTread = "";
                    Label lblType = row.FindControl("lblType") as Label;
                    Label lblTypeRatesVal = row.FindControl("lblTypeRatesVal") as Label;

                    try
                    {
                        //Typemaster Details
                        foreach (DataRow dtrow in dt.Select("type='" + lblType.Text + "'"))
                        {
                            strLip = dtrow["lipgum"].ToString() == "" ? "" : dtrow["lipgum"].ToString();
                            strBase = dtrow["base"].ToString() == "" ? "" : dtrow["base"].ToString();
                            strCenter = dtrow["center"].ToString() == "" ? "" : dtrow["center"].ToString();
                            strTread = dtrow["tread"].ToString() == "" ? "" : dtrow["tread"].ToString();

                            lipval = dtrow["lipgumper"].ToString() == "" ? 0 : Convert.ToDecimal(dtrow["lipgumper"].ToString());
                            baseval = dtrow["baseper"].ToString() == "" ? 0 : Convert.ToDecimal(dtrow["baseper"].ToString());
                            centerval = dtrow["centerper"].ToString() == "" ? 0 : Convert.ToDecimal(dtrow["centerper"].ToString());
                            treadval = dtrow["treadper"].ToString() == "" ? 0 : Convert.ToDecimal(dtrow["treadper"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteToErrorLog("TTS", "get details from typemaster", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                    }

                    //Lip Details
                    foreach (GridViewRow liprow in gv_Lip.Rows)
                    {
                        TextBox txtLipCost = liprow.FindControl("txtLipCost") as TextBox;
                        Label lblLip = liprow.FindControl("lblLip") as Label;
                        if (strLip == lblLip.Text)
                        {
                            gvlipval = Convert.ToDecimal(txtLipCost.Text) * lipval / 100;
                            break;
                        }
                    }

                    //Base Details
                    foreach (GridViewRow baserow in gv_Base.Rows)
                    {
                        TextBox txtBaseCost = baserow.FindControl("txtBaseCost") as TextBox;
                        Label lblBase = baserow.FindControl("lblBase") as Label;
                        if (strBase == lblBase.Text)
                        {
                            gvbaseval = Convert.ToDecimal(txtBaseCost.Text) * baseval / 100;
                            break;
                        }
                    }

                    //Center Details
                    foreach (GridViewRow centerrow in gv_Center.Rows)
                    {
                        TextBox txtCenterCost = centerrow.FindControl("txtCenterCost") as TextBox;
                        Label lblCenter = centerrow.FindControl("lblCenter") as Label;
                        if (strCenter == lblCenter.Text)
                        {
                            gvcenterval = Convert.ToDecimal(txtCenterCost.Text) * centerval / 100;
                            break;
                        }
                    }

                    //Tread Details
                    foreach (GridViewRow treadrow in gv_Tread.Rows)
                    {
                        TextBox txtTreadCost = treadrow.FindControl("txtTreadCost") as TextBox;
                        Label lblTread = treadrow.FindControl("lblTread") as Label;
                        if (strTread == lblTread.Text)
                        {
                            gvtreadval = Convert.ToDecimal(txtTreadCost.Text) * treadval / 100;
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(txtLoatFactor.Text))
                    {
                        lblTypeRatesVal.Text = Convert.ToDecimal((gvlipval + gvbaseval + gvcenterval + gvtreadval) + ((gvlipval + gvbaseval + gvcenterval + gvtreadval) * Convert.ToDecimal(txtLoatFactor.Text) / 100)).ToString("0.00");
                    }
                    else
                    {
                        lblTypeRatesVal.Text = Convert.ToDecimal(gvlipval + gvbaseval + gvcenterval + gvtreadval).ToString("0.00");
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptOpen", "showProgress();", true);
                //Check this rates-id
                DataTable dt = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@RatesID", txtRatesID.Text);
                sp1[1] = new SqlParameter("@Cur", txtCurRate.Text);
                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Chk_RatesID", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    lblErrMsg.Text = "Rates-ID Already Exists: Enter new Rates_ID / Click edit button";
                }
                else
                {
                    btnCal_Click(sender, e);
                    Insert_Rates_Currency();
                    Insert_Rates_LoadFact_ConvCost();
                    Ins_Edit_Loop_Values();//Size & ID Values
                    Insert_Edit_Rates_TypeVal();

                    Response.Redirect("default.aspx", false);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Ins_Edit_Loop_Values()
        {
            //Insert ID Value
            try
            {
                DataTable dtIDValue = new DataTable();
                DataColumn dColumn = new DataColumn("ID", typeof(System.String));
                dtIDValue.Columns.Add(dColumn);
                dColumn = new DataColumn("IDValue", typeof(System.Decimal));
                dtIDValue.Columns.Add(dColumn);
                dColumn = new DataColumn("RatesID", typeof(System.String));
                dtIDValue.Columns.Add(dColumn);

                foreach (GridViewRow liprow in gv_Lip.Rows)
                {
                    TextBox txtLipCost = liprow.FindControl("txtLipCost") as TextBox;
                    Label lblLip = liprow.FindControl("lblLip") as Label;
                    dtIDValue.Rows.Add(lblLip.Text, Convert.ToDecimal(txtLipCost.Text).ToString("0.00"), txtRatesID.Text);
                }

                foreach (GridViewRow baserow in gv_Base.Rows)
                {
                    TextBox txtBaseCost = baserow.FindControl("txtBaseCost") as TextBox;
                    Label lblBase = baserow.FindControl("lblBase") as Label;
                    dtIDValue.Rows.Add(lblBase.Text, Convert.ToDecimal(txtBaseCost.Text).ToString("0.00"), txtRatesID.Text);
                }

                foreach (GridViewRow centerrow in gv_Center.Rows)
                {
                    TextBox txtCenterCost = centerrow.FindControl("txtCenterCost") as TextBox;
                    Label lblCenter = centerrow.FindControl("lblCenter") as Label;
                    dtIDValue.Rows.Add(lblCenter.Text, Convert.ToDecimal(txtCenterCost.Text).ToString("0.00"), txtRatesID.Text);
                }

                foreach (GridViewRow treadrow in gv_Tread.Rows)
                {
                    TextBox txtTreadCost = treadrow.FindControl("txtTreadCost") as TextBox;
                    Label lblTread = treadrow.FindControl("lblTread") as Label;
                    dtIDValue.Rows.Add(lblTread.Text, Convert.ToDecimal(txtTreadCost.Text).ToString("0.00"), txtRatesID.Text);
                }

                if (dtIDValue.Rows.Count > 0)
                {
                    Insert_Rates_IDValue_TableWise(dtIDValue);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "ID Value Insert", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

            //Insert Size Value
            try
            {
                DataTable dtSizeValue = new DataTable();
                DataColumn dColumn = new DataColumn("TyreSize", typeof(System.String));
                dtSizeValue.Columns.Add(dColumn);
                dColumn = new DataColumn("SizeVal", typeof(System.Decimal));
                dtSizeValue.Columns.Add(dColumn);
                dColumn = new DataColumn("RatesID", typeof(System.String));
                dtSizeValue.Columns.Add(dColumn);

                foreach (GridViewRow solidrow in gv_SolidSize.Rows)
                {
                    TextBox txtSolidSizeValue = solidrow.FindControl("txtSolidSizeValue") as TextBox;
                    Label lblSolidSize = solidrow.FindControl("lblSolidSize") as Label;
                    dtSizeValue.Rows.Add(lblSolidSize.Text, Convert.ToDecimal(txtSolidSizeValue.Text), txtRatesID.Text);
                }

                foreach (GridViewRow pobrow in gv_PobSize.Rows)
                {
                    TextBox txtPobSizeValue = pobrow.FindControl("txtPobSizeValue") as TextBox;
                    Label lblPobSize = pobrow.FindControl("lblPobSize") as Label;
                    dtSizeValue.Rows.Add(lblPobSize.Text, Convert.ToDecimal(txtPobSizeValue.Text), txtRatesID.Text);
                }

                foreach (GridViewRow pneumaticrow in gv_PneumaticSize.Rows)
                {
                    TextBox txtPneumaticSizeValue = pneumaticrow.FindControl("txtPneumaticSizeValue") as TextBox;
                    Label lblPneumaticSize = pneumaticrow.FindControl("lblPneumaticSize") as Label;
                    dtSizeValue.Rows.Add(lblPneumaticSize.Text, Convert.ToDecimal(txtPneumaticSizeValue.Text), txtRatesID.Text);
                }

                if (dtSizeValue.Rows.Count > 0)
                    Insert_Edit_Rates_SizeVal_TableWise(dtSizeValue);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "Size Value Insert", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Insert_Rates_Currency()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@Cur", txtCurRate.Text);
                sp1[1] = new SqlParameter("@CurValue", Convert.ToDecimal(txtIndRate.Text));
                sp1[2] = new SqlParameter("@RatesID", txtRatesID.Text);
                sp1[3] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                daTTS.ExecuteNonQuery_SP("Sp_Ins_RatesCur", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Insert_Rates_LoadFact_ConvCost()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@ConCost", Convert.ToDecimal(txtConvCost.Text));
                sp1[1] = new SqlParameter("@LoadFact", Convert.ToDecimal(txtLoatFactor.Text));
                sp1[2] = new SqlParameter("@RatesID", txtRatesID.Text);
                sp1[3] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                daTTS.ExecuteNonQuery_SP("Sp_Ins_RatesLoadFact", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Insert_Edit_Rates_TypeVal()
        {
            try
            {
                DataTable dtTypeCostMap = new DataTable();
                DataColumn dColumn = new DataColumn("TyreType", typeof(System.String));
                dtTypeCostMap.Columns.Add(dColumn);
                dColumn = new DataColumn("Typecost", typeof(System.Decimal));
                dtTypeCostMap.Columns.Add(dColumn);
                dColumn = new DataColumn("RatesID", typeof(System.String));
                dtTypeCostMap.Columns.Add(dColumn);

                foreach (GridViewRow row in gv_TypeRates.Rows)
                {
                    Label lblType = row.FindControl("lblType") as Label;
                    Label lblTypeRatesVal = row.FindControl("lblTypeRatesVal") as Label;

                    dtTypeCostMap.Rows.Add(lblType.Text, lblTypeRatesVal.Text, txtRatesID.Text);
                }
                if (dtTypeCostMap.Rows.Count > 0)
                    Insert_Edit_Rates_TypeCost_TableWise(dtTypeCostMap);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Insert_Rates_IDValue_TableWise(DataTable dtMap)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@idvalue_datatable", dtMap);
                sp1[1] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);

                daTTS.ExecuteNonQuery_SP("Sp_RaetsIDValue_Merge", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Insert_Edit_Rates_TypeCost_TableWise(DataTable dtCostMap)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@typecost_datatable", dtCostMap);
                sp1[1] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);

                daTTS.ExecuteNonQuery_SP("Sp_RaetsTypeCost_Merge", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Insert_Edit_Rates_SizeVal_TableWise(DataTable dtSizeValMap)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@sizeamt_datatable", dtSizeValMap);
                sp1[1] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);

                daTTS.ExecuteNonQuery_SP("Sp_RaetsSizeAmt_Merge", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptOpen", "showProgress();", true);
                //Check this rates-id
                DataTable dt = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@RatesID", txtRatesID.Text);
                sp1[1] = new SqlParameter("@Cur", txtCurRate.Text);
                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Chk_RatesID", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count == 0)
                {
                    lblErrMsg.Text = "Rates-ID not exists: Do u want save this details plz Click add button";
                }
                else
                {
                    btnCal_Click(sender, e);
                    Edit_Rates_Currency();
                    Edit_Rates_LoadFact_ConvCost();
                    Ins_Edit_Loop_Values();//Size & ID Values
                    Insert_Edit_Rates_TypeVal();

                    Response.Redirect("default.aspx", false);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript", "hideProgress();", true);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Edit_Rates_Currency()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@Cur", txtCurRate.Text);
                sp1[1] = new SqlParameter("@CurValue", Convert.ToDecimal(txtIndRate.Text));
                sp1[2] = new SqlParameter("@RatesID", txtRatesID.Text);
                sp1[3] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                daTTS.ExecuteNonQuery_SP("Sp_Edit_RatesCur", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Edit_Rates_LoadFact_ConvCost()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@ConCost", Convert.ToDecimal(txtConvCost.Text));
                sp1[1] = new SqlParameter("@LoadFact", Convert.ToDecimal(txtLoatFactor.Text));
                sp1[2] = new SqlParameter("@RatesID", txtRatesID.Text);
                sp1[3] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                daTTS.ExecuteNonQuery_SP("Sp_Edit_RatesLoadFact", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        #region [Web Method Cost Details]
        [WebMethod]

        public static string GetCost_WebMethod(string ratesid, string strCol)
        {
            System.Threading.Thread.Sleep(200);
            return Get_ID_List_WebMethod(strCol, ratesid.ToString()).GetXml();
        }

        private static DataSet Get_ID_List_WebMethod(string strCol, string ratesid)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@StrColumn", strCol);
                sp1[1] = new SqlParameter("@RatesID", ratesid);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Sel_IDList_WebMethod", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "RatesMaster.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[WebMethod Size Deatils]
        [WebMethod]

        public static string GetSizeBeadBand_WebMethod(string ratesid, string sizeCategory)
        {
            System.Threading.Thread.Sleep(200);
            return Get_Size_List_WebMethod(sizeCategory, ratesid.ToString()).GetXml();
        }

        private static DataSet Get_Size_List_WebMethod(string sizeCategory, string ratesid)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@SizeCategory", sizeCategory);
                sp1[1] = new SqlParameter("@RatesID", ratesid);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Sel_Rates_SizeList_WebMethod", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "RatesMaster.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[WebMethod Type Deatils]
        [WebMethod]

        public static string GetType_List_WebMethod(string ratesid)
        {
            System.Threading.Thread.Sleep(200);
            return Get_Type_List_WebMethod(ratesid.ToString()).GetXml();
        }

        private static DataSet Get_Type_List_WebMethod(string ratesid)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@RatesID", ratesid);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Sel_RatesTypeVal", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "RatesMaster.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[WebMethod CurCost]
        [WebMethod]
        public static string GetCur_Cost_WebMethod(string ratesid)
        {
            System.Threading.Thread.Sleep(200);
            return Get_CurCost_List_WebMethod(ratesid.ToString()).GetXml();
        }

        private static DataSet Get_CurCost_List_WebMethod(string ratesid)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@RatesID", ratesid);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Sel_Rates_CurCost", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "RatesMaster.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion

        #region[WebMethod GetCurrency_ChangeValue]
        [WebMethod]
        public static string GetCurrency_ChangeValue_WebMethod(string ratesid, string curtype)
        {
            return GetCurrency_ChangeValue(ratesid.ToString(), curtype.ToString()).GetXml();
        }

        private static DataSet GetCurrency_ChangeValue(string ratesid, string curtype)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@RatesID", ratesid);
                sp1[1] = new SqlParameter("@Cur", curtype);
                ds = (DataSet)daTTSWebMethod.ExecuteReader_SP("Sp_Get_Rates_CurValue_CurWise", sp1, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "RatesMaster.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion
    }
}