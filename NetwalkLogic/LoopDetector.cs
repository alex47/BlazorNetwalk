using NetwalkLogic.Elements;

namespace NetwalkLogic;

public class LoopDetector
{
    public static void DetectLoops(AbstractElement[,]? elements)
    {
        if (elements is null)
        {
            return;
        }

        foreach (var element in elements)
        {
            element.IsInLoop = false;
        }

        var loopElements = new HashSet<AbstractElement>();
        var visited = new HashSet<AbstractElement>();

        foreach (var element in elements)
        {
            if (visited.Contains(element) == false)
            {
                var path = new List<AbstractElement>();
                DetectLoopDFS(element, null, path, visited, loopElements);
            }
        }

        foreach (var element in loopElements)
        {
            element.IsInLoop = true;
        }
    }

    private static bool DetectLoopDFS(AbstractElement element, AbstractElement? parent, List<AbstractElement> path, HashSet<AbstractElement> visited, HashSet<AbstractElement> loopElements)
    {
        if (visited.Contains(element))
        {
            int cycleStartIndex = path.IndexOf(element);

            if (cycleStartIndex != -1)
            {
                var cycle = path.GetRange(cycleStartIndex, path.Count - cycleStartIndex);

                foreach (var cycleElement in cycle)
                {
                    loopElements.Add(cycleElement);
                }

                return true;
            }

            return false;
        }

        visited.Add(element);
        path.Add(element);

        foreach (var neighbor in GetConnectedNeighbors(element, parent))
        {
            DetectLoopDFS(neighbor, element, path, visited, loopElements);
        }

        path.Remove(element);

        return false;
    }

    private static List<AbstractElement> GetConnectedNeighbors(AbstractElement element, AbstractElement? parent)
    {
        var neighbours = new List<AbstractElement>();

        void CheckNeighbour(AbstractElement? neighbour, Func<bool> connectionCheck)
        {
            if (neighbour is not null && neighbour != parent && connectionCheck())
            {
                neighbours.Add(neighbour);
            }
        }

        CheckNeighbour(element.LeftElement, () =>
            element.IsConnectedWithLeftElement() &&
            element.LeftElement?.IsConnectedWithRightElement() == true);

        CheckNeighbour(element.RightElement, () =>
            element.IsConnectedWithRightElement() &&
            element.RightElement?.IsConnectedWithLeftElement() == true);

        CheckNeighbour(element.TopElement, () =>
            element.IsConnectedWithTopElement() &&
            element.TopElement?.IsConnectedWithBottomElement() == true);

        CheckNeighbour(element.BottomElement, () =>
            element.IsConnectedWithBottomElement() &&
            element.BottomElement?.IsConnectedWithTopElement() == true);

        return neighbours;
    }
}
