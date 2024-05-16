using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace TTS
{
    public partial class cotsgstzenapi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //9ae58295-6f8c-4aea-bd28-9ffdfe8b1113
            if (!IsPostBack)
            {
                ////GET
                string strGetUrl = string.Format("https://your.gstzen.in/~gstzen/a/post-einvoice-data/einvoice-json/version/");
                //("http://jsonplaceholder.typicode.com/posts/1/comments");
            
                WebRequest reqGetObject = WebRequest.Create(strGetUrl);
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.DefaultConnectionLimit = 9999;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                //ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };


                reqGetObject.Method = "GET";
                HttpWebResponse resGetObj = null;
                resGetObj = (HttpWebResponse)reqGetObject.GetResponse();

                string strGetResult = null;
                using (Stream streamGet = resGetObj.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(streamGet);
                    strGetResult = sr.ReadToEnd();
                    sr.Close();
                }

                ////POST
                //string strPostUrl = string.Format("https://my.gstzen.in/~gstzen/a/post-einvoice-data/einvoice-json/");
                //("https://jsonplaceholder.typicode.com/posts");
                //WebRequest reqPostObject = WebRequest.Create(strPostUrl);
                //reqPostObject.Method = "POST";
                //reqPostObject.ContentType = "application/json";

                //string strPostData = "{\"title\":\"testdata\",\"body\":\"testbody\",\"userId\":\"100\"}";
                //using (var streamPostWriter = new StreamWriter(reqPostObject.GetRequestStream()))
                //{
                //    streamPostWriter.Write(strPostData);
                //    streamPostWriter.Flush();
                //    streamPostWriter.Close();

                //    var httpResponse = (HttpWebResponse)reqPostObject.GetResponse();
                //    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        var postResult = streamReader.ReadToEnd();
                //    }
                //}

            }
        }
    }
}