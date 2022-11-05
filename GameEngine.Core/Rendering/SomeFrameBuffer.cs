using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering; 

public class SomeFrameBuffer {

    public uint ID { get; private set; }
    public uint TextureColorBuffer { get; private set; }
    private readonly Renderer _renderCtx;
    
    public SomeFrameBuffer(Renderer renderCtx) {
        _renderCtx = renderCtx;
        SetupFrameBuffers();
    }
    
    private void SetupFrameBuffers() {
        // create and bind frame buffer object
        ID = _renderCtx.MainWindow.Gl.GenFramebuffer();
        _renderCtx.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        
        // attach textures buffer object
        TextureColorBuffer = _renderCtx.MainWindow.Gl.GenTexture();
        _renderCtx.MainWindow.Gl.BindTexture(TextureTarget.Texture2D, TextureColorBuffer);
        unsafe {
            if(Application.Instance!.Config.UseHDR)
                _renderCtx.MainWindow.Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb16f, Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight, 0, PixelFormat.Rgb, PixelType.Float, null);
            else
                _renderCtx.MainWindow.Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgb, Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight, 0, PixelFormat.Rgb, PixelType.UnsignedByte, null);
        }
        _renderCtx.MainWindow.Gl.TextureParameterI(TextureColorBuffer, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear);
        _renderCtx.MainWindow.Gl.TextureParameterI(TextureColorBuffer, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
        _renderCtx.MainWindow.Gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, TextureColorBuffer, 0);
        
        // attach depth stencil buffer
        //unsafe {
        //    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, GL.GL_DEPTH24_STENCIL8, 800, 600, 0, GL.GL_DEPTH_STENCIL, GL.GL_UNSIGNED_INT_24_8, null);
        //}
        //GL.glFramebufferTexture2D(GL.GL_FRAMEBUFFER, GL.GL_DEPTH_STENCIL_ATTACHMENT, GL.GL_TEXTURE_2D, texture, 0); 
        
        // attach render buffer object
        uint rbo = _renderCtx.MainWindow.Gl.GenRenderbuffer();
        _renderCtx.MainWindow.Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, rbo);
        _renderCtx.MainWindow.Gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.Depth24Stencil8, Application.Instance!.Config.WindowWidth, Application.Instance!.Config.WindowHeight);
        
        _renderCtx.MainWindow.Gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

        _renderCtx.MainWindow.Gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, rbo);
        
        //check for framebuffer status
        if(_renderCtx.MainWindow.Gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer) == GLEnum.FramebufferComplete)
            Console.LogSuccess("Framebuffer setup succeeded");
        else
            Console.LogError("Framebuffer is not complete");
        
        // bind default frame buffer to render to
        _renderCtx.MainWindow.Gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
}
