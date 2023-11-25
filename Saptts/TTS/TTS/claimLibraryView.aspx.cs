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
    public partial class claimLibraryView : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["claim_imagelibrary_view"].ToString() == "True")
                        {
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_ddl_Complainttype_ClaimLibrary", DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                ddlComplainttype.DataSource = dt;
                                ddlComplainttype.DataTextField = "Complaintype";
                                ddlComplainttype.DataValueField = "Complaintype";
                                ddlComplainttype.DataBind();
                                ddlComplainttype.Items.Insert(0, "Choose");
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please Contact administrator";
                        }
                    }
                }
                else
                    Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void bind_image(string strImgCategory, string strImgCount)
        {
            try
            {
                dtimglist.DataSource = null;
                dtimglist.DataBind();
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
                for (int i = 1; i <= Convert.ToInt16(strImgCount); i++)
                {
                    if (Directory.Exists(Server.MapPath("~/ClaimLibrary" + "/" + strImgCategory + "/")))
                        build_imgDataTable(i, dtImg, strImgCategory);
                }
                if (dtImg.Rows.Count > 0)
                {
                    dtimglist.DataSource = dtImg;
                    dtimglist.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void build_imgDataTable(int icount, DataTable dtImage, string iCategory)
        {
            try
            {
                string strURL = ConfigurationManager.AppSettings["virdir"] + "ClaimLibrary" + "/" + iCategory + "/" + icount + ".jpeg";
                FileInfo file = new FileInfo(Server.MapPath("~/ClaimLibrary" + "/" + iCategory + "/" + icount + ".jpeg"));
                if (file.Exists)
                {
                    Bitmap img = new Bitmap(Server.MapPath("~/ClaimLibrary" + "/" + iCategory + "/" + icount + ".jpeg"));
                    var sizeInBytes = Math.Round((Convert.ToDecimal(file.Length) / 1024), 2);
                    var imageHeight = img.Height;
                    var imageWidth = img.Width;
                    try
                    {
                        if (Convert.ToInt32(imageHeight) > 200 || Convert.ToInt32(imageWidth) > 200)
                        {
                            decimal decMultiTimes = 0;
                            if (Convert.ToInt32(imageHeight) > Convert.ToInt32(imageWidth))
                                decMultiTimes = Convert.ToInt32(imageHeight) / 200;
                            else if (Convert.ToInt32(imageWidth) > Convert.ToInt32(imageHeight))
                                decMultiTimes = Convert.ToInt32(imageWidth) / 200;
                            var imgWidth = Convert.ToInt32((Convert.ToInt32(imageWidth) / (decMultiTimes + 1)));
                            var imgHeight = Convert.ToInt32((Convert.ToInt32(imageHeight) / (decMultiTimes + 1)));
                            imageWidth = imgWidth;
                            imageHeight = imgHeight;
                        }
                    }
                    catch (Exception ex)
                    {
                        imageHeight = 200;
                        imageWidth = 200;
                        Utilities.WriteToErrorLog("TTS: Image Size Convert", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                    }
                    img.Dispose();
                    if (dtImage != null)
                    {
                        DataTable dtImg1 = new DataTable();
                        DataColumn col1 = new DataColumn("ImgUrl", typeof(System.String));
                        dtImg1.Columns.Add(col1);
                        dtImg1.Rows.Add(ResolveUrl(strURL));
                        dtImage.Rows.Add(icount, imageWidth + "px", imageHeight + "px", sizeInBytes + " KB", ResolveUrl(strURL));
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlComplainttype_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlParameter[] sp1 = new SqlParameter[1];
            sp1[0] = new SqlParameter("@Complaintype", ddlComplainttype.SelectedValue);
            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimLibrary_param", sp1, DataAccess.Return_Type.DataTable);
            if (dt.Rows.Count > 0)
            {
                string category = Utilities.RemoveSpecialCharacters(ddlComplainttype.SelectedValue);
                gvClaimLibrary.DataSource = dt; gvClaimLibrary.DataBind();
                bind_image(category, dt.Rows[0]["imgcount"].ToString());
            }
        }
    }
}