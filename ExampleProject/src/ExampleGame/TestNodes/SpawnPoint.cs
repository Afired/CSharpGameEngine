using GameEngine.Core.Nodes;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes; 

[Has<Transform3D>]
public partial class SpawnPoint : Transform3D {

    [Serialized] private MyValueFloat _myValueFloat;

}
