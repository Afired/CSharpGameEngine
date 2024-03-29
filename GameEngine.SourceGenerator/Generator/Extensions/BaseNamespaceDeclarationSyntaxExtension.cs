using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameEngine.SourceGenerator.Extensions {
    
    public static class BaseNamespaceDeclarationSyntaxExtension {
        
        public static string AsFileScopedNamespaceText(this BaseNamespaceDeclarationSyntax baseNamespaceDeclarationSyntax) {
            return baseNamespaceDeclarationSyntax is null ? null : $"namespace {baseNamespaceDeclarationSyntax.Name};";
        }
        
    }
    
}
