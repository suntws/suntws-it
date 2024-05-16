using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Configuration;

namespace TTS
{
    public partial class s3NetworkCount : System.Web.UI.Page
    {
        DataAccess daPORT = new DataAccess(ConfigurationManager.ConnectionStrings["PORTDB"].ConnectionString);
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        DataTable dtUser = Session["dtuserlevel"] as DataTable;
                        if (dtUser != null && dtUser.Rows.Count > 0 && dtUser.Rows[0]["s3_network"].ToString() == "True")
                        {
                            DataTable dtSouthEast = Bind_Count("South", "East");
                            if (dtSouthEast.Rows.Count > 0)
                            {
                                gv_CountSouthEast.DataSource = dtSouthEast;
                                gv_CountSouthEast.DataBind();
                            }

                            DataTable dtWestNorth = Bind_Count("West", "North");
                            if (dtWestNorth.Rows.Count > 0)
                            {
                                gv_CountWestNorth.DataSource = dtWestNorth;
                                gv_CountWestNorth.DataBind();
                            }

                            ScriptManager.RegisterStartupScript(Page, GetType(), "JscriptTotal", "bind_RunningTotal();", true);
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
                Utilities.WriteToErrorLog("TTS-3S", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private DataTable Bind_Count(string zone1, string zone2)
        {
            DataTable dtCount = new DataTable();
            try
            {
                SqlParameter[] sp1 = new SqlParameter[2];
                sp1[0] = new SqlParameter("@zone1", zone1);
                sp1[1] = new SqlParameter("@zone2", zone2);

                DataTable dtList = (DataTable)daPORT.ExecuteReader_SP("sp_sel_3sCountList_CurYear", sp1, DataAccess.Return_Type.DataTable);

                DataView view = new DataView(dtList);
                DataTable distinctValues = view.ToTable(true, "AsMonth", "AsYear");

                dtCount.Columns.Add("MonthField", typeof(string));
                dtCount.Columns.Add("Activated", typeof(Int16));
                dtCount.Columns.Add("UnderActivation", typeof(Int16));
                dtCount.Columns.Add("SignedUnder", typeof(Int16));
                dtCount.Columns.Add("Total3s", typeof(Int16));
                dtCount.Columns.Add("StartedDiscussion", typeof(Int16));
                dtCount.Columns.Add("NotYetIdentified", typeof(Int16));

                foreach (DataRow subrow in distinctValues.Rows)
                {
                    DateTime date = new DateTime(1, Convert.ToInt32(subrow["AsMonth"].ToString()), 1);
                    int Activated = 0;
                    int UnderActivation = 0;
                    int SignedUnder = 0;
                    int Total3s = 0;
                    int StartedDiscussion = 0;
                    int NotYetIdentified = 0;
                    foreach (DataRow row in dtList.Select("AsMonth='" + subrow["AsMonth"].ToString() + "'"))
                    {
                        if (row["ToCategoryID"].ToString() == "12")
                            Activated = Convert.ToInt16(row["AsCount"].ToString());
                        else if (row["ToCategoryID"].ToString() == "5")
                            UnderActivation = Convert.ToInt16(row["AsCount"].ToString());
                        else if (row["ToCategoryID"].ToString() == "9")
                            SignedUnder = Convert.ToInt16(row["AsCount"].ToString());
                        else if (row["ToCategoryID"].ToString() == "7")
                            StartedDiscussion = Convert.ToInt16(row["AsCount"].ToString());
                        else if (row["ToCategoryID"].ToString() == "11")
                            NotYetIdentified = Convert.ToInt16(row["AsCount"].ToString());
                    }
                    Total3s = Activated + UnderActivation + SignedUnder;
                    dtCount.Rows.Add(date.ToString("MMM").ToUpper() + "-" + subrow["AsYear"].ToString(), Activated, UnderActivation, SignedUnder, Total3s, StartedDiscussion, NotYetIdentified);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS-3S", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
            return dtCount;
        }
    }
}