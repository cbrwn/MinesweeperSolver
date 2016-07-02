using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace MinesweeperSolver.solvers {

    internal class SmartSolver : Solver {
        private Dictionary<Point, int> _locs;
        private int[,] _oldBoard;
        private List<Point> _squares;
        private Stopwatch _timer;

        private List<Board> _valids;

        public override bool DoMove() {
            if (base.DoMove())
                return true;

            // TODO: Move most of this stuff to Update
            var bd = Board.GetBorderSquares();
            _oldBoard = (int[,]) Board.Squares.Clone();

            if (bd.Count == 0) {
                Console.WriteLine(@"Clicking random");
                ClickRandom();
                return true;
            }

            // Grab connected squares
            // This splits up the border and makes the solver so much faster, as the amount of possibilities increases exponentially
            _squares = Board.GetConnectedSquares(bd[0], bd);
            foreach (var s in bd.Select(b => Board.GetConnectedSquares(b, bd)).Where(s => s.Count < _squares.Count))
                _squares = s;

            if (_squares.Count >= 35) {
                Console.WriteLine(@"Falling back to probability solver - too many possibilities");
                var ps = new ProbabilitySolver();
                ps.Update();
                return ps.DoMove();
            }

            var possibilities = GetValidMinePlacements();

            Console.WriteLine($"Found {possibilities.Count} possible layouts! Processing...");
            // Get new flag locations
            _locs = new Dictionary<Point, int>();
            foreach (var p in possibilities) {
                for (var y = 0; y < Board.Rows; y++) {
                    for (var x = 0; x < Board.Columns; x++) {
                        if (Board.GetSquare(x, y) != -1)
                            continue;
                        if (p.Squares[y, x] != 9)
                            continue;
                        var pnt = new Point(x, y);
                        if (_locs.ContainsKey(pnt))
                            _locs[pnt]++;
                        else
                            _locs.Add(pnt, 1);
                    }
                }
            }

            Console.WriteLine(@"Getting impossible bomb spots...");
            var clicked = false;
            foreach (var b in _squares.Where(b => !_locs.ContainsKey(b))) {
                Console.WriteLine(@" -> Found one!");
                ClickSweeperSquare(b);
                clicked = true;
            }

            Console.WriteLine(@"Getting guaranteed bomb locations...");
            foreach (var p in _locs.Keys.Where(p => _locs[p] == _locs.Count)) {
                Console.WriteLine(@" -> Found one!");
                ClickSweeperSquare(p, true);
                clicked = true;
            }
            if (clicked && base.DoMove()) {
                Console.WriteLine(@"Able to continue without guessing...");
                return true;
            }

            Console.WriteLine(@"Finding least likely bomb placement...");
            var lowestCount = 1000000;
            var lowestPoint = Point.Empty;
            foreach (var p in _locs.Keys.Where(p => _locs[p] < lowestCount)) {
                lowestCount = _locs[p];
                lowestPoint = p;
            }
            if (lowestPoint == Point.Empty) {
                ClickRandom();
                return true;
            }
            Console.WriteLine($"Safest square is ({lowestPoint.X}, {lowestPoint.Y}) with a {100d*((double) lowestCount/_locs.Values.Max())}% bomb chance");
            ClickSweeperSquare(lowestPoint);

            return true;
        }

        /// <summary>
        ///     Grab all valid combinations of mine placements
        /// </summary>
        /// <returns>List of valid boards</returns>
        private List<Board> GetValidMinePlacements() {
            // Iterate through every possibility of mine placements and only add the valid ones to the list
            //var border = Board.GetBorderSquares();

            Console.WriteLine($"Iterating through all mine placements... ({Math.Pow(2, _squares.Count)} possible combinations - {_squares.Count} possible places)");
            _valids = new List<Board>();
            _timer = new Stopwatch();
            _timer.Start();
            GetFlagCombinations(Board, 0);
            _timer.Stop();
            return _valids;
        }

        private void GetFlagCombinations(Board board, int depth) {
            while (true) {
                var tboard = new Board(board);
                if (!tboard.IsValid())
                    return;
                if (depth == _squares.Count) {
                    if (!tboard.IsBombful(_squares))
                        return;
                    _valids.Add(tboard);
                    if (!(_timer.Elapsed.TotalSeconds >= 1))
                        return;
                    Console.WriteLine($"{_valids.Count} valid boards found ({(int) _timer.Elapsed.TotalMilliseconds}ms)");
                    _timer.Restart();
                    return;
                }
                tboard.SetSquare(_squares[depth], 9);
                GetFlagCombinations(tboard, depth + 1);
                tboard.SetSquare(_squares[depth], -1);
                board = tboard;
                depth = depth + 1;
            }
        }

        public override Bitmap GetBrainImage() {
            if (_locs == null || _oldBoard == null)
                return null;
            const int size = 32;
            const int fontMulti = (int) (size/8d);
            var result = new Bitmap(Board.Columns*size, Board.Rows*size);
            using (var g = Graphics.FromImage(result)) {
                for (var y = 0; y < Board.Rows; y++) {
                    for (var x = 0; x < Board.Columns; x++) {
                        var col = Board.NumberColors[0];
                        var pos = 1000d;
                        if (_locs.ContainsKey(new Point(x, y)))
                            pos = 100*((double) _locs[new Point(x, y)]/_locs.Values.Max());
                        var val = _oldBoard[y, x]; //Board.GetSquare(x, y);
                        if (val == 9)
                            col = Color.Magenta;
                        else if (val > -1 && val < 9)
                            col = Board.NumberColors[0];
                        else if (pos > 100)
                            col = Color.DarkBlue;
                        else if (pos > 50) {
                            var n = (int) (255*((pos - 50)/50d));
                            col = Color.FromArgb(255, Math.Abs(n - 255), 0);
                        } else if (pos >= 0) {
                            var n = (int) (255*(pos/50d));
                            col = Color.FromArgb(n, 255, 0);
                        }
                        g.FillRectangle(new SolidBrush(col), x*size, y*size, size, size);
                        if (val > 0 && val < 9) {
                            var textCol = Board.NumberColors[val];
                            g.DrawString(val.ToString(), new Font(FontFamily.GenericMonospace, 5*fontMulti, FontStyle.Bold), new SolidBrush(textCol), fontMulti + x*size, fontMulti/2 + y*size);
                        }
                        g.DrawRectangle(new Pen(Color.Gray), x*size, y*size, size, size);
                    }
                }
            }
            return result;
        }
    }

}