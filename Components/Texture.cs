using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace Project.Components;

public class Texture
{
    private uint _program;
    private GL _gl;
    
    public Texture(GL gl, string path)
    {
        _gl = gl;
        if (!File.Exists(path))
            throw new FileNotFoundException($"The file {path} was not found.");
        
        var fileData = File.ReadAllBytes(path);

        
        var imageResult = ImageResult.FromMemory(fileData, ColorComponents.RedGreenBlueAlpha);
        var data = (ReadOnlySpan<byte>)imageResult.Data.AsSpan();
        
        Load(gl, data, (uint)imageResult.Width, (uint)imageResult.Height);
    }
    
    private void Load(GL gl, ReadOnlySpan<byte> data, uint width, uint height)
    {
        _gl = gl;

        _program = _gl.GenTexture();
        Bind();
        
        _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

        _gl.GenerateMipmap(TextureTarget.Texture2D);
    }

    public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(textureSlot);
        _gl.BindTexture(TextureTarget.Texture2D, _program);
    }
    
    public void Free()
    {
        _gl.DeleteTexture(_program);
    }
}