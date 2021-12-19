using System.Runtime.InteropServices;
using GLFW;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace GameEngine.Rendering.Textures;

public class Texture2D : Texture {
    
    private uint _width;
    private uint _height;
    private uint _textureID;
    
    public unsafe Texture2D( string path) {
        GL gl = GL.GetApi(Glfw.GetProcAddress);
        
        //Loading an image using imagesharp.
        Image<Rgba32> img = (Image<Rgba32>) Image.Load(path);
        _width = (uint) img.Width;
        _height = (uint) img.Height;
        
        // OpenGL has image origin in the bottom-left corner.
        fixed (void* data = &MemoryMarshal.GetReference(img.GetPixelRowSpan(0))) {
            //Loading the actual image.
            Load(gl, data, _width, _height);
        }

        //Deleting the img from imagesharp.
        img.Dispose();
    }
    
    private unsafe void Load(GL gl, void* data, uint width, uint height) {
        
        //Generating the opengl handle;
        _textureID = gl.GenTexture();
        Bind();

        //Setting the data of a texture.
        gl.TexImage2D(TextureTarget.Texture2D, 0, (int) InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        //Setting some texture perameters so the texture behaves as expected.
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) GLEnum.Repeat);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) GLEnum.Repeat);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GLEnum.Nearest); // linear, nearest
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GLEnum.Nearest);
        
        //Generating mipmaps.
        //gl.GenerateMipmap(TextureTarget.Texture2D);
    }
    
    ~Texture2D() {
        Dispose();
    }

    public void Dispose() {
        GL gl = GL.GetApi(Glfw.GetProcAddress);
        gl.DeleteTexture(_textureID);
    }

    public override void Bind(uint slot = 0) {
        GL gl = GL.GetApi(Glfw.GetProcAddress);
        
        //When we bind a texture we can choose which textureslot we can bind it to.
        TextureUnit textureSlot = TextureUnit.Texture0;
        gl.ActiveTexture((TextureUnit)slot);
        gl.BindTexture(TextureTarget.Texture2D, _textureID);
    }
    
}
