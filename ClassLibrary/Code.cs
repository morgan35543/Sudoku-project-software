using Algorithms;
using Code;
using PuzzleCreation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// 3 namespaces' in 1 .cs file. 
/// </summary>

namespace Algorithms
{
    /// <summary>
    /// Bruteforce algorithm.
    /// A simple algorithm designed to solve a Sudoku through sheer processing power. 
    /// </summary>
    public class BruteForce // Uses recursion to check and populate each cell
    {
        public static bool SudokuSolve(int[,] puzzle, int row, int col) // When  in a row may return the array[,] has been fi;ed, the  .
        {
            if (row < 9 && col < 9) // Within grid boundaries (Base case)
            {
                if (puzzle[row, col] != 0) // Real value at co-ordinates
                {
                    if ((col + 1) < 9) // Can do recursion on col+1 with no error
                    {
                        return SudokuSolve(puzzle, row, col + 1); // Recursion on the next column postion
                    }
                    else if ((row + 1) < 9) // Can do recursion on row+1 with no error
                    {
                        return SudokuSolve(puzzle, row + 1, 0);
                    }
                    else // Final position already populated - Should be solved
                    {
                        return true;
                    }
                }
                else
                {
                    for (int i = 0; i < 9; i++) // Checking integers 1-9 against position
                    {
                        if (IsAvailable(puzzle, row, col, i + 1)) // Check i + 1 (1-9) is available (un-used row, col, 3x3)
                        {
                            puzzle[row, col] = i + 1; // Sets co-ordinate as i + 1 as it's availble

                            if ((col + 1) < 9) // Check is not final column
                            {
                                if (SudokuSolve(puzzle, row, col + 1)) // Recursion begins at next column position
                                {
                                    return true; // Only returns if recursion no longer possible
                                }
                                else // Will force a False return and cause a step back in recursion (To previous coordinate)
                                {
                                    puzzle[row, col] = 0;
                                }
                            }
                            else if ((row + 1) < 9) // Check is not final row
                            {
                                if (SudokuSolve(puzzle, row + 1, 0)) // Recursion begins at next row
                                {
                                    return true; // Only returns if recursion no longer possible
                                }
                                else // Will force a False return and cause a step back in recursion (To previous coordinate)
                                {
                                    puzzle[row, col] = 0;
                                }
                            }
                            else // Final position populated - Should be solved
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsAvailable(int[,] puzzleCheck, int currentRow, int currentCol, int numChecked)
        {
            // numChecked = Integer to be checked for previous use in the row, column and 3x3 grid            

            // Defines 3x3 grid positional co-ordinates
            int startRow = (currentRow / 3) * 3;
            int startCol = (currentCol / 3) * 3;

            for (int i = 0; i < 9; i++) // Counter for row, column, and grid cell
            {
                if (puzzleCheck[currentRow, i] == numChecked) // Checks row for number
                {
                    return false;
                }
                if (puzzleCheck[i, currentCol] == numChecked) // Checks column for number
                {
                    return false;
                }
                if (puzzleCheck[startRow + (i % 3), startCol + (i / 3)] == numChecked) // Checks 3x3 grid for number
                {
                    return false;
                }
            }

            return true;
        }
    }


    /// <summary>
    /// Depth-first search algorithm.
    /// Jianminchen (2018) Leetcode.
    /// Modified to select one of the available digits at random so as to be used in puzzle generation.
    /// Available online at: https://leetcode.com/problems/sudoku-solver/discuss/177442/C-depth-search-back-tracking-with-clear-structure
    /// </summary>
    public class DepthFirst
    {
        public char[,] SolveSudoku(char[,] board)
        {
            sudokuSolveHelper(ref board, 0, 0);
            return board;
        }

        private static bool sudokuSolveHelper(ref char[,] board, int row, int col)
        {
            int count = 0;
            foreach (char c in board)
            {
                if (c != '0')
                    count++;
            }

            if (count == 81)
            {
                return true;
            }
            
            var isLastColumn = col == 8;
            int nextRow = isLastColumn ? (row + 1) : row;
            int nextCol = isLastColumn ? 0 : col + 1;

            var current = board[row, col];

            bool isDot = current == 48;
            bool isNumber = (current >= 49) && (current <= 57);
                      
            if (isDot)
            {
                List<char> available = getAvailableDigits(board, row, col).ToList();
                Random randomNum = new Random();
                int chosenNum = randomNum.Next(0, available.Count);
                if (available.Count != 0)
                {
                    board[row, col] = available.ElementAt(chosenNum);
                }
                else
                {
                    board[row, col] = '0';
                    return false;
                }

                while (!sudokuSolveHelper(ref board, nextRow, nextCol))
                {
                    available.RemoveAt(chosenNum);
                    chosenNum = randomNum.Next(0, available.Count);

                    if (available.Count != 0)
                    {
                        board[row, col] = available.ElementAt(chosenNum);
                    }
                    else
                    {
                        board[row, col] = '0';
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return sudokuSolveHelper(ref board, nextRow, nextCol);
            }
        }

        private static IEnumerable<char> getAvailableDigits(char[,] board, int currentRow, int currentCol)
        {
            var hashSet = new HashSet<char>("123456789".ToCharArray());

            for (int index = 0; index < 9; index++)
            {
                hashSet.Remove(board[currentRow, index]);
                hashSet.Remove(board[index, currentCol]);
            }

            var smallRow = currentRow / 3 * 3;
            var smallCol = currentCol / 3 * 3;
            for (int row = smallRow; row < smallRow + 3; row++)
            {
                for (int col = smallCol; col < smallCol + 3; col++)
                {
                    hashSet.Remove(board[row, col]);
                }
            }

            return hashSet;
        }
    }


    /// <summary>
    /// Bitwise backtracking algorithm.
    /// Zuche123 (2019) Leetcode.
    /// Available online at: https://leetcode.com/problems/sudoku-solver/discuss/352204/C-faster-than-99.36-using-bitwise-operations-for-number-storage
    /// </summary>
    public class Bitwise
    {
        private int[] rows;
        private int[] cols;
        private int[] cubes;
        private bool solved = false;

        public Bitwise(char[,] board)
        {
            SolveSudoku(board);
        }

        private void SolveSudoku(char[,] board)
        {
            rows = new int[9];
            cols = new int[9];
            cubes = new int[9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j] != '0')
                    {
                        int cube = determineCube(i, j);
                        int num = board[i, j] - '0';

                        addNumber(i, j, cube, board, num);
                    }
                }
            }

            backtrack(board, 0, 0);
        }

        private void backtrack(char[,] b, int row, int col)
        {
            if (b[row, col] == '0')
            {
                int cube = determineCube(row, col);
                for (int d = 1; d < 10; d++)
                {
                    if (placeNumber(row, col, cube, b, d))
                    {
                        helper(row, col, b);

                        if (!solved)
                        {
                            removeNumber(row, col, cube, b, d);
                        }
                    }
                }
            }
            else
            {
                helper(row, col, b);
            }
        }

        private void helper(int row, int col, char[,] b)
        {
            if (col == 8 && row == 8)
            {
                solved = true;
            }
            else
            {
                if (col == 8)
                {
                    backtrack(b, row + 1, 0);
                }
                else
                {
                    backtrack(b, row, col + 1);
                }
            }
        }

        private int determineCube(int row, int col)
        {
            return (row / 3) * 3 + col / 3;
        }

        private bool placeNumber(int row, int col, int cube, char[,] b, int d)
        {
            int check = ((rows[row] >> d) & 1) + ((cols[col] >> d) & 1) + ((cubes[cube] >> d) & 1);


            if (check == 0)
            {
                addNumber(row, col, cube, b, d);
                return true;
            }
            return false;
        }

        private void addNumber(int row, int col, int cube, char[,] b, int d)
        {
            rows[row] |= 1 << d;
            cols[col] |= 1 << d;
            cubes[cube] |= 1 << d;

            b[row, col] = (char)(d + 48);
        }

        private void removeNumber(int row, int col, int cube, char[,] b, int d)
        {
            rows[row] ^= 1 << d;
            cols[col] ^= 1 << d;
            cubes[cube] ^= 1 << d;
            b[row, col] = '0';
        }
    }


    /// <summary>
    /// Algorithm designed and created for this project.
    /// Sole candidate using naked singles only.
    /// Rates puzzles based on total search space.
    /// </summary>
    public class SoleCandidate
    {
        public bool solveSudoku(ref char[,] board, Dictionary<int, LinkedList<NodeData>> treeDict)
        {
            LinkedList<NodeData> currentTreePath = new LinkedList<NodeData>();
            int node = 0;
            if (recursiveSolver(board, ref treeDict, currentTreePath, ref node))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Retrieve candidate lists for each cell and place lists into a dictionary (Each time the method is called - recursive).
        /// Order the dictionary in ascending order based upon the value.count not the key.
        /// Find cells with only 1 candidate and fill first.
        /// If none exist find the first cell with 2 candidates and test each number/branch.
        /// If a cell has no candidates roll-back previous step.
        /// 
        /// Branch-and-Bound technique means the solution can be mapped to a decision tree.
        /// </summary>
        /// <param name="board"></param>
        public bool recursiveSolver(char[,] board, ref Dictionary<int, LinkedList<NodeData>> treeDict, LinkedList<NodeData> currentTreePath, ref int node)
        {
            Dictionary<int, List<int>> candidateLists = new Dictionary<int, List<int>>();
            List<int> indexesUnfilled = new List<int>();
            int filledCount = 0;
            int target = 81;

            // Loops through all 81 cells and finds the candidate list of each
            for (int dictIndex = 0; dictIndex < 81; dictIndex++)
            {
                indexesUnfilled.Add(dictIndex);
                List<int> candidates = new List<int>();
                indexTranslate(dictIndex, out int row, out int column);

                if (board[row, column] != '0') // Current tree will have all nodes filled up to selected
                {
                    indexesUnfilled.Remove(dictIndex);
                    candidates.Add(int.Parse(board[row, column].ToString()));
                    candidateLists.Add(dictIndex, candidates);
                    filledCount++;
                }
                else
                {
                    candidates = retrieveCandidates(board, dictIndex);
                    candidateLists.Add(dictIndex, candidates);
                }

                if (candidates.Count == 0)
                    return false;
            }

            // May only return true when
            if (filledCount == target)
                return true;

            // Dictionary to ascending order based on value.count (List.count)
            var orderedLists = candidateLists.OrderBy(d => d.Value.Count).Select(d => new
            {
                Index = d.Key,
                Candidates = d.Value
            });
        
            // Retrieves the first candidate list with the lowest number of candidates (More constrained search = theoretically faster runtime)
            int count = 0;
            while (!indexesUnfilled.Contains(orderedLists.ElementAt(count).Index))
            {
                count++;
            }

            var treeNode = orderedLists.ElementAt(0);
            for (int index = 0; index < 81; index++)
            {
                treeNode = orderedLists.ElementAt(index); 
                if (indexesUnfilled.Contains(treeNode.Index))
                {
                    bool cellFilled = false;
                    for (int candidateIndex = 0; candidateIndex < treeNode.Candidates.Count; candidateIndex++)
                    {
                        NodeData curNode = new NodeData(treeNode.Index, treeNode.Candidates[candidateIndex]);
                        currentTreePath.AddLast(curNode);

                        indexTranslate(treeNode.Index, out int row, out int column);
                        board[row, column] = treeNode.Candidates[candidateIndex].ToString().ToCharArray()[0];
                        
                        if (recursiveSolver(board, ref treeDict, currentTreePath, ref node))
                        {
                            treeDict.Add(treeDict.Count+1, currentTreePath);
                            cellFilled = true;
                            break;
                        }
                        else
                        {
                            treeDict.Add(treeDict.Count+1, currentTreePath);
                            currentTreePath.Remove(curNode);
                            board[row, column] = '0';
                            cellFilled = false;
                        }
                    }

                    if (cellFilled == false)
                    {
                        return false;
                    }
                    else
                    {
                        return recursiveSolver(board, ref treeDict, currentTreePath, ref node);
                    }
                }
            }

            
            return false;
        }

        /// <summary>
        /// Retrieves a cells candidates return a List ont integers.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="dictIndex"></param>
        /// <returns></returns>
        public List<int> retrieveCandidates(char[,] board, int dictIndex)
        {
            List<int> candidates = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            indexTranslate(dictIndex, out int row, out int column);

            // Obtains 3x3 grid boundaries
            int position2Grid = (row / 3) * 3;
            int position1Grid = (column / 3) * 3;

            for (int searchIndex = 0; searchIndex < 9; searchIndex++)
            {
                int blockRow = position2Grid + (searchIndex / 3);
                int blockCol = position1Grid + (searchIndex % 3);

                if (board[row, searchIndex] != '0')
                    candidates.Remove(int.Parse(board[row, searchIndex].ToString()));
                if (board[searchIndex, column] != '0')
                    candidates.Remove(int.Parse(board[searchIndex, column].ToString()));
                if (board[blockRow, blockCol] != '0')
                    candidates.Remove(int.Parse(board[blockRow, blockCol].ToString()));
            }
            return candidates;
        }

        /// <summary>
        /// Convert the dictionary index to [row,column] array indices.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void indexTranslate(int index, out int row, out int column)
        {
            int tempInt = index % 9; // Retrieves the [,column] value
            column = tempInt;

            tempInt = ((index - tempInt) / 9); // Retrieves the [row,] value
            row = tempInt;
        }

        /// <summary>
        /// A struct to hold a nodes index and value.
        /// </summary>
        public struct NodeData
        {
            public NodeData(int index, int value)
            {
                nodeIndex = index;
                nodeValue = value;
            }

            public int nodeIndex { get; private set; }
            public int nodeValue { get; private set; }
        }
    }


    /// <summary>
    /// Algorithm designed and created for this project.
    /// Sole candidate using hidden and naked singles.
    /// Execution time comparisons can be made for the two solving techniquess.
    /// </summary>
    public class SoleCandidateHiddenSingles
    {
        public bool solveSudoku(ref char[,] board, Dictionary<int, LinkedList<NodeData>> treeDict)
        {
            LinkedList<NodeData> currentTreePath = new LinkedList<NodeData>();
            int node = 0;
            if (recursiveSolver(board, ref treeDict, currentTreePath, ref node))
            {
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// Attempt at implementing hidden single search.
        /// </summary>
        /// <param name="board"></param>
        public bool recursiveSolver(char[,] board, ref Dictionary<int, LinkedList<NodeData>> treeDict, LinkedList<NodeData> currentTreePath, ref int node)
        {
            Dictionary<int, List<int>> candidateLists = new Dictionary<int, List<int>>();
            List<int> indexesUnfilled = new List<int>();
            int filledCount = 0;
            int target = 81;

            // Loops through all 81 cells and finds the candidate list of each
            for (int dictIndex = 0; dictIndex < 81; dictIndex++)
            {
                indexesUnfilled.Add(dictIndex);
                List<int> candidates = new List<int>();
                indexTranslate(dictIndex, out int row, out int column);


                if (board[row, column] != '0') // Current tree will have all nodes filled up to selected
                {
                    indexesUnfilled.Remove(dictIndex);
                    candidates.Add(int.Parse(board[row, column].ToString()));
                    candidateLists.Add(dictIndex, candidates);
                    filledCount++;
                }
                else
                {
                    candidates = retrieveCandidates(board, dictIndex);
                    candidateLists.Add(dictIndex, candidates);
                }

                if (candidates.Count == 0)
                    return false;
            }

            // Base case - All cells filled puzzle then is valid and complete
            if (filledCount == target)
                return true;

            // Converts candidate lsits dictionary to an anonymous data type Key/Value pair, ordered in ascending based on the value.count (List.count)
            var orderedLists = candidateLists.OrderBy(d => d.Value.Count).Select(d => new
            {
                Index = d.Key,
                Candidates = d.Value
            });

            //var treeNode = orderedLists.ElementAt(0);

            for (int index = 0; index < 81; index++)
            {
                var treeNode = orderedLists.ElementAt(index);
                if (indexesUnfilled.Contains(treeNode.Index))
                {
                    //currentTreePath.Add(treeNode.Index); // Legacy, used when it was a List<int>
                    List<int> candidatesRemaining = scanForHidden(candidateLists, treeNode.Candidates, treeNode.Index);

                    bool cellFilled = false;
                    for (int candidateIndex = 0; candidateIndex < candidatesRemaining.Count; candidateIndex++)
                    {
                        NodeData curNode = new NodeData(treeNode.Index, candidatesRemaining[candidateIndex]);
                        currentTreePath.AddLast(curNode);

                        indexTranslate(treeNode.Index, out int row, out int column);
                        board[row, column] = candidatesRemaining[candidateIndex].ToString().ToCharArray()[0];

                        if (recursiveSolver(board, ref treeDict, currentTreePath, ref node))
                        {
                            //node++;
                            treeDict.Add(treeDict.Count+1, currentTreePath);
                            cellFilled = true;
                            break;
                        }
                        else
                        {
                            //node+++;
                            treeDict.Add(treeDict.Count+1, currentTreePath);
                            currentTreePath.Remove(curNode);
                            board[row, column] = '0';
                            cellFilled = false;
                        }
                    }

                    if (cellFilled == false)
                    {
                        //node++;
                        //treeDict.Add(node, currentTreePath);
                        return false;
                    }
                    else
                    {
                        return recursiveSolver(board, ref treeDict, currentTreePath, ref node);
                    }
                }
            }


            return false;
        }

        /// <summary>
        /// Retrieves a cells candidates return a List ont integers.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="dictIndex"></param>
        /// <returns></returns>
        public List<int> retrieveCandidates(char[,] board, int dictIndex)
        {
            List<int> candidates = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            indexTranslate(dictIndex, out int row, out int column);

            // Obtains 3x3 grid boundaries
            int position2Grid = (row / 3) * 3;
            int position1Grid = (column / 3) * 3;

            for (int searchIndex = 0; searchIndex < 9; searchIndex++)
            {
                int blockRow = position2Grid + (searchIndex / 3);
                int blockCol = position1Grid + (searchIndex % 3);

                if (board[row, searchIndex] != '0')
                    candidates.Remove(int.Parse(board[row, searchIndex].ToString()));
                if (board[searchIndex, column] != '0')
                    candidates.Remove(int.Parse(board[searchIndex, column].ToString()));
                if (board[blockRow, blockCol] != '0')
                    candidates.Remove(int.Parse(board[blockRow, blockCol].ToString()));
            }

            return candidates;
        }

        /// <summary>
        /// Convert the dictionary index to [row,column] array indeces.
        /// Also adding +1 to both the row & column provides the grid co-ordinates in place of the array indeces.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void indexTranslate(int index, out int row, out int column)
        {
            int tempInt = index % 9; // Retrieves the [,column] value
            column = tempInt;

            tempInt = ((index - tempInt) / 9); // Retrieves the [row,] value
            row = tempInt;
        }


        /// <summary>
        /// Takes positional coordinates and outputs the index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public void coordTranslate(out int index, int row, int column)
        {
            index = (row * 9) + column;
        }


        /// <summary>
        /// A struct to hold a nodes index and value.
        /// </summary>
        public struct NodeData
        {
            public NodeData(int index, int value)
            {
                nodeIndex = index;
                nodeValue = value;
            }

            public int nodeIndex { get; private set; }
            public int nodeValue { get; private set; }
        }


        /// <summary>
        /// Searches for hidden singles.
        /// </summary>
        /// <param name="candidateLists"></param>
        /// <param name="candidates"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<int> scanForHidden(Dictionary<int, List<int>> candidateLists, List<int> candidates, int index)
        {
            indexTranslate(index, out int row, out int column);
            int startRow = (row / 3) * 3;
            int startCol = (column / 3) * 3;

            List<int> rowCandidatesSeen = new List<int>(); // The candidate that have been seen (Matches to be removed from method parameter "List<int> candidates")
            List<int> colCandidatesSeen = new List<int>();
            List<int> blockCandidatesSeen = new List<int>();

            for (int i = 0; i < 9; i++) // Counter for row, column, and grid cell (Checks 9 blocks)
            {


                coordTranslate(out int rowIndex, i, column); // Retrieve row index
                coordTranslate(out int colIndex, row, i); // Retrieve column index
                coordTranslate(out int blockIndex, startRow + (i % 3), startCol + (i / 3)); // Retrieve block index

                if ((i != row) && candidateLists.TryGetValue(rowIndex, out List<int> rowCandidates)) // Checks row for number
                {
                    rowCandidates = rowCandidates.Except(rowCandidatesSeen).ToList();
                    rowCandidatesSeen.AddRange(rowCandidates);
                }
                if ((i != column) && candidateLists.TryGetValue(colIndex, out List<int> colCandidates)) // Checks column for number
                {
                    colCandidates = colCandidates.Except(colCandidatesSeen).ToList();
                    colCandidatesSeen.AddRange(colCandidates);
                }
                if ((startRow + (i % 3) != row && startCol + (i / 3) != column) && candidateLists.TryGetValue(blockIndex, out List<int> blockCandidates)) // Checks 3x3 grid for number
                {
                    blockCandidates = blockCandidates.Except(blockCandidatesSeen).ToList();
                    blockCandidatesSeen.AddRange(blockCandidates);
                }
            }

            rowCandidatesSeen = candidates.Except(rowCandidatesSeen).ToList();
            colCandidatesSeen = candidates.Except(colCandidatesSeen).ToList();
            blockCandidatesSeen = candidates.Except(blockCandidatesSeen).ToList();

            if (rowCandidatesSeen.Count == 1)
                return rowCandidatesSeen;
            if (colCandidatesSeen.Count == 1)
                return colCandidatesSeen;
            if (blockCandidatesSeen.Count == 1)
                return blockCandidatesSeen;
            else
                return candidates;
        }
    }


    /// <summary>
    /// Puzzle creation.
    /// </summary>
    public class PuzzleCreate
    {
        /// <summary>
        /// Puzzle creation. Uses Depth First search to first create a solved Sudoku then removes numbers at random until the desired amount remain. 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="cellsRemain"></param>
        public char[,] puzzleCreate(int cellsRemain)
        {
            char[,] newboard = Tools.intArrayToChar(new int[9, 9]);

            int count = 81;
            int position2 = 0;
            int position1 = 0;
            Random randomPos = new Random();
            DepthFirst depthfirst = new DepthFirst();
            depthfirst.SolveSudoku(newboard);

            
            while (cellsRemain < count)
            {
                int positionIndex = randomPos.Next(0, 81); // Select random position
                indexToPositions(positionIndex, out position2, out position1);


                if (newboard[position2, position1] != '0')
                {
                    newboard[position2, position1] = '0';
                }
                

                count = 0;
                foreach (char cell in newboard) // Count cells with number removed
                {
                    if (cell != '0')
                        count++;
                }
            }

            return newboard;
        }


        /// <summary>
        /// Absolves using two Random.Next(0,8) resulting in x,y being the same value too often. 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="position2"></param>
        /// <param name="position1"></param>
        private static void indexToPositions(int index, out int position2, out int position1)
        {
            int indexPos1 = index;
            int indexPos2 = 0;

            indexPos1 = index % 9;
            indexPos2 = index / 9;

            position1 = indexPos1;
            position2 = indexPos2;
        }
    }
}

namespace Code
{
    /// <summary>
    /// New instance to be created in console.
    /// </summary>
    public class ConsoleCode
    {
        /// <summary>
        /// Console Code is located in the ClassLibrary LIVE.cs file
        /// </summary>
        public ConsoleCode()
        {
            Console.WriteLine("Sudoku solving algorithm analysis - Morgan Wright - Student number: 201708317" + "\r\n");

            /// <summary>
            /// While loop to allow multiple analysis without closing the application.
            /// All code held within.
            /// </summary>
            bool appOpen = true;
            while (appOpen == true)
            {
                Console.WriteLine("Select puzzle difficulty by typing the corresponding number and pressing enter.");
                Console.WriteLine("1 - Easy");
                Console.WriteLine("2 - Medium");
                Console.WriteLine("3 - Hard");
                Console.WriteLine("4 - Very Hard");
                int puzzleDifficulty = 0;
                string userDifficultyInput = Console.ReadLine();
                while (!int.TryParse(userDifficultyInput, out puzzleDifficulty) && (puzzleDifficulty < 1 || puzzleDifficulty > 4))
                {
                    Console.WriteLine("Invalid input please try again");
                    userDifficultyInput = Console.ReadLine();
                    int.TryParse(userDifficultyInput, out puzzleDifficulty);
                }

                Console.WriteLine("Enter number of puzzles to tests to run (Note each test may take up to a maximum of 2 minutes to complete):");
                string userTestCountInput = Console.ReadLine();
                int testCount = 0;
                while (!int.TryParse(userTestCountInput, out testCount))
                {
                    Console.WriteLine("Invalid input please try again");
                    userTestCountInput = Console.ReadLine();
                    int.TryParse(userTestCountInput, out testCount);
                }


                string puzzleString = IO.loadPresetPuzzle(puzzleDifficulty - 1, "PuzzleCreator");

                for (int testNumber = 0; testNumber < testCount; testNumber++)
                {
                    Console.WriteLine($"Tests Started: {testNumber}");
                    runTimeTest(puzzleDifficulty - 1, puzzleString);


                    //if (testNumber % 25 == 0 && testNumber != 0)
                        Console.WriteLine($"Tests completed: {testNumber}");
                }

            }
            Console.WriteLine("Press any key to exit");
            Console.Read();
            // Application exit.
        }

        private static void runTimeTest(int puzzleDifficulty, string puzzleString) // All ran against same puzzle
        {
            Stopwatch testingWatchBF = new Stopwatch(); // Brute-force
            Stopwatch testingWatchDFS = new Stopwatch(); // Depth-first
            Stopwatch testingWatchBW = new Stopwatch(); // Bitwise
            Stopwatch testingWatchSCN = new Stopwatch(); // Sole candidate naked single
            Stopwatch testingWatchSCH = new Stopwatch(); // Sole candidate naked & hidden singles
            testingWatchBF.Start();
            testingWatchDFS.Start();
            testingWatchBW.Start();
            testingWatchSCN.Start();
            testingWatchSCH.Start();
            char[] stringToCharArr = puzzleString.ToCharArray();
            char[,] charPuzzle = new char[9, 9];
            int[,] intPuzzle = new int[9, 9];
            int counter = 0;
            for (int currentRow = 0; currentRow < 9; currentRow++)
            {
                for (int currentCol = 0; currentCol < 9; currentCol++)
                {
                    charPuzzle[currentRow, currentCol] = stringToCharArr[counter];
                    intPuzzle[currentRow, currentCol] = int.Parse(stringToCharArr[counter].ToString());
                    counter++;
                }
            }
            int[,] bfCopy = intPuzzle.Clone() as int[,];
            char[,] dfsCopy = charPuzzle.Clone() as char[,];
            char[,] bitCopy = charPuzzle.Clone() as char[,];
            char[,] scnCopy = charPuzzle.Clone() as char[,];
            char[,] schCopy = charPuzzle.Clone() as char[,];
            testingWatchBF.Stop();
            testingWatchDFS.Stop();
            testingWatchBW.Stop();
            testingWatchSCN.Stop();
            testingWatchSCH.Stop();
            // Stops the stopwach being inaccurate from starting directly after initialising.
            testingWatchBF.Reset();
            testingWatchDFS.Reset();
            testingWatchBW.Reset();
            testingWatchSCN.Reset();
            testingWatchSCH.Reset();

            DepthFirst depthFirst = new DepthFirst();
            Dictionary<int, LinkedList<SoleCandidate.NodeData>> soleCandTree = new Dictionary<int, LinkedList<SoleCandidate.NodeData>>();
            Dictionary<int, LinkedList<SoleCandidateHiddenSingles.NodeData>> soleCandHiddenTree = new Dictionary<int, LinkedList<SoleCandidateHiddenSingles.NodeData>>();
            SoleCandidate soleCandidate = new SoleCandidate();
            SoleCandidateHiddenSingles soleCandidateHiddenSingles = new SoleCandidateHiddenSingles();

            bool Completed = false;
            int executeCount = 0;
            while (Completed == false)
            {
                if (executeCount != 0)
                {
                    Console.WriteLine("Algorithms took to long. Test Restarted");
                    stringToCharArr = puzzleString.ToCharArray();
                    charPuzzle = new char[9, 9];
                    intPuzzle = new int[9, 9];
                    counter = 0;
                    for (int currentRow = 0; currentRow < 9; currentRow++)
                    {
                        for (int currentCol = 0; currentCol < 9; currentCol++)
                        {
                            charPuzzle[currentRow, currentCol] = stringToCharArr[counter];
                            intPuzzle[currentRow, currentCol] = int.Parse(stringToCharArr[counter].ToString());
                            counter++;
                        }
                    }
                    bfCopy = intPuzzle.Clone() as int[,];
                    dfsCopy = charPuzzle.Clone() as char[,];
                    bitCopy = charPuzzle.Clone() as char[,];
                    scnCopy = charPuzzle.Clone() as char[,];
                    schCopy = charPuzzle.Clone() as char[,];
                }

                Completed = ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(120000), () =>
                {
                    testingWatchBF.Start();
                    BruteForce.SudokuSolve(bfCopy, 0, 0);
                    testingWatchBF.Stop();

                    testingWatchDFS.Start();
                    depthFirst.SolveSudoku(dfsCopy);
                    testingWatchDFS.Stop();

                    testingWatchBW.Start();
                    Bitwise bitwise = new Bitwise(bitCopy);
                    testingWatchBW.Stop();

                    testingWatchSCN.Start();
                    soleCandidate.solveSudoku(ref scnCopy, soleCandTree);
                    testingWatchSCN.Stop();

                    testingWatchSCH.Start();
                    soleCandidateHiddenSingles.solveSudoku(ref schCopy, soleCandHiddenTree);
                    testingWatchSCH.Stop();
                });

                char[,] bfAsChar = new char[9, 9];
                for (int row = 0; row < 9; row++)
                    for (int column = 0; column < 9; column++)
                        bfAsChar[row, column] = bfCopy[row, column].ToString().ToCharArray()[0];

                if (!PuzzleValidation.puzzleCorrect(bfAsChar) || !PuzzleValidation.puzzleCorrect(dfsCopy) || !PuzzleValidation.puzzleCorrect(bitCopy) || !PuzzleValidation.puzzleCorrect(scnCopy) || !PuzzleValidation.puzzleCorrect(schCopy))
                    Completed = false;

                executeCount++;
            }

            string path = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            string folderName = "TestLogs";
            path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(path).FullName).FullName).FullName).FullName;
            path = path + "\\" + folderName;
            string curFile = "";

            string puzzleDifficultyString = "";
            switch (puzzleDifficulty)
            {
                case 0:
                    puzzleDifficultyString = "Easy";
                    break;

                case 1:
                    puzzleDifficultyString = "Medium";
                    break;

                case 2:
                    puzzleDifficultyString = "Hard";
                    break;

                case 3:
                    puzzleDifficultyString = "VeryHard";
                    break;
            }

            // Results csv file
            curFile = path + $"\\SamePuzzle{puzzleDifficultyString}.csv";

            if (!File.Exists(curFile))
            {
                string clientHeader = $"\"Puzzle\",\"BruteForce\",\"DepthFirst\",\"BitWise\",\"NakedSingles\",\"HiddenSingles\",\"NSSearch\",\"HSSearch\"{Environment.NewLine}";
                File.AppendAllText(curFile, clientHeader);
            }

            using (StreamWriter streamWriter = File.AppendText(curFile))
            {
                TimeSpan timeSpanBF = testingWatchBF.Elapsed;
                string timeSpanBFms = timeSpanBF.TotalMilliseconds.ToString();
                TimeSpan timeSpanDF = testingWatchDFS.Elapsed;
                string timeSpanDFms = timeSpanDF.TotalMilliseconds.ToString();
                TimeSpan timeSpanBW = testingWatchBW.Elapsed;
                string timeSpanBWms = timeSpanBW.TotalMilliseconds.ToString();
                TimeSpan timeSpanSCN = testingWatchSCN.Elapsed;
                string timeSpanSCNms = timeSpanSCN.TotalMilliseconds.ToString();
                TimeSpan timeSpanSCH = testingWatchSCH.Elapsed;
                string timeSpanSCHms = timeSpanSCH.TotalMilliseconds.ToString();


                string csvOutput = $"\"{puzzleString}\"" + "," + timeSpanBFms + "," + timeSpanDFms + "," + timeSpanBWms + "," + timeSpanSCNms + "," + timeSpanSCHms + "," + soleCandTree.Count + "," + soleCandHiddenTree.Count;
                streamWriter.WriteLine(csvOutput);
                streamWriter.Flush();
                streamWriter.Close();
            }
            /*
            // PuzzleCopy
            curFile = path + $"\\puzzlesTested{puzzleDifficultyString}.csv";
            using (StreamWriter streamWriter = File.AppendText(curFile))
            {
                streamWriter.WriteLine(puzzleString + ",");
                streamWriter.Flush();
                streamWriter.Close();
            }

            // Bruteforce
            curFile = path + $"\\BruteForce{puzzleDifficultyString}.csv";
            using (StreamWriter streamWriter = File.AppendText(curFile))
            {
                TimeSpan timeSpan = testingWatchBF.Elapsed;
                string timeSpanMS = timeSpan.TotalMilliseconds.ToString();
                streamWriter.WriteLine(timeSpanMS + ",");
                streamWriter.Flush();
                streamWriter.Close();
            }

            // Depth-First
            curFile = path + $"\\DepthFirst{puzzleDifficultyString}.csv";
            using (StreamWriter streamWriter = File.AppendText(curFile))
            {
                TimeSpan timeSpan = testingWatchDFS.Elapsed;
                string timeSpanMS = timeSpan.TotalMilliseconds.ToString();
                streamWriter.WriteLine(timeSpanMS + ",");
                streamWriter.Flush();
                streamWriter.Close();
            }

            // Bitwise
            curFile = path + $"\\Bitwise{puzzleDifficultyString}.csv";
            using (StreamWriter streamWriter = File.AppendText(curFile))
            {
                TimeSpan timeSpan = testingWatchBW.Elapsed;
                string timeSpanMS = timeSpan.TotalMilliseconds.ToString();
                streamWriter.WriteLine(timeSpanMS + ",");
                streamWriter.Flush();
                streamWriter.Close();
            }

            // Naked Singles
            curFile = path + $"\\NakedSingle{puzzleDifficultyString}.csv";
            using (StreamWriter streamWriter = File.AppendText(curFile))
            {
                TimeSpan timeSpan = testingWatchSCN.Elapsed;
                string timeSpanMS = timeSpan.TotalMilliseconds.ToString();
                timeSpanMS += $" - {soleCandTree.Count}";
                streamWriter.WriteLine(timeSpanMS + ",");
                streamWriter.Flush();
                streamWriter.Close();
            }

            // Hidden Singles
            curFile = path + $"\\HiddenSingle{puzzleDifficultyString}.csv";
            using (StreamWriter streamWriter = File.AppendText(curFile))
            {
                TimeSpan timeSpan = testingWatchSCH.Elapsed;
                string timeSpanMS = timeSpan.TotalMilliseconds.ToString();
                timeSpanMS += $" - {soleCandHiddenTree.Count}";
                streamWriter.WriteLine(timeSpanMS + ",");
                streamWriter.Flush();
                streamWriter.Close();
            }*/
        }


        /// <summary>
        /// Shankar_pratap (2012) available online at: https://stackoverflow.com/questions/7413612/how-to-limit-the-execution-time-of-a-function-in-c-sharp [accessed: 15/04/20]
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="codeBlock"></param>
        /// <returns></returns>
        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
            }
        }
    }


