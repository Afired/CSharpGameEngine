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
    public class EntityGenerator : ISourceGenerator {
        
        private const string ENTITY_BASECLASS_NAME = "Entity";
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
                        if(currentBaseSymbol.Name == ENTITY_BASECLASS_NAME) {
                            // is inherited from entity:
                            
                            //todo: exclude class that are not partial
                            //if()
                            //    break;
                            
                            var usingDirectives = fileWithClasses.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                            var usingDirectivesAsText = string.Join("\r\n", usingDirectives);

                            var classVisibility = "public";
                            
                            var className = declaredClass.Identifier.ToString();

                            string namespaceAsText = declaredClass.GetNamespace();
                            if(string.IsNullOrEmpty(namespaceAsText)) {
                                // if its not a normal scoped namespace, it may be a file scoped namespace
                                var filescopedNamespaceDeclaration = fileWithClasses.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>();
                                namespaceAsText = filescopedNamespaceDeclaration.FirstOrDefault()?.Name.ToString();
                            }

                            var namespaceScope = string.IsNullOrEmpty(namespaceAsText) ? "" : $"namespace {namespaceAsText};";
                            
                            string requiredInterfaces = null;
                            var interfaces = declaredClass.BaseList.Types.Select(baseType => baseType.ToString());
                            requiredInterfaces = string.Join(", ", interfaces);
                            string requiredInterfacesAsText = string.IsNullOrEmpty(requiredInterfaces) ? "" : $" : {requiredInterfaces}";
                            
                            var sourceBuilder = new StringBuilder();
                            sourceBuilder.Append(
$@"{usingDirectivesAsText}

{namespaceScope}

{classVisibility} partial class {className}{requiredInterfacesAsText} {{
    public int Hey {{ get; set; }}
}}
"
                            );
                            context.AddSource($"{className}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                        }
                    }
                }
            }
        }
    }
}
