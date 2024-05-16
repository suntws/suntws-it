using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;

namespace TTS
{
    public partial class cotscustwisetrack : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_ordertrack"].ToString() == "True")
                        {
                            if (Request["disid"] != null)
                            {
                                if (Request["disid"].ToString() == "dom")
                                    lblPageHead.Text = "DOMESTIC ";
                                else if (Request["disid"].ToString() == "exp")
                                    lblPageHead.Text = "INTERNATIONAL ";
                                string strUserName = string.Empty;
                                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                                    strUserName = Request.Cookies["TTSUserType"].Value;
                                SqlParameter[] sp = new SqlParameter[] { 
                                    new SqlParameter("@OrderType", Request["disid"].ToString().ToUpper()), 
                                    new SqlParameter("@Username", strUserName) };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_customer_leadwise", sp, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    ddlCustomerName.DataSource = dt;
                                    ddlCustomerName.DataTextField = "custfullname";
                                    ddlCustomerName.DataValueField = "custcode";
                                    ddlCustomerName.DataBind();
                                    ddlCustomerName.Items.Insert(0, "CHOOSE");
                                }
                            }
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
                if (hdnCustomerName.Value != "")
                    ddlCustomerName.SelectedIndex = ddlCustomerName.Items.IndexOf(ddlCustomerName.Items.FindByText(hdnCustomerName.Value));
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCustomerName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCustomerName")
                {
                    gv_TrackOrderList.DataSource = null;
                    gv_TrackOrderList.DataBind();
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@custfullname", ddlCustomerName.SelectedItem.Text), new SqlParameter("@stdcustcode", ddlCustomerName.SelectedItem.Value) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name_orderdump", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlUserID.DataSource = dt;
                        ddlUserID.DataTextField = "username";
                        ddlUserID.DataValueField = "ID";
                        ddlUserID.DataBind();
                        if (dt.Rows.Count == 1)
                            ddlUserID_IndexChange(sender, e);
                        else
                            ddlUserID.Items.Insert(0, "CHOOSE");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlUserID_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@custcode", ddlUserID.SelectedItem.Value) };
                DataTable dtOrder = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_customer_order_dump", sp, DataAccess.Return_Type.DataTable);
                ViewState["dtGv"] = dtOrder;
                Bind_DDL_GV(dtOrder, "ALL");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_DDL_GV(DataTable dtGv, string strCont)
        {
            try
            {
                gv_TrackOrderList.DataSource = null;
                gv_TrackOrderList.DataBind();

                DataTable dt = ViewState["dtGv"] as DataTable;
                DataView dtView = new DataView(dt);
                switch (strCont)
                {
                    case "Plant":
                        dtView.Sort = "OYear DESC";
                        DataTable distinctyear = dtView.ToTable(true, "OYear");
                        Utilities.ddl_Binding(ddlYear, distinctyear, "OYear", "OYear", "");
                        dtView = new DataView(dtGv, "OYear='" + ddlYear.SelectedItem.Text + "'", "OMonthID", DataViewRowState.CurrentRows);

                        dtView.Sort = "OMonthID DESC";
                        DataTable distinctMonth = dtView.ToTable(true, "NameOfMonth", "OMonthID");
                        Utilities.ddl_Binding(ddlMonth, distinctMonth, "NameOfMonth", "OMonthID", "");
                        break;
                    case "Year":
                        dtView = new DataView(dtGv, "OYear='" + ddlYear.SelectedItem.Text + "'", "OMonthID", DataViewRowState.CurrentRows);
                        dtView.Sort = "OMonthID DESC";
                        distinctMonth = dtView.ToTable(true, "NameOfMonth", "OMonthID");
                        Utilities.ddl_Binding(ddlMonth, distinctMonth, "NameOfMonth", "OMonthID", "");
                        break;
                    case "Month":
                        break;
                    default:
                        dtView.Sort = "Plant";
                        DataTable distinctplant = dtView.ToTable(true, "Plant");
                        Utilities.ddl_Binding(ddlplant, distinctplant, "Plant", "Plant", "");
                        ddlplant.Items.Insert(0, "ALL");

                        dtView.Sort = "OYear DESC";
                        distinctyear = dtView.ToTable(true, "OYear");
                        Utilities.ddl_Binding(ddlYear, distinctyear, "OYear", "OYear", "");
                        dtView = new DataView(dtGv, "OYear='" + ddlYear.SelectedItem.Text + "'", "OMonthID", DataViewRowState.CurrentRows);

                        dtView.Sort = "OMonthID DESC";
                        distinctMonth = dtView.ToTable(true, "NameOfMonth", "OMonthID");
                        Utilities.ddl_Binding(ddlMonth, distinctMonth, "NameOfMonth", "OMonthID", "");
                        break;
                }
                dtView = new DataView(dtGv, "OYear='" + ddlYear.SelectedItem.Text + "' and OMonthID='" + ddlMonth.SelectedItem.Value + "'", "OMonthID DESC", DataViewRowState.CurrentRows);
                if (dtView.Count > 0)
                {
                    gv_TrackOrderList.DataSource = dtView;
                    gv_TrackOrderList.DataBind();
                    lblQuoteHead1.Text = "";
                }
                else
                    lblQuoteHead1.Text = "NO RECORDS";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Make_Where_Condition(string strField)
        {
            try
            {
                string strCond;
                DataTable dt = ViewState["dtGv"] as DataTable;
                if (strField == "Year")
                    strCond = " OYear='" + ddlYear.SelectedItem.Text + "'";
                else
                    strCond = " OYear='" + ddlYear.SelectedItem.Text + "' and OMonthID='" + ddlMonth.SelectedItem.Value + "' ";
                if (ddlplant.SelectedItem.Text != "ALL")
                    strCond += " and Plant='" + ddlplant.SelectedItem.Text + "'";
                DataView dtView = new DataView(dt, strCond, "OMonthID DESC", DataViewRowState.CurrentRows);
                Bind_DDL_GV(dtView.ToTable(), strField);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            Make_Where_Condition("Plant");
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            Make_Where_Condition("Year");
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Make_Where_Condition("Month");
        }
        protected void btnShowDetails_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = ((Button)sender);
                hdnOID.Value = btn.CommandArgument.ToString();

                GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;
                lblOrderNo.Text = clickedRow.Cells[0].Text;
                Bind_OrderWiseItemList();
                Bind_OrderMasterDetails();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divItemDetails');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_TrackOrderList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ddlCustomerName_IndexChange(null, null);
                gv_TrackOrderList.PageIndex = e.NewPageIndex;
                gv_TrackOrderList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAddress;
        }
        private void Bind_OrderWiseItemList()
        {
            try
            {
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(hdnOID.Value));
                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    gvOrderItemList.FooterRow.Cells[7].Text = "TOTAL";
                    gvOrderItemList.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                    object sumQty = dtItemList.Compute("Sum(itemqty)", "");
                    gvOrderItemList.FooterRow.Cells[8].Text = sumQty.ToString();

                    object sumCost = dtItemList.Compute("Sum(unitprice)", "");
                    gvOrderItemList.FooterRow.Cells[9].Text = Convert.ToDecimal(sumCost).ToString();

                    object sumFwt = dtItemList.Compute("Sum(finishedwt)", "");
                    gvOrderItemList.FooterRow.Cells[10].Text = Convert.ToDecimal(sumFwt).ToString();
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
                DataTable dtMasterList = DomesticScots.Bind_OrderMasterDetails(Convert.ToInt32(hdnOID.Value));
                if (dtMasterList.Rows.Count > 0)
                {
                    dlOrderMaster.DataSource = dtMasterList;
                    dlOrderMaster.DataBind();

                    lblAttachment.Text = "";
                    lblInvoiceTxt.Text = "";
                    lnkinvoicefiles.Text = "";
                    lblProformaTxt.Text = "";
                    lnkproformafiles.Text = "";
                    lblPdiList.Text = "";
                    lnkPdiList.Text = "";
                    lblLRTxt.Text = "";
                    lnklrcopyfiles.Text = "";

                    if (Convert.ToInt32(dtMasterList.Rows[0]["OrderStatus"].ToString()) != 1 && Convert.ToInt32(dtMasterList.Rows[0]["OrderStatus"].ToString()) != 6)
                    {
                        SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                        DataTable dtFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_customer_AttachPdfFiles", sp2, DataAccess.Return_Type.DataTable);
                        if (dtFile.Rows.Count > 0)
                        {
                            lblAttachment.Text = "DOWNLOAD FILES";
                            foreach (DataRow fRow in dtFile.Rows)
                            {
                                if (fRow["FileType"].ToString() == "PROFORMA FILE")
                                {
                                    lblProformaTxt.Text = "PROFORMA";
                                    lnkproformafiles.Text = fRow["AttachFileName"].ToString();
                                }
                                if (fRow["FileType"].ToString() == "UPLOAD LR COPY")
                                {
                                    lblLRTxt.Text = "L/R COPY";
                                    lnklrcopyfiles.Text = fRow["AttachFileName"].ToString();
                                }
                                if (fRow["FileType"].ToString() == "PREPARE BUYER INVOICE" || fRow["FileType"].ToString() == "ORIGINAL FOR RECEPIENT")
                                {
                                    lblInvoiceTxt.Text = "INVOICE";
                                    lnkinvoicefiles.Text = fRow["AttachFileName"].ToString();
                                }
                                if (fRow["FileType"].ToString() == "PDI LIST")
                                {
                                    lblPdiList.Text = "PDI LSIT";
                                    lnkPdiList.Text = fRow["AttachFileName"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkAttachFile_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string fileFolder = lnkTxt.ID.Replace("lnk", "").Replace("PdiList", "invoicefiles");
                string serverURL = Server.MapPath("~/" + fileFolder + "/" + ddlUserID.SelectedItem.Value + "/").Replace("TTS", "pdfs").Replace("SCOTS", "pdfs").Replace("COTS", "pdfs");
                string spdfPathTo = serverURL + "/" + lnkTxt.Text;

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
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