    /// <summary>
    /// I/O.
    /// </summary>
    public class IO
    {
        private static string GetPuzzlePath()
        {
            string path = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            string folderName = "SudokuPuzzles";

            path = Directory.GetParent(Directory.GetParent(Directory.GetParent(path).FullName).FullName).FullName;
            path = path + "\\" + folderName;

            return path;
        }

        public static string PuzzleSaveAs(string saveStr, string toolStrName) // Opens a saveFileDialog for choosing save location
        {
            // Code example from documentation used
            // Documentation link:
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.savefiledialog?view=netframework-4.8

            string fileName = "";

            Stream myStream;
            SaveFileDialog saveAsPuzzle = new SaveFileDialog();

            string path = GetPuzzlePath();

            saveAsPuzzle.InitialDirectory = path;
            saveAsPuzzle.Filter = "txt files (*.txt)|*.txt";
            saveAsPuzzle.FilterIndex = 1;
            saveAsPuzzle.RestoreDirectory = true;
            saveAsPuzzle.FileName = toolStrName; // Provides a default name based on puzzle type/difficulty



            if (saveAsPuzzle.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveAsPuzzle.OpenFile()) != null)
                {
                    StreamWriter puzzleWriteStream = new StreamWriter(myStream);
                    puzzleWriteStream.Write(saveStr);
                    puzzleWriteStream.Flush();

                    myStream.Close();
                }
            }

