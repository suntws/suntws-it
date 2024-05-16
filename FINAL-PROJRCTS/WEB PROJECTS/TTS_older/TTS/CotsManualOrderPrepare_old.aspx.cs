using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Text;
using System.Xml;
using System.Web.Services;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TTS
{
    public partial class CotsManualOrderPrepare : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_orderentry"].ToString() == "True")
                        {
                            string strSheetExpire = (string)daCOTS.ExecuteScalar_SP("sp_chk_pricesheet_expireddate");
                            if (strSheetExpire == "")
                            {
                                DataTable dtCustList = new DataTable();
                                if (Request.Cookies["TTSUserType"].Value.ToLower() == "admin" || Request.Cookies["TTSUserType"].Value.ToLower() == "support")
                                    dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsDomesticCustomer", DataAccess.Return_Type.DataTable);
                                else
                                {
                                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@LeadWise", Request.Cookies["TTSUser"].Value) };
                                    dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsDomesticCustomer_LeadWise", sp, DataAccess.Return_Type.DataTable);
                                }
                                if (dtCustList.Rows.Count > 0)
                                {
                                    ddl_CustomerName.DataSource = dtCustList;
                                    ddl_CustomerName.DataTextField = "custfullname";
                                    ddl_CustomerName.DataValueField = "custcode";
                                    ddl_CustomerName.DataBind();
                                    ddl_CustomerName.Items.Insert(0, "CHOOSE");
                                }
                                Bind_Radiobuttons();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = strSheetExpire;
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
                if (hdnCustomerName.Value != "")
                    ddl_CustomerName.SelectedIndex = ddl_CustomerName.Items.IndexOf(ddl_CustomerName.Items.FindByText(hdnCustomerName.Value));
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Radiobuttons()
        {
            try
            {
                string strXmlShipList = HttpContext.Current.Server.MapPath(
                    ConfigurationManager.AppSettings.Get("xmlFolder") + @"shippingmethod.xml").Replace("TTS", "" + ConfigurationManager.AppSettings.Get("SCOTS") + "");
                XmlDocument xmlShipList = new XmlDocument();
                xmlShipList.Load(strXmlShipList);
                if (xmlShipList != null)
                {
                    var dict = new Dictionary<string, string>();
                    DataTable dtShipList = new DataTable();
                    dtShipList.Columns.Add("Item", typeof(string));

                    //packing loaded
                    dict.Clear();
                    dtShipList.Clear();
                    foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/packing"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtShipList.Rows.Add(item.Value);
                        }
                        rdo_PackingMethod.DataSource = dtShipList;
                        rdo_PackingMethod.DataTextField = "item";
                        rdo_PackingMethod.DataValueField = "item";
                        rdo_PackingMethod.DataBind();
                    }

                    //Freight Charges
                    dict.Clear();
                    dtShipList.Clear();
                    foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/freight"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtShipList.Rows.Add(item.Value);
                        }
                        rdo_FreightCharges.DataSource = dtShipList;
                        rdo_FreightCharges.DataTextField = "item";
                        rdo_FreightCharges.DataValueField = "item";
                        rdo_FreightCharges.DataBind();
                    }

                    //delivery method
                    dict.Clear();
                    dtShipList.Clear();
                    foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/delivery"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtShipList.Rows.Add(item.Value);
                        }
                        rdo_DeliveryMethod.DataSource = dtShipList;
                        rdo_DeliveryMethod.DataTextField = "item";
                        rdo_DeliveryMethod.DataValueField = "item";
                        rdo_DeliveryMethod.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_CustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddl_CustomerName")
                {
                    ddl_UserId.DataSource = "";
                    ddl_UserId.DataBind();
                    ddl_BillingAddress.DataSource = "";
                    ddl_BillingAddress.DataBind();
                    ddl_ShippingAddress.DataSource = "";
                    ddl_ShippingAddress.DataBind();
                    lbl_SelectedBillAddress.Text = "";
                    lbl_SelectedShipAddress.Text = "";
                    lbl_BillAddressErr.Text = "";
                    lbl_ShipAddressErr.Text = "";
                    if (ddl_CustomerName.SelectedItem.Text != "CHOOSE")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@custfullname", ddl_CustomerName.SelectedItem.Text), new SqlParameter("@stdcustcode", ddl_CustomerName.SelectedItem.Value) };
                        DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                        if (dtUserList.Rows.Count > 0)
                        {
                            ddl_UserId.DataSource = dtUserList;
                            ddl_UserId.DataTextField = "username";
                            ddl_UserId.DataValueField = "ID";
                            ddl_UserId.DataBind();
                            if (dtUserList.Rows.Count == 1)
                                ddl_UserId_SelectedIndexChanged(sender, e);
                            else
                                ddl_UserId.Items.Insert(0, "CHOOSE");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_UserId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_BillingAddress.DataSource = "";
                ddl_BillingAddress.DataBind();
                ddl_ShippingAddress.DataSource = "";
                ddl_ShippingAddress.DataBind();
                lbl_SelectedBillAddress.Text = "";
                lbl_SelectedShipAddress.Text = "";
                lbl_BillAddressErr.Text = "";
                lbl_ShipAddressErr.Text = "";
                if (ddl_UserId.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@custcode", ddl_UserId.SelectedItem.Value) };

                    DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList", sp1, DataAccess.Return_Type.DataTable);
                    if (dtBillAddress.Rows.Count > 0)
                    {
                        ddl_BillingAddress.DataSource = dtBillAddress;
                        ddl_BillingAddress.DataTextField = "ShipAddress";
                        ddl_BillingAddress.DataValueField = "shipid";
                        ddl_BillingAddress.DataBind();
                        if (dtBillAddress.Rows.Count == 1)
                            ddl_BillingAddress_SelectedIndexChanged(sender, e);
                        else
                            ddl_BillingAddress.Items.Insert(0, "CHOOSE");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_BillingAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_ShippingAddress.DataSource = "";
                ddl_ShippingAddress.DataBind();
                lbl_SelectedBillAddress.Text = "";
                lbl_SelectedShipAddress.Text = "";
                lbl_BillAddressErr.Text = "";
                lbl_ShipAddressErr.Text = "";
                if (ddl_BillingAddress.SelectedItem.Text != "CHOOSE")
                {
                    lbl_SelectedBillAddress.Text = Bind_Address(ddl_BillingAddress.SelectedItem.Value, lbl_BillAddressErr);
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@BillID", Convert.ToInt32(ddl_BillingAddress.SelectedItem.Value)) };
                    DataTable dtShippAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ShipList_BillID", sp1, DataAccess.Return_Type.DataTable);
                    if (dtShippAddress.Rows.Count > 0)
                    {
                        ddl_ShippingAddress.DataSource = dtShippAddress;
                        ddl_ShippingAddress.DataTextField = "ShipAddress";
                        ddl_ShippingAddress.DataValueField = "shipid";
                        ddl_ShippingAddress.DataBind();
                        if (dtShippAddress.Rows.Count == 1)
                            ddl_ShippingAddress_SelectedIndexChanged(sender, e);
                        else
                            ddl_ShippingAddress.Items.Insert(0, "Choose");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_ShippingAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lbl_SelectedShipAddress.Text = "";
                lbl_ShipAddressErr.Text = "";
                if (ddl_ShippingAddress.SelectedItem.Text != "CHOOSE")
                {
                    lbl_SelectedShipAddress.Text = Bind_Address(ddl_ShippingAddress.SelectedItem.Value, lbl_ShipAddressErr);
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custcode", ddl_UserId.SelectedItem.Value), 
                        new SqlParameter("@ShipID", Convert.ToInt32(ddl_ShippingAddress.SelectedItem.Value)) 
                    };
                    DataTable dtPer = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_CotsManualOrder_GST", sp1, DataAccess.Return_Type.DataTable);
                    {
                        txtCGST.Text = dtPer.Rows[0]["CGST"].ToString().Trim() == "" ? "0.00" : dtPer.Rows[0]["CGST"].ToString().Trim();
                        txtSGST.Text = dtPer.Rows[0]["SGST"].ToString().Trim() == "" ? "0.00" : dtPer.Rows[0]["SGST"].ToString().Trim();
                        txtIGST.Text = dtPer.Rows[0]["IGST"].ToString().Trim() == "" ? "0.00" : dtPer.Rows[0]["IGST"].ToString().Trim();
                    }
                    txt_OrderRefNo.Focus();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private string Bind_Address(string AddressID, Label lblErr)
        {
            try
            {
                string strAddress = "";
                DataTable dtAddressList = DomesticScots.Bind_BillingAddress(AddressID);
                if (dtAddressList.Rows.Count > 0)
                {
                    DataRow row = dtAddressList.Rows[0];
                    strAddress += "<div style='font-weight:bold;'>M/S. " + row["CompanyName"].ToString() + "</div>";
                    strAddress += "<div>" + row["shipaddress"].ToString().Replace("~", "<br/>") + "</div>";
                    strAddress += "<div>" + row["city"].ToString() + ", " + row["statename"].ToString() + "</div>";
                    strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
                    strAddress += "<div>" + row["contact_name"].ToString() + "</div>";
                    strAddress += "<div>" + row["EmailID"].ToString() + "</div>";
                    strAddress += "<div>" + row["mobile"].ToString() + " / " + row["PhoneNo"].ToString() + "</div>";
                    strAddress += "<div>GST: " + row["GST_No"].ToString() + "</div>";
                    hdnMajorDataVerified.Value = row["MajorDataVerified"].ToString();

                    if (!Convert.ToBoolean(row["MajorDataVerified"].ToString()))
                    {
                        string strAddErr = "";
                        if (row["zipcode"].ToString().Trim().Length != 6)
                            strAddErr += "<div>PINCODE IS NOT VALID.</div>";
                        if (row["mobile"].ToString().Trim().Length < 10 || row["mobile"].ToString().Trim().Length > 12)
                            strAddErr += "<div>MOBILE NO IS NOT VALID.</div>";
                        if (row["PhoneNo"].ToString() != "" && (row["PhoneNo"].ToString().Trim().Length < 10 || row["PhoneNo"].ToString().Trim().Length > 12))
                            strAddErr += "<div>PHONE NO IS NOT VALID.</div>";
                        if (row["GST_No"].ToString() != "NA" && row["GST_No"].ToString().Trim().Length != 15)
                            strAddErr += "<div>GST NO IS NOT VALID.</div>";
                        else if (row["GST_No"].ToString().Trim().Length == 15)
                        {
                            Regex regex = new Regex("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$");
                            Match match = regex.Match(row["GST_No"].ToString());
                            if (!match.Success)
                                strAddErr += "<div>GST NO IS NOT VALID.</div>";
                        }

                        lblErr.Text = strAddErr + "<div>DUE TO E-INVOICE PREPARATION, CONFIRM</div>";
                        lblErr.Text += "<div>THE ABOVE MOBILE/PHONE/PINCODE/GST DATA ONCE.</div>";
                        lblErr.Text += "<br/><div>CLICK THE OPTION FOR ";
                        if (lblErr.ID == "lbl_BillAddressErr")
                            lblErr.Text += "<a href=\"cotsdomesticaddress.aspx?qid=2&cid=\">CHANGE THE DATA</a>";
                        else if (lblErr.ID == "lbl_ShipAddressErr")
                            lblErr.Text += "<a href=\"cotsdomesticaddress.aspx?qid=1&cid=\">CHANGE THE DATA</a>";

                        if (strAddErr == "")
                            lblErr.Text += "  OR  <span style='color: -webkit-link;cursor: pointer;text-decoration: underline;' onclick=\"save_MajorDataVerified('" +
                                AddressID + "','" + lblErr.ID + "')\">DATA IS CORRECT</span>";
                        lblErr.Text += "</div>";
                    }
                }
                return strAddress;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        protected void btnSaveCustDetails_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[19];
                sp[0] = new SqlParameter("@CustCode", ddl_UserId.SelectedItem.Value);
                sp[1] = new SqlParameter("@OrderRefNo", txt_OrderRefNo.Text);
                sp[2] = new SqlParameter("@ShipID", ddl_ShippingAddress.SelectedItem.Value);
                sp[3] = new SqlParameter("@BillID", ddl_BillingAddress.SelectedItem.Value);
                sp[4] = new SqlParameter("@PackingMethod", rdo_PackingMethod.SelectedItem.Value);
                sp[5] = new SqlParameter("@PackingOthers", txt_PackingMethodOthers.Text);
                sp[6] = new SqlParameter("@SplIns", txt_SpecialInstruction.Text.Replace("\r\n", "~"));
                sp[7] = new SqlParameter("@PriceRefNo", "Manual Order Prepared By: " + Request.Cookies["TTSUser"].Value);
                sp[8] = new SqlParameter("@DesiredShipDate", DateTime.ParseExact(txt_DesiredShippingDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                sp[9] = new SqlParameter("@FreightCharges", rdo_FreightCharges.SelectedItem.Value);
                sp[10] = new SqlParameter("@DeliveryMethod", rdo_DeliveryMethod.SelectedItem.Value);
                sp[11] = new SqlParameter("@GodownName", txt_transporterGodownName.Text);
                sp[12] = new SqlParameter("@TransportDetails", txt_PreparedTransporter.Text);
                sp[13] = new SqlParameter("@SpecialRequset", txt_SpecialRequest.Text.Replace("\r\n", "~"));
                sp[14] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp[15] = new SqlParameter("@grade", "");
                sp[16] = new SqlParameter("@CGST", chkCGST.Checked == true ? Convert.ToDecimal(txtCGST.Text) : Convert.ToDecimal("0.00"));
                sp[17] = new SqlParameter("@SGST", chkSGST.Checked == true ? Convert.ToDecimal(txtSGST.Text) : Convert.ToDecimal("0.00"));
                sp[18] = new SqlParameter("@IGST", chkIGST.Checked == true ? Convert.ToDecimal(txtIGST.Text) : Convert.ToDecimal("0.00"));
                object resp = (object)daCOTS.ExecuteScalar_SP("SP_INS_CotsManualOrder_OrderMasterdetails", sp);
                if (Convert.ToInt32(resp.ToString()) > 0)
                    Response.Redirect("cots_manualorderprepare_01.aspx?oid=" + Utilities.Encrypt(resp.ToString()) + "&fid=" + Utilities.Encrypt("d"), false);
                else
                    lblErrMsg1.Text = "ORDER NOT SAVED. CONTACT ADMINISTRATOR.";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        [WebMethod]
        public static string Update_MajorData_Confirmation(string addrID)
        {
            try
            {
                using (DataAccess daCOTS = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString))
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@addrID", Convert.ToInt32(addrID)) };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_majordataverified_confirm", sp);
                    if (resp > 0)
                        return "SUCCESS";
                    else
                        return "NOT UPDATE";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "processidrequest", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }

        protected void chkCGST_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void chkSGST_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}