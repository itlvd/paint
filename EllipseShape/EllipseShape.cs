using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using IContract;

namespace EclipseShape
{
    public class EllipseShape : IShape
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public string Name => "Ellipse";

        public string Icon => "./img/circle.png";

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
            double width = Math.Abs(End.X - Start.X);
            double height = Math.Abs(End.Y - Start.Y);

            var shape = new Ellipse()
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Color),
                StrokeThickness = Size
            };

            Canvas.SetLeft(shape, Start.X);
            Canvas.SetTop(shape, Start.Y);
            return shape;
        }
        public bool isTouch(Point p)
        {
            return false;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
