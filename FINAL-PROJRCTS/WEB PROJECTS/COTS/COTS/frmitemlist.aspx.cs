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
using System.Text;

namespace COTS
{
    public partial class frmitemlist : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                    {
                        if (Request["qno"] != null && Request["qno"].ToString() != "")
                        {
                            hdnOrderNo.Value = Request["qno"].ToString();
                            Bind_OrderWiseItemList(hdnOrderNo.Value);
                            Bind_OrderMasterDetails();
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
        }

        private void Bind_OrderWiseItemList(string strorderrefno)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@orderrefno", strorderrefno);

                DataTable dtItemList = new DataTable();
                dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_customer_order_itemlist", sp1, DataAccess.Return_Type.DataTable);

                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    object sumQty;
                    sumQty = dtItemList.Compute("Sum(itemqty)", "");
                    txtTotQty.Text = sumQty.ToString();
                    object sumWt;
                    sumWt = dtItemList.Compute("Sum(finishedwt)", "");
                    txtTotWeight.Text = sumWt.ToString();
                    object sumCost;
                    sumCost = dtItemList.Compute("Sum(unitprice)", "");
                    txtTotCost.Text = Session["cotscur"].ToString().ToLower() == "inr" ? Math.Round(Convert.ToDecimal(sumCost)).ToString() : sumCost.ToString();
                    object sumVol;
                    sumVol = dtItemList.Compute("Sum(loadingwt)", "");
                    double strVolume = Convert.ToDouble(sumVol.ToString()) / 0.9;
                    txtTotVolume.Text = strVolume.ToString("0") + " %";
                }
                else
                    lblErrMsg.Text = "No records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_OrderMasterDetails()
        {
            SqlParameter[] sp1 = new SqlParameter[2];
            sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
            sp1[1] = new SqlParameter("@orderrefno", hdnOrderNo.Value);

            DataTable dtMasterList = new DataTable();
            dtMasterList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderAllDetails", sp1, DataAccess.Return_Type.DataTable);

            if (dtMasterList.Rows.Count > 0)
            {
                dlOrderMaster.DataSource = dtMasterList;
                dlOrderMaster.DataBind();

                int stratudID = Convert.ToInt32(dtMasterList.Rows[0]["OrderStatus"].ToString());
                if (stratudID == 2)
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "display_divStatusButton();", true);

                lnkInvoiceFile.Text = "";
                lblInvoiceTxt.Text = "";
                lblProformaTxt.Text = "";
                lnkProformaFile.Text = "";
                if (stratudID != 1 && stratudID != 6 && stratudID != 5)
                {
                    lnkProformaFile.Text = "proforma.pdf";
                    lblProformaTxt.Text = "PROFORMA: ";
                }
                if (stratudID != 1 && stratudID == 5)
                {
                    lnkInvoiceFile.Text = "invoice.pdf";
                    lblInvoiceTxt.Text = "ORIGINAL INVOICE: ";

                    if (Session["cotscur"].ToString().ToLower() == "inr")
                    {
                        SqlParameter[] sp2 = new SqlParameter[3];
                        sp2[0] = new SqlParameter("@CustCode", Session["cotscode"].ToString());
                        sp2[1] = new SqlParameter("@OrderRefNo", hdnOrderNo.Value);
                        sp2[2] = new SqlParameter("@FileType", "UPLOAD LR COPY");
                        DataTable dtLRFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AttachPdfFiles", sp2, DataAccess.Return_Type.DataTable);
                        if (dtLRFile.Rows.Count > 0)
                        {
                            lnkLRCopyFile.Text = dtLRFile.Rows[0]["AttachFileName"].ToString();
                            lblLRTxt.Text = "L/R COPY: ";
                        }
                    }
                    else if (Session["cotscur"].ToString().ToLower() != "inr")
                    {
                        lnkLRCopyFile.Text = "packinglist.pdf";
                    }
                }
            }
        }

        public string Bind_BillingAddress(string BillID)
        {
            string strAddress = string.Empty;
            SqlParameter[] sp1 = new SqlParameter[2];
            sp1[0] = new SqlParameter("@addressid", BillID);
            sp1[1] = new SqlParameter("@custcode", Session["cotscode"].ToString());
            DataTable dtAddressList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderAddressDetails", sp1, DataAccess.Return_Type.DataTable);

            if (dtAddressList.Rows.Count > 0)
            {
                DataRow row = dtAddressList.Rows[0];
                strAddress += "<div style='font-weight:bold;'>" + row["contact_name"].ToString() + "</div>";
                strAddress += "<div>" + row["shipaddress"].ToString().Replace("~", "<br/>") + "</div>";
                strAddress += "<div>" + row["city"].ToString() + "</div>";
                strAddress += "<div>" + row["statename"].ToString() + "</div>";
                strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
            }
            return strAddress;
        }

        protected void lnkProformaFile_Click(object sender, EventArgs e)
        {
            string serverURL = Server.MapPath("~/proformafiles/" + Session["cotscode"].ToString() + "/").Replace("SCOTS", "pdfs").Replace("COTS", "pdfs");
            string spdfPathTo = serverURL + hdnOrderNo.Value + ".pdf";

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + hdnOrderNo.Value + ".pdf");
            Response.WriteFile(spdfPathTo);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

            Bind_OrderWiseItemList(hdnOrderNo.Value);
            Bind_OrderMasterDetails();
        }

        protected void lnkInvoiceFile_Click(object sender, EventArgs e)
        {
            string spdfPathTo = string.Empty;
            string serverURL = Server.MapPath("~/invoicefiles/" + Session["cotscode"].ToString() + "/").Replace("SCOTS", "pdfs").Replace("COTS", "pdfs");
            if (Session["cotscur"].ToString().ToLower() == "inr")
            {
                spdfPathTo = serverURL + "Buyer_invoice_" + hdnOrderNo.Value + ".pdf";

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + "Buyer_invoice_" + hdnOrderNo.Value + ".pdf");
                Response.WriteFile(spdfPathTo);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (Session["cotscur"].ToString().ToLower() != "inr")
            {
                spdfPathTo = serverURL + "Invoice_" + hdnOrderNo.Value + ".pdf";

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + "Invoice_" + hdnOrderNo.Value + ".pdf");
                Response.WriteFile(spdfPathTo);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            Bind_OrderWiseItemList(hdnOrderNo.Value);
            Bind_OrderMasterDetails();
        }

        protected void lnkLRCopyFile_Click(object sender, EventArgs e)
        {
            string serverURL = string.Empty;
            string spdfPathTo = string.Empty;
            if (Session["cotscur"].ToString().ToLower() == "inr")
            {
                serverURL = Server.MapPath("~/lrcopyfiles/" + Session["cotscode"].ToString() + "/").Replace("SCOTS", "pdfs").Replace("COTS", "pdfs");
                spdfPathTo = serverURL + "/" + lnkLRCopyFile.Text;

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkLRCopyFile.Text);
                Response.WriteFile(spdfPathTo);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else if (Session["cotscur"].ToString().ToLower() != "inr")
            {
                serverURL = Server.MapPath("~/invoicefiles/" + Session["cotscode"].ToString() + "/").Replace("SCOTS", "pdfs").Replace("COTS", "pdfs");
                spdfPathTo = serverURL + "PackingList_" + hdnOrderNo.Value + ".pdf";

                Response.AddHeader("content-disposition", "attachment; filename=" + "PackingList_" + hdnOrderNo.Value + ".pdf");
                Response.WriteFile(spdfPathTo);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            Bind_OrderWiseItemList(hdnOrderNo.Value);
            Bind_OrderMasterDetails();
        }

        protected void btnStatusChange_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@orderid", hdnOrderNo.Value);
                sp1[1] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[2] = new SqlParameter("@statusid", Convert.ToInt32("3"));
                sp1[3] = new SqlParameter("@feedback", txtOrderChangeComments.Text.Replace("\r\n", "~"));
                sp1[4] = new SqlParameter("@username", "CUSTOMER");

                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_StatusChangedDetails", sp1);
                if (resp > 0)
                {
                    SqlParameter[] spMail = new SqlParameter[1];
                    spMail[0] = new SqlParameter("@ID", Convert.ToInt32(3));
                    DataTable dtMail = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_DomesticEmailAlert_IDWise", spMail, DataAccess.Return_Type.DataTable);
                    if (dtMail.Rows.Count > 0)
                    {
                        DataRow mRow = dtMail.Rows[0];
                        StringBuilder mailConcat = new StringBuilder();
                        mailConcat.Append("ORDER CONFIRMED FROM: " + Session["cotsuserfullname"].ToString() + "<br/>");
                        mailConcat.Append("ORDER REF NO.: " + hdnOrderNo.Value + "<br/>");
                        mailConcat.Append("DOMESTIC ORDER CONFIRMED. PLEASE RELEASE WORK ORDER AFTER INSERTING SHIP DATE AND PACKING METHOD<br/><br/>");
                        mailConcat.Append("TRACK THIS ORDER: <a href=\"http://www.suntws.com/tts/cotsproductiontodispatch.aspx?cid=" + Session["cotscode"].ToString() + "&coid=" + hdnOrderNo.Value + "\">CLICK HERE</a>");
                        Utilities.CotsOrderMailSent(mailConcat.ToString(), "PLEASE RELEASE WORKORDER", mRow["MailReceipent"].ToString(), mRow["MailCC"].ToString());
                    }
                    Response.Redirect("frmmsgdisplay.aspx?msgtype=confirmedmsg", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}