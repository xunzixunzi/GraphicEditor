using System.Windows;
using System.Windows.Media;

namespace GraphicsViewFramework;

public interface IGraphicsItem
{
    IGraphicsScene? Scene { get; set; }
    IGraphicsItem? Parent { get; set; }

    Point Position { get; set; }
    double Rotation { get; set; }
    double Scale { get; set; }
    double ZValue { get; set; }

    bool IsVisible { get; set; }
    bool IsEnabled { get; set; }
    bool IsSelected { get; set; }

    bool Contains(Point localPoint);
    Rect BoundingRect { get; }
    Rect SceneBoundingRect { get; }

    void Paint(DrawingContext context);

    Point ScenePosition { get; }
    Transform SceneTransform { get; }

    Point MapToScene(Point point);
    Point MapFromScene(Point point);
    Rect MapToScene(Rect rect);
    Rect MapFromScene(Rect rect);

    Point MapToParent(Point point);
    Point MapFromParent(Point point);
    Rect MapToParent(Rect rect);
    Rect MapFromParent(Rect rect);

    Point MapToItem(IGraphicsItem? item, Point point);
    Point MapFromItem(IGraphicsItem? item, Point point);
    Rect MapToItem(IGraphicsItem? item, Rect rect);
    Rect MapFromItem(IGraphicsItem? item, Rect rect);
}
