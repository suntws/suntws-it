using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;

namespace TTS.cargomanagement
{
    public partial class tyredimensionmaster : System.Web.UI.UserControl
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                DataTable dtUser = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_UserLevel", sp1, DataAccess.Return_Type.DataTable);
                if (dtUser.Rows.Count > 0)
                {
                    if (dtUser.Rows[0]["cotsexpcargomanagement"].ToString() == "True")
                    {
                        DataTable dtCustList = new DataTable();
                        dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_lst_user_export_customers", DataAccess.Return_Type.DataTable);
                        Utilities.ddl_Binding(ddlCustomer, dtCustList, "custfullname", "CustCode", "CHOOSE");
                        if (ddlCustomer.Items.Count == 2)
                        {
                            ddlCustomer.SelectedIndex = 1;
                            ddlCustomer_SelectedIndexChanged(sender, e);
                        }
                    }
                }
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCustomer.SelectedIndex <= 0) return;
                ddlOrder.Items.Clear();
                SqlParameter[] sp_param1 = new SqlParameter[] { new SqlParameter("@custcode", ddlCustomer.SelectedValue) };
                DataTable dtOrders = (DataTable)daCOTS.ExecuteReader_SP("sp_lst_customer_orders", sp_param1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlOrder, dtOrders, "OrderRefNo", "OrderRefNo", "Choose");
                if (ddlOrder.Items.Count == 2)
                {
                    ddlOrder.SelectedIndex = 1;
                    ddlOrder_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                lblErrMsg.Text = ex.Message.ToString();
            }
        }

        protected void ddlOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOrder.SelectedIndex <= 0) return;
            GetCustomerOrders();

            SqlParameter[] sp1 = new SqlParameter[2];
            sp1[0] = new SqlParameter("@CustCode", ddlCustomer.SelectedValue);
            sp1[1] = new SqlParameter("@OrderRefNo", ddlOrder.SelectedItem.Text);
            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_SplIns", sp1, DataAccess.Return_Type.DataTable);
            txtCargoSplIns.Text = dt.Rows[0]["SplIns"].ToString().Replace("~", "\r\n");
            lblUserDetails.Text = "PLANT : " + dt.Rows[0]["Plant"].ToString() + "<br/> DATA ENTERED BY : " + dt.Rows[0]["UserName"].ToString() + " - " + dt.Rows[0]["OrderDate"].ToString();
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            GetCustomerOrders();
            gvProcessIdDetail.PageIndex = e.NewPageIndex;
            gvProcessIdDetail.DataBind();
        }

        private void GetCustomerOrders()
        {
            try
            {
                SqlParameter[] sp_param2 = new SqlParameter[2];
                sp_param2[0] = new SqlParameter("@orderid", ddlOrder.SelectedValue);
                sp_param2[1] = new SqlParameter("@custcode", ddlCustomer.SelectedValue);
                DataTable dtOrderItems = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_OrderItem", sp_param2, DataAccess.Return_Type.DataTable);

                if (dtOrderItems.Rows.Count <= 0) return;
                gvProcessIdDetail.DataSource = null;
                gvProcessIdDetail.DataBind();
                gvProcessIdDetail.DataSource = dtOrderItems;
                gvProcessIdDetail.DataBind();

                ScriptManager.RegisterStartupScript(Page, GetType(), "showSaveButton", "showSaveButton();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ProcessID", typeof(string));
                dt.Columns.Add("Weight", typeof(decimal));
                dt.Columns.Add("Width", typeof(decimal));
                dt.Columns.Add("InnerDiameter", typeof(decimal));
                dt.Columns.Add("OuterDiameter", typeof(decimal));
                dt.Columns.Add("Volume", typeof(decimal));

                for (int i = 0; i < gvProcessIdDetail.Rows.Count; i++)
                {
                    if (gvProcessIdDetail.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        HiddenField hdnProcessId = (HiddenField)gvProcessIdDetail.Rows[i].FindControl("hdnProcessID");
                        TextBox txtWeight = (TextBox)gvProcessIdDetail.Rows[i].FindControl("txtWeight");
                        TextBox txtWidth = (TextBox)gvProcessIdDetail.Rows[i].FindControl("txtWidth");
                        TextBox txtInnerDiameter = (TextBox)gvProcessIdDetail.Rows[i].FindControl("txtInnerDiameter");
                        TextBox txtOuterDiameter = (TextBox)gvProcessIdDetail.Rows[i].FindControl("txtOuterDiameter");
                        //TextBox txtVolume = (TextBox)gvProcessIdDetail.Rows[i].FindControl("txtVolume");
                        dt.Rows.Add(hdnProcessId.Value, txtWeight.Text == "" ? Convert.ToInt16("0") : Convert.ToDouble(txtWeight.Text),
                            txtWidth.Text == "" ? Convert.ToInt16("0") : Convert.ToDouble(txtWidth.Text), txtInnerDiameter.Text == "" ? Convert.ToInt16("0") : Convert.ToDouble(txtInnerDiameter.Text),
                            txtOuterDiameter.Text == "" ? Convert.ToInt16("0") : Convert.ToDouble(txtOuterDiameter.Text), Convert.ToInt16("0"));
                        //txtVolume.Text == "" ? Convert.ToInt16("0") : Convert.ToDouble(txtVolume.Text)
                    }

                }
                SqlParameter[] sp1 = new SqlParameter[]{
                    new SqlParameter("@custcode", ddlCustomer.SelectedValue),
                    new SqlParameter("@orderid", ddlOrder.SelectedValue),
                    new SqlParameter("@CargoManagement_TyreDimension_tbl", dt),
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                };
                daTTS.ExecuteNonQuery_SP("SP_INS_TyreDimensions", sp1);

                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@status",1),
                    new SqlParameter("@custcode", ddlCustomer.SelectedValue),
                    new SqlParameter("@orderid", ddlOrder.SelectedValue)
                };
                daCOTS.ExecuteNonQuery_SP("SP_UPD_CargoStatus", sp2);
                Response.Redirect("cargo_management.aspx?vid=2", false);
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
            }
        }
    }
}