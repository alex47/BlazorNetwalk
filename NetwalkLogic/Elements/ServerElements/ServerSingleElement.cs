using NetwalkLogic.Elements.BaseElements;

namespace NetwalkLogic.Elements.ServerElements;

public class ServerSingleElement : BaseSingleElement
{
    public ServerSingleElement() : base()
    {
        IconNormal = "Assets/Elements/ServerElements/ServerSingleElement.png";
        IconConnectedToServer = IconNormal;

        IsConnectedToServer = true;
    }
}
