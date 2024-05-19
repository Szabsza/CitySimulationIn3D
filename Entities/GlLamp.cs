using System.Numerics;
using Project.Managers;
using Project.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = Project.Components.Shader;
using Type = Project.Util.Type;

namespace Project.Entities;

public class GlLamp
{
    private GlModel _lamp;
    public Direction Direction;
    public Type Type;

    public Vector3D<float> Position;
    public Vector3D<float> Size;
    
    public Vector3D<float> LightPosition;
    public Vector3D<float> LightDirection;

    private bool _isFalling;
    private Vector3D<float> _fallAxis;
    private float _fallAngle = 0f;
    private float _fallSpeed = 1.0f;
    
    public Vector3D<float> CalculateLightPosition()
    {
        var position = Direction switch
        {
            Direction.North => new Vector3D<float>(0.5f, 0.9f, 0),
            Direction.South => new Vector3D<float>(-0.5f, 0.9f, 0),
            Direction.East => new Vector3D<float>(0, 0.9f, 0.5f),
            Direction.West => new Vector3D<float>(0, 0.9f, -0.5f)
        };
        return position;
    }
    
    public GlLamp(GL gl, Type type, Vector3D<float> position, Direction facingDirection = Direction.South)
    {
        Position = position;
        Direction = facingDirection;
        Type = type;
        LightDirection = new Vector3D<float>(0, -1, 0);
        LightPosition = position + CalculateLightPosition();
        Size = new Vector3D<float>(0.1f, 1f, 0.1f);
        _isFalling = false;

        var objName = type switch
        {
            Type.TrafficLight => "trafficlight_C.obj",
            Type.StreetLight => "streetlight.obj",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        
        _lamp = new GlModel(gl, objName, [Assimp.TextureType.Diffuse]);    
    }

    public void Fall(float orientation)
    {
        if (!_isFalling)
        {
            _isFalling = true;
            _fallAxis = new Vector3D<float>(MathF.Sin(orientation), 0, MathF.Cos(orientation));
        }
    }

    public void Update(float deltaTime)
    {
        if (_isFalling)
        {
            _fallAngle += _fallSpeed * deltaTime;
            if (_fallAngle >= MathF.PI / 3)
            {
                _fallAngle = MathF.PI / 3;
            }
        }
    }
    
    public void Draw()
    {
        var lamps = LampManager.Instance.Lamps;
        var lampIndex = lamps.IndexOf(this);
        
        Shader.Instance.SetUniform($"streetlights[{lampIndex}].position", (Vector3)lamps[lampIndex].LightPosition);
        Shader.Instance.SetUniform($"streetlights[{lampIndex}].direction", (Vector3)lamps[lampIndex].LightDirection);
        Shader.Instance.SetUniform($"streetlights[{lampIndex}].cutOff", MathF.Cos(MathF.PI / 3));
        
        var rotationMatrix = Direction switch
        {
            Direction.North => Matrix4X4.CreateRotationY(MathF.PI),
            Direction.South => Matrix4X4.CreateRotationY(MathF.PI * 2),
            Direction.East => Matrix4X4.CreateRotationY(MathF.PI / 2),
            Direction.West => Matrix4X4.CreateRotationY(MathF.PI * 3 / 2),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var translationmatrix = Matrix4X4.CreateTranslation(Position);
        
        var fallRotation = Matrix4X4<float>.Identity;
        if (_isFalling)
        {
            fallRotation = Matrix4X4.CreateFromAxisAngle(_fallAxis, _fallAngle);
        }

        var modelMatrix = rotationMatrix * fallRotation * translationmatrix;
        
        _lamp.Draw(modelMatrix);
    }
    
    public void Free()
    {
        _lamp.Free();
    }
}