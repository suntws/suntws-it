namespace GT
{
    partial class frmStencilProdModify
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCurrProcessID = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnSAVE = new System.Windows.Forms.Button();
            this.txtPrevProcessId = new System.Windows.Forms.TextBox();
            this.lblprocessid = new System.Windows.Forms.Label();
            this.lblDATE = new System.Windows.Forms.Label();
            this.lblPLATFORM = new System.Windows.Forms.Label();
            this.lblBRAND = new System.Windows.Forms.Label();
            this.lblSIDEWALL = new System.Windows.Forms.Label();
            this.lblTYRESIZE = new System.Windows.Forms.Label();
            this.lblRIM = new System.Windows.Forms.Label();
            this.lblTYPE = new System.Windows.Forms.Label();
            this.lblGRADE = new System.Windows.Forms.Label();
            this.cboPlatform = new System.Windows.Forms.ComboBox();
            this.cbobrand = new System.Windows.Forms.ComboBox();
            this.cboGrade = new System.Windows.Forms.ComboBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.cborim = new System.Windows.Forms.ComboBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.cbotyresize = new System.Windows.Forms.ComboBox();
            this.lblREMARKS = new System.Windows.Forms.Label();
            this.cbosidewall = new System.Windows.Forms.ComboBox();
            this.cbotype = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStencil = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTyreRemarks = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.lblErrMsg);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtStencil);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(764, 439);
            this.panel1.TabIndex = 75;
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.AutoSize = true;
            this.lblErrMsg.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(12, 77);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(98, 14);
            this.lblErrMsg.TabIndex = 110;
            this.lblErrMsg.Text = "Error Message";
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.Location = new System.Drawing.Point(459, 37);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(73, 23);
            this.btnFind.TabIndex = 97;
            this.btnFind.Text = "FIND";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboTyreRemarks);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtCurrProcessID);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.btnSAVE);
            this.groupBox1.Controls.Add(this.txtPrevProcessId);
            this.groupBox1.Controls.Add(this.lblprocessid);
            this.groupBox1.Controls.Add(this.lblDATE);
            this.groupBox1.Controls.Add(this.lblPLATFORM);
            this.groupBox1.Controls.Add(this.lblBRAND);
            this.groupBox1.Controls.Add(this.lblSIDEWALL);
            this.groupBox1.Controls.Add(this.lblTYRESIZE);
            this.groupBox1.Controls.Add(this.lblRIM);
            this.groupBox1.Controls.Add(this.lblTYPE);
            this.groupBox1.Controls.Add(this.lblGRADE);
            this.groupBox1.Controls.Add(this.cboPlatform);
            this.groupBox1.Controls.Add(this.cbobrand);
            this.groupBox1.Controls.Add(this.cboGrade);
            this.groupBox1.Controls.Add(this.txtBarcode);
            this.groupBox1.Controls.Add(this.cborim);
            this.groupBox1.Controls.Add(this.txtRemarks);
            this.groupBox1.Controls.Add(this.cbotyresize);
            this.groupBox1.Controls.Add(this.lblREMARKS);
            this.groupBox1.Controls.Add(this.cbosidewall);
            this.groupBox1.Controls.Add(this.cbotype);
            this.groupBox1.Location = new System.Drawing.Point(2, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 337);
            this.groupBox1.TabIndex = 83;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(564, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 111;
            this.label2.Text = "To";
            // 
            // txtCurrProcessID
            // 
            this.txtCurrProcessID.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrProcessID.Location = new System.Drawing.Point(591, 20);
            this.txtCurrProcessID.Name = "txtCurrProcessID";
            this.txtCurrProcessID.ReadOnly = true;
            this.txtCurrProcessID.Size = new System.Drawing.Size(121, 23);
            this.txtCurrProcessID.TabIndex = 110;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.CustomFormat = "dd-MMM-yyyy";
            this.dateTimePicker1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dateTimePicker1.Location = new System.Drawing.Point(437, 113);
            this.dateTimePicker1.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(121, 23);
            this.dateTimePicker1.TabIndex = 92;
            this.dateTimePicker1.Value = new System.DateTime(2015, 5, 5, 9, 47, 38, 0);
            // 
            // btnSAVE
            // 
            this.btnSAVE.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSAVE.Location = new System.Drawing.Point(529, 297);
            this.btnSAVE.Name = "btnSAVE";
            this.btnSAVE.Size = new System.Drawing.Size(149, 23);
            this.btnSAVE.TabIndex = 96;
            this.btnSAVE.Text = "SAVE && RE-PRINT";
            this.btnSAVE.UseVisualStyleBackColor = true;
            this.btnSAVE.Click += new System.EventHandler(this.btnSAVE_Click);
            // 
            // txtPrevProcessId
            // 
            this.txtPrevProcessId.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrevProcessId.Location = new System.Drawing.Point(437, 21);
            this.txtPrevProcessId.Name = "txtPrevProcessId";
            this.txtPrevProcessId.ReadOnly = true;
            this.txtPrevProcessId.Size = new System.Drawing.Size(121, 23);
            this.txtPrevProcessId.TabIndex = 108;
            // 
            // lblprocessid
            // 
            this.lblprocessid.AutoSize = true;
            this.lblprocessid.Location = new System.Drawing.Point(361, 26);
            this.lblprocessid.Name = "lblprocessid";
            this.lblprocessid.Size = new System.Drawing.Size(72, 13);
            this.lblprocessid.TabIndex = 107;
            this.lblprocessid.Text = "PROCESS ID";
            // 
            // lblDATE
            // 
            this.lblDATE.AutoSize = true;
            this.lblDATE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblDATE.Location = new System.Drawing.Point(361, 118);
            this.lblDATE.Name = "lblDATE";
            this.lblDATE.Size = new System.Drawing.Size(62, 13);
            this.lblDATE.TabIndex = 97;
            this.lblDATE.Text = "MFD DATE";
            this.lblDATE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPLATFORM
            // 
            this.lblPLATFORM.AutoSize = true;
            this.lblPLATFORM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblPLATFORM.Location = new System.Drawing.Point(10, 26);
            this.lblPLATFORM.Name = "lblPLATFORM";
            this.lblPLATFORM.Size = new System.Drawing.Size(65, 13);
            this.lblPLATFORM.TabIndex = 98;
            this.lblPLATFORM.Text = "PLATFORM";
            // 
            // lblBRAND
            // 
            this.lblBRAND.AutoSize = true;
            this.lblBRAND.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblBRAND.Location = new System.Drawing.Point(10, 74);
            this.lblBRAND.Name = "lblBRAND";
            this.lblBRAND.Size = new System.Drawing.Size(45, 13);
            this.lblBRAND.TabIndex = 99;
            this.lblBRAND.Text = "BRAND";
            // 
            // lblSIDEWALL
            // 
            this.lblSIDEWALL.AutoSize = true;
            this.lblSIDEWALL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblSIDEWALL.Location = new System.Drawing.Point(10, 118);
            this.lblSIDEWALL.Name = "lblSIDEWALL";
            this.lblSIDEWALL.Size = new System.Drawing.Size(62, 13);
            this.lblSIDEWALL.TabIndex = 100;
            this.lblSIDEWALL.Text = "SIDEWALL";
            // 
            // lblTYRESIZE
            // 
            this.lblTYRESIZE.AutoSize = true;
            this.lblTYRESIZE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblTYRESIZE.Location = new System.Drawing.Point(10, 202);
            this.lblTYRESIZE.Name = "lblTYRESIZE";
            this.lblTYRESIZE.Size = new System.Drawing.Size(63, 13);
            this.lblTYRESIZE.TabIndex = 102;
            this.lblTYRESIZE.Text = "TYRE SIZE";
            // 
            // lblRIM
            // 
            this.lblRIM.AutoSize = true;
            this.lblRIM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblRIM.Location = new System.Drawing.Point(10, 246);
            this.lblRIM.Name = "lblRIM";
            this.lblRIM.Size = new System.Drawing.Size(27, 13);
            this.lblRIM.TabIndex = 103;
            this.lblRIM.Text = "RIM";
            // 
            // lblTYPE
            // 
            this.lblTYPE.AutoSize = true;
            this.lblTYPE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblTYPE.Location = new System.Drawing.Point(10, 159);
            this.lblTYPE.Name = "lblTYPE";
            this.lblTYPE.Size = new System.Drawing.Size(35, 13);
            this.lblTYPE.TabIndex = 101;
            this.lblTYPE.Text = "TYPE";
            // 
            // lblGRADE
            // 
            this.lblGRADE.AutoSize = true;
            this.lblGRADE.Location = new System.Drawing.Point(361, 74);
            this.lblGRADE.Name = "lblGRADE";
            this.lblGRADE.Size = new System.Drawing.Size(45, 13);
            this.lblGRADE.TabIndex = 104;
            this.lblGRADE.Text = "GRADE";
            // 
            // cboPlatform
            // 
            this.cboPlatform.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboPlatform.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboPlatform.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPlatform.FormattingEnabled = true;
            this.cboPlatform.Location = new System.Drawing.Point(78, 20);
            this.cboPlatform.Name = "cboPlatform";
            this.cboPlatform.Size = new System.Drawing.Size(228, 24);
            this.cboPlatform.TabIndex = 85;
            this.cboPlatform.SelectedIndexChanged += new System.EventHandler(this.cboPlatform_SelectedIndexChanged);
            // 
            // cbobrand
            // 
            this.cbobrand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbobrand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbobrand.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbobrand.FormattingEnabled = true;
            this.cbobrand.Location = new System.Drawing.Point(78, 68);
            this.cbobrand.Name = "cbobrand";
            this.cbobrand.Size = new System.Drawing.Size(228, 24);
            this.cbobrand.TabIndex = 86;
            this.cbobrand.SelectedIndexChanged += new System.EventHandler(this.cbobrand_SelectedIndexChanged);
            // 
            // cboGrade
            // 
            this.cboGrade.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboGrade.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboGrade.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboGrade.FormattingEnabled = true;
            this.cboGrade.Location = new System.Drawing.Point(437, 68);
            this.cboGrade.Name = "cboGrade";
            this.cboGrade.Size = new System.Drawing.Size(121, 24);
            this.cboGrade.TabIndex = 91;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Enabled = false;
            this.txtBarcode.Font = new System.Drawing.Font("Verdana", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.Location = new System.Drawing.Point(6, 286);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(436, 40);
            this.txtBarcode.TabIndex = 94;
            // 
            // cborim
            // 
            this.cborim.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cborim.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cborim.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cborim.FormattingEnabled = true;
            this.cborim.Location = new System.Drawing.Point(78, 240);
            this.cborim.Name = "cborim";
            this.cborim.Size = new System.Drawing.Size(228, 24);
            this.cborim.TabIndex = 90;
            this.cborim.SelectedIndexChanged += new System.EventHandler(this.cborim_SelectedIndexChanged);
            // 
            // txtRemarks
            // 
            this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemarks.Location = new System.Drawing.Point(436, 152);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(315, 64);
            this.txtRemarks.TabIndex = 95;
            // 
            // cbotyresize
            // 
            this.cbotyresize.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbotyresize.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbotyresize.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbotyresize.FormattingEnabled = true;
            this.cbotyresize.Location = new System.Drawing.Point(78, 196);
            this.cbotyresize.Name = "cbotyresize";
            this.cbotyresize.Size = new System.Drawing.Size(228, 24);
            this.cbotyresize.TabIndex = 89;
            this.cbotyresize.SelectedIndexChanged += new System.EventHandler(this.cbotyresize_SelectedIndexChanged);
            // 
            // lblREMARKS
            // 
            this.lblREMARKS.AutoSize = true;
            this.lblREMARKS.Location = new System.Drawing.Point(361, 178);
            this.lblREMARKS.Name = "lblREMARKS";
            this.lblREMARKS.Size = new System.Drawing.Size(60, 13);
            this.lblREMARKS.TabIndex = 106;
            this.lblREMARKS.Text = "REMARKS";
            // 
            // cbosidewall
            // 
            this.cbosidewall.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbosidewall.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbosidewall.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbosidewall.FormattingEnabled = true;
            this.cbosidewall.Location = new System.Drawing.Point(78, 112);
            this.cbosidewall.Name = "cbosidewall";
            this.cbosidewall.Size = new System.Drawing.Size(228, 24);
            this.cbosidewall.TabIndex = 87;
            this.cbosidewall.SelectedIndexChanged += new System.EventHandler(this.cbosidewall_SelectedIndexChanged);
            // 
            // cbotype
            // 
            this.cbotype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbotype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbotype.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbotype.FormattingEnabled = true;
            this.cbotype.Location = new System.Drawing.Point(78, 152);
            this.cbotype.Name = "cbotype";
            this.cbotype.Size = new System.Drawing.Size(228, 24);
            this.cbotype.TabIndex = 88;
            this.cbotype.SelectedIndexChanged += new System.EventHandler(this.cbotype_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(193, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 82;
            this.label1.Text = "STENCIL";
            // 
            // txtStencil
            // 
            this.txtStencil.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.txtStencil.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtStencil.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStencil.Location = new System.Drawing.Point(265, 33);
            this.txtStencil.MaxLength = 12;
            this.txtStencil.Name = "txtStencil";
            this.txtStencil.Size = new System.Drawing.Size(188, 31);
            this.txtStencil.TabIndex = 0;
            this.txtStencil.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStencil_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(320, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 20);
            this.label3.TabIndex = 80;
            this.label3.Text = "EDIT BARCODE";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboTyreRemarks
            // 
            this.cboTyreRemarks.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTyreRemarks.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboTyreRemarks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTyreRemarks.DropDownWidth = 293;
            this.cboTyreRemarks.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboTyreRemarks.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTyreRemarks.FormattingEnabled = true;
            this.cboTyreRemarks.Location = new System.Drawing.Point(358, 243);
            this.cboTyreRemarks.Name = "cboTyreRemarks";
            this.cboTyreRemarks.Size = new System.Drawing.Size(392, 27);
            this.cboTyreRemarks.TabIndex = 112;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(360, 227);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 13);
            this.label4.TabIndex = 113;
            this.label4.Text = "TYRE MODIFY REMARKS";
            // 
            // frmStencilProdModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 451);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmStencilProdModify";
            this.Load += new System.EventHandler(this.frmStencilProdModify_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStencil;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCurrProcessID;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox txtPrevProcessId;
        private System.Windows.Forms.Label lblprocessid;
        private System.Windows.Forms.Label lblDATE;
        private System.Windows.Forms.Label lblPLATFORM;
        private System.Windows.Forms.Label lblBRAND;
        private System.Windows.Forms.Label lblSIDEWALL;
        private System.Windows.Forms.Label lblTYRESIZE;
        private System.Windows.Forms.Label lblRIM;
        private System.Windows.Forms.Label lblTYPE;
        private System.Windows.Forms.Label lblGRADE;
        private System.Windows.Forms.ComboBox cboPlatform;
        private System.Windows.Forms.ComboBox cbobrand;
        private System.Windows.Forms.ComboBox cboGrade;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.ComboBox cborim;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.ComboBox cbotyresize;
        private System.Windows.Forms.Label lblREMARKS;
        private System.Windows.Forms.ComboBox cbosidewall;
        private System.Windows.Forms.ComboBox cbotype;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label lblErrMsg;
        private System.Windows.Forms.Button btnSAVE;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboTyreRemarks;
    }
}