using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;

namespace COTS
{
    /// <summary>
    /// Summary description for uploadhandler
    /// </summary>
    public class uploadhandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.Files.Count > 0)
                {
                    context.Response.ContentType = "text/plain";
                    string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("SCOTS", "TTS").Replace("COTS", "TTS");
                    if (!Directory.Exists(serverURL + "/claimimages/" + context.Session["cotscode"].ToString() + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/dummy/"))
                        Directory.CreateDirectory(serverURL + "/claimimages/" + context.Session["cotscode"].ToString() + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/dummy/");
                    string path = serverURL + "/claimimages/" + context.Session["cotscode"].ToString() + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/dummy/";
                    for (int z = 0; z < context.Request.Files.Count; z++)
                    {
                        HttpPostedFile uploadFiles = context.Request.Files[z];
                        string ext = Path.GetExtension(uploadFiles.FileName).ToLower();
                        string pathToSave = path + uploadFiles.FileName;
                        uploadFiles.SaveAs(pathToSave);
                        string pathCompress = path + Path.GetFileNameWithoutExtension(uploadFiles.FileName);
                        string strName = uploadFiles.FileName;
                        string strsave = pathCompress + ext;
                        if (uploadFiles.ContentLength > (1024 * 1024))
                        {
                            if (ext == ".jpeg" || ext == ".bmp" || ext == ".png" || ext == ".tif" || ext == ".jpg")
                            {
                                Image img = Image.FromFile(pathToSave, false);
                                FileInfo file = new FileInfo(pathToSave);
                                var imageHeight = img.Height;
                                var imageWidth = img.Width;
                                try
                                {
                                    if (Convert.ToInt32(imageHeight) > 1000 || Convert.ToInt32(imageWidth) > 1000)
                                    {
                                        decimal decMultiTimes = 0;
                                        if (Convert.ToInt32(imageHeight) > Convert.ToInt32(imageWidth))
                                            decMultiTimes = Convert.ToInt32(imageHeight) / 1000;
                                        else if (Convert.ToInt32(imageWidth) > Convert.ToInt32(imageHeight))
                                            decMultiTimes = Convert.ToInt32(imageWidth) / 1000;

                                        var imgWidth = Convert.ToInt32((Convert.ToInt32(imageWidth) / (decMultiTimes + 1)));
                                        var imgHeight = Convert.ToInt32((Convert.ToInt32(imageHeight) / (decMultiTimes + 1)));

                                        var decDivEqual = 0;
                                        if (Convert.ToInt32(imgHeight) > Convert.ToInt32(imgWidth))
                                            decDivEqual = 1000 - imgHeight;
                                        else if (Convert.ToInt32(imgWidth) > Convert.ToInt32(imgHeight))
                                            decDivEqual = 1000 - imgWidth;

                                        if (img.Width > 1000)
                                            imageWidth = imgWidth + decDivEqual;
                                        if (img.Height > 1000)
                                            imageHeight = imgHeight + decDivEqual;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    imageHeight = 1000;
                                    imageWidth = 1000;
                                    Utilities.WriteToErrorLog("TTS: Image Size Convert", "", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                                }
                                var thumbnailImg = new Bitmap(imageWidth, imageHeight);
                                var thumbGraph = Graphics.FromImage(thumbnailImg);
                                var imageRectangle = new Rectangle(0, 0, imageWidth, imageHeight);
                                thumbGraph.DrawImage(img, imageRectangle);
                                Bitmap bm = new Bitmap(thumbnailImg);
                                thumbnailImg.Dispose();
                                thumbnailImg = null;
                                strName = Path.GetFileNameWithoutExtension(uploadFiles.FileName) + "1.jpeg";
                                strsave = pathCompress + "1.jpeg";
                                bm.Save(strsave, ImageFormat.Jpeg);
                                img.Dispose();
                                File.Delete(pathToSave);
                            }
                        }
                        FileInfo file1 = new FileInfo(strsave);
                        if (file1.Exists)
                        {
                            string strrename = strName.ToString().Replace("(", "").Replace(")", "").Replace("&", "").Replace(":", "").Replace("/", "");
                            if (strName != strrename)
                                Directory.Move(strsave, serverURL + "/claimimages/" + context.Session["cotscode"].ToString() + "/" + DateTime.Now.ToString("dd-MM-yyyy") + "/dummy/" + strrename);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", context.Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}