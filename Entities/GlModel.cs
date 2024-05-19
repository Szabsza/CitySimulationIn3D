using Assimp;
using Project.Components.Obj;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = Project.Components.Shader;
using Texture = Project.Components.Texture;

namespace Project.Entities;

public class GlModel
{
    private GL _gl;
    public Scene Scene;
    public List<Mesh> Meshes;
    public List<GlTexturedObject> Objects;

    public GlModel(GL gl, string objPath, TextureType[] texturesToLoad)
    {
        _gl = gl;

        Meshes = new List<Mesh>();
        Objects = new List<GlTexturedObject>();

        const PostProcessSteps postProcessFlags = PostProcessSteps.Triangulate |
                                                  PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs |
                                                  PostProcessSteps.CalculateTangentSpace;

        var assimpContext = new AssimpContext();
        Scene = assimpContext.ImportFile(objPath, postProcessFlags);
        ProcessNode(Scene.RootNode, Scene, texturesToLoad);
    }

    private void ProcessNode(Node node, Scene scene, TextureType[] texturesToLoad)
    {
        for (var i = 0; i < node.MeshCount; i++)
        {
            var mesh = scene.Meshes[i];
            Meshes.Add(mesh);
            var obj = ProcessMesh(mesh, scene, texturesToLoad);
            Objects.Add(obj);
        }

        for (var i = 0; i < node.ChildCount; i++)
        {
            ProcessNode(node.Children[i], scene, texturesToLoad);
        }
    }

    private GlTexturedObject ProcessMesh(Mesh mesh, Scene scene, TextureType[] texturesToLoad)
    {
        var vertices = new List<Vertex>();
        var vertexNormals = new List<VertexNormal>();
        var vertexTextures = new List<VertexTexture>();

        var indices = new List<uint>();
        var textures = new List<Texture>();

        for (var i = 0; i < mesh.Vertices.Count; i++)
        {
            var v = new Vertex(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z);
            var vn = new VertexNormal(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z);
            var vt = new VertexTexture(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y);

            vertices.Add(v);
            vertexNormals.Add(vn);
            vertexTextures.Add(vt);
        }

        foreach (var face in mesh.Faces)
        {
            for (var j = 0; j < face.IndexCount; j++)
            {
                indices.Add((uint)face.Indices[j]);
            }
        }

        var materialIndex = scene.Materials[mesh.MaterialIndex];
        foreach (var item in texturesToLoad)
        {
            var loadedTextures = LoadTextures(materialIndex, item);
            textures.AddRange(loadedTextures);
        }

        return new GlTexturedObject(_gl, ConvertToRawVertices(vertices, vertexNormals, vertexTextures),
            indices.ToArray(), textures);
    }

    private float[] ConvertToRawVertices(
        List<Vertex> vertices,
        List<VertexNormal> vertexNormals,
        List<VertexTexture> vertexTextures
    )
    {
        var rawVertices = new float[vertices.Count * 8];
        var index = 0;
        for (var i = 0; i < vertices.Count; ++i)
        {
            rawVertices[index++] = vertices[i].X;
            rawVertices[index++] = vertices[i].Y;
            rawVertices[index++] = vertices[i].Z;
            //
            rawVertices[index++] = vertexNormals[i].X;
            rawVertices[index++] = vertexNormals[i].Y;
            rawVertices[index++] = vertexNormals[i].Z;
            //
            rawVertices[index++] = vertexTextures[i].U;
            rawVertices[index++] = vertexTextures[i].V;
        }

        return rawVertices;
    }

    private Texture ReadTexture(string path)
    {
        return new Texture(_gl, path);
    }

    private List<Texture> LoadTextures(Material material, TextureType type)
    {
        List<Texture> loadedTextures = [];

        for (var i = 0; i < material.GetMaterialTextureCount(type); i++)
        {
            material.GetMaterialTexture(type, i, out var texture);
            loadedTextures.Add(ReadTexture(texture.FilePath));
        }

        return loadedTextures;
    }

    public virtual void Draw(Matrix4X4<float> modelMatrix)
    {
        foreach (var obj in Objects)
        {
            Shader.Instance.SetUniform("uProjection", Config.Instance.ProjectionMatrix);
            Shader.Instance.SetUniform("uView", Config.Instance.ViewMatrix);
            Shader.Instance.SetUniform("uModel", modelMatrix);
            Shader.Instance.SetUniform("ambientIntensity", Config.Instance.AmbientIntensity);

            obj.Draw();
        }
    }

    public void Free()
    {
        foreach (var obj in Objects)
        {
            obj.Free();
        }
    }
}