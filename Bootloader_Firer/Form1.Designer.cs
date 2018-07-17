namespace Bootloader_Firer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.btnFire = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtfileName = new System.Windows.Forms.TextBox();
            this.btnbrown = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbparity = new System.Windows.Forms.ComboBox();
            this.cbbdatalength = new System.Windows.Forms.ComboBox();
            this.cbbbaud = new System.Windows.Forms.ComboBox();
            this.cbbserialport = new System.Windows.Forms.ComboBox();
            this.timerScanSerialPort = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtstatus = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.timerSendPing = new System.Windows.Forms.Timer(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnFire
            // 
            this.btnFire.Enabled = false;
            this.btnFire.Font = new System.Drawing.Font("Ringfinger", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFire.ForeColor = System.Drawing.Color.Red;
            this.btnFire.Location = new System.Drawing.Point(17, 25);
            this.btnFire.Name = "btnFire";
            this.btnFire.Size = new System.Drawing.Size(124, 72);
            this.btnFire.TabIndex = 0;
            this.btnFire.Text = "Fire";
            this.btnFire.UseVisualStyleBackColor = true;
            this.btnFire.Click += new System.EventHandler(this.btnFire_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "STM32Program";
            this.openFileDialog1.Filter = "BIN files|*.bin";
            this.openFileDialog1.InitialDirectory = "@\"C:\\\"";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // txtfileName
            // 
            this.txtfileName.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtfileName.Location = new System.Drawing.Point(6, 25);
            this.txtfileName.Multiline = true;
            this.txtfileName.Name = "txtfileName";
            this.txtfileName.ReadOnly = true;
            this.txtfileName.Size = new System.Drawing.Size(537, 33);
            this.txtfileName.TabIndex = 2;
            this.txtfileName.DoubleClick += new System.EventHandler(this.txtfileName_DoubleClick);
            // 
            // btnbrown
            // 
            this.btnbrown.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnbrown.Location = new System.Drawing.Point(566, 25);
            this.btnbrown.Name = "btnbrown";
            this.btnbrown.Size = new System.Drawing.Size(124, 33);
            this.btnbrown.TabIndex = 3;
            this.btnbrown.Text = "Open";
            this.btnbrown.UseVisualStyleBackColor = true;
            this.btnbrown.Click += new System.EventHandler(this.btnbrown_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Ringfinger", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(127, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(467, 69);
            this.label1.TabIndex = 4;
            this.label1.Text = "The Boot Loader Firer";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(11, 569);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(711, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbbparity);
            this.groupBox1.Controls.Add(this.cbbdatalength);
            this.groupBox1.Controls.Add(this.cbbbaud);
            this.groupBox1.Controls.Add(this.cbbserialport);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 159);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(543, 113);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(272, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 19);
            this.label5.TabIndex = 1;
            this.label5.Text = "Parity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(272, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 19);
            this.label4.TabIndex = 1;
            this.label4.Text = "Data Length";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "Baud";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Serial Port";
            // 
            // cbbparity
            // 
            this.cbbparity.Enabled = false;
            this.cbbparity.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbparity.FormattingEnabled = true;
            this.cbbparity.Items.AddRange(new object[] {
            "None"});
            this.cbbparity.Location = new System.Drawing.Point(399, 64);
            this.cbbparity.Name = "cbbparity";
            this.cbbparity.Size = new System.Drawing.Size(121, 27);
            this.cbbparity.TabIndex = 0;
            // 
            // cbbdatalength
            // 
            this.cbbdatalength.Enabled = false;
            this.cbbdatalength.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbdatalength.FormattingEnabled = true;
            this.cbbdatalength.Items.AddRange(new object[] {
            "8 Bits"});
            this.cbbdatalength.Location = new System.Drawing.Point(399, 26);
            this.cbbdatalength.Name = "cbbdatalength";
            this.cbbdatalength.Size = new System.Drawing.Size(121, 27);
            this.cbbdatalength.TabIndex = 0;
            // 
            // cbbbaud
            // 
            this.cbbbaud.Enabled = false;
            this.cbbbaud.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbbaud.FormattingEnabled = true;
            this.cbbbaud.Items.AddRange(new object[] {
            "1000000"});
            this.cbbbaud.Location = new System.Drawing.Point(133, 64);
            this.cbbbaud.Name = "cbbbaud";
            this.cbbbaud.Size = new System.Drawing.Size(121, 27);
            this.cbbbaud.TabIndex = 0;
            // 
            // cbbserialport
            // 
            this.cbbserialport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbserialport.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbserialport.FormattingEnabled = true;
            this.cbbserialport.Location = new System.Drawing.Point(133, 26);
            this.cbbserialport.Name = "cbbserialport";
            this.cbbserialport.Size = new System.Drawing.Size(121, 27);
            this.cbbserialport.TabIndex = 0;
            this.cbbserialport.SelectedIndexChanged += new System.EventHandler(this.cbbserialport_SelectedIndexChanged);
            // 
            // timerScanSerialPort
            // 
            this.timerScanSerialPort.Enabled = true;
            this.timerScanSerialPort.Interval = 500;
            this.timerScanSerialPort.Tick += new System.EventHandler(this.timerScanSerialPort_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtfileName);
            this.groupBox2.Controls.Add(this.btnbrown);
            this.groupBox2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(710, 72);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Program File";
            // 
            // txtstatus
            // 
            this.txtstatus.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtstatus.ForeColor = System.Drawing.Color.Green;
            this.txtstatus.Location = new System.Drawing.Point(6, 25);
            this.txtstatus.Name = "txtstatus";
            this.txtstatus.ReadOnly = true;
            this.txtstatus.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtstatus.Size = new System.Drawing.Size(684, 241);
            this.txtstatus.TabIndex = 8;
            this.txtstatus.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtstatus);
            this.groupBox3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 278);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(710, 285);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            // 
            // timerSendPing
            // 
            this.timerSendPing.Interval = 50;
            this.timerSendPing.Tick += new System.EventHandler(this.timerSendPing_Tick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnFire);
            this.groupBox4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(561, 159);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(161, 113);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Program";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 598);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BootLoader Firer For STM32";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFire;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtfileName;
        private System.Windows.Forms.Button btnbrown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbparity;
        private System.Windows.Forms.ComboBox cbbdatalength;
        private System.Windows.Forms.ComboBox cbbbaud;
        private System.Windows.Forms.ComboBox cbbserialport;
        private System.Windows.Forms.Timer timerScanSerialPort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox txtstatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Timer timerSendPing;
        private System.Windows.Forms.GroupBox groupBox4;
        //private System.Windows.Forms.Timer timerSendPing;
    }
}

