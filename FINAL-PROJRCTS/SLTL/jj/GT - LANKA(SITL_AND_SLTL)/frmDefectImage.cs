using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Configuration;

namespace GT
{
    public partial class frmDefectImage : Form
    {
        DBAccess dba = new DBAccess();
        public frmDefectImage()
        {
            InitializeComponent();
        }
        private void frmDefectImage_Load(object sender, EventArgs e)
        {
            try
            {
                lblStencil.Text = this.Text;

            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDefectImage", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Upload_DefectImages();
                if (frmInspect.strImgMethod != "")
                {
                    SqlParameter[] spImg = new SqlParameter[] { new SqlParameter("@stencilno", this.Text) };
                    DataTable dtImg = (DataTable)dba.ExecuteReader_SP("sp_sel_defect_images", spImg, DBAccess.Return_Type.DataTable);
                    if (dtImg.Rows.Count != 2)
                        Upload_DefectImages();
                }
                MessageBox.Show("Upload Successfully");
                frmInspect.strImgMethod = "";
                this.Close();
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDefectImage", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void Upload_DefectImages()
        {
            try
            {
                openFileDialog1.Filter = "jpeg|*.jpg";
                DialogResult res = openFileDialog1.ShowDialog();
                if (res == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                    //using MemoryStream:  
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                    byte[] photo_aray = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(photo_aray, 0, photo_aray.Length);

                    SqlParameter[] sp = new SqlParameter[] { 
                        new SqlParameter("@stencilno", this.Text), 
                        new SqlParameter("@username", Program.strUserName), 
                        new SqlParameter("@img_category", frmInspect.strImgMethod != "" ? frmInspect.strImgMethod : "AFTER"), 
                        new SqlParameter("@photo", photo_aray) 
                    };
                    int resp = dba.ExecuteNonQuery_SP("sp_ins_defect_images", sp);
                    if (resp < 0)
                        MessageBox.Show("Image Upload Failed");
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDefectImage", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { new SqlParameter("@stencilno", txtImgStencil.Text) };
                DataTable dtImg = (DataTable)dba.ExecuteReader_SP("sp_sel_defect_images", sp, DBAccess.Return_Type.DataTable);
                if (dtImg.Rows.Count > 0)
                {
                    pictureBox1.Image = null;
                    lblStencil.Text = dtImg.Rows[0]["stencilno"].ToString();
                    lblCategory.Text = dtImg.Rows[0]["img_category"].ToString();
                    lblUser.Text = dtImg.Rows[0]["username"].ToString();
                    lblDate.Text = dtImg.Rows[0]["createddate"].ToString();
                    if (dtImg.Rows[0]["photo"] != System.DBNull.Value)
                    {
                        byte[] photo_aray = (byte[])dtImg.Rows[0]["photo"];
                        MemoryStream ms = new MemoryStream(photo_aray);
                        pictureBox1.Image = Image.FromStream(ms);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteToGtErrorLog("GT", "frmDefectImage", System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
    }
}
