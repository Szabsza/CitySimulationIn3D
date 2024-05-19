using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = Project.Components.Shader;
using Texture = Project.Components.Texture;

namespace Project.Entities;

public class GlSkybox
{
    private GlTexturedObject _skybox;

    public GlSkybox(GL gl, string texturePath)
    {
        List<Texture> textures = [];
        textures.Add(new Texture(gl, texturePath));

        float[] vertices = [
            // top face
            -0.5f, 0.5f, 0.5f, 0f, -1f, 0f, 1f/4f, 0f/3f,
            0.5f, 0.5f, 0.5f, 0f, -1f, 0f, 2f/4f, 0f/3f,
            0.5f, 0.5f, -0.5f, 0f, -1f, 0f, 2f/4f, 1f/3f,
            -0.5f, 0.5f, -0.5f, 0f, -1f, 0f, 1f/4f, 1f/3f,

            // front face
            -0.5f, 0.5f, 0.5f, 0f, 0f, -1f, 1, 1f/3f,
            -0.5f, -0.5f, 0.5f, 0f, 0f, -1f, 4f/4f, 2f/3f,
            0.5f, -0.5f, 0.5f, 0f, 0f, -1f, 3f/4f, 2f/3f,
            0.5f, 0.5f, 0.5f, 0f, 0f, -1f,  3f/4f, 1f/3f,

            // left face
            -0.5f, 0.5f, 0.5f, 1f, 0f, 0f, 0, 1f/3f,
            -0.5f, 0.5f, -0.5f, 1f, 0f, 0f,1f/4f, 1f/3f,
            -0.5f, -0.5f, -0.5f, 1f, 0f, 0f, 1f/4f, 2f/3f,
            -0.5f, -0.5f, 0.5f, 1f, 0f, 0f, 0f/4f, 2f/3f,

            // bottom face
            -0.5f, -0.5f, 0.5f, 0f, 1f, 0f, 1f/4f, 1f,
            0.5f, -0.5f, 0.5f,0f, 1f, 0f, 2f/4f, 1f,
            0.5f, -0.5f, -0.5f,0f, 1f, 0f, 2f/4f, 2f/3f,
            -0.5f, -0.5f, -0.5f,0f, 1f, 0f, 1f/4f, 2f/3f,

            // back face
            0.5f, 0.5f, -0.5f, 0f, 0f, 1f, 2f/4f, 1f/3f,
            -0.5f, 0.5f, -0.5f, 0f, 0f, 1f, 1f/4f, 1f/3f,
            -0.5f, -0.5f, -0.5f,0f, 0f, 1f, 1f/4f, 2f/3f,
            0.5f, -0.5f, -0.5f,0f, 0f, 1f, 2f/4f, 2f/3f,

            // right face
            0.5f, 0.5f, 0.5f, -1f, 0f, 0f, 3f/4f, 1f/3f,
            0.5f, 0.5f, -0.5f,-1f, 0f, 0f, 2f/4f, 1f/3f,
            0.5f, -0.5f, -0.5f, -1f, 0f, 0f, 2f/4f, 2f/3f,
            0.5f, -0.5f, 0.5f, -1f, 0f, 0f, 3f/4f, 2f/3f,
        ];
        
        var indices = new uint[] {
            0, 2, 1,
            0, 3, 2,

            4, 6, 5,
            4, 7, 6,

            8, 10, 9,
            10, 8, 11,

            12, 13, 14,
            12, 14, 15,

            17, 19, 16,
            17, 18, 19,

            20, 21, 22,
            20, 22, 23
        };
        
        _skybox = new GlTexturedObject(gl, vertices, indices, textures);
    }

    public void Draw(Matrix4X4<float> modelMatrix)
    {
        Shader.Instance.SetUniform("uProjection", Config.Instance.ProjectionMatrix);
        Shader.Instance.SetUniform("uView", Config.Instance.ViewMatrix);
        Shader.Instance.SetUniform("uModel", modelMatrix);
        Shader.Instance.SetUniform("ambientIntensity", Config.Instance.AmbientIntensity);
        _skybox.Draw();
    }

    public void Free()
    {
        _skybox.Free();
    }
}