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

namespace TTS
{
    public partial class exportcotsaddress : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_usermaster"].ToString() == "True")
                        {
                            if (Request["qid"] != null && Request["qid"].ToString() != "")
                            {
                                if (Request["qid"].ToString() == "1")
                                {
                                    lblAddressText.Text = "EXPORT CUSTOMER SHIPPING ADDRESS";
                                }
                                else if (Request["qid"].ToString() == "2")
                                {
                                    lblAddressText.Text = "EXPORT CUSTOMER BILLING ADDRESS";
                                    billCtrlDiv.Style.Add("display", "none");
                                }
                                hdnQueryType.Value = Request["qid"].ToString();
                            }
                            ddlCotsCustName.DataSource = "";
                            ddlCotsCustName.DataBind();
                            DataTable dtName = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_export_fullname", DataAccess.Return_Type.DataTable);

                            if (dtName.Rows.Count > 0)
                                Utilities.ddl_Binding(ddlCotsCustName, dtName, "custfullname", "custcode", "Choose");

                            if (Request["cid"].ToString() != "")
                            {
                                SqlParameter[] sp = new SqlParameter[1];
                                sp[0] = new SqlParameter("@ID", Convert.ToInt16(Request["cid"].ToString()));
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_Usermasterdetail_Id", sp, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    Utilities.selectedListItem_Find(ddlCotsCustName, dt.Rows[0]["custfullname"].ToString(), "text");
                                    bind_username();
                                    Utilities.selectedListItem_Find(ddlLoginUserName, Request["cid"].ToString(), "value");
                                    hdnLoginName.Value = ddlLoginUserName.SelectedItem.Text;
                                    hdnCotsCustID.Value = Request["cid"].ToString();
                                    ddlLoginUserName_IndexChange(sender, e);
                                    if (Request["qid"].ToString() == "1")
                                    {
                                        SqlParameter[] sp2 = new SqlParameter[2];
                                        sp2[0] = new SqlParameter("@custcode", Request["cid"].ToString());
                                        sp2[1] = new SqlParameter("@AddressType", 2);
                                        DataTable dtAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList_Customerwise", sp2, DataAccess.Return_Type.DataTable);
                                        if (dtAddress.Rows.Count > 0)
                                        {
                                            hdnBillID.Value = dtAddress.Rows[0]["shipid"].ToString();
                                            Utilities.selectedListItem_Find(ddlBillAddress, dtAddress.Rows[0]["shipid"].ToString(), "value");
                                            ddlBillAddress_indexChange(sender, e);
                                        }
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
                    if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCotsCustName")
                        ddlCotsCustName_IndexChange(sender, e);
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

        private void bind_username()
        {
            if (ddlCotsCustName.SelectedItem.Text != "Choose")
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddlCotsCustName.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddlCotsCustName.SelectedItem.Value) 
                    };
                DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                if (dtUserList.Rows.Count > 0)
                {
                    Utilities.ddl_Binding(ddlLoginUserName, dtUserList, "username", "ID", "Choose");
                    hdnLoginName.Value = dtUserList.Rows[0]["username"].ToString();
                    hdnCotsCustID.Value = dtUserList.Rows[0]["ID"].ToString();
                }
                btnAddressSave.Text = "SAVE";
                hdnAddressID.Value = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "AllCtrlMakeEmpty();", true);
            }
        }

        protected void ddlCotsCustName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCotsCustName")
                {
                    ddlBillAddress.DataSource = "";
                    ddlBillAddress.DataBind();
                    ddlLoginUserName.DataSource = null;
                    ddlLoginUserName.DataBind();
                    dlAddressList.DataSource = null;
                    dlAddressList.DataBind();
                    Utilities.selectedListItem_Find(ddlCotsCustName, hdnCotsName.Value, "text");
                    bind_username();
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
                ddlBillAddress.DataSourceID = null;
                ddlBillAddress.DataBind();
                dlAddressList.DataSource = null;
                dlAddressList.DataBind();
                Select_DropDownValue();
                if (ddlLoginUserName.SelectedItem.Text != "Choose")
                {
                    SqlParameter[] sp1 = new SqlParameter[2];
                    sp1[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                    sp1[1] = new SqlParameter("@AddressType", 2);
                    DataTable dtAddress = new DataTable();
                    if (Request["qid"].ToString() == "2")
                    {
                        dtAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CustomerAddressList", sp1, DataAccess.Return_Type.DataTable);
                        dlAddressList.DataSource = dtAddress;
                        dlAddressList.DataBind();
                    }
                    else if (Request["qid"].ToString() == "1")
                    {
                        dtAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList_Customerwise", sp1, DataAccess.Return_Type.DataTable);
                        if (dtAddress.Rows.Count > 0)
                        {
                            Utilities.ddl_Binding(ddlBillAddress, dtAddress, "BillAddress", "shipid", "Choose");
                        }
                    }
                    btnAddressSave.Text = "SAVE";
                    hdnAddressID.Value = "";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript2", "AllCtrlMakeEmpty();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlBillAddress_indexChange(object sender, EventArgs e)
        {
            try
            {
                if (Request["qid"].ToString() == "1")
                {
                    dlAddressList.DataSource = null;
                    dlAddressList.DataBind();
                    Select_DropDownValue();
                    if (ddlBillAddress.SelectedItem.Text != "Choose")
                    {
                        SqlParameter[] sp1 = new SqlParameter[3];
                        sp1[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                        sp1[1] = new SqlParameter("@AddressType", Convert.ToInt32(Request["qid"].ToString()));
                        sp1[2] = new SqlParameter("@BillID", Convert.ToInt32(ddlBillAddress.SelectedItem.Value));
                        DataTable dtAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CustomerAddressList_BillID", sp1, DataAccess.Return_Type.DataTable);
                        if (dtAddress.Rows.Count > 0)
                        {
                            dlAddressList.DataSource = dtAddress;
                            dlAddressList.DataBind();
                        }
                        btnAddressSave.Text = "SAVE";
                        hdnAddressID.Value = "";
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript3", "AllCtrlMakeEmpty();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void dlAddressList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Select_DropDownValue();
                int row = dlAddressList.SelectedIndex;
                HiddenField hdnSelectID = (HiddenField)dlAddressList.Items[row].FindControl("hdnSelectID");
                hdnAddressID.Value = hdnSelectID.Value;
                Label lblCompanyName = (Label)dlAddressList.Items[row].FindControl("lblCompanyName");
                Label lblShippAddress = (Label)dlAddressList.Items[row].FindControl("lblShippAddress");
                Label lblCity = (Label)dlAddressList.Items[row].FindControl("lblCity");
                Label lblState = (Label)dlAddressList.Items[row].FindControl("lblState");
                Label lblCountry = (Label)dlAddressList.Items[row].FindControl("lblCountry");
                Label lblZipcode = (Label)dlAddressList.Items[row].FindControl("lblZipcode");
                Label lblContact = (Label)dlAddressList.Items[row].FindControl("lblContact");
                Label lblPhone = (Label)dlAddressList.Items[row].FindControl("lblPhone");
                Label lblmobile = (Label)dlAddressList.Items[row].FindControl("lblmobile");
                Label lblfax = (Label)dlAddressList.Items[row].FindControl("lblfax");
                Label lblEmail = (Label)dlAddressList.Items[row].FindControl("lblEmail");

                txtCompanyName.Text = lblCompanyName.Text;
                txtCompanyName.Enabled = false;
                txtAddress.Text = lblShippAddress.Text.Replace("<br>", "\r\n");
                txtCity.Text = lblCity.Text;
                txtState.Text = lblState.Text;
                txtCountry.Text = lblCountry.Text;
                txtPincode.Text = lblZipcode.Text;
                txtAttn.Text = lblContact.Text;
                txtPhone.Text = lblPhone.Text;
                txtMobile.Text = lblmobile.Text;
                txtFax.Text = lblfax.Text;
                txtMail.Text = lblEmail.Text;
                btnAddressSave.Text = "UPDATE";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnAddressSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[26];
                if (btnAddressSave.Text == "SAVE")
                    sp1 = new SqlParameter[26];
                else if (btnAddressSave.Text == "UPDATE")
                    sp1 = new SqlParameter[27];
                sp1[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@contact_name", txtAttn.Text);
                sp1[2] = new SqlParameter("@shipaddress", txtAddress.Text.Replace("\r\n", "~"));
                sp1[3] = new SqlParameter("@city", txtCity.Text);
                sp1[4] = new SqlParameter("@country", txtCountry.Text);
                sp1[5] = new SqlParameter("@zipcode", txtPincode.Text);
                sp1[6] = new SqlParameter("@mobile", txtMobile.Text);
                sp1[7] = new SqlParameter("@fax", txtFax.Text);
                sp1[8] = new SqlParameter("@statename", txtState.Text);
                sp1[9] = new SqlParameter("@PhoneNo", txtPhone.Text);
                sp1[10] = new SqlParameter("@CompanyName", txtCompanyName.Text);
                sp1[11] = new SqlParameter("@EmailID", txtMail.Text);
                sp1[12] = new SqlParameter("@ExciseDuty", Convert.ToDecimal("0.00"));
                sp1[13] = new SqlParameter("@Education", Convert.ToDecimal("0.00"));
                sp1[14] = new SqlParameter("@HighEducation", Convert.ToDecimal("0.00"));
                sp1[15] = new SqlParameter("@CstPer", Convert.ToDecimal("0.00"));
                sp1[16] = new SqlParameter("@VatPer1", Convert.ToDecimal("0.00"));
                sp1[17] = new SqlParameter("@AddressType", Convert.ToInt32(Request["qid"].ToString()));
                sp1[18] = new SqlParameter("@BillID", Request["qid"].ToString() == "2" ? 0 : Convert.ToInt32(hdnBillID.Value));
                sp1[19] = new SqlParameter("@LedgerInfo", false);
                sp1[20] = new SqlParameter("@StateCode", string.Empty);
                sp1[21] = new SqlParameter("@GstNo", string.Empty);
                sp1[22] = new SqlParameter("@CGST", "0.00");
                sp1[23] = new SqlParameter("@SGST", "0.00");
                sp1[24] = new SqlParameter("@IGST", "0.00");
                sp1[25] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                if (btnAddressSave.Text == "UPDATE")
                    sp1[26] = new SqlParameter("@shipid", Convert.ToInt32(hdnAddressID.Value));

                if (btnAddressSave.Text == "SAVE")
                    daCOTS.ExecuteNonQuery_SP("sp_ins_shipbilladdresslist", sp1);
                else if (btnAddressSave.Text == "UPDATE")
                    daCOTS.ExecuteNonQuery_SP("sp_edit_shipbilladdresslist", sp1);
                if (Request["qid"].ToString() == "1")
                {
                    if (Request["cid"].ToString() == "")
                        ddlBillAddress_indexChange(sender, e);
                    else
                        Response.Redirect("default.aspx", false);
                }
                if (Request["qid"].ToString() == "2")
                {
                    if (Request["cid"].ToString() == "")
                        ddlLoginUserName_IndexChange(sender, e);
                    else
                        Response.Redirect("exportcotsaddress.aspx?qid=1&cid=" + Request["cid"].ToString(), false);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "AllCtrlMakeEmpty();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Select_DropDownValue()
        {
            try
            {
                Utilities.selectedListItem_Find(ddlCotsCustName, hdnCotsName.Value, "text");
                Utilities.selectedListItem_Find(ddlLoginUserName, hdnLoginName.Value, "text");
                Utilities.selectedListItem_Find(ddlBillAddress, hdnBillID.Value, "text");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}