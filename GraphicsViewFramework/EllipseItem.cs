using System.Windows;
using System.Windows.Media;

namespace GraphicsViewFramework;

public class EllipseItem : GraphicsItem
{
    private double _width = 100;
    private double _height = 100;
    private Brush _fill = Brushes.LightGreen;
    private Brush _stroke = Brushes.Black;
    private double _strokeThickness = 1;

    public double Width
    {
        get => _width;
        set
        {
            if (_width != value)
            {
                _width = value;
                OnPropertyChanged();
            }
        }
    }

    public double Height
    {
        get => _height;
        set
        {
            if (_height != value)
            {
                _height = value;
                OnPropertyChanged();
            }
        }
    }

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

    public override Rect BoundingRect => new Rect(0, 0, Width, Height);

    public override bool Contains(Point localPoint)
    {
        var centerX = Width / 2;
        var centerY = Height / 2;
        var radiusX = Width / 2;
        var radiusY = Height / 2;

        var normalizedX = (localPoint.X - centerX) / radiusX;
        var normalizedY = (localPoint.Y - centerY) / radiusY;

        return (normalizedX * normalizedX + normalizedY * normalizedY) <= 1.0;
    }

    public override void Paint(DrawingContext context)
    {
        var rect = new Rect(0, 0, Width, Height);
        var centerPoint = new Point(Width / 2, Height / 2);
        var radiusX = Width / 2;
        var radiusY = Height / 2;
        context.DrawEllipse(Fill, new Pen(Stroke, StrokeThickness), centerPoint, radiusX, radiusY);

        if (IsSelected)
        {
            context.DrawRectangle(
                null,
                new Pen(Brushes.Blue, 2) { DashStyle = DashStyles.Dash },
                new Rect(-2, -2, Width + 4, Height + 4));
        }
    }
}
