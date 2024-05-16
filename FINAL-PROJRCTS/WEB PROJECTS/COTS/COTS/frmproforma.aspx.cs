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

namespace COTS
{
    public partial class frmproforma : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["cotsuser"] != null || Session["cotsuser"].ToString() != "")
                {
                    if (!IsPostBack)
                    {
                        if (Request["qcomplete"] != null && Utilities.Decrypt(Request["qcomplete"].ToString()) != "")
                        {
                            lblOrderNo.Text = Utilities.Decrypt(Request["qcomplete"].ToString());
                            Bind_OrderItem(lblOrderNo.Text);
                            if (Request["qno"] != null && Request["qyr"] != null)
                                btnSendToTvs_Click(sender, e);
                            else
                            {
                                if (Session["cotsstdcode"].ToString() == "DE0048")
                                {
                                    btnSendToTvs.Style.Add("display", "block");
                                    btnPrepareMaster.Style.Add("display", "none");
                                }
                                else
                                {
                                    btnSendToTvs.Style.Add("display", "none");
                                    btnPrepareMaster.Style.Add("display", "block");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("SessionExp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderItem(string strorderno)
        {
            try
            {
                btnSendToTvs.Visible = false;
                gvItemList.DataSource = null;
                gvItemList.DataBind();

                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@orderrefno", strorderno);
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cust_proforma_orderitemlist", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvItemList.DataSource = dtItemList;
                    gvItemList.DataBind();
                    btnSendToTvs.Visible = true;

                    gvItemList.FooterRow.Cells[5].Text = "TOTAL";
                    gvItemList.FooterRow.Cells[6].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gvItemList.FooterRow.Cells[10].Text = Session["cotsstdcode"].ToString() == "DE0048" ?
                        Math.Round(Convert.ToDecimal(dtItemList.Compute("Sum(totprice)", ""))).ToString() : dtItemList.Compute("Sum(totprice)", "").ToString();
                    gvItemList.FooterRow.Cells[11].Text = dtItemList.Compute("Sum(totwt)", "").ToString();

                    ViewState["dtItemList"] = dtItemList;
                }
                else
                    lblErrMsg.Text = "No records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvItemList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gvItemList.Rows[e.RowIndex];
                Label lblProcessID = row.FindControl("lblProcessID") as Label;
                if (lblProcessID.Text != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ItemID", ((HiddenField)row.FindControl("hdnO_ItemID")).Value) };
                    daCOTS.ExecuteNonQuery_SP("sp_del_orderitemlist", sp1);
                    Bind_OrderItem(lblOrderNo.Text);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvItemList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvItemList.EditIndex = e.NewEditIndex;
                Bind_OrderItem(lblOrderNo.Text);

                GridViewRow row = gvItemList.Rows[e.NewEditIndex];
                CheckBox chkRimAssy = (CheckBox)row.FindControl("chkRimAssy");
                DropDownList ddl_EdcNo = (DropDownList)row.FindControl("ddl_EdcNo");
                TextBox txt_RimWt = row.FindControl("txt_RimWt") as TextBox;
                TextBox txt_RimPrice = row.FindControl("txt_RimPrice") as TextBox;
                TextBox txt_RimDesc = row.FindControl("txt_RimDesc") as TextBox;

                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@TyreSize", ((Label)row.FindControl("lblTyreSize")).Text),
                    new SqlParameter("@Rimsize", ((Label)row.FindControl("lblRimSize")).Text)
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_EDCNO_Order_TyresizeWise", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddl_EdcNo.DataSource = dt;
                    ddl_EdcNo.DataTextField = "EDCNO";
                    ddl_EdcNo.DataValueField = "EDCNO";
                    ddl_EdcNo.DataBind();
                    ddl_EdcNo.Items.Insert(0, "CHOOSE");

                    chkRimAssy.Visible = true;
                    DataTable dtItem = ViewState["dtItemList"] as DataTable;
                    foreach (DataRow pRow in dtItem.Select("processid='" + ((Label)row.FindControl("lblProcessID")).Text + "'"))
                    {
                        ddl_EdcNo.SelectedIndex = ddl_EdcNo.Items.IndexOf(ddl_EdcNo.Items.FindByValue(pRow["EdcNo"].ToString()));
                        chkRimAssy.Checked = Convert.ToBoolean(pRow["AssyRimstatus"].ToString());
                        txt_RimWt.Text = pRow["Rimfinishedwt"].ToString();
                        txt_RimPrice.Text = pRow["Rimunitprice"].ToString();
                        txt_RimDesc.Text = pRow["RimDwg"].ToString();
                    }
                }
                else
                    chkRimAssy.Visible = false;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvItemList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvItemList.Rows[e.RowIndex];
                Label lblProcessID = row.FindControl("lblProcessID") as Label;
                TextBox txtQty = row.FindControl("txtQty") as TextBox;
                CheckBox chkRimAssy = row.FindControl("chkRimAssy") as CheckBox;
                DropDownList ddl_EdcNo = row.FindControl("ddl_EdcNo") as DropDownList;
                TextBox txt_RimWt = row.FindControl("txt_RimWt") as TextBox;
                TextBox txt_RimPrice = row.FindControl("txt_RimPrice") as TextBox;
                TextBox txt_RimDesc = row.FindControl("txt_RimDesc") as TextBox;

                if (lblProcessID.Text != "" && txtQty.Text != "" && Convert.ToInt32(txtQty.Text) > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[10];
                    sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                    sp1[1] = new SqlParameter("@orderid", lblOrderNo.Text);
                    sp1[2] = new SqlParameter("@processid", lblProcessID.Text);
                    sp1[3] = new SqlParameter("@itemqty", Convert.ToInt32(txtQty.Text));
                    sp1[4] = new SqlParameter("@AssyRimstatus", chkRimAssy != null && chkRimAssy.Checked ? true : false);
                    sp1[5] = new SqlParameter("@Rimitemqty", chkRimAssy != null && chkRimAssy.Checked ? txtQty.Text : "0");
                    sp1[6] = new SqlParameter("@Rimunitprice", chkRimAssy != null && chkRimAssy.Checked ? txt_RimPrice.Text : "0");
                    sp1[7] = new SqlParameter("@Rimfinishedwt", chkRimAssy != null && chkRimAssy.Checked ? txt_RimWt.Text : "0");
                    sp1[8] = new SqlParameter("@RimDwg", chkRimAssy != null && chkRimAssy.Checked ? txt_RimDesc.Text : "");
                    sp1[9] = new SqlParameter("@EdcNo", chkRimAssy != null && chkRimAssy.Checked ? ddl_EdcNo.SelectedItem.Text : "");
                    daCOTS.ExecuteNonQuery_SP("sp_edit_OrderItemList_qty", sp1);
                }
                gvItemList.EditIndex = -1;
                Bind_OrderItem(lblOrderNo.Text);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvItemList_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gvItemList.EditIndex = -1;
                Bind_OrderItem(lblOrderNo.Text);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSendToTvs_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@orderid", lblOrderNo.Text);
                int resp = daCOTS.ExecuteNonQuery_SP("sp_edit_Change_completed_status", sp1);

                if (resp > 0)
                {
                    StringBuilder mailConcat = new StringBuilder();
                    SqlParameter[] sp4 = new SqlParameter[2];
                    sp4[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                    sp4[1] = new SqlParameter("@orderrefno", lblOrderNo.Text);
                    DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cust_proforma_orderitemlist", sp4, DataAccess.Return_Type.DataTable);
                    if (dtItemList.Rows.Count > 0)
                    {
                        string maketable = string.Empty;
                        foreach (DataRow row in dtItemList.Rows)
                        {
                            maketable += "<tr><td style='float:left;width:80px;'>" + row["tyretype"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:100px;'>" + row["brand"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:100px;'>" + row["sidewall"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:180px;'>" + row["tyresize"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:60px;'>" + row["rimsize"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:100px;'>" + row["unitprice"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:60px;'>" + row["itemqty"].ToString() + "</td>";
                            maketable += "<td style='float:left;width:120px;'>" + row["totprice"].ToString() + "</td></tr>";
                        }

                        if (maketable.Length > 0)
                        {
                            mailConcat.Append("ORDER RECEIVED FROM : " + Session["cotsuserfullname"].ToString() + "<br/>");
                            mailConcat.Append("ORDER REF NO. :" + lblOrderNo.Text);
                            mailConcat.Append("<table border='1' cellspacing='0' rules='all' style='width:834px;border-collapse:collapse;'>");
                            mailConcat.Append("<tr style='text-align:center;font-weight:bold;background-color: #60FC79;'><td style='float:left;width:80px;'>TYPE</td>");
                            mailConcat.Append("<td style='float:left;width:100px;'>BRAND</td><td style='float:left;width:100px;'>SIDEWALL</td>");
                            mailConcat.Append("<td style='float:left;width:180px;'>SIZE</td><td style='float:left;width:60px;'>RIM</td>");
                            mailConcat.Append("<td style='float:left;width:100px;'>UNIT PRICE</td><td style='float:left;width:60px;'>QTY</td>");
                            mailConcat.Append("<td style='float:left;width:120px;'>TOTAL PRICE</td></tr>");
                            mailConcat.Append(maketable + "</table>");
                            mailConcat.Append("ORDER TOTAL WT: " + dtItemList.Compute("Sum(totwt)", "").ToString() + "<br/><br/>");
                        }
                    }
                    if (mailConcat.ToString().Length > 0)
                    {
                        string strToMail = Utilities.Build_CC_MailList(Session["cotscode"].ToString(), "");
                        Utilities.CotsOrderMailSent(mailConcat.ToString(), "DOMESTIC ORDER RECEIVED", strToMail, Session["cotsmail"].ToString());
                    }
                    Response.Redirect("frmmsgdisplay.aspx?msgtype=ordercomplete&oid=" + lblOrderNo.Text, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnBackToEntry_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["cotsstdcode"].ToString() == "DE0048")
                    Response.Redirect("frmitementry.aspx?qno=" + Utilities.Encrypt(lblOrderNo.Text) + "&qtype=" + Utilities.Encrypt("old"), false);
                else
                {
                    if (Session["cotscode"].ToString() == "2642")
                        Response.Redirect("frmexporder.aspx?qno=" + Utilities.Encrypt(lblOrderNo.Text) + "&qtype=" + Utilities.Encrypt("old"), false);
                    else
                        Response.Redirect("frmexpitementry.aspx?qno=" + Utilities.Encrypt(lblOrderNo.Text) + "&qtype=" + Utilities.Encrypt("old"), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnPrepareMaster_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("expordermaster.aspx?eno=" + Utilities.Encrypt(lblOrderNo.Text), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}