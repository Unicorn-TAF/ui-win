using System.Windows;

namespace Unicorn.UI.Core.Input
{
    public static class CoordinatesExtension
    {
        public static Point ConvertToWindowsPoint(this System.Drawing.Point point)
        {
            return new Point(point.X, point.Y);
        }

        public static System.Drawing.Point ToDrawingPoint(this Point point)
        {
            return new System.Drawing.Point((int)point.X, (int)point.Y);
        }

        public static bool IsInvalid(this Point point)
        {
            return point.X.IsInvalid() || point.Y.IsInvalid();
        }

        public static bool IsInvalid(this double @double)
        {
            return @double == double.PositiveInfinity || @double == double.NegativeInfinity || double.IsNaN(@double);
        }
    }
}
