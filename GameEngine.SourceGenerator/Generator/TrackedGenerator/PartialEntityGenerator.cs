using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.SourceGenerator.Extensions;
using GameEngine.SourceGenerator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.SourceGenerator.Tracked {
    
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
                    
                    IEnumerable<NodeDefinition> GetComponentInterfaceDefinitionsFromBaseListSyntax(BaseListSyntax baseList) {
                        foreach(string baseTypeName in baseList.Types.Select(baseType => baseType.ToString())) {
                            if(NodeRegister.TryToGetDefinition(baseTypeName, (s1, d) => s1 == d.InterfaceName, out NodeDefinition interfaceDefinition)) {
                                yield return interfaceDefinition;
                                foreach(NodeDefinition required in interfaceDefinition.GetAllChildNodes()) {
                                    yield return required;
                                }
                            }
                        }
                    }
                    
                    StringBuilder propertiesSb = new StringBuilder();
                    StringBuilder initializationSb = new StringBuilder();
                    StringBuilder addToComponentsListSb = new StringBuilder();
                    foreach(NodeDefinition required in GetComponentInterfaceDefinitionsFromBaseListSyntax(classSyntax.BaseList).Distinct()) {
                        propertiesSb.Append("    public ");
                        propertiesSb.Append(required.Namespace);
                        propertiesSb.Append('.');
                        propertiesSb.Append(required.ClassName);
                        propertiesSb.Append(' ');
                        propertiesSb.Append(required.ClassName);
                        propertiesSb.Append(" { get; }\n");
                                
                        initializationSb.Append("        ");
                        initializationSb.Append(required.ClassName);
                        initializationSb.Append(" = new ");
                        initializationSb.Append(required.Namespace);
                        initializationSb.Append('.');
                        initializationSb.Append(required.ClassName);
                        initializationSb.Append("(this);\n");
                        
                        addToComponentsListSb.Append("        (Components as System.Collections.Generic.List<GameEngine.Core.Components.Component>)!.Add(");
                        addToComponentsListSb.Append(required.ClassName);
                        addToComponentsListSb.Append(");\n");
                    }
                    string properties = propertiesSb.ToString();
                    string initialization = initializationSb.ToString();
                    string addToComponentsList = addToComponentsListSb.ToString();
                    
                    var sourceBuilder = new StringBuilder();
                    sourceBuilder.Append(
$@"//usingDirectives

{fileScopedNamespace}

{classAccessibility} partial class {className} {{

{properties}
    public {className}() : base() {{
{initialization}
{addToComponentsList}
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
