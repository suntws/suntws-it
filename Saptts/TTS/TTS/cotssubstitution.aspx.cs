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
    public partial class cotssubstitution : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["ot_qcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_qcpdk"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_ppcmmn"].ToString() == "True" || dtUser.Rows[0]["ot_ppcpdk"].ToString() == "True" ||
                            dtUser.Rows[0]["ot_ppc_sltl"].ToString() == "True" || dtUser.Rows[0]["ot_ppc_sitl"].ToString() == "True"))
                        {
                            lblPageHead.Text = (Utilities.Decrypt(Request["pid"].ToString()).ToUpper() + (Utilities.Decrypt(Request["fid"].ToString()) == "e" ?
                                    " - EXPORT " : " - DOMESTIC ")) + "ORDER SUBSTITUTION / LIQUIDATION";

                            SqlParameter[] sp1 = new SqlParameter[] { 
                                new SqlParameter("@Plant", Utilities.Decrypt(Request["pid"].ToString())), 
                                new SqlParameter("@QString", Utilities.Decrypt(Request["fid"].ToString())) 
                            };
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_prodcution_orders", sp1, DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvReviseOrderList.DataSource = dtorderlist;
                                gvReviseOrderList.DataBind();
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
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkReviseBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                ItemRowClicked(clickedRow);
                hdnselectedrow.Value = Convert.ToString(clickedRow.RowIndex);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void ItemRowClicked(GridViewRow rowClick)
        {
            try
            {
                lblStausOrderRefNo.Text = ((Label)rowClick.FindControl("lblOrderRefNo")).Text;
                lblCustName.Text = ((Label)rowClick.FindControl("lblStatusCustName")).Text;
                lblCurrentStatus.Text = ((Label)rowClick.FindControl("lblStatusText")).Text;
                hdnCotsCustCode.Value = ((HiddenField)rowClick.FindControl("hdnOrderCustCode")).Value;
                hdnOID.Value = ((HiddenField)rowClick.FindControl("hdnOrderID")).Value;

                Bind_OrderItem();
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
                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_orderitemlist_substitution", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvReviseItemList.DataSource = dtItemList;
                    gvReviseItemList.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divStatusChange');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkCommercial_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((LinkButton)sender).NamingContainer as GridViewRow;
                Label lblConfig = (Label)row.FindControl("lblConfig");
                Label lblSize = (Label)row.FindControl("lblSize");
                Label lblRim = (Label)row.FindControl("lblRim");
                Label lblType = (Label)row.FindControl("lblType");
                Label lblBrand = (Label)row.FindControl("lblBrand");
                Label lblSidewall = (Label)row.FindControl("lblSidewall");
                HiddenField hdnProcessid = (HiddenField)row.FindControl("hdnProcessid");

                lblSubPlatform.Text = lblConfig.Text;
                lblSubSize.Text = lblSize.Text;
                lblSubRim.Text = lblRim.Text;
                lblSubType.Text = lblType.Text;
                lblSubBrand.Text = lblBrand.Text;
                lblSubSidewall.Text = lblSidewall.Text;
                lblSubProcessID.Text = hdnProcessid.Value;

                lblSSize.Text = lblSize.Text;
                lblSRim.Text = lblRim.Text;
                lblSSize1.Text = lblSize.Text;
                lblSRim1.Text = lblRim.Text;
                DataTable dt = (DataTable)daCOTS.ExecuteReader("select distinct tyretype from ttsdb.dbo.ProcessID_Details where TyreSize='" + lblSubSize.Text + "' and TyreRim='" +
                    lblSubRim.Text + "' and (TyreType='" + lblSubType.Text + "' or TyreType in (select UpgradeType from TypeWise_UpgradeList where MasterType='"+
                    lblSubType.Text +"' and StockMethod='GSA' and UpgradeStatus=1))",
                    DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlSType.DataSource = dt;
                    ddlSType.DataTextField = "tyretype";
                    ddlSType.DataValueField = "tyretype";
                    ddlSType.DataBind();
                    ListItem lstType = ddlSType.Items.FindByText(lblType.Text);
                    if (lstType != null)
                        lstType.Selected = true;

                    ddlSType1.DataSource = dt;
                    ddlSType1.DataTextField = "tyretype";
                    ddlSType1.DataValueField = "tyretype";
                    ddlSType1.DataBind();
                    lstType = ddlSType1.Items.FindByText(lblType.Text);
                    if (lstType != null)
                        lstType.Selected = true;
                }
                else
                {
                    ddlSType.DataSource = "";
                    ddlSType.DataBind();

                    ddlSType1.DataSource = "";
                    ddlSType1.DataBind();
                }
                ddlSBrand.Visible = true;
                ddlSPlatform.Visible = true;
                ddlSSidewall.Visible = true;

                ddlSBrand1.Visible = true;
                ddlSPlatform1.Visible = true;
                ddlSSidewall1.Visible = true;
                dt = (DataTable)daCOTS.ExecuteReader("select distinct brand from ttsdb.dbo.ProcessID_Details where TyreSize='" + lblSubSize.Text + "' and TyreRim='" + lblSubRim.Text +
                    "' and (TyreType='" + lblSubType.Text + "' or TyreType in (select UpgradeType from TypeWise_UpgradeList where MasterType='" +
                    lblSubType.Text + "' and StockMethod='GSA' and UpgradeStatus=1))",
                    DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlSBrand.DataSource = dt;
                    ddlSBrand.DataTextField = "brand";
                    ddlSBrand.DataValueField = "brand";
                    ddlSBrand.DataBind();
                    ListItem lstBrand = ddlSBrand.Items.FindByText(lblBrand.Text);
                    if (lstBrand != null)
                        lstBrand.Selected = true;

                    ddlSBrand1.DataSource = dt;
                    ddlSBrand1.DataTextField = "brand";
                    ddlSBrand1.DataValueField = "brand";
                    ddlSBrand1.DataBind();
                    lstBrand = ddlSBrand1.Items.FindByText(lblBrand.Text);
                    if (lstBrand != null)
                        lstBrand.Selected = true;
                }
                else
                {
                    ddlSBrand.DataSource = "";
                    ddlSBrand.DataBind();

                    ddlSBrand1.DataSource = "";
                    ddlSBrand1.DataBind();
                }
                dt = (DataTable)daCOTS.ExecuteReader("select distinct config from ttsdb.dbo.ProcessID_Details where TyreSize='" + lblSubSize.Text + "' and TyreRim='" + lblSubRim.Text +
                    "' and (TyreType='" + lblSubType.Text + "' or TyreType in (select UpgradeType from TypeWise_UpgradeList where MasterType='" +
                    lblSubType.Text + "' and StockMethod='GSA' and UpgradeStatus=1))",
                    DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlSPlatform.DataSource = dt;
                    ddlSPlatform.DataTextField = "config";
                    ddlSPlatform.DataValueField = "config";
                    ddlSPlatform.DataBind();
                    ListItem lstPlatform = ddlSPlatform.Items.FindByText(lblSubPlatform.Text);
                    if (lstPlatform != null)
                        lstPlatform.Selected = true;

                    ddlSPlatform1.DataSource = dt;
                    ddlSPlatform1.DataTextField = "config";
                    ddlSPlatform1.DataValueField = "config";
                    ddlSPlatform1.DataBind();
                    lstPlatform = ddlSPlatform1.Items.FindByText(lblSubPlatform.Text);
                    if (lstPlatform != null)
                        lstPlatform.Selected = true;
                }
                else
                {
                    ddlSPlatform.DataSource = "";
                    ddlSPlatform.DataBind();

                    ddlSPlatform1.DataSource = "";
                    ddlSPlatform1.DataBind();
                }
                dt = (DataTable)daCOTS.ExecuteReader("select distinct sidewall from ttsdb.dbo.ProcessID_Details where TyreSize='" + lblSubSize.Text + "' and TyreRim='" +
                    lblSubRim.Text + "' and (TyreType='" + lblSubType.Text + "' or TyreType in (select UpgradeType from TypeWise_UpgradeList where MasterType='" +
                    lblSubType.Text + "' and StockMethod='GSA' and UpgradeStatus=1))",
                    DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlSSidewall.DataSource = dt;
                    ddlSSidewall.DataTextField = "sidewall";
                    ddlSSidewall.DataValueField = "sidewall";
                    ddlSSidewall.DataBind();
                    ListItem lstSidewall = ddlSSidewall.Items.FindByText(lblSubSidewall.Text);
                    if (lstSidewall != null)
                        lstSidewall.Selected = true;

                    ddlSSidewall1.DataSource = dt;
                    ddlSSidewall1.DataTextField = "sidewall";
                    ddlSSidewall1.DataValueField = "sidewall";
                    ddlSSidewall1.DataBind();
                    lstSidewall = ddlSSidewall1.Items.FindByText(lblSubSidewall.Text);
                    if (lstSidewall != null)
                        lstSidewall.Selected = true;
                }
                else
                {
                    ddlSSidewall.DataSource = "";
                    ddlSSidewall.DataBind();

                    ddlSSidewall1.DataSource = "";
                    ddlSSidewall1.DataBind();
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "Jgoto", "gotoNewDiv('divSubstitution');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnCommercial_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnAlterProcessID.Value != "" || hdnAlterProcessID1.Value != "")
                {
                    int resp = 0;
                    if (hdnAlterProcessID.Value != "")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@O_ID", hdnOID.Value), 
                            new SqlParameter("@mainprocessid", lblSubProcessID.Text), 
                            new SqlParameter("@sType", ddlSType.SelectedItem.Text), 
                            new SqlParameter("@sBrand", ddlSBrand.SelectedItem.Text), 
                            new SqlParameter("@sProcessID", hdnAlterProcessID.Value), 
                            new SqlParameter("sPlatform", ddlSPlatform.SelectedItem.Text), 
                            new SqlParameter("@sSidewall", ddlSSidewall.SelectedItem.Text) 
                        };
                        resp = daCOTS.ExecuteNonQuery_SP("sp_update_item_commercialdecision", sp1);
                    }
                    if (hdnAlterProcessID1.Value != "")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@O_ID", hdnOID.Value), 
                            new SqlParameter("@mainprocessid", lblSubProcessID.Text), 
                            new SqlParameter("@sType", ddlSType1.SelectedItem.Text), 
                            new SqlParameter("@sBrand", ddlSBrand1.SelectedItem.Text), 
                            new SqlParameter("@sProcessID", hdnAlterProcessID1.Value), 
                            new SqlParameter("sPlatform", ddlSPlatform1.SelectedItem.Text), 
                            new SqlParameter("@sSidewall", ddlSSidewall1.SelectedItem.Text) 
                        };
                        resp = daCOTS.ExecuteNonQuery_SP("sp_update_item_commercialdecision_1", sp1);
                    }
                    if (resp > 0)
                    {
                        SqlParameter[] sp2 = new SqlParameter[] { 
                            new SqlParameter("@O_ID", hdnOID.Value), 
                            new SqlParameter("@Preview", lblSubProcessID.Text), 
                            new SqlParameter("@Revise", hdnAlterProcessID.Value + "~" + hdnAlterProcessID1.Value), 
                            new SqlParameter("@ReviseType", "Substitute/Liquidate"), 
                            new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value), 
                            new SqlParameter("@ProcessID", lblSubProcessID.Text) 
                        };
                        daCOTS.ExecuteNonQuery_SP("sp_ins_OrderRevisedHistory", sp2);
                        gvReviseOrderList.SelectedIndex = Convert.ToInt32(hdnselectedrow.Value);
                        ItemRowClicked(gvReviseOrderList.SelectedRow);
                    }
                    gvReviseOrderList.SelectedIndex = Convert.ToInt32(hdnselectedrow.Value);
                    ItemRowClicked(gvReviseOrderList.SelectedRow);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlSType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvReviseItemList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}