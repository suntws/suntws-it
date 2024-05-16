using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace GT
{
    public partial class frmDashboard : Form
    {
        DBAccess dba = new DBAccess();
        public frmDashboard()
        {
            InitializeComponent();
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            lblHead.Text = Program.strPlantName + " PRODUCTION";
            timer1.Start();
            tmrData.Start();
            tmrData_Tick(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.Hour.ToString("00") + ":" + DateTime.Now.Minute.ToString("00") + ":" + DateTime.Now.Second.ToString("00");
        }

        private void tmrData_Tick(object sender, EventArgs e)
        {
            try
            {
                lblCount1.Text = "000";
                lblCount2.Text = "000";
                lblCount3.Text = "000";
                lblTotalCount.Text = "000";

                lblWt1.Text = "00.000";
                lblWt2.Text = "00.000";
                lblWt3.Text = "00.000";
                lblTotalWt.Text = "00.000";

                DateTime endTime = Convert.ToDateTime("06:06:00");
                if (System.DateTime.Now.TimeOfDay < endTime.TimeOfDay)
                {
                    DateTime result = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                    dtpPrepareDate.Value = result;
                }
                else
                    dtpPrepareDate.Value = DateTime.Now;

                lblDate.Text = (dtpPrepareDate.Value.DayOfWeek + "\n" + dtpPrepareDate.Value.ToString("dd-MMM-yyyy")).ToUpper();

                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@RecDate", dtpPrepareDate.Value.ToString("yyyy/MM/dd")) };
                DataSet ds = (DataSet)dba.ExecuteReader_SP("sp_sel_loadingCount_Dashboard", sp, DBAccess.Return_Type.DataSet);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["SHIFT"].ToString() == "SHIFT 1")
                                lblCount1.Text = (Convert.ToInt32(lblCount1.Text) + Convert.ToInt32(row["LOADCOUNT"].ToString())).ToString("000");
                            else if (row["SHIFT"].ToString() == "SHIFT 2")
                                lblCount2.Text = (Convert.ToInt32(lblCount2.Text) + Convert.ToInt32(row["LOADCOUNT"].ToString())).ToString("000");
                            else if (row["SHIFT"].ToString() == "SHIFT 3")
                                lblCount3.Text = (Convert.ToInt32(lblCount3.Text) + Convert.ToInt32(row["LOADCOUNT"].ToString())).ToString("000");
                        }
                        lblTotalCount.Text = (Convert.ToInt32(lblCount1.Text) + Convert.ToInt32(lblCount2.Text) + Convert.ToInt32(lblCount3.Text)).ToString("000");
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            if (row["SHIFT"].ToString() == "SHIFT 1" && row["WT"].ToString() != "")
                                lblWt1.Text = (Convert.ToDecimal(lblWt1.Text) + Convert.ToDecimal(row["WT"].ToString())).ToString("00.000");
                            else if (row["SHIFT"].ToString() == "SHIFT 2" && row["WT"].ToString() != "")
                                lblWt2.Text = (Convert.ToDecimal(lblWt2.Text) + Convert.ToDecimal(row["WT"].ToString())).ToString("00.000");
                            else if (row["SHIFT"].ToString() == "SHIFT 3" && row["WT"].ToString() != "")
                                lblWt3.Text = (Convert.ToDecimal(lblWt3.Text) + Convert.ToDecimal(row["WT"].ToString())).ToString("00.000");
                        }
                        lblTotalWt.Text = (Convert.ToDecimal(lblWt1.Text) + Convert.ToDecimal(lblWt2.Text) + Convert.ToDecimal(lblWt3.Text)).ToString("00.000");
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDashboard", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
