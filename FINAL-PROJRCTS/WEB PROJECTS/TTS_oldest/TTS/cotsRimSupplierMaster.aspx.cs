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

namespace TTS
{
    public partial class cotsRimSupplierMaster : System.Web.UI.Page
    {
        DataAccess dbCon = new DataAccess(ConfigurationManager.ConnectionStrings["orderdb"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        bindGridView();
                        dataGridView1.Visible = true;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_suppliername.Text == "")
                {
                    lblErrMsgcontent.Text = "ENTER SUPPLIERNAME";
                    txt_suppliername.Focus();
                }
                else if (txt_contactperson.Text == "")
                {
                    lblErrMsgcontent.Text = "Enter Contactperson";
                    txt_contactperson.Focus();
                }
                else if (txt_contactnumber.Text == "")
                {
                    lblErrMsgcontent.Text = "Enter Contact Number";
                    txt_contactnumber.Focus();
                }
                else if (txt_emailid.Text == "" && !this.txt_emailid.Text.Contains('@') || !this.txt_emailid.Text.Contains('.'))
                {
                    lblErrMsgcontent.Text = "Enter Email-ID";
                    txt_emailid.Focus();
                }
                else if (txt_address.Text == "")
                {
                    lblErrMsgcontent.Text = "Enter Address";
                    txt_address.Focus();
                }
                else
                {
                    int mode = 0;
                    if (btnSave.Text == "SAVE")
                    {
                        SqlParameter[] spChk = new SqlParameter[] { new SqlParameter("@SupplierName", txt_suppliername.Text) };
                        string dtName = (string)dbCon.ExecuteScalar_SP("SP_Chk_SupplierMaster", spChk);
                        if (dtName != "")
                            lblErrMsgcontent.Text = dtName;
                        else
                            mode = 1;
                    }
                    else if (btnSave.Text == "UPDATE")
                        mode = 2;

                    if (mode > 0)
                    {
                        SqlParameter[] sp1 = new SqlParameter[] { 
                            new SqlParameter("@suppliername", txt_suppliername.Text.ToUpper()), 
                            new SqlParameter("@address", txt_address.Text.ToUpper()), 
                            new SqlParameter("@contactperson", txt_contactperson.Text.ToUpper()), 
                            new SqlParameter("@contactnumber", Convert.ToInt64(txt_contactnumber.Text)), 
                            new SqlParameter("@emailid", txt_emailid.Text.ToLower()),
                            new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value),
                            new SqlParameter("@mode", mode)
                        };
                        int resp = dbCon.ExecuteNonQuery_SP("SP_Ins_SupplierMaster", sp1);
                        if (resp > 0)
                        {
                            lblErrMsgcontent.Text ="Details successfully " + btnSave.Text.ToLower() + "d";
                            btnClear_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Request.RawUrl.ToString(), false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@SupplierName", txt_suppliername.Text), new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value) };
                int resp = dbCon.ExecuteNonQuery_SP("SP_Del_SupplierMaster", sp);
                if (resp > 0)
                {
                    lblErrMsgcontent.Text ="Details successfully " + btnDelete.Text.ToLower() + "d";
                    btnClear_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindGridView()
        {
            try
            {
                DataTable dt = (DataTable)dbCon.ExecuteReader_SP("SP_SEL_SupplierMaster_Details", DataAccess.Return_Type.DataTable);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void dataGridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedIndex >= 0)
                {
                    txt_suppliername.Text = dataGridView1.SelectedRow.Cells[2].Text;
                    txt_address.Text = dataGridView1.SelectedRow.Cells[3].Text;
                    txt_contactperson.Text = dataGridView1.SelectedRow.Cells[4].Text;
                    txt_contactnumber.Text = dataGridView1.SelectedRow.Cells[5].Text;
                    txt_emailid.Text = dataGridView1.SelectedRow.Cells[6].Text;
                    btnSave.Text = "UPDATE";
                    btnDelete.Visible = true;
                    txt_suppliername.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}