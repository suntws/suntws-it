using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;

namespace TTS
{
    public partial class stencilnosearch : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {

                    }
                }
                else
                {
                    Response.Redirect("sessionexp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            btnTrigger_Click(sender, e);
        }
        protected void btnTrigger_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                dlStencilData.DataSource = null;
                dlStencilData.DataBind();
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StencilNo", txt_StencilNo.Text) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Stencil_Search_v1", sp, DataAccess.Return_Type.DataTable);
                if (dt != null && dt.Rows.Count > 0)
                {
                    dlStencilData.DataSource = dt;
                    dlStencilData.DataBind();
                }
                else
                    lblErrMsg.Text = "NO RECORDS";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}