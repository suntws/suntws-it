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

namespace COTS
{
    public partial class expordermaster : System.Web.UI.Page
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
                        if (Request["eno"] != null && Utilities.Decrypt(Request["eno"].ToString()) != "")
                        {
                            lblOrderRefNo.Text = Utilities.Decrypt(Request["eno"].ToString());

                            FillRadionButtonList_From_XML();
                            Bind_PaymentTerms();
                            Bind_OrderMasterDetails();

                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@custcode", Session["cotscode"].ToString()) };
                            DataTable dtBill = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_address_ForOrderEntry", sp, DataAccess.Return_Type.DataTable);
                            if (dtBill.Rows.Count > 0)
                            {
                                dlBillAddress.DataSource = dtBill;
                                dlBillAddress.DataBind();
                                for (int m = 0; m < dlBillAddress.Items.Count; m++)
                                {
                                    if (((HiddenField)dlBillAddress.Items[m].FindControl("hdnBill")).Value == hdnBillID.Value)
                                    {
                                        ((RadioButton)dlBillAddress.Items[m].FindControl("rdbBill")).Checked = true;
                                        Bind_BillAddress(m);
                                    }
                                }
                            }
                        }
                    }
                    else if (IsPostBack)
                    {
                        for (int m = 0; m < dlBillAddress.Items.Count; m++)
                        {
                            if (((HiddenField)dlBillAddress.Items[m].FindControl("hdnBill")).Value == hdnBillID.Value)
                            {
                                ((RadioButton)dlBillAddress.Items[m].FindControl("rdbBill")).Checked = true;
                                dlBillAddress.Items[m].CssClass = "billaddress";
                            }
                        }

                        for (int m = 0; m < dlShipAddress.Items.Count; m++)
                        {
                            if (((HiddenField)dlShipAddress.Items[m].FindControl("hdnShip")).Value == hdnShipID.Value)
                            {
                                ((RadioButton)dlShipAddress.Items[m].FindControl("rdbShip")).Checked = true;
                                dlShipAddress.Items[m].CssClass = "shipaddress";
                            }
                        }
                        ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript1", "PackingMehtod();", true);
                    }
                }
                else
                    Response.Redirect("SessionExp.aspx", false);
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
                        rdbPackingMethod.DataSource = dtShipList;
                        rdbPackingMethod.DataTextField = "item";
                        rdbPackingMethod.DataValueField = "item";
                        rdbPackingMethod.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_PaymentTerms()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                DataTable dtPay = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Payterms", sp1, DataAccess.Return_Type.DataTable);
                if (dtPay.Rows.Count > 0)
                {
                    txtPayTerms.Text = dtPay.Rows[0]["PaymentTerms"].ToString().Replace("~", "\r\n");
                    txtInstructions.Text = "\r\n" + dtPay.Rows[0]["CustInstruction"].ToString().Replace("~", "\r\n");
                }

                SqlParameter[] sp3 = new SqlParameter[1];
                sp3[0] = new SqlParameter("@Custcode", Session["cotscode"].ToString());
                DataTable dtDoc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ExpDocumentMaster", sp3, DataAccess.Return_Type.DataTable);
                if (dtDoc.Rows.Count > 0)
                {
                    gv_DocList.DataSource = dtDoc;
                    gv_DocList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderMasterDetails()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@OrderRefNo", lblOrderRefNo.Text);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ExpOrderMasterDetails_cots", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtDesiredDate.Text = row["DesiredShipDate"].ToString();
                    txtSplIns.Text = row["SplIns"].ToString().Replace("~", "\r\n");
                    txtSplReq.Text = row["SpecialRequset"].ToString().Replace("~", "\r\n");
                    hdnBillID.Value = row["BillID"].ToString();
                    hdnShipID.Value = row["ShipID"].ToString();
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
                hdnShipID.Value = "";
                hdnBillID.Value = "";
                DataListItem row = (DataListItem)((RadioButton)sender).NamingContainer;
                hdnBillID.Value = ((HiddenField)dlBillAddress.Items[row.ItemIndex].FindControl("hdnBill")).Value;
                for (int k = 0; k < dlBillAddress.Items.Count; k++)
                {
                    if (k != row.ItemIndex)
                    {
                        ((RadioButton)dlBillAddress.Items[k].FindControl("rdbBill")).Checked = false;
                        dlBillAddress.Items[k].CssClass = "";
                    }
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

                    for (int m = 0; m < dlShipAddress.Items.Count; m++)
                    {
                        if (((HiddenField)dlShipAddress.Items[m].FindControl("hdnShip")).Value == hdnShipID.Value)
                        {
                            ((RadioButton)dlShipAddress.Items[m].FindControl("rdbShip")).Checked = true;
                            dlShipAddress.Items[m].CssClass = "shipaddress";
                        }
                        else
                        {
                            ((RadioButton)dlShipAddress.Items[m].FindControl("rdbShip")).Checked = false;
                            dlShipAddress.Items[m].CssClass = "";
                        }
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
                    {
                        ((RadioButton)dlShipAddress.Items[k].FindControl("rdbShip")).Checked = false;
                        dlShipAddress.Items[k].CssClass = "";
                    }
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
        protected void rdbDeliveryTerms_IndexChange(object sender, EventArgs e)
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
                    if (rdbDeliveryTerms.SelectedItem.Text == "OCEAN FREIGHT")
                    {
                        foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/ocean"))
                        {
                            dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                        }
                    }
                    else if (rdbDeliveryTerms.SelectedItem.Text == "AIR FREIGHT")
                    {
                        foreach (XmlNode xNode in xmlShipList.SelectNodes("/shipping/air"))
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
                        rdbDeliveryMethod.DataSource = dtShipList;
                        rdbDeliveryMethod.DataTextField = "item";
                        rdbDeliveryMethod.DataValueField = "item";
                        rdbDeliveryMethod.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSendOrder_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[15];
                sp1[0] = new SqlParameter("@CustCode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@OrderRefNo", lblOrderRefNo.Text);
                sp1[2] = new SqlParameter("@PackingMethod", rdbPackingMethod.SelectedItem.Text);
                sp1[3] = new SqlParameter("@PackingOthers", txtPackingOthers.Text);
                sp1[4] = new SqlParameter("@DeliveryMethod", rdbDeliveryTerms.SelectedItem.Text);
                sp1[5] = new SqlParameter("@GodownName", rdbDeliveryMethod.SelectedItem.Text);
                sp1[6] = new SqlParameter("@TransportDetails", txtDeliveryAddress.Text.Replace("\r\n", "~"));
                sp1[7] = new SqlParameter("@SplIns", txtSplIns.Text.Replace("\r\n", "~"));
                sp1[8] = new SqlParameter("@SpecialRequset", txtSplReq.Text.Replace("\r\n", "~"));
                try
                {
                    sp1[9] = new SqlParameter("@DesiredShipDate", DateTime.ParseExact(txtDesiredDate.Text, "dd/MM/yyyy", null));
                }
                catch (Exception ex)
                {
                    sp1[9] = new SqlParameter("@DesiredShipDate", DateTime.Parse(txtDesiredDate.Text));
                    Utilities.WriteToErrorLog("DATE FORMAT", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                }
                sp1[10] = new SqlParameter("@OtherReqDocuments", txtOtherDocuments.Text.Replace("\r\n", "~"));
                sp1[11] = new SqlParameter("@CountryOfDestination", txtCoutryDestination.Text);
                sp1[12] = new SqlParameter("@FinalDestination", txtFinalDestination.Text);
                sp1[13] = new SqlParameter("@ShipID", hdnShipID.Value);
                sp1[14] = new SqlParameter("@BillID", hdnBillID.Value);
                daCOTS.ExecuteNonQuery_SP("sp_edit_Exp_OrderMasterDetails", sp1);

                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp2[1] = new SqlParameter("@orderid", lblOrderRefNo.Text);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_edit_Change_completed_status", sp2);

                if (resp > 0)
                {
                    StringBuilder mailConcat = new StringBuilder();

                    SqlParameter[] sp4 = new SqlParameter[2];
                    sp4[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                    sp4[1] = new SqlParameter("@orderrefno", lblOrderRefNo.Text);
                    DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cust_proforma_orderitemlist", sp4, DataAccess.Return_Type.DataTable);
                    if (dtItemList.Rows.Count > 0)
                    {
                        string maketable = string.Empty;
                        foreach (DataRow row in dtItemList.Rows)
                        {
                            maketable += "<tr><td style='float:left;width:80px;'>" + row["tyretype"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:100px;'>" + row["brand"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:100px;'>" + row["sidewall"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:180px;'>" + row["tyresize"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:60px;'>" + row["rimsize"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:100px;'>" + row["unitprice"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:60px;'>" + row["itemqty"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:120px;'>" + row["totprice"].ToString() + "</td></tr>";
                        }
                        if (maketable.Length > 0)
                        {
                            mailConcat.Append("ORDER RECEIVED FROM: " + Session["cotsuserfullname"].ToString() + "<br/>");
                            mailConcat.Append("ORDER REF NO.:" + lblOrderRefNo.Text);
                            mailConcat.Append("<table border='1' cellspacing='0' rules='all' style='width:834px;border-collapse:collapse;'>");
                            mailConcat.Append("<tr style='text-align:center;font-weight:bold;background-color: #60FC79;'><td style='float:left;width:80px;'>TYPE</td>");
                            mailConcat.Append("<td style='float:left;width:100px;'>BRAND</td><td style='float:left;width:100px;'>SIDEWALL</td>");
                            mailConcat.Append("<td style='float:left;width:180px;'>SIZE</td><td style='float:left;width:60px;'>RIM</td>");
                            mailConcat.Append("<td style='float:left;width:100px;'>UNIT PRICE</td><td style='float:left;width:60px;'>QTY</td>");
                            mailConcat.Append("<td style='float:left;width:120px;'>TOTAL PRICE</td></tr>");
                            mailConcat.Append(maketable + "</table>");
                            mailConcat.Append("ORDER TOTAL WT: " + dtItemList.Compute("Sum(totwt)", "").ToString() + "<br/><br/>");
                        }
                    }
                    if (mailConcat.ToString().Length > 0)
                    {
                        string strToMail = Utilities.Build_CC_MailList(Session["cotscode"].ToString(), "");
                        Utilities.CotsOrderMailSent(mailConcat.ToString(), "ORDER RECEIVED", strToMail, Session["cotsmail"].ToString());
                    }
                    Response.Redirect("frmmsgdisplay.aspx?msgtype=ordercomplete&oid=" + lblOrderRefNo.Text, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}