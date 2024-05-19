using System.Numerics;
using Project.Camera;
using Project.Entities;
using Project.Interface;
using Project.Managers;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using Shader = Project.Components.Shader;
using Window = Silk.NET.Windowing.Window;

namespace Project
{
    internal static class Program
    {
        private static uint _program;

        private static GL _gl = null!;

        private static IWindow _window = null!;

        private static IInputContext _inputContext = null!;
        private static Gui _gui;
        
        private static Vector2D<int> _lastMousePosition;
        private static bool _isFirstMouseMovement = true;
        
        private static GlCity _city = null!;
        
        private static GlSkybox _skyBoxDay;
        private static GlSkybox _skyboxNight;
        
        private const string VertexShaderPath = "VertexShader.vert";
        private const string FragmentShaderPath = "FragmentShader.frag";
        
        private static void Main()
        {
            var windowOptions = WindowOptions.Default;
            windowOptions.Title = "Project";
            windowOptions.Size = new Vector2D<int>(1920, 1080);
            windowOptions.PreferredDepthBufferBits = 24;

            _window = Window.Create(windowOptions);

            _window.Load += Window_Load;
            _window.Update += Window_Update;
            _window.Render += Window_Render;
            _window.Closing += Window_Closing;

            _window.Run();
        }

        private static void Window_Load()
        {
            _inputContext = _window.CreateInput();
            foreach (var keyboard in _inputContext.Keyboards)
            {
                keyboard.KeyDown += Keyboard_KeyDown;
                keyboard.KeyUp += Keyboard_KeyUp;
            }
            
            foreach (var mouse in _inputContext.Mice)
            {
                mouse.Scroll += Mouse_Scroll;
                mouse.MouseMove += Mouse_MouseMove;
            }

            _gl = _window.CreateOpenGL();
            
            _gui = new Gui(_gl, _window, _inputContext);
            _window.FramebufferResize += s => { _gl.Viewport(s); };

            _gl.ClearColor(System.Drawing.Color.Black);
            _gl.Enable(EnableCap.DepthTest);

            Config.Instance.Init();
            Shader.Instance.Init(_gl, VertexShaderPath, FragmentShaderPath);
            CarManager.Instance.Init(_gl);
            ParticleSystemManager.Instance.Init(_gl);
            
            _skyBoxDay = new GlSkybox(_gl, "skybox_day.png");
            _skyboxNight = new GlSkybox(_gl, "skybox_night.png");
            
            _city = new GlCity(_gl);
            
            CarManager.Instance.AddPlayerCar(new Car(_gl, new Vector3D<float>(0, 0.15f, 0)));
            
            _gl.Enable(EnableCap.CullFace);
            _gl.Enable(EnableCap.DepthTest);
            _gl.DepthFunc(DepthFunction.Lequal);
        }

        private static void Window_Update(double deltaTime)
        {
            _gui.Update((float) deltaTime);
            CarManager.Instance.Update((float) deltaTime);
            GlCamera.Instance.Update((float) deltaTime, CarManager.Instance.PlayerCar.Position,  CarManager.Instance.PlayerCar.Orientation);
            Shader.Instance.SetUniform("viewPos", (Vector3)GlCamera.Instance.Position);
            Shader.Instance.SetUniform("isNightMode", Config.Instance.IsNight);
            LampManager.Instance.Update((float) deltaTime);
            ParticleSystemManager.Instance.Update((float) deltaTime);
        }

