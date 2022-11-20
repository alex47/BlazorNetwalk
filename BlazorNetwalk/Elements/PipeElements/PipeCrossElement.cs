﻿using BlazorNetwalk.Elements.BaseElements;

namespace BlazorNetwalk.Elements.PipeElements;

public class PipeCrossElement : BaseCrossElement
{
    public PipeCrossElement() : base() 
    {
        SetIcons();
    }

    public PipeCrossElement(List<Direction> directions) : base(directions)
    {
        SetIcons();
    }

    private void SetIcons()
    {
        IconNormal = "Assets/Elements/PipeElements/PipeCrossElement.png";
        IconConnectedToServer = "Assets/Elements/PipeElements/PipeCrossElement_ConnectedToServer.png";
    }
}
