using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using Color = System.Drawing.Color;
using IContract;

namespace EclipseShape
{
    [Serializable]
    public class EllipseShape : IShape
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public string Name => "Ellipse";

        public string Icon => "./img/circle.png";

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
            double width = Math.Abs(End.X - Start.X);
            double height = Math.Abs(End.Y - Start.Y);
            var _StrokeDashArray = Array.ConvertAll(StrokeStyle.Split(" "), Double.Parse);
            var shape = new Ellipse()
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Color.A, Color.R, Color.G, Color.B)),
                StrokeThickness = Size,
                StrokeDashArray = new DoubleCollection(_StrokeDashArray),
                Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(FillColor.A, FillColor.R, FillColor.G, FillColor.B))
            };

            if (End.X - Start.X < 0 && End.Y - Start.Y < 0)
            {
                Canvas.SetLeft(shape, End.X);
                Canvas.SetTop(shape, End.Y);
            }
            else if (End.X - Start.X < 0 && End.Y - Start.Y > 0)
            {
                Canvas.SetLeft(shape, End.X);
                Canvas.SetTop(shape, Start.Y);
            }
            else if (End.X - Start.X > 0 && End.Y - Start.Y < 0)
            {
                Canvas.SetLeft(shape, Start.X);
                Canvas.SetTop(shape, End.Y);
            }
            else
            {
                Canvas.SetLeft(shape, Start.X);
                Canvas.SetTop(shape, Start.Y);
            }
            return shape;
        }
        public bool isTouch(Point p)
        {
            Point start, end;
            if (End.X - Start.X < 0 && End.Y - Start.Y < 0)
            {
                start = new Point(End.X, End.Y);
                end = new Point(Start.X, Start.Y);
            }
            else if (End.X - Start.X < 0 && End.Y - Start.Y > 0)
            {
                start = new Point(End.X, Start.Y);
                end = new Point(Start.X, End.Y);
            }
            else if (End.X - Start.X > 0 && End.Y - Start.Y < 0)
            {
                start = new Point(Start.X, End.Y);
                end = new Point(End.X, Start.Y);
            }
            else
            {
                start = Start;
                end = End;
            }
            double width = Math.Abs(end.X - start.X);
            double height = Math.Abs(end.Y - start.Y);

            double a =  width / 2;
            double b =  height / 2;

            double h = Math.Abs(end.X - a);
            double k = Math.Abs(end.Y - b);

            return ((p.X - h) * (p.X - h)) / (a * a) + ((p.Y - k) * (p.Y - k)) / (b * b) <= 1.0;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
