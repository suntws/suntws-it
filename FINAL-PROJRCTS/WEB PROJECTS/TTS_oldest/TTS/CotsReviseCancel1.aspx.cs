using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace TTS
{
    public partial class CotsReviseCancel1 : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dom_revise"].ToString() == "True" || dtUser.Rows[0]["dom_invoice"].ToString() == "True"))
                        {
                            if (Utilities.Decrypt(Request["type"].ToString()) == "eic")
                            {
                                lblHeadPage.Text = "DOMESTIC - IRN e-INVOICE CANCEL";
                                SqlParameter[] spI = new SqlParameter[] { new SqlParameter("@username", Request.Cookies["TTSUser"].Value) };
                                DataTable dteInv = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_eInvoice_cancel_list", spI, DataAccess.Return_Type.DataTable);
                                if (dteInv.Rows.Count > 0)
                                {
                                    gv_eInvoiceCancel.DataSource = dteInv;
                                    gv_eInvoiceCancel.DataBind();
                                }
                            }
                            else
                            {
                                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@RequestType", Utilities.Decrypt(Request["type"].ToString())) };
                                DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_processing_ForReviosion", sp, DataAccess.Return_Type.DataTable);
                                if (dtorderlist.Rows.Count > 0)
                                {
                                    gvReviseOrderList.DataSource = dtorderlist;
                                    gvReviseOrderList.DataBind();

                                    gvReviseOrderList.Columns[8].Visible = false;
                                    gvReviseOrderList.Columns[9].Visible = false;
                                    if (Utilities.Decrypt(Request["type"].ToString()) == "r")
                                    {
                                        gvReviseOrderList.Columns[8].Visible = true;
                                        lblHeadPage.Text = "DOMESTIC - ORDER REVISIONS";
                                    }
                                    else if (Utilities.Decrypt(Request["type"].ToString()) == "c")
                                    {
                                        gvReviseOrderList.Columns[9].Visible = true;
                                        lblHeadPage.Text = "DOMESTIC - ORDER CANCEL";
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
                    divCancelBtn.Visible = false;
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
        protected void lnkReviseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Response.Redirect("cotsorderrevise.aspx?oid=" + Utilities.Encrypt(((HiddenField)clickedRow.FindControl("hdnOrderID")).Value) + "&reqid=" +
                    Utilities.Encrypt(((HiddenField)clickedRow.FindControl("hdnRequestStatus")).Value) + "&cid=" +
                 Utilities.Encrypt(((HiddenField)clickedRow.FindControl("hdnOrderCustCode")).Value), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkCancelBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblCustName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblStausOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCotsCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnOrderCustCode")).Value;
                hdnPlant.Value = ((Label)clickedRow.FindControl("lblPlant")).Text;
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;
                divCancelBtn.Visible = true;
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(hdnOID.Value));
                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    ViewState["dtItemList"] = dtItemList;

                    gvOrderItemList.FooterRow.Cells[6].Text = "TOTAL";
                    gvOrderItemList.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    object sumCost = dtItemList.Compute("Sum(unitprice)", "");
                    gvOrderItemList.FooterRow.Cells[7].Text = Convert.ToDecimal(sumCost).ToString();

                    object sumQty = dtItemList.Compute("Sum(itemqty)", "");
                    gvOrderItemList.FooterRow.Cells[8].Text = sumQty.ToString();

                    object sumFwt = dtItemList.Compute("Sum(finishedwt)", "");
                    gvOrderItemList.FooterRow.Cells[9].Text = Convert.ToDecimal(sumFwt).ToString();

                    gvOrderItemList.Columns[10].Visible = false; //RIM QTY
                    gvOrderItemList.Columns[11].Visible = false; //RIM BASIC PRICE
                    gvOrderItemList.Columns[12].Visible = false; //RIM FWT
                    gvOrderItemList.Columns[13].Visible = false; //DWG NO.
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[10].Visible = true;
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;
                        gvOrderItemList.Columns[13].Visible = true;
                        break;
                    }
                }
                btnCotsOrderCancel.Focus();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCotsOrderCancel_Click(object sender, EventArgs e)
        {
            try
            {
                int resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), "6", txtCotsOrderCancelFeedBack.Text.Replace("\r\n", "~"),
                                    Request.Cookies["TTSUser"].Value);
                if (resp > 0)
                {
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (System.Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-COTSDB", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnk_eInvoice_click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            int intOID = Convert.ToInt32(((HiddenField)clickedRow.FindControl("hdn_OID")).Value);
            dom_eInvoice.eInvoiceRoot respJsonClass = dom_eInvoice.Dom_eInvoice_Irn(intOID, "CAN");
            if (respJsonClass.status == 1)
            {

                if (respJsonClass.IrnStatus.ToLower() == "can")
                {
                    SqlParameter[] spIns = new SqlParameter[] { 
                            new SqlParameter("@O_ID", intOID), 
                            new SqlParameter("@invoiceno", clickedRow.Cells[3].Text), 
                            new SqlParameter("@respstatus", respJsonClass.status), 
                            new SqlParameter("@respmessage", respJsonClass.message != null ? respJsonClass.message : ""), 
                            new SqlParameter("@uuid", respJsonClass.uuid != null ? respJsonClass.uuid : ""), 
                            new SqlParameter("@SignedQrCodeImgUrl", respJsonClass.SignedQrCodeImgUrl != null ? respJsonClass.SignedQrCodeImgUrl : ""), 
                            new SqlParameter("@Irn", respJsonClass.Irn != null ? respJsonClass.Irn : ""), 
                            new SqlParameter("@AckDt", respJsonClass.AckDt != null ? respJsonClass.AckDt : ""), 
                            new SqlParameter("@AckNo", respJsonClass.status == 1 ? respJsonClass.AckNo : 0), 
                            new SqlParameter("@invStatus", respJsonClass.Status != null ? respJsonClass.Status : ""), 
                            new SqlParameter("@SignedQRCode", respJsonClass.SignedQRCode != null ? respJsonClass.SignedQRCode : ""), 
                            new SqlParameter("@SignedInvoice", respJsonClass.SignedInvoice != null ? respJsonClass.SignedInvoice : ""), 
                            new SqlParameter("@IrnStatus", respJsonClass.IrnStatus != null ? respJsonClass.IrnStatus : ""), 
                            new SqlParameter("@EwbStatus", respJsonClass.EwbStatus != null ? respJsonClass.EwbStatus : ""), 
                            new SqlParameter("@EwbNo", respJsonClass.EwbNo != null ? respJsonClass.EwbNo : ""), 
                            new SqlParameter("@EwbDt", respJsonClass.EwbDt != null ? respJsonClass.EwbDt : ""), 
                            new SqlParameter("@EwbValidTill", respJsonClass.EwbValidTill != null ? respJsonClass.EwbValidTill : ""), 
                            new SqlParameter("@Remarks", respJsonClass.Remarks != null ? respJsonClass.Remarks : ""  ), 
                            new SqlParameter("@ErrorCode", respJsonClass.ErrorCode != null ? respJsonClass.ErrorCode : ""), 
                            new SqlParameter("@ErrorDetails", respJsonClass.ErrorDetails != null ? respJsonClass.ErrorDetails : ""), 
                            new SqlParameter("@InfoDtls", respJsonClass.InfoDtls != null ? respJsonClass.InfoDtls : ""), 
                            new SqlParameter("@Irp", respJsonClass.Irp != null ? respJsonClass.Irp : ""), 
                            new SqlParameter("@createdby", Request.Cookies["TTSUser"].Value) 
                        };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_eInvoiceResponse", spIns);
                    if (resp > 0)
                    {
                        SqlParameter[] spUpd = new SqlParameter[] { 
                                new SqlParameter("@Irn", respJsonClass.Irn), 
                                new SqlParameter("@invoiceno", clickedRow.Cells[3].Text), 
                                new SqlParameter("@O_ID", intOID) 
                            };
                        int respUpd = daCOTS.ExecuteNonQuery_SP("sp_upd_Irn_invoicecancel", spUpd);
                        if (respUpd > 0)
                        {
                            string strRemarks = ((TextBox)clickedRow.FindControl("txt_eInvoiceCancelRemarks")).Text;
                            resp = DomesticScots.ins_dom_StatusChangedDetails(intOID, "7", "e-INVOCIE CANCEL: " + strRemarks.Replace("\r\n", "~"),
                                Request.Cookies["TTSUser"].Value);
                            if (resp > 0)
                                Response.Redirect(Request.RawUrl, false);
                        }
                    }
                }
            }
        }
    }
}