using GameEngine.Entities;

namespace GameEngine.Components; 

[GenerateComponentInterface]
public class TestComponent : Component {
    public TestComponent(Entity entity) : base(entity) { }
}
