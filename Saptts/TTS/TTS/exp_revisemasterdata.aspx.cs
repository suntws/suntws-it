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
using System.Xml;
using System.Text;
using System.Globalization;

namespace TTS
{
    public partial class exp_revisemasterdata : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_revise"].ToString() == "True")
                        {
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_masterdata_revise_orders", DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gv_ReviseOrders.DataSource = dtorderlist;
                                gv_ReviseOrders.DataBind();

                                FillRadionButtonList_From_XML();
                            }
                            else
                                lblErrMsgcontent.Text = "No Records";
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkMasterRevise_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow Row = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnCustCode.Value = ((HiddenField)Row.FindControl("hdnCustCode")).Value;
                lblSelectedOrderNo.Text = ((Label)Row.FindControl("lblOrderRefNo")).Text;
                lblSelectedCustomer.Text = ((Label)Row.FindControl("lblStatusCustName")).Text;
                hdnOID.Value = ((HiddenField)Row.FindControl("hdnOrderID")).Value;
                Bind_ChoosedOrder();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ChoosedOrder()
        {
            try
            {
                Bind_masterdocuments();
                Bind_OrderMasterDetails();
                ScriptManager.RegisterStartupScript(Page, GetType(), "Enabletable", "gotoPreviewDiv('tb_Master');", true);
                if (hdnMasterChangeStatus.Value == "1")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "gotoPreviewDiv('divMasterChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void FillRadionButtonList_From_XML()
        {
            try
            {
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
                    foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/cargo"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtShipList.Rows.Add(item.Value);
                        }
                        Utilities.rdb_Binding(rdo_PackingMethod, dtShipList, "item", "item");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_masterdocuments()
        {
            try
            {
                SqlParameter[] sp3 = new SqlParameter[1];
                sp3[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                DataTable dtDoc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ExpDocumentMaster", sp3, DataAccess.Return_Type.DataTable);
                if (dtDoc.Rows.Count > 0)
                {
                    gv_DocList.DataSource = dtDoc;
                    gv_DocList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ReviseBillingAddress()
        {
            try
            {
                ddl_BillingAddress.DataSource = null;
                ddl_BillingAddress.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@custcode", hdnCustCode.Value) };
                DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList", sp1, DataAccess.Return_Type.DataTable);
                ddl_BillingAddress.DataSource = dtBillAddress;
                ddl_BillingAddress.DataTextField = "ShipAddress";
                ddl_BillingAddress.DataValueField = "shipid";
                ddl_BillingAddress.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderMasterDetails()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderMasterDetails_Exp", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txt_DesiredDate.Text = row["DesiredShipDate"].ToString();
                    txt_SplIns.Text = row["SplIns"].ToString().Replace("~", "\r\n");
                    txt_SplReq.Text = row["SpecialRequset"].ToString().Replace("~", "\r\n");
                    Bind_ReviseBillingAddress();
                    ddl_BillingAddress.SelectedIndex = ddl_BillingAddress.Items.IndexOf(ddl_BillingAddress.Items.FindByValue(row["BillID"].ToString()));
                    ddl_BillingAddress_SelectedIndexChanged(null, null);
                    ddl_ShippingAddress.SelectedIndex = ddl_ShippingAddress.Items.IndexOf(ddl_ShippingAddress.Items.FindByValue(row["ShipID"].ToString()));
                    ddl_ShippingAddress_SelectedIndexChanged(null, null);
                    lbl_BillingAddress.Text = Bind_Address(row["BillID"].ToString());
                    lbl_ShipingDetails.Text = Bind_Address(row["ShipID"].ToString());
                    txt_CountryDestination.Text = row["CountryOfDestination"].ToString();
                    txt_FinalDestination.Text = row["FinalDestination"].ToString();
                    rdo_PackingMethod.SelectedIndex = rdo_PackingMethod.Items.IndexOf(rdo_PackingMethod.Items.FindByValue(row["PackingMethod"].ToString()));
                    txt_PackingOthers.Text = row["PackingOthers"].ToString();
                    rdo_DeliveryTerms.SelectedIndex = rdo_DeliveryTerms.Items.IndexOf(rdo_DeliveryTerms.Items.FindByValue(row["DeliveryMethod"].ToString()));
                    rdo_DeliveryTerms_SelectedIndexChanged(null, EventArgs.Empty);
                    rdo_DeliveryMethod.SelectedIndex = rdo_DeliveryMethod.Items.IndexOf(rdo_DeliveryMethod.Items.FindByValue(row["GodownName"].ToString()));
                    txt_OtherDocuments.Text = row["OtherReqDocuments"].ToString().Replace("~", "\r\n");
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
                if (ddl_BillingAddress.SelectedItem.Text != "Choose")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@BillID", Convert.ToInt32(ddl_BillingAddress.SelectedItem.Value)) };

                    lbl_BillingAddress.Text = Bind_Address(ddl_BillingAddress.SelectedItem.Value);
                    DataTable dtShippAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ShipList_BillID", sp1, DataAccess.Return_Type.DataTable);
                    ddl_ShippingAddress.DataSource = dtShippAddress;
                    ddl_ShippingAddress.DataTextField = "ShipAddress";
                    ddl_ShippingAddress.DataValueField = "shipid";
                    ddl_ShippingAddress.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Enabletable1", "gotoPreviewDiv('tb_Master');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_ShippingAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddl_ShippingAddress.SelectedItem.Text != "Choose")
                    lbl_ShipingDetails.Text = Bind_Address(ddl_ShippingAddress.SelectedItem.Value);
                ScriptManager.RegisterStartupScript(Page, GetType(), "Enabletable2", "gotoPreviewDiv('tb_Master');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
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
        protected void rdo_DeliveryTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string strXmlShipList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"shippingmethod.xml");
                XmlDocument xmlShipList = new XmlDocument();

                xmlShipList.Load(strXmlShipList);
                if (xmlShipList != null)
                {
                    var dict = new Dictionary<string, string>();
                    DataTable dtShipList = new DataTable();
                    dtShipList.Columns.Add("Item", typeof(string));
                    //delivery method
                    if (rdo_DeliveryTerms.SelectedItem.Text == "OCEAN FREIGHT")
                    {
                        foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/ocean"))
                        {
                            dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                        }
                    }
                    else if (rdo_DeliveryTerms.SelectedItem.Text == "AIR FREIGHT")
                    {
                        foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/air"))
                        {
                            dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                        }
                    }
                    else if (rdo_DeliveryTerms.SelectedItem.Text == "ROAD WAY FREIGHT")
                    {
                        foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/road"))
                        {
                            dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                        }
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtShipList.Rows.Add(item.Value);
                        }
                        Utilities.rdb_Binding(rdo_DeliveryMethod, dtShipList, "item", "item");
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Enabletable3", "gotoPreviewDiv('tb_Master');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_Update_Records_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@BillID", ddl_BillingAddress.SelectedItem.Value), 
                        new SqlParameter("@ShipID", ddl_ShippingAddress.SelectedItem.Value), 
                        new SqlParameter("@FinalDestination", txt_FinalDestination.Text.ToUpper()),
                        new SqlParameter("@CountryOfDestination", txt_CountryDestination.Text.ToUpper()),
                        new SqlParameter("@PackingMethod", rdo_PackingMethod.SelectedItem.Text), 
                        new SqlParameter("@PackingOthers", txt_PackingOthers.Text), 
                        new SqlParameter("@DeliveryMethod", rdo_DeliveryTerms.SelectedItem.Value), 
                        new SqlParameter("@GodownName", rdo_DeliveryMethod.SelectedItem.Text), 
                        new SqlParameter("@TransportDetails", ""), 
                        new SqlParameter("@SplIns", txt_SplIns.Text.Replace("\r\n", "~")), 
                        new SqlParameter("@SpecialRequset", txt_SplReq.Text.Replace("\r\n", "~")), 
                        new SqlParameter("@DesiredShipDate", DateTime.ParseExact(txt_DesiredDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                        new SqlParameter("@OtherReqDocuments", txt_OtherDocuments.Text.Replace("\r\n", "~")), 
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value), 
                    };
                int resp = daCOTS.ExecuteNonQuery_SP("SP_UPD_ExpOrderRevise_MasterDetail", sp);
                if (resp > 0)
                    hdnMasterChangeStatus.Value = "1";
                Bind_ChoosedOrder();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnMasterChangeCompleted_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_upd_exp_masterchange_completed", sp);
                if (resp > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkFinalLoad_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow Row = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnCustCode.Value = ((HiddenField)Row.FindControl("hdnCustCode")).Value;
                lblSelectedOrderNo.Text = ((Label)Row.FindControl("lblOrderRefNo")).Text;
                lblSelectedCustomer.Text = ((Label)Row.FindControl("lblStatusCustName")).Text;
                hdnOID.Value = ((HiddenField)Row.FindControl("hdnOrderID")).Value;

                SqlParameter[] spC = new SqlParameter[] { new SqlParameter("@CustCode", hdnCustCode.Value), new SqlParameter("@OrderRefNo", lblSelectedOrderNo.Text) };
                DataTable dtConf = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_Exp_PdiStatus_For_ContainerLoad", spC, DataAccess.Return_Type.DataTable);
                if (dtConf.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "alettMsg", "alert('" + dtConf.Rows[0]["PdiPlant"].ToString() +
                        " COMPLETED ON " + dtConf.Rows[0]["loadedon"].ToString() + "');", true);
                }
                else
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                    DataTable dtSumList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_Order_SumValue_Logistics", sp1, DataAccess.Return_Type.DataTable);
                    if (dtSumList.Rows.Count > 0)
                    {
                        gv_OrderSumValue.DataSource = dtSumList;
                        gv_OrderSumValue.DataBind();

                        gv_OrderSumValue.FooterRow.Cells[6].Text = "Total";
                        gv_OrderSumValue.FooterRow.Cells[7].Text = dtSumList.Compute("Sum([TOT QTY])", "").ToString();
                        gv_OrderSumValue.FooterRow.Cells[8].Text = dtSumList.Compute("Sum([TOT PRICE])", "").ToString();
                        gv_OrderSumValue.FooterRow.Cells[9].Text = dtSumList.Compute("Sum([TOT WT])", "").ToString();

                        DataView dv_Plant = new DataView(dtSumList);
                        dv_Plant.Sort = "PLANT ASC";
                        DataTable dt_Plant = dv_Plant.ToTable(true, "PLANT");

                        if (dt_Plant.Rows.Count > 0)
                        {
                            ddlPlantList.DataSource = dt_Plant;
                            ddlPlantList.DataTextField = "PLANT";
                            ddlPlantList.DataValueField = "PLANT";
                            ddlPlantList.DataBind();

                            ddlPlantList.SelectedIndex = ddlPlantList.Items.IndexOf(ddlPlantList.Items.FindByValue(Row.Cells[5].Text));
                            ScriptManager.RegisterStartupScript(Page, GetType(), "Enabletable", "gotoPreviewDiv('tr_ContainerLoad');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnChangeFinalLoad_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spU = new SqlParameter[] { 
                    new SqlParameter("@CustCode", hdnCustCode.Value), 
                    new SqlParameter("@OrderRefNo", lblSelectedOrderNo.Text), 
                    new SqlParameter("@ContainerLoadFrom", ddlPlantList.SelectedItem.Text) 
                };
                int resp = (int)daCOTS.ExecuteNonQuery_SP("sp_upd_Exp_Container_Load", spU);
                if (resp > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                    resp = daCOTS.ExecuteNonQuery_SP("sp_upd_exp_masterchange_completed", sp);
                    if (resp > 0)
                        Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}