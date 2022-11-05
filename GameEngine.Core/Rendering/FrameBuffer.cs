using System;
using GameEngine.Core.Guard;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering;

// public class FrameBufferConfig {
//     public bool AutomaticResize { get; set; }
//     //todo: scale multiplier for resizing
//     public uint Width { get; set; }
//     public uint Height { get; set; }
// }

public class FrameBuffer : IDisposable {
    
    // public FrameBufferConfig Config { get; private set; }
    public uint ID { get; private set; }
    public uint ColorAttachment { get; private set; }
    public uint DepthAttachment { get; private set; }
    
    public uint Width { get; private set; }
    public uint Height { get; private set; }
    public bool AutomaticResize { get; }
    private readonly Renderer _rendererCtx;
    
    public FrameBuffer(Renderer rendererCtx, uint width, uint height, bool automaticResize) {
        _rendererCtx = rendererCtx;
        Width = width;
        Height = height;
        AutomaticResize = automaticResize;
        Update();
        if(AutomaticResize)
            rendererCtx.MainWindow.OnResize += Resize;
    }

    public void Bind() {
        _rendererCtx.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
    }

    public void Unbind() {
        _rendererCtx.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
    
    public void Update() {

        if(ID != 0) {
            Dispose();
        }
        
        ID = _rendererCtx.MainWindow.Gl.CreateFramebuffer();

        uint currentFrameBuffer = _rendererCtx.FinalFrameBuffer?.ID ?? 0;  //todo: which framebuffer is currently in use?
        
        _rendererCtx.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        
        ColorAttachment = CreateColorAttachment();
        DepthAttachment = CreateRenderBuffer();

        _rendererCtx.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, currentFrameBuffer);
    }

    public void Resize(uint width, uint height) {
        if(width <= 0 || height <= 0) {
            Console.LogWarning($"Trying to resize the frame buffer to width or height 0 or smaller");
            return;
        }
        Width = width;
        Height = height;
        Update();
    }
    
    public void Resize(int width, int height) {
        if(width <= 0 || height <= 0) {
            Console.LogWarning($"Trying to resize the frame buffer to width or height 0 or smaller");
            return;
        }
        Width = (uint) width;
        Height = (uint) height;
        Update();
    }

    private uint CreateColorAttachment() {
        uint colorAttachment = _rendererCtx.MainWindow.Gl.GenTexture();
        _rendererCtx.MainWindow.Gl.BindTexture(TextureTarget.Texture2D, colorAttachment);
        unsafe {
            _rendererCtx.MainWindow.Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
        }
        _rendererCtx.MainWindow.Gl.TextureParameterI(colorAttachment, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
        _rendererCtx.MainWindow.Gl.TextureParameterI(colorAttachment, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

        _rendererCtx.MainWindow.Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorAttachment, 0);
        return colorAttachment;
    }

    private uint CreateRenderBuffer() {
        uint depthAttachment = _rendererCtx.MainWindow.Gl.GenTexture();
        _rendererCtx.MainWindow.Gl.BindTexture(TextureTarget.Texture2D, depthAttachment);
        _rendererCtx.MainWindow.Gl.TexStorage2D(TextureTarget.Texture2D, 1, GLEnum.Depth24Stencil8, Width, Height);
        //unsafe {
        //    Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Depth24Stencil8, Config.Width, Config.Height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt, null);
        //}
        _rendererCtx.MainWindow.Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, depthAttachment, 0);
        
        Throw.If((FramebufferStatus) _rendererCtx.MainWindow.Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferStatus.FramebufferComplete, "Creation of framebuffer incomplete");
        
        _rendererCtx.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        return depthAttachment;
    }

    public void Dispose() {
        _rendererCtx.MainWindow.Gl.DeleteFramebuffer(ID);
        _rendererCtx.MainWindow.Gl.DeleteTextures(1, ColorAttachment);
        _rendererCtx.MainWindow.Gl.DeleteTextures(1, DepthAttachment);
    }
    
}
