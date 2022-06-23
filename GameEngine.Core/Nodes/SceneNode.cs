using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes; 

public partial class SceneNode : Node, Arr<Node?> {
    
    [Serialized] public string Name { get; init; }
    
}


public partial class MySpecialSceneNode : SceneNode, Has<Transform>, Arr<Transform> {
    
}
