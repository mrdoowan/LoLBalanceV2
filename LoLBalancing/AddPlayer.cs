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

		public void AddDialog(ref DataGridView player_dgv) {
			this.ShowDialog();
			if (button_pressed) {
				DataGridViewButtonColumn button = new DataGridViewButtonColumn();
                string primary = "", secondary = "";
				foreach (Control c in groupBox_PrimaryRoles.Controls) {
                    RadioButton rb = (RadioButton)c;
                    if (rb.Checked) { primary = rb.Text; }
                }
                foreach (Control c in groupBox_SecondaryRoles.Controls) {
                    RadioButton rb = (RadioButton)c;
                    if (rb.Checked) { secondary = rb.Text; }
                }
				// Add into DataGridView
				player_dgv.Rows.Add(button, textBox_Name.Text, textBox_IGN.Text, comboBox_Tier.Text, 
                    numericUpDown_Div.Value.ToString(), primary, secondary, textBox_Duo.Text);
				DataGridViewRow Player = player_dgv.Rows[player_dgv.Rows.Count - 1];
				Player.Cells[0].Value = "X";
				// Modify colors based on Ranking
				switch (comboBox_Tier.Text) {
                    case "Unranked":
                        Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.UNRANKHEX);
                        break;
					case "Bronze":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.BRONZEHEX);
						break;
					case "Silver":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.SILVERHEX);
						break;
					case "Gold":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.GOLDHEX);
						break;
					case "Platinum":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.PLATHEX);
						break;
					case "Diamond":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.DIAMONDHEX);
						break;
					case "Master":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.MASTERHEX);
						break;
					case "Challenger":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.CHALLENGERHEX);
						break;
					default:
						break;
				}
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
            string primary = "", secondary = "";
            try { primary = Players.SelectedRows[0].Cells[5].Value.ToString(); } catch { }
            try { secondary = Players.SelectedRows[0].Cells[6].Value.ToString(); } catch { }
            foreach (Control c in groupBox_PrimaryRoles.Controls) {
                RadioButton rb = (RadioButton)c;
                if (rb.Text == primary) { rb.Checked = true; }
            }
            foreach (Control c in groupBox_SecondaryRoles.Controls) {
                RadioButton rb = (RadioButton)c;
                if (rb.Text == secondary) { rb.Checked = true; }
            }
            textBox_Duo.Text = Players.SelectedRows[0].Cells[7].Value.ToString();
			this.ShowDialog();
			// Copy and paste from above
			if (button_pressed) {
                foreach (Control c in groupBox_PrimaryRoles.Controls) {
                    RadioButton rb = (RadioButton)c;
                    if (rb.Checked) { primary = rb.Text; }
                }
                foreach (Control c in groupBox_SecondaryRoles.Controls) {
                    RadioButton rb = (RadioButton)c;
                    if (rb.Checked) { secondary = rb.Text; }
                }
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
				switch (comboBox_Tier.Text) {
                    case "Unranked":
                        Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.UNRANKHEX);
                        break;
                    case "Bronze":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.BRONZEHEX);
						break;
					case "Silver":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.SILVERHEX);
						break;
					case "Gold":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.GOLDHEX);
						break;
					case "Platinum":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.PLATHEX);
						break;
					case "Diamond":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.DIAMONDHEX);
						break;
					case "Master":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.MASTERHEX);
						break;
					case "Challenger":
						Player.Cells[3].Style.BackColor = ColorTranslator.FromHtml(MainForm.CHALLENGERHEX);
						break;
					default:
						break;
				}
				Players.ClearSelection();
			}
		}

		private void button_OK_Click(object sender, EventArgs e) {
			button_pressed = true;
			this.Close();
		}
    }
}
