using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Reflection;

namespace TTS
{
    public partial class cargo_management : System.Web.UI.Page
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (Request["vid"] != null && Request["vid"].ToString() != "")
                    {
                        if (Request["vid"].ToString() == "0")
                            plhCargoManagement.Controls.Add(LoadControl("cargomanagement/containerorderentry.ascx"));
                        else if (Request["vid"].ToString() == "0e")
                            plhCargoManagement.Controls.Add(LoadControl("cargomanagement/revisecargoentry.ascx"));
                        else if (Request["vid"].ToString() == "1")
                            plhCargoManagement.Controls.Add(LoadControl("cargomanagement/tyredimensionmaster.ascx"));
                        else if (Request["vid"].ToString() == "2")
                            plhCargoManagement.Controls.Add(LoadControl("cargomanagement/containerdetail.ascx"));
                        else if (Request["vid"].ToString() == "3")
                            plhCargoManagement.Controls.Add(LoadControl("cargomanagement/loadplanning.ascx"));
                        else if (Request["vid"].ToString() == "4")
                            plhCargoManagement.Controls.Add(LoadControl("cargomanagement/editmasterdetails.ascx"));
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
    }
}