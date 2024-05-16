namespace GT
{
    partial class frmCustomerBarcode
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboCustomer = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboWorkorder = new System.Windows.Forms.ComboBox();
            this.dg_OrderItem = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dg_OrderItem)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(220, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(445, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "BARCODE FOR CUSTOMER REQUIRED";
            // 
            // cboCustomer
            // 
            this.cboCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCustomer.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCustomer.FormattingEnabled = true;
            this.cboCustomer.Location = new System.Drawing.Point(96, 59);
            this.cboCustomer.Name = "cboCustomer";
            this.cboCustomer.Size = new System.Drawing.Size(388, 24);
            this.cboCustomer.TabIndex = 1;
            this.cboCustomer.SelectedIndexChanged += new System.EventHandler(this.cboCustomer_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "CUSTOMER";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(537, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "WORK ORDER";
            // 
            // cboWorkorder
            // 
            this.cboWorkorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWorkorder.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboWorkorder.FormattingEnabled = true;
            this.cboWorkorder.Location = new System.Drawing.Point(643, 59);
            this.cboWorkorder.Name = "cboWorkorder";
            this.cboWorkorder.Size = new System.Drawing.Size(229, 24);
            this.cboWorkorder.TabIndex = 3;
            this.cboWorkorder.SelectedIndexChanged += new System.EventHandler(this.cboWorkorder_SelectedIndexChanged);
            // 
            // dg_OrderItem
            // 
            this.dg_OrderItem.AllowUserToAddRows = false;
            this.dg_OrderItem.AllowUserToDeleteRows = false;
            this.dg_OrderItem.AllowUserToResizeColumns = false;
            this.dg_OrderItem.AllowUserToResizeRows = false;
            this.dg_OrderItem.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dg_OrderItem.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Coral;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Khaki;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dg_OrderItem.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dg_OrderItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_OrderItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dg_OrderItem.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dg_OrderItem.GridColor = System.Drawing.Color.Silver;
            this.dg_OrderItem.Location = new System.Drawing.Point(6, 105);
            this.dg_OrderItem.MultiSelect = false;
            this.dg_OrderItem.Name = "dg_OrderItem";
            this.dg_OrderItem.ReadOnly = true;
            this.dg_OrderItem.RowHeadersVisible = false;
            this.dg_OrderItem.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dg_OrderItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dg_OrderItem.Size = new System.Drawing.Size(900, 290);
            this.dg_OrderItem.TabIndex = 6;
            this.dg_OrderItem.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_OrderItem_CellClick);
            // 
            // frmCustomerBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 403);
            this.ControlBox = false;
            this.Controls.Add(this.dg_OrderItem);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboWorkorder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboCustomer);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "frmCustomerBarcode";
            this.Load += new System.EventHandler(this.frmCustomerBarcode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_OrderItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCustomer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboWorkorder;
        private System.Windows.Forms.DataGridView dg_OrderItem;
    }
}