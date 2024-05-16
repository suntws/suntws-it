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
using System.Globalization;
namespace TTS
{
    public partial class ExpPPC_Verification : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sitl"].ToString() == "True"
                            || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True" || dtUser.Rows[0]["ot_qcmmn"].ToString() == "True"
                            || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Utilities.Decrypt(Request["pid"].ToString()) != "" && Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ");
                                SqlParameter[] sp1 = new SqlParameter[] { 
                                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                                    new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())) 
                                };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_ppc_verification_orders", sp1, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gv_Orders.DataSource = dt;
                                    gv_Orders.DataBind();
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL IS WRONG";
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
        protected void lnkOrderSelection_click(object sender, EventArgs e)
        {
            GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
            hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
            hdnCompleteStatus.Value = "0";

            //if (((HiddenField)clickedRow.FindControl("hdnOrderStatus")).Value == "35")
            //    Response.Redirect("expppc_stencilapproval.aspx?pid=" + Request["pid"].ToString() + "&fid=" + Request["fid"].ToString() + "&oid=" +
            //        Utilities.Encrypt(((HiddenField)clickedRow.FindControl("hdnOrderID")).Value), false);
            //else
            Bind_SelectOrderDetails(clickedRow);
        }
        private void Bind_SelectOrderDetails(GridViewRow clickedRow)
        {
            try
            {
                lblOrderType.Text = "";
                lblSelectedCustomerName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblSelectedOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
                hdnSelectStatus.Value = ((HiddenField)clickedRow.FindControl("hdnOrderStatus")).Value;
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;
                lblExpectedShipDate.Text = ((HiddenField)clickedRow.FindControl("hdnExpectedShipDate")).Value;

                Bind_OrderItemList();
                Bind_DefaultValues();
                Bind_WorkOrderFiles();
                if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                {
                    lblOrderType.Text = "THIS IS " + ((Label)clickedRow.FindControl("lblShipmentType")).Text + " SHIPMENT";
                    if (((Label)clickedRow.FindControl("lblShipmentType")).Text == "COMBI")
                    {
                        lblOrderType.Text += " AND FINAL LOADING FROM " + ((HiddenField)clickedRow.FindControl("hdnContainerLoadFrom")).Value;
                        Bind_AnotherPlantOrderItem();
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Sub_OrderItems');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderItemList()
        {
            try
            {
                gv_OrderItems.DataSource = null;
                gv_OrderItems.DataBind();
                SqlParameter[] sp = new SqlParameter[]
                { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) 
                };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Stock_export_orderitem_list_PPC", sp, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gv_OrderItems.DataSource = dtItemList;
                    gv_OrderItems.DataBind();

                    gv_OrderItems.FooterRow.Cells[7].Text = "Total";
                    gv_OrderItems.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                    gv_OrderItems.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[12].Text = dtItemList.Compute("Sum(O_Newproduction)", "").ToString();
                    gv_OrderItems.FooterRow.Cells[16].Text = Convert.ToDecimal(dtItemList.Compute("Sum(finishedwt)", "")).ToString();

                    gv_OrderItems.Columns[13].Visible = false;
                    gv_OrderItems.Columns[14].Visible = false;
                    gv_OrderItems.Columns[15].Visible = false;
                    foreach (DataRow row1 in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gv_OrderItems.Columns[13].Visible = true;
                        gv_OrderItems.Columns[14].Visible = true;
                        gv_OrderItems.Columns[15].Visible = true;
                        gv_OrderItems.FooterRow.Cells[14].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_AnotherPlantOrderItem()
        {
            try
            {
                gvCombiOrderItem.DataSource = null;
                gvCombiOrderItem.DataBind();
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) 
                };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_orderitem_CombiPlant_PPC", sp, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvCombiOrderItem.DataSource = dtItemList;
                    gvCombiOrderItem.DataBind();

                    lblCombiOrder.Text = "ANOTHER PLANT ITEMS FOR THIS ORDER";
                    spandisplay.InnerText = "SHOW";

                    gvCombiOrderItem.FooterRow.Cells[7].Text = "Total";
                    gvCombiOrderItem.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;

                    gvCombiOrderItem.FooterRow.Cells[8].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gvCombiOrderItem.FooterRow.Cells[13].Text = Convert.ToDecimal(dtItemList.Compute("Sum(finishedwt)", "")).ToString();

                    gvCombiOrderItem.Columns[10].Visible = false;
                    gvCombiOrderItem.Columns[11].Visible = false;
                    gvCombiOrderItem.Columns[12].Visible = false;
                    foreach (DataRow row1 in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvCombiOrderItem.Columns[10].Visible = true;
                        gvCombiOrderItem.Columns[11].Visible = true;
                        gvCombiOrderItem.Columns[12].Visible = true;
                        gvCombiOrderItem.FooterRow.Cells[11].Text = dtItemList.Compute("Sum(Rimitemqty)", "").ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_DefaultValues()
        {
            try
            {
                rdo_ProdFeasibility.SelectedIndex = 0;
                rdo_LoadFeasibility.SelectedIndex = 0;
                rdo_EquipmentReq.SelectedIndex = 0;
                rdo_TechReq.SelectedIndex = 0;
                rdo_PurchaseReq.SelectedIndex = 0;

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Exp_PPC_TechnicalDetails", sp, DataAccess.Return_Type.DataSet);
                if (ds != null && ds.Tables.Count == 6 && ds.Tables[5].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rdo_ProdFeasibility.SelectedIndex = rdo_ProdFeasibility.Items.IndexOf(rdo_ProdFeasibility.Items.FindByText(ds.Tables[0].Rows[0]["Tech_Status"].ToString()));
                        if (rdo_ProdFeasibility.SelectedItem.Text == "NOT OK")
                            ScriptManager.RegisterStartupScript(Page, GetType(), "ShowFeasiblity", "document.getElementById('div_Feasablity').style.display = 'block';", true);
                        txt_ProdFeasiblityComments.Text = ds.Tables[0].Rows[0]["Req_Comments"].ToString();
                    }
                    else if (ds.Tables[1].Rows.Count > 0)
                    {
                        rdo_LoadFeasibility.SelectedIndex = rdo_LoadFeasibility.Items.IndexOf(rdo_LoadFeasibility.Items.FindByText(ds.Tables[1].Rows[0]["Tech_Status"].ToString()));
                        if (rdo_LoadFeasibility.SelectedItem.Text == "NOT OK")
                            ScriptManager.RegisterStartupScript(Page, GetType(), "ShowFeasiblity", "document.getElementById('div_LoadFeasibility').style.display = 'block';", true);
                        txt_LoadFeasibilityComments.Text = ds.Tables[1].Rows[0]["Req_Comments"].ToString();
                    }
                    else if (ds.Tables[2].Rows.Count > 0)
                    {
                        rdo_EquipmentReq.SelectedIndex = rdo_EquipmentReq.Items.IndexOf(rdo_EquipmentReq.Items.FindByText(ds.Tables[2].Rows[0]["Tech_Status"].ToString()));
                        if (rdo_EquipmentReq.SelectedItem.Text == "YES")
                            ScriptManager.RegisterStartupScript(Page, GetType(), "ShowRequirement", "document.getElementById('div_EquipmentReq').style.display = 'block';", true);
                        txt_EquipmentReqComments.Text = ds.Tables[2].Rows[0]["Req_Comments"].ToString();
                    }
                    else if (ds.Tables[3].Rows.Count > 0)
                    {
                        rdo_TechReq.SelectedIndex = rdo_TechReq.Items.IndexOf(rdo_TechReq.Items.FindByText(ds.Tables[3].Rows[0]["Tech_Status"].ToString()));
                        if (rdo_TechReq.SelectedItem.Text == "YES")
                            ScriptManager.RegisterStartupScript(Page, GetType(), "ShowTechReq", "document.getElementById('div_TechReq').style.display = 'block';", true);
                        txt_TechReqComments.Text = ds.Tables[3].Rows[0]["Req_Comments"].ToString();
                    }
                    else if (ds.Tables[4].Rows.Count > 0)
                    {
                        rdo_PurchaseReq.SelectedIndex = rdo_PurchaseReq.Items.IndexOf(rdo_PurchaseReq.Items.FindByText(ds.Tables[4].Rows[0]["Tech_Status"].ToString()));
                        if (rdo_PurchaseReq.SelectedItem.Text == "YES")
                            ScriptManager.RegisterStartupScript(Page, GetType(), "ShowTechReq", "document.getElementById('div_PurchaseReq').style.display = 'block';", true);
                        txt_PurchaseReqComments.Text = ds.Tables[4].Rows[0]["Req_Comments"].ToString();
                    }
                    txt_RFD.Text = ds.Tables[5].Rows[0]["RFD_Date"].ToString();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowMoveCntrls", "DisableAllCtrls();", true);
                }
                if ((ds == null || ds.Tables[5].Rows.Count == 0 || hdnSelectStatus.Value == "32" || txt_RFD.Text == "") && hdnCompleteStatus.Value == "0")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowSaveCntrls", "EnableAllCtrls();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_WorkOrderFiles()
        {
            SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value), new SqlParameter("@Qstring", "production") };
            DataTable dtPdfFile = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_pdf_AttachInvoiceFiles", sp1, DataAccess.Return_Type.DataTable);
            if (dtPdfFile != null && dtPdfFile.Rows.Count > 0)
            {
                gv_DownloadFiles.DataSource = dtPdfFile;
                gv_DownloadFiles.DataBind();
            }
        }
        protected void btn_SaveRecords_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtPPC = new DataTable();
                dtPPC.Columns.Add(new DataColumn("processid", typeof(System.String)));
                dtPPC.Columns.Add(new DataColumn("ItemQty", typeof(System.Int32)));
                dtPPC.Columns.Add(new DataColumn("RimItemQty", typeof(System.Int32)));
                dtPPC.Columns.Add(new DataColumn("AssyRimstatus", typeof(System.Boolean)));
                dtPPC.Columns.Add(new DataColumn("EdcNo", typeof(System.String)));
                dtPPC.Columns.Add(new DataColumn("Newproduction", typeof(System.Int32)));
                foreach (GridViewRow row in gv_OrderItems.Rows)
                {
                    TextBox txt_AvalQty = (TextBox)row.FindControl("txt_AvalQty");
                    TextBox txt_AvalRimQty = (TextBox)row.FindControl("txt_AvalRimQty");
                    HiddenField hdnProcessID = (HiddenField)row.FindControl("hdnProcessID");
                    HiddenField hdnEdcNo = (HiddenField)row.FindControl("hdnEdcNo");
                    Boolean boolAssyStatus = ((Label)row.FindControl("lblAssyStatus")).Text == " (ASSY)" ? true : false;
                    Label lblQty = row.FindControl("lblQty") as Label;
                    int intNewProd = Convert.ToInt32(Convert.ToInt32(lblQty.Text) - Convert.ToInt32(txt_AvalQty.Text));
                    dtPPC.Rows.Add(hdnProcessID.Value, txt_AvalQty.Text, boolAssyStatus ? txt_AvalRimQty.Text : "0", boolAssyStatus, hdnEdcNo.Value, intNewProd);
                }
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@OrderItemList_ProcessID_dt", dtPPC), 
                    new SqlParameter("@RFD_Date", DateTime.ParseExact(txt_RFD.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)) 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_Upd_Exp_PPC_AvailableQty_OrderItemList", sp);
                if (rdo_ProdFeasibility.SelectedIndex == 1)
                {
                    // to save production feasiblity
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@Tech_Type", "Production feasiblity"), 
                        new SqlParameter("@Tech_Status", rdo_ProdFeasibility.Text), 
                        new SqlParameter("@Req_Comments", txt_ProdFeasiblityComments.Text), 
                        new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value) };
                    int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_Exp_PPC_TechnicalDetails", sp1);
                }
                if (rdo_LoadFeasibility.SelectedIndex == 1)
                {
                    //to save loading feasibility
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@Tech_Type", "Loading feasibility"),
                        new SqlParameter("@Tech_Status", rdo_LoadFeasibility.Text),
                        new SqlParameter("@Req_Comments", txt_LoadFeasibilityComments.Text), 
                        new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value)
                    };
                    int resp2 = daCOTS.ExecuteNonQuery_SP("sp_ins_Exp_PPC_TechnicalDetails", sp1);
                }
                if (rdo_EquipmentReq.SelectedIndex == 1)
                {
                    //to save Equipments Requirement
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@Tech_Type", "Equipments requirement"),
                        new SqlParameter("@Tech_Status", rdo_EquipmentReq.Text),
                        new SqlParameter("@Req_Comments", txt_EquipmentReqComments.Text), 
                        new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value)
                    };
                    int resp3 = daCOTS.ExecuteNonQuery_SP("sp_ins_Exp_PPC_TechnicalDetails", sp1);
                }
                if (rdo_TechReq.SelectedIndex == 1)
                {
                    //to save EDC/QC Requirement
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@Tech_Type", "EDC/QC requirement"),
                        new SqlParameter("@Tech_Status", rdo_TechReq.Text),
                        new SqlParameter("@Req_Comments", txt_TechReqComments.Text), 
                        new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value)
                    };
                    int resp4 = daCOTS.ExecuteNonQuery_SP("sp_ins_Exp_PPC_TechnicalDetails", sp1);
                }
                if (rdo_PurchaseReq.SelectedIndex == 1)
                {
                    //to save Purchase Requirement
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@O_ID", hdnOID.Value), 
                        new SqlParameter("@Tech_Type", "Purchase requirement"),
                        new SqlParameter("@Tech_Status", rdo_PurchaseReq.Text),
                        new SqlParameter("@Req_Comments", txt_PurchaseReqComments.Text), 
                        new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value) 
                    };
                    int resp5 = daCOTS.ExecuteNonQuery_SP("sp_ins_Exp_PPC_TechnicalDetails1", sp1);
                }
                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "SaveSuccess", "alert('Record Saved Succesffully');", true);
                    hdnCompleteStatus.Value = "1";
                    gv_Orders.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                    Bind_SelectOrderDetails(gv_Orders.SelectedRow);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_StatusChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                {
                    SqlParameter[] sp1 = new SqlParameter[] {
                        new SqlParameter("@O_ID", hdnOID.Value),
                        new SqlParameter("@statusid", Convert.ToInt32(19)),
                        new SqlParameter("@feedback", txt_StatusChangeComments.Text.Replace("\r\n", "~")),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                    if (resp > 0)
                        Response.Redirect(Request.RawUrl, false);
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
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                LinkButton lnkTxt = sender as LinkButton;
                string linkUrl = Server.MapPath("~/workorderfiles/" + hdnCustCode.Value + "/").Replace("TTS", "pdfs");
                string path = linkUrl + lnkTxt.Text;
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
    }
}