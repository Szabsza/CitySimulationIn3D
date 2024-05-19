using Silk.NET.OpenGL;
using Shader = Project.Components.Shader;
using Texture = Project.Components.Texture;

namespace Project.Entities;

public class GlTexturedObject : GlObject
{
    public List<Texture> Textures;
    public float[] Vertices;

    public GlTexturedObject(
        GL gl,
        float[] vertices,
        uint[] indices,
        List<Texture> textures
    ) : base(
        gl,
        vertices,
        indices
    )
    {
        Textures = textures;
        Vertices = vertices;

        Vao.VertexAttributePointer(2, 2, VertexAttribPointerType.Float, 8, 6);
    }

    public override unsafe void Draw()
    {
        Vao.Bind();
        for (var i = 0; i < Textures.Count; i++)
        {
            Textures[i].Bind((TextureUnit)i);
        }

        Shader.Instance.Use();

        Gl.DrawElements(GLEnum.Triangles, (uint)Indices.Length, GLEnum.UnsignedInt, null);
        Gl.BindVertexArray(0);
    }

    public override void Free()
    {
        Ebo.Free();
        Vbo.Free();
        Vao.Free();
        foreach (var texture in Textures)
        {
            texture.Free();
        }
    }
}