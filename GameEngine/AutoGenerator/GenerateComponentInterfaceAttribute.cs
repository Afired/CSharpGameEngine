using System;

namespace GameEngine.AutoGenerator; 

[AttributeUsage(AttributeTargets.Class)]
public class GenerateComponentInterfaceAttribute : Attribute { }

//todo: generate Component interfaces as default if the class derives from component and is not abstract

//todo: waiting for c#11 for generic attributes
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequireComponent : Attribute {
    
    public RequireComponent(Type component) {
        
    }
    
    public RequireComponent(params Type[] components) {
        
    }
    
}

//todo: if you want to manually want to create the component interface, you can use this attribute
[AttributeUsage(AttributeTargets.Class)]
public class DoNotGenerateComponent : Attribute { }
