using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoLBalancing
{
	public partial class AddPlayer : Form
	{
		public AddPlayer() {
			InitializeComponent();
		}

		private bool button_pressed = false;
        private string primary = string.Empty;
        private string secondary = string.Empty;

		public void AddDialog(ref DataGridView player_dgv) {
			this.ShowDialog();
			if (button_pressed) {
				DataGridViewButtonColumn button = new DataGridViewButtonColumn();
				// Add into DataGridView
				player_dgv.Rows.Add(button, textBox_Name.Text, textBox_IGN.Text, comboBox_Tier.Text, 
                    numericUpDown_Div.Value.ToString(), primary, secondary, textBox_Duo.Text);
				DataGridViewRow Player = player_dgv.Rows[player_dgv.Rows.Count - 1];
				Player.Cells[0].Value = "X";
                // Modify colors based on Ranking
                Player.Cells[3].Style.BackColor = MainForm.RankToColor(comboBox_Tier.Text);
				player_dgv.ClearSelection();
			}
		}

		public void EditDialog(ref DataGridView Players) {
			this.Text = "Edit Player";
			button_OK.Text = "Edit";
            // Put it into the Form first.
            try { textBox_Name.Text = Players.SelectedRows[0].Cells[1].Value.ToString(); } catch { }
            try { textBox_IGN.Text = Players.SelectedRows[0].Cells[2].Value.ToString(); } catch { }
            try { comboBox_Tier.Text = Players.SelectedRows[0].Cells[3].Value.ToString(); } catch { }
            try { numericUpDown_Div.Value = int.Parse(Players.SelectedRows[0].Cells[4].Value.ToString()); } catch { }
            try { primary = Players.SelectedRows[0].Cells[5].Value.ToString(); } catch { }
            try { secondary = Players.SelectedRows[0].Cells[6].Value.ToString(); } catch { }
            foreach (Control c in groupBox_PrimaryRoles.Controls) {
                RadioButton rb = (RadioButton)c;
                if (rb.Text == primary) { rb.Checked = true; }
            }
            string[] secondList = secondary.Replace(" ", "").Split(',');
            foreach (Control c in groupBox_SecondaryRoles.Controls) {
                CheckBox rb = (CheckBox)c;
                if (secondList.Contains(rb.Text)) { rb.Checked = true; }
            }
            textBox_Duo.Text = Players.SelectedRows[0].Cells[7].Value.ToString();
			this.ShowDialog();
			// Copy and paste from above
			if (button_pressed) {
                // Edit into DataGridView
                Players.SelectedRows[0].Cells[1].Value = textBox_Name.Text;
				Players.SelectedRows[0].Cells[2].Value = textBox_IGN.Text;
				Players.SelectedRows[0].Cells[3].Value = comboBox_Tier.Text;
				Players.SelectedRows[0].Cells[4].Value = numericUpDown_Div.Value.ToString();
				Players.SelectedRows[0].Cells[5].Value = primary;
				Players.SelectedRows[0].Cells[6].Value = secondary;
                Players.SelectedRows[0].Cells[7].Value = textBox_Duo.Text;
                DataGridViewRow Player = Players.SelectedRows[0];
				Player.Cells[0].Value = "X";
                // Modify colors based on Ranking
                Player.Cells[3].Style.BackColor = MainForm.RankToColor(comboBox_Tier.Text);
				Players.ClearSelection();
			}
		}

		private void button_OK_Click(object sender, EventArgs e) {
            primary = string.Empty;
            foreach (Control c in groupBox_PrimaryRoles.Controls) {
                RadioButton rb = (RadioButton)c;
                if (rb.Checked) { primary = rb.Text; }
            }
            secondary = string.Empty;
            foreach (Control c in groupBox_SecondaryRoles.Controls) {
                CheckBox rb = (CheckBox)c;
                if (rb.Checked) { secondary += ", " + rb.Text; }
            }
            secondary = secondary.TrimStart(',', ' ');
            // Do checks
            string error_Message = string.Empty;
            if (string.IsNullOrWhiteSpace(textBox_Name.Text)) {
                error_Message += "Please provide a name for the Player\n";
            }
            if (string.IsNullOrWhiteSpace(textBox_IGN.Text)) {
                error_Message += "Please provide a summoner IGN for the Player\n";
            }
            if (string.IsNullOrWhiteSpace(comboBox_Tier.Text)) {
                error_Message += "Please provide a Tier for the Player\n";
            }
            if (string.IsNullOrWhiteSpace(primary)) {
                error_Message += "Please select a primary role for the Player\n";
            }
            if (string.IsNullOrWhiteSpace(secondary)) {
                error_Message += "Please provide secondary roles for the Player\n";
            }
            if (primary == secondary && primary != "Fill") {
                error_Message += "A Player should not have the same primary and secondary role.\n";
            }
            if (string.IsNullOrWhiteSpace(error_Message)) {
                button_pressed = true;
                this.Close();
            }
            else {
                MessageBox.Show(error_Message.TrimEnd('\n'), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}
    }
}
