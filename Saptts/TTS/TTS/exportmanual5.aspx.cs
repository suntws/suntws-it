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
    public partial class exportmanual5 : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_orderentry"].ToString() == "True")
                        {
                            Bind_OrderMasterDetails();
                            FillRadionButtonList_From_XML();
                            Bind_BillingAddress();
                            Bind_masterdocuments();

                            Bind_OrderItem();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                }
                else Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
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
                        Utilities.rdb_Binding(rdbPackingMethod, dtShipList, "item", "item");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderItem()
        {
            try
            {
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(Utilities.Decrypt(Request["oid"].ToString())));
                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    gvOrderItemList.FooterRow.Cells[7].Text = "Total";
                    gvOrderItemList.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                    gvOrderItemList.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gvOrderItemList.FooterRow.Cells[9].Text = Convert.ToDecimal(dtItemList.Compute("Sum(unitpricepdf)", "")).ToString();
                    gvOrderItemList.FooterRow.Cells[10].Text = Convert.ToDecimal(dtItemList.Compute("Sum(totalfwt)", "")).ToString();

                    gvOrderItemList.Columns[11].Visible = false;
                    gvOrderItemList.Columns[12].Visible = false;
                    gvOrderItemList.Columns[13].Visible = false;
                    gvOrderItemList.Columns[14].Visible = false;
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;
                        gvOrderItemList.Columns[13].Visible = true;
                        gvOrderItemList.Columns[14].Visible = true;

                        gvOrderItemList.FooterRow.Cells[12].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                        gvOrderItemList.FooterRow.Cells[13].Text = Convert.ToDecimal(dtItemList.Compute("Sum(Rimpricepdf)", "")).ToString();
                        gvOrderItemList.FooterRow.Cells[14].Text = Convert.ToDecimal(dtItemList.Compute("Sum(totalRimWt)", "")).ToString();
                        break;
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
        private void Bind_BillingAddress()
        {
            try
            {
                ddlBillingAddress.DataSource = null;
                ddlBillingAddress.DataBind();
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlBillingAddress, dtBillAddress, "ShipAddress", "shipid", "Choose");
                if (ddlBillingAddress.Items.Count == 2)
                {
                    ddlBillingAddress.SelectedIndex = 1;
                    ddlBillingAddress_IndexChange(null, EventArgs.Empty);
                }
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
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString())) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ExpOrderMasterDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    hdnCustCode.Value = row["custcode"].ToString();
                    lblCustomer.Text = row["customer"].ToString();
                    lblUserCurrency.Text = row["usercurrency"].ToString();
                    lblPayMentDetails.Text = row["ExpPayMode"].ToString() + (row["ExpPayTime"].ToString() != "" ? (" /" + row["ExpPayTime"].ToString()
                        + (row["ExpPayTime"].ToString() != "" ? ("(" + row["ExpPayTime"].ToString() + ")") : "")) : "");
                    txtDesiredDate.Text = row["DesiredShipDate"].ToString();
                    txtSplIns.Text = row["SplIns"].ToString().Replace("~", "\r\n");
                    txtSplReq.Text = row["SpecialRequset"].ToString().Replace("~", "\r\n");
                    Utilities.selectedListItem_Find(ddlBillingAddress, row["BillID"].ToString(), "VALUE");
                    Utilities.selectedListItem_Find(ddlShippingAddress, row["ShipID"].ToString(), "VALUE");
                    lblBillAddress.Text = Bind_Address(row["BillID"].ToString());
                    lblShipDetails.Text = Bind_Address(row["ShipID"].ToString());
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
                if (ddlBillingAddress.SelectedItem.Text != "Choose")
                {
                    ddlShippingAddress.DataSource = "";
                    ddlShippingAddress.DataBind();
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@BillID", Convert.ToInt32(ddlBillingAddress.SelectedItem.Value)) };

                    lblBillAddress.Text = Bind_Address(ddlBillingAddress.SelectedItem.Value);
                    DataTable dtShippAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ShipList_BillID", sp1, DataAccess.Return_Type.DataTable);
                    Utilities.ddl_Binding(ddlShippingAddress, dtShippAddress, "ShipAddress", "shipid", "Choose");
                    if (ddlShippingAddress.Items.Count == 2)
                    {
                        ddlShippingAddress.SelectedIndex = 1;
                        ddlShippingAddress_SelectedIndexChanged(sender, e);
                    }
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
                    else if (rdbDeliveryTerms.SelectedItem.Text == "ROAD WAY FREIGHT")
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
                        Utilities.rdb_Binding(rdbDeliveryMethod, dtShipList, "item", "item");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSendOrder_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[11];
                sp1[0] = new SqlParameter("@O_ID", Utilities.Decrypt(Request["oid"].ToString()));
                sp1[1] = new SqlParameter("@PackingMethod", rdbPackingMethod.SelectedItem.Text);
                sp1[2] = new SqlParameter("@PackingOthers", txtPackingOthers.Text);
                sp1[3] = new SqlParameter("@DeliveryMethod", rdbDeliveryTerms.SelectedItem.Value);
                sp1[4] = new SqlParameter("@GodownName", rdbDeliveryMethod.SelectedItem.Text);
                sp1[5] = new SqlParameter("@SplIns", txtSplIns.Text.Replace("\r\n", "~"));
                sp1[6] = new SqlParameter("@SpecialRequset", txtSplReq.Text.Replace("\r\n", "~"));
                sp1[7] = new SqlParameter("@DesiredShipDate", DateTime.ParseExact(txtDesiredDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                sp1[8] = new SqlParameter("@OtherReqDocuments", txtOtherDocuments.Text.Replace("\r\n", "~"));
                sp1[9] = new SqlParameter("@FinalDestination", txtFinalDestination.Text.ToUpper());
                sp1[10] = new SqlParameter("@CountryOfDestination", txtCoutryDestination.Text.ToUpper());
                int resp = daCOTS.ExecuteNonQuery_SP("sp_edit_ExpOrderMasterDetails", sp1);
                if (resp > 0)
                    Response.Redirect("exportmanual1.aspx", false);
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
        protected void ddlShippingAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
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