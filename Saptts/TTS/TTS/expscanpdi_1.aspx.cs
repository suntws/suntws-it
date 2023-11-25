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
using System.Web.Services;
using System.Data.Common;

namespace TTS
{
    public partial class expscanpdi_1 : System.Web.UI.Page
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
                            (dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk"].ToString() == "True"))
                        {
                            lblPageTitle.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ");
                            hdnPlant.Value = Utilities.Decrypt(Request["pid"].ToString());
                            hdnCustCode.Value = Utilities.Decrypt(Request["ccode"].ToString());
                            lbl_OrderNo.Text = Utilities.Decrypt(Request["oid"].ToString());

                            SqlParameter[] sp1 = new SqlParameter[] { 
                                new SqlParameter("@custcode", hdnCustCode.Value),
                                new SqlParameter("@orderrefno", lbl_OrderNo.Text),
                                new SqlParameter("@O_ID", Utilities.Decrypt(Request["id"].ToString())), 
                                new SqlParameter("@pdiPlant", hdnPlant.Value) 
                            };
                            DataSet dtDomItemList = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_orderitem_list_for_pdi_inspect", sp1, DataAccess.Return_Type.DataSet);
                            if (dtDomItemList.Tables.Count == 2)
                            {
                                if (dtDomItemList.Tables[0].Rows.Count > 0)
                                {
                                    gvPdiItemList.DataSource = dtDomItemList.Tables[0];
                                    gvPdiItemList.DataBind();
                                    ViewState["dtDomItemList"] = dtDomItemList.Tables[0];
                                    hdnPdiFor.Value = dtDomItemList.Tables[0].Rows[0]["ProcessType"].ToString();
                                }
                                if (dtDomItemList.Tables[1].Rows.Count > 0)
                                {
                                    hdnPID.Value = dtDomItemList.Tables[1].Rows[0]["PID"].ToString();
                                    txt_OrderRefNo.Text = dtDomItemList.Tables[1].Rows[0]["workorderno"].ToString();
                                    txt_OrderRefNo.Enabled = dtDomItemList.Tables[1].Rows[0]["workorderno"].ToString() == "" ? true : false;
                                    lblCustomer.Text = dtDomItemList.Tables[1].Rows[0]["custfullname"].ToString();
                                    txt_OrderQty.Text = dtDomItemList.Tables[1].Rows[0]["itemqty"].ToString();
                                }

                                if (hdnPdiFor.Value == "ORDER")
                                {
                                    SqlParameter[] sp = new SqlParameter[] { 
                                        new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString())), 
                                        new SqlParameter("@O_ID", Utilities.Decrypt(Request["id"].ToString()))
                                    };
                                    DataTable dtEarmark = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmark_barcodelist_ForApproval_Chk", sp, DataAccess.Return_Type.DataTable);
                                    if (dtEarmark != null && dtEarmark.Rows.Count > 0)
                                    {
                                        ViewState["EarmarkInspectQty"] = dtEarmark.Rows.Count;
                                        ViewState["dtEarmarkList"] = dtEarmark;
                                    }

                                    if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                                        lnkWorkOrder.Text = (Utilities.Decrypt(Request["oid"].ToString()) + ".pdf");
                                    else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                                        lnkWorkOrder.Text = (Utilities.Decrypt(Request["oid"].ToString()) + "_" + Utilities.Decrypt(Request["pid"].ToString()) + ".pdf");

                                    if (txt_OrderRefNo.Text == "")
                                    {
                                        SqlParameter[] spWo = new SqlParameter[] { 
                                            new SqlParameter("@O_ID", Utilities.Decrypt(Request["id"].ToString())), 
                                            new SqlParameter("@Plant", hdnPlant.Value) 
                                        };
                                        txt_OrderRefNo.Text = (string)daCOTS.ExecuteScalar_SP("sp_sel_workorderno", spWo);
                                    }
                                }
                                if (hdnPID.Value != "")
                                {
                                    SqlParameter[] spInspQty = new SqlParameter[] { new SqlParameter("@PID", hdnPID.Value) };
                                    txtInspectedQty.Text = (string)daCOTS.ExecuteScalar_SP("sp_sel_Pdi_InspQty", spInspQty);
                                    lblNoOfRecords.Text = "SCAN COUNT: " + txtInspectedQty.Text + "/" + txt_OrderQty.Text;
                                    if (txt_OrderQty.Text == txtInspectedQty.Text)
                                    {
                                        lblSucessMsg.Text = "Order Qty Completed";
                                        btnSaveItem.Style.Add("display", "none");
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
        protected void btnBarcodeCheck_Click(object sender, EventArgs e)
        {
            btnTriggerScan_Click(sender, e);
        }
        protected void btnTriggerScan_Click(object sender, EventArgs e)
        {
            try
            {
                CtrlClear();
                if (txtBarcode.Text != "" && txtBarcode.Text.Length >= 19)
                {
                    string message = "";
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@barcode", txtBarcode.Text), 
                        new SqlParameter("@custcode", hdnCustCode.Value), 
                        new SqlParameter("@workorderno", txt_OrderRefNo.Text), 
                        new SqlParameter("@PdiPlant", hdnPlant.Value), 
                        new SqlParameter("@ResultQuery", SqlDbType.VarChar, 8000, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Default, message) 
                    };
                    message = (string)daCOTS.ExecuteScalar_SP("SP_CHK_PDI_SCANDATA", sp);
                    if (!message.Contains("BARCODE LABLE "))
                    {
                        string[] splitVal = message.Split('|');
                        lblPlatform.Text = splitVal[0].ToString();
                        lblTyresize.Text = splitVal[1].ToString();
                        lblRim.Text = splitVal[2].ToString();
                        lblType.Text = splitVal[3].ToString();
                        lblBrand.Text = splitVal[4].ToString();
                        lblSidewall.Text = splitVal[5].ToString();
                        lblGrade.Text = splitVal[6].ToString();
                        lblStencil.Text = splitVal[7].ToString();
                        lblLocation.Text = splitVal[8].ToString();
                        lblBarcode.Text = splitVal[9].ToString();
                        txtBarcode.Text = "";

                        Bind_ReData(lblTyresize.Text, lblRim.Text, lblType.Text);
                    }
                    else
                        lblErrMsg.Text = message;
                }
                else
                    lblErrMsg.Text = "INVALID BARCODE";
            }
            catch (Exception ex)
            {
                if (ex.Message != "Index was outside the bounds of the array.")
                    Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void CtrlClear()
        {
            lblPlatform.Text = "";
            lblTyresize.Text = "";
            lblRim.Text = "";
            lblType.Text = "";
            lblBrand.Text = "";
            lblSidewall.Text = "";
            lblGrade.Text = "";
            lblStencil.Text = "";
            lblLocation.Text = "";
            lblBarcode.Text = "";
            lblErrMsg.Text = "";
            ScriptManager.RegisterStartupScript(Page, GetType(), "JGoto", "gotoPreviewDiv('lblCustomer');", true);
        }
        private void Bind_ReData(string strSize, string strRim, string strType)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@TyreSize", strSize), new SqlParameter("@RimSize", strRim), new SqlParameter("@TyreType", strType) };
                DataTable dtProcessIdDetails = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdi_concession_v1", sp, DataAccess.Return_Type.DataTable);
                DataView viewBrand = new DataView(dtProcessIdDetails);
                viewBrand.Sort = "Brand ASC";
                DataTable dtBrand = viewBrand.ToTable(true, "Brand");
                ddlReBrand.DataSource = dtBrand;
                ddlReBrand.DataTextField = "Brand";
                ddlReBrand.DataValueField = "Brand";
                ddlReBrand.DataBind();
                ddlReBrand.Items.Insert(0, "CHOOSE");

                DataView viewSidewall = new DataView(dtProcessIdDetails);
                viewSidewall.Sort = "Sidewall ASC";
                DataTable dtSidewall = viewSidewall.ToTable(true, "Sidewall");
                ddlReSidewall.DataSource = dtSidewall;
                ddlReSidewall.DataTextField = "Sidewall";
                ddlReSidewall.DataValueField = "Sidewall";
                ddlReSidewall.DataBind();
                ddlReSidewall.Items.Insert(0, "CHOOSE");

                DataView viewType = new DataView(dtProcessIdDetails);
                viewType.Sort = "TyreType ASC";
                DataTable dtTyreType = viewType.ToTable(true, "TyreType");
                ddlReType.DataSource = dtTyreType;
                ddlReType.DataTextField = "TyreType";
                ddlReType.DataValueField = "TyreType";
                ddlReType.DataBind();
                ddlReType.Items.Insert(0, "CHOOSE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        protected void btnSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool itemmatching = Check_OrderItem();
                string strBarcode = txtBarcode.Text != "" ? txtBarcode.Text : lblBarcode.Text;
                if (strBarcode != "" && itemmatching == true)
                {
                    if (hdnPID.Value == "")
                        SaveMasterData();

                    string message = "";
                    SqlParameter[] sp1 = new SqlParameter[]{
                        new SqlParameter("@PID",hdnPID.Value),
                        new SqlParameter("@barcode",strBarcode),
                        new SqlParameter("@MetalDetector",rdbDetector.SelectedItem.Text == "YES" ? true : false),
                        new SqlParameter("@IdGauge",rdbGauge.SelectedItem.Text == "YES" ? true : false),
                        new SqlParameter("@TreadHard",txtTread.Text),
                        new SqlParameter("@BaseHard",txtBase.Text),
                        new SqlParameter("@ReBrand",ddlReBrand.SelectedItem.Text != "CHOOSE" ? ddlReBrand.SelectedItem.Text : ""),
                        new SqlParameter("@ReSidewall",ddlReSidewall.SelectedItem.Text != "CHOOSE" ? ddlReSidewall.SelectedItem.Text : ""),
                        new SqlParameter("@ConcessionType",ddlReType.SelectedItem.Text != "CHOOSE" ? ddlReType.SelectedItem.Text : ""),
                        new SqlParameter("@BeadType",rdbBeadType.SelectedIndex !=-1 ? rdbBeadType.SelectedItem.Text : ""),
                        new SqlParameter("@QualityTest",rdbBestQuality.SelectedIndex !=-1 ? rdbBestQuality.SelectedItem.Text : ""),
                        new SqlParameter("@Remarks",txtremarks.Text),
                        new SqlParameter("@PdiPlant",hdnPlant.Value),
                        new SqlParameter("@ScanningBy",Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@ResultQuery",SqlDbType.VarChar,8000,ParameterDirection.Output,true,0,0,"",DataRowVersion.Default,message)
                    };
                    message = (string)daCOTS.ExecuteScalar_SP("sp_ins_pdi_scanbarcodelist", sp1);

                    if (message.Contains("Success"))
                    {
                        txtInspectedQty.Text = message.Replace("Success ", "");
                        lblNoOfRecords.Text = "SCAN COUNT: " + txtInspectedQty.Text + "/" + txt_OrderQty.Text;
                        if (txtInspectedQty.Text == txt_OrderQty.Text)
                        {
                            lblSucessMsg.Text = "Order Qty Completed";
                            btnSaveItem.Style.Add("display", "none");
                        }
                        else
                        {
                            lblSucessMsg.Text = "";
                            btnSaveItem.Style.Add("display", "block");
                        }

                        rdbDetector.SelectedIndex = 1;
                        rdbGauge.SelectedIndex = 1;
                        txtTread.Text = "";
                        txtBase.Text = "";
                        rdbBeadType.SelectedIndex = -1;
                        rdbBestQuality.SelectedIndex = -1;
                        txtremarks.Text = "";
                        ddlReBrand.SelectedIndex = -1;
                        ddlReSidewall.SelectedIndex = -1;
                        ddlReType.SelectedIndex = -1;
                        CtrlClear();
                    }
                    else
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JEMsg2", "bind_errmsg('" + message + "');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void SaveMasterData()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@custcode", hdnCustCode.Value), 
                    new SqlParameter("@workorderno", txt_OrderRefNo.Text), 
                    new SqlParameter("@orderqty", Convert.ToInt32(txt_OrderQty.Text)), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value), 
                    new SqlParameter("@orderrefno", lbl_OrderNo.Text),
                    new SqlParameter("@PdiPlant", hdnPlant.Value.ToUpper()),
                    new SqlParameter("@O_ID", Utilities.Decrypt(Request["id"].ToString()))
                };
                int resp = (int)daCOTS.ExecuteScalar_SP("sp_ins_pdi_scanmasterdata", sp1);
                if (resp > 0)
                    hdnPID.Value = resp.ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private bool Check_OrderItem()
        {
            bool domItemMatch = false;
            try
            {
                DataTable dtDomItemList = ViewState["dtDomItemList"] as DataTable;
                if (dtDomItemList.Rows[0]["ProcessType"].ToString() == "ORDER")
                {
                    foreach (DataRow iRow in dtDomItemList.Select("tyresize='" + lblTyresize.Text + "' and rimsize='" + lblRim.Text + "'"))
                    {
                        if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                        {
                            //if (iRow["tyresize"].ToString() == lblTyresize.Text && iRow["rimsize"].ToString() == lblRim.Text)// &&
                            //(iRow["tyretype"].ToString() == lblType.Text || iRow["sType"].ToString() == lblType.Text || iRow["sType1"].ToString() == lblType.Text) &&
                            //(iRow["brand"].ToString() == lblBrand.Text || iRow["sBrand"].ToString() == lblBrand.Text || iRow["sBrand1"].ToString() == lblBrand.Text) &&
                            //(iRow["Config"].ToString() == lblPlatform.Text || iRow["sPlatform"].ToString() == lblPlatform.Text || iRow["sPlatform1"].ToString() == lblPlatform.Text) &&
                            //(iRow["sidewall"].ToString() == lblSidewall.Text || iRow["sSidewall"].ToString() == lblSidewall.Text || iRow["sSidewall1"].ToString() == lblSidewall.Text))
                            //{
                            domItemMatch = true;
                            break;
                            //}
                        }
                        else if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                        {
                            if (iRow["tyresize"].ToString() == lblTyresize.Text && iRow["rimsize"].ToString() == lblRim.Text && iRow["tyretype"].ToString() == "-" &&
                                iRow["brand"].ToString() == "-" && (iRow["tyresize"].ToString() == "15X41/2-8LUG" || iRow["tyresize"].ToString() == "15X41/2-8GUNDULUG") &&
                                iRow["rimsize"].ToString() == "3.00" && (hdnCustCode.Value == "32" || hdnCustCode.Value == "97" || hdnCustCode.Value == "146" ||
                                hdnCustCode.Value == "159"))
                            {
                                domItemMatch = true;
                                break;
                            }
                            else if (iRow["tyresize"].ToString() == lblTyresize.Text && iRow["rimsize"].ToString() == lblRim.Text &&
                              (iRow["tyretype"].ToString() == lblType.Text || iRow["sType"].ToString() == lblType.Text || iRow["sType1"].ToString() == lblType.Text) &&
                              (iRow["brand"].ToString() == lblBrand.Text || iRow["sBrand"].ToString() == lblBrand.Text || iRow["sBrand1"].ToString() == lblBrand.Text))
                            {
                                domItemMatch = true;
                                break;
                            }
                            DataTable dtEarmarkList = ViewState["dtEarmarkList"] as DataTable;
                            if (dtEarmarkList != null && dtEarmarkList.Rows.Count > 0)
                            {
                                string strBarcode = txtBarcode.Text != "" ? txtBarcode.Text : lblBarcode.Text;
                                foreach (DataRow eRow in dtEarmarkList.Select("stencilno='" + strBarcode.Substring(8, 10) + "'"))
                                {
                                    domItemMatch = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!domItemMatch)
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JEMsg2", "bind_errmsg('BARCODE ITEM NOT MATCHED TO ORDER ITEM');", true);
                }
                else
                    domItemMatch = true;
            }
            catch (Exception ex)
            {
                domItemMatch = false;
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return domItemMatch;
        }
        protected void lnkWorkOrder_CLick(object sender, EventArgs e)
        {
            try
            {
                string linkUrl = (Server.MapPath("~/workorderfiles/" + hdnCustCode.Value + "/") + lnkWorkOrder.Text).Replace("TTS", "pdfs");
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkWorkOrder.Text);
                Response.WriteFile(linkUrl);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}