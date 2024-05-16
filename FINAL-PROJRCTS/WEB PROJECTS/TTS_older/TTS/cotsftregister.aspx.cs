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
using System.Globalization;

namespace TTS
{
    public partial class cotsftregister : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dom_invoice"].ToString() == "True")
                        {
                            lblHeadPlant.Text = Request["pid"].ToString().ToUpper();
                            DataTable dtCustList = new DataTable();
                            dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsDomesticCustomer", DataAccess.Return_Type.DataTable);
                            if (dtCustList.Rows.Count > 0)
                            {
                                ddlCotsCustName.DataSource = dtCustList;
                                ddlCotsCustName.DataTextField = "custfullname";
                                ddlCotsCustName.DataValueField = "custcode";
                                ddlCotsCustName.DataBind();
                            }
                            ddlCotsCustName.Items.Insert(0, "Choose");
                            ddlCotsCustName.Text = "Choose";

                            DataTable dtSize = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Tyresize", DataAccess.Return_Type.DataTable);
                            if (dtSize.Rows.Count > 0)
                            {
                                ddlTyreSize.DataSource = dtSize;
                                ddlTyreSize.DataTextField = "TyreSize";
                                ddlTyreSize.DataValueField = "TyreSize";
                                ddlTyreSize.DataBind();
                            }
                            ddlTyreSize.Items.Insert(0, "Choose");
                            create_Datatable_ForFTList();
                            statusChangeDiv.Style.Add("display", "none");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                    if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCotsCustName")
                        ddlCotsCustName_IndexChange(sender, e);
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
        private void create_Datatable_ForFTList()
        {
            try
            {
                DataTable dtFtList = new DataTable();
                DataColumn col = new DataColumn("tyresize", typeof(System.String));
                dtFtList.Columns.Add(col);
                col = new DataColumn("itemqty", typeof(System.Int32));
                dtFtList.Columns.Add(col);
                ViewState["dtFtList"] = dtFtList;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnAddNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool rowExist = false;
                DataTable dtFtList = ViewState["dtFtList"] as DataTable;
                if (dtFtList != null)
                {
                    foreach (DataRow row in dtFtList.Select("tyresize='" + ddlTyreSize.SelectedItem.Text + "'"))
                    {
                        rowExist = true;
                    }
                }
                if (rowExist)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JEMsg2", "bind_errmsg('Item already added. You can change the qty or delete this item');", true);
                }
                else if (!rowExist)
                {
                    DataRow nRow = dtFtList.NewRow();
                    nRow["itemqty"] = txtQty.Text;
                    nRow["tyresize"] = ddlTyreSize.SelectedItem.Text;
                    dtFtList.Rows.Add(nRow);
                    Build_FtListItem(dtFtList);
                }
                txtQty.Text = "";
                ddlTyreSize.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_FtListItem(DataTable dtFtList)
        {
            gvFtList.DataSource = null;
            gvFtList.DataBind();
            lblTotQTy.Text = "";
            if (dtFtList.Rows.Count > 0)
            {
                gvFtList.DataSource = dtFtList;
                gvFtList.DataBind();
                ViewState["dtFtList"] = dtFtList;
                object sumQty;
                sumQty = dtFtList.Compute("Sum(itemqty)", "");
                hdntotalqty.Value = sumQty.ToString();
                lblTotQTy.Text = "<span class='headCss'>TOTAL QTY:</span>" + sumQty.ToString();

                ddlBillingAddress.Enabled = false;
                ddlCotsCustName.Enabled = false;
                ddlLoginUserName.Enabled = false;
                txtGatePassDate.Enabled = false;
                txtGatePassNo.Enabled = false;
                txtPan.Enabled = false;
                txtTin.Enabled = false;
                statusChangeDiv.Style.Add("display", "block");
            }
            else
            {
                statusChangeDiv.Style.Add("display", "none");
                ddlBillingAddress.Enabled = true;
                ddlCotsCustName.Enabled = true;
                ddlLoginUserName.Enabled = true;
                txtGatePassDate.Enabled = true;
                txtGatePassNo.Enabled = true;
                txtPan.Enabled = true;
                txtTin.Enabled = true;
            }
            Select_DropDownList();
        }
        protected void ddlCotsCustName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlCotsCustName")
                {
                    //hdnFullName.Value = ddlCotsCustName.SelectedItem.Text;
                    ddlLoginUserName.DataSource = "";
                    ddlLoginUserName.DataBind();
                    Select_DropDownList();
                    if (ddlCotsCustName.SelectedItem.Text != "Choose")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddlCotsCustName.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddlCotsCustName.SelectedItem.Value) 
                    };
                        DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                        if (dtUserList.Rows.Count > 0)
                        {
                            ddlLoginUserName.DataSource = dtUserList;
                            ddlLoginUserName.DataTextField = "username";
                            ddlLoginUserName.DataValueField = "ID";
                            ddlLoginUserName.DataBind();
                            if (dtUserList.Rows.Count == 1)
                                ddlLoginUserName_IndexChange(sender, e);
                            else
                                ddlLoginUserName.Items.Insert(0, "Choose");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlLoginUserName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                hdnCotsCustID.Value = ddlLoginUserName.SelectedItem.Value;
                Select_DropDownList();
                ddlBillingAddress.DataSource = "";
                ddlBillingAddress.DataBind();
                if (ddlLoginUserName.SelectedItem.Text != "Choose")
                {
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@custcode", ddlLoginUserName.SelectedItem.Value);
                    DataTable dtBillAddress = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_BillList", sp1, DataAccess.Return_Type.DataTable);
                    if (dtBillAddress.Rows.Count > 0)
                    {
                        ddlBillingAddress.DataSource = dtBillAddress;
                        ddlBillingAddress.DataTextField = "ShipAddress";
                        ddlBillingAddress.DataValueField = "shipid";
                        ddlBillingAddress.DataBind();
                        if (dtBillAddress.Rows.Count == 1)
                            ddlBillingAddress_IndexChange(sender, e);
                        else
                            ddlBillingAddress.Items.Insert(0, "Choose");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlBillingAddress_IndexChange(object sender, EventArgs e)
        {
            try
            {
                hdnBillID.Value = ddlBillingAddress.SelectedItem.Value;
                Select_DropDownList();
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@BillAddId", ddlBillingAddress.SelectedValue);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_FtMasterdetails_taxno", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    txtPan.Text = dt.Rows[0]["PanNo"].ToString();
                    txtTin.Text = dt.Rows[0]["ServiceTaxno"].ToString();
                }

                sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", hdnCotsCustID.Value);
                sp1[1] = new SqlParameter("@shipid", Convert.ToInt32(ddlBillingAddress.SelectedValue));
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_address_IDwise", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    StringBuilder strApp = new StringBuilder();
                    DataRow row = dt.Rows[0];
                    strApp.Append("<table>");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string tr = "<tr style='vertical-align: top;'><th style='width:85px;'>" + dt.Columns[i].ToString().ToUpper() + "</th><td style='font-weight:bold;'>:</td><td style='font-weight:bold;'>" + row[i].ToString().Replace("~", "<br/>") + "</td></tr>";
                        strApp.Append(tr);
                    }
                    strApp.Append("</table>");
                    lblBillingAddress.Text = strApp.ToString();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Select_DropDownList()
        {
            ListItem selectedListItem = ddlCotsCustName.Items.FindByText("" + hdnFullName.Value + "");
            if (selectedListItem != null)
            {
                ddlCotsCustName.Items.FindByText(ddlCotsCustName.SelectedItem.Text).Selected = false;
                selectedListItem.Selected = true;
            }
        }
        protected void gvFtList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtFtList"] as DataTable;
                gvFtList.EditIndex = e.NewEditIndex;
                Build_FtListItem(dtQuoteItem);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvFtList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtFtList"] as DataTable;
                GridViewRow row = gvFtList.Rows[e.RowIndex];
                Label lbltyresize = row.FindControl("lbltyresize") as Label;
                TextBox txtGItemQty = row.FindControl("txtGItemQty") as TextBox;
                if (lbltyresize.Text != "" && txtGItemQty.Text != "")
                {
                    if (dtQuoteItem.Rows.Count > 0)
                    {
                        foreach (DataRow iRow in dtQuoteItem.Select("tyresize='" + lbltyresize.Text + "'"))
                        {
                            iRow["itemqty"] = Convert.ToInt32(txtGItemQty.Text);
                        }
                    }
                }
                gvFtList.EditIndex = -1;
                Build_FtListItem(dtQuoteItem);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvFtList_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtFtList"] as DataTable;
                e.Cancel = true;
                gvFtList.EditIndex = -1;
                Build_FtListItem(dtQuoteItem);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvFtList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DataTable dtQuoteItem = ViewState["dtFtList"] as DataTable;
                GridViewRow row = gvFtList.Rows[e.RowIndex];
                Label lbltyresize = row.FindControl("lbltyresize") as Label;
                if (lbltyresize.Text != "")
                {
                    if (dtQuoteItem.Rows.Count > 0)
                    {
                        foreach (DataRow iRow in dtQuoteItem.Select("tyresize='" + lbltyresize.Text + "'"))
                            iRow.Delete();
                        dtQuoteItem.AcceptChanges();
                    }
                    Build_FtListItem(dtQuoteItem);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtFtList = ViewState["dtFtList"] as DataTable;
                if (dtFtList.Rows.Count > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[11];
                    sp1[0] = new SqlParameter("@CustCode", hdnCotsCustID.Value);
                    sp1[1] = new SqlParameter("@BillAddId", hdnBillID.Value);
                    sp1[2] = new SqlParameter("@GSTNo", txtTin.Text.Trim().ToUpper());
                    sp1[3] = new SqlParameter("@PanNo", txtPan.Text.Trim().ToUpper());
                    sp1[4] = new SqlParameter("@ChallanRefno", txtGatePassNo.Text.Trim().ToUpper());
                    sp1[5] = new SqlParameter("@ChallanDate", DateTime.ParseExact(txtGatePassDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    sp1[6] = new SqlParameter("@CreatedBy", Request.Cookies["TTSUser"].Value);
                    sp1[7] = new SqlParameter("@ReceivedQty", hdntotalqty.Value);
                    sp1[8] = new SqlParameter("@FtItemList_datatable", dtFtList);
                    sp1[9] = new SqlParameter("@Plant", lblHeadPlant.Text);
                    sp1[10] = new SqlParameter("@Comments", txtComments.Text.Replace("\r\n", "~"));
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_FtMasterDetails", sp1);
                    if (resp > 0)
                        Response.Redirect("cotsfttrack.aspx?pid=" + lblHeadPlant.Text.ToLower() + "&aid=rec", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}