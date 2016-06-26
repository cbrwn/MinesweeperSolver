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
    }

}