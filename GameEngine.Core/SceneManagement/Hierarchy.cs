using System.Collections.Generic;
using System.IO;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Nodes;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.SceneManagement; 

public static class Hierarchy {
    
    private static readonly Stack<(Node node, INodeArr nodeArr)> RegisteredNodes = new();
    public static AssetRef<Node>? CurrentlyLoadedNodeRef { get; private set; }
    public static Node? RootNode { get; private set; }
    
//    public static void SetRootNode(Node? newRootNode) {
//        RootNode = newRootNode;
//    }
    
    public static void Open(AssetRef<Node> assetRef) {
        string? assetPath = AssetManager.Instance.GetAssetPath(assetRef.Guid);
        if(assetPath is null) {
            Console.LogWarning($"Can't open node with guid {assetRef.Guid}, because it failed to resolve to a valid asset path!");
            return;
        }
        Node node = Serializer.DeserializeNode(assetPath);
        RootNode = node;
        CurrentlyLoadedNodeRef = assetRef;
    }
    
    public static void Close() {
        RootNode = null;
        CurrentlyLoadedNodeRef = null;
    }
    
    public static void Clear() {
        RootNode = null;
        RegisteredNodes.Clear();
    }
    
    public static void Awake() {
        
        while(RegisteredNodes.TryPop(out (Node node, INodeArr nodeArr) entry)) {
            entry.nodeArr.Add(entry.node);
        }
        
        if(RootNode is null)
            return;
        
        RootNode.Awake();
    }
    
    public static void Update(float elapsedTime) {
        if(RootNode is null)
            return;
        
        Time.DeltaTime = elapsedTime;
        RootNode.Update();
    }
    
    public static void PrePhysicsUpdate() {
        if(RootNode is null)
            return;
        RootNode.PrePhysicsUpdate();
    }
    
    public static void PhysicsUpdate(float physicsTimeStep) {
        if(RootNode is null)
            return;
        Time.PhysicsTimeStep = physicsTimeStep;
        RootNode.PhysicsUpdate();
    }

    internal static void Draw() {
        if(RootNode is null)
            return;
        RootNode.Draw();
    }
    
    public static void RegisterNode(Node node, INodeArr nodeArr) {
        RegisteredNodes.Push((node, nodeArr));
    }
    
    public static void SaveCurrentRootNode() {
        if(RootNode is null) {
            Console.LogWarning($"There is nothing loaded in Hierarchy, therefore saving is skipped!");
            return;
        }

        if(CurrentlyLoadedNodeRef is null) {
            Console.LogWarning($"There is no AssetRef defined in Hierarchy, therefore saving is skipped!");
            return;
        }
        
        string? nodeAssetPath = AssetManager.Instance.GetAssetPath(CurrentlyLoadedNodeRef.Value.Guid);
        
        if(nodeAssetPath is null) {
            Console.LogWarning($"Could not save node of type {RootNode.GetType()} because there is no asset path defined");
            return;
        }
        
        File.WriteAllText(nodeAssetPath, Serializer.SerializeNode(RootNode));
        Console.LogSuccess($"Saved node of type {RootNode.GetType()} to {nodeAssetPath}");
    }
    
    //public static string? CurrentlyLoadedNodesAssetPath { get; set; } //TODO: replace with managed asset reference
    
}
