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

    public string IconNormal { get; protected set; } = string.Empty;
    public string IconConnectedToServer { get; protected set; } = string.Empty;

    public int Position { get; set; } = 0;
    public List<Direction> ConnectedDirections { get; } = new();

    public bool IsFixed { get; private set; } = false;
    public bool IsConnectedToServer { get; set; } = false;

    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;

    public AbstractElement? LeftElement { get; set; }
    public AbstractElement? RightElement { get; set; }
    public AbstractElement? TopElement { get; set; }
    public AbstractElement? BottomElement { get; set; }

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

    public bool IsConnectedWithLeftElement() => HasConnectionToLeft() && LeftElement?.HasConnectionToRight() == true;
    public bool IsConnectedWithRightElement() => HasConnectionToRight() && RightElement?.HasConnectionToLeft() == true;
    public bool IsConnectedWithTopElement() => HasConnectionToTop() && TopElement?.HasConnectionToBottom() == true;
    public bool IsConnectedWithBottomElement() => HasConnectionToBottom() && BottomElement?.HasConnectionToTop() == true;

    public bool MustConnectToLeft() => LeftElement is not null && LeftElement.IsFixed == true && LeftElement.HasConnectionToRight();
    public bool MustConnectToRight() => RightElement is not null && RightElement.IsFixed == true && RightElement.HasConnectionToLeft();
    public bool MustConnectToTop() => TopElement is not null && TopElement.IsFixed == true && TopElement.HasConnectionToBottom();
    public bool MustConnectToBottom() => BottomElement is not null && BottomElement.IsFixed == true && BottomElement.HasConnectionToTop();

    public virtual bool CanNotConnectToLeft() => LeftElement is null || (LeftElement.IsFixed && LeftElement.HasConnectionToRight() == false);
    public virtual bool CanNotConnectToRight() => RightElement is null || (RightElement.IsFixed && RightElement.HasConnectionToLeft() == false);
    public virtual bool CanNotConnectToTop() => TopElement is null || (TopElement.IsFixed && TopElement.HasConnectionToBottom() == false);
    public virtual bool CanNotConnectToBottom() => BottomElement is null || (BottomElement.IsFixed && BottomElement.HasConnectionToTop() == false);
}
