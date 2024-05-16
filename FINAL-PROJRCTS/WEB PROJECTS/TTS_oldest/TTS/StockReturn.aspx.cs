using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.Globalization;

namespace TTS
{
    public partial class StockReturn : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["ot_qcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True"))
                        {
                            lblPageHead.Text = Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ") + "STOCK RETURN";

                            ddlInventoryPlant.SelectedIndex = ddlInventoryPlant.Items.IndexOf(ddlInventoryPlant.Items.FindByText("" + Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + ""));
                            SqlParameter[] sp = new SqlParameter[] { 
                                new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                                new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())) 
                            };
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatched_year", sp, DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                ddlDispatchYear.DataSource = dt;
                                ddlDispatchYear.DataTextField = "loadedon";
                                ddlDispatchYear.DataValueField = "loadedon";
                                ddlDispatchYear.DataBind();
                                if (dt.Rows.Count == 1)
                                    ddlDispatchYear_IndexChanged(null, null);
                                else
                                    ddlDispatchYear.Items.Insert(0, "CHOOSE"); ;
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlDispatchYear_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlDispatchMonth.DataSource = "";
                ddlDispatchMonth.DataBind();
                ddl_CustomerName.DataSource = "";
                ddl_CustomerName.DataBind();
                ddl_UserID.DataSource = "";
                ddl_UserID.DataBind();
                ddl_OrderRefNo.DataSource = "";
                ddl_OrderRefNo.DataBind();
                ddl_InvoiceNo.DataSource = "";
                ddl_InvoiceNo.DataBind();
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                if (ddlDispatchYear.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                        new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@Year", ddlDispatchYear.SelectedItem.Text) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatched_month", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlDispatchMonth.DataSource = dt;
                        ddlDispatchMonth.DataTextField = "loadedmonthname";
                        ddlDispatchMonth.DataValueField = "loadedMonth";
                        ddlDispatchMonth.DataBind();
                        if (dt.Rows.Count == 1)
                            ddlDispatchMonth_IndexChanged(null, null);
                        else
                            ddlDispatchMonth.Items.Insert(0, "CHOOSE"); ;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlDispatchMonth_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_CustomerName.DataSource = "";
                ddl_CustomerName.DataBind();
                ddl_UserID.DataSource = "";
                ddl_UserID.DataBind();
                ddl_OrderRefNo.DataSource = "";
                ddl_OrderRefNo.DataBind();
                ddl_InvoiceNo.DataSource = "";
                ddl_InvoiceNo.DataBind();
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                    new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())), 
                    new SqlParameter("@Year", ddlDispatchYear.SelectedItem.Text), 
                    new SqlParameter("@Month", ddlDispatchMonth.SelectedItem.Value) 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatched_customer", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddl_CustomerName.DataSource = dt;
                    ddl_CustomerName.DataTextField = "customer";
                    ddl_CustomerName.DataValueField = "customer";
                    ddl_CustomerName.DataBind();
                    if (dt.Rows.Count == 1)
                        ddl_CustomerName_SelectedIndexChanged(null, null);
                    else
                        ddl_CustomerName.Items.Insert(0, "CHOOSE"); ;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_CustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_UserID.DataSource = "";
                ddl_UserID.DataBind();
                ddl_OrderRefNo.DataSource = "";
                ddl_OrderRefNo.DataBind();
                ddl_InvoiceNo.DataSource = "";
                ddl_InvoiceNo.DataBind();
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                if (ddl_CustomerName.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddl_CustomerName.SelectedItem.Text), 
                        new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString())),
                        new SqlParameter("@Year", ddlDispatchYear.SelectedItem.Text),
                        new SqlParameter("@Month", ddlDispatchMonth.SelectedItem.Value)
                    };
                    DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_userid_dispatch_return_Customer", sp1, DataAccess.Return_Type.DataTable);
                    ddl_UserID.DataSource = dtUserList;
                    ddl_UserID.DataTextField = "username";
                    ddl_UserID.DataValueField = "ID";
                    ddl_UserID.DataBind();
                    if (dtUserList.Rows.Count == 1)
                        ddl_UserID_SelectedIndexChanged(sender, e);
                    else
                        ddl_UserID.Items.Insert(0, "CHOOSE");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_UserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddl_OrderRefNo.DataSource = "";
                ddl_OrderRefNo.DataBind();
                ddl_InvoiceNo.DataSource = "";
                ddl_InvoiceNo.DataBind();
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                if (ddl_UserID.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                        new SqlParameter("@Year", ddlDispatchYear.SelectedItem.Text), 
                        new SqlParameter("@Month", ddlDispatchMonth.SelectedItem.Value), 
                        new SqlParameter("@custcode", ddl_UserID.SelectedItem.Value) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatch_return_refno", sp1, DataAccess.Return_Type.DataTable);
                    ViewState["ds"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        Utilities.ddl_Binding(ddl_OrderRefNo, dt, "orderrefno", "ID", "CHOOSE");
                        if (ddl_OrderRefNo.Items.Count == 2)
                        {
                            ddl_OrderRefNo.SelectedIndex = 1;
                            ddl_OrderRefNo_SelectedIndexChanged(null, null);
                        }
                        else
                            Utilities.ddl_Binding(ddl_InvoiceNo, dt, "invoiceno", "ID", "CHOOSE");
                    }
                    else
                        ScriptManager.RegisterStartupScript(Page, GetType(), "EmptyAlert", "alert('No Records to show');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_OrderRefNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clear();
                DataTable dt = (DataTable)ViewState["ds"];
                string strCond = "ID='" + ddl_OrderRefNo.SelectedItem.Value + "'";
                DataView dtView = new DataView(dt, strCond, "invoiceno", DataViewRowState.CurrentRows);
                ddl_InvoiceNo.DataTextField = "invoiceno";
                ddl_InvoiceNo.DataValueField = "ID";
                ddl_InvoiceNo.DataSource = dtView;
                ddl_InvoiceNo.DataBind();
                Bind_gv();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_InvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clear();
                DataTable dt = (DataTable)ViewState["ds"];
                string strCond = "ID='" + ddl_InvoiceNo.SelectedItem.Value + "'";
                DataView dtView = new DataView(dt, strCond, "invoiceno", DataViewRowState.CurrentRows);
                DataTable dTable;
                dTable = dtView.ToTable();
                ddl_OrderRefNo.DataTextField = "orderrefno";
                ddl_OrderRefNo.DataValueField = "ID";
                ddl_OrderRefNo.DataSource = dTable;
                ddl_OrderRefNo.DataBind();
                Bind_gv();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_gv()
        {
            try
            {
                gvStockDetails.DataSource = null;
                gvStockDetails.DataBind();
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())), 
                    new SqlParameter("@PdiPlant", Utilities.Decrypt(Request["pid"].ToString()).ToUpper()), 
                    new SqlParameter("@Year", ddlDispatchYear.SelectedItem.Text), 
                    new SqlParameter("@Month", ddlDispatchMonth.SelectedItem.Value), 
                    new SqlParameter("@O_ID", ddl_OrderRefNo.SelectedItem.Value) 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dispatch_return_stencillist", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvStockDetails.DataSource = dt;
                    gvStockDetails.DataBind();

                    txt_ReturnQty.Text = dt.Rows.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveRecords_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@DcNoDate", DateTime.ParseExact(txt_DcnoDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    new SqlParameter("@DateOfReceipt", DateTime.ParseExact(txt_DateofReceipt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                    new SqlParameter("@O_ID", ddl_OrderRefNo.SelectedItem.Value), 
                    new SqlParameter("@ReturnQty", txt_ReturnQty.Text), 
                    new SqlParameter("@DcNo", txt_DcNO.Text), 
                    new SqlParameter("@Remarks", txt_Remarks.Text), 
                    new SqlParameter("@ReturnForReason", txt_ReturnforReason.Text), 
                    new SqlParameter("@ReturnBy", Request.Cookies["TTSUser"].Value), 
                    new SqlParameter("@Plant", ddlInventoryPlant.SelectedItem.Text)
                };
                object returnID = (object)daCOTS.ExecuteScalar_SP("sp_ins_dispatch_return_master", sp);
                if (returnID.ToString() != "" && Convert.ToInt32(returnID.ToString()) > 0)
                {
                    foreach (GridViewRow row in gvStockDetails.Rows)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chk_item") as CheckBox);
                        if (chkRow.Checked)
                        {
                            SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@Barcode", row.Cells[9].Text), new SqlParameter("@ReturnID", returnID.ToString()) };
                            int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_dispatch_return_stencillist", sp1);
                        }
                    }
                    Response.Redirect(Request.RawUrl, false);
                }
                else
                    ScriptManager.RegisterStartupScript(Page, GetType(), "SaveSuccess", "alert('DC NO ALREADY EXIST FOR THIS INVOICE');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public void clear()
        {
            try
            {
                txt_DateofReceipt.Text = "";
                txt_DcNO.Text = "";
                txt_DcnoDate.Text = "";
                txt_Remarks.Text = "";
                txt_ReturnforReason.Text = "";
                txt_ReturnQty.Text = "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}