using System.Windows;
using System.Windows.Media;

namespace GraphicsViewFramework;

public enum DragMode
{
    NoDrag,
    ScrollHandDrag,
    RubberBandDrag
}

public enum ViewportAnchor
{
    NoAnchor,
    AnchorViewCenter,
    AnchorUnderMouse
}

public interface IGraphicsView
{
    IGraphicsScene? Scene { get; set; }

    Transform ViewTransform { get; }
    void SetTransform(Transform transform);
    void ResetTransform();

    void Scale(double scaleX, double scaleY);
    void Rotate(double angle);
    void Rotate(double angle, Point anchorPoint);
    void Translate(double dx, double dy);

    ViewportAnchor TransformationAnchor { get; set; }
    ViewportAnchor ResizeAnchor { get; set; }
    DragMode DragMode { get; set; }

    Point MapToScene(Point viewPoint);
    Point MapFromScene(Point scenePoint);
    Rect MapToScene(Rect viewRect);
    Rect MapFromScene(Rect sceneRect);

    void CenterOn(Point position);
    void CenterOn(IGraphicsItem item);
    void EnsureVisible(Rect area, double marginX = 50, double marginY = 50);
    void EnsureVisible(IGraphicsItem item, double marginX = 50, double marginY = 50);
}
