using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameEngine.SourceGenerator.Extensions {
    
    public static class ClassDeclarationSyntaxExtensions {
        
        public static bool HasAttribute(this ClassDeclarationSyntax source, string attributeName) {
            return source.AttributeLists.Any(x => x.Attributes.Any(y => y.Name.ToString().Contains(attributeName)));
        }
        
        public static bool IsPartial(this ClassDeclarationSyntax classDeclarationSyntax) {
            return classDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword);
        }
        
    }
    
}
