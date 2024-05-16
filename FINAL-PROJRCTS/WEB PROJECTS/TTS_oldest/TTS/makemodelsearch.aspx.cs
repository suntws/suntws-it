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
    public partial class makemodelsearch : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["makemodel_search"].ToString() == "True")
                        {
                            string strQry = "select distinct Manufacture from MakeModelTyreList where DataStatus=1 order by Manufacture";
                            DataTable dt = (DataTable)daTTS.ExecuteReader(strQry, DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                ddlOem.DataSource = dt;
                                ddlOem.DataTextField = "Manufacture";
                                ddlOem.DataValueField = "Manufacture";
                                ddlOem.DataBind();
                            }
                            ddlOem.Items.Insert(0, "CHOOSE");
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

        protected void ddlModelCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gvMakeModelSearchList.DataSource = null;
                gvMakeModelSearchList.DataBind();
                ddlModelValue.DataSource = null;
                ddlModelValue.DataBind();
                if (ddlModelCategory.SelectedItem.Text != "CHOOSE")
                {
                    string strQry = "select distinct " + ddlModelCategory.SelectedItem.Value + " from MakeModelTyreList where DataStatus=1 order by " + ddlModelCategory.SelectedItem.Value;
                    DataTable dt = (DataTable)daTTS.ExecuteReader(strQry, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlModelValue.DataSource = dt;
                        ddlModelValue.DataTextField = "" + ddlModelCategory.SelectedItem.Value + "";
                        ddlModelValue.DataValueField = "" + ddlModelCategory.SelectedItem.Value + "";
                        ddlModelValue.DataBind();
                    }
                    ddlModelValue.Items.Insert(0, "CHOOSE");
                    ddlModelValue.Text = "CHOOSE";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlModelValue_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gvMakeModelSearchList.DataSource = null;
                gvMakeModelSearchList.DataBind();
                if (ddlModelCategory.SelectedItem.Text != "CHOOSE")
                {
                    string strQry = "select Manufacture,ModelID,Lift_Type,Lift_WeightKg,Lift_CapacityKg,Wheel_Tyresize_Front,Wheel_Tyresize_Rear,Nos_Tyres_Front,Nos_Tyres_Rear,TyreType,SunAvailability from MakeModelTyreList where DataStatus=1 and " + ddlModelCategory.SelectedItem.Value + "='" + ddlModelValue.SelectedItem.Text + "'";
                    DataTable dt = (DataTable)daTTS.ExecuteReader(strQry, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        gvMakeModelSearchList.DataSource = dt;
                        gvMakeModelSearchList.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlOem_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gvMakeModelSearchList.DataSource = null;
                gvMakeModelSearchList.DataBind();
                ddlModel.DataSource = "";
                ddlModel.DataBind();
                if (ddlOem.SelectedItem.Text != "CHOOSE")
                {
                    string strQry = "select Manufacture,ModelID,Lift_Type,Lift_WeightKg,Lift_CapacityKg,Wheel_Tyresize_Front,Wheel_Tyresize_Rear,Nos_Tyres_Front,Nos_Tyres_Rear,TyreType,SunAvailability from MakeModelTyreList where DataStatus=1 and Manufacture='" + ddlOem.SelectedItem.Text + "'";
                    DataTable dt = (DataTable)daTTS.ExecuteReader(strQry, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        gvMakeModelSearchList.DataSource = dt;
                        gvMakeModelSearchList.DataBind();
                    }

                    strQry = "select distinct ModelID from MakeModelTyreList where DataStatus=1 and Manufacture='" + ddlOem.SelectedItem.Text + "' order by ModelID";
                    dt = (DataTable)daTTS.ExecuteReader(strQry, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        ddlModel.DataSource = dt;
                        ddlModel.DataTextField = "ModelID";
                        ddlModel.DataValueField = "ModelID";
                        ddlModel.DataBind();
                        ddlModel.Items.Insert(0, "CHOOSE");
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlModel_IndexChange(object sender, EventArgs e)
        {
            try
            {
                gvMakeModelSearchList.DataSource = null;
                gvMakeModelSearchList.DataBind();
                if (ddlModel.SelectedItem.Text != "CHOOSE")
                {
                    string strQry = "select Manufacture,ModelID,Lift_Type,Lift_WeightKg,Lift_CapacityKg,Wheel_Tyresize_Front,Wheel_Tyresize_Rear,Nos_Tyres_Front,Nos_Tyres_Rear,TyreType,SunAvailability from MakeModelTyreList where DataStatus=1 and Manufacture='" + ddlOem.SelectedItem.Text + "' and ModelID='" + ddlModel.SelectedItem.Text + "'";
                    DataTable dt = (DataTable)daTTS.ExecuteReader(strQry, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        gvMakeModelSearchList.DataSource = dt;
                        gvMakeModelSearchList.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvMakeModelSearchList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Invisibling the first three columns of second row header (normally created on binding)
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; // Invisibiling OEM Header Cell
                e.Row.Cells[1].Visible = false; // Invisibiling MODEL Header Cell
                e.Row.Cells[9].Visible = false; // Invisibiling TYRE TYPE Header Cell
                e.Row.Cells[10].Visible = false; // Invisibiling AVAILABILITY Header Cell
            }
        }

        protected void gvMakeModelSearchList_RowCreated(object sender, GridViewRowEventArgs e)
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

                //Adding the Row at the 0th position (first row) in the Grid
                ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
            }
        }
    }
}