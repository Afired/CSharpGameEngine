using System;
using System.IO;
using GLFW;
using OpenGL;
using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using Console = GameEngine.Debugging.Console;
using GL = Silk.NET.OpenGL.GL;
using Image = SixLabors.ImageSharp.Image;

namespace GameEngine.Rendering.Textures;

public class Texture2D : Texture {

    private int _width;
    private int _height;
    private uint _textureID;

    public Texture2D(string path) {
        using var image = Image.Load(path);
        _width = image.Width;
        _height = image.Height;

        GL gl = GL.GetApi(Glfw.GetProcAddress);
        gl.CreateTextures(TextureTarget.Texture2D, 1, out _textureID);
        gl.TextureStorage2D(_textureID, 1, SizedInternalFormat.Rgb8, (uint) _width, (uint) _height);
        
        gl.TextureParameterI(_textureID, TextureParameterName.TextureMinFilter, OpenGL.GL.GL_NEAREST);
        gl.TextureParameterI(_textureID, TextureParameterName.TextureMagFilter, OpenGL.GL.GL_NEAREST);
        
        unsafe {

            byte[] arr = image.ToArray(PngFormat.Instance);
            
            fixed(byte* b = arr) {
                gl.TextureSubImage2D(_textureID, 0, 0, 0, (uint) _width, (uint) _height, PixelFormat.Rgb, PixelType.Byte, b);
            }
        }
             
        image.Dispose();
    }
    
    ~Texture2D() {
        GL gl = GL.GetApi(Glfw.GetProcAddress);
        gl.DeleteTexture(_textureID);
    }

    public override void Bind(uint slot = 0) {
        GL gl = GL.GetApi(Glfw.GetProcAddress);
        gl.BindTextureUnit(slot, _textureID);
    }
    
}

public static class ImageSharpExtensions
{
    public static byte[] ToArray(this Image image, IImageFormat imageFormat)
    {
        using (var memoryStream = new MemoryStream())
        {
            var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(imageFormat);
            image.Save(memoryStream, imageEncoder);
            return memoryStream.ToArray();
        }
    }
}
