using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace TTS.ebidpurchase
{
    public partial class enquirypreparedlist : System.Web.UI.UserControl
    {
        DataAccess daEBID = new DataAccess(ConfigurationManager.ConnectionStrings["eBidDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            bindEnqList();
        }

        private void bindEnqList()
        {
            try
            {
                SqlParameter[] SpParams = new SqlParameter[] { };
                DataTable dtEnquiriesList = (DataTable)daEBID.ExecuteReader_SP("eBitData_SP_LST_PreparedEnquiries", SpParams, DataAccess.Return_Type.DataTable);
                gvEnquiries.DataSource = dtEnquiriesList;
                gvEnquiries.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}