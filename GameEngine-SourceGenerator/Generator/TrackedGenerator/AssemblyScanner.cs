using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.Generator.Extensions;
using GameEngine.Generator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.Generator.Tracked {
    
    //reference: https://stackoverflow.com/questions/68055210/generate-source-based-on-other-assembly-classes-c-source-generator
    internal static class AssemblyScanner {
        
        private const string COMPONENT_BASECLASS_NAME = "Component";
        private const string DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME = "DoNotGenerateComponentInterface";
        private const string REQUIRE_COMPONENT_ATTRIBUTE_NAME = "RequireComponent";
        
        internal static void ScanOtherAssemblies(GeneratorExecutionContext context) {
            
            // if it doesnt reference the assembly just return
            if(!context.Compilation.SourceModule.ReferencedAssemblySymbols.Any(q => q.Identity.Name == "GameEngine"))
                return;
            
            // retrieve referenced assembly symbols
            IAssemblySymbol assemblySymbol = context.Compilation.SourceModule.ReferencedAssemblySymbols.FirstOrDefault(q => q.Identity.Name == "GameEngine");
            
            foreach(INamedTypeSymbol typeSymbol in GetNamedTypeSymbols(assemblySymbol.GlobalNamespace)) {
                if(!typeSymbol.IsDerivedFromType(COMPONENT_BASECLASS_NAME))
                    continue;
                if(typeSymbol.IsAbstract)
                    continue;
                //if(HasAttribute(typeSymbol, DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME))
                //    continue;

                string[] requiredComponentsNames = null;
                var attributeData1 = typeSymbol.GetAttributes().FirstOrDefault(attribute =>
                    attribute.AttributeClass.Name == REQUIRE_COMPONENT_ATTRIBUTE_NAME
                    // exclude attributes with 0 arguments
                    && attribute.ConstructorArguments.Length != 0);
                if(attributeData1 != null) {

                    TypedConstant firstArgument = attributeData1.ConstructorArguments[0];

                    switch(firstArgument.Kind) {
                        case TypedConstantKind.Array:
                            requiredComponentsNames = firstArgument.Values
                                .Where(arg => arg.Value.ToString() != typeSymbol.Name)
                                .Select(arg => arg.Value.ToString()).ToArray();
                            break;
                        default:
                            if(firstArgument.Value.ToString() != typeSymbol.Name)
                                requiredComponentsNames = new string[] { firstArgument.Value.ToString() };
                            break;
                    }
                    
                }
                /*
                foreach(AttributeData attributeData in typeSymbol.GetAttributes().
                            Where(attribute =>
                                // filter attributes for attribute name
                                attribute.AttributeClass.Name == REQUIRE_COMPONENT_ATTRIBUTE_NAME
                                // exclude attributes with 0 arguments
                                && attribute.ConstructorArguments.Length != 0)) {

                    foreach(TypedConstant argument in attributeData.ConstructorArguments) {
                        switch(argument.Kind) {
                            case TypedConstantKind.Array:
                                requiredComponentsNames = argument.Values.Where(arg => arg.Value.ToString() != typeSymbol.Name).Select(arg => arg.Value.ToString()).ToArray();
                                break;
                            default:
                                requiredComponentsNames = argument.Where(arg => arg.Value.ToString() != className).Select(arg => arg.Value.ToString().Substring(1)).ToArray();
                                break;
                        }
                        //! WE CURRENTLY ONLY PROCESSES FIRST ENTRY OF VALUES
                        break;
                    }
                    
                    //requiredComponentsNames = attributeData.ConstructorArguments.Where(arg => arg.Value.ToString() != typeSymbol.Name).Select(arg => arg.Value.ToString().Substring(1)).ToArray();
                    break;
                }*/
                ComponentInterfaceRegister.RegisterForOtherAssembly(new ComponentInterfaceDefinition(typeSymbol.ContainingNamespace.Name, $"I{typeSymbol.Name}", typeSymbol.Name, requiredComponentsNames));
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
        
        internal static void ScanThisAssembly(GeneratorExecutionContext context) {
            
            // get all files with class declarations
            var files = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Any());
            
            foreach(SyntaxTree file in files) {
                
                var semanticModel = context.Compilation.GetSemanticModel(file);
                
                foreach(ClassDeclarationSyntax classSyntax in file.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()) {
                    
                    INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classSyntax);
                    
                    //exclude abstract classes
                    if(classSymbol.IsAbstract)
                        continue;
                    
                    // exclude classes not derived from component
                    if(!classSymbol.IsDerivedFromType(COMPONENT_BASECLASS_NAME))
                        continue;
                    
                    //exclude class that have [DontGeneratorComponentInterface] attribute
                    if(classSyntax.HasAttribute(DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME))
                        break;
                    
                    string usingDirectives = file.GetUsingDirectives().Format();

                    var @namespace = file.GetNamespace(classSyntax);
                    string fileScopedNamespace = @namespace.AsFileScopedNamespaceText();
                    
                    string className = classSymbol.Name;
                    var interfaceName = $"I{className}";
                    /*
                    string[] requiredComponentsNames = null;
                    foreach(AttributeData attributeData in classSymbol.GetAttributes().
                                Where(attribute =>
                                    // filter attributes for attribute name
                                    attribute.AttributeClass.Name == REQUIRE_COMPONENT_ATTRIBUTE_NAME
                                    // exclude attributes with 0 arguments
                                    && attribute.ConstructorArguments.Length != 0)) {
                        
                        requiredComponentsNames = attributeData.ConstructorArguments.Where(arg => arg.Value.ToString() != className).Select(arg => arg.Value.ToString()).ToArray();
                        break;
                    }*/
                    
                    string[] requiredComponentsNames = null;
                    var attributeData1 = classSymbol.GetAttributes().FirstOrDefault(attribute =>
                        attribute.AttributeClass.Name == REQUIRE_COMPONENT_ATTRIBUTE_NAME
                        // exclude attributes with 0 arguments
                        && attribute.ConstructorArguments.Length != 0);
                    if(attributeData1 != null) {

                        TypedConstant firstArgument = attributeData1.ConstructorArguments[0];

                        switch(firstArgument.Kind) {
                            case TypedConstantKind.Array:
                                requiredComponentsNames = firstArgument.Values
                                    .Where(arg => arg.Value.ToString() != classSymbol.Name)
                                    .Select(arg => arg.Value.ToString()).ToArray();
                                break;
                            default:
                                if(firstArgument.Value.ToString() != classSymbol.Name)
                                    requiredComponentsNames = new string[] { firstArgument.Value.ToString() };
                                break;
                        }
                    
                    }
                    
                    ComponentInterfaceRegister.RegisterForThisAssembly(new ComponentInterfaceDefinition(@namespace.Name.ToString(), interfaceName, className, requiredComponentsNames));
                }
            }
        }
        
    }
    
}
