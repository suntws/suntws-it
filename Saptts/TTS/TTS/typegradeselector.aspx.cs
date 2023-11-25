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
using System.Web.UI.HtmlControls;
using System.Web.Services;

namespace TTS
{
    public partial class typegradeselector : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["gradeselect_tool"].ToString() == "True")
                        {
                            DataTable dtGradeHead = (DataTable)daTTS.ExecuteReader("select AppHead,SubHead,Product from TypeGradeMainHead", DataAccess.Return_Type.DataTable);
                            ViewState["dtGradeHead"] = dtGradeHead;

                            DataTable dtMainApp = (DataTable)daTTS.ExecuteReader("select * from TypeGradeMainHead", DataAccess.Return_Type.DataTable);
                            ViewState["dtMainApp"] = dtMainApp;

                            DataTable dtGradeMaster = (DataTable)daTTS.ExecuteReader("select  Category,SubCategory,Position from TypeGradeMaster", DataAccess.Return_Type.DataTable);
                            Session.Add("dtGradeMaster", dtGradeMaster);

                            Build_ApplicationHead();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Build_ApplicationHead()
        {
            try
            {
                lblApplication.Text = "";
                dlAppHead.DataSource = null;
                dlAppHead.DataBind();
                DataTable dt = ViewState["dtGradeHead"] as DataTable;
                DataView view = new DataView(dt);
                view.Sort = "AppHead ASC";
                DataTable dtApp = view.ToTable(true, "AppHead");

                if (dtApp.Rows.Count > 0)
                {
                    lblApplication.Text = "APPLICATION";
                    dlAppHead.DataSource = dtApp;
                    dlAppHead.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void dlAppHead_IndexChange(object sender, EventArgs e)
        {
            try
            {
                int idx = dlAppHead.SelectedIndex;
                LinkButton lnkAppHead = (LinkButton)dlAppHead.Items[idx].FindControl("lnkAppHead");
                hdnAppIndex.Value = idx.ToString();

                hdnSubIndex.Value = "";
                hdnProIndex.Value = "";
                hdnSubApplication.Value = "";
                hdnProduct.Value = "";
                lblSubApplication.Text = "";
                dlSubAppHead.DataSource = null;
                dlSubAppHead.DataBind();
                lblProduct.Text = "";
                dlProduct.DataSource = null;
                dlProduct.DataBind();
                gvCategoryMasterList.DataSource = null;
                gvCategoryMasterList.DataBind();
                divSubApp.Style.Add("display", "none");
                divProduct.Style.Add("display", "none");

                if (lnkAppHead.Text != "")
                {
                    hdnApplication.Value = lnkAppHead.Text;
                    DataTable dt = ViewState["dtGradeHead"] as DataTable;
                    DataTable dtSubApp = new DataTable();
                    DataColumn col = new DataColumn("SubHead", typeof(System.String));
                    dtSubApp.Columns.Add(col);
                    foreach (DataRow row in dt.Select("AppHead='" + lnkAppHead.Text + "' and SubHead<>''"))
                    {
                        dtSubApp.Rows.Add(row["SubHead"].ToString());
                    }
                    if (dtSubApp.Rows.Count > 0)
                    {
                        DataView view = new DataView(dtSubApp);
                        view.Sort = "SubHead ASC";
                        DataTable dtSub = view.ToTable(true, "SubHead");

                        lblSubApplication.Text = "SUB APPLICATION";
                        dlSubAppHead.DataSource = dtSub;
                        dlSubAppHead.DataBind();
                        divSubApp.Style.Add("display", "block");
                    }
                    else
                    {
                        DataTable dtProduct = new DataTable();
                        col = new DataColumn("Product", typeof(System.String));
                        dtProduct.Columns.Add(col);
                        foreach (DataRow row in dt.Select("AppHead='" + lnkAppHead.Text + "'"))
                        {
                            dtProduct.Rows.Add(row["Product"].ToString());
                        }
                        if (dtProduct.Rows.Count > 0)
                        {
                            DataView view = new DataView(dtProduct);
                            view.Sort = "Product ASC";
                            DataTable dtPro = view.ToTable(true, "Product");

                            lblProduct.Text = "PRODUCT";
                            dlProduct.DataSource = dtPro;
                            dlProduct.DataBind();
                            divProduct.Style.Add("display", "block");
                        }
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptDL", "datalistSelectIndexCss('dlAppHead','" + hdnAppIndex.Value + "');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void dlSubAppHead_IndexChange(object sender, EventArgs e)
        {
            try
            {
                int idx = dlSubAppHead.SelectedIndex;
                LinkButton lnkSubAppHead = (LinkButton)dlSubAppHead.Items[idx].FindControl("lnkSubAppHead");
                hdnSubIndex.Value = idx.ToString();

                hdnProIndex.Value = "";
                hdnProduct.Value = "";
                lblProduct.Text = "";
                dlProduct.DataSource = null;
                dlProduct.DataBind();
                gvCategoryMasterList.DataSource = null;
                gvCategoryMasterList.DataBind();
                divProduct.Style.Add("display", "none");

                if (lnkSubAppHead.Text != "")
                {
                    hdnSubApplication.Value = lnkSubAppHead.Text;
                    DataTable dt = ViewState["dtGradeHead"] as DataTable;
                    DataTable dtProduct = new DataTable();
                    DataColumn col = new DataColumn("Product", typeof(System.String));
                    dtProduct.Columns.Add(col);
                    foreach (DataRow row in dt.Select("AppHead='" + hdnApplication.Value + "' and SubHead='" + lnkSubAppHead.Text + "'"))
                    {
                        dtProduct.Rows.Add(row["Product"].ToString());
                    }
                    if (dtProduct.Rows.Count > 0)
                    {
                        DataView view = new DataView(dtProduct);
                        view.Sort = "Product ASC";
                        DataTable dtPro = view.ToTable(true, "Product");

                        lblProduct.Text = "PRODUCT";
                        dlProduct.DataSource = dtPro;
                        dlProduct.DataBind();
                        divProduct.Style.Add("display", "block");
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptDL1", "datalistSelectIndexCss('dlAppHead','" + hdnAppIndex.Value + "');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptDL2", "datalistSelectIndexCss('dlSubAppHead','" + hdnSubIndex.Value + "');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void dlProduct_IndexChange(object sender, EventArgs e)
        {
            try
            {
                int idx = dlProduct.SelectedIndex;
                LinkButton lnkProductList = (LinkButton)dlProduct.Items[idx].FindControl("lnkProductList");
                hdnProIndex.Value = idx.ToString();

                gvCategoryMasterList.DataSource = null;
                gvCategoryMasterList.DataBind();

                if (lnkProductList.Text != "")
                {
                    hdnProduct.Value = lnkProductList.Text;
                    Build_AllCategory();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptDL1", "datalistSelectIndexCss('dlAppHead','" + hdnAppIndex.Value + "');", true);
                    if (hdnSubIndex.Value != "")
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptDL2", "datalistSelectIndexCss('dlSubAppHead','" + hdnSubIndex.Value + "');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptDL3", "datalistSelectIndexCss('dlProduct','" + hdnProIndex.Value + "');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Build_AllCategory()
        {
            try
            {
                gvCategoryMasterList.DataSource = null;
                gvCategoryMasterList.DataBind();
                DataTable dtCategory = new DataTable();
                DataTable dt = (DataTable)daTTS.ExecuteReader("select distinct Category from TypeGradeMaster", DataAccess.Return_Type.DataTable);
                Build_HiddenFieldForCategory(dt);
                DataColumn dmaincol;
                if (dt.Rows.Count > 0)
                {
                    string contQry = string.Empty;
                    contQry = "AppHead='" + hdnApplication.Value + "' and Product='" + hdnProduct.Value + "'";
                    if (hdnSubApplication.Value != "")
                        contQry += " and SubHead='" + hdnSubApplication.Value + "'";
                    DataTable dtMainApp = ViewState["dtMainApp"] as DataTable;
                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (DataRow mRow in dtMainApp.Select("" + contQry + " and " + row["Category"].ToString().Replace(" ", "_") + "<>''"))
                        {
                            dmaincol = new DataColumn(row["Category"].ToString().Replace(" ", "_"), typeof(System.String));
                            dtCategory.Columns.Add(dmaincol);
                        }
                    }

                    DataTable dtMaxCount = (DataTable)daTTS.ExecuteReader("select distinct Category,count(SubCategory) as ListCount from TypeGradeMaster group by Category order by ListCount desc ", DataAccess.Return_Type.DataTable);
                    for (int j = 0; j < Convert.ToInt32(dtMaxCount.Rows[0]["ListCount"].ToString()); j++)
                    {
                        dtCategory.Rows.Add(dtCategory.NewRow());
                    }

                    DataTable dtList = (DataTable)daTTS.ExecuteReader("select Category,SubCategory,Position from TypeGradeMaster", DataAccess.Return_Type.DataTable);
                    if (dtList.Rows.Count > 0)
                    {
                        foreach (DataColumn mCol in dtCategory.Columns)
                        {
                            int k = 0;
                            string strColName = mCol.ColumnName;
                            foreach (DataRow rowList in dtList.Select("Category='" + strColName.Replace("_", " ") + "'", "Position ASC"))
                            {
                                dtCategory.Rows[k]["" + strColName + ""] = rowList["SubCategory"].ToString();
                                k++;
                            }
                        }
                    }
                    gvCategoryMasterList.DataSource = dtCategory;
                    gvCategoryMasterList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Build_HiddenFieldForCategory(DataTable dtCat)
        {
            try
            {
                if (dtCat.Rows.Count > 0)
                {
                    HtmlGenericControl table = new HtmlGenericControl("table");
                    foreach (DataRow row in dtCat.Rows)
                    {
                        HtmlGenericControl tr = new HtmlGenericControl("tr");

                        HtmlGenericControl td1 = new HtmlGenericControl("td");
                        Label lblField = new Label();
                        lblField.ID = "lbl_" + row["Category"].ToString().Replace(" ", "_");
                        td1.Controls.Add(lblField);
                        tr.Controls.Add(td1);

                        //HtmlGenericControl td2 = new HtmlGenericControl("td");
                        //HiddenField hdnField = new HiddenField();
                        //hdnField.ID = "hdn_" + row["Category"].ToString().Replace(" ", "_");
                        //td2.Controls.Add(hdnField);
                        //tr.Controls.Add(td2);
                        table.Controls.Add(tr);
                    }
                    PlaceHolderHidden.Controls.Add(table);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnResetPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("typegradeselector.aspx", false);
        }

        #region[Get Grade Selector WebMethod]
        [WebMethod]
        public static string get_GradeSelector_WebMethod(string strQuery)
        {
            return Get_Grade_Details(strQuery.ToString()).GetXml();
        }

        private static DataSet Get_Grade_Details(string strQ)
        {
            DataAccess daTTSWebMethod = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = HttpContext.Current.Session["dtGradeMaster"] as DataTable;
                string makeQuery = "select distinct TyreType from TypeGradeDetails";
                string ConQuery = string.Empty;
                string[] splitCategory = strQ.Split('~');
                for (int j = 0; j < splitCategory.Length; j++)
                {
                    string[] splitVal = splitCategory[j].ToString().Split('|');
                    var conField = splitVal[0].ToString();
                    string[] splitIDs = splitVal[1].ToString().Split(',');
                    for (int k = 0; k < splitIDs.Length; k++)
                    {
                        foreach (DataRow row in dt.Select("Category='" + conField.Replace("_", " ") + "' and SubCategory='" + splitIDs[k].ToString() + "'"))
                        {
                            if (ConQuery.Length > 0)
                                ConQuery += " and " + conField + " like '%" + row["Position"].ToString() + "%'";
                            else
                                ConQuery = conField + " like '%" + row["Position"].ToString() + "%'";
                        }
                    }
                }
                if (ConQuery.Length > 0)
                {
                    makeQuery = makeQuery + " where " + ConQuery;
                }

                ds = (DataSet)daTTSWebMethod.ExecuteReader(makeQuery, DataAccess.Return_Type.DataSet);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "PreparePriceSheet.aspx.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return ds;
        }
        #endregion
    }
}