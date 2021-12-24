using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using GameEngine.Core;
using ImGuiNET;
using Silk.NET.OpenGL;
using Dear_ImGui_Sample;
using GameEngine.Rendering;
using Silk.NET.GLFW;
using Texture = Silk.NET.OpenGL.Texture;

namespace GameEngine.GUI; 

public class GUI {
    
    private bool _frameBegun;
    private GameEngine.Rendering.Shaders.Shader _shader;
    private int _vertexArray;
    private int _vertexBuffer;
    private int _vertexBufferSize;
    private int _indexBuffer;
    private int _indexBufferSize;
    private ImGuiIOPtr _handle;
    private Texture _fontTexture;

    private uint _textureID;

    private GL GL => RenderingEngine.Gl;
    private Glfw Glfw => RenderingEngine.Glfw;

    public void Attach() {
        IntPtr context = ImGui.CreateContext();
        ImGui.SetCurrentContext(context);
        //ImGui.StyleColorsDark();
        ImGui.StyleColorsLight();
        _handle = ImGui.GetIO();
        //_handle.BackendFlags = ImGuiBackendFlags.HasMouseCursors | ImGuiBackendFlags.HasSetMousePos;
        _handle.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
        _handle.Fonts.AddFontDefault();
        
        CreateDeviceResources();
        
        _handle.KeyMap[(int) ImGuiKey.Tab] = (int) Keys.Tab;
        _handle.KeyMap[(int) ImGuiKey.Enter] = (int) Keys.Enter;
        _handle.KeyMap[(int) ImGuiKey.Space] = (int) Keys.Space;
        _handle.KeyMap[(int) ImGuiKey.Backspace] = (int) Keys.Backspace;
        _handle.KeyMap[(int) ImGuiKey.Escape] = (int) Keys.Escape;
        
        SetPerFrameImGuiData(1f / 60f);
        
        ImGui.NewFrame();
        _frameBegun = true;
    }
    
    private void SetPerFrameImGuiData(float deltaSeconds)
    {
        ImGuiIOPtr io = ImGui.GetIO();
        io.DisplaySize = new System.Numerics.Vector2(
            Configuration.WindowWidth / 1.0f,         //scale factor x
            Configuration.WindowHeight / 1.0f);       //scale factor y
        io.DisplayFramebufferScale = Vector2.One;       //scale factor
        io.DeltaTime = deltaSeconds;                  // DeltaTime is in seconds.
    }

    public void Detach() {
        
    }

    public void Update(/*GameWindow wnd,*/ float deltaSeconds)
    {
        if (_frameBegun)
        {
            ImGui.Render();
        }

        SetPerFrameImGuiData(deltaSeconds);
        //UpdateImGuiInput(wnd);

        _frameBegun = true;
        ImGui.NewFrame();
    }
    
    public void Render()
    {/*
        if (_frameBegun)
        {
            _frameBegun = false;
            ImGui.Render();
            RenderImDrawData(ImGui.GetDrawData());
        }*/
        SetPerFrameImGuiData(0.2f);
        ImGui.NewFrame();
        
        //ImGui.Begin("Window Name");
        //ImGui.Text("Test Text");
        //ImGui.End();
        ImGui.ShowDemoWindow();
        
        ImGui.Render();
        RenderImDrawData(ImGui.GetDrawData());
    }
    
    /*
    private void Render() {
        if(_frameBegun) {
            _frameBegun = false;
            ImGui.Render();
            RenderImDrawData(ImGui.GetDrawData());
        }
        _handle = ImGui.GetIO();
        _handle.DisplaySize = new Vector2(Configuration.WindowWidth, Configuration.WindowHeight);
        
        float time = (float) Glfw.Time;
        _handle.DeltaTime = _time > 0.0f ? (time - _time) : (1.0f / 60.0f);
        _time = time;
        
        ImGui.NewFrame();
        
        bool showDemoWindow = true;
        ImGui.ShowDemoWindow(ref showDemoWindow);
        
        ImGui.Render();
        ImGui.GetDrawData();
        
    }*/
    
