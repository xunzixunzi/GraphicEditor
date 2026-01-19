using System.Windows;
using System.Windows.Media;

namespace GraphicsViewFramework;

public class PolygonItem : GraphicsItem
{
    private readonly List<Point> _points = new();
    private Brush _fill = Brushes.Yellow;
    private Brush _stroke = Brushes.Black;
    private double _strokeThickness = 1;

    public IList<Point> Points => _points.AsReadOnly();

    public Brush Fill
    {
        get => _fill;
        set
        {
            if (_fill != value)
            {
                _fill = value;
                OnPropertyChanged();
            }
        }
    }

    public Brush Stroke
    {
        get => _stroke;
        set
        {
            if (_stroke != value)
            {
                _stroke = value;
                OnPropertyChanged();
            }
        }
    }

    public double StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            if (_strokeThickness != value)
            {
                _strokeThickness = value;
                OnPropertyChanged();
            }
        }
    }

    public void AddPoint(Point point)
    {
        _points.Add(point);
        OnPropertyChanged();
    }

    public void AddPoints(params Point[] points)
    {
        foreach (var point in points)
        {
            _points.Add(point);
        }
        OnPropertyChanged();
    }

    public void ClearPoints()
    {
        _points.Clear();
        OnPropertyChanged();
    }

    public override Rect BoundingRect
    {
        get
        {
            if (_points.Count == 0)
                return Rect.Empty;

            var minX = double.MaxValue;
            var minY = double.MaxValue;
            var maxX = double.MinValue;
            var maxY = double.MinValue;

            foreach (var point in _points)
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);
                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            }

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }
    }

    public override bool Contains(Point localPoint)
    {
        if (_points.Count < 3)
            return false;

        return IsPointInPolygon(localPoint, _points);
    }

    public override void Paint(DrawingContext context)
    {
        if (_points.Count < 2)
            return;

        if (_points.Count == 2)
        {
            context.DrawLine(
                new Pen(Stroke, StrokeThickness),
                _points[0],
                _points[1]);
        }
        else
        {
            var segments = new PolyLineSegment();
            for (int i = 1; i < _points.Count; i++)
            {
                segments.Points.Add(_points[i]);
            }

            var figure = new PathFigure(_points[0], new[] { segments }, true);
            var geometry = new PathGeometry(new[] { figure });

            context.DrawGeometry(Fill, new Pen(Stroke, StrokeThickness), geometry);
        }

        if (IsSelected)
        {
            var bounds = BoundingRect;
            context.DrawRectangle(
                null,
                new Pen(Brushes.Blue, 2) { DashStyle = DashStyles.Dash },
                new Rect(bounds.X - 2, bounds.Y - 2, bounds.Width + 4, bounds.Height + 4));
        }
    }

    private static bool IsPointInPolygon(Point point, IList<Point> polygon)
    {
        int crossings = 0;
        int n = polygon.Count;

        for (int i = 0; i < n; i++)
        {
            var p1 = polygon[i];
            var p2 = polygon[(i + 1) % n];

            if (((p1.Y > point.Y) != (p2.Y > point.Y)) &&
                (point.X < (p2.X - p1.X) * (point.Y - p1.Y) / (p2.Y - p1.Y) + p1.X))
            {
                crossings++;
            }
        }

        return (crossings % 2) == 1;
    }
}
