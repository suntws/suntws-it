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
using System.Drawing.Imaging;
using System.Drawing;
namespace TTS
{
    public partial class tyreimageedit : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["img_upload"].ToString() == "True")
                        {
                            DataTable dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_ddl_ImgCategory", DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                ddlimgCategory.DataSource = dt;
                                ddlimgCategory.DataTextField = "categoryname";
                                ddlimgCategory.DataValueField = "categoryname";
                                ddlimgCategory.DataBind();
                                ddlimgCategory.Items.Insert(3, "ADD NEW CATEGORY");
                            }
                            Bind_DropDown("Config", ddlPlatform);
                            Bind_DropDown("Brand", ddlBrand);
                            Bind_DropDown("Sidewall", ddlSidewall);
                            Bind_DropDown("TyreType", ddlType);
                            Bind_DropDown("TyreSize", ddlSize);
                            Bind_DropDown("TyreRim", ddlRim);
                            if (Request["strupath"] != null && Request["filename1"].ToString() != "")
                            {
                                EDIT_IMG(Request["strupath"].ToString(), Request["filename1"].ToString());
                            }
                            else
                            {
                                DataTable dtselmenu = new DataTable();
                                dtselmenu = (DataTable)daTTS.ExecuteReader_SP("sp_sel_edit_menu_TyreImageCatalog", DataAccess.Return_Type.DataTable);
                                gvImageLibraryList.DataSource = dtselmenu;
                                gvImageLibraryList.DataBind();
                                ViewState["dtImgList"] = dtselmenu;
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

        private void EDIT_IMG(string imgcategory, string jathagam)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[2];
                DataTable dtsel = new DataTable();
                if (Request["strupath"] != null && Request["filename1"].ToString() != "")
                {
                    hdnimgurl.Value = imgcategory;
                    string[] strSplit = jathagam.Split('~');
                    hdnimgname.Value = strSplit[1].ToString();
                    hdnimgCategory.Value = strSplit[0].ToString();
                    hdnDelcount.Value = strSplit[2].ToString();

                    if (strSplit.Length == 3)
                    {
                        sp[0] = new SqlParameter("@imgname", strSplit[1].ToString());
                        sp[1] = new SqlParameter("@imgCategory", strSplit[0].ToString());
                    }
                }
                else
                {
                    hdnimgurl.Value = imgcategory;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divImageLibraryView');", true);
                    sp[0] = new SqlParameter("@imgname", hdnimgname.Value);
                    sp[1] = new SqlParameter("@imgCategory", hdnimgCategory.Value);
                }
                dtsel = (DataTable)daTTS.ExecuteReader_SP("sp_sel_edit_TyreImageCatalog", sp, DataAccess.Return_Type.DataTable);
                if (dtsel.Rows.Count > 0)
                {
                    ddlimgCategory.SelectedItem.Text = dtsel.Rows[0]["imgcategory"].ToString();
                    if (dtsel.Rows[0]["config"].ToString() == "&nbsp;" || dtsel.Rows[0]["config"].ToString() == "") ddlPlatform.SelectedItem.Text = "Choose"; else bind_ddldata(dtsel.Rows[0]["config"].ToString(), ddlPlatform);
                    if (dtsel.Rows[0]["sidewall"].ToString() == "&nbsp;" || dtsel.Rows[0]["sidewall"].ToString() == "") ddlSidewall.SelectedItem.Text = "Choose"; else bind_ddldata(dtsel.Rows[0]["sidewall"].ToString(), ddlSidewall);
                    if (dtsel.Rows[0]["brand"].ToString() == "&nbsp;" || dtsel.Rows[0]["brand"].ToString() == "") ddlBrand.SelectedItem.Text = "Choose"; else bind_ddldata(dtsel.Rows[0]["brand"].ToString(), ddlBrand);
                    if (dtsel.Rows[0]["tyretype"].ToString() == "&nbsp;" || dtsel.Rows[0]["tyretype"].ToString() == "") ddlType.SelectedItem.Text = "Choose"; else bind_ddldata(dtsel.Rows[0]["tyretype"].ToString(), ddlType);
                    if (dtsel.Rows[0]["tyresize"].ToString() == "&nbsp;" || dtsel.Rows[0]["tyresize"].ToString() == "") ddlSize.SelectedItem.Text = "Choose"; else bind_ddldata(dtsel.Rows[0]["tyresize"].ToString(), ddlSize);
                    if (dtsel.Rows[0]["rimsize"].ToString() == "&nbsp;" || dtsel.Rows[0]["rimsize"].ToString() == "") ddlRim.SelectedItem.Text = "Choose"; else bind_ddldata(dtsel.Rows[0]["rimsize"].ToString(), ddlRim);
                    string strDelImgNo = hdnimgurl.Value.Remove(0, hdnimgurl.Value.LastIndexOf('-') + 1);
                    string strDelImgNo1 = strDelImgNo.Length == 2 ? strDelImgNo.Substring(0, 2) : strDelImgNo.Substring(0, 1);
                    build_imgDataTable(Convert.ToInt32(strDelImgNo1), null, hdnimgname.Value, dtsel.Rows[0]["imgcategory"].ToString());
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('imgdata');", true);
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

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            DataListItem row = (DataListItem)((LinkButton)sender).NamingContainer;
            HiddenField hdndturl = row.FindControl("hdndturl") as HiddenField;
            EDIT_IMG(hdndturl.Value, "");
        }

        private void bind_gvImgList()
        {
            gvImageLibraryList.DataSource = null;
            gvImageLibraryList.DataBind();
            DataTable dtImgList = ViewState["dtImgList"] as DataTable;
            gvImageLibraryList.DataSource = dtImgList;
            gvImageLibraryList.DataBind();
        }

        protected void ddlimgCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlimgCategory.SelectedItem.Text == "ADD NEW CATEGORY")
                txtimgCategory.Visible = true;
            else
                txtimgCategory.Visible = false;
            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divImageLibraryView');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "imageupload(" + hdnwidth.Value + ", " + hdnheigth.Value + ");", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow3", "gotoPreviewDiv('imgdata');", true);

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
                string strURL = string.Empty;
                string strHead1 = row.Cells[0].Text != "" && row.Cells[0].Text != "&nbsp;" ? "CATEGORY- " + row.Cells[0].Text : "";
                strHead1 += row.Cells[1].Text != "" && row.Cells[1].Text != "&nbsp;" ? "<br/>PLATFORM- " + row.Cells[1].Text : "";
                string strHead2 = row.Cells[2].Text != "" && row.Cells[2].Text != "&nbsp;" ? "BRAND- " + row.Cells[2].Text : "";
                strHead2 += row.Cells[3].Text != "" && row.Cells[3].Text != "&nbsp;" ? "<br/>SIDEWALL- " + row.Cells[3].Text : "";
                string strHead3 = row.Cells[4].Text != "" && row.Cells[4].Text != "&nbsp;" ? "TYPE- " + row.Cells[4].Text : "";
                strHead3 += row.Cells[5].Text != "" && row.Cells[5].Text != "&nbsp;" ? "<br/>SIZE- " + row.Cells[5].Text : "";
                strHead3 += row.Cells[6].Text != "" && row.Cells[6].Text != "&nbsp;" ? "<br/>RIM- " + row.Cells[6].Text : "";
                hdnimgCategory.Value = row.Cells[0].Text;
                lblListHeading1.Text = strHead1;
                lblListHeading2.Text = strHead2;
                lblListHeading3.Text = strHead3;
                HiddenField hdnImgName = row.FindControl("hdnImgName") as HiddenField;
                HiddenField hdnImgCount = row.FindControl("hdnImgCount") as HiddenField;
                HiddenField hdnDelCount1 = row.FindControl("hdnDelCount") as HiddenField;
                HiddenField hdnDelImg = row.FindControl("hdnDelImg") as HiddenField;
                Label lblAvailableImgCount = row.FindControl("lblAvailableImgCount") as Label;
                hdnimgname.Value = hdnImgName.Value;
                ddlimgCategory.SelectedValue = row.Cells[0].Text;
                hdnDelcount.Value = hdnDelCount1.Value;
                bind_image(row.Cells[0].Text, lblAvailableImgCount.Text, hdnImgName.Value, hdnImgCount.Value, hdnDelImg.Value, hdnDelCount1.Value);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('divImageLibraryView');", true);
                //ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displayDiv('imgedit');", true);
                //ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow3", "displayDiv('imgdata');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
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
                hdnURL.Value = ConfigurationManager.AppSettings["virdir"] + "IMAGESCATALOG" + "/" + iCategory + "/" + iName + "-" + icount + ".jpeg";

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
                        if (Convert.ToInt32(imageHeight) > 760 || Convert.ToInt32(imageWidth) > 760)
                        {
                            decimal decMultiTimes = 0;
                            if (Convert.ToInt32(imageHeight) > Convert.ToInt32(imageWidth))
                                decMultiTimes = Convert.ToInt32(imageHeight) / 760;
                            else if (Convert.ToInt32(imageWidth) > Convert.ToInt32(imageHeight))
                                decMultiTimes = Convert.ToInt32(imageWidth) / 760;

                            var imgWidth = Convert.ToInt32((Convert.ToInt32(imageWidth) / (decMultiTimes + 1)));
                            var imgHeight = Convert.ToInt32((Convert.ToInt32(imageHeight) / (decMultiTimes + 1)));

                            var decDivEqual = 0;
                            if (Convert.ToInt32(imgHeight) > Convert.ToInt32(imgWidth))
                                decDivEqual = 755 - imgHeight;
                            else if (Convert.ToInt32(imgWidth) > Convert.ToInt32(imgHeight))
                                decDivEqual = 755 - imgWidth;

                            if (img.Width > 760)
                                imageWidth = imgWidth + decDivEqual;
                            if (img.Height > 760)
                                imageHeight = imgHeight + decDivEqual;
                            hdnheigth.Value = imageHeight.ToString();
                            hdnwidth.Value = imageWidth.ToString();
                        }
                        else
                        {
                            hdnheigth.Value = imageHeight.ToString();
                            hdnwidth.Value = imageWidth.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        imageHeight = 760;
                        imageWidth = 760;
                        hdnheigth.Value = imageHeight.ToString();
                        hdnwidth.Value = imageWidth.ToString();
                        Utilities.WriteToErrorLog("TTS: Image Size Convert", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                    }
                    img.Dispose();
                    if (dtImage != null)
                    {
                        DataTable dtImg1 = new DataTable();
                        DataColumn col1 = new DataColumn("ImgUrl", typeof(System.String));
                        dtImg1.Columns.Add(col1);
                        dtImg1.Rows.Add(ResolveUrl(hdnURL.Value));
                        dtImage.Rows.Add(iName + "-" + icount, imageWidth + "px", imageHeight + "px", sizeInBytes + " KB", ResolveUrl(hdnURL.Value), iName, ResolveUrl(ThumnailstrURL));
                        ViewState["dtimage"] = dtImg1;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "imageupload(" + hdnwidth.Value + ", " + hdnheigth.Value + ");", true);
                    }
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
                ddlName.Items.Insert(1, "NA");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strExistFileName = string.Empty;
            string imgcatelog = string.Empty;
            try
            {
                DataTable dtImg = Get_ImgCount_ImgName_FromDB();
                Int32 imgCount = 0; string imgName = string.Empty;
                if (dtImg.Rows.Count > 0)
                {
                    imgCount = Convert.ToInt32(dtImg.Rows[0]["imgcount"].ToString());
                    imgName = dtImg.Rows[0]["imgname"].ToString();
                }
                else
                {
                    imgCount = 0;
                    imgName = ddlPlatform.SelectedItem.Text != "Choose" ? ddlPlatform.SelectedItem.Text + "-" : "";
                    imgName += ddlBrand.SelectedItem.Text != "Choose" ? ddlBrand.SelectedItem.Text + "-" : "";
                    imgName += ddlSidewall.SelectedItem.Text != "Choose" ? ddlSidewall.SelectedItem.Text + "-" : "";
                    imgName += ddlType.SelectedItem.Text != "Choose" ? ddlType.SelectedItem.Text + "-" : "";
                    imgName += ddlSize.SelectedItem.Text != "Choose" ? ddlSize.SelectedItem.Text + "-" : "";
                    imgName += ddlRim.SelectedItem.Text != "Choose" ? ddlRim.SelectedItem.Text + "-" : "";
                    imgName += "Img";
                }
                if (ddlimgCategory.SelectedItem.Text == "ADD NEW CATEGORY")
                {
                    imgcatelog = txtimgCategory.Text;
                    SqlParameter[] sp11 = new SqlParameter[2];
                    sp11[0] = new SqlParameter("@imgcategory", imgcatelog);
                    sp11[1] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                    int resp = daTTS.ExecuteNonQuery_SP("sp_ins_ddl_ImgCategory", sp11);
                }
                else
                    imgcatelog = ddlimgCategory.SelectedItem.Text;

                if (!Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + imgcatelog + "/")))
                    Directory.CreateDirectory(Server.MapPath("~/IMAGESCATALOG" + "/" + imgcatelog + "/"));
                string strDestinationPath = Server.MapPath("~/IMAGESCATALOG" + "/" + imgcatelog + "/");

                Label lblImgName = new Label();
                lblImgName.Text = hdnimgname.Value;
                strExistFileName = lblImgName.Text;
                if (Request.Url.Host.ToLower() == "www.suntws.com")
                    hdnimgurl.Value = hdnimgurl.Value.Replace("/tts/", "/");
                string strSourcePath = Server.MapPath("~/" + hdnimgurl.Value);
                string fileName = lblImgName.Text;
                try
                {
                    if (fileName != "")
                    {
                        imgName = imgName.Replace("\"", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("\"", "");
                        imgCount++;
                        FileInfo file = new FileInfo(Server.MapPath("~/IMAGESCATALOG" + "/" + ddlimgCategory.SelectedItem.Text + "/" + imgName + "-" + imgCount + ".jpeg"));
                        if (file.Exists)
                        {
                            imgCount++;
                        }
                        FileInfo sourcefile = new FileInfo(strSourcePath);
                        string pathCompress = strDestinationPath + imgName + "-" + imgCount;
                        string strsave = pathCompress + ".jpeg";
                        sourcefile.MoveTo(strsave);
                        string imgsrcname = imgName + "-" + imgCount;
                        GenerateThumbnails(imgsrcname);

                        if (imgCount > 0)
                        {
                            SqlParameter[] sp = new SqlParameter[13];
                            sp[0] = new SqlParameter("@config", ddlPlatform.SelectedItem.Text != "Choose" ? ddlPlatform.SelectedItem.Text : "");
                            sp[1] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text != "Choose" ? ddlSize.SelectedItem.Text : "");
                            sp[2] = new SqlParameter("@rimsize", ddlRim.SelectedItem.Text != "Choose" ? ddlRim.SelectedItem.Text : "");
                            sp[3] = new SqlParameter("@tyretype", ddlType.SelectedItem.Text != "Choose" ? ddlType.SelectedItem.Text : "");
                            sp[4] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text != "Choose" ? ddlBrand.SelectedItem.Text : "");
                            sp[5] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text != "Choose" ? ddlSidewall.SelectedItem.Text : "");
                            sp[6] = new SqlParameter("@imgtype", "TYRE");
                            sp[7] = new SqlParameter("@imgcategory", imgcatelog);
                            sp[8] = new SqlParameter("@imgname", imgName);
                            sp[9] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                            sp[10] = new SqlParameter("@imgcount", Convert.ToInt32(imgCount));
                            sp[11] = new SqlParameter("@processid", "-");
                            sp[12] = new SqlParameter("@category", "Edited");
                            int resp = daTTS.ExecuteNonQuery_SP("sp_ins_TyreImageCatalog", sp);
                            string strDelImgNo = hdnDelcount.Value.Remove(0, hdnDelcount.Value.LastIndexOf('-') + 1);
                            SqlParameter[] spDel = new SqlParameter[4];
                            spDel[0] = new SqlParameter("@imgname", hdnimgname.Value);
                            spDel[1] = new SqlParameter("@imgCategory", hdnimgCategory.Value);
                            spDel[2] = new SqlParameter("@DelImg", strDelImgNo.Replace(".jpeg", ""));
                            spDel[3] = new SqlParameter("@DelUser", Request.Cookies["TTSUser"].Value);
                            resp = daTTS.ExecuteNonQuery_SP("sp_delete_TyreImageCatalog_ImgCount", spDel);
                            if (resp > 0)
                            {
                                if (Request["strupath"] != null && Request["filename1"].ToString() != "")
                                    Response.Redirect("TyreIMAGESDOWNLOAD.aspx", false);
                                else
                                {
                                    //Response.Redirect("tyreimageedit.aspx", false);
                                    gvImageLibraryList.SelectedIndex = Convert.ToInt32(hdnSelectIndex.Value);
                                    Build_SelectImgDetails(gvImageLibraryList.SelectedRow);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "1 " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message + ": " + strExistFileName);
            }
        }

        private void GenerateThumbnails(string imgname)
        {
            try
            {
                string url = hdnimgurl.Value;
                string name = url.Remove(0, url.LastIndexOf("/") + 1);
                if (!Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + ddlimgCategory.SelectedItem.Text + "/THUMBNAILS" + "/" + name)))
                {
                    string imgcatagory = ddlimgCategory.SelectedItem.Text == "ADD NEW CATEGORY" ? txtimgCategory.Text : ddlimgCategory.SelectedItem.Text;
                    string sourcePath = Server.MapPath("~/IMAGESCATALOG" + "/" + hdnimgCategory.Value + "/THUMBNAILS" + "/" + name);
                    string strDestinationPath = Server.MapPath("~/IMAGESCATALOG" + "/" + imgcatagory + "/THUMBNAILS" + "/");
                    FileInfo sourcefilethumb = new FileInfo(sourcePath);
                    string pathCompress = strDestinationPath + imgname;
                    string strsave = pathCompress + ".jpeg";

                    if (!Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + imgcatagory + "/THUMBNAILS" + "/")))
                        Directory.CreateDirectory(Server.MapPath("~/IMAGESCATALOG" + "/" + imgcatagory + "/THUMBNAILS" + "/"));
                    sourcefilethumb.MoveTo(strsave);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private DataTable Get_ImgCount_ImgName_FromDB()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[7];
                sp1[0] = new SqlParameter("@imgcategory", ddlimgCategory.SelectedItem.Text != "Choose" ? ddlimgCategory.SelectedItem.Text : "");
                sp1[1] = new SqlParameter("@config", ddlPlatform.SelectedItem.Text != "Choose" ? ddlPlatform.SelectedItem.Text : "");
                sp1[2] = new SqlParameter("@brand", ddlBrand.SelectedItem.Text != "Choose" ? ddlBrand.SelectedItem.Text : "");
                sp1[3] = new SqlParameter("@sidewall", ddlSidewall.SelectedItem.Text != "Choose" ? ddlSidewall.SelectedItem.Text : "");
                sp1[4] = new SqlParameter("@tyretype", ddlType.SelectedItem.Text != "Choose" ? ddlType.SelectedItem.Text : "");
                sp1[5] = new SqlParameter("@tyresize", ddlSize.SelectedItem.Text != "Choose" ? ddlSize.SelectedItem.Text : "");
                sp1[6] = new SqlParameter("@rimsize", ddlRim.SelectedItem.Text != "Choose" ? ddlRim.SelectedItem.Text : "");
                dt = (DataTable)daTTS.ExecuteReader_SP("sp_sel_tyreimages_count", sp1, DataAccess.Return_Type.DataTable);
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = "File not uploaded";
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dt;
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