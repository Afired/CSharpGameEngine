using System.Linq;
using System.Text;
using GameEngine.Generator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.Generator {
    
    public static class PartialEntityGenerator {
        
        private const string ENTITY_BASECLASS_NAME = "Entity";
        
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
                    
                    // exclude classes not derived from entity
                    if(!classSymbol.IsDerivedFromType(ENTITY_BASECLASS_NAME))
                        continue;
                    
                    // warn to use partial keyword
                    if(!classSyntax.IsPartial()) {
                        // these currently dont work on runtime, but when building solution
                        Diagnostic diagnostic = Diagnostic.Create(new DiagnosticDescriptor("TEST01", "Title", "Message", "Category", DiagnosticSeverity.Error, true), classSyntax.GetLocation());
                        context.ReportDiagnostic(diagnostic);
                    }
                    
                    var usingDirectives = file.GetUsingDirectives();
                    
                    var classVisibility = "public";
                    
                    var className = classSyntax.Identifier.ToString();
                    
                    string namespaceAsText = classSyntax.GetNamespace();
                    if(string.IsNullOrEmpty(namespaceAsText)) {
                        // if its not a normal scoped namespace, it may be a file scoped namespace
                        var filescopedNamespaceDeclaration = file.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>();
                        namespaceAsText = filescopedNamespaceDeclaration.FirstOrDefault()?.Name.ToString();
                    }
                    
                    var namespaceScope = string.IsNullOrEmpty(namespaceAsText) ? "" : $"namespace {namespaceAsText};";
                    
                    var baseTypeNames = classSyntax.BaseList.Types.Select(baseType => baseType.ToString());
                    
                    //TODO: also gather interface BaseTypes  from interface BaseType recursively to ensure even "nested" required components get implemented
                    
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
$@"{usingDirectives.Format()}

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
