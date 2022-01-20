namespace GameEngine.Generator.Tracked.Tracking {
    
    // use records once source generators targeting version gets updated 
    internal struct ComponentInterfaceDefinition {
        
        internal readonly string NameSpace;
        internal readonly string InterfaceName;
        internal readonly string ComponentName;
        internal readonly ComponentInterfaceDefinition[] RequiredComponents;
        
        
        internal ComponentInterfaceDefinition(string nameSpace, string interfaceName, string componentName, ComponentInterfaceDefinition[] requiredComponents) {
            NameSpace = nameSpace;
            InterfaceName = interfaceName;
            ComponentName = componentName;
            RequiredComponents = requiredComponents;
        }
        
    }
    
}
