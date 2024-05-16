using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Globalization;

namespace TTS
{
    public partial class requirementAnalysis : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getRequirements();
            }
        }

        private void getRequirements()
        {
            try
            {
                DataTable dt = (DataTable)daTTS.ExecuteReader_SP("SP_LST_Requirement", DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvViewRequirements.DataSource = null;
                    gvViewRequirements.DataBind();
                    gvViewRequirements.DataSource = dt;
                    gvViewRequirements.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string title = "", modulename = "", requirement = "", pendingtask = "", communicationtype = "", communicatedperson = "", queryDate = "", dueDate = "";
                int id = 0, isCompleted = 0;
                double percentage = 0.0;
                title = txtTitle.Text;
                modulename = txtModuleName.Text;
                requirement = hdnRequirement.Value;
                pendingtask = txtPendingTask.Text;
                communicationtype = txtCommunicationType.Text;
                communicatedperson = txtCommunicatePerson.Text;
                queryDate = txtQueryDate.Text;
                dueDate = txtDueDate.Text;
                if (hdnId.Value != "") id = Convert.ToInt32(hdnId.Value);
                if (chkIsCompleted.Checked == true) isCompleted = 1;
                if (txtPercentage.Text != "") percentage = Convert.ToDouble(txtPercentage.Text);
                SqlParameter[] sp = new SqlParameter[]{
                    new SqlParameter("@Id",id),
                    new SqlParameter("@title",title),
                    new SqlParameter("@modulename",modulename),
                    new SqlParameter("@requirement",requirement),
                    new SqlParameter("@pendingtask",pendingtask),
                    new SqlParameter("@communicationtype",communicationtype),
                    new SqlParameter("@communicatedperson",communicatedperson),
                    new SqlParameter("@queryDate",SqlDbType.DateTime),
                    new SqlParameter("@dueDate",SqlDbType.DateTime),
                    new SqlParameter("@IsCompleted", isCompleted),
                    new SqlParameter("@Percentage", percentage)
                };

                if (queryDate.ToString() != "" && dueDate.ToString() != "")
                {
                    sp[7].Value = DateTime.ParseExact(queryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    sp[8].Value = DateTime.ParseExact(dueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                daTTS.ExecuteNonQuery_SP("SP_INS_Requirement", sp);
                Response.Redirect("requirementAnalysis.aspx", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("requirementAnalysis.aspx", false);
        }
    }
}