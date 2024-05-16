namespace GT
{
    partial class frmUserCreate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkList1 = new System.Windows.Forms.CheckedListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.gvUserDetails = new System.Windows.Forms.DataGridView();
            this.txtEmpID = new System.Windows.Forms.TextBox();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.txtConfirmPass = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUserDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkList1);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.lblErrMsg);
            this.panel1.Controls.Add(this.gvUserDetails);
            this.panel1.Controls.Add(this.txtEmpID);
            this.panel1.Controls.Add(this.txtMobile);
            this.panel1.Controls.Add(this.txtConfirmPass);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.txtUserName);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1077, 580);
            this.panel1.TabIndex = 29;
            // 
            // chkList1
            // 
            this.chkList1.CheckOnClick = true;
            this.chkList1.ColumnWidth = 250;
            this.chkList1.FormattingEnabled = true;
            this.chkList1.Items.AddRange(new object[] {
            "WEIGHT VARIANT MASTER",
            "FRICTION ORIGIN MASTER",
            "TYPE MASTER",
            "MOULD MASTER",
            "BLEND MASTER",
            "WEIGHT TOLERANCE",
            "D4 DEFECT ALLOWANCE",
            "WEIGHT STANDARD",
            "TYRE SIZE-PRESS STANDARD",
            "PRODUCTION ALLOCATE",
            "RUNNING MOULD ALLOCATE",
            "PRODUCTION REVOKE",
            "WEIGH (STAGE-I)",
            "BUILD (STAGE-II)",
            "CURE (STAGE-III)",
            "INSPECTION (STAGE-IV)",
            "INSPECTION REVIEW",
            "STENCIL-NO UPDATE",
            "STENCIL-NO CANCEL",
            "STENCIL WISE REPORT",
            "COMMON REPORT",
            "COMPOUND CONSUMPTION REPORT",
            "GT BUILD REPORT",
            "LOAD/UNLOAD REPORT",
            "BARCODED REPORT",
            "DEFECTED REPORT",
            "REJECTED REPORT",
            "DISPATCHED REPORT",
            "DASHBOARD",
            "LOAD/UNLOD DELAY APPROVAL",
            "MANUAL WEIGHING ENTRY",
            "COMP CONCESSION CHART",
            "COMP CONCESSION PLAN",
            "RIM STOCK INWARD",
            "RIM STOCK CONVERT"});
            this.chkList1.Location = new System.Drawing.Point(8, 6);
            this.chkList1.MultiColumn = true;
            this.chkList1.Name = "chkList1";
            this.chkList1.Size = new System.Drawing.Size(504, 565);
            this.chkList1.TabIndex = 44;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(904, 64);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 30);
            this.btnDelete.TabIndex = 42;
            this.btnDelete.Text = "SUSPEND";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.AutoSize = true;
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(332, 542);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(0, 14);
            this.lblErrMsg.TabIndex = 41;
            // 
            // gvUserDetails
            // 
            this.gvUserDetails.AllowUserToAddRows = false;
            this.gvUserDetails.AllowUserToDeleteRows = false;
            this.gvUserDetails.AllowUserToResizeColumns = false;
            this.gvUserDetails.AllowUserToResizeRows = false;
            this.gvUserDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvUserDetails.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvUserDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gvUserDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvUserDetails.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gvUserDetails.EnableHeadersVisualStyles = false;
            this.gvUserDetails.Location = new System.Drawing.Point(517, 154);
            this.gvUserDetails.MultiSelect = false;
            this.gvUserDetails.Name = "gvUserDetails";
            this.gvUserDetails.ReadOnly = true;
            this.gvUserDetails.RowHeadersVisible = false;
            this.gvUserDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvUserDetails.Size = new System.Drawing.Size(554, 416);
            this.gvUserDetails.TabIndex = 40;
            // 
            // txtEmpID
            // 
            this.txtEmpID.Location = new System.Drawing.Point(634, 123);
            this.txtEmpID.MaxLength = 12;
            this.txtEmpID.Name = "txtEmpID";
            this.txtEmpID.Size = new System.Drawing.Size(199, 22);
            this.txtEmpID.TabIndex = 39;
            // 
            // txtMobile
            // 
            this.txtMobile.Location = new System.Drawing.Point(634, 94);
            this.txtMobile.MaxLength = 12;
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(199, 22);
            this.txtMobile.TabIndex = 38;
            // 
            // txtConfirmPass
            // 
            this.txtConfirmPass.Location = new System.Drawing.Point(634, 66);
            this.txtConfirmPass.MaxLength = 12;
            this.txtConfirmPass.Name = "txtConfirmPass";
            this.txtConfirmPass.PasswordChar = '*';
            this.txtConfirmPass.Size = new System.Drawing.Size(199, 22);
            this.txtConfirmPass.TabIndex = 37;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(634, 38);
            this.txtPassword.MaxLength = 12;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(199, 22);
            this.txtPassword.TabIndex = 36;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(634, 11);
            this.txtUserName.MaxLength = 50;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(199, 22);
            this.txtUserName.TabIndex = 35;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(904, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClear.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(904, 117);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(90, 30);
            this.btnClear.TabIndex = 33;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(535, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 14);
            this.label5.TabIndex = 32;
            this.label5.Text = "EMPLOYEE ID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(535, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 14);
            this.label4.TabIndex = 31;
            this.label4.Text = "MOBILE";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(535, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 28);
            this.label3.TabIndex = 30;
            this.label3.Text = "CONFIRM PASSWORD";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(535, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 29;
            this.label2.Text = "PASSWORD";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(535, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 14);
            this.label1.TabIndex = 28;
            this.label1.Text = "USER NAME";
            // 
            // frmUserCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 588);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserCreate";
            this.Load += new System.EventHandler(this.frmUserCreate_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUserDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gvUserDetails;
        private System.Windows.Forms.TextBox txtEmpID;
        private System.Windows.Forms.TextBox txtMobile;
        private System.Windows.Forms.TextBox txtConfirmPass;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblErrMsg;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckedListBox chkList1;
    }
}