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
    public partial class TyreDwgAllocation : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dwg_allocate"].ToString() == "True")
                        {
                            Bind_DropDown("filecategory", ddlFileCategory);
                            Bind_DropDown("config", ddlPlatform);
                            Bind_DropDown("brand", ddlBrand);
                            Bind_DropDown("sidewall", ddlSidewall);
                            Bind_DropDown("tyretype", ddlType);
                            Bind_DropDown("tyresize", ddlSize);
                            Bind_DropDown("rimwidth", ddlRim);
                            Bind_DropDown("ETRTOREF", ddlETRTOREF);
                            Bind_DropDown("RIMTYPE", ddlRIMTYPE);
                            Bind_DropDown("NoOfHoles", ddlNoOfHoles);
                            Bind_DropDown("PCD", ddlPCD);
                            Bind_DropDown("BOREDIA", ddlBOREDIA);
                            Bind_DropDown("HoleDia", ddlHoleDia);
                            Bind_DropDown("CUSTOMERSPECIFIC", ddlCustomerDwg);
                            if (Request["pid"].ToString() == "new")
                            {
                                if (Request["aid"].ToString() == "cus")
                                {
                                    lblheading.Text = "NEW DRAWING ALLOCATION FOR CUSTOMER ";
                                    hdnDwgCategory.Value = "CUSTOMER";
                                }
                                else if (Request["aid"].ToString() == "sup")
                                {
                                    lblheading.Text = "NEW DRAWING ALLOCATION FOR SUPPLIER";
                                    hdnDwgCategory.Value = "SUPPLIER";
                                }
                                btnSave.Text = "SAVE";
                            }
                            else if (Request["pid"].ToString() == "edit")
                            {
                                if (Request["aid"].ToString() == "cus")
                                {
                                    lblheading.Text = "EDIT DRAWING ALLOCATION FOR CUSTOMER ";
                                    hdnDwgCategory.Value = "CUSTOMER";
                                }
                                else if (Request["aid"].ToString() == "sup")
                                {
                                    lblheading.Text = "EDIT DRAWING ALLOCATION FOR SUPPLIER";
                                    hdnDwgCategory.Value = "SUPPLIER";
                                }

                                btnSave.Text = "UPDATE";
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
            try
            {
                gvDwgLibraryList.DataSource = null;
                gvDwgLibraryList.DataBind();
                string strCondition = string.Empty;
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
                strCategory = ddlFileCategory.SelectedItem.Text != "Choose" ? " filecategory='" + ddlFileCategory.SelectedItem.Text + "'" : "";
                strPlatform = ddlPlatform.SelectedItem.Text != "Choose" ? " config='" + ddlPlatform.SelectedItem.Text + "'" : "";
                strBrand = ddlBrand.SelectedItem.Text != "Choose" ? " brand='" + ddlBrand.SelectedItem.Text + "'" : "";
                strSidewall = ddlSidewall.SelectedItem.Text != "Choose" ? " sidewall='" + ddlSidewall.SelectedItem.Text + "'" : "";
                strType = ddlType.SelectedItem.Text != "Choose" ? " tyretype='" + ddlType.SelectedItem.Text + "'" : "";
                strSize = ddlSize.SelectedItem.Text != "Choose" ? " tyresize='" + ddlSize.SelectedItem.Text + "'" : "";
                strRim = ddlRim.SelectedItem.Text != "Choose" ? " rimwidth='" + ddlRim.SelectedItem.Text + "'" : "";
                strETRTOREF = ddlETRTOREF.SelectedItem.Text != "Choose" ? " ETRTOREF='" + ddlETRTOREF.SelectedItem.Text + "'" : "";
                strRIMTYPE = ddlRIMTYPE.SelectedItem.Text != "Choose" ? " RIMTYPE='" + ddlRIMTYPE.SelectedItem.Text + "'" : "";
                strNoOfHoles = ddlNoOfHoles.SelectedItem.Text != "Choose" ? " NoOfHoles='" + ddlNoOfHoles.SelectedItem.Text + "'" : "";
                strPCD = ddlPCD.SelectedItem.Text != "Choose" ? " PCD='" + ddlPCD.SelectedItem.Text + "'" : "";
                strBOREDIA = ddlBOREDIA.SelectedItem.Text != "Choose" ? " BOREDIA='" + ddlBOREDIA.SelectedItem.Text + "'" : "";
                strHoleDia = ddlHoleDia.SelectedItem.Text != "Choose" ? " HoleDia='" + ddlHoleDia.SelectedItem.Text + "'" : "";
                strCustomer = ddlCustomerDwg.SelectedItem.Text != "Choose" ? " CUSTOMERSPECIFIC='" + ddlCustomerDwg.SelectedItem.Text + "'" : "";
                if (Request["pid"].ToString() == "new")
                {
                    if (Request["aid"].ToString() == "cus")
                        strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,DwgApprove,RIMDRAWING,ID,DwgCategory from TyreDrawingCataloge where pdfcount is not null and pdfcount<>DwgApprove and DwgAllocationStatus=0  and DwgCategory='CUSTOMER' and (RIMDRAWING is not null or RIMDRAWING='')";
                    else if (Request["aid"].ToString() == "sup")
                        strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,DwgApprove,RIMDRAWING,ID,DwgCategory from TyreDrawingCataloge where pdfcount is not null and pdfcount<>DwgApprove and DwgAllocationStatus=0  and DwgCategory='SUPPLIER' and (RIMDRAWING is not null or RIMDRAWING='')";
                }
                else if (Request["pid"].ToString() == "edit")
                {
                    if (Request["aid"].ToString() == "cus")
                        strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,DwgApprove,RIMDRAWING,ID,DwgCategory from TyreDrawingCataloge where pdfcount is not null and pdfcount<>DwgApprove and DwgAllocationStatus=1  and DwgCategory='CUSTOMER' and (RIMDRAWING is not null or RIMDRAWING='')";
                    else if (Request["aid"].ToString() == "sup")
                        strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,DwgApprove,RIMDRAWING,ID,DwgCategory from TyreDrawingCataloge where pdfcount is not null and pdfcount<>DwgApprove and DwgAllocationStatus=1  and DwgCategory='SUPPLIER' and (RIMDRAWING is not null or RIMDRAWING='')";
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
                        ViewState["dtDwgList"] = dtImgList;
                        bind_gvDwgList();
                    }
                    else
                        lblMsg.Text = "NO RECORDS";
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
        private void bind_gvDwgList()
        {
            try
            {
                gvDwgLibraryList.DataSource = null;
                gvDwgLibraryList.DataBind();
                DataTable dtImgList = ViewState["dtDwgList"] as DataTable;
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

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public string Bind_DrawingPdfLink(string strFileName, string strPdfCount, string strFileCategory, string StrDwgName, string Dwgtype)
        {
            string strReValue = string.Empty;
            try
            {
                string strUrl = ConfigurationManager.AppSettings["virdir"] + "TYREDRAWINGCATALOG" + "/" + Dwgtype + "/" + strFileCategory + "/" + strFileName + "-" + strPdfCount + ".pdf";
                strReValue = "<a href='" + strUrl + "' target='_blank' style='font-size: 10px;'>VIEW</a>";
                //strReValue += "<div style='width: 65px; float: left; text-decoration: underline; color: #082DEA;cursor: pointer;font-size: 10px;' onclick='attachDownload(\"" + strUrl + "\",\"" + StrDwgName + "\")'>Download</div>";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strReValue;
        }
        protected void lnkAllocation_Click(object sender, EventArgs e)
        {
            try
            {
                gvTyreSizeList.DataSource = null;
                gvTyreSizeList.DataBind();
                GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
                Label lblgrimwidth = row.FindControl("lblgrimwidth") as Label;
                Label lblgtyresize = row.FindControl("lblgtyresize") as Label; Label lblgrfilecategory = row.FindControl("lblgrfilecategory") as Label;
                HiddenField hdnsrtname = row.FindControl("hdnsrtname") as HiddenField;
                HiddenField hdnid = row.FindControl("hdnid") as HiddenField; HiddenField hdnstrPdfCount = row.FindControl("hdnstrPdfCount") as HiddenField;
                lbltyresize.Text = lblgtyresize.Text;
                lblRimWidth.Text = lblgrimwidth.Text;
                hdnId.Value = hdnid.Value;
                hdnfilename.Value = hdnsrtname.Value;
                hdncategory.Value = lblgrfilecategory.Text;
                hdnimgcount.Value = hdnstrPdfCount.Value;
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@tyresize", lblgtyresize.Text);
                sp1[1] = new SqlParameter("@rimwidth", lblgrimwidth.Text);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_DwgAllocation_tyresize", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvTyreSizeList.DataSource = dt;
                    gvTyreSizeList.DataBind();
                    if (Request["pid"].ToString() == "edit")
                    {
                        SqlParameter[] sp = new SqlParameter[1];
                        sp[0] = new SqlParameter("@Pid", Convert.ToInt32(hdnid.Value));
                        DataTable dt1 = (DataTable)daTTS.ExecuteReader_SP("sp_sel_TyreDwgAllocation", sp, DataAccess.Return_Type.DataTable);
                        if (dt1.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dt1.Rows)
                            {
                                foreach (GridViewRow dr1 in gvTyreSizeList.Rows)
                                {
                                    Label lblgtyresize1 = dr1.FindControl("lblgtyresize") as Label;
                                    CheckBox ChkList = dr1.FindControl("ChkList") as CheckBox;
                                    if (lblgtyresize1.Text == dr["TyreSize"].ToString())
                                        ChkList.Checked = true;
                                }
                            }
                        }
                    }
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divshow');", true);

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
                string newfilename = string.Empty;
                string oldfilename = string.Empty;
                string tyresize = string.Empty;
                string oldpath = string.Empty;
                string newpath = string.Empty;
                if (lbltyresize.Text.Contains("/"))
                    oldfilename = lbltyresize.Text.Replace("/", "").Replace(" ", "_");
                else
                    oldfilename = lbltyresize.Text;
                oldpath = Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + hdncategory.Value + "/" + hdnfilename.Value + "-" + hdnimgcount.Value + ".pdf");
                FileInfo file = new FileInfo(oldpath);

                DataTable dtTyreSize = new DataTable();
                DataColumn col = new DataColumn("TyreSize", typeof(System.String));
                dtTyreSize.Columns.Add(col);
                col = new DataColumn("pdfFilename", typeof(System.String));
                dtTyreSize.Columns.Add(col);
                foreach (GridViewRow row in gvTyreSizeList.Rows)
                {
                    Label lblgtyresize = row.FindControl("lblgtyresize") as Label;
                    CheckBox ChkList = row.FindControl("ChkList") as CheckBox;
                    if (lblgtyresize.Text.Contains("/"))
                        tyresize = lblgtyresize.Text.Replace("/", "").Replace(" ", "_");
                    else
                        tyresize = lblgtyresize.Text;
                    newfilename = hdnfilename.Value.Replace(oldfilename, tyresize);
                    if (ChkList.Checked)
                    {
                        if (file.Exists)
                        {
                            newpath = Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + hdncategory.Value + "/ALC-" + newfilename + "-1.pdf");
                            FileInfo file2 = new FileInfo(newpath);
                            if (file2.Exists)
                                File.Delete(newpath);
                            File.Copy(oldpath, newpath);
                        }
                        dtTyreSize.Rows.Add(lblgtyresize.Text, "ALC-" + newfilename);
                    }
                    else
                    {
                        string delpath = Server.MapPath("~/TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + hdncategory.Value + "/ALC-" + newfilename + "-1.pdf");
                        FileInfo file1 = new FileInfo(delpath);
                        if (file1.Exists)
                            File.Delete(delpath);
                    }
                }
                if (dtTyreSize.Rows.Count > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[4];
                    sp1[0] = new SqlParameter("@PID", hdnId.Value);
                    sp1[1] = new SqlParameter("@RimWidth", lblRimWidth.Text);
                    sp1[2] = new SqlParameter("@DtTyreDwgAllocationTyreSize", dtTyreSize);
                    sp1[3] = new SqlParameter("@CreatedBy", Request.Cookies["TTSUser"].Value);
                    int resp = daTTS.ExecuteNonQuery_SP("sp_ins_TyreDwgAllocation", sp1);
                    if (resp > 0)
                        Response.Redirect("TyreDwgAllocation.aspx?pid=" + Request["pid"].ToString() + "&aid=" + Request["aid"].ToString(), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}