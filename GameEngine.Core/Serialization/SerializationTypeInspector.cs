using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace GameEngine.Core.Serialization; 

public class SerializationTypeInspector : TypeInspectorSkeleton {
    
    private readonly ITypeInspector _innerTypeDescriptor;
    
    public SerializationTypeInspector(ITypeInspector innerTypeDescriptor) {
        _innerTypeDescriptor = innerTypeDescriptor;
    }
    
    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object? container) {
        var props = _innerTypeDescriptor.GetProperties(type, container);
        props = props.Where(p => p.GetCustomAttribute<Serialized>() is not null);
        return props;
    }
    
}