        private static void Window_Render(double deltaTime)
        {
            _gl.Clear(ClearBufferMask.ColorBufferBit);
            _gl.Clear(ClearBufferMask.DepthBufferBit);
    
            Config.Instance.ViewMatrix = Matrix4X4.CreateLookAt(GlCamera.Instance.Position, GlCamera.Instance.Target, GlCamera.Instance.UpVector);
            Config.Instance.ProjectionMatrix = Matrix4X4.CreatePerspectiveFieldOfView((float)Math.PI / 3f, _window.Size.X / (float)_window.Size.Y, 0.1f, 400);
            
            if (Config.Instance.IsNight)
            {
                _skyboxNight.Draw(Matrix4X4.CreateScale(100f));
            }
            else
            {
                _skyBoxDay.Draw(Matrix4X4.CreateScale(100f));
            }
            
            CarManager.Instance.Draw();
            BuildingsManager.Instance.Draw();
            RoadsManager.Instance.Draw();
            LampManager.Instance.Draw();
            ParticleSystemManager.Instance.Draw();
            
            _gui.Draw();
        }

        private static void Window_Closing()
        {
            _skyBoxDay.Free();
            _skyboxNight.Free();
            
            BuildingsManager.Instance.Free();
            RoadsManager.Instance.Free();
            LampManager.Instance.Free();
            CarManager.Instance.Free();
            ParticleSystemManager.Instance.Free();
            Shader.Instance.Free();
        }
        
        private static void Keyboard_KeyDown(IKeyboard keyboard, Key key, int arg3)
        {
            switch (key)
            {
                case Key.W:
                    CarManager.Instance.PlayerCar.StartMovingForward();
                    break;
                case Key.S:
                    CarManager.Instance.PlayerCar.StartMovingBackward();
                    break;
                case Key.A:
                    CarManager.Instance.PlayerCar.StartMovingLeft();
                    break;
                case Key.D:
                    CarManager.Instance.PlayerCar.StartMovingRight();
                    break;
                case Key.Left:
                    GlCamera.Instance.DecreaseZyAngle();
                    break;
                case Key.Right:
                    GlCamera.Instance.IncreaseZyAngle();
                    break;
                case Key.Down:
                    GlCamera.Instance.IncreaseDistance();
                    break;
                case Key.Up:
                    GlCamera.Instance.DecreaseDistance();
                    break;
                case Key.U:
                    GlCamera.Instance.IncreaseZxAngle();
                    break;
                case Key.J:
                    GlCamera.Instance.DecreaseZxAngle();
                    break;
                case Key.V:
                    GlCamera.Instance.ToggleFollowMode(CarManager.Instance.PlayerCar.Position);
                    break;
                case Key.L:
                    CarManager.Instance.PlayerCar.ToggleHeadlights();
                    break;
                case Key.Space:
                    break;
            }
        }
        
        private static void Keyboard_KeyUp(IKeyboard keyboard, Key key, int arg3)
        {
            switch (key)
            {
                case Key.W:
                    CarManager.Instance.PlayerCar.StopMovingForward();
                    break;
                case Key.S:
                    CarManager.Instance.PlayerCar.StopMovingBackward();
                    break;
                case Key.A:
                    CarManager.Instance.PlayerCar.StopMovingLeft();
                    break;
                case Key.D:
                    CarManager.Instance.PlayerCar.StopMovingRight();
                    break;
            }
        }
        
        private static void Mouse_Scroll(IMouse mouse, ScrollWheel scrollWheel)
        {
            switch (scrollWheel.Y)
            {
                case > 0:
                    GlCamera.Instance.DecreaseDistance();
                    break;
                case < 0:
                    GlCamera.Instance.IncreaseDistance();
                    break;
            }
        }
        
        private static void Mouse_MouseMove(IMouse mouse, Vector2 position)
        {
            if (_isFirstMouseMovement)
            {
                _lastMousePosition = new Vector2D<int>((int)position.X, (int)position.Y);
                _isFirstMouseMovement = false;
                return;
            }

            var deltaX = position.X - _lastMousePosition.X;
            
            _lastMousePosition = new Vector2D<int>((int)position.X, (int)position.Y);

            if (mouse.IsButtonPressed(MouseButton.Left))
            {
                const float sensitivityX = 0.005f;
                
                GlCamera.Instance.DecreaseZyAngle(deltaX * sensitivityX);
            }
        }
    }
}