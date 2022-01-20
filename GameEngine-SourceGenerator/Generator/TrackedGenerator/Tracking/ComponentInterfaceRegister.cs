using System.Collections.Generic;

namespace GameEngine.Generator.Tracked.Tracking {
    
    internal class ComponentInterfaceRegister {
        
        internal List<ComponentInterfaceDefinition> _componentInterfaceDefinitions { get; private set; }

        internal void Add(ComponentInterfaceDefinition componentInterfaceDefinition) {
            _componentInterfaceDefinitions.Add(componentInterfaceDefinition);
        }
        
    }
    
}
