using NetwalkLogic.Elements.BaseElements;

namespace NetwalkLogic.Elements.ServerElements;

public class ServerCrossElement : BaseCrossElement
{
    public ServerCrossElement() : base()
    {
        IconNormal = "Assets/Elements/ServerElements/ServerCrossElement.png";
        IconConnectedToServer = IconNormal;
        IconLoop = IconNormal;

        IsConnectedToServer = true;
    }
}
