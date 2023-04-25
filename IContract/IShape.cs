using System;
using System.Windows;

namespace IContract
{
    public interface IShape : ICloneable
    {
        string Name { get; }
        string Icon {  get; }
        void UpdateStart(System.Windows.Point p);
        void UpdateEnd(System.Windows.Point p);
        UIElement Draw(System.Windows.Media.Color color, int thickness);
    }
}
