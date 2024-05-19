using Silk.NET.OpenGL;

namespace Project.Components.OpenGl;

public class Ebo(GL gl, Span<uint> data) : BufferObject<uint>(gl, data, GLEnum.ElementArrayBuffer, GLEnum.StaticDraw);