using BlazorNetwalk.Elements;
using BlazorNetwalk.Elements.BaseElements;
using BlazorNetwalk.Elements.ComputerElements;
using BlazorNetwalk.Elements.PipeElements;
using BlazorNetwalk.Elements.ServerElements;
using System.Xml.Linq;
using static BlazorNetwalk.Elements.AbstractElement;

namespace BlazorNetwalk.GameLogic;

public class GameGenerator
{
    public static AbstractElement[,] CreateNewGame(int rowCount, int columnCount, bool isWrapping)
    {
        var elements = GenerateGame(rowCount, columnCount, isWrapping);
        
        if (isWrapping)
        {
            ConnectElementsWithWrapping(ref elements);
        }
        else
        {
            ConnectElementsWithoutWrapping(ref elements);
        }
        

        RotateElementsRandomly(ref elements);

        return elements;
    }

    private static AbstractElement[,] GenerateGame(int rowCount, int columnCount, bool isWrapping)
    {
        AbstractElement[,] elements = new AbstractElement[rowCount, columnCount];
        var elementsWithPossibleChoices = new List<AbstractElement>();

        const int seed = 1;
        var random = new Random();
        
        int randomServerElementType = random.Next(4);

        AbstractElement serverElement = randomServerElementType switch
        {
            0 => new ServerSingleElement(),
            1 => new ServerLineElement(),
            2 => new ServerCornerElement(),
            3 => new ServerCrossElement(),
            _ => throw new Exception("This should never happen."),
        };

        serverElement.Y = rowCount / 2;
        serverElement.X = columnCount / 2;
        serverElement.Position = random.Next(4);

        elements[serverElement.Y, serverElement.X] = serverElement;

        var serverElementDirections = serverElement.GetConnectionDirections();

        foreach (var direction in serverElementDirections)
        {
            switch (direction)
            {
                case Direction.Left:
                    var newLeftElement = ElementGenerator.CreateElementWithDirections(new List<Direction>() { Direction.Right });
                    SetElementCoordinatesAndAddToArray(ref elements, serverElement, newLeftElement, Direction.Left);
                    elementsWithPossibleChoices.Add(newLeftElement);
                    break;
                case Direction.Right:
                    var newRightElement = ElementGenerator.CreateElementWithDirections(new List<Direction>() { Direction.Left });
                    SetElementCoordinatesAndAddToArray(ref elements, serverElement, newRightElement, Direction.Right);
                    elementsWithPossibleChoices.Add(newRightElement);
                    break;
                case Direction.Top:
                    var newTopElement = ElementGenerator.CreateElementWithDirections(new List<Direction>() { Direction.Bottom });
                    SetElementCoordinatesAndAddToArray(ref elements, serverElement, newTopElement, Direction.Top);
                    elementsWithPossibleChoices.Add(newTopElement);
                    break;
                case Direction.Bottom:
                    var newBottomElement = ElementGenerator.CreateElementWithDirections(new List<Direction>() { Direction.Top });
                    SetElementCoordinatesAndAddToArray(ref elements, serverElement, newBottomElement, Direction.Bottom);
                    elementsWithPossibleChoices.Add(newBottomElement);
                    break;
            }
        }

        while (elementsWithPossibleChoices.Any())
        {
            // 1. Choose a random element from the elementsWithPossibleChoices list
            int randomElementIndex = random.Next(elementsWithPossibleChoices.Count);
            var element = elementsWithPossibleChoices[randomElementIndex];

            // 2. Remove us from the elementsWithPossibleChoices list if we don't have a possible direction
            // If it has at least 3 connected directions
            if (3 <= element.GetConnectionDirections().Count)
            {
                elementsWithPossibleChoices.Remove(element);
                continue;
            }

            // 3. Choose a possible direction it can take next
            //  - a possible direction is a null valued neighbour
            var availableNeighbours = new List<Direction>();

            if (GetNeighbourElementOfDirectionFromArray(ref elements, element, Direction.Left) is null)
            {
                availableNeighbours.Add(Direction.Left);
            }

            if (GetNeighbourElementOfDirectionFromArray(ref elements, element, Direction.Right) is null)
            {
                availableNeighbours.Add(Direction.Right);
            }

            if (GetNeighbourElementOfDirectionFromArray(ref elements, element, Direction.Top) is null)
            {
                availableNeighbours.Add(Direction.Top);
            }

            if (GetNeighbourElementOfDirectionFromArray(ref elements, element, Direction.Bottom) is null)
            {
                availableNeighbours.Add(Direction.Bottom);
            }

            // If all neighbours are elements
            if (availableNeighbours.Count == 0)
            {
                elementsWithPossibleChoices.Remove(element);
                continue;
            }

            int randomDirectionIndex = random.Next(availableNeighbours.Count);
            var nextDirection = availableNeighbours[randomDirectionIndex];

            // 4. Create a new single element there and connect it to us
            // and add the new element to the elementsWithPossibleChoices list
            switch (nextDirection)
            {
                case Direction.Left:
                    var newLeftElement = ElementGenerator.CreateElementWithDirections(new List<Direction>() { Direction.Right });
                    SetElementCoordinatesAndAddToArray(ref elements, element, newLeftElement, Direction.Left);
                    elementsWithPossibleChoices.Add(newLeftElement);
                    break;
                case Direction.Right:
                    var newRightElement = ElementGenerator.CreateElementWithDirections(new List<Direction>() { Direction.Left });
                    SetElementCoordinatesAndAddToArray(ref elements, element, newRightElement, Direction.Right);
                    elementsWithPossibleChoices.Add(newRightElement);
                    break;
                case Direction.Top:
                    var newTopElement = ElementGenerator.CreateElementWithDirections(new List<Direction>() { Direction.Bottom });
                    SetElementCoordinatesAndAddToArray(ref elements, element, newTopElement, Direction.Top);
                    elementsWithPossibleChoices.Add(newTopElement);
                    break;
                case Direction.Bottom:
                    var newBottomElement = ElementGenerator.CreateElementWithDirections(new List<Direction>() { Direction.Top });
                    SetElementCoordinatesAndAddToArray(ref elements, element, newBottomElement, Direction.Bottom);
                    elementsWithPossibleChoices.Add(newBottomElement);
                    break;
            }

            // 5. Transform us to a new element with correct position to accomodate our new form
            var newDirections = element.GetConnectionDirections();
            newDirections.Add(nextDirection);

            var newElement = ElementGenerator.CreateElementWithDirections(newDirections);
            elementsWithPossibleChoices.Remove(element);
            elementsWithPossibleChoices.Add(newElement);

            ReplaceElementInArray(ref elements, element, newElement);
        }

        return elements;
    }

