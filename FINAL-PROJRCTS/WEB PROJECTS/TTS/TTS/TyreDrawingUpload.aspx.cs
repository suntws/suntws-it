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

namespace TTS
{
    public partial class TyreDrawingUpload : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dwg_upload"].ToString() == "True")
                        {
                            if (Request["aid"] != null && Request["aid"].ToString() != "")
                            {
                                btnOverWrite.Style.Add("display", "none");
                                Bind_DropDown("Config", ddlPlatform);
                                Bind_DropDown("Brand", ddlBrand);
                                Bind_DropDown("Sidewall", ddlSidewall);
                                Bind_DropDown("TyreType", ddlType);
                                Bind_DropDown("TyreSize", ddlSize);
                                Bind_DropDown("TyreRim", ddlRim);
                                Bind_DropDown1(ddlETRTOREF, "1");
                                Bind_DropDown1(ddlRIMTYPE, "2");
                                Bind_DropDown1(ddlNoOfHoles, "3");
                                Bind_DropDown1(ddlPCD, "4");
                                Bind_DropDown1(ddlBOREDIA, "5");
                                Bind_DropDown1(ddlHoleDia, "6");

                                DataTable dtCust = (DataTable)daCOTS.ExecuteReader("select distinct custfullname from usermaster where userstatus=1 order by custfullname", DataAccess.Return_Type.DataTable);
                                if (dtCust.Rows.Count > 0)
                                {
                                    ddlCustomerSpecific.DataSource = dtCust;
                                    ddlCustomerSpecific.DataTextField = "custfullname";
                                    ddlCustomerSpecific.DataValueField = "custfullname";
                                    ddlCustomerSpecific.DataBind();
                                }
                                ddlCustomerSpecific.Items.Insert(0, "Choose");
                                DataTable dt = new DataTable();
                                if (Request["aid"].ToString() == "cus")
                                {
                                    dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_TyreDrawingRequest", DataAccess.Return_Type.DataTable);
                                    lblhead.Text = "TYRE DRAWING UPLOAD FOR CUSTOMER";
                                    hdnDwgCategory.Value = "CUSTOMER";
                                }
                                else if (Request["aid"].ToString() == "sup")
                                {
                                    dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_TyreDrawingRequest_supplier", DataAccess.Return_Type.DataTable);
                                    lblhead.Text = "TYRE DRAWING UPLOAD FOR SUPPLIER";
                                    hdnDwgCategory.Value = "SUPPLIER";
                                }
                                if (dt.Rows.Count > 0)
                                {

                                    gvDwgRequest.DataSource = dt;
                                    gvDwgRequest.DataBind();
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                        lblErrMsgcontent.Text = "User privilege disabled. Please Contact administrator";
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

        private void Bind_DropDown(string strField, DropDownList ddlName)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@FieldName", strField);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_distinct_processid_jathagam", sp1, DataAccess.Return_Type.DataTable);

                if (dt.Rows.Count > 0)
                {
                    ddlName.DataSource = dt;
                    ddlName.DataTextField = strField;
                    ddlName.DataValueField = strField;
                    ddlName.DataBind();
                }
                ddlName.Items.Insert(0, "Choose");
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
        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
                if (hdnselectedrow.Value != "")
                {
                    var previousRowIndex = Convert.ToInt32(hdnselectedrow.Value);
                    GridViewRow PreviousRow = gvDwgRequest.Rows[previousRowIndex];
                    PreviousRow.BackColor = System.Drawing.Color.White;
                }
                row.BackColor = System.Drawing.Color.Aqua;
                hdnselectedrow.Value = row.RowIndex.ToString();

                Label lblTyreSize = row.FindControl("lblTyreSize") as Label;
                Label lblRimWidth = row.FindControl("lblRimWidth") as Label;
                Label lblMountHoles = row.FindControl("lblMountHoles") as Label;
                Label lblMountHolesDia = row.FindControl("lblMountHolesDia") as Label;

                Label lblPCD = row.FindControl("lblPCD") as Label;
                Label lblBoreDia = row.FindControl("lblBoreDia") as Label;
                HiddenField hdnSlno = row.FindControl("hdnSlno") as HiddenField;
                HiddenField hdnRimType = row.FindControl("hdnRimType") as HiddenField;
                HiddenField hdnDwgRemarks = row.FindControl("hdnDwgRemarks") as HiddenField;
                Label lblCustomerName = row.FindControl("lblCustomerName") as Label;
                hdnRequestNo.Value = hdnSlno.Value;
                ListItem list1 = ddlSize.Items.FindByText(lblTyreSize.Text);
                if (list1 != null)
                {
                    ddlSize.Items.FindByText(ddlSize.SelectedItem.Text).Selected = false;
                    list1.Selected = true;
                }
                if (lblRimWidth.Text != "")
                {
                    ListItem list2 = ddlRim.Items.FindByText(lblRimWidth.Text);
                    if (list2 != null)
                    {
                        ddlRim.Items.FindByText(ddlRim.SelectedItem.Text).Selected = false;
                        list2.Selected = true;
                        ddlRim.Enabled = false;
                    }
                }
                ListItem list3 = ddlRIMTYPE.Items.FindByText(hdnRimType.Value);
                if (list3 != null)
                {
                    ddlRIMTYPE.Items.FindByText(ddlRIMTYPE.SelectedItem.Text).Selected = false;
                    list3.Selected = true;
                }
                ListItem list4 = ddlNoOfHoles.Items.FindByText(lblMountHoles.Text);
                if (list4 != null)
                {
                    ddlNoOfHoles.Items.FindByText(ddlNoOfHoles.SelectedItem.Text).Selected = false;
                    list4.Selected = true;
                }
                ListItem list5 = ddlHoleDia.Items.FindByText(lblMountHolesDia.Text);
                if (list5 != null)
                {
                    ddlHoleDia.Items.FindByText(ddlHoleDia.SelectedItem.Text).Selected = false;
                    list5.Selected = true;
                }
                ListItem list6 = ddlPCD.Items.FindByText(lblPCD.Text);
                if (list6 != null)
                {
                    ddlPCD.Items.FindByText(ddlPCD.SelectedItem.Text).Selected = false;
                    list6.Selected = true;
                }
                ListItem list7 = ddlBOREDIA.Items.FindByText(lblBoreDia.Text);
                if (list7 != null)
                {
                    ddlBOREDIA.Items.FindByText(ddlBOREDIA.SelectedItem.Text).Selected = false;
                    list7.Selected = true;
                }
                ListItem list8 = ddlCustomerSpecific.Items.FindByText(lblCustomerName.Text);
                if (list8 != null)
                {
                    ddlCustomerSpecific.Items.FindByText(ddlCustomerSpecific.SelectedItem.Text).Selected = false;
                    list8.Selected = true;
                }
                ddlSize.Enabled = false;
                ddlRIMTYPE.Enabled = false;
                ddlNoOfHoles.Enabled = false;
                ddlHoleDia.Enabled = false;
                ddlPCD.Enabled = false;
                ddlBOREDIA.Enabled = false;
                ddlCustomerSpecific.Enabled = false;
                lblDwgRemarks.Text = hdnDwgRemarks.Value == "" ? "" : "<span class='headCss' style='width: 100px;'>WHEEL PAINTING / PACKAGING REQUIREMENTS : </span>" + hdnDwgRemarks.Value;
                divsave.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dlExistDwg.DataSource = null;
                dlExistDwg.DataBind();
                hdnETRTOREF.Value = ddlETRTOREF.SelectedItem.Text == "ADD NEW ENTRY" ? txtETRTOREF.Text : ddlETRTOREF.SelectedItem.Text;
                hdnRIMTYPE.Value = ddlRIMTYPE.SelectedItem.Text == "ADD NEW ENTRY" ? txtRIMTYPE.Text : ddlRIMTYPE.SelectedItem.Text;
                hdnNoOfHoles.Value = ddlNoOfHoles.SelectedItem.Text == "ADD NEW ENTRY" ? txtNoOfHoles.Text : ddlNoOfHoles.SelectedItem.Text;
                hdnPCD.Value = ddlPCD.SelectedItem.Text == "ADD NEW ENTRY" ? txtPCD.Text : ddlPCD.SelectedItem.Text;
                hdnBOREDIA.Value = ddlBOREDIA.SelectedItem.Text == "ADD NEW ENTRY" ? txtBOREDIA.Text : ddlBOREDIA.SelectedItem.Text;
                hdnHoleDia.Value = ddlHoleDia.SelectedItem.Text == "ADD NEW ENTRY" ? txtHoleDia.Text : ddlHoleDia.SelectedItem.Text;
                string imgName = string.Empty;
                string strimgName = string.Empty;
                Int32 imgCount = 0;
                if (!Directory.Exists(Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/")))
                    Directory.CreateDirectory(Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/"));
                DataTable dtImg = Get_DwgCount_DwgName_FromDB();
                if (dtImg.Rows.Count > 0)
                {
                    hdncountimg.Value = dtImg.Rows[0]["pdfcount"].ToString();
                    hdnfiles.Value = dtImg.Rows[0]["PRODUCTDRAWING"].ToString();
                    lblErrMsg.Text = "Drawing file already exist belong this catagory";
                    DataTable dtDwg = new DataTable();
                    DataColumn col = new DataColumn("filename", typeof(System.String));
                    dtDwg.Columns.Add(col);
                    col = new DataColumn("FILEURL", typeof(System.String));
                    dtDwg.Columns.Add(col);
                    col = new DataColumn("FILEname1", typeof(System.String));
                    dtDwg.Columns.Add(col);
                    string strval = dtImg.Rows[0]["pdfcount"].ToString();
                    if (Directory.Exists(Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/")))
                    {
                        string filetextname = dtImg.Rows[0]["PRODUCTDRAWING"].ToString();
                        string strURL = ConfigurationManager.AppSettings["virdir"] + "TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/" + filetextname + "-" + dtImg.Rows[0]["pdfcount"].ToString() + ".pdf";
                        dtDwg.Rows.Add(ResolveUrl(strURL), dtImg.Rows[0]["pdfcount"].ToString(), filetextname + "-" + dtImg.Rows[0]["pdfcount"].ToString() + ".pdf");
                    }
                    if (dtDwg.Rows.Count > 0)
                    {
                        dlExistDwg.DataSource = dtDwg;
                        dlExistDwg.DataBind();
                    }
                    btnOverWrite.Style.Add("display", "block");
                    btnSave.Style.Add("display", "none");
                    divsave.Style.Add("display", "block");
                }
                else
                {
                    save_ddl();
                    hdnorginalfilename.Value = fupTDS.FileName;
                    imgCount = 0;
                    imgName = ddlPlatform.SelectedItem.Text != "Choose" ? ddlPlatform.SelectedItem.Text + "-" : "";
                    imgName += ddlBrand.SelectedItem.Text != "Choose" ? ddlBrand.SelectedItem.Text + "-" : "";
                    imgName += ddlSidewall.SelectedItem.Text != "Choose" ? ddlSidewall.SelectedItem.Text + "-" : "";
                    imgName += ddlType.SelectedItem.Text != "Choose" ? ddlType.SelectedItem.Text + "-" : "";
                    imgName += ddlSize.SelectedItem.Text != "Choose" ? ddlSize.SelectedItem.Text + "-" : "";
                    imgName += ddlRim.SelectedItem.Text != "Choose" ? ddlRim.SelectedItem.Text + "-" : "";
                    imgName += hdnETRTOREF.Value != "Choose" ? hdnETRTOREF.Value + "-" : "";
                    imgName += hdnRIMTYPE.Value != "Choose" ? hdnRIMTYPE.Value + "-" : "";
                    imgName += hdnNoOfHoles.Value != "Choose" ? hdnNoOfHoles.Value + "-" : "";
                    imgName += hdnPCD.Value != "Choose" ? hdnPCD.Value + "-" : "";
                    imgName += hdnBOREDIA.Value != "Choose" ? hdnBOREDIA.Value + "-" : "";
                    imgName += hdnHoleDia.Value != "Choose" ? hdnHoleDia.Value + "-" : "";
                    imgName += "DRAWING";
                    if (imgName.Contains("/"))
                        strimgName = imgName.Replace("/", "").Replace(" ", "_");
                    else
                        strimgName = imgName;
                    insert_save(strimgName, imgCount);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void insert_save(string imgName, int imgCount)
        {
            try
            {
                string path = Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/");
                imgCount++;
                string strsave = path + imgName + "-" + imgCount + ".pdf";
                fupTDS.SaveAs(strsave);
                if (imgCount > 0)
                {
                    SqlParameter[] sp = new SqlParameter[24];
                    sp[0] = new SqlParameter("@config", ddlPlatform.SelectedItem.Text != "Choose" ? ddlPlatform.SelectedItem.Text : "");
                    sp[1] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text != "Choose" ? ddlSize.SelectedItem.Text : "");
                    sp[2] = new SqlParameter("@rimwidth", ddlRim.SelectedItem.Text != "Choose" ? ddlRim.SelectedItem.Text : "");
                    sp[3] = new SqlParameter("@tyretype", ddlType.SelectedItem.Text != "Choose" ? ddlType.SelectedItem.Text : "");
                    sp[4] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text != "Choose" ? ddlBrand.SelectedItem.Text : "");
                    sp[5] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text != "Choose" ? ddlSidewall.SelectedItem.Text : "");
                    sp[6] = new SqlParameter("@processid", "-");
                    sp[7] = new SqlParameter("@ETRTOREF", hdnETRTOREF.Value != "Choose" ? hdnETRTOREF.Value : "");
                    sp[8] = new SqlParameter("@RIMTYPE", hdnRIMTYPE.Value != "Choose" ? hdnRIMTYPE.Value : "");
                    sp[9] = new SqlParameter("@NoOfHoles", hdnNoOfHoles.Value != "Choose" ? hdnNoOfHoles.Value : "");
                    sp[10] = new SqlParameter("@PCD", hdnPCD.Value != "Choose" ? hdnPCD.Value : "");
                    sp[11] = new SqlParameter("@CUSTOMERSPECIFIC", ddlCustomerSpecific.SelectedItem.Text != "Choose" ? ddlCustomerSpecific.SelectedItem.Text : "");
                    sp[12] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    sp[13] = new SqlParameter("@BOREDIA", hdnBOREDIA.Value != "Choose" ? hdnBOREDIA.Value : "");
                    sp[14] = new SqlParameter("@SizeCategory", hdnorginalfilename.Value);//pdf file name
                    sp[15] = new SqlParameter("@pdfCount", imgCount);
                    sp[16] = new SqlParameter("@pdfname", imgName);
                    sp[17] = new SqlParameter("@filecategory", ddlFileCategory.SelectedItem.Text != "Choose" ? ddlFileCategory.SelectedItem.Text : "");
                    sp[18] = new SqlParameter("@pdfcountstr", imgCount.ToString());
                    sp[19] = new SqlParameter("@HoleDia", hdnHoleDia.Value != "Choose" ? hdnHoleDia.Value : "");
                    sp[20] = new SqlParameter("@DrwaingNo", txtDrwaingNo.Text);
                    sp[21] = new SqlParameter("@UploadRemarks", txtDwgRemarks.Text.Replace("\r\n", "~"));
                    sp[22] = new SqlParameter("@RequestNo", hdnRequestNo.Value);
                    sp[23] = new SqlParameter("@DwgCategory", hdnDwgCategory.Value);
                    int resp = daTTS.ExecuteNonQuery_SP("sp_ins_TyreDrawingCataloge", sp);
                    if (resp > 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JEMsg2", "bind_errmsg('SAVE SUCCESSFULLY');", true);
                        Response.Redirect("TyreDrawingUpload.aspx?aid=" + Request["aid"].ToString(), false);
                    }
                    else
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JEMsg2", "bind_errmsg('Files not uploaded');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private DataTable Get_DwgCount_DwgName_FromDB()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[16];
                sp1[0] = new SqlParameter("@config", ddlPlatform.SelectedItem.Text != "Choose" ? ddlPlatform.SelectedItem.Text : "");
                sp1[1] = new SqlParameter("@ETRTOREF", hdnETRTOREF.Value != "Choose" ? hdnETRTOREF.Value : "");
                sp1[2] = new SqlParameter("@RIMTYPE", hdnRIMTYPE.Value != "Choose" ? hdnRIMTYPE.Value : "");
                sp1[3] = new SqlParameter("@NoOfHoles", hdnNoOfHoles.Value != "Choose" ? hdnNoOfHoles.Value : "");
                sp1[4] = new SqlParameter("@PCD", hdnPCD.Value != "Choose" ? hdnPCD.Value : "");
                sp1[5] = new SqlParameter("@BOREDIA", hdnBOREDIA.Value != "Choose" ? hdnBOREDIA.Value : "");
                sp1[6] = new SqlParameter("@FileCategory", ddlFileCategory.SelectedItem.Text != "Choose" ? ddlFileCategory.SelectedItem.Text : "");
                sp1[7] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text != "Choose" ? ddlBrand.SelectedItem.Text : "");
                sp1[8] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text != "Choose" ? ddlSidewall.SelectedItem.Text : "");
                sp1[9] = new SqlParameter("@tyretype", ddlType.SelectedItem.Text != "Choose" ? ddlType.SelectedItem.Text : "");
                sp1[10] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text != "Choose" ? ddlSize.SelectedItem.Text : "");
                sp1[11] = new SqlParameter("@rimwidth", ddlRim.SelectedItem.Text != "Choose" ? ddlRim.SelectedItem.Text : "");
                sp1[12] = new SqlParameter("@HoleDia", hdnHoleDia.Value != "Choose" ? hdnHoleDia.Value : "");
                sp1[13] = new SqlParameter("@CUSTOMERSPECIFIC", ddlCustomerSpecific.SelectedItem.Text != "Choose" ? ddlCustomerSpecific.SelectedItem.Text : "");
                sp1[14] = new SqlParameter("@processid", "-");
                sp1[15] = new SqlParameter("@DwgCategory", hdnDwgCategory.Value);
                dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_TyreDrawingCataloge_ImgCount", sp1, DataAccess.Return_Type.DataTable);
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "File not uploaded";
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
        }

        protected void btnOverWrite_Click(object sender, EventArgs e)
        {
            hdnorginalfilename.Value = fupTDS.FileName.ToString();
            insert_save(hdnfiles.Value, Convert.ToInt32(hdncountimg.Value));
        }

        private void save_ddl()
        {
            try
            {
                if (ddlETRTOREF.SelectedItem.Text == "ADD NEW ENTRY" && hdnETRTOREF.Value != "")
                    insert_ddl(hdnETRTOREF.Value, "1", "ETRTOREF");
                if (ddlRIMTYPE.SelectedItem.Text == "ADD NEW ENTRY" && hdnRIMTYPE.Value != "")
                    insert_ddl(hdnRIMTYPE.Value, "2", "RIMTYPE");
                if (ddlNoOfHoles.SelectedItem.Text == "ADD NEW ENTRY" && hdnNoOfHoles.Value != "")
                    insert_ddl(hdnNoOfHoles.Value, "3", "NoOfHoles");
                if (ddlPCD.SelectedItem.Text == "ADD NEW ENTRY" && hdnPCD.Value != "")
                    insert_ddl(hdnPCD.Value, "4", "PCD");
                if (ddlBOREDIA.SelectedItem.Text == "ADD NEW ENTRY" && hdnBOREDIA.Value != "")
                    insert_ddl(hdnBOREDIA.Value, "5", "BOREDIA");
                if (ddlHoleDia.SelectedItem.Text == "ADD NEW ENTRY" && hdnHoleDia.Value != "")
                    insert_ddl(hdnHoleDia.Value, "6", "HOLEDIA");
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

        protected void btnclear_Click(object sender, EventArgs e)
        {
            Response.Redirect("TyreDrawingUpload.aspx?aid=" + Request["aid"].ToString(), false);
        }


    }
}