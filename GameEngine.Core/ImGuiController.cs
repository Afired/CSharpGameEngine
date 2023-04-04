using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Input.Extensions;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace GameEngine.Core;

public class ImGuiController : IDisposable {
    
    private readonly GL _gl;
    private readonly IView _view;
    private readonly IInputContext _input;
    private readonly List<char> _pressedChars = new();
    private Version? _glVersion;
    private bool _frameBegun;
    private IKeyboard? _keyboard;
    private int _attribLocationTex;
    private int _attribLocationProjMtx;
    private int _attribLocationVtxPos;
    private int _attribLocationVtxUV;
    private int _attribLocationVtxColor;
    private uint _vboHandle;
    private uint _elementsHandle;
    private uint _vertexArrayObject;
    private GameEngine.Core.Rendering.Textures.Texture2D? _fontTexture;
    private GameEngine.Core.Rendering.Shaders.Shader? _shader;
    private int _windowWidth;
    private int _windowHeight;
    
    /// <summary>Constructs a new ImGuiController.</summary>
    public ImGuiController(GL gl, IView view, IInputContext input) {
        _gl = gl;
        _glVersion = new Version(gl.GetInteger(GLEnum.MajorVersion), gl.GetInteger(GLEnum.MinorVersion));
        _view = view;
        _input = input;
        _windowWidth = view.Size.X;
        _windowHeight = view.Size.Y;
        ImGui.SetCurrentContext(ImGui.CreateContext());
        ImGui.StyleColorsDark();
        ImGuiIOPtr io = ImGui.GetIO();
        
        io.Fonts.AddFontDefault();
        // merge in icons from Font Awesome
        
        
        unsafe {
//            ImVector usedChars = new ImVector();
//            usedChars.
//            ImFontGlyphRangesBuilder imFontGlyphRangesBuilder = new ImFontGlyphRangesBuilder() {
//                UsedChars = usedChars,
//            };
//            ImFontGlyph imFontGlyph = new ImFontGlyph();
//            imFontGlyph.
//            ImFontConfig config = new ImFontConfig {
//                MergeMode = Convert.ToByte(true),
//                PixelSnapH = Convert.ToByte(true),
//            };
//            //        ImFontGlyphRangesBuilder builder = new ImFontGlyphRangesBuilder();
//            
//            io.Fonts.AddFontFromFileTTF("../../ImGUI/Fonts/Icons", 13.0f, &config, imFontGlyphRangesBuilder.UsedChars.Data);
        }
        //https: //fontawesome.com/v5/cheatsheet/free/solid#use
        
//        unsafe {
//            ImFontConfig config = new ImFontConfig {
//                MergeMode = Convert.ToByte(true),
//                PixelSnapH = Convert.ToByte(true),
//            };
//            
//            string path = Path.GetFullPath("../../../Assets/Fonts/FontAwesome.ttf");
//            Span<ushort> span = stackalloc ushort[] { 0xF641, 0 };
////            MemoryMarshal.GetReference(span)
//            io.Fonts.AddFontFromFileTTF(path, 14, null, io.Fonts.GetGlyphRangesJapanese());
//            
//            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
//            CreateDeviceResources();
//        }

//        ImFontGlyph imFontGlyph = new ImFontGlyph();
//        imFontGlyph.;
//        ImFontGlyphRangesBuilder imFontGlyphRangesBuilder = new ImFontGlyphRangesBuilder();
//        imFontGlyphRangesBuilder.UsedChars = new ImVector()
//        io.Fonts.Fonts[0].AddGlyph();
        
//        unsafe {
//            ImFontConfig config = new ImFontConfig {
//                MergeMode = Convert.ToByte(true),
//                PixelSnapH = Convert.ToByte(true),
//            };
//            
//            string path = Path.GetFullPath("../../../Assets/Fonts/FontAwesome.ttf");
//            
//            fixed(ushort* ptr = new ushort[] { 0xF641 }) {
//                io.Fonts.AddFontFromFileTTF(path, 14, &config, MemoryMarshal.GetReference(ptr)/*io.Fonts.GetGlyphRangesKorean()*/);
//            }
//        }
        
        io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
        this.CreateDeviceResources();
        SetKeyMappings();
        SetPerFrameImGuiData(0.016666668f);
        BeginFrame();
    }
    
