using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.SourceGenerator; 

public static class PartialNodeGenerator {
    
    private const string NODE_BASECLASS_NAME = "Node";
    
    public static bool IsDerivedFromType(INamedTypeSymbol symbol, string typeName) {
        INamedTypeSymbol? currentBaseSymbol = symbol;
        while((currentBaseSymbol = currentBaseSymbol.BaseType) is not null) {
            if(currentBaseSymbol.Name == typeName) {
                return true;
            }
        }
        return false;
    }
    
    public static void Execute(GeneratorExecutionContext context) {
        
        // get all files with class declarations
        IEnumerable<SyntaxTree> files = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Any());
        
        foreach(SyntaxTree file in files) {
            
            SemanticModel semanticModel = context.Compilation.GetSemanticModel(file);
            
            foreach(ClassDeclarationSyntax classDeclarationSyntax in file.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()) {
                VisitClass(context, classDeclarationSyntax, semanticModel);
            }
            
        }
        
    }
    
    private static void VisitClass(GeneratorExecutionContext context, ClassDeclarationSyntax classDeclarationSyntax, SemanticModel semanticModel) {
        
        INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax)!;
        
        // semanticModel.GetSymbolInfo(classDeclarationSyntax).Symbol.
        
        // exclude classes not derived from node
        if(!classSymbol.IsDerivedFromType(NODE_BASECLASS_NAME))
            return;
        
        // exclude abstract classes
        // if(classSymbol.IsAbstract)
        //     continue;
        
        // warn to use partial keyword
        if(!classDeclarationSyntax.IsPartial()) {
            // these currently dont work on runtime, but when building solution
            // fix merged on feb 16th into dotnet:main of roslyn repo -> use dotnet 7 preview?
            Diagnostic diagnostic = Diagnostic.Create(new DiagnosticDescriptor("TEST01", "Title", "Message", "Category", DiagnosticSeverity.Error, true), classDeclarationSyntax.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
        
        List<ISymbol> hasNodes = new();
        List<ISymbol> arrNodes = new();
        
        if(classDeclarationSyntax.BaseList is null) // symbol gave back a BaseType of Node, but if the class declaration base list is null -> doubled partial class declaration
            return;
        //todo: ignore if its not the main partial class
        
        foreach(BaseTypeSyntax baseTypeSyntax in classDeclarationSyntax.BaseList.Types) { // use classSymbol.AllInterfaces
            
            if(baseTypeSyntax.Type is not GenericNameSyntax baseNameSyntax)
                continue;
            
            ISymbol? interfaceSymbol = semanticModel.GetSymbolInfo(baseTypeSyntax.Type).Symbol;
            
            if(interfaceSymbol is null)
                continue;
            if(interfaceSymbol.ContainingAssembly.Name != "GameEngine.Core")
                continue;
            if(interfaceSymbol.ContainingNamespace.ToDisplayString() != "GameEngine.Core.Nodes")
                continue;
            
            if(interfaceSymbol.Name == "Has") {
                
                var typeArgumentListSyntax = baseNameSyntax.TypeArgumentList.Arguments;
                
                if(typeArgumentListSyntax.Count != 1)
                    continue;
                
                ISymbol? symbol = semanticModel.GetSymbolInfo(typeArgumentListSyntax[0]).Symbol;
                
                if(symbol is not null) { // if its null, its invalid type
                    // string symbolName = symbol.Name; // "TestNode2"
                    // INamespaceSymbol symbolNamespace = symbol.ContainingNamespace; // {GameEngine.Core.Nodes}
                    hasNodes.Add(symbol);
                }
                
            } else if(interfaceSymbol.Name == "Arr") {
                
                var typeArgumentListSyntax = baseNameSyntax.TypeArgumentList.Arguments;
                
                if(typeArgumentListSyntax.Count != 1)
                    continue;
                
                ISymbol? symbol = semanticModel.GetSymbolInfo(typeArgumentListSyntax[0]).Symbol;
                
                if(symbol is not null) { // if its null, its invalid type
                    // string symbolName = symbol.Name; // "TestNode3"
                    // INamespaceSymbol symbolNamespace = symbol.ContainingNamespace; // {GameEngine.Core.Nodes}
                    // nullable?
                    arrNodes.Add(symbol);
                }
                
            }
            
        }
        
        GeneratePartialNode(classSymbol, hasNodes, arrNodes, context);
        
    }
    
    private static void GeneratePartialNode(INamedTypeSymbol nodeSymbol, List<ISymbol> hasNodes, List<ISymbol> arrNodes, GeneratorExecutionContext context) {
        
        StringBuilder propertiesSb = new();
        StringBuilder initializationSb = new();
        StringBuilder addToNodeListSb = new();
        
        foreach(ISymbol hasNode in hasNodes) {
            propertiesSb.Append("    [GameEngine.Core.Serialization.Serialized(GameEngine.Core.Serialization.Editor.Hidden)] public ");
            propertiesSb.Append(hasNode.ContainingNamespace.ToDisplayString());
            propertiesSb.Append('.');
            propertiesSb.Append(hasNode.Name);
            propertiesSb.Append(' ');
            propertiesSb.Append(hasNode.Name);
            propertiesSb.Append(" { get; init; } = null!;\n");
            
            initializationSb.Append("\n        ");
            initializationSb.Append(hasNode.Name);
            initializationSb.Append(" = new ");
            initializationSb.Append(hasNode.ContainingNamespace.ToDisplayString());
            initializationSb.Append('.');
            initializationSb.Append(hasNode.Name);
            initializationSb.Append("(this);");
            
            addToNodeListSb.Append("\n        childNodes.Add(");
            addToNodeListSb.Append(hasNode.Name);
            addToNodeListSb.Append(");");
        }
        
        foreach(ISymbol arrNode in arrNodes) {
            propertiesSb.Append("    [GameEngine.Core.Serialization.Serialized(GameEngine.Core.Serialization.Editor.Hidden)] public System.Collections.Generic.List<");
            propertiesSb.Append(arrNode.ContainingNamespace.ToDisplayString());
            propertiesSb.Append('.');
            propertiesSb.Append(arrNode.Name);
            propertiesSb.Append("> ");
            propertiesSb.Append(arrNode.Name);
            propertiesSb.Append("s { get; init; } = null!;\n");
            
            initializationSb.Append("\n        ");
            initializationSb.Append(arrNode.Name);
            initializationSb.Append("s = new System.Collections.Generic.List<");
            initializationSb.Append(arrNode.ContainingNamespace.ToDisplayString());
            initializationSb.Append('.');
            initializationSb.Append(arrNode.Name);
            initializationSb.Append(">();");
        }
        
        string properties = propertiesSb.ToString();
        string initialization = initializationSb.ToString();
        string addToComponentsList = addToNodeListSb.ToString();
        
        StringBuilder sourceBuilder = new();
        sourceBuilder.Append(
$@"#nullable enable

namespace {nodeSymbol.ContainingNamespace.ToDisplayString()};

public partial class {nodeSymbol.Name} {{
    
{properties}
    public {nodeSymbol.Name}(GameEngine.Core.Nodes.Node? parentNode = null) : this(parentNode, out System.Collections.Generic.List<GameEngine.Core.Nodes.Node> childNodes) {{
        _childNodes = childNodes;
    }}
    
    public {nodeSymbol.Name}(GameEngine.Core.Nodes.Node? parentNode, out System.Collections.Generic.List<GameEngine.Core.Nodes.Node> childNodes) : base(parentNode, out childNodes) {{{initialization}
{addToComponentsList}
    }}
    
    [Newtonsoft.Json.JsonConstructor]
    protected {nodeSymbol.Name}(bool isJsonConstructed) : base(isJsonConstructed) {{ }}
    
}}
"
        );
        context.AddSource($"{nodeSymbol.Name}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        
    }
    
}
