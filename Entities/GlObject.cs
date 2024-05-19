using System.Numerics;
using Project.Components.OpenGl;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = Project.Components.Shader;

namespace Project.Entities;

public class GlObject
{
    public GL Gl;
    public Vao Vao;
    public Ebo Ebo;
    public Vbo Vbo;
    public uint[] Indices;

    public GlObject(
        GL gl,
        float[] vertices,
        uint[] indices
    )
    {
        Gl = gl;
        Indices = indices;

        Ebo = new Ebo(gl, indices);
        Vbo = new Vbo(gl, vertices, GLEnum.StaticDraw);

        Vao = new Vao(gl, Vbo, Ebo);
        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 8, 0);
        Vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, 8, 3);
    }

    public virtual unsafe void Draw()
    {
        Vao.Bind();
        Shader.Instance.Use();
        Gl.DrawElements(GLEnum.Triangles, (uint)Indices.Length, GLEnum.UnsignedInt, null);
        Gl.BindVertexArray(0);
    }

    public virtual void Free()
    {
        Vbo.Free();
        Ebo.Free();
        Vao.Free();
    }
}