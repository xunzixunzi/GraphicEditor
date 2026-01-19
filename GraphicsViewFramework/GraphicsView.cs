using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphicsViewFramework;

public class GraphicsView : Control, IGraphicsView
{
    private IGraphicsScene? _scene;
    private readonly TransformGroup _viewTransformGroup;
    private TranslateTransform _scrollTranslate = new();
    private ScaleTransform _scaleTransform = new();
    private RotateTransform _rotateTransform = new();

    protected Point _lastMousePosition;
    protected bool _isPanning;

    public static readonly DependencyProperty SceneProperty =
        DependencyProperty.Register(
            nameof(Scene),
            typeof(IGraphicsScene),
            typeof(GraphicsView),
            new PropertyMetadata(null, OnSceneChanged));

    public static readonly DependencyProperty ZoomSpeedProperty =
        DependencyProperty.Register(
            nameof(ZoomSpeed),
            typeof(double),
            typeof(GraphicsView),
            new PropertyMetadata(0.001));

    public IGraphicsScene? Scene
    {
        get => (IGraphicsScene?)GetValue(SceneProperty);
        set => SetValue(SceneProperty, value);
    }

    public double ZoomSpeed
    {
        get => (double)GetValue(ZoomSpeedProperty);
        set => SetValue(ZoomSpeedProperty, value);
    }

    public Transform ViewTransform => _viewTransformGroup;

    public ViewportAnchor TransformationAnchor { get; set; }

    public ViewportAnchor ResizeAnchor { get; set; }

    public DragMode DragMode { get; set; }

