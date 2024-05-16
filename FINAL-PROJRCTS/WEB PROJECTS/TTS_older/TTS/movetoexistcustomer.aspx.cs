using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class movetoexistcustomer : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
            {
                hdnTitleName.Value = ConfigurationManager.AppSettings["pagetitle"];
                if (!IsPostBack)
                {
                    if (Request.Cookies["TTSUser"].Value.ToLower() == "admin" || Request.Cookies["TTSUser"].Value.ToLower() == "somu" ||
                        Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                    {
                        if (Request["custname"] != null && Request["custname"].ToString() != "")
                        {
                            bind_prospectcustdetails(Request["custname"].ToString());
                        }
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

        private DataTable get_SingleCustList(string strCustName)
        {
            SqlParameter[] sp1 = new SqlParameter[1];
            sp1[0] = new SqlParameter("@custname", strCustName);

            DataTable dt = new DataTable();

            return dt = (DataTable)daPORT.ExecuteReader_SP("Sp_Sel_Particular_ProspectCustomer", sp1, DataAccess.Return_Type.DataTable);
        }

        private void bind_prospectcustdetails(string strProspectCustName)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = get_SingleCustList(strProspectCustName);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        txtName.Text = strProspectCustName;
                        lblCustName.Text = strProspectCustName;

                        string concatAddress = row["Add1"].ToString() + " " + row["Add2"].ToString() + " " + row["Add3"].ToString();
                        lblAddress.Text = concatAddress.Trim() == "" ? "&nbsp;" : concatAddress;

                        lblCountry.Text = row["Country"].ToString();
                        lblCity.Text = row["City"].ToString() == "" ? "&nbsp;" : row["City"].ToString();

                        lblZipcode.Text = row["Zipcode"].ToString() == "" ? "&nbsp;" : row["Zipcode"].ToString();
                        lblEmail.Text = row["Email"].ToString() == "" ? "&nbsp;" : row["Email"].ToString();
                        lblType.Text = row["Custtype"].ToString();
                        lblCode.Text = row["Custcode"].ToString();
                        lblContactPerson.Text = row["Contactname"].ToString() == "" ? "&nbsp;" : row["Contactname"].ToString();
                        lblPhone1.Text = row["Phoneno"].ToString() == "" ? "&nbsp;" : row["Phoneno"].ToString();
                        lblPhone2.Text = row["Mobile"].ToString() == "" ? "&nbsp;" : row["Mobile"].ToString();
                        lblChannel.Text = row["Channel"].ToString() == "" ? "&nbsp;" : row["Channel"].ToString();
                        lblPriceBasis.Text = row["Pricebasis"].ToString() == "" ? "&nbsp;" : row["Pricebasis"].ToString();
                        lblPriceUnit.Text = row["PriceUnit"].ToString() == "" ? "&nbsp;" : row["PriceUnit"].ToString();
                        txtMoveCustSpl.Text = row["specialinstruction"].ToString();
                        lblWebAddress.Text = row["webaddress"].ToString() == "" ? "&nbsp;" : row["webaddress"].ToString();

                        Generate_CustCode();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnMoveToExist_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = get_SingleCustList(lblCustName.Text);

                if (dt.Rows.Count > 0)
                {
                    SqlParameter[] sParam = new SqlParameter[20];

                    sParam[0] = new SqlParameter("@Custcode", lblGenreateCode.Text);
                    sParam[1] = new SqlParameter("@Custname", lblCustName.Text);
                    sParam[2] = new SqlParameter("@Custtype", dt.Rows[0]["Custtype"].ToString());
                    sParam[3] = new SqlParameter("@Add1", dt.Rows[0]["Add1"].ToString());
                    sParam[4] = new SqlParameter("@Add2", dt.Rows[0]["Add2"].ToString());
                    sParam[5] = new SqlParameter("@Add3", dt.Rows[0]["Add3"].ToString());
                    sParam[6] = new SqlParameter("@City", dt.Rows[0]["City"].ToString());
                    sParam[7] = new SqlParameter("@Country", dt.Rows[0]["Country"].ToString());
                    sParam[8] = new SqlParameter("@Zipcode", dt.Rows[0]["Zipcode"].ToString());
                    sParam[9] = new SqlParameter("@Phoneno", dt.Rows[0]["Phoneno"].ToString());
                    sParam[10] = new SqlParameter("@Contactname", dt.Rows[0]["Contactname"].ToString());
                    sParam[11] = new SqlParameter("@Mobile", dt.Rows[0]["Mobile"].ToString());
                    sParam[12] = new SqlParameter("@Email", dt.Rows[0]["Email"].ToString());
                    sParam[13] = new SqlParameter("@Channel", dt.Rows[0]["Channel"].ToString());
                    sParam[14] = new SqlParameter("@Pricebasis", dt.Rows[0]["Pricebasis"].ToString());
                    sParam[15] = new SqlParameter("@PriceUnit", dt.Rows[0]["PriceUnit"].ToString());
                    sParam[16] = new SqlParameter("@specialinstruction", dt.Rows[0]["specialinstruction"].ToString());
                    sParam[17] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                    sParam[18] = new SqlParameter("@CustStatus", true);
                    sParam[19] = new SqlParameter("@webaddress", dt.Rows[0]["webaddress"].ToString());

                    int resp = 0;
                    resp = daTTS.ExecuteNonQuery_SP("Sp_Ins_CustMaster", sParam);

                    if (resp > 0)
                    {
                        SqlParameter[] sp2 = new SqlParameter[1];
                        sp2[0] = new SqlParameter("@custcode", lblCode.Text);
                        resp = daPORT.ExecuteNonQuery_SP("Sp_Temp_MoveToExist", sp2);

                        if (resp > 0 && hdnTitleName.Value.ToLower() != "sil")
                            Response.Redirect("custapprovedlist.aspx?ccode=" + Utilities.Encrypt(lblCode.Text) + "&gpage=" + Utilities.Encrypt("masterentry"), false);
                        else
                            Response.Redirect("default.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Generate_CustCode()
        {
            string strMaxCode = daTTS.ExecuteScalar_SP("Sp_Get_MaxCustCode").ToString();

            if (!string.IsNullOrEmpty(strMaxCode))
            {
                int intMaxCode = Convert.ToInt32(strMaxCode) + 1;
                lblGenreateCode.Text = Get_CustType(lblType.Text) + intMaxCode.ToString("0000");
            }
            else
            {
                lblGenreateCode.Text = Get_CustType(lblType.Text) + "0001";
            }
        }

        private string Get_CustType(string custtype)
        {
            string strSplitType = string.Empty;

            switch (custtype.ToLower())
            {
                case "end user":
                    strSplitType = "EU";
                    break;
                case "corporate":
                    strSplitType = "CP";
                    break;
                case "dealer":
                    strSplitType = "DE";
                    break;
                case "oem":
                    strSplitType = "OEM";
                    break;
                case "competitor":
                    strSplitType = "COM";
                    break;
            }
            return strSplitType;
        }
    }
}