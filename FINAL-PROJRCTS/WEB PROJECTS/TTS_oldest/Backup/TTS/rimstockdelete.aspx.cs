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
    public partial class rimstockdelete : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 &&
                            (dtUser.Rows[0]["processid_create"].ToString() == "True" && dtUser.Rows[0]["stock_report"].ToString() == "True"))
                        {
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@RimPlant", Utilities.Decrypt(Request["spid"].ToString())) };
                            DataTable dtEdc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_edcno_for_delete", sp, DataAccess.Return_Type.DataTable);
                            if (dtEdc.Rows.Count > 0)
                            {
                                ddl_EdcNo.DataSource = dtEdc;
                                ddl_EdcNo.DataValueField = "EDCNO";
                                ddl_EdcNo.DataTextField = "EDCNO";
                                ddl_EdcNo.DataBind();
                                ddl_EdcNo.Items.Insert(0, "CHOOSE");
                            }
                            btnDeleteRimBarcode.Style.Add("display", "none");
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
                    Response.Redirect("SessionExp.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "rimstockdelete.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_EdcNo_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gvRimStock.DataSource = null;
                gvRimStock.DataBind();
                gv_RimBarcodeList.DataSource = null;
                gv_RimBarcodeList.DataBind();
                btnDeleteRimBarcode.Style.Add("display", "none");
                if (ddl_EdcNo.SelectedIndex > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { 
                        new SqlParameter("@Edcno", ddl_EdcNo.SelectedItem.Text), 
                        new SqlParameter("@RimPlant", Utilities.Decrypt(Request["spid"].ToString())) 
                    };
                    DataTable dtEdc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_edcno_rimstock_list_for_delete", sp1, DataAccess.Return_Type.DataTable);
                    if (dtEdc.Rows.Count > 0)
                    {
                        gvRimStock.DataSource = dtEdc;
                        gvRimStock.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "rimstockdelete.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvRimStock_DatBound(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gvRimStock.Rows)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@EDCNO", ((Label)row.FindControl("lblEdcNo")).Text) };
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_sel_Rim_ProcessID_Details_ForEdcNo", sp, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        FormView frmRimProcessID = (FormView)row.FindControl("frmRimProcessID_Details");
                        frmRimProcessID.DataSource = dt;
                        frmRimProcessID.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "rimstockdelete.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkRimBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                gv_RimBarcodeList.DataSource = null;
                gv_RimBarcodeList.DataBind();
                btnDeleteRimBarcode.Style.Add("display", "none");
                SqlParameter[] sp1 = new SqlParameter[] { 
                    new SqlParameter("@Edcno", ddl_EdcNo.SelectedItem.Text), 
                    new SqlParameter("@RimPlant", Utilities.Decrypt(Request["spid"].ToString())) 
                };
                DataTable dtEdc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_edcno_rimbarcode_list_for_delete", sp1, DataAccess.Return_Type.DataTable);
                if (dtEdc.Rows.Count > 0)
                {
                    gv_RimBarcodeList.DataSource = dtEdc;
                    gv_RimBarcodeList.DataBind();
                    btnDeleteRimBarcode.Style.Add("display", "block");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "rimstockdelete.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDeleteRimBarcode_click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_StencilNo = new DataTable();
                dt_StencilNo.Columns.Add("RimBarcode", typeof(string));
                foreach (GridViewRow Row in gv_RimBarcodeList.Rows)
                {
                    CheckBox chk = Row.FindControl("chk_selectQty") as CheckBox;
                    if (chk.Checked && chk.Enabled)
                        dt_StencilNo.Rows.Add(((Label)Row.FindControl("lblRimbarcode")).Text);
                }

                if (dt_StencilNo.Rows.Count > 0)
                {
                    SqlParameter[] spDel = new SqlParameter[] { 
                        new SqlParameter("@Rim_Barcode_List", dt_StencilNo), 
                        new SqlParameter("@RevisedBy", Request.Cookies["TTSUser"].Value) 
                    };
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_del_rim_barcode", spDel);

                    if (resp > 0)
                    {
                        ddl_EdcNo_IndexChange(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", "rimstockdelete.aspx", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}