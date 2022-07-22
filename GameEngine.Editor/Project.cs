using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;

namespace GameEngine.Editor; 

public class Project {
    
    public static Project? Current { get; private set; }
    public string ProjectDirectory { get; }
    public string ProjectAssetDirectory { get; }
    
    public Project(string projectDirectory) {
        ProjectDirectory = projectDirectory;
        ProjectAssetDirectory = projectDirectory + @"\Assets";
    }
    
    public static void Open(string projFilePath) {
        //TODO: make sure its a file with extension .geproj
        string? projectPath = Path.GetDirectoryName(projFilePath);
        
        if(string.IsNullOrEmpty(projectPath))
            throw new Exception();
        
        Current = new Project(projectPath);
        EditorApplication.Instance.RegisterReloadOfExternalAssemblies();
        TextureRegister.Reload();
        ShaderRegister.Reload();
        MeshRegister.Reload();
    }
    
}
