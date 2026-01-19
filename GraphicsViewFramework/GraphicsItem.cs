using System.Windows;
using System.Windows.Media;

namespace GraphicsViewFramework;

public class GraphicsItem : IGraphicsItem
{
    private IGraphicsScene? _scene;
    private IGraphicsItem? _parent;
    private Point _position = new(0, 0);
    private double _rotation;
    private double _scale = 1.0;
    private double _zValue;
    private bool _isVisible = true;
    private bool _isEnabled = true;
    private bool _isSelected;

    public IGraphicsScene? Scene
    {
        get => _scene;
        set => _scene = value;
    }

    public IGraphicsItem? Parent
    {
        get => _parent;
        set => _parent = value;
    }

    public Point Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                OnPropertyChanged();
            }
        }
    }

    public double Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                OnPropertyChanged();
            }
        }
    }

    public double Scale
    {
        get => _scale;
        set
        {
            if (_scale != value)
            {
                _scale = value;
                OnPropertyChanged();
            }
        }
    }

    public double ZValue
    {
        get => _zValue;
        set
        {
            if (_zValue != value)
            {
                _zValue = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled != value)
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public virtual bool Contains(Point localPoint) => BoundingRect.Contains(localPoint);

    public virtual Rect BoundingRect => Rect.Empty;

    public virtual Rect SceneBoundingRect
    {
        get
        {
            var bounds = BoundingRect;
            var transform = GetSceneTransform();
            if (transform == null || bounds.IsEmpty)
                return bounds;

            var topLeft = transform.Transform(bounds.TopLeft);
            var bottomRight = transform.Transform(bounds.BottomRight);
            var topRight = transform.Transform(bounds.TopRight);
            var bottomLeft = transform.Transform(bounds.BottomLeft);

            var minX = Math.Min(topLeft.X, Math.Min(bottomRight.X, Math.Min(topRight.X, bottomLeft.X)));
            var minY = Math.Min(topLeft.Y, Math.Min(bottomRight.Y, Math.Min(topRight.Y, bottomLeft.Y)));
            var maxX = Math.Max(topLeft.X, Math.Max(bottomRight.X, Math.Max(topRight.X, bottomLeft.X)));
            var maxY = Math.Max(topLeft.Y, Math.Max(bottomRight.Y, Math.Max(topRight.Y, bottomLeft.Y)));

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }
    }

    public virtual void Paint(DrawingContext context)
    {
    }

    protected virtual void OnPropertyChanged()
    {
        if (_scene is GraphicsScene graphicsScene)
        {
            graphicsScene.TriggerInvalidated();
        }
    }

    public virtual Transform? GetSceneTransform()
    {
        var group = new TransformGroup();

        var current = this;
        while (current != null)
        {
            var matrix = Matrix.Identity;
            matrix.Translate(current.Position.X, current.Position.Y);
            matrix.RotateAt(current.Rotation, 0, 0);
            matrix.ScaleAt(current.Scale, current.Scale, 0, 0);

            var transform = new MatrixTransform(matrix);
            group.Children.Add(transform);

            if (current.Parent == null || current.Parent is GraphicsItem)
            {
                current = current.Parent as GraphicsItem;
            }
            else
            {
                current = null;
            }
        }

        return group;
    }

    public virtual Point ScenePosition
    {
        get
        {
            if (_parent == null)
                return _position;

            return _parent.MapToScene(_position);
        }
    }

    public virtual Transform SceneTransform
    {
        get
        {
            var transform = GetSceneTransform();
            return transform ?? new MatrixTransform(Matrix.Identity);
        }
    }

    public virtual Point MapToScene(Point point)
    {
        var transform = GetSceneTransform();
        if (transform == null)
            return point;

        return transform.Transform(point);
    }

    public virtual Point MapFromScene(Point point)
    {
        var transform = GetSceneTransform();
        if (transform == null)
            return point;

        var inverse = transform.Inverse;
        if (inverse == null)
            return point;

        return inverse.Transform(point);
    }

    public virtual Rect MapToScene(Rect rect)
    {
        var transform = GetSceneTransform();
        if (transform == null)
            return rect;

        var topLeft = transform.Transform(rect.TopLeft);
        var bottomRight = transform.Transform(rect.BottomRight);
        var topRight = transform.Transform(rect.TopRight);
        var bottomLeft = transform.Transform(rect.BottomLeft);

        var minX = Math.Min(topLeft.X, Math.Min(bottomRight.X, Math.Min(topRight.X, bottomLeft.X)));
        var minY = Math.Min(topLeft.Y, Math.Min(bottomRight.Y, Math.Min(topRight.Y, bottomLeft.Y)));
        var maxX = Math.Max(topLeft.X, Math.Max(bottomRight.X, Math.Max(topRight.X, bottomLeft.X)));
        var maxY = Math.Max(topLeft.Y, Math.Max(bottomRight.Y, Math.Max(topRight.Y, bottomLeft.Y)));

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    public virtual Rect MapFromScene(Rect rect)
    {
        var transform = GetSceneTransform();
        if (transform == null)
            return rect;

        var inverse = transform.Inverse;
        if (inverse == null)
            return rect;

        var topLeft = inverse.Transform(rect.TopLeft);
        var bottomRight = inverse.Transform(rect.BottomRight);
        var topRight = inverse.Transform(rect.TopRight);
        var bottomLeft = inverse.Transform(rect.BottomLeft);

        var minX = Math.Min(topLeft.X, Math.Min(bottomRight.X, Math.Min(topRight.X, bottomLeft.X)));
        var minY = Math.Min(topLeft.Y, Math.Min(bottomRight.Y, Math.Min(topRight.Y, bottomLeft.Y)));
        var maxX = Math.Max(topLeft.X, Math.Max(bottomRight.X, Math.Max(topRight.X, bottomLeft.X)));
        var maxY = Math.Max(topLeft.Y, Math.Max(bottomRight.Y, Math.Max(topRight.Y, bottomLeft.Y)));

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    public virtual Point MapToParent(Point point)
    {
        var matrix = Matrix.Identity;
        matrix.Translate(_position.X, _position.Y);
        matrix.RotateAt(_rotation, 0, 0);
        matrix.ScaleAt(_scale, _scale, 0, 0);

        var transform = new MatrixTransform(matrix);
        return transform.Transform(point);
    }

    public virtual Point MapFromParent(Point point)
    {
        var matrix = Matrix.Identity;
        matrix.Translate(_position.X, _position.Y);
        matrix.RotateAt(_rotation, 0, 0);
        matrix.ScaleAt(_scale, _scale, 0, 0);

        var transform = new MatrixTransform(matrix);
        var inverse = transform.Inverse;
        if (inverse == null)
            return point;

        return inverse.Transform(point);
    }

    public virtual Rect MapToParent(Rect rect)
    {
        var matrix = Matrix.Identity;
        matrix.Translate(_position.X, _position.Y);
        matrix.RotateAt(_rotation, 0, 0);
        matrix.ScaleAt(_scale, _scale, 0, 0);

        var transform = new MatrixTransform(matrix);
        var topLeft = transform.Transform(rect.TopLeft);
        var bottomRight = transform.Transform(rect.BottomRight);
        var topRight = transform.Transform(rect.TopRight);
        var bottomLeft = transform.Transform(rect.BottomLeft);

        var minX = Math.Min(topLeft.X, Math.Min(bottomRight.X, Math.Min(topRight.X, bottomLeft.X)));
        var minY = Math.Min(topLeft.Y, Math.Min(bottomRight.Y, Math.Min(topRight.Y, bottomLeft.Y)));
        var maxX = Math.Max(topLeft.X, Math.Max(bottomRight.X, Math.Max(topRight.X, bottomLeft.X)));
        var maxY = Math.Max(topLeft.Y, Math.Max(bottomRight.Y, Math.Max(topRight.Y, bottomLeft.Y)));

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    public virtual Rect MapFromParent(Rect rect)
    {
        var matrix = Matrix.Identity;
        matrix.Translate(_position.X, _position.Y);
        matrix.RotateAt(_rotation, 0, 0);
        matrix.ScaleAt(_scale, _scale, 0, 0);

        var transform = new MatrixTransform(matrix);
        var inverse = transform.Inverse;
        if (inverse == null)
            return rect;

        var topLeft = inverse.Transform(rect.TopLeft);
        var bottomRight = inverse.Transform(rect.BottomRight);
        var topRight = inverse.Transform(rect.TopRight);
        var bottomLeft = inverse.Transform(rect.BottomLeft);

        var minX = Math.Min(topLeft.X, Math.Min(bottomRight.X, Math.Min(topRight.X, bottomLeft.X)));
        var minY = Math.Min(topLeft.Y, Math.Min(bottomRight.Y, Math.Min(topRight.Y, bottomLeft.Y)));
        var maxX = Math.Max(topLeft.X, Math.Max(bottomRight.X, Math.Max(topRight.X, bottomLeft.X)));
        var maxY = Math.Max(topLeft.Y, Math.Max(bottomRight.Y, Math.Max(topRight.Y, bottomLeft.Y)));

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    public virtual Point MapToItem(IGraphicsItem? item, Point point)
    {
        var scenePoint = MapToScene(point);
        if (item == null)
            return scenePoint;

        return item.MapFromScene(scenePoint);
    }

    public virtual Point MapFromItem(IGraphicsItem? item, Point point)
    {
        if (item == null)
            return MapFromScene(point);

        var scenePoint = item.MapToScene(point);
        return MapFromScene(scenePoint);
    }

    public virtual Rect MapToItem(IGraphicsItem? item, Rect rect)
    {
        var sceneRect = MapToScene(rect);
        if (item == null)
            return sceneRect;

        return item.MapFromScene(sceneRect);
    }

    public virtual Rect MapFromItem(IGraphicsItem? item, Rect rect)
    {
        if (item == null)
            return MapFromScene(rect);

        var sceneRect = item.MapToScene(rect);
        return MapFromScene(sceneRect);
    }
}