            fileName = Path.GetFileNameWithoutExtension(saveAsPuzzle.FileName);
            return fileName;
        }

        public static string PuzzleSave(string saveStr, string toolStrName) // Saves file if it already exists, opens saveFileDialog if it doesn't
        {
            // Code example from documentation used
            // Documentation link:
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.savefiledialog?view=netframework-4.8

            string fileName = "";
            string path = GetPuzzlePath();

            string curFile = path + "\\" + toolStrName + ".txt";

            if (File.Exists(curFile))
            {
                StreamWriter puzzleWriteStream = new StreamWriter(curFile);
                puzzleWriteStream.Write(saveStr);
                puzzleWriteStream.Flush();
                puzzleWriteStream.Close();
                fileName = toolStrName;
            }
            else
            {
                Stream myStream;
                SaveFileDialog savePuzzle = new SaveFileDialog()
                {
                    InitialDirectory = path,
                    Filter = "txt files (*.txt)|*.txt",
                    FilterIndex = 1,
                    RestoreDirectory = true,
                    FileName = toolStrName // Provides a default name based on puzzle type/difficulty
                };

                if (savePuzzle.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = savePuzzle.OpenFile()) != null)
                    {
                        StreamWriter puzzleWriteStream = new StreamWriter(myStream);
                        puzzleWriteStream.Write(saveStr);
                        puzzleWriteStream.Flush();

                        myStream.Close();
                    }
                }