    private static void SetElementCoordinatesAndAddToArray(ref AbstractElement[,] elements, AbstractElement originElement, AbstractElement newElement, Direction direction)
    {
        int rowCount = elements.GetLength(0);
        int columnCount = elements.GetLength(1);

        switch (direction)
        {
            case Direction.Left:
                newElement.X = originElement.X == 0 ? columnCount - 1 : originElement.X - 1;
                newElement.Y = originElement.Y;
                break;
            case Direction.Right:
                newElement.X = originElement.X == columnCount - 1 ? 0 : originElement.X + 1;
                newElement.Y = originElement.Y;
                break;
            case Direction.Top:
                newElement.X = originElement.X;
                newElement.Y = originElement.Y == 0 ? rowCount - 1 : originElement.Y - 1;
                break;
            case Direction.Bottom:
                newElement.X = originElement.X;
                newElement.Y = originElement.Y == rowCount - 1 ? 0 : originElement.Y + 1;
                break;
            default:
                break;
        }

        elements[newElement.Y, newElement.X] = newElement;
    }

    private static AbstractElement GetNeighbourElementOfDirectionFromArray(ref AbstractElement[,] elements, AbstractElement element, Direction direction)
    {
        int rowCount = elements.GetLength(0);
        int columnCount = elements.GetLength(1);

        int x = element.X;
        int y = element.Y;

        switch (direction)
        {
            case Direction.Left:
                return elements[y, (x == 0 ? columnCount - 1 : x - 1)];
            case Direction.Right:
                return elements[y, (x == columnCount - 1 ? 0 : x + 1)];
            case Direction.Top:
                return elements[(y == 0 ? rowCount - 1 : y - 1), x];
            case Direction.Bottom:
                return elements[(y == rowCount - 1 ? 0 : y + 1), x];
            default:
                return null;
        }
    }

    private static void ConnectElementsWithWrapping(ref AbstractElement[,] elements)
    {
        int rows = elements.GetLength(0);
        int columns = elements.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                elements[i, j].LeftElement = elements[i, (j == 0 ? columns - 1 : j - 1)];
                elements[i, j].RightElement = elements[i, (j == columns - 1 ? 0 : j + 1)];
                elements[i, j].TopElement = elements[(i == 0 ? rows - 1 : i - 1), j];
                elements[i, j].BottomElement = elements[(i == rows - 1 ? 0 : i + 1), j];
            }
        }
    }

    private static void ConnectElementsWithoutWrapping(ref AbstractElement[,] elements)
    {
        int rows = elements.GetLength(0);
        int columns = elements.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                elements[i, j].LeftElement = j == 0 ? null : elements[i, j - 1];
                elements[i, j].RightElement = j == columns - 1 ? null : elements[i, j + 1];
                elements[i, j].TopElement = i == 0 ? null : elements[i - 1, j];
                elements[i, j].BottomElement = i == rows - 1 ? null : elements[i + 1, j];
            }
        }
    }

    private static void ReplaceElementInArray(ref AbstractElement[,] elements, AbstractElement oldElement, AbstractElement newElement)
    {
        newElement.X = oldElement.X;
        newElement.Y = oldElement.Y;

        elements[newElement.Y, newElement.X] = newElement;

        return;








        newElement.LeftElement = oldElement.LeftElement;
        newElement.RightElement = oldElement.RightElement;
        newElement.TopElement = oldElement.TopElement;
        newElement.BottomElement = oldElement.BottomElement;

        if (oldElement.LeftElement is not null)
        {
            oldElement.LeftElement.RightElement = newElement;
        }

        if (oldElement.RightElement is not null)
        {
            oldElement.RightElement.LeftElement = newElement;
        }

        if (oldElement.TopElement is not null)
        {
            oldElement.TopElement.BottomElement = newElement;
        }

        if (oldElement.BottomElement is not null)
        {
            oldElement.BottomElement.TopElement = newElement;
        }
    }

    private static void RotateElementsRandomly(ref AbstractElement[,] elements)
    {
        int rows = elements.GetLength(0);
        int columns = elements.GetLength(1);

        var random = new Random();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int rotationCount = random.Next(1, 5);

                for (int k = 0; k < rotationCount; k++)
                {
                    elements[i, j].Rotate();
                }

            }
        }
    }
}
