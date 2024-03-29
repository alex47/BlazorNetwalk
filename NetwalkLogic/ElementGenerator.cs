﻿using NetwalkLogic.Elements;
using NetwalkLogic.Elements.ComputerElements;
using NetwalkLogic.Elements.PipeElements;
using static NetwalkLogic.Elements.AbstractElement;

namespace NetwalkLogic;

public class ElementGenerator
{
    public static AbstractElement CreateElementWithDirections(List<Direction> directions)
    {
        if (directions.Count < 1 || 3 < directions.Count)
        {
            throw new ArgumentException("Direction count must be between zero and three.");
        }

        return directions.Count switch
        {
            1 => new ComputerSingleElement(directions[0]),
            2 => GetCornerOrLineElementWithDirections(directions),
            3 => new PipeCrossElement(directions),
            _ => throw new Exception("This should never happen."),
        };
    }

    private static AbstractElement GetCornerOrLineElementWithDirections(List<Direction> directions)
    {
        try
        {
            return new PipeLineElement(directions);
        }
        catch (InvalidDataException) { }

        try
        {
            return new PipeCornerElement(directions);
        }
        catch (InvalidDataException) { }

        throw new Exception("This should never happen.");
    }
}
