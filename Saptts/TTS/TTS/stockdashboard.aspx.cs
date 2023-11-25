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
    public partial class stockdashboard : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["stock_report"].ToString() == "True")
                        {
                            daCOTS.ExecuteNonQuery_SP("sp_ins_StockDashboard");
                            Bind_StockReport();
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
        private void Bind_StockReport()
        {
            try
            {
                //Previous
                DataTable dtPrevious = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_StockDashboard_Previous", DataAccess.Return_Type.DataTable);
                DataRow pRow = dtPrevious.NewRow();
                pRow["StockDate"] = "";
                pRow["StockType"] = "TOTAL";
                pRow["AGrade"] = dtPrevious.Compute("Sum(AGrade)", "").ToString();
                pRow["BGrade"] = dtPrevious.Compute("Sum(BGrade)", "").ToString();
                pRow["CGrade"] = dtPrevious.Compute("Sum(CGrade)", "").ToString();
                pRow["DGrade"] = dtPrevious.Compute("Sum(DGrade)", "").ToString();
                pRow["EGrade"] = dtPrevious.Compute("Sum(EGrade)", "").ToString();
                pRow["PlantTotal"] = dtPrevious.Compute("Sum(PlantTotal)", "").ToString();
                pRow["A_Fwt"] = dtPrevious.Compute("Sum(A_Fwt)", "").ToString();
                pRow["B_Fwt"] = dtPrevious.Compute("Sum(B_Fwt)", "").ToString();
                pRow["C_Fwt"] = dtPrevious.Compute("Sum(C_Fwt)", "").ToString();
                pRow["D_Fwt"] = dtPrevious.Compute("Sum(D_Fwt)", "").ToString();
                pRow["E_Fwt"] = dtPrevious.Compute("Sum(E_Fwt)", "").ToString();
                pRow["PlantTotalFwt"] = dtPrevious.Compute("Sum(PlantTotalFwt)", "").ToString();
                dtPrevious.Rows.Add(pRow);

                rptPreviousStock.DataSource = dtPrevious;
                rptPreviousStock.DataBind();

                //Current
                DataTable dtCurrent = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_StockDashboard_Current", DataAccess.Return_Type.DataTable);
                DataRow cRow = dtCurrent.NewRow();
                cRow["StockDate"] = "";
                cRow["StockType"] = "TOTAL";
                cRow["AGrade"] = dtCurrent.Compute("Sum(AGrade)", "").ToString();
                cRow["BGrade"] = dtCurrent.Compute("Sum(BGrade)", "").ToString();
                cRow["CGrade"] = dtCurrent.Compute("Sum(CGrade)", "").ToString();
                cRow["DGrade"] = dtCurrent.Compute("Sum(DGrade)", "").ToString();
                cRow["EGrade"] = dtCurrent.Compute("Sum(EGrade)", "").ToString();
                cRow["PlantTotal"] = dtCurrent.Compute("Sum(PlantTotal)", "").ToString();
                cRow["A_Fwt"] = dtCurrent.Compute("Sum(A_Fwt)", "").ToString();
                cRow["B_Fwt"] = dtCurrent.Compute("Sum(B_Fwt)", "").ToString();
                cRow["C_Fwt"] = dtCurrent.Compute("Sum(C_Fwt)", "").ToString();
                cRow["D_Fwt"] = dtCurrent.Compute("Sum(D_Fwt)", "").ToString();
                cRow["E_Fwt"] = dtCurrent.Compute("Sum(E_Fwt)", "").ToString();
                cRow["PlantTotalFwt"] = dtCurrent.Compute("Sum(PlantTotalFwt)", "").ToString();
                dtCurrent.Rows.Add(cRow);

                rptCurrentStock.DataSource = dtCurrent;
                rptCurrentStock.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}