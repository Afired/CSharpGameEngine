# C# Game Engine
![C# 10](https://img.shields.io/badge/-C%23_10-b5faaa)
![.NET 6.0](https://img.shields.io/badge/-.NET_6.0-acfcf0)
![GLFW](https://img.shields.io/badge/-GLFW-e1aafa)
![OpenGL 3.3](https://img.shields.io/badge/-OpenGL_3.3-faaaaa)
![Box2D](https://img.shields.io/badge/-Box2D-fad5aa)
![ImGui](https://img.shields.io/badge/-ImGui-f6faaa)

## Editor
Import Assets, configure Nodes, composite Scenes, preview in Play Mode, serialize Data, reload Assemblies...

![EditorScreenshotCrop](https://user-images.githubusercontent.com/59524587/180666030-dd88ece8-1ac1-4d69-9a06-125f2855295f.png)

## Nodes
The Node System is a type save and reliable way of structuring code and creating logic. Thanks to the use of source generators the correct way of implementing the subclass sandbox pattern is handled automatically. Nodes are capable of inheritance and composition, making them both reusable and flexible at the same time. Even the Scene is a Node which can be inherited from, making it easy to create unique level logic/data or share similar ones.

```csharp
public partial class EnemySpawner : Node, Arr<SpawnPoint> {
    
    [Serialized] private float SpawnInterval { get; set; } = 0.1f;
    private float TimeUntilNextSpawn { get; set; } = 0f;
    
    protected override void OnUpdate() {
        if(TimeUntilNextSpawn > 0) {
            TimeUntilNextSpawn -= Time.DeltaTime;
            return;
        }
        TimeUntilNextSpawn = SpawnInterval;
        
        SpawnEnemy();
    }
    
    private void SpawnEnemy() {
        Enemy newEnemy = New<Enemy>();
        newEnemy.Position = SpawnPoints.GetRandom()?.Position ?? Vector3.Zero;
        Hierarchy.RegisterNode(newEnemy);
    }
    
}
```

## Physics
Nodes like Trigger, Collider or RigidBody come out of the box and provide 2D physics using Box 2D under the hood.

https://user-images.githubusercontent.com/59524587/180666532-f09d643a-d931-4a82-b2ce-78ea18661b86.mp4

## Extensibility
One of the main goals of this project is to create an engine which can be easily extended. Providing an intuitive API for extending the Editor or making Tools is therefore also high priority. This is achieved by loading custom editor assemblies, in which you can define Editor Windows, Property Drawers, Node Drawers and more to come..

```csharp
public class InspectorWindow : EditorWindow {
    
    public InspectorWindow() {
        Title = "Inspector";
    }
    
    protected override void Draw() {
        
        if(ImGui.BeginMenuBar()) {
            ImGui.Text(Selection.Current is not null ? Selection.Current.GetType().ToString() : "select an object to inspect");
            ImGui.EndMenuBar();
        }
        if(Selection.Current is null) {
            return;
        }
        
        if(Selection.Current is Node node)
            NodeDrawer.Draw(node);
    }
    
}
```
