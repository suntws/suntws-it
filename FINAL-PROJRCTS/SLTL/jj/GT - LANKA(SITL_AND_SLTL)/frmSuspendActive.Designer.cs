namespace GT
{
    partial class frmSuspendActive
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
            this.rdoWtClass = new System.Windows.Forms.RadioButton();
            this.rdoFriction = new System.Windows.Forms.RadioButton();
            this.rdoType = new System.Windows.Forms.RadioButton();
            this.rdoMould = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblRim = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnActive = new System.Windows.Forms.Button();
            this.lblValue = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblCaption = new System.Windows.Forms.Label();
            this.cboName = new System.Windows.Forms.ComboBox();
            this.gvSuspendList = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSuspendList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.rdoWtClass);
            this.panel1.Controls.Add(this.rdoFriction);
            this.panel1.Controls.Add(this.rdoType);
            this.panel1.Controls.Add(this.rdoMould);
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(585, 35);
            this.panel1.TabIndex = 0;
            // 
            // rdoWtClass
            // 
            this.rdoWtClass.AutoSize = true;
            this.rdoWtClass.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoWtClass.Location = new System.Drawing.Point(411, 7);
            this.rdoWtClass.Name = "rdoWtClass";
            this.rdoWtClass.Size = new System.Drawing.Size(161, 21);
            this.rdoWtClass.TabIndex = 3;
            this.rdoWtClass.TabStop = true;
            this.rdoWtClass.Text = "WEIGHT VARIANT";
            this.rdoWtClass.UseVisualStyleBackColor = true;
            this.rdoWtClass.CheckedChanged += new System.EventHandler(this.rdoSuspend_CheckedChanged);
            // 
            // rdoFriction
            // 
            this.rdoFriction.AutoSize = true;
            this.rdoFriction.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoFriction.Location = new System.Drawing.Point(232, 7);
            this.rdoFriction.Name = "rdoFriction";
            this.rdoFriction.Size = new System.Drawing.Size(163, 21);
            this.rdoFriction.TabIndex = 2;
            this.rdoFriction.TabStop = true;
            this.rdoFriction.Text = "FRICTION ORIGIN";
            this.rdoFriction.UseVisualStyleBackColor = true;
            this.rdoFriction.CheckedChanged += new System.EventHandler(this.rdoSuspend_CheckedChanged);
            // 
            // rdoType
            // 
            this.rdoType.AutoSize = true;
            this.rdoType.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoType.Location = new System.Drawing.Point(151, 7);
            this.rdoType.Name = "rdoType";
            this.rdoType.Size = new System.Drawing.Size(65, 21);
            this.rdoType.TabIndex = 1;
            this.rdoType.TabStop = true;
            this.rdoType.Text = "TYPE";
            this.rdoType.UseVisualStyleBackColor = true;
            this.rdoType.CheckedChanged += new System.EventHandler(this.rdoSuspend_CheckedChanged);
            // 
            // rdoMould
            // 
            this.rdoMould.AutoSize = true;
            this.rdoMould.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoMould.Location = new System.Drawing.Point(13, 7);
            this.rdoMould.Name = "rdoMould";
            this.rdoMould.Size = new System.Drawing.Size(122, 21);
            this.rdoMould.TabIndex = 0;
            this.rdoMould.TabStop = true;
            this.rdoMould.Text = "MOULD SIZE";
            this.rdoMould.UseVisualStyleBackColor = true;
            this.rdoMould.CheckedChanged += new System.EventHandler(this.rdoSuspend_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblRim);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.btnActive);
            this.panel2.Controls.Add(this.lblValue);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtRemarks);
            this.panel2.Controls.Add(this.lblCaption);
            this.panel2.Controls.Add(this.cboName);
            this.panel2.Controls.Add(this.gvSuspendList);
            this.panel2.Location = new System.Drawing.Point(0, 71);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(585, 442);
            this.panel2.TabIndex = 1;
            // 
            // lblRim
            // 
            this.lblRim.AutoSize = true;
            this.lblRim.Location = new System.Drawing.Point(443, 353);
            this.lblRim.Name = "lblRim";
            this.lblRim.Size = new System.Drawing.Size(42, 17);
            this.lblRim.TabIndex = 38;
            this.lblRim.Text = "label3";
            this.lblRim.Visible = false;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClear.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(495, 408);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 29);
            this.btnClear.TabIndex = 37;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnActive
            // 
            this.btnActive.BackColor = System.Drawing.Color.Lime;
            this.btnActive.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActive.Location = new System.Drawing.Point(495, 373);
            this.btnActive.Name = "btnActive";
            this.btnActive.Size = new System.Drawing.Size(75, 29);
            this.btnActive.TabIndex = 36;
            this.btnActive.Text = "ACTIVE";
            this.btnActive.UseVisualStyleBackColor = false;
            this.btnActive.Click += new System.EventHandler(this.btnActive_Click);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue.ForeColor = System.Drawing.Color.Green;
            this.lblValue.Location = new System.Drawing.Point(135, 325);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(78, 17);
            this.lblValue.TabIndex = 35;
            this.lblValue.Text = "CAPTION";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Teal;
            this.label2.Location = new System.Drawing.Point(5, 325);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 17);
            this.label2.TabIndex = 34;
            this.label2.Text = "ACTIVATE THIS: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(5, 353);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(334, 17);
            this.label1.TabIndex = 33;
            this.label1.Text = "ENTER REMARKS FOR SUSPEND TO ACTIVATE";
            // 
            // txtRemarks
            // 
            this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemarks.Location = new System.Drawing.Point(3, 373);
            this.txtRemarks.MaxLength = 500;
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(482, 66);
            this.txtRemarks.TabIndex = 32;
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblCaption.Location = new System.Drawing.Point(12, 7);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(74, 17);
            this.lblCaption.TabIndex = 31;
            this.lblCaption.Text = "CAPTION";
            // 
            // cboName
            // 
            this.cboName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboName.FormattingEnabled = true;
            this.cboName.Location = new System.Drawing.Point(180, 4);
            this.cboName.Name = "cboName";
            this.cboName.Size = new System.Drawing.Size(296, 25);
            this.cboName.TabIndex = 30;
            this.cboName.SelectedIndexChanged += new System.EventHandler(this.cboName_SelectedIndexChanged);
            // 
            // gvSuspendList
            // 
            this.gvSuspendList.AllowUserToAddRows = false;
            this.gvSuspendList.AllowUserToDeleteRows = false;
            this.gvSuspendList.AllowUserToResizeColumns = false;
            this.gvSuspendList.AllowUserToResizeRows = false;
            this.gvSuspendList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvSuspendList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvSuspendList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvSuspendList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gvSuspendList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gvSuspendList.EnableHeadersVisualStyles = false;
            this.gvSuspendList.Location = new System.Drawing.Point(3, 33);
            this.gvSuspendList.MultiSelect = false;
            this.gvSuspendList.Name = "gvSuspendList";
            this.gvSuspendList.ReadOnly = true;
            this.gvSuspendList.RowHeadersVisible = false;
            this.gvSuspendList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvSuspendList.Size = new System.Drawing.Size(579, 279);
            this.gvSuspendList.TabIndex = 29;
            this.gvSuspendList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvSuspendList_CellClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(201, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(187, 19);
            this.label4.TabIndex = 2;
            this.label4.Text = "SUSPEND TO ACTIVATE";
            // 
            // frmSuspendActive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 514);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmSuspendActive";
            this.Load += new System.EventHandler(this.frmSuspendActive_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSuspendList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoWtClass;
        private System.Windows.Forms.RadioButton rdoFriction;
        private System.Windows.Forms.RadioButton rdoType;
        private System.Windows.Forms.RadioButton rdoMould;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnActive;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.ComboBox cboName;
        private System.Windows.Forms.DataGridView gvSuspendList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblRim;
    }
}