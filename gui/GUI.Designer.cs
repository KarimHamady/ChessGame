namespace ChessGame
{
    partial class GUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            trackBar1 = new TrackBar();
            label1 = new Label();
            groupBox3 = new GroupBox();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            groupBox2 = new GroupBox();
            radioButton4 = new RadioButton();
            radioButton3 = new RadioButton();
            panel1 = new Panel();
            panel2 = new Panel();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // trackBar1
            // 
            trackBar1.LargeChange = 100;
            trackBar1.Location = new Point(15, 464);
            trackBar1.Maximum = 2000;
            trackBar1.Minimum = 1000;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(153, 45);
            trackBar1.SmallChange = 50;
            trackBar1.TabIndex = 2;
            trackBar1.TickFrequency = 100;
            trackBar1.Value = 1000;
            trackBar1.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(67, 442);
            label1.Name = "label1";
            label1.Size = new Size(46, 19);
            label1.TabIndex = 8;
            label1.Text = "Level";
            label1.Visible = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(radioButton2);
            groupBox3.Controls.Add(radioButton1);
            groupBox3.ForeColor = SystemColors.ButtonFace;
            groupBox3.Location = new Point(42, 224);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(98, 84);
            groupBox3.TabIndex = 7;
            groupBox3.TabStop = false;
            groupBox3.Text = "Play as";
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(17, 55);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(64, 23);
            radioButton2.TabIndex = 4;
            radioButton2.Text = "Black";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(17, 26);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(68, 23);
            radioButton1.TabIndex = 3;
            radioButton1.TabStop = true;
            radioButton1.Text = "White";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(radioButton4);
            groupBox2.Controls.Add(radioButton3);
            groupBox2.ForeColor = SystemColors.ButtonFace;
            groupBox2.Location = new Point(42, 337);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(98, 85);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Play vs";
            // 
            // radioButton4
            // 
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(12, 54);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(42, 23);
            radioButton4.TabIndex = 1;
            radioButton4.Text = "AI";
            radioButton4.UseVisualStyleBackColor = true;
            radioButton4.CheckedChanged += radioButton4_CheckedChanged;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Checked = true;
            radioButton3.Location = new Point(12, 25);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(71, 23);
            radioButton3.TabIndex = 0;
            radioButton3.TabStop = true;
            radioButton3.Text = "Player";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Location = new Point(1, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(586, 586);
            panel1.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ActiveCaptionText;
            panel2.Controls.Add(button1);
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(trackBar1);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(groupBox3);
            panel2.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point);
            panel2.ForeColor = SystemColors.ButtonFace;
            panel2.Location = new Point(584, 1);
            panel2.Name = "panel2";
            panel2.Size = new Size(181, 586);
            panel2.TabIndex = 0;
            // 
            // button1
            // 
            button1.BackColor = SystemColors.ActiveCaptionText;
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(42, 515);
            button1.Name = "button1";
            button1.Size = new Size(110, 41);
            button1.TabIndex = 0;
            button1.Text = "Play";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // GUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(763, 586);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "GUI";
            Text = "Chess";
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TrackBar trackBar1;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Label label1;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private RadioButton radioButton4;
        private RadioButton radioButton3;
        private Panel panel1;
        private Panel panel2;
        private Button button1;
    }
}