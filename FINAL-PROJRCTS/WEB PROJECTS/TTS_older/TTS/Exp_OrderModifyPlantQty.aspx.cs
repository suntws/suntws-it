using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.Data;
namespace TTS
{
    public partial class Exp_OrderModifyPlantQty : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["exp_ordersplit"].ToString() == "True")
                        {
                            DataTable dtorderlist = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_export_split_itemqty_change_orders", DataAccess.Return_Type.DataTable);
                            if (dtorderlist.Rows.Count > 0)
                            {
                                gvorderlist.DataSource = dtorderlist;
                                gvorderlist.DataBind();
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
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnCustCode.Value = ((HiddenField)clickedRow.FindControl("hdnStatusCustCode")).Value;
                lblCustName.Text = ((Label)clickedRow.FindControl("lblStatusCustName")).Text;
                lblStausOrderRefNo.Text = ((Label)clickedRow.FindControl("lblOrderRefNo")).Text;
                hdnOID.Value = ((HiddenField)clickedRow.FindControl("hdnOrderID")).Value;

                SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@O_ID", hdnOID.Value) };
                DataTable dtItemList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_exp_itemqty_revise", sp1, DataAccess.Return_Type.DataTable);
                if (dtItemList.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = dtItemList.Clone();
                    dt.Columns.Remove("itemPlant");
                    dt.Columns.Add("MMN_TYRE");
                    dt.Columns.Add("SLTL_TYRE");
                    dt.Columns.Add("SITL_TYRE");
                    dt.Columns.Add("PDK_TYRE");
                    dt.Columns.Add("MMN_RIM");
                    dt.Columns.Add("SLTL_RIM");
                    dt.Columns.Add("SITL_RIM");
                    dt.Columns.Add("PDK_RIM");
                    dt.ImportRow(dtItemList.Rows[0]);
                    foreach (DataRow row in dtItemList.Rows)
                    {
                        dynamic duplicate = null;
                        foreach (DataRow row1 in dt.Rows)
                        {
                            if (row["processid"].ToString() == row1["processid"].ToString() && row["AssyRimstatus"].ToString() == row1["AssyRimstatus"].ToString() &&
                                row["EdcNo"].ToString() == row1["EdcNo"].ToString())
                            {
                                row1[row["itemPlant"].ToString() + "_TYRE"] = row["itemqty"].ToString();
                                row1[row["itemPlant"].ToString() + "_RIM"] = row["Rimitemqty"].ToString();
                                duplicate = true;
                                break;
                            }
                            else
                                duplicate = false;
                        }
                        if (!duplicate)
                        {
                            dt.ImportRow(row);
                            foreach (DataRow dr in dt.Select("processid='" + row["processid"].ToString() + "' and AssyRimstatus='" + row["AssyRimstatus"].ToString()
                                + "' and EdcNo='" + row["EdcNo"].ToString() + "'"))
                            {
                                dr[row["itemPlant"].ToString() + "_TYRE"] = row["itemqty"].ToString();
                                dr[row["itemPlant"].ToString() + "_RIM"] = row["Rimitemqty"].ToString();
                            }
                        }
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["MMN_TYRE"] = dr["MMN_TYRE"].ToString() == "" ? "0" : dr["MMN_TYRE"].ToString();
                        dr["SLTL_TYRE"] = dr["SLTL_TYRE"].ToString() == "" ? "0" : dr["SLTL_TYRE"].ToString();
                        dr["SITL_TYRE"] = dr["SITL_TYRE"].ToString() == "" ? "0" : dr["SITL_TYRE"].ToString();
                        dr["PDK_TYRE"] = dr["PDK_TYRE"].ToString() == "" ? "0" : dr["PDK_TYRE"].ToString();
                        dr["MMN_RIM"] = dr["MMN_RIM"].ToString() == "" ? "0" : dr["MMN_RIM"].ToString();
                        dr["SLTL_RIM"] = dr["SLTL_RIM"].ToString() == "" ? "0" : dr["SLTL_RIM"].ToString();
                        dr["SITL_RIM"] = dr["SITL_RIM"].ToString() == "" ? "0" : dr["SITL_RIM"].ToString();
                        dr["PDK_RIM"] = dr["PDK_RIM"].ToString() == "" ? "0" : dr["PDK_RIM"].ToString();
                        dr["itemqty"] = Convert.ToString(Convert.ToInt32(dr["MMN_TYRE"].ToString()) + Convert.ToInt32(dr["SLTL_TYRE"].ToString()) +
                            Convert.ToInt32(dr["SITL_TYRE"].ToString()) + Convert.ToInt32(dr["PDK_TYRE"].ToString()));
                        dr["Rimitemqty"] = Convert.ToString(Convert.ToInt32(dr["MMN_RIM"].ToString()) + Convert.ToInt32(dr["SLTL_RIM"].ToString()) +
                            Convert.ToInt32(dr["SITL_RIM"].ToString()) + Convert.ToInt32(dr["PDK_RIM"].ToString()));
                    }
                    gvPlantAssignList.DataSource = dt;
                    gvPlantAssignList.DataBind();

                    gvPlantAssignList.Columns[13].Visible = false;
                    gvPlantAssignList.Columns[14].Visible = false;
                    gvPlantAssignList.Columns[15].Visible = false;
                    gvPlantAssignList.Columns[16].Visible = false;
                    gvPlantAssignList.Columns[17].Visible = false;
                    foreach (DataRow row in dt.Select("AssyRimstatus='True' or category in ('SPLIT RIMS','POB WHEEL')"))
                    {
                        gvPlantAssignList.Columns[13].Visible = true;
                        gvPlantAssignList.Columns[14].Visible = true;
                        gvPlantAssignList.Columns[15].Visible = true;
                        gvPlantAssignList.Columns[16].Visible = true;
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
        protected void btnPlantQtyReAssign_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtMMN = new DataTable();
                dtMMN.Columns.Add("processid", typeof(System.String));
                dtMMN.Columns.Add("ItemQty", typeof(System.Int32));
                dtMMN.Columns.Add("RimItemQty", typeof(System.Int32));
                dtMMN.Columns.Add("AssyRimstatus", typeof(System.Boolean));
                dtMMN.Columns.Add("EdcNo", typeof(System.String));
                DataTable dtSLTL = dtMMN.Clone();
                DataTable dtSITL = dtMMN.Clone();
                DataTable dtPDK = dtMMN.Clone();
                foreach (GridViewRow row in gvPlantAssignList.Rows)
                {
                    Label lblProcessID = (Label)row.FindControl("lblProcessid");
                    Label lblEdcNo = (Label)row.FindControl("lblEdcNo");
                    Boolean boolAssyStatus = ((Label)row.FindControl("lblAssyStatus")).Text == " (ASSY)" ? true : false;
                    TextBox txtMMN = (TextBox)row.FindControl("txtqty_mmn");
                    TextBox txtSLTL = (TextBox)row.FindControl("txtqty_sltl");
                    TextBox txtSITL = (TextBox)row.FindControl("txtqty_sitl");
                    TextBox txtPDK = (TextBox)row.FindControl("txtqty_pdk");
                    dtMMN.Rows.Add(lblProcessID.Text, txtMMN.Text, boolAssyStatus ? txtMMN.Text : "0", boolAssyStatus, lblEdcNo.Text);
                    dtSLTL.Rows.Add(lblProcessID.Text, txtSLTL.Text, boolAssyStatus ? txtSLTL.Text : "0", boolAssyStatus, lblEdcNo.Text);
                    dtSITL.Rows.Add(lblProcessID.Text, txtSITL.Text, boolAssyStatus ? txtSITL.Text : "0", boolAssyStatus, lblEdcNo.Text);
                    dtPDK.Rows.Add(lblProcessID.Text, txtPDK.Text, boolAssyStatus ? txtPDK.Text : "0", boolAssyStatus, lblEdcNo.Text);
                }
                DataTable dtPlant = new DataTable();
                dtPlant.Columns.Add("Plant", typeof(System.String));
                if (dtMMN.Rows.Count > 0)
                    dtPlant.Rows.Add("MMN");
                if (dtSLTL.Rows.Count > 0)
                    dtPlant.Rows.Add("SLTL");
                if (dtSITL.Rows.Count > 0)
                    dtPlant.Rows.Add("SITL");
                if (dtPDK.Rows.Count > 0)
                    dtPlant.Rows.Add("PDK");
                int resp = 0;
                for (int z = 0; z < dtPlant.Rows.Count; z++)
                {
                    string strPlant = dtPlant.Rows[z]["Plant"].ToString();
                    DataTable dt = strPlant == "MMN" ? dtMMN : strPlant == "SLTL" ? dtSLTL : strPlant == "SITL" ? dtSITL : dtPDK;
                    SqlParameter[] sp2 = new SqlParameter[]{
                        new SqlParameter("@OrderItemList_ProcessID_dt", dt),
                        new SqlParameter("@username", Request.Cookies["TTSUser"].Value),
                        new SqlParameter("@O_ID", hdnOID.Value),
                        new SqlParameter("@Plant", strPlant)
                    };
                    resp += daCOTS.ExecuteNonQuery_SP("sp_update_Export_plantQty_ordermaster_Combi", sp2);
                }
                if (resp > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@CustCode", hdnCustCode.Value), new SqlParameter("@OrderRefNo", lblStausOrderRefNo.Text) };
                    daCOTS.ExecuteNonQuery_SP("sp_del_exp_ItemList_MasterDetails", sp);
                    Response.Redirect("exp_ordermodifyplantqty.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvorderlist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}