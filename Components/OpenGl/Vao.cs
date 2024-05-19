using Silk.NET.OpenGL;

namespace Project.Components.OpenGl;

public class Vao(GL gl, Vbo vbo, Ebo ebo) : VertexArrayObject<float, uint>(gl, vbo, ebo);