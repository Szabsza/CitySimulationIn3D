using System.Numerics;
using Project.Managers;
using Project.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = Project.Components.Shader;

namespace Project.Entities;

public class CopCar(GL gl, Vector3D<float> position) : GlCar(gl, position, "car_police.obj")
{
    private List<Vector3D<float>> _path = [];

    private int _currentPathIndex = 0;
    private float _pathUpdateTimer = 0;
    private const float PathUpdateInterval = 1.0f;

    private float _lightTimer = 0;
    private bool _lightToggle = false;

    public bool ChasingMode = false;

    public void SetModeToChasing()
    {
        Size = new Vector3D<float>(1.5f, 1.5f, 1.5f);
        AutopilotMode = false;
        ChasingMode = true;
    }

    public void SetModeToRegular()
    {
        Size = new Vector3D<float>(0.6f, 0.6f, 0.6f);
        ChasingMode = false;
        AutopilotMode = true;
        ReachedTarget = true;
    }

    private void Chasing(float deltaTime)
    {
        var playerPosition = CarManager.Instance.PlayerCar.Position;
        var closestPoint = FindClosestGraphPoint(playerPosition);
        var selfClosestPoint = FindClosestGraphPoint(Position);

        if (CollisionDetection.IsCollidingWithPlayerCar(Position, Size))
        {
            Position = closestPoint;
            SetModeToRegular();

            ParticleSystemManager.Instance.ExplosionParticleSystem.Emit(playerPosition);
            CarManager.Instance.PlayerCar.PlaceSomeWhereOnTheRoads();
            
            return;
        }

        _pathUpdateTimer -= deltaTime;
        if (_pathUpdateTimer <= 0)
        {
            _path = Pathfinding.FindPath(RoadsManager.Instance.RoadGraph, selfClosestPoint, closestPoint);

            _currentPathIndex = 0;
            _pathUpdateTimer = PathUpdateInterval;
        }

        _lightTimer -= deltaTime;
        if (_lightTimer <= 0)
        {
            _lightToggle = !_lightToggle;
            _lightTimer = 0.5f;
        }

        FollowPath(deltaTime);
    }

    public override void Update(float deltaTime)
    {
        var regularCar = CollisionDetection.IsCollidingWithRegularCar(Position, Size);
        if (regularCar != null)
        {
            ParticleSystemManager.Instance.ExplosionParticleSystem.Emit(regularCar.Position);
            
            regularCar.ReachedTarget = true;
            regularCar.PlaceSomeWhereOnTheRoads();
        }
        
        if (ChasingMode)
        {
            Chasing(deltaTime);
            return;
        }
        
        base.Update(deltaTime);
    }

    private void FollowPath(float deltaTime)
    {
        if (_currentPathIndex >= _path.Count) return;

        var target = _path[_currentPathIndex];
        var direction = Vector3D.Normalize(target - Position);

        Position += direction * deltaTime * Speed;

        if (MathF.Abs(direction.X) > MathF.Abs(direction.Z))
        {
            Orientation = direction.X > 0 ? MathF.PI / 2 : MathF.PI * 3 / 2;
        }
        else
        {
            Orientation = direction.Z > 0 ? 2 * MathF.PI : MathF.PI;
        }

        if (Vector3D.Distance(Position, target) < deltaTime * Speed)
        {
            _currentPathIndex++;
        }
    }

    public void TurnOnLightBars(bool on)
    {
        var policeLightColor = _lightToggle ? new Vector3(1.0f, 0.0f, 0.0f) : new Vector3(0.0f, 0.0f, 1.0f);
        var policeLightDirection = new Vector3(0, -1, 0);
        
        var copCarIndex = CarManager.Instance.CopCars.IndexOf(this);
        
        Shader.Instance.SetUniform($"isChasing[{copCarIndex}]", on);
        Shader.Instance.SetUniform($"policeLightColor", policeLightColor);
        Shader.Instance.SetUniform($"policeLights[{copCarIndex}].position",
            (Vector3)Position + new Vector3(0, 2, 0));
        Shader.Instance.SetUniform($"policeLights[{copCarIndex}].direction",
            policeLightDirection);
        Shader.Instance.SetUniform($"policeLights[{copCarIndex}].cutOff",
            MathF.Cos(MathF.PI / 3));
    }
    
    public override void Draw()
    {
        base.Draw();
        TurnOnLightBars(ChasingMode);
    }

    private Vector3D<float> FindClosestGraphPoint(Vector3D<float> position)
    {
        return RoadsManager.Instance.RoadGraph.Keys.MinBy(p => Vector3D.Distance(p, position));
    }
}