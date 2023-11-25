using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TTS.cargomanagement
{
    public partial class containerdetail : System.Web.UI.UserControl
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
                        bindddlCustomers();
                        bindddlContainers();
                    }
                }
            }
        }

        private void bindddlCustomers()
        {
            try
            {
                DataTable dtCustList = new DataTable();
                dtCustList = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_Cargo_CustomerName", DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlCustomer, dtCustList, "custfullname", "ID", "Choose");
                if (ddlCustomer.Items.Count == 2)
                {
                    ddlCustomer.SelectedIndex = 1;
                    ddlCustomer_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlCustomer.SelectedIndex <= 0) return;
                ddlOrder.Items.Clear();
                SqlParameter[] sp_param1 = new SqlParameter[] { new SqlParameter("@custcode", ddlCustomer.SelectedValue) };
                DataTable dtOrders = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_Cargo_customerOrders", sp_param1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlOrder, dtOrders, "OrderRefNo", "OrderRefNo", "Choose");
                if (ddlOrder.Items.Count == 2)
                    ddlOrder.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                lblErrMsg.Text = ex.Message.ToString();
            }
        }

        private void bindddlContainers()
        {
            try
            {
                DataTable dtContainers = (DataTable)daTTS.ExecuteReader_SP("SP_LST_Cargo_ContainerType", DataAccess.Return_Type.DataTable);

                if (dtContainers.Rows.Count <= 0) return;
                string options = "";
                for (int i = 0; i < dtContainers.Rows.Count; i++)
                {
                    options += string.Format("<option>{0}</option>", dtContainers.Rows[i]["ContainerType"].ToString());
                }
                ltrlContainerType.Mode = LiteralMode.Encode;
                ltrlContainerType.Mode = LiteralMode.PassThrough;
                ltrlContainerType.Mode = LiteralMode.Transform;
                ltrlContainerType.Text = options;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnAddDimension_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp_param2 = new SqlParameter[] { 
                    new SqlParameter("@custcode", ddlCustomer.SelectedValue.ToString()),
                    new SqlParameter("@orderid", ddlOrder.SelectedValue.ToString()),
                    new SqlParameter("@containerType", txtContainerType.Text)
                };
                DataTable dtDimensions = (DataTable)daTTS.ExecuteReader_SP("SP_GET_Cargo_ContainerDetails", sp_param2, DataAccess.Return_Type.DataTable);
                if (dtDimensions.Rows.Count > 0)
                {
                    txtHeight.Text = dtDimensions.Rows[0]["ContainerHeight"].ToString();
                    txtwidth.Text = dtDimensions.Rows[0]["containerWidth"].ToString();
                    txtLength.Text = dtDimensions.Rows[0]["ContainerLength"].ToString();
                    //txtLoadCapacity.Text = dtDimensions.Rows[0]["ContainerLoadCapacity"].ToString();
                }

                ScriptManager.RegisterStartupScript(Page, GetType(), "showDiv", "showDiv();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp_param3 = new SqlParameter[] {
                    new SqlParameter("@custcode", ddlCustomer.SelectedValue), 
                    new SqlParameter("@orderid", ddlOrder.SelectedValue), 
                    new SqlParameter("@containerType", txtContainerType.Text), 
                    new SqlParameter("@qty", Convert.ToInt32(txtContainerQty.Text)), 
                    new SqlParameter("@height", Convert.ToDouble(txtHeight.Text)), 
                    new SqlParameter("@width", Convert.ToDouble(txtwidth.Text)), 
                    new SqlParameter("@length", Convert.ToDouble(txtLength.Text)), 
                    new SqlParameter("@loadCapacity", Convert.ToDouble("0.00")),//Convert.ToDouble(txtLoadCapacity.Text)
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value)

                };
                DataTable dtOrders = (DataTable)daTTS.ExecuteReader_SP("SP_INS_Cargo_ContainerDetails", sp_param3, DataAccess.Return_Type.DataTable);

                SqlParameter[] sp2 = new SqlParameter[] { new SqlParameter("@status",2),
                    new SqlParameter("@custcode", ddlCustomer.SelectedValue),
                    new SqlParameter("@orderid", ddlOrder.SelectedValue)
                };
                daCOTS.ExecuteNonQuery_SP("SP_UPD_CargoStatus", sp2);
                Response.Redirect("cargo_management.aspx?vid=3", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}