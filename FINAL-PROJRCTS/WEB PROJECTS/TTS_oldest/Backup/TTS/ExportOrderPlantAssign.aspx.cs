using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Collections;

namespace TTS
{
    public partial class ExportOrderPlantAssign : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_plantassign"].ToString() == "True")
                        {
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_plant_unassigned_list", DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvorderlist.DataSource = dtorderlist;
                                gvorderlist.DataBind();

                                DataTable dtPlant = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_plantlist", DataAccess.Return_Type.DataTable);
                                if (dtPlant.Rows.Count > 0)
                                {
                                    ddlPlantList.DataSource = dtPlant;
                                    ddlPlantList.DataTextField = "Plant";
                                    ddlPlantList.DataValueField = "Plant";
                                    ddlPlantList.DataBind();
                                    ddlPlantList.Items.Insert(0, "CHOOSE");
                                }
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
        protected void lnkPlantAssign_Click(object sender, EventArgs e)
        {
            try
            {
                rdbShipmentType.SelectedIndex = -1;
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
                lblCustName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblStausOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnOrderID.Value = ((HiddenField)clickedRow.FindControl("hdnOID")).Value;

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@OID", hdnOrderID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Stock_OrderItemList_PlantAssign_v1", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    gvPlantAssignList.DataSource = dtItemList;
                    gvPlantAssignList.DataBind();

                    gvPlantAssignList.Columns[9].Visible = false;
                    gvPlantAssignList.Columns[11].Visible = false;
                    gvPlantAssignList.Columns[13].Visible = false;
                    gvPlantAssignList.Columns[15].Visible = false;
                    gvPlantAssignList.Columns[17].Visible = false;
                    gvPlantAssignList.Columns[18].Visible = false;
                    gvPlantAssignList.Columns[19].Visible = false;
                    gvPlantAssignList.Columns[20].Visible = false;
                    gvPlantAssignList.Columns[21].Visible = false;
                    hdnStatus.Value = "";
                    foreach (DataRow row1 in dtItemList.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        hdnStatus.Value = "ASSY";
                        gvPlantAssignList.Columns[17].Visible = true;
                        break;
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('div_Subgv');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void rdbShipmentType_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gvPlantAssignList.Columns[9].Visible = false;
                gvPlantAssignList.Columns[11].Visible = false;
                gvPlantAssignList.Columns[13].Visible = false;
                gvPlantAssignList.Columns[15].Visible = false;

                gvPlantAssignList.Columns[18].Visible = false;
                gvPlantAssignList.Columns[19].Visible = false;
                gvPlantAssignList.Columns[20].Visible = false;
                gvPlantAssignList.Columns[21].Visible = false;

                if (rdbShipmentType.SelectedItem.Text == "COMBI")
                {
                    gvPlantAssignList.Columns[9].Visible = true;
                    gvPlantAssignList.Columns[11].Visible = true;
                    gvPlantAssignList.Columns[13].Visible = true;
                    gvPlantAssignList.Columns[15].Visible = true;
                    if (hdnStatus.Value == "ASSY")
                    {
                        gvPlantAssignList.Columns[18].Visible = true;
                        gvPlantAssignList.Columns[19].Visible = true;
                        gvPlantAssignList.Columns[20].Visible = true;
                        gvPlantAssignList.Columns[21].Visible = true;
                    }
                    lblText.Text = "FINAL CONTAINER LOADING FROM";
                }
                else if (rdbShipmentType.SelectedItem.Text == "DIRECT")
                    lblText.Text = "SELECT PLANT";

                ScriptManager.RegisterStartupScript(Page, GetType(), "ShowplantCntrls2", "document.getElementById('tr_Plantselctn').style.display = 'block';", true);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "gotoPreviewDiv('div_Subgv');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnOrderPlantAssign_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvPlantAssignList.HeaderRow != null)
                {
                    int resp = 0;
                    if (rdbShipmentType.SelectedItem.Text == "DIRECT")
                    {
                        SqlParameter[] sp3 = new SqlParameter[3];
                        sp3[0] = new SqlParameter("@Plant", ddlPlantList.SelectedItem.Text);
                        sp3[1] = new SqlParameter("@AssignedUser", Request.Cookies["TTSUser"].Value);
                        sp3[2] = new SqlParameter("@OID", hdnOrderID.Value);
                        resp = daCOTS.ExecuteNonQuery_SP("sp_update_Export_plant_Assign_Direct", sp3);
                    }
                    else if (rdbShipmentType.SelectedItem.Text == "COMBI")
                    {
                        DataTable dtMMN = new DataTable();
                        dtMMN.Columns.Add(new DataColumn("processid", typeof(System.String)));
                        dtMMN.Columns.Add(new DataColumn("ItemQty", typeof(System.Int32)));
                        dtMMN.Columns.Add(new DataColumn("RimItemQty", typeof(System.Int32)));
                        dtMMN.Columns.Add(new DataColumn("AssyRimstatus", typeof(System.Boolean)));
                        dtMMN.Columns.Add(new DataColumn("EdcNo", typeof(System.String)));

                        DataTable dtSLTL = dtMMN.Clone();
                        DataTable dtSITL = dtMMN.Clone();
                        DataTable dtPDK = dtMMN.Clone();
                        foreach (GridViewRow row in gvPlantAssignList.Rows)
                        {
                            Label lblProcessID = (Label)row.FindControl("lblProcessid");
                            Boolean boolAssyStatus = ((Label)row.FindControl("lblAssyStatus")).Text == " (ASSY)" ? true : false;
                            Label lblEdcNo = (Label)row.FindControl("lblEdcNo");

                            TextBox txtMMN = (TextBox)row.FindControl("txtqty_mmn");
                            TextBox txtSLTL = (TextBox)row.FindControl("txtqty_sltl");
                            TextBox txtSITL = (TextBox)row.FindControl("txtqty_sitl");
                            TextBox txtPDK = (TextBox)row.FindControl("txtqty_pdk");

                            if (txtMMN.Text != "0")
                                dtMMN.Rows.Add(lblProcessID.Text, txtMMN.Text, boolAssyStatus ? txtMMN.Text : "0", boolAssyStatus, lblEdcNo.Text);
                            if (txtSLTL.Text != "0")
                                dtSLTL.Rows.Add(lblProcessID.Text, txtSLTL.Text, boolAssyStatus ? txtSLTL.Text : "0", boolAssyStatus, lblEdcNo.Text);
                            if (txtSITL.Text != "0")
                                dtSITL.Rows.Add(lblProcessID.Text, txtSITL.Text, boolAssyStatus ? txtSITL.Text : "0", boolAssyStatus, lblEdcNo.Text);
                            if (txtPDK.Text != "0")
                                dtPDK.Rows.Add(lblProcessID.Text, txtPDK.Text, boolAssyStatus ? txtPDK.Text : "0", boolAssyStatus, lblEdcNo.Text);
                        }

                        DataTable dtPlant = new DataTable();
                        DataColumn pCol = new DataColumn("Plant", typeof(System.String));
                        dtPlant.Columns.Add(pCol);

                        if (dtMMN.Rows.Count > 0)
                            dtPlant.Rows.Add("MMN");
                        if (dtSLTL.Rows.Count > 0)
                            dtPlant.Rows.Add("SLTL");
                        if (dtSITL.Rows.Count > 0)
                            dtPlant.Rows.Add("SITL");
                        if (dtPDK.Rows.Count > 0)
                            dtPlant.Rows.Add("PDK");

                        for (int z = 0; z < dtPlant.Rows.Count; z++)
                        {
                            string strPlant = dtPlant.Rows[z]["Plant"].ToString();
                            DataTable dt = strPlant == "MMN" ? dtMMN : strPlant == "SLTL" ? dtSLTL : strPlant == "SITL" ? dtSITL : dtPDK;

                            SqlParameter[] spMaster = new SqlParameter[] { 
                                new SqlParameter("@OID", hdnOrderID.Value), 
                                new SqlParameter("@Plant", strPlant), 
                                new SqlParameter("@ContainerLoadFrom", ddlPlantList.SelectedItem.Text), 
                                new SqlParameter("@username", Request.Cookies["TTSUser"].Value) 
                            };
                            resp += daCOTS.ExecuteNonQuery_SP("sp_Ins_Exp_OrderMasterDetails_CombiPlant", spMaster);

                            SqlParameter[] spItem = new SqlParameter[] { 
                                new SqlParameter("@Custcode", hdnCustCode.Value), 
                                new SqlParameter("@OrderRefNo", lblStausOrderRefNo.Text), 
                                new SqlParameter("@Plant", strPlant), 
                                new SqlParameter("@OrderItemList_ProcessID_dt", dt),
                                new SqlParameter("@OID", hdnOrderID.Value)
                            };
                            resp += daCOTS.ExecuteNonQuery_SP("sp_Ins_Exp_OrderItemList_CombiPlant", spItem);
                        }
                        SqlParameter[] spDel = new SqlParameter[] { new SqlParameter("@O_ID", hdnOrderID.Value) };
                        daCOTS.ExecuteNonQuery_SP("sp_del_PlantAssign_MasterOrder", spDel);
                    }
                    if (resp > 0)
                        Response.Redirect("exportorderplantassign.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}