using GameEngine.Core;
using GameEngine.Debugging;
using Silk.NET.OpenGL;
using GL = OpenGL.GL;

namespace GameEngine.Rendering; 

public class FrameBuffer {

    public uint ID { get; private set; }
    public uint TextureColorBuffer { get; private set; }

    public FrameBuffer() {
        SetupFrameBuffers();
    }
    
    private void SetupFrameBuffers() {
        // create and bind frame buffer object
        ID = GL.glGenFramebuffer();
        GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, ID);
        
        // attach textures buffer object
        TextureColorBuffer = GL.glGenTexture();
        GL.glBindTexture(GL.GL_TEXTURE_2D, TextureColorBuffer);
        unsafe {
            if(Configuration.UseHDR)
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int) InternalFormat.Rgb16f, Configuration.WindowWidth, Configuration.WindowHeight, 0, (int) PixelFormat.Rgb, GL.GL_FLOAT, null);
            else
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int) InternalFormat.Rgb, Configuration.WindowWidth, Configuration.WindowHeight, 0, (int) PixelFormat.Rgb, GL.GL_UNSIGNED_BYTE, null);
        }
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
        GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
        GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_COLOR_ATTACHMENT0, GL.GL_TEXTURE_2D, TextureColorBuffer, 0);
        
        // attach depth stencil buffer
        //unsafe {
        //    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_DEPTH24_STENCIL8, 800, 600, 0, GL.GL_DEPTH_STENCIL, GL.GL_UNSIGNED_INT_24_8, null);
        //}
        //GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, texture, 0); 
        
        // attach render buffer object
        uint rbo = GL.glGenRenderbuffer();
        GL.glBindRenderbuffer(rbo);
        GL.glRenderbufferStorage(GL.GL_RENDERBUFFER, GL.GL_DEPTH24_STENCIL8, Configuration.WindowWidth, Configuration.WindowHeight);
        
        GL.glBindRenderbuffer(0);

        GL.glFramebufferRenderbuffer(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_RENDERBUFFER, rbo);
        
        //check for framebuffer status
        if(GL.glCheckFramebufferStatus(GL.GL_FRAMEBUFFER) == GL.GL_FRAMEBUFFER_COMPLETE)
            Console.LogSuccess("Framebuffer setup succeeded");
        else
            Console.LogError("Framebuffer is not complete");
        
        // bind default frame buffer to render to
        GL.glBindFramebuffer(GL.GL_FRAMEBUFFER, 0);
    }
}
