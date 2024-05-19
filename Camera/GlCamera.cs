using Silk.NET.Maths;

namespace Project.Camera;

public class GlCamera
{
    private static GlCamera? _instance;
    
    public static GlCamera Instance => _instance ??= new GlCamera();
    
    private const int DefaultDistanceToOrigin = 30;
    private const double DefaultAngleToZyPlane = 0;
    private const double DefaultAngleToZxPlane = MathF.PI / 4;
    
    private double _angleToZyPlane;
    private double _angleToZxPlane; 
    
    private double _distanceToOrigin;
    private float _distanceFromTarget;
    
    private const double DistanceScaleFactor = 1.1;
    private const double AngleChangeStepSize = Math.PI / 180 * 5;
    
    private Vector3D<float> _followOffset;
    private Vector3D<float> _followOffsetThirdPerson;
    private Vector3D<float> _followOffsetTopDown;
    public Vector3D<float> TargetPosition;
    public float TargetAngle;
    
    
    public bool IsFollowing;
    
    public Vector3D<float> Position { get; private set; }

    public Vector3D<float> UpVector => Vector3D.Normalize(GetPointFromAngles(_distanceToOrigin, _angleToZyPlane, _angleToZxPlane + Math.PI / 2));

    public Vector3D<float> Target { get; private set; } = Vector3D<float>.Zero;

    public Vector3D<float> Front { get; private set; }

    private GlCamera()
    {
        IsFollowing = false;
        
        SetToDefault();
        
        Position = GetPointFromAngles(_distanceToOrigin, _angleToZyPlane, _angleToZxPlane);
        
        _followOffsetThirdPerson = new Vector3D<float>(0, 1, 4);
        _followOffsetTopDown = new Vector3D<float>(0, 4, 0);

        Front = GetFrontVector(_angleToZyPlane, _angleToZxPlane);
        
        UpdatePosition();
    }

    public void SetToDefault()
    {
        _angleToZyPlane = DefaultAngleToZyPlane;
        _angleToZxPlane = DefaultAngleToZxPlane;
        _distanceToOrigin = DefaultDistanceToOrigin;
    }
    
    public void ToggleFollowMode(Vector3D<float> targetPosition)
    {
        SetToDefault();
        
        IsFollowing = !IsFollowing;
        if (IsFollowing)
        {
            _angleToZxPlane = 0;
            _angleToZyPlane = 0;
        }
        
        TargetPosition = targetPosition;
        
        UpdatePosition();
    }
    
    public void Update(float deltaTime, Vector3D<float> targetPosition, float targetAngle)
    {
        if (!IsFollowing)
        {
            Target = Vector3D<float>.Zero;
        }
        
        if (IsFollowing)
        {
            if (Config.Instance.ThirdPersonView)
            {
                _followOffset = new Vector3D<float>(0, 1, 4);
                
                var xOffset = MathF.Sin(targetAngle) * _followOffsetThirdPerson.Z;
                var zOffset = MathF.Cos(targetAngle) * _followOffsetThirdPerson.Z;
            
                var newPosition = new Vector3D<float>(
                    targetPosition.X - xOffset,
                    targetPosition.Y + _followOffsetThirdPerson.Y,
                    targetPosition.Z - zOffset
                );
            
                Target = targetPosition;
                Position = newPosition;
            }

            if (Config.Instance.TopDownView)
            {
                _followOffset = new Vector3D<float>(0, 3, 0);
                
                var xOffset = MathF.Sin(targetAngle) * _followOffsetTopDown.Z;
                var zOffset = MathF.Cos(targetAngle) * _followOffsetTopDown.Z;
            
                var newPosition = new Vector3D<float>(
                    targetPosition.X - xOffset,
                    targetPosition.Y + _followOffsetTopDown.Y,
                    targetPosition.Z - zOffset
                );
            
                Target = targetPosition;
                Position = newPosition;
            }
        }
    }

