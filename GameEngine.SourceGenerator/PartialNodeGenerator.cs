using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using GameEngine.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameEngine.SourceGenerator; 

//todo: disallow any constructors on nodes
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
        
        foreach(AttributeSyntax attributeSyntax in classDeclarationSyntax.AttributeLists.SelectMany(attributeList => attributeList.Attributes)) {
            ISymbol? attributeSymbol = semanticModel.GetSymbolInfo(attributeSyntax).Symbol;
            
            if(attributeSymbol is null)
                continue;
            if(attributeSymbol.ContainingAssembly.Name != "GameEngine.Core")
                continue;
            if(attributeSymbol.ContainingNamespace.ToDisplayString() != "GameEngine.Core.Nodes")
                continue;
            
            INamedTypeSymbol? attributeTypeSymbol = attributeSymbol.ContainingType;
            
            ISymbol? _ = attributeTypeSymbol.OriginalDefinition;
            
            if(attributeTypeSymbol.Name == "Has") {
                
                ImmutableArray<ITypeSymbol> typeArguments = attributeTypeSymbol.TypeArguments;
                if(typeArguments.Length != 1)
                    continue;
                //TODO: filter invalid type
                ITypeSymbol firstType = typeArguments[0];
                hasNodes.Add(firstType);
                
            } else if(attributeTypeSymbol.Name == "Arr") {
                
                ImmutableArray<ITypeSymbol> typeArguments = attributeTypeSymbol.TypeArguments;
                if(typeArguments.Length != 1)
                    continue;
                //TODO: filter invalid type
                ITypeSymbol firstType = typeArguments[0];
                arrNodes.Add(firstType);
            }
            
        }
        
        GeneratePartialNode(classSymbol, hasNodes, arrNodes, context);
        
    }
    
    private static void GeneratePartialNode(ISymbol nodeSymbol, List<ISymbol> hasNodes, List<ISymbol> arrNodes, GeneratorExecutionContext context) {
        
        StringBuilder propertiesSb = new();
        
        foreach(ISymbol hasNode in hasNodes) {
            propertiesSb.Append("    [GameEngine.Core.Serialization.Serialized(GameEngine.Core.Serialization.Editor.Hierarchy)] public ");
            propertiesSb.Append(hasNode.ContainingNamespace.ToDisplayString());
            propertiesSb.Append('.');
            propertiesSb.Append(hasNode.Name);
            propertiesSb.Append(' ');
            propertiesSb.Append(hasNode.Name);
            propertiesSb.Append(" { get; init; } = null!;\n");
        }
        
        foreach(ISymbol arrNode in arrNodes) {
            propertiesSb.Append("    [GameEngine.Core.Serialization.Serialized(GameEngine.Core.Serialization.Editor.Hierarchy)] public GameEngine.Core.Nodes.NodeArr<");
            propertiesSb.Append(arrNode.ContainingNamespace.ToDisplayString());
            propertiesSb.Append('.');
            propertiesSb.Append(arrNode.Name);
            propertiesSb.Append("> ");
            propertiesSb.Append(arrNode.Name);
            propertiesSb.Append("s { get; init; } = null!;\n");
        }
        
        string properties = propertiesSb.ToString();
        
        StringBuilder sourceBuilder = new();
        sourceBuilder.Append(
            $@"#nullable enable

namespace {nodeSymbol.ContainingNamespace.ToDisplayString()};

public partial class {nodeSymbol.Name} {{
    
{properties}

}}
"
        );
        context.AddSource($"{nodeSymbol.Name}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        
    }
    
}
