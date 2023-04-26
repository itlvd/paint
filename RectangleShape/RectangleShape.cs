using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using IContract;
using System.Linq;

namespace RectangleShape
{
    public class RectangleShape : IShape
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public string Name => "Rectangle";
        public string Icon => "./img/rectangular.png";
        public int Size { get; set; } = 2;
        
        public Color Color { get; set; } = Colors.Black;
        public string StrokeStyle { get; set; } = "1 0";
        public Color FillColor { get; set; } = Colors.White;

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
            if (End.X - Start.X < 0)
            {
                Point x = Start;
                Start = End;
                End = x;
            }

            double width = Math.Abs(End.X - Start.X);
            double height = Math.Abs(End.Y - Start.Y);
            
            var _StrokeDashArray = Array.ConvertAll(StrokeStyle.Split(" "), Double.Parse);

            var shape = new Rectangle()
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Color),
                StrokeThickness = Size,
                StrokeDashArray =  new DoubleCollection(_StrokeDashArray),
                Fill = new SolidColorBrush(FillColor)
            };

            Canvas.SetLeft(shape, Start.X);
            Canvas.SetTop(shape, Start.Y);
            return shape;
        }
        public bool isTouch(Point p)
        {
            return p.X > Start.X && p.X < End.X && p.Y > Start.Y && p.Y < End.Y;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
