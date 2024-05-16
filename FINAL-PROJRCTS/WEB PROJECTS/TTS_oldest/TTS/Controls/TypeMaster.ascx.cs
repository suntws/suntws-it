using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace TTS.Controls
{
    public partial class TypeMaster : System.Web.UI.UserControl
    {
        DataAccess da = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
            {
                if (!IsPostBack)
                {
                    DataTable dtUser = Session["dtuserlevel"] as DataTable;
                    if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["TypeMaster"].ToString() == "True")
                    {
                        BindRepeater();
                        BindEmptyTypes();
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

        private void BindRepeater()
        {
            if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)da.ExecuteReader_SP("sp_Sel_TypeMaster", DataAccess.Return_Type.DataTable);

                gvTypeDetails.DataSource = dt;
                gvTypeDetails.DataBind();
            }
            else
            {
                Response.Redirect("sessionexp.aspx", false);
            }
        }

        private void BindEmptyTypes()
        {
            try
            {
                rdb_EmptyTypes.DataSource = "";
                rdb_EmptyTypes.DataBind();
                DataTable dt = new DataTable();
                dt = (DataTable)da.ExecuteReader_SP("Sp_Sel_EmptyNull_TypeMaster", DataAccess.Return_Type.DataTable);

                rdb_EmptyTypes.DataSource = dt;
                rdb_EmptyTypes.DataValueField = "type";
                rdb_EmptyTypes.DataTextField = "type";
                rdb_EmptyTypes.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvTypeDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvTypeDetails.EditIndex = e.NewEditIndex;
                BindRepeater();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }

        protected void gvTypeDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvTypeDetails.Rows[e.RowIndex];
                TextBox txtLip = row.FindControl("txtLip") as TextBox;
                TextBox txtLipPer = row.FindControl("txtLipPer") as TextBox;
                TextBox txtBase = row.FindControl("txtBase") as TextBox;
                TextBox txtBasePer = row.FindControl("txtBasePer") as TextBox;
                TextBox txtCenter = row.FindControl("txtCenter") as TextBox;
                TextBox txtCenterPer = row.FindControl("txtCenterPer") as TextBox;
                TextBox txtTread = row.FindControl("txtTread") as TextBox;
                TextBox txtTreadPer = row.FindControl("txtTreadPer") as TextBox;
                TextBox txtTypeDesc = row.FindControl("txtTypeDesc") as TextBox;
                CheckBox chkBead = row.FindControl("chkBead") as CheckBox;
                string strBead = chkBead.Checked == true ? "Yes" : "No";
                Label lblType = row.FindControl("lblType") as Label;
                DropDownList ddlLip = row.FindControl("ddlLip") as DropDownList;
                DropDownList ddlBase = row.FindControl("ddlBase") as DropDownList;
                DropDownList ddlCenter = row.FindControl("ddlCenter") as DropDownList;
                DropDownList ddlTread = row.FindControl("ddlTread") as DropDownList;

                SqlParameter[] sparam = new SqlParameter[12];
                sparam[0] = new SqlParameter("@type", lblType.Text);
                sparam[1] = new SqlParameter("@lipgum", ddlLip.SelectedItem.Text == "CHOOSE" ? "" : (ddlLip.SelectedItem.Text == "ADD NEW" ? txtLip.Text.ToUpper() : ddlLip.SelectedItem.Text));
                sparam[2] = new SqlParameter("@lipgumper", txtLipPer.Text);
                sparam[3] = new SqlParameter("@base", ddlBase.SelectedItem.Text == "CHOOSE" ? "" : (ddlBase.SelectedItem.Text == "ADD NEW" ? txtBase.Text.ToUpper() : ddlBase.SelectedItem.Text));
                sparam[4] = new SqlParameter("@baseper", txtBasePer.Text);
                sparam[5] = new SqlParameter("@center", ddlCenter.SelectedItem.Text == "CHOOSE" ? "" : (ddlCenter.SelectedItem.Text == "ADD NEW" ? txtCenter.Text.ToUpper() : ddlCenter.SelectedItem.Text));
                sparam[6] = new SqlParameter("@centerper", txtCenterPer.Text);
                sparam[7] = new SqlParameter("@tread", ddlTread.SelectedItem.Text == "CHOOSE" ? "" : (ddlTread.SelectedItem.Text == "ADD NEW" ? txtTread.Text.ToUpper() : ddlTread.SelectedItem.Text));
                sparam[8] = new SqlParameter("@treadper", txtTreadPer.Text);
                sparam[9] = new SqlParameter("@beadband", strBead);
                sparam[10] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sparam[11] = new SqlParameter("@TypeDesc", txtTypeDesc.Text);

                da.ExecuteNonQuery_SP("Sp_Edit_TypeMaster", sparam);
                gvTypeDetails.EditIndex = -1;
                BindRepeater();
                BindEmptyTypes();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvTypeDetails_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gvTypeDetails.EditIndex = -1;
                BindRepeater();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvTypeDetails_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                BindRepeater();
                gvTypeDetails.PageIndex = e.NewPageIndex;
                gvTypeDetails.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvTypeDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && gvTypeDetails.EditIndex == e.Row.RowIndex)
                {
                    DropDownList ddlLip = (DropDownList)e.Row.FindControl("ddlLip");
                    ddlLip.DataSource = Get_ID_Details("lipgum");
                    ddlLip.DataTextField = "lipgum";
                    ddlLip.DataValueField = "lipgum";
                    ddlLip.DataBind();
                    ddlLip.Items.Insert(0, "CHOOSE");
                    ddlLip.Items.Insert(ddlLip.Items.Count, "ADD NEW");
                    TextBox txtLip = e.Row.FindControl("txtLip") as TextBox;
                    if (txtLip.Text != "")
                        ddlLip.Items.FindByValue(txtLip.Text).Selected = true;

                    DropDownList ddlBase = (DropDownList)e.Row.FindControl("ddlBase");
                    ddlBase.DataSource = Get_ID_Details("base");
                    ddlBase.DataTextField = "base";
                    ddlBase.DataValueField = "base";
                    ddlBase.DataBind();
                    ddlBase.Items.Insert(0, "CHOOSE");
                    ddlBase.Items.Insert(ddlBase.Items.Count, "ADD NEW");
                    TextBox txtBase = e.Row.FindControl("txtBase") as TextBox;
                    if (txtBase.Text != "")
                        ddlBase.Items.FindByValue(txtBase.Text).Selected = true;

                    DropDownList ddlCenter = (DropDownList)e.Row.FindControl("ddlCenter");
                    ddlCenter.DataSource = Get_ID_Details("center");
                    ddlCenter.DataTextField = "center";
                    ddlCenter.DataValueField = "center";
                    ddlCenter.DataBind();
                    ddlCenter.Items.Insert(0, "CHOOSE");
                    ddlCenter.Items.Insert(ddlCenter.Items.Count, "ADD NEW");
                    TextBox txtCenter = e.Row.FindControl("txtCenter") as TextBox;
                    if (txtCenter.Text != "")
                        ddlCenter.Items.FindByValue(txtCenter.Text).Selected = true;

                    DropDownList ddlTread = (DropDownList)e.Row.FindControl("ddlTread");
                    ddlTread.DataSource = Get_ID_Details("tread");
                    ddlTread.DataTextField = "tread";
                    ddlTread.DataValueField = "tread";
                    ddlTread.DataBind();
                    ddlTread.Items.Insert(0, "CHOOSE");
                    ddlTread.Items.Insert(ddlTread.Items.Count, "ADD NEW");
                    TextBox txtTread = e.Row.FindControl("txtTread") as TextBox;
                    if (txtTread.Text != "")
                        ddlTread.Items.FindByValue(txtTread.Text).Selected = true;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private DataTable Get_ID_Details(string strColumn)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@StrColumn", strColumn);
                dt = (DataTable)da.ExecuteReader_SP("Sp_Sel_IDList", sp1, DataAccess.Return_Type.DataTable);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }

        protected void btnEdit1_click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sparam = new SqlParameter[12];

                sparam[0] = new SqlParameter("@type", hdnEmptyType.Value);
                sparam[1] = new SqlParameter("@lipgum", txtLip1.Text.ToUpper());
                sparam[2] = new SqlParameter("@lipgumper", txtLipPer1.Text);
                sparam[3] = new SqlParameter("@base", txtBase1.Text.ToUpper());
                sparam[4] = new SqlParameter("@baseper", txtBasePer1.Text);
                sparam[5] = new SqlParameter("@center", txtCenter1.Text.ToUpper());
                sparam[6] = new SqlParameter("@centerper", txtCenterPer1.Text);
                sparam[7] = new SqlParameter("@tread", txtTread1.Text.ToUpper());
                sparam[8] = new SqlParameter("@treadper", txtTreadPer1.Text);
                sparam[9] = new SqlParameter("@beadband", chkBead1.Checked == true ? "Yes" : "No");
                sparam[10] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sparam[11] = new SqlParameter("@TypeDesc", txtTypeDesc1.Text);

                da.ExecuteNonQuery_SP("Sp_Edit_TypeMaster", sparam);
                gvTypeDetails.EditIndex = -1;
                BindRepeater();
                BindEmptyTypes();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}