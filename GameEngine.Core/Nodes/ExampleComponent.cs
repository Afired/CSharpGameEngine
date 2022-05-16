using System.Collections.Generic;

namespace GameEngine.Core.Nodes;

// before we directly compared the actual strings which means this is not being detected: [GameEngine.AutoGenerator.GenerateComponentInterface]
// now we directly look if the attribute string contains the name
//todo: even this would work because we check for the actual string containing the name: [Something.Blablabla.GenerateComponentInterface.Blabla]

public partial class ExampleComponent : Node, ITransform {
    
    //autogenerated
    
//    public Trigger Trigger { get; init; } = null!;
//    public Renderer Renderer { get; init; } = null!;
//    
//    public ExampleComponent(Node? parent = null) : this(parent, out List<Node> childNodes) {
//        ChildNodes = childNodes;
//    }
//    
//    public ExampleComponent(Node? parentNode, out List<Node> childNodes) : base(parentNode, out childNodes) {
//        Trigger = new Trigger(this);
//        Renderer = new Renderer(this);
//        
//        childNodes.Add(Trigger);
//        childNodes.Add(Renderer);
//    }
//    
//    [JsonConstructor]
//    protected ExampleComponent(bool isJsonConstructed) : base(isJsonConstructed) { }
    
}
