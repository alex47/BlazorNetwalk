using NetwalkLogic.Elements.ComputerElements;

namespace NetwalkLogic.Elements.BaseElements;

public class BaseSingleElement : AbstractElement
{
    public BaseSingleElement() { }

    public BaseSingleElement(Direction direction)
    {
        Position = direction switch
        {
            Direction.Left => 2,
            Direction.Right => 0,
            Direction.Top => 1,
            Direction.Bottom => 3,
            _ => throw new Exception("This should never happen."),
        };
    }

    public override bool TryFixing()
    {
        if (IsFixed)
        {
            return false;
        }

        TryFixing_OneNeighbourFixed_FacingThis();
        TryFixing_ThreeNeighbours_CanNotConnect();

        return IsFixed;
    }

    private void TryFixing_OneNeighbourFixed_FacingThis()
    {
        if (IsFixed)
        {
            return;
        }

        else if (MustConnectToTop())
        {
            SetFixedPosition(1);
        }

        else if (MustConnectToBottom())
        {
            SetFixedPosition(3);
        }

        else if (MustConnectToLeft())
        {
            SetFixedPosition(2);
        }

        else if (MustConnectToRight())
        {
            SetFixedPosition(0);
        }
    }

    private void TryFixing_ThreeNeighbours_CanNotConnect()
    {
        if (IsFixed)
        {
            return;
        }

        else if (CanNotConnectToTop() && CanNotConnectToBottom() && CanNotConnectToLeft())
        {
            SetFixedPosition(0);
        }

        else if (CanNotConnectToTop() && CanNotConnectToBottom() && CanNotConnectToRight())
        {
            SetFixedPosition(2);
        }

        else if (CanNotConnectToLeft() && CanNotConnectToRight() && CanNotConnectToTop())
        {
            SetFixedPosition(3);
        }

        else if (CanNotConnectToLeft() && CanNotConnectToRight() && CanNotConnectToBottom())
        {
            SetFixedPosition(1);
        }
    }


    public override List<Direction> GetConnectionDirections()
    {
        return Position switch
        {
            0 => new List<Direction>() { Direction.Right },
            1 => new List<Direction>() { Direction.Top },
            2 => new List<Direction>() { Direction.Left },
            3 => new List<Direction>() { Direction.Bottom },
            _ => throw new ArgumentOutOfRangeException("Position must be between 0 and 3"),
        };
    }


    public override bool CanNotConnectToLeft() => base.CanNotConnectToLeft() || LeftElement is BaseSingleElement;

    public override bool CanNotConnectToRight() => base.CanNotConnectToRight() || RightElement is BaseSingleElement;

    public override bool CanNotConnectToTop() => base.CanNotConnectToTop() || TopElement is BaseSingleElement;

    public override bool CanNotConnectToBottom() => base.CanNotConnectToBottom() || BottomElement is BaseSingleElement;


    public override bool HasConnectionToLeft() => Position == 2;

    public override bool HasConnectionToRight() => Position == 0;

    public override bool HasConnectionToTop() => Position == 1;

    public override bool HasConnectionToBottom() => Position == 3;


    public override bool ElementCanNotConnectToLeft() => false;

    public override bool ElementCanNotConnectToRight() => false;

    public override bool ElementCanNotConnectToTop() => false;

    public override bool ElementCanNotConnectToBottom() => false;
}
