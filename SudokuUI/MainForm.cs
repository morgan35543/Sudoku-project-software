using Algorithms;
using Code;
using PuzzleCreation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SudokuProjectUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            toolStripFileName.Text = "Blank puzzle";
            stopW1.Reset();
            time1 = new System.Windows.Forms.Timer();
            time1.Interval = 10;
            time1.Tick += timerUI_Tick;
            time1.Start();
        }


        #region Toolstrip


        #region Unselecter
        /// <summary>
        /// Unselects other toolstrip menu items. Only one solver may be selected at any given moment.
        /// </summary>
        /// <param name="selectedMenuItem"></param>
        public void UncheckOtherToolStripMenuItems(ToolStripMenuItem selectedMenuItem)
        {
            selectedMenuItem.Checked = true;

            foreach (var ltoolStripMenuItem in (from object
                                                    item in selectedMenuItem.Owner.Items
                                                let ltoolStripMenuItem = item as ToolStripMenuItem
                                                where ltoolStripMenuItem != null
                                                where !item.Equals(selectedMenuItem)
                                                select ltoolStripMenuItem))
                (ltoolStripMenuItem).Checked = false;

            // Keeps the menu open after selecting
            selectedMenuItem.Owner.Show();
        }
                
        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setBtn.Show();
            PuzzleInfoBox.Text = "";
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
            stopW1.Stop();
            stopW1.Reset();
            stopW1.Start();
        }
        private void bruteForceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setBtn.Hide();
            PuzzleInfoBox.Text = "";
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
            stopW1.Stop();
            stopW1.Reset();
        }
        private void depthFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setBtn.Hide();
            PuzzleInfoBox.Text = "";
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
            stopW1.Stop();
            stopW1.Reset();
        }
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setBtn.Hide();
            PuzzleInfoBox.Text = "";
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
            stopW1.Stop();
            stopW1.Reset();
        }
        private void soleCandidateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setBtn.Hide();
            PuzzleInfoBox.Text = "";
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
            stopW1.Stop();
            stopW1.Reset();
        }
        private void soleCandidateHiddenSinglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setBtn.Hide();
            PuzzleInfoBox.Text = "";
            UncheckOtherToolStripMenuItems((ToolStripMenuItem)sender);
            stopW1.Stop();
            stopW1.Reset();
        }
        #endregion Unselecter


        /// <summary>
        /// Close form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        #region puzzle I/O


        // New - Easy
        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newPuzzle = IO.loadPresetPuzzle(0, "MainForm");
            char[] tempCharPuzzle = newPuzzle.ToCharArray();
            char[,] readPuzzle = new char[9,9];
            int counter = 0;

            for (int currentRow = 0; currentRow < 9; currentRow++)
            {
                for (int currentCol = 0; currentCol < 9; currentCol++)
                {
                    readPuzzle[currentRow, currentCol] = tempCharPuzzle[counter];
                    puzzleCopy[currentRow, currentCol] = int.Parse(tempCharPuzzle[counter].ToString());
                    counter++;
                }
            }

            CharArrToText(readPuzzle);
            toolStripFileName.Text = "Easy puzzle";
        }

        // New - Medium
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newPuzzle = IO.loadPresetPuzzle(1, "MainForm");
            char[] tempCharPuzzle = newPuzzle.ToCharArray();
            char[,] readPuzzle = new char[9, 9];
            int counter = 0;

            for (int currentRow = 0; currentRow < 9; currentRow++)
            {
                for (int currentCol = 0; currentCol < 9; currentCol++)
                {
                    readPuzzle[currentRow, currentCol] = tempCharPuzzle[counter];
                    puzzleCopy[currentRow, currentCol] = int.Parse(tempCharPuzzle[counter].ToString());
                    counter++;
                }
            }

            CharArrToText(readPuzzle);
            toolStripFileName.Text = "Medium puzzle";
        }

        // New - Hard
        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newPuzzle = IO.loadPresetPuzzle(2, "MainForm");
            char[] tempCharPuzzle = newPuzzle.ToCharArray();
            char[,] readPuzzle = new char[9, 9];
            int counter = 0;

            for (int currentRow = 0; currentRow < 9; currentRow++)
            {
                for (int currentCol = 0; currentCol < 9; currentCol++)
                {
                    readPuzzle[currentRow, currentCol] = tempCharPuzzle[counter];
                    puzzleCopy[currentRow, currentCol] = int.Parse(tempCharPuzzle[counter].ToString());
                    counter++;
                }
            }

            CharArrToText(readPuzzle);
            toolStripFileName.Text = "Hard puzzle";
        }

        // New - Very Hard
        private void veryHardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newPuzzle = IO.loadPresetPuzzle(3, "MainForm");
            char[] tempCharPuzzle = newPuzzle.ToCharArray();
            char[,] readPuzzle = new char[9, 9];
            int counter = 0;

            for (int currentRow = 0; currentRow < 9; currentRow++)
            {
                for (int currentCol = 0; currentCol < 9; currentCol++)
                {
                    readPuzzle[currentRow, currentCol] = tempCharPuzzle[counter];
                    puzzleCopy[currentRow, currentCol] = int.Parse(tempCharPuzzle[counter].ToString());
                    counter++;
                }
            }

            CharArrToText(readPuzzle);
            toolStripFileName.Text = "Very Hard puzzle";
        }

        // New - Custom(blank)
        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BoardClear();
            PuzzleInfoBox.Text = "Custom puzzle";
            toolStripFileName.Text = "Custom puzzle";
            stopW1.Stop();
            stopW1.Reset();
            puzzleCopy = blankPuzzle;
        }
        
        // String used when saving a puzzle
        private string toolStrName = "";

        // Save As 
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] saveArray = new int[9, 9];
            TextToArr(saveArray);

            string emptyStr = "";
            int countRow = 0;
            int countCol = 0;

            foreach (var x in saveArray)
            {
                if (countCol == 0)
                {
                    emptyStr += "[";
                }

                if (countCol < 8)
                {
                    emptyStr += x + ",";
                    countCol++;
                }
                else if (countCol == 8)
                {
                    emptyStr += x + "]\r\n";
                    countRow++;
                    countCol = 0;
                }
            }

            toolStrName = toolStripFileName.Text;
            string fileName = IO.PuzzleSaveAs(emptyStr, toolStrName);
            toolStripFileName.Text = fileName;
        }

        // Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[,] saveArray = new int[9, 9];
            TextToArr(saveArray);

            string emptyStr = "";
            int countRow = 0;
            int countCol = 0;

            foreach (var x in saveArray)
            {
                if (countCol == 0)
                {
                    emptyStr += "[";
                }

                if (countCol < 8)
                {
                    emptyStr += x + ",";
                    countCol++;
                }
                else if (countCol == 8)
                {
                    emptyStr += x + "]\r\n";
                    countRow++;
                    countCol = 0;
                }
            }

            toolStrName = toolStripFileName.Text;
            string fileName = IO.PuzzleSave(emptyStr, toolStrName);
            toolStripFileName.Text = fileName;
        }

        // Load
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string loadStr;
                loadStr = IO.PuzzleLoad();

                string[] splitLoadArr = loadStr.Split('|');
                string fileName = splitLoadArr[0];
                string fileContent = splitLoadArr[1];

                string puzzleStr = fileContent;
                puzzleStr.Trim();
                puzzleStr = puzzleStr.Replace("\r\n", "");

                string[] puzzleRows = puzzleStr.Split(']');


                int[,] puzzle = new int[9, 9];
                int rowCoord = 0;
                int colCoord = 0;

                foreach (string row in puzzleRows)
                {
                    string cleanRow = row.Replace("[", "");
                    string[] puzzleColumn = cleanRow.Split(',');

                    foreach (string x in puzzleColumn)
                    {
                        if (x != "")
                        {
                            puzzle[rowCoord, colCoord] = int.Parse(x);

                            colCoord++;
                        }
                    }
                    rowCoord++;
                    colCoord = 0;
                }


                RichBoxArray();
                BoardClear();
                for (int i = 0; i < 9; i++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        if (puzzle[i, x] != 0)
                        {
                            richboxes[i, x].Text = puzzle[i, x].ToString();
                        }
                        else
                        {
                            richboxes[i, x].Text = "";
                        }
                    }
                }

                puzzleCopy = puzzle.Clone() as int[,];

                toolStripFileName.Text = fileName;
                stopW1.Stop();
                stopW1.Reset();
                stopW1.Start();
            }
            catch
            {
                PuzzleInfoBox.Text = "Invalid text file";
                stopW1.Stop();
                stopW1.Reset();
            }
        }

        #endregion puzzle I/O

        #endregion Toolstrip



        #region Form objects

        /// <summary>
        /// The solver button. Most form logic is executed when this button is clicked.
        /// </summary>
        private void solveBtn_Click(object sender, EventArgs e)
        {
            Stopwatch algoSW = new Stopwatch();
            algoSW.Start();

            // Int array data structure
            int[,] setPuzzle = new int[9, 9]; // New blank array
            Array.Copy(blankPuzzle, setPuzzle, 9); // Fills array with 0's

            PuzzleInfoBox.Text = ""; // Initialises the info text box to be blank

            // Char array data structure
            char[,] charPuzzle = new char[9, 9]; // New blank char array

            // Iterates through the textboxes placing numbers into a 2d integer array
            if (!TextToArr(setPuzzle))
            {
                return;
            }

            foreach (int a in setPuzzle)
            {
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        int charNum = (char)setPuzzle[x, y] + (char)48;
                        charPuzzle[x, y] = (char)charNum;
                    }
                }
            }

            // Checkbox - If checked then puzzle solvability is checked
            if (checkBoxPuzzle.Checked) // Used to check if custom puzzle is solvable
            {
                try
                {
                    if (PuzzleValidation.puzzleCorrect(charPuzzle))
                    {
                        PuzzleInfoBox.Text = "Puzzle solved";
                        stopW1.Stop();
                        return;
                    }
                    else if (PuzzleValidation.puzzleStillValid(charPuzzle, out List<int> dupIndexes))
                    {
                        PuzzleInfoBox.Text = "Puzzle solvable";
                        return;
                    }
                    else
                    {
                        PuzzleInfoBox.Text = "Puzzle unsolvable";
                        return;
                    }
                }
                catch
                {
                    PuzzleInfoBox.Text = "Timeout error";
                }
            }


            // If statements control the solver that is selected
            // in "Select Solver" in the tool strip            
            if (manualMethodTS.Checked) // Manual user inputs 
            {
                if (PuzzleValidation.puzzleCorrect(charPuzzle))
                {
                    stopW1.Stop();
                    TimeSpan ts = stopW1.Elapsed;
                    string elapsedTimeStr = ts.ToString("hh\\:mm\\:ss\\.ff");
                    PuzzleInfoBox.Text = $"Puzzle solved in: {elapsedTimeStr}";
                    return;
                }
                else
                {
                    PuzzleInfoBox.Text = "Puzzle unsolved";
                }
                return;
            }
            if (bruteForceMethodTS.Checked) // Brute force method
            {
                stopW1.Stop();
                stopW1.Reset();
                stopW1.Start();
                if (BruteForce.SudokuSolve(setPuzzle, 0, 0)) // Calls brute force method on the 2d array
                {
                    stopW1.Stop();
                    string executeTime = AlgorithmExecuteTime();

                    ArrToText(setPuzzle);
                    PuzzleInfoBox.Text = $"Puzzle solved in: {executeTime}s"; // Must occur AFTER the text to array
                }
                else
                {
                    PuzzleInfoBox.Text = "Algorithm failed";
                }
                return;
            }
            if (depthFirstMethodTS.Checked)
            {
                stopW1.Stop();
                stopW1.Reset();

                try
                {
                    stopW1.Start();
                    DepthFirst depthfirst = new DepthFirst();
                    stopW1.Stop();
                    charPuzzle = depthfirst.SolveSudoku(charPuzzle);
                    string executeTime = AlgorithmExecuteTime();

                    CharArrToText(charPuzzle);
                    PuzzleInfoBox.Text = $"Puzzle solved in: {executeTime}s"; // Must occur AFTER the text to array
                }
                catch 
                {
                    PuzzleInfoBox.Text = "Error caught";
                }

                return;
            }
            if (bitwiseToolStripMenuItem.Checked)
            {
                stopW1.Stop();
                stopW1.Reset();

                try
                {
                    stopW1.Start();
                    Bitwise newBitwiseInstance = new Bitwise(charPuzzle);
                    stopW1.Stop();

                    string executeTime = AlgorithmExecuteTime();

                    CharArrToText(charPuzzle);
                    PuzzleInfoBox.Text = $"Puzzle solved in: {executeTime}s"; // Must occur AFTER the text to array
                }
                catch 
                {
                    PuzzleInfoBox.Text = "Error caught";
                }

                return;
            }
            if (soleCandidateToolStripMenuItem.Checked)
            {
                stopW1.Stop();
                stopW1.Reset();

                try
                {
                    SoleCandidate soleCandidateInstance = new SoleCandidate();
                    Dictionary<int, LinkedList<SoleCandidate.NodeData>> treePath = new Dictionary<int, LinkedList<SoleCandidate.NodeData>>();

                    stopW1.Start();
                    if (soleCandidateInstance.solveSudoku(ref charPuzzle, treePath))
                    {
                        stopW1.Stop();
                        string executeTime = AlgorithmExecuteTime();

                        CharArrToText(charPuzzle);
                        PuzzleInfoBox.Text = $"Puzzle solved in: {executeTime}s"; // Must occur AFTER the text to array
                                               
                        LinkedList<SoleCandidate.NodeData> successfulTree = treePath.Last().Value;
                        int valCount = treePath.Values.Count;
                        string finalTree = $"Succesful tree:\r\n";
                        foreach (var node in successfulTree)
                        {
                            int tempInt = node.nodeIndex % 9; // Retrieves the [,column] value
                            int column = tempInt;

                            tempInt = ((node.nodeIndex - tempInt) / 9); // Retrieves the [row,] value
                            int row = tempInt;

                            string indexVal = $"[{row},{column}]{node.nodeValue}";
                            finalTree += indexVal;

                            if (node.nodeIndex != successfulTree.Last.Value.nodeIndex)
                            {
                                finalTree += " - ";
                            }
                        }
                        finalTree += $"\r\nPuzzle required {valCount} decision tree branches";
                        MessageBox.Show(finalTree);
                    }
                    else
                    {
                        PuzzleInfoBox.Text = "Algorithm failed";
                    }
                }
                catch (Exception except)
                {
                    PuzzleInfoBox.Text = "Error caught";
                    MessageBox.Show(except.ToString());
                }

                return;
            }
            if (soleCandidateHiddenSinglesToolStripMenuItem.Checked)
            {
                stopW1.Stop();
                stopW1.Reset();

                try
                {
                    SoleCandidateHiddenSingles hiddenSingles = new SoleCandidateHiddenSingles();
                    Dictionary<int, LinkedList<SoleCandidateHiddenSingles.NodeData>> treePath = new Dictionary<int, LinkedList<SoleCandidateHiddenSingles.NodeData>>();

                    stopW1.Start();
                    if (hiddenSingles.solveSudoku(ref charPuzzle, treePath))
                    {
                        stopW1.Stop();
                        string executeTime = AlgorithmExecuteTime();

                        CharArrToText(charPuzzle);
                        PuzzleInfoBox.Text = $"Puzzle solved in: {executeTime}s"; // Must occur AFTER the text to array

                        LinkedList<SoleCandidateHiddenSingles.NodeData> successfulTree = treePath.Last().Value;
                        int valCount = treePath.Values.Count;
                        string finalTree = $"Succesful tree:\r\n";
                        foreach (var node in successfulTree)
                        {
                            int tempInt = node.nodeIndex % 9; // Retrieves the [,column] value
                            int column = tempInt;

                            tempInt = ((node.nodeIndex - tempInt) / 9); // Retrieves the [row,] value
                            int row = tempInt;

                            string indexVal = $"[{row},{column}]{node.nodeValue}";
                            finalTree += indexVal;

                            if (node.nodeIndex != successfulTree.Last.Value.nodeIndex)
                            {
                                finalTree += " - ";
                            }
                        }
                        finalTree += $"\r\nPuzzle required {valCount} decision tree branches";
                        MessageBox.Show(finalTree);
                    }
                    else
                    {
                        PuzzleInfoBox.Text = "Algorithm failed";
                    }
                }
                catch (Exception except)
                {
                    PuzzleInfoBox.Text = "Error caught";
                    MessageBox.Show(except.ToString());
                }

                return;
            }
        }


        /// <summary>
        /// Resets puzzle to beginning state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetBtn_Click(object sender, EventArgs e)
        {
            PuzzleInfoBox.Text = "Puzzle reset";
            ResetPuzzle();
            stopW1.Stop();
            stopW1.Reset();
            stopW1.Start();
            return;
        }


        /// <summary>
        /// Sets the user created puzzle so that the reset button works with it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setBtn_Click(object sender, EventArgs e)
        {
            RichBoxArray();
            TextToArr(puzzleCopy);

            PuzzleInfoBox.Text = "Puzzle set successfully";
            return;
        }

        #endregion Form objects



        #region Form Logic   

        /// <summary>
        /// Validates the input when text is entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sudokuTextBox_TextChanged(object sender, EventArgs e)
        {
            RichTextBox richBox = (RichTextBox)sender;
            richBox.SelectAll();
            richBox.SelectionAlignment = HorizontalAlignment.Center;
            richBox.DeselectAll();

            if (Regex.IsMatch(richBox.Text, "[^1-9]")) // Limits inputs to numerical 1-9, textbox settings ensure only 1 character and no keyboard shortcuts
            {
                MessageBox.Show("Please enter only numbers 1 to 9.");
                richBox.Text = richBox.Text.Remove(richBox.Text.Length - 1);
                return;
            }
            else if (richBox.Text != "") // Colour text if invalid input
            {
                colourChangeInvalid();
                return;
            }
            else
            {
                colourChangeInvalid();
            }
        }


        /// <summary>
        /// Executes on textchanged event. Highlights any constraint violators.
        /// </summary>
        private void colourChangeInvalid()
        {
            RichBoxArrayInitalisedObj(out RichTextBox[,] boxArray);
            RichBoxArray();

            List<int> dupIndexes = new List<int>();
            Dictionary<int, int> boxDictionary = new Dictionary<int, int>();
            int counter = 0;
            foreach (var box in boxArray)
            {
                box.ForeColor = Color.Black;
                if (box.Text != "")
                {
                    boxDictionary.Add(counter, int.Parse(box.Text));
                }
                else
                {
                    boxDictionary.Add(counter, 0);
                }
                counter++;
            }
            
            foreach (var index in boxDictionary)
            {
                if (index.Value == 0)
                    continue;

                indexTranslate(index.Key, out int row, out int column);

                int startRow = (row / 3) * 3;
                int startCol = (column / 3) * 3;

                for (int i = 0; i < 9; i++) // Counter for row, column, and grid cell
                {
                    coordTranslate(out int rowIndex, i, column); // Retrieve row index
                    coordTranslate(out int colIndex, row, i); // Retrieve column index
                    coordTranslate(out int blockIndex, startRow + (i % 3), startCol + (i / 3)); // Retrieve block index

                    if (boxDictionary.ElementAt(rowIndex).Key != index.Key && boxDictionary.ElementAt(rowIndex).Value == index.Value)
                        dupIndexes.Add(rowIndex);
                    if (boxDictionary.ElementAt(colIndex).Key != index.Key && boxDictionary.ElementAt(colIndex).Value == index.Value)
                        dupIndexes.Add(colIndex);
                    if (boxDictionary.ElementAt(blockIndex).Key != index.Key && boxDictionary.ElementAt(blockIndex).Value == index.Value)
                        dupIndexes.Add(blockIndex);
                }
            }

            dupIndexes = dupIndexes.Distinct().ToList();
            foreach (int index in dupIndexes)
            {
                indexTranslate(index, out int row, out int column);
                richboxes[row, column].ForeColor = Color.Red;
            }

        }

        public void indexTranslate(int index, out int row, out int column)
        {
            int tempInt = index % 9; // Retrieves the [,column] value
            column = tempInt;

            tempInt = ((index - tempInt) / 9); // Retrieves the [row,] value
            row = tempInt;
        }

        public void coordTranslate(out int index, int row, int column)
        {
            index = (row * 9) + column;
        }

        /// <summary>
        /// Clears the board. Re-populates with input char[,] array values.
        /// </summary>
        private void CharArrToText(char[,] charPuzzle)
        {
            BoardClear();
            RichBoxArray();

            for (int row = 0; row < 9; row++) // Fills RichTextBox array in turn displaying the numbers in the table
            {
                for (int col = 0; col < 9; col++)
                {
                    richboxes[row, col].ReadOnly = false;
                    if (charPuzzle[row, col] != '0')
                    {
                        richboxes[row, col].Text = charPuzzle[row, col].ToString();
                        richboxes[row, col].ReadOnly = true;
                    }
                }
            }
        }


        /// <summary>
        /// Saves the set puzzle to allow for reset.
        /// </summary>
        private int[,] puzzleCopy = {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

        /// <summary>
        /// Resets the puzzle to it's original state.
        /// </summary>
        private void ResetPuzzle()
        {
            BoardClear();
            RichBoxArray();
            ArrToText(puzzleCopy);
        }

        /// <summary>
        /// Blank int[9,9] array.
        /// </summary>
        private int[,] blankPuzzle = {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };


        #region richtextbox array initialisation
        /// <summary>
        /// Initialises the forms richtextbox grid into a RichTextBox[9,9] array so the .Text values may be set/retrieved.
        /// Allows for the readonly property of each textbox to be set independantly.
        /// </summary>
        private RichTextBox[,] richboxes;

        /// <summary>
        /// Initialises the forms richtextbox grid into a RichTextBox[9,9] array so the .Text values may be set/retrieved.
        /// Allows for the readonly property of each textbox to be set independantly.        /// 
        /// </summary>
        public void RichBoxArray()
        {
            richboxes = new RichTextBox[,] {
                        {richBox00, richBox01, richBox02, richBox03, richBox04, richBox05, richBox06, richBox07, richBox08},
                        {richBox10, richBox11, richBox12, richBox13, richBox14, richBox15, richBox16, richBox17, richBox18},
                        {richBox20, richBox21, richBox22, richBox23, richBox24, richBox25, richBox26, richBox27, richBox28},
                        {richBox30, richBox31, richBox32, richBox33, richBox34, richBox35, richBox36, richBox37, richBox38},
                        {richBox40, richBox41, richBox42, richBox43, richBox44, richBox45, richBox46, richBox47, richBox48},
                        {richBox50, richBox51, richBox52, richBox53, richBox54, richBox55, richBox56, richBox57, richBox58},
                        {richBox60, richBox61, richBox62, richBox63, richBox64, richBox65, richBox66, richBox67, richBox68},
                        {richBox70, richBox71, richBox72, richBox73, richBox74, richBox75, richBox76, richBox77, richBox78},
                        {richBox80, richBox81, richBox82, richBox83, richBox84, richBox85, richBox86, richBox87, richBox88}
                        };
        }
        #endregion richtextbox array initialisation

        public void RichBoxArrayInitalisedObj(out RichTextBox[,] richboxes)
        {
            richboxes = new RichTextBox[,] {
                        {richBox00, richBox01, richBox02, richBox03, richBox04, richBox05, richBox06, richBox07, richBox08},
                        {richBox10, richBox11, richBox12, richBox13, richBox14, richBox15, richBox16, richBox17, richBox18},
                        {richBox20, richBox21, richBox22, richBox23, richBox24, richBox25, richBox26, richBox27, richBox28},
                        {richBox30, richBox31, richBox32, richBox33, richBox34, richBox35, richBox36, richBox37, richBox38},
                        {richBox40, richBox41, richBox42, richBox43, richBox44, richBox45, richBox46, richBox47, richBox48},
                        {richBox50, richBox51, richBox52, richBox53, richBox54, richBox55, richBox56, richBox57, richBox58},
                        {richBox60, richBox61, richBox62, richBox63, richBox64, richBox65, richBox66, richBox67, richBox68},
                        {richBox70, richBox71, richBox72, richBox73, richBox74, richBox75, richBox76, richBox77, richBox78},
                        {richBox80, richBox81, richBox82, richBox83, richBox84, richBox85, richBox86, richBox87, richBox88}
                        };
        }

        /// <summary>
        /// Clears the board. Re-populates with input int[,] array values.
        /// </summary>
        /// <param name="setPuzzle"></param>
        private void ArrToText(int[,] setPuzzle)
        {
            BoardClear();
            RichBoxArray();

            for (int row = 0; row < 9; row++) // Fills RichTextBox array in turn displaying the numbers in the table
            {
                for (int col = 0; col < 9; col++)
                {
                    richboxes[row, col].ReadOnly = false;
                    if (setPuzzle[row, col] != 0)
                    {
                        richboxes[row, col].Text = setPuzzle[row, col].ToString();
                        richboxes[row, col].ReadOnly = true;
                    }
                }
            }
        }

        /// <summary>
        /// Text to array.
        /// This method retrieves the text from each textbox in the grid and creates a 2d integer array.
        /// This method then validates the array, ensuring the puzzle is valid and within the rules.
        /// </summary>
        private bool TextToArr(int[,] setPuzzle)
        {
            RichBoxArrayInitalisedObj(out RichTextBox[,] richboxes);
            int setRow = 0, setCol = 0; // row, col = x, y

            foreach (var a in richboxes)
            {
                int boxNum = 0;

                if (a.Text != "")
                {
                    boxNum = int.Parse(a.Text);
                }

                if (boxNum != 0)
                    setPuzzle[setRow, setCol] = boxNum; 
                
                /*
                // Responsible for seeking duplicate numbers
                if (boxNum != 0 && !BruteForce.IsAvailable(setPuzzle, setRow, setCol, boxNum))
                {
                    PuzzleInfoBox.Text = "Invalid Puzzle";
                    return false;
                }
                else if (boxNum != 0 && BruteForce.IsAvailable(setPuzzle, setRow, setCol, boxNum))
                {
                    setPuzzle[setRow, setCol] = boxNum;
                    a.ForeColor = Color.Black;
                }*/

                setCol++; // x-axis
                if (setCol == 9)
                {
                    setRow++; // y-axis
                    setCol = 0;
                }
            }

            PuzzleInfoBox.Text = "";
            return true;
        }

        /// <summary>
        /// Clears the board.
        /// </summary>
        private void BoardClear()
        {
            RichBoxArray();
            foreach (RichTextBox richbox in richboxes)
                richbox.Text = "";
        }

        /// <summary>
        /// Clears the info text box on the checkbox checkchanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxPuzzle_CheckedChanged(object sender, EventArgs e)
        {
            PuzzleInfoBox.Text = "";
        }

        /// <summary>
        /// Stopwatch and timer keep the form updating in the background and allows for a realtime counter.
        /// </summary>
        System.Windows.Forms.Timer time1;
        Stopwatch stopW1 = new Stopwatch();

        /// <summary>
        /// UI timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUI_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopW1.Elapsed;
            string elapsedTimeStr = ts.ToString("hh\\:mm\\:ss\\.ff");
            statusStripTimer.Text = elapsedTimeStr;
        }

        /// <summary>
        /// Timer for algorithm (Form only not the same one used by the algorithm analytics)
        /// </summary>
        /// <returns></returns>
        private string AlgorithmExecuteTime() 
        {
            stopW1.Stop();
            TimeSpan ts = stopW1.Elapsed;
            string elapsedTimeStr = ts.ToString("ss\\.fffffff");
            return elapsedTimeStr;
        }

        #endregion Form Logic



        #region Auto generated events
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void masterTable_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox81_TextChanged(object sender, EventArgs e)
        {

        }

        private void topLeftPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void solutionMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PuzzleInfoBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void backtracingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripTimer_Click(object sender, EventArgs e)
        {

        }


        #endregion
    }
}
