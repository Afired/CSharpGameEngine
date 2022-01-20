using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Generator.Tracked.Tracking {
    
    internal class ComponentInterfaceRegister {

        private List<ComponentInterfaceDefinition> _componentInterfaceDefinitions;


        internal ComponentInterfaceRegister() {
            _componentInterfaceDefinitions = new List<ComponentInterfaceDefinition>();
        }

        internal void Add(ComponentInterfaceDefinition componentInterfaceDefinition) {
            _componentInterfaceDefinitions.Add(componentInterfaceDefinition);
        }

        internal string ComponentToInterface(string component) {
            return _componentInterfaceDefinitions.Find(componentInterfaceDefinition => componentInterfaceDefinition.ComponentName == component).InterfaceName;
        }
        
        internal string InterfaceToComponent(string @interface) {
            return _componentInterfaceDefinitions.Find(componentInterfaceDefinition => componentInterfaceDefinition.InterfaceName == @interface).ComponentName;
        }

        internal bool ContainsInterface(string @interface) {
            return _componentInterfaceDefinitions.Any(componentInterfaceDefinition => componentInterfaceDefinition.InterfaceName == @interface);
        }
        
    }
    
}
