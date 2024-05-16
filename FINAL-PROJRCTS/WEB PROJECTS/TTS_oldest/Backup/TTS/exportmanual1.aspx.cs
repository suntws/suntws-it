using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Globalization;

namespace TTS
{
    public partial class exportmanual1 : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_orderentry"].ToString() == "True")
                        {
                            DataTable dtCustList = new DataTable();
                            dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_export_fullname", DataAccess.Return_Type.DataTable);
                            Utilities.ddl_Binding(ddlCotsCustName, dtCustList, "custfullname", "custcode", "Choose");
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
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCotsCustName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCotsCustName")
                {
                    ddlLoginUserName.DataSource = "";
                    ddlLoginUserName.DataBind();
                    ddlBillingAddress.DataSource = "";
                    ddlBillingAddress.DataBind();
                    ddlShippingAddress.DataSource = "";
                    ddlShippingAddress.DataBind();
                    lblBillAddress.Text = "";
                    lblShipDetails.Text = "";
                    lblErrMsg.Text = "";
                    Utilities.selectedListItem_Find(ddlCotsCustName, hdnFullName.Value, "TEXT");
                    if (ddlCotsCustName.SelectedItem.Text != "Choose")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddlCotsCustName.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddlCotsCustName.SelectedItem.Value) 
                    };
                        DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                        Utilities.ddl_Binding(ddlLoginUserName, dtUserList, "username", "ID", "Choose");
                        if (ddlLoginUserName.Items.Count == 2)
                        {
                            ddlLoginUserName.SelectedIndex = 1;
                            ddlLoginUserName_IndexChange(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlLoginUserName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                Utilities.selectedListItem_Find(ddlCotsCustName, hdnFullName.Value, "TEXT");
                ddlBillingAddress.DataSource = "";
                ddlBillingAddress.DataBind();
                ddlShippingAddress.DataSource = "";
                ddlShippingAddress.DataBind();
                lblBillAddress.Text = "";
                lblShipDetails.Text = "";
                lblErrMsg.Text = "";
                hdnLoginName.Value = ddlLoginUserName.SelectedItem.Text;
                hdnCotsCustID.Value = ddlLoginUserName.SelectedItem.Value;
                if (ddlLoginUserName.SelectedItem.Text != "Choose")
                {
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@custcode", ddlLoginUserName.SelectedItem.Value);
                    DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList", sp1, DataAccess.Return_Type.DataTable);
                    Utilities.ddl_Binding(ddlBillingAddress, dtBillAddress, "ShipAddress", "shipid", "Choose");
                    if (dtBillAddress.Rows.Count > 0)
                    {
                        ddlBillingAddress.Enabled = true;
                        ddlShippingAddress.Enabled = true;
                    }
                    else
                    {
                        ddlBillingAddress.Enabled = false;
                        ddlShippingAddress.Enabled = false;
                        lblErrMsg.Text = "BILLING ADDRESS NOT ENTERED IN MASTER";
                    }
                    if (ddlBillingAddress.Items.Count == 2)
                    {
                        ddlBillingAddress.SelectedIndex = 1;
                        ddlBillingAddress_IndexChange(sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlBillingAddress_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblBillAddress.Text = "";
                lblShipDetails.Text = "";
                lblErrMsg.Text = "";
                Utilities.selectedListItem_Find(ddlCotsCustName, hdnFullName.Value, "TEXT");
                ddlShippingAddress.DataSource = "";
                ddlShippingAddress.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@BillID", Convert.ToInt32(ddlBillingAddress.SelectedItem.Value)) };
                hdnBillID.Value = ddlBillingAddress.SelectedItem.Value;
                lblBillAddress.Text = Bind_Address(ddlBillingAddress.SelectedItem.Value);
                DataTable dtShippAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ShipList_BillID", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlShippingAddress, dtShippAddress, "ShipAddress", "shipid", "Choose");
                if (ddlShippingAddress.Items.Count == 2)
                {
                    ddlShippingAddress.SelectedIndex = 1;
                    ddlShippingAddress_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                sp2[1] = new SqlParameter("@orderrefno", txtOrderRefNo.Text);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_orderrefno", sp2, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0 && dt.Rows[0]["OrderRefNo"].ToString() == txtOrderRefNo.Text && dt.Rows[0]["OrderRefNo"].ToString() != "")
                    lblErrMsg.Text = "ORDER NO ALREADY EXISTING";
                else
                {
                    SqlParameter[] sp1 = new SqlParameter[9];
                    sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                    sp1[1] = new SqlParameter("@OrderRefNo", txtOrderRefNo.Text.Trim());
                    sp1[2] = new SqlParameter("@ShipID", ddlShippingAddress.SelectedItem.Value);
                    sp1[3] = new SqlParameter("@BillID", ddlBillingAddress.SelectedItem.Value);
                    sp1[4] = new SqlParameter("@SplIns", txtSplIns.Text.Replace("\r\n", "~"));
                    sp1[5] = new SqlParameter("@DesiredShipDate", DateTime.ParseExact(txtDesiredDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    sp1[6] = new SqlParameter("@PriceRefNo", "Manual Order Prepared By: " + Request.Cookies["TTSUser"].Value);
                    sp1[7] = new SqlParameter("@SpecialRequset", txtSplReq.Text.Replace("\r\n", "~"));
                    sp1[8] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);

                    object resp = (object)daCOTS.ExecuteScalar_SP("sp_ins_OrderMasterDetails_ExpManual", sp1);
                    if (Convert.ToInt32(resp.ToString()) > 0)
                        Response.Redirect("cots_manualorderprepare_01.aspx?oid=" + Utilities.Encrypt(resp.ToString()) + "&fid=" + Utilities.Encrypt("e"), false);
                    else
                        lblErrMsg.Text = "ORDER NOT SAVED. CONTACT ADMINISTRATOR.";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private string Bind_Address(string BillID)
        {
            try
            {
                string strAddress = "";
                DataTable dtAddressList = DomesticScots.Bind_BillingAddress(BillID);
                if (dtAddressList.Rows.Count > 0)
                {
                    DataRow row = dtAddressList.Rows[0];
                    strAddress += "<div style='font-weight:bold;'>M/S. " + row["CompanyName"].ToString() + "</div>";
                    strAddress += "<div>" + row["shipaddress"].ToString().Replace("~", "<br/>") + "</div>";
                    strAddress += "<div>" + row["city"].ToString() + ", " + row["statename"].ToString() + "</div>";
                    strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
                    strAddress += "<div>" + row["contact_name"].ToString() + "</div>";
                    strAddress += "<div>" + row["EmailID"].ToString() + " / " + row["mobile"].ToString() + "</div>";
                }
                return strAddress;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        protected void ddlShippingAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblShipDetails.Text = "";
                if (ddlShippingAddress.SelectedItem.Text != "Choose")
                    lblShipDetails.Text = Bind_Address(ddlShippingAddress.SelectedItem.Value);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}