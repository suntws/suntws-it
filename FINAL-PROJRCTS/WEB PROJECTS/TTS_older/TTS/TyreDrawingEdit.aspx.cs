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
using System.IO;
namespace TTS
{
    public partial class TyreDrawingEdit : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dwg_upload"].ToString() == "True")
                        {
                            if (Request["aid"] != null && Request["aid"].ToString() != "" && Request["pid"] != null && Request["pid"].ToString() != "")
                            {
                                string strHead1 = Request["aid"].ToString() == "1" ? "EDIT" : "NOT APPROVED";
                                if (Request["pid"].ToString() == "cus")
                                {

                                    lblhead.Text = "CUSTOMER DRAWING " + strHead1 + " LIST";
                                    hdnDwgCategory.Value = "CUSTOMER";
                                }
                                else if (Request["pid"].ToString() == "sup")
                                {
                                    lblhead.Text = "SUPPLIER DRAWING " + strHead1 + " LIST";
                                    hdnDwgCategory.Value = "SUPPLIER";
                                }
                                hdnqurstr.Value = Request["aid"].ToString();
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

                                Bind_DwgCatalog_DropDown("filecategory", ddlMFileCategory);
                                Bind_DwgCatalog_DropDown("Config", ddlMPlatform);
                                Bind_DwgCatalog_DropDown("Brand", ddlMBrand);
                                Bind_DwgCatalog_DropDown("Sidewall", ddlMSidewall);
                                Bind_DwgCatalog_DropDown("TyreType", ddlMGrade);
                                Bind_DwgCatalog_DropDown("TyreSize", ddlMSize);
                                Bind_DwgCatalog_DropDown("rimwidth", ddlMRimWidth);
                                Bind_DwgCatalog_DropDown("ETRTOREF", ddlMETRTOREF);
                                Bind_DwgCatalog_DropDown("RIMTYPE", ddlMRimType);
                                Bind_DwgCatalog_DropDown("NoOfHoles", ddlMStudHoles);
                                Bind_DwgCatalog_DropDown("PCD", ddlMPCD);
                                Bind_DwgCatalog_DropDown("BOREDIA", ddlMBOREDIA);
                                Bind_DwgCatalog_DropDown("HoleDia", ddlMStudHolesDia);
                                Bind_DwgCatalog_DropDown("CUSTOMERSPECIFIC", ddlMCustomerDwg);
                                DataTable dtCust = (DataTable)daCOTS.ExecuteReader("select distinct custfullname from usermaster where userstatus=1 order by custfullname", DataAccess.Return_Type.DataTable);
                                if (dtCust.Rows.Count > 0)
                                {
                                    ddlCustomerDwg.DataSource = dtCust;
                                    ddlCustomerDwg.DataTextField = "custfullname";
                                    ddlCustomerDwg.DataValueField = "custfullname";
                                    ddlCustomerDwg.DataBind();
                                }
                                ddlCustomerDwg.Items.Insert(0, "Choose");
                                if (Request["aid"].ToString() == "2")
                                    gridbind();
                                else
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "displayDiv('divmcont');", true);
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            gridbind();
            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displayDiv('divmcont');", true);
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

        private void Bind_DwgCatalog_DropDown(string strField, DropDownList ddlName)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@FieldName", strField);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_distinct_TyreDrawingCatalog_jathagam", sp1, DataAccess.Return_Type.DataTable);

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

