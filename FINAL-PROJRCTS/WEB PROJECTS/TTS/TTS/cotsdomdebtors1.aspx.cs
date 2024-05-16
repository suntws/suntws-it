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
using System.Globalization;

namespace TTS
{
    public partial class cotsdomdebtors1 : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        int res;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        lblPageTitle.Text = "DOMESTIC DEBTORS DETAILS";

                        if (Request["qstring"] != null && Request["qstring"].ToString() != "")
                        {
                            SqlParameter[] sp = new SqlParameter[]{
                                new SqlParameter("@plant",Utilities.Decrypt(Request["qstring"].ToString()))};
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_selCustName", sp,DataAccess.Return_Type.DataTable);
                            ddlCustName.DataSource = dt;
                            ddlCustName.DataTextField = "CustName";
                            ddlCustName.DataValueField = "CustName";
                            ddlCustName.DataBind();
                            ddlCustName.Items.Insert(0, "--SELECT--");
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
        protected void ddlCustName_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlUserId.DataSource = "";
                ddlUserId.DataBind();
                lblCrm.Text = "";
                lblCateg.Text = "";
                lblRegion.Text = "";
                lblCreDays.Text = "";
                lblCreLimit.Text = "";

                if (ddlCustName.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[]{
                        new SqlParameter("@custname",ddlCustName.SelectedItem.ToString())
                    };

                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_selCustID", sp, DataAccess.Return_Type.DataTable);
                    ddlUserId.DataSource = dt;
                    ddlUserId.DataTextField = "userid";
                    ddlUserId.DataValueField = "userid";
                    ddlUserId.DataBind();
                    ddlUserId.Items.Insert(0, "--SELECT--");

                    if (dt.Rows.Count == 1)
                    {
                        ddlUserId.SelectedIndex = 1;

                        ddlUserId_indexchanged(sender, e);

                    }
                }
            }
            catch (Exception ex)
            {


                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        protected void ddlUserId_indexchanged(object sender, EventArgs e)
        {
            try
            {
                lblCrm.Text = "";
                lblCateg.Text = "";
                lblRegion.Text = "";
                lblCreDays.Text = "";
                lblCreLimit.Text = "";
                txtTodayRecAmt.Focus();

                if (ddlUserId.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] {
                        new SqlParameter("@userid",ddlUserId.SelectedItem.ToString())
                    };
                    DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_debtDetails", sp, DataAccess.Return_Type.DataSet);

                    lblCrm.Text = ds.Tables[0].Rows[0]["crm"].ToString();
                    lblCateg.Text = ds.Tables[0].Rows[0]["category"].ToString();
                    lblRegion.Text = ds.Tables[0].Rows[0]["region"].ToString();
                    lblCreDays.Text = ds.Tables[0].Rows[0]["creditdays"].ToString();
                    lblCreLimit.Text = ds.Tables[0].Rows[0]["creditlimit"].ToString();

                    gvInvDebtors.DataSource = ds.Tables[1];
                    gvInvDebtors.DataBind();
                    btnSave.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow Row in gvInvDebtors.Rows)
                {
                    CheckBox chk = Row.FindControl("chkSelect") as CheckBox;
                    TextBox txt = Row.FindControl("txtRecAmt") as TextBox;
                    HiddenField hdn = Row.FindControl("hdnPending") as HiddenField;
                    if (chk.Checked && chk.Enabled)
                    {
                        SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@invno", Row.Cells[1].Text),
                        new SqlParameter("@recamt", txt.Text), 
                        new SqlParameter("@pendamt", hdn.Value),
                        new SqlParameter("@recon", DateTime.ParseExact(txtDatepick.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                        new SqlParameter("@recby", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@plant", Utilities.Decrypt(Request["qstring"].ToString()))
                        };

                        res += daCOTS.ExecuteNonQuery_SP("sp_ins_debtDetails", sp);
                    }
                }
                if (res > 0)
                {
                    Response.Redirect(Request.RawUrl.ToString(), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}