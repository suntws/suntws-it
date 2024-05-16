using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using System.Xml;
namespace TTS
{
    public partial class TyreDrawingRequest : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dwg_request"].ToString() == "True")
                        {
                            btnDwgRequestSave.Text = "DRAWING REQUEST SAVE";
                            DataTable dtCust = (DataTable)daCOTS.ExecuteReader("select distinct custfullname,ID from usermaster where userstatus=1 order by custfullname", DataAccess.Return_Type.DataTable);
                            if (dtCust.Rows.Count > 0)
                            {
                                ddlCustomerSpecific.DataSource = dtCust;
                                ddlCustomerSpecific.DataTextField = "custfullname";
                                ddlCustomerSpecific.DataValueField = "custfullname";
                                ddlCustomerSpecific.DataBind();
                            }
                            ddlCustomerSpecific.Items.Insert(0, "Choose");
                            ddlRimWidth.Items.Insert(0, "Choose");
                            DataTable dtSize = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Tyresize", DataAccess.Return_Type.DataTable);
                            if (dtSize.Rows.Count > 0)
                            {
                                ddlTyreSize.DataSource = dtSize;
                                ddlTyreSize.DataTextField = "TyreSize";
                                ddlTyreSize.DataValueField = "TyreSize";
                                ddlTyreSize.DataBind();
                            }
                            ddlTyreSize.Items.Insert(0, "Choose");
                            Bind_DropDown(rdbRims, "2");
                            Bind_DropDown(rdbNoOfHole, "3");
                            Bind_DropDown1(ddlPCD, "4");
                            Bind_DropDown1(ddlBOREDIA, "5");
                            Bind_DropDown1(ddlHoleDia, "6");
                            Fill_ddl_XML();
                            DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_TyreDrawingRequest", DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                gvDwgRequest.DataSource = dt;
                                gvDwgRequest.DataBind();
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please Contact administrator";
                        }
                    }
                }
                else Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_DropDown(RadioButtonList ddlName, string strvalue)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@FieldName", "textofcolumn");
                sp1[1] = new SqlParameter("@value", strvalue);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_distinct_DrawingmasterCategory", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.Sort = "ID";
                    DataTable sortedDT = dv.ToTable();
                    ddlName.DataSource = sortedDT;
                    ddlName.DataTextField = "textofcolumn";
                    ddlName.DataValueField = "textofcolumn";
                    ddlName.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Bind_DropDown1(DropDownList ddlName, string strvalue)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@FieldName", "textofcolumn");
                sp1[1] = new SqlParameter("@value", strvalue);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_distinct_DrawingmasterCategory", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlName.DataSource = dt;
                    ddlName.DataTextField = "textofcolumn";
                    ddlName.DataValueField = "textofcolumn";
                    ddlName.DataBind();
                }
                ddlName.Items.Insert(0, "Choose");
                ddlName.Items.Insert(1, "ADD NEW ENTRY");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Fill_ddl_XML()
        {
            try
            {
                string strXmlUserLevelList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"TyreDwgDetails.xml");
                XmlDocument xmlLevelList = new XmlDocument();

                xmlLevelList.Load(strXmlUserLevelList);

                if (xmlLevelList != null)
                {
                    var dict = new Dictionary<string, string>();
                    DataTable dtList = new DataTable();
                    dtList.Columns.Add("text", typeof(string));
                    dtList.Columns.Add("key", typeof(string));

                    //multipiecerims
                    foreach (XmlNode xNode in xmlLevelList.SelectNodes("/TyreDwg/multipiecerims/list"))
                    { dtList.Rows.Add(xNode.Attributes["text"].Value, xNode.Attributes["key"].Value); }
                    if (dtList.Rows.Count > 0)
                    {
                        ddlNoofpiece.DataSource = dtList;
                        ddlNoofpiece.DataTextField = "text";
                        ddlNoofpiece.DataValueField = "key";
                        ddlNoofpiece.DataBind();
                    }

                    //Radius
                    dtList.Clear();
                    foreach (XmlNode xNode in xmlLevelList.SelectNodes("/TyreDwg/Radius/list"))
                    { dtList.Rows.Add(xNode.Attributes["text"].Value, xNode.Attributes["key"].Value); }
                    if (dtList.Rows.Count > 0)
                    {
                        ddlRadius.DataSource = dtList;
                        ddlRadius.DataTextField = "text";
                        ddlRadius.DataValueField = "key";
                        ddlRadius.DataBind();
                    }

                    //Angle
                    dtList.Clear();
                    foreach (XmlNode xNode in xmlLevelList.SelectNodes("/TyreDwg/Angle/list"))
                    { dtList.Rows.Add(xNode.Attributes["text"].Value, xNode.Attributes["key"].Value); }

                    if (dtList.Rows.Count > 0)
                    {
                        ddlangle.DataSource = dtList;
                        ddlangle.DataTextField = "text";
                        ddlangle.DataValueField = "key";
                        ddlangle.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnDwgRequestSave_Click(object sender, EventArgs e)
        {
            try
            {
                save_ddl();
                SqlParameter[] sp = new SqlParameter[25];
                int resp = 0;
                if (hdneditslno.Value != "")
                    sp = new SqlParameter[26];
                sp[0] = new SqlParameter("@TyreSize", ddlTyreSize.SelectedItem.Text);
                sp[1] = new SqlParameter("@RimWidth", ddlRimWidth.SelectedItem.Text == "Choose" ? "" : ddlRimWidth.SelectedItem.Text);
                sp[2] = new SqlParameter("@TyreType", rdbTyreType.SelectedItem.Text);
                sp[3] = new SqlParameter("@Location", rdbWheelLocation.SelectedValue);
                sp[4] = new SqlParameter("@AxelEnd", rdbWheelAxleEnd.SelectedValue);
                sp[5] = new SqlParameter("@WheelApp", rdbWheelApp.SelectedValue);
                sp[6] = new SqlParameter("@RimType", rdbRims.SelectedValue);
                sp[7] = new SqlParameter("@NoofPiece", rdbRims.SelectedValue == "MULTI PIECE" ? ddlNoofpiece.SelectedValue : "");
                sp[8] = new SqlParameter("@VehicleSpeed", txtVehicleSpeed.Text);
                sp[9] = new SqlParameter("@Piloted", rbdPiloted.SelectedValue);
                sp[10] = new SqlParameter("@MountHoles", rdbNoOfHole.SelectedValue);
                sp[11] = new SqlParameter("@PCD", ddlPCD.SelectedItem.Text == "ADD NEW ENTRY" ? txtPCD.Text : ddlPCD.SelectedItem.Text);
                sp[12] = new SqlParameter("@MountHolesDia", ddlHoleDia.SelectedItem.Text == "ADD NEW ENTRY" ? txtHoleDia.Text : ddlHoleDia.SelectedItem.Text);
                sp[13] = new SqlParameter("@HolesType", rdbHolesType.SelectedValue);
                sp[14] = new SqlParameter("@Countersink", rdbCountersink.SelectedValue);
                sp[15] = new SqlParameter("@radius_angle", rdbCountersink.SelectedValue == "SPHERICAL" ? ddlRadius.SelectedValue : ddlangle.SelectedValue);
                sp[16] = new SqlParameter("@BoreDia", ddlBOREDIA.SelectedItem.Text == "ADD NEW ENTRY" ? txtBoreDia.Text : ddlBOREDIA.SelectedItem.Text);
                sp[17] = new SqlParameter("@SpecialKey", txtSpecialKey.Text);
                sp[18] = new SqlParameter("@WheelStudLen", txtWheelStudLen.Text);
                sp[19] = new SqlParameter("@WheelWeight", txtWheelWeight.Text != "" ? txtWheelWeight.Text : "0");
                sp[20] = new SqlParameter("@DwgRemarks", txtDwgRemarks.Text.Replace("\r\n", "~"));
                sp[21] = new SqlParameter("@CustomerName", ddlCustomerSpecific.SelectedItem.Text != "Choose" ? ddlCustomerSpecific.SelectedItem.Text : "");
                sp[22] = new SqlParameter("@CreatedBy", Request.Cookies["TTSUser"].Value);
                sp[23] = new SqlParameter("@InfoDerivedFromPast", txtInfoDerived.Text.Replace("\r\n", "~"));
                sp[24] = new SqlParameter("@StatutoryInfo", txtStatutory.Text.Replace("\r\n", "~"));
                if (hdneditslno.Value != "")
                {
                    sp[25] = new SqlParameter("@SLno", hdneditslno.Value);
                    resp = daTTS.ExecuteNonQuery_SP("sp_update_TyreDrawingRequest", sp);
                }
                else
                    resp = daTTS.ExecuteNonQuery_SP("sp_ins_TyreDrawingRequest", sp);
                if (resp > 0)
                {
                    if (fupTDS.HasFile)
                    {
                        SqlParameter[] sp1 = new SqlParameter[2];
                        sp1[0] = new SqlParameter("@CustomerName", ddlCustomerSpecific.SelectedItem.Text != "Choose" ? ddlCustomerSpecific.SelectedItem.Text : "");
                        sp1[1] = new SqlParameter("@CreatedBy", Request.Cookies["TTSUser"].Value);
                        DataTable dtfilename = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Slno_DwgRequest", sp1, DataAccess.Return_Type.DataTable);
                        if (dtfilename.Rows.Count > 0)
                        {
                            if (!Directory.Exists(Server.MapPath("~/TYREDRAWINGCATALOG/DWGRequest/")))
                                Directory.CreateDirectory(Server.MapPath("~/TYREDRAWINGCATALOG/DWGRequest/"));
                            string filename = hdneditslno.Value == "" ? dtfilename.Rows[0]["Slno"].ToString() : hdneditslno.Value;
                            string strsave = Server.MapPath("~/TYREDRAWINGCATALOG/DWGRequest/") + filename + ".pdf";
                            fupTDS.SaveAs(strsave);
                            SqlParameter[] sp2 = new SqlParameter[1];
                            sp2[0] = new SqlParameter("@Slno", filename);
                            daTTS.ExecuteNonQuery_SP("sp_update_DwgRequest_FileStatus", sp2);
                        }
                    }
                    Response.Redirect("TyreDrawingRequest.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
                Label lblTyreSize = row.FindControl("lblTyreSize") as Label;
                Label lblTyreType = row.FindControl("lblTyreType") as Label;
                Label lblLocation = row.FindControl("lblLocation") as Label;
                Label lblAxelEnd = row.FindControl("lblAxelEnd") as Label;
                Label lblWheelApp = row.FindControl("lblWheelApp") as Label;
                Label lblRimType = row.FindControl("lblRimType") as Label;
                Label lblVehicleSpeed = row.FindControl("lblVehicleSpeed") as Label;
                Label lblPiloted = row.FindControl("lblPiloted") as Label;
                Label lblMountHoles = row.FindControl("lblMountHoles") as Label;
                Label lblPCD = row.FindControl("lblPCD") as Label;
                Label lblMountHolesDia = row.FindControl("lblMountHolesDia") as Label;
                Label lblHolesType = row.FindControl("lblHolesType") as Label;
                Label lblBoreDia = row.FindControl("lblBoreDia") as Label;
                Label lblSpecialKey = row.FindControl("lblSpecialKey") as Label;
                Label lblWheelStudLen = row.FindControl("lblWheelStudLen") as Label;
                Label lblRimWidth = row.FindControl("lblRimWidth") as Label;
                Label lblWheelWeight = row.FindControl("lblWheelWeight") as Label;
                HiddenField hdnSlno = row.FindControl("hdnSlno") as HiddenField;
                HiddenField hdnNoofPiece = row.FindControl("hdnNoofPiece") as HiddenField;
                HiddenField hdnCountersink = row.FindControl("hdnCountersink") as HiddenField;
                HiddenField hdnDwgRemarks = row.FindControl("hdnDwgRemarks") as HiddenField;
                HiddenField hdnradius_angle = row.FindControl("hdnradius_angle") as HiddenField;
                Label lblCustomerName = row.FindControl("lblCustomerName") as Label;
                HiddenField hdnInfoDerived = row.FindControl("hdnInfoDerived") as HiddenField;
                HiddenField hdnStatutory = row.FindControl("hdnStatutory") as HiddenField;

                ddlTyreSize.SelectedValue = lblTyreSize.Text;
                ddlTyreSize_SelectedIndexChanged(sender, e);
                ddlRimWidth.SelectedValue = lblRimWidth.Text == "" ? "Choose" : lblRimWidth.Text;
                rdbTyreType.SelectedValue = lblTyreType.Text;
                rdbWheelLocation.SelectedValue = lblLocation.Text;
                rdbWheelAxleEnd.SelectedValue = lblAxelEnd.Text;
                rdbWheelApp.SelectedValue = lblWheelApp.Text;
                rdbRims.SelectedValue = lblRimType.Text;
                if (lblRimType.Text == "MULTI PIECE")
                    ddlNoofpiece.SelectedValue = hdnNoofPiece.Value;
                txtVehicleSpeed.Text = lblVehicleSpeed.Text;
                rbdPiloted.SelectedValue = lblPiloted.Text;
                rdbNoOfHole.SelectedValue = lblMountHoles.Text;
                ddlPCD.SelectedValue = lblPCD.Text;
                ddlHoleDia.SelectedValue = lblMountHolesDia.Text;

                rdbHolesType.SelectedValue = lblHolesType.Text;
                if (lblHolesType.Text == "COUNTERSINK")
                {
                    divcountersinkangle.Style.Add("display", "block");
                    if (hdnCountersink.Value == "SPHERICAL") { divconical.Style.Add("display", "none"); divspherical.Style.Add("display", "block"); ddlRadius.SelectedValue = hdnradius_angle.Value; }
                    else { divspherical.Style.Add("display", "none"); divconical.Style.Add("display", "block"); ddlangle.SelectedValue = hdnradius_angle.Value; }
                }
                else { divcountersinkangle.Style.Add("display", "none"); divconical.Style.Add("display", "none"); divspherical.Style.Add("display", "none"); }
                if (lblRimType.Text == "MULTI PIECE") divrimsspecify.Style.Add("display", "block");
                else divrimsspecify.Style.Add("display", "none");
                rdbCountersink.SelectedValue = hdnCountersink.Value;
                ddlBOREDIA.SelectedValue = lblBoreDia.Text;
                txtSpecialKey.Text = lblSpecialKey.Text;
                txtWheelStudLen.Text = lblWheelStudLen.Text;
                txtWheelWeight.Text = lblWheelWeight.Text;
                txtDwgRemarks.Text = hdnDwgRemarks.Value.Replace("~", "\r\n");
                hdneditslno.Value = hdnSlno.Value;
                txtInfoDerived.Text = hdnInfoDerived.Value.Replace("~", "\r\n");
                txtStatutory.Text = hdnStatutory.Value.Replace("~", "\r\n");
                if (lblCustomerName.Text != "")
                    ddlCustomerSpecific.SelectedValue = lblCustomerName.Text;
                divpdf.Style.Add("display", "none");
                FileInfo file = new FileInfo(Server.MapPath("~/TYREDRAWINGCATALOG/DWGRequest/" + hdneditslno.Value + ".pdf"));
                if (file.Exists)
                {
                    hdnurl.Value = "TYREDRAWINGCATALOG/DWGRequest/" + hdneditslno.Value + ".pdf";
                    divpdf.Style.Add("display", "block");
                }
                btnDwgRequestSave.Text = "DRAWING REQUEST UPDATE";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void save_ddl()
        {
            try
            {
                if (ddlPCD.SelectedItem.Text == "ADD NEW ENTRY" && txtPCD.Text != "")
                    insert_ddl(txtPCD.Text, "4", "PCD");
                if (ddlBOREDIA.SelectedItem.Text == "ADD NEW ENTRY" && txtBoreDia.Text != "")
                    insert_ddl(txtBoreDia.Text, "5", "BOREDIA");
                if (ddlHoleDia.SelectedItem.Text == "ADD NEW ENTRY" && txtHoleDia.Text != "")
                    insert_ddl(txtHoleDia.Text, "6", "HOLEDIA");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void insert_ddl(string name, string value, string columnname)
        {
            try
            {
                SqlParameter[] sp11 = new SqlParameter[4];
                sp11[0] = new SqlParameter("@textofcolumn", name);
                sp11[1] = new SqlParameter("@value", value);
                sp11[2] = new SqlParameter("@nameofcolumn", columnname);
                sp11[3] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                int resp = daTTS.ExecuteNonQuery_SP("sp_ins_DrawingMasterCategory", sp11);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlTyreSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlRimWidth.Items.Clear();
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@TyreSize", ddlTyreSize.SelectedValue);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_Rimsize", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    ddlRimWidth.DataSource = dt;
                    ddlRimWidth.DataTextField = "RimSize";
                    ddlRimWidth.DataValueField = "RimSize";
                    ddlRimWidth.DataBind();
                }
                ddlRimWidth.Items.Insert(0, "Choose");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}