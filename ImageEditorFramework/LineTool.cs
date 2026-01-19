using GraphicsViewFramework;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageEditorFramework;

public class LineTool : IDrawingTool
{
    private Point? _startPoint;
    private LineItem? _previewItem;
    private bool _isDrawing;

    public string Name => "Line";

    public void OnMouseDown(Point scenePoint, ImageScene scene, ImageView view)
    {
        if (!_isDrawing)
        {
            _startPoint = scenePoint;
            _previewItem = new LineItem
            {
                Position = _startPoint.Value,
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 0),
                Stroke = Brushes.Green,
                StrokeThickness = 2,
                ZValue = double.MaxValue
            };
            scene.AddItem(_previewItem);
            _isDrawing = true;
        }
        else if (_startPoint.HasValue)
        {
            var dx = scenePoint.X - _startPoint.Value.X;
            var dy = scenePoint.Y - _startPoint.Value.Y;
            var length = Math.Sqrt(dx * dx + dy * dy);

            if (length >= 2)
            {
                var lineItem = new LineItem
                {
                    Position = _startPoint.Value,
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(dx, dy),
                    Stroke = Brushes.Green,
                    StrokeThickness = 2,
                    ZValue = 100
                };

                scene.AddItem(lineItem);
            }

            Reset(scene);
        }
    }

    public void OnMouseMove(Point scenePoint, ImageScene scene, ImageView view)
    {
        if (!_isDrawing || _startPoint == null || _previewItem == null)
            return;

        var dx = scenePoint.X - _startPoint.Value.X;
        var dy = scenePoint.Y - _startPoint.Value.Y;
        _previewItem.EndPoint = new Point(dx, dy);
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
