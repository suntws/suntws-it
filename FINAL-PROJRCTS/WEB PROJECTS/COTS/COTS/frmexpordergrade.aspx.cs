using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Reflection;
using System.Text;

namespace COTS
{
    public partial class frmexpordergrade : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                    {
                        if (Request["qno"] != null && Request["qno"].ToString() != "")
                        {
                            if (Request["qtype"] != null && Request["qtype"].ToString() != "")
                            {
                                hdnStdCustCode.Value = Session["cotsstdcode"].ToString();
                                hdnCustCode.Value = Session["cotscode"].ToString();
                                txtOrderRefNo.Text = Request["qno"].ToString();
                                Bind_data();
                                if (Request["qtype"].ToString() == "old")
                                {
                                    DataTable dt = new DataTable();
                                    dt = (DataTable)daCOTS.ExecuteReader("select Config,TyreType,brand,sizecategory from ApprovedtyreList_order where Custcode='" + HttpContext.Current.Session["cotsstdcode"].ToString() + "' and orderno='" + txtOrderRefNo.Text + "'", DataAccess.Return_Type.DataTable);
                                    if (dt.Rows.Count > 0)
                                    {
                                        int i = 0;
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            ScriptManager.RegisterStartupScript(Page, GetType(), "Jscript" + i, "disabledata('" + dr["sizecategory"].ToString().ToUpper() + "', '" + dr["Config"].ToString() + "', '" + dr["brand"].ToString() + "', '" + dr["TyreType"].ToString() + "');", true);
                                            i++;
                                        }
                                    }
                                }
                            }
                        }
                        else
                            Response.Redirect("frmmsgdisplay.aspx?msgtype=sheetmsg", false);
                    }
                    else
                        Response.Redirect("SessionExp.aspx", false);
                }
                catch (Exception ex)
                {
                    Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                }
            }
        }
        private void Bind_data()
        {
            try
            {
                StringBuilder strApp = new StringBuilder();
                DataTable dtfull = new DataTable();
                dtfull = (DataTable)daTTS.ExecuteReader("select distinct SizeCategory,brand,Config,Tyretype from ApprovedTyreList where CustCode='" + HttpContext.Current.Session["cotsstdcode"].ToString() + "'", DataAccess.Return_Type.DataTable);
                DataView dtDescView = new DataView(dtfull);
                dtDescView.Sort = "SizeCategory ASC";
                DataTable distinctTyretype = dtDescView.ToTable(true, "brand", "Tyretype", "Config", "SizeCategory");
                DataTable distinctSizeCategory = dtDescView.ToTable(true, "SizeCategory");
                DataTable distinctConfig = dtDescView.ToTable(true, "Config", "SizeCategory");
                DataTable distinctbrand = dtDescView.ToTable(true, "Config", "SizeCategory", "brand");

                strApp.Append("<div style='width:1184; float: left;'>");
                int i = 0;
                foreach (DataRow drSizeCategory in distinctSizeCategory.Rows)
                {
                    strApp.Append("<div style='width: 592px; float: left;'>");
                    strApp.Append("<div class='divCategory' style='float: left;background-color: #ccc; width:590px;'><input id='rdbSizeCategory_" + i + "' type='radio' name='SizeCategory' value='" + drSizeCategory["SizeCategory"].ToString() + "'><label for='rdbSizeCategory_" + i + "'>" + drSizeCategory["SizeCategory"].ToString() + "</label></div>");
                    int j = 0; strApp.Append("</br>");
                    strApp.Append("<div style='width: 592px; float: left;'>");
                    foreach (DataRow drPlatform in distinctConfig.Select("SizeCategory='" + drSizeCategory["SizeCategory"].ToString() + "'"))
                    {
                        strApp.Append("<div style='width:296px;float: left; '>");
                        strApp.Append("<div style='float: left;width:294px; display:none;background-color:#F4A460;' class='cssconfig" + drSizeCategory["SizeCategory"].ToString() + " cssconfig'><input id='rdbConfig" + drSizeCategory["SizeCategory"].ToString() + "_" + j + "' type='radio' name='Config" + drSizeCategory["SizeCategory"].ToString() + "' value='" + drPlatform["Config"].ToString() + "'><label for='rdbConfig" + drSizeCategory["SizeCategory"].ToString() + "_" + j + "'>" + drPlatform["Config"].ToString() + "</label></div>");
                        int k = 0;
                        foreach (DataRow drbrand in distinctbrand.Select("SizeCategory='" + drSizeCategory["SizeCategory"].ToString() + "' and Config='" + drPlatform["Config"].ToString() + "'"))
                        {
                            strApp.Append("<div style='width:120px;float: left;'>");
                            strApp.Append("<div style='width:120px;float: left; display:none;padding-left: 5px; background-color: #c18ff0;'class='cssbrand" + drSizeCategory["SizeCategory"].ToString() + drPlatform["Config"].ToString() + " cssbrand'><input id='chkbrand" + drSizeCategory["SizeCategory"].ToString() + drPlatform["Config"].ToString() + "_" + k + "' type='checkbox' name='brand" + drSizeCategory["SizeCategory"].ToString() + drPlatform["Config"].ToString() + "' value='" + drbrand["brand"].ToString() + "'><label for='chkbrand" + drSizeCategory["SizeCategory"].ToString() + drPlatform["Config"].ToString() + "_" + k + "'>" + drbrand["brand"].ToString() + "</label></div>");
                            int l = 0; strApp.Append("</br>");
                            strApp.Append("<div style='width:100px;float:left; padding-left: 10px;'>");
                            foreach (DataRow drTyretype in distinctTyretype.Select("SizeCategory='" + drSizeCategory["SizeCategory"].ToString() + "' and Config='" + drPlatform["Config"].ToString() + "'and brand='" + drbrand["brand"].ToString() + "'"))
                            {
                                strApp.Append("<div style='width:100px;float: left;background-color: #ABEFDE; display:none;' class='csstype" + drSizeCategory["SizeCategory"].ToString() + drPlatform["Config"].ToString() + drbrand["brand"].ToString() + " csstype'><input id='chkTyretype_" + drSizeCategory["SizeCategory"].ToString() + '_' + drPlatform["Config"].ToString() + '_' + drbrand["brand"].ToString() + "_" + drTyretype["Tyretype"].ToString() + "' type='checkbox' name='Tyretype" + drSizeCategory["SizeCategory"].ToString() + drPlatform["Config"].ToString() + drbrand["brand"].ToString() + "' value='" + drTyretype["Tyretype"].ToString() + "' Text='" + drTyretype["Tyretype"].ToString() + "'><label for='chkTyretype_" + drSizeCategory["SizeCategory"].ToString() + '_' + drPlatform["Config"].ToString() + '_' + drbrand["brand"].ToString() + "_" + drTyretype["Tyretype"].ToString() + "'>" + drTyretype["Tyretype"].ToString() + "</label></div>");
                                l++;
                            } k++;
                            strApp.Append("</div>"); strApp.Append("</div>");
                        } strApp.Append("</div>");
                        j++;
                    } strApp.Append("</div>");
                    strApp.Append("</div>");
                    i++;
                }
                strApp.Append("</div>");
                lbl.Text = strApp.ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            DataTable dtordergrade = new DataTable();
            DataColumn col = new DataColumn("sizecategory", typeof(System.String));
            dtordergrade.Columns.Add(col);
            col = new DataColumn("Config", typeof(System.String));
            dtordergrade.Columns.Add(col);
            col = new DataColumn("brand", typeof(System.String));
            dtordergrade.Columns.Add(col);
            col = new DataColumn("TyreType", typeof(System.String));
            dtordergrade.Columns.Add(col);

            string strdata = hdndata.Value;
            string[] split = strdata.Split('~');
            for (int i = 0; i < split.Length; i++)
            {
                string[] strvalue = split[i].Split('_');
                DataRow nRow = dtordergrade.NewRow();
                nRow["Config"] = strvalue[2].ToString();
                nRow["brand"] = strvalue[3].ToString();
                nRow["TyreType"] = strvalue[4].ToString();
                nRow["sizecategory"] = strvalue[1].ToString() == "POB" ? "Pob" : "Solid";
                dtordergrade.Rows.Add(nRow);
            }
            if (dtordergrade.Rows.Count > 0)
            {
                SqlParameter[] sp1 = new SqlParameter[3];
                sp1[0] = new SqlParameter("@Custcode", Session["cotsstdcode"].ToString());
                sp1[1] = new SqlParameter("@orderno", txtOrderRefNo.Text.Trim());
                sp1[2] = new SqlParameter("@ApprovedtyreList_order_DataTable", dtordergrade);
                int resp = 0;
                resp = daCOTS.ExecuteNonQuery_SP("sp_ins_ApprovedtyreList_order", sp1);
                if (resp > 0)
                    Response.Redirect("frmexpitementry.aspx?qno=" + txtOrderRefNo.Text + "&qtype=" + Request["qtype"].ToString(), false);
            }

        }

    }
}