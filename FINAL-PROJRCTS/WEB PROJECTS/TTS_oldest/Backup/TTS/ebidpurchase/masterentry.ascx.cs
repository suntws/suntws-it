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
    public partial class masterentry : System.Web.UI.UserControl
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daEBID = new DataAccess(ConfigurationManager.ConnectionStrings["eBidDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        if (Request["vid"].ToString() == "1")
                        {
                            divProduct.Style.Add("display", "block");
                            divSupplier.Style.Add("display", "none");

                            DataTable dtProduct = (DataTable)daEBID.ExecuteReader_SP("sp_sel_ProductList", DataAccess.Return_Type.DataTable);
                            if (dtProduct.Rows.Count > 0)
                            {
                                gvPurchaseProductList.DataSource = dtProduct;
                                gvPurchaseProductList.DataBind();
                            }
                            ScriptManager.RegisterStartupScript(Page, GetType(), "btnTotShow", "setTitle('purchase product master');", true);
                        }
                        else if (Request["vid"].ToString() == "2")
                        {
                            divProduct.Style.Add("display", "none");
                            divSupplier.Style.Add("display", "block");

                            DataTable dtSupplier = (DataTable)daEBID.ExecuteReader_SP("sp_sel_SupplierList", DataAccess.Return_Type.DataTable);
                            if (dtSupplier.Rows.Count > 0)
                            {
                                gvPurchaseSupplierList.DataSource = dtSupplier;
                                gvPurchaseSupplierList.DataBind();
                            }
                            ScriptManager.RegisterStartupScript(Page, GetType(), "btnTotShow", "setTitle('supplier master');", true);
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

        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@ProdCategory", txtCategory.Text);
                sp1[1] = new SqlParameter("@ProdDesc", txtProductDesc.Text);
                sp1[2] = new SqlParameter("@ProdMeasurement", ddlMeasurement.SelectedItem.Text);
                sp1[3] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                int resp = daEBID.ExecuteNonQuery_SP("sp_ins_ProductList", sp1);
                if (resp > 0)
                    Response.Redirect("ebid_purchase.aspx?vid=1", false);
                else if (resp == -1)
                    lblErrMsg.Text = "Already product exists";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSaveSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[]{
                    new SqlParameter("@SupplierName", txtSupplier.Text),
                    new SqlParameter("@SuppCountry", txtCountry.Text),
                    new SqlParameter("@SuppCity", txtCity.Text),
                    new SqlParameter("@SuppAddress", txtSupplierAddress.Text),
                    new SqlParameter("@SuppContactPerson", txtContactPerson.Text),
                    new SqlParameter("@SuppContactNo", txtContactNo.Text),
                    new SqlParameter("@SuppEmailID", txtEmailID.Text),
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@SuppAltContactNo", txtContactNo.Text),
                    new SqlParameter("@SuppAltEmailID", txtEmailID.Text),
                    new SqlParameter("@PaymentTerms", txtPaymentTerms.Text),
                    new SqlParameter("@Category", ddlCategory.Text)
                };
                int resp = daEBID.ExecuteNonQuery_SP("sp_ins_SupplierList", sp1);
                if (resp > 0)
                    Response.Redirect("ebid_purchase.aspx?vid=2", false);
                else if (resp == -1)
                    lblErrMsg.Text = "Already supplier details exists";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}