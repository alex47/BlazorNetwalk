using BlazorNetwalk.Elements.BaseElements;

namespace BlazorNetwalk.Elements.ServerElements;

public class ServerLineElement : BaseLineElement
{
    public ServerLineElement() : base()
    {
        IconNormal = "Assets/Elements/ServerElements/ServerLineElement.png";
        IconConnectedToServer = IconNormal;

        IsConnectedToServer = true;
    }
}
