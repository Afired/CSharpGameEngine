using GameEngine.Core;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;
using NativeFileDialogSharp;
using YamlDotNet.Serialization;

namespace GameEngine.Editor; 

public class Project {
    
    public static Project? Current { get; private set; }
    public string ProjectFilePath { get; }
    public string ProjectDirectory { get; }
    public string ProjectAssetDirectory { get; }
    
    private Project(string projFilePath) {
        string? projectDirectory = Path.GetDirectoryName(projFilePath);
        if(projectDirectory is null)
            throw new Exception();

        ProjectFilePath = projFilePath;
        ProjectDirectory = projectDirectory;
        ProjectAssetDirectory = projectDirectory + @"\Assets";
    }
    
    public static void Open(string projFilePath) {
        if(Path.GetExtension(projFilePath) != ".geproj") {
            Console.LogWarning($"Trying to open project, but linked file is not of type .geproj");
            return;
        }
        
        string? projectPath = Path.GetDirectoryName(projFilePath);
        
        if(string.IsNullOrEmpty(projectPath))
            throw new Exception();
        
        Current = new Project(projFilePath);
        
        // Reload Assemblies
        EditorApplication.Instance.RegisterReloadOfExternalAssemblies();
        
        AssetDatabase.Reload(Application.Instance);
    }
    
    public static void OpenProjectWithFileExplorer() {
        DialogResult dialogResult = Dialog.FileOpen("geproj", null);
        if(dialogResult.IsCancelled)
            return;
        if(dialogResult.IsError)
            throw new Exception(dialogResult.ErrorMessage);
        Open(dialogResult.Path);
    }
    
    public string[] GetExternalGameAssemblyDirectories() {
        string projectFileString = File.ReadAllText(ProjectFilePath);
        Deserializer deserializer = new();
        ProjectSettings projectSettings = deserializer.Deserialize<ProjectSettings>(projectFileString);

        List<string> projectDirectories = new(projectSettings.GameAssemblies.Length);
        foreach(string gameAssemblyCsProjPath in projectSettings.GameAssemblies) {
            string absoluteGameAssemblyCsProjPath = Path.Combine(ProjectDirectory, gameAssemblyCsProjPath);
            
            if(Path.GetExtension(absoluteGameAssemblyCsProjPath) != ".csproj") {
                Console.LogWarning("linked game assembly is not .csproj or is invalid");
                continue;
            }
            string gameAssemblyDirectory = Path.GetDirectoryName(absoluteGameAssemblyCsProjPath)!;
            projectDirectories.Add(gameAssemblyDirectory);
        }
        return projectDirectories.ToArray();
    }
    
    public string[] GetExternalEditorAssemblyDirectories() {
        string projectFileString = File.ReadAllText(ProjectFilePath);
        Deserializer deserializer = new();
        ProjectSettings projectSettings = deserializer.Deserialize<ProjectSettings>(projectFileString);

        List<string> projectDirectories = new(projectSettings.EditorAssemblies.Length);
        foreach(string editorAssemblyCsProjPath in projectSettings.EditorAssemblies) {
            string absoluteEditorAssemblyCsProjPath = Path.Combine(ProjectDirectory, editorAssemblyCsProjPath);
            
            if(Path.GetExtension(absoluteEditorAssemblyCsProjPath) != ".csproj") {
                Console.LogWarning("linked editor assembly is not .csproj or is invalid");
                continue;
            }
            string editorAssemblyDirectory = Path.GetDirectoryName(absoluteEditorAssemblyCsProjPath)!;
            projectDirectories.Add(editorAssemblyDirectory);
        }
        return projectDirectories.ToArray();
    }

    public string[] GetExternalGameAssemblyNames() {
        string projectFileString = File.ReadAllText(ProjectFilePath);
        Deserializer deserializer = new();
        ProjectSettings projectSettings = deserializer.Deserialize<ProjectSettings>(projectFileString);

        List<string> projectNames = new(projectSettings.GameAssemblies.Length);
        foreach(string gameAssemblyCsProjPath in projectSettings.GameAssemblies) {
            string absoluteGameAssemblyCsProjPath = Path.Combine(ProjectDirectory, gameAssemblyCsProjPath);
            
            if(Path.GetExtension(absoluteGameAssemblyCsProjPath) != ".csproj") {
                Console.LogWarning("linked game assembly is not .csproj or is invalid");
                continue;
            }
            string gameAssemblyName = Path.GetFileNameWithoutExtension(absoluteGameAssemblyCsProjPath)!;
            projectNames.Add(gameAssemblyName);
        }
        return projectNames.ToArray();
    }
    
    public string[] GetExternalEditorAssemblyNames() {
        string projectFileString = File.ReadAllText(ProjectFilePath);
        Deserializer deserializer = new();
        ProjectSettings projectSettings = deserializer.Deserialize<ProjectSettings>(projectFileString);

        List<string> projectNames = new(projectSettings.EditorAssemblies.Length);
        foreach(string editorAssemblyCsProjPath in projectSettings.EditorAssemblies) {
            string absoluteEditorAssemblyCsProjPath = Path.Combine(ProjectDirectory, editorAssemblyCsProjPath);
            
            if(Path.GetExtension(absoluteEditorAssemblyCsProjPath) != ".csproj") {
                Console.LogWarning("linked editor assembly is not .csproj or is invalid");
                continue;
            }
            string editorAssemblyName = Path.GetFileNameWithoutExtension(absoluteEditorAssemblyCsProjPath)!;
            projectNames.Add(editorAssemblyName);
        }
        return projectNames.ToArray();
    }
    
}

public class ProjectSettings {
    public string[] GameAssemblies;
    public string[] EditorAssemblies;
}
