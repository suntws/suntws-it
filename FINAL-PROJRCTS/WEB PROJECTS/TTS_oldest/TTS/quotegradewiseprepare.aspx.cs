using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace TTS
{
    public partial class quotegradewiseprepare : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        public bool isExisting = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_quote"].ToString() == "True")
                        {
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AllQuoteDiscount", DataAccess.Return_Type.DataTable);
                            ViewState["DiscStru"] = dt;
                            DataTable dtgv1 = new DataTable();
                            dtgv1.Columns.Add("slno", typeof(String));
                            for (int i = 1; i <= 3; i++)
                                dtgv1.Rows.Add(i);
                            gvAmountSub.DataSource = dtgv1;
                            gvAmountSub.DataBind();
                            txtCGST.Text = "0.00";
                            txtSGST.Text = "0.00";
                            txtIGST.Text = "0.00";
                            chkCGST.Checked = false;
                            chkSGST.Checked = false;
                            chkIGST.Checked = false;

                            lblQuoteRefHead.Text = "";
                            string strTerms = string.Empty;
                            strTerms += "1. Valid for 30 days from the date of the offer.\r\n";
                            strTerms += "2. The prices quoted above are Ex-Works, exclusive of freight to destination.\r\n";
                            strTerms += "3. GST @ 28% shall be levied in accordance to the ordance. Should there be any charges in the rates of GST at the time of supply shall be applicable.\r\n";
                            txtQuoteTerms.Text = strTerms;

                            if (Request["qac"] == null && Request["qref"] == null)
                            {
                                DataTable dtQuoteItem = new DataTable();
                                dtQuoteItem.Columns.Add("Config", typeof(String));
                                dtQuoteItem.Columns.Add("Brand", typeof(String));
                                dtQuoteItem.Columns.Add("Sidewall", typeof(String));
                                dtQuoteItem.Columns.Add("TyreType", typeof(String));
                                dtQuoteItem.Columns.Add("TyreSize", typeof(String));
                                dtQuoteItem.Columns.Add("RimSize", typeof(String));
                                dtQuoteItem.Columns.Add("ListPrice", typeof(decimal));
                                dtQuoteItem.Columns.Add("BasicPrice", typeof(decimal));
                                dtQuoteItem.Columns.Add("Qty", typeof(int));
                                dtQuoteItem.Columns.Add("TotalPrice", typeof(decimal));
                                dtQuoteItem.Columns.Add("Discount", typeof(decimal));
                                dtQuoteItem.Columns.Add("ProcessID", typeof(String));
                                dtQuoteItem.Columns.Add("FinishedWt", typeof(decimal));
                                dtQuoteItem.Columns.Add("LoadingQty", typeof(decimal));
                                dtQuoteItem.Columns.Add("SizePosition", typeof(int));
                                dtQuoteItem.Columns.Add("TypePosition", typeof(int));
                                dtQuoteItem.Columns.Add("category", typeof(String));
                                dtQuoteItem.Columns.Add("TypeDesc", typeof(String));
                                ViewState["dtQuoteItem"] = dtQuoteItem;
                                btnQuotePrepareSend.Style.Add("display", "none");
                                btnQuoteSave.Style.Add("display", "none");
                                btnQuoteDelete.Style.Add("display", "none");
                                lblQuoteHead.Text = "QUOTATION PREPARE";
                            }
                            else if (Request["qac"] != null && Request["qref"] != null)
                            {
                                lblQuoteHead.Text = "QUOTATION REVISE";
                                SqlParameter[] sp1 = new SqlParameter[2];
                                sp1[0] = new SqlParameter("@QAcYear", Request["qac"].ToString());
                                sp1[1] = new SqlParameter("@QRefNo", Request["qref"].ToString());
                                DataTable dtQMaster = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_QuotePrepare_MasterDetails", sp1, DataAccess.Return_Type.DataTable);

                                if (dtQMaster.Rows.Count > 0)
                                {
                                    DataRow row = dtQMaster.Rows[0];
                                    lblQuoteAcYear.Text = Request["qac"].ToString();
                                    lblQuoteRefNo.Text = Request["qref"].ToString();
                                    lblQuoteReviseCount.Text = row["QRevisedCount"].ToString();
                                    txtQuoteCustomer.Text = row["QCustomer"].ToString();

                                    txtBillContactName.Text = row["QPerson"].ToString();

                                    txtBillContactNo.Text = row["QPhoneNo"].ToString();
                                    rdbTYPECustomer.Items.FindByText(row["customer"].ToString()).Selected = true;
                                    //rdbTYPECustomer.Text = row["customer"].ToString();
                                    lblDUserName.Text = row["USERID"].ToString();
                                    if (rdbTYPECustomer.Text == "NEW CUSTOMER")
                                    {
                                        txtQuoteCustomer.Visible = true;
                                        ddlUsername.Visible = false;
                                        rdbTYPECustomer.SelectedIndex = 0;
                                        SqlParameter[] spAddress = new SqlParameter[]{
                                                new SqlParameter("@QAcYear", Request["qac"].ToString()),
                                                new SqlParameter("@QRefNo", Request["qref"].ToString())
                                            };
                                        DataTable dtAddress = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_QuotePrepare_NewCustAddress", spAddress, DataAccess.Return_Type.DataTable);
                                        if (dtAddress.Rows.Count > 0)
                                        {
                                            DataRow dr = dtAddress.Rows[0];
                                            txtBillCompanyName.Text = dr["BillCompanyName"].ToString();
                                            txtBillAddress.Text = dr["BillAddress"].ToString();
                                            txtBillCity.Text = dr["BillCity"].ToString();
                                            txtBillZipCode.Text = dr["BillZipCode"].ToString();
                                            txtBillState.Text = dr["BillStateName"].ToString();
                                            txtBillStateCode.Text = dr["BillStateCode"].ToString();
                                            txtBillGSTNo.Text = dr["BillGST_No"].ToString();
                                            txtBillContactName.Text = dr["BillContactName"].ToString();
                                            txtBillContactNo.Text = dr["BillContactNo"].ToString();

                                            txtConsCompanyName.Text = dr["ShipCompanyName"].ToString();
                                            txtConsAddress.Text = dr["ShipAddress"].ToString();
                                            txtConsCity.Text = dr["ShipCity"].ToString();
                                            txtConsZipCode.Text = dr["ShipZipCode"].ToString();
                                            txtConsState.Text = dr["ShipStateName"].ToString();
                                            txtConsStateCode.Text = dr["ShipStateCode"].ToString();
                                            txtConsGSTNo.Text = dr["ShipGST_No"].ToString();
                                            txtConsContactName.Text = dr["ShipContactName"].ToString();
                                            txtConsContactNo.Text = dr["ShipContactNo"].ToString();
                                        }
                                    }
                                    else if (rdbTYPECustomer.Text == "EXISTING CUSTOMER")
                                    {
                                        ddlQuotecustomer.Visible = false;
                                        txtQuoteCustomer.Visible = true;
                                        ddlUsername.Visible = false;
                                        rdbTYPECustomer.SelectedIndex = 1;
                                        ddlBillingAddress.Visible = true;
                                        ddlShippingAddress.Visible = true;

                                        ddlUsername_SelectedIndexChanged(null, null);
                                        System.Web.UI.WebControls.ListItem Bli = ddlBillingAddress.Items.FindByValue(row["BillId"].ToString());
                                        if (Bli != null)
                                        {
                                            Bli.Selected = true;
                                            ddlBillingAddress.Enabled = false;

                                            ddlBillingAddress_IndexChange(null, null);
                                            System.Web.UI.WebControls.ListItem Sli = ddlShippingAddress.Items.FindByValue(row["ShipId"].ToString());
                                            if (Sli != null)
                                            {
                                                Sli.Selected = true;
                                                ddlShippingAddress.Enabled = false;
                                                ddlShippingAddress_IndexChange(null, null);
                                            }
                                        }
                                    }
                                    txtQuoteEmail.Text = row["QEmail"].ToString();
                                    txtQuoteCC.Text = row["QCC"].ToString();
                                    bindOtherCharges();
                                    if (dtQMaster.Rows[0]["CGST"].ToString() != "" && Convert.ToDecimal(dtQMaster.Rows[0]["CGST"].ToString()) > 0)
                                    {
                                        txtCGST.Text = dtQMaster.Rows[0]["CGST"].ToString();
                                        lblCGST.Text = dtQMaster.Rows[0]["CGSTVal"].ToString();
                                        chkCGST.Checked = true;
                                    }

                                    if (dtQMaster.Rows[0]["SGST"].ToString() != "" && Convert.ToDecimal(dtQMaster.Rows[0]["SGST"].ToString()) > 0)
                                    {
                                        txtSGST.Text = dtQMaster.Rows[0]["SGST"].ToString();
                                        lblSGST.Text = dtQMaster.Rows[0]["SGSTVal"].ToString();
                                        chkSGST.Checked = true;
                                    }

                                    if (dtQMaster.Rows[0]["IGST"].ToString() != "" && Convert.ToDecimal(dtQMaster.Rows[0]["IGST"].ToString()) > 0)
                                    {
                                        txtIGST.Text = dtQMaster.Rows[0]["IGST"].ToString();
                                        lblIGST.Text = dtQMaster.Rows[0]["IGSTVal"].ToString();
                                        chkIGST.Checked = true;
                                    }
                                    rdbQuoteType.Items.FindByText(row["QCustType"].ToString()).Selected = true;
                                    rdbQuoteGrade.Items.FindByText(row["QGrade"].ToString()).Selected = true;
                                    txtQuoteTerms.Text = row["QTerms"].ToString().Replace("~", "\r\n");
                                    hdnMaxQuoteDiscount.Value = row["QMaxDisc"].ToString();
                                    lblQuoteRefHead.Text = "Quote Ref No.: " + lblQuoteAcYear.Text + "/" + lblQuoteRefNo.Text + "        Revise No.: " + lblQuoteReviseCount.Text;
                                    rdbQuoteType_IndexChange(sender, e);

                                    SqlParameter[] sp2 = new SqlParameter[2];
                                    sp2[0] = new SqlParameter("@QAcYear", Request["qac"].ToString());
                                    sp2[1] = new SqlParameter("@QRefNo", Request["qref"].ToString());

                                    DataTable dtQItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_quote_itemdetails", sp2, DataAccess.Return_Type.DataTable);
                                    if (dtQItems.Rows.Count > 0)
                                    {
                                        Bind_gvOrderItem(dtQItems);
                                    }
                                    if (lblQuoteReviseCount.Text != "0")
                                        lnkQuoteFile.Text = lblQuoteAcYear.Text + "_" + lblQuoteRefNo.Text + "_" + lblQuoteReviseCount.Text + ".pdf";
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JMsg100", "bind_errmsg('NO RECORDS FOUND');", true);
                                }
                            }
                        }

                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                    if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlSize")
                        ddlSize_IndexChange(sender, e);
                    if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlRim")
                        ddlRim_IndexChange(sender, e);
                    if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlQuotecustomer")
                        ddlQuotecustomer_SelectedIndexChanged(sender, e);
                    if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlUsername")
                        ddlUsername_SelectedIndexChanged(sender, e);
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

        private void bindOtherCharges()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@QAcYear", Request["qac"].ToString());
                sp1[1] = new SqlParameter("@QRefNo", Request["qref"].ToString());
                DataTable dtOtherCharges = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_QuotePrepare_OtherCharge", sp1, DataAccess.Return_Type.DataTable);
                if (dtOtherCharges.Rows.Count > 0)
                {
                    int gvrowindex = 0;
                    for (int i = 0; i < dtOtherCharges.Rows.Count; i++)
                    {
                        if (dtOtherCharges.Rows[i]["Discounttype"].ToString() == "OTHER CHARGES")
                        {
                            TextBox txt = gvAmountSub.Rows[gvrowindex].FindControl("txtAddDesc") as TextBox;
                            txt.Text = dtOtherCharges.Rows[i]["DescriptionDiscount"].ToString();
                            TextBox txtAmnt = gvAmountSub.Rows[gvrowindex].FindControl("txtCAddAmt") as TextBox;
                            txtAmnt.Text = dtOtherCharges.Rows[i]["Amount"].ToString();
                            gvrowindex++;
                        }
                        else if (dtOtherCharges.Rows[i]["Discounttype"].ToString() == "CLAIM ADJUSTMENT")
                        {
                            txtClaimAdjustment.Text = dtOtherCharges.Rows[i]["DescriptionDiscount"].ToString();
                            txtLESSAMT.Text = dtOtherCharges.Rows[i]["Amount"].ToString();
                        }
                        else if (dtOtherCharges.Rows[i]["Discounttype"].ToString() == "OTHER DISCOUNT")
                        {
                            txtotherdiscount.Text = dtOtherCharges.Rows[i]["DescriptionDiscount"].ToString();
                            txtOtherDisAmt.Text = dtOtherCharges.Rows[i]["Amount"].ToString();
                        }
                    }
                    gvAmountSub_Amount_TextChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);

            }
        }

        protected void rdbQuoteType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlPlatform.DataSource = "";
                ddlPlatform.DataBind();
                DataTable dtDiscType = (DataTable)daTTS.ExecuteReader_SP("SP_SEL_QuotePrepare_ApprovedList", DataAccess.Return_Type.DataTable);
                if (dtDiscType.Rows.Count > 0)
                {
                    ViewState["dtDiscType"] = dtDiscType;
                    DataView dtTypeView = new DataView(dtDiscType);
                    dtTypeView.Sort = "Config ASC";
                    DataTable disConfig = dtTypeView.ToTable(true, "Config");

                    if (disConfig.Rows.Count > 0)
                    {
                        Bind_DiscountStructure();

                        ddlPlatform.DataSource = disConfig;
                        ddlPlatform.DataTextField = "Config";
                        ddlPlatform.DataValueField = "Config";
                        ddlPlatform.DataBind();
                        if (disConfig.Rows.Count == 1)
                            ddlPlatform_IndexChange(sender, e);
                        else
                            ddlPlatform.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void rdbQuoteGrade_IndexChange(object sender, EventArgs e)
        {
            Bind_DiscountStructure();
        }

        private void Bind_DiscountStructure()
        {
            try
            {
                DataTable dt = ViewState["DiscStru"] as DataTable;
                if (rdbQuoteGrade.SelectedItem != null && rdbQuoteType.SelectedItem != null)
                {
                    foreach (DataRow row in dt.Select("Grade='" + rdbQuoteGrade.SelectedItem.Text + "' and CustType='" + rdbQuoteType.SelectedItem.Text + "'"))
                    {
                        if (Request.Cookies["TTSUserType"].Value.ToLower() == "admin" || Request.Cookies["TTSUserType"].Value.ToLower() == "support" || Request.Cookies["TTSUserType"].Value.ToLower() == "")
                            hdnMaxQuoteDiscount.Value = row["ManagerPer"].ToString() == "" ? "0" : row["ManagerPer"].ToString();
                        else
                            hdnMaxQuoteDiscount.Value = row[Request.Cookies["TTSUserType"].Value + "Per"].ToString() == "" ? "0" : row[Request.Cookies["TTSUserType"].Value + "Per"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlPlatform_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                DataTable dtDiscType = ViewState["dtDiscType"] as DataTable;
                if (dtDiscType.Rows.Count > 0)
                {
                    //DataTable dt = new DataTable();
                    //DataColumn dmaincol = new DataColumn("Brand", typeof(System.String));
                    //dt.Columns.Add(dmaincol);

                    //foreach (DataRow row in dtDiscType.Select("Config='" + ddlPlatform.SelectedItem.Text + "'"))
                    //{
                    //    dt.Rows.Add(row["Brand"].ToString());
                    //}

                    DataView dtTypeView = new DataView(dtDiscType);
                    dtTypeView.RowFilter = "Config = '" + ddlPlatform.SelectedItem.Text + "'";
                    dtTypeView.Sort = "Brand ASC";
                    DataTable disBrand = dtTypeView.ToTable(true, "Brand");
                    if (disBrand.Rows.Count > 0)
                    {
                        ddlBrand.DataSource = disBrand;
                        ddlBrand.DataTextField = "Brand";
                        ddlBrand.DataValueField = "Brand";
                        ddlBrand.DataBind();
                        if (disBrand.Rows.Count == 1)
                            ddlBrand_IndexChange(sender, e);
                        else
                            ddlBrand.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlBrand_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();

                DataTable dtDiscType = ViewState["dtDiscType"] as DataTable;
                if (dtDiscType.Rows.Count > 0)
                {
                    DataView dtTypeView = new DataView(dtDiscType);
                    dtTypeView.RowFilter = "Config = '" + ddlPlatform.SelectedItem.Text + "' and Brand='" + ddlBrand.SelectedItem.Text + "'";
                    dtTypeView.Sort = "Sidewall ASC";
                    DataTable disSidewall = dtTypeView.ToTable(true, "Sidewall");
                    if (disSidewall.Rows.Count > 0)
                    {
                        ddlSidewall.DataSource = disSidewall;
                        ddlSidewall.DataTextField = "Sidewall";
                        ddlSidewall.DataValueField = "Sidewall";
                        ddlSidewall.DataBind();
                        if (disSidewall.Rows.Count == 1)
                            ddlSidewall_IndexChange(sender, e);
                        else
                            ddlSidewall.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                    }
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlSidewall_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlType.DataSource = "";
                ddlType.DataBind();

                DataTable dtDiscType = ViewState["dtDiscType"] as DataTable;
                if (dtDiscType.Rows.Count > 0)
                {
                    //DataTable dt = new DataTable();
                    //DataColumn dmaincol = new DataColumn("TyreType", typeof(System.String));
                    //dt.Columns.Add(dmaincol);

                    //foreach (DataRow row in dtDiscType.Select("Config='" + ddlPlatform.SelectedItem.Text + "' and Brand='" + ddlBrand.SelectedItem.Text + "' and Sidewall='" + ddlSidewall.SelectedItem.Text + "'"))
                    //{
                    //    dt.Rows.Add(row["TyreType"].ToString());
                    //}

                    DataView dtTypeView = new DataView(dtDiscType);
                    dtTypeView.RowFilter = "Config = '" + ddlPlatform.SelectedItem.Text + "' and Brand='" + ddlBrand.SelectedItem.Text + "' and Sidewall='" + ddlSidewall.SelectedItem.Text + "'";
                    dtTypeView.Sort = "TyreType ASC";
                    DataTable disTyreType = dtTypeView.ToTable(true, "TyreType");
                    if (disTyreType.Rows.Count > 0)
                    {
                        ddlType.DataSource = disTyreType;
                        ddlType.DataTextField = "TyreType";
                        ddlType.DataValueField = "TyreType";
                        ddlType.DataBind();
                        if (disTyreType.Rows.Count == 1)
                            ddlType_IndexChange(sender, e);
                        else
                            ddlType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JMsg1", "bind_errmsg('');", true);
                ddlSize.DataSource = "";
                ddlSize.DataBind();

                txtQuoteDiscount.Text = "0";

                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);

                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Size_For_ManualOrder", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlSize.DataSource = dt;
                    ddlSize.DataTextField = "TyreSize";
                    ddlSize.DataValueField = "TyreSize";
                    ddlSize.DataBind();
                    if (dt.Rows.Count == 1)
                        ddlSize_IndexChange(sender, e);
                    else
                        ddlSize.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JMsg2", "bind_errmsg('Size not available for this type in process-ID List ');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JMsg3", "bind_errmsg('');", true);
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtQuoteQty.Text = "";

                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                sp1[4] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);
                //sp_sel_quote_rimsize
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Rim_ForManualOrder", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlRim.DataSource = dt;
                    ddlRim.DataTextField = "RimSize";
                    ddlRim.DataValueField = "RimSize";
                    ddlRim.DataBind();
                    if (dt.Rows.Count == 1)
                        ddlRim_IndexChange(sender, e);
                    else
                        ddlRim.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JMsg4", "bind_errmsg('rim width list not available in published price-sheet list');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlRim_IndexChange(object sender, EventArgs e)
        {
            try
            {
                txtBasicPrice.Text = "0.00";
                txtCustomPriceSheet.Text = "0.00";
                hdnProcessID.Text = "";
                txtFinishedWt.Text = "";

                SqlParameter[] sp1 = new SqlParameter[6];
                sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                sp1[4] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);
                sp1[5] = new SqlParameter("@rimsize", ddlRim.SelectedItem.Text);

                //DataTable dt = (DataTable)daTTS.ExecuteReader_SP("SP_GET_QuotationPrepare_FinishedWt", sp1, DataAccess.Return_Type.DataTable);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_applicable_quoteprice", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    hdnProcessID.Text = dt.Rows[0]["ProcessID"].ToString();
                    txtFinishedWt.Text = dt.Rows[0]["FinishedWt"].ToString();
                    hdncategory.Text = dt.Rows[0]["SizeCategory"].ToString();
                    txtCustomPriceSheet.Text = dt.Rows[0]["UnitPrice"].ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JMsg7", "bind_errmsg('Price not available');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnQuoteMoreItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtQuoteItem"] as DataTable;
                bool dtExist = false;
                if (dtQuoteItem.Rows.Count > 0)
                {
                    foreach (DataRow row in dtQuoteItem.Select("ProcessID='" + hdnProcessID.Text + "'"))
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JMsg7", "bind_errmsg('Item already added. You can delete the item and re-enter');", true);
                        dtExist = true;
                    }
                }
                if (!dtExist)
                    Build_QuoteOrderItem(dtQuoteItem);
                if (hdnFullName.Value != "")
                {
                    System.Web.UI.WebControls.ListItem selectedListItem = ddlQuotecustomer.Items.FindByText("" + hdnFullName.Value + "");
                    if (selectedListItem != null)
                    {
                        ddlQuotecustomer.Items.FindByText(ddlQuotecustomer.SelectedItem.Text).Selected = false;
                        selectedListItem.Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Build_QuoteOrderItem(DataTable dtItem)
        {
            try
            {
                lblQtyTotal.Text = "";
                lblWtTotal.Text = "";
                lblPriceTotal.Text = "";
                decimal decBasicPrice = Convert.ToDecimal(txtQuoteDiscount.Text) > 0 ?
                       Math.Round((Convert.ToDecimal(txtCustomPriceSheet.Text) - (Convert.ToDecimal(txtCustomPriceSheet.Text) * (Convert.ToDecimal(txtQuoteDiscount.Text) / 100)))) :
                        Convert.ToDecimal(txtCustomPriceSheet.Text);
                DataRow nRow = dtItem.NewRow();
                nRow["Config"] = ddlPlatform.SelectedItem.Text;
                nRow["Brand"] = ddlBrand.SelectedItem.Text;
                nRow["Sidewall"] = ddlSidewall.SelectedItem.Text;
                nRow["TyreType"] = ddlType.SelectedItem.Text;
                nRow["TyreSize"] = ddlSize.SelectedItem.Text;
                nRow["RimSize"] = ddlRim.SelectedItem.Text;
                nRow["ListPrice"] = txtCustomPriceSheet.Text == "" ? 0 : Convert.ToDecimal(txtCustomPriceSheet.Text);
                nRow["BasicPrice"] = decBasicPrice.ToString("0.00");
                nRow["Qty"] = txtQuoteQty.Text == "" ? 0 : Convert.ToInt32(txtQuoteQty.Text);
                nRow["TotalPrice"] = (Convert.ToDecimal(txtQuoteQty.Text) * decBasicPrice).ToString("0.00");
                nRow["Discount"] = txtQuoteDiscount.Text != "" ? Convert.ToDecimal(txtQuoteDiscount.Text) : 0;
                nRow["ProcessID"] = hdnProcessID.Text;
                nRow["FinishedWt"] = txtFinishedWt.Text != "" ? Convert.ToDecimal(txtQuoteQty.Text) * Convert.ToDecimal(txtFinishedWt.Text) : 0;
                nRow["LoadingQty"] = hdnLoadingQty.Text != "" ? Convert.ToDecimal(hdnLoadingQty.Text) : 0;
                nRow["SizePosition"] = hdnSizePosition.Text != "" ? Convert.ToDecimal(hdnSizePosition.Text) : 0;
                nRow["TypePosition"] = hdnTypePosition.Text != "" ? Convert.ToDecimal(hdnTypePosition.Text) : 0;
                nRow["category"] = hdncategory.Text;
                nRow["TypeDesc"] = hdnTypeDesc.Text.Trim() != "" ? hdnTypeDesc.Text.Trim() : "";
                dtItem.Rows.Add(nRow);
                Bind_gvOrderItem(dtItem);

                ddlRim.SelectedIndex = 0;
                txtQuoteQty.Text = "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_gvOrderItem(DataTable dtQItem)
        {
            btnQuotePrepareSend.Style.Add("display", "none");
            btnQuoteSave.Style.Add("display", "none");
            btnQuoteDelete.Style.Add("display", "none");
            lblQtyTotal.Text = "";
            lblWtTotal.Text = "";
            lblPriceTotal.Text = "";
            gvQuoteItem.DataSource = null;
            gvQuoteItem.DataBind();
            ddlQuotecustomer.Enabled = true;
            ddlUsername.Enabled = true;
            rdbTYPECustomer.Enabled = true;
            txtBillContactName.Enabled = true;
            txtBillContactNo.Enabled = true;
            txtQuoteEmail.Enabled = true;
            txtQuoteCC.Enabled = true;
            txtBillCompanyName.Enabled = true;
            txtBillAddress.Enabled = true;
            txtBillCity.Enabled = true;
            txtBillZipCode.Enabled = true;
            txtBillState.Enabled = true;
            txtBillStateCode.Enabled = true;
            txtBillGSTNo.Enabled = true;
            txtConsContactName.Enabled = true;
            txtConsContactNo.Enabled = true;
            txtConsCompanyName.Enabled = true;
            txtConsAddress.Enabled = true;
            txtConsCity.Enabled = true;
            txtConsZipCode.Enabled = true;
            txtConsState.Enabled = true;
            txtConsStateCode.Enabled = true;
            txtConsGSTNo.Enabled = true;

            rdbQuoteType.Enabled = true;
            rdbQuoteGrade.Enabled = true;
            txtQuoteCustomer.Enabled = true;

            if (dtQItem.Rows.Count > 0)
            {
                gvQuoteItem.DataSource = dtQItem;
                gvQuoteItem.DataBind();
                btnQuotePrepareSend.Style.Add("display", "block");
                btnQuoteSave.Style.Add("display", "block");
                btnQuoteDelete.Style.Add("display", "none");
                if (lblQuoteRefHead.Text != "")
                {
                    btnQuoteSave.Style.Add("display", "none");
                    btnQuoteDelete.Style.Add("display", "block");
                }
                object sumQty;
                sumQty = dtQItem.Compute("Sum(Qty)", "");
                lblQtyTotal.Text = "TOTAL QTY: " + sumQty.ToString();
                object sumWt;
                sumWt = dtQItem.Compute("Sum(FinishedWt)", "");
                lblWtTotal.Text = "TOTAL WEIGHT: " + sumWt.ToString();
                object sumCost;
                sumCost = dtQItem.Compute("Sum(TotalPrice)", "");
                lblPriceTotal.Text = "TOTAL PRICE: " + Math.Round(Convert.ToDecimal(sumCost)).ToString("0.00");
                ViewState["dtQuoteItem"] = dtQItem;
                hdnTotalBasicPrice.Value = Math.Round(Convert.ToDecimal(sumCost)).ToString("0.00");
                gvAmountSub_Amount_TextChanged(null, null);

                //hdnTotalAmount.Value = Math.Round(Convert.ToDecimal(hdnTotalAmount.Value) + Convert.ToDecimal(sumCost)).ToString("0.00");
                //Build_TaxCalculation();
                ddlQuotecustomer.Enabled = false;
                ddlUsername.Enabled = false;
                rdbTYPECustomer.Enabled = false;
                txtBillContactName.Enabled = false;
                txtBillContactNo.Enabled = false;
                txtQuoteEmail.Enabled = false;
                txtQuoteCC.Enabled = true;
                txtBillCompanyName.Enabled = false;
                txtBillAddress.Enabled = false;
                txtBillCity.Enabled = false;
                txtBillZipCode.Enabled = false;
                txtBillState.Enabled = false;
                txtBillStateCode.Enabled = false;
                txtBillGSTNo.Enabled = false;
                txtConsContactName.Enabled = false;
                txtConsContactNo.Enabled = false;
                txtConsCompanyName.Enabled = false;
                txtConsAddress.Enabled = false;
                txtConsCity.Enabled = false;
                txtConsZipCode.Enabled = false;
                txtConsState.Enabled = false;
                txtConsStateCode.Enabled = false;
                txtConsGSTNo.Enabled = false;

                rdbQuoteType.Enabled = false;
                rdbQuoteGrade.Enabled = false;
                txtQuoteCustomer.Enabled = false;
            }
        }

        protected void gvQuoteItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtQuoteItem"] as DataTable;
                GridViewRow row = gvQuoteItem.Rows[e.RowIndex];
                Label lblProcessid = row.FindControl("lblProcessid") as Label;
                if (lblProcessid.Text != "")
                {
                    if (dtQuoteItem.Rows.Count > 0)
                    {
                        foreach (DataRow iRow in dtQuoteItem.Select("ProcessID='" + lblProcessid.Text + "'"))
                        {
                            //hdnTotalAmount.Value = Math.Round(Convert.ToDecimal(hdnTotalAmount.Value) - Convert.ToDecimal(iRow["TotalPrice"].ToString())).ToString("0.00");
                            iRow.Delete();
                        }
                        dtQuoteItem.AcceptChanges();
                    }
                    Bind_gvOrderItem(dtQuoteItem);
                    rdbQuoteType.Enabled = true;
                    //if (rdbTaxType.SelectedItem.Text == "CST")
                    //    ScriptManager.RegisterStartupScript(Page, GetType(), "JChkCST7", "chk_CSTshow('block');", true);
                    //else if (rdbTaxType.SelectedItem.Text == "VAT")
                    //    ScriptManager.RegisterStartupScript(Page, GetType(), "JChkVAT7", "chk_VATshow('none');", true);
                    rdbQuoteType.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvQuoteItem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                Bind_QuoteMasterDetails();
                gvQuoteItem.EditIndex = e.NewEditIndex;
                Bind_gvOrderItem(ViewState["dtQuoteItem"] as DataTable);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        protected void gvQuoteItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvQuoteItem.Rows[e.RowIndex];
                Label lblProcessid = row.FindControl("lblProcessid") as Label;
                TextBox txtItemQty = row.FindControl("txtItemQty") as TextBox;
                TextBox txtEditQuoteDisc = row.FindControl("txtEditQuoteDisc") as TextBox;
                Label lblListPrice = row.FindControl("lblListPrice") as Label;

                if ((txtItemQty.Text != "" && Convert.ToDecimal(txtItemQty.Text) > 0) && txtEditQuoteDisc.Text != "")
                {
                    decimal decBasicPrice = Convert.ToDecimal(txtEditQuoteDisc.Text) > 0 ?
                       Math.Round((Convert.ToDecimal(lblListPrice.Text) - (Convert.ToDecimal(lblListPrice.Text) * (Convert.ToDecimal(txtEditQuoteDisc.Text) / 100)))) :
                        Convert.ToDecimal(lblListPrice.Text);
                    SqlParameter[] spQUpd = new SqlParameter[] { 
                        new SqlParameter("@QAcYear", Request["qac"].ToString()), 
                        new SqlParameter("@QRefNo", Request["qref"].ToString()), 
                        new SqlParameter("@ProcessID", lblProcessid.Text), 
                        new SqlParameter("@Qty", Convert.ToInt32(txtItemQty.Text)), 
                        new SqlParameter("@disc", Convert.ToDecimal(txtEditQuoteDisc.Text)), 
                        new SqlParameter("@basicprice", decBasicPrice) 
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_upd_QuoteItemDetails", spQUpd);
                }
                gvQuoteItem.EditIndex = -1;

                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@QAcYear", Request["qac"].ToString());
                sp2[1] = new SqlParameter("@QRefNo", Request["qref"].ToString());

                DataTable dtQItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_quote_itemdetails", sp2, DataAccess.Return_Type.DataTable);
                if (dtQItems.Rows.Count > 0)
                    Bind_gvOrderItem(dtQItems);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvQuoteItem_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gvQuoteItem.EditIndex = -1;
                Bind_gvOrderItem(ViewState["dtQuoteItem"] as DataTable);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnQuoteSave_Click(object sender, EventArgs e)
        {
            try
            {
                int insResp = Bind_QuoteMasterDetails();
                DataTable dtQuoteItem = ViewState["dtQuoteItem"] as DataTable;
                if (dtQuoteItem.Rows.Count > 0 && insResp > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Jalert1", "alert('Quote saved.');", true);
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnQuotePrepareSend_Click(object sender, EventArgs e)
        {
            try
            {
                int insResp = Bind_QuoteMasterDetails();
                DataTable dtQuoteItem = ViewState["dtQuoteItem"] as DataTable;
                if (dtQuoteItem.Rows.Count > 0 && insResp > 0)
                {
                    string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                    if (!Directory.Exists(serverURL + "/quote/"))
                        Directory.CreateDirectory(serverURL + "/quote/");
                    string path = serverURL + "/quote/";
                    string sPathToWritePdfTo = path + lblQuoteAcYear.Text + "_" + lblQuoteRefNo.Text + "_" + lblQuoteReviseCount.Text + ".pdf";

                    Document document = new Document(PageSize.A4, 18f, 2f, 30f, 20f);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sPathToWritePdfTo, FileMode.Create));
                    writer.PageEvent = new PDFWriterEvents("QUOTATION");
                    document.Open();
                    document.Add(Build_QuotationDetails(document, "QUOTATION"));
                    document.Close();

                    try
                    {
                        string mailConcat = string.Empty;
                        mailConcat += "Dear Customer,<br/><br/>";
                        mailConcat += "     We trust you find this offer competitive &<br/>";
                        mailConcat += "     look forward to receiving your valuable order.<br/><br/>";
                        string strQtype = rdbTYPECustomer.SelectedItem.Text == "NEW CUSTOMER" ? "new" : "exist";
                        if (Request.Url.Host.ToLower() == "www.suntws.com")
                            mailConcat += "<a href=\"http://" + Request.Url.Host.ToLower() + "" + ConfigurationManager.AppSettings["virdir"] + "quotecustdetailsentry.aspx?acy=" + lblQuoteAcYear.Text + "&acref=" + lblQuoteRefNo.Text + "&qtype=" + strQtype + "\">CLICK HERE TO CONFIRM THIS ORDER</a>";
                        else
                            mailConcat += "<a href=\"http://" + Request.Url.Host + ":" + Request.Url.Port + "/quotecustdetailsentry.aspx?acy=" + lblQuoteAcYear.Text + "&acref=" + lblQuoteRefNo.Text + "&qtype=" + strQtype + "\">CLICK HERE TO CONFIRM THIS ORDER</a>";

                        string strMailID = txtQuoteEmail.Text;
                        if (!string.IsNullOrEmpty(strMailID))
                        {
                            YmailSender es = new YmailSender();
                            es.From = "s-cots_domestic@sun-tws.com";
                            es.To = strMailID;
                            string strCC = string.Empty;
                            if (txtQuoteCC.Text != "")
                                strCC = "," + txtQuoteCC.Text;
                            es.CC = Request.Cookies["TTSUserEmail"].Value + strCC;
                            es.Password = "Y4K/HsD1";
                            es.Subject = "QUOTATION: " + lblQuoteAcYear.Text + "/" + lblQuoteRefNo.Text;
                            es.Body = mailConcat + "<br/><br/><br/><span style='color:#d34639;'>This is a system generated mail. Please do not reply to this email ID.</span>";
                            es.AttachFile = sPathToWritePdfTo;
                            es.IsHtmlBody = true;
                            es.EmailProvider = YmailSender.EmailProviderType.Gmail;
                            es.Send();
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Quote Mail Error: " + ex.Message);
                    }

                    int resp = 0;
                    if (lblQuoteReviseCount.Text == "0")
                    {
                        SqlParameter[] sp1 = new SqlParameter[3];
                        sp1[0] = new SqlParameter("@QAcYear", lblQuoteAcYear.Text);
                        sp1[1] = new SqlParameter("@QRefNo", lblQuoteRefNo.Text);
                        sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                        resp = daCOTS.ExecuteNonQuery_SP("sp_edit_QuoteComplete", sp1);
                    }
                    if (insResp > 0 || resp > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Jalert2", "alert('Quote sent.');", true);
                        Response.Redirect("default.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnQuoteDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@QAcYear", lblQuoteAcYear.Text);
                sp1[1] = new SqlParameter("@QRefNo", lblQuoteRefNo.Text);
                sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_edit_DeleteQuote", sp1);
                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Jalert3", "alert('Quote delete.');", true);
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private PdfPTable Build_QuotationDetails(Document doc, string strFileHead)
        {
            var contentFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = 560f;
            table.LockedWidth = true;
            //                             
            float[] widths = new float[] { 12f, 2f, 16f };
            table.SetWidths(widths);

            Build_AddressChildTable(table);
            Build_PdfHeadingTable(table, strFileHead);
            Build_CustAddressDetailsTable(table);
            Build_QuoteOrderItems(table);
            Build_QuoteBottomDetails(table);

            return table;
        }

        private void Build_AddressChildTable(PdfPTable table)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            string AddressTxt = string.Empty;
            //Registration Address
            var headFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            //Chunk postCell1 = new Chunk("T.S.RAJAM TYRES PRIVATE LIMITED\n", headFont);
            Chunk postCell1 = new Chunk("SUNDARAM INDUSTRIES PRIVATE LIMITED\n", headFont);////*************To Be inserted  On 14-02-2022***********************
            var subHead = FontFactory.GetFont("Arial",6, Font.BOLD);
            Chunk postCell2 = new Chunk("(Formerly Known As T.S.RAJAM TYRES PRIVATE LIMITED)\n", subHead);
            DataRow dtRow = Utilities.Get_PlantAddress("MMN");
            AddressTxt = dtRow["PlantAddress"].ToString().Replace("~", "\n");
            Chunk postCell3 = new Chunk(AddressTxt, titleFont);
            Phrase para = new Phrase();
            para.Add(postCell1);
            para.Add(postCell2);
            para.Add(postCell3);
            PdfPCell cell = new PdfPCell(new Phrase(para));
            cell.ExtraParagraphSpace = 2f;
            PdfPCell nesthousing = new PdfPCell(cell);
            nesthousing.Padding = 0f;
            table.AddCell(nesthousing);
        }

        private void Build_PdfHeadingTable(PdfPTable table, string strInvoiceFileHead)
        {
            PdfPTable nested = new PdfPTable(2);
            PdfPTable nested1 = new PdfPTable(1);
            nested1.TotalWidth = 20f;
            //Heading
            var headFont = FontFactory.GetFont("Arial", 13, Font.BOLD);
            Chunk cellTot = new Chunk("QUOTATION", headFont);
            PdfPCell cell = new PdfPCell(new Phrase(cellTot));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.FixedHeight = 30f;
            nested1.AddCell(cell);

            var titleBoldFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            cell = new PdfPCell(new Phrase("GSTIN No : 33AAGCT6465R2ZJ", titleBoldFont));
            cell.HorizontalAlignment = 0;
            cell.VerticalAlignment = PdfPCell.ALIGN_LEFT;
            cell.ExtraParagraphSpace = 4f;
            nested1.AddCell(cell);

            PdfPCell nesthousing = new PdfPCell(nested1);
            nesthousing.Padding = 0f;
            nested.AddCell(nesthousing);

            string imageFilePath = Server.MapPath("~/images/tvs_suntws.jpg");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageFilePath);
            img.ScaleToFit(165f, 39f);
            PdfPCell cell1 = new PdfPCell(img);
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.Padding = 0f;
            nested.AddCell(cell1);

            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            var valueFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPTable qNoTable = new PdfPTable(3);
            float[] widths = new float[] { 2.5f, 0.5f, 7f };
            qNoTable.SetWidths(widths);
            string strQuoteRefNo = lblQuoteAcYear.Text + "/" + lblQuoteRefNo.Text;
            PdfPCell qNoCell = new PdfPCell(new Phrase("OFFER", titleFont));
            qNoCell.Border = 0;
            qNoTable.AddCell(qNoCell);
            qNoCell = new PdfPCell(new Phrase(":", titleFont));
            qNoCell.Border = 0;
            qNoTable.AddCell(qNoCell);
            qNoCell = new PdfPCell(new Phrase(strQuoteRefNo, valueFont));
            qNoCell.Border = 0;
            qNoTable.AddCell(qNoCell);

            string aa = DateTime.Now.ToShortDateString();
            if (lblQuoteReviseCount.Text != "0")
            {
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@QAcYear", lblQuoteAcYear.Text);
                sp2[1] = new SqlParameter("@QRefNo", lblQuoteRefNo.Text);

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_QuotePreparedDate", sp2, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    aa = dt.Rows[0]["CompletedDate"].ToString();
                }
            }
            qNoCell = new PdfPCell(new Phrase("DATE", titleFont));
            qNoCell.Border = 0;
            qNoTable.AddCell(qNoCell);
            qNoCell = new PdfPCell(new Phrase(":", titleFont));
            qNoCell.Border = 0;
            qNoTable.AddCell(qNoCell);
            qNoCell = new PdfPCell(new Phrase(aa, valueFont));
            qNoCell.Border = 0;
            qNoTable.AddCell(qNoCell);
            nested.AddCell(qNoTable);

            qNoTable = new PdfPTable(3);
            widths = new float[] { 4f, 0.5f, 5f };
            qNoTable.SetWidths(widths);
            if (lblQuoteReviseCount.Text != "0")
            {
                qNoCell = new PdfPCell(new Phrase("REVISED NO.", titleFont));
                qNoCell.Border = 0;
                qNoTable.AddCell(qNoCell);
                qNoCell = new PdfPCell(new Phrase(":", titleFont));
                qNoCell.Border = 0;
                qNoTable.AddCell(qNoCell);
                qNoCell = new PdfPCell(new Phrase(lblQuoteReviseCount.Text, valueFont));
                qNoCell.Border = 0;
                qNoTable.AddCell(qNoCell);

                qNoCell = new PdfPCell(new Phrase("DATE", titleFont));
                qNoCell.Border = 0;
                qNoTable.AddCell(qNoCell);
                qNoCell = new PdfPCell(new Phrase(":", titleFont));
                qNoCell.Border = 0;
                qNoTable.AddCell(qNoCell);
                qNoCell = new PdfPCell(new Phrase(DateTime.Now.ToShortDateString(), valueFont));
                qNoCell.Border = 0;
                qNoTable.AddCell(qNoCell);
                nested.AddCell(qNoTable);
            }
            else
            {
                PdfPCell cellEmpty = new PdfPCell(new Phrase("\n"));
                cellEmpty.Border = 0;
                nested.AddCell(cellEmpty);
            }

            PdfPCell nesthousing1 = new PdfPCell(nested);
            nesthousing1.Padding = 0f;
            nesthousing1.Colspan = 2;
            table.AddCell(nesthousing1);
        }

        private void Build_CustAddressDetailsTable(PdfPTable table)
        {
            try
            {
                var normalFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
                var boldFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
                PdfPTable pdfAddressHeader = new PdfPTable(2);
                PdfPCell cell = new PdfPCell(new Phrase("BILLED TO", boldFont));
                cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                pdfAddressHeader.AddCell(cell);
                cell = new PdfPCell(new Phrase("CONSIGNEE", boldFont));
                cell.HorizontalAlignment = 0;
                pdfAddressHeader.AddCell(cell);

                PdfPCell nesthousingMain = new PdfPCell(pdfAddressHeader);
                nesthousingMain.Padding = 0f;
                nesthousingMain.Colspan = 3;
                table.AddCell(nesthousingMain);

                PdfPTable tblAddress = new PdfPTable(2);
                PdfPTable tblBilledTo = new PdfPTable(3);

                tblBilledTo.TotalWidth = 260f;
                float[] widthsOrder = new float[] { 2f, 0.2f, 6f };
                tblBilledTo.SetWidths(widthsOrder);

                cell = new PdfPCell(new Phrase("NAME", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase("M/S." + txtBillCompanyName.Text + "", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase("ADDRESS", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                string AddressTxt = string.Empty;
                AddressTxt += txtBillAddress.Text.Replace("\r\n", "\n") + "\n";
                AddressTxt += txtBillCity.Text + " - " + txtBillZipCode.Text + "\n";
                cell = new PdfPCell(new Phrase(AddressTxt, boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase("STATE / CODE", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase(txtBillState.Text + " / " + txtBillStateCode.Text, boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase("GSTIN No", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase(txtBillGSTNo.Text, boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase("CONTACT", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblBilledTo.AddCell(cell);

                cell = new PdfPCell(new Phrase(txtBillContactName.Text + " / " + txtBillContactNo.Text, boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblBilledTo.AddCell(cell);

                PdfPCell nesthousing = new PdfPCell(tblBilledTo);
                nesthousing.Padding = 0f;
                tblAddress.AddCell(nesthousing);

                PdfPTable tblConsignee = new PdfPTable(3);
                tblBilledTo.TotalWidth = 260f;
                widthsOrder = new float[] { 2f, 0.2f, 6f };
                tblConsignee.SetWidths(widthsOrder);

                cell = new PdfPCell(new Phrase("NAME", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase("M/S." + txtConsCompanyName.Text + "", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase("ADDRESS", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                AddressTxt = string.Empty;
                AddressTxt += txtConsAddress.Text.Replace("~", "\n") + "\n";
                AddressTxt += txtConsCity.Text + " - " + txtConsZipCode.Text + "\n";
                cell = new PdfPCell(new Phrase(AddressTxt, boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase("STATE / CODE", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase(txtConsState.Text + " / " + txtConsStateCode.Text, boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase("GSTIN No", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase(txtConsGSTNo.Text, boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblConsignee.AddCell(cell);


                cell = new PdfPCell(new Phrase("CONTACT", normalFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase(":", boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell.Border = 0;
                tblConsignee.AddCell(cell);

                cell = new PdfPCell(new Phrase(txtConsContactName.Text + " / " + txtConsContactNo.Text, boldFont));
                cell.HorizontalAlignment = 0;
                cell.VerticalAlignment = Element.ALIGN_TOP;
                cell.Border = Rectangle.NO_BORDER;
                tblConsignee.AddCell(cell);

                nesthousing = new PdfPCell(tblConsignee);
                nesthousing.Padding = 0f;
                tblAddress.AddCell(nesthousing);

                nesthousingMain = new PdfPCell(tblAddress);
                nesthousingMain.Padding = 0f;
                nesthousingMain.Colspan = 3;
                table.AddCell(nesthousingMain);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_QuoteOrderItems(PdfPTable table)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPTable dt = new PdfPTable(10);
            dt.TotalWidth = 560f;
            dt.LockedWidth = true;
            float[] widths = new float[] { 0.9f, 8f, 2.5f, 1.2f, 1.5f, 2f, 2.5f, 2f, 2.8f, 3f };
            dt.SetWidths(widths);

            //Order Items Heading
            dt.AddCell(Build_ItemTableHeadStyle("Sl. No."));
            dt.AddCell(Build_ItemTableHeadStyle("Description of Goods"));
            dt.AddCell(Build_ItemTableHeadStyle("HSN \n Code/SAC"));
            dt.AddCell(Build_ItemTableHeadStyle("Uom"));
            dt.AddCell(Build_ItemTableHeadStyle("Qty"));
            dt.AddCell(Build_ItemTableHeadStyle("Weight\n(per unit)"));
            dt.AddCell(Build_ItemTableHeadStyle("Rate\n(per item)"));
            dt.AddCell(Build_ItemTableHeadStyle("Discount"));
            dt.AddCell(Build_ItemTableHeadStyle("Total\n Weight"));
            dt.AddCell(Build_ItemTableHeadStyle("Taxable Value"));

            //Items Add
            DataTable dtQuoteItem = ViewState["dtQuoteItem"] as DataTable;
            int balanceItem = 12 - dtQuoteItem.Rows.Count;
            int balanceMod = balanceItem / 2;
            for (int k = 1; k < balanceMod; k++)
            {
                PdfPCell cell = new PdfPCell(new Phrase("\n"));
                cell.Border = Rectangle.RIGHT_BORDER;
                dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
                dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
            }
            int j = 1;
            foreach (DataRow row in dtQuoteItem.Rows)
            {
                dt.AddCell(Build_ItemTableListStyle(j.ToString(), 2));
                string strItemType = row["TyreType"].ToString();
                string strType = strItemType.Length > 2 ? (strItemType.Substring(0, 2)) + (strItemType[2].ToString() == "N" ? "N" : "") : strItemType;
                dt.AddCell(Build_ItemTableListStyle(row["Tyresize"].ToString() + "/" + row["RimSize"].ToString() + " " + row["Brand"].ToString() + " " + strType, 0));
                dt.AddCell(Build_ItemTableListStyle("4012.9020", 0));
                dt.AddCell(Build_ItemTableListStyle("Nos", 2));
                dt.AddCell(Build_ItemTableListStyle(row["Qty"].ToString(), 2));
                dt.AddCell(Build_ItemTableListStyle((Convert.ToDecimal(row["FinishedWt"].ToString()) / Convert.ToDecimal(row["Qty"].ToString())).ToString(), 2));
                dt.AddCell(Build_ItemTableListStyle(row["BasicPrice"].ToString(), 2));
                dt.AddCell(Build_ItemTableListStyle("0.0", 2));
                dt.AddCell(Build_ItemTableListStyle(row["FinishedWt"].ToString(), 2));
                dt.AddCell(Build_ItemTableListStyle(row["TotalPrice"].ToString(), 2));
                j++;
            }
            for (int k = dtQuoteItem.Rows.Count + balanceMod; k < 12; k++)
            {
                PdfPCell cell = new PdfPCell(new Phrase("\n"));
                cell.Border = Rectangle.RIGHT_BORDER;
                dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
                dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell); dt.AddCell(cell);
            }
            // total value row
            dt.AddCell(Build_ItemTableBottomStyle("Total", 2, 3));
            dt.AddCell(Build_ItemTableBottomStyle("", 2, 1)); // units - empty
            object sumQty;
            sumQty = dtQuoteItem.Compute("Sum(Qty)", "");
            dt.AddCell(Build_ItemTableBottomStyle(sumQty.ToString(), 2, 1)); // sum of qty
            dt.AddCell(Build_ItemTableBottomStyle("", 2, 1)); // weights - empty
            dt.AddCell(Build_ItemTableBottomStyle(" ", 2, 1)); // rate  - empty
            dt.AddCell(Build_ItemTableBottomStyle(" ", 2, 1)); // discount - empty
            object sumWt;
            sumWt = dtQuoteItem.Compute("Sum(FinishedWt)", "");
            dt.AddCell(Build_ItemTableBottomStyle(sumWt.ToString(), 2, 1)); // sum of total weight
            object sumCost;
            sumCost = dtQuoteItem.Compute("Sum(TotalPrice)", "");
            dt.AddCell(Build_ItemTableBottomStyle(sumCost.ToString(), 2, 1)); // sum of taxable value 

            dt.AddCell(Build_Duties_Taxes(Convert.ToDecimal(sumCost.ToString())));

            PdfPCell nesthousing1 = new PdfPCell(dt);
            nesthousing1.Padding = 0f;
            nesthousing1.Colspan = 3;
            table.AddCell(nesthousing1);
        }

        private PdfPCell Build_Duties_Taxes(decimal totTaxValue)
        {
            PdfPCell cell = new PdfPCell();
            try
            {
                PdfPTable tab2 = new PdfPTable(3);
                float[] widths2 = new float[] { 23.2f, 3.2f, 3.4f };
                tab2.TotalWidth = 560f;
                tab2.SetWidths(widths2);

                SqlParameter[] spdiscount = new SqlParameter[2];
                spdiscount[0] = new SqlParameter("@QRefNo", lblQuoteRefNo.Text);
                spdiscount[1] = new SqlParameter("@QAcYear", lblQuoteAcYear.Text);
                DataTable dtdiscount = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_QuotePrepare_OtherCharge", spdiscount, DataAccess.Return_Type.DataTable);

                for (int i = 0; i < dtdiscount.Rows.Count; i++)
                {
                    tab2.AddCell(Build_TaxesTableListStyle(dtdiscount.Rows[i]["Category"].ToString() + ": " + dtdiscount.Rows[i]["DescriptionDiscount"].ToString(), 2));
                    tab2.AddCell(Build_TaxesTableListStyle("", 2));
                    tab2.AddCell(Build_TaxesTableListStyle(dtdiscount.Rows[i]["Amount"].ToString(), 2));
                }

                decimal txtTOTALCOST1 = totTaxValue;
                for (int i = 0; i < dtdiscount.Rows.Count; i++)
                {
                    if (dtdiscount.Rows[i]["Category"].ToString() == "ADD")
                    {
                        txtTOTALCOST1 += (Convert.ToDecimal(dtdiscount.Rows[i]["Amount"]));
                    }
                    else if (dtdiscount.Rows[i]["Category"].ToString() == "LESS")
                    {
                        txtTOTALCOST1 -= (Convert.ToDecimal(dtdiscount.Rows[i]["Amount"]));
                    }
                }

                tab2.AddCell(Build_TaxesTableBottomListStyle("SUB TOTAL", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                tab2.AddCell(Build_TaxesTableBottomListStyle(txtTOTALCOST1.ToString("0.00"), 2));

                decimal totCGST = 0, totSGST = 0, totIGST = 0;
                //CGST
                decimal cgst = Convert.ToDecimal(txtCGST.Text == "" ? "0" : txtCGST.Text);
                tab2.AddCell(Build_TaxesTableListStyle("CGST", 2));
                tab2.AddCell(Build_TaxesTableListStyle(cgst.ToString() + "%", 2));
                totCGST = Math.Round(txtTOTALCOST1 * (cgst / 100), 0);
                tab2.AddCell(Build_TaxesTableListStyle(totCGST.ToString("0.00"), 2));
                //SGST
                decimal sgst = Convert.ToDecimal(txtSGST.Text == "" ? "0" : txtSGST.Text);
                tab2.AddCell(Build_TaxesTableListStyle("SGST", 2));
                tab2.AddCell(Build_TaxesTableListStyle(sgst.ToString() + "%", 2));
                totSGST = Math.Round(txtTOTALCOST1 * (sgst / 100), 0);
                tab2.AddCell(Build_TaxesTableListStyle(totSGST.ToString("0.00"), 2));
                //IGST
                decimal igst = Convert.ToDecimal(txtIGST.Text == "" ? "0" : txtIGST.Text);
                tab2.AddCell(Build_TaxesTableListStyle("IGST", 2));
                tab2.AddCell(Build_TaxesTableListStyle(igst.ToString() + "%", 2));
                totIGST = Math.Round(txtTOTALCOST1 * (igst / 100), 0);
                tab2.AddCell(Build_TaxesTableListStyle(totIGST.ToString("0.00"), 2));

                tab2.AddCell(Build_TaxesTableBottomListStyle("Total Tax", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                decimal totalTax = totCGST + totSGST + totIGST;
                tab2.AddCell(Build_TaxesTableBottomListStyle(totalTax.ToString("0.00"), 2));

                tab2.AddCell(Build_TaxesTableBottomListStyle("Total Invoice Value (in figure)", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                decimal totalInvoice = totalTax + txtTOTALCOST1;
                tab2.AddCell(Build_TaxesTableBottomListStyle(totalInvoice.ToString("0.00"), 2));

                tab2.AddCell(Build_TaxesTableBottomListStyle("Amount of TaxSubject to Reverse charge", 2));
                tab2.AddCell(Build_TaxesTableListStyle("\n", 1));
                tab2.AddCell(Build_TaxesTableBottomListStyle("0", 2));

                PdfPTable tab3 = new PdfPTable(2);
                tab3.TotalWidth = 560f;
                tab3.LockedWidth = true;
                float[] widths3 = new float[] { 5f, 25f };
                tab3.SetWidths(widths3);

                var titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                Chunk p1 = new Chunk("Total Invoice Value (in words): ", titleFont);
                titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);

                string[] strSplit = totalInvoice.ToString().Split('.');
                int intPart = Convert.ToInt32(strSplit[0].ToString());
                string decimalPart = strSplit.Length > 1 ? strSplit[1].ToString() : "00";
                string text = Utilities.NumberToText(intPart, true, false).ToString();
                if (Convert.ToDecimal(decimalPart) > 0)
                    text += " Point " + Utilities.DecimalToText(decimalPart) + " Only";
                else
                    text += " Only";

                Chunk p2 = new Chunk(text, titleFont);
                Paragraph para = new Paragraph();
                para.Add(p1);
                para.Add(p2);
                PdfPCell cell1 = new PdfPCell(new Phrase(para));
                cell1.HorizontalAlignment = 0;
                cell1.Colspan = 2;
                cell1.VerticalAlignment = PdfPCell.ALIGN_TOP;
                tab3.AddCell(cell1);

                titleFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                p1 = new Chunk("Total Tax Value (in words): ", titleFont);
                titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);

                string[] strSplit1 = totalTax.ToString().Split('.');
                int intPart1 = Convert.ToInt32(strSplit1[0].ToString());
                string decimalPart1 = strSplit1.Length > 1 ? strSplit1[1].ToString() : "00";
                string text1 = Utilities.NumberToText(intPart1, true, false).ToString();
                if (Convert.ToDecimal(decimalPart1) > 0)
                    text1 += " Point " + Utilities.DecimalToText(decimalPart1) + " Only";
                else
                    text1 += " Only";

                p2 = new Chunk(text1, titleFont);
                para = new Paragraph();
                para.Add(p1);
                para.Add(p2);
                cell1 = new PdfPCell(new Phrase(para));
                cell1.HorizontalAlignment = 0;
                cell1.VerticalAlignment = PdfPCell.ALIGN_TOP;
                cell1.Colspan = 2;
                //cell1.FixedHeight = 24f;
                tab3.AddCell(cell1);

                PdfPTable nested = new PdfPTable(1);
                PdfPCell nesthousing = new PdfPCell(tab2);
                nesthousing.Padding = 0f;
                nesthousing.Colspan = 3;
                nested.AddCell(nesthousing);

                PdfPCell nesthousing1 = new PdfPCell(tab3);
                nesthousing1.Padding = 0f;
                nesthousing1.Colspan = 3;
                nested.AddCell(nesthousing1);

                cell = new PdfPCell(nested);
                cell.Padding = 0f;
                cell.Colspan = 10;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PrepareInvoice.cs", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return cell;
        }

        private void Build_QuoteBottomDetails(PdfPTable table)
        {
            PdfPTable tab3 = new PdfPTable(2);
            tab3.TotalWidth = 560f;
            tab3.LockedWidth = true;
            float[] widths3 = new float[] { 4f, 26f };
            tab3.SetWidths(widths3);

            PdfPTable termsTd = new PdfPTable(1);
            termsTd.TotalWidth = 560f;
            termsTd.LockedWidth = true;
            float[] widths = new float[] { 30f };
            termsTd.SetWidths(widths);
            var titleFont = FontFactory.GetFont("Arial", 9, Font.UNDERLINE);
            Chunk p1 = new Chunk("TERMS & CONDITION\n\n", titleFont);

            titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            Chunk p2 = new Chunk(txtQuoteTerms.Text.Replace("\r\n", "\n") + "\n", titleFont);
            Paragraph para = new Paragraph();
            para.Add(p1);
            para.Add(p2);
            PdfPCell cell = new PdfPCell(para);
            termsTd.AddCell(cell);

            var titleFont1 = FontFactory.GetFont("Arial", 9, Font.BOLD);
            string strBankDet = string.Empty;
            //strBankDet += "   BANK NAME : STATE BANK OF INDIA                ACCOUNT TYPE : CURRENT ACCOUNT                ACCOUNT NO   : 40582959714\n\n";
            //strBankDet += "   IFSC CODE   : SBIN0009999                                  BRANCH NAME  : CAG BRANCH, CHENNAI.\n";
            strBankDet += "   BANK NAME : STATE BANK OF INDIA                ACCOUNT TYPE : CASH CREDIT                 ACCOUNT NO   : 40776755597\n\n";
            strBankDet += "   IFSC CODE   : SBIN0011933                               BRANCH NAME  : SME MARAIMALAINAGAR\n";
            p1 = new Chunk(strBankDet, titleFont1);
            para = new Paragraph();
            para.Add(p1);
            para.Alignment = Element.ALIGN_LEFT;
            PdfPCell cell1 = new PdfPCell(para);
            cell1.Padding = 0f;
            cell1.Colspan = 3;
            cell1.FixedHeight = 30f;
            termsTd.AddCell(cell1);

            //p1 = new Chunk("T.S.Rajam Tyres Private Limited\n", titleFont);
            p1 = new Chunk("Sundaram Industries Private Limited (Formerly Known as T.S.Rajam Tyres Private Limited) \n", titleFont);//*************To Be inserted  On 14-02-2022***********************
            titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            string strRegAdd = string.Empty;
            //strRegAdd += "Regd Office: ‘TVS Building’, 7-B, West veli street, Madurai – 625001.\n";
            strRegAdd += "Regd Office: No.10, Jawahar Road, Chokki Kulam, Madurai, Tamil Nadu, India - 625002.\n";
            //strRegAdd += "\n";
            strRegAdd += "CIN: U51901TN2018PTC121156         Email: crm@sun-tws.com         Website: www.sun-tws.com         Tel: 044-45504157";
            p2 = new Chunk(strRegAdd, titleFont);
            para = new Paragraph();
            para.Add(p1);
            para.Add(p2);
            cell = new PdfPCell(new Phrase(para));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.Padding = 0f;
            cell.Colspan = 3;
            cell.FixedHeight = 30f;
            termsTd.AddCell(cell);

            PdfPCell nesthousingTerms = new PdfPCell(termsTd);
            nesthousingTerms.Padding = 0f;
            nesthousingTerms.Colspan = 2;
            tab3.AddCell(nesthousingTerms);

            PdfPCell nesthousing1 = new PdfPCell(tab3);
            nesthousing1.Padding = 0f;
            nesthousing1.Colspan = 3;
            table.AddCell(nesthousing1);
        }

        private PdfPCell Build_ItemTableHeadStyle(string strText)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = 1;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }

        private PdfPCell Build_ItemTableListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Border = Rectangle.RIGHT_BORDER;
            return cell;
        }

        private PdfPCell Build_ItemTableBottomStyle(string strText, int align, int colspan)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            return cell;
        }

        private PdfPCell Build_TaxesTableListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            return cell;
        }

        private PdfPCell Build_TaxesTableBottomListStyle(string strText, int align)
        {
            var titleFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
            PdfPCell cell = new PdfPCell(new Phrase(strText, titleFont));
            cell.HorizontalAlignment = align;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.FixedHeight = 18f;
            return cell;
        }

        private int Bind_QuoteMasterDetails()
        {
            int resp = 0;
            try
            {
                string strAcYear = Build_QuoteYear();
                SqlParameter[] sp1 = new SqlParameter[29];
                sp1[0] = new SqlParameter("@QAcYear", strAcYear);
                sp1[1] = new SqlParameter("@QRefNo", lblQuoteRefNo.Text != "" ? lblQuoteRefNo.Text.Replace(strAcYear, "") : "");
                sp1[2] = new SqlParameter("@QCustomer", txtQuoteCustomer.Text != "" ? txtQuoteCustomer.Text : hdnFullName.Value);
                sp1[3] = new SqlParameter("@QEmail", txtQuoteEmail.Text);
                sp1[4] = new SqlParameter("@QPerson", txtBillContactName.Text);
                sp1[5] = new SqlParameter("@QPhoneNo", txtBillContactNo.Text);
                sp1[6] = new SqlParameter("@QCustType", rdbQuoteType.SelectedItem.Text);
                sp1[7] = new SqlParameter("@QExcisePer", Convert.ToDecimal("0.00"));
                sp1[8] = new SqlParameter("@QEducationPer", Convert.ToDecimal("0.00"));
                sp1[9] = new SqlParameter("@QHigherPer", Convert.ToDecimal("0.00"));
                sp1[10] = new SqlParameter("@QServiceTax", String.Empty);
                sp1[11] = new SqlParameter("@QTaxPer", Convert.ToDecimal("0.00"));
                sp1[12] = new SqlParameter("@QCstAgainst", String.Empty);
                sp1[13] = new SqlParameter("@QTerms", txtQuoteTerms.Text.Replace("\r\n", "~"));
                sp1[14] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp1[15] = new SqlParameter("@ExpiredDay", Convert.ToInt32(30));
                sp1[16] = new SqlParameter("@QCC", txtQuoteCC.Text);
                sp1[17] = new SqlParameter("@QGrade", rdbQuoteGrade.SelectedItem.Text);
                sp1[18] = new SqlParameter("@QMaxDisc", hdnMaxQuoteDiscount.Value != "" ? Convert.ToDecimal(hdnMaxQuoteDiscount.Value) : Convert.ToDecimal("0"));
                sp1[19] = new SqlParameter("@Customer", rdbTYPECustomer.SelectedItem.Text);
                sp1[20] = new SqlParameter("@CGST", chkCGST.Checked == true ? Convert.ToDecimal(txtCGST.Text) : Convert.ToDecimal("0.00"));
                sp1[21] = new SqlParameter("@SGST", chkSGST.Checked == true ? Convert.ToDecimal(txtSGST.Text) : Convert.ToDecimal("0.00"));
                sp1[22] = new SqlParameter("@IGST", chkIGST.Checked == true ? Convert.ToDecimal(txtIGST.Text) : Convert.ToDecimal("0.00"));
                sp1[23] = new SqlParameter("@CGSTVal", chkCGST.Checked == true ? Convert.ToDecimal(hdnCGSTVal.Value) : Convert.ToDecimal("0.00"));
                sp1[24] = new SqlParameter("@SGSTVal", chkSGST.Checked == true ? Convert.ToDecimal(hdnSGSTVal.Value) : Convert.ToDecimal("0.00"));
                sp1[25] = new SqlParameter("@IGSTVal", chkIGST.Checked == true ? Convert.ToDecimal(hdnIGSTVal.Value) : Convert.ToDecimal("0.00"));
                if (rdbTYPECustomer.Text == "NEW CUSTOMER")
                {
                    sp1[26] = new SqlParameter("@USERID", "");
                    sp1[27] = new SqlParameter("@billID", "0");
                    sp1[28] = new SqlParameter("@shipID", "0");
                }
                else if (rdbTYPECustomer.Text == "EXISTING CUSTOMER")
                {
                    sp1[26] = new SqlParameter("@USERID", ddlUsername.Items.Count == 0 ? "" : ddlUsername.SelectedValue);
                    sp1[27] = new SqlParameter("@billID", ddlBillingAddress.SelectedItem.Value);
                    sp1[28] = new SqlParameter("@shipID", ddlShippingAddress.SelectedItem.Value);
                }

                resp = daCOTS.ExecuteNonQuery_SP("SP_INS_QuotationPrepare_QuoteDetails", sp1);

                SqlParameter[] sp2 = new SqlParameter[5];
                sp2[0] = new SqlParameter("@QCustomer", txtQuoteCustomer.Text != "" ? txtQuoteCustomer.Text : hdnFullName.Value);
                sp2[1] = new SqlParameter("@QEmail", txtQuoteEmail.Text);
                sp2[2] = new SqlParameter("@QPerson", txtBillContactName.Text);
                sp2[3] = new SqlParameter("@QPhoneNo", txtBillContactNo.Text);
                sp2[4] = new SqlParameter("@QCustType", rdbQuoteType.SelectedItem.Text);

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_QuoteRefno", sp2, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    lblQuoteAcYear.Text = dt.Rows[0]["QAcYear"].ToString();
                    lblQuoteRefNo.Text = dt.Rows[0]["QRefNo"].ToString();
                    lblQuoteReviseCount.Text = dt.Rows[0]["QRevisedCount"].ToString();

                    DataTable dtQuoteItem = ViewState["dtQuoteItem"] as DataTable;
                    SqlParameter[] sp3 = new SqlParameter[3];
                    sp3[0] = new SqlParameter("@quoteitems_DT", dtQuoteItem);
                    sp3[1] = new SqlParameter("@QAcYear", lblQuoteAcYear.Text);
                    sp3[2] = new SqlParameter("@QRefNo", lblQuoteRefNo.Text);

                    daCOTS.ExecuteNonQuery_SP("sp_ins_QuoteItemDetails_DT", sp3);

                    AddtionalCharge(lblQuoteAcYear.Text, Convert.ToInt32(lblQuoteRefNo.Text));
                }

                if (rdbTYPECustomer.Text == "NEW CUSTOMER")
                {
                    // INSERTing customer address Details
                    SqlParameter[] spAddress = new SqlParameter[]{
                        new SqlParameter("@AcYear",lblQuoteAcYear.Text),
                        new SqlParameter("@RefNo",lblQuoteRefNo.Text),
                        new SqlParameter("@CustFullName", txtQuoteCustomer.Text),
                        new SqlParameter("@BillCompanyName", txtBillCompanyName.Text),
                        new SqlParameter("@BillAddress", txtBillAddress.Text),
                        new SqlParameter("@BillCity", txtBillCity.Text),
                        new SqlParameter("@BillStateName", txtBillState.Text),
                        new SqlParameter("@BillStateCode", txtBillStateCode.Text),
                        new SqlParameter("@BillGSTNo", txtBillGSTNo.Text),
                        new SqlParameter("@BillZipCode", txtBillZipCode.Text),
                        new SqlParameter("@BillContactName", txtBillContactName.Text),
                        new SqlParameter("@BillContactNo", txtBillContactNo.Text),
                        new SqlParameter("@ShipCompanyName", txtConsCompanyName.Text),
                        new SqlParameter("@ShipAddress", txtConsAddress.Text),
                        new SqlParameter("@ShipCity", txtConsCity.Text),
                        new SqlParameter("@ShipStateName", txtConsState.Text),
                        new SqlParameter("@ShipStateCode", txtConsStateCode.Text),
                        new SqlParameter("@ShipGSTNo", txtConsGSTNo.Text),
                        new SqlParameter("@ShipZipCode", txtConsZipCode.Text),
                        new SqlParameter("@ShipContactName", txtConsContactName.Text),
                        new SqlParameter("@ShipContactNo", txtConsContactNo.Text)
                    };
                    resp = daCOTS.ExecuteNonQuery_SP("SP_SAV_QuoteAddressDetails", spAddress);
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return resp;
        }

        private string Build_QuoteYear()
        {
            string acYearFormat = string.Empty;
            if (DateTime.Today.Month < 4)
            {
                string nextyear = DateTime.Today.Year.ToString();
                acYearFormat = (DateTime.Today.Year - 1).ToString() + '-' + nextyear.Substring(2, 2);
            }
            else if (DateTime.Today.Month > 3)
            {
                string nextyear = (DateTime.Today.Year + 1).ToString();
                acYearFormat = DateTime.Today.Year.ToString() + '-' + nextyear.Substring(2, 2);
            }
            return acYearFormat;
        }

        protected void lnkQuoteFile_click(object sender, EventArgs e)
        {
            LinkButton lnkTxt = sender as LinkButton;
            string serverUrl = Server.MapPath("~/quote/").Replace("TTS", "pdfs");
            string path = serverUrl + "/" + lnkTxt.Text;

            //Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
            Response.WriteFile(path);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void rbdTYPECustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                rbdTypeCustomer(rdbTYPECustomer.SelectedItem.Text);
                Bind_DomesticCustomer();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void rbdTypeCustomer(string pa)
        {
            try
            {
                txtQuoteCustomer.Text = "";
                txtQuoteEmail.Text = "";
                txtQuoteCC.Text = "";
                //hdnTotalAmount.Value = "0";
                txtBillContactName.Text = "";
                txtBillContactNo.Text = "";
                txtBillCompanyName.Text = "";
                txtBillAddress.Text = "";
                txtBillCity.Text = "";
                txtBillZipCode.Text = "";
                txtBillStateCode.Text = "";
                txtBillState.Text = "";
                txtBillGSTNo.Text = "";
                txtConsContactName.Text = "";
                txtConsContactNo.Text = "";
                txtConsCompanyName.Text = "";
                txtConsAddress.Text = "";
                txtConsCity.Text = "";
                txtConsZipCode.Text = "";
                txtConsStateCode.Text = "";
                txtConsState.Text = "";
                txtConsGSTNo.Text = "";
                txtClaimAdjustment.Text = "";
                txtotherdiscount.Text = "";
                txtOtherDisAmt.Text = "";
                txtLESSAMT.Text = "";

                if (pa == "NEW CUSTOMER")
                {
                    txtQuoteCustomer.Visible = true;
                    ddlQuotecustomer.Visible = false;
                    ddlUsername.Visible = false;
                    lblUsername.Visible = false;
                    rdbQuoteGrade.SelectedIndex = -1;
                    rdbQuoteType.Enabled = true;
                    ddlBillingAddress.Visible = false;
                    ddlShippingAddress.Visible = false;

                    txtBillContactName.Enabled = true;
                    txtBillContactNo.Enabled = true;
                    txtBillCompanyName.Enabled = true;
                    txtBillAddress.Enabled = true;
                    txtBillCity.Enabled = true;
                    txtBillZipCode.Enabled = true;
                    txtBillStateCode.Enabled = true;
                    txtBillState.Enabled = true;
                    txtBillGSTNo.Enabled = true;
                    txtConsContactName.Enabled = true;
                    txtConsContactNo.Enabled = true;
                    txtConsCompanyName.Enabled = true;
                    txtConsAddress.Enabled = true;
                    txtConsCity.Enabled = true;
                    txtConsZipCode.Enabled = true;
                    txtConsStateCode.Enabled = true;
                    txtConsState.Enabled = true;
                    txtConsGSTNo.Enabled = true;

                }
                else if (pa == "EXISTING CUSTOMER")
                {
                    txtQuoteCustomer.Visible = false;
                    ddlQuotecustomer.Visible = true;
                    ddlUsername.Visible = true;
                    lblUsername.Visible = true;
                    rdbQuoteGrade.SelectedIndex = -1;
                    ddlUsername.Items.Insert(0, "Choose");
                    ddlUsername.SelectedIndex = 0;
                    ddlQuotecustomer.Items.Insert(0, "Choose");
                    ddlQuotecustomer.SelectedIndex = 0;
                    rdbQuoteType.Enabled = false;
                    ddlBillingAddress.Visible = true;
                    ddlShippingAddress.Visible = true;

                    txtBillContactName.Enabled = false;
                    txtBillContactNo.Enabled = false;
                    txtBillCompanyName.Enabled = false;
                    txtBillAddress.Enabled = false;
                    txtBillCity.Enabled = false;
                    txtBillZipCode.Enabled = false;
                    txtBillStateCode.Enabled = false;
                    txtBillState.Enabled = false;
                    txtBillGSTNo.Enabled = false;
                    txtConsContactName.Enabled = false;
                    txtConsContactNo.Enabled = false;
                    txtConsCompanyName.Enabled = false;
                    txtConsAddress.Enabled = false;
                    txtConsCity.Enabled = false;
                    txtConsZipCode.Enabled = false;
                    txtConsStateCode.Enabled = false;
                    txtConsState.Enabled = false;
                    txtConsGSTNo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_DomesticCustomer()
        {
            try
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
                    ddlQuotecustomer.DataSource = dtCustList;
                    ddlQuotecustomer.DataTextField = "custfullname";
                    ddlQuotecustomer.DataValueField = "custcode";
                    ddlQuotecustomer.DataBind();
                }
                ddlQuotecustomer.Items.Insert(0, "Choose");
                ddlQuotecustomer.Text = "Choose";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlQuotecustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlQuotecustomer")
                {
                    if (rdbQuoteGrade.SelectedIndex == -1)
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please Select Grade');", true);
                    else
                    {
                        System.Web.UI.WebControls.ListItem selectedListItem = ddlQuotecustomer.Items.FindByText("" + hdnFullName.Value + "");
                        if (selectedListItem != null)
                        {
                            ddlQuotecustomer.Items.FindByText(ddlQuotecustomer.SelectedItem.Text).Selected = false;
                            selectedListItem.Selected = true;
                        }
                        SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddlQuotecustomer.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddlQuotecustomer.SelectedItem.Value) 
                    };
                        DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                        if (dtUserList.Rows.Count > 0)
                        {
                            ddlUsername.DataSource = dtUserList;
                            ddlUsername.DataTextField = "username";
                            ddlUsername.DataValueField = "ID";
                            ddlUsername.DataBind();
                            if (dtUserList.Rows.Count == 1)
                                ddlUsername_SelectedIndexChanged(sender, e);
                            else
                            {
                                ddlUsername.Items.Insert(0, "Choose");
                                CtrlClear(4);
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

        protected void ddlUsername_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Web.UI.WebControls.ListItem selectedListItem = ddlUsername.Items.FindByValue("" + hdnUsername.Value + "");
                if (selectedListItem != null)
                {
                    ddlUsername.Items.FindByText(ddlUsername.SelectedItem.Text).Selected = false;
                    selectedListItem.Selected = true;
                }
                System.Web.UI.WebControls.ListItem selectedItem = ddlQuotecustomer.Items.FindByText("" + hdnFullName.Value + "");
                if (selectedItem != null)
                {
                    ddlQuotecustomer.Items.FindByText(ddlQuotecustomer.SelectedItem.Text).Selected = false;
                    selectedItem.Selected = true;
                }

                if (rdbTYPECustomer.Text == "NEW CUSTOMER")
                    rdbQuoteType_IndexChange(sender, e);
                else if (rdbTYPECustomer.Text == "EXISTING CUSTOMER")
                {
                    ddlBillingAddress.DataSource = "";
                    ddlBillingAddress.DataBind();
                    DataTable dtBillAddress = new DataTable();
                    SqlParameter[] sp1 = new SqlParameter[1];

                    if (Request["qac"] != null && Request["qref"] != null)
                    {
                        sp1[0] = new SqlParameter("@custcode", lblDUserName.Text);
                        dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList", sp1, DataAccess.Return_Type.DataTable);
                    }
                    else if (ddlUsername.SelectedItem.Text != "Choose")
                    {
                        sp1[0] = new SqlParameter("@custcode", ddlUsername.SelectedItem.Value);
                        dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList", sp1, DataAccess.Return_Type.DataTable);
                    }

                    if (dtBillAddress.Rows.Count > 0)
                    {
                        ddlBillingAddress.DataSource = dtBillAddress;
                        ddlBillingAddress.DataTextField = "ShipAddress";
                        ddlBillingAddress.DataValueField = "shipid";
                        ddlBillingAddress.DataBind();
                        if (dtBillAddress.Rows.Count == 1)
                            ddlBillingAddress_IndexChange(sender, e);
                        else
                        {
                            ddlBillingAddress.Items.Insert(0, "Choose");
                            CtrlClear(3);
                        }
                    }
                }

                SqlParameter[] sp2 = new SqlParameter[1];
                if (Request["qac"] != null && Request["qref"] != null)
                    sp2[0] = new SqlParameter("@custCode", lblDUserName.Text);
                else
                    sp2[0] = new SqlParameter("@custCode", ddlUsername.SelectedItem.Value);
                DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_QutaPreExsCust", sp2, DataAccess.Return_Type.DataTable);
                if (dtUserList.Rows.Count > 0)
                {
                    txtQuoteEmail.Text = dtUserList.Rows[0]["EmailID"].ToString();
                    txtBillContactName.Text = dtUserList.Rows[0]["contact_name"].ToString();
                    txtBillContactNo.Text = dtUserList.Rows[0]["mobile"].ToString();
                    System.Web.UI.WebControls.ListItem selecteditem = rdbQuoteType.Items.FindByText(dtUserList.Rows[0]["CustCategory"].ToString());
                    if (selecteditem != null)
                    {
                        if (rdbQuoteType.SelectedIndex != -1)
                            rdbQuoteType.Items.FindByText(rdbQuoteType.SelectedItem.Text).Selected = false;
                        selecteditem.Selected = true;
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
                SqlParameter[] sp2 = new SqlParameter[2];
                if (Request["qac"] != null && Request["qref"] != null)
                    sp2[0] = new SqlParameter("@custcode", lblDUserName.Text);
                else
                    sp2[0] = new SqlParameter("@custcode", ddlUsername.SelectedItem.Value);
                sp2[1] = new SqlParameter("@shipid", Convert.ToInt32(ddlBillingAddress.SelectedItem.Value));
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_QuotationPrepare_Address", sp2, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    txtBillCompanyName.Text = dt.Rows[0]["name"].ToString();
                    txtBillAddress.Text = HttpUtility.HtmlDecode(dt.Rows[0]["Address"].ToString().Replace("~", "\r\n"));
                    txtBillCity.Text = dt.Rows[0]["city"].ToString();
                    txtBillContactName.Text = dt.Rows[0]["contact_name"].ToString();
                    txtBillContactNo.Text = dt.Rows[0]["mobile"].ToString();
                    txtBillGSTNo.Text = dt.Rows[0]["GSTIN No"].ToString();
                    txtBillState.Text = dt.Rows[0]["State"].ToString();
                    txtBillStateCode.Text = dt.Rows[0]["State Code"].ToString();
                    txtBillZipCode.Text = dt.Rows[0]["zipcode"].ToString();
                }

                ddlShippingAddress.DataSource = "";
                ddlShippingAddress.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@BillID", Convert.ToInt32(ddlBillingAddress.SelectedItem.Value)) };

                DataTable dtShippAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ShipList_BillID", sp1, DataAccess.Return_Type.DataTable);
                if (dtShippAddress.Rows.Count > 0)
                {
                    ddlShippingAddress.DataSource = dtShippAddress;
                    ddlShippingAddress.DataTextField = "ShipAddress";
                    ddlShippingAddress.DataValueField = "shipid";
                    ddlShippingAddress.DataBind();
                    if (dtShippAddress.Rows.Count == 1)
                        ddlShippingAddress_IndexChange(sender, e);
                    else
                    {
                        ddlShippingAddress.Items.Insert(0, "Choose");
                        CtrlClear(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlShippingAddress_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[2];
                if (Request["qac"] != null && Request["qref"] != null)
                    sp2[0] = new SqlParameter("@custcode", lblDUserName.Text);
                else
                    sp2[0] = new SqlParameter("@custcode", ddlUsername.SelectedItem.Value);
                sp2[1] = new SqlParameter("@shipid", Convert.ToInt32(ddlShippingAddress.SelectedItem.Value));
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_QuotationPrepare_Address", sp2, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    txtConsCompanyName.Text = dt.Rows[0]["Name"].ToString();
                    txtConsAddress.Text = HttpUtility.HtmlDecode(dt.Rows[0]["Address"].ToString().Replace("~", "\r\n"));
                    txtConsCity.Text = dt.Rows[0]["city"].ToString();
                    txtConsContactName.Text = dt.Rows[0]["contact_name"].ToString();
                    txtConsContactNo.Text = dt.Rows[0]["mobile"].ToString();
                    txtConsGSTNo.Text = dt.Rows[0]["GSTIN No"].ToString();
                    txtConsState.Text = dt.Rows[0]["State"].ToString();
                    txtConsStateCode.Text = dt.Rows[0]["State Code"].ToString();
                    txtConsZipCode.Text = dt.Rows[0]["zipcode"].ToString();
                }

                SqlParameter[] sp1 = new SqlParameter[2];
                if (Request["qac"] != null && Request["qref"] != null)
                    sp1[0] = new SqlParameter("@custcode", lblDUserName.Text);
                else
                    sp1[0] = new SqlParameter("@custcode", ddlUsername.SelectedItem.Value);
                sp1[1] = new SqlParameter("@ShipID", Convert.ToInt32(ddlShippingAddress.SelectedItem.Value));
                DataTable dtPer = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_CotsManualOrder_GST", sp1, DataAccess.Return_Type.DataTable);
                {
                    txtCGST.Text = dtPer.Rows[0]["CGST"].ToString().Trim() == "" ? "0.00" : dtPer.Rows[0]["CGST"].ToString().Trim();
                    txtSGST.Text = dtPer.Rows[0]["SGST"].ToString().Trim() == "" ? "0.00" : dtPer.Rows[0]["SGST"].ToString().Trim();
                    txtIGST.Text = dtPer.Rows[0]["IGST"].ToString().Trim() == "" ? "0.00" : dtPer.Rows[0]["IGST"].ToString().Trim();
                }

                rdbQuoteType_IndexChange(sender, e);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void CtrlClear(int ToInt)
        {
            for (int i = 1; i <= ToInt; i++)
            {
                if (i == 4)
                {
                    ddlBillingAddress.DataSource = "";
                    ddlBillingAddress.DataBind();
                }
                if (i == 3)
                {
                    txtBillCompanyName.Text = "";
                    txtBillAddress.Text = "";
                    txtBillCity.Text = "";
                    txtBillZipCode.Text = "";
                    txtBillState.Text = "";
                    txtBillStateCode.Text = "";
                    txtBillGSTNo.Text = "";
                    txtBillContactName.Text = "";
                    txtBillContactNo.Text = "";
                }
                if (i == 2)
                {
                    ddlShippingAddress.DataSource = "";
                    ddlShippingAddress.DataBind();
                }
                if (i == 1)
                {
                    txtConsCompanyName.Text = "";
                    txtConsAddress.Text = "";
                    txtConsCity.Text = "";
                    txtConsZipCode.Text = "";
                    txtConsState.Text = "";
                    txtConsStateCode.Text = "";
                    txtConsGSTNo.Text = "";
                    txtConsContactName.Text = "";
                    txtConsContactNo.Text = "";
                }
            }
        }

        private void AddtionalCharge(string quoteAcYear, int quoteRefNo)
        {
            try
            {
                SqlParameter[] spdel = new SqlParameter[2];
                spdel[0] = new SqlParameter("@QAcYear", quoteAcYear);
                spdel[1] = new SqlParameter("@QRefNo", quoteRefNo);
                daCOTS.ExecuteNonQuery_SP("SP_UPD_QuoteDiscountStatus", spdel);

                for (int i = 0; i < gvAmountSub.Rows.Count; i++)
                {
                    string DESC = ((TextBox)gvAmountSub.Rows[i].FindControl("txtAddDesc")).Text.Trim();
                    string AMT = ((TextBox)gvAmountSub.Rows[i].FindControl("txtCAddAmt")).Text.Trim();
                    string ddlCalcType = ((DropDownList)gvAmountSub.Rows[i].FindControl("ddlCalcType")).SelectedItem.Value.Trim();
                    if (DESC != "" && AMT != "")
                    {
                        SqlParameter[] sp2 = new SqlParameter[7];
                        sp2[0] = new SqlParameter("@QAcYear", quoteAcYear);
                        sp2[1] = new SqlParameter("@QRefNo", quoteRefNo);
                        sp2[2] = new SqlParameter("@Category", ddlCalcType);
                        sp2[3] = new SqlParameter("@DescriptionDiscount", DESC);
                        sp2[4] = new SqlParameter("@Amount", AMT);
                        sp2[5] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                        sp2[6] = new SqlParameter("@discountType", "OTHER CHARGES");
                        int resp = daCOTS.ExecuteNonQuery_SP("SP_INS_QuoteOtherCharges", sp2);
                    }
                }
                if (txtClaimAdjustment.Text != "" && txtLESSAMT.Text != "")
                {
                    SqlParameter[] sp12 = new SqlParameter[7];
                    sp12[0] = new SqlParameter("@QAcYear", quoteAcYear);
                    sp12[1] = new SqlParameter("@QRefNo", quoteRefNo);
                    sp12[2] = new SqlParameter("@Category", lblLESSclaimAdjus.ToolTip);
                    sp12[3] = new SqlParameter("@DescriptionDiscount", txtClaimAdjustment.Text);
                    sp12[4] = new SqlParameter("@Amount", txtLESSAMT.Text);
                    sp12[5] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    sp12[6] = new SqlParameter("@discountType", "CLAIM ADJUSTMENT");
                    int resp1 = daCOTS.ExecuteNonQuery_SP("SP_INS_QuoteOtherCharges", sp12);
                }
                if (txtotherdiscount.Text != "" && txtOtherDisAmt.Text != "")
                {
                    SqlParameter[] sp112 = new SqlParameter[7];
                    sp112[0] = new SqlParameter("@QAcYear", quoteAcYear);
                    sp112[1] = new SqlParameter("@QRefNo", quoteRefNo);
                    sp112[2] = new SqlParameter("@Category", lblLessdiscount.ToolTip);
                    sp112[3] = new SqlParameter("@DescriptionDiscount", txtotherdiscount.Text);
                    sp112[4] = new SqlParameter("@Amount", txtOtherDisAmt.Text);
                    sp112[5] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    sp112[6] = new SqlParameter("@discountType", "OTHER DISCOUNT");
                    daCOTS.ExecuteNonQuery_SP("SP_INS_QuoteOtherCharges", sp112);
                }

                //SqlParameter[] spGST = new SqlParameter[]{
                //    new SqlParameter("@QAcYear", );
                //    new SqlParameter("@QRefNo", );
                //    new SqlParameter("@CGST", chkCGST.Checked == true ? Convert.ToDecimal(txtCGST.Text) : Convert.ToDecimal("0.00")),
                //    new SqlParameter("@SGST", chkSGST.Checked == true ? Convert.ToDecimal(txtSGST.Text) : Convert.ToDecimal("0.00")),
                //    new SqlParameter("@IGST", chkIGST.Checked == true ? Convert.ToDecimal(txtIGST.Text) : Convert.ToDecimal("0.00"))
                //};
                //daCOTS.ExecuteNonQuery_SP("SP_UPD_CotsProfomasent_GST", spGST);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvAmountSub_Amount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal totBasicPrice = 0;
                if (hdnTotalBasicPrice.Value != "")
                    totBasicPrice = Convert.ToDecimal(hdnTotalBasicPrice.Value);
                for (int i = 0; i < gvAmountSub.Rows.Count; i++)
                {
                    TextBox txt = gvAmountSub.Rows[i].FindControl("txtCAddAmt") as TextBox;
                    if (txt.Text != "")
                        totBasicPrice += Convert.ToDecimal(txt.Text);
                }
                if (txtLESSAMT.Text != "")
                    totBasicPrice -= Convert.ToDecimal(txtLESSAMT.Text);
                if (txtOtherDisAmt.Text != "")
                    totBasicPrice -= Convert.ToDecimal(txtOtherDisAmt.Text);

                lblsubTotalVal.Text = totBasicPrice.ToString("0.00");

                GSTValue_Changed(null, null);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void GSTValue_Changed(object sender, EventArgs e)
        {
            try
            {
                hdnCGSTVal.Value = chkCGST.Checked == true ? (Convert.ToDecimal(lblsubTotalVal.Text) * Convert.ToDecimal(txtCGST.Text) / 100).ToString() : "0";
                hdnSGSTVal.Value = chkSGST.Checked == true ? (Convert.ToDecimal(lblsubTotalVal.Text) * Convert.ToDecimal(txtSGST.Text) / 100).ToString() : "0";
                hdnIGSTVal.Value = chkIGST.Checked == true ? (Convert.ToDecimal(lblsubTotalVal.Text) * Convert.ToDecimal(txtIGST.Text) / 100).ToString() : "0";
                decimal CGST = chkCGST.Checked == true ? Convert.ToDecimal(hdnCGSTVal.Value) : Convert.ToDecimal("0.00");
                decimal SGST = chkSGST.Checked == true ? Convert.ToDecimal(hdnSGSTVal.Value) : Convert.ToDecimal("0.00");
                decimal IGST = chkIGST.Checked == true ? Convert.ToDecimal(hdnIGSTVal.Value) : Convert.ToDecimal("0.00");
                decimal subTotal = Convert.ToDecimal(lblsubTotalVal.Text);
                decimal taxTot = 0;
                if (CGST > 0) taxTot += CGST;
                if (SGST > 0) taxTot += SGST;
                if (IGST > 0) taxTot += IGST;
                lblTotTaxValue.Text = Convert.ToInt32(taxTot).ToString("0.00");
                lblGrandTotal.Text = Convert.ToInt32(subTotal + taxTot).ToString("0.00");
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoQuoteDiv('lblPriceTotal');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}