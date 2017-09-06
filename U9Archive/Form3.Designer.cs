namespace U9Archive
{
    partial class Form3
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
            this.treeU9Menu = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.arDate = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvU9 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvU9)).BeginInit();
            this.SuspendLayout();
            // 
            // treeU9Menu
            // 
            this.treeU9Menu.CheckBoxes = true;
            this.treeU9Menu.Location = new System.Drawing.Point(12, 12);
            this.treeU9Menu.Name = "treeU9Menu";
            this.treeU9Menu.Size = new System.Drawing.Size(191, 637);
            this.treeU9Menu.TabIndex = 13;
            this.treeU9Menu.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeU9Menu_AfterCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(233, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "迁移截止时间：";
            // 
            // arDate
            // 
            this.arDate.Location = new System.Drawing.Point(328, 7);
            this.arDate.Name = "arDate";
            this.arDate.Size = new System.Drawing.Size(134, 21);
            this.arDate.TabIndex = 14;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(587, 7);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(134, 20);
            this.comboBox1.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(480, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "选择迁入历史库：";
            // 
            // dgvU9
            // 
            this.dgvU9.AllowUserToAddRows = false;
            this.dgvU9.AllowUserToDeleteRows = false;
            this.dgvU9.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvU9.Location = new System.Drawing.Point(235, 74);
            this.dgvU9.Name = "dgvU9";
            this.dgvU9.ReadOnly = true;
            this.dgvU9.RowHeadersVisible = false;
            this.dgvU9.RowTemplate.Height = 23;
            this.dgvU9.Size = new System.Drawing.Size(740, 575);
            this.dgvU9.TabIndex = 21;
            this.dgvU9.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvU9_CellClick);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 661);
            this.Controls.Add(this.dgvU9);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.arDate);
            this.Controls.Add(this.treeU9Menu);
            this.Name = "Form3";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.dgvU9)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeU9Menu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker arDate;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvU9;
    }
}