    /// <summary>
    /// Constructs a new ImGuiController with font configuration.
    /// </summary>
    public ImGuiController(GL gl, IView view, IInputContext input, ImGuiFontConfig imGuiFontConfig) {
        _gl = gl;
        _glVersion = new Version(gl.GetInteger(GLEnum.MajorVersion), gl.GetInteger(GLEnum.MinorVersion));
        _view = view;
        _input = input;
        _windowWidth = view.Size.X;
        _windowHeight = view.Size.Y;
        ImGui.SetCurrentContext(ImGui.CreateContext());
        ImGui.StyleColorsDark();
        ImGuiIOPtr io = ImGui.GetIO();
        io.Fonts.AddFontFromFileTTF(imGuiFontConfig.FontPath, (float)imGuiFontConfig.FontSize);
        io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
        CreateDeviceResources();
        SetKeyMappings();
        SetPerFrameImGuiData(0.016666668f);
        BeginFrame();
    }
    
    private void BeginFrame() {
        ImGui.NewFrame();
        _frameBegun = true;
        _keyboard = _input.Keyboards[0];
        _view.Resize += new Action<Vector2D<int>>(WindowResized);
        _keyboard.KeyChar += new Action<IKeyboard, char>(OnKeyChar);
    }
    
    private void OnKeyChar(IKeyboard arg1, char arg2) {
        _pressedChars.Add(arg2);
    }
    
    private void WindowResized(Vector2D<int> size) {
        _windowWidth = size.X;
        _windowHeight = size.Y;
    }
    
    /// <summary>
    /// Renders the ImGui draw list data.
    /// This method requires a <see cref="!:GraphicsDevice" /> because it may create new DeviceBuffers if the size of vertex
    /// or index data has increased beyond the capacity of the existing buffers.
    /// A <see cref="!:CommandList" /> is needed to submit drawing and resource update commands.
    /// </summary>
    public void Render() {
        if(!_frameBegun) return;
        _frameBegun = false;
        ImGui.Render();
        RenderImDrawData(ImGui.GetDrawData());
    }
    
    /// <summary>Updates ImGui input and IO configuration state.</summary>
    public void Update(float deltaSeconds) {
        if(_frameBegun) ImGui.Render();
        SetPerFrameImGuiData(deltaSeconds);
        UpdateImGuiInput();
        _frameBegun = true;
        ImGui.NewFrame();
    }
    
    /// <summary>
    /// Sets per-frame data based on the associated window.
    /// This is called by Update(float).
    /// </summary>
    private void SetPerFrameImGuiData(float deltaSeconds) {
        var io = ImGui.GetIO();
        io.DisplaySize = new Vector2((float)_windowWidth, (float)_windowHeight);
        if(_windowWidth > 0 && _windowHeight > 0)
            io.DisplayFramebufferScale = new Vector2((float)(_view.FramebufferSize.X / _windowWidth),
                (float)(_view.FramebufferSize.Y / _windowHeight)
            );
        io.DeltaTime = deltaSeconds;
    }
    
