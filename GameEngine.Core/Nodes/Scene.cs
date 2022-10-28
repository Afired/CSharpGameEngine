using GameEngine.Core.Physics;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes; 

public partial class Scene : Node, Arr<Node?> {
    
    [Serialized] public string Name { get; private set; } = "New Scene";
    protected sealed override bool AwakeThisNodeBeforeItsChildren => true;
    
    protected override void OnAwake() {
        base.OnAwake();
        Application.Instance.PhysicsEngine.InitializeWorld();
    }
    
}


public partial class MySpecialSceneNode : Scene, Has<Transform3D>, Arr<Transform3D> {
    
}
