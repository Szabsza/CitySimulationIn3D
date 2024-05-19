using Project.Entities;
using Silk.NET.Maths;

namespace Project.Managers;

public class RoadsManager
{
    private static RoadsManager? _instance;
    private List<GlRoad> _roads;
    
    public readonly Dictionary<Vector3D<float>, List<Vector3D<float>>> RoadGraph;

    private RoadsManager()
    {
        _roads = new List<GlRoad>();
        RoadGraph = new Dictionary<Vector3D<float>, List<Vector3D<float>>>();
    }

    public static RoadsManager Instance => _instance ??= new RoadsManager();

    public void AddRoad(GlRoad road)
    {
        _roads.Add(road);
    }

    public void Draw()
    {
        foreach (var road in _roads)
        {
            road.Draw();
        }
    }

    public void Free()
    {
        foreach (var road in _roads)
        {
            road.Free();
        }
    }
}