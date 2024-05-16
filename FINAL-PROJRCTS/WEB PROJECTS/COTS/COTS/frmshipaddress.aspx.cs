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
using System.Globalization;

namespace COTS
{
    public partial class frmshipaddress : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        FillRadionButtonList_From_XML();
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@custcode", Session["cotscode"].ToString()) };
                        DataTable dtBill = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_address_ForOrderEntry", sp, DataAccess.Return_Type.DataTable);
                        if (dtBill.Rows.Count > 0)
                        {
                            dlBillAddress.DataSource = dtBill;
                            dlBillAddress.DataBind();
                            if (dtBill.Rows.Count == 1)
                            {
                                ((RadioButton)dlBillAddress.Items[0].FindControl("rdbBill")).Checked = true;
                                Bind_BillAddress(0);
                            }
                        }

                        SqlParameter[] sp1 = new SqlParameter[1];
                        sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                        DataTable dtPay = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Payterms", sp1, DataAccess.Return_Type.DataTable);
                        if (dtPay.Rows.Count > 0)
                        {
                            txtPayTerms.Text = dtPay.Rows[0]["PaymentTerms"].ToString().Replace("~", "\r\n");
                            txtInstructions.Text = "\r\n" + dtPay.Rows[0]["CustInstruction"].ToString().Replace("~", "\r\n");
                        }

