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

namespace TTS
{
    public partial class Cotsfttordertrrack : System.Web.UI.Page
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
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FtDispatched_year_details", sp, DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                ddlYear.DataSource = dtorderlist;
                                ddlYear.DataTextField = "CYear";
                                ddlYear.DataValueField = "CYear";
                                ddlYear.DataBind();

                                ddlYear_IndexChange(sender, e);
                            }
                        }
                        else
                        {
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
        protected void ddlYear_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gvdispatchlist.DataSource = null;
                gvdispatchlist.DataBind();
                SqlParameter[] sp = new SqlParameter[2];
                sp[0] = new SqlParameter("@Plant", lblHeadPlant.Text);
                sp[1] = new SqlParameter("@Year", ddlYear.Text);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FtDispatched_Month_details", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlMonth.DataSource = dt;
                    ddlMonth.DataTextField = "CMonth";
                    ddlMonth.DataValueField = "MonthID";
                    ddlMonth.DataBind();

                    ddlMonth_IndexChange(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlMonth_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gvdispatchlist.DataSource = null;
                gvdispatchlist.DataBind();
                SqlParameter[] sp = new SqlParameter[3];
                sp[0] = new SqlParameter("@Plant", lblHeadPlant.Text);
                sp[1] = new SqlParameter("@Year", Convert.ToInt32(ddlYear.SelectedItem.Text));
                sp[2] = new SqlParameter("@Month", ddlMonth.SelectedItem.Value);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FttDispatched_details", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvdispatchlist.DataSource = dt;
                    gvdispatchlist.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkShow_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblStausInvoiceNo.Text = ((Label)row.FindControl("lblinvoice")).Text;
                hdnCotsCustID.Value = ((HiddenField)row.FindControl("hdnCustCode")).Value;
                lblCustName.Text = ((Label)row.FindControl("lblStatusCustName")).Text;
                hdnyear.Value = ((HiddenField)row.FindControl("hdngvyear")).Value;

                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@invoiceno", lblStausInvoiceNo.Text);
                sp1[2] = new SqlParameter("@Plant", lblHeadPlant.Text);
                sp1[3] = new SqlParameter("@InvoiceYear", hdnyear.Value);
                DataTable dtotherFtNo = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_FtItemListInvoice", sp1, DataAccess.Return_Type.DataTable);
                if (dtotherFtNo.Rows.Count > 0)
                {
                    gvdispatchitem.DataSource = dtotherFtNo;
                    gvdispatchitem.DataBind();
                    invoice_details(dtotherFtNo);

                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "Hidedispatch();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void invoice_details(DataTable dtFtNoDisList)
        {
            try
            {
                DataView dtView = new DataView(dtFtNoDisList);
                dtView.Sort = "Invoiceno";
                DataTable distinct = dtView.ToTable(true, "Invoiceno", "Plant", "InvoiceYear", "ContactPerson", "Contactno", "DeliveryTo", "DispatchMethod",
                    "GoDown", "Location", "LrDate", "LrNo", "Transpoter", "VehicleNo", "Comments", "GrandTotal", "servicetax", "Total", "TotalQty", "ServicetaxPercent",
                    "CGST", "SGST", "IGST", "CGSTVal", "SGSTVal", "IGSTVal");
                if (distinct.Rows.Count > 0)
                {
                    gvFtInvoiceDownload.DataSource = distinct;
                    gvFtInvoiceDownload.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkInvoiceFile_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}