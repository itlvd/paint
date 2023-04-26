using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using IContract;

namespace LineShape
{
    public class LineShape : IShape
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public string Name => "Line";
        public string Icon => "./img/line.png";
        public int Size { get; set; } = 2;
        public Color Color { get; set; } = Colors.Black;

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
            return new Line()
            {
                X1 = Start.X,
                Y1 = Start.Y,
                X2 = End.X,
                Y2 = End.Y,
                Stroke = new SolidColorBrush(Color),
                StrokeThickness = Size
            };
        }
        private double _getDistance(Point p1, Point p2)
        {
            double distance = Math.Sqrt( (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            
            return distance;
        }
        public bool isTouch(Point p)
        {
            return _getDistance(Start, End) + 0.01 >= _getDistance(Start, p) + _getDistance(End, p);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
