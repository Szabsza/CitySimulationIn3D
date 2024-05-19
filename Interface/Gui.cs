using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Project.Interface;

public class Gui
{
    private ImGuiController _controller;

    private string[] _cameraModes;
    private int _selectedCameraMode;

    private bool _isNight;
    
    public Gui(GL gl, IWindow window, IInputContext inputContext)
    {
        _cameraModes = ["Third person", "Top Down"];
        _selectedCameraMode = 0;
            
        _controller = new ImGuiController(gl, window, inputContext);
    }

    public void Update(float deltaTime)
    {
        _controller.Update(deltaTime);
    }

    public void Draw()
    {
        ImGui.Begin("Control Panel", ImGuiWindowFlags.AlwaysAutoResize);
        ImGui.Checkbox("Night Mode", ref _isNight);
        Config.Instance.SetIsNight(_isNight);
        
        if (ImGui.Combo("Camera Mode", ref _selectedCameraMode, _cameraModes, _cameraModes.Length))
        {
            switch (_selectedCameraMode)
            {
                case 0:
                    Config.Instance.SetThirdPersonView();
                    break;
                case 1:
                    Config.Instance.SetTopDownView();
                    break;
            }
        }
        
        ImGui.SliderInt("Car Count", ref Config.Instance.CarCount, 0, Config.Instance.MaxCarCount);
        ImGui.SliderInt("Cops Count", ref Config.Instance.CopsCount, 0, Config.Instance.MaxCopCount);

        ImGui.Text("Movement: W A S D");
        ImGui.Text("Toggle lights: L");
        ImGui.Text("Toggle view: V");
        ImGui.Text("Zoom: Mouse Scroll");
        ImGui.End();
        
        _controller.Render();
    }

    public void Free()
    {
        _controller.Dispose();
    }
}