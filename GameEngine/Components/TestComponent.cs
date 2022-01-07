using GameEngine.AutoGenerator;
using GameEngine.Entities;

namespace GameEngine.Components;

// before we directly compared the actual strings which means this is not being detected: [GameEngine.AutoGenerator.GenerateComponentInterface]
// now we directly look if the attribute string contains the name
//todo: even this would work because we check for the actual string containing the name: [Something.Blablabla.GenerateComponentInterface.Blabla]

//todo: limit attribute should be possible to set for abstract classes
[RequireComponent(typeof(ITransform), typeof(ITestComponent))]
public class TestComponent : Component {
    public TestComponent(Entity entity) : base(entity) { }
}
