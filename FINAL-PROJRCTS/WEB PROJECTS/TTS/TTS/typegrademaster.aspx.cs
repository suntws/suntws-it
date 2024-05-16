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
    public partial class typegrademaster : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["gradeselect_entry"].ToString() == "True")
                        {
                            ViewState["dtCategory"] = "";
                            Build_AllCategory();
                            div1.Style.Add("display", "none");
                            div2.Style.Add("display", "none");
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

        private void Build_AllCategory()
        {
            try
            {
                gvAllCategory.DataSource = null;
                gvAllCategory.DataBind();
                gvTypeDetails.DataSource = null;
                gvTypeDetails.DataBind();
                DataTable dtCategory = new DataTable();
                DataTable dt = (DataTable)daTTS.ExecuteReader("select distinct Category from TypeGradeMaster", DataAccess.Return_Type.DataTable);
                DataColumn dmaincol;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        dmaincol = new DataColumn(row["Category"].ToString().Replace(" ", "_"), typeof(System.String));
                        dtCategory.Columns.Add(dmaincol);
                    }

                    DataTable dtMaxCount = (DataTable)daTTS.ExecuteReader("select distinct Category,count(SubCategory) as ListCount from TypeGradeMaster group by Category order by ListCount desc ", DataAccess.Return_Type.DataTable);
                    for (int j = 0; j < Convert.ToInt32(dtMaxCount.Rows[0]["ListCount"].ToString()); j++)
                    {
                        dtCategory.Rows.Add(dtCategory.NewRow());
                    }

                    DataTable dtList = (DataTable)daTTS.ExecuteReader("select Category,SubCategory,Position from TypeGradeMaster", DataAccess.Return_Type.DataTable);
                    if (dtList.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            int k = 0;
                            string strColName = row["Category"].ToString();
                            foreach (DataRow rowList in dtList.Select("Category='" + strColName + "'", "Position ASC"))
                            {
                                k++;
                                dtCategory.Rows[k - 1]["" + strColName.Replace(" ", "_") + ""] = k + ". " + rowList["SubCategory"].ToString();
                            }
                        }
                    }
                    gvAllCategory.DataSource = dtCategory;
                    gvAllCategory.DataBind();

                    string strFields = "select TyreType";
                    foreach (DataColumn col in dtCategory.Columns)
                    {
                        strFields += "," + col.ColumnName;
                    }
                    strFields += " from TypeGradeDetails order by TyreType";
                    DataTable dtDetails = (DataTable)daTTS.ExecuteReader(strFields, DataAccess.Return_Type.DataTable);
                    if (dtDetails.Rows.Count > 0)
                    {
                        gvTypeDetails.DataSource = dtDetails;
                        gvTypeDetails.DataBind();
                    }

                    string strApps = "select AppHead as Application,SubHead as SubApplication,Product";
                    foreach (DataColumn col in dtCategory.Columns)
                    {
                        strApps += "," + col.ColumnName;
                    }
                    strApps += " from TypeGradeMainHead order by ID";
                    DataTable dtApps = (DataTable)daTTS.ExecuteReader(strApps, DataAccess.Return_Type.DataTable);
                    if (dtApps.Rows.Count > 0)
                    {
                        gvApplicationDetails.DataSource = dtApps;
                        gvApplicationDetails.DataBind();
                    }

                    ViewState["dtCategory"] = dtCategory;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void rdbMasterStyle_IndexChange(object sender, EventArgs e)
        {
            try
            {
                div1.Style.Add("display", "none");
                div2.Style.Add("display", "none");
                if (rdbMasterStyle.SelectedItem.Value == "1")
                {
                    ddlCategory.DataSource = "";
                    ddlCategory.DataBind();
                    div1.Style.Add("display", "block");
                    DataTable daCate = (DataTable)daTTS.ExecuteReader("select distinct Category from TypeGradeMaster", DataAccess.Return_Type.DataTable);
                    if (daCate.Rows.Count > 0)
                    {
                        ddlCategory.DataSource = daCate;
                        ddlCategory.DataTextField = "Category";
                        ddlCategory.DataValueField = "Category";
                        ddlCategory.DataBind();
                    }
                    ddlCategory.Items.Add("Add New Category");
                    ddlCategory.Items.Add("Choose");
                    ddlCategory.Text = "Choose";
                }
                else if (rdbMasterStyle.SelectedItem.Value == "2")
                {
                    ddlTypeList.DataSource = "";
                    ddlTypeList.DataBind();
                    div2.Style.Add("display", "block");
                    DataTable daType = (DataTable)daTTS.ExecuteReader("select TyreType from TypeGradeDetails", DataAccess.Return_Type.DataTable);
                    if (daType.Rows.Count > 0)
                    {
                        ddlTypeList.DataSource = daType;
                        ddlTypeList.DataTextField = "TyreType";
                        ddlTypeList.DataValueField = "TyreType";
                        ddlTypeList.DataBind();
                    }
                    ddlTypeList.Items.Add("Add New Type");
                    ddlTypeList.Items.Add("Choose");
                    ddlTypeList.Text = "Choose";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                dlSubCategory.DataSource = null;
                dlSubCategory.DataBind();
                if (ddlCategory.SelectedItem.Text != "Choose" && ddlCategory.SelectedItem.Text != "Add New Category")
                {
                    SqlParameter[] sp1 = new SqlParameter[1];
                    sp1[0] = new SqlParameter("@Category", ddlCategory.SelectedItem.Text);
                    DataTable daSubCate = (DataTable)daTTS.ExecuteReader_SP("sp_sel_TypeGradeMaster_subcategory", sp1, DataAccess.Return_Type.DataTable);
                    if (daSubCate.Rows.Count > 0)
                    {
                        dlSubCategory.DataSource = daSubCate;
                        dlSubCategory.DataBind();
                    }
                }
                else if (ddlCategory.SelectedItem.Text == "Add New Category")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptOpen", "categoryShow();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlTypeList_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gvTypeWiseDetails.DataSource = null;
                gvTypeWiseDetails.DataBind();
                if (ddlTypeList.SelectedItem.Text != "Choose")
                {
                    DataTable dtDetails = (DataTable)daTTS.ExecuteReader("select * from TypeGradeDetails where TyreType='" + ddlTypeList.SelectedItem.Text + "'", DataAccess.Return_Type.DataTable);

                    DataTable dtCat = ViewState["dtCategory"] as DataTable;
                    if (dtCat != null && dtCat.Rows.Count > 0)
                    {
                        DataTable dtGvEntry = new DataTable();
                        DataColumn dColumn = new DataColumn("CategoryList", typeof(System.String));
                        dtGvEntry.Columns.Add(dColumn);
                        dColumn = new DataColumn("SubCategoryList", typeof(System.String));
                        dtGvEntry.Columns.Add(dColumn);
                        string strColName = string.Empty;
                        foreach (DataColumn col in dtCat.Columns)
                        {
                            strColName = col.ColumnName;
                            DataRow row = dtGvEntry.NewRow();
                            row["CategoryList"] = strColName.Replace("_", " ");
                            row["SubCategoryList"] = dtDetails.Rows.Count > 0 ? dtDetails.Rows[0]["" + strColName + ""].ToString() : "";
                            dtGvEntry.Rows.Add(row);
                        }

                        if (dtGvEntry.Rows.Count > 0)
                        {
                            gvTypeWiseDetails.DataSource = dtGvEntry;
                            gvTypeWiseDetails.DataBind();
                        }
                    }
                }
                if (ddlTypeList.SelectedItem.Text == "Add New Type")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptOpen", "typeShow();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnCategorySave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@Category", ddlCategory.SelectedItem.Text != "Add New Category" ? ddlCategory.SelectedItem.Text : txtCategory.Text);
                sp1[1] = new SqlParameter("@SubCategory", txtSubCategory.Text);
                sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                int resp = daTTS.ExecuteNonQuery_SP("sp_ins_TypeGradeMaster", sp1);

                if (resp > 0 && ddlCategory.SelectedItem.Text == "Add New Category")
                {
                    daTTS.ExecuteNonQuery("Alter table TypeGradeDetails add " + txtCategory.Text.Replace(" ", "_") + " nvarchar(50)");
                    daTTS.ExecuteNonQuery("Alter table TypeGradeMainHead add " + txtCategory.Text.Replace(" ", "_") + " nvarchar(50)");
                }

                if (resp > 0)
                    Response.Redirect("typegrademaster.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnTypeCreate_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtCat = ViewState["dtCategory"] as DataTable;
                if (dtCat != null && gvTypeWiseDetails.Rows.Count > 0 && dtCat.Rows.Count > 0 && gvTypeWiseDetails.Rows.Count == dtCat.Columns.Count)
                {
                    string qryBuild = string.Empty;
                    if (ddlTypeList.SelectedItem.Text != "Choose" && ddlTypeList.SelectedItem.Text == "Add New Type")
                    {
                        string strTypeChk = (string)daTTS.ExecuteScalar("select TyreType from TypeGradeDetails where TyreType='" + txtCreateType.Text + "'");
                        if (strTypeChk == null || strTypeChk == "")
                        {
                            qryBuild = "insert into TypeGradeDetails(TyreType,CreatedDate,ModifiedDate,UserName";
                            foreach (DataColumn col in dtCat.Columns)
                            {
                                qryBuild += "," + col.ColumnName;
                            }
                            qryBuild += ") values('" + txtCreateType.Text + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "','" + Request.Cookies["TTSUser"].Value + "'";
                            foreach (GridViewRow gvRow in gvTypeWiseDetails.Rows)
                            {
                                Label lblCateogyName = gvRow.FindControl("lblCateogyName") as Label;
                                TextBox txtSubList = gvRow.FindControl("txtSubList") as TextBox;
                                qryBuild += ",'" + txtSubList.Text + "'";
                            }
                            qryBuild += ")";
                        }
                        else
                        {
                            lblErrMsg.Text = strTypeChk + " Type already added";
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptOpen", "typeShow();", true);
                        }
                    }
                    else if (ddlTypeList.SelectedItem.Text != "Choose" && ddlTypeList.SelectedItem.Text != "Add New Type")
                    {
                        qryBuild = "update TypeGradeDetails set ModifiedDate='" + DateTime.Now.ToString() + "',UserName='" + Request.Cookies["TTSUser"].Value + "'";
                        foreach (GridViewRow gvRow in gvTypeWiseDetails.Rows)
                        {
                            Label lblCateogyName = gvRow.FindControl("lblCateogyName") as Label;
                            TextBox txtSubList = gvRow.FindControl("txtSubList") as TextBox;
                            qryBuild += "," + lblCateogyName.Text.Replace(" ", "_") + "='" + txtSubList.Text + "'";
                        }
                        qryBuild += " where TyreType='" + ddlTypeList.SelectedItem.Text + "'";
                    }
                    int resp = 0;
                    if (qryBuild.Length > 0)
                        resp = daTTS.ExecuteNonQuery(qryBuild);
                    if (resp > 0)
                        Response.Redirect("typegrademaster.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}