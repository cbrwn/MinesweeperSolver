using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MinesweeperSolver {

    public class InputHelper {
        [Flags]
        public enum MouseEventFlags {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static void SetCursorPosition(int X, int Y) {
            SetCursorPos(X, Y);
        }

        public static void SetCursorPosition(MousePoint point) {
            SetCursorPos(point.X, point.Y);
        }

        public static MousePoint GetCursorPosition() {
            MousePoint currentMousePoint;
            var gotPoint = GetCursorPos(out currentMousePoint);
            if (!gotPoint)
                currentMousePoint = new MousePoint(0, 0);
            return currentMousePoint;
        }

        public static void MouseEvent(MouseEventFlags value) {
            var position = GetCursorPosition();

            mouse_event
            ((int) value,
            position.X,
            position.Y,
            0,
            0)
            ;
        }

        const int slp = 0;
        public static void LeftClick(int x, int y) {
            SetCursorPosition(x, y);
            Thread.Sleep(slp);
            MouseEvent(MouseEventFlags.LeftDown);
            Thread.Sleep(slp);
            MouseEvent(MouseEventFlags.LeftUp);
        }

        public static void RightClick(int x, int y) {
            SetCursorPosition(x, y);
            Thread.Sleep(slp);
            MouseEvent(MouseEventFlags.RightDown);
            Thread.Sleep(slp);
            MouseEvent(MouseEventFlags.RightUp);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint {
            public int X;
            public int Y;

            public MousePoint(int x, int y) {
                X = x;
                Y = y;
            }
        }
    }

}