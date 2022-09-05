using GameEngine.Core.Nodes;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes; 

public partial class SpawnPoint : Transform {

    [Serialized] private MyValueFloat _myValueFloat;

}
