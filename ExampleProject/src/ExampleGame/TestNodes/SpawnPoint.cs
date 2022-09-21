using GameEngine.Core.Nodes;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes; 

public partial class SpawnPoint : Transform3D {

    [Serialized] private MyValueFloat _myValueFloat;

}
