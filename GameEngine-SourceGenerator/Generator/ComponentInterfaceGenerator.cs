using System.Linq;
using System.Text;
using GameEngine.Generator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.Generator {
    
    public static class ComponentInterfaceGenerator {
        
        private const string COMPONENT_BASECLASS_NAME = "Component";
        private const string DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME = "DoNotGenerateComponentInterface";
        private const string REQUIRE_COMPONENT_ATTRIBUTE_NAME = "RequireComponent";
        
        public static void Execute(GeneratorExecutionContext context) {
            
            var filesWithClasses = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes()
                .OfType<ClassDeclarationSyntax>().Any()
            );

            foreach(SyntaxTree fileWithClasses in filesWithClasses) {

                var semanticModel = context.Compilation.GetSemanticModel(fileWithClasses);
                
                foreach(ClassDeclarationSyntax declaredClass in fileWithClasses.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()) {
                    
                    INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(declaredClass);
                    
                    //exclude abstract classes
                    if(classSymbol.IsAbstract)
                        continue;

                    // exclude classes not derived from component
                    if(!classSymbol.IsDerivedFromType(COMPONENT_BASECLASS_NAME))
                        continue;
                    
                    //exclude class that have [DontGeneratorComponentInterface] attribute
                    if(declaredClass.HasAttribute(DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME))
                        break;
                    
                    var usingDirectives = fileWithClasses.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                    var usingDirectivesAsText = string.Join("\r\n", usingDirectives);
                    
                    var className = declaredClass.Identifier.ToString();
                    var interfaceName = $"I{className}";
                    
                    string namespaceAsText = declaredClass.GetNamespace();
                    if(string.IsNullOrEmpty(namespaceAsText)) {
                        // if its not a normal scoped namespace, it may be a file scoped namespace
                        var filescopedNamespaceDeclaration = fileWithClasses.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>();
                        namespaceAsText = filescopedNamespaceDeclaration.FirstOrDefault()?.Name.ToString();
                    }
                    
                    var namespaceScope = string.IsNullOrEmpty(namespaceAsText) ? "" : $"namespace {namespaceAsText};";
                    
                    string requiredComponents = null;
                    foreach(AttributeData attributeData in classSymbol.GetAttributes().
                                Where(attribute =>
                                    // filter attributes for attribute name
                                    attribute.AttributeClass.Name == REQUIRE_COMPONENT_ATTRIBUTE_NAME
                                    // exclude attributes with 0 arguments
                                    && attribute.ConstructorArguments.Length != 0)) {
                        
                        requiredComponents = string.Join(", ", attributeData.ConstructorArguments.Where(arg => arg.Value.ToString() != interfaceName).Select(arg => arg.Value));
                        break;
                    }
                    string requiredComponentsAsText = string.IsNullOrEmpty(requiredComponents) ? "" : $" : {requiredComponents}";
                    
                    var sourceBuilder = new StringBuilder();
                    sourceBuilder.Append(
$@"{usingDirectivesAsText}

{namespaceScope}

public interface {interfaceName}{requiredComponentsAsText} {{
    {className} {className} {{ get; }}
}}
"
                    );
                    context.AddSource($"{interfaceName}",
                        SourceText.From(sourceBuilder.ToString(), Encoding.UTF8)
                    );
                }
            }
        }
    }
}
