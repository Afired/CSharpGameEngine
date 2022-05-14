using System;
using System.Collections.Generic;

namespace GameEngine.SourceGenerator.Tracked.Tracking {
    
    internal static class NodeRegister {
        
        private static readonly List<NodeDefinition> _definitionsFromThis;
        private static readonly List<NodeDefinition> _definitionsFromOthers;

        public static NodeDefinition[] AllDefinitions;
        private static int _definitionsFromThisCount;
        
        internal static IEnumerable<NodeDefinition> EnumerateDefinitionsFromThisAssembly() {
            for(int i = 0; i < _definitionsFromThisCount; i++) {
                yield return AllDefinitions[i];
            }
        }
        
        internal static IEnumerable<NodeDefinition> EnumerateDefinitionsFromOtherAssembly() {
            for(int i = _definitionsFromThisCount + 1; i < AllDefinitions.Length; i++) {
                yield return AllDefinitions[i];
            }
        }
        
        static NodeRegister() {
            _definitionsFromThis = new List<NodeDefinition>();
            _definitionsFromOthers = new List<NodeDefinition>();
        }
        

        public static void Resolve() {
            FillArray();
            ClearLists();
            // we cant use foreach because we have to operate directly on the struct in the array and not a copy
//            foreach(ComponentInterfaceDefinition definition in AllDefinitions) {
//                definition.ResolveRequiredComponents();
//            }
            for(int i = 0; i < AllDefinitions.Length; i++) {
                AllDefinitions[i].ResolveRequiredComponents();
            }
        }
        
        private static void FillArray() {
            AllDefinitions = new NodeDefinition[_definitionsFromOthers.Count + _definitionsFromThis.Count];
            int i = 0;
            foreach(NodeDefinition definition in _definitionsFromThis) {
                AllDefinitions[i] = definition;
                i++;
            }
            _definitionsFromThisCount = i;
            foreach(NodeDefinition definition in _definitionsFromOthers) {
                AllDefinitions[i] = definition;
                i++;
            }
        }
        
        private static void ClearLists() {
            _definitionsFromOthers.Clear();
            _definitionsFromThis.Clear();
        }

        #region TryToGetDefinitionMethods
        
        public static bool TryToGetDefinition(string s1, Func<string, NodeDefinition, bool> compareFunc, out NodeDefinition nodeDefinition) {
            foreach(NodeDefinition definition in AllDefinitions) {
                if(compareFunc.Invoke(s1, definition)) {
                    nodeDefinition = definition;
                    return true;
                }
            }
            nodeDefinition = default;
            return false;
        }
        
        public static bool TryToGetDefinition(string s1, string s2, Func<string, string, NodeDefinition, bool> compareFunc, out NodeDefinition nodeDefinition) {
            foreach(NodeDefinition definition in AllDefinitions) {
                if(compareFunc.Invoke(s1, s2, definition)) {
                    nodeDefinition = definition;
                    return true;
                }
            }
            nodeDefinition = default;
            return false;
        }
        
        public static bool TryToGetDefinition(string s1, string s2, string s3, Func<string, string, string, NodeDefinition, bool> compareFunc, out NodeDefinition nodeDefinition) {
            foreach(NodeDefinition definition in AllDefinitions) {
                if(compareFunc.Invoke(s1, s2, s3, definition)) {
                    nodeDefinition = definition;
                    return true;
                }
            }
            nodeDefinition = default;
            return false;
        }

        #endregion
        
        public static void RegisterForThisAssembly(NodeDefinition nodeDefinition) {
            _definitionsFromThis.Add(nodeDefinition);
        }

        public static void RegisterForOtherAssembly(NodeDefinition nodeDefinition) {
            _definitionsFromOthers.Add(nodeDefinition);
        }
    }
    
}
