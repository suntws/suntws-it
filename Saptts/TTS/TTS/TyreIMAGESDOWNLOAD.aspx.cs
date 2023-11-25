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
using System.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using Ionic.Zip;

namespace TTS
{
    public partial class TyreIMAGESDOWNLOAD : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["img_download"].ToString() == "True")
                        {
                            Bind_DropDown("imgcategory", ddlimgCategory);
                            Bind_DropDown("Config", ddlPlatform);
                            Bind_DropDown("Brand", ddlBrand);
                            Bind_DropDown("Sidewall", ddlSidewall);
                            Bind_DropDown("TyreType", ddlType);
                            Bind_DropDown("TyreSize", ddlSize);
                            Bind_DropDown("RimSize", ddlRim);
                            CurrentPageIndex = 0;
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
            SqlParameter[] sp1 = new SqlParameter[1];
            sp1[0] = new SqlParameter("@FieldName", strField);
            DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_distinct_TyreImageCatalog_jathagam", sp1, DataAccess.Return_Type.DataTable);

            if (dt.Rows.Count > 0)
            {
                ddlName.DataSource = dt;
                ddlName.DataTextField = strField;
                ddlName.DataValueField = strField;
                ddlName.DataBind();
            }
            ddlName.Items.Insert(0, "Choose");
        }

        private void bind_image(string strImgCategory, string StrAvailableCount, string strImgName, string strImgCount, string strDelImg, string strDelCount)
        {
            try
            {
                DataTable dtImg = new DataTable();
                DataColumn col = new DataColumn("ImgName", typeof(System.String));
                dtImg.Columns.Add(col);
                col = new DataColumn("ImgWidth", typeof(System.String));
                dtImg.Columns.Add(col);
                col = new DataColumn("ImgHeight", typeof(System.String));
                dtImg.Columns.Add(col);
                col = new DataColumn("ImgSize", typeof(System.String));
                dtImg.Columns.Add(col);
                col = new DataColumn("ImgUrl", typeof(System.String));
                dtImg.Columns.Add(col);
                col = new DataColumn("tbImgName", typeof(System.String));
                dtImg.Columns.Add(col);
                col = new DataColumn("ThumbnailImgurl", typeof(System.String));
                dtImg.Columns.Add(col);

                if (StrAvailableCount != "")
                {
                    if (strDelImg != "" && strDelCount != "")
                    {
                        string[] cdelimgint = strDelImg.Split(new string[] { "~" }, StringSplitOptions.None);
                        int[] convertedItems = Array.ConvertAll<string, int>(cdelimgint, int.Parse);
                        int arrcount = cdelimgint.Length;
                        Array.Sort(convertedItems);
                        for (int icount = 1, i = 0; icount <= Convert.ToInt16(strImgCount); icount++)
                        {
                            if (icount != Convert.ToInt32(convertedItems[i]))
                            {
                                if (Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + strImgCategory + "/")))
                                    build_imgDataTable(icount, dtImg, strImgName, strImgCategory);
                            }
                            else
                            {
                                if (i < arrcount - 1)
                                    i++;
                            }
                        }
                    }
                    else
                    {
                        for (int icount = 1; icount <= Convert.ToInt32(strImgCount); icount++)
                        {
                            if (Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + strImgCategory + "/")))
                                build_imgDataTable(icount, dtImg, strImgName, strImgCategory);
                        }
                    }
                    if (dtImg.Rows.Count > 0)
                    {
                        ViewState["dtImg"] = dtImg;
                        bind_gvImagesShowingList();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bind_gvImagesShowingList()
        {
            PagedDataSource pgd = new PagedDataSource();
            DataSet ds = new DataSet();
            DataTable dtImg = ViewState["dtImg"] as DataTable;
            ds.Tables.Add(dtImg);
            pgd.DataSource = ds.Tables[0].DefaultView;
            pgd.CurrentPageIndex = CurrentPageIndex;
            pgd.AllowPaging = true;
            pgd.PageSize = 8;
            lnkNext.Visible = !(pgd.IsLastPage);
            lnkPrevious.Visible = !(pgd.IsFirstPage);
            dtimglist.DataSource = pgd;
            dtimglist.DataBind();
            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divImageLibraryView');", true);
        }

        private void build_imgDataTable(int icount, DataTable dtImage, string iName, string iCategory)
        {
            try
            {
                string strURL = ConfigurationManager.AppSettings["virdir"] + "IMAGESCATALOG" + "/" + iCategory + "/" + iName + "-" + icount + ".jpeg";
                string ThumnailstrURL = ConfigurationManager.AppSettings["virdir"] + "IMAGESCATALOG" + "/" + iCategory + "/THUMBNAILS/" + iName + "-" + icount + ".jpeg";
                FileInfo file = new FileInfo(Server.MapPath("~/IMAGESCATALOG" + "/" + iCategory + "/" + iName + "-" + icount + ".jpeg"));
                if (file.Exists)
                {
                    Bitmap img = new Bitmap(Server.MapPath("~/IMAGESCATALOG" + "/" + iCategory + "/" + iName + "-" + icount + ".jpeg"));
                    var sizeInBytes = Math.Round((Convert.ToDecimal(file.Length) / 1024), 2);
                    var imageHeight = img.Height;
                    var imageWidth = img.Width;
                    try
                    {
                        if (Convert.ToInt32(imageHeight) > 900 || Convert.ToInt32(imageWidth) > 900)
                        {
                            decimal decMultiTimes = 0;
                            if (Convert.ToInt32(imageHeight) > Convert.ToInt32(imageWidth))
                                decMultiTimes = Convert.ToInt32(imageHeight) / 900;
                            else if (Convert.ToInt32(imageWidth) > Convert.ToInt32(imageHeight))
                                decMultiTimes = Convert.ToInt32(imageWidth) / 900;
                            var imgWidth = Convert.ToInt32((Convert.ToInt32(imageWidth) / (decMultiTimes + 1)));
                            var imgHeight = Convert.ToInt32((Convert.ToInt32(imageHeight) / (decMultiTimes + 1)));
                            var decDivEqual = 0;
                            if (Convert.ToInt32(imgHeight) > Convert.ToInt32(imgWidth))
                                decDivEqual = 895 - imgHeight;
                            else if (Convert.ToInt32(imgWidth) > Convert.ToInt32(imgHeight))
                                decDivEqual = 895 - imgWidth;
                            if (img.Width > 760)
                                imageWidth = imgWidth + decDivEqual;
                            if (img.Height > 760)
                                imageHeight = imgHeight + decDivEqual;
                        }
                    }
                    catch (Exception ex)
                    {
                        imageHeight = 760;
                        imageWidth = 760;
                        Utilities.WriteToErrorLog("TTS: Image Size Convert", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                    }
                    img.Dispose();
                    DataTable dtImg1 = new DataTable();
                    DataColumn col1 = new DataColumn("ImgUrl", typeof(System.String));
                    dtImg1.Columns.Add(col1);
                    dtImg1.Rows.Add(ResolveUrl(strURL));
                    dtImage.Rows.Add(iName + "-" + icount, imageWidth + "px", imageHeight + "px", sizeInBytes + " KB", ResolveUrl(strURL), iName, ResolveUrl(ThumnailstrURL));
                    ViewState["dtimage"] = dtImg1;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public string Bind_ExistingImage(string strImgUrl, string strImgWidth, string strImgHeight)
        {
            string returnVal = string.Empty;
            returnVal = "<div style='width:900px;float:left;'><img src='" + strImgUrl + "' width='" + strImgWidth + "' height='" + strImgHeight + "' /></div>";
            return returnVal;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                string strCategory = string.Empty;
                string strPlatform = string.Empty;
                string strBrand = string.Empty;
                string strSidewall = string.Empty;
                string strType = string.Empty;
                string strSize = string.Empty;
                string strRim = string.Empty;

                strCategory = ddlimgCategory.SelectedItem.Text != "Choose" ? " imgcategory='" + ddlimgCategory.SelectedItem.Text + "'" : "";
                strPlatform = ddlPlatform.SelectedItem.Text != "Choose" ? " config='" + ddlPlatform.SelectedItem.Text + "'" : "";
                strBrand = ddlBrand.SelectedItem.Text != "Choose" ? " brand='" + ddlBrand.SelectedItem.Text + "'" : "";
                strSidewall = ddlSidewall.SelectedItem.Text != "Choose" ? " sidewall='" + ddlSidewall.SelectedItem.Text + "'" : "";
                strType = ddlType.SelectedItem.Text != "Choose" ? " tyretype='" + ddlType.SelectedItem.Text + "'" : "";
                strSize = ddlSize.SelectedItem.Text != "Choose" ? " tyresize='" + ddlSize.SelectedItem.Text + "'" : "";
                strRim = ddlRim.SelectedItem.Text != "Choose" ? " rimsize='" + ddlRim.SelectedItem.Text + "'" : "";

                string strCondition = "select imgcategory,config,brand,sidewall,tyretype,tyresize,rimsize,imgcount,imgname,Delcount,DelImg from TyreImageCatalog where imgstatus=1 ";
                if (strCategory != "" || strPlatform != "" || strBrand != "" || strSidewall != "" || strType != "" || strSize != "" || strRim != "")
                {
                    strCondition += strCategory != "" ? " and " + strCategory : "";
                    strCondition += strPlatform != "" ? " and " + strPlatform : "";
                    strCondition += strBrand != "" ? " and " + strBrand : "";
                    strCondition += strSidewall != "" ? " and " + strSidewall : "";
                    strCondition += strType != "" ? " and " + strType : "";
                    strCondition += strSize != "" ? " and " + strSize : "";
                    strCondition += strRim != "" ? " and " + strRim : "";
                }
                if (strCondition != "")
                {
                    DataTable dtImgList = (DataTable)daTTS.ExecuteReader(strCondition, DataAccess.Return_Type.DataTable);
                    if (dtImgList.Rows.Count > 0)
                    {
                        ViewState["dtImgList"] = dtImgList;
                        bind_gvImgList();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bind_gvImgList()
        {
            gvImageLibraryList.DataSource = null;
            gvImageLibraryList.DataBind();
            DataTable dtImgList = ViewState["dtImgList"] as DataTable;
            gvImageLibraryList.DataSource = dtImgList;
            gvImageLibraryList.DataBind();
        }

        protected void gvImageLibraryList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                bind_gvImgList();
                gvImageLibraryList.PageIndex = e.NewPageIndex;
                gvImageLibraryList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkShowImg_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                hdnSelectIndex.Value = Convert.ToString(clickedRow.RowIndex);
                Build_SelectImgDetails(clickedRow);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Build_SelectImgDetails(GridViewRow row)
        {
            try
            {
                string strHead1 = row.Cells[0].Text != "" && row.Cells[0].Text != "&nbsp;" ? "CATEGORY- " + row.Cells[0].Text : "";
                strHead1 += row.Cells[1].Text != "" && row.Cells[1].Text != "&nbsp;" ? "<br/>PLATFORM- " + row.Cells[1].Text : "";
                string strHead2 = row.Cells[2].Text != "" && row.Cells[2].Text != "&nbsp;" ? "BRAND- " + row.Cells[2].Text : "";
                strHead2 += row.Cells[3].Text != "" && row.Cells[3].Text != "&nbsp;" ? "<br/>SIDEWALL- " + row.Cells[3].Text : "";
                string strHead3 = row.Cells[4].Text != "" && row.Cells[4].Text != "&nbsp;" ? "TYPE- " + row.Cells[4].Text : "";
                strHead3 += row.Cells[5].Text != "" && row.Cells[5].Text != "&nbsp;" ? "<br/>SIZE- " + row.Cells[5].Text : "";
                strHead3 += row.Cells[6].Text != "" && row.Cells[6].Text != "&nbsp;" ? "<br/>RIM- " + row.Cells[6].Text : "";
                hdnImgCategory.Value = row.Cells[0].Text;
                lblListHeading1.Text = strHead1;
                lblListHeading2.Text = strHead2;
                lblListHeading3.Text = strHead3;
                HiddenField hdnImgName = row.FindControl("hdnImgName") as HiddenField;
                HiddenField hdnImgCount = row.FindControl("hdnImgCount") as HiddenField;
                HiddenField hdnDelCount = row.FindControl("hdnDelCount") as HiddenField;
                HiddenField hdnDelImg = row.FindControl("hdnDelImg") as HiddenField;
                Label lblAvailableImgCount = row.FindControl("lblAvailableImgCount") as Label;
                hdnimgname.Value = hdnImgName.Value;
                bind_image(row.Cells[0].Text, lblAvailableImgCount.Text, hdnImgName.Value, hdnImgCount.Value, hdnDelImg.Value, hdnDelCount.Value);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divImageLibraryView');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string zipname = "IMAGES_" + DateTime.Now.ToString("yyyy-MMM-dd-HHmmss") + "_" + Request.Cookies["TTSUser"].Value;
                string zipSavePath = Server.MapPath("~/IMAGESCATALOG/DOWNLOADIMAGES/" + zipname + ".zip");
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("images");
                    foreach (DataListItem item in dtimglist.Items)
                    {
                        CheckBox chbox = item.FindControl("chkimage") as CheckBox;
                        HiddenField hdndturl = item.FindControl("hdndturl") as HiddenField;
                        if (chbox.Checked == true)
                        {
                            if (Request.Url.Host.ToLower() == "www.suntws.com")
                                hdndturl.Value = hdndturl.Value.Replace("/tts/", "/");
                            string filePath = Server.MapPath("~/" + hdndturl.Value);
                            zip.AddFile(filePath, "images");
                        }
                    }
                    Response.Clear();
                    Response.BufferOutput = false;
                    //if (!Directory.Exists(Server.MapPath("~/IMAGESCATALOG/DOWNLOADIMAGES/")))
                    //    Directory.CreateDirectory(Server.MapPath("~/IMAGESCATALOG/DOWNLOADIMAGES/"));
                    //zip.Save(zipSavePath);
                    //zip.Dispose();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipname + ".zip");
                    zip.Save(Response.OutputStream);
                    Response.End();
                }
                //Response.ContentType = "application/zip";
                //Response.AddHeader("content-disposition", "attachment; filename=" + zipname + ".zip");
                //Response.WriteFile(zipSavePath);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                if (ViewState["pg"] == null)
                    return 0;
                else
                    return Convert.ToInt16(ViewState["pg"]);
            }
            set
            {
                ViewState["pg"] = value;
            }
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            CurrentPageIndex++;
            bind_gvImagesShowingList();
        }

        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            CurrentPageIndex--;
            bind_gvImagesShowingList();
        }
    }
}
