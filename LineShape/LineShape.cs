using System;
using Color = System.Drawing.Color;
using System.Windows.Shapes;
using System.Windows;
using IContract;
using System.Windows.Media;

namespace LineShape
{
    [Serializable]
    public class LineShape : IShape
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public string Name => "Line";
        public string Icon => "./img/line.png";
        public int Size { get; set; } = 2;
        public Color Color { get; set; } = Color.Black;
        public string StrokeStyle { get; set; } = "1 0";
        public Color FillColor { get; set; } = Color.White;

        public void UpdateStart(Point p)
        {
            Start = p;
        }
        public void UpdateEnd(Point p)
        {
            End = p;
        }

        public void UpdateSize(int size)
        {
            Size = size;
        }

        public void UpdateColor(Color color)
        {
            Color = color;
        }

        public UIElement Draw()
        {
            var _StrokeDashArray = Array.ConvertAll(StrokeStyle.Split(" "), Double.Parse);

            return new Line()
            {
                X1 = Start.X,
                Y1 = Start.Y,
                X2 = End.X,
                Y2 = End.Y,
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Color.A, Color.R, Color.G, Color.B)),
                StrokeThickness = Size,
                StrokeDashArray = new DoubleCollection(_StrokeDashArray)
            };
        }
        private double _getDistance(Point p1, Point p2)
        {
            double distance = Math.Sqrt( (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            
            return distance;
        }
        public bool isTouch(Point p)
        {
            return _getDistance(Start, End) + 0.1 >= _getDistance(Start, p) + _getDistance(End, p); // tăng độ nhạy bằng cách tăng sai số lên.
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
