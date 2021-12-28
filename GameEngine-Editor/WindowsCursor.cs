using System.Drawing;
using System.Runtime.InteropServices;

namespace Editor; 

internal static class CursorPosition {
    [StructLayout(LayoutKind.Sequential)]
    public struct PointInter {
        public int X;
        public int Y;
        public static explicit operator Point(PointInter point) => new Point(point.X, point.Y);
    }

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out PointInter lpPoint);

    // For your convenience
    public static Point GetCursorPosition() {
        PointInter lpPoint;
        GetCursorPos(out lpPoint);
        return (Point)lpPoint;
    }
}

