using BlazorNetwalk.Elements.BaseElements;

namespace BlazorNetwalk.Elements.ServerElements;

public class ServerSingleElement : BaseSingleElement
{
    public ServerSingleElement() : base()
    {
        IconNormal = "Assets/Elements/ServerElements/ServerSingleElement.png";
        IconConnectedToServer = IconNormal;

        IsConnectedToServer = true;
    }
}
