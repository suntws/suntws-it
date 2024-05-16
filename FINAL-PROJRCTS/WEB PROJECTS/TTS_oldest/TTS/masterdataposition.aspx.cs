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
using System.Drawing;
namespace TTS
{
    public partial class masterdataposition : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["masterdata"].ToString() == "True")
                        {
                            Bind_gv_TypeDatais();
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
        private void Bind_gv_TypeDatais()
        {
            try
            {
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_Sel_TypeMaster", DataAccess.Return_Type.DataTable);
                gvTypeDetails.DataSource = dt;
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
                if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.Footer)
                {

                }
                else if (e.Row.RowType == DataControlRowType.DataRow && gvTypeDetails.EditIndex == e.Row.RowIndex)
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

                    DropDownList ddlInterface = (DropDownList)e.Row.FindControl("ddlInterface");
                    ddlInterface.DataSource = Get_ID_Details("Interface");
                    ddlInterface.DataTextField = "Interface";
                    ddlInterface.DataValueField = "Interface";
                    ddlInterface.DataBind();
                    ddlInterface.Items.Insert(0, "CHOOSE");
                    ddlInterface.Items.Insert(ddlInterface.Items.Count, "ADD NEW");
                    TextBox txtInterface = e.Row.FindControl("txtInterface") as TextBox;
                    if (txtInterface.Text != "")
                        ddlInterface.Items.FindByValue(txtInterface.Text).Selected = true;
                }
                else
                {
                    Label lbllipper = (Label)e.Row.FindControl("lbllipper");
                    Label lblbaseper = (Label)e.Row.FindControl("lblbaseper");
                    Label lblcenterper = (Label)e.Row.FindControl("lblcenterper");
                    Label lblthreadper = (Label)e.Row.FindControl("lbltreadper");
                    Label lblinterfaceper = (Label)e.Row.FindControl("lblinterfaceper");
                    if (lbllipper.Text == "" && lblbaseper.Text == "" && lblcenterper.Text == "" && lblthreadper.Text == "" && lblinterfaceper.Text == "")
                        e.Row.BackColor = Color.LightSkyBlue;
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
                dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_IDList", sp1, DataAccess.Return_Type.DataTable);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }
        protected void gvTypeDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvTypeDetails.EditIndex = e.NewEditIndex;
                Bind_gv_TypeDatais();
                gvTypeDetails.Rows[e.NewEditIndex].BackColor = Color.Yellow;
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
                TextBox txtInterface = row.FindControl("txtInterface") as TextBox;
                TextBox txtInterfacePer = row.FindControl("txtInterfacePer") as TextBox;
                TextBox txtTypeDesc = row.FindControl("txtTypeDesc") as TextBox;
                TextBox txtTypePosition = row.FindControl("txtTypePosition") as TextBox;
                Label lblType = row.FindControl("lblType") as Label;
                DropDownList ddlLip = row.FindControl("ddlLip") as DropDownList;
                DropDownList ddlBase = row.FindControl("ddlBase") as DropDownList;
                DropDownList ddlCenter = row.FindControl("ddlCenter") as DropDownList;
                DropDownList ddlTread = row.FindControl("ddlTread") as DropDownList;
                DropDownList ddlInterface = row.FindControl("ddlInterface") as DropDownList;

                SqlParameter[] sparam = new SqlParameter[14];
                sparam[0] = new SqlParameter("@type", lblType.Text);
                sparam[1] = new SqlParameter("@lipgum", ddlLip.SelectedItem.Text == "CHOOSE" ? "" : (ddlLip.SelectedItem.Text == "ADD NEW" ? txtLip.Text.ToUpper() : ddlLip.SelectedItem.Text));
                sparam[2] = new SqlParameter("@lipgumper", txtLipPer.Text);
                sparam[3] = new SqlParameter("@base", ddlBase.SelectedItem.Text == "CHOOSE" ? "" : (ddlBase.SelectedItem.Text == "ADD NEW" ? txtBase.Text.ToUpper() : ddlBase.SelectedItem.Text));
                sparam[4] = new SqlParameter("@baseper", txtBasePer.Text);
                sparam[5] = new SqlParameter("@center", ddlCenter.SelectedItem.Text == "CHOOSE" ? "" : (ddlCenter.SelectedItem.Text == "ADD NEW" ? txtCenter.Text.ToUpper() : ddlCenter.SelectedItem.Text));
                sparam[6] = new SqlParameter("@centerper", txtCenterPer.Text);
                sparam[7] = new SqlParameter("@tread", ddlTread.SelectedItem.Text == "CHOOSE" ? "" : (ddlTread.SelectedItem.Text == "ADD NEW" ? txtTread.Text.ToUpper() : ddlTread.SelectedItem.Text));
                sparam[8] = new SqlParameter("@treadper", txtTreadPer.Text);
                sparam[9] = new SqlParameter("@Interface", ddlInterface.SelectedItem.Text == "CHOOSE" ? "" : (ddlInterface.SelectedItem.Text == "ADD NEW" ? txtInterface.Text.ToUpper() : ddlInterface.SelectedItem.Text));
                sparam[10] = new SqlParameter("@InterfacePer", txtInterfacePer.Text);
                sparam[11] = new SqlParameter("@TypePosition", txtTypePosition.Text);
                sparam[12] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sparam[13] = new SqlParameter("@TypeDesc", txtTypeDesc.Text);

                daTTS.ExecuteNonQuery_SP("Sp_Edit_TypeMaster", sparam);
                gvTypeDetails.EditIndex = -1;
                Bind_gv_TypeDatais();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvTypeDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gvTypeDetails.EditIndex = -1;
                Bind_gv_TypeDatais();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}