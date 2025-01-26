using NetwalkLogic.Elements.BaseElements;

namespace NetwalkLogic.Elements.PipeElements;

public class PipeLineElement : BaseLineElement
{
    public PipeLineElement() : base() 
    {
        SetIcons();
    }

    public PipeLineElement(List<Direction> directions) : base(directions)
    {
        SetIcons();
    }

    private void SetIcons()
    {
        IconNormal = "Assets/Elements/PipeElements/PipeLineElement.png";
        IconConnectedToServer = "Assets/Elements/PipeElements/PipeLineElement_ConnectedToServer.png";
    }
}
