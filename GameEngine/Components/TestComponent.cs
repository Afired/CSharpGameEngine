using GameEngine.Entities;
using GameEngine.Components; // for now, because we dont include the namespace: GameEngine.Components

namespace GameEngine.Components; 

[GenerateComponentInterface]
public class TestComponent : Component {
    public TestComponent(Entity entity) : base(entity) { }
}
