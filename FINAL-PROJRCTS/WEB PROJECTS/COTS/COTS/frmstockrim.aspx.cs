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
    public partial class frmstockrim : System.Web.UI.Page
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
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@CustCode", Session["cotsstdcode"].ToString()) };
                        DataTable dtEdc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_edcno_pricesheet_custwise", sp, DataAccess.Return_Type.DataTable);
                        if (dtEdc.Rows.Count > 0)
                        {
                            gvRimStock.DataSource = dtEdc;
                            gvRimStock.DataBind();
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
                Utilities.WriteToErrorLog("COTS", "frmstockrim", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvRimStock_DatBound(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvRimStock.Rows)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@EDCNO", ((Label)row.FindControl("lblEdcNo")).Text) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_sel_Rim_ProcessID_Details_ForEdcNo", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        FormView frmRimProcessID = (FormView)row.FindControl("frmRimProcessID_Details");
                        frmRimProcessID.DataSource = dt;
                        frmRimProcessID.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "frmstockrim", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}