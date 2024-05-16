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
    public partial class editCustDetails : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["customermaster"].ToString() == "True")
                        {
                            lnkApprovedDetails.Style.Add("display", "none");
                            rdbCustCategory_IndexChange(sender, e);
                            if (Utilities.Decrypt(Request["ccode"]) != null && Utilities.Decrypt(Request["ccode"].ToString()) != "")
                            {
                                ListItem selectedListItem = ddlCustName.Items.FindByValue("" + Utilities.Decrypt(Request["ccode"].ToString()) + "");
                                if (selectedListItem != null)
                                {
                                    ddlCustName.Items.FindByText(ddlCustName.SelectedItem.Text).Selected = false;
                                    selectedListItem.Selected = true;
                                    ddlCustName_IndexChange(sender, e);
                                }
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
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void FillDropDown_From_DB()
        {
            try
            {
                ddlCountry.DataSource = "";
                ddlCountry.DataBind();
                DataTable dtCountry = Utilities.GetCountryList(HttpContext.Current);
                if (dtCountry.Rows.Count > 0)
                {
                    ddlCountry.DataSource = dtCountry;
                    ddlCountry.DataValueField = "CountryName";
                    ddlCountry.DataTextField = "CountryName";
                    ddlCountry.DataBind();

                    ddlCountry.Items.Insert(0, "Add New Country");
                    ddlCountry.Text = "India 91";
                }

                ddlPriceUnit.DataSource = "";
                ddlPriceUnit.DataBind();
                DataTable dtCurrency = Utilities.GetCurrecnyList(HttpContext.Current);
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
                        ddlChannel.DataSource = "";
                        ddlChannel.DataBind();
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
                        ddlPriceBasis.DataSource = "";
                        ddlPriceBasis.DataBind();
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

        protected void rdbCustCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                FillDropDown_From_DB();
                FillDropDown_From_XML();
                txtCode.Text = "";
                txtType.Text = "";
                DataTable dt = new DataTable();
                if (hdnCustCategory.Value == "" || hdnCustCategory.Value == "Exist")
                {
                    dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_All_CustList", DataAccess.Return_Type.DataTable);
                    lnkApprovedDetails.Style.Add("display", "block");
                }
                else if (hdnCustCategory.Value == "Prospect")
                {
                    dt = (DataTable)daPORT.ExecuteReader_SP("Sp_Sel_All_ProspectCustList", DataAccess.Return_Type.DataTable);
                    lnkApprovedDetails.Style.Add("display", "none");
                }

                ddlCustName.DataSource = "";
                ddlCustName.DataBind();
                if (dt.Rows.Count > 0)
                {
                    ddlCustName.DataSource = dt;
                    ddlCustName.DataTextField = "custname";
                    ddlCustName.DataValueField = "Custcode";
                    ddlCustName.DataBind();
                }
                ddlCustName.Items.Insert(0, "CHOOSE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlCustName_IndexChange(object sender, EventArgs e)
        {
            try
            {
                if (hdnCustCategory.Value == "" || hdnCustCategory.Value == "Exist")
                    Bind_CustomerDetails(ddlCustName.SelectedItem.Text);
                else if (hdnCustCategory.Value == "Prospect")
                    bind_prospectcustdetails(ddlCustName.SelectedItem.Text);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkUpdate_click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sParam = new SqlParameter[18];
                if (hdnCustCategory.Value == "" || hdnCustCategory.Value == "Exist")
                    sParam = new SqlParameter[18];
                else if (hdnCustCategory.Value == "Prospect")
                    sParam = new SqlParameter[20];

                sParam[0] = new SqlParameter("@Custname", ddlCustName.SelectedItem.Text);
                sParam[1] = new SqlParameter("@Add1", txtAdd1.Text);
                sParam[2] = new SqlParameter("@Add2", txtAdd2.Text);
                sParam[3] = new SqlParameter("@Add3", txtAdd3.Text);
                sParam[4] = new SqlParameter("@City", hdnCityName.Value == "Add New City" ? txtNewCity.Text : ddlCity.SelectedItem.Text);
                sParam[5] = new SqlParameter("@Country", hdnCountryName.Value == "Add New Country" ? txtNewCountry.Text : ddlCountry.SelectedItem.Text);
                sParam[6] = new SqlParameter("@Zipcode", txtZip.Text);
                sParam[7] = new SqlParameter("@Phoneno", txtPhone.Text);
                sParam[8] = new SqlParameter("@Contactname", txtContact.Text);
                sParam[9] = new SqlParameter("@Mobile", txtMobile.Text);
                sParam[10] = new SqlParameter("@Email", txtEmail.Text);
                sParam[11] = new SqlParameter("@Channel", ddlChannel.SelectedItem.Text);
                sParam[12] = new SqlParameter("@Pricebasis", ddlPriceBasis.SelectedItem.Text);
                sParam[13] = new SqlParameter("@PriceUnit", ddlPriceUnit.SelectedItem.Text);
                sParam[14] = new SqlParameter("@specialinstruction", txtCustSpl.Text.Replace("\r\n", "~"));
                sParam[15] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                sParam[16] = new SqlParameter("@CustCode", txtCode.Text);
                sParam[17] = new SqlParameter("@webaddress", txtWebAddress.Text);

                int resp = 0;
                if (hdnCustCategory.Value == "" || hdnCustCategory.Value == "Exist")
                    resp = daTTS.ExecuteNonQuery_SP("Sp_Edit_CustMaster", sParam);
                else if (hdnCustCategory.Value == "Prospect")
                {
                    sParam[18] = new SqlParameter("@Port", txtPort.Text);
                    sParam[19] = new SqlParameter("@LeadSource", txtLeadSource.Text);
                    resp = daPORT.ExecuteNonQuery_SP("Sp_Edit_ProspectCustMaster", sParam);
                }

                if (resp != 1)
                    divErrMsg.InnerText = "Customer Details Not Saved";
                else
                {
                    if (hdnCityName.Value == "Add New City")
                    {
                        SqlParameter[] sp1 = new SqlParameter[3];
                        sp1[0] = new SqlParameter("@countryandcode", hdnCountryName.Value == "Add New Country" ? txtNewCountry.Text : ddlCountry.SelectedItem.Text);
                        sp1[1] = new SqlParameter("@cityandcode", txtNewCity.Text);
                        sp1[2] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);

                        daTTS.ExecuteNonQuery_SP("sp_ins_countrymaster", sp1);
                    }
                    Response.Redirect("default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlCountry_IndexChange(object sender, EventArgs e)
        {
            hdnCountryName.Value = ddlCountry.SelectedItem.Text;
            if (ddlCountry.SelectedItem.Text != "Add New Country")
            {
                ddlCity.DataSource = "";
                ddlCity.DataBind();
                DataTable dtCity = Utilities.GetCityList(HttpContext.Current, ddlCountry.SelectedItem.Text);
                if (dtCity.Rows.Count > 0)
                {
                    ddlCity.DataSource = dtCity;
                    ddlCity.DataValueField = "CityName";
                    ddlCity.DataTextField = "CityName";
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

        private void Bind_CustomerDetails(string strCustName)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custname", strCustName);
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Particular_CustDetails", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        txtAdd1.Text = row["Add1"].ToString();
                        txtAdd2.Text = row["Add2"].ToString();
                        txtAdd3.Text = row["Add3"].ToString();

                        BindCity(row["Country"].ToString(), row["City"].ToString());

                        txtZip.Text = row["Zipcode"].ToString();
                        txtEmail.Text = row["Email"].ToString();
                        txtType.Text = row["Custtype"].ToString();
                        txtCode.Text = row["Custcode"].ToString();
                        txtContact.Text = row["Contactname"].ToString();
                        txtPhone.Text = row["Phoneno"].ToString();
                        txtMobile.Text = row["Mobile"].ToString();
                        ddlChannel.Text = row["Channel"].ToString();
                        ddlPriceBasis.Text = row["Pricebasis"].ToString();
                        ddlPriceUnit.Text = row["PriceUnit"].ToString();
                        txtCustSpl.Text = row["specialinstruction"].ToString();
                        txtWebAddress.Text = row["webaddress"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void BindCity(string strCountry, string strCity)
        {
            DataTable dtCity = new DataTable();
            if (hdnCustCategory.Value == "Exist")
            {
                dtCity = Utilities.GetCityList(HttpContext.Current, strCountry);
                ddlCountry.Text = strCountry;
            }
            else if (hdnCustCategory.Value == "Prospect")
            {
                SqlParameter[] sparam = new SqlParameter[1];
                sparam[0] = new SqlParameter("@countryname", strCountry);
                dtCity = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_City_ProspectCustWise", sparam, DataAccess.Return_Type.DataTable);
                if (dtCity.Rows.Count > 0)
                {
                    sparam = new SqlParameter[1];
                    sparam[0] = new SqlParameter("@countryname", strCountry);
                    DataTable dtCountry = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_Country_ProsCustWise", sparam, DataAccess.Return_Type.DataTable);

                    if (dtCountry.Rows.Count > 0)
                        ddlCountry.Text = dtCountry.Rows[0][0].ToString();
                }
                else
                {
                    ddlCountry.Items.Insert(0, "Choose");
                    ddlCountry.Text = "Choose";
                }
            }

            if ((dtCity.Rows.Count > 0 && ddlCity.Items.Count == 0) || ddlCity.Items.Count != dtCity.Rows.Count)
            {
                ddlCity.DataSource = dtCity;
                ddlCity.DataValueField = "CityName";
                ddlCity.DataTextField = "CityName";
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, "Choose");
                ddlCity.Items.Insert(1, "Add New City");
                ddlCity.Text = "Choose";
            }

            if (!string.IsNullOrEmpty(strCity))
            {
                ListItem selectedListItem = ddlCity.Items.FindByText(strCity);
                if (selectedListItem != null)
                {
                    ddlCity.Items.FindByText(ddlCity.SelectedItem.Text).Selected = false;
                    selectedListItem.Selected = true;
                }
                else
                    ddlCity.Text = "Choose";
            }
        }

        private void bind_prospectcustdetails(string strProspectCustName)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custname", strProspectCustName);
                DataTable dt = (DataTable)daPORT.ExecuteReader_SP("Sp_Sel_Particular_ProspectCustomer", sp1, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        txtAdd1.Text = row["Add1"].ToString();
                        txtAdd2.Text = row["Add2"].ToString();
                        txtAdd3.Text = row["Add3"].ToString();

                        BindCity(row["Country"].ToString(), row["City"].ToString());

                        txtZip.Text = row["Zipcode"].ToString();
                        txtEmail.Text = row["Email"].ToString();
                        txtType.Text = row["Custtype"].ToString();
                        txtCode.Text = row["Custcode"].ToString();
                        txtContact.Text = row["Contactname"].ToString();
                        txtPhone.Text = row["Phoneno"].ToString();
                        txtMobile.Text = row["Mobile"].ToString();
                        ddlChannel.Text = row["Channel"].ToString() != "" ? row["Channel"].ToString() : "Direct";
                        ddlPriceBasis.Text = row["Pricebasis"].ToString() != "" ? row["Pricebasis"].ToString() : "FOB";
                        ddlPriceUnit.Text = row["PriceUnit"].ToString() != "" ? row["PriceUnit"].ToString() : "INR India, Rupees";
                        txtCustSpl.Text = row["specialinstruction"].ToString();
                        txtWebAddress.Text = row["webaddress"].ToString();
                        txtPort.Text = row["port"].ToString();
                        txtLeadSource.Text = row["LeadSource"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkApprocedDetails_click(object sender, EventArgs e)
        {
            Response.Redirect("custapprovedlist.aspx?ccode=" + Utilities.Encrypt(ddlCustName.SelectedItem.Value) + "&gpage=" + Utilities.Encrypt("masterentry"), false);
        }
    }
}
