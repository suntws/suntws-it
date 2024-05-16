using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace COTS
{
    public partial class frmtrackorder : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                {
                    if (!IsPostBack)
                        bind_OrderList();
                }
                else
                    Response.Redirect("SessionExp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_OrderList()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Desc_orderrefno", sp1, DataAccess.Return_Type.DataTable);
                if (dtList.Rows.Count > 0)
                {
                    gv_TrackOrderList.DataSource = dtList;
                    gv_TrackOrderList.DataBind();
                }

                if (Session["trackorder"] != null && Session["hdnOID"] != null && Session["trackorder"].ToString() != "" && Session["hdnOID"].ToString() != "")
                {
                    hdnOrderNo.Value = Session["trackorder"].ToString();
                    hdnOID.Value = Session["hdnOID"].ToString();

                    Bind_OrderWiseItemList();
                    Bind_OrderMasterDetails();

                    Bind_TrackOrderNoList();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkTrackOrder_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = ((LinkButton)sender);
                hdnOID.Value = btn.CommandArgument.ToString();
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblOrderReceiveddt.Text = clickedRow.Cells[1].Text;
                hdnOrderNo.Value = clickedRow.Cells[0].Text;

                Bind_OrderWiseItemList();
                Bind_OrderMasterDetails();
                if (Session["cotsstdcode"].ToString() != "DE0048")
                    Bind_TrackOrderNoList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_TrackOrderNoList()
        {
            try
            {
                lblProformaPrepareddt.Text = "";
                lblOrderConfirmeddt.Text = "";
                lblWorkOrderdt.Text = "";
                lblProdCompletedt.Text = "";
                lblInspectdt.Text = "";
                lblRFDByPPCdt.Text = "";
                lblDispatchdt.Text = "";
                lblTentVesseldt.Text = "";
                lblActVesseldt.Text = "";
                lblDocMaildt.Text = "";
                lblDocCourierdt.Text = "";
                lblPaymentReceiveddt.Text = "";
                lblDeliverydt.Text = "";

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataSet dslist = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_orderstatus_for_trackstyle", sp, DataAccess.Return_Type.DataSet);
                if (dslist.Tables[0].Rows.Count > 0)
                {
                    lblReviseCount.Text = ("ORDER NO: " + ((Label)dlOrderMaster.Items[0].FindControl("lblOrderNo")).Text) + ((dslist.Tables[1].Rows[0]["reviseno"].ToString() != "0" ||
                        dslist.Tables[1].Rows[0]["reviseno"].ToString() != "") ? " REV NO: " + dslist.Tables[1].Rows[0]["reviseno"].ToString() : "");
                    foreach (DataRow row in dslist.Tables[0].Rows)
                    {
                        string date = row["StatusDate"].ToString();
                        int statusid = date != "" ? Convert.ToInt32(row["TrackID"].ToString()) : 0;

                        if (statusid == 1)
                            lblOrderReceiveddt.Text = date.ToString();
                        else if (statusid == 2)
                            lblProformaPrepareddt.Text = date.ToString();
                        else if (statusid == 3)
                            lblOrderConfirmeddt.Text = date.ToString();
                        else if (statusid == 4)
                            lblWorkOrderdt.Text = date.ToString();
                        else if (statusid == 5)
                            lblProdCompletedt.Text = date.ToString();
                        else if (statusid == 6)
                            lblInspectdt.Text = date.ToString();
                        else if (statusid == 7)
                            lblRFDByPPCdt.Text = date.ToString();
                        else if (statusid == 8)
                            lblDispatchdt.Text = date.ToString();
                        else if (statusid == 9)
                        {
                            lblTentVesseldt.Text = date.ToString();
                            statusid = lblDispatchdt.Text != "" ? statusid : 0;
                        }
                        else if (statusid == 10)
                        {
                            lblActVesseldt.Text = date.ToString();
                            statusid = lblDispatchdt.Text != "" ? statusid : 0;
                        }
                        else if (statusid == 11)
                            lblDocMaildt.Text = date.ToString();
                        else if (statusid == 12)
                            lblDocCourierdt.Text = date.ToString();
                        else if (statusid == 13)
                            lblPaymentReceiveddt.Text = date.ToString();
                        else if (statusid == 14)
                            lblDeliverydt.Text = date.ToString();

                        ScriptManager.RegisterStartupScript(Page, GetType(), ("text" + statusid), "lnkTrackBtn_Click(" + statusid + ");", true);
                    }

                    ScriptManager.RegisterStartupScript(Page, GetType(), "divShow", "popup_open();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_TrackOrderList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                bind_OrderList();
                gv_TrackOrderList.PageIndex = e.NewPageIndex;
                gv_TrackOrderList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public string Bind_BillingAddress(string BillID)
        {
            string strAddress = string.Empty;
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@addressid", BillID) };
                DataTable dtAddressList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_OrderAddressDetails", sp1, DataAccess.Return_Type.DataTable);

                if (dtAddressList.Rows.Count > 0)
                {
                    DataRow row = dtAddressList.Rows[0];
                    strAddress += "<div style='font-weight:bold;'>" + row["CompanyName"].ToString() + "</div>";
                    strAddress += "<div>" + row["shipaddress"].ToString().Replace("~", "<br/>") + "</div>";
                    strAddress += "<div>" + row["city"].ToString() + "</div>";
                    strAddress += "<div>" + row["statename"].ToString() + "</div>";
                    strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
                    strAddress += "<div>" + row["contact_name"].ToString() + "</div>";
                    strAddress += "<div>" + row["mobile"].ToString() + "</div>";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAddress;
        }
        private void Bind_OrderWiseItemList()
        {
            try
            {
                gvOrderItemList.DataSource = null;
                gvOrderItemList.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@custcode", Session["cotscode"].ToString()), new SqlParameter("@orderrefno", hdnOrderNo.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cust_track_orderitemlist", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    gvOrderItemList.FooterRow.Cells[7].Text = "TOTAL";
                    gvOrderItemList.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gvOrderItemList.FooterRow.Cells[9].Text = Session["cotsstdcode"].ToString() == "DE0048" ?
                        Math.Round(Convert.ToDecimal(dtItemList.Compute("Sum(totprice)", ""))).ToString() : dtItemList.Compute("Sum(totprice)", "").ToString();
                    gvOrderItemList.FooterRow.Cells[10].Text = dtItemList.Compute("Sum(totwt)", "").ToString();
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
                gv_DownloadFiles.DataSource = null;
                gv_DownloadFiles.DataBind();
                dlOrderMaster.DataSource = null;
                dlOrderMaster.DataBind();
                imgdownload.Style.Add("display", "none");

                DataTable dtMasterList = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                if (Session["cotsstdcode"].ToString() == "DE0048")
                    dtMasterList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_TrackOrder_MasterDetails", sp1, DataAccess.Return_Type.DataTable);
                else
                    dtMasterList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_OrderAllDetails_customer", sp1, DataAccess.Return_Type.DataTable);
                if (dtMasterList.Rows.Count > 0)
                {
                    dlOrderMaster.DataSource = dtMasterList;
                    dlOrderMaster.DataBind();

                    if (Session["cotsstdcode"].ToString() == "DE0048")
                        ((Label)dlOrderMaster.Items[0].FindControl("lblTypeOf")).Text = "FREIGHT CHARGES";

                    if (Convert.ToInt32(dtMasterList.Rows[0]["OrderStatus"].ToString()) != 1 && Convert.ToInt32(dtMasterList.Rows[0]["OrderStatus"].ToString()) != 6)
                    {
                        SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                        DataTable dtFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_customer_AttachPdfFiles", sp2, DataAccess.Return_Type.DataTable);
                        if (dtFile.Rows.Count > 0)
                        {
                            gv_DownloadFiles.DataSource = dtFile;
                            gv_DownloadFiles.DataBind();
                            imgdownload.Style.Add("display", "block");
                        }
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "gotoShowDiv('divItemDetails');", true);
                    if (Convert.ToInt32(dtMasterList.Rows[0]["OrderStatus"].ToString()) == 2)
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoShowDiv('divStatusButton');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_DownloadFiles_ItemCommand(object source, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)source).NamingContainer as GridViewRow;
                Label FileType = (Label)clickedRow.FindControl("lblFileType");
                LinkButton lnkpdfFileName = (LinkButton)clickedRow.FindControl("lnkPdfFileName");
                string Url = "";
                switch (FileType.Text)
                {
                    case "PROFORMA FILE":
                        Url = Server.MapPath("~/proformafiles/" + Session["cotscode"].ToString() + "/") + lnkpdfFileName.Text;
                        break;
                    case "WORKORDER FILE":
                        Url = Server.MapPath("~/workorderfiles/" + Session["cotscode"].ToString() + "/") + lnkpdfFileName.Text;
                        break;
                    case "UPLOAD LR COPY":
                        Url = Server.MapPath("~/lrcopyfiles/" + Session["cotscode"].ToString() + "/") + lnkpdfFileName.Text;
                        break;
                    case "DUPLICATE FOR TRANSPORTER":
                    case "ORIGINAL FOR RECEPIENT":
                    case "TRIPLICATE FOR SUPPLIER":
                    case "PDI LIST":
                        Url = Server.MapPath("~/invoicefiles/" + Session["cotscode"].ToString() + "/") + lnkpdfFileName.Text;
                        break;
                    case "POST SHIPMENT DOCUMENTS":
                        Url = Server.MapPath("~/PostShipmentDetails/" + Session["cotscode"].ToString() + "/") + lnkpdfFileName.Text;
                        break;
                }
                Url = Url.Replace("SCOTS", "pdfs");
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkpdfFileName.Text);
                Response.WriteFile(Url);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnStatusChange_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = 0;
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@custcode", Session["cotscode"].ToString()), 
                    new SqlParameter("@statusid", Convert.ToInt32("3")), 
                    new SqlParameter("@feedback", txtOrderChangeComments.Text.Replace("\r\n", "~")), 
                    new SqlParameter("@username", "CUSTOMER") 
                };
                if (Session["cotsstdcode"].ToString() != "DE0048")
                    resp = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                else
                    resp = daCOTS.ExecuteNonQuery_SP("sp_ins_dom_StatusChangedDetails", sp1);
                if (resp > 0)
                {
                    SqlParameter[] spMail = new SqlParameter[1];
                    spMail[0] = new SqlParameter("@ID", Convert.ToInt32(3));
                    DataTable dtMail = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_DomesticEmailAlert_IDWise", spMail, DataAccess.Return_Type.DataTable);
                    if (dtMail.Rows.Count > 0)
                    {
                        DataRow mRow = dtMail.Rows[0];
                        StringBuilder mailConcat = new StringBuilder();
                        mailConcat.Append("PROFORMA CONFIRMED FROM: " + Session["cotsuserfullname"].ToString() + "<br/>");
                        mailConcat.Append("ORDER REF NO.: " + hdnOrderNo.Value + "<br/>");
                        mailConcat.Append("PROFORMA CONFIRMED. PLEASE RELEASE WORK ORDER AFTER INSERTING SHIP DATE AND PACKING METHOD<br/><br/>");
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