using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace TTS
{
    public partial class exportdocuments : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_usermaster"].ToString() == "True")
                        {
                            Fill_DocumentsList_XML();
                            Bind_ExportCustomer();
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Fill_DocumentsList_XML()
        {
            try
            {
                string strXmlUserLevelList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"exportcontent.xml");
                XmlDocument xmlLevelList = new XmlDocument();

                xmlLevelList.Load(strXmlUserLevelList);
                if (xmlLevelList != null)
                {
                    var dict = new Dictionary<string, string>();

                    DataTable dtList = new DataTable();
                    dtList.Columns.Add("text", typeof(string));

                    //Bind Process Conditions & Parameters
                    foreach (XmlNode xNode in xmlLevelList.SelectNodes("/export/documents"))
                    {
                        dtList.Rows.Add(xNode.Attributes["item"].Value);
                    }
                    if (dtList.Rows.Count > 0)
                    {
                        chkExpDocuments.DataSource = dtList;
                        chkExpDocuments.DataTextField = "text";
                        chkExpDocuments.DataValueField = "text";
                        chkExpDocuments.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_ExportCustomer()
        {
            try
            {
                DataTable dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsExportCustomer", DataAccess.Return_Type.DataTable);
                if (dtCustList.Rows.Count > 0)
                {
                    ddlExpCustName.DataSource = dtCustList;
                    ddlExpCustName.DataTextField = "custfullname";
                    ddlExpCustName.DataValueField = "custcode";
                    ddlExpCustName.DataBind();
                }
                ddlExpCustName.Items.Insert(0, "Choose");
                ddlExpCustName.Text = "Choose";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlExpCustName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlExpCustName")
                {
                    gv_ExportDocList.DataSource = null;
                    gv_ExportDocList.DataBind();
                    hdnLoginName.Value = "";
                    hdnCotsCustID.Value = "";
                    ddlExpLoginUserName.DataSource = "";
                    ddlExpLoginUserName.DataBind();
                    Select_DropDownValue();
                    if (ddlExpCustName.SelectedItem.Text != "Choose")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@custfullname", ddlExpCustName.SelectedItem.Text), 
                        new SqlParameter("@stdcustcode", ddlExpCustName.SelectedItem.Value) 
                    };
                        DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                        if (dtUserList.Rows.Count > 0)
                        {
                            ddlExpLoginUserName.DataSource = dtUserList;
                            ddlExpLoginUserName.DataTextField = "username";
                            ddlExpLoginUserName.DataValueField = "ID";
                            ddlExpLoginUserName.DataBind();
                            if (dtUserList.Rows.Count == 1)
                                ddlExpLoginUserName_IndexChange(sender, e);
                            else
                                ddlExpLoginUserName.Items.Insert(0, "Choose");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlExpLoginUserName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gv_ExportDocList.DataSource = null;
                gv_ExportDocList.DataBind();
                Select_DropDownValue();
                if (ddlExpLoginUserName.SelectedItem.Text != "Choose")
                {
                    SqlParameter[] sp3 = new SqlParameter[1];
                    sp3[0] = new SqlParameter("@Custcode", hdnCotsCustID.Value);
                    DataTable dtDoc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ExpDocumentMaster", sp3, DataAccess.Return_Type.DataTable);
                    if (dtDoc.Rows.Count > 0)
                    {
                        gv_ExportDocList.DataSource = dtDoc;
                        gv_ExportDocList.DataBind();
                        foreach (ListItem item in chkExpDocuments.Items)
                        {
                            item.Selected = false;
                            item.Enabled = true;
                            foreach (DataRow row in dtDoc.Rows)
                            {
                                if (item.Text == row["DocName"].ToString())
                                {
                                    item.Selected = true;
                                    item.Enabled = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Select_DropDownValue()
        {
            ddlExpCustName.SelectedIndex = ddlExpCustName.Items.IndexOf(ddlExpCustName.Items.FindByText(hdnFullName.Value));
            ddlExpLoginUserName.SelectedIndex = ddlExpLoginUserName.Items.IndexOf(ddlExpLoginUserName.Items.FindByText(hdnLoginName.Value));
        }
        protected void gv_ExportDocList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gv_ExportDocList.Rows[e.RowIndex];
                string strDocName = row.Cells[0].Text;
                if (strDocName != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@Custcode", hdnCotsCustID.Value);
                    sp1[1] = new SqlParameter("@DocName", strDocName);
                    sp1[2] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_del_ExpDocumentMaster", sp1);
                    if (resp > 0)
                        ddlExpLoginUserName_IndexChange(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtDoc = new DataTable();
                DataColumn dCol = new DataColumn("DocName", typeof(System.String));
                dtDoc.Columns.Add(dCol);

                foreach (ListItem item in chkExpDocuments.Items)
                {
                    if (item.Selected && item.Enabled)
                        dtDoc.Rows.Add(item.Text);
                }
                if (dtDoc.Rows.Count > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@Custcode", hdnCotsCustID.Value);
                    sp1[1] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                    sp1[2] = new SqlParameter("@ExpDocumentMaster_dt", dtDoc);

                    int resp = daCOTS.ExecuteNonQuery_SP("sp_Merge_ExpDocumentMaster", sp1);
                    if (resp > 0)
                        ddlExpLoginUserName_IndexChange(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}