    static GraphicsView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(GraphicsView),
            new FrameworkPropertyMetadata(typeof(GraphicsView)));
    }

    public GraphicsView()
    {
        ClipToBounds = true;
        _viewTransformGroup = new TransformGroup();
        _viewTransformGroup.Children.Add(_scaleTransform);
        _viewTransformGroup.Children.Add(_rotateTransform);
        _viewTransformGroup.Children.Add(_scrollTranslate);

        PreviewMouseDown += OnPreviewMouseDown;
        PreviewMouseUp += OnPreviewMouseUp;
        PreviewMouseMove += OnPreviewMouseMove;
        MouseWheel += OnMouseWheel;
        LostMouseCapture += OnLostMouseCapture;
    }

    protected virtual void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Middle)
        {
            _lastMousePosition = e.GetPosition(this);
            _isPanning = true;
            CaptureMouse();
            Cursor = Cursors.Hand;
            e.Handled = true;
        }
    }

    protected virtual void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Middle)
        {
            _isPanning = false;
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }
            Cursor = null;
            e.Handled = true;
        }
    }

    protected virtual void OnPreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_isPanning && IsMouseCaptured)
        {
            var currentPos = e.GetPosition(this);
            var dx = currentPos.X - _lastMousePosition.X;
            var dy = currentPos.Y - _lastMousePosition.Y;

            Translate(dx, dy);

            _lastMousePosition = currentPos;
        }
    }

    protected virtual void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        var scaleFactor = 1.0 + (e.Delta * ZoomSpeed);

        if (scaleFactor > 0)
        {
            var mousePoint = e.GetPosition(this);
            var scenePoint = MapToScene(mousePoint);

            Scale(scaleFactor, scaleFactor);

            var adjustedMousePoint = MapFromScene(scenePoint);
            var dx = mousePoint.X - adjustedMousePoint.X;
            var dy = mousePoint.Y - adjustedMousePoint.Y;

            Translate(dx, dy);
        }

        e.Handled = true;
    }

    protected virtual void OnLostMouseCapture(object sender, MouseEventArgs e)
    {
        _isPanning = false;
        Cursor = null;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (Background != null)
        {
            drawingContext.DrawRectangle(
                Background,
                null,
                new Rect(RenderSize));
        }

        drawingContext.PushTransform(_viewTransformGroup);
        DrawScene(drawingContext);
        drawingContext.Pop();
    }

    private void OnSceneInvalidated(object? sender, EventArgs e)
    {
        InvalidateVisual();
    }

    private void DrawScene(DrawingContext drawingContext)
    {
        if (_scene == null)
            return;

        foreach (var item in _scene.Items.OrderBy(i => i.ZValue))
        {
            if (item.IsVisible)
            {
                drawingContext.PushTransform(GetItemTransform(item));
                item.Paint(drawingContext);
                drawingContext.Pop();
            }
        }
    }

    private Transform GetItemTransform(IGraphicsItem item)
    {
        if (item is GraphicsItem graphicsItem)
        {
            return graphicsItem.GetSceneTransform() ?? new MatrixTransform(Matrix.Identity);
        }
        return new TranslateTransform(item.Position.X, item.Position.Y);
    }

    public void SetTransform(Transform transform)
    {
        if (transform is TransformGroup group)
        {
            var matrix = group.Value;
            ApplyMatrix(matrix);
        }
        else
        {
            ApplyMatrix(transform.Value);
        }

        InvalidateVisual();
    }

    private void ApplyMatrix(Matrix matrix)
    {
        _scaleTransform.ScaleX = matrix.M11;
        _scaleTransform.ScaleY = matrix.M22;

        _rotateTransform.Angle = Math.Atan2(matrix.M21, matrix.M11) * 180 / Math.PI;

        _scrollTranslate.X = matrix.OffsetX;
        _scrollTranslate.Y = matrix.OffsetY;
    }

    public void ResetTransform()
    {
        _scaleTransform.ScaleX = 1;
        _scaleTransform.ScaleY = 1;
        _rotateTransform.Angle = 0;
        _scrollTranslate.X = 0;
        _scrollTranslate.Y = 0;
        InvalidateVisual();
    }

    public void Scale(double scaleX, double scaleY)
    {
        _scaleTransform.ScaleX *= scaleX;
        _scaleTransform.ScaleY *= scaleY;

        InvalidateVisual();
    }

    public void Rotate(double angle)
    {
        var anchorPoint = GetAnchorPoint();
        _rotateTransform.Angle += angle;

        AdjustForAnchor(anchorPoint);
        InvalidateVisual();
    }

    public void Rotate(double angle, Point anchorPoint)
    {
        _rotateTransform.Angle += angle;

        AdjustForAnchor(anchorPoint);
        InvalidateVisual();
    }

    public void Translate(double dx, double dy)
    {
        _scrollTranslate.X += dx;
        _scrollTranslate.Y += dy;
        InvalidateVisual();
    }

    private void AdjustForAnchor(Point anchorPoint)
    {
        var sceneAnchor = MapToScene(anchorPoint);
        var adjustedAnchor = MapFromScene(sceneAnchor);

        _scrollTranslate.X += anchorPoint.X - adjustedAnchor.X;
        _scrollTranslate.Y += anchorPoint.Y - adjustedAnchor.Y;
    }

    private Point GetAnchorPoint()
    {
        return TransformationAnchor switch
        {
            ViewportAnchor.AnchorViewCenter => new Point(RenderSize.Width / 2, RenderSize.Height / 2),
            ViewportAnchor.AnchorUnderMouse => new Point(RenderSize.Width / 2, RenderSize.Height / 2),
            _ => new Point(0, 0)
        };
    }

    public Point MapToScene(Point viewPoint)
    {
        var inverse = _viewTransformGroup.Inverse;
        if (inverse == null)
            return viewPoint;

        return inverse.Transform(viewPoint);
    }

    public Point MapFromScene(Point scenePoint)
    {
        return _viewTransformGroup.Transform(scenePoint);
    }

    public Rect MapToScene(Rect viewRect)
    {
        var inverse = _viewTransformGroup.Inverse;
        if (inverse == null)
            return viewRect;

        var topLeft = inverse.Transform(viewRect.TopLeft);
        var bottomRight = inverse.Transform(viewRect.BottomRight);
        var topRight = inverse.Transform(viewRect.TopRight);
        var bottomLeft = inverse.Transform(viewRect.BottomLeft);

        var minX = Math.Min(topLeft.X, Math.Min(bottomRight.X, Math.Min(topRight.X, bottomLeft.X)));
        var minY = Math.Min(topLeft.Y, Math.Min(bottomRight.Y, Math.Min(topRight.Y, bottomLeft.Y)));
        var maxX = Math.Max(topLeft.X, Math.Max(bottomRight.X, Math.Max(topRight.X, bottomLeft.X)));
        var maxY = Math.Max(topLeft.Y, Math.Max(bottomRight.Y, Math.Max(topRight.Y, bottomLeft.Y)));

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    public Rect MapFromScene(Rect sceneRect)
    {
        var topLeft = _viewTransformGroup.Transform(sceneRect.TopLeft);
        var bottomRight = _viewTransformGroup.Transform(sceneRect.BottomRight);
        var topRight = _viewTransformGroup.Transform(sceneRect.TopRight);
        var bottomLeft = _viewTransformGroup.Transform(sceneRect.BottomLeft);

        var minX = Math.Min(topLeft.X, Math.Min(bottomRight.X, Math.Min(topRight.X, bottomLeft.X)));
        var minY = Math.Min(topLeft.Y, Math.Min(bottomRight.Y, Math.Min(topRight.Y, bottomLeft.Y)));
        var maxX = Math.Max(topLeft.X, Math.Max(bottomRight.X, Math.Max(topRight.X, bottomLeft.X)));
        var maxY = Math.Max(topLeft.Y, Math.Max(bottomRight.Y, Math.Max(topRight.Y, bottomLeft.Y)));

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    public void CenterOn(Point position)
    {
        var viewCenter = new Point(RenderSize.Width / 2, RenderSize.Height / 2);
        var currentViewPos = MapFromScene(position);
        var dx = viewCenter.X - currentViewPos.X;
        var dy = viewCenter.Y - currentViewPos.Y;
        Translate(dx, dy);
    }

    public void CenterOn(IGraphicsItem item)
    {
        var center = item.SceneBoundingRect;
        var position = new Point(center.X + center.Width / 2, center.Y + center.Height / 2);
        CenterOn(position);
    }

    public void EnsureVisible(Rect area, double marginX = 50, double marginY = 50)
    {
        var viewportRect = new Rect(0, 0, RenderSize.Width, RenderSize.Height);
        var sceneArea = MapToScene(area);

        if (!viewportRect.IntersectsWith(MapFromScene(sceneArea)))
        {
            CenterOn(new Point(sceneArea.X + sceneArea.Width / 2, sceneArea.Y + sceneArea.Height / 2));
            return;
        }

        var mappedArea = MapFromScene(sceneArea);

        if (mappedArea.Left < marginX)
        {
            Translate(mappedArea.Left - marginX, 0);
        }
        else if (mappedArea.Right > RenderSize.Width - marginX)
        {
            Translate(mappedArea.Right - RenderSize.Width + marginX, 0);
        }

        if (mappedArea.Top < marginY)
        {
            Translate(0, mappedArea.Top - marginY);
        }
        else if (mappedArea.Bottom > RenderSize.Height - marginY)
        {
            Translate(0, mappedArea.Bottom - RenderSize.Height + marginY);
        }
    }

    public void EnsureVisible(IGraphicsItem item, double marginX = 50, double marginY = 50)
    {
        EnsureVisible(item.SceneBoundingRect, marginX, marginY);
    }

    private static void OnSceneChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var view = (GraphicsView)d;

        if (view._scene is GraphicsScene oldScene)
        {
            oldScene.Invalidated -= view.OnSceneInvalidated;
        }

        view._scene = (IGraphicsScene?)e.NewValue;

        if (view._scene is GraphicsScene newScene)
        {
            newScene.Invalidated += view.OnSceneInvalidated;
        }

        view.InvalidateVisual();
    }
}
