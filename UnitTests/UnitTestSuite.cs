using System;
using System.Collections.Generic;
using System.Text;
using Algorithms;
using PuzzleCreation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace UnitTests
{
    [TestClass]
    class AlgorithmUnitTests
    {
        // Only need blank puzzles to prove algorithms can fill/solve a sudoku table.
        private int[,] intPuzzle = {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 }};

        private char[,] charPuzzle = {
                { '0', '0', '0', '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0', '0', '0', '0' },
                { '0', '0', '0', '0', '0', '0', '0', '0', '0' }};

        [Test]
        public void bruteForceUnitTest()
        {
            Assert.IsTrue(BruteForce.SudokuSolve(intPuzzle, 0, 0));
        }

        [Test]
        public void depthFirstUnitTest()
        {
            DepthFirst depthFirst = new DepthFirst();
            charPuzzle = depthFirst.SolveSudoku(charPuzzle);
            Assert.IsFalse(charPuzzle.ToString().Contains('0'));
        }

        [Test]
        public void bitwiseUnitTest()
        {
            Bitwise bitwise = new Bitwise(charPuzzle);
            Assert.IsFalse(charPuzzle.ToString().Contains('0'));
        }

        [Test]
        public void soloNakedUnitTest()
        {
            SoleCandidate soleCandidate = new SoleCandidate();
            Dictionary<int, LinkedList<SoleCandidate.NodeData>> treeDictionary = new Dictionary<int, LinkedList<SoleCandidate.NodeData>>();
            Assert.IsTrue(soleCandidate.solveSudoku(ref charPuzzle, treeDictionary));
        }

        [Test]
        public void soloHiddenUnitTest()
        {
            SoleCandidateHiddenSingles soleCandidate = new SoleCandidateHiddenSingles();
            Dictionary<int, LinkedList<SoleCandidateHiddenSingles.NodeData>> treeDictionary = new Dictionary<int, LinkedList<SoleCandidateHiddenSingles.NodeData>>();
            Assert.IsTrue(soleCandidate.solveSudoku(ref charPuzzle, treeDictionary));
        }
    }

    [TestClass]
    class MethodUnitTests
    {
        private char[,] charPuzzle = {
                { '1','2','3','4','5','6','7','8','9' },
                { '4','5','6','7','8','9','1','2','3' },
                { '7','8','9','1','2','3','4','5','6' },
                { '2','1','4','3','6','5','8','9','7' },
                { '3','6','5','8','9','7','2','1','4' },
                { '8','9','7','2','1','4','3','6','5' },
                { '5','3','1','6','4','2','9','7','8' },
                { '6','4','2','9','7','8','5','3','1' },
                { '9','7','8','5','3','1','6','4','2' }};


        [Test]
        public void puzzleCorrectUnitTest()
        {
            SoleCandidate soleCandidate = new SoleCandidate();
            Dictionary<int, LinkedList<SoleCandidate.NodeData>> treeDictionary = new Dictionary<int, LinkedList<SoleCandidate.NodeData>>();

            char[,] puzzleClone = charPuzzle.Clone() as char[,];

            if (soleCandidate.solveSudoku(ref puzzleClone, treeDictionary))
            {
                Assert.IsTrue(PuzzleValidation.puzzleCorrect(puzzleClone));
            }
        }

        private char[,] stillValidTruePuzzle = {
                { '1','2','3','4','5','6','7','8','9' },
                { '4','0','6','0','8','0','1','2','3' },
                { '7','8','9','0','2','0','4','5','6' },
                { '2','1','0','0','0','5','8','9','7' },
                { '3','6','5','0','9','7','2','1','4' },
                { '0','9','7','0','1','4','3','6','5' },
                { '5','0','1','6','4','2','0','7','0' },
                { '0','4','2','9','7','8','0','3','1' },
                { '9','7','8','5','3','1','6','0','0' }};

        [Test]
        public void puzzleStillValidTrueUnitTest()
        {
            PuzzleValidation.puzzleStillValid(stillValidTruePuzzle, out List<int> dupIndexes);
            Assert.IsTrue(dupIndexes.Count == 0);
        }

        private char[,] stillValidFalsePuzzle = {
                { '1','2','3','4','5','6','7','8','9' },
                { '4','0','6','0','8','0','1','2','3' },
                { '7','8','9','0','2','0','4','5','6' },
                { '2','1','0','0','0','5','8','9','7' },
                { '3','6','5','0','9','7','2','1','4' },
                { '0','9','7','0','1','4','3','6','5' },
                { '5','0','1','6','4','2','0','7','7' },
                { '0','4','2','9','7','8','0','3','1' },
                { '9','7','8','5','3','1','6','0','0' }};

        [Test]
        public void puzzleStillValidFalseUnitTest()
        {
            PuzzleValidation.puzzleStillValid(stillValidFalsePuzzle, out List<int>  dupIndexes);
            Assert.IsFalse(dupIndexes.Count == 0);
        }
    }
}
