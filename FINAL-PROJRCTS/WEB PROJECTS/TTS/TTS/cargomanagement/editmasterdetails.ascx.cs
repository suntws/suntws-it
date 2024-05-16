using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Web.Services;
using System.Text;

namespace TTS.cargomanagement
{
    public partial class editprocessiddetails : System.Web.UI.UserControl
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                DataTable dtUser = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_UserLevel", sp1, DataAccess.Return_Type.DataTable);
                if (dtUser.Rows.Count > 0)
                {
                    if (dtUser.Rows[0]["cotsexpcargomanagement"].ToString() == "True")
                    {
                        bindDdlPlatform();
                    }
                }
            }
        }

        private void bindDdlPlatform()
        {
            try
            {
                SqlParameter[] sp_params1 = new SqlParameter[] { new SqlParameter("@config",DBNull.Value),
                    new SqlParameter("@TyreSize",DBNull.Value),
                    new SqlParameter("@TyreRim",DBNull.Value),
                    new SqlParameter("@TyreType",DBNull.Value),
                    new SqlParameter("@Brand",DBNull.Value),
                    new SqlParameter("@Column",1) };
                DataTable dtConfigList = (DataTable)daTTS.ExecuteReader_SP("SP_LST_JATHAGAM_DETAIL", sp_params1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlPlatform, dtConfigList, "config", "config", "Choose");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            get_jathagam(2);
            ddlRimsize.Items.Clear();
            ddlTyreType.Items.Clear();
            ddlBrand.Items.Clear();
            ddlSidewall.Items.Clear();
        }

        protected void ddlTyreSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            get_jathagam(3);
            ddlTyreType.Items.Clear();
            ddlBrand.Items.Clear();
            ddlSidewall.Items.Clear();
        }

        protected void ddlRimsize_SelectedIndexChanged(object sender, EventArgs e)
        {
            get_jathagam(4);
            ddlBrand.Items.Clear();
            ddlSidewall.Items.Clear();
        }

        protected void ddlTyreType_SelectedIndexChanged(object sender, EventArgs e)
        {
            get_jathagam(5);
            ddlSidewall.Items.Clear();
        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            get_jathagam(6);
        }

        protected void ddlSidewall_SelectedIndexChanged(object sender, EventArgs e)
        {
            get_processId();
        }

        private void get_jathagam(int col)
        {
            try
            {
                string config = ddlPlatform.Text.ToString();
                string tyresize = ddlTyreSize.Text.ToString();
                string rimsize = ddlRimsize.Text.ToString();
                string tyretype = ddlTyreType.Text.ToString();
                string brand = ddlBrand.Text.ToString();
                int column = col;

                SqlParameter[] sp_params2 = new SqlParameter[] { 
                    new SqlParameter("@config",SqlDbType.NVarChar,50),
                    new SqlParameter("@TyreSize",SqlDbType.NVarChar,50),
                    new SqlParameter("@TyreRim",SqlDbType.NVarChar,50),
                    new SqlParameter("@TyreType",SqlDbType.NVarChar,50),
                    new SqlParameter("@Brand",SqlDbType.NVarChar,50),
                    new SqlParameter("@Column",column)
                };

                if (config != "") sp_params2[0].Value = config; else sp_params2[0].Value = DBNull.Value;
                if (tyresize != "") sp_params2[1].Value = tyresize; else sp_params2[1].Value = DBNull.Value;
                if (rimsize != "") sp_params2[2].Value = rimsize; else sp_params2[2].Value = DBNull.Value;
                if (tyretype != "") sp_params2[3].Value = tyretype; else sp_params2[3].Value = DBNull.Value;
                if (brand != "") sp_params2[4].Value = brand; else sp_params2[4].Value = DBNull.Value;

                DataTable dtRecordList = (DataTable)daTTS.ExecuteReader_SP("SP_LST_JATHAGAM_DETAIL", sp_params2, DataAccess.Return_Type.DataTable);

                switch (column)
                {
                    case 2:
                        Utilities.ddl_Binding(ddlTyreSize, dtRecordList, "TyreSize", "TyreSize", "Choose");
                        break;
                    case 3:
                        Utilities.ddl_Binding(ddlRimsize, dtRecordList, "TyreRim", "TyreRim", "Choose");
                        break;
                    case 4:
                        Utilities.ddl_Binding(ddlTyreType, dtRecordList, "TyreType", "TyreType", "Choose");
                        break;
                    case 5:
                        Utilities.ddl_Binding(ddlBrand, dtRecordList, "Brand", "Brand", "Choose");
                        break;
                    case 6:
                        Utilities.ddl_Binding(ddlSidewall, dtRecordList, "Sidewall", "Sidewall", "Choose");
                        break;
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToString();
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_EditMaster", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            gvTyredimension.EditIndex = -1;
            get_processId();
        }

        private void get_processId()
        {
            try
            {
                string config = "", tyresize = "", rimsize = "", sidewall = "", brand = "", tyretype = "";
                if (ddlPlatform.SelectedIndex > 0) config = ddlPlatform.Text.ToString();
                if (ddlTyreSize.SelectedIndex > 0) tyresize = ddlTyreSize.Text.ToString();
                if (ddlRimsize.SelectedIndex > 0) rimsize = ddlRimsize.Text.ToString();
                if (ddlTyreType.SelectedIndex > 0) tyretype = ddlTyreType.Text.ToString();
                if (ddlBrand.SelectedIndex > 0) brand = ddlBrand.Text.ToString();
                if (ddlSidewall.SelectedIndex > 0) sidewall = ddlSidewall.Text.ToString();

                SqlParameter[] sp_params3 = new SqlParameter[] { 
                    new SqlParameter("@config",SqlDbType.NVarChar,50),
                    new SqlParameter("@TyreSize",SqlDbType.NVarChar,50),
                    new SqlParameter("@TyreRim",SqlDbType.NVarChar,50),
                    new SqlParameter("@TyreType",SqlDbType.NVarChar,50),
                    new SqlParameter("@Brand",SqlDbType.NVarChar,50),
                    new SqlParameter("@Sidewall",SqlDbType.NVarChar,50)
                };

                if (config != "") sp_params3[0].Value = config; else sp_params3[0].Value = DBNull.Value;
                if (tyresize != "") sp_params3[1].Value = tyresize; else sp_params3[1].Value = DBNull.Value;
                if (rimsize != "") sp_params3[2].Value = rimsize; else sp_params3[2].Value = DBNull.Value;
                if (tyretype != "") sp_params3[3].Value = tyretype; else sp_params3[3].Value = DBNull.Value;
                if (brand != "") sp_params3[4].Value = brand; else sp_params3[4].Value = DBNull.Value;
                if (sidewall != "") sp_params3[5].Value = sidewall; else sp_params3[5].Value = DBNull.Value;

                DataTable dtDimensionRecord = (DataTable)daTTS.ExecuteReader_SP("SP_LST_TyreDimensions", sp_params3, DataAccess.Return_Type.DataTable);

                gvTyredimension.DataSource = null;
                gvTyredimension.DataBind();

                gvTyredimension.DataSource = dtDimensionRecord;
                gvTyredimension.DataBind();


            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToString();
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_EditMaster", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void EditDimensions(object sender, GridViewEditEventArgs e)
        {
            gvTyredimension.EditIndex = e.NewEditIndex;
            get_processId();
        }

        protected void UpdateDimensions(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                HiddenField hdnProcessId = (HiddenField)gvTyredimension.Rows[e.RowIndex].FindControl("hdnProcessId");
                TextBox txtWeight = (TextBox)gvTyredimension.Rows[e.RowIndex].FindControl("txtWeight");
                TextBox txtWidth = (TextBox)gvTyredimension.Rows[e.RowIndex].FindControl("txtWidth");
                TextBox txtInnerDiameter = (TextBox)gvTyredimension.Rows[e.RowIndex].FindControl("txtInnerDiameter");
                TextBox txtOuterDiameter = (TextBox)gvTyredimension.Rows[e.RowIndex].FindControl("txtOuterDiameter");
                TextBox txtVolume = (TextBox)gvTyredimension.Rows[e.RowIndex].FindControl("txtVolume");

                string processId, username;
                processId = hdnProcessId.Value;
                username = Request.Cookies["TTSUser"].Value.ToString();
                SqlParameter[] sp_params4 = new SqlParameter[] { 
                    new SqlParameter("@ProcessID",processId),
                    new SqlParameter("@Weight",SqlDbType.Float),
                    new SqlParameter("@Width",SqlDbType.Float),
                    new SqlParameter("@InnerDiameter",SqlDbType.Float),
                    new SqlParameter("@OuterDiameter",SqlDbType.Float),
                    new SqlParameter("@Volume",SqlDbType.Float),
                    new SqlParameter("@username",username),
                };

                if (txtWeight.Text != "") sp_params4[1].Value = Convert.ToDouble(txtWeight.Text); else sp_params4[1].Value = DBNull.Value;
                if (txtWidth.Text != "") sp_params4[2].Value = Convert.ToDouble(txtWidth.Text); else sp_params4[2].Value = DBNull.Value;
                if (txtInnerDiameter.Text != "") sp_params4[3].Value = Convert.ToDouble(txtInnerDiameter.Text); else sp_params4[3].Value = DBNull.Value;
                if (txtOuterDiameter.Text != "") sp_params4[4].Value = Convert.ToDouble(txtOuterDiameter.Text); else sp_params4[4].Value = DBNull.Value;
                if (txtVolume.Text != "") sp_params4[5].Value = Convert.ToDouble(txtVolume.Text); else sp_params4[5].Value = DBNull.Value;

                daTTS.ExecuteNonQuery_SP("SP_SAV_TyreDimensions", sp_params4);
                CancelEdit(null, null);

            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToString();
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_EditMaster", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void CancelEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTyredimension.EditIndex = -1;
            get_processId();
        }

        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            get_processId();
            gvTyredimension.PageIndex = e.NewPageIndex;
            gvTyredimension.DataBind();
        }
    }
}