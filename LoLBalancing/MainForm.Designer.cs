namespace LoLBalancing
{
	partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.button_newTemplate = new System.Windows.Forms.Button();
            this.label_Total = new System.Windows.Forms.Label();
            this.button_SavePlayers = new System.Windows.Forms.Button();
            this.button_LoadPlayers = new System.Windows.Forms.Button();
            this.dataGridView_Players = new System.Windows.Forms.DataGridView();
            this.X_Col = new System.Windows.Forms.DataGridViewButtonColumn();
            this.name_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.summ_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tier_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.div_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.primary_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.secondary_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.duo_Col = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_AddPlayer = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.richTextBox_Console = new System.Windows.Forms.RichTextBox();
            this.button_Balance = new System.Windows.Forms.Button();
            this.groupBox_Setting = new System.Windows.Forms.GroupBox();
            this.checkBox_BestOutput = new System.Windows.Forms.CheckBox();
            this.numeric_Secondary = new System.Windows.Forms.NumericUpDown();
            this.numeric_AutoFill = new System.Windows.Forms.NumericUpDown();
            this.button_CustomPoints = new System.Windows.Forms.Button();
            this.checkBox_TrueRandom = new System.Windows.Forms.CheckBox();
            this.checkBox_WriteRange = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numeric_DesThrshRange = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numeric_MaxChecks = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numeric_StartSeed = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_Version = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Players)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox_Setting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Secondary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_AutoFill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_DesThrshRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_MaxChecks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_StartSeed)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(549, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 100);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(118, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(425, 100);
            this.label1.TabIndex = 2;
            this.label1.Text = "League of Legends\r\nTournament Balancer";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 150);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(637, 399);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.button_newTemplate);
            this.tabPage1.Controls.Add(this.label_Total);
            this.tabPage1.Controls.Add(this.button_SavePlayers);
            this.tabPage1.Controls.Add(this.button_LoadPlayers);
            this.tabPage1.Controls.Add(this.dataGridView_Players);
            this.tabPage1.Controls.Add(this.button_AddPlayer);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(629, 373);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Player Roster";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.ForeColor = System.Drawing.Color.OrangeRed;
            this.label3.Location = new System.Drawing.Point(6, 348);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(617, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "NOTE: When loading, the First row of the Excel sheet will be ignored.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_newTemplate
            // 
            this.button_newTemplate.Location = new System.Drawing.Point(264, 6);
            this.button_newTemplate.Name = "button_newTemplate";
            this.button_newTemplate.Size = new System.Drawing.Size(88, 23);
            this.button_newTemplate.TabIndex = 3;
            this.button_newTemplate.Text = "New Template";
            this.button_newTemplate.UseVisualStyleBackColor = true;
            this.button_newTemplate.Click += new System.EventHandler(this.button_newTemplate_Click);
            // 
            // label_Total
            // 
            this.label_Total.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Total.ForeColor = System.Drawing.Color.Teal;
            this.label_Total.Location = new System.Drawing.Point(358, 6);
            this.label_Total.Name = "label_Total";
            this.label_Total.Size = new System.Drawing.Size(265, 23);
            this.label_Total.TabIndex = 5;
            this.label_Total.Text = "Total Players: 0";
            this.label_Total.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_SavePlayers
            // 
            this.button_SavePlayers.Location = new System.Drawing.Point(178, 6);
            this.button_SavePlayers.Name = "button_SavePlayers";
            this.button_SavePlayers.Size = new System.Drawing.Size(80, 23);
            this.button_SavePlayers.TabIndex = 2;
            this.button_SavePlayers.Text = "Save Players";
            this.button_SavePlayers.UseVisualStyleBackColor = true;
            this.button_SavePlayers.Click += new System.EventHandler(this.button_SavePlayers_Click);
            // 
            // button_LoadPlayers
            // 
            this.button_LoadPlayers.Location = new System.Drawing.Point(92, 6);
            this.button_LoadPlayers.Name = "button_LoadPlayers";
            this.button_LoadPlayers.Size = new System.Drawing.Size(80, 23);
            this.button_LoadPlayers.TabIndex = 1;
            this.button_LoadPlayers.Text = "Load Players";
            this.button_LoadPlayers.UseVisualStyleBackColor = true;
            this.button_LoadPlayers.Click += new System.EventHandler(this.button_LoadPlayers_Click);
            // 
            // dataGridView_Players
            // 
            this.dataGridView_Players.AllowUserToAddRows = false;
            this.dataGridView_Players.AllowUserToDeleteRows = false;
            this.dataGridView_Players.AllowUserToResizeRows = false;
            this.dataGridView_Players.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_Players.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Players.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_Players.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Players.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.X_Col,
            this.name_Col,
            this.summ_Col,
            this.tier_Col,
            this.div_Col,
            this.primary_Col,
            this.secondary_Col,
            this.duo_Col});
            this.dataGridView_Players.Location = new System.Drawing.Point(6, 35);
            this.dataGridView_Players.MultiSelect = false;
            this.dataGridView_Players.Name = "dataGridView_Players";
            this.dataGridView_Players.RowHeadersVisible = false;
            this.dataGridView_Players.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Players.Size = new System.Drawing.Size(617, 308);
            this.dataGridView_Players.TabIndex = 1;
            this.dataGridView_Players.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Players_CellContentClick);
            this.dataGridView_Players.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Players_CellDoubleClick);
            // 
            // X_Col
            // 
            this.X_Col.HeaderText = "X";
            this.X_Col.Name = "X_Col";
            this.X_Col.ReadOnly = true;
            this.X_Col.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.X_Col.Width = 25;
            // 
            // name_Col
            // 
            this.name_Col.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.name_Col.DefaultCellStyle = dataGridViewCellStyle4;
            this.name_Col.HeaderText = "Name";
            this.name_Col.MinimumWidth = 120;
            this.name_Col.Name = "name_Col";
            this.name_Col.ReadOnly = true;
            this.name_Col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.name_Col.Width = 120;
            // 
            // summ_Col
            // 
            this.summ_Col.HeaderText = "Summoner";
            this.summ_Col.MinimumWidth = 130;
            this.summ_Col.Name = "summ_Col";
            this.summ_Col.ReadOnly = true;
            this.summ_Col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.summ_Col.Width = 130;
            // 
            // tier_Col
            // 
            this.tier_Col.HeaderText = "Tier";
            this.tier_Col.MinimumWidth = 60;
            this.tier_Col.Name = "tier_Col";
            this.tier_Col.ReadOnly = true;
            this.tier_Col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.tier_Col.Width = 60;
            // 
            // div_Col
            // 
            this.div_Col.HeaderText = "Div";
            this.div_Col.MinimumWidth = 30;
            this.div_Col.Name = "div_Col";
            this.div_Col.ReadOnly = true;
            this.div_Col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.div_Col.Width = 30;
            // 
            // primary_Col
            // 
            this.primary_Col.HeaderText = "Primary";
            this.primary_Col.MinimumWidth = 50;
            this.primary_Col.Name = "primary_Col";
            this.primary_Col.ReadOnly = true;
            this.primary_Col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.primary_Col.Width = 50;
            // 
            // secondary_Col
            // 
            this.secondary_Col.HeaderText = "Second";
            this.secondary_Col.MinimumWidth = 50;
            this.secondary_Col.Name = "secondary_Col";
            this.secondary_Col.ReadOnly = true;
            this.secondary_Col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.secondary_Col.Width = 50;
            // 
            // duo_Col
            // 
            this.duo_Col.HeaderText = "Duo";
            this.duo_Col.MinimumWidth = 148;
            this.duo_Col.Name = "duo_Col";
            this.duo_Col.ReadOnly = true;
            this.duo_Col.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.duo_Col.Width = 148;
            // 
            // button_AddPlayer
            // 
            this.button_AddPlayer.Location = new System.Drawing.Point(6, 6);
            this.button_AddPlayer.Name = "button_AddPlayer";
            this.button_AddPlayer.Size = new System.Drawing.Size(80, 23);
            this.button_AddPlayer.TabIndex = 0;
            this.button_AddPlayer.Text = "Add Player";
            this.button_AddPlayer.UseVisualStyleBackColor = true;
            this.button_AddPlayer.Click += new System.EventHandler(this.button_AddPlayer_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.richTextBox_Console);
            this.tabPage2.Controls.Add(this.button_Balance);
            this.tabPage2.Controls.Add(this.groupBox_Setting);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(629, 373);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Balancing";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(486, 22);
            this.label8.TabIndex = 20;
            this.label8.Text = "Console Log";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // richTextBox_Console
            // 
            this.richTextBox_Console.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_Console.BackColor = System.Drawing.SystemColors.ControlLight;
            this.richTextBox_Console.Location = new System.Drawing.Point(6, 221);
            this.richTextBox_Console.Name = "richTextBox_Console";
            this.richTextBox_Console.ReadOnly = true;
            this.richTextBox_Console.Size = new System.Drawing.Size(617, 146);
            this.richTextBox_Console.TabIndex = 1;
            this.richTextBox_Console.Text = "";
            this.richTextBox_Console.VisibleChanged += new System.EventHandler(this.richTextBox_Console_VisibleChanged);
            // 
            // button_Balance
            // 
            this.button_Balance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Balance.Location = new System.Drawing.Point(492, 192);
            this.button_Balance.Name = "button_Balance";
            this.button_Balance.Size = new System.Drawing.Size(125, 23);
            this.button_Balance.TabIndex = 0;
            this.button_Balance.Text = "BALANCE TEAMS!";
            this.button_Balance.UseVisualStyleBackColor = true;
            this.button_Balance.Click += new System.EventHandler(this.button_Balance_Click);
            // 
            // groupBox_Setting
            // 
            this.groupBox_Setting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Setting.Controls.Add(this.checkBox_BestOutput);
            this.groupBox_Setting.Controls.Add(this.numeric_Secondary);
            this.groupBox_Setting.Controls.Add(this.numeric_AutoFill);
            this.groupBox_Setting.Controls.Add(this.button_CustomPoints);
            this.groupBox_Setting.Controls.Add(this.checkBox_TrueRandom);
            this.groupBox_Setting.Controls.Add(this.checkBox_WriteRange);
            this.groupBox_Setting.Controls.Add(this.label7);
            this.groupBox_Setting.Controls.Add(this.label5);
            this.groupBox_Setting.Controls.Add(this.numeric_DesThrshRange);
            this.groupBox_Setting.Controls.Add(this.label4);
            this.groupBox_Setting.Controls.Add(this.numeric_MaxChecks);
            this.groupBox_Setting.Controls.Add(this.label2);
            this.groupBox_Setting.Controls.Add(this.numeric_StartSeed);
            this.groupBox_Setting.Controls.Add(this.label9);
            this.groupBox_Setting.Controls.Add(this.label6);
            this.groupBox_Setting.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Setting.Name = "groupBox_Setting";
            this.groupBox_Setting.Size = new System.Drawing.Size(617, 180);
            this.groupBox_Setting.TabIndex = 1;
            this.groupBox_Setting.TabStop = false;
            this.groupBox_Setting.Text = "Settings";
            // 
            // checkBox_BestOutput
            // 
            this.checkBox_BestOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_BestOutput.AutoSize = true;
            this.checkBox_BestOutput.Location = new System.Drawing.Point(310, 20);
            this.checkBox_BestOutput.Name = "checkBox_BestOutput";
            this.checkBox_BestOutput.Size = new System.Drawing.Size(249, 17);
            this.checkBox_BestOutput.TabIndex = 9;
            this.checkBox_BestOutput.Text = "Output best possible balance, without threshold";
            this.checkBox_BestOutput.UseVisualStyleBackColor = true;
            this.checkBox_BestOutput.CheckedChanged += new System.EventHandler(this.checkBox_BestOutput_CheckedChanged);
            // 
            // numeric_Secondary
            // 
            this.numeric_Secondary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numeric_Secondary.Location = new System.Drawing.Point(310, 44);
            this.numeric_Secondary.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_Secondary.Name = "numeric_Secondary";
            this.numeric_Secondary.Size = new System.Drawing.Size(52, 20);
            this.numeric_Secondary.TabIndex = 10;
            this.numeric_Secondary.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // numeric_AutoFill
            // 
            this.numeric_AutoFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numeric_AutoFill.Location = new System.Drawing.Point(310, 70);
            this.numeric_AutoFill.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_AutoFill.Name = "numeric_AutoFill";
            this.numeric_AutoFill.Size = new System.Drawing.Size(52, 20);
            this.numeric_AutoFill.TabIndex = 11;
            this.numeric_AutoFill.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            // 
            // button_CustomPoints
            // 
            this.button_CustomPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CustomPoints.Location = new System.Drawing.Point(486, 128);
            this.button_CustomPoints.Name = "button_CustomPoints";
            this.button_CustomPoints.Size = new System.Drawing.Size(125, 40);
            this.button_CustomPoints.TabIndex = 13;
            this.button_CustomPoints.Text = "Assign Custom Point Values to Ranks";
            this.button_CustomPoints.UseVisualStyleBackColor = true;
            this.button_CustomPoints.Click += new System.EventHandler(this.button_CustomPoints_Click);
            // 
            // checkBox_TrueRandom
            // 
            this.checkBox_TrueRandom.AutoSize = true;
            this.checkBox_TrueRandom.Location = new System.Drawing.Point(6, 71);
            this.checkBox_TrueRandom.Name = "checkBox_TrueRandom";
            this.checkBox_TrueRandom.Size = new System.Drawing.Size(177, 17);
            this.checkBox_TrueRandom.TabIndex = 7;
            this.checkBox_TrueRandom.Text = "Truly randomize without seeding";
            this.checkBox_TrueRandom.UseVisualStyleBackColor = true;
            this.checkBox_TrueRandom.CheckedChanged += new System.EventHandler(this.checkBox_TrueRandom_CheckedChanged);
            // 
            // checkBox_WriteRange
            // 
            this.checkBox_WriteRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_WriteRange.AutoSize = true;
            this.checkBox_WriteRange.Checked = true;
            this.checkBox_WriteRange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_WriteRange.Location = new System.Drawing.Point(310, 95);
            this.checkBox_WriteRange.Name = "checkBox_WriteRange";
            this.checkBox_WriteRange.Size = new System.Drawing.Size(206, 17);
            this.checkBox_WriteRange.TabIndex = 12;
            this.checkBox_WriteRange.Text = "Write other possible ranges in Console";
            this.checkBox_WriteRange.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label7.Location = new System.Drawing.Point(6, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(474, 57);
            this.label7.TabIndex = 19;
            this.label7.Text = "Minimum of 40 Players.\r\nMaximum of (#Players * 60%) Duos";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(64, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(243, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "Desired range threshold";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numeric_DesThrshRange
            // 
            this.numeric_DesThrshRange.Location = new System.Drawing.Point(6, 19);
            this.numeric_DesThrshRange.Name = "numeric_DesThrshRange";
            this.numeric_DesThrshRange.Size = new System.Drawing.Size(52, 20);
            this.numeric_DesThrshRange.TabIndex = 0;
            this.numeric_DesThrshRange.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(64, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(243, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Max number of checks (pref 100)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numeric_MaxChecks
            // 
            this.numeric_MaxChecks.Location = new System.Drawing.Point(6, 94);
            this.numeric_MaxChecks.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numeric_MaxChecks.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numeric_MaxChecks.Name = "numeric_MaxChecks";
            this.numeric_MaxChecks.Size = new System.Drawing.Size(52, 20);
            this.numeric_MaxChecks.TabIndex = 8;
            this.numeric_MaxChecks.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(64, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Starting seed value";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numeric_StartSeed
            // 
            this.numeric_StartSeed.Location = new System.Drawing.Point(6, 45);
            this.numeric_StartSeed.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numeric_StartSeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_StartSeed.Name = "numeric_StartSeed";
            this.numeric_StartSeed.Size = new System.Drawing.Size(52, 20);
            this.numeric_StartSeed.TabIndex = 1;
            this.numeric_StartSeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(363, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(243, 20);
            this.label9.TabIndex = 27;
            this.label9.Text = "% of value when Autofilled";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(363, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(243, 20);
            this.label6.TabIndex = 26;
            this.label6.Text = "% of value when Secondary or Filling";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Version
            // 
            this.label_Version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Version.Location = new System.Drawing.Point(329, 548);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(316, 23);
            this.label_Version.TabIndex = 5;
            this.label_Version.Text = "VERSION MESSAGE";
            this.label_Version.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.ReshowDelay = 100;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 575);
            this.Controls.Add(this.label_Version);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(677, 614);
            this.Name = "MainForm";
            this.Text = "League of Legends Balancer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Players)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox_Setting.ResumeLayout(false);
            this.groupBox_Setting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_Secondary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_AutoFill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_DesThrshRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_MaxChecks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_StartSeed)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button button_AddPlayer;
		private System.Windows.Forms.DataGridView dataGridView_Players;
		private System.Windows.Forms.Button button_SavePlayers;
		private System.Windows.Forms.Button button_LoadPlayers;
		private System.Windows.Forms.GroupBox groupBox_Setting;
		private System.Windows.Forms.Button button_Balance;
		private System.Windows.Forms.Label label_Total;
		private System.Windows.Forms.Label label_Version;
		private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button_newTemplate;
        private System.Windows.Forms.RichTextBox richTextBox_Console;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewButtonColumn X_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn name_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn summ_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn tier_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn div_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn primary_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn secondary_Col;
        private System.Windows.Forms.DataGridViewTextBoxColumn duo_Col;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numeric_DesThrshRange;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numeric_MaxChecks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numeric_StartSeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox_WriteRange;
        private System.Windows.Forms.CheckBox checkBox_TrueRandom;
        private System.Windows.Forms.Button button_CustomPoints;
        private System.Windows.Forms.CheckBox checkBox_BestOutput;
        private System.Windows.Forms.NumericUpDown numeric_Secondary;
        private System.Windows.Forms.NumericUpDown numeric_AutoFill;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
    }
}

