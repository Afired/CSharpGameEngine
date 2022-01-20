namespace GameEngine.Generator.Tracked.Tracking {
    
    // use records once source generators targeting version gets updated 
    internal struct ComponentInterfaceDefinition {
        
        internal readonly string Namespace;
        internal readonly string InterfaceName;
        internal readonly string ComponentName;
        internal readonly ComponentInterfaceDefinition[] RequiredComponents;
        
        
        internal ComponentInterfaceDefinition(string @namespace, string interfaceName, string componentName, ComponentInterfaceDefinition[] requiredComponents) {
            Namespace = @namespace;
            InterfaceName = interfaceName;
            ComponentName = componentName;
            RequiredComponents = requiredComponents;
        }
        
    }
    
}
