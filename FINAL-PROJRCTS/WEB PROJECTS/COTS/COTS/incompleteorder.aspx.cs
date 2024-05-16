using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace COTS
{
    public partial class incompleteorder : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
            {
                try
                {
                    if (!IsPostBack)
                    {
                        SqlParameter[] sp1 = new SqlParameter[1];
                        sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                        DataTable dtRefNoList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Incomplete_orderrefno", sp1, DataAccess.Return_Type.DataTable);
                        if (dtRefNoList.Rows.Count > 0)
                        {
                            gv_Incomplete.DataSource = dtRefNoList;
                            gv_Incomplete.DataBind();
                        }
                        else
                            lblErrMsg.Text = "No Records";
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                }
            }
            else
            {
                Response.Redirect("SessionExp.aspx", false);
            }
        }
        protected void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;
                Label lblIncompleteOrderNo = (Label)clickedRow.FindControl("lblIncompleteOrderNo");
                if (lblIncompleteOrderNo.Text != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[2];
                    sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                    sp1[1] = new SqlParameter("@OrderRefNo", lblIncompleteOrderNo.Text);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_del_Cust_orderrefno", sp1);
                    if (resp > 0)
                        Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnGotoOrderEntry_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;
                Label lblIncompleteOrderNo = (Label)clickedRow.FindControl("lblIncompleteOrderNo");
                if (lblIncompleteOrderNo.Text != "")
                {
                    if (Session["cotscode"].ToString() == "2642")
                        Response.Redirect("frmexporder.aspx?qno=" + Utilities.Encrypt(lblIncompleteOrderNo.Text) + "&qtype=" + Utilities.Encrypt("old"), false);
                    else if (clickedRow.Cells[3].Text == "0")
                    {
                        if (Session["cotsstdcode"].ToString().ToUpper() == "DE0048")
                            Response.Redirect("frmitementry.aspx?qno=" + Utilities.Encrypt(lblIncompleteOrderNo.Text) + "&qtype=" + Utilities.Encrypt("old"), false);
                        else
                            Response.Redirect("frmexpitementry.aspx?qno=" + Utilities.Encrypt(lblIncompleteOrderNo.Text) + "&qtype=" + Utilities.Encrypt("old"), false);
                    }
                    else
                        Response.Redirect("frmproforma.aspx?qcomplete=" + Utilities.Encrypt(lblIncompleteOrderNo.Text), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}