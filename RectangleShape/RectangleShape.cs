using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using IContract;

namespace RectangleShape
{
    public class RectangleShape : IShape
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public string Name => "Rectangle";
        public string Icon => "./img/rectangular.png";

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
            if (End.X - Start.X < 0)
            {
                Point x = Start;
                Start = End;
                End = x;
            }

            double width = Math.Abs(End.X - Start.X);
            double height = Math.Abs(End.Y - Start.Y);


            var shape = new Rectangle()
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