                fileName = Path.GetFileNameWithoutExtension(savePuzzle.FileName);
            }

            return fileName;
        }

        public static string PuzzleLoad() // Loads selected puzzle
        {
            // Code example from documentation used
            // Documentation link:
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.openfiledialog?view=netframework-4.8

            int[,] puzzle = new int[9, 9];

            string path = GetPuzzlePath();

            var fileContent = string.Empty;
            var filePath = string.Empty;
            string fileName = "";

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;


                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }


            return fileName + "|" + fileContent;
        }


        /// <summary>
        /// 0 - Easy.
        /// 1 - Medium.
        /// 2 - Hard.
        /// 3 - Very Hard.
        /// </summary>
        /// <param name="puzzleDifficulty"></param>
        /// <returns></returns>
        public static string loadPresetPuzzle(int puzzleDifficulty, string program)
        {
            string path = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            string folderName = "PuzzlePresets";

            if (program == "PuzzleCreator")
            {
                path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(path).FullName).FullName).FullName).FullName;
                
            }
            else
            {
                path = Directory.GetParent(Directory.GetParent(Directory.GetParent(path).FullName).FullName).FullName;
            }

            path = path + "\\" + folderName;

            string curFile = "";

            switch (puzzleDifficulty)
            {
                case 0:
                    curFile = path + "\\EasyPresets.txt";
                    break;

                case 1:
                    curFile = path + "\\MediumPresets.txt";
                    break;

                case 2:
                    curFile = path + "\\HardPresets.txt";
                    break;

                case 3:
                    curFile = path + "\\VeryHardPresets.txt";
                    break;
            }

            Random rndGen = new Random();
            int rndInt = 0;
            string[] lines = null;

            using (StreamReader read = new StreamReader(curFile))
            {
                lines = File.ReadAllLines(curFile);

                rndInt = rndGen.Next(0, lines.Length - 1);
            }


            return lines[rndInt];
        }
    }


    /// <summary>
    /// Methods for a few repeatable tasks.
    /// </summary>
    public class Tools
    {
        public static void colourChangePrint(string printString, ConsoleColor colourSelected)
        {
            try { Console.ForegroundColor = colourSelected; }
            finally { Console.WriteLine(printString); }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static char[,] intArrayToChar(int[,] arrayTypeInt)
        {
            char[,] arrayTypeChar = new char[9, 9];
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    int charNum = (char)arrayTypeInt[x, y] + (char)48;
                    arrayTypeChar[x, y] = (char)charNum;
                }
            }

            return arrayTypeChar;
        }

        public static int[,] charArrayToInt(char[,] arrayTypeChar)
        {
            int[,] arrayTypeInt = new int[9, 9];
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    arrayTypeInt[row, col] = arrayTypeChar[row, col] - 48;
                }
            }
            return arrayTypeInt;
        }
    }
}

