using GameEngine.Core.AutoGenerator;
using GameEngine.Core.Entities;

namespace GameEngine.Core.Components;

// before we directly compared the actual strings which means this is not being detected: [GameEngine.AutoGenerator.GenerateComponentInterface]
// now we directly look if the attribute string contains the name
//todo: even this would work because we check for the actual string containing the name: [Something.Blablabla.GenerateComponentInterface.Blabla]

[RequireComponent(typeof(Transform))]
public partial class ExampleComponent : Component {
    
    // Awake Callback
    protected override void OnAwake() {
        base.OnAwake();
    }
    
    // Update Callback
    protected override void OnUpdate() {
        base.OnUpdate();
    }
    
    private void GetReferenceToRequiredComponent() {
        {
            // normally you would use this long expression to convert the generic entity this component is attached to to a different required component
            Transform _ = (Entity as ITransform).Transform;
            // ...
        }
        {
            // because we know that a required component always has to be implemented by the entity the component is attached to, we can suppress the warning implicitly
            Transform _ = (Entity as ITransform)!.Transform;
            // ...
        }
        {
            // to shorten this long expression and to avoid making mistakes when implicitly suppressing a warning of a component that is not required anymore, we can use the auto generated helper properties
            Transform _ = Transform;
            // ...
        }
    }
    
    private void GetReferenceToOptionalComponent() {
        {
            // references to optional components of an entity should never be retrieved like this, because it would crash instantly if the Entity doesnt have the component
            Renderer _ = (Entity as IRenderer).Renderer;
            // ...
        }
        {
            // this is even worse equally as bad, because you only suppress the warning but it will crash the same way as the example above
            Renderer _ = (Entity as IRenderer)!.Renderer;
        }
        {
            // instead use the is keyword to validate
            if(Entity is IRenderer iRenderer) {
                Renderer _ = iRenderer.Renderer;
                // ...
            } else {
                // ...
            }
        }
        
    }
    
}
