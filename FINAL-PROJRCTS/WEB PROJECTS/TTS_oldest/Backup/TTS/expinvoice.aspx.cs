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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
                            DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_InvoiceMake_ExportOrder_v1", DataAccess.Return_Type.DataTable);
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
                BILLOFLADING.Text = "";
                containerno.Text = "";
                SERIALNO.Text = "";
                finaldestination.Text = "";

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
                Bind_Attach_PdfFiles();
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
                    hdnorderref.Value = dtMasterList.Rows[0]["orderrefno"].ToString();
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
        private void Bind_Attach_PdfFiles()
        {
            try
            {
                lnkPdfLink.Text = "";
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value), new SqlParameter("@FileType", "EXPORT INVOICE") };
                DataTable dtPdfFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_AttachPdfFiles", sp1, DataAccess.Return_Type.DataTable);

                if (dtPdfFile != null && dtPdfFile.Rows.Count > 0)
                    lnkPdfLink.Text = dtPdfFile.Rows[0]["AttachFileName"].ToString();
                else
                {
                    string strTerms = (string)daCOTS.ExecuteScalar("select PaymentTerms from usermaster where ID='" + hdnCustCode.Value + "'");
                    txtPayterms.Text = Server.HtmlDecode(strTerms.Replace("~", "\r\n"));
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }



        protected void btnPrepareExpInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                invoiceDocPreparation();
                AddtionalCharge();


                string serverURL = Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/export_invoicefiles/" + hdnCustCode.Value + "/";

                string pdfwrtto = path + hdnorderref.Value + ".pdf";
                DirectoryInfo directoryinfo = new DirectoryInfo(path);
                directoryinfo.Create();
                InvoiceDataModel iDM;
                iDM = Build_exportInvoiceDataModel();


                Build_allexportinvoice(pdfwrtto, iDM);



            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void AddtionalCharge()
        {
            try
            {
                SqlParameter[] spdel = new SqlParameter[] { new SqlParameter("@OID", hdnOID.Value) };
                daCOTS.ExecuteNonQuery_SP("sp_del_Export_Discount", spdel);

                for (int i = 0; i < gvAmountSub.Rows.Count; i++)
                {
                    string DESC = ((TextBox)gvAmountSub.Rows[i].FindControl("txtAddDesc")).Text.Trim();
                    string AMT = ((TextBox)gvAmountSub.Rows[i].FindControl("txtCAddAmt")).Text.Trim();
                    if (DESC != "" && AMT != "")
                    {
                        SqlParameter[] sp2 = new SqlParameter[] {
                            new SqlParameter("@OID", hdnOID.Value),
                            new SqlParameter("@Category", "ADD"),
                            new SqlParameter("@DescriptionDiscount", DESC),
                            new SqlParameter("@Amount", AMT),
                            new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                            new SqlParameter("@discountType", "OTHER CHARGES")
                        };
                        int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp2);
                    }
                }
                if (txtClaimAdjustment.Text != "" && txtLESSAMT.Text != "")
                {
                    SqlParameter[] sp12 = new SqlParameter[]{
                        new SqlParameter("@OID", hdnOID.Value),
                        new SqlParameter("@Category", lblLESSclaimAdjus.ToolTip),
                        new SqlParameter("@DescriptionDiscount", txtClaimAdjustment.Text),
                        new SqlParameter("@Amount", txtLESSAMT.Text),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@discountType", "CLAIM ADJUSTMENT")
                    };
                    int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp12);
                }
                if (txtotherdiscount.Text != "" && txtOtherDisAmt.Text != "")
                {
                    SqlParameter[] sp112 = new SqlParameter[]{
                        new SqlParameter("@OID", hdnOID.Value),
                        new SqlParameter("@Category", lblLessdiscount.ToolTip),
                        new SqlParameter("@DescriptionDiscount", txtotherdiscount.Text),
                        new SqlParameter("@Amount", txtOtherDisAmt.Text),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@discountType", "OTHER DISCOUNT")
                    };
                    daCOTS.ExecuteNonQuery_SP("sp_ins_Export_Discount", sp112);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkPdfLink_click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string path = Server.MapPath("~/export_invoicefiles/" + hdnCustCode.Value + "/").Replace("TTS", "pdfs") + lnkTxt.Text;

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private InvoiceDataModel Build_exportInvoiceDataModel()
        {
            InvoiceDataModel iDM = new InvoiceDataModel();
            iDM.OID = Convert.ToInt32(hdnOID.Value);
            iDM.InvoiceNo = txtInvoiceNo.Text;
            iDM.InvoiceDate = lblInvoiceDate.Text;
            iDM.OrderRefNo = hdnorderref.Value;
            iDM.custcurrency = lblCustCurrency.Text;

            return iDM;
        }

        private void Build_allexportinvoice(string path, InvoiceDataModel iDM)
        {
            try
            {
                Document document = new Document(PageSize.A4, 18f, 2f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
                document.Open();
                document.Add(PrepareInvoice_Exp.Prepare(document, iDM));
                document.Close();

                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@OID", hdnOID.Value);
                sp1[1] = new SqlParameter("@AttachFileName", lblStausOrderRefNo.Text + ".pdf");
                sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sp1[3] = new SqlParameter("@FileType", "EXPORT INVOICE");
                daCOTS.ExecuteNonQuery_SP("sp_ins_AttachInvoiceFiles_V1", sp1);



            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void invoiceDocPreparation()
        {

            try
            {
                SqlParameter[] sp2 = new SqlParameter[16];
                sp2[0] = new SqlParameter("@OID", hdnOID.Value);
                sp2[1] = new SqlParameter("@invoiceno", txtInvoiceNo.Text);
                sp2[2] = new SqlParameter("@exportauthorised", ddlExpAuthorizedAddress.SelectedItem.Text);
                sp2[3] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                sp2[4] = new SqlParameter("@Billoflading", BILLOFLADING.Text);
                sp2[5] = new SqlParameter("@containertype", ddlContainerType.SelectedItem.Text);
                sp2[6] = new SqlParameter("@precarriaby", ddlPreCarriageBy.SelectedItem.Text);
                sp2[7] = new SqlParameter("@incoterm", ddlPriceBasis.SelectedItem.Text);
                sp2[8] = new SqlParameter("@containerno", containerno.Text);
                sp2[9] = new SqlParameter("@countorigin", ddlCountryOrigin.SelectedItem.Text);
                sp2[10] = new SqlParameter("@incotermport", txtPriceBasisContent.Text);
                sp2[11] = new SqlParameter("@containerseal", SERIALNO.Text);
                sp2[12] = new SqlParameter("@portofloading", ddlPortLoading.SelectedItem.Text);
                sp2[13] = new SqlParameter("@placerecepit", ddlPlaceofReceipt.SelectedItem.Value);
                sp2[14] = new SqlParameter("@finaldestination", finaldestination.Text);
                sp2[15] = new SqlParameter("@vesselno", vesselname.Text);
                //sp2[16] = new SqlParameter("@Billoflading", BILLOFLADING.Text);


                daCOTS.ExecuteNonQuery_SP("sp_ins_invoiceCreateDetails_Export", sp2);


            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}