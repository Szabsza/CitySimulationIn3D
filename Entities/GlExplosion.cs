using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Project.Entities;

public class GlExplosion(GL gl) : GlParticleSystem(gl)
{
    private readonly List<Vector3D<float>> _colors = [
        new Vector3D<float>(0.79f, 0.20f, 0.24f),
        new Vector3D<float>(0.97f, 0.71f, 0.30f),
        new Vector3D<float>(0.92f, 0.38f, 0.25f),
        new Vector3D<float>(0.33f, 0.23f, 0.24f),
        new Vector3D<float>(0.41f, 0.29f, 0.34f),
    ];

    private readonly Random _random = new();
    private const int ParticlesCount = 1000;

    public override void Emit(Vector3D<float> position)
    {
        var goldenR = (1 + MathF.Sqrt(5)) / 2;
        var angleStep = 2 * MathF.PI * goldenR;
        
        for (var i = 0; i < ParticlesCount; i++)
        {
            var t = (float) i / ParticlesCount;
            var inclination = MathF.Acos(1 - 2 * t);
            var angle = angleStep * i;  
            
            var speed = _random.NextDouble();
            
            var velocity = new Vector3D<float>(
                MathF.Sin(inclination) * MathF.Cos(angle),
                MathF.Sin(inclination) * MathF.Sin(angle),
                MathF.Cos(inclination)
            ) * (float) speed;
            
            var color = _random.Next(_colors.Count);
            Particles.Add(new GlParticle(position, velocity, _colors[color], 3.0f));
        }
    }
}