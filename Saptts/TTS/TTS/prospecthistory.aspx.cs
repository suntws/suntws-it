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
    public partial class prospecthistory : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["prospect_leadfeedback"].ToString() == "True")
                        {

                            Bind_Supplier_DropDown();

                            if (Request["custname"] != null && Request["custname"].ToString() != "")
                            {
                                supplierDetails(Request["custname"].ToString());
                                Bind_Cust_DropDown(Request["custname"].ToString());
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
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
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Cust_DropDown(string strCustName)
        {
            try
            {
                DataTable dtCode = (DataTable)daPORT.ExecuteReader("select Custcode from prospectcustomer where Custname='" + strCustName + "'", DataAccess.Return_Type.DataTable);

                if (dtCode.Rows.Count > 0)
                {
                    hdnProspectCode.Value = dtCode.Rows[0]["Custcode"].ToString();
                    txtHistoryCust.Text = strCustName;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_Supplier_DropDown()
        {
            try
            {
                DataTable dtSupp = (DataTable)daPORT.ExecuteReader("select sup_id,sup_name from supplierlist order by sup_name", DataAccess.Return_Type.DataTable);

                if (dtSupp.Rows.Count > 0)
                {
                    ddlSupplierName.DataSource = dtSupp;
                    ddlSupplierName.DataTextField = "sup_name";
                    ddlSupplierName.DataValueField = "sup_id";
                    ddlSupplierName.DataBind();
                }
                ddlSupplierName.Items.Add("ADD NEW SUPPLIER");
                ddlSupplierName.Items.Add("Choose");
                ddlSupplierName.Text = "Choose";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void supplierDetails(string strCustName)
        {
            try
            {
                DataTable dtCust = (DataTable)daPORT.ExecuteReader("select b.sup_name,a.sup_from,a.sup_to,a.sno from Supply_FromTo a,supplierlist b where a.sup_id=b.sup_id and a.p_custcode in (select CustCode from ProspectCustomer where Custname='" + strCustName + "')", DataAccess.Return_Type.DataTable);
                if (dtCust.Rows.Count > 0)
                {
                    gv_SupplierHistory.DataSource = dtCust;
                    gv_SupplierHistory.DataBind();
                }
                else
                {
                    gv_SupplierHistory.DataSource = "";
                    gv_SupplierHistory.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gv_SupplierHistory.EditIndex = e.NewEditIndex;
                supplierDetails(Request["custname"].ToString());
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gv_SupplierHistory.Rows[e.RowIndex];
                TextBox txtSupTo = row.FindControl("txtSupTo") as TextBox;
                HiddenField hdnSupSno = row.FindControl("hdnSupSno") as HiddenField;

                if ((txtSupTo.Text != "" && Convert.ToInt32(txtSupTo.Text) > 0) && hdnSupSno.Value != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@sup_to", txtSupTo.Text);
                    sp1[1] = new SqlParameter("@sno", hdnSupSno.Value);
                    sp1[2] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);

                    daPORT.ExecuteNonQuery_SP("sp_edit_supplier_to", sp1);
                }
                gv_SupplierHistory.EditIndex = -1;
                supplierDetails(Request["custname"].ToString());
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gv_SupplierHistory.EditIndex = -1;
                supplierDetails(Request["custname"].ToString());
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnAddHistory_click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@p_custcode", hdnProspectCode.Value);
                sp1[1] = new SqlParameter("@sup_from", txtFromYear.Text);
                sp1[2] = new SqlParameter("@sup_to", txtToYear.Text);
                sp1[3] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                if (ddlSupplierName.SelectedItem.Text != "ADD NEW SUPPLIER")
                {
                    sp1[4] = new SqlParameter("@sup_id", ddlSupplierName.SelectedItem.Value);
                    daPORT.ExecuteNonQuery_SP("Sp_Ins_Supply_FromTo", sp1);
                }
                else if (ddlSupplierName.SelectedItem.Text == "ADD NEW SUPPLIER")
                {
                    sp1[4] = new SqlParameter("@sup_name", txtNewSupplier.Text);
                    daPORT.ExecuteNonQuery_SP("Sp_Ins_New_Supply_FromTo", sp1);
                    Bind_Supplier_DropDown();
                }
                supplierDetails(txtHistoryCust.Text);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}