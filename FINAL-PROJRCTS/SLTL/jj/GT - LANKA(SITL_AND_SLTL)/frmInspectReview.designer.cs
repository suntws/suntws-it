namespace GT
{
    partial class frmInspectReview
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgv_DefectList = new System.Windows.Forms.DataGridView();
            this.pnl_Cntrls = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txt_Remarks = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.gp_rdp_Grade = new System.Windows.Forms.GroupBox();
            this.rdo_Grade_R = new System.Windows.Forms.RadioButton();
            this.rdo_Grade_E = new System.Windows.Forms.RadioButton();
            this.rdo_Grade_D = new System.Windows.Forms.RadioButton();
            this.rdo_Grade_C = new System.Windows.Forms.RadioButton();
            this.rdo_Grade_B = new System.Windows.Forms.RadioButton();
            this.rdo_Grade_A = new System.Windows.Forms.RadioButton();
            this.pnl_Defects = new System.Windows.Forms.Panel();
            this.txt_Grade = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_DefectObeservation = new System.Windows.Forms.TextBox();
            this.txt_DefectName = new System.Windows.Forms.TextBox();
            this.txt_DefectCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pnl_Jathagam = new System.Windows.Forms.Panel();
            this.txt_Rim = new System.Windows.Forms.TextBox();
            this.txt_Size = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_Type = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_Sidewall = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Brand = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Platform = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txt_StencilNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lstStencilNo = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_DefectList)).BeginInit();
            this.pnl_Cntrls.SuspendLayout();
            this.gp_rdp_Grade.SuspendLayout();
            this.pnl_Defects.SuspendLayout();
            this.pnl_Jathagam.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dgv_DefectList);
            this.panel1.Controls.Add(this.pnl_Cntrls);
            this.panel1.Controls.Add(this.pnl_Jathagam);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1114, 671);
            this.panel1.TabIndex = 0;
            // 
            // dgv_DefectList
            // 
            this.dgv_DefectList.AllowUserToAddRows = false;
            this.dgv_DefectList.AllowUserToDeleteRows = false;
            this.dgv_DefectList.AllowUserToResizeColumns = false;
            this.dgv_DefectList.AllowUserToResizeRows = false;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_DefectList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dgv_DefectList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_DefectList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_DefectList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgv_DefectList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_DefectList.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_DefectList.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgv_DefectList.EnableHeadersVisualStyles = false;
            this.dgv_DefectList.GridColor = System.Drawing.Color.Black;
            this.dgv_DefectList.Location = new System.Drawing.Point(200, 109);
            this.dgv_DefectList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgv_DefectList.MultiSelect = false;
            this.dgv_DefectList.Name = "dgv_DefectList";
            this.dgv_DefectList.ReadOnly = true;
            this.dgv_DefectList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_DefectList.RowHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgv_DefectList.RowHeadersVisible = false;
            this.dgv_DefectList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_DefectList.RowsDefaultCellStyle = dataGridViewCellStyle20;
            this.dgv_DefectList.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_DefectList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv_DefectList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgv_DefectList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.dgv_DefectList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgv_DefectList.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_DefectList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_DefectList.Size = new System.Drawing.Size(906, 292);
            this.dgv_DefectList.TabIndex = 100;
            this.dgv_DefectList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_DefectList_CellContentClick);
            // 
            // pnl_Cntrls
            // 
            this.pnl_Cntrls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.pnl_Cntrls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Cntrls.Controls.Add(this.label14);
            this.pnl_Cntrls.Controls.Add(this.label13);
            this.pnl_Cntrls.Controls.Add(this.txt_Remarks);
            this.pnl_Cntrls.Controls.Add(this.btnSave);
            this.pnl_Cntrls.Controls.Add(this.btnClear);
            this.pnl_Cntrls.Controls.Add(this.gp_rdp_Grade);
            this.pnl_Cntrls.Controls.Add(this.pnl_Defects);
            this.pnl_Cntrls.Location = new System.Drawing.Point(200, 406);
            this.pnl_Cntrls.Name = "pnl_Cntrls";
            this.pnl_Cntrls.Size = new System.Drawing.Size(907, 258);
            this.pnl_Cntrls.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.SeaGreen;
            this.label14.Location = new System.Drawing.Point(457, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(96, 19);
            this.label14.TabIndex = 81;
            this.label14.Text = "Select Grade";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.SeaGreen;
            this.label13.Location = new System.Drawing.Point(455, 82);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(112, 19);
            this.label13.TabIndex = 80;
            this.label13.Text = "Enter Remarks";
            // 
            // txt_Remarks
            // 
            this.txt_Remarks.BackColor = System.Drawing.Color.White;
            this.txt_Remarks.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Remarks.Location = new System.Drawing.Point(452, 103);
            this.txt_Remarks.Multiline = true;
            this.txt_Remarks.Name = "txt_Remarks";
            this.txt_Remarks.Size = new System.Drawing.Size(432, 106);
            this.txt_Remarks.TabIndex = 79;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(51)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(554, 216);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 36);
            this.btnSave.TabIndex = 77;
            this.btnSave.Text = "&SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(173)))), ((int)(((byte)(78)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(708, 214);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(105, 36);
            this.btnClear.TabIndex = 78;
            this.btnClear.Text = "&CLEAR";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // gp_rdp_Grade
            // 
            this.gp_rdp_Grade.BackColor = System.Drawing.Color.Transparent;
            this.gp_rdp_Grade.Controls.Add(this.rdo_Grade_R);
            this.gp_rdp_Grade.Controls.Add(this.rdo_Grade_E);
            this.gp_rdp_Grade.Controls.Add(this.rdo_Grade_D);
            this.gp_rdp_Grade.Controls.Add(this.rdo_Grade_C);
            this.gp_rdp_Grade.Controls.Add(this.rdo_Grade_B);
            this.gp_rdp_Grade.Controls.Add(this.rdo_Grade_A);
            this.gp_rdp_Grade.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gp_rdp_Grade.Location = new System.Drawing.Point(451, 32);
            this.gp_rdp_Grade.Name = "gp_rdp_Grade";
            this.gp_rdp_Grade.Size = new System.Drawing.Size(432, 48);
            this.gp_rdp_Grade.TabIndex = 3;
            this.gp_rdp_Grade.TabStop = false;
            // 
            // rdo_Grade_R
            // 
            this.rdo_Grade_R.AutoSize = true;
            this.rdo_Grade_R.Location = new System.Drawing.Point(379, 20);
            this.rdo_Grade_R.Name = "rdo_Grade_R";
            this.rdo_Grade_R.Size = new System.Drawing.Size(37, 23);
            this.rdo_Grade_R.TabIndex = 5;
            this.rdo_Grade_R.TabStop = true;
            this.rdo_Grade_R.Text = "R";
            this.rdo_Grade_R.UseVisualStyleBackColor = true;
            // 
            // rdo_Grade_E
            // 
            this.rdo_Grade_E.AutoSize = true;
            this.rdo_Grade_E.Location = new System.Drawing.Point(309, 20);
            this.rdo_Grade_E.Name = "rdo_Grade_E";
            this.rdo_Grade_E.Size = new System.Drawing.Size(36, 23);
            this.rdo_Grade_E.TabIndex = 4;
            this.rdo_Grade_E.TabStop = true;
            this.rdo_Grade_E.Text = "E";
            this.rdo_Grade_E.UseVisualStyleBackColor = true;
            // 
            // rdo_Grade_D
            // 
            this.rdo_Grade_D.AutoSize = true;
            this.rdo_Grade_D.Location = new System.Drawing.Point(237, 20);
            this.rdo_Grade_D.Name = "rdo_Grade_D";
            this.rdo_Grade_D.Size = new System.Drawing.Size(38, 23);
            this.rdo_Grade_D.TabIndex = 3;
            this.rdo_Grade_D.TabStop = true;
            this.rdo_Grade_D.Text = "D";
            this.rdo_Grade_D.UseVisualStyleBackColor = true;
            // 
            // rdo_Grade_C
            // 
            this.rdo_Grade_C.AutoSize = true;
            this.rdo_Grade_C.Location = new System.Drawing.Point(165, 20);
            this.rdo_Grade_C.Name = "rdo_Grade_C";
            this.rdo_Grade_C.Size = new System.Drawing.Size(38, 23);
            this.rdo_Grade_C.TabIndex = 2;
            this.rdo_Grade_C.TabStop = true;
            this.rdo_Grade_C.Text = "C";
            this.rdo_Grade_C.UseVisualStyleBackColor = true;
            // 
            // rdo_Grade_B
            // 
            this.rdo_Grade_B.AutoSize = true;
            this.rdo_Grade_B.Location = new System.Drawing.Point(94, 20);
            this.rdo_Grade_B.Name = "rdo_Grade_B";
            this.rdo_Grade_B.Size = new System.Drawing.Size(37, 23);
            this.rdo_Grade_B.TabIndex = 1;
            this.rdo_Grade_B.TabStop = true;
            this.rdo_Grade_B.Text = "B";
            this.rdo_Grade_B.UseVisualStyleBackColor = true;
            // 
            // rdo_Grade_A
            // 
            this.rdo_Grade_A.AutoSize = true;
            this.rdo_Grade_A.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdo_Grade_A.Location = new System.Drawing.Point(22, 20);
            this.rdo_Grade_A.Name = "rdo_Grade_A";
            this.rdo_Grade_A.Size = new System.Drawing.Size(38, 23);
            this.rdo_Grade_A.TabIndex = 0;
            this.rdo_Grade_A.TabStop = true;
            this.rdo_Grade_A.Text = "A";
            this.rdo_Grade_A.UseVisualStyleBackColor = true;
            // 
            // pnl_Defects
            // 
            this.pnl_Defects.BackColor = System.Drawing.Color.GhostWhite;
            this.pnl_Defects.Controls.Add(this.txt_Grade);
            this.pnl_Defects.Controls.Add(this.label11);
            this.pnl_Defects.Controls.Add(this.txt_DefectObeservation);
            this.pnl_Defects.Controls.Add(this.txt_DefectName);
            this.pnl_Defects.Controls.Add(this.txt_DefectCode);
            this.pnl_Defects.Controls.Add(this.label10);
            this.pnl_Defects.Controls.Add(this.label9);
            this.pnl_Defects.Controls.Add(this.label8);
            this.pnl_Defects.Location = new System.Drawing.Point(8, 7);
            this.pnl_Defects.Name = "pnl_Defects";
            this.pnl_Defects.Size = new System.Drawing.Size(437, 243);
            this.pnl_Defects.TabIndex = 2;
            // 
            // txt_Grade
            // 
            this.txt_Grade.BackColor = System.Drawing.Color.White;
            this.txt_Grade.Enabled = false;
            this.txt_Grade.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Grade.Location = new System.Drawing.Point(158, 196);
            this.txt_Grade.Name = "txt_Grade";
            this.txt_Grade.ReadOnly = true;
            this.txt_Grade.Size = new System.Drawing.Size(269, 26);
            this.txt_Grade.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label11.Location = new System.Drawing.Point(11, 196);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 19);
            this.label11.TabIndex = 10;
            this.label11.Text = "Grade";
            // 
            // txt_DefectObeservation
            // 
            this.txt_DefectObeservation.BackColor = System.Drawing.Color.White;
            this.txt_DefectObeservation.Enabled = false;
            this.txt_DefectObeservation.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DefectObeservation.Location = new System.Drawing.Point(158, 119);
            this.txt_DefectObeservation.Multiline = true;
            this.txt_DefectObeservation.Name = "txt_DefectObeservation";
            this.txt_DefectObeservation.ReadOnly = true;
            this.txt_DefectObeservation.Size = new System.Drawing.Size(269, 60);
            this.txt_DefectObeservation.TabIndex = 9;
            // 
            // txt_DefectName
            // 
            this.txt_DefectName.BackColor = System.Drawing.Color.White;
            this.txt_DefectName.Enabled = false;
            this.txt_DefectName.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DefectName.Location = new System.Drawing.Point(158, 55);
            this.txt_DefectName.Multiline = true;
            this.txt_DefectName.Name = "txt_DefectName";
            this.txt_DefectName.ReadOnly = true;
            this.txt_DefectName.Size = new System.Drawing.Size(269, 46);
            this.txt_DefectName.TabIndex = 8;
            // 
            // txt_DefectCode
            // 
            this.txt_DefectCode.BackColor = System.Drawing.Color.White;
            this.txt_DefectCode.Enabled = false;
            this.txt_DefectCode.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DefectCode.Location = new System.Drawing.Point(158, 10);
            this.txt_DefectCode.Name = "txt_DefectCode";
            this.txt_DefectCode.ReadOnly = true;
            this.txt_DefectCode.Size = new System.Drawing.Size(269, 26);
            this.txt_DefectCode.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label10.Location = new System.Drawing.Point(11, 127);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(140, 19);
            this.label10.TabIndex = 3;
            this.label10.Text = "Defect Observation";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label9.Location = new System.Drawing.Point(11, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 19);
            this.label9.TabIndex = 2;
            this.label9.Text = "Defect Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label8.Location = new System.Drawing.Point(11, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 19);
            this.label8.TabIndex = 1;
            this.label8.Text = "Defect Code";
            // 
            // pnl_Jathagam
            // 
            this.pnl_Jathagam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Jathagam.Controls.Add(this.txt_Rim);
            this.pnl_Jathagam.Controls.Add(this.txt_Size);
            this.pnl_Jathagam.Controls.Add(this.label7);
            this.pnl_Jathagam.Controls.Add(this.label6);
            this.pnl_Jathagam.Controls.Add(this.txt_Type);
            this.pnl_Jathagam.Controls.Add(this.label5);
            this.pnl_Jathagam.Controls.Add(this.txt_Sidewall);
            this.pnl_Jathagam.Controls.Add(this.label4);
            this.pnl_Jathagam.Controls.Add(this.txt_Brand);
            this.pnl_Jathagam.Controls.Add(this.label3);
            this.pnl_Jathagam.Controls.Add(this.txt_Platform);
            this.pnl_Jathagam.Controls.Add(this.label2);
            this.pnl_Jathagam.Location = new System.Drawing.Point(199, 34);
            this.pnl_Jathagam.Name = "pnl_Jathagam";
            this.pnl_Jathagam.Size = new System.Drawing.Size(906, 68);
            this.pnl_Jathagam.TabIndex = 2;
            // 
            // txt_Rim
            // 
            this.txt_Rim.BackColor = System.Drawing.Color.White;
            this.txt_Rim.Enabled = false;
            this.txt_Rim.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Rim.Location = new System.Drawing.Point(803, 30);
            this.txt_Rim.Name = "txt_Rim";
            this.txt_Rim.ReadOnly = true;
            this.txt_Rim.Size = new System.Drawing.Size(95, 26);
            this.txt_Rim.TabIndex = 11;
            // 
            // txt_Size
            // 
            this.txt_Size.BackColor = System.Drawing.Color.White;
            this.txt_Size.Enabled = false;
            this.txt_Size.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Size.Location = new System.Drawing.Point(620, 30);
            this.txt_Size.Name = "txt_Size";
            this.txt_Size.ReadOnly = true;
            this.txt_Size.Size = new System.Drawing.Size(170, 26);
            this.txt_Size.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.SeaGreen;
            this.label7.Location = new System.Drawing.Point(806, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 19);
            this.label7.TabIndex = 10;
            this.label7.Text = "Rim";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.SeaGreen;
            this.label6.Location = new System.Drawing.Point(623, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 19);
            this.label6.TabIndex = 8;
            this.label6.Text = "Size";
            // 
            // txt_Type
            // 
            this.txt_Type.BackColor = System.Drawing.Color.White;
            this.txt_Type.Enabled = false;
            this.txt_Type.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Type.Location = new System.Drawing.Point(467, 30);
            this.txt_Type.Name = "txt_Type";
            this.txt_Type.ReadOnly = true;
            this.txt_Type.Size = new System.Drawing.Size(138, 26);
            this.txt_Type.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.SeaGreen;
            this.label5.Location = new System.Drawing.Point(470, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 19);
            this.label5.TabIndex = 6;
            this.label5.Text = "Type";
            // 
            // txt_Sidewall
            // 
            this.txt_Sidewall.BackColor = System.Drawing.Color.White;
            this.txt_Sidewall.Enabled = false;
            this.txt_Sidewall.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Sidewall.Location = new System.Drawing.Point(313, 30);
            this.txt_Sidewall.Name = "txt_Sidewall";
            this.txt_Sidewall.ReadOnly = true;
            this.txt_Sidewall.Size = new System.Drawing.Size(140, 26);
            this.txt_Sidewall.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.SeaGreen;
            this.label4.Location = new System.Drawing.Point(316, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 19);
            this.label4.TabIndex = 4;
            this.label4.Text = "Sidewall";
            // 
            // txt_Brand
            // 
            this.txt_Brand.BackColor = System.Drawing.Color.White;
            this.txt_Brand.Enabled = false;
            this.txt_Brand.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Brand.Location = new System.Drawing.Point(160, 30);
            this.txt_Brand.Name = "txt_Brand";
            this.txt_Brand.ReadOnly = true;
            this.txt_Brand.Size = new System.Drawing.Size(138, 26);
            this.txt_Brand.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SeaGreen;
            this.label3.Location = new System.Drawing.Point(161, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Brand";
            // 
            // txt_Platform
            // 
            this.txt_Platform.BackColor = System.Drawing.Color.White;
            this.txt_Platform.Enabled = false;
            this.txt_Platform.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Platform.Location = new System.Drawing.Point(8, 30);
            this.txt_Platform.Name = "txt_Platform";
            this.txt_Platform.ReadOnly = true;
            this.txt_Platform.Size = new System.Drawing.Size(137, 26);
            this.txt_Platform.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SeaGreen;
            this.label2.Location = new System.Drawing.Point(11, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "Platfrom";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txt_StencilNo);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.lstStencilNo);
            this.panel2.Location = new System.Drawing.Point(2, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(189, 631);
            this.panel2.TabIndex = 1;
            // 
            // txt_StencilNo
            // 
            this.txt_StencilNo.BackColor = System.Drawing.Color.White;
            this.txt_StencilNo.Enabled = false;
            this.txt_StencilNo.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_StencilNo.Location = new System.Drawing.Point(5, 32);
            this.txt_StencilNo.Name = "txt_StencilNo";
            this.txt_StencilNo.ReadOnly = true;
            this.txt_StencilNo.Size = new System.Drawing.Size(179, 32);
            this.txt_StencilNo.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.SeaGreen;
            this.label12.Location = new System.Drawing.Point(29, 7);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 19);
            this.label12.TabIndex = 1;
            this.label12.Text = "Stencil No";
            // 
            // lstStencilNo
            // 
            this.lstStencilNo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lstStencilNo.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstStencilNo.ForeColor = System.Drawing.Color.Black;
            this.lstStencilNo.FormattingEnabled = true;
            this.lstStencilNo.ItemHeight = 23;
            this.lstStencilNo.Location = new System.Drawing.Point(4, 79);
            this.lstStencilNo.Name = "lstStencilNo";
            this.lstStencilNo.Size = new System.Drawing.Size(179, 533);
            this.lstStencilNo.TabIndex = 0;
            this.lstStencilNo.SelectedIndexChanged += new System.EventHandler(this.lstStencilNo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1112, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "GRADE APPROVAL";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmInspectReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1116, 674);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmInspectReview";
            this.Load += new System.EventHandler(this.frmInspectReview_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_DefectList)).EndInit();
            this.pnl_Cntrls.ResumeLayout(false);
            this.pnl_Cntrls.PerformLayout();
            this.gp_rdp_Grade.ResumeLayout(false);
            this.gp_rdp_Grade.PerformLayout();
            this.pnl_Defects.ResumeLayout(false);
            this.pnl_Defects.PerformLayout();
            this.pnl_Jathagam.ResumeLayout(false);
            this.pnl_Jathagam.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lstStencilNo;
        private System.Windows.Forms.Panel pnl_Jathagam;
        private System.Windows.Forms.TextBox txt_Rim;
        private System.Windows.Forms.TextBox txt_Size;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_Type;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_Sidewall;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Brand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Platform;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnl_Cntrls;
        private System.Windows.Forms.Panel pnl_Defects;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox gp_rdp_Grade;
        private System.Windows.Forms.RadioButton rdo_Grade_R;
        private System.Windows.Forms.RadioButton rdo_Grade_E;
        private System.Windows.Forms.RadioButton rdo_Grade_D;
        private System.Windows.Forms.RadioButton rdo_Grade_C;
        private System.Windows.Forms.RadioButton rdo_Grade_B;
        private System.Windows.Forms.RadioButton rdo_Grade_A;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgv_DefectList;
        private System.Windows.Forms.TextBox txt_Grade;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_DefectObeservation;
        private System.Windows.Forms.TextBox txt_DefectName;
        private System.Windows.Forms.TextBox txt_DefectCode;
        private System.Windows.Forms.TextBox txt_StencilNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txt_Remarks;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
    }
}