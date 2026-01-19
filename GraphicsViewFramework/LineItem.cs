using System.Windows;
using System.Windows.Media;

namespace GraphicsViewFramework;

public class LineItem : GraphicsItem
{
    private Point _startPoint;
    private Point _endPoint = new Point(100, 100);
    private Brush _stroke = Brushes.Black;
    private double _strokeThickness = 2;

    public Point StartPoint
    {
        get => _startPoint;
        set
        {
            if (_startPoint != value)
            {
                _startPoint = value;
                OnPropertyChanged();
            }
        }
    }

    public Point EndPoint
    {
        get => _endPoint;
        set
        {
            if (_endPoint != value)
            {
                _endPoint = value;
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

    public override Rect BoundingRect
    {
        get
        {
            var x = Math.Min(StartPoint.X, EndPoint.X);
            var y = Math.Min(StartPoint.Y, EndPoint.Y);
            var width = Math.Abs(EndPoint.X - StartPoint.X);
            var height = Math.Abs(EndPoint.Y - StartPoint.Y);
            return new Rect(x, y, width, height);
        }
    }

    public override bool Contains(Point localPoint)
    {
        var distanceToLine = DistanceFromPointToLine(localPoint, StartPoint, EndPoint);
        return distanceToLine <= StrokeThickness + 2;
    }

    public override void Paint(DrawingContext context)
    {
        context.DrawLine(new Pen(Stroke, StrokeThickness), StartPoint, EndPoint);

        if (IsSelected)
        {
            var bounds = BoundingRect;
            context.DrawRectangle(
                null,
                new Pen(Brushes.Blue, 2) { DashStyle = DashStyles.Dash },
                new Rect(bounds.X - 2, bounds.Y - 2, bounds.Width + 4, bounds.Height + 4));
        }
    }

    private static double DistanceFromPointToLine(Point point, Point lineStart, Point lineEnd)
    {
        var dx = lineEnd.X - lineStart.X;
        var dy = lineEnd.Y - lineStart.Y;

        if (dx == 0 && dy == 0)
        {
            return Math.Sqrt(Math.Pow(point.X - lineStart.X, 2) + Math.Pow(point.Y - lineStart.Y, 2));
        }

        var t = ((point.X - lineStart.X) * dx + (point.Y - lineStart.Y) * dy) / (dx * dx + dy * dy);
        t = Math.Max(0, Math.Min(1, t));

        var closestX = lineStart.X + t * dx;
        var closestY = lineStart.Y + t * dy;

        return Math.Sqrt(Math.Pow(point.X - closestX, 2) + Math.Pow(point.Y - closestY, 2));
    }
}
