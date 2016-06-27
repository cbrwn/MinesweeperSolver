using System;
using System.Windows.Forms;

namespace MinesweeperSolver {

    internal static class MineSolver {
        // Width/Height of each square
        public const int SQUARE_SIZE = 16;
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
    }

}