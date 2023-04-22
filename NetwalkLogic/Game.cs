using NetwalkLogic.Elements;
using NetwalkLogic.Elements.ServerElements;

namespace NetwalkLogic;

public class Game
{
    public AbstractElement[,]? Elements { get; private set; }

    public int RowCount { get; private set; }

    public int ColumnCount { get; private set; }

    private AbstractElement serverElement;

    public Game()
    {

    }

    public void NewGame(int rowCount, int columnCount, bool isWrapping)
    {
        Elements = GameGenerator.CreateNewGame(rowCount, columnCount, isWrapping);

        RowCount = Elements.GetLength(0);
        ColumnCount = Elements.GetLength(1);

        serverElement = GetServerElement();

        UpdateServerConnectionStates();
    }

    public void RestartGame()
    {
        
    }

    public void TrySolveGame()
    {
        if (Elements is null)
        {
            return;
        }

        bool didFixAnyElement;
        do
        {
            didFixAnyElement = false;

            foreach (var element in Elements)
            {
                if (element.TryFixing())
                {
                    didFixAnyElement = true;
                }
            }
        } while (didFixAnyElement);
    }

    public void UpdateServerConnectionStates()
    {
        SetServerConnectionStatesToFalse();
        SetConnectedNeighboursToConnectedToServer(serverElement);
    }

    public bool IsGameSolved()
    {
        if (Elements is null)
        {
            return false;
        }

        return Elements
            .OfType<Elements.ComputerElements.ComputerSingleElement>()
            .All(computer => computer.IsConnectedToServer);
    }

    private void SetServerConnectionStatesToFalse()
    {
        if (Elements is null)
        {
            return;
        }

        foreach (var element in Elements)
        {
            element.IsConnectedToServer = false;
        }
    }

    private AbstractElement GetServerElement()
    {
        if (Elements is null)
        {
            throw new NullReferenceException("Elements is null!");
        }

        // There will always be a server element, no need to check if we can't find one
        return Elements
            .OfType<AbstractElement>()
            .First(element => element 
                is ServerCornerElement
                or ServerCrossElement
                or ServerLineElement
                or ServerSingleElement);
    }

    private void SetConnectedNeighboursToConnectedToServer(AbstractElement? thisElement)
    {
        if (thisElement is null)
        {
            return;
        }

        if (thisElement.IsConnectedToServer)
        {
            return;
        }

        thisElement.IsConnectedToServer = true;

        if (thisElement.IsConnectedWithLeftElement())
        {
            SetConnectedNeighboursToConnectedToServer(thisElement.LeftElement);
        }

        if (thisElement.IsConnectedWithRightElement())
        {
            SetConnectedNeighboursToConnectedToServer(thisElement.RightElement);
        }

        if (thisElement.IsConnectedWithTopElement())
        {
            SetConnectedNeighboursToConnectedToServer(thisElement.TopElement);
        }

        if (thisElement.IsConnectedWithBottomElement())
        {
            SetConnectedNeighboursToConnectedToServer(thisElement.BottomElement);
        }
    }
}
