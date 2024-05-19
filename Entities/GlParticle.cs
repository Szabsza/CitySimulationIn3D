using Silk.NET.Maths;

namespace Project.Entities;

public class GlParticle(
    Vector3D<float> position,
    Vector3D<float> velocity,
    Vector3D<float> color,
    float lifetime
)
{
    public Vector3D<float> Position = position;
    public Vector3D<float> Velocity = velocity;
    public Vector3D<float> Color = color;
    public float Lifetime = lifetime;
    public bool Alive = true;

    public void Update(float deltaTime)
    {
        if (!Alive) return;

        Lifetime -= deltaTime;
        if (Lifetime <= 0)
        {
            Alive = false;
            return;
        }

        Position += Velocity * deltaTime;
    }
}