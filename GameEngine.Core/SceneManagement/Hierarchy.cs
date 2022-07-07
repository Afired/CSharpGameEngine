using System.Collections.Generic;
using GameEngine.Core.Nodes;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.SceneManagement; 

public static class Hierarchy {
    
    
    private static readonly Stack<(Node node, INodeArr nodeArr)> RegisteredNodes = new();
    public static Node? RootNode { get; private set; }
    
    public static void SetRootNode(Node? newRootNode) {
        RootNode = newRootNode;
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
        
        Time.TotalTimeElapsed += elapsedTime;
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
        if(RootNode is null)
            return;
        Serializer.Serialize(RootNode, "Test");
    }
    
}
