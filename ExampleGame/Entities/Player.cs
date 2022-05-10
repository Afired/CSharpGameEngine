using ExampleGame.Components;
using GameEngine.Core.Components;
using GameEngine.Core.Entities;

namespace ExampleGame.Entities; 

public partial class Player : Node, ITransform, IRenderer, IPlayerControls, IMovable, ITrigger, IBlaster {
    
}
