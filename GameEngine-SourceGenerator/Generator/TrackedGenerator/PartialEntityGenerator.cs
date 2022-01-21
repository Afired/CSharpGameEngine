using System;
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
    
    internal static class PartialEntityGenerator {
        
        private const string ENTITY_BASECLASS_NAME = "Entity";
        
        internal static void Execute(GeneratorExecutionContext context) {
            
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
                    if(!classSymbol.IsDerivedFromType(ENTITY_BASECLASS_NAME))
                        continue;
                    
                    // warn to use partial keyword
                    if(!classSyntax.IsPartial()) {
                        // these currently dont work on runtime, but when building solution
                        Diagnostic diagnostic = Diagnostic.Create(new DiagnosticDescriptor("TEST01", "Title", "Message", "Category", DiagnosticSeverity.Error, true), classSyntax.GetLocation());
                        context.ReportDiagnostic(diagnostic);
                    }
                    
                    string usingDirectives = file.GetUsingDirectives().Format();
                    
                    string fileScopedNamespace = file.GetNamespace(classSyntax).AsFileScopedNamespaceText();
                    
                    string classAccessibility = classSymbol.DeclaredAccessibility.AsText();
                    
                    string className = classSymbol.Name;
                    
                    //TODO: also gather interface BaseTypes  from interface BaseType recursively to ensure even "nested" required components get implemented
                    // IEnumerable<INamedTypeSymbol> interfaces = classSymbol.GetAllInterfaces();
                    
                    // get names of interfaces without relying on symbols
                    var baseTypeNames = classSyntax.BaseList.Types.Select(baseType => baseType.ToString()); //TODO: may want to remove duplicates
                    var interfaceNames = baseTypeNames.Where(interfaceName => ComponentInterfaceRegister.AllDefinitions.Any(definition => definition.InterfaceName == interfaceName));
                    
                    StringBuilder propertiesSb = new StringBuilder();
                    StringBuilder initializationSb = new StringBuilder();
                    foreach(string baseTypeName in baseTypeNames) {
                        if(ComponentInterfaceRegister.TryToGetDefinition(baseTypeName, (s1, d) => s1 == d.InterfaceName, out ComponentInterfaceDefinition interfaceDefinition)) {

                            foreach(ComponentInterfaceDefinition required in interfaceDefinition.GetAllRequiredComponents()) {
                                
                                propertiesSb.Append("    public ");
                                propertiesSb.Append(required.Namespace);
                                propertiesSb.Append('.');
                                propertiesSb.Append(required.ComponentName);
                                propertiesSb.Append(' ');
                                propertiesSb.Append(required.ComponentName);
                                propertiesSb.Append(" {{ get; }}\n");
                                
                                initializationSb.Append("        ");
                                initializationSb.Append(required.ComponentName);
                                initializationSb.Append(" = new ");
                                initializationSb.Append(required.Namespace);
                                initializationSb.Append('.');
                                initializationSb.Append(required.ComponentName);
                                initializationSb.Append("(this);\n");
                            }
                            
                        }
                    }
                    string properties = propertiesSb.ToString();
                    string initialization = initializationSb.ToString();
                    
                    var sourceBuilder = new StringBuilder();
                    sourceBuilder.Append(
$@"//usingDirectives

{fileScopedNamespace}

{classAccessibility} partial class {className} {{

{properties}
    public {className}() {{
{initialization}
        Init();
    }}

}}
"
                    );
                    context.AddSource($"{className}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                }
            }
        }
    }
}
