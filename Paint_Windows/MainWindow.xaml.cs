using Fluent;
using IContract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace paintting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Dictionary<string, IShape> _abilities =
            new Dictionary<string, IShape>();

        bool _isDrawing = false;
        IShape? _prototype = null;
        string _selectedType = "";
        Color _selectedColor = Colors.Black;
        int _selectedThickness = 2;

        Point _start;
        Point _end;

        List<IShape> _shapes = new List<IShape>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Tự scan chương trình nạp lên các khả năng của mình
            var domain = AppDomain.CurrentDomain;
            var folder = domain.BaseDirectory;
            var folderInfo = new DirectoryInfo(folder);
            var dllFiles = folderInfo.GetFiles("*.dll");

            foreach (var dll in dllFiles)
            {
                Debug.WriteLine(dll.FullName);
                var assembly = Assembly.LoadFrom(dll.FullName);

                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass &&
                        typeof(IShape).IsAssignableFrom(type))
                    {
                        var shape = Activator.CreateInstance(type) as IShape;
                        _abilities.Add(shape!.Name, shape);
                    }
                }
            }

            foreach (var ability in _abilities)
            {
                var button = new System.Windows.Controls.Button()
                {
                    Width = 80,
                    Height = 35,
                    Content = ability.Value.Name,
                    Tag = ability.Value.Name
                };
                button.Click += ability_Click;
                abilitiesStackPanel.Children.Add(button);
            }
        }

        private void ability_Click(object sender, RoutedEventArgs e)
        {
            var button = (System.Windows.Controls.Button)sender;
            string name = (string)button.Tag;
            _selectedType = name;
        }


        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;
            _start = e.GetPosition(actualCanvas);


            _prototype = (IShape)
                _abilities[_selectedType].Clone();
            _prototype.UpdateStart(_start);
        }


        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                actualCanvas.Children.Clear();

                foreach (var shape in _shapes)
                {
                    UIElement oldShape = shape.Draw(_selectedColor, _selectedThickness);
                    actualCanvas.Children.Add(oldShape);
                }

                _end = e.GetPosition(actualCanvas);
                _prototype.UpdateEnd(_end);

                UIElement newShape = _prototype.Draw(_selectedColor, _selectedThickness);
                actualCanvas.Children.Add(newShape);
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _shapes.Add((IShape)_prototype.Clone());

            _isDrawing = false;

            Title = "Up";
        }

        private void OpenFile_BackstageTabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
