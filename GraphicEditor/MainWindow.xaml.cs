using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GraphicsViewFramework;

namespace GraphicEditor;

public partial class MainWindow : Window
{
    private readonly GraphicsScene _scene;

    public MainWindow()
    {
        InitializeComponent();
        _scene = new GraphicsScene();
        graphicsView.Scene = _scene;

        InitializeDemo();
    }

    private void InitializeDemo()
    {
        var rect1 = new RectangleItem
        {
            Position = new Point(100, 100),
            Width = 150,
            Height = 100,
            Fill = System.Windows.Media.Brushes.LightBlue,
            Stroke = System.Windows.Media.Brushes.DarkBlue,
            StrokeThickness = 2
        };
        _scene.AddItem(rect1);

        var ellipse1 = new EllipseItem
        {
            Position = new Point(300, 150),
            Width = 120,
            Height = 120,
            Fill = System.Windows.Media.Brushes.LightGreen,
            Stroke = System.Windows.Media.Brushes.DarkGreen,
            StrokeThickness = 2
        };
        _scene.AddItem(ellipse1);

        var line1 = new LineItem
        {
            Position = new Point(450, 100),
            StartPoint = new Point(0, 0),
            EndPoint = new Point(100, 150),
            Stroke = System.Windows.Media.Brushes.Red,
            StrokeThickness = 3
        };
        _scene.AddItem(line1);

        var rect2 = new RectangleItem
        {
            Position = new Point(200, 300),
            Width = 100,
            Height = 80,
            Fill = System.Windows.Media.Brushes.Orange,
            Stroke = System.Windows.Media.Brushes.DarkOrange,
            StrokeThickness = 2,
            Rotation = 45,
            ZValue = 1
        };
        _scene.AddItem(rect2);

        var triangle = new PolygonItem
        {
            Position = new Point(550, 300),
            Fill = System.Windows.Media.Brushes.Pink,
            Stroke = System.Windows.Media.Brushes.Purple,
            StrokeThickness = 2,
            ZValue = 2
        };
        triangle.AddPoints(
            new Point(50, 0),
            new Point(100, 86),
            new Point(0, 86)
        );
        _scene.AddItem(triangle);

        var text1 = new TextItem
        {
            Position = new Point(50, 450),
            Text = "WPF Graphics View Framework",
            FontSize = 32,
            FontWeight = FontWeights.Bold,
            Foreground = System.Windows.Media.Brushes.DarkBlue,
            ZValue = 3
        };
        _scene.AddItem(text1);
    }

