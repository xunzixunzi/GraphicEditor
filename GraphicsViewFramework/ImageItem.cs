using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GraphicsViewFramework;

public class ImageItem : GraphicsItem
{
    private ImageSource? _source;
    private double _width;
    private double _height;
    private Stretch _stretch = Stretch.Uniform;

    public ImageSource? Source
    {
        get => _source;
        set
        {
            if (_source != value)
            {
                _source = value;
                UpdateSize();
                OnPropertyChanged();
            }
        }
    }

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

    public Stretch Stretch
    {
        get => _stretch;
        set
        {
            if (_stretch != value)
            {
                _stretch = value;
                OnPropertyChanged();
            }
        }
    }

    public ImageItem()
    {
    }

    public ImageItem(ImageSource source)
    {
        _source = source;
        UpdateSize();
    }

    public ImageItem(string uri)
    {
        try
        {
            _source = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
            UpdateSize();
        }
        catch
        {
        }
    }

    private void UpdateSize()
    {
        if (_source is BitmapImage bitmap)
        {
            _width = bitmap.PixelWidth;
            _height = bitmap.PixelHeight;
        }
        else if (_source != null)
        {
            _width = _source.Width;
            _height = _source.Height;
        }
    }

    public override Rect BoundingRect => new Rect(0, 0, Width, Height);

    public override bool Contains(Point localPoint) => BoundingRect.Contains(localPoint);

    public override void Paint(DrawingContext context)
    {
        if (_source == null)
            return;

        var destRect = new Rect(0, 0, Width, Height);
        context.DrawImage(_source, destRect);

        if (IsSelected)
        {
            context.DrawRectangle(
                null,
                new Pen(Brushes.Blue, 2) { DashStyle = DashStyles.Dash },
                new Rect(-2, -2, Width + 4, Height + 4));
        }
    }
}
