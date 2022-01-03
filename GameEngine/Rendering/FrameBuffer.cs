using System;
using GameEngine.Guard;
using Silk.NET.OpenGL;

namespace GameEngine.Rendering;

public class FrameBufferConfig {
    public uint Width { get; set; }
    public uint Height { get; set; }
}

public class FrameBuffer : IDisposable {
    
    public FrameBufferConfig Config { get; private set; }
    public uint ID { get; private set; }
    public uint ColorAttachment { get; private set; }
    public uint DepthAttachment { get; private set; }
    
    
    public FrameBuffer(FrameBufferConfig config) {
        Update(config);
    }

    public void Bind() {
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
    }

    public void Unbind() {
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
    
    public void Update(FrameBufferConfig config) {

        if(ID != 0) {
            Dispose();
        }
        
        
        Config = config;
        
        ID = Gl.CreateFramebuffer();
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        
        ColorAttachment = CreateColorAttachment();
        DepthAttachment = CreateRenderBuffer();

        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    public void Resize(uint width, uint height) {
        Config.Width = width;
        Config.Height = height;
        Update(Config);
    }

    private uint CreateColorAttachment() {
        uint colorAttachment = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2D, colorAttachment);
        unsafe {
            Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, Config.Width, Config.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, null);
        }
        Gl.TextureParameterI(colorAttachment, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
        Gl.TextureParameterI(colorAttachment, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

        Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorAttachment, 0);
        return colorAttachment;
    }

    private uint CreateRenderBuffer() {
        uint depthAttachment = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2D, depthAttachment);
        Gl.TexStorage2D(TextureTarget.Texture2D, 1, GLEnum.Depth24Stencil8, Config.Width, Config.Height);
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
