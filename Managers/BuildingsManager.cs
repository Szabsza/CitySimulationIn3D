using Project.Entities;
using Silk.NET.Maths;

namespace Project.Managers;

public class BuildingsManager
{
    private static BuildingsManager? _instance;
    private List<GlBuilding> _buildings;

    private BuildingsManager()
    {
        _buildings = new List<GlBuilding>();
    }

    public static BuildingsManager Instance => _instance ??= new BuildingsManager();

    public void AddBuilding(GlBuilding building)
    {
        _buildings.Add(building);
    }

    public List<GlBuilding> GetBuildings()
    {
        return _buildings;
    }

    public void Draw()
    {
        foreach (var building in _buildings)
        {
            building.Draw();
        }
    }

    public void Free()
    {
        foreach (var building in _buildings)
        {
            building.Free();
        }
    }
}