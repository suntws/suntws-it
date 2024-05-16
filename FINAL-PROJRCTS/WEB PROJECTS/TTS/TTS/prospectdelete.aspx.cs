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
using System.Text;

namespace TTS
{
    public partial class prospectdelete : System.Web.UI.Page
    {
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        //History Data
                        if (Request.Cookies["TTSUser"].Value.ToLower() == "admin" || Request.Cookies["TTSUser"].Value.ToLower() == "somu" ||
                            Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                        {
                            DataTable dtSupply = (DataTable)daPORT.ExecuteReader("select b.sup_name,a.p_custcode,a.sup_from,a.sup_to from Supply_FromTo a,supplierlist b where a.sup_id=b.sup_id", DataAccess.Return_Type.DataTable);
                            if (dtSupply.Rows.Count > 0)
                            {
                                ViewState["dtSupplyHistory"] = dtSupply;
                            }

                            ViewState["sortCol"] = "Custname";
                            ViewState["sortorder"] = "ASC";
                            DataTable dtDeleteList = (DataTable)daPORT.ExecuteReader("select Custcode,Custname,Country,focus,flag,port from ProspectCustomer where CustStatus=1", DataAccess.Return_Type.DataTable);
                            ViewState["dtDeleteList"] = dtDeleteList;
                            Bind_GV();
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

        private void Bind_GV()
        {
            try
            {
                DataTable dtDeleteList = ViewState["dtDeleteList"] as DataTable;
                if (dtDeleteList.Rows.Count > 0)
                {
                    DataView view = new DataView(dtDeleteList);
                    if (ViewState["sortCol"] != null && ViewState["sortCol"].ToString() != "")
                    {
                        if (ViewState["sortorder"].ToString() == "ASC")
                            view.Sort = ViewState["sortCol"].ToString() + " ASC";
                        else
                            view.Sort = ViewState["sortCol"].ToString() + " DESC";
                    }
                    else
                    {
                        view.Sort = "Custname ASC";
                        ViewState["sortCol"] = "Custname";
                        ViewState["sortorder"] = "ASC";
                    }
                    gv_ProspectDeleteList.DataSource = view;
                    gv_ProspectDeleteList.DataBind();
                    gv_ProspectDeleteList.HeaderRow.Cells[GetIndex(ViewState["sortCol"].ToString())].CssClass = ViewState["sortorder"].ToString() == "ASC" ? "sortasc" : "sortdesc";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen", "BindFlagBgColor();", true);
                }
                else
                {
                    ViewState["dtDeleteList"] = null;
                    gv_ProspectDeleteList.DataSource = null;
                    gv_ProspectDeleteList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public string Bind_SupplierDetails(string custid)
        {
            StringBuilder strReturn = new StringBuilder();
            string strConcat = string.Empty;
            try
            {
                DataTable dtSupply = ViewState["dtSupplyHistory"] as DataTable;
                if (dtSupply != null && dtSupply.Rows.Count > 0)
                {

                    foreach (DataRow row in dtSupply.Select("p_custcode='" + custid + "'"))
                    {
                        strConcat += "<tr><td style='width:240px;'>" + row["sup_name"].ToString() + "</td><td style='width:30px;'>" + row["sup_from"].ToString() + "</td><td style='width:30px;'>" + row["sup_to"].ToString() + "</td></tr>";
                    }
                    if (strConcat.Length > 0)
                        strReturn.Append("<table border='1px' style='width:250px;background-color:#E1EE9E;color: #000;'>" + strConcat + "</table>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strReturn.ToString();
        }

        protected void gv_ProspectDeleteList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int resp = 0;
                GridViewRow row = gv_ProspectDeleteList.Rows[e.RowIndex];
                string custCode = row.Cells[0].Text;
                if (custCode != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[2];
                    sp1[0] = new SqlParameter("@Custcode", custCode);
                    sp1[1] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                    resp = daPORT.ExecuteNonQuery_SP("sp_edit_custstatus", sp1);
                    ViewState["sortorder"] = ViewState["sortorder"].ToString() == "ASC" ? "DESC" : "ASC";
                }
                if (resp == 1)
                    Bind_GV();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gv_LeadCustList_sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen1", "showProgress();", true);
                DataTable dtDeleteList = ViewState["dtDeleteList"] as DataTable;
                if (dtDeleteList.Rows.Count > 0)
                {
                    DataView view = new DataView(dtDeleteList);
                    if (e.SortDirection == SortDirection.Ascending && ViewState["sortorder"].ToString() == "ASC")
                        view.Sort = e.SortExpression + " DESC";
                    else
                        view.Sort = e.SortExpression + " ASC";

                    ViewState["sortCol"] = e.SortExpression;
                    gv_ProspectDeleteList.DataSource = view;
                    gv_ProspectDeleteList.DataBind();

                    if (ViewState["sortorder"].ToString() == "DESC")
                    {
                        gv_ProspectDeleteList.HeaderRow.Cells[GetIndex(e.SortExpression)].CssClass = "sortasc";
                        ViewState["sortorder"] = "ASC";
                    }
                    else
                    {
                        gv_ProspectDeleteList.HeaderRow.Cells[GetIndex(e.SortExpression)].CssClass = "sortdesc";
                        ViewState["sortorder"] = "DESC";
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen", "BindFlagBgColor();", true);
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen2", "hideProgress();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen2", "hideProgress();", true);
            }
        }

        private int GetIndex(string SortExp)
        {
            int i = 0;
            foreach (DataControlField c in gv_ProspectDeleteList.Columns)
            {
                if (c.SortExpression == SortExp)
                    return i;
                else
                    i++;
            }
            return i;
        }

        protected void gvProspectDeleteList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Bind_PageIndexWise();
                gv_ProspectDeleteList.PageIndex = e.NewPageIndex;
                gv_ProspectDeleteList.DataBind();
                if (ViewState["sortorder"].ToString() == "DESC")
                    gv_ProspectDeleteList.HeaderRow.Cells[GetIndex(ViewState["sortCol"].ToString())].CssClass = "sortdesc";
                else
                    gv_ProspectDeleteList.HeaderRow.Cells[GetIndex(ViewState["sortCol"].ToString())].CssClass = "sortasc";
                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen", "BindFlagBgColor();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }


        private void Bind_PageIndexWise()
        {
            try
            {
                DataTable dtDeleteList = ViewState["dtDeleteList"] as DataTable;
                DataView view = new DataView(dtDeleteList);
                if (ViewState["sortCol"] != null && ViewState["sortCol"].ToString() != "")
                {
                    if (ViewState["sortorder"].ToString() == "ASC")
                        view.Sort = ViewState["sortCol"].ToString() + " ASC";
                    else
                        view.Sort = ViewState["sortCol"].ToString() + " DESC";
                }
                else
                {
                    view.Sort = "Custname ASC";
                    ViewState["sortCol"] = "Custname";
                    ViewState["sortorder"] = "ASC";
                }
                gv_ProspectDeleteList.DataSource = view;
                gv_ProspectDeleteList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}