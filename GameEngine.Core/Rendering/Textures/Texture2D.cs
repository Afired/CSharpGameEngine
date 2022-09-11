using System;
using System.IO;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace GameEngine.Core.Rendering.Textures;

public class Texture2D : Texture {
    
    public static Texture2D MissingTexture { get; internal set; }
    public uint Width { get; init; }
    public uint Height { get; init; }
    public uint ID { get; private set; }
    
    public unsafe Texture2D(string path) {
        using var stream = File.OpenRead(path);
        ImageInfo? info = ImageInfo.FromStream(stream);
        if(!info.HasValue)
            throw new Exception();
        Width = (uint) info.Value.Width;
        Height = (uint)info.Value.Height;
//            info.Value.ColorComponents;
//            info.Value.BitsPerChannel;
            
        ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            
        fixed(void* data = image.Data) {
            Load(Gl, data, Width, Height);
        }
    }
    
    public unsafe Texture2D(void* data, uint width, uint height) {
        Width = width;
        Height = height;
        Load(Gl, data, Width, Height);
    }
    
    internal static unsafe Texture2D CreateMissingTexture() {
        fixed(void* data = new byte[] {
                  204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255,
                  0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255,
                  204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255,
                  0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255
              }) {
            return new Texture2D(data, 4, 4);
        }
    }
    
    private unsafe void Load(GL gl, void* data, uint width, uint height) {
        
        //Generating the opengl handle;
        ID = gl.GenTexture();
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
        //TODO: Gl.DeleteTexture(ID);
    }

    public override void Bind(uint slot = 0) {
        //When we bind a texture we can choose which textureslot we can bind it to.
        TextureUnit textureSlot = TextureUnit.Texture0;
        Gl.ActiveTexture((TextureUnit)slot);
        Gl.BindTexture(TextureTarget.Texture2D, ID);
    }
    
}
