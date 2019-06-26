using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace LoLBalancing
{
    public partial class MainForm : Form
    {
        public MainForm() {
            InitializeComponent();
        }

        #region Variables and Structs

        // Constants
        private const int NUM_PLAYERS = 5;
        //private const int MAX_PLAYERS_SCROLL = 12;
        //private const int DEFAULT_LASTCOL_WIDTH = 148;
        //private const int SCROLLBAR_WIDTH = 17;
        private const int START_EXCEL_ROW = 2;
        private const int TIER_ROW = 3;

        // Variables for upgrading
        //static private bool upgrading = false;
        private const string VERSION = "2.4.1";

        // Ranking consts
        public const string IRON = "Iron";
        public const string BRONZE = "Bronze";
        public const string SILVER = "Silver";
        public const string GOLD = "Gold";
        public const string PLATINUM = "Platinum";
        public const string DIAMOND = "Diamond";
        public const string MASTER = "Master";
        public const string GRANDMASTER = "Grandmaster";
        public const string CHALLENGER = "Challenger";
        public static readonly string[] TIER_LIST = 
            { IRON, BRONZE, SILVER, GOLD, PLATINUM, DIAMOND, MASTER, GRANDMASTER, CHALLENGER };

        // Color Codes for Ranks
        //public const string UNRANKHEX = "#B4A7D6";
        public const string ERRORHEX = "#FF0000";
        public const string IRONHEX = "#5E5354";
        public const string BRONZEHEX = "#864029";
        public const string SILVERHEX = "#849FA8";
        public const string GOLDHEX = "#D3963D";
        public const string PLATHEX = "#28B164";
        public const string DIAMONDHEX = "#A3C2DD";
        public const string MASTERHEX = "#C242C9";
        public const string GRANDMASTERHEX = "#EE555A";
        public const string CHALLENGERHEX = "#F5C085";

        // Current iteration of rank2pts value for balancing
        public static Dictionary<string, int> currRank2Pts = new Dictionary<string, int>();
        // Default dictionary of rank -> pt value
        public static Dictionary<string, int> DEFAULT_PLAYER2VALUE = new Dictionary<string, int>() {
            { "Iron 4", 1 },
            { "Iron 3", 2 },
            { "Iron 2", 3 },
            { "Iron 1", 4 },
            { "Bronze 4", 5 },
            { "Bronze 3", 6 },
            { "Bronze 2", 7 },
            { "Bronze 1", 8 },
            { "Silver 4", 9 },
            { "Silver 3", 10 },
            { "Silver 2", 11 },
            { "Silver 1", 12 },
            { "Gold 4", 13 },
            { "Gold 3", 14 },
            { "Gold 2", 15 },
            { "Gold 1", 16 },
            { "Platinum 4", 17 },
            { "Platinum 3", 18 },
            { "Platinum 2", 19 },
            { "Platinum 1", 20 },
            { "Diamond 4", 21 },
            { "Diamond 3", 22 },
            { "Diamond 2", 23 },
            { "Diamond 1", 24 },
            { "Master", 25 },
            { "Grandmaster", 25 },
            { "Challenger", 25 }
        };

        #endregion

        #region Helper Functions

        // For securing the Trash
        private void releaseObject(object obj) {
            try {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex) {
                obj = null;
                throw ex;
            }
            finally {
                GC.Collect();
            }
        }

        // Determine a ColorTranslator color baesd on the Ranking
        public static Color RankToColor(string tier) {
            switch (tier) {
                case IRON:
                    return ColorTranslator.FromHtml(IRONHEX);
                case BRONZE:
                    return ColorTranslator.FromHtml(BRONZEHEX);
                case SILVER:
                    return ColorTranslator.FromHtml(SILVERHEX);
                case GOLD:
                    return ColorTranslator.FromHtml(GOLDHEX);
                case PLATINUM:
                    return ColorTranslator.FromHtml(PLATHEX);
                case DIAMOND:
                    return ColorTranslator.FromHtml(DIAMONDHEX);
                case MASTER:
                    return ColorTranslator.FromHtml(MASTERHEX);
                case GRANDMASTER:
                    return ColorTranslator.FromHtml(GRANDMASTERHEX);
                case CHALLENGER:
                    return ColorTranslator.FromHtml(CHALLENGERHEX);
                default:
                    return ColorTranslator.FromHtml(ERRORHEX);
            }
        }

        // To fill in the background Cell color of a datagridview based on Ranking
        public static void FillCellColor(DataGridViewRow Row, int Ind, string Tier) {
            Row.Cells[Ind].Style.BackColor = RankToColor(Tier);
        }

        // Updates the label for total players
        private void Update_TotPlayers() {
            int numPlayers = dataGridView_Players.Rows.Count;
            label_Total.Text = "Total Players: " + numPlayers;
        }

        #endregion

        #region Event Handlers (Opening/Closing MainForm)

        private void MainForm_Load(object sender, EventArgs e) {
            label_Version.Text = "v" + VERSION + " by Steven Duan (sduans@umich.edu)";
            // Find a way to resize between different computers
            // https://stackoverflow.com/questions/8691951/can-the-datagridview-header-automatically-resize-itself

            // Load Properties.Settings
            try {
                string ptsList = Properties.Settings.Default.pointsList;
                string[] ptsListArr = ptsList.Split(' ');
                if (ptsListArr.Length == DEFAULT_PLAYER2VALUE.Count) {
                    int i = 0; // index for ptsListArr
                    foreach (string rank in DEFAULT_PLAYER2VALUE.Keys) {
                        currRank2Pts.Add(rank, int.Parse(ptsListArr[i]));
                        i++;
                    }
                    // This is totally assuming that they're equal size
                }
            }
            catch { }
            // Remember to check for empty currRank2Pts
            if (currRank2Pts.Count == 0) {
                foreach (string rank in DEFAULT_PLAYER2VALUE.Keys) {
                    currRank2Pts.Add(rank, DEFAULT_PLAYER2VALUE[rank]);
                }
            }

            string balSetStr = Properties.Settings.Default.balanceSettings;
            string[] balSetArr = balSetStr.Split(' ');
            try { numeric_DesThrshRange.Value = int.Parse(balSetArr[0]); } catch { }
            try { numeric_StartSeed.Value = int.Parse(balSetArr[1]); } catch { }
            try { checkBox_TrueRandom.Checked = bool.Parse(balSetArr[2]); } catch { }
            try { numeric_MaxChecks.Value = int.Parse(balSetArr[3]); } catch { }
            try { checkBox_BestOutput.Checked = bool.Parse(balSetArr[4]); } catch { }
            try { numeric_Secondary.Value = int.Parse(balSetArr[5]); } catch { }
            try { numeric_AutoFill.Value = int.Parse(balSetArr[6]); } catch { }
            try { checkBox_WriteRange.Checked = bool.Parse(balSetArr[7]); } catch { }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            // Save into Properties.Settings
            string ptsList = "";
            foreach (string rank in currRank2Pts.Keys) {
                int pt = currRank2Pts[rank];
                ptsList += pt + " ";
            }
            ptsList.TrimEnd(' ');
            Properties.Settings.Default.pointsList = ptsList;

            StringBuilder sbSettings = new StringBuilder();
            sbSettings.Append(numeric_DesThrshRange.Value + " ");   // [0] (int)
            sbSettings.Append(numeric_StartSeed.Value + " ");       // [1] (int)
            sbSettings.Append(checkBox_TrueRandom.Checked + " ");   // [2] (bool)
            sbSettings.Append(numeric_MaxChecks.Value + " ");       // [3] (int)
            sbSettings.Append(checkBox_BestOutput.Checked + " ");   // [4] (bool)
            sbSettings.Append(numeric_Secondary.Value + " ");       // [5] (int)
            sbSettings.Append(numeric_AutoFill.Value + " ");        // [6] (int)
            sbSettings.Append(checkBox_WriteRange.Checked);         // [7] (bool)
            Properties.Settings.Default.balanceSettings = sbSettings.ToString();

            Properties.Settings.Default.Save();
        }

        #endregion

        #region Event Handlers (TabPage: Player Roster)

        // WinForm for Adding a Player
        private void button_AddPlayer_Click(object sender, EventArgs e) {
            AddPlayer Player_Win = new AddPlayer();
            Player_Win.AddDialog(ref dataGridView_Players);
            Update_TotPlayers();
        }

        // OpenDialog Excel
        private void button_LoadPlayers_Click(object sender, EventArgs e) {
            if (dataGridView_Players.Rows.Count > 0) {
                if (MessageBox.Show("NOTE: You'll lose all data on the Players you currently have on the grid.\n" +
                    "Do you want to proceed?", "Caution", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) {
                    return;
                }
            }
            OpenFileDialog openExcelDialog = new OpenFileDialog();
            openExcelDialog.Filter = "Excel Sheet (*.xlsx)|*.xlsx";
            openExcelDialog.Title = "Load Players";
            openExcelDialog.RestoreDirectory = true;
            if (openExcelDialog.ShowDialog() == DialogResult.OK) {
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;
                // Open Excel
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(openExcelDialog.FileName);
                Excel.Worksheet Sheet = xlWorkBook.Worksheets.get_Item(1);
                dataGridView_Players.Rows.Clear();
                string name = "", summoner = "", tier = "", division = "", primary = "", secondary = "", duo = "";
                for (int i = START_EXCEL_ROW; Sheet.Cells[i, 1].value != null; ++i) {
                    int column = 1;
                    try { name = Sheet.Cells[i, column++].value.ToString(); } catch { name = ""; }
                    try { summoner = Sheet.Cells[i, column++].value.ToString(); } catch { summoner = ""; }
                    try { tier = Sheet.Cells[i, column++].value.ToString(); } catch { tier = ""; }
                    try { division = Sheet.Cells[i, column++].value.ToString(); } catch { division = ""; }
                    try { primary = Sheet.Cells[i, column++].value.ToString(); } catch { primary = ""; }
                    try { secondary = Sheet.Cells[i, column++].value.ToString(); } catch { secondary = ""; }
                    try { duo = Sheet.Cells[i, column++].value.ToString(); } catch { duo = ""; }
                    DataGridViewButtonColumn button = new DataGridViewButtonColumn();
                    dataGridView_Players.Rows.Add(button, name, summoner, tier, division, primary, secondary, duo);
                    DataGridViewRow playerCell = dataGridView_Players.Rows[dataGridView_Players.Rows.Count - 1];
                    playerCell.Cells[0].Value = "X";
                    FillCellColor(playerCell, TIER_ROW, tier);
                    // Update Players
                    Update_TotPlayers();
                }
                xlWorkBook.Close();
                xlApp.Quit();
                releaseObject(Sheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                dataGridView_Players.ClearSelection();
                Cursor.Current = Cursors.Default;
            }
        }

        // SaveDialog Excel
        private void button_SavePlayers_Click(object sender, EventArgs e) {
            SaveFileDialog saveExcelDialog = new SaveFileDialog();
            saveExcelDialog.Filter = "Excel Sheet (*.xlsx)|*.xlsx";
            saveExcelDialog.Title = "Save Players";
            if (saveExcelDialog.ShowDialog() == DialogResult.OK) {
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;
                // Make an Excel Sheet
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object mis = System.Reflection.Missing.Value;
                xlWorkBook = xlApp.Workbooks.Add(mis);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                try {
                    xlWorkSheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaper11x17;
                    xlWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                }
                catch { }
                xlWorkSheet.Columns["A"].ColumnWidth = 20.00;	// (1) Name
                xlWorkSheet.Columns["B"].ColumnWidth = 20.00;   // (2) Summoner
                xlWorkSheet.Columns["C"].ColumnWidth = 12.00;   // (3) Tier
                xlWorkSheet.Columns["D"].ColumnWidth = 5.00;    // (4) Division
                xlWorkSheet.Columns["E"].ColumnWidth = 8.00;	// (5) Primary
                xlWorkSheet.Columns["F"].ColumnWidth = 8.00;    // (6) Secondary
                xlWorkSheet.Columns["G"].ColumnWidth = 20.00;   // (7) Duo
                int column = 1;
                xlWorkSheet.Cells[1, column++] = "Name";
                xlWorkSheet.Cells[1, column++] = "Summoner";
                xlWorkSheet.Cells[1, column++] = "Tier";
                xlWorkSheet.Cells[1, column++] = "Div";
                xlWorkSheet.Cells[1, column++] = "Primary";
                xlWorkSheet.Cells[1, column++] = "Second";
                xlWorkSheet.Cells[1, column++] = "Duo";
                xlWorkSheet.Rows[1].Font.Bold = true;
                xlWorkSheet.Rows[1].Font.Underline = true;
                xlWorkSheet.get_Range("A1", "G1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                int row = 2;
                foreach (DataGridViewRow playerRow in dataGridView_Players.Rows) {
                    column = 1;
                    xlWorkSheet.Cells[row, column] = playerRow.Cells[column++].Value.ToString();
                    xlWorkSheet.Cells[row, column] = playerRow.Cells[column++].Value.ToString();
                    string tier = playerRow.Cells[column].Value.ToString();
                    xlWorkSheet.Cells[row, column] = tier;
                    xlWorkSheet.Cells[row, column].Interior.Color = RankToColor(tier);
                    xlWorkSheet.Cells[row, ++column] = playerRow.Cells[column++].Value.ToString();
                    xlWorkSheet.Cells[row, column] = playerRow.Cells[column++].Value.ToString();
                    xlWorkSheet.Cells[row, column] = playerRow.Cells[column++].Value.ToString();
                    xlWorkSheet.Cells[row, column] = playerRow.Cells[column++].Value.ToString();
                    row++;
                }

                releaseObject(xlWorkSheet);
                string filename = saveExcelDialog.FileName;
                try {
                    xlApp.DisplayAlerts = false;
                    xlWorkBook.SaveAs(filename);
                    xlApp.Visible = true;
                }
                catch (Exception ex) {
                    MessageBox.Show("Can't overwrite file. Please save it as another name." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                Cursor.Current = Cursors.Default;
            }
        }

        // Creates a new Template for Excel sheet
        private void button_newTemplate_Click(object sender, EventArgs e) {
            SaveFileDialog saveExcelDialog = new SaveFileDialog();
            saveExcelDialog.Filter = "Excel Sheet (*.xlsx)|*.xlsx";
            saveExcelDialog.Title = "Make New Template";
            if (saveExcelDialog.ShowDialog() == DialogResult.OK) {
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;
                // Make an Excel Sheet
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object mis = System.Reflection.Missing.Value;
                xlWorkBook = xlApp.Workbooks.Add(mis);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                try {
                    xlWorkSheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaper11x17;
                    xlWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                }
                catch { }
                xlWorkSheet.Columns["A"].ColumnWidth = 20.00;	// (1) Name
                xlWorkSheet.Columns["B"].ColumnWidth = 20.00;   // (2) Summoner
                xlWorkSheet.Columns["C"].ColumnWidth = 12.00;   // (3) Tier
                xlWorkSheet.Columns["D"].ColumnWidth = 5.00;    // (4) Division
                xlWorkSheet.Columns["E"].ColumnWidth = 8.00;	// (5) Primary
                xlWorkSheet.Columns["F"].ColumnWidth = 8.00;    // (6) Secondary
                xlWorkSheet.Columns["G"].ColumnWidth = 20.00;   // (7) Duo
                int column = 1;
                xlWorkSheet.Cells[1, column++] = "Name";
                xlWorkSheet.Cells[1, column++] = "Summoner";
                xlWorkSheet.Cells[1, column++] = "Tier";
                xlWorkSheet.Cells[1, column++] = "Div";
                xlWorkSheet.Cells[1, column++] = "Primary";
                xlWorkSheet.Cells[1, column++] = "Second";
                xlWorkSheet.Cells[1, column++] = "Duo";
                xlWorkSheet.Rows[1].Font.Bold = true;
                xlWorkSheet.Rows[1].Font.Underline = true;
                xlWorkSheet.get_Range("A1", "G1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                releaseObject(xlWorkSheet);
                string filename = saveExcelDialog.FileName;
                try {
                    xlApp.DisplayAlerts = false;
                    xlWorkBook.SaveAs(filename);
                    xlApp.Visible = true;
                }
                catch (Exception ex) {
                    MessageBox.Show("Can't overwrite file. Please save it as another name." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                Cursor.Current = Cursors.Default;
            }
        }

        // WinForm for Editing the Player
        private void dataGridView_Players_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            AddPlayer Player_Win = new AddPlayer();
            Player_Win.EditDialog(ref dataGridView_Players);
        }

        // Removing a Player
        private void dataGridView_Players_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            DataGridView Grid = (DataGridView)sender;
            if (e.RowIndex >= 0) {
                DataGridViewRow playerRow = Grid.Rows[e.RowIndex];
                if (Grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0) {
                    // Button Clicked for that row.
                    string message = "Do you want to remove \"" + playerRow.Cells[1].Value.ToString() + "\"?";
                    if (MessageBox.Show(message, "Reminder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        Grid.Rows.RemoveAt(playerRow.Index);
                        Grid.Refresh();
                        Grid.ClearSelection();
                    }
                }
            }
            Update_TotPlayers();
        }

        #endregion

        #region Event Handlers (TabPage: Balancing)

        // String -> Tier
        private static Dictionary<string, Tier> STRING_TO_TIER = new Dictionary<string, Tier>() {
            { IRON, Tier.IRON },
            { BRONZE, Tier.BRONZE },
            { SILVER, Tier.SILVER },
            { GOLD, Tier.GOLD },
            { PLATINUM, Tier.PLATINUM },
            { DIAMOND, Tier.DIAMOND },
            { MASTER, Tier.MASTER },
            { GRANDMASTER, Tier.GRANDMASTER },
            { CHALLENGER, Tier.CHALLENGER }
        };

        // String -> Role
        private static Dictionary<string, Role> STRING_TO_ROLE = new Dictionary<string, Role>() {
            { "Top", Role.TOP },
            { "Jungle", Role.JNG },
            { "Mid", Role.MID },
            { "ADC", Role.ADC },
            { "Support", Role.SUP },
            { "Fill", Role.FILL }
        };

        // Conducts the Balancing Algorithm and Saves once a roster is set
        private void button_Balance_Click(object sender, EventArgs e) {
            Application.DoEvents();
            Cursor.Current = Cursors.WaitCursor;
            richTextBox_Console.Clear();

            StartAlgo balanceFXN = new StartAlgo((int)numeric_DesThrshRange.Value, (int)numeric_StartSeed.Value,
                (int)numeric_MaxChecks.Value, checkBox_TrueRandom.Checked, checkBox_WriteRange.Checked, checkBox_BestOutput.Checked);
            if (!balanceFXN.loadParamsFunction(dataGridView_Players,
                (int)numeric_Secondary.Value, (int)numeric_AutoFill.Value, ref richTextBox_Console)) { return; }
            if (!balanceFXN.validateDuosFunction(ref richTextBox_Console)) { return; }
            if (!balanceFXN.balance(ref richTextBox_Console)) { return; }
        }

        // To enable/disable start seed value
        private void checkBox_TrueRandom_CheckedChanged(object sender, EventArgs e) {
            if (checkBox_TrueRandom.Checked) { numeric_StartSeed.Enabled = false; }
            else { numeric_StartSeed.Enabled = true; }
        }

        // Write in Console log. Error is true if it forces an exit.
        private void write_ConsoleLog(string msg, bool error) {
            if (error) {
                MessageBox.Show("An error occurred when trying to balance. Please look at the Console Log.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            richTextBox_Console.Text += msg + '\n';
        }

        // Makes a new window that allows point values to be customized from rank
        private void button_CustomPoints_Click(object sender, EventArgs e) {
            Rank2Pts_Form rank2ptsWin = new Rank2Pts_Form();
            rank2ptsWin.openForm(ref currRank2Pts);
        }

        // Still doesn't work yet
        private void checkBox_BestOutput_CheckedChanged(object sender, EventArgs e) {
            if (checkBox_BestOutput.Checked) { numeric_DesThrshRange.Enabled = false; }
            else { numeric_DesThrshRange.Enabled = true; }
        }

        // To enable autoscrolling when the textbox spits output
        private void richTextBox_Console_VisibleChanged(object sender, EventArgs e) {
            if (richTextBox_Console.Visible) {
                richTextBox_Console.SelectionStart = richTextBox_Console.TextLength;
                richTextBox_Console.ScrollToCaret();
            }
        }

        #endregion
    }
}
