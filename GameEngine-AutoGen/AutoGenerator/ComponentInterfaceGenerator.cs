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
        
        var classWithAttributes = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Any(p => p.DescendantNodes().OfType<AttributeSyntax>().Any()));
        
        foreach (SyntaxTree tree in classWithAttributes) {
            
            var semanticModel = context.Compilation.GetSemanticModel(tree);
                
            foreach(var declaredClass in tree
                        .GetRoot()
                        .DescendantNodes()
                        .OfType<ClassDeclarationSyntax>()
                        .Where(cd => cd.DescendantNodes().OfType<AttributeSyntax>().Any())) {
                
                var hasAttribute = declaredClass.AttributeLists.Any(x => x.Attributes.Any(y => y.Name.ToString() == ATTRIBUTE_NAME));
                
                if(hasAttribute) {
                    var usingDirectives = tree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                    var usingDirectivesAsText = string.Join("\r\n", usingDirectives);

                    var className = declaredClass.Identifier.ToString();
                    var interfaceName = $"I{className}";

                    //var classNamespace = GetNamespace(context.Compilation, syntaxTree, autoGenerateComponentInterfaceDeclaration);
                    //var namespaceDeclaration = tree.GetRoot().DescendantNodes().OfType<NamespaceDeclarationSyntax>();
                    var filescopedNamespaceDeclaration = tree.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>();
                    var namespaceAsText = filescopedNamespaceDeclaration.FirstOrDefault()?.Name.ToString();
                    var namespaceUsing = string.IsNullOrEmpty(namespaceAsText) ? "" : $"using {namespaceAsText};";

                    var sourceBuilder = new StringBuilder();
                    sourceBuilder.Append(
                        $@"
{usingDirectivesAsText}
{namespaceUsing}

public interface {interfaceName} {{
    {className} {className} {{ get; set; }}
}}
");
                    context.AddSource($"{interfaceName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                }
            }
        }
        
    }
    
    public static string GetNamespaceFrom(SyntaxNode s) =>
        s.Parent switch
        {
            NamespaceDeclarationSyntax namespaceDeclarationSyntax => namespaceDeclarationSyntax.Name.ToString(),
            null => string.Empty, // or whatever you want to do
            _ => GetNamespaceFrom(s.Parent)
        };
    
    public static string GetPrefix(SyntaxNode member)
    {
        if (member == null) {
            return "";
        }

        StringBuilder sb = new StringBuilder();
        SyntaxNode node = member;

        while(node.Parent != null) {
            node = node.Parent;

            if (node is NamespaceDeclarationSyntax) {
                var namespaceDeclaration = (NamespaceDeclarationSyntax) node;

                sb.Insert(0, ".");
                sb.Insert(0, namespaceDeclaration.Name.ToString());
            } else if (node is ClassDeclarationSyntax) {
                var classDeclaration = (ClassDeclarationSyntax) node;

                sb.Insert(0, ".");
                sb.Insert(0, classDeclaration.Identifier.ToString());
            }
        }

        return sb.ToString();
    }

    private static string GetNamespace(Compilation compilation, SyntaxTree yourSyntaxTree, TypeSyntax yourTypeSyntax) {
        var semanticModel = compilation.GetSemanticModel(yourSyntaxTree);
        var typeSymbol = semanticModel.GetSymbolInfo(yourTypeSyntax).Symbol as INamedTypeSymbol;
        //var namespaceSymbol = typeSymbol.Namespace;
        var namespaceSymbol = typeSymbol.Name;
        return namespaceSymbol;
    }
    
}

public static class ClassDeclarationSyntaxExtensions
{
    public const string NESTED_CLASS_DELIMITER = "+";
    public const string NAMESPACE_CLASS_DELIMITER = ".";

    public static string GetFullName(this ClassDeclarationSyntax source)
    {
        Contract.Requires(null != source);

        var items = new List<string>();
        var parent = source.Parent;
        while (parent.IsKind(SyntaxKind.ClassDeclaration))
        {
            var parentClass = parent as ClassDeclarationSyntax;
            Contract.Assert(null != parentClass);
            items.Add(parentClass.Identifier.Text);

            parent = parent.Parent;
        }

        var nameSpace = parent as NamespaceDeclarationSyntax;
        Contract.Assert(null != nameSpace);
        var sb = new StringBuilder().Append(nameSpace.Name).Append(NAMESPACE_CLASS_DELIMITER);
        items.Reverse();
        items.ForEach(i => { sb.Append(i).Append(NESTED_CLASS_DELIMITER); });
        sb.Append(source.Identifier.Text);

        var result = sb.ToString();
        return result;
    }
}