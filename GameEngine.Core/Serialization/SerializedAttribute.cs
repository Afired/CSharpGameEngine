using System;

namespace GameEngine.Core.Serialization; 

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class Serialized : Attribute {
    
}
