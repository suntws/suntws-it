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

namespace TTS
{
    public partial class tyreImagethumbnails : System.Web.UI.Page
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                dlimglist.DataSource = null;
                dlimglist.DataBind();
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

                string strCondition = "select imgcategory,(imgcount-Delcount) as availabelcount,imgcount,imgname,Delcount,DelImg from TyreImageCatalog where imgstatus=1 ";
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
                        DataTable dtImg = new DataTable();
                        DataColumn col = new DataColumn("ThumbnailImgurl", typeof(System.String));
                        dtImg.Columns.Add(col);
                        col = new DataColumn("ImgURL", typeof(System.String));
                        dtImg.Columns.Add(col);
                        col = new DataColumn("ImgWidth", typeof(System.String));
                        dtImg.Columns.Add(col);
                        col = new DataColumn("ImgHeight", typeof(System.String));
                        dtImg.Columns.Add(col);
                        col = new DataColumn("ImgName", typeof(System.String));
                        dtImg.Columns.Add(col);
                        col = new DataColumn("tbImgName", typeof(System.String));
                        dtImg.Columns.Add(col);

                        DataTable dtunique = dtImgList.DefaultView.ToTable(true, "imgcategory");
                        foreach (DataRow catRow in dtunique.Rows)
                        {
                            foreach (DataRow thumbRow in dtImgList.Select("imgcategory='" + catRow["imgcategory"].ToString() + "'"))
                            {
                                if (Convert.ToInt32(thumbRow["availabelcount"].ToString()) > 0)
                                {
                                    if (thumbRow["Delcount"].ToString() != "" && thumbRow["DelImg"].ToString() != "")
                                    {
                                        string[] cdelimgint = thumbRow["DelImg"].ToString().Split(new string[] { "~" }, StringSplitOptions.None);
                                        int[] convertedItems = Array.ConvertAll<string, int>(cdelimgint, int.Parse);
                                        int arrcount = cdelimgint.Length;
                                        Array.Sort(convertedItems);
                                        for (int icount = 1, i = 0; icount <= Convert.ToInt16(thumbRow["imgcount"].ToString()); icount++)
                                        {
                                            if (icount != Convert.ToInt32(convertedItems[i]))
                                            {
                                                if (Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + catRow["imgcategory"].ToString() + "/THUMBNAILS/")))
                                                    Build_ThumbNailsSearchImages(catRow["imgcategory"].ToString(), dtImg, thumbRow["imgname"].ToString() + "-" + icount);
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
                                        for (int icount = 1; icount <= Convert.ToInt32(thumbRow["imgcount"].ToString()); icount++)
                                        {
                                            if (Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + catRow["imgcategory"].ToString() + "/THUMBNAILS/")))
                                                Build_ThumbNailsSearchImages(catRow["imgcategory"].ToString(), dtImg, thumbRow["imgname"].ToString() + "-" + icount);
                                        }
                                    }
                                }
                            }
                        }
                        if (dtImg.Rows.Count > 0)
                        {
                            dlimglist.DataSource = dtImg;
                            dlimglist.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Build_ThumbNailsSearchImages(string strImgCategory, DataTable dtImg, string strImgName)
        {
            try
            {
                string path = Server.MapPath("~/IMAGESCATALOG/" + strImgCategory + "/THUMBNAILS/");
                if (Directory.Exists(path) && Directory.GetFiles(path).Count() > 0)
                {
                    FileInfo file = new FileInfo(Server.MapPath("~/IMAGESCATALOG" + "/" + strImgCategory + "/THUMBNAILS/" + strImgName + ".jpeg"));
                    if (file.Exists)
                    {
                        string d = Server.MapPath("~/IMAGESCATALOG" + "/" + strImgCategory + "/THUMBNAILS/" + strImgName + ".jpeg");
                        hdnImgCategory.Value = strImgCategory;
                        string strThumbImgName = d.Replace(Server.MapPath("~/IMAGESCATALOG/" + strImgCategory + "/THUMBNAILS/"), "");
                        string pathfilethum = ConfigurationManager.AppSettings["virdir"] + "IMAGESCATALOG/" + strImgCategory + "/THUMBNAILS/" + strThumbImgName;
                        string pathfile = ConfigurationManager.AppSettings["virdir"] + "IMAGESCATALOG/" + strImgCategory + "/" + strThumbImgName;
                        Bitmap img = new Bitmap(Server.MapPath("~/IMAGESCATALOG" + "/" + strImgCategory + "/THUMBNAILS/" + strThumbImgName));
                        var imageHeight = img.Height;
                        var imageWidth = img.Width;
                        string strname = strThumbImgName.Replace(".jpeg", "");
                        string strnamecount = strname.Remove(0, strname.LastIndexOf("-") + 1);
                        string strimgname = strname.Replace("Img-" + strnamecount, "Img");
                        dtImg.Rows.Add(ResolveUrl(pathfilethum), pathfile, imageWidth, imageHeight, strnamecount, strimgname);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-THUMBNAILS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "IMAGE-BUILD: " + ex.Message);
            }
        }
    }
}