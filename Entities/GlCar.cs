using Assimp;
using Project.Managers;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Vector3D = Silk.NET.Maths.Vector3D;

namespace Project.Entities;

public abstract class GlCar(GL gl, Vector3D<float> position, string carModelPath)
{
    private GL _gl = gl;
    private Random _random = new();
    private GlModel _carBase = new(gl, carModelPath, [TextureType.Diffuse]);
    private GlModel _wheelFrontRight = new(gl, "car_sedan_wheel_front_right.obj", [TextureType.Diffuse]);
    private GlModel _wheelFrontLeft = new(gl, "car_sedan_wheel_front_left.obj", [TextureType.Diffuse]);
    private GlModel _wheelRearRight = new(gl, "car_sedan_wheel_rear_right.obj", [TextureType.Diffuse]);
    private GlModel _wheelRearLeft = new(gl, "car_sedan_wheel_rear_left.obj", [TextureType.Diffuse]);

    protected bool _isMovingForward;
    protected bool _isMovingBackward;
    protected bool _isMovingLeft;
    protected bool _isMovingRight;
    protected bool _shouldRotateWheelsToDefault;
    
    public bool ReachedTarget = true;
    public bool AutopilotMode = false;
    protected const float Speed = 3.0f;

    protected float _wheelOrientation;
    protected float _wheelRotationForward;

    // values from blender
    private static readonly Vector3D<float> OffsetCenterRightFrontWheel = new(0.17f, -0.011f, -0.245f);
    private static readonly Vector3D<float> OffsetCenterFrontLeftWheel = new(-0.17f, -0.011f, -0.245f);
    private static readonly Vector3D<float> OffsetOriginalRightFrontWheel = new(-0.17f, 0.011f, 0.245f);
    private static readonly Vector3D<float> OffsetOriginalFrontLeftWheel = new(0.17f, 0.011f, 0.245f);
    private static readonly Vector3D<float> OffsetCenterRearWheel = new(0, -0.011f, 0.256f);
    private static readonly Vector3D<float> OffsetOriginalRearWheel = new(0, 0.011f, -0.256f);
    
    public float Orientation;
    public Vector3D<float> Position = position;
    public Vector3D<float> Size = new(0.6f, 0.6f, 0.6f);
    public Vector3D<float> Target = new(0, 0, 0);
    public Vector3D<float> LastTarget = new (-200, -200, -200);
    
    public void PlaceSomeWhereOnTheRoads()
    {
        var random = new Random();
        var index = random.Next(0, RoadsManager.Instance.RoadGraph.Count);
        Position = RoadsManager.Instance.RoadGraph.ElementAt(index).Key;
    }
    
    public void StartMovingForward()
    {
        _isMovingForward = true;
        _isMovingBackward = false;
    }

    public void StopMovingForward()
    {
        _isMovingForward = false;
        _isMovingBackward = false;
    }

    public void StartMovingBackward()
    {
        _isMovingForward = false;
        _isMovingBackward = true;
    }

    public void StopMovingBackward()
    {
        _isMovingForward = false;
        _isMovingBackward = false;
    }

    public void StartMovingLeft()
    {
        _shouldRotateWheelsToDefault = false;
        _isMovingLeft = true;
        _isMovingRight = false;
    }

    public void StopMovingLeft()
    {
        _shouldRotateWheelsToDefault = true;
        _isMovingLeft = false;
        _isMovingRight = false;
    }

    public void StartMovingRight()
    {
        _shouldRotateWheelsToDefault = false;
        _isMovingLeft = false;
        _isMovingRight = true;
    }

    public void StopMovingRight()
    {
        _shouldRotateWheelsToDefault = true;
        _isMovingLeft = false;
        _isMovingRight = false;
    }
    
    public virtual void Draw()
    {
        var modelMatrix = Matrix4X4.CreateRotationY(Orientation) * Matrix4X4.CreateTranslation(Position);
        _carBase.Draw(modelMatrix);

        var forwardRot = Matrix4X4.CreateRotationX(_wheelRotationForward);
        var sideRot = Matrix4X4.CreateRotationY(_wheelOrientation);

        var translateToCenterFrontRightWheel = Matrix4X4.CreateTranslation(OffsetCenterRightFrontWheel);
        var translateToCenterFrontLeftWheel = Matrix4X4.CreateTranslation(OffsetCenterFrontLeftWheel);

        var translateToOriginalFrontRightWheel = Matrix4X4.CreateTranslation(OffsetOriginalRightFrontWheel);
        var translateToOriginalFrontLeftWheel = Matrix4X4.CreateTranslation(OffsetOriginalFrontLeftWheel);

        var translateToCenterRearWheel = Matrix4X4.CreateTranslation(OffsetCenterRearWheel);
        var translateToOriginalRearWheel = Matrix4X4.CreateTranslation(OffsetOriginalRearWheel);


        _wheelFrontRight.Draw(translateToCenterFrontRightWheel * forwardRot * sideRot *
                              translateToOriginalFrontRightWheel * modelMatrix);
        _wheelFrontLeft.Draw(translateToCenterFrontLeftWheel * forwardRot * sideRot *
                             translateToOriginalFrontLeftWheel * modelMatrix);
        _wheelRearRight.Draw(translateToCenterRearWheel * forwardRot * translateToOriginalRearWheel * modelMatrix);
        _wheelRearLeft.Draw(translateToCenterRearWheel * forwardRot * translateToOriginalRearWheel * modelMatrix);
    }

    public void ToggleAutopilotMode()
    {
        AutopilotMode = !AutopilotMode;
    }
    
    private void Autopilot(float deltaTime)
    {
        if (ReachedTarget)
        {
            var neighbourPositions = RoadsManager.Instance.RoadGraph[Position];
            Vector3D<float> newTarget;
            
            do
            {
                newTarget = neighbourPositions[_random.Next(neighbourPositions.Count)];
            } while (newTarget == LastTarget);

            LastTarget = Target;
            Target = newTarget;
            ReachedTarget = false;
            
            var direction = Target - Position;
            if (MathF.Abs(direction.X) > MathF.Abs(direction.Z))
            {
                Orientation = direction.X > 0 ? MathF.PI / 2 : MathF.PI * 3 / 2;
            }
            else
            {
                Orientation = direction.Z > 0 ? 2 * MathF.PI : MathF.PI;
            }
        }
        else
        {
            var direction = Vector3D.Normalize(Target - Position);
            Position += direction * deltaTime * Speed;
            
            if (Vector3D.Distance(Position, Target) < deltaTime * Speed)
            {
                Position = Target;
                ReachedTarget = true;
            }
        }
    }
    
    public virtual void Update(float deltaTime)
    {
        if (AutopilotMode)
        {
            Autopilot(deltaTime);
            return;
        }
    }
    
    public void Free()
    {
        _wheelFrontRight.Free();
        _wheelFrontLeft.Free();
        _wheelRearLeft.Free();
        _wheelRearRight.Free();
        _carBase.Free();
    }
}