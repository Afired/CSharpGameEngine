using System;

namespace GameEngine.Core.SourceGenerator; 

//todo: if you want to manually want to create the component interface, you can use this attribute
[AttributeUsage(AttributeTargets.Class)]
public class DoNotGenerateComponentInterface : Attribute { }
