using Silk.NET.OpenGL;

namespace Project.Components.OpenGl;

public class BufferObject<TDataType> where TDataType : unmanaged
{
    private readonly uint _program;
    private readonly GLEnum _bufferType;
    private readonly GL _gl;

    public unsafe BufferObject(GL gl, Span<TDataType> data, GLEnum bufferType, GLEnum staticOrDynamic)
    {
        _gl = gl;
        _bufferType = bufferType;

        _program = _gl.GenBuffer();
        Bind();

        _gl.BufferData(bufferType, (nuint) (data.Length * sizeof(TDataType)), (ReadOnlySpan<TDataType>)data.ToArray().AsSpan(), staticOrDynamic);
        
    }

    public void Bind()
    {
        _gl.BindBuffer(_bufferType, _program);
    }

    public void Free()
    {
        _gl.DeleteBuffer(_program);
    }
}