using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace IContract
{
    public interface IShape : ICloneable
    {
        string Name { get; }
        string Icon { get; }
        int Size { get; set; }
        System.Drawing.Color Color { get; set; }
        System.Drawing.Color FillColor { get; set; }

        string StrokeStyle { get; set; }
        void UpdateStart(System.Windows.Point p);
        void UpdateEnd(System.Windows.Point p);
        void UpdateSize(int size);
        void UpdateColor(System.Drawing.Color color);

        bool isTouch(System.Windows.Point p);
        UIElement Draw();
    }
}
