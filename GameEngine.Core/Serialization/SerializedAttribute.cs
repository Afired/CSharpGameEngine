using System;

namespace GameEngine.Core.Serialization; 

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class Serialized : Attribute {
    
    public readonly Editor Editor;
    
    public Serialized(Editor editor = Editor.Edit) {
        Editor = editor;
    }
    
}

public enum Editor {
    Edit,
    Hidden,
    Readonly
}
