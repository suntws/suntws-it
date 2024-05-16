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
using System.Reflection;

namespace GT
{
    public partial class frmStencilDataModify : Form
    {
        DBAccess dba = new DBAccess();
        DataTable dt_Jathagam = null;
        public frmStencilDataModify()
        {
            InitializeComponent();
        }

        // Form control events
        private void frmStencilDataModify_Load(object sender, EventArgs e)
        {
            try
            {
                //form load event
                txtSearchStencil_Auto();
                lbl_StencilNo.Text = "Enter Stencil No !!..";
                dt_Jathagam = (DataTable)dba.ExecuteReader_SP("SP_SEL_EditBarcode_ProcessIdDetails", DBAccess.Return_Type.DataTable);
                cmb_Grade.Items.Add("CHOOSE");
                cmb_Grade.Items.Add("A");
                cmb_Grade.Items.Add("B");
                cmb_Grade.Items.Add("C");
                cmb_Grade.Items.Add("D");
                cmb_Grade.Items.Add("E");
                cmb_Grade.Items.Add("R");
                cmb_Grade.Items.Add("S");
                cmb_Grade.Items.Add("Z");
                pnl_Cntrls.Visible = false;
                pnl_Jathagam.Visible = false;

                DataTable dtRe = (DataTable)dba.ExecuteReader_SP("sp_sel_tyre_remarks", DBAccess.Return_Type.DataTable);
                if (dtRe.Rows.Count > 0)
                {
                    DataRow dr = dtRe.NewRow();
                    dr.ItemArray = new object[] { "CHOOSE" };
                    dtRe.Rows.InsertAt(dr, 0);

                    cboTyreRemarks.DataSource = dtRe;
                    cboTyreRemarks.DisplayMember = "remarks";
                    cboTyreRemarks.ValueMember = "remarks";

                    if (cboTyreRemarks.Items.Count == 2)
                        cboTyreRemarks.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            //button click to bind details of stencil no
            try
            {
                if (txt_StencilNo.Text.Length > 9 && txt_StencilNo.Text.Length < 13)
                {
                    btn_Save.Visible = true;
                    SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@StencilNo", txt_StencilNo.Text) };
                    DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_StencilSearch_tbqualitycontrol", sp, DBAccess.Return_Type.DataTable);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["dispatchStatus"].ToString().ToUpper() == "YES")
                        {
                            MessageBox.Show("Stencil already dispatched to " + dt.Rows[0]["tyrestatus"].ToString() + " on " + Convert.ToDateTime(dt.Rows[0]["Dispatchdate"].ToString()).ToShortDateString());
                            btn_Save.Visible = false;
                        }
                        else
                        {
                            dtp_MFDdate.MaxDate = DateTime.Now;
                            dtp_MFDdate.Text = Convert.ToDateTime(dt.Rows[0].Field<DateTime>("dateofmanufacture").ToString()).ToString("dd-MMM-yyyy");
                            lbl_StencilNo.Text = txt_StencilNo.Text;
                            txt_Size.Text = dt.Rows[0]["Size"].ToString();
                            txt_Rim.Text = dt.Rows[0]["Rim"].ToString();
                            Bind_Jathagam("Platform");
                            cmb_Platform.SelectedIndex = cmb_Platform.FindStringExact(dt.Rows[0]["Platform"].ToString());
                            ComboBox_SelectedIndexChanged(cmb_Platform, e);
                            cmb_Brand.SelectedIndex = cmb_Brand.FindStringExact(dt.Rows[0]["Brand"].ToString());
                            ComboBox_SelectedIndexChanged(cmb_Brand, e);
                            cmb_Sidewall.SelectedIndex = cmb_Sidewall.FindStringExact(dt.Rows[0]["SideWall"].ToString());
                            ComboBox_SelectedIndexChanged(cmb_Sidewall, e);
                            cmb_Type.SelectedIndex = cmb_Type.FindStringExact(dt.Rows[0]["Type"].ToString());
                            cmb_Grade.SelectedIndex = cmb_Grade.FindStringExact(dt.Rows[0]["qualitygrade"].ToString());
                            txt_ProcessID_Old.Text = dt.Rows[0]["ProcessId"].ToString();
                            txt_ProcessID_New.Text = dt.Rows[0]["ProcessId"].ToString();
                            pnl_Cntrls.Visible = true;
                            pnl_Jathagam.Visible = true;
                            cboTyreRemarks.SelectedIndex = cboTyreRemarks.FindStringExact(dt.Rows[0]["tyre_modify_remarks"].ToString());
                            cmb_Grade.Focus();
                        }
                    }
                    else
                        MessageBox.Show("NO RECORDS", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            //button to reset all controls
            try
            {
                pnl_Cntrls.Visible = false;
                pnl_Jathagam.Visible = false;
                cboTyreRemarks.SelectedIndex = 0;
                lbl_StencilNo.Text = "Enter StencilNo!!--";
                txt_StencilNo.Text = "";
                txt_StencilNo.Focus();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            //button to save records
            try
            {
                if (txt_Remarks.Text.Trim() == "")
                {
                    MessageBox.Show("Enter Remarks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_Remarks.Focus();
                }
                else if (cboTyreRemarks.SelectedIndex <= 0)
                {
                    MessageBox.Show("Choose Tyre Modify Remarks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboTyreRemarks.Focus();
                }
                else
                {
                    string strOldProcessID = txt_ProcessID_Old.Text != txt_ProcessID_New.Text ? txt_ProcessID_Old.Text : "NULL";
                    SqlParameter[] spUpd = new SqlParameter[] { 
                        new SqlParameter("@config", cmb_Platform.Text), 
                        new SqlParameter("@tyresize", txt_Size.Text), 
                        new SqlParameter("@tyrerim", txt_Rim.Text), 
                        new SqlParameter("@tyretype", cmb_Type.Text), 
                        new SqlParameter("@brand", cmb_Brand.Text), 
                        new SqlParameter("@sidewall", cmb_Sidewall.Text), 
                        new SqlParameter("@processId", txt_ProcessID_New.Text), 
                        new SqlParameter("@qualitygrade", cmb_Grade.Text), 
                        new SqlParameter("@barcode", txt_Barcode.Text), 
                        new SqlParameter("@PrevProcessId", strOldProcessID), 
                        new SqlParameter("@remarks", txt_Remarks.Text), 
                        new SqlParameter("@dateofmanufacture", dtp_MFDdate.Text), 
                        new SqlParameter("@username", Program.strUserName), 
                        new SqlParameter("@stencilno", txt_StencilNo.Text),
                        new SqlParameter("@tyre_modify_remarks", cboTyreRemarks.SelectedValue.ToString())
                    };
                    int resp = dba.ExecuteNonQuery_SP("sp_upd_tbqualitycontrol_stencil_data", spUpd);
                    if (resp > 0)
                    {
                        Program.PreparePrintLable(txt_Barcode.Text);
                        MessageBox.Show("Record Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txt_StencilNo_KeyDown(object sender, KeyEventArgs e)
        {
            //KeyDown event for 'txt_StencilNo' textbox
            try
            {
                if ((txt_StencilNo.Text.Length > 9 && txt_StencilNo.Text.Length < 13 && e.KeyValue == 13))
                {
                    e.Handled = true;
                    btn_Search_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SelectedIndexChanged for combobox controls(cmb_Platfrom,cmb_Brand,cmb_Sidewall,cmb_Type)
            try
            {
                ComboBox cmb = (ComboBox)sender;
                if (cmb.SelectedIndex != 0 && cmb.SelectedIndex != -1)
                {
                    switch (cmb.Name.Remove(0, 4))
                    {
                        case "Platform":
                            Bind_Jathagam("Brand");
                            break;
                        case "Brand":
                            Bind_Jathagam("Sidewall");
                            break;
                        case "Sidewall":
                            Bind_Jathagam("Type");
                            break;
                        case "Type":
                            break;
                    }
                    Bind_ProcessID_Create_Barcode();
                }
                else if (cmb.SelectedIndex == 0)
                {
                    switch (cmb.Name.Remove(0, 4))
                    {
                        case "Platform":
                            cmb_Brand.DataSource = null;
                            cmb_Sidewall.DataSource = null;
                            cmb_Type.DataSource = null;
                            break;
                        case "Brand":
                            cmb_Sidewall.DataSource = null;
                            cmb_Type.DataSource = null;
                            break;
                        case "Sidewall":
                            cmb_Type.DataSource = null;
                            break;
                    }
                    txt_ProcessID_New.Text = "";
                    cmb_Grade.SelectedIndex = 0;
                    txt_Barcode.Text = "";
                    btn_Save.Visible = false;
                    cmb.Focus();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            // KeyUpEvent for combobox controls(cmb_Platfrom,cmb_Brand,cmb_Sidewall,cmb_Type)
            try
            {
                string typngValue = "";
                ComboBox cmb = (ComboBox)sender;
                if (e != null)
                {
                    if (cmb.Text != "CHOOSE" && cmb.Text != "")
                    {
                        bool chk = false;
                        foreach (string value in cmb.Items)
                        {
                            if (value.StartsWith(cmb.Text.ToUpper()))
                            {
                                chk = true;
                                break;
                            }
                        }
                        if (!chk)
                        {
                            typngValue = cmb.Text.Remove(cmb.Text.Length - 1, 1).ToUpper();
                            cmb.Text = typngValue;
                            cmb.Focus();
                            cmb.SelectionStart = cmb.Text.Length;
                        }
                    }
                    if (e.KeyData == Keys.Enter && cmb.Text == "CHOOSE")
                    {
                        MessageBox.Show("Choose " + cmb.Name.Remove(0, 4) + "!!..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SendKeys.Send("{F4}");
                        cmb.SelectedIndex = -1;
                        cmb.Focus();
                    }
                    else if (e.KeyData == Keys.Enter && cmb.Text != "CHOOSE")
                    {
                        switch (cmb.Name.Remove(0, 4))
                        {
                            case "Platform":
                                Bind_Jathagam("Brand");
                                break;
                            case "Brand":
                                Bind_Jathagam("Sidewall");
                                break;
                            case "Sidewall":
                                Bind_Jathagam("Type");
                                break;
                            case "Type":
                                break;
                        }
                        Bind_ProcessID_Create_Barcode();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            // TextChangedEvent for combobox controls(cmb_Platfrom,cmb_Brand,cmb_Sidewall,cmb_Type)
            try
            {
                ComboBox cmb = (ComboBox)sender;
                string strtxt = cmb.Text.ToUpper();
                cmb.Text = strtxt;
                cmb.Focus();
                cmb.SelectionStart = cmb.Text.Length;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void ComboBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // PreviewKeyDown for combobox controls(cmb_Platfrom,cmb_Brand,cmb_Sidewall,cmb_Type)
            try
            {
                ComboBox cmb = (ComboBox)sender;
                if (e.KeyData == Keys.Tab)
                {
                    bool chk = false;
                    foreach (string value in cmb.Items)
                    {
                        if (value == cmb.Text.ToUpper())
                        {
                            chk = true;
                            break;
                        }
                    }
                    if (chk)
                        SendKeys.Send("{TAB}");
                    else
                        cmb.Focus();
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        //functions to access controls
        private void Bind_Jathagam(string strField)
        {
            //function to bind jathagam
            try
            {
                switch (strField)
                {
                    case "Platform":
                        List<string> lst_Config = dt_Jathagam.AsEnumerable()
                            .Where(w => w.Field<string>("TyreSize").Equals(txt_Size.Text))
                            .Where(w => w.Field<string>("TyreRim").Equals(txt_Rim.Text))
                            .OrderBy(n => n.Field<string>("Config"))
                            .Select(A => A.Field<string>("Config")).Distinct().ToList();
                        if (lst_Config.Count > 0)
                        {
                            lst_Config.Insert(0, "CHOOSE");
                            cmb_Platform.DataSource = lst_Config;
                            if (lst_Config.Count == 2)
                            {
                                cmb_Platform.SelectedIndex = 1;
                                Bind_Jathagam("Brand");
                            }
                        }
                        break;
                    case "Brand":
                        List<string> lst_Brand = dt_Jathagam.AsEnumerable()
                            .Where(w => w.Field<string>("TyreSize").Equals(txt_Size.Text))
                            .Where(w => w.Field<string>("TyreRim").Equals(txt_Rim.Text))
                            .Where(w => w.Field<string>("Config").Equals(cmb_Platform.Text))
                            .OrderBy(n => n.Field<string>("Brand"))
                            .Select(A => A.Field<string>("Brand")).Distinct().ToList();
                        if (lst_Brand.Count > 0)
                        {
                            lst_Brand.Insert(0, "CHOOSE");
                            cmb_Brand.DataSource = lst_Brand;
                            if (lst_Brand.Count == 2)
                            {
                                cmb_Brand.SelectedIndex = 1;
                                Bind_Jathagam("Sidewall");
                            }
                        }
                        break;
                    case "Sidewall":
                        List<string> lst_Sidewall = dt_Jathagam.AsEnumerable()
                            .Where(w => w.Field<string>("TyreSize").Equals(txt_Size.Text))
                            .Where(w => w.Field<string>("TyreRim").Equals(txt_Rim.Text))
                            .Where(w => w.Field<string>("Config").Equals(cmb_Platform.Text))
                            .Where(w => w.Field<string>("Brand").Equals(cmb_Brand.Text))
                            .OrderBy(n => n.Field<string>("Sidewall"))
                            .Select(A => A.Field<string>("Sidewall")).Distinct().ToList();
                        if (lst_Sidewall.Count > 0)
                        {
                            lst_Sidewall.Insert(0, "CHOOSE");
                            cmb_Sidewall.DataSource = lst_Sidewall;
                            if (lst_Sidewall.Count == 2)
                            {
                                cmb_Sidewall.SelectedIndex = 1;
                                Bind_Jathagam("Type");
                            }
                        }
                        break;
                    case "Type":
                        List<string> lst_Type = dt_Jathagam.AsEnumerable()
                            .Where(w => w.Field<string>("TyreSize").Equals(txt_Size.Text))
                            .Where(w => w.Field<string>("TyreRim").Equals(txt_Rim.Text))
                            .Where(w => w.Field<string>("Config").Equals(cmb_Platform.Text))
                            .Where(w => w.Field<string>("Brand").Equals(cmb_Brand.Text))
                            .Where(w => w.Field<string>("Sidewall").Equals(cmb_Sidewall.Text))
                            .OrderBy(n => n.Field<string>("TyreType"))
                            .Select(A => A.Field<string>("TyreType")).Distinct().ToList();
                        if (lst_Type.Count > 0)
                        {
                            lst_Type.Insert(0, "CHOOSE");
                            cmb_Type.DataSource = lst_Type;
                            if (lst_Type.Count == 2)
                            {
                                cmb_Type.SelectedIndex = 1;
                                cmb_Grade.Focus();
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void txtSearchStencil_Auto()
        {
            //Auto complete string collection for txt_StencilNo textobox
            try
            {
                var stencilCollection = new AutoCompleteStringCollection();
                DataTable dt = (DataTable)dba.ExecuteReader_SP("sp_sel_Dispatch_non_list", DBAccess.Return_Type.DataTable);
                if (dt.Rows.Count > 0)
                {
                    string[] postSource = dt.AsEnumerable().Select<System.Data.DataRow, String>(x => x.Field<String>("stencilno")).ToArray();
                    stencilCollection.AddRange(postSource);
                }
                txt_StencilNo.AutoCompleteCustomSource = stencilCollection;
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void Bind_ProcessID_Create_Barcode()
        {
            //function to bind processid and create barcode
            try
            {
                txt_ProcessID_New.Text = ""; txt_Barcode.Text = ""; btn_Save.Visible = false;
                if (txt_Size.Text != "" && txt_Rim.Text != "" && cmb_Platform.SelectedItem.ToString() != "CHOOSE" && cmb_Brand.SelectedItem.ToString() != "CHOOSE" &&
                    cmb_Sidewall.SelectedItem.ToString() != "CHOOSE" && cmb_Type.SelectedItem.ToString() != "CHOOSE" && cmb_Grade.SelectedItem.ToString() != "CHOOSE")
                {
                    if (dt_Jathagam != null && dt_Jathagam.Rows.Count > 0)
                    {
                        List<string> lst = dt_Jathagam.AsEnumerable().Where(
                                        b => b.Field<string>("Config").Equals(cmb_Platform.Text) &&
                                        b.Field<string>("Brand").Equals(cmb_Brand.Text) &&
                                        b.Field<string>("Sidewall").Equals(cmb_Sidewall.Text) &&
                                        b.Field<string>("TyreType").Equals(cmb_Type.Text) &&
                                        b.Field<string>("TyreSize").Equals(txt_Size.Text) &&
                                        b.Field<string>("TyreRim").Equals(txt_Rim.Text)
                                        ).Select(A => A.Field<string>("ProcessID")).Distinct().ToList();
                        txt_ProcessID_New.Text = lst[0].ToString();

                        if (txt_StencilNo.Text.Length == 10 && txt_ProcessID_New.Text != "")
                            txt_Barcode.Text = (txt_ProcessID_New.Text + txt_StencilNo.Text + cmb_Grade.Text).ToUpper();
                        if (txt_Barcode.Text != "")
                            btn_Save.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmStencilDataModify", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        private void cmb_Grade_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //grade change event
            if (cmb_Grade.SelectedIndex > 0)
            {
                Bind_ProcessID_Create_Barcode();
                btn_Save.Visible = true;
            }
            else
            {
                txt_Barcode.Text = "";
                btn_Save.Visible = false;
                cmb_Grade.Focus();
            }
        }
    }
}
