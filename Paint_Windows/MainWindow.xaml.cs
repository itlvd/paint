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
        bool _isEraser = false;

        IShape? _prototype = null;
        string _selectedType = "";
        int _selectedSize = 2;
        Color _selectedColor = Colors.Black;
        string _strokeStype = "1 0";

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
                var button = new Fluent.Button()
                {
                    SizeDefinition = "Large",
                    Content = ability.Value.Name,
                    Tag = ability.Value.Name,
                    Icon = ability.Value.Icon,
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
            _start = new Point(0.0, 0.0);
            _end = new Point(0.0, 0.0);
            if (!_isEraser)
            {
                _start = e.GetPosition(actualCanvas);

                if (_selectedType == "")
                {
                    _selectedType = "Line";
                }

                _prototype = (IShape)
                    _abilities[_selectedType].Clone();
                _prototype.UpdateStart(_start);

                _prototype.UpdateSize(_selectedSize);
                _prototype.UpdateColor(_selectedColor);
                _prototype.StrokeStyle = _strokeStype;
            }
        }


        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                actualCanvas.Children.Clear();

                _end = e.GetPosition(actualCanvas);

                foreach (var shape in _shapes)
                {
                    UIElement oldShape = shape.Draw();
                    actualCanvas.Children.Add(oldShape);
                }

                

                if (!_isEraser) // khong xoa thi ve
                {
                    _prototype.UpdateEnd(_end);

                    UIElement newShape = _prototype.Draw();
                    actualCanvas.Children.Add(newShape);
                }
                else // dang che do nhan nut xoa
                {
                    foreach (var shape in _shapes)
                    {
                        if(shape.isTouch(_end)) {
                            _shapes.Remove(shape);
                            break;
                        }
                    }
                }
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isEraser)
            {
                _shapes.Add((IShape)_prototype.Clone());
            }
            

            _isDrawing = false;
            _start = new Point(0.0,0.0);
            _end = new Point(0.0, 0.0);

            //Title = "Up";
        }

        private void OpenFile_BackstageTabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Stroke_Big_Clicked(object sender, RoutedEventArgs e)
        {
            _selectedSize = 20;
            DropdownSizeBtn.Icon = "./img/stroke_big.png";
        }

        private void Stroke_Medium_Clicked(object sender, RoutedEventArgs e)
        {
            _selectedSize = 10;
            DropdownSizeBtn.Icon = "./img/stroke_medium.png";
        }

        private void Stroke_Light_Clicked(object sender, RoutedEventArgs e)
        {
            _selectedSize = 2;
            DropdownSizeBtn.Icon = "./img/stroke_light.png";
        }

        private void Color_Black_Clicked(object sender, RoutedEventArgs e)
        {
            _selectedColor = Colors.Black;
            DropdownColorBtn.Background = Brushes.White;
            DropdownColorBtn.Foreground = Brushes.Black;
        }

        private void Color_Red_Clicked(object sender, RoutedEventArgs e)
        {
            _selectedColor = Colors.Red;
            DropdownColorBtn.Background = Brushes.Red;
        }

        private void Color_Blue_Clicked(object sender, RoutedEventArgs e)
        {
            _selectedColor = Colors.Blue;
            DropdownColorBtn.Background = Brushes.Blue;
            DropdownColorBtn.Foreground = Brushes.White;
        }

        private void Color_Green_Clicked(object sender, RoutedEventArgs e)
        {
            _selectedColor = Colors.Green;
            DropdownColorBtn.Background = Brushes.Green;
            DropdownColorBtn.Foreground = Brushes.White;
        }

        private void Is_Eraser_Btn(object sender, RoutedEventArgs e)
        {
            _isEraser = _isEraser == false? true: false;
        }

        private void Stroke_Style_1_Clicked(object sender, RoutedEventArgs e)
        {
            _strokeStype = "1 0";
            Stroke_Style_Btn.Icon = new Rectangle()
            {
                Width = 80,
                Height = 2,
                Stroke = Brushes.Black,
                StrokeDashArray = { 1 , 0},
                StrokeThickness = 2,
            };
        }

        private void Stroke_Style4_1_1_Clicked(object sender, RoutedEventArgs e)
        {
            _strokeStype = "4 1 1";
            Stroke_Style_Btn.Icon = new Rectangle()
            {
                Width = 80,
                Height = 2,
                Stroke = Brushes.Black,
                StrokeDashArray = { 4, 1, 1 },
                StrokeThickness = 2,
            };
        }

        private void Stroke_Style_1_6_Clicked(object sender, RoutedEventArgs e)
        {
            _strokeStype = "1 6";
            Stroke_Style_Btn.Icon = new Rectangle()
            {
                Width = 80,
                Height = 2,
                Stroke = Brushes.Black,
                StrokeDashArray = { 1, 6 },
                StrokeThickness = 2,
            };
        }
    }
}