        private void gridbind()
        {
            try
            {
                gvDwgLibraryList.DataSource = null;
                gvDwgLibraryList.DataBind();

                string strCategory = string.Empty;
                string strPlatform = string.Empty;
                string strBrand = string.Empty;
                string strSidewall = string.Empty;
                string strType = string.Empty;
                string strSize = string.Empty;
                string strRim = string.Empty;
                string strETRTOREF = string.Empty;
                string strRIMTYPE = string.Empty;
                string strNoOfHoles = string.Empty;
                string strPCD = string.Empty;
                string strBOREDIA = string.Empty;
                string strHoleDia = string.Empty;
                string strCustomer = string.Empty;

                strCategory = ddlMFileCategory.SelectedItem.Text != "Choose" ? " filecategory='" + ddlMFileCategory.SelectedItem.Text + "'" : "";
                strPlatform = ddlMPlatform.SelectedItem.Text != "Choose" ? " config='" + ddlMPlatform.SelectedItem.Text + "'" : "";
                strBrand = ddlMBrand.SelectedItem.Text != "Choose" ? " brand='" + ddlMBrand.SelectedItem.Text + "'" : "";
                strSidewall = ddlMSidewall.SelectedItem.Text != "Choose" ? " sidewall='" + ddlMSidewall.SelectedItem.Text + "'" : "";
                strType = ddlMGrade.SelectedItem.Text != "Choose" ? " tyretype='" + ddlMGrade.SelectedItem.Text + "'" : "";
                strSize = ddlMSize.SelectedItem.Text != "Choose" ? " tyresize='" + ddlMSize.SelectedItem.Text + "'" : "";
                strRim = ddlMRimWidth.SelectedItem.Text != "Choose" ? " rimwidth='" + ddlMRimWidth.SelectedItem.Text + "'" : "";
                strETRTOREF = ddlMETRTOREF.SelectedItem.Text != "Choose" ? " ETRTOREF='" + ddlMETRTOREF.SelectedItem.Text + "'" : "";
                strRIMTYPE = ddlMRimType.SelectedItem.Text != "Choose" ? " RIMTYPE='" + ddlMRimType.SelectedItem.Text + "'" : "";
                strNoOfHoles = ddlMStudHoles.SelectedItem.Text != "Choose" ? " NoOfHoles='" + ddlMStudHoles.SelectedItem.Text + "'" : "";
                strPCD = ddlMPCD.SelectedItem.Text != "Choose" ? " PCD='" + ddlMPCD.SelectedItem.Text + "'" : "";
                strBOREDIA = ddlMBOREDIA.SelectedItem.Text != "Choose" ? " BOREDIA='" + ddlMBOREDIA.SelectedItem.Text + "'" : "";
                strHoleDia = ddlMStudHolesDia.SelectedItem.Text != "Choose" ? " HoleDia='" + ddlMStudHolesDia.SelectedItem.Text + "'" : "";
                strCustomer = ddlMCustomerDwg.SelectedItem.Text != "Choose" ? " CUSTOMERSPECIFIC='" + ddlMCustomerDwg.SelectedItem.Text + "'" : "";
                string strCondition = string.Empty;
                if (Request["aid"].ToString() == "1")
                {
                    if (Request["pid"].ToString() == "cus")
                        strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,CUSTOMERSPECIFIC,DwgApprove,ID,RequestNo from TyreDrawingCataloge where (pdfcount is not null and pdfcount>0) and DwgApprove=1 and Dwgstatus=1 and DwgCategory='CUSTOMER'";
                    else if (Request["pid"].ToString() == "sup")
                        strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,CUSTOMERSPECIFIC,DwgApprove,ID,RequestNo from TyreDrawingCataloge where (pdfcount is not null and pdfcount>0) and DwgApprove=1 and Dwgstatus=1  and DwgCategory='SUPPLIER'";
                }
                else if (Request["aid"].ToString() == "2")
                {
                    if (Request["pid"].ToString() == "cus")
                        strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,CUSTOMERSPECIFIC,DwgApprove,ID,RequestNo from TyreDrawingCataloge where (pdfcount is not null and pdfcount>0) and DwgApprove=1 and Dwgstatus=0  and DwgCategory='CUSTOMER'";
                    else if (Request["pid"].ToString() == "sup")
                        strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,CUSTOMERSPECIFIC,DwgApprove,ID,RequestNo from TyreDrawingCataloge where (pdfcount is not null and pdfcount>0) and DwgApprove=1 and Dwgstatus=0  and DwgCategory='SUPPLIER'";
                }
                if (strCategory != "" || strPlatform != "" || strBrand != "" || strSidewall != "" || strType != "" || strSize != "" || strRim != "" || strETRTOREF != "" || strRIMTYPE != "" || strNoOfHoles != "" || strPCD != "" || strBOREDIA != "" || strHoleDia != "" || strCustomer != "")
                {
                    strCondition += strCategory != "" ? " and " + strCategory : "";
                    strCondition += strPlatform != "" ? " and " + strPlatform : "";
                    strCondition += strBrand != "" ? " and " + strBrand : "";
                    strCondition += strSidewall != "" ? " and " + strSidewall : "";
                    strCondition += strType != "" ? " and " + strType : "";
                    strCondition += strSize != "" ? " and " + strSize : "";
                    strCondition += strRim != "" ? " and " + strRim : "";
                    strCondition += strETRTOREF != "" ? " and " + strETRTOREF : "";
                    strCondition += strRIMTYPE != "" ? " and " + strRIMTYPE : "";
                    strCondition += strNoOfHoles != "" ? " and " + strNoOfHoles : "";
                    strCondition += strPCD != "" ? " and " + strPCD : "";
                    strCondition += strBOREDIA != "" ? " and " + strBOREDIA : "";
                    strCondition += strHoleDia != "" ? " and " + strHoleDia : "";
                    strCondition += strCustomer != "" ? " and " + strCustomer : "";
                }
                if (strCondition != "")
                {
                    DataTable dtImgList = (DataTable)daTTS.ExecuteReader(strCondition, DataAccess.Return_Type.DataTable);
                    if (dtImgList.Rows.Count > 0)
                    {
                        ViewState["dtImgList"] = dtImgList;
                        bind_gvDwgList();
                    }
                    //else
                    //  lblMsg.Text = "NO RECORDS";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bind_ddldata(string strField, DropDownList ddlName)
        {
            ListItem selectedListItem = ddlName.Items.FindByText("" + strField + "");
            if (selectedListItem != null)
            {
                ddlName.Items.FindByText(ddlName.SelectedItem.Text).Selected = false;
                selectedListItem.Selected = true;
            }
        }

        private void bind_gvDwgList()
        {
            try
            {
                gvDwgLibraryList.DataSource = null;
                gvDwgLibraryList.DataBind();
                DataTable dtImgList = ViewState["dtImgList"] as DataTable;
                gvDwgLibraryList.DataSource = dtImgList;
                gvDwgLibraryList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvDwgLibraryList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                bind_gvDwgList();
                gvDwgLibraryList.PageIndex = e.NewPageIndex;
                gvDwgLibraryList.DataBind();
                if (Request["aid"].ToString() == "1")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow3", "displayDiv('divmcont');", true);
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
                Label lblgrfilecategory = row.FindControl("lblgrfilecategory") as Label;
                Label lblgrconfig = row.FindControl("lblgrconfig") as Label;
                Label lblgrbrand = row.FindControl("lblgrbrand") as Label;
                Label lblgrsidewall = row.FindControl("lblgrsidewall") as Label;
                Label lblgrtyretype = row.FindControl("lblgrtyretype") as Label;
                Label lblgrtyresize = row.FindControl("lblgrtyresize") as Label;
                Label lblgrrimwidth = row.FindControl("lblgrrimwidth") as Label;
                Label lblgrETRTOREF = row.FindControl("lblgrETRTOREF") as Label;
                Label lblgrRIMTYPE = row.FindControl("lblgrRIMTYPE") as Label;
                Label lblgrNoOfHoles = row.FindControl("lblgrNoOfHoles") as Label;
                Label lblgrPCD = row.FindControl("lblgrPCD") as Label;
                Label lblgrBOREDIA = row.FindControl("lblgrBOREDIA") as Label;
                Label lblgrHoleDia = row.FindControl("lblgrHoleDia") as Label;
                HiddenField hdnDrwaingNo = row.FindControl("hdnDrwaingNo") as HiddenField;
                HiddenField hdnsrtname = row.FindControl("hdnsrtname") as HiddenField;
                HiddenField hdnstrcustomerspecific = row.FindControl("hdnstrcustomerspecific") as HiddenField;
                HiddenField hdnid = row.FindControl("hdnid") as HiddenField;
                HiddenField hdnstrPdfCount = row.FindControl("hdnstrPdfCount") as HiddenField;
                HiddenField hdnfilecategory = row.FindControl("hdnfilecategory") as HiddenField;
                HiddenField hdnRequestNo = row.FindControl("hdnRequestNo") as HiddenField;
                hdnRequestSlNo.Value = hdnRequestNo.Value;
                bind_ddldata(lblgrfilecategory.Text, ddlFileCategory);
                if (lblgrconfig.Text == "&nbsp;" || lblgrconfig.Text == "") ddlPlatform.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrconfig.Text, ddlPlatform);
                if (lblgrbrand.Text == "&nbsp;" || lblgrbrand.Text == "") ddlBrand.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrbrand.Text, ddlBrand);
                if (lblgrtyretype.Text == "&nbsp;" || lblgrtyretype.Text == "") ddlType.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrtyretype.Text, ddlType);
                if (lblgrsidewall.Text == "&nbsp;" || lblgrsidewall.Text == "") ddlSidewall.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrsidewall.Text, ddlSidewall);
                if (lblgrtyresize.Text == "&nbsp;" || lblgrtyresize.Text == "") ddlSize.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrtyresize.Text, ddlSize);
                if (lblgrrimwidth.Text == "&nbsp;" || lblgrrimwidth.Text == "") ddlRim.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrrimwidth.Text, ddlRim);
                if (lblgrETRTOREF.Text == "&nbsp;" || lblgrETRTOREF.Text == "") ddlETRTOREF.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrETRTOREF.Text, ddlETRTOREF);
                if (lblgrRIMTYPE.Text == "&nbsp;" || lblgrRIMTYPE.Text == "") ddlRIMTYPE.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrRIMTYPE.Text, ddlRIMTYPE);
                if (lblgrNoOfHoles.Text == "&nbsp;" || lblgrNoOfHoles.Text == "") ddlNoOfHoles.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrNoOfHoles.Text, ddlNoOfHoles);
                if (lblgrPCD.Text == "&nbsp;" || lblgrPCD.Text == "") ddlPCD.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrPCD.Text, ddlPCD);
                if (lblgrBOREDIA.Text == "&nbsp;" || lblgrBOREDIA.Text == "") ddlBOREDIA.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrBOREDIA.Text, ddlBOREDIA);
                if (lblgrHoleDia.Text == "&nbsp;" || lblgrHoleDia.Text == "") ddlHoleDia.SelectedItem.Text = "Choose"; else bind_ddldata(lblgrHoleDia.Text, ddlHoleDia);
                if (hdnDrwaingNo.Value == "&nbsp;" || hdnDrwaingNo.Value == "") txtDrwaingNo.Text = ""; else txtDrwaingNo.Text = hdnDrwaingNo.Value;
                if (hdnstrcustomerspecific.Value == "&nbsp;" || hdnstrcustomerspecific.Value == "") ddlCustomerDwg.SelectedItem.Text = "Choose"; else bind_ddldata(hdnstrcustomerspecific.Value, ddlCustomerDwg);
                hdnOfileName.Value = hdnsrtname.Value;
                string strPdfCount = hdnstrPdfCount.Value;
                hdnOCategory.Value = hdnfilecategory.Value;
                string strUrl = ConfigurationManager.AppSettings["virdir"] + "TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + hdnOCategory.Value + "/" + hdnOfileName.Value + "-" + strPdfCount + ".pdf";
                hdnurl.Value = strUrl;
                hdnID.Value = hdnid.Value;
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow14", "displayDiv('Divcont');", true);
                if (Request["aid"].ToString() == "1")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow3", "displayDiv('divmcont');", true);
            }
            catch (Exception ex) { Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message); }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dlExistDwg.DataSource = null;
                dlExistDwg.DataBind();
                string drgName = string.Empty;
                string strimgName = string.Empty;
                hdnETRTOREF.Value = ddlETRTOREF.SelectedItem.Text == "ADD NEW ENTRY" ? txtETRTOREF.Text : ddlETRTOREF.SelectedItem.Text;
                hdnRIMTYPE.Value = ddlRIMTYPE.SelectedItem.Text == "ADD NEW ENTRY" ? txtRIMTYPE.Text : ddlRIMTYPE.SelectedItem.Text;
                hdnNoOfHoles.Value = ddlNoOfHoles.SelectedItem.Text == "ADD NEW ENTRY" ? txtNoOfHoles.Text : ddlNoOfHoles.SelectedItem.Text;
                hdnPCD.Value = ddlPCD.SelectedItem.Text == "ADD NEW ENTRY" ? txtPCD.Text : ddlPCD.SelectedItem.Text;
                hdnBOREDIA.Value = ddlBOREDIA.SelectedItem.Text == "ADD NEW ENTRY" ? txtBOREDIA.Text : ddlBOREDIA.SelectedItem.Text;
                hdnHoleDia.Value = ddlHoleDia.SelectedItem.Text == "ADD NEW ENTRY" ? txtHoleDia.Text : ddlHoleDia.SelectedItem.Text;

                Int32 drgCount = 0;
                if (!Directory.Exists(Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/")))
                    Directory.CreateDirectory(Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/"));
                DataTable dtImg = Get_DwgCount_DwgName_FromDB();
                if (dtImg.Rows.Count > 0)
                {
                    hdncountimg.Value = dtImg.Rows[0]["pdfcount"].ToString();
                    hdnfiles.Value = dtImg.Rows[0]["PRODUCTDRAWING"].ToString();
                    hdnorginalfilename.Value = dtImg.Rows[0]["SizeCategory"].ToString();
                    if ((dtImg.Rows[0]["DwgApprove"].ToString() == "False" && dtImg.Rows[0]["pdfcount"].ToString() != dtImg.Rows[0]["RIMDRAWING"].ToString()) || dtImg.Rows[0]["RIMDRAWING"].ToString() != "True")
                    { save_Pdf(Convert.ToInt32(hdncountimg.Value)); }
                    else
                    {
                        lblErrMsg.Text = "Drawing file already exist belong this catagory";
                        DataTable dtDwg = new DataTable();
                        DataColumn col = new DataColumn("filename", typeof(System.String)); dtDwg.Columns.Add(col);
                        col = new DataColumn("FILEURL", typeof(System.String)); dtDwg.Columns.Add(col);
                        col = new DataColumn("FILEname1", typeof(System.String)); dtDwg.Columns.Add(col);
                        string strval = dtImg.Rows[0]["pdfcount"].ToString();
                        if (Directory.Exists(Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/")))
                        {
                            string filetextname = dtImg.Rows[0]["PRODUCTDRAWING"].ToString();
                            string strURL = ConfigurationManager.AppSettings["virdir"] + "TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/" + filetextname + "-" + dtImg.Rows[0]["pdfcount"].ToString() + ".pdf";
                            dtDwg.Rows.Add(ResolveUrl(strURL), dtImg.Rows[0]["pdfcount"].ToString(), filetextname + "-" + dtImg.Rows[0]["pdfcount"].ToString() + ".pdf");
                        }
                        if (dtDwg.Rows.Count > 0) { dlExistDwg.DataSource = dtDwg; dlExistDwg.DataBind(); }
                        btnOverWrite.Style.Add("display", "block");
                        btnSave.Style.Add("display", "none");
                        if (Request["aid"].ToString() == "1")
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow3", "displayDiv('divmcont');", true);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displayDiv('Divcont');", true);
                    }
                }
                else
                {
                    save_ddl(); drgCount = 0;
                    drgName = ddlPlatform.SelectedItem.Text != "Choose" ? ddlPlatform.SelectedItem.Text + "-" : "";
                    drgName += ddlBrand.SelectedItem.Text != "Choose" ? ddlBrand.SelectedItem.Text + "-" : "";
                    drgName += ddlSidewall.SelectedItem.Text != "Choose" ? ddlSidewall.SelectedItem.Text + "-" : "";
                    drgName += ddlType.SelectedItem.Text != "Choose" ? ddlType.SelectedItem.Text + "-" : "";
                    drgName += ddlSize.SelectedItem.Text != "Choose" ? ddlSize.SelectedItem.Text + "-" : "";
                    drgName += ddlRim.SelectedItem.Text != "Choose" ? ddlRim.SelectedItem.Text + "-" : "";
                    drgName += hdnETRTOREF.Value != "Choose" ? hdnETRTOREF.Value + "-" : "";
                    drgName += hdnRIMTYPE.Value != "Choose" ? hdnRIMTYPE.Value + "-" : "";
                    drgName += hdnNoOfHoles.Value != "Choose" ? hdnNoOfHoles.Value + "-" : "";
                    drgName += hdnPCD.Value != "Choose" ? hdnPCD.Value + "-" : "";
                    drgName += hdnBOREDIA.Value != "Choose" ? hdnBOREDIA.Value + "-" : "";
                    drgName += hdnHoleDia.Value != "Choose" ? hdnHoleDia.Value + "-" : "";
                    drgName += "DRAWING";
                    if (drgName.Contains("/")) hdnfiles.Value = drgName.Replace("/", "").Replace(" ", "_");
                    else hdnfiles.Value = drgName;
                    save_Pdf(drgCount);
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void save_Pdf(Int32 drgCount)
        {
            if (!Directory.Exists(Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/")))
                Directory.CreateDirectory(Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/"));
            string strDestinationPath = Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/");
            string strSourcePath = Server.MapPath("~/" + hdnurl.Value);
            string fileName = hdnfiles.Value;
            if (fileName != "")
            {
                try
                {
                    //fileName = fileName.Replace("\"", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("\"", "").Replace(" ", "_"); ;
                    drgCount++;
                    FileInfo file = new FileInfo(Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + ddlFileCategory.SelectedItem.Text + "/" + fileName + "-" + drgCount + ".pdf"));
                    if (file.Exists) drgCount++;
                    if (!fupTDS.HasFile)
                    {
                        FileInfo sourcefile = new FileInfo(strSourcePath);
                        string strsave = strDestinationPath + hdnfiles.Value + "-" + drgCount + ".pdf";
                        sourcefile.MoveTo(strsave);
                    }
                    else if (fupTDS.HasFile)
                    {
                        hdnorginalfilename.Value = fupTDS.FileName.ToString();
                        // FileInfo sourcefile = new FileInfo(strSourcePath); sourcefile.Delete(); 
                        string strsave = strDestinationPath + hdnfiles.Value + "-" + drgCount + ".pdf";
                        fupTDS.SaveAs(strsave);
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "2 " + ex.Message + ": ");
                }
                insert_save(fileName, drgCount);
            }
        }

        private void insert_save(string fileName, int drgCount)
        {
            try
            {
                if (drgCount > 0)
                {
                    //SqlParameter[] sp2 = new SqlParameter[2];
                    //sp2[0] = new SqlParameter("@PID", Convert.ToInt32(hdnID.Value));
                    //sp2[1] = new SqlParameter("@ApprovedBy", Request.Cookies["TTSUser"].Value);
                    //int resp = daTTS.ExecuteNonQuery_SP("sp_edit_DwgApproveHistory_ID_Wise", sp2);
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
                    sp[11] = new SqlParameter("@CUSTOMERSPECIFIC", ddlCustomerDwg.SelectedItem.Text != "Choose" ? ddlCustomerDwg.SelectedItem.Text : "");
                    sp[12] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    sp[13] = new SqlParameter("@BOREDIA", hdnBOREDIA.Value != "Choose" ? hdnBOREDIA.Value : "");
                    sp[14] = new SqlParameter("@SizeCategory", hdnorginalfilename.Value);//pdf file name
                    sp[15] = new SqlParameter("@pdfCount", drgCount);
                    sp[16] = new SqlParameter("@pdfname", fileName);
                    sp[17] = new SqlParameter("@filecategory", ddlFileCategory.SelectedItem.Text != "Choose" ? ddlFileCategory.SelectedItem.Text : "");
                    sp[18] = new SqlParameter("@pdfcountstr", drgCount.ToString());
                    sp[19] = new SqlParameter("@HoleDia", hdnHoleDia.Value != "Choose" ? hdnHoleDia.Value : "");
                    sp[20] = new SqlParameter("@DrwaingNo", txtDrwaingNo.Text);
                    sp[21] = new SqlParameter("@UploadRemarks", txtDwgRemarks.Text.Replace("\r\n", "~"));
                    sp[22] = new SqlParameter("@RequestNo", hdnRequestSlNo.Value);
                    sp[23] = new SqlParameter("@DwgCategory", hdnDwgCategory.Value);
                    int resp = daTTS.ExecuteNonQuery_SP("sp_ins_TyreDrawingCataloge", sp);
                    if (resp > 0)
                    {
                        if ((hdnOCategory.Value != ddlFileCategory.SelectedItem.Text || hdnOfileName.Value != fileName) && !fupTDS.HasFile)
                        {
                            SqlParameter[] sp1 = new SqlParameter[2];
                            sp1[0] = new SqlParameter("@filecategory", hdnOCategory.Value);
                            sp1[1] = new SqlParameter("@pdfname", hdnOfileName.Value);
                            daTTS.ExecuteNonQuery_SP("sp_edit_TyreDrawingCataloge", sp1);

                        }
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JEMsg2", "bind_errmsg('SAVE SUCCESSFULLY');", true);
                        Response.Redirect("TyreDrawingEdit.aspx?aid=" + Request["aid"].ToString() + "&pid=" + Request["pid"].ToString(), false);
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

        protected void btnOverWrite_Click(object sender, EventArgs e)
        {
            save_Pdf(Convert.ToInt32(hdncountimg.Value));
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
                sp1[13] = new SqlParameter("@CUSTOMERSPECIFIC", ddlCustomerDwg.SelectedItem.Text != "Choose" ? ddlCustomerDwg.SelectedItem.Text : "");
                sp1[14] = new SqlParameter("@processid", "-");
                sp1[15] = new SqlParameter("@DwgCategory", hdnDwgCategory.Value);
                dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_TyreDrawingCataloge_ImgCount", sp1, DataAccess.Return_Type.DataTable);
            }
            catch (Exception ex)
            {

                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
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
    }
}