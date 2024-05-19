using Silk.NET.Maths;

namespace Project.Util;

public static class Pathfinding
{
    public static List<Vector3D<float>> FindPath(
        Dictionary<Vector3D<float>, List<Vector3D<float>>> graph,
        Vector3D<float> start,
        Vector3D<float> goal
    )
    {
        var gScore = new Dictionary<Vector3D<float>, float>();
        var fScore = new Dictionary<Vector3D<float>, float>();
        
        var openSet = new SortedSet<Vector3D<float>>(Comparer<Vector3D<float>>.Create((a, b) =>
        {
            var comparison = (gScore[a] + fScore[a]).CompareTo(gScore[b] + fScore[b]);
            return comparison == 0 ? a.GetHashCode().CompareTo(b.GetHashCode()) : comparison;
        }));

        var cameFrom = new Dictionary<Vector3D<float>, Vector3D<float>>();
        
        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            var current = openSet.Min;
            openSet.Remove(current);

            if (current.Equals(goal))
                return ReconstructPath(cameFrom, current);

            foreach (var neighbor in graph[current])
            {
                var tentativeGScore = gScore[current] + Vector3D.Distance(current, neighbor);

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

                    openSet.Add(neighbor);
                }
            }
        }

        return [];
    }

    private static float Heuristic(Vector3D<float> a, Vector3D<float> b)
    {
        return Vector3D.Distance(a, b);
    }
    
    private static List<Vector3D<float>> ReconstructPath(
        Dictionary<Vector3D<float>, Vector3D<float>> cameFrom,
        Vector3D<float> current
    )
    {
        var totalPath = new List<Vector3D<float>> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }

        totalPath.Remove(totalPath[^1]);
        totalPath.Reverse();
        
        return totalPath;
    }
}