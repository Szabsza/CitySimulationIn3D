using System.Numerics;
using Project.Components.OpenGl;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = Project.Components.Shader;

namespace Project.Entities;

public abstract class GlParticleSystem
{
    private GL _gl;
    
    public Vao Vao;
    public Ebo Ebo;
    public Vbo Vbo;
    
    public List<GlParticle> Particles;
    
    public abstract void Emit(Vector3D<float> position);
    
    public GlParticleSystem(GL gl)
    {
        _gl = gl;
        
        var vertices = new float[]
        {
            0f, 0f, 0f
        };

        uint[] indices = [0]; 
        
        Ebo = new Ebo(gl, indices);
        Vbo = new Vbo(gl, vertices, GLEnum.StaticDraw);

        Vao = new Vao(gl, Vbo, Ebo);
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 3, 0);

        Particles = new List<GlParticle>();
    }
    
    public void Update(float deltaTime)
    {
        foreach (var particle in Particles)
        {
            particle.Update(deltaTime);
        }
        Particles.RemoveAll(p => !p.Alive);
    }
    
    public unsafe void Draw()
    {
        Shader.Instance.Use();
        Vao.Bind();
    
        Shader.Instance.SetUniform("isParticle", true);
        _gl.PointSize(10f);
        foreach (var particle in Particles)
        {
            if (!particle.Alive) continue;
            
            var modelMatrix = Matrix4X4.CreateTranslation(particle.Position);
            Shader.Instance.SetUniform("uModel", modelMatrix);
            Shader.Instance.SetUniform("particleColor",(Vector3) particle.Color);

            _gl.DrawElements(GLEnum.Points, 1, GLEnum.UnsignedInt, null);
        }
        Shader.Instance.SetUniform("isParticle", false);

        _gl.BindVertexArray(0);
    }

    public void Free()
    {
        Vbo.Free();
        Ebo.Free();
        Vao.Free();
    }
}