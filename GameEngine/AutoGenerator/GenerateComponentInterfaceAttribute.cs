using System;

namespace GameEngine.AutoGenerator;

//todo: generate Component interfaces as default if the class derives from component and is not abstract

//todo: convert to use generic attributes in c#11
[AttributeUsage(AttributeTargets.Class)]
public class RequireComponent : Attribute {
    
    public RequireComponent(Type component) {
        
    }
    
    public RequireComponent(params Type[] components) {
        
    }
    
}

//todo: if you want to manually want to create the component interface, you can use this attribute
[AttributeUsage(AttributeTargets.Class)]
public class DoNotGenerateComponentInterface : Attribute { }
