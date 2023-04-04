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
    public uint Id { get; private set; }
    public uint ColorAttachment { get; private set; }
    public uint DepthBuffer { get; private set; }
    
    public uint Width { get; private set; }
    public uint Height { get; private set; }
    public bool AutomaticResize { get; }
    private readonly Renderer _rendererCtx;
    private readonly GL _gl;
    
    public FrameBuffer(GL gl, Renderer rendererCtx, uint width, uint height, bool automaticResize) {
        _rendererCtx = rendererCtx;
        Width = width;
        Height = height;
        AutomaticResize = automaticResize;
        
        
        uint originallyActiveFrameBuffer = _rendererCtx.FinalFrameBuffer?.Id ?? 0; //todo: which framebuffer is currently in use?
        
        
        _gl = gl;
        
        // Create a new framebuffer object
        uint framebuffer = gl.GenFramebuffer();
        
        // Bind the framebuffer object
        gl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
        
        // Create a new texture object to use as the color attachment
        uint colorTexture = gl.GenTexture();
        gl.BindTexture(TextureTarget.Texture2D, colorTexture);
        
        //TODO: was 'InternalFormat.Rgba8' before refactor
        // Set the desired width and height of the color attachment
        unsafe {
            gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
        }
        gl.TextureParameterI(colorTexture, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
        gl.TextureParameterI(colorTexture, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
        // Attach the texture object to the framebuffer as the color attachment
        gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorTexture, 0);
        
        // Create a new renderbuffer object to use as the depth attachment
        uint depthRenderbuffer = gl.GenRenderbuffer();
        gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthRenderbuffer);
        // Set the desired width and height of the depth attachment
        gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, width, height);
        // Attach the renderbuffer object to the framebuffer as the depth attachment
        gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthRenderbuffer);
        
        // Check that the framebuffer is complete
        FramebufferStatus status = (FramebufferStatus) gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        if (status != FramebufferStatus.FramebufferComplete) {
            Console.LogError(status.ToString());
        }
        
        // Unbind the framebuffer object
        gl.BindFramebuffer(FramebufferTarget.Framebuffer, originallyActiveFrameBuffer);

        Id = framebuffer;
        ColorAttachment = colorTexture;
        DepthBuffer = depthRenderbuffer;
        
        if(AutomaticResize)
            rendererCtx.MainWindow.OnResize += Resize;
    }

    public void Bind() {
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
    }

    public void Unbind() {
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
    
    public void Resize(int width, int height) {
        if(width <= 0 || height <= 0) {
            Console.LogWarning($"Trying to resize the frame buffer to width or height 0 or smaller");
            return;
        }
        Resize((uint) width, (uint) height);
    }
    
    public void Resize(uint width, uint height) {
        if(width <= 0 || height <= 0) {
            Console.LogWarning($"Trying to resize the frame buffer to width or height 0 or smaller");
            return;
        }
        Width = width;
        Height = height;
        
        if(Id != 0) {
            // only dispose attachments, because we can and will reuse the FBO
            _gl.DeleteTextures(1, ColorAttachment);
            _gl.DeleteRenderbuffer(DepthBuffer);
        }
        
        
        
        uint originallyActiveFrameBuffer = _rendererCtx.FinalFrameBuffer?.Id ?? 0; //todo: which framebuffer is currently in use?
        
        
        // Bind the existing framebuffer object
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
        
        // Create a new texture object to use as the color attachment
        uint colorTexture = _gl.GenTexture();
        _gl.BindTexture(TextureTarget.Texture2D, colorTexture);
        
        //TODO: was 'InternalFormat.Rgba8' before refactor
        // Set the desired width and height of the color attachment
        unsafe {
            _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
        }
        _gl.TextureParameterI(colorTexture, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
        _gl.TextureParameterI(colorTexture, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
        // Attach the texture object to the framebuffer as the color attachment
        _gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorTexture, 0);
        
        // Create a new renderbuffer object to use as the depth attachment
        uint depthRenderbuffer = _gl.GenRenderbuffer();
        _gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthRenderbuffer);
        // Set the desired width and height of the depth attachment
        _gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, width, height);
        // Attach the renderbuffer object to the framebuffer as the depth attachment
        _gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthRenderbuffer);
        
        // Check that the framebuffer is complete
        FramebufferStatus status = (FramebufferStatus) _gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        if (status != FramebufferStatus.FramebufferComplete) {
            Console.LogError(status.ToString());
        }
        
        // Unbind the framebuffer object
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, originallyActiveFrameBuffer);
        
        Id = Id;
        ColorAttachment = colorTexture;
        DepthBuffer = depthRenderbuffer;
    }
    
    private uint CreateColorAttachment() {
        uint colorAttachment = _gl.GenTexture();
        _gl.BindTexture(TextureTarget.Texture2D, colorAttachment);
        unsafe {
            _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
        }
        _gl.TextureParameterI(colorAttachment, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
        _gl.TextureParameterI(colorAttachment, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

        _gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorAttachment, 0);
        return colorAttachment;
    }

    private uint CreateDepthAttachment() {
        uint depthAttachment = _gl.GenTexture();
        _gl.BindTexture(TextureTarget.Texture2D, depthAttachment);
        _gl.TexStorage2D(TextureTarget.Texture2D, 1, GLEnum.Depth24Stencil8, Width, Height);
        //unsafe {
        //    Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Depth24Stencil8, Config.Width, Config.Height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt, null);
        //}
        _gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, depthAttachment, 0);
        
        Throw.If((FramebufferStatus) _gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferStatus.FramebufferComplete, "Creation of framebuffer incomplete");
        
//        _rendererCtx.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        return depthAttachment;
    }

    public void Dispose() {
        _gl.DeleteFramebuffer(Id);
        _gl.DeleteTextures(1, ColorAttachment);
        _gl.DeleteRenderbuffer(DepthBuffer);
    }
    
}