    public void CreateDeviceResources() {
        
            Util.CreateVertexArray("ImGui", out _vertexArray);

            _vertexBufferSize = 10000;
            _indexBufferSize = 2000;

            Util.CreateVertexBuffer("ImGui", out _vertexBuffer);
            Util.CreateElementBuffer("ImGui", out _indexBuffer);
            GL.NamedBufferData((uint) _vertexBuffer, (uint) _vertexBufferSize, IntPtr.Zero, VertexBufferObjectUsage.DynamicDraw);
            GL.NamedBufferData((uint) _indexBuffer, (uint) _indexBufferSize, IntPtr.Zero, VertexBufferObjectUsage.DynamicDraw);

            RecreateFontDeviceTexture();

            string VertexSource = @"#version 330 core
uniform mat4 projection_matrix;
layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_texCoord;
layout(location = 2) in vec4 in_color;
out vec4 color;
out vec2 texCoord;
void main()
{
    gl_Position = projection_matrix * vec4(in_position, 0, 1);
    color = in_color;
    texCoord = in_texCoord;
}";
            string FragmentSource = @"#version 330 core
uniform sampler2D in_fontTexture;
in vec4 color;
in vec2 texCoord;
out vec4 outputColor;
void main()
{
    outputColor = color * texture(in_fontTexture, texCoord);
}";
            _shader = new GameEngine.Rendering.Shaders.Shader(VertexSource, FragmentSource);

            GL.VertexArrayVertexBuffer((uint) _vertexArray, 0, (uint) _vertexBuffer, IntPtr.Zero, (uint) Unsafe.SizeOf<ImDrawVert>());
            GL.VertexArrayElementBuffer((uint) _vertexArray, (uint) _indexBuffer);

            GL.EnableVertexArrayAttrib((uint) _vertexArray, 0);
            GL.VertexArrayAttribBinding((uint) _vertexArray, 0, 0);
            GL.VertexArrayAttribFormat((uint) _vertexArray, 0, 2, VertexAttribType.Float, false, 0);

            GL.EnableVertexArrayAttrib((uint) _vertexArray, 1);
            GL.VertexArrayAttribBinding((uint) _vertexArray, 1, 0);
            GL.VertexArrayAttribFormat((uint) _vertexArray, 1, 2, VertexAttribType.Float, false, 8);

            GL.EnableVertexArrayAttrib((uint) _vertexArray, 2);
            GL.VertexArrayAttribBinding((uint) _vertexArray, 2, 0);
            GL.VertexArrayAttribFormat((uint) _vertexArray, 2, 4, VertexAttribType.UnsignedByte, true, 16);

            Util.CheckGLError("End of ImGui setup");
        }
    
    /// <summary>
    /// Recreates the device texture used to render text.
    /// </summary>
    public unsafe void RecreateFontDeviceTexture()
    {
        ImGuiIOPtr io = ImGui.GetIO();
        io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out int width, out int height, out int bytesPerPixel);

        GL gl = GL.GetApi(Glfw.GetProcAddress);
        _textureID = gl.GenTexture();
        
        //When we bind a texture we can choose which textureslot we can bind it to.
        gl.ActiveTexture((TextureUnit)0);
        gl.BindTexture(TextureTarget.Texture2D, _textureID);

