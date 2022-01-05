using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

//namespace GameEngine.AutoGenerator; 

// todo: update to incremental generator
// https://andrewlock.net/exploring-dotnet-6-part-9-source-generator-updates-incremental-generators/
[Generator]
public class ComponentInterfaceGenerator : ISourceGenerator {

    public void Initialize(GeneratorInitializationContext context) {
        #if DEBUG
        if(!Debugger.IsAttached) Debugger.Launch();
        #endif
    }

    public void Execute(GeneratorExecutionContext context) {

        const string ATTRIBUTE_NAME = "GenerateComponentInterface";

        var classWithAttributes = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .Any(p => p.DescendantNodes().OfType<AttributeSyntax>().Any())
        );

        foreach(SyntaxTree tree in classWithAttributes) {

            var semanticModel = context.Compilation.GetSemanticModel(tree);

            foreach(var declaredClass in tree
                        .GetRoot()
                        .DescendantNodes()
                        .OfType<ClassDeclarationSyntax>()
                        .Where(cd => cd.DescendantNodes().OfType<AttributeSyntax>().Any())) {

                var hasAttribute = declaredClass.AttributeLists.Any(x => x.Attributes.Any(y => y.Name.ToString().Contains(ATTRIBUTE_NAME)));
                
                if(hasAttribute) {
                    var usingDirectives = tree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                    var usingDirectivesAsText = string.Join("\r\n", usingDirectives);

                    var className = declaredClass.Identifier.ToString();
                    var interfaceName = $"I{className}";
                    
                    string namespaceAsText = declaredClass.GetNamespace();
                    if(string.IsNullOrEmpty(namespaceAsText)) { // if its not a normal scoped namespace, it may be a file scoped namespace
                        var filescopedNamespaceDeclaration = tree.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>();
                        namespaceAsText = filescopedNamespaceDeclaration.FirstOrDefault()?.Name.ToString();
                    }
                    var namespaceScope = string.IsNullOrEmpty(namespaceAsText) ? "" : $"namespace {namespaceAsText};";
                    
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

public static class ClassDeclarationSyntaxExtensions {

    public static string GetNamespace(this ClassDeclarationSyntax source) {
        if(source == null)
            return null;
        
        var parent = source.Parent;
        while (parent.IsKind(SyntaxKind.ClassDeclaration)) {
            var parentClass = parent as ClassDeclarationSyntax;
            
            if(parentClass == null)
                return null;
            
            parent = parent.Parent;
        }

        var nameSpace = parent as NamespaceDeclarationSyntax;
        
        return nameSpace?.Name.ToString();
    }
}