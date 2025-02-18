﻿using NetwalkLogic.Elements.BaseElements;

namespace NetwalkLogic.Elements.PipeElements;

public class PipeCornerElement : BaseCornerElement
{
    public PipeCornerElement() : base() 
    {
        SetIcons();
    }

    public PipeCornerElement(List<Direction> directions) : base(directions)
    {
        SetIcons();
    }

    private void SetIcons()
    {
        IconNormal = "Assets/Elements/PipeElements/PipeCornerElement.png";
        IconConnectedToServer = "Assets/Elements/PipeElements/PipeCornerElement_ConnectedToServer.png";
        IconLoop = "Assets/Elements/PipeElements/PipeCornerElement_Loop.png";
    }
}
