using System.Text;
using GameEngine.Generator.Tracked.Tracking;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.Generator.Tracked {
    
    internal static class ComponentInterfaceGenerator {
        
        internal static void Execute(GeneratorExecutionContext context) {

            foreach(ComponentInterfaceDefinition definition in ComponentInterfaceRegister.EnumerateDefinitionsFromThisAssembly()) {

                string requiredInterfaces = null;
                if(definition.HasRequiredComponents) {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" :");
                    foreach(ComponentInterfaceDefinition required in definition.GetAllRequiredComponents()) {
                        sb.Append(" ");
                        sb.Append(required.Namespace);
                        sb.Append('.');
                        sb.Append(required.InterfaceName);
                    }
                    requiredInterfaces = sb.ToString();
                }
                
                StringBuilder sourceBuilder = new StringBuilder();
                sourceBuilder.Append(
$@"{definition.NamespaceAsFileScopedText()}

public interface {definition.InterfaceName}{requiredInterfaces} {{
    {definition.ComponentName} {definition.ComponentName} {{ get; }}
}}
"
                );
                context.AddSource($"{definition.InterfaceName}",
                    SourceText.From(sourceBuilder.ToString(), Encoding.UTF8)
                );
                
            }
            
        }
        
         /*
         private const string COMPONENT_BASECLASS_NAME = "Component";
         private const string DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME = "DoNotGenerateComponentInterface";
         private const string REQUIRE_COMPONENT_ATTRIBUTE_NAME = "RequireComponent";
        
         internal static void Execute(GeneratorExecutionContext context, ComponentInterfaceRegister componentInterfaceRegister) {
            
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
                    
                    string requiredComponents = null;
                    foreach(AttributeData attributeData in classSymbol.GetAttributes().
                                Where(attribute =>
                                    // filter attributes for attribute name
                                    attribute.AttributeClass.Name == REQUIRE_COMPONENT_ATTRIBUTE_NAME
                                    // exclude attributes with 0 arguments
                                    && attribute.ConstructorArguments.Length != 0)) {
                        
                        requiredComponents = string.Join(", ", attributeData.ConstructorArguments.Where(arg => arg.Value.ToString() != className).Select(arg => arg.Value.ToString().Substring(1)));
                        break;
                    }
                    string requiredComponentsAsText = string.IsNullOrEmpty(requiredComponents) ? "" : $" : {requiredComponents}";
                    
                    var sourceBuilder = new StringBuilder();
                    sourceBuilder.Append(
$@"{usingDirectives}

{fileScopedNamespace}

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
        }*/
    }
}
