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
    public partial class filterwiserequest : System.Web.UI.Page
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
                        lblPageTitle.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "e" ? "Export " : "Domestic ") + "Dispatched " +
                            (Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "Stencil " : "Item ") + " Wise Report";

                        if (Request["fid"] != null && Request["fid"].ToString() != "")
                        {
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())) };

                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP(Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "sp_SelPlant" : "sp_Sel_InvoiceRep_Plant",
                                sp, DataAccess.Return_Type.DataTable);
                            ddlPlant.DataSource = dt;
                            ddlPlant.DataTextField = "plant";
                            ddlPlant.DataValueField = "plant";
                            ddlPlant.DataBind();
                            ddlPlant.Items.Insert(0, "--SELECT--");
                        }

                        if (Utilities.Decrypt(Request["qplant"]) != null && Utilities.Decrypt(Request["qplant"].ToString()) != "" &&
                            Utilities.Decrypt(Request["qfyear"]) != null && Utilities.Decrypt(Request["qfyear"].ToString()) != "" &&
                            Utilities.Decrypt(Request["qfmonth"]) != null && Utilities.Decrypt(Request["qfmonth"].ToString()) != "" &&
                            Utilities.Decrypt(Request["qtyear"]) != null && Utilities.Decrypt(Request["qtyear"].ToString()) != "" &&
                            Utilities.Decrypt(Request["qtmonth"]) != null && Utilities.Decrypt(Request["qtmonth"].ToString()) != "")
                        {
                            ddlPlant.SelectedIndex = ddlPlant.Items.IndexOf(ddlPlant.Items.FindByValue(Utilities.Decrypt(Request["qplant"].ToString())));
                            ddlPlant_indexchanged(null, null);
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
        protected void ddlPlant_indexchanged(object sender, EventArgs e)
        {
            try
            {
                ddlFromYear.DataSource = "";
                ddlFromYear.DataBind();
                ddlFromMonth.DataSource = "";
                ddlFromMonth.DataBind();
                ddlToYear.DataSource = "";
                ddlToYear.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();

                if (ddlPlant.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP(Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "sp_sel_dispatchedstencreport_Fyear" : "sp_Sel_InvoiceRep_Fyear",
                        sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlFromYear.DataSource = dt;
                        ddlFromYear.DataTextField = Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "disStencilRepFYear" : "InvoiceRepFYear";
                        ddlFromYear.DataValueField = Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "disStencilRepFYear" : "InvoiceRepFYear";
                        ddlFromYear.DataBind();
                        ddlFromYear.Items.Insert(0, "--SELECT--");

                        if (ddlPlant.SelectedItem.Text == Utilities.Decrypt(Request["qplant"].ToString()))
                        {
                            ddlFromYear.SelectedIndex = ddlFromYear.Items.IndexOf(ddlFromYear.Items.FindByValue(Utilities.Decrypt(Request["qfyear"].ToString())));
                            ddlFromYear_indexchanged(null, null);
                        }
                        else if (ddlFromYear.Items.Count == 2)
                        {
                            ddlFromYear.SelectedIndex = 1;
                            ddlFromYear_indexchanged(null, null);
                        }
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
                ddlFromMonth.DataSource = "";
                ddlFromMonth.DataBind();
                ddlToYear.DataSource = "";
                ddlToYear.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();

                if (ddlFromYear.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@year", Convert.ToInt32(ddlFromYear.SelectedItem.Text)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP(Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "sp_sel_disStencilRep_FMonth" : "sp_Sel_InvoiceRep_Fmonth",
                        sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlFromMonth.DataSource = dt;
                        ddlFromMonth.DataTextField = Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "disStencilRepmonth" : "InvoiceRepFmonth";
                        ddlFromMonth.DataValueField = Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "disStencilRepid" : "InvoiceRepFmonthId";
                        ddlFromMonth.DataBind();
                        ddlFromMonth.Items.Insert(0, "--SELECT--");

                        if (ddlPlant.SelectedItem.Text == Utilities.Decrypt(Request["qplant"].ToString()) &&
                            ddlFromYear.SelectedItem.Text == Utilities.Decrypt(Request["qfyear"].ToString()))
                        {
                            ddlFromMonth.SelectedIndex = ddlFromMonth.Items.IndexOf(ddlFromMonth.Items.FindByText(Utilities.Decrypt(Request["qfmonth"].ToString())));
                            ddlFromMonth_indexchanged(null, null);
                        }
                        else if (ddlFromMonth.Items.Count == 2)
                        {
                            ddlFromMonth.SelectedIndex = 1;
                            ddlFromMonth_indexchanged(null, null);
                        }
                    }
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
                ddlToYear.DataSource = "";
                ddlToYear.DataBind();
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();

                if (ddlFromMonth.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@year", Convert.ToInt32(ddlFromYear.SelectedItem.Text)),
                        new SqlParameter("@month", Convert.ToInt32(ddlFromMonth.SelectedItem.Value))
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP(Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "sp_sel_disFilterReport_Tyear" : "sp_sel_InvoiceFilterReport_Tyear",
                        sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlToYear.DataSource = dt;
                        ddlToYear.DataTextField = Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "disFilterRepTYear" : "InvoiceFilTYear";
                        ddlToYear.DataValueField = Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "disFilterRepTYear" : "InvoiceFilTYear";
                        ddlToYear.DataBind();
                        ddlToYear.Items.Insert(0, "--SELECT--");

                        if (ddlPlant.SelectedItem.Text == Utilities.Decrypt(Request["qplant"].ToString()) &&
                            ddlFromYear.SelectedItem.Text == Utilities.Decrypt(Request["qfyear"].ToString()) &&
                            ddlFromMonth.SelectedItem.Text == Utilities.Decrypt(Request["qfmonth"].ToString()))
                        {
                            ddlToYear.SelectedIndex = ddlToYear.Items.IndexOf(ddlToYear.Items.FindByValue(Utilities.Decrypt(Request["qtyear"].ToString())));
                            ddlToYear_indexchanged(null, null);
                        }
                        else if (ddlToYear.Items.Count == 2)
                        {
                            ddlToYear.SelectedIndex = 1;
                            ddlToYear_indexchanged(null, null);
                        }
                    }
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
                ddlToMonth.DataSource = "";
                ddlToMonth.DataBind();
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();

                if (ddlToYear.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@fyear", Convert.ToInt32(ddlFromYear.SelectedItem.Text)), 
                        new SqlParameter("@tyear", Convert.ToInt32(ddlToYear.SelectedItem.Text)), 
                        new SqlParameter("@fmonth", Convert.ToInt32(ddlFromMonth.SelectedItem.Value)) 
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP(Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "sp_sel_disFilterReport_TMonth" : "sp_Sel_InvoiceFilter_Tmonth",
                        sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlToMonth.DataSource = dt;
                        ddlToMonth.DataTextField = Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "disFilterRepmonth" : "InvoiceFilTmonth";
                        ddlToMonth.DataValueField = Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "disFilterRepid" : "InvoiceFilTmonthId";
                        ddlToMonth.DataBind();
                        ddlToMonth.Items.Insert(0, "--SELECT--");

                        if (ddlPlant.SelectedItem.Text == Utilities.Decrypt(Request["qplant"].ToString()) &&
                            ddlFromYear.SelectedItem.Text == Utilities.Decrypt(Request["qfyear"].ToString()) &&
                            ddlFromMonth.SelectedItem.Text == Utilities.Decrypt(Request["qfmonth"].ToString()) &&
                            ddlToYear.SelectedItem.Text == Utilities.Decrypt(Request["qtyear"].ToString()))
                        {
                            ddlToMonth.SelectedIndex = ddlToMonth.Items.IndexOf(ddlToMonth.Items.FindByText(Utilities.Decrypt(Request["qtmonth"].ToString())));
                            ddlToMonth_indexchanged(null, null);
                        }
                        else if (ddlToMonth.Items.Count == 2)
                        {
                            ddlToMonth.SelectedIndex = 1;
                            ddlToMonth_indexchanged(null, null);
                        }
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
                ddlCustName.DataSource = "";
                ddlCustName.DataBind();

                if (ddlToMonth.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@fid", Utilities.Decrypt(Request["fid"].ToString())), 
                        new SqlParameter("@plant", ddlPlant.SelectedItem.Text), 
                        new SqlParameter("@fyear", Convert.ToInt32(ddlFromYear.SelectedItem.Text)), 
                        new SqlParameter("@tyear", Convert.ToInt32(ddlToYear.SelectedItem.Text)), 
                        new SqlParameter("@fmonth", Convert.ToInt32(ddlFromMonth.SelectedItem.Value)),
                        new SqlParameter("@tmonth", Convert.ToInt32(ddlToMonth.SelectedItem.Value))
                    };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP(Utilities.Decrypt(Request["fileid"].ToString()) == "dis" ? "sp_sel_disFilterReport_CustName" : "sp_sel_InvoiceFilter_CustName",
                        sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlCustName.DataSource = dt;
                        ddlCustName.DataTextField = "CustName";
                        ddlCustName.DataValueField = "CustCode";
                        ddlCustName.DataBind();
                        ddlCustName.Items.Insert(0, "--SELECT--");

                        if (dt.Rows.Count > 1)
                            ddlCustName.Items.Insert(1, "ALL");
                    }
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
                if (ddlCustName.SelectedIndex > 0)
                {
                    Response.Redirect("filterwiseresponse.aspx?qplant=" + Utilities.Encrypt(ddlPlant.SelectedItem.Text) + "&qfyear=" +
                        Utilities.Encrypt(ddlFromYear.SelectedItem.Text) + "&qfmonth=" + Utilities.Encrypt(ddlFromMonth.SelectedItem.Text) + "&qtyear=" +
                        Utilities.Encrypt(ddlToYear.SelectedItem.Text) + "&qtmonth=" + Utilities.Encrypt(ddlToMonth.SelectedItem.Text) + "&qcustcode=" +
                        Utilities.Encrypt(ddlCustName.SelectedItem.Value) + "&qcustname=" + Utilities.Encrypt(ddlCustName.SelectedItem.Text) + "&fid=" +
                        Request["fid"].ToString() + "&fileid=" + Request["fileid"].ToString(), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}