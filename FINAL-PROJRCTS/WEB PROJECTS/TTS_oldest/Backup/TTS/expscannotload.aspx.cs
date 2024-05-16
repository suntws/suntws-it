using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Web.UI.HtmlControls;

namespace TTS
{
    public partial class expscannotload : System.Web.UI.Page
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
                            (dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True" ||dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk"].ToString() == "True"))
                        {
                            if (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageTitle.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ");
                                SqlParameter[] sp = new SqlParameter[] { 
                                    new SqlParameter("@qstring", Utilities.Decrypt(Request["fid"].ToString())), 
                                    new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())) 
                                };
                                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_LoadingCheck_OrderList", sp, DataAccess.Return_Type.DataTable);
                                if (dtList.Rows.Count > 0)
                                {
                                    gvLoadCheckOrder.DataSource = dtList;
                                    gvLoadCheckOrder.DataBind();
                                    btnSaveLoadStatus.Text = "DISPATCH TO CUSTOMER";
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL is wrong.";
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
        protected void lnkPdiLoad_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_Select_PDI_Order(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_Select_PDI_Order(GridViewRow row)
        {
            try
            {
                lblOrderQty.Text = "";
                lblInspectedBy.Text = "";
                lblApprovedBy.Text = "";
                lblApprovedDate.Text = "";
                lblLoadScanQty.Text = "";
                lblBarcode.Text = "";
                txtBarcode.Text = "";
                txtLoadScanStatus.Text = "";
                lblShipType.Text = "";
                lblFinalLoading.Text = "";
                lblQtyDetails.Text = "";
                hdnTotOrderQty.Value = "";
                btnSaveLoadStatus.Text = "DISPATCH TO CUSTOMER";
                ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "document.getElementById('divLoadDetails').style.display='none';" +
                    "document.getElementById('btnLoadCheck').style.display='block';", true);
                divLoadDetails.Style.Add("display", "none");

                hdnCustcode.Value = ((HiddenField)row.FindControl("hdnOrderCustCode")).Value;
                hdnOrderRef.Value = ((HiddenField)row.FindControl("hdnOrderRefNo")).Value;
                hdnPID.Value = ((HiddenField)row.FindControl("hdnOrderPID")).Value;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                lblCustName.Text = row.Cells[0].Text;
                lblWorkorderNo.Text = row.Cells[1].Text;
                lblOrderQty.Text = row.Cells[2].Text;
                lblInspectedBy.Text = row.Cells[3].Text;
                lblApprovedBy.Text = row.Cells[4].Text;
                lblApprovedDate.Text = row.Cells[5].Text;

                hdnTotOrderQty.Value = lblOrderQty.Text;

                if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                {
                    SqlParameter[] spType = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                    DataSet dsType = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_order_shiptype_v2", spType, DataAccess.Return_Type.DataSet);
                    if (dsType.Tables[0].Rows.Count == 1)
                    {
                        lblShipType.Text = dsType.Tables[0].Rows[0]["ShipmentType"].ToString();
                        lblFinalLoading.Text = dsType.Tables[0].Rows[0]["ContainerLoadFrom"].ToString();
                        if (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() != lblFinalLoading.Text)
                            btnSaveLoadStatus.Text = "SENT TO " + lblFinalLoading.Text;
                        else
                        {
                            string strQtyTable = "<table cellspacing='0' rules='all' border='0'>";
                            LinkButton lnk = new LinkButton();
                            foreach (DataRow rowQ in dsType.Tables[1].Rows)
                            {
                                lnk.Text = rowQ["ItemPlant"].ToString();
                                strQtyTable += "<tr><th style='background-color: #f1e8aa;text-align: left;'>" + rowQ["ItemPlant"].ToString() +
                                         " QTY</th><td style='text-align: right;'>" + rowQ["orderqty"].ToString() + "</td><td  style='text-align: right;'>  <span class='btnShowData' id='" + rowQ["ItemPlant"].ToString() + "' onclick='showpdiNotLoaded(this.id)'>" + rowQ["ItemPlant"].ToString() + "</span></td></tr>";
                            }
                            strQtyTable += "<tr><th style='background-color: #f1e8aa;text-align: left;'>TOT QTY</th><td style='text-align: right;'>" +
                                dsType.Tables[1].Compute("Sum(orderqty)", "").ToString() + "</td></tr></table>";
                            lblQtyDetails.Text = strQtyTable;
                            hdnTotOrderQty.Value = dsType.Tables[1].Compute("Sum(orderqty)", "").ToString();
                        }
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JSubDiv", "gotoPreviewDiv('div_export_data');", true);
                }

                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@ID", hdnPID.Value) };
                DataTable dtLoadqty = new DataTable();
                if (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() == lblFinalLoading.Text && Utilities.Decrypt(Request["fid"].ToString()).ToLower() == "e" &&
                        lblShipType.Text == "COMBI")
                    dtLoadqty = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanMasterData_Loading_combi", sp2, DataAccess.Return_Type.DataTable);
                else
                    dtLoadqty = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanMasterData_Loading", sp2, DataAccess.Return_Type.DataTable);
                lblLoadScanQty.Text = dtLoadqty.Rows[0]["LoadQty"].ToString() + "/" + hdnTotOrderQty.Value;
                if (dtLoadqty.Rows[0]["LoadQty"].ToString() == hdnTotOrderQty.Value)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "document.getElementById('divLoadDetails').style.display='block';" +
                        "document.getElementById('btnLoadCheck').style.display='none';", true);
                    divLoadDetails.Style.Add("display", "block");
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JMainDiv", "gotoPreviewDiv('div_LoadOrder');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnBarcodeCheck_Click(object sender, EventArgs e)
        {
            btnTriggerLoadScan_Click(sender, e);
        }
        protected void btnTriggerLoadScan_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcode.Text != "" && txtBarcode.Text.Length >= 19)
                {
                    txtLoadScanStatus.Text = "";
                    string message = "";
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@ID", hdnPID.Value),
                        new SqlParameter("@barcode",txtBarcode.Text),
                        new SqlParameter("@LoadingBy",Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())), 
                        new SqlParameter("@ResultMsg",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message)
                    };
                    if (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() == lblFinalLoading.Text && Utilities.Decrypt(Request["fid"].ToString()).ToLower() == "e" &&
                        lblShipType.Text == "COMBI")
                        message = (string)daCOTS.ExecuteScalar_SP("sp_sel_PDIscanList_LoadCheck_Combi", sp);
                    else
                        message = (string)daCOTS.ExecuteScalar_SP("sp_sel_PDIscanList_LoadCheck", sp);

                    if (message.Contains("SCAN OK"))
                    {
                        txtLoadScanStatus.Text = "SCAN OK";
                        txtLoadScanStatus.Style.Add("color", "#11c728");
                        lblLoadScanQty.Text = message.Replace("SCAN OK ", "") + "/" + hdnTotOrderQty.Value;
                    }
                    else
                    {
                        txtLoadScanStatus.Text = message;
                        txtLoadScanStatus.Style.Add("color", "#c7112a");
                    }
                    lblBarcode.Text = txtBarcode.Text;
                    if (message.Replace("SCAN OK ", "") == hdnTotOrderQty.Value && hdnTotOrderQty.Value != "")
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "document.getElementById('divLoadDetails').style.display='block';" +
                            "document.getElementById('btnLoadCheck').style.display='none';", true);
                        divLoadDetails.Style.Add("display", "block");
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "speak('" + txtLoadScanStatus.Text + "');", true);
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveLoadStatus_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@containerno", txtContainerNo.Text) ,
                    new SqlParameter("@vehicleno", txtVehicleNo.Text) ,
                    new SqlParameter("@loadedremarks", txtRemarks.Text.Replace("\r\n","~")) ,
                    new SqlParameter("@loadedby", Request.Cookies["TTSUser"].Value) ,
                    new SqlParameter("@ID", hdnPID.Value) 
                };
                int resp = 0;
                if (btnSaveLoadStatus.Text == "DISPATCH TO CUSTOMER")
                    resp = daCOTS.ExecuteNonQuery_SP("sp_upd_pdiscan_loaded", sp);
                else
                    resp = daCOTS.ExecuteNonQuery_SP("sp_upd_pdiscan_loaded_TransferToPlant", sp);
                if (resp > 0)
                {
                    Build_PDIReport();
                    if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                        resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), "21", txtContainerNo.Text + "~" +
                            txtVehicleNo.Text + "~" + txtRemarks.Text, Request.Cookies["TTSUser"].Value);
                    else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                        resp = DomesticScots.ins_dom_StatusChangedDetails(Convert.ToInt32(hdnOID.Value), btnSaveLoadStatus.Text == "DISPATCH TO CUSTOMER" ? "38" : "9",
                            txtContainerNo.Text + "~" + txtVehicleNo.Text + "~" + txtRemarks.Text, Request.Cookies["TTSUser"].Value);

                    Response.Redirect(Request.RawUrl.ToString(), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_PDIReport()
        {
            try
            {
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                if (!Directory.Exists(serverURL + "/invoicefiles/" + hdnCustcode.Value + "/"))
                    Directory.CreateDirectory(serverURL + "/invoicefiles/" + hdnCustcode.Value + "/");
                string path = serverURL + "/invoicefiles/" + hdnCustcode.Value + "/";
                string strfileAdditional = "PDIList_";
                string strOrderNo = Utilities.Decrypt(Request["fid"].ToString()).ToLower() == "d" ? hdnOrderRef.Value : hdnPID.Value;
                string sPathToWritePdfTo = path + strfileAdditional + strOrderNo + "_" + Utilities.Decrypt(Request["pid"].ToString()) + ".pdf";

                Exp_PDI_Report.CustomerName = lblCustName.Text;
                Exp_PDI_Report.OrderNumber = lblWorkorderNo.Text;
                Exp_PDI_Report.OrderQuantity = lblOrderQty.Text;
                Exp_PDI_Report.InspectedBy = lblInspectedBy.Text;
                Exp_PDI_Report.VerifiedBy = Request.Cookies["TTSUser"].Value;
                Exp_PDI_Report.ApprovedBy = lblApprovedBy.Text;

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@ID", hdnPID.Value) };
                DataTable dtData = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PDIscanList", sp1, DataAccess.Return_Type.DataTable);
                Exp_PDI_Report.dtScanList = dtData;

                Exp_PDI_Report.PDI_Generation(sPathToWritePdfTo);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnLoadCheck_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@PID", hdnPID.Value), 
                    new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())), 
                    new SqlParameter("@Loadedby", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@ResultMsg", SqlDbType.VarChar, 8000, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Default, message) 
                };
                if (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() == lblFinalLoading.Text && Utilities.Decrypt(Request["fid"].ToString()).ToLower() == "e" &&
                        lblShipType.Text == "COMBI")
                    message = (string)daCOTS.ExecuteScalar_SP("sp_upd_Pdi_LoadCheck_Combi", sp);
                else
                    message = (string)daCOTS.ExecuteScalar_SP("sp_upd_Pdi_LoadCheck", sp);
                if (message.Contains("LOADING CHECK COMPLETED"))
                {
                    gvLoadCheckOrder.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                    Build_Select_PDI_Order(gvLoadCheckOrder.SelectedRow);
                }
                else
                {
                    txtLoadScanStatus.Text = message;
                    txtLoadScanStatus.Style.Add("color", "#c7112a");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}