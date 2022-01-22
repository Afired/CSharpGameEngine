using System;
using System.Collections.Generic;

namespace GameEngine.Generator.Tracked.Tracking {
    
    internal struct ComponentInterfaceDefinition {
        
        internal readonly string Namespace;
        internal readonly string InterfaceName;
        internal readonly string ComponentName;
        private string[] _requiredComponentsNamespacesAndNames;
        private int[] _requiredComponentsIndices;

        internal bool HasRequiredComponents => _requiredComponentsIndices.Length != 0;
        internal IEnumerable<ComponentInterfaceDefinition> GetAllRequiredComponents() {
            HashSet<ComponentInterfaceDefinition> hashSet = new HashSet<ComponentInterfaceDefinition>();
            foreach(int index in _requiredComponentsIndices) {
                IfNotIncludedAddToHashSetElseReturn(hashSet, ComponentInterfaceRegister.AllDefinitions[index]);
            }
            return hashSet;
        }

        private static void IfNotIncludedAddToHashSetElseReturn(HashSet<ComponentInterfaceDefinition> hashSet, ComponentInterfaceDefinition definition) {
            if(hashSet.Contains(definition))
                return;
            hashSet.Add(definition);
        }
        
        internal ComponentInterfaceDefinition(string @namespace, string interfaceName, string componentName, string[] requiredComponentsNamespacesAndNames) {
            Namespace = @namespace;
            InterfaceName = interfaceName;
            ComponentName = componentName;
            _requiredComponentsNamespacesAndNames = requiredComponentsNamespacesAndNames ?? Array.Empty<string>();
            _requiredComponentsIndices = Array.Empty<int>();
        }

        internal void ResolveRequiredComponents() {
            List<int> requiredComponentsIndices = new List<int>(_requiredComponentsNamespacesAndNames.Length);
            foreach(string current in _requiredComponentsNamespacesAndNames) {
                ExtractNamespaceAndName(current, out string @namespace, out string componentName);
                if(TryGetIndexFromMatchingNamespaceAndComponentName(@namespace, componentName, out int index))
                    requiredComponentsIndices.Add(index);
            }
            _requiredComponentsIndices = requiredComponentsIndices.ToArray();
            _requiredComponentsNamespacesAndNames = null;
        }
        
        private static void ExtractNamespaceAndName(string namespaceAndName, out string @namespace, out string name) {
            int lastIndexOfDot = namespaceAndName.LastIndexOf('.');
            @namespace = namespaceAndName.Substring(0, lastIndexOfDot);
            name = namespaceAndName.Substring(lastIndexOfDot + 1);
        }
        
        private static bool TryGetIndexFromMatchingNamespaceAndComponentName(string @namespace, string componentName, out int index) {
            index = 0;
            foreach(ComponentInterfaceDefinition definition in ComponentInterfaceRegister.AllDefinitions) {
                if(definition.Namespace == @namespace && definition.ComponentName == componentName)
                    return true;
                index++;
            }
            return false;
        }
        
    }

    internal static class ComponentInterfaceDefinitionExtension {
        
        internal static string NamespaceAsFileScopedText(this ComponentInterfaceDefinition definition) {
            return string.IsNullOrEmpty(definition.Namespace) ? string.Empty : $"namespace {definition.Namespace};";
        }
        
        internal static string ComponentWithNamespace(this ComponentInterfaceDefinition definition) {
            return string.IsNullOrEmpty(definition.Namespace) ? definition.ComponentName : $"{definition.Namespace}.{definition.ComponentName}";
        }
        
        internal static string InterfaceWithNamespace(this ComponentInterfaceDefinition definition) {
            return string.IsNullOrEmpty(definition.Namespace) ? definition.InterfaceName : $"{definition.Namespace}.{definition.InterfaceName}";
        }
        
    }
    
}
