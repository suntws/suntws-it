using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace TTS
{
    public partial class expscanpdidatashow : System.Web.UI.Page
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
                        if (Request["pid"] != null && Request["pid"].ToString() != "" && Request["mtype"] != null && Request["mtype"].ToString() != "")
                        {
                            SqlParameter[] spScan = new SqlParameter[] { 
                                new SqlParameter("@PID", Request["pid"].ToString()), 
                                new SqlParameter("@mtype", Request["mtype"].ToString()),
                                new SqlParameter("@plant", Request["plant"].ToString()) 
                            };
                            DataSet dsScan = (DataSet)daCOTS.ExecuteReader_SP("sp_sel_Pdi_Scanbarcodelist", spScan, DataAccess.Return_Type.DataSet);
                            if (dsScan.Tables.Count > 0)
                            {
                                if (dsScan.Tables[0].Rows.Count > 0)
                                {
                                    lblCustName.Text = dsScan.Tables[0].Rows[0]["custfullname"].ToString();
                                    lblOrderRefNo.Text = dsScan.Tables[0].Rows[0]["workorderno"].ToString();
                                    lblNoOfRecords.Text = Request["mtype"].ToString().ToUpper() + " COUNT: " + dsScan.Tables[0].Rows[0]["ScanQty"].ToString() +
                                        "/" + dsScan.Tables[0].Rows[0]["orderqty"].ToString();
                                }

                                if (dsScan.Tables[1].Rows.Count > 0)
                                {
                                    gvScanBarcode.DataSource = dsScan.Tables[1];
                                    gvScanBarcode.DataBind();

                                    DataTable dtLoadData = dsScan.Tables[1];
                                    if (dsScan.Tables.Count > 4)
                                    {
                                        if (dsScan.Tables[4] != null && dsScan.Tables[5] != null && dsScan.Tables[4].Rows.Count == 1 && dsScan.Tables[5].Rows.Count > 0)
                                        {
                                            int intScanQty = Convert.ToInt32(dsScan.Tables[0].Rows[0]["ScanQty"].ToString()) +
                                                Convert.ToInt32(dsScan.Tables[4].Rows[0]["ScanQty"].ToString());
                                            int intOrderQty = Convert.ToInt32(dsScan.Tables[0].Rows[0]["orderqty"].ToString()) +
                                                Convert.ToInt32(dsScan.Tables[4].Rows[0]["orderqty"].ToString());

                                            lblNoOfRecords.Text = Request["mtype"].ToString().ToUpper() + " COUNT: " + intScanQty.ToString() + "/" + intOrderQty.ToString();

                                            dtLoadData.Merge(dsScan.Tables[5]);
                                        }
                                    }
                                    ViewState["dtData"] = dtLoadData;
                                    Bind_DDL_GV(dsScan.Tables[1], "");
                                }

                                if (dsScan.Tables[2].Rows.Count > 0)
                                {
                                    gvScannedItemWise.DataSource = dsScan.Tables[2];
                                    gvScannedItemWise.DataBind();
                                }

                                Button3.Visible = false;
                                if (dsScan.Tables[3].Rows.Count > 0)
                                {
                                    gvAssignQty.DataSource = dsScan.Tables[3];
                                    gvAssignQty.DataBind();
                                    Button3.Visible = true;
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JMsg", "Bind_AssignList();", true);
                                }

                                Button1.CssClass = "Clicked";
                                Button2.CssClass = "Initial";
                                Button3.CssClass = "Initial";
                                MultiView1.ActiveViewIndex = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void Tab_Click(object sender, EventArgs e)
        {
            try
            {
                Button1.CssClass = "Initial";
                Button2.CssClass = "Initial";
                Button3.CssClass = "Initial";
                Button lnkTxt = sender as Button;
                if (lnkTxt.Text == "BARCODE WISE")
                {
                    Button1.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 0;
                }
                else if (lnkTxt.Text == "ITEM QTY WISE")
                {
                    Button2.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 1;
                }
                else if (lnkTxt.Text == "ASSIGN QTY WISE")
                {
                    Button3.CssClass = "Clicked";
                    MultiView1.ActiveViewIndex = 2;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvScanBarcode_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //DataTable dtScan = ViewState["dtData"] as DataTable;

                //gvScanBarcode.DataSource = dtScan;
                //gvScanBarcode.DataBind();

                Make_Where_Condition("");

                gvScanBarcode.PageIndex = e.NewPageIndex;
                gvScanBarcode.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlPlatform_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("PLATFORM");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlTyreSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("TYRESIZE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlRimSize_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("RIMSIZE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlTyretype_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("TYRETYPE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlBrand_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("BRAND");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddl_Sidewall_IndexChange(object sender, EventArgs e)
        {
            try
            {
                Make_Where_Condition("SIDEWALL");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Make_Where_Condition(string strField)
        {
            try
            {
                string strCond = "";
                if (ddlPlatform.SelectedItem.Text != "ALL")
                {
                    if (strCond.Length == 0)
                        strCond += "PLATFORM='" + ddlPlatform.SelectedItem.Text + "'";
                    else
                        strCond += " and PLATFORM='" + ddlPlatform.SelectedItem.Text + "'";
                    strField = "PLATFORM";
                }
                if (ddlTyreSize.SelectedItem.Text != "ALL")
                {
                    if (strCond.Length == 0)
                        strCond += "[TYRE SIZE]='" + ddlTyreSize.SelectedItem.Text + "'";
                    else
                        strCond += " and [TYRE SIZE]='" + ddlTyreSize.SelectedItem.Text + "'";
                    strField = "TYRESIZE";
                }
                if (ddlRimSize.SelectedItem.Text != "ALL")
                {
                    if (strCond.Length == 0)
                        strCond += "RIM='" + ddlRimSize.SelectedItem.Text + "'";
                    else
                        strCond += " and RIM='" + ddlRimSize.SelectedItem.Text + "'";
                    strField = "RIMSIZE";
                }
                if (ddlTyretype.SelectedItem.Text != "ALL")
                {
                    if (strCond.Length == 0)
                        strCond += "TYPE='" + ddlTyretype.SelectedItem.Text + "'";
                    else
                        strCond += " and TYPE='" + ddlTyretype.SelectedItem.Text + "'";
                    strField = "TYRETYPE";
                }
                if (ddlBrand.SelectedItem.Text != "ALL")
                {
                    if (strCond.Length == 0)
                        strCond += "BRAND='" + ddlBrand.SelectedItem.Text + "'";
                    else
                        strCond += " and BRAND='" + ddlBrand.SelectedItem.Text + "'";
                    strField = "BRAND";
                }
                if (ddl_Sidewall.SelectedItem.Text != "ALL")
                {
                    if (strCond.Length == 0)
                        strCond += "SIDEWALL='" + ddl_Sidewall.SelectedItem.Text + "'";
                    else
                        strCond += " and SIDEWALL='" + ddl_Sidewall.SelectedItem.Text + "'";
                    strField = "SIDEWALL";
                }
                DataView dtView = new DataView((DataTable)ViewState["dtData"], strCond, "PROCESSID", DataViewRowState.CurrentRows);
                if (dtView.Count > 0)
                {
                    Bind_DDL_GV(dtView.ToTable(), strField);
                }
                else
                {
                    gvScanBarcode.DataSource = dtView;
                    gvScanBarcode.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_DDL_GV(DataTable dtGv, string strCont)
        {
            try
            {
                DataView dtView = new DataView(dtGv);
                switch (strCont)
                {
                    case "PLATFORM":
                        dtView.Sort = "TYRE SIZE";
                        DataTable distinctTyreSize = dtView.ToTable(true, "TYRE SIZE");
                        Utilities.ddl_Binding(ddlTyreSize, distinctTyreSize, "TYRE SIZE", "TYRE SIZE", "");
                        ddlTyreSize.Items.Insert(0, "ALL");
                        dtView.Sort = "RIM";
                        DataTable distinctRimSize = dtView.ToTable(true, "RIM");
                        Utilities.ddl_Binding(ddlRimSize, distinctRimSize, "RIM", "RIM", "");
                        ddlRimSize.Items.Insert(0, "ALL");
                        dtView.Sort = "TYPE";
                        DataTable distinctTyretype = dtView.ToTable(true, "TYPE");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "TYPE", "TYPE", "");
                        ddlTyretype.Items.Insert(0, "ALL");
                        dtView.Sort = "BRAND";
                        DataTable distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");
                        dtView.Sort = "SIDEWALL";
                        DataTable distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "SIDEWALL", "SIDEWALL", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                    case "TYRESIZE":
                        dtView.Sort = "RIM";
                        distinctRimSize = dtView.ToTable(true, "RIM");
                        Utilities.ddl_Binding(ddlRimSize, distinctRimSize, "RIM", "RIM", "");
                        ddlRimSize.Items.Insert(0, "ALL");
                        dtView.Sort = "TYPE";
                        distinctTyretype = dtView.ToTable(true, "TYPE");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "TYPE", "TYPE", "");
                        ddlTyretype.Items.Insert(0, "ALL");
                        dtView.Sort = "BRAND";
                        distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");
                        dtView.Sort = "SIDEWALL";
                        distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "SIDEWALL", "SIDEWALL", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                    case "RIMSIZE":
                        distinctTyretype = dtView.ToTable(true, "TYPE");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "TYPE", "TYPE", "");
                        ddlTyretype.Items.Insert(0, "ALL");


                        dtView.Sort = "BRAND";
                        distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");

                        dtView.Sort = "SIDEWALL";
                        distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "SIDEWALL", "SIDEWALL", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;

                    case "TYRETYPE":
                        dtView.Sort = "BRAND";
                        distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");

                        dtView.Sort = "SIDEWALL";
                        distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;

                    case "BRAND":
                        dtView.Sort = "sidewall";
                        distinctSidewall = dtView.ToTable(true, "sidewall");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "sidewall", "sidewall", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                    case "SIDEWALL":
                        break;
                    default:
                        dtView.Sort = "PLATFORM";
                        DataTable distinctType = dtView.ToTable(true, "PLATFORM");
                        Utilities.ddl_Binding(ddlPlatform, distinctType, "PLATFORM", "PLATFORM", "");
                        ddlPlatform.Items.Insert(0, "ALL");

                        dtView.Sort = "TYRE SIZE";
                        distinctTyreSize = dtView.ToTable(true, "TYRE SIZE");
                        Utilities.ddl_Binding(ddlTyreSize, distinctTyreSize, "TYRE SIZE", "TYRE SIZE", "");
                        ddlTyreSize.Items.Insert(0, "ALL");

                        dtView.Sort = "RIM";
                        distinctRimSize = dtView.ToTable(true, "RIM");
                        Utilities.ddl_Binding(ddlRimSize, distinctRimSize, "RIM", "RIM", "");
                        ddlRimSize.Items.Insert(0, "ALL");

                        dtView.Sort = "TYPE";
                        distinctTyretype = dtView.ToTable(true, "TYPE");
                        Utilities.ddl_Binding(ddlTyretype, distinctTyretype, "TYPE", "TYPE", "");
                        ddlTyretype.Items.Insert(0, "ALL");


                        dtView.Sort = "BRAND";
                        distinctBrand = dtView.ToTable(true, "BRAND");
                        Utilities.ddl_Binding(ddlBrand, distinctBrand, "BRAND", "BRAND", "");
                        ddlBrand.Items.Insert(0, "ALL");

                        dtView.Sort = "SIDEWALL";
                        distinctSidewall = dtView.ToTable(true, "SIDEWALL");
                        Utilities.ddl_Binding(ddl_Sidewall, distinctSidewall, "SIDEWALL", "SIDEWALL", "");
                        ddl_Sidewall.Items.Insert(0, "ALL");
                        break;
                }
                dtView = new DataView(dtGv, "", "PROCESSID DESC", DataViewRowState.CurrentRows);
                if (dtView.Count > 0)
                {
                    gvScanBarcode.DataSource = dtView;
                    gvScanBarcode.DataBind();
                }
                else
                {
                    gvScanBarcode.DataSource = null;
                    gvScanBarcode.DataBind();
                }
            }

            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}