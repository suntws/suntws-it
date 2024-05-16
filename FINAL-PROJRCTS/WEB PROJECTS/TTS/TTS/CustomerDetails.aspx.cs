using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Xml;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;

namespace TTS
{
    public partial class CustomerDetails : System.Web.UI.Page
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
                    hdnCustCategory.Value = "Exist";
                    DataTable dtUser = Session["dtuserlevel"] as DataTable;
                    if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["customermaster"].ToString() == "True")
                    {
                        FillDropDown_From_DB();
                        FillDropDown_From_XML();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, GetType(), "JTotShow2", "displaycon('displaycontent');", true);
                        lblErrMsgcontent.Text = "User privilege disabled. Please contact administrator.";
                    }
                }
                if (hdnCustCategory.Value == "Prospect")
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptNewProspect", "NewProspectAdd();", true);
            }
            else
            {
                Response.Redirect("sessionexp.aspx", false);
            }
        }

        private void FillDropDown_From_DB()
        {
            try
            {
                DataTable dtCountry = new DataTable();
                dtCountry = Utilities.GetCountryList(HttpContext.Current);
                if (dtCountry.Rows.Count > 0)
                {
                    ddlCountry.DataSource = dtCountry;
                    ddlCountry.DataValueField = "CountryName";
                    ddlCountry.DataTextField = "CountryName";
                    ddlCountry.DataBind();

                    ddlCountry.Items.Insert(0, "Add New Country");
                    ddlCountry.Text = "India 91";
                }

                DataTable dtCity = new DataTable();
                dtCity = Utilities.GetCityList(HttpContext.Current, "India 91");

                if (dtCity.Rows.Count > 0)
                {
                    ddlCity.DataSource = dtCity;
                    ddlCity.DataValueField = "CityName";
                    ddlCity.DataTextField = "CityName";
                    ddlCity.DataBind();
                    ddlCity.Items.Insert(0, "Add New City");
                    ddlCity.Text = "Chennai 44";
                }

                DataTable dtCurrency = new DataTable();
                dtCurrency = Utilities.GetCurrecnyList(HttpContext.Current);
                if (dtCurrency.Rows.Count > 0)
                {
                    ddlPriceUnit.DataSource = dtCurrency;
                    ddlPriceUnit.DataValueField = "CurrencyName";
                    ddlPriceUnit.DataTextField = "CurrencyName";
                    ddlPriceUnit.DataBind();

                    ddlPriceUnit.Text = "INR India, Rupees";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void FillDropDown_From_XML()
        {
            try
            {
                string strXmlCustList = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("xmlFolder") + @"CustDetailsList.xml");
                XmlDocument xmlCustList = new XmlDocument();

                xmlCustList.Load(strXmlCustList);

                if (xmlCustList != null)
                {
                    var dict = new Dictionary<string, string>();

                    DataTable dtCustList = new DataTable();
                    dtCustList.Columns.Add("Item", typeof(string));

                    //Type Loaded
                    foreach (XmlNode xNode in xmlCustList.SelectNodes("/customer/type"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtCustList.Rows.Add(item.Value);
                        }
                        ddlType.DataSource = dtCustList;
                        ddlType.DataTextField = "item";
                        ddlType.DataValueField = "item";
                        ddlType.DataBind();
                        ddlType.Items.Insert(0, "CHOOSE");
                    }

                    //Channel Loaded
                    dict.Clear();
                    dtCustList.Clear();
                    foreach (XmlNode xNode in xmlCustList.SelectNodes("/customer/channel"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtCustList.Rows.Add(item.Value);
                        }
                        ddlChannel.DataSource = dtCustList;
                        ddlChannel.DataTextField = "item";
                        ddlChannel.DataValueField = "item";
                        ddlChannel.DataBind();
                    }

                    //Price Basis Loaded
                    dict.Clear();
                    dtCustList.Clear();
                    foreach (XmlNode xNode in xmlCustList.SelectNodes("/customer/basis"))
                    {
                        dict.Add(xNode.Attributes["id"].Value, xNode.Attributes["item"].Value);
                    }
                    if (dict.Count > 0)
                    {
                        foreach (var item in dict.OrderBy(c => c.Key))
                        {
                            dtCustList.Rows.Add(item.Value);
                        }
                        ddlPriceBasis.DataSource = dtCustList;
                        ddlPriceBasis.DataTextField = "item";
                        ddlPriceBasis.DataValueField = "item";
                        ddlPriceBasis.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkSave_click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sParam = new SqlParameter[20];
                if (hdnCustCategory.Value == "" || hdnCustCategory.Value == "Exist")
                    sParam = new SqlParameter[20];
                else if (hdnCustCategory.Value == "Prospect")
                    sParam = new SqlParameter[22];

                sParam[0] = new SqlParameter("@Custcode", hdnCustType.Value + "0000");
                sParam[1] = new SqlParameter("@Custname", txtName.Text);
                sParam[2] = new SqlParameter("@Custtype", ddlType.SelectedItem.Text);
                sParam[3] = new SqlParameter("@Add1", txtAdd1.Text);
                sParam[4] = new SqlParameter("@Add2", txtAdd2.Text);
                sParam[5] = new SqlParameter("@Add3", txtAdd3.Text);
                sParam[6] = new SqlParameter("@City", hdnCityName.Value == "Add New City" ? txtNewCity.Text : ddlCity.SelectedItem.Text);
                sParam[7] = new SqlParameter("@Country", hdnCountryName.Value == "Add New Country" ? txtNewCountry.Text : ddlCountry.SelectedItem.Text);
                sParam[8] = new SqlParameter("@Zipcode", txtZip.Text);
                sParam[9] = new SqlParameter("@Phoneno", txtPhone.Text);
                sParam[10] = new SqlParameter("@Contactname", txtContact.Text);
                sParam[11] = new SqlParameter("@Mobile", txtMobile.Text);
                sParam[12] = new SqlParameter("@Email", txtEmail.Text);
                sParam[13] = new SqlParameter("@Channel", ddlChannel.SelectedItem.Text);
                sParam[14] = new SqlParameter("@Pricebasis", ddlPriceBasis.SelectedItem.Text);
                sParam[15] = new SqlParameter("@PriceUnit", ddlPriceUnit.SelectedItem.Text);
                sParam[16] = new SqlParameter("@specialinstruction", txtCustSpl.Text.Replace("\r\n", "~"));
                sParam[17] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sParam[18] = new SqlParameter("@CustStatus", true);
                sParam[19] = new SqlParameter("@webaddress", txtWebAddress.Text);

                int resp = 0;
                if (hdnCustCategory.Value == "" || hdnCustCategory.Value == "Exist")
                    resp = daTTS.ExecuteNonQuery_SP("Sp_Ins_CustMaster", sParam);
                else if (hdnCustCategory.Value == "Prospect")
                {
                    sParam[20] = new SqlParameter("@Port", txtPort.Text);
                    sParam[21] = new SqlParameter("@LeadSource", txtLeadSource.Text);
                    resp = daPORT.ExecuteNonQuery_SP("Sp_Ins_prospectcustomer", sParam);
                }
                if (resp == 1)
                {
                    if (hdnCityName.Value == "Add New City")
                    {
                        SqlParameter[] sp1 = new SqlParameter[3];
                        sp1[0] = new SqlParameter("@countryandcode", hdnCountryName.Value == "Add New Country" ? txtNewCountry.Text : ddlCountry.SelectedItem.Text);
                        sp1[1] = new SqlParameter("@cityandcode", txtNewCity.Text);
                        sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                        daTTS.ExecuteNonQuery_SP("sp_ins_countrymaster", sp1);
                    }
                    if (hdnCustCategory.Value == "" || hdnCustCategory.Value == "Exist")
                        Update_exist_CustCode(hdnCustType.Value, txtName.Text);
                    else if (hdnCustCategory.Value == "Prospect")
                        update_Prospect_CustCode(txtName.Text);

                    if ((hdnCustCategory.Value == "" || hdnCustCategory.Value == "Exist") && hdnTitleName.Value.ToLower() != "sil")
                        Response.Redirect("custapprovedlist.aspx?ccode=" + Utilities.Encrypt(hdnCustCode.Value) + "&gpage=" + Utilities.Encrypt("masterentry"), false);
                    else
                        Response.Redirect("default.aspx", false);
                }
                else
                    divErrMsg.InnerText = "Customer Details Not Saved";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void update_Prospect_CustCode(string strCustName)
        {
            try
            {
                string strCode = daPORT.ExecuteScalar("select max(id) from ProspectCustomer").ToString();
                if (!string.IsNullOrEmpty(strCode))
                    strCode = "P" + strCode;
                else
                    strCode = "P1";

                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@Custcode", strCode);
                sp1[1] = new SqlParameter("@Custname", strCustName);
                sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                daPORT.ExecuteNonQuery_SP("sp_update_custcode_prospect", sp1);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", "BindRecords.cs", MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Update_exist_CustCode(string strCustType, string strCustName)
        {
            string strMaxCode = daTTS.ExecuteScalar_SP("Sp_Get_MaxCustCode").ToString();
            if (!string.IsNullOrEmpty(strMaxCode))
            {
                int intMaxCode = Convert.ToInt32(strMaxCode) + 1;
                hdnCustCode.Value = strCustType + intMaxCode.ToString("0000");
            }
            else
                hdnCustCode.Value = strCustType + "0001";

            SqlParameter[] sp1 = new SqlParameter[3];
            sp1[0] = new SqlParameter("@Custcode", hdnCustCode.Value);
            sp1[1] = new SqlParameter("@Custname", strCustName);
            sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
            daTTS.ExecuteNonQuery_SP("sp_update_custcode", sp1);
        }

        protected void ddlCountry_IndexChange(object sender, EventArgs e)
        {
            hdnCountryName.Value = ddlCountry.SelectedItem.Text;
            if (ddlCountry.SelectedItem.Text != "Add New Country")
            {
                DataTable dtCity = new DataTable();
                dtCity = Utilities.GetCityList(HttpContext.Current, ddlCountry.SelectedItem.Text);

                if (dtCity.Rows.Count > 0)
                {
                    ddlCity.DataSource = dtCity;
                    ddlCity.DataValueField = "CityName";
                    ddlCity.DataTextField = "CityName";
                    ddlCity.DataBind();
                }
                else
                {
                    ddlCity.DataSource = "";
                    ddlCity.DataBind();
                }
                ddlCity.Items.Insert(0, "Choose");
                ddlCity.Items.Insert(1, "Add New City");
                ddlCity.Text = "Choose";
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptAddCountry", "AddNewCountry();", true);
            }
        }
    }
}
