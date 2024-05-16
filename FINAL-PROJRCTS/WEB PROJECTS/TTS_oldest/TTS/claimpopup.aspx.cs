using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class claimpopup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["cid"] != null && Request["cid"].ToString() != "")
                hdnCustCode.Value = Request["cid"].ToString();
            if (Request["sno"] != null && Request["sno"].ToString() != "")
                lblStencilNo.Text = Request["sno"].ToString();
            if (Request["cno"] != null && Request["cno"].ToString() != "")
                lblComplaintNo.Text = Request["cno"].ToString();

            Build_ClaimImages(lblStencilNo.Text, hdnCustCode.Value, lblComplaintNo.Text);
        }

        private void Build_ClaimImages(string strStencil, string strCustCode, string strClaimNo)
        {
            try
            {
                gvClaimImages.DataSource = null;
                gvClaimImages.DataBind();
                DataTable dtImage = new DataTable();
                DataColumn col = new DataColumn("ClaimImage", typeof(System.String));
                dtImage.Columns.Add(col);

                if (Directory.Exists(Server.MapPath("~/claimimages" + "/" + strCustCode + "/" + strClaimNo + "/" + strStencil + "/")))
                {
                    string strFileName = string.Empty;
                    foreach (string d in Directory.GetFiles(Server.MapPath("~/claimimages" + "/" + strCustCode + "/" + strClaimNo + "/" + strStencil + "/")))
                    {
                        string strImgName = d.Replace(Server.MapPath("~/claimimages" + "/" + strCustCode + "/" + strClaimNo + "/" + strStencil + "/"), "");
                        string[] strSplit = strImgName.Split('.');
                        string strExtension = "." + strSplit[(strSplit.Length) - 1].ToString().ToLower();
                        if (strExtension == ".jpeg" || strExtension == ".bmp" || strExtension == ".png" || strExtension == ".tif" || strExtension == ".jpg")
                        {
                            string strURL = ConfigurationManager.AppSettings["virdir"] + "claimimages" + "/" + strCustCode + "/" + strClaimNo + "/" + strStencil + "/" + strImgName;
                            dtImage.Rows.Add(ResolveUrl(strURL));
                        }
                    }
                }

                if (dtImage.Rows.Count > 0)
                {
                    gvClaimImages.DataSource = dtImage;
                    gvClaimImages.DataBind();
                    ViewState["dtImage"] = dtImage;
                }
                else
                {
                    lblErrMsg.Text = "IMAGES NOT AVAILABLE";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvClaimImages_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                DataTable dtImage = ViewState["dtImage"] as DataTable;
                gvClaimImages.DataSource = dtImage;
                gvClaimImages.DataBind();
                //Build_ClaimImages(lblClaimStencils.Text);
                gvClaimImages.PageIndex = e.NewPageIndex;
                gvClaimImages.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

    }
}