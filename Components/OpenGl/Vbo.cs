using Silk.NET.OpenGL;

namespace Project.Components.OpenGl;

public class Vbo(GL gl, Span<float> data, GLEnum staticOrDynamic)
    : BufferObject<float>(gl, data, GLEnum.ArrayBuffer, staticOrDynamic);