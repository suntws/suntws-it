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
    public partial class earmarkstencillist : System.Web.UI.Page
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
                            (dtUser.Rows[0]["dom_ordertrack"].ToString() == "True" || dtUser.Rows[0]["exp_ordertrack"].ToString() == "True"))
                        {
                            if (Request["disid"] != null)
                            {
                                if (Request["disid"].ToString() == "dom")
                                    lblPageHead.Text = "DOMESTIC ";
                                else if (Request["disid"].ToString() == "exp")
                                    lblPageHead.Text = "INTERNATIONAL ";
                                DataTable dt = new DataTable();
                                SqlParameter[] sp = new SqlParameter[1];
                                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                                    sp = new SqlParameter[2];
                                sp[0] = new SqlParameter("@OrderType", Request["disid"].ToString().ToUpper());
                                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                                {
                                    sp[1] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmarkedlist_plant_userwise", sp, DataAccess.Return_Type.DataTable);
                                }
                                else
                                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmarkedlist_plant", sp, DataAccess.Return_Type.DataTable);
                                Utilities.ddl_Binding(ddlplant, dt, "plant", "plant", "ALL");
                                if (dt.Rows.Count > 0)
                                {
                                    ddl_bindyear();
                                    ddl_bindMonth();
                                    Bind_Earmarkedgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
                                }
                                else
                                    lblErrMsg.Text = "No Records";
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ddl_bindyear()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[2];
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                    sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@ClaimType", Request["disid"].ToString().ToUpper());
                sp1[1] = new SqlParameter("@Plant", ddlplant.SelectedItem.Text);
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                {
                    sp1[2] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmarkedlist_year_userwise", sp1, DataAccess.Return_Type.DataTable);
                }
                else
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmarkedlist_year", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlYear, dt, "earmarkyear", "earmarkyear", "");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ddl_bindMonth()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] sp1 = new SqlParameter[3];
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                    sp1 = new SqlParameter[4];
                sp1[0] = new SqlParameter("@ClaimType", Request["disid"].ToString().ToUpper());
                sp1[1] = new SqlParameter("@Plant", ddlplant.SelectedItem.Text);
                sp1[2] = new SqlParameter("@year", Convert.ToInt32(ddlYear.SelectedItem.Text));
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                {
                    sp1[3] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmarkedlist_Month_userwise", sp1, DataAccess.Return_Type.DataTable);
                }
                else
                    dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmarkedlist_Month", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlMonth, dt, "Earmarkedmonth", "monthid", "");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindMonth();
            Bind_Earmarkedgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_Earmarkedgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindyear();
            ddl_bindMonth();
            Bind_Earmarkedgvlist(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        private void Bind_Earmarkedgvlist(string plant, string year, string month)
        {
            try
            {
                gvEarmarkedorderlist.DataSource = null;
                gvEarmarkedorderlist.DataBind();
                DataTable dtorderlist = new DataTable();
                if (Request.Cookies["TTSUserType"].Value.ToLower() != "admin" && Request.Cookies["TTSUserType"].Value.ToLower() != "support")
                {
                    SqlParameter[] sp1 = new SqlParameter[5];
                    sp1[0] = new SqlParameter("@Username", Request.Cookies["TTSUser"].Value);
                    sp1[1] = new SqlParameter("@Plant", plant);
                    sp1[2] = new SqlParameter("@year", Convert.ToInt32(year));
                    sp1[3] = new SqlParameter("@month", Convert.ToInt32(month));
                    sp1[4] = new SqlParameter("@OrderType", Request["disid"].ToString().ToUpper());
                    dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmarked_orders_Userwise_PlantWise", sp1, DataAccess.Return_Type.DataTable);
                }
                else
                {
                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@Plant", plant);
                    sp1[1] = new SqlParameter("@year", Convert.ToInt32(year));
                    sp1[2] = new SqlParameter("@month", Convert.ToInt32(month));
                    sp1[3] = new SqlParameter("@OrderType", Request["disid"].ToString().ToUpper());
                    dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_earmarked_orders_PlantWise", sp1, DataAccess.Return_Type.DataTable);
                }
                Bind_GvList(dtorderlist);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_GvList(DataTable dtList)
        {
            try
            {
                lblErrMsg.Text = "";
                if (dtList.Rows.Count > 0)
                {
                    gvEarmarkedorderlist.DataSource = dtList;
                    gvEarmarkedorderlist.DataBind();
                }
                else
                    lblErrMsg.Text = "No records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkEarmarkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                Build_SelectEarmarkedOrder(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SelectEarmarkedOrder(GridViewRow row)
        {
            try
            {
                lblStausOrderRefNo.Text = ((Label)row.FindControl("lblOrderRefNo")).Text;
                hdnCustCode.Value = ((HiddenField)row.FindControl("hdnStatusCustCode")).Value;
                lblCustName.Text = ((Label)row.FindControl("lblStatusCustName")).Text;
                hdnOID.Value = ((HiddenField)row.FindControl("hdnOrderID")).Value;

                Bind_OrderItem();
                //SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                //lnkEarmarkExcel.Text = (string)daCOTS.ExecuteScalar_SP("sp_sel_earmark_excel_file", sp1);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_OrderItem()
        {
            try
            {
                DataTable dtItemList = DomesticScots.complete_orderitem_list_V1(Convert.ToInt32(hdnOID.Value));
                if (dtItemList.Rows.Count > 0)
                {
                    gvOrderItemList.DataSource = dtItemList;
                    gvOrderItemList.DataBind();

                    gvOrderItemList.FooterRow.Cells[6].Text = "TOTAL";
                    gvOrderItemList.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                    object sumQty = dtItemList.Compute("Sum(itemqty)", "");
                    gvOrderItemList.FooterRow.Cells[8].Text = sumQty.ToString();

                    gvOrderItemList.Columns[10].Visible = false; //RIM QTY
                    gvOrderItemList.Columns[11].Visible = false; //RIM BASIC PRICE
                    gvOrderItemList.Columns[12].Visible = false; //RIM FWT
                    foreach (DataRow row in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvOrderItemList.Columns[10].Visible = true;
                        gvOrderItemList.Columns[11].Visible = true;
                        gvOrderItemList.Columns[12].Visible = true;
                        break;
                    }
                    DataTable dtUser = (DataTable)Session["dtuserlevel"];
                    if (dtUser != null && (dtUser.Rows[0]["dom_proforma"].ToString() != "True" && dtUser.Rows[0]["dom_paymentconfirm"].ToString() != "True" &&
                        dtUser.Rows[0]["dom_invoice"].ToString() != "True" && dtUser.Rows[0]["exp_proforma"].ToString() != "True"))
                    {
                        gvOrderItemList.Columns[7].Visible = false;
                        gvOrderItemList.Columns[11].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkEarmarkExcel_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((LinkButton)sender).NamingContainer as GridViewRow;
                string path = Server.MapPath("~/stockearmark/" + ((HiddenField)row.FindControl("hdnStatusCustCode")).Value +
                    "/").Replace("TTS", "pdfs") + ((HiddenField)row.FindControl("hdnEarmarkfile")).Value;

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + ((HiddenField)row.FindControl("hdnEarmarkfile")).Value);
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/x-msexcel";
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvEarmarkedorderlist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}