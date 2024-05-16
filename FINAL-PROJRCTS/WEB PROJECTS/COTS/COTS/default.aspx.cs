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
    public partial class _default : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                    {
                        if (Session["cotsstdcode"].ToString() == "DE0048")
                        {
                            SqlParameter[] sp2 = new SqlParameter[2];
                            sp2[0] = new SqlParameter("@CustCategory", Session["cotscategory"].ToString());
                            sp2[1] = new SqlParameter("@ScotsType", Session["cotsstdcode"].ToString() == "DE0048" ? "DOMESTIC" : "EXPORT");
                            DataTable dtTxt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsCommon", sp2, DataAccess.Return_Type.DataTable);
                            if (dtTxt.Rows.Count > 0)
                                txtLeftSide.Text = dtTxt.Rows[0]["txtCommon"].ToString().Replace("~", "\r\n");

                            SqlParameter[] sp1 = new SqlParameter[1];
                            sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                            DataTable dtPay = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Payterms", sp1, DataAccess.Return_Type.DataTable);
                            if (dtPay.Rows.Count > 0)
                            {
                                txtPayTerms.Text = dtPay.Rows[0]["PaymentTerms"].ToString().Replace("~", "\r\n"); ;
                                txtInstruction.Text = "\r\n" + dtPay.Rows[0]["CustInstruction"].ToString().Replace("~", "\r\n");
                            }
                            div_Domestic.Style.Add("display", "block");
                        }
                        else
                        {
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@custcode", Session["cotscode"].ToString()) };
                            DataTable dtOrder = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Export_Pending_List_custwise", sp, DataAccess.Return_Type.DataTable);
                            if (dtOrder.Rows.Count > 0)
                            {
                                gv_OrderList.DataSource = dtOrder;
                                gv_OrderList.DataBind();
                            }
                            div_Export.Style.Add("display", "block");
                        }
                    }
                    else
                    {
                        Response.Redirect("login.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkGuidePdfFile_Click(object sender, EventArgs e)
        {
            string serverURL = Server.MapPath("~/xml/ORDER-ENTRY-GUIDE.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=ORDER-ENTRY-GUIDE.pdf");
            Response.WriteFile(serverURL);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        protected void lnkStaus_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Session["trackorder"] = clickedRow.Cells[0].Text;
                Session["hdnOID"] = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;
                Response.Redirect("frmtrackorder.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}