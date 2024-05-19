using System.Numerics;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Project.Components;

public class Shader
{
    private static Shader? _instance;
    public static Shader Instance => _instance ??= new Shader();
    
    private uint _program;
    private GL _gl = null!;
    
    private Shader() {}
    
    public void Init(GL gl, string vertexShaderPath, string fragmentShaderPath)
    {
        _gl = gl;
        var vertexShader = LoadShader(ShaderType.VertexShader, vertexShaderPath);
        var fragmentShader = LoadShader(ShaderType.FragmentShader, fragmentShaderPath);

        _program = _gl.CreateProgram();
        _gl.AttachShader(_program, vertexShader);
        _gl.AttachShader(_program, fragmentShader);
        _gl.LinkProgram(_program);

        _gl.GetProgram(_program, GLEnum.LinkStatus, out var status);
        if (status == 0)
        {
            throw new Exception($"Program failed to link with error: {_gl.GetProgramInfoLog(_program)}");
        }
        
        _gl.DetachShader(_program, vertexShader);
        _gl.DetachShader(_program, fragmentShader);

        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);
    }
    
    private uint LoadShader(ShaderType type, string path)
    {
        var src = File.ReadAllText(path);

        var shader = _gl.CreateShader(type);
        _gl.ShaderSource(shader, src);
        _gl.CompileShader(shader);

        var infoLog = _gl.GetShaderInfoLog(shader);
        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
        }

        return shader;
    }

    public void Use()
    {
        _gl.UseProgram(_program);
    }

    public void Free()
    {
        _gl.DeleteProgram(_program);
    }

    public void SetUniform(string name, int value)
    {
        var location = _gl.GetUniformLocation(_program, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }

        Use();
        _gl.Uniform1(location, value);
    }
    
    public void SetUniform(string name, bool value)
    {
        var location = _gl.GetUniformLocation(_program, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }

        Use();

        _gl.Uniform1(location, value ? 1 : 0);
    }

    public void SetUniform(string name, Vector2 value)
    {
        var location = _gl.GetUniformLocation(_program, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }

        Use();
        _gl.Uniform2(location, value);
    }
    
    public void SetUniform(string name, Vector3 value)
    {
        var location = _gl.GetUniformLocation(_program, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }

        Use();
        _gl.Uniform3(location, value);
    }

    public unsafe void SetUniform(string name, Matrix4X4<float> value)
    {
        var location = _gl.GetUniformLocation(_program, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }

        Use();
        _gl.UniformMatrix4(location, 1, false, (float*)&value);
    }
    
    public unsafe void SetUniform(string name, Matrix3X3<float> value)
    {
        var location = _gl.GetUniformLocation(_program, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }

        Use();
        _gl.UniformMatrix4(location, 1, false, (float*)&value);
    }

    public void SetUniform(string name, float value)
    {
        var location = _gl.GetUniformLocation(_program, name);
        if (location == -1)
        {
            throw new Exception($"{name} uniform not found on shader.");
        }

        Use();
        _gl.Uniform1(location, value);
    }
}