    private void AddRectangle_Click(object sender, RoutedEventArgs e)
    {
        var random = new Random();
        var rect = new RectangleItem
        {
            Position = new Point(
                random.Next(50, 600),
                random.Next(50, 400)),
            Width = random.Next(50, 150),
            Height = random.Next(50, 150),
            Fill = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255))),
            Stroke = System.Windows.Media.Brushes.Black,
            StrokeThickness = 2,
            ZValue = _scene.Items.Count
        };
        _scene.AddItem(rect);
    }

    private void AddEllipse_Click(object sender, RoutedEventArgs e)
    {
        var random = new Random();
        var ellipse = new EllipseItem
        {
            Position = new Point(
                random.Next(50, 600),
                random.Next(50, 400)),
            Width = random.Next(50, 150),
            Height = random.Next(50, 150),
            Fill = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255))),
            Stroke = System.Windows.Media.Brushes.Black,
            StrokeThickness = 2,
            ZValue = _scene.Items.Count
        };
        _scene.AddItem(ellipse);
    }

    private void AddLine_Click(object sender, RoutedEventArgs e)
    {
        var random = new Random();
        var line = new LineItem
        {
            Position = new Point(
                random.Next(50, 600),
                random.Next(50, 400)),
            EndPoint = new Point(
                random.Next(0, 150),
                random.Next(0, 150)),
            Stroke = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(0, 255),
                    (byte)random.Next(0, 255),
                    (byte)random.Next(0, 255))),
            StrokeThickness = random.Next(2, 5),
            ZValue = _scene.Items.Count
        };
        _scene.AddItem(line);
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        _scene.Clear();
    }

    private void AddTriangle_Click(object sender, RoutedEventArgs e)
    {
        var random = new Random();
        var polygon = new PolygonItem
        {
            Position = new Point(
                random.Next(50, 600),
                random.Next(50, 400)),
            Fill = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255))),
            Stroke = System.Windows.Media.Brushes.Black,
            StrokeThickness = 2,
            ZValue = _scene.Items.Count
        };

        var baseSize = random.Next(60, 120);
        var height = baseSize * Math.Sqrt(3) / 2;
        polygon.AddPoints(
            new Point(baseSize / 2, 0),
            new Point(baseSize, height),
            new Point(0, height)
        );
        _scene.AddItem(polygon);
    }

    private void AddPentagon_Click(object sender, RoutedEventArgs e)
    {
        var random = new Random();
        var polygon = new PolygonItem
        {
            Position = new Point(
                random.Next(50, 600),
                random.Next(50, 400)),
            Fill = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255))),
            Stroke = System.Windows.Media.Brushes.Black,
            StrokeThickness = 2,
            ZValue = _scene.Items.Count
        };

        var radius = random.Next(40, 80);
        for (int i = 0; i < 5; i++)
        {
            var angle = i * 2 * Math.PI / 5 - Math.PI / 2;
            var x = radius * Math.Cos(angle) + radius;
            var y = radius * Math.Sin(angle) + radius;
            polygon.AddPoint(new Point(x, y));
        }
        _scene.AddItem(polygon);
    }

    private void AddStar_Click(object sender, RoutedEventArgs e)
    {
        var random = new Random();
        var polygon = new PolygonItem
        {
            Position = new Point(
                random.Next(50, 600),
                random.Next(50, 400)),
            Fill = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(200, 255),
                    (byte)random.Next(200, 255),
                    (byte)random.Next(100, 255))),
            Stroke = System.Windows.Media.Brushes.DarkOrange,
            StrokeThickness = 2,
            ZValue = _scene.Items.Count
        };

        var outerRadius = random.Next(50, 80);
        var innerRadius = outerRadius * 0.4;
        int points = 5;

        for (int i = 0; i < points * 2; i++)
        {
            var angle = i * Math.PI / points - Math.PI / 2;
            var radius = i % 2 == 0 ? outerRadius : innerRadius;
            var x = radius * Math.Cos(angle) + outerRadius;
            var y = radius * Math.Sin(angle) + outerRadius;
            polygon.AddPoint(new Point(x, y));
        }
        _scene.AddItem(polygon);
    }

    private void AddText_Click(object sender, RoutedEventArgs e)
    {
        var random = new Random();
        var textItem = new TextItem
        {
            Position = new Point(
                random.Next(50, 600),
                random.Next(50, 400)),
            Text = "Hello, WPF!",
            FontSize = random.Next(16, 48),
            FontWeight = random.NextDouble() > 0.5 ? FontWeights.Bold : FontWeights.Normal,
            Foreground = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(0, 200),
                    (byte)random.Next(0, 200),
                    (byte)random.Next(0, 200))),
            ZValue = _scene.Items.Count
        };
        _scene.AddItem(textItem);
    }

    private void AddImage_Click(object sender, RoutedEventArgs e)
    {
        var random = new Random();
        
        var width = random.Next(80, 150);
        var height = random.Next(80, 150);
        
        var drawingVisual = new System.Windows.Media.DrawingVisual();
        using (var context = drawingVisual.RenderOpen())
        {
            var rect = new Rect(0, 0, width, height);
            
            var gradientBrush = new System.Windows.Media.LinearGradientBrush(
                System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255)),
                System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255),
                    (byte)random.Next(100, 255)),
                45);
            
            context.DrawRectangle(gradientBrush, null, rect);

            var brush = new System.Windows.Media.SolidColorBrush(
                System.Windows.Media.Color.FromRgb(255, 255, 255));
            var typeface = new System.Windows.Media.Typeface(
                new FontFamily("Segoe UI"),
                FontStyles.Normal,
                FontWeights.Bold,
                FontStretches.Normal);

            var drawingVisualForDpi = new System.Windows.Media.DrawingVisual();
            var dpiScale = VisualTreeHelper.GetDpi(drawingVisualForDpi);

            var formattedText = new FormattedText(
                "IMG",
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                width / 2,
                brush,
                new NumberSubstitution(),
                dpiScale.DpiScaleX);

            context.DrawText(formattedText,
                new Point((width - formattedText.Width) / 2,
                         (height - formattedText.Height) / 2));
        }

        var bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
        bitmap.Render(drawingVisual);

        var imageItem = new ImageItem
        {
            Position = new Point(
                random.Next(50, 600),
                random.Next(50, 400)),
            Source = bitmap,
            ZValue = _scene.Items.Count
        };
        _scene.AddItem(imageItem);
    }
}
