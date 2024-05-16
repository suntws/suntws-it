using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GT
{
    public partial class frmRePrint : Form
    {
        DBAccess dba = new DBAccess();
        public frmRePrint()
        {
            InitializeComponent();
        }
        private void frmRePrint_Load(object sender, EventArgs e)
        {
            CtrlClear();
        }
        private void CtrlClear()
        {
            lblPlatform.Text = "";
            lblTyreSize.Text = "";
            lblRim.Text = "";
            lblType.Text = "";
            lblBrand.Text = "";
            lblSidewall.Text = "";
            lblProcessID.Text = "";
            lblStencil.Text = "";
            lblGrade.Text = "";
            lblDom.Text = "";
            lblDispatchStatus.Text = "";
            lblDispatchDate.Text = "";

            lblInspOn.Text = "";
            lblInspBy.Text = "";
            lblInspRemarks.Text = "";
            lblAppOn.Text = "";
            lblAppBy.Text = "";
            lblAppRemarks.Text = "";
            lblBarcodeOn.Text = "";
            lblReBarcodeOn.Text = "";
            lblPrevProcessID.Text = "";
            lblBarcodeBy.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && txtStencilNo.Text.Length == 10)
                button1_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                CtrlClear();
                if (txtStencilNo.Text.Length == 10)
                {
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@stencilno", txtStencilNo.Text) };
                    DataSet dsData = (DataSet)dba.ExecuteReader_SP("sp_sel_stencil_data_v1", sp, DBAccess.Return_Type.DataSet);
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = dsData.Tables[0];
                        lblPlatform.Text = dt.Rows[0]["config"].ToString();
                        lblTyreSize.Text = dt.Rows[0]["tyresize"].ToString();
                        lblRim.Text = dt.Rows[0]["tyrerim"].ToString();
                        lblType.Text = dt.Rows[0]["tyretype"].ToString();
                        lblBrand.Text = dt.Rows[0]["brand"].ToString();
                        lblSidewall.Text = dt.Rows[0]["sidewall"].ToString();
                        lblProcessID.Text = dt.Rows[0]["processid"].ToString();
                        lblStencil.Text = dt.Rows[0]["stencilno"].ToString();
                        lblGrade.Text = dt.Rows[0]["grade"].ToString();
                        lblDom.Text = dt.Rows[0]["dom"].ToString();
                        lblDispatchStatus.Text = dt.Rows[0]["DispatchStatus"].ToString();
                        lblDispatchDate.Text = dt.Rows[0]["Dispatchdate"].ToString();

                        lblBarcodeOn.Text = dt.Rows[0]["CreateDate"].ToString();
                        lblReBarcodeOn.Text = dt.Rows[0]["Modifydatte"].ToString();
                        lblPrevProcessID.Text = dt.Rows[0]["PrevProcessid"].ToString(); ;
                        lblBarcodeBy.Text = dt.Rows[0]["username"].ToString();

                        if (dsData.Tables[1].Rows.Count > 0)
                        {
                            DataTable dtInsp = dsData.Tables[1];
                            lblInspOn.Text = dtInsp.Rows[0]["Inspected_Date"].ToString();
                            lblInspBy.Text = dtInsp.Rows[0]["Inspected_User"].ToString();
                            lblInspRemarks.Text = dtInsp.Rows[0]["Inspected_Remarks"].ToString();
                            lblAppOn.Text = dtInsp.Rows[0]["Approved_Date"].ToString();
                            lblAppBy.Text = dtInsp.Rows[0]["Approved_User"].ToString(); ;
                            lblAppRemarks.Text = dtInsp.Rows[0]["Approved_Remarks"].ToString();
                        }
                        txtStencilNo.Text = "";
                    }
                    else
                        MessageBox.Show("NO RECORDS");
                }
                else
                    MessageBox.Show("INVALID STENCIL NO");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLable_Click(object sender, EventArgs e)
        {
            if (lblProcessID.Text != "" && lblStencil.Text != "" && lblGrade.Text != "")
                Program.PreparePrintLable((lblProcessID.Text + lblStencil.Text + lblGrade.Text).ToUpper());
            else
                MessageBox.Show("NO RECORDS");
        }
    }
}
