using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Text;
using System.Xml;
using System.Web.Services;
using System.IO;

namespace TTS
{
    public partial class Stockorderrevise : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["dom_orderentry"].ToString() == "True" || dtUser.Rows[0]["dom_ordertrack"].ToString() == "True"
                            || dtUser.Rows[0]["exp_orderentry"].ToString() == "True" || dtUser.Rows[0]["exp_ordertrack"].ToString() == "True" || Request.Cookies["TTSUserDepartment"].Value == "EDC" ||
                            Request.Cookies["TTSUserDepartment"].Value == "QC" || Request.Cookies["TTSUserDepartment"].Value == "PPC"))
                        {
                            SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@ordertype", Utilities.Decrypt(Request["fid"].ToString())) };
                            if (Utilities.Decrypt(Request["qid"].ToString()) == "1")
                            {
                                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Incomplete_Stockorder", sp, DataAccess.Return_Type.DataTable);
                                if (dtList.Rows.Count > 0)
                                {
                                    gv_StockOrderList.DataSource = dtList;
                                    gv_StockOrderList.DataBind();
                                }

                                if ((Utilities.Decrypt(Request["fid"].ToString()) == "d"))
                                    lblQuoteHead.Text = "DOMESTIC INCOMPLETE STOCK ORDER";
                                else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                                    lblQuoteHead.Text = "EXPORT INCOMPLETE STOCK ORDER ";
                                else if (Utilities.Decrypt(Request["fid"].ToString()) == "t")
                                    lblQuoteHead.Text = "INCOMPLETE TRIAL ORDER ";

                                gv_StockOrderList.Columns[6].Visible = true;
                                gv_StockOrderList.Columns[7].Visible = false;
                                gv_StockOrderList.Columns[8].Visible = false;
                            }
                            else if (Utilities.Decrypt(Request["qid"].ToString()) == "2")
                            {
                                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Revise_Stockorder", sp, DataAccess.Return_Type.DataTable);
                                if (dtList.Rows.Count > 0)
                                {
                                    gv_StockOrderList.DataSource = dtList;
                                    gv_StockOrderList.DataBind();
                                }

                                if ((Utilities.Decrypt(Request["fid"].ToString()) == "d"))
                                    lblQuoteHead.Text = "DOMESTIC REVISE STOCK ORDER";
                                else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                                    lblQuoteHead.Text = "EXPORT REVISE STOCK ORDER ";
                                else if (Utilities.Decrypt(Request["fid"].ToString()) == "t")
                                    lblQuoteHead.Text = "REVISE TRIAL ORDER ";

                                gv_StockOrderList.Columns[6].Visible = false;
                                gv_StockOrderList.Columns[7].Visible = true;
                                gv_StockOrderList.Columns[8].Visible = false;
                            }
                            else if (Utilities.Decrypt(Request["qid"].ToString()) == "3")
                            {
                                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Track_Stockorder", sp, DataAccess.Return_Type.DataTable);
                                if (dtList.Rows.Count > 0)
                                {
                                    gv_StockOrderList.DataSource = dtList;
                                    gv_StockOrderList.DataBind();
                                }

                                if ((Utilities.Decrypt(Request["fid"].ToString()) == "d"))
                                    lblQuoteHead.Text = "DOMESTIC STOCK ORDER TRACK";
                                else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                                    lblQuoteHead.Text = "EXPORT STOCK ORDER TRACK";
                                else if (Utilities.Decrypt(Request["fid"].ToString()) == "t")
                                    lblQuoteHead.Text = "TRIAL ORDER TRACK";

                                gv_StockOrderList.Columns[6].Visible = false;
                                gv_StockOrderList.Columns[7].Visible = false;
                                gv_StockOrderList.Columns[8].Visible = true;
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
        protected void btnIncompleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((Button)sender).NamingContainer as GridViewRow;
                Label lblOrderRefNo = (Label)row.FindControl("lbl_OrderRefNo");
                HiddenField hdnStockOrderid = (HiddenField)row.FindControl("hdnStockOrderid");
                Response.Redirect("stockorderprepare.aspx?sid=" + Utilities.Encrypt(hdnStockOrderid.Value) + "&fid=" + Request["fid"].ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnReviseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((Button)sender).NamingContainer as GridViewRow;
                lblCustomer.Text = row.Cells[0].Text;
                hdn_CustCode.Value = ((HiddenField)row.FindControl("hdnCustStdcode")).Value;
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StockOrderId", Convert.ToInt32(((HiddenField)row.FindControl("hdnStockOrderid")).Value)) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complete_Stockorderitem_list", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ViewState["StockItemlist"] = dt;
                    gv_Addeditems.DataSource = dt;
                    gv_Addeditems.DataBind();

                    gv_Addeditems.FooterRow.Cells[7].Text = "Total";
                    gv_Addeditems.FooterRow.Cells[8].Text = dt.Compute("Sum(totfwt)", "").ToString();
                    gv_Addeditems.FooterRow.Cells[9].Text = dt.Compute("Sum(itemqty)", "").ToString();

                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    string path2 = serverURL + "Stockorderfiles\\" + hdn_CustCode.Value + "\\" + ((HiddenField)row.FindControl("hdnStockOrderid")).Value + ".pdf";
                    FileInfo file2 = new FileInfo(path2);
                    if (file2.Exists)
                        lnkStockWorkOrder.Text = ((HiddenField)row.FindControl("hdnStockOrderid")).Value + ".pdf";

                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnTrackOrder_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((Button)sender).NamingContainer as GridViewRow;
                lblCustomer.Text = row.Cells[0].Text;
                hdn_CustCode.Value = ((HiddenField)row.FindControl("hdnCustStdcode")).Value;
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StockOrderId", Convert.ToInt32(((HiddenField)row.FindControl("hdnStockOrderid")).Value)) };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_complete_Stockorderitem_list", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ViewState["StockItemlist"] = dt;
                    gv_Addeditems.DataSource = dt;
                    gv_Addeditems.DataBind();

                    gv_Addeditems.FooterRow.Cells[7].Text = "Total";
                    gv_Addeditems.FooterRow.Cells[8].Text = dt.Compute("Sum(totfwt)", "").ToString();
                    gv_Addeditems.FooterRow.Cells[9].Text = dt.Compute("Sum(itemqty)", "").ToString();

                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                    string path2 = serverURL + "Stockorderfiles\\" + hdn_CustCode.Value + "\\" + ((HiddenField)row.FindControl("hdnStockOrderid")).Value + ".pdf";
                    FileInfo file2 = new FileInfo(path2);
                    if (file2.Exists)
                        lnkStockWorkOrder.Text = ((HiddenField)row.FindControl("hdnStockOrderid")).Value + ".pdf";

                    ScriptManager.RegisterStartupScript(Page, GetType(), "showSubGV", "gotoPreviewDiv('div_custorder');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkStockWorkOrder_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string serverUrl = Server.MapPath("~/Stockorderfiles/" + hdn_CustCode.Value + "/").Replace("TTS", "pdfs");
                string path = serverUrl + "/" + lnkTxt.Text;

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}