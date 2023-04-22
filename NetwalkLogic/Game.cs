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

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    bool didFixElement = Elements[i, j].TryFixing();

                    if (didFixElement)
                    {
                        didFixAnyElement = true;
                    }
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

        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                // Game is won when every computer is connected to the server
                if (Elements[i, j] is Elements.ComputerElements.ComputerSingleElement && Elements[i, j].IsConnectedToServer == false)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void SetServerConnectionStatesToFalse()
    {
        if (Elements is null)
        {
            return;
        }

        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                Elements[i, j].IsConnectedToServer = false;
            }
        }
    }

    private AbstractElement GetServerElement()
    {
        if (Elements is null)
        {
            throw new NullReferenceException("Elements is null!");
        }

        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                if (Elements[i, j] is ServerCornerElement or ServerCrossElement or ServerLineElement or ServerSingleElement)
                {
                    return Elements[i, j];
                }
            }
        }

        throw new Exception("Server element was not found!");
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
