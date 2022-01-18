using System;
using System.Collections.Generic;
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
                    
                    string usingDirectives = file.GetUsingDirectives().Format();
                    
                    string fileScopedNamespace = file.GetNamespace(classSyntax).AsFileScopedNamespaceText();
                    
                    string classAccessibility = classSymbol.DeclaredAccessibility.AsText();
                    
                    string className = classSymbol.Name;
                    
                    //TODO: also gather interface BaseTypes  from interface BaseType recursively to ensure even "nested" required components get implemented
                    IEnumerable<INamedTypeSymbol> interfaces = classSymbol.GetAllInterfaces();
                    StringBuilder propertiesSb = new StringBuilder();
                    StringBuilder initializationSb = new StringBuilder();
                    foreach(INamedTypeSymbol @interface in interfaces) {
                        if(@interface.TypeKind == TypeKind.Error) throw new Exception();    // will throw error when interface is auto generated in a different assembly
                        string interfaceName = @interface.Name;
                        string componentName = ConvertFromInterfaceToComponent(interfaceName);
                        propertiesSb.Append($"    public {componentName} {componentName} {{ get; }}\n");
                        initializationSb.Append($"        {componentName} = new {componentName}(this);\n");
                    }
                    string properties = propertiesSb.ToString();
                    string initialization = initializationSb.ToString();
                    
                    var sourceBuilder = new StringBuilder();
                    sourceBuilder.Append(
$@"{usingDirectives}

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
        
        private static string ConvertFromInterfaceToComponent(string interfaceName) {
            if(!interfaceName.Contains('.'))
                return interfaceName.Substring(1);
            int index = interfaceName.LastIndexOf('.');
            return interfaceName.Substring(index + 2);
        }
        
    }
}
