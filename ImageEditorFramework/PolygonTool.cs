using GraphicsViewFramework;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageEditorFramework;

public class PolygonTool : IDrawingTool
{
    private PolygonItem? _currentPolygon;
    private LineItem? _previewLine;
    private Point? _firstPoint;
    private Point? _lastPoint;
    private bool _isDrawing;

    public string Name => "Polygon";

    public void OnMouseDown(Point scenePoint, ImageScene scene, ImageView view)
    {
        if (Keyboard.Modifiers == ModifierKeys.Control)
        {
            if (_isDrawing && _currentPolygon != null && _firstPoint.HasValue)
            {
                var dx = scenePoint.X - _firstPoint.Value.X;
                var dy = scenePoint.Y - _firstPoint.Value.Y;
                _currentPolygon.AddPoint(new Point(dx, dy));
            }
            Finish(scene);
            return;
        }

        if (!_isDrawing)
        {
            _firstPoint = scenePoint;
            _lastPoint = scenePoint;

            _currentPolygon = new PolygonItem
            {
                Position = scenePoint,
                Stroke = Brushes.Purple,
                StrokeThickness = 2,
                Fill = Brushes.Transparent,
                ZValue = 100,
                IsClosed = false
            };
            _currentPolygon.AddPoint(new Point(0, 0));
            scene.AddItem(_currentPolygon);

            _previewLine = new LineItem
            {
                Position = scenePoint,
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 0),
                Stroke = new SolidColorBrush(Color.FromArgb(128, 128, 0, 128)),
                StrokeThickness = 2,
                ZValue = double.MaxValue
            };
            scene.AddItem(_previewLine);

            _isDrawing = true;
        }
        else if (_currentPolygon != null && _firstPoint.HasValue && _previewLine != null)
        {
            var dx = scenePoint.X - _firstPoint.Value.X;
            var dy = scenePoint.Y - _firstPoint.Value.Y;
            _currentPolygon.AddPoint(new Point(dx, dy));

            _previewLine.Position = scenePoint;
            _previewLine.StartPoint = new Point(0, 0);
            _previewLine.EndPoint = new Point(0, 0);
            _lastPoint = scenePoint;
        }
    }

    public void OnMouseMove(Point scenePoint, ImageScene scene, ImageView view)
    {
        if (_previewLine != null && _lastPoint.HasValue)
        {
            var dx = scenePoint.X - _lastPoint.Value.X;
            var dy = scenePoint.Y - _lastPoint.Value.Y;
            _previewLine.EndPoint = new Point(dx, dy);
        }
    }

    public void OnMouseUp(Point scenePoint, ImageScene scene, ImageView view)
    {
    }


    public void Finish(ImageScene scene)
    {
        if (_previewLine != null)
        {
            scene.RemoveItem(_previewLine);
            _previewLine = null;
        }
        if (_currentPolygon != null)
        {
            _currentPolygon.IsClosed = true;
        }
        _currentPolygon = null;
        _firstPoint = null;
        _lastPoint = null;
        _isDrawing = false;
    }
}
