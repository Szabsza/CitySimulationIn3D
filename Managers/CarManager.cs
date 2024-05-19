using Project.Entities;
using Silk.NET.OpenGL;

namespace Project.Managers;

public class CarManager
{
    private static CarManager? _instance;

    private GL _gl = null!;
    
    public List<RegularCar> RegularCars;
    public List<CopCar> CopCars;
    public Car PlayerCar = null!;

    public void Init(GL gl)
    {
        _gl = gl;
    }   
    
    private CarManager()
    {
        RegularCars = new List<RegularCar>();
        CopCars = new List<CopCar>();
    }

    public static CarManager Instance => _instance ??= new CarManager();

    public void AddPlayerCar(Car playerCar)
    {
        PlayerCar = playerCar;
        PlayerCar.PlaceSomeWhereOnTheRoads();
    }

    public void PlayerCollidedWithLamp()
    {
        foreach (var cop in CopCars)
        {
            cop.SetModeToChasing();
        }
    }
    
    public void AdjustCarCount(int desiredCarCount)
    {
        var currentCarCount = RegularCars.Count;
        if (currentCarCount < desiredCarCount)
        {
            for (var i = currentCarCount; i < desiredCarCount; i++)
            {
                var newCar = new RegularCar(_gl, default);
                newCar.PlaceSomeWhereOnTheRoads();
                newCar.ToggleAutopilotMode();
                RegularCars.Add(newCar);
            }
        }
        else if (currentCarCount > desiredCarCount)
        {
            for (var i = currentCarCount - 1; i >= desiredCarCount; i--)
            {
                RegularCars[i].Free();
                RegularCars.RemoveAt(i);
            }
        }
    }

    public void AdjustCopCarCount(int desiredCopCarCount)
    {
        var currentCopCarCount = CopCars.Count;
        if (currentCopCarCount < desiredCopCarCount)
        {
            for (var i = currentCopCarCount; i < desiredCopCarCount; i++)
            {
                var newCopCar = new CopCar(_gl, default);
                newCopCar.PlaceSomeWhereOnTheRoads();
                newCopCar.ToggleAutopilotMode();
                CopCars.Add(newCopCar);
            }
        }
        else if (currentCopCarCount > desiredCopCarCount)
        {
            for (var i = currentCopCarCount - 1; i >= desiredCopCarCount; i--)
            {
                CopCars[i].TurnOnLightBars(false);
                CopCars[i].Free();
                CopCars.RemoveAt(i);
            }
        }
    }
    
    public void Draw()
    {
        PlayerCar.Draw();

        foreach (var cop in CopCars)
        {
            cop.Draw();
        }

        foreach (var car in RegularCars)
        {
            car.Draw();
        }
    }

    public void Update(float deltaTime)
    {
        AdjustCarCount(Config.Instance.CarCount);
        AdjustCopCarCount(Config.Instance.CopsCount);
        
        PlayerCar.Update(deltaTime);
        
        foreach (var cop in CopCars)
        {
            cop.Update(deltaTime);
        }

        foreach (var car in RegularCars)
        {
            car.Update(deltaTime);
        }
    }
    
    public void Free()
    {
        foreach (var car in RegularCars)
        {
            car.Free();
        }

        foreach (var cop in CopCars)
        {
            cop.Free();
        }

        PlayerCar.Free();
    }
}