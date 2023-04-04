using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes; 

[Arr<Node>]
public partial class Scene : Node {
    
    [Serialized] public string Name { get; private set; } = "New Scene";
    protected sealed override bool AwakeThisBeforeItsChildren => true;
    
    protected override void OnAwake() {
        base.OnAwake();
        Application.Instance.PhysicsEngine.InitializeWorld();
    }
    
}


[Has<Transform3D>]
[Arr<Node>]
public partial class MySpecialSceneNode : Scene {
    
    protected override void OnAwake() {
        base.OnAwake();
    }
    
}
