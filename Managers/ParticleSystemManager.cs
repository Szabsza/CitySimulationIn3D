using Project.Entities;
using Silk.NET.OpenGL;

namespace Project.Managers;

public class ParticleSystemManager
{
    private static ParticleSystemManager? _instance;
    public GlExplosion ExplosionParticleSystem;

    private ParticleSystemManager() {}

    public static ParticleSystemManager Instance => _instance ??= new ParticleSystemManager();

    public void Init(GL gl)
    {
        ExplosionParticleSystem = new GlExplosion(gl);
    }
    
    public void Draw()
    {   
        ExplosionParticleSystem.Draw();
    }

    public void Update(float deltaTime)
    {   
        ExplosionParticleSystem.Update(deltaTime);
    }
    
    public void Free()
    {
        ExplosionParticleSystem.Free();
    }
}