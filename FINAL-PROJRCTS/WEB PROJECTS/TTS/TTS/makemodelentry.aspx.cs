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

namespace TTS
{
    public partial class makemodelentry : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["makemodel_entry"].ToString() == "True")
                        {
                            Bind_GvList();
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

        private void Bind_GvList()
        {
            DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_MakeModelList", DataAccess.Return_Type.DataTable);
            gvMakeModelEntryList.DataSource = dt;
            gvMakeModelEntryList.DataBind();
        }

        protected void gvMakeModelEntryList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Invisibling the first three columns of second row header (normally created on binding)
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; // Invisibiling OEM Header Cell
                e.Row.Cells[1].Visible = false; // Invisibiling MODEL Header Cell
                e.Row.Cells[9].Visible = false; // Invisibiling TYRE TYPE Header Cell
                e.Row.Cells[10].Visible = false; // Invisibiling AVAILABILITY Header Cell
                e.Row.Cells[11].Visible = false; // Invisibiling ACTION Header Cell
            }
        }

        protected void gvMakeModelEntryList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // Adding a column manually once the header created
            if (e.Row.RowType == DataControlRowType.Header) // If header created
            {
                GridView ProductGrid = (GridView)sender;

                // Creating a Row
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                //Adding OEM Column
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "OEM";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2; // For merging first, second row cells to one
                HeaderCell.CssClass = "mergecss configCss";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding MODEL Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "MODEL";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "mergecss configCss";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding LIFT Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "LIFT";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 3; // For merging three columns (Direct, Referral, Total)
                HeaderCell.CssClass = "mergecss configCss";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding WHEEL/TYRE SIZE Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "WHEEL/TYRE SIZE";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 2; // For merging three columns (Direct, Referral, Total)
                HeaderCell.CssClass = "mergecss configCss";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding NOs. TYRES Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "NOs. TYRES";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 2; // For merging three columns (Direct, Referral, Total)
                HeaderCell.CssClass = "mergecss configCss";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding TYRE TYPE Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "TYRE TYPE";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "mergecss configCss";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding AVAILABILITY Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "AVAILABILITY";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "mergecss configCss";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding ACTION Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "ACTION";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "mergecss configCss";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding the Row at the 0th position (first row) in the Grid
                ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
            }
        }

        protected void gvMakeModelEntryList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gvMakeModelEntryList.EditIndex = e.NewEditIndex;
                Bind_GvList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }

        protected void gvMakeModelEntryList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gvMakeModelEntryList.Rows[e.RowIndex];
                TextBox txtTyreType = row.FindControl("txtTyreType") as TextBox;
                TextBox txtAvailable = row.FindControl("txtAvailable") as TextBox;
                Label lblManufacture = row.FindControl("lblManufacture") as Label;
                Label lblModel = row.FindControl("lblModel") as Label;
                Label lblLiftType = row.FindControl("lblLiftType") as Label;
                Label lblLiftWeightKg = row.FindControl("lblLiftWeightKg") as Label;
                Label lblLiftCapacityKg = row.FindControl("lblLiftCapacityKg") as Label;
                Label lblLiftSizeFront = row.FindControl("lblLiftSizeFront") as Label;
                Label lblLiftSizeRear = row.FindControl("lblLiftSizeRear") as Label;
                Label lblNosFront = row.FindControl("lblNosFront") as Label;
                Label lblNosRear = row.FindControl("lblNosRear") as Label;

                if (txtTyreType.Text != "" || txtAvailable.Text != "")
                {
                    SqlParameter[] sp1 = new SqlParameter[12];
                    sp1[0] = new SqlParameter("@Manufacture", lblManufacture.Text);
                    sp1[1] = new SqlParameter("@ModelID", lblModel.Text);
                    sp1[2] = new SqlParameter("@Lift_Type", lblLiftType.Text);
                    sp1[3] = new SqlParameter("@Lift_WeightKg", lblLiftWeightKg.Text);
                    sp1[4] = new SqlParameter("@Lift_CapacityKg", lblLiftCapacityKg.Text);
                    sp1[5] = new SqlParameter("@Wheel_Tyresize_Front", lblLiftSizeFront.Text);
                    sp1[6] = new SqlParameter("@Wheel_Tyresize_Rear", lblLiftSizeRear.Text);
                    sp1[7] = new SqlParameter("@Nos_Tyres_Front", lblNosFront.Text);
                    sp1[8] = new SqlParameter("@Nos_Tyres_Rear", lblNosRear.Text);
                    sp1[9] = new SqlParameter("@TyreType", txtTyreType.Text);
                    sp1[10] = new SqlParameter("@SunAvailability", txtAvailable.Text);
                    sp1[11] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                    daTTS.ExecuteNonQuery_SP("sp_edit_MakeModelTyreList", sp1);
                }
                gvMakeModelEntryList.EditIndex = -1;
                Bind_GvList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvMakeModelEntryList_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                e.Cancel = true;
                gvMakeModelEntryList.EditIndex = -1;
                Bind_GvList();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvMakeModelEntryList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Bind_GvList();
                gvMakeModelEntryList.PageIndex = e.NewPageIndex;
                gvMakeModelEntryList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSaveNewModel_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[12];
                sp1[0] = new SqlParameter("@Manufacture", txtEntryOem.Text);
                sp1[1] = new SqlParameter("@ModelID", txtEntryModel.Text);
                sp1[2] = new SqlParameter("@Lift_Type", txtEntryLiftType.Text);
                sp1[3] = new SqlParameter("@Lift_WeightKg", txtEntryWeightKg.Text);
                sp1[4] = new SqlParameter("@Lift_CapacityKg", txtEntryCapacityKg.Text);
                sp1[5] = new SqlParameter("@Wheel_Tyresize_Front", txtSizeFront.Text);
                sp1[6] = new SqlParameter("@Wheel_Tyresize_Rear", txtSizeRear.Text);
                sp1[7] = new SqlParameter("@Nos_Tyres_Front", txtNosFront.Text);
                sp1[8] = new SqlParameter("@Nos_Tyres_Rear", txtNosRear.Text);
                sp1[9] = new SqlParameter("@TyreType", txtEntryTyretype.Text);
                sp1[10] = new SqlParameter("@SunAvailability", txtEntryAvailability.Text);
                sp1[11] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                int resp = daTTS.ExecuteNonQuery_SP("sp_ins_MakeModelTyreList", sp1);
                if (resp > 0)
                    Response.Redirect("makemodelentry.aspx", false);

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

    }
}