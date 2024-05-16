using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class Exp_ScanCombiTyresSend : System.Web.UI.Page
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
                            (dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" 
                            || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True" ||
                            dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True"))
                        {
                            if (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageTitle.Text = "COMBI ORDER TYRES SEND TO FINAL LOADING PLANT";
                                bindgrid_events();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "URL IS WRONG";
                            }
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bindgrid_events()
        {
            try
            {
                lblCustName.Text = "";
                lblWorkorderNo.Text = "";
                txtDcno.Text = "";
                txtVehicleNo.Text = "";
                txtRemarks.Text = "";

                SqlParameter[] sp = new SqlParameter[] {  
                                    new SqlParameter("@pdiplant", Utilities.Decrypt(Request["pid"].ToString())),
                                    new SqlParameter("@combiplant", Utilities.Decrypt(Request["pid"].ToString()) == "mmn" ? "PDK" : "MMN") 
                                };
                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CombiOreder_Details", sp, DataAccess.Return_Type.DataTable);
                if (dtList.Rows.Count > 0)
                {
                    gvLoadCheckOrder.DataSource = dtList;
                    gvLoadCheckOrder.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkPdiLoad_Click(object sender, EventArgs e)
        {
            try
            {
                txtLoadTotQty.Text = "";
                txtDcno.Text = "";
                txtVehicleNo.Text = "";
                txtRemarks.Text = "";

                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnPID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderPID")).Value;
                hdnSentQty.Value = ((HiddenField)clickedRow.FindControl("hdnSenQty")).Value;
                hdnInspectQty.Value = ((HiddenField)clickedRow.FindControl("hdnInspQty")).Value;
                hdnBalToSend.Value = ((HiddenField)clickedRow.FindControl("hdnRemain")).Value;
                lblCustName.Text = ((HiddenField)clickedRow.FindControl("hdnCustname")).Value;
                lblWorkorderNo.Text = ((HiddenField)clickedRow.FindControl("hdnWo")).Value;
                divLoadDetails.Style.Add("display", "none");

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@PID", hdnPID.Value) };
                DataTable dtDc = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_DcSentqty_details", sp1, DataAccess.Return_Type.DataTable);
                if (dtDc.Rows.Count > 0)
                {

                    grdDcDetails.DataSource = dtDc;
                    grdDcDetails.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JMainDiv", "gotoPreviewDiv('div_LoadOrder');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "disElements", "disableElements();", true);
                btnBarcodeCheck_Click(sender, e);
                txtLoadTotQty.Focus();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnBarcodeCheck_Click(object sender, EventArgs e)
        {
            try
            {

                txtLoadScanStatus.Text = "";
                lblBarcode.Text = "";
                lblLoadScanQty.Text = "0";
                gvBarcodelist.DataSource = "";
                gvBarcodelist.DataBind();

                SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@ID", hdnPID.Value),
                        new SqlParameter("@barcode",txtBarcode.Text),
                        new SqlParameter("@LoadingBy",Request.Cookies["TTSUser"].Value),
                    };

                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_LoadCheck_Combi", sp, DataAccess.Return_Type.DataSet);

                if (ds.Tables[1].Rows.Count > 0)
                {
                    lblLoadScanQty.Text = ds.Tables[0].Rows[0]["qty"].ToString();
                    gvBarcodelist.DataSource = ds.Tables[1];
                    gvBarcodelist.DataBind();
                }
                if (txtBarcode.Text != "" && txtBarcode.Text.Length >= 19)
                {
                    if (ds.Tables[0].Rows[0]["result"].ToString() == "SCAN OK")
                    {
                        txtLoadScanStatus.Text = "SCAN OK";
                        txtLoadScanStatus.Style.Add("color", "#11c728");
                    }
                    else
                    {
                        txtLoadScanStatus.Text = ds.Tables[0].Rows[0]["result"].ToString();
                        txtLoadScanStatus.Style.Add("color", "#c7112a");
                    }
                    lblBarcode.Text = txtBarcode.Text;
                }
                if (lblLoadScanQty.Text == txtLoadTotQty.Text && txtLoadTotQty.Text != "")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "endLoad();", true);
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JMainDiv1", "gotoPreviewDiv('div_LoadOrder');", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "tblShow", "document.getElementById('tblBarcodeScan').style.display='block';", true);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkbtnDel_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow clckRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                SqlParameter[] spDel = new SqlParameter[]{
                    new SqlParameter("@pid",hdnPID.Value),
                    new SqlParameter("@barcode",((HiddenField)clckRow.FindControl("hdnDelBarcode")).Value)
                 };

                int res = (int)daCOTS.ExecuteNonQuery_SP("sp_del_combiLoaded_bardode", spDel);

                if (res > 0)
                {
                    btnBarcodeCheck_Click(sender, e);
                    divLoadDetails.Style.Add("display", "none");
                    lblBarcode.Text = "";
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveLoadStatus_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] spDc = new SqlParameter[]{
                    new SqlParameter("@dcPid",hdnPID.Value),
                    new SqlParameter("@dcNo",txtDcno.Text),
                    new SqlParameter("@createUser",Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@vehicleNo",txtVehicleNo.Text.ToUpper()),
                    new SqlParameter("@qty",lblLoadScanQty.Text),
                    new SqlParameter("@receivePlant",Utilities.Decrypt(Request["pid"].ToString()) == "mmn" ? "PDK" : "MMN")
                };

                int resp = (int)daCOTS.ExecuteNonQuery_SP("sp_ins_CombiPdi_Dcdetails", spDc);

                if (resp > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "DCShow", "alert('DC SUCCESSFULLY GENERATED!');", true);
                    bindgrid_events();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}