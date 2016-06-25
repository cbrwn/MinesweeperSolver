using System;
using System.Drawing;
using System.Linq;

namespace MinesweeperSolver {

    public class Board {
        private static readonly Color[] NumberColors = { Color.FromArgb(189, 189, 189), Color.FromArgb(0, 0, 255), Color.FromArgb(0, 123, 0), Color.FromArgb(255, 0, 0), Color.FromArgb(0, 0, 123), Color.FromArgb(123, 0, 0), Color.FromArgb(0, 123, 123), Color.FromArgb(0, 0, 0), Color.FromArgb(123, 123, 123) };
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
        public int Score => (int)(100*(Squares.Cast<int>().Count(i => i != -1)/(double)Squares.Length));

        /// <summary>
        ///     Updates the board state.
        ///     Also spits out the current board state if it's a full update
        /// </summary>
        /// <param name="newImage">Bitmap of the whole Minesweeper window</param>
        /// <param name="full">Whether or not to exclusively update non-clicked squares</param>
        public void Update(Bitmap newImage, bool full = false) {
            for (var y = 0; y < Rows; y++) {
                for (var x = 0; x < Columns; x++) {
                    if (Squares[y, x] >= 0 && !full) // Values under 0 are unclicked - we only check them if we're doing a full update (in the main loop)
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
        /// <returns>The value of the updated square</returns>
        private int UpdateSquare(Bitmap image, int col, int row) {
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
            }
            return Squares[row, col];
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

        public Bitmap GetVisualization() {
            var result = new Bitmap(Columns*8, Rows*8);
            using (var g = Graphics.FromImage(result)) {
                for (var y = 0; y < Rows; y++) {
                    for (var x = 0; x < Columns; x++) {
                        var col = Color.Gray;
                        var pos = SweeperHelper.GetBombProbability(this, x, y);
                        if (GetSquare(x, y) == 9) {
                            col = Color.Magenta;
                        } else if (pos > 100) {
                            col = Color.DarkBlue;
                        } else if (pos > 50) {
                            var n = (int)(255*((pos - 50)/50d));
                            col = Color.FromArgb(255, Math.Abs(n - 50), 0);
                        } else if (pos > 0) {
                            var n = (int) (255*(pos/50d));
                            col = Color.FromArgb(n, 255, 0);
                        }
                        g.FillRectangle(new SolidBrush(col), x*8, y*8, 8, 8);
                    }
                }
            }
            return result;
        }
    }

}