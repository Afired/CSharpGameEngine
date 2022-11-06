using System.Diagnostics;
using GameEngine.Core;
using GameEngine.Core.Layers;
using GameEngine.Core.Physics;
using GameEngine.Core.Rendering;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;
using GameEngine.Editor.PropertyDrawers;
using ImGuiNET;

namespace GameEngine.Editor;

public class EditorApplication : Application<EditorApplication> {
    
    internal EditorLayer EditorLayer { get; private set; }
    
    public EditorApplication(Configuration config) : base(config) {
        EditorAssetManager.Init();
        // CompileExternalAssemblies();
        //base();
        ImGui.LoadIniSettingsFromDisk("ImGui");
        EditorResources.Load();
        EditorLayer = new EditorLayer();
        Renderer.LayerStack.Push(EditorLayer, LayerType.Overlay);
        EditorGui editorGui = new();
    }
    
    public override void Terminate() {
        base.Terminate();
        ImGui.SaveIniSettingsToDisk("ImGui");
    }
    
    protected override void CompileExternalAssemblies() {
        // base.CompileExternalAssemblies();
        if(Project.Current is not null) {
            foreach(string externalGameAssemblyDirectory in Project.Current.GetExternalGameAssemblyDirectories()) {
                CompileExternalAssembly(externalGameAssemblyDirectory, new DotnetBuildProperty[] {
                    new("GameEngineCoreDLL", System.Reflection.Assembly.GetAssembly(typeof(GameEngine.Core.Application))!.Location),
                    new("GamEngineSourceGeneratorDLL", Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(GameEngine.Core.Application))!.Location) + @"\..\..\..\..\GameEngine.SourceGenerator\bin\Debug\netstandard2.0\GameEngine.SourceGenerator.dll"),
                    new("ProjectRoot", Project.Current.ProjectDirectory),
                });
            }
        
            foreach(string externalEditorAssemblyDirectory in Project.Current.GetExternalEditorAssemblyDirectories()) {
                CompileExternalAssembly(externalEditorAssemblyDirectory, new DotnetBuildProperty[] {
                    new("GameEngineCoreDLL", System.Reflection.Assembly.GetAssembly(typeof(GameEngine.Core.Application))!.Location),
                    new("GameEngineEditorDLL", System.Reflection.Assembly.GetAssembly(typeof(GameEngine.Editor.EditorApplication))!.Location),
                    new("ProjectRoot", Project.Current.ProjectDirectory),
                });
            }
        }
    }
    
    public override void LoadExternalAssemblies() {

        if(Project.Current is not null) {
            // _ealcm.LoadExternalAssembly(EXTERNAL_EDITOR_ASSEMBLY_DLL, true);
            foreach(string externalEditorAssemblyName in Project.Current.GetExternalEditorAssemblyNames()) {
                AssemblyLoadContextManager.LoadExternalAssembly(Project.Current.ProjectDirectory + @"\bin\Debug\net6.0\" + externalEditorAssemblyName + ".dll", true);
            }
        
            // _ealcm.LoadExternalAssembly(EXTERNAL_ASSEMBLY_DLL, true);
            foreach(string externalGameAssemblyName in Project.Current.GetExternalGameAssemblyNames()) {
                AssemblyLoadContextManager.LoadExternalAssembly(Project.Current.ProjectDirectory + @"\bin\Debug\net6.0\" + externalGameAssemblyName + ".dll", true);
            }
        }
        Serializer.LoadAssemblyIfNotLoadedAlready();
        
        AssemblyLoadContextManager.AddUnloadTask(() => {
            Hierarchy.SaveCurrentRootNode();
            Hierarchy.Clear();
            Renderer.SetActiveCamera(null);
            Serializer.UnloadResources();
            ClearTypeDescriptorCache();
            PhysicsEngine.World = null;
            return true;
        });
        
        
        PropertyDrawer.GenerateLookUp();
        AssemblyLoadContextManager.AddUnloadTask(() => {
            Selection.Clear();
            PropertyDrawer.ClearLookUp();
            return true;
        });
    }
    
    protected override unsafe void Loop() {
        Stopwatch updateTimer = new();
        Stopwatch physicsTimer = new();
        updateTimer.Start();
        physicsTimer.Start();
        
        while(IsRunning) {
            
            if(IsReloadingExternalAssemblies)
                ReloadExternalAssemblies();

            ExecuteQueuedTasks();
            
            float updateTime = (float) updateTimer.Elapsed.TotalSeconds;
            if(Application.Instance!.Config.TargetFrameRate > 0) {
                TimeSpan timeOut = TimeSpan.FromSeconds(1 / Application.Instance!.Config.TargetFrameRate - updateTime);
                if(timeOut.TotalSeconds > 0) {
                    Thread.Sleep(timeOut);
                    updateTime = (float) updateTimer.Elapsed.TotalSeconds;
                }
            }
            Time.TotalTimeElapsed += updateTime;
            updateTimer.Restart();
            
            if(PlayMode.Current == PlayMode.Mode.Playing) {
                Hierarchy.Awake();
                Hierarchy.Update(updateTime);
            } else
                EditorCamera.Instance.EditorUpdate(updateTime);
            
            Renderer.InputHandler.ResetMouseDelta(Renderer.WindowHandle);
            
            float physicsTime = (float) physicsTimer.Elapsed.TotalSeconds;
            if(physicsTime > Application.Instance!.Config.FixedTimeStep) {
                if(PlayMode.Current == PlayMode.Mode.Playing) {
                    Hierarchy.PrePhysicsUpdate();
                    PhysicsEngine.DoStep(Config.FixedTimeStep);
                    Hierarchy.PhysicsUpdate(Config.FixedTimeStep);
                }
                physicsTimer.Restart();
            }
            
            Renderer.Render();
            
            // handle input
            Renderer.MainWindow.Glfw.PollEvents();
            Renderer.InputHandler.HandleMouseInput(Renderer.WindowHandle);
            
            if(Renderer.MainWindow.Glfw.WindowShouldClose(Renderer.WindowHandle))
                Terminate();
        }
        
    }
    
    public override void Dispose() {
        base.Dispose();
        EditorResources.Unload();
    }
    
}
