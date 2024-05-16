using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TTS
{
    public partial class invoiceReport : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblPageTitle.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "Domestic " : "Export ") + "Invoice No Wise Report";

                    if (Request["fid"] != null && Request["fid"].ToString() != "")
                    {
                        SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())) };

                        DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_InvoiceRep_Plant", sp, DataAccess.Return_Type.DataTable);
                        ddlPlant.DataSource = dt;
                        ddlPlant.DataTextField = "plant";
                        ddlPlant.DataValueField = "plant";
                        ddlPlant.DataBind();
                        ddlPlant.Items.Insert(0, "--SELECT--");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlPlant_indexchanged(object sender, EventArgs e)
        {
            try
            {
                gvInvoiceNoList.DataSource = "";
                gvInvoiceNoList.DataBind();
                ddlFromYear.DataSource = "";
                ddlFromYear.DataBind();
                ddlFromMonth.DataSource = "";
                ddlFromMonth.DataBind();
                ddlToYear.DataSource = "";
                ddlToYear.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                lblErrMsg.Text = "";

                if (ddlPlant.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_InvoiceRep_Fyear", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlFromYear.DataSource = dt;
                        ddlFromYear.DataTextField = "InvoiceRepFYear";
                        ddlFromYear.DataValueField = "InvoiceRepFYear";
                        ddlFromYear.DataBind();
                        ddlFromYear.Items.Insert(0, "--SELECT--");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        protected void ddlFromYear_indexchanged(object sender, EventArgs e)
        {
            try
            {
                gvInvoiceNoList.DataSource = "";
                gvInvoiceNoList.DataBind();
                ddlFromMonth.DataSource = "";
                ddlFromMonth.DataBind();
                ddlToYear.DataSource = "";
                ddlToYear.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                lblErrMsg.Text = "";

                if (ddlFromYear.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@year", Convert.ToInt32(ddlFromYear.SelectedItem.Text)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_InvoiceRep_Fmonth", sp1, DataAccess.Return_Type.DataTable);
                    ddlFromMonth.DataSource = dt;
                    ddlFromMonth.DataTextField = "InvoiceRepFmonth";
                    ddlFromMonth.DataValueField = "InvoiceRepFmonthId";
                    ddlFromMonth.DataBind();
                    ddlFromMonth.Items.Insert(0, "--SELECT--");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlFromMonth_indexchanged(object sender, EventArgs e)
        {
            try
            {
                gvInvoiceNoList.DataSource = "";
                gvInvoiceNoList.DataBind();
                ddlToYear.DataSource = "";
                ddlToYear.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                lblErrMsg.Text = "";

                if (ddlFromMonth.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@year", Convert.ToInt32(ddlFromYear.SelectedItem.Text)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_InvoiceRep_Tyear", sp1, DataAccess.Return_Type.DataTable);
                    ddlToYear.DataSource = dt;
                    ddlToYear.DataTextField = "InvoiceRepTYear";
                    ddlToYear.DataValueField = "InvoiceRepTYear";
                    ddlToYear.DataBind();
                    ddlToYear.Items.Insert(0, "--SELECT--");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlToYear_indexchanged(object sender, EventArgs e)
        {
            try
            {
                gvInvoiceNoList.DataSource = "";
                gvInvoiceNoList.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                lblErrMsg.Text = "";

                if (ddlToYear.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@fyear", Convert.ToInt32(ddlFromYear.SelectedItem.Text)), 
                        new SqlParameter("@tyear", Convert.ToInt32(ddlToYear.SelectedItem.Text)), 
                        new SqlParameter("@fmonth", Convert.ToInt32(ddlFromMonth.SelectedItem.Value)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_Sel_InvoiceRep_Tmonth", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlToMonth.DataSource = dt;
                        ddlToMonth.DataTextField = "InvoiceRepTmonth";
                        ddlToMonth.DataValueField = "InvoiceRepTmonthId";
                        ddlToMonth.DataBind();
                        ddlToMonth.Items.Insert(0, "--SELECT--");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlToMonth_indexchanged(object sender, EventArgs e)
        {
            try
            {
                gvInvoiceNoList.DataSource = "";
                gvInvoiceNoList.DataBind();
                btnSave.Style.Add("display", "block");
                if (ddlToMonth.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@CustType", Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "DOMESTIC" : "EXPORT"), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@fyear", Convert.ToInt32(ddlFromYear.SelectedItem.Text)), 
                        new SqlParameter("@fmonth", ddlFromMonth.SelectedItem.Text), 
                        new SqlParameter("@tyear", Convert.ToInt32(ddlToYear.SelectedItem.Text)), 
                        new SqlParameter("@tmonth", ddlToMonth.SelectedItem.Text), 
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_IsExist_InvoiceRep", sp1, DataAccess.Return_Type.DataTable);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        gvInvoiceNoList.DataSource = dt;
                        gvInvoiceNoList.DataBind();
                        btnSave.Style.Add("display", "none");
                        if (dt.Rows[0]["FileName"].ToString() == "")
                            lblErrMsg.Text = "With in 30 minutes the requested excel fill will generate and send to the user mail address";
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[] { 
                    new SqlParameter("@CustType", Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "DOMESTIC" : "EXPORT"), 
                    new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                    new SqlParameter("@fyear", Convert.ToInt32(ddlFromYear.SelectedItem.Text)), 
                    new SqlParameter("@fmonth", ddlFromMonth.SelectedItem.Text), 
                    new SqlParameter("@tyear", Convert.ToInt32(ddlToYear.SelectedItem.Text)), 
                    new SqlParameter("@tmonth", ddlToMonth.SelectedItem.Text), 
                    new SqlParameter("@username", Request.Cookies["TTSUser"].Value) 
                };
                int res = daCOTS.ExecuteNonQuery_SP("sp_Ins_InvoiceRep", sp2);
                if (res > 0)
                {
                    ddlToMonth_indexchanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Request.RawUrl.ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkFileName_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkpdfFileName = (LinkButton)sender;
                string strUrl = Server.MapPath("~/invoiceReport/").Replace("TTS", "pdfs");
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + lnkpdfFileName.Text);
                Response.WriteFile(strUrl);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}