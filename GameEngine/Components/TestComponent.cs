using GameEngine.AutoGenerator;
using GameEngine.Entities;

namespace GameEngine.Components;

//todo: this doesnt work because it compares the actual string which is being used: [GameEngine.AutoGenerator.GenerateComponentInterface]
[GenerateComponentInterface]
public class TestComponent : Component {
    public TestComponent(Entity entity) : base(entity) { }
}
