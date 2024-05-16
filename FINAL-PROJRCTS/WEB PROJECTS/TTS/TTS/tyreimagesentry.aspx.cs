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
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TTS
{
    public partial class tyreimagesentry : System.Web.UI.Page
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
                            }

                            Bind_DropDown("Config", ddlPlatform);
                            Bind_DropDown("Brand", ddlBrand);
                            Bind_DropDown("Sidewall", ddlSidewall);
                            Bind_DropDown("TyreType", ddlType);
                            Bind_DropDown("TyreSize", ddlSize);
                            Bind_DropDown("TyreRim", ddlRim);

                            Bind_ImageFolderName();
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

        private void Bind_ImageFolderName()
        {
            try
            {
                ddlFolderNmae.DataSource = "";
                ddlFolderNmae.DataBind();
                divImgDdpList.Style.Add("display", "none");
                dlImageList.DataSource = null;
                dlImageList.DataBind();

                DataTable dtFolder = new DataTable();
                DataColumn col = new DataColumn("FolderName", typeof(System.String));
                dtFolder.Columns.Add(col);
                string path = Server.MapPath("~/TYREIMAGES");
                foreach (string s in Directory.GetDirectories(path))
                {
                    if (Directory.GetFiles(s).Count() > 0 || Directory.GetDirectories(s).Count() > 0)
                    {
                        if (s.Remove(0, s.LastIndexOf('\\') + 1) != "SKIPIMAGES" && s.Remove(0, s.LastIndexOf('\\') + 1) != "SKIPOTHERS")
                            dtFolder.Rows.Add(s.Remove(0, s.LastIndexOf('\\') + 1));
                    }
                }
                if (dtFolder.Rows.Count > 0)
                {
                    ddlFolderNmae.DataSource = dtFolder;
                    ddlFolderNmae.DataTextField = "FolderName";
                    ddlFolderNmae.DataValueField = "FolderName";
                    ddlFolderNmae.DataBind();
                    ddlFolderName_Index_Change();
                }
                else
                    lblErrMsg.Text = "Image files not available in the server";
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

        protected void ddlFolderNmae_IndexChange(object sender, EventArgs e)
        {
            ddlFolderName_Index_Change();
        }

        private void ddlFolderName_Index_Change()
        {
            try
            {
                dlImageList.DataSource = null;
                dlImageList.DataBind();
                Reset_ddl();
                divImgDdpList.Style.Add("display", "none");
                string path = Server.MapPath("~/TYREIMAGES/" + ddlFolderNmae.SelectedItem.Text + "/");
                DataTable dtImg = new DataTable();
                if (Directory.Exists(path) && Directory.GetFiles(path).Count() > 0)
                {
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

                    if (Directory.GetFiles(path).Count() > 0)
                    {
                        foreach (string s in Directory.GetFiles(path))
                        {
                            string ext = s.Remove(0, s.LastIndexOf('.') + 1).ToLower();
                            if (ext == "jpeg" || ext == "bmp" || ext == "png" || ext == "tif" || ext == "jpg")
                            {
                                Get_SourceImage(path, dtImg);
                                if (dtImg.Rows.Count == 1)
                                {
                                    dlImageList.DataSource = dtImg;
                                    dlImageList.DataBind();
                                    divImgDdpList.Style.Add("display", "block");
                                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "gotoPreviewDiv('fileImages');", true);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("tyreimagesentry.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Reset_ddl()
        {
            try
            {
                ddlimgCategory.SelectedIndex = -1;
                ddlPlatform.SelectedIndex = -1;
                ddlBrand.SelectedIndex = -1;
                ddlSidewall.SelectedIndex = -1;
                ddlType.SelectedIndex = -1;
                ddlSize.SelectedIndex = -1;
                ddlRim.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Get_SourceImage(string ImgPath, DataTable dtImgSource)
        {
            string strImgName = string.Empty;
            try
            {
                foreach (string d in Directory.GetFiles(ImgPath))
                {
                    strImgName = d.Replace(Server.MapPath("~/TYREIMAGES/" + ddlFolderNmae.SelectedItem.Text + "/"), "");
                    string ext = "." + strImgName.Remove(0, strImgName.LastIndexOf('.') + 1);
                    strImgName = strImgName.Replace(ext, "");
                    if (strImgName.Contains("(") || strImgName.Contains(")") || strImgName.Contains("&") || strImgName.Contains("."))
                    {
                        strImgName = "Rename-" + DateTime.Now.ToShortTimeString().Replace(":", "") + "-" + strImgName.Replace("(", "").Replace(")", "").Replace("&", "").Replace(".", "").Replace(" ", "") + ext;
                        Directory.Move(d, ImgPath + strImgName);
                    }
                    else
                        strImgName = strImgName + ext;
                    FileInfo file = new FileInfo(Server.MapPath("~/TYREIMAGES" + "/" + ddlFolderNmae.SelectedItem.Text + "/" + strImgName));
                    if (file.Exists)
                    {
                        string strURL = ConfigurationManager.AppSettings["virdir"] + "TYREIMAGES/" + ddlFolderNmae.SelectedItem.Text + "/" + strImgName;
                        Bitmap img = new Bitmap(Server.MapPath("~/TYREIMAGES" + "/" + ddlFolderNmae.SelectedItem.Text + "/" + strImgName));
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
                            }
                            img.Dispose();
                        }
                        catch (Exception ex)
                        {
                            imageHeight = 760;
                            imageWidth = 760;
                            Utilities.WriteToErrorLog("TTS: Image Size Convert", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                        }
                        dtImgSource.Rows.Add(strImgName, imageWidth + "px", imageHeight + "px", sizeInBytes + " KB", ResolveUrl(strURL));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Skip_File("SKIPOTHERS", strImgName);
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message + ":SKIPOTHERS: " + ImgPath);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strExistFileName = string.Empty;
            try
            {
                if (dlImageList.Items.Count == 1)//fupImg.HasFile
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

                    if (!Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + ddlimgCategory.SelectedItem.Text + "/")))
                        Directory.CreateDirectory(Server.MapPath("~/IMAGESCATALOG" + "/" + ddlimgCategory.SelectedItem.Text + "/"));
                    string strDestinationPath = Server.MapPath("~/IMAGESCATALOG" + "/" + ddlimgCategory.SelectedItem.Text + "/");

                    Label lblImgName = dlImageList.Items[0].FindControl("lblImgName") as Label;
                    strExistFileName = lblImgName.Text;
                    string strSourcePath = Server.MapPath("~/TYREIMAGES" + "/" + ddlFolderNmae.SelectedItem.Text + "/" + lblImgName.Text);
                    string fileName = lblImgName.Text; //Path.GetFileName(fupImg.FileName).ToLower();
                    try
                    {
                        if (fileName != "")
                        {
                            string ext = "." + lblImgName.Text.Remove(0, lblImgName.Text.LastIndexOf('.') + 1).ToLower(); //Path.GetExtension(fupImg.FileName).ToLower();
                            if (ext == ".jpeg" || ext == ".bmp" || ext == ".png" || ext == ".tif" || ext == ".jpg")
                            {
                                try
                                {
                                    imgName = imgName.Replace("\"", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace("\"", "");
                                    imgCount++;
                                    FileInfo file = new FileInfo(Server.MapPath("~/IMAGESCATALOG" + "/" + ddlimgCategory.SelectedItem.Text + "/" + imgName + "-" + imgCount + ".jpeg"));
                                    if (file.Exists)
                                    {
                                        imgCount++;
                                    }
                                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(strSourcePath);
                                    string pathCompress = strDestinationPath + imgName + "-" + imgCount;
                                    string strsave = pathCompress + ".jpeg";
                                    bm.Save(strsave, ImageFormat.Jpeg);
                                    string thumimg = imgName + "-" + imgCount;
                                    GenerateThumbnails(strSourcePath, thumimg);
                                    bm.Dispose();
                                    File.Delete(strSourcePath);//pathToSave
                                }
                                catch (Exception ex)
                                {
                                    Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "2 " + ex.Message + ": " + lblImgName.Text);
                                }
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
                                    sp[7] = new SqlParameter("@imgcategory", ddlimgCategory.SelectedItem.Text != "Choose" ? ddlimgCategory.SelectedItem.Text : "");
                                    sp[8] = new SqlParameter("@imgname", imgName);
                                    sp[9] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                                    sp[10] = new SqlParameter("@imgcount", Convert.ToInt32(imgCount));
                                    sp[11] = new SqlParameter("@processid", "-");
                                    sp[12] = new SqlParameter("@category", ddlFolderNmae.SelectedItem.Text);
                                    int resp = daTTS.ExecuteNonQuery_SP("sp_ins_TyreImageCatalog", sp);
                                    if (resp > 0)
                                    {
                                        ddlFolderNmae_IndexChange(sender, e);
                                    }
                                }
                            }
                            else
                            {
                                Skip_File("SKIPOTHERS", lblImgName.Text);
                                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "SKIPOTHERS");
                                ddlFolderNmae_IndexChange(sender, e);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "1 " + ex.Message);
                    }
                }
                else
                    Bind_ImageFolderName();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message + ": " + strExistFileName);
                Bind_ImageFolderName();
            }
        }

        private void GenerateThumbnails(string sourcePath, string imgname)
        {
            if (!Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + ddlimgCategory.SelectedItem.Text + "/THUMBNAILS" + "/")))
                Directory.CreateDirectory(Server.MapPath("~/IMAGESCATALOG" + "/" + ddlimgCategory.SelectedItem.Text + "/THUMBNAILS" + "/"));
            string strDestinationPath = Server.MapPath("~/IMAGESCATALOG" + "/" + ddlimgCategory.SelectedItem.Text + "/THUMBNAILS" + "/" + imgname + ".jpeg");
            System.Drawing.Image image = System.Drawing.Image.FromFile(sourcePath, false);
            var imageHeight = image.Height;
            var imageWidth = image.Width;
            try
            {
                if (Convert.ToInt32(imageHeight) > 100 || Convert.ToInt32(imageWidth) > 100)
                {
                    decimal decMultiTimes = 0;
                    if (Convert.ToInt32(imageHeight) > Convert.ToInt32(imageWidth))
                        decMultiTimes = Convert.ToInt32(imageHeight) / 100;
                    else if (Convert.ToInt32(imageWidth) > Convert.ToInt32(imageHeight))
                        decMultiTimes = Convert.ToInt32(imageWidth) / 100;
                    var imgWidth = Convert.ToInt32((Convert.ToInt32(imageWidth) / (decMultiTimes + 1)));
                    var imgHeight = Convert.ToInt32((Convert.ToInt32(imageHeight) / (decMultiTimes + 1)));
                    imageWidth = imgWidth;
                    imageHeight = imgHeight;
                }
            }
            catch (Exception ex)
            {
                imageHeight = 100;
                imageWidth = 100;
                Utilities.WriteToErrorLog("TTS: Image Size Convert", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            var thumbnailImg = new Bitmap(imageWidth, imageHeight);
            var thumbGraph = Graphics.FromImage(thumbnailImg);
            var imageRectangle = new Rectangle(0, 0, imageWidth, imageHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            Bitmap bm = new Bitmap(thumbnailImg);
            thumbnailImg.Dispose();
            thumbnailImg = null;
            bm.Save(strDestinationPath, ImageFormat.Jpeg);
            image.Dispose();
        }

        private DataTable Get_ImgCount_ImgName_FromDB()
        {

            DataTable dt = new DataTable(); try
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

        public string Bind_ExistingImage(string strImgUrl, string strImgWidth, string strImgHeight)
        {
            string returnVal = string.Empty;
            returnVal = "<div style='width:760px;float:left;'><img src='" + strImgUrl + "' width='" + strImgWidth + "' height='" + strImgHeight + "' /></div>";
            return returnVal;
        }

        protected void lnkSkipImage_Click(object sender, EventArgs e)
        {
            try
            {
                Label lblImgName = dlImageList.Items[0].FindControl("lblImgName") as Label;
                Skip_File("SKIPIMAGES", lblImgName.Text);
                ddlFolderNmae_IndexChange(sender, e);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Skip_File(string strFolderName, string strFileName)
        {
            try
            {
                if (strFolderName != "" && strFileName != "")
                {
                    string strSourcePath = Server.MapPath("~/TYREIMAGES" + "/" + ddlFolderNmae.SelectedItem.Text + "/" + strFileName);

                    if (!Directory.Exists(Server.MapPath("~/TYREIMAGES" + "/" + strFolderName + "/")))
                        Directory.CreateDirectory(Server.MapPath("~/TYREIMAGES" + "/" + strFolderName + "/"));
                    string strDestinationPath = Server.MapPath("~/TYREIMAGES" + "/" + strFolderName + "/");
                    FileInfo file = new FileInfo(Server.MapPath("~/TYREIMAGES" + "/" + strFolderName + "/" + strFileName));
                    if (file.Exists)
                    {
                        Directory.Move(strSourcePath, strDestinationPath + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToShortTimeString().Replace(":", "") + "-" + strFileName);
                    }
                    else
                    {
                        Directory.Move(strSourcePath, strDestinationPath + strFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message + ": " + strFolderName + " ~ " + strFileName);
                Response.Redirect("tyreimagesentry.aspx", false);
            }
        }
    }
}