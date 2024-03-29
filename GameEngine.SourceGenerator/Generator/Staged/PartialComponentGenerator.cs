using System.Linq;
using System.Text;
using GameEngine.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.SourceGenerator.Staged {
    
    public static class PartialComponentGenerator {
        
        private const string COMPONENT_BASECLASS_NAME = "Component";
        private const string DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME = "DoNotGenerateComponentInterface";
        private const string REQUIRE_COMPONENT_ATTRIBUTE_NAME = "RequireComponent";
        
        public static void Execute(GeneratorExecutionContext context) {
            
            // get all files with class declarations
            var files = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Any());
            
            foreach(SyntaxTree file in files) {
                
                var semanticModel = context.Compilation.GetSemanticModel(file);
                
                foreach(ClassDeclarationSyntax classSyntax in file.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()) {
                    
                    INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classSyntax);
                    
                    // exclude abstract classes
                    if(classSymbol.IsAbstract)
                        continue;
                    
                    // exclude classes not derived from component
                    if(!classSymbol.IsDerivedFromType(COMPONENT_BASECLASS_NAME))
                        continue;
                    
                    // exclude class that have [DontGeneratorComponentInterface] attribute
                    if(classSyntax.HasAttribute(DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME))
                        break;
                    
                    // warn to use partial keyword
                    if(!classSyntax.IsPartial()) {
                        // these currently dont work on runtime, but when building solution
                        Diagnostic diagnostic = Diagnostic.Create(new DiagnosticDescriptor("TEST01", "Title", "Message", "Category", DiagnosticSeverity.Error, true), classSyntax.GetLocation());
                        context.ReportDiagnostic(diagnostic);
                    }

                    string usingDirectives = file.GetUsingDirectives().Format();
                    
                    string fileScopedNamespace = file.GetNamespace(classSyntax).AsFileScopedNamespaceText();
                    
                    string className = classSymbol.Name;
                    var interfaceName = $"I{className}";
                    
                    string classAccessibility = classSymbol.DeclaredAccessibility.AsText();
                    
                    StringBuilder autogenPropertiesSB = new StringBuilder();
                    foreach(AttributeData attributeData in classSymbol.GetAttributes().
                                Where(attribute =>
                                    // filter attributes for attribute name
                                    attribute.AttributeClass.Name == REQUIRE_COMPONENT_ATTRIBUTE_NAME
                                    // exclude attributes with 0 arguments
                                    && attribute.ConstructorArguments.Length != 0)) {
                        
                        foreach(string requiredComponentInterface in attributeData.ConstructorArguments.Where(arg => arg.Value.ToString() != interfaceName).Select(arg => arg.Value.ToString())) {
                            string requiredComponent = ConvertFromInterfaceToComponent(requiredComponentInterface);
                            autogenPropertiesSB.Append($"    public {requiredComponent} {requiredComponent} => (Entity as {requiredComponentInterface})!.{requiredComponent};\n");
                        }
                        break;
                    }
                    string autogenProperties = autogenPropertiesSB.ToString();
                    
                    var sourceBuilder = new StringBuilder();
                    sourceBuilder.Append(
$@"{usingDirectives}

{fileScopedNamespace}

{classAccessibility} partial class {className} {{

{autogenProperties}

    public {className}(Entity entity) : base(entity) {{ }}

}}
"
                    );
                    context.AddSource($"{className}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                }
            }
        }

        private static string ConvertFromInterfaceToComponent(string interfaceName) {
            if(!interfaceName.Contains('.'))
                return interfaceName.Substring(1);
            int index = interfaceName.LastIndexOf('.');
            return interfaceName.Substring(index + 2);
        }
        
    }
}
