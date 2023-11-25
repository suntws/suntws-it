using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.IO;
namespace TTS
{
    public partial class TyreImgPopup : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (Request["imgurl"] != null && Request["imgurl"].ToString() != "")
                    {
                        hdntgname.Value = Request["strcatagory"].ToString();
                        string[] STRURL = hdntgname.Value.Split('~');
                        hdnimgurl.Value = Request["imgurl"].ToString();
                        hdnname.Value = STRURL[1].ToString();
                        //if (Request.Url.Host.ToLower() == "www.suntws.com")
                        //    hdnimgurl.Value = hdnimgurl.Value.Replace("/tts/", "/");
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JShow2", "popup('" + hdnimgurl.Value + "','" + hdnname.Value + "','" + hdntgname.Value + "');", true);
                        hdnVirtualPath.Value = ConfigurationManager.AppSettings["virdir"].ToLower();
                        if (Request.Cookies["TTSUser"].Value.ToLower() == "admin" || Request.Cookies["TTSUser"].Value.ToLower() == "somu" ||
                            Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                        {
                            btnEdit.Text = "EDIT";
                            lnkImgDelete.Text = "DELETE";
                        }
                        DataTable dtsel = new DataTable();
                        SqlParameter[] sp = new SqlParameter[2];
                        sp[0] = new SqlParameter("@imgname", STRURL[1].ToString());
                        sp[1] = new SqlParameter("@imgCategory", STRURL[0].ToString());
                        dtsel = (DataTable)daTTS.ExecuteReader_SP("sp_sel_edit_TyreImageCatalog", sp, DataAccess.Return_Type.DataTable);
                        if (dtsel.Rows.Count > 0)
                        {
                            lblCategory.Text = dtsel.Rows[0]["imgcategory"].ToString();
                            lblPlatform.Text = dtsel.Rows[0]["config"].ToString();
                            lblSideWall.Text = dtsel.Rows[0]["sidewall"].ToString();
                            lblBrand.Text = dtsel.Rows[0]["brand"].ToString();
                            lblType.Text = dtsel.Rows[0]["tyretype"].ToString();
                            lblSize.Text = dtsel.Rows[0]["tyresize"].ToString();
                            lblRim.Text = dtsel.Rows[0]["rimsize"].ToString();
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

        protected void btnImgDelete_Click(object sender, EventArgs e)
        {
            try
            {

                string[] STRURL = hdntgname.Value.Split('~');
                string strDelImgNo = STRURL[2].Remove(0, STRURL[2].LastIndexOf('-') + 1);
                SqlParameter[] spDel = new SqlParameter[4];
                spDel[0] = new SqlParameter("@imgname", STRURL[1]);
                spDel[1] = new SqlParameter("@imgCategory", STRURL[0]);
                spDel[2] = new SqlParameter("@DelImg", strDelImgNo.Replace(".jpeg", ""));
                spDel[3] = new SqlParameter("@DelUser", Request.Cookies["TTSUser"].Value);
                int resp = daTTS.ExecuteNonQuery_SP("sp_delete_TyreImageCatalog_ImgCount", spDel);
                if (resp > 0)
                {
                    //string sourcePath = Server.MapPath("~/IMAGESCATALOG" + "/" + STRURL[0] + "/THUMBNAILS" + "/" + STRURL[1] + "-" + strDelImgNo.Replace(".jpeg", "") +".jpeg");
                    //if (!Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + STRURL[0] + "/DELETE" + "/")))
                    //    Directory.CreateDirectory(Server.MapPath("~/IMAGESCATALOG" + "/" + STRURL[0] + "/DELETE" + "/"));
                    //string strDestinationPath = Server.MapPath("~/IMAGESCATALOG" + "/" + STRURL[0] + "/DELETE/THUMBNAILS" + "/");
                    //FileInfo sourcefilethumb = new FileInfo(sourcePath);
                    //string pathCompress = strDestinationPath + STRURL[1] + "-" + strDelImgNo.Replace(".jpeg", "");
                    //string strsave = pathCompress + ".jpeg";
                    //string strSourcePath = Server.MapPath("~/IMAGESCATALOG" + "/" + STRURL[0] + "/" + STRURL[1] + "-" + strDelImgNo.Replace(".jpeg", "")+ ".jpeg");
                    //string DestinationPath = Server.MapPath("~/IMAGESCATALOG" + "/" + STRURL[0] + "/DELETE/");
                    //FileInfo sourcefile = new FileInfo(strSourcePath);
                    //string pathCompress1 = DestinationPath + STRURL[1] + "-" + strDelImgNo.Replace(".jpeg", "");
                    //string strsave1 = pathCompress1 + ".jpeg";
                    //sourcefile.MoveTo(strsave1);
                    //if (!Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + STRURL[0] + "/DELETE/THUMBNAILS" + "/")))
                    //    Directory.CreateDirectory(Server.MapPath("~/IMAGESCATALOG" + "/" + STRURL[0] + "/DELETE/THUMBNAILS" + "/"));
                    //sourcefilethumb.MoveTo(strsave);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow", "closePopup();", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkImgDownload_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.Url.Host.ToLower() == "www.suntws.com")
                    hdnimgurl.Value = hdnimgurl.Value.Replace("/tts/", "");
                string path = Server.MapPath("~/" + hdnimgurl.Value);
                Response.ContentType = "application/jpeg";
                Response.AddHeader("content-disposition", "attachment; filename=" + hdnname.Value + ".jpeg");
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}