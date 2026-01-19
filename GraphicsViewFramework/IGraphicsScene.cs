using System.Windows;

namespace GraphicsViewFramework;

public interface IGraphicsScene
{
    IList<IGraphicsItem> Items { get; }
    event EventHandler? Invalidated;

    Rect SceneRect { get; set; }

    void AddItem(IGraphicsItem item);
    void RemoveItem(IGraphicsItem item);
    void Clear();

    IGraphicsItem? ItemAt(Point scenePoint);
    IList<IGraphicsItem> ItemsAt(Point scenePoint);
    IList<IGraphicsItem> ItemsInRect(Rect sceneRect);

    Rect SceneBoundingRect();
    void UpdateSceneRect();
}
