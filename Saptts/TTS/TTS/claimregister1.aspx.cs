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
using System.Text;
using System.IO;

namespace TTS
{
    public partial class claimregister1 : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && (dtUser.Rows[0]["claim_stencilassign"].ToString() == "True" ||
                            dtUser.Rows[0]["claim_crm_exp"].ToString() == "True" || dtUser.Rows[0]["claim_crm_dom"].ToString() == "True"))
                        {
                            DataTable dtCustList = new DataTable();
                            if (Utilities.Decrypt(Request["fid"].ToString()) == "d")
                            {
                                if (Request.Cookies["TTSUserType"].Value.ToLower() == "admin" || Request.Cookies["TTSUserType"].Value.ToLower() == "support")
                                    dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsDomesticCustomer", DataAccess.Return_Type.DataTable);
                                else
                                {
                                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@LeadWise", Request.Cookies["TTSUser"].Value) };
                                    dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_CotsDomesticCustomer_LeadWise", sp, DataAccess.Return_Type.DataTable);
                                }
                            }
                            else if (Utilities.Decrypt(Request["fid"].ToString()) == "e")
                                dtCustList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_export_fullname", DataAccess.Return_Type.DataTable);

                            if (dtCustList.Rows.Count > 0)
                            {
                                ddlClaimCustomer.DataSource = dtCustList;
                                ddlClaimCustomer.DataTextField = "custfullname";
                                ddlClaimCustomer.DataValueField = "custcode";
                                ddlClaimCustomer.DataBind();
                                ddlClaimCustomer.Items.Insert(0, "CHOOSE");
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
        protected void ddlClaimCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["__EVENTTARGET"] == "ctl00$MainContent$ddlClaimCustomer")
                {
                    ddlClaimCustomer.SelectedIndex = ddlClaimCustomer.Items.IndexOf(ddlClaimCustomer.Items.FindByText(hdnCustomer.Value));
                    gvClaimList.DataSource = null;
                    gvClaimList.DataBind();
                    if (ddlClaimCustomer.SelectedItem.Text != "CHOOSE")
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@custfullname", ddlClaimCustomer.SelectedItem.Text), 
                            new SqlParameter("@stdcustcode", ddlClaimCustomer.SelectedItem.Value) 
                        };
                        DataTable dtUserList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_user_name", sp1, DataAccess.Return_Type.DataTable);
                        if (dtUserList.Rows.Count > 0)
                        {
                            ddlUserID.DataSource = dtUserList;
                            ddlUserID.DataTextField = "username";
                            ddlUserID.DataValueField = "ID";
                            ddlUserID.DataBind();
                            if (dtUserList.Rows.Count == 1)
                                ddlUserID_SelectedIndexChanged(sender, e);
                            else
                                ddlUserID.Items.Insert(0, "CHOOSE");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlUserID.SelectedItem.Text != "CHOOSE")
                {
                    Session["cotscode"] = ddlUserID.SelectedItem.Value;
                    gvClaimList.DataSource = null;
                    gvClaimList.DataBind();

                    ddl_bind(ddlClaimBrand);
                    create_Datatable_Column();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ddl_bind(DropDownList ddllist)
        {
            DataTable dt = new DataTable();
            SqlParameter[] sp1 = new SqlParameter[1];
            if (ddlClaimCustomer.SelectedItem.Value == "DE0048")
            {
                sp1[0] = new SqlParameter("@custcode", ddlUserID.SelectedItem.Value);
                dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_domestic_cust_brand", sp1, DataAccess.Return_Type.DataTable);
            }
            else if (ddlClaimCustomer.SelectedItem.Value != "DE0048")
            {
                sp1[0] = new SqlParameter("@custcode", ddlClaimCustomer.SelectedItem.Value);
                dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Export_cust_brand", sp1, DataAccess.Return_Type.DataTable);
            }
            if (dt.Rows.Count > 0)
            {
                ddllist.DataSource = dt;
                ddllist.DataTextField = "brand";
                ddllist.DataValueField = "brand";
                ddllist.DataBind();
                ddllist.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
            }
            else
            {
                ddllist.Items.Clear();
                ddllist.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
            }
        }
        private void create_Datatable_Column()
        {
            try
            {
                DataTable dtClaim = new DataTable();
                DataColumn col = new DataColumn("config", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("TyreType", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("brand", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("TyreSize", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("Qty", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("StencilNo", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("RunningHours", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("AppStyle", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("ClaimDesc", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("Comments", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("TyrePosition", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("ForkliftBrand", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("ForkliftType", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("DriveType", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("HoursPerDay", typeof(System.Decimal));
                dtClaim.Columns.Add(col);
                col = new DataColumn("MaxTemp", typeof(System.Decimal));
                dtClaim.Columns.Add(col);
                col = new DataColumn("MaxTempType", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("AnyAttach", typeof(System.Boolean));
                dtClaim.Columns.Add(col);
                col = new DataColumn("AttachDetails", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("FittedDate", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("FailureDate", typeof(System.String));
                dtClaim.Columns.Add(col);
                col = new DataColumn("ServiceHours", typeof(System.Decimal));
                dtClaim.Columns.Add(col);
                col = new DataColumn("MaxLoadCarrier", typeof(System.Decimal));
                dtClaim.Columns.Add(col);
                col = new DataColumn("OutsideDia", typeof(System.Decimal));
                dtClaim.Columns.Add(col);

                ViewState["dtClaim"] = dtClaim;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnAddMore_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrmsg.Text = "";
                bool rowExcist = false;
                DataTable dtClaim = ViewState["dtClaim"] as DataTable;

                foreach (DataRow rowChk in dtClaim.Rows)
                {
                    if (rowChk["StencilNo"].ToString() == txtStencilNos.Text.Trim())
                    {
                        lblErrmsg.Text = "Stencil No. Already Added";
                        string serverURL = Server.MapPath("~/");
                        string path = serverURL + "/claimimages/" + ddlUserID.SelectedItem.Value + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/dummy/";
                        if (System.IO.Directory.Exists(path))
                        {
                            var dir = new System.IO.DirectoryInfo(path);
                            dir.Delete(true);
                        }
                        rowExcist = true;
                        break;
                    }
                }

                if (!rowExcist)
                {
                    DataRow row = dtClaim.NewRow();
                    row["config"] = "";
                    row["TyreType"] = hdnTyreType.Value;
                    row["brand"] = hdnBrand.Value;
                    row["TyreSize"] = hdnTyreSize.Value;
                    row["ClaimDesc"] = "";
                    row["Comments"] = txtClaimComments.Text.Replace("\r\n", "~");
                    row["AppStyle"] = txtComplaint.Text.Replace("\r\n", "~");
                    row["RunningHours"] = txtFloorConditions.Text.Replace("\r\n", "~");
                    row["Qty"] = 0;
                    row["StencilNo"] = txtStencilNos.Text.Trim();

                    row["TyrePosition"] = rdbTyrePosition.SelectedIndex != -1 ? rdbTyrePosition.SelectedItem.Text : "";
                    row["ForkliftBrand"] = txtForkliftBrand.Text;
                    row["ForkliftType"] = rdbForkliftType.SelectedIndex != -1 ? rdbForkliftType.SelectedItem.Text : "";
                    row["DriveType"] = rdbForkliftDriveType.SelectedIndex != -1 ? rdbForkliftDriveType.SelectedItem.Text : "";
                    row["HoursPerDay"] = txtForkliftHours.Text != "" ? Convert.ToDecimal(txtForkliftHours.Text) : Convert.ToDecimal("0.00");
                    row["MaxTemp"] = txtTemperature.Text != "" ? Convert.ToDecimal(txtTemperature.Text) : Convert.ToDecimal("0.00");
                    row["MaxTempType"] = rdbTemperatureType.SelectedIndex != -1 ? rdbTemperatureType.SelectedItem.Text : ""; ;
                    row["AnyAttach"] = rdbAnyAttach.SelectedIndex != -1 && rdbAnyAttach.SelectedItem.Text == "Yes" ? true : false;
                    row["AttachDetails"] = txtAnyAttachDetails.Text.Replace("\r\n", "~");
                    row["FittedDate"] = txtTyreFittedDate.Text;
                    row["FailureDate"] = txtFailureDate.Text;
                    row["ServiceHours"] = txtServiceHours.Text != "" ? Convert.ToDecimal(txtServiceHours.Text).ToString() : "0.00";
                    row["MaxLoadCarrier"] = txtMaximumLoadKg.Text != "" ? Convert.ToDecimal(txtMaximumLoadKg.Text).ToString() : "0.00";
                    row["OutsideDia"] = txtOutsideDiaMM.Text != "" ? Convert.ToDecimal(txtOutsideDiaMM.Text).ToString() : "0.00";
                    dtClaim.Rows.Add(row);

                    string serverURL = Server.MapPath("~/");
                    string path = serverURL + "/claimimages/" + ddlUserID.SelectedItem.Value + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/dummy/";
                    if (System.IO.Directory.Exists(path))
                        System.IO.Directory.Move(path, serverURL + "/claimimages/" + ddlUserID.SelectedItem.Value + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/" + txtStencilNos.Text.Trim() + "/");
                }
                if (dtClaim.Rows.Count > 0)
                {
                    gvClaimList.DataSource = dtClaim;
                    gvClaimList.DataBind();
                    ViewState["dtClaim"] = dtClaim;
                }
                ddlClaimBrand.SelectedIndex = 0;
                txtClaimComments.Text = "";
                txtComplaint.Text = "";
                txtFloorConditions.Text = "";
                txtStencilNos.Text = "";
                hdnBrand.Value = "";
                hdnTyreSize.Value = "";
                hdnTyreType.Value = "";
                rdbTyrePosition.SelectedIndex = -1;
                txtForkliftBrand.Text = "";
                rdbForkliftType.SelectedIndex = -1;
                rdbForkliftDriveType.SelectedIndex = -1;
                txtForkliftHours.Text = "";
                txtTemperature.Text = "";
                rdbTemperatureType.SelectedIndex = -1;
                rdbAnyAttach.SelectedIndex = -1;
                txtAnyAttachDetails.Text = "";
                txtTyreFittedDate.Text = "";
                txtFailureDate.Text = "";
                txtServiceHours.Text = "";
                txtMaximumLoadKg.Text = "";
                txtOutsideDiaMM.Text = "";
                ddlClaimCustomer.SelectedIndex = ddlClaimCustomer.Items.IndexOf(ddlClaimCustomer.Items.FindByText(hdnCustomer.Value));
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                lblErrmsg.Text = ex.Message;
            }
        }
        protected void btnTriggerGv_Click(object sender, EventArgs e)
        {
            try
            {
                gv_StencilFailure.DataSource = null;
                gv_StencilFailure.DataBind();
                DataTable dtImage = new DataTable();
                DataColumn col = new DataColumn("ClaimImage", typeof(System.String));
                dtImage.Columns.Add(col);
                col = new DataColumn("ClaimImageName", typeof(System.String));
                dtImage.Columns.Add(col);

                string serverURL = HttpContext.Current.Server.MapPath("~/");
                string strFolder = "/claimimages" + "/" + ddlUserID.SelectedItem.Value + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/dummy/";
                if (Directory.Exists(serverURL + strFolder))
                {
                    string strFileName = string.Empty;
                    foreach (string d in Directory.GetFiles(serverURL + strFolder))
                    {
                        string strImgName = d.Replace(serverURL + strFolder, "");
                        string strURL = strFolder + strImgName;
                        dtImage.Rows.Add(ResolveUrl(strURL), strImgName);
                    }
                    gv_StencilFailure.DataSource = dtImage;
                    gv_StencilFailure.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvClaimList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gvClaimList.Rows[e.RowIndex];
                Label lblStencilNo = row.FindControl("lblStencilNo") as Label;

                string _strStencil = lblStencilNo.Text;
                if (_strStencil != "")
                {
                    DataTable dtClaim = ViewState["dtClaim"] as DataTable;
                    foreach (DataRow rowChk in dtClaim.Rows)
                    {
                        if (rowChk["StencilNo"].ToString() == _strStencil)
                        {
                            rowChk.Delete();
                            try
                            {
                                string serverURL = Server.MapPath("~/").Replace("SCOTS", "TTS").Replace("COTS", "TTS");
                                string path = serverURL + "/claimimages/" + ddlUserID.SelectedItem.Value + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/" + _strStencil + "/";
                                if (System.IO.Directory.Exists(path))
                                {
                                    var dir = new System.IO.DirectoryInfo(path);
                                    dir.Delete(true);
                                }
                            }
                            catch (Exception ex)
                            {
                                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Delete Folder: " + ex.Message);
                            }
                            break;
                        }
                    }
                    gvClaimList.EditIndex = -1;
                    gvClaimList.DataSource = dtClaim;
                    gvClaimList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvClaimList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtclaimregisterlist = ViewState["dtClaim"] as DataTable;
                gvClaimList.EditIndex = e.NewEditIndex;
                gvClaimList.DataSource = dtclaimregisterlist;
                gvClaimList.DataBind();
                GridViewRow row = gvClaimList.Rows[e.NewEditIndex];
                HiddenField lbledTyreSize = row.FindControl("lbledTyreSize") as HiddenField;
                HiddenField lbledbrand = row.FindControl("lbledbrand") as HiddenField;
                HiddenField lbledTyreType = row.FindControl("lbledTyreType") as HiddenField;
                DropDownList ddledbrand = (DropDownList)row.FindControl("ddledbrand");
                DropDownList ddledTyreType = (DropDownList)row.FindControl("ddledTyreType");
                DropDownList ddledTyreSize = (DropDownList)row.FindControl("ddledTyreSize");
                ddl_bind(ddledbrand);
                ddledbrand.SelectedValue = lbledbrand.Value;
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@brand", lbledbrand.Value);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_tyresize_BrandWise", sp1, DataAccess.Return_Type.DataTable);

                ddledTyreSize.DataSource = dt;
                ddledTyreSize.DataTextField = "TyreSize";
                ddledTyreSize.DataValueField = "TyreSize";
                ddledTyreSize.DataBind();
                ddledTyreSize.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                SqlParameter[] sp = new SqlParameter[2];
                sp[0] = new SqlParameter("@brand", lbledbrand.Value);
                sp[1] = new SqlParameter("@TyreSize", lbledTyreSize.Value);
                DataTable dt1 = (DataTable)daTTS.ExecuteReader_SP("sp_sel_tyreType_Brand_Size_Wise", sp, DataAccess.Return_Type.DataTable);
                if (dt1.Rows.Count > 0)
                {
                    ddledTyreType.DataSource = dt1;
                    ddledTyreType.DataTextField = "TyreType";
                    ddledTyreType.DataValueField = "TyreType";
                    ddledTyreType.DataBind();
                    ddledTyreType.Items.Insert(0, new ListItem("CHOOSE", "CHOOSE"));
                }
                ddledTyreSize.SelectedValue = lbledTyreSize.Value;
                ddledTyreType.SelectedValue = lbledTyreType.Value;
                hdnBrand.Value = ddledbrand.SelectedItem.Text;
                hdnTyreSize.Value = ddledTyreSize.SelectedItem.Text;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvClaimList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dtclaim = ViewState["dtClaim"] as DataTable;
                GridViewRow row = gvClaimList.Rows[e.RowIndex];
                HiddenField hdnstencilNo = row.FindControl("hdnstencilNo") as HiddenField;
                TextBox txtStencil = row.FindControl("txtStencil") as TextBox;
                TextBox txtAppStyle = row.FindControl("txtAppStyle") as TextBox;
                TextBox txtRunningHours = row.FindControl("txtRunningHours") as TextBox;
                DropDownList ddledTyreSize = row.FindControl("ddledTyreSize") as DropDownList;
                DropDownList ddledbrand = row.FindControl("ddledbrand") as DropDownList;
                DropDownList ddledTyreType = row.FindControl("ddledTyreType") as DropDownList;
                if (hdnstencilNo.Value != "" && txtStencil.Text != "")
                {
                    if (dtclaim.Rows.Count > 0)
                    {
                        foreach (DataRow iRow in dtclaim.Select("StencilNo='" + hdnstencilNo.Value + "'"))
                        {
                            iRow["brand"] = hdnBrand.Value;
                            iRow["TyreSize"] = hdnTyreSize.Value;
                            iRow["TyreType"] = hdnTyreType.Value;
                            iRow["AppStyle"] = txtAppStyle.Text.Replace("\r\n", "~");
                            iRow["RunningHours"] = txtRunningHours.Text.Replace("\r\n", "~");
                            iRow["StencilNo"] = txtStencil.Text;
                            if (hdnstencilNo.Value != txtStencil.Text)
                            {
                                string serverURL = Server.MapPath("~/").Replace("SCOTS", "TTS").Replace("COTS", "TTS");
                                string path = serverURL + "/claimimages/" + ddlUserID.SelectedItem.Value + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/" + hdnstencilNo.Value.Trim() + "/";
                                if (System.IO.Directory.Exists(path))
                                    System.IO.Directory.Move(path, serverURL + "/claimimages/" + ddlUserID.SelectedItem.Value + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/" + txtStencil.Text.Trim() + "/");
                            }
                        }
                        ViewState["dtClaim"] = dtclaim;
                    }
                }
                gvClaimList.EditIndex = -1;
                gvClaimList.DataSource = dtclaim;
                gvClaimList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void gvClaimList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                DataTable dtclaimregisterlist = ViewState["dtClaim"] as DataTable;
                e.Cancel = true;
                gvClaimList.EditIndex = -1;
                gvClaimList.DataSource = dtclaimregisterlist;
                gvClaimList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSendClaimList_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnBrand.Value != "" && hdnTyreSize.Value != "" && txtStencilNos.Text.Trim() != "" && txtComplaint.Text != "")
                {
                    btnAddMore_Click(sender, e);
                }
                lblErrmsg.Text = "";
                if (gvClaimList.Rows.Count > 0)
                {
                    ddlClaimCustomer.SelectedIndex = ddlClaimCustomer.Items.IndexOf(ddlClaimCustomer.Items.FindByText(hdnCustomer.Value));
                    string stdcode = ddlClaimCustomer.SelectedItem.Value == "DE0048" ? "D" : "E";
                    DataTable dtClaim = ViewState["dtClaim"] as DataTable;
                    SqlParameter[] sp1 = new SqlParameter[6];
                    sp1[0] = new SqlParameter("@custcode", ddlUserID.SelectedItem.Value);
                    sp1[1] = new SqlParameter("@custname", ddlClaimCustomer.SelectedItem.Text);
                    sp1[2] = new SqlParameter("@claimitem_datatable_1", dtClaim);
                    sp1[3] = new SqlParameter("@CompCommets", txtClaimComments.Text.Replace("\r\n", "~"));
                    sp1[4] = new SqlParameter("@stdcode", stdcode.Trim());
                    sp1[5] = new SqlParameter("@CustomerRefNo", txtCustomerRefNo.Text);
                    int resp = daCOTS.ExecuteNonQuery_SP("sp_ins_claimlist_datatable_1", sp1);
                    string strClaimNo = string.Empty;
                    if (resp > 0)
                    {
                        SqlParameter[] sp2 = new SqlParameter[1];
                        sp2[0] = new SqlParameter("@custcode", ddlUserID.SelectedItem.Value);
                        strClaimNo = (string)daCOTS.ExecuteScalar_SP("sp_sel_lastclaimregister", sp2);
                        try
                        {
                            string serverURL = Server.MapPath("~/");
                            string path = serverURL + "/claimimages/" + ddlUserID.SelectedItem.Value + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/";
                            if (System.IO.Directory.Exists(path))
                                System.IO.Directory.Move(path, serverURL + "/claimimages/" + ddlUserID.SelectedItem.Value + "/" + strClaimNo + "/");
                        }
                        catch (Exception ex)
                        {
                            Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Folder Name Change: " + ex.Message);
                        }
                        
                    }
                    Response.Redirect("claimstencilassign.aspx", false);
                }
                else
                    lblErrmsg.Text = "No records";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlClaimBrand_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
    }
}