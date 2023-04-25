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
            return new Line()
            {
                X1 = Start.X,
                Y1 = Start.Y,
                X2 = End.X,
                Y2 = End.Y,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = thickness
            };
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
