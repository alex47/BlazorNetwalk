using BlazorNetwalk.Elements.BaseElements;

namespace BlazorNetwalk.Elements.ServerElements;

public class ServerCrossElement : BaseCrossElement
{
    public ServerCrossElement() : base()
    {
        IconNormal = "Assets/Elements/ServerElements/ServerCrossElement.png";
        IconConnectedToServer = IconNormal;

        IsConnectedToServer = true;
    }
}
