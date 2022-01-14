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
                            
                            // these currently dont work on runtime, but when building solution
                            Diagnostic diagnostic = Diagnostic.Create(new DiagnosticDescriptor("TEST01", "Title", "Message", "Category", DiagnosticSeverity.Warning, true), declaredClass.GetLocation());
                            context.ReportDiagnostic(diagnostic);
                            
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
                            
                            var baseTypeNames = declaredClass.BaseList.Types.Select(baseType => baseType.ToString());
                            
                            // really quick and dirty implementation
                            var interfaceNames = baseTypeNames.Where(name => name.StartsWith("I"));
                            StringBuilder autogenPropertiesSB = new StringBuilder();
                            foreach(string interfaceName in interfaceNames) {
                                autogenPropertiesSB.Append($"    public {interfaceName.Substring(1)} {interfaceName.Substring(1)} {{ get; }}\n");
                            }
                            string autogenProperties = autogenPropertiesSB.ToString();
                            
                            StringBuilder initAutogenPropertiesSB = new StringBuilder();
                            foreach(string interfaceName in interfaceNames) {
                                initAutogenPropertiesSB.Append($"        {interfaceName.Substring(1)} = new {interfaceName.Substring(1)}(this);\n");
                            }
                            string initAutogenProperties = initAutogenPropertiesSB.ToString();
                            
                            string baseTypes = string.Join(", ", baseTypeNames);
                            string requiredBaseTypesAsText = string.IsNullOrEmpty(baseTypes) ? "" : $" : {baseTypes}";
                            
                            var sourceBuilder = new StringBuilder();
                            sourceBuilder.Append(
$@"{usingDirectivesAsText}

{namespaceScope}

{classVisibility} partial class {className} /*not necessary and also gives warning of double specified base types {requiredBaseTypesAsText}*/ {{

{autogenProperties}
    public {className}() {{
{initAutogenProperties}
        Init();
    }}

    //private partial void Init();

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
