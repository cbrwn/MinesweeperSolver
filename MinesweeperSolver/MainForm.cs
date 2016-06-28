using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MinesweeperSolver.solvers;

namespace MinesweeperSolver {

    public partial class MainForm : Form {
        private readonly string _screenshotLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MinesweeperFails");
        private Solver _solver;

        public MainForm() {
            InitializeComponent();

            if (!Directory.Exists(_screenshotLocation))
                Directory.CreateDirectory(_screenshotLocation);
            SetToolTips();
        }

        private void SetToolTips() {
            var t = new ToolTip();
            t.SetToolTip(tckTries, @"Set to 0 for unlimited");
            t.SetToolTip(lblMaxTries, @"Set to 0 for unlimited");
        }

        private void StartSolve(object sender, EventArgs e) {
            MineSolver.Running = !MineSolver.Running;
            if (!MineSolver.Running)
                btnSolve.Enabled = false; // Let the thread re-enable this when it's stopped
            else {
                _solver = new ProbabilitySolver();
                new Thread(SolveSweeper).Start();
            }
        }

        // Thread functions

        /// <summary>
        ///     Finds the Minesweeper window, and if found, starts to solve
        ///     It does this by taking a screenshot, analyzing the numbers based on colour, and performing the best action
        ///     Updates the UI with the screenshot which shows exactly what's being analyzed
        ///     Saves a screenshot of the current board when it fails, then restarts
        /// </summary>
        private void SolveSweeper() {
            Invoke((MethodInvoker) delegate {
                btnSolve.Text = @"Stop";
                strWaiting.Visible = true;
                strStatus.Text = @"Finding Minesweeper...";
            });

            if (!_solver.FindSweeperWindow()) {
                Invoke((MethodInvoker) delegate {
                    btnSolve.Text = @"Solve";
                    strWaiting.Visible = false;
                    strStatus.Text = @"Couldn't find game!";
                });
                MineSolver.Running = false;
                return;
            }

            // Timer for checking if the Minesweeper window is still there
            var tmr = new Stopwatch();
            tmr.Start();

            var willDoubleScreenshot = false; // whether or not the bot screenshotted - to stop multiple screenshots without limiting performance

            Invoke((MethodInvoker) delegate {
                strWaiting.Style = ProgressBarStyle.Continuous;
                strWaiting.Value = 0;
                strWaiting.Maximum = 100;
                strStatus.Text = @"Solving...";
            });

            var wins = 0;
            var losses = 0;
            var clears = new List<int>();
            var tries = 0;
            // Stop this loop on button click (Running) or form exit
            while (MineSolver.Running && !IsDisposed) {
                _solver.Update();
                imgGame.Image = _solver.GetBrainImage();

                if (_solver.Board.IsFailed || _solver.Board.IsComplete) {
                    if (willDoubleScreenshot)
                        continue;
                    willDoubleScreenshot = true;

                    // Screenshot stuff
                    var saveThis = (chkSaveWin.Checked && _solver.Board.IsComplete) || (chkSaveLoss.Checked && _solver.Board.IsFailed);
                    if (saveThis) {
                        var directory = Path.Combine(_screenshotLocation, _solver.Board.IsComplete ? @"Wins" : $"{_solver.Board.Columns}x{_solver.Board.Rows}");
                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);
                        var filename = Path.Combine(directory, (_solver.Board.IsComplete ? @"win" : $"{_solver.Board.Score}%") + $"-{(int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds}-");
                        if (chkSaveBrain.Checked) {
                            _solver.GetBrainImage().Save(filename + "brain.png");
                            Console.WriteLine($" -> Saved screenshot to {filename.Replace(AppDomain.CurrentDomain.BaseDirectory, "")}brain.png");
                        }
                        if (chkSaveGame.Checked) {
                            _solver.GetMinesweeperScreenshot().Save(filename + "game.png");
                            Console.WriteLine($" -> Saved screenshot to {filename.Replace(AppDomain.CurrentDomain.BaseDirectory, "")}game.png");
                        }
                    }

                    tries++;
                    if (_solver.Board.IsFailed)
                        losses++;
                    else
                        wins++;
                    clears.Add(_solver.Board.Score);
                    UpdateAverages(clears, wins, losses);
                    if (tckTries.Value > 0 && tries > tckTries.Value)
                        break;
                    if (chkStopWin.Checked && _solver.Board.IsComplete)
                        break;
                    _solver.ClickSweeperRestart();
                    continue;
                }
                willDoubleScreenshot = false;

                // New board
                if (_solver.Board.IsNew) {
                    Console.WriteLine($"============================\nNew game!");
                    _solver.ClickSweeperSquare(0, 0);
                    continue;
                }

                // Clicks
                _solver.DoMove();

                // Window check, every second because checking too often massively hurts performance
                if (tmr.Elapsed.TotalMilliseconds >= 1000) {
                    tmr.Restart();
                    if (!_solver.FindSweeperWindow(true))
                        break;
                }
                Invoke((MethodInvoker) delegate { strWaiting.Value = _solver.Board.Score; });
            }

            // Done!
            MineSolver.Running = false;
            if (!IsDisposed) {
                Invoke((MethodInvoker) delegate {
                    btnSolve.Enabled = true;
                    strWaiting.Visible = false;
                    btnSolve.Text = @"Solve";
                    strStatus.Text = @"Finished " + (_solver.Board.IsComplete ? " successfully!" : " with a loss :(");
                });
            }
        }

        private void UpdateAverages(IEnumerable<int> clears, int wins, int losses) {
            Invoke((MethodInvoker) delegate {
                strAvgClear.Text = $"{(int) clears.Average()}% Avg. Clear";
                strWinRate.Text = $"{100d*((double) wins/(wins + losses))}% Win Rate";
            });
        }
    }

}