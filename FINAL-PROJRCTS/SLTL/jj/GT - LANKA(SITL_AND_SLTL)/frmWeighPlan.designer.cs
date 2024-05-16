namespace GT
{
    partial class frmWeighPlan
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgv_Prod_PlanItems = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.gp_ItemProcessing = new System.Windows.Forms.GroupBox();
            this.btnTrack = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cboBrand = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTyreSize = new System.Windows.Forms.ComboBox();
            this.cboPress = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.btnFromSeq = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Prod_PlanItems)).BeginInit();
            this.gp_ItemProcessing.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGray;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dgv_Prod_PlanItems);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.gp_ItemProcessing);
            this.panel1.Controls.Add(this.lblErrMsg);
            this.panel1.Location = new System.Drawing.Point(4, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1253, 661);
            this.panel1.TabIndex = 4;
            // 
            // dgv_Prod_PlanItems
            // 
            this.dgv_Prod_PlanItems.AllowUserToAddRows = false;
            this.dgv_Prod_PlanItems.AllowUserToDeleteRows = false;
            this.dgv_Prod_PlanItems.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Prod_PlanItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_Prod_PlanItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Prod_PlanItems.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Prod_PlanItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_Prod_PlanItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Prod_PlanItems.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Prod_PlanItems.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_Prod_PlanItems.EnableHeadersVisualStyles = false;
            this.dgv_Prod_PlanItems.GridColor = System.Drawing.Color.Black;
            this.dgv_Prod_PlanItems.Location = new System.Drawing.Point(3, 38);
            this.dgv_Prod_PlanItems.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgv_Prod_PlanItems.MultiSelect = false;
            this.dgv_Prod_PlanItems.Name = "dgv_Prod_PlanItems";
            this.dgv_Prod_PlanItems.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Prod_PlanItems.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_Prod_PlanItems.RowHeadersVisible = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Prod_PlanItems.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_Prod_PlanItems.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_Prod_PlanItems.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgv_Prod_PlanItems.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgv_Prod_PlanItems.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.dgv_Prod_PlanItems.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgv_Prod_PlanItems.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Prod_PlanItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Prod_PlanItems.Size = new System.Drawing.Size(1245, 618);
            this.dgv_Prod_PlanItems.TabIndex = 160;
            this.dgv_Prod_PlanItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Prod_Weighing_CellContentClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Verdana", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(7, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(351, 23);
            this.label2.TabIndex = 159;
            this.label2.Text = "ALLOCATION BASED WEIGHING";
            // 
            // gp_ItemProcessing
            // 
            this.gp_ItemProcessing.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gp_ItemProcessing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(225)))), ((int)(((byte)(238)))));
            this.gp_ItemProcessing.Controls.Add(this.btnFromSeq);
            this.gp_ItemProcessing.Controls.Add(this.btnTrack);
            this.gp_ItemProcessing.Controls.Add(this.label4);
            this.gp_ItemProcessing.Controls.Add(this.cboBrand);
            this.gp_ItemProcessing.Controls.Add(this.label3);
            this.gp_ItemProcessing.Controls.Add(this.cboType);
            this.gp_ItemProcessing.Controls.Add(this.label1);
            this.gp_ItemProcessing.Controls.Add(this.cboTyreSize);
            this.gp_ItemProcessing.Controls.Add(this.cboPress);
            this.gp_ItemProcessing.Controls.Add(this.label6);
            this.gp_ItemProcessing.Location = new System.Drawing.Point(363, 3);
            this.gp_ItemProcessing.Name = "gp_ItemProcessing";
            this.gp_ItemProcessing.Size = new System.Drawing.Size(883, 32);
            this.gp_ItemProcessing.TabIndex = 154;
            this.gp_ItemProcessing.TabStop = false;
            // 
            // btnTrack
            // 
            this.btnTrack.BackColor = System.Drawing.Color.Yellow;
            this.btnTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrack.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrack.Location = new System.Drawing.Point(539, 4);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(154, 23);
            this.btnTrack.TabIndex = 141;
            this.btnTrack.Text = "ASSIGN NEW PLAN";
            this.btnTrack.UseVisualStyleBackColor = false;
            this.btnTrack.Visible = false;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label4.Location = new System.Drawing.Point(619, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 18);
            this.label4.TabIndex = 145;
            this.label4.Text = "BRAND";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboBrand
            // 
            this.cboBrand.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBrand.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBrand.FormattingEnabled = true;
            this.cboBrand.IntegralHeight = false;
            this.cboBrand.ItemHeight = 18;
            this.cboBrand.Location = new System.Drawing.Point(687, 3);
            this.cboBrand.MaxDropDownItems = 10;
            this.cboBrand.Name = "cboBrand";
            this.cboBrand.Size = new System.Drawing.Size(190, 26);
            this.cboBrand.TabIndex = 144;
            this.cboBrand.SelectedIndexChanged += new System.EventHandler(this.cboBrand_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(462, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 18);
            this.label3.TabIndex = 143;
            this.label3.Text = "TYPE";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboType
            // 
            this.cboType.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.FormattingEnabled = true;
            this.cboType.IntegralHeight = false;
            this.cboType.ItemHeight = 18;
            this.cboType.Location = new System.Drawing.Point(512, 3);
            this.cboType.MaxDropDownItems = 10;
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(103, 26);
            this.cboType.TabIndex = 142;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(219, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 18);
            this.label1.TabIndex = 140;
            this.label1.Text = "SIZE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboTyreSize
            // 
            this.cboTyreSize.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboTyreSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTyreSize.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTyreSize.FormattingEnabled = true;
            this.cboTyreSize.IntegralHeight = false;
            this.cboTyreSize.ItemHeight = 18;
            this.cboTyreSize.Location = new System.Drawing.Point(266, 3);
            this.cboTyreSize.MaxDropDownItems = 10;
            this.cboTyreSize.Name = "cboTyreSize";
            this.cboTyreSize.Size = new System.Drawing.Size(190, 26);
            this.cboTyreSize.TabIndex = 139;
            this.cboTyreSize.SelectedIndexChanged += new System.EventHandler(this.cboTyreSize_SelectedIndexChanged);
            // 
            // cboPress
            // 
            this.cboPress.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboPress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPress.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPress.FormattingEnabled = true;
            this.cboPress.IntegralHeight = false;
            this.cboPress.ItemHeight = 18;
            this.cboPress.Location = new System.Drawing.Point(63, 3);
            this.cboPress.MaxDropDownItems = 10;
            this.cboPress.Name = "cboPress";
            this.cboPress.Size = new System.Drawing.Size(148, 26);
            this.cboPress.TabIndex = 138;
            this.cboPress.SelectedIndexChanged += new System.EventHandler(this.cboPress_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label6.Location = new System.Drawing.Point(3, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 18);
            this.label6.TabIndex = 137;
            this.label6.Text = "PRESS";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblErrMsg.AutoSize = true;
            this.lblErrMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblErrMsg.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(539, 149);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(0, 19);
            this.lblErrMsg.TabIndex = 153;
            this.lblErrMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFromSeq
            // 
            this.btnFromSeq.BackColor = System.Drawing.Color.Lime;
            this.btnFromSeq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFromSeq.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFromSeq.Location = new System.Drawing.Point(718, 5);
            this.btnFromSeq.Name = "btnFromSeq";
            this.btnFromSeq.Size = new System.Drawing.Size(141, 23);
            this.btnFromSeq.TabIndex = 146;
            this.btnFromSeq.Text = "FROM SEQUENCE";
            this.btnFromSeq.UseVisualStyleBackColor = false;
            this.btnFromSeq.Visible = false;
            this.btnFromSeq.Click += new System.EventHandler(this.btnFromSeq_Click);
            // 
            // frmWeighPlan
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1259, 665);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frmWeighPlan";
            this.Load += new System.EventHandler(this.frmWeighPlan_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmWeighPlan_KeyUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Prod_PlanItems)).EndInit();
            this.gp_ItemProcessing.ResumeLayout(false);
            this.gp_ItemProcessing.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gp_ItemProcessing;
        private System.Windows.Forms.Label lblErrMsg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dgv_Prod_PlanItems;
        private System.Windows.Forms.ComboBox cboPress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTyreSize;
        private System.Windows.Forms.Button btnTrack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboBrand;
        private System.Windows.Forms.Button btnFromSeq;
    }
}