        //Setting the data of a texture.
        gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint) width, (uint) height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
        //Setting some texture perameters so the texture behaves as expected.
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear); // linear, nearest
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
        
        //_fontTexture = new Texture2D()("ImGui Text Atlas", width, height, pixels);
            
        io.Fonts.SetTexID((IntPtr)_textureID);

        io.Fonts.ClearTexData();
    }
    
    private unsafe void RenderImDrawData(ImDrawDataPtr draw_data) {
        GL GL = Silk.NET.OpenGL.GL.GetApi(Glfw.GetProcAddress);
            if (draw_data.CmdListsCount == 0)
            {
                return;
            }

            for (int i = 0; i < draw_data.CmdListsCount; i++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[i];

                int vertexSize = cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>();
                if (vertexSize > _vertexBufferSize)
                {
                    int newSize = (int)Math.Max(_vertexBufferSize * 1.5f, vertexSize);
                    GL.NamedBufferData((uint) _vertexBuffer, (nuint) newSize, IntPtr.Zero, VertexBufferObjectUsage.DynamicDraw);
                    _vertexBufferSize = newSize;

                    Console.LogWarning($"Resized dear imgui vertex buffer to new size {_vertexBufferSize}");
                }

                int indexSize = cmd_list.IdxBuffer.Size * sizeof(ushort);
                if (indexSize > _indexBufferSize)
                {
                    int newSize = (int)Math.Max(_indexBufferSize * 1.5f, indexSize);
                    GL.NamedBufferData((uint) _indexBuffer, (nuint) newSize, IntPtr.Zero, VertexBufferObjectUsage.DynamicDraw);
                    _indexBufferSize = newSize;

                    Console.LogWarning($"Resized dear imgui index buffer to new size {_indexBufferSize}");
                }
            }

            // Setup orthographic projection matrix into our constant buffer
            ImGuiIOPtr io = ImGui.GetIO();
            GameEngine.Numerics.Matrix4x4 mvp = GameEngine.Numerics.Matrix4x4.CreateOrthographicOffCenter(
                0.0f,
                io.DisplaySize.X,
                io.DisplaySize.Y,
                0.0f,
                -10.0f,
                10.0f);

            _shader.Use();
            _shader.SetMatrix4x4("projection_matrix", mvp);
            //GL.UniformMatrix4(_shader.GetUniformLocation("projection_matrix"), false, ref mvp);
            
            //bind
            GL.ActiveTexture((TextureUnit)0);
            GL.BindTexture(TextureTarget.Texture2D, _textureID);
            
            _shader.SetInt("in_fontTexture", 0);
            //GL.Uniform1(_shader.GetUniformLocation("in_fontTexture"), 0);
            Util.CheckGLError("Projection");

            GL.BindVertexArray((uint) _vertexArray);
            Util.CheckGLError("VAO");

            draw_data.ScaleClipRects(io.DisplayFramebufferScale);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ScissorTest);
            GL.BlendEquation(BlendEquationModeEXT.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            // Render command lists
            for (int n = 0; n < draw_data.CmdListsCount; n++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[n];

                GL.NamedBufferSubData((uint) _vertexBuffer, IntPtr.Zero, (nuint) (cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>()), cmd_list.VtxBuffer.Data.ToPointer());
                Util.CheckGLError($"Data Vert {n}");

                GL.NamedBufferSubData((uint) _vertexBuffer, IntPtr.Zero, (nuint) (cmd_list.IdxBuffer.Size * sizeof(ushort)), cmd_list.IdxBuffer.Data.ToPointer());
                Util.CheckGLError($"Data Idx {n}");

                int vtx_offset = 0;
                int idx_offset = 0;

                for (int cmd_i = 0; cmd_i < cmd_list.CmdBuffer.Size; cmd_i++)
                {
                    ImDrawCmdPtr pcmd = cmd_list.CmdBuffer[cmd_i];
                    if (pcmd.UserCallback != IntPtr.Zero)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.BindTexture(TextureTarget.Texture2D, (uint) pcmd.TextureId);
                        Util.CheckGLError("Texture");

                        // We do _windowHeight - (int)clip.W instead of (int)clip.Y because gl has flipped Y when it comes to these coordinates
                        var clip = pcmd.ClipRect;
                        GL.Scissor((int)clip.X, (int) Configuration.WindowHeight - (int)clip.W, (uint)(clip.Z - clip.X), (uint)(clip.W - clip.Y));
                        Util.CheckGLError("Scissor");

                        if ((io.BackendFlags & ImGuiBackendFlags.RendererHasVtxOffset) != 0)
                        {
                            GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (uint)pcmd.ElemCount, DrawElementsType.UnsignedShort, (IntPtr)(idx_offset * sizeof(ushort)), vtx_offset);
                        }
                        else
                        {
                            GL.DrawElements(PrimitiveType.Triangles, (uint)pcmd.ElemCount, DrawElementsType.UnsignedShort, (int)pcmd.IdxOffset * sizeof(ushort));
                        }
                        Util.CheckGLError("Draw");
                    }

                    idx_offset += (int)pcmd.ElemCount;
                }
                vtx_offset += cmd_list.VtxBuffer.Size;
            }

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.ScissorTest);
        }
    
}
