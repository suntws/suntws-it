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

namespace TTS
{
    public partial class ratesmaster_v1 : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["ratesid_build"].ToString() == "True")
                        {
                            lblPageHead.Text = Utilities.Decrypt(Request["rmid"].ToString());

                            DataTable dtCur = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Currency_ForRatesID", DataAccess.Return_Type.DataTable);
                            ddlCurrency.DataSource = dtCur;
                            ddlCurrency.DataValueField = "CurrencyID";
                            ddlCurrency.DataTextField = "CurrencyName";
                            ddlCurrency.DataBind();

                            ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue("INR"));
                            lblCurrencyType.Text = "INR";

                            DataTable dtRID = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_RatesRefID", DataAccess.Return_Type.DataTable);
                            if (dtRID.Rows.Count > 0)
                            {
                                ddlRatesID.DataSource = dtRID;
                                ddlRatesID.DataTextField = "RatesRefID";
                                ddlRatesID.DataValueField = "RatesID";
                                ddlRatesID.DataBind();
                            }
                            ddlRatesID.Items.Insert(0, "CHOOSE");

                            DataTable dtType = (DataTable)daTTS.ExecuteReader_SP("sp_Sel_Type_Compound_CodeAndPercentage", DataAccess.Return_Type.DataTable);
                            if (dtType.Rows.Count > 0)
                            {
                                DataView dtView = new DataView(dtType);
                                DataTable dtTypeList = dtView.ToTable(true, "type");
                                dtTypeList.Columns.Add("typeval", typeof(decimal));
                                dlTypeList.DataSource = dtTypeList;
                                dlTypeList.DataBind();
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTxtFill", "$('#MainContent_dlTypeList').find('input:text').val('0');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }

                    if (Utilities.Decrypt(Request["rmid"].ToString()) == "disable")
                    {
                        btnSave.Text = "CLICK THE BUTTON TO DISABLE THE RATES-ID";
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JDisable", "$('input:text,#MainContent_ddlCurrency').attr('disabled',true);", true);
                    }
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
        protected void ddlRatesID_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtRatesID.Text = "";
                lblCurrencyType.Text = "";
                txtInrValue.Text = "";
                txtLoadFactor.Text = "";
                ddlCurrency.SelectedIndex = 0;

                if (ddlRatesID.SelectedIndex > 0)
                {
                    SqlParameter[] spS = new SqlParameter[] { new SqlParameter("@RatesID", ddlRatesID.SelectedItem.Value) };
                    DataSet dsT = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Exists_Rates_Data", spS, DataAccess.Return_Type.DataSet);
                    if (dsT.Tables.Count > 0)
                    {
                        if (dsT.Tables[0].Rows.Count > 0)
                        {
                            txtRatesID.Text = dsT.Tables[0].Rows[0]["RatesRefID"].ToString();
                            lblCurrencyType.Text = dsT.Tables[0].Rows[0]["Cur"].ToString();
                            txtInrValue.Text = dsT.Tables[0].Rows[0]["CurValue"].ToString();
                            txtLoadFactor.Text = dsT.Tables[0].Rows[0]["LoadFactor"].ToString();
                            ddlCurrency.SelectedIndex = ddlCurrency.Items.IndexOf(ddlCurrency.Items.FindByValue(lblCurrencyType.Text));
                        }
                        if (dsT.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataListItem iRow in dlTypeList.Items)
                            {
                                string strTypeVal = "0";
                                foreach (DataRow dRow in dsT.Tables[2].Select("type='" + ((Label)iRow.FindControl("lblTyreType")).Text + "'"))
                                {
                                    strTypeVal = dRow["TypeValue"].ToString();
                                }
                                ((TextBox)iRow.FindControl("txtTypeVal")).Text = strTypeVal;
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
        protected void ddlCurrency_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblCurrencyType.Text = "";
                txtInrValue.Text = "";
                if (ddlCurrency.SelectedIndex > 0)
                    lblCurrencyType.Text = ddlCurrency.SelectedItem.Value;

                ScriptManager.RegisterStartupScript(Page, GetType(), "JTxtFill", "$('#MainContent_dlTypeList').find('input:text').val('0');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCalcRmCost_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                if (Utilities.Decrypt(Request["rmid"].ToString()) == "disable")
                {
                    SqlParameter[] spU = new SqlParameter[] { 
                        new SqlParameter("@RatesID", ddlRatesID.SelectedItem.Value), 
                        new SqlParameter("@ModifiedBy", Request.Cookies["TTSUser"].Value) 
                    };
                    resp = daCOTS.ExecuteNonQuery_SP("sp_upd_RatesID_Master_status", spU);
                }
                else
                {
                    DataTable dtType = new DataTable();
                    dtType.Columns.Add("TyreType", typeof(string));
                    dtType.Columns.Add("TypeValue", typeof(decimal));
                    foreach (DataListItem dRow in dlTypeList.Items)
                    {
                        Label lblTyreType = (Label)dRow.FindControl("lblTyreType");
                        TextBox txtTypeVal = (TextBox)dRow.FindControl("txtTypeVal");
                        if (txtTypeVal.Text.Length > 0 && Convert.ToDecimal(txtTypeVal.Text) > 0)
                            dtType.Rows.Add(lblTyreType.Text, txtTypeVal.Text);
                    }

                    SqlParameter[] spI = new SqlParameter[] { 
                        new SqlParameter("@RatesRefID", txtRatesID.Text), 
                        new SqlParameter("@Cur", lblCurrencyType.Text), 
                        new SqlParameter("@CurValue", txtInrValue.Text), 
                        new SqlParameter("@LoadFactor", txtLoadFactor.Text), 
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value), 
                        new SqlParameter("@RatesCompund_DT", null), 
                        new SqlParameter("@RatesSize_DT", null), 
                        new SqlParameter("@RatesType_DT", dtType) 
                    };
                    resp = daCOTS.ExecuteNonQuery_SP("sp_ins_RatesID_Data", spI);
                }
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
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
    }
}