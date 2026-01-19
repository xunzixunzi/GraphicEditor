using GraphicsViewFramework;
using System.Windows.Media;

namespace ImageEditorFramework;

public class ImageScene : GraphicsScene
{
    private readonly ImageItem _imageItem;

    public ImageItem ImageItem => _imageItem;

    public ImageSource? Image
    {
        get => _imageItem.Source;
        set
        {
            _imageItem.Source = value;
            UpdateSceneRect();
        }
    }

    public ImageScene()
    {
        _imageItem = new ImageItem
        {
            ZValue = double.MinValue
        };
        AddItem(_imageItem);
    }

    public ImageScene(ImageSource image) : this()
    {
        Image = image;
    }
}