                        Get_PriceSheetRatesID();
                    }
                }
                else
                {
                    Response.Redirect("SessionExp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Get_PriceSheetRatesID()
        {
            try
            {
                if (Session["cotsstdcode"] != null)
                {
                    SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@custcode", Session["cotsstdcode"].ToString()) };
                    DataTable dtSheet = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_availablepricesheet", sp2, DataAccess.Return_Type.DataTable);
                    if (dtSheet.Rows.Count > 0)
                    {
                        string strSheetConcat = string.Empty;
                        string strRatesIDConcat = string.Empty;
                        foreach (DataRow row in dtSheet.Rows)
                        {
                            if (strSheetConcat == "")
                            {
                                strSheetConcat = row["PriceSheetRefNo"].ToString();
                                strRatesIDConcat = row["RatesID"].ToString();
                            }
                            else
                            {
                                strSheetConcat += "~" + row["PriceSheetRefNo"].ToString();
                                strRatesIDConcat += "~" + row["RatesID"].ToString();
                            }
                        }
                        hdnPriceSheet.Value = strSheetConcat;
                        hdnRatesID.Value = strRatesIDConcat;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
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
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
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
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
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

                    //mode of transport
                    dict.Clear();
                    dtShipList.Clear();
                    foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/modeoftransport"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtShipList.Rows.Add(item.Value);
                        }
                        rdbModeOfTransport.DataSource = dtShipList;
                        rdbModeOfTransport.DataTextField = "item";
                        rdbModeOfTransport.DataValueField = "item";
                        rdbModeOfTransport.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdbBill_IndexChaged(object sender, EventArgs e)
        {
            try
            {
                DataListItem row = (DataListItem)((RadioButton)sender).NamingContainer;
                for (int k = 0; k < dlBillAddress.Items.Count; k++)
                {
                    if (k != row.ItemIndex)
                        ((RadioButton)dlBillAddress.Items[k].FindControl("rdbBill")).Checked = false;
                }
                Bind_BillAddress(row.ItemIndex);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_BillAddress(int index)
        {
            try
            {
                hdnBillID.Value = ""; hdnShipID.Value = "";
                hdnBillID.Value = ((HiddenField)dlBillAddress.Items[index].FindControl("hdnBill")).Value;
                dlBillAddress.Items[index].CssClass = "billaddress";
                SqlParameter[] spShip = new SqlParameter[] { new SqlParameter("@custcode", Session["cotscode"].ToString()), new SqlParameter("@billid", hdnBillID.Value) };
                DataTable dtShip = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_shipaddress_ForOrderEntry", spShip, DataAccess.Return_Type.DataTable);
                if (dtShip.Rows.Count > 0)
                {
                    dlShipAddress.DataSource = dtShip;
                    dlShipAddress.DataBind();
                    if (dtShip.Rows.Count == 1)
                    {
                        ((RadioButton)dlShipAddress.Items[0].FindControl("rdbShip")).Checked = true;
                        hdnShipID.Value = ((HiddenField)dlShipAddress.Items[0].FindControl("hdnShip")).Value;
                        dlShipAddress.Items[0].CssClass = "shipaddress";
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdbShip_IndexChaged(object sender, EventArgs e)
        {
            try
            {
                hdnShipID.Value = "";
                DataListItem row = (DataListItem)((RadioButton)sender).NamingContainer;
                hdnShipID.Value = ((HiddenField)dlShipAddress.Items[row.ItemIndex].FindControl("hdnShip")).Value;
                dlShipAddress.Items[row.ItemIndex].CssClass = "shipaddress";
                for (int k = 0; k < dlShipAddress.Items.Count; k++)
                {
                    if (k != row.ItemIndex)
                        ((RadioButton)dlShipAddress.Items[k].FindControl("rdbShip")).Checked = false;
                }

                for (int m = 0; m < dlBillAddress.Items.Count; m++)
                {
                    if (((HiddenField)dlBillAddress.Items[m].FindControl("hdnBill")).Value == hdnBillID.Value)
                        dlBillAddress.Items[m].CssClass = "billaddress";
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
                sp2[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp2[1] = new SqlParameter("@orderrefno", txtOrderRefNo.Text);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_chk_orderrefno", sp2, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0 && dt.Rows[0]["OrderRefNo"].ToString() == txtOrderRefNo.Text && dt.Rows[0]["OrderRefNo"].ToString() != "")
                    lblErrMsg.Text = "ORDER NO ALREADY EXISTING";
                else
                {
                    SqlParameter[] sp1 = new SqlParameter[24];
                    sp1[0] = new SqlParameter("@CustCode", Session["cotscode"].ToString());
                    sp1[1] = new SqlParameter("@OrderRefNo", txtOrderRefNo.Text.Trim());
                    sp1[2] = new SqlParameter("@ShipID", hdnShipID.Value);
                    sp1[3] = new SqlParameter("@BillID", hdnBillID.Value);
                    sp1[4] = new SqlParameter("@StationaryReq", "");
                    sp1[5] = new SqlParameter("@StationaryOthers", "");
                    sp1[6] = new SqlParameter("@PackingMethod", rdbPackingMethod.SelectedItem.Text);
                    sp1[7] = new SqlParameter("@PackingOthers", txtPackingOthers.Text);
                    sp1[8] = new SqlParameter("@AgentType", "");
                    sp1[9] = new SqlParameter("@AgentOthers", "");
                    sp1[10] = new SqlParameter("@SplIns", txtSplIns.Text.Replace("\r\n", "~"));
                    sp1[11] = new SqlParameter("@Country", "");
                    sp1[12] = new SqlParameter("@DesiredShipDate", DateTime.ParseExact(txtDesiredDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    sp1[13] = new SqlParameter("@PriceRefNo", hdnPriceSheet.Value);
                    sp1[14] = new SqlParameter("@RatesID", hdnRatesID.Value);
                    sp1[15] = new SqlParameter("@TinNo", "");
                    sp1[16] = new SqlParameter("@CstNo", "");
                    sp1[17] = new SqlParameter("@FreightCharges", rdbFreightCharges.SelectedItem.Text);
                    sp1[18] = new SqlParameter("@DeliveryMethod", rdbDelivery.SelectedItem.Text);
                    sp1[19] = new SqlParameter("@GodownName", txtGodownName.Text);
                    sp1[20] = new SqlParameter("@TransportDetails", txtTransportDetails.Text.Replace("\r\n", "~"));
                    sp1[21] = new SqlParameter("@SpecialRequset", txtSplReq.Text.Replace("\r\n", "~"));
                    sp1[22] = new SqlParameter("@username", Session["cotsuser"].ToString());
                    sp1[23] = new SqlParameter("@ModeOfTransport", rdbModeOfTransport.SelectedItem.Text);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_OrderMasterDetails", sp1);
                    if (resp > 0)
                    {
                        if (Request["qno"] != null && Request["qyr"] != null)
                        {
                            SqlParameter[] sporder = new SqlParameter[4];
                            sporder[0] = new SqlParameter("@CustCode", Session["cotscode"].ToString());
                            sporder[1] = new SqlParameter("@refno", Request["qno"].ToString());
                            sporder[2] = new SqlParameter("@acyear", Request["qyr"].ToString());
                            sporder[3] = new SqlParameter("@orderid", txtOrderRefNo.Text.Trim());
                            resp = daCOTS.ExecuteNonQuery_SP("sp_ins_OrderItemList_fromShipment", sporder);
                            if (resp > 0)
                                Response.Redirect("frmproforma.aspx?qcomplete=" + Utilities.Encrypt(txtOrderRefNo.Text) + "&qno=" +
                                    Utilities.Encrypt(Request["qno"].ToString()) + "&qyr=" + Utilities.Encrypt(Request["qyr"].ToString()), false);
                        }
                        else
                            Response.Redirect("frmitementry.aspx?qno=" + Utilities.Encrypt(txtOrderRefNo.Text) + "&qtype=" + Utilities.Encrypt("new"), false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}