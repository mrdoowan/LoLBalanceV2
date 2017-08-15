namespace LoLBalancing
{
	partial class AddPlayer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPlayer));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_Tier = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Name = new System.Windows.Forms.TextBox();
            this.textBox_IGN = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Duo = new System.Windows.Forms.TextBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.groupBox_PrimaryRoles = new System.Windows.Forms.GroupBox();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox_SecondaryRoles = new System.Windows.Forms.GroupBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.radioButton12 = new System.Windows.Forms.RadioButton();
            this.numericUpDown_Div = new System.Windows.Forms.NumericUpDown();
            this.groupBox_PrimaryRoles.SuspendLayout();
            this.groupBox_SecondaryRoles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Div)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 18);
            this.label3.TabIndex = 3;
            this.label3.Text = "Rank:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 18);
            this.label4.TabIndex = 2;
            this.label4.Text = "Summoner IGN:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_Tier
            // 
            this.comboBox_Tier.DropDownHeight = 100;
            this.comboBox_Tier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Tier.FormattingEnabled = true;
            this.comboBox_Tier.IntegralHeight = false;
            this.comboBox_Tier.Items.AddRange(new object[] {
            "Unranked",
            "Bronze",
            "Silver",
            "Gold",
            "Platinum",
            "Diamond",
            "Master",
            "Challenger"});
            this.comboBox_Tier.Location = new System.Drawing.Point(115, 65);
            this.comboBox_Tier.Name = "comboBox_Tier";
            this.comboBox_Tier.Size = new System.Drawing.Size(162, 23);
            this.comboBox_Tier.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 18);
            this.label5.TabIndex = 6;
            this.label5.Text = "Roles:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_Name
            // 
            this.textBox_Name.Location = new System.Drawing.Point(114, 12);
            this.textBox_Name.Name = "textBox_Name";
            this.textBox_Name.Size = new System.Drawing.Size(226, 21);
            this.textBox_Name.TabIndex = 7;
            // 
            // textBox_IGN
            // 
            this.textBox_IGN.Location = new System.Drawing.Point(114, 39);
            this.textBox_IGN.Name = "textBox_IGN";
            this.textBox_IGN.Size = new System.Drawing.Size(226, 21);
            this.textBox_IGN.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 269);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 18);
            this.label6.TabIndex = 15;
            this.label6.Text = "Duo:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_Duo
            // 
            this.textBox_Duo.Location = new System.Drawing.Point(114, 271);
            this.textBox_Duo.Name = "textBox_Duo";
            this.textBox_Duo.Size = new System.Drawing.Size(226, 21);
            this.textBox_Duo.TabIndex = 16;
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(265, 298);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 17;
            this.button_OK.Text = "Add";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // groupBox_PrimaryRoles
            // 
            this.groupBox_PrimaryRoles.Controls.Add(this.radioButton6);
            this.groupBox_PrimaryRoles.Controls.Add(this.radioButton5);
            this.groupBox_PrimaryRoles.Controls.Add(this.radioButton3);
            this.groupBox_PrimaryRoles.Controls.Add(this.radioButton4);
            this.groupBox_PrimaryRoles.Controls.Add(this.radioButton2);
            this.groupBox_PrimaryRoles.Controls.Add(this.radioButton1);
            this.groupBox_PrimaryRoles.Location = new System.Drawing.Point(115, 94);
            this.groupBox_PrimaryRoles.Name = "groupBox_PrimaryRoles";
            this.groupBox_PrimaryRoles.Size = new System.Drawing.Size(109, 171);
            this.groupBox_PrimaryRoles.TabIndex = 20;
            this.groupBox_PrimaryRoles.TabStop = false;
            this.groupBox_PrimaryRoles.Text = "Primary";
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(6, 145);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(41, 19);
            this.radioButton6.TabIndex = 27;
            this.radioButton6.Text = "Fill";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(6, 120);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(68, 19);
            this.radioButton5.TabIndex = 26;
            this.radioButton5.Text = "Support";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 95);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(49, 19);
            this.radioButton3.TabIndex = 25;
            this.radioButton3.Text = "ADC";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(6, 70);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(46, 19);
            this.radioButton4.TabIndex = 24;
            this.radioButton4.Text = "Mid";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 45);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(62, 19);
            this.radioButton2.TabIndex = 23;
            this.radioButton2.Text = "Jungle";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(46, 19);
            this.radioButton1.TabIndex = 22;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Top";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox_SecondaryRoles
            // 
            this.groupBox_SecondaryRoles.Controls.Add(this.radioButton7);
            this.groupBox_SecondaryRoles.Controls.Add(this.radioButton8);
            this.groupBox_SecondaryRoles.Controls.Add(this.radioButton9);
            this.groupBox_SecondaryRoles.Controls.Add(this.radioButton10);
            this.groupBox_SecondaryRoles.Controls.Add(this.radioButton11);
            this.groupBox_SecondaryRoles.Controls.Add(this.radioButton12);
            this.groupBox_SecondaryRoles.Location = new System.Drawing.Point(230, 94);
            this.groupBox_SecondaryRoles.Name = "groupBox_SecondaryRoles";
            this.groupBox_SecondaryRoles.Size = new System.Drawing.Size(110, 171);
            this.groupBox_SecondaryRoles.TabIndex = 21;
            this.groupBox_SecondaryRoles.TabStop = false;
            this.groupBox_SecondaryRoles.Text = "Secondary";
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(6, 145);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(41, 19);
            this.radioButton7.TabIndex = 33;
            this.radioButton7.Text = "Fill";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(6, 120);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(68, 19);
            this.radioButton8.TabIndex = 32;
            this.radioButton8.Text = "Support";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(6, 95);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(49, 19);
            this.radioButton9.TabIndex = 31;
            this.radioButton9.Text = "ADC";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new System.Drawing.Point(6, 70);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(46, 19);
            this.radioButton10.TabIndex = 30;
            this.radioButton10.Text = "Mid";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // radioButton11
            // 
            this.radioButton11.AutoSize = true;
            this.radioButton11.Location = new System.Drawing.Point(6, 45);
            this.radioButton11.Name = "radioButton11";
            this.radioButton11.Size = new System.Drawing.Size(62, 19);
            this.radioButton11.TabIndex = 29;
            this.radioButton11.Text = "Jungle";
            this.radioButton11.UseVisualStyleBackColor = true;
            // 
            // radioButton12
            // 
            this.radioButton12.AutoSize = true;
            this.radioButton12.Checked = true;
            this.radioButton12.Location = new System.Drawing.Point(6, 20);
            this.radioButton12.Name = "radioButton12";
            this.radioButton12.Size = new System.Drawing.Size(46, 19);
            this.radioButton12.TabIndex = 28;
            this.radioButton12.TabStop = true;
            this.radioButton12.Text = "Top";
            this.radioButton12.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_Div
            // 
            this.numericUpDown_Div.Location = new System.Drawing.Point(283, 66);
            this.numericUpDown_Div.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_Div.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Div.Name = "numericUpDown_Div";
            this.numericUpDown_Div.Size = new System.Drawing.Size(57, 21);
            this.numericUpDown_Div.TabIndex = 22;
            this.numericUpDown_Div.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // AddPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 333);
            this.Controls.Add(this.numericUpDown_Div);
            this.Controls.Add(this.groupBox_SecondaryRoles);
            this.Controls.Add(this.groupBox_PrimaryRoles);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.textBox_Duo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_IGN);
            this.Controls.Add(this.textBox_Name);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox_Tier);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AddPlayer";
            this.Text = "Add Player";
            this.groupBox_PrimaryRoles.ResumeLayout(false);
            this.groupBox_PrimaryRoles.PerformLayout();
            this.groupBox_SecondaryRoles.ResumeLayout(false);
            this.groupBox_SecondaryRoles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Div)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox comboBox_Tier;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox_Name;
		private System.Windows.Forms.TextBox textBox_IGN;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textBox_Duo;
		private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.GroupBox groupBox_PrimaryRoles;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox_SecondaryRoles;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.RadioButton radioButton11;
        private System.Windows.Forms.RadioButton radioButton12;
        private System.Windows.Forms.NumericUpDown numericUpDown_Div;
    }
}