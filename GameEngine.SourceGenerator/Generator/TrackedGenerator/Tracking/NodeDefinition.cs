using System;
using System.Collections.Generic;

namespace GameEngine.SourceGenerator.Tracked.Tracking {
    
    internal struct NodeDefinition {
        
        internal readonly string Namespace;
        internal readonly string InterfaceName;
        internal readonly string ClassName;
        private string[] _baseTypesNamespacesAndNames;
        private int[] _childNodesIndices;
        
        internal bool HasChildNodes => _childNodesIndices.Length != 0;
        internal IEnumerable<NodeDefinition> GetAllChildNodes() {
            HashSet<NodeDefinition> hashSet = new HashSet<NodeDefinition>();
            foreach(int index in _childNodesIndices) {
                IfNotIncludedAddToHashSetElseReturn(hashSet, NodeRegister.AllDefinitions[index]);
            }
            return hashSet;
        }
        
        private static void IfNotIncludedAddToHashSetElseReturn(HashSet<NodeDefinition> hashSet, NodeDefinition definition) {
            if(hashSet.Contains(definition))
                return;
            hashSet.Add(definition);
        }
        
        internal NodeDefinition(string @namespace, string className, string interfaceName, string[] baseTypesNamespacesAndNames) {
            Namespace = @namespace;
            InterfaceName = interfaceName;
            ClassName = className;
            _baseTypesNamespacesAndNames = baseTypesNamespacesAndNames ?? Array.Empty<string>();
            _childNodesIndices = Array.Empty<int>();
        }
        
        internal void ResolveRequiredComponents() {
            List<int> requiredComponentsIndices = new List<int>(_baseTypesNamespacesAndNames.Length);
            foreach(string current in _baseTypesNamespacesAndNames) {
                
                if(!IsValid(current))
                    continue;
                /*
                ExtractNamespaceAndName(current, out string @namespace, out string componentName);
                
                if(TryGetIndexFromMatchingNamespaceAndNodeName(@namespace, componentName, out int index))
                    requiredComponentsIndices.Add(index);
                */

                if(TryGetIndexFromMatchingNodeName(current, out int index)) {
                    requiredComponentsIndices.Add(index);
                }
            }
            _childNodesIndices = requiredComponentsIndices.ToArray();
            _baseTypesNamespacesAndNames = null;
        }
        
        private static bool IsValid(string namespaceAndName) {
            if(string.IsNullOrEmpty(namespaceAndName)) return false;
            if(namespaceAndName == "?") return false;
            return true;
        }
        
        private static void ExtractNamespaceAndName(string namespaceAndName, out string @namespace, out string name) {
            
            int lastIndexOfDot = namespaceAndName.LastIndexOf('.');
            
            if(lastIndexOfDot == -1) {
                @namespace = "";
                name = namespaceAndName;
                return;
            }
            
            @namespace = namespaceAndName.Substring(0, lastIndexOfDot);
            name = namespaceAndName.Substring(lastIndexOfDot + 1);
        }
        
        private static bool TryGetIndexFromMatchingNamespaceAndNodeName(string @namespace, string nodeName, out int index) {
            index = 0;
            foreach(NodeDefinition definition in NodeRegister.AllDefinitions) {
                if(definition.Namespace == @namespace && definition.ClassName == nodeName)
                    return true;
                index++;
            }
            return false;
        }
        
        private static bool TryGetIndexFromMatchingNodeName(string nodeName, out int index) {
            index = 0;
            foreach(NodeDefinition definition in NodeRegister.AllDefinitions) {
                if(definition.ClassName == nodeName)
                    return true;
                index++;
            }
            return false;
        }
        
    }
    
    internal static class ComponentInterfaceDefinitionExtension {
        
        internal static string NamespaceAsFileScopedText(this NodeDefinition definition) {
            return string.IsNullOrEmpty(definition.Namespace) ? string.Empty : $"namespace {definition.Namespace};";
        }
        
    }
    
}
