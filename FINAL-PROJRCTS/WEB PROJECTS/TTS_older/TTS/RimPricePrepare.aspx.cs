using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.Globalization;

namespace TTS
{
    public partial class RimPricePrepare : System.Web.UI.Page
    {
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_usermaster"].ToString() == "True")
                        {
                            DataTable dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_RimPriceSheet_Custname", DataAccess.Return_Type.DataTable);
                            if (dtCustList.Rows.Count > 0)
                            {
                                ddlCustomer.DataSource = dtCustList;
                                ddlCustomer.DataTextField = "custname";
                                ddlCustomer.DataValueField = "custcode";
                                ddlCustomer.DataBind();
                                if (dtCustList.Rows.Count == 1)
                                    ddlCustomer_SelectedIndexChanged(sender, e);
                                else
                                    ddlCustomer.Items.Insert(0, "CHOOSE");
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
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@custcode", ddlCustomer.SelectedItem.Value) };
                DataTable dtEdcList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PublishedRimPriceSheet_EdcNo", sp1, DataAccess.Return_Type.DataTable);
                if (dtEdcList.Rows.Count > 0)
                {
                    ddl_Edcno.DataSource = dtEdcList;
                    ddl_Edcno.DataTextField = "EDCNO";
                    ddl_Edcno.DataValueField = "EDCNO";
                    ddl_Edcno.DataBind();
                    if (dtEdcList.Rows.Count == 1)
                        ddl_Edcno_IndexChanged(sender, e);
                    else
                    {
                        ddl_Edcno.Items.Insert(0, "CHOOSE");
                        Bind_GvRimPriceData();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Edcno_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                Bind_GvRimPriceData();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_GvRimPriceData()
        {
            try
            {
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_ValDate.Text = "";
                gvRimPrice.DataSource = null;
                gvRimPrice.DataBind();

                string strEDC = ddl_Edcno.SelectedItem.Text != "CHOOSE" ? ddl_Edcno.SelectedItem.Text : "";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@custcode", ddlCustomer.SelectedItem.Value), new SqlParameter("@EdcNo", strEDC) };
                DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_RimPublishedPrice", sp, DataAccess.Return_Type.DataTable);
                if (dtData.Rows.Count > 0)
                {
                    if (strEDC != "")
                    {
                        txt_RimPrice.Text = dtData.Rows[0]["RimPrice"].ToString();
                        txt_RimWeight.Text = dtData.Rows[0]["RimWeight"].ToString();
                        txt_ValDate.Text = dtData.Rows[0]["EndDate"].ToString();
                    }
                    gvRimPrice.DataSource = dtData;
                    gvRimPrice.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveRimPriceDetails_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@custcode", ddlCustomer.SelectedItem.Value);
                sp1[1] = new SqlParameter("@EDCNO", ddl_Edcno.SelectedItem.Text);
                sp1[2] = new SqlParameter("@Rimprice", Convert.ToDecimal(txt_RimPrice.Text));
                sp1[3] = new SqlParameter("@Rimweight", txt_RimWeight.Text);
                sp1[4] = new SqlParameter("@EndDate", DateTime.ParseExact(txt_ValDate.Text, "dd/MM/YYYY", CultureInfo.InvariantCulture));
                sp1[5] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_update_PublishedRimPriceSheet", sp1);
                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EmptyAlert", "alert('SAVED SUCESSFULLY');", true);
                    btn_clear_Click(sender, e);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "EmptyAlert", "alert('DATA ALREADY EXIST');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_clear_Click(object sender, EventArgs e)
        {
            try
            {
                ddl_Edcno.SelectedIndex = 0;
                txt_RimPrice.Text = "";
                txt_RimWeight.Text = "";
                txt_ValDate.Text = "";
                Bind_GvRimPriceData();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}