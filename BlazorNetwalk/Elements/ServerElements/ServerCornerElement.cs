using BlazorNetwalk.Elements.BaseElements;

namespace BlazorNetwalk.Elements.ServerElements;

public class ServerCornerElement : BaseCornerElement
{
    public ServerCornerElement() : base()
    {
        IconNormal = "Assets/Elements/ServerElements/ServerCornerElement.png";
        IconConnectedToServer = IconNormal;

        IsConnectedToServer = true;
    }
}
