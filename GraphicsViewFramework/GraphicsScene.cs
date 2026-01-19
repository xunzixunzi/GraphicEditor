using System.Windows;

namespace GraphicsViewFramework;

public class GraphicsScene : IGraphicsScene
{
    private readonly List<IGraphicsItem> _items = new();
    private Rect _sceneRect = Rect.Empty;

    public IList<IGraphicsItem> Items => _items.AsReadOnly();
    public event EventHandler? Invalidated;

    public Rect SceneRect
    {
        get => _sceneRect;
        set
        {
            if (_sceneRect != value)
            {
                _sceneRect = value;
                TriggerInvalidated();
            }
        }
    }

    public void AddItem(IGraphicsItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_items.Contains(item))
            return;

        _items.Add(item);
        item.Scene = this;
        SortItems();
        UpdateSceneRect();
        TriggerInvalidated();
    }

    public void RemoveItem(IGraphicsItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_items.Remove(item))
        {
            item.Scene = null;
            UpdateSceneRect();
            TriggerInvalidated();
        }
    }

    public void Clear()
    {
        foreach (var item in _items)
        {
            item.Scene = null;
        }
        _items.Clear();
        _sceneRect = Rect.Empty;
        TriggerInvalidated();
    }

    public IGraphicsItem? ItemAt(Point scenePoint)
    {
        foreach (var item in _items.OrderBy(i => i.ZValue))
        {
            if (item.IsVisible && item.IsEnabled && item.Contains(scenePoint))
            {
                return item;
            }
        }
        return null;
    }

    public IList<IGraphicsItem> ItemsAt(Point scenePoint)
    {
        var result = new List<IGraphicsItem>();
        foreach (var item in _items.OrderBy(i => i.ZValue))
        {
            if (item.IsVisible && item.IsEnabled && item.Contains(scenePoint))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public IList<IGraphicsItem> ItemsInRect(Rect sceneRect)
    {
        var result = new List<IGraphicsItem>();
        foreach (var item in _items.OrderBy(i => i.ZValue))
        {
            if (item.IsVisible && item.IsEnabled && item.SceneBoundingRect.IntersectsWith(sceneRect))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public Rect SceneBoundingRect()
    {
        var result = Rect.Empty;
        foreach (var item in _items)
        {
            if (item.IsVisible && item.IsEnabled)
            {
                var itemRect = item.SceneBoundingRect;
                if (!itemRect.IsEmpty)
                {
                    if (result.IsEmpty)
                    {
                        result = itemRect;
                    }
                    else
                    {
                        var left = Math.Min(result.Left, itemRect.Left);
                        var top = Math.Min(result.Top, itemRect.Top);
                        var right = Math.Max(result.Right, itemRect.Right);
                        var bottom = Math.Max(result.Bottom, itemRect.Bottom);
                        result = new Rect(left, top, right - left, bottom - top);
                    }
                }
            }
        }
        return result;
    }

    public void UpdateSceneRect()
    {
        var bounds = SceneBoundingRect();
        if (!bounds.IsEmpty)
        {
            var margin = 50.0;
            _sceneRect = new Rect(
                bounds.X - margin,
                bounds.Y - margin,
                bounds.Width + margin * 2,
                bounds.Height + margin * 2);
        }
        else
        {
            _sceneRect = new Rect(0, 0, 2000, 2000);
        }
    }

    internal void TriggerInvalidated()
    {
        Invalidated?.Invoke(this, EventArgs.Empty);
    }

    private void SortItems()
    {
        _items.Sort((a, b) => b.ZValue.CompareTo(a.ZValue));
    }
}
