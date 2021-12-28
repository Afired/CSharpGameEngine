using GameEngine.Core;
using Silk.NET.OpenGL;

namespace GameEngine.Rendering; 

public class SomeFrameBuffer {

    public uint ID { get; private set; }
    public uint TextureColorBuffer { get; private set; }

    public SomeFrameBuffer() {
        SetupFrameBuffers();
    }
    
    private void SetupFrameBuffers() {
        // create and bind frame buffer object
        ID = Gl.GenFramebuffer();
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        
        // attach textures buffer object
        TextureColorBuffer = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2D, TextureColorBuffer);
        unsafe {
            if(Configuration.UseHDR)
                Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb16f, Configuration.WindowWidth, Configuration.WindowHeight, 0, PixelFormat.Rgb, PixelType.Float, null);
            else
                Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, Configuration.WindowWidth, Configuration.WindowHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);
        }
        Gl.TextureParameterI(TextureColorBuffer, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear);
        Gl.TextureParameterI(TextureColorBuffer, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
        Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, TextureColorBuffer, 0);
        
        // attach depth stencil buffer
        //unsafe {
        //    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_DEPTH24_STENCIL8, 800, 600, 0, GL.GL_DEPTH_STENCIL, GL.GL_UNSIGNED_INT_24_8, null);
        //}
        //GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, texture, 0); 
        
        // attach render buffer object
        uint rbo = Gl.GenRenderbuffer();
        Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
        Gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, Configuration.WindowWidth, Configuration.WindowHeight);
        
        Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

        Gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);
        
        //check for framebuffer status
        if(Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == GLEnum.FramebufferComplete)
            Console.LogSuccess("Framebuffer setup succeeded");
        else
            Console.LogError("Framebuffer is not complete");
        
        // bind default frame buffer to render to
        Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
}
