using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MinesweeperSolver {

    public static class ScreenHelper {
        /// <summary>
        ///     Grabs a screenshot of all monitors and converts it to 24bppRgb
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetDesktopScreenshot() {
            // I use SystemInformation.VirtualScreen here to get the dimensions of all monitors together
            var result = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            using (var g = Graphics.FromImage(result))
                g.CopyFromScreen(new Point(SystemInformation.VirtualScreen.Left, SystemInformation.VirtualScreen.Top), Point.Empty, result.Size);
            return result.Clone(new Rectangle(0, 0, result.Width, result.Height), PixelFormat.Format24bppRgb);
        }

        /// <summary>
        ///     Grabs a screenshot of the Minesweeper window, returns null if the window hasn't been found
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetMinesweeperScreenshot() {
            if (MineSolver.MinesweeperWindow == Rectangle.Empty)
                return null;
            var result = new Bitmap(MineSolver.MinesweeperWindow.Width, MineSolver.MinesweeperWindow.Height);
            using (var g = Graphics.FromImage(result))
            // VirtualScreen + Window because VirtualScreens go negative and the position in the image is obviously positive
                g.CopyFromScreen(SystemInformation.VirtualScreen.Left + MineSolver.MinesweeperWindow.X, SystemInformation.VirtualScreen.Top + MineSolver.MinesweeperWindow.Y, 0, 0, MineSolver.MinesweeperWindow.Size);
            return result;
        }

        /// <summary>
        ///     Gets the position of a bitmap inside another bitmap
        ///     Mostly from StackOverflow, with some changes for things like start position and a non-list result
        ///     http://stackoverflow.com/questions/28586793/c-sharp-search-for-a-bitmap-within-another-bitmap
        /// </summary>
        /// <param name="source">The bitmap to search inside</param>
        /// <param name="search">The bitmap to search for</param>
        /// <param name="startx">x position to start searching from</param>
        /// <param name="starty">y position to start searching from</param>
        /// <returns></returns>
        public static Point GetBitmapPosition(Bitmap source, Bitmap search, int startx = 0, int starty = 0) {
            if (source == null || search == null)
                throw new ArgumentNullException();
            if (source.PixelFormat != search.PixelFormat)
                throw new ArgumentException($"Incompatible pixel formats: {source.PixelFormat} vs {search.PixelFormat}");
            if (source.Width < search.Width || source.Height < search.Height)
                throw new ArgumentException("Search image is larger than source image");

            var pixSize = Image.GetPixelFormatSize(source.PixelFormat)/8;

            // Stick images into array
            var sourceData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, source.PixelFormat);
            var sourceBytes = new byte[sourceData.Stride*source.Height];
            Marshal.Copy(sourceData.Scan0, sourceBytes, 0, sourceBytes.Length);
            source.UnlockBits(sourceData);

            var searchData = search.LockBits(new Rectangle(0, 0, search.Width, search.Height), ImageLockMode.ReadOnly, search.PixelFormat);
            var searchBytes = new byte[searchData.Stride*search.Height];
            Marshal.Copy(searchData.Scan0, searchBytes, 0, searchBytes.Length);

            // Now we search
            for (var y = starty; y < source.Height - search.Height + 1; y++) {
                var sy = y*sourceData.Stride;
                for (var x = startx; x < source.Width - search.Width + 1; x++) {
                    var sx = x*pixSize;

                    var isEqual = true;
                    for (var c = 0; c < pixSize; c++) {
                        if (sourceBytes[sx + sy + c] == searchBytes[c])
                            continue;
                        isEqual = false;
                        break;
                    }
                    if (!isEqual)
                        continue;

                    var shouldStop = false;
                    for (var y1 = 0; y1 < search.Height; y1++) {
                        var my = y1*searchData.Stride;
                        var sourcey = (y + y1)*sourceData.Stride;
                        for (var x1 = 0; x1 < search.Width; x1++) {
                            var mx = x1*pixSize;
                            var sourcex = (x + x1)*pixSize;

                            for (var c = 0; c < pixSize; c++) {
                                if (sourceBytes[sourcex + sourcey + c] == searchBytes[mx + my + c])
                                    continue;
                                shouldStop = true;
                                break;
                            }
                            if (shouldStop)
                                break;
                        }
                        if (shouldStop)
                            break;
                    }

                    if (!shouldStop)
                        return new Point(x, y);
                }
            }
            return Point.Empty;
        }
    }

}