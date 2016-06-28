using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MinesweeperSolver.Properties;

namespace MinesweeperSolver.solvers {

    public class Solver {
        public readonly Board Board;
        private Rectangle _minesweeperWindow = Rectangle.Empty;

        protected Solver() {
            var screenshot = GetMinesweeperScreenshot();
            if (screenshot == null)
                throw new ArgumentNullException();
            Board = new Board(screenshot);
        }

        /// <summary>
        ///     Finds the Minesweeper window and stores the position in MinesweeperWindow
        /// </summary>
        /// <param name="check">
        ///     Whether or not to start it close to the old window. Used for a quick "check" that the window is
        ///     still there
        /// </param>
        /// <returns>Whether or not the window was found</returns>
        public bool FindSweeperWindow(bool check = false) {
            check = check && _minesweeperWindow != Rectangle.Empty;
            var desktop = ScreenHelper.GetDesktopScreenshot();
            var topLeft = ScreenHelper.GetBitmapPosition(desktop, Resources.topleft, check ? _minesweeperWindow.X - 1 : 0, check ? _minesweeperWindow.Y - 1 : 0);
            if (check)
                return topLeft != Point.Empty;
            var bottomRight = topLeft != Point.Empty ? ScreenHelper.GetBitmapPosition(desktop, Resources.bottomright, topLeft.X + 20, topLeft.Y + 20) : Point.Empty;
            _minesweeperWindow = bottomRight != Point.Empty ? new Rectangle(topLeft.X, topLeft.Y, bottomRight.X + Resources.bottomright.Width - topLeft.X, bottomRight.Y + Resources.bottomright.Height - topLeft.Y) : Rectangle.Empty;
            return _minesweeperWindow != Rectangle.Empty;
        }

        /// <summary>
        ///     Clicks the restart button on the Minesweeper window
        /// </summary>
        public void ClickSweeperRestart() {
            var posx = SystemInformation.VirtualScreen.Left + _minesweeperWindow.X + _minesweeperWindow.Width/2;
            var posy = SystemInformation.VirtualScreen.Top + _minesweeperWindow.Y + 25;
            InputHelper.LeftClick(posx, posy);
        }

        /// <summary>
        ///     Click a square in the Minesweeper window
        /// </summary>
        /// <param name="x">The column to click</param>
        /// <param name="y">The row to click</param>
        /// <param name="flag">Whether or not to set a flag (right click)</param>
        public void ClickSweeperSquare(int x, int y, bool flag = false) {
            if (Board.GetSquare(x, y) != -1)
                return;
            // Leftmost + window pos + offset + col or row + half width
            var posx = SystemInformation.VirtualScreen.Left + _minesweeperWindow.X + 8 + 16*x + 8;
            var posy = SystemInformation.VirtualScreen.Top + _minesweeperWindow.Y + 50 + 16*y + 8;

            if (flag) {
                InputHelper.RightClick(posx, posy);
                Board.SetSquare(x, y, 9);
            } else {
                InputHelper.LeftClick(posx, posy);
                Board.SetSquare(x, y, -3);
            }
            Thread.Sleep(10);
        }

        /// <summary>
        ///     Click a square in the Minesweeper window
        /// </summary>
        /// <param name="p">Point of the square to click</param>
        /// <param name="flag">Whether or not to set a flag (right click)</param>
        public void ClickSweeperSquare(Point p, bool flag = false) {
            ClickSweeperSquare(p.X, p.Y, flag);
        }

        /// <summary>
        ///     Clicks a list of Minesweeper squares
        /// </summary>
        /// <param name="squares">List of squares to click</param>
        /// <param name="flag">Whether or not to set a flag (right click)</param>
        private void ClickSquareList(IEnumerable<Point> squares, bool flag = false) {
            foreach (var p in squares)
                ClickSweeperSquare(p, flag);
        }

        /// <summary>
        ///     Grabs a screenshot of the Minesweeper window, returns null if the window hasn't been found
        /// </summary>
        /// <returns>A screenshot of the whole Minesweeper window</returns>
        public Bitmap GetMinesweeperScreenshot() {
            if (_minesweeperWindow == Rectangle.Empty)
                FindSweeperWindow();
            if (_minesweeperWindow == Rectangle.Empty)
                return null;
            var result = new Bitmap(_minesweeperWindow.Width, _minesweeperWindow.Height);
            using (var g = Graphics.FromImage(result))
            // VirtualScreen + Window because VirtualScreens go negative and the position in the image is obviously positive
                g.CopyFromScreen(SystemInformation.VirtualScreen.Left + _minesweeperWindow.X, SystemInformation.VirtualScreen.Top + _minesweeperWindow.Y, 0, 0, _minesweeperWindow.Size);
            return result;
        }

        /// <summary>
        ///     Update information needed by the solver, called before DoMove
        /// </summary>
        public virtual void Update() {
            Board.Update(GetMinesweeperScreenshot(), true);
        }

        /// <summary>
        ///     Does a move (or set of moves) in the game
        ///     Does all obvious moves by default, allowing Solver classes to focus on the guessing strategy
        /// </summary>
        public virtual bool DoMove() {
            var result = false;
            var bombs = Board.GetGuaranteedBombs();
            if (bombs.Count != 0) {
                result = true;
                ClickSquareList(bombs, true);
            }
            var safe = Board.GetSafeSquares();
            if (safe.Count != 0) {
                result = true;
                ClickSquareList(safe);
            }
            return result;
        }

        /// <summary>
        ///     Grab an image of how the solver sees the board
        /// </summary>
        /// <returns>Bitmap of the solver's perceptionk</returns>
        public virtual Bitmap GetBrainImage() {
            throw new NotImplementedException("GetBrainImage not implemented!");
        }
    }

}