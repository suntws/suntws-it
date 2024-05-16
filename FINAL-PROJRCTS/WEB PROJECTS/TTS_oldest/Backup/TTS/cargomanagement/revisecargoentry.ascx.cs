using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;

namespace TTS.cargomanagement
{
    public partial class revisecargoentry : System.Web.UI.UserControl
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
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
                                if (Request["vid"].ToString() == "0e")
                                {
                                    DataTable dtCustList = new DataTable();
                                    dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsCargoCustomer", DataAccess.Return_Type.DataTable);
                                    if (dtCustList.Rows.Count > 0)
                                    {
                                        ddlCustomer.DataSource = dtCustList;
                                        ddlCustomer.DataTextField = "custfullname";
                                        ddlCustomer.DataValueField = "custcode";
                                        ddlCustomer.DataBind();
                                        ddlCustomer.Items.Insert(0, "CHOOSE");
                                    }
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
        protected void ddlCustomer_SelectedIndexChange(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlOrderNo_SelectedIndexChange(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}