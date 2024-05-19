using Silk.NET.OpenGL;

namespace Project.Components.OpenGl;

public class VertexArrayObject<TVertexType, TIndexType> where TVertexType : unmanaged where TIndexType : unmanaged
{
    private uint _vao;
    private Ebo _ebo;
    private Vbo _vbo;
    private GL _gl;

    protected VertexArrayObject(GL gl, Vbo vbo, Ebo ebo)
    {
        _gl = gl;
        _ebo = ebo;
        _vbo = vbo;
        _vao = _gl.GenVertexArray();
        Bind();
    }

    public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, uint vertexSize,
        int offSet)
    {
        _gl.VertexAttribPointer(index, count, type, false, vertexSize * (uint)sizeof(TVertexType),
            (void*)(offSet * sizeof(TVertexType)));
        _gl.EnableVertexAttribArray(index);
    }

    public void Bind()
    {
        _gl.BindVertexArray(_vao);
        _vbo.Bind();
        _ebo.Bind();
    }

    public void Free()
    {
        _gl.DeleteVertexArray(_vao);
    }
}