using ImageEditorFramework;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageEditorDemo;

public partial class MainWindow : Window
{
    private readonly ImageScene _scene;

    public MainWindow()
    {
        InitializeComponent();
        _scene = new ImageScene();
        ImageView.Scene = _scene;

        UpdateInfoText();
    }

    private void LoadImage_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*",
            Title = "Select an Image"
        };

        if (dialog.ShowDialog() == true)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(dialog.FileName);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            _scene.Image = bitmap;
            ImageView.FitToImage();
        }
    }

    private void ZoomIn_Click(object sender, RoutedEventArgs e)
    {
        ImageView.Scale(1.2, 1.2);
        UpdateInfoText();
    }

    private void ZoomOut_Click(object sender, RoutedEventArgs e)
    {
        ImageView.Scale(1.0 / 1.2, 1.0 / 1.2);
        UpdateInfoText();
    }

    private void FitToWindow_Click(object sender, RoutedEventArgs e)
    {
        ImageView.FitToImage();
        UpdateInfoText();
    }

    private void ActualSize_Click(object sender, RoutedEventArgs e)
    {
        ImageView.ResetTransform();
        UpdateInfoText();
    }

    private void ApplyZoomSpeed_Click(object sender, RoutedEventArgs e)
    {
        if (double.TryParse(ZoomSpeedTextBox.Text, out double speed) && speed > 0)
        {
            ImageView.ZoomSpeed = speed;
        }
    }

    private void UpdateInfoText()
    {
        var transform = ImageView.ViewTransform;
        var toolName = ImageView.CurrentTool?.Name ?? "View";
        var hint = toolName == "Polygon" ? " (Ctrl+Click to finish)" : " (Click to add points)";
        InfoTextBlock.Text = $"Mode: {toolName}{hint} | Zoom: {transform.Value.M11:F2}x";
    }

    private void SetViewTool_Click(object sender, RoutedEventArgs e)
    {
        ImageView.CurrentTool = null;
        UpdateInfoText();
    }

    private void SetRectTool_Click(object sender, RoutedEventArgs e)
    {
        ImageView.CurrentTool = new RectangleTool();
        UpdateInfoText();
    }

    private void SetEllipseTool_Click(object sender, RoutedEventArgs e)
    {
        ImageView.CurrentTool = new EllipseTool();
        UpdateInfoText();
    }

    private void SetLineTool_Click(object sender, RoutedEventArgs e)
    {
        ImageView.CurrentTool = new LineTool();
        UpdateInfoText();
    }

    private void SetTextTool_Click(object sender, RoutedEventArgs e)
    {
        ImageView.CurrentTool = new TextTool { Text = "Text" };
        UpdateInfoText();
    }

    private void SetPolygonTool_Click(object sender, RoutedEventArgs e)
    {
        ImageView.CurrentTool = new PolygonTool();
        UpdateInfoText();
    }
}
