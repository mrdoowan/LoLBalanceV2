using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        private const int MAX_PLAYERS_SCROLL = 12;
        private const int DEFAULT_LASTCOL_WIDTH = 148;
        private const int SCROLLBAR_WIDTH = 17;

        // Variables for upgrading
        //static private bool upgrading = false;
        private const string VERSION = "2.0";

        // Color Codes for Ranks
        public const string UNRANKHEX = "#B4A7D6";
        public const string BRONZEHEX = "#9B5105";
        public const string SILVERHEX = "#C0C0C0";
        public const string GOLDHEX = "#F6B26B";
        public const string PLATHEX = "#5CBFA1";
        public const string DIAMONDHEX = "#A4C2F4";
        //public const string MASTERHEX = "#E5E5E5";
        public const string MASTERHEX = "#FFD966";
        public const string CHALLENGERHEX = "#FFD966";

        #endregion

        #region Helper Functions

        // For securing the Trash
        private static void releaseObject(object obj) {
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

        // Check update for a new VERSION
        private void Check_Update() {

        }

        // To fill in the background Cell color of a datagridview based on Ranking
        private void FillCellColor(DataGridViewRow Row, int Ind, string Tier) {
            switch (Tier) {
                case "Unranked":
                    Row.Cells[Ind].Style.BackColor = ColorTranslator.FromHtml(UNRANKHEX);
                    break;
                case "Bronze":
                    Row.Cells[Ind].Style.BackColor = ColorTranslator.FromHtml(BRONZEHEX);
                    break;
                case "Silver":
                    Row.Cells[Ind].Style.BackColor = ColorTranslator.FromHtml(SILVERHEX);
                    break;
                case "Gold":
                    Row.Cells[Ind].Style.BackColor = ColorTranslator.FromHtml(GOLDHEX);
                    break;
                case "Platinum":
                    Row.Cells[Ind].Style.BackColor = ColorTranslator.FromHtml(PLATHEX);
                    break;
                case "Diamond":
                    Row.Cells[Ind].Style.BackColor = ColorTranslator.FromHtml(DIAMONDHEX);
                    break;
                case "Master":
                    Row.Cells[Ind].Style.BackColor = ColorTranslator.FromHtml(MASTERHEX);
                    break;
                case "Challenger":
                    Row.Cells[Ind].Style.BackColor = ColorTranslator.FromHtml(CHALLENGERHEX);
                    break;
                default:
                    break;
            }
        }

        // Updates the label for total players
        private void Update_TotPlayers() {
            int numPlayers = dataGridView_Players.Rows.Count;
            label_Total.Text = "Total Players: " + numPlayers;
        }

        // Adjusts the range to remove the horizontal scroll
        private void Update_dgvHeaderSize() {
            if (dataGridView_Players.Rows.Count > MAX_PLAYERS_SCROLL) {
                duo_Col.MinimumWidth = DEFAULT_LASTCOL_WIDTH - SCROLLBAR_WIDTH;
                duo_Col.Width = DEFAULT_LASTCOL_WIDTH - SCROLLBAR_WIDTH;
            }
            else {
                duo_Col.MinimumWidth = DEFAULT_LASTCOL_WIDTH;
                duo_Col.Width = DEFAULT_LASTCOL_WIDTH;
            }
        }

        #endregion

        #region Event Handlers (Opening/Closing MainForm)

        private void MainForm_Load(object sender, EventArgs e) {
            label_Version.Text = "v" + VERSION + " by Steven Duan (sduans@umich.edu)";
            // Find a way to resize between different computers
            // https://stackoverflow.com/questions/8691951/can-the-datagridview-header-automatically-resize-itself
            
            // Load Properties.Settings
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            // Save into Properties.Settings
            // Properties.Settings.Default.Save();
        }

        #endregion

        #region Event Handlers (TabPage: Player Roster)

        // WinForm for Adding a Player
        private void button_AddPlayer_Click(object sender, EventArgs e) {
            AddPlayer Player_Win = new AddPlayer();
            Player_Win.AddDialog(ref dataGridView_Players);
            Update_TotPlayers();
            Update_dgvHeaderSize();
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
                for (int i = 2; Sheet.Cells[i, 1].value != null; ++i) { // Start at Row 2
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
                    FillCellColor(playerCell, 3, tier);
                    // Update Players
                    Update_TotPlayers();
                    Update_dgvHeaderSize();
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
                    switch (tier) {
                        case "Unranked":
                            xlWorkSheet.Cells[row, column].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(UNRANKHEX));
                            break;
                        case "Bronze":
                            xlWorkSheet.Cells[row, column].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(BRONZEHEX));
                            break;
                        case "Silver":
                            xlWorkSheet.Cells[row, column].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(SILVERHEX));
                            break;
                        case "Gold":
                            xlWorkSheet.Cells[row, column].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(GOLDHEX));
                            break;
                        case "Platinum":
                            xlWorkSheet.Cells[row, column].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(PLATHEX));
                            break;
                        case "Diamond":
                            xlWorkSheet.Cells[row, column].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(DIAMONDHEX));
                            break;
                        case "Master":
                            xlWorkSheet.Cells[row, column].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(MASTERHEX));
                            break;
                        case "Challenger":
                            xlWorkSheet.Cells[row, column].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(CHALLENGERHEX));
                            break;
                        default:
                            break;
                    }
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

        // Creates a new Template to go off of
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
            Update_dgvHeaderSize();
        }

        #endregion

        #region Event Handlers (TabPage: Balancing)

        // Constant variables
        private const int MIN_PLAYERS = 40;
        private const int NUM_ROLES = 5;

        // String -> Tier
        private static Dictionary<string, Tier> STRING_TO_TIER = new Dictionary<string, Tier>() {
            { "Bronze", Tier.BRONZE },
            { "Silver", Tier.SILVER },
            { "Gold", Tier.GOLD },
            { "Platinum", Tier.PLATINUM },
            { "Diamond", Tier.DIAMOND },
            { "Master", Tier.MASTER },
            { "Challenger", Tier.MASTER }
        };

        // String -> Role
        private static Dictionary<string, Role> STRING_TO_ROLE = new Dictionary<string, Role>() {
            { "Top", Role.TOP },
            { "Jungle", Role.JNG },
            { "Mid", Role.MID },
            { "ADC", Role.BOT },
            { "Support", Role.SUP },
            { "Fill", Role.FILL }
        };

        // Conducts the Balancing Algorithm and Saves once a roster is set
        private void button_Balance_Click(object sender, EventArgs e) {
            Application.DoEvents();
            Cursor.Current = Cursors.WaitCursor;
            richTextBox_Console.Clear();
            int parseRow = 0;
            // IGN -> Player Obj. IGN ALWAYS LOWERCASE
            Dictionary<string, Player> roster = new Dictionary<string, Player>();
            // IGN -> duoIGN. ALWAYS IN LOWERCASE
            Dictionary<string, string> duoList = new Dictionary<string, string>();
            try {
                foreach (DataGridViewRow playerRow in dataGridView_Players.Rows) {
                    string playerName = playerRow.Cells[1].Value.ToString();
                    string playerIGN = playerRow.Cells[2].Value.ToString();
                    Tier playerTier = STRING_TO_TIER[playerRow.Cells[3].Value.ToString()];
                    int playerDiv = int.Parse(playerRow.Cells[4].Value.ToString());
                    Role playerPri = STRING_TO_ROLE[playerRow.Cells[5].Value.ToString()];
                    Role playerSec = (playerPri == Role.FILL) ?
                            Role.FILL : STRING_TO_ROLE[playerRow.Cells[6].Value.ToString()];
                    if (playerPri == playerSec && playerPri != Role.FILL) {
                        string msg = playerName + ": Filled out same primary and secondary role.";
                        write_ConsoleLog(msg, false);
                        playerSec = Role.FILL;
                    }
                    string playerDuo = playerRow.Cells[7].Value.ToString();
                    duoList.Add(playerIGN.ToLower(), playerDuo.ToLower());
                    roster.Add(playerIGN.ToLower(), new Player(playerName, playerIGN,
                        playerTier, playerDiv, playerPri, playerSec, playerDuo));
                }

            }
            catch (Exception ex) {
                string msg = "ERROR - Parsing Inputs: " + ex.Message + ". Check Row: " + parseRow;
                write_ConsoleLog(msg, true);
                return;
            }

            // ----- Check number of Players
            if (roster.Count < MIN_PLAYERS) {
                string msg = "ERROR - Total number of Players needs to be 40.";
                write_ConsoleLog(msg, true);
                return;
            }
            if (roster.Count % NUM_ROLES != 0) {
                string msg = "ERROR - Total number of Players is not divisible by 5.";
                write_ConsoleLog(msg, true);
                return;
            }
            int numTeams = roster.Count / NUM_ROLES;
            // ----- Validate duos and their Roles
            List<string> noDuoList = new List<string>();
            foreach (string summoner in duoList.Keys) {
                try {
                    string duoIGNLower = duoList[summoner.ToLower()];
                    string origIGNLower = duoList[duoIGNLower];
                    if (summoner.ToLower() != origIGNLower) {
                        throw new Exception();
                        // Pretty much being treated as not finding a Key
                        // It's bad practice, but suitable
                    }
                    // At this point, the duos are confirmed.
                    // Now both duos roles for primary and secondary must be different
                    // If not, Fill for both primary and secondary
                    // 1) If Primary/Secondary are the same
                    Player origPlayer = roster[summoner];
                    Player duoPlayer = roster[duoIGNLower];
                    if ((origPlayer.primaryRole == duoPlayer.secondRole && origPlayer.secondRole == duoPlayer.primaryRole) &&
                        (origPlayer.primaryRole != Role.FILL || duoPlayer.primaryRole != Role.FILL)) {
                        string msg = summoner + " and " + duoIGNLower + 
                            ": Both duos for primary and secondary were interchangeably the same.";
                        write_ConsoleLog(msg, false);
                        origPlayer.secondRole = Role.FILL;
                        duoPlayer.secondRole = Role.FILL;
                    }
                    // 2) If both Primary is same
                    // Roll a number on who to bump Secondary to Primary, and make Secondary Fill
                    if (origPlayer.primaryRole == duoPlayer.primaryRole) {
                        string msg = summoner + " and " + duoIGNLower +
                            ": Both duos had the same primary role.";
                        write_ConsoleLog(msg, false);
                        if (randomNumber(1, 2) == 1) {
                            origPlayer.primaryRole = origPlayer.secondRole;
                            origPlayer.secondRole = Role.FILL;
                        }
                        else {
                            duoPlayer.primaryRole = origPlayer.secondRole;
                            origPlayer.secondRole = Role.FILL;
                        }
                    }
                }
                catch {
                    // Player couldn't be found or incorrect, so we blank out duo
                    Player player = roster[summoner];
                    if (!string.IsNullOrWhiteSpace(player.duo)) {
                        string msg = summoner + ": Duo \"" + player.duo + "\" does not exist.";
                        write_ConsoleLog(msg, false);
                    }
                    player.duo = "";
                    // Can't modify duoList yet, so store the name
                    noDuoList.Add(summoner);
                }
            }
            // Blank out duoList
            foreach (string summoner in noDuoList) {
                duoList[summoner] = "";
            }
            // Store lowest ranked player
            Player lowestPlayer = roster.Values.ToList().Min();

            // ----- PART 2: ASSIGN ROLES
            int lowestRange = (int)numeric_Threshold.Value + 1;
            int randSeed = (int)numeric_StartSeed.Value, currChecks = 0;
            Balance bestBalance = new Balance(); // This will store our bestBalance
            // Begin megaloop
            while (lowestRange != (int)numeric_Threshold.Value && 
                currChecks < (int)numeric_MaxChecks.Value) {
                // Preparations: Make a deep copy of roster that can be edited
                Dictionary<string, Player> masterList = deepClone(roster);

                // Begin
                Dictionary<Role, List<Player>> assignRoleList = new Dictionary<Role, List<Player>>() {
                        { Role.TOP, new List<Player>() },
                        { Role.JNG, new List<Player>() },
                        { Role.MID, new List<Player>() },
                        { Role.BOT, new List<Player>() },
                        { Role.SUP, new List<Player>() },
                    };
                // Assign non-Fill Primary Roles
                try {
                    assignRoles(ref masterList, ref assignRoleList, duoList,
                        numTeams, randSeed, true);
                }
                catch (Exception ex) {
                    string msg = "ERROR - Assigning primary roles. " +
                        "Reason: " + ex.Message;
                    write_ConsoleLog(msg, true);
                    return;
                }
                // Assign non-Fill Secondary Roles
                try {
                    assignRoles(ref masterList, ref assignRoleList, duoList,
                        numTeams, randSeed, false);
                }
                catch (Exception ex) {
                    string msg = "ERROR - Assigning secondary roles. " +
                        "Reason: " + ex.Message;
                    write_ConsoleLog(msg, true);
                    return;
                }
                // the remaining masterList is now fillList
                while (masterList.Count > 0) {
                    // Find role with smallest points, and add highest Ranked player
                    Role addFillRole = roleMinPointsFill(assignRoleList, numTeams);
                    Player addPlayer = masterList.Values.ToList().Max();
                    addPlayer.assignedRole = addFillRole;
                    assignRoleList[addFillRole].Add(addPlayer);
                    masterList.Remove(addPlayer.ign.ToLower());
                }
                // Validation: Should hopefully never occur
                foreach (List<Player> roleList in assignRoleList.Values) {
                    if (roleList.Count != numTeams) {
                        string msg = "ERROR - assignRoleList does not have numTeams";
                        write_ConsoleLog(msg, true);
                        return;
                    }
                }

                // ---- PART 3: BALANCING
                // Initialize the Balance abstract
                Balance currBalance = new Balance(assignRoleList, out lowestRange);
                // All queues acting as a clock queue
                int currMaxIndex = currBalance.getMaxTeamIndex();
                int currMinIndex = currBalance.getMinTeamIndex();
                int rolesRemain = NUM_ROLES;
                Queue<Role> roleQ = new Queue<Role>(new Role[] {
                    Role.TOP, Role.JNG, Role.MID, Role.BOT, Role.SUP });
                Queue<int> maxIndexQ = new Queue<int>();
                Queue<int> minIndexQ = new Queue<int>();
                for (int i = 0; i < numTeams; ++i) {
                    maxIndexQ.Enqueue(i);
                    minIndexQ.Enqueue(i);
                }
                // Conditions:
                // 1) rolesRemaining == 0
                // If lowestRange changes: rolesRemaining set back to 5
                // To move onto the next Role
                // 1) Check every iteration of minTeamIndex (minCheckRemain == 0)
                // 2) Check every iteration of maxTeamIndex (maxCheckRemain == 0)
                // If lowestRange changes: both checkRemaining set back to numTeams
                // Break entirely if: 
                // 1) lowestRange <= THRESHOLD
                while (rolesRemain > 0) {
                    Role checkRole = roleQ.Peek();
                    int minCheckRemain = numTeams, maxCheckRemain = numTeams;
                    bool minFlag = true;
                    // Check within each Role
                    while (!(minCheckRemain == 0 && maxCheckRemain == 0)) {
                        // Work on minIndex first
                        int swapIndex = (minFlag) ? minIndexQ.Peek() : maxIndexQ.Peek();
                        int newRange = (minFlag) ?
                            currBalance.switchPlayer(checkRole, currMinIndex, swapIndex) :
                            currBalance.switchPlayer(checkRole, currMaxIndex, swapIndex);
                        if (newRange < lowestRange) {
                            // Condition 2: New minRange
                            bestBalance = deepClone(currBalance);
                            currMinIndex = currBalance.getMinTeamIndex();
                            currMaxIndex = currBalance.getMaxTeamIndex();
                            lowestRange = newRange;
                            minCheckRemain = numTeams;
                            maxCheckRemain = numTeams;
                            rolesRemain = NUM_ROLES;
                            minFlag = true;
                        }
                        else {
                            // Condition 3: Continue iterating. Switch currBalance back
                            if (minFlag) {
                                minCheckRemain--;
                                currBalance.switchPlayer(checkRole, currMinIndex, swapIndex);
                            }
                            else {
                                maxCheckRemain--;
                                currBalance.switchPlayer(checkRole, currMaxIndex, swapIndex);
                            }
                            if (minCheckRemain == 0) {
                                minFlag = false;
                            }
                        }
                        // Cycle through the clock queue
                        if (minFlag) {
                            minIndexQ.Dequeue();
                            minIndexQ.Enqueue(swapIndex);
                        }
                        else {
                            maxIndexQ.Dequeue();
                            maxIndexQ.Enqueue(swapIndex);
                        }
                    }
                    // Made it to the end of a complete role check
                    rolesRemain--;
                    roleQ.Dequeue();
                    roleQ.Enqueue(checkRole);
                }
                // An entire "seedCheck" is finished here
                if (checkBox_WriteRange.Checked) {
                    string msg = "Lowest range in Seed " + randSeed + ": " + lowestRange;
                    write_ConsoleLog(msg, false);
                }
                currChecks++; randSeed++;
            }
            Cursor.Current = Cursors.Default;
            // ---- PART 4: OUTPUT BEST
            // If we got to this point, that means bestBalance has our best teams
            if (lowestRange <= (int)numeric_Threshold.Value &&
                MessageBox.Show("A balance was found with a Range of " + lowestRange + " at seed " + randSeed +
                    ".\nWould you like to save it?",
                    "Finished", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                save_TeamsExcel(bestBalance);
            }
            else if (lowestRange > (int)numeric_Threshold.Value) {
                MessageBox.Show("Could not find a balance with that threshold.");
            }
            return;
        }

        // Write in Console log. Error is true if it forces an exit.
        private void write_ConsoleLog(string msg, bool error) {
            if (error) {
                MessageBox.Show("An error occurred when trying to balance. Please look at the Console Log.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            richTextBox_Console.Text += msg + '\n';
        }

        // Once Teams are completely balanced, output and save them into an Excel sheet.
        private void save_TeamsExcel(Balance outBalance) {
            SaveFileDialog sfd_Excel = new SaveFileDialog();
            sfd_Excel.Filter = "Excel Sheet (*.xlsx)|*.xlsx";
            sfd_Excel.Title = "Save Teams";
            if (sfd_Excel.ShowDialog() == DialogResult.OK) {
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;
                // Make an Excel Sheet
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object mis = System.Reflection.Missing.Value;
                xlWorkBook = xlApp.Workbooks.Add(mis);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.ActiveSheet;
                xlWorkSheet.Name = "Teams Balance";
                try {
                    xlWorkSheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaper11x17;
                    xlWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                }
                catch { }

                #region Make Excel Sheet
                xlWorkSheet.Columns["A"].ColumnWidth = 20.00;   // Name
                xlWorkSheet.Columns["B"].ColumnWidth = 20.00;   // Summoner Name
                xlWorkSheet.Columns["C"].ColumnWidth = 20.00;   // Assigned Role
                xlWorkSheet.Columns["D"].ColumnWidth = 15.00;   // Ranking
                xlWorkSheet.Cells[1, 1] = "Name";
                xlWorkSheet.Cells[1, 2] = "Summoner Name";
                xlWorkSheet.Cells[1, 3] = "Assigned Role";
                xlWorkSheet.Cells[1, 4] = "Ranking";
                xlWorkSheet.Rows[1].Font.Bold = true;
                xlWorkSheet.Rows[1].Font.Underline = true;
                xlWorkSheet.get_Range("A1", "D1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                int row = 3, team = 1;
                List<Role> roleList = new List<Role>(new Role[] {
                            Role.TOP, Role.JNG, Role.MID, Role.BOT, Role.SUP });
                Dictionary<Role, string> role2String = new Dictionary<Role, string>() {
                            { Role.TOP, "Top" },
                            { Role.JNG, "Jungle" },
                            { Role.MID, "Mid" },
                            { Role.BOT, "ADC" },
                            { Role.SUP, "Support" },
                            { Role.NONE, "None" }
                        };
                Dictionary<Tier, string> tier2String = new Dictionary<Tier, string>() {
                            { Tier.BRONZE, "Bronze" },
                            { Tier.SILVER, "Silver" },
                            { Tier.GOLD, "Gold" },
                            { Tier.PLATINUM, "Platinum" },
                            { Tier.DIAMOND, "Diamond" },
                            { Tier.MASTER, "Master" }
                        };
                foreach (Team teamSel in outBalance.teams) {
                    xlWorkSheet.Cells[row, 1] = "Team " + team;
                    xlWorkSheet.get_Range("A" + row, "D" + row).Merge();

                    foreach (Role role in roleList) {
                        row++;
                        Player player = teamSel.getPlayerRole(role);
                        xlWorkSheet.Cells[row, 1] = player.name;
                        string ignString = player.ign;
                        if (player.hasDuo()) { ignString += " (D)"; }
                        xlWorkSheet.Cells[row, 2] = ignString;
                        string roleString = role2String[player.assignedRole];
                        if (player.isAutoFilled()) { roleString += " (Autofilled)"; }
                        xlWorkSheet.Cells[row, 3] = roleString;
                        string ranking = tier2String[player.tier] + " ";
                        if (player.tier != Tier.MASTER) { ranking += player.division; }
                        xlWorkSheet.Cells[row, 4] = ranking;
                        switch (player.tier) {
                            case Tier.BRONZE:
                                xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(BRONZEHEX));
                                break;
                            case Tier.SILVER:
                                xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(SILVERHEX));
                                break;
                            case Tier.GOLD:
                                xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(GOLDHEX));
                                break;
                            case Tier.PLATINUM:
                                xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(PLATHEX));
                                break;
                            case Tier.DIAMOND:
                                xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(DIAMONDHEX));
                                break;
                            case Tier.MASTER:
                                xlWorkSheet.Cells[row, 4].Interior.Color = ColorTranslator.ToOle(ColorTranslator.FromHtml(CHALLENGERHEX));
                                break;
                            default:
                                break;
                        }
                    }
                    row++; row++; team++;
                }
                #endregion

                releaseObject(xlWorkSheet);
                string filename = sfd_Excel.FileName;
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

        // Deep Copy function
        public T deepClone<T>(T obj) {
            using (var ms = new MemoryStream()) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        // Rolls a uniform random number between min and max
        private int randomNumber(int min, int max) {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }
        private int randomNumber(int min, int max, int seedVal) {
            Random rnd = new Random(seedVal);
            return rnd.Next(min, max);
        }

        // Assigning roles from masterList based on primary or secondary
        private void assignRoles(ref Dictionary<string, Player> masterList,
            ref Dictionary<Role, List<Player>> assignRoleList,
            Dictionary<string, string> duoList,
            int numTeams, int randSeed, bool primaryRole) {
            // Fxn start
            List<string> removeList = new List<string>();
            foreach (string ign in masterList.Keys) {
                Player player = masterList[ign];
                string duoName = duoList[ign];
                Role role = (primaryRole) ? player.primaryRole : player.secondRole;
                if (role != Role.FILL && !containsName(assignRoleList[role], duoName)) {
                    player.assignedRole = role;
                    assignRoleList[role].Add(player);
                    // Remove from masterList
                    // Currently immutabale due to iteration. Remove later
                    removeList.Add(ign);
                }
            }
            foreach (string removeKey in removeList) {
                masterList.Remove(removeKey.ToLower());
            }
            // From Autofill options, determine the option number
            int roleChangeOption = 0;
            if (radioButton_Best.Checked) { roleChangeOption = 1; }
            else if (radioButton_High.Checked) { roleChangeOption = 2; }
            else if (radioButton_Low.Checked) { roleChangeOption = 3; }
            // Check if each Role has more than 8 players.
            // If so, "roleChange" a player
            foreach (List<Player> roleList in assignRoleList.Values) {
                while (roleList.Count > numTeams) {
                    // Used to determine who gets kicked
                    Player changePlayer = roleChange(roleChangeOption, roleList, randSeed, primaryRole);
                    roleList.Remove(changePlayer);
                    changePlayer.assignedRole = Role.NONE;
                    masterList.Add(changePlayer.ign.ToLower(), changePlayer);
                }
            }
        }

        // Removes a player from the roleList if > numTeams
        // Based on the option, it determines who gets kicked
        // The Player getting kicked is returned.
        private Player roleChange(int option, List<Player> roleList,
            int randSeed, bool primary) {
            // Make a new list of secondaries if primary = false
            List<Player> roleSecList = new List<Player>();
            if (!primary) {
                foreach (Player addPlayer in roleList) {
                    if (addPlayer.isSecondaryAssigned()) {
                        roleSecList.Add(addPlayer);
                    }
                }
            }
            Player changePlayer = new Player();
            if (option == 1) {
                // Method 1: Remove by random
                int index = (primary) ?
                    randomNumber(0, roleList.Count - 1, randSeed) :
                    randomNumber(0, roleSecList.Count - 1, randSeed);
                changePlayer = (primary) ? roleList[index] : roleSecList[index];
            }
            else if (option == 2) {
                // Method 2: Remove the higher ranked
                changePlayer = (primary) ? roleList.Max() : roleSecList.Max();
            }
            else if (option == 3) {
                // Method 3: Remove the lower ranked
                changePlayer = (primary) ? roleList.Min() : roleSecList.Min();
            }
            return changePlayer;
        }

        // Figure out which role that's < numTeams has the least
        private Role roleMinPointsFill(Dictionary<Role, List<Player>> assignRoleList,
            int numTeams) {
            Role retRole = Role.NONE;
            int smallest = 9000;    // yeah...
            foreach (Role role in assignRoleList.Keys) {
                if (assignRoleList[role].Count < numTeams) {
                    int roleTotalVal = 0;
                    foreach (Player player in assignRoleList[role]) {
                        roleTotalVal += player.rankValue(true);
                    }
                    if (roleTotalVal < smallest) {
                        smallest = roleTotalVal;
                        retRole = role;
                    }
                }
            }
            return retRole;
        }

        // Checks if the name is in the playerList
        private bool containsName(List<Player> roleList, string name) {
            foreach (Player player in roleList) {
                if (player.ign.ToLower() == name.ToLower()) {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
