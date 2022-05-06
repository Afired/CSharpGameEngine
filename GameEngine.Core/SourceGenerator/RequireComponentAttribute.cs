using System;

namespace GameEngine.Core.SourceGenerator;

//todo: generate Component interfaces as default if the class derives from component and is not abstract
//todo: convert to use generic attributes in c#11
[AttributeUsage(AttributeTargets.Class)]
public class RequireComponent : Attribute {
    
    public RequireComponent(Type component) { }
    public RequireComponent(params Type[] components) { }
    
}
