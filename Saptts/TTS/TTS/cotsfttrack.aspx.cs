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

namespace TTS
{
    public partial class cotsfttrack : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["dom_invoice"].ToString() == "True" || dtUser.Rows[0]["dom_ordertrack"].ToString() == "True"))
                        {
                            lblHeadPlant.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper();

                            SqlParameter[] sp = new SqlParameter[1];
                            sp[0] = new SqlParameter("@Plant", lblHeadPlant.Text);
                            DataTable dtFtlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FtMasterDetailsForInvoice", sp, DataAccess.Return_Type.DataTable);
                            if (dtFtlist.Rows.Count > 0)
                            {
                                gvFtList.DataSource = dtFtlist;
                                gvFtList.DataBind();
                            }
                            else
                                lblErrMsg.Text = "No records";
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkInvoiceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectInvoiceFt(clickedRow);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectInvoiceFt(GridViewRow row)
        {
            try
            {
                Label lblFtNo = (Label)row.FindControl("lblFtNo");
                HiddenField hdnCustCode = (HiddenField)row.FindControl("hdnCustCode");
                HiddenField hdnBillId = (HiddenField)row.FindControl("hdnBillId");
                Label lblStatusCustName = (Label)row.FindControl("lblStatusCustName");
                HiddenField hdnTaxNo = (HiddenField)row.FindControl("hdnTaxNo");
                HiddenField hdnPanno = (HiddenField)row.FindControl("hdnPanno");
                HiddenField hdnComments = (HiddenField)row.FindControl("hdnComments");
                hdnCotsCustID.Value = hdnCustCode.Value;
                lblCustName.Text = lblStatusCustName.Text;
                lblStausOrderRefNo.Text = lblFtNo.Text;
                lbltaxno.Text = hdnTaxNo.Value == "" ? "" : "<span class='headCss'>CUSTOMER SERVICE TAX NO: </span>" + hdnTaxNo.Value + "<br/>";
                lblPanno.Text = hdnPanno.Value == "" ? "" : "<span class='headCss'>CUSTOMER PAN NO: </span>" + hdnPanno.Value + "<br/>";
                lblComments.Text = hdnComments.Value == "" ? "" : "<span class='headCss'>COMMENTS / INSTRCUTION: </span><br/><span style='line-height:12px;'>" + hdnComments.Value.Replace("~", "<br/>") + "</span>";
                lblbillTo.Text = Get_Address_CustChoice(hdnBillId.Value);

                gvItemlist.DataSource = null;
                gvItemlist.DataBind();
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@FtNo", lblFtNo.Text);
                sp1[2] = new SqlParameter("@Plant", lblHeadPlant.Text);
                DataTable dtotherFtNo = (DataTable)daCOTS.ExecuteReader_SP("Sp_Sel_FtMaster_track", sp1, DataAccess.Return_Type.DataTable);
                if (dtotherFtNo.Rows.Count > 0)
                {
                    gvItemlist.DataSource = dtotherFtNo;
                    gvItemlist.DataBind();
                }

                gvdispatchitem.DataSource = null;
                gvdispatchitem.DataBind();
                sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@FtNo", lblFtNo.Text);
                sp1[2] = new SqlParameter("@Plant", lblHeadPlant.Text);
                DataTable dtFtNoDisList = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_DispatchedItemList_FtNoWise", sp1, DataAccess.Return_Type.DataTable);
                if (dtFtNoDisList.Rows.Count > 0)
                {
                    gvdispatchitem.DataSource = dtFtNoDisList;
                    gvdispatchitem.DataBind();
                    invoice_details(dtFtNoDisList);
                    lblAlreadyPreapre.Text = "ALREADY DISPATCHED INVOICE DETAILS FOR FITMENT ORDER NO: " + lblFtNo.Text; ;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void invoice_details(DataTable dtFtNoDisList)
        {
            DataView dtView = new DataView(dtFtNoDisList);
            dtView.Sort = "Invoiceno";
            DataTable distinct = dtView.ToTable(true, "Invoiceno", "Plant", "InvoiceYear", "ContactPerson", "Contactno", "DeliveryTo", "DispatchMethod", "GoDown",
                "Location", "LrDate", "LrNo", "Transpoter", "VehicleNo", "Comments", "GrandTotal", "servicetax", "Total", "TotalQty", "ServicetaxPercent", "CGST",
                "SGST", "IGST", "CGSTVal", "SGSTVal", "IGSTVal");
            if (distinct.Rows.Count > 0)
            {
                gvFtInvoiceDownload.DataSource = distinct;
                gvFtInvoiceDownload.DataBind();
            }
        }
        private string Get_Address_CustChoice(string strBillID)
        {
            StringBuilder strApp = new StringBuilder();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@shipid", strBillID);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_address_IDwise", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    strApp.Append("<table style='border-collapse: collapse; border-color: #000; width: 502px; line-height: 16px;'>");
                    DataRow row = dt.Rows[0];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string tr = "<tr style='vertical-align: top;'><th style='width:85px; background-color: #861616; text-align: left; color: #fff;'>" +
                            dt.Columns[i].ToString().ToUpper() + "</th><td style='font-weight:bold;'>:</td><td style='font-weight:bold;'>" +
                            row[i].ToString().Replace("~", "<br/>") + "</td></tr>";
                        strApp.Append(tr);
                    }
                    strApp.Append("</table>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strApp.ToString();
        }
        protected void lnkInvoiceFile_Click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            Label lblDownloadInvoiceNo = clickedRow.FindControl("lblDownloadInvoiceNo") as Label;
            HiddenField hdnDownloadUInvoiceYear = clickedRow.FindControl("hdnDownloadUInvoiceYear") as HiddenField;

            LinkButton lnkTxt = sender as LinkButton;
            string serverURL = Server.MapPath("~/ftinvoicefiles/" + hdnCotsCustID.Value + "/" + lblHeadPlant.Text + "/").Replace("TTS", "pdfs");
            string spdfPathTo = serverURL + lnkTxt.ID + hdnDownloadUInvoiceYear.Value + "_" + lblDownloadInvoiceNo.Text + ".pdf";

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.ID + hdnDownloadUInvoiceYear.Value + "_" + lblDownloadInvoiceNo.Text + ".pdf");
            Response.WriteFile(spdfPathTo);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}