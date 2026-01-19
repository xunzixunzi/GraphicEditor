using System.Windows;

namespace ImageEditorFramework;

public interface IDrawingTool
{
    string Name { get; }
    void OnMouseDown(Point scenePoint, ImageScene scene, ImageView view);
    void OnMouseMove(Point scenePoint, ImageScene scene, ImageView view);
    void OnMouseUp(Point scenePoint, ImageScene scene, ImageView view);
}
