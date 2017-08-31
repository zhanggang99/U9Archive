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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("工作流");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("个性化模板");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("日志数据");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("平台数据", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("标准采购");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("标准销售");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("生产管理");
            this.treeU9Menu = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.arDate = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnComputer = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Columns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.迁移 = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeU9Menu
            // 
            this.treeU9Menu.CheckBoxes = true;
            this.treeU9Menu.Location = new System.Drawing.Point(12, 12);
            this.treeU9Menu.Name = "treeU9Menu";
            treeNode1.Name = "节点4";
            treeNode1.Text = "工作流";
            treeNode2.Name = "节点5";
            treeNode2.Text = "个性化模板";
            treeNode3.Name = "节点6";
            treeNode3.Text = "日志数据";
            treeNode4.Name = "节点0";
            treeNode4.Text = "平台数据";
            treeNode5.Name = "节点1";
            treeNode5.Text = "标准采购";
            treeNode6.Name = "节点2";
            treeNode6.Text = "标准销售";
            treeNode7.Name = "节点3";
            treeNode7.Text = "生产管理";
            this.treeU9Menu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7});
            this.treeU9Menu.Size = new System.Drawing.Size(191, 637);
            this.treeU9Menu.TabIndex = 13;
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
            // btnComputer
            // 
            this.btnComputer.Location = new System.Drawing.Point(233, 45);
            this.btnComputer.Name = "btnComputer";
            this.btnComputer.Size = new System.Drawing.Size(75, 23);
            this.btnComputer.TabIndex = 20;
            this.btnComputer.Text = "迁移评估";
            this.btnComputer.UseVisualStyleBackColor = true;
            this.btnComputer.Click += new System.EventHandler(this.btnComputer_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(458, 45);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 19;
            this.btnStop.Tag = "";
            this.btnStop.Text = "停止迁移";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(349, 45);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 18;
            this.btnStart.Text = "开始迁移";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Columns,
            this.Column5,
            this.迁移});
            this.dataGridView1.Location = new System.Drawing.Point(235, 74);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(770, 575);
            this.dataGridView1.TabIndex = 21;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "模块名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "评估迁移量";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "最早单据日期";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "已迁移数据";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Columns
            // 
            this.Columns.HeaderText = "迁移状态";
            this.Columns.Name = "Columns";
            this.Columns.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "评估";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // 迁移
            // 
            this.迁移.HeaderText = "迁移";
            this.迁移.Name = "迁移";
            this.迁移.ReadOnly = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 661);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnComputer);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.arDate);
            this.Controls.Add(this.treeU9Menu);
            this.Name = "Form3";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeU9Menu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker arDate;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnComputer;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Columns;
        private System.Windows.Forms.DataGridViewButtonColumn Column5;
        private System.Windows.Forms.DataGridViewButtonColumn 迁移;
    }
}