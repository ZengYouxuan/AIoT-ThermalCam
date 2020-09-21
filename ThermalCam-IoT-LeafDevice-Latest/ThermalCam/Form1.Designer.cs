namespace ThermalCam
{
    partial class ThermalCam
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBoxIdentifiedFace = new System.Windows.Forms.PictureBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.pwd_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.user_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.port_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ip_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Play = new System.Windows.Forms.Button();
            this.pb_PlayBox = new System.Windows.Forms.PictureBox();
            this.pb_VisSceneImage = new System.Windows.Forms.PictureBox();
            this.listView_ManTempInfo = new System.Windows.Forms.ListView();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pb_ThermalSceneImage = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkBoxThermalChannel = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIdentifiedFace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_PlayBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_VisSceneImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ThermalSceneImage)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox1.Controls.Add(this.pictureBoxIdentifiedFace);
            this.groupBox1.Controls.Add(this.btn_Login);
            this.groupBox1.Controls.Add(this.pwd_textBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.user_textBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.port_textBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ip_textBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1560, 122);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device Login(设备登录)";
            // 
            // pictureBoxIdentifiedFace
            // 
            this.pictureBoxIdentifiedFace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxIdentifiedFace.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBoxIdentifiedFace.Location = new System.Drawing.Point(1448, 16);
            this.pictureBoxIdentifiedFace.Name = "pictureBoxIdentifiedFace";
            this.pictureBoxIdentifiedFace.Size = new System.Drawing.Size(107, 99);
            this.pictureBoxIdentifiedFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxIdentifiedFace.TabIndex = 21;
            this.pictureBoxIdentifiedFace.TabStop = false;
            this.pictureBoxIdentifiedFace.Visible = false;
            // 
            // btn_Login
            // 
            this.btn_Login.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Login.Location = new System.Drawing.Point(576, 29);
            this.btn_Login.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(140, 30);
            this.btn_Login.TabIndex = 12;
            this.btn_Login.Text = "Login(登录)";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // pwd_textBox
            // 
            this.pwd_textBox.Location = new System.Drawing.Point(401, 79);
            this.pwd_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.pwd_textBox.Name = "pwd_textBox";
            this.pwd_textBox.Size = new System.Drawing.Size(120, 35);
            this.pwd_textBox.TabIndex = 7;
            this.pwd_textBox.Text = "pass@word1";
            this.pwd_textBox.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(281, 82);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 25);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password(密码):";
            // 
            // user_textBox
            // 
            this.user_textBox.Location = new System.Drawing.Point(131, 79);
            this.user_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.user_textBox.Name = "user_textBox";
            this.user_textBox.Size = new System.Drawing.Size(120, 35);
            this.user_textBox.TabIndex = 5;
            this.user_textBox.Text = "mtc";
            this.user_textBox.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 82);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Name(用户名):";
            // 
            // port_textBox
            // 
            this.port_textBox.Location = new System.Drawing.Point(401, 35);
            this.port_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new System.Drawing.Size(120, 35);
            this.port_textBox.TabIndex = 3;
            this.port_textBox.Text = "37777";
            this.port_textBox.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(309, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port(端口):";
            // 
            // ip_textBox
            // 
            this.ip_textBox.Location = new System.Drawing.Point(131, 35);
            this.ip_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.ip_textBox.Name = "ip_textBox";
            this.ip_textBox.Size = new System.Drawing.Size(120, 35);
            this.ip_textBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP(设备IP):";
            // 
            // btn_Play
            // 
            this.btn_Play.Enabled = false;
            this.btn_Play.Location = new System.Drawing.Point(7, 28);
            this.btn_Play.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Play.Name = "btn_Play";
            this.btn_Play.Size = new System.Drawing.Size(140, 30);
            this.btn_Play.TabIndex = 11;
            this.btn_Play.Text = "Play(播放)";
            this.btn_Play.UseVisualStyleBackColor = true;
            this.btn_Play.Click += new System.EventHandler(this.btn_Play_Click);
            // 
            // pb_PlayBox
            // 
            this.pb_PlayBox.Location = new System.Drawing.Point(7, 65);
            this.pb_PlayBox.Name = "pb_PlayBox";
            this.pb_PlayBox.Size = new System.Drawing.Size(300, 186);
            this.pb_PlayBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_PlayBox.TabIndex = 7;
            this.pb_PlayBox.TabStop = false;
            // 
            // pb_VisSceneImage
            // 
            this.pb_VisSceneImage.Location = new System.Drawing.Point(6, 22);
            this.pb_VisSceneImage.Name = "pb_VisSceneImage";
            this.pb_VisSceneImage.Size = new System.Drawing.Size(831, 469);
            this.pb_VisSceneImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_VisSceneImage.TabIndex = 15;
            this.pb_VisSceneImage.TabStop = false;
            // 
            // listView_ManTempInfo
            // 
            this.listView_ManTempInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_ManTempInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
            this.listView_ManTempInfo.GridLines = true;
            this.listView_ManTempInfo.HideSelection = false;
            this.listView_ManTempInfo.Location = new System.Drawing.Point(1570, 278);
            this.listView_ManTempInfo.Name = "listView_ManTempInfo";
            this.listView_ManTempInfo.Size = new System.Drawing.Size(314, 716);
            this.listView_ManTempInfo.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.listView_ManTempInfo.TabIndex = 18;
            this.listView_ManTempInfo.UseCompatibleStateImageBehavior = false;
            this.listView_ManTempInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "ObjectID";
            this.columnHeader9.Width = 80;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "HighTemp";
            this.columnHeader10.Width = 100;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "TempUnit";
            this.columnHeader11.Width = 100;
            // 
            // pb_ThermalSceneImage
            // 
            this.pb_ThermalSceneImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_ThermalSceneImage.Location = new System.Drawing.Point(843, 22);
            this.pb_ThermalSceneImage.Name = "pb_ThermalSceneImage";
            this.pb_ThermalSceneImage.Size = new System.Drawing.Size(713, 542);
            this.pb_ThermalSceneImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_ThermalSceneImage.TabIndex = 20;
            this.pb_ThermalSceneImage.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.BackColor = System.Drawing.Color.Black;
            this.groupBox3.Controls.Add(this.pb_ThermalSceneImage);
            this.groupBox3.Controls.Add(this.pb_VisSceneImage);
            this.groupBox3.Location = new System.Drawing.Point(3, 143);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1561, 861);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Body Temperature Screening";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.groupBox4.Controls.Add(this.chkBoxThermalChannel);
            this.groupBox4.Controls.Add(this.btn_Play);
            this.groupBox4.Controls.Add(this.pb_PlayBox);
            this.groupBox4.Location = new System.Drawing.Point(1570, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(314, 260);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "RealPlay(实时监视)";
            // 
            // chkBoxThermalChannel
            // 
            this.chkBoxThermalChannel.AutoSize = true;
            this.chkBoxThermalChannel.Location = new System.Drawing.Point(155, 31);
            this.chkBoxThermalChannel.Name = "chkBoxThermalChannel";
            this.chkBoxThermalChannel.Size = new System.Drawing.Size(129, 29);
            this.chkBoxThermalChannel.TabIndex = 12;
            this.chkBoxThermalChannel.Text = "Thermal";
            this.chkBoxThermalChannel.UseVisualStyleBackColor = true;
            // 
            // ThermalCam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1896, 1016);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.listView_ManTempInfo);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ThermalCam";
            this.Text = "Leaf-Cam";
            this.Load += new System.EventHandler(this.ThermalCam_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIdentifiedFace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_PlayBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_VisSceneImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_ThermalSceneImage)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.Button btn_Play;
        private System.Windows.Forms.TextBox pwd_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox user_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ip_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pb_PlayBox;
        private System.Windows.Forms.PictureBox pb_VisSceneImage;
        private System.Windows.Forms.ListView listView_ManTempInfo;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.PictureBox pb_ThermalSceneImage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.PictureBox pictureBoxIdentifiedFace;
        private System.Windows.Forms.CheckBox chkBoxThermalChannel;
    }
}

