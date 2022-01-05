using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.AutoGenerator; 

[Generator]
public class ComponentInterfaceGenerator : ISourceGenerator {
    
    public void Initialize(GeneratorInitializationContext context) {
//        #if DEBUG
//        if(!Debugger.IsAttached) Debugger.Launch();
//        #endif
    }

    public void Execute(GeneratorExecutionContext context) {
        var syntaxTrees = context.Compilation.SyntaxTrees;

        foreach(SyntaxTree syntaxTree in syntaxTrees) {
            var autoGenerateComponentInterfaceDeclarations = syntaxTree.GetRoot().DescendantNodes()
                .OfType<TypeDeclarationSyntax>()
                .Where(x => x.AttributeLists.Any(y => y.ToString() == "[GenerateComponentInterface]")).ToList();

            foreach(TypeDeclarationSyntax autoGenerateComponentInterfaceDeclaration in autoGenerateComponentInterfaceDeclarations) {
                var usingDirectives = syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                var usingDirectivesAsText = string.Join("\r\n", usingDirectives);
                var sourceBuilder = new StringBuilder();

                var className = autoGenerateComponentInterfaceDeclaration.Identifier.ToString();
                var autoGenerateComponentInterfaceName = $"I{className}";
                
                sourceBuilder.Append($@"
{usingDirectivesAsText}

public interface {autoGenerateComponentInterfaceName} {{
    {className} {className} {{ get; set; }}
}}
");
                context.AddSource($"{autoGenerateComponentInterfaceName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            }
        }
    }
    
}
