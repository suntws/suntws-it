using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.Globalization;

namespace TTS
{
    public partial class CotsPriceApproval : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["dom_usermaster"].ToString() == "True" || dtUser.Rows[0]["exp_usermaster"].ToString() == "True"))
                        {
                            if (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                                {
                                    ddl_Customer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("DOMESTIC", "DE0048"));
                                    ddl_Customer_SelectedIndexChanged(sender, e);
                                }
                                else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                                {
                                    DataTable dtCust = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_pricesheetapproval_customer", DataAccess.Return_Type.DataTable);
                                    if (dtCust.Rows.Count > 0)
                                    {
                                        ddl_Customer.DataSource = dtCust;
                                        ddl_Customer.DataTextField = "custfullname";
                                        ddl_Customer.DataValueField = "custcode";
                                        ddl_Customer.DataBind();
                                        if (dtCust.Rows.Count == 1)
                                            ddl_Customer_SelectedIndexChanged(sender, e);
                                        else
                                            ddl_Customer.Items.Insert(0, "CHOOSE");
                                    }
                                }
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
        protected void ddl_Customer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                gv_PriceDetails.DataSource = null;
                gv_PriceDetails.DataBind();
                ddl_PriceSheetRefNo.DataSource = "";
                ddl_PriceSheetRefNo.DataBind();
                txtRatesId.Text = "";
                txtEndDate.Text = "";

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Custcode", ddl_Customer.SelectedItem.Value) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_pricesheetapproval_refno", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddl_PriceSheetRefNo.DataSource = dt;
                    ddl_PriceSheetRefNo.DataTextField = "PriceSheet_RefNo";
                    ddl_PriceSheetRefNo.DataValueField = "ID";
                    ddl_PriceSheetRefNo.DataBind();
                    if (dt.Rows.Count == 1)
                        ddl_PriceSheetRefNo_SelectedIndexChanged(sender, e);
                    else
                        ddl_PriceSheetRefNo.Items.Insert(0, "CHOOSE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_PriceSheetRefNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                gv_PriceDetails.DataSource = null;
                gv_PriceDetails.DataBind();
                txtRatesId.Text = "";
                txtEndDate.Text = "";

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@ID", ddl_PriceSheetRefNo.SelectedItem.Value) };
                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_PriceSheetApproval", sp, DataAccess.Return_Type.DataSet);
                if (ds.Tables.Count == 2)
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        txtEndDate.Text = ds.Tables[0].Rows[0]["End_Date"].ToString();
                        txtRatesId.Text = ds.Tables[0].Rows[0]["Rates_ID"].ToString();
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        gv_PriceDetails.DataSource = ds.Tables[1];
                        gv_PriceDetails.DataBind();
                    }
                }
                else
                    lblErrMsg.Text = "No Records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSavePriceSheet_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                foreach (GridViewRow row in gv_PriceDetails.Rows)
                {
                    CheckBox chk_Select = (CheckBox)row.FindControl("chkSelect");
                    Label lblPlatform = (Label)row.FindControl("lblPlatform");
                    Label lblCategory = (Label)row.FindControl("lblCategory");
                    Label lblTyreSize = (Label)row.FindControl("lblTyreSize");
                    Label lblRimSize = (Label)row.FindControl("lblRimSize");
                    Label lblTyreType = (Label)row.FindControl("lblTyretype");
                    Label lblBrand = (Label)row.FindControl("lblBrand");
                    Label lblSidewall = (Label)row.FindControl("lblSidewall");
                    Label lblFinishedWt = (Label)row.FindControl("FinishedWt");
                    Label lblUnitPrice = (Label)row.FindControl("UnitPrice");
                    HiddenField hdnProcessId = (HiddenField)row.FindControl("hdnProcessId");
                    if (chk_Select.Checked)
                    {
                        SqlParameter[] sp = new SqlParameter[15];
                        sp[0] = new SqlParameter("@Category", lblCategory.Text);
                        sp[1] = new SqlParameter("@ProcessId", hdnProcessId.Value);
                        sp[2] = new SqlParameter("@Config", lblPlatform.Text);
                        sp[3] = new SqlParameter("@TyreSize", lblTyreSize.Text);
                        sp[4] = new SqlParameter("@RimSize", lblRimSize.Text);
                        sp[5] = new SqlParameter("@TyreType", lblTyreType.Text);
                        sp[6] = new SqlParameter("@Brand", lblBrand.Text);
                        sp[7] = new SqlParameter("@SideWall", lblSidewall.Text);
                        sp[8] = new SqlParameter("@FinishedWt", lblFinishedWt.Text);
                        sp[9] = new SqlParameter("@UnitPrice", lblUnitPrice.Text);
                        sp[10] = new SqlParameter("@PriceSheetRefNo", ddl_PriceSheetRefNo.SelectedItem.Text);
                        sp[11] = new SqlParameter("@RatedId", txtRatesId.Text);
                        sp[12] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                        sp[13] = new SqlParameter("@Custcode", ddl_Customer.SelectedItem.Value);
                        sp[14] = new SqlParameter("@P_ID", ddl_PriceSheetRefNo.SelectedItem.Value);

                        resp = daCOTS.ExecuteNonQuery_SP("sp_ins_publishedPriceSheet", sp);
                    }
                }
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}