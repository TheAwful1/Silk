using Silk.NET.Input;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using System.Drawing;

namespace Silk
{
    public class Juego
    {
        private static uint _vao;
        private static uint _vbo;
        private static uint _ebo;
        private static GL _gl;


        private static void KeyDown(IKeyboard keyboard, Key key, int keyCode) {
            Console.WriteLine(key);
            if (key == Key.Escape)
                _window.Close();          
        }
        private static IWindow _window;
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Silk.NET.Maths.Vector2D<int>(800, 600),
            Title = "Mi Juego"
        };
        public Juego()
        {
            // Constructor del juego
        }
        public void Iniciar()
        {           
            _window = Window.Create(options);
            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;

            _window.Run();
        }
        private static unsafe void OnLoad() {
            _gl = _window.CreateOpenGL();
            _gl.ClearColor(Color.CornflowerBlue);
            //Creando y bindeando el VAO
            _vao = _gl.GenVertexArray();
            _gl.BindVertexArray(_vao);
            float[] vertices = 
                { 
                0.5f,  0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                -0.5f,  0.5f, 0.0f
                };
            uint[] indices =
            {
                0u, 1u, 3u,
                1u, 2u, 3u
            };
            //GLSL
            const string vertexCode = @"
                    #version 330 core
                    
                    layout (location = 0) in vec3 aPosition;
                    
                    void main()
                    {
                        gl_Position = vec4(aPosition, 1.0);
                    }";

            //Creando y bindeando el VBO
            _vbo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            fixed (float* buf = vertices)
                _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
            //Creando y bindeando el EBO
            _ebo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
            fixed (uint* buf = indices)
                _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)), buf, BufferUsageARB.StaticDraw);

            //Esto es para tomar el input del teclado
            IInputContext input = _window.CreateInput();
            for (int i = 0; i < input.Keyboards.Count; i++)
                input.Keyboards[i].KeyDown += KeyDown;            
        }
        private static void OnUpdate(double deltaTime) {
            
        }
        private static unsafe void OnRender(double deltaTime) {
            _gl.Clear(ClearBufferMask.ColorBufferBit);
        }



        static void Main(String[] args)
        {
            var juego = new Juego();
            juego.Iniciar();
        }


    }    
}