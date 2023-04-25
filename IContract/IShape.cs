using System;
using System.Windows;
using System.Windows.Media;

namespace IContract
{
    public interface IShape : ICloneable
    {
        string Name { get; }
        string Icon {  get; }
        int Size { get; set; }
        Color Color { get; set; }
        void UpdateStart(System.Windows.Point p);
        void UpdateEnd(System.Windows.Point p);
        void UpdateSize(int size);
        void UpdateColor(Color color);
        UIElement Draw();
    }
}