    private void UpdateImGuiInput() {
        ImGuiIOPtr io = ImGui.GetIO();
        
        MouseState mouseState = _input.Mice[0].CaptureState();
        IKeyboard keyboard = _input.Keyboards[0];
        
        io.MouseDown[0] = mouseState.IsButtonPressed(MouseButton.Left);
        io.MouseDown[1] = mouseState.IsButtonPressed(MouseButton.Right);
        io.MouseDown[2] = mouseState.IsButtonPressed(MouseButton.Middle);
        
        Point point = new Point((int) mouseState.Position.X, (int) mouseState.Position.Y);
        io.MousePos = new Vector2((float) point.X, (float) point.Y);
        
//        ScrollWheel scrollWheel = mouseState.GetScrollWheels()[0];
//        io.MouseWheel = scrollWheel.Y;
//        io.MouseWheelH = scrollWheel.X;

        io.MouseWheel = (float) Input.Input.ScrollDelta.Y;
        io.MouseWheelH = (float) Input.Input.ScrollDelta.X;
        
        foreach(Key key in Enum.GetValues(typeof(Key))) {
            if(key != Key.Unknown)
                io.KeysDown[(int)key] = keyboard.IsKeyPressed(key);
        }
        foreach(char pressedChar in _pressedChars) {
            io.AddInputCharacter(pressedChar);
        }
        _pressedChars.Clear();
        io.KeyCtrl = keyboard.IsKeyPressed(Key.ControlLeft) || keyboard.IsKeyPressed(Key.ControlRight);
        io.KeyAlt = keyboard.IsKeyPressed(Key.AltLeft) || keyboard.IsKeyPressed(Key.AltRight);
        io.KeyShift = keyboard.IsKeyPressed(Key.ShiftLeft) || keyboard.IsKeyPressed(Key.ShiftRight);
        io.KeySuper = keyboard.IsKeyPressed(Key.SuperLeft) || keyboard.IsKeyPressed(Key.SuperRight);
    }
    
    internal void PressChar(char keyChar) {
        _pressedChars.Add(keyChar);
    }
    
    private static void SetKeyMappings() {
        var io = ImGui.GetIO();
//        io.KeyMap[0] = 258;
//        io.KeyMap[1] = 263;
//        io.KeyMap[2] = 262;
//        io.KeyMap[3] = 265;
//        io.KeyMap[4] = 264;
//        io.KeyMap[5] = 266;
//        io.KeyMap[6] = 267;
//        io.KeyMap[7] = 268;
//        io.KeyMap[8] = 269;
//        io.KeyMap[10] = 261;
//        io.KeyMap[11] = 259;
//        io.KeyMap[13] = 257;
//        io.KeyMap[14] = 256;
        
//        io.KeyMap[16] = 65;
//        io.KeyMap[17] = 67;
//        io.KeyMap[18] = 86;
//        io.KeyMap[19] = 88;
//        io.KeyMap[20] = 89;
//        io.KeyMap[21] = 90;
        
        io.KeyMap[(int)ImGuiKey.Tab] = (int)Key.Tab;
        io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Key.Left;
        io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Key.Right;
        io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Key.Up;
        io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Key.Down;
        io.KeyMap[(int)ImGuiKey.PageUp] = (int)Key.PageUp;
        io.KeyMap[(int)ImGuiKey.PageDown] = (int)Key.PageDown;
        io.KeyMap[(int)ImGuiKey.Home] = (int)Key.Home;
        io.KeyMap[(int)ImGuiKey.End] = (int)Key.End;
        io.KeyMap[(int)ImGuiKey.Delete] = (int)Key.Delete;
        io.KeyMap[(int)ImGuiKey.Backspace] = (int)Key.Backspace;
        io.KeyMap[(int)ImGuiKey.Enter] = (int)Key.Enter;
        io.KeyMap[(int)ImGuiKey.Escape] = (int)Key.Escape;
    }
    
