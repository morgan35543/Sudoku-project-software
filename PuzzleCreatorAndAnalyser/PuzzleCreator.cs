using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Code;
using PuzzleCreation;
using Algorithms;
using System.IO;

namespace PuzzleCreatorAndAnalyser
{
    class PuzzleCreator
    {
        static void Main(string[] args)
        {
            CreatorCode();
            Console.WriteLine("Any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Code for puzzle creator.
        /// </summary>
        private static void CreatorCode()
        {
            /// <summary>
            /// Used to create and store unique puzzles based upon the total number of decision tree branches required for a solution.
            /// </summary>

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Enter the number of cycles (positive integers only):");

            string lineIn = Console.ReadLine();
            int cycleNum = 0;
            int puzzlesCreated = 0;

            while (!int.TryParse(lineIn, out cycleNum))
            {
                Console.WriteLine("Invalid entry, please enter a positive integer only:");
                lineIn = Console.ReadLine();
            }

            cycleNum = int.Parse(lineIn);

            SoleCandidateHiddenSingles soleCandidate = new SoleCandidateHiddenSingles();

            string path = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            string folderName = "PuzzlePresets";

            path = Directory.GetParent(Directory.GetParent(Directory.GetParent(path).FullName).FullName).FullName;
            path = path + "\\" + folderName;

            for (int cycleCounter = 0; cycleCounter < cycleNum; cycleCounter++)
            {
                try
                {
                    int puzzleDifficulty = 0; // 0 - Easy (<=81), 1 - Medium(81<300), 2 - Hard(>=300)

                    PuzzleCreate puzzleCreator = new PuzzleCreate();
                    char[,] puzzle = puzzleCreator.puzzleCreate(25); // 25 cells. Explanation in report.

                    if ((cycleCounter + 1) % 50 == 0 && cycleCounter != 0)
                    {
                        Console.WriteLine("Cycle: " + (cycleCounter + 1));
                    }

                    string puzzleStr = "\r\n";

                    string curFile = "";

                    Dictionary<int, LinkedList<SoleCandidateHiddenSingles.NodeData>> dictOfTrees = new Dictionary<int, LinkedList<SoleCandidateHiddenSingles.NodeData>>();

                    // 70s timeout to allow for puzzles that the algorithm may find difficult to solve in a timely manner (There are anti-algorithm puzzles)
                    // There will be more difficult puzzles that require even longer to solve, however time and system resources impose a limitation.
                    // This program was ran overnight, on 2 seperate occassions (~14 hours total) in order to build large banks of preset puzzles at each difficulty.
                    // Allowed 2 minutes to complete a puzzle (Some puzzles require 10s of thousands of decision tree branches)
                    bool Completed = ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(120000), () =>
                    {
                        char[,] solution = puzzle.Clone() as char[,];
                        if (soleCandidate.solveSudoku(ref solution, dictOfTrees))
                        {

                            foreach (char character in puzzle)
                            {
                                puzzleStr += character;
                            }

                            // Difficulties are rated based upon the required number of Decision tree branches for a solution. 
                            // This is due to the sole candidate solution method being the most widely used human solution method.
                            if (dictOfTrees.Count < 80)
                            {
                                puzzleDifficulty = 0;
                            }
                            else if (dictOfTrees.Count >= 80 && dictOfTrees.Count < 200)
                            {
                                puzzleDifficulty = 1;
                            }
                            else if (dictOfTrees.Count >= 200 && dictOfTrees.Count < 800)
                            {
                                puzzleDifficulty = 2;
                            }
                            else if (dictOfTrees.Count >= 800)
                            {
                                puzzleDifficulty = 3;
                            }


                            switch (puzzleDifficulty)
                            {
                                case 0:
                                    // Comment out if not wanting to generate Easy puzzles 
                                    curFile = path + "\\EasyPresets.txt";
                                    break;

                                case 1:
                                    // Comment out if not wanting to generate Medium puzzles
                                    curFile = path + "\\MediumPresets.txt";
                                    break;

                                case 2:
                                    // Comment out if not wanting to generate Hard puzzles 
                                    curFile = path + "\\HardPresets.txt";
                                    break;

                                case 3:
                                    // Comment out if not wanting to generate Near Impossible puzzles 
                                    curFile = path + "\\VeryHardPresets.txt";
                                    break;
                            }
                        }
                    });

                    if (Completed == true && curFile != "")
                    {
                        using (StreamWriter logWriteStream = File.AppendText(curFile))
                        {
                            logWriteStream.Write(puzzleStr);
                            logWriteStream.Flush();
                            logWriteStream.Close();
                        }

                        puzzlesCreated++;
                        if (puzzlesCreated % 50 == 0)
                        {
                            dictOfTrees = null;
                            puzzle = null;
                            puzzleCreator = null;
                            Console.WriteLine("Puzzles created thus far: " + puzzlesCreated);
                        }

                    }
                }
                catch (Exception except)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("EXCEPTION THROWN");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
            }
        }


        /// <summary>
        /// Shankar_pratap (2012) available online at: https://stackoverflow.com/questions/7413612/how-to-limit-the-execution-time-of-a-function-in-c-sharp [accessed: 15/04/20]
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="codeBlock"></param>
        /// <returns></returns>
        private static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
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
}
