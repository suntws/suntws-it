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
using System.IO;

namespace TTS
{
    public partial class cotsRimReport : System.Web.UI.Page
    {
        DataAccess dba = new DataAccess(ConfigurationManager.ConnectionStrings["orderdb"].ConnectionString);
        DataTable dtMD;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        lblPageTitle.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "DOMESTIC" : "EXPORT") + " RIM STOCK SHEET";
                        SqlParameter[] sp = new SqlParameter[] { 
                            new SqlParameter("@RimPlant", Utilities.Decrypt(Request["pid"].ToString())),
                            new SqlParameter("@RimType",Utilities.Decrypt(Request["fid"].ToString()))};
                        dtMD = (DataTable)dba.ExecuteReader_SP("sel_Rimprocessid_ddlRimstock_v1", sp, DataAccess.Return_Type.DataTable);
                        if (dtMD.Rows.Count > 0)
                        {
                            DataView viewMM = new DataView(dtMD);
                            DataTable distinctRimSize = viewMM.ToTable(true, "RimSize");
                            List<string> lstRimSize = new List<string>();
                            lstRimSize = distinctRimSize.AsEnumerable().Where(n => n.Field<string>("RimSize").ToString() != "").Select(n => n.Field<string>("RimSize")).ToList<string>();
                            lstRimSize.Insert(0, "--SELECT--");
                            cmb_RimSize.DataTextField = "RimSize";
                            cmb_RimSize.DataValueField = "RimSize";
                            cmb_RimSize.DataSource = lstRimSize;
                            if (lstRimSize.Count == 2)
                                cmb_RimSize.SelectedIndex = 1;

                            DataTable distinctEDCNO = viewMM.ToTable(true, "EDCNO");
                            List<string> lstEDCNO = new List<string>();
                            lstEDCNO = dtMD.AsEnumerable().Where(n => n.Field<string>("EDCNO").ToString() != "").Select(n => n.Field<string>("EDCNO")).ToList<string>();
                            lstEDCNO.Insert(0, "--SELECT--");
                            cmb_EdcNumber.DataTextField = "EDCNO";
                            cmb_EdcNumber.DataValueField = "EDCNO";
                            cmb_EdcNumber.DataSource = lstEDCNO;
                            if (lstEDCNO.Count == 2)
                                cmb_EdcNumber.SelectedIndex = 1;
                        }
                        else
                            lblErrMsgcontent.Text = "NO RECORDS";
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

        protected void cmb_RimSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dg_StockDetails.DataSource = null;
                if (cmb_RimSize.SelectedIndex > 0)
                {
                    DataView viewMM = new DataView(dtMD);
                    DataTable distinctEDCNO = viewMM.ToTable(true, "EDCNO");
                    List<string> lstEDCNO = new List<string>();
                    lstEDCNO = dtMD.AsEnumerable().Where(n => n.Field<string>("RimSize").ToString() == cmb_RimSize.Text).Select(n => n.Field<string>("EDCNO")).ToList<string>();
                    //if (lstEDCNO.Count == 1 && cmb_EdcNumber.Items.IndexOf(lstEDCNO[0].ToString()) > 0)
                    //    cmb_EdcNumber.SelectedIndex = cmb_EdcNumber.Items.IndexOf(lstEDCNO[0].ToString());
                    //else
                    //{
                    //    lstEDCNO.Insert(0, "--SELECT--");
                    //    cmb_EdcNumber.DataTextField = "EDCNO";
                    //    cmb_EdcNumber.DataValueField = "EDCNO";
                    //    cmb_EdcNumber.DataSource = lstEDCNO;
                    //    if (lstEDCNO.Count == 2)
                    //        cmb_EdcNumber.SelectedIndex = 1;
                    //}
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void cmb_EdcNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dg_StockDetails.DataSource = null;
                if (cmb_EdcNumber.SelectedIndex > 0)
                {
                    DataView viewMM = new DataView(dtMD);
                    DataTable distinctRimSize = viewMM.ToTable(true, "RimSize");
                    List<string> lstRimSize = new List<string>();
                    lstRimSize = dtMD.AsEnumerable().Where(n => n.Field<string>("EDCNO").ToString() == cmb_EdcNumber.Text).Select(n => n.Field<string>("RimSize")).ToList<string>();
                    if (lstRimSize.Count == 1)
                        //cmb_RimSize.SelectedIndex = cmb_RimSize.Items.IndexOf(lstRimSize[0].ToString());
                    bindGridView();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindGridView()
        {
            try
            {
                if (cmb_EdcNumber.SelectedIndex > 0)
                {
                    dg_StockDetails.DataSource = null;

                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@Edcno", cmb_EdcNumber.Text), new SqlParameter("@RimPlant", Utilities.Decrypt(Request["pid"].ToString())),
                    new SqlParameter("@qstring",Utilities.Decrypt(Request["fid"].ToString()))};
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("SP_SEL_RimStock_Details_v1", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        dg_StockDetails.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}