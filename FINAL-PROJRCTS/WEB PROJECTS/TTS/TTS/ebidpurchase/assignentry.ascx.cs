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

namespace TTS.ebidpurchase
{
    public partial class assignentry : System.Web.UI.UserControl
    {
        DataAccess daEBID = new DataAccess(ConfigurationManager.ConnectionStrings["eBidDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (Request["vid"].ToString() == "3")
                        ScriptManager.RegisterStartupScript(Page, GetType(), "btnTotShow1", "setTitle('available suppliers of product');", true);
                    else if (Request["vid"].ToString() == "4")
                        ScriptManager.RegisterStartupScript(Page, GetType(), "btnTotShow", "setTitle('available products of supplier');", true);
                    if (!IsPostBack)
                    {
                        if (Request["vid"].ToString() == "3")
                        {
                            divAssignSupplier.Style.Add("display", "block");
                            divAssignProduct.Style.Add("display", "none");
                            DataTable dtProduct = (DataTable)daEBID.ExecuteReader_SP("sp_sel_ProductList", DataAccess.Return_Type.DataTable);
                            if (dtProduct.Rows.Count > 0)
                            {
                                gvPurchaseProductList.DataSource = dtProduct;
                                gvPurchaseProductList.DataBind();

                                DataTable dtSupplier = (DataTable)daEBID.ExecuteReader_SP("sp_sel_SupplierList", DataAccess.Return_Type.DataTable);
                                if (dtSupplier.Rows.Count > 0)
                                {
                                    gvPurchaseAssignSupplierList.DataSource = dtSupplier;
                                    gvPurchaseAssignSupplierList.DataBind();
                                }
                            }
                        }
                        else if (Request["vid"].ToString() == "4")
                        {
                            divAssignSupplier.Style.Add("display", "none");
                            divAssignProduct.Style.Add("display", "block");
                            DataTable dtSupplier = (DataTable)daEBID.ExecuteReader_SP("sp_sel_SupplierList", DataAccess.Return_Type.DataTable);
                            if (dtSupplier.Rows.Count > 0)
                            {
                                gvPurchaseSupplierList.DataSource = dtSupplier;
                                gvPurchaseSupplierList.DataBind();

                                DataTable dtProduct = (DataTable)daEBID.ExecuteReader_SP("sp_sel_ProductList", DataAccess.Return_Type.DataTable);
                                if (dtProduct.Rows.Count > 0)
                                {
                                    gvPurchaseAssignProductList.DataSource = dtProduct;
                                    gvPurchaseAssignProductList.DataBind();
                                }
                            }
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

        protected void rdbProductID_IndexChange(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((RadioButton)sender).NamingContainer as GridViewRow;
                hdnProductId.Value = ((HiddenField)clickedRow.FindControl("hdnProductID")).Value;

                foreach (GridViewRow row1 in gvPurchaseAssignSupplierList.Rows)
                {
                    CheckBox chkSupplierAssign = row1.FindControl("chkSupplierAssign") as CheckBox;
                    chkSupplierAssign.Checked = false;
                    chkSupplierAssign.Enabled = true;
                }
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@AssignID", hdnProductId.Value);
                sp1[1] = new SqlParameter("@Method", "SUPPLIER");
                DataTable dt = (DataTable)daEBID.ExecuteReader_SP("sp_sel_AssignList", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    foreach (GridViewRow row1 in gvPurchaseAssignSupplierList.Rows)
                    {
                        CheckBox chkSupplierAssign = row1.FindControl("chkSupplierAssign") as CheckBox;
                        HiddenField hdnSupplierAssign = row1.FindControl("hdnSupplierAssign") as HiddenField;
                        chkSupplierAssign.Checked = false;
                        DataRow[] dtRow = dt.Select("SupID=" + hdnSupplierAssign.Value + "");
                        if (dtRow.Length > 0)
                        {
                            chkSupplierAssign.Checked = true;
                            chkSupplierAssign.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnAssignSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DataColumn c1 = new DataColumn("ProdID", typeof(System.Int32));
                dt.Columns.Add(c1);
                c1 = new DataColumn("SupID", typeof(System.Int32));
                dt.Columns.Add(c1);
                foreach (GridViewRow row1 in gvPurchaseAssignSupplierList.Rows)
                {
                    CheckBox chk1 = row1.FindControl("chkSupplierAssign") as CheckBox;
                    if (chk1.Enabled && chk1.Checked)
                    {
                        HiddenField hdnSupplierAssign = row1.FindControl("hdnSupplierAssign") as HiddenField;
                        dt.Rows.Add(Convert.ToInt32(hdnProductId.Value), Convert.ToInt32(hdnSupplierAssign.Value));
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[2];
                    sp1[0] = new SqlParameter("@DtAssignProductSupplier", dt);
                    sp1[1] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    daEBID.ExecuteNonQuery_SP("sp_ins_Assign_Product_Supplier", sp1);
                }
                Response.Redirect("ebid_purchase.aspx?vid=3", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void rdbSupplierID_IndexChange(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((RadioButton)sender).NamingContainer as GridViewRow;
                hdnSupplierId.Value = ((HiddenField)clickedRow.FindControl("hdnSupplierID")).Value;

                foreach (GridViewRow row1 in gvPurchaseAssignProductList.Rows)
                {
                    CheckBox chkProductAssign = row1.FindControl("chkProductAssign") as CheckBox;
                    chkProductAssign.Checked = false;
                    chkProductAssign.Enabled = true;
                }
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@AssignID", hdnSupplierId.Value);
                sp1[1] = new SqlParameter("@Method", "PRODUCT");
                DataTable dt = (DataTable)daEBID.ExecuteReader_SP("sp_sel_AssignList", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    foreach (GridViewRow row1 in gvPurchaseAssignProductList.Rows)
                    {
                        CheckBox chkProductAssign = row1.FindControl("chkProductAssign") as CheckBox;
                        HiddenField hdnProductAssign = row1.FindControl("hdnProductAssign") as HiddenField;
                        chkProductAssign.Checked = false;
                        DataRow[] dtRow = dt.Select("ProdID=" + hdnProductAssign.Value + "");
                        if (dtRow.Length > 0)
                        {
                            chkProductAssign.Checked = true;
                            chkProductAssign.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnAssignProduct_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                DataColumn c1 = new DataColumn("ProdID", typeof(System.Int32));
                dt.Columns.Add(c1);
                c1 = new DataColumn("SupID", typeof(System.Int32));
                dt.Columns.Add(c1);
                foreach (GridViewRow row1 in gvPurchaseAssignProductList.Rows)
                {
                    CheckBox chk1 = row1.FindControl("chkProductAssign") as CheckBox;
                    if (chk1.Enabled && chk1.Checked)
                    {
                        HiddenField hdnProductAssign = row1.FindControl("hdnProductAssign") as HiddenField;
                        dt.Rows.Add(Convert.ToInt32(hdnProductAssign.Value), Convert.ToInt32(hdnSupplierId.Value));
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[2];
                    sp1[0] = new SqlParameter("@DtAssignProductSupplier", dt);
                    sp1[1] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    daEBID.ExecuteNonQuery_SP("sp_ins_Assign_Product_Supplier", sp1);
                }
                Response.Redirect("ebid_purchase.aspx?vid=4", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}