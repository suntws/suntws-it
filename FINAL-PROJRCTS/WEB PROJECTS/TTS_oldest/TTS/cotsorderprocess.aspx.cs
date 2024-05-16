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

namespace TTS
{
    public partial class cotsorderprocess : System.Web.UI.Page
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
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dom_proforma"].ToString() == "True" ||
                            dtUser.Rows[0]["dom_invoice"].ToString() == "True" || dtUser.Rows[0]["dom_paymentconfirm"].ToString() == "True"))
                        {
                            if (Request["qstring"] != null && Utilities.Decrypt(Request["qstring"].ToString()) != "")
                            {
                                lblpageHeading.Text = "";
                                switch (Utilities.Decrypt(Request["qstring"].ToString()))
                                {
                                    case "proforma":
                                        lblpageHeading.Text = "Order Process - Proforma Preparation";
                                        break;
                                    case "workorder":
                                        lblpageHeading.Text = "Order Process - WorkOrder Preparation";
                                        break;
                                    case "invoice":
                                        lblpageHeading.Text = "Order Process - Invoice Preparation";
                                        break;
                                    case "payment":
                                        lblpageHeading.Text = "Order Process - Payment Conformation";
                                        break;
                                    case "tcs":
                                        lblpageHeading.Text = "Order Process - Tcs Conformation";
                                        break;
                                }
                                Bind_gvReceivedOrderList("ALL");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL IS WRONG !!!";
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "USER PRIVILEGE DISABLED. PLEASE CONTACT ADMINISTRATOR !!!";
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
        private void Bind_gvReceivedOrderList(string Plant)
        {
            try
            {
                gvReceivedOrderList.DataSource = null;
                gvReceivedOrderList.DataBind();
                lblErrMsg.Text = "";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@Qstring", Utilities.Decrypt(Request["qstring"].ToString())), new SqlParameter("@Plant", Plant) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_vs2_ReceivedOrders", sp, DataAccess.Return_Type.DataTable);
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvReceivedOrderList.DataSource = dt;
                    gvReceivedOrderList.DataBind();
                    txtOrderCount.Text = Convert.ToString(dt.Rows.Count);
                    if (ddlplant.Items.Count == 0)
                    {
                        DataView dtDescView = new DataView(dt);
                        dtDescView.Sort = "Plant ASC";
                        DataTable distinctPlant = dtDescView.ToTable(true, "Plant");

                        ddlplant.DataSource = distinctPlant;
                        ddlplant.DataTextField = "Plant";
                        ddlplant.DataValueField = "Plant";
                        ddlplant.DataBind();
                        ddlplant.Items.Insert(0, "ALL");
                    }
                }
                else
                {
                    txtOrderCount.Text = "0";
                    lblErrMsg.Text = "NO RECORDS !!!";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_gvReceivedOrderList(ddlplant.SelectedItem.Text);
        }
        protected void lnkProcessOrders_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((Button)sender).NamingContainer as GridViewRow;
                HiddenField hdnOrderID = (HiddenField)clickedRow.FindControl("hdnOrderID");
                Response.Redirect("cotsorderprocess2.aspx?qstring=" + Request["qstring"].ToString() + "&oid=" + Utilities.Encrypt(hdnOrderID.Value.ToString()), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvReceivedOrderList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}