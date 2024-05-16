using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace COTS
{
    public partial class claimtrack : System.Web.UI.Page
    {
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Session["cotsuser"] != null && Session["cotsuser"].ToString() != "")
                    {
                        bind_ComplaintList();
                    }
                    else
                    {
                        Response.Redirect("SessionExp.aspx", false);
                    }
                }
                catch (Exception ex)
                {
                    Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
                }
            }
        }

        private void bind_ComplaintList()
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());

                DataTable dtList = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimTrackList_cots", sp1, DataAccess.Return_Type.DataTable);

                if (dtList.Rows.Count > 0)
                {
                    gvClaimTrackList.DataSource = dtList;
                    gvClaimTrackList.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvClaimTrackList_PageIndex(object sender, GridViewPageEventArgs e)
        {
            try
            {
                bind_ComplaintList();
                gvClaimTrackList.PageIndex = e.NewPageIndex;
                gvClaimTrackList.DataBind();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("COTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void lnkClaimNo_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
                lblClaimNo.Text = clickedRow.Cells[0].Text;

                gvClaimApproveItems.DataSource = null;
                gvClaimApproveItems.DataBind();
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@custcode", Session["cotscode"].ToString());
                sp2[1] = new SqlParameter("@complaintno", lblClaimNo.Text);

                DataTable dtItems = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_ClaimCustomerItems", sp2, DataAccess.Return_Type.DataTable);
                if (dtItems.Rows.Count > 0)
                {
                    gvClaimApproveItems.DataSource = dtItems;
                    gvClaimApproveItems.DataBind();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}