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

namespace COTS
{
    public partial class frmexporder : System.Web.UI.Page
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
                        if (Request["qno"] != null && Utilities.Decrypt(Request["qno"].ToString()) != "")
                        {
                            lblOrderNo.Text = Utilities.Decrypt(Request["qno"].ToString());

                            chkRim.Visible = false;
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@custstdcode", Session["cotsstdcode"].ToString()) };
                            DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_processid_custwise", sp, DataAccess.Return_Type.DataTable);
                            if (dtList.Rows.Count > 0)
                            {
                                ddlProcessID.DataSource = dtList;
                                ddlProcessID.DataTextField = "ProcessID";
                                ddlProcessID.DataValueField = "ProcessID";
                                ddlProcessID.DataBind();
                                ddlProcessID.Items.Insert(0, "CHOOSE");
                            }
                            Bind_OrderItem();
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
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlProcessID_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblErrmsg.Text = "";
                chkRim.Visible = false;
                chkRim.Checked = false;
                chkRim_CheckedChange(sender, e);
                frmProcessIDDetails.DataSource = null;
                frmProcessIDDetails.DataBind();
                if (ddlProcessID.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@custstdcode", Session["cotsstdcode"].ToString()), 
                        new SqlParameter("@processid", ddlProcessID.SelectedItem.Text) 
                    };
                    DataTable dtDetails = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_processid_details", sp, DataAccess.Return_Type.DataTable);
                    if (dtDetails.Rows.Count > 0)
                    {
                        frmProcessIDDetails.DataSource = dtDetails;
                        frmProcessIDDetails.DataBind();

                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@TyreSize", dtDetails.Rows[0]["TyreSize"].ToString()),
                            new SqlParameter("@Rimsize", dtDetails.Rows[0]["RimSize"].ToString())
                        };
                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("Sp_sel_EDCNO_Order_TyresizeWise", sp1, DataAccess.Return_Type.DataTable);
                        if (dt.Rows.Count > 0)
                        {
                            ddl_EdcNo.DataSource = dt;
                            ddl_EdcNo.DataTextField = "EDCNO";
                            ddl_EdcNo.DataValueField = "EDCNO";
                            ddl_EdcNo.DataBind();
                            ddl_EdcNo.Items.Insert(0, "CHOOSE");

                            chkRim.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void chkRim_CheckedChange(object sender, EventArgs e)
        {
            try
            {
                tbEdcDetails.Style.Add("display", "none");
                if (chkRim.Checked)
                    tbEdcDetails.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlEdcNo_IndexChange(object sender, EventArgs e)
        {
            try
            {
                lblErrmsg.Text = "";
                if (ddl_EdcNo.SelectedItem.Text != "CHOOSE")
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@custstdcode", Session["cotsstdcode"].ToString()), 
                        new SqlParameter("@EdcNo", ddl_EdcNo.SelectedItem.Text) 
                    };
                    DataTable dtRim = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_RimPrice", sp, DataAccess.Return_Type.DataTable);
                    if (dtRim.Rows.Count > 0)
                    {
                        lblRimWt.Text = dtRim.Rows[0]["RimWeight"].ToString();
                        lblRimPrice.Text = dtRim.Rows[0]["RimPrice"].ToString();
                    }
                    else
                    {
                        lblErrmsg.Text = "PRICE SHEET NOT PUBLISHED TO EDC " + ddl_EdcNo.SelectedItem.Text + ". KINDLY CONTACT SUN-TWS";
                        ddl_EdcNo.SelectedIndex = -1;
                    }
                    chkRim_CheckedChange(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@custcode", Session["cotscode"].ToString()), 
                    new SqlParameter("@OrderId", lblOrderNo.Text), 
                    new SqlParameter("@ItemQty", txtPrepareQty.Text), 
                    new SqlParameter("@RimUnitPrce",  chkRim.Checked && lblRimPrice.Text !=""?lblRimPrice.Text:"0.00"), 
                    new SqlParameter("@RimItemQty", chkRim.Checked ? txtPrepareQty.Text :"0"), 
                    new SqlParameter("@RimFinishedWt", chkRim.Checked && lblRimWt.Text !=""?lblRimWt.Text:"0.00"), 
                    new SqlParameter("@AssyRimStatus", chkRim.Checked?true:false), 
                    new SqlParameter("@RimDwg", txt_RimDesc.Text), 
                    new SqlParameter("@ProcessId", ddlProcessID.SelectedItem.Text), 
                    new SqlParameter("@EdcNo", ddl_EdcNo.SelectedItem.Text), 
                    new SqlParameter("@custstdcode", Session["cotsstdcode"].ToString()) 
                };
                int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_exp_prepareitems", sp);
                Bind_OrderItem();

                frmProcessIDDetails.DataSource = null;
                frmProcessIDDetails.DataBind();
                ddlProcessID.SelectedIndex = -1;
                txtPrepareQty.Text = "";
                chkRim.Checked = false;
                chkRim.Visible = false;
                ddl_EdcNo.SelectedIndex = -1;
                txt_RimDesc.Text = "";
                lblRimPrice.Text = "";
                lblRimWt.Text = "";
                lblErrmsg.Text = "";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderItem()
        {
            try
            {
                btnCompleted.Style.Add("display", "none");
                gvPrepareItems.DataSource = null;
                gvPrepareItems.DataBind();

                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp1[1] = new SqlParameter("@orderrefno", lblOrderNo.Text);
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_cust_proforma_orderitemlist", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvPrepareItems.DataSource = dtItemList;
                    gvPrepareItems.DataBind();

                    gvPrepareItems.FooterRow.Cells[5].Text = "TOTAL";
                    gvPrepareItems.FooterRow.Cells[6].Text = dtItemList.Compute("Sum(itemqty)", "").ToString();
                    gvPrepareItems.FooterRow.Cells[10].Text = Session["cotsstdcode"].ToString() == "DE0048" ?
                        Math.Round(Convert.ToDecimal(dtItemList.Compute("Sum(totprice)", ""))).ToString() : dtItemList.Compute("Sum(totprice)", "").ToString();
                    gvPrepareItems.FooterRow.Cells[11].Text = dtItemList.Compute("Sum(totwt)", "").ToString();

                    btnCompleted.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvPrepareItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gvPrepareItems.Rows[e.RowIndex];
                Label lblProcessID = row.FindControl("lblProcessID") as Label;
                if (lblProcessID.Text != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ItemID", ((HiddenField)row.FindControl("hdnOItem")).Value) };
                    daCOTS.ExecuteNonQuery_SP("sp_del_orderitemlist", sp1);
                    Bind_OrderItem();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnCompleted_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("expordermaster.aspx?eno=" + Request["qno"].ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}