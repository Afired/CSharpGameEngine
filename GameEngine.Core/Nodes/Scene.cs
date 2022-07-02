using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes; 

public partial class Scene : Node, Arr<Node?> {
    
    [Serialized] public string Name { get; private set; } = string.Empty;
    
}


public partial class MySpecialSceneNode : Scene, Has<Transform>, Arr<Transform> {
    
}
