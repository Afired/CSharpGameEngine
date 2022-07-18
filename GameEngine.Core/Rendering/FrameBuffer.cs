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
    
    
    public FrameBuffer(uint width, uint height, bool automaticResize) {
        Width = width;
        Height = height;
        AutomaticResize = automaticResize;
        Update();
        if(AutomaticResize)
            GlfwWindow.OnResize += Resize;
    }

    public void Bind() {
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
    }

    public void Unbind() {
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
    
    public void Update() {

        if(ID != 0) {
            Dispose();
        }
        
        ID = Gl.CreateFramebuffer();

        uint currentFrameBuffer = Renderer.FinalFrameBuffer?.ID ?? 0;  //todo: which framebuffer is currently in use?
        
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        
        ColorAttachment = CreateColorAttachment();
        DepthAttachment = CreateRenderBuffer();

        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, currentFrameBuffer);
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
        uint colorAttachment = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2D, colorAttachment);
        unsafe {
            Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
        }
        Gl.TextureParameterI(colorAttachment, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
        Gl.TextureParameterI(colorAttachment, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

        Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorAttachment, 0);
        return colorAttachment;
    }

    private uint CreateRenderBuffer() {
        uint depthAttachment = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2D, depthAttachment);
        Gl.TexStorage2D(TextureTarget.Texture2D, 1, GLEnum.Depth24Stencil8, Width, Height);
        //unsafe {
        //    Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Depth24Stencil8, Config.Width, Config.Height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt, null);
        //}
        Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, depthAttachment, 0);
        
        Throw.If((FramebufferStatus) Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferStatus.FramebufferComplete, "Creation of framebuffer incomplete");
        
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        return depthAttachment;
    }

    public void Dispose() {
        Gl.DeleteFramebuffer(ID);
        Gl.DeleteTextures(1, ColorAttachment);
        Gl.DeleteTextures(1, DepthAttachment);
    }
    
}
