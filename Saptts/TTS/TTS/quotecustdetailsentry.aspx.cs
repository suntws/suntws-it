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
using System.Globalization;

namespace TTS
{
    public partial class quotecustdetailsentry : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    tbQuote.Style.Add("display", "none");
                    tbProforma.Style.Add("display", "none");
                    if (Request["acy"] != null && Request["acref"] != null)
                    {
                        tbQuote.Style.Add("display", "block");
                        Bind_Quote_Confirmation();
                    }
                    else if (Request["prid"] != null && Request["prref"] != null)
                    {
                        tbProforma.Style.Add("display", "block");
                        Bind_Proforma_Confirmation();
                    }
                    else
                    {
                        divMsgBox.Style.Add("display", "none");
                        divEntryBox.Style.Add("display", "none");
                        lblErrMsg.Text = "Error: URL is wrong. Please contact SUN-TYRE & WHEEL SYSTEMS";
                    }
                }
                catch (Exception ex)
                {
                    lblErrMsg.Text += "Error: " + ex.Message;
                }
            }
        }
        private void Bind_Quote_Confirmation()
        {
            try
            {
                lblQuoteNo.Text = "OFFER: " + Request["acy"].ToString() + "/" + Request["acref"].ToString();
                string StrQtype = Request["qtype"].ToString();
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@QAcYear", Request["acy"].ToString());
                sp2[1] = new SqlParameter("@QRefNo", Request["acref"].ToString());

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Chk_QuoteConfirmation", sp2, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    lblQuoteReviseNo.Text = dt.Rows[0]["QRevisedCount"].ToString();
                    txtCustName.Text = dt.Rows[0]["QCustomer"].ToString();
                    txtQEmail.Text = dt.Rows[0]["QEmail"].ToString();
                    txtQcc.Text = dt.Rows[0]["QCC"].ToString();
                    hdnTtsUsername.Value = dt.Rows[0]["UserName"].ToString();
                    divMsgBox.Style.Add("display", "none");
                    divEntryBox.Style.Add("display", "none");
                    DateTime sysDate = DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    DateTime expDate = DateTime.ParseExact(dt.Rows[0]["ExpiredDate"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    if (expDate < sysDate)
                    {
                        lblErrMsg.Text = "Quote already expired on " + dt.Rows[0]["ExpiredDate"].ToString();
                        divMsgBox.Style.Add("display", "block");
                    }
                    if (dt.Rows[0]["ConfirmedDate"].ToString() != "")
                    {
                        lblErrMsg.Text = "Quote already confirmed on " + dt.Rows[0]["ConfirmedDate"].ToString();
                        divMsgBox.Style.Add("display", "block");
                    }
                    else
                    {
                        if (Request["qtype"] == "new")
                        {
                            divMsgBox.Style.Add("display", "none");
                            divEntryBox.Style.Add("display", "block");
                            SqlParameter[] sp1 = new SqlParameter[2];
                            sp1[0] = new SqlParameter("@QAcYear", Request["acy"].ToString());
                            sp1[1] = new SqlParameter("@QRefNo", Request["acref"].ToString());
                            DataTable dtDetails = (DataTable)daCOTS.ExecuteReader_SP("SP_SEL_QuotePrepare_NewCustAddress", sp1, DataAccess.Return_Type.DataTable);
                            if (dtDetails.Rows.Count > 0)
                            {
                                DataRow dr = dtDetails.Rows[0];
                                txtBillCompanyName.Text = dr["BillCompanyName"].ToString();
                                txtBillAddress.Text = dr["BillAddress"].ToString();
                                txtBillCity.Text = dr["BillCity"].ToString();
                                txtBillContactName.Text = dr["BillContactName"].ToString();
                                txtBillContactNo.Text = dr["BillContactNo"].ToString();
                                txtBillGSTNo.Text = dr["BillGST_No"].ToString();
                                txtBillPincode.Text = dr["BillZipCode"].ToString();
                                txtBillState.Text = dr["BillStateName"].ToString();
                                txtBillStateCode.Text = dr["BillStateCode"].ToString();

                                txtShipCompanyName.Text = dr["ShipCompanyName"].ToString();
                                txtShipAddress.Text = dr["ShipAddress"].ToString();
                                txtShipCity.Text = dr["ShipCity"].ToString();
                                txtShipContactName.Text = dr["ShipContactName"].ToString();
                                txtShipContactNo.Text = dr["ShipContactNo"].ToString();
                                txtShipGSTNo.Text = dr["ShipGST_No"].ToString();
                                txtShipPincode.Text = dr["ShipZipCode"].ToString();
                                txtShipState.Text = dr["ShipStateName"].ToString();
                                txtShipStateCode.Text = dr["ShipStateCode"].ToString();
                            }
                        }
                        else if (Request["qtype"] == "exist")
                        {
                            sp_edit();

                            divMsgBox.Style.Add("display", "block");
                            lblSuccessMsg.Text = "Thank For Your Conformation";
                            lblSuccessMsg.Font.Size = 20;
                        }
                    }
                }
                else
                {
                    divMsgBox.Style.Add("display", "none");
                    divEntryBox.Style.Add("display", "none");
                    lblErrMsg.Text = "Error: URL is wrong. Please contact SUN-TYRE & WHEEL SYSTEMS";
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text += "Error: " + ex.Message;
            }
        }
        protected void lnkQuotePdf_Click(object sender, EventArgs e)
        {
            try
            {
                string serverUrl = Server.MapPath("~/quote/").Replace("TTS", "pdfs");
                string path = serverUrl + "/" + Request["acy"].ToString() + "_" + Request["acref"].ToString() + "_" + lblQuoteReviseNo.Text + ".pdf";
                string lnkTxt = Request["acy"].ToString() + "_" + Request["acref"].ToString() + "_" + lblQuoteReviseNo.Text + ".pdf";

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (Request["qtype"] == "new")
                {

                    SqlParameter[] spAddress = new SqlParameter[]{
                        new SqlParameter("@AcYear",Request["acy"].ToString()),
                        new SqlParameter("@RefNo",Request["acref"].ToString()),
                        new SqlParameter("@CustFullName",txtCustName.Text),
                        new SqlParameter("@BillCompanyName",txtBillCompanyName.Text),
                        new SqlParameter("@BillAddress", txtBillAddress.Text),
                        new SqlParameter("@BillCity", txtBillCity.Text),
                        new SqlParameter("@BillStateName", txtBillState.Text),
                        new SqlParameter("@BillStateCode", txtBillStateCode.Text),
                        new SqlParameter("@BillGSTNo", txtBillGSTNo.Text),
                        new SqlParameter("@BillZipCode", txtBillPincode.Text),
                        new SqlParameter("@BillContactName", txtBillContactName.Text),
                        new SqlParameter("@BillContactNo", txtBillContactNo.Text),
                        new SqlParameter("@ShipCompanyName",txtShipCompanyName.Text),
                        new SqlParameter("@ShipAddress", txtShipAddress.Text),
                        new SqlParameter("@ShipCity", txtShipCity.Text),
                        new SqlParameter("@ShipStateName", txtShipState.Text),
                        new SqlParameter("@ShipStateCode", txtShipStateCode.Text),
                        new SqlParameter("@ShipGSTNo", txtShipGSTNo.Text),
                        new SqlParameter("@ShipZipCode", txtShipPincode.Text),
                        new SqlParameter("@ShipContactName", txtShipContactName.Text),
                        new SqlParameter("@ShipContactNo", txtShipContactNo.Text),
                        new SqlParameter("@CustComment",txtCustComments.Text.Replace("\r\n", "~"))
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("SP_UPD_QuoteAddressDetails", spAddress);
                    if (resp > 0)
                    {
                        sp_edit();
                        divMsgBox.Style.Add("display", "block");
                        divEntryBox.Style.Add("display", "none");
                        lblSuccessMsg.Text = "Details saved successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "Error: " + ex.Message;
            }
        }
        private void sp_edit()
        {
            SqlParameter[] sp2 = new SqlParameter[2];
            sp2[0] = new SqlParameter("@QAcYear", Request["acy"].ToString());
            sp2[1] = new SqlParameter("@QRefNo", Request["acref"].ToString());

            int resp1 = daCOTS.ExecuteNonQuery_SP("sp_edit_QuoteConfirmed", sp2);
            if (resp1 > 0)
            {
                divMsgBox.Style.Add("display", "block");
                divEntryBox.Style.Add("display", "none");
                Quote_ConfirmMail();
            }
        }
        private void Quote_ConfirmMail()
        {
            try
            {
                string mailConcat = string.Empty;
                mailConcat += "Dear Supplier,<br/><br/>";
                mailConcat += "     We hereby confirm the your quote & please arrange for supplies<br/>";

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@username", hdnTtsUsername.Value) };
                string strToMail = (string)daTTS.ExecuteScalar_SP("sp_sel_QuotePrepareMailID", sp1);
                if (!string.IsNullOrEmpty(txtQEmail.Text))
                {
                    YmailSender es = new YmailSender();
                    es.From = "s-cots_domestic@sun-tws.com";
                    es.To = strToMail;
                    string strCC = string.Empty;
                    if (txtQcc.Text != "")
                        strCC = "," + txtQcc.Text;
                    es.CC = txtQEmail.Text + strCC;
                    es.Password = "Y4K/HsD1";
                    es.Subject = "QUOTATION: " + Request["acy"].ToString() + "/" + Request["acref"].ToString();
                    es.Body = mailConcat + "<br/><br/><br/><span style='color:#d34639;'>This is a system generated mail. Please do not reply to this email ID.</span>";
                    es.IsHtmlBody = true;
                    es.EmailProvider = YmailSender.EmailProviderType.Gmail;
                    es.Send();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Quote Mail Error: " + ex.Message);
            }
        }

        private void Bind_Proforma_Confirmation()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Proforma Error: " + ex.Message);
            }
        }
    }
}