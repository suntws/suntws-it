namespace GT
{
    partial class frmNonDataStencilBarcode
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
            this.txtProcessID = new System.Windows.Forms.TextBox();
            this.cmbGradeSelection = new System.Windows.Forms.ComboBox();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtStencil = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboRim = new System.Windows.Forms.ComboBox();
            this.cboSize = new System.Windows.Forms.ComboBox();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.cboSidewall = new System.Windows.Forms.ComboBox();
            this.cboBrand = new System.Windows.Forms.ComboBox();
            this.cboPlatform = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.dtpProductionDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.pnlContainer.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtProcessID
            // 
            this.txtProcessID.BackColor = System.Drawing.Color.White;
            this.txtProcessID.Enabled = false;
            this.txtProcessID.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcessID.Location = new System.Drawing.Point(510, 191);
            this.txtProcessID.Name = "txtProcessID";
            this.txtProcessID.Size = new System.Drawing.Size(193, 27);
            this.txtProcessID.TabIndex = 90;
            // 
            // cmbGradeSelection
            // 
            this.cmbGradeSelection.DropDownHeight = 100;
            this.cmbGradeSelection.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGradeSelection.FormattingEnabled = true;
            this.cmbGradeSelection.IntegralHeight = false;
            this.cmbGradeSelection.ItemHeight = 19;
            this.cmbGradeSelection.Location = new System.Drawing.Point(510, 97);
            this.cmbGradeSelection.Name = "cmbGradeSelection";
            this.cmbGradeSelection.Size = new System.Drawing.Size(193, 27);
            this.cmbGradeSelection.TabIndex = 8;
            this.cmbGradeSelection.SelectedIndexChanged += new System.EventHandler(this.cmbGradeSelection_SelectedIndexChanged);
            this.cmbGradeSelection.Click += new System.EventHandler(this.cmbGradeSelection_Click);
            this.cmbGradeSelection.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbGradeSelection_KeyPress);
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pnlContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlContainer.Controls.Add(this.groupBox3);
            this.pnlContainer.Location = new System.Drawing.Point(-1, -3);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Size = new System.Drawing.Size(731, 437);
            this.pnlContainer.TabIndex = 74;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(251)))));
            this.groupBox3.Controls.Add(this.txtStencil);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cboRim);
            this.groupBox3.Controls.Add(this.cboSize);
            this.groupBox3.Controls.Add(this.cboType);
            this.groupBox3.Controls.Add(this.cboSidewall);
            this.groupBox3.Controls.Add(this.cboBrand);
            this.groupBox3.Controls.Add(this.cboPlatform);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.txtLocation);
            this.groupBox3.Controls.Add(this.txtProcessID);
            this.groupBox3.Controls.Add(this.cmbGradeSelection);
            this.groupBox3.Controls.Add(this.dtpProductionDate);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Controls.Add(this.shapeContainer1);
            this.groupBox3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(7, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(714, 419);
            this.groupBox3.TabIndex = 80;
            this.groupBox3.TabStop = false;
            // 
            // txtStencil
            // 
            this.txtStencil.BackColor = System.Drawing.Color.White;
            this.txtStencil.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStencil.Location = new System.Drawing.Point(510, 146);
            this.txtStencil.MaxLength = 10;
            this.txtStencil.Name = "txtStencil";
            this.txtStencil.Size = new System.Drawing.Size(193, 27);
            this.txtStencil.TabIndex = 120;
            this.txtStencil.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStencil_KeyPress);
            this.txtStencil.Leave += new System.EventHandler(this.txtStencil_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Green;
            this.label3.Location = new System.Drawing.Point(412, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 19);
            this.label3.TabIndex = 119;
            this.label3.Text = "STENCIL NO";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(382, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 24);
            this.label1.TabIndex = 116;
            this.label1.Text = " STENCIL BARCODE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboRim
            // 
            this.cboRim.DropDownHeight = 100;
            this.cboRim.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRim.FormattingEnabled = true;
            this.cboRim.IntegralHeight = false;
            this.cboRim.ItemHeight = 19;
            this.cboRim.Location = new System.Drawing.Point(510, 53);
            this.cboRim.Name = "cboRim";
            this.cboRim.Size = new System.Drawing.Size(193, 27);
            this.cboRim.TabIndex = 6;
            this.cboRim.Click += new System.EventHandler(this.cboRim_Click);
            this.cboRim.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboRim_KeyPress);
            // 
            // cboSize
            // 
            this.cboSize.DropDownHeight = 100;
            this.cboSize.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSize.FormattingEnabled = true;
            this.cboSize.IntegralHeight = false;
            this.cboSize.ItemHeight = 19;
            this.cboSize.Location = new System.Drawing.Point(104, 241);
            this.cboSize.Name = "cboSize";
            this.cboSize.Size = new System.Drawing.Size(293, 27);
            this.cboSize.TabIndex = 6;
            this.cboSize.Click += new System.EventHandler(this.cboSize_Click);
            this.cboSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboSize_KeyPress);
            // 
            // cboType
            // 
            this.cboType.DropDownHeight = 100;
            this.cboType.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.FormattingEnabled = true;
            this.cboType.IntegralHeight = false;
            this.cboType.ItemHeight = 19;
            this.cboType.Location = new System.Drawing.Point(104, 191);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(293, 27);
            this.cboType.TabIndex = 5;
            this.cboType.Click += new System.EventHandler(this.cboType_Click);
            this.cboType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboType_KeyPress);
            // 
            // cboSidewall
            // 
            this.cboSidewall.DropDownHeight = 100;
            this.cboSidewall.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSidewall.FormattingEnabled = true;
            this.cboSidewall.IntegralHeight = false;
            this.cboSidewall.ItemHeight = 19;
            this.cboSidewall.Location = new System.Drawing.Point(104, 146);
            this.cboSidewall.Name = "cboSidewall";
            this.cboSidewall.Size = new System.Drawing.Size(293, 27);
            this.cboSidewall.TabIndex = 4;
            this.cboSidewall.Click += new System.EventHandler(this.cboSidewall_Click);
            this.cboSidewall.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboSidewall_KeyPress);
            // 
            // cboBrand
            // 
            this.cboBrand.DropDownHeight = 100;
            this.cboBrand.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBrand.FormattingEnabled = true;
            this.cboBrand.IntegralHeight = false;
            this.cboBrand.ItemHeight = 19;
            this.cboBrand.Location = new System.Drawing.Point(104, 97);
            this.cboBrand.Name = "cboBrand";
            this.cboBrand.Size = new System.Drawing.Size(293, 27);
            this.cboBrand.TabIndex = 3;
            this.cboBrand.Click += new System.EventHandler(this.cboBrand_Click);
            this.cboBrand.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboBrand_KeyPress);
            // 
            // cboPlatform
            // 
            this.cboPlatform.DropDownHeight = 100;
            this.cboPlatform.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPlatform.FormattingEnabled = true;
            this.cboPlatform.IntegralHeight = false;
            this.cboPlatform.ItemHeight = 19;
            this.cboPlatform.Location = new System.Drawing.Point(104, 53);
            this.cboPlatform.Name = "cboPlatform";
            this.cboPlatform.Size = new System.Drawing.Size(293, 27);
            this.cboPlatform.TabIndex = 2;
            this.cboPlatform.Click += new System.EventHandler(this.cboPlatform_Click);
            this.cboPlatform.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboPlatform_KeyPress);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.White;
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.txtBarcode);
            this.groupBox4.Controls.Add(this.btnReset);
            this.groupBox4.Controls.Add(this.btnSave);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.txtRemarks);
            this.groupBox4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(38, 282);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(632, 125);
            this.groupBox4.TabIndex = 100;
            this.groupBox4.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 19);
            this.label2.TabIndex = 103;
            this.label2.Text = "Barcode";
            // 
            // txtBarcode
            // 
            this.txtBarcode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtBarcode.Enabled = false;
            this.txtBarcode.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.Location = new System.Drawing.Point(9, 29);
            this.txtBarcode.Multiline = true;
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(484, 37);
            this.txtBarcode.TabIndex = 102;
            this.txtBarcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(152)))), ((int)(((byte)(178)))));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(504, 79);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(114, 36);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "&RESET";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(173)))), ((int)(((byte)(77)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(504, 29);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 36);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "&SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(59)))), ((int)(((byte)(46)))));
            this.label24.Location = new System.Drawing.Point(430, -16);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(71, 19);
            this.label24.TabIndex = 72;
            this.label24.Text = "Remarks";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(59)))), ((int)(((byte)(46)))));
            this.label23.Location = new System.Drawing.Point(9, 66);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(71, 19);
            this.label23.TabIndex = 22;
            this.label23.Text = "Remarks";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(9, 86);
            this.txtRemarks.MaxLength = 500;
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(482, 36);
            this.txtRemarks.TabIndex = 21;
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocation.Location = new System.Drawing.Point(510, 241);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(193, 27);
            this.txtLocation.TabIndex = 9;
            this.txtLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLocation_KeyPress);
            // 
            // dtpProductionDate
            // 
            this.dtpProductionDate.CustomFormat = "dd/MMM/yyyy";
            this.dtpProductionDate.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpProductionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpProductionDate.Location = new System.Drawing.Point(161, 13);
            this.dtpProductionDate.MaxDate = new System.DateTime(2018, 4, 27, 0, 0, 0, 0);
            this.dtpProductionDate.MinDate = new System.DateTime(2009, 1, 1, 0, 0, 0, 0);
            this.dtpProductionDate.Name = "dtpProductionDate";
            this.dtpProductionDate.Size = new System.Drawing.Size(128, 25);
            this.dtpProductionDate.TabIndex = 7;
            this.dtpProductionDate.Value = new System.DateTime(2018, 4, 27, 0, 0, 0, 0);
            this.dtpProductionDate.ValueChanged += new System.EventHandler(this.dtpProductionDate_ValueChanged);
            this.dtpProductionDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtpProductionDate_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Green;
            this.label5.Location = new System.Drawing.Point(412, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 19);
            this.label5.TabIndex = 27;
            this.label5.Text = "PROCESS ID";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Green;
            this.label12.Location = new System.Drawing.Point(412, 245);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 19);
            this.label12.TabIndex = 12;
            this.label12.Text = "LOCATION";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Green;
            this.label13.Location = new System.Drawing.Point(412, 105);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 19);
            this.label13.TabIndex = 11;
            this.label13.Text = "GRADE";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Green;
            this.label14.Location = new System.Drawing.Point(5, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(153, 19);
            this.label14.TabIndex = 10;
            this.label14.Text = "PRODUCTION DATE";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Green;
            this.label6.Location = new System.Drawing.Point(412, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 19);
            this.label6.TabIndex = 6;
            this.label6.Text = "RIM";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Green;
            this.label11.Location = new System.Drawing.Point(6, 150);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 19);
            this.label11.TabIndex = 5;
            this.label11.Text = "SIDEWALL";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Green;
            this.label19.Location = new System.Drawing.Point(6, 57);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(90, 19);
            this.label19.TabIndex = 4;
            this.label19.Text = "PLATFORM";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.Green;
            this.label20.Location = new System.Drawing.Point(6, 195);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(90, 19);
            this.label20.TabIndex = 3;
            this.label20.Text = "TYRE TYPE";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.Green;
            this.label25.Location = new System.Drawing.Point(6, 245);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(84, 19);
            this.label25.TabIndex = 1;
            this.label25.Text = "TYRE SIZE";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.Green;
            this.label26.Location = new System.Drawing.Point(6, 101);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(63, 19);
            this.label26.TabIndex = 0;
            this.label26.Text = "BRAND";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 22);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(708, 394);
            this.shapeContainer1.TabIndex = 118;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = -5;
            this.lineShape1.X2 = 851;
            this.lineShape1.Y1 = 17;
            this.lineShape1.Y2 = 17;
            // 
            // frmNonDataStencilBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 432);
            this.ControlBox = false;
            this.Controls.Add(this.pnlContainer);
            this.Name = "frmNonDataStencilBarcode";
            this.Load += new System.EventHandler(this.frmDailyProduction_Load);
            this.pnlContainer.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContainer;
        private System.Windows.Forms.TextBox txtProcessID;
        private System.Windows.Forms.ComboBox cmbGradeSelection;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.DateTimePicker dtpProductionDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.ComboBox cboSize;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.ComboBox cboSidewall;
        private System.Windows.Forms.ComboBox cboBrand;
        private System.Windows.Forms.ComboBox cboPlatform;
        private System.Windows.Forms.ComboBox cboRim;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.TextBox txtStencil;
        private System.Windows.Forms.Label label3;
    }
}