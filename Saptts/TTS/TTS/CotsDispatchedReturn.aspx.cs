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


namespace TTS
{
    public partial class CotsDispatchedReturn : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "")
            {
                if (!IsPostBack)
                {
                    Bind_CustomerName();
                }
            }
            else
            {
                Response.Redirect("sessionexp.aspx", false);
            }
        }
        //DropDown CustomerName
        private void Bind_CustomerName()
        {

            DataTable dt = Bind_CustomerDetails("DE0048", "CustomerName");
            if (dt.Rows.Count > 0)
            {
                ddl_CustomerName.DataSource = dt;
                ddl_CustomerName.DataTextField = "custfullname";
                ddl_CustomerName.DataValueField = "custfullname";
                ddl_CustomerName.DataBind();
                ddl_CustomerName.Items.Insert(0, "Choose CustomerName");
            }
        }
        //Build DropDown UserId
        protected void ddl_CustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_CustomerName.SelectedValue != "Choose CustomerName")
            {
                DataTable dt = Bind_CustomerDetails(ddl_CustomerName.SelectedValue.ToString(), "UserId");
                if (dt.Rows.Count > 0)
                {
                    ddl_UserId.DataSource = dt;
                    ddl_UserId.DataTextField = "username";
                    ddl_UserId.DataValueField = "ID";
                    ddl_UserId.DataBind();
                    ddl_UserId.Items.Insert(0, "Choose UserId");
                }
            }
            else
            {
                lblErrMsgcontent.Text = "Choose CustomerName";
            }

        }
        //Build DropDown OrderRefNo
        protected void ddl_UserId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_UserId.SelectedValue != "Choose UserId")
            {
                DataTable dt = Bind_CustomerDetails(ddl_UserId.SelectedValue.ToString(), "OrderRefNo");
                if (dt.Rows.Count > 0)
                {
                    ddl_OrderRefNo.DataSource = dt;
                    ddl_OrderRefNo.DataTextField = "OrderRefNo";
                    ddl_OrderRefNo.DataValueField = "OrderRefNo";
                    ddl_OrderRefNo.DataBind();
                    ddl_OrderRefNo.Items.Insert(0, "Choose OrderRefNo");
                }
            }
            else
            {
                lblErrMsgcontent.Text = "Choose UserId";
            }
        }
        //Build label itemQuantity
        protected void ddl_OrderRefNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_OrderRefNo.SelectedValue != "Choose OrderRefNo")
            {
                DataTable dt = Bind_CustomerDetails(ddl_OrderRefNo.SelectedValue.ToString(), ddl_UserId.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                        lblDispatchedQty.Text = dr["itemqty"].ToString();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow3", "document.getElementById('div_QtySelection').style.display='block'", true);
                Bind_GridView();
            }
            else
            {
                lblErrMsgcontent.Text = "Choose OrderRefNo";
            }
        }
        //Datatable to Get CutomerDetails
        private DataTable Bind_CustomerDetails(string input, string inputType)
        {
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@input", input), new SqlParameter("@inputType", inputType) };
            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CustReviseOrderDetails", sp, DataAccess.Return_Type.DataTable);
            return dt;
        }
        //Bind GridView Order Items
        private void Bind_GridView()
        {
            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@CustCode", ddl_UserId.SelectedValue.ToString()), 
                    new SqlParameter("@OrderRefNo", ddl_OrderRefNo.SelectedValue.ToString()) };
            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_DispatchedList", sp, DataAccess.Return_Type.DataTable);
            if (dt.Rows.Count > 0)
            {
                gvCustOrderItem.DataSource = dt;
                gvCustOrderItem.DataBind();
            }
        }
        //Save Revise Details
        protected void btnSendRevise_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvCustOrderItem.Rows)
            {
                CheckBox chk = row.FindControl("chk_selectQty") as CheckBox;
                if (chk.Checked)
                {
                    SqlParameter[] sp = new SqlParameter[]{
                      new SqlParameter("@CustCode",ddl_UserId.SelectedValue.ToString()),
                      new SqlParameter("@OrderRefNo",ddl_OrderRefNo.SelectedValue.ToString()),
                      new SqlParameter("@Barcode",row.Cells[1].Text),
                      new SqlParameter("@Comments",txtComments.Text),
                      new SqlParameter("@UserName",Request.Cookies["TTSUser"].Value)
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_CotsCustOrderItemRevise", sp);
                    if (resp != 0)
                        Response.Redirect("default.aspx", false);
                }
            }
        }

    }
}