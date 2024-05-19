using Project.Entities;
using Project.Managers;
using Silk.NET.Maths;

namespace Project.Util;

public static class CollisionDetection
{
    private static bool CheckCollision(Vector3D<float> pos1, Vector3D<float> size1, Vector3D<float> pos2, Vector3D<float> size2)
    {
        var halfSize1 = size1 / 2;
        var halfSize2 = size2 / 2;

        return (pos1.X - halfSize1.X < pos2.X + halfSize2.X && pos1.X + halfSize1.X > pos2.X - halfSize2.X &&
                pos1.Z - halfSize1.Z < pos2.Z + halfSize2.Z && pos1.Z + halfSize1.Z > pos2.Z - halfSize2.Z);
    }
    
    public static GlLamp? IsCollidingWithLamps(Vector3D<float> position, Vector3D<float> size)
    {
        foreach (var lamp in LampManager.Instance.GetLamps())
        {
            if (CheckCollision(position, size, lamp.Position, lamp.Size))
            {
                return lamp;
            }
        }
        return null;
    }
    
    public static bool IsCollidingWithBuildings(Vector3D<float> position, Vector3D<float> size)
    {
        foreach (var building in BuildingsManager.Instance.GetBuildings())
        {
            if (CheckCollision(position, size, building.Position, building.Size))
            {
                return true;
            }
        }
        return false;
    }
    
    public static bool IsCollidingWithPlayerCar(Vector3D<float> position, Vector3D<float> size)
    {
        var playerCar = CarManager.Instance.PlayerCar;
        if (CheckCollision(position, size, playerCar.Position, playerCar.Size))
        {
            return true;
        }
      
        return false;
    }
    
    public static RegularCar? IsCollidingWithRegularCar(Vector3D<float> position, Vector3D<float> size)
    {
        foreach (var car in CarManager.Instance.RegularCars)
        {
            if (CheckCollision(position, size, car.Position, car.Size))
            {
                return car;
            }
        }
        return null;
    }
}