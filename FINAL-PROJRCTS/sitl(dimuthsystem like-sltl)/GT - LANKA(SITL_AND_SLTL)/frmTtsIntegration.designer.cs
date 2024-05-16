namespace GT
{
    partial class frmTtsIntegration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProcessIDMerge = new System.Windows.Forms.Button();
            this.btnStockMerge = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 98);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1090, 90);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(528, 206);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 59);
            this.label1.TabIndex = 1;
            this.label1.Text = "/";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnProcessIDMerge
            // 
            this.btnProcessIDMerge.BackColor = System.Drawing.Color.Teal;
            this.btnProcessIDMerge.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessIDMerge.ForeColor = System.Drawing.Color.White;
            this.btnProcessIDMerge.Location = new System.Drawing.Point(141, 12);
            this.btnProcessIDMerge.Name = "btnProcessIDMerge";
            this.btnProcessIDMerge.Size = new System.Drawing.Size(291, 50);
            this.btnProcessIDMerge.TabIndex = 2;
            this.btnProcessIDMerge.Text = "PROCESS-ID MERGE";
            this.btnProcessIDMerge.UseVisualStyleBackColor = false;
            this.btnProcessIDMerge.Click += new System.EventHandler(this.btnProcessIDMerge_Click);
            // 
            // btnStockMerge
            // 
            this.btnStockMerge.BackColor = System.Drawing.Color.Purple;
            this.btnStockMerge.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStockMerge.ForeColor = System.Drawing.Color.White;
            this.btnStockMerge.Location = new System.Drawing.Point(561, 12);
            this.btnStockMerge.Name = "btnStockMerge";
            this.btnStockMerge.Size = new System.Drawing.Size(217, 50);
            this.btnStockMerge.TabIndex = 3;
            this.btnStockMerge.Text = "STOCK MERGE";
            this.btnStockMerge.UseVisualStyleBackColor = false;
            this.btnStockMerge.Click += new System.EventHandler(this.btnStockMerge_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClose.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(907, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 50);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmTtsIntegration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 306);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStockMerge);
            this.Controls.Add(this.btnProcessIDMerge);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTtsIntegration";
            this.Load += new System.EventHandler(this.frmAutoUpdate_Load);
            this.Leave += new System.EventHandler(this.frmAutoUpdate_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProcessIDMerge;
        private System.Windows.Forms.Button btnStockMerge;
        private System.Windows.Forms.Button btnClose;
    }
}