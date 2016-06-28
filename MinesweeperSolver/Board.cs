using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace MinesweeperSolver {

    public class Board {
        public static readonly Color[] NumberColors = { Color.FromArgb(189, 189, 189), Color.FromArgb(0, 0, 255), Color.FromArgb(0, 123, 0), Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 123), Color.FromArgb(123, 0, 0), Color.FromArgb(0, 123, 123), Color.FromArgb(0, 0, 0), Color.FromArgb(123, 123, 123) };
        public readonly int Columns;
        public readonly int Rows;
        public readonly int[,] Squares;

        /// <summary>
        ///     New instance of a board, with all squares being unclicked
        /// </summary>
        private Board(int columns = 30, int rows = 16) {
            Columns = columns;
            Rows = rows;
            Squares = new int[Rows, Columns];
            // Start with a fresh board, all unclicked
            for (var y = 0; y < Rows; y++) {
                for (var x = 0; x < Columns; x++)
                    Squares[y, x] = -1;
            }
        }

        /// <summary>
        ///     New instance of a Board, made using a bitmap
        /// </summary>
        /// <param name="boardImage">The bitmap of the whole Minesweeper window</param>
        public Board(Bitmap boardImage)
        : this((int) Math.Round((boardImage.Width - 15d)/MineSolver.SQUARE_SIZE), (int) Math.Round((boardImage.Height - 57d)/MineSolver.SQUARE_SIZE)) {
            // ^ I think that's how it works, it looks super messy though
            Console.WriteLine($"Detected {Columns}x{Rows} grid from screenshot");

            for (var y = 0; y < Rows; y++) {
                for (var x = 0; x < Columns; x++)
                    Squares[y, x] = -2; // -2 so we can tell when processing didn't work
            }
            Update(boardImage, true);
        }

        public bool IsComplete => Squares.Cast<int>().All(i => i != -1);
        public bool IsNew => Squares.Cast<int>().All(i => i == -1);
        public bool IsFailed => (from int i in Squares where i == 100 select i).Any();
        public int Score => (int) (100*(Squares.Cast<int>().Count(i => i != -1)/(double) Squares.Length));

        /// <summary>
        ///     Updates the board state.
        ///     Also spits out the current board state if it's a full update
        /// </summary>
        /// <param name="newImage">Bitmap of the whole Minesweeper window</param>
        /// <param name="full">Whether or not to exclusively update non-clicked squares</param>
        public void Update(Bitmap newImage, bool full = false) {
            for (var y = 0; y < Rows; y++) {
                for (var x = 0; x < Columns; x++) {
                    if (Squares[y, x] >= 0 && Squares[y,x] != 9 && !full) // Values under 0 are unclicked - we only check them if we're doing a full update (in the main loop)
                        continue;
                    UpdateSquare(newImage, x, y);
                }
            }
        }

        /// <summary>
        ///     Updates a specific square
        ///     Could be put back into Update
        /// </summary>
        /// <param name="image">Bitmap of the whole Minesweeper window</param>
        /// <param name="col">The column (or x value) of the square to update</param>
        /// <param name="row">The row (or y value) of the square to update</param>
        private void UpdateSquare(Bitmap image, int col, int row) {
            // Top/left margins
            const int xoff = 8;
            const int yoff = 50;

            // Top left of the square
            var xpos = xoff + col*16;
            var ypos = yoff + row*16;

            // Check colours
            if (image.GetPixel(xpos + 1, ypos + 1) == Color.FromArgb(255, 0, 0)) {
                // This will only happen with a clicked mine - bad guess or misclick
                // Set the square to exploded mine
                // TODO: Make enum for square types
                Squares[row, col] = 100;
            } else if (image.GetPixel(xpos, ypos) == Color.FromArgb(255, 255, 255)) {
                // Unopened square - either a blank square (-1) or bomb (9)
                if (image.GetPixel(xpos + 8, ypos + 7) == Color.FromArgb(255, 0, 0))
                    Squares[row, col] = 9;
                else
                    Squares[row, col] = -1;
            } else {
                // A clicked square, luckily all the numbers have the colours at (8,9) so we only need to check 1 pixel
                var numCol = image.GetPixel(9 + xpos, 8 + ypos);
                for (var i = 0; i < NumberColors.Length; i++) {
                    if (numCol.ToArgb() != NumberColors[i].ToArgb())
                        continue;
                    Squares[row, col] = i;
                    break;
                }
                // Black in middle - could be a bomb!
                if (Squares[row, col] == 7 && image.GetPixel(xpos + 6, ypos + 6) == Color.FromArgb(255, 255, 255)) {
                    // Bomb has white shimmer on it at 5,5 to 7,7
                    Squares[row, col] = 100;
                }
            }
        }

        /// <summary>
        ///     Gets the value of a square at a specific point
        /// </summary>
        /// <param name="x">Column of square</param>
        /// <param name="y">Row of square</param>
        /// <returns>The number of bombs next to the square</returns>
        public int GetSquare(int x, int y) {
            return Squares[y, x];
        }

        /// <summary>
        ///     Gets the value of a square at a specific point
        /// </summary>
        /// <param name="pos">Point of square</param>
        /// <returns>The number of bombs next to the square</returns>
        public int GetSquare(Point pos) {
            return Squares[pos.Y, pos.X];
        }

        /// <summary>
        ///     Grabs a list of surrounding flagged squares
        /// </summary>
        /// <param name="x">Column of the square to search around</param>
        /// <param name="y">Row of the square to search around</param>
        /// <returns>List of surrounding flag squares</returns>
        public List<Point> GetSurroundingBombs(int x, int y) {
            return GetSurrounding(x, y, 9);
        }

        /// <summary>
        ///     Grabs a list of surrounding unclicked squares
        /// </summary>
        /// <param name="x">Column of the square to search around</param>
        /// <param name="y">Row of the square to search around</param>
        /// <returns>List of surrounding squares which are unclicked</returns>
        public List<Point> GetSurroundingClicks(int x, int y) {
            return GetSurrounding(x, y, -1);
        }

        /// <summary>
        ///     Grabs a list of surrounding clicked squares
        /// </summary>
        /// <param name="x">Column of the square to search around</param>
        /// <param name="y">Row of the square to search around</param>
        /// <returns>List of surrounding squares which have been clicked</returns>
        public List<Point> GetSurroundingNumbers(int x, int y) {
            var result = new List<Point>();
            for (var i = 0; i < 9; i++)
                result.AddRange(GetSurrounding(x, y, i));
            return result;
        }

        /// <summary>
        ///     Grabs a list of squares with value 'type' which surround the square at (x,y)
        /// </summary>
        /// <param name="x">Column of the square to search around</param>
        /// <param name="y">Row of the square to search around</param>
        /// <param name="type">Value of squares to return</param>
        /// <returns>List of squares of type 'type' which surround the square</returns>
        private List<Point> GetSurrounding(int x, int y, int type = -2) {
            var result = new List<Point>();
            for (var i = Math.Max(0, y - 1); i <= Math.Min(Rows - 1, y + 1); i++) {
                for (var j = Math.Max(0, x - 1); j <= Math.Min(Columns - 1, x + 1); j++) {
                    if (Squares[i, j] == type || type == -2)
                        result.Add(new Point(j, i));
                }
            }
            return result;
        }
    }

}