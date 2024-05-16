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
    public partial class TyreDrawingDownload : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["dwg_download"].ToString() == "True")
                        {
                            if (Request["pid"].ToString() == "cus")
                                hdnDwgCategory.Value = "CUSTOMER";
                            else if (Request["pid"].ToString() == "sup")
                                hdnDwgCategory.Value = "SUPPLIER";
                            //ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow14", "displayDiv();", true);
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

        public string Bind_DrwaingPdfLink(string strFileName, string strPdfCount, string strFileCategory, string StrDwgName)
        {
            string strReValue = string.Empty;
            try
            {
                string strUrl = ConfigurationManager.AppSettings["virdir"] + "TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + strFileCategory + "/" + strFileName + "-" + strPdfCount + ".pdf";
                strReValue = "<a href='" + strUrl + "' target='_blank' style='font-size: 10px;'>VIEW</a>";
                //strReValue += "<div style='width: 65px; float: left; text-decoration: underline; color: #082DEA;cursor: pointer;font-size: 10px;' onclick='attachDownload(\"" + strUrl + "\",\"" + StrDwgName + "\")'>Download</div>";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return strReValue;
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
                if (Request["pid"].ToString() == "cus")
                    strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,DwgApprove,RIMDRAWING,ID,Pid,'HISTORY' as lnkText from TyreDrawingCataloge where pdfcount is not null and pdfcount<>DwgApprove and DwgCategory='CUSTOMER' and (RIMDRAWING is not null or RIMDRAWING='')";
                else if (Request["pid"].ToString() == "sup")
                    strCondition = "select filecategory,Config,brand,sidewall,tyretype,tyresize,rimwidth,ETRTOREF,RIMTYPE,NoOfHoles,PCD,BOREDIA,PRODUCTDRAWING,pdfcount,HoleDia,DrwaingNo,DwgApprove,RIMDRAWING,ID,Pid,'HISTORY' as lnkText from TyreDrawingCataloge where pdfcount is not null and pdfcount<>DwgApprove and DwgCategory='SUPPLIER' and (RIMDRAWING is not null or RIMDRAWING='')";
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
                    else
                        lblMsg.Text = "NO RECORDS";
                }
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
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lblDownload_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)((LinkButton)sender).NamingContainer;
                Label lblgrfilecategory = row.FindControl("lblgrfilecategory") as Label;
                HiddenField hdnsrtname = row.FindControl("hdnsrtname") as HiddenField;
                HiddenField hdnid = row.FindControl("hdnid") as HiddenField;
                HiddenField hdnstrPdfCount = row.FindControl("hdnstrPdfCount") as HiddenField;
                //HiddenField hdnDwgApprove = row.FindControl("hdnDwgApprove") as HiddenField;
                HiddenField hdnfilecategory = row.FindControl("hdnfilecategory") as HiddenField;
                Label lblDrwaingNo = row.FindControl("lblDrwaingNo") as Label;
                string strFileName = hdnsrtname.Value;
                string strPdfCount = hdnstrPdfCount.Value;
                string strFileCategory = hdnfilecategory.Value;
                string strUrl = ConfigurationManager.AppSettings["virdir"] + "TYREDRAWINGCATALOG" + "/" + hdnDwgCategory.Value + "/" + strFileCategory + "/" + strFileName + "-" + strPdfCount + ".pdf";
                SqlParameter[] sp1 = new SqlParameter[5];
                sp1[0] = new SqlParameter("@Category", lblgrfilecategory.Text);
                sp1[1] = new SqlParameter("@DarwingName", strFileName);
                sp1[2] = new SqlParameter("@PdfNo", Convert.ToInt32(strPdfCount));
                sp1[3] = new SqlParameter("@downloadBy", Request.Cookies["TTSUser"].Value);
                sp1[4] = new SqlParameter("@PdID", Convert.ToInt32(hdnid.Value));
                int resp = daTTS.ExecuteNonQuery_SP("sp_ins_DrawingDownloadHistory", sp1);
                if (resp > 0)
                {
                    string serverUrl = Server.MapPath("~/TYREDRAWINGCATALOG/" + "/" + hdnDwgCategory.Value + "/" + strFileCategory + "/");
                    string path = serverUrl + "/" + strFileName + "-" + strPdfCount + ".pdf";
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=" + lblDrwaingNo.Text.ToUpper() + "_REV" + strPdfCount + ".pdf");
                    Response.WriteFile(path);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}