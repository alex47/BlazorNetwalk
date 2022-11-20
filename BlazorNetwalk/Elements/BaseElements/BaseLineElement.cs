using BlazorNetwalk.Elements.ComputerElements;

namespace BlazorNetwalk.Elements.BaseElements;

public class BaseLineElement : AbstractElement
{
    public BaseLineElement() { }

    public BaseLineElement(List<Direction> directions)
    {
        if (directions.Count != 2)
        {
            throw new ArgumentException($"Direction count must be exactly two for { nameof(BaseLineElement) }");
        }
        else if (directions.Contains(Direction.Left) && directions.Contains(Direction.Right))
        {
            Position = 0;
        }
        else if (directions.Contains(Direction.Top) && directions.Contains(Direction.Bottom))
        {
            Position = 1;
        }
        else
        {
            throw new InvalidDataException($"Directions invalid for ${ nameof(BaseLineElement) }");
        }
    }

    public override bool TryFixing()
    {
        if (IsFixed)
        {
            return false;
        }

        TryFixing_OneNeighbourFixed_FacingThis();
        TryFixing_OneNeighbourFixed_FacingAway();
        TryFixing_BetweenTwoComputerElements();

        return IsFixed;
    }

    private void TryFixing_OneNeighbourFixed_FacingThis()
    {
        if (IsFixed)
        {
            return;
        }

        else if (MustConnectToTop() || MustConnectToBottom())
        {
            SetFixedPosition(1);
        }

        else if (MustConnectToLeft() || MustConnectToRight())
        {
            SetFixedPosition(0);
        }
    }

    private void TryFixing_OneNeighbourFixed_FacingAway()
    {
        if (IsFixed)
        {
            return;
        }

        else if (CanNotConnectToLeft() || CanNotConnectToRight())
        {
            SetFixedPosition(1);
        }

        else if (CanNotConnectToTop() || CanNotConnectToBottom())
        {
            SetFixedPosition(0);
        }
    }

    private void TryFixing_BetweenTwoComputerElements()
    {
        if (IsFixed)
        {
            return;
        }

        else if (TopElement is ComputerSingleElement && BottomElement is ComputerSingleElement)
        {
            SetFixedPosition(0);
        }

        else if (LeftElement is ComputerSingleElement && RightElement is ComputerSingleElement)
        {
            SetFixedPosition(1);
        }
    }


    public override List<Direction> GetConnectionDirections()
    {
        return Position switch
        {
            0 => new List<Direction>() { Direction.Left, Direction.Right },
            1 => new List<Direction>() { Direction.Top, Direction.Bottom },
            2 => new List<Direction>() { Direction.Left, Direction.Right },
            3 => new List<Direction>() { Direction.Top, Direction.Bottom },
            _ => throw new ArgumentOutOfRangeException("Position must be between 0 and 3"),
        };
    }


    public override bool HasConnectionToLeft() => Position == 0 || Position == 2;

    public override bool HasConnectionToRight() => HasConnectionToLeft();

    public override bool HasConnectionToTop() => Position == 1 || Position == 3;

    public override bool HasConnectionToBottom() => HasConnectionToTop();
}
