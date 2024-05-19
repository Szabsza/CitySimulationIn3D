using System.Numerics;
using Project.Managers;
using Project.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Project.Entities;

public class RegularCar(GL gl, Vector3D<float> position) : GlCar(gl, position, "car_sedan.obj")
{
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        var regularCar = CollisionDetection.IsCollidingWithRegularCar(Position, Size);
        if (regularCar != null && regularCar != this)
        {
            ParticleSystemManager.Instance.ExplosionParticleSystem.Emit(Position);
            ParticleSystemManager.Instance.ExplosionParticleSystem.Emit(regularCar.Position);

            ReachedTarget = true;
            PlaceSomeWhereOnTheRoads();
            
            regularCar.ReachedTarget = true;
            regularCar.PlaceSomeWhereOnTheRoads();
        }
    }
}