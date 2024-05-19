using System.Numerics;
using Silk.NET.Maths;

namespace Project;

public class Config
{
    private static Config? _instance;
    public static Config Instance => _instance ??= new Config();
    
    // World
    public bool IsNight;
    public int CarCount;
    public int CopsCount;

    public int MaxCarCount = 10;
    public int MaxCopCount = 5;
    
    // Camera
    public bool ThirdPersonView;
    public bool TopDownView;
    
    // Uniform
    public Matrix4X4<float> ViewMatrix;
    public Matrix4X4<float> ProjectionMatrix;
    public float AmbientIntensity;
    
    
    private Config()
    {
        IsNight = false;
        CarCount = 3;
        CopsCount = 1;
        SetThirdPersonView();
        SetAmbientIntensity(0.5f);
    }

    public void Init() {}
    
    public void SetIsNight(bool value)
    {
        IsNight = value;
        AmbientIntensity = value ? 0.2f : 0.8f;
    }
    
    public void SetThirdPersonView()
    {
        ThirdPersonView = true;
        TopDownView = false;
    }
    
    public void SetTopDownView()
    {
        ThirdPersonView = false;
        TopDownView = true;
    }
    
    public void SetAmbientIntensity(float value)
    {
        AmbientIntensity = value;
    }
}