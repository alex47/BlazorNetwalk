using NetwalkLogic.Elements.BaseElements;

namespace NetwalkLogic.Elements.ServerElements;

public class ServerCornerElement : BaseCornerElement
{
    public ServerCornerElement() : base()
    {
        IconNormal = "Assets/Elements/ServerElements/ServerCornerElement.png";
        IconConnectedToServer = IconNormal;
        IconLoop = IconNormal;

        IsConnectedToServer = true;
    }
}
