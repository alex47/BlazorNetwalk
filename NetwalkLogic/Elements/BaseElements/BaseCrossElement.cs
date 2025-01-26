namespace NetwalkLogic.Elements.BaseElements;

public class BaseCrossElement : AbstractElement
{
    public BaseCrossElement() { }

    public BaseCrossElement(List<Direction> directions)
    {
        if (directions.Count != 3)
        {
            throw new ArgumentException($"Direction count must be exactly three for { nameof(BaseCrossElement) }");
        }
        else if (directions.Contains(Direction.Left) && directions.Contains(Direction.Right) && directions.Contains(Direction.Top))
        {
            Position = 1;
        }
        else if (directions.Contains(Direction.Left) && directions.Contains(Direction.Right) && directions.Contains(Direction.Bottom))
        {
            Position = 3;
        }
        else if (directions.Contains(Direction.Top) && directions.Contains(Direction.Bottom) && directions.Contains(Direction.Left))
        {
            Position = 0;
        }
        else if (directions.Contains(Direction.Top) && directions.Contains(Direction.Bottom) && directions.Contains(Direction.Right))
        {
            Position = 2;
        }
        else
        {
            throw new ArgumentException($"Directions invalid for ${ nameof(BaseCrossElement) }");
        }
    }

    public override bool TryFixing()
    {
        if (IsFixed)
        {
            return false;
        }

        TryFixing_OneNeighbourFixed_FacingAway();
        TryFixing_ThreeNeighboursFixed_FacingThis();

        return IsFixed;
    }

    private void TryFixing_OneNeighbourFixed_FacingAway()
    {
        if (IsFixed)
        {
            return;
        }

        else if (CanNotConnectToTop())
        {
            SetFixedPosition(3);
        }

        else if (CanNotConnectToBottom())
        {
            SetFixedPosition(1);
        }

        else if (CanNotConnectToLeft())
        {
            SetFixedPosition(0);
        }

        else if (CanNotConnectToRight())
        {
            SetFixedPosition(2);
        }
    }

    private void TryFixing_ThreeNeighboursFixed_FacingThis()
    {
        if (IsFixed)
        {
            return;
        }

        else if (MustConnectToTop() && MustConnectToBottom() && MustConnectToLeft())
        {
            SetFixedPosition(2);
        }

        else if (MustConnectToTop() && MustConnectToBottom() && MustConnectToRight())
        {
            SetFixedPosition(0);
        }

        else if (MustConnectToLeft() && MustConnectToRight() && MustConnectToTop())
        {
            SetFixedPosition(1);
        }

        else if (MustConnectToLeft() && MustConnectToRight() && MustConnectToBottom())
        {
            SetFixedPosition(3);
        }
    }


    public override List<Direction> GetConnectionDirections() => Position switch
    {
        0 => new List<Direction>() { Direction.Top, Direction.Bottom, Direction.Right },
        1 => new List<Direction>() { Direction.Left, Direction.Right, Direction.Top },
        2 => new List<Direction>() { Direction.Top, Direction.Bottom, Direction.Left },
        3 => new List<Direction>() { Direction.Left, Direction.Right, Direction.Bottom },
        _ => throw new ArgumentOutOfRangeException("Position must be between 0 and 3"),
    };


    public override bool HasConnectionToLeft() => Position != 0;

    public override bool HasConnectionToRight() => Position != 2;

    public override bool HasConnectionToTop() => Position != 3;

    public override bool HasConnectionToBottom() => Position != 1;


    public override bool ElementCanNotConnectToLeft() => false;

    public override bool ElementCanNotConnectToRight() => false;

    public override bool ElementCanNotConnectToTop() => false;

    public override bool ElementCanNotConnectToBottom() => false;
}
