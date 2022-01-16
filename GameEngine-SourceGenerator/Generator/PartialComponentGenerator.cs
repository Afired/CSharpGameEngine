using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GameEngine.Generator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.Generator {

// todo: update to incremental generator
// https://andrewlock.net/exploring-dotnet-6-part-9-source-generator-updates-incremental-generators/
    [Generator]
    public class PartialComponentGenerator : ISourceGenerator {
        
        private const string COMPONENT_BASECLASS_NAME = "Component";
        private const string DO_NOT_GENERATE_COMPONENT_INTERFACE_ATTRIBUTE_NAME = "DoNotGenerateComponentInterface";
        private const string REQUIRE_COMPONENT_ATTRIBUTE_NAME = "RequireComponent";
        
        public void Initialize(GeneratorInitializationContext context) {
            // uncomment for debugging of the source generator process
//            #if DEBUG
//            if(!Debugger.IsAttached) Debugger.Launch();
//            #endif
        }
        
        public void Execute(GeneratorExecutionContext context) {
            
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

                    INamedTypeSymbol currentBaseSymbol = classSymbol;
                    while((currentBaseSymbol = currentBaseSymbol.BaseType) != null) {
                        if(currentBaseSymbol.Name == COMPONENT_BASECLASS_NAME) {
                            // is inherited from component:
                            
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

                            string classVisibility = "public";
                            
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
                                $@"{usingDirectivesAsText}

{namespaceScope}

{classVisibility} partial class {className} {{

{autogenProperties}

    public {className}(Entity entity) : base(entity) {{ }}

}}
"
                            );
                            context.AddSource($"{className}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                        }
                    }
                }
            }
        }

        private string ConvertFromInterfaceToComponent(string interfaceName) {
            if(!interfaceName.Contains('.'))
                return interfaceName.Substring(1);
            int index = interfaceName.LastIndexOf('.');
            return interfaceName.Substring(index + 2);
        }
        
    }
}
