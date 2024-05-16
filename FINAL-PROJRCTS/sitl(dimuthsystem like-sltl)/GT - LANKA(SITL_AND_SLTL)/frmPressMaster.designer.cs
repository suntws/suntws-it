namespace GT
{
    partial class frmPressMaster
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpType = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.grpPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgv_PressMaster = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.lbl_Press = new System.Windows.Forms.Label();
            this.lbl_PressMaster = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.grpType.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PressMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpType);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(2, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1061, 504);
            this.panel1.TabIndex = 0;
            // 
            // grpType
            // 
            this.grpType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.grpType.Controls.Add(this.label1);
            this.grpType.Controls.Add(this.cmbCategory);
            this.grpType.Controls.Add(this.grpPanel);
            this.grpType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpType.Location = new System.Drawing.Point(282, 5);
            this.grpType.Name = "grpType";
            this.grpType.Size = new System.Drawing.Size(771, 493);
            this.grpType.TabIndex = 22;
            this.grpType.TabStop = false;
            this.grpType.Text = "TYRE SIZE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "CATEGORY";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(96, 19);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(121, 21);
            this.cmbCategory.TabIndex = 1;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // grpPanel
            // 
            this.grpPanel.AutoScroll = true;
            this.grpPanel.BackColor = System.Drawing.Color.Transparent;
            this.grpPanel.Location = new System.Drawing.Point(8, 55);
            this.grpPanel.Name = "grpPanel";
            this.grpPanel.Size = new System.Drawing.Size(758, 431);
            this.grpPanel.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.lblErrMsg);
            this.panel2.Controls.Add(this.lbl_Press);
            this.panel2.Controls.Add(this.lbl_PressMaster);
            this.panel2.Location = new System.Drawing.Point(4, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(273, 495);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgv_PressMaster);
            this.panel3.Location = new System.Drawing.Point(13, 110);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(248, 381);
            this.panel3.TabIndex = 1;
            // 
            // dgv_PressMaster
            // 
            this.dgv_PressMaster.AllowUserToAddRows = false;
            this.dgv_PressMaster.AllowUserToDeleteRows = false;
            this.dgv_PressMaster.AllowUserToResizeColumns = false;
            this.dgv_PressMaster.AllowUserToResizeRows = false;
            this.dgv_PressMaster.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_PressMaster.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_PressMaster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_PressMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv_PressMaster.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgv_PressMaster.EnableHeadersVisualStyles = false;
            this.dgv_PressMaster.Location = new System.Drawing.Point(6, 5);
            this.dgv_PressMaster.MultiSelect = false;
            this.dgv_PressMaster.Name = "dgv_PressMaster";
            this.dgv_PressMaster.ReadOnly = true;
            this.dgv_PressMaster.RowHeadersVisible = false;
            this.dgv_PressMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_PressMaster.Size = new System.Drawing.Size(237, 369);
            this.dgv_PressMaster.TabIndex = 28;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(64, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(196, 22);
            this.textBox1.TabIndex = 7;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Location = new System.Drawing.Point(160, 52);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 31);
            this.btnSave.TabIndex = 50;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Location = new System.Drawing.Point(49, 52);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 31);
            this.btnClear.TabIndex = 49;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.AutoSize = true;
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(4, 90);
            this.lblErrMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(0, 14);
            this.lblErrMsg.TabIndex = 5;
            // 
            // lbl_Press
            // 
            this.lbl_Press.AutoSize = true;
            this.lbl_Press.Location = new System.Drawing.Point(6, 29);
            this.lbl_Press.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Press.Name = "lbl_Press";
            this.lbl_Press.Size = new System.Drawing.Size(51, 14);
            this.lbl_Press.TabIndex = 4;
            this.lbl_Press.Text = "PRESS";
            // 
            // lbl_PressMaster
            // 
            this.lbl_PressMaster.AutoSize = true;
            this.lbl_PressMaster.BackColor = System.Drawing.Color.Cyan;
            this.lbl_PressMaster.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_PressMaster.Location = new System.Drawing.Point(14, 2);
            this.lbl_PressMaster.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_PressMaster.Name = "lbl_PressMaster";
            this.lbl_PressMaster.Size = new System.Drawing.Size(241, 18);
            this.lbl_PressMaster.TabIndex = 3;
            this.lbl_PressMaster.Text = "SIZE ALLOCATE TO PRESS";
            // 
            // frmPressMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 504);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmPressMaster";
            this.Load += new System.EventHandler(this.frmPressMaster_Load);
            this.panel1.ResumeLayout(false);
            this.grpType.ResumeLayout(false);
            this.grpType.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PressMaster)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dgv_PressMaster;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblErrMsg;
        private System.Windows.Forms.Label lbl_Press;
        private System.Windows.Forms.Label lbl_PressMaster;
        private System.Windows.Forms.GroupBox grpType;
        private System.Windows.Forms.Panel grpPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCategory;
    }
}