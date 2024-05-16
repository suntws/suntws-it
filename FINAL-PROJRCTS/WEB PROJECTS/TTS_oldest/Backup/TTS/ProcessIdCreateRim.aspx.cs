using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Services;
using System.Reflection;
using System.IO;
using System.Xml;

namespace TTS
{
    public partial class ProcessIdCreateRim : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dwg_upload"].ToString() == "True")
                        {
                            Fill_ddl_RimProcessID_Data();
                            Fill_ddl_XML();
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
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
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

                    //No of Molunting & Fixing Holes
                    dtList.Clear();
                    foreach (XmlNode xNode in xmlLevelList.SelectNodes("/TyreDwg/NoOfHoles/list"))
                    { dtList.Rows.Add(xNode.Attributes["text"].Value, xNode.Attributes["key"].Value); }

                    if (dtList.Rows.Count > 0)
                    {
                        ddlNoOfMH.DataSource = dtList;
                        ddlNoOfMH.DataTextField = "text";
                        ddlNoOfMH.DataValueField = "key";
                        ddlNoOfMH.DataBind();

                        ddlNOFH.DataSource = dtList;
                        ddlNOFH.DataTextField = "text";
                        ddlNOFH.DataValueField = "key";
                        ddlNOFH.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Fill_ddl_RimProcessID_Data()
        {
            try
            {
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_sel_Rim_ProcessID_Details_List", DataAccess.Return_Type.DataTable);
                List<string> lstRimsize = new List<string>();
                List<string> lstMHpcd = new List<string>();
                List<string> lstMHdia = new List<string>();
                List<string> lstDiscOffSet = new List<string>();
                List<string> lstDiscThickness = new List<string>();
                List<string> lstFHpcd = new List<string>();
                List<string> lstFHdia = new List<string>();
                List<string> lstBoredia = new List<string>();
                List<string> lstWallThickness = new List<string>();

                lstRimsize = dt.AsEnumerable().Where(n => n.Field<string>("Rimsize").ToString() != "").Select(n => n.Field<string>("RimSize")).ToList<string>();
                lstRimsize.Insert(0, "--SELECT--");
                lstRimsize.Insert(lstRimsize.Count, "ADD NEW RIMSIZE");

                lstMHpcd = dt.AsEnumerable().Where(n => n.Field<string>("MHpcd").ToString() != "").Select(n => n.Field<string>("MHpcd")).ToList<string>();
                lstMHpcd.Insert(0, "--SELECT--");
                lstMHpcd.Insert(lstMHpcd.Count, "ADD NEW MHPCD");

                lstMHdia = dt.AsEnumerable().Where(n => n.Field<string>("MHdia").ToString() != "").Select(n => n.Field<string>("MHdia")).ToList<string>();
                lstMHdia.Insert(0, "--SELECT--");
                lstMHdia.Insert(lstMHdia.Count, "ADD NEW MHDIA");

                lstDiscThickness = dt.AsEnumerable().Where(n => n.Field<string>("DiscThickness").ToString() != "").Select(n => n.Field<string>("DiscThickness")).ToList<string>();
                lstDiscThickness.Insert(0, "--SELECT--");
                lstDiscThickness.Insert(lstDiscThickness.Count, "ADD NEW DT");

                lstDiscOffSet = dt.AsEnumerable().Where(n => n.Field<string>("DiscOffSet").ToString() != "").Select(n => n.Field<string>("DiscOffSet")).ToList<string>();
                lstDiscOffSet.Insert(0, "--SELECT--");
                lstDiscOffSet.Insert(lstDiscOffSet.Count, "ADD NEW DO");

                lstFHpcd = dt.AsEnumerable().Where(n => n.Field<string>("FHpcd").ToString() != "").Select(n => n.Field<string>("FHpcd")).ToList<string>();
                lstFHpcd.Insert(0, "--SELECT--");
                lstFHpcd.Insert(lstFHpcd.Count, "ADD NEW FHPCD");

                lstFHdia = dt.AsEnumerable().Where(n => n.Field<string>("FHdia").ToString() != "").Select(n => n.Field<string>("FHdia")).ToList<string>();
                lstFHdia.Insert(0, "--SELECT--");
                lstFHdia.Insert(lstFHdia.Count, "ADD NEW FHD");

                lstBoredia = dt.AsEnumerable().Where(n => n.Field<string>("Boredia").ToString() != "").Select(n => n.Field<string>("Boredia")).ToList<string>();
                lstBoredia.Insert(0, "--SELECT--");
                lstBoredia.Insert(lstBoredia.Count, "ADD NEW BD");

                lstWallThickness = dt.AsEnumerable().Where(n => n.Field<string>("WallThickness").ToString() != "").Select(n => n.Field<string>("WallThickness")).ToList<string>();
                lstWallThickness.Insert(0, "--SELECT--");
                lstWallThickness.Insert(lstWallThickness.Count, "ADD NEW WT");

                dt = null;
                ddlRimSize.DataSource = lstRimsize;
                ddlRimSize.DataBind();

                ddlMHPCD.DataSource = lstMHpcd;
                ddlMHPCD.DataBind();

                ddlMHDIA.DataSource = lstMHdia;
                ddlMHDIA.DataBind();

                ddlDO.DataSource = lstDiscOffSet;
                ddlDO.DataBind();

                ddlDT.DataSource = lstDiscThickness;
                ddlDT.DataBind();

                ddlFPcd.DataSource = lstFHpcd;
                ddlFPcd.DataBind();

                ddlFHD.DataSource = lstFHdia;
                ddlFHD.DataBind();

                ddlBD.DataSource = lstBoredia;
                ddlBD.DataBind();

                ddlWT.DataSource = lstWallThickness;
                ddlWT.DataBind();

                gv_RimProcessID.DataSource = null;
                gv_RimProcessID.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnTriggerGrid_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JEnable", "CtrlEnable();", true);
                lblErrMsg.Text = "";
                lblSaveMsg.Text = "";
                lblNoOfRecords.Text = "";
                hdnRimEdcNo.Value = "";
                gv_RimProcessID.DataSource = null;
                gv_RimProcessID.DataBind();
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@RimSize", ddlRimSize.SelectedItem.Text != "--SELECT--" ? ddlRimSize.SelectedItem.Text : ""), 
                    new SqlParameter("@RimType", ddlRimtype.SelectedItem.Text != "--SELECT--" ? ddlRimtype.SelectedItem.Text : ""), 
                    new SqlParameter("@NoOfPiece", ddlRimtype.SelectedItem.Text == "MULTI PIECE" ? ddlNoofpiece.SelectedItem.Text : ""), 
                    new SqlParameter("@TyreCategory", ddlTyreType.SelectedItem.Text != "--SELECT--" ? ddlTyreType.SelectedItem.Text : ""), 
                    new SqlParameter("@Piloted", ddlPiloted.SelectedItem.Text != "--SELECT--" ? ddlPiloted.SelectedItem.Text : ""), 
                    new SqlParameter("@NoOfMH", ddlNoOfMH.SelectedItem.Text != "--SELECT--" ? ddlNoOfMH.SelectedItem.Text : ""), 
                    new SqlParameter("@MHpcd", ddlMHPCD.SelectedItem.Text != "--SELECT--" ? ddlMHPCD.SelectedItem.Text : ""), 
                    new SqlParameter("@MHdia", ddlMHDIA.SelectedItem.Text != "--SELECT--" ? ddlMHDIA.SelectedItem.Text : ""), 
                    new SqlParameter("@MHtype", ddlMHT.SelectedItem.Text != "--SELECT--" ? ddlMHT.SelectedItem.Text : ""), 
                    new SqlParameter("@Radius", ddlMHT.SelectedItem.Text != "--SELECT--" && ddlMHT.SelectedItem.Text == "COUNTERSINK SPHERICAL" ? 
                        (ddlRadius.SelectedItem.Text !="Choose"? ddlRadius.SelectedItem.Text : "") : ""), 
                    new SqlParameter("@Angle", ddlMHT.SelectedItem.Text != "--SELECT--" && ddlMHT.SelectedItem.Text == "COUNTERSINK CONICAL" ? 
                        (ddlangle.SelectedItem.Text !="Choose" ? ddlangle.SelectedValue : "") : ""), 
                    new SqlParameter("@DiscOffSet", ddlDO.SelectedItem.Text != "--SELECT--" ? ddlDO.SelectedItem.Text : ""), 
                    new SqlParameter("@DiscThickness", ddlDT.SelectedItem.Text != "--SELECT--" ? ddlDT.SelectedItem.Text : ""), 
                    new SqlParameter("@NoOfFH", ddlNOFH.SelectedItem.Text != "--SELECT--" ? ddlNOFH.SelectedItem.Text : ""), 
                    new SqlParameter("@FHpcd", ddlFPcd.SelectedItem.Text != "--SELECT--" ? ddlFPcd.SelectedItem.Text : ""), 
                    new SqlParameter("@FHdia", ddlFHD.SelectedItem.Text != "--SELECT--" ? ddlFHD.SelectedItem.Text : ""), 
                    new SqlParameter("@FHType", ddlFHT.SelectedItem.Text != "--SELECT--" ? ddlFHT.SelectedItem.Text : ""), 
                    new SqlParameter("@Boredia", ddlBD.SelectedItem.Text != "--SELECT--" ? ddlBD.SelectedItem.Text : ""), 
                    new SqlParameter("@PaintingColor", ddlPC.SelectedItem.Text != "--SELECT--" ? ddlPC.SelectedItem.Text : ""), 
                    new SqlParameter("@WallThickness", ddlWT.SelectedItem.Text != "--SELECT--" ? ddlWT.SelectedItem.Text : "") 
                };
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("SP_Sel_Filtered_Rim_ProcessID_Details_v1", sp, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gv_RimProcessID.DataSource = dt;
                    gv_RimProcessID.DataBind();
                    lblNoOfRecords.Text = "FILTER WISE PROCESS-ID COUNT : " + dt.Rows.Count;

                    ScriptManager.RegisterStartupScript(Page, GetType(), "JDisable", "CtrlDisable();", true);
                    DataView dtDescView = new DataView(dt);
                    DataTable disEdc = dtDescView.ToTable(true, "EDC");
                    if (disEdc.Rows.Count == 1)
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JChange1", "edc_change();", true);

                    if (gv_RimProcessID.Rows.Count == 1)
                    {
                        hdnRimEdcNo.Value = dt.Rows[0]["EDCNO"].ToString();
                        if (dt.Rows[0]["DwgFile"].ToString() != "")
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JView", "CtrlDwgView();", true);
                        else
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JUpload", "CtrlDwgUpload();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSaveEDC_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                lblErrMsg.Text = "";
                lblSaveMsg.Text = "";
                using (DataAccess daOrderDB = new DataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString))
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@EDCNO", ((txtEdcNoProcessID.Text == "" ? hdnRimEdcNo.Value.Substring(0,5) : txtEdcNoProcessID.Text) + "R" + txtRevCount.Text)), 
                        new SqlParameter("@RimSize", ddlRimSize.SelectedItem.Text != "--SELECT--" && ddlRimSize.SelectedItem.Text != "ADD NEW RIMSIZE" ? 
                            ddlRimSize.SelectedItem.Text : txtRIMSIZE.Text), 
                        new SqlParameter("@RimType", ddlRimtype.SelectedItem.Text != "--SELECT--" ? ddlRimtype.SelectedItem.Text : ""), 
                        new SqlParameter("@NoOfPiece", ddlRimtype.SelectedItem.Text == "MULTI PIECE" ? ddlNoofpiece.SelectedItem.Text : 
                            (ddlRimtype.SelectedItem.Text == "SPLIT RIMS" ? "2" : "1")), 
                        new SqlParameter("@TyreCategory", ddlTyreType.SelectedItem.Text), 
                        new SqlParameter("@Piloted", ddlPiloted.SelectedItem.Text), 
                        new SqlParameter("@NoOfMH", ddlNoOfMH.SelectedItem.Text), 
                        new SqlParameter("@MHpcd", ddlMHPCD.SelectedItem.Text != "--SELECT--" && ddlMHPCD.SelectedItem.Text != "ADD NEW MHPCD" ? 
                            ddlMHPCD.SelectedItem.Text : txtMHPCD.Text), 
                        new SqlParameter("@MHdia", ddlMHDIA.SelectedItem.Text != "--SELECT--" && ddlMHDIA.SelectedItem.Text != "ADD NEW MHDIA" ? 
                            ddlMHDIA.SelectedItem.Text : txtMHDIA.Text), 
                        new SqlParameter("@MHtype", ddlMHT.SelectedItem.Text != "--SELECT--" ? ddlMHT.SelectedItem.Text : ""), 
                        new SqlParameter("@Radius",  ddlMHT.SelectedItem.Text == "COUNTERSINK SPHERICAL" ? ddlRadius.SelectedValue : ""), 
                        new SqlParameter("@Angle", ddlMHT.SelectedItem.Text == "COUNTERSINK CONICAL" ? ddlangle.SelectedValue : ""), 
                        new SqlParameter("@DiscOffSet", ddlDO.SelectedItem.Text != "--SELECT--" && ddlDO.SelectedItem.Text != "ADD NEW DO" ? 
                            ddlDO.SelectedItem.Text : txtDO.Text), 
                        new SqlParameter("@DiscThickness", ddlDT.SelectedItem.Text != "--SELECT--" && ddlDT.SelectedItem.Text != "ADD NEW DT" ? 
                            ddlDT.SelectedItem.Text : txtDT.Text), 
                        new SqlParameter("@NoOfFH", ddlNOFH.SelectedItem.Text), 
                        new SqlParameter("@FHpcd", ddlFPcd.SelectedItem.Text != "--SELECT--" && ddlFPcd.SelectedItem.Text != "ADD NEW FHPCD" ? 
                            ddlFPcd.SelectedItem.Text : txtFHPCD.Text), 
                        new SqlParameter("@FHdia", ddlFHD.SelectedItem.Text != "--SELECT--" && ddlFHD.SelectedItem.Text != "ADD NEW FHD" ? 
                            ddlFHD.SelectedItem.Text : txtFHD.Text), 
                        new SqlParameter("@FHType", ddlFHT.SelectedItem.Text), 
                        new SqlParameter("@Boredia", ddlBD.SelectedItem.Text != "--SELECT--" && ddlBD.SelectedItem.Text != "ADD NEW BD" ? 
                            ddlBD.SelectedItem.Text : txtBD.Text), 
                        new SqlParameter("@PaintingColor", ddlPC.SelectedItem.Text), 
                        new SqlParameter("@WallThickness", ddlWT.SelectedItem.Text != "--SELECT--" && ddlWT.SelectedItem.Text != "ADD NEW WT" ? 
                            ddlWT.SelectedItem.Text : txtWT.Text), 
                        new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value), 
                        new SqlParameter("@RimWt", txtRimWt.Text),
                        new SqlParameter("@revision_reason", (txtRevision.Text != "" ? "REASON FOR REVISION: " + txtRevision.Text : "")),
                        new SqlParameter("@ResultQuery", SqlDbType.VarChar, 8000, ParameterDirection.Output, true, 0, 0, "", DataRowVersion.Default, message)
                    };
                    message = (string)daCOTS.ExecuteScalar_SP("SP_Ins_Rim_ProcessID_Details_v1", sp);
                    if (message == "SUCCESS")
                    {
                        hdnRimEdcNo.Value = (txtEdcNoProcessID.Text + "R" + txtRevCount.Text);
                        btnUpload_Click(sender, e);
                    }
                    else
                    {
                        lblErrMsg.Text = message;
                        if (hdnRimEdcNo.Value.Substring(0, 5) == txtEdcNoProcessID.Text)
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JChange", "edc_change();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "ProcessIdCreateRim", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (FileUploadControl_Dwg.HasFile)
                {
                    using (Stream fs = FileUploadControl_Dwg.PostedFile.InputStream)
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            byte[] bytes = br.ReadBytes((Int32)fs.Length);

                            SqlParameter[] spUpd = new SqlParameter[] { 
                                new SqlParameter("@DwgFile", bytes), 
                                new SqlParameter("@DwgContentType", FileUploadControl_Dwg.PostedFile.ContentType), 
                                new SqlParameter("@DwgName", FileUploadControl_Dwg.PostedFile.FileName),
                                new SqlParameter("@EDCNO", hdnRimEdcNo.Value) 
                            };
                            daCOTS.ExecuteNonQuery_SP("sp_upd_Rim_DwgFile", spUpd);
                        }
                    }
                }
                Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "ProcessIdCreateRim", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void lnkDwg_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] bytes;
                SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@EDCNO", hdnRimEdcNo.Value) };
                DataTable dtDwg = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Rim_DwgFile", spSel, DataAccess.Return_Type.DataTable);
                if (dtDwg.Rows.Count == 1)
                {
                    bytes = (byte[])dtDwg.Rows[0]["DwgFile"];

                    this.Context.Response.ContentType = dtDwg.Rows[0]["DwgContentType"].ToString();
                    this.Context.Response.AddHeader("Content-disposition", "attachment; filename=" + dtDwg.Rows[0]["DwgName"].ToString());
                    this.Context.Response.BinaryWrite(bytes);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "ProcessIdCreateRim", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            finally
            {
            }
        }
    }
}
