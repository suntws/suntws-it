namespace GT
{
    partial class frmOrderPressAssign
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvSavedRecords = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gp_ItemProcessing = new System.Windows.Forms.GroupBox();
            this.lbl_processid = new System.Windows.Forms.Label();
            this.txtItemqty = new System.Windows.Forms.TextBox();
            this.txtRimsize = new System.Windows.Forms.TextBox();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.txtTyresize = new System.Windows.Forms.TextBox();
            this.txtTyretype = new System.Windows.Forms.TextBox();
            this.txtsidewall = new System.Windows.Forms.TextBox();
            this.txtBrand = new System.Windows.Forms.TextBox();
            this.txtPlatform = new System.Windows.Forms.TextBox();
            this.lblProcessID = new System.Windows.Forms.Label();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkPress = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStockQuantity = new System.Windows.Forms.TextBox();
            this.txtRequiredQuantity = new System.Windows.Forms.TextBox();
            this.lbl = new System.Windows.Forms.Label();
            this.txtOrderQuantity = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbCustomerName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbWorkOrderNo = new System.Windows.Forms.ComboBox();
            this.lblHeading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSavedRecords)).BeginInit();
            this.panel1.SuspendLayout();
            this.gp_ItemProcessing.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSavedRecords
            // 
            this.dgvSavedRecords.AllowUserToAddRows = false;
            this.dgvSavedRecords.AllowUserToDeleteRows = false;
            this.dgvSavedRecords.AllowUserToResizeColumns = false;
            this.dgvSavedRecords.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSavedRecords.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSavedRecords.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSavedRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSavedRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSavedRecords.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSavedRecords.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSavedRecords.EnableHeadersVisualStyles = false;
            this.dgvSavedRecords.GridColor = System.Drawing.Color.Black;
            this.dgvSavedRecords.Location = new System.Drawing.Point(0, 247);
            this.dgvSavedRecords.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvSavedRecords.MultiSelect = false;
            this.dgvSavedRecords.Name = "dgvSavedRecords";
            this.dgvSavedRecords.ReadOnly = true;
            this.dgvSavedRecords.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSavedRecords.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSavedRecords.RowHeadersVisible = false;
            this.dgvSavedRecords.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSavedRecords.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSavedRecords.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvSavedRecords.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSavedRecords.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvSavedRecords.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.dgvSavedRecords.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvSavedRecords.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSavedRecords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSavedRecords.Size = new System.Drawing.Size(902, 334);
            this.dgvSavedRecords.TabIndex = 2;
            this.dgvSavedRecords.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSavedRecords_CellClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.gp_ItemProcessing);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.lblHeading);
            this.panel1.Controls.Add(this.dgvSavedRecords);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(906, 584);
            this.panel1.TabIndex = 2;
            // 
            // gp_ItemProcessing
            // 
            this.gp_ItemProcessing.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gp_ItemProcessing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.gp_ItemProcessing.Controls.Add(this.lbl_processid);
            this.gp_ItemProcessing.Controls.Add(this.txtItemqty);
            this.gp_ItemProcessing.Controls.Add(this.txtRimsize);
            this.gp_ItemProcessing.Controls.Add(this.lblErrMsg);
            this.gp_ItemProcessing.Controls.Add(this.txtTyresize);
            this.gp_ItemProcessing.Controls.Add(this.txtTyretype);
            this.gp_ItemProcessing.Controls.Add(this.txtsidewall);
            this.gp_ItemProcessing.Controls.Add(this.txtBrand);
            this.gp_ItemProcessing.Controls.Add(this.txtPlatform);
            this.gp_ItemProcessing.Controls.Add(this.lblProcessID);
            this.gp_ItemProcessing.Controls.Add(this.btnComplete);
            this.gp_ItemProcessing.Controls.Add(this.btnClear);
            this.gp_ItemProcessing.Controls.Add(this.chkPress);
            this.gp_ItemProcessing.Controls.Add(this.label2);
            this.gp_ItemProcessing.Controls.Add(this.btnSave);
            this.gp_ItemProcessing.Controls.Add(this.label13);
            this.gp_ItemProcessing.Controls.Add(this.label9);
            this.gp_ItemProcessing.Controls.Add(this.label8);
            this.gp_ItemProcessing.Controls.Add(this.label12);
            this.gp_ItemProcessing.Controls.Add(this.label11);
            this.gp_ItemProcessing.Controls.Add(this.label10);
            this.gp_ItemProcessing.Controls.Add(this.label7);
            this.gp_ItemProcessing.Location = new System.Drawing.Point(-1, 100);
            this.gp_ItemProcessing.Name = "gp_ItemProcessing";
            this.gp_ItemProcessing.Size = new System.Drawing.Size(902, 147);
            this.gp_ItemProcessing.TabIndex = 106;
            this.gp_ItemProcessing.TabStop = false;
            // 
            // lbl_processid
            // 
            this.lbl_processid.AutoSize = true;
            this.lbl_processid.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_processid.ForeColor = System.Drawing.Color.Green;
            this.lbl_processid.Location = new System.Drawing.Point(581, 65);
            this.lbl_processid.Name = "lbl_processid";
            this.lbl_processid.Size = new System.Drawing.Size(100, 18);
            this.lbl_processid.TabIndex = 104;
            this.lbl_processid.Text = "PROCESS ID";
            // 
            // txtItemqty
            // 
            this.txtItemqty.Enabled = false;
            this.txtItemqty.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtItemqty.Location = new System.Drawing.Point(368, 79);
            this.txtItemqty.Name = "txtItemqty";
            this.txtItemqty.Size = new System.Drawing.Size(174, 26);
            this.txtItemqty.TabIndex = 197;
            // 
            // txtRimsize
            // 
            this.txtRimsize.Enabled = false;
            this.txtRimsize.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRimsize.Location = new System.Drawing.Point(189, 79);
            this.txtRimsize.Name = "txtRimsize";
            this.txtRimsize.Size = new System.Drawing.Size(174, 26);
            this.txtRimsize.TabIndex = 196;
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrMsg.AutoSize = true;
            this.lblErrMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblErrMsg.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(11, 119);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(73, 17);
            this.lblErrMsg.TabIndex = 100;
            this.lblErrMsg.Text = "lblErrmsg";
            this.lblErrMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTyresize
            // 
            this.txtTyresize.Enabled = false;
            this.txtTyresize.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTyresize.Location = new System.Drawing.Point(10, 79);
            this.txtTyresize.Name = "txtTyresize";
            this.txtTyresize.Size = new System.Drawing.Size(174, 26);
            this.txtTyresize.TabIndex = 195;
            // 
            // txtTyretype
            // 
            this.txtTyretype.Enabled = false;
            this.txtTyretype.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTyretype.Location = new System.Drawing.Point(550, 28);
            this.txtTyretype.Name = "txtTyretype";
            this.txtTyretype.Size = new System.Drawing.Size(174, 26);
            this.txtTyretype.TabIndex = 194;
            // 
            // txtsidewall
            // 
            this.txtsidewall.Enabled = false;
            this.txtsidewall.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsidewall.Location = new System.Drawing.Point(368, 28);
            this.txtsidewall.Name = "txtsidewall";
            this.txtsidewall.Size = new System.Drawing.Size(174, 26);
            this.txtsidewall.TabIndex = 193;
            // 
            // txtBrand
            // 
            this.txtBrand.Enabled = false;
            this.txtBrand.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBrand.Location = new System.Drawing.Point(189, 28);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(174, 26);
            this.txtBrand.TabIndex = 192;
            // 
            // txtPlatform
            // 
            this.txtPlatform.Enabled = false;
            this.txtPlatform.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlatform.Location = new System.Drawing.Point(10, 28);
            this.txtPlatform.Name = "txtPlatform";
            this.txtPlatform.Size = new System.Drawing.Size(174, 26);
            this.txtPlatform.TabIndex = 191;
            // 
            // lblProcessID
            // 
            this.lblProcessID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblProcessID.AutoSize = true;
            this.lblProcessID.BackColor = System.Drawing.Color.Transparent;
            this.lblProcessID.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(40)))), ((int)(((byte)(1)))));
            this.lblProcessID.Location = new System.Drawing.Point(637, 91);
            this.lblProcessID.Name = "lblProcessID";
            this.lblProcessID.Size = new System.Drawing.Size(0, 17);
            this.lblProcessID.TabIndex = 182;
            this.lblProcessID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnComplete
            // 
            this.btnComplete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(152)))), ((int)(((byte)(36)))));
            this.btnComplete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComplete.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComplete.Location = new System.Drawing.Point(552, 114);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(170, 28);
            this.btnComplete.TabIndex = 6;
            this.btnComplete.Text = "COMPLETE ORDER";
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Red;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(297, 114);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(109, 28);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // chkPress
            // 
            this.chkPress.CheckOnClick = true;
            this.chkPress.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPress.HorizontalScrollbar = true;
            this.chkPress.Location = new System.Drawing.Point(731, 27);
            this.chkPress.Name = "chkPress";
            this.chkPress.Size = new System.Drawing.Size(167, 109);
            this.chkPress.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(434, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 18);
            this.label2.TabIndex = 139;
            this.label2.Text = "QTY";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Olive;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(426, 114);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(109, 28);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Green;
            this.label13.Location = new System.Drawing.Point(765, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 18);
            this.label13.TabIndex = 136;
            this.label13.Text = "PRESS";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Green;
            this.label9.Location = new System.Drawing.Point(233, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 18);
            this.label9.TabIndex = 128;
            this.label9.Text = "RIM SIZE";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Green;
            this.label8.Location = new System.Drawing.Point(50, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 18);
            this.label8.TabIndex = 126;
            this.label8.Text = "TYRE SIZE";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Green;
            this.label12.Location = new System.Drawing.Point(407, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(84, 18);
            this.label12.TabIndex = 134;
            this.label12.Text = "SIDEWALL";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Green;
            this.label11.Location = new System.Drawing.Point(243, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 18);
            this.label11.TabIndex = 132;
            this.label11.Text = "BRAND";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Green;
            this.label10.Location = new System.Drawing.Point(588, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 18);
            this.label10.TabIndex = 130;
            this.label10.Text = "TYRE TYPE";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Green;
            this.label7.Location = new System.Drawing.Point(47, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 18);
            this.label7.TabIndex = 124;
            this.label7.Text = "PLATFORM";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtStockQuantity);
            this.groupBox3.Controls.Add(this.txtRequiredQuantity);
            this.groupBox3.Controls.Add(this.lbl);
            this.groupBox3.Controls.Add(this.txtOrderQuantity);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.cmbCustomerName);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cmbWorkOrderNo);
            this.groupBox3.Location = new System.Drawing.Point(2, 33);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(903, 65);
            this.groupBox3.TabIndex = 105;
            this.groupBox3.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Green;
            this.label4.Location = new System.Drawing.Point(686, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 18);
            this.label4.TabIndex = 113;
            this.label4.Text = "STOCK QTY";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtStockQuantity
            // 
            this.txtStockQuantity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtStockQuantity.BackColor = System.Drawing.Color.White;
            this.txtStockQuantity.Enabled = false;
            this.txtStockQuantity.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStockQuantity.Location = new System.Drawing.Point(696, 30);
            this.txtStockQuantity.Name = "txtStockQuantity";
            this.txtStockQuantity.Size = new System.Drawing.Size(84, 26);
            this.txtStockQuantity.TabIndex = 112;
            // 
            // txtRequiredQuantity
            // 
            this.txtRequiredQuantity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtRequiredQuantity.BackColor = System.Drawing.Color.White;
            this.txtRequiredQuantity.Enabled = false;
            this.txtRequiredQuantity.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRequiredQuantity.Location = new System.Drawing.Point(798, 30);
            this.txtRequiredQuantity.Name = "txtRequiredQuantity";
            this.txtRequiredQuantity.Size = new System.Drawing.Size(84, 26);
            this.txtRequiredQuantity.TabIndex = 111;
            // 
            // lbl
            // 
            this.lbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl.AutoSize = true;
            this.lbl.BackColor = System.Drawing.Color.Transparent;
            this.lbl.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.ForeColor = System.Drawing.Color.Green;
            this.lbl.Location = new System.Drawing.Point(799, 9);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(74, 18);
            this.lbl.TabIndex = 110;
            this.lbl.Text = "REQ QTY";
            this.lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtOrderQuantity
            // 
            this.txtOrderQuantity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtOrderQuantity.BackColor = System.Drawing.Color.White;
            this.txtOrderQuantity.Enabled = false;
            this.txtOrderQuantity.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOrderQuantity.Location = new System.Drawing.Point(591, 30);
            this.txtOrderQuantity.Name = "txtOrderQuantity";
            this.txtOrderQuantity.Size = new System.Drawing.Size(84, 26);
            this.txtOrderQuantity.TabIndex = 109;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Green;
            this.label5.Location = new System.Drawing.Point(578, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 18);
            this.label5.TabIndex = 108;
            this.label5.Text = "ORDER QTY";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Green;
            this.label3.Location = new System.Drawing.Point(415, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 18);
            this.label3.TabIndex = 105;
            this.label3.Text = "WORK ORDER NO";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbCustomerName
            // 
            this.cmbCustomerName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbCustomerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomerName.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCustomerName.FormattingEnabled = true;
            this.cmbCustomerName.IntegralHeight = false;
            this.cmbCustomerName.ItemHeight = 18;
            this.cmbCustomerName.Location = new System.Drawing.Point(13, 30);
            this.cmbCustomerName.MaxDropDownItems = 10;
            this.cmbCustomerName.Name = "cmbCustomerName";
            this.cmbCustomerName.Size = new System.Drawing.Size(384, 26);
            this.cmbCustomerName.TabIndex = 0;
            this.cmbCustomerName.SelectedIndexChanged += new System.EventHandler(this.cmbCustomerName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(122, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 18);
            this.label1.TabIndex = 103;
            this.label1.Text = "CUSTOMER NAME";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbWorkOrderNo
            // 
            this.cmbWorkOrderNo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbWorkOrderNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWorkOrderNo.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbWorkOrderNo.FormattingEnabled = true;
            this.cmbWorkOrderNo.IntegralHeight = false;
            this.cmbWorkOrderNo.ItemHeight = 18;
            this.cmbWorkOrderNo.Location = new System.Drawing.Point(413, 30);
            this.cmbWorkOrderNo.MaxDropDownItems = 10;
            this.cmbWorkOrderNo.Name = "cmbWorkOrderNo";
            this.cmbWorkOrderNo.Size = new System.Drawing.Size(158, 26);
            this.cmbWorkOrderNo.TabIndex = 1;
            this.cmbWorkOrderNo.SelectedIndexChanged += new System.EventHandler(this.cmbWorkOrderNo_SelectedIndexChanged);
            // 
            // lblHeading
            // 
            this.lblHeading.BackColor = System.Drawing.Color.Teal;
            this.lblHeading.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHeading.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.ForeColor = System.Drawing.Color.White;
            this.lblHeading.Location = new System.Drawing.Point(2, 1);
            this.lblHeading.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(903, 30);
            this.lblHeading.TabIndex = 104;
            this.lblHeading.Text = "PRESS ALLOCATION";
            this.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmOrderPressAssign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(912, 673);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOrderPressAssign";
            this.Load += new System.EventHandler(this.frmOrderPressAssign_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSavedRecords)).EndInit();
            this.panel1.ResumeLayout(false);
            this.gp_ItemProcessing.ResumeLayout(false);
            this.gp_ItemProcessing.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSavedRecords;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gp_ItemProcessing;
        private System.Windows.Forms.Label lbl_processid;
        private System.Windows.Forms.TextBox txtItemqty;
        private System.Windows.Forms.TextBox txtRimsize;
        private System.Windows.Forms.Label lblErrMsg;
        private System.Windows.Forms.TextBox txtTyresize;
        private System.Windows.Forms.TextBox txtTyretype;
        private System.Windows.Forms.TextBox txtsidewall;
        private System.Windows.Forms.TextBox txtBrand;
        private System.Windows.Forms.TextBox txtPlatform;
        private System.Windows.Forms.Label lblProcessID;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckedListBox chkPress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStockQuantity;
        private System.Windows.Forms.TextBox txtRequiredQuantity;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.TextBox txtOrderQuantity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbCustomerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbWorkOrderNo;
        private System.Windows.Forms.Label lblHeading;



    }
}