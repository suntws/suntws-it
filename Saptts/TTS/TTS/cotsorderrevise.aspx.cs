using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Reflection;
using System.Text;
using System.Globalization;

namespace TTS
{
    public partial class cotsorderrevise : System.Web.UI.Page
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
                        Dictionary<string, string> rdoList = new Dictionary<string, String>();
                        if (Utilities.Decrypt(Request["reqid"].ToString()) == "")
                        {
                            rdoList.Add("0", "MASTER DETAILS");
                            rdoList.Add("1", "ORDER ITEMS");
                        }
                        else if (Utilities.Decrypt(Request["reqid"].ToString()) == "14")
                            rdoList.Add("0", "MASTER DETAILS");
                        else if (Utilities.Decrypt(Request["reqid"].ToString()) == "15" || Utilities.Decrypt(Request["reqid"].ToString()) == "16")
                        {
                            rdoList.Add("1", "ORDER ITEMS");
                            SqlParameter[] spProforma = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) };
                            DataTable dtProforma = (DataTable)daCOTS.ExecuteReader_SP("SP_CHK_CotsOrderRevise_ProformaCreated", spProforma, DataAccess.Return_Type.DataTable);
                            if (dtProforma.Rows.Count > 0)
                                rdoList.Add("2", "PROFORMA DETAILS");
                        }
                        rdbEditType.DataSource = rdoList;
                        rdbEditType.DataTextField = "Value";
                        rdbEditType.DataValueField = "Key";
                        rdbEditType.DataBind();

                        SqlParameter[] sp1 = new SqlParameter[1];
                        sp1[0] = new SqlParameter("@CustCode", "DE0048");
                        DataTable dtAppType = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ApprovedList_For_ManualTyreType", sp1, DataAccess.Return_Type.DataTable);
                        ViewState["dtAppType"] = dtAppType;

                        if (Utilities.Decrypt(Request["oid"].ToString()) != "")
                            bindDetail();
                    }
                    if (Request["__EVENTTARGET"] == "ctl00$MainContent$gvMasterDetail$ctl02$ddlBillingAddress")
                        ddlBillingAddress_IndexChange(Page.FindControl("ctl00$MainContent$gvMasterDetail$ctl02$ddlBillingAddress"), e);
                    if (Request["__EVENTTARGET"] == "ctl00$MainContent$gvMasterDetail$ctl02$ddlShippingAddress")
                        ddlShippingAddress_IndexChange(Page.FindControl("ctl00$MainContent$gvMasterDetail$ctl02$ddlShippingAddress"), e);
                }
                else
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindDetail()
        {
            try
            {
                // bind master details
                gvMasterDetail.DataSource = null;
                gvMasterDetail.DataBind();
                SqlParameter[] spMaster = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) };
                DataTable dtMaster = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_CotsOrderRevise_MasterDetails", spMaster, DataAccess.Return_Type.DataTable);
                if (dtMaster.Rows.Count > 0)
                {
                    gvMasterDetail.DataSource = dtMaster;
                    gvMasterDetail.DataBind();
                    ViewState["dtMaster"] = dtMaster;

                    lblCustName.Text = dtMaster.Rows[0]["custfullname"].ToString();
                    lblStausOrderRefNo.Text = dtMaster.Rows[0]["OrderRefNo"].ToString();
                }
                if (Utilities.Decrypt(Request["reqid"].ToString()) == "" || Utilities.Decrypt(Request["reqid"].ToString()) == "15" ||
                    Utilities.Decrypt(Request["reqid"].ToString()) == "16")
                {
                    Build_ManualOrderItem();
                }
                if (Utilities.Decrypt(Request["reqid"].ToString()) == "15" || Utilities.Decrypt(Request["reqid"].ToString()) == "16")
                {
                    //bind proforma details
                    if (rdbEditType.Items.Count == 2)
                    {
                        DataTable dtgv1 = new DataTable();
                        dtgv1.Columns.Add("slno", typeof(String));
                        for (int i = 1; i <= 3; i++)
                            dtgv1.Rows.Add(i);
                        gvAmountSub.DataSource = dtgv1;
                        gvAmountSub.DataBind();

                        chkCGST.Checked = false;
                        chkSGST.Checked = false;
                        chkIGST.Checked = false;
                        txtCGST.Text = "0.00";
                        txtSGST.Text = "0.00";
                        txtIGST.Text = "0.00";

                        if (dtMaster.Rows[0]["ModeOfTransport"].ToString().Trim() != "")
                            rdbModeOfTransport.Items.FindByText(dtMaster.Rows[0]["ModeOfTransport"].ToString()).Selected = true;
                        if (dtMaster.Rows[0]["CGST"].ToString().Trim() != "" && Convert.ToDecimal(dtMaster.Rows[0]["CGST"].ToString()) > 0)
                        {
                            txtCGST.Text = dtMaster.Rows[0]["CGST"].ToString();
                            chkCGST.Checked = true;
                        }
                        if (dtMaster.Rows[0]["SGST"].ToString().Trim() != "" && Convert.ToDecimal(dtMaster.Rows[0]["SGST"].ToString()) > 0)
                        {
                            txtSGST.Text = dtMaster.Rows[0]["SGST"].ToString();
                            chkSGST.Checked = true;
                        }
                        if (dtMaster.Rows[0]["IGST"].ToString().Trim() != "" && Convert.ToDecimal(dtMaster.Rows[0]["IGST"].ToString()) > 0)
                        {
                            txtIGST.Text = dtMaster.Rows[0]["IGST"].ToString();
                            chkIGST.Checked = true;
                        }
                        SqlParameter[] spDiscount = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) };
                        DataTable dtDiscount = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_CotsOrderRevise_ExtraCharge", spDiscount, DataAccess.Return_Type.DataTable);
                        if (dtDiscount.Rows.Count > 0)
                        {
                            DataTable dt = dtDiscount.Select("DiscountType='OTHER CHARGES'").CopyToDataTable();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                ((TextBox)gvAmountSub.Rows[i].FindControl("txtAddDesc")).Text = dt.Rows[i]["DescriptionDiscount"].ToString();
                                ((TextBox)gvAmountSub.Rows[i].FindControl("txtCAddAmt")).Text = dt.Rows[i]["Amount"].ToString();
                            }

                            foreach (DataRow dr in dtDiscount.Select("DiscountType='CLAIM ADJUSTMENT'"))
                            {
                                txtClaimAdjustment.Text = dr["DescriptionDiscount"].ToString();
                                txtLESSAMT.Text = dr["Amount"].ToString();
                            }

                            foreach (DataRow dr in dtDiscount.Select("DiscountType='OTHER DISCOUNT'"))
                            {
                                txtotherdiscount.Text = dr["DescriptionDiscount"].ToString();
                                txtOtherDisAmt.Text = dr["Amount"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected string bindAddress(string strID)
        {
            try
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@addressid", Convert.ToInt32(strID)) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderAddressDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string tr = "<tr style='vertical-align: top;'><th style='width:85px;'>" + dt.Columns[i].ToString().ToUpper() +
                            "</th><td style='font-weight:bold;'>:</td><td style='font-weight:bold;'>" + row[i].ToString().Replace("~", "<br/>") + "</td></tr>";
                        builder.Append(tr);
                    }
                }
                return builder.ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        protected void gvMasterDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "Edit")
                {
                    gvMasterDetail.EditIndex = Convert.ToInt32(e.CommandArgument);
                    bindDetail();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvMasterDetail_RowEdit(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvMasterDetail.EditIndex = e.NewEditIndex;
                bindDetail();

                GridViewRow gvr = ((GridView)sender).Rows[e.NewEditIndex];
                FillRadioButtonList_From_XML(gvr);
                bindAddress(gvr);

                DataTable dtMaster = ViewState["dtMaster"] as DataTable;
                if (((RadioButtonList)gvr.FindControl("rdbOrderGrade")).Items.FindByValue(dtMaster.Rows[0]["grade"].ToString()) != null)
                    ((RadioButtonList)gvr.FindControl("rdbOrderGrade")).Items.FindByValue(dtMaster.Rows[0]["grade"].ToString()).Selected = true;

                ((DropDownList)gvr.FindControl("ddlBillingAddress")).Items.FindByValue(dtMaster.Rows[0]["BillID"].ToString()).Selected = true;
                ddlBillingAddress_IndexChange((DropDownList)gvr.FindControl("ddlBillingAddress"), e);

                ((DropDownList)gvr.FindControl("ddlShippingAddress")).Items.FindByValue(dtMaster.Rows[0]["ShipID"].ToString()).Selected = true;
                ddlShippingAddress_IndexChange((DropDownList)gvr.FindControl("ddlShippingAddress"), e);

                ((TextBox)gvr.FindControl("txtDesiredDate")).Text = Convert.ToDateTime(dtMaster.Rows[0]["DesiredShipDate"].ToString()).ToString("dd/MM/yyyy");

                if (dtMaster.Rows[0]["PackingMethod"].ToString() != "" &&
                    ((RadioButtonList)gvr.FindControl("rdbPackingMethod")).Items.FindByValue(dtMaster.Rows[0]["PackingMethod"].ToString().ToUpper()) != null)
                    ((RadioButtonList)gvr.FindControl("rdbPackingMethod")).Items.FindByValue(dtMaster.Rows[0]["PackingMethod"].ToString().ToUpper()).Selected = true;

                if (((RadioButtonList)gvr.FindControl("rdbFreightCharges")).Items.FindByValue(dtMaster.Rows[0]["FreightCharges"].ToString().ToUpper()) != null)
                    ((RadioButtonList)gvr.FindControl("rdbFreightCharges")).Items.FindByValue(dtMaster.Rows[0]["FreightCharges"].ToString().ToUpper()).Selected = true;

                if (((RadioButtonList)gvr.FindControl("rdbDelivery")).Items.FindByValue(dtMaster.Rows[0]["DeliveryMethod"].ToString().ToUpper()) != null)
                    ((RadioButtonList)gvr.FindControl("rdbDelivery")).Items.FindByValue(dtMaster.Rows[0]["DeliveryMethod"].ToString().ToUpper()).Selected = true;
                rdbEditType.Enabled = false;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvMasterDetail_CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gvMasterDetail.EditIndex = -1;
                bindDetail();
                rdbEditType.Enabled = true;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvMasterDetail_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow gvr = ((GridView)sender).Rows[e.RowIndex];
                SqlParameter[] spMaster = new SqlParameter[]{
                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())),
                        new SqlParameter("@grade", ((RadioButtonList) gvr.FindControl("rdbOrderGrade")).SelectedItem.Text),
                        new SqlParameter("@ShipID", ((DropDownList) gvr.FindControl("ddlShippingAddress") ).SelectedItem.Value),
                        new SqlParameter("@BillID", ((DropDownList) gvr.FindControl("ddlBillingAddress")).SelectedItem.Value),
                        new SqlParameter("@DesiredShipDate", DateTime.ParseExact(((TextBox)gvr.FindControl("txtDesiredDate")).Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        new SqlParameter("@PackingMethod", ((RadioButtonList) gvr.FindControl("rdbPackingMethod")).SelectedItem.Text),
                        new SqlParameter("@PackingOthers", ((TextBox)gvr.FindControl("txtPackingOthers")).Text),
                        new SqlParameter("@FreightCharges", ((RadioButtonList) gvr.FindControl("rdbFreightCharges")).SelectedItem.Text),
                        new SqlParameter("@DeliveryMethod", ((RadioButtonList) gvr.FindControl("rdbDelivery")).SelectedItem.Text),
                        new SqlParameter("@GodownName", ((TextBox) gvr.FindControl("txtGodownName")).Text),
                        new SqlParameter("@TransportDetails", ((TextBox) gvr.FindControl("txtTransportDetails")).Text),
                        new SqlParameter("@SpecialRequset", ((TextBox) gvr.FindControl("txtSplReq")).Text.Replace("\r\n", "~")),
                        new SqlParameter("@SplIns", ((TextBox) gvr.FindControl("txtSplIns")).Text.Replace("\r\n", "~")),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value.ToString())
                    };
                daCOTS.ExecuteNonQuery_SP("SP_UPD_CotsOrderRevise_MasterDetail", spMaster);
                gvMasterDetail_CancelEdit(null, null);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('divStatusChange').style.display='block';", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void FillRadioButtonList_From_XML(GridViewRow gvr)
        {
            try
            {
                //string strXmlShipList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"shippingmethod.xml").Replace("TTS", "" + ConfigurationManager.AppSettings.Get("SCOTS") + "");
                string strXmlShipList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"shippingmethod.xml");
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

                    RadioButtonList rdbPackingMethod = gvr.FindControl("rdbPackingMethod") as RadioButtonList;
                    RadioButtonList rdbFreightCharges = gvr.FindControl("rdbFreightCharges") as RadioButtonList;
                    RadioButtonList rdbDelivery = gvr.FindControl("rdbDelivery") as RadioButtonList;

                    foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/packing"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value.ToString().ToUpper());
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtShipList.Rows.Add(item.Value);
                        }

                        rdbPackingMethod.DataSource = dtShipList;
                        rdbPackingMethod.DataTextField = "item";
                        rdbPackingMethod.DataValueField = "item";
                        rdbPackingMethod.DataBind();
                    }

                    //Freight Charges
                    dict.Clear();
                    dtShipList.Clear();
                    foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/freight"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value.ToString().ToUpper());
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtShipList.Rows.Add(item.Value);
                        }
                        rdbFreightCharges.DataSource = dtShipList;
                        rdbFreightCharges.DataTextField = "item";
                        rdbFreightCharges.DataValueField = "item";
                        rdbFreightCharges.DataBind();
                    }

                    //delivery method
                    dict.Clear();
                    dtShipList.Clear();
                    foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/delivery"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value.ToString().ToUpper());
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtShipList.Rows.Add(item.Value);
                        }
                        rdbDelivery.DataSource = dtShipList;
                        rdbDelivery.DataTextField = "item";
                        rdbDelivery.DataValueField = "item";
                        rdbDelivery.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindAddress(GridViewRow gvr)
        {
            try
            {
                DropDownList ddlBillingAddress = (DropDownList)gvr.FindControl("ddlBillingAddress");
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@custcode", Utilities.Decrypt(Request["cid"].ToString())) };
                DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList", sp1, DataAccess.Return_Type.DataTable);
                if (dtBillAddress.Rows.Count > 0)
                {
                    ddlBillingAddress.DataSource = dtBillAddress;
                    ddlBillingAddress.DataTextField = "ShipAddress";
                    ddlBillingAddress.DataValueField = "shipid";
                    ddlBillingAddress.DataBind();
                    if (dtBillAddress.Rows.Count == 1)
                        ddlBillingAddress_IndexChange(ddlBillingAddress, null);
                    else
                        ddlBillingAddress.Items.Insert(0, "Choose");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlBillingAddress_IndexChange(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvr = (GridViewRow)((Control)sender).Parent.Parent;
                DropDownList ddlShippingAddress = (DropDownList)gvr.FindControl("ddlShippingAddress");
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@BillID", Convert.ToInt32(((DropDownList)sender).SelectedItem.Value)) };
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
                        ddlShippingAddress.Items.Insert(0, "Choose");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlShippingAddress_IndexChange(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvr = (GridViewRow)((Control)sender).Parent.Parent;
                DropDownList ddlShippingAddress = (DropDownList)sender;

                ((Literal)gvr.FindControl("ltrlBillAddress")).Text = bindAddress(((DropDownList)gvr.FindControl("ddlBillingAddress")).SelectedItem.Value);
                ((Literal)gvr.FindControl("ltrlShipAddress")).Text = bindAddress(ddlShippingAddress.SelectedItem.Value);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlPlatform.DataSource = "";
                ddlPlatform.DataBind();
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();
                ddlType.DataSource = "";
                ddlType.DataBind();
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtFinishedWt.Text = "";
                txtCustomPriceSheet.Text = "";
                txtBasicPrice.Text = "";
                txtDiscount.Text = "";
                lblProcessID.Text = "";
                txtRimPrice.Text = "";
                txtRimWt.Text = "";
                txtRimDwg.Text = "";
                chkrimassmbly.Checked = false;
                chkrimassmbly.Enabled = false;
                if (ddlCategory.SelectedItem.Value != "Choose")
                {
                    if (ddlCategory.SelectedItem.Value == "1" || ddlCategory.SelectedValue.ToString() == "2" || ddlCategory.SelectedValue.ToString() == "3")
                    {
                        DataTable dtAppType = ViewState["dtAppType"] as DataTable;
                        if (dtAppType.Rows.Count > 0)
                        {
                            DataView dtTypeView = new DataView(dtAppType);
                            dtTypeView.RowFilter = "SizeCategory = '" + ddlCategory.SelectedItem.Text + "'";
                            dtTypeView.Sort = "Config ASC";
                            DataTable disConfig = dtTypeView.ToTable(true, "Config");
                            if (disConfig.Rows.Count > 0)
                            {
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
                        chkrimassmbly.Enabled = true;
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('tr_rimAssembly').style.display='none';", true);
                    }
                    else if (ddlCategory.SelectedValue.ToString() == "4" || ddlCategory.SelectedValue.ToString() == "5")
                    {
                        ddl_EdcNo.DataSource = "";
                        ddl_EdcNo.DataBind();
                        frmRimProcessID_Details.DataSource = "";
                        frmRimProcessID_Details.DataBind();

                        string Query = "select distinct EDCNO from Rim_ProcessID_Details";
                        DataTable dt = (DataTable)daCOTS.ExecuteReader(Query, DataAccess.Return_Type.DataTable);
                        if (dt.Rows.Count > 0)
                        {
                            ddl_EdcNo.DataSource = dt;
                            ddl_EdcNo.DataTextField = "EDCNO";
                            ddl_EdcNo.DataValueField = "EDCNO";
                            ddl_EdcNo.DataBind();
                        }
                        ddl_EdcNo.Items.Insert(0, new ListItem("Choose", "Choose"));
                        chkrimassmbly.Enabled = false;
                        ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview", "DisableJathagam_Category();", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "document.getElementById('tr_rimAssembly').style.display='block';", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlPlatform_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlBrand.DataSource = "";
                ddlBrand.DataBind();
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();
                ddlType.DataSource = "";
                ddlType.DataBind();
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtFinishedWt.Text = "";
                txtCustomPriceSheet.Text = "";
                txtBasicPrice.Text = "";
                txtDiscount.Text = "";
                lblProcessID.Text = "";
                txtRimPrice.Text = "";
                txtRimWt.Text = "";
                txtRimDwg.Text = "";
                chkrimassmbly.Checked = false;
                chkrimassmbly.Enabled = false;
                if (ddlPlatform.SelectedItem.Value != "Choose")
                {
                    DataTable dtAppType = ViewState["dtAppType"] as DataTable;
                    if (dtAppType.Rows.Count > 0)
                    {
                        DataView dtTypeView = new DataView(dtAppType);
                        dtTypeView.RowFilter = "SizeCategory = '" + ddlCategory.SelectedItem.Text + "' and Config = '" + ddlPlatform.SelectedItem.Text + "'";
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
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlBrand_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlSidewall.DataSource = "";
                ddlSidewall.DataBind();
                ddlType.DataSource = "";
                ddlType.DataBind();
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtFinishedWt.Text = "";
                txtCustomPriceSheet.Text = "";
                txtBasicPrice.Text = "";
                txtDiscount.Text = "";
                lblProcessID.Text = "";
                txtRimPrice.Text = "";
                txtRimWt.Text = "";
                txtRimDwg.Text = "";
                chkrimassmbly.Checked = false;
                chkrimassmbly.Enabled = false;
                if (ddlBrand.SelectedItem.Value != "CHOOSE")
                {
                    DataTable dtAppType = ViewState["dtAppType"] as DataTable;
                    if (dtAppType.Rows.Count > 0)
                    {
                        DataView dtTypeView = new DataView(dtAppType);
                        dtTypeView.RowFilter = "SizeCategory = '" + ddlCategory.SelectedItem.Text + "' and Config = '" + ddlPlatform.SelectedItem.Text + "' and Brand='" +
                            ddlBrand.SelectedItem.Text + "'";
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
                if (ddlCategory.SelectedItem.Value == "4" || ddlCategory.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview2", "DisableJathagam_Category();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlSidewall_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlType.DataSource = "";
                ddlType.DataBind();
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtFinishedWt.Text = "";
                txtCustomPriceSheet.Text = "";
                txtBasicPrice.Text = "";
                txtDiscount.Text = "";
                lblProcessID.Text = "";
                txtRimPrice.Text = "";
                txtRimWt.Text = "";
                txtRimDwg.Text = "";
                chkrimassmbly.Checked = false;
                chkrimassmbly.Enabled = false;
                if (ddlSidewall.SelectedItem.Value != "Choose")
                {
                    DataTable dtAppType = ViewState["dtAppType"] as DataTable;
                    if (dtAppType.Rows.Count > 0)
                    {
                        DataView dtTypeView = new DataView(dtAppType);
                        dtTypeView.RowFilter = "SizeCategory = '" + ddlCategory.SelectedItem.Text + "' and Config = '" + ddlPlatform.SelectedItem.Text + "' and Brand='" +
                            ddlBrand.SelectedItem.Text + "' and Sidewall='" + ddlSidewall.SelectedItem.Text + "'";
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
                if (ddlCategory.SelectedItem.Value == "4" || ddlCategory.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview2", "DisableJathagam_Category();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlSize.DataSource = "";
                ddlSize.DataBind();
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtFinishedWt.Text = "";
                txtCustomPriceSheet.Text = "";
                txtBasicPrice.Text = "";
                txtDiscount.Text = "";
                lblProcessID.Text = "";
                txtRimPrice.Text = "";
                txtRimWt.Text = "";
                txtRimDwg.Text = "";
                chkrimassmbly.Checked = false;
                chkrimassmbly.Enabled = false;
                if (ddlType.SelectedItem.Value != "CHOOSE")
                {
                    if (hdnItemEditMode.Value == "0")
                    {
                        SqlParameter[] sp2 = new SqlParameter[]{
                            new SqlParameter("@Config", ddlPlatform.SelectedItem.Text),
                            new SqlParameter("@brand", ddlBrand.SelectedItem.Text),
                            new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text),
                            new SqlParameter("@Tyretype", ddlType.SelectedItem.Text),
                            new SqlParameter("@custId", Utilities.Decrypt(Request["cid"].ToString())),
                            new SqlParameter("@grade", "A")
                        };
                        DataTable dt1 = (DataTable)daCOTS.ExecuteReader_SP("SP_GET_TypeWiseDiscount_Discount", sp2, DataAccess.Return_Type.DataTable);

                        hdnDiscount.Value = dt1.Rows[0]["Discount"].ToString();
                        txtDiscount.Text = hdnDiscount.Value;
                    }

                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                    sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                    sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                    sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                    DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Size_For_ManualOrder", sp1, DataAccess.Return_Type.DataTable);

                    if (dt.Rows.Count > 0)
                    {
                        DataView dtTypeView = new DataView(dt);
                        DataTable dtType = dtTypeView.ToTable(true, "TyreSize");
                        ddlSize.DataSource = dtType;
                        ddlSize.DataTextField = "TyreSize";
                        ddlSize.DataValueField = "TyreSize";
                        ddlSize.DataBind();
                        if (dt.Rows.Count == 1)
                            ddlSize_IndexChange(sender, e);
                        else
                            ddlSize.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                    }
                }
                if (ddlCategory.SelectedItem.Value == "4" || ddlCategory.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview4", "DisableJathagam_Category();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                ddlRim.DataSource = "";
                ddlRim.DataBind();
                txtFinishedWt.Text = "";
                txtCustomPriceSheet.Text = "";
                txtBasicPrice.Text = "";
                txtDiscount.Text = "";
                lblProcessID.Text = "";
                txtRimPrice.Text = "";
                txtRimWt.Text = "";
                txtRimDwg.Text = "";
                chkrimassmbly.Checked = false;
                chkrimassmbly.Enabled = false;
                if (ddlSize.SelectedItem.Value != "Choose")
                {
                    if (ddlCategory.SelectedValue.ToString() == "1" || ddlCategory.SelectedValue.ToString() == "2" || ddlCategory.SelectedValue.ToString() == "3")
                    {
                        SqlParameter[] sp1 = new SqlParameter[5];
                        sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                        sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                        sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                        sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                        sp1[4] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);

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
                            {
                                ddlRim.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Choose", "Choose"));
                            }
                        }
                    }
                }
                if (ddlCategory.SelectedItem.Value == "4" || ddlCategory.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview5", "DisableJathagam_Category();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlRim_IndexChange(object sender, EventArgs e)
        {
            try
            {
                txtFinishedWt.Text = "";
                txtCustomPriceSheet.Text = "";
                txtBasicPrice.Text = "";
                txtDiscount.Text = "";
                lblProcessID.Text = "";
                txtRimPrice.Text = "";
                txtRimWt.Text = "";
                txtRimDwg.Text = "";
                chkrimassmbly.Checked = false;
                chkrimassmbly.Enabled = false;
                if (ddlRim.SelectedItem.Value != "Choose")
                {
                    if (ddlCategory.SelectedValue == "1" || ddlCategory.SelectedValue == "2" || ddlCategory.SelectedValue == "3")
                    {
                        txtFinishedWt.Enabled = true;
                        if (hdnItemEditMode.Value == "0")
                            txtBasicPrice.Enabled = true;

                        SqlParameter[] spSpl = new SqlParameter[7];
                        spSpl[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                        spSpl[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                        spSpl[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                        spSpl[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                        spSpl[4] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);
                        spSpl[5] = new SqlParameter("@rimsize", ddlRim.SelectedItem.Text);
                        spSpl[6] = new SqlParameter("@custcode", Utilities.Decrypt(Request["cid"].ToString()));
                        DataTable dtSpl = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_manualorder_unitprice", spSpl, DataAccess.Return_Type.DataTable);

                        if (dtSpl.Rows.Count > 0)
                        {
                            DataRow priceRow = dtSpl.Rows[0];
                            lblProcessID.Text = priceRow["ProcessID"].ToString() != "" ? priceRow["ProcessID"].ToString() : "";
                            txtFinishedWt.Text = priceRow["FinishedWt"].ToString();
                            hdnSizePosition.Value = priceRow["SizePosition"].ToString();
                            hdnTypePosition.Value = priceRow["TypePosition"].ToString();
                            hdnTypeDesc.Value = priceRow["TypeDesc"].ToString();
                            txtCustomPriceSheet.Text = priceRow["UnitPrice"].ToString() == "" ? "0" : priceRow["UnitPrice"].ToString();
                            txtBasicPrice.Text = priceRow["BasicPrice"].ToString() == "" ? "0" : priceRow["BasicPrice"].ToString();
                            txtDiscount.Text = priceRow["Discount"].ToString() == "" ? "0" : priceRow["Discount"].ToString();
                        }
                        else
                        {
                            SqlParameter[] sp1 = new SqlParameter[6];
                            sp1[0] = new SqlParameter("@Config", ddlPlatform.SelectedItem.Text);
                            sp1[1] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text);
                            sp1[2] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text);
                            sp1[3] = new SqlParameter("@Tyretype", ddlType.SelectedItem.Text);
                            sp1[4] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text);
                            sp1[5] = new SqlParameter("@TyreRim", ddlRim.SelectedItem.Text);
                            DataTable dtOther = (DataTable)daTTS.ExecuteReader_SP("sp_sel_OtherDetails_ForManualOrder", sp1, DataAccess.Return_Type.DataTable);
                            if (dtOther.Rows.Count > 0)
                            {
                                DataRow oRow = dtOther.Rows[0];
                                lblProcessID.Text = oRow["ProcessID"].ToString() != "" ? oRow["ProcessID"].ToString() : "";
                                txtFinishedWt.Text = oRow["Finished"].ToString();
                                hdnSizePosition.Value = oRow["position"].ToString();
                                hdnTypePosition.Value = oRow["TypePosition"].ToString();
                                hdnTypeDesc.Value = oRow["TypeDesc"].ToString();
                                if (hdnItemEditMode.Value == "0")
                                    divErrmsg.InnerHtml = "unit price not available in published price-sheet list<br/> you can enter manual price sheet value";
                            }
                        }

                        ddl_EdcNo.DataSource = "";
                        ddl_EdcNo.DataBind();
                        frmRimProcessID_Details.DataSource = "";
                        frmRimProcessID_Details.DataBind();

                        SqlParameter[] spEdc = new SqlParameter[] { 
                            new SqlParameter("@TyreSize", ddlSize.SelectedItem.Text), 
                            new SqlParameter("@Rimsize", ddlRim.SelectedItem.Text) 
                        };
                        DataTable dtEDC = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_EDCNO_Order_TyresizeWise", spEdc, DataAccess.Return_Type.DataTable);
                        if (dtEDC.Rows.Count > 0)
                        {
                            ddl_EdcNo.DataSource = dtEDC;
                            ddl_EdcNo.DataTextField = "EDCNO";
                            ddl_EdcNo.DataValueField = "EDCNO";
                            ddl_EdcNo.DataBind();
                            ddl_EdcNo.Items.Insert(0, new ListItem("Choose", "Choose"));

                            chkrimassmbly.Enabled = true;
                            chkrimassmbly.Checked = false;
                        }
                    }
                    if (ddlCategory.SelectedItem.Value == "4" || ddlCategory.SelectedItem.Value == "5")
                        ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview6", "DisableJathagam_Category();", true);
                    if (txtFinishedWt.Text != "")
                        txtFinishedWt.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void chkrimassmbly_CheckedChanged(object sender, EventArgs e)
        {
            if (chkrimassmbly.Checked)
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "document.getElementById('tr_rimAssembly').style.display='block';", true);
            else if (!chkrimassmbly.Checked)
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('tr_rimAssembly').style.display='none';", true);
        }
        protected void btnAddNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                divErrmsg.InnerHtml = "";
                string strPlatform = string.Empty, strBrand = string.Empty, strSidewall = string.Empty, strType = string.Empty, strSize = string.Empty, strRim = string.Empty;
                bool rimAssmly = chkrimassmbly.Checked == true ? true : false;
                if (ddlCategory.SelectedValue.ToString() == "1" || ddlCategory.SelectedValue.ToString() == "2" || ddlCategory.SelectedValue.ToString() == "3")
                {
                    strPlatform = ddlPlatform.SelectedItem.Text;
                    strBrand = ddlBrand.SelectedItem.Text;
                    strSidewall = ddlSidewall.SelectedItem.Text;
                    strType = ddlType.SelectedItem.Text;
                    strSize = ddlSize.SelectedItem.Text;
                    strRim = ddlRim.SelectedItem.Text;
                }
                string strEDC = (ddl_EdcNo.SelectedItem != null && ddl_EdcNo.SelectedItem.Text != "Choose") ? ddl_EdcNo.SelectedItem.Text : "";

                int itemcount = 0;
                if (hdnEditProcessId.Value == "")
                {
                    SqlParameter[] spExists = new SqlParameter[] { 
                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
                        new SqlParameter("@category", ddlCategory.SelectedItem.Text),
                        new SqlParameter("@Config", strPlatform),
                        new SqlParameter("@brand", strBrand),
                        new SqlParameter("@sidewall", strSidewall),
                        new SqlParameter("@tyretype", strType),
                        new SqlParameter("@tyresize", strSize),
                        new SqlParameter("@rimsize", strRim),
                        new SqlParameter("@AssyRimstatus", rimAssmly),
                        new SqlParameter("@EdcNo", strEDC)
                    };
                    itemcount = (int)daCOTS.ExecuteScalar_SP("sp_chk_ItemExists", spExists);
                }
                if (itemcount == 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("category", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("processid", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("itemqty", typeof(System.Int32)));
                    dt.Columns.Add(new DataColumn("unitprice", typeof(System.Decimal)));
                    dt.Columns.Add(new DataColumn("finishedwt", typeof(System.Decimal)));
                    dt.Columns.Add(new DataColumn("loadingwt", typeof(System.Decimal)));
                    dt.Columns.Add(new DataColumn("sizeposition", typeof(System.Int32)));
                    dt.Columns.Add(new DataColumn("typeposition", typeof(System.Int32)));
                    dt.Columns.Add(new DataColumn("Discount", typeof(System.Decimal)));
                    dt.Columns.Add(new DataColumn("Config", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("tyresize", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("rimsize", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("tyretype", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("brand", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("sidewall", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("TypeDesc", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("SheetPrice", typeof(System.Decimal)));
                    dt.Columns.Add(new DataColumn("Rimitemqty", typeof(System.Int32)));
                    dt.Columns.Add(new DataColumn("Rimunitprice", typeof(System.Decimal)));
                    dt.Columns.Add(new DataColumn("Rimfinishedwt", typeof(System.Decimal)));
                    dt.Columns.Add(new DataColumn("RIMSTATUS", typeof(System.Boolean)));
                    dt.Columns.Add(new DataColumn("RimDwg", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("ItemCode", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("AdditionalInfo", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("EdcNo", typeof(System.String)));

                    DataRow nRow = dt.NewRow();
                    nRow["category"] = ddlCategory.SelectedItem.Text;
                    nRow["processid"] = lblProcessID.Text != "" ? lblProcessID.Text : (hdnItemEditMode.Value == "1" ? hdnEditProcessId.Value : "M" +
                       Utilities.Decrypt(Request["cid"].ToString()) + "-" + gv_ManualOrderList.Rows.Count.ToString());
                    nRow["itemqty"] = txtQty.Text;
                    nRow["unitprice"] = txtBasicPrice.Text != "" ? Convert.ToDecimal(txtBasicPrice.Text) : Convert.ToDecimal("0.00");
                    nRow["finishedwt"] = txtFinishedWt.Text != "" ? Convert.ToDecimal(txtFinishedWt.Text) : Convert.ToDecimal("0.00");
                    nRow["sizeposition"] = hdnSizePosition.Value != "" ? hdnSizePosition.Value : "0";
                    nRow["typeposition"] = hdnTypePosition.Value != "" ? hdnTypePosition.Value : "0";
                    nRow["Discount"] = txtDiscount.Text != "" ? Convert.ToDecimal(txtDiscount.Text) : Convert.ToDecimal("0.00");
                    nRow["Config"] = strPlatform;
                    nRow["tyresize"] = strSize;
                    nRow["rimsize"] = strRim;
                    nRow["tyretype"] = strType;
                    nRow["brand"] = strBrand;
                    nRow["sidewall"] = strSidewall;
                    nRow["TypeDesc"] = hdnTypeDesc.Value;
                    nRow["SheetPrice"] = txtCustomPriceSheet.Text != "" ? Convert.ToDecimal(txtCustomPriceSheet.Text) : Convert.ToDecimal("0.00");
                    nRow["Rimfinishedwt"] = (rimAssmly || strEDC != "") && txtRimWt.Text != "" ? Convert.ToDecimal(txtRimWt.Text) : Convert.ToDecimal("0.00");
                    nRow["Rimunitprice"] = (rimAssmly || strEDC != "") && txtRimPrice.Text != "" ? Convert.ToDecimal(txtRimPrice.Text) : Convert.ToDecimal("0.00");
                    nRow["Rimitemqty"] = (rimAssmly || strEDC != "") ? txtQty.Text : "0";
                    nRow["RIMSTATUS"] = rimAssmly;
                    nRow["RimDwg"] = (rimAssmly || strEDC != "") && txtRimDwg.Text != "" ? txtRimDwg.Text : "";
                    nRow["ItemCode"] = "";
                    nRow["AdditionalInfo"] = "";
                    nRow["EdcNo"] = strEDC;
                    dt.Rows.Add(nRow);

                    SqlParameter[] sp = new SqlParameter[]{
                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())),
                        new SqlParameter("@manual_orderitem_dt", dt),
                        new SqlParameter("@prevProcessId",hdnItemEditMode.Value == "1"? hdnEditProcessId.Value:""),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value.ToString())
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("SP_SAV_CotsOrderRevise_OrderItem_V1", sp);

                    if (resp > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('divStatusChange').style.display='block';", true);
                        gv_ManualOrderList.EditIndex = -1;
                        bindDetail();
                    }
                }
                else
                    divErrmsg.InnerHtml = "Item Already Added. You can change the qty or delete this item";

                btnAddNextItem.Text = "ADD NEXT ITEM";
                hdnItemEditMode.Value = "0";
                ddlCategory.Enabled = true;
                ddlPlatform.Enabled = true;
                ddlBrand.Enabled = true;
                ddlSidewall.Enabled = true;
                ddlType.Enabled = true;
                ddlSize.Enabled = true;
                ddlRim.Enabled = true;
                txtDiscount.Enabled = true;
                txtCustomPriceSheet.Enabled = true;
                txtBasicPrice.Enabled = true;
                rdbEditType.Enabled = true;
                hdnEditProcessId.Value = "";

                txtQty.Text = "";
                txtRimPrice.Text = "";
                txtRimWt.Text = "";
                txtRimDwg.Text = "";
                txtBasicPrice.Text = "";
                txtCustomPriceSheet.Text = "";
                lblProcessID.Text = "";
                txtFinishedWt.Enabled = true;
                txtFinishedWt.Text = "";
                hdnSizePosition.Value = "";
                hdnTypePosition.Value = "";
                hdnTypeDesc.Value = "";
                if (ddlCategory.SelectedValue.ToString() == "1" || ddlCategory.SelectedValue.ToString() == "2" || ddlCategory.SelectedValue.ToString() == "3")
                {
                    ddlSize.SelectedIndex = 0;
                    ddlRim.SelectedIndex = 0;
                }
                ddl_EdcNo.DataSource = "";
                ddl_EdcNo.DataBind();
                frmRimProcessID_Details.DataSource = "";
                frmRimProcessID_Details.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_ManualOrderItem()
        {
            try
            {
                gv_ManualOrderList.DataSource = null;
                gv_ManualOrderList.DataBind();
                chkrimassmbly.Checked = false;
                DataTable dtItems = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())));
                if (dtItems.Rows.Count > 0)
                {
                    gv_ManualOrderList.DataSource = dtItems;
                    gv_ManualOrderList.DataBind();

                    gv_ManualOrderList.FooterRow.Cells[9].Text = "TOTAL";
                    gv_ManualOrderList.FooterRow.Cells[10].Text = dtItems.Compute("Sum(itemqty)", "").ToString();
                    chkrimassmbly.Checked = false;

                    gv_ManualOrderList.Columns[12].Visible = false;
                    gv_ManualOrderList.Columns[13].Visible = false;
                    foreach (DataRow row in dtItems.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gv_ManualOrderList.Columns[12].Visible = true;
                        gv_ManualOrderList.Columns[13].Visible = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_ManualOrderList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                hdnItemEditMode.Value = "1";
                divErrmsg.InnerHtml = "";
                btnAddNextItem.Text = "UPDATE ITEM";
                gv_ManualOrderList.EditIndex = e.NewEditIndex;
                Build_ManualOrderItem();

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
                    new SqlParameter("@processid", ((HiddenField)gv_ManualOrderList.Rows[e.NewEditIndex].FindControl("hdnprocessid")).Value), 
                    new SqlParameter("@AssyRimstatus", Convert.ToBoolean(((HiddenField)gv_ManualOrderList.Rows[e.NewEditIndex].FindControl("hdnAssyStatus")).Value)), 
                    new SqlParameter("@EdcNo", ((Label)gv_ManualOrderList.Rows[e.NewEditIndex].FindControl("lblEdcNo")).Text) 
                };
                DataTable dtItem = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_revise_item", sp, DataAccess.Return_Type.DataTable);
                bindCategory(dtItem.Rows[0]);

                ddlCategory.Enabled = false;
                ddlPlatform.Enabled = false;
                ddlBrand.Enabled = false;
                ddlSidewall.Enabled = false;
                ddlType.Enabled = false;
                ddlSize.Enabled = false;
                ddlRim.Enabled = false;
                txtDiscount.Enabled = false;
                txtCustomPriceSheet.Enabled = false;
                txtBasicPrice.Enabled = false;
                rdbEditType.Enabled = false;
                hdnEditProcessId.Value = ((HiddenField)gv_ManualOrderList.Rows[e.NewEditIndex].FindControl("hdnprocessid")).Value;
                ScriptManager.RegisterStartupScript(Page, GetType(), "gotoItemDiv", "show_ManualOrderCtrl('divAddItem');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_ManualOrderList_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gv_ManualOrderList.EditIndex = -1;
                Build_ManualOrderItem();
                hdnItemEditMode.Value = "0";
                ddlCategory.Enabled = true;
                ddlPlatform.Enabled = true;
                ddlBrand.Enabled = true;
                ddlSidewall.Enabled = true;
                ddlType.Enabled = true;
                ddlSize.Enabled = true;
                ddlRim.Enabled = true;
                txtDiscount.Enabled = true;
                txtCustomPriceSheet.Enabled = true;
                txtBasicPrice.Enabled = true;
                rdbEditType.Enabled = true;
                divErrmsg.InnerHtml = "";
                hdnEditProcessId.Value = "";
                btnAddNextItem.Text = "ADD NEXT ITEM";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_ManualOrderList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gv_ManualOrderList.Rows[e.RowIndex];
                HiddenField hdnprocessid = row.FindControl("hdnprocessid") as HiddenField;
                if (hdnprocessid.Value != "")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())), 
                        new SqlParameter("@ProcessId", hdnprocessid.Value),
                        new SqlParameter("@AssyRimStatus", ((HiddenField)row.FindControl("hdnAssyStatus")).Value ),
                        new SqlParameter("@EdcNo", ((Label)row.FindControl("lblEdcNo")).Text),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("SP_DEL_CotsOrderRevise_Item", sp);
                    if (resp > 0)
                        Build_ManualOrderItem();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindCategory(DataRow dr)
        {
            try
            {
                ddlCategory.ClearSelection();
                ddlCategory.Items.FindByText(dr["category"].ToString().ToUpper()).Selected = true;
                ddlCategory_IndexChange(null, null);
                if (dr["category"].ToString().ToUpper() == "SOLID" || dr["category"].ToString().ToUpper() == "POB" || dr["category"].ToString().ToUpper() == "PNEUMATIC")
                {
                    ddlPlatform.ClearSelection();
                    ddlPlatform.Items.FindByText(dr["Config"].ToString()).Selected = true;
                    ddlPlatform_IndexChange(null, null);

                    ddlBrand.ClearSelection();
                    ddlBrand.Items.FindByText(dr["brand"].ToString()).Selected = true;
                    ddlBrand_IndexChange(null, null);

                    ddlSidewall.ClearSelection();
                    ddlSidewall.Items.FindByText(dr["sidewall"].ToString()).Selected = true;
                    ddlSidewall_IndexChange(null, null);

                    ddlType.ClearSelection();
                    ddlType.Items.FindByText(dr["tyretype"].ToString()).Selected = true;
                    ddlType_IndexChange(null, null);

                    ddlSize.ClearSelection();
                    ddlSize.Items.FindByText(dr["tyresize"].ToString()).Selected = true;
                    ddlSize_IndexChange(null, null);

                    ddlRim.ClearSelection();
                    ddlRim.Items.FindByText(dr["rimsize"].ToString()).Selected = true;
                    ddlRim_IndexChange(null, null);
                }

                txtDiscount.Text = dr["Discount"].ToString();
                txtCustomPriceSheet.Text = dr["SheetPrice"].ToString();
                txtBasicPrice.Text = dr["unitprice"].ToString();
                txtQty.Text = dr["itemqty"].ToString();
                txtFinishedWt.Text = dr["finishedwt"].ToString();
                lblProcessID.Text = dr["processid"].ToString();
                chkrimassmbly.Checked = false;
                if (Convert.ToBoolean(dr["AssyRimstatus"].ToString()) == true || dr["category"].ToString() == "SPLIT RIMS" || dr["AssyRimstatus"].ToString() == "POB WHEEL")
                {
                    chkrimassmbly.Checked = Convert.ToBoolean(dr["AssyRimstatus"].ToString());
                    txtRimPrice.Text = dr["Rimunitprice"].ToString();
                    txtRimWt.Text = dr["Rimfinishedwt"].ToString();
                    txtRimDwg.Text = dr["RimDwg"].ToString();

                    ddl_EdcNo.ClearSelection();
                    ddl_EdcNo.Items.FindByText(dr["EdcNo"].ToString()).Selected = true;
                    ddl_EdcNo_SelectedIndexChanged(null, null);

                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "document.getElementById('tr_rimAssembly').style.display='block';", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_EdcNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                frmRimProcessID_Details.DataSource = "";
                frmRimProcessID_Details.DataBind();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@EDCNO", ddl_EdcNo.SelectedValue) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_sel_Rim_ProcessID_Details_ForEdcNo", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    frmRimProcessID_Details.DataSource = dt;
                    frmRimProcessID_Details.DataBind();

                    txtRimWt.Text = dt.Rows[0]["RimWt"].ToString();
                }
                if (ddlCategory.SelectedItem.Value == "4" || ddlCategory.SelectedItem.Value == "5")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "gotopreview7", "DisableJathagam_Category();", true);
                else
                    chkrimassmbly_CheckedChanged(null, null);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnProformaUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                daCOTS.ExecuteNonQuery_SP("sp_del_Export_Discount", sp);

                for (int i = 0; i < gvAmountSub.Rows.Count; i++)
                {
                    string DESC = ((TextBox)gvAmountSub.Rows[i].FindControl("txtAddDesc")).Text.Trim();
                    string AMT = ((TextBox)gvAmountSub.Rows[i].FindControl("txtCAddAmt")).Text.Trim();
                    if (DESC != "" && AMT != "")
                    {
                        SqlParameter[] sp2 = new SqlParameter[]{
                            new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                            new SqlParameter("@Category", "ADD"),
                            new SqlParameter("@DescriptionDiscount", DESC),
                            new SqlParameter("@Amount", AMT),
                            new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                            new SqlParameter("@discountType", "OTHER CHARGES")
                        };
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp2);
                    }
                }
                if (txtClaimAdjustment.Text != "" && txtLESSAMT.Text != "")
                {
                    SqlParameter[] sp12 = new SqlParameter[]{
                        new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                        new SqlParameter("@Category", lblLESSclaimAdjus.ToolTip),
                        new SqlParameter("@DescriptionDiscount", txtClaimAdjustment.Text),
                        new SqlParameter("@Amount", txtLESSAMT.Text),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@discountType", "CLAIM ADJUSTMENT")
                    };
                    int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp12);
                }
                if (txtotherdiscount.Text != "" && txtOtherDisAmt.Text != "")
                {
                    SqlParameter[] sp112 = new SqlParameter[]{
                        new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                        new SqlParameter("@Category", lblLessdiscount.ToolTip),
                        new SqlParameter("@DescriptionDiscount", txtotherdiscount.Text),
                        new SqlParameter("@Amount", txtOtherDisAmt.Text),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@discountType", "OTHER DISCOUNT")
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp112);
                }

                SqlParameter[] spGST = new SqlParameter[]{
                    new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())),
                    new SqlParameter("@CGST", chkCGST.Checked == true ? Convert.ToDecimal(txtCGST.Text) : Convert.ToDecimal("0.00")),
                    new SqlParameter("@SGST", chkSGST.Checked == true ? Convert.ToDecimal(txtSGST.Text) : Convert.ToDecimal("0.00")),
                    new SqlParameter("@IGST", chkIGST.Checked == true ? Convert.ToDecimal(txtIGST.Text) : Convert.ToDecimal("0.00")),
                    new SqlParameter("@ModeOfTransport",rdbModeOfTransport.SelectedItem.Text),
                    new SqlParameter("@username",Request.Cookies["TTSUser"].Value.ToString())
                };
                daCOTS.ExecuteNonQuery_SP("SP_UPD_CotsOrderRevise_GST", spGST);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('divStatusChange').style.display='block';", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnProformaCancel_Click(object sender, EventArgs e)
        {
            try
            {
                hdnProformaEditMode.Value = "0";
                bindDetail();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("cotsrevisecancel1.aspx?pcid=dom&type=r", false);
        }
        protected void btnStatusChange_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())), "1",
                    txtStatusComments.Text.Replace("\r\n", "~"), Request.Cookies["TTSUser"].Value);
                if (resp > 0)
                    Response.Redirect("default.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}