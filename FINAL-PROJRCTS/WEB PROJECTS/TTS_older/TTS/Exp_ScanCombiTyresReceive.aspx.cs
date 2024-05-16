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
    public partial class Exp_ScanCombiTyresReceive : System.Web.UI.Page
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
                            (dtUser.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dtUser.Rows[0]["exp_pdi_pdk"].ToString() == "True"
                            || dtUser.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dtUser.Rows[0]["dom_pdi_pdk"].ToString() == "True"))
                        {
                            if (Request["fid"] != null && Utilities.Decrypt(Request["fid"].ToString()) != "")
                            {
                                lblPageTitle.Text = "COMBI ORDER RECEIVE FROM " + (Utilities.Decrypt(Request["pid"].ToString()) == "mmn" ? "PDK" : "MMN");
                                bindGrid();
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
        private void bindGrid()
        {
            try
            {
                lblCustName.Text = "";
                lblWorkorderNo.Text = "";

                SqlParameter[] sp = new SqlParameter[] {  
                                    new SqlParameter("@sendPlant", Utilities.Decrypt(Request["pid"].ToString()) == "mmn" ? "PDK" : "MMN"),
                                    new SqlParameter("@recPlant",Utilities.Decrypt(Request["pid"].ToString()))
                                };
                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_sentCombi_details", sp, DataAccess.Return_Type.DataTable);
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
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnPID.Value = ((HiddenField)clickedRow.FindControl("hdnDPID")).Value;
                hdnDCID.Value = ((HiddenField)clickedRow.FindControl("hdnDID")).Value;
                hdnDCNo.Value = ((HiddenField)clickedRow.FindControl("hdnDNo")).Value;
                lblCustName.Text = ((HiddenField)clickedRow.FindControl("hdnCustname")).Value;
                lblWorkorderNo.Text = ((HiddenField)clickedRow.FindControl("hdnWo")).Value;
                lblQtytoLoad.Text = ((HiddenField)clickedRow.FindControl("hdnSentqty")).Value;

                btnBarcodeCheck_Click(sender, e);
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
                rdblRejOpinion.Items.Clear();

                SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@dcID", hdnDCID.Value),
                        new SqlParameter("@dcPid",hdnPID.Value),
                        new SqlParameter("@barcode",txtBarcode.Text),
                        new SqlParameter("@qualStatus",rdbQualityStatus.SelectedItem.Text),
                        new SqlParameter("@rejectBy",Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@rejectReason",txtRejRemark.Text.ToUpper())
                    };

                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_upd_sel_ScanReceived_Combi", sp, DataAccess.Return_Type.DataSet);

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
                if (lblLoadScanQty.Text == lblQtytoLoad.Text)
                {
                    DataRow[] foundStatus = ds.Tables[1].Select("qstatus = 'REJECTED'");
                    hdnRejlengthVal.Value = foundStatus.Length.ToString();
                    if ( Convert.ToInt32(hdnRejlengthVal.Value) != 0)
                    {
                        rdblRejOpinion.Items.Add("INWARD TO "+Utilities.Decrypt(Request["pid"].ToString()).ToUpper()+" WAREHOUSE");
                        rdblRejOpinion.Items.Add("RETURN TO "+(Utilities.Decrypt(Request["pid"].ToString()) == "mmn" ? "PDK" : "MMN")+" PLANT");
                        ScriptManager.RegisterStartupScript(Page, GetType(), "tblRejOpinShow", "document.getElementById('tblRejOpinion').style.display='block';", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "btnSavehide", "document.getElementById('btnSave').style.display='none';", true);
                        btnSave.Text = "SEND OPINION TO SENT PLANT AND CLOSE THE DC";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "tblRejOpinhide", "document.getElementById('tblRejOpinion').style.display='none';", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "btnSaveshow", "document.getElementById('btnSave').style.display='inline-block';", true);
                        btnSave.Text = "INWARD STOCK TO WAREHOUSE";
                    }
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "endLoad();", true);
                }
                else { ScriptManager.RegisterStartupScript(Page, GetType(), "tblQualityshow", "document.getElementById('tblqualityCheck').style.display='block';", true); }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                rdbQualityStatus.SelectedIndex = 0;
                txtRejRemark.Text = "";
                ScriptManager.RegisterStartupScript(Page, GetType(), "tblShow", "document.getElementById('tblBarcodeScan').style.display='block';", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string rdbTextval = (rdblRejOpinion.SelectedIndex < 0 ? "" : rdblRejOpinion.SelectedItem.Text);

                SqlParameter[] spDc = new SqlParameter[]{
                    new SqlParameter("@dcId",hdnDCID.Value),
                    new SqlParameter("@dcPid",hdnPID.Value),
                    new SqlParameter("@dcNo",hdnDCNo.Value),
                    new SqlParameter("@recUser",Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@recOpinion",rdbTextval)
                };

                int resp = (int)daCOTS.ExecuteNonQuery_SP("sp_upd_combiReceived_DCmaster", spDc);

                if (resp > 0)
                {
                    if (Convert.ToInt32(hdnRejlengthVal.Value) != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DCShow", "alert('OPENION HAS BEEN FORWARDED TO SENT PLANT FOR DC NO: " + hdnDCNo.Value + "');", true);
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "DCShow", "alert('DC NO: " + hdnDCNo.Value + " HAS BEEN CHECKED AND INWARDED TO THE WAREHOUSE!');", true);

                    bindGrid();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}