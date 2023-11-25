using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

namespace TTS
{
    public partial class roomreservations : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daErrDB = new DataAccess(ConfigurationManager.ConnectionStrings["ErrDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["roomreserve"].ToString() == "True")
                        {
                            Bind_RoomApplicableList();
                            Bind_ReservedList();
                            Bind_FromTimeList();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                            lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
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
                Utilities.WriteToErrorLog("ROOM_RESERVE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_RoomApplicableList()
        {
            SqlParameter[] sp1 = new SqlParameter[1];
            sp1[0] = new SqlParameter("@BookUser", Request.Cookies["TTSUser"].Value);
            string strIDList = (string)daErrDB.ExecuteScalar_SP("sp_sel_RoomPrivilege", sp1);

            if (!string.IsNullOrEmpty(strIDList))
            {
                DataTable dt = (DataTable)daErrDB.ExecuteReader("select RoomID,RoomNo,RoomName,MaxSeat from RoomDetailsMaster where RoomStatus=1 and RoomID in(" + strIDList + ")", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvRoomMasterList.DataSource = dt;
                    gvRoomMasterList.DataBind();
                }
            }
            else
            {
                lblErrMsg.Text = "Room booking not assign to you, please contact administrator";
            }
        }

        private void Bind_ReservedList()
        {
            DataTable dtReserve = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_RoomReservationDetails", DataAccess.Return_Type.DataTable);
            if (dtReserve.Rows.Count > 0)
            {
                gvRoomBookedList.DataSource = dtReserve;
                gvRoomBookedList.DataBind();
            }
        }

        public string Bind_CancelledButton(string strReservedDate, string strReservedName, string strRoomID, string strFSno, string strTSno)
        {
            string strBuild = string.Empty;
            if (Request.Cookies["TTSUser"].Value.ToLower() == strReservedName.ToLower() || Request.Cookies["TTSUser"].Value.ToLower() == "admin")
            {
                strBuild += "<span class='btnclear' style='font-size:11px;' onclick='cancel_ReservedRoom(\"" + strReservedDate + "\",\"" + strReservedName + "\",\"" + strRoomID + "\",\"" + strFSno + "\",\"" + strTSno + "\");'>CANCEL</span>";
            }
            return strBuild;
        }

        private void Bind_FromTimeList()
        {
            DataTable dtTime = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_From_RoomReserveTimeList", DataAccess.Return_Type.DataTable);
            if (dtTime.Rows.Count > 0)
            {
                ddlFromTime.DataSource = dtTime;
                ddlFromTime.DataValueField = "Sno";
                ddlFromTime.DataTextField = "TimeVal";
                ddlFromTime.DataBind();
                ddlFromTime.Items.Insert(0, "CHOOSE");
                ddlFromTime.Text = "CHOOSE";
            }
        }

        protected void ddlFromTime_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@RecCount", ddlFromTime.SelectedItem.Value);
                DataTable dtTime = (DataTable)daErrDB.ExecuteReader_SP("sp_sel_To_RoomReserveTimeList", sp1, DataAccess.Return_Type.DataTable);
                if (dtTime.Rows.Count > 0)
                {
                    ddlToTime.DataSource = dtTime;
                    ddlToTime.DataValueField = "Sno";
                    ddlToTime.DataTextField = "TimeVal";
                    ddlToTime.DataBind();
                    ddlToTime.Items.Insert(0, "CHOOSE");
                    ddlToTime.Text = "CHOOSE";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ROOM_RESERVE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnRoomReserveSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrMsg.Text = "";
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@RoomID", hdnRoomId.Value);
                sp2[1] = new SqlParameter("@ReservedDate", DateTime.ParseExact(txtReserveDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                DataTable dt = (DataTable)daErrDB.ExecuteReader_SP("sp_chk_ReservedDate", sp2, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    int fSno = 0; int tSno = 0; int fSnoBook = Convert.ToInt32(ddlFromTime.SelectedItem.Value); int tSnoBook = Convert.ToInt32(ddlToTime.SelectedItem.Value);
                    foreach (DataRow row1 in dt.Rows)
                    {
                        fSno = Convert.ToInt32(row1["FSno"].ToString());
                        tSno = Convert.ToInt32(row1["TSno"].ToString());

                        if ((fSno < fSnoBook && tSno > fSnoBook) || (fSno < tSnoBook && tSno > tSnoBook) || fSno == fSnoBook || tSno == tSnoBook)
                            lblErrMsg.Text += "Already booked by " + row1["ReservedBy"].ToString() + " from " + row1["TimeFrom"].ToString() + " to " + row1["TimeTo"].ToString() + "<br/>";
                    }
                }
                if (lblErrMsg.Text == "")
                {
                    Insert_ReservedDetails();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ROOM_RESERVE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Insert_ReservedDetails()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[9];
                sp1[0] = new SqlParameter("@RoomID", hdnRoomId.Value);
                sp1[1] = new SqlParameter("@ReservedDate", DateTime.ParseExact(txtReserveDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                sp1[2] = new SqlParameter("@FSno", ddlFromTime.SelectedItem.Value);
                sp1[3] = new SqlParameter("@TimeFrom", ddlFromTime.SelectedItem.Text);
                sp1[4] = new SqlParameter("@TSno", ddlToTime.SelectedItem.Value);
                sp1[5] = new SqlParameter("@TimeTo", ddlToTime.SelectedItem.Text);
                sp1[6] = new SqlParameter("@RequiredSeat", txtRequiredSeat.Text);
                sp1[7] = new SqlParameter("@Purpose", txtPurpose.Text);
                sp1[8] = new SqlParameter("@ReservedBy", Request.Cookies["TTSUser"].Value);

                int resp = daErrDB.ExecuteNonQuery_SP("sp_ins_RoomReservationDetails", sp1);
                if (resp > 0)
                {
                    Room_Reserve_MailSent();
                    Response.Redirect("roomreservations.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("ROOM_RESERVE", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Room_Reserve_MailSent()
        {
            try
            {
                string mailConcat = string.Empty;
                mailConcat += "Dear Sir/Madam,<br/><br/>";
                mailConcat += "Your Room Reserved Details:<br/>";
                mailConcat += "DATE: " + txtReserveDate.Text + "<br/>TIME: " + ddlFromTime.SelectedItem.Text + " - " + ddlToTime.SelectedItem.Text + "<br/>";
                mailConcat += "ROOM NO: " + hdnRoomNo.Value + "<br/>ROOM NAME: " + hdnRoomName.Value + "<br/>";
                mailConcat += "PURPOSE: " + txtPurpose.Text + "<br/>REQUIRED SEATS: " + txtRequiredSeat.Text;

                string strMailID = Request.Cookies["TTSUserEmail"].Value;
                if (!string.IsNullOrEmpty(strMailID))
                {
                    YmailSender es = new YmailSender();
                    es.From = "cityoffice_reservations@sun-tws.com";
                    es.To = strMailID;
                    es.CC = "operations@sun-tws.com";
                    es.Password = "$GCm#8g1";
                    es.Subject = "CITY OFFICE MEETING ROOM RESERVED: " + txtReserveDate.Text;
                    es.Body = mailConcat + "<br/><br/><br/><span style='color:#d34639;'>This is a system generated mail. Please do not reply to this email ID.</span>";
                    es.IsHtmlBody = true;
                    es.EmailProvider = YmailSender.EmailProviderType.Gmail;
                    es.Send();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, "Room Reserve Mail Error: " + ex.Message);
            }
        }
    }
}