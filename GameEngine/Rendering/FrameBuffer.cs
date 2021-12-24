using GameEngine.Core;
using Silk.NET.OpenGL;

namespace GameEngine.Rendering; 

public class FrameBuffer {

    public uint ID { get; private set; }
    public uint TextureColorBuffer { get; private set; }
    private GL GL => RenderingEngine.Gl;

    public FrameBuffer() {
        SetupFrameBuffers();
    }
    
    private void SetupFrameBuffers() {
        // create and bind frame buffer object
        ID = GL.GenFramebuffer();
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        
        // attach textures buffer object
        TextureColorBuffer = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, TextureColorBuffer);
        unsafe {
            if(Configuration.UseHDR)
                GL.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb16f, Configuration.WindowWidth, Configuration.WindowHeight, 0, PixelFormat.Rgb, PixelType.Float, null);
            else
                GL.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, Configuration.WindowWidth, Configuration.WindowHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);
        }
        GL.TextureParameterI(TextureColorBuffer, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear);
        GL.TextureParameterI(TextureColorBuffer, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
        GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, TextureColorBuffer, 0);
        
        // attach depth stencil buffer
        //unsafe {
        //    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_DEPTH24_STENCIL8, 800, 600, 0, GL.GL_DEPTH_STENCIL, GL.GL_UNSIGNED_INT_24_8, null);
        //}
        //GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, texture, 0); 
        
        // attach render buffer object
        uint rbo = GL.GenRenderbuffer();
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
        GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, Configuration.WindowWidth, Configuration.WindowHeight);
        
        GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

        GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);
        
        //check for framebuffer status
        if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == GLEnum.FramebufferComplete)
            Console.LogSuccess("Framebuffer setup succeeded");
        else
            Console.LogError("Framebuffer is not complete");
        
        // bind default frame buffer to render to
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
}
