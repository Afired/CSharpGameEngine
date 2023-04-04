using System;
using System.IO;
using GameEngine.Core.AssetManagement;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace GameEngine.Core.Rendering.Textures;

public class Texture2D : IAsset {
    
    public uint Width { get; private set; }
    public uint Height { get; private set; }
    public uint Id { get; private set; }
    private readonly GL _gl;
    
    private static Texture2D? _missingTexture2D;
    public static Texture2D GetMissingTexture2D(GL gl) {
        return _missingTexture2D ??= CreateMissingTexture2D(gl);
    }
    
    private static unsafe Texture2D CreateMissingTexture2D(GL gl) {
        fixed(void* data = new byte[] {
                  204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255,
                  0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255,
                  204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255,
                  0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255
              }) {
            return new Texture2D(gl, data, 4, 4);
        }
    }
    
    public unsafe Texture2D(GL gl, string path) {
        _gl = gl;
        using var stream = File.OpenRead(path);
        ImageInfo? info = ImageInfo.FromStream(stream);
        if(!info.HasValue)
            throw new Exception();
        Width = (uint) info.Value.Width;
        Height = (uint)info.Value.Height;
//            info.Value.ColorComponents;
//            info.Value.BitsPerChannel;
        
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        
        fixed(void* data = image.Data) {
            Load(data, Width, Height);
        }
    }

    public unsafe Texture2D(GL gl, void* data, uint width, uint height) {
        _gl = gl;
        Width = width;
        Height = height;
        Load(data, width, height);
    }
    
    private unsafe void Load(void* data, uint width, uint height) {
        
        //Generating the opengl handle;
        Id = _gl.GenTexture();
        Bind();
        
        //Setting the data of a texture.
        _gl.TexImage2D(TextureTarget.Texture2D, 0, (int) InternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        //Setting some texture perameters so the texture behaves as expected.
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) GLEnum.Repeat);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) GLEnum.Repeat);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GLEnum.Linear); // linear, nearest
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GLEnum.Linear);
        
        //Generating mipmaps.
        //gl.GenerateMipmap(TextureTarget.Texture2D);
    }
    
    public void Bind(uint slot = 0) {
        //When we bind a texture we can choose which texture slot we can bind it to.
        if(slot > 31)
            Console.LogWarning($"Can't assign texture to texture slot {slot}");
        
        _gl.ActiveTexture((TextureUnit) slot + 33984);
        _gl.BindTexture(TextureTarget.Texture2D, Id);
    }
    
    public void SetMagFilter(TextureMagFilter magFilter) {
        _gl.TextureParameter(Id, TextureParameterName.TextureMagFilter, (int) magFilter);
    }
    
    public void SetMinFilter(TextureMinFilter minFilter) {
        _gl.TextureParameter(Id, TextureParameterName.TextureMinFilter, (int) minFilter);
    }
    
    public void Dispose() {
        _gl.DeleteTexture(Id);
    }
    
}
