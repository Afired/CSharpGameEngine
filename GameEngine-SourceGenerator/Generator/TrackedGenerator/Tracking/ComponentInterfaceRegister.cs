using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Generator.Tracked.Tracking {
    
    internal static class ComponentInterfaceRegister {
        
        private static List<ComponentInterfaceDefinition> _definitionsFromThis;
        private static List<ComponentInterfaceDefinition> _definitionsFromOthers;

        public static ComponentInterfaceDefinition[] AllDefinitions;
        private static int _definitionsFromThisCount;
        private static int _definitionsFromOthersCount;
        
        internal static IEnumerable<ComponentInterfaceDefinition> EnumerateDefinitionsFromThisAssembly() {
            for(int i = 0; i < _definitionsFromThisCount; i++) {
                yield return AllDefinitions[i];
            }
        }
        
        internal static IEnumerable<ComponentInterfaceDefinition> EnumerateDefinitionsFromOtherAssembly() {
            for(int i = _definitionsFromThisCount + 1; i < AllDefinitions.Length; i++) {
                yield return AllDefinitions[i];
            }
        }
        
        static ComponentInterfaceRegister() {
            _definitionsFromThis = new List<ComponentInterfaceDefinition>();
            _definitionsFromOthers = new List<ComponentInterfaceDefinition>();
        }
        

        public static void Resolve() {
            FillArray();
            ClearLists();
            foreach(ComponentInterfaceDefinition definition in AllDefinitions) {
                definition.ResolveRequiredComponents();
            }
        }
        
        private static void FillArray() {
            AllDefinitions = new ComponentInterfaceDefinition[_definitionsFromOthers.Count + _definitionsFromThis.Count];
            int i = 0;
            foreach(ComponentInterfaceDefinition definition in _definitionsFromOthers) {
                AllDefinitions[i] = definition;
                i++;
            }
            foreach(ComponentInterfaceDefinition definition in _definitionsFromThis) {
                AllDefinitions[i] = definition;
                i++;
            }
        }
        
        private static void ClearLists() {
            _definitionsFromOthers = null;
            _definitionsFromThis = null;
        }

        #region TryToGetDefinitionMethods
        
        public static bool TryToGetDefinition(string s1, Func<string, ComponentInterfaceDefinition, bool> compareFunc, out ComponentInterfaceDefinition componentInterfaceDefinition) {
            foreach(ComponentInterfaceDefinition definition in AllDefinitions) {
                if(compareFunc.Invoke(s1, definition)) {
                    componentInterfaceDefinition = definition;
                    return true;
                }
            }
            componentInterfaceDefinition = default;
            return false;
        }
        
        public static bool TryToGetDefinition(string s1, string s2, Func<string, string, ComponentInterfaceDefinition, bool> compareFunc, out ComponentInterfaceDefinition componentInterfaceDefinition) {
            foreach(ComponentInterfaceDefinition definition in AllDefinitions) {
                if(compareFunc.Invoke(s1, s2, definition)) {
                    componentInterfaceDefinition = definition;
                    return true;
                }
            }
            componentInterfaceDefinition = default;
            return false;
        }
        
        public static bool TryToGetDefinition(string s1, string s2, string s3, Func<string, string, string, ComponentInterfaceDefinition, bool> compareFunc, out ComponentInterfaceDefinition componentInterfaceDefinition) {
            foreach(ComponentInterfaceDefinition definition in AllDefinitions) {
                if(compareFunc.Invoke(s1, s2, s3, definition)) {
                    componentInterfaceDefinition = definition;
                    return true;
                }
            }
            componentInterfaceDefinition = default;
            return false;
        }

        #endregion
        
        public static void RegisterForThisAssembly(ComponentInterfaceDefinition componentInterfaceDefinition) {
            _definitionsFromThis.Add(componentInterfaceDefinition);
        }

        public static void RegisterForOtherAssembly(ComponentInterfaceDefinition componentInterfaceDefinition) {
            _definitionsFromOthers.Add(componentInterfaceDefinition);
        }
    }
    
}
