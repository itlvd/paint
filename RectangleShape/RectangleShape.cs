using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using IContract;
using Color = System.Drawing.Color;

namespace RectangleShape
{
    [Serializable]
    public class RectangleShape : IShape
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public string Name => "Rectangle";
        public string Icon => "./img/rectangular.png";
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

            var shape = new Rectangle()
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
            if (End.X - Start.X < 0 && End.Y - Start.Y < 0)
            {
                return p.X <= Start.X && p.X >= End.X && p.Y <= Start.Y && p.Y >= End.Y;
            }
            else if (End.X - Start.X < 0 && End.Y - Start.Y > 0)
            {
                return p.X <= Start.X && p.X >= End.X && p.Y >= Start.Y && p.Y <= End.Y;
            }
            else if (End.X - Start.X > 0 && End.Y - Start.Y < 0)
            {
                return p.X >= Start.X && p.X <= End.X && p.Y <= Start.Y && p.Y >= End.Y;
            }
            else
            {
                return p.X >= Start.X && p.X <= End.X && p.Y >= Start.Y && p.Y <= End.Y;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
