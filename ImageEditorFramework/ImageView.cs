using GraphicsViewFramework;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageEditorFramework;

public enum InteractionMode
{
    View,
    Draw
}

public class ImageView : GraphicsView
{
    private IDrawingTool? _currentTool;

    public static readonly DependencyProperty InteractionModeProperty =
        DependencyProperty.Register(
            nameof(InteractionMode),
            typeof(InteractionMode),
            typeof(ImageView),
            new PropertyMetadata(InteractionMode.View));

    public InteractionMode InteractionMode
    {
        get => (InteractionMode)GetValue(InteractionModeProperty);
        set => SetValue(InteractionModeProperty, value);
    }

    public IDrawingTool? CurrentTool
    {
        get => _currentTool;
        set
        {
            if (_currentTool != value)
            {
                _currentTool = value;
                InteractionMode = _currentTool != null ? InteractionMode.Draw : InteractionMode.View;
            }
        }
    }

    public ImageView()
    {
        ZoomSpeed = 0.001;
    }

    public ImageScene? ImageScene => Scene as ImageScene;

    public ImageSource? Image
    {
        get => ImageScene?.Image;
        set
        {
            if (ImageScene != null)
            {
                ImageScene.Image = value;
            }
        }
    }

    public void FitToImage()
    {
        if (ImageScene?.ImageItem == null || ActualWidth <= 0 || ActualHeight <= 0)
            return;

        var bounds = ImageScene.ImageItem.SceneBoundingRect;
        var scaleX = ActualWidth / bounds.Width;
        var scaleY = ActualHeight / bounds.Height;
        var scale = Math.Min(scaleX, scaleY) * 0.9;

        SetTransform(new ScaleTransform(scale, scale));

        var center = new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
        CenterOn(center);
    }

    protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
    {
        if (InteractionMode == InteractionMode.Draw && CurrentTool != null && ImageScene != null && e.ChangedButton == MouseButton.Left)
        {
            var viewPoint = e.GetPosition(this);
            var scenePoint = MapToScene(viewPoint);
            CurrentTool.OnMouseDown(scenePoint, ImageScene, this);
            e.Handled = true;
        }
        else
        {
            base.OnPreviewMouseDown(e);
        }
    }

    protected override void OnPreviewMouseMove(MouseEventArgs e)
    {
        if (InteractionMode == InteractionMode.Draw && CurrentTool != null && ImageScene != null)
        {
            var viewPoint = e.GetPosition(this);
            var scenePoint = MapToScene(viewPoint);
            CurrentTool.OnMouseMove(scenePoint, ImageScene, this);
            e.Handled = true;
        }
        else
        {
            base.OnPreviewMouseMove(e);
        }
    }

    protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
    {
        if (InteractionMode == InteractionMode.Draw && CurrentTool != null && ImageScene != null && e.ChangedButton == MouseButton.Left)
        {
            var viewPoint = e.GetPosition(this);
            var scenePoint = MapToScene(viewPoint);
            CurrentTool.OnMouseUp(scenePoint, ImageScene, this);
            e.Handled = true;
        }
        else
        {
            base.OnPreviewMouseUp(e);
        }
    }
}
