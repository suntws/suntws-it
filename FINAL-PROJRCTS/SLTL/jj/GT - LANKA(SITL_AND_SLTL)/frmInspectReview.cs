using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;

namespace GT
{
    public partial class frmInspectReview : Form
    {
        DBAccess dba = new DBAccess();
        string strProcessID = "";
        public frmInspectReview()
        {
            InitializeComponent();
        }

        private void frmInspectReview_Load(object sender, EventArgs e)
        {
            try
            {
                ClearRecords();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspectReview", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void lstStencilNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Bind Stencil Values and Defects
                foreach (TextBox txt in pnl_Jathagam.Controls.OfType<TextBox>())
                    txt.Text = "";
                if (lstStencilNo.SelectedIndex >= 0)
                {
                    txt_StencilNo.Text = lstStencilNo.Text;
                    SqlParameter[] spSel = new SqlParameter[] { new SqlParameter("@StencilNo", lstStencilNo.Text) };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_Stage3_Data", spSel, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        txt_Platform.Text = dt.Rows[0]["Config"].ToString();
                        txt_Brand.Text = dt.Rows[0]["Brand"].ToString();
                        txt_Sidewall.Text = dt.Rows[0]["Sidewall"].ToString();
                        txt_Type.Text = dt.Rows[0]["TyreType"].ToString();
                        txt_Size.Text = dt.Rows[0]["TyreSize"].ToString();
                        txt_Rim.Text = dt.Rows[0]["RimSize"].ToString();
                        strProcessID = dt.Rows[0]["Process_Id"].ToString();
                        Bind_gvDefects();
                        pnl_Jathagam.Visible = true;
                        dgv_DefectList.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspectReview", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_gvDefects()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Defect Code");
                dt.Columns.Add("Defect Name");
                dt.Columns.Add("Observations");
                dt.Columns.Add("Grade");
                string strD4MouldOpen = "";
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StencilNo", lstStencilNo.Text) };
                DataSet ds = (DataSet)dba.ExecuteReader_SP("sp_sel_DefectList", sp, DBAccess.Return_Type.DataSet);
                if (ds.Tables.Count == 4)
                {
                    DataTable dt_Defect = ds.Tables[0];
                    DataTable dt_DefectName = ds.Tables[1];
                    DataTable dt_DefectObsrtns = ds.Tables[2];
                    DataTable dt_DefectQuestions = ds.Tables[3];
                    strD4MouldOpen = dt_Defect.Rows[0]["D4MouldOpen"].ToString();
                    dt_Defect.Columns.Remove("D4MouldOpen");
                    if (dt_Defect.Rows.Count > 0)
                    {
                        foreach (DataColumn col in dt_Defect.Columns)
                        {
                            if (dt_Defect.Rows[0][col].ToString() != "A+" && dt_Defect.Rows[0][col].ToString() != "A")
                            {
                                string[] DefectName = dt_DefectName.AsEnumerable().Where(b => b.Field<string>("DefectCode").Equals(col.ColumnName)).Select(A => A.Field<string>("DefectName")).ToArray();
                                string[] DefectObservtns = new string[1];
                                if (col.ColumnName == "D4")
                                    DefectObservtns[0] = "Mould Open = " + strD4MouldOpen;
                                else if (col.ColumnName != "D4")
                                    DefectObservtns = dt_DefectObsrtns.AsEnumerable().Where(b => b.Field<string>("DefectCode").Equals(col.ColumnName)).Where(b => b.Field<string>("Grade").Equals(dt_Defect.Rows[0][col].ToString())).Select(A => A.Field<string>("Observation")).ToArray();
                                if (DefectObservtns.Length == 0)
                                    DefectObservtns = dt_DefectQuestions.AsEnumerable().Where(b => b.Field<string>("DefectCode").Equals(col.ColumnName)).Select(A => A.Field<string>("Question")).ToArray();
                                dt.Rows.Add(col.ColumnName, DefectName.Length == 0 ? "" : DefectName[0], DefectObservtns.Length == 0 ? "" : DefectObservtns[0],
                                    dt_Defect.Rows[0][col].ToString());
                            }
                        }
                        dgv_DefectList.Columns.Clear();
                        dgv_DefectList.DataSource = null;
                        if (dt.Rows.Count > 0)
                        {
                            dgv_DefectList.DataSource = dt;
                            DataGridViewButtonColumn ActionButton = new DataGridViewButtonColumn();
                            ActionButton.HeaderText = "Action";
                            ActionButton.Text = "APPROVAL";
                            ActionButton.Name = "btnReAssign";
                            ActionButton.UseColumnTextForButtonValue = true;
                            dgv_DefectList.Columns.Insert(4, ActionButton);
                            string ColumnWidth = "100,150,150,100,100";
                            string[] width = ColumnWidth.Split(',');
                            for (int i = 0; i < 5; i++)
                            {
                                dgv_DefectList.Columns[i].Width = Convert.ToInt32(width[i]);
                            }
                            dgv_DefectList.ClearSelection();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspectReview", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void dgv_DefectList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Gridview Button Click
            try
            {
                var senderId = (DataGridView)sender;
                if (senderId.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    txt_DefectCode.Text = dgv_DefectList.Rows[e.RowIndex].Cells["Defect Code"].Value.ToString();
                    txt_DefectName.Text = dgv_DefectList.Rows[e.RowIndex].Cells["Defect Name"].Value.ToString();
                    txt_DefectObeservation.Text = dgv_DefectList.Rows[e.RowIndex].Cells["Observations"].Value.ToString();
                    txt_Grade.Text = dgv_DefectList.Rows[e.RowIndex].Cells["Grade"].Value.ToString();
                    pnl_Cntrls.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspectReview", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //SqlParameter[] spImg = new SqlParameter[] { new SqlParameter("@stencilno", txt_StencilNo.Text) };
                //DataTable dtImg = (DataTable)dba.ExecuteReader_SP("sp_sel_defect_images", spImg, DBAccess.Return_Type.DataTable);
                //if (dtImg.Rows.Count < 3 && txt_StencilNo.Text != "")
                //{
                //    MessageBox.Show("Upload defect images");
                //    Form childForm = new frmDefectImage();
                //    childForm.Text = txt_StencilNo.Text;
                //    childForm.Show();
                //    childForm.WindowState = FormWindowState.Maximized;
                //}
                //else
                //{
                bool GradeChk = false;
                string strGrade = "";
                foreach (RadioButton rdo in gp_rdp_Grade.Controls.OfType<RadioButton>())
                {
                    if (rdo.Checked)
                    {
                        strGrade = rdo.Text;
                        GradeChk = true;
                        break;
                    }
                }

                if (!GradeChk)
                    MessageBox.Show("Choose Grade to Continue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (GradeChk && txt_Remarks.Text.Trim() == "")
                    MessageBox.Show("Enter Remarks to Continue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (GradeChk && txt_Remarks.Text.Trim() != "")
                {
                    string strBarcode = (strProcessID + txt_StencilNo.Text + strGrade).ToUpper();
                    Program.PreparePrintLable(strBarcode);
                    SqlParameter[] sp = new SqlParameter[] { 
                            new SqlParameter("@StencilNo", txt_StencilNo.Text), 
                            new SqlParameter("@Barcode", strBarcode), 
                            new SqlParameter("@Grade", strGrade), 
                            new SqlParameter("@username", Program.strUserName) 
                        };
                    int resp = dba.ExecuteNonQuery_SP("sp_ins_tbQualityCntrl_V2", sp);
                    if (resp > 0)
                    {
                        sp = new SqlParameter[] { 
                                new SqlParameter("@StencilNo", txt_StencilNo.Text), 
                                new SqlParameter("@Approved_User", Program.strUserName), 
                                new SqlParameter("@Approved_Remarks", txt_DefectCode.Text + " " + txt_Grade.Text + "-\n" + txt_Remarks.Text) 
                            };
                        resp = 0;
                        resp = dba.ExecuteNonQuery_SP("sp_upt_TyreFinalinspect", sp);
                        if (resp > 0)
                            MessageBox.Show("Record Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    ClearRecords();
                }
                //}
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspectReview", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void ClearRecords()
        {
            try
            {
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_Inspect_WaitingList", DBAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    lstStencilNo.DataSource = dt;
                    lstStencilNo.DisplayMember = "StencilNo";
                    lstStencilNo.ValueMember = "StencilNo";
                }

                txt_StencilNo.Text = "";
                txt_Remarks.Text = "";
                foreach (TextBox txt in pnl_Jathagam.Controls.OfType<TextBox>())
                    txt.Text = "";
                foreach (TextBox txt in pnl_Defects.Controls.OfType<TextBox>())
                    txt.Text = "";
                foreach (RadioButton rdo in gp_rdp_Grade.Controls.OfType<RadioButton>())
                    rdo.Checked = false;
                pnl_Jathagam.Visible = false;
                dgv_DefectList.Visible = false;
                pnl_Cntrls.Visible = false;
                //lstStencilNo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmInspectReview", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearRecords();
        }
    }
}
