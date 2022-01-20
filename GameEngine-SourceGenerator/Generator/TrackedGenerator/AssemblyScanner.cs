using System.Collections.Generic;
using System.Linq;
using GameEngine.Generator.Extensions;
using GameEngine.Generator.Tracked.Tracking;
using Microsoft.CodeAnalysis;

namespace GameEngine.Generator.Tracked {
    
    //reference: https://stackoverflow.com/questions/68055210/generate-source-based-on-other-assembly-classes-c-source-generator
    internal static class AssemblyScanner {
        
        private const string COMPONENT_BASECLASS_NAME = "Component";
        private const string DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME = "DoNotGenerateComponentInterface";
        private const string REQUIRE_COMPONENT_ATTRIBUTE_NAME = "RequireComponent";
        
        internal static void RetrieveAdditionalComponentInterfacesFromOtherAssemblies(GeneratorExecutionContext context, ComponentInterfaceRegister componentInterfaceRegister) {
            
            // if it doesnt reference the assembly just return
            if(!context.Compilation.SourceModule.ReferencedAssemblySymbols.Any(q => q.Identity.Name == "GameEngine"))
                return;
            
            // retrieve referenced assembly symbols
            IAssemblySymbol assemblySymbol = context.Compilation.SourceModule.ReferencedAssemblySymbols.FirstOrDefault(q => q.Identity.Name == "GameEngine");
            /*
            // use assembly symbol to get namespace and type symbols
            // all members in namespace Core.Entities
            var namespaces = assemblySymbol.GlobalNamespace.GetAllNamespaces();

            foreach(INamespaceSymbol namespaceSymbol in namespaces) {

                var members = namespaceSymbol.ConstituentNamespaces[0].
                    GetTypeMembers().
                    Where(m => m.TypeKind == TypeKind.Class).
                    Where(m => ClassSymbolExtensions.IsDerivedFromType(m, COMPONENT_BASECLASS_NAME) &&
                        !m.IsAbstract &&
                        !SymbolExtensions.HasAttribute(m, DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME));
                
                // find classes that derive from Component
                foreach (var member in members) {
                    componentInterfaceRegister.Add(new ComponentInterfaceDefinition(member.ContainingNamespace.Name, $"I{member.Name}", "member.Name", null));
                }
                
            }*/
            
            foreach(INamedTypeSymbol typeSymbol in GetNamedTypeSymbols(assemblySymbol.GlobalNamespace)) {
                if(!typeSymbol.IsDerivedFromType(COMPONENT_BASECLASS_NAME))
                    continue;
                if(typeSymbol.IsAbstract)
                    continue;
                //if(HasAttribute(typeSymbol, DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME))
                //    continue;
                componentInterfaceRegister.Add(new ComponentInterfaceDefinition(typeSymbol.ContainingNamespace.Name, $"I{typeSymbol.Name}", typeSymbol.Name, null));
            }
            
        }
        
        private static IEnumerable<INamedTypeSymbol> GetNamedTypeSymbols(INamespaceSymbol globalNamespace) {
            var stack = new Stack<INamespaceSymbol>();
            stack.Push(globalNamespace);

            while (stack.Count > 0) {
                var @namespace = stack.Pop();

                foreach (INamespaceOrTypeSymbol member in @namespace.GetMembers()) {
                    switch(member) {
                        case INamespaceSymbol memberAsNamespace:
                            stack.Push(memberAsNamespace);
                            break;
                        case INamedTypeSymbol memberAsNamedTypeSymbol:
                            yield return memberAsNamedTypeSymbol;
                            break;
                    }
                }
            }
        }
        
    }
    
}
