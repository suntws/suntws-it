using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace TTS
{
    public partial class custactivestatus : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
            {
                if (!IsPostBack)
                {
                    if (Request["CStatus"] != null && Request["CStatus"].ToString() != "")
                    {
                        if (Request["CStatus"].ToString() == "suspend")
                        {
                            hdnCustStatus.Value = "true";
                            lnkStatus.Text = "Suspend";
                        }
                        else if (Request["CStatus"].ToString() == "reactive")
                        {
                            hdnCustStatus.Value = "false";
                            lnkStatus.Text = "Reactive";
                        }

                        BindCustList();
                    }
                }
            }
            else
            {
                Response.Redirect("sessionexp.aspx", false);
            }
        }

        private void BindCustList()
        {
            try
            {
                DataTable dt = new DataTable();

                SqlParameter[] sparam = new SqlParameter[1];
                sparam[0] = new SqlParameter("@CustStatus", hdnCustStatus.Value);

                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Cust_StatusWise", sparam, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    chkCustList.DataSource = dt;
                    chkCustList.DataTextField = "Custname";
                    chkCustList.DataValueField = "Custname";
                    chkCustList.DataBind();
                }
                else
                {
                    chkCustList.DataSource = null;
                    chkCustList.DataBind();
                    lblMsg.Text = "No records";
                    lblMsg.Style.Add("color", "#ff0000");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkStatus_click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = "";
                for (int I = 0; I <= chkCustList.Items.Count - 1; I++)
                {
                    if (chkCustList.Items[I].Selected)
                    {
                        string strCustName = chkCustList.Items[I].Value;
                        SqlParameter[] sp1 = new SqlParameter[2];
                        sp1[0] = new SqlParameter("@Custname", strCustName);
                        sp1[1] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                        int resp = 0;
                        if (hdnCustStatus.Value == "true")
                            resp = daTTS.ExecuteNonQuery_SP("Sp_Suspend_CustMaster", sp1);
                        else
                            resp = daTTS.ExecuteNonQuery_SP("Sp_Reactive_CustMaster", sp1);

                        if (resp != 1)
                        {
                            lblMsg.Text = strCustName + " not updated";
                            lblMsg.Style.Add("color", "#ff0000");
                        }
                    }
                }
                if (lblMsg.Text == "")
                {
                    BindCustList();
                    lblMsg.Text += lnkStatus.Text + " successfully";
                    lblMsg.Style.Add("color", "#0BB104");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                lblMsg.Text = ex.Message;
                lblMsg.Style.Add("color", "#ff0000");
            }
        }
    }
}