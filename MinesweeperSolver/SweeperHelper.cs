using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MinesweeperSolver {

    public static class SweeperHelper {
        /// <summary>
        ///     Grabs a list of surrounding flagged squares
        /// </summary>
        /// <param name="board">The board to search</param>
        /// <param name="x">Column of the square to search around</param>
        /// <param name="y">Row of the square to search around</param>
        /// <returns>List of surrounding flag squares</returns>
        public static List<Point> GetSurroundingBombs(Board board, int x, int y) {
            return GetSurrounding(board, x, y, 9);
        }

        /// <summary>
        ///     Grabs a list of surrounding unclicked squares
        /// </summary>
        /// <param name="board">The board to search</param>
        /// <param name="x">Column of the square to search around</param>
        /// <param name="y">Row of the square to search around</param>
        /// <returns>List of surrounding squares which are unclicked</returns>
        public static List<Point> GetSurroundingClicks(Board board, int x, int y) {
            return GetSurrounding(board, x, y, -1);
        }

        /// <summary>
        ///     Grabs a list of surrounding clicked squares
        /// </summary>
        /// <param name="board">The board to search</param>
        /// <param name="x">Column of the square to search around</param>
        /// <param name="y">Row of the square to search around</param>
        /// <returns>List of surrounding squares which have been clicked</returns>
        public static List<Point> GetSurroundingNumbers(Board board, int x, int y) {
            var result = new List<Point>();
            for (var i = 0; i < 9; i++)
                result.AddRange(GetSurrounding(board, x, y, i));
            return result;
        }

        /// <summary>
        ///     Grabs a list of squares with value 'type' which surround the square at (x,y)
        /// </summary>
        /// <param name="board">The board to search</param>
        /// <param name="x">Column of the square to search around</param>
        /// <param name="y">Row of the square to search around</param>
        /// <param name="type">Value of squares to return</param>
        /// <returns>List of squares of type 'type' which surround the square</returns>
        private static List<Point> GetSurrounding(Board board, int x, int y, int type = -2) {
            var result = new List<Point>();
            for (var i = Math.Max(0, y - 1); i <= Math.Min(board.Rows - 1, y + 1); i++) {
                for (var j = Math.Max(0, x - 1); j <= Math.Min(board.Columns - 1, x + 1); j++) {
                    if (board.Squares[i, j] == type || type == -2)
                        result.Add(new Point(j, i));
                }
            }
            return result;
        }

        /// <summary>
        ///     Gets the probability of a square being a bomb
        /// </summary>
        /// <param name="board">Board to test</param>
        /// <param name="x">Column of square to test</param>
        /// <param name="y">Row of square to test</param>
        /// <returns>Probability of the square being a bomb, from 0 to 100</returns>
        public static double GetBombProbability(Board board, int x, int y) {
            // 100% sure it's a bomb if it's... a bomb
            if (board.GetSquare(x, y) == 9 || board.GetSquare(x, y) == 100)
                return 100;
            // It can't be a bomb if it has already been clicked and wasn't a bomb
            if (board.GetSquare(x, y) >= 0)
                return 0;

            var surrounding = GetSurroundingNumbers(board, x, y);
            if (surrounding.Count == 0) // No surrounding numbers - safest to assume it's 101% a bomb
                return 101;
            // We'll list the probabilities from each surrounding square (e.g. if a square is "3" and has 1 bomb next to it, the probability will be 50% or 50)
            var probabilities = new List<double>();
            foreach (var p in surrounding) {
                var val = board.GetSquare(p);
                if (val == 0) // If a surrounding square has no nearby bombs, this one can't be a bomb
                    return 0;
                if (val == 1) // If there's 1 bomb nearby a surrounding square and this one hasn't been clicked, it must be a bomb
                    return 100;
                var clicks = GetSurroundingClicks(board, p.X, p.Y);
                if (clicks.Count == 0) // If a surrounding square can't be clicked on, this one must either be clicked or a bomb (should never happen, just stopping div by zero in case it does)
                    return 0;
                var bombs = GetSurroundingBombs(board, p.X, p.Y);

                val -= bombs.Count; // How many bombs we still need to find

                // Remaining bombs /
                probabilities.Add(100*((double) val/clicks.Count));
            }

            return (double)probabilities.Sum()/probabilities.Count;
        }
    }

}