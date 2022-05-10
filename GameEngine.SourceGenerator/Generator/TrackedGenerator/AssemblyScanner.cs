using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.SourceGenerator.Extensions;
using GameEngine.SourceGenerator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameEngine.SourceGenerator.Tracked {
    
    //reference: https://stackoverflow.com/questions/68055210/generate-source-based-on-other-assembly-classes-c-source-generator
    internal static class AssemblyScanner {
        
        private const string NODE_BASECLASS_NAME = "Node";
        private const string GAME_ENGINE_CORE_ASSEMBLY_NAME = "GameEngine.Core";
        
        
        internal static void ScanThisAssembly(GeneratorExecutionContext context) {
            
            // get all files with class declarations
            var files = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Any());
            
            foreach(SyntaxTree file in files) {
                
                var semanticModel = context.Compilation.GetSemanticModel(file);
                
                foreach(ClassDeclarationSyntax classSyntax in file.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()) {
                    
                    INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classSyntax);
                    
                    // exclude abstract classes
                    if(classSymbol.IsAbstract)
                        continue;
                    
                    // exclude classes not derived from entity
                    if(!classSymbol.IsDerivedFromType(NODE_BASECLASS_NAME))
                        continue;
                    
                    // warn to use partial keyword
                    if(!classSyntax.IsPartial()) {
                        // these currently dont work on runtime, but when building solution
                        Diagnostic diagnostic = Diagnostic.Create(new DiagnosticDescriptor("TEST01", "Title", "Message", "Category", DiagnosticSeverity.Error, true), classSyntax.GetLocation());
                        context.ReportDiagnostic(diagnostic);
                    }
                    
//                    string usingDirectives = file.GetUsingDirectives().Format();
                    string @namespace = file.GetNamespace(classSyntax).Name.ToString();
                    string className = classSymbol.Name;
//                    string classAccessibility = classSymbol.DeclaredAccessibility.AsText();
                    string[] baseTypes = classSyntax.BaseList.Types.Select(type => type.ToString()).ToArray();
                    
                    NodeRegister.RegisterForThisAssembly(new NodeDefinition(
                        @namespace,
                        className,
                        $"I{className}",
                        baseTypes)
                    );
                }
            }
        }
        
//        internal static void ScanOtherAssemblies(GeneratorExecutionContext context) {
//            
//            // if it doesnt reference the assembly just return
//            if(!context.Compilation.SourceModule.ReferencedAssemblySymbols.Any(q => q.Identity.Name == GAME_ENGINE_CORE_ASSEMBLY_NAME))
//                return;
//            
//            // retrieve referenced assembly symbols
//            IAssemblySymbol assemblySymbol = context.Compilation.SourceModule.ReferencedAssemblySymbols.FirstOrDefault(q => q.Identity.Name == GAME_ENGINE_CORE_ASSEMBLY_NAME);
//            
//            foreach(INamedTypeSymbol typeSymbol in GetNamedTypeSymbols(assemblySymbol.GlobalNamespace)) {
//                if(!typeSymbol.IsDerivedFromType(NODE_BASECLASS_NAME))
//                    continue;
//                
//                //? not sure
//                if(typeSymbol.IsAbstract)
//                    continue;
//                
//                if(typeSymbol)
//                
//                NodeRegister.RegisterForOtherAssembly(new NodeDefinition(
//                    typeSymbol.ContainingNamespace.ToString(), 
//                    $"I{typeSymbol.Name}", 
//                    typeSymbol.Name, 
//                    requiredComponentsNames)
//                );
//            }
//            
//        }
        
        internal static IEnumerable<NodeDefinition> GetComponentInterfaceDefinitionsFromBaseListSyntax(BaseListSyntax baseList) {
            foreach(string baseTypeName in baseList.Types.Select(baseType => baseType.ToString())) {
                if(NodeRegister.TryToGetDefinition(baseTypeName, (s1, d) => s1 == d.InterfaceName, out NodeDefinition interfaceDefinition)) {
                    yield return interfaceDefinition;
                    foreach(NodeDefinition required in interfaceDefinition.GetAllChildNodes()) {
                        yield return required;
                    }
                }
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
        
//        internal static void ScanThisAssembly(GeneratorExecutionContext context) {
//            
//            // get all files with class declarations
//            var files = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Any());
//            
//            foreach(SyntaxTree file in files) {
//                
//                var semanticModel = context.Compilation.GetSemanticModel(file);
//                
//                foreach(ClassDeclarationSyntax classSyntax in file.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()) {
//                    
//                    INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classSyntax);
//                    
//                    if(!classSymbol.IsDerivedFromType(NODE_BASECLASS_NAME))
//                        continue;
//                    
//                    //? not sure
//                    if(classSymbol.IsAbstract)
//                        continue;
//                    
//                    string usingDirectives = file.GetUsingDirectives().Format();
//                    
//                    var @namespace = file.GetNamespace(classSyntax);
//                    string fileScopedNamespace = @namespace.AsFileScopedNamespaceText();
//                    
//                    string className = classSymbol.Name;
//                    var interfaceName = $"I{className}";
//
//
//                    string[] childNodeNames = null;
//                    
//                    string[] requiredComponentsNames = null;
//                    
//                    
//                    NodeRegister.RegisterForThisAssembly(new NodeDefinition(
//                        @namespace.Name.ToString(), 
//                        interfaceName, 
//                        className, 
//                        requiredComponentsNames)
//                    );
//                }
//            }
//        }
    }
}
