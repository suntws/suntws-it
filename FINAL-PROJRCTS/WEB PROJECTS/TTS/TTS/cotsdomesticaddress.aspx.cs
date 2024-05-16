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
    public partial class cotsdomesticaddress : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_usermaster"].ToString() == "True")
                        {
                            if (Request["qid"] != null && Request["qid"].ToString() != "")
                            {
                                if (Request["qid"].ToString() == "1")
                                {
                                    lblAddressText.Text = "SHIPPING";
                                    divGST.Style.Add("display", "block");
                                    billCtrlDiv.Style.Add("display", "block");
                                }
                                else if (Request["qid"].ToString() == "2")
                                {
                                    lblAddressText.Text = "BILLING";
                                    divGST.Style.Add("display", "none");
                                    billCtrlDiv.Style.Add("display", "none");
                                }
                                hdnQueryType.Value = Request["qid"].ToString();
                            }

                            DataTable dtState = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dom_GstState_codes", DataAccess.Return_Type.DataTable);
                            Utilities.ddl_Binding(ddlState, dtState, "StateName", "StateIntCode", "Choose");

                            DataTable dtName = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_domestic_fullname", DataAccess.Return_Type.DataTable);
                            Utilities.ddl_Binding(ddlCotsCustName, dtName, "custfullname", "custcode", "Choose");

                            if (Request["cid"].ToString() != "")
                            {
                                hdnCotsCustID.Value = Request["cid"].ToString();
                                SqlParameter[] sp = new SqlParameter[1];
                                sp[0] = new SqlParameter("@ID", Convert.ToInt16(Request["cid"].ToString()));
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_Usermasterdetail_Id", sp, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    Utilities.selectedListItem_Find(ddlCotsCustName, dt.Rows[0]["custfullname"].ToString(), "text");
                                    bind_username();
                                    Utilities.selectedListItem_Find(ddlLoginUserName, Request["cid"].ToString(), "value");
                                    hdnLoginName.Value = ddlLoginUserName.SelectedItem.Text;
                                    //if (ddlLoginUserName.SelectedItem.Text != "Choose")
                                    //    ddlLoginUserName_IndexChange(sender, e);
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
                                            if (ddlBillAddress.Items.Count == 2)
                                            {
                                                ddlBillAddress.SelectedIndex = 1;
                                                ddlBillAddress_indexChange(sender, e);
                                            }
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
                    if (ddlLoginUserName.Items.Count == 2)
                    {
                        ddlLoginUserName.SelectedIndex = 1;
                        ddlLoginUserName_IndexChange(null, null);
                    }
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
                    ddlBillAddress.DataSource = null;
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
                hdnLoginName.Value = ddlLoginUserName.SelectedItem.Text;
                hdnCotsCustID.Value = ddlLoginUserName.SelectedItem.Value;
                ddlBillAddress.DataSource = null;
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
                        Utilities.ddl_Binding(ddlBillAddress, dtAddress, "BillAddress", "shipid", "Choose");
                        if (ddlBillAddress.Items.Count == 2)
                        {
                            ddlBillAddress.SelectedIndex = 1;
                            ddlBillAddress_indexChange(sender, e);
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
                    hdnBillID.Value = ddlBillAddress.SelectedItem.Value;
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
                txtCompanyName.Text = ((Label)dlAddressList.Items[row].FindControl("lblCompanyName")).Text;
                txtAddress.Text = ((Label)dlAddressList.Items[row].FindControl("lblShippAddress")).Text.Replace("<br>", "\r\n");
                txtCity.Text = ((Label)dlAddressList.Items[row].FindControl("lblCity")).Text;
                ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByText(((Label)dlAddressList.Items[row].FindControl("lblState")).Text));
                txtStateCode.Text = ddlState.SelectedItem.Value;
                txtCountry.Text = ((Label)dlAddressList.Items[row].FindControl("lblCountry")).Text;
                txtPincode.Text = ((Label)dlAddressList.Items[row].FindControl("lblZipcode")).Text;
                txtAttn.Text = ((Label)dlAddressList.Items[row].FindControl("lblContact")).Text;
                txtPhone.Text = ((Label)dlAddressList.Items[row].FindControl("lblPhone")).Text;
                txtMobile.Text = ((Label)dlAddressList.Items[row].FindControl("lblmobile")).Text;
                txtFax.Text = ((Label)dlAddressList.Items[row].FindControl("lblfax")).Text;
                txtMail.Text = ((Label)dlAddressList.Items[row].FindControl("lblEmail")).Text;
                txtGSTNo.Text = ((Label)dlAddressList.Items[row].FindControl("lblGSTNo")).Text;
                Label lblCGST = (Label)dlAddressList.Items[row].FindControl("lblCGST");
                Label lblSGST = (Label)dlAddressList.Items[row].FindControl("lblSGST");
                Label lblIGST = (Label)dlAddressList.Items[row].FindControl("lblIGST");

                txtCompanyName.Enabled = false;
                chkCGST.Checked = false;
                chkSGST.Checked = false;
                chkIGST.Checked = false;
                if (lblCGST.Text != "")
                {
                    txtCGST.Text = lblCGST.Text;
                    chkCGST.Checked = Convert.ToDecimal(lblCGST.Text) > 0 ? true : false;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptCGST", "chktxtEnableDisable('chkCGST','divCGST');", true);
                }
                if (lblSGST.Text != "")
                {
                    txtSGST.Text = lblSGST.Text;
                    chkSGST.Checked = Convert.ToDecimal(lblSGST.Text) > 0 ? true : false;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptSGST", "chktxtEnableDisable('chkSGST','divSGST');", true);
                }
                if (lblIGST.Text != "")
                {
                    txtIGST.Text = lblIGST.Text;
                    chkIGST.Checked = Convert.ToDecimal(lblIGST.Text) > 0 ? true : false;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptIGST", "chktxtEnableDisable('chkIGST','divIGST');", true);
                }
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
                sp1[8] = new SqlParameter("@statename", ddlState.SelectedItem.Text);
                sp1[9] = new SqlParameter("@PhoneNo", txtPhone.Text);
                sp1[10] = new SqlParameter("@CompanyName", txtCompanyName.Text);
                sp1[11] = new SqlParameter("@EmailID", txtMail.Text);
                sp1[12] = new SqlParameter("@ExciseDuty", Convert.ToDecimal("0.00"));//chkExciseDuty.Checked == true ? Convert.ToDecimal(txtExciseDuty.Text) : Convert.ToDecimal("0.00")
                sp1[13] = new SqlParameter("@Education", Convert.ToDecimal("0.00"));//chkExciseDuty.Checked == true ? Convert.ToDecimal(txtEducation.Text) : Convert.ToDecimal("0.00")
                sp1[14] = new SqlParameter("@HighEducation", Convert.ToDecimal("0.00"));//chkExciseDuty.Checked == true ? Convert.ToDecimal(txtHighEducation.Text) : Convert.ToDecimal("0.00")
                sp1[15] = new SqlParameter("@CstPer", Convert.ToDecimal("0.00"));//chkCST.Checked == true ? Convert.ToDecimal(txtCST.Text) : Convert.ToDecimal("0.00")
                sp1[16] = new SqlParameter("@VatPer1", Convert.ToDecimal("0.00"));//chkVat1.Checked == true ? Convert.ToDecimal(txtVAT1.Text) : Convert.ToDecimal("0.00")
                sp1[17] = new SqlParameter("@AddressType", Convert.ToInt32(Request["qid"].ToString()));
                sp1[18] = new SqlParameter("@BillID", Request["qid"].ToString() == "2" ? 0 : Convert.ToInt32(hdnBillID.Value));
                sp1[19] = new SqlParameter("@LedgerInfo", false);//chkLedgerInfo.Checked
                sp1[20] = new SqlParameter("@StateCode", ddlState.SelectedItem.Value);
                sp1[21] = new SqlParameter("@GstNo", txtGSTNo.Text);
                sp1[22] = new SqlParameter("@CGST", chkCGST.Checked == true ? Convert.ToDecimal(txtCGST.Text) : Convert.ToDecimal("0.00"));
                sp1[23] = new SqlParameter("@SGST", chkSGST.Checked == true ? Convert.ToDecimal(txtSGST.Text) : Convert.ToDecimal("0.00"));
                sp1[24] = new SqlParameter("@IGST", chkIGST.Checked == true ? Convert.ToDecimal(txtIGST.Text) : Convert.ToDecimal("0.00"));
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
                        Response.Redirect("cotsdomesticaddress.aspx?qid=1&cid=" + Request["cid"].ToString(), false);
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