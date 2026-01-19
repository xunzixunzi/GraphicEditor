using GraphicsViewFramework;
using System.Windows;
using System.Windows.Input;

namespace ImageEditorFramework;

public class TextTool : IDrawingTool
{
    private string _text = "Text";

    public string Name => "Text";

    public string Text
    {
        get => _text;
        set => _text = value;
    }

    public void OnMouseDown(Point scenePoint, ImageScene scene, ImageView view)
    {
        var textItem = new TextItem
        {
            Position = scenePoint,
            Text = _text,
            FontSize = 24,
            Foreground = System.Windows.Media.Brushes.Black,
            ZValue = 100
        };

        scene.AddItem(textItem);
    }

    public void OnMouseMove(Point scenePoint, ImageScene scene, ImageView view)
    {
    }

    public void OnMouseUp(Point scenePoint, ImageScene scene, ImageView view)
    {
    }
}
