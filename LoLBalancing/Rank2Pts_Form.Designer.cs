namespace LoLBalancing
{
    partial class Rank2Pts_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rank2Pts_Form));
            this.dgv_Ranks = new System.Windows.Forms.DataGridView();
            this.Rank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_Reset = new System.Windows.Forms.Button();
            this.button_Confirm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Ranks)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_Ranks
            // 
            this.dgv_Ranks.AllowUserToAddRows = false;
            this.dgv_Ranks.AllowUserToDeleteRows = false;
            this.dgv_Ranks.AllowUserToResizeColumns = false;
            this.dgv_Ranks.AllowUserToResizeRows = false;
            this.dgv_Ranks.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv_Ranks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Ranks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Rank,
            this.Value});
            this.dgv_Ranks.Location = new System.Drawing.Point(12, 12);
            this.dgv_Ranks.MultiSelect = false;
            this.dgv_Ranks.Name = "dgv_Ranks";
            this.dgv_Ranks.RowHeadersVisible = false;
            this.dgv_Ranks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_Ranks.Size = new System.Drawing.Size(175, 511);
            this.dgv_Ranks.TabIndex = 1;
            // 
            // Rank
            // 
            this.Rank.Frozen = true;
            this.Rank.HeaderText = "Rank";
            this.Rank.Name = "Rank";
            this.Rank.ReadOnly = true;
            this.Rank.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Rank.Width = 125;
            // 
            // Value
            // 
            this.Value.Frozen = true;
            this.Value.HeaderText = "Pts";
            this.Value.Name = "Value";
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Value.Width = 30;
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(12, 529);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(75, 23);
            this.button_Reset.TabIndex = 2;
            this.button_Reset.Text = "Reset";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // button_Confirm
            // 
            this.button_Confirm.Location = new System.Drawing.Point(112, 529);
            this.button_Confirm.Name = "button_Confirm";
            this.button_Confirm.Size = new System.Drawing.Size(75, 23);
            this.button_Confirm.TabIndex = 3;
            this.button_Confirm.Text = "Confirm";
            this.button_Confirm.UseVisualStyleBackColor = true;
            this.button_Confirm.Click += new System.EventHandler(this.button_Confirm_Click);
            // 
            // Rank2Pts_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 561);
            this.Controls.Add(this.button_Confirm);
            this.Controls.Add(this.button_Reset);
            this.Controls.Add(this.dgv_Ranks);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(215, 600);
            this.MinimumSize = new System.Drawing.Size(215, 600);
            this.Name = "Rank2Pts_Form";
            this.Text = "Assign Pts";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Ranks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Ranks;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rank;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.Button button_Confirm;
    }
}