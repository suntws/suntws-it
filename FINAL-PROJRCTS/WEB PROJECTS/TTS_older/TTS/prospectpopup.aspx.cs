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
    public partial class prospectpopup : System.Web.UI.Page
    {
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        if (Request["qstr"] != null && Request["qstr"].ToString() != "")
                        {
                            if (Request["qstr"].ToString() == "prospectreview" || Request["qstr"].ToString() == "prospectreviewdom")
                            {
                                hdnProsCustCode.Value = Request["custcode"].ToString();
                                hdnfocus.Value = Request["focus"].ToString();
                                hdnflag.Value = Request["flag"].ToString();
                                hdnURL.Value = Request["qstr"].ToString();
                                hdnqstr.Value = Request["querystr"].ToString();
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen", "SelectSingleReview();", true);
                            }
                            else if (Request["qstr"].ToString() == "prospectleadstatus")
                            {
                                hdnProsCustCode.Value = Request["custcode"].ToString();
                                hdnfocus.Value = Request["focus"].ToString();
                                hdnURL.Value = "prospectleadstatus";
                                hdnqstr.Value = Request["querystr"].ToString();
                                DataTable dtAssigned = new DataTable();
                                SqlParameter[] sp1 = new SqlParameter[1];
                                sp1[0] = new SqlParameter("@custcode", hdnProsCustCode.Value);
                                dtAssigned = (DataTable)daPORT.ExecuteReader_SP("sp_sel_custdetails_codewise", sp1, DataAccess.Return_Type.DataTable);
                                if (dtAssigned.Rows.Count == 1)
                                {
                                    lblCountry.Text = dtAssigned.Rows[0]["Country"].ToString();
                                    lblCity.Text = dtAssigned.Rows[0]["City"].ToString();
                                    lblFlag.Text = dtAssigned.Rows[0]["flag"].ToString();
                                    lblfocus.Text = dtAssigned.Rows[0]["focus"].ToString();
                                    lblPort.Text = dtAssigned.Rows[0]["port"].ToString();
                                    txtContactPerson.Text = dtAssigned.Rows[0]["Contactname"].ToString();
                                    txtContact1.Text = dtAssigned.Rows[0]["Mobile"].ToString();
                                    txtContact2.Text = dtAssigned.Rows[0]["Phoneno"].ToString();
                                    txtWebaddress.Text = dtAssigned.Rows[0]["webaddress"].ToString();
                                    txtEmail.Text = dtAssigned.Rows[0]["Email"].ToString();
                                }
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen", "SelectSinglenew();", true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void btnLeadSave_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = string.Empty;
                if (FileUploadControl.HasFile)
                {
                    if (FileUploadControl.PostedFile.ContentLength < 1048576)
                    {
                        if (!Directory.Exists(Server.MapPath("~/leadfeedback/" + hdnProsCustCode.Value + "/")))
                            Directory.CreateDirectory(Server.MapPath("~/leadfeedback/" + hdnProsCustCode.Value + "/"));
                        filename = Path.GetFileName(FileUploadControl.FileName);
                        DateTime onDate = System.DateTime.Now;
                        string makeFileName = onDate.Date.ToShortDateString() + "_" + onDate.Hour + ":" + onDate.Minute;
                        FileUploadControl.SaveAs(Server.MapPath("~/leadfeedback/" + hdnProsCustCode.Value + "/") + filename);
                    }
                }
                SqlParameter[] sp1 = new SqlParameter[10];
                sp1[0] = new SqlParameter("@custcode", hdnProsCustCode.Value);
                sp1[1] = new SqlParameter("@contactperson", txtContactPerson.Text);
                sp1[2] = new SqlParameter("@mobileno", txtContact1.Text);
                sp1[3] = new SqlParameter("@webaddress", txtWebaddress.Text);
                sp1[4] = new SqlParameter("@email", txtEmail.Text);
                sp1[5] = new SqlParameter("@leadsfeedback", txtFeedBack.Text.Replace("\r\n", "~"));
                sp1[6] = new SqlParameter("@flag", ddlCustFocus.SelectedItem.Text);
                sp1[7] = new SqlParameter("@phoneno", txtContact2.Text);
                sp1[8] = new SqlParameter("@AttachFileName", filename);
                sp1[9] = new SqlParameter("@username", Request.Cookies["TTSUser"].Value);
                daPORT.ExecuteNonQuery_SP("Sp_Ins_LeadDetails", sp1);
                ScriptManager.RegisterStartupScript(Page, GetType(), "JBack", "closePopupOnly();", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-PORTDB", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}