using Project.Managers;
using Project.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Type = Project.Util.Type;

namespace Project.Entities;

public class GlRoad
{
    private GlModel _road;
    public Direction FacingDirection;
    public Vector3D<float> Position;
    public Type Type;
    
    public GlRoad(GL gl, Type type, Vector3D<float> position, Direction facingDirection = Direction.South)
    {
        Position = position;
        Type = type;
        FacingDirection = facingDirection;
        
        string objName;
        switch (type)
        {
            case Type.Straight:
                objName = "road_straight.obj";
                break;
            case Type.Corner:
                objName = "road_corner.obj";
                break;
            case Type.Junction:
                objName = "road_junction.obj";
                break;
            case Type.Tsplit:
                objName = "road_tsplit.obj";
                break;
            case Type.Crossing:
                objName = "road_straight_crossing.obj";
                break;
            default:
                objName = "";
                break;
        }

        _road = new GlModel(gl, objName, [Assimp.TextureType.Diffuse]);
    }

    public void Draw()
    {
        var rotationMatrix = FacingDirection switch
        {
            Direction.North => Matrix4X4.CreateRotationY(MathF.PI),
            Direction.South => Matrix4X4.CreateRotationY(MathF.PI * 2),
            Direction.East => Matrix4X4.CreateRotationY(MathF.PI / 2),
            Direction.West => Matrix4X4.CreateRotationY(MathF.PI * 3 / 2),
            _ => throw new ArgumentOutOfRangeException()
        };

        var translationMatrix = Matrix4X4.CreateTranslation(Position);

        _road.Draw(rotationMatrix * translationMatrix);
    }

    public void Free()
    {
        _road.Free();
    }
}