using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace GT
{
    public partial class testing : Form
    {
        public testing()
        {
            InitializeComponent();
            button1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
        }

        private void testing_Load(object sender, EventArgs e)
        {

            string barcode = "P-192124P180803533A";
            Bitmap bitmCode128 = new Bitmap(600, 200);
            using (Graphics graphic = Graphics.FromImage(bitmCode128))
            {
                Font barcodefont = new Font("code128", 30);
                PointF point = new PointF(40f, 15f);
                SolidBrush black = new SolidBrush(Color.Black);
                SolidBrush white = new SolidBrush(Color.White);
                graphic.FillRectangle(white, 0, 0, bitmCode128.Width, bitmCode128.Height);
                graphic.DrawString("*" + barcode + "*", barcodefont, black, point);

                Font textfont = new Font("Arial", 12);
                PointF pointText = new PointF(55f, 55f);
                graphic.DrawString(barcode, textfont, black, pointText);
            }

            using (MemoryStream Mmst = new MemoryStream())
            {
                bitmCode128.Save("ms", ImageFormat.Jpeg);
                pictureBox2.Image = bitmCode128;
                pictureBox2.Width = bitmCode128.Width;
                pictureBox2.Height = bitmCode128.Height;
            }

            Bitmap bitm_Code_128 = new Bitmap(600, 200);
            using (Graphics graphic = Graphics.FromImage(bitm_Code_128))
            {
                Font barcodefont = new Font("code 128", 40);
                PointF point = new PointF(40f, 15f);
                SolidBrush black = new SolidBrush(Color.Black);
                SolidBrush white = new SolidBrush(Color.White);
                graphic.FillRectangle(white, 0, 0, bitm_Code_128.Width, bitm_Code_128.Height);
                graphic.DrawString("*" + barcode + "*", barcodefont, black, point);

                Font textfont = new Font("Arial", 12);
                PointF pointText = new PointF(65f, 65f);
                graphic.DrawString(barcode, textfont, black, pointText);
            }

            using (MemoryStream Mmst = new MemoryStream())
            {
                bitm_Code_128.Save("ms", ImageFormat.Jpeg);
                pictureBox3.Image = bitm_Code_128;
                pictureBox3.Width = bitm_Code_128.Width;
                pictureBox3.Height = bitm_Code_128.Height;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument1 = new PrintDocument();
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.Print();
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string barcode = "P-192124P180803533A";
            Bitmap bitm = new Bitmap(600, 200);
            using (Graphics graphic = Graphics.FromImage(bitm))
            {
                Font barcodefont = new Font("Code 128", 30);
                PointF point = new PointF(40f, 15f);
                SolidBrush black = new SolidBrush(Color.Black);
                SolidBrush white = new SolidBrush(Color.White);
                graphic.FillRectangle(white, 0, 0, bitm.Width, bitm.Height);
                graphic.DrawString(barcode, barcodefont, black, point);

                Font textfont = new Font("Time New Roman", 32);
                PointF pointText = new PointF(4f, 4f);
                graphic.DrawString("*" + barcode + "*", textfont, black, pointText);
            }
            e.Graphics.DrawImage(bitm, 0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Font f = new Font("Code 128", 80);
            this.Font = f;

            Label l = new Label();
            l.Text = "*STACKOVERFLOW*";
            l.Size = new System.Drawing.Size(800, 600);
            this.Controls.Add(l);

            this.Size = new Size(800, 600);

        }
    }
}