    private void UpdatePosition()
    {
        if (!IsFollowing)
        {
            Position = GetPointFromAngles(_distanceToOrigin, _angleToZyPlane, _angleToZxPlane);
            Front = GetFrontVector(_angleToZyPlane, _angleToZxPlane);
        }
    }

    public void IncreaseZxAngle(float value = 0f)
    {
        if (IsFollowing) return;
        
        if (value == 0)
        {
            _angleToZxPlane += AngleChangeStepSize;
        }
        else
        {
            _angleToZxPlane += value;
        }
        UpdatePosition();
    }

    public void DecreaseZxAngle(float value = 0f)
    {
        if (IsFollowing) return;
        
        if (value == 0)
        {
            _angleToZxPlane -= AngleChangeStepSize;
        }
        else
        {
            _angleToZxPlane -= value;
        }
        
        UpdatePosition();
    }

    public void IncreaseZyAngle(float value = 0f)
    {
        if (IsFollowing) return;
        
        if (value == 0)
        {
            _angleToZyPlane += AngleChangeStepSize;
        }
        else
        {
            _angleToZyPlane += value;
        }
        
        UpdatePosition();
    }

    public void DecreaseZyAngle(float value = 0f)
    {
        if (IsFollowing) return;
        
        if (value == 0)
        {
            _angleToZyPlane -= AngleChangeStepSize;
        }
        else
        {
            _angleToZyPlane -= value;
        }
        
        UpdatePosition();
    }

    public void IncreaseDistance()
    {
        if (!IsFollowing)
        {
            _distanceToOrigin *= DistanceScaleFactor;
            UpdatePosition();
        }
        
        if (IsFollowing)
        {
            if (Config.Instance.ThirdPersonView)
            {
                _followOffsetThirdPerson *= new Vector3D<float>((float)DistanceScaleFactor, (float)DistanceScaleFactor, (float)DistanceScaleFactor);
            }

            if (Config.Instance.TopDownView)
            {
                _followOffsetTopDown *= new Vector3D<float>((float)DistanceScaleFactor, (float)DistanceScaleFactor, (float)DistanceScaleFactor);
            }
        }
    }

    public void DecreaseDistance()
    {
        if (!IsFollowing && _distanceToOrigin > 3)
        {
            _distanceToOrigin /= DistanceScaleFactor;
            UpdatePosition();
        }

        if (IsFollowing)
        {
            if (Config.Instance.ThirdPersonView && _followOffsetThirdPerson.Z > 2)
            {
                _followOffsetThirdPerson /= new Vector3D<float>((float)DistanceScaleFactor, (float)DistanceScaleFactor, (float)DistanceScaleFactor);
            }

            if (Config.Instance.TopDownView && _followOffsetTopDown.Y > 3)
            {
                _followOffsetTopDown /= new Vector3D<float>((float)DistanceScaleFactor, (float)DistanceScaleFactor, (float)DistanceScaleFactor);
            }
        }
    }


    private static Vector3D<float> GetFrontVector(double angleToZyPlane, double angleToZxPlane)
    {
        var x = Math.Cos(angleToZyPlane) * Math.Cos(angleToZxPlane);
        var y = -Math.Sin(angleToZxPlane);
        var z = Math.Sin(angleToZyPlane) * Math.Cos(angleToZxPlane);;
        return Vector3D.Normalize(new Vector3D<float>((float)x, (float)y, (float)z));
    }
    
    private static Vector3D<float> GetPointFromAngles(double distanceToOrigin, double angleToZyPlane, double angleToZxPlane)
    {
        var x = distanceToOrigin * Math.Cos(angleToZxPlane) * Math.Sin(angleToZyPlane);
        var z = distanceToOrigin * Math.Cos(angleToZxPlane) * Math.Cos(angleToZyPlane);
        var y = distanceToOrigin * Math.Sin(angleToZxPlane);
        return new Vector3D<float>((float)x, (float)y, (float)z);
    }
}
