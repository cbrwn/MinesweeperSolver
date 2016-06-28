﻿using System;
using System.Diagnostics;
using System.Drawing;

namespace MinesweeperSolver.solvers {

    internal class ProbabilitySolver : Solver {
        private int[,] _prob;

        private void UpdateProbabilities() {
            _prob = new int[Board.Rows, Board.Columns];
            for (var y = 0; y < Board.Rows; y++) {
                for (var x = 0; x < Board.Columns; x++)
                    _prob[y, x] = -1;
            }
            for (var y = 0; y < Board.Rows; y++) {
                for (var x = 0; x < Board.Columns; x++) {
                    var val = Board.GetSquare(x, y);
                    if (!(val >= 0 && val < 9)) { // Only want to process clicked squares, we'll process the squares around them using their value
                        continue;
                    }
                    var sur = Board.GetSurroundingClicks(x, y);
                    if (sur.Count == 0) // If there's no more unopened squares around it, there's no potential bombs to search for
                        continue;
                    var bombCount = Board.GetSurroundingBombs(x, y).Count;
                    var thisprob = 100d*((val - bombCount)/(double) sur.Count);
                    foreach (var p in sur)
                        _prob[p.Y, p.X] = (int) Math.Max(thisprob, _prob[p.Y, p.X]);
                }
            }

            for (var y = 0; y < Board.Rows; y++) {
                for (var x = 0; x < Board.Columns; x++) {
                    if (_prob[y, x] == -1)
                        _prob[y, x] = 101;
                }
            }
        }

        public override void DoMove() {
            // TODO: Consider making this base.DoMove and just using DoMove for non-obvious moves
            // Squares which require no guessing
            var clicked = false;
            var tmr = new Stopwatch();
            tmr.Start();
            for (var y = 0; y < Board.Rows; y++) {
                for (var x = 0; x < Board.Columns; x++) {
                    if (Board.GetSquare(x, y) == 0)
                        continue;
                    var clicks = Board.GetSurroundingClicks(x, y);
                    var bombs = Board.GetSurroundingBombs(x, y);
                    if (clicks.Count == 0)
                        continue;
                    // Click a square around a clicked where the clicked's bombs have all been found
                    if (Board.GetSquare(x, y) - bombs.Count == 0) {
                        ClickSquareList(clicks);
                        clicked = true;
                    }

                    // Flag a guaranteed bomb
                    if (Board.GetSquare(x, y) != clicks.Count + bombs.Count)
                        continue;
                    ClickSquareList(clicks, true);
                    clicked = true;
                }
                if (!(tmr.Elapsed.TotalMilliseconds >= 500))
                    continue;
                tmr.Restart();
                if (!FindSweeperWindow(true))
                    return;
            }
            if (clicked) // Leave this method and re-process the board, no need to start guessing yet
                return;

            // Now we need to guess
            UpdateProbabilities();
            var lowestProb = 1000d;
            int lx = 0, ly = 0;
            for (var y = 0; y < Board.Rows; y++) {
                for (var x = 0; x < Board.Columns; x++) {
                    if (Board.GetSquare(x, y) != -1) // Only want to search through unclicked (-1)
                        continue;
                    var prob = _prob[y, x];
                    if (prob >= lowestProb)
                        continue;
                    lowestProb = prob;
                    lx = x;
                    ly = y;
                }
            }
            Console.WriteLine($"Guessed ({lx},{ly}) with probability of {lowestProb}%");
            // Guess that it's a bomb if the bomb probability is over 50%
            // Except if the bomb probability is 100% - then something must have gone wrong so we can click it and fail
            ClickSweeperSquare(lx, ly, lowestProb > 50 && lowestProb >= 100);
        }

        public override Bitmap GetBrainImage() {
            UpdateProbabilities();

            const int size = 32;
            var fontMulti = (int)(size / 8d);
            var result = new Bitmap(Board.Columns * size, Board.Rows * size);
            using (var g = Graphics.FromImage(result)) {
                for (var y = 0; y < Board.Rows; y++) {
                    for (var x = 0; x < Board.Columns; x++) {
                        var col = Board.NumberColors[0];
                        var pos = _prob[y, x];
                        var val = Board.GetSquare(x, y);
                        if (val == 9)
                            col = Color.Magenta;
                        else if (val > -1 && val < 9)
                            col = Board.NumberColors[0];
                        else if (pos > 100)
                            col = Color.DarkBlue;
                        else if (pos > 50) {
                            var n = (int)(255 * ((pos - 50) / 50d));
                            col = Color.FromArgb(255, Math.Abs(n - 255), 0);
                        } else if (pos >= 0) {
                            var n = (int)(255 * (pos / 50d));
                            col = Color.FromArgb(n, 255, 0);
                        }
                        g.FillRectangle(new SolidBrush(col), x * size, y * size, size, size);
                        if (val > 0 && val < 9) {
                            var textCol = Board.NumberColors[val];
                            g.DrawString(val.ToString(), new Font(FontFamily.GenericMonospace, 5 * fontMulti, FontStyle.Bold), new SolidBrush(textCol), fontMulti + x * size, fontMulti / 2 + y * size);
                        }
                        g.DrawRectangle(new Pen(Color.Gray), x * size, y * size, size, size);
                    }
                }
            }
            return result;
        }
    }

}