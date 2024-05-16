using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Globalization;

namespace TTS
{
    public partial class ExpLogistic_Entry : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["ot_logisticsmmn"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_logistics_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_logistics_sitl"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_logisticspdk"].ToString() == "True"))
                        {
                            if (Request["pid"] != null && Request["pid"].ToString() != "")
                            {
                                lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + " - ";
                                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())) };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_orders_logistics", sp, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    gv_Orders.DataSource = dt;
                                    gv_Orders.DataBind();
                                    Bind_PortList();
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
        private void Bind_PortList()
        {
            try
            {
                DataSet dsPort = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_exp_Logistics_PortList", DataAccess.Return_Type.DataSet);
                if (dsPort.Tables[0].Rows.Count > 0 && dsPort.Tables[1].Rows.Count > 0)
                {
                    Bind_PortList_InDropDown(ddl_ETDPort_Minus3_c1, dsPort.Tables[0], "ETDPortName");
                    Bind_PortList_InDropDown(ddl_ETDPort_Plus3_c1, dsPort.Tables[0], "ETDPortName");
                    Bind_PortList_InDropDown(ddl_ETDPort_Plus7_c1, dsPort.Tables[0], "ETDPortName");

                    Bind_PortList_InDropDown(ddl_ETDPort_Minus3_c2, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETDPort_Plus3_c2, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETDPort_Plus7_c2, dsPort.Tables[1], "ETAPortName");

                    Bind_PortList_InDropDown(ddl_ETDPort_Minus3_c3, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETDPort_Plus3_c3, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETDPort_Plus7_c3, dsPort.Tables[1], "ETAPortName");

                    Bind_PortList_InDropDown(ddl_ETAPort_Minus3_c1, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETAPort_Plus3_c1, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETAPort_Plus7_c1, dsPort.Tables[1], "ETAPortName");

                    Bind_PortList_InDropDown(ddl_ETAPort_Minus3_c2, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETAPort_Plus3_c2, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETAPort_Plus7_c2, dsPort.Tables[1], "ETAPortName");

                    Bind_PortList_InDropDown(ddl_ETAPort_Minus3_c3, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETAPort_Plus3_c3, dsPort.Tables[1], "ETAPortName");
                    Bind_PortList_InDropDown(ddl_ETAPort_Plus7_c3, dsPort.Tables[1], "ETAPortName");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_PortList_InDropDown(DropDownList ddl, DataTable dt, string valfield)
        {
            ddl.DataSource = dt;
            ddl.DataValueField = valfield;
            ddl.DataTextField = valfield;
            ddl.DataBind();
            ddl.Items.Insert(0, "CHOOSE");
        }
        protected void lnkOrderSelection_click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectOrder(clickedRow);
                ScriptManager.RegisterStartupScript(Page, GetType(), "clearctrl", "clrCtrl();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectOrder(GridViewRow clickedRow)
        {
            lnkSkipTentative.Text = "TENTATIVE SCHEDULE NOT PREPARED. MOVE TO PDI.";
            btn_StatusChange.Text = "MOVE TO PDI";
            hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
            hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;
            lblSelectedCustomerName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
            lblSelectedOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
            Bind_OrderMasterDetails();
            Bind_OrderSumValue();
            if (((HiddenField)clickedRow.FindControl("hdnOrderStatus")).Value == "38")
            {
                SqlParameter[] spDate = new SqlParameter[] { 
                    new SqlParameter("@custcode", hdnCustCode.Value), 
                    new SqlParameter("@orderrefno", lblSelectedOrderRefNo.Text), 
                    new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())) 
                };
                DateTime disDate = (DateTime)daCOTS.ExecuteScalar_SP("sp_sel_ContainerMovedOn", spDate);
                lbl_PPC_RFDdate.Text = "CONTAINER MOVED FROM FACTORY ON: " + disDate.ToShortDateString();
                hdnRfdDate.Value = disDate.AddDays(-1).ToShortDateString();
                lnkSkipTentative.Text = "";
                btn_StatusChange.Text = "MOVE TO DOCUMENT PROCESS";
                ScriptManager.RegisterStartupScript(Page, GetType(), "hidecol", "hideTwoCol();", true);
            }
            Bind_Gv();
        }
        private void Bind_OrderMasterDetails()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtMasterList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_MasterDetails_Logistics", sp1, DataAccess.Return_Type.DataTable);
                if (dtMasterList.Rows.Count > 0)
                {
                    dlOrderMaster.DataSource = dtMasterList;
                    dlOrderMaster.DataBind();
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
                    strAddress += "<div>" + row["mobile"].ToString() + "</div>";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strAddress;
        }
        private void Bind_OrderSumValue()
        {
            try
            {
                hdnRfdDate.Value = "";
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

                    string strRFD = dtSumList.Select("PLANT='" + Utilities.Decrypt(Request["pid"].ToString()) + "'")[0].ItemArray[10].ToString();
                    lbl_PPC_RFDdate.Text = Utilities.Decrypt(Request["pid"].ToString()) + " RFD: " + strRFD;
                    hdnRfdDate.Value = strRFD;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_Gv()
        {
            try
            {
                dlTentativeList.DataSource = null;
                dlTentativeList.DataBind();

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value),
                    new SqlParameter("@Dispatch_Time", btn_StatusChange.Text == "MOVE TO DOCUMENT PROCESS" ? "Actual" : "Tenative")
                };
                DataTable dtProcessOrder = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Exp_LogisticEntry", sp, DataAccess.Return_Type.DataTable);
                if (dtProcessOrder.Rows.Count > 0)
                {
                    dlTentativeList.DataSource = dtProcessOrder;
                    dlTentativeList.DataBind();
                    Session["dtProcessOrder"] = dtProcessOrder;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Sub_OrderItems');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowSaveRecords", "gotoPreviewDiv('div_TransitDetails_Gv');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Sub_OrderItems');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowSaveCntrls", "gotoPreviewDiv('div_Revise');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_ReviseRecords_click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtProcessOrder = (DataTable)Session["dtProcessOrder"];
                if (dtProcessOrder.Rows.Count > 0)
                {
                    foreach (DataRow row in dtProcessOrder.Rows)
                    {
                        if (row["Dispatch_Time"].ToString() == "Advance -3 days")
                        {
                            txt_RFDdate_Minus3.Text = dtProcessOrder.Rows[0]["RFD"].ToString();
                            txt_GateOpen_Minus3.Text = dtProcessOrder.Rows[0]["GateOpen"].ToString();
                            txt_GateClose_Minus3.Text = dtProcessOrder.Rows[0]["GateClose"].ToString();
                            txt_ETDdate_Minus3_c1.Text = dtProcessOrder.Rows[0]["ETD_1"].ToString();
                            ddl_ETDPort_Minus3_c1.SelectedIndex = ddl_ETDPort_Minus3_c1.Items.IndexOf(ddl_ETDPort_Minus3_c1.Items.FindByValue(dtProcessOrder.Rows[0]["ETD_Port_1"].ToString()));
                            txt_ETAdate_Minus3_c1.Text = dtProcessOrder.Rows[0]["ETA_1"].ToString();
                            ddl_ETAPort_Minus3_c1.SelectedIndex = ddl_ETAPort_Minus3_c1.Items.IndexOf(ddl_ETAPort_Minus3_c1.Items.FindByValue(dtProcessOrder.Rows[0]["ETA_Port_1"].ToString()));
                            txt_Vessel_Minus3_c1.Text = dtProcessOrder.Rows[0]["Vessel_1"].ToString();

                            txt_ETDdate_Minus3_c2.Text = dtProcessOrder.Rows[0]["ETD_2"].ToString();
                            ddl_ETDPort_Minus3_c2.SelectedIndex = ddl_ETDPort_Minus3_c2.Items.IndexOf(ddl_ETDPort_Minus3_c2.Items.FindByValue(dtProcessOrder.Rows[0]["ETD_Port_2"].ToString()));
                            txt_ETAdate_Minus3_c2.Text = dtProcessOrder.Rows[0]["ETA_2"].ToString();
                            ddl_ETAPort_Minus3_c2.SelectedIndex = ddl_ETAPort_Minus3_c2.Items.IndexOf(ddl_ETAPort_Minus3_c2.Items.FindByValue(dtProcessOrder.Rows[0]["ETA_Port_2"].ToString()));
                            txt_Vessel_Minus3_c2.Text = dtProcessOrder.Rows[0]["Vessel_2"].ToString();

                            txt_ETDdate_Minus3_c3.Text = dtProcessOrder.Rows[0]["ETD_3"].ToString();
                            ddl_ETDPort_Minus3_c3.SelectedIndex = ddl_ETDPort_Minus3_c3.Items.IndexOf(ddl_ETDPort_Minus3_c3.Items.FindByValue(dtProcessOrder.Rows[0]["ETD_Port_3"].ToString()));
                            txt_ETAdate_Minus3_c3.Text = dtProcessOrder.Rows[0]["ETA_3"].ToString();
                            ddl_ETAPort_Minus3_c3.SelectedIndex = ddl_ETAPort_Minus3_c3.Items.IndexOf(ddl_ETAPort_Minus3_c3.Items.FindByValue(dtProcessOrder.Rows[0]["ETA_Port_3"].ToString()));
                            txt_Vessel_Minus3_c3.Text = dtProcessOrder.Rows[0]["Vessel_3"].ToString();

                            txt_transitDays_Minus3.Text = dtProcessOrder.Rows[0]["TransitDays"].ToString();
                            txt_Destination_Minus3.Text = dtProcessOrder.Rows[0]["Destination"].ToString();
                            txt_Comments_Minus3.Text = dtProcessOrder.Rows[0]["Comments"].ToString();
                        }
                        else if (row["Dispatch_Time"].ToString() == "Delay +3 days" || row["Dispatch_Time"].ToString() == "Actual")
                        {
                            txt_RFDdate_Plus3.Text = dtProcessOrder.Rows[0]["RFD"].ToString();
                            txt_GateOpen_Plus3.Text = dtProcessOrder.Rows[0]["GateOpen"].ToString();
                            txt_GateClose_Plus3.Text = dtProcessOrder.Rows[0]["GateClose"].ToString();
                            txt_ETDdate_Plus3_c1.Text = dtProcessOrder.Rows[0]["ETD_1"].ToString();
                            ddl_ETDPort_Plus3_c1.SelectedIndex = ddl_ETDPort_Plus3_c1.Items.IndexOf(ddl_ETDPort_Plus3_c1.Items.FindByValue(dtProcessOrder.Rows[0]["ETD_Port_1"].ToString()));
                            txt_ETAdate_Plus3_c1.Text = dtProcessOrder.Rows[0]["ETA_1"].ToString();
                            ddl_ETAPort_Plus3_c1.SelectedIndex = ddl_ETAPort_Plus3_c1.Items.IndexOf(ddl_ETAPort_Plus3_c1.Items.FindByValue(dtProcessOrder.Rows[0]["ETA_Port_1"].ToString()));
                            txt_Vessel_Plus3_c1.Text = dtProcessOrder.Rows[0]["Vessel_1"].ToString();

                            txt_ETDdate_Plus3_c2.Text = dtProcessOrder.Rows[0]["ETD_2"].ToString();
                            ddl_ETDPort_Plus3_c2.SelectedIndex = ddl_ETDPort_Plus3_c2.Items.IndexOf(ddl_ETDPort_Plus3_c2.Items.FindByValue(dtProcessOrder.Rows[0]["ETD_Port_2"].ToString()));
                            txt_ETAdate_Plus3_c2.Text = dtProcessOrder.Rows[0]["ETA_2"].ToString();
                            ddl_ETAPort_Plus3_c2.SelectedIndex = ddl_ETAPort_Plus3_c2.Items.IndexOf(ddl_ETAPort_Plus3_c2.Items.FindByValue(dtProcessOrder.Rows[0]["ETA_Port_2"].ToString()));
                            txt_Vessel_Plus3_c2.Text = dtProcessOrder.Rows[0]["Vessel_2"].ToString();

                            txt_ETDdate_Plus3_c3.Text = dtProcessOrder.Rows[0]["ETD_3"].ToString();
                            ddl_ETDPort_Plus3_c3.SelectedIndex = ddl_ETDPort_Plus3_c3.Items.IndexOf(ddl_ETDPort_Plus3_c3.Items.FindByValue(dtProcessOrder.Rows[0]["ETD_Port_3"].ToString()));
                            txt_ETAdate_Plus3_c3.Text = dtProcessOrder.Rows[0]["ETA_3"].ToString();
                            ddl_ETAPort_Plus3_c3.SelectedIndex = ddl_ETAPort_Plus3_c3.Items.IndexOf(ddl_ETAPort_Plus3_c3.Items.FindByValue(dtProcessOrder.Rows[0]["ETA_Port_3"].ToString()));
                            txt_Vessel_Plus3_c3.Text = dtProcessOrder.Rows[0]["Vessel_3"].ToString();

                            txt_transitDays_Plus3.Text = dtProcessOrder.Rows[0]["TransitDays"].ToString();
                            txt_Destination_Plus3.Text = dtProcessOrder.Rows[0]["Destination"].ToString();
                            txt_Comments_Plus3.Text = dtProcessOrder.Rows[0]["Comments"].ToString();
                        }
                        else if (row["Dispatch_Time"].ToString() == "Delay +7 days")
                        {
                            txt_RFDdate_Plus7.Text = dtProcessOrder.Rows[0]["RFD"].ToString();
                            txt_GateOpen_Plus7.Text = dtProcessOrder.Rows[0]["GateOpen"].ToString();
                            txt_GateClose_Plus7.Text = dtProcessOrder.Rows[0]["GateClose"].ToString();
                            txt_ETDdate_Plus7_c1.Text = dtProcessOrder.Rows[0]["ETD_1"].ToString();
                            ddl_ETDPort_Plus7_c1.SelectedIndex = ddl_ETDPort_Plus7_c1.Items.IndexOf(ddl_ETDPort_Plus7_c1.Items.FindByValue(dtProcessOrder.Rows[0]["ETD_Port_1"].ToString()));
                            txt_ETAdate_Plus7_c1.Text = dtProcessOrder.Rows[0]["ETA_1"].ToString();
                            ddl_ETAPort_Plus7_c1.SelectedIndex = ddl_ETAPort_Plus7_c1.Items.IndexOf(ddl_ETAPort_Plus7_c1.Items.FindByValue(dtProcessOrder.Rows[0]["ETA_Port_1"].ToString()));
                            txt_Vessel_Plus7_c1.Text = dtProcessOrder.Rows[0]["Vessel_1"].ToString();

                            txt_ETDdate_Plus7_c2.Text = dtProcessOrder.Rows[0]["ETD_2"].ToString();
                            ddl_ETDPort_Plus7_c2.SelectedIndex = ddl_ETDPort_Plus7_c2.Items.IndexOf(ddl_ETDPort_Plus7_c2.Items.FindByValue(dtProcessOrder.Rows[0]["ETD_Port_2"].ToString()));
                            txt_ETAdate_Plus7_c2.Text = dtProcessOrder.Rows[0]["ETA_2"].ToString();
                            ddl_ETAPort_Plus7_c2.SelectedIndex = ddl_ETAPort_Plus7_c2.Items.IndexOf(ddl_ETAPort_Plus7_c2.Items.FindByValue(dtProcessOrder.Rows[0]["ETA_Port_2"].ToString()));
                            txt_Vessel_Plus7_c2.Text = dtProcessOrder.Rows[0]["Vessel_2"].ToString();

                            txt_ETDdate_Plus7_c3.Text = dtProcessOrder.Rows[0]["ETD_3"].ToString();
                            ddl_ETDPort_Plus7_c3.SelectedIndex = ddl_ETDPort_Plus7_c3.Items.IndexOf(ddl_ETDPort_Plus7_c3.Items.FindByValue(dtProcessOrder.Rows[0]["ETD_Port_3"].ToString()));
                            txt_ETAdate_Plus7_c3.Text = dtProcessOrder.Rows[0]["ETA_3"].ToString();
                            ddl_ETAPort_Plus7_c3.SelectedIndex = ddl_ETAPort_Plus7_c3.Items.IndexOf(ddl_ETAPort_Plus7_c3.Items.FindByValue(dtProcessOrder.Rows[0]["ETA_Port_3"].ToString()));
                            txt_Vessel_Plus7_c3.Text = dtProcessOrder.Rows[0]["Vessel_3"].ToString();

                            txt_transitDays_Plus7.Text = dtProcessOrder.Rows[0]["TransitDays"].ToString();
                            txt_Destination_Plus7.Text = dtProcessOrder.Rows[0]["Destination"].ToString();
                            txt_Comments_Plus7.Text = dtProcessOrder.Rows[0]["Comments"].ToString();
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_Sub_OrderItems');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "ShowSaveCntrls", "gotoPreviewDiv('div_Revise');", true);
                if (btn_StatusChange.Text == "MOVE TO DOCUMENT PROCESS")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "hidecol", "hideTwoCol();", true);
            }

            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_SaveRecords_Click(object sender, EventArgs e)
        {
            try
            {
                //btn Save Records for Promotion(-3days)
                if (txt_RFDdate_Minus3.Text != "")
                    SaveRecords("-3");
                //btn Save Records for Delay(+3days)
                if (txt_RFDdate_Plus3.Text != "")
                    SaveRecords("+3");
                //btn Save Records for Delay(+7days)
                if (txt_RFDdate_Plus7.Text != "")
                    SaveRecords("+7");
                ScriptManager.RegisterStartupScript(Page, GetType(), "SaveSuccess", "alert('Record Saved Succesffully');", true);
                gv_Orders.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                Build_SelectOrder(gv_Orders.SelectedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void SaveRecords(string type)
        {
            try
            {
                string strRFD = type == "-3" ? txt_RFDdate_Minus3.Text : type == "+3" ? txt_RFDdate_Plus3.Text : txt_RFDdate_Plus7.Text;
                string strGateOpen = type == "-3" ? txt_GateOpen_Minus3.Text : type == "+3" ? txt_GateOpen_Plus3.Text : txt_GateOpen_Plus7.Text;
                string strGateClose = type == "-3" ? txt_GateClose_Minus3.Text : type == "+3" ? txt_GateClose_Plus3.Text : txt_GateClose_Plus7.Text;
                //loop 1
                string strETD_loop1 = type == "-3" ? txt_ETDdate_Minus3_c1.Text : type == "+3" ? txt_ETDdate_Plus3_c1.Text : txt_ETDdate_Plus7_c1.Text;
                string strETDPort_loop1 = type == "-3" ? ddl_ETDPort_Minus3_c1.Text : type == "+3" ? ddl_ETDPort_Plus3_c1.Text : ddl_ETDPort_Plus7_c1.Text;
                string strETA_loop1 = type == "-3" ? txt_ETAdate_Minus3_c1.Text : type == "+3" ? txt_ETAdate_Plus3_c1.Text : txt_ETAdate_Plus7_c1.Text;
                string strETAPort_loop1 = type == "-3" ? ddl_ETAPort_Minus3_c1.Text : type == "+3" ? ddl_ETAPort_Plus3_c1.Text : ddl_ETAPort_Plus7_c1.Text;
                string strVessel_loop1 = type == "-3" ? txt_Vessel_Minus3_c1.Text : type == "+3" ? txt_Vessel_Plus3_c1.Text : txt_Vessel_Plus7_c1.Text;
                //loop 2
                string strETD_loop2 = type == "-3" ? txt_ETDdate_Minus3_c2.Text : type == "+3" ? txt_ETDdate_Plus3_c2.Text : txt_ETDdate_Plus7_c2.Text;
                string strETDPort_loop2 = type == "-3" ? ddl_ETDPort_Minus3_c2.Text : type == "+3" ? ddl_ETDPort_Plus3_c2.Text : ddl_ETDPort_Plus7_c2.Text;
                string strETA_loop2 = type == "-3" ? txt_ETAdate_Minus3_c2.Text : type == "+3" ? txt_ETAdate_Plus3_c2.Text : txt_ETAdate_Plus7_c2.Text;
                string strETAPort_loop2 = type == "-3" ? ddl_ETAPort_Minus3_c2.Text : type == "+3" ? ddl_ETAPort_Plus3_c2.Text : ddl_ETAPort_Plus7_c2.Text;
                string strVessel_loop2 = type == "-3" ? txt_Vessel_Minus3_c2.Text : type == "+3" ? txt_Vessel_Plus3_c2.Text : txt_Vessel_Plus7_c2.Text;
                //loop 2
                string strETD_loop3 = type == "-3" ? txt_ETDdate_Minus3_c3.Text : type == "+3" ? txt_ETDdate_Plus3_c3.Text : txt_ETDdate_Plus7_c3.Text;
                string strETDPort_loop3 = type == "-3" ? ddl_ETDPort_Minus3_c3.Text : type == "+3" ? ddl_ETDPort_Plus3_c3.Text : ddl_ETDPort_Plus7_c3.Text;
                string strETA_loop3 = type == "-3" ? txt_ETAdate_Minus3_c3.Text : type == "+3" ? txt_ETAdate_Plus3_c3.Text : txt_ETAdate_Plus7_c3.Text;
                string strETAPort_loop3 = type == "-3" ? ddl_ETAPort_Minus3_c3.Text : type == "+3" ? ddl_ETAPort_Plus3_c3.Text : ddl_ETAPort_Plus7_c3.Text;
                string strVessel_loop3 = type == "-3" ? txt_Vessel_Minus3_c3.Text : type == "+3" ? txt_Vessel_Plus3_c3.Text : txt_Vessel_Plus7_c3.Text;

                string strTransitDays = type == "-3" ? hdn_Days_Minus3.Value : type == "+3" ? hdn_Days_Plus3.Value : hdn_Days_Plus7.Value;
                string strDestination = type == "-3" ? txt_Destination_Minus3.Text : type == "+3" ? txt_Destination_Plus3.Text : txt_Destination_Plus7.Text;
                string strComments = type == "-3" ? txt_Comments_Minus3.Text : type == "+3" ? txt_Comments_Plus3.Text : txt_Comments_Plus7.Text;
                string Dispatch_Time = type == "-3" ? "Advance -3 days" : type == "+3" ? "Delay +3 days" : "Delay +7 days";

                SqlParameter[] sp_loop1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value), 
                    new SqlParameter("@Dispatch_Time", btn_StatusChange.Text == "MOVE TO DOCUMENT PROCESS" ? "Actual" : Dispatch_Time), 
                    new SqlParameter("@Active_Status", 1), 
                    new SqlParameter("@RFD", DateTime.ParseExact(strRFD, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                    new SqlParameter("@GateOpen", DateTime.ParseExact(strGateOpen, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                    new SqlParameter("@GateClose", DateTime.ParseExact(strGateClose, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                    new SqlParameter("@ETD_1", DateTime.ParseExact(strETD_loop1, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                    new SqlParameter("@ETD_Port_1", strETDPort_loop1), 
                    new SqlParameter("@ETA_1", DateTime.ParseExact(strETA_loop1, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                    new SqlParameter("@ETA_Port_1", strETAPort_loop1), 
                    new SqlParameter("@Vessel_1", strVessel_loop1), 
                    new SqlParameter("@TransitDays", strTransitDays), 
                    new SqlParameter("@Destination", strDestination),
                    new SqlParameter("@Comments", strComments)
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_Exp_LogisticEntry", sp_loop1);
                if (resp > 0)
                {
                    if (strETD_loop2 != "")
                    {
                        SqlParameter[] sp_loop2 = new SqlParameter[] { 
                            new SqlParameter("@O_ID", hdnOID.Value), 
                            new SqlParameter("@ETD_2", DateTime.ParseExact(strETD_loop2, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                            new SqlParameter("@ETD_Port_2", strETDPort_loop2), 
                            new SqlParameter("@ETA_2", DateTime.ParseExact(strETA_loop2, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                            new SqlParameter("@ETA_Port_2", strETAPort_loop2), 
                            new SqlParameter("@Vessel_2", strVessel_loop2)
                        };
                        daCOTS.ExecuteNonQuery_SP("sp_upt_Exp_LogisticEntry1", sp_loop2);
                    }
                    if (strETD_loop3 != "")
                    {
                        SqlParameter[] sp_loop3 = new SqlParameter[] 
                        { 
                            new SqlParameter("@O_ID", hdnOID.Value), 
                            new SqlParameter("@ETD_3", DateTime.ParseExact(strETD_loop3, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                            new SqlParameter("@ETD_Port_3", strETDPort_loop3), 
                            new SqlParameter("@ETA_3", DateTime.ParseExact(strETA_loop3, "dd/MM/yyyy", CultureInfo.InvariantCulture)), 
                            new SqlParameter("@ETA_Port_3", strETAPort_loop3), 
                            new SqlParameter("@Vessel_3", strVessel_loop3)
                        };
                        daCOTS.ExecuteNonQuery_SP("sp_upt_Exp_LogisticEntry2", sp_loop3);
                    }
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
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@statusid", btn_StatusChange.Text == "MOVE TO PDI" ? Convert.ToInt32(19) : Convert.ToInt32(9)), 
                    new SqlParameter("@feedback", txt_StatusChangeComments.Text), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                };
                int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
                if (resp1 > 0)
                    Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkSkipTentative_Click(object sender, EventArgs e)
        {
            SqlParameter[] sp1 = new SqlParameter[] 
                { 
                    new SqlParameter("@O_ID", hdnOID.Value), 
                    new SqlParameter("@statusid", Convert.ToInt32(19)), 
                    new SqlParameter("@feedback", "TENTATIVE SCHEDULE NOT PREPARED"), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)
                };
            int resp1 = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_StatusChangedDetails", sp1);
            if (resp1 > 0)
                Response.Redirect(Request.RawUrl, false);
        }
    }
}