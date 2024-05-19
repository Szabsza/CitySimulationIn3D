using System.Numerics;
using Project.Managers;
using Project.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = Project.Components.Shader;

namespace Project.Entities;

public class Car(GL gl, Vector3D<float> position) : GlCar(gl, position, "car_sedan.obj")
{
    public bool AreHeadlightsTurnedOn;
    
    public void ToggleHeadlights()
    {
        AreHeadlightsTurnedOn = !AreHeadlightsTurnedOn;
    }
    
    public override void Draw()
    {
        base.Draw();    
        var forwardDirection = new Vector3D<float>(MathF.Sin(Orientation), 0f, MathF.Cos(Orientation));
        var rightDirection = new Vector3D<float>(MathF.Cos(Orientation), 0, -MathF.Sin(Orientation));
        
        Shader.Instance.SetUniform("lightDirection1", (Vector3)forwardDirection);
        Shader.Instance.SetUniform("lightPosition1",
            (Vector3)(Position + forwardDirection * 0.4f + rightDirection * 0.2f));

        Shader.Instance.SetUniform("lightDirection2", (Vector3)forwardDirection);
        Shader.Instance.SetUniform("lightPosition2",
            (Vector3)(Position + forwardDirection * 0.4f + rightDirection * -0.2f));

        Shader.Instance.SetUniform("areHeadlightsTurnedOn", AreHeadlightsTurnedOn);
    }
    
    
    
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        
        var moveX = MathF.Sin(Orientation) * Speed * deltaTime;
        var moveZ = MathF.Cos(Orientation) * Speed * deltaTime;
        
        var potentialPosition = Position;

        if (_shouldRotateWheelsToDefault)
        {
            if (_wheelOrientation > 0)
            {
                _wheelOrientation -= deltaTime * Speed;
                if (_wheelOrientation <= 0)
                {
                    _wheelOrientation = 0;
                }
            }

            if (_wheelOrientation < 0)
            {
                _wheelOrientation += deltaTime * Speed;
                if (_wheelOrientation >= 0)
                {
                    _wheelOrientation = 0;
                }
            }
        }

        if (_isMovingForward)
        {
            _wheelRotationForward += deltaTime * Speed;
            potentialPosition += new Vector3D<float>(moveX, 0, moveZ);
        }

        if (_isMovingBackward)
        {
            _wheelRotationForward -= deltaTime * Speed;
            potentialPosition -= new Vector3D<float>(moveX, 0, moveZ);
        }

        if (_isMovingLeft)
        {
            if (_isMovingForward) Orientation += deltaTime * 2;
            if (_isMovingBackward) Orientation -= deltaTime * 2;
            if (_wheelOrientation + deltaTime < MathF.PI / 6) _wheelOrientation += deltaTime * Speed;
        }

        if (_isMovingRight)
        {
            if (_isMovingForward) Orientation -= deltaTime * 2;
            if (_isMovingBackward) Orientation += deltaTime * 2;
            if (_wheelOrientation - deltaTime > -MathF.PI / 6) _wheelOrientation -= deltaTime * Speed;
        }
        
        if (!CollisionDetection.IsCollidingWithBuildings(potentialPosition, Size))
        {
            Position = potentialPosition;
        }
        
        var lamp = CollisionDetection.IsCollidingWithLamps(potentialPosition, Size);
        if (lamp != null)
        {
            CarManager.Instance.PlayerCollidedWithLamp();
        }
        lamp?.Fall(Orientation);

        var regularCar = CollisionDetection.IsCollidingWithRegularCar(potentialPosition, Size);
        if (regularCar != null)
        {
            ParticleSystemManager.Instance.ExplosionParticleSystem.Emit(regularCar.Position);
            
            regularCar.ReachedTarget = true;
            regularCar.PlaceSomeWhereOnTheRoads();
        }
        
    }
}

