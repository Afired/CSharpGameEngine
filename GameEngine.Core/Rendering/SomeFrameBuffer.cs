using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering; 

public class SomeFrameBuffer {

    public uint ID { get; private set; }
    public uint TextureColorBuffer { get; private set; }
    
    public SomeFrameBuffer(Renderer renderer) {
        // create and bind frame buffer object
        ID = renderer.MainWindow.Gl.GenFramebuffer();
        renderer.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        
        // attach textures buffer object
        TextureColorBuffer = renderer.MainWindow.Gl.GenTexture();
        renderer.MainWindow.Gl.BindTexture(TextureTarget.Texture2D, TextureColorBuffer);
        unsafe {
            if(Application.Instance!.Config.UseHDR)
                renderer.MainWindow.Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb16f, Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight, 0, PixelFormat.Rgb, PixelType.Float, null);
            else
                renderer.MainWindow.Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);
        }
        renderer.MainWindow.Gl.TextureParameterI(TextureColorBuffer, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear);
        renderer.MainWindow.Gl.TextureParameterI(TextureColorBuffer, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
        renderer.MainWindow.Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, TextureColorBuffer, 0);
        
        // attach depth stencil buffer
        //unsafe {
        //    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_DEPTH24_STENCIL8, 800, 600, 0, GL.GL_DEPTH_STENCIL, GL.GL_UNSIGNED_INT_24_8, null);
        //}
        //GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, texture, 0); 
        
        // attach render buffer object
        uint rbo = renderer.MainWindow.Gl.GenRenderbuffer();
        renderer.MainWindow.Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
        renderer.MainWindow.Gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight);
        
        renderer.MainWindow.Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

        renderer.MainWindow.Gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);
        
        //check for framebuffer status
        if(renderer.MainWindow.Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == GLEnum.FramebufferComplete)
            Console.LogSuccess("Framebuffer setup succeeded");
        else
            Console.LogError("Framebuffer is not complete");
        
        // bind default frame buffer to render to
        renderer.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
    
}
