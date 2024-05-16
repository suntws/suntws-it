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
    public partial class claimLibraryEntry : System.Web.UI.Page
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
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["claim_imagelibrary_entry"].ToString() == "True")
                        {
                            btnSave.Text = "SAVE";
                            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_ddl_Complainttype_ClaimLibrary", DataAccess.Return_Type.DataTable);
                            if (dt.Rows.Count > 0)
                            {
                                ddlComplainttype.DataSource = dt;
                                ddlComplainttype.DataTextField = "Complaintype";
                                ddlComplainttype.DataValueField = "Complaintype";
                                ddlComplainttype.DataBind();
                                ddlComplainttype.Items.Insert(0, "Choose");
                                ddlComplainttype.Items.Insert(1, "ADD NEW ENTRY");
                                txtComplaintType.Style.Add("display", "none");
                            }
                            lnkPrevious.Visible = false;
                            lnkNext.Visible = false;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please Contact administrator";
                        }
                    }
                }
                else Response.Redirect("sessionexp.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlComplainttype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlComplainttype.SelectedValue != "ADD NEW ENTRY")
            {
                txtComplaintType.Style.Add("display", "none");
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@Complaintype", ddlComplainttype.SelectedValue);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimLibrary_param", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    txtAppearance.Text = dt.Rows[0]["Apperance"].ToString().Replace("~", "\n");
                    txtAction.Text = dt.Rows[0]["actioncomments"].ToString().Replace("~", "\n");
                    txtManufacturingEnd.Text = dt.Rows[0]["ManufacturingEnd"].ToString().Replace("~", "\n");
                    txtCustomerEnd.Text = dt.Rows[0]["CustomerEnd"].ToString().Replace("~", "\n");
                    txtWarranty.Text = dt.Rows[0]["warranty"].ToString().Replace("~", "\n");
                    btnSave.Text = "UPDATE";
                    hdnCount.Value = dt.Rows[0]["imgcount"].ToString();
                    hdnID.Value = dt.Rows[0]["ID"].ToString();
                    string ComplaintType = Utilities.RemoveSpecialCharacters(ddlComplainttype.SelectedValue);
                    bind_image(ComplaintType, hdnCount.Value);
                }
            }
            else
            {
                txtAppearance.Text = "";
                txtAction.Text = "";
                txtManufacturingEnd.Text = "";
                txtCustomerEnd.Text = "";
                txtWarranty.Text = "";
                btnSave.Text = "SAVE";
                hdnCount.Value = "";
                hdnID.Value = "";
                txtComplaintType.Style.Add("display", "block");
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
                col = new DataColumn("ImgUrl", typeof(System.String));
                dtImg.Columns.Add(col);
                for (int i = 1; i <= Convert.ToInt16(strImgCount); i++)
                {
                    if (Directory.Exists(Server.MapPath("~/ClaimLibrary" + "/" + strImgCategory + "/")))
                        build_imgDataTable(i, dtImg, strImgCategory);
                }
                if (dtImg.Rows.Count > 0)
                {
                    ViewState["dtImg"] = dtImg;
                    bind_gvImagesShowingList();
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
                        dtImage.Rows.Add(icount, imageWidth + "px", imageHeight + "px", ResolveUrl(strURL));
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
            pgd.PageSize = 10;
            lnkNext.Visible = !(pgd.IsLastPage);
            lnkPrevious.Visible = !(pgd.IsFirstPage);
            dtimglist.DataSource = pgd;
            dtimglist.DataBind();
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
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataListItem row = (DataListItem)((LinkButton)sender).NamingContainer;
                HiddenField hdnurl = row.FindControl("hdnurl") as HiddenField;
                string strsave = Server.MapPath("~" + hdnurl.Value);
                File.Delete(strsave);
                string category = Utilities.RemoveSpecialCharacters(ddlComplainttype.SelectedValue);
                bind_image(category, hdnCount.Value);
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
                int count = 0; string strsave = string.Empty;
                string ComplaintType = ddlComplainttype.SelectedValue.Trim() == "ADD NEW ENTRY" ? txtComplaintType.Text.Trim().ToUpper() : ddlComplainttype.SelectedValue.Trim().ToUpper();
                string ComplaintPath = Utilities.RemoveSpecialCharacters(ComplaintType);
                if (hdnID.Value != "")
                    count = Convert.ToInt32(hdnCount.Value);
                if (Fupimage.HasFile)
                {
                    if (!Directory.Exists(Server.MapPath("~/ClaimLibrary/" + ComplaintPath + "/")))
                        Directory.CreateDirectory(Server.MapPath("~/ClaimLibrary/" + ComplaintPath + "/"));
                    HttpFileCollection hfc = Request.Files;
                    for (int i = 0; i < hfc.Count; i++)
                    {
                        HttpPostedFile hpf = hfc[i];
                        if (hpf.ContentLength > 0)
                        {
                            count++;
                            string filename = count.ToString();
                            if (hpf.ContentLength > (1024 * 1024))
                                strsave = Server.MapPath("~/ClaimLibrary/" + ComplaintPath + "/") + filename + ".jpg";
                            else
                                strsave = Server.MapPath("~/ClaimLibrary/" + ComplaintPath + "/") + filename + ".jpeg";
                            hpf.SaveAs(strsave);
                            if (hpf.ContentLength > (1024 * 1024))
                            {
                                FileInfo file = new FileInfo(Server.MapPath("~/ClaimLibrary" + "/" + ComplaintPath + "/" + filename + ".jpg"));
                                if (file.Exists)
                                {
                                    Bitmap img = new Bitmap(Server.MapPath("~/ClaimLibrary" + "/" + ComplaintPath + "/" + filename + ".jpg"));
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
                                    }
                                    catch (Exception ex)
                                    {
                                        imageHeight = 760;
                                        imageWidth = 760;
                                        Utilities.WriteToErrorLog("TTS: Image Size Convert", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                                    }
                                    var ResizeImg = new Bitmap(imageWidth, imageHeight);
                                    var thumbGraph = Graphics.FromImage(ResizeImg);
                                    var imageRectangle = new Rectangle(0, 0, imageWidth, imageHeight);
                                    thumbGraph.DrawImage(img, imageRectangle);
                                    Bitmap bm = new Bitmap(ResizeImg);
                                    ResizeImg.Dispose();
                                    ResizeImg = null;
                                    string strsaveresize = Server.MapPath("~/ClaimLibrary/" + ComplaintPath + "/") + filename + ".jpeg";
                                    bm.Save(strsaveresize, ImageFormat.Jpeg);
                                    img.Dispose();
                                    File.Delete(strsave);
                                }
                            }
                        }
                    }
                }
                int resp = 0;
                SqlParameter[] sp = new SqlParameter[8];
                if (hdnID.Value != "")
                    sp = new SqlParameter[9];
                sp[0] = new SqlParameter("@Complaintype", ComplaintType);
                sp[1] = new SqlParameter("@Apperance", txtAppearance.Text.Replace("\r\n", "~"));
                sp[2] = new SqlParameter("@ManufacturingEnd", txtManufacturingEnd.Text.Replace("\r\n", "~"));
                sp[3] = new SqlParameter("@CustomerEnd", txtCustomerEnd.Text.Replace("\r\n", "~"));
                sp[4] = new SqlParameter("@actioncomments", txtAction.Text.Replace("\r\n", "~"));
                sp[5] = new SqlParameter("@warranty", txtWarranty.Text.Replace("\r\n", "~"));
                sp[6] = new SqlParameter("@imgcount", count);
                sp[7] = new SqlParameter("@CreatedUser", Request.Cookies["TTSUser"].Value);
                if (hdnID.Value != "")
                {
                    sp[8] = new SqlParameter("@ID", hdnID.Value);
                    resp = daCOTS.ExecuteNonQuery_SP("sp_update_ClaimLibrary", sp);
                }
                else
                    resp = daCOTS.ExecuteNonQuery_SP("sp_ins_ClaimLibrary", sp);
                if (resp > 0)
                    Response.Redirect("claimLibraryEntry.aspx", false);
            }

            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}