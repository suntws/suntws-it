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
    public partial class expinvoice : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["exp_documents"].ToString() == "True"))
                        {
                            DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_InvoiceMake_ExportOrder", DataAccess.Return_Type.DataTable);
                            if (dtData.Rows.Count > 0)
                            {
                                gvInvoiceMakeList.DataSource = dtData;
                                gvInvoiceMakeList.DataBind();
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
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkExpInvoiceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectOrderDetails(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectOrderDetails(GridViewRow row)
        {
            try
            {
                lblCustCurrency.Text = ((HiddenField)row.FindControl("hdnUserCurrency")).Value;

                DataTable dtgv1 = new DataTable();
                dtgv1.Columns.Add("slno", typeof(String));
                for (int i = 1; i <= 3; i++)
                    dtgv1.Rows.Add(i);
                gvAmountSub.DataSource = dtgv1;
                gvAmountSub.DataBind();
                gvAmountSub.HeaderRow.Cells[3].Text = lblCustCurrency.Text;

                txtInvoiceNo.Text = "";
                lblInvoiceDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtPayterms.Text = "";
                txtPriceBasisContent.Text = "";
                txtClaimAdjustment.Text = "";
                txtLESSAMT.Text = "";
                txtotherdiscount.Text = "";
                txtOtherDisAmt.Text = "";

                hdnCustCode.Value = ((HiddenField)row.FindControl("hdnStatusCustCode")).Value;
                lblCustName.Text = row.Cells[0].Text;
                lblStausOrderRefNo.Text = row.Cells[1].Text;
                lblWorkorderno.Text = row.Cells[2].Text;
                lblShipmentType.Text = row.Cells[6].Text;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                Bind_OrderMasterDetails();
                Bind_OrderItem();
                if (lblShipmentType.Text == "COMBI")
                    Bind_OrderSumValue();
                Bind_ReviseDetails();
                //Bind_Attach_PdfFiles();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
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
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtMasterList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_OrderAllDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dtMasterList.Rows.Count > 0)
                {
                    dlOrderMaster.DataSource = dtMasterList;
                    dlOrderMaster.DataBind();
                    ddlPreCarriageBy.SelectedIndex = ddlPreCarriageBy.Items.IndexOf(ddlPreCarriageBy.Items.FindByText(dtMasterList.Rows[0]["DeliveryMethod"].ToString()));
                    ddlPriceBasis.SelectedIndex = ddlPriceBasis.Items.IndexOf(ddlPriceBasis.Items.FindByText(dtMasterList.Rows[0]["GodownName"].ToString()));
                }

                DataTable dtExpAuthorizedAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CustWise_AuthorizedAddress", DataAccess.Return_Type.DataTable);
                if (dtExpAuthorizedAddress.Rows.Count > 0)
                {
                    ddlExpAuthorizedAddress.DataSource = dtExpAuthorizedAddress;
                    ddlExpAuthorizedAddress.DataTextField = "ExpoterAddress";
                    ddlExpAuthorizedAddress.DataValueField = "ID";
                    ddlExpAuthorizedAddress.DataBind();
                }
                ddlExpAuthorizedAddress.Items.Insert(0, "CHOOSE");
                ddlExpAuthorizedAddress.SelectedIndex = ddlExpAuthorizedAddress.Items.IndexOf(ddlExpAuthorizedAddress.Items.FindByValue(dtMasterList.Rows[0]["ExporterAddressID"].ToString()));

                txtInvoiceNo.Enabled = true;
                SqlParameter[] sprev = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtrev = (DataTable)daCOTS.ExecuteReader_SP("Sp_Chk_Exp_InvoiceNo", sprev, DataAccess.Return_Type.DataTable);
                if (dtrev != null && dtrev.Rows.Count > 0)
                {
                    txtInvoiceNo.Text = dtrev.Rows[0]["invoiceno"].ToString();
                    lblInvoiceDate.Text = dtrev.Rows[0]["INVDATE"].ToString();
                    txtInvoiceNo.Enabled = false;
                }

                SqlParameter[] spRef = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtProformaRef = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_ProformaRefNo", spRef, DataAccess.Return_Type.DataTable);
                if (dtProformaRef != null && dtProformaRef.Rows.Count > 0)
                {
                    txtPayterms.Text = dtProformaRef.Rows[0]["payterms"].ToString().Replace("~", "\r\n");
                    txtPriceBasisContent.Text = dtProformaRef.Rows[0]["PriceBasisContent"].ToString();
                    ddlPriceBasis.SelectedIndex = ddlPriceBasis.Items.IndexOf(ddlPriceBasis.Items.FindByText(dtProformaRef.Rows[0]["PriceMethod"].ToString()));
                    ddlContainerType.SelectedIndex = ddlContainerType.Items.IndexOf(ddlContainerType.Items.FindByText(dtProformaRef.Rows[0]["ContainerType"].ToString()));
                }
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
                    strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
                    strAddress += "<div>" + row["contact_name"].ToString() + "</div>";
                    strAddress += "<div>" + row["EmailID"].ToString() + " / " + row["mobile"].ToString() + "</div>";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAddress;
        }
        private void Bind_OrderItem()
        {
            try
            {
                gvOrderItemList.DataSource = null;
                gvOrderItemList.DataBind();

                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(hdnOID.Value));
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
                        gvOrderItemList.FooterRow.Cells[13].Text = lblCustCurrency.Text + ": " + Convert.ToDecimal(dtItemList.Compute("Sum(Rimpricepdf)", "")).ToString();
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
        private void Bind_OrderSumValue()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtSumList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_Order_SumValue_Logistics", sp1, DataAccess.Return_Type.DataTable);
                if (dtSumList.Rows.Count > 0)
                {
                    gv_OrderSumValue.DataSource = dtSumList;
                    gv_OrderSumValue.DataBind();

                    gv_OrderSumValue.FooterRow.Cells[6].Text = "Total";
                    gv_OrderSumValue.FooterRow.Cells[7].Text = dtSumList.Compute("Sum([TOT QTY])", "").ToString();
                    gv_OrderSumValue.FooterRow.Cells[8].Text = dtSumList.Compute("Sum([TOT PRICE])", "").ToString();
                    gv_OrderSumValue.FooterRow.Cells[9].Text = dtSumList.Compute("Sum([TOT WT])", "").ToString();

                    if ((Convert.ToDecimal(gv_OrderSumValue.FooterRow.Cells[9].Text) / 1000) < 14)
                        ddlContainerType.SelectedIndex = 2;
                    else if ((Convert.ToDecimal(gv_OrderSumValue.FooterRow.Cells[9].Text) / 1000) > 20)
                        ddlContainerType.SelectedIndex = 1;
                    else
                        ddlContainerType.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ReviseDetails()
        {
            try
            {
                SqlParameter[] spdiscount = new SqlParameter[] { new SqlParameter("@OID", hdnOID.Value) };
                DataTable dtdiscount = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_Discount", spdiscount, DataAccess.Return_Type.DataTable);
                if (dtdiscount != null && dtdiscount.Rows.Count > 0)
                {
                    int gvRow = 0;
                    foreach (DataRow disRow in dtdiscount.Select("Discounttype='OTHER CHARGES'"))
                    {
                        TextBox txtAddDesc = gvAmountSub.Rows[gvRow].FindControl("txtAddDesc") as TextBox;
                        TextBox txtCAddAmt = gvAmountSub.Rows[gvRow].FindControl("txtCAddAmt") as TextBox;

                        txtAddDesc.Text = disRow["DescriptionDiscount"].ToString();
                        txtCAddAmt.Text = disRow["Amount"].ToString();
                        gvRow++;
                    }
                    foreach (DataRow disRow in dtdiscount.Select("Discounttype='CLAIM ADJUSTMENT'"))
                    {
                        txtClaimAdjustment.Text = disRow["DescriptionDiscount"].ToString();
                        txtLESSAMT.Text = disRow["Amount"].ToString();
                    }
                    foreach (DataRow disRow in dtdiscount.Select("Discounttype='OTHER DISCOUNT'"))
                    {
                        txtotherdiscount.Text = disRow["DescriptionDiscount"].ToString();
                        txtOtherDisAmt.Text = disRow["Amount"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        //private void Bind_Attach_PdfFiles()
        //{
        //    try
        //    {
        //        lnkPdfLink.Text = "";
        //        SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value), new SqlParameter("@FileType", "PROFORMA FILE") };
        //        DataTable dtPdfFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AttachPdfFiles", sp1, DataAccess.Return_Type.DataTable);

        //        if (dtPdfFile != null && dtPdfFile.Rows.Count > 0)
        //            lnkPdfLink.Text = dtPdfFile.Rows[0]["AttachFileName"].ToString();
        //        else
        //        {
        //            string strTerms = (string)daCOTS.ExecuteScalar("select PaymentTerms from usermaster where ID='" + hdnCustCode.Value + "'");
        //            txtPayterms.Text = Server.HtmlDecode(strTerms.Replace("~", "\r\n"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
        //    }
        //}

        protected void btnPrepareExpInvoice_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}