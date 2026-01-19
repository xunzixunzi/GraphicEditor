using System.Windows;
using System.Windows.Media;

namespace GraphicsViewFramework;

public class TextItem : GraphicsItem
{
    private string _text = "Text";
    private FontFamily _fontFamily = new("Segoe UI");
    private double _fontSize = 24;
    private FontStyle _fontStyle = FontStyles.Normal;
    private FontWeight _fontWeight = FontWeights.Normal;
    private Brush _foreground = Brushes.Black;
    private FormattedText? _formattedText;

    public string Text
    {
        get => _text;
        set
        {
            if (_text != value)
            {
                _text = value;
                UpdateFormattedText();
                OnPropertyChanged();
            }
        }
    }

    public FontFamily FontFamily
    {
        get => _fontFamily;
        set
        {
            if (_fontFamily != value)
            {
                _fontFamily = value;
                UpdateFormattedText();
                OnPropertyChanged();
            }
        }
    }

    public double FontSize
    {
        get => _fontSize;
        set
        {
            if (_fontSize != value)
            {
                _fontSize = value;
                UpdateFormattedText();
                OnPropertyChanged();
            }
        }
    }

    public FontStyle FontStyle
    {
        get => _fontStyle;
        set
        {
            if (_fontStyle != value)
            {
                _fontStyle = value;
                UpdateFormattedText();
                OnPropertyChanged();
            }
        }
    }

    public FontWeight FontWeight
    {
        get => _fontWeight;
        set
        {
            if (_fontWeight != value)
            {
                _fontWeight = value;
                UpdateFormattedText();
                OnPropertyChanged();
            }
        }
    }

    public Brush Foreground
    {
        get => _foreground;
        set
        {
            if (_foreground != value)
            {
                _foreground = value;
                UpdateFormattedText();
                OnPropertyChanged();
            }
        }
    }

    private void UpdateFormattedText()
    {
        if (string.IsNullOrEmpty(_text))
        {
            _formattedText = null;
            return;
        }

        var drawingVisual = new DrawingVisual();
        var dpiScale = VisualTreeHelper.GetDpi(drawingVisual);
        var pixelsPerDip = dpiScale.DpiScaleX;

        _formattedText = new FormattedText(
            _text,
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new Typeface(_fontFamily, _fontStyle, _fontWeight, FontStretches.Normal),
            _fontSize,
            _foreground,
            pixelsPerDip);
    }

    public override Rect BoundingRect
    {
        get
        {
            if (_formattedText == null)
                return Rect.Empty;

            return new Rect(0, 0, _formattedText.Width, _formattedText.Height);
        }
    }

    public override bool Contains(Point localPoint) => BoundingRect.Contains(localPoint);

    public override void Paint(DrawingContext context)
    {
        if (_formattedText == null)
            return;

        context.DrawText(_formattedText, new Point(0, 0));

        if (IsSelected)
        {
            var bounds = BoundingRect;
            context.DrawRectangle(
                null,
                new Pen(Brushes.Blue, 2) { DashStyle = DashStyles.Dash },
                new Rect(bounds.X - 2, bounds.Y - 2, bounds.Width + 4, bounds.Height + 4));
        }
    }
}
