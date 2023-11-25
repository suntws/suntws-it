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
using System.IO;
namespace TTS
{
    public partial class claimclosed : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        int count = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        if (Request["pid"] != null && Request["pid"].ToString() != "")
                        {
                            DataTable dtUser = Session["dtuserlevel"] as DataTable;
                            if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["claim_track"].ToString() == "True")
                            {
                                if (dtUser.Rows[0]["claim_creditnote_settle_exp"].ToString() == "True" || dtUser.Rows[0]["claim_creditnote_settle_dom"].ToString() == "True")
                                    hdnacc.Value = "1";
                                if (Request["pid"].ToString() == "e")
                                    lblClosedHead.Text = "CLOSED CLAIM LIST EXPORT";
                                else if (Request["pid"].ToString() == "d")
                                    lblClosedHead.Text = "CLOSED CLAIM LIST DOMESTIC";

                                SqlParameter[] sp = new SqlParameter[1];
                                sp[0] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Claimclosed_plant", sp, DataAccess.Return_Type.DataTable);
                                Utilities.ddl_Binding(ddlplant, dt, "plant", "plant", "ALL");
                                ddl_bindyear();
                                ddl_bindMonth();
                                if (ddlYear.Items.Count > 0)
                                    gv_bind(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                                lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "URL IS WRONG";
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
        private void ddl_bindyear()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                sp1[1] = new SqlParameter("@Plant", ddlplant.SelectedItem.Text);
                DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claimClosed_year", sp1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlYear, dt, "closedyear", "closedyear", "");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void ddl_bindMonth()
        {
            try
            {
                if (ddlYear.Items.Count > 0)
                {
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
                    sp1[1] = new SqlParameter("@Plant", ddlplant.SelectedItem.Text);
                    sp1[2] = new SqlParameter("@year", Convert.ToInt32(ddlYear.SelectedItem.Text));
                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_claimClosed_Month", sp1, DataAccess.Return_Type.DataTable);
                    Utilities.ddl_Binding(ddlMonth, dt, "closedmonth", "closedid", "");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void gv_bind(string plant, string year, string month)
        {
            lblDCno.Text = "";
            lbljjno.Text = "";
            lblqty.Text = "";
            lbltransport.Text = "";
            lbllrno.Text = "";
            lbllrdate.Text = "";
            DataTable dt = new DataTable();
            SqlParameter[] sp1 = new SqlParameter[4];
            sp1[0] = new SqlParameter("@ClaimType", Request["pid"].ToString().ToUpper());
            sp1[1] = new SqlParameter("@Plant", plant);
            sp1[2] = new SqlParameter("@year", Convert.ToInt32(year));
            sp1[3] = new SqlParameter("@month", Convert.ToInt32(month));
            dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimClosedList", sp1, DataAccess.Return_Type.DataTable);
            lblRecordCount.Text = "0";
            if (dt.Rows.Count > 0)
            {
                gvClaimClosedList.DataSource = dt;
                gvClaimClosedList.DataBind();
                lblRecordCount.Text = dt.Rows.Count.ToString();
            }
        }
        protected void lnkClaimNo_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblClaimCustName.Text = clickedRow.Cells[0].Text;
                lblClaimNo.Text = clickedRow.Cells[1].Text;
                HiddenField hdnClaimCustCode = clickedRow.FindControl("hdnClaimCustCode") as HiddenField;
                count = 0;
                if (lblClaimCustName.Text != "" && lblClaimNo.Text != "")
                    lblClaim.Text = "-";
                hdnCustCode.Value = hdnClaimCustCode.Value;
                gvClaimApproveItems.DataSource = null;
                gvClaimApproveItems.DataBind();
                string strPlant = clickedRow.Cells[3].Text.Replace("&amp;", "&");
                SqlParameter[] sp2 = new SqlParameter[3];
                sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                sp2[2] = new SqlParameter("@assigntoqc", strPlant);

                DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimTrack_ItemList", sp2, DataAccess.Return_Type.DataTable);
                if (dtItems.Rows.Count > 0)
                {
                    gvClaimApproveItems.DataSource = dtItems;
                    gvClaimApproveItems.DataBind();
                    gvClaimDetails.DataSource = null;
                    gvClaimDetails.DataBind();
                    SqlParameter[] sp1 = new SqlParameter[3];
                    sp1[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
                    sp1[1] = new SqlParameter("@ComplaintNo", lblClaimNo.Text);
                    sp1[2] = new SqlParameter("@plant", strPlant);

                    DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_Status_track", sp1, DataAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["CrmSettleType"].ToString() == "Free replacement in next shipment" && Request["pid"].ToString() == "d")
                        {
                            sp2 = new SqlParameter[3];
                            sp2[0] = new SqlParameter("@custcode", hdnCustCode.Value);
                            sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);
                            sp2[2] = new SqlParameter("@CreditNoteNo", dt.Rows[0]["CreditNoteNo"].ToString());
                            DataTable dtItem1 = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimReplacementDetails", sp2, DataAccess.Return_Type.DataTable);
                            if (dtItem1.Rows.Count > 0)
                            {
                                lblDCno.Text = "<span class='headCss' style='width:80px;float:left'>DCNO: </span>" + dtItem1.Rows[0]["DCNO"].ToString();
                                lbljjno.Text = "<span class='headCss' style='width:80px;float:left'>FORM JJ NO.</span>" + dtItem1.Rows[0]["JJNO"].ToString();
                                lblqty.Text = "<span class='headCss' style='width:80px;float:left'>QTY: </span>" + dtItem1.Rows[0]["Qty"].ToString();
                                lbltransport.Text = "<span class='headCss' style='width:150px;float:left'>NAME OF TRANSPORT: </span>" + dtItem1.Rows[0]["TranspoterName"].ToString();
                                lbllrno.Text = "<span class='headCss' style='width:150px;float:left'>LR NO.: </span>" + dtItem1.Rows[0]["LRNO"].ToString();
                                lbllrdate.Text = "<span class='headCss' style='width:150px;float:left'>LR DATE: </span>" + dtItem1.Rows[0]["LRDATE"].ToString();
                                ScriptManager.RegisterStartupScript(Page, GetType(), "JOpen112", "showdiv('divdispatch');", true);
                            }
                            else
                            {
                                lblDCno.Text = "";
                                lbljjno.Text = "";
                                lblqty.Text = "";
                                lbltransport.Text = "";
                                lbllrno.Text = "";
                                lbllrdate.Text = "";
                            }
                        }
                        gvClaimDetails.DataSource = dt;
                        gvClaimDetails.DataBind();
                    }

                    ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow1", "showdiv('divshow');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "gotoClaimDiv('divgvclaimapproved');", true);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        public string Excel_Export(string strfilename)
        {
            string strfile = string.Empty;
            if (hdnacc.Value == "1")
            {
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/ExcelCreditNote/" + hdnCustCode.Value + "/" + strfilename.Replace(".pdf", ".xls");
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                    strfile = strfilename;
            } return strfile;
        }
        public string CreaditNote(string creditno, string CreditNoteYear)
        {
            string strCreditNo = string.Empty;
            if (lblClaimNo.Text.Substring(0, 1).ToLower() == "e")
                strCreditNo = "EXP/CN-" + creditno + "/" + CreditNoteYear;
            else if (lblClaimNo.Text.Substring(0, 1).ToLower() == "d")
                strCreditNo = "DOM/CN-" + creditno + "/" + CreditNoteYear;
            return strCreditNo;
        }

        protected void lnkcreditno_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/CreditNote/" + hdnCustCode.Value + "/" + lnkTxt.Text;

                Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                Response.WriteFile(path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlplant_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindyear();
            ddl_bindMonth();
            gv_bind(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_bindMonth();
            gv_bind(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_bind(ddlplant.SelectedItem.Text, ddlYear.SelectedItem.Text, ddlMonth.SelectedItem.Value);
        }
        protected void lnkcreditExcel_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkTxt = sender as LinkButton;
                string serverURL = HttpContext.Current.Server.MapPath("~/").Replace("TTS", "pdfs");
                string path = serverURL + "/ExcelCreditNote/" + hdnCustCode.Value + "/" + lnkTxt.Text;
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    try
                    {
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", "attachment; filename=" + lnkTxt.Text);
                        Response.AddHeader("Content-Type", "application/Excel");
                        Response.ContentType = "application/vnd.xls";
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.WriteFile(file.FullName);
                        Response.End();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvClaimTrackList_OnDataBound(object sender, EventArgs e)
        {
            for (int i = gvClaimClosedList.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gvClaimClosedList.Rows[i];
                GridViewRow previousRow = gvClaimClosedList.Rows[i - 1];
                for (int j = 0; j < 2; j++)
                {
                    if (row.Cells[j].Text == previousRow.Cells[j].Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                                previousRow.Cells[j].RowSpan += 2;
                            else
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
        public string Build_ClaimImages1(string strStencil)
        {
            string img = string.Empty;
            try
            {
                count++;
                DataTable dtImage = new DataTable();
                DataColumn col = new DataColumn("ClaimImage", typeof(System.String));
                dtImage.Columns.Add(col);
                if (Directory.Exists(Server.MapPath("~/claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/")))
                {
                    string strFileName = string.Empty;
                    foreach (string d in Directory.GetFiles(Server.MapPath("~/claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/")))
                    {
                        string strImgName = d.Replace(Server.MapPath("~/claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/"), "");
                        string[] strSplit = strImgName.Split('.');
                        string strExtension = "." + strSplit[(strSplit.Length) - 1].ToString().ToLower();
                        if (strExtension == ".jpeg" || strExtension == ".bmp" || strExtension == ".png" || strExtension == ".tif" || strExtension == ".jpg")
                        {
                            string strURL = ConfigurationManager.AppSettings["virdir"] + "claimimages" + "/" + hdnCustCode.Value + "/" + lblClaimNo.Text + "/" + strStencil + "/" + strImgName;
                            if (img.Length == 0)
                                img = "<a id='imageLink' href='" + strURL + "' rel='lightbox[Brussels" + count + "]' runat='server'>VIEW</a>";
                            else
                                img += "<a id='imageLink' href='" + strURL + "' rel='lightbox[Brussels" + count + "]' runat='server'></a>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            } return img;
        }

    }
}