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
using System.IO;
namespace TTS
{
    public partial class cotsRimMoveFromGodown : System.Web.UI.Page
    {
        DataAccess dba = new DataAccess(ConfigurationManager.ConnectionStrings["orderdb"].ConnectionString);
        DataTable dtProcessIdDetails = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["TTSUser"] != null && Request.Cookies["TTSUser"].Value != "" && Session["dtuserlevel"] != null)
                {
                    if (!IsPostBack)
                    {
                        lblPageTitle.Text = (Utilities.Decrypt(Request["fid"].ToString()) == "d" ? "DOMESTIC" : "EXPORT") + " MOVE FROM GODOWN";
                        dg_OrderEdc.DataSource = null;
                        SqlParameter[] sp = new SqlParameter[] { 
                            new SqlParameter("@Qstring", Utilities.Decrypt(Request["fid"].ToString())), 
                            new SqlParameter("@RimPlant", Utilities.Decrypt(Request["pid"].ToString()))
                        };
                        DataTable dtCust = (DataTable)dba.ExecuteReader_SP("sp_sel_RimOrderCustomerList", sp, DataAccess.Return_Type.DataTable);
                        if (dtCust.Rows.Count > 0)
                        {
                            DataRow dr = dtCust.NewRow();
                            dr.ItemArray = new object[] { "CHOOSE" };
                            dtCust.Rows.InsertAt(dr, 0);
                            cboCustomer.DataSource = dtCust;
                            cboCustomer.DataTextField = "custfullname";
                            cboCustomer.DataValueField = "custfullname";
                            if (dtCust.Rows.Count == 2)
                            {
                                cboCustomer.SelectedIndex = 1;
                                cboCustomer.Focus();
                            }
                        }
                        else
                            lblErrMsgcontent.Text = "NO RECORDS";
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
        protected void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsgcontent.Text = "";
                dg_OrderEdc.DataSource = null;
                if (cboCustomer.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@custfullname", cboCustomer.SelectedValue), 
                        new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString())) 
                    };
                    DataTable dtWork = (DataTable)dba.ExecuteReader_SP("sp_sel_RimWorkOrderList", sp, DataAccess.Return_Type.DataTable);
                    if (dtWork.Rows.Count > 0)
                    {
                        DataRow dr = dtWork.NewRow();
                        dr.ItemArray = new object[] { "CHOOSE" };
                        dtWork.Rows.InsertAt(dr, 0);
                        cboWorkOrder.DataSource = dtWork;
                        cboWorkOrder.DataTextField = "workorderno";
                        cboWorkOrder.DataValueField = "workorderno";
                        if (dtWork.Rows.Count == 2)
                        {
                            cboWorkOrder.Focus();
                            cboWorkOrder.SelectedIndex = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void cboWorkOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsgcontent.Text = "";
                dg_OrderEdc.DataSource = null;
                if (cboWorkOrder.SelectedIndex > 0)
                {
                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@custfullname", cboCustomer.SelectedValue), 
                        new SqlParameter("@plant", Utilities.Decrypt(Request["pid"].ToString())), 
                        new SqlParameter("@workorderno", cboWorkOrder.SelectedValue) 
                    };
                    DataTable dtVal = (DataTable)dba.ExecuteReader_SP("sp_sel_RimOrderQty", sp, DataAccess.Return_Type.DataTable);
                    if (dtVal.Rows.Count > 0)
                    {
                        lblOrderQty.Text = dtVal.Compute("Sum(orderqty)", "").ToString().PadLeft(4, '0');
                        lblCustcode.Text = dtVal.Rows[0]["custcode"].ToString();

                        dg_OrderEdc.DataSource = dtVal;
                        dg_OrderEdc.Columns[0].Visible = false;
                        dg_OrderEdc.Columns[1].HeaderText = "EDC NO";
                        dg_OrderEdc.Columns[2].HeaderText = "QTY";
                        dg_OrderEdc.Columns[3].HeaderText = "DESCRIPTION";
                        

                        SqlParameter[] spIns = new SqlParameter[] { 
                            new SqlParameter("@CustCode", lblCustcode.Text), 
                            new SqlParameter("@Workorderno", cboWorkOrder.SelectedValue), 
                            new SqlParameter("@DispatchQty", Convert.ToInt32(lblOrderQty.Text)), 
                            new SqlParameter("@RimPlant", Utilities.Decrypt(Request["pid"].ToString())) 
                        };
                        DataTable dtDID = (DataTable)dba.ExecuteReader_SP("sp_ins_Rim_DispatchMaster", spIns, DataAccess.Return_Type.DataTable);
                        if (dtDID != null && dtDID.Rows.Count > 0)
                        {
                            lblDispatchID.Text = dtDID.Rows[0]["DispatchID"].ToString();
                            Build_LeftFromGodownQty();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Build_LeftFromGodownQty()
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@MoveForOrderID", lblDispatchID.Text) };
                lblLeftGodownQty.Text = (dba.ExecuteScalar_SP("sp_sel_MovedFromGodown_Qty", sp)).ToString().PadLeft(4, '0');

                if (Convert.ToInt32(lblLeftGodownQty.Text) == Convert.ToInt32(lblOrderQty.Text))
                {
                    lblErrMsgcontent.Text="QTY COMPLETED";
                    txt_Barcode.Visible = false;
                    txt_Barcode.Text = "";
                }
                else
                {
                    txt_Barcode.Visible = true;
                    txt_Barcode.Text = "";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        protected void txt_Barcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblErrMsgcontent.Text = "";
                if (txt_Barcode.Text.Length == 19)
                {
                    SqlParameter[] sp1 = new SqlParameter[] { new SqlParameter("@Barcode", Convert.ToString(txt_Barcode.Text)) };
                    Int32 dtNoOfPiece = Convert.ToInt32((string)dba.ExecuteScalar_SP("SP_CHK_RimBarcode", sp1));
                    if (dtNoOfPiece > 0)
                    {
                        for (int i = 1; i < dtNoOfPiece; i++)
                        {
                            string strBarcode = txt_Barcode.Text.Substring(0, 18) + Convert.ToChar(65 + i);
                            SqlParameter[] spIns = new SqlParameter[] { 
                                new SqlParameter("@RimBarcode", txt_Barcode.Text), 
                                new SqlParameter("@MoveForOrderID", lblDispatchID.Text), 
                                new SqlParameter("@NewRimBarcode", strBarcode) 
                            };
                            int resp = dba.ExecuteNonQuery_SP("sp_ins_Rim_StockList_Data_Female", spIns);
                            if (resp > 0)
                                PreparePrintLable(strBarcode);
                        }
                        SqlParameter[] spUpd = new SqlParameter[] { 
                            new SqlParameter("@RimBarcode", txt_Barcode.Text), 
                            new SqlParameter("@MoveForOrderID", lblDispatchID.Text) 
                        };
                        int updResp = dba.ExecuteNonQuery_SP("sp_upd_rim_stocklistdata_movedfromgodown", spUpd);
                        if (updResp > 0)
                            Build_LeftFromGodownQty();
                        else
                            lblErrMsgcontent.Text ="ALREADY SCANNED FOR DISPATCH";
                    }
                    else if (dtNoOfPiece == 0)
                        lblErrMsgcontent.Text ="INVALID BARCODE";
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void PreparePrintLable(string strlblVal)
        {
            string strLogFile = @"C:\\Barcode Data\\";
            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    strLogFile = @"C:\\Barcode Data\\";
                    if (!Directory.Exists(strLogFile))
                        Directory.CreateDirectory(strLogFile);
                    strLogFile = strLogFile + strlblVal + ".txt";
                    StreamWriter SWrtiter = System.IO.File.AppendText(strLogFile);
                    SWrtiter.WriteLine(strlblVal);
                    SWrtiter.Close();
                }
                else
                {
                    strLogFile = @"C:\\Barone\\jobs\\";
                    if (!Directory.Exists(strLogFile))
                        Directory.CreateDirectory(strLogFile);
                    strLogFile = strLogFile + strlblVal + ".pj";
                    StreamWriter SWrtiter = System.IO.File.AppendText(strLogFile);
                    SWrtiter.WriteLine(@"C:\Barone\formats\tyres.lbl" + System.Environment.NewLine + "1" + System.Environment.NewLine + strlblVal.Trim());
                    SWrtiter.Close();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("TTS", Request.Url.ToString(), MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}