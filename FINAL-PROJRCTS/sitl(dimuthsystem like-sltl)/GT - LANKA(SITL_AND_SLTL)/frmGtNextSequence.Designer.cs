namespace GT
{
    partial class frmGtNextSequence
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlSequence = new System.Windows.Forms.Panel();
            this.cboSeqUnit = new System.Windows.Forms.ComboBox();
            this.btnSequenceClose = new System.Windows.Forms.Button();
            this.dgvNextSequence = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlSequence.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNextSequence)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSequence
            // 
            this.pnlSequence.BackColor = System.Drawing.Color.Blue;
            this.pnlSequence.Controls.Add(this.cboSeqUnit);
            this.pnlSequence.Controls.Add(this.btnSequenceClose);
            this.pnlSequence.Controls.Add(this.dgvNextSequence);
            this.pnlSequence.Controls.Add(this.label3);
            this.pnlSequence.Location = new System.Drawing.Point(0, 0);
            this.pnlSequence.Name = "pnlSequence";
            this.pnlSequence.Size = new System.Drawing.Size(1034, 442);
            this.pnlSequence.TabIndex = 164;
            // 
            // cboSeqUnit
            // 
            this.cboSeqUnit.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSeqUnit.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSeqUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSeqUnit.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSeqUnit.FormattingEnabled = true;
            this.cboSeqUnit.Location = new System.Drawing.Point(178, 3);
            this.cboSeqUnit.Name = "cboSeqUnit";
            this.cboSeqUnit.Size = new System.Drawing.Size(213, 25);
            this.cboSeqUnit.TabIndex = 164;
            this.cboSeqUnit.SelectedIndexChanged += new System.EventHandler(this.cboSeqUnit_SelectedIndexChanged);
            // 
            // btnSequenceClose
            // 
            this.btnSequenceClose.Location = new System.Drawing.Point(948, 3);
            this.btnSequenceClose.Name = "btnSequenceClose";
            this.btnSequenceClose.Size = new System.Drawing.Size(75, 23);
            this.btnSequenceClose.TabIndex = 163;
            this.btnSequenceClose.Text = "CLOSE";
            this.btnSequenceClose.UseVisualStyleBackColor = true;
            this.btnSequenceClose.Click += new System.EventHandler(this.btnSequenceClose_Click);
            // 
            // dgvNextSequence
            // 
            this.dgvNextSequence.AllowUserToAddRows = false;
            this.dgvNextSequence.AllowUserToDeleteRows = false;
            this.dgvNextSequence.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNextSequence.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvNextSequence.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvNextSequence.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNextSequence.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvNextSequence.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNextSequence.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNextSequence.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvNextSequence.EnableHeadersVisualStyles = false;
            this.dgvNextSequence.GridColor = System.Drawing.Color.Black;
            this.dgvNextSequence.Location = new System.Drawing.Point(13, 30);
            this.dgvNextSequence.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvNextSequence.MultiSelect = false;
            this.dgvNextSequence.Name = "dgvNextSequence";
            this.dgvNextSequence.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNextSequence.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvNextSequence.RowHeadersVisible = false;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNextSequence.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvNextSequence.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvNextSequence.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvNextSequence.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvNextSequence.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            this.dgvNextSequence.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvNextSequence.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNextSequence.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNextSequence.Size = new System.Drawing.Size(1011, 402);
            this.dgvNextSequence.TabIndex = 162;
            this.dgvNextSequence.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNextSequence_CellContentClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(17, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "SEQUENCE UNIT";
            // 
            // frmGtNextSequence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 443);
            this.ControlBox = false;
            this.Controls.Add(this.pnlSequence);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmGtNextSequence";
            this.Load += new System.EventHandler(this.frmGtNextSequence_Load);
            this.pnlSequence.ResumeLayout(false);
            this.pnlSequence.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNextSequence)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSequence;
        private System.Windows.Forms.Button btnSequenceClose;
        private System.Windows.Forms.DataGridView dgvNextSequence;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboSeqUnit;
    }
}