using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameEngine.SourceGenerator.Extensions {
    
    public static class SyntaxTreeExtensions {
        
        public static BaseNamespaceDeclarationSyntax GetNamespace(this SyntaxTree file, ClassDeclarationSyntax classSyntax) {
            // first try if it is in a file scoped namespace
            FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclarationSyntax = file.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            if(fileScopedNamespaceDeclarationSyntax != null)
                return fileScopedNamespaceDeclarationSyntax;
            
            // the class may be in a normal scoped namespace
            SyntaxNode parent = classSyntax.Parent;
            while(parent.IsKind(SyntaxKind.ClassDeclaration)) {
                if(!(parent is ClassDeclarationSyntax parentClass)) return null;

                parent = parent.Parent;
            }
            return parent as NamespaceDeclarationSyntax;
        }
        
        public static IEnumerable<UsingDirectiveSyntax> GetUsingDirectives(this SyntaxTree file) {
            return file.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
        }
        
        public static string Format(this IEnumerable<UsingDirectiveSyntax> usingDirectives) {
            return string.Join("\r\n", usingDirectives);
        }
        
    }
    
}
