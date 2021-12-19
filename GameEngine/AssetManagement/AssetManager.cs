using System.IO;
using GameEngine.Core;

namespace GameEngine.AssetManagement; 

public static class AssetManager {

    public static string[] GetAllTexturePaths() {
        string folderPath = Path.Join(Configuration.RootPath, Configuration.AssetFolderName, Configuration.TextureFolderName);
        MakeSureFolderPathExists(folderPath);
        string[] paths = Directory.GetFiles(folderPath, "*.png", SearchOption.TopDirectoryOnly);
        return paths;
    }
    
    public static string[] GetAllShaderPaths() {
        string folderPath = Path.Join(Configuration.RootPath, Configuration.AssetFolderName, Configuration.ShaderFolderName);
        MakeSureFolderPathExists(folderPath);
        string[] paths = Directory.GetFiles(folderPath, "*.glsl", SearchOption.TopDirectoryOnly);
        return paths;
    }

    public static void MakeSureFolderPathExists(string folderPath) {
        Directory.CreateDirectory(folderPath);
    }

}
