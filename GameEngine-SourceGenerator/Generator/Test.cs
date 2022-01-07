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
    public class ComponentInterfaceGenerator : ISourceGenerator {
        
        private const string COMPONENT_BASECLASS_NAME = "Component";
        //private const string ATTRIBUTE_NAME = "GenerateComponentInterface";
        
        public void Initialize(GeneratorInitializationContext context) {
            #if DEBUG
            if(!Debugger.IsAttached) Debugger.Launch();
            #endif
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
                            
                            //todo: exclude if class has attribute [DontGeneratorComponentInterface]
                            
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

                            var namespaceScope =
                                string.IsNullOrEmpty(namespaceAsText) ? "" : $"namespace {namespaceAsText};";

                            var sourceBuilder = new StringBuilder();
                            sourceBuilder.Append(
                                $@"
{usingDirectivesAsText}

{namespaceScope}

public interface {interfaceName} {{
    {className} {className} {{ get; set; }}
}}
"
                            );
                            context.AddSource($"{interfaceName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                        }
                    }
                }
            }
        }
    }
}
