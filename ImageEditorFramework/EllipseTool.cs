using GraphicsViewFramework;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageEditorFramework;

public class EllipseTool : IDrawingTool
{
    private Point? _startPoint;
    private EllipseItem? _previewItem;
    private bool _isDrawing;

    public string Name => "Ellipse";

    public void OnMouseDown(Point scenePoint, ImageScene scene, ImageView view)
    {
        if (!_isDrawing)
        {
            _startPoint = scenePoint;
            _previewItem = new EllipseItem
            {
                Position = scenePoint,
                Width = 0,
                Height = 0,
                Stroke = Brushes.Blue,
                StrokeThickness = 2,
                Fill = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255)),
                ZValue = double.MaxValue
            };
            scene.AddItem(_previewItem);
            _isDrawing = true;
        }
        else if (_startPoint.HasValue)
        {
            var x = Math.Min(_startPoint.Value.X, scenePoint.X);
            var y = Math.Min(_startPoint.Value.Y, scenePoint.Y);
            var width = Math.Abs(scenePoint.X - _startPoint.Value.X);
            var height = Math.Abs(scenePoint.Y - _startPoint.Value.Y);

            if (width >= 2 && height >= 2)
            {
                var ellipseItem = new EllipseItem
                {
                    Position = new Point(x, y),
                    Width = width,
                    Height = height,
                    Stroke = Brushes.Blue,
                    StrokeThickness = 2,
                    Fill = Brushes.Transparent,
                    ZValue = 100
                };

                scene.AddItem(ellipseItem);
            }

            Reset(scene);
        }
    }

    public void OnMouseMove(Point scenePoint, ImageScene scene, ImageView view)
    {
        if (!_isDrawing || _startPoint == null || _previewItem == null)
            return;

        var x = Math.Min(_startPoint.Value.X, scenePoint.X);
        var y = Math.Min(_startPoint.Value.Y, scenePoint.Y);
        var width = Math.Abs(scenePoint.X - _startPoint.Value.X);
        var height = Math.Abs(scenePoint.Y - _startPoint.Value.Y);

        _previewItem.Position = new Point(x, y);
        _previewItem.Width = width;
        _previewItem.Height = height;
    }

    public void OnMouseUp(Point scenePoint, ImageScene scene, ImageView view)
    {
    }

    private void Reset(ImageScene scene)
    {
        if (_previewItem != null)
        {
            scene.RemoveItem(_previewItem);
            _previewItem = null;
        }
        _startPoint = null;
        _isDrawing = false;
    }
}
