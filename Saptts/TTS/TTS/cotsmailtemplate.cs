using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text;
using System.Configuration;
using System.Net.Mime;

namespace Mail_Sender
{
    public class cotsmailtemplate
    {
        public string CustomerName { get; set; }
        public string OrderNo { get; set; }
        public string OrderStatus { get; set; }
        public string OrderDate { get; set; }
        public string Message { get; set; }
        public int StyleSheetCode { get; set; }
        public DataTable dtOrderItems { get; set; }
        public string MailSubject { get; set; }
        public string MailToAddress { get; set; }
        public string MailCcAddress { get; set; }
        public string MailBody { get; set; }

        public string Generate_Mail()
        {
            string Mailbody = Build_MailBody();
            if (Mailbody.Length > 0)
            {
                string MailSendStatus = SendMail();
                if (MailSendStatus == "Success")
                    return MailSendStatus;
                else
                    return "";
            }
            return "";
        }
        private string Build_MailBody()
        {
            MailBody = string.Empty;
            WebRequest request = WebRequest.Create(HttpContext.Current.Server.MapPath("~/cotsmailtemplate.htm"));
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
            {
                MailBody = reader.ReadToEnd();

            }
            MailBody = MailBody.Replace("{Customer}", CustomerName);

            MailBody = MailBody.Replace("{Order_No}", OrderNo);

            MailBody = MailBody.Replace("{Status}", OrderStatus);

            MailBody = MailBody.Replace("{OrderDate}", Convert.ToDateTime(OrderDate).ToString("dd MM YYYY"));

            MailBody = MailBody.Replace("{Message}", Message);

            MailBody = MailBody.Replace("{GridView}", GetGridviewData(dtOrderItems));

            MailBody = MailBody.Replace("{stylesheet}", GetStyleSheet(StyleSheetCode));

            return MailBody;
        }
        private string GetGridviewData(DataTable dt)
        {
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            gv.CssClass = "mGrid";
            gv.AlternatingRowStyle.CssClass = "alt";
            gv.PagerStyle.CssClass = "pgr";
            gv.HeaderStyle.CssClass = "";
            gv.Columns[1].ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            StringBuilder strBuilder = new StringBuilder();
            StringWriter strWriter = new StringWriter(strBuilder);
            HtmlTextWriter htw = new HtmlTextWriter(strWriter);
            gv.RenderControl(htw);
            return strBuilder.ToString();
        }
        private string GetStyleSheet(int stylesheetCode)
        {
            string str_style = "";
            switch (stylesheetCode)
            {
                case 1:
                    {
                        str_style = ".confirm .imgcircle,.confirm span.line { background-color: #98D091; } ";
                        return str_style;
                    }
                case 2:
                    {
                        str_style = ".confirm .imgcircle,.confirm span.line, .process .imgcircle," +
                            " .process span.line { background-color: #98D091; } ";
                        return str_style;
                    }
                case 3:
                    {
                        str_style = ".confirm .imgcircle,.confirm span.line, .process .imgcircle," +
                            " .process span.line, .quality .imgcircle, .quality span.line { background-color: #98D091; } ";
                        return str_style;
                    }
                case 4:
                    {
                        str_style = ".confirm .imgcircle,.confirm span.line, .process .imgcircle," +
                            " .process span.line, .quality .imgcircle, .quality span.line, .dispatch .imgcircle," +
                            " .dispatch span.line { background-color: #98D091; } ";
                        return str_style;
                    }
                case 5:
                    {
                        str_style = ".confirm .imgcircle,.confirm span.line, .process .imgcircle, .process span.line," +
                            " .quality .imgcircle, .quality span.line, .dispatch .imgcircle, .dispatch span.line, .delivery" +
                            " .imgcircle { background-color: #98D091; } ";
                        return str_style;
                    }
                default:
                    {
                        str_style = "";
                        return str_style;
                    }
            }
        }
        private string SendMail()
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    SmtpClient smtp = new SmtpClient();
                    try
                    {
                        AlternateView HtmlView = AlternateView.CreateAlternateViewFromString(MailBody, null, MediaTypeNames.Text.Html);
                        LinkedResource img1 = new LinkedResource(HttpContext.Current.Server.MapPath("~/images/imgmail/dispatch.png"), "image/png");
                        LinkedResource img2 = new LinkedResource(HttpContext.Current.Server.MapPath("~/images/imgmail/process.png"), "image/png");
                        LinkedResource img3 = new LinkedResource(HttpContext.Current.Server.MapPath("~/images/imgmail/quality.png"), "image/png");
                        LinkedResource img4 = new LinkedResource(HttpContext.Current.Server.MapPath("~/images/tvs_suntws.jpg"), "image/png");
                        LinkedResource img5 = new LinkedResource(HttpContext.Current.Server.MapPath("~/images/imgmail/delivery.png"), "image/png");
                        LinkedResource img6 = new LinkedResource(HttpContext.Current.Server.MapPath("~/images/imgmail/confirm.png"), "image/png");
                        img1.ContentId = "ImgDispatch";
                        img2.ContentId = "ImgProcess";
                        img3.ContentId = "ImgQuality";
                        img4.ContentId = "ImgLogo";
                        img5.ContentId = "ImgDelivery";
                        img6.ContentId = "ImgConform";
                        HtmlView.LinkedResources.Add(img1);
                        HtmlView.LinkedResources.Add(img2);
                        HtmlView.LinkedResources.Add(img3);
                        HtmlView.LinkedResources.Add(img4);
                        HtmlView.LinkedResources.Add(img5);
                        HtmlView.LinkedResources.Add(img6);

                        mailMessage.AlternateViews.Add(HtmlView);
                        mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString("This is plain text", null, MediaTypeNames.Text.Plain));
                        
                        mailMessage.From = new MailAddress("s-cots_domestic@sun-tws.com");
                        mailMessage.Subject = MailSubject;
                        mailMessage.To.Add(new MailAddress(MailToAddress));
                        mailMessage.CC.Add(new MailAddress(MailCcAddress));
                        mailMessage.BodyEncoding = Encoding.Default;
                        mailMessage.IsBodyHtml = true;

                        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                        NetworkCred.UserName = "s-cots_domestic@sun-tws.com";
                        NetworkCred.Password = "Y4K/HsD1";
                        
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mailMessage);
                        smtp.Dispose();

                        return "Success";
                    }
                    catch (SmtpFailedRecipientsException ex)
                    {
                        for (int z = 0; z < ex.InnerExceptions.Length; z++)
                        {
                            SmtpStatusCode status = ex.InnerExceptions[z].StatusCode;
                            if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                            {
                                System.Threading.Thread.Sleep(5000);
                                smtp.Send(mailMessage);
                                smtp.Dispose();
                                //return "Success";
                            }
                            else
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }
    }
}