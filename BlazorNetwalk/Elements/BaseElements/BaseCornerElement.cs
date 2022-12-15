using BlazorNetwalk.Elements.ComputerElements;

namespace BlazorNetwalk.Elements.BaseElements;

public class BaseCornerElement : AbstractElement
{
    public BaseCornerElement() { }

    public BaseCornerElement(List<Direction> directions)
    {
        if (directions.Count != 2)
        {
            throw new ArgumentException($"Direction count must be exactly two for { nameof(BaseCornerElement) }");
        }
        else if (directions.Contains(Direction.Left) && directions.Contains(Direction.Top))
        {
            Position = 2;
        }

        else if (directions.Contains(Direction.Left) && directions.Contains(Direction.Bottom))
        {
            Position = 3;
        }

        else if (directions.Contains(Direction.Right) && directions.Contains(Direction.Top))
        {
            Position = 1;
        }

        else if (directions.Contains(Direction.Right) && directions.Contains(Direction.Bottom))
        {
            Position = 0;
        }
        else
        {
            throw new InvalidDataException($"Directions invalid for ${ nameof(BaseCornerElement) }");
        }
    }

    public override bool TryFixing()
    {
        if (IsFixed)
        {
            return false;
        }

        TryFixing_Bridge();
        TryFixing_OneNeighbourFixedIsComputer_OtherCanNotBeComputer();
        TryFixing_OneNeighbourFixed();
        TryFixing_TwoNeighboursFixed();

        return IsFixed;
    }

    private void TryFixing_Bridge()
    {
        if (IsFixed)
        {
            return;
        }

        else if (MustConnectToTop())
        {
            if (MustConnectToLeft())
            {
                SetFixedPosition(2);
            }
            else if (MustConnectToRight())
            {
                SetFixedPosition(1);
            }
        }

        else if (MustConnectToBottom())
        {
            if (MustConnectToLeft())
            {
                SetFixedPosition(3);
            }
            else if (MustConnectToRight())
            {
                SetFixedPosition(0);
            }
        }
    }

    private void TryFixing_OneNeighbourFixedIsComputer_OtherCanNotBeComputer()
    {
        if (IsFixed)
        {
            return;
        }

        else if (MustConnectToLeft() && LeftElement is ComputerSingleElement)
        {
            if (TopElement is ComputerSingleElement)
            {
                SetFixedPosition(3);
            }
            else if (BottomElement is ComputerSingleElement)
            {
                SetFixedPosition(2);
            }
        }

        else if (MustConnectToRight() && RightElement is ComputerSingleElement)
        {
            if (TopElement is ComputerSingleElement)
            {
                SetFixedPosition(0);
            }
            else if (BottomElement is ComputerSingleElement)
            {
                SetFixedPosition(1);
            }
        }

        else if (MustConnectToTop() && TopElement is ComputerSingleElement)
        {
            if (LeftElement is ComputerSingleElement)
            {
                SetFixedPosition(1);
            }
            else if (RightElement is ComputerSingleElement)
            {
                SetFixedPosition(2);
            }
        }

        else if (MustConnectToBottom() && BottomElement is ComputerSingleElement)
        {
            if (LeftElement is ComputerSingleElement)
            {
                SetFixedPosition(0);
            }
            else if (RightElement is ComputerSingleElement)
            {
                SetFixedPosition(3);
            }
        }
    }

    private void TryFixing_OneNeighbourFixed()
    {
        if (IsFixed)
        {
            return;
        }

        else if (MustConnectToTop())
        {
            if (CanNotConnectToLeft())
            {
                SetFixedPosition(1);
            }
            else if (CanNotConnectToRight())
            {
                SetFixedPosition(2);
            }
        }

        else if (MustConnectToBottom())
        {
            if (CanNotConnectToLeft())
            {
                SetFixedPosition(0);
            }
            else if (CanNotConnectToRight())
            {
                SetFixedPosition(3);
            }
        }

        else if (MustConnectToLeft())
        {
            if (CanNotConnectToTop())
            {
                SetFixedPosition(3);
            }
            else if (CanNotConnectToBottom())
            {
                SetFixedPosition(2);
            }
        }

        else if (MustConnectToRight())
        {
            if (CanNotConnectToTop())
            {
                SetFixedPosition(0);
            }
            else if (CanNotConnectToBottom())
            {
                SetFixedPosition(1);
            }
        }
    }

    private void TryFixing_TwoNeighboursFixed()
    {
        if (IsFixed)
        {
            return;
        }

        else if (CanNotConnectToTop())
        {
            if (CanNotConnectToLeft())
            {
                SetFixedPosition(0);
            }
            else if (CanNotConnectToRight())
            {
                SetFixedPosition(3);
            }
        }

        else if (CanNotConnectToBottom())
        {
            if (CanNotConnectToLeft())
            {
                SetFixedPosition(1);
            }
            else if (CanNotConnectToRight())
            {
                SetFixedPosition(2);
            }
        }
    }


    public override List<Direction> GetConnectionDirections()
    {
        return Position switch
        {
            0 => new List<Direction>() { Direction.Right, Direction.Bottom },
            1 => new List<Direction>() { Direction.Right, Direction.Top },
            2 => new List<Direction>() { Direction.Left, Direction.Top },
            3 => new List<Direction>() { Direction.Left, Direction.Bottom },
            _ => throw new ArgumentOutOfRangeException("Position must be between 0 and 3"),
        };
    }


    public override bool HasConnectionToLeft() => Position == 2 || Position == 3;

    public override bool HasConnectionToRight() => Position == 0 || Position == 1;

    public override bool HasConnectionToTop() => Position == 1 || Position == 2;

    public override bool HasConnectionToBottom() => Position == 0 || Position == 3;


    public override bool CanNotConnectToLeft()
    {
        if (base.CanNotConnectToLeft())
        {
            return true;
        }

        if (MustConnectToRight())
        {
            return true;
        }

        return false;
    }

    public override bool CanNotConnectToRight()
    {
        if (base.CanNotConnectToRight())
        {
            return true;
        }

        if (MustConnectToLeft())
        {
            return true;
        }

        return false;
    }

    public override bool CanNotConnectToTop()
    {
        if (base.CanNotConnectToTop())
        {
            return true;
        }

        if (MustConnectToBottom())
        {
            return true;
        }

        return false;
    }

    public override bool CanNotConnectToBottom()
    {
        if (base.CanNotConnectToBottom())
        {
            return true;
        }

        if (MustConnectToTop())
        {
            return true;
        }

        return false;
    }
}
