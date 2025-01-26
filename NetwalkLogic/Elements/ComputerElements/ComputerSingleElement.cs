using NetwalkLogic.Elements.BaseElements;

namespace NetwalkLogic.Elements.ComputerElements;

public class ComputerSingleElement : BaseSingleElement
{
    public ComputerSingleElement() : base()
    {
        SetIcons();
    }

    public ComputerSingleElement(Direction direction) : base(direction)
    {
        SetIcons();
    }

    private void SetIcons()
    {
        IconNormal = "Assets/Elements/ComputerElements/ComputerSingleElement.png";
        IconConnectedToServer = "Assets/Elements/ComputerElements/ComputerSingleElement_ConnectedToServer.png";
        IconLoop = IconNormal; // Never used under normal conditions
    }
}
