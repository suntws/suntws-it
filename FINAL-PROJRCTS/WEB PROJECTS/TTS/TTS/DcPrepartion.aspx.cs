using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TTS
{
    public partial class DcPrepartion : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["fin_dc_mmn"].ToString() == "True"))
                        {

                            if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dc_mmn"].ToString() == "True"))
                            {
                                btnapprove.Visible = false;

                            }
                            Bind_gvReceivedOrderList();

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "USER PRIVILEGE DISABLED. PLEASE CONTACT ADMINISTRATOR !!!";
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

        private void Bind_gvReceivedOrderList()
        {
            try
            {
                GRVIEWLALL.DataSource = null;
                GRVIEWLALL.DataBind();
                lblErrMsg.Text = "";
                SqlParameter[] sp = new SqlParameter[] { (new SqlParameter("@Plant", "mmn")) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_dcOrders", sp, DataAccess.Return_Type.DataTable);
                if (dt != null && dt.Rows.Count > 0)
                {            
                    GRVIEWLALL.DataSource = dt;
                    GRVIEWLALL.DataBind();



                }
                else
                {

                    lblErrMsg.Text = "NO RECORDS !!!";
                }


            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }


        }
        
        protected void lnkProcessOrders_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow ClickedRow = ((Button)sender).NamingContainer as GridViewRow;
                hdnSelectedRow.Value = Convert.ToString(ClickedRow.RowIndex);
                Bind_GVscannedList(ClickedRow);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('divbarcodelist');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        //public override void VerifyRenderingInServerForm(Control control)
        //{
        //    //
        //}

        //protected void GRVIEWLALL_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}
        private void Bind_GVscannedList(GridViewRow Row)
        {
            try
            {
                ViewState["dtscanneslist"] = null;
                lblDCNO.Text = ((Label)Row.FindControl("lblDcno")).Text;
                Hdndcid.Value = ((HiddenField)Row.FindControl("hdntid")).Value;
                lblSelectedVEHICLENO.Text = ((Label)Row.FindControl("Lblvehicle")).Text;
                lblSelectedOrderQty.Text = ((Label)Row.FindControl("Lblorderqty")).Text;


                SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@tid", Hdndcid.Value),

                };
                //DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_PreScannedList", sp, DataAccess.Return_Type.DataTable);
                //if (dt.Rows.Count > 0)
                //{
                //    gv_ScanedList.DataSource = dt;
                //    gv_ScanedList.DataBind();
                //    ViewState["dtscanneslist"] = dt;


                //}

                DataSet ds = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_PreScannedList", sp, DataAccess.Return_Type.DataSet);
                
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    //weight.Text = dt.TableName[2].Rows[1]["totalweight"].ToString();

                    weight.Text = ds.Tables[1].Rows[0]["totalweight"].ToString();

                    gv_ScanedList.DataSource = dt;
                    gv_ScanedList.DataBind();
                    ViewState["dtscanneslist"] = dt;


                }


                Button1.CssClass = "Clicked";
                Button2.CssClass = "Initial";
                MultiView1.ActiveViewIndex = 0;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        
    

        
        protected void gv_ScanedList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridViewRow Mainrow = GRVIEWLALL.Rows[Convert.ToInt32(hdnSelectedRow.Value)];
                Bind_GVscannedList(Mainrow);
                gv_ScanedList.PageIndex = e.NewPageIndex;
                gv_ScanedList.DataBind();
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('divbarcodelist');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void Tab_Click(object sender, EventArgs e)
        {
            try
            {
                Button1.CssClass = "Initial";
                Button2.CssClass = "Initial";
                Button lnkTxt = sender as Button;
                if (lnkTxt.Text == "BARCODE WISE")
                {
                    Button1.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 0;
                }
                else if (lnkTxt.Text == "ITEM QTY WISE")
                {
                    if (Hdndcid.Value != "")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@tid", Hdndcid.Value) };
                        DataTable dtQty = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_prescanItemQty", sp1, DataAccess.Return_Type.DataTable);
                        if (dtQty.Rows.Count > 0)
                        {
                            gvScannedItemWise.DataSource = dtQty;
                            gvScannedItemWise.DataBind();
                        }
                    }
                    Button2.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 1;
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShowfront", "gotoPreviewDiv('divbarcodelist');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnapprove_click(object sender, EventArgs e)
        {
            if (Hdndcid.Value != "")
            {
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@tid", Hdndcid.Value),new SqlParameter("@username", Request.Cookies["TTSUser"].Value) };
                int resp2 = daCOTS.ExecuteNonQuery_SP("sp_approve_dc", sp1);
                if(resp2 > 0)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JVerify3", "alert('APPROVED:');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JVerify3", "alert('RECHECK IT');", true);
                }


            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        protected void exportclick(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                if (ViewState["dtscanneslist"] != null)
                {
                    string s = "";
                    string r = s.Reverse().ToString();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.xls", "scanned" + DateTime.Now.ToShortDateString() + ""));
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/x-msexcel";
                    using (System.IO.StringWriter sw = new System.IO.StringWriter())
                    {
                        HtmlTextWriter hw = new HtmlTextWriter(sw);
                        gv_ScanedList.AllowPaging = false;
                        DataTable dtList = ViewState["dtscanneslist"] as DataTable;
                        gv_ScanedList.DataSource = dtList;
                        gv_ScanedList.DataBind();
                        gv_ScanedList.RenderControl(hw);
                        Response.Write(sw.ToString());
                        Response.Flush();
                        Response.Clear();
                        Response.End();
                    }
                }
                else
                    lblErrMsg.Text = "NO RECORDS";
            }
            catch (System.Threading.ThreadAbortException)
            {

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }





    }
}


