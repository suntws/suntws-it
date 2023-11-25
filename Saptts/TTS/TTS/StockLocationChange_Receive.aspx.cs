using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class StockLocationChange_Receive : System.Web.UI.Page
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
                        lblPageTitle.Text = "STOCK LOCATION CHANGING RECEIVE (" + Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + ")";
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["stock_report"].ToString() == "True")
                        {
                            bindMainGrid();
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
        private void bindMainGrid()
        {
            try
            {
                lblErrMsgcontent.Text = "";
                gvLocatChangeOrders.DataSource = "";
                gvLocatChangeOrders.DataBind();
                gvOrderItems.DataSource = "";
                gvOrderItems.DataBind();

                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@plant",Utilities.Decrypt(Request["pid"].ToString()).ToUpper())
                };

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stockLocateChange_ReceiveItems", sp, DataAccess.Return_Type.DataTable);

                if (dt != null && dt.Rows.Count > 0)
                {
                    gvLocatChangeOrders.DataSource = dt;
                    gvLocatChangeOrders.DataBind();
                }
                else
                {
                    lblErrMsgcontent.Text = "NO RECORDS";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkOrderView_Click(object sender, EventArgs e)
        {
            try
            {
                gvOrderItems.DataSource = "";
                gvOrderItems.DataBind();
                lblErrMsgcontent.Text = "";

                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnMasterId.Value = ((HiddenField)clickedRow.FindControl("hdnMaster_Id")).Value;

                SqlParameter[] spItems = new SqlParameter[]{
                    new SqlParameter("@masterId",Convert.ToInt32(hdnMasterId.Value)),
                };

                DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_stockLocateChange_RecItems_Details", spItems, DataAccess.Return_Type.DataTable);

                if (dtItems != null && dtItems.Rows.Count > 0)
                {
                    lblTotalQty.Text = dtItems.AsEnumerable().Sum(x => x.Field<int>("qty")).ToString();
                    gvOrderItems.DataSource = dtItems;
                    gvOrderItems.DataBind();
                    btnBarcodeCheck_Click(sender, e);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JMainDiv1", "gotoPreviewDiv('divItemDetails');", true);
                }
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
                lblScannedQty.Text = "0";

                SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@masterID", Convert.ToInt32(hdnMasterId.Value)),
                        new SqlParameter("@barcode",txtBarcode.Text)
                    };

                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_locateChange_BarcodeCheck_Rec", sp, DataAccess.Return_Type.DataTable);

                if (dt != null && dt.Rows.Count > 0)
                {
                    lblScannedQty.Text = dt.Rows[0]["qty"].ToString();
                }

                if (txtBarcode.Text != "" && txtBarcode.Text.Length >= 19)
                {
                    if (dt.Rows[0]["result"].ToString() == "SCAN OK")
                    {
                        txtLoadScanStatus.Text = "SCAN OK";
                        txtLoadScanStatus.Style.Add("color", "#11c728");
                    }
                    else
                    {
                        txtLoadScanStatus.Text = dt.Rows[0]["result"].ToString();
                        txtLoadScanStatus.Style.Add("color", "#c7112a");
                    }
                    lblBarcode.Text = txtBarcode.Text;
                }
                if (lblScannedQty.Text != "" && lblTotalQty.Text == lblScannedQty.Text)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow3", "isCompleted();", true);
                }
                txtBarcode.Text = "";
                txtBarcode.Focus();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JMainDiv2", "gotoPreviewDiv('divItemDetails');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsgcontent.Text = "";

                SqlParameter[] spcomp = new SqlParameter[]{
                    new SqlParameter("@user",Request.Cookies["TTSUser"].Value),
                    new SqlParameter("@masterId",Convert.ToInt32(hdnMasterId.Value))
                };

                int rlt = daCOTS.ExecuteNonQuery_SP("sp_upd_stockLocateChange_RecOrderComplete", spcomp);

                if (rlt > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JMainDiv2", "alert('ORDER INWARDED TO " + Utilities.Decrypt(Request["recPlace"].ToString()).ToUpper() + "');", true);
                    bindMainGrid();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}