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

        public void UpdateStart(Point p)
        {
            Start = p;
        }
        public void UpdateEnd(Point p)
        {
            End = p;
        }

        public UIElement Draw(Color color, int thickness)
        {
            double width = Math.Abs(End.X - Start.X);
            double height = Math.Abs(End.Y - Start.Y);

            var shape = new Ellipse()
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = thickness
            };

            Canvas.SetLeft(shape, Start.X);
            Canvas.SetTop(shape, Start.Y);
            return shape;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
