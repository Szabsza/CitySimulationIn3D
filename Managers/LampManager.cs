using System.Numerics;
using Project.Components;
using Project.Entities;
using Silk.NET.Maths;

namespace Project.Managers;

public class LampManager
{
    private static LampManager? _instance;
    private HashSet<Vector3D<float>> _lampPositions = new();

    public List<GlLamp> Lamps;

    private LampManager()
    {
        Lamps = new List<GlLamp>();
    }

    public static LampManager Instance => _instance ??= new LampManager();

    public void AddLamp(GlLamp lamp)
    {
        Lamps.Add(lamp);
        _lampPositions.Add(lamp.Position);
    }

    public List<GlLamp> GetLamps()
    {
        return Lamps;
    }

    public void RemoveLampAt(Vector3D<float> position)
    {
        var lampsToRemove = Lamps.Where(lamp => lamp.Position == position).ToList();
        foreach (var lamp in lampsToRemove)
        {
            Lamps.Remove(lamp);
            _lampPositions.Remove(position);
        }
    }
    
    public void Draw()
    {
        for (var i = 0; i < Lamps.Count; i++)
        {
            Lamps[i].Draw();
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var lamp in Lamps)
        {
            lamp.Update(deltaTime);
        }
    }
    
    public void Free()
    {
        foreach (var road in Lamps)
        {
            road.Free();
        }
    }
}