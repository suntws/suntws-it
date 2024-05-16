using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Globalization;

namespace TTS.ebidpurchase
{
    public partial class enquiryprepare : System.Web.UI.UserControl
    {
        DataAccess daEBID = new DataAccess(ConfigurationManager.ConnectionStrings["eBidDB"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        if (Request["vid"].ToString() == "5" && Request["enqid"] == null)
                        {
                            DataTable dtEnqItem = new DataTable();
                            dtEnqItem.Columns.Add("ProdCategory", typeof(String));
                            dtEnqItem.Columns.Add("ProdDesc", typeof(String));
                            dtEnqItem.Columns.Add("ProdMeasurement", typeof(String));
                            dtEnqItem.Columns.Add("EnqItemQty", typeof(Int32));
                            dtEnqItem.Columns.Add("EnqPordID", typeof(Int32));
                            ViewState["dtEnqItem"] = dtEnqItem;

                            DataTable dt = (DataTable)daEBID.ExecuteReader_SP("sp_sel_ProdCategory", DataAccess.Return_Type.DataTable);
                            ddlProductCategory.DataSource = dt;
                            ddlProductCategory.DataTextField = "ProdCategory";
                            ddlProductCategory.DataValueField = "ProdCategory";
                            ddlProductCategory.DataBind();
                            ddlProductCategory.Items.Insert(0, "CHOOSE");
                        }
                        if (Request["enqid"] != null)
                        {
                            DataTable dt = (DataTable)daEBID.ExecuteReader_SP("sp_sel_ProdCategory", DataAccess.Return_Type.DataTable);
                            ddlProductCategory.DataSource = dt;
                            ddlProductCategory.DataTextField = "ProdCategory";
                            ddlProductCategory.DataValueField = "ProdCategory";
                            ddlProductCategory.DataBind();
                            ddlProductCategory.Items.Insert(0, "CHOOSE");
                            bindEnqDetails(Convert.ToInt32(Request["enqid"]));
                            ScriptManager.RegisterStartupScript(Page, GetType(), "EditMode", "toggleEditMode(0);", true);
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
                Utilities.WriteToErrorLog("ebid_enquiryprepare", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlProductCategory_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@ProdCategory", ddlProductCategory.SelectedItem.Text);
                DataTable dt = (DataTable)daEBID.ExecuteReader_SP("sp_sel_ProdDescription", sp1, DataAccess.Return_Type.DataTable);
                ddlProductDesc.DataSource = dt;
                ddlProductDesc.DataTextField = "ProdDesc";
                ddlProductDesc.DataValueField = "ID";
                ddlProductDesc.DataBind();
                ddlProductDesc.Items.Insert(0, "CHOOSE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlProductDesc_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp2 = new SqlParameter[2];
                sp2[0] = new SqlParameter("@ProdCategory", ddlProductCategory.SelectedItem.Text);
                sp2[1] = new SqlParameter("@ProdDesc", ddlProductDesc.SelectedItem.Text);
                DataTable dt = (DataTable)daEBID.ExecuteReader_SP("sp_sel_ProdMeasurement", sp2, DataAccess.Return_Type.DataTable);
                ddlMeasurement.DataSource = dt;
                ddlMeasurement.DataTextField = "ProdMeasurement";
                ddlMeasurement.DataValueField = "ProdMeasurement";
                ddlMeasurement.DataBind();
                ddlMeasurement.Items.Insert(0, "CHOOSE");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void ddlMeasurement_IndexChange(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp3 = new SqlParameter[3];
                sp3[0] = new SqlParameter("@ProdCategory", ddlProductCategory.SelectedItem.Text);
                sp3[1] = new SqlParameter("@ProdDesc", ddlProductDesc.SelectedItem.Text);
                sp3[2] = new SqlParameter("@ProdMeasurement", ddlMeasurement.SelectedItem.Text);
                hdnEnqProductID.Value = (string)daEBID.ExecuteScalar_SP("sp_sel_ProdID", sp3).ToString();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnPurchaseItemAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtEnqItem = ViewState["dtEnqItem"] as DataTable;
                DataRow[] rowChk = dtEnqItem.Select("EnqPordID=" + hdnEnqProductID.Value + "");
                if (rowChk.Length == 0)
                {
                    dtEnqItem.Rows.Add(ddlProductCategory.SelectedItem.Text, ddlProductDesc.SelectedItem.Text, ddlMeasurement.SelectedItem.Text, Convert.ToInt32(txtProductQty.Text), Convert.ToInt32(hdnEnqProductID.Value));
                    if (dtEnqItem.Rows.Count > 0)
                        Bind_GridView(dtEnqItem);
                }
                else
                    lblErrMsg.Text = "Item already added please check the below table";
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_GridView(DataTable dt1)
        {
            btnGotoEnqNext.Visible = false;
            gvPurchaseEnqItem.DataSource = dt1;
            gvPurchaseEnqItem.DataBind();
            ViewState["dtEnqItem"] = dt1;
            if (dt1.Rows.Count > 0)
                btnGotoEnqNext.Visible = true;
            ddlProductCategory.SelectedIndex = -1;
            ddlProductDesc.SelectedIndex = -1;
            ddlMeasurement.SelectedIndex = -1;
            txtProductQty.Text = "";
        }

        protected void gvPurchaseEnqItem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DataTable dtEnqItem = ViewState["dtEnqItem"] as DataTable;
                gvPurchaseEnqItem.EditIndex = e.NewEditIndex;
                Bind_GridView(dtEnqItem);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvPurchaseEnqItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                DataTable dtEnqItem = ViewState["dtEnqItem"] as DataTable;
                GridViewRow row = gvPurchaseEnqItem.Rows[e.RowIndex];
                HiddenField hdnEnqID = row.FindControl("hdnEnqID") as HiddenField;
                TextBox txtPurchaseQty = row.FindControl("txtPurchaseQty") as TextBox;

                if (hdnEnqID.Value != "" && txtPurchaseQty.Text != "" && dtEnqItem.Rows.Count > 0)
                {
                    foreach (DataRow iRow in dtEnqItem.Select("EnqPordID='" + hdnEnqID.Value + "'"))
                    {
                        iRow["EnqItemQty"] = txtPurchaseQty.Text;
                    }
                }
                gvPurchaseEnqItem.EditIndex = -1;
                Bind_GridView(dtEnqItem);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvPurchaseEnqItem_RowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                DataTable dtEnqItem = ViewState["dtEnqItem"] as DataTable;
                e.Cancel = true;
                gvPurchaseEnqItem.EditIndex = -1;
                Bind_GridView(dtEnqItem);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void gvPurchaseEnqItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DataTable dtEnqItem = ViewState["dtEnqItem"] as DataTable;
                GridViewRow row = gvPurchaseEnqItem.Rows[e.RowIndex];
                HiddenField hdnEnqID = row.FindControl("hdnEnqID") as HiddenField;
                if (hdnEnqID.Value != "" && dtEnqItem.Rows.Count > 0)
                {
                    foreach (DataRow iRow in dtEnqItem.Select("EnqPordID='" + hdnEnqID.Value + "'"))
                    {
                        iRow.Delete();
                    }
                    dtEnqItem.AcceptChanges();
                    Bind_GridView(dtEnqItem);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnGotoEnqNext_Click(object sender, EventArgs e)
        {
            try
            {
                string strID = string.Empty;
                foreach (GridViewRow row1 in gvPurchaseEnqItem.Rows)
                {
                    HiddenField hdnEnqID = row1.FindControl("hdnEnqID") as HiddenField;
                    if (strID.Length > 0)
                        strID += "," + hdnEnqID.Value;
                    else if (strID.Length == 0)
                        strID = hdnEnqID.Value;
                }


                SqlParameter[] sp4 = new SqlParameter[1];
                sp4[0] = new SqlParameter("@EnqProdID", strID);
                DataTable dt = (DataTable)daEBID.ExecuteReader_SP("sp_sel_EnqItem_AvailableSupplierList", sp4, DataAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    gvPurchaseEnqSupplier.DataSource = dt;
                    gvPurchaseEnqSupplier.DataBind();
                    ScriptManager.RegisterStartupScript(Page, GetType(), "JShow1", "ShowEnqAssignSupplier();", true);
                }
                else
                {
                    gvPurchaseEnqSupplier.DataSource = null;
                    gvPurchaseEnqSupplier.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToString();
            }
        }

        protected void btnEnqPrepareSave_Click(object sender, EventArgs e)
        {
            try
            {
                string strEnqExpireDate = txtEnqExpiredDate.Text;
                string strEnquiryComments = txtEnqComments.Text;
                string strUsername = Request.Cookies["TTSUser"].Value.ToString();
                HttpCookie userEmail = new HttpCookie("TTSUserEmail");
                userEmail = Request.Cookies["TTSUserEmail"];

                string strXmlProductList = "<ProductList>";
                for (int i = 0; i < gvPurchaseEnqItem.Rows.Count; i++)
                {
                    strXmlProductList += "<Product>";
                    HiddenField hdnProdId = (HiddenField)gvPurchaseEnqItem.Rows[i].FindControl("hdnEnqID");
                    Label txtProdDesc = (Label)gvPurchaseEnqItem.Rows[i].FindControl("lblDesc");
                    Label txtProdQty = (Label)gvPurchaseEnqItem.Rows[i].FindControl("lblPurchaseQTy");
                    string ProdId = hdnProdId.Value.ToString();
                    string ProductDesc = txtProdDesc.Text;
                    string productQty = txtProdQty.Text;
                    strXmlProductList += "<ProdId>" + ProdId + "</ProdId>";
                    strXmlProductList += "<ProductDesc>" + ProductDesc + "</ProductDesc>";
                    strXmlProductList += "<ProductQty>" + productQty + "</ProductQty>";
                    strXmlProductList += "</Product>";
                }
                strXmlProductList += "</ProductList>";

                string strXmlSupplierList = "<SupplierList>";
                for (int i = 0; i < gvPurchaseEnqSupplier.Rows.Count; i++)
                {
                    CheckBox cbo = (CheckBox)gvPurchaseEnqSupplier.Rows[i].Cells[0].FindControl("chkSupplierAssign");
                    if (cbo.Checked == true)
                    {
                        strXmlSupplierList += "<Supplier>";
                        HiddenField hdnText = (HiddenField)gvPurchaseEnqSupplier.Rows[i].Cells[0].FindControl("hdnSupID");
                        string supplierId = hdnText.Value.ToString();
                        string supplierName = (string)gvPurchaseEnqSupplier.Rows[i].Cells[1].Text.ToString();
                        strXmlSupplierList += "<SupplierId>" + supplierId + "</SupplierId>"
                                    + "<SupplierName>" + supplierName + "</SupplierName>";
                        strXmlSupplierList += "</Supplier>";
                    }
                }
                strXmlSupplierList += "</SupplierList>";

                string message = "";

                SqlConnection con = new SqlConnection(daEBID.ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "eBitData_SP_INS_Enquiries";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;
                SqlParameter param_EnquiryID = cmd.Parameters.Add("@EnquiryID", SqlDbType.Int);
                SqlParameter param_ExpiredDate = cmd.Parameters.Add("@ExpiredDate", SqlDbType.DateTime);
                SqlParameter param_EnqComments = cmd.Parameters.Add("@EnqComments", SqlDbType.NVarChar);
                SqlParameter param_EnqUsername = cmd.Parameters.Add("@EnqUsername", SqlDbType.NVarChar);
                SqlParameter param_EnqEmail = cmd.Parameters.Add("@EnqEmail", SqlDbType.NVarChar);
                SqlParameter param_EnqPhoneNo = cmd.Parameters.Add("@EnqPhoneNo", SqlDbType.Int);
                SqlParameter param_xmlSupplierData = cmd.Parameters.Add("@xmlSupplierData", SqlDbType.Xml);
                SqlParameter param_xmlProductData = cmd.Parameters.Add("@xmlProductData", SqlDbType.Xml);
                SqlParameter param_Message = cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 250);

                if (Request["enqid"] != null) param_EnquiryID.Value = Convert.ToInt32(Request["enqid"]);
                else param_EnquiryID.Value = DBNull.Value;

                if (strEnqExpireDate.ToString() != "")
                {
                    try
                    {
                        param_ExpiredDate.Value = DateTime.Parse(strEnqExpireDate);
                    }
                    catch (Exception)
                    {
                        param_ExpiredDate.Value = DateTime.ParseExact(strEnqExpireDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                else { param_ExpiredDate.Value = DBNull.Value; }
                param_EnqComments.Value = strEnquiryComments;
                param_EnqUsername.Value = strUsername;
                if (userEmail != null) param_EnqEmail.Value = userEmail.Value.ToString();
                else param_EnqEmail.Value = "";
                param_EnqPhoneNo.Value = DBNull.Value;
                param_xmlSupplierData.Value = strXmlSupplierList;
                param_xmlProductData.Value = strXmlProductList;
                param_Message.Direction = ParameterDirection.Output;
                param_Message.Value = message;
                cmd.ExecuteNonQuery();
                cmd = null;
                con.Close();
                message = param_Message.Value.ToString();

                Response.Redirect("ebid_purchase.aspx?vid=6", false);
                Response.End();
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void bindEnqDetails(int enqId)
        {
            try
            {
                SqlParameter[] SpParams = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId) };
                SqlParameter[] SpParams2 = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId) };
                DataTable dtEnquiries_Product = (DataTable)daEBID.ExecuteReader_SP("eBitData_SP_SEL_EnquiryDetails_Product", SpParams, DataAccess.Return_Type.DataTable);
                DataTable dtEnquiries_Supplier = (DataTable)daEBID.ExecuteReader_SP("eBitData_SP_SEL_EnquiryDetails_Supplier", SpParams2, DataAccess.Return_Type.DataTable);

                DataTable dtEnqItem = ViewState["dtEnqItem"] as DataTable;

                dtEnqItem = dtEnquiries_Product;
                if (dtEnqItem.Rows.Count > 0) Bind_GridView(dtEnqItem);
                btnGotoEnqNext_Click(null, null);

                for (int i = 0; i < dtEnquiries_Supplier.Rows.Count; i++)
                {
                    for (int j = 0; j < gvPurchaseEnqSupplier.Rows.Count; j++)
                    {
                        HiddenField supId = (HiddenField)gvPurchaseEnqSupplier.Rows[j].FindControl("hdnSupID");
                        CheckBox chkSupplier = (CheckBox)gvPurchaseEnqSupplier.Rows[j].FindControl("chkSupplierAssign");
                        if (supId.Value == "" + dtEnquiries_Supplier.Rows[i]["SuppId"])
                        {
                            chkSupplier.Checked = true;
                        }
                    }
                }
                SqlParameter[] SpParam3 = new SqlParameter[] { new SqlParameter("@EnquiryId", enqId) };
                DataTable dtEnquiryOtherDetails = (DataTable)daEBID.ExecuteReader_SP("eBitData_SP_SEL_EnquiryOtherDetails", SpParam3, DataAccess.Return_Type.DataTable);

                txtEnqComments.Text = dtEnquiryOtherDetails.Rows[0]["EnqComments"].ToString();
                if (dtEnquiryOtherDetails.Rows[0]["ExpiredDate"].ToString() != "") txtEnqExpiredDate.Text = Convert.ToDateTime(dtEnquiryOtherDetails.Rows[0]["ExpiredDate"]).ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void btnSendToCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                string enqComments = txtEnqComments.Text.ToString();
                string expiredDate = Convert.ToDateTime(txtEnqExpiredDate.Text).AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss");
                SqlParameter[] SpParams = new SqlParameter[] { 
                        new SqlParameter("@EnquiryId",Convert.ToInt32(Request["enqid"])),
                        new SqlParameter("@EnqStatus",2),
                        new SqlParameter("@ExpiredDate",expiredDate),
                        new SqlParameter("@EnqComments",enqComments)
                };
                daEBID.ExecuteNonQuery_SP("eBitData_SP_UPD_EnquiryMaster", SpParams);
                Response.Redirect("ebid_purchase.aspx?vid=6", false);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}