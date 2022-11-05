using System;
using System.IO;
using GameEngine.Core.AssetManagement;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace GameEngine.Core.Rendering.Textures;

public class Texture2D : IAsset {
    
    public uint Width { get; private set; }
    public uint Height { get; private set; }
    public uint ID { get; private set; }
    
    public static Texture2D Missing { get; }
    
    static Texture2D() {
        Missing = CreateMissing();
    }
    
    private Texture2D() { }
    
    private static unsafe Texture2D CreateMissing() {
        fixed(void* data = new byte[] {
                  204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255,
                  0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255,
                  204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255,
                  0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255
              }) {
            return Texture2D.Create(data, 4, 4);
        }
    }
    
    public static unsafe Texture2D? Create(string path) {
        Texture2D newTexture = new Texture2D();
        
        using var stream = File.OpenRead(path);
        ImageInfo? info = ImageInfo.FromStream(stream);
        if(!info.HasValue)
            throw new Exception();
        newTexture.Width = (uint) info.Value.Width;
        newTexture.Height = (uint)info.Value.Height;
//            info.Value.ColorComponents;
//            info.Value.BitsPerChannel;
        
        StbImage.stbi_set_flip_vertically_on_load(1);
        ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        
        fixed(void* data = image.Data) {
            newTexture.Load(Application.Instance!.Renderer.MainWindow.Gl, data, newTexture.Width, newTexture.Height);
        }
        return newTexture;
    }
    
    public static unsafe Texture2D? Create(void* data, uint width, uint height) {
        Texture2D newTexture = new Texture2D();
        newTexture.Width = width;
        newTexture.Height = height;
        newTexture.Load(Application.Instance!.Renderer.MainWindow.Gl, data, width, height);
        return newTexture;
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
    
    public void Bind(uint slot = 0) {
        //When we bind a texture we can choose which texture slot we can bind it to.
        if(slot > 31)
            Console.LogWarning($"Can't assign texture to texture slot {slot}");
        
        Application.Instance!.Renderer.MainWindow.Gl.ActiveTexture((TextureUnit) slot + 33984);
        Application.Instance!.Renderer.MainWindow.Gl.BindTexture(TextureTarget.Texture2D, ID);
    }
    
}
