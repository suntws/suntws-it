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
using System.Text;
using System.Web.Configuration;

namespace TTS
{
    public partial class Cots_ManualOrderPrepare_01 : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Session["Reset"] = true;
                    Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.Config");
                    SessionStateSection section = (SessionStateSection)config.GetSection("system.web/sessionState");
                    int timeout = (int)section.Timeout.TotalMinutes * 1000 * 60;
                    ClientScript.RegisterStartupScript(this.GetType(), "SessionAlert", "SessionExpireAlert(" + timeout + ");", true);

                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["dom_orderentry"].ToString() == "True" || dtUser.Rows[0]["exp_orderentry"].ToString() == "True"))
                        {
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                            DataTable dtMasterList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_vs2_OrderMasterdetails", sp, DataAccess.Return_Type.DataTable);
                            if (dtMasterList.Rows.Count > 0)
                            {
                                hdn_CustCode.Value = dtMasterList.Rows[0]["custcode"].ToString();
                                SqlParameter[] spPre = new SqlParameter[] { new SqlParameter("@Custcode", hdn_CustCode.Value) };
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PreDispatchedItem_OrderitemList", spPre, DataAccess.Return_Type.DataTable);
                                if (dt.Rows.Count > 0)
                                {
                                    lblPreText.Text = "Previously Bought Items";
                                    gv_Jathagam.DataSource = dt;
                                    gv_Jathagam.DataBind();

                                    if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                                    {
                                        frmOrderMasterDetails_Dom.DataSource = dtMasterList;
                                        frmOrderMasterDetails_Dom.DataBind();
                                    }
                                    else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                                    {
                                        frmOrderMasterDetails_Exp.DataSource = dtMasterList;
                                        frmOrderMasterDetails_Exp.DataBind();
                                    }

                                    DataTable dt_SelectedItems = new DataTable();
                                    sp = new SqlParameter[] { new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())) };
                                    dt_SelectedItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_incompleteorder_selecteditem", sp, DataAccess.Return_Type.DataTable);
                                    if (dt_SelectedItems != null && dt_SelectedItems.Rows.Count > 0)
                                    {
                                        Build_gv_SelectedItems(dt_SelectedItems);
                                        foreach (GridViewRow row in gv_Jathagam.Rows)
                                        {
                                            foreach (DataRow dr in dt_SelectedItems.Select("ProcessID='" + ((HiddenField)row.FindControl("hdn_ProcessId")).Value + "'"))
                                            {
                                                ((CheckBox)row.FindControl("chk_jathagam")).Checked = true;
                                            }
                                        }
                                    }
                                    else if (dt_SelectedItems == null)
                                    {
                                        string[] arrColumns = { "Category", "Config", "Brand", "Sidewall", "TyreType", "TyreSize", "RimSize", "ProcessID", "FinishedWt", "Discount", 
                                                              "ItemCode", "AssyRimstatus", "itemqty", "Discount", "unitprice", "SheetPrice", "Rimfinishedwt", "Rimunitprice", 
                                                              "RimDwg","AdditionalInfo","EdcNo" };
                                        foreach (var col in arrColumns)
                                            dt_SelectedItems.Columns.Add(col);
                                    }
                                    ViewState["dt_SelectedItems"] = dt_SelectedItems;
                                }
                                else
                                    btn_SkipItems_Click(null, null);
                            }
                            else
                                btn_SkipItems_Click(null, null);
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
        public string Bind_BillingAddress(string BillID, bool isShipAddress)
        {
            try
            {
                string strAddress = string.Empty;
                DataTable dtAddressList = DomesticScots.Bind_BillingAddress(BillID);
                if (dtAddressList.Rows.Count > 0)
                {
                    DataRow row = dtAddressList.Rows[0];
                    strAddress += "<div style='font-weight:bold;'>M/S. " + row["CompanyName"].ToString() + "</div>";
                    strAddress += "<div>" + row["shipaddress"].ToString().Replace("~", "<br/>") + "</div>";
                    strAddress += "<div>" + row["city"].ToString() + ", " + row["statename"].ToString() + "</div>";
                    strAddress += "<div>" + row["country"].ToString() + " - " + row["zipcode"].ToString() + "</div>";
                    strAddress += "<div>" + row["contact_name"].ToString() + "</div>";
                    strAddress += "<div>" + row["EmailID"].ToString() + " / " + row["mobile"].ToString() + "</div>";
                }
                return strAddress;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                return "";
            }
        }
        protected void btn_SelectItem_Click(object sender, EventArgs e)
        {
            try
            {
                //btn to select items
                StringBuilder sb = new StringBuilder();
                lbl_ErrMsg1.Text = "";
                DataTable dt_SelectedItems = (DataTable)ViewState["dt_SelectedItems"];
                foreach (GridViewRow row in gv_Jathagam.Rows)
                {
                    if (((CheckBox)row.FindControl("chk_jathagam")).Checked)
                    {
                        if (dt_SelectedItems.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt_SelectedItems.Rows)
                            {
                                if (dr["ProcessID"].ToString() == ((HiddenField)row.FindControl("hdn_ProcessId")).Value)
                                {
                                    if (sb.ToString() == "")
                                        sb.Append("Below items are already added. You can change the qty or delete this item in final stage");
                                    sb.AppendLine(row.Cells[1].Text + ", " + row.Cells[2].Text + ", " + row.Cells[3].Text + ", " + row.Cells[4].Text + ", " +
                                        row.Cells[5].Text + ", " + row.Cells[6].Text + ", " + row.Cells[7].Text);
                                    break;
                                }
                                else
                                {
                                    dt_SelectedItems.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text, row.Cells[6].Text,
                                        row.Cells[7].Text, ((HiddenField)row.FindControl("hdn_ProcessId")).Value, ((HiddenField)row.FindControl("hdn_FinishedWt")).Value,
                                        ((HiddenField)row.FindControl("hdn_Discount")).Value, ((HiddenField)row.FindControl("hdn_PartNo")).Value, "False", "0", "0.00", "0.00",
                                        ((HiddenField)row.FindControl("hdn_Unitprice")).Value, "0.00", "0.00", "");
                                    break;
                                }
                            }
                        }
                        else
                            dt_SelectedItems.Rows.Add(row.Cells[1].Text, row.Cells[2].Text, row.Cells[3].Text, row.Cells[4].Text, row.Cells[5].Text, row.Cells[6].Text,
                                row.Cells[7].Text, ((HiddenField)row.FindControl("hdn_ProcessId")).Value, ((HiddenField)row.FindControl("hdn_FinishedWt")).Value,
                                ((HiddenField)row.FindControl("hdn_Discount")).Value, ((HiddenField)row.FindControl("hdn_PartNo")).Value, "False", "0", "0.00", "0.00",
                                ((HiddenField)row.FindControl("hdn_Unitprice")).Value, "0.00", "0.00", "");
                    }
                    foreach (DataRow dr in dt_SelectedItems.Rows)
                    {
                        if (dr["ProcessID"].ToString() == ((HiddenField)row.FindControl("hdn_ProcessId")).Value)
                            ((CheckBox)row.FindControl("chk_jathagam")).Checked = true;
                    }
                }
                if (dt_SelectedItems.Rows.Count > 0)
                {
                    ViewState["dt_SelectedItems"] = dt_SelectedItems;
                    Build_gv_SelectedItems(dt_SelectedItems);
                }
                if (sb.ToString().Length > 0)
                    lbl_ErrMsg1.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_gv_SelectedItems(DataTable dt)
        {
            try
            {
                gv_SelectedItems.DataSource = dt;
                gv_SelectedItems.DataBind();
                if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                {
                    gv_SelectedItems.Columns[12].Visible = false;
                    gv_SelectedItems.Columns[17].Visible = false;
                }
                else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                {
                    gv_SelectedItems.Columns[9].Visible = false;
                    gv_SelectedItems.Columns[10].Visible = false;
                    gv_SelectedItems.Columns[11].Visible = false;
                }
                if (gv_SelectedItems.Rows.Count > 0)
                    ScriptManager.RegisterStartupScript(Page, GetType(), "DisplayItems", "gotoPreviewDiv('div_SelectedItems');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gv_SelectedItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gv_SelectedItems.Rows[e.RowIndex];
                HiddenField hdn_ProcessID_S = (HiddenField)row.FindControl("hdn_ProcessId");
                foreach (GridViewRow row1 in gv_Jathagam.Rows)
                {
                    CheckBox chk_D = (CheckBox)row1.FindControl("chk_jathagam");
                    if (chk_D.Checked)
                    {
                        HiddenField hdn_ProcessID_D = (HiddenField)row1.FindControl("hdn_ProcessId");
                        if (hdn_ProcessID_D.Value == hdn_ProcessID_S.Value)
                        {
                            chk_D.Checked = false;
                            break;
                        }
                    }
                }

                CheckBox chk_RimAssembly = (CheckBox)row.FindControl("chk_RimAssembly");
                HiddenField hdnEdcNo = (HiddenField)row.FindControl("hdnEdcNo");
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString())), 
                    new SqlParameter("@ProcessId", hdn_ProcessID_S.Value),
                    new SqlParameter("@AssyRimStatus", chk_RimAssembly != null? chk_RimAssembly.Checked:false),
                    new SqlParameter("@EdcNo", hdnEdcNo.Value)
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_del_vs2_Orderitemlist", sp);

                DataTable dt = (DataTable)ViewState["dt_SelectedItems"];
                dt.Rows.RemoveAt(e.RowIndex);
                ViewState["dt_SelectedItems"] = dt;
                Build_gv_SelectedItems(dt);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_SaveItems_Click(object sender, EventArgs e)
        {
            try
            {
                // btn to save items
                lbl_ErrMsg1.Text = "";
                int gvRowCount = gv_SelectedItems.Rows.Count;
                foreach (GridViewRow row in gv_SelectedItems.Rows)
                {
                    string str_Discount = "0.00";
                    string str_SheetPrice = "0.00";
                    string str_BasicPrice = "0.00";
                    if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                    {
                        str_Discount = ((TextBox)row.FindControl("txt_Discount")).Text;
                        str_SheetPrice = ((TextBox)row.FindControl("txt_SheetPrice")).Text;
                        str_BasicPrice = (Convert.ToDecimal(str_Discount) > 0 ? Math.Round((Convert.ToDecimal(str_SheetPrice) -
                            (Convert.ToDecimal(str_SheetPrice) * (Convert.ToDecimal(((TextBox)row.FindControl("txt_Discount")).Text) / 100)))) :
                            Convert.ToDecimal(str_SheetPrice)).ToString();
                    }
                    else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                    {
                        str_SheetPrice = ((TextBox)row.FindControl("txt_ExpBasicPrice")).Text;
                        str_BasicPrice = ((TextBox)row.FindControl("txt_ExpBasicPrice")).Text;
                    }

                    string str_ItemQty = ((TextBox)row.FindControl("txt_ItemQty")).Text;
                    string str_RimBasicPrice = ((TextBox)row.FindControl("txt_Rim_BasicPrice")).Text;
                    string str_RimFinishedWt = ((TextBox)row.FindControl("txt_Rim_FinishedWt")).Text;
                    string str_DrawingNo = ((TextBox)row.FindControl("txt_Rim_DrawingNo")).Text;
                    bool rimAssmly = ((CheckBox)row.FindControl("chk_RimAssembly")).Checked ? true : false;
                    string str_AddInfo = ((TextBox)row.FindControl("txtAddInfo")).Text;
                    DropDownList ddl_Rim_EdcNo = ((DropDownList)row.FindControl("ddl_Rim_EdcNo"));
                    string strEdcNo = ddl_Rim_EdcNo.SelectedItem != null ? ddl_Rim_EdcNo.SelectedItem.Text : "";

                    SqlParameter[] sp = new SqlParameter[23];
                    sp[0] = new SqlParameter("@OID", Utilities.Decrypt(Request["oid"].ToString()));
                    sp[1] = new SqlParameter("@Category", ((Label)row.FindControl("lbl_Category")).Text);
                    sp[2] = new SqlParameter("@Config", row.Cells[1].Text);
                    sp[3] = new SqlParameter("@Brand", row.Cells[2].Text);
                    sp[4] = new SqlParameter("@Sidewall", row.Cells[3].Text);
                    sp[5] = new SqlParameter("@TyreType", row.Cells[4].Text);
                    sp[6] = new SqlParameter("@TyreSize", row.Cells[5].Text);
                    sp[7] = new SqlParameter("@RimSize", row.Cells[6].Text);
                    sp[8] = new SqlParameter("@ItemQty", str_ItemQty);
                    sp[9] = new SqlParameter("@UnitPrice", Convert.ToDecimal(str_BasicPrice != "" ? str_BasicPrice : "0.00"));
                    sp[10] = new SqlParameter("@FinishedWt", Convert.ToDecimal(row.Cells[7].Text != "" ? row.Cells[7].Text : "0.00"));
                    sp[11] = new SqlParameter("@Discount", Convert.ToDecimal(str_Discount != "" ? str_Discount : "0.00"));
                    sp[12] = new SqlParameter("@SheetPrice", Convert.ToDecimal(str_SheetPrice != "" ? str_SheetPrice : "0.00"));
                    sp[13] = new SqlParameter("@RimUnitPrce", Convert.ToDecimal(rimAssmly && str_RimBasicPrice != "" ? str_RimBasicPrice : "0.00"));
                    sp[14] = new SqlParameter("@RimItemQty", rimAssmly == true ? str_ItemQty : "0");
                    sp[15] = new SqlParameter("@RimFinishedWt", Convert.ToDecimal(rimAssmly && str_RimFinishedWt != "" ? str_RimFinishedWt : "0.00"));
                    sp[16] = new SqlParameter("@AssyRimStatus", rimAssmly);
                    sp[17] = new SqlParameter("@RimDwg", rimAssmly && str_DrawingNo != "" ? str_DrawingNo : "");
                    sp[18] = new SqlParameter("@ProcessId", ((HiddenField)row.FindControl("hdn_ProcessId")).Value);
                    sp[19] = new SqlParameter("@Grade", "A");
                    sp[20] = new SqlParameter("@ItemCode", ((HiddenField)row.FindControl("hdn_PartNo")).Value);
                    sp[21] = new SqlParameter("@AdditionalInfo", str_AddInfo);
                    sp[22] = new SqlParameter("@EdcNo", rimAssmly && strEdcNo != "" ? strEdcNo : "");
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_vs2_OrderItemList", sp);
                }
                btn_SkipItems_Click(null, null);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btn_SkipItems_Click(object sender, EventArgs e)
        {
            if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                Response.Redirect("cotsmanualorderprepare1.aspx?oid=" + Request["oid"].ToString(), false);
            else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                Response.Redirect("exportmanual2.aspx?oid=" + Request["oid"].ToString(), false);
        }
        protected void gvSelectedItems_DataBound(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gv_SelectedItems.Rows)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@TyreSize", row.Cells[5].Text), new SqlParameter("@Rimsize", row.Cells[6].Text) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_EDCNO_Order_TyresizeWise_v1", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        DropDownList ddl_Rim_EdcNo = (DropDownList)row.FindControl("ddl_Rim_EdcNo");
                        ddl_Rim_EdcNo.DataSource = dt;
                        ddl_Rim_EdcNo.DataTextField = "EDCNO";
                        ddl_Rim_EdcNo.DataValueField = "RimWt";
                        ddl_Rim_EdcNo.DataBind();

                        ddl_Rim_EdcNo.SelectedIndex = ddl_Rim_EdcNo.Items.IndexOf(ddl_Rim_EdcNo.Items.FindByText(((HiddenField)row.FindControl("hdnEdcNo")).Value));
                        ((TextBox)row.FindControl("txt_Rim_FinishedWt")).Text = ddl_Rim_EdcNo.SelectedItem.Value;
                    }
                    else
                        ((CheckBox)row.FindControl("chk_RimAssembly")).Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}