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

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
