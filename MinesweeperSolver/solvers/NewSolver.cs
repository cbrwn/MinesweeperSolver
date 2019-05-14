using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace MinesweeperSolver.solvers
{

    internal class NewSolver : Solver
    {

        bool hasPrinted = false;
        int foundBoards = 0;
        private Stopwatch _timer;
        bool wasStopped = false;

        public override bool DoMove()
        {
            if (base.DoMove())
                return true;

            // get all border squares
            var border = Board.GetBorderSquares();

            var groups = GetBorderGroups(border);
            Console.WriteLine($"There are {groups.Count} border groups!");
            foreach (var group in groups)
            {
                if (group.Count < border.Count)
                    border = group;
            }

            var validBoards = new List<Board>();

            Console.WriteLine($"Finding valid boards with {border.Count} border squares ({Math.Pow(2, border.Count)} possibilities)");
            hasPrinted = false;
            foundBoards = 0;

            _timer = new Stopwatch();
            _timer.Start();
            wasStopped = false;
            GetValidBoards(border, ref validBoards);

            if (!MineSolver.Running)
                return false;

            Console.WriteLine($"Found {validBoards.Count} valid boards");

            if (validBoards.Count == 0)
            {
                // fall back to fast solver when it takes too long
                var ps = new ProbabilitySolver();
                ps.Update();
                return ps.DoMove();
            }

            var bombPoints = new Dictionary<Point, float>();
            // find guaranteed bombs
            foreach (Board b in validBoards)
            {
                for (int i = 0; i < border.Count; ++i)
                {
                    Point p = border[i];
                    if (!bombPoints.ContainsKey(p))
                        bombPoints.Add(p, 0.0f);

                    if (b.GetSquare(p) == 9)
                        bombPoints[p] += 1.0f;
                }
            }

            bool foundGuarantee = false;
            float highestProb = 0.0f;
            Point highestProbPoint = border[0];
            bool flagHighest = true;

            foreach (Point k in bombPoints.Keys)
            {
                if (bombPoints[k] == 0.0f && !wasStopped)
                {
                    ClickSweeperSquare(k);
                    foundGuarantee = true;
                }

                float prob = bombPoints[k] / (float)validBoards.Count;

                if (prob >= 1.0f && !wasStopped)
                {
                    ClickSweeperSquare(k, true);
                    foundGuarantee = true;
                }

                if (prob > highestProb && !wasStopped)
                {
                    highestProb = prob;
                    highestProbPoint = k;
                    flagHighest = true;
                }

                float emptyProb = 1.0f - prob;
                if (emptyProb > highestProb)
                {
                    highestProb = emptyProb;
                    highestProbPoint = k;
                    flagHighest = false;
                }
            }

            if (!foundGuarantee)
            {
                Console.WriteLine($"Clicking {highestProbPoint.X},{highestProbPoint.Y} as {(flagHighest ? "flag" : "safe")} with {100.0f * highestProb}%");
                ClickSweeperSquare(highestProbPoint, flagHighest);
            }

            return true;
        }

        void GetValidBoards(List<Point> borders, ref List<Board> valids, int idx = 0, Board currentBoard = null)
        {
            if (!MineSolver.Running)
                return;
            if (idx >= borders.Count)
            {
                foundBoards++;
                bool boardValid = true;
                for (int i = 0; i < borders.Count; ++i)
                {
                    Point bp = borders[i];

                    var nums = currentBoard.GetSurroundingNumbers(bp.X, bp.Y);
                    foreach (Point nm in nums)
                    {
                        if (!currentBoard.IsFulfilled(nm.X, nm.Y))
                        {
                            boardValid = false;
                            break;
                        }
                    }

                    if (!boardValid)
                        break;
                }
                if (boardValid && currentBoard.IsValid())
                {
                    valids.Add(new Board(currentBoard));
                    if (valids.Count % 1 == 0)
                    {
                        if (hasPrinted)
                            Console.CursorTop -= 1;
                        Console.CursorLeft = 0;
                        Console.WriteLine($" => Found {valids.Count}/{foundBoards} valid boards...");
                        hasPrinted = true;
                        _timer.Restart();
                    }
                }
                return;
            }

            if (_timer.Elapsed.TotalSeconds > 10)
            {
                wasStopped = true;
                return;
            }

            Board newBoard = null;
            if (currentBoard == null)
                newBoard = new Board(Board);
            else
                newBoard = new Board(currentBoard);

            Point p = borders[idx];

            newBoard.SetSquare(p, 9);
            if (newBoard.IsValid())
                GetValidBoards(borders, ref valids, idx + 1, newBoard);
            newBoard.SetSquare(p, -1);
            if (newBoard.IsValid())
                GetValidBoards(borders, ref valids, idx + 1, newBoard);
        }

        public override Bitmap GetBrainImage()
        {
            return null;
        }

        List<List<Point>> GetBorderGroups(List<Point> border)
        {
            List<List<Point>> groups = new List<List<Point>>();
            List<Point> brdr = new List<Point>(border);

            while (brdr.Count > 0)
            {
                List<Point> grp = new List<Point>();
                GetBorderGroup(brdr[0], ref grp);

                foreach (Point p in grp)
                    brdr.Remove(p);

                groups.Add(grp);
            }
            return groups;
        }

        void GetBorderGroup(Point p, ref List<Point> group)
        {
            if (Board.GetSquare(p) != -1) // Only want unclicked squares
                return;
            if (group.Contains(p))
                return;
            if (Board.GetSurroundingNumbers(p.X, p.Y).Count > 0)
            {
                group.Add(p);
                foreach (Point op in Board.GetSurroundingClicks(p.X, p.Y))
                {
                    GetBorderGroup(op, ref group);
                }
            }
        }
    }

}