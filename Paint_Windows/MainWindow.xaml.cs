using Fluent;
using IContract;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
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
        bool _isFillColor = false;
        bool _isPressMouse = false; // Nếu không có biến này thì chỉ cần chấm 2 điểm là nó vẽ, bắt buộc phải kéo xong thả chuột thì mới được phép vẽ.

        IShape? _prototype = null;
        string _selectedType = "";
        int _selectedSize = 2;
        Color _selectedColor = Colors.Black;
        string _strokeStype = "1 0";

        Point _start;
        Point _end;

        ShapesList _shapes = new ShapesList();
        Stack<IShape> _undoStack = new Stack<IShape>();

        KeyBinding UndoKeyBinding = new KeyBinding(
            ApplicationCommands.Undo,
            Key.Z,
            ModifierKeys.Control);
        KeyBinding RedoKeyBinding = new KeyBinding(
           ApplicationCommands.Redo,
           Key.Y,
           ModifierKeys.Control);
        private bool _justErased = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InputBindings.Add(UndoKeyBinding);
            InputBindings.Add(RedoKeyBinding);
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo,
               (sender, e) => {
                   Title = "Project Paint" + " (Undo-ing: Ctrl-Z)";
                   if (!_justErased)
                   {
                       var shape = _shapes[_shapes.Count - 1].Clone();
                       _shapes.RemoveAt(_shapes.Count - 1); 
                       _undoStack.Push((IShape)shape);
                       actualCanvas.Children.RemoveAt(actualCanvas.Children.Count - 1);
                   }
                   else
                   {
                       var shape = _undoStack.Pop();
                       _shapes.Add(shape);
                       actualCanvas.Children.Add(shape.Draw());
                       _justErased = false;
                   }
               },
               (sender, e) => { 
                   e.CanExecute = _shapes.Count > 0 && !_isDrawing; 
               }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo,
               (sender, e) => {
                   Title = "Project Paint" + " (Redo-ing: Ctrl-Y)";

                   var shape = _undoStack.Pop();
                   _shapes.Add(shape);
                   actualCanvas.Children.Add(shape.Draw());
               },
               (sender, e) => {
                   e.CanExecute = _undoStack.Count > 0 && !_isDrawing;
               }));
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
            _undoStack.Clear();
            _justErased = false;

            Title = "Project Paint";

            _start = new Point(0.0, 0.0);
            _end = new Point(0.0, 0.0);

            _start = e.GetPosition(actualCanvas);

            if (_isFillColor)
            {
                _isDrawing = false;
                var newShapes = new ShapesList();
                foreach (var shape in _shapes)
                {
                    if(shape.isTouch(_start))
                    {
                        IShape newShape = (IShape)shape.Clone();
                        newShape.FillColor = System.Drawing.Color.FromArgb(_selectedColor.A, _selectedColor.R, _selectedColor.G, _selectedColor.B);
                        newShapes.Add(newShape);
                        actualCanvas.Children.Add(newShape.Draw());
                    }
                }
                _shapes.AddRange(newShapes);
            }
            else if (!_isEraser)
            {
                if (_selectedType == "")
                {
                    _selectedType = "Line";
                }

                _prototype = (IShape)
                    _abilities[_selectedType].Clone();
                _prototype.UpdateStart(_start);

                _prototype.UpdateSize(_selectedSize);
                _prototype.UpdateColor(System.Drawing.Color.FromArgb(_selectedColor.A, _selectedColor.R, _selectedColor.G, _selectedColor.B));
                _prototype.StrokeStyle = _strokeStype;
            }
        }


        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                _isPressMouse = true;

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
                    var newShapes = new ShapesList();
                    foreach (var shape in _shapes)
                    {
                        if(shape.isTouch(_end)) {
                            _undoStack.Push(shape);
                            _justErased = true;
                        }
                        else
                        {
                            newShapes.Add(shape);
                        }
                    }
                    _shapes = newShapes;
                }
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isFillColor)
            {
                return;
            }

            if (!_isEraser && _isDrawing && _isPressMouse)
            {
                _shapes.Add((IShape)_prototype.Clone());
            }
            

            _isDrawing = false;
            _isPressMouse = false;
            _start = new Point(0.0,0.0);
            _end = new Point(0.0, 0.0);

            //Title = "Up";
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

        private void Color_White_Clicked(object sender, RoutedEventArgs e)
        {
            _selectedColor = Colors.White;
            DropdownColorBtn.Background = Brushes.White;
            DropdownColorBtn.Foreground = Brushes.Black;
        }

        private void Color_Black_Clicked(object sender, RoutedEventArgs e)
        {
            _selectedColor = Colors.Black;
            DropdownColorBtn.Background = Brushes.Black;
            DropdownColorBtn.Foreground = Brushes.White;
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
            _isFillColor = false;
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

        private void Is_Fill_Btn(object sender, RoutedEventArgs e)
        {
            _isFillColor = _isFillColor == true? false: true;
            _isEraser = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Title = "Project Paint" + " (Saving: Ctrl-S)"; 
                SaveDrawing();
            }
        }
        public void SaveFile(object sender, RoutedEventArgs e)
        {
            int width = (int)actualCanvas.RenderSize.Width;
            int height = (int)actualCanvas.RenderSize.Height;

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            rtb.Render(actualCanvas);

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "IMG files| (*.png)";
            saveFileDialog.FileName = "Untitled " + DateTime.Now.Ticks + ".png";


            if (saveFileDialog.ShowDialog() == true)
            {
                using (var stream = saveFileDialog.OpenFile())
                {
                    pngEncoder.Save(stream);
                    //_shapes.Clear();
                    //_undoStack.Clear();
                    //actualCanvas.Children.Clear();
                    MessageBox.Show("Image saved successfully.");
                }
            }
            else
            {
                MessageBox.Show("Image saved failed.");
            }
        }

        private void SaveDrawing()
        {
            SaveFile(null, null);
        }

        public void SavePaintFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "All files (*.bin)|*.bin";
            saveFileDialog.FileName = "Untitled Shapes - " + DateTime.Now.Ticks + ".bin";


            if (saveFileDialog.ShowDialog() == true)
            {
                using (var stream = saveFileDialog.OpenFile())
                {
                    _shapes.Save(stream);
                    //_undoStack.Clear();
                    //_shapes.Clear();
                    //actualCanvas.Children.Clear();
                    MessageBox.Show("Shapes saved successfully.");
                }
            }
            else
            {
                MessageBox.Show("Image saved failed.");
            }
        }

        private void OpenFile_BackstageTabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.bin)|*.bin";
            if (openFileDialog.ShowDialog() == true)
            {
                using (var stream = openFileDialog.OpenFile())
                {
                    _shapes.Load(stream);
                    _undoStack.Clear();
                    actualCanvas.Children.Clear();
                    foreach (var shape in _shapes)
                    {
                        actualCanvas.Children.Add(shape.Draw());
                    }
                    backStage.IsOpen = false;
                }
            }
        }

        private void Is_Pen_Btn(object sender, RoutedEventArgs e)
        {
            _isEraser = false;
            _isFillColor = false;
        }
    }

    [Serializable]
    public class ShapesList : List<IShape>
    {
        public void Save(Stream stream)
        {
            try
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, this);
            } catch (Exception ex)
            {
            }
        }
        public void Load(Stream stream)
        {
            try
            {
                BinaryFormatter bin = new BinaryFormatter();
                var shapes = (ShapesList)bin.Deserialize(stream);
                this.Clear();
                this.AddRange(shapes);
            } catch (Exception ex)
            {
            }
        }
        public void Draw()
        {
            this.ForEach(x => x.Draw());
        }
    }
}
