using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;

namespace TTS
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = ConfigurationManager.AppSettings["pagetitle"];
            try
            {
                if (!IsPostBack)
                {
                    hdnVirtualStr.Value = ConfigurationManager.AppSettings["virdir"];
                    if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "")
                    {
                        lblWelcome.InnerText = Utilities.ProperCase(Request.Cookies["TTSUser"].Value);
                        lblLogTime.InnerText = "[Last Login: " + Convert.ToDateTime(Request.Cookies["TTSLastLogin"].Value).ToString("MMM dd yyyy") + ", " + Convert.ToDateTime(Request.Cookies["TTSLastLogin"].Value).ToString("hh:mm tt") + "]";
                        DataTable dtuserlevel = Session["dtuserlevel"] as DataTable;
                        if (dtuserlevel != null && dtuserlevel.Rows.Count > 0)
                        {
                            StringBuilder menuAppend = new StringBuilder();
                            menuAppend.Append("<div id=\"myjquerymenu\" class=\"jquerycssmenu\">");
                            menuAppend.Append("<ul>");//Start Parent
                            Build_DashBoard_MenuList(menuAppend, dtuserlevel, "");
                            Build_SctosDomestic_MenuList(menuAppend, dtuserlevel, "");
                            Build_SctosExport_MenuList(menuAppend, dtuserlevel, "");
                            Build_Claim_MenuList(menuAppend, dtuserlevel, "");
                            //Build_eBid_MenuList(menuAppend, dtuserlevel, "");
                            Build_Prospect_MenuList(menuAppend, dtuserlevel, "");
                            Build_Master_MenuList(menuAppend, dtuserlevel, "");

                            if (Request.Cookies["TTSUser"].Value.ToLower() == "somu" || Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                            {
                                menuAppend.Append("<li><a href=\"#\">Requirement</a><ul>");
                                menuAppend.Append("<li><a href=\"requirementticketrise.aspx\">Create</a></li>");
                                menuAppend.Append("<li><a href=\"requirementticketresponse.aspx\">Analyze</a></li>");
                                menuAppend.Append("<li><a href=\"cotsgstzenapi.aspx\">API</a></li>");
                                menuAppend.Append("</ul></li>");
                            }
                            menuAppend.Append("<li><a href=\"changePassword.aspx\">Change Password</a></li>");

                            menuAppend.Append("</ul><br style=\"clear: left\" />");//End Parent
                            menuAppend.Append("</div>");
                            litUserLevelMenu.Text = menuAppend.ToString();
                        }
                        else
                        {
                            Response.Redirect("sessionexp.aspx", false);
                        }
                    }
                    else if (Request.Cookies["TTSUser"] == null || Request.Cookies["TTSUser"].Value == "")
                        Response.Redirect("login.aspx?ReturnUrl=" + Request.Url.PathAndQuery.Replace("/", ""), false);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }

        }
        private void Build_DashBoard_MenuList(StringBuilder menuAppend, DataTable dt, string strImg)
        {
            try
            {
                if (dt.Rows[0]["processid_create"].ToString() == "True" || dt.Rows[0]["processid_request"].ToString() == "True" || dt.Rows[0]["orderposition_entry"].ToString() == "True"
                    || dt.Rows[0]["orderposition_all"].ToString() == "True" || dt.Rows[0]["orderposition_domestic"].ToString() == "True"
                    || dt.Rows[0]["stock_report"].ToString() == "True" || dt.Rows[0]["roomreserve"].ToString() == "True" || dt.Rows[0]["gradeselect_entry"].ToString() == "True"
                    || dt.Rows[0]["gradeselect_tool"].ToString() == "True" || dt.Rows[0]["makemodel_entry"].ToString() == "True" || dt.Rows[0]["makemodel_search"].ToString() == "True"
                    || dt.Rows[0]["img_upload"].ToString() == "True" || dt.Rows[0]["img_download"].ToString() == "True" || dt.Rows[0]["dwg_request"].ToString() == "True"
                    || dt.Rows[0]["dwg_upload"].ToString() == "True" || dt.Rows[0]["dwg_edc_level1"].ToString() == "True" || dt.Rows[0]["dwg_edc_level2"].ToString() == "True"
                    || dt.Rows[0]["dwg_edc_level3"].ToString() == "True" || dt.Rows[0]["dwg_crm_approve"].ToString() == "True" || dt.Rows[0]["dwg_allocate"].ToString() == "True"
                    || dt.Rows[0]["dwg_download"].ToString() == "True")
                {
                    menuAppend.Append("<li><a href=\"#\">" + strImg + "Dashboard</a><ul>");
                    if (dt.Rows[0]["processid_request"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"processidrequest.aspx\">Process-ID Check/Create</a></li>");
                    if (dt.Rows[0]["processid_create"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"processidrimsizeassign.aspx\">Edc No Assign To Tyre size</a></li>");
                    if (dt.Rows[0]["orderposition_entry"].ToString() == "True" || dt.Rows[0]["orderposition_all"].ToString() == "True" || dt.Rows[0]["orderposition_domestic"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Order Position</a><ul>");
                        if (dt.Rows[0]["orderposition_entry"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"orderpositionentry.aspx\">Data Entry Old Layout</a></li>");
                            menuAppend.Append("<li><a href=\"orderpositiondetailsentry.aspx\">Data Entry New Layout</a></li>");
                        }
                        if (dt.Rows[0]["orderposition_all"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"orderpositionshow.aspx\">Old Layout Report</a></li>");
                            menuAppend.Append("<li><a href=\"orderpositiondetailsviewall.aspx\">New Layout Report</a></li>");
                        }
                        if (dt.Rows[0]["orderposition_domestic"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"orderpositiondomestic.aspx\">View Domestic</a></li>");
                            menuAppend.Append("<li><a href=\"cotsdashboard.aspx\">Domestic Sales Dashboard</a><li>");
                        }
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["s3_network"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">3s Network</a><ul>");
                        menuAppend.Append("<li><a href=\"s3entry.aspx?qKey=0\">New Entry</a></li>");
                        menuAppend.Append("<li><a href=\"s3dashboard.aspx\">3s Details</a></li>");
                        menuAppend.Append("<li><a href=\"s3networkcount.aspx\">3s Count</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["stock_report"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Tyre Stock</a><ul>");
                        menuAppend.Append("<li><a href=\"stockdump.aspx\">Global Stock Dump</a></li>");
                        menuAppend.Append("<li><a href=\"#\">Search Filter Wise</a><ul>");
                        menuAppend.Append("<li><a href=\"stockfilteredreport_v1.aspx?spid=" + Utilities.Encrypt("MMN") + "\">MMN</a></li>");
                        menuAppend.Append("<li><a href=\"stockfilteredreport_v1.aspx?spid=" + Utilities.Encrypt("SLTL") + "\">SLTL</a></li>");
                        menuAppend.Append("<li><a href=\"stockfilteredreport_v1.aspx?spid=" + Utilities.Encrypt("SITL") + "\">SITL</a></li>");
                        menuAppend.Append("<li><a href=\"stockfilteredreport_v1.aspx?spid=" + Utilities.Encrypt("PDK") + "\">PDK</a></li>");
                        //menuAppend.Append("<li><a href=\"stockfilteredreport_v1.aspx?spid=" + Utilities.Encrypt("PDK") + "\">NEW PDK</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"stockmatrixreport.aspx\">Matrix Format Count </a></li>");
                        menuAppend.Append("<li><a href=\"stockdashboard.aspx\">Dashboard</a></li>");
                        menuAppend.Append("<li><a href=\"#\">Location Update</a><ul>");
                        menuAppend.Append("<li><a href=\"stocklocation.aspx?pid=" + Utilities.Encrypt("MMN") + "\">MMN</a></li>");
                        menuAppend.Append("<li><a href=\"stocklocation.aspx?pid=" + Utilities.Encrypt("SLTL") + "\">SLTL</a></li>");
                        menuAppend.Append("<li><a href=\"stocklocation.aspx?pid=" + Utilities.Encrypt("SITL") + "\">SITL</a></li>");
                        menuAppend.Append("<li><a href=\"stocklocation.aspx?pid=" + Utilities.Encrypt("PDK") + "\">PDK</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"stencilnosearch.aspx\">Find Stencil History</a></li>");
                        menuAppend.Append("<li><a href=\"liquidationreport.aspx\">Liquidation Report</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"#\">Rim Stock</a><ul>");
                        menuAppend.Append("<li><a href=\"stockrim.aspx?spid=" + Utilities.Encrypt("MMN") + "\">MMN</a></li>");
                        menuAppend.Append("<li><a href=\"stockrim.aspx?spid=" + Utilities.Encrypt("PDK") + "\">PDK</a></li>");
                        menuAppend.Append("</ul></li>");
                        //menuAppend.Append("<li><a href=\"exportsalesdatareport.aspx\">Export Sales Matrix</a></li>");
                    }
                    if (dt.Rows[0]["processid_create"].ToString() == "True" && dt.Rows[0]["stock_report"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Rim Stock Delete</a><ul>");
                        menuAppend.Append("<li><a href=\"rimstockdelete.aspx?spid=" + Utilities.Encrypt("MMN") + "\">MMN</a></li>");
                        menuAppend.Append("<li><a href=\"rimstockdelete.aspx?spid=" + Utilities.Encrypt("PDK") + "\">PDK</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["gradeselect_entry"].ToString() == "True" || dt.Rows[0]["gradeselect_tool"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Grade Selection</a><ul>");
                        if (dt.Rows[0]["gradeselect_entry"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"typegrademaster.aspx\">Data Entry</a></li>");
                        if (dt.Rows[0]["gradeselect_tool"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"typegradeselector.aspx\">Data Tool</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["makemodel_entry"].ToString() == "True" || dt.Rows[0]["makemodel_search"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Make-Model</a><ul>");
                        if (dt.Rows[0]["makemodel_entry"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"makemodelentry.aspx\">Data Entry</a></li>");
                        if (dt.Rows[0]["makemodel_search"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"makemodelsearch.aspx\">Data Search</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["img_upload"].ToString() == "True" || dt.Rows[0]["img_download"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Image Catalog</a><ul>");
                        if (dt.Rows[0]["img_upload"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"tyreimagesentry.aspx\">Upload</a></li>");
                            menuAppend.Append("<li><a href=\"tyreimageedit.aspx\">Edit</a></li>");
                        }
                        if (dt.Rows[0]["img_download"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">Download</a><ul>");
                            menuAppend.Append("<li><a href=\"tyreimagethumbnails.aspx\">Thumbnails List</a></li>");
                            menuAppend.Append("<li><a href=\"tyreimagesdownload.aspx\">Data Wise</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["dwg_request"].ToString() == "True" || dt.Rows[0]["dwg_upload"].ToString() == "True" || dt.Rows[0]["dwg_edc_level1"].ToString() == "True"
                        || dt.Rows[0]["dwg_edc_level2"].ToString() == "True" || dt.Rows[0]["dwg_edc_level3"].ToString() == "True" || dt.Rows[0]["dwg_crm_approve"].ToString() == "True"
                        || dt.Rows[0]["dwg_allocate"].ToString() == "True" || dt.Rows[0]["dwg_download"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Drawing Catalog</a><ul>");
                        if (dt.Rows[0]["dwg_request"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"tyredrawingrequest.aspx\">Design Request</a></li>");
                        if (dt.Rows[0]["dwg_upload"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"processidcreaterim.aspx\">Rim Process-ID Check/Create</a></li>");
                            menuAppend.Append("<li><a href=\"#\">Upload</a><ul>");
                            menuAppend.Append("<li><a href=\"tyredrawingupload.aspx?aid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"tyredrawingupload.aspx?aid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                            menuAppend.Append("<li><a href=\"#\">Edit</a><ul>");
                            menuAppend.Append("<li><a href=\"tyredrawingedit.aspx?aid=1&pid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"tyredrawingedit.aspx?aid=1&pid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                            menuAppend.Append("<li><a href=\"#\">Not Approved</a><ul>");
                            menuAppend.Append("<li><a href=\"tyredrawingedit.aspx?aid=2&pid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"tyredrawingedit.aspx?aid=2&pid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dwg_edc_level1"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">EDC Approve Level1</a><ul>");
                            menuAppend.Append("<li><a href=\"tyredrawingapprove.aspx?qid=edc1&pid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"tyredrawingapprove.aspx?qid=edc1&pid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dwg_edc_level1"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">EDC Approve Level2</a><ul>");
                            menuAppend.Append("<li><a href=\"tyredrawingapprove.aspx?qid=edc2&pid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"tyredrawingapprove.aspx?qid=edc2&pid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dwg_edc_level1"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">EDC Approve Level3</a><ul>");
                            menuAppend.Append("<li><a href=\"tyredrawingapprove.aspx?qid=edc3&pid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"tyredrawingapprove.aspx?qid=edc3&pid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dwg_crm_approve"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">Design Approve</a><ul>");
                            menuAppend.Append("<li><a href=\"tyredrawingapprove.aspx?qid=crm&pid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"tyredrawingapprove.aspx?qid=crm&pid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dwg_allocate"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">Allocation</a><ul>");
                            menuAppend.Append("<li><a href=\"#\">New</a><ul>");
                            menuAppend.Append("<li><a href=\"TyreDwgAllocation.aspx?pid=new&aid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"TyreDwgAllocation.aspx?pid=new&aid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                            menuAppend.Append("<li><a href=\"#\">Edit</a><ul>");
                            menuAppend.Append("<li><a href=\"TyreDwgAllocation.aspx?pid=edit&aid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"TyreDwgAllocation.aspx?pid=edit&aid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dwg_download"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">Download</a><ul>");
                            menuAppend.Append("<li><a href=\"tyredrawingdownload.aspx?pid=cus\">Customer</a></li>");
                            menuAppend.Append("<li><a href=\"tyredrawingdownload.aspx?pid=sup\">Supplier</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        menuAppend.Append("</ul></li>");
                        if (dt.Rows[0]["roomreserve"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"roomreservations.aspx\">City Office Room Reservation</a></li>");
                    }
                    if (Request.Cookies["TTSUser"].Value.ToLower() == "somu" || Request.Cookies["TTSUser"].Value.ToLower() != "anand" ||
                        Request.Cookies["TTSUser"].Value.ToLower() == "balamurugan" || Request.Cookies["TTSUser"].Value.ToLower() == "arun")
                        menuAppend.Append("<li><a href=\"typecostforaf.aspx\">Tally Type Cost</a></li>");
                    menuAppend.Append("</ul></li>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SctosDomestic_MenuList(StringBuilder menuAppend, DataTable dt, string strImg)
        {
            try
            {
                if (dt.Rows[0]["dom_quote"].ToString() == "True" || dt.Rows[0]["dom_orderentry"].ToString() == "True" || dt.Rows[0]["dom_plantassign"].ToString() == "True"
                    || dt.Rows[0]["dom_proforma"].ToString() == "True" || dt.Rows[0]["dom_substitution"].ToString() == "True" || dt.Rows[0]["dom_paymentconfirm"].ToString() == "True"
                    || dt.Rows[0]["dom_invoice"].ToString() == "True" || dt.Rows[0]["dom_ordertrack"].ToString() == "True" || dt.Rows[0]["dom_pricechange"].ToString() == "True"
                    || dt.Rows[0]["dom_ordersplit"].ToString() == "True" || dt.Rows[0]["dom_revise"].ToString() == "True" || dt.Rows[0]["dom_usermaster"].ToString() == "True"
                    || dt.Rows[0]["dom_paymentcontrol"].ToString() == "True" || dt.Rows[0]["dom_leadassign"].ToString() == "True" || dt.Rows[0]["dom_pricecreate"].ToString() == "True"
                    || dt.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dt.Rows[0]["dom_pdi_pdk"].ToString() == "True" || dt.Rows[0]["dom_sunprime_jsd"].ToString() == "True"
                    || dt.Rows[0]["ot_qcmmn"].ToString() == "True" || dt.Rows[0]["ot_qcpdk"].ToString() == "True" || dt.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True"
                    || dt.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True")
                {
                    menuAppend.Append("<li><a href=\"#\">" + strImg + "SCOTS Domestic</a><ul>");
                    if (dt.Rows[0]["dom_quote"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Quotation</a><ul>");
                        menuAppend.Append("<li><a href=\"quotegradewiseprepare.aspx\">Prepare</a></li>");
                        menuAppend.Append("<li><a href=\"quoteincomplete.aspx?qid=1\">Incomplete</a></li>");
                        menuAppend.Append("<li><a href=\"quoteincomplete.aspx?qid=2\">Revise</a></li>");
                        menuAppend.Append("<li><a href=\"quotestatus.aspx\">Confirmed</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["dom_orderentry"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Order Entry</a><ul>");
                        menuAppend.Append("<li><a href=\"cotsmanualorderprepare.aspx\">New</a></li>");
                        menuAppend.Append("<li><a href=\"cotsmanualorderincomplete.aspx?fid=" + Utilities.Encrypt("d") + "\">Incomplete</a></li>");
                        menuAppend.Append("<li><a href=\"stockorderprepare.aspx?fid=" + Utilities.Encrypt("d") + "&sid=" + Utilities.Encrypt("") +
                            "\">Stock Order Prepare</a></li>");
                        menuAppend.Append("<li><a href=\"Stockorderrevise.aspx?fid=" + Utilities.Encrypt("d") + "&qid=" + Utilities.Encrypt("1") +
                            "\">Stock Order Incomplete</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["dom_plantassign"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"cotsorderplantassign.aspx\">Plant Assign</a></li>");
                    if (dt.Rows[0]["dom_proforma"].ToString() == "True" || dt.Rows[0]["dom_substitution"].ToString() == "True" || dt.Rows[0]["dom_paymentconfirm"].ToString() == "True"
                        || dt.Rows[0]["dom_invoice"].ToString() == "True" || dt.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dt.Rows[0]["dom_pdi_pdk"].ToString() == "True"
                        || dt.Rows[0]["ot_qcmmn"].ToString() == "True" || dt.Rows[0]["ot_qcpdk"].ToString() == "True" || dt.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True"
                        || dt.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True" || dt.Rows[0]["dom_earmark_mmn"].ToString() == "True" || dt.Rows[0]["dom_earmark_pdk"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Order Process</a><ul>");
                        menuAppend.Append("<li><a href=\"cotsreviseresponse.aspx\">Approve For Revisions</a></li>");
                        if (dt.Rows[0]["dom_proforma"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"cotsorderprocess.aspx?qstring=" + Utilities.Encrypt("proforma") + "\">Proforma Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"cotsorderprocess.aspx?qstring=" + Utilities.Encrypt("workorder") + "\">Workorder Prepare</a></li>");
                        }
                        if (dt.Rows[0]["dom_earmark_mmn"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">MMN Stencil</a><ul>");
                            menuAppend.Append("<li><a href=\"earmark.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">Assign</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_earmark_revoke.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">Approve/ Revoke</a></li>");
                            menuAppend.Append("<li><a href=\"exppc_reject_revoke.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                               "\">QC Rejected Revoke</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dom_earmark_pdk"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PDK Stencil</a><ul>");
                            menuAppend.Append("<li><a href=\"earmark.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">Assign</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_earmark_revoke.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">Approve/ Revoke</a></li>");
                            menuAppend.Append("<li><a href=\"exppc_reject_revoke.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                               "\">QC Rejected Revoke</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["ot_qcmmn"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PPC MMN</a><ul>");
                            menuAppend.Append("<li><a href=\"expppc_verification1.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">RFD Assign</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_revision.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">RFD/Stock Revision</a></li>");
                            menuAppend.Append("<li><a href=\"cotssubstitution.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">Substitution/Liquidation</a></li>");
                            menuAppend.Append("<li><a href=\"stockorder_ppc.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">Stock Order</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["ot_qcpdk"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PPC PDK</a><ul>");
                            menuAppend.Append("<li><a href=\"expppc_verification1.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">RFD Assign</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_revision.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">RFD/Stock Revision</a></li>");
                            menuAppend.Append("<li><a href=\"cotssubstitution.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">Substitution/Liquidation</a></li>");
                            menuAppend.Append("<li><a href=\"stockorder_ppc.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">Stock Order</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dom_pdi_mmn"].ToString() == "True" || dt.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PDI MMN</a><ul>");
                            if (dt.Rows[0]["dom_pdi_mmn"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"cotspdiinspect.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("d") + "\">Scan For Inspection</a></li>");
                                menuAppend.Append("<li><a href=\"expscanincomplete.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("d") +
                                    "&rtype=" + Utilities.Encrypt("inspect") + "\">Inspection Incomplete</a></li>");
                                menuAppend.Append("<li><a href=\"expscancontainer.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("d") + "\">Loading Check</a></li>");
                            }
                            if (dt.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("d") +
                                    "&rtype=" + Utilities.Encrypt("delete") + "\">Barcode Delete</a></li>");
                                menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("d") +
                                    "&rtype=" + Utilities.Encrypt("approval") + "\">Approval For Loading</a></li>");
                                menuAppend.Append("<li><a href=\"expscaninspectionreport.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("d") +
                                    "\">Final Inspection Report Prepare</a></li>");
                            }
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dom_pdi_pdk"].ToString() == "True" || dt.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PDI PDK</a><ul>");
                            if (dt.Rows[0]["dom_pdi_pdk"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"cotspdiinspect.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("d") + "\">Scan For Inspection</a></li>");
                                menuAppend.Append("<li><a href=\"expscanincomplete.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("d") +
                                    "&rtype=" + Utilities.Encrypt("inspect") + "\">Inspection Incomplete</a></li>");
                                menuAppend.Append("<li><a href=\"expscancontainer.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("d") + "\">Loading Check</a></li>");
                            }
                            if (dt.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("d") +
                                    "&rtype=" + Utilities.Encrypt("delete") + "\">Barcode Delete</a></li>");
                                menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("d") +
                                    "&rtype=" + Utilities.Encrypt("approval") + "\">Approval For Loading</a></li>");
                                menuAppend.Append("<li><a href=\"expscaninspectionreport.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("d") +
                                    "\">Final Inspection Report Prepare</a></li>");
                            }
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["dom_invoice"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"cotsorderprocess.aspx?qstring=" + Utilities.Encrypt("invoice") + "\">Invoice Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"cotsrevisecancel1.aspx?type=" + Utilities.Encrypt("eic") + "\">e-Invoice Cancel</a></li>");
                        }
                        if (dt.Rows[0]["dom_paymentconfirm"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"cotsorderprocess.aspx?qstring=" + Utilities.Encrypt("payment") + "\">Payment Confirmation</a></li>");
                            menuAppend.Append("<li><a href=\"cotsorderprocess.aspx?qstring=" + Utilities.Encrypt("tcs") + "\">Tcs Confirmation</a></li>");
                        }
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["ot_qcmmn"].ToString() == "True" || dt.Rows[0]["ot_qcpdk"].ToString() == "True" || dt.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True" ||
                        dt.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True" || dt.Rows[0]["dom_paymentconfirm"].ToString() == "True" || dt.Rows[0]["dom_invoice"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Dispatch Return</a><ul>");
                        if (dt.Rows[0]["ot_qcmmn"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturn.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">MMN Return Entry</a></li>");
                        if (dt.Rows[0]["ot_qcpdk"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturn.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">PDK Return Entry</a></li>");
                        if (dt.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnqc.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">MMN Return Inspection</a></li>");
                        if (dt.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnqc.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">PDK Return Inspection</a></li>");
                        if (dt.Rows[0]["dom_paymentconfirm"].ToString() == "True" || dt.Rows[0]["dom_invoice"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"stockreturnaf.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">MMN Return Credit Note</a></li>");
                            menuAppend.Append("<li><a href=\"stockreturnaf.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">PDK Return Credit Note</a></li>");
                        }
                        if (dt.Rows[0]["ot_qcmmn"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnfg.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("d") +
                                "\">MMN Return Move To Godown</a></li>");
                        if (dt.Rows[0]["ot_qcpdk"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnfg.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("d") +
                                "\">PDK Return Move To Godown</a></li>");
                        if (dt.Rows[0]["ot_qcmmn"].ToString() == "True" || dt.Rows[0]["dom_pdi_mmn_approval"].ToString() == "True" ||
                            dt.Rows[0]["ot_qcpdk"].ToString() == "True" || dt.Rows[0]["dom_pdi_pdk_approval"].ToString() == "True" ||
                            dt.Rows[0]["dom_paymentconfirm"].ToString() == "True" || dt.Rows[0]["dom_invoice"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnreview.aspx?fid=" + Utilities.Encrypt("d") + "\">Return Review</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["dom_ordertrack"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"expscanreview.aspx?fid=" + Utilities.Encrypt("d") + "\">PDI Report</a></li>");
                    }
                    if (dt.Rows[0]["dom_paymentcontrol"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Customer Credit</a><ul>");
                        menuAppend.Append("<li><a href=\"cotscustomerholdrelease.aspx?qid=0\">Hold</a></li>");
                        menuAppend.Append("<li><a href=\"cotscustomerholdrelease.aspx?qid=1\">Revoke</a></li>");
                        menuAppend.Append("<li><a href=\"cotscustomercreditperiod.aspx\">Credit Period/ Sales Limit</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["dom_leadassign"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Customer</a><ul>");
                        menuAppend.Append("<li><a href=\"cotsuserreassign.aspx?cid=lead\">Lead Change</a></li>");
                        menuAppend.Append("<li><a href=\"cotsuserreassign.aspx?cid=type\">Type Change</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["dom_pricechange"].ToString() == "True" || dt.Rows[0]["dom_ordersplit"].ToString() == "True" || dt.Rows[0]["dom_revise"].ToString() == "True"
                        || dt.Rows[0]["dom_orderentry"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">CRM Revisions</a><ul>");
                        menuAppend.Append("<li><a href=\"cotsreviserequest.aspx\">Request For Revise</a></li>");
                        menuAppend.Append("<li><a href=\"expppc_revision.aspx?pid=" + Utilities.Encrypt("1") + "&fid=" + Utilities.Encrypt("d") +
                            "\">Approve For PPC Revision</a></li>");
                        if (dt.Rows[0]["dom_pricechange"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"cotspricechange.aspx\">Price Change</a></li>");
                        if (dt.Rows[0]["dom_ordersplit"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"cotsordersplit.aspx?fid=" + Utilities.Encrypt("d") + "\">Qty Split</a></li>");
                        if (dt.Rows[0]["dom_revise"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"cotsrevisecancel1.aspx?type=" + Utilities.Encrypt("r") + "\">Revise</a></li>");
                            menuAppend.Append("<li><a href=\"cotsrevisecancel1.aspx?type=" + Utilities.Encrypt("c") + "\">Cancel</a></li>");
                        }
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["dom_ordertrack"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Order Wise Track</a><ul>");
                        menuAppend.Append("<li><a href=\"cotstrackstatus.aspx\">Pending</a></li>");
                        menuAppend.Append("<li><a href=\"cotsdispatchedlist.aspx?disid=dom\">Dispatched</a></li>");
                        menuAppend.Append("<li><a href=\"cotscustwisetrack.aspx?disid=dom\">Customer Order Dump</a></li>");
                        menuAppend.Append("<li><a href=\"Stockorderrevise.aspx?fid=" + Utilities.Encrypt("d") + "&qid=" + Utilities.Encrypt("3") +
                            "\">Stock Order Track</a></li>");
                        menuAppend.Append("<li><a href=\"earmarkstencillist.aspx?disid=dom\">Stencil Assigned List</a></li>");
                        menuAppend.Append("<li><a href=\"#\">Dispatched Report Dump</a><ul>");
                        menuAppend.Append("<li><a href=\"filterwiserequest.aspx?fid=" + Utilities.Encrypt("d") + "&fileid=" + Utilities.Encrypt("dis") +
                            "&qplant=&qfyear=&qfmonth=&qtyear=&qtmonth\">Stencil Wise</a></li>");
                        menuAppend.Append("<li><a href=\"filterwiserequest.aspx?fid=" + Utilities.Encrypt("d") + "&fileid=" + Utilities.Encrypt("inv") +
                            "&qplant=&qfyear=&qfmonth=&qtyear=&qtmonth\">Item Wise</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["dom_orderentry"].ToString() == "True" || dt.Rows[0]["dom_proforma"].ToString() == "True" || dt.Rows[0]["dom_invoice"].ToString() == "True"
                        || dt.Rows[0]["dom_paymentconfirm"].ToString() == "True" || dt.Rows[0]["dom_pricechange"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"cotsinvoicepricereport.aspx\">Previous Price Analyze</a></li>");
                    }
                    if (dt.Rows[0]["dom_usermaster"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Customer Master</a><ul>");
                        menuAppend.Append("<li><a href=\"#\">User-ID</a><ul>");
                        menuAppend.Append("<li><a href=\"cotsusercreate.aspx?uid=new\">Create</a></li>");
                        menuAppend.Append("<li><a href=\"cotsusercreate.aspx?uid=modify\">Update</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"#\">Address</a><ul>");
                        menuAppend.Append("<li><a href=\"cotsdomesticaddress.aspx?qid=2&cid=\">Billing</a></li>");
                        menuAppend.Append("<li><a href=\"cotsdomesticaddress.aspx?qid=1&cid=\">Shipping</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"cotscustdomestic.aspx\">Approve Grade & Discount</a></li>");
                        menuAppend.Append("<li><a href=\"cotspricecreate.aspx\">List Price Prepare</a></li>");
                        menuAppend.Append("<li><a href=\"cotspriceapproval.aspx?fid=" + Utilities.Encrypt("d") + "\">Price Sheet Approval</a></li>");
                        menuAppend.Append("<li><a href=\"cotscommon.aspx?gid=dom\">General Announcement</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (Request.Cookies["TTSUser"].Value.ToLower() == "somu" || Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                    {
                        menuAppend.Append("<li><a href=\"cotsdomesticmaillist.aspx?mid=dom\">Domestic Order Mail Alert</a></li>");
                        menuAppend.Append("<li><a href=\"quotediscount.aspx\">Quote Discount</a></li>");
                    }
                    if (dt.Rows[0]["dom_invoice"].ToString() == "True" || dt.Rows[0]["dom_ordertrack"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Fitment Order</a><ul>");
                        menuAppend.Append("<li><a href=\"#\">MMN</a><ul>");
                        if (dt.Rows[0]["dom_invoice"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"cotsftregister.aspx?pid=mmn\">Received Entry</a></li>");
                            menuAppend.Append("<li><a href=\"cotsftinvoice.aspx?pid=mmn\">Invoice Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"cotsfttrack.aspx?pid=" + Utilities.Encrypt("mmn") + "\">Pending</a></li>");
                        }
                        menuAppend.Append("<li><a href=\"cotsfttordertrrack.aspx?pid=" + Utilities.Encrypt("mmn") + "\">Dispatched</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"#\">PDK</a><ul>");
                        if (dt.Rows[0]["dom_invoice"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"cotsftregister.aspx?pid=pdk\">Received Entry</a></li>");
                            menuAppend.Append("<li><a href=\"cotsftinvoice.aspx?pid=pdk\">Invoice Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"cotsfttrack.aspx?pid=" + Utilities.Encrypt("pdk") + "\">Pending</a></li>");
                        }
                        menuAppend.Append("<li><a href=\"cotsfttordertrrack.aspx?pid=" + Utilities.Encrypt("pdk") + "\">Dispatched</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["dom_sunprime_jsd"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"cotssunprimeprocess.aspx?gid=jsd\">Sunprime JSD</a><li>");
                    menuAppend.Append("<li><a href=\"customerwisesalesanalysis.aspx?sale=" + Utilities.Encrypt("DOMESTIC") + "\">Sales Analysis</a></li>");
                    menuAppend.Append("</ul></li>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_SctosExport_MenuList(StringBuilder menuAppend, DataTable dt, string strImg)
        {
            try
            {
                if (dt.Rows[0]["exp_orderentry"].ToString() == "True" || dt.Rows[0]["exp_plantassign"].ToString() == "True"
                    || dt.Rows[0]["exp_proforma"].ToString() == "True" || dt.Rows[0]["exp_substitution"].ToString() == "True" || dt.Rows[0]["exp_documents"].ToString() == "True"
                    || dt.Rows[0]["exp_ordertrack"].ToString() == "True" || dt.Rows[0]["exp_pricechange"].ToString() == "True" || dt.Rows[0]["exp_ordersplit"].ToString() == "True"
                    || dt.Rows[0]["exp_revise"].ToString() == "True" || dt.Rows[0]["exp_usermaster"].ToString() == "True" || dt.Rows[0]["exp_paymentcontrol"].ToString() == "True"
                    || dt.Rows[0]["exp_leadassign"].ToString() == "True" || dt.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dt.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dt.Rows[0]["exp_pdi_sitl"].ToString() == "True"
                    || dt.Rows[0]["exp_pdi_pdk"].ToString() == "True" || dt.Rows[0]["ot_ppcmmn"].ToString() == "True" || dt.Rows[0]["ot_ppc_sltl"].ToString() == "True" || dt.Rows[0]["ot_ppc_sitl"].ToString() == "True"
                    || dt.Rows[0]["ot_ppcpdk"].ToString() == "True" || dt.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" || dt.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True" || dt.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True"
                    || dt.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True" || dt.Rows[0]["exp_earmark_mmn"].ToString() == "True" || dt.Rows[0]["exp_earmark_sltl"].ToString() == "True" || dt.Rows[0]["exp_earmark_sitl"].ToString() == "True"
                    || dt.Rows[0]["exp_earmark_pdk"].ToString() == "True" || dt.Rows[0]["prod_approval_mmn"].ToString() == "True" || dt.Rows[0]["prod_approval_sltl"].ToString() == "True" || dt.Rows[0]["prod_approval_sitl"].ToString() == "True"
                    || dt.Rows[0]["prod_approval_pdk"].ToString() == "True")
                {
                    menuAppend.Append("<li><a href=\"#\">" + strImg + "SCOTS International</a><ul>");
                    if (dt.Rows[0]["exp_orderentry"].ToString() == "True" || Request.Cookies["TTSUserDepartment"].Value == "EDC"
                        || Request.Cookies["TTSUserDepartment"].Value == "QC" || Request.Cookies["TTSUserDepartment"].Value == "PPC")
                    {
                        menuAppend.Append("<li><a href=\"#\">Order Entry</a><ul>");
                        if (dt.Rows[0]["exp_orderentry"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"exportmanual1.aspx\">New</a></li>");
                            menuAppend.Append("<li><a href=\"cotsmanualorderincomplete.aspx?fid=" + Utilities.Encrypt("e") + "\">Incomplete</a></li>");
                            menuAppend.Append("<li><a href=\"stockorderprepare.aspx?fid=" + Utilities.Encrypt("e") + "&sid=" + Utilities.Encrypt("") +
                                "\">Stock Order Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"Stockorderrevise.aspx?fid=" + Utilities.Encrypt("e") + "&qid=" + Utilities.Encrypt("1") +
                                "\">Stock Order Incomplete</a></li>");
                        }
                        if (Request.Cookies["TTSUserDepartment"].Value == "EDC" || Request.Cookies["TTSUserDepartment"].Value == "QC"
                            || Request.Cookies["TTSUserDepartment"].Value == "PPC")
                        {
                            menuAppend.Append("<li><a href=\"stockorderprepare.aspx?fid=" + Utilities.Encrypt("t") + "&sid=" + Utilities.Encrypt("") +
                            "\">Trial Order Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"Stockorderrevise.aspx?fid=" + Utilities.Encrypt("t") + "&qid=" + Utilities.Encrypt("1") +
                                "\">Trial Order Incomplete</a></li>");
                        }
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["exp_plantassign"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"exportorderplantassign.aspx\">Plant Assign</a></li>");

                    if (dt.Rows[0]["exp_proforma"].ToString() == "True" || dt.Rows[0]["exp_substitution"].ToString() == "True" || dt.Rows[0]["exp_documents"].ToString() == "True"
                        || dt.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dt.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dt.Rows[0]["exp_pdi_sitl"].ToString() == "True"
                        || dt.Rows[0]["exp_pdi_pdk"].ToString() == "True" || dt.Rows[0]["ot_ppcmmn"].ToString() == "True" || dt.Rows[0]["ot_ppc_sltl"].ToString() == "True"
                        || dt.Rows[0]["ot_ppc_sitl"].ToString() == "True" || dt.Rows[0]["ot_ppcpdk"].ToString() == "True" || dt.Rows[0]["ot_logisticsmmn"].ToString() == "True"
                        || dt.Rows[0]["ot_logistics_sltl"].ToString() == "True" || dt.Rows[0]["ot_logistics_sitl"].ToString() == "True" || dt.Rows[0]["ot_logisticspdk"].ToString() == "True"
                        || dt.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" || dt.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True" || dt.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True"
                        || dt.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True" || dt.Rows[0]["prod_approval_mmn"].ToString() == "True" || dt.Rows[0]["prod_approval_sltl"].ToString() == "True"
                        || dt.Rows[0]["prod_approval_sitl"].ToString() == "True" || dt.Rows[0]["prod_approval_pdk"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Order Process</a><ul>");
                        if (dt.Rows[0]["prod_approval_mmn"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"expprodapprove.aspx?pid=" + Utilities.Encrypt("MMN") + "\">Production Approve - MMN</a></li>");
                        //if (dt.Rows[0]["prod_approval_sltl"].ToString() == "True")
                        //    menuAppend.Append("<li><a href=\"expprodapprove.aspx?pid=" + Utilities.Encrypt("SLTL") + "\">Production Approve - SLTL</a></li>");
                        //if (dt.Rows[0]["prod_approval_sitl"].ToString() == "True")
                        //    menuAppend.Append("<li><a href=\"expprodapprove.aspx?pid=" + Utilities.Encrypt("SITL") + "\">Production Approve - SITL</a></li>");
                        if (dt.Rows[0]["prod_approval_pdk"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"expprodapprove.aspx?pid=" + Utilities.Encrypt("PDK") + "\">Production Approve - PDK</a></li>");

                        menuAppend.Append("<li><a href=\"Exp_Revise_ReqRes.aspx?type=" + Utilities.Encrypt("response") + "\">Approve For Revisions</a></li>");
                        if (dt.Rows[0]["exp_proforma"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"exportcotsproforma.aspx\">Proforma Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"exportcotsproduction.aspx\">Workorder Prepare</a></li>");
                        }
                        if (dt.Rows[0]["exp_earmark_mmn"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">MMN Stencil</a><ul>");
                            menuAppend.Append("<li><a href=\"earmark.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">Assign</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_earmark_revoke.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">Approve/ Revoke</a></li>");
                            menuAppend.Append("<li><a href=\"exppc_reject_revoke.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                               "\">QC Rejected Revoke</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        //if (dt.Rows[0]["exp_earmark_sltl"].ToString() == "True")
                        //{
                        //    menuAppend.Append("<li><a href=\"#\">SLTL Stencil</a><ul>");
                        //    menuAppend.Append("<li><a href=\"earmark.aspx?pid=" + Utilities.Encrypt("SLTL") + "&fid=" + Utilities.Encrypt("e") +
                        //        "\">Assign</a></li>");
                        //    menuAppend.Append("<li><a href=\"expppc_earmark_revoke.aspx?pid=" + Utilities.Encrypt("SLTL") + "&fid=" + Utilities.Encrypt("e") +
                        //        "\">Approve/ Revoke</a></li>");
                        //    menuAppend.Append("<li><a href=\"exppc_reject_revoke.aspx?pid=" + Utilities.Encrypt("SLTL") + "&fid=" + Utilities.Encrypt("e") +
                        //       "\">QC Rejected Revoke</a></li>");
                        //    menuAppend.Append("</ul></li>");
                        //}
                        //if (dt.Rows[0]["exp_earmark_sitl"].ToString() == "True")
                        //{
                        //    menuAppend.Append("<li><a href=\"#\">SITL Stencil</a><ul>");
                        //    menuAppend.Append("<li><a href=\"earmark.aspx?pid=" + Utilities.Encrypt("SITL") + "&fid=" + Utilities.Encrypt("e") +
                        //        "\">Assign</a></li>");
                        //    menuAppend.Append("<li><a href=\"expppc_earmark_revoke.aspx?pid=" + Utilities.Encrypt("SITL") + "&fid=" + Utilities.Encrypt("e") +
                        //        "\">Approve/ Revoke</a></li>");
                        //    menuAppend.Append("<li><a href=\"exppc_reject_revoke.aspx?pid=" + Utilities.Encrypt("SITL") + "&fid=" + Utilities.Encrypt("e") +
                        //       "\">QC Rejected Revoke</a></li>");
                        //    menuAppend.Append("</ul></li>");
                        //}
                        if (dt.Rows[0]["exp_earmark_pdk"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PDK Stencil</a><ul>");
                            menuAppend.Append("<li><a href=\"earmark.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                "\">Assign</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_earmark_revoke.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                "\">Approve/ Revoke</a></li>");
                            menuAppend.Append("<li><a href=\"exppc_reject_revoke.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                               "\">QC Rejected Revoke</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["ot_ppcmmn"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PPC MMN</a><ul>");
                            menuAppend.Append("<li><a href=\"expppc_verification1.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">RFD Assign</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_revision.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">RFD/Stock Revision</a></li>");
                            menuAppend.Append("<li><a href=\"cotssubstitution.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">Substitution/Liquidation</a></li>");
                            menuAppend.Append("<li><a href=\"stockorder_ppc.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">Stock Order</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["ot_ppc_sltl"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PPC SLTL</a><ul>");
                            menuAppend.Append("<li><a href=\"stockinward.aspx?pid=" + Utilities.Encrypt("SLTL") + "\">Stock Inward</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_verification.aspx?pid=" + Utilities.Encrypt("SLTL") + "&fid=" + Utilities.Encrypt("e") +
                                "\">RFD Assign</a></li>");
                            //menuAppend.Append("<li><a href=\"expppc_revision.aspx?pid=" + Utilities.Encrypt("SLTL") + "&fid=" + Utilities.Encrypt("e") +
                            //    "\">RFD/Stock Revision</a></li>");
                            //menuAppend.Append("<li><a href=\"cotssubstitution.aspx?pid=" + Utilities.Encrypt("SLTL") + "&fid=" + Utilities.Encrypt("e") +
                            //    "\">Substitution/Liquidation</a></li>");
                            ////menuAppend.Append("<li><a href=\"stockorder_ppc.aspx?pid=" + Utilities.Encrypt("SLTL") + "&fid=" + Utilities.Encrypt("e") +
                            ////    "\">Stock Order</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["ot_ppc_sitl"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PPC SITL</a><ul>");
                            menuAppend.Append("<li><a href=\"stockinward.aspx?pid=" + Utilities.Encrypt("SITL") + "\">Stock Inward</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_verification.aspx?pid=" + Utilities.Encrypt("SITL") + "&fid=" + Utilities.Encrypt("e") +
                                "\">RFD Assign</a></li>");
                            //menuAppend.Append("<li><a href=\"expppc_revision.aspx?pid=" + Utilities.Encrypt("SITL") + "&fid=" + Utilities.Encrypt("e") +
                            //    "\">RFD/Stock Revision</a></li>");
                            //menuAppend.Append("<li><a href=\"cotssubstitution.aspx?pid=" + Utilities.Encrypt("SITL") + "&fid=" + Utilities.Encrypt("e") +
                            //    "\">Substitution/Liquidation</a></li>");
                            ////menuAppend.Append("<li><a href=\"stockorder_ppc.aspx?pid=" + Utilities.Encrypt("SITL") + "&fid=" + Utilities.Encrypt("e") +
                            ////    "\">Stock Order</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["ot_ppcpdk"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PPC PDK</a><ul>");
                            menuAppend.Append("<li><a href=\"expppc_verification1.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                "\">RFD Assign</a></li>");
                            menuAppend.Append("<li><a href=\"expppc_revision.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                 "\">RFD/Stock Revision</a></li>");
                            menuAppend.Append("<li><a href=\"cotssubstitution.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                "\">Substitution/Liquidation</a></li>");
                            menuAppend.Append("<li><a href=\"stockorder_ppc.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                "\">Stock Order</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["ot_logisticsmmn"].ToString() == "True" || dt.Rows[0]["ot_logistics_sltl"].ToString() == "True" || dt.Rows[0]["ot_logistics_sitl"].ToString() == "True" || dt.Rows[0]["ot_logisticspdk"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">Logistics Outbound</a><ul>");
                            menuAppend.Append("<li><a href=\"explogistic_entry.aspx?pid=" + Utilities.Encrypt("MMN") + "\">MMN</a></li>");
                            //menuAppend.Append("<li><a href=\"explogistic_entry.aspx?pid=" + Utilities.Encrypt("SLTL") + "\">SLTL</a></li>");
                            //menuAppend.Append("<li><a href=\"explogistic_entry.aspx?pid=" + Utilities.Encrypt("SITL") + "\">SITL</a></li>");
                            menuAppend.Append("<li><a href=\"explogistic_entry.aspx?pid=" + Utilities.Encrypt("PDK") + "\">PDK</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dt.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PDI MMN</a><ul>");
                            if (dt.Rows[0]["exp_pdi_mmn"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"cotspdiinspect.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("e") +
                                    "\">Scan For Inspection</a></li>");
                                menuAppend.Append("<li><a href=\"expscanincomplete.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("e") +
                                    "&rtype=" + Utilities.Encrypt("inspect") + "\">Inspection Incomplete</a></li>");
                            }
                            menuAppend.Append("<li><a href=\"expscancontainer.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("e") + "\">Loading Check</a></li>");
                            if (dt.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("e") +
                                    "&rtype=" + Utilities.Encrypt("delete") + "\">Barcode Delete</a></li>");
                                menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("e") +
                                    "&rtype=" + Utilities.Encrypt("approval") + "\">Approval For Loading</a></li>");
                                menuAppend.Append("<li><a href=\"expscaninspectionreport.aspx?pid=" + Utilities.Encrypt("mmn") + "&fid=" + Utilities.Encrypt("e") +
                                    "\">Final Inspection Report Prepare</a></li>");
                            }
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dt.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PDI SLTL</a><ul>");
                            if (dt.Rows[0]["exp_pdi_sltl"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"cotspdiinspect.aspx?pid=" + Utilities.Encrypt("sltl") + "&fid=" + Utilities.Encrypt("e") +
                                    "\">PDI Barcode Upload</a></li>");
                                //menuAppend.Append("<li><a href=\"expscanincomplete.aspx?pid=" + Utilities.Encrypt("sltl") + "&fid=" + Utilities.Encrypt("e") +
                                //    "&rtype=" + Utilities.Encrypt("inspect") + "\">Inspection Incomplete</a></li>");
                                //menuAppend.Append("<li><a href=\"expscancontainer.aspx?pid=" + Utilities.Encrypt("sltl") + "&fid=" + Utilities.Encrypt("e") + "\">Loading Check</a></li>");
                            }
                            if (dt.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True")
                            {
                                //menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("sltl") + "&fid=" + Utilities.Encrypt("e") +
                                //    "&rtype=" + Utilities.Encrypt("delete") + "\">Barcode Delete</a></li>");
                                //menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("sltl") + "&fid=" + Utilities.Encrypt("e") +
                                //    "&rtype=" + Utilities.Encrypt("approval") + "\">Approval For Loading</a></li>");
                                //menuAppend.Append("<li><a href=\"expscaninspectionreport.aspx?pid=" + Utilities.Encrypt("sltl") + "&fid=" + Utilities.Encrypt("e") +
                                //    "\">Final Inspection Report Prepare</a></li>");
                            }
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dt.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PDI SITL</a><ul>");
                            if (dt.Rows[0]["exp_pdi_sitl"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"cotspdiinspect.aspx?pid=" + Utilities.Encrypt("sitl") + "&fid=" + Utilities.Encrypt("e") +
                                    "\">PDI Barcode Upload</a></li>");
                                //menuAppend.Append("<li><a href=\"expscanincomplete.aspx?pid=" + Utilities.Encrypt("sitl") + "&fid=" + Utilities.Encrypt("e") +
                                //    "&rtype=" + Utilities.Encrypt("inspect") + "\">Inspection Incomplete</a></li>");
                                //menuAppend.Append("<li><a href=\"expscancontainer.aspx?pid=" + Utilities.Encrypt("sitl") + "&fid=" + Utilities.Encrypt("e") + "\">Loading Check</a></li>");
                            }
                            if (dt.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True")
                            {
                                //menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("sitl") + "&fid=" + Utilities.Encrypt("e") +
                                //    "&rtype=" + Utilities.Encrypt("delete") + "\">Barcode Delete</a></li>");
                                //menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("sitl") + "&fid=" + Utilities.Encrypt("e") +
                                //    "&rtype=" + Utilities.Encrypt("approval") + "\">Approval For Loading</a></li>");
                                //menuAppend.Append("<li><a href=\"expscaninspectionreport.aspx?pid=" + Utilities.Encrypt("sitl") + "&fid=" + Utilities.Encrypt("e") +
                                //    "\">Final Inspection Report Prepare</a></li>");
                            }
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["exp_pdi_pdk"].ToString() == "True" || dt.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PDI PDK</a><ul>");
                            if (dt.Rows[0]["exp_pdi_pdk"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"cotspdiinspect.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("e") +
                                    "\">Scan For Inspection</a></li>");
                                menuAppend.Append("<li><a href=\"expscanincomplete.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("e") +
                                    "&rtype=" + Utilities.Encrypt("inspect") + "\">Inspection Incomplete</a></li>");
                            }
                            menuAppend.Append("<li><a href=\"expscancontainer.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("e") + "\">Loading Check</a></li>");
                            if (dt.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True")
                            {
                                menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("e") +
                                    "&rtype=" + Utilities.Encrypt("delete") + "\">Barcode Delete</a></li>");
                                menuAppend.Append("<li><a href=\"expscanrevise.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("e") +
                                    "&rtype=" + Utilities.Encrypt("approval") + "\">Approval For Loading</a></li>");
                                menuAppend.Append("<li><a href=\"expscaninspectionreport.aspx?pid=" + Utilities.Encrypt("pdk") + "&fid=" + Utilities.Encrypt("e") +
                                    "\">Final Inspection Report Prepare</a></li>");
                            }
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["exp_documents"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"expinvoice.aspx\">Export Invoice Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"exp_documents.aspx?mid=" + Utilities.Encrypt("documents") + "\">Post Shipment Documents</a></li>");
                        }
                        menuAppend.Append("<li><a href=\"exp_documents.aspx?mid=" + Utilities.Encrypt("arrive") + "\">Delivered At Destination</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dt.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" ||
                        dt.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dt.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True" ||
                        dt.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dt.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True" ||
                        dt.Rows[0]["exp_pdi_pdk"].ToString() == "True" || dt.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Inter Plant Action</a><ul>");
                        if (dt.Rows[0]["exp_pdi_mmn"].ToString() == "True" || dt.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">MMN</a><ul>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["exp_pdi_sltl"].ToString() == "True" || dt.Rows[0]["exp_pdi_sltl_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">SLTL</a><ul>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["exp_pdi_sitl"].ToString() == "True" || dt.Rows[0]["exp_pdi_sitl_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">SITL</a><ul>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["exp_pdi_pdk"].ToString() == "True" || dt.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">PDK</a><ul>");
                            menuAppend.Append("</ul></li>");
                        }
                        menuAppend.Append("</ul></li>");
                    }

                    if (dt.Rows[0]["ot_ppcmmn"].ToString() == "True" || dt.Rows[0]["ot_ppcpdk"].ToString() == "True" || dt.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" ||
                        dt.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True" || dt.Rows[0]["exp_documents"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Dispatch Return</a><ul>");
                        if (dt.Rows[0]["ot_ppcmmn"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturn.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">MMN Return Entry</a></li>");
                        if (dt.Rows[0]["ot_ppcpdk"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturn.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                "\">PDK Return Entry</a></li>");
                        if (dt.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnqc.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">MMN Return Inspection</a></li>");
                        if (dt.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnqc.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                "\">PDK Return Inspection</a></li>");
                        if (dt.Rows[0]["exp_documents"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"stockreturnaf.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">MMN Return Credit Note</a></li>");
                            menuAppend.Append("<li><a href=\"stockreturnaf.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                "\">PDK Return Credit Note</a></li>");
                        }
                        if (dt.Rows[0]["ot_ppcmmn"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnfg.aspx?pid=" + Utilities.Encrypt("MMN") + "&fid=" + Utilities.Encrypt("e") +
                                "\">MMN Return Move To Godown</a></li>");
                        if (dt.Rows[0]["ot_ppcpdk"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnfg.aspx?pid=" + Utilities.Encrypt("PDK") + "&fid=" + Utilities.Encrypt("e") +
                                "\">PDK Return Move To Godown</a></li>");
                        if (dt.Rows[0]["ot_ppcmmn"].ToString() == "True" || dt.Rows[0]["exp_pdi_mmn_approval"].ToString() == "True" ||
                            dt.Rows[0]["ot_ppcpdk"].ToString() == "True" || dt.Rows[0]["exp_pdi_pdk_approval"].ToString() == "True" ||
                            dt.Rows[0]["exp_documents"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"stockreturnreview.aspx?fid=" + Utilities.Encrypt("e") + "\">Return Review</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["exp_ordertrack"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"expscanreview.aspx?fid=" + Utilities.Encrypt("e") + "\">PDI Report</a></li>");
                    }
                    if (dt.Rows[0]["exp_pricechange"].ToString() == "True" || dt.Rows[0]["exp_ordersplit"].ToString() == "True" || dt.Rows[0]["exp_revise"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">CRM Revisions</a><ul>");
                        menuAppend.Append("<li><a href=\"Exp_Revise_ReqRes.aspx?type=" + Utilities.Encrypt("request") + "\">Revise Request</a></li>");
                        menuAppend.Append("<li><a href=\"expppc_revision.aspx?pid=" + Utilities.Encrypt("1") + "&fid=" + Utilities.Encrypt("e") +
                            "\">Approve For PPC Revision</a></li>");
                        if (dt.Rows[0]["exp_pricechange"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"exppricechange.aspx\">Price Change</a></li>");
                        if (dt.Rows[0]["exp_ordersplit"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"exp_ordermodifyplantqty.aspx\">Split Or Item/Qty Changes To Another Plant</a></li>");
                            menuAppend.Append("<li><a href=\"cotsordersplit.aspx?fid=" + Utilities.Encrypt("e") + "\">Part Wise Split</a></li>");
                        }
                        if (dt.Rows[0]["exp_revise"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"exp_revisemasterdata.aspx\">Master Data Revise</a></li>");
                            menuAppend.Append("<li><a href=\"exp_orderrevise.aspx\">Item Revise</a></li>");
                            menuAppend.Append("<li><a href=\"expordercancel.aspx\">Order Cancel</a></li>");
                        }
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["exp_ordertrack"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Order Wise Track</a><ul>");
                        menuAppend.Append("<li><a href=\"exportcotstrackstatus.aspx\">Pending</a></li>");
                        menuAppend.Append("<li><a href=\"cotsdispatchedlist.aspx?disid=exp\">Dispatched</a></li>");
                        menuAppend.Append("<li><a href=\"Stockorderrevise.aspx?fid=" + Utilities.Encrypt("e") + "&qid=" + Utilities.Encrypt("3") +
                            "\">Stock Order Track</a></li>");
                        menuAppend.Append("<li><a href=\"earmarkstencillist.aspx?disid=exp\">Stencil Assigned List</a></li>");
                        menuAppend.Append("<li><a href=\"#\">Dispatched Report Dump</a><ul>");
                        menuAppend.Append("<li><a href=\"filterwiserequest.aspx?fid=" + Utilities.Encrypt("e") + "&fileid=" + Utilities.Encrypt("dis") +
                            "&qplant=&qfyear=&qfmonth=&qtyear=&qtmonth\">Stencil Wise</a></li>");
                        menuAppend.Append("<li><a href=\"filterwiserequest.aspx?fid=" + Utilities.Encrypt("e") + "&fileid=" + Utilities.Encrypt("inv") +
                            "&qplant=&qfyear=&qfmonth=&qtyear=&qtmonth\">Item Wise</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("</ul></li>");

                        menuAppend.Append("<li><a href=\"#\">Review</a><ul>");
                        menuAppend.Append("<li><a href=\"expreviewmeeting.aspx?pid=" + Utilities.Encrypt("MMN") + "\">MMN</a></li>");
                        menuAppend.Append("<li><a href=\"expreviewmeeting.aspx?pid=" + Utilities.Encrypt("SLTL") + "\">SLTL</a></li>");
                        menuAppend.Append("<li><a href=\"expreviewmeeting.aspx?pid=" + Utilities.Encrypt("SITL") + "\">SITL</a></li>");
                        menuAppend.Append("<li><a href=\"expreviewmeeting.aspx?pid=" + Utilities.Encrypt("PDK") + "\">PDK</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["exp_usermaster"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Customer Master</a><ul>");
                        menuAppend.Append("<li><a href=\"#\">User-ID</a><ul>");
                        menuAppend.Append("<li><a href=\"expusercreation.aspx?type=new\">Create</a></li>");
                        menuAppend.Append("<li><a href=\"expusercreation.aspx?type=modify\">Update</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"#\">Address</a><ul>");
                        menuAppend.Append("<li><a href=\"exportcotsaddress.aspx?qid=2&cid=\">Billing</a></li>");
                        menuAppend.Append("<li><a href=\"exportcotsaddress.aspx?qid=1&cid=\">Shipping</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"export_gradeapproval.aspx\">Approve Grade</a></li>");
                        menuAppend.Append("<li><a href=\"exportdocuments.aspx\">Required Documents</a></li>");
                        menuAppend.Append("<li><a href=\"exp_pricecreate.aspx\">Price Sheet Prepare</a></li>");
                        menuAppend.Append("<li><a href=\"cotspriceapproval.aspx?fid=" + Utilities.Encrypt("e") + "\">Price Sheet Approval</a></li>");
                        menuAppend.Append("<li><a href=\"rimpriceprepare.aspx\">Rim Price Prepare</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"cotscommon.aspx?gid=exp\">General Announcement</a></li>");
                    }
                    menuAppend.Append("<li><a href=\"customerwisesalesanalysis.aspx?sale=" + Utilities.Encrypt("EXPORT") + "\">Sales Analysis</a></li>");
                    //if (dt.Rows[0]["exp_cargo_masterdata"].ToString() == "True" || dt.Rows[0]["exp_cargo_planning"].ToString() == "True")
                    //{
                    //    menuAppend.Append("<li><a href=\"#\">" + strImg + "Cargo Management</a><ul>");
                    //    menuAppend.Append("<li><a href=\"#\">Order Entry</a><ul>");
                    //    menuAppend.Append("<li><a href=\"cargo_management.aspx?vid=0\">New</a></li>");
                    //    menuAppend.Append("<li><a href=\"cargo_management.aspx?vid=0e\">Revise</a></li>");
                    //    menuAppend.Append("</ul></li>");
                    //    menuAppend.Append("<li><a href=\"cargo_management.aspx?vid=1\">Tyre Dimension Details</a></li>");
                    //    menuAppend.Append("<li><a href=\"cargo_management.aspx?vid=2\">Container Details</a></li>");
                    //    menuAppend.Append("<li><a href=\"cargo_management.aspx?vid=3\">Load planning</a></li>");
                    //    menuAppend.Append("<li><a href=\"cargo_management.aspx?vid=4\">Edit Master Details</a></li>");
                    //    menuAppend.Append("</ul></li>");
                    //}
                    //if (Request.Cookies["TTSUser"].Value.ToLower() == "admin")
                    //    menuAppend.Append("<li><a href=\"cotsdomesticmaillist.aspx?mid=exp\">Export Order Mail Alert</a></li>");
                    menuAppend.Append("</ul></li>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_Claim_MenuList(StringBuilder menuAppend, DataTable dt, string strImg)
        {
            try
            {
                if (dt.Rows[0]["claim_stencilassign"].ToString() == "True" || dt.Rows[0]["claim_qc_mmn"].ToString() == "True" || dt.Rows[0]["claim_qc_sltl"].ToString() == "True" || dt.Rows[0]["claim_qc_sitl"].ToString() == "True"
                    || dt.Rows[0]["claim_qc_pdk"].ToString() == "True" || dt.Rows[0]["claim_edc_mmn"].ToString() == "True" || dt.Rows[0]["claim_edc_sltl"].ToString() == "True" || dt.Rows[0]["claim_edc_sitl"].ToString() == "True"
                    || dt.Rows[0]["claim_edc_pdk"].ToString() == "True" || dt.Rows[0]["claim_crm_exp"].ToString() == "True" || dt.Rows[0]["claim_crm_dom"].ToString() == "True"
                    || dt.Rows[0]["claim_creditnote_settle_exp"].ToString() == "True" || dt.Rows[0]["claim_creditnote_settle_dom"].ToString() == "True"
                    || dt.Rows[0]["claim_dispatch_mmn"].ToString() == "True" || dt.Rows[0]["claim_dispatch_pdk"].ToString() == "True" || dt.Rows[0]["claim_track"].ToString() == "True"
                    || dt.Rows[0]["claim_imagelibrary_entry"].ToString() == "True" || dt.Rows[0]["claim_imagelibrary_view"].ToString() == "True")
                {
                    menuAppend.Append("<li><a href=\"#\">Claim</a><ul>");
                    if (dt.Rows[0]["claim_stencilassign"].ToString() == "True" || dt.Rows[0]["claim_crm_exp"].ToString() == "True" || dt.Rows[0]["claim_crm_dom"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Register</a><ul>");
                        if (dt.Rows[0]["claim_stencilassign"].ToString() == "True" || dt.Rows[0]["claim_crm_exp"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"claimregister1.aspx?fid=" + Utilities.Encrypt("e") + "\">Export</a></li>");
                        if (dt.Rows[0]["claim_stencilassign"].ToString() == "True" || dt.Rows[0]["claim_crm_dom"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"claimregister1.aspx?fid=" + Utilities.Encrypt("d") + "\">Domestic</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["claim_stencilassign"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"claimstencilassign.aspx\">Stencil Assign To QC</a></li>");
                    if (dt.Rows[0]["claim_qc_mmn"].ToString() == "True" || dt.Rows[0]["claim_qc_sltl"].ToString() == "True" || dt.Rows[0]["claim_qc_sitl"].ToString() == "True" || dt.Rows[0]["claim_qc_pdk"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"claimQcAnalysis.aspx\">QC Analysis &rArr; To EDC</a></li>");
                    if (dt.Rows[0]["claim_edc_mmn"].ToString() == "True" || dt.Rows[0]["claim_edc_sltl"].ToString() == "True" || dt.Rows[0]["claim_edc_sitl"].ToString() == "True" || dt.Rows[0]["claim_edc_pdk"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"claimmovetocrm.aspx\">EDC Opinion &rArr; To CRM</a></li>");
                    if (dt.Rows[0]["claim_crm_exp"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">CRM Export</a><ul>");
                        menuAppend.Append("<li><a href=\"claimopinion.aspx?pid=e\">Opinion</a></li>");
                        menuAppend.Append("<li><a href=\"claimadditionaldetails.aspx?pid=e\">Additional Details Add</a></li>");
                        menuAppend.Append("<li><a href=\"claimcreditapprove.aspx?pid=e\">Credit Note Approval</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["claim_crm_dom"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">CRM Domestic</a><ul>");
                        menuAppend.Append("<li><a href=\"claimopinion.aspx?pid=d\">Opinion</a></li>");
                        menuAppend.Append("<li><a href=\"claimadditionaldetails.aspx?pid=d\">Additional Details Add</a></li>");
                        menuAppend.Append("<li><a href=\"claimcreditapprove.aspx?pid=d\">Credit Note Approval</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["claim_creditnote_settle_exp"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">A&F Export</a><ul>");
                        menuAppend.Append("<li><a href=\"claimsettle.aspx?cid=25&pid=e\">Credit Note Prepare</a></li>");
                        menuAppend.Append("<li><a href=\"claimsettle.aspx?cid=28&pid=e\">Invoice Adjustment & Settlement</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["claim_creditnote_settle_dom"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">A&F Domestic</a><ul>");
                        menuAppend.Append("<li><a href=\"claimsettle.aspx?cid=25&pid=d\">Credit Note Prepare</a></li>");
                        menuAppend.Append("<li><a href=\"claimsettle.aspx?cid=28&pid=d\">Invoice Adjustment & Settlement</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["claim_dispatch_mmn"].ToString() == "True" || dt.Rows[0]["claim_dispatch_pdk"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Domestic</a><ul>");
                        menuAppend.Append("<li><a href=\"claimfreereplacement.aspx\">Free Replacement Dispatch</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["claim_track"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Track</a><ul>");
                        menuAppend.Append("<li><a href=\"#\">Active</a><ul>");
                        menuAppend.Append("<li><a href=\"claimstatustrack.aspx?pid=e\">Export</a></li>");
                        menuAppend.Append("<li><a href=\"claimstatustrack.aspx?pid=d\">Domestic</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"#\">Closed</a><ul>");
                        menuAppend.Append("<li><a href=\"claimclosed.aspx?pid=e\">Export</a></li>");
                        menuAppend.Append("<li><a href=\"claimclosed.aspx?pid=d\">Domestic</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("<li><a href=\"#\">Review</a><ul>");
                        menuAppend.Append("<li><a href=\"claimreview.aspx?crid=c\">Customer</a></li>");
                        menuAppend.Append("<li><a href=\"claimreview.aspx?crid=g\">Global</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["claim_crm_exp"].ToString() == "True" || dt.Rows[0]["claim_creditnote_settle_exp"].ToString() == "True" || dt.Rows[0]["claim_track"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Service Complaint Export</a><ul>");
                        if (dt.Rows[0]["claim_crm_exp"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">CRM</a><ul>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=E&pid=1\">Register</a></li>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=E&pid=2\">Opinion</a></li>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=E&pid=4\">Credit Note Approval</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["claim_creditnote_settle_exp"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">A&F</a><ul>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=E&pid=3\">Credit Note Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=E&pid=5\">Settlement</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["claim_track"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">Track</a><ul>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=E&pid=0\">Active</a></li>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=E&pid=6\">Closed</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["claim_crm_dom"].ToString() == "True" || dt.Rows[0]["claim_creditnote_settle_dom"].ToString() == "True" || dt.Rows[0]["claim_track"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Service Complaint Domestic</a><ul>");
                        if (dt.Rows[0]["claim_crm_dom"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">CRM</a><ul>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=D&pid=1\">Register</a></li>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=D&pid=2\">Opinion</a></li>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=D&pid=4\">Credit Note Approval</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["claim_creditnote_settle_dom"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">A&F</a><ul>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=D&pid=3\">Credit Note Prepare</a></li>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=D&pid=5\">Settlement</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        if (dt.Rows[0]["claim_track"].ToString() == "True")
                        {
                            menuAppend.Append("<li><a href=\"#\">Track</a><ul>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=D&pid=0\">Active</a></li>");
                            menuAppend.Append("<li><a href=\"claimservicecomplaint.aspx?pcid=D&pid=6\">Closed</a></li>");
                            menuAppend.Append("</ul></li>");
                        }
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["claim_imagelibrary_entry"].ToString() == "True" || dt.Rows[0]["claim_imagelibrary_view"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Claim Library</a><ul>");
                        if (dt.Rows[0]["claim_imagelibrary_entry"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"claimlibraryentry.aspx\">Data Entry</a></li>");
                        if (dt.Rows[0]["claim_imagelibrary_view"].ToString() == "True")
                            menuAppend.Append("<li><a href=\"claimlibraryview.aspx\">View</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    menuAppend.Append("</ul></li>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_eBid_MenuList(StringBuilder menuAppend, DataTable dt, string strImg)
        {
            try
            {
                if (dt.Rows[0]["ebid_masterdata"].ToString() == "True" || dt.Rows[0]["ebid_track"].ToString() == "True" || dt.Rows[0]["ebid_process"].ToString() == "True")
                {
                    menuAppend.Append("<li><a href=\"#\">" + strImg + "e-Bid</a><ul>");
                    if (dt.Rows[0]["ebid_masterdata"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Master</a><ul>");
                        menuAppend.Append("<li><a href=\"ebid_purchase.aspx?vid=1\">Product</a></li>");
                        menuAppend.Append("<li><a href=\"ebid_purchase.aspx?vid=2\">Supplier</a></li>");
                        menuAppend.Append("<li><a href=\"#\">Allocate</a><ul>");
                        menuAppend.Append("<li><a href=\"ebid_purchase.aspx?vid=3\">Supplier Based Product</a></li>");
                        menuAppend.Append("<li><a href=\"ebid_purchase.aspx?vid=4\">Product Based Supplier</a></li>");
                        menuAppend.Append("</ul></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["ebid_track"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Track</a><ul>");
                        menuAppend.Append("<li><a href=\"default.aspx\">Enquiry</a></li>");
                        menuAppend.Append("<li><a href=\"default.aspx\">Purchase</a></li>");
                        menuAppend.Append("<li><a href=\"default.aspx\">Cancelled</a></li>");
                        menuAppend.Append("<li><a href=\"default.aspx\">Purchased</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["ebid_process"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Process</a><ul>");
                        menuAppend.Append("<li><a href=\"ebid_purchase.aspx?vid=5\">Enquiry Prepare</a></li>");
                        menuAppend.Append("<li><a href=\"ebid_purchase.aspx?vid=6\">Enquiry Sent List</a></li>");
                        menuAppend.Append("<li><a href=\"ebid_purchase.aspx?vid=7\">Supplier Quoted List</a></li>");
                        menuAppend.Append("<li><a href=\"ebid_purchase.aspx?vid=8\">Quote Analyze & PO Raise</a></li>");
                        menuAppend.Append("<li><a href=\"ebid_purchase.aspx?vid=9\">Enquiry Cancel</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    menuAppend.Append("</ul></li>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_Prospect_MenuList(StringBuilder menuAppend, DataTable dt, string strImg)
        {
            try
            {
                if (dt.Rows[0]["prospect_assign"].ToString() == "True" || dt.Rows[0]["prospect_leadfeedback"].ToString() == "True"
                    || dt.Rows[0]["prospect_reviewexp"].ToString() == "True" || dt.Rows[0]["prospect_reviewdom"].ToString() == "True")
                {
                    menuAppend.Append("<li><a href=\"#\">Prospect</a><ul>");
                    if (dt.Rows[0]["prospect_assign"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"prospectassign.aspx\">Assign</a></li>");
                        menuAppend.Append("<li><a href=\"prospectreassign.aspx\">Re-Assign</a></li>");
                    }
                    if (dt.Rows[0]["prospect_leadfeedback"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"prospectstatus.aspx\">Lead Feedback</a></li>");
                        menuAppend.Append("<li><a href=\"prospecthistory.aspx?custname=''\">Add Supplier History</a></li>");
                    }
                    if (dt.Rows[0]["prospect_reviewexp"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"prospectreview.aspx?proscode=null\">Review Export</a></li>");
                    if (dt.Rows[0]["prospect_reviewdom"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"prospectreviewdomestic.aspx?proscode=null\">Review Domestic</a></li>");
                    if (Request.Cookies["TTSUser"].Value.ToLower() == "admin" || Request.Cookies["TTSUser"].Value.ToLower() == "somu" ||
                        Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                    {
                        menuAppend.Append("<li><a href=\"prospectdelete.aspx\">Delete Prospect Customer</a></li>");
                        menuAppend.Append("<li><a href=\"movetoexistcustomer.aspx\">Prospect Move To Exist Customer</a></li>");
                        if (ConfigurationManager.AppSettings["pagetitle"].ToLower() != "sil")
                            menuAppend.Append("<li><a href=\"emailalert.aspx\">E-Mail Alert System</a></li>");
                    }
                    menuAppend.Append("</ul></li>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_Master_MenuList(StringBuilder menuAppend, DataTable dt, string strImg)
        {
            try
            {
                if (dt.Rows[0]["masterdata"].ToString() == "True" || dt.Rows[0]["userprivilege"].ToString() == "True" || dt.Rows[0]["customermaster"].ToString() == "True"
                    || dt.Rows[0]["pricesheet_build"].ToString() == "True" || dt.Rows[0]["ratesid_build"].ToString() == "True" || dt.Rows[0]["type_masterdata"].ToString() == "True")
                {
                    menuAppend.Append("<li><a href=\"#\">TTS Master</a><ul>");
                    if (dt.Rows[0]["customermaster"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Customer Master</a><ul>");
                        menuAppend.Append("<li><a href=\"customerdetails.aspx\">Create</a></li>");
                        menuAppend.Append("<li><a href=\"editcustdetails.aspx?ccode=\">Update</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["pricesheet_build"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Price Sheet</a><ul>");
                        menuAppend.Append("<li><a href=\"ratesmaster_customer.aspx?shid=" + Utilities.Encrypt("master") + "\">Master</a></li>");
                        menuAppend.Append("<li><a href=\"ratesmaster_customer.aspx?shid=" + Utilities.Encrypt("buffer") + "\">Buffer</a></li>");
                        menuAppend.Append("<li><a href=\"ratesmaster_customer.aspx?shid=" + Utilities.Encrypt("download") + "\">Download</a></li>");
                        //menuAppend.Append("<li><a href=\"custpricemaster.aspx\">Master</a></li>");
                        //menuAppend.Append("<li><a href=\"preparepricesheet.aspx\">Prepare</a></li>");
                        //menuAppend.Append("<li><a href=\"pricesheetcompare.aspx\">Compare</a></li>");
                        //menuAppend.Append("<li><a href=\"reportspricesheet.aspx\">Export/ Publish</a></li>");
                        menuAppend.Append("</ul></li>");
                    }
                    if (dt.Rows[0]["ratesid_build"].ToString() == "True")
                    {
                        menuAppend.Append("<li><a href=\"#\">Rates-ID</a><ul>");
                        menuAppend.Append("<li><a href=\"ratesmaster_v1.aspx?rmid=" + Utilities.Encrypt("prepare") + "\">Prepare</a></li>");
                        menuAppend.Append("<li><a href=\"ratesmaster_v1.aspx?rmid=" + Utilities.Encrypt("disable") + "\">Disable</a></li>");
                        menuAppend.Append("</ul></li>");
                        //menuAppend.Append("<li><a href=\"ratesmaster.aspx\">Rates-ID Prepare</a></li>");
                    }
                    if (dt.Rows[0]["type_masterdata"].ToString() == "True")
                    {
                        //menuAppend.Append("<li><a href=\"masterdataposition.aspx\">Type Ascending Position</a></li>");
                        menuAppend.Append("<li><a href=\"typeupgrade.aspx\">Assign Type Upgrade For Substitution</a></li>");
                        menuAppend.Append("<li><a href=\"typehardnessrange.aspx\">Tyre Hardness Range</a></li>");
                    }
                    if (dt.Rows[0]["UserPrivilege"].ToString() == "True")
                        menuAppend.Append("<li><a href=\"userprivilege.aspx\">TTS User Privileges</a></li>");
                    if (Request.Cookies["TTSUser"].Value.ToLower() == "admin" || Request.Cookies["TTSUser"].Value.ToLower() == "somu" ||
                        Request.Cookies["TTSUser"].Value.ToLower() != "anand")
                        menuAppend.Append("<li><a href=\"roomreservemaster.aspx\">Room Reservation Seeking Privileges</a></li>");

                    menuAppend.Append("</ul></li>");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
