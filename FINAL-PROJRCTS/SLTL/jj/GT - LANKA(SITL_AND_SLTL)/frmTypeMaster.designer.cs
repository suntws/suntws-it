namespace GT
{
    partial class frmTypeMaster
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSuspend = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.pnlCategory = new System.Windows.Forms.Panel();
            this.rdoBoth = new System.Windows.Forms.RadioButton();
            this.rdoExport = new System.Windows.Forms.RadioButton();
            this.rdoDomestic = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.grpOriginFriction = new System.Windows.Forms.GroupBox();
            this.pnlOriginFriction = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label12 = new System.Windows.Forms.Label();
            this.gvTypeMaster = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoCureFamily3 = new System.Windows.Forms.RadioButton();
            this.rdoCureFamily2 = new System.Windows.Forms.RadioButton();
            this.rdoCureFamily1 = new System.Windows.Forms.RadioButton();
            this.rdbbwstatusno = new System.Windows.Forms.RadioButton();
            this.rdbbwstatus = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtTreadPer = new System.Windows.Forms.TextBox();
            this.txtTread = new System.Windows.Forms.TextBox();
            this.txtCenterPer = new System.Windows.Forms.TextBox();
            this.txtCenter = new System.Windows.Forms.TextBox();
            this.txtInterfacePer = new System.Windows.Forms.TextBox();
            this.txtInterface = new System.Windows.Forms.TextBox();
            this.txtBasePer = new System.Windows.Forms.TextBox();
            this.txtBase = new System.Windows.Forms.TextBox();
            this.txtHbwPer = new System.Windows.Forms.TextBox();
            this.txtHbw = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlCategory.SuspendLayout();
            this.grpOriginFriction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvTypeMaster)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnSuspend);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.pnlCategory);
            this.panel2.Controls.Add(this.grpOriginFriction);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.gvTypeMaster);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Location = new System.Drawing.Point(0, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1285, 586);
            this.panel2.TabIndex = 50;
            // 
            // btnSuspend
            // 
            this.btnSuspend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnSuspend.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSuspend.ForeColor = System.Drawing.Color.White;
            this.btnSuspend.Location = new System.Drawing.Point(1130, 22);
            this.btnSuspend.Name = "btnSuspend";
            this.btnSuspend.Size = new System.Drawing.Size(88, 25);
            this.btnSuspend.TabIndex = 75;
            this.btnSuspend.Text = "SUSPEND";
            this.btnSuspend.UseVisualStyleBackColor = false;
            this.btnSuspend.Visible = false;
            this.btnSuspend.Click += new System.EventHandler(this.btnSuspend_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel4.Controls.Add(this.cboType);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Location = new System.Drawing.Point(401, 94);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(358, 28);
            this.panel4.TabIndex = 37;
            // 
            // cboType
            // 
            this.cboType.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.FormattingEnabled = true;
            this.cboType.IntegralHeight = false;
            this.cboType.ItemHeight = 16;
            this.cboType.Location = new System.Drawing.Point(176, 2);
            this.cboType.MaxDropDownItems = 10;
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(175, 24);
            this.cboType.TabIndex = 137;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 8);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(174, 14);
            this.label16.TabIndex = 32;
            this.label16.Text = "PROCESS-ID EQUAL TYPE :";
            // 
            // pnlCategory
            // 
            this.pnlCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pnlCategory.Controls.Add(this.rdoBoth);
            this.pnlCategory.Controls.Add(this.rdoExport);
            this.pnlCategory.Controls.Add(this.rdoDomestic);
            this.pnlCategory.Controls.Add(this.label15);
            this.pnlCategory.Location = new System.Drawing.Point(0, 94);
            this.pnlCategory.Name = "pnlCategory";
            this.pnlCategory.Size = new System.Drawing.Size(395, 28);
            this.pnlCategory.TabIndex = 34;
            // 
            // rdoBoth
            // 
            this.rdoBoth.AutoSize = true;
            this.rdoBoth.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoBoth.Location = new System.Drawing.Point(120, 4);
            this.rdoBoth.Name = "rdoBoth";
            this.rdoBoth.Size = new System.Drawing.Size(66, 20);
            this.rdoBoth.TabIndex = 34;
            this.rdoBoth.TabStop = true;
            this.rdoBoth.Text = "BOTH";
            this.rdoBoth.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rdoBoth.UseVisualStyleBackColor = true;
            // 
            // rdoExport
            // 
            this.rdoExport.AutoSize = true;
            this.rdoExport.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoExport.Location = new System.Drawing.Point(303, 4);
            this.rdoExport.Name = "rdoExport";
            this.rdoExport.Size = new System.Drawing.Size(82, 20);
            this.rdoExport.TabIndex = 33;
            this.rdoExport.TabStop = true;
            this.rdoExport.Text = "EXPORT";
            this.rdoExport.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rdoExport.UseVisualStyleBackColor = true;
            // 
            // rdoDomestic
            // 
            this.rdoDomestic.AutoSize = true;
            this.rdoDomestic.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoDomestic.Location = new System.Drawing.Point(194, 4);
            this.rdoDomestic.Name = "rdoDomestic";
            this.rdoDomestic.Size = new System.Drawing.Size(101, 20);
            this.rdoDomestic.TabIndex = 32;
            this.rdoDomestic.TabStop = true;
            this.rdoDomestic.Text = "DOMESTIC";
            this.rdoDomestic.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rdoDomestic.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(4, 7);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(115, 14);
            this.label15.TabIndex = 31;
            this.label15.Text = "TYPE CATEGORY :";
            // 
            // grpOriginFriction
            // 
            this.grpOriginFriction.BackColor = System.Drawing.Color.White;
            this.grpOriginFriction.Controls.Add(this.pnlOriginFriction);
            this.grpOriginFriction.Location = new System.Drawing.Point(1094, 92);
            this.grpOriginFriction.Name = "grpOriginFriction";
            this.grpOriginFriction.Size = new System.Drawing.Size(187, 486);
            this.grpOriginFriction.TabIndex = 33;
            this.grpOriginFriction.TabStop = false;
            // 
            // pnlOriginFriction
            // 
            this.pnlOriginFriction.AutoScroll = true;
            this.pnlOriginFriction.BackColor = System.Drawing.Color.White;
            this.pnlOriginFriction.Location = new System.Drawing.Point(2, 15);
            this.pnlOriginFriction.Name = "pnlOriginFriction";
            this.pnlOriginFriction.Size = new System.Drawing.Size(175, 465);
            this.pnlOriginFriction.TabIndex = 32;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(1121, 71);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(130, 14);
            this.label14.TabIndex = 32;
            this.label14.Text = "ORIGIN FRICTION";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(761, 97);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(331, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 30;
            this.progressBar1.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(405, 4);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(134, 18);
            this.label12.TabIndex = 29;
            this.label12.Text = "TYPE MASTER";
            // 
            // gvTypeMaster
            // 
            this.gvTypeMaster.AllowUserToAddRows = false;
            this.gvTypeMaster.AllowUserToDeleteRows = false;
            this.gvTypeMaster.AllowUserToResizeColumns = false;
            this.gvTypeMaster.AllowUserToResizeRows = false;
            this.gvTypeMaster.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvTypeMaster.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvTypeMaster.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvTypeMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTypeMaster.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gvTypeMaster.EnableHeadersVisualStyles = false;
            this.gvTypeMaster.Location = new System.Drawing.Point(0, 123);
            this.gvTypeMaster.MultiSelect = false;
            this.gvTypeMaster.Name = "gvTypeMaster";
            this.gvTypeMaster.ReadOnly = true;
            this.gvTypeMaster.RowHeadersVisible = false;
            this.gvTypeMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvTypeMaster.Size = new System.Drawing.Size(1092, 432);
            this.gvTypeMaster.TabIndex = 27;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.rdbbwstatusno);
            this.panel1.Controls.Add(this.rdbbwstatus);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.txtTreadPer);
            this.panel1.Controls.Add(this.txtTread);
            this.panel1.Controls.Add(this.txtCenterPer);
            this.panel1.Controls.Add(this.txtCenter);
            this.panel1.Controls.Add(this.txtInterfacePer);
            this.panel1.Controls.Add(this.txtInterface);
            this.panel1.Controls.Add(this.txtBasePer);
            this.panel1.Controls.Add(this.txtBase);
            this.panel1.Controls.Add(this.txtHbwPer);
            this.panel1.Controls.Add(this.txtHbw);
            this.panel1.Controls.Add(this.txtType);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1091, 70);
            this.panel1.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.rdoCureFamily3);
            this.groupBox1.Controls.Add(this.rdoCureFamily2);
            this.groupBox1.Controls.Add(this.rdoCureFamily1);
            this.groupBox1.Location = new System.Drawing.Point(795, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(118, 54);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CURE FAMILY";
            // 
            // rdoCureFamily3
            // 
            this.rdoCureFamily3.AutoSize = true;
            this.rdoCureFamily3.Location = new System.Drawing.Point(81, 22);
            this.rdoCureFamily3.Name = "rdoCureFamily3";
            this.rdoCureFamily3.Size = new System.Drawing.Size(33, 18);
            this.rdoCureFamily3.TabIndex = 3;
            this.rdoCureFamily3.TabStop = true;
            this.rdoCureFamily3.Text = "3";
            this.rdoCureFamily3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rdoCureFamily3.UseVisualStyleBackColor = true;
            // 
            // rdoCureFamily2
            // 
            this.rdoCureFamily2.AutoSize = true;
            this.rdoCureFamily2.Location = new System.Drawing.Point(42, 22);
            this.rdoCureFamily2.Name = "rdoCureFamily2";
            this.rdoCureFamily2.Size = new System.Drawing.Size(33, 18);
            this.rdoCureFamily2.TabIndex = 2;
            this.rdoCureFamily2.TabStop = true;
            this.rdoCureFamily2.Text = "2";
            this.rdoCureFamily2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rdoCureFamily2.UseVisualStyleBackColor = true;
            // 
            // rdoCureFamily1
            // 
            this.rdoCureFamily1.AutoSize = true;
            this.rdoCureFamily1.Location = new System.Drawing.Point(2, 23);
            this.rdoCureFamily1.Name = "rdoCureFamily1";
            this.rdoCureFamily1.Size = new System.Drawing.Size(33, 18);
            this.rdoCureFamily1.TabIndex = 1;
            this.rdoCureFamily1.TabStop = true;
            this.rdoCureFamily1.Text = "1";
            this.rdoCureFamily1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rdoCureFamily1.UseVisualStyleBackColor = true;
            // 
            // rdbbwstatusno
            // 
            this.rdbbwstatusno.AutoSize = true;
            this.rdbbwstatusno.Location = new System.Drawing.Point(969, 32);
            this.rdbbwstatusno.Name = "rdbbwstatusno";
            this.rdbbwstatusno.Size = new System.Drawing.Size(44, 18);
            this.rdbbwstatusno.TabIndex = 41;
            this.rdbbwstatusno.TabStop = true;
            this.rdbbwstatusno.Text = "NO";
            this.rdbbwstatusno.UseVisualStyleBackColor = true;
            // 
            // rdbbwstatus
            // 
            this.rdbbwstatus.AutoSize = true;
            this.rdbbwstatus.Location = new System.Drawing.Point(918, 32);
            this.rdbbwstatus.Name = "rdbbwstatus";
            this.rdbbwstatus.Size = new System.Drawing.Size(48, 18);
            this.rdbbwstatus.TabIndex = 40;
            this.rdbbwstatus.TabStop = true;
            this.rdbbwstatus.Text = "YES";
            this.rdbbwstatus.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(922, 12);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 14);
            this.label13.TabIndex = 39;
            this.label13.Text = "BW STATUS";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(1017, 37);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(1017, 8);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(68, 23);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtTreadPer
            // 
            this.txtTreadPer.Location = new System.Drawing.Point(747, 31);
            this.txtTreadPer.MaxLength = 5;
            this.txtTreadPer.Name = "txtTreadPer";
            this.txtTreadPer.Size = new System.Drawing.Size(42, 22);
            this.txtTreadPer.TabIndex = 10;
            // 
            // txtTread
            // 
            this.txtTread.Location = new System.Drawing.Point(661, 31);
            this.txtTread.MaxLength = 20;
            this.txtTread.Name = "txtTread";
            this.txtTread.Size = new System.Drawing.Size(79, 22);
            this.txtTread.TabIndex = 9;
            this.txtTread.Leave += new System.EventHandler(this.txt_ConcateInterface_Leave);
            // 
            // txtCenterPer
            // 
            this.txtCenterPer.Location = new System.Drawing.Point(612, 31);
            this.txtCenterPer.MaxLength = 5;
            this.txtCenterPer.Name = "txtCenterPer";
            this.txtCenterPer.Size = new System.Drawing.Size(42, 22);
            this.txtCenterPer.TabIndex = 8;
            // 
            // txtCenter
            // 
            this.txtCenter.Location = new System.Drawing.Point(526, 31);
            this.txtCenter.MaxLength = 20;
            this.txtCenter.Name = "txtCenter";
            this.txtCenter.Size = new System.Drawing.Size(79, 22);
            this.txtCenter.TabIndex = 7;
            this.txtCenter.Leave += new System.EventHandler(this.txt_ConcateInterface_Leave);
            // 
            // txtInterfacePer
            // 
            this.txtInterfacePer.Location = new System.Drawing.Point(477, 31);
            this.txtInterfacePer.MaxLength = 5;
            this.txtInterfacePer.Name = "txtInterfacePer";
            this.txtInterfacePer.Size = new System.Drawing.Size(42, 22);
            this.txtInterfacePer.TabIndex = 6;
            // 
            // txtInterface
            // 
            this.txtInterface.Enabled = false;
            this.txtInterface.Location = new System.Drawing.Point(359, 31);
            this.txtInterface.MaxLength = 20;
            this.txtInterface.Name = "txtInterface";
            this.txtInterface.Size = new System.Drawing.Size(111, 22);
            this.txtInterface.TabIndex = 5;
            // 
            // txtBasePer
            // 
            this.txtBasePer.Location = new System.Drawing.Point(310, 31);
            this.txtBasePer.MaxLength = 5;
            this.txtBasePer.Name = "txtBasePer";
            this.txtBasePer.Size = new System.Drawing.Size(42, 22);
            this.txtBasePer.TabIndex = 4;
            // 
            // txtBase
            // 
            this.txtBase.Location = new System.Drawing.Point(224, 31);
            this.txtBase.MaxLength = 20;
            this.txtBase.Name = "txtBase";
            this.txtBase.Size = new System.Drawing.Size(79, 22);
            this.txtBase.TabIndex = 3;
            this.txtBase.Leave += new System.EventHandler(this.txt_ConcateInterface_Leave);
            // 
            // txtHbwPer
            // 
            this.txtHbwPer.Location = new System.Drawing.Point(175, 31);
            this.txtHbwPer.MaxLength = 5;
            this.txtHbwPer.Name = "txtHbwPer";
            this.txtHbwPer.Size = new System.Drawing.Size(42, 22);
            this.txtHbwPer.TabIndex = 2;
            // 
            // txtHbw
            // 
            this.txtHbw.Location = new System.Drawing.Point(89, 31);
            this.txtHbw.MaxLength = 20;
            this.txtHbw.Name = "txtHbw";
            this.txtHbw.Size = new System.Drawing.Size(79, 22);
            this.txtHbw.TabIndex = 1;
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(3, 31);
            this.txtType.MaxLength = 10;
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(79, 22);
            this.txtType.TabIndex = 0;
            this.txtType.Leave += new System.EventHandler(this.txtType_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(758, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(20, 14);
            this.label11.TabIndex = 35;
            this.label11.Text = "%";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(677, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 14);
            this.label10.TabIndex = 34;
            this.label10.Text = "TREAD";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(623, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 14);
            this.label9.TabIndex = 33;
            this.label9.Text = "%";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(537, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 14);
            this.label8.TabIndex = 32;
            this.label8.Text = "CENTER";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(488, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 14);
            this.label7.TabIndex = 31;
            this.label7.Text = "%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(377, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 14);
            this.label6.TabIndex = 30;
            this.label6.Text = "INTERFACE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(321, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 14);
            this.label5.TabIndex = 29;
            this.label5.Text = "%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(244, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 14);
            this.label4.TabIndex = 28;
            this.label4.Text = "BASE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(186, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 14);
            this.label3.TabIndex = 27;
            this.label3.Text = "%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 14);
            this.label2.TabIndex = 26;
            this.label2.Text = "H+BW";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 14);
            this.label1.TabIndex = 25;
            this.label1.Text = "TYPE";
            // 
            // frmTypeMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 598);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTypeMaster";
            this.Load += new System.EventHandler(this.frmTypeMaster_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlCategory.ResumeLayout(false);
            this.pnlCategory.PerformLayout();
            this.grpOriginFriction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvTypeMaster)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtTreadPer;
        private System.Windows.Forms.TextBox txtTread;
        private System.Windows.Forms.TextBox txtCenterPer;
        private System.Windows.Forms.TextBox txtCenter;
        private System.Windows.Forms.TextBox txtInterfacePer;
        private System.Windows.Forms.TextBox txtInterface;
        private System.Windows.Forms.TextBox txtBasePer;
        private System.Windows.Forms.TextBox txtBase;
        private System.Windows.Forms.TextBox txtHbwPer;
        private System.Windows.Forms.TextBox txtHbw;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gvTypeMaster;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RadioButton rdbbwstatusno;
        private System.Windows.Forms.RadioButton rdbbwstatus;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoCureFamily3;
        private System.Windows.Forms.RadioButton rdoCureFamily2;
        private System.Windows.Forms.RadioButton rdoCureFamily1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox grpOriginFriction;
        private System.Windows.Forms.Panel pnlOriginFriction;
        private System.Windows.Forms.Panel pnlCategory;
        private System.Windows.Forms.RadioButton rdoBoth;
        private System.Windows.Forms.RadioButton rdoExport;
        private System.Windows.Forms.RadioButton rdoDomestic;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Button btnSuspend;

    }
}

