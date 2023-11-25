using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
namespace TTS
{
    public partial class thumbnails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dtrr = string.Empty;
            DataTable dtFolder = new DataTable();
            DataColumn col = new DataColumn("FolderName", typeof(System.String));
            dtFolder.Columns.Add(col);
            string path = Server.MapPath("~/IMAGESCATALOG");
            string[] fileEntries1 = Directory.GetDirectories(path);
            foreach (string s in fileEntries1)
            {
                if (Directory.GetFiles(s).Count() > 0 || Directory.GetDirectories(s).Count() > 0)
                {
                    dtrr = s.Remove(0, s.LastIndexOf('\\') + 1);
                    if (!Directory.Exists(Server.MapPath("~/IMAGESCATALOG" + "/" + dtrr + "/THUMBNAILS" + "/")))
                        Directory.CreateDirectory(Server.MapPath("~/IMAGESCATALOG" + "/" + dtrr + "/THUMBNAILS" + "/"));
                    string sPath = Server.MapPath("~/IMAGESCATALOG" + "/" + dtrr + "/");
                    string[] fileEntries = Directory.GetFiles(sPath);
                    foreach (string d in fileEntries)
                    {
                        string filename = d.Remove(0, d.LastIndexOf('\\') + 1);
                        string sourcePath = d;
                        string strDestinationPath = Server.MapPath("~/IMAGESCATALOG" + "/" + dtrr + "/THUMBNAILS" + "/" + filename);
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
                }

            }
        }
    }
}