    private unsafe void SetupRenderState(ImDrawDataPtr drawDataPtr, int framebufferWidth, int framebufferHeight) {
        _gl.Enable(GLEnum.Blend);
        _gl.BlendEquation(GLEnum.FuncAdd);
        _gl.BlendFuncSeparate(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha, GLEnum.True, GLEnum.OneMinusSrcAlpha);
        _gl.Disable(GLEnum.CullFace);
        _gl.Disable(GLEnum.DepthTest);
        _gl.Disable(GLEnum.StencilTest);
        _gl.Enable(GLEnum.ScissorTest);
        _gl.Disable(GLEnum.PrimitiveRestart);
        _gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);
        float x = drawDataPtr.DisplayPos.X;
        float num1 = drawDataPtr.DisplayPos.X + drawDataPtr.DisplaySize.X;
        float y = drawDataPtr.DisplayPos.Y;
        float num2 = drawDataPtr.DisplayPos.Y + drawDataPtr.DisplaySize.Y;
        Span<float> pointer = stackalloc float[16];
        pointer[0] = (float)(2.0 / ((double)num1 - (double)x));
        pointer[1] = 0.0f;
        pointer[2] = 0.0f;
        pointer[3] = 0.0f;
        pointer[4] = 0.0f;
        pointer[5] = (float)(2.0 / ((double)y - (double)num2));
        pointer[6] = 0.0f;
        pointer[7] = 0.0f;
        pointer[8] = 0.0f;
        pointer[9] = 0.0f;
        pointer[10] = -1f;
        pointer[11] = 0.0f;
        pointer[12] = (float)(((double)num1 + (double)x) / ((double)x - (double)num1));
        pointer[13] = (float)(((double)y + (double)num2) / ((double)num2 - (double)y));
        pointer[14] = 0.0f;
        pointer[15] = 1f;
//        _shader.UseShader();
        _shader.Use();
        _gl.Uniform1(_attribLocationTex, 0);
        _gl.UniformMatrix4(_attribLocationProjMtx, 1U, false, pointer);
        _gl.BindSampler(0U, 0U);
        _vertexArrayObject = _gl.GenVertexArray();
        _gl.BindVertexArray(_vertexArrayObject);
        _gl.BindBuffer(GLEnum.ArrayBuffer, _vboHandle);
        _gl.BindBuffer(GLEnum.ElementArrayBuffer, _elementsHandle);
        _gl.EnableVertexAttribArray((uint)_attribLocationVtxPos);
        _gl.EnableVertexAttribArray((uint)_attribLocationVtxUV);
        _gl.EnableVertexAttribArray((uint)_attribLocationVtxColor);
        _gl.VertexAttribPointer((uint)_attribLocationVtxPos, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*)null);
        _gl.VertexAttribPointer((uint)_attribLocationVtxUV, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*) new nint(8));
        _gl.VertexAttribPointer((uint)_attribLocationVtxColor, 4, GLEnum.UnsignedByte, true, (uint)sizeof(ImDrawVert), (void*) new nint(16));
    }
    
    private unsafe void RenderImDrawData(ImDrawDataPtr drawDataPtr) {
        var framebufferWidth = (int)((double)drawDataPtr.DisplaySize.X * (double)drawDataPtr.FramebufferScale.X);
        var framebufferHeight = (int)((double)drawDataPtr.DisplaySize.Y * (double)drawDataPtr.FramebufferScale.Y);
        if(framebufferWidth <= 0 || framebufferHeight <= 0) return;
        _gl.GetInteger(GLEnum.ActiveTexture, out int data1);
        _gl.ActiveTexture(GLEnum.Texture0);
        _gl.GetInteger(GLEnum.CurrentProgram, out int data2);
        _gl.GetInteger(GLEnum.TextureBinding2D, out int data3);
        _gl.GetInteger(GLEnum.SamplerBinding, out int data4);
        _gl.GetInteger(GLEnum.ArrayBufferBinding, out int data5);
        _gl.GetInteger(GLEnum.VertexArrayBinding, out int data6);
        // ISSUE: untyped stack allocation
//        var data7 = new Span<int>((void*)__untypedstackalloc(new nint(8)), 2);
        Span<int> data7 = stackalloc int[8];
        _gl.GetInteger(GLEnum.PolygonMode, data7);
        // ISSUE: untyped stack allocation
//        var data8 = new Span<int>((void*)__untypedstackalloc(new nint(16)), 4);
        Span<int> data8 = stackalloc int[4];
        _gl.GetInteger(GLEnum.ScissorBox, data8);
        _gl.GetInteger(GLEnum.BlendSrcRgb, out int data9);
        _gl.GetInteger(GLEnum.BlendDstRgb, out int data10);
        _gl.GetInteger(GLEnum.BlendSrcAlpha, out int data11);
        _gl.GetInteger(GLEnum.BlendDstAlpha, out int data12);
        _gl.GetInteger(GLEnum.BlendEquation, out int data13);
        _gl.GetInteger(GLEnum.BlendEquationAlpha, out int data14);
        bool flag1 = _gl.IsEnabled(GLEnum.Blend);
        bool flag2 = _gl.IsEnabled(GLEnum.CullFace);
        bool flag3 = _gl.IsEnabled(GLEnum.DepthTest);
        bool flag4 = _gl.IsEnabled(GLEnum.StencilTest);
        bool flag5 = _gl.IsEnabled(GLEnum.ScissorTest);
        bool flag6 = _gl.IsEnabled(GLEnum.PrimitiveRestart);
        SetupRenderState(drawDataPtr, framebufferWidth, framebufferHeight);
        Vector2 vector2_1 = drawDataPtr.DisplayPos;
        Vector2 vector2_2 = drawDataPtr.FramebufferScale;
        for(int index1 = 0; index1 < drawDataPtr.CmdListsCount; ++index1) {
            ImDrawListPtr imDrawListPtr = drawDataPtr.CmdListsRange[index1];
            _gl.BufferData(GLEnum.ArrayBuffer, (nuint)(imDrawListPtr.VtxBuffer.Size * sizeof(ImDrawVert)),
                (void*)imDrawListPtr.VtxBuffer.Data, GLEnum.StreamDraw
            );
            _gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(imDrawListPtr.IdxBuffer.Size * 2),
                (void*)imDrawListPtr.IdxBuffer.Data, GLEnum.StreamDraw
            );
            for(int index2 = 0; index2 < imDrawListPtr.CmdBuffer.Size; ++index2) {
                ImDrawCmdPtr imDrawCmdPtr = imDrawListPtr.CmdBuffer[index2];
                if(imDrawCmdPtr.UserCallback != nint.Zero) throw new NotImplementedException();
                Vector4 vector4;
                vector4.X = (imDrawCmdPtr.ClipRect.X - vector2_1.X) * vector2_2.X;
                vector4.Y = (imDrawCmdPtr.ClipRect.Y - vector2_1.Y) * vector2_2.Y;
                vector4.Z = (imDrawCmdPtr.ClipRect.Z - vector2_1.X) * vector2_2.X;
                vector4.W = (imDrawCmdPtr.ClipRect.W - vector2_1.Y) * vector2_2.Y;
                if((double)vector4.X < (double)framebufferWidth && (double)vector4.Y < (double)framebufferHeight &&
                   (double)vector4.Z >= 0.0 && (double)vector4.W >= 0.0) {
                    _gl.Scissor((int)vector4.X, (int)((double)framebufferHeight - (double)vector4.W),
                        (uint)((double)vector4.Z - (double)vector4.X), (uint)((double)vector4.W - (double)vector4.Y)
                    );
                    _gl.BindTexture(GLEnum.Texture2D, (uint)(int)imDrawCmdPtr.TextureId);
                    _gl.DrawElementsBaseVertex(GLEnum.Triangles, imDrawCmdPtr.ElemCount, GLEnum.UnsignedShort,
                        (void*)(imDrawCmdPtr.IdxOffset * 2U), (int)imDrawCmdPtr.VtxOffset
                    );
                }
            }
        }
        
        _gl.DeleteVertexArray(_vertexArrayObject);
        _vertexArrayObject = 0U;
        _gl.UseProgram((uint)data2);
        _gl.BindTexture(GLEnum.Texture2D, (uint)data3);
        _gl.BindSampler(0U, (uint)data4);
        _gl.ActiveTexture((GLEnum)data1);
        _gl.BindVertexArray((uint)data6);
        _gl.BindBuffer(GLEnum.ArrayBuffer, (uint)data5);
        _gl.BlendEquationSeparate((GLEnum)data13, (GLEnum)data14);
        _gl.BlendFuncSeparate((GLEnum)data9, (GLEnum)data10, (GLEnum)data11, (GLEnum)data12);
        SetGlEnum(GLEnum.Blend, flag1);
        SetGlEnum(GLEnum.CullFace, flag2);
        SetGlEnum(GLEnum.DepthTest, flag3);
        SetGlEnum(GLEnum.StencilTest, flag4);
        SetGlEnum(GLEnum.ScissorTest, flag5);
        SetGlEnum(GLEnum.PrimitiveRestart, flag6);
        _gl.PolygonMode(GLEnum.FrontAndBack, (GLEnum)data7[0]);
        _gl.Scissor(data8[0], data8[1], (uint)data8[2], (uint)data8[3]);
    }
    
    private void SetGlEnum(GLEnum glEnum, bool value) {
        if(value)
            _gl.Enable(glEnum);
        else
            _gl.Disable(glEnum);
    }
    
    private void CreateDeviceResources() {
        _gl.GetInteger(GLEnum.TextureBinding2D, out int data1);
        _gl.GetInteger(GLEnum.ArrayBufferBinding, out int data2);
        _gl.GetInteger(GLEnum.VertexArrayBinding, out int data3);
        _shader = new GameEngine.Core.Rendering.Shaders.Shader(_gl,
            "#version 330\n        layout (location = 0) in vec2 Position;\n        layout (location = 1) in vec2 UV;\n        layout (location = 2) in vec4 Color;\n        uniform mat4 ProjMtx;\n        out vec2 Frag_UV;\n        out vec4 Frag_Color;\n        void main()\n        {\n            Frag_UV = UV;\n            Frag_Color = Color;\n            gl_Position = ProjMtx * vec4(Position.xy,0,1);\n        }",
            "#version 330\n        in vec2 Frag_UV;\n        in vec4 Frag_Color;\n        uniform sampler2D Texture;\n        layout (location = 0) out vec4 Out_Color;\n        void main()\n        {\n            Out_Color = Frag_Color * texture(Texture, Frag_UV.st);\n        }"
        );
        _attribLocationTex = _shader.GetUniformLocation("Texture"u8);
        _attribLocationProjMtx = _shader.GetUniformLocation("ProjMtx"u8);
        _attribLocationVtxPos = _shader.GetAttributeLocation("Position"u8);
        _attribLocationVtxUV = _shader.GetAttributeLocation("UV"u8);
        _attribLocationVtxColor = _shader.GetAttributeLocation("Color"u8);
        _vboHandle = _gl.GenBuffer();
        _elementsHandle = _gl.GenBuffer();
        RecreateFontDeviceTexture();
        _gl.BindTexture(GLEnum.Texture2D, (uint)data1);
        _gl.BindBuffer(GLEnum.ArrayBuffer, (uint)data2);
        _gl.BindVertexArray((uint)data3);
    }
    
    /// <summary>Creates the texture used to render text.</summary>
    private void RecreateFontDeviceTexture() {
        ImGuiIOPtr io = ImGui.GetIO();
        io.Fonts.GetTexDataAsRGBA32(out nint out_pixels, out int out_width, out int out_height, out var _);
        _gl.GetInteger(GLEnum.Texture2D, out int data);
        unsafe {
            _fontTexture = new Texture2D(_gl, (void*) out_pixels, (uint) out_width, (uint) out_height);
        }
        _fontTexture.Bind();
        _fontTexture.SetMagFilter(TextureMagFilter.Linear);
        _fontTexture.SetMinFilter(TextureMinFilter.Linear);
        io.Fonts.SetTexID((nint)_fontTexture.Id);
        _gl.BindTexture(GLEnum.Texture2D, (uint)data);
    }
    
    /// <summary>Frees all graphics resources used by the renderer.</summary>
    public void Dispose() {
        _view.Resize -= WindowResized;
        _keyboard.KeyChar -= OnKeyChar;
        _gl.DeleteBuffer(_vboHandle);
        _gl.DeleteBuffer(_elementsHandle);
        _gl.DeleteVertexArray(_vertexArrayObject);
        _fontTexture.Dispose();
        _shader.Dispose();
    }
}
