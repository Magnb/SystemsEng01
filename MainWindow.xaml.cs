using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RectanglesWithLinesApp01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool rectangleIsChecked = false;
        public bool lineIsChecked = false;
        private IList<Rectangle> rectangles = new List<Rectangle>();
        private Canvas drawingGrid;
        private Point? firstLinePoint = null;

        public MainWindow()
        {
            InitializeComponent();
            drawingGrid = FindName("drawGrid") as Canvas; 
            drawingGrid.PreviewMouseLeftButtonDown += OpenHelpDialog;
        }

        private void OpenHelpDialog(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Choose 'Rectangle' and then add some here or choose 'Lines' and connect them!");
        }

        private static Rectangle CreateRectangle(int width, int height)
        {
            var rect = new Rectangle
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 7
            };
            return rect;
        }

        private static Line CreateLine(Point pointA, Point pointB)
        {
            var line = new Line
            {
                X1 = pointA.X,
                X2 = pointB.X,
                Y1 = pointA.Y,
                Y2 = pointB.Y,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 3
            };
            return line;
        }

        private void StartCreateRectangles(object sender, RoutedEventArgs e)
        {
            drawingGrid.PreviewMouseLeftButtonDown -= OpenHelpDialog;
            drawingGrid.PreviewMouseLeftButtonDown += DrawingGridOnPreviewMouseLeftButtonDown;
        }

        private void DrawingGridOnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs a)
        {
            //MessageBox.Show($"Canvas{a.GetPosition(drawingGrid).X}/{a.GetPosition(drawingGrid).Y}");
            var heightField = FindName("EnterHeight") as TextBox;
            var widthField = FindName("EnterWidth") as TextBox;
            var width = int.Parse(widthField.Text);
            var height = int.Parse(heightField.Text);
            var newRect = CreateRectangle(width, height);
            drawingGrid.Children.Add(newRect);
            Canvas.SetTop(newRect, a.GetPosition(drawGrid).Y - height / 2);
            Canvas.SetLeft(newRect, a.GetPosition(drawGrid).X - width / 2);
            rectangles.Add(newRect);
            a.Handled = false;
        }

        private void StartCreateLines(object sender, RoutedEventArgs e)
        {
            drawingGrid.PreviewMouseLeftButtonDown -= DrawingGridOnPreviewMouseLeftButtonDown;
            drawingGrid.PreviewMouseLeftButtonDown -= OpenHelpDialog;
            foreach (var rectangle in rectangles)
            {
                rectangle.PreviewMouseLeftButtonDown += (rectSender, rectArgs) =>
                {
                    if (firstLinePoint == null)
                    {
                        firstLinePoint = rectArgs.GetPosition(drawingGrid);
                    }
                    else
                    {
                        var newLine = CreateLine((Point)firstLinePoint, rectArgs.GetPosition(drawingGrid));
                        drawingGrid.Children.Add(newLine);
                        firstLinePoint = null;
                    }
                    rectArgs.Handled = false;
                };
            }
        }
    }
}