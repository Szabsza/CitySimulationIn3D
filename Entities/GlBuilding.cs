using Project.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Type = Project.Util.Type;

namespace Project.Entities;

public class GlBuilding
{
    private readonly GlModel _building;
    private readonly Direction _facingDirection;
    
    public Vector3D<float> Position;
    public Vector3D<float> Size;
    
    public GlBuilding(GL gl, Type type, Vector3D<float> position, Direction facingDirection = Direction.South)
    {
        Position = position;
        _facingDirection = facingDirection;
    
        Size = new Vector3D<float>(1.6f, 1.6f, 1.6f);
        
        var objName = type switch
        {
            Type.A => "building_A.obj",
            Type.B => "building_B.obj",
            Type.C => "building_C.obj",
            Type.D => "building_D.obj",
            Type.E => "building_E.obj",
            Type.F => "building_F.obj",
            Type.G => "building_G.obj",
            Type.H => "building_H.obj",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        _building = new GlModel(gl, objName, [Assimp.TextureType.Diffuse]);
    }

    public void Draw()
    {
        var rotationMatrix = _facingDirection switch
        {
            Direction.North => Matrix4X4.CreateRotationY(MathF.PI),
            Direction.South => Matrix4X4.CreateRotationY(MathF.PI * 2),
            Direction.East => Matrix4X4.CreateRotationY(MathF.PI / 2),
            Direction.West => Matrix4X4.CreateRotationY(MathF.PI * 3 / 2),
            _ => throw new ArgumentOutOfRangeException()
        };

        var translationMatrix = Matrix4X4.CreateTranslation(Position);

        _building.Draw( rotationMatrix * translationMatrix);
    }

    public void Free()
    {
        _building.Free();
    }
}