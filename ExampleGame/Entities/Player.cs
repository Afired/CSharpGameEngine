using ExampleGame.Components;
using GameEngine.Components;
using GameEngine.Entities;

namespace ExampleGame.Entities; 

public partial class Player : Entity, ITransform, IQuad, IRenderer, IPlayerControls, IMovable, ITrigger, IBlaster {
    
}
