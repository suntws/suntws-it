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
    public partial class ExpCustAddressCreation : System.Web.UI.Page
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
                            hdnType.Value = Request.QueryString["type"].ToString();
                            ddl_CustomerSelection.DataSource = null;
                            ddl_CustomerSelection.DataBind();
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_export_fullname", DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                                Utilities.ddl_Binding(ddl_CustomerSelection, dt, "custfullname", "custcode", "CHOOSE");
                            if (hdnType.Value == "new")
                            {
                                lblPageHeading.Text = "Address Creation";
                                btn_SaveRecord.Text = "Save Record";
                            }
                            if (hdnType.Value == "modify")
                            {
                                lblPageHeading.Text = "Address Modification";
                                btn_SaveRecord.Text = "Update Record";
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
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_CustomerSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddl_CustomerSelection.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddl_CustomerSelection.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddl_CustomerSelection.SelectedItem.Value) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        Utilities.ddl_Binding(ddl_UserID, dt, "username", "ID", "CHOOSE");
                        if (ddl_UserID.Items.Count == 2)
                        {
                            ddl_UserID.SelectedIndex = 1;
                            ddl_UserID_SelectedIndexChanged(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_UserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddl_UserID.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@CustCode", ddl_UserID.SelectedItem.Value), new SqlParameter("@AddressType", 2) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList_Customerwise", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        Utilities.ddl_Binding(ddl_BillingAddress, dt, "BillAddress", "shipid", "CHOOSE");
                        ddl_BillingAddress.Items.Insert(1, "ADD NEW BILLING ADDRESS");
                    }
                    else
                    {
                        ddl_BillingAddress.DataSource = "";
                        ddl_BillingAddress.DataBind();
                        ddl_BillingAddress.Items.Insert(0, "ADD NEW BILLING ADDRESS");
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
                if (ddl_BillingAddress.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@custcode", ddl_UserID.SelectedItem.Value), 
                        new SqlParameter("@AddressID", ddl_BillingAddress.SelectedItem.Value) 
                    };
                    DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_CustAddress_IDWise", sp, DataAccess.Return_Type.DataSet);
                    if (ds.Tables.Count > 0)
                    {
                        DataRow dr_Bill = ds.Tables[0].Rows[0];
                        //to fill billing address details
                        txt_CompanyName_Bill.Text = dr_Bill["CompanyName"].ToString();
                        txt_EmailID_Bill.Text = dr_Bill["EmailID"].ToString();
                        txt_Address_Bill.Text = dr_Bill["shipaddress"].ToString().Replace("~", "\r \n");
                        txt_pincode_Bill.Text = dr_Bill["zipcode"].ToString();
                        txt_Contact_Bill.Text = dr_Bill["contact_name"].ToString();
                        txt_Country_Bill.Text = dr_Bill["country"].ToString();
                        txt_State_Bill.Text = dr_Bill["statename"].ToString();
                        txt_City_Bill.Text = dr_Bill["city"].ToString();
                        txt_FaxNo_Bill.Text = dr_Bill["fax"].ToString();
                        txt_PhoneNo_Bill.Text = dr_Bill["PhoneNo"].ToString();
                        txt_MobileNo_Bill.Text = dr_Bill["mobile"].ToString();
                        hdnIDforBill.Value = dr_Bill["ID"].ToString();

                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@CustCode", ddl_UserID.SelectedItem.Value), 
                            new SqlParameter("@BillID", ddl_BillingAddress.SelectedItem.Value) 
                        };
                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ShipingList_BillIDWise", sp1, DataAccess.Return_Type.DataTable);
                        if (dt.Rows.Count > 0)
                        {
                            Utilities.ddl_Binding(ddl_ShippingAddress, dt, "ShipAddress", "shipid", "CHOOSE");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void chk_BillasShip_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chk_BillasShip.Checked)
                {
                    txt_CompanyName_Ship.Text = txt_CompanyName_Bill.Text;
                    txt_EmailID_Ship.Text = txt_EmailID_Bill.Text;
                    txt_Address_Ship.Text = txt_Address_Bill.Text;
                    txt_pincode_Ship.Text = txt_pincode_Bill.Text;
                    txt_Contact_Ship.Text = txt_Contact_Bill.Text;
                    txt_Country_Ship.Text = txt_Country_Bill.Text;
                    txt_State_Ship.Text = txt_State_Bill.Text;
                    txt_City_Ship.Text = txt_City_Bill.Text;
                    txt_FaxNo_Ship.Text = txt_FaxNo_Bill.Text;
                    txt_PhoneNo_Ship.Text = txt_PhoneNo_Bill.Text;
                    txt_MobileNo_Ship.Text = txt_MobileNo_Bill.Text;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "DisablePayTimeDays", "DisableTypeDiv('none');", true);
                }
                else if (!chk_BillasShip.Checked)
                {
                    txt_CompanyName_Ship.Text = "";
                    txt_EmailID_Ship.Text = "";
                    txt_Address_Ship.Text = "";
                    txt_pincode_Ship.Text = "";
                    txt_Contact_Ship.Text = "";
                    txt_Country_Ship.Text = "";
                    txt_State_Ship.Text = "";
                    txt_City_Ship.Text = "";
                    txt_FaxNo_Ship.Text = "";
                    txt_PhoneNo_Ship.Text = "";
                    txt_MobileNo_Ship.Text = "";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "DisablePayTimeDays", "DisableTypeDiv('block');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_SaveRecord_Click(object sender, EventArgs e)
        {
            try
            {
                //btn to save/update records
                int BillIDforShip = 0;
                SqlParameter[] sp_Bill = new SqlParameter[] { 
                    new SqlParameter("@custcode", ddl_UserID.SelectedItem.Value),
                    new SqlParameter("@contact_name", txt_Contact_Bill.Text),
                    new SqlParameter("@shipaddress",txt_Address_Bill.Text.Replace("\r\n", "~")),
                    new SqlParameter("@city", txt_City_Bill.Text),
                    new SqlParameter("@country", txt_Country_Bill.Text),
                    new SqlParameter("@zipcode", txt_pincode_Bill.Text),
                    new SqlParameter("@mobile", txt_MobileNo_Bill.Text),
                    new SqlParameter("@fax", txt_FaxNo_Bill.Text),
                    new SqlParameter("@statename", txt_State_Bill.Text),
                    new SqlParameter("@PhoneNo", txt_PhoneNo_Bill.Text),
                    new SqlParameter("@CompanyName", txt_CompanyName_Bill.Text),
                    new SqlParameter("@EmailID", txt_EmailID_Bill.Text),
                    new SqlParameter("@ExciseDuty", Convert.ToDecimal("0.00")),
                    new SqlParameter("@Education", Convert.ToDecimal("0.00")),
                    new SqlParameter("@HighEducation", Convert.ToDecimal("0.00")),
                    new SqlParameter("@CstPer", Convert.ToDecimal("0.00")),
                    new SqlParameter("@VatPer1", Convert.ToDecimal("0.00")),
                    new SqlParameter("@AddressType", 2),
                    new SqlParameter("@BillID",Convert.ToInt32(0)),
                    new SqlParameter("@LedgerInfo", false),
                    new SqlParameter("@StateCode", string.Empty),
                    new SqlParameter("@GstNo", string.Empty),
                    new SqlParameter("@CGST", "0.00"),
                    new SqlParameter("@SGST", "0.00"),
                    new SqlParameter("@IGST", "0.00"),
                    new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@shipid",hdnType.Value=="modify"?Convert.ToInt32(hdnIDforBill.Value):0)
                };

                SqlParameter[] sp_Ship = new SqlParameter[] { 
                    new SqlParameter("@custcode", ddl_UserID.SelectedItem.Value),
                    new SqlParameter("@contact_name", txt_Contact_Ship.Text),
                    new SqlParameter("@shipaddress",txt_Address_Ship.Text.Replace("\r\n", "~")),
                    new SqlParameter("@city", txt_City_Ship.Text),
                    new SqlParameter("@country", txt_Country_Ship.Text),
                    new SqlParameter("@zipcode", txt_pincode_Ship.Text),
                    new SqlParameter("@mobile", txt_MobileNo_Ship.Text),
                    new SqlParameter("@fax", txt_FaxNo_Ship.Text),
                    new SqlParameter("@statename", txt_State_Ship.Text),
                    new SqlParameter("@PhoneNo", txt_PhoneNo_Ship.Text),
                    new SqlParameter("@CompanyName", txt_CompanyName_Ship.Text),
                    new SqlParameter("@EmailID", txt_EmailID_Ship.Text),
                    new SqlParameter("@ExciseDuty", Convert.ToDecimal("0.00")),
                    new SqlParameter("@Education", Convert.ToDecimal("0.00")),
                    new SqlParameter("@HighEducation", Convert.ToDecimal("0.00")),
                    new SqlParameter("@CstPer", Convert.ToDecimal("0.00")),
                    new SqlParameter("@VatPer1", Convert.ToDecimal("0.00")),
                    new SqlParameter("@AddressType", 1),
                    new SqlParameter("@BillID", BillIDforShip),
                    new SqlParameter("@LedgerInfo", false),
                    new SqlParameter("@StateCode", string.Empty),
                    new SqlParameter("@GstNo", string.Empty),
                    new SqlParameter("@CGST", "0.00"),
                    new SqlParameter("@SGST", "0.00"),
                    new SqlParameter("@IGST", "0.00"),
                    new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@shipid",hdnType.Value=="modify"?Convert.ToInt32(hdnIDforShip.Value):0)
                };

                if (btn_SaveRecord.Text != "Add New Record")
                {
                    string strSpName = hdnType.Value == "new" ? "sp_ins_ShipBillAddress" : "sp_upd_ShipBillAddress";
                    int resp = daCOTS.ExecuteNonQuery_SP(strSpName, sp_Bill);
                    if (resp > 0)
                    {
                        string query = "select ID from shipbilladdresslist where custcode='" + ddl_UserID.SelectedItem.Value + "' and AddressType=2";
                        BillIDforShip = (int)daCOTS.ExecuteScalar(query);
                        if (BillIDforShip > 0)
                        {
                            int resp1 = daCOTS.ExecuteNonQuery_SP(strSpName, sp_Ship);
                            if (resp1 > 0)
                                ScriptManager.RegisterStartupScript(Page, GetType(), "SuccessAlert1", "alert('Record Saved Successfully');", true);
                        }
                    }
                }
                else if (btn_SaveRecord.Text == "Add New Record")
                {
                    sp_Ship[18] = new SqlParameter("@BillID", Convert.ToInt32(ddl_BillingAddress.SelectedItem.Value));
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_ShipBillAddress", sp_Ship);
                    if (resp > 0)
                        ScriptManager.RegisterStartupScript(Page, GetType(), "SuccessAlert2", "alert('Record Saved Successfully');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_AddNewShipAddress_Click(object sender, EventArgs e)
        {
            try
            {
                //btn to add new ship address
                txt_CompanyName_Ship.Text = "";
                txt_EmailID_Ship.Text = "";
                txt_Address_Ship.Text = "";
                txt_pincode_Ship.Text = "";
                txt_Contact_Ship.Text = "";
                txt_Country_Ship.Text = "";
                txt_State_Ship.Text = "";
                txt_City_Ship.Text = "";
                txt_FaxNo_Ship.Text = "";
                txt_PhoneNo_Ship.Text = "";
                txt_MobileNo_Ship.Text = "";
                btn_SaveRecord.Text = "Add New Record";
                txt_CompanyName_Ship.Enabled = true;
                ScriptManager.RegisterStartupScript(Page, GetType(), "DisableBillAddress", "DisableBillAddressDiv();", true);
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
                if (ddl_ShippingAddress.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@custcode", ddl_UserID.SelectedItem.Value), 
                        new SqlParameter("@AddressID", ddl_ShippingAddress.SelectedItem.Value) 
                    };
                    DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_CustAddress_IDWise", sp, DataAccess.Return_Type.DataSet);
                    if (ds.Tables.Count > 0)
                    {
                        DataRow dr_Ship = ds.Tables[0].Rows[0];
                        //to fill billing Shipping details
                        txt_CompanyName_Ship.Text = dr_Ship["CompanyName"].ToString();
                        txt_EmailID_Ship.Text = dr_Ship["EmailID"].ToString();
                        txt_Address_Ship.Text = dr_Ship["shipaddress"].ToString().Replace("~", "\r \n");
                        txt_pincode_Ship.Text = dr_Ship["zipcode"].ToString();
                        txt_Contact_Ship.Text = dr_Ship["contact_name"].ToString();
                        txt_Country_Ship.Text = dr_Ship["country"].ToString();
                        txt_State_Ship.Text = dr_Ship["statename"].ToString();
                        txt_City_Ship.Text = dr_Ship["city"].ToString();
                        txt_FaxNo_Ship.Text = dr_Ship["fax"].ToString();
                        txt_PhoneNo_Ship.Text = dr_Ship["PhoneNo"].ToString();
                        txt_MobileNo_Ship.Text = dr_Ship["mobile"].ToString();
                        hdnIDforShip.Value = dr_Ship["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}