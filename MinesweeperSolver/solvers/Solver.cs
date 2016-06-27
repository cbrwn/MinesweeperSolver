using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MinesweeperSolver.Properties;

namespace MinesweeperSolver.solvers {

    public class Solver {
        public Board Board;
        public Rectangle MinesweeperWindow = Rectangle.Empty;

        public Solver() {
            var screenshot = GetMinesweeperScreenshot();
            if (screenshot == null)
                throw new ArgumentNullException();
            Board = new Board(screenshot);
        }

        public Solver(Board board) {
            Board = board;
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
            check = check && MinesweeperWindow != Rectangle.Empty;
            var desktop = ScreenHelper.GetDesktopScreenshot();
            var topLeft = ScreenHelper.GetBitmapPosition(desktop, Resources.topleft, check ? MinesweeperWindow.X - 1 : 0, check ? MinesweeperWindow.Y - 1 : 0);
            if (check)
                return topLeft != Point.Empty;
            var bottomRight = topLeft != Point.Empty ? ScreenHelper.GetBitmapPosition(desktop, Resources.bottomright, topLeft.X + 20, topLeft.Y + 20) : Point.Empty;
            MinesweeperWindow = bottomRight != Point.Empty ? new Rectangle(topLeft.X, topLeft.Y, bottomRight.X + Resources.bottomright.Width - topLeft.X, bottomRight.Y + Resources.bottomright.Height - topLeft.Y) : Rectangle.Empty;
            return MinesweeperWindow != Rectangle.Empty;
        }

        /// <summary>
        ///     Clicks the restart button on the Minesweeper window
        /// </summary>
        public void ClickSweeperRestart() {
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
        public void ClickSweeperSquare(int x, int y, bool flag = false) {
            if (Board.GetSquare(x, y) != -1)
                return;
            // Leftmost + window pos + offset + col or row + half width
            var posx = SystemInformation.VirtualScreen.Left + MinesweeperWindow.X + 8 + 16*x + 8;
            var posy = SystemInformation.VirtualScreen.Top + MinesweeperWindow.Y + 50 + 16*y + 8;

            if (flag) {
                InputHelper.RightClick(posx, posy);
                Board.Squares[y, x] = 9;
            } else {
                InputHelper.LeftClick(posx, posy);
                Board.Squares[y, x] = -3;
            }
            Thread.Sleep(6);
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
        public void ClickSquareList(IEnumerable<Point> squares, bool flag = false) {
            foreach (var p in squares)
                ClickSweeperSquare(p, flag);
        }

        /// <summary>
        ///     Grabs a screenshot of the Minesweeper window, returns null if the window hasn't been found
        /// </summary>
        /// <returns>A screenshot of the whole Minesweeper window</returns>
        public Bitmap GetMinesweeperScreenshot() {
            if (MinesweeperWindow == Rectangle.Empty)
                FindSweeperWindow();
            if (MinesweeperWindow == Rectangle.Empty)
                return null;
            var result = new Bitmap(MinesweeperWindow.Width, MinesweeperWindow.Height);
            using (var g = Graphics.FromImage(result))
            // VirtualScreen + Window because VirtualScreens go negative and the position in the image is obviously positive
                g.CopyFromScreen(SystemInformation.VirtualScreen.Left + MinesweeperWindow.X, SystemInformation.VirtualScreen.Top + MinesweeperWindow.Y, 0, 0, MinesweeperWindow.Size);
            return result;
        }

        public virtual void Update() {
            Board.Update(GetMinesweeperScreenshot(), true);
        }

        public virtual void DoMove() {
            throw new NotImplementedException("DoMove not implemented!");
        }

        public virtual Bitmap GetBrainImage() {
            throw new NotImplementedException("GetBrainImage not implemented!");
        }
    }

}