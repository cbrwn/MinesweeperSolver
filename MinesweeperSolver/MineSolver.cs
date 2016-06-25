using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MinesweeperSolver.Properties;

namespace MinesweeperSolver {

    internal static class MineSolver {
        // Width/Height of each square
        public const int SQUARE_SIZE = 16;
        public static Rectangle MinesweeperWindow = Rectangle.Empty;
        public static volatile bool Running = false;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        ///     Finds the Minesweeper window and stores the position in MinesweeperWindow
        /// </summary>
        /// <param name="check">
        ///     Whether or not to start it close to the old window. Used for a quick "check" that the window is
        ///     still there
        /// </param>
        /// <returns>Whether or not the window was found</returns>
        public static bool FindSweeperWindow(bool check = false) {
            var tmr = new Stopwatch();
            tmr.Start();

            check = check && MinesweeperWindow != Rectangle.Empty;
            var desktop = ScreenHelper.GetDesktopScreenshot();
            var topLeft = ScreenHelper.GetBitmapPosition(desktop, Resources.topleft, check ? MinesweeperWindow.X - 1 : 0, check ? MinesweeperWindow.Y - 1 : 0);
            // Minimize search time after finding topLeft by starting the search from around the expected bottom right
            if (!check) {
                var bottomRight = topLeft != Point.Empty ? ScreenHelper.GetBitmapPosition(desktop, Resources.bottomright, topLeft.X + 20, topLeft.Y + 20) : Point.Empty;
                MinesweeperWindow = bottomRight != Point.Empty ? new Rectangle(topLeft.X, topLeft.Y, bottomRight.X + Resources.bottomright.Width - topLeft.X, bottomRight.Y + Resources.bottomright.Height - topLeft.Y) : Rectangle.Empty;
            }

            tmr.Stop();
            return check ? topLeft != Point.Empty : MinesweeperWindow != Rectangle.Empty;
        }

        /// <summary>
        ///     Clicks the restart button on the Minesweeper window
        /// </summary>
        public static void ClickSweeperRestart() {
            var posx = SystemInformation.VirtualScreen.Left + MinesweeperWindow.X + MinesweeperWindow.Width/2;
            var posy = SystemInformation.VirtualScreen.Top + MinesweeperWindow.Y + 25;
            InputHelper.LeftClick(posx, posy);
        }

        /// <summary>
        ///     Click a square in the Minesweeper window
        /// </summary>
        /// <param name="x">The column to click</param>
        /// <param name="y">The row to click</param>
        /// <param name="flag">Whether or not to set a flag (right click)</param>
        public static void ClickSweeperSquare(int x, int y, ref Board board, bool flag = false) {
            if (board.GetSquare(x, y) != -1)
                return;
            // Leftmost + window pos + offset + col or row + half width
            var posx = SystemInformation.VirtualScreen.Left + MinesweeperWindow.X + 8 + 16*x + 8;
            var posy = SystemInformation.VirtualScreen.Top + MinesweeperWindow.Y + 50 + 16*y + 8;

            if (flag) {
                InputHelper.RightClick(posx, posy);
                board.Squares[y, x] = 9;
            } else {
                InputHelper.LeftClick(posx, posy);
                board.Squares[y, x] = -3;
            }
            Thread.Sleep(2);
        }

        /// <summary>
        ///     Click a square in the Minesweeper window
        /// </summary>
        /// <param name="p">Point of the square to click</param>
        /// <param name="board">The board so we can locally change the state</param>
        /// <param name="flag">Whether or not to set a flag (right click)</param>
        public static void ClickSweeperSquare(Point p, ref Board board, bool flag = false) {
            ClickSweeperSquare(p.X, p.Y, ref board, flag);
        }

        /// <summary>
        ///     Clicks a list of Minesweeper squares
        /// </summary>
        /// <param name="squares">List of squares to click</param>
        /// <param name="board">The board so we can locally change the state</param>
        /// <param name="flag">Whether or not to set a flag (right click)</param>
        private static void ClickSquareList(IEnumerable<Point> squares, ref Board board, bool flag = false) {
            foreach (var p in squares)
                ClickSweeperSquare(p, ref board, flag);
        }

        /// <summary>
        ///     Looks for and performs a Minesweeper move
        /// </summary>
        /// <param name="board">The current board</param>
        public static bool DoBestSweeperActions(Board board) {
            // This is really badly done, just an idea right now

            // Squares which require no guessing
            var clicked = false;
            var tmr = new Stopwatch();
            tmr.Start();
            for (var y = 0; y < board.Rows; y++) {
                for (var x = 0; x < board.Columns; x++) {
                    if (board.GetSquare(x, y) == 0)
                        continue;
                    var clicks = SweeperHelper.GetSurroundingClicks(board, x, y);
                    var bombs = SweeperHelper.GetSurroundingBombs(board, x, y);
                    if (clicks.Count == 0)
                        continue;
                    // Click a square around a clicked where the clicked's bombs have all been found
                    if (board.GetSquare(x, y) - bombs.Count == 0) {
                        ClickSquareList(clicks, ref board);
                        //return true;
                        clicked = true;
                    }

                    // Flag a guaranteed bomb
                    if (board.GetSquare(x, y) == clicks.Count + bombs.Count) {
                        ClickSquareList(clicks, ref board, true);
                        //return true;
                        clicked = true;
                    }
                }
                if (tmr.Elapsed.TotalMilliseconds >= 500) {
                    tmr.Restart();
                    if (!FindSweeperWindow(true))
                        return true;
                }
            }
            if (clicked)
                return true;

            // Now we need to guess
            var lowestProb = 1000d;
            int lx = 0, ly = 0;
            for (var y = 0; y < board.Rows; y++) {
                for (var x = 0; x < board.Columns; x++) {
                    if (board.GetSquare(x, y) != -1)
                        continue;
                    var prob = SweeperHelper.GetBombProbability(board, x, y);
                    if (prob >= lowestProb)
                        continue;
                    lowestProb = prob;
                    lx = x;
                    ly = y;
                }
            }
            Console.WriteLine($"Guessed ({lx},{ly}) with probability of {lowestProb}%");
            ClickSweeperSquare(lx, ly, ref board, lowestProb>50);
            return true;
        }
    }

}