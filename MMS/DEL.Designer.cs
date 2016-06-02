namespace MMS
{
    partial class DEL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DEL));
            this.year = new System.Windows.Forms.NumericUpDown();
            this.month = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_Pic = new System.Windows.Forms.TextBox();
            this.tb_Vid = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.shrink = new System.Windows.Forms.CheckBox();
            this.tb_Tables = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.month2 = new System.Windows.Forms.NumericUpDown();
            this.year2 = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.year)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.month)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.month2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.year2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // year
            // 
            this.year.Location = new System.Drawing.Point(155, 20);
            this.year.Maximum = new decimal(new int[] {
            2099,
            0,
            0,
            0});
            this.year.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.year.Name = "year";
            this.year.Size = new System.Drawing.Size(54, 21);
            this.year.TabIndex = 3;
            this.year.Value = new decimal(new int[] {
            2013,
            0,
            0,
            0});
            // 
            // month
            // 
            this.month.Location = new System.Drawing.Point(235, 20);
            this.month.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.month.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.month.Name = "month";
            this.month.Size = new System.Drawing.Size(47, 21);
            this.month.TabIndex = 4;
            this.month.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(211, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "年";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(282, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "月";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(310, 264);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 43);
            this.button1.TabIndex = 7;
            this.button1.Text = "确定删除";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "开始时间：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "图片目录：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 223);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "视频目录：";
            // 
            // tb_Pic
            // 
            this.tb_Pic.Location = new System.Drawing.Point(102, 183);
            this.tb_Pic.Name = "tb_Pic";
            this.tb_Pic.Size = new System.Drawing.Size(208, 21);
            this.tb_Pic.TabIndex = 11;
            // 
            // tb_Vid
            // 
            this.tb_Vid.Location = new System.Drawing.Point(102, 218);
            this.tb_Vid.Name = "tb_Vid";
            this.tb_Vid.Size = new System.Drawing.Size(208, 21);
            this.tb_Vid.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(310, 182);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "浏览...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(310, 217);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "浏览...";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // shrink
            // 
            this.shrink.AutoSize = true;
            this.shrink.Checked = true;
            this.shrink.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shrink.Location = new System.Drawing.Point(35, 291);
            this.shrink.Name = "shrink";
            this.shrink.Size = new System.Drawing.Size(120, 16);
            this.shrink.TabIndex = 15;
            this.shrink.Text = "删除后收缩数据库";
            this.shrink.UseVisualStyleBackColor = true;
            // 
            // tb_Tables
            // 
            this.tb_Tables.Location = new System.Drawing.Point(102, 120);
            this.tb_Tables.Multiline = true;
            this.tb_Tables.Name = "tb_Tables";
            this.tb_Tables.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Tables.Size = new System.Drawing.Size(283, 42);
            this.tb_Tables.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "数据表名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "结束时间：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(282, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 22;
            this.label9.Text = "月";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(211, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 21;
            this.label10.Text = "年";
            // 
            // month2
            // 
            this.month2.Location = new System.Drawing.Point(235, 56);
            this.month2.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.month2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.month2.Name = "month2";
            this.month2.Size = new System.Drawing.Size(47, 21);
            this.month2.TabIndex = 20;
            this.month2.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // year2
            // 
            this.year2.Location = new System.Drawing.Point(155, 56);
            this.year2.Maximum = new decimal(new int[] {
            2099,
            0,
            0,
            0});
            this.year2.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.year2.Name = "year2";
            this.year2.Size = new System.Drawing.Size(54, 21);
            this.year2.TabIndex = 19;
            this.year2.Value = new decimal(new int[] {
            2013,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.year);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.month);
            this.groupBox1.Controls.Add(this.month2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.year2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(35, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 100);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "时间范围";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(102, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 25;
            this.label1.Text = "多个表名之间用空格分隔";
            // 
            // DEL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 335);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tb_Tables);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.shrink);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tb_Vid);
            this.Controls.Add(this.tb_Pic);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DEL";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "删除数据、图片和视频";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.year)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.month)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.month2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.year2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown year;
        private System.Windows.Forms.NumericUpDown month;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_Pic;
        private System.Windows.Forms.TextBox tb_Vid;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox shrink;
        private System.Windows.Forms.TextBox tb_Tables;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown month2;
        private System.Windows.Forms.NumericUpDown year2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
    }
}