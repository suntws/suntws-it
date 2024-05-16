namespace GT
{
    partial class frmConcessionPlan
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.chkSelAll = new System.Windows.Forms.CheckBox();
            this.lblType = new System.Windows.Forms.Label();
            this.grpPanel = new System.Windows.Forms.Panel();
            this.grdConDetails = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbConcCode = new System.Windows.Forms.ComboBox();
            this.lblConcCode = new System.Windows.Forms.Label();
            this.lblErr = new System.Windows.Forms.Label();
            this.txtCompLimit = new System.Windows.Forms.TextBox();
            this.cmbCompCode = new System.Windows.Forms.ComboBox();
            this.cmbCateg = new System.Windows.Forms.ComboBox();
            this.lblConslimit = new System.Windows.Forms.Label();
            this.lblCompCode = new System.Windows.Forms.Label();
            this.lblCateg = new System.Windows.Forms.Label();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTtile = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdConDetails)).BeginInit();
            this.pnlTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.pnlTitle);
            this.panel1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(898, 484);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.pnlGrid);
            this.panel2.Controls.Add(this.grdConDetails);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.cmbConcCode);
            this.panel2.Controls.Add(this.lblConcCode);
            this.panel2.Controls.Add(this.lblErr);
            this.panel2.Controls.Add(this.txtCompLimit);
            this.panel2.Controls.Add(this.cmbCompCode);
            this.panel2.Controls.Add(this.cmbCateg);
            this.panel2.Controls.Add(this.lblConslimit);
            this.panel2.Controls.Add(this.lblCompCode);
            this.panel2.Controls.Add(this.lblCateg);
            this.panel2.Location = new System.Drawing.Point(2, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(893, 446);
            this.panel2.TabIndex = 1;
            // 
            // pnlGrid
            // 
            this.pnlGrid.AutoSize = true;
            this.pnlGrid.BackColor = System.Drawing.Color.LightPink;
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.chkSelAll);
            this.pnlGrid.Controls.Add(this.lblType);
            this.pnlGrid.Controls.Add(this.grpPanel);
            this.pnlGrid.Location = new System.Drawing.Point(-1, 81);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(893, 171);
            this.pnlGrid.TabIndex = 2;
            // 
            // chkSelAll
            // 
            this.chkSelAll.AutoSize = true;
            this.chkSelAll.Location = new System.Drawing.Point(206, 8);
            this.chkSelAll.Name = "chkSelAll";
            this.chkSelAll.Size = new System.Drawing.Size(50, 20);
            this.chkSelAll.TabIndex = 3;
            this.chkSelAll.Text = "ALL";
            this.chkSelAll.UseVisualStyleBackColor = true;
            this.chkSelAll.Click += new System.EventHandler(this.chkSelAll_Click);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(6, 9);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(177, 16);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "APPLICABLE TYRE TYPE";
            // 
            // grpPanel
            // 
            this.grpPanel.AutoScroll = true;
            this.grpPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.grpPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grpPanel.Location = new System.Drawing.Point(3, 30);
            this.grpPanel.Name = "grpPanel";
            this.grpPanel.Size = new System.Drawing.Size(883, 132);
            this.grpPanel.TabIndex = 1;
            // 
            // grdConDetails
            // 
            this.grdConDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdConDetails.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.MediumAquamarine;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdConDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdConDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdConDetails.Location = new System.Drawing.Point(2, 253);
            this.grdConDetails.Name = "grdConDetails";
            this.grdConDetails.ReadOnly = true;
            this.grdConDetails.RowHeadersVisible = false;
            this.grdConDetails.Size = new System.Drawing.Size(887, 150);
            this.grdConDetails.TabIndex = 0;
            this.grdConDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdConDetails_CellClick);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.SystemColors.Window;
            this.btnDelete.Location = new System.Drawing.Point(626, 409);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(98, 32);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "DELETE";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Thistle;
            this.btnClear.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(795, 409);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(93, 32);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightGreen;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(449, 409);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 32);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbConcCode
            // 
            this.cmbConcCode.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbConcCode.FormattingEnabled = true;
            this.cmbConcCode.Location = new System.Drawing.Point(678, 5);
            this.cmbConcCode.Name = "cmbConcCode";
            this.cmbConcCode.Size = new System.Drawing.Size(184, 26);
            this.cmbConcCode.TabIndex = 6;
            this.cmbConcCode.SelectedIndexChanged += new System.EventHandler(this.cmbConcCode_SelectedIndexChanged);
            // 
            // lblConcCode
            // 
            this.lblConcCode.AutoSize = true;
            this.lblConcCode.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConcCode.Location = new System.Drawing.Point(468, 9);
            this.lblConcCode.Name = "lblConcCode";
            this.lblConcCode.Size = new System.Drawing.Size(208, 18);
            this.lblConcCode.TabIndex = 2;
            this.lblConcCode.Text = "CONCESSION COMP CODE";
            // 
            // lblErr
            // 
            this.lblErr.AutoSize = true;
            this.lblErr.ForeColor = System.Drawing.Color.Red;
            this.lblErr.Location = new System.Drawing.Point(6, 421);
            this.lblErr.Name = "lblErr";
            this.lblErr.Size = new System.Drawing.Size(40, 16);
            this.lblErr.TabIndex = 8;
            this.lblErr.Text = "lblErr";
            // 
            // txtCompLimit
            // 
            this.txtCompLimit.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCompLimit.Location = new System.Drawing.Point(678, 49);
            this.txtCompLimit.MaxLength = 8;
            this.txtCompLimit.Name = "txtCompLimit";
            this.txtCompLimit.Size = new System.Drawing.Size(184, 26);
            this.txtCompLimit.TabIndex = 7;
            this.txtCompLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCompLimit_KeyPress);
            this.txtCompLimit.Leave += new System.EventHandler(this.txtCompLimit_Leave);
            // 
            // cmbCompCode
            // 
            this.cmbCompCode.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCompCode.FormattingEnabled = true;
            this.cmbCompCode.Location = new System.Drawing.Point(183, 49);
            this.cmbCompCode.Name = "cmbCompCode";
            this.cmbCompCode.Size = new System.Drawing.Size(184, 26);
            this.cmbCompCode.TabIndex = 5;
            this.cmbCompCode.SelectedIndexChanged += new System.EventHandler(this.cmbCompCode_SelectedIndexChanged);
            // 
            // cmbCateg
            // 
            this.cmbCateg.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCateg.DisplayMember = "CHOOSE";
            this.cmbCateg.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCateg.FormattingEnabled = true;
            this.cmbCateg.Location = new System.Drawing.Point(183, 5);
            this.cmbCateg.Name = "cmbCateg";
            this.cmbCateg.Size = new System.Drawing.Size(184, 26);
            this.cmbCateg.TabIndex = 4;
            this.cmbCateg.SelectedIndexChanged += new System.EventHandler(this.cmbCateg_SelectedIndexChanged);
            // 
            // lblConslimit
            // 
            this.lblConslimit.AutoSize = true;
            this.lblConslimit.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConslimit.Location = new System.Drawing.Point(468, 53);
            this.lblConslimit.Name = "lblConslimit";
            this.lblConslimit.Size = new System.Drawing.Size(155, 18);
            this.lblConslimit.TabIndex = 3;
            this.lblConslimit.Text = "CONCESSION LIMIT";
            // 
            // lblCompCode
            // 
            this.lblCompCode.AutoSize = true;
            this.lblCompCode.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompCode.Location = new System.Drawing.Point(13, 53);
            this.lblCompCode.Name = "lblCompCode";
            this.lblCompCode.Size = new System.Drawing.Size(167, 18);
            this.lblCompCode.TabIndex = 1;
            this.lblCompCode.Text = "MASTER COMP CODE";
            // 
            // lblCateg
            // 
            this.lblCateg.AutoSize = true;
            this.lblCateg.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCateg.Location = new System.Drawing.Point(13, 9);
            this.lblCateg.Name = "lblCateg";
            this.lblCateg.Size = new System.Drawing.Size(89, 18);
            this.lblCateg.TabIndex = 0;
            this.lblCateg.Text = "CATEGORY";
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.Color.NavajoWhite;
            this.pnlTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTitle.Controls.Add(this.lblTtile);
            this.pnlTitle.Location = new System.Drawing.Point(2, 2);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(893, 32);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblTtile
            // 
            this.lblTtile.AutoSize = true;
            this.lblTtile.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTtile.Location = new System.Drawing.Point(324, 5);
            this.lblTtile.Name = "lblTtile";
            this.lblTtile.Size = new System.Drawing.Size(248, 18);
            this.lblTtile.TabIndex = 0;
            this.lblTtile.Text = "COMPOUND CONCESSION PLAN";
            // 
            // frmConcessionPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 488);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmConcessionPlan";
            this.Load += new System.EventHandler(this.frmConcessionPlan_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            this.pnlGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdConDetails)).EndInit();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblCateg;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTtile;
        private System.Windows.Forms.TextBox txtCompLimit;
        private System.Windows.Forms.ComboBox cmbConcCode;
        private System.Windows.Forms.ComboBox cmbCompCode;
        private System.Windows.Forms.ComboBox cmbCateg;
        private System.Windows.Forms.Label lblConslimit;
        private System.Windows.Forms.Label lblConcCode;
        private System.Windows.Forms.Label lblCompCode;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblErr;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView grdConDetails;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel grpPanel;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.CheckBox chkSelAll;
    }
}