namespace PuzzleCreation
{
    /// <summary>
    /// Puzzle validation.
    /// </summary>
    public class PuzzleValidation
    {
        /// <summary>
        /// Checks finished puzzle adheres to constraints.
        /// </summary>
        /// <param name="puzzle"></param>
        /// <returns></returns>
        public static bool puzzleCorrect(char[,] puzzle)
        {
            List<int> correctList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int rowIndexer = 0; rowIndexer < 9; rowIndexer++)
            {
                for (int columnIndexer = 0; columnIndexer < 9; columnIndexer++)
                {
                    int startRow = (rowIndexer / 3) * 3;
                    int startCol = (columnIndexer / 3) * 3;
                    List<int> rowSave = new List<int>();
                    List<int> columnSave = new List<int>();
                    List<int> blockSave = new List<int>();

                    for (int i = 0; i < 9; i++) // Counter for row, column, and grid cell
                    {
                        rowSave.Add(int.Parse(puzzle[rowIndexer, i].ToString()));
                        columnSave.Add(int.Parse(puzzle[i, columnIndexer].ToString()));
                        blockSave.Add(int.Parse(puzzle[startRow + (i % 3), startCol + (i / 3)].ToString()));
                    }

                    rowSave.Sort();
                    columnSave.Sort();
                    blockSave.Sort();

                    if (!rowSave.SequenceEqual(correctList) || !columnSave.SequenceEqual(correctList) || !blockSave.SequenceEqual(correctList))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Searches every cell on text change to check puzzle is still valid.
        /// 
        /// Returns list of cells containing duplicate numbers so that the duplicate 
        /// cells may have their colour changed to notify the user.
        /// </summary>
        /// <param name="puzzle"></param>
        /// <param name="dupIndexes"></param>
        /// <returns></returns>
        public static bool puzzleStillValid(char[,] puzzle, out List<int> dupIndexes) 
        {
            dupIndexes = new List<int>();

            for (int rowIndexer = 0; rowIndexer < 9; rowIndexer++)
            {
                for (int columnIndexer = 0; columnIndexer < 9; columnIndexer++)
                {
                    int startRow = (rowIndexer / 3) * 3;
                    int startCol = (columnIndexer / 3) * 3;
                    List<int> rowSave = new List<int>();
                    List<int> columnSave = new List<int>();
                    List<int> blockSave = new List<int>();

                    for (int i = 0; i < 9; i++) // Counter for row, column, and grid cell
                    {
                        if (puzzle[rowIndexer, i] != '0')
                            rowSave.Add(int.Parse(puzzle[rowIndexer, i].ToString()));
                        if (puzzle[i, columnIndexer] != '0')
                            columnSave.Add(int.Parse(puzzle[i, columnIndexer].ToString()));
                        if (puzzle[startRow + (i % 3), startCol + (i / 3)] != '0')
                            blockSave.Add(int.Parse(puzzle[startRow + (i % 3), startCol + (i / 3)].ToString()));
                    }

                    rowSave.Sort(); // Numbers in current cell row
                    columnSave.Sort(); // Numbers in current cell column
                    blockSave.Sort(); // Numbers in current cell block

                    int currentCellInteger = int.Parse(puzzle[rowIndexer, columnIndexer].ToString());

                    // Retrieves indexes of duplicate numbers
                    var rowResult = Enumerable.Range(0, rowSave.Count)
                                .Where(i => rowSave[i] == currentCellInteger)
                                .ToList();

                    var columnResult = Enumerable.Range(0, columnSave.Count)
                                .Where(i => columnSave[i] == currentCellInteger)
                                .ToList();

                    var blockResult = Enumerable.Range(0, blockSave.Count)
                                .Where(i => blockSave[i] == currentCellInteger)
                                .ToList();

                    if (rowResult.Count > 1)
                    {
                        foreach (int index in rowResult)
                        {
                            dupIndexes.Add((rowIndexer * 9) + index);
                        }
                    }
                    if (columnResult.Count > 1)
                    {
                        foreach (int index in columnResult)
                        {
                            dupIndexes.Add((rowIndexer * 9) + columnIndexer + index);
                        }
                    }
                    if (blockResult.Count > 1)
                    {
                        foreach (int index in blockResult)
                        {
                            int miniBlock = index % 3;

                            if (index < 3)
                            {
                                dupIndexes.Add(startRow + startCol + miniBlock);
                            }
                            else if (index < 6)
                            {
                                dupIndexes.Add((startRow + 1) + (startCol + 1) + miniBlock);
                            }
                            else if (index < 9)
                            {
                                dupIndexes.Add((startRow + 2) + (startCol + 2) + miniBlock);
                            }
                        }
                    }
                }
            }

            dupIndexes.Sort();
            dupIndexes = dupIndexes.Distinct().ToList();

            if (dupIndexes.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}