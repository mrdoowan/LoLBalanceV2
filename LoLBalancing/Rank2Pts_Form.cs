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
    public partial class Rank2Pts_Form : Form
    {
        private bool button_pressed = false;

        // Ctor
        public Rank2Pts_Form() {
            InitializeComponent();
        }

        // Prompt to open window
        public void openForm(ref Dictionary<string, int> rank2pts_Ref) {
            // Load dictionary into form
            foreach (string rank in rank2pts_Ref.Keys) {
                string pts = rank2pts_Ref[rank].ToString();
                dgv_Ranks.Rows.Add(rank, pts);
                DataGridViewRow rankRow = dgv_Ranks.Rows[dgv_Ranks.Rows.Count - 1];
                string tier = rank.Split(' ')[0];
                MainForm.FillCellColor(rankRow, 0, tier);
            }

            this.ShowDialog();
            if (button_pressed) {
                rank2pts_Ref.Clear();
                foreach (DataGridViewRow rankRow in dgv_Ranks.Rows) {
                    string Rank = rankRow.Cells[0].Value.ToString();
                    int Pts = int.Parse(rankRow.Cells[1].Value.ToString());
                    rank2pts_Ref.Add(Rank, Pts);
                }
            }
        }

        // Confirmed
        private void button_Confirm_Click(object sender, EventArgs e) {
            this.Close();
            button_pressed = true;
        }

        // Reset back 
        private void button_Reset_Click(object sender, EventArgs e) {
            if (MessageBox.Show("This will reset back to default values. Are you sure?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                int i = 0;
                foreach (int pts in MainForm.DEFAULT_PLAYER2VALUE.Values) {
                    try { dgv_Ranks[1, i].Value = pts.ToString(); }
                    catch { }
                    i++;
                }
            }
        }
    }
}
