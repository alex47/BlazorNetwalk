namespace NetwalkLogic.Elements;

public abstract class AbstractElement
{
    public enum Direction
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public string IconNormal = string.Empty;
    public string IconConnectedToServer = string.Empty;
    
    public int Position = 0;
    public List<Direction> ConnectedDirections = new();

    public bool IsFixed = false;
    public bool IsConnectedToServer = false;

    public int X = 0;
    public int Y = 0;

    public AbstractElement? LeftElement;
    public AbstractElement? RightElement;
    public AbstractElement? TopElement;
    public AbstractElement? BottomElement;

    protected AbstractElement() { }


    public void Rotate()
    {
        if (IsFixed)
        {
            return;
        }

        Position = (Position + 1) % 4;
    }

    public void SetFixedPosition(int position)
    {
        IsFixed = true;
        Position = position;
    }

    public abstract bool TryFixing();


    public abstract List<Direction> GetConnectionDirections();


    public abstract bool HasConnectionToLeft();

    public abstract bool HasConnectionToRight();

    public abstract bool HasConnectionToTop();

    public abstract bool HasConnectionToBottom();


    public bool IsConnectedWithLeftElement() => HasConnectionToLeft() && LeftElement is not null && LeftElement.HasConnectionToRight();

    public bool IsConnectedWithRightElement() => HasConnectionToRight() && RightElement is not null && RightElement.HasConnectionToLeft();

    public bool IsConnectedWithTopElement() => HasConnectionToTop() && TopElement is not null && TopElement.HasConnectionToBottom();

    public bool IsConnectedWithBottomElement() => HasConnectionToBottom() && BottomElement is not null && BottomElement.HasConnectionToTop();


    public bool MustConnectToLeft() => LeftElement is not null && LeftElement.IsFixed && LeftElement.HasConnectionToRight();

    public bool MustConnectToRight() => RightElement is not null && RightElement.IsFixed && RightElement.HasConnectionToLeft();

    public bool MustConnectToTop() => TopElement is not null && TopElement.IsFixed && TopElement.HasConnectionToBottom();

    public bool MustConnectToBottom() => BottomElement is not null && BottomElement.IsFixed && BottomElement.HasConnectionToTop();


    public virtual bool CanNotConnectToLeft()
    {
        if (LeftElement is null)
        {
            return true;
        }

        if (LeftElement.IsFixed && LeftElement.HasConnectionToRight() == false)
        {
            return true;
        }

        return false;
    }

    public virtual bool CanNotConnectToRight()
    {
        if (RightElement is null)
        {
            return true;
        }

        if (RightElement.IsFixed && RightElement.HasConnectionToLeft() == false)
        {
            return true;
        }

        return false;
    }

    public virtual bool CanNotConnectToTop()
    {
        if (TopElement is null)
        {
            return true;
        }

        if (TopElement.IsFixed && TopElement.HasConnectionToBottom() == false)
        {
            return true;
        }

        return false;
    }

    public virtual bool CanNotConnectToBottom()
    {
        if (BottomElement is null)
        {
            return true;
        }

        if (BottomElement.IsFixed && BottomElement.HasConnectionToTop() == false)
        {
            return true;
        }

        return